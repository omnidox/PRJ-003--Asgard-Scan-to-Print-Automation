

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading;
using PX.Api;
using PX.CarrierService;
using PX.Common;
using PX.Data;
using PX.Data.BQL;
using PX.Data.BQL.Fluent;
using PX.Data.DependencyInjection;
using PX.Data.WorkflowAPI;
using PX.LicensePolicy;
using PX.Objects.AR;
using PX.Objects.AR.MigrationMode;
using PX.Objects.CM;
using PX.Objects.Common;
using PX.Objects.Common.Extensions;
using PX.Objects.Common.Scopes;
using PX.Objects.CR;
using PX.Objects.CS;
using PX.Objects.EP;
using PX.Objects.GL.FinPeriods;
using PX.Objects.GL.FinPeriods.TableDefinition;
using PX.Objects.IN;
using PX.Objects.IN.InventoryRelease.Accumulators.QtyAllocated;
using PX.Objects.PO;
using PX.Objects.SO.GraphExtensions.CarrierRates;
using PX.Objects.SO.GraphExtensions.SOOrderEntryExt;
using PX.Objects.SO.GraphExtensions.SOShipmentEntryExt;
using PX.Objects.SO.Models;
using PX.Objects.SO.Services;
using PX.SM;
using POLineType = PX.Objects.PO.POLineType;
using POReceiptLine = PX.Objects.PO.POReceiptLine;
using ShipmentActions = PX.Objects.SO.SOShipmentEntryActionsAttribute;

namespace PX.Objects.SO
{
	public partial class SOShipmentEntry : PXGraph, IGraphWithInitialization
	{
		public SOShipmentLineSplittingExtension LineSplittingExt => FindImplementation();
		public SOShipmentItemAvailabilityExtension ItemAvailabilityExt => FindImplementation();

		public ToggleCurrency CurrencyView;
		[PXViewName(Messages.SOShipment)]
		public PXSelectJoin>,
			LeftJoin>>,
			Where2>>>,
			And>>>>>>> Document;
		public PXSelect>>> CurrentDocument;
		public PXSelect>>, OrderBy>>> Transactions;
		public PXSelect>, And>>>> splits;
		public PXSelect>, And>>>> unassignedSplits;
		[PXViewName(Messages.ShippingAddress)]
		public PXSelect>>> Shipping_Address;
		[PXViewName(Messages.ShippingContact)]
		public PXSelect>>> Shipping_Contact;

		public PXSelect sosetupapproval;

		public PXSelect>, And>>, OrderBy>>> FreeItems;
		[PXViewName(Messages.SOPackageDetail)]
		public PXSelect>>> Packages;
		[PXHidden]
		public PXSelect>>> PackagesForRates;
		[PXHidden]
		[PXCopyPasteHiddenView]
		public PXSelect LabelHistory;
		public PXSetup>>> carrier;
		public PXSelect>>> currencyinfo;
		public PXSelect DummyCuryInfo;

		public PXSetup insetup;
		public PXSetup sosetup;
		public PXSetup arsetup;
		public PXSetupOptional commonsetup;

		public PXSetup>>, Where>, And, Equal, Or>, And, NotEqual>>>>> Company; //TODO: Need review INRegister Branch and SOShipment SiteID/DestinationSiteID AC-55773
		public PXSetup>>> customer;
		public PXSetup>, And>>>> location;

		public class SkipShipCompleteValidationScope : FlaggedModeScopeBase { }

		[PXViewName(CR.Messages.MainContact)]
		public PXSelect DefaultCompanyContact;
		protected virtual IEnumerable defaultCompanyContact()
		{
			return OrganizationMaint.GetDefaultContactForCurrentOrganization(this);
		}

		public PXAction putOnHold;
		[PXButton(CommitChanges = true), PXUIField(DisplayName = "Hold")]
		protected virtual IEnumerable PutOnHold(PXAdapter adapter) => adapter.Get();

		public PXAction releaseFromHold;
		[PXButton(CommitChanges = true), PXUIField(DisplayName = "Remove Hold")]
		protected virtual IEnumerable ReleaseFromHold(PXAdapter adapter) => adapter.Get();

		public PXInitializeState initializeState;

		public PXAction notification;
		[PXUIField(DisplayName = "Notifications", Visible = false)]
		[PXButton(ImageKey = PX.Web.UI.Sprite.Main.DataEntryF)]
		protected virtual IEnumerable Notification(PXAdapter adapter,
		[PXString]
		string notificationCD
		)
		{
			var shipments = adapter.Get().ToList();
			PXProcessing.ProcessRecords(shipments, adapter.MassProcess, shipment =>
			{
				Document.Current = shipment;

				var parameters = new Dictionary();
				parameters["SOShipment.ShipmentNbr"] = shipment.ShipmentNbr;

				GL.Branch branch = PXSelectReadonly2>>,
						Where>,
								And, Equal,
							Or>,
								And, NotEqual>>>>>
					.SelectSingleBound(this, new object[] { shipment });

				this.GetExtension().SendNotification(ARNotificationSource.Customer, notificationCD, (branch != null && branch.BranchID != null) ? branch.BranchID : Accessinfo.BranchID, parameters, adapter.MassProcess);
			});

			return shipments;
		}

		public PXAction emailShipment;
		[PXButton(CommitChanges = true), PXUIField(DisplayName = "Email Shipment", MapEnableRights = PXCacheRights.Select, MapViewRights = PXCacheRights.Select)]
		public virtual IEnumerable EmailShipment(
			PXAdapter adapter,
			[PXString]
			string notificationCD = null) => Notification(adapter, notificationCD ?? "SHIPMENT");

		#region Action menu items
		public PXAction applyAssignmentRules;
		[PXButton(CommitChanges = true), PXUIField(DisplayName = ShipmentActions.Messages.ApplyAssignmentRules, Visible = false)]
		protected virtual IEnumerable ApplyAssignmentRules(PXAdapter adapter)
		{
			if (sosetup.Current.DefaultShipmentAssignmentMapID == null)
				throw new PXSetPropertyException(Messages.AssignNotSetup, Messages.SOSetup);

			var list = adapter.Get().ToList();

			var processor = CreateInstance>();
			processor.Assign(Document.Current, sosetup.Current.DefaultShipmentAssignmentMapID);
			Document.Update(Document.Current);

			return list;
		}

		public PXAction correctShipmentAction;
		[PXButton(CommitChanges = true), PXUIField(DisplayName = ShipmentActions.Messages.CorrectShipment, MapEnableRights = PXCacheRights.Select, MapViewRights = PXCacheRights.Select)]
		protected virtual IEnumerable CorrectShipmentAction(PXAdapter adapter)
		{
			var list = adapter.Get().ToList();
			bool massProcess = adapter.MassProcess;

			Save.Press();

			PXLongOperation.StartOperation(this, delegate ()
			{
				var docgraph = CreateInstance();

				var sourceGraph = docgraph.GetOrigDocumentGraph(string.Empty);

				PXProcessing.ProcessRecords(list, massProcess, shipment =>
				{
					using PXTransactionScope ts = new();

					docgraph.SetSuppressWorkflowOnCorrectShipment();
					docgraph.CorrectShipment(new (shipment, sourceGraph));
					docgraph.CancelPackages(shipment);

					ts.Complete();
				});
			});

			return list;
		}

		public PXAction printPickListAction;
		[PXButton(CommitChanges = true), PXUIField(DisplayName = ShipmentActions.Messages.PrintPickList, MapEnableRights = PXCacheRights.Select, MapViewRights = PXCacheRights.Select)]
		protected virtual IEnumerable PrintPickListAction(PXAdapter adapter)
		{
			if (!adapter.MassProcess && IsDirty)
				this.Save.Press();

			var list = adapter.Get().ToList();
			LongOperationManager.StartAsyncOperation(ct => PrintPickListOperation(list, adapter, ct));
			return list;
		}

		#endregion

		public PXAction inquiry;
		[PXUIField(DisplayName = "Inquiries", MapEnableRights = PXCacheRights.Select)]
		[PXButton(SpecialType = PXSpecialButtonType.InquiriesFolder, MenuAutoOpen = true)]
		protected virtual IEnumerable Inquiry(PXAdapter adapter,
			[PXInt]
			[PXIntList(new int[] { }, new string[] { })]
			int? inquiryID,
			[PXString()]
			string ActionName
			)
		{
			if (!string.IsNullOrEmpty(ActionName))
			{
				PXAction action = this.Actions[ActionName];

				if (action != null)
				{
					Save.Press();
					foreach (object data in action.Press(adapter)) ;
				}
			}
			return adapter.Get();
		}

		//throw new PXReportRequiredException(parameters, "SO642000", "Shipment Confirmation");
		public PXAction report;
		[PXUIField(DisplayName = "Reports", MapEnableRights = PXCacheRights.Select)]
		[PXButton(SpecialType = PXSpecialButtonType.ReportsFolder, MenuAutoOpen = true)]
		public virtual IEnumerable Report(PXAdapter adapter, [PXString(8, InputMask = "CC.CC.CC.CC")] string reportID)
		{
			var shipments = adapter.Get().ToImmutableList();
			if (!String.IsNullOrEmpty(reportID) && shipments.Any())
			{
				Save.Press();

				string GetActualReportID(SOShipment shipment)
				{
					if (adapter.MassProcess)
						PXProcessing.SetCurrentItem(shipment);

					Document.Current = shipment;
					GL.Branch company = null;
					using (new PXReadBranchRestrictedScope()) company = Company.Select();
					return new NotificationUtility(this).SearchCustomerReport(reportID, shipment.CustomerID, company.BranchID);
				}

				PXReportRequiredException combinedReport = shipments
					.Select(sh =>
					(
						ActualReportID: GetActualReportID(sh),
						Parameters: new Dictionary { ["SOShipment.ShipmentNbr"] = sh.ShipmentNbr }
					))
					.Aggregate(
						(PXReportRequiredException)null,
						(acc, elem) =>
						{
							CurrentLocalization localization = null;
							INSite warehouse = INSite.PK.Find(this, Document.Current.SiteID);
							if (warehouse != null)
							{
								localization = new CurrentLocalization(OrganizationLocalizationHelper.GetCurrentLocalizationCodeForBranch(warehouse.BranchID));
							}
							var report = PXReportRequiredException.CombineReport(acc, elem.ActualReportID, elem.Parameters, localization);

							if (adapter.MassProcess)
								PXProcessing.SetProcessed();

							report.Mode = PXBaseRedirectException.WindowMode.New;
							return report;
						});

				if (combinedReport != null)
				{
					if (PXAccess.FeatureInstalled())
					{
						LongOperationManager.StartAsyncOperation(async ct=>
						await SMPrintJobMaint.CreatePrintJobGroup(adapter, new NotificationUtility(this).SearchPrinter,
							SONotificationSource.Customer, reportID, Accessinfo.BranchID, combinedReport, null, ct));
					}

					throw combinedReport;
				}
			}
			return shipments;
		}

		public PXAction printShipmentConfirmation;
		[PXButton(CommitChanges = true), PXUIField(DisplayName = "Print Shipment Confirmation", MapEnableRights = PXCacheRights.Select, MapViewRights = PXCacheRights.Select)]
		public virtual IEnumerable PrintShipmentConfirmation(PXAdapter adapter) => Report(adapter.Apply(it => it.Menu = "Print Shipment Confirmation"), "SO642000");

		public PXAction calculateFreight;
		[PXUIField(DisplayName = Messages.RefreshFreight, MapEnableRights = PXCacheRights.Update, MapViewRights = PXCacheRights.Update)]
		[PXButton()]
		public virtual IEnumerable CalculateFreight(PXAdapter adapter)
		{
			CalculateFreightCost(false);

			return adapter.Get();
		}

		public virtual void PrintConfirmation() => PrintShipmentConfirmation(CreateDummyAdapter());

		public PXAction inventorySummary;
		[PXUIField(DisplayName = "Inventory Summary", MapEnableRights = PXCacheRights.Select, MapViewRights = PXCacheRights.Select)]
		[PXLookupButton]
		public virtual IEnumerable InventorySummary(PXAdapter adapter)
		{
			PXCache tCache = Transactions.Cache;
			SOShipLine line = Transactions.Current;
			if (line == null) return adapter.Get();

			InventoryItem item = InventoryItem.PK.Find(this, line.InventoryID);
			if (item != null && item.StkItem == true)
			{
				INSubItem sbitem = (INSubItem)PXSelectorAttribute.Select(tCache, line);
				InventorySummaryEnq.Redirect(item.InventoryID,
											 ((sbitem != null) ? sbitem.SubItemCD : null),
											 line.SiteID,
											 line.LocationID);
			}
			return adapter.Get();
		}

		public SOShipmentEntry()
		{
			if (PXAccess.FeatureInstalled())
			{
				INSetup inrecord = insetup.Current;
			}

			CommonSetup csrecord = commonsetup.Current;
			SOSetup sorecord = sosetup.Current;


			ARSetupNoMigrationMode.EnsureMigrationModeDisabled(this);

			CopyPaste.SetVisible(false);

			PXUIFieldAttribute.SetDisplayName(Caches[typeof(Contact)], CR.Messages.Attention);
			this.Views.Caches.Add(typeof(SOLineSplit));
			this.Views.Caches.Add(typeof(NoteDoc));

			FieldDefaulting.AddHandler((sender, e) => { if (e.Row != null) e.NewValue = BAccountType.CustomerType; });

			if (!PXAccess.FeatureInstalled())
			{
				CarrierRatesExt.shopRates.SetCaption(PXMessages.LocalizeNoPrefix(Messages.Packages));
			}
		}

		#region Entity Event Handlers
		public PXWorkflowEventHandler OnShipmentCorrected;

		public PXWorkflowEventHandler OnInvoiceReleased;
		public PXWorkflowEventHandler OnInvoiceCancelled;
		#endregion

		[InjectDependency]
		protected ILicenseLimitsService _licenseLimits { get; set; }

		void IGraphWithInitialization.Initialize()
		{
			if (_licenseLimits != null)
			{
				OnBeforeCommit += CheckLicenseLimitsBeforeCommitHandler;
			}
		}

