

/* ---------------------------------------------------------------------*
*                             Acumatica Inc.                            *

*              Copyright (c) 2005-2025 All rights reserved.             *

*                                                                       *

*                                                                       *

* This file and its contents are protected by United States and         *

* International copyright laws.  Unauthorized reproduction and/or       *

* distribution of all or any portion of the code contained herein       *

* is strictly prohibited and will result in severe civil and criminal   *

* penalties.  Any violations of this copyright will be prosecuted       *

* to the fullest extent possible under law.                             *

*                                                                       *

* UNDER NO CIRCUMSTANCES MAY THE SOURCE CODE BE USED IN WHOLE OR IN     *

* PART, AS THE BASIS FOR CREATING A PRODUCT THAT PROVIDES THE SAME, OR  *

* SUBSTANTIALLY THE SAME, FUNCTIONALITY AS ANY ACUMATICA PRODUCT.       *

*                                                                       *

* THIS COPYRIGHT NOTICE MAY NOT BE REMOVED FROM THIS FILE.              *

* --------------------------------------------------------------------- */

using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using PX.SM;
using PX.Common;
using PX.BarcodeProcessing;
using PX.Data;
using PX.Data.BQL;
using PX.Data.BQL.Fluent;
using PX.Objects.Common.Extensions;
using PX.Objects.CS;
using PX.Objects.IN;
using PX.Objects.IN.WMS;

namespace PX.Objects.SO.WMS
{
	using WMSBase = WarehouseManagementSystem;

	public partial class PickPackShip : WMSBase
	{
		public sealed class PackMode : ScanMode
		{
			public const string Value = "PACK";
			public class value : BqlString.Constant { public value() : base(PackMode.Value) { } }

			public override string Code => Value;
			public override string Description => Msg.Description;

			protected override bool IsModeActive() => Basis.Setup.Current.ShowPackTab == true;

			#region State Machine
			protected override IEnumerable> CreateStates()
			{
				yield return new ShipmentState();
				yield return new BoxState();
				yield return new InventoryItemState() { AlternateType = INPrimaryAlternateType.CPN, IsForIssue = true, IsForTransfer = Basis.IsTransfer, SuppressModuleItemStatusCheck = true };
				yield return new LotSerialState();
				yield return new ConfirmState();
				yield return new CommandOrShipmentOnlyState();

				yield return new BoxConfirming.StartState();
				yield return new BoxConfirming.WeightState();
				yield return new BoxConfirming.DimensionsState();
				yield return new BoxConfirming.CompleteState();

				if (!Basis.HasPick)
				{
					yield return new LocationState();
					yield return new ExpireDateState() { IsForIssue = true, IsForTransfer = Basis.IsTransfer };
				}
			}

			protected override IEnumerable> CreateTransitions()
			{
				var itemFlow = StateFlow(flow => flow
					.ForkBy(basis => basis.HasPick)
					.PositiveBranch(separatePicking => separatePicking
						.From()
						.NextTo()
						.NextTo()
						.NextTo())
					.NegativeBranch(packOnly => packOnly
						.From()
						.NextTo()
						.NextTo()
						.NextTo()
						.NextTo()
						.NextTo()));
				var boxFlow = StateFlow(flow => flow
					.From()
					.NextTo()
					.NextTo()
					.NextTo());

				return itemFlow.Concat(boxFlow);
			}

			protected override IEnumerable> CreateCommands()
			{
				yield return new RemoveCommand();
				yield return new QtySupport.SetQtyCommand();
				yield return new ConfirmPackageCommand();
				yield return new ConfirmShipmentCommand();
			}

			protected override IEnumerable> CreateQuestions()
			{
				yield return new BoxConfirming.WeightState.SkipQuestion();
				yield return new BoxConfirming.WeightState.SkipScalesQuestion();
				yield return new BoxConfirming.DimensionsState.SkipQuestion();
			}

			protected override IEnumerable> CreateRedirects() => AllWMSRedirects.CreateFor();

			protected override void ResetMode(bool fullReset)
			{
				base.ResetMode(fullReset);

				Clear(when: fullReset && !Basis.IsWithinReset);
				Clear(when: fullReset);
				Clear(when: fullReset);
				Clear();

				Clear();
				Clear();

				if (fullReset)
					Get().PackageLineNbrUI = null;

				if (!Basis.HasPick)
				{
					Clear(when: fullReset || Basis.PromptLocationForEveryLine);
					Clear();
				}
			}
			#endregion

			#region Logic
			public class Logic : ScanExtension
			{
				#region State
				public PackScanHeader PackHeader => Basis.Header.Get() ?? new PackScanHeader();
				public ValueSetter.Ext PackSetter => Basis.HeaderSetter.With();

				#region PackageLineNbr
				public int? PackageLineNbr
				{
					get => PackHeader.PackageLineNbr;
					set => PackSetter.Set(h => h.PackageLineNbr, value);
				}
				#endregion
				#region PackageLineNbrUI
				public int? PackageLineNbrUI
				{
					get => PackHeader.PackageLineNbrUI;
					set => PackSetter.Set(h => h.PackageLineNbrUI, value);
				}
				#endregion
				#endregion

				#region Views
				public
					SelectFrom.
					InnerJoin.On.
					OrderBy<
						SOShipLineSplit.shipmentNbr.Asc,
						SOShipLineSplit.isUnassigned.Desc,
						SOShipLineSplit.lineNbr.Asc>.
					View PickedForPack;
				protected virtual IEnumerable pickedForPack()
				{
					var delegateResult = new PXDelegateResult { IsResultSorted = true };
					delegateResult.AddRange(Basis.GetSplits(Basis.RefNbr, includeUnassigned: true, s => s.PackedQty >= s.Qty));
					return delegateResult;
				}

				public SelectFrom.View Packed;
				protected virtual IEnumerable packed()
				{
					return Basis.Header == null
						? Enumerable.Empty>() :
						from link in Graph.PackageDetailExt.PackageDetailSplit.SelectMain(Basis.RefNbr, PackageLineNbrUI)
						from split in PickedForPack.Select().Cast>()
						where
							((SOShipLineSplit)split).ShipmentNbr == link.ShipmentNbr &&
							((SOShipLineSplit)split).LineNbr == link.ShipmentLineNbr &&
							((SOShipLineSplit)split).SplitLineNbr == link.ShipmentSplitLineNbr
						select split;
				}

				public
					SelectFrom.
					Where<
						SOPackageDetailEx.shipmentNbr.IsEqual.
						And>>.
					View ShownPackage;
				#endregion

				#region Buttons
				public PXAction ReviewPack;
				[PXButton, PXUIField(DisplayName = "Review")]
				protected virtual IEnumerable reviewPack(PXAdapter adapter)
				{
					PackageLineNbrUI = null;
					return adapter.Get();
				}
				#endregion

				#region Event Handlers
				protected virtual void _(Events.RowSelected e)
				{
					if (e.Row == null)
						return;

					new[] {
						Base.Packages.Cache,
						Base.PackageDetailExt.PackageDetailSplit.Cache
					}
					.Modify(c => c.AllowInsert = c.AllowUpdate = c.AllowDelete = !Basis.DocumentIsConfirmed)
					.Consume();

					ReviewPack.SetVisible(Base.IsMobile && e.Row.Mode == PackMode.Value);
				}
				#endregion

