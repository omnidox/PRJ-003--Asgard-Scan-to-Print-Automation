using System;
using PX.Data;
using PX.Objects.CS;
using PX.Objects.IN;
using PX.Objects.SO;
using WMS;

namespace AA.Objects.AL.Integration.RomanSunStone
{
	// Token: 0x02000004 RID: 4
	public class ALRSSSOShipmentEntryExt : PXGraphExtension<SOShipmentEntry>
	{
		// Token: 0x06000003 RID: 3 RVA: 0x00002061 File Offset: 0x00000261
		public static bool IsActive()
		{
			return true;
		}

		// Token: 0x04000002 RID: 2
		[PXViewName("AL RSS Package Details")]
		public PXSelectJoin<SOPackageDetail, LeftJoin<SOShipment, On<SOPackageDetail.FK.Shipment>, LeftJoin<SOOrder, On<SOOrder.orderNbr, Equal<SOPackageDetailExt.usrOrderNbr>>, LeftJoin<CSBox, On<SOPackageDetail.FK.Box>, LeftJoin<InventoryItem, On<SOPackageDetail.FK.InventoryItem>>>>>, Where<SOPackageDetail.shipmentNbr, Equal<Current<SOShipment.shipmentNbr>>>, OrderBy<Asc<SOPackageDetail.lineNbr>>> ALiStarPackages;
	}
}
