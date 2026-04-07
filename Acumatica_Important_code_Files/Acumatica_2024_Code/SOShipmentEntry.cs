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
using System.Collections.Immutable;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using PX.Data;
using PX.Data.BQL;
using PX.Data.BQL.Fluent;
using PX.Data.WorkflowAPI;
using PX.Common;
using PX.Objects.AR;
using PX.Objects.CM;
using PX.Objects.CR;
using PX.Objects.CS;
using PX.Objects.EP;
using PX.Objects.GL;
using PX.Objects.IN;
using PX.Objects.PM.Lite;
using PX.SM;
using POLineType = PX.Objects.PO.POLineType;
using POReceiptLine = PX.Objects.PO.POReceiptLine;
using PX.CarrierService;
using PX.Data.DependencyInjection;
using PX.LicensePolicy;
using PX.Objects.SO.Services;
using PX.Objects.PO;
using PX.Objects.AR.MigrationMode;
using PX.Objects.Common;
using PX.Objects.Common.Discount;
using PX.Objects.Common.Extensions;
using PX.Common.Collection;
using PX.Objects.SO.GraphExtensions.CarrierRates;
using PX.Objects.SO.GraphExtensions.SOShipmentEntryExt;
using PX.Api;
using ShipmentActions = PX.Objects.SO.SOShipmentEntryActionsAttribute;
using PdfSharp.Pdf.IO;
using PX.Objects.IN.Attributes;
using PX.Concurrency;
using PX.Objects.SO.GraphExtensions.SOOrderEntryExt;
using PX.Objects.GL.FinPeriods.TableDefinition;
using PX.Objects.GL.FinPeriods;
using PX.Objects.IN.InventoryRelease;
using PX.Objects.IN.InventoryRelease.Accumulators.QtyAllocated;
using PX.Objects.Common.Scopes;

namespace PX.Objects.SO
{
	public partial class SOShipmentEntry : PXGraph, IGraphWithInitialization
	{
		private DiscountEngine _discountEngine => DiscountEngineProvider.GetEngineFor();
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
		[PXViewName(Messages.SOOrderShipment)]
		public
			PXSelectJoin,
				And>>,
			LeftJoin>,
			LeftJoin>,
			LeftJoin>>>>>,
			Where<
				SOOrderShipment.shipmentNbr, Equal>,
				And>>>>
			OrderList;

		public PXSelect>,
				And>>>>
			soorder;
		public PXSetup>>> soordertype;
		public PXSelect sosetupapproval;
		public EPApprovalAutomation Approval;

		public PXSelect>, And>>>> OrderListSimple;
		public PXSelect>>> DiscountDetails;
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

		public PXSelect soline;
		public PXSelect solinesplit;
		public PXSelect dummy_soline; //will prevent collection was modified if no Select was executed prior to Persist()

		public PXFilter addsofilter;
        public PXSelectJoinOrderBy>,
               InnerJoin, And, And>>>>>,
			   OrderBy>>>> soshipmentplan;

		private class SkipShipCompleteValidationScope : Common.Scopes.FlaggedModeScopeBase { }

		[PXViewName(CR.Messages.MainContact)]
		public PXSelect DefaultCompanyContact;
		protected virtual IEnumerable defaultCompanyContact()
		{
			return OrganizationMaint.GetDefaultContactForCurrentOrganization(this);
		}

		[PXCopyPasteHiddenView()]
		public SelectFrom
			.InnerJoin.On
			.Where.View OrderSite;

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
			foreach (SOShipment shipment in adapter.Get())
			{
				Document.Current = shipment;

				var parameters = new Dictionary();
				parameters["SOShipment.ShipmentNbr"] = shipment.ShipmentNbr;

				GL.Branch branch = PXSelectReadonly2>>,
						Where>,
								And, Equal,
							Or>,
								And, NotEqual>>>>>
					.SelectSingleBound(this, new object[] {shipment});

				this.GetExtension().SendNotification(ARNotificationSource.Customer, notificationCD, (branch != null && branch.BranchID != null) ? branch.BranchID : Accessinfo.BranchID, parameters, adapter.MassProcess);

				yield return shipment;
			}
		}

		public PXAction emailShipment;
		[PXButton(CommitChanges = true), PXUIField(DisplayName = "Email Shipment", MapEnableRights = PXCacheRights.Select, MapViewRights = PXCacheRights.Select)]
		public virtual IEnumerable EmailShipment(
			PXAdapter adapter,
			[PXString]
			string notificationCD = null) => Notification(adapter, notificationCD ?? "SHIPMENT");

		#region Action menu items

		public PXAction confirmShipmentAction;
		[PXButton(CommitChanges = true), PXUIField(DisplayName = ShipmentActions.Messages.ConfirmShipment, MapEnableRights = PXCacheRights.Select, MapViewRights = PXCacheRights.Select)]
		protected virtual IEnumerable ConfirmShipmentAction(PXAdapter adapter)
		{
			var list = adapter.Get().ToList();
			bool massProcess = adapter.MassProcess;

			Save.Press();

			PXLongOperation.StartOperation(this, delegate ()
			{
				SOShipmentEntry docgraph = PXGraph.CreateInstance();
				SOOrderEntry orderentry = PXGraph.CreateInstance();

				docgraph.MergeStatusCachesBetweenGraphs(docgraph, orderentry);

				PXCache cache = orderentry.Caches[typeof(SOShipLineSplit)];
				cache = orderentry.Caches[typeof(INTranSplit)];

				foreach (SOShipment shipment in list)
				{
					if (massProcess)
						PXProcessing.SetCurrentItem(shipment);

					try
					{
						docgraph.PrepareShipmentForConfirmation(shipment);

						docgraph.ShipPackages(shipment);

						docgraph.IsShipmentReadyForConfirmation = true;
						docgraph.ConfirmShipment(orderentry, shipment);

						if (massProcess)
							PXProcessing.SetProcessed();
					}
					catch (Exception ex) when (massProcess)
					{
						PXProcessing.SetError(ex);
					}
					finally
					{
						docgraph.IsShipmentReadyForConfirmation = false;
					}
				}
			});

			return list;
		}

		public PXAction createInvoice;
		[PXButton(CommitChanges = true), PXUIField(DisplayName = ShipmentActions.Messages.CreateInvoice, MapEnableRights = PXCacheRights.Select, MapViewRights = PXCacheRights.Select)]
		protected virtual IEnumerable CreateInvoice(PXAdapter adapter)
		{
			var shipments = adapter.Get().ToList();
			var adapterSlice = (adapter.MassProcess, adapter.AllowRedirect, adapter.QuickProcessFlow);
			var redirectRequired = !this.IsImport;
			if (!adapter.Arguments.TryGetValue(nameof(SOShipmentFilter.InvoiceDate), out object invoiceDate) || invoiceDate == null)
				invoiceDate = Accessinfo.BusinessDate;

			Save.Press();

			PXLongOperation.StartOperation(this, delegate ()
			{
				var shipmentEntry = CreateInstance();
				var invoiceEntry = CreateInstance();

				InvoiceList createdInvoices = new ShipmentInvoices(shipmentEntry);

				foreach (SOShipment shipment in shipments)
				{
					try
					{
						shipmentEntry.SelectTimeStamp();
						invoiceEntry.SelectTimeStamp();

						if (adapterSlice.MassProcess)
							PXProcessing.SetCurrentItem(shipment);

						shipmentEntry.InvoiceShipment(invoiceEntry, shipment, (DateTime)invoiceDate, createdInvoices, adapterSlice.QuickProcessFlow);

						if (adapterSlice.MassProcess) // shipment is updated and saved somewhere in InvoiceShipment method
						{
							shipmentEntry.Document.Cache.RestoreCopy(shipment, SOShipment.PK.Find(shipmentEntry, shipment));
							PXProcessing.SetProcessed();
						}
					}
					catch (Exception ex) when (adapterSlice.MassProcess)
					{
						PXProcessing.SetError(ex);
					}
				}

				invoiceEntry.CompleteProcessingImpl(createdInvoices);

				if (adapterSlice.AllowRedirect && !adapterSlice.MassProcess && redirectRequired && createdInvoices.Count > 0)
				{
					using (new PXTimeStampScope(null))
					{
						ARInvoice firstInvoice = createdInvoices[0];
						invoiceEntry = PXGraph.CreateInstance();

						invoiceEntry.Document.Current = invoiceEntry.Document.Search(firstInvoice.DocType, firstInvoice.RefNbr, firstInvoice.DocType);
						throw new PXRedirectRequiredException(invoiceEntry, "Invoice");
					}
				}
			});

			return shipments;
		}

		public PXAction UpdateIN;
		[PXButton(CommitChanges = true), PXUIField(DisplayName = ShipmentActions.Messages.PostInvoiceToIN, MapEnableRights = PXCacheRights.Select, MapViewRights = PXCacheRights.Select)]
		protected virtual IEnumerable updateIN(PXAdapter adapter, List shipmentList = null)
		{
			List shipments = shipmentList ?? adapter.Get().ToList();
			var adapterSlice = (adapter.MassProcess, adapter.QuickProcessFlow);

			if (!UnattendedMode && NeedWarningShipNotInvoicedUpdateIN(this, sosetup.Current, shipments, true))
			{
				WebDialogResult result = Document.View.Ask(Document.Current, GL.Messages.Confirmation, Messages.ShipNotInvoicedUpdateIN, MessageButtons.YesNo, MessageIcon.Question);
				if (result != WebDialogResult.Yes)
					return shipments;
			}

			Save.Press();

			PXLongOperation.StartOperation(this, delegate ()
			{
				var shipmentEntry = CreateInstance();
				var factory = new INRegisterEntryFactory(shipmentEntry);
				var createdINDocs = new DocumentList(shipmentEntry);

				foreach (SOShipment shipment in shipments)
				{
					try
					{
						if (adapterSlice.MassProcess)
							PXProcessing.SetCurrentItem(shipment);

						shipmentEntry.PostShipment(factory, shipment, createdINDocs);

						if (adapterSlice.MassProcess) // shipment is updated and saved somewhere in PostShipment method
							shipmentEntry.Document.Cache.RestoreCopy(shipment, SOShipment.PK.Find(shipmentEntry, shipment));
					}
					catch (Exception ex) when (adapterSlice.MassProcess)
					{
						PXProcessing.SetError(ex);
					}
				}

				if (shipmentEntry.sosetup.Current.AutoReleaseIN == true && createdINDocs.Count > 0 && createdINDocs[0].Hold == false)
					INDocumentRelease.ReleaseDoc(createdINDocs, false, processFlow: adapterSlice.QuickProcessFlow);

				if (createdINDocs.Count == 1 && adapterSlice.QuickProcessFlow != PXQuickProcess.ActionFlow.NoFlow)
					INDocumentRelease.RedirectTo(createdINDocs[0]);
			});

			return shipments;
		}

		public static bool NeedWarningShipNotInvoicedUpdateIN(PXGraph graph, SOSetup setup, IEnumerable shipments, bool validateEachShipmentLine = false)
		{
			bool IsNotBillable(SOShipment shipment) =>
				shipment.Confirmed == true &&
				shipment.UnbilledOrderCntr == 0 &&
				shipment.BilledOrderCntr == 0 &&
				shipment.ReleasedOrderCntr == 0;

			if (setup.UseShipDateForInvoiceDate != true)
			{
				bool shippedNotInvoicedIsDisabled = false;
				if (validateEachShipmentLine)
				{
					foreach (SOShipment shipment in shipments)
					{
						SOShipLine shipLineWithOrderTypeWithDisbledShippedNotInvoiced = SelectFrom.
							InnerJoin.On.
								And>.
								And>.
								And>>.
							Where>.View.SelectSingleBound(graph, null, shipment.ShipmentNbr);

						if (shipLineWithOrderTypeWithDisbledShippedNotInvoiced != null)
						{
							shippedNotInvoicedIsDisabled = true;
							break;
						}
					}
				}
				else
				{
					SOOrderType orderTypeWithDisbledShippedNotInvoiced = SelectFrom.
						Where.
						And>.
						And>.
						And>>.View.SelectSingleBound(graph, null);

					if (orderTypeWithDisbledShippedNotInvoiced != null)
						shippedNotInvoicedIsDisabled = true;

				}

				if (shippedNotInvoicedIsDisabled)
				{
					return shipments.Any(shipment => shipment.Status == SOShipmentStatus.Confirmed && !IsNotBillable(shipment));
				}
			}

			return false;
		}

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
				var orderentry = CreateInstance();

				docgraph.MergeStatusCachesBetweenGraphs(docgraph, orderentry);

				PXCache cache = orderentry.Caches[typeof(SOShipLineSplit)];
				cache = orderentry.Caches[typeof(INTranSplit)];

