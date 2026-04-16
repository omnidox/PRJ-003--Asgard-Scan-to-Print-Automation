using System;
using System.Collections;
using AA.Objects.Labels.Mobile;
using PX.BarcodeProcessing;
using PX.Data;
using PX.Objects.AM;
using PX.Objects.AM.Attributes;
using PX.Objects.AP;
using PX.Objects.AR;
using PX.Objects.EP;
using PX.Objects.IN;
using PX.Objects.IN.WMS;
using PX.Objects.PO;
using PX.Objects.SO;
using PX.Objects.SO.WMS;

namespace AA.Objects.Labels.Integration
{
	// Token: 0x020001B8 RID: 440
	public static class LabelIntegrations
	{
		// Token: 0x02000875 RID: 2165
		public class ALCustomerMaintExt : ALHandlerExt<CustomerMaint, Customer>
		{
			// Token: 0x060021E9 RID: 8681 RVA: 0x0005F28C File Offset: 0x0005D48C
			public static bool IsActive()
			{
				return ALSetupSlot.IsActive(typeof(CustomerMaint));
			}
		}

		// Token: 0x02000876 RID: 2166
		public class ALVendorMaintExt : ALHandlerExt<VendorMaint, Vendor>
		{
			// Token: 0x060021EB RID: 8683 RVA: 0x0005F2A6 File Offset: 0x0005D4A6
			public static bool IsActive()
			{
				return ALSetupSlot.IsActive(typeof(VendorMaint));
			}
		}

		// Token: 0x02000877 RID: 2167
		public class ALEmployeeMaintExt : ALHandlerExt<EmployeeMaint, EPEmployee>
		{
			// Token: 0x060021ED RID: 8685 RVA: 0x0005F2C0 File Offset: 0x0005D4C0
			public static bool IsActive()
			{
				return ALSetupSlot.IsActive(typeof(EmployeeMaint));
			}
		}

		// Token: 0x02000878 RID: 2168
		public class ALPOOrderEntryExt : ALHandlerExt<POOrderEntry, POOrder>
		{
			// Token: 0x060021EF RID: 8687 RVA: 0x000425A7 File Offset: 0x000407A7
			public static bool IsActive()
			{
				return ALSetupSlot.IsActive(typeof(POOrderEntry));
			}
		}

		// Token: 0x02000879 RID: 2169
		public class ALPOReceiptEntryExt : ALHandlerExt<POReceiptEntry, POReceipt>
		{
			// Token: 0x060021F1 RID: 8689 RVA: 0x000425C1 File Offset: 0x000407C1
			public static bool IsActive()
			{
				return ALSetupSlot.IsActive(typeof(POReceiptEntry));
			}
		}

		// Token: 0x0200087A RID: 2170
		public class ALSOOrderEntryExt : ALHandlerExt<SOOrderEntry, SOOrder>
		{
			// Token: 0x060021F3 RID: 8691 RVA: 0x00042618 File Offset: 0x00040818
			public static bool IsActive()
			{
				return ALSetupSlot.IsActive(typeof(SOOrderEntry));
			}
		}

		// Token: 0x0200087B RID: 2171
		public class ALSOShipmentEntryExt : ALHandlerExt<SOShipmentEntry, SOShipment>
		{
			// Token: 0x060021F5 RID: 8693 RVA: 0x00042632 File Offset: 0x00040832
			public static bool IsActive()
			{
				return ALSetupSlot.IsActive(typeof(SOShipmentEntry));
			}
		}

		// Token: 0x0200087C RID: 2172
		public class ALINTransferEntryExt : ALHandlerExt<INTransferEntry, INRegister>
		{
			// Token: 0x060021F7 RID: 8695 RVA: 0x0005F2FE File Offset: 0x0005D4FE
			public static bool IsActive()
			{
				return ALSetupSlot.IsActive(typeof(INTransferEntry));
			}
		}

		// Token: 0x0200087D RID: 2173
		public class ALINAdjustmentEntryExt : ALHandlerExt<INAdjustmentEntry, INRegister>
		{
			// Token: 0x060021F9 RID: 8697 RVA: 0x0005F318 File Offset: 0x0005D518
			public static bool IsActive()
			{
				return ALSetupSlot.IsActive(typeof(INAdjustmentEntry));
			}
		}

