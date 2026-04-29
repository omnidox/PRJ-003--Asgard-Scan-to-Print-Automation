

using System;
using System.Collections.Generic;
using System.Linq;
using PX.Api;
using PX.Common;
using PX.Data;
using PX.Objects.CM;
using PX.Objects.Common.Extensions;
using PX.Objects.CS;
using PX.Objects.IN;
using PX.Objects.SO.Interfaces;
using PX.Objects.SO.Models;
using LineShipment = PX.Objects.SO.SOShipmentEntry.LineShipment;

namespace PX.Objects.SO.GraphExtensions.SOShipmentEntryExt
{
	/// 

	/// An extension of the  graph and
	///  and  graph extensions.
	/// This extension manages the relations between the  and  entities
	/// during shipment creation.
	/// 

	// Acuminator disable once PX1016 ExtensionDoesNotDeclareIsActiveMethod extension should be constantly active
	[PXProtectedAccess(typeof(SOOrderExtension))]
	public abstract class CreateShipmentSOExtension : PXGraphExtension
	{
		#region Protected Access
		/// Uses 
		[PXProtectedAccess]
		protected abstract void ValidateLineType(SOLine line, InventoryItem item, string message);

		/// Uses 
		[PXProtectedAccess]
		protected abstract void AllocateGroupFreeItems(SOOrder order);

		/// Uses 
		[PXProtectedAccess]
		protected abstract void AdjustFreeItemLines();

		/// Uses 
		[PXProtectedAccess]
		protected abstract void UpdateShipmentCntr(PXCache sender, SOOrderShipment row, short? counter);
		#endregion

		/// Overrides 
		[PXOverride]
		public void ValidateCreateShipmentArgs(CreateShipmentArgs args,
			Action base_ValidateCreateShipmentArgs)
		{
			base_ValidateCreateShipmentArgs(args);

			if (args.Order == null)
			{
				return;
			}

			SOOrderType ordertype = Base1.soordertype.Select(args.Order.OrderType);

			if (ordertype != null)
			{
				args.Operation ??= ordertype.DefaultOperation;
				args.CopyLineNotesAndFilesSettings ??= new CopySettings(ordertype.CopyLineFilesToShipment, ordertype.CopyLineNotesToShipment);

				args.CopyNotesAndFilesSettings ??= new CopySettings(ordertype.CopyHeaderFilesToShipment, ordertype.CopyHeaderNotesToShipment ?? false);
				args.FilesAndNotesSource = args.Order;
			}

			SOOrderTypeOperation orderOperation = SOOrderTypeOperation.PK.Find(Base, args.Order.OrderType, args.Operation);

			if (orderOperation != null && orderOperation.Active == true && string.IsNullOrEmpty(orderOperation.ShipmentPlanType))
			{
				object state = Base.Caches().GetStateExt(orderOperation);
				throw new PXException(Messages.ShipmentPlanTypeNotSetup, args.Order.OrderType, state);
			}
			args.ShipmentType = INTranType.DocType(orderOperation.INDocType);

			if (args.ShipmentList != null)
			{
				bool? selected = args.Order.Selected;
				var order = ActualizeAndValidateOrder(args.Graph, args.Order, args.Operation);
				PXCache.RestoreCopy(args.Order, order);
				args.Order.Selected = selected;
			}
		}

		/// Overrides 
		[PXOverride]
		public DateTime? GetShipmentDate(CreateShipmentArgs args,
			Func base_GetShipmentDate)
		{
			var res = base_GetShipmentDate(args);
			if (args.UseOptimalShipDate != true || args.Order == null)
			{
				return res;
			}

			SOOrder order = args.Order;
			SOShipmentPlan plan =
				order.ShipComplete == SOShipComplete.BackOrderAllowed ?
					PXSelectJoinGroupBy>>,
					Where>,
						And>,
						And>,
						And>>>>>,
					Aggregate>>.
					Select(Base, args.SiteID, order.OrderType, order.OrderNbr, args.Operation)
				:
					PXSelectJoinGroupBy>>,
					Where>,
						And>,
						And>,
						And>>>>>,
					Aggregate>>.
					Select(Base, args.SiteID, order.OrderType, order.OrderNbr, args.Operation);

			return plan.PlanDate > res ? plan.PlanDate : res;
		}

