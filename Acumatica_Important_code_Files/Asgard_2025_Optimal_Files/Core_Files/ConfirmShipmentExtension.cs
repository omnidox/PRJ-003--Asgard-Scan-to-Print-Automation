

using System;
using System.Collections;
using System.Linq;
using PX.Common;
using PX.Data;
using PX.Data.BQL;
using PX.Data.BQL.Fluent;
using PX.Objects.Common.Extensions;
using PX.Objects.CS;
using PX.Objects.IN;
using PX.Objects.IN.Attributes;
using PX.Objects.SO.Models;
using ShipmentActions = PX.Objects.SO.SOShipmentEntryActionsAttribute;

namespace PX.Objects.SO.GraphExtensions.SOShipmentEntryExt
{
	/// 

	/// An extension of the  graph that provides functionality
	/// for confirming shipments.
	/// 

	// Acuminator disable once PX1016 ExtensionDoesNotDeclareIsActiveMethod extension should be constantly active
	public class ConfirmShipmentExtension : PXGraphExtension
	{
		public Data.WorkflowAPI.PXWorkflowEventHandler OnShipmentConfirmed;

		public PXAction confirmShipmentAction;
		[PXButton(CommitChanges = true), PXUIField(DisplayName = ShipmentActions.Messages.ConfirmShipment, MapEnableRights = PXCacheRights.Select, MapViewRights = PXCacheRights.Select)]
		protected virtual IEnumerable ConfirmShipmentAction(PXAdapter adapter)
		{
			var list = adapter.Get().ToList();
			bool massProcess = adapter.MassProcess;

			Base.Save.Press();

			PXLongOperation.StartOperation(this, delegate ()
			{
				SOShipmentEntry docgraph = PXGraph.CreateInstance();

				var sourceGraph = docgraph.GetOrigDocumentGraph(string.Empty);

				PXProcessing.ProcessRecords(list, massProcess,
					processRecordBy: shipment =>
					{
						docgraph.GetExtension().PrepareShipmentForConfirmation(shipment);

						docgraph.ShipPackages(shipment);

						docgraph.GetExtension().ConfirmShipment(new(shipment, sourceGraph, true));
					});
			});

			return list;
		}

		public virtual void ConfirmShipment(ConfirmShipmentArgs args)
		{
			SOShipment shiporder = args.Shipment;
			PXGraph sourceGraph = args.OrigDocumentGraph;

			if (!args.IsShipmentReadyForConfirmation)
			{
				PrepareShipmentForConfirmation(shiporder);
			}

			if (Base.Document.Current == null)
			{
				return;
			}

			MarkConfirmed(Base.Document.Current);
			Base.Document.Cache.MarkUpdated(Base.Document.Current, assertError: true);
			Base.Document.Cache.IsDirty = true;

			foreach (PXResult res in
				PXSelectJoin>>,
				Where>>>.
				Select(Base))
			{
				SOShipLineSplit split = (SOShipLineSplit)res;
				INItemPlan plan = PXCache.CreateCopy((INItemPlan)res);
				INPlanType plantype = INPlanType.PK.Find(Base, plan.PlanType);

				if (plantype.DeleteOnEvent == true)
				{
					Base.Caches[typeof(INItemPlan)].Delete(plan);
				}
				else if (!string.IsNullOrEmpty(plantype.ReplanOnEvent))
				{
					plan.PlanType = plantype.ReplanOnEvent;
					plan.OrigPlanType = null;
					Base.Caches[typeof(INItemPlan)].Update(plan);
				}
				split = (SOShipLineSplit)Base.splits.Cache.Locate(split) ?? split;
				Base.splits.Cache.MarkUpdated(split, assertError: true);
				if (split != null)
				{
					split.Confirmed = true;
					if (plantype.DeleteOnEvent == true)
					{
						split.PlanID = null;
					}
				}
				Base.splits.Cache.IsDirty = true;
			}

			if ((PXAccess.FeatureInstalled() || PXAccess.FeatureInstalled()) && Base.Document.Current.CurrentWorksheetNbr != null)
			{
				//clear dirty object loaded in scope of separate PXConnectionScope via PXFormulaAttribute
				Base.Caches().Clear();
				Base.Caches().ClearQueryCacheObsolete();

				SOPickingWorksheet worksheet =
					SelectFrom.
					Where>.
					View.Select(Base, Base.Document.Current.CurrentWorksheetNbr);

				if (worksheet.WorksheetType.IsIn(SOPickingWorksheet.worksheetType.Wave, SOPickingWorksheet.worksheetType.Single))
				{
					Base.CartSupportExt?.RemoveItemsFromCart();
				}

				Base.TryCompleteWorksheet(worksheet);
			}

			using (PXTransactionScope ts = new PXTransactionScope())
			{
				SetSuppressWorkflowOnConfirmShipment();

				UpdateOrigDocumentOnConfirmShipment(args);

				GroupShipLinesForInvoicing(Base.Document.Current);

				Base.WorkLogExt?.CloseFor(Base.Document.Current.ShipmentNbr);

				SOShipment.Events
					.Select(e => e.ShipmentConfirmed)
					.FireOn(Base, Base.Document.Current);
				Base.Save.Press();

				ts.Complete();
			}

			Base.Document.Cache.RestoreCopy(shiporder, Base.Document.Current);
		}

