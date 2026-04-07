

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PX.Common;
using PX.Data;
using PX.Objects.CS;
using PX.Objects.IN;
using PX.Objects.IN.InventoryRelease.Accumulators.QtyAllocated;
using PX.Objects.SO.Interfaces;

namespace PX.Objects.SO.GraphExtensions.SOShipmentEntryExt
{
	/// 

	/// An extension of the  graph that handles logic related to the creation of shipments.
	/// The extension includes logic related to only the  entity.
	/// 

	// Acuminator disable once PX1016 ExtensionDoesNotDeclareIsActiveMethod extension should be constantly active
	public class CreateShipmentExtension : PXGraphExtension
	{
		public virtual void CreateShipment(CreateShipmentArgs args)
		{
			SiteLotSerial.AccumulatorAttribute.ForceAvailQtyValidation(Base, true);
			ItemLotSerial.AccumulatorAttribute.ForceAvailQtyValidation(Base, true);

			if (args.QuickProcessFlow != PXQuickProcess.ActionFlow.NoFlow)
			{
				Base.sosetup.Current.HoldShipments = false;
			}

			ValidateCreateShipmentArgs(args);

			SOShipment newdoc;
			bool newlyCreated;
			if (args.ShipmentList != null)
			{
				Base.Clear();

				newdoc = FindOrCreateShipment(args);

				newlyCreated = newdoc.ShipmentNbr == null;
				if (newlyCreated)
				{
					newdoc = Base.Document.Insert(newdoc);
				}
				else
				{
					Base.Document.Current = Base.Document.Search(newdoc.ShipmentNbr);
					if (Base.Document.Current.Confirmed == true)
					{
						throw new PXException(IN.Messages.Document_Status_Invalid);
					}
				}
			}
			else
			{
				newdoc = PXCache.CreateCopy(Base.Document.Current);
				newlyCreated = newdoc.OrderCntr == 0;
			}

			bool updatedFromOrder = SetShipmentFieldsFromOrigDocument(newdoc, args, newlyCreated);
			if (newlyCreated)
			{
				SetShipAddressAndContactFromArgs(newdoc, args);
			}
			if (newlyCreated || updatedFromOrder)
			{
				Base.Document.Update(newdoc);
			}

			CreateShipmentDetails(args);

			if (args.FilesAndNotesSource != null && args.CopyNotesAndFilesSettings != null)
			{
				PXNoteAttribute.CopyNoteAndFiles(
					Base.Caches[args.FilesAndNotesSource.GetType()], args.FilesAndNotesSource,
					Base.Document.Cache, Base.Document.Current,
					PXNoteAttribute.GetNote(Base.Document.Cache, Base.Document.Current) == null && (args.CopyNotesAndFilesSettings.CopyNotes ?? false), args.CopyNotesAndFilesSettings.CopyFiles);
			}

			if (args.ShipmentList != null)
			{
				if (ShouldSaveAfterCreateShipment(args))
				{
					using (new SOShipmentEntry.SkipShipCompleteValidationScope()) // Ship-Complete rule has already been validated.
						Base.Save.Press();

					AfterSaveCreateShipment(args);
				}
			}

			ItemLotSerial.AccumulatorAttribute.ForceAvailQtyValidation(Base, false);
			SiteLotSerial.AccumulatorAttribute.ForceAvailQtyValidation(Base, false);
		}

		protected virtual void ValidateCreateShipmentArgs(CreateShipmentArgs args)
		{
			args.ShipDate = GetShipmentDate(args);
			args.EndDate = args.EndDate ?? args.ShipDate;
		}

		/// 

		/// Returns the date of the Shipment
		/// 

		protected virtual DateTime? GetShipmentDate(CreateShipmentArgs args)
		{
			return args.ShipDate;
		}

		protected virtual SOShipment FindOrCreateShipment(CreateShipmentArgs args)
		{
			return args.ShipmentList.Find(GetShipmentFieldLookups(args))
				?? new SOShipment();
		}

		protected virtual FieldLookup[] GetShipmentFieldLookups(CreateShipmentArgs args)
		{
			return new FieldLookup[]
			{
				new FieldLookup(args.ShipDate),
				new FieldLookup(args.SiteID),
				new FieldLookup(args.ShipmentType),
				new FieldLookup(false),
			};
		}

		protected virtual bool SetShipmentFieldsFromOrigDocument(SOShipment shipment, CreateShipmentArgs args, bool newlyCreated)
		{
			if (newlyCreated)
			{
				shipment.SiteID = args.SiteID;
				shipment.ShipmentType = args.ShipmentType;
				shipment.Operation = args.Operation;
				shipment.ShipDate = args.ShipDate;

				return true;
			}
			else
			{
				return false;
			}
		}

