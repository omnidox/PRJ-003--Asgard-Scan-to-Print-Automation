using System;
using PX.Data;
using PX.Objects.CR;
using PX.Objects.CS;
using PX.Objects.GL;
using PX.Objects.IN;
using PX.Objects.SO;

namespace AA.Objects.AL.Integration
{
	// Token: 0x0200029C RID: 668
	public class ALSOShipmentEntryExt : PXGraphExtension<SOShipmentEntry>
	{
		// Token: 0x06001999 RID: 6553 RVA: 0x000189E0 File Offset: 0x00016BE0
		public static bool IsActive()
		{
			return true;
		}

		// Token: 0x04000B3A RID: 2874
		[PXViewName("AL Details")]
		public PXSelectJoin<SOShipLine, LeftJoin<SOLine, On<SOShipLine.FK.OrderLine>, LeftJoin<InventoryItem, On<SOShipLine.FK.InventoryItem>, LeftJoin<ALTemplateItem, On<ALTemplateItem.inventoryID, Equal<InventoryItem.templateItemID>>, LeftJoin<INSite, On<SOShipLine.FK.Site>, LeftJoin<INLocation, On<SOShipLine.FK.Location>, LeftJoin<INItemLotSerial, On<INItemLotSerial.inventoryID, Equal<SOShipLine.inventoryID>, And<INItemLotSerial.lotSerialNbr, Equal<SOShipLine.lotSerialNbr>>>, LeftJoin<INSiteLotSerial, On<INSiteLotSerial.inventoryID, Equal<SOShipLine.inventoryID>, And<INSiteLotSerial.siteID, Equal<SOShipLine.siteID>, And<INSiteLotSerial.lotSerialNbr, Equal<SOShipLine.lotSerialNbr>>>>>>>>>>>, Where<SOShipLine.shipmentNbr, Equal<Current<SOShipment.shipmentNbr>>>, OrderBy<Asc<SOShipLine.sortOrder, Asc<SOShipLine.lineNbr>>>> ALDetails;

		// Token: 0x04000B3B RID: 2875
		[PXViewName("AL Allocations")]
		public PXSelectJoin<SOShipLineSplit, LeftJoin<SOShipLine, On<SOShipLineSplit.FK.ShipmentLine>, LeftJoin<SOLine, On<SOShipLine.FK.OrderLine>, LeftJoin<InventoryItem, On<SOShipLineSplit.FK.InventoryItem>, LeftJoin<ALTemplateItem, On<ALTemplateItem.inventoryID, Equal<InventoryItem.templateItemID>>, LeftJoin<INSite, On<SOShipLine.FK.Site>, LeftJoin<INLocation, On<SOShipLine.FK.Location>, LeftJoin<INItemLotSerial, On<INItemLotSerial.inventoryID, Equal<SOShipLineSplit.inventoryID>, And<INItemLotSerial.lotSerialNbr, Equal<SOShipLineSplit.lotSerialNbr>>>, LeftJoin<INSiteLotSerial, On<INSiteLotSerial.inventoryID, Equal<SOShipLineSplit.inventoryID>, And<INSiteLotSerial.siteID, Equal<SOShipLineSplit.siteID>, And<INSiteLotSerial.lotSerialNbr, Equal<SOShipLineSplit.lotSerialNbr>>>>>>>>>>>>, Where<SOShipLineSplit.shipmentNbr, Equal<Current<SOShipment.shipmentNbr>>>, OrderBy<Asc<SOShipLineSplit.lineNbr, Asc<SOShipLineSplit.splitLineNbr>>>> ALAllocations;