		// Token: 0x0200087E RID: 2174
		public class ALINReceiptEntryExt : ALHandlerExt<INReceiptEntry, INRegister>
		{
			// Token: 0x060021FB RID: 8699 RVA: 0x0005F332 File Offset: 0x0005D532
			public static bool IsActive()
			{
				return ALSetupSlot.IsActive(typeof(INReceiptEntry));
			}
		}

		// Token: 0x0200087F RID: 2175
		public class ALINIssueEntryExt : ALHandlerExt<INIssueEntry, INRegister>
		{
			// Token: 0x060021FD RID: 8701 RVA: 0x0005F34C File Offset: 0x0005D54C
			public static bool IsActive()
			{
				return ALSetupSlot.IsActive(typeof(INIssueEntry));
			}
		}

		// Token: 0x02000880 RID: 2176
		public class ALKitAssemblyEntryExt : ALHandlerExt<KitAssemblyEntry, INKitRegister>
		{
			// Token: 0x060021FF RID: 8703 RVA: 0x0005F366 File Offset: 0x0005D566
			public static bool IsActive()
			{
				return ALSetupSlot.IsActive(typeof(KitAssemblyEntry));
			}
		}

		// Token: 0x02000881 RID: 2177
		public class ALInventoryItemMaintExt : ALHandlerExt<InventoryItemMaint, InventoryItem>
		{
			// Token: 0x06002201 RID: 8705 RVA: 0x0004258D File Offset: 0x0004078D
			public static bool IsActive()
			{
				return ALSetupSlot.IsActive(typeof(InventoryItemMaint));
			}
		}

		// Token: 0x02000882 RID: 2178
		public class ALNonStockItemMaintExt : ALHandlerExt<NonStockItemMaint, InventoryItem>
		{
			// Token: 0x06002203 RID: 8707 RVA: 0x0005F389 File Offset: 0x0005D589
			public static bool IsActive()
			{
				return ALSetupSlot.IsActive(typeof(NonStockItemMaint));
			}
		}

		// Token: 0x02000883 RID: 2179
		public class ALINSiteMaintExt : ALHandlerExt<INSiteMaint, INSite>
		{
			// Token: 0x06002205 RID: 8709 RVA: 0x0005F3A3 File Offset: 0x0005D5A3
			public static bool IsActive()
			{
				return ALSetupSlot.IsActive(typeof(INSiteMaint));
			}
		}

		// Token: 0x02000884 RID: 2180
		public class ALINItemSiteMaintExt : ALHandlerExt<INItemSiteMaint, INItemSite>
		{
			// Token: 0x06002207 RID: 8711 RVA: 0x00042573 File Offset: 0x00040773
			public static bool IsActive()
			{
				return ALSetupSlot.IsActive(typeof(INItemSiteMaint));
			}
		}

		// Token: 0x02000885 RID: 2181
		public class ALINSiteBuildingMaintExt : ALHandlerExt<INSiteBuildingMaint, INSiteBuilding>
		{
			// Token: 0x06002209 RID: 8713 RVA: 0x0005F3C6 File Offset: 0x0005D5C6
			public static bool IsActive()
			{
				return ALSetupSlot.IsActive(typeof(INSiteBuildingMaint));
			}
		}

		// Token: 0x02000886 RID: 2182
		public class ALINPIReviewExt : ALHandlerExt<INPIReview, INPIHeader>
		{
			// Token: 0x0600220B RID: 8715 RVA: 0x0005F3E0 File Offset: 0x0005D5E0
			public static bool IsActive()
			{
				return ALSetupSlot.IsActive(typeof(INPIReview));
			}
		}

		// Token: 0x02000887 RID: 2183
		public class ALINPICountEntryExt : ALHandlerExt<INPICountEntry, INPIHeader>
		{
			// Token: 0x0600220D RID: 8717 RVA: 0x0005F3FA File Offset: 0x0005D5FA
			public static bool IsActive()
			{
				return ALSetupSlot.IsActive(typeof(INPICountEntry));
			}
		}

