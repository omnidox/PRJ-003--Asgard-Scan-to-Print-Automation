

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
using System.Threading;
using PX.SM;
using PX.Common;
using PX.BarcodeProcessing;
using PX.Data;
using PX.Data.BQL;
using PX.Data.BQL.Fluent;

using PX.Objects.Common;
using PX.Objects.Common.Extensions;
using PX.Objects.Extensions;
using PX.Objects.AR;
using PX.Objects.CS;
using PX.Objects.IN;
using PX.Objects.IN.WMS;
using PX.Objects.SO.GraphExtensions.SOShipmentEntryExt;

namespace PX.Objects.SO.WMS
{
	using WMSBase = WarehouseManagementSystem;

	public partial class PickPackShip : WMSBase
	{
		public class Host : SOShipmentEntry
		{
			public PickPackShip WMS => FindImplementation();
		}

		public new class QtySupport : WMSBase.QtySupport { }
		public new class GS1Support : WMSBase.GS1Support { }
		public class UserSetup : PXUserSetup { }

		#region State
		#region BaseQty
		public new decimal BaseQty => INUnitAttribute.ConvertToBase(Graph.Transactions.Cache, InventoryID, UOM, Qty ?? 0, INPrecision.NOROUND);
		#endregion
		#endregion

		#region Configuration
		public override bool ExplicitConfirmation => Setup.Current.ExplicitLineConfirmation == true;

		public override bool DocumentIsEditable => base.DocumentIsEditable && !DocumentIsConfirmed;
		public virtual bool DocumentIsConfirmed => Shipment?.Confirmed == true;

		protected override bool UseQtyCorrection => Setup.Current.UseDefaultQty != true;
		protected override bool CanOverrideQty => base.CanOverrideQty ||
			DocumentIsEditable && DefaultLotSerial && LotSerialTrack.IsTrackedSerial ||
			DocumentIsEditable && IsTransfer && LotSerialTrack.IsTrackedSerial && LotSerialTrack.IsEnterable;

		public virtual bool DefaultLocation => UserSetup.For(Base).DefaultLocationFromShipment == true;
		public virtual bool DefaultLotSerial => UserSetup.For(Base).DefaultLotSerialFromShipment == true;

		public virtual bool HasPick => Setup.Current.ShowPickTab == true;
		public virtual bool HasPack => Setup.Current.ShowPackTab == true;

		public virtual bool CannotConfirmPartialShipments => Setup.Current.ShortShipmentConfirmation == SOPickPackShipSetup.shortShipmentConfirmation.Forbid;
		public virtual bool PromptLocationForEveryLine => Setup.Current.RequestLocationForEachItem == true;
		#endregion

		#region Views
		public
			PXSetupOptional>>
			Setup;
		#endregion

		#region Buttons
		public PXAction ViewOrder;
		[PXButton, PXUIField(DisplayName = "View Order")]
		protected virtual IEnumerable viewOrder(PXAdapter adapter)
		{
			SOShipLineSplit currentSplit = (SOShipLineSplit)Graph.Caches().Current;
			if (currentSplit == null)
				return adapter.Get();

			SOShipLine currentLine =
				SelectFrom.
				Where<
					SOShipLine.shipmentNbr.IsEqual.
					And>>.
				View.SelectSingleBound(Graph, new[] { currentSplit });
			if (currentLine == null)
				return adapter.Get();

			var orderEntry = PXGraph.CreateInstance();
			orderEntry.Document.Current = orderEntry.Document.Search(currentLine.OrigOrderType, currentLine.OrigOrderNbr);
			throw new PXRedirectRequiredException(orderEntry, true, nameof(ViewOrder)) { Mode = PXBaseRedirectException.WindowMode.NewWindow };
		}
		#endregion

		#region Event Handlers
		protected override void _(Events.RowSelected e)
		{
			base._(e);

			if (e.Row == null)
				return;

			if (DocumentIsConfirmed == true)
			{
				PXCache splitsCache = Graph.Caches();

				splitsCache.SetAllEditPermissions(false);
				splitsCache.AdjustUI().ForAllFields(a => a.Enabled = false);
			}

			if (String.IsNullOrEmpty(RefNbr))
				Graph.Document.Current = null;
			else
				Graph.Document.Current = Base.Document.Search(RefNbr);
		}

		protected virtual void _(Events.RowUpdated e) => e.Row.IsOverridden = !e.Row.SameAs(Setup.Current);
		protected virtual void _(Events.RowInserted e) => e.Row.IsOverridden = !e.Row.SameAs(Setup.Current);

		protected virtual void _(Events.FieldSelecting e)
		{
			if (e.Row != null && e.Row.IsUnassigned == true)
				e.ReturnValue = IN.Messages.Unassigned;
		}