				foreach (SOShipment shipment in list)
				{
					try
					{
						if (massProcess)
							PXProcessing.SetCurrentItem(shipment);

						using (PXTransactionScope ts = new PXTransactionScope())
						{
							docgraph.SetSuppressWorkflowOnCorrectShipment();
							docgraph.CorrectShipment(orderentry, shipment);
							docgraph.CancelPackages(shipment);

							ts.Complete();
						}

					}
					catch (Exception ex) when (massProcess)
					{
						PXProcessing.SetError(ex);
					}
				}
			});

			return list;
		}

		public PXAction createDropshipInvoice;
		[PXButton(CommitChanges = true), PXUIField(DisplayName = ShipmentActions.Messages.CreateDropshipInvoice, Visible = false, MapEnableRights = PXCacheRights.Select, MapViewRights = PXCacheRights.Select)]
		protected virtual IEnumerable CreateDropshipInvoice(PXAdapter adapter)
		{
			var list = adapter.Get().ToList();
			var adapterSlice = (adapter.MassProcess, adapter.Arguments);

			PXLongOperation.StartOperation(this, delegate ()
			{
				var shipmentEntry = CreateInstance();
				InvoiceList createdInvoices = new ShipmentInvoices(shipmentEntry);

				InvoiceReceipt(adapterSlice.Arguments, list, createdInvoices, adapterSlice.MassProcess);

				if (adapterSlice.MassProcess) // shipment is updated and saved somewhere in InvoiceReceipt method
					list.ForEach(sh => shipmentEntry.Document.Cache.RestoreCopy(sh, SOShipment.PK.Find(shipmentEntry, shipmentEntry.Document.Current)));

				if (!adapterSlice.MassProcess && createdInvoices.Count > 0)
				{
					using (new PXTimeStampScope(null))
					{
						var invoiceEntry = CreateInstance();
						invoiceEntry.Document.Current = createdInvoices[0];
						throw new PXRedirectRequiredException(invoiceEntry, "Invoice");
					}
				}
			});

			return list;
		}

		public PXAction printPickListAction;
		[PXButton(CommitChanges = true), PXUIField(DisplayName = ShipmentActions.Messages.PrintPickList, MapEnableRights = PXCacheRights.Select, MapViewRights = PXCacheRights.Select)]
		protected virtual IEnumerable PrintPickListAction(PXAdapter adapter)
		{
			var list = adapter.Get().ToList();
			LongOperationManager.StartAsyncOperation(ct=>PrintPickList(list, adapter, ct));
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

		public virtual void SOShipmentPlan_RowSelected(PXCache sender, PXRowSelectedEventArgs e)
		{
			if (e.Row == null) return;

			SOShipmentPlan plan = (SOShipmentPlan)e.Row;
			if (Document.Current.ShipDate < plan.PlanDate)
			{
				PXUIFieldAttribute.SetWarning(sender, plan, Messages.PlanDateGreaterShipDate);
			}
		}

		public PXSelectJoin>,
					LeftJoin,
							And,
							And,
							And,
							And,
							And>>>>>>>,
					LeftJoin,
							And,
							And>>>>>>,
					Where>,
						And>,
						And2>, Or, IsNull>>,
						And>,
						And>,
						And2, IsNull,
							Or>>>,
						And2,
							Or,
							Or2, Or>>,
							Or>>>>,
						And, IsNull,
							Or>>>>>>>>>>>>> sOshipmentplanSelect;

		public virtual IEnumerable sOshipmentplan()
		{
			string shipmentFreightSrc = this.Document.Current?.FreightAmountSource,
				orderFreightSrc = this.addsofilter.Current?.FreightAmountSource;
			if (!shipmentFreightSrc.IsIn(null, this.addsofilter.Current?.FreightAmountSource))
				yield break;

			var shipmentSOLineSplits = new Lazy(() => CollectShipmentOrigSOLineSplits());

			foreach (PXResult res in
					sOshipmentplanSelect.Select())
			{
				SOLineSplit sls = (SOLineSplit)res;
				if (!shipmentSOLineSplits.Value.Contains(sls))
				{
					yield return new PXResult((SOShipmentPlan)res, sls, (SOLine)res);
				}
			}
		}

		protected virtual OrigSOLineSplitSet CollectShipmentOrigSOLineSplits()
		{
			var ret = new OrigSOLineSplitSet();
			PXSelectBase cmd = new PXSelectReadonly>>>(this);
			using (new PXFieldScope(cmd.View, typeof(SOShipLine.shipmentNbr), typeof(SOShipLine.lineNbr),
					typeof(SOShipLine.origOrderType), typeof(SOShipLine.origOrderNbr), typeof(SOShipLine.origLineNbr), typeof(SOShipLine.origSplitLineNbr)))
			{
				foreach (SOShipLine sl in cmd.Select())
				{
					ret.Add(sl);
				}
			}
			foreach (SOShipLine sl in Transactions.Cache.Deleted)
			{
				ret.Remove(sl);
			}
			foreach (SOShipLine sl in Transactions.Cache.Inserted)
			{
				ret.Add(sl);
			}
			return ret;
		}

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
			PXDBDefaultAttribute.SetDefaultForInsert(OrderList.Cache, null, true);
			PXDBDefaultAttribute.SetDefaultForUpdate(OrderList.Cache, null, true);

			PXDBDefaultAttribute.SetDefaultForInsert(OrderList.Cache, null, true);
			PXDBDefaultAttribute.SetDefaultForUpdate(OrderList.Cache, null, true);

			PXDBLiteDefaultAttribute.SetDefaultForInsert(OrderList.Cache, null, true);
			PXDBLiteDefaultAttribute.SetDefaultForUpdate(OrderList.Cache, null, true);

			PXUIFieldAttribute.SetDisplayName(Caches[typeof(Contact)], CR.Messages.Attention);
			this.Views.Caches.Add(typeof(SOLineSplit));
			this.Views.Caches.Add(typeof(NoteDoc));

			FieldDefaulting.AddHandler((sender, e) => { if (e.Row != null) e.NewValue = BAccountType.CustomerType; });
		}

		#region Entity Event Handlers
		public PXWorkflowEventHandler OnShipmentConfirmed;
		public PXWorkflowEventHandler OnShipmentCorrected;

		public PXWorkflowEventHandler OnInvoiceLinked;
		public PXWorkflowEventHandler OnInvoiceUnlinked;

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

		public PXAction selectSO;
		[PXUIField(DisplayName = "Add Order", MapEnableRights = PXCacheRights.Select, MapViewRights = PXCacheRights.Select)]
		[PXLookupButton]
		public virtual IEnumerable SelectSO(PXAdapter adapter)
		{
			if (this.Document.Cache.AllowDelete)
			{
				WebDialogResult result = addsofilter.AskExt();
				if (result == WebDialogResult.OK)
					AddSO(adapter);
			}

			return adapter.Get();
		}

		public PXAction addSO;
		[PXUIField(DisplayName = "Add", MapEnableRights = PXCacheRights.Select, MapViewRights = PXCacheRights.Select)]
		[PXLookupButton]
		public virtual IEnumerable AddSO(PXAdapter adapter)
		{
			SOOrder order = PXSelect>,
					And>>>>.Select(this);

			bool selected = order != null &&
				(addsofilter.Current?.AddAllLines == true || AnySelected(soshipmentplan.Cache));

			if (selected)
			{
				try
				{
					using (LineSplittingExt.ForceUnattendedModeScope(true))
						CreateShipment(new CreateShipmentArgs
						{
							MassProcess = false,
							Order = order,
							OrderLineNbr = addsofilter.Current.OrderLineNbr,
							SiteID = Document.Current.SiteID,
							ShipDate = Document.Current.ShipDate,
							UseOptimalShipDate = false,
							Operation = addsofilter.Current.Operation,
							ShipmentList = addsofilter.Current.AddAllLines == true ? new DocumentList(this) : null,
						});
				}
				finally
				{
					addsofilter.Current.AddAllLines = false;
				}

			}

			if (addsofilter.Current != null && !IsImport)
			{
				try
				{
					addsofilter.Cache.SetDefaultExt(addsofilter.Current);
					addsofilter.Current.OrderNbr = null;
				}
				catch { }
			}

			soshipmentplan.Cache.Clear();
			soshipmentplan.View.Clear();
			soshipmentplan.Cache.ClearQueryCacheObsolete();
			sOshipmentplanSelect.View.Clear();
			ShipmentScheduleSelect.View.Clear();

			return adapter.Get();
		}

		public PXAction addSOCancel;
		[PXUIField(DisplayName = "Cancel", MapEnableRights = PXCacheRights.Select, MapViewRights = PXCacheRights.Select)]
		[PXLookupButton]
		public virtual IEnumerable AddSOCancel(PXAdapter adapter)
		{
			addsofilter.Cache.SetDefaultExt(addsofilter.Current);
			addsofilter.Current.OrderNbr = null;
			soshipmentplan.Cache.Clear();
			soshipmentplan.View.Clear();

			return adapter.Get();
		}

		#region SOOrder Events
		protected virtual void SOOrder_RowPersisting(PXCache sender, PXRowPersistingEventArgs e)
		{
			SOOrder order = e.Row as SOOrder;

			if (e.Operation == PXDBOperation.Update)
			{
				if (order.ShipmentCntr < 0 || order.OpenShipmentCntr < 0 || (order.ShipmentCntr == 0 || order.OpenShipmentCntr == 0) &&
					((IEnumerable)OrderList.Cache.Inserted).Any(a => a.OrderType == order.OrderType && a.OrderNbr == order.OrderNbr) ||
					order.ShipmentCntr == 0 &&
					((IEnumerable)OrderList.Cache.Updated).Any(a => a.OrderType == order.OrderType && a.OrderNbr == order.OrderNbr))
				{
					throw new Exceptions.InvalidShipmentCountersException();
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

		#endregion

		#region SOLine CacheAttached

		[PXDBBool()]
		[PXFormula(typeof(Switch, And, And>>>, True>, False>))]
		[DirtyFormula(typeof(Switch, And, Or>>>>, int1>, int0>),
			typeof(SumCalc), IsUnbound:true)]
		[PXUIField(DisplayName = "Open Line", Enabled = false)]
		public virtual void SOLine_OpenLine_CacheAttached(PXCache sender)
		{
		}

		#endregion

		#region SOShipLine Cache Attached

		[PXMergeAttributes(Method = MergeMethod.Append)]
		[PXRemoveBaseAttribute(typeof(INUnitAttribute))]
		[SOShipLineUnit(DisplayName = "UOM")]
		protected virtual void SOShipLine_UOM_CacheAttached(PXCache sender) { }

		[PXMergeAttributes(Method = MergeMethod.Append)]
		[PXFormula(null, typeof(CountCalc))]
		protected virtual void SOShipLine_ShipmentNbr_CacheAttached(PXCache sender)
		{
		}

		[PXMergeAttributes(Method = MergeMethod.Append)]
		[PXFormula(null, typeof(SumCalc))]
		[PXFormula(null, typeof(SumCalc))]
		protected virtual void SOShipLine_ShippedQty_CacheAttached(PXCache sender)
		{
		}

		[PXMergeAttributes(Method = MergeMethod.Append)]
		[PXFormula(typeof(Mult>>>), typeof(SumCalc))]
		[PXFormula(null, typeof(SumCalc))]
		protected virtual void SOShipLine_LineAmt_CacheAttached(PXCache sender)
		{
		}

		[PXMergeAttributes(Method = MergeMethod.Append)]
		[PXFormula(typeof(Mult.WithDependencies, SOShipLine.unitWeigth>), typeof(SumCalc))]
		[PXFormula(null, typeof(SumCalc))]
		protected virtual void SOShipLine_ExtWeight_CacheAttached(PXCache sender)
		{
		}

		[PXMergeAttributes(Method = MergeMethod.Append)]
		[PXFormula(typeof(Mult.WithDependencies, SOShipLine.unitVolume>), typeof(SumCalc))]
		[PXFormula(null, typeof(SumCalc))]
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

		#region SOLine2 Events

		[PXDBString(TX.TaxCategory.taxCategoryID.Length, IsUnicode = true, BqlField = typeof(SOLine.taxCategoryID))]
		public virtual void SOLine2_TaxCategoryID_CacheAttached(PXCache sender)
		{
		}

		[PXDBLong(BqlField = typeof(SOLineSplit.planID), IsImmutable = true)]
		protected virtual void SOLineSplit2_PlanID_CacheAttached(PXCache sender) { }

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

			if (!sender.ObjectsEqual(row, oldRow))
				SyncShipDateWithLinks(row);

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

		protected virtual void SyncShipDateWithLinks(SOShipment shipment)
		{
			var cache = OrderList.Cache;

			foreach (SOOrderShipment link in OrderList.Select())
			{
				if (link.ShipmentType != SOShipmentType.DropShip)
				{
					cache.SetValue(link, shipment.ShipDate);
					cache.MarkUpdated(link, assertError: true);
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
			selectSO.SetEnabled(row.SiteID != null && sender.AllowDelete);

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

			if (row.UnlimitedPackages == true)
			{
				sender.RaiseExceptionHandling(row, null, new PXSetPropertyException(row,
					Messages.ShopForRatesDisabledDueToUnlimitedPackages, PXErrorLevel.Warning));
			}
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

		#endregion

		#region SOOrderShipment Events

		protected virtual void UpdateShipmentCntr(PXCache sender, SOOrderShipment row, short? Counter)
		{
			SOOrder order = PXParentAttribute.SelectParent(sender, row);
			if (order != null)
			{
				order.ShipmentDeleted = (Counter == -1) ? true : (bool?)null;
				order.ShipmentCntr += Counter;
				if (row.Confirmed == false)
				{
					order.OpenShipmentCntr += Counter;
				}
				soorder.Cache.Update(order);
			}
			SOOrderSite orderSite = PXParentAttribute.SelectParent(sender, row);
			if (orderSite != null && Counter != 0)
			{
				orderSite.ShipmentCntr += Counter;
				if (row.Confirmed == false)
					orderSite.OpenShipmentCntr += Counter;

				orderSite = OrderSite.Update(orderSite);
			}
		}

		protected virtual void SOOrderShipment_RowInserted(PXCache sender, PXRowInsertedEventArgs e)
		{
			SOOrderShipment row = (SOOrderShipment)e.Row;
			UpdateShipmentCntr(sender, row, 1);
			UpdateManualFreightCost(Document.Current, row, 0m, row.ShipmentQty, true);

		}

		protected virtual void SOOrderShipment_RowUpdated(PXCache sender, PXRowUpdatedEventArgs e)
		{
			//during correct shipment this will eliminate overwrite of SOOrder in SOShipmentEntry.Persist()
			if (!object.ReferenceEquals(e.Row, e.OldRow))
			{
				UpdateShipmentCntr(sender, (SOOrderShipment)e.OldRow, -1);
				UpdateShipmentCntr(sender, (SOOrderShipment)e.Row, 1);
				SOOrderShipment row = (SOOrderShipment)e.Row;
				SOOrderShipment oldRow = (SOOrderShipment)e.OldRow;
				if (row.ShipmentQty - oldRow.ShipmentQty != 0m)
				{
					UpdateManualFreightCost(Document.Current, row, oldRow.ShipmentQty, row.ShipmentQty);
				}
			}
		}

		protected virtual void SOOrderShipment_RowDeleted(PXCache sender, PXRowDeletedEventArgs e)
		{
			var link = (SOOrderShipment)e.Row;
			UpdateShipmentCntr(sender, link, -1);

			SOOrderShipment.Events
				.Select(ev => ev.ShipmentUnlinked)
				.FireOn(this, link, Document.Current);

			UpdateManualFreightCost(Document.Current, link, link.ShipmentQty, 0m);

			RestoreCustomerOrderNbr();
			ResetManualPackageFlag();
		}

		protected virtual void RestoreCustomerOrderNbr()
		{
			SOShipment shipment = Document.Current;
			if (shipment == null || shipment.OrderCntr != 1 || shipment.CustomerOrderNbr != null)
				return;

			// If we have single Order within shipment we should fill CustomerOrderNbr.
			SOOrderShipment orderShipment = OrderListSimple.Select();
			if (orderShipment == null)
				return;

			SOOrder order = PXParentAttribute.SelectParent(OrderListSimple.Cache, orderShipment);
			if (!string.IsNullOrEmpty(order.CustomerOrderNbr))
			{
				shipment.CustomerOrderNbr = order.CustomerOrderNbr;
				Document.Update(shipment);
			}
		}

		protected virtual void ResetManualPackageFlag()
		{
			SOShipment shipment = Document.Current;
			if (shipment == null || shipment.OrderCntr != 0)
				return;

			shipment.IsManualPackage = null;
			Document.Update(shipment);
		}

		protected virtual void SOOrderShipment_ShipmentNbr_FieldVerifying(PXCache sender, PXFieldVerifyingEventArgs e)
		{
			e.Cancel = true;
		}

		[PXMergeAttributes(Method = MergeMethod.Append)]
		[PXRemoveBaseAttribute(typeof(PXDefaultAttribute))]
		[PXDBDefault(typeof(SOShipment.siteID), PersistingCheck = PXPersistingCheck.Nothing)]
		protected void SOOrderShipment_SiteID_CacheAttached(PXCache sender) { }

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

		protected virtual void SOOrderShipment_RowSelected(PXCache sender, PXRowSelectedEventArgs e)
		{
			PXUIFieldAttribute.SetEnabled(sender, e.Row, false);
			PXUIFieldAttribute.SetEnabled(sender, e.Row, true);

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

				splits.Cache.AllowInsert = sender.AllowUpdate && SyncLineWithOrder(row);
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

			if (row != null && row.IsFree != true && !sender.ObjectsEqual(e.Row, e.OldRow))
			{
				PXSelectBase selectDiscountDetailsByOrder = new PXSelect>,
					And>,
					And>,
					And>>>>>(this);

				foreach (SOShipmentDiscountDetail sdd in selectDiscountDetailsByOrder.Select(row.OrigOrderType, row.OrigOrderNbr, row.ShipmentNbr))
				{
					_discountEngine.DeleteDiscountDetail(sender, DiscountDetails, sdd);
				}

				SOOrder order = soorder.Select(row.OrigOrderType, row.OrigOrderNbr);

				if (order != null && !sender.Graph.UnattendedMode)
				{
					// Acuminator disable once PX1045 PXGraphCreateInstanceInEventHandlers - one of the graph methods is used to collect line-discount relations
					AllocateGroupFreeItems(order);
					AdjustFreeItemLines();
				}
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

			SOShipLine line = PXSelect>, And>, And>, And>>>>>>.SelectSingleBound(this, new object[] { deleted });
			if (line == null)
			{
				SOOrderShipment oship = PXSelect>, And>, And>, And>>>>>>.SelectSingleBound(this, new object[] { deleted });
				OrderList.Delete(oship);
			}

			SOOrder order = soorder.Select(deleted.OrigOrderType, deleted.OrigOrderNbr);

			if (order != null)
			{
				// Acuminator disable once PX1045 PXGraphCreateInstanceInEventHandlers - one of the graph methods is used to collect line-discount relations
				AllocateGroupFreeItems(order);
				AdjustFreeItemLines();
				deleted.KeepManualFreight = false;

				if (line == null)
				{
					Guid[] orderFileGuids = PXNoteAttribute.GetFileNotes(this.Caches[typeof(SOOrder)], order);

					foreach (NoteDoc file in this.Caches[typeof(NoteDoc)].Cached)
					{
						if (orderFileGuids.Contains(file.FileID ?? Guid.Empty) && file.NoteID == Document.Current.NoteID)
						{
							this.Caches[typeof(NoteDoc)].Delete(file);
						}
					}
				}
			}

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

		protected virtual void SOShipLine_RowUpdating(PXCache sender, PXRowUpdatingEventArgs e)
		{
			if (!SyncLineWithOrder((SOShipLine)e.Row))
			{
				e.Cancel = true;
				var error = new PXSetPropertyException(Messages.CannotEditShipLineWithDiffSite, PXErrorLevel.Warning);
				if (sender.RaiseExceptionHandling(e.NewRow, ((SOShipLine)e.Row).ShippedQty, error))
					throw error;
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

		#region AddSOFilter Events
		protected virtual void AddSOFilter_RowSelected(PXCache sender, PXRowSelectedEventArgs e)
		{
			var doc = this.Document.Current;
			PXUIFieldAttribute.SetEnabled(sender, e.Row, doc?.Operation == null);

			var filter = (AddSOFilter)e.Row;
			if (filter == null)
				return;

			var warningException =
				(doc?.FreightAmountSource == null || filter.FreightAmountSource == null || doc.FreightAmountSource == filter.FreightAmountSource)
				? null
				: new PXSetPropertyException(Messages.CantAddOrderWithFreightAmountSource, PXErrorLevel.Warning,
					sender.GetValueExt(filter));
			sender.RaiseExceptionHandling(e.Row, filter.FreightAmountSource, warningException);
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

		private const string UpsCarrierPlugin = "PX.UpsCarrier.UpsCarrier";
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
		public virtual decimal? ShipAvailableLots(SOShipmentPlan plan, SOShipLine newline, INLotSerClass lotserclass)
		{
			return CreateSplitsForAvailableLots(plan.PlanQty, plan.PlanType, plan.LotSerialNbr, newline, lotserclass);
		}

		public virtual decimal? CreateSplitsForAvailableLots(
			decimal? PlannedQty, string origPlanType, string origLotSerialNbr,
			SOShipLine newline, INLotSerClass lotserclass)
		{
			return CreateSplitsForAvailableLotsImpl<
				INLotSerialStatusByCostCenter,
				LotSerialStatusByCostCenter,
				INSiteStatusByCostCenter,
				SiteStatusByCostCenter>(PlannedQty, origPlanType, origLotSerialNbr, newline, lotserclass);
		}

		protected virtual List SelectLotSerialStatus(string origLotSerialNbr, SOShipLine newline, INLotSerClass lotserclass)
		{
			PXSelectBase cmd;
			if (!string.IsNullOrEmpty(origLotSerialNbr))
			{
				cmd = new PXSelectReadonly2,
					LeftJoin,
						And,
						And,
						And>>>>>>,
				Where>,
					And>,
					And>,
					And>,
					And,
					And>>>>>>>(this);
			}
			else
			{
				cmd = new PXSelectReadonly2,
					LeftJoin,
						And,
						And,
						And>>>>,
					InnerJoin,
						And,
						And>>>>>>,
				Where>,
					And>,
					And>,
					And>,
					And,
					And,
					And,
					And>>>>>>>>>(this);
			}

			var pars = new List(capacity: 8) { newline.InventoryID, newline.SubItemID, newline.SiteID, newline.CostCenterID };

			if (!string.IsNullOrEmpty(origLotSerialNbr))
			{
				cmd.WhereAnd>>>();
				pars.Add(origLotSerialNbr);
			}

			AppendFiltersForStatusSelect(newline, cmd, pars);

			LineSplittingExt.AppendSerialStatusCmdOrderBy(cmd, newline, lotserclass);

			return cmd.Select(pars.ToArray()).AsEnumerable().Cast().ToList();
		}

		protected virtual void AppendFiltersForStatusSelect(SOShipLine line, PXSelectBase select, List parameters)
			where TStatus : class, IBqlTable, new()
		{
			if (line.ProjectID != null && line.TaskID != null)
			{
				select.WhereAnd>>>>();
				parameters.Add(line.ProjectID);
			}

			if (IsSyncUnassignedScope && UnassignedSplitsLocationID != null)
			{
				select.WhereAnd>>>();
				parameters.Add(UnassignedSplitsLocationID);
			}
		}

		public virtual decimal? CreateSplitsForAvailableLotsImpl(
			decimal? PlannedQty, string origPlanType, string origLotSerialNbr,
			SOShipLine newline, INLotSerClass lotserclass)
			where L : class, IStatus, ILotSerial, IBqlTable, new()
			where LA : class, IStatus, ILotSerial, IBqlTable, new()
			where S : class, IStatus, IBqlTable, new()
			where SA : class, IStatus, IBqlTable, new()
		{
			if (lotserclass.LotSerTrack == INLotSerTrack.SerialNumbered)
			{
				PlannedQty = Math.Floor((decimal)PlannedQty);
			}

			List resultset = SelectLotSerialStatus(origLotSerialNbr, newline, lotserclass);
			ResortStockForShipment(newline, resultset);

			PXCache lcache = Caches[typeof(L)];
			PXCache scache = Caches[typeof(S)];
			PXCache tcache = Caches[typeof(INSiteLotSerial)];

			bool isFullLineAllocation = (PlannedQty >= newline.BaseShippedQty);
			int locCounter = 0;
			int? assignedLocation = null;
			int? assignedTaskID = null;
			if (string.IsNullOrEmpty(origLotSerialNbr))
			{
				foreach (PXResult available in resultset)
				{
					var location = (INLocation)available;
					if (locCounter > 0 && newline.TaskID != null && assignedTaskID != location.TaskID)
					{
						continue;
					}

					L avail = (L)available;
					INSiteLotSerial siteLotAvail = (INSiteLotSerial)available;

					LA accumavail = new LA();
					lcache.RestoreCopy(accumavail, avail);

					SiteLotSerial accumSiteLotAvail = new SiteLotSerial();
					tcache.RestoreCopy(accumSiteLotAvail, siteLotAvail);

					accumSiteLotAvail = (SiteLotSerial)this.Caches[typeof(SiteLotSerial)].Insert(accumSiteLotAvail);

					accumavail = (LA)this.Caches[typeof(LA)].Insert(accumavail);

					S siteavail = (S)available;
					SA accumsiteavail = new SA();
					scache.RestoreCopy(accumsiteavail, siteavail);
					accumsiteavail = (SA)this.Caches[typeof(SA)].Insert(accumsiteavail);

					decimal? AvailableQty = 0m;

					decimal? SiteLotAvailableQty = siteLotAvail.QtyHardAvail + accumSiteLotAvail.QtyHardAvail;
					decimal? StatusAvailableQty = avail.QtyHardAvail + accumavail.QtyHardAvail;
					decimal? SiteAvailableQty = siteavail.QtyHardAvail + accumsiteavail.QtyHardAvail;

					//We should not check INSiteStatus for allocated lines
					if (!origPlanType.IsIn(INPlanConstants.Plan61, INPlanConstants.Plan63, INPlanConstants.PlanM7))
					{
						AvailableQty = Math.Min(SiteAvailableQty.GetValueOrDefault(), Math.Min(SiteLotAvailableQty.GetValueOrDefault(), StatusAvailableQty.GetValueOrDefault()));
					}
					else
					{
						AvailableQty = Math.Min(SiteLotAvailableQty.GetValueOrDefault(), StatusAvailableQty.GetValueOrDefault());
					}

					if (AvailableQty <= 0m)
					{
						continue;
					}

					IBqlTable newsplit = (newline.IsUnassigned == true) ? (IBqlTable)newline.ToUnassignedSplit() : (SOShipLineSplit)newline;
					PXCache cache = (newline.IsUnassigned == true) ? unassignedSplits.Cache : splits.Cache;

					cache.SetValue(newsplit, null);
					cache.SetValue(newsplit, null);
					cache.SetValue(newsplit, avail.LocationID);
					cache.SetValue(newsplit, newline.IsUnassigned == true ? string.Empty : avail.LotSerialNbr);
					cache.SetValue(newsplit, avail.ExpireDate);
					cache.SetValue(newsplit, newline.IsUnassigned);
					cache.SetValue(newsplit, (AvailableQty < PlannedQty) ? AvailableQty : PlannedQty);
					cache.SetValue(newsplit, null);
					cache.Insert(newsplit);

					if (locCounter == 0)
					{
						if (newline.TaskID != null)
							assignedTaskID = location.TaskID;
						assignedLocation = location.LocationID;
					}
					else if (assignedLocation != location.LocationID)
					{
						assignedLocation = null;
					}
					locCounter++;

					if (AvailableQty < PlannedQty)
					{
						PlannedQty -= AvailableQty;
					}
					else
					{
						PlannedQty = 0m;
						break;
					}
				}
			}
			else
			{
				foreach (PXResult available in resultset)
				{
					var location = (INLocation)available;
					if (locCounter > 0 && newline.TaskID != null && assignedTaskID != location.TaskID)
					{
						continue;
					}

					L avail = (L)available;
					LA accumavail = new LA();
					lcache.RestoreCopy(accumavail, avail);

					S siteavail = (S)available;
					SA accumsiteavail = new SA();
					scache.RestoreCopy(accumsiteavail, siteavail);

					accumavail = (LA)this.Caches[typeof(LA)].Insert(accumavail);
					accumsiteavail = (SA)this.Caches[typeof(SA)].Insert(accumsiteavail);

					decimal? AvailableQty = avail.QtyHardAvail + accumavail.QtyHardAvail;
					decimal? SiteAvailableQty = siteavail.QtyHardAvail + accumsiteavail.QtyHardAvail;

					//We should not check INSiteStatus for allocated lines
					AvailableQty = (SiteAvailableQty < AvailableQty && !origPlanType.IsIn(INPlanConstants.Plan61, INPlanConstants.Plan63)) ? SiteAvailableQty : AvailableQty;

					if (AvailableQty <= 0m)
					{
						continue;
					}

					IBqlTable newsplit = (newline.IsUnassigned == true) ? (IBqlTable)newline.ToUnassignedSplit() : (SOShipLineSplit)newline;
					PXCache cache = (newline.IsUnassigned == true) ? unassignedSplits.Cache : splits.Cache;

					cache.SetValue(newsplit, null);
					cache.SetValue(newsplit, null);
					cache.SetValue(newsplit, avail.LocationID);
					cache.SetValue(newsplit, avail.LotSerialNbr);
					cache.SetValue(newsplit, avail.ExpireDate);
					cache.SetValue(newsplit, newline.IsUnassigned);
					cache.SetValue(newsplit, (AvailableQty < PlannedQty) ? AvailableQty : PlannedQty);
					cache.SetValue(newsplit, null);
					cache.Insert(newsplit);

					if (locCounter == 0)
					{
						if (newline.TaskID != null)
							assignedTaskID = location.TaskID;
						assignedLocation = location.LocationID;
					}
					else if (assignedLocation != location.LocationID)
					{
						assignedLocation = null;
					}
					locCounter++;

					if (AvailableQty < PlannedQty)
					{
						PlannedQty -= AvailableQty;
					}
					else
					{
						PlannedQty = 0m;
						break;
					}
				}
			}

			if (newline.IsUnassigned == true && isFullLineAllocation && assignedLocation != null)
			{
				/// for assigned lines the location is set by 
				this.Transactions.Cache.SetValue(newline, assignedLocation);
			}

			return PlannedQty;
		}

		public virtual decimal? ShipAvailableNonLots(SOShipmentPlan plan, SOShipLine newline, INLotSerClass lotserclass)
		{
			return CreateSplitsForAvailableNonLots(plan.PlanQty, plan.PlanType, newline, lotserclass);
		}

		public virtual decimal? CreateSplitsForAvailableNonLots(
			decimal? PlannedQty, string origPlanType,
			SOShipLine newline, INLotSerClass lotserclass)
		{
			return CreateSplitsForAvailableNonLotsImpl<
				INLocationStatusByCostCenter,
				LocationStatusByCostCenter,
				INSiteStatusByCostCenter,
				SiteStatusByCostCenter>(PlannedQty, origPlanType, newline, lotserclass);
		}

		protected virtual List SelectLocationStatus(SOShipLine newline)
		{
			var select = new PXSelectReadonly2,
				LeftJoin,
					And,
					And,
					And>>>>>>,
				Where>,
					And>,
					And>,
					And,
					And>>>>>,
				OrderBy>>>(this);

			var pars = new List(capacity: 8) { newline.InventoryID, newline.SiteID, newline.CostCenterID };
			if (PXAccess.FeatureInstalled())
			{
				select.WhereAnd>>>();
				pars.Add(newline.SubItemID);
			}

			AppendFiltersForStatusSelect(newline, select, pars);

			return select.Select(pars.ToArray()).AsEnumerable().Cast().ToList();
		}

		public virtual decimal? CreateSplitsForAvailableNonLotsImpl(
			decimal? PlannedQty, string origPlanType,
			SOShipLine newline, INLotSerClass lotserclass)
			where L : class, IStatus, IBqlTable, new()
			where LA : class, IStatus, IBqlTable, new()
			where S : class, IStatus, IBqlTable, new()
			where SA : class, IStatus, IBqlTable, new()
		{
			List resultset = SelectLocationStatus(newline);
			ResortStockForShipment(newline, resultset);

			bool isFullLineAllocation = (PlannedQty >= newline.BaseShippedQty);
			int locCounter = 0;
			int? assignedLocation = null;
			int? assignedTaskID = null;
			PXCache lcache = Caches[typeof(L)];
			PXCache scache = Caches[typeof(S)];
			foreach (PXResult available in resultset)
			{
				var location = (INLocation)available;
				if (locCounter > 0 && newline.TaskID != null && assignedTaskID != location.TaskID)
				{
					continue;
				}

				L avail = (L) available;
				LA accumavail = new LA();
				lcache.RestoreCopy(accumavail, avail);

				S siteavail = (S)available;
				SA accumsiteavail = new SA();
				scache.RestoreCopy(accumsiteavail, siteavail);

				accumavail = (LA)this.Caches[typeof(LA)].Insert(accumavail);
				accumsiteavail = (SA)this.Caches[typeof(SA)].Insert(accumsiteavail);

				decimal? AvailableQty = avail.QtyHardAvail + accumavail.QtyHardAvail;
				decimal? SiteAvailableQty = siteavail.QtyHardAvail + accumsiteavail.QtyHardAvail;

				//We should not check INSiteStatus for allocated lines
				AvailableQty = (SiteAvailableQty < AvailableQty && !origPlanType.IsIn(INPlanConstants.Plan61, INPlanConstants.Plan63)) ? SiteAvailableQty : AvailableQty;

				if (AvailableQty <= 0m)
				{
					continue;
				}

				InsertSplitsForNonLotsOnLocation(newline, lotserclass, location.LocationID, AvailableQty, PlannedQty);

				if (locCounter == 0)
				{
					if (newline.TaskID != null)
						assignedTaskID = location.TaskID;
					assignedLocation = location.LocationID;
				}
				else if (assignedLocation != location.LocationID)
				{
					assignedLocation = null;
				}
				locCounter++;

				if (AvailableQty < PlannedQty)
				{
					PlannedQty -= AvailableQty;
				}
				else
				{
					PlannedQty = 0m;
					break;
				}
			}

			if (PlannedQty > 0m && (lotserclass.LotSerTrack == INLotSerTrack.NotNumbered || lotserclass.LotSerAssign == INLotSerAssign.WhenUsed))
			{
				InventoryItem item = InventoryItem.PK.Find(this, newline.InventoryID);
				if (item?.NegQty == true)
				{
					SOOrderType orderType = soordertype.Select(newline.OrigOrderType);
					if (orderType?.ShipFullIfNegQtyAllowed == true)
					{
						int? locationID = GetLocationIDForNotAvailableStock(item, newline.SiteID);
						if (locationID == null)
						{
							throw new PXException(Messages.NegShipmentCantBeCreatedLocationNotSetup, item.InventoryCD);
						}

						bool addNegQtyLocation = true;
						if (locCounter > 0)
						{
							INLocation location = INLocation.PK.Find(this, locationID);
							addNegQtyLocation = (location?.TaskID == assignedTaskID);
						}

						if (addNegQtyLocation)
						{
							InsertSplitsForNonLotsOnLocation(newline, lotserclass, locationID, PlannedQty, PlannedQty);
							PlannedQty = 0m;
							if (locCounter == 0)
							{
								assignedLocation = locationID;
							}
							else if (assignedLocation != locationID)
							{
								assignedLocation = null;
							}
						}
					}
				}
			}

			if (newline.IsUnassigned == true && isFullLineAllocation && assignedLocation != null)
			{
				/// for assigned lines the location is set by 
				this.Transactions.Cache.SetValue(newline, assignedLocation);
			}

			return PlannedQty;
		}

		protected virtual void ResortStockForShipment(SOShipLine newline, List resultset)
		{
			ResortStockForShipmentByDefaultItemLocation(newline, resultset);
			ResortStockForShipmentByProjectAndTask(newline, resultset);
		}

		protected virtual void ResortStockForShipmentByDefaultItemLocation(SOShipLine newline, List resultset)
		{
			if (INSite.PK.Find(this, newline.SiteID)?.UseItemDefaultLocationForPicking != true)
				return;

			var dfltShipLocationID = INItemSite.PK.Find(this, newline.InventoryID, newline.SiteID)?.DfltShipLocationID;
			if (dfltShipLocationID == null)
				return;

			var listOrderedByDfltShipLocationID = resultset.OrderByDescending(
				r => PXResult.Unwrap(r).LocationID == dfltShipLocationID).ToList();
			resultset.Clear();
			resultset.AddRange(listOrderedByDfltShipLocationID);
		}

		protected virtual void ResortStockForShipmentByProjectAndTask(SOShipLine newline, List resultset)
		{
			if (newline.ProjectID == null || newline.TaskID == null)
				return;

			int capacity = resultset.Count;
			var first = new List(capacity);//matching ProjectID and TaskID
			var second = new List(capacity);//matching ProjectID, TaskID not specified
			var third = new List(capacity);//ProjectID and TaskID not specified
			var forth = new List(capacity);//matching ProjectID, different TaskID

			foreach (PXResult available in resultset)
			{
				INLocation location = PXResult.Unwrap(available);
				if (location.ProjectID != null && location.ProjectID == newline.ProjectID && location.TaskID == newline.TaskID)
				{
					first.Add(available);
				}
				else if (location.ProjectID != null && location.ProjectID == newline.ProjectID && location.TaskID == null)
				{
					second.Add(available);
				}
				else if (location.ProjectID == null && location.TaskID == null)
				{
					third.Add(available);
				}
				else if (location.ProjectID != null && location.ProjectID == newline.ProjectID && location.TaskID != null)
				{
					forth.Add(available);
				}
			}

			resultset.Clear();
			resultset.AddRange(first);
			resultset.AddRange(second);
			resultset.AddRange(third);
			resultset.AddRange(forth);
		}


		public virtual void InsertSplitsForNonLotsOnLocation(SOShipLine newline, INLotSerClass lotserclass, int? locationID, decimal? availableQty, decimal? plannedQty)
        {
            IBqlTable newsplit = (newline.IsUnassigned == true) ? (IBqlTable)newline.ToUnassignedSplit() : (SOShipLineSplit)newline;
            PXCache cache = (newline.IsUnassigned == true) ? unassignedSplits.Cache : splits.Cache;

			cache.SetValue(newsplit, null);
			cache.SetValue(newsplit, null);
			cache.SetValue(newsplit, locationID);
            cache.SetValue(newsplit, newline.IsUnassigned);
            if (newline.IsUnassigned == true)
            {
                cache.SetValue(newsplit, string.Empty);
            }

            if (newline.IsClone == false)
            {
                PXParentAttribute.SetParent(cache, newsplit, typeof(SOShipLine), newline);
            }

            decimal? qtyAllocate = (availableQty < plannedQty) ? availableQty : plannedQty;
            if (lotserclass.LotSerTrack == INLotSerTrack.SerialNumbered &&
                (lotserclass.LotSerAssign != INLotSerAssign.WhenUsed
                || newline.ShipmentType != SOShipmentType.Transfer && newline.IsIntercompany != true))
            {
				cache.SetValue(newsplit, 1m);
                cache.SetValue(newsplit, 1m);

                for (int i = 0; i < (int)qtyAllocate; i++)
                {
                    cache.Insert(newsplit);
                }
            }
            else
            {
				cache.SetValue(newsplit, qtyAllocate);
                cache.SetValue(newsplit, null);
                cache.Insert(newsplit);
            }
        }

		public virtual int? GetLocationIDForNotAvailableStock(InventoryItem item, int? siteID)
		{
			var itemSite = (INItemSite)PXSelectReadonly>,
					And>>>>
				.Select(this, item.InventoryID, siteID);
			if (itemSite?.DfltShipLocationID != null)
			{
				return itemSite.DfltShipLocationID;
			}

			var site = INSite.PK.Find(this, siteID);
			InventoryItemCurySettings itemCurySettings = InventoryItemCurySettings.PK.Find(this, item.InventoryID, site?.BaseCuryID);

			if (itemCurySettings?.DfltSiteID == siteID && itemCurySettings.DfltShipLocationID != null)
			{
				return itemCurySettings.DfltShipLocationID;
			}

			return site?.ShipLocationID;
		}

		public virtual decimal? ShipNonStock(SOShipmentPlan plan, SOShipLine newline)
		{
			decimal? PlannedQty = plan.PlanQty;

			SOShipLineSplit newsplit = (SOShipLineSplit)newline;
			newsplit.UOM = null;
			newsplit.SplitLineNbr = null;
			newsplit.LocationID = INSite.PK.Find(this, newsplit.SiteID)?.NonStockPickingLocationID;
			newsplit.Qty = PlannedQty;
			newsplit.BaseQty = null;
			splits.Insert(newsplit);

			return 0m;
		}

		public virtual decimal? ShipAvailable(SOShipmentPlan plan, SOShipLine newline, PXResult item)
		{
			INLotSerClass lotserclass = item;
			InventoryItem initem = item;

			if (initem.StkItem == false && initem.KitItem == true)
			{
				decimal? kitqty = plan.PlanQty;
				object lastComponentID = null;
				bool HasSerialComponents = false;
				SOShipLine copy;

				ShipNonStockKit(plan, newline, ref kitqty, ref lastComponentID, ref HasSerialComponents);

                bool hassplits = false;
                foreach(SOShipLineSplit split in splits.Cache.Inserted)
                {
                    if (split.ShipmentNbr == newline.ShipmentNbr && split.LineNbr == newline.LineNbr)
                    {
                        hassplits = true;
                        break;
                    }
                }

                if (!hassplits)
                {
                    RemoveLineFromShipment(newline, true);
                    return 0m;
                }

				copy = PXCache.CreateCopy(newline);
				copy.ShippedQty = (copy.UOM == copy.OrderUOM && kitqty == copy.BaseFullOrderQty) ? copy.FullOrderQty
					: INUnitAttribute.ConvertFromBase(Transactions.Cache, copy, copy.UOM, (decimal)kitqty, INPrecision.QUANTITY, INMidpointRounding.FLOOR);
				LineSplittingExt.LastComponentID = (int?)lastComponentID;
				try
				{
					Transactions.Update(copy);
				}
				finally
				{
					LineSplittingExt.LastComponentID = null;
				}

				return 0m;
			}
			else if (lotserclass == null || lotserclass.LotSerTrack == null)
			{
				return ShipNonStock(plan, newline);
			}
			else if (lotserclass.LotSerTrack == INLotSerTrack.NotNumbered || lotserclass.LotSerAssign == INLotSerAssign.WhenUsed || newline.IsUnassigned == true)
			{
				return ShipAvailableNonLots(plan, newline, lotserclass);
			}
			else
			{
				return ShipAvailableLots(plan, newline, lotserclass);
			}
		}

		public virtual void ReceiveLotSerial(SOShipmentPlan plan, SOShipLine newline, SOLineSplit soSplit, PXResult item)
		{
			PXSelectBase cmd = new PXSelectReadonly2>,
				Where>,
					And>,
					And>,
					And>,
					And>>>>>>(this);

			if (!string.IsNullOrEmpty(plan.LotSerialNbr))
			{
				cmd.WhereAnd>>>();
			}

			INLotSerialStatusByCostCenter avail = cmd.SelectWindowed(0, 1, newline.InventoryID, newline.SubItemID, newline.SiteID, newline.CostCenterID, plan.LotSerialNbr);

				SOShipLineSplit newsplit = (SOShipLineSplit)newline;
				newsplit.UOM = null;
				newsplit.Qty = newsplit.BaseQty;
				newsplit.SplitLineNbr = null;
			if (avail != null)
			{
				if (newsplit.LocationID == null)
					newsplit.LocationID = avail.LocationID;
				newsplit.LotSerialNbr = avail.LotSerialNbr;
				newsplit.ExpireDate = soSplit?.ExpireDate ?? avail.ExpireDate;
			}
			else
			{
				INSite site = INSite.PK.Find(this, newline.SiteID);
				newsplit.LocationID = site.ReturnLocationID;
				newsplit.LotSerialNbr = plan.LotSerialNbr;
				newsplit.ExpireDate = soSplit?.ExpireDate ?? newline.ExpireDate;
			}

			if (!string.IsNullOrEmpty(plan.LotSerialNbr))
					splits.Update(newsplit);
			}

        public virtual void PromptReplenishment(PXCache sender, SOShipLine newline, InventoryItem item, SOShipmentPlan plan)
		{
			if (newline.ProjectID != null && newline.TaskID != null)
			{
				// we can't prompt replenishment reliably for lines assigned to project and task
				return;
			}

            decimal planrequired = (plan.PlanQty ?? 0m) - newline.BaseShippedQty.GetValueOrDefault();
            decimal qtyrequired = planrequired;

            SOLine soLine = PXSelect>,
            And>,
            And>>>>>.Select(this, newline.OrigOrderType, newline.OrigOrderNbr, newline.OrigLineNbr);

            if (item.StkItem == false && item.KitItem == true)
            {
                if (soLine.ShipComplete != SOShipComplete.ShipComplete)
                {
                    //if it's not shipcomplete than we must check if we can assemble at least one non-stock kit
                    qtyrequired = 1;
            }

                List itemsNotAvailable = new List();
                decimal? maxPromptQty = null;

				foreach (PXResult compres in PXSelectJoin>>,
					Where>>>.Select(this, newline.InventoryID))
				{
                    INKitSpecStkDet spec = (INKitSpecStkDet)compres;

					if (spec.DfltCompQty.GetValueOrDefault() == 0)
						continue;

                    Tuple availability = CalculateItemAvailability(spec.CompInventoryID, spec.CompSubItemID, newline.SiteID, newline.CostCenterID);

                    if ((qtyrequired * spec.DfltCompQty) > availability.Item1)
                    {
                        //actually it's a error, but it will be thrown further
                        return;
                    }
                    else
					{
                        decimal possibleQty = Math.Floor(availability.Item1 / spec.DfltCompQty.Value);
                        if (maxPromptQty == null || possibleQty < maxPromptQty)
                            maxPromptQty = possibleQty;
                    }
                    }
                if (maxPromptQty <= 0m)
                    return;

                foreach (PXResult compres in PXSelectJoin>>,
                  Where>>>.Select(this, newline.InventoryID))
                {
                    INKitSpecStkDet spec = (INKitSpecStkDet)compres;
					if (spec.DfltCompQty.GetValueOrDefault() == 0)
						continue;

                    Tuple availability = CalculateItemAvailability(spec.CompInventoryID, spec.CompSubItemID, newline.SiteID, newline.CostCenterID);

                    if (availability.Item2 < (maxPromptQty * spec.DfltCompQty))
                        itemsNotAvailable.Add((InventoryItem)compres);
					}

                if (itemsNotAvailable.Count == 0)
                    return;

                StringBuilder invetoryCDs = new StringBuilder(itemsNotAvailable[0].InventoryCD);
                for (int i = 1; i < itemsNotAvailable.Count; i++)
                {
                    invetoryCDs.Append(", " + itemsNotAvailable[i].InventoryCD);
				}

                throw new PXException(Messages.PromptReplenishment, invetoryCDs);
			}
			else
			{
                Tuple availability = CalculateItemAvailability(newline.InventoryID, newline.SubItemID, newline.SiteID, newline.CostCenterID);

                if (soLine.ShipComplete != SOShipComplete.ShipComplete)
                {
                    //if it's not shipcomplete than we must throw error if we can ship at least smthing more
                    qtyrequired = 0m;
                }

                //actually it's a error, but it will be thrown further
                if (qtyrequired > availability.Item1)
                    return;

                if (availability.Item1 > 0m)
					throw new PXException(Messages.PromptReplenishment, sender.GetValueExt(newline));
				}
			}

        private Tuple CalculateItemAvailability(int? inventoryID, int? subItemID, int? siteID, int? costCenterID)
        {
            decimal totalAvalableQty = 0;
            decimal totalAvalableForSalesQty = 0;

			INSiteStatusByCostCenter sitestatus = PXSelectReadonly>,
				And>,
				And>,
				And>,
					Or, IsNull>>>>>>>
				.SelectSingleBound(this, new object[] { }, inventoryID, siteID, costCenterID, subItemID, subItemID);

			// AC-71766: Correction is required to consider items allocated in Sales Order but without created shipment yet.
			// This items are considered in SiteStatus but not considered in LocationStatus yet.
			decimal allocatedcorrection = 0m;
			if (sitestatus != null)
			{
				allocatedcorrection = -1 * (
					(sitestatus.QtySOShipping ?? 0m) +
					(sitestatus.QtyFSSrvOrdAllocated ?? 0m) +
					(sitestatus.QtyProductionAllocated ?? 0m));
			}

			var select = new PXSelectReadonly2>,
				Where>,
					And>,
					And>,
					And>>>>,
				OrderBy>>(this);

			object[] pars;
			if (PXAccess.FeatureInstalled())
			{
				select.WhereAnd>>>();
				pars = new object[] { inventoryID, siteID, costCenterID, subItemID };
			}
			else
			{
				pars = new object[] { inventoryID, siteID, costCenterID };
			}
			PXResultset resultset = select.Select(pars);

			foreach (PXResult res in resultset)
            {
                INLocation loc = (INLocation)res;
				INLocationStatusByCostCenter avail = (INLocationStatusByCostCenter)res;
				LocationStatusByCostCenter accumavail = new LocationStatusByCostCenter();
				PXCache.RestoreCopy(accumavail, avail);
				accumavail = (LocationStatusByCostCenter)this.Caches[typeof(LocationStatusByCostCenter)].Insert(accumavail);

                allocatedcorrection +=
					(avail.QtySOShipping ?? 0m) +
					(avail.QtyFSSrvOrdAllocated ?? 0m) +
					(avail.QtyProductionAllocated ?? 0m);
                decimal qtyAvailable = avail.QtyHardAvail.GetValueOrDefault() + accumavail.QtyHardAvail.GetValueOrDefault();
                totalAvalableQty += qtyAvailable;
                if (loc.SalesValid == true)
                {
                    totalAvalableForSalesQty += qtyAvailable;
                }
            }

            return new Tuple(totalAvalableQty + allocatedcorrection, totalAvalableForSalesQty + allocatedcorrection);
		}

		public virtual void ShipNonStockKit(SOShipmentPlan plan, SOShipLine newline, ref decimal? kitqty, ref object lastComponentID, ref bool HasSerialComponents)
		{
			SOShipLine copy;
		    object lastSubitemID = null;

			using (LineSplittingExt.KitProcessingScope(InventoryItem.PK.Find(this, newline.InventoryID)))
			{
				foreach (PXResult compres in
					PXSelectJoin,
					InnerJoin>>,
					Where>>>.Select(this, newline.InventoryID))
				{
					INKitSpecStkDet compitem = (INKitSpecStkDet)compres;
					InventoryItem component = (InventoryItem)compres;

					if (component.ItemStatus == INItemStatus.Inactive)
					{
						throw new PXException(Messages.KitComponentIsInactive, component.InventoryCD);
					}
					copy = LineSplittingExt.Clone(newline);

					copy.IsStockItem = true;
					copy.InventoryID = compitem.CompInventoryID;
					copy.SubItemID = compitem.CompSubItemID;
					copy.UOM = compitem.UOM;
					copy.Qty = compitem.DfltCompQty * plan.PlanQty;

					//clear splits with correct ComponentID
					LineSplittingExt.RaiseRowDeleted(copy);

					SOShipmentPlan plancopy = PXCache.CreateCopy(plan);
					plancopy.PlanQty = INUnitAttribute.ConvertToBase(Transactions.Cache, copy, copy.UOM, (decimal)copy.Qty, INPrecision.QUANTITY);
					if (copy.Operation == SOOperation.Receipt)
					{
						INSite site = INSite.PK.Find(this, copy.SiteID);
						if (site != null)
						{
							if (site.ReturnLocationID == null)
								throw new PXException(Messages.NoRMALocation, site.SiteCD);

							if (((INLotSerClass)compres).LotSerTrack == INLotSerTrack.SerialNumbered)
							{
								for ( int i = 0; i < copy.Qty; i++ )
								{
									SOShipLineSplit newsplit = (SOShipLineSplit)copy;
									newsplit.Qty = 1;
									newsplit.SplitLineNbr = null;
									newsplit.LocationID = site.ReturnLocationID;
									newsplit = splits.Insert(newsplit);
									PXDefaultAttribute.SetPersistingCheck(splits.Cache, newsplit, PXPersistingCheck.Nothing);
									PXDefaultAttribute.SetPersistingCheck(splits.Cache, newsplit, PXPersistingCheck.Nothing);
								}
							}
							else
							{
								SOShipLineSplit newsplit = (SOShipLineSplit)copy;
								newsplit.SplitLineNbr = null;
								newsplit.LocationID = site.ReturnLocationID;
								newsplit = splits.Insert(newsplit);
								PXDefaultAttribute.SetPersistingCheck(splits.Cache, newsplit, PXPersistingCheck.Nothing);
								PXDefaultAttribute.SetPersistingCheck(splits.Cache, newsplit, PXPersistingCheck.Nothing);
							}
						}
					}
					else
					{
						decimal? unshippedqty = ShipAvailable(plancopy, copy, new PXResult(compres, compres));

						if (plancopy.PlanQty != 0m && (plancopy.PlanQty - unshippedqty) * plan.PlanQty / plancopy.PlanQty < kitqty)
						{
							kitqty = (plancopy.PlanQty - unshippedqty) * plan.PlanQty / plancopy.PlanQty;
							lastComponentID = copy.InventoryID;
							lastSubitemID = copy.SubItemID;

						}
					}
					HasSerialComponents |= ((INLotSerClass)compres).LotSerTrack == INLotSerTrack.SerialNumbered;
				}
			}

			foreach (PXResult compres in PXSelectJoin>,
				Where>,
					And, Or>>>>>.Select(this, newline.InventoryID))
			{
				INKitSpecNonStkDet compitem = compres;
				InventoryItem item = compres;

				copy = LineSplittingExt.Clone(newline);

				copy.IsStockItem = false;
				copy.InventoryID = compitem.CompInventoryID;
				copy.SubItemID = null;
				copy.UOM = compitem.UOM;
				copy.Qty = compitem.DfltCompQty * plan.PlanQty;

				//clear splits with correct ComponentID
				LineSplittingExt.RaiseRowDeleted(copy);

				SOShipmentPlan plancopy = PXCache.CreateCopy(plan);
				plancopy.PlanQty = INUnitAttribute.ConvertToBase(Transactions.Cache, copy, copy.UOM, (decimal)copy.Qty, INPrecision.QUANTITY);

				if (item.StkItem == false && item.KitItem == true)
				{
					decimal? subkitqty = plancopy.PlanQty;

					ShipNonStockKit(plancopy, copy, ref subkitqty, ref lastComponentID, ref HasSerialComponents);

					if (plancopy.PlanQty != 0m && subkitqty * plan.PlanQty / plancopy.PlanQty < kitqty)
					{
						kitqty = subkitqty * plan.PlanQty / plancopy.PlanQty;
					}
				}
				else
				{
					ShipAvailable(plancopy, copy, new PXResult(compres, null));
				}
			}

			if (HasSerialComponents)
			{
				kitqty = decimal.Floor((decimal)kitqty);
			}

			if (kitqty <= 0m
				&& lastComponentID != null)
			{
				object lastComponentCD = lastComponentID;
			    object lastSubitemCD = lastSubitemID;

				Transactions.Cache.RaiseFieldSelecting(newline, ref lastComponentCD, true);
			    Transactions.Cache.RaiseFieldSelecting(newline, ref lastSubitemCD, true);

			    if (PXAccess.FeatureInstalled() && lastSubitemID != null)
			    {
                    PXTrace.WriteInformation(Messages.ItemWithSubitemNotAvailableTraced, lastComponentCD, Transactions.GetValueExt(newline), lastSubitemCD);
                }
			    else
			    {
                    PXTrace.WriteInformation(Messages.ItemNotAvailableTraced, lastComponentCD, Transactions.GetValueExt(newline));
                }
			}
		}

		public virtual bool RemoveLineFromShipment(SOShipLine shipline, bool RemoveFlag)
		{
			if (RemoveFlag)
			{
				if (shipline.CostCenterID != CostCenter.FreeStock && INCostCenter.PK.Find(this, shipline.CostCenterID)?.CostLayerType == CostLayerType.Special)
				{
					PXTrace.WriteInformation(Messages.ItemNotAvailableTraced_Special, Transactions.GetValueExt(shipline), Transactions.GetValueExt(shipline));
				}
				else if (PXAccess.FeatureInstalled() && shipline != null && shipline.SubItemID != null)
                {
                    PXTrace.WriteInformation(Messages.ItemWithSubitemNotAvailableTraced, Transactions.GetValueExt(shipline), Transactions.GetValueExt(shipline), Transactions.GetValueExt(shipline));
                }
                else
                {
                    PXTrace.WriteInformation(Messages.ItemNotAvailableTraced, Transactions.GetValueExt(shipline), Transactions.GetValueExt(shipline));
                }
				shipline.KeepManualFreight = true;
				Transactions.Delete(shipline);
				return true;
			}

			Transactions.Cache.RaiseExceptionHandling(shipline, null, new PXSetPropertyException(Messages.ItemNotAvailable, PXErrorLevel.RowWarning));
			return false;
		}

		public virtual bool CreateShipmentFromSchedules(PXResult res, SOShipLine newline, SOOrderType ordertype, string operation, DocumentList list)
		{
			bool deleted = false;

			SOShipmentPlan plan = res;
			SOLine line = res;
			SOLineSplit linesplit = res;
			INSite site = res;
			InventoryItem item = res;
			bool requireAllocationUnallocated = plan.RequireAllocation == true && linesplit.LineType != SOLineType.NonInventory && linesplit.Operation != SOOperation.Receipt && plan.InclQtySOShipping != 1 && plan.InclQtySOShipped != 1;
			bool addZeroLineForUnallocated = requireAllocationUnallocated && sosetup.Current.AddAllToShipment == true;

			if (plan.Selected == true || list != null && (!requireAllocationUnallocated || sosetup.Current.AddAllToShipment == true))
			{
				ValidateLineBeforeShipment(line);

				newline.OrigOrderType = line.OrderType;
				newline.OrigOrderNbr = line.OrderNbr;
				newline.OrigLineNbr = line.LineNbr;
				newline.OrigPlanType = (linesplit.POCreate != true && linesplit.IsAllocated != true) ? linesplit.PlanType: plan.PlanType;
				newline.IsStockItem = line.IsStockItem;
				newline.InventoryID = line.InventoryID;
				newline.SubItemID = line.SubItemID;
				newline.SiteID = line.SiteID;
				newline.TranDesc = line.TranDesc;
				newline.CustomerID = line.CustomerID;
				newline.InvtMult = line.OrderQty < 0m ? (short?)-line.InvtMult : line.InvtMult;
				newline.SOLineSign = line.LineSign;
				newline.Operation = line.Operation;
				newline.LineType = line.LineType;
				newline.ReasonCode = line.ReasonCode;
				newline.ProjectID = line.ProjectID;
				newline.TaskID = line.TaskID;
				newline.CostCodeID = line.CostCodeID;
				newline.UOM = linesplit.UOM;
				newline.IsFree = line.IsFree;
				newline.ManualDisc = line.ManualDisc;

				newline.DiscountID = line.DiscountID;
				newline.DiscountSequenceID = line.DiscountSequenceID;

				newline.AlternateID = line.AlternateID;
				newline.BlanketType = line.BlanketType;
				newline.BlanketNbr = line.BlanketNbr;
				newline.BlanketLineNbr = line.BlanketLineNbr;
				newline.BlanketSplitLineNbr = line.BlanketSplitLineNbr;

				newline.IsSpecialOrder = line.IsSpecialOrder;
				newline.CostCenterID = line.CostCenterID;

				UpdateOrigValues(newline, line, plan.PlanQty);

				ValidateLineType(line, item, Messages.CannotCreateShipmentNonInventoryNonStockKit);

				INLotSerClass lotSerClass = (INLotSerClass)res;
				bool isNonStock = lotSerClass.LotSerTrack == null;
				if (isNonStock)
				{
					newline.ShippedQty = (newline.UOM == newline.OrderUOM && plan.PlanQty == newline.BaseFullOrderQty) ? newline.FullOrderQty
						: INUnitAttribute.ConvertFromBase(Transactions.Cache, newline.InventoryID, newline.UOM, (decimal)plan.PlanQty, INPrecision.QUANTITY);
					newline = LineSplittingExt.InsertWithoutSplits(newline);

					try
					{
						ShipAvailable(plan, newline, new PXResult(res, res));
					}
					catch (PXException ex)
					{
						LineSplittingExt.lsselect.Delete(newline);
						throw ex;
					}
				}
				else if (operation == SOOperation.Receipt)
				{
					newline.ShippedQty = (newline.UOM == newline.OrderUOM && plan.PlanQty == newline.BaseFullOrderQty) ? newline.FullOrderQty
						: INUnitAttribute.ConvertFromBase(Transactions.Cache, newline.InventoryID, newline.UOM, (decimal)plan.PlanQty, INPrecision.QUANTITY);
					newline.LocationID = site.ReturnLocationID;
					if (newline.LocationID == null && list != null)
						throw new PXException(Messages.NoRMALocation, site.SiteCD);
					newline = Transactions.Insert(newline);
					ReceiveLotSerial(plan, newline, linesplit, new PXResult(res, res));
				}
				else
				{
					SOShipLine existing = (SOShipLine)Transactions.Cache.Locate(newline);
					if (existing == null || Transactions.Cache.GetStatus(existing).IsIn(PXEntryStatus.Deleted, PXEntryStatus.InsertedDeleted))
					{
						newline.ShippedQty = 0m;
						newline = LineSplittingExt.InsertWithoutSplits(newline);
					}
					if (!addZeroLineForUnallocated)
					{
					newline.IsUnassigned = lotSerClass.IsManualAssignRequired == true && plan.PlanQty > 0 && string.IsNullOrEmpty(plan.LotSerialNbr)
						&& (lotSerClass.LotSerAssign != INLotSerAssign.WhenUsed || newline.ShipmentType != SOShipmentType.Transfer && newline.IsIntercompany != true);

					decimal? notShipped = ShipAvailable(plan, newline, new PXResult(res, res));
					if (newline.IsUnassigned == true)
					{
						var oldRow = (SOShipLine)Transactions.Cache.CreateCopy(newline);
						newline.UnassignedQty = plan.PlanQty - notShipped;
						newline.BaseShippedQty = plan.PlanQty - notShipped;
						newline.ShippedQty = (newline.UOM == newline.OrderUOM && newline.BaseShippedQty == newline.BaseFullOrderQty) ? newline.FullOrderQty
							: INUnitAttribute.ConvertFromBase(unassignedSplits.Cache, newline.InventoryID, newline.UOM, (decimal)newline.BaseShippedQty, INPrecision.QUANTITY);

						using (LineSplittingExt.SuppressedModeScope(true))
						{
							Transactions.Cache.RaiseFieldUpdated(newline, oldRow.ShippedQty);
							Transactions.Cache.RaiseRowUpdated(newline, oldRow);
						}
					}
				}
				}

				if (newline.BaseShippedQty < plan.PlanQty && string.IsNullOrEmpty(plan.LotSerialNbr) && !addZeroLineForUnallocated)
				{
					PromptReplenishment(Transactions.Cache, newline, (InventoryItem)res, plan);
				}

				PXNoteAttribute.CopyNoteAndFiles(Caches[typeof(SOLine)], line, Caches[typeof(SOShipLine)], newline, ordertype.CopyLineNotesToShipment, ordertype.CopyLineFilesToShipment);

				if (newline.ShippedQty == 0m)
				{
					deleted = RemoveLineFromShipment(newline, list != null && sosetup.Current.AddAllToShipment == false);
				}

				if (newline.BaseShippedQty < plan.PlanQty * line.CompleteQtyMin / 100m && line.ShipComplete == SOShipComplete.ShipComplete)
				{
					deleted = RemoveLineFromShipment(newline, list != null);
				}

				if (!deleted && !addZeroLineForUnallocated && plan.PlanType != linesplit.PlanType && linesplit.POCreate != true && linesplit.IsAllocated != true)
				{
					INItemPlan actualPlan = PXSelect>>>.Select(this, plan.PlanID);
					if (actualPlan != null)
					{
						actualPlan.PlanType = linesplit.PlanType;
						Caches[typeof(INItemPlan)].Update(actualPlan);
					}
				}
			}
			return deleted;
		}

		protected virtual bool ValidateLineBeforeShipment(SOLine line) => true;

		public PXSelectJoin>,
						InnerJoin, And, And>>>,
						InnerJoin>,
						LeftJoin,
						LeftJoin>>>>>,
						Where>,
						And>,
						And>,
						And>,
						And2>, Or, IsNull>>,
						And>,
							And,
								And,
								And,
								And,
								And>>>>>>>>>>>>>>>>>>,
						OrderBy>>>>> ShipmentScheduleSelect;

		public virtual void CreateShipment(CreateShipmentArgs args)
		{
			SOOrder order = args.Order;
			SiteLotSerial.AccumulatorAttribute.ForceAvailQtyValidation(this, true);
			ItemLotSerial.AccumulatorAttribute.ForceAvailQtyValidation(this, true);
			SOOrderType ordertype = soordertype.Select(order.OrderType);
			SOShipment newdoc;
			string operation = args.Operation ?? ordertype.DefaultOperation;
			args.Operation = operation;
			SOOrderTypeOperation orderoperation = SOOrderTypeOperation.PK.Find(this, order.OrderType, operation);

			if (args.QuickProcessFlow != PXQuickProcess.ActionFlow.NoFlow)
				sosetup.Current.HoldShipments = false;

			if (orderoperation != null && orderoperation.Active == true && String.IsNullOrEmpty(orderoperation.ShipmentPlanType))
            {
                object state = this.Caches().GetStateExt(orderoperation);
                throw new PXException(Messages.ShipmentPlanTypeNotSetup, order.OrderType, state);
            }

			DateTime? shipDate = GetShipmentDate(args);
			DateTime? endDate = args.EndDate ?? shipDate;

			bool addOrder = (args.ShipmentList == null);
			if (!addOrder)
			{
				this.Clear();

				order = ActualizeAndValidateOrder(args.Graph, order, operation);

				newdoc = FindOrCreateShipment(args, order, orderoperation, shipDate);

				bool newlyCreated = (newdoc.ShipmentNbr == null);
				if (newlyCreated)
				{
					newdoc = Document.Insert(newdoc);
				}
				else
				{
					Document.Current = Document.Search(newdoc.ShipmentNbr);
					if (Document.Current.Confirmed == true)
						throw new PXException(IN.Messages.Document_Status_Invalid);
				}

				bool updatedFromOrder = SetShipmentFieldsFromOrder(order, newdoc, args.SiteID, shipDate, operation, orderoperation, newlyCreated);

				if (!newlyCreated && args.MassProcess && OrderList.Select().Count() == 1)
				{
					newdoc.ShipmentDesc = PXMessages.LocalizeNoPrefix(Messages.MultiOrderShipment);
					updatedFromOrder = true;
				}

				if (updatedFromOrder)
				{
					newdoc = Document.Update(newdoc);
				}
				if (newlyCreated)
				{
					SetShipAddressAndContact(newdoc, order.ShipAddressID, order.ShipContactID);

					newdoc = Document.Update(newdoc);
					newdoc = Document.Search(newdoc.ShipmentNbr);
				}
			}
			else
			{
				newdoc = PXCache.CreateCopy(Document.Current);

				bool newlyCreated = (newdoc.OrderCntr == 0);
				bool updatedFromOrder = SetShipmentFieldsFromOrder(order, newdoc, args.SiteID, shipDate, operation, orderoperation, newlyCreated);
				if (newlyCreated)
				{
					SetShipAddressAndContact(newdoc, order.ShipAddressID, order.ShipContactID);

				}
				if (updatedFromOrder)
				{
					newdoc = Document.Update(newdoc);
				}
			}

			if (order.OpenShipmentCntr > 0)
			{
				SOOrderShipment openShipment = PXSelectReadonly>,
					And>,
					And>,
					And>,
					And>>>>>>.Select(this, order.OrderType, order.OrderNbr, args.SiteID, newdoc.ShipmentNbr);
				if (openShipment != null)
				{
					throw new PXException(Messages.OrderHasOpenShipment, order.OrderType, order.OrderNbr, openShipment.ShipmentNbr);
				}
			}

			CopyOrderHeaderNoteAndFiles(order, Document.Current, ordertype);

			var neworder = new SOOrderShipment
			{
				OrderType = order.OrderType,
				OrderNbr = order.OrderNbr,
				OrderNoteID = order.NoteID,
				ShipmentNbr = Document.Current.ShipmentNbr,
				ShipmentType = Document.Current.ShipmentType,
				ShippingRefNoteID = Document.Current.NoteID,
				Operation = Document.Current.Operation,
				ProjectID = order.ProjectID
			};

			soorder.Cache.Hold(order);
			PXParentAttribute.SetParent(OrderList.Cache, neworder, typeof(SOOrder), order);

			var orderlist = OrderListSimple.Select().ToList();
			var located = OrderList.Locate(neworder);

			if (located == null || OrderList.Cache.GetStatus(located).IsIn(PXEntryStatus.Deleted, PXEntryStatus.InsertedDeleted))
			{
				neworder = OrderList.Insert(located ?? neworder);
			}
			else
				neworder = located;

			PXRowDeleting SOOrderShipment_RowDeleting = delegate (PXCache sender, PXRowDeletingEventArgs e)
			{
				e.Cancel = true;
			};

			this.RowDeleting.AddHandler(SOOrderShipment_RowDeleting);

			bool anyDeleted = false;
			var lineships = new Dictionary();
			void SOShipLine_RowDeleted(PXCache sender, PXRowDeletedEventArgs e)
			{
				var row = (SOShipLine)e.Row;
				var keys = new SOLine2
			{
					OrderType = row.OrigOrderType,
					OrderNbr = row.OrigOrderNbr,
					LineNbr = row.OrigLineNbr
			};
				var cached = (SOLine2)soline.Cache.Locate(keys);

				if (lineships.TryGetValue(cached, out LineShipment lineship))
					lineship.AnyDeleted = true;

				anyDeleted = true;
			}

			this.RowDeleted.AddHandler(SOShipLine_RowDeleted);

			foreach (SOLine2 sl in PXSelect>, And>,
					And>, And>,
					And>>>>>>
				.Select(this, order.OrderType, order.OrderNbr, args.SiteID, operation))
			{
				PXParentAttribute.SetParent(soline.Cache, sl, typeof(SOOrder), order);
			}

			foreach (SOLineSplit2 sl in PXSelect>, And>,
					And>, And>,
					And>>>>>>
				.Select(this, order.OrderType, order.OrderNbr, args.SiteID, operation))
			{
				//just place into cache
			}

			foreach (SOShipLine sl in PXSelect>,
					And>>>>
				.Select(this))
			{
				PXParentAttribute.SetParent(Transactions.Cache, sl, typeof(SOOrder), order);
			}

			SOShipLine newline = null;
			skipAdjustFreeItemLines = true;// Free items will still be Adjusted at the end of this method
			bool hasUnallocatedSplits = false;
			List schedulesList = new List();
			try
			{
				foreach (PXResult res in
					ShipmentScheduleSelect.Select(args.SiteID, endDate, order.OrderType, order.OrderNbr, args.OrderLineNbr, args.OrderLineNbr, operation))
				{
					SOShipmentPlan plan = res;
					SOLineSplit split = res;

					if (plan.RequireAllocation == true && split.LineType != SOLineType.NonInventory && split.Operation != SOOperation.Receipt
						&& plan.InclQtySOShipping != 1 && plan.InclQtySOShipped != 1)
					{
						hasUnallocatedSplits = true;
						if (sosetup.Current.AddAllToShipment != true)
						continue;
					}

					newline = new SOShipLine();
					newline.OrigSplitLineNbr = ((SOLineSplit)res).SplitLineNbr;

					schedulesList.Add(new ShipmentSchedule(new PXResult(plan, split, res, res, res, res, new SOShipLine()), newline));
				}

				schedulesList.Sort();

				foreach (ShipmentSchedule ss in schedulesList)
				{
					ss.ShipLine.ShipmentType = Document.Current.ShipmentType;
					ss.ShipLine.ShipmentNbr = Document.Current.ShipmentNbr;
					ss.ShipLine.LineNbr = (int?)PXLineNbrAttribute.NewLineNbr(Transactions.Cache, Document.Current);

					PXParentAttribute.SetParent(Transactions.Cache, ss.ShipLine, typeof(SOOrder), order);

					SOLine2 sl = new SOLine2();

					sl.OrderType = ((SOLine)ss.Result).OrderType;
					sl.OrderNbr = ((SOLine)ss.Result).OrderNbr;
					sl.LineNbr = ((SOLine)ss.Result).LineNbr;

					sl = soline.Locate(sl);
					if (sl != null)
					{
						PXParentAttribute.SetParent(Transactions.Cache, ss.ShipLine, typeof(SOLine2), sl);
					}
					else
					{
						SOLine line = ss.Result;
						if (line.Completed == true && ((SOLineSplit)ss.Result).Completed != true)
						{
							throw new PXException(Messages.CompletedSOLineHasIncompleteSplit, line.OrderNbr, line.LineNbr, ((InventoryItem)ss.Result).InventoryCD);
						}
					}

					LineShipment lineship = lineships.Ensure(sl, () => new LineShipment());
					lineship.Add(ss.ShipLine);

					SOLineSplit2 sp = new SOLineSplit2();
					sp.OrderType = ((SOLineSplit)ss.Result).OrderType;
					sp.OrderNbr = ((SOLineSplit)ss.Result).OrderNbr;
					sp.LineNbr = ((SOLineSplit)ss.Result).LineNbr;
					sp.SplitLineNbr = ((SOLineSplit)ss.Result).SplitLineNbr;

					sp = solinesplit.Locate(sp);
					if (sp != null)
					{
						PXParentAttribute.SetParent(Transactions.Cache, ss.ShipLine, typeof(SOLineSplit2), sp);
					}

					PXParentAttribute.SetParent(Transactions.Cache, ss.ShipLine, typeof(SOOrderShipment), neworder);

					if (args.ShipmentList == null || sl.ShipComplete != SOShipComplete.ShipComplete || lineship.AnyDeleted == false)
					{
						CreateShipmentFromSchedules(ss.Result, ss.ShipLine, ordertype, operation, args.ShipmentList);
					}

					if (args.ShipmentList != null && sl.ShipComplete == SOShipComplete.ShipComplete && lineship.AnyDeleted)
					{
						foreach (SOShipLine shipline in lineship)
						{
							Transactions.Delete(shipline);
						}
						lineship.Clear();
					}
				}

				foreach (KeyValuePair pair in lineships)
				{
					if (pair.Key.ShipComplete == SOShipComplete.ShipComplete && pair.Key.ShippedQty < pair.Key.OrderQty * pair.Key.CompleteQtyMin / 100m)
					{
						foreach (SOShipLine shipline in pair.Value)
						{
							RemoveLineFromShipment(shipline, args.ShipmentList != null);
						}
					}
				}
			}
			finally
			{
				skipAdjustFreeItemLines = false;
			}

			if (args.QuickProcessFlow != PXQuickProcess.ActionFlow.NoFlow && sosetup.Current.RequireShipmentTotal == true)
				Document.Current.ControlQty = Document.Current.ShipmentQty;

			AllocateGroupFreeItems(order);
			AdjustFreeItemLines();

			this.RowDeleting.RemoveHandler(SOOrderShipment_RowDeleting);
			this.RowDeleted.RemoveHandler(SOShipLine_RowDeleted);

			foreach (SOOrderShipment item in OrderList.Cache.Inserted)
			{
				if (args.ShipmentList == null && item.ShipmentQty == 0m)
				{
					SOShipLine shipline = PXSelect>,
						And>,
						And>,
						And>>>>>>.SelectSingleBound(this, null, item.ShipmentType, item.ShipmentNbr, item.OrderType, item.OrderNbr);
					if (shipline == null)
					{
						OrderList.Delete(item);
					}
				}

				try
				{
					if (args.ShipmentList != null && item.LineCntr > 0 && item.ShipmentQty == 0m && sosetup.Current.AddAllToShipment == true && sosetup.Current.CreateZeroShipments != true)
					{
						throw new SOShipmentException(SOShipmentException.ErrorCode.CannotShipTraced, item, Messages.CannotShipTraced, item.OrderType, item.OrderNbr);
					}

					if (args.ShipmentList != null && item.LineCntr == 0)
					{
						if (hasUnallocatedSplits)
						{
							throw new SOShipmentException(SOShipmentException.ErrorCode.NotAllocatedLines, item, Messages.NotAllocatedLines);
						}
						else if (anyDeleted)
						{
							throw new SOShipmentException(SOShipmentException.ErrorCode.CannotShipCompleteTraced, item, Messages.CannotShipCompleteTraced, item.OrderType, item.OrderNbr);
						}
						else if (operation == SOOperation.Issue)
						{
							throw new SOShipmentException(SOShipmentException.ErrorCode.NothingToShipTraced, item, Messages.NothingToShipTraced, item.OrderType, item.OrderNbr, item.ShipDate);
						}
						else
						{
							throw new SOShipmentException(SOShipmentException.ErrorCode.NothingToReceiveTraced, item, Messages.NothingToReceiveTraced, item.OrderType, item.OrderNbr, item.ShipDate);
						}
					}

					if (args.ShipmentList != null && item.ShipComplete == SOShipComplete.ShipComplete)
					{
						bool anyMarkPONotFullyReceived = false;
						bool CannotShipComplete = false;

						foreach (SOLine2 line in PXSelect>, And>, And>, And>, And>>>>>>.Select(this, item.OrderType, item.OrderNbr, item.SiteID, item.Operation))
						{
							var original = (SOLine2)this.Caches().GetOriginal(line);
							if (line.LineType == SOLineType.Inventory &&
								line.ShippedQty - original?.ShippedQty == 0m &&
								line.POSource == INReplenishmentSource.PurchaseToOrder)
							{
								PXTrace.WriteError(Messages.MarkForPOItemsNotFullyReceivedTrace, InventoryItem.PK.Find(this, line.InventoryID)?.InventoryCD, line.LineNbr, INSite.PK.Find(this, line.SiteID)?.SiteCD);
								anyMarkPONotFullyReceived = true;
						}

							if (line.LineType == SOLineType.Inventory && line.ShippedQty - original?.ShippedQty == 0m && DateTime.Compare((DateTime)line.ShipDate, (DateTime)item.ShipDate) <= 0 && line.POSource != INReplenishmentSource.DropShipToOrder)
								CannotShipComplete = true;
						}
						if(anyMarkPONotFullyReceived)
							throw new SOShipmentException(Messages.CannotShipCompleteMarkForPOItemsTraced, order.OrderNbr, order.OrderType);
						if(CannotShipComplete)
							throw new SOShipmentException(Messages.CannotShipCompleteTraced, order.OrderType, order.OrderNbr);
					}


				}
				catch (SOShipmentException)
				{
					//decrement OpenShipmentCntr
					UpdateShipmentCntr(OrderList.Cache, item, -1);
					//clear ShipmentDeleted flag
					UpdateShipmentCntr(OrderList.Cache, item, 0);
					throw;
				}
			}

			if (operation == SOOperation.Issue)
				neworder.LinkShipment(Document.Current, this);

			if (args.ShipmentList != null)
			{
				if (OrderList.Cache.Inserted.Count() > 0 || OrderList.SelectWindowed(0, 1) != null)
				{
					using (new SkipShipCompleteValidationScope()) // Ship-Complete rule has already been validated.
					Save.Press();

					//obtain modified object back.
					if (soorder.Locate(order) is SOOrder cached)
					{
						bool? selected = args.Order.Selected;
						PXCache.RestoreCopy(args.Order, cached);
						args.Order.Selected = selected;
					}

					if (args.ShipmentList.Find(Document.Current) is null)
						args.ShipmentList.Add(Document.Current);
				}
			}
			ItemLotSerial.AccumulatorAttribute.ForceAvailQtyValidation(this, false);
			SiteLotSerial.AccumulatorAttribute.ForceAvailQtyValidation(this, false);
		}

		protected virtual SOShipment FindOrCreateShipment(CreateShipmentArgs args, SOOrder order, SOOrderTypeOperation orderOperation, DateTime? shipDate)
		{
			if (order.ShipSeparately == false)
			{
				return args.ShipmentList.Find(GetShipmentFieldLookups(args, order, orderOperation, shipDate))
					?? new SOShipment();

			}
			else
			{
				return new SOShipment() { Hidden = true };
			}
		}

		protected virtual FieldLookup[] GetShipmentFieldLookups(CreateShipmentArgs args, SOOrder order, SOOrderTypeOperation orderOperation, DateTime? shipDate)
		{
			return new FieldLookup[]
			{
				new FieldLookup(order.CustomerID),
				new FieldLookup(shipDate),
				new FieldLookup(order.ShipAddressID),
				new FieldLookup(order.ShipContactID),
				new FieldLookup(args.SiteID),
				new FieldLookup(order.FOBPoint),
				new FieldLookup(order.ShipVia),
				new FieldLookup(order.ShipTermsID),
				new FieldLookup(order.ShipZoneID),
				new FieldLookup(order.ARDocType != ARDocType.NoUpdate),
				new FieldLookup(order.UseCustomerAccount),
				new FieldLookup(INTranType.DocType(orderOperation.INDocType)),
				new FieldLookup(order.FreightAmountSource),
				new FieldLookup(false),
				new FieldLookup(order.IsManualPackage)
			};
		}

		/// 
		protected virtual DateTime? GetShipmentDate(CreateShipmentArgs args)
		{
			if (args.UseOptimalShipDate == true)
			{
				SOOrder order = args.Order;
				SOShipmentPlan plan =
					order.ShipComplete == SOShipComplete.BackOrderAllowed
						? PXSelectJoinGroupBy

		/// Returns the date of the Shipment
		/// 
>>,
							Where>,
						  	And>,
						  	And>,
						  	And>>>>>,
						  Aggregate>>.Select(this, args.SiteID, order.OrderType, order.OrderNbr, args.Operation)
						: PXSelectJoinGroupBy>>,
						  Where>,
						  	And>,
						  	And>,
						  	And>>>>>,
						  Aggregate>>.Select(this, args.SiteID, order.OrderType, order.OrderNbr, args.Operation);

				if (plan.PlanDate > args.ShipDate)
					return plan.PlanDate;
			}
			return args.ShipDate;
		}

		/// 
		public virtual void CopyOrderHeaderNoteAndFiles(SOOrder srcOrder, SOShipment dstShipment, SOOrderType orderType)
		{
			bool copyNote = PXNoteAttribute.GetNote(Document.Cache, dstShipment) == null ? (orderType.CopyHeaderNotesToShipment ?? false) : false;
			PXNoteAttribute.CopyNoteAndFiles(Caches[typeof(SOOrder)], srcOrder, Document.Cache, dstShipment, copyNote, orderType.CopyHeaderFilesToShipment);
		}

		protected virtual SOOrder ActualizeAndValidateOrder(SOOrderEntry orderEntry, SOOrder order, string operation)
		{
			order = soorder.Select(order.OrderType, order.OrderNbr);
			if (orderEntry == null) return order;

			bool? isWorkflowActionEnabled = (operation == SOOperation.Receipt)
				? WorkflowAction.HasWorkflowActionEnabled(orderEntry, g => g.createShipmentReceipt, order)
				: WorkflowAction.HasWorkflowActionEnabled(orderEntry, g => g.createShipmentIssue, order);
			if (isWorkflowActionEnabled == false)
			{
				var action = (operation == SOOperation.Receipt) ? orderEntry.createShipmentReceipt : orderEntry.createShipmentIssue;
				throw new PXInvalidOperationException(Messages.ActionNotAvailableInCurrentState,
					action.GetCaption(), soorder.Cache.GetRowDescription(order));
			}

			return order;
		}

		public virtual bool SetShipmentFieldsFromOrder(SOOrder order, SOShipment shipment,
			int? siteID, DateTime? shipDate, string operation, SOOrderTypeOperation orderOperation,
			bool newlyCreated)
		{
			if (newlyCreated)
			{
				// unconditionally copy fields from the first added order only
				shipment.SiteID = siteID;
				shipment.ShipmentType = INTranType.DocType(orderOperation.INDocType);
				shipment.Operation = operation;
				shipment.ShipDate = shipDate;

				shipment.CustomerID = order.CustomerID;
				shipment.CustomerLocationID = order.CustomerLocationID;
				shipment.UseCustomerAccount = order.UseCustomerAccount;
				shipment.CustomerOrderNbr = order.CustomerOrderNbr;
				shipment.Resedential = order.Resedential;
				shipment.SaturdayDelivery = order.SaturdayDelivery;
				shipment.Insurance = order.Insurance;
				shipment.GroundCollect = order.GroundCollect;
				shipment.TaxCategoryID = order.FreightTaxCategoryID;
				shipment.DestinationSiteID = order.DestinationSiteID;
				shipment.FreightAmountSource = order.FreightAmountSource;
				shipment.IsManualPackage = order.IsManualPackage;

				if (shipment.FOBPoint == null || !this.IsContractBasedAPI)
					shipment.FOBPoint = order.FOBPoint;

				if (shipment.ShipTermsID == null || !this.IsContractBasedAPI)
					shipment.ShipTermsID = order.ShipTermsID;

				if (shipment.ShipVia == null || !this.IsContractBasedAPI)
					shipment.ShipVia = order.ShipVia;

				if (shipment.ShipZoneID == null || !this.IsContractBasedAPI)
					shipment.ShipZoneID = order.ShipZoneID;

				if (string.IsNullOrEmpty(shipment.ShipmentDesc))
					shipment.ShipmentDesc = order.OrderDesc;

				return true;
			}
			else
			{
				if (shipment.FreightAmountSource != order.FreightAmountSource)
				{
					// double check that we don't mix orders with different Freight Amount Source
					throw new PXException();
				}

				if (OrderList.Select

		/// Copies notes and files from Sales Order to Shipment
		/// 
().Count() == 1)
				{
					SOOrder firstOrder = (PXResult)OrderList.Select();

					if (firstOrder.OrderNbr != order.OrderNbr || firstOrder.OrderType != order.OrderType)
					{
						if (!string.IsNullOrEmpty(shipment.CustomerOrderNbr))
						{
							// If we have several Orders within shipment we can't fill CustomerOrderNbr.
							shipment.CustomerOrderNbr = null;
							return true;
						}
					}
				}

				return false;
			}
		}

		public virtual void SetShipAddressAndContact(SOShipment shipment, int? shipAddressID, int? shipContactID)
		{
			foreach (SOShipmentAddress address in this.Shipping_Address.Select())
			{
				if (address.AddressID < 0)
				{
					Shipping_Address.Delete(address);
				}
			}

			foreach (SOShipmentContact contact in this.Shipping_Contact.Select())
			{
				if (contact.ContactID < 0)
				{
					Shipping_Contact.Delete(contact);
				}
			}

			SOAddress soAddress = SOAddress.PK.Find(this, shipAddressID);
			if (soAddress.IsDefaultAddress == true)
			{
				shipment.ShipAddressID = shipAddressID;
			}
			else
			{
				SOShipmentAddress address = new SOShipmentAddress { };
				AddressAttribute.Copy(address, soAddress);

				address = Shipping_Address.Insert(address);
				shipment.ShipAddressID = address.AddressID;
			}

			SOContact soContact = SOContact.PK.Find(this, shipContactID);
			if (soContact.IsDefaultContact == true)
			{
				shipment.ShipContactID = shipContactID; ;
			}
			else
			{
				SOShipmentContact contact = new SOShipmentContact { };
				ContactAttribute.CopyContact(contact, soContact);

				contact = Shipping_Contact.Insert(contact);
				shipment.ShipContactID = contact.ContactID;
			}
		}

        public virtual void CorrectShipment(SOOrderEntry docgraph, SOShipment shiporder)
        {
            this.Clear();

            Document.Current = Document.Search(shiporder.ShipmentNbr);
			if (WorkflowAction.HasWorkflowActionEnabled(this, g => g.correctShipmentAction, Document.Current) == false)
			{
				throw new PXInvalidOperationException(Messages.ActionNotAvailableInCurrentState,
					correctShipmentAction.GetCaption(), Document.Cache.GetRowDescription(Document.Current));
			}
			MarkOpen(Document.Current);

			Document.Cache.MarkUpdated(Document.Current, assertError: true);
            Document.Cache.IsDirty = true;
            ItemAvailabilityExt.AdvancedCheck = false;

            using (PXTransactionScope ts = new PXTransactionScope())
			using (docgraph.LineSplittingExt.SuppressedModeScope(true))
            {
				var shipLinesClearedSOAllocation = new HashSet();

                foreach (PXResult ordres in OrderList.Select())
                {
                    SOOrderShipment order = ordres;
					SOOrder soorder = ordres;
                    var ordertype = SOOrderType.PK.Find(this, order.OrderType);

                    if (!string.IsNullOrEmpty(order.InvoiceNbr) && ordertype.ARDocType != ARDocType.NoUpdate || !string.IsNullOrEmpty(order.InvtRefNbr))
                    {
                        throw new PXException(Messages.ShipmentInvoicedCannotReopen, order.OrderType, order.OrderNbr);
                    }
                    if (soorder.Cancelled == true)
                    {
                        throw new PXException(Messages.ShipmentCancelledCannotReopen, order.OrderType, order.OrderNbr);
                    }


                    docgraph.Clear();

                    docgraph.Document.Current = docgraph.Document.Search(order.OrderNbr, order.OrderType);
                    docgraph.Document.Current.OpenShipmentCntr++;
                    docgraph.Document.Current.Completed = false;
                    docgraph.Document.Cache.MarkUpdated(docgraph.Document.Current, assertError: true);

					var orderSite = docgraph.OrderSite.SelectSingle(order.OrderType, order.OrderNbr, order.SiteID);
					orderSite.OpenShipmentCntr++;
					orderSite = docgraph.OrderSite.Update(orderSite);

                    docgraph.soordertype.Current.RequireControlTotal = false;
                    docgraph.RecalculateExternalTaxesSync = true;

					order.CreateINDoc = false;
                    order.Confirmed = false;
                    this.OrderList.Cache.Update(order);

                    if (docgraph.Document.Current.OpenShipmentCntr > 1)
                    {
                        foreach (SOOrderShipment shipment2 in PXSelect>, And>, And>, And>, And>>>>>>.SelectSingleBound(this, new object[] { order }))
                        {
                            throw new PXException(Messages.ShipmentExistsForSiteCannotReopen, order.OrderType, order.OrderNbr);
                        }
                    }

                    Dictionary> demand = new Dictionary>();

                    foreach (PXResult res in PXSelectReadonly2>>,
                        Where>,
						And>,
						And>>>>>.Select(docgraph, order.ShipmentNbr, order.OrderType, order.OrderNbr))
                    {
                        SOShipLineSplit line = res;
                        INItemPlan plan = res;

                        List ex;
                        if (!demand.TryGetValue(line.LineNbr, out ex))
                        {
                            demand[line.LineNbr] = ex = new List();
                        }
                        ex.Add(plan);
                    }

                    HashSet toSkipReopen = new HashSet();
                    var lineOpenQuantities = new Dictionary();
                    SOLine prev_line = null;

                    //no Misc lines will be selected because of SiteID constraint
                    foreach (PXResult res in
                        PXSelectJoin,
                                And,
                                And,
								And>,
								And>>>>>>>,
                            Where>,
                                And>,
								And>,
								And,
								And >, Or>>>>>>>
							.SelectMultiBound(docgraph, new [] { order }))
                    {
                        SOLine line = (SOLine)res;
                        SOShipLine shipline = (SOShipLine)res;

						if (shipline.InventoryID != null)
						{
							var item = InventoryItem.PK.Find(this, shipline.InventoryID);
							if (item?.IsConverted == true && shipline.IsStockItem != null && shipline.IsStockItem != item.StkItem)
								throw new PXException(Messages.CannotCorrectShipmentItemConverted);
						}

						if(shipline.SiteID != null && line.SiteID != shipline.SiteID)
						{
							if (shipline.ShippedQty == 0)
							{
								var shipLineCache = Transactions.Cache;
								shipline = (SOShipLine)shipLineCache.Locate(shipline) ?? shipline;
								shipline.Confirmed = false;
								shipline.InvoiceGroupNbr = null;
								shipLineCache.MarkUpdated(shipline, assertError: true);
								shipLineCache.IsDirty = true;
							}
							continue;
						}

						bool cancelRemainder =
							(line.ShipComplete == SOShipComplete.CancelRemainder &&
							soorder.ShipComplete == SOShipComplete.CancelRemainder &&
							soorder.SiteCntr == 1);

                        if (shipline.ShipmentNbr == null &&
							(line.Completed == false || line.LineSign * line.ShippedQty > 0m || (line.ShipDate > order.ShipDate && !cancelRemainder)))
                        {
                            toSkipReopen.Add(line.LineNbr);
                            continue;
                        }

						bool lineSwitched = (prev_line == null || prev_line.LineNbr != line.LineNbr);
						prev_line = docgraph.CorrectSingleLine(line, shipline, lineSwitched, lineOpenQuantities);
                    }

                    decimal? UnallocatedQty = 0m;
                    SOLineSplit prev_split = null;

                    PXResultset allocations = PXSelectJoin,
                            And,
                            And,
                            And,
							And>,
							And>>>>>>>>,
                        Where>,
                            And>,
                            And>,
                            And>,
							And2,
								Or>>,
                            And2,
                            Or, //marked for PO splits that are not received yet will be reopened
									And, And,
									And, And>>>>>>>>>,
							And>>>>>>>>>>,
                        OrderBy<
                            Asc>>>>>>>>>.SelectMultiBound(docgraph, new object[] { order });

					allocations.ForEach(_ => _.CreateCopy());

					foreach (PXResult res in allocations)
                    {
                        SOLineSplit split = PXCache.CreateCopy(res);
                        SOShipLine shipline = res;

                        foreach (SOLineSplit sibling in PXParentAttribute.SelectSiblings(docgraph.splits.Cache, split, typeof(SOLine)))
                        {
                            if (sibling.ShipmentNbr != null && split.ShipmentNbr != null && sibling.ParentSplitLineNbr == split.SplitLineNbr)
                            {
                                throw new PXException(Messages.OrderHasSubsequentShipments, split.ShipmentNbr, docgraph.splits.Cache.GetValueExt(split), order.OrderType, order.OrderNbr);
                            }
                        }

                        if (toSkipReopen.Contains(split.LineNbr))
                            continue;

                        if (prev_split == null || prev_split.LineNbr != split.LineNbr)
                        {
                            UnallocatedQty = 0m;

                            (SOLine Line, decimal? BaseOpenQty, decimal? OpenQty) lineQty;
                            if (lineOpenQuantities.TryGetValue(split.LineNbr, out lineQty))
                            {
								if (lineQty.Line.UOM == split.UOM)
									UnallocatedQty = lineQty.OpenQty;
								else
									UnallocatedQty = INUnitAttribute.ConvertFromBase(docgraph.splits.Cache, split.InventoryID, split.UOM, (decimal)lineQty.BaseOpenQty, INPrecision.QUANTITY);
                            }
                        }

                        prev_split = split;
                        if (object.Equals(split.ShipmentNbr, order.ShipmentNbr))
                        {
                            decimal? QtyToAllocate = 0m;

                            if (split.IsAllocated == true)
                            {
								decimal? siteStatusQtyHardAvail = GetQtyHardAvailFromSiteStatus(docgraph, split);

								decimal? PrevAllocatedQty = allocations.AsEnumerable()
									.RowCast()
									.TakeWhile(_ => !docgraph.splits.Cache.ObjectsEqual(_, split))
									.Where(_ => _.InventoryID == split.InventoryID && _.SubItemID == split.SubItemID && _.SiteID == split.SiteID && _.ShipmentNbr == order.ShipmentNbr && _.IsAllocated == true)
                                    .Sum(_ => _.BaseQty);

								decimal? NextAllocatedQty = allocations.AsEnumerable()
									.RowCast()
									.Where(_ => _.ParentSplitLineNbr == split.SplitLineNbr && _.IsAllocated == true)
									.Sum(_ => _.BaseQty);

								decimal? QtyHardAvail = siteStatusQtyHardAvail + PrevAllocatedQty + NextAllocatedQty > 0 ? siteStatusQtyHardAvail + PrevAllocatedQty + NextAllocatedQty : 0;
                                QtyHardAvail = INUnitAttribute.ConvertFromBase(docgraph.splits.Cache, split.InventoryID, split.UOM, (decimal)QtyHardAvail, INPrecision.QUANTITY);

                                QtyToAllocate = Math.Min((decimal)UnallocatedQty, (decimal)QtyHardAvail);
                            }
                            else
                            {
                                QtyToAllocate = UnallocatedQty;
                            }

                            if (QtyToAllocate >= split.Qty - split.ShippedQty)
                            {
                                UnallocatedQty -= split.Qty - split.ShippedQty;
                            }
                            else
                            {
                                UnallocatedQty -= QtyToAllocate;
                                split.Qty = split.ShippedQty + QtyToAllocate;
                            }
                        }
                        else if (split.Qty >= UnallocatedQty)
                        {
                            split.Qty = UnallocatedQty;
                            UnallocatedQty = 0m;
                        }
                        else
                        {
                            UnallocatedQty -= split.Qty;
                        }

						bool shippedSplit = !string.IsNullOrEmpty(split.ShipmentNbr);
                        split.Completed = false;
                        split.ShipmentNbr = null;

						if (split.IsAllocated == true && !string.IsNullOrEmpty(split.LotSerialNbr)
							&& !string.IsNullOrEmpty(shipline.ShipmentNbr)
							&& !string.Equals(split.LotSerialNbr, shipline.LotSerialNbr, StringComparison.InvariantCultureIgnoreCase))
                            {
							// 1. SN1 is allocated in SO#1, then it is changed in Shipment#1 to SN2. Shipment#1 is confirmed.
							// 2. SN1 is allocated or shipped or issued somewhere else.
							// 3. Correct Shipment#1 => trying to allocate both SN1 and SN2 and stuck.
							// To avoid this situation the allocation of SO#1 is cleared.
							INSiteLotSerial status = PXSelectReadonly>,
								And>, And>>>>>
								.Select(this, split.InventoryID, split.SiteID, split.LotSerialNbr);
							if (status == null || status.QtyHardAvail < split.BaseQty)
                                {
                                    split.IsAllocated = false;
                                    split.LotSerialNbr = null;
								shipLinesClearedSOAllocation.Add(shipline.LineNbr);
                            }
                        }

						SOLineSplit deletedSplit = null;
                        if (split.Qty <= 0 && !shippedSplit || docgraph.splits.Cache.GetStatus(split) == PXEntryStatus.Inserted)
                        {
                            docgraph.splits.Delete(split);
							deletedSplit = split;
                            split = null;
                        }
                        else
                        {
                            split = docgraph.splits.Update(split);
                        }

                        //reattach demand from shipment back to SO schedules
                        if (split != null && split.PlanID != null && shipline.LineNbr != null)
                        {
                            List scheduledemand;
                            if (demand.TryGetValue(shipline.LineNbr, out scheduledemand))
                            {
                                foreach (INItemPlan item in scheduledemand)
                                {
                                    item.SupplyPlanID = split.PlanID;
                                    docgraph.Caches[typeof(INItemPlan)].MarkUpdated(item, assertError: true);
                                }
                                demand.Remove(shipline.LineNbr);
                            }
                        }
						//reattach demand from deleted back orders back to SO schedules
						if (deletedSplit?.PlanID > 0 && deletedSplit.ParentSplitLineNbr != null)
						{
							SOLineSplit parentSplit = docgraph.splits.Locate(new SOLineSplit
							{
								OrderType = deletedSplit.OrderType,
								OrderNbr = deletedSplit.OrderNbr,
								LineNbr = deletedSplit.LineNbr,
								SplitLineNbr = deletedSplit.ParentSplitLineNbr,
							});
							if (parentSplit != null)
							{
								foreach (INItemPlan plan in SelectFrom
									.Where>
									.View.Select(docgraph, deletedSplit.PlanID))
								{
									plan.SupplyPlanID = parentSplit.PlanID;
									docgraph.Caches[typeof(INItemPlan)].MarkUpdated(plan);
								}
							}
						}
                    }

                    SOOrder copy = PXCache.CreateCopy(docgraph.Document.Current);
                    PXFormulaAttribute.CalcAggregate(docgraph.Transactions.Cache, copy);
                    docgraph.Document.Update(copy);

					SOOrder.Events
						.Select(e => e.GotShipmentCorrected)
						.FireOn(docgraph, docgraph.Document.Current);
					docgraph.Save.Press();
                }

                foreach (PXResult res in PXSelectJoin>,
					LeftJoin>>,
					Where>,
						And>>.Select(this))
                {
                    INItemPlan plan = res;
					SOShipLineSplit split = res;
					SOLineSplit soLineSplit = res;
					SOOrderType ordertype = SOOrderType.PK.Find(this, split.OrigOrderType);
					if (ordertype != null)
					{
						split.Confirmed = false;
						if (shipLinesClearedSOAllocation.Contains(split.LineNbr))
						{
							split.OrigPlanType = INPlanConstants.Plan60;
						}
						Caches[typeof(SOShipLineSplit)].MarkUpdated(split, assertError: true);
						Caches[typeof(SOShipLineSplit)].IsDirty = true;

						plan = PXCache.CreateCopy(plan);

						plan.PlanType = split.PlanType;
						plan.OrigPlanType = split.OrigPlanType;
						plan.OrigPlanID = soLineSplit?.PlanID;

						this.Caches[typeof(INItemPlan)].Update(plan);
					}
                }

                //this is done to reset BackOrder plans back to Order Plans because SOLinePlanIDAttribute does not initialize plans normally
                foreach (PXResult line2 in PXSelectJoin, And, And>>>,
                    InnerJoin,
                    And,
                    And,
                    And>>>>,
                    InnerJoin>>>>,
                    Where>,
                    And>>>>.Select(this))
                {
                    SOLineSplit2 solinesplit2 = (SOLineSplit2)line2;
                    SOLine soline = (SOLine)line2;
                    SOShipLine soshipline = (SOShipLine)line2;
                    INItemPlan plan = (INItemPlan)line2;

                    SOLineSplit2 copy = PXCache.CreateCopy(solinesplit2);
                    this.Caches[typeof(SOLineSplit2)].RaiseRowUpdated(solinesplit2, copy);

                    SOShipLine shiplinecopy = PXCache.CreateCopy(soshipline);
                    shiplinecopy.Confirmed = false;
					shiplinecopy.InvoiceGroupNbr = null;
					if (shipLinesClearedSOAllocation.Contains(shiplinecopy.LineNbr))
					{
						shiplinecopy.OrigPlanType = INPlanConstants.Plan60;
					}

					UpdateOrigValues(shiplinecopy, soline, plan.PlanQty);

                    this.Caches[typeof(SOShipLine)].Update(shiplinecopy);
                }

				this.Caches().Clear();
				this.Caches().ClearQueryCache();
				this.Caches().Clear();
				this.Caches().ClearQueryCache();
				this.Caches().Clear();
				this.Caches().ClearQueryCache();

				SOShipment.Events
					.Select(e => e.ShipmentCorrected)
					.FireOn(this, Document.Current);
				Save.Press();

				ts.Complete();
				Document.Cache.RestoreCopy(shiporder, Document.Current);
			}
		}

		protected virtual decimal? GetQtyHardAvailFromSiteStatus(PXGraph docgraph, SOLineSplit split)
        {
			var accum = new SiteStatusByCostCenter()
			{
				InventoryID = split.InventoryID,
				SiteID = split.SiteID,
				SubItemID = split.SubItemID,
				CostCenterID = split.CostCenterID,
			};
			accum = (SiteStatusByCostCenter)docgraph.Caches[typeof(SiteStatusByCostCenter)].Insert(accum);

			var stat = INSiteStatusByCostCenter.PK.Find(docgraph, split.InventoryID, split.SubItemID, split.SiteID, split.CostCenterID);

			return accum.QtyHardAvail + (stat?.QtyHardAvail ?? 0m);
		}

		public virtual void PrepareShipmentForConfirmation(SOShipment shiporder)
		{
			this.Clear();

			Document.Current = Document.Search(shiporder.ShipmentNbr);
			if (WorkflowAction.HasWorkflowActionEnabled(this, g => g.confirmShipmentAction, Document.Current) == false)
			{
				throw new PXInvalidOperationException(Messages.ActionNotAvailableInCurrentState,
					confirmShipmentAction.GetCaption(), Document.Cache.GetRowDescription(Document.Current));
			}
			ItemAvailabilityExt.AdvancedCheck = true;

	        ValidateShipment(shiporder);
        }

        public virtual void ValidateShipment(SOShipment shiporder)
        {
	        if (sosetup.Current.RequireShipmentTotal == true)
			{
				if (Document.Current.ShipmentQty != Document.Current.ControlQty)
				{
					throw new PXException(Messages.MissingShipmentControlTotal);
				}
			}

			if (Document.Current.ShipmentQty == 0)
				throw new PXException(Messages.UnableConfirmZeroShipment, Document.Current.ShipmentNbr);

	        if ((SOOrderShipment) OrderList.SelectWindowed(0, 1) == null)
				throw new PXException(Messages.UnableConfirmShipment, Document.Current.ShipmentNbr);

			Carrier carrier = Carrier.PK.Find(this, Document.Current.ShipVia);
			if (carrier != null && carrier.IsExternal == true && carrier.PackageRequired == true)
			{
		        //check for at least one package
				SOPackageDetail p = Packages.SelectSingle();
				if (p == null)
					throw new PXException(Messages.PackageIsRequired);
	        }

			foreach (SOShipLine line in Transactions.Select())
				ConvertedInventoryItemAttribute.ValidateRow(Transactions.Cache, line);
        }

		private bool IsShipmentReadyForConfirmation = false;
		public virtual void ConfirmShipment(SOOrderEntry docgraph, SOShipment shiporder)
        {
	        if (!IsShipmentReadyForConfirmation)
		        PrepareShipmentForConfirmation(shiporder);

			if (Document.Current == null)
				return;

			MarkConfirmed(Document.Current);
			Document.Cache.MarkUpdated(Document.Current, assertError: true);
			Document.Cache.IsDirty = true;

			foreach (PXResult res in PXSelectJoin>,
				LeftJoin, And>>>>,
			Where>>>.Select(this))
			{
				SOOrder order = (SOOrder)res;
				SOShipLineSplit split = (SOShipLineSplit)res;
				INItemPlan plan = PXCache.CreateCopy((INItemPlan)res);
				INPlanType plantype = INPlanType.PK.Find(this, plan.PlanType);



				if ((bool)plantype.DeleteOnEvent)
				{
					Caches[typeof(INItemPlan)].Delete(plan);
				}
				else if (string.IsNullOrEmpty(plantype.ReplanOnEvent) == false)
				{
					plan.PlanType = plantype.ReplanOnEvent;
					plan.OrigPlanType = null;
					plan.OrigNoteID = order.NoteID;
					Caches[typeof(INItemPlan)].Update(plan);
				}
				split = (SOShipLineSplit)splits.Cache.Locate(split) ?? split;
				splits.Cache.MarkUpdated(split, assertError: true);
				if (split != null)
				{
					split.Confirmed = true;
					if ((bool)plantype.DeleteOnEvent)
					{
						split.PlanID = null;
					}
				}
				splits.Cache.IsDirty = true;
			}

			if ((PXAccess.FeatureInstalled() || PXAccess.FeatureInstalled()) && Document.Current.CurrentWorksheetNbr != null)
			{
				//clear dirty object loaded in scope of separate PXConnectionScope via PXFormulaAttribute
				this.Caches().Clear();
				this.Caches().ClearQueryCacheObsolete();

				SOPickingWorksheet worksheet =
					SelectFrom.
					Where>.
					View.Select(this, Document.Current.CurrentWorksheetNbr);

				if (worksheet.WorksheetType.IsIn(SOPickingWorksheet.worksheetType.Wave, SOPickingWorksheet.worksheetType.Single))
					CartSupportExt?.RemoveItemsFromCart();

				TryCompleteWorksheet(worksheet);
			}

			using (PXTransactionScope ts = new PXTransactionScope())
			{
				SetSuppressWorkflowOnConfirmShipment();

				foreach (SOOrderShipment order in OrderList.Select())
				{
					if (order.ShipmentQty <= 0m)
						throw new PXException(Messages.UnableConfirmZeroOrderShipment, Document.Current.ShipmentNbr, order.OrderType, order.OrderNbr);

					order.Confirmed = true;
					OrderList.Cache.MarkUpdated(order, assertError: true);
					OrderList.Cache.IsDirty = true;

					docgraph.Clear();

					docgraph.Document.Current = docgraph.Document.Search(order.OrderNbr, order.OrderType);
					docgraph.Document.Current.OpenShipmentCntr--;
					docgraph.Document.Current.LastSiteID = order.SiteID;
					docgraph.Document.Current.LastShipDate = order.ShipDate;
					docgraph.Document.Cache.MarkUpdated(docgraph.Document.Current, assertError: true);

					var orderSite = docgraph.OrderSite.SelectSingle(order.OrderType, order.OrderNbr, order.SiteID);
					orderSite.OpenShipmentCntr--;
					orderSite = docgraph.OrderSite.Update(orderSite);

					docgraph.soordertype.Current.RequireControlTotal = false;

					bool backorderExists = false;
					var schedulesClosing = new HashSet(docgraph.Transactions.Cache.GetComparer());
					Dictionary> demand = new Dictionary>();

					foreach (PXResult res in PXSelectReadonly2>>,
						Where>,
							And>>>>.Select(docgraph, order.OrderType, order.OrderNbr))
					{
						SOLineSplit line = res;
						INItemPlan plan = res;

						List ex;
						if (!demand.TryGetValue(line.PlanID, out ex))
						{
							demand[line.PlanID] = ex = new List();
						}
						ex.Add(plan);
					}

					foreach (PXResult res in PXSelectJoin,
							And,
							And,
							And>,
							And>>>>>>>,
						Where>,
							And>,
							And>,
							And2>, Or>,
							And,
							And, Equal, // The siteCntr condition is from BA requirements (Jira: AC-216918), to fix only the known scenario.
										And, Equal,
										And,
										Or>>>>>,
									And, Or>>>>>>>>>>>,
						OrderBy>>.SelectMultiBound(docgraph, new object[] { order, docgraph.Document.Current }))
					{
						SOLine line = (SOLine)res;
						SOShipLine shipline = (SOShipLine)res;

						if(shipline.SiteID > 0 && line.SiteID != shipline.SiteID)
						{
							if (shipline.ShippedQty == 0)
							{
								var shipLineCache = Transactions.Cache;
								shipline = (SOShipLine)shipLineCache.Locate(shipline) ?? shipline;
								shipline.Confirmed = true;
								shipline.RequireINUpdate = false;
								shipLineCache.MarkUpdated(shipline, assertError: true);
								shipLineCache.IsDirty = true;
							}
							continue;
						}

						InventoryItem ii = InventoryItem.PK.Find(this, line.InventoryID);

						if (shipline.ShipmentNbr != null && Math.Abs((decimal)shipline.BaseQty) < 0.0000005m && this.sosetup.Current.AddAllToShipment == false)
						{
							Caches[typeof(SOShipLine)].SetStatus(shipline, PXEntryStatus.Deleted);
							Caches[typeof(SOShipLine)].ClearQueryCacheObsolete();

							shipline = new SOShipLine();
						}

						string lineShippingRule = GetShippingRule(line, shipline);
						if (shipline.ShipmentNbr != null && lineShippingRule == SOShipComplete.ShipComplete
							&& line.LineSign * line.BaseShippedQty < line.LineSign * line.BaseOrderQty * line.CompleteQtyMin / 100m)
						{
							throw new PXException(Messages.CannotShipComplete_Line, ii.InventoryCD);
						}

						if (shipline.ShipmentNbr == null && order.ShipComplete == SOShipComplete.ShipComplete && line.POSource != INReplenishmentSource.DropShipToOrder)
						{
							throw new PXException(Messages.CannotShipComplete_Order, line.OrderType, line.OrderNbr, ii.InventoryCD);
						}

						if (shipline.ShipmentNbr != null && ii.StkItem == false && ii.KitItem == true &&
							Math.Abs((decimal)shipline.BaseQty) >= 0.0000005m &&
							((SOShipLineSplit)PXSelect>,
													And>>>>.SelectSingleBound(this, new object[] { shipline })) == null)
						{
							throw new PXException(Messages.CannotShipEmptyNonStockKit, ii.InventoryCD, shipline.LineNbr);
						}

						ValidateLineType(line, ii, Messages.CannotConfirmShipmentNonInventoryNonStockKit);

						bool IsLineShippedOrCancelRemainder = (shipline.ShipmentNbr != null || lineShippingRule == SOShipComplete.CancelRemainder);
						if ((IsLineShippedOrCancelRemainder || this.insetup.Current.ReplanBackOrders == true)
							&& schedulesClosing.Add(line))
						{
								foreach (PXResult schedres
									in PXSelectJoin>,
								LeftJoin,
										And,
										And,
										And,
										And>,
										And>>>>>>>,
									LeftJoin,
											And,
											And,
											And>>>>>>>,
								Where>,
									And>,
									And>,
									And>,
									//And,
									//And,
									And>,
										Or>>>>>>>>>.Select(docgraph, order.ShipmentType, order.ShipmentNbr, line.OrderType, line.OrderNbr, line.LineNbr, order.SiteID, order.ShipDate))
							{
								SOLineSplit schedule = schedres;
								INItemPlan schedplan = schedres;
								SOShipLine shline = schedres;

								if (IsLineShippedOrCancelRemainder)
								{
									if (shline.ShipmentNbr != null && Math.Abs((decimal)shline.BaseQty) < 0.0000005m && this.sosetup.Current.AddAllToShipment == false)
									{
										shline = new SOShipLine();
									}

									List scheduleDemand = null;
									if (schedule.PlanID != null && demand.TryGetValue(schedule.PlanID, out scheduleDemand))
									{
										INItemPlan shipPlan = PXSelectJoin>>,
														Where>,
															And>>>>
											.SelectSingleBound(this, new object[] { shline });

										if (shipPlan?.PlanID != null)
										{
											foreach (INItemPlan item in scheduleDemand)
											{
												item.SupplyPlanID = shipPlan.PlanID;
												docgraph.Caches[typeof(INItemPlan)].MarkUpdated(item, assertError: true);
											}
											demand.Remove(schedule.PlanID);
											scheduleDemand = null;
										}
									}

									using (docgraph.LineSplittingExt.SuppressedModeScope(true))
									{
										if (schedule.FixedSource != INReplenishmentSource.None && schedule.FixedSource != INReplenishmentSource.DropShipToOrder && lineShippingRule == SOShipComplete.CancelRemainder)
										{
											schedule = PXCache.CreateCopy(schedule);
											schedule.Completed = true;
											schedule.ShipComplete = line.ShipComplete;

											schedule = docgraph.splits.Update(schedule);
											docgraph.Caches[typeof(INItemPlan)].Delete(schedplan);

											schedule.PlanID = null;
										}

										//should precede back-order insertion
										if (shline.ShipmentNbr != null || lineShippingRule == SOShipComplete.ShipComplete || lineShippingRule == SOShipComplete.CancelRemainder && schedule.FixedSource == INReplenishmentSource.None)
										{
											schedule = PXCache.CreateCopy(schedule);
											schedule.Completed = true;
											schedule.ShipmentNbr = shline.ShipmentNbr != null ? order.ShipmentNbr : null;
											schedule.ShipComplete = line.ShipComplete;
											schedule = docgraph.splits.Update(schedule);
											docgraph.Caches[typeof(INItemPlan)].Delete(schedplan);

											schedule.PlanID = null;

											if (lineShippingRule == SOShipComplete.CancelRemainder && schedule.FixedSource == INReplenishmentSource.None && schedule.ShippedQty == 0m)
											{
												INItemPlan demandPlan =
													PXSelect>,
														And>>>.SelectSingleBound(this, new object[] { schedplan });
												if (demandPlan != null)
												{
													docgraph.Caches[typeof(INItemPlan)].Delete(demandPlan);
												}
											}
										}

									if (shline.ShipmentNbr != null && lineShippingRule.IsNotIn(SOShipComplete.ShipComplete, SOShipComplete.CancelRemainder)
											&& line.LineSign * line.BaseShippedQty < line.LineSign * line.BaseOrderQty * line.CompleteQtyMin / 100m)
										{
											SOLineSplit split = PXCache.CreateCopy(schedule);
											split.PlanID = null;
											split.PlanType = split.BackOrderPlanType;
											split.ParentSplitLineNbr = split.SplitLineNbr;
											split.SplitLineNbr = null;
											split.IsAllocated = schedule.IsAllocated;
											split.Completed = false;
											split.ShipmentNbr = null;
											split.LotSerialNbr = schedule.LotSerialNbr;
											split.LotSerClassID = schedule.LotSerClassID;

											split.ClearPOFlags();
											split.ClearPOReferences();
											split.ClearSOReferences();
											split.VendorID = null;
											split.RefNoteID = null;

											split.BaseReceivedQty = 0m;
											split.ReceivedQty = 0m;
											split.BaseShippedQty = 0m;
											split.ShippedQty = 0m;
											split.BaseQty = (schedule.BaseQty - schedule.BaseShippedQty);
											split.Qty = INUnitAttribute.ConvertFromBase(docgraph.splits.Cache, split.InventoryID, split.UOM, (decimal)split.BaseQty, INPrecision.QUANTITY);

											if (PXAccess.FeatureInstalled() && commonsetup.Current != null && commonsetup.Current.DecPlQty == 0m)
											{
												if (INUnitAttribute.ConvertToBase(docgraph.splits.Cache, split.InventoryID, split.UOM, (decimal)split.Qty, INPrecision.QUANTITY) != split.BaseQty)
												{
													throw new PXException(Messages.LowQuantityPrecision, docgraph.splits.GetValueExt(split).ToString().Trim());
												}
											}

											if (split.BaseQty > 0m)
											{
												docgraph.Transactions.Current = docgraph.Transactions.Search(split.OrderType, split.OrderNbr, split.LineNbr);
												schedule = docgraph.LineSplittingAllocatedExt.InsertShipmentRemainder(split);

												if (scheduleDemand != null)
												{
													// linking the demand to the back orders if nothing was shipped
													foreach (INItemPlan item in scheduleDemand)
													{
														item.SupplyPlanID = schedule.PlanID;
														docgraph.Caches[typeof(INItemPlan)].MarkUpdated(item);
													}
													scheduleDemand = null;
												}
											}
										}
									}
								}

								if (schedule != null &&
									schedule.Completed == false &&
									schedule.PlanID != null &&
									(this.insetup.Current.ReplanBackOrders == true ||
									 schedule.IsAllocated == true))
								{
										var hardAvail = ((INSiteStatusByCostCenter)schedres).QtyHardAvail ?? 0;

									if ((hardAvail > 0 &&
										(lineShippingRule != SOShipComplete.ShipComplete || hardAvail >= schedule.BaseQty)) ||
										schedule.IsAllocated == true)
									{
										INItemPlan plan = PXSelect>>>.SelectSingleBound(docgraph, new[] { schedule });

										if (plan != null)
										{
											SOOrderType ordertype = PXSetup.Select(docgraph);
											// We should skip allocated plans. In general we should process only "normal" plans.
											var initPlanType = docgraph.FindImplementation().IsPlanRegular(ordertype, plan);
											if (initPlanType == true)
											{
												schedule.PlanType = schedule.IsAllocated == true
													? schedule.AllocatedPlanType
													: schedule.BookedPlanType;

												plan = PXCache.CreateCopy(plan);
												plan.IsSkippedWhenBackOrdered = true;
												plan.PlanType = schedule.PlanType;
												docgraph.Caches().Update(plan);
											}
										}
									}
								}
							}
						}

						CreateNewSOLines(docgraph, line, shipline);

						docgraph.ConfirmSingleLine(line, shipline, lineShippingRule, ref backorderExists);

						if (shipline.ShipmentNbr != null)
						{
							object cached = Caches[typeof(SOShipLine)].Locate(shipline);
							if (cached != null)
							{
								shipline = (SOShipLine)cached;
							}

							if (Math.Abs((decimal)shipline.BaseQty) < 0.0000005m)
							{
								LineSplittingExt.RaiseRowDeleted(shipline);
							}

							shipline.Confirmed = true;

							if (shipline.LineType == SOLineType.Inventory)
							{
								if (ii.StkItem == false && ii.KitItem == true &&
									((SOShipLineSplit)PXSelectJoin>>>,
											 Where>,
												And>>>>.SelectSingleBound(this, new object[] { shipline })) == null)
								{
									shipline.RequireINUpdate = false;
								}
								else
								{
									shipline.RequireINUpdate = true;
									order.CreateINDoc = true;
								}
							}
							else
							{
								shipline.RequireINUpdate = false;
							}

							Caches[typeof(SOShipLine)].MarkUpdated(shipline, assertError: true);
							Caches[typeof(SOShipLine)].IsDirty = true;
						}
					}

					SOOrder.Events
						.Select(e => e.GotShipmentConfirmed)
						.FireOn(docgraph, docgraph.Document.Current);
					docgraph.Save.Press();
				}

				GroupShipLinesForInvoicing(Document.Current);

				WorkLogExt?.CloseFor(Document.Current.ShipmentNbr);

				SOShipment.Events
					.Select(e => e.ShipmentConfirmed)
					.FireOn(this, Document.Current);
				Save.Press();

				ts.Complete();
			}

			Document.Cache.RestoreCopy(shiporder, Document.Current);
		}

		protected virtual void GroupShipLinesForInvoicing(SOShipment ship)
		{
			foreach (var shipGroupBySOLine in Transactions.Cache.Updated.RowCast()
				.Where(sl => sl.ShipmentNbr == ship.ShipmentNbr && sl.ShipmentType == ship.ShipmentType)
				.GroupBy(sl => new { sl.OrigOrderType, sl.OrigOrderNbr, sl.OrigLineNbr }))
			{
				SOShipLine firstShipLine = shipGroupBySOLine.First();
				var shipGroupsByUom = shipGroupBySOLine.GroupBy(sl => sl.UOM, StringComparer.OrdinalIgnoreCase);
				// we expect only 2 different UOMs as maximum - Sales Line UOM and Base UOM
				if (shipGroupsByUom.Count() != 2)
				{
					shipGroupBySOLine.ForEach(sl => sl.InvoiceGroupNbr = 1);
					continue;
				}
				var item = InventoryItem.PK.Find(this, firstShipLine.InventoryID);
				var shipGroupWithBaseUom = shipGroupsByUom.FirstOrDefault(g => string.Equals(g.Key, item?.BaseUnit, StringComparison.OrdinalIgnoreCase));
				var shipGroupWithSalesUom = shipGroupsByUom.FirstOrDefault(g => !string.Equals(g.Key, item?.BaseUnit, StringComparison.OrdinalIgnoreCase));
				if (shipGroupWithBaseUom == null || shipGroupWithSalesUom == null)
				{
					shipGroupBySOLine.ForEach(sl => sl.InvoiceGroupNbr = 1);
					continue;
				}

				shipGroupWithSalesUom.ForEach(sl => sl.InvoiceGroupNbr = 1);
				decimal? baseUomQtySum = shipGroupWithBaseUom.Sum(sl => sl.ShippedQty);
				decimal baseUomQtyConvertedToSalesUom = INUnitAttribute.ConvertFromBase(Transactions.Cache,
					firstShipLine.InventoryID,
					shipGroupWithSalesUom.Key,
					baseUomQtySum ?? 0m,
					INPrecision.NOROUND);
				shipGroupWithBaseUom.ForEach(sl => sl.InvoiceGroupNbr = (baseUomQtyConvertedToSalesUom % 1m == 0m) ? 1 : 2);
			}
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

		protected virtual string GetShippingRule(SOLine line, SOShipLine shipline)
		{
			return line.ShipComplete;
		}

		protected virtual void CreateNewSOLines(SOOrderEntry docgraph, SOLine line, SOShipLine shipline)
		{
			//do not create issue lines if nothing is in the current shipment i.e. shipline.Operation == null
			if (line.AutoCreateIssueLine == true && shipline.Operation == SOOperation.Receipt)
			{
				SOLine newLine = PXSelect>,
						And>,
						And>,
						And>,
						And>>>>>>>
					.SelectWindowed(docgraph, 0, 1, line.OrderType, line.OrderNbr, line.OrderType, line.OrderNbr, line.LineNbr);
				if (newLine == null)
				{
					newLine = new SOLine();
					newLine.OrderType = line.OrderType;
					newLine.OrderNbr = line.OrderNbr;
					newLine = PXCache.CreateCopy(docgraph.Transactions.Insert(newLine));
					newLine.IsStockItem = line.IsStockItem;
					newLine.InventoryID = line.InventoryID;
					newLine.SubItemID = line.SubItemID;
					newLine.UOM = line.UOM;
					newLine.SiteID = line.SiteID;
					newLine.OrigOrderType = line.OrderType;
					newLine.OrigOrderNbr = line.OrderNbr;
					newLine.OrigLineNbr = line.LineNbr;
					newLine.ManualDisc = line.ManualDisc;
					newLine.ManualPrice = true;
					newLine.CuryUnitPrice = line.CuryUnitPrice;
					newLine.SalesPersonID = line.SalesPersonID;
					newLine.ProjectID = line.ProjectID;
					newLine.TaskID = line.TaskID;
					newLine.CostCodeID = line.CostCodeID;
					newLine.ReasonCode = line.ReasonCode;
					newLine.IsSpecialOrder = false;
					newLine = docgraph.Transactions.Update(newLine);

					bool processSplit = false;
					var item = InventoryItem.PK.Find(docgraph, line.InventoryID);
					if (line.OrderQty % 1m != 0m && item?.DecimalSalesUnit == false
						&& !string.Equals(item.BaseUnit, line.UOM, StringComparison.OrdinalIgnoreCase))
					{
						var salesUnit = INUnit.UK.ByInventory.Find(docgraph, line.InventoryID, line.UOM);
						if (salesUnit?.UnitMultDiv == MultDiv.Multiply && salesUnit.UnitRate > 1m)
						{
							processSplit = true;
						}
					}

					if (processSplit)
					{
						var newsplit = new SOLineSplit()
						{
							UOM = item.BaseUnit
						};
						newsplit = PXCache.CreateCopy(docgraph.splits.Insert(newsplit));
						newsplit.Qty = line.LineSign * line.BaseOrderQty;
						newsplit = docgraph.splits.Update(newsplit);
					}
					else
					{
						newLine = PXCache.CreateCopy(newLine);
						newLine.OrderQty = -line.OrderQty;
						newLine.BaseOrderQty = -line.BaseOrderQty;
						newLine = docgraph.Transactions.Update(newLine);
					}

					if (line.ManualDisc == true)
					{
						newLine = PXCache.CreateCopy(newLine);
						newLine.DiscPct = line.DiscPct;
						newLine.CuryDiscAmt = -line.CuryDiscAmt;
						newLine.CuryLineAmt = -line.CuryLineAmt;

						newLine = docgraph.Transactions.Update(newLine);
					}
				}
			}
		}

		public virtual void UpdateOrigValues(SOShipLine shipline, SOLine soline, decimal? baseOrigQty)
		{
		}

		public virtual void InvoiceShipment(SOInvoiceEntry docgraph, SOShipment shiporder, DateTime invoiceDate, InvoiceList list, PXQuickProcess.ActionFlow quickProcessFlow)
		{
			this.Clear();

			Document.Current = Document.Search(shiporder.ShipmentNbr);
			if (WorkflowAction.HasWorkflowActionEnabled(this, g => g.createInvoice, Document.Current) == false)
			{
				throw new PXInvalidOperationException(Messages.ActionNotAvailableInCurrentState,
					createInvoice.GetCaption(), Document.Cache.GetRowDescription(Document.Current));
			}

			Document.Current.Status = shiporder.Status;

			Document.Cache.MarkUpdated(Document.Current, assertError: true);

			using (PXTransactionScope ts = new PXTransactionScope())
			{
				this.Save.Press();

				foreach (PXResult order in PXSelectJoin, And>>,
					InnerJoin>,
					InnerJoin>,
					InnerJoin>,
					InnerJoin>,
					InnerJoin,
										And>>>>>>>>,
					Where>,
						And>,
						And>>>.Select(this))
				{
					((SOOrderShipment)order).BillShipmentSeparately = shiporder.BillSeparately;

					docgraph.Clear();
					docgraph.Clear(PXClearOption.ClearQueriesOnly);
					docgraph.ARSetup.Current.RequireControlTotal = false;

					var shipmentInvoices = list as ShipmentInvoices;
					if (shipmentInvoices != null)
					{
						var orderType = SOOrderType.PK.Find(this, ((SOOrder)order).OrderType);
						var docType = docgraph.GetInvoiceDocType(orderType, ((SOOrder)order), ((SOOrderShipment)order).Operation);
						var subList = new InvoiceList(docgraph);
						subList.AddRange(shipmentInvoices.GetInvoices(docType));
						int oldCount = subList.Count;
						docgraph.InvoiceOrder(new InvoiceOrderArgs(order)
						{
							InvoiceDate = invoiceDate,
							Customer = customer.Current,
							List = subList,
							QuickProcessFlow = quickProcessFlow,
							OptimizeExternalTaxCalc = true
						});

						if (subList.Count > oldCount)
							list.Add(subList[oldCount], subList[oldCount], (CM.Extensions.CurrencyInfo)subList[oldCount][typeof(CM.Extensions.CurrencyInfo)]);
					}
					else
						docgraph.InvoiceOrder(new InvoiceOrderArgs(order)
						{
							InvoiceDate = invoiceDate,
							Customer = customer.Current,
							List = list,
							QuickProcessFlow = quickProcessFlow,
							OptimizeExternalTaxCalc = true
						});
				}
				ts.Complete();
			}
		}

		public static void InvoiceReceipt(Dictionary parameters, List list, InvoiceList created, bool isMassProcess = false)
		{
			bool optimizeExternalTaxCalc = isMassProcess;
			SOShipmentEntry docgraph = PXGraph.CreateInstance();
			SOInvoiceEntry invoiceEntry = PXGraph.CreateInstance();

			list.Sort((x,y)=> { return (x.ShipmentNbr).CompareTo(y.ShipmentNbr);});

			foreach (SOShipment poreceipt in list)
			{
				try
				{
					if (isMassProcess)
					{
						PXProcessing.SetCurrentItem(poreceipt);
					}

					invoiceEntry.Clear();
					invoiceEntry.Clear(PXClearOption.ClearQueriesOnly);
					invoiceEntry.ARSetup.Current.RequireControlTotal = false;

					char[] a = typeof(SOShipmentFilter.invoiceDate).Name.ToCharArray();
					a[0] = char.ToUpper(a[0]);
					object invoiceDate;
					if (!parameters.TryGetValue(new string(a), out invoiceDate))
					{
						invoiceDate = invoiceEntry.Accessinfo.BusinessDate;
					}

					foreach (PXResult res in PXSelectJoin, And>>,
					InnerJoin>,
					InnerJoin>,
					InnerJoin>>>>>,
					Where,
						And>,
						And>>>.Select(docgraph, poreceipt.ShipmentNbr))
					{
						SOOrderShipment shipment = res;
						shipment.BillShipmentSeparately = poreceipt.BillSeparately;
						SOOrder order = res;
						PXResult record =
							new PXResult(shipment, order, (CurrencyInfo)res, (SOAddress)res, (SOContact)res);

						PXResultset details = new PXResultset();
						details.AddRange(docgraph.CollectDropshipDetails(shipment));

						var shipmentInvoices = created as ShipmentInvoices;
						if (shipmentInvoices != null)
						{
							var orderType = SOOrderType.PK.Find(docgraph, order.OrderType);
							var docType = invoiceEntry.GetInvoiceDocType(orderType, order, shipment.Operation);

							var subList = new InvoiceList(docgraph);
							subList.AddRange(shipmentInvoices.GetInvoices(docType));
							int oldCount = subList.Count;

							invoiceEntry.InvoiceOrder(new InvoiceOrderArgs(record)
							{
								InvoiceDate = (DateTime)invoiceDate,
								Details = details,
								List = subList,
								OptimizeExternalTaxCalc = optimizeExternalTaxCalc
							});

							if (subList.Count > oldCount)
								created.Add(subList[oldCount], subList[oldCount], (CM.Extensions.CurrencyInfo)subList[oldCount][typeof(CM.Extensions.CurrencyInfo)]);
						}
						else
							invoiceEntry.InvoiceOrder(new InvoiceOrderArgs(record)
							{
								InvoiceDate = (DateTime)invoiceDate,
								Details = details,
								List = created,
								OptimizeExternalTaxCalc = optimizeExternalTaxCalc
							});

						if (invoiceEntry.Caches.ContainsKey(typeof(SOOrder)) && PXTimeStampScope.GetPersisted(invoiceEntry.Caches[typeof(SOOrder)], order) != null)
							PXTimeStampScope.PutPersisted(invoiceEntry.Caches[typeof(SOOrder)], order, invoiceEntry.TimeStamp);
					}
					if (isMassProcess)
					{
						PXProcessing.SetProcessed();
					}
				}
				catch (Exception ex)
				{
					if (!isMassProcess)
					{
						throw;
					}
					PXProcessing.SetError(ex);
				}
			}

			if (optimizeExternalTaxCalc)
			{
				invoiceEntry.CompleteProcessingImpl(created);
			}
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

		public virtual void PostReceipt(INIssueEntry docgraph, PXResult sh, ARInvoice invoice, DocumentList list)
		{
			SOOrderShipment shiporder = sh;
			SOOrder order = sh;

			this.Clear();
			docgraph.Clear();

			docgraph.insetup.Current.HoldEntry = false;
			docgraph.insetup.Current.RequireControlTotal = false;

			INRegister newdoc =
				list.Find(shiporder.ShipmentType, shiporder.ShipmentNbr)
				?? new INRegister();

			if (newdoc.RefNbr != null)
			{
				docgraph.issue.Current = docgraph.issue.Search(newdoc.DocType, newdoc.RefNbr);
				if (docgraph.issue.Current != null && docgraph.issue.Current.SrcRefNbr == null) //Non-db fields cannot be restored after .Clear()
				{
					docgraph.issue.Current.SrcDocType = shiporder.ShipmentType;
					docgraph.issue.Current.SrcRefNbr = shiporder.ShipmentNbr;
				}
			}
			else
			{
				newdoc.BranchID = order.BranchID;
				newdoc.DocType = INDocType.Issue;
				newdoc.SiteID = shiporder.SiteID;
				newdoc.TranDate = invoice.DocDate;
				newdoc.OrigModule = GL.BatchModule.SO;
				newdoc.SrcDocType = shiporder.ShipmentType;
				newdoc.SrcRefNbr = shiporder.ShipmentNbr;
				newdoc.FinPeriodID = invoice.FinPeriodID;

				docgraph.issue.Insert(newdoc);
			}

			INTran newline = null;
			POReceiptLine prev_line = null;
			BqlCommand selectDropshipReceiptsCmd = GetDropshipReceiptsSelectCommand(shiporder);

			var selectDropshipReceiptsView = new PXView(this, false, selectDropshipReceiptsCmd);

			foreach (PXResult res in selectDropshipReceiptsView.SelectMultiBound(new object[] { shiporder }))
			{
				POReceiptLine line = res;
				SOLine soline = PXResult.Unwrap(res);
				ARTran artran = PXResult.Unwrap(res);
				var orderoperation = SOOrderTypeOperation.PK.Find(this, soline.OrderType, soline.Operation);
				INLocation loc = PXResult.Unwrap(res);
				INLotSerClass lsclass = PXResult.Unwrap(res);
				INPostClass postclass = PXResult.Unwrap(res);
				InventoryItem item = PXResult.Unwrap(res);
				INSite site = PXResult.Unwrap(res);

				if (Caches[typeof(POReceiptLine)].ObjectsEqual(prev_line, line))
					continue;

				if (line.LineType == POLineType.GoodsForDropShip && loc.LocationID == null)
				{
					throw new PXException(Messages.NoDropShipLocation, Caches[typeof(POReceiptLine)].GetValueExt(line));
				}

				TryToGetProjectAndTask(res, line, out var project, out var task);

				newline = new INTran();
				newline.BranchID = soline.BranchID;
				newline.TranType = orderoperation.INDocType;
				newline.POReceiptType = line.ReceiptType;
				newline.POReceiptNbr = line.ReceiptNbr;
				newline.POReceiptLineNbr = line.LineNbr;
				newline.POLineType = line.LineType;
				newline.SOShipmentNbr = line.ReceiptNbr;
				newline.SOShipmentType = SOShipmentType.DropShip;
				newline.SOShipmentLineNbr = line.LineNbr;
				newline.SOOrderType = soline.OrderType;
				newline.SOOrderNbr = soline.OrderNbr;
				newline.SOOrderLineNbr = soline.LineNbr;
				newline.ARDocType = artran.TranType;
				newline.ARRefNbr = artran.RefNbr;
				newline.ARLineNbr = artran.LineNbr;

				newline.InventoryID = line.InventoryID;
				newline.SubItemID = line.SubItemID;
				newline.SiteID = line.SiteID;
				newline.LocationID = loc.LocationID;
				newline.BAccountID = soline.CustomerID;
				newline.InvtMult = (short)0;
				newline.IsCostUnmanaged = true;

				newline.UOM = line.UOM;
				newline.Qty = line.ReceiptQty;
				newline.UnitPrice = artran.UnitPrice ?? 0m;
				bool signMismatch = artran.DrCr == DrCr.Credit && artran.SOOrderLineOperation == SOOperation.Receipt
					|| artran.DrCr == DrCr.Debit && artran.SOOrderLineOperation == SOOperation.Issue;
				newline.TranAmt = (signMismatch ? -artran.TranAmt : artran.TranAmt) ?? 0m;
				newline.UnitCost = line.UnitCost;
				newline.TranCost = line.TranCostFinal;
				newline.TranDesc = soline.TranDesc;
				newline.ReasonCode = soline.ReasonCode;
				newline.AcctID = line.POAccrualAcctID;
				newline.SubID = line.POAccrualSubID;
				newline.ReclassificationProhibited = true;
				if (line.ExpenseAcctID == null && postclass != null && postclass.COGSSubFromSales == true)
				{
					newline.COGSAcctID = INReleaseProcess.GetAccountDefaults(this,
						PX.Objects.IN.Services.InventoryAccountServiceHelper.Params(item, site, postclass, project, task));
					newline.COGSSubID = artran.SubID;
				}
				else
				{
					newline.COGSAcctID = line.ExpenseAcctID;
					newline.COGSSubID = (postclass != null && postclass.COGSSubFromSales == true ? artran.SubID : null) ?? line.ExpenseSubID;
				}
				newline.ProjectID = line.ProjectID;
				newline.TaskID = line.TaskID;
				newline.CostCodeID = line.CostCodeID;
				docgraph.CostCenterDispatcherExt?.SetCostLayerType(newline);
				newline = docgraph.transactions.Insert(newline);

				PXSelectBase selectSplits = new PXSelect>,
					And>,
					And>,
					And>>>>>(this);

				foreach (POReceiptLineSplit split in selectSplits.Select(line.ReceiptType, line.ReceiptNbr, line.LineNbr))
				{
					INTranSplit newsplit = (INTranSplit)newline;
					newsplit.SplitLineNbr = null;
					newsplit.LotSerialNbr = split.LotSerialNbr;
					newsplit.ExpireDate = split.ExpireDate;
					newsplit.BaseQty = split.BaseQty;
					newsplit.Qty = split.Qty;
					newsplit.UOM = split.UOM;
					newsplit.InvtMult = 0;

					docgraph.splits.Insert(newsplit);
				}

				prev_line = line;
			}

			INRegister copy = PXCache.CreateCopy(docgraph.issue.Current);
			PXFormulaAttribute.CalcAggregate(docgraph.transactions.Cache, copy);
			PXFormulaAttribute.CalcAggregate(docgraph.transactions.Cache, copy);
			PXFormulaAttribute.CalcAggregate(docgraph.transactions.Cache, copy);
			docgraph.issue.Update(copy);

			using (PXTransactionScope ts = new PXTransactionScope())
			{
				if (docgraph.transactions.Cache.IsDirty)
				{
					docgraph.Save.Press();

					{
						shiporder.InvtDocType = docgraph.issue.Current.DocType;
						shiporder.InvtRefNbr = docgraph.issue.Current.RefNbr;
						shiporder.InvtNoteID = docgraph.issue.Current.NoteID;

						OrderList.Cache.Update(shiporder);
					}

					PXDBDefaultAttribute.SetDefaultForUpdate(OrderList.Cache, null, false);
					PXDBDefaultAttribute.SetDefaultForUpdate(OrderList.Cache, null, false);
					PXDBLiteDefaultAttribute.SetDefaultForUpdate(OrderList.Cache, null, false);

					this.Save.Press();

					if (list.Find(docgraph.issue.Current) == null)
					{
						list.Add(docgraph.issue.Current);
					}
				}
				ts.Complete();
			}
		}

		protected virtual BqlCommand GetDropshipReceiptsSelectCommand(SOOrderShipment shiporder)
		{
			BqlCommand selectDropshipReceiptsCmd = BqlCommand.CreateInstance(
							typeof(Select2,
								LeftJoin,
								LeftJoin,
								InnerJoin,
								LeftJoin>>>>>>,
							Where>,
								And>,
								And>,
								And,
								And,
								And>>>>>>));
			// TODO: DropshipReturn
			// unify join to SOLine or move to a separate method
			if (shiporder.Operation == SOOperation.Receipt)
			{
				selectDropshipReceiptsCmd = selectDropshipReceiptsCmd.WhereAnd>>();

				selectDropshipReceiptsCmd = BqlCommand.AppendJoin<
					InnerJoin>>(selectDropshipReceiptsCmd);
			}
			else
			{
				selectDropshipReceiptsCmd = selectDropshipReceiptsCmd.WhereAnd>>();

				selectDropshipReceiptsCmd = BqlCommand.AppendJoin<
					InnerJoin, And, And>>>,
					InnerJoin>>>(selectDropshipReceiptsCmd);
			}
			selectDropshipReceiptsCmd = BqlCommand.AppendJoin<
				InnerJoin,
					And,
					And,
					And,
					And,
					And>>>>>>,
				LeftJoin,
					And,
					And,
					And,
					And,
					And>>>>>>>>>(selectDropshipReceiptsCmd);
			return selectDropshipReceiptsCmd;
		}

		protected virtual void TryToGetProjectAndTask(PXResult res, POReceiptLine line, out PMProject project, out PMTask task)
		{
			project = null;
			task = null;
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

		public virtual void PostShipment(INRegisterEntryFactory factory, SOShipment shiporder, DocumentList list)
		{
			this.Clear();
			INRegisterEntryBase docgraph = factory.GetOrCreateINRegisterEntry(shiporder);

			Document.Current = Document.Search(shiporder.ShipmentNbr);
			if (WorkflowAction.HasWorkflowActionEnabled(this, g => g.UpdateIN, Document.Current) == false)
			{
				throw new PXInvalidOperationException(Messages.ActionNotAvailableInCurrentState,
					UpdateIN.GetCaption(), Document.Cache.GetRowDescription(Document.Current));
			}

			Document.Current.Status = shiporder.Status;
			Document.Cache.MarkUpdated(Document.Current, assertError: true);
			Document.Cache.IsDirty = true;

			using (PXTransactionScope ts = new PXTransactionScope())
			{
				SetSuppressWorkflowOnUpdateIN();

				foreach (PXResult res in PXSelectJoin, And>>>,
					Where>, And>,
					And>>>.SelectMultiBound(this, new object[] { shiporder }))
				{
					this.PostShipment(docgraph, res, list, null);
				}
				ts.Complete();
			}
		}

		public virtual void ShipmentINTranRowPersisted(PXCache sender, PXRowPersistedEventArgs e)
		{
			INTran row = e.Row as INTran;

			if (e.Operation != PXDBOperation.Insert|| e.TranStatus != PXTranStatus.Open || row == null)
				return;

			using (PXDataRecord rec = PXDatabase.SelectSingle(
				new PXDataField(),
				new PXDataFieldValue(row.SOShipmentType),
				new PXDataFieldValue(row.SOShipmentNbr),
				new PXDataFieldValue(row.SOShipmentLineNbr),
				new PXDataFieldValue(row.DocType),
				new PXDataFieldValue(row.RefNbr, PXComp.NE)))
			{
				if (rec != null)
					throw new PXException(ErrorMessages.RecordAddedByAnotherProcess, sender.DisplayName, ErrorMessages.ChangesWillBeLost);
			}
		}

		public virtual void PostShipment(INRegisterEntryBase docgraph, PXResult sh, DocumentList list, ARInvoice invoice)
		{
			try
			{
				SOOrderShipment shiporder = sh;
				SOOrder order = sh;
				var reattachedPlans = new List();
				var orderEntry = new Lazy(() => PXGraph.CreateInstance());
				using (docgraph.TranSplitPlanExt.ReleaseModeScope())
				{
					docgraph.RowPersisted.AddHandler(ShipmentINTranRowPersisted);

					GL.Branch branch = PXSelectJoin>>, Where>>>.SelectSingleBound(this, null); //TODO: Need review INRegister Branch and SOShipment SiteID/DestinationSiteID AC-55773

					if (!Document.Cache.IsDirty)
					{
						this.Clear();
						docgraph.Clear();

						Document.Current = Document.Search(shiporder.ShipmentNbr);
					}

					docgraph.insetup.Current.HoldEntry = false;
					docgraph.insetup.Current.RequireControlTotal = false;

					bool needInsertNewDoc = false;
					INRegister newdoc =
						list.Find(shiporder.ShipmentType, shiporder.ShipmentNbr)
						?? new INRegister();

					if (newdoc.RefNbr != null)
					{
						docgraph.INRegisterDataMember.Current = PXSelect.Search(docgraph, newdoc.DocType, newdoc.RefNbr);
						if (docgraph.INRegisterDataMember.Current != null && docgraph.INRegisterDataMember.Current.SrcRefNbr == null) //Non-db fields cannot be restored after .Clear()
						{
							docgraph.INRegisterDataMember.Current.SrcDocType = shiporder.ShipmentType;
							docgraph.INRegisterDataMember.Current.SrcRefNbr = shiporder.ShipmentNbr;
						}
					}
					else
					{
						newdoc.BranchID = (shiporder.ShipmentType == SOShipmentType.Transfer) ? branch.BranchID : (invoice?.BranchID ?? order.BranchID);
						newdoc.DocType = shiporder.ShipmentType;
						newdoc.SiteID = shiporder.SiteID;
						newdoc.ToSiteID = Document.Current.DestinationSiteID;
						if (newdoc.DocType == SOShipmentType.Transfer)
						{
							newdoc.TransferType = INTransferType.TwoStep;
						}
						if (invoice == null)
						{
							newdoc.TranDate = shiporder.ShipDate;
						}
						else
						{
							newdoc.TranDate = invoice.DocDate;
							newdoc.FinPeriodID = invoice.FinPeriodID;
						}
						newdoc.OrigModule = GL.BatchModule.SO;
						newdoc.SrcDocType = shiporder.ShipmentType;
						newdoc.SrcRefNbr = shiporder.ShipmentNbr;

						needInsertNewDoc = true; // IN Doc will be inserted only if IN Transactions are actually created to prevent unneeded validations
					}

					SOShipLine prev_line = null;
					ARTran prev_artran = null;
					INTran newline = null;

					Dictionary> demand = new Dictionary>();

					foreach (PXResult res in PXSelectJoin, And>>,
						InnerJoin>>>,
					Where>,
						And>,
						And>,
						And>>>>>>.SelectMultiBound(this, new object[] { shiporder }))
					{
						SOShipLineSplit split = res;
						INItemPlan plan = res;

						List ex;
						if (!demand.TryGetValue(split.PlanID, out ex))
						{
							demand[split.PlanID] = ex = new List();
						}
						ex.Add(plan);
					}

					foreach (PXResult res in PXSelectJoin,
							And>>,
						LeftJoin,
							And,
							And>>>,
						LeftJoin,
							And,
							And,
							And,
							And,
							And,
							And,
							And,
							And>>>>>>>>>,
						LeftJoin, And, And>>>,
						LeftJoin>>>>>>,
					Where>,
						And>,
						And>,
						And>,
						And>>>>,
					OrderBy>>>.SelectMultiBound(this, new object[] { shiporder }))
					{
						SOShipLine line = res;
						SOShipLineSplit split = res;
						INItemPlan plan = res;
						INPlanType plantype = INPlanType.PK.Find(this, plan.PlanType) ?? new INPlanType();
						SOLine soline = res;
						ARTran artran = res;
						SOOrderType ordertype = SOOrderType.PK.Find(this, shiporder.OrderType);
						SOShipLineSplit splitcopy = PXCache.CreateCopy(split);

						//TODO: Temporary solution. Review when AC-80210 is fixed
						if ((shiporder.ShipmentNbr != Constants.NoShipmentNbr && shiporder.ShipmentType != SOShipmentType.DropShip && shiporder.Confirmed != true) ||
							line.Confirmed != true || line.IsUnassigned == true ||
							(split.LineType == SOLineType.Inventory && split.IsStockItem == true && split.Confirmed != true))
						{
							throw new PXException(Messages.UnableToProcessUnconfirmedShipment, shiporder.ShipmentNbr);
						}

						//avoid ReadItem()
						if (plan.PlanID != null)
						{
							Caches[typeof(INItemPlan)].SetStatus(plan, PXEntryStatus.Notchanged);
						}

						bool zeroLine = line.BaseShippedQty < 0.0000005m;
						bool reattachExistingPlan = false;
						if (plantype.DeleteOnEvent == true || zeroLine)
						{
							if (zeroLine)
							{
								Caches[typeof(INItemPlan)].Delete(plan);
							}
							else
							{
								reattachExistingPlan = true;
							}

							Caches[typeof(SOShipLineSplit)].MarkUpdated(split, assertError: true);
							split = (SOShipLineSplit)Caches[typeof(SOShipLineSplit)].Locate(split);
							if (split != null)
							{
								split.PlanID = null;
								split.Released = true;
							}

							Caches[typeof(SOShipLineSplit)].IsDirty = true;

							if (zeroLine)
							{
								continue;
							}
						}
						else if (string.IsNullOrEmpty(plantype.ReplanOnEvent) == false)
						{
							plan = PXCache.CreateCopy(plan);
							plan.PlanType = plantype.ReplanOnEvent;
							Caches[typeof(INItemPlan)].Update(plan);

							Caches[typeof(SOShipLineSplit)].MarkUpdated(split, assertError: true);
							Caches[typeof(SOShipLineSplit)].IsDirty = true;
						}

						if ((Caches[typeof(SOShipLine)].ObjectsEqual(prev_line, line) == false || object.Equals(line.InventoryID, split.InventoryID) == false || (line.TaskID != null && line.LocationID != split.LocationID)) && split.IsStockItem == true)
						{
							if (needInsertNewDoc)
							{
								docgraph.INRegisterDataMember.Insert(newdoc);
								needInsertNewDoc = false;
							}
							line.Released = true;
							Caches[typeof(SOShipLine)].MarkUpdated(line, assertError: true);
							Caches[typeof(SOShipLine)].IsDirty = true;

							bool artranReleased = (artran?.Released == true);
							bool shippedNotInvoicedScenario = !artranReleased && line.OrigOrderNbr != null && line.ShipmentNbr != null
									&& ordertype?.UseShippedNotInvoiced == true;
							newline = new INTran();
							newline.BranchID = shiporder.ShipmentType == SOShipmentType.Transfer ? branch.BranchID : (artranReleased ? artran.BranchID : soline.BranchID);
							newline.DocType = newdoc.DocType;
							newline.TranType = line.TranType;
							newline.SOShipmentNbr = line.ShipmentNbr;
							newline.SOShipmentType = line.ShipmentType;
							newline.SOShipmentLineNbr = line.LineNbr;
							newline.SOOrderType = line.OrigOrderType;
							newline.SOOrderNbr = line.OrigOrderNbr;
							newline.SOOrderLineNbr = line.OrigLineNbr;
							newline.SOLineType = line.LineType;
							newline.ARDocType = artran.TranType;
							newline.ARRefNbr = artran.RefNbr;
							newline.ARLineNbr = artran.LineNbr;
							newline.BAccountID = line.CustomerID;
							newline.UpdateShippedNotInvoiced = shippedNotInvoicedScenario;
							if (shippedNotInvoicedScenario)
							{
								newline.COGSAcctID = ordertype.ShippedNotInvoicedAcctID;
								newline.COGSSubID = ordertype.ShippedNotInvoicedSubID;
							}
							if (ordertype.ARDocType != ARDocType.NoUpdate)
							{
								newline.AcctID = artran.AccountID ?? soline.SalesAcctID;
								newline.SubID = artran.SubID ?? soline.SalesSubID;

								if (newline.AcctID == null)
								{
									throw new PXException(ErrorMessages.FieldIsEmpty, PXUIFieldAttribute.GetDisplayName(Caches[typeof(SOLine)]));
								}

								if (newline.SubID == null)
								{
									throw new PXException(ErrorMessages.FieldIsEmpty, PXUIFieldAttribute.GetDisplayName(Caches[typeof(SOLine)]));
								}
							}
							newline.ProjectID = line.ProjectID;
							newline.TaskID = line.TaskID;
							newline.CostCodeID = line.CostCodeID;

							newline.IsStockItem = split.IsStockItem;
							newline.InventoryID = split.InventoryID;
							newline.SiteID = line.SiteID;
							newline.ToSiteID = Document.Current.DestinationSiteID;
							newline.InvtMult = line.InvtMult;
							newline.IsIntercompany = line.IsIntercompany;
							newline.IsSpecialOrder = soline.IsSpecialOrder;
							newline.Qty = 0m;

							if (object.Equals(line.InventoryID, split.InventoryID) == false)
							{
								newline.IsComponentItem = split.IsComponentItem;
								newline.SubItemID = split.SubItemID;
								newline.UOM = split.UOM;
								newline.UnitPrice = 0m;
								newline.UnitCost = GetNSKitComponentUnitCost(soline, line, split) ?? 0;
								newline.TranDesc = null;
							}
							else
							{
								newline.SubItemID = line.SubItemID;
								newline.UOM = line.UOM;
								newline.UnitPrice = INUnitAttribute.ConvertFromTo(
									docgraph.LSSelectDataMember.Cache,
									newline, newline.UOM, artran.UOM, artran.UnitPrice ?? 0m, INPrecision.UNITCOST);
								newline.UnitCost = GetINTranUnitCost(soline, line, split);
								newline.TranDesc = line.TranDesc;
								newline.ReasonCode = line.ReasonCode;

								newline.OrigUOM = line.UOM;
								newline.OrigFullQty = line.ShippedQty;
								newline.BaseOrigFullQty = line.BaseShippedQty;
							}
							docgraph.CostCenterDispatcherExt?.SetCostLayerType(newline);

							PXFieldDefaulting cancelDefaulting = (c, e) => { e.Cancel = true; e.NewValue = null; };
							docgraph.FieldDefaulting.AddHandler(cancelDefaulting);
							try
							{
							newline = docgraph.LSSelectDataMember.Insert(newline);
						}
							finally
							{
								docgraph.FieldDefaulting.RemoveHandler(cancelDefaulting);
							}
						}

						prev_line = line;
						prev_artran = artran;

						if (split.IsStockItem == true && split.Qty != 0m)
						{
							INTranSplit newsplit = (INTranSplit)newline;
							newsplit.SplitLineNbr = null;
							newsplit.SubItemID = split.SubItemID;
							newsplit.LocationID = split.LocationID;
							newsplit.LotSerialNbr = split.LotSerialNbr;
							newsplit.ExpireDate = split.ExpireDate;
							newsplit.UOM = split.UOM;
							newsplit.Qty = split.Qty;
							newsplit.BaseQty = null;
							if (line.ShipmentType == SOShipmentType.Transfer)
							{
								newsplit.TransferType = INTransferType.TwoStep;
							}
							if (reattachExistingPlan)
							{
								newsplit.PlanID = plan.PlanID;
								reattachedPlans.Add(plan);
							}

							PXParentAttribute.SetParent(docgraph.INTranSplitDataMember.Cache, newsplit, typeof(INTran), newline);

							newsplit = docgraph.INTranSplitDataMember.Insert(newsplit);

							if (splitcopy.PlanID != null && demand.TryGetValue(splitcopy.PlanID, out List demandPlans))
							{
								SplitDemandAndAssignSupply(orderEntry, docgraph, line, splitcopy, newsplit, demandPlans);
							}

							if (object.Equals(line.InventoryID, split.InventoryID))
							{
								INTran copy = PXCache.CreateCopy(newline);

								docgraph.LSSelectDataMember.Cache.SetValueExt(newline, newline.Qty * newline.UnitCost);

								bool sameUom = string.Equals(newline.UOM, artran.UOM, StringComparison.OrdinalIgnoreCase);
								bool signMismatch = artran.DrCr == DrCr.Credit && artran.SOOrderLineOperation == SOOperation.Receipt
									|| artran.DrCr == DrCr.Debit && artran.SOOrderLineOperation == SOOperation.Issue;
								newline.TranAmt = (signMismatch ? -artran.TranAmt : artran.TranAmt) ?? 0m;
								if (((sameUom ? artran.Qty : artran.BaseQty) ?? 0m) != 0m && (artran.SOShipmentLineNbr == null || artran.TaskID != null))
								{
									object tranAmt = newline.TranAmt * (sameUom ? newline.Qty / artran.Qty : newline.BaseQty / artran.BaseQty);
									docgraph.LSSelectDataMember.Cache.RaiseFieldUpdating(newline, ref tranAmt);
									newline.TranAmt = (decimal?)tranAmt;
								}

								docgraph.LSSelectDataMember.Cache.RaiseRowUpdated(newline, copy);
							}
						}
					}
				}

				if (docgraph.LSSelectDataMember.Cache.IsDirty)
				{
					using (PXTransactionScope ts = new PXTransactionScope())
					{
						docgraph.Save.Press();
						if (orderEntry.IsValueCreated)
							orderEntry.Value.Save.Press();

						PXResultset res = PXSelect>,
								And>>>>
							.Select(this, shiporder.ShipmentNbr, shiporder.ShipmentType);

						bool shipmentReleased = true;
						foreach (SOOrderShipment item in res)
						{
							if (item.OrderType == order.OrderType && item.OrderNbr == order.OrderNbr)
							{
								item.InvtDocType = docgraph.INRegisterDataMember.Current.DocType;
								item.InvtRefNbr = docgraph.INRegisterDataMember.Current.RefNbr;
								item.InvtNoteID = docgraph.INRegisterDataMember.Current.NoteID;

								OrderList.Cache.Update(item);

								UpdatePlansRefNoteID(item, item.InvtNoteID, reattachedPlans);
							}

						shipmentReleased &= item.InvtRefNbr != null || item.CreateINDoc == false;
						}

						Document.Current.Released = shipmentReleased;
						UpdateStatusOnPostShipment(Document.Current);
						Document.Cache.MarkUpdated(Document.Current, assertError: true);

						this.Save.Press();

						INRegister existing;
						if ((existing = list.Find(docgraph.INRegisterDataMember.Current)) == null)
						{
							list.Add(docgraph.INRegisterDataMember.Current);
						}
						else
						{
							docgraph.INRegisterDataMember.Cache.RestoreCopy(existing, docgraph.INRegisterDataMember.Current);
						}
						ts.Complete();
					}
				}
				else
				{
					SOShipment shipment = Document.Current;

					bool isTransfer = shipment.BilledOrderCntr == 0 &&
						shipment.UnbilledOrderCntr == 0 &&
						shipment.ReleasedOrderCntr == 0;

					if (isTransfer)
					{
						bool hasOnlyNonStockItems = PXParentAttribute.SelectChildren(OrderList.Cache, shipment, typeof(SOShipment))
								.Cast().All(s => s.CreateINDoc != true);

						if (isTransfer && hasOnlyNonStockItems)
						{
							shipment.Released = true;
							UpdateStatusOnPostShipment(shipment);
						Document.Cache.MarkUpdated(shipment);
							Save.Press();
						}
					}
				}
			}
			finally
			{
				docgraph.RowPersisted.RemoveHandler(ShipmentINTranRowPersisted);
			}
		}

		protected virtual void SplitDemandAndAssignSupply(Lazy orderEntry, INRegisterEntryBase docgraph,
			SOShipLine line, SOShipLineSplit splitcopy, INTranSplit newsplit, List demandPlans)
		{
			decimal? restShipQty = line.BaseShippedQty;
			foreach (INItemPlan demandPlan in demandPlans)
			{
				if (restShipQty >= demandPlan.PlanQty)
				{
					demandPlan.SupplyPlanID = newsplit.PlanID;
					docgraph.Caches().MarkUpdated(demandPlan, assertError: true);
					restShipQty -= demandPlan.PlanQty;
				}
				else
				{
					restShipQty -= SplitDemandAndAssignSupply(orderEntry.Value, line, demandPlan, newsplit, restShipQty);
				}
			}
		}

		protected virtual decimal? SplitDemandAndAssignSupply(SOOrderEntry orderEntry, SOShipLine line, INItemPlan demandPlan, INTranSplit newsplit, decimal? restShipQty)
		{
			if (!LoadDemandOrder(orderEntry, demandPlan))
				return 0m;
			demandPlan.SupplyPlanID = newsplit.PlanID;
			orderEntry.Caches().MarkUpdated(demandPlan, assertError: true);
			decimal? restDemandQty = demandPlan.PlanQty - restShipQty;

			// 1. create first split and assign part to the back ordered qty in the transfer order if any
			PXResult backOrdered = SelectFrom
				.InnerJoin.On
				.Where
					.And
					.And
					.And
					.And>>>>>
				.View.SelectSingleBound(orderEntry, new[] { line });
			if (backOrdered != null)
			{
				INItemPlan backOrderedPlan = PXResult.Unwrap(backOrdered);
				decimal? assignedToBackOrderQty = restDemandQty <= backOrderedPlan.PlanQty ? restDemandQty : backOrderedPlan.PlanQty;

				InsertNewSOLineSplit(orderEntry, assignedToBackOrderQty, backOrderedPlan.PlanID);

				restDemandQty -= assignedToBackOrderQty;
			}
			// 2. create second split for the rest qty to create a new transfer order further
			if (restDemandQty > 0m)
			{
				InsertNewSOLineSplit(orderEntry, restDemandQty, null);

				restDemandQty = 0m;
			}

			return restShipQty;
		}

		protected virtual SOLineSplit InsertNewSOLineSplit(SOOrderEntry orderEntry, decimal? baseQty, long? supplyPlanID)
		{
			using (orderEntry.LineSplittingExt.SuppressedModeScope(true))
			{
				SOLineSplit oldSplit = orderEntry.splits.Current;
				decimal? reducedBaseQty = oldSplit.BaseQty - baseQty;
				if (reducedBaseQty <= 0m)
					return null;
				oldSplit = PXCache.CreateCopy(oldSplit);
				oldSplit.BaseQty = reducedBaseQty;
				oldSplit.Qty = INUnitAttribute.ConvertFromBase(orderEntry.splits.Cache, oldSplit.InventoryID, oldSplit.UOM, (decimal)oldSplit.BaseQty, INPrecision.QUANTITY);
				oldSplit = orderEntry.splits.Update(oldSplit);
				var selectPlanCmd = new SelectFrom.Where>.View(orderEntry);
				INItemPlan oldPlan = selectPlanCmd.Select(oldSplit.PlanID);
				oldPlan = PXCache.CreateCopy(oldPlan);
				oldPlan.SiteID = oldSplit.ToSiteID;

				if (oldPlan.CostCenterID != CostCenter.FreeStock)
				{
					var line = SOLineSplit.FK.OrderLine.FindParent(this, oldSplit);
					if (line == null)
						throw new Common.Exceptions.RowNotFoundException(soline.Cache, oldSplit.OrderType, oldSplit.OrderNbr, oldSplit.LineNbr);

					oldPlan.CostCenterID = line.CostCenterID;
				}

				oldPlan = (INItemPlan)orderEntry.Caches().Update(oldPlan);

				SOLineSplit newSplit = PXCache.CreateCopy(oldSplit);
				newSplit.PlanID = null;
				newSplit.SplitLineNbr = null;
				newSplit.ShipmentNbr = null;
				if (supplyPlanID == null)
				{
					newSplit.IsAllocated = false;
					newSplit.SiteID = null;
					newSplit.CostCenterID = null;
					newSplit.ClearPOFlags();
					newSplit.ClearSOReferences();
					newSplit.POType = null;
					newSplit.PONbr = null;
					newSplit.POLineNbr = null;
					newSplit.VendorID = null;
					newSplit.RefNoteID = null;
				}
				newSplit.BaseReceivedQty = 0m;
				newSplit.ReceivedQty = 0m;
				newSplit.BaseShippedQty = 0m;
				newSplit.ShippedQty = 0m;
				newSplit.BaseQty = 0m;
				newSplit.Qty = 0m;
				newSplit = orderEntry.splits.Insert(newSplit);
				if (supplyPlanID == null)
				{
					int? saveSiteID = newSplit.SiteID;
					newSplit = PXCache.CreateCopy(newSplit);
					newSplit.IsAllocated = true;
					newSplit.SiteID = oldSplit.SiteID;
					newSplit = orderEntry.splits.Update(newSplit);
					var accumStatus = new SiteStatusByCostCenter()
					{
						InventoryID = newSplit.InventoryID,
						SubItemID = newSplit.SubItemID,
						SiteID = newSplit.SiteID,
						CostCenterID = newSplit.CostCenterID,
					};
					var status = INSiteStatusByCostCenter.PK.Find(orderEntry, accumStatus);
					accumStatus = (SiteStatusByCostCenter)orderEntry.Caches().Locate(accumStatus);
					decimal qtyHardAvail = status?.QtyHardAvail ?? 0m + accumStatus?.QtyHardAvail ?? 0m;
					if (qtyHardAvail < baseQty)
					{
						// revert hard allocation if not enough stock
						newSplit = PXCache.CreateCopy(newSplit);
						newSplit.IsAllocated = false;
						newSplit.SiteID = saveSiteID;
						newSplit = PXCache.CreateCopy(orderEntry.splits.Update(newSplit));
						newSplit.POCreate = false;
						newSplit = orderEntry.splits.Update(newSplit);
					}
				}
				newSplit = PXCache.CreateCopy(newSplit);
				newSplit.BaseQty = baseQty;
				newSplit.Qty = INUnitAttribute.ConvertFromBase(orderEntry.splits.Cache, newSplit.InventoryID, newSplit.UOM, (decimal)newSplit.BaseQty, INPrecision.QUANTITY);
				newSplit = orderEntry.splits.Update(newSplit);
				if (supplyPlanID != null)
				{
					INItemPlan newPlan = selectPlanCmd.Select(newSplit.PlanID);
					newPlan = PXCache.CreateCopy(newPlan);
					newPlan.SupplyPlanID = supplyPlanID;
					newPlan.PlanType = INPlanConstants.Plan93;
					newPlan.SiteID = newSplit.ToSiteID;
					newPlan.CostCenterID = oldPlan.CostCenterID;
					newPlan.FixedSource = null;
					newPlan = (INItemPlan)orderEntry.Caches().Update(newPlan);
				}
				orderEntry.splits.Current = oldSplit;
				return newSplit;
			}
		}

		protected virtual bool LoadDemandOrder(SOOrderEntry orderEntry, INItemPlan demandPlan)
		{
			SOLineSplit demandSplit = SelectFrom.Where>.View.Select(orderEntry, demandPlan.PlanID);
			if (demandSplit == null)
				return false;
			orderEntry.Document.Current = orderEntry.Document.Search(demandSplit.OrderNbr, demandSplit.OrderType);
			orderEntry.Transactions.Current = orderEntry.Transactions.Search(
				demandSplit.OrderType, demandSplit.OrderNbr, demandSplit.LineNbr);
			orderEntry.splits.Current = demandSplit;
			return true;
		}

		protected virtual void UpdateStatusOnPostShipment(SOShipment shipment)
		{
			if (shipment.UnbilledOrderCntr == 0 &&
				shipment.BilledOrderCntr == 0 &&
				shipment.ReleasedOrderCntr == 0 &&
				shipment.Released == true)
			{
				MarkCompleted(shipment);
			}
		}

		protected virtual decimal? GetINTranUnitCost(SOLine soline, SOShipLine line, SOShipLineSplit split)
		{
			if (line.Operation == SOOperation.Receipt && !string.IsNullOrEmpty(line.LotSerialNbr)
				&& InventoryItem.PK.Find(this, line.InventoryID)?.ValMethod == INValMethod.Specific)
			{
				var origINTranCosts = SelectFrom
					.InnerJoin.On
					.InnerJoin
						.On
							.And>
							.And>>
					.Where
						.And>
						.And>
						.And>>
					.View.ReadOnly.Select(this,
						line.LotSerialNbr,
						soline.InvoiceType,
						soline.InvoiceNbr,
						soline.InvoiceLineNbr)
					.RowCast().ToList();
				decimal? qtySum = origINTranCosts.Sum(c => c.Qty);
				if ((qtySum ?? 0m) != 0m)
				{
					return INUnitAttribute.ConvertToBase(
						Transactions.Cache,
						line.InventoryID,
						line.UOM,
						origINTranCosts.Sum(c => c.TranCost).Value / qtySum.Value,
						INPrecision.UNITCOST);
				}
			}

			return line.UnitCost;
		}

		protected virtual decimal? GetNSKitComponentUnitCost(SOLine soline, SOShipLine line, SOShipLineSplit split)
		{
			INTran invoiced = null;

			if (line.Operation == SOOperation.Receipt)
			{
				invoiced = SelectFrom.
							Where.
							And>.
							And>.
							And>>
					.View.SelectSingleBound(this, new[] { split });
				if (invoiced != null)
				{
					return invoiced.UnitCost;
				}

				var itemSite = INItemSite.PK.Find(this, split.InventoryID, split.SiteID);
				if (itemSite?.TranUnitCost != null)
				{
					return itemSite.TranUnitCost;
				}
				else
				{
					var branch = GL.Branch.PK.Find(this, soline.BranchID);
					var itemCost = INItemCost.PK.Find(this, split.InventoryID, branch?.BaseCuryID);
					return itemCost?.TranUnitCost ?? 0m;
				}
			}

			PXResultset resultset = PXSelectJoin, And, And>>>>,
				Where>,
				And>,
				And>,
				And>>>>>>
				.Select(this, soline.InvoiceType, soline.InvoiceNbr, line.InventoryID, split.InventoryID);
			if (!string.IsNullOrEmpty(split.LotSerialNbr))
			{
				invoiced = resultset.AsEnumerable().Where(intran => string.Equals(((INTran)intran).LotSerialNbr, split.LotSerialNbr, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
				if (invoiced != null)
					return invoiced.UnitCost;
			}
			invoiced = resultset;
			if (invoiced != null)
				return invoiced.UnitCost;

			return null;
		}

		public virtual void UpdatePlansRefNoteID(SOOrderShipment orderShipment, Guid? refNoteID, IEnumerable reattachedPlans)
		{
			if (!reattachedPlans.Any()) return;

			// supposed that at this point there may be only deleted INItemPlan records in our graph
			// they should be persisted before the following direct update
			this.Caches[typeof(INItemPlan)].Persist(PXDBOperation.Delete);
			this.Caches[typeof(INItemPlan)].Persisted(false);

			// update INItemPlan.RefNoteID with the new IN Issue identifier
			PXUpdateJoin<
				Set,
				Set>>,
				INItemPlan,
					InnerJoin>>,
				Where>,
					And>,
					And>>>>>
			.Update(this,
				refNoteID,
				orderShipment.OrderType,
				orderShipment.OrderNbr,
				orderShipment.ShipmentNbr);

			var stamp = PXDatabase.SelectTimeStamp();
			foreach (var plan in reattachedPlans)
				PXTimeStampScope.PutPersisted(this.Caches[typeof(INItemPlan)], plan, stamp);
		}

		#endregion

		#region Discount

		private void AllocateGroupFreeItems(SOOrder order)
		{
			Dictionary freeItems = new Dictionary();
			List shipLinesToCheck = new List();
			bool freeItemPresent = false;

			foreach (SOShipLine line in Transactions.Select())
			{
				if (line.OrigOrderType == order.OrderType && line.OrigOrderNbr == order.OrderNbr && line.IsFree == false) shipLinesToCheck.Add(line);
				if (line.IsFree == true) freeItemPresent = true;
			}

			bool useBaseQty = DiscountEngine.ApplyQuantityDiscountByBaseUOMForAR(this);

			if (freeItemPresent)
			{
				soorder.Current = order;
				PXCache cache = this.Caches[typeof(SOLine)];
				PXSelectBase transactions = new PXSelect>, And>>>>(this);
				PXSelectBase discountdetail = new PXSelect>, And>>>>(this);

				TwoWayLookup discountCodesWithApplicableSOLines = DiscountEngineProvider.GetEngineFor()
					.GetListOfLinksBetweenDiscountsAndDocumentLines(cache, transactions, discountdetail);

				if (sosetup.Current.FreeItemShipping == FreeItemShipType.Proportional)
				{
					foreach (SOOrderDiscountDetail dsGroup in discountCodesWithApplicableSOLines.LeftValues.Where(x => x.FreeItemQty > 0m && x.SkipDiscount != true))
					{
						decimal shippedQty = 0m;
						decimal shippedGroupQty = 0m;
						foreach (SOLine soLine in discountCodesWithApplicableSOLines.RightsFor(dsGroup))
						{
							foreach (SOShipLine shipLine in shipLinesToCheck)
							{
								if (soLine.LineNbr == shipLine.OrigLineNbr)
								{
									shippedGroupQty += ((useBaseQty ? shipLine.BaseShippedQty : shipLine.ShippedQty) ?? 0m);
								}
							}
						}

						shippedQty = (shippedGroupQty * (decimal)dsGroup.FreeItemQty / (decimal)dsGroup.DiscountableQty);

						DiscKey discKey = new DiscKey(dsGroup.DiscountID, dsGroup.DiscountSequenceID, (int)dsGroup.FreeItemID);
						freeItems.Add(discKey, Math.Floor(shippedQty));
					}
				}
				else
				{
					//Ship on last shipment
					foreach (SOOrderDiscountDetail dsGroup in discountCodesWithApplicableSOLines.LeftValues.Where(x => x.FreeItemQty > 0m && x.SkipDiscount != true))
					{
						decimal shippedBOGroupQty = 0m;
						decimal orderBOGroupQty = 0m;
						decimal shippedGroupQty = 0m;
						decimal orderGroupQty = 0m;

						decimal shippedQty = 0m;
						foreach (SOLine soLine in discountCodesWithApplicableSOLines.RightsFor(dsGroup))
						{
							SOLine2 keys = new SOLine2();
							keys.OrderType = soLine.OrderType;
							keys.OrderNbr = soLine.OrderNbr;
							keys.LineNbr = soLine.LineNbr;

							SOLine2 solineWithUpdatedShippedQty = (SOLine2)this.Caches[typeof(SOLine2)].Locate(keys);
							if (solineWithUpdatedShippedQty != null)
							{
								orderGroupQty += soLine.Qty ?? 0m;
								if (soLine.ShipComplete == SOShipComplete.BackOrderAllowed)
								{
									orderBOGroupQty += soLine.Qty ?? 0m;
									if (solineWithUpdatedShippedQty.ShippedQty >= soLine.OrderQty)
									{
										if (soLine.LineNbr == solineWithUpdatedShippedQty.LineNbr)
										{
											shippedBOGroupQty += (solineWithUpdatedShippedQty.ShippedQty ?? 0m);
										}
									}
								}
								else
								{
									shippedGroupQty += solineWithUpdatedShippedQty.ShippedQty ?? 0m;
								}
							}
						}

							if (shippedGroupQty + shippedBOGroupQty < orderGroupQty)
							shippedQty = ((shippedGroupQty + shippedBOGroupQty) / (decimal)dsGroup.DiscountableQty) * (decimal)dsGroup.FreeItemQty;
							else
							shippedQty = (decimal)dsGroup.FreeItemQty;

						DiscKey discKey = new DiscKey(dsGroup.DiscountID, dsGroup.DiscountSequenceID, (int)dsGroup.FreeItemID);
						freeItems.Add(discKey, shippedBOGroupQty >= orderBOGroupQty ? Math.Floor(shippedQty) : 0m);
					}
				}

				foreach (KeyValuePair kv in freeItems)
				{
					SOShipmentDiscountDetail sdd = new SOShipmentDiscountDetail();
					sdd.Type = DiscountType.Line;
					sdd.OrderType = order.OrderType;
					sdd.OrderNbr = order.OrderNbr;
					sdd.DiscountID = kv.Key.DiscID;
					sdd.DiscountSequenceID = kv.Key.DiscSeqID;
					sdd.FreeItemID = kv.Key.FreeItemID;
					sdd.FreeItemQty = kv.Value;

					UpdateInsertDiscountTrace(sdd);
				}
			}
		}

		private struct DiscKey
		{
			string discID;
			string discSeqID;
			int freeItemID;

			public string DiscID { get { return discID; } }
			public string DiscSeqID { get { return discSeqID; } }
			public int FreeItemID { get { return freeItemID; } }

			public DiscKey(string discID, string discSeqID, int freeItemID)
			{
				this.discID = discID;
				this.discSeqID = discSeqID;
				this.freeItemID = freeItemID;
			}
		}

		private DiscountSequence GetDiscountSequenceByID(string discountID, string discountSequenceID)
		{
			return PXSelect>,
				And>>>>.Select(this, discountID, discountSequenceID);

		}

		private void RecalculateFreeItemQtyTotal()
		{
			if (Document.Current != null)
			{
				Document.Cache.SetValueExt(Document.Current, SumFreeItemQtyTotal());
			}
		}

		private decimal SumFreeItemQtyTotal()
		{
			PXSelectBase select =
					new PXSelect>>>(this);

			decimal total = 0;
			foreach (SOShipmentDiscountDetail record in select.Select())
			{
				total += record.FreeItemQty ?? 0;
			}

			return total;
		}

		private void AdjustFreeItemLines()
		{
			foreach (SOShipLine line in Transactions.Select())
			{
				if (line.IsFree == true && line.ManualDisc != true)
					AdjustFreeItemLines(line);
			}

			Transactions.View.RequestRefresh();
		}

		private bool skipAdjustFreeItemLines = false;

		private void AdjustFreeItemLines(SOShipLine line)
		{
			if (skipAdjustFreeItemLines) return;

			PXSelectBase select = new PXSelect>,
				And>,
				And>,
				And>>>>>>(this);

			PXResultset shipmentDiscountDetails = select.Select(line.InventoryID, line.OrigOrderType, line.OrigOrderNbr);
			if (shipmentDiscountDetails.Count != 0)
			{
				decimal? qtyTotal = 0;
				foreach (SOShipmentDiscountDetail item in shipmentDiscountDetails)
				{
					if (item.FreeItemID != null && item.FreeItemQty != null && item.FreeItemQty.Value > 0)
					{
						qtyTotal += item.FreeItemQty.Value;
					}
				}

				SOShipLine oldLine = PXCache.CreateCopy(line);
				oldLine.ShippedQty = qtyTotal;
				FreeItems.Update(oldLine);
			}
			//Note: Do not delete Free item line if its qty = 0.
			//New free item is not inserted if the qty of the original line is increased.
		}

		private void UpdateInsertDiscountTrace(SOShipmentDiscountDetail newTrace)
		{
			SOShipmentDiscountDetail trace = PXSelect>,
					And>,
					And>,
					And>,
					And>,
					And>>>>>>>>.Select(this, newTrace.OrderType, newTrace.OrderNbr, newTrace.Type, newTrace.DiscountID, newTrace.DiscountSequenceID);

			if (trace != null)
			{
				trace.DiscountableQty = newTrace.DiscountableQty;
				trace.DiscountPct = newTrace.DiscountPct;
				trace.FreeItemID = newTrace.FreeItemID;
				trace.FreeItemQty = newTrace.FreeItemQty;

				_discountEngine.UpdateDiscountDetail(DiscountDetails.Cache, DiscountDetails, trace);
			}
			else
				_discountEngine.InsertDiscountDetail(DiscountDetails.Cache, DiscountDetails, newTrace);
		}

		private bool ProrateDiscount
		{
			get
			{
				SOSetup sosetup = PXSelect.Select(this);

				if (sosetup == null)
				{
					return true;//default true
				}
				else
				{
					if (sosetup.ProrateDiscounts == null)
						return true;
					else
						return sosetup.ProrateDiscounts == true;
				}

			}
		}

		#endregion

		#region Packaging into boxes

		protected virtual SOPackageEngine CreatePackageEngine()
		{
			return new SOPackageEngine(this);
		}

		#endregion

		protected virtual bool SyncLineWithOrder(SOShipLine row)
		{
			if (row.ShippedQty == 0)
			{
				var soLine = PXParentAttribute.SelectParent(Transactions.Cache, row);
				if (soLine != null)
					return soLine.SiteID == row.SiteID;
			}
			return true;
		}

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
								CM.PXCurrencyAttribute.CuryConvCury(Document.Cache, Document.Current);

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

											var pluginMethod = PXSelectorAttribute.Select(this.carrier.Cache, carrier) as PX.Objects.CS.CarrierMethodSelectorAttribute.CarrierPluginMethod;
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

		protected virtual System.Threading.Tasks.Task PrintPickList(List list, CancellationToken cancellationToken)
		{
			return PrintPickList(list, null, cancellationToken);
		}

		protected virtual async System.Threading.Tasks.Task PrintPickList(List list, PXAdapter adapter, CancellationToken cancellationToken)
		{
			if (list.Count == 0) return;
			Document.Current = list[0];
			int? branchID;
			using (new PXReadBranchRestrictedScope())
			{
				GL.Branch company = Company.Select();
				branchID = company.BranchID;
			}

			PXReportRequiredException ex = null;
			foreach (SOShipment order in list)
			{
				order.PickListPrinted = true;
				Document.Update(order);

				if (order.Hold == true)
					this.releaseFromHold.PressWithSuppressedWorkflowPersist();
			}

			PXRowPersisted shipmentPersisted = (sender, eventArgs) =>
			{
				if (eventArgs != null && eventArgs.Row != null && eventArgs.TranStatus == PXTranStatus.Completed)
				{
					var shipment = (SOShipment)eventArgs.Row;

					if (shipment.PickListPrinted == true)
					{
						Dictionary parameters = new Dictionary();
						parameters["SOShipment.ShipmentNbr"] = shipment.ShipmentNbr;
						string actualReportID = new NotificationUtility(this).SearchCustomerReport(SOReports.PrintPickList, shipment.CustomerID, branchID);
						ex = PXReportRequiredException.CombineReport(ex, actualReportID, parameters);
						ex.Mode = PXBaseRedirectException.WindowMode.New;
					}
				}
			};

			RowPersisted.AddHandler(shipmentPersisted);

			try
			{
				this.Save.Press();
			}
			finally
			{
				RowPersisted.RemoveHandler(shipmentPersisted);
			}

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

		public override void Persist()
		{
				foreach (SOShipLine line in Transactions.Cache.Deleted
					.Concat_(Transactions.Cache.Updated)
					.Concat_(Transactions.Cache.Inserted))
				{
					this.SyncUnassigned(line);
				}

			base.Persist();
		}

		protected bool IsSyncUnassignedScope;
		protected int? UnassignedSplitsLocationID
		{
			get; private set;
			}
		protected decimal? QuantityToCreate
			{
			get; private set;
			}

		public class SyncUnassignedScope : IDisposable
		{
			private readonly SOShipmentEntry parent;

			public SyncUnassignedScope(SOShipmentEntry shipmentEntry, int? locationID, decimal? quantity = null)
			{
				parent = shipmentEntry;
				parent.IsSyncUnassignedScope = true;
				parent.UnassignedSplitsLocationID = locationID;
				parent.QuantityToCreate = quantity;
		}

			void IDisposable.Dispose()
			{
				parent.UnassignedSplitsLocationID = null;
				parent.IsSyncUnassignedScope = false;
				parent.QuantityToCreate = null;
			}
		}

		public bool IsPPS
		{
			get
			{
				return this.FindImplementation() != null;
			}
		}

		public virtual void SyncUnassigned(SOShipLine line)
		{
			if (line.IsUnassigned != true && line.UnassignedQty == 0m || line.Operation != SOOperation.Issue)
				return;

			var item = InventoryItem.PK.Find(this, line.InventoryID.Value);
			INLotSerClass lotSerClass = null;
			if (item != null && item.StkItem == true)
			{
				lotSerClass = INLotSerClass.PK.Find(this, item.LotSerClassID);
			}
			if (lotSerClass == null || lotSerClass.IsManualAssignRequired != true)
				return;

			bool deleteUnassigned = false;
			bool recreateUnassigned = false;
			int? deletedLocation = null;
			decimal? deletedQuantity = null;

			List> unassignedSplitRows = null;
			var linesCache = Transactions.Cache;
			if (linesCache.GetStatus(line) == PXEntryStatus.Deleted || line.UnassignedQty == 0m)
			{
				deleteUnassigned = true;
			}
			else if (unassignedSplits.Cache.Updated.RowCast().Any(s => s.LineNbr == line.LineNbr)
				|| unassignedSplits.Cache.Deleted.RowCast().Any(s => s.LineNbr == line.LineNbr))
			{
				recreateUnassigned = true;
			}
			else if (splits.Cache.Updated.RowCast().Any(s => s.LineNbr == line.LineNbr
					&& !splits.Cache.ObjectsEqual(s, splits.Cache.GetOriginal(s) as SOShipLineSplit))
				|| splits.Cache.Deleted.RowCast().Any(s => s.LineNbr == line.LineNbr))
			{
				if (IsPPS && lotSerClass?.LotSerTrack == INLotSerTrack.SerialNumbered)
				{
					deletedQuantity = splits.Cache.Deleted.RowCast().Sum(s => s.BaseQty);
					if (deletedQuantity == 0)
					{
						deletedQuantity = null;
					}

					var locations = splits.Cache.Deleted.RowCast().Where(s => s.LineNbr == line.LineNbr).Select(s => s.LocationID).Distinct();
					if (locations.Count() == 1)
					{
						deletedLocation = locations.First();
					}
				}
				recreateUnassigned = true;
			}
			else if (!Equals(line.LocationID, linesCache.GetValueOriginal(line)) && line.LocationID != null)
			{
				recreateUnassigned = true;
			}
			else
			{
				var insertedSplits = splits.Cache.Inserted.RowCast().ToList();
				decimal? insertedSplitsQty = insertedSplits.Sum(s => s.BaseQty ?? 0m);

				unassignedSplitRows = PXSelectJoin>>,
					Where>,
						And>>>,
					OrderBy>>
					.Select(this, line.ShipmentNbr, line.LineNbr).ToList();
				decimal? unassignedSplitsQty = unassignedSplitRows.Sum(r => ((Unassigned.SOShipLineSplit)r).BaseQty);

				decimal? qtyToReduceUnassigned = unassignedSplitsQty - line.UnassignedQty;
				if (insertedSplitsQty <= qtyToReduceUnassigned)
				{
					var locations = new List();
					var locationsAssignedQty = new Dictionary();
					foreach (SOShipLineSplit split in insertedSplits)
					{
						int locationID = split.LocationID ?? -1;
						if (!locationsAssignedQty.ContainsKey(locationID))
						{
							locations.Add(locationID);
							locationsAssignedQty.Add(locationID, 0m);
						}
						locationsAssignedQty[locationID] += split.BaseQty;
					}
					locations.Add(int.MinValue);
					locationsAssignedQty[int.MinValue] = qtyToReduceUnassigned - insertedSplitsQty;

					ApplyAssignedQty(locations, locationsAssignedQty, unassignedSplitRows, true);
					ApplyAssignedQty(locations, locationsAssignedQty, unassignedSplitRows, false);
				}
				else
				{
					recreateUnassigned = true;
				}
			}

			if (deleteUnassigned || recreateUnassigned && deletedQuantity == null)
			{
				this.DeleteUnassignedSplits(line, unassignedSplitRows);
			}
			line.IsUnassigned = line.UnassignedQty != 0m;

			if (recreateUnassigned && line.IsUnassigned == true)
			{
				decimal? remainQty = null;
				using (new SyncUnassignedScope(this, line.LocationID ?? deletedLocation, deletedQuantity))
				{
					remainQty = RecreateUnassignedSplits(line, lotSerClass);
				}

				if (remainQty > 0 && line.LocationID != null)
				{
					var pars = new object[]
					{
						linesCache.GetStateExt(line),
						linesCache.GetStateExt(line),
						linesCache.GetStateExt(line),
						linesCache.GetStateExt(line)
					};

					linesCache.RaiseExceptionHandling(line,
						linesCache.GetStateExt(line),
						new PXSetPropertyException(IN.Messages.StatusCheck_QtyLocationNegative, PXErrorLevel.Error, pars));

					throw new PXException(IN.Messages.StatusCheck_QtyLocationNegative, pars);
				}
			}
		}

		protected virtual void ValidateShipComplete(SOShipment shipment)
		{
			string orderType = null, orderNbr = null;

			ValidateLineShipComplete(shipment, ref orderType, ref orderNbr);
			ValidateOrderShipComplete(shipment, ref orderType, ref orderNbr);

			if (orderType != null)
				throw new PXException(Messages.CannotShipCompleteTraced, orderType, orderNbr);
		}

		private void ValidateLineShipComplete(SOShipment shipment, ref string orderType, ref string orderNbr)
		{
			foreach (SOLine2 orderLine in soline.Cache.Updated)
			{
				bool theSameOrder = (orderType == null || (orderType == orderLine.OrderType && orderNbr == orderLine.OrderNbr));

				if (theSameOrder && !object.Equals(soline.Cache.GetValueOriginal(orderLine), orderLine.BaseShippedQty))
				{
					var orderShipment = new SOOrderShipment()
					{
						OrderType = orderLine.OrderType,
						OrderNbr = orderLine.OrderNbr,
						ShippingRefNoteID = shipment.NoteID
					};
					orderShipment = OrderList.Locate(orderShipment) ?? orderShipment;
					var orderShipmentStatus = OrderList.Cache.GetStatus(orderShipment);

					if (orderShipmentStatus.IsIn(PXEntryStatus.Deleted, PXEntryStatus.InsertedDeleted))
						continue;

					if (!ValidateLineShipComplete(orderLine))
					{
						orderType = orderLine.OrderType;
						orderNbr = orderLine.OrderNbr;
					}
				}
			}
		}

		private bool ValidateLineShipComplete(SOLine2 orderLine)
		{
			if (orderLine.ShipComplete == SOShipComplete.ShipComplete && orderLine.LineType != SOLineType.MiscCharge &&
				orderLine.LineSign * orderLine.BaseShippedQty < orderLine.LineSign * orderLine.BaseOrderQty * orderLine.CompleteQtyMin / 100m)
			{
				if (orderLine.BaseShippedQty == 0m)
				{
					SOShipLine shipLine = SelectFrom
						.Where
							.And>
							.And>>
						.View.SelectSingleBound(this, new object[] { orderLine });

					bool shipLineDeleted = (shipLine == null);

					if (shipLineDeleted)
						return true;
				}

				var inventoryCD = InventoryItem.PK.Find(this, orderLine.InventoryID)?.InventoryCD?.TrimEnd();
				PXTrace.WriteInformation(Messages.CannotSaveComplete_Line, inventoryCD, orderLine.OrderType, orderLine.OrderNbr);

				return false;
			}

			return true;
		}

		private void ValidateOrderShipComplete(SOShipment shipment, ref string orderType, ref string orderNbr)
		{
			if (!Transactions.Cache.Deleted.Any_() &&
				!Transactions.Cache.Inserted.Any_() &&
				object.Equals(Document.Cache.GetValueOriginal(shipment), shipment.ShipDate))
			{
				return;
			}

			var shipLines = new Lazy>(() =>
				Transactions.View.SelectMultiBound(new object[] { shipment })
				.RowCast()
				.Select(l => (l.OrigOrderType, l.OrigOrderNbr, l.OrigLineNbr))
				.ToHashSet());

			foreach (var order in OrderList.View.SelectMultiBound(new object[] { shipment }).RowCast())
			{
				bool shipComplete = (order.ShipComplete == SOShipComplete.ShipComplete);
				bool theSameOrder = (orderType == null || (orderType == order.OrderType && orderNbr == order.OrderNbr));

				if (shipComplete && theSameOrder && !ValidateOrderShipComplete(shipLines.Value, order))
				{
					orderType = order.OrderType;
					orderNbr = order.OrderNbr;
					break;
				}
			}
		}

		private bool ValidateOrderShipComplete(HashSet<(string orderType, string orderNbr, int? lineNbr)> shiplinesLines, SOOrderShipment order)
		{
			bool valid = true;

			var lines = SelectFrom
				.Where
					.And>
					.And>
					.And>
					.And>
					.And>
					.And>
					.And>>>
				.View.ReadOnly.SelectMultiBound(this, new object[] { order });
			
			foreach (SOLine2 line in lines)
			{
				if (!shiplinesLines.Contains((line.OrderType, line.OrderNbr, line.LineNbr)))
				{
					valid = false;

					var inventoryCD = InventoryItem.PK.Find(this, line.InventoryID)?.InventoryCD?.TrimEnd();
					PXTrace.WriteInformation(Messages.CannotSaveComplete_Order, line.OrderType, line.OrderNbr, inventoryCD);
				}
			}

			return valid;
		}

		private void ApplyAssignedQty(
			List locations, Dictionary locationsAssignedQty,
			List> unassignedSplitRows,
			bool onlyCoincidentLocation)
		{
			foreach (int locationID in locations)
			{
				decimal? qtyToAssign = locationsAssignedQty[locationID];
				while (qtyToAssign > 0m && unassignedSplitRows.Count > 0)
				{
					var coincidentLocIndexes = unassignedSplitRows
						.SelectIndexesWhere(r => ((Unassigned.SOShipLineSplit)r).LocationID == locationID);
					int? selectedIndex = coincidentLocIndexes.Any() ? coincidentLocIndexes.First()
						: !onlyCoincidentLocation ? unassignedSplitRows.Count - 1 : (int?)null;
					if (!selectedIndex.HasValue)
						break;

					var selectedUnassigned = unassignedSplitRows[selectedIndex.Value];
					var split = (Unassigned.SOShipLineSplit)selectedUnassigned;

					if (qtyToAssign >= split.BaseQty)
					{
						qtyToAssign -= split.BaseQty;
						unassignedSplits.Delete(split);
						unassignedSplitRows.RemoveAt(selectedIndex.Value);
					}
					else
					{
						split.BaseQty -= qtyToAssign;
						split.Qty = INUnitAttribute.ConvertFromBase(unassignedSplits.Cache, split.InventoryID, split.UOM, (decimal)split.BaseQty, INPrecision.QUANTITY);
						qtyToAssign = 0m;
						unassignedSplits.Update(split);
					}
				}
				locationsAssignedQty[locationID] = qtyToAssign;
			}
		}

		public virtual void DeleteUnassignedSplits(SOShipLine line, IEnumerable> unassignedSplitRows)
		{
			if (unassignedSplitRows == null)
			{
				unassignedSplitRows = PXSelect>,
						And>>>>
					.Select(this, line.ShipmentNbr, line.LineNbr).AsEnumerable();
			}
			foreach (Unassigned.SOShipLineSplit s in unassignedSplitRows)
			{
				unassignedSplits.Cache.Delete(s);
			}
		}

		public virtual decimal? RecreateUnassignedSplits(SOShipLine line, INLotSerClass lotSerClass)
		{
			Transactions.Current = line;

			if (lotSerClass.LotSerAssign == INLotSerAssign.WhenReceived)
			{
				SOLineSplit origSplit = PXSelectReadonly>,
						And>,
						And>,
						And>>>>>>
					.Select(this, line.OrigOrderType, line.OrigOrderNbr, line.OrigLineNbr, line.OrigSplitLineNbr);

				if (!string.IsNullOrEmpty(origSplit?.LotSerialNbr))
				{
					return CreateSplitsForAvailableLots(QuantityToCreate ?? line.UnassignedQty, line.OrigPlanType, origSplit?.LotSerialNbr, line, lotSerClass);
				}
			}

			return CreateSplitsForAvailableNonLots(QuantityToCreate ?? line.UnassignedQty, line.OrigPlanType, line, lotSerClass);
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


		private void UpdateManualFreightCost(SOShipment shipment, SOOrderShipment sOOrderShipment, decimal? oldShipmentQty, decimal? newShipmentQty, bool newOrderSelected = false)
		{
			SOOrder order = soorder.Select(sOOrderShipment.OrderType, sOOrderShipment.OrderNbr);
			if (shipment != null && order != null && order.OrderQty != null && order.OrderQty > 0)
			{
				Carrier carrier = Carrier.PK.Find(this, order.ShipVia);
				if (carrier != null && carrier.CalcMethod == CarrierCalcMethod.Manual)
				{
					if (sosetup.Current?.FreightAllocation == FreightAllocationList.FullAmount && (order.ShipmentCntr > 1))
						return;

					if (sosetup.Current != null)
					{
						SOShipment shipmentCopy = PXCache.CreateCopy(shipment);
						decimal? orderFreightCost = order.FreightCost;
						decimal? shipmentFreightCost = shipmentCopy.CuryFreightCost;

						if (sosetup.Current.FreightAllocation == FreightAllocationList.Prorate)
						{
							decimal previousValue = PXCurrencyAttribute.BaseRound(this, (oldShipmentQty / order.OrderQty) * orderFreightCost ?? 0m);
							decimal newValue = PXCurrencyAttribute.BaseRound(this, (newShipmentQty / order.OrderQty) * orderFreightCost ?? 0m);

							shipmentFreightCost += -previousValue + newValue;

							shipmentCopy.CuryFreightCost = Math.Max((decimal)shipmentFreightCost, 0m);
							if (Document.Cache.GetStatus(shipment).IsNotIn(PXEntryStatus.Deleted, PXEntryStatus.InsertedDeleted))
								Document.Update(shipmentCopy);
						}
						else if (newOrderSelected)
						{
							shipmentFreightCost += order.FreightCost;

							shipmentCopy.CuryFreightCost = Math.Max((decimal)shipmentFreightCost, 0m);
							Document.Update(shipmentCopy);
						}
					}
				}
			}
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

		protected virtual bool AnySelected(PXCache cache)
			where TSelectedField : IBqlField
		{
			return cache.Cached.Cast().Any(
				p => (bool?)cache.GetValue(p) == true &&
				cache.GetStatus(p).IsNotIn(PXEntryStatus.Deleted, PXEntryStatus.InsertedDeleted));
		}

		protected virtual void ValidateLineType(SOLine line, InventoryItem item, string message)
		{
			if (item.KitItem == true && item.StkItem != true && line.LineType == SOLineType.NonInventory)
			{
				throw new PXException(message, line.LineNbr, line.OrderNbr);
			}
		}

		protected virtual void MarkConfirmed(SOShipment shipment)
		{
			shipment.Confirmed = true;
			shipment.ConfirmedToVerify = false;
			shipment.Status = SOShipmentStatus.Confirmed;
		}

		protected virtual void MarkOpen(SOShipment shipment)
		{
			shipment.Confirmed = false;
			shipment.ConfirmedToVerify = true;
			shipment.Status = SOShipmentStatus.Open;

			shipment.LabelsPrinted = false;
			shipment.CommercialInvoicesPrinted = false;
		}

		protected virtual void MarkCompleted(SOShipment shipment)
		{
			shipment.Status = SOShipmentStatus.Completed;
		}

		[PXInternalUseOnly]
		protected virtual void SetSuppressWorkflowOnConfirmShipment()
			=> PXTransactionScope.SetSuppressWorkflow(true);

		[PXInternalUseOnly]
		protected virtual void SetSuppressWorkflowOnCorrectShipment()
			=> PXTransactionScope.SetSuppressWorkflow(true);

		[PXInternalUseOnly]
		protected virtual void SetSuppressWorkflowOnUpdateIN()
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

		private class ShipmentSchedule : IComparable
		{
			private int sortOrder;
			private int soLineNbr;
			private int splitLineNbr;

			public ShipmentSchedule(PXResult result, SOShipLine shipLine)
			{
				this.sortOrder = ((SOLine)result).SortOrder.GetValueOrDefault(1000);
				this.soLineNbr = ((SOLine)result).LineNbr.GetValueOrDefault(int.MaxValue);
				this.splitLineNbr = ((SOLineSplit)result).SplitLineNbr.GetValueOrDefault(int.MaxValue);
				this.Result = result;
				this.ShipLine = shipLine;
			}

			public PXResult Result { get; private set; }
			public SOShipLine ShipLine;

			public int CompareTo(ShipmentSchedule other)
			{
				int compareResult = sortOrder.CompareTo(other.sortOrder);
				if (compareResult == 0)
				{
					compareResult = soLineNbr.CompareTo(other.soLineNbr);
					if(compareResult == 0)
					{
						compareResult = splitLineNbr.CompareTo(other.splitLineNbr);
					}
				}

				return compareResult;
			}
		}

		public class OrigSOLineSplitSet : HashSet
		{
			public class SplitComparer : IEqualityComparer
			{
				public bool Equals(SOShipLine a, SOShipLine b)
				{
					return a.OrigOrderType == b.OrigOrderType && a.OrigOrderNbr == b.OrigOrderNbr
						&& a.OrigLineNbr == b.OrigLineNbr && a.OrigSplitLineNbr == b.OrigSplitLineNbr;
				}

				public int GetHashCode(SOShipLine a)
				{
					unchecked
					{
						int hash = 17;
						hash = hash * 23 + a.OrigOrderType?.GetHashCode() ?? 0;
						hash = hash * 23 + a.OrigOrderNbr?.GetHashCode() ?? 0;
						hash = hash * 23 + a.OrigLineNbr.GetHashCode();
						hash = hash * 23 + a.OrigSplitLineNbr.GetHashCode();
						return hash;
					}
				}
			}

			private SOShipLine _shipLine = new SOShipLine();

			public OrigSOLineSplitSet()
				: base(new SplitComparer())
			{
			}

			public bool Contains(SOLineSplit sls)
			{
				_shipLine.OrigOrderType = sls.OrderType;
				_shipLine.OrigOrderNbr = sls.OrderNbr;
				_shipLine.OrigLineNbr = sls.LineNbr;
				_shipLine.OrigSplitLineNbr = sls.SplitLineNbr;
				return this.Contains(_shipLine);
			}
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

			[PXOverride]
			public virtual void ShipPackages(SOShipment shiporder, Action baseMethod)
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

				baseMethod(shiporder);
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
		public class CarrierRates : PX.Objects.SO.GraphExtensions.CarrierRates.CarrierRatesExtension
		{
			protected override DocumentMapping GetDocumentMapping() => new DocumentMapping(typeof(SOShipment)) { DocumentDate = typeof(SOShipment.shipDate) };
			protected override DocumentPackageMapping GetDocumentPackageMapping() => new DocumentPackageMapping(typeof(SOPackageDetailEx)) { };

			protected override void CalculateFreightCost(Document doc)
			{
				Base.CalculateFreightCost(true);
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
									Base.soorder.Cache, order, box.DeclaredValue ?? 0m, out baseCuryDeclaredValue);

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
					IEnumerable sOOrderShipments = Base.OrderListSimple.Select().RowCast();

					//if the freight amount is based on Sales Order and the shipment has multiple SO or if it is a partial shipment, then add attribute "SKIPFREIGHTCHARGE"
					//don't send the overriden freight price field in this case. Otherwise send FreightAmt + PremiumFreightAmt from SO
					if (sOOrderShipments.Count() == 1)
					{
						SOOrderShipment soOrderShipment = sOOrderShipments.FirstOrDefault();
						SOOrder order = Base.soorder.Select(soOrderShipment?.OrderType, soOrderShipment?.OrderNbr);

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

				var soOrderList = Base.OrderList.Select();
				SOAddress billToAddress = soOrderList.RowCast().FirstOrDefault();
				SOContact billToContact = soOrderList.RowCast().FirstOrDefault();

				if (soOrderList.Count() == 1)
				{
					cr.BillToAddress = billToAddress;
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

			[PXOverride]
			public virtual void Persist(Action baseMtd)
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

				baseMtd();
			}

			protected override IEnumerable GetApplicableCarrierPlugins()
			{
				return PXSelectReadonly>>>>
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

	public class SOShipmentException : PXException
	{
		public ErrorCode Code { get; private set; }
		public SOOrderShipment Item { get; private set; }

		public SOShipmentException(ErrorCode code, SOOrderShipment item, string message, params object[] args)
			: base(message, args)
		{
			this.Code = code;
			this.Item = item;
		}

		public SOShipmentException(string message, params object[] args)
			: base(message, args)
		{
		}

		public SOShipmentException(string message)
			: base(message)
		{
		}


		public SOShipmentException(SerializationInfo info, StreamingContext context)
				: base(info, context)
		{
		}

		public enum ErrorCode
		{
			None,
			CannotShipTraced,
			NotAllocatedLines,
			CannotShipCompleteTraced,
			NothingToShipTraced,
			NothingToReceiveTraced
		}

	}

	[PXProjection(typeof(Select2,
		InnerJoin>,
		InnerJoin>>>,
	Where,
	  And,
		And,
		And,
		And,
		And>>>>>>>>>))]
	[Serializable]
	public partial class SOShipmentPlan : PXBqlTable, IBqlTable
	{
		#region Selected
		public abstract class selected : PX.Data.BQL.BqlBool.Field { }
		protected bool? _Selected = false;
		[PXBool()]
		[PXDefault(false)]
		[PXUIField(DisplayName = "Selected")]
		public virtual bool? Selected
		{
			get
			{
				return _Selected;
			}
			set
			{
				_Selected = value;
			}
		}
		#endregion
		#region OrderType
		public abstract class orderType : PX.Data.BQL.BqlString.Field { }
		protected String _OrderType;
		[PXDBString(2, IsKey = true, IsFixed = true, InputMask = ">LL", BqlField = typeof(SOOrder.orderType))]
		public virtual String OrderType
		{
			get
			{
				return this._OrderType;
			}
			set
			{
				this._OrderType = value;
			}
		}
		#endregion
		#region OrderNbr
		public abstract class orderNbr : PX.Data.BQL.BqlString.Field { }
		protected String _OrderNbr;
		[PXDBString(15, IsUnicode = true, IsKey = true, InputMask = "", BqlField = typeof(SOOrder.orderNbr))]
		public virtual String OrderNbr
		{
			get
			{
				return this._OrderNbr;
			}
			set
			{
				this._OrderNbr = value;
			}
		}
		#endregion
		#region DestinationSiteID
		public abstract class destinationSiteID : PX.Data.BQL.BqlInt.Field { }
		protected Int32? _DestinationSiteID;
		[PXDefault()]
		[IN.ToSite(DisplayName = "Destination Warehouse", DescriptionField = typeof(INSite.descr), BqlField = typeof(SOOrder.destinationSiteID))]
		public virtual Int32? DestinationSiteID
		{
			get
			{
				return this._DestinationSiteID;
			}
			set
			{
				this._DestinationSiteID = value;
			}
		}
		#endregion
		#region InventoryID
		public abstract class inventoryID : PX.Data.BQL.BqlInt.Field { }
		protected Int32? _InventoryID;
		[PXDBInt(BqlField = typeof(INItemPlan.inventoryID))]
		public virtual Int32? InventoryID
		{
			get
			{
				return this._InventoryID;
			}
			set
			{
				this._InventoryID = value;
			}
		}
		#endregion
		#region SubItemID
		public abstract class subItemID : PX.Data.BQL.BqlInt.Field { }
		protected Int32? _SubItemID;
		[PXDBInt(BqlField = typeof(INItemPlan.subItemID))]
		public virtual Int32? SubItemID
		{
			get
			{
				return this._SubItemID;
			}
			set
			{
				this._SubItemID = value;
			}
		}
		#endregion
		#region SiteID
		public abstract class siteID : PX.Data.BQL.BqlInt.Field { }
		protected Int32? _SiteID;
		[PXDBInt(BqlField = typeof(INItemPlan.siteID))]
		[PXSelector(typeof(Search), CacheGlobal = true, SubstituteKey = typeof(INSite.siteCD))]
		public virtual Int32? SiteID
		{
			get
			{
				return this._SiteID;
			}
			set
			{
				this._SiteID = value;
			}
		}
		#endregion
		#region LotSerialNbr
		public abstract class lotSerialNbr : PX.Data.BQL.BqlString.Field { }
		[PXDBString(100, IsUnicode = true, BqlField = typeof(INItemPlan.lotSerialNbr))]
		public virtual String LotSerialNbr
		{
			get;
			set;
		}
		#endregion
		#region PlanType
		public abstract class planType : PX.Data.BQL.BqlString.Field { }
		[PXDBString(2, IsFixed = true, BqlField = typeof(INItemPlan.planType))]
		public virtual String PlanType
		{
			get;
			set;
		}
		#endregion
		#region PlanDate
		public abstract class planDate : PX.Data.BQL.BqlDateTime.Field { }
		protected DateTime? _PlanDate;
		[PXDBDate(BqlField = typeof(INItemPlan.planDate))]
		[PXUIField(DisplayName = "Sched. Ship. Date")]
		public virtual DateTime? PlanDate
		{
			get
			{
				return this._PlanDate;
			}
			set
			{
				this._PlanDate = value;
			}
		}
		#endregion
		#region PlanID
		public abstract class planID : PX.Data.BQL.BqlLong.Field { }
		protected Int64? _PlanID;
		[PXDBLong(IsKey = true, BqlField = typeof(INItemPlan.planID))]
		public virtual Int64? PlanID
		{
			get
			{
				return this._PlanID;
			}
			set
			{
				this._PlanID = value;
			}
		}
		#endregion
		#region DemandPlanID
		public abstract class demandPlanID : PX.Data.BQL.BqlLong.Field { }
		[PXDBLong(BqlField = typeof(INItemPlan.demandPlanID))]
		public virtual Int64? DemandPlanID
		{
			get;
			set;
		}
		#endregion
		#region PlanQty
		public abstract class planQty : PX.Data.BQL.BqlDecimal.Field { }
		protected Decimal? _PlanQty;
		[PXDBDecimal(6, BqlField = typeof(INItemPlan.planQty))]
		public virtual Decimal? PlanQty
		{
			get
			{
				return this._PlanQty;
			}
			set
			{
				this._PlanQty = value;
			}
		}
		#endregion
		#region Reverse
		public abstract class reverse : PX.Data.BQL.BqlBool.Field { }
		protected Boolean? _Reverse;
		[PXDBBool(BqlField = typeof(INItemPlan.reverse))]
		public virtual Boolean? Reverse
		{
			get
			{
				return this._Reverse;
			}
			set
			{
				this._Reverse = value;
			}
		}
		#endregion
		#region InclQtySOBackOrdered
		public abstract class inclQtySOBackOrdered : PX.Data.BQL.BqlShort.Field { }
		protected Int16? _InclQtySOBackOrdered;
		[PXDBShort(BqlField = typeof(INPlanType.inclQtySOBackOrdered))]
		public virtual Int16? InclQtySOBackOrdered
		{
			get
			{
				return this._InclQtySOBackOrdered;
			}
			set
			{
				this._InclQtySOBackOrdered = value;
			}
		}
		#endregion
		#region InclQtySOShipping
		public abstract class inclQtySOShipping : PX.Data.BQL.BqlShort.Field { }
		protected Int16? _InclQtySOShipping;
		[PXDBShort(BqlField = typeof(INPlanType.inclQtySOShipping))]
		public virtual Int16? InclQtySOShipping
		{
			get
			{
				return this._InclQtySOShipping;
			}
			set
			{
				this._InclQtySOShipping = value;
			}
		}
		#endregion
		#region InclQtySOShipped
		public abstract class inclQtySOShipped : PX.Data.BQL.BqlShort.Field { }
		protected Int16? _InclQtySOShipped;
		[PXDBShort(BqlField = typeof(INPlanType.inclQtySOShipped))]
		public virtual Int16? InclQtySOShipped
		{
			get
			{
				return this._InclQtySOShipped;
			}
			set
			{
				this._InclQtySOShipped = value;
			}
		}
		#endregion
		#region RequireAllocation
		public abstract class requireAllocation : PX.Data.BQL.BqlBool.Field { }
		protected Boolean? _RequireAllocation;
		[PXDBBool(BqlField = typeof(SOOrderType.requireAllocation))]
		public virtual Boolean? RequireAllocation
		{
			get
			{
				return this._RequireAllocation;
			}
			set
			{
				this._RequireAllocation = value;
			}
		}
		#endregion
		#region IsManualPackage
		public abstract class isManualPackage : PX.Data.BQL.BqlBool.Field { }

		[PXDBBool(BqlField = typeof(SOOrder.isManualPackage))]
		public virtual bool? IsManualPackage
		{
			get;
			set;
		}
		#endregion
	}


	[PXProjection(typeof(Select2,
		InnerJoin>>,
		Where>>), new Type[] { typeof(SOLine) })]
	[Serializable]
	public partial class SOLine2 : PXBqlTable, IBqlTable, ISortOrder
	{
		#region OrderType
		public abstract class orderType : PX.Data.BQL.BqlString.Field { }
		protected string _OrderType;
		[PXDBString(2, IsKey = true, IsFixed = true, BqlField = typeof(SOLine.orderType))]
		public virtual String OrderType
		{
			get
			{
				return this._OrderType;
			}
			set
			{
				this._OrderType = value;
			}
		}
		#endregion
		#region Behavior
		public abstract class behavior : PX.Data.BQL.BqlString.Field { }
		protected String _Behavior;
		/// 
		[PXDBString(2, IsFixed = true, InputMask = ">aa",BqlField = typeof(SOLine.behavior))]
		public virtual String Behavior
		{
			get
			{
				return this._Behavior;
			}
			set
			{
				this._Behavior = value;
			}
		}
		#endregion
		#region OrderNbr
		public abstract class orderNbr : PX.Data.BQL.BqlString.Field { }
		protected string _OrderNbr;
		[PXDBString(15, IsUnicode = true, IsKey = true, InputMask = "", BqlField = typeof(SOLine.orderNbr))]
		[PXParent(typeof(Select>, And>>>>))]
		public virtual String OrderNbr
		{
			get
			{
				return this._OrderNbr;
			}
			set
			{
				this._OrderNbr = value;
			}
		}
		#endregion
		#region LineNbr
		public abstract class lineNbr : PX.Data.BQL.BqlInt.Field { }
		protected Int32? _LineNbr;
		[PXDBInt(IsKey = true, BqlField = typeof(SOLine.lineNbr))]
		public virtual Int32? LineNbr
		{
			get
			{
				return this._LineNbr;
			}
			set
			{
				this._LineNbr = value;
			}
		}
		#endregion
		#region SortOrder
		public abstract class sortOrder : PX.Data.BQL.BqlInt.Field { }
		protected Int32? _SortOrder;
		[PXDBInt(BqlField = typeof(SOLine.sortOrder))]
		public virtual Int32? SortOrder
		{
			get
			{
				return this._SortOrder;
			}
			set
			{
				this._SortOrder = value;
			}
		}
		#endregion
		#region LineType
		public abstract class lineType : PX.Data.BQL.BqlString.Field { }
		protected String _LineType;
		[PXDBString(2, IsFixed = true, BqlField = typeof(SOLine.lineType))]
		public virtual String LineType
		{
			get
			{
				return this._LineType;
			}
			set
			{
				this._LineType = value;
			}
		}
		#endregion
		#region Operation
		public abstract class operation : PX.Data.BQL.BqlString.Field { }
		protected String _Operation;
		[PXDBString(1, IsFixed = true, InputMask = ">a", BqlField = typeof(SOLine.operation))]
		[PXUIField(DisplayName = "Operation")]
		[SOOperation.List]
		public virtual String Operation
		{
			get
			{
				return this._Operation;
			}
			set
			{
				this._Operation = value;
			}
		}
		#endregion
		#region LineSign
		public abstract class lineSign : BqlShort.Field { }
		[PXDBShort(BqlField = typeof(SOLine.lineSign))]
		[PXDefault]
		public virtual short? LineSign
		{
			get;
			set;
		}
		#endregion
		#region ShipComplete
		public abstract class shipComplete : PX.Data.BQL.BqlString.Field { }
		protected String _ShipComplete;
		[PXDBString(1, IsFixed = true, BqlField = typeof(SOLine.shipComplete))]
		public virtual String ShipComplete
		{
			get
			{
				return this._ShipComplete;
			}
			set
			{
				this._ShipComplete = value;
			}
		}
		#endregion
		#region Completed
		public abstract class completed : PX.Data.BQL.BqlBool.Field { }
		protected Boolean? _Completed;
		[PXDBBool(BqlField = typeof(SOLine.completed))]
		public virtual Boolean? Completed
		{
			get
			{
				return this._Completed;
			}
			set
			{
				this._Completed = value;
			}
		}
		#endregion
		#region InventoryID
		public abstract class inventoryID : PX.Data.BQL.BqlInt.Field { }
		protected Int32? _InventoryID;
		[PXDBInt(BqlField = typeof(SOLine.inventoryID))]
		public virtual Int32? InventoryID
		{
			get
			{
				return this._InventoryID;
			}
			set
			{
				this._InventoryID = value;
			}
		}
		#endregion
		#region SubItemID
		public abstract class subItemID : PX.Data.BQL.BqlInt.Field { }
		protected Int32? _SubItemID;
		[PXDBInt(BqlField = typeof(SOLine.subItemID))]
		public virtual Int32? SubItemID
		{
			get
			{
				return this._SubItemID;
			}
			set
			{
				this._SubItemID = value;
			}
		}
		#endregion
		#region SiteID
		public abstract class siteID : PX.Data.BQL.BqlInt.Field { }
		protected Int32? _SiteID;
		[PXDBInt(BqlField = typeof(SOLine.siteID))]
		public virtual Int32? SiteID
		{
			get
			{
				return this._SiteID;
			}
			set
			{
				this._SiteID = value;
			}
		}
		#endregion
		#region SalesAcctID
		public abstract class salesAcctID : PX.Data.BQL.BqlInt.Field { }
		protected Int32? _SalesAcctID;
		[PXDBInt(BqlField = typeof(SOLine.salesAcctID))]
		public virtual Int32? SalesAcctID
		{
			get
			{
				return this._SalesAcctID;
			}
			set
			{
				this._SalesAcctID = value;
			}
		}
		#endregion
		#region SalesSubID
		public abstract class salesSubID : PX.Data.BQL.BqlInt.Field { }
		protected Int32? _SalesSubID;
		[PXDBInt(BqlField = typeof(SOLine.salesSubID))]
		public virtual Int32? SalesSubID
		{
			get
			{
				return this._SalesSubID;
			}
			set
			{
				this._SalesSubID = value;
			}
		}
		#endregion
		#region TranDesc
		public abstract class tranDesc : PX.Data.BQL.BqlString.Field { }
		protected String _TranDesc;
		[PXDBString(256, IsUnicode = true, BqlField = typeof(SOLine.tranDesc))]
		public virtual String TranDesc
		{
			get
			{
				return this._TranDesc;
			}
			set
			{
				this._TranDesc = value;
			}
		}
		#endregion
		#region UOM
		public abstract class uOM : PX.Data.BQL.BqlString.Field { }
		protected String _UOM;
		[INUnit(typeof(SOLine2.inventoryID), BqlField = typeof(SOLine.uOM))]
		public virtual String UOM
		{
			get
			{
				return this._UOM;
			}
			set
			{
				this._UOM = value;
			}
		}
		#endregion
		#region OrderQty
		public abstract class orderQty : PX.Data.BQL.BqlDecimal.Field { }
		protected Decimal? _OrderQty;
		[PXDBDecimal(6, BqlField = typeof(SOLine.orderQty))]
		[PXDefault]
		public virtual Decimal? OrderQty
		{
			get
			{
				return this._OrderQty;
			}
			set
			{
				this._OrderQty = value;
			}
		}
		#endregion
		#region BaseOrderQty
		public abstract class baseOrderQty : PX.Data.BQL.BqlDecimal.Field { }
		[PXDBDecimal(6, BqlField = typeof(SOLine.baseOrderQty))]
		[PXDefault]
		public virtual decimal? BaseOrderQty
		{
			get;
			set;
		}
		#endregion
		#region BaseShippedQty
		public abstract class baseShippedQty : PX.Data.BQL.BqlDecimal.Field { }
		protected Decimal? _BaseShippedQty;
		[PXDBBaseQtyWithOrigQty(typeof(uOM), typeof(shippedQty), typeof(uOM), typeof(baseOrderQty), typeof(orderQty),
			BqlField = typeof(SOLine.baseShippedQty))]
		[PXDefault]
		public virtual Decimal? BaseShippedQty
		{
			get
			{
				return this._BaseShippedQty;
			}
			set
			{
				this._BaseShippedQty = value;
			}
		}
		#endregion
		#region OriginalBaseShippedQty
		public abstract class originalBaseShippedQty : PX.Data.BQL.BqlDecimal.Field { }

		// Acuminator disable once PX1007 NoXmlCommentForPublicEntityOrDacProperty to be documented later
		[PXQuantity]
		[PXDBCalced(typeof(baseShippedQty), typeof(decimal), Persistent = true)]
		public virtual Decimal? OriginalBaseShippedQty
		{
			get;
			set;
		}
		#endregion
		#region ShippedQty
		public abstract class shippedQty : PX.Data.BQL.BqlDecimal.Field { }
		protected Decimal? _ShippedQty;
		[PXDBDecimal(6, BqlField = typeof(SOLine.shippedQty))]
		[PXDefault]
		public virtual Decimal? ShippedQty
		{
			get
			{
				return this._ShippedQty;
			}
			set
			{
				this._ShippedQty = value;
			}
		}
		#endregion
		#region BilledQty
		public abstract class billedQty : PX.Data.BQL.BqlDecimal.Field { }
		protected Decimal? _BilledQty;
		[PXDBDecimal(6, BqlField = typeof(SOLine.billedQty))]
		[PXDefault]
		public virtual Decimal? BilledQty
		{
			get
			{
				return this._BilledQty;
			}
			set
			{
				this._BilledQty = value;
			}
		}
		#endregion
		#region BaseBilledQty
		public abstract class baseBilledQty : PX.Data.BQL.BqlDecimal.Field { }
		protected Decimal? _BaseBilledQty;
		[PXDBBaseQuantity(typeof(SOLine2.uOM), typeof(SOLine2.billedQty), BqlField = typeof(SOLine.baseBilledQty))]
		[PXDefault]
		public virtual Decimal? BaseBilledQty
		{
			get
			{
				return this._BaseBilledQty;
			}
			set
			{
				this._BaseBilledQty = value;
			}
		}
		#endregion
		#region UnbilledQty
		public abstract class unbilledQty : PX.Data.BQL.BqlDecimal.Field { }
		protected Decimal? _UnbilledQty;
		[PXDBQuantity(BqlField = typeof(SOLine.unbilledQty))]
		[PXUnboundFormula(typeof(unbilledQty.Multiply), typeof(SumCalc))]
		[PXDefault]
		public virtual Decimal? UnbilledQty
		{
			get
			{
				return this._UnbilledQty;
			}
			set
			{
				this._UnbilledQty = value;
			}
		}
		#endregion
		#region BaseUnbilledQty
		public abstract class baseUnbilledQty : PX.Data.BQL.BqlDecimal.Field { }
		protected Decimal? _BaseUnbilledQty;
		[PXDBBaseQuantity(typeof(SOLine2.uOM), typeof(SOLine2.unbilledQty), BqlField = typeof(SOLine.baseUnbilledQty))]
		[PXDefault]
		public virtual Decimal? BaseUnbilledQty
		{
			get
			{
				return this._BaseUnbilledQty;
			}
			set
			{
				this._BaseUnbilledQty = value;
			}
		}
		#endregion
		#region OpenQty
		public abstract class openQty : PX.Data.BQL.BqlDecimal.Field { }
		protected Decimal? _OpenQty;
		[PXDBDecimal(6, MinValue = 0, BqlField = typeof(SOLine.openQty))]
		[PXDefault]
		public virtual Decimal? OpenQty
		{
			get
			{
				return this._OpenQty;
			}
			set
			{
				this._OpenQty = value;
			}
		}
		#endregion
		#region BaseOpenQty
		public abstract class baseOpenQty : PX.Data.BQL.BqlDecimal.Field { }
		protected Decimal? _BaseOpenQty;
		[PXDBDecimal(6, MinValue = 0, BqlField = typeof(SOLine.baseOpenQty))]
		[PXDefault(TypeCode.Decimal, "0.0")]
		[PXUIField(DisplayName = "Base Open Qty.")]
		public virtual Decimal? BaseOpenQty
		{
			get
			{
				return this._BaseOpenQty;
			}
			set
			{
				this._BaseOpenQty = value;
			}
		}
		#endregion
		#region CompleteQtyMin
		public abstract class completeQtyMin : PX.Data.BQL.BqlDecimal.Field { }
		protected Decimal? _CompleteQtyMin;
		[PXDBDecimal(2, MinValue = 0.0, MaxValue = 99.0, BqlField = typeof(SOLine.completeQtyMin))]
		[PXDefault(TypeCode.Decimal, "0.0")]
		public virtual Decimal? CompleteQtyMin
		{
			get
			{
				return this._CompleteQtyMin;
			}
			set
			{
				this._CompleteQtyMin = value;
			}
		}
		#endregion
		#region CompleteQtyMax
		public abstract class completeQtyMax : PX.Data.BQL.BqlDecimal.Field { }
		protected Decimal? _CompleteQtyMax;
		[PXDBDecimal(2, MinValue = 100.0, MaxValue = 999.0, BqlField = typeof(SOLine.completeQtyMax))]
		[PXDefault(TypeCode.Decimal, "100.0")]
		public virtual Decimal? CompleteQtyMax
		{
			get
			{
				return this._CompleteQtyMax;
			}
			set
			{
				this._CompleteQtyMax = value;
			}
		}
		#endregion
		#region ShipDate
		public abstract class shipDate : PX.Data.BQL.BqlDateTime.Field { }
		protected DateTime? _ShipDate;
		[PXDBDate(BqlField = typeof(SOLine.shipDate))]
		public virtual DateTime? ShipDate
		{
			get
			{
				return this._ShipDate;
			}
			set
			{
				this._ShipDate = value;
			}
		}
		#endregion
		#region CuryInfoID
		public abstract class curyInfoID : PX.Data.BQL.BqlLong.Field { }
		protected Int64? _CuryInfoID;
		[PXDBLong(BqlField = typeof(SOLine.curyInfoID))]
		[CurrencyInfo()]
		public virtual Int64? CuryInfoID
		{
			get
			{
				return this._CuryInfoID;
			}
			set
			{
				this._CuryInfoID = value;
			}
		}
		#endregion
		#region CuryUnitPrice
		public abstract class curyUnitPrice : PX.Data.BQL.BqlDecimal.Field { }
		protected Decimal? _CuryUnitPrice;
		[PXDBDecimal(6, BqlField = typeof(SOLine.curyUnitPrice))]
		[PXDefault]
		public virtual Decimal? CuryUnitPrice
		{
			get
			{
				return this._CuryUnitPrice;
			}
			set
			{
				this._CuryUnitPrice = value;
			}
		}
		#endregion
		#region ActualUnitPrice
		public abstract class actualUnitPrice : PX.Data.BQL.BqlDecimal.Field
        {
		}
		[PXDBPriceCostCalced(typeof(
			Switch>, SOLine.unitPrice>,
				Div>),
			typeof(decimal),
			CastToScale = 9, CastToPrecision = 25)]
		public virtual decimal? ActualUnitPrice
		{
			get;
			set;
		}
		#endregion
		#region UnitCost
		public abstract class unitCost : PX.Data.BQL.BqlDecimal.Field
        {
		}
		[PXDBDecimal(6, BqlField = typeof(SOLine.unitCost))]
		[PXDefault]
		public virtual decimal? UnitCost
		{
			get;
			set;
		}
		#endregion
		#region DiscPct
		public abstract class discPct : PX.Data.BQL.BqlDecimal.Field { }
		protected Decimal? _DiscPct;
		[PXDBDecimal(6, BqlField = typeof(SOLine.discPct))]
		[PXDefault]
		public virtual Decimal? DiscPct
		{
			get
			{
				return this._DiscPct;
			}
			set
			{
				this._DiscPct = value;
			}
		}
		#endregion
		#region CuryBilledAmt
		public abstract class curyBilledAmt : PX.Data.BQL.BqlDecimal.Field { }
		protected Decimal? _CuryBilledAmt;
		[PXFormula(typeof(Mult, Sub>>))]
		[PXDBCurrency(typeof(SOLine2.curyInfoID), typeof(SOLine2.billedAmt), BqlField = typeof(SOLine.curyBilledAmt))]
		[PXDefault(TypeCode.Decimal, "0.0")]
		public virtual Decimal? CuryBilledAmt
		{
			get
			{
				return this._CuryBilledAmt;
			}
			set
			{
				this._CuryBilledAmt = value;
			}
		}
		#endregion
		#region BilledAmt
		public abstract class billedAmt : PX.Data.BQL.BqlDecimal.Field { }
		protected Decimal? _BilledAmt;
		[PXDBDecimal(4, BqlField = typeof(SOLine.billedAmt))]
		[PXDefault(TypeCode.Decimal, "0.0")]
		public virtual Decimal? BilledAmt
		{
			get
			{
				return this._BilledAmt;
			}
			set
			{
				this._BilledAmt = value;
			}
		}
		#endregion
		#region CuryOpenAmt
		public abstract class curyOpenAmt : PX.Data.BQL.BqlDecimal.Field { }
		protected Decimal? _CuryOpenAmt;
		[PXDBCurrency(typeof(SOLine2.curyInfoID), typeof(SOLine2.openAmt), BqlField = typeof(SOLine.curyOpenAmt))]
		[PXUIField(DisplayName = "Open Amount")]
		[PXDefault]
		public virtual Decimal? CuryOpenAmt
		{
			get
			{
				return this._CuryOpenAmt;
			}
			set
			{
				this._CuryOpenAmt = value;
			}
		}
		#endregion
		#region OpenAmt
		public abstract class openAmt : PX.Data.BQL.BqlDecimal.Field { }
		protected Decimal? _OpenAmt;
		[PXDBDecimal(4, BqlField = typeof(SOLine.openAmt))]
		[PXDefault]
		public virtual Decimal? OpenAmt
		{
			get
			{
				return this._OpenAmt;
			}
			set
			{
				this._OpenAmt = value;
			}
		}
		#endregion
		#region CuryUnbilledAmt
		public abstract class curyUnbilledAmt : PX.Data.BQL.BqlDecimal.Field { }
		protected Decimal? _CuryUnbilledAmt;
		[PXDBCurrency(typeof(SOLine2.curyInfoID), typeof(SOLine2.unbilledAmt), BqlField = typeof(SOLine.curyUnbilledAmt))]
		[PXFormula(typeof(Mult, Sub>>))]
		[PXDefault]
		public virtual Decimal? CuryUnbilledAmt
		{
			get
			{
				return this._CuryUnbilledAmt;
			}
			set
			{
				this._CuryUnbilledAmt = value;
			}
		}
		#endregion
		#region UnbilledAmt
		public abstract class unbilledAmt : PX.Data.BQL.BqlDecimal.Field { }
		protected Decimal? _UnbilledAmt;
		[PXDBDecimal(4, BqlField = typeof(SOLine.unbilledAmt))]
		[PXDefault]
		public virtual Decimal? UnbilledAmt
		{
			get
			{
				return this._UnbilledAmt;
			}
			set
			{
				this._UnbilledAmt = value;
			}
		}
		#endregion
		#region GroupDiscountRate
		public abstract class groupDiscountRate : PX.Data.BQL.BqlDecimal.Field { }
		protected Decimal? _GroupDiscountRate;
		[PXDBDecimal(18, BqlField = typeof(SOLine.groupDiscountRate))]
		[PXDefault(TypeCode.Decimal, "1.0")]
		public virtual Decimal? GroupDiscountRate
		{
			get
			{
				return this._GroupDiscountRate;
			}
			set
			{
				this._GroupDiscountRate = value;
			}
		}
		#endregion
		#region DocumentDiscountRate
		public abstract class documentDiscountRate : PX.Data.BQL.BqlDecimal.Field { }
		protected Decimal? _DocumentDiscountRate;
		[PXDBDecimal(18, BqlField = typeof(SOLine.documentDiscountRate))]
		[PXDefault(TypeCode.Decimal, "1.0")]
		public virtual Decimal? DocumentDiscountRate
		{
			get
			{
				return this._DocumentDiscountRate;
			}
			set
			{
				this._DocumentDiscountRate = value;
			}
		}
		#endregion
		#region DisableAutomaticTaxCalculation
		public abstract class disableAutomaticTaxCalculation : PX.Data.BQL.BqlBool.Field { }
		[PXDBBool(BqlField = typeof(SOLine.disableAutomaticTaxCalculation))]
		public virtual Boolean? DisableAutomaticTaxCalculation
		{
			get;
			set;
		}
		#endregion
		#region TaxZoneID
		public abstract class taxZoneID : PX.Data.BQL.BqlString.Field { }

		[PXDBString(10, IsUnicode = true, BqlField = typeof(SOLine.taxZoneID))]
		public virtual String TaxZoneID
		{
			get;
			set;
		}
		#endregion
		#region TaxCategoryID
		public abstract class taxCategoryID : PX.Data.BQL.BqlString.Field { }
		protected String _TaxCategoryID;
		[PXDBString(TX.TaxCategory.taxCategoryID.Length, IsUnicode = true, BqlField = typeof(SOLine.taxCategoryID))]
		[SOUnbilledTax2(typeof(SOOrder), typeof(SOTax), typeof(SOTaxTran),
			   //Per Unit Tax settings
			   Inventory = typeof(SOLine2.inventoryID), UOM = typeof(SOLine2.uOM), LineQty = typeof(SOLine2.unbilledQty))]
		[SOOpenTax2(typeof(SOOrder), typeof(SOTax), typeof(SOTaxTran),
			   //Per Unit Tax settings
			   Inventory = typeof(SOLine2.inventoryID), UOM = typeof(SOLine2.uOM), LineQty = typeof(SOLine2.openQty))]
		public virtual String TaxCategoryID
		{
			get
			{
				return this._TaxCategoryID;
			}
			set
			{
				this._TaxCategoryID = value;
			}
		}
		#endregion
		#region PlanType
		public abstract class planType : PX.Data.BQL.BqlString.Field { }
		protected String _PlanType;
		[PXDBString(2, IsFixed = true, BqlField = typeof(SOOrderTypeOperation.orderPlanType))]
		public virtual String PlanType
		{
			get
			{
				return this._PlanType;
			}
			set
			{
				this._PlanType = value;
			}
		}
		#endregion
		#region POSource
		public abstract class pOSource : PX.Data.BQL.BqlString.Field { }
		protected string _POSource;
		[PXDBString(BqlField = typeof(SOLine.pOSource))]
		public virtual string POSource
		{
			get
			{
				return this._POSource;
			}
			set
			{
				this._POSource = value;
			}
		}
		#endregion
		#region CuryDiscAmt
		public abstract class curyDiscAmt : PX.Data.BQL.BqlDecimal.Field { }

		/// 
		[PXDBCurrency(typeof(SOLine2.curyInfoID), typeof(SOLine2.discAmt), BqlField = typeof(SOLine.curyDiscAmt))]
		[PXUIField(DisplayName = "Discount Amount")]
		[PXDefault(TypeCode.Decimal, "0.0")]
		public virtual Decimal? CuryDiscAmt
		{
			get;
			set;
		}
		#endregion
		#region DiscAmt
		public abstract class discAmt : PX.Data.BQL.BqlDecimal.Field { }
		/// 
		[PXDBDecimal(4, BqlField = typeof(SOLine.discAmt))]
		[PXDefault(TypeCode.Decimal, "0.0")]
		public virtual Decimal? DiscAmt
		{
			get;
			set;
		}
		#endregion
		#region CuryExtPrice
		public abstract class curyExtPrice : PX.Data.BQL.BqlDecimal.Field { }
		protected Decimal? _CuryExtPrice;

		/// 
		[PXDBCurrency(typeof(SOLine2.curyInfoID), typeof(SOLine2.extPrice), BqlField = typeof(SOLine.curyExtPrice))]
		[PXUIField(DisplayName = "Ext. Price")]
		[PXFormula(typeof(Mult))]
		[PXDefault(TypeCode.Decimal, "0.0")]
		public virtual Decimal? CuryExtPrice
		{
			get;
			set;
		}
		#endregion
		#region ExtPrice
		public abstract class extPrice : PX.Data.BQL.BqlDecimal.Field { }

		/// 
		[PXDBDecimal(4, BqlField = typeof(SOLine.extPrice))]
		[PXDefault(TypeCode.Decimal, "0.0")]
		public virtual Decimal? ExtPrice
		{
			get;
			set;
		}
		#endregion

		#region LastModifiedByID
		public abstract class lastModifiedByID : PX.Data.BQL.BqlGuid.Field { }

		[PXDBLastModifiedByID(BqlField = typeof(SOLine.lastModifiedByID))]
		public virtual Guid? LastModifiedByID { get; set; }
		#endregion
		#region LastModifiedByScreenID
		public abstract class lastModifiedByScreenID : PX.Data.BQL.BqlString.Field { }

		[PXDBLastModifiedByScreenID(BqlField = typeof(SOLine.lastModifiedByScreenID))]
		public virtual string LastModifiedByScreenID { get; set; }
		#endregion
		#region LastModifiedDateTime
		public abstract class lastModifiedDateTime : PX.Data.BQL.BqlDateTime.Field { }

		[PXDBLastModifiedDateTime(BqlField = typeof(SOLine.lastModifiedDateTime))]
		public virtual DateTime? LastModifiedDateTime { get; set; }
		#endregion
		#region tstamp
		public abstract class Tstamp : PX.Data.BQL.BqlByteArray.Field { }

		[PXDBTimestamp(BqlField = typeof(SOLine.Tstamp), VerifyTimestamp = VerifyTimestampOptions.BothFromGraphAndRecord)]
		public virtual byte[] tstamp { get; set; }
		#endregion
	}

	[PXProjection(typeof(Select2,
		InnerJoin>>>), new Type[] { typeof(SOLineSplit) })]
	[Serializable]
	public partial class SOLineSplit2 : PXBqlTable, IBqlTable
	{
		#region OrderType
		public abstract class orderType : PX.Data.BQL.BqlString.Field { }
		protected string _OrderType;
		[PXDBString(2, IsKey = true, IsFixed = true, BqlField = typeof(SOLineSplit.orderType))]
		public virtual String OrderType
		{
			get
			{
				return this._OrderType;
			}
			set
			{
				this._OrderType = value;
			}
		}
		#endregion
		#region OrderNbr
		public abstract class orderNbr : PX.Data.BQL.BqlString.Field { }
		protected string _OrderNbr;
		[PXDBString(15, IsUnicode = true, IsKey = true, InputMask = "", BqlField = typeof(SOLineSplit.orderNbr))]
		public virtual String OrderNbr
		{
			get
			{
				return this._OrderNbr;
			}
			set
			{
				this._OrderNbr = value;
			}
		}
		#endregion
		#region LineNbr
		public abstract class lineNbr : PX.Data.BQL.BqlInt.Field { }
		protected Int32? _LineNbr;
		[PXDBInt(IsKey = true, BqlField = typeof(SOLineSplit.lineNbr))]
		public virtual Int32? LineNbr
		{
			get
			{
				return this._LineNbr;
			}
			set
			{
				this._LineNbr = value;
			}
		}
		#endregion
		#region SplitLineNbr
		public abstract class splitLineNbr : PX.Data.BQL.BqlInt.Field { }
		protected Int32? _SplitLineNbr;
		[PXDBInt(IsKey = true, BqlField = typeof(SOLineSplit.splitLineNbr))]
		public virtual Int32? SplitLineNbr
		{
			get
			{
				return this._SplitLineNbr;
			}
			set
			{
				this._SplitLineNbr = value;
			}
		}
		#endregion
		#region Operation
		public abstract class operation : PX.Data.BQL.BqlString.Field { }
		protected String _Operation;
		[PXDBString(1, IsFixed = true, InputMask = ">a", BqlField = typeof(SOLineSplit.operation))]
		[PXUIField(DisplayName = "Operation")]
		[SOOperation.List]
		public virtual String Operation
		{
			get
			{
				return this._Operation;
			}
			set
			{
				this._Operation = value;
			}
		}
		#endregion
		#region Completed
		public abstract class completed : PX.Data.BQL.BqlBool.Field { }
		protected Boolean? _Completed;
		[PXDBBool(BqlField = typeof(SOLineSplit.completed))]
		public virtual Boolean? Completed
		{
			get
			{
				return this._Completed;
			}
			set
			{
				this._Completed = value;
			}
		}
		#endregion
		#region InventoryID
		public abstract class inventoryID : PX.Data.BQL.BqlInt.Field { }
		protected Int32? _InventoryID;
		[PXDBInt(BqlField = typeof(SOLineSplit.inventoryID))]
		public virtual Int32? InventoryID
		{
			get
			{
				return this._InventoryID;
			}
			set
			{
				this._InventoryID = value;
			}
		}
		#endregion
		#region SiteID
		public abstract class siteID : PX.Data.BQL.BqlInt.Field { }
		protected Int32? _SiteID;
		[PXDBInt(BqlField = typeof(SOLineSplit.siteID))]
		public virtual Int32? SiteID
		{
			get
			{
				return this._SiteID;
			}
			set
			{
				this._SiteID = value;
			}
		}
		#endregion
		#region ToSiteID
		public abstract class toSiteID : PX.Data.BQL.BqlInt.Field { }
		protected Int32? _ToSiteID;
		[PXDBInt(BqlField = typeof(SOLineSplit.toSiteID))]
		public virtual Int32? ToSiteID
		{
			get
			{
				return this._ToSiteID;
			}
			set
			{
				this._ToSiteID = value;
			}
		}
		#endregion
		#region CostCenterID
		public abstract class costCenterID : Data.BQL.BqlInt.Field { }

		/// 
		[PXDBInt(BqlField = typeof(SOLineSplit.costCenterID))]
		public virtual int? CostCenterID
		{
			get;
			set;
		}
		#endregion
		#region LotSerialNbr
		public abstract class lotSerialNbr : PX.Data.BQL.BqlString.Field { }
		protected String _LotSerialNbr;
		[PXDBString(100, IsUnicode = true, BqlField = typeof(SOLineSplit.lotSerialNbr))]
		public virtual String LotSerialNbr
		{
			get
			{
				return this._LotSerialNbr;
			}
			set
			{
				this._LotSerialNbr = value;
			}
		}
		#endregion
		#region UOM
		public abstract class uOM : PX.Data.BQL.BqlString.Field { }
		protected String _UOM;
		[INUnit(typeof(SOLineSplit2.inventoryID), BqlField = typeof(SOLineSplit.uOM))]
		public virtual String UOM
		{
			get
			{
				return this._UOM;
			}
			set
			{
				this._UOM = value;
			}
		}
		#endregion
		#region Qty
		public abstract class qty : PX.Data.BQL.BqlDecimal.Field { }
		protected Decimal? _Qty;
		[PXDBDecimal(6, BqlField = typeof(SOLineSplit.qty))]
		[PXDefault]
		public virtual Decimal? Qty
		{
			get
			{
				return this._Qty;
			}
			set
			{
				this._Qty = value;
			}
		}
		#endregion
		#region BaseQty
		public abstract class baseQty : PX.Data.BQL.BqlDecimal.Field { }
		[PXDBDecimal(6, BqlField = typeof(SOLineSplit.baseQty))]
		[PXDefault]
		public virtual Decimal? BaseQty
		{
			get;
			set;
		}
		#endregion
		#region ShippedQty
		public abstract class shippedQty : PX.Data.BQL.BqlDecimal.Field { }
		protected Decimal? _ShippedQty;
		[PXDBDecimal(6, BqlField = typeof(SOLineSplit.shippedQty))]
		[PXDefault]
		public virtual Decimal? ShippedQty
		{
			get
			{
				return this._ShippedQty;
			}
			set
			{
				this._ShippedQty = value;
			}
		}
		#endregion
		#region BaseShippedQty
		public abstract class baseShippedQty : PX.Data.BQL.BqlDecimal.Field { }
		protected Decimal? _BaseShippedQty;
		[PXDBBaseQtyWithOrigQty(typeof(uOM), typeof(shippedQty), typeof(uOM), typeof(baseQty), typeof(qty),
			BqlField = typeof(SOLineSplit.baseShippedQty))]
		[PXDefault]
		public virtual Decimal? BaseShippedQty
		{
			get
			{
				return this._BaseShippedQty;
			}
			set
			{
				this._BaseShippedQty = value;
			}
		}
		#endregion
		#region ShipDate
		public abstract class shipDate : PX.Data.BQL.BqlDateTime.Field { }
		protected DateTime? _ShipDate;
		[PXDBDate(BqlField = typeof(SOLineSplit.shipDate))]
		public virtual DateTime? ShipDate
		{
			get
			{
				return this._ShipDate;
			}
			set
			{
				this._ShipDate = value;
			}
		}
		#endregion
		#region PlanType
		public abstract class planType : PX.Data.BQL.BqlString.Field { }
		protected String _PlanType;
		[PXDBString(2, IsFixed = true, BqlField = typeof(SOOrderTypeOperation.orderPlanType))]
		public virtual String PlanType
		{
			get
			{
				return this._PlanType;
			}
			set
			{
				this._PlanType = value;
			}
		}
		#endregion
		#region POCreate
		public abstract class pOCreate : PX.Data.BQL.BqlBool.Field { }
		protected Boolean? _POCreate;
		[PXDBBool(BqlField = typeof(SOLineSplit.pOCreate))]
		public virtual Boolean? POCreate
		{
			get;
			set;
		}
		#endregion
		#region IsAllocated
		public abstract class isAllocated : PX.Data.BQL.BqlBool.Field { }
		protected Boolean? _IsAllocated;
		[PXDBBool(BqlField = typeof(SOLineSplit.isAllocated))]
		public virtual Boolean? IsAllocated
		{
			get;
			set;
		}
		#endregion
		#region RefNoteID
		public abstract class refNoteID : PX.Data.BQL.BqlGuid.Field { }
		protected Guid? _RefNoteID;
		[PXRefNote(BqlField = typeof(SOLineSplit.refNoteID))]
		public virtual Guid? RefNoteID
		{
			get
			{
				return this._RefNoteID;
			}
			set
			{
				this._RefNoteID = value;
			}
		}
		#endregion
		#region PlanID
		public abstract class planID : PX.Data.BQL.BqlLong.Field { }
		protected Int64? _PlanID;
		[PXDBLong(BqlField = typeof(SOLineSplit.planID), IsImmutable = true)]
		public virtual Int64? PlanID
		{
			get
			{
				return this._PlanID;
			}
			set
			{
				this._PlanID = value;
			}
		}
		#endregion
		#region SOOrderType
		public abstract class sOOrderType : PX.Data.BQL.BqlString.Field { }
		protected String _SOOrderType;
		[PXDBString(2, IsFixed = true, BqlField = typeof(SOLineSplit.sOOrderType))]
		public virtual String SOOrderType
		{
			get
			{
				return this._SOOrderType;
			}
			set
			{
				this._SOOrderType = value;
			}
		}
		#endregion
		#region SOOrderNbr
		public abstract class sOOrderNbr : PX.Data.BQL.BqlString.Field { }
		protected String _SOOrderNbr;
		[PXDBString(15, IsUnicode = true, BqlField = typeof(SOLineSplit.sOOrderNbr))]
		public virtual String SOOrderNbr
		{
			get
			{
				return this._SOOrderNbr;
			}
			set
			{
				this._SOOrderNbr = value;
			}
		}
		#endregion
		#region SOLineNbr
		public abstract class sOLineNbr : PX.Data.BQL.BqlInt.Field { }
		protected Int32? _SOLineNbr;
		[PXDBInt(BqlField = typeof(SOLineSplit.sOLineNbr))]
		public virtual Int32? SOLineNbr
		{
			get
			{
				return this._SOLineNbr;
			}
			set
			{
				this._SOLineNbr = value;
			}
		}
		#endregion
		#region SOSplitLineNbr
		public abstract class sOSplitLineNbr : PX.Data.BQL.BqlInt.Field { }
		protected Int32? _SOSplitLineNbr;
		[PXDBInt(BqlField = typeof(SOLineSplit.sOSplitLineNbr))]
		public virtual Int32? SOSplitLineNbr
		{
			get
			{
				return this._SOSplitLineNbr;
			}
			set
			{
				this._SOSplitLineNbr = value;
			}
		}
		#endregion

		#region LastModifiedByID
		public abstract class lastModifiedByID : PX.Data.BQL.BqlGuid.Field { }

		[PXDBLastModifiedByID(BqlField = typeof(SOLineSplit.lastModifiedByID))]
		public virtual Guid? LastModifiedByID { get; set; }
		#endregion
		#region LastModifiedByScreenID
		public abstract class lastModifiedByScreenID : PX.Data.BQL.BqlString.Field { }

		[PXDBLastModifiedByScreenID(BqlField = typeof(SOLineSplit.lastModifiedByScreenID))]
		public virtual string LastModifiedByScreenID { get; set; }
		#endregion
		#region LastModifiedDateTime
		public abstract class lastModifiedDateTime : PX.Data.BQL.BqlDateTime.Field { }

		[PXDBLastModifiedDateTime(BqlField = typeof(SOLineSplit.lastModifiedDateTime))]
		public virtual DateTime? LastModifiedDateTime { get; set; }
		#endregion
		#region tstamp
		public abstract class Tstamp : PX.Data.BQL.BqlByteArray.Field { }

		[PXDBTimestamp(BqlField = typeof(SOLineSplit.Tstamp), VerifyTimestamp = VerifyTimestampOptions.BothFromGraphAndRecord)]
		public virtual byte[] tstamp { get; set; }
		#endregion
	}

	[PXProjection(typeof(Select>>), Persistent = true)]
	[Serializable]
	public partial class SOLine4 : PXBqlTable, IBqlTable, ISortOrder
	{
		#region BranchID
		public abstract class branchID : PX.Data.BQL.BqlInt.Field { }
		[PXDBInt(BqlField = typeof(SOLine.branchID))]
		public virtual int? BranchID
		{
			get;
			set;
		}
		#endregion
		#region OrderType
		public abstract class orderType : PX.Data.BQL.BqlString.Field { }
		protected string _OrderType;
		[PXDBString(2, IsKey = true, IsFixed = true, BqlField = typeof(SOLine.orderType))]
		public virtual String OrderType
		{
			get
			{
				return this._OrderType;
			}
			set
			{
				this._OrderType = value;
			}
		}
		#endregion
		#region OrderNbr
		public abstract class orderNbr : PX.Data.BQL.BqlString.Field { }
		protected string _OrderNbr;
		[PXDBString(15, IsUnicode = true, IsKey = true, InputMask = "", BqlField = typeof(SOLine.orderNbr))]
		[PXParent(typeof(Select>, And>>>>))]
		public virtual String OrderNbr
		{
			get
			{
				return this._OrderNbr;
			}
			set
			{
				this._OrderNbr = value;
			}
		}
		#endregion
		#region LineNbr
		public abstract class lineNbr : PX.Data.BQL.BqlInt.Field { }
		protected Int32? _LineNbr;
		[PXDBInt(IsKey = true, BqlField = typeof(SOLine.lineNbr))]
		public virtual Int32? LineNbr
		{
			get
			{
				return this._LineNbr;
			}
			set
			{
				this._LineNbr = value;
			}
		}
		#endregion
		#region SortOrder
		public abstract class sortOrder : PX.Data.BQL.BqlInt.Field { }
		protected Int32? _SortOrder;
		[PXDBInt(BqlField = typeof(SOLine.sortOrder))]
		public virtual Int32? SortOrder
		{
			get
			{
				return this._SortOrder;
			}
			set
			{
				this._SortOrder = value;
			}
		}
		#endregion
		#region Operation
		public abstract class operation : PX.Data.BQL.BqlString.Field { }
		protected String _Operation;
		[PXDBString(1, IsFixed = true, InputMask = ">a", BqlField = typeof(SOLine.operation))]
		[PXUIField(DisplayName = "Operation")]
		[SOOperation.List]
		public virtual String Operation
		{
			get
			{
				return this._Operation;
			}
			set
			{
				this._Operation = value;
			}
		}
		#endregion
		#region LineSign
		public abstract class lineSign : BqlShort.Field { }
		[PXDBShort(BqlField = typeof(SOLine.lineSign))]
		[PXDefault]
		public virtual short? LineSign
		{
			get;
			set;
		}
		#endregion
		#region ShipComplete
		public abstract class shipComplete : PX.Data.BQL.BqlString.Field { }
		protected String _ShipComplete;
		[PXDBString(1, IsFixed = true, BqlField = typeof(SOLine.shipComplete))]
		public virtual String ShipComplete
		{
			get
			{
				return this._ShipComplete;
			}
			set
			{
				this._ShipComplete = value;
			}
		}
		#endregion
		#region InventoryID
		public abstract class inventoryID : PX.Data.BQL.BqlInt.Field { }
		protected Int32? _InventoryID;
		[PXDBInt(BqlField = typeof(SOLine.inventoryID))]
		public virtual Int32? InventoryID
		{
			get
			{
				return this._InventoryID;
			}
			set
			{
				this._InventoryID = value;
			}
		}
		#endregion
		#region SiteID
		/// 
		[PXParent(typeof(Select>, And>,
				And>>>>>), LeaveChildren = true, ParentCreate = true)]
		[PXUnboundFormula(typeof(IIf>, int1, int0>), typeof(SumCalc),
			SkipZeroUpdates = false, ValidateAggregateCalculation = true)]
		[PXDBInt(BqlField = typeof(SOLine.siteID))]
		public virtual Int32? SiteID { get; set; }
		public abstract class siteID : PX.Data.BQL.BqlInt.Field { }
		#endregion
		#region UOM
		public abstract class uOM : PX.Data.BQL.BqlString.Field { }
		protected String _UOM;
		[INUnit(typeof(SOLine4.inventoryID), BqlField = typeof(SOLine.uOM))]
		public virtual String UOM
		{
			get
			{
				return this._UOM;
			}
			set
			{
				this._UOM = value;
			}
		}
		#endregion
		#region BaseOrderQty
		public abstract class baseOrderQty : PX.Data.BQL.BqlDecimal.Field { }
		[PXDBBaseQuantity(typeof(SOLine4.uOM), typeof(SOLine4.orderQty), BqlField = typeof(SOLine.baseOrderQty))]
		[PXDefault]
		public virtual decimal? BaseOrderQty
		{
			get;
			set;
		}
		#endregion
		#region OrderQty
		public abstract class orderQty : PX.Data.BQL.BqlDecimal.Field { }
		protected Decimal? _OrderQty;
		[PXDBDecimal(6, BqlField = typeof(SOLine.orderQty))]
		[PXDefault]
		public virtual Decimal? OrderQty
		{
			get
			{
				return this._OrderQty;
			}
			set
			{
				this._OrderQty = value;
			}
		}
		#endregion
		#region BaseShippedQty
		public abstract class baseShippedQty : PX.Data.BQL.BqlDecimal.Field { }
		protected Decimal? _BaseShippedQty;
		[PXDBBaseQuantity(typeof(SOLine4.uOM), typeof(SOLine4.shippedQty), BqlField = typeof(SOLine.baseShippedQty))]
		public virtual Decimal? BaseShippedQty
		{
			get
			{
				return this._BaseShippedQty;
			}
			set
			{
				this._BaseShippedQty = value;
			}
		}
		#endregion
		#region ShippedQty
		public abstract class shippedQty : PX.Data.BQL.BqlDecimal.Field { }
		protected Decimal? _ShippedQty;
		[PXDBDecimal(6, BqlField = typeof(SOLine.shippedQty))]
		[PXDefault]
		public virtual Decimal? ShippedQty
		{
			get
			{
				return this._ShippedQty;
			}
			set
			{
				this._ShippedQty = value;
			}
		}
		#endregion
		#region UnbilledQty
		public abstract class unbilledQty : PX.Data.BQL.BqlDecimal.Field { }
		protected Decimal? _UnbilledQty;
		[PXDBQuantity(typeof(SOLine4.uOM), typeof(SOLine4.baseUnbilledQty), MinValue = 0, BqlField = typeof(SOLine.unbilledQty))]
		[PXUnboundFormula(typeof(unbilledQty.Multiply), typeof(SumCalc))]
		[PXDefault]
		public virtual Decimal? UnbilledQty
		{
			get
			{
				return this._UnbilledQty;
			}
			set
			{
				this._UnbilledQty = value;
			}
		}
		#endregion
		#region BaseUnbilledQty
		public abstract class baseUnbilledQty : PX.Data.BQL.BqlDecimal.Field { }
		protected Decimal? _BaseUnbilledQty;
		[PXDBDecimal(6, BqlField = typeof(SOLine.baseUnbilledQty))]
		public virtual Decimal? BaseUnbilledQty
		{
			get
			{
				return this._BaseUnbilledQty;
			}
			set
			{
				this._BaseUnbilledQty = value;
			}
		}
		#endregion
		#region OpenQty
		public abstract class openQty : PX.Data.BQL.BqlDecimal.Field { }
		protected Decimal? _OpenQty;
		[PXDBQuantity(typeof(SOLine4.uOM), typeof(SOLine4.baseOpenQty), BqlField = typeof(SOLine.openQty))]
		[PXFormula(typeof(Sub))]
		[PXUnboundFormula(typeof(openQty.Multiply), typeof(SumCalc))]
		[PXDefault]
		public virtual Decimal? OpenQty
		{
			get
			{
				return this._OpenQty;
			}
			set
			{
				this._OpenQty = value;
			}
		}
		#endregion
		#region BaseOpenQty
		public abstract class baseOpenQty : PX.Data.BQL.BqlDecimal.Field { }
		protected Decimal? _BaseOpenQty;
		[PXDBDecimal(6, MinValue = 0, BqlField = typeof(SOLine.baseOpenQty))]
		[PXDefault(TypeCode.Decimal, "0.0")]
		[PXUIField(DisplayName = "Base Open Qty.")]
		public virtual Decimal? BaseOpenQty
		{
			get
			{
				return this._BaseOpenQty;
			}
			set
			{
				this._BaseOpenQty = value;
			}
		}
		#endregion
		#region CompleteQtyMin
		public abstract class completeQtyMin : PX.Data.BQL.BqlDecimal.Field { }
		protected Decimal? _CompleteQtyMin;
		[PXDBDecimal(2, MinValue = 0.0, MaxValue = 99.0, BqlField = typeof(SOLine.completeQtyMin))]
		[PXDefault(TypeCode.Decimal, "0.0")]
		public virtual Decimal? CompleteQtyMin
		{
			get
			{
				return this._CompleteQtyMin;
			}
			set
			{
				this._CompleteQtyMin = value;
			}
		}
		#endregion
		#region CompleteQtyMax
		public abstract class completeQtyMax : PX.Data.BQL.BqlDecimal.Field { }
		protected Decimal? _CompleteQtyMax;
		[PXDBDecimal(2, MinValue = 100.0, MaxValue = 999.0, BqlField = typeof(SOLine.completeQtyMax))]
		[PXDefault(TypeCode.Decimal, "100.0")]
		public virtual Decimal? CompleteQtyMax
		{
			get
			{
				return this._CompleteQtyMax;
			}
			set
			{
				this._CompleteQtyMax = value;
			}
		}
		#endregion
		#region Completed
		public abstract class completed : PX.Data.BQL.BqlBool.Field { }
		[PXDBBool(BqlField = typeof(SOLine.completed))]
		public virtual Boolean? Completed
		{
			get;
			set;
		}
		#endregion
		#region CuryInfoID
		public abstract class curyInfoID : PX.Data.BQL.BqlLong.Field { }
		protected Int64? _CuryInfoID;
		[PXDBLong(BqlField = typeof(SOLine.curyInfoID))]
		[CurrencyInfo()]
		public virtual Int64? CuryInfoID
		{
			get
			{
				return this._CuryInfoID;
			}
			set
			{
				this._CuryInfoID = value;
			}
		}
		#endregion
		#region CuryUnitPrice
		public abstract class curyUnitPrice : PX.Data.BQL.BqlDecimal.Field { }
		protected Decimal? _CuryUnitPrice;
		[PXDBDecimal(6, BqlField = typeof(SOLine.curyUnitPrice))]
		[PXDefault]
		public virtual Decimal? CuryUnitPrice
		{
			get
			{
				return this._CuryUnitPrice;
			}
			set
			{
				this._CuryUnitPrice = value;
			}
		}
		#endregion
		#region UnitPrice
		public abstract class unitPrice : PX.Data.BQL.BqlDecimal.Field { }
		protected Decimal? _UnitPrice;
		[PXDBDecimal(6, BqlField = typeof(SOLine.unitPrice))]
		[PXDefault]
		public virtual Decimal? UnitPrice
		{
			get
			{
				return this._UnitPrice;
			}
			set
			{
				this._UnitPrice = value;
			}
		}
		#endregion
		#region DiscPct
		public abstract class discPct : PX.Data.BQL.BqlDecimal.Field { }
		protected Decimal? _DiscPct;
		[PXDBDecimal(6, BqlField = typeof(SOLine.discPct))]
		[PXDefault]
		public virtual Decimal? DiscPct
		{
			get
			{
				return this._DiscPct;
			}
			set
			{
				this._DiscPct = value;
			}
		}
		#endregion
		#region CuryOpenAmt
		public abstract class curyOpenAmt : PX.Data.BQL.BqlDecimal.Field { }
		protected Decimal? _CuryOpenAmt;
		[PXDBCurrency(typeof(SOLine4.curyInfoID), typeof(SOLine4.openAmt), BqlField = typeof(SOLine.curyOpenAmt))]
		[PXFormula(typeof(Mult, Sub>>))]
		[PXUIField(DisplayName = "Open Amount")]
		[PXDefault]
		public virtual Decimal? CuryOpenAmt
		{
			get
			{
				return this._CuryOpenAmt;
			}
			set
			{
				this._CuryOpenAmt = value;
			}
		}
		#endregion
		#region OpenAmt
		public abstract class openAmt : PX.Data.BQL.BqlDecimal.Field { }
		protected Decimal? _OpenAmt;
		[PXDBDecimal(4, BqlField = typeof(SOLine.openAmt))]
		[PXDefault]
		public virtual Decimal? OpenAmt
		{
			get
			{
				return this._OpenAmt;
			}
			set
			{
				this._OpenAmt = value;
			}
		}
		#endregion
		#region CuryUnbilledAmt
		public abstract class curyUnbilledAmt : PX.Data.BQL.BqlDecimal.Field { }
		protected Decimal? _CuryUnbilledAmt;
		[PXDBCurrency(typeof(SOLine4.curyInfoID), typeof(SOLine4.unbilledAmt), BqlField = typeof(SOLine.curyUnbilledAmt))]
		[PXFormula(typeof(Mult, Sub>>))]
		[PXDefault]
		public virtual Decimal? CuryUnbilledAmt
		{
			get
			{
				return this._CuryUnbilledAmt;
			}
			set
			{
				this._CuryUnbilledAmt = value;
			}
		}
		#endregion
		#region UnbilledAmt
		public abstract class unbilledAmt : PX.Data.BQL.BqlDecimal.Field { }
		protected Decimal? _UnbilledAmt;
		[PXDBDecimal(4, BqlField = typeof(SOLine.unbilledAmt))]
		[PXDefault]
		public virtual Decimal? UnbilledAmt
		{
			get
			{
				return this._UnbilledAmt;
			}
			set
			{
				this._UnbilledAmt = value;
			}
		}
		#endregion
		#region GroupDiscountRate
		public abstract class groupDiscountRate : PX.Data.BQL.BqlDecimal.Field { }
		protected Decimal? _GroupDiscountRate;
		[PXDBDecimal(18, BqlField = typeof(SOLine.groupDiscountRate))]
		[PXDefault(TypeCode.Decimal, "1.0")]
		public virtual Decimal? GroupDiscountRate
		{
			get
			{
				return this._GroupDiscountRate;
			}
			set
			{
				this._GroupDiscountRate = value;
			}
		}
		#endregion
		#region DocumentDiscountRate
		public abstract class documentDiscountRate : PX.Data.BQL.BqlDecimal.Field { }
		protected Decimal? _DocumentDiscountRate;
		[PXDBDecimal(18, BqlField = typeof(SOLine.documentDiscountRate))]
		[PXDefault(TypeCode.Decimal, "1.0")]
		public virtual Decimal? DocumentDiscountRate
		{
			get
			{
				return this._DocumentDiscountRate;
			}
			set
			{
				this._DocumentDiscountRate = value;
			}
		}
		#endregion
		#region TaxCategoryID
		public abstract class taxCategoryID : PX.Data.BQL.BqlString.Field { }
		protected String _TaxCategoryID;
		[PXDBString(TX.TaxCategory.taxCategoryID.Length, IsUnicode = true, BqlField = typeof(SOLine.taxCategoryID))]
		[SOOpenTax4(typeof(SOOrder), typeof(SOTax), typeof(SOTaxTran),
			   //Per Unit Tax settings
			   Inventory = typeof(SOLine4.inventoryID), UOM = typeof(SOLine4.uOM), LineQty = typeof(SOLine4.openQty))]
		[SOUnbilledTax4(typeof(SOOrder), typeof(SOTax), typeof(SOTaxTran),
			   //Per Unit Tax settings
			   Inventory = typeof(SOLine4.inventoryID), UOM = typeof(SOLine4.uOM), LineQty = typeof(SOLine4.unbilledQty))]
		public virtual String TaxCategoryID
		{
			get
			{
				return this._TaxCategoryID;
			}
			set
			{
				this._TaxCategoryID = value;
			}
		}
		#endregion
		#region ShipDate
		public abstract class shipDate : PX.Data.BQL.BqlDateTime.Field { }
		[PXDBDate(BqlField = typeof(SOLine.shipDate))]
		public virtual DateTime? ShipDate
		{
			get;
			set;
		}
		#endregion
		#region LineAmt
		public abstract class lineAmt : PX.Data.BQL.BqlDecimal.Field { }
		[PXDBDecimal(4, BqlField = typeof(SOLine.lineAmt))]
		[PXDefault(TypeCode.Decimal, "0.0")]
		public virtual Decimal? LineAmt
		{
			get;
			set;
		}
		#endregion
		#region SalesAcctID
		public abstract class salesAcctID : PX.Data.BQL.BqlInt.Field { }
		[PXDBInt(BqlField = typeof(SOLine.salesAcctID))]
		public virtual Int32? SalesAcctID
		{
			get;
			set;
		}
		#endregion
		#region ProjectID
		public abstract class projectID : PX.Data.BQL.BqlInt.Field { }
		[PXDBInt(BqlField = typeof(SOLine.projectID))]
		public virtual Int32? ProjectID
		{
			get;
			set;
		}
		#endregion
		#region TaskID
		public abstract class taskID : PX.Data.BQL.BqlInt.Field { }
		[PXDBInt(BqlField = typeof(SOLine.taskID))]
		public virtual Int32? TaskID
		{
			get;
			set;
		}
		#endregion
		#region OpenLine
		public abstract class openLine : PX.Data.BQL.BqlBool.Field { }
		protected Boolean? _OpenLine;
		[PXDBBool(BqlField = typeof(SOLine.openLine))]
		public virtual Boolean? OpenLine
		{
			get
			{
				return this._OpenLine;
			}
			set
			{
				this._OpenLine = value;
			}
		}
		#endregion
		#region POCreate
		public abstract class pOCreate : PX.Data.BQL.BqlBool.Field { }
		[PXDBBool(BqlField = typeof(SOLine.pOCreate))]
		public virtual bool? POCreate
		{
			get;
			set;
		}
		#endregion
		#region POSource
		public abstract class pOSource : PX.Data.BQL.BqlString.Field { }
		[PXDBString(BqlField = typeof(SOLine.pOSource))]
		public virtual string POSource
		{
			get;
			set;
		}
		#endregion
		#region tstamp
		public abstract class Tstamp : PX.Data.BQL.BqlByteArray.Field { }

		[PXDBTimestamp(BqlField = typeof(SOLine.Tstamp), VerifyTimestamp = VerifyTimestampOptions.BothFromGraphAndRecord)]
		public virtual byte[] tstamp { get; set; }
		#endregion
		#region BlanketType
		/// 
		public abstract class blanketType : BqlString.Field { }

		/// 
		[PXDBString(2, IsFixed = true, BqlField = typeof(SOLine.blanketType))]
		public virtual string BlanketType
		{
			get;
			set;
		}
		#endregion
		#region BlanketNbr
		/// 
		public abstract class blanketNbr : BqlString.Field { }

		/// 
		[PXDBString(15, IsUnicode = true, BqlField = typeof(SOLine.blanketNbr))]
		public virtual string BlanketNbr
		{
			get;
			set;
		}
		#endregion
		#region BlanketLineNbr
		/// 
		public abstract class blanketLineNbr : BqlInt.Field { }

		/// 
		[PXDBInt(BqlField = typeof(SOLine.blanketLineNbr))]
		public virtual int? BlanketLineNbr
		{
			get;
			set;
		}
		#endregion
		#region BlanketSplitLineNbr
		/// 
		public abstract class blanketSplitLineNbr : BqlInt.Field { }

		/// 
		[PXDBInt(BqlField = typeof(SOLine.blanketSplitLineNbr))]
		public virtual int? BlanketSplitLineNbr
		{
			get;
			set;
		}
		#endregion
	}

	[PXProjection(typeof(Select>>), Persistent = true)]
	[Serializable]
	public partial class SOMiscLine2 : PXBqlTable, IBqlTable, ISortOrder
	{
		#region BranchID
		public abstract class branchID : PX.Data.BQL.BqlInt.Field { }
		protected Int32? _BranchID;
		[PXDBInt(BqlField = typeof(SOLine.branchID))]
		public virtual Int32? BranchID
		{
			get
			{
				return this._BranchID;
			}
			set
			{
				this._BranchID = value;
			}
		}
		#endregion
		#region OrderType
		public abstract class orderType : PX.Data.BQL.BqlString.Field { }
		protected string _OrderType;
		[PXDBString(2, IsKey = true, IsFixed = true, BqlField = typeof(SOLine.orderType))]
		public virtual String OrderType
		{
			get
			{
				return this._OrderType;
			}
			set
			{
				this._OrderType = value;
			}
		}
		#endregion
		#region OrderNbr
		public abstract class orderNbr : PX.Data.BQL.BqlString.Field { }
		protected string _OrderNbr;
		[PXDBString(15, IsUnicode = true, IsKey = true, InputMask = "", BqlField = typeof(SOLine.orderNbr))]
		[PXParent(typeof(Select>, And>>>>))]
		public virtual String OrderNbr
		{
			get
			{
				return this._OrderNbr;
			}
			set
			{
				this._OrderNbr = value;
			}
		}
		#endregion
		#region LineNbr
		public abstract class lineNbr : PX.Data.BQL.BqlInt.Field { }
		protected Int32? _LineNbr;
		[PXDBInt(IsKey = true, BqlField = typeof(SOLine.lineNbr))]
		public virtual Int32? LineNbr
		{
			get
			{
				return this._LineNbr;
			}
			set
			{
				this._LineNbr = value;
			}
		}
		#endregion
		#region SortOrder
		public abstract class sortOrder : PX.Data.BQL.BqlInt.Field { }
		protected Int32? _SortOrder;
		[PXDBInt(BqlField = typeof(SOLine.sortOrder))]
		public virtual Int32? SortOrder
		{
			get
			{
				return this._SortOrder;
			}
			set
			{
				this._SortOrder = value;
			}
		}
		#endregion
		#region DefaultOperation
		public abstract class defaultOperation : PX.Data.BQL.BqlString.Field { }
		[PXDBString(1, IsFixed = true, BqlField = typeof(SOLine.defaultOperation))]
		public virtual string DefaultOperation
		{
			get;
			set;
		}
		#endregion
		#region Operation
		public abstract class operation : PX.Data.BQL.BqlString.Field { }
		protected String _Operation;
		[PXDBString(1, IsFixed = true, InputMask = ">a", BqlField = typeof(SOLine.operation))]
		[PXUIField(DisplayName = "Operation")]
		[SOOperation.List]
		public virtual String Operation
		{
			get
			{
				return this._Operation;
			}
			set
			{
				this._Operation = value;
			}
		}
		#endregion
		#region LineSign
		public abstract class lineSign : BqlShort.Field { }
		[PXDBShort(BqlField = typeof(SOLine.lineSign))]
		[PXDefault]
		public virtual short? LineSign
		{
			get;
			set;
		}
		#endregion
		#region Completed
		public abstract class completed : PX.Data.BQL.BqlBool.Field { }
		[PXDBBool(BqlField = typeof(SOLine.completed))]
		[PXDefault]
		[PXUIField(DisplayName = "Completed")]
		public virtual Boolean? Completed
		{
			get;
			set;
		}
		#endregion
		#region InventoryID
		public abstract class inventoryID : PX.Data.BQL.BqlInt.Field { }
		protected Int32? _InventoryID;
		[NonStockItem(BqlField = typeof(SOLine.inventoryID))]
		public virtual Int32? InventoryID
		{
			get
			{
				return this._InventoryID;
			}
			set
			{
				this._InventoryID = value;
			}
		}
		#endregion
		#region SiteID
		public abstract class siteID : PX.Data.BQL.BqlInt.Field { }
		protected Int32? _SiteID;
		[PXDBInt(BqlField = typeof(SOLine.siteID))]
		public virtual Int32? SiteID
		{
			get
			{
				return this._SiteID;
			}
			set
			{
				this._SiteID = value;
			}
		}
		#endregion
		#region ProjectID
		public abstract class projectID : PX.Data.BQL.BqlInt.Field { }
		protected Int32? _ProjectID;
		[PXDBInt(BqlField = typeof(SOLine.projectID))]
		public virtual Int32? ProjectID
		{
			get
			{
				return this._ProjectID;
			}
			set
			{
				this._ProjectID = value;
			}
		}
		#endregion
		#region ShipDate
		public abstract class shipDate : PX.Data.BQL.BqlDateTime.Field { }
		protected DateTime? _ShipDate;
		[PXDBDate(BqlField =typeof(SOLine.shipDate))]
		public virtual DateTime? ShipDate
		{
			get
			{
				return this._ShipDate;
			}
			set
			{
				this._ShipDate = value;
			}
		}
		#endregion
		#region InvoiceType
		public abstract class invoiceType : PX.Data.BQL.BqlString.Field { }

		/// 
		[PXDBString(3, IsFixed = true, BqlField = typeof(SOLine.invoiceType))]
		public virtual string InvoiceType { get; set; }
		#endregion
		#region InvoiceNbr
		public abstract class invoiceNbr : PX.Data.BQL.BqlString.Field

		/// Type of the Invoice to which the return SO line is applied.
		/// 
 { }

		/// 
		[PXDBString(15, IsUnicode = true, BqlField = typeof(SOLine.invoiceNbr))]
		public virtual string InvoiceNbr { get; set; }
		#endregion
		#region InvoiceLineNbr
		public abstract class invoiceLineNbr : PX.Data.BQL.BqlInt.Field

		/// Number of the Invoice to which the return SO line is applied.
		/// 
 { }
		/// 
		[PXDBInt(BqlField = typeof(SOLine.invoiceLineNbr))]
		public virtual int? InvoiceLineNbr
		{
			get;
			set;
		}
		#endregion
		#region InvoiceDate
		public abstract class invoiceDate : PX.Data.BQL.BqlDateTime.Field

		/// Number of the Invoice line to which the return SO line is applied.
		/// 
 { }

		[PXDBDate(BqlField = typeof(SOLine.invoiceDate))]
		public virtual DateTime? InvoiceDate { get; set; }
		#endregion
		#region CuryInfoID
		public abstract class curyInfoID : PX.Data.BQL.BqlLong.Field { }
		protected Int64? _CuryInfoID;
		[PXDBLong(BqlField = typeof(SOLine.curyInfoID))]
		[CurrencyInfo()]
		public virtual Int64? CuryInfoID
		{
			get
			{
				return this._CuryInfoID;
			}
			set
			{
				this._CuryInfoID = value;
			}
		}
		#endregion
		#region UOM
		public abstract class uOM : PX.Data.BQL.BqlString.Field { }
		protected String _UOM;
		[INUnit(typeof(SOMiscLine2.inventoryID), BqlField = typeof(SOLine.uOM))]
		public virtual String UOM
		{
			get
			{
				return this._UOM;
			}
			set
			{
				this._UOM = value;
			}
		}
		#endregion
		#region OrderQty
		public abstract class orderQty : PX.Data.BQL.BqlDecimal.Field { }
		protected Decimal? _OrderQty;
		[PXDBDecimal(6, BqlField =typeof(SOLine.orderQty))]
		[PXDefault]
		public virtual Decimal? OrderQty
		{
			get
			{
				return this._OrderQty;
			}
			set
			{
				this._OrderQty = value;
			}
		}
		#endregion
		#region BilledQty
		public abstract class billedQty : PX.Data.BQL.BqlDecimal.Field { }
		protected Decimal? _BilledQty;
		[PXDBDecimal(6, BqlField = typeof(SOLine.billedQty))]
		[PXDefault]
		public virtual Decimal? BilledQty
		{
			get
			{
				return this._BilledQty;
			}
			set
			{
				this._BilledQty = value;
			}
		}
		#endregion
		#region BaseBilledQty
		public abstract class baseBilledQty : PX.Data.BQL.BqlDecimal.Field { }
		protected Decimal? _BaseBilledQty;
		[PXDBBaseQuantity(typeof(SOMiscLine2.uOM), typeof(SOMiscLine2.billedQty), BqlField = typeof(SOLine.baseBilledQty))]
		[PXDefault]
		public virtual Decimal? BaseBilledQty
		{
			get
			{
				return this._BaseBilledQty;
			}
			set
			{
				this._BaseBilledQty = value;
			}
		}
		#endregion
		#region UnbilledQty
		public abstract class unbilledQty : PX.Data.BQL.BqlDecimal.Field { }
		protected Decimal? _UnbilledQty;
		[PXDBQuantity(BqlField = typeof(SOLine.unbilledQty))]
		[PXUnboundFormula(typeof(unbilledQty.Multiply), typeof(SumCalc))]
		[PXDefault]
		public virtual Decimal? UnbilledQty
		{
			get
			{
				return this._UnbilledQty;
			}
			set
			{
				this._UnbilledQty = value;
			}
		}
		#endregion
		#region BaseUnbilledQty
		public abstract class baseUnbilledQty : PX.Data.BQL.BqlDecimal.Field { }
		protected Decimal? _BaseUnbilledQty;
		[PXDBBaseQuantity(typeof(SOMiscLine2.uOM), typeof(SOMiscLine2.unbilledQty), BqlField = typeof(SOLine.baseUnbilledQty))]
		public virtual Decimal? BaseUnbilledQty
		{
			get
			{
				return this._BaseUnbilledQty;
			}
			set
			{
				this._BaseUnbilledQty = value;
			}
		}
		#endregion
		#region CuryUnitPrice
		public abstract class curyUnitPrice : PX.Data.BQL.BqlDecimal.Field { }
		protected Decimal? _CuryUnitPrice;
		[PXDBDecimal(6, BqlField = typeof(SOLine.curyUnitPrice))]
		[PXDefault]
		public virtual Decimal? CuryUnitPrice
		{
			get
			{
				return this._CuryUnitPrice;
			}
			set
			{
				this._CuryUnitPrice = value;
			}
		}
		#endregion
		#region CuryExtPrice
		public abstract class curyExtPrice : PX.Data.BQL.BqlDecimal.Field
        {
		}
		[PXDBDecimal(6, BqlField = typeof(SOLine.curyExtPrice))]
		[PXDefault]
		public virtual decimal? CuryExtPrice
		{
			get;
			set;
		}
		#endregion
		#region CuryLineAmt
		public abstract class curyLineAmt : PX.Data.BQL.BqlDecimal.Field { }
		protected Decimal? _CuryLineAmt;
		[PXDBCurrency(typeof(SOMiscLine2.curyInfoID), typeof(SOMiscLine2.lineAmt), BqlField = typeof(SOLine.curyLineAmt))]
		[PXUIField(DisplayName = "Ext. Amount")]
		[PXDefault(TypeCode.Decimal, "0.0")]
		public virtual Decimal? CuryLineAmt
		{
			get
			{
				return this._CuryLineAmt;
			}
			set
			{
				this._CuryLineAmt = value;
			}
		}
		#endregion
		#region LineAmt
		public abstract class lineAmt : PX.Data.BQL.BqlDecimal.Field { }
		protected Decimal? _LineAmt;
		[PXDBDecimal(4, BqlField = typeof(SOLine.lineAmt))]
		[PXDefault(TypeCode.Decimal, "0.0")]
		public virtual Decimal? LineAmt
		{
			get
			{
				return this._LineAmt;
			}
			set
			{
				this._LineAmt = value;
			}
		}
		#endregion
		#region CuryBilledAmt
		public abstract class curyBilledAmt : PX.Data.BQL.BqlDecimal.Field { }
		protected Decimal? _CuryBilledAmt;
		[PXDBCurrency(typeof(SOMiscLine2.curyInfoID), typeof(SOMiscLine2.billedAmt), BqlField = typeof(SOLine.curyBilledAmt))]
		[PXDefault(TypeCode.Decimal, "0.0")]
		public virtual Decimal? CuryBilledAmt
		{
			get
			{
				return this._CuryBilledAmt;
			}
			set
			{
				this._CuryBilledAmt = value;
			}
		}
		#endregion
		#region BilledAmt
		public abstract class billedAmt : PX.Data.BQL.BqlDecimal.Field { }
		protected Decimal? _BilledAmt;
		[PXDBDecimal(4, BqlField = typeof(SOLine.billedAmt))]
		[PXDefault(TypeCode.Decimal, "0.0")]
		public virtual Decimal? BilledAmt
		{
			get
			{
				return this._BilledAmt;
			}
			set
			{
				this._BilledAmt = value;
			}
		}
		#endregion
		#region CuryUnbilledAmt
		public abstract class curyUnbilledAmt : PX.Data.BQL.BqlDecimal.Field { }
		protected Decimal? _CuryUnbilledAmt;
		[PXDBCurrency(typeof(SOMiscLine2.curyInfoID), typeof(SOMiscLine2.unbilledAmt), BqlField = typeof(SOLine.curyUnbilledAmt))]
		[PXDefault]
		public virtual Decimal? CuryUnbilledAmt
		{
			get
			{
				return this._CuryUnbilledAmt;
			}
			set
			{
				this._CuryUnbilledAmt = value;
			}
		}
		#endregion
		#region UnbilledAmt
		public abstract class unbilledAmt : PX.Data.BQL.BqlDecimal.Field { }
		protected Decimal? _UnbilledAmt;
		[PXDBDecimal(4, BqlField = typeof(SOLine.unbilledAmt))]
		[PXDefault]
		public virtual Decimal? UnbilledAmt
		{
			get
			{
				return this._UnbilledAmt;
			}
			set
			{
				this._UnbilledAmt = value;
			}
		}
		#endregion
		#region CuryDiscAmt
		public abstract class curyDiscAmt : PX.Data.BQL.BqlDecimal.Field { }
		protected Decimal? _CuryDiscAmt;
		[PXDBCurrency(typeof(SOMiscLine2.curyInfoID), typeof(SOMiscLine2.discAmt), BqlField = typeof(SOLine.curyDiscAmt))]
		[PXUIField(DisplayName = "Ext. Amount")]
		[PXDefault(TypeCode.Decimal, "0.0")]
		public virtual Decimal? CuryDiscAmt
		{
			get
			{
				return this._CuryDiscAmt;
			}
			set
			{
				this._CuryDiscAmt = value;
			}
		}
		#endregion
		#region DiscAmt
		public abstract class discAmt : PX.Data.BQL.BqlDecimal.Field { }
		protected Decimal? _DiscAmt;
		[PXDBDecimal(4, BqlField = typeof(SOLine.discAmt))]
		[PXDefault(TypeCode.Decimal, "0.0")]
		public virtual Decimal? DiscAmt
		{
			get
			{
				return this._DiscAmt;
			}
			set
			{
				this._DiscAmt = value;
			}
		}
		#endregion
		#region DiscPct
		public abstract class discPct : PX.Data.BQL.BqlDecimal.Field { }
		protected Decimal? _DiscPct;
		[PXDBDecimal(6, BqlField = typeof(SOLine.discPct))]
		[PXDefault(TypeCode.Decimal, "0.0")]
		public virtual Decimal? DiscPct
		{
			get
			{
				return this._DiscPct;
			}
			set
			{
				this._DiscPct = value;
			}
		}
		#endregion
		#region GroupDiscountRate
		public abstract class groupDiscountRate : PX.Data.BQL.BqlDecimal.Field { }
		protected Decimal? _GroupDiscountRate;
		[PXDBDecimal(18, BqlField = typeof(SOLine.groupDiscountRate))]
		[PXDefault(TypeCode.Decimal, "1.0")]
		public virtual Decimal? GroupDiscountRate
		{
			get
			{
				return this._GroupDiscountRate;
			}
			set
			{
				this._GroupDiscountRate = value;
			}
		}
		#endregion
		#region DocumentDiscountRate
		public abstract class documentDiscountRate : PX.Data.BQL.BqlDecimal.Field { }
		protected Decimal? _DocumentDiscountRate;
		[PXDBDecimal(18, BqlField = typeof(SOLine.documentDiscountRate))]
		[PXDefault(TypeCode.Decimal, "1.0")]
		public virtual Decimal? DocumentDiscountRate
		{
			get
			{
				return this._DocumentDiscountRate;
			}
			set
			{
				this._DocumentDiscountRate = value;
			}
		}
		#endregion
		#region TaxZoneID
		[PXDBString(10, IsUnicode = true, BqlField = typeof(SOLine.taxZoneID))]
		public virtual string TaxZoneID { get; set; }
		public abstract class taxZoneID : BqlString.Field { }
		#endregion
		#region TaxCategoryID
		public abstract class taxCategoryID : PX.Data.BQL.BqlString.Field { }
		protected String _TaxCategoryID;
		[PXDBString(TX.TaxCategory.taxCategoryID.Length, IsUnicode = true, BqlField = typeof(SOLine.taxCategoryID))]
		[SOUnbilledMiscTax2(typeof(SOOrder), typeof(SOTax), typeof(SOTaxTran),
			   //Per Unit Tax settings
			   Inventory = typeof(SOMiscLine2.inventoryID), UOM = typeof(SOMiscLine2.uOM), LineQty = typeof(SOMiscLine2.unbilledQty))]
		public virtual String TaxCategoryID
		{
			get
			{
				return this._TaxCategoryID;
			}
			set
			{
				this._TaxCategoryID = value;
			}
		}
		#endregion
		#region SalesPersonID
		public abstract class salesPersonID : PX.Data.BQL.BqlInt.Field { }
		protected Int32? _SalesPersonID;
		[SalesPerson(BqlField = typeof(SOLine.salesPersonID))]
		public virtual Int32? SalesPersonID
		{
			get
			{
				return this._SalesPersonID;
			}
			set
			{
				this._SalesPersonID = value;
			}
		}
		#endregion
		#region SalesAcctID
		public abstract class salesAcctID : PX.Data.BQL.BqlInt.Field { }
		protected Int32? _SalesAcctID;
		[Account(Visible = false, BqlField = typeof(SOLine.salesAcctID))]
		public virtual Int32? SalesAcctID
		{
			get
			{
				return this._SalesAcctID;
			}
			set
			{
				this._SalesAcctID = value;
			}
		}
		#endregion
		#region SalesSubID
		public abstract class salesSubID : PX.Data.BQL.BqlInt.Field { }
		protected Int32? _SalesSubID;
		[SubAccount(typeof(SOMiscLine2.salesAcctID), Visible = false, BqlField = typeof(SOLine.salesSubID))]
		public virtual Int32? SalesSubID
		{
			get
			{
				return this._SalesSubID;
			}
			set
			{
				this._SalesSubID = value;
			}
		}
		#endregion
		#region TaskID
		public abstract class taskID : PX.Data.BQL.BqlInt.Field { }
		protected Int32? _TaskID;
		[PX.Objects.PM.ActiveProjectTask(typeof(SOLine.projectID), BatchModule.SO, BqlField = typeof(SOLine.taskID), DisplayName = "Project Task")]
		public virtual Int32? TaskID
		{
			get
			{
				return this._TaskID;
			}
			set
			{
				this._TaskID = value;
			}
		}
		#endregion
		#region CostCodeID
		public abstract class costCodeID : PX.Data.BQL.BqlInt.Field { }
		protected Int32? _CostCodeID;
		[PXDBInt(BqlField = typeof(SOLine.costCodeID))]
		public virtual Int32? CostCodeID
		{
			get
			{
				return this._CostCodeID;
			}
			set
			{
				this._CostCodeID = value;
			}
		}
		#endregion
		#region TranDesc
		public abstract class tranDesc : PX.Data.BQL.BqlString.Field { }
		protected String _TranDesc;
		[PXDBString(256, IsUnicode = true, BqlField = typeof(SOLine.tranDesc))]
		[PXUIField(DisplayName = "Line Description")]
		public virtual String TranDesc
		{
			get
			{
				return this._TranDesc;
			}
			set
			{
				this._TranDesc = value;
			}
		}
		#endregion
		#region NoteID
		public abstract class noteID : PX.Data.BQL.BqlGuid.Field { }
		protected Guid? _NoteID;
		[PXNote(BqlField = typeof(SOLine.noteID))]
		public virtual Guid? NoteID
		{
			get
			{
				return this._NoteID;
			}
			set
			{
				this._NoteID = value;
			}
		}
		#endregion
		#region Commissionable
		public abstract class commissionable : PX.Data.BQL.BqlBool.Field { }
		protected bool? _Commissionable;
		[PXDBBool(BqlField = typeof(SOLine.commissionable))]
		public bool? Commissionable
		{
			get
			{
				return _Commissionable;
			}
			set
			{
				_Commissionable = value;
			}
		}
		#endregion
		#region IsFree
		public abstract class isFree : PX.Data.BQL.BqlBool.Field { }
		protected Boolean? _IsFree;
		[PXDBBool(BqlField = typeof(SOLine.isFree))]
		[PXDefault(false)]
		[PXUIField(DisplayName = "Free Item")]
		public virtual Boolean? IsFree
		{
			get
			{
				return this._IsFree;
			}
			set
			{
				this._IsFree = value;
			}
		}
		#endregion
		#region ManualPrice
		public abstract class manualPrice : PX.Data.BQL.BqlBool.Field { }
		protected Boolean? _ManualPrice;
		[PXDBBool(BqlField = typeof(SOLine.manualPrice))]
		[PXDefault(false)]
		public virtual Boolean? ManualPrice
		{
			get
			{
				return this._ManualPrice;
			}
			set
			{
				this._ManualPrice = value;
			}
		}
		#endregion
		#region ManualDisc
		public abstract class manualDisc : PX.Data.BQL.BqlBool.Field { }
		protected Boolean? _ManualDisc;
		[PXDBBool(BqlField = typeof(SOLine.manualDisc))]
		[PXDefault(false)]
		[PXUIField(DisplayName = "Manual Discount", Visibility = PXUIVisibility.Visible)]
		public virtual Boolean? ManualDisc
		{
			get
			{
				return this._ManualDisc;
			}
			set
			{
				this._ManualDisc = value;
			}
		}
		#endregion

		#region DiscountID
		public abstract class discountID : PX.Data.BQL.BqlString.Field { }
		protected String _DiscountID;
		[PXDBString(10, IsUnicode = true, BqlField = typeof(SOLine.discountID))]
		[PXSelector(typeof(Search>>))]
		[PXUIField(DisplayName = "Discount Code", Visible = true, Enabled = false)]
		public virtual String DiscountID
		{
			get
			{
				return this._DiscountID;
			}
			set
			{
				this._DiscountID = value;
			}
		}
		#endregion
		#region DiscountSequenceID
		public abstract class discountSequenceID : PX.Data.BQL.BqlString.Field { }
		protected String _DiscountSequenceID;
		[PXDBString(10, IsUnicode = true, BqlField = typeof(SOLine.discountSequenceID))]
		[PXUIField(DisplayName = "Discount Sequence", Visible = false, Enabled = false)]
		public virtual String DiscountSequenceID
		{
			get
			{
				return this._DiscountSequenceID;
			}
			set
			{
				this._DiscountSequenceID = value;
			}
		}
		#endregion
		#region DRTermStartDate
		public abstract class dRTermStartDate : PX.Data.BQL.BqlDateTime.Field { }

		protected DateTime? _DRTermStartDate;

		[PXDBDate(BqlField = typeof(SOLine.dRTermStartDate))]
		[PXUIField(DisplayName = "Term Start Date")]
		public DateTime? DRTermStartDate
		{
			get { return _DRTermStartDate; }
			set { _DRTermStartDate = value; }
		}
		#endregion
		#region DRTermEndDate
		public abstract class dRTermEndDate : PX.Data.BQL.BqlDateTime.Field { }

		protected DateTime? _DRTermEndDate;

		[PXDBDate(BqlField = typeof(SOLine.dRTermEndDate))]
		[PXUIField(DisplayName = "Term End Date")]
		public DateTime? DRTermEndDate
		{
			get { return _DRTermEndDate; }
			set { _DRTermEndDate = value; }
		}
		#endregion
		#region CuryUnitPriceDR
		public abstract class curyUnitPriceDR : PX.Data.BQL.BqlDecimal.Field { }

		protected decimal? _CuryUnitPriceDR;

		[PXUIField(DisplayName = "Unit Price for DR", Visible = false)]
		[PXDBDecimal(typeof(Search), BqlField = typeof(SOLine.curyUnitPriceDR))]
		public virtual decimal? CuryUnitPriceDR
		{
			get { return _CuryUnitPriceDR; }
			set { _CuryUnitPriceDR = value; }
		}
		#endregion
		#region LineDiscountDR
		public abstract class discPctDR : PX.Data.BQL.BqlDecimal.Field { }

		protected decimal? _DiscPctDR;

		[PXUIField(DisplayName = "Discount Percent for DR", Visible = false)]
		[PXDBDecimal(6, MinValue = -100, MaxValue = 100, BqlField = typeof(SOLine.discPctDR))]
		public virtual decimal? DiscPctDR
		{
			get { return _DiscPctDR; }
			set { _DiscPctDR = value; }
		}
		#endregion
		#region DefScheduleID
		public abstract class defScheduleID : PX.Data.BQL.BqlInt.Field { }
		protected int? _DefScheduleID;
		[PXDBInt(BqlField = typeof(SOLine.defScheduleID))]
		public virtual int? DefScheduleID
		{
			get
			{
				return this._DefScheduleID;
			}
			set
			{
				this._DefScheduleID = value;
			}
		}
		#endregion

		#region BlanketType
		public abstract class blanketType : Data.BQL.BqlString.Field { }
		[PXDBString(2, IsFixed = true, BqlField = typeof(SOLine.blanketType))]
		public virtual string BlanketType
		{
			get;
			set;
		}
		#endregion
		#region BlanketNbr
		public abstract class blanketNbr : Data.BQL.BqlString.Field { }
		[PXDBString(15, IsUnicode = true, BqlField = typeof(SOLine.blanketNbr))]
		public virtual string BlanketNbr
		{
			get;
			set;
		}
		#endregion
		#region BlanketLineNbr
		public abstract class blanketLineNbr : Data.BQL.BqlInt.Field { }
		[PXDBInt(BqlField = typeof(SOLine.blanketLineNbr))]
		public virtual int? BlanketLineNbr
		{
			get;
			set;
		}
		#endregion
		#region BlanketSplitLineNbr
		public abstract class blanketSplitLineNbr : Data.BQL.BqlInt.Field { }
		[PXDBInt(BqlField = typeof(SOLine.blanketSplitLineNbr))]
		public virtual int? BlanketSplitLineNbr
		{
			get;
			set;
		}
		#endregion
		#region tstamp
		public abstract class Tstamp : PX.Data.BQL.BqlByteArray.Field { }

		[PXDBTimestamp(BqlField = typeof(SOLine.Tstamp), VerifyTimestamp = VerifyTimestampOptions.BothFromGraphAndRecord)]
		public virtual byte[] tstamp { get; set; }
		#endregion
	}

	[Serializable()]
	public partial class AddSOFilter : PXBqlTable, IBqlTable
	{
		#region Operation
		public abstract class operation : PX.Data.BQL.BqlString.Field { }
		protected String _Operation;
		[PXDBString(1, IsFixed = true, InputMask = ">a")]
		[PXUIField(DisplayName = "Operation")]
		[PXDefault(SOOperation.Issue, typeof(SOShipment.operation))]
		[SOOperation.List]
		public virtual String Operation
		{
			get
			{
				return this._Operation;
			}
			set
			{
				this._Operation = value;
			}
		}
		#endregion
		#region OrderType
		public abstract class orderType : PX.Data.BQL.BqlString.Field { }
		protected String _OrderType;
		[PXDBString(2, IsFixed = true, InputMask = ">aa")]
		[PXSelector(typeof(Search2>,
			Where, And, And,
				And>,
				And, And, Equal,
				Or, And, Equal>>>>>>>>>>))]
		[PXRestrictor(typeof(Where, IsNull,
			Or, Equal>, False, True>>>>),
			Messages.OrderTypeCantSelectBecauseARDocType, typeof(SOOrderType.orderType))]
		[PXDefault(typeof(Search2,
			LeftJoin>>>,
			Where, And, And,
				And>,
				And2, And, Equal,
					Or, And, Equal>>>>,
				And, IsNull,
					Or, Equal>, False, True>>>>>>>>>>,
			OrderBy>>>))]
		[PXUIField(DisplayName = "Order Type")]
		[PXFormula(typeof(Default))]
		public virtual String OrderType
		{
			get
			{
				return this._OrderType;
			}
			set
			{
				this._OrderType = value;
			}
		}
		#endregion
		#region OrderNbr
		public abstract class orderNbr : PX.Data.BQL.BqlString.Field { }
		protected String _OrderNbr;
		[PXDBString(15, IsUnicode = true, InputMask = "")]
		[PXUIField(DisplayName = "Order Nbr.")]
		[PXDefault]
		[SO.RefNbr(typeof(Search>,
			And>,
			And2>, Or, Equal>>,
			And,
			And,
			And,
			And,
			And>>>>>>>>>), Filterable = true)]
		[PXFormula(typeof(Default))]
		public virtual String OrderNbr
		{
			get
			{
				return this._OrderNbr;
			}
			set
			{
				this._OrderNbr = value;
			}
		}
		#endregion

		#region OrderLineNbr
		/// 
		public abstract class orderLineNbr : PX.Data.BQL.BqlInt.Field { }
		/// 
		[PXDBInt()]
		[PXUIField(DisplayName = "Line Nbr.", Visible = false, Enabled = false)]
		public virtual Int32? OrderLineNbr
		{
			get;
			set;
		}
		#endregion

		#region FreightAmountSource
		public abstract class freightAmountSource : PX.Data.BQL.BqlString.Field { }
		[PXDBString(1, IsFixed = true)]
		[FreightAmountSource]
		[PXFormula(typeof(Selector))]
		public virtual string FreightAmountSource
		{
			get;
			set;
		}
		#endregion
		#region AddAllLines
		public abstract class addAllLines : PX.Data.BQL.BqlBool.Field { }
		protected Boolean? _AddAllLines;
		[PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
		public virtual Boolean? AddAllLines
		{
			get
			{
				return this._AddAllLines;
			}
			set
			{
				this._AddAllLines = value;
			}
		}
		#endregion
	}
}
