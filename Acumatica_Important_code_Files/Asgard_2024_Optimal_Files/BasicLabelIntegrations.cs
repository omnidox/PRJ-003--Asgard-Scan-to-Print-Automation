using System;
using PX.Data;
using PX.Objects.AM;
using PX.Objects.AP;
using PX.Objects.AR;
using PX.Objects.EP;
using PX.Objects.IN;
using PX.Objects.PO;
using PX.Objects.SO;

namespace AA.Objects.AL.Integration
{
	// Token: 0x0200029D RID: 669
	public static class BasicLabelIntegrations
	{
		// Token: 0x020009E5 RID: 2533
		public class ALCustomerMaintExt : ALBasicLabelHandlerExt<CustomerMaint, Customer>
		{
			// Token: 0x06002A6E RID: 10862 RVA: 0x000189E0 File Offset: 0x00016BE0
			public static bool IsActive()
			{
				return true;
			}
		}

		// Token: 0x020009E6 RID: 2534
		public class ALVendorMaintExt : ALBasicLabelHandlerExt<VendorMaint, Vendor>
		{
			// Token: 0x06002A70 RID: 10864 RVA: 0x000189E0 File Offset: 0x00016BE0
			public static bool IsActive()
			{
				return true;
			}
		}

		// Token: 0x020009E7 RID: 2535
		public class ALEmployeeMaintExt : ALBasicLabelHandlerExt<EmployeeMaint, EPEmployee>
		{
			// Token: 0x06002A72 RID: 10866 RVA: 0x000189E0 File Offset: 0x00016BE0
			public static bool IsActive()
			{
				return true;
			}
		}

		// Token: 0x020009E8 RID: 2536
		public class ALPOOrderEntryExt : ALBasicLabelHandlerExt<POOrderEntry, POOrder>
		{
			// Token: 0x06002A74 RID: 10868 RVA: 0x000189E0 File Offset: 0x00016BE0
			public static bool IsActive()
			{
				return true;
			}
		}

		// Token: 0x020009E9 RID: 2537
		public class ALPOReceiptEntryExt : ALBasicLabelHandlerExt<POReceiptEntry, POReceipt>
		{
			// Token: 0x06002A76 RID: 10870 RVA: 0x000189E0 File Offset: 0x00016BE0
			public static bool IsActive()
			{
				return true;
			}
		}

		// Token: 0x020009EA RID: 2538
		public class ALSOOrderEntryExt : ALBasicLabelHandlerExt<SOOrderEntry, SOOrder>
		{
			// Token: 0x06002A78 RID: 10872 RVA: 0x000189E0 File Offset: 0x00016BE0
			public static bool IsActive()
			{
				return true;
			}
		}

		// Token: 0x020009EB RID: 2539
		public class ALSOShipmentEntryExt : ALBasicLabelHandlerExt<SOShipmentEntry, SOShipment>
		{
			// Token: 0x06002A7A RID: 10874 RVA: 0x000189E0 File Offset: 0x00016BE0
			public static bool IsActive()
			{
				return true;
			}
		}

		// Token: 0x020009EC RID: 2540
		public class ALINTransferEntryExt : ALBasicLabelHandlerExt<INTransferEntry, INRegister>
		{
			// Token: 0x06002A7C RID: 10876 RVA: 0x000189E0 File Offset: 0x00016BE0
			public static bool IsActive()
			{
				return true;
			}
		}

		// Token: 0x020009ED RID: 2541
		public class ALINAdjustmentEntryExt : ALBasicLabelHandlerExt<INAdjustmentEntry, INRegister>
		{
			// Token: 0x06002A7E RID: 10878 RVA: 0x000189E0 File Offset: 0x00016BE0
			public static bool IsActive()
			{
				return true;
			}
		}

		// Token: 0x020009EE RID: 2542
		public class ALINReceiptEntryExt : ALBasicLabelHandlerExt<INReceiptEntry, INRegister>
		{
			// Token: 0x06002A80 RID: 10880 RVA: 0x000189E0 File Offset: 0x00016BE0
			public static bool IsActive()
			{
				return true;
			}
		}

		// Token: 0x020009EF RID: 2543
		public class ALINIssueEntryExt : ALBasicLabelHandlerExt<INIssueEntry, INRegister>
		{
			// Token: 0x06002A82 RID: 10882 RVA: 0x000189E0 File Offset: 0x00016BE0
			public static bool IsActive()
			{
				return true;
			}
		}

		// Token: 0x020009F0 RID: 2544
		public class ALKitAssemblyEntryExt : ALBasicLabelHandlerExt<KitAssemblyEntry, INKitRegister>
		{
			// Token: 0x06002A84 RID: 10884 RVA: 0x000189E0 File Offset: 0x00016BE0
			public static bool IsActive()
			{
				return true;
			}
		}

		// Token: 0x020009F1 RID: 2545
		public class ALInventoryItemMaintExt : ALBasicLabelHandlerExt<InventoryItemMaint, InventoryItem>
		{
			// Token: 0x06002A86 RID: 10886 RVA: 0x000189E0 File Offset: 0x00016BE0
			public static bool IsActive()
			{
				return true;
			}
		}