		protected virtual void _(Events.RowSelected e)
		{
			if (e.Row != null && e.Row.IsUnassigned == true)
				e.Cache.Adjust(e.Row).ForAllFields(a => a.Enabled = false);
		}
		#endregion

		#region DAC overrides
		#region ScanHeader
		[Common.Attributes.BorrowedNote(typeof(SOShipment), typeof(SOShipmentEntry))]
		protected virtual void _(Events.CacheAttached e) { }

		[PXMergeAttributes(Method = MergeMethod.Replace)]
		[PXString(15, IsUnicode = true, InputMask = ">CCCCCCCCCCCCCCC")]
		[PXUIField(DisplayName = "Shipment Nbr.", Enabled = false)]
		[PXSelector(typeof(SOShipment.shipmentNbr))]
		protected virtual void _(Events.CacheAttached e) { }

		[PXMergeAttributes]
		[PXFormula(typeof(InventoryMultiplicator.increase.When>.Else))]
		protected virtual void _(Events.CacheAttached e) { }
		#endregion
		#region SOShipLineSplit
		[PXCustomizeBaseAttribute(typeof(PXUIFieldAttribute), nameof(PXUIFieldAttribute.Visible), true)]
		protected virtual void _(Events.CacheAttached e) { }

		[PXCustomizeBaseAttribute(typeof(SOShipLotSerialNbrAttribute), nameof(SOShipLotSerialNbrAttribute.ForceDisable), true)]
		protected virtual void _(Events.CacheAttached e) { }

		[PXCustomizeBaseAttribute(typeof(SiteAttribute), nameof(SiteAttribute.Enabled), false)]
		protected virtual void _(Events.CacheAttached e) { }

		[PXCustomizeBaseAttribute(typeof(SOLocationAvailAttribute), nameof(SOLocationAvailAttribute.Enabled), false)]
		protected virtual void _(Events.CacheAttached e) { }

		[PXCustomizeBaseAttribute(typeof(PXUIFieldAttribute), nameof(PXUIFieldAttribute.Enabled), false)]
		protected virtual void _(Events.CacheAttached e) { }
		#endregion
		#region SOShipLine
		[PXMergeAttributes]
		[PXSelector(typeof(SearchFor.Where>))]
		protected virtual void _(Events.CacheAttached e) { }
		#endregion
		#endregion

		#region State Machine
		protected override ScanMode GetDefaultMode()
		{
			UserPreferences userPreferences =
				SelectFrom.
				Where>.
				View.Select(Base);
			var preferencesExt = userPreferences?.GetExtension();

			var pickMode = ScanModes.OfType().FirstOrDefault();
			var packMode = ScanModes.OfType().FirstOrDefault();
			var shipMode = ScanModes.OfType().FirstOrDefault();
			var returnMode = ScanModes.OfType().FirstOrDefault();

			return
				preferencesExt?.PPSMode == DefaultPickPackShipModeByUser.pPSMode.Pick && Setup.Current.ShowPickTab == true ? pickMode :
				preferencesExt?.PPSMode == DefaultPickPackShipModeByUser.pPSMode.Pack && Setup.Current.ShowPackTab == true ? packMode :
				preferencesExt?.PPSMode == DefaultPickPackShipModeByUser.pPSMode.Ship && Setup.Current.ShowShipTab == true ? shipMode :
				preferencesExt?.PPSMode == DefaultPickPackShipModeByUser.pPSMode.Return && Setup.Current.ShowReturningTab == true ? returnMode :
				Setup.Current.ShowPickTab == true ? pickMode :
				Setup.Current.ShowPackTab == true ? packMode :
				Setup.Current.ShowShipTab == true ? shipMode :
				Setup.Current.ShowReturningTab == true ? returnMode :
				base.GetDefaultMode();
		}

		protected override IEnumerable> CreateScanModes()
		{
			yield return new PickMode();
			yield return new PackMode();
			yield return new ShipMode();
			yield return new ReturnMode();
		}
		#endregion

		#region Logic
		public virtual SOShipment Shipment => SOShipment.PK.Find(Base, Base.Document.Current);

		public virtual bool IsTransfer => Shipment?.ShipmentType == SOShipmentType.Transfer;