		/// Overrides 
		[PXOverride]
		public SOShipment FindOrCreateShipment(CreateShipmentArgs args,
			Func base_FindOrCreateShipment)
		{
			SOOrder order = args.Order;
			if (order?.ShipSeparately == true)
			{
				return new SOShipment() { Hidden = true };
			}
			else
			{
				return base_FindOrCreateShipment(args);
			}
		}

		/// Overrides 
		[PXOverride]
		public FieldLookup[] GetShipmentFieldLookups(CreateShipmentArgs args,
			Func base_GetShipmentFieldLookups)
		{
			var res = base_GetShipmentFieldLookups(args);

			SOOrder order = args.Order;
			if (order == null)
			{
				return res;
			}

			List list = new List(res) {
				new FieldLookup(order.CustomerID),
				new FieldLookup(order.ShipAddressID),
				new FieldLookup(order.ShipContactID),
				new FieldLookup(order.FOBPoint),
				new FieldLookup(order.ShipVia),
				new FieldLookup(order.ShipTermsID),
				new FieldLookup(order.ShipZoneID),
				new FieldLookup(order.ARDocType != AR.ARDocType.NoUpdate),
				new FieldLookup(order.UseCustomerAccount),
				new FieldLookup(order.FreightAmountSource),
				new FieldLookup(order.IsManualPackage)
			};

			return list.ToArray();
		}

		/// Overrides 
		[PXOverride]
		public bool SetShipmentFieldsFromOrigDocument(SOShipment shipment, CreateShipmentArgs args, bool newlyCreated,
			Func base_SetShipmentFieldsFromOrigDocument)
		{
			bool result = base_SetShipmentFieldsFromOrigDocument(shipment, args, newlyCreated);

			SOOrder order = args.Order;
			if (order == null)
			{
				return result;
			}

			if (newlyCreated)
			{
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

				if (order.FOBPoint != null && (shipment.FOBPoint == null || !Base.IsContractBasedAPI))
				{
					shipment.FOBPoint = order.FOBPoint;
				}

				if (order.ShipTermsID != null && (shipment.ShipTermsID == null || !Base.IsContractBasedAPI))
				{
					shipment.ShipTermsID = order.ShipTermsID;
				}

				if (order.ShipVia != null && (shipment.ShipVia == null || !Base.IsContractBasedAPI))
				{
					shipment.ShipVia = order.ShipVia;
				}

				if (order.ShipZoneID != null && (shipment.ShipZoneID == null || !Base.IsContractBasedAPI))
				{
					shipment.ShipZoneID = order.ShipZoneID;
				}

				if (string.IsNullOrEmpty(shipment.ShipmentDesc))
				{
					shipment.ShipmentDesc = order.OrderDesc;
				}

				return true;
			}
			else
			{
				if (shipment.FreightAmountSource != order.FreightAmountSource)
				{
					// double check that we don't mix orders with different Freight Amount Source
					throw new PXException();
				}

				if (Base1.OrderList.Select().Count() == 1)
				{
					if (args.MassProcess && args.ShipmentList != null)
					{
						shipment.ShipmentDesc = PXMessages.LocalizeNoPrefix(Messages.MultiOrderShipment);
						result = true;
					}

					SOOrder firstOrder = (PXResult)Base1.OrderList.Select();
					if (firstOrder.OrderNbr != order.OrderNbr || firstOrder.OrderType != order.OrderType)
					{
						if (!string.IsNullOrEmpty(shipment.CustomerOrderNbr))
						{
							// If we have several Orders within shipment we can't fill CustomerOrderNbr.
							shipment.CustomerOrderNbr = null;
							result = true;
						}
					}
				}

				return result;
			}
		}

		/// Overrides 
		[PXOverride]
		public void SetShipAddressAndContactFromArgs(SOShipment shipment, CreateShipmentArgs args,
			Action base_SetShipAddressAndContactFromArgs)
		{
			base_SetShipAddressAndContactFromArgs(shipment, args);
			if (args.Order != null)
			{
				SetShipAddressAndContact(shipment, args.Order.ShipAddressID, args.Order.ShipContactID);
			}
		}