		[PXInternalUseOnly]
		protected virtual void SetSuppressWorkflowOnConfirmShipment() => PXTransactionScope.SetSuppressWorkflow(true);

		public virtual void PrepareShipmentForConfirmation(SOShipment shiporder)
		{
			Base.Clear();

			Base.Document.Current = Base.Document.Search(shiporder.ShipmentNbr);
			if (PX.SM.WorkflowAction.HasWorkflowActionEnabled(Base, g => g.GetExtension().confirmShipmentAction, Base.Document.Current) == false)
			{
				throw new PXInvalidOperationException(Messages.ActionNotAvailableInCurrentState,
					confirmShipmentAction.GetCaption(), Base.Document.Cache.GetRowDescription(Base.Document.Current));
			}

			ValidateShipment(shiporder);
		}

		public virtual void MarkConfirmed(SOShipment shipment)
		{
			shipment.Confirmed = true;
			shipment.ConfirmedToVerify = false;
			shipment.Status = SOShipmentStatus.Confirmed;
		}

		public virtual bool ConfirmSingleLine(SOShipLine shipline, InventoryItem inventoryItem)
		{
			bool requireINUpdate = false;

			object cached = Base.Caches[typeof(SOShipLine)].Locate(shipline);
			if (cached != null)
			{
				shipline = (SOShipLine)cached;
			}

			if (Math.Abs((decimal)shipline.BaseQty) < 0.0000005m)
			{
				Base.LineSplittingExt.RaiseRowDeleted(shipline);
			}

			shipline.Confirmed = true;

			if (shipline.LineType == SOLineType.Inventory)
			{
				if (inventoryItem.StkItem == false && inventoryItem.KitItem == true
					&& ((SOShipLineSplit)PXSelectJoin>>>,
						Where>,
							And>>>>.
						SelectSingleBound(Base, new object[] { shipline })) == null)
				{
					shipline.RequireINUpdate = false;
				}
				else
				{
					shipline.RequireINUpdate = true;
					requireINUpdate = true;
				}
			}
			else
			{
				shipline.RequireINUpdate = false;
			}

			Base.Caches[typeof(SOShipLine)].MarkUpdated(shipline, assertError: true);
			Base.Caches[typeof(SOShipLine)].IsDirty = true;

			return requireINUpdate;
		}

		public virtual void UpdateOrigDocumentOnConfirmShipment(ConfirmShipmentArgs args) { }

		public virtual void GroupShipLinesForInvoicing(SOShipment ship)
		{
			foreach (var shipGroupBySOLine in Base.Transactions.Cache.Updated.RowCast()
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
				var item = InventoryItem.PK.Find(Base, firstShipLine.InventoryID);
				var shipGroupWithBaseUom = shipGroupsByUom.FirstOrDefault(g => string.Equals(g.Key, item?.BaseUnit, StringComparison.OrdinalIgnoreCase));
				var shipGroupWithSalesUom = shipGroupsByUom.FirstOrDefault(g => !string.Equals(g.Key, item?.BaseUnit, StringComparison.OrdinalIgnoreCase));
				if (shipGroupWithBaseUom == null || shipGroupWithSalesUom == null)
				{
					shipGroupBySOLine.ForEach(sl => sl.InvoiceGroupNbr = 1);
					continue;
				}

				shipGroupWithSalesUom.ForEach(sl => sl.InvoiceGroupNbr = 1);
				decimal? baseUomQtySum = shipGroupWithBaseUom.Sum(sl => sl.ShippedQty);
				decimal baseUomQtyConvertedToSalesUom = INUnitAttribute.ConvertFromBase(Base.Transactions.Cache,
					firstShipLine.InventoryID,
					shipGroupWithSalesUom.Key,
					baseUomQtySum ?? 0m,
					INPrecision.NOROUND);
				shipGroupWithBaseUom.ForEach(sl => sl.InvoiceGroupNbr = (baseUomQtyConvertedToSalesUom % 1m == 0m) ? 1 : 2);
			}
		}

		public virtual void ValidateShipment(SOShipment shiporder)
		{
			if (Base.sosetup.Current.RequireShipmentTotal == true)
			{
				if (Base.Document.Current.ShipmentQty != Base.Document.Current.ControlQty)
				{
					throw new PXException(Messages.MissingShipmentControlTotal);
				}
			}

			if (Base.Document.Current.ShipmentQty == 0)
			{
				throw new PXException(Messages.UnableConfirmZeroShipment, Base.Document.Current.ShipmentNbr);
			}

			Carrier carrier = Carrier.PK.Find(Base, Base.Document.Current.ShipVia);
			if (carrier != null && carrier.IsExternal == true && carrier.PackageRequired == true)
			{
				//check for at least one package
				SOPackageDetail p = Base.Packages.SelectSingle();
				if (p == null)
				{
					throw new PXException(Messages.PackageIsRequired);
				}
			}

			foreach (SOShipLine line in Base.Transactions.Select())
			{
				ConvertedInventoryItemAttribute.ValidateRow(Base.Transactions.Cache, line);
			}
		}
	}
}