		public virtual IEnumerable> GetSplits(string shipmentNbr, bool includeUnassigned = false, Func processedSeparator = null)
		{
			var assignedOnly =
				SelectFrom.
				InnerJoin.On.
				InnerJoin.On>.
				Where>.
				View.Select(Base, shipmentNbr)
				.AsEnumerable()
				.Cast>();

			IEnumerable> splits;
			if (includeUnassigned)
			{
				SOShipLineSplit MakeAssigned(Unassigned.SOShipLineSplit unassignedSplit) => PropertyTransfer.Transfer(unassignedSplit, new SOShipLineSplit());

				var unassignedOnly =
					SelectFrom.
					InnerJoin.On.
					InnerJoin.On>.
					Where>.
					View.Select(Base, shipmentNbr)
					.AsEnumerable()
					.Cast>()
					.Select(r => new PXResult(MakeAssigned(r), r, r));

				splits = assignedOnly.Concat(unassignedOnly);
			}
			else
				splits = assignedOnly;

			(var processed, var notProcessed) = processedSeparator != null
				? splits.DisuniteBy(s => processedSeparator(s.GetItem()))
				: (Array.Empty>(), splits);

			var result = new List>();

			result.AddRange(
				notProcessed
				.OrderBy(
					r => Setup.Current.ShipmentLocationOrdering == SOPickPackShipSetup.shipmentLocationOrdering.Pick
						? r.GetItem().PickPriority
						: r.GetItem().PathPriority)
				.ThenBy(r => r.GetItem().IsUnassigned == false) // unassigned first
				.ThenBy(r => r.GetItem().InventoryID)
				.ThenBy(r => r.GetItem().LotSerialNbr));

			result.AddRange(
				processed
				.OrderByDescending(
					r => Setup.Current.ShipmentLocationOrdering == SOPickPackShipSetup.shipmentLocationOrdering.Pick
						? r.GetItem().PickPriority
						: r.GetItem().PathPriority)
				.ThenByDescending(r => r.GetItem().InventoryID)
				.ThenByDescending(r => r.GetItem().LotSerialNbr));

			return result;
		}

		public virtual bool IsLocationMissing(PXSelectBase splitView, INLocation location, out Validation error)
		{
			if (splitView.SelectMain().All(t => t.LocationID != location.LocationID))
			{
				error = Validation.Fail(Msg.LocationMissingInShipment, location.LocationCD);
				return true;
			}
			else
			{
				error = Validation.Ok;
				return false;
			}
		}

		public virtual bool IsItemMissing(PXSelectBase splitView, PXResult item, out Validation error)
		{
			(INItemXRef xref, InventoryItem inventoryItem) = item;
			if (splitView.SelectMain().All(t => t.InventoryID != inventoryItem.InventoryID))
			{
				error = Validation.Fail(Msg.InventoryMissingInShipment, inventoryItem.InventoryCD);
				return true;
			}
			else
			{
				error = Validation.Ok;
				return false;
			}
		}

		public virtual bool IsLotSerialMissing(PXSelectBase splitView, string lotSerialNbr, out Validation error)
		{
			if (!LotSerialTrack.IsEnterable && splitView.SelectMain().All(t => !string.Equals(t.LotSerialNbr, lotSerialNbr, StringComparison.OrdinalIgnoreCase)))
			{
				error = Validation.Fail(Msg.LotSerialMissingInShipment, lotSerialNbr);
				return true;
			}
			else
			{
				error = Validation.Ok;
				return false;
			}
		}

		public void EnsureAssignedSplitEditing(SOShipLineSplit split)
		{
			if (split.IsUnassigned == true)
				throw new InvalidOperationException("Unassigned splits should not be edited directly by WMS screen");
		}

		[Obsolete]
		public virtual string GetCommandOrShipmentOnlyPrompt() => Get().GetPromptForCommandOrShipmentOnly();

		public virtual bool HasNonStockLinesWithEmptyLocation(SOShipment shipment, out Validation error)
		{
			SOShipLine shipLine =
				SelectFrom.
				InnerJoin.On.
				Where<
					InventoryItem.stkItem.IsEqual.
					And>.
					And.
					And>>.
				View.ReadOnly.Select(this, shipment.ShipmentNbr);

			if(shipLine != null)
			{
				error = Validation.Fail(Msg.ShipmentContainsNonStockItemWithEmptyLocation, shipment.ShipmentNbr);
				return true;
			}

			error = Validation.Ok;
			return false;
		}

		public virtual bool HasIncompleteLinesBy()
			where TQtyField : class, IBqlField, IImplement
		{
			bool hasIncompleteLines =
				SelectFrom.
				InnerJoin.On.
				InnerJoin.On.
				InnerJoin.On.
				Where<
					SOShipLine.FK.Shipment.SameAsCurrent.
					And>.IsGreater>.
					And<
						SOOrder.shipComplete.IsEqual.
						Or>>>.
				View.SelectMultiBound(this, new[] { Shipment }).Any();
			return hasIncompleteLines;
		}