		private void CheckLicenseLimitsBeforeCommitHandler(PXGraph e)
		{
			var checkTransactions = _licenseLimits.GetCheckerDelegate(new TableQuery(TransactionTypes.LinesPerMasterRecord, typeof(SOShipLine), (graph) =>
				{
					return new PXDataFieldValue[] { new PXDataFieldValue(((SOShipmentEntry)graph).Document.Current?.ShipmentNbr) };
				}));

			try
			{
				checkTransactions.Invoke(e);
			}
			catch (PXException)
			{
				throw new PXException(Messages.LicenseShipLine);
			}


			var checkSplits = _licenseLimits.GetCheckerDelegate(new TableQuery(TransactionTypes.SerialsPerDocument, typeof(SOShipLineSplit), (graph) =>
				{
					return new PXDataFieldValue[] { new PXDataFieldValue(((SOShipmentEntry)graph).Document.Current?.ShipmentNbr) };
				}));

			try
			{
				checkSplits.Invoke(e);
			}
			catch (PXException)
			{
				throw new PXException(Messages.LicenseShipLineSplit);
			}

			if (Document.Current?.UnlimitedPackages != true)
			{
				var checkPackages = _licenseLimits.GetCheckerDelegate(new TableQuery(TransactionTypes.LinesPerMasterRecord, typeof(SOPackageDetail), (graph) =>
				{
					return new PXDataFieldValue[] { new PXDataFieldValue(((SOShipmentEntry)graph).Document.Current?.ShipmentNbr) };
				}));

				try
				{
					checkPackages.Invoke(e);
				}
				catch (PXException)
				{
					throw new PXException(Messages.LicensePackageDetail);
				}
			}
		}

		[PXMergeAttributes(Method = MergeMethod.Append)]
		[PXRestrictor(typeof(Where, Equal, And, Equal,
			Or>>>>),
			Messages.CantSelectShipTermsWithFreightAmountSource, typeof(ShipTerms.freightAmountSource))]
		protected virtual void SOShipment_ShipTermsID_CacheAttached(PXCache sender)
		{
		}

		#region SOShipLine Cache Attached

		[PXMergeAttributes(Method = MergeMethod.Append)]
		[PXRemoveBaseAttribute(typeof(INUnitAttribute))]
		[SOShipLineUnit(DisplayName = "UOM")]
		protected virtual void SOShipLine_UOM_CacheAttached(PXCache sender) { }

		[PXMergeAttributes(Method = MergeMethod.Append)]
		[PXFormula(null, typeof(SumCalc))]
		protected virtual void SOShipLine_ShippedQty_CacheAttached(PXCache sender)
		{
		}

		[PXMergeAttributes(Method = MergeMethod.Append)]
		[PXFormula(typeof(Mult>>>), typeof(SumCalc))]
		protected virtual void SOShipLine_LineAmt_CacheAttached(PXCache sender)
		{
		}

		[PXMergeAttributes(Method = MergeMethod.Append)]
		[PXFormula(typeof(Mult.WithDependencies, SOShipLine.unitWeigth>), typeof(SumCalc))]
		protected virtual void SOShipLine_ExtWeight_CacheAttached(PXCache sender)
		{
		}

		[PXMergeAttributes(Method = MergeMethod.Append)]
		[PXFormula(typeof(Mult.WithDependencies, SOShipLine.unitVolume>), typeof(SumCalc))]
		protected virtual void SOShipLine_ExtVolume_CacheAttached(PXCache sender)
		{
		}
		#endregion

		#region SOShipmentAddress Cache Attached

		[PXMergeAttributes(Method = MergeMethod.Append)]
		[PXCustomizeBaseAttribute(typeof(PXUIFieldAttribute), nameof(PXUIFieldAttribute.Visible), true)]
		public virtual void _(Events.CacheAttached e) { }

		[PXMergeAttributes(Method = MergeMethod.Append)]
		[PXCustomizeBaseAttribute(typeof(PXUIFieldAttribute), nameof(PXUIFieldAttribute.Visible), true)]
		public virtual void _(Events.CacheAttached e) { }

		protected virtual void _(Events.RowUpdated e)
		{
			SOShipmentAddress row = e.Row;
			if (row == null) return;

			if (!e.Cache.ObjectsEqual(row, e.OldRow))
			{
				ResetFreightCostIsValid(Document.Current);
			}
		}

		#endregion

		public PXAction validateAddresses;
		[PXButton(CommitChanges = true), PXUIField(DisplayName = CS.Messages.ValidateAddresses, MapEnableRights = PXCacheRights.Select, MapViewRights = PXCacheRights.Select, FieldClass = CS.Messages.ValidateAddress)]
		public virtual IEnumerable ValidateAddresses(PXAdapter adapter)
		{
			foreach (SOShipment current in adapter.Get())
			{
				if (current != null)
				{
					FindAllImplementations().ValidateAddresses();
				}
				yield return current;
			}
		}

		#region CurrencyInfo events

		protected virtual void CurrencyInfo_CuryEffDate_FieldDefaulting(PXCache sender, PXFieldDefaultingEventArgs e)
		{
			if (Document.Current != null)
			{
				e.NewValue = Document.Current.ShipDate;
				e.Cancel = true;
			}
		}

		protected virtual void CurrencyInfo_RowSelected(PXCache sender, PXRowSelectedEventArgs e)
		{
			CurrencyInfo info = e.Row as CurrencyInfo;
			if (info != null)
			{
				bool curyenabled = info.AllowUpdate(this.Transactions.Cache);

				PXUIFieldAttribute.SetEnabled(sender, info, curyenabled);
				PXUIFieldAttribute.SetEnabled(sender, info, curyenabled);
				PXUIFieldAttribute.SetEnabled(sender, info, curyenabled);
				PXUIFieldAttribute.SetEnabled(sender, info, curyenabled);
			}
		}
		#endregion

		#region SOShipment Events
		protected virtual void SOShipment_RowUpdated(PXCache sender, PXRowUpdatedEventArgs e)
		{
			var row = (SOShipment)e.Row;
			var oldRow = (SOShipment)e.OldRow;

			EnsureControlQty(row);

			if (!sender.ObjectsEqual(e.Row, e.OldRow))
			{
				ResetFreightCostIsValid(row);
			}

			if (IsFreightRecalculationNeeded(row, oldRow))
				RecalculateFreight(row, oldRow);
		}

		protected void ResetFreightCostIsValid(SOShipment row)
		{
			if (row != null)
			{
				Carrier carrier = Carrier.PK.Find(this, row.ShipVia);
				if (carrier?.IsExternal == true)
				{
					row.FreightCostIsValid = false;
				}
				else
				{
					row.FreightCostIsValid = true;
				}
			}
		}

		protected virtual void EnsureControlQty(SOShipment shipment)
		{
			var cache = Document.Cache;

			if (sosetup.Current.RequireShipmentTotal != true)
			{
				cache.SetValue(shipment, shipment.ShipmentQty ?? 0m);
			}
			else if (shipment.Hold == false && shipment.Confirmed == false)
			{
				var controlQtyMsg = shipment.ShipmentQty != shipment.ControlQty && shipment.ControlQty != 0m
					? new PXSetPropertyException(Messages.DocumentOutOfBalance)
					: null;

				cache.RaiseExceptionHandling(shipment, shipment.ControlQty, controlQtyMsg);
			}
		}

		protected virtual bool IsFreightRecalculationNeeded(SOShipment row, SOShipment oldRow)
		{
			return
				!Document.Cache.ObjectsEqualBy>(oldRow, row);
		}

		protected virtual void RecalculateFreight(SOShipment row, SOShipment oldRow)
		{
			PXResultset shipLines = Transactions.Select();
			if (shipLines != null)
			{
				Carrier carrier = Carrier.PK.Find(this, row.ShipVia);
				if (!Document.Cache.ObjectsEqual(oldRow, row) && carrier?.CalcMethod == CarrierCalcMethod.Manual)
					row.FreightCost = 0m;

				if (UseFreightCalculator(row, carrier))
				{
					FreightCalculator fc = CreateFreightCalculator();
					fc.CalcFreightCost(Document.Cache, row);

					if (row.OverrideFreightAmount != true)
						fc.ApplyFreightTerms(Document.Cache, row, shipLines.Count);
				}
					else if (UseCarrierService(row, carrier) && row.OverrideFreightAmount != true)
					{
						FreightCalculator fc = CreateFreightCalculator();
						if (fc.IsFlatRate(Document.Cache, row))
						{
							fc.ApplyFreightTerms(Document.Cache, row, shipLines.Count);
						}
					}
			}
		}

		private void CalculateFreightCost(bool supressErrors)
		{
			if (Document.Current.ShipVia != null)
			{
				Carrier carrier = Carrier.PK.Find(this, Document.Current.ShipVia);
				if (carrier != null && carrier.IsExternal == true)
				{
					if (carrier.IsActive == false)
					{
						throw new PXException(Messages.ShipViaNotActive);
					}
					CarrierPlugin plugin = CarrierPlugin.PK.Find(this, carrier.CarrierPluginID);
					CarrierResult serviceResult = CarrierPluginMaint.CreateCarrierService(this, plugin, true);
					ICarrierService cs = serviceResult.Result;
					cs.Method = carrier.PluginMethod;

					CarrierRequest cr = CarrierRatesExt.BuildRateRequest(Document.Current);
					CarrierResult result = cs.GetRateQuote(cr);

					if (result != null)
					{
						StringBuilder sb = new StringBuilder();
						foreach (Message message in result.Messages)
						{
							sb.AppendFormat("{0}:{1} ", message.Code, message.Description);
						}

						if (result.IsSuccess)
						{
							decimal baseCost = ConvertAmtToBaseCury(result.Result.Currency, arsetup.Current.DefaultRateTypeID, Document.Current.ShipDate.Value, result.Result.Amount);
							Document.Current.CuryFreightCost = baseCost;
							Document.Current.FreightCostIsValid = true;
							Document.Update(Document.Current);

							if (result.Messages.Count > 0)
							{
								if (!supressErrors)
									Document.Cache.RaiseExceptionHandling(Document.Current, Document.Current.CuryFreightCost,
										new PXSetPropertyException(Document.Current, sb.ToString(), PXErrorLevel.Warning));
								else
									PXTrace.WriteWarning(sb.ToString());
							}
						}
						else
						{
							Document.Current.FreightCostIsValid = false;
							Document.Update(Document.Current);

							if (!supressErrors)
							{
								Document.Cache.RaiseExceptionHandling(Document.Current, Document.Current.CuryFreightCost,
										new PXSetPropertyException(Document.Current, Messages.CarrierServiceError, PXErrorLevel.Error, sb.ToString()));

								throw new PXException(Messages.CarrierServiceError, sb.ToString());
							}
							else
								PXTrace.WriteError(string.Format(Messages.CarrierServiceError, sb.ToString()));
						}
					}
				}
			}
		}

		protected virtual void SOShipment_RowSelected(PXCache sender, PXRowSelectedEventArgs e)
		{
			if (e.Row == null)
			{
				return;
			}

			SOShipment row = (SOShipment)e.Row;
			bool isTransfer = row.ShipmentType == SOShipmentType.Transfer;
			bool isNotConfirmed = row.Confirmed == false;
			bool isNotAddedToWorksheet = row.CurrentWorksheetNbr == null;
			bool isNotHeldByPicking = isNotAddedToWorksheet || row.Picked == true;
			bool isNotReadonly = isNotConfirmed;

			PXUIFieldAttribute.SetVisible(sender, e.Row,
				PXAccess.FeatureInstalled() && !isTransfer);

			bool curyenabled = true;

			PXUIFieldAttribute.SetEnabled(sender, e.Row, isNotReadonly && curyenabled);
			PXUIFieldAttribute.SetEnabled(sender, e.Row, isNotReadonly && row.OverrideFreightAmount == true);
			PXUIFieldAttribute.SetEnabled(sender, e.Row, AllowChangingOverrideFreightAmount(row));

			sender.AllowInsert = true;
			sender.AllowUpdate = isNotConfirmed;
			sender.AllowDelete = isNotReadonly && isNotAddedToWorksheet;

			Transactions.Cache.AllowInsert = false;
			Transactions.Cache.AllowUpdate = isNotReadonly && isNotAddedToWorksheet;
			Transactions.Cache.AllowDelete = isNotReadonly && isNotAddedToWorksheet;

			splits.Cache.AllowInsert = isNotReadonly && isNotAddedToWorksheet;
			splits.Cache.AllowUpdate = isNotReadonly && isNotAddedToWorksheet;
			splits.Cache.AllowDelete = isNotReadonly && isNotAddedToWorksheet;

			Packages.Cache.AllowInsert = isNotConfirmed && isNotHeldByPicking;
			Packages.Cache.AllowUpdate = isNotConfirmed && isNotHeldByPicking;
			Packages.Cache.AllowDelete = isNotConfirmed && isNotHeldByPicking;

			PXUIFieldAttribute.SetVisible(sender, e.Row, (bool)sosetup.Current.RequireShipmentTotal);

			bool allowUpdateAndHasNoDetails = sender.AllowUpdate && Transactions.Select().Count == 0;
			PXUIFieldAttribute.SetEnabled(sender, e.Row,
				allowUpdateAndHasNoDetails && sender.GetStatus(e.Row) == PXEntryStatus.Inserted);
			PXUIFieldAttribute.SetEnabled(sender, e.Row, allowUpdateAndHasNoDetails);
			PXUIFieldAttribute.SetEnabled(sender, e.Row, allowUpdateAndHasNoDetails);
			PXUIFieldAttribute.SetEnabled(sender, e.Row, allowUpdateAndHasNoDetails);
			PXUIFieldAttribute.SetEnabled(sender, e.Row, allowUpdateAndHasNoDetails);
			PXUIFieldAttribute.SetEnabled(sender, e.Row, allowUpdateAndHasNoDetails && isTransfer);

			this.validateAddresses.SetEnabled(isNotReadonly && FindAllImplementations().RequiresValidation());

			if (((SOShipment)e.Row).ShipVia != null)
			{
				Carrier carrier = Carrier.PK.Find(this, row.ShipVia);

				if (carrier != null)
				{
					PXUIFieldAttribute.SetEnabled(sender, e.Row, carrier.CalcMethod == CarrierCalcMethod.Manual && isNotReadonly);
				}

				//If the error is already attached to curyFreightCost, do not attach further warnings
				string FreightCostError = PXUIFieldAttribute.GetErrorOnly(sender, row);
				if (carrier?.IsExternal == true && string.IsNullOrEmpty(FreightCostError))
				{
					PXUIFieldAttribute.SetWarning(sender, e.Row, row.FreightCostIsValid == false && isNotReadonly ? Messages.FreightCostNotUptoDate : null);
				}
			}

			PXUIFieldAttribute.SetVisible(sender, e.Row, this.CanUseGroundCollect(row));

			PXUIFieldAttribute.SetVisible(sender, e.Row, !isTransfer);
			PXUIFieldAttribute.SetVisible(sender, e.Row, !isTransfer);

			PXUIFieldAttribute.SetVisible(sender, e.Row, isTransfer);

			PXUIFieldAttribute.SetVisible(Transactions.Cache, null, !isTransfer);

			PXUIFieldAttribute.SetRequired(sender, true);

			PXUIFieldAttribute.SetVisible(sender, e.Row, row.FreightAmountSource.IsIn(null, FreightAmountSourceAttribute.ShipmentBased));
			PXUIFieldAttribute.SetVisible(sender, e.Row, row.FreightAmountSource.IsIn(null, FreightAmountSourceAttribute.ShipmentBased));
		}