		/// Overrides 
		[PXOverride]
		public void CreateShipmentDetails(CreateShipmentArgs args,
			Action base_CreateShipmentDetails)
		{
			base_CreateShipmentDetails(args);

			var order = args.Order;
			if (order == null)
			{
				return;
			}

			if (order.OpenShipmentCntr > 0)
			{
				SOOrderShipment openShipment =
					PXSelectReadonly>,
						And>,
						And>,
						And>,
						And>>>>>>.
					Select(Base, order.OrderType, order.OrderNbr, args.SiteID, Base.Document.Current.ShipmentNbr);
				if (openShipment != null)
				{
					throw new PXException(Messages.OrderHasOpenShipment, order.OrderType, order.OrderNbr, openShipment.ShipmentNbr);
				}
			}

			var newOrderShipment = new SOOrderShipment
			{
				OrderType = order.OrderType,
				OrderNbr = order.OrderNbr,
				OrderNoteID = order.NoteID,
				ShipmentNbr = Base.Document.Current.ShipmentNbr,
				ShipmentType = Base.Document.Current.ShipmentType,
				ShippingRefNoteID = Base.Document.Current.NoteID,
				Operation = Base.Document.Current.Operation,
				ProjectID = order.ProjectID
			};

			Base1.soorder.Cache.Hold(order);
			PXParentAttribute.SetParent(Base1.OrderList.Cache, newOrderShipment, typeof(SOOrder), order);

			var orderlist = Base1.OrderListSimple.Select().ToList();
			var located = Base1.OrderList.Locate(newOrderShipment);

			if (located == null || Base1.OrderList.Cache.GetStatus(located).IsIn(PXEntryStatus.Deleted, PXEntryStatus.InsertedDeleted))
			{
				newOrderShipment = Base1.OrderList.Insert(located ?? newOrderShipment);
			}
			else
			{
				newOrderShipment = located;
			}

			void SOOrderShipment_RowDeleting(PXCache sender, PXRowDeletingEventArgs e)
			{
				e.Cancel = true;
			}
			Base.RowDeleting.AddHandler(SOOrderShipment_RowDeleting);

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
				var cached = (SOLine2)Base1.soline.Cache.Locate(keys);

				if (lineships.TryGetValue(cached, out LineShipment lineship))
				{
					lineship.AnyDeleted = true;
				}

				anyDeleted = true;
			}
			Base.RowDeleted.AddHandler(SOShipLine_RowDeleted);

			foreach (SOLine2 sl in
				PXSelect>,
					And>,
					And>,
					And>,
					And>>>>>>.
				Select(Base, order.OrderType, order.OrderNbr, args.SiteID, args.Operation))
			{
				PXParentAttribute.SetParent(Base1.soline.Cache, sl, typeof(SOOrder), order);
			}

			foreach (SOLineSplit2 sl in
				PXSelect>,
					And>,
					And>,
					And>,
					And>>>>>>.
				Select(Base, order.OrderType, order.OrderNbr, args.SiteID, args.Operation))
			{
				//just place into cache
			}

			foreach (SOShipLine sl in
				PXSelect>,
					And>>>>.
				Select(Base))
			{
				PXParentAttribute.SetParent(Base.Transactions.Cache, sl, typeof(SOOrder), order);
			}