				#region Decorations
				public virtual void InjectExpireDateForPackDeactivationOnAlreadyEnteredLot(ExpireDateState expireDateState)
				{
					expireDateState.Intercept.IsStateActive.ByConjoin(basis =>
						basis.SelectedLotSerialClass?.LotSerAssign == INLotSerAssign.WhenUsed &&
						basis.Get().PickedForPack.SelectMain().Any(t =>
							t.IsUnassigned == true ||
							t.LotSerialNbr == basis.LotSerialNbr && t.PackedQty == 0));
				}

				public virtual void InjectItemAbsenceHandlingByBox(InventoryItemState itemState)
				{
					itemState.Intercept.HandleAbsence.ByAppend((basis, barcode) =>
						basis.Get().TryAutoConfirmCurrentPackageAndLoadNext(barcode) == false
							? AbsenceHandling.Skipped
							: AbsenceHandling.Done);
				}

				public virtual void InjectItemPromptForPackageConfirm(InventoryItemState itemState)
				{
					itemState.Intercept.StatePrompt.ByOverride((basis, base_StatePrompt) =>
						basis.Get().With(mode =>
							basis.Remove != true && mode.CanConfirmPackage
								? basis.HasActive()
									? Msg.BoxConfirmOrContinuePromptNoPick
									: Msg.BoxConfirmOrContinuePrompt
								: null)
						?? base_StatePrompt());
				}
				#endregion

				#region Overrides
				/// Overrides 
				[PXOverride]
				public ScanState DecorateScanState(ScanState original,
					Func, ScanState> base_DecorateScanState)
				{
					var state = base_DecorateScanState(original);

					if (state.ModeCode == PackMode.Value)
					{
						PXSelectBase viewSelector(PickPackShip basis) => basis.Get().PickedForPack;

						if (state is LocationState locationState)
						{
							Basis.InjectLocationDeactivationOnDefaultLocationOption(locationState);
							Basis.InjectLocationSkippingOnPromptLocationForEveryLineOption(locationState);
							Basis.InjectLocationPresenceValidation(locationState, viewSelector);
						}
						else if (state is InventoryItemState itemState)
						{
							if (!Basis.HasPick)
								Basis.InjectItemAbsenceHandlingByLocation(itemState);

							InjectItemPromptForPackageConfirm(itemState);
							InjectItemAbsenceHandlingByBox(itemState);
							Basis.InjectItemPresenceValidation(itemState, viewSelector);
						}
						else if (state is LotSerialState lsState)
						{
							Basis.InjectLotSerialPresenceValidation(lsState, viewSelector);
							Basis.InjectLotSerialDeactivationOnDefaultLotSerialOption(lsState, isEntranceAllowed: !Basis.HasPick);
						}
						else if (state is ExpireDateState expireDateState)
						{
							InjectExpireDateForPackDeactivationOnAlreadyEnteredLot(expireDateState);
						}
					}

					return state;
				}

				/// Overrides 
				[PXOverride]
				public void OnBeforeFullClear(Action base_OnBeforeFullClear)
				{
					base_OnBeforeFullClear();

					if (Basis.CurrentMode is PackMode && Basis.RefNbr != null)
						if (Graph.WorkLogExt.SuspendFor(Basis.RefNbr, Graph.Accessinfo.UserID, Basis.HasPick ? SOShipmentProcessedByUser.jobType.Pack : SOShipmentProcessedByUser.jobType.PackOnly))
							Graph.WorkLogExt.PersistWorkLog();
				}

				[Obsolete]
				public virtual string GetCommandOrShipmentOnlyPrompt(Func base_GetCommandOrShipmentOnlyPrompt)
					=> base_GetCommandOrShipmentOnlyPrompt();

				// Acuminator disable once PX1016 ExtensionDoesNotDeclareIsActiveMethod extension should be constantly active
				public class AlterCommandOrShipmentOnlyStatePrompt : ScanExtension
				{
					/// Overrides 
					[PXOverride]
					public virtual string GetPromptForCommandOrShipmentOnly(Func base_GetPromptForCommandOrShipmentOnly)
					{
						if (Basis.CurrentMode is PackMode && Basis.Get() is Logic mode && mode.CanConfirmPackage)
							return PackMode.Msg.BoxConfirmPrompt;

						return base_GetPromptForCommandOrShipmentOnly();
					}
				}

                #endregion

                public virtual bool ShowPackTab(ScanHeader row) => Basis.HasPack && row.Mode == PackMode.Value;
				public virtual bool CanPack => Basis.HasPick
					? PickedForPack.SelectMain().Any(s => s.PackedQty < s.PickedQty)
					: PickedForPack.SelectMain().Any(s => s.PackedQty < s.Qty);

				public virtual bool CanConfirmPackage => Basis.RefNbr != null && HasConfirmableBoxes && !HasSingleAutoPackage(Basis.RefNbr, out var _) && PackageLineNbr != null && SelectedPackage != null && !IsPackageEmpty(SelectedPackage);

				public virtual bool IsPackageEmpty(SOPackageDetailEx package) => Graph.PackageDetailExt.PackageDetailSplit.Select(package.ShipmentNbr, package.LineNbr).Count == 0;

				public SOPackageDetailEx SelectedPackage => Graph.Packages.Search(PackageLineNbr);

				public virtual bool HasConfirmableBoxes => Graph.Packages.SelectMain().Any(p => p.Confirmed == false && !IsPackageEmpty(p));
				public virtual bool HasSingleAutoPackage(string shipmentNbr, out SOPackageDetailEx package)
				{
					if (PXAccess.FeatureInstalled())
					{
						var packages =
							SelectFrom.
							Where>.
							View.Select(Basis, shipmentNbr)
							.RowCast()
							.ToArray();
						if (packages.Length == 1 && packages[0].PackageType == SOPackageType.Auto)
						{
							package = packages[0];
							return true;
						}
						else if (packages.Any(p => p.PackageType == SOPackageType.Auto))
							throw new PXInvalidOperationException(Msg.CannotBePacked, shipmentNbr);
					}

					package = null;
					return false;
				}

				public virtual bool? TryAutoConfirmCurrentPackageAndLoadNext(string boxBarcode)
				{
					if (Basis.Remove == false)
					{
						CSBox box = CSBox.PK.Find(Basis, boxBarcode);
						if (box != null)
						{
							if (!Basis.Get().TryAutoConfirm())
								return null;

							if (Basis.TryProcessBy(boxBarcode, StateSubstitutionRule.KeepPositiveReports | StateSubstitutionRule.KeepApplication | StateSubstitutionRule.KeepStateChange))
								return true;
						}
					}

					return false;
				}
			}
			#endregion

			#region States
			public new sealed class ShipmentState : PickPackShip.ShipmentState
			{
				protected override Validation Validate(SOShipment shipment)
				{
					if (shipment.Operation != SOOperation.Issue)
						return Validation.Fail(Msg.InvalidOperation, shipment.ShipmentNbr, Basis.SightOf(shipment));

					if (shipment.Status != SOShipmentStatus.Open)
						return Validation.Fail(Msg.InvalidStatus, shipment.ShipmentNbr, Basis.SightOf(shipment));

					var splits = Basis.GetSplits(shipment.ShipmentNbr, includeUnassigned: true).RowCast().AsEnumerable();
					if (Basis.HasPick)
					{
						if (splits.All(s => s.PickedQty == 0))
							return Validation.Fail(Msg.ShouldBePickedFirst, shipment.ShipmentNbr);
					}

					if (Basis.HasNonStockLinesWithEmptyLocation(shipment, out Validation error))
						return error;

					Get().HasSingleAutoPackage(shipment.ShipmentNbr, out var _);

					return Validation.Ok;
				}

				protected override void ReportSuccess(SOShipment shipment) => Basis.ReportInfo(Msg.Ready, shipment.ShipmentNbr);

				protected override void SetNextState()
				{
					var mode = Basis.Get();
					if (Basis.Remove == true || mode.CanPack || mode.HasConfirmableBoxes)
						base.SetNextState();
					else
					{
						Basis.ReportInfo(BarcodeProcessing.CommonMessages.SentenceCombiner, Basis.Info.Current.Message,
							Basis.Localize(PackMode.Msg.Completed, Basis.RefNbr));
						Basis.SetScanState(BuiltinScanStates.Command);
					}
				}

				#region Messages
				[PXLocalizable]
				public new abstract class Msg : PickPackShip.ShipmentState.Msg
				{
					public new const string Ready = "The {0} shipment is loaded and ready to be packed.";
					public const string InvalidStatus = "The {0} shipment cannot be packed because it has the {1} status.";
					public const string InvalidOperation = "The {0} shipment cannot be packed because it has the {1} operation.";
					public const string ShouldBePickedFirst = "The {0} shipment cannot be packed because the items have not been picked.";
				}
				#endregion
			}