		protected virtual bool AllowChangingOverrideFreightAmount(SOShipment doc)
		{
			return doc.Confirmed == false &&
				doc.FreightAmountSource.IsIn(null, FreightAmountSourceAttribute.ShipmentBased);
		}

		protected virtual bool UseFreightCalculator(SOShipment row, Carrier carrier)
			=> carrier == null
				|| (carrier.IsExternal != true //for external carrier cost and terms are calculated in ShipPackages().
					&& AllowCalculateFreight(row, carrier));

		protected virtual bool UseCarrierService(SOShipment row, Carrier carrier)
			=> carrier != null && carrier.IsExternal == true && AllowCalculateFreight(row, carrier);

		protected virtual bool AllowCalculateFreight(SOShipment row, Carrier carrier)
		{
			if (row.Operation == SOOperation.Receipt)
				return carrier.CalcFreightOnReturn == true;
			return true;
		}

		protected virtual void SOShipment_RowPersisting(PXCache sender, PXRowPersistingEventArgs e)
		{
            if ((e.Operation & PXDBOperation.Command) == PXDBOperation.Delete) return;
			SOShipment doc = (SOShipment)e.Row;
			if (doc.ShipmentType == SOShipmentType.Transfer && doc.DestinationSiteID == null)
			{
				throw new PXRowPersistingException(typeof(SOOrder.destinationSiteID).Name, null, ErrorMessages.FieldIsEmpty, typeof(SOOrder.destinationSiteID).Name);
			}

			if (!SkipShipCompleteValidationScope.IsActive)
				ValidateShipComplete(doc);
		}

		protected virtual void SOShipment_CustomerID_FieldUpdated(PXCache sender, PXFieldUpdatedEventArgs e)
		{
			sender.SetDefaultExt(e.Row);
		}

		protected virtual void SOShipment_CustomerLocationID_FieldUpdated(PXCache sender, PXFieldUpdatedEventArgs e)
		{
			if (((SOShipment)e.Row).ShipmentType != SOShipmentType.Transfer && (((SOShipment)e.Row).SiteID == null || e.ExternalCall))
				sender.SetDefaultExt(e.Row);
			SOShipmentAddressAttribute.DefaultRecord(sender, e.Row);
			SOShipmentContactAttribute.DefaultRecord(sender, e.Row);
		}

		protected virtual void SOShipment_DestinationSiteID_FieldDefaulting(PXCache sender, PXFieldDefaultingEventArgs e)
		{
			SOShipment shipment = e.Row as SOShipment;
			if (shipment == null || shipment.ShipmentType != SOShipmentType.Transfer)
			{
				e.NewValue = null;
				e.Cancel = true;
			}
		}

		protected virtual void SOShipment_DestinationSiteID_FieldUpdated(PXCache sender, PXFieldUpdatedEventArgs e)
		{
			Company.RaiseFieldUpdated(sender, e.Row);
			GL.Branch company = null;
			using (new PXReadBranchRestrictedScope())
			{
				company = Company.Select();
			}

			if (((SOShipment)e.Row).ShipmentType == SOShipmentType.Transfer && company != null)
			{
				sender.SetValueExt(e.Row, company.BranchCD);
			}

			SOShipmentAddressAttribute.DefaultRecord(sender, e.Row);
			SOShipmentContactAttribute.DefaultRecord(sender, e.Row);
		}

		protected virtual void SOShipment_ShipVia_FieldUpdated(PXCache sender, PXFieldUpdatedEventArgs e)
		{
			RedefaultCurrencyInfo(sender, e);

			sender.SetDefaultExt(e.Row);

			SOShipment row = e.Row as SOShipment;
			if (row != null)
			{
				object pendingValue = sender.GetValuePending(e.Row);
				if (pendingValue != PXCache.NotSetValue && row.ShipViaUpdateFromShopForRate != true)
				{
					row.UseCustomerAccount = CanUseCustomerAccount(row) && (bool?)pendingValue == true;
				}
				else
				{
					row.UseCustomerAccount = CanUseCustomerAccount(row);
				}

				sender.SetValue(row, false);
				Document.Current.RecalcPackagesReason = (Document.Current.RecalcPackagesReason ?? 0) | SOShipment.recalcPackagesReason.ShipVia;
			}
		}

		public virtual void RedefaultCurrencyInfo(PXCache sender, PXFieldUpdatedEventArgs e)
		{
			if (PXAccess.FeatureInstalled())
			{
				CurrencyInfo info = CurrencyInfoAttribute.SetDefaults(sender, e.Row);

				string message = PXUIFieldAttribute.GetError(currencyinfo.Cache, info);
				if (string.IsNullOrEmpty(message) == false)
				{
					sender.RaiseExceptionHandling(e.Row, ((SOShipment)e.Row).ShipDate, new PXSetPropertyException(message, PXErrorLevel.Warning));
				}

				if (info != null)
				{
					sender.SetValue(e.Row, info.CuryID);
				}
			}
		}

		protected virtual bool CanUseCustomerAccount(SOShipment row)
		{
			Carrier carrier = Carrier.PK.Find(this, row.ShipVia);
			if (carrier != null && !string.IsNullOrEmpty(carrier.CarrierPluginID))
			{
				foreach (CarrierPluginCustomer cpc in PXSelect>,
						And>,
						And>>>>.Select(this, carrier.CarrierPluginID, row.CustomerID))
				{
					if (!string.IsNullOrEmpty(cpc.CarrierAccount) &&
						(cpc.CustomerLocationID == row.CustomerLocationID || cpc.CustomerLocationID == null)
						)
					{
						return true;
					}
				}
			}

			return false;
		}

		protected virtual bool CanUseGroundCollect(SOShipment row)
		{
			if (string.IsNullOrEmpty(row.ShipVia))
				return false;

			Carrier carrier = Carrier.PK.Find(this, row.ShipVia);
			if (carrier?.IsExternal != true || string.IsNullOrEmpty(carrier?.CarrierPluginID))
				return false;

			return CarrierPluginMaint.GetCarrierPluginAttributes(this, carrier.CarrierPluginID).Contains("COLLECT");
		}

		protected virtual void SOShipment_UseCustomerAccount_FieldVerifying(PXCache sender, PXFieldVerifyingEventArgs e)
		{
			SOShipment row = e.Row as SOShipment;
			if (row != null)
			{
				bool canBeTrue = CanUseCustomerAccount(row);

				if (e.NewValue != null && ((bool)e.NewValue) && !canBeTrue)
				{
					e.NewValue = false;
					throw new PXSetPropertyException(Messages.CustomeCarrierAccountIsNotSetup);
				}
			}
		}

		protected virtual void SOShipment_ShipTermsID_FieldVerifying(PXCache sender, PXFieldVerifyingEventArgs e)
		{
			var row = (SOShipment)e.Row;
			if (row != null && row.OrderCntr > 0 && row.FreightAmountSource == FreightAmountSourceAttribute.OrderBased)
			{
				PXUIFieldAttribute.SetWarning(sender, e.Row, Messages.FreightPriceNotRecalcInSO);
			}
		}

		[InjectDependency]
		public IFinPeriodRepository FinPeriodRepository { get; set; }

		protected virtual void SOShipment_ShipDate_FieldVerifying(PXCache sender, PXFieldVerifyingEventArgs e)
		{
			var row = (SOShipment)e.Row;
			if (e.NewValue != null)
			{
					FinPeriod finPeriod = FinPeriodRepository.FindFinPeriodByDate((DateTime?)e.NewValue, FinPeriod.organizationID.MasterValue);

					if (finPeriod == null)
					{
						throw new PXSetPropertyException(GL.Messages.TranDateOutOfRange, e.NewValue, PXAccess.GetOrganizationCD(FinPeriod.organizationID.MasterValue));
					}
			}
		}

		#endregion

		#region SOShipLine Events

		protected virtual void SOShipLine_InventoryID_FieldVerifying(PXCache sender, PXFieldVerifyingEventArgs e)
		{
			object oldValue = sender.GetValue(e.Row);
			if (oldValue != null)
			{
				e.NewValue = oldValue;
			}
		}

		protected virtual void SOShipLine_SubItemID_FieldVerifying(PXCache sender, PXFieldVerifyingEventArgs e)
		{
			object oldValue = sender.GetValue(e.Row);
			if (oldValue != null && e.NewValue != null && e.ExternalCall)
			{
				e.NewValue = oldValue;
			}
		}

		protected virtual void SOShipLine_SiteID_FieldVerifying(PXCache sender, PXFieldVerifyingEventArgs e)
		{
			object oldValue = sender.GetValue(e.Row);
			if (oldValue != null && e.ExternalCall)
			{
				e.NewValue = oldValue;
			}
		}

		protected virtual void _(Events.FieldVerifying e)
		{
			if (e.NewValue != null && (decimal?)e.NewValue < 0 && e.Row.LineType.IsIn(SOLineType.Inventory, SOLineType.NonInventory))
			{
				throw new PXSetPropertyException(Common.Messages.ShouldNotBeNegative, PXUIFieldAttribute.GetDisplayName(e.Cache));
			}
		}

		protected virtual void SOShipLine_InventoryID_FieldUpdated(PXCache sender, PXFieldUpdatedEventArgs e)
		{
			sender.SetDefaultExt(e.Row);
			sender.SetDefaultExt(e.Row);
			sender.SetDefaultExt(e.Row);

			SOShipLine tran = e.Row as SOShipLine;
			InventoryItem item = InventoryItem.PK.Find(this, tran?.InventoryID);
			if (item != null && tran != null)
			{
				tran.TranDesc = PXDBLocalizableStringAttribute.GetTranslation(Caches[typeof(InventoryItem)], item, nameof(InventoryItem.Descr), customer.Current?.LocaleName);
			}
		}

		protected virtual void SOShipLine_LocationID_FieldDefaulting(PXCache sender, PXFieldDefaultingEventArgs e)
		{
			if (PXAccess.FeatureInstalled())
			{
				e.NewValue = null;
				e.Cancel = true;
			}
		}

		protected virtual void DefaultUnitPrice(PXCache sender, PXFieldUpdatedEventArgs e)
		{
			object UnitPrice;
			sender.RaiseFieldDefaulting(e.Row, out UnitPrice);

			if (UnitPrice != null && (decimal)UnitPrice != 0m)
			{
				decimal? unitprice = INUnitAttribute.ConvertFromTo(sender, e.Row, ((SOShipLine)e.Row).UOM, ((SOShipLine)e.Row).OrderUOM, (decimal)UnitPrice, INPrecision.NOROUND);
				sender.SetValueExt(e.Row, unitprice);
			}
		}

		protected virtual void DefaultUnitCost(PXCache sender, PXFieldUpdatedEventArgs e)
		{
			object UnitCost;
			sender.RaiseFieldDefaulting(e.Row, out UnitCost);

			if (UnitCost != null && (decimal)UnitCost != 0m)
			{
				decimal? unitcost = INUnitAttribute.ConvertFromTo(sender, e.Row, ((SOShipLine)e.Row).UOM, ((SOShipLine)e.Row).OrderUOM, (decimal)UnitCost, INPrecision.UNITCOST);
				sender.SetValueExt(e.Row, unitcost);
			}
		}

		protected virtual void SOShipLine_UOM_FieldUpdated(PXCache sender, PXFieldUpdatedEventArgs e)
		{
			SOShipLine row = e.Row as SOShipLine;
			if (row != null)
			{
				DefaultUnitPrice(sender, e);
				DefaultUnitCost(sender, e);

				Transactions.Cache.RaiseFieldUpdated(row, null);
			}
		}

		protected virtual void SOShipLine_RowSelected(PXCache sender, PXRowSelectedEventArgs e)
		{
			SOShipLine row = e.Row as SOShipLine;
			if (row != null)
			{
				bool lineTypeInventory = row.LineType == SOLineType.Inventory;
				PXUIFieldAttribute.SetEnabled(sender, row, lineTypeInventory);
				PXUIFieldAttribute.SetEnabled(sender, row, lineTypeInventory);

				InventoryItem item = InventoryItem.PK.Find(this, row.InventoryID);
				if (item != null)
					PXUIFieldAttribute.SetEnabled(splits.Cache, null, item.KitItem == true && item.StkItem != true);
			}
		}

		protected virtual void SOShipLine_RowInserted(PXCache sender, PXRowInsertedEventArgs e)
		{
			Document.SetValueExt(Document.Current, false);
			Document.Current.RecalcPackagesReason = (Document.Current.RecalcPackagesReason ?? 0) | SOShipment.recalcPackagesReason.ShipLine;

			SOShipLine row = e.Row as SOShipLine;
			if (row != null)
			{
				row.SortOrder = row.LineNbr;
			}
		}