		public virtual bool CreateShipmentFromSchedules(CreateShipmentArgs args, IShipLineSource res, SOShipLine newline)
		{
			bool deleted = false;

			IShipLineSource plan = res;

			bool requireAllocationUnallocated = res.RequireAllocationUnallocated;
			bool addZeroLineForUnallocated = requireAllocationUnallocated && Base.sosetup.Current.AddAllToShipment == true;

			if (plan.Selected == true || args.ShipmentList != null && (!requireAllocationUnallocated || Base.sosetup.Current.AddAllToShipment == true))
			{
				FillShipLineFromSource(newline, res);

				INLotSerClass lotSerClass = res.INLotSerClass;
				bool isNonStock = lotSerClass.LotSerTrack == null;
				if (isNonStock)
				{
					newline.ShippedQty = (newline.UOM == newline.OrderUOM && plan.PlanQty == newline.BaseFullOrderQty) ? newline.FullOrderQty
						: INUnitAttribute.ConvertFromBase(Base.Transactions.Cache, newline.InventoryID, newline.UOM, (decimal)plan.PlanQty, INPrecision.QUANTITY);
					newline = Base.LineSplittingExt.InsertWithoutSplits(newline);

					try
					{
						ShipAvailable(plan, newline, new PXResult(res.InventoryItem, res.INLotSerClass));
					}
					catch (PXException ex)
					{
						Base.LineSplittingExt.lsselect.Delete(newline);
						throw ex;
					}
				}
				else if (args.Operation == SOOperation.Receipt)
				{
					newline.ShippedQty = (newline.UOM == newline.OrderUOM && plan.PlanQty == newline.BaseFullOrderQty) ? newline.FullOrderQty
						: INUnitAttribute.ConvertFromBase(Base.Transactions.Cache, newline.InventoryID, newline.UOM, (decimal)plan.PlanQty, INPrecision.QUANTITY);
					newline.LocationID = res.INSite?.ReturnLocationID;
					if (newline.LocationID == null && args.ShipmentList != null)
					{
						throw new PXException(Messages.NoRMALocation, res.INSite?.SiteCD);
					}
					newline = Base.Transactions.Insert(newline);
					ReceiveLotSerial(plan, newline, new PXResult(res.InventoryItem, res.INLotSerClass));
				}
				else
				{
					SOShipLine existing = (SOShipLine)Base.Transactions.Cache.Locate(newline);
					if (existing == null || Base.Transactions.Cache.GetStatus(existing).IsIn(PXEntryStatus.Deleted, PXEntryStatus.InsertedDeleted))
					{
						newline.ShippedQty = 0m;
						newline = Base.LineSplittingExt.InsertWithoutSplits(newline);
					}
					if (!addZeroLineForUnallocated)
					{
						newline.IsUnassigned = lotSerClass.IsManualAssignRequired == true && plan.PlanQty > 0 && string.IsNullOrEmpty(plan.LotSerialNbr) &&
							(lotSerClass.LotSerAssign != INLotSerAssign.WhenUsed || newline.ShipmentType != SOShipmentType.Transfer && newline.IsIntercompany != true);

						decimal? notShipped = ShipAvailable(plan, newline, new PXResult(res.InventoryItem, res.INLotSerClass));
						if (newline.IsUnassigned == true)
						{
							var oldRow = (SOShipLine)Base.Transactions.Cache.CreateCopy(newline);
							newline.UnassignedQty = plan.PlanQty - notShipped;
							newline.BaseShippedQty = plan.PlanQty - notShipped;
							newline.ShippedQty = (newline.UOM == newline.OrderUOM && newline.BaseShippedQty == newline.BaseFullOrderQty) ? newline.FullOrderQty
								: INUnitAttribute.ConvertFromBase(Base.unassignedSplits.Cache, newline.InventoryID, newline.UOM, (decimal)newline.BaseShippedQty, INPrecision.QUANTITY);

							using (Base.LineSplittingExt.SuppressedModeScope(true))
							{
								Base.Transactions.Cache.RaiseFieldUpdated(newline, oldRow.ShippedQty);
								Base.Transactions.Cache.RaiseRowUpdated(newline, oldRow);
							}
						}
					}
				}

				if (newline.BaseShippedQty < plan.PlanQty && string.IsNullOrEmpty(plan.LotSerialNbr) && !addZeroLineForUnallocated)
				{
					PromptReplenishment(Base.Transactions.Cache, newline, res.InventoryItem, plan);
				}

				if (newline.ShippedQty == 0m)
				{
					deleted = RemoveLineFromShipment(newline, args.ShipmentList != null && Base.sosetup.Current.AddAllToShipment == false);
				}

				if (newline.BaseShippedQty < res.MinRequiredBaseShippedQty && res.ShippingRule == SOShipComplete.ShipComplete)
				{
					deleted = RemoveLineFromShipment(newline, args.ShipmentList != null);
				}

				if (!deleted && res.FilesAndNotesSource != null && args.CopyLineNotesAndFilesSettings != null)
				{
					PXNoteAttribute.CopyNoteAndFiles(Base.Caches[res.FilesAndNotesSource.GetType()], res.FilesAndNotesSource, Base.Caches[typeof(SOShipLine)], newline, args.CopyLineNotesAndFilesSettings);
				}

				if (!deleted && !addZeroLineForUnallocated && plan.RequireINItemPlanUpdate)
				{
					INItemPlan actualPlan = PXSelect>>>.Select(Base, plan.PlanID);
					if (actualPlan != null)
					{
						actualPlan.PlanType = plan.NewPlanType;
						Base.Caches[typeof(INItemPlan)].Update(actualPlan);
					}
				}
			}
			return deleted;
		}