			public sealed class BoxState : EntityState
			{
				public const string Value = "BOX";
				public class value : BqlString.Constant { public value() : base(BoxState.Value) { } }

				public PackMode.Logic Mode => Get();

				public override string Code => Value;
				protected override string StatePrompt => Msg.Prompt;

				protected override bool IsStateSkippable() => base.IsStateSkippable() || Mode.PackageLineNbr != null;

				protected override void OnTakingOver()
				{
					if (Mode.HasSingleAutoPackage(Basis.RefNbr, out SOPackageDetailEx package)) // skip single auto package input
					{
						Mode.PackageLineNbr = package.LineNbr;
						Mode.PackageLineNbrUI = package.LineNbr;
						Basis.Graph.Packages.Current = package;
						MoveToNextState();
					}
				}

				protected override CSBox GetByBarcode(string barcode) => CSBox.PK.Find(Basis, barcode);

				protected override void Apply(CSBox box)
				{
					SOPackageDetailEx package = Basis.Graph.Packages.SelectMain().FirstOrDefault(p => string.Equals(p.BoxID.Trim(), box.BoxID.Trim(), StringComparison.OrdinalIgnoreCase) && p.Confirmed == false);
					if (package == null)
					{
						package = (SOPackageDetailEx)Basis.Graph.Packages.Cache.CreateInstance();
						package.BoxID = box.BoxID;
						package.ShipmentNbr = Basis.RefNbr;
						package = Basis.Graph.Packages.Insert(package);
						Basis.Save.Press();
					}

					Mode.PackageLineNbr = package.LineNbr;
					Mode.PackageLineNbrUI = package.LineNbr;
					Basis.Graph.Packages.Current = package;
				}

				protected override void ClearState()
				{
					Mode.PackageLineNbr = null;
					Basis.Graph.Packages.Current = null;
				}

				protected override void ReportSuccess(CSBox entity) => Basis.ReportInfo(Msg.Ready, entity.BoxID);
				protected override void ReportMissing(string barcode) => Basis.ReportError(Msg.Missing, barcode);

				protected override void SetNextState()
				{
					if (Basis.Remove != true && !Mode.CanPack)
					{
						Basis.SetScanState(BuiltinScanStates.Command);

						if (!Mode.CanConfirmPackage)
							Basis.ReportInfo(PackMode.Msg.Completed, Basis.RefNbr);
					}
					else
					{
						base.SetNextState();
					}
				}

				#region Messages
				[PXLocalizable]
				public abstract class Msg
				{
					public const string Prompt = "Scan the box barcode.";
					public const string Ready = "The {0} box is selected.";
					public const string Missing = "The {0} box is not found in the database.";
				}
				#endregion
			}

			public static class BoxConfirming
			{
				public sealed class StartState : MediatorState
				{
					public const string Value = "BCS";
					public class value : BqlString.Constant { public value() : base(StartState.Value) { } }

					public override string Code => Value;

					protected override bool IsStateActive() => Get().CanConfirmPackage;

					protected override void Apply()
					{
						Basis.Clear();
						Basis.Clear();
					}

					protected override void SetNextState()
					{
						if (Get().HasSingleAutoPackage(Basis.RefNbr, out var _)) // package is already fully prepared
						{
							Basis.ReportInfo(BarcodeProcessing.CommonMessages.SentenceCombiner, Basis.Info.Current.Message,
								Basis.Localize(PackMode.Msg.Completed, Basis.RefNbr));
							Basis.SetScanState(BuiltinScanStates.Command);
						}
						else
							base.SetNextState();
					}
				}

				public sealed class WeightState : EntityState
				{
					public const string Value = "BWGT";
					public class value : BqlString.Constant { public value() : base(WeightState.Value) { } }

					public Logic This => Get();
					public PackMode.Logic Mode => Get();

					#region Flow
					public override string Code => Value;
					protected override string StatePrompt => Msg.Prompt;

					protected override void OnTakingOver()
					{
						if (This.TryPrepareWeightAndSkipInputFor(Mode.SelectedPackage))
							MoveToNextState();
					}

					protected override void OnDismissing() => Basis.RevokeQuestion();

					protected override decimal? GetByBarcode(string barcode) => decimal.TryParse(barcode, out decimal value) ? value : (decimal?)null;

					protected override void ReportMissing(string barcode) => Basis.ReportError(Msg.BadFormat);

					protected override Validation Validate(decimal? value)
					{
						if (Basis.HasFault(value, base.Validate, out var fault))
							return fault;

						var package = Mode.SelectedPackage;

						if (Basis.IsValid(package, value.Value, out string error) == false)
							return Validation.Fail(error);

						return Validation.Ok;
					}

					protected override void Apply(decimal? value) => This.Weight = value.Value;

					protected override void ClearState() => (This.Weight, This.LastWeighingTime) = (null, null);

					protected override void ReportSuccess(decimal? value) => Basis.ReportInfo(Msg.Success, value.Value, Mode.SelectedPackage.WeightUOM);
					#endregion

					#region SkipQuestion
					public sealed class SkipQuestion : ScanQuestion
					{
						public Logic State => Get();

						public override string Code => "SKIPWEIGHT";

						protected override string GetPrompt() => Msg.Prompt;

						protected override void Confirm()
						{
							if (State.TryUsePreparedWeightFor(State.Target.SelectedPackage, explicitConfirmation: true) && Basis.CurrentState is WeightState)
								Basis.DispatchNext();
						}

						protected override void Reject() { }

						#region Messages
						[PXLocalizable]
						public abstract class Msg
						{
							public const string Prompt = "To skip the weighing, click OK.";
						}
						#endregion
					}
					#endregion

					#region SkipScalesQuestion
					public sealed class SkipScalesQuestion : ScanQuestion
					{
						public Logic State => Get();

						public override string Code => "SKIPSCALES";

						protected override string GetPrompt() => Msg.Prompt;

						protected override void Confirm()
						{
							var package = State.Target.SelectedPackage;

							if (State.SelectedScales.LastModifiedDateTime == State.LastWeighingTime)
							{
								// user wants not to use scale
								if (State.TryUsePreparedWeightFor(package) && Basis.CurrentState is WeightState)
									Basis.DispatchNext();
							}
							else
							{
								// user tried to use scales again
								if (State.ProcessScales(package) && Basis.CurrentState is WeightState)
									Basis.DispatchNext();
							}
						}

						protected override void Reject() { }

						#region Messages
						[PXLocalizable]
						public abstract class Msg
						{
							public const string Prompt = "Put the package on the scale and click OK. To skip the weighing, click OK without using the scale.";
						}
						#endregion
					}
					#endregion

