

using System.Collections.Generic;

using PX.Common;
using PX.BarcodeProcessing;
using PX.Data;
using PX.Data.BQL;
using PX.Data.BQL.Fluent;

using PX.Objects.IN.WMS;
using PX.Objects.SO.GraphExtensions.SOShipmentEntryExt;

namespace PX.Objects.SO.WMS
{
	using WMSBase = WarehouseManagementSystem;

	public partial class PickPackShip : WMSBase
	{
		public sealed class ShipMode : ScanMode
		{
			public const string Value = "SHIP";
			public class value : BqlString.Constant { public value() : base(ShipMode.Value) { } }

			public override string Code => Value;
			public override string Description => Msg.Description;

			protected override bool IsModeActive() => Basis.Setup.Current.ShowShipTab == true;

			#region State Machine
			protected override ScanState GetDefaultState() => Basis.RefNbr == null ? base.GetDefaultState() : FindState(BuiltinScanStates.Command);

			protected override IEnumerable> CreateStates()
			{
				yield return new ShipmentState();
				yield return new CommandOrShipmentOnlyState();
			}

			protected override IEnumerable> CreateCommands()
			{
				yield return new RefreshRatesCommand();
				yield return new GetLabelsCommand();
				yield return new ConfirmShipmentCommand();
			}

			protected override IEnumerable> CreateRedirects() => AllWMSRedirects.CreateFor();

			protected override void ResetMode(bool fullReset)
			{
				base.ResetMode(fullReset);
				Clear(when: fullReset && !Basis.IsWithinReset);
			}
			#endregion

			#region Logic
			public class Logic : ScanExtension
			{
				protected virtual void _(Events.RowSelected e)
				{
					if (e.Row?.Mode == ShipMode.Value)
						Basis.ScanConfirm.SetVisible(false);
				}

				public virtual bool ShowShipTab(ScanHeader row) => Basis.Setup.Current.ShowShipTab == true && row.Mode == ShipMode.Value;
			}

			// Acuminator disable once PX1016 ExtensionDoesNotDeclareIsActiveMethod extension should be constantly active
			public class CarrierRatesLogic : ScanExtension
			{
				protected virtual void ClearCarrierRates() => Graph.CarrierRatesExt.CarrierRates.Cache.Clear();
				protected virtual void _(Events.RowInserted e) => ClearCarrierRates();
				protected virtual void _(Events.RowUpdated e) => ClearCarrierRates();
				protected virtual void _(Events.RowDeleted e) => ClearCarrierRates();
			}
			#endregion

			#region States
			public new sealed class ShipmentState : PickPackShip.ShipmentState
			{
				private bool _needToRefreshRates = false;

				protected override Validation Validate(SOShipment shipment)
				{
					if (shipment.Operation != SOOperation.Issue)
						return Validation.Fail(Msg.InvalidOperation, shipment.ShipmentNbr, Basis.SightOf(shipment));

					if (shipment.Status != SOShipmentStatus.Open)
						return Validation.Fail(Msg.InvalidStatus, shipment.ShipmentNbr, Basis.SightOf(shipment));

					return Validation.Ok;
				}

				protected override void Apply(SOShipment shipment)
				{
					_needToRefreshRates = false;
					string prevShipmentNbr = Basis.RefNbr;

					base.Apply(shipment);

					if (Basis.RefNbr.IsNotIn(null, prevShipmentNbr) && !Basis.Header.Barcode.StartsWith(ScanMarkers.Redirect))
						_needToRefreshRates = true;
				}

				protected override void ClearState()
				{
					base.ClearState();

					_needToRefreshRates = false;
				}

				protected override void ReportSuccess(SOShipment shipment) => Basis.ReportInfo(Msg.Ready, shipment.ShipmentNbr);

				protected override void SetNextState()
				{
					if (_needToRefreshRates)
					Basis.Get().UpdateRates();

					_needToRefreshRates = false;
				}

				#region Messages
				[PXLocalizable]
				public new abstract class Msg : PickPackShip.ShipmentState.Msg
				{
					public new const string Ready = "{0} shipment loaded and ready to be shipped.";
					public const string InvalidStatus = "The {0} shipment cannot be processed in Ship mode because it has the {1} status.";
					public const string InvalidOperation = "The {0} shipment cannot be processed in Ship mode because it has the {1} operation.";
				}
				#endregion
			}
			#endregion

			#region Commands
			public sealed class GetLabelsCommand : ScanCommand
			{
				public override string Code => "GET*LABELS";
				public override string ButtonName => "scanGetLabels";
				public override string DisplayName => SOShipmentEntryActionsAttribute.Messages.GetReturnLabels;
				protected override bool IsEnabled => Basis.DocumentIsEditable;

				protected override bool Process() => Get().GetLabels();