		protected virtual void FillShipLineFromSource(SOShipLine newline, IShipLineSource line)
		{
			newline.OrigLineNbr = line.LineNbr;

			newline.IsStockItem = line.IsStockItem;
			newline.InventoryID = line.InventoryID;
			newline.SubItemID = line.SubItemID;
			newline.SiteID = line.SiteID;
			newline.TranDesc = line.TranDesc;

			newline.ProjectID = line.ProjectID;
			newline.TaskID = line.TaskID;
			newline.CostCodeID = line.CostCodeID;

			newline.UOM = line.UOM;

			newline.CostCenterID = line.CostCenterID;
		}

		public virtual decimal? ShipAvailable(IShipLineSource plan, SOShipLine newline, PXResult item)
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

				copy = PXCache.CreateCopy(newline);
				copy.ShippedQty = (copy.UOM == copy.OrderUOM && kitqty == copy.BaseFullOrderQty) ? copy.FullOrderQty
					: INUnitAttribute.ConvertFromBase(Base.Transactions.Cache, copy, copy.UOM, (decimal)kitqty, INPrecision.QUANTITY, INMidpointRounding.FLOOR);
				Base.LineSplittingExt.LastComponentID = (int?)lastComponentID;
				try
				{
					Base.Transactions.Update(copy);
				}
				finally
				{
					Base.LineSplittingExt.LastComponentID = null;
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

		public virtual void ReceiveLotSerial(IShipLineSource plan, SOShipLine newline, PXResult item)
		{
			PXSelectBase cmd =
				new PXSelectReadonly2>,
				Where>,
					And>,
					And>,
					And>,
					And>>>>>>(Base);

			if (!string.IsNullOrEmpty(plan.LotSerialNbr))
			{
				cmd.WhereAnd>>>();
			}

			INLotSerialStatusByCostCenter avail = cmd.SelectWindowed(0, 1, newline.InventoryID, newline.SubItemID, newline.SiteID, newline.CostCenterID, plan.LotSerialNbr);

			SOShipLineSplit newsplit = SOShipLineSplit.FromSOShipLine(newline);
			newsplit.UOM = null;
			newsplit.Qty = newsplit.BaseQty;
			newsplit.SplitLineNbr = null;
			if (avail != null)
			{
				if (newsplit.LocationID == null)
				{
					newsplit.LocationID = avail.LocationID;
				}
				newsplit.LotSerialNbr = avail.LotSerialNbr;
				newsplit.ExpireDate = plan.ExpireDate ?? avail.ExpireDate;
			}
			else
			{
				INSite site = INSite.PK.Find(Base, newline.SiteID);
				newsplit.LocationID = site.ReturnLocationID;
				newsplit.LotSerialNbr = plan.LotSerialNbr;
				newsplit.ExpireDate = plan.ExpireDate ?? newline.ExpireDate;
			}

			if (!string.IsNullOrEmpty(plan.LotSerialNbr))
			{
				Base.splits.Update(newsplit);
			}
		}

		public virtual void PromptReplenishment(PXCache sender, SOShipLine newline, InventoryItem item, IShipLineSource plan)
		{
			if (newline.ProjectID != null && newline.TaskID != null)
			{
				// we can't prompt replenishment reliably for lines assigned to project and task
				return;
			}

			decimal planrequired = (plan.PlanQty ?? 0m) - newline.BaseShippedQty.GetValueOrDefault();
			decimal qtyrequired = planrequired;

			if (item.StkItem == false && item.KitItem == true)
			{
				if (newline.ShipComplete != SOShipComplete.ShipComplete)
				{
					//if it's not shipcomplete than we must check if we can assemble at least one non-stock kit
					qtyrequired = 1;
				}

				List itemsNotAvailable = new List();
				decimal? maxPromptQty = null;

				foreach (PXResult compres in
					PXSelectJoin>>,
					Where>>>.
					Select(Base, newline.InventoryID))
				{
					INKitSpecStkDet spec = (INKitSpecStkDet)compres;

					if (spec.DfltCompQty.GetValueOrDefault() == 0)
					{
						continue;
					}

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
						{
							maxPromptQty = possibleQty;
						}
					}
				}
				if (maxPromptQty <= 0m)
				{
					return;
				}

				foreach (PXResult compres in
					PXSelectJoin>>,
					Where>>>.
					Select(Base, newline.InventoryID))
				{
					INKitSpecStkDet spec = (INKitSpecStkDet)compres;
					if (spec.DfltCompQty.GetValueOrDefault() == 0)
					{
						continue;
					}

					Tuple availability = CalculateItemAvailability(spec.CompInventoryID, spec.CompSubItemID, newline.SiteID, newline.CostCenterID);

					if (availability.Item2 < (maxPromptQty * spec.DfltCompQty))
					{
						itemsNotAvailable.Add((InventoryItem)compres);
					}
				}

				if (itemsNotAvailable.Count == 0)
				{
					return;
				}

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

				if (newline.ShipComplete != SOShipComplete.ShipComplete)
				{
					//if it's not shipcomplete than we must throw error if we can ship at least smthing more
					qtyrequired = 0m;
				}

				//actually it's a error, but it will be thrown further
				if (qtyrequired > availability.Item1)
				{
					return;
				}

				if (availability.Item1 > 0m)
				{
					throw new PXException(Messages.PromptReplenishment, sender.GetValueExt(newline));
				}
			}
		}