					#region Logic
					// Acuminator disable once PX1016 ExtensionDoesNotDeclareIsActiveMethod extension should be constantly active
					public class Logic : ScanExtension
					{
						public PXSetupOptional CommonSetupUOM;

						public virtual double ScaleWeightValiditySeconds => 30;

						public SMScale SelectedScales
						{
							get
							{
								Guid? scaleDeviceID = UserSetup.For(Graph).ScaleDeviceID;
								Graph.Caches().ClearQueryCache();
								return SMScale.PK.Find(Basis, scaleDeviceID);
							}
						}

						#region Weight
						public decimal? Weight
						{
							get => Target.PackHeader.Weight;
							set => Target.PackSetter.Set(h => h.Weight, value);
						}
						#endregion
						#region LastWeighingTime
						public DateTime? LastWeighingTime
						{
							get => Target.PackHeader.LastWeighingTime;
							set => Target.PackSetter.Set(h => h.LastWeighingTime, value);
						}
						#endregion

						public virtual bool TryUsePreparedWeightFor(SOPackageDetailEx package, bool explicitConfirmation = false)
						{
							if (!explicitConfirmation && Basis.Setup.Current.ConfirmEachPackageWeight == true)
								return false; // explicit user input required

							if (!CanSkipInputFor(package))
							{
								Basis.ReportError(Msg.NoSkip);
								Basis.RevokeQuestion();
								Basis.RevokeQuestion();
								return false;
							}
							else if (Weight != null && Basis.HasFault(Weight, w => Basis.TryValidate(w).By(), out var fault))
							{
								Basis.ReportError(fault.Message, fault.MessageArgs);
								Basis.RevokeQuestion();
								Basis.RevokeQuestion();
								return false;
							}
							else
							{
								return true;
							}
						}

						public virtual bool TryPrepareWeightAndSkipInputFor(SOPackageDetailEx package)
						{
							Weight = package.Weight == 0
								? AutoCalculateBoxWeightBasedOnItems(package)
								: package.Weight.Value;

							if (UserSetup.For(Basis).UseScale == true && !ProcessScales(package))
								return false;

							Basis.ReportInfo(Msg.CalculatedWeight, package.BoxID, Weight, package.WeightUOM);

							if (Basis.Setup.Current.ConfirmEachPackageWeight == true)
							{
								if (CanSkipInputFor(package))
									AskToSkipFor(package);

								return false;
							}

							return CanSkipInputFor(package);
						}

						protected virtual bool CanSkipInputFor(SOPackageDetailEx package) => Weight.IsNotIn(null, 0) || package.Weight.IsNotIn(null, 0);

						protected virtual void AskToSkipFor(SOPackageDetailEx package)
						{
							if (Basis.HasFault(Weight, w => Basis.TryValidate(w).By(), out var _))
								Basis.Warn(Msg.CalculatedWeight, package.BoxID, Weight, package.WeightUOM);
							else
								Basis.Ask(Msg.CalculatedWeight, package.BoxID, Weight, package.WeightUOM);
						}

						protected virtual decimal AutoCalculateBoxWeightBasedOnItems(SOPackageDetailEx package)
						{
							decimal calculatedWeight = CSBox.PK.Find(Basis, package.BoxID)?.BoxWeight ?? 0m;
							SOShipLineSplitPackage[] links = Graph.PackageDetailExt.PackageDetailSplit.SelectMain(package.ShipmentNbr, package.LineNbr);
							foreach (var link in links)
							{
								var inventory = InventoryItem.PK.Find(Basis, link.InventoryID);
								calculatedWeight += (inventory.BaseWeight ?? 0) * (link.BasePackedQty ?? 0);
							}

							return Math.Round(calculatedWeight, 4);
						}

						public virtual bool ProcessScales(SOPackageDetailEx package)
						{
							SMScale scale = SelectedScales;

							if (scale == null)
							{
								Basis.ReportError(Msg.ScaleMissing, "");
								return false;
							}

							DateTime dbNow = GetServerTime();
							LastWeighingTime = scale.LastModifiedDateTime.Value;

							if (scale.LastModifiedDateTime.Value.AddHours(1) < dbNow)
							{
								Basis.ReportError(Msg.ScaleDisconnected, scale.ScaleID);
								return false;
							}

							if (scale.LastWeight.GetValueOrDefault() == 0)
							{
								Basis.Warn(Msg.ScaleNoBox, scale.ScaleID);
								return false;
							}

							if (scale.LastModifiedDateTime.Value.AddSeconds(ScaleWeightValiditySeconds) < dbNow)
							{
								Basis.ReportError(Msg.ScaleTimeout, scale.ScaleID, ScaleWeightValiditySeconds);
								return false;
							}

							var scaleConversion = scale.GetExtension();
							if (scaleConversion?.CompanyUOM == null)
							{
								Basis.ReportError(IN.Messages.BaseCompanyUomIsNotDefined);
								return false;
							}

							if (scaleConversion?.CompanyLastWeight == null)
							{
								Basis.ReportError(IN.Messages.MissingGlobalUnitConversion, scale.UOM, scaleConversion.CompanyUOM);
								return false;
							}

							Weight = scaleConversion.CompanyLastWeight ?? 0;
							return true;
						}

						protected virtual DateTime GetServerTime()
						{
							PXDatabase.SelectDate(out var _, out DateTime dbNow);
							dbNow = PXTimeZoneInfo.ConvertTimeFromUtc(dbNow, LocaleInfo.GetTimeZone());
							return dbNow;
						}

						/// Overrides 
						[PXOverride]
						public ScanCommand DecorateScanCommand(ScanCommand original,
							Func, ScanCommand> base_DecorateScanCommand)
						{
							var command = base_DecorateScanCommand(original);

							if (command is ConfirmPackageCommand confirmPackage)
								confirmPackage.Intercept.IsEnabled.ByConjoin(basis => !(basis.CurrentState is WeightState));

							return command;
						}
					}
					#endregion

					#region AlterComplete
					// Acuminator disable once PX1016 ExtensionDoesNotDeclareIsActiveMethod extension should be constantly active
					public class AlterComplete : ScanExtension
					{
						/// Overrides 
						[PXOverride]
						public void ApplyChanges(SOPackageDetailEx package,
							Action base_ApplyChanges)
						{
							base_ApplyChanges(package);

							var state = Basis.Get();

							if (state.Weight.IsNotIn(null, 0m))
								package.Weight = Math.Round(state.Weight.Value, 4);
						}

						/// Overrides 
						[PXOverride]
						public bool TryForwardProcessing(Func base_TryForwardProcessing)
						{
							if (Basis.CurrentState is WeightState)
								if (TryForward() == false)
									return false;

							if (base_TryForwardProcessing() == false)
								return false;

							if (Basis.CurrentState is WeightState)
								if (TryForward() == false)
									return false;

							return true;
						}

						/// Overrides 
						[PXOverride]
						public void ClearStates(Action base_ClearStates)
						{
							base_ClearStates();
							Basis.Clear();
						}

						protected virtual bool TryForward()
						{
							var state = Basis.Get();

							if (state.TryUsePreparedWeightFor(state.Target.SelectedPackage) == false)
								return false;

							Basis.DispatchNext();
							return true;
						}
					}
					#endregion

					#region Messages
					[PXLocalizable]
					public abstract class Msg
					{
						public const string Prompt = "Enter the actual total weight of the package.";
						public const string BadFormat = "The quantity format does not fit the locale settings.";
						public const string Success = "Once the package is confirmed, it will have the following weight: {0} {1}.";