		// Token: 0x02000888 RID: 2184
		public class ALPXGenericInqGrphExt : ALHandlerExt<PXGenericInqGrph, GenericFilter>
		{
			// Token: 0x0600220F RID: 8719 RVA: 0x0005F414 File Offset: 0x0005D614
			public static bool IsActive()
			{
				return ALSetupSlot.IsActive(typeof(PXGenericInqGrph));
			}
		}

		// Token: 0x02000889 RID: 2185
		public class ALMaterialEntryExt : ALHandlerExt<MaterialEntry, AMBatch>
		{
			// Token: 0x06002211 RID: 8721 RVA: 0x000424AF File Offset: 0x000406AF
			public static bool IsActive()
			{
				return ALSetupSlot.IsActive(typeof(MaterialEntry));
			}
		}

		// Token: 0x0200088A RID: 2186
		public class ALMoveEntryExt : ALHandlerExt<MoveEntry, AMBatch>
		{
			// Token: 0x06002213 RID: 8723 RVA: 0x000424C9 File Offset: 0x000406C9
			public static bool IsActive()
			{
				return ALSetupSlot.IsActive(typeof(MoveEntry));
			}
		}

		// Token: 0x0200088B RID: 2187
		public class ALLaborEntryExt : ALHandlerExt<LaborEntry, AMBatch>
		{
			// Token: 0x06002215 RID: 8725 RVA: 0x00042495 File Offset: 0x00040695
			public static bool IsActive()
			{
				return ALSetupSlot.IsActive(typeof(LaborEntry));
			}
		}

		// Token: 0x0200088C RID: 2188
		public class ALDisassemblyEntryExt : ALHandlerExt<DisassemblyEntry, AMDisassembleBatch>
		{
			// Token: 0x06002217 RID: 8727 RVA: 0x0005F449 File Offset: 0x0005D649
			public static bool IsActive()
			{
				return ALSetupSlot.IsActive(typeof(DisassemblyEntry));
			}
		}

		// Token: 0x0200088D RID: 2189
		public class ALUnreleasedMaterialAllocationsExt : ALHandlerExt<UnreleasedMaterialAllocations, AMUnrelMaterialAllocationsFilter>
		{
			// Token: 0x06002219 RID: 8729 RVA: 0x0005F463 File Offset: 0x0005D663
			public static bool IsActive()
			{
				return ALSetupSlot.IsActive(typeof(UnreleasedMaterialAllocations));
			}
		}

		// Token: 0x0200088E RID: 2190
		public class ALProdMaintExt : ALHandlerExt<ProdMaint, AMProdItem>
		{
			// Token: 0x0600221B RID: 8731 RVA: 0x000424FD File Offset: 0x000406FD
			public static bool IsActive()
			{
				return ALSetupSlot.IsActive(typeof(ProdMaint));
			}
		}

		// Token: 0x0200088F RID: 2191
		public class ALProdDetailExt : ALHandlerExt<ProdDetail, AMProdItem>
		{
			// Token: 0x0600221D RID: 8733 RVA: 0x000424E3 File Offset: 0x000406E3
			public static bool IsActive()
			{
				return ALSetupSlot.IsActive(typeof(ProdDetail));
			}
		}

		// Token: 0x02000890 RID: 2192
		public class InventorySummaryEnqExt : ALHandlerExt<InventorySummaryEnq, InventorySummaryEnqFilter>
		{
			// Token: 0x0600221F RID: 8735 RVA: 0x0005F48F File Offset: 0x0005D68F
			public static bool IsActive()
			{
				return ALSetupSlot.IsActive(typeof(InventorySummaryEnq));
			}
		}

		// Token: 0x02000891 RID: 2193
		public class StoragePlaceEnqExt : ALHandlerExt<StoragePlaceEnq, StoragePlaceEnq.StoragePlaceFilter>
		{
			// Token: 0x06002221 RID: 8737 RVA: 0x0005F4A9 File Offset: 0x0005D6A9
			public static bool IsActive()
			{
				return ALSetupSlot.IsActive(typeof(StoragePlaceEnq));
			}
		}

		// Token: 0x02000892 RID: 2194
		public class INScanIssueExt : INScanRegisterBaseExt<INScanIssue, INScanIssue.Host, INDocType.issue>
		{
			// Token: 0x06002223 RID: 8739 RVA: 0x0005F4C3 File Offset: 0x0005D6C3
			public static bool IsActive()
			{
				return ALSetupSlot.IsActive(typeof(INScanIssue));
			}