		public virtual bool RemoveLineFromShipment(SOShipLine shipline, bool removeFlag)
		{
			if (removeFlag)
			{
				if (shipline.CostCenterID != CostCenter.FreeStock && INCostCenter.PK.Find(Base, shipline.CostCenterID)?.CostLayerType == CostLayerType.Special)
				{
					PXTrace.WriteInformation(Messages.ItemNotAvailableTraced_Special, Base.Transactions.GetValueExt(shipline), Base.Transactions.GetValueExt(shipline));
				}
				else if (PXAccess.FeatureInstalled() && shipline != null && shipline.SubItemID != null)
				{
					PXTrace.WriteInformation(Messages.ItemWithSubitemNotAvailableTraced, Base.Transactions.GetValueExt(shipline), Base.Transactions.GetValueExt(shipline), Base.Transactions.GetValueExt(shipline));
				}
				else
				{
					PXTrace.WriteInformation(Messages.ItemNotAvailableTraced, Base.Transactions.GetValueExt(shipline), Base.Transactions.GetValueExt(shipline));
				}
				shipline.KeepManualFreight = true;
				Base.Transactions.Delete(shipline);
				return true;
			}

			Base.Transactions.Cache.RaiseExceptionHandling(shipline, null, new PXSetPropertyException(shipline, Messages.ItemNotAvailable, PXErrorLevel.RowWarning));
			return false;
		}

		public virtual void ShipNonStockKit(IShipLineSource plan, SOShipLine newline, ref decimal? kitqty, ref object lastComponentID, ref bool HasSerialComponents)
		{
			SOShipLine newLineCopy;
			object lastSubitemID = null;

			using (Base.LineSplittingExt.KitProcessingScope(InventoryItem.PK.Find(Base, newline.InventoryID)))
			{
				foreach (PXResult compres in
					PXSelectJoin,
					InnerJoin>>,
					Where>>>.
					Select(Base, newline.InventoryID))
				{
					(INKitSpecStkDet compitem, InventoryItem component) = compres;

					if (component.ItemStatus == INItemStatus.Inactive)
					{
						throw new PXException(Messages.KitComponentIsInactive, component.InventoryCD);
					}
					newLineCopy = Base.LineSplittingExt.Clone(newline);

					newLineCopy.IsStockItem = true;
					newLineCopy.InventoryID = compitem.CompInventoryID;
					newLineCopy.SubItemID = compitem.CompSubItemID;
					newLineCopy.UOM = compitem.UOM;
					newLineCopy.Qty = compitem.DfltCompQty * plan.PlanQty;

					//clear splits with correct ComponentID
					Base.LineSplittingExt.RaiseRowDeleted(newLineCopy);

					IShipLineSource plancopy = (IShipLineSource)plan.Clone();
					plancopy.PlanQty = INUnitAttribute.ConvertToBase(Base.Transactions.Cache, newLineCopy, newLineCopy.UOM, (decimal)newLineCopy.Qty, INPrecision.QUANTITY);
					if (newLineCopy.Operation == SOOperation.Receipt)
					{
						INSite site = INSite.PK.Find(Base, newLineCopy.SiteID);
						if (site != null)
						{
							if (site.ReturnLocationID == null)
							{
								throw new PXException(Messages.NoRMALocation, site.SiteCD);
							}

							if (((INLotSerClass)compres).LotSerTrack == INLotSerTrack.SerialNumbered)
							{
								for (int i = 0; i < newLineCopy.Qty; i++)
								{
									SOShipLineSplit newsplit = SOShipLineSplit.FromSOShipLine(newLineCopy);
									newsplit.Qty = 1;
									newsplit.SplitLineNbr = null;
									newsplit.LocationID = site.ReturnLocationID;
									newsplit = Base.splits.Insert(newsplit);
									PXDefaultAttribute.SetPersistingCheck(Base.splits.Cache, newsplit, PXPersistingCheck.Nothing);
									PXDefaultAttribute.SetPersistingCheck(Base.splits.Cache, newsplit, PXPersistingCheck.Nothing);
								}
							}
							else
							{
								SOShipLineSplit newsplit = SOShipLineSplit.FromSOShipLine(newLineCopy);
								newsplit.SplitLineNbr = null;
								newsplit.LocationID = site.ReturnLocationID;
								newsplit = Base.splits.Insert(newsplit);
								PXDefaultAttribute.SetPersistingCheck(Base.splits.Cache, newsplit, PXPersistingCheck.Nothing);
								PXDefaultAttribute.SetPersistingCheck(Base.splits.Cache, newsplit, PXPersistingCheck.Nothing);
							}
						}
					}
					else
					{
						decimal? unshippedqty = ShipAvailable(plancopy, newLineCopy, new PXResult(compres, compres));

						if (plancopy.PlanQty != 0m && (plancopy.PlanQty - unshippedqty) * plan.PlanQty / plancopy.PlanQty < kitqty)
						{
							kitqty = (plancopy.PlanQty - unshippedqty) * plan.PlanQty / plancopy.PlanQty;
							lastComponentID = newLineCopy.InventoryID;
							lastSubitemID = newLineCopy.SubItemID;
						}
					}
					HasSerialComponents |= ((INLotSerClass)compres).LotSerTrack == INLotSerTrack.SerialNumbered;
				}
			}

			foreach (PXResult compres in
				PXSelectJoin>,
				Where>,
					And, Or>>>>>.
				Select(Base, newline.InventoryID))
			{
				(INKitSpecNonStkDet compitem, InventoryItem item)= compres;

				newLineCopy = Base.LineSplittingExt.Clone(newline);

				newLineCopy.IsStockItem = false;
				newLineCopy.InventoryID = compitem.CompInventoryID;
				newLineCopy.SubItemID = null;
				newLineCopy.UOM = compitem.UOM;
				newLineCopy.Qty = compitem.DfltCompQty * plan.PlanQty;

				//clear splits with correct ComponentID
				Base.LineSplittingExt.RaiseRowDeleted(newLineCopy);

				IShipLineSource plancopy = (IShipLineSource)plan.Clone();
				plancopy.PlanQty = INUnitAttribute.ConvertToBase(Base.Transactions.Cache, newLineCopy, newLineCopy.UOM, (decimal)newLineCopy.Qty, INPrecision.QUANTITY);

				if (item.StkItem == false && item.KitItem == true)
				{
					decimal? subkitqty = plancopy.PlanQty;

					ShipNonStockKit(plancopy, newLineCopy, ref subkitqty, ref lastComponentID, ref HasSerialComponents);

					if (plancopy.PlanQty != 0m && subkitqty * plan.PlanQty / plancopy.PlanQty < kitqty)
					{
						kitqty = subkitqty * plan.PlanQty / plancopy.PlanQty;
					}
				}
				else
				{
					ShipAvailable(plancopy, newLineCopy, new PXResult(compres, null));
				}
			}

			if (HasSerialComponents)
			{
				kitqty = decimal.Floor((decimal)kitqty);
			}

			if (kitqty <= 0m &&
				lastComponentID != null)
			{
				object lastComponentCD = lastComponentID;
				object lastSubitemCD = lastSubitemID;

				Base.Transactions.Cache.RaiseFieldSelecting(newline, ref lastComponentCD, true);
				Base.Transactions.Cache.RaiseFieldSelecting(newline, ref lastSubitemCD, true);

				if (PXAccess.FeatureInstalled() && lastSubitemID != null)
				{
					PXTrace.WriteInformation(Messages.ItemWithSubitemNotAvailableTraced, lastComponentCD, Base.Transactions.GetValueExt(newline), lastSubitemCD);
				}
				else
				{
					PXTrace.WriteInformation(Messages.ItemNotAvailableTraced, lastComponentCD, Base.Transactions.GetValueExt(newline));
				}
			}
		}