		protected override void LogScan(ScanHeader headerBefore, ScanHeader headerAfter)
		{
			base.LogScan(headerBefore, headerAfter);

			if (!headerBefore.Barcode.StartsWith(ScanMarkers.Redirect))
			{
				UpdateWorkLogOnLogScan(Graph.WorkLogExt, Info.Current.MessageType == ScanMessageTypes.Error);
				if (Graph.Caches().IsDirty)
					Graph.WorkLogExt.PersistWorkLog();
			}
		}
		protected virtual void UpdateWorkLogOnLogScan(SOShipmentEntry.WorkLog workLogger, bool isError)
		{
			if (Shipment == null)
				return;

			string jobType;
			if (CurrentMode is PackMode)
			{
				jobType = HasPick ? SOShipmentProcessedByUser.jobType.Pack : SOShipmentProcessedByUser.jobType.PackOnly;
			}
			else if (CurrentMode is PickMode || CurrentMode is ReturnMode)
			{
				jobType = SOShipmentProcessedByUser.jobType.Pick;
			}
			else return;

			workLogger.LogScanFor(
				Shipment.ShipmentNbr,
				Graph.Accessinfo.UserID,
				jobType,
				isError);
		}
		#endregion

		#region States
		public abstract class ShipmentState : RefNbrState
		{
			protected override string StatePrompt => Msg.Prompt;

			protected override SOShipment GetByBarcode(string barcode)
			{
				SOShipment shipment =
					SelectFrom.
					InnerJoin.On.
					LeftJoin.On>.SingleTableOnly.
					Where<
						SOShipment.shipmentNbr.IsEqual<@P.AsString>.
						And>.
						And<
							Customer.bAccountID.IsNull.
							Or>>>.
					View.ReadOnly.Select(Basis, barcode);
				return shipment;
			}

			protected override void Apply(SOShipment shipment)
			{
				Basis.Graph.Document.Current = shipment;

				Basis.RefNbr = shipment.ShipmentNbr;
				Basis.SiteID = shipment.SiteID;
				Basis.TranDate = shipment.ShipDate;
				Basis.TranType =
					shipment.ShipmentType == SOShipmentType.Transfer ? INTranType.Transfer :
					shipment.Operation == SOOperation.Receipt ? INTranType.Return :
					INTranType.Issue;
				Basis.NoteID = shipment.NoteID;
			}

			protected override void ClearState()
			{
				Basis.Graph.Document.Current = null;

				Basis.RefNbr = null;
				Basis.SiteID = null;
				Basis.TranDate = null;
				Basis.TranType = null;
				Basis.NoteID = null;
			}

			protected override void ReportMissing(string barcode) => Basis.ReportError(Msg.Missing, barcode);
			protected override void ReportSuccess(SOShipment shipment) => Basis.ReportInfo(Msg.Ready, shipment.ShipmentNbr);

			#region Messages
			[PXLocalizable]
			public abstract class Msg
			{
				public const string Prompt = "Scan the shipment number.";
				public const string Ready = "The {0} shipment is loaded and ready to be processed.";
				public const string Missing = "The {0} shipment is not found.";
				public const string Invalid = "The {0} shipment cannot be processed because it has the {1} status.";
			}
			#endregion
		}

		public sealed class CommandOrShipmentOnlyState : CommandOnlyStateBase
		{
			public override void MoveToNextState() { }
			public override string Prompt => Basis.Get().GetPromptForCommandOrShipmentOnly();
			public override bool Process(string barcode)
			{
				if (Basis.TryProcessBy(barcode, StateSubstitutionRule.KeepAbsenceHandling))
				{
					Basis.Clear();
					Basis.Reset(fullReset: false);
					Basis.SetScanState();
					Basis.CurrentMode.FindState().Process(barcode);
					return true;
				}
				else
				{
					Basis.Reporter.Error(Basis.Get().GetErrorForCommandOrShipmentOnly());
					return false;
				}
			}

			// Acuminator disable once PX1016 ExtensionDoesNotDeclareIsActiveMethod extension should be constantly active
			public class Logic : ScanExtension
			{
				public virtual string GetPromptForCommandOrShipmentOnly() => Msg.UseCommandOrShipmentToContinue;
				public virtual string GetErrorForCommandOrShipmentOnly() => Msg.OnlyCommandsAndShipmentsAreAllowed;
			}

			#region Messages
			[PXLocalizable]
			public new abstract class Msg
			{
				public const string UseCommandOrShipmentToContinue = "Use any command or scan the next shipment to continue.";
				public const string OnlyCommandsAndShipmentsAreAllowed = "Only commands or a shipment can be used to continue.";
			}
			#endregion
		}
		#endregion

		#region Commands
		public sealed class ConfirmShipmentCommand : ScanCommand
		{
			public override string Code => "CONFIRM*SHIPMENT";
			public override string ButtonName => "scanConfirmShipment";
			public override string DisplayName => Msg.DisplayName;
			protected override bool IsEnabled => Basis.DocumentIsEditable;