			// Token: 0x170009B5 RID: 2485
			// (get) Token: 0x06002224 RID: 8740 RVA: 0x0005F4D4 File Offset: 0x0005D6D4
			public override object PrimaryRow
			{
				get
				{
					return base.ScanBasis.Document;
				}
			}

			// Token: 0x170009B6 RID: 2486
			// (get) Token: 0x06002225 RID: 8741 RVA: 0x0005F4E1 File Offset: 0x0005D6E1
			public override IEnumerable Details
			{
				get
				{
					return base.ScanBasis.Details.Cache.Cached;
				}
			}

			// Token: 0x170009B7 RID: 2487
			// (get) Token: 0x06002226 RID: 8742 RVA: 0x0005F4F8 File Offset: 0x0005D6F8
			protected override string[] ViewNames { get; } = new string[]
			{
				"issue",
				"transactions",
				"splits",
				"lsselect"
			};
		}

		// Token: 0x02000893 RID: 2195
		public class INScanReceiveExt : INScanRegisterBaseExt<INScanReceive, INScanReceive.Host, INDocType.receipt>
		{
			// Token: 0x06002228 RID: 8744 RVA: 0x0005F535 File Offset: 0x0005D735
			public static bool IsActive()
			{
				return ALSetupSlot.IsActive(typeof(INScanReceive));
			}

			// Token: 0x170009B8 RID: 2488
			// (get) Token: 0x06002229 RID: 8745 RVA: 0x0005F546 File Offset: 0x0005D746
			public override object PrimaryRow
			{
				get
				{
					return base.ScanBasis.Document;
				}
			}

			// Token: 0x170009B9 RID: 2489
			// (get) Token: 0x0600222A RID: 8746 RVA: 0x0005F553 File Offset: 0x0005D753
			protected override string[] ViewNames { get; } = new string[]
			{
				"receipt",
				"transactions",
				"splits",
				"lsselect"
			};
		}

		// Token: 0x02000894 RID: 2196
		public class INScanTransferExt : INScanRegisterBaseExt<INScanTransfer, INScanTransfer.Host, INDocType.transfer>
		{
			// Token: 0x0600222C RID: 8748 RVA: 0x0005F590 File Offset: 0x0005D790
			public static bool IsActive()
			{
				return ALSetupSlot.IsActive(typeof(INScanTransfer));
			}

			// Token: 0x170009BA RID: 2490
			// (get) Token: 0x0600222D RID: 8749 RVA: 0x0005F5A1 File Offset: 0x0005D7A1
			public override object PrimaryRow
			{
				get
				{
					return base.ScanBasis.Document;
				}
			}

			// Token: 0x170009BB RID: 2491
			// (get) Token: 0x0600222E RID: 8750 RVA: 0x0005F5AE File Offset: 0x0005D7AE
			protected override string[] ViewNames { get; } = new string[]
			{
				"transfer",
				"transactions",
				"splits",
				"lsselect"
			};
		}

		// Token: 0x02000895 RID: 2197
		public class INScanCountExt : ALScanWMSHandleExt<INScanCount, INScanCount.Host>
		{
			// Token: 0x06002230 RID: 8752 RVA: 0x0005F5EB File Offset: 0x0005D7EB
			public static bool IsActive()
			{
				return ALSetupSlot.IsActive(typeof(INScanCount));
			}

			// Token: 0x170009BC RID: 2492
			// (get) Token: 0x06002231 RID: 8753 RVA: 0x0005F5FC File Offset: 0x0005D7FC
			public override object PrimaryRow
			{
				get
				{
					return base.ScanBasis.Document;
				}
			}

			// Token: 0x170009BD RID: 2493
			// (get) Token: 0x06002232 RID: 8754 RVA: 0x0005F609 File Offset: 0x0005D809
			protected override string[] ViewNames { get; } = new string[]
			{
				"PIHeader",
				"PIDetail"
			};
		}

