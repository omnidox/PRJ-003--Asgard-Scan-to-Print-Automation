using System;
using PX.Data;
using PX.Objects.CR;
using PX.Objects.CS;
using PX.Objects.GL;
using PX.Objects.IN;
using PX.Objects.SO;

namespace AA.Objects.Labels.Integration
{
	// Token: 0x020001C7 RID: 455
	public class ALSOShipmentEntryExt : PXGraphExtension<SOShipmentEntry>
	{
		// Token: 0x0600133D RID: 4925 RVA: 0x00042632 File Offset: 0x00040832
		public static bool IsActive()
		{
			return ALSetupSlot.IsActive(typeof(SOShipmentEntry));
		}

		// Token: 0x040007CD RID: 1997
		[PXViewName("AL Details")]
		public PXSelectJoin<SOShipLine, LeftJoin<SOLine, On<SOShipLine.FK.OrderLine>, LeftJoin<InventoryItem, On<SOShipLine.FK.InventoryItem>, LeftJoin<INSite, On<SOShipLine.FK.Site>, LeftJoin<INLocation, On<SOShipLine.FK.Location>, LeftJoin<INItemLotSerial, On<INItemLotSerial.inventoryID, Equal<SOShipLine.inventoryID>, And<INItemLotSerial.lotSerialNbr, Equal<SOShipLine.lotSerialNbr>>>, LeftJoin<INSiteLotSerial, On<INSiteLotSerial.inventoryID, Equal<SOShipLine.inventoryID>, And<INSiteLotSerial.siteID, Equal<SOShipLine.siteID>, And<INSiteLotSerial.lotSerialNbr, Equal<SOShipLine.lotSerialNbr>>>>>>>>>>, Where<SOShipLine.shipmentNbr, Equal<Current<SOShipment.shipmentNbr>>>, OrderBy<Asc<SOShipLine.sortOrder, Asc<SOShipLine.lineNbr>>>> ALDetails;

		// Token: 0x040007CE RID: 1998
		[PXViewName("AL Allocations")]
		public PXSelectJoin<SOShipLineSplit, LeftJoin<SOShipLine, On<SOShipLineSplit.FK.ShipmentLine>, LeftJoin<SOLine, On<SOShipLine.FK.OrderLine>, LeftJoin<InventoryItem, On<SOShipLineSplit.FK.InventoryItem>, LeftJoin<INSite, On<SOShipLine.FK.Site>, LeftJoin<INLocation, On<SOShipLine.FK.Location>, LeftJoin<INItemLotSerial, On<INItemLotSerial.inventoryID, Equal<SOShipLineSplit.inventoryID>, And<INItemLotSerial.lotSerialNbr, Equal<SOShipLineSplit.lotSerialNbr>>>, LeftJoin<INSiteLotSerial, On<INSiteLotSerial.inventoryID, Equal<SOShipLineSplit.inventoryID>, And<INSiteLotSerial.siteID, Equal<SOShipLineSplit.siteID>, And<INSiteLotSerial.lotSerialNbr, Equal<SOShipLineSplit.lotSerialNbr>>>>>>>>>>>, Where<SOShipLineSplit.shipmentNbr, Equal<Current<SOShipment.shipmentNbr>>>, OrderBy<Asc<SOShipLineSplit.lineNbr, Asc<SOShipLineSplit.splitLineNbr>>>> ALAllocations;

		// Token: 0x040007CF RID: 1999
		[PXViewName("AL Package Content")]
		public PXSelectJoin<SOShipLineSplitPackage, LeftJoin<SOShipLineSplit, On<SOShipLineSplitPackage.FK.ShipmentLineSplit>, LeftJoin<SOPackageDetail, On<SOShipLineSplitPackage.FK.PackageDetail>, LeftJoin<SOShipment, On<SOShipLineSplit.FK.Shipment>, LeftJoin<SOShipLine, On<SOShipLineSplit.FK.ShipmentLine>, LeftJoin<SOOrder, On<SOShipLineSplit.FK.OriginalOrder>, LeftJoin<SOLine, On<SOShipLineSplit.FK.OriginalOrderLine>, LeftJoin<SOLineSplit, On<SOShipLineSplit.FK.OriginalOrderLineSplit>, LeftJoin<InventoryItem, On<SOShipLineSplit.FK.InventoryItem>>>>>>>>>, Where<SOShipLineSplitPackage.shipmentNbr, Equal<Current<SOShipment.shipmentNbr>>>, OrderBy<Asc<SOShipLineSplitPackage.packageLineNbr>>> ALPackageContents;

		// Token: 0x040007D0 RID: 2000
		[PXViewName("AL Packages")]
		public PXSelectJoin<SOPackageDetail, LeftJoin<SOShipment, On<SOPackageDetail.FK.Shipment>, LeftJoin<CSBox, On<SOPackageDetail.FK.Box>, LeftJoin<InventoryItem, On<SOPackageDetail.FK.InventoryItem>>>>, Where<SOPackageDetail.shipmentNbr, Equal<Current<SOShipment.shipmentNbr>>>, OrderBy<Asc<SOPackageDetail.lineNbr>>> ALPackages;

		// Token: 0x040007D1 RID: 2001
		[PXViewName("AL Package Details")]
		public PXSelectJoin<SOShipLineSplitPackage, LeftJoin<SOShipLineSplit, On<SOShipLineSplitPackage.FK.ShipmentLineSplit>, LeftJoin<SOShipment, On<SOShipLineSplit.FK.Shipment>, LeftJoin<SOShipLine, On<SOShipLineSplit.FK.ShipmentLine>, LeftJoin<SOOrder, On<SOShipLineSplit.FK.OriginalOrder>, LeftJoin<SOLine, On<SOShipLineSplit.FK.OriginalOrderLine>, LeftJoin<SOLineSplit, On<SOShipLineSplit.FK.OriginalOrderLineSplit>, LeftJoin<InventoryItem, On<SOShipLineSplit.FK.InventoryItem>>>>>>>>, Where<SOShipLineSplitPackage.shipmentNbr, Equal<Current<SOShipment.shipmentNbr>>, And<SOShipLineSplitPackage.packageLineNbr, Equal<Current<SOPackageDetail.lineNbr>>>>, OrderBy<Asc<SOShipLineSplit.origOrderType, Asc<SOShipLineSplit.origOrderNbr, Asc<SOShipLineSplit.origLineNbr>>>>> ALPackageDetails;

		// Token: 0x040007D2 RID: 2002
		[PXViewName("AL Ship From Branch")]
		public PXSelectJoin<Branch, LeftJoin<INSite, On<INSite.branchID, Equal<Branch.branchID>>>, Where<INSite.siteID, Equal<Current<SOShipment.siteID>>>> ALWarehouse_Branch;

		// Token: 0x040007D3 RID: 2003
		[PXViewName("AL Ship From Address")]
		public PXSelectJoin<Address, LeftJoin<INSite, On<INSite.addressID, Equal<Address.addressID>>>, Where<INSite.siteID, Equal<Current<SOShipment.siteID>>>> ALWarehouse_Address;

		// Token: 0x040007D4 RID: 2004
		[PXViewName("AL Ship From Contact")]
		public PXSelectJoin<Contact, LeftJoin<INSite, On<INSite.contactID, Equal<Contact.contactID>>>, Where<INSite.siteID, Equal<Current<SOShipment.siteID>>>> ALWarehouse_Contact;
	}
}