			protected override bool Process() => Basis.Get().ConfirmShipment(confirmAsIs: !Basis.HasPick && !Basis.HasPack);

			#region Logic
			public class Logic : ScanExtension
			{
				public virtual bool ConfirmShipment(bool confirmAsIs)
				{
					if (!CanConfirm(confirmAsIs))
						return true;

					PackMode.Logic packLogic = Basis.Get();
					if (packLogic.SelectedPackage?.Confirmed == false)
						if (Basis.Get().TryAutoConfirm() == false)
							return true;

					int? packageLineNbr = packLogic.PackageLineNbr;
					Basis.Reset(fullReset: false);
					Basis.Clear();
					packLogic.PackageLineNbr = packageLineNbr;

					SOPackageDetailEx autoPackageToConfirm = null;
					if (!confirmAsIs && Basis.Header.Mode.IsIn(PackMode.Value, ShipMode.Value))
						packLogic.HasSingleAutoPackage(Basis.RefNbr, out autoPackageToConfirm);

					var (shipmentNbr, setup, userSetup) = (Basis.RefNbr, Basis.Setup.Current, UserSetup.For(Basis));

					Basis.SaveChanges();

					Basis
					.AwaitFor((basis, doc, ct) => ConfirmShipmentHandler(doc.ShipmentNbr, confirmAsIs, setup, userSetup, autoPackageToConfirm, ct))
					.WithDescription(Msg.InProcess, Basis.RefNbr)
					.ActualizeDataBy((basis, doc) => SOShipment.PK.Find(basis, doc))
					.OnSuccess(x => x.Say(Msg.Success).ChangeStateTo())
					.OnFail(x => x.Say(Msg.Fail))
					.BeginAwait(Basis.Shipment);

					return true;
				}

				protected static System.Threading.Tasks.Task ConfirmShipmentHandler(string shipmentNbr, bool confirmAsIs, SOPickPackShipSetup setup, SOPickPackShipUserSetup userSetup, SOPackageDetailEx autoPackageToConfirm, CancellationToken cancellationToken)
					=> PXGraph.CreateInstance().FindImplementation().ApplyPickedQtyAndConfirmShipment(shipmentNbr, confirmAsIs, setup, userSetup, autoPackageToConfirm, cancellationToken);

				protected virtual bool CanConfirm(bool confirmAsIs)
				{
					if (confirmAsIs)
						return true;

					if (Basis.HasPick && !CanConfirmPicked())
						return false;

					if (Basis.HasPack && !CanConfirmPacked())
						return false;

					return true;
				}

				protected virtual bool CanConfirmPicked()
				{
					var splits = Basis.Get().Picked.SelectMain();
					if (splits.All(s => s.PickedQty == 0))
					{
						Basis.ReportError(Msg.ShipmentCannotBeConfirmed);
						return false;
					}

					if (Basis.Info.Current.MessageType != ScanMessageTypes.Warning && splits.Any(s => s.PickedQty < s.Qty * Basis.Graph.GetMinQtyThreshold(s)))
					{
						if (Basis.CannotConfirmPartialShipments)
							Basis.ReportError(Msg.ShipmentCannotBeConfirmedInPart);
						else
							Basis.ReportWarning(Msg.ShipmentShouldNotBeConfirmedInPart);
						return false;
					}

					if (Basis.HasIncompleteLinesBy())
					{
						Basis.ReportError(Msg.ShipmentCannotBeConfirmedInPart);
						return false;
					}

					return true;
				}

				protected virtual bool CanConfirmPacked()
				{
					var splits = Basis.Get().PickedForPack.SelectMain();
					if (splits.All(s => s.PackedQty == 0))
						return true;

					if (Basis.Info.Current.MessageType != ScanMessageTypes.Warning && splits.Any(s => s.PackedQty < s.Qty * Basis.Graph.GetMinQtyThreshold(s)))
					{
						if (Basis.CannotConfirmPartialShipments)
							Basis.ReportError(Msg.ShipmentCannotBeConfirmedInPart);
						else
							Basis.ReportWarning(Msg.ShipmentShouldNotBeConfirmedInPart);
						return false;
					}

					if (Basis.HasIncompleteLinesBy())
					{
						Basis.ReportError(Msg.ShipmentCannotBeConfirmedInPart);
						return false;
					}

					return true;
				}

				[Obsolete("Use the " + nameof(PickPackShip) + "." + nameof(PickPackShip.HasIncompleteLinesBy) + " method instead.")]
				protected virtual bool HasIncompleteLinesBy()
					where TQtyField : class, IBqlField, IImplement
					=> Basis.HasIncompleteLinesBy();
			}