		protected virtual void SOShipLine_RowUpdated(PXCache sender, PXRowUpdatedEventArgs e)
		{
			SOShipLine row = e.Row as SOShipLine;
			SOShipLine oldRow = e.OldRow as SOShipLine;
			if (row != null && sender.GetStatus(row) == PXEntryStatus.Inserted)
			{
				row.OriginalShippedQty = row.ShippedQty;
				row.BaseOriginalShippedQty = row.BaseShippedQty;
			}

			if (row != null && oldRow != null && (row.BaseQty != oldRow.BaseQty))
			{
				Document.SetValueExt(Document.Current, false);
				Document.Current.RecalcPackagesReason = (Document.Current.RecalcPackagesReason ?? 0) | SOShipment.recalcPackagesReason.ShipLine;
			}
		}

		protected virtual void SOShipLine_RowDeleted(PXCache sender, PXRowDeletedEventArgs e)
		{
			SOShipLine deleted = (SOShipLine)e.Row;
			if (deleted == null) return;

			var parentDeleted = Document.Cache.GetStatus(Document.Current) == PXEntryStatus.Deleted;
			if (parentDeleted)
				return;

			Document.SetValueExt(Document.Current, false);
			Document.Current.RecalcPackagesReason = (Document.Current.RecalcPackagesReason ?? 0) | SOShipment.recalcPackagesReason.ShipLine;
		}

		protected virtual void SOShipLine_RowPersisting(PXCache sender, PXRowPersistingEventArgs e)
		{
			SOShipLine row = (SOShipLine)e.Row;

			if (((e.Operation & PXDBOperation.Command) == PXDBOperation.Insert || (e.Operation & PXDBOperation.Command) == PXDBOperation.Update))
			{
				CheckSplitsForSameTask(sender, row);
				CheckLocationTaskRule(sender, row);

				if (row.ShippedQty == decimal.Zero && row.BaseShippedQty != decimal.Zero)
				{
					throw new PXRowPersistingException(typeof(SOShipLine.shippedQty).Name, row.ShippedQty, Messages.BaseUnitQtyIsZeroBecauseOfLowPrecision, row.UOM, row.OrigLineNbr);
				}
			}
		}
		#endregion

		#region SOShipLineSplit Events

		protected virtual void SOShipLineSplit_InventoryID_FieldVerifying(PXCache sender, PXFieldVerifyingEventArgs e)
		{
			SOShipLine line = PXParentAttribute.SelectParent(sender, e.Row);
			if (line != null )
			{
				InventoryItem item = InventoryItem.PK.Find(this, line.InventoryID);
				if (item != null && item.KitItem == true && item.StkItem != true)
				{
					INKitSpecHdr detail =
						PXSelectJoin>,
							LeftJoin>>>,
						Where>,
						And<
							Where>,
							Or>>>>>>.SelectWindowed(this, 0, 1, line.InventoryID, e.NewValue, e.NewValue);

					if (detail == null)
					{
						InventoryItem val = InventoryItem.PK.Find(this, (int?)e.NewValue);

						var ex = new PXSetPropertyException(Messages.NotKitsComponent);
						ex.ErrorValue = val?.InventoryCD;

						throw ex;
					}
				}
			}
		}

		#endregion

		#region SOPackageDetail Events

		protected virtual void SOPackageDetailEx_Weight_FieldUpdated(PXCache sender, PXFieldUpdatedEventArgs e)
		{
			SOPackageDetail row = e.Row as SOPackageDetail;
			if (row != null)
			{
				row.Confirmed = true;
			}
		}

		protected virtual void SOPackageDetailEx_Weight_FieldVerifying(PXCache cache, PXFieldVerifyingEventArgs e)
		{
			var row = (SOPackageDetail) e.Row;
			if (row != null)
			{
				CSBox box = SOPackageDetail.FK.Box.FindParent(cache.Graph, row);
				if (box != null && box.MaxWeight < (decimal?) e.NewValue)
					throw new PXSetPropertyException(Messages.WeightExceedsBoxSpecs);
			}
		}

		protected virtual void SOPackageDetailEx_RowSelected(PXCache sender, PXRowSelectedEventArgs e)
		{
			SOPackageDetail row = e.Row as SOPackageDetail;
			if (row != null)
			{
				row.WeightUOM = commonsetup.Current.WeightUOM;
			}
		}

		#endregion

		#region SOShipmentContact Events

		private const string UpsCarrierPlugin = "PX.UpsRestCarrier.UpsRestCarrier";
		private const int MaxNameLengthForUps = 35;

		protected virtual bool BusinessNameLengthIsExceeded(SOShipment doc, SOShipmentContact contact)
		{
			if (doc == null || doc.ShipVia == null || doc.Confirmed == true
				|| contact == null || (contact.FullName ?? string.Empty).Length <= MaxNameLengthForUps)
				return false;

			Carrier carrier = Carrier.PK.Find(this, Document.Current.ShipVia);
			if (carrier == null || carrier.IsExternal != true)
				return false;

			CarrierPlugin carrierPlugin = CarrierPlugin.PK.Find(this, carrier.CarrierPluginID);
			if (carrierPlugin == null || carrierPlugin.PluginTypeName != UpsCarrierPlugin)
				return false;

			return true;
		}

		protected virtual void SOShipmentContact_RowSelected(PXCache sender, PXRowSelectedEventArgs e)
		{
			var contact = (SOShipmentContact)e.Row;
			if (contact == null)
				return;

			bool warnOnBusinessName = BusinessNameLengthIsExceeded(Document.Current, contact);
			var warnOnBusinessNameExc = !warnOnBusinessName ? null : new PXSetPropertyException(
				Messages.TooLongValueForUPS, PXErrorLevel.Warning,
				PXUIFieldAttribute.GetDisplayName(sender), MaxNameLengthForUps);

			sender.RaiseExceptionHandling(e.Row, contact.FullName, warnOnBusinessNameExc);
		}

		#endregion

		#region Processing

		/// 

		/// Gets graph instance required to process original document.
		/// 

		public virtual PXGraph GetOrigDocumentGraph(string origDocumentType)
		{
			var graph = CreateOrigDocumentGraph(origDocumentType);
			MergeStatusCachesBetweenGraphs(this, graph);
			InitOrigDocumentGraph(graph);
			return graph;
		}

		protected virtual PXGraph CreateOrigDocumentGraph(string origDocumentType)
		{
			throw new PXNotSupportedException();
		}

		protected virtual void InitOrigDocumentGraph(PXGraph graph)
		{
		}

		protected virtual Tuple GetBillToAddressContact()
		{
			return new (null, null);
		}

		public virtual void CorrectShipment(CorrectShipmentArgs args)
		{
			Clear();

			SOShipment shiporder = args.Shipment;
            Document.Current = Document.Search(shiporder.ShipmentNbr);
			if (WorkflowAction.HasWorkflowActionEnabled(this, g => g.correctShipmentAction, Document.Current) == false)
			{
				throw new PXInvalidOperationException(Messages.ActionNotAvailableInCurrentState,
					correctShipmentAction.GetCaption(), Document.Cache.GetRowDescription(Document.Current));
			}
			MarkOpen(Document.Current);

			Document.Cache.MarkUpdated(Document.Current, assertError: true);
            Document.Cache.IsDirty = true;

            using (PXTransactionScope ts = new PXTransactionScope())
			{
				UpdateOrigDocumentOnCorrectShipment(args);

				PXView shipLineSplitsToCorrect = new PXView(this, false, GetShipLineSplitsToCorrectCommand(args));
				foreach (PXResult res in shipLineSplitsToCorrect.SelectMultiBound(new object[] { Document.Current }))
				{
					INItemPlan plan = GetUpdatedPlanByShipLineSplit(args, res);

					this.Caches[typeof(INItemPlan)].Update(plan);
				}

				UpdateShipLinesOnCorrectShipment(args);

				AfterCorrectShipment();

				SOShipment.Events
					.Select(e => e.ShipmentCorrected)
					.FireOn(this, Document.Current);
				Save.Press();

				ts.Complete();
				Document.Cache.RestoreCopy(shiporder, Document.Current);
			}
		}

		protected virtual void UpdateOrigDocumentOnCorrectShipment(CorrectShipmentArgs args)
		{
		}

		protected virtual BqlCommand GetShipLineSplitsToCorrectCommand(CorrectShipmentArgs args)
		{
			return BqlCommand.CreateInstance(typeof(
				Select2>>,
				Where>>>));
		}

		protected virtual INItemPlan GetUpdatedPlanByShipLineSplit(CorrectShipmentArgs args, PXResult pxResult)
		{
			SOShipLineSplit split = PXResult.Unwrap(pxResult);

			split.Confirmed = false;
			if (args.ShipLinesClearedSOAllocation.Contains(split.LineNbr))
			{
				split.OrigPlanType = INPlanConstants.Plan60;
			}
			Caches[typeof(SOShipLineSplit)].MarkUpdated(split, assertError: true);
			Caches[typeof(SOShipLineSplit)].IsDirty = true;

			INItemPlan plan = PXCache.CreateCopy(pxResult);
			plan.PlanType = split.PlanType;
			plan.OrigPlanType = split.OrigPlanType;
			return plan;
		}

		protected virtual void UpdateShipLinesOnCorrectShipment(CorrectShipmentArgs args)
		{
			foreach (var soshipline in Transactions.Select())
			{
				SOShipLine shiplinecopy = PXCache.CreateCopy(soshipline);
				CorrectShipLine(args.ShipLinesClearedSOAllocation, shiplinecopy);

				this.Caches[typeof(SOShipLine)].Update(shiplinecopy);
			}
		}

		public virtual void CorrectShipLine(HashSet shipLinesClearedSOAllocation, SOShipLine shiplinecopy)
		{
			shiplinecopy.Confirmed = false;
			shiplinecopy.InvoiceGroupNbr = null;
			if (shipLinesClearedSOAllocation.Contains(shiplinecopy.LineNbr))
			{
				shiplinecopy.OrigPlanType = INPlanConstants.Plan60;
			}
		}

		protected virtual void AfterCorrectShipment()
		{
		}

		public virtual bool TryCompleteWorksheet(SOPickingWorksheet worksheet)
			=> TryCompleteWorksheet(this, worksheet);

		public virtual bool TryCompleteWorksheet(PXGraph graph, SOPickingWorksheet worksheet)
		{
			bool needToCompleteWorksheet =
				worksheet.Status.IsNotIn(SOPickingWorksheet.status.Completed, SOPickingWorksheet.status.Cancelled) &&
				SelectFrom.
				Where>.
				View.Select(graph, worksheet.WorksheetNbr)
				.RowCast()
				.AsEnumerable()
				.All(sh => sh.Confirmed == true);
			if (needToCompleteWorksheet)
			{
				worksheet.Status = SOPickingWorksheet.status.Completed;
				graph.Caches().Update(worksheet);
				graph.EnsureCachePersistence();

				var pickingJobs =
					SelectFrom.
					Where>.
					View.Select(graph, worksheet.WorksheetNbr);
				foreach (SOPickingJob job in pickingJobs)
				{
					job.Status = SOPickingJob.status.Completed;
					graph.Caches().Update(job);
					graph.EnsureCachePersistence();
				}

				return true;
			}
			return false;
		}

		public virtual void UpdateOrigValues(SOShipLine shipline, SOLine soline, decimal? baseOrigQty)
		{
		}

		public virtual IEnumerable> CollectDropshipDetails(SOOrderShipment shipment)
		{
			foreach (PXResult line in PXSelectJoin, And,
					And>>>,
				InnerJoin>>,
				Where,
					And>,
					And>,
					And>,
					And>>>>>>>
				.SelectMultiBound(this,
					new object[] { shipment },
					shipment.Operation == SOOperation.Receipt ? POReceiptType.POReturn : POReceiptType.POReceipt,
					shipment.ShipmentNbr
				).AsEnumerable()
				.Cast>())
			{
				yield return new PXResult(SOShipLine.FromDropShip(line, line), line);
			}
		}

		public virtual INRegisterEntryFactory CreateINRegisterFactory()
		{
			return new INRegisterEntryFactory(this);
		}

		public void MergeCachesWithINRegisterEntry(INRegisterEntryBase graph)
		{
			MergeStatusCachesBetweenGraphs(this, graph);
		}

		protected virtual void MergeStatusCachesBetweenGraphs(PXGraph source, PXGraph target)
		{
			target.Caches[typeof(SiteStatusByCostCenter)] = source.Caches[typeof(SiteStatusByCostCenter)];
			target.Caches[typeof(LocationStatusByCostCenter)] = source.Caches[typeof(LocationStatusByCostCenter)];
			target.Caches[typeof(LotSerialStatusByCostCenter)] = source.Caches[typeof(LotSerialStatusByCostCenter)];
			target.Caches[typeof(SiteLotSerial)] = source.Caches[typeof(SiteLotSerial)];
			target.Caches[typeof(ItemLotSerial)] = source.Caches[typeof(ItemLotSerial)];

			target.Views.Caches.Remove(typeof(SiteStatusByCostCenter));
			target.Views.Caches.Remove(typeof(LocationStatusByCostCenter));
			target.Views.Caches.Remove(typeof(LotSerialStatusByCostCenter));
			target.Views.Caches.Remove(typeof(SiteLotSerial));
			target.Views.Caches.Remove(typeof(ItemLotSerial));
		}

		#endregion

		#region Packaging into boxes

		protected virtual SOPackageEngine CreatePackageEngine()
		{
			return new SOPackageEngine(this);
		}

		#endregion

		protected virtual void CheckLocationTaskRule(PXCache sender, SOShipLine row)
		{
			if (row.TaskID != null)
			{
				INLocation selectedLocation = INLocation.PK.Find(this, row.LocationID);

				if (selectedLocation != null && selectedLocation.TaskID != row.TaskID && selectedLocation.TaskID != null)
				{
					sender.RaiseExceptionHandling(row, selectedLocation.LocationCD,
						new PXSetPropertyException(IN.Messages.LocationIsMappedToAnotherTask, PXErrorLevel.Warning));
				}
			}
		}

		[Obsolete]
		protected virtual void CheckSplitsForSameTask(PXCache sender, SOShipLine row)
		{
        }