			bool hasUnallocatedSplits = false;
			using (new SOOrderExtension.SkipAdjustFreeItemLinesScope(Base1))// Free items will still be Adjusted at the end of this method
			{
				List schedulesList = new List();

				foreach (PXResult res in
					Base1.ShipmentScheduleSelect.Select(args.SiteID, args.EndDate, order.OrderType, order.OrderNbr, args.OrderLineNbr, args.OrderLineNbr, args.Operation))
				{
					SOShipmentPlan plan = res;
					SOLineSplit split = res;

					if (plan.RequireAllocation == true && split.LineType != SOLineType.NonInventory && split.Operation != SOOperation.Receipt
						&& plan.InclQtySOShipping != 1 && plan.InclQtySOShipped != 1)
					{
						hasUnallocatedSplits = true;
						if (Base.sosetup.Current.AddAllToShipment != true)
						{
							continue;
						}
					}

					schedulesList.Add(new ShipmentSchedule(
						new PXResult(plan, split, res, res, res, res),
						new SOShipLine { OrigSplitLineNbr = split.SplitLineNbr }));
				}

				schedulesList.Sort();

				foreach (ShipmentSchedule ss in schedulesList)
				{
					ss.ShipLine.ShipmentType = Base.Document.Current.ShipmentType;
					ss.ShipLine.ShipmentNbr = Base.Document.Current.ShipmentNbr;
					ss.ShipLine.LineNbr = (int?)PXLineNbrAttribute.NewLineNbr(Base.Transactions.Cache, Base.Document.Current);

					PXParentAttribute.SetParent(Base.Transactions.Cache, ss.ShipLine, typeof(SOOrder), order);

					SOLine soLine = ss.Result;
					SOLineSplit soSplit = ss.Result;
					SOLine2 soLine2 = Base1.soline.Locate(new SOLine2
					{
						OrderType = soLine.OrderType,
						OrderNbr = soLine.OrderNbr,
						LineNbr = soLine.LineNbr
					});

					if (soLine2 != null)
					{
						PXParentAttribute.SetParent(Base.Transactions.Cache, ss.ShipLine, typeof(SOLine2), soLine2);
					}
					else
					{
						if (soLine.Completed == true && soSplit.Completed != true)
						{
							throw new PXException(Messages.CompletedSOLineHasIncompleteSplit, soLine.OrderNbr, soLine.LineNbr, ((InventoryItem)ss.Result).InventoryCD);
						}
					}

					LineShipment lineship = lineships.Ensure(soLine2, () => new LineShipment());
					lineship.Add(ss.ShipLine);

					SOLineSplit2 soSplit2 = Base1.solinesplit.Locate(new SOLineSplit2
					{
						OrderType = soSplit.OrderType,
						OrderNbr = soSplit.OrderNbr,
						LineNbr = soSplit.LineNbr,
						SplitLineNbr = soSplit.SplitLineNbr
					});
					if (soSplit2 != null)
					{
						PXParentAttribute.SetParent(Base.Transactions.Cache, ss.ShipLine, typeof(SOLineSplit2), soSplit2);
					}

					PXParentAttribute.SetParent(Base.Transactions.Cache, ss.ShipLine, typeof(SOOrderShipment), newOrderShipment);

					if (args.ShipmentList == null || soLine2.ShipComplete != SOShipComplete.ShipComplete || lineship.AnyDeleted == false)
					{
						Base2.CreateShipmentFromSchedules(args, new SOLineShipLineSource(ss.Result), ss.ShipLine);
					}

					if (args.ShipmentList != null && soLine2.ShipComplete == SOShipComplete.ShipComplete && lineship.AnyDeleted)
					{
						foreach (SOShipLine shipline in lineship)
						{
							Base.Transactions.Delete(shipline);
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
							Base2.RemoveLineFromShipment(shipline, args.ShipmentList != null);
						}
					}
				}
			}

			if (args.QuickProcessFlow != PXQuickProcess.ActionFlow.NoFlow && Base.sosetup.Current.RequireShipmentTotal == true)
			{
				Base.Document.Current.ControlQty = Base.Document.Current.ShipmentQty;
			}

			AllocateGroupFreeItems(order);
			AdjustFreeItemLines();

			Base.RowDeleting.RemoveHandler(SOOrderShipment_RowDeleting);
			Base.RowDeleted.RemoveHandler(SOShipLine_RowDeleted);

			foreach (SOOrderShipment item in Base1.OrderList.Cache.Inserted)
			{
				if (args.ShipmentList == null && item.ShipmentQty == 0m)
				{
					SOShipLine shipline =
						PXSelect>,
							And>,
							And>,
							And>>>>>>.
						SelectSingleBound(Base, null, item.ShipmentType, item.ShipmentNbr, item.OrderType, item.OrderNbr);
					if (shipline == null)
					{
						Base1.OrderList.Delete(item);
					}
				}

				try
				{
					if (args.ShipmentList != null && item.LineCntr > 0 && item.ShipmentQty == 0m && Base.sosetup.Current.AddAllToShipment == true && Base.sosetup.Current.CreateZeroShipments != true)
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
						else if (args.Operation == SOOperation.Issue)
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
						bool cannotShipComplete = false;

						foreach (SOLine2 line in
							PXSelect>,
								And>,
								And>,
								And>,
								And>>>>>>.
							Select(Base, item.OrderType, item.OrderNbr, item.SiteID, item.Operation))
						{
							var original = Base.Caches().GetOriginal(line);
							if (line.LineType == SOLineType.Inventory
								&& line.ShippedQty - original?.ShippedQty == 0m
								&& line.POSource == INReplenishmentSource.PurchaseToOrder)
							{
								PXTrace.WriteError(Messages.MarkForPOItemsNotFullyReceivedTrace, InventoryItem.PK.Find(Base, line.InventoryID)?.InventoryCD, line.LineNbr, INSite.PK.Find(Base, line.SiteID)?.SiteCD);
								anyMarkPONotFullyReceived = true;
							}

							if (line.LineType == SOLineType.Inventory
								&& line.ShippedQty - original?.ShippedQty == 0m
								&& DateTime.Compare((DateTime)line.ShipDate, (DateTime)item.ShipDate) <= 0
								&& line.POSource != INReplenishmentSource.DropShipToOrder)
							{
								cannotShipComplete = true;
							}
						}
						if (anyMarkPONotFullyReceived)
						{
							throw new SOShipmentException(Messages.CannotShipCompleteMarkForPOItemsTraced, order.OrderNbr, order.OrderType);
						}
						if (cannotShipComplete)
						{
							throw new SOShipmentException(Messages.CannotShipCompleteTraced, order.OrderType, order.OrderNbr);
						}
					}
				}
				catch (SOShipmentException)
				{
					//decrement OpenShipmentCntr
					UpdateShipmentCntr(Base1.OrderList.Cache, item, -1);
					//clear ShipmentDeleted flag
					UpdateShipmentCntr(Base1.OrderList.Cache, item, 0);
					throw;
				}
			}