						public const string CalculatedWeight = "The {0} package is ready to be confirmed. The calculated weight is {1} {2}.";
						public const string NoSkip = "The package does not have a predefined weight.";

						public const string ScaleMissing = "The {0} scale is not found in the database.";
						public const string ScaleDisconnected = "No information from the {0} scales. Check connection of the scales.";
						public const string ScaleTimeout = "Measurement on the {0} scale is more than {1} seconds old. Remove the package from the scale and weigh it again.";
						public const string ScaleNoBox = "No information from the {0} scales. Make sure that items are on the scales.";
					}
					#endregion
				}

				public sealed class DimensionsState : EntityState<(decimal L, decimal W, decimal H)?>
				{
					public const string Value = "BDIM";
					public class value : BqlString.Constant { public value() : base(DimensionsState.Value) { } }

					public Logic This => Get();
					public PackMode.Logic Mode => Get();

					#region Flow
					public override string Code => Value;
					protected override string StatePrompt => Msg.Prompt;

					protected override bool IsStateSkippable() =>
						Basis.Setup.Current.ConfirmEachPackageDimensions == false ||
						Mode.SelectedPackage == null ||
						CSBox.PK.Find(Basis, Mode.SelectedPackage.BoxID)?.AllowOverrideDimension != true;

					protected override void OnTakingOver()
					{
						if (This.TryPrepareDimensionsAndSkipInputFor(Mode.SelectedPackage))
							MoveToNextState();
					}
					protected override void OnDismissing() => Basis.RevokeQuestion();

					protected override (decimal L, decimal W, decimal H)? GetByBarcode(string barcode)
					{
						string[] dimensions = barcode.Trim().Split(' ');

						if (dimensions.Length < 3)
							return null;

						(string lengthStr, string widthStr, string heightStr) = dimensions;

						if (decimal.TryParse(lengthStr, out decimal lenght) &&
							decimal.TryParse(widthStr, out decimal width) &&
							decimal.TryParse(heightStr, out decimal height))
							return (lenght, width, height);

						return null;
					}

					protected override void ReportMissing(string barcode) => Basis.ReportError(Msg.BadFormat);

					protected override Validation Validate((decimal L, decimal W, decimal H)? value)
					{
						if (Basis.HasFault(value, base.Validate, out var fault))
							return fault;

						var package = Mode.SelectedPackage;

						if (Basis.IsValid(package, value.Value.L, out string errorLength) == false)
							return Validation.Fail(errorLength);

						if (Basis.IsValid(package, value.Value.W, out string errorWidth) == false)
							return Validation.Fail(errorWidth);

						if (Basis.IsValid(package, value.Value.H, out string errorHeight) == false)
							return Validation.Fail(errorHeight);

						return Validation.Ok;
					}

					protected override void Apply((decimal L, decimal W, decimal H)? value) => This.Dimensions = value.Value;

					protected override void ClearState() => This.Dimensions = null;

					protected override void ReportSuccess((decimal L, decimal W, decimal H)? value)
						=> Basis.ReportInfo(Msg.Success, value.Value.L, value.Value.W, value.Value.H, Mode.SelectedPackage.LinearUOM);
					#endregion

					#region SkipQuestion
					public sealed class SkipQuestion : ScanQuestion
					{
						public Logic State => Get();

						public override string Code => "SKIPDIMENSIONS";

						protected override string GetPrompt() => Msg.Prompt;

						protected override void Confirm()
						{
							if (State.TryUsePreparedDimensionsFor(State.Target.SelectedPackage, explicitConfirmation: true) && Basis.CurrentState is DimensionsState)
								Basis.DispatchNext();
						}

						protected override void Reject() { }

						#region Messages
						[PXLocalizable]
						public abstract class Msg
						{
							public const string Prompt = "To use the default dimensions, click OK.";
						}
						#endregion
					}
					#endregion

					#region Logic
					// Acuminator disable once PX1016 ExtensionDoesNotDeclareIsActiveMethod extension should be constantly active
					public class Logic : ScanExtension
					{
						#region Dimensions
						public (decimal L, decimal W, decimal H)? Dimensions
						{
							get
							{
								if (((decimal?)null).IsIn(Target.PackHeader.Length, Target.PackHeader.Width, Target.PackHeader.Height))
									return null;

								return
								(
									Target.PackHeader.Length.Value,
									Target.PackHeader.Width.Value,
									Target.PackHeader.Height.Value
								);
							}
							set
							{
								if (value == null)
								{
									Target.PackSetter.Set(h => h.Length, null);
									Target.PackSetter.Set(h => h.Width, null);
									Target.PackSetter.Set(h => h.Height, null);
								}
								else
								{
									Target.PackSetter.Set(h => h.Length, value.Value.L);
									Target.PackSetter.Set(h => h.Width, value.Value.W);
									Target.PackSetter.Set(h => h.Height, value.Value.H);
								}
							}
						}
						#endregion

						public virtual bool TryUsePreparedDimensionsFor(SOPackageDetailEx package, bool explicitConfirmation = false)
						{
							if (!explicitConfirmation && Basis.Setup.Current.ConfirmEachPackageDimensions == true)
								return false; // explicit user input required

							if (!CanSkipInputFor(package))
							{
								Basis.ReportError(Msg.NoSkip);
								Basis.RevokeQuestion();
								return false;
							}
							else if (Dimensions != null && Basis.HasFault(Dimensions, dims => Basis.TryValidate(dims).By(), out var fault))
							{
								Basis.ReportError(fault.Message, fault.MessageArgs);
								Basis.RevokeQuestion();
								return false;
							}
							else
							{
								return true;
							}
						}

						public virtual bool TryPrepareDimensionsAndSkipInputFor(SOPackageDetailEx package)
						{
							Dimensions = (package.Length ?? 0, package.Width ?? 0, package.Height ?? 0);

							Basis.ReportInfo(Msg.CalculatedDimensions, package.BoxID, Dimensions.Value.L, Dimensions.Value.W, Dimensions.Value.H, package.LinearUOM);

							if (Basis.Setup.Current.ConfirmEachPackageDimensions == true)
							{
								if (CanSkipInputFor(package))
									AskToSkipFor(package);

								return false;
							}

							return CanSkipInputFor(package);
						}

						protected virtual bool CanSkipInputFor(SOPackageDetailEx package) =>
							Dimensions.IsNotIn(null, (0m, 0m, 0m)) ||
							package.Length.IsNotIn(null, 0) &&
							package.Width.IsNotIn(null, 0) &&
							package.Height.IsNotIn(null, 0);

						protected virtual void AskToSkipFor(SOPackageDetailEx package)
						{
							if (Basis.HasFault(Dimensions, dims => Basis.TryValidate(dims).By(), out var _))
								Basis.Warn(Msg.CalculatedDimensions, package.BoxID, Dimensions.Value.L, Dimensions.Value.W, Dimensions.Value.H, package.LinearUOM);
							else
								Basis.Ask(Msg.CalculatedDimensions, package.BoxID, Dimensions.Value.L, Dimensions.Value.W, Dimensions.Value.H, package.LinearUOM);
						}

						/// Overrides 
						[PXOverride]
						public ScanCommand DecorateScanCommand(ScanCommand original,
							Func, ScanCommand> base_DecorateScanCommand)
						{
							var command = base_DecorateScanCommand(original);

							if (command is ConfirmPackageCommand confirmPackage)
								confirmPackage.Intercept.IsEnabled.ByConjoin(basis => !(basis.CurrentState is DimensionsState));

							return command;
						}
					}