		public virtual decimal? ShipNonStock(IShipLineSource plan, SOShipLine newline)
		{
			decimal? PlannedQty = plan.PlanQty;

			SOShipLineSplit newsplit = SOShipLineSplit.FromSOShipLine(newline);
			newsplit.UOM = null;
			newsplit.SplitLineNbr = null;
			newsplit.LocationID = INSite.PK.Find(Base, newsplit.SiteID)?.NonStockPickingLocationID;
			newsplit.Qty = PlannedQty;
			newsplit.BaseQty = null;
			Base.splits.Insert(newsplit);

			return 0m;
		}

		public virtual decimal? ShipAvailableNonLots(IShipLineSource plan, SOShipLine newline, INLotSerClass lotserclass)
		{
			return CreateSplitsForAvailableNonLots(plan.PlanQty, plan.PlanType, newline, lotserclass);
		}

		public virtual decimal? ShipAvailableLots(IShipLineSource plan, SOShipLine newline, INLotSerClass lotserclass)
		{
			return CreateSplitsForAvailableLots(plan.PlanQty, plan.PlanType, plan.LotSerialNbr, newline, lotserclass);
		}

		private Tuple CalculateItemAvailability(int? inventoryID, int? subItemID, int? siteID, int? costCenterID)
		{
			decimal totalAvalableQty = 0;
			decimal totalAvalableForSalesQty = 0;

			INSiteStatusByCostCenter sitestatus =
				PXSelectReadonly>,
					And>,
					And>,
					And>,
						Or, IsNull>>>>>>>.
				SelectSingleBound(Base, new object[] { }, inventoryID, siteID, costCenterID, subItemID, subItemID);

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

			var select =
				new PXSelectReadonly2>,
				Where>,
					And>,
					And>,
					And>>>>,
				OrderBy>>(Base);

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
				accumavail = (LocationStatusByCostCenter)Base.Caches[typeof(LocationStatusByCostCenter)].Insert(accumavail);

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

		public virtual decimal? CreateSplitsForAvailableNonLots(
			decimal? plannedQty, string origPlanType,
			SOShipLine newline, INLotSerClass lotserclass)
		{
			List resultset = SelectLocationStatus(newline);
			ResortStockForShipment(newline, resultset);

			bool isFullLineAllocation = plannedQty >= newline.BaseShippedQty;
			int locCounter = 0;
			int? assignedLocation = null;
			int? assignedTaskID = null;
			PXCache lcache = Base.Caches[typeof(INLocationStatusByCostCenter)];
			PXCache scache = Base.Caches[typeof(INSiteStatusByCostCenter)];
			foreach (PXResult available in resultset)
			{
				var location = (INLocation)available;
				if (locCounter > 0 && newline.TaskID != null && assignedTaskID != location.TaskID)
				{
					continue;
				}

				INLocationStatusByCostCenter avail = (INLocationStatusByCostCenter)available;
				LocationStatusByCostCenter accumavail = new LocationStatusByCostCenter();
				lcache.RestoreCopy(accumavail, avail);

				INSiteStatusByCostCenter siteavail = (INSiteStatusByCostCenter)available;
				SiteStatusByCostCenter accumsiteavail = new SiteStatusByCostCenter();
				scache.RestoreCopy(accumsiteavail, siteavail);

				accumavail = (LocationStatusByCostCenter)Base.Caches[typeof(LocationStatusByCostCenter)].Insert(accumavail);
				accumsiteavail = (SiteStatusByCostCenter)Base.Caches[typeof(SiteStatusByCostCenter)].Insert(accumsiteavail);

				decimal? availableQty = avail.QtyHardAvail + accumavail.QtyHardAvail;
				decimal? siteAvailableQty = siteavail.QtyHardAvail + accumsiteavail.QtyHardAvail;

				//We should not check INSiteStatus for allocated lines
				availableQty = (siteAvailableQty < availableQty && !INPlanConstants.IsAllocated(origPlanType)) //origPlanType.IsIn(INPlanConstants.Plan61, INPlanConstants.Plan63))
					? siteAvailableQty : availableQty;

				if (availableQty <= 0m)
				{
					continue;
				}

				InsertSplitsForNonLotsOnLocation(newline, lotserclass, location.LocationID, availableQty, plannedQty);

				if (locCounter == 0)
				{
					if (newline.TaskID != null)
					{
						assignedTaskID = location.TaskID;
					}
					assignedLocation = location.LocationID;
				}
				else if (assignedLocation != location.LocationID)
				{
					assignedLocation = null;
				}
				locCounter++;

				if (availableQty < plannedQty)
				{
					plannedQty -= availableQty;
				}
				else
				{
					plannedQty = 0m;
					break;
				}
			}

			if (plannedQty > 0m && (lotserclass.LotSerTrack == INLotSerTrack.NotNumbered || lotserclass.LotSerAssign == INLotSerAssign.WhenUsed))
			{
				InventoryItem item = InventoryItem.PK.Find(Base, newline.InventoryID);
				if (item?.NegQty == true && ShipFullIfNegQtyAllowed(newline) == true)
				{
					int? locationID = GetLocationIDForNotAvailableStock(item, newline.SiteID);
					if (locationID == null)
					{
						throw new PXException(Messages.NegShipmentCantBeCreatedLocationNotSetup, item.InventoryCD);
					}

					bool addNegQtyLocation = true;
					if (locCounter > 0)
					{
						INLocation location = INLocation.PK.Find(Base, locationID);
						addNegQtyLocation = (location?.TaskID == assignedTaskID);
					}

					if (addNegQtyLocation)
					{
						InsertSplitsForNonLotsOnLocation(newline, lotserclass, locationID, plannedQty, plannedQty);
						plannedQty = 0m;
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

			if (newline.IsUnassigned == true && isFullLineAllocation && assignedLocation != null)
			{
				/// for assigned lines the location is set by 
				Base.Transactions.Cache.SetValue(newline, assignedLocation);
			}

			return plannedQty;
		}

		public virtual decimal? CreateSplitsForAvailableLots(
			decimal? plannedQty, string origPlanType, string origLotSerialNbr,
			SOShipLine newline, INLotSerClass lotserclass)
		{
			if (lotserclass.LotSerTrack == INLotSerTrack.SerialNumbered)
			{
				plannedQty = Math.Floor((decimal)plannedQty);
			}

			List resultset = SelectLotSerialStatus(origLotSerialNbr, newline, lotserclass);
			ResortStockForShipment(newline, resultset);

			PXCache lcache = Base.Caches[typeof(INLotSerialStatusByCostCenter)];
			PXCache scache = Base.Caches[typeof(INSiteStatusByCostCenter)];
			PXCache tcache = Base.Caches[typeof(INSiteLotSerial)];

			bool isFullLineAllocation = (plannedQty >= newline.BaseShippedQty);
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

					INLotSerialStatusByCostCenter avail = (INLotSerialStatusByCostCenter)available;
					INSiteLotSerial siteLotAvail = (INSiteLotSerial)available;

					LotSerialStatusByCostCenter accumavail = new LotSerialStatusByCostCenter();
					lcache.RestoreCopy(accumavail, avail);

					SiteLotSerial accumSiteLotAvail = new SiteLotSerial();
					tcache.RestoreCopy(accumSiteLotAvail, siteLotAvail);

					accumSiteLotAvail = (SiteLotSerial)Base.Caches[typeof(SiteLotSerial)].Insert(accumSiteLotAvail);

					accumavail = (LotSerialStatusByCostCenter)Base.Caches[typeof(LotSerialStatusByCostCenter)].Insert(accumavail);

					INSiteStatusByCostCenter siteavail = (INSiteStatusByCostCenter)available;
					SiteStatusByCostCenter accumsiteavail = new SiteStatusByCostCenter();
					scache.RestoreCopy(accumsiteavail, siteavail);
					accumsiteavail = (SiteStatusByCostCenter)Base.Caches[typeof(SiteStatusByCostCenter)].Insert(accumsiteavail);

					decimal? availableQty = 0m;

					decimal? siteLotAvailableQty = siteLotAvail.QtyHardAvail + accumSiteLotAvail.QtyHardAvail;
					decimal? statusAvailableQty = avail.QtyHardAvail + accumavail.QtyHardAvail;
					decimal? siteAvailableQty = siteavail.QtyHardAvail + accumsiteavail.QtyHardAvail;

					//We should not check INSiteStatus for allocated lines
					if (!INPlanConstants.IsAllocated(origPlanType))// origPlanType.IsIn(INPlanConstants.Plan61, INPlanConstants.Plan63, INPlanConstants.PlanM7))
					{
						availableQty = Math.Min(siteAvailableQty.GetValueOrDefault(), Math.Min(siteLotAvailableQty.GetValueOrDefault(), statusAvailableQty.GetValueOrDefault()));
					}
					else
					{
						availableQty = Math.Min(siteLotAvailableQty.GetValueOrDefault(), statusAvailableQty.GetValueOrDefault());
					}

					if (availableQty <= 0m)
					{
						continue;
					}

					IBqlTable newsplit = (newline.IsUnassigned == true) ? newline.ToUnassignedSplit() : SOShipLineSplit.FromSOShipLine(newline);
					PXCache cache = (newline.IsUnassigned == true) ? Base.unassignedSplits.Cache : Base.splits.Cache;

					cache.SetValue(newsplit, null);
					cache.SetValue(newsplit, null);
					cache.SetValue(newsplit, avail.LocationID);
					cache.SetValue(newsplit, newline.IsUnassigned == true ? string.Empty : avail.LotSerialNbr);
					cache.SetValue(newsplit, avail.ExpireDate);
					cache.SetValue(newsplit, newline.IsUnassigned);
					cache.SetValue(newsplit, (availableQty < plannedQty) ? availableQty : plannedQty);
					cache.SetValue(newsplit, null);
					cache.Insert(newsplit);

					if (locCounter == 0)
					{
						if (newline.TaskID != null)
						{
							assignedTaskID = location.TaskID;
						}
						assignedLocation = location.LocationID;
					}
					else if (assignedLocation != location.LocationID)
					{
						assignedLocation = null;
					}
					locCounter++;

					if (availableQty < plannedQty)
					{
						plannedQty -= availableQty;
					}
					else
					{
						plannedQty = 0m;
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

					INLotSerialStatusByCostCenter avail = (INLotSerialStatusByCostCenter)available;
					LotSerialStatusByCostCenter accumavail = new LotSerialStatusByCostCenter();
					lcache.RestoreCopy(accumavail, avail);

					INSiteStatusByCostCenter siteavail = (INSiteStatusByCostCenter)available;
					SiteStatusByCostCenter accumsiteavail = new SiteStatusByCostCenter();
					scache.RestoreCopy(accumsiteavail, siteavail);

					accumavail = (LotSerialStatusByCostCenter)Base.Caches[typeof(LotSerialStatusByCostCenter)].Insert(accumavail);
					accumsiteavail = (SiteStatusByCostCenter)Base.Caches[typeof(SiteStatusByCostCenter)].Insert(accumsiteavail);

					decimal? availableQty = avail.QtyHardAvail + accumavail.QtyHardAvail;
					decimal? siteAvailableQty = siteavail.QtyHardAvail + accumsiteavail.QtyHardAvail;

					//We should not check INSiteStatus for allocated lines
					availableQty = (siteAvailableQty < availableQty && !INPlanConstants.IsAllocated(origPlanType))//origPlanType.IsIn(INPlanConstants.Plan61, INPlanConstants.Plan63))
						? siteAvailableQty : availableQty;

					if (availableQty <= 0m)
					{
						continue;
					}

					IBqlTable newsplit = (newline.IsUnassigned == true) ? newline.ToUnassignedSplit() : SOShipLineSplit.FromSOShipLine(newline);
					PXCache cache = (newline.IsUnassigned == true) ? Base.unassignedSplits.Cache : Base.splits.Cache;

					cache.SetValue(newsplit, null);
					cache.SetValue(newsplit, null);
					cache.SetValue(newsplit, avail.LocationID);
					cache.SetValue(newsplit, avail.LotSerialNbr);
					cache.SetValue(newsplit, avail.ExpireDate);
					cache.SetValue(newsplit, newline.IsUnassigned);
					cache.SetValue(newsplit, (availableQty < plannedQty) ? availableQty : plannedQty);
					cache.SetValue(newsplit, null);
					cache.Insert(newsplit);

					if (locCounter == 0)
					{
						if (newline.TaskID != null)
						{
							assignedTaskID = location.TaskID;
						}
						assignedLocation = location.LocationID;
					}
					else if (assignedLocation != location.LocationID)
					{
						assignedLocation = null;
					}
					locCounter++;

					if (availableQty < plannedQty)
					{
						plannedQty -= availableQty;
					}
					else
					{
						plannedQty = 0m;
						break;
					}
				}
			}

			if (newline.IsUnassigned == true && isFullLineAllocation && assignedLocation != null)
			{
				/// for assigned lines the location is set by 
				Base.Transactions.Cache.SetValue(newline, assignedLocation);
			}

			return plannedQty;
		}

		protected virtual List SelectLocationStatus(SOShipLine newline)
		{
			var select =
				new PXSelectReadonly2,
				LeftJoin,
					And,
					And,
					And>>>>>>,
				Where>,
					And>,
					And>,
					And,
					And>>>>>,
				OrderBy>>>(Base);

			var pars = new List(capacity: 8) { newline.InventoryID, newline.SiteID, newline.CostCenterID };
			if (PXAccess.FeatureInstalled())
			{
				select.WhereAnd>>>();
				pars.Add(newline.SubItemID);
			}

			AppendFiltersForStatusSelect(newline, select, pars);

			return select.Select(pars.ToArray()).AsEnumerable().Cast().ToList();
		}

		protected virtual void ResortStockForShipment(SOShipLine newline, List resultset)
		{
			ResortStockForShipmentByDefaultItemLocation(newline, resultset);
			ResortStockForShipmentByProjectAndTask(newline, resultset);
		}

		public virtual void InsertSplitsForNonLotsOnLocation(SOShipLine newline, INLotSerClass lotserclass, int? locationID, decimal? availableQty, decimal? plannedQty)
		{
			IBqlTable newsplit = (newline.IsUnassigned == true) ? newline.ToUnassignedSplit() : SOShipLineSplit.FromSOShipLine(newline);
			PXCache cache = (newline.IsUnassigned == true) ? Base.unassignedSplits.Cache : Base.splits.Cache;

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
				(lotserclass.LotSerAssign != INLotSerAssign.WhenUsed ||
					newline.ShipmentType != SOShipmentType.Transfer && newline.IsIntercompany != true))
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

		protected virtual bool? ShipFullIfNegQtyAllowed(SOShipLine newline)
		{
			return true;
		}

		public virtual int? GetLocationIDForNotAvailableStock(InventoryItem item, int? siteID)
		{
			var itemSite = (INItemSite)
				PXSelectReadonly>,
					And>>>>.
				Select(Base, item.InventoryID, siteID);
			if (itemSite?.DfltShipLocationID != null)
			{
				return itemSite.DfltShipLocationID;
			}

			var site = INSite.PK.Find(Base, siteID);
			InventoryItemCurySettings itemCurySettings = InventoryItemCurySettings.PK.Find(Base, item.InventoryID, site?.BaseCuryID);

			if (itemCurySettings?.DfltSiteID == siteID && itemCurySettings.DfltShipLocationID != null)
			{
				return itemCurySettings.DfltShipLocationID;
			}

			return site?.ShipLocationID;
		}

		protected virtual List SelectLotSerialStatus(string origLotSerialNbr, SOShipLine newline, INLotSerClass lotserclass)
		{
			PXSelectBase cmd;
			if (!string.IsNullOrEmpty(origLotSerialNbr))
			{
				cmd =
					new PXSelectReadonly2,
					LeftJoin,
						And,
						And,
						And>>>>>>,
					Where>,
						And>,
						And>,
						And>,
						And,
						And>>>>>>>(Base);
			}
			else
			{
				cmd =
					new PXSelectReadonly2,
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
						And>>>>>>>>>(Base);
			}

			var pars = new List(capacity: 8) { newline.InventoryID, newline.SubItemID, newline.SiteID, newline.CostCenterID };

			if (!string.IsNullOrEmpty(origLotSerialNbr))
			{
				cmd.WhereAnd>>>();
				pars.Add(origLotSerialNbr);
			}

			AppendFiltersForStatusSelect(newline, cmd, pars);

			Base.LineSplittingExt.AppendSerialStatusCmdOrderBy(cmd, newline, lotserclass);

			return cmd.Select(pars.ToArray()).AsEnumerable().Cast().ToList();
		}

		public virtual void AppendFiltersForStatusSelect(SOShipLine line, PXSelectBase select, List parameters)
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

		protected virtual void ResortStockForShipmentByDefaultItemLocation(SOShipLine newline, List resultset)
		{
			if (INSite.PK.Find(Base, newline.SiteID)?.UseItemDefaultLocationForPicking != true)
			{
				return;
			}

			var dfltShipLocationID = INItemSite.PK.Find(Base, newline.InventoryID, newline.SiteID)?.DfltShipLocationID;
			if (dfltShipLocationID == null)
			{
				return;
			}

			var listOrderedByDfltShipLocationID = resultset.OrderByDescending(
				r => PXResult.Unwrap(r).LocationID == dfltShipLocationID).ToList();
			resultset.Clear();
			resultset.AddRange(listOrderedByDfltShipLocationID);
		}

		protected virtual void ResortStockForShipmentByProjectAndTask(SOShipLine newline, List resultset)
		{
			if (newline.ProjectID == null || newline.TaskID == null)
			{
				return;
			}

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

		protected virtual void AfterSaveCreateShipment(CreateShipmentArgs args)
		{
			if (args.ShipmentList.Find(Base.Document.Current) is null)
			{
				args.ShipmentList.Add(Base.Document.Current);
			}
		}

		protected virtual void SetShipAddressAndContactFromArgs(SOShipment shipment, CreateShipmentArgs args)
		{
		}

		protected virtual void CreateShipmentDetails(CreateShipmentArgs args)
		{
		}

		protected virtual bool ShouldSaveAfterCreateShipment(CreateShipmentArgs args)
		{
			return false;
		}

		protected bool IsSyncUnassignedScope;
		protected int? UnassignedSplitsLocationID
		{
			get; private set;
		}
		public decimal? QuantityToCreate
		{
			get; private set;
		}

		public class SyncUnassignedScope : IDisposable
		{
			private readonly CreateShipmentExtension parent;

			public SyncUnassignedScope(CreateShipmentExtension extension, int? locationID, decimal? quantity = null)
			{
				parent = extension;
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
	}
}