		// Token: 0x02000896 RID: 2198
		public class StoragePlaceLookupExt : ALScanHandleExt<StoragePlaceLookup, StoragePlaceLookup.Host>
		{
			// Token: 0x06002234 RID: 8756 RVA: 0x0005F636 File Offset: 0x0005D836
			public static bool IsActive()
			{
				return ALSetupSlot.IsActive(typeof(StoragePlaceLookup));
			}

			// Token: 0x170009BE RID: 2494
			// (get) Token: 0x06002235 RID: 8757 RVA: 0x0005F647 File Offset: 0x0005D847
			public override object PrimaryRow
			{
				get
				{
					return base.ScanBasis.StorageHeader;
				}
			}

			// Token: 0x170009BF RID: 2495
			// (get) Token: 0x06002236 RID: 8758 RVA: 0x0005F654 File Offset: 0x0005D854
			protected override string[] ViewNames { get; } = new string[]
			{
				"storages"
			};
		}

		// Token: 0x02000897 RID: 2199
		public class InventoryItemLookupExt : ALScanHandleExt<InventoryItemLookup, InventoryItemLookup.Host>
		{
			// Token: 0x06002238 RID: 8760 RVA: 0x0005F679 File Offset: 0x0005D879
			public static bool IsActive()
			{
				return ALSetupSlot.IsActive(typeof(InventoryItemLookup));
			}

			// Token: 0x170009C0 RID: 2496
			// (get) Token: 0x06002239 RID: 8761 RVA: 0x0005F68A File Offset: 0x0005D88A
			public override object PrimaryRow
			{
				get
				{
					return base.ScanBasis.InventoryItem;
				}
			}

			// Token: 0x170009C1 RID: 2497
			// (get) Token: 0x0600223A RID: 8762 RVA: 0x0005F697 File Offset: 0x0005D897
			protected override string[] ViewNames { get; } = new string[]
			{
				"ISERecords"
			};
		}

		// Token: 0x02000898 RID: 2200
		public class ScanLaborExt : ScanProductionBaseExt<ScanLabor, ScanLabor.Host, AMDocType.labor>
		{
			// Token: 0x0600223C RID: 8764 RVA: 0x0005F6BC File Offset: 0x0005D8BC
			public static bool IsActive()
			{
				return ALSetupSlot.IsActive(typeof(ScanLabor));
			}

			// Token: 0x170009C2 RID: 2498
			// (get) Token: 0x0600223D RID: 8765 RVA: 0x0005F6CD File Offset: 0x0005D8CD
			public override object PrimaryRow
			{
				get
				{
					return base.ScanBasis.Batch;
				}
			}

			// Token: 0x170009C3 RID: 2499
			// (get) Token: 0x0600223E RID: 8766 RVA: 0x0005F6DA File Offset: 0x0005D8DA
			protected override string[] ViewNames { get; } = new string[]
			{
				"batch",
				"transactions",
				"splits"
			};
		}

		// Token: 0x02000899 RID: 2201
		public class ScanMaterialExt : ScanProductionBaseExt<ScanMaterial, ScanMaterial.Host, AMDocType.material>
		{
			// Token: 0x06002240 RID: 8768 RVA: 0x0005F70F File Offset: 0x0005D90F
			public static bool IsActive()
			{
				return ALSetupSlot.IsActive(typeof(ScanMaterial));
			}

			// Token: 0x170009C4 RID: 2500
			// (get) Token: 0x06002241 RID: 8769 RVA: 0x0005F720 File Offset: 0x0005D920
			public override object PrimaryRow
			{
				get
				{
					return base.ScanBasis.Batch;
				}
			}

			// Token: 0x170009C5 RID: 2501
			// (get) Token: 0x06002242 RID: 8770 RVA: 0x0005F72D File Offset: 0x0005D92D
			protected override string[] ViewNames { get; } = new string[]
			{
				"batch",
				"transactions",
				"splits"
			};
		}

		// Token: 0x0200089A RID: 2202
		public class ScanMoveExt : ScanProductionBaseExt<ScanMove, ScanMove.Host, AMDocType.move>
		{
			// Token: 0x06002244 RID: 8772 RVA: 0x0005F762 File Offset: 0x0005D962
			public static bool IsActive()
			{
				return ALSetupSlot.IsActive(typeof(ScanMove));
			}