		public virtual void ShipPackages(SOShipment shiporder)
		{
			var carrier = Carrier.PK.Find(this, shiporder.ShipVia);

			if (carrier?.IsActive == false)
			{
				throw new PXException(Messages.ShipViaNotActive);
			}

			if (!UseCarrierService(shiporder, carrier))
				return;

			CarrierPlugin plugin = null;

			if (carrier.IsExternal == true)
			{
				plugin = CarrierPlugin.PK.Find(this, carrier.CarrierPluginID);
				if (plugin?.SiteID != null && plugin.SiteID != shiporder.SiteID)
				{
					throw new PXException(Messages.ShipViaNotApplicableToShipment, Document.Cache.GetValueExt(shiporder));
				}
			}

			if (shiporder.ShippedViaCarrier != true)
			{
				ICarrierService cs = CarrierMaint.CreateCarrierService(this, shiporder.ShipVia);
				CarrierRequest cr = CarrierRatesExt.BuildRequest(shiporder);
				if (cr.Packages.Count > 0)
				{
					CarrierResult result = cs.Ship(cr);

					if (result != null)
					{
						StringBuilder sb = new StringBuilder();
						foreach (Message message in result.Messages)
						{
							sb.AppendFormat("{0}:{1} ", message.Code, message.Description);
						}

						if (result.IsSuccess)
						{
							using (PXTransactionScope ts = new PXTransactionScope())
							{
								PXTransactionScope.SetSuppressWorkflow(true);
								//re-read document, do not use passed object because it contains fills from Automation that will be committed even
								//if shipment confirmation will fail later.
								Document.Current = Document.Search(shiporder.ShipmentNbr);

								decimal freightCost = 0;

								if (shiporder.UseCustomerAccount != true && (shiporder.GroundCollect != true || !this.CanUseGroundCollect(shiporder)))
								{
									freightCost = ConvertAmtToBaseCury(result.Result.Cost.Currency, arsetup.Current.DefaultRateTypeID, shiporder.ShipDate.Value, result.Result.Cost.Amount);
								}

								Document.Current.FreightCost = freightCost;
								PXCurrencyAttribute.CuryConvCury(Document.Cache, Document.Current);

								if (Document.Current.OverrideFreightAmount != true)
								{
									if (result.Result.Price == null)
									{
										PXResultset res = Transactions.Select();
										FreightCalculator fc = CreateFreightCalculator();
										fc.ApplyFreightTerms(Document.Cache, Document.Current, res.Count);
									}
									else
									{
										Document.Current.FreightAmt = ConvertAmtToBaseCury(result.Result.Price.Currency, arsetup.Current.DefaultRateTypeID, Document.Current.ShipDate.Value, result.Result.Price.Amount);
										PXCurrencyAttribute.CuryConvCury(Document.Cache, Document.Current);
									}
								}

								Document.Current.ShippedViaCarrier = true;
								Document.Current.FreightCostIsValid = true;

								UploadFileMaintenance upload = PXGraph.CreateInstance();

								if (result.Result.Image != null)
								{
									string fileName = string.Format("High Value Report.{0}", result.Result.Format);
									FileInfo file = new FileInfo(fileName, null, result.Result.Image);
									try
									{
										upload.SaveFile(file, FileExistsAction.CreateVersion);
									}
									catch (PXNotSupportedFileTypeException exc)
									{
										throw new PXException(exc, Messages.NotSupportedFileTypeFromCarrier, result.Result.Format);
									}
									PXNoteAttribute.SetFileNotes(Document.Cache, Document.Current, file.UID.Value);
								}

								if (result.Result.AttachedFiles != null)
								{
									foreach (CarrierFileInfo info in result.Result.AttachedFiles)
									{
										string fileName = string.Format("{0}.{1}", info.Name, info.Format);
										FileInfo file = new FileInfo(fileName, null, info.Data);

										try
										{
											upload.SaveFile(file, FileExistsAction.CreateVersion);
										}
										catch (PXNotSupportedFileTypeException ex)
										{
											throw new PXException(ex, Messages.NotSupportedFileTypeFromCarrier, info.Format);
										}

										PXNoteAttribute.SetFileNotes(Document.Cache, Document.Current, file.UID.Value);
									}
								}

								Document.Update(Document.Current);

								foreach (PackageData pd in result.Result.Data)
								{
									SOPackageDetailEx sdp = PXSelect>,
										And>>>>.Select(this, shiporder.ShipmentNbr, pd.RefNbr);

									if (sdp != null)
									{
										if (pd.Image != null)
										{
											string fileName = string.Format("Label #{0}.{1}", pd.TrackingNumber, pd.Format);
											FileInfo file = new FileInfo(fileName, null, pd.Image);
											try
											{
												upload.SaveFile(file);
											}
											catch (PXNotSupportedFileTypeException exc)
											{
												throw new PXException(exc, Messages.NotSupportedFileTypeFromCarrier, pd.Format);
											}
											PXNoteAttribute.SetFileNotes(Packages.Cache, sdp, file.UID.Value);

											var pluginMethod = PXSelectorAttribute.Select(this.carrier.Cache, carrier) as CarrierMethodSelectorAttribute.CarrierPluginMethod;
											string serviceMethod = $"{carrier.PluginMethod} - {pluginMethod?.Description}";
											if (serviceMethod.Length > CarrierLabelHistory.serviceMethod.Length)
											{
												serviceMethod = serviceMethod.Substring(0, CarrierLabelHistory.serviceMethod.Length);
											}
											decimal rateAmount = ConvertAmtToBaseCury(result.Result.Cost.Currency, arsetup.Current.DefaultRateTypeID, shiporder.ShipDate.Value, pd.RateAmount);

											LabelHistory.Insert(new CarrierLabelHistory()
											{
												ShipmentNbr = shiporder.ShipmentNbr,
												LineNbr = pd.RefNbr,
												PluginTypeName = plugin?.PluginTypeName,
												ServiceMethod = serviceMethod,
												RateAmount = rateAmount
											});
										}
										sdp.TrackNumber = pd.TrackingNumber;
										sdp.TrackUrl = pd.TrackingUrl;
										sdp.TrackData = pd.TrackingData;
										Packages.Update(sdp);
									}
								}

								this.Save.Press();
								ts.Complete();
							}
							Document.Cache.RestoreCopy(shiporder, Document.Current);

							//show warnings:
							if (result.Messages.Count > 0)
							{
								Document.Cache.RaiseExceptionHandling(shiporder, shiporder.CuryFreightCost,
									new PXSetPropertyException(sb.ToString(), PXErrorLevel.Warning));

								PXTrace.WriteWarning(sb.ToString());
							}

						}
						else
						{
							if (!string.IsNullOrEmpty(result.RequestData))
								PXTrace.WriteError(result.RequestData);

							Document.Cache.RaiseExceptionHandling(shiporder, shiporder.CuryFreightCost,
									new PXSetPropertyException(Messages.CarrierServiceError, PXErrorLevel.Error, sb.ToString()));

							throw new PXException(Messages.CarrierServiceError, sb.ToString());
						}

					}
				}
			}
		}

		protected virtual FreightCalculator CreateFreightCalculator()
		{
			return new FreightCalculator(this);
		}

		public virtual void CancelPackages(SOShipment shiporder, bool isReturn = false)
		{
			if (shiporder.ShippedViaCarrier == true && IsWithLabels(shiporder.ShipVia))
			{
                SOShipment currentShipment = Document.Search(shiporder.ShipmentNbr);

                ICarrierService cs = CarrierMaint.CreateCarrierService(this, currentShipment.ShipVia);

				SOPackageDetailEx sdp = PXSelect>>>.SelectWindowed(this, 0, 1, currentShipment.ShipmentNbr);

				string trackNumber = isReturn ? sdp.ReturnTrackNumber : sdp.TrackNumber;

				if (sdp != null && !string.IsNullOrEmpty(trackNumber))
				{
					CarrierResult result = cs.Cancel(trackNumber, sdp.TrackData);

					if (result != null)
					{
						StringBuilder sb = new StringBuilder();
						foreach (Message message in result.Messages)
						{
							sb.AppendFormat("{0}:{1} ", message.Code, message.Description);
						}

						//Clear Tracking numbers no matter where the call to the carrier were successfull or not

						foreach (SOPackageDetailEx pd in PXSelect>>>.Select(this, currentShipment.ShipmentNbr))
						{
							pd.Confirmed = false;
							if (!isReturn)
								pd.TrackNumber = null;
							pd.ReturnTrackNumber = null;
							pd.TrackUrl = null;
							Packages.Update(pd);

							foreach (NoteDoc nd in PXSelect>>>.Select(this, pd.NoteID))
							{
								UploadFileMaintenance.DeleteFile(nd.FileID);
							}
						}

                        currentShipment.CuryFreightCost = 0;
						if (currentShipment.OverrideFreightAmount != true)
						{
							currentShipment.CuryFreightAmt = 0;
						}
                        currentShipment.ShippedViaCarrier = false;
						Document.Update(currentShipment);
						Document.Cache.RestoreCopy(shiporder, Document.Current);

						this.Save.Press();

						//Log errors if any: (Log Errors/Warnings to Trace do not return them - In processing warning are displayed as errors (( )
						//CancelPackages should not throw Exceptions since CorrectShipment follows it and must be executed.
						if (!result.IsSuccess)
						{
							//Document.Cache.RaiseExceptionHandling(shiporder, shiporder.CuryFreightCost,
							//        new PXSetPropertyException(Messages.CarrierServiceError, PXErrorLevel.Error, sb.ToString()));

							//throw new PXException(Messages.CarrierServiceError, sb.ToString());

							PXTrace.WriteWarning("Tracking Numbers and Labels for the shipment was succesfully cleared but Carrier Void Service Returned Error: " + sb.ToString());
						}
						else
						{
							//show warnings:
							if (result.Messages.Count > 0)
							{
								//Document.Cache.RaiseExceptionHandling(shiporder, shiporder.CuryFreightCost,
								//    new PXSetPropertyException(sb.ToString(), PXErrorLevel.Warning));

								PXTrace.WriteWarning("Tracking Numbers and Labels for the shipment was succesfully cleared but Carrier Void Service Returned Warnings: " + sb.ToString());
							}
						}
					}
				}
			}
		}

		protected static System.Threading.Tasks.Task PrintPickListOperation(List list, PXAdapter uiAdapter, CancellationToken cancellationToken)
		{
			var shipmentEntry = CreateInstance();
			return shipmentEntry.PrintPickList(list, uiAdapter.CloneTo(shipmentEntry), cancellationToken);
		}

		[Obsolete]
		protected virtual System.Threading.Tasks.Task PrintPickList(List list, CancellationToken cancellationToken) => PrintPickList(list, null, cancellationToken);
		protected virtual async System.Threading.Tasks.Task PrintPickList(List list, PXAdapter adapter, CancellationToken cancellationToken)
		{
			if (list.Count == 0)
				return;

			PXProcessing.ProcessRecords(list, adapter.MassProcess, sh =>
			{
				SOShipment shipment = Document.Search(sh.ShipmentNbr);

				shipment.PickListPrinted = true;
				shipment = Document.Update(shipment);

				if (shipment.Hold == true)
					releaseFromHold.PressWithSuppressedWorkflowPersist();
			});

			PXReportRequiredException ex = null;
			void shipmentPersisted(PXCache sender, PXRowPersistedEventArgs e)
			{
				if (e.TranStatus == PXTranStatus.Completed && e.Row is SOShipment shipment && shipment.PickListPrinted == true)
				{
					using (new PXReadBranchRestrictedScope())
					{
						var graph = (SOShipmentEntry)sender.Graph;

						GL.Branch siteBranch = PXResult.Unwrap(graph.Company.View.SelectSingleBound(new[] { shipment }));

						Dictionary parameters = new() { ["SOShipment.ShipmentNbr"] = shipment.ShipmentNbr };
						string actualReportID = new NotificationUtility(graph).SearchCustomerReport(SOReports.PrintPickList, shipment.CustomerID, siteBranch.BranchID);

						ex = PXReportRequiredException.CombineReport(ex, actualReportID, parameters);
						ex.Mode = PXBaseRedirectException.WindowMode.New;
					}
				}
			}

			using (new SimpleScope(
				() => RowPersisted.AddHandler(shipmentPersisted),
				() => RowPersisted.RemoveHandler(shipmentPersisted)))
			{
				Save.Press();
			}

			if (adapter?.MassProcess == true)
				list.ForEach(sh => Document.Cache.RestoreCopy(sh, SOShipment.PK.Find(this, sh.ShipmentNbr)));

			if (ex != null)
			{
				if (PXAccess.FeatureInstalled())
					await SMPrintJobMaint.CreatePrintJobGroup(
						adapter,
						new NotificationUtility(this).SearchPrinter,
						SONotificationSource.Customer,
						SOReports.PrintPickList,
						Accessinfo.BranchID, ex,
						ShipmentActions.Messages.PrintPickList, cancellationToken);

				throw ex;
			}
		}

		protected PXAdapter CreateDummyAdapter()
		{
			return new PXAdapter(PXView.Dummy.For(this))
			{
				MassProcess = true, //Device Hub require this flag to know if supported
				Arguments =
							{
								[nameof(IPrintable.PrintWithDeviceHub)] = true,
								[nameof(IPrintable.DefinePrinterManually)] = false
							}
			};
		}

		protected virtual bool IsWithLabels(string shipVia)
		{
			Carrier carrier = Carrier.PK.Find(this, shipVia);
			return carrier != null && carrier.IsExternal == true;
		}

		protected virtual bool ValidateAvailablePackages()
		{
			if (string.IsNullOrEmpty(Document.Current.ShipVia))
				return false;

			var boxes = CreatePackageEngine()
				.GetBoxesByCarrierID(Document.Current.ShipVia)
				.Select(b => b.BoxID)
				.ToHashSet();

			foreach (SOPackageDetail package in Packages.Select())
			{
				if (!boxes.Contains(package.BoxID))
					return false;
			}

			return true;
		}

		public bool IsPPS
		{
			get
			{
				return this.FindImplementation() != null;
			}
		}