					// Acuminator disable once PX1016 ExtensionDoesNotDeclareIsActiveMethod extension should be constantly active
					[PXUIField(DisplayName = Msg.PackageDimensionsCombined)]
					public class PackageDimensionsCombined : FieldAttached.To.AsString.Named
					{
						public override string GetValue(SOPackageDetailEx Row) => $"{Row.Length} x {Row.Width} x {Row.Height} {Row.LinearUOM}";
					}
					#endregion

					#region AlterComplete
					// Acuminator disable once PX1016 ExtensionDoesNotDeclareIsActiveMethod extension should be constantly active
					public class AlterComplete : ScanExtension
					{
						/// Overrides 
						[PXOverride]
						public void ApplyChanges(SOPackageDetailEx package,
							Action base_ApplyChanges)
						{
							base_ApplyChanges(package);

							var state = Basis.Get();

							if (state.Dimensions.IsNotIn(null, (0m, 0m, 0m)))
							{
								package.Length = Math.Round(state.Dimensions.Value.L, 4);
								package.Width = Math.Round(state.Dimensions.Value.W, 4);
								package.Height = Math.Round(state.Dimensions.Value.H, 4);
							}
						}

						/// Overrides 
						[PXOverride]
						public bool TryForwardProcessing(Func base_TryForwardProcessing)
						{
							if (Basis.CurrentState is DimensionsState)
								if (TryForward() == false)
									return false;

							if (base_TryForwardProcessing() == false)
								return false;

							if (Basis.CurrentState is DimensionsState)
								if (TryForward() == false)
									return false;

							return true;
						}

						/// Overrides 
						[PXOverride]
						public void ClearStates(Action base_ClearStates)
						{
							base_ClearStates();
							Basis.Clear();
						}

						protected virtual bool TryForward()
						{
							var state = Basis.Get();

							if (state.TryUsePreparedDimensionsFor(state.Target.SelectedPackage) == false)
								return false;

							Basis.DispatchNext();
							return true;
						}
					}
					#endregion

					#region Messages
					[PXLocalizable]
					public abstract class Msg
					{
						public const string Prompt = "Enter the actual length, width, and height of the package. Use a space as a separator.";
						public const string BadFormat = "The entered data has an incorrect format. The data input should contain three numeric dimensions separated by a space. Example: 31.2 20 13.5";
						public const string Success = "Once the package is confirmed, it will have the following dimensions: {0} x {1} x {2} {3}.";
						public const string NoSkip = "The package does not have predefined dimensions.";
						public const string CalculatedDimensions = "The {0} package is ready to be confirmed. It has the following default dimensions: {1} x {2} x {3} {4}.";
						public const string PackageDimensionsCombined = "Dimensions (L x W x H)";
					}
					#endregion
				}

				public sealed class CompleteState : MediatorState
				{
					public const string Value = "BCC";
					public class value : BqlString.Constant { public value() : base(CompleteState.Value) { } }

					public override string Code => Value;

					protected override void Apply() => Get().Call(state => state.SettleAndConfirmPackage(state.Target.SelectedPackage));

					protected override void SetNextState() => Basis.SetDefaultState();

					#region Logic
					// Acuminator disable once PX1016 ExtensionDoesNotDeclareIsActiveMethod extension should be constantly active
					public class Logic : ScanExtension
					{
						public virtual bool TryAutoConfirm()
						{
							if (!Basis.HasActive())
								return true;

							Basis.SetScanState();

							if (Target.SelectedPackage.Confirmed == true)
								return true;

							if (TryForwardProcessing() == false)
								return false;

							SettleAndConfirmPackage(Target.SelectedPackage);

							return true;
						}

						public virtual void SettleAndConfirmPackage(SOPackageDetailEx package)
						{
							ApplyChanges(package);

							package.Confirmed = true;
							Graph.Packages.Update(package);
							Basis.Save.Press();

							ClearStates();
							Basis.Reset(fullReset: false);
							Basis.ReportInfo(Msg.Success);
						}

						protected virtual bool TryForwardProcessing() => true;
						protected virtual void ApplyChanges(SOPackageDetailEx package) { }
						protected virtual void ClearStates() => Basis.Clear();
					}
					#endregion

					#region Messages
					[PXLocalizable]
					public abstract class Msg
					{
						public const string Success = "The package has been confirmed.";
					}
					#endregion
				}
			}

			public sealed class ConfirmState : ConfirmationState
			{
				public sealed override string Prompt => Basis.Localize(Msg.Prompt, Basis.SightOf(), Basis.Qty, Basis.UOM);

				protected sealed override FlowStatus PerformConfirmation() => Get().Confirm();

				#region Logic
				public class Logic : ScanExtension
				{
					protected PackMode.Logic Mode { get; private set; }
					public override void Initialize() => Mode = Basis.Get();

					public virtual FlowStatus Confirm()
					{
						var nothingToDoError = FlowStatus.Fail(Basis.Remove == true ? Msg.NothingToRemove : Msg.NothingToPack);

						if (Mode.PackageLineNbr == null)
							return nothingToDoError;

						var packageDetail = Mode.SelectedPackage;

						if (Basis.InventoryID == null || Basis.Qty == 0)
							return nothingToDoError;

						void KeepPackageSelection() => Mode.PackageLineNbr = packageDetail.LineNbr;

						var packedSplits = GetSplitsToPack();
						if (packedSplits.Any() == false)
							return nothingToDoError.WithModeReset.WithPostAction(KeepPackageSelection);

						decimal qty = Basis.BaseQty;
						string inventoryCD = Basis.SightOf();

						if (Basis.Remove == true ? packedSplits.Sum(s => s.PackedQty) - qty < 0 : packedSplits.Sum(s => TargetQty(s) - s.PackedQty) < qty)
							return FlowStatus.Fail(Basis.Remove == true ? Msg.BoxCanNotUnpack : Msg.BoxCanNotPack, inventoryCD, Basis.Qty, Basis.UOM);

						decimal unassignedQty = Sign.MinusIf(Basis.Remove == true) * qty;
						foreach (var packedSplit in packedSplits)
						{
							decimal currentQty = Basis.Remove == true
								? -Math.Min(packedSplit.PackedQty.Value, -unassignedQty)
								: +Math.Min(TargetQty(packedSplit).Value - packedSplit.PackedQty.Value, unassignedQty);

							if (PackSplit(packedSplit, packageDetail, currentQty) == false)
								return FlowStatus.Fail(Basis.Remove == true ? Msg.BoxCanNotUnpack : Msg.BoxCanNotPack, inventoryCD, Basis.Qty, Basis.UOM);

							unassignedQty -= currentQty;
							if (unassignedQty == 0)
								break;
						}

						if (Mode.IsPackageEmpty(packageDetail))
						{
							Basis.Graph.Packages.Delete(packageDetail);
							Basis.Clear();
							Mode.PackageLineNbrUI = null;
						}
						else
						{
							Mode.PackageLineNbrUI = Mode.PackageLineNbr;
						}

						EnsureShipmentUserLinkForPack();

						Basis.ReportInfo(
							Basis.Remove == true ? Msg.InventoryRemoved : Msg.InventoryAdded,
							Basis.SightOf(), Basis.Qty, Basis.UOM);

						return FlowStatus.Ok.WithDispatchNext;
					}

					public virtual decimal? TargetQty(SOShipLineSplit split) => Basis.HasPick ? split.PickedQty : split.Qty * Basis.Graph.GetQtyThreshold(split);