			if (args.Operation == SOOperation.Issue)
			{
				newOrderShipment.LinkShipment(Base.Document.Current, Base);
			}
		}

		/// Overrides 
		[PXOverride]
		public void FillShipLineFromSource(SOShipLine newline, IShipLineSource lineSource,
			Action base_FillShipLineFromSource)
		{
			if (lineSource is not SOLineShipLineSource scheduleShipLineSource)
			{
				base_FillShipLineFromSource(newline, lineSource);
				return;
			}

			var soLine = scheduleShipLineSource.SOLine;
			var soLineSplit = scheduleShipLineSource.SOLineSplit;

			ValidateLineBeforeShipment(soLine);

			base_FillShipLineFromSource(newline, lineSource);

			newline.OrigOrderType = soLine.OrderType;
			newline.OrigOrderNbr = soLine.OrderNbr;
			newline.OrigPlanType = (soLineSplit.POCreate != true && soLineSplit.IsAllocated != true) ? soLineSplit.PlanType : lineSource.PlanType;
			newline.CustomerID = soLine.CustomerID;
			newline.InvtMult = soLine.OrderQty < 0m ? (short?)-soLine.InvtMult : soLine.InvtMult;
			newline.SOLineSign = soLine.LineSign;
			newline.Operation = soLine.Operation;
			newline.LineType = soLine.LineType;
			newline.ReasonCode = soLine.ReasonCode;
			newline.IsFree = soLine.IsFree;
			newline.ManualDisc = soLine.ManualDisc;

			newline.DiscountID = soLine.DiscountID;
			newline.DiscountSequenceID = soLine.DiscountSequenceID;

			newline.AlternateID = soLine.AlternateID;
			newline.BlanketType = soLine.BlanketType;
			newline.BlanketNbr = soLine.BlanketNbr;
			newline.BlanketLineNbr = soLine.BlanketLineNbr;
			newline.BlanketSplitLineNbr = soLine.BlanketSplitLineNbr;

			newline.IsSpecialOrder = soLine.IsSpecialOrder;

			Base.UpdateOrigValues(newline, soLine, lineSource.PlanQty);

			ValidateLineType(soLine, lineSource.InventoryItem, Messages.CannotCreateShipmentNonInventoryNonStockKit);
		}

		/// Overrides 
		///  value of related order type.
		[PXOverride]
		public bool? ShipFullIfNegQtyAllowed(SOShipLine newline,
			Func base_ShipFullIfNegQtyAllowed)
		{
			SOOrderType orderType = Base1.soordertype.Select(newline.OrigOrderType);
			return orderType?.ShipFullIfNegQtyAllowed == true;
		}

		/// Overrides 
		[PXOverride]
		public bool ShouldSaveAfterCreateShipment(CreateShipmentArgs args,
			Func base_ShouldSaveAfterCreateShipment)
		{
			return base_ShouldSaveAfterCreateShipment(args) || Base1.OrderList.Cache.Inserted.Count() > 0 || Base1.OrderList.SelectWindowed(0, 1) != null;
		}

		/// Overrides 
		[PXOverride]
		public void AfterSaveCreateShipment(CreateShipmentArgs args,
			Action base_AfterSaveCreateShipment)
		{
			base_AfterSaveCreateShipment(args);

			var order = args.Order;
			if (order == null)
			{
				return;
			}

			// obtain modified object back.
			if (Base1.soorder.Locate(order) is SOOrder cached)
			{
				bool? selected = args.Order.Selected;
				PXCache.RestoreCopy(args.Order, cached);
				args.Order.Selected = selected;
			}
		}

		public virtual bool ValidateLineBeforeShipment(SOLine line) => true;

		protected virtual SOOrder ActualizeAndValidateOrder(SOOrderEntry orderEntry, SOOrder order, string operation)
		{
			order = Base1.soorder.Select(order.OrderType, order.OrderNbr);
			if (orderEntry == null)
			{
				return order;
			}

			bool? isWorkflowActionEnabled = (operation == SOOperation.Receipt)
				? PX.SM.WorkflowAction.HasWorkflowActionEnabled(orderEntry, g => g.createShipmentReceipt, order)
				: PX.SM.WorkflowAction.HasWorkflowActionEnabled(orderEntry, g => g.createShipmentIssue, order);
			if (isWorkflowActionEnabled == false)
			{
				var action = (operation == SOOperation.Receipt) ? orderEntry.createShipmentReceipt : orderEntry.createShipmentIssue;
				throw new PXInvalidOperationException(Messages.ActionNotAvailableInCurrentState,
					action.GetCaption(), Base1.soorder.Cache.GetRowDescription(order));
			}

			return order;
		}

		protected virtual void SetShipAddressAndContact(SOShipment shipment, int? shipAddressID, int? shipContactID)
		{
			foreach (SOShipmentAddress address in Base.Shipping_Address.Select())
			{
				if (address.AddressID < 0)
				{
					Base.Shipping_Address.Delete(address);
				}
			}

			foreach (SOShipmentContact contact in Base.Shipping_Contact.Select())
			{
				if (contact.ContactID < 0)
				{
					Base.Shipping_Contact.Delete(contact);
				}
			}

			SOAddress soAddress = SOAddress.PK.Find(Base, shipAddressID);
			if (soAddress.IsDefaultAddress == true)
			{
				shipment.ShipAddressID = shipAddressID;
			}
			else
			{
				SOShipmentAddress address = new SOShipmentAddress { };
				AddressAttribute.Copy(address, soAddress);

				address = Base.Shipping_Address.Insert(address);
				shipment.ShipAddressID = address.AddressID;
			}

			SOContact soContact = SOContact.PK.Find(Base, shipContactID);
			if (soContact.IsDefaultContact == true)
			{
				shipment.ShipContactID = shipContactID;
			}
			else
			{
				SOShipmentContact contact = new SOShipmentContact { };
				ContactAttribute.CopyContact(contact, soContact);

				contact = Base.Shipping_Contact.Insert(contact);
				shipment.ShipContactID = contact.ContactID;
			}
		}

		#region Private Classes
		private class ShipmentSchedule : IComparable
		{
			private readonly int sortOrder;
			private readonly int soLineNbr;
			private readonly int splitLineNbr;

			public ShipmentSchedule(PXResult result, SOShipLine shipLine)
			{
				sortOrder = ((SOLine)result).SortOrder.GetValueOrDefault(1000);
				soLineNbr = ((SOLine)result).LineNbr.GetValueOrDefault(int.MaxValue);
				splitLineNbr = ((SOLineSplit)result).SplitLineNbr.GetValueOrDefault(int.MaxValue);
				Result = result;
				ShipLine = shipLine;
			}

			public PXResult Result
			{
				get; private set;
			}

			public SOShipLine ShipLine;

			public int CompareTo(ShipmentSchedule other)
			{
				int compareResult = sortOrder.CompareTo(other.sortOrder);
				if (compareResult == 0)
				{
					compareResult = soLineNbr.CompareTo(other.soLineNbr);
					if (compareResult == 0)
					{
						compareResult = splitLineNbr.CompareTo(other.splitLineNbr);
					}
				}

				return compareResult;
			}
		}

		private class CopySettings : PXNoteAttribute.IPXCopySettings
		{
			public CopySettings(bool? copyFiles, bool? copyNotes)
			{
				CopyFiles = copyFiles;
				CopyNotes = copyNotes;
			}

			public bool? CopyNotes
			{
				get;
			}
			public bool? CopyFiles
			{
				get;
			}
		}
		#endregion
	}
}