		protected virtual void ValidateShipComplete(SOShipment shipment)
		{
		}

		protected decimal ConvertAmtToBaseCury(string from, string rateType, DateTime effectiveDate, decimal amount)
		{
			decimal result = amount;

			using (ReadOnlyScope rs = new ReadOnlyScope(DummyCuryInfo.Cache))
			{
				CurrencyInfo ci = new CurrencyInfo();
				ci.CuryRateTypeID = rateType;
				ci.CuryID = from;
				ci = (CurrencyInfo)DummyCuryInfo.Cache.Insert(ci);
				ci.SetCuryEffDate(DummyCuryInfo.Cache, effectiveDate);
				DummyCuryInfo.Cache.Update(ci);
				PXCurrencyAttribute.CuryConvBase(DummyCuryInfo.Cache, ci, amount, out result);
				DummyCuryInfo.Cache.Delete(ci);
			}

			return result;
		}

		public virtual decimal GetQtyThreshold(SOShipLineSplit sosplit)
		{
			decimal threshold =
				SelectFrom
				.InnerJoin.On
				.Where
					.And>>
				.View.Select(this, sosplit.ShipmentNbr, sosplit.LineNbr)
				.TopFirst?.CompleteQtyMax ?? 100m;
			return threshold / 100m;
		}

		public virtual decimal GetMinQtyThreshold(SOShipLineSplit sosplit)
		{
			decimal threshold =
				SelectFrom
				.InnerJoin.On
				.Where
					.And>>
				.View.Select(this, sosplit.ShipmentNbr, sosplit.LineNbr)
				.TopFirst?.CompleteQtyMin ?? 100m;
			return threshold / 100m;
		}

		protected virtual void MarkOpen(SOShipment shipment)
		{
			shipment.Confirmed = false;
			shipment.ConfirmedToVerify = true;
			shipment.Status = SOShipmentStatus.Open;

			shipment.LabelsPrinted = false;
			shipment.CommercialInvoicesPrinted = false;
		}

		[PXInternalUseOnly]
		protected virtual void SetSuppressWorkflowOnCorrectShipment()
			=> PXTransactionScope.SetSuppressWorkflow(true);

		public class LineShipment : IEnumerable, ICollection
		{
			private List _List = new List();
			public bool AnyDeleted = false;

			#region Ctor
			public LineShipment()
			{
			}
			#endregion
			#region Implementation
			public int Count
			{
				get
				{
					return ((ICollection)_List).Count;
				}
			}

			public bool IsReadOnly
			{
				get
				{
					return ((ICollection)_List).IsReadOnly;
				}
			}

			public IEnumerator GetEnumerator()
			{
				return ((IEnumerable)_List).GetEnumerator();
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return ((IEnumerable)_List).GetEnumerator();
			}

			public void Clear()
			{
				((ICollection)_List).Clear();
			}

			public bool Contains(SOShipLine item)
			{
				return ((ICollection)_List).Contains(item);
			}

			public void CopyTo(SOShipLine[] array, int arrayIndex)
			{
				((ICollection)_List).CopyTo(array, arrayIndex);
			}

			public bool Remove(SOShipLine item)
			{
				return ((ICollection)_List).Remove(item);
			}

			public void Add(SOShipLine item)
			{
				((ICollection)_List).Add(item);
			}
			#endregion
		}

		#region Well-known extension
		public PackageDetail PackageDetailExt => FindImplementation();
		// Acuminator disable once PX1016 ExtensionDoesNotDeclareIsActiveMethod extension should be constantly active
		public class PackageDetail : PXGraphExtension
		{
			public PXSelect>,
				And>>>> PackageDetailSplit;

			protected virtual void _(Events.RowSelected e)
			{
				PackageDetailSplit.Cache.AllowInsert = Base.Packages.AllowInsert && e.Row != null;
				PackageDetailSplit.AllowDelete = Base.Packages.AllowDelete;
				PackageDetailSplit.AllowSelect = Base.Packages.AllowSelect;
				PackageDetailSplit.AllowUpdate = Base.Packages.AllowUpdate;
			}

			protected virtual void _(Events.RowSelected e)
			{
				if (e.Row == null)
					return;

				PXUIFieldAttribute.SetEnabled(e.Cache, e.Row, Base.IsImport);
				Exception packageException = null;

				if (e.Row.IsPackageContentDeleted == true)
					packageException = new PXSetPropertyException(e.Row, Messages.PackageContentDeleted, PXErrorLevel.Warning);

				e.Cache.RaiseExceptionHandling(e.Row, null, packageException);
			}

			protected virtual void _(Events.RowInserted e)
			{
				Base.Document.Current.IsPackageContentDeleted = false;
				UpdateParentShipmentLine(e.Cache, e.Row, null);
			}
			protected virtual void _(Events.RowUpdated e) => UpdateParentShipmentLine(e.Cache, e.Row, e.OldRow);
			protected virtual void _(Events.RowDeleted e) => UpdateParentShipmentLine(e.Cache, null, e.Row);

			protected virtual void _(Events.FieldUpdated e)
			{
				if (e.Row != null)
				{
					var shipmentLineSplit = PXParentAttribute.SelectParent(e.Cache, e.Row);
					e.Row.InventoryID = shipmentLineSplit?.InventoryID;
					e.Row.UOM = shipmentLineSplit?.UOM;
					e.Row.PackedQty = shipmentLineSplit?.Qty - shipmentLineSplit?.PackedQty;
				}
			}

			private class RowPersistingScope : FlaggedModeScopeBase { }

			protected virtual void _(Events.FieldVerifying e)
			{
				if (e.Row == null || e.NewValue == null) return;

				if (!Base.IsContractBasedAPI || RowPersistingScope.IsActive)
				{
				decimal adjustment = (decimal)e.NewValue - e.Row.PackedQty.GetValueOrDefault();
				var shipmentLineSplit = PXParentAttribute.SelectParent(e.Cache, e.Row);
				if (shipmentLineSplit != null && shipmentLineSplit.PackedQty + adjustment > shipmentLineSplit.Qty * Base.GetQtyThreshold(shipmentLineSplit))
					throw new PXSetPropertyException(e.Row, Messages.QuantityPackedExceedsShippedQuantityForLine);
			}
			}

			protected virtual void _(Events.RowPersisting e)
			{
				if (e.Operation.Command().IsIn(PXDBOperation.Insert, PXDBOperation.Update))
				{
					using (new RowPersistingScope())
						e.Cache.VerifyFieldAndRaiseException(e.Row);
				}
			}

			protected void UpdateParentShipmentLine(PXCache sender, SOShipLineSplitPackage row, SOShipLineSplitPackage oldRow)
			{
				if (row != null && oldRow != null && row.ShipmentLineNbr == oldRow.ShipmentLineNbr)
				{
					var shipmentLineSplit = PXParentAttribute.SelectParent(sender, row);
					if (shipmentLineSplit != null)
					{
						if (row.ShipmentSplitLineNbr == oldRow.ShipmentSplitLineNbr)
						{
						UpdateShipmentLine(sender, shipmentLineSplit, row, row.PackedQty.GetValueOrDefault() - oldRow.PackedQty.GetValueOrDefault());
					}
						else
						{
							UpdateShipmentLine(sender, shipmentLineSplit, row, row.PackedQty.GetValueOrDefault());

							var oldShipmentLineSplit = PXParentAttribute.SelectParent(sender, oldRow);
							if (oldShipmentLineSplit != null)
								UpdateShipmentLine(sender, oldShipmentLineSplit, oldRow, -oldRow.PackedQty.GetValueOrDefault());
						}
					}
				}
				else
				{
					if (row != null)
					{
						var shipmentLineSplit = PXParentAttribute.SelectParent(sender, row);
						if (shipmentLineSplit != null)
						{
							var shipmentLine = PXParentAttribute.SelectParent(Base.splits.Cache, shipmentLineSplit);
							var lineItem = InventoryItem.PK.Find(Base, shipmentLine.InventoryID);

							decimal unitPrice = shipmentLine.UnitPrice ?? 0m;
							decimal factor = 1m;
							if (lineItem.StkItem != true && lineItem.KitItem == true)
							{
								var kitComponentsCount = GetNonStockKitComponentsCount(shipmentLine, lineItem);
								factor = kitComponentsCount != 0m ? kitComponentsCount : 1m;
								unitPrice = INUnitAttribute.ConvertFromBase(
									Base.Transactions.Cache,
									shipmentLine,
									shipmentLine.UOM,
									unitPrice,
									INPrecision.NOROUND);
							}
							else
							{
								unitPrice = INUnitAttribute.ConvertFromTo(
									Base.Transactions.Cache,
									shipmentLine,
									shipmentLineSplit.UOM,
									shipmentLine.UOM,
									unitPrice,
									INPrecision.NOROUND);
							}

							row.UnitPriceFactor = PXDBPriceCostAttribute.Round(unitPrice * (1m - (shipmentLine.DiscPct ?? 0m)/100m) / factor);
							row.WeightFactor = factor;

							UpdateShipmentLine(sender, shipmentLineSplit, row, row.PackedQty.GetValueOrDefault());
						}
					}

					if (oldRow != null)
					{
						var shipmentLineSplit = PXParentAttribute.SelectParent(sender, oldRow);
						if (shipmentLineSplit != null)
						{
							UpdateShipmentLine(sender, shipmentLineSplit, row, -oldRow.PackedQty.GetValueOrDefault());
						}
					}
				}
				if (row != null && oldRow != null && row.PackedQty != oldRow.PackedQty)
				{
					Base.ResetFreightCostIsValid(Base.Document.Current);
				}
			}

			protected virtual decimal GetNonStockKitComponentsCount(SOShipLine shipmentLine, InventoryItem item)
			{
				if ((shipmentLine.BaseShippedQty ?? 0m) == 0m)
					return 0m;

				var lineSplits = PXParentAttribute.SelectChildren(Base.splits.Cache, shipmentLine, typeof(SOShipLine));
				return lineSplits.Sum(s => ((SOShipLineSplit)s).Qty ?? 0m) / shipmentLine.BaseShippedQty.Value;
			}

			protected void UpdateShipmentLine(PXCache sender, SOShipLineSplit shipmentLineSplit, SOShipLineSplitPackage packageDetailSplit, decimal adjustment)
			{
				if (adjustment != 0)
				{
					bool syncPickedWithPacked =
						adjustment > 0 && shipmentLineSplit.PackedQty.GetValueOrDefault() + adjustment > shipmentLineSplit.PickedQty.GetValueOrDefault() ||
						adjustment < 0 && (PXAccess.FeatureInstalled() == false || SOPickPackShipSetup.PK.Find(Base, Base.Accessinfo.BranchID)?.ShowPickTab == false);

					UpdatePickPackInfoOf(
						shipmentLineSplit,
						split => split.PackedQty.GetValueOrDefault() + adjustment,
						syncPickedWithPacked,
						raiseRowUpdated: true);

					UpdatePickPackInfoOf(
						PXParentAttribute.SelectParent(Base.splits.Cache, shipmentLineSplit),
						line =>
						{
							var splits = PXParentAttribute.SelectChildren(Base.splits.Cache, line);

							NonStockKitSpecHelper kitSpecHelper = new(Base);
							if (kitSpecHelper.IsNonStockKit(line.InventoryID))
							{
								var RequireShipping = Func.Memorize((int inventoryID) => InventoryItem.PK.Find(Base, inventoryID).With(item => item.StkItem == true || item.NonStockShip == true));

								// kitInventoryID -> compInventory -> qty
								var nonStockKitSpec = kitSpecHelper.GetNonStockKitSpec(line.InventoryID.Value).Where(pair => RequireShipping(pair.Key)).ToDictionary();
								var nonStockKitSplits = splits.GroupBy(r => r.InventoryID.Value).ToDictionary(g => g.Key, g => g.Sum(s => s.PackedQty ?? 0));

								decimal integerKitQty = nonStockKitSpec.Keys.Count() == 0 || nonStockKitSpec.Keys.Except(nonStockKitSplits.Keys).Count() > 0
									? 0
									: (from split in nonStockKitSplits
									   join spec in nonStockKitSpec on split.Key equals spec.Key
									   select Math.Floor(decimal.Divide(split.Value, spec.Value))).Min();

								return integerKitQty;
							}
							else
							{
								return INUnitAttribute.ConvertFromBase(Base.Transactions.Cache, line.InventoryID, line.UOM, splits.Sum(s => s.PackedQty ?? 0), INPrecision.NOROUND);
							}
						},
						syncPickedWithPacked);


					UpdatePickPackInfoOf(
						PXParentAttribute.SelectParent(Base.splits.Cache, shipmentLineSplit),
						shipment => PXParentAttribute.SelectChildren(Base.Transactions.Cache, shipment).Sum(l => l.PackedQty ?? 0),
						syncPickedWithPacked);
				}

				void UpdatePickPackInfoOf(TEntity row, Func calculateNewPackedQty, bool syncPickedWithPacked, bool raiseRowUpdated = false)
					where TEntity : PXBqlTable, IBqlTable, new()
					where TPickedQtyField : IBqlField, IImplement
					where TPackedQtyField : IBqlField, IImplement
				{
					PXCache cache = Base.Caches();

					TEntity original = cache.Rows.CreateCopy(row);
					cache.MarkUpdated(row, assertError: true);

					var originalPackedQty = (decimal?)cache.GetValue(row);
					cache.SetValue(row, calculateNewPackedQty(row));
					cache.RaiseFieldUpdated(row, originalPackedQty);

					if (syncPickedWithPacked)
					{
						var originalPickedQty = (decimal?)cache.GetValue(row);
						cache.SetValue(row, cache.GetValue(row));
						cache.RaiseFieldUpdated(row, originalPickedQty);
					}

					if (raiseRowUpdated)
						cache.RaiseRowUpdated(row, original);
				}
			}