			public class PickPackShipShipmentConfirmation : PXGraphExtension
			{
				public static bool IsActive() => PXAccess.FeatureInstalled();

				public virtual async System.Threading.Tasks.Task ApplyPickedQtyAndConfirmShipment(string shipmentNbr, bool confirmAsIs, SOPickPackShipSetup setup, SOPickPackShipUserSetup userSetup, SOPackageDetailEx autoPackageToConfirm, CancellationToken cancellationToken)
				{
					using (var tranScope = new PXTransactionScope())
					{
						try
						{
							Base.Document.Current =
								SelectFrom.
								Where>.
								View.Select(Base, shipmentNbr);

							CloseShipmentUserLinks(shipmentNbr);

							ApplyPickedQty(confirmAsIs, setup);

							HandleCarts();

							if (Base.IsDirty && Base.Document.Current.ShipmentQty == 0)
								throw new PXException(Messages.UnableConfirmZeroShipment, Base.Document.Current.ShipmentNbr);

							HandlePackages(confirmAsIs, setup, autoPackageToConfirm);

							if (Base.IsDirty)
								Base.Save.Press();

							TryUseExternalConfirmation();

							Base.confirmShipmentAction.Press();

							Base.Clear();
							Base.Document.Current = Base.Document.Search(shipmentNbr);

							tranScope.Complete(Base);
						}
						catch (PXBaseRedirectException)
						{
							tranScope.Complete(Base);
							throw;
						}
					}

					await TryPrintShipmentForms(userSetup, cancellationToken);
				}

				protected virtual void CloseShipmentUserLinks(string shipmentNbr) => Base.WorkLogExt?.CloseFor(shipmentNbr);

				protected virtual void ApplyPickedQty(bool confirmAsIs, SOPickPackShipSetup setup)
				{
					var kitSpecHelper = new NonStockKitSpecHelper(Base);
					var RequireShipping = Func.Memorize((int inventoryID) => InventoryItem.PK.Find(Base, inventoryID).With(item => item.StkItem == true || item.NonStockShip == true));

					if (!confirmAsIs && (setup.ShowPickTab == true || setup.ShowPackTab == true))
					{
						PXSelectBase lines = Base.Transactions;
						PXSelectBase splits = Base.splits;

						foreach (SOShipLine line in lines.Select())
						{
							lines.Current = line;
							decimal lineQty = 0;

							decimal GetNewQty(SOShipLineSplit split) => setup.ShowPickTab == true ? split.PickedQty ?? 0 : Math.Max(split.PickedQty ?? 0, split.PackedQty ?? 0);
							if (kitSpecHelper.IsNonStockKit(line.InventoryID))
							{
								// kitInventoryID -> compInventory -> qty
								var nonStockKitSpec = kitSpecHelper.GetNonStockKitSpec(line.InventoryID.Value).Where(pair => RequireShipping(pair.Key)).ToDictionary();
								var nonStockKitSplits = splits.SelectMain().GroupBy(r => r.InventoryID.Value).ToDictionary(g => g.Key, g => g.Sum(s => GetNewQty(s)));

								lineQty = nonStockKitSpec.Keys.Count() == 0 || nonStockKitSpec.Keys.Except(nonStockKitSplits.Keys).Count() > 0
									? 0
									: (from split in nonStockKitSplits
									   join spec in nonStockKitSpec on split.Key equals spec.Key
									   select Math.Floor(decimal.Divide(split.Value, spec.Value))).Min();
							}
							else
							{
								using (new UpdateIfFieldsChangedScope().AppendContext(typeof(SOShipLine.locationID)))
								foreach (SOShipLineSplit split in splits.Select())
								{
									splits.Current = split;

									decimal newQty = GetNewQty(splits.Current);
									if (newQty != splits.Current.Qty)
									{
										splits.Current.Qty = newQty;
										splits.UpdateCurrent();
									}

									if (splits.Current.Qty != 0)
										lineQty += splits.Current.Qty ?? 0;
								}
								lineQty = INUnitAttribute.ConvertFromBase(lines.Cache, lines.Current.InventoryID, lines.Current.UOM, lineQty, INPrecision.NOROUND);
							}

							lines.Current.Qty = lineQty;
							lines.UpdateCurrent();

							PXSelectBase sosetup = Base.sosetup;
							if (lines.Current.Qty == 0 && sosetup.Current.AddAllToShipment == false)
								lines.DeleteCurrent();
						}
					}
				}

				protected virtual void HandleCarts()
				{
					foreach (SOCartShipment cartLink in SelectFrom.Where>.View.Select(Base))
						Base.Caches().Delete(cartLink);
				}