			// Token: 0x170009C6 RID: 2502
			// (get) Token: 0x06002245 RID: 8773 RVA: 0x0005F773 File Offset: 0x0005D973
			public override object PrimaryRow
			{
				get
				{
					return base.ScanBasis.Batch;
				}
			}

			// Token: 0x170009C7 RID: 2503
			// (get) Token: 0x06002246 RID: 8774 RVA: 0x0005F780 File Offset: 0x0005D980
			protected override string[] ViewNames { get; } = new string[]
			{
				"batch",
				"transactions",
				"splits"
			};
		}

		// Token: 0x0200089B RID: 2203
		public class LookupAndPrintExt : ALScanHandleExt<LookupAndPrint, LookupAndPrint.Host>
		{
			// Token: 0x06002248 RID: 8776 RVA: 0x0005F7B5 File Offset: 0x0005D9B5
			public static bool IsActive()
			{
				return ALSetupSlot.IsActive(typeof(LookupAndPrint));
			}

			// Token: 0x170009C8 RID: 2504
			// (get) Token: 0x06002249 RID: 8777 RVA: 0x0005F7C6 File Offset: 0x0005D9C6
			public override object PrimaryRow
			{
				get
				{
					return base.ScanBasis.SelectedSite;
				}
			}

			// Token: 0x170009C9 RID: 2505
			// (get) Token: 0x0600224A RID: 8778 RVA: 0x0005F7D3 File Offset: 0x0005D9D3
			protected override string[] ViewNames { get; } = new string[]
			{
				"storages"
			};
		}

		// Token: 0x0200089C RID: 2204
		public class PickPackShipExt : ALScanWMSHandleExt<PickPackShip, PickPackShip.Host>, ILabelHandler<PickPackShip, PickPackShip.Host>, ILabelHandler<PickPackShip.Host>
		{
			// Token: 0x0600224C RID: 8780 RVA: 0x0005F7F8 File Offset: 0x0005D9F8
			public static bool IsActive()
			{
				return ALSetupSlot.IsActive(typeof(PickPackShip.Host));
			}

			// Token: 0x170009CA RID: 2506
			// (get) Token: 0x0600224D RID: 8781 RVA: 0x0005F809 File Offset: 0x0005DA09
			public override object PrimaryRow
			{
				get
				{
					return base.ScanBasis.Shipment;
				}
			}

			// Token: 0x0600224E RID: 8782 RVA: 0x0005F818 File Offset: 0x0005DA18
			protected virtual void _(Events.RowSelected<SOShipment> e)
			{
				bool flag = e.Row != null;
				if (flag)
				{
					this._labelConductor.ShowHideModels(e.Row, null);
				}
			}

			// Token: 0x0600224F RID: 8783 RVA: 0x0005F848 File Offset: 0x0005DA48
			[PXOverride]
			public override ScanMode<PickPackShip> DecorateScanMode(ScanMode<PickPackShip> original, Func<ScanMode<PickPackShip>, ScanMode<PickPackShip>> base_DecorateScanMode)
			{
				ScanMode<PickPackShip> scanMode = base_DecorateScanMode(original);
				bool flag = this._labelConductor == null;
				ScanMode<PickPackShip> result;
				if (flag)
				{
					result = scanMode;
				}
				else
				{
					PickPackShip.PickMode pickMode = scanMode as PickPackShip.PickMode;
					bool flag2 = pickMode != null;
					if (flag2)
					{
						this.InjectPrintLabels(pickMode, "Picked");
					}
					else
					{
						PickPackShip.PackMode packMode = scanMode as PickPackShip.PackMode;
						bool flag3 = packMode != null;
						if (flag3)
						{
							this.InjectPrintLabels(packMode, "Packed");
							this.InjectPrintLabels(packMode, "PickedForPack");
							this.InjectPrintLabels(packMode, "ShownPackage");
						}
						else
						{
							PickPackShip.ReturnMode returnMode = scanMode as PickPackShip.ReturnMode;
							bool flag4 = returnMode != null;
							if (flag4)
							{
								this.InjectPrintLabels(returnMode, "Returned");
							}
						}
					}
					result = scanMode;
				}
				return result;
			}
		}
	}
}