			/// Overrides 
			[PXOverride]
			public void ShipPackages(SOShipment shiporder,
				Action base_ShipPackages)
			{
				Carrier carrier = Carrier.PK.Find(Base, shiporder.ShipVia);
				if (carrier != null)
				{
					if (carrier.ValidatePackedQty == true)
					{
						ValidatePackagedQuantities(shiporder);
					}

					// Automatically print return label if enabled for selected ship via when issuing
					bool printRetLabel = carrier.IsExternal == true && shiporder.ShippedViaCarrier != true && carrier.ReturnLabel == true
						&& shiporder.Operation == SOOperation.Issue && shiporder.UnlimitedPackages != true;
					if (printRetLabel)
					{
						Base1.GetReturnLabels(shiporder);
					}
				}

				base_ShipPackages(shiporder);
			}

			protected virtual void ValidatePackagedQuantities(SOShipment shiporder)
			{
				Base.Document.Current = Base.Document.Search(shiporder.ShipmentNbr);
				if (Base.Document.Current.ShipmentType == SOShipmentType.Issue)
				{
					foreach (SOShipLine line in Base.Transactions.Select())
					{
						Base.Transactions.Current = Base.Transactions.Search(line.ShipmentNbr, line.LineNbr);
						if (line.LineType == SOLineType.Inventory)
						{
							if (line.BaseShippedQty != line.BasePackedQty)
							{
								InventoryItem item = InventoryItem.PK.Find(Base, line.InventoryID);

								if (item.StkItem == true || item.KitItem != true)
								throw new PXException(Messages.ShipmentLineQuantityNotPacked, item?.InventoryCD.Trim());
							}

							foreach (SOShipLineSplit split in Base.splits.Select())
							{
								if (split.BaseQty != split.BasePackedQty)
								{
									InventoryItem item = InventoryItem.PK.Find(Base, line.InventoryID);
									throw new PXException(Messages.ShipmentLineQuantityNotPacked, item?.InventoryCD.Trim());
								}
							}
						}
					}
				}
			}

			public virtual void OnBeforeRecalculatePackages(Document doc)
			{
				Base.Document.Current.IsPackageContentDeleted = false;
			}

			public virtual void OnAutoPackageContentDeleted(SOShipLineSplitPackage row)
			{
				Base.Document.Current.IsPackageContentDeleted = true;
			}
		}

		public CarrierRates CarrierRatesExt => FindImplementation();
		public class CarrierRates : CarrierRatesExtension
		{
			protected override DocumentMapping GetDocumentMapping() => new DocumentMapping(typeof(SOShipment)) { DocumentDate = typeof(SOShipment.shipDate) };
			protected override DocumentPackageMapping GetDocumentPackageMapping() => new DocumentPackageMapping(typeof(SOPackageDetailEx)) { };

			protected override void CalculateFreightCost(Document doc)
			{
				Base.CalculateFreightCost(true);
			}

			protected override void UpdatePackageWeightFromScale(decimal? weight)
			{
				Base.Packages.Current.Weight = weight;
				Base.Packages.Update(Base.Packages.Current);
			}

			public virtual CarrierRequest BuildRateRequest(SOShipment order) => base.BuildRateRequest(Documents.Cache.GetExtension(order));
			protected override CarrierRequest GetCarrierRequest(Document doc, UnitsType unit, List methods, List boxes)
			{
				var shipment = (SOShipment)Documents.Cache.GetMain(doc);

				SOShipmentAddress shipAddress = Base.Shipping_Address.Select();
				BAccount companyAccount = PXSelectJoin>>, Where>>>.Select(Base, Base.Accessinfo.BranchID);
				Address companyAddress = PXSelect>>>.Select(Base, companyAccount.DefAddressID);
				SOShipmentContact shipContact = Base.Shipping_Contact.Select();
				Contact companyContact = PXSelect>>>.Select(Base, companyAccount.DefContactID);

				CarrierRequest cr = new CarrierRequest(unit, shipment.CuryID);
				cr.Shipper = companyAddress;
				cr.Origin = null;
				cr.Destination = shipAddress;
				cr.PackagesEx = boxes;
				cr.Resedential = shipment.Resedential == true;
				cr.SaturdayDelivery = shipment.SaturdayDelivery == true;
				cr.Insurance = shipment.Insurance == true;
				cr.ShipDate = Tools.Max(Base.Accessinfo.BusinessDate.Value.Date, shipment.ShipDate.Value);
				cr.Methods = methods;
				cr.Attributes = new List();
				cr.InvoiceLineTotal = Base.Document.Current.LineTotal.GetValueOrDefault();
				cr.ShipperContact = companyContact;
				cr.DestinationContact = shipContact;

				if (shipment.GroundCollect == true && Base.CanUseGroundCollect(shipment))
					cr.Attributes.Add("COLLECT");

				return cr;
			}

			protected override IEnumerable> GetLines(Document doc)
			{
				var shipment = (SOShipment)Documents.Cache.GetMain(doc);

				return
					PXSelectJoin>,
					Where,
					OrderBy>>>>
					.SelectMultiBound(Base, new object[] { shipment }).AsEnumerable()
					.Cast>()
					.Select(r => Tuple.Create(new LineInfo(r), r));
			}

			protected override IList GetPackages(Document doc, bool suppressRecalc = false)
			{
				var shipment = (SOShipment)Documents.Cache.GetMain(doc);

				SOPackageEngine.PackSet set = new SOPackageEngine.PackSet(shipment.SiteID.Value);
				foreach (SOPackageDetailEx package in Base.Packages.View.SelectMultiBound(new object[] { shipment }))
					set.Packages.Add(package.ToPackageInfo(shipment.SiteID.Value));

				return set.AsSingleEnumerable().ToList();
			}

			protected override void ClearPackages(Document doc)
			{
				foreach (SOPackageDetailEx package in Base.Packages.View.SelectMultiBound(new object[] { Documents.Cache.GetMain(doc) }))
					Base.Packages.Delete(package);
			}

			protected override void InsertPackages(IEnumerable packages)
			{
				foreach (SOPackageInfoEx package in packages)
					Base.Packages.Insert(package.ToPackageDetail(SOPackageType.Auto).Apply(d => d.ShipmentNbr = Base.Document.Current.ShipmentNbr));
			}

			protected override void RecalculatePackagesForOrder(Document doc)
			{
				if (Base.Document.Current != null
					&& Base.Document.Current.UnlimitedPackages != true)
				{
					if (Base.Document.Current.Released == true || Base.Document.Current.Confirmed == true)
						throw new PXException(Messages.PackagesRecalcErrorReleasedDocument);

					if (Base.Document.Current.SiteID == null)
						throw new PXException(Messages.PackagesRecalcErrorWarehouseIdNotSpecified);

					Base.PackageDetailExt.OnBeforeRecalculatePackages(doc);

					PXRowDeleted packageContentDeleted = (s,e) =>
						Base.PackageDetailExt.OnAutoPackageContentDeleted((SOShipLineSplitPackage)e.Row);

					int packageCount = 0;
					decimal weightTotal = 0;
					SOPackageEngine.PackSet manualPackSet;
					IList packsets = CalculatePackages(Base.Document.Current, out manualPackSet);

					try
					{
						Base.RowDeleted.AddHandler(packageContentDeleted);

						foreach (SOPackageDetailEx package in Base.Packages.Select())
						{
							if (manualPackSet.Packages.Count == 0 && package.PackageType != SOPackageType.Auto)
							{
								weightTotal += package.Weight.GetValueOrDefault();
								packageCount++;
								continue;
							}
							Base.Packages.Delete(package);
						}
					}
					finally
					{
						Base.RowDeleted.RemoveHandler(packageContentDeleted);
					}

					foreach (SOPackageEngine.PackSet ps in packsets)
					{
						foreach (SOPackageInfoEx package in ps.Packages)
						{
							weightTotal += package.GrossWeight.GetValueOrDefault();

							SOPackageDetailEx detail = new SOPackageDetailEx();
							detail.PackageType = SOPackageType.Auto;
							detail.ShipmentNbr = Base.Document.Current.ShipmentNbr;
							detail.BoxID = package.BoxID;
							detail.Weight = package.GrossWeight;
							detail.WeightUOM = package.WeightUOM;
							detail.Qty = package.Qty;
							detail.QtyUOM = package.QtyUOM;
							detail.InventoryID = package.InventoryID;
							detail.DeclaredValue = package.DeclaredValue;

							detail = Base.Packages.Insert(detail);
							detail.Confirmed = false;
							packageCount++;
						}
					}

					foreach (SOPackageInfoEx package in manualPackSet.Packages)
					{
						weightTotal += package.GrossWeight.GetValueOrDefault();

						SOPackageDetailEx detail = new SOPackageDetailEx();
						detail.PackageType = SOPackageType.Manual;
						detail.ShipmentNbr = Base.Document.Current.ShipmentNbr;
						detail.BoxID = package.BoxID;
						detail.Weight = package.GrossWeight;
						detail.WeightUOM = package.WeightUOM;
						detail.Qty = package.Qty;
						detail.QtyUOM = package.QtyUOM;
						detail.InventoryID = package.InventoryID;
						detail.DeclaredValue = package.DeclaredValue;
						detail.Height = package.Height;
						detail.Width = package.Width;
						detail.Length = package.Length;

						detail = Base.Packages.Insert(detail);
						detail.Confirmed = false;
						packageCount++;
					}

					Base.Document.Current.IsPackageValid = true;
					Base.Document.Current.RecalcPackagesReason = SOShipment.recalcPackagesReason.None;
					Base.Document.Current.PackageWeight = weightTotal;
					Base.Document.Current.PackageCount = packageCount;

					Base.Document.Update(Base.Document.Current);
				}
			}

			protected virtual IList CalculatePackages(SOShipment shipment, out SOPackageEngine.PackSet manualPackSet)
			{
				SOOrderExtension ext = Base.GetExtension();

				Dictionary stats = new Dictionary();

				PXSelectBase selectManual = new PXSelect>,
						And>,
						And>>>>>(Base);

				SOPackageEngine.OrderInfo orderInfo = new SOPackageEngine.OrderInfo(shipment.ShipVia);

				manualPackSet = new SOPackageEngine.PackSet(shipment.SiteID.Value);
				List processedManualPackageOrders = new List();
				foreach (SOShipLine line in Base.Transactions.View.SelectMultiBound(new object[] { shipment }))
				{
					SOOrder order = PXParentAttribute.SelectParent(Base.Transactions.Cache, line);
					bool manualPackaging =
						PXAccess.FeatureInstalled() == false
						|| order?.IsManualPackage == true
						|| Base.Document?.Current?.UnlimitedPackages == true;
					if (manualPackaging)
					{
						string key = string.Format("{0}.{1}.{2}", order.OrderType, order.OrderNbr, shipment.SiteID);
						if (!processedManualPackageOrders.Contains(key))
						{
							foreach (SOPackageInfoEx box in selectManual.Select(order.OrderType, order.OrderNbr, shipment.SiteID))
							{
								// DeclaredValue from Sales Order should be converted to base currency.
								decimal baseCuryDeclaredValue;
								PXDBCurrencyAttribute.CuryConvBase(
									ext.soorder.Cache, order, box.DeclaredValue ?? 0m, out baseCuryDeclaredValue);

								box.DeclaredValue = baseCuryDeclaredValue;
								manualPackSet.Packages.Add(box);
							}
							processedManualPackageOrders.Add(key);
						}
					}
					else
					{
						InventoryItem item = InventoryItem.PK.Find(Base, line.InventoryID);

						if (item.PackageOption == INPackageOption.Manual)
							continue;

						orderInfo.AddLine(item, line.BaseQty);


						int inventoryID = item.PackSeparately == true
							? item.InventoryID.Value
							: SOPackageEngine.ItemStats.Mixed;

						string key = string.Format("{0}.{1}.{2}.{3}", line.SiteID, inventoryID, item.PackageOption, line.Operation);

						SOPackageEngine.ItemStats stat;
						if (stats.ContainsKey(key))
						{
							stat = stats[key];
							stat.BaseQty += line.BaseQty.GetValueOrDefault();
							stat.BaseWeight += line.ExtWeight.GetValueOrDefault();
							stat.DeclaredValue += line.LineAmt ?? 0m;
							stat.AddLine(item, line.BaseQty);
						}
						else
						{
							stat = new SOPackageEngine.ItemStats();
							stat.SiteID = line.SiteID;
							stat.InventoryID = inventoryID;
							stat.Operation = line.Operation;
							stat.PackOption = item.PackageOption;
							stat.BaseQty += line.BaseQty.GetValueOrDefault();
							stat.BaseWeight += line.ExtWeight.GetValueOrDefault();
							stat.DeclaredValue += line.LineAmt ?? 0m;
							stat.AddLine(item, line.BaseQty);
							stats.Add(key, stat);
						}
					}
				}
				orderInfo.Stats.AddRange(stats.Values);

				SOPackageEngine engine = CreatePackageEngine();
				return engine.Pack(orderInfo);
			}


			protected virtual IList GetPackages(SOShipment shiporder, Carrier carrier, CarrierPlugin plugin)
			{
				List list = new List();

				List packages = PXSelect>>>
					.Select(Base, shiporder.ShipmentNbr).RowCast().ToList();

				bool failed = false;
				List carrierPackages = GetCarrierPackageDetail(packages, carrier.CarrierID);

				foreach (SOCarrierPackageDetailEx pkgDetail in carrierPackages)
				{
					SOPackageDetailEx detail = pkgDetail.Package;
					if (carrier.ConfirmationRequired == true)
					{
						if (detail.Confirmed != true)
						{
							failed = true;

							Base.Packages.Cache.RaiseExceptionHandling(detail, detail.Confirmed,
								new PXSetPropertyException(Messages.ConfirmationIsRequired, PXErrorLevel.Error));
						}
					}

					list.Add(BuildCarrierPackage(pkgDetail, plugin));
				}

				if (failed)
				{
					throw new PXException(Messages.ConfirmationIsRequired);
				}

				return list;
			}