					protected virtual bool IsSelectedSplit(SOShipLineSplit split)
					{
						return
							split.InventoryID == Basis.InventoryID &&
							split.SubItemID == Basis.SubItemID &&
							split.SiteID == Basis.SiteID &&
							split.LocationID == (Basis.LocationID ?? split.LocationID) &&
							string.Equals(split.LotSerialNbr, Basis.LotSerialNbr ?? split.LotSerialNbr, StringComparison.OrdinalIgnoreCase);
					}

					public virtual IEnumerable GetSplitsToPack()
					{
						bool locationIsRequired = Basis.HasActive();
						return
							Mode.PickedForPack.SelectMain()
							.Where(r =>
								r.InventoryID == Basis.InventoryID &&
								r.SubItemID == Basis.SubItemID &&
								(r.IsUnassigned == true || r.HasGeneratedLotSerialNbr == true || string.Equals(r.LotSerialNbr, Basis.LotSerialNbr ?? r.LotSerialNbr, StringComparison.OrdinalIgnoreCase)) &&
								locationIsRequired.Implies(r.LocationID == (Basis.LocationID ?? r.LocationID)) &&
								(Basis.Remove == true ? r.PackedQty > 0 : TargetQty(r) > r.PackedQty))
							.With(PrioritizeSplits);
					}

					public virtual IOrderedEnumerable PrioritizeSplits(IEnumerable splits)
					{
						if (Basis.HasPick == false && Basis.Shipment?.PickedViaWorksheet == false)
						{
							return splits
								.OrderByAccordanceTo(split => split.IsUnassigned == false && split.HasGeneratedLotSerialNbr == false)
								.ThenByAccordanceTo(split => Basis.Remove == true ? split.PickedQty > 0 : split.Qty > split.PickedQty)
								.ThenByAccordanceTo(split => string.Equals(split.LotSerialNbr, Basis.LotSerialNbr ?? split.LotSerialNbr, StringComparison.OrdinalIgnoreCase))
								.ThenByAccordanceTo(split => string.IsNullOrEmpty(split.LotSerialNbr))
								.ThenByAccordanceTo(split => (split.Qty > split.PickedQty || Basis.Remove == true) && split.PickedQty > 0)
								.ThenByDescending(split => Sign.MinusIf(Basis.Remove == true) * (split.Qty - split.PickedQty));
						}
						else
						{
							return splits.OrderBy(split => 0); // no sorting
						}
					}

					public virtual bool PackSplit(SOShipLineSplit split, SOPackageDetailEx package, decimal qty)
					{
						if (Basis.HasPick)
							Basis.EnsureAssignedSplitEditing(split);
						else if (split.IsUnassigned == true)
						{
							var existingSplit = Mode.PickedForPack.SelectMain().FirstOrDefault(s =>
								s.LineNbr == split.LineNbr &&
								s.IsUnassigned == false &&
								string.Equals(s.LotSerialNbr, Basis.LotSerialNbr ?? s.LotSerialNbr, StringComparison.OrdinalIgnoreCase) &&
								IsSelectedSplit(s));

							if (existingSplit == null)
							{
								var newSplit = PXCache.CreateCopy(split);
								newSplit.SplitLineNbr = null;
								newSplit.LotSerialNbr = Basis.LotSerialNbr;
								newSplit.ExpireDate = Basis.ExpireDate;
								newSplit.Qty = qty;
								newSplit.PickedQty = qty;
								newSplit.PackedQty = 0;
								newSplit.IsUnassigned = false;
								newSplit.PlanID = null;

								split = Mode.PickedForPack.Insert(newSplit);
							}
							else
							{
								existingSplit.Qty += qty;
								existingSplit.ExpireDate = Basis.ExpireDate;
								split = Mode.PickedForPack.Update(existingSplit);
							}
						}
						else if (split.HasGeneratedLotSerialNbr == true)
						{
							var donorSplit = PXCache.CreateCopy(split);
							if (donorSplit.Qty == qty)
							{
								Mode.PickedForPack.Delete(donorSplit);
							}
							else
							{
								donorSplit.Qty -= qty;
								donorSplit.PickedQty -= Math.Min(qty, donorSplit.PickedQty.Value);
								Mode.PickedForPack.Update(donorSplit);
							}

							var existingSplit = Mode.PickedForPack.SelectMain().FirstOrDefault(s =>
								s.LineNbr == split.LineNbr &&
								s.HasGeneratedLotSerialNbr == false &&
								string.Equals(s.LotSerialNbr, Basis.LotSerialNbr ?? s.LotSerialNbr, StringComparison.OrdinalIgnoreCase) &&
								IsSelectedSplit(s));

							if (existingSplit == null)
							{
								var newSplit = PXCache.CreateCopy(split);
								newSplit.SplitLineNbr = null;
								newSplit.LotSerialNbr = Basis.LotSerialNbr;
								if (Basis.ExpireDate != null)
									newSplit.ExpireDate = Basis.ExpireDate;
								newSplit.Qty = qty;
								newSplit.PickedQty = qty;
								newSplit.PackedQty = 0;
								newSplit.PlanID = null;

								split = Mode.PickedForPack.Insert(newSplit);

								split.HasGeneratedLotSerialNbr = false;
								split = Mode.PickedForPack.Update(split);
							}
							else
							{
								existingSplit.Qty += qty;
								existingSplit.PickedQty += qty;
								if (Basis.ExpireDate != null)
									existingSplit.ExpireDate = Basis.ExpireDate;
								split = Mode.PickedForPack.Update(existingSplit);
							}
						}

						SOShipLineSplitPackage link = Graph.PackageDetailExt.PackageDetailSplit
							.SelectMain(package.ShipmentNbr, package.LineNbr)
							.FirstOrDefault(t =>
								t.ShipmentNbr == split.ShipmentNbr
								&& t.ShipmentLineNbr == split.LineNbr
								&& t.ShipmentSplitLineNbr == split.SplitLineNbr);

						if (qty < 0)
						{
							if (link == null || link.PackedQty + qty < 0)
								return false;

							if (!Basis.HasPick && Basis.LotSerialTrack.IsEnterable)
							{
								if (Basis.SelectedLotSerialClass.AutoNextNbr == true)
								{
									if (split.PackedQty + qty == 0)
									{
										split.HasGeneratedLotSerialNbr = true;
										split = Mode.PickedForPack.Update(split);
									}
								}
								else
								{
								split.Qty += qty;
								if (split.Qty == 0)
									split = Mode.PickedForPack.Delete(split);
								else
									split = Mode.PickedForPack.Update(split);
							}
							}

							if (link.PackedQty + qty > 0)
							{
								link.PackedQty += qty;
								Graph.PackageDetailExt.PackageDetailSplit.Update(link);
							}
							else if (link.PackedQty + qty == 0)
							{
								Graph.PackageDetailExt.PackageDetailSplit.Delete(link);
							}

							package.Confirmed = false;
							Graph.Packages.Update(package);
						}
						else
						{
							if (link == null)
							{
								link = (SOShipLineSplitPackage)Base.PackageDetailExt.PackageDetailSplit.Cache.CreateInstance();

								PXFieldVerifying ver = (c, a) => a.Cancel = true;
								Graph.FieldVerifying.AddHandler(ver);
								link.ShipmentSplitLineNbr = split.SplitLineNbr;
								link.PackedQty = qty;
								link = Graph.PackageDetailExt.PackageDetailSplit.Insert(link);
								Graph.FieldVerifying.RemoveHandler(ver);

								link.ShipmentNbr = split.ShipmentNbr;
								link.ShipmentLineNbr = split.LineNbr;
								link.PackageLineNbr = package.LineNbr;
								link.InventoryID = split.InventoryID;
								link.SubItemID = split.SubItemID;
								link.LotSerialNbr = split.LotSerialNbr;
								link.UOM = split.UOM;

								link = Graph.PackageDetailExt.PackageDetailSplit.Update(link);
							}
							else
							{
								link.PackedQty += qty;
								Graph.PackageDetailExt.PackageDetailSplit.Update(link);
							}
						}

						return true;
					}