				#region Logic
				public class Logic : ScanExtension
				{
					public virtual bool GetLabels()
					{
						Basis.Save.Press();
						var clone = Graph.Clone();
						var refNbr = Basis.RefNbr;
						PXLongOperation.StartOperation(Basis.Graph, () =>
						{
							PXLongOperation.SetCustomInfo(clone); // Redirect

							SOShipment shipment =
								SelectFrom.
								Where>.
								View.Select(clone, refNbr);

							clone.GetExtension().GetReturnLabels(shipment);
						});
						return true;
					}
				}
				#endregion
			}

			public sealed class RefreshRatesCommand : ScanCommand
			{
				public override string Code => "REFRESH*RATES";
				public override string ButtonName => "scanRefreshRates";
				public override string DisplayName => Messages.RefreshRatesButton;
				protected override bool IsEnabled => Basis.DocumentIsEditable;

				protected override bool Process() => Get().PerformRatesRefresh();

				#region Logic
				public class Logic : ScanExtension
				{
					public virtual bool PerformRatesRefresh()
					{
						if (!string.IsNullOrEmpty(Basis.RefNbr))
						{
							Basis.Save.Press();
							var clone = Graph.Clone();

							PXLongOperation.StartOperation(Graph, () =>
							{
								PXLongOperation.SetCustomInfo(clone); // Redirect
								UpdateRates(clone);
							});

							Basis.Graph.RowSelected.AddHandler((cache, args) =>
							{
								if (args.Row != null)
									cache.AdjustUI(args.Row).For(a =>
									{
										if (a.ErrorLevel == PXErrorLevel.Error)
											((IPXInterfaceField)a).ErrorLevel = PXErrorLevel.RowError;
									});
							});
						}
						return true;
					}

					public static void UpdateRates(PickPackShip.Host graph)
					{
						var carrierRateErrors = new Dictionary();
						void saveCarrierRateError(PXCache cache, PXExceptionHandlingEventArgs args)
						{
							if (args.Exception is PXSetPropertyException ex)
								carrierRateErrors[(SOCarrierRate)args.Row] = ex;
						};

						try
						{
							graph.ExceptionHandling.AddHandler(saveCarrierRateError);
							graph.CarrierRatesExt.UpdateRates();
						}
						finally
						{
							graph.ExceptionHandling.RemoveHandler(saveCarrierRateError);
						}

						var carrierRateCache = graph.Caches();
						foreach (var eInfo in carrierRateErrors)
						{
							var carrierRate = eInfo.Key;
							var error = eInfo.Value;
							error = new PXSetPropertyException(error.Message, PXErrorLevel.Error) { ErrorValue = carrierRate.Amount };
							carrierRateCache.RaiseExceptionHandling(carrierRate, carrierRate.Amount, error);
						}
					}

					public virtual void UpdateRates()
					{
						if ((SOPackageDetailEx)Basis.Graph.Packages.SelectWindowed(0, 1) == null)
							return;

						try
						{
							Basis.Graph.CarrierRatesExt.UpdateRates();
						}
						catch (PXException exception)
						{
							Basis.ReportError(exception.MessageNoPrefix);
						}
					}
				}
				#endregion
			}
			#endregion

			#region Redirect
			public sealed class RedirectFrom : WMSBase.RedirectFrom.SetMode
				where TForeignBasis : PXGraphExtension, IBarcodeDrivenStateMachine
			{
				public override string Code => ShipMode.Value;
				public override string DisplayName => Msg.Description;

				private string RefNbr { get; set; }

				public override bool IsPossible
				{
					get
					{
						if (Basis.Graph.IsMobile)
							return false;

						bool wmsFulfillment = PXAccess.FeatureInstalled();
						var ppsSetup = SOPickPackShipSetup.PK.Find(Basis.Graph, Basis.Graph.Accessinfo.BranchID);
						return wmsFulfillment && ppsSetup?.ShowShipTab == true;
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
					{
						if (pps.TryProcessBy(PickPackShip.ShipmentState.Value, RefNbr, StateSubstitutionRule.KeepAll & ~StateSubstitutionRule.KeepPositiveReports))
						{
							pps.SetDefaultState();
							RefNbr = null;

							bool needToConfirmPackage = pps.Get().HasSingleAutoPackage(pps.RefNbr, out SOPackageDetailEx autoPackage) && autoPackage.Confirmed != true;
							if (needToConfirmPackage)
							{
								autoPackage.Confirmed = true;
								pps.Graph.Packages.Update(autoPackage);
								pps.Graph.Document.Current.IsPackageValid = true;
								pps.Graph.Document.UpdateCurrent();
								pps.Reset(fullReset: false);
								pps.SaveChanges();
							}

							pps.Get().UpdateRates();
						}
					}
				}
			}
			#endregion

			#region Messages
			[PXLocalizable]
			public new abstract class Msg : ScanMode.Msg
			{
				public const string Description = "Ship";
			}
			#endregion

			#region Attached Fields
			[PXUIField(Visible = false)]
			public class ShowShip : FieldAttached.To.AsBool.Named
			{
				public override bool? GetValue(ScanHeader row) => Base.WMS.Get().ShowShipTab(row) == true;
			}
			#endregion
		}
	}
}