		// Token: 0x04000B3C RID: 2876
		[PXViewName("AL Package Content")]
		public PXSelectJoin<SOShipLineSplitPackage, LeftJoin<SOShipLineSplit, On<SOShipLineSplitPackage.FK.ShipmentLineSplit>, LeftJoin<SOPackageDetail, On<SOShipLineSplitPackage.FK.PackageDetail>, LeftJoin<SOShipment, On<SOShipLineSplit.FK.Shipment>, LeftJoin<SOShipLine, On<SOShipLineSplit.FK.ShipmentLine>, LeftJoin<SOOrder, On<SOShipLineSplit.FK.OriginalOrder>, LeftJoin<SOLine, On<SOShipLineSplit.FK.OriginalOrderLine>, LeftJoin<SOLineSplit, On<SOShipLineSplit.FK.OriginalOrderLineSplit>, LeftJoin<InventoryItem, On<SOShipLineSplit.FK.InventoryItem>, LeftJoin<ALTemplateItem, On<ALTemplateItem.inventoryID, Equal<InventoryItem.templateItemID>>>>>>>>>>>, Where<SOShipLineSplitPackage.shipmentNbr, Equal<Current<SOShipment.shipmentNbr>>>, OrderBy<Asc<SOShipLineSplitPackage.packageLineNbr>>> ALPackageContents;

		// Token: 0x04000B3D RID: 2877
		[PXViewName("AL Packages")]
		public PXSelectJoin<SOPackageDetail, LeftJoin<SOShipment, On<SOPackageDetail.FK.Shipment>, LeftJoin<CSBox, On<SOPackageDetail.FK.Box>, LeftJoin<InventoryItem, On<SOPackageDetail.FK.InventoryItem>, LeftJoin<ALTemplateItem, On<ALTemplateItem.inventoryID, Equal<InventoryItem.templateItemID>>>>>>, Where<SOPackageDetail.shipmentNbr, Equal<Current<SOShipment.shipmentNbr>>>, OrderBy<Asc<SOPackageDetail.lineNbr>>> ALPackages;

		// Token: 0x04000B3E RID: 2878
		[PXViewName("AL Package Details")]
		public PXSelectJoin<SOShipLineSplitPackage, LeftJoin<SOShipLineSplit, On<SOShipLineSplitPackage.FK.ShipmentLineSplit>, LeftJoin<SOShipment, On<SOShipLineSplit.FK.Shipment>, LeftJoin<SOShipLine, On<SOShipLineSplit.FK.ShipmentLine>, LeftJoin<SOOrder, On<SOShipLineSplit.FK.OriginalOrder>, LeftJoin<SOLine, On<SOShipLineSplit.FK.OriginalOrderLine>, LeftJoin<SOLineSplit, On<SOShipLineSplit.FK.OriginalOrderLineSplit>, LeftJoin<InventoryItem, On<SOShipLineSplit.FK.InventoryItem>, LeftJoin<ALTemplateItem, On<ALTemplateItem.inventoryID, Equal<InventoryItem.templateItemID>>>>>>>>>>, Where<SOShipLineSplitPackage.shipmentNbr, Equal<Current<SOShipment.shipmentNbr>>, And<SOShipLineSplitPackage.packageLineNbr, Equal<Current<SOPackageDetail.lineNbr>>>>, OrderBy<Asc<SOShipLineSplit.origOrderType, Asc<SOShipLineSplit.origOrderNbr, Asc<SOShipLineSplit.origLineNbr>>>>> ALPackageDetails;

		// Token: 0x04000B3F RID: 2879
		[PXViewName("AL Ship From Branch")]
		public PXSelectJoin<Branch, LeftJoin<INSite, On<INSite.branchID, Equal<Branch.branchID>>>, Where<INSite.siteID, Equal<Current<SOShipment.siteID>>>> ALWarehouse_Branch;

		// Token: 0x04000B40 RID: 2880
		[PXViewName("AL Ship From Address")]
		public PXSelectJoin<Address, LeftJoin<INSite, On<INSite.addressID, Equal<Address.addressID>>>, Where<INSite.siteID, Equal<Current<SOShipment.siteID>>>> ALWarehouse_Address;

		// Token: 0x04000B41 RID: 2881
		[PXViewName("AL Ship From Contact")]
		public PXSelectJoin<Contact, LeftJoin<INSite, On<INSite.contactID, Equal<Contact.contactID>>>, Where<INSite.siteID, Equal<Current<SOShipment.siteID>>>> ALWarehouse_Contact;
	}
}