				protected virtual void HandlePackages(bool confirmAsIs, SOPickPackShipSetup setup, SOPackageDetailEx autoPackageToConfirm)
				{
					if (!confirmAsIs && (setup.ShowPickTab == true || setup.ShowPackTab == true))
					{
						foreach (SOPackageDetailEx package in Base.Packages.SelectMain())
							if (package.PackageType == SOPackageType.Manual && Base.PackageDetailExt.PackageDetailSplit.Select(package.ShipmentNbr, package.LineNbr).Count == 0)
								Base.Packages.Delete(package);
					}

					if (autoPackageToConfirm?.Confirmed == false)
					{
						autoPackageToConfirm.Confirmed = true;
						Base.Packages.Update(autoPackageToConfirm);
					}

					if (PXAccess.FeatureInstalled())
					{
						var packages = Base.Packages.SelectMain();
						if (confirmAsIs)
						{
							foreach (var package in packages.Where(x => x.Confirmed != true))
							{
								package.Confirmed = true;
								Base.Packages.Cache.Update(package);
							}
						}
						if (Base.Document.Current.IsPackageValid == false && packages.Any(p => p.PackageType == SOPackageType.Auto))
						{
							Base.Document.Current.IsPackageValid = true;
							Base.Document.UpdateCurrent();
						}
					}

					if (Base.IsDirty)
					{
						Base.Document.Current.IsPackageValid = true;
						Base.Document.UpdateCurrent();
						Base.Save.Press();
					}
				}

				protected virtual void TryUseExternalConfirmation()
				{
					if (UseExternalShippingApplication(Base.Document.Current, out Carrier carrier))
					{
						// Shipping Tool will confirm the shipment.
						throw new PXRedirectToUrlException(
							$"../../Frames/ShipmentAppLauncher.html?ShipmentApplicationType={carrier.ShippingApplicationType}&ShipmentNbr={Base.Document.Current.ShipmentNbr}",
							PXBaseRedirectException.WindowMode.NewWindow, true, string.Empty);
					}
				}

				public virtual async System.Threading.Tasks.Task TryPrintShipmentForms(SOPickPackShipUserSetup userSetup, CancellationToken cancellationToken)
				{
					bool anyPrinted = false;
					if (PXAccess.FeatureInstalled())
					{
						var labelsPrintingExt = Base.GetExtension();
						//Labels should ALWAYS be printer first because they go out faster, and that gives time to user to peel/stick them while shipment confirmation is spooling
						if (userSetup.PrintShipmentLabels == true)
						{
							try
							{
								await labelsPrintingExt.PrintCarrierLabels(cancellationToken);
							}
							catch (PXBaseRedirectException) { }
						}

						if (userSetup.PrintCommercialInvoices == true)
						{
							try
							{
								await labelsPrintingExt.PrintCommercInvoices(cancellationToken);
								anyPrinted = true;
							}
							catch (PXBaseRedirectException) { }
						}

						if (userSetup.PrintShipmentConfirmation == true)
						{
							WithSuppressedRedirects(() => Base.PrintConfirmation());
							anyPrinted = true;
						}
					}

					return anyPrinted;
				}

				protected virtual bool UseExternalShippingApplication(SOShipment shipment, out Carrier carrier)
				{
					carrier = Carrier.PK.Find(Base, shipment.ShipVia);
					return Base.IsMobile == false && carrier != null && carrier.IsExternalShippingApplication == true;
				}
			}
			#endregion

			#region Messages
			[PXLocalizable]
			public abstract class Msg
			{
				public const string DisplayName = "Confirm Shipment";
				public const string InProcess = "The {0} shipment is being confirmed.";
				public const string Success = "The shipment has been successfully confirmed.";
				public const string Fail = "The shipment confirmation failed.";

				public const string ShipmentCannotBeConfirmed = "The shipment cannot be confirmed because no items have been picked.";
				public const string ShipmentCannotBeConfirmedNoPacked = "The shipment cannot be confirmed because no items have been packed.";
				public const string ShipmentCannotBeConfirmedInPart = "The shipment cannot be confirmed because it is not complete.";
				public const string ShipmentShouldNotBeConfirmedInPart = "The shipment is incomplete and should not be confirmed. Do you want to confirm the shipment?";
			}
			#endregion
		}

		public sealed class ConfirmShipmentAsIsCommand : ScanCommand
		{
			public override string Code => "CONFIRM*SHIPMENT*ALL";
			public override string ButtonName => "scanConfirmShipmentAll";
			public override string DisplayName => Msg.DisplayName;
			protected override bool IsEnabled => Basis.DocumentIsEditable;

			protected override bool Process() => Basis.Get().ConfirmShipment(confirmAsIs: true);