					public virtual void EnsureShipmentUserLinkForPack()
					{
						Graph.WorkLogExt.EnsureFor(
							Basis.RefNbr,
							Graph.Accessinfo.UserID,
							Basis.HasPick == true
								? SOShipmentProcessedByUser.jobType.Pack
								: SOShipmentProcessedByUser.jobType.PackOnly);
					}
				}
				#endregion

				#region Messages
				[PXLocalizable]
				public new abstract class Msg
				{
					public const string Prompt = "Confirm packing {0} x {1} {2}.";

					public const string NothingToPack = "No items to pack.";
					public const string NothingToRemove = "No items to remove from the shipment.";

					public const string InventoryAdded = "{0} x {1} {2} has been added to the shipment.";
					public const string InventoryRemoved = "{0} x {1} {2} has been removed from the shipment.";

					public const string BoxCanNotPack = "Cannot pack {1} {2} of {0}.";
					public const string BoxCanNotUnpack = "Cannot unpack {1} {2} of {0}.";
				}
				#endregion
			}
			#endregion

			#region Commands
			public sealed class ConfirmPackageCommand : ScanCommand
			{
				public PackMode.Logic Mode => Get();

				public override string Code => "PACKAGE*CONFIRM";
				public override string ButtonName => "scanConfirmPackage";
				public override string DisplayName => Msg.DisplayName;
				protected override bool IsEnabled => Mode.CanConfirmPackage;
				protected override bool Process()
				{
					Basis.SetScanState();
					return true;
				}

				#region Messages
				[PXLocalizable]
				public abstract class Msg
				{
					public const string DisplayName = "Confirm Package";
				}
				#endregion
			}

			public sealed class RemoveCommand : WMSBase.RemoveCommand
			{
				protected override bool IsEnabled => base.IsEnabled && Get().HasConfirmableBoxes;
			}
			#endregion

			#region Redirect
			public sealed class RedirectFrom : WMSBase.RedirectFrom.SetMode
				where TForeignBasis : PXGraphExtension, IBarcodeDrivenStateMachine
			{
				public override string Code => PackMode.Value;
				public override string DisplayName => Msg.Description;

				private string RefNbr { get; set; }

				public override bool IsPossible
				{
					get
					{
						bool wmsFulfillment = PXAccess.FeatureInstalled();
						var ppsSetup = SOPickPackShipSetup.PK.Find(Basis.Graph, Basis.Graph.Accessinfo.BranchID);
						return wmsFulfillment && ppsSetup?.ShowPackTab == true;
					}
				}

				protected override bool PrepareRedirect()
				{
					if (Basis is PickPackShip pps && pps.RefNbr != null && pps.DocumentIsConfirmed == false)
					{
						if (pps.FindMode().TryValidate(pps.Shipment).By() is Validation valid && valid.IsError == true)
						{
							pps.ReportError(valid.Message, valid.MessageArgs);
							return false;
						}
						else
							RefNbr = pps.RefNbr;
					}
					
					return true;
				}

				protected override void CompleteRedirect()
				{
					if (Basis is PickPackShip pps && pps.CurrentMode.Code != ReturnMode.Value && this.RefNbr != null)
						if (pps.TryProcessBy(PickPackShip.ShipmentState.Value, RefNbr, StateSubstitutionRule.KeepAll & ~StateSubstitutionRule.KeepPositiveReports))
						{
							pps.SetDefaultState();
							RefNbr = null;
						}
				}
			}
			#endregion

			#region Messages
			[PXLocalizable]
			public new abstract class Msg : ScanMode.Msg
			{
				public const string Description = "Pack";
				public const string PackedQtyPerBox = "Packed Qty.";

				public const string Completed = "The {0} shipment is packed.";
				public const string CannotBePacked = "The {0} shipment cannot be processed in the Pack mode because the shipment has two or more packages assigned.";

				public const string BoxConfirmPrompt = "Confirm the package.";
				public const string BoxConfirmOrContinuePrompt = "Confirm package or scan the next item.";
				public const string BoxConfirmOrContinuePromptNoPick = "Confirm package, or scan the next item or the next location.";
			}
			#endregion

			#region Attached Fields
			[PXUIField(Visible = false)]
			public class ShowPack : FieldAttached.To.AsBool.Named
			{
				public override bool? GetValue(ScanHeader row) => Base.WMS.Get().ShowPackTab(row) == true;
			}

			[PXUIField(DisplayName = Msg.PackedQtyPerBox)]
			public class PackedQtyPerBox : FieldAttached.To.AsDecimal.Named
			{
				public override decimal? GetValue(SOShipLineSplit row)
				{
					SOShipLineSplitPackage package = Base.PackageDetailExt.PackageDetailSplit
						.SelectMain(Base.WMS.RefNbr, Base.WMS.Get().PackageLineNbrUI)
						.FirstOrDefault(t => t.ShipmentSplitLineNbr == row.SplitLineNbr);
					return package?.PackedQty ?? 0m;
				}
			}
			#endregion
		}
	}

	/// 
	public sealed class PackScanHeader : PXCacheExtension
	{
		#region PackageLineNbr
		[PXInt]
		[PXFormula(typeof(Null.When.Else))]
		public int? PackageLineNbr { get; set; }
		public abstract class packageLineNbr : BqlInt.Field { }
		#endregion
		#region PackageLineNbrUI
		[PXInt]
		[PXUIField(DisplayName = "Package")]
		[PXSelector(
			typeof(SearchFor.Where>),
			typeof(BqlFields.FilledWith<
				SOPackageDetailEx.confirmed,
				SOPackageDetailEx.lineNbr,
				SOPackageDetailEx.boxID,
				SOPackageDetailEx.boxDescription,
				SOPackageDetailEx.weight,
				SOPackageDetailEx.maxWeight,
				SOPackageDetailEx.weightUOM,
				SOPackageDetailEx.length,
				SOPackageDetailEx.width,
				SOPackageDetailEx.height>),
			DescriptionField = typeof(SOPackageDetailEx.boxID), DirtyRead = true, SuppressUnconditionalSelect = true)]
		[PXFormula(typeof(packageLineNbr.When.Else))]
		public int? PackageLineNbrUI { get; set; }
		public abstract class packageLineNbrUI : BqlInt.Field { }
		#endregion

		#region Weight
		[PXDecimal(6)]
		[PXUnboundDefault(TypeCode.Decimal, "0.0")]
		public decimal? Weight { get; set; }
		public abstract class weight : BqlDecimal.Field { }
		#endregion
		#region LastWeighingTime
		[PXDate]
		public DateTime? LastWeighingTime { get; set; }
		public abstract class lastWeighingTime : BqlDateTime.Field { }
		#endregion

		#region Length
		[PXDecimal(6)]
		public decimal? Length { get; set; }
		public abstract class length : BqlDecimal.Field { }
		#endregion
		#region Width
		[PXDecimal(6)]
		public decimal? Width { get; set; }
		public abstract class width : BqlDecimal.Field { }
		#endregion
		#region Height
		[PXDecimal(6)]
		public decimal? Height { get; set; }
		public abstract class height : BqlDecimal.Field { }
		#endregion
	}
}