			public virtual CarrierBox BuildCarrierPackage(SOCarrierPackageDetailEx pkgDetail, CarrierPlugin plugin)
			{
				SOPackageDetailEx detail = pkgDetail.Package;
				CarrierBox box = new CarrierBox(detail.LineNbr.Value, ConvertWeightValue(detail.Weight ?? 0, plugin));
				box.Description = detail.Description;
				box.DeclaredValue = detail.DeclaredValue ?? 0;
				box.COD = detail.COD ?? 0;
				box.Length = ConvertLinearValue(detail.Length ?? 0, plugin);
				box.Width = ConvertLinearValue(detail.Width ?? 0, plugin);
				box.Height = ConvertLinearValue(detail.Height ?? 0, plugin);
				box.CarrierPackage = pkgDetail.CarrierBoxName;
				box.CustomRefNbr1 = detail.CustomRefNbr1;
				box.CustomRefNbr2 = detail.CustomRefNbr2;

				return box;
			}

			private List GetCarrierPackageDetail(List packages, string carrierID)
			{
				List sOCarrierPackages = new List();
				var carrierPackages = PXSelect>>>
					.Select(Base, carrierID).RowCast().AsEnumerable();

				foreach (var package in packages)
				{
					SOCarrierPackageDetailEx box = new SOCarrierPackageDetailEx();
					box.CarrierID = carrierID;
					box.CarrierBoxName = carrierPackages.Where(x => x.BoxID.Equals(package.BoxID)).Select(y => y.CarrierBox).FirstOrDefault();
					box.Package = package;

					sOCarrierPackages.Add(box);
				}

				return sOCarrierPackages;
			}

			public virtual CarrierRequest BuildRequest(SOShipment shiporder)
			{
				INSite warehouse = INSite.PK.Find(Base, shiporder.SiteID);
				if (warehouse == null)
				{
					Base.Document.Cache.RaiseExceptionHandling(shiporder, shiporder.SiteID,
								new PXSetPropertyException(Messages.WarehouseIsRequired, PXErrorLevel.Error));

					throw new PXException(Messages.WarehouseIsRequired);
				}

				SOShipmentAddress shipAddress = PXSelect>>>.Select(Base, shiporder.ShipAddressID);
				SOShipmentContact shipToContact = PXSelect>>>.Select(Base, shiporder.ShipContactID);
				Address warehouseAddress = PXSelect>>>.Select(Base, warehouse.AddressID);
				Contact warehouseContact = PXSelect>>>.Select(Base, warehouse.ContactID);
				PXResult result = (PXResult)
																			PXSelectJoin>,
																			InnerJoin>>>,
																			Where>>>.Select(Base, warehouse.BranchID);
				BAccount companyAccount = result;
				GL.Branch branch = result;
				GL.DAC.Organization organization = result;

				Address shipperAddress = PXSelect>>>.Select(Base, companyAccount.DefAddressID);
				Contact shipperContact = PXSelect>>>.Select(Base, companyAccount.DefContactID);

				Carrier carrier = Carrier.PK.Find(Base, shiporder.ShipVia);
				CarrierPlugin plugin = CarrierPlugin.PK.Find(Base, carrier.CarrierPluginID);
				ValidatePlugin(plugin);

				CarrierRequest cr = new CarrierRequest(GetUnitsType(plugin), shiporder.CuryID);
				cr.Attributes = new List();

				Location customerLocation = PXSelect>, And>>>>.Select(Base, shiporder.CustomerID, shiporder.CustomerLocationID);

				bool useGroundCollect = (shiporder.GroundCollect == true && Base.CanUseGroundCollect(shiporder));
				if (useGroundCollect || shiporder.UseCustomerAccount == true)
				{
					//by customer and location
					CarrierPluginCustomer cpc = PXSelect>,
							And>,
							And,
							And>, Or>>>>>,
						OrderBy>>
						.Select(Base, plugin.CarrierPluginID, shiporder.CustomerID, shiporder.CustomerLocationID);

					if (!string.IsNullOrEmpty(cpc?.CarrierAccount))
					{
						cr.ThirdPartyAccountID = cpc.CarrierAccount;

						Address customerAddress = PXSelect>>>.Select(Base, customerLocation.DefAddressID);
						cr.ThirdPartyPostalCode = cpc.PostalCode ?? customerAddress.PostalCode;
						cr.ThirdPartyCountryCode = cpc.CountryID ?? customerAddress.CountryID;
					}
					else if (shiporder.UseCustomerAccount == true)
					{
						throw new PXException(Messages.CustomeCarrierAccountIsNotSetup);
					}

					if (shiporder.UseCustomerAccount == true && cpc?.CarrierBillingType == CarrierBillingTypes.Receiver)
					{
						cr.Attributes.Add("RECEIVER");
					}
				}

				decimal freightCharge = 0m;
				if (shiporder.FreightAmountSource == FreightAmountSourceAttribute.OrderBased)
				{
					SOOrderExtension ext = Base.GetExtension();
					IEnumerable sOOrderShipments = ext.OrderListSimple.Select().RowCast();

					//if the freight amount is based on Sales Order and the shipment has multiple SO or if it is a partial shipment, then add attribute "SKIPFREIGHTCHARGE"
					//don't send the overriden freight price field in this case. Otherwise send FreightAmt + PremiumFreightAmt from SO
					if (sOOrderShipments.Count() == 1)
					{
						SOOrderShipment soOrderShipment = sOOrderShipments.FirstOrDefault();
						SOOrder order = ext.soorder.Select(soOrderShipment?.OrderType, soOrderShipment?.OrderNbr);

						if (order?.OrderQty == soOrderShipment?.ShipmentQty)
						{
							freightCharge = (order?.FreightAmt ?? 0m) + (order?.PremiumFreightAmt ?? 0m);
						}
						else
						{
							cr.Attributes.Add("SKIPFREIGHTCHARGE");
						}
					}
					else
					{
						cr.Attributes.Add("SKIPFREIGHTCHARGE");
					}
				}
				else
				{
					freightCharge = shiporder.FreightAmt ?? 0m;
				}

				cr.FreightCharge = freightCharge;

				(SOAddress billToAddress, SOContact billToContact) = Base.GetBillToAddressContact();

				if (billToAddress != null)
				{
					cr.BillToAddress = billToAddress;
				}
				if (billToContact != null)
				{
					cr.BillToContact = billToContact;
				}

				cr.Shipper = shipperAddress;
				cr.ShipperContact = shipperContact;
				cr.Origin = warehouseAddress;
				cr.OriginContact = warehouseContact;
				cr.Destination = shipAddress;
				cr.DestinationContact = shipToContact;
				cr.Packages = GetPackages(shiporder, carrier, plugin);
				cr.Resedential = shiporder.Resedential == true;
				cr.SaturdayDelivery = shiporder.SaturdayDelivery == true;
				cr.Insurance = shiporder.Insurance == true;
				cr.ShipDate = Tools.Max(Base.Accessinfo.BusinessDate.Value.Date, shiporder.ShipDate.Value.Date);
				cr.ReceiverTaxID = customerLocation?.TaxRegistrationID;
				cr.ShipperTaxID = companyAccount.TaxRegistrationID;

				if (useGroundCollect)
				{
					cr.Attributes.Add("COLLECT");
				}
				cr.InvoiceLineTotal = shiporder.LineTotal.GetValueOrDefault();

				if (!string.IsNullOrWhiteSpace(warehouse.CarrierFacility))
				{
					cr.Attributes.Add(string.Concat("CarrierFacilityW:", warehouse.CarrierFacility));
				}

				if (!string.IsNullOrWhiteSpace(branch.CarrierFacility))
				{
					cr.Attributes.Add(string.Concat("CarrierFacilityB:", branch.CarrierFacility));
				}
				else if (!string.IsNullOrWhiteSpace(organization.CarrierFacility))
				{
					cr.Attributes.Add(string.Concat("CarrierFacilityB:", organization.CarrierFacility));
				}

				return cr;
			}

			protected override WebDialogResult AskForRateSelection() => Base.CurrentDocument.AskExt();

			protected virtual void _(Events.FieldUpdated e)
			{
				if (e.Row != null)
				{
					Base.Packages.Cache.SetDefaultExt(e.Row);
					Base.Packages.Cache.SetDefaultExt(e.Row);
					Base.Packages.Cache.SetDefaultExt(e.Row);
					Base.Packages.Cache.SetDefaultExt(e.Row);
					Base.Packages.Cache.SetDefaultExt(e.Row);
					Base.Packages.Cache.SetDefaultExt(e.Row);
				}
			}

			protected virtual void _(Events.RowSelected e)
			{
				if (!(e.Row is SOShipment row))
					return;

				if (row.UnlimitedPackages == true)
				{
					shopRates.SetEnabled(false);
					shopRates.SetTooltip(Messages.ShopForRatesDisabledDueToUnlimitedPackages);
				}
				else
				{
					shopRates.SetEnabled(true);
					shopRates.SetTooltip(Messages.ShopForRatesButtonCaption);
				}
			}

			/// Overrides 
			[PXOverride]
			public void Persist( // TODO: override PrePersist instead
				Action base_Persist)
			{
				if (Base.Document.Current != null && Base.Document.Current.IsPackageValid != true &&
					Base.Document.Current.Released != true && Base.Document.Current.Confirmed != true && Base.Document.Current.SiteID != null)
				{
					if (Base.Document.Current.RecalcPackagesReason == SOShipment.recalcPackagesReason.ShipVia && Base.ValidateAvailablePackages())
					{
						foreach (SOPackageDetail package in Base.Packages.Select())
						{
							if (package.PackageType == SOPackageType.Auto)
								package.Confirmed = false;
						}

						Base.Document.Current.IsPackageValid = true;
					}
					else
					{
					recalculatePackages.Press();
				}
				}

				base_Persist();
			}

			protected override IEnumerable GetApplicableCarrierPlugins()
			{
				return PXSelectReadonly, And>>>>>
					.Select(Base)
					.RowCast();
			}

			private class LineInfo : ILineInfo
			{
				private SOShipLine _line;
				public LineInfo(SOShipLine line) { _line = line; }

				public decimal? BaseQty => _line.BaseQty;
				public decimal? CuryLineAmt => _line.LineAmt;
				public decimal? ExtWeight => _line.ExtWeight;
				public int? SiteID => _line.SiteID;
				public string Operation => _line.Operation;
			}
		}

		public CartSupport CartSupportExt => FindImplementation();
		public class CartSupport : PXGraphExtension
		{
			public static bool IsActive() => PXAccess.FeatureInstalled();

			public SelectFrom.Where.View ShipmentCartLinks;
			public SelectFrom.View PickListCartLinks;
			public SelectFrom.View CartLinks;

			public virtual void RemoveItemsFromCart()
			{
				var links =
					SelectFrom.
					InnerJoin.On.
					InnerJoin.On.
					Where.
					View
					.Select(Base)
					.AsEnumerable()
					.Cast>()
					.ToArray();

				foreach ((var sosplit, var link, var cartsplit) in links)
				{
					decimal cartQty = Math.Min(sosplit.Qty.Value, link.Qty.Value);

					var actualLink = ShipmentCartLinks.Locate(link) ?? link;
					actualLink.Qty -= cartQty;
					if (actualLink.Qty <= 0)
						ShipmentCartLinks.Delete(actualLink);
					else
						ShipmentCartLinks.Update(actualLink);

					var actualCartSplit = CartLinks.Locate(cartsplit) ?? cartsplit;
					actualCartSplit.Qty -= cartQty;
					if (actualCartSplit.Qty <= 0)
						CartLinks.Delete(actualCartSplit);
					else
						CartLinks.Update(actualCartSplit);
				}
			}

			public virtual SOShipmentSplitToCartSplitLink TransformCartLinks(SOShipLineSplit shipSplit, IReadOnlyCollection pickerCartLinks)
			{
				if (pickerCartLinks.Any(link => link.Qty > 0))
				{
					var firstOldLink = pickerCartLinks.Where(link => link.Qty > 0).First();

					SOShipmentSplitToCartSplitLink existingLink = ShipmentCartLinks.Search<
						SOShipmentSplitToCartSplitLink.shipmentNbr,
						SOShipmentSplitToCartSplitLink.shipmentLineNbr,
						SOShipmentSplitToCartSplitLink.shipmentSplitLineNbr,
						SOShipmentSplitToCartSplitLink.siteID,
						SOShipmentSplitToCartSplitLink.cartID,
						SOShipmentSplitToCartSplitLink.cartSplitLineNbr>(
						shipSplit.ShipmentNbr,
						shipSplit.LineNbr,
						shipSplit.SplitLineNbr,
						firstOldLink.SiteID,
						firstOldLink.CartID,
						firstOldLink.CartSplitLineNbr);

					if (existingLink == null)
					{
					var newLink = new SOShipmentSplitToCartSplitLink
					{
						ShipmentNbr = shipSplit.ShipmentNbr,
						ShipmentLineNbr = shipSplit.LineNbr,
						ShipmentSplitLineNbr = shipSplit.SplitLineNbr,

							SiteID = firstOldLink.SiteID,
							CartID = firstOldLink.CartID,
							CartSplitLineNbr = firstOldLink.CartSplitLineNbr,
						Qty = 0m
					};
						existingLink = ShipmentCartLinks.Insert(newLink);
					}

					decimal linkRestQty = shipSplit.Qty.Value;
					foreach (var oldLink in pickerCartLinks.Where(link => link.Qty > 0))
					{
						if (linkRestQty == 0)
							break;

						decimal linkQty = Math.Min(oldLink.Qty.Value, linkRestQty);

						existingLink.Qty += linkQty;
						oldLink.Qty -= linkQty;
						if (oldLink.Qty > 0)
							PickListCartLinks.Update(oldLink);
						else
							PickListCartLinks.Delete(oldLink);

						linkRestQty -= linkQty;
					}

					return ShipmentCartLinks.Update(existingLink);
				}

				return null;
			}
		}

		public WorkLog WorkLogExt => FindImplementation();
		public class WorkLog : GraphExtensions.ShipmentWorkLog
		{
			public static bool IsActive() => PXAccess.FeatureInstalled();
		}
		#endregion

		#region Address Lookup Extension
		/// 
		public class SOShipmentEntryAddressLookupExtension : CR.Extensions.AddressLookupExtension
		{
			protected override string AddressView => nameof(Base.Shipping_Address);
		}

		public class SOShipmentEntryShippingAddressCachingHelper : AddressValidationExtension
		{
			protected override IEnumerable> AddressSelects()
			{
				yield return Base.Shipping_Address;
			}
		}
		#endregion
	}
}