		// Token: 0x020009F2 RID: 2546
		public class ALNonStockItemMaintExt : ALBasicLabelHandlerExt<NonStockItemMaint, InventoryItem>
		{
			// Token: 0x06002A88 RID: 10888 RVA: 0x000189E0 File Offset: 0x00016BE0
			public static bool IsActive()
			{
				return true;
			}
		}

		// Token: 0x020009F3 RID: 2547
		public class ALINSiteMaintExt : ALBasicLabelHandlerExt<INSiteMaint, INSite>
		{
			// Token: 0x06002A8A RID: 10890 RVA: 0x000189E0 File Offset: 0x00016BE0
			public static bool IsActive()
			{
				return true;
			}
		}

		// Token: 0x020009F4 RID: 2548
		public class ALINItemSiteMaintExt : ALBasicLabelHandlerExt<INItemSiteMaint, INItemSite>
		{
			// Token: 0x06002A8C RID: 10892 RVA: 0x000189E0 File Offset: 0x00016BE0
			public static bool IsActive()
			{
				return true;
			}
		}

		// Token: 0x020009F5 RID: 2549
		public class ALINSiteBuildingMaintExt : ALBasicLabelHandlerExt<INSiteBuildingMaint, INSiteBuilding>
		{
			// Token: 0x06002A8E RID: 10894 RVA: 0x000189E0 File Offset: 0x00016BE0
			public static bool IsActive()
			{
				return true;
			}
		}

		// Token: 0x020009F6 RID: 2550
		public class ALINPIReviewExt : ALBasicLabelHandlerExt<INPIReview, INPIHeader>
		{
			// Token: 0x06002A90 RID: 10896 RVA: 0x000189E0 File Offset: 0x00016BE0
			public static bool IsActive()
			{
				return true;
			}
		}

		// Token: 0x020009F7 RID: 2551
		public class ALINPICountEntryExt : ALBasicLabelHandlerExt<INPICountEntry, INPIHeader>
		{
			// Token: 0x06002A92 RID: 10898 RVA: 0x000189E0 File Offset: 0x00016BE0
			public static bool IsActive()
			{
				return true;
			}
		}

		// Token: 0x020009F8 RID: 2552
		public class ALPXGenericInqGrphExt : ALBasicLabelHandlerExt<PXGenericInqGrph, GenericFilter>
		{
			// Token: 0x06002A94 RID: 10900 RVA: 0x000189E0 File Offset: 0x00016BE0
			public static bool IsActive()
			{
				return true;
			}
		}

		// Token: 0x020009F9 RID: 2553
		public class ALMaterialEntryExt : ALBasicLabelHandlerExt<MaterialEntry, AMBatch>
		{
			// Token: 0x06002A96 RID: 10902 RVA: 0x000189E0 File Offset: 0x00016BE0
			public static bool IsActive()
			{
				return true;
			}
		}

		// Token: 0x020009FA RID: 2554
		public class ALMoveEntryExt : ALBasicLabelHandlerExt<MoveEntry, AMBatch>
		{
			// Token: 0x06002A98 RID: 10904 RVA: 0x000189E0 File Offset: 0x00016BE0
			public static bool IsActive()
			{
				return true;
			}
		}

		// Token: 0x020009FB RID: 2555
		public class ALLaborEntryExt : ALBasicLabelHandlerExt<LaborEntry, AMBatch>
		{
			// Token: 0x06002A9A RID: 10906 RVA: 0x000189E0 File Offset: 0x00016BE0
			public static bool IsActive()
			{
				return true;
			}
		}

		// Token: 0x020009FC RID: 2556
		public class ALDisassemblyEntryExt : ALBasicLabelHandlerExt<DisassemblyEntry, AMDisassembleBatch>
		{
			// Token: 0x06002A9C RID: 10908 RVA: 0x000189E0 File Offset: 0x00016BE0
			public static bool IsActive()
			{
				return true;
			}
		}

		// Token: 0x020009FD RID: 2557
		public class ALUnreleasedMaterialAllocationsExt : ALBasicLabelHandlerExt<UnreleasedMaterialAllocations, AMUnrelMaterialAllocationsFilter>
		{
			// Token: 0x06002A9E RID: 10910 RVA: 0x000189E0 File Offset: 0x00016BE0
			public static bool IsActive()
			{
				return true;
			}
		}

		// Token: 0x020009FE RID: 2558
		public class ALProdMaintExt : ALBasicLabelHandlerExt<ProdMaint, AMProdItem>
		{
			// Token: 0x06002AA0 RID: 10912 RVA: 0x000189E0 File Offset: 0x00016BE0
			public static bool IsActive()
			{
				return true;
			}
		}

		// Token: 0x020009FF RID: 2559
		public class ALProdDetailExt : ALBasicLabelHandlerExt<ProdDetail, AMProdItem>
		{
			// Token: 0x06002AA2 RID: 10914 RVA: 0x000189E0 File Offset: 0x00016BE0
			public static bool IsActive()
			{
				return true;
			}
		}
	}
}