			#region Messages
			[PXLocalizable]
			public new abstract class Msg : ConfirmShipmentCommand.Msg
			{
				public new const string DisplayName = "Confirm Shipment As Is";
			}
			#endregion
		}
		#endregion

		#region Decorations
		public virtual void InjectLocationDeactivationOnDefaultLocationOption(LocationState locationState)
		{
			locationState.Intercept.IsStateActive.ByConjoin(basis =>
				!basis.DefaultLocation);
		}

		public virtual void InjectLotSerialDeactivationOnDefaultLotSerialOption(LotSerialState lsState, bool isEntranceAllowed)
		{
			lsState.Intercept.IsStateActive.ByConjoin(basis =>
				!basis.DefaultLotSerial || basis.Remove == true || isEntranceAllowed && basis.LotSerialTrack.IsEnterable);
			lsState.Intercept.IsStateActive.ByConjoin(basis =>
				basis.SelectedLotSerialClass.With(it => it.LotSerAssign == INLotSerAssign.WhenUsed).Implies(!basis.IsTransfer));
		}

		public virtual void InjectLocationSkippingOnPromptLocationForEveryLineOption(LocationState locationState)
		{
			locationState.Intercept.IsStateSkippable.ByDisjoin(basis =>
				!basis.PromptLocationForEveryLine && basis.LocationID != null);
		}

		public virtual void InjectItemAbsenceHandlingByLocation(InventoryItemState inventoryState)
		{
			inventoryState.Intercept.HandleAbsence.ByAppend((basis, barcode) =>
				basis.TryProcessBy(barcode, StateSubstitutionRule.KeepPositiveReports | StateSubstitutionRule.KeepApplication)
					? AbsenceHandling.Done
					: AbsenceHandling.Skipped);
		}

		public virtual void InjectLocationPresenceValidation(LocationState locationState, Func> viewSelector)
		{
			locationState.Intercept.Validate.ByAppend((basis, location) =>
				basis.IsLocationMissing(viewSelector(basis), location, out var error)
					? error
					: Validation.Ok);
		}

		public virtual void InjectItemPresenceValidation(InventoryItemState itemState, Func> viewSelector)
		{
			itemState.Intercept.Validate.ByAppend((basis, item) =>
				basis.IsItemMissing(viewSelector(basis), item, out var error)
					? error
					: Validation.Ok);
		}

		public virtual void InjectLotSerialPresenceValidation(LotSerialState lotSerailState, Func> viewSelector)
		{
			lotSerailState.Intercept.Validate.ByAppend((basis, lotSerialNbr) =>
				basis.IsLotSerialMissing(viewSelector(basis), lotSerialNbr, out var error)
					? error
					: Validation.Ok);
		}
		#endregion

		#region Messages
		[PXLocalizable]
		public new abstract class Msg : WMSBase.Msg
		{
			public const string ShipmentIsNotEditable = "The shipment became unavailable for editing. Contact your manager.";

			public const string InventoryMissingInShipment = "The {0} inventory item is not present in the shipment.";
			public const string LocationMissingInShipment = "The {0} location is not present in the shipment.";
			public const string LotSerialMissingInShipment = "The {0} lot/serial number is not present in the shipment.";
			public const string ShipmentContainsNonStockItemWithEmptyLocation = "The {0} shipment cannot be processed via Pick, Pack and Ship because it contains non-stock item with empty location.";
		}
		#endregion

		#region Attached Fields
		public static class FieldAttached
		{
			public abstract class To : PXFieldAttachedTo.By
				where TTable : class, IBqlTable, new()
			{ }

			[PXUIField(DisplayName = Msg.Fits)]
			public class Fits : FieldAttached.To.AsBool.Named
			{
				public override bool? GetValue(SOShipLineSplit row)
				{
					bool fits = true;
					if (Base.WMS.LocationID != null)
						fits &= Base.WMS.LocationID == row.LocationID;
					if (Base.WMS.InventoryID != null)
						fits &= Base.WMS.InventoryID == row.InventoryID && Base.WMS.SubItemID == row.SubItemID;
					if (Base.WMS.LotSerialNbr != null)
						fits &= string.Equals(Base.WMS.LotSerialNbr, row.LotSerialNbr, StringComparison.OrdinalIgnoreCase) || Base.WMS.Header.Mode == PickMode.Value && Base.WMS.LotSerialTrack.IsEnterable && row.PickedQty == 0;
					return fits;
				}
			}

			[PXUIField(Visible = false)]
			public class ShowLog : FieldAttached.To.AsBool.Named
			{
				public override bool? GetValue(ScanHeader row) => Base.WMS.Setup.Current.ShowScanLogTab == true;
			}
		}
		#endregion
	}
}
