using System;
using System.Collections.Generic;
using AA.Objects.Core;
using AA.Objects.Labels.Integration.AutoPrint;
using AA.Objects.Labels.Integration.BoxPrint;
using AA.Objects.Labels.Integration.FixPackage;
using AA.Objects.Labels.Integration.NbCopies;
using AA.Objects.Labels.Integration.OwnShipment;
using AA.Objects.Labels.Integration.PrinterOverride;
using AA.Objects.License;
using Asgard.Labels.Abstractions.Helpers;
using PX.Common;
using PX.Data;
using Scriban.Runtime;

namespace AA.Objects.Labels
{
	// Token: 0x02000173 RID: 371
	public sealed class ALSetupSlot : IPrefetchable, IPXCompanyDependent
	{
		// Token: 0x170005E1 RID: 1505
		// (get) Token: 0x060010BB RID: 4283 RVA: 0x00036EAC File Offset: 0x000350AC
		private static IList<object> Values
		{
			get
			{
				ALSetupSlot slot = PXDatabase.GetSlot<ALSetupSlot>(typeof(ALSetupSlot).FullName, new Type[]
				{
					typeof(ALSetup)
				});
				if (slot != null)
				{
					return slot.values;
				}
				throw new PXException("Label Basic Preferences not found");
			}
		}

		// Token: 0x170005E2 RID: 1506
		// (get) Token: 0x060010BC RID: 4284 RVA: 0x00036EFC File Offset: 0x000350FC
		public static string LabelaryAPI
		{
			get
			{
				return BasicHelper.GetValue<string>(ALSetupSlot.Values, 0);
			}
		}

		// Token: 0x170005E3 RID: 1507
		// (get) Token: 0x060010BD RID: 4285 RVA: 0x00036F09 File Offset: 0x00035109
		[ScriptMemberIgnore]
		public static string LabelaryAPIKey
		{
			get
			{
				return AsgardCoreUtils.Decrypt(BasicHelper.GetValue<string>(ALSetupSlot.Values, 1));
			}
		}

		// Token: 0x170005E4 RID: 1508
		// (get) Token: 0x060010BE RID: 4286 RVA: 0x00036F1B File Offset: 0x0003511B
		public static string DefaultLanguage
		{
			get
			{
				return BasicHelper.GetValue<string>(ALSetupSlot.Values, 2);
			}
		}

		// Token: 0x170005E5 RID: 1509
		// (get) Token: 0x060010BF RID: 4287 RVA: 0x00036F28 File Offset: 0x00035128
		public static Guid? DefaultMarginID
		{
			get
			{
				return BasicHelper.GetValue<Guid?>(ALSetupSlot.Values, 3);
			}
		}

		// Token: 0x170005E6 RID: 1510
		// (get) Token: 0x060010C0 RID: 4288 RVA: 0x00036F35 File Offset: 0x00035135
		public static Guid? DefaultFormatID
		{
			get
			{
				return BasicHelper.GetValue<Guid?>(ALSetupSlot.Values, 4);
			}
		}

		// Token: 0x170005E7 RID: 1511
		// (get) Token: 0x060010C1 RID: 4289 RVA: 0x00036F42 File Offset: 0x00035142
		public static Guid? DefaultCategoryID
		{
			get
			{
				return BasicHelper.GetValue<Guid?>(ALSetupSlot.Values, 5);
			}
		}

		// Token: 0x170005E8 RID: 1512
		// (get) Token: 0x060010C2 RID: 4290 RVA: 0x00036F4F File Offset: 0x0003514F
		internal static string DevModeValue
		{
			get
			{
				return LicenseHelper.LicenseManager.HasFeature(typeof(ALSetup.devMode)) ? BasicHelper.GetValue<string>(ALSetupSlot.Values, 6) : null;
			}
		}

		// Token: 0x170005E9 RID: 1513
		// (get) Token: 0x060010C3 RID: 4291 RVA: 0x00036F75 File Offset: 0x00035175
		public static bool ShowAutomation
		{
			get
			{
				return LicenseHelper.LicenseManager.HasFeature(typeof(LicenseHelper.Features.ShowAutomation));
			}
		}

		// Token: 0x170005EA RID: 1514
		// (get) Token: 0x060010C4 RID: 4292 RVA: 0x00036F8B File Offset: 0x0003518B
		public static bool EnableAutomation
		{
			get
			{
				return ALSetupSlot.ShowAutomation && LicenseHelper.LicenseManager.HasFeature(typeof(LicenseHelper.Features.EnableAutomation));
			}
		}

		// Token: 0x170005EB RID: 1515
		// (get) Token: 0x060010C5 RID: 4293 RVA: 0x00036FAB File Offset: 0x000351AB
		internal static string EnableCopiesValue
		{
			get
			{
				return BasicHelper.GetValue<string>(ALSetupSlot.Values, 7);
			}
		}

		// Token: 0x170005EC RID: 1516
		// (get) Token: 0x060010C6 RID: 4294 RVA: 0x00036FB8 File Offset: 0x000351B8
		internal static string PrinterOverrideValue
		{
			get
			{
				return BasicHelper.GetValue<string>(ALSetupSlot.Values, 8);
			}
		}

		// Token: 0x170005ED RID: 1517
		// (get) Token: 0x060010C7 RID: 4295 RVA: 0x00036FC5 File Offset: 0x000351C5
		public static string PrintNodeAPI
		{
			get
			{
				return BasicHelper.GetValue<string>(ALSetupSlot.Values, 9);
			}
		}

		// Token: 0x170005EE RID: 1518
		// (get) Token: 0x060010C8 RID: 4296 RVA: 0x00036FD3 File Offset: 0x000351D3
		[ScriptMemberIgnore]
		public static string PrintNodeAPIKey
		{
			get
			{
				return AsgardCoreUtils.Decrypt(BasicHelper.GetValue<string>(ALSetupSlot.Values, 10));
			}
		}

		// Token: 0x170005EF RID: 1519
		// (get) Token: 0x060010C9 RID: 4297 RVA: 0x00036FE6 File Offset: 0x000351E6
		public static bool FixPackageLineNbr
		{
			get
			{
				return BasicHelper.GetValue<bool>(ALSetupSlot.Values, 11) && LicenseHelper.LicenseManager.HasFeature(typeof(ALSOPackageDetailExt));
			}
		}

		// Token: 0x170005F0 RID: 1520
		// (get) Token: 0x060010CA RID: 4298 RVA: 0x0003700D File Offset: 0x0003520D
		public static int? RecordImportMode
		{
			get
			{
				return BasicHelper.GetValue<int?>(ALSetupSlot.Values, 12);
			}
		}

		// Token: 0x170005F1 RID: 1521
		// (get) Token: 0x060010CB RID: 4299 RVA: 0x0003701B File Offset: 0x0003521B
		public static bool BoxPrint
		{
			get
			{
				return BasicHelper.GetValue<bool>(ALSetupSlot.Values, 13) && LicenseHelper.LicenseManager.HasFeature(typeof(ALBoxPrintSOShipmentEntryExt));
			}
		}

		// Token: 0x170005F2 RID: 1522
		// (get) Token: 0x060010CC RID: 4300 RVA: 0x00037042 File Offset: 0x00035242
		public static Guid? BoxPrintModelID
		{
			get
			{
				return BasicHelper.GetValue<Guid?>(ALSetupSlot.Values, 14);
			}
		}

		// Token: 0x170005F3 RID: 1523
		// (get) Token: 0x060010CD RID: 4301 RVA: 0x00037050 File Offset: 0x00035250
		public static bool OwnShipment
		{
			get
			{
				return BasicHelper.GetValue<bool>(ALSetupSlot.Values, 15) && LicenseHelper.LicenseManager.HasFeature(typeof(ALOwnShipmentSOShipmentEntryExt));
			}
		}

		// Token: 0x170005F4 RID: 1524
		// (get) Token: 0x060010CE RID: 4302 RVA: 0x00037077 File Offset: 0x00035277
		public static bool PrintOnConfirm
		{
			get
			{
				return BasicHelper.GetValue<bool>(ALSetupSlot.Values, 16) && LicenseHelper.LicenseManager.HasFeature(typeof(ALPrintOnConfirmSOShipmentEntryExt));
			}
		}

		// Token: 0x170005F5 RID: 1525
		// (get) Token: 0x060010CF RID: 4303 RVA: 0x0003709E File Offset: 0x0003529E
		public static Guid? PrintOnConfirmModelID
		{
			get
			{
				return BasicHelper.GetValue<Guid?>(ALSetupSlot.Values, 17);
			}
		}

		// Token: 0x170005F6 RID: 1526
		// (get) Token: 0x060010D0 RID: 4304 RVA: 0x000370AC File Offset: 0x000352AC
		public static string LabelZoomAPI
		{
			get
			{
				return BasicHelper.GetValue<string>(ALSetupSlot.Values, 18);
			}
		}

		// Token: 0x170005F7 RID: 1527
		// (get) Token: 0x060010D1 RID: 4305 RVA: 0x000370BA File Offset: 0x000352BA
		[ScriptMemberIgnore]
		public static string LabelZoomAPIKey
		{
			get
			{
				return AsgardCoreUtils.Decrypt(BasicHelper.GetValue<string>(ALSetupSlot.Values, 19));
			}
		}

		// Token: 0x170005F8 RID: 1528
		// (get) Token: 0x060010D2 RID: 4306 RVA: 0x000370CD File Offset: 0x000352CD
		public static string MongoURL
		{
			get
			{
				return BasicHelper.GetValue<string>(ALSetupSlot.Values, 20);
			}
		}

		// Token: 0x170005F9 RID: 1529
		// (get) Token: 0x060010D3 RID: 4307 RVA: 0x000370DB File Offset: 0x000352DB
		[ScriptMemberIgnore]
		public static string MongoOptions
		{
			get
			{
				return AsgardCoreUtils.Decrypt(BasicHelper.GetValue<string>(ALSetupSlot.Values, 21));
			}
		}

		// Token: 0x170005FA RID: 1530
		// (get) Token: 0x060010D4 RID: 4308 RVA: 0x000370EE File Offset: 0x000352EE
		public static Guid? LabelZoomCategoryID
		{
			get
			{
				return BasicHelper.GetValue<Guid?>(ALSetupSlot.Values, 22);
			}
		}

		// Token: 0x170005FB RID: 1531
		// (get) Token: 0x060010D5 RID: 4309 RVA: 0x000370FC File Offset: 0x000352FC
		public static Guid? LabelZoomImageSubstitutionID
		{
			get
			{
				return BasicHelper.GetValue<Guid?>(ALSetupSlot.Values, 23);
			}
		}

		// Token: 0x170005FC RID: 1532
		// (get) Token: 0x060010D6 RID: 4310 RVA: 0x0003710A File Offset: 0x0003530A
		internal static string GraphIntegrationValue
		{
			get
			{
				return BasicHelper.GetValue<string>(ALSetupSlot.Values, 24);
			}
		}

		// Token: 0x170005FD RID: 1533
		// (get) Token: 0x060010D7 RID: 4311 RVA: 0x00037118 File Offset: 0x00035318
		public static bool ProductionTicketViaCloud
		{
			get
			{
				return BasicHelper.GetValue<bool>(ALSetupSlot.Values, 25);
			}
		}

		// Token: 0x170005FE RID: 1534
		// (get) Token: 0x060010D8 RID: 4312 RVA: 0x00037126 File Offset: 0x00035326
		public static Guid? ProductionTicketModelID
		{
			get
			{
				return BasicHelper.GetValue<Guid?>(ALSetupSlot.Values, 26);
			}
		}

		// Token: 0x170005FF RID: 1535
		// (get) Token: 0x060010D9 RID: 4313 RVA: 0x00037134 File Offset: 0x00035334
		public static Guid? ProductionTicketRuleID
		{
			get
			{
				return BasicHelper.GetValue<Guid?>(ALSetupSlot.Values, 27);
			}
		}

		// Token: 0x17000600 RID: 1536
		// (get) Token: 0x060010DA RID: 4314 RVA: 0x00037142 File Offset: 0x00035342
		public static bool GenerateShipment2D
		{
			get
			{
				return BasicHelper.GetValue<bool>(ALSetupSlot.Values, 28);
			}
		}

		// Token: 0x17000601 RID: 1537
		// (get) Token: 0x060010DB RID: 4315 RVA: 0x00037150 File Offset: 0x00035350
		public static Guid? Shipment2DModelID
		{
			get
			{
				return BasicHelper.GetValue<Guid?>(ALSetupSlot.Values, 29);
			}
		}

		// Token: 0x17000602 RID: 1538
		// (get) Token: 0x060010DC RID: 4316 RVA: 0x0003715E File Offset: 0x0003535E
		public static string PackingListReportID
		{
			get
			{
				return BasicHelper.GetValue<string>(ALSetupSlot.Values, 30);
			}
		}

		// Token: 0x17000603 RID: 1539
		// (get) Token: 0x060010DD RID: 4317 RVA: 0x0003716C File Offset: 0x0003536C
		public static bool CarrierLabelsViaCloud
		{
			get
			{
				return BasicHelper.GetValue<bool>(ALSetupSlot.Values, 31);
			}
		}

		// Token: 0x17000604 RID: 1540
		// (get) Token: 0x060010DE RID: 4318 RVA: 0x0003717A File Offset: 0x0003537A
		public static Guid? CarrierLabelsModelID
		{
			get
			{
				return BasicHelper.GetValue<Guid?>(ALSetupSlot.Values, 32);
			}
		}

		// Token: 0x17000605 RID: 1541
		// (get) Token: 0x060010DF RID: 4319 RVA: 0x00037188 File Offset: 0x00035388
		public static bool CopiesOverrideSO
		{
			get
			{
				return BasicHelper.HasFlag<ALNbCopies.Options>(ALSetupSlot.EnableCopiesValue, ALNbCopies.Options.SalesOrderLine) && LicenseHelper.LicenseManager.HasFeature(typeof(ALNbCopiesSOLineExt));
			}
		}

		// Token: 0x17000606 RID: 1542
		// (get) Token: 0x060010E0 RID: 4320 RVA: 0x000371AE File Offset: 0x000353AE
		public static bool CopiesOverrideSH
		{
			get
			{
				return BasicHelper.HasFlag<ALNbCopies.Options>(ALSetupSlot.EnableCopiesValue, ALNbCopies.Options.ShipmentLine) && LicenseHelper.LicenseManager.HasFeature(typeof(ALNbCopiesSOShipLineExt));
			}
		}

		// Token: 0x17000607 RID: 1543
		// (get) Token: 0x060010E1 RID: 4321 RVA: 0x000371D4 File Offset: 0x000353D4
		public static bool CopiesOverridePO
		{
			get
			{
				return BasicHelper.HasFlag<ALNbCopies.Options>(ALSetupSlot.EnableCopiesValue, ALNbCopies.Options.PurchaseOrderLine) && LicenseHelper.LicenseManager.HasFeature(typeof(ALNbCopiesPOLineExt));
			}
		}

		// Token: 0x17000608 RID: 1544
		// (get) Token: 0x060010E2 RID: 4322 RVA: 0x000371FA File Offset: 0x000353FA
		public static bool CopiesOverridePR
		{
			get
			{
				return BasicHelper.HasFlag<ALNbCopies.Options>(ALSetupSlot.EnableCopiesValue, ALNbCopies.Options.PurchaseReceiptLine) && LicenseHelper.LicenseManager.HasFeature(typeof(ALNbCopiesPOReceiptLineExt));
			}
		}

		// Token: 0x17000609 RID: 1545
		// (get) Token: 0x060010E3 RID: 4323 RVA: 0x00037221 File Offset: 0x00035421
		public static bool CopiesOverrideKA
		{
			get
			{
				return BasicHelper.HasFlag<ALNbCopies.Options>(ALSetupSlot.EnableCopiesValue, ALNbCopies.Options.KitAssemblyLine) && LicenseHelper.LicenseManager.HasFeature(typeof(ALNbCopiesINKitTranSplitExt));
			}
		}

		// Token: 0x1700060A RID: 1546
		// (get) Token: 0x060010E4 RID: 4324 RVA: 0x00037248 File Offset: 0x00035448
		public static bool CopiesOverrideSP
		{
			get
			{
				return BasicHelper.HasFlag<ALNbCopies.Options>(ALSetupSlot.EnableCopiesValue, ALNbCopies.Options.ShipmentPackage) && LicenseHelper.LicenseManager.HasFeature(typeof(ALNbCopiesSOPackageDetailExt));
			}
		}

		// Token: 0x1700060B RID: 1547
		// (get) Token: 0x060010E5 RID: 4325 RVA: 0x0003726E File Offset: 0x0003546E
		public static bool CopiesOverrideLO
		{
			get
			{
				return BasicHelper.HasFlag<ALNbCopies.Options>(ALSetupSlot.EnableCopiesValue, ALNbCopies.Options.WarehouseLocation) && LicenseHelper.LicenseManager.HasFeature(typeof(ALNbCopiesINLocationExt));
			}
		}

		// Token: 0x1700060C RID: 1548
		// (get) Token: 0x060010E6 RID: 4326 RVA: 0x00037295 File Offset: 0x00035495
		public static bool CopiesOverrideAM
		{
			get
			{
				return BasicHelper.HasFlag<ALNbCopies.Options>(ALSetupSlot.EnableCopiesValue, ALNbCopies.Options.ProductionOrder) && LicenseHelper.LicenseManager.HasFeature(typeof(ALNbCopiesAMProdItemSplitExt));
			}
		}

		// Token: 0x1700060D RID: 1549
		// (get) Token: 0x060010E7 RID: 4327 RVA: 0x000372BF File Offset: 0x000354BF
		public static bool CopiesOverrideIA
		{
			get
			{
				return BasicHelper.HasFlag<ALNbCopies.Options>(ALSetupSlot.EnableCopiesValue, ALNbCopies.Options.InventoryAdjustment) && LicenseHelper.LicenseManager.HasFeature(typeof(ALNbCopiesINAdjustmentEntryExt));
			}
		}

		// Token: 0x1700060E RID: 1550
		// (get) Token: 0x060010E8 RID: 4328 RVA: 0x000372E9 File Offset: 0x000354E9
		public static bool CopiesOverrideII
		{
			get
			{
				return BasicHelper.HasFlag<ALNbCopies.Options>(ALSetupSlot.EnableCopiesValue, ALNbCopies.Options.InventoryIssue) && LicenseHelper.LicenseManager.HasFeature(typeof(ALNbCopiesINIssueEntryExt));
			}
		}

		// Token: 0x1700060F RID: 1551
		// (get) Token: 0x060010E9 RID: 4329 RVA: 0x00037313 File Offset: 0x00035513
		public static bool CopiesOverrideIR
		{
			get
			{
				return BasicHelper.HasFlag<ALNbCopies.Options>(ALSetupSlot.EnableCopiesValue, ALNbCopies.Options.InventoryReceipt) && LicenseHelper.LicenseManager.HasFeature(typeof(ALNbCopiesINReceiptEntryExt));
			}
		}

		// Token: 0x17000610 RID: 1552
		// (get) Token: 0x060010EA RID: 4330 RVA: 0x0003733D File Offset: 0x0003553D
		public static bool CopiesOverrideIT
		{
			get
			{
				return BasicHelper.HasFlag<ALNbCopies.Options>(ALSetupSlot.EnableCopiesValue, ALNbCopies.Options.InventoryTransfer) && LicenseHelper.LicenseManager.HasFeature(typeof(ALNbCopiesINTransferEntryExt));
			}
		}

		// Token: 0x17000611 RID: 1553
		// (get) Token: 0x060010EB RID: 4331 RVA: 0x00037367 File Offset: 0x00035567
		public static bool CopiesOverridePIReview
		{
			get
			{
				return BasicHelper.HasFlag<ALNbCopies.Options>(ALSetupSlot.EnableCopiesValue, ALNbCopies.Options.PhysicalInventoryReview) && LicenseHelper.LicenseManager.HasFeature(typeof(ALNbCopiesINPIReviewExt));
			}
		}

		// Token: 0x17000612 RID: 1554
		// (get) Token: 0x060010EC RID: 4332 RVA: 0x00037391 File Offset: 0x00035591
		public static bool CopiesOverridePICount
		{
			get
			{
				return BasicHelper.HasFlag<ALNbCopies.Options>(ALSetupSlot.EnableCopiesValue, ALNbCopies.Options.PhysicalInventoryCount) && LicenseHelper.LicenseManager.HasFeature(typeof(ALNbCopiesINPICountEntryExt));
			}
		}

		// Token: 0x17000613 RID: 1555
		// (get) Token: 0x060010ED RID: 4333 RVA: 0x000373BB File Offset: 0x000355BB
		public static bool PrinterOverrideSI
		{
			get
			{
				return BasicHelper.HasFlag<ALPrinterOverride.Options>(ALSetupSlot.PrinterOverrideValue, ALPrinterOverride.Options.StockItem) && LicenseHelper.LicenseManager.HasFeature(typeof(ALPrinterOverrideInventoryItemExt));
			}
		}

		// Token: 0x17000614 RID: 1556
		// (get) Token: 0x060010EE RID: 4334 RVA: 0x000373E1 File Offset: 0x000355E1
		public static bool PrinterOverrideNS
		{
			get
			{
				return BasicHelper.HasFlag<ALPrinterOverride.Options>(ALSetupSlot.PrinterOverrideValue, ALPrinterOverride.Options.NonStockItem) && LicenseHelper.LicenseManager.HasFeature(typeof(ALPrinterOverrideInventoryItemExt));
			}
		}

		// Token: 0x17000615 RID: 1557
		// (get) Token: 0x060010EF RID: 4335 RVA: 0x00037407 File Offset: 0x00035607
		public static bool PrinterOverrideTI
		{
			get
			{
				return BasicHelper.HasFlag<ALPrinterOverride.Options>(ALSetupSlot.PrinterOverrideValue, ALPrinterOverride.Options.TemplateItem) && LicenseHelper.LicenseManager.HasFeature(typeof(ALPrinterOverrideInventoryItemExt));
			}
		}

		// Token: 0x17000616 RID: 1558
		// (get) Token: 0x060010F0 RID: 4336 RVA: 0x0003742D File Offset: 0x0003562D
		public static bool PrinterOverridePR
		{
			get
			{
				return BasicHelper.HasFlag<ALPrinterOverride.Options>(ALSetupSlot.PrinterOverrideValue, ALPrinterOverride.Options.PurchaseReceipt) && LicenseHelper.LicenseManager.HasFeature(typeof(ALPrinterOverridePOReceiptExt));
			}
		}

		// Token: 0x17000617 RID: 1559
		// (get) Token: 0x060010F1 RID: 4337 RVA: 0x00037453 File Offset: 0x00035653
		public static bool PrinterOverrideIA
		{
			get
			{
				return BasicHelper.HasFlag<ALPrinterOverride.Options>(ALSetupSlot.PrinterOverrideValue, ALPrinterOverride.Options.InventoryAdjustment) && LicenseHelper.LicenseManager.HasFeature(typeof(ALPrinterOverrideINRegisterExt));
			}
		}

		// Token: 0x17000618 RID: 1560
		// (get) Token: 0x060010F2 RID: 4338 RVA: 0x0003747D File Offset: 0x0003567D
		public static bool PrinterOverrideII
		{
			get
			{
				return BasicHelper.HasFlag<ALPrinterOverride.Options>(ALSetupSlot.PrinterOverrideValue, ALPrinterOverride.Options.InventoryIssue) && LicenseHelper.LicenseManager.HasFeature(typeof(ALPrinterOverrideINRegisterExt));
			}
		}

		// Token: 0x17000619 RID: 1561
		// (get) Token: 0x060010F3 RID: 4339 RVA: 0x000374A4 File Offset: 0x000356A4
		public static bool PrinterOverrideIR
		{
			get
			{
				return BasicHelper.HasFlag<ALPrinterOverride.Options>(ALSetupSlot.PrinterOverrideValue, ALPrinterOverride.Options.InventoryReceipt) && LicenseHelper.LicenseManager.HasFeature(typeof(ALPrinterOverrideINRegisterExt));
			}
		}

		// Token: 0x1700061A RID: 1562
		// (get) Token: 0x060010F4 RID: 4340 RVA: 0x000374CB File Offset: 0x000356CB
		public static bool PrinterOverrideIT
		{
			get
			{
				return BasicHelper.HasFlag<ALPrinterOverride.Options>(ALSetupSlot.PrinterOverrideValue, ALPrinterOverride.Options.InventoryTransfer) && LicenseHelper.LicenseManager.HasFeature(typeof(ALPrinterOverrideINRegisterExt));
			}
		}

		// Token: 0x1700061B RID: 1563
		// (get) Token: 0x060010F5 RID: 4341 RVA: 0x000374F2 File Offset: 0x000356F2
		public static bool PrinterOverrideAM
		{
			get
			{
				return BasicHelper.HasFlag<ALPrinterOverride.Options>(ALSetupSlot.PrinterOverrideValue, ALPrinterOverride.Options.ProductionOrder) && LicenseHelper.LicenseManager.HasFeature(typeof(ALPrinterOverrideAMProdItemExt));
			}
		}

		// Token: 0x1700061C RID: 1564
		// (get) Token: 0x060010F6 RID: 4342 RVA: 0x0003751C File Offset: 0x0003571C
		public static bool PrinterOverridePIReview
		{
			get
			{
				return BasicHelper.HasFlag<ALPrinterOverride.Options>(ALSetupSlot.PrinterOverrideValue, ALPrinterOverride.Options.PhysicalInventoryReview) && LicenseHelper.LicenseManager.HasFeature(typeof(ALPrinterOverrideINPIHeaderExt));
			}
		}

		// Token: 0x1700061D RID: 1565
		// (get) Token: 0x060010F7 RID: 4343 RVA: 0x00037546 File Offset: 0x00035746
		public static bool PrinterOverridePICount
		{
			get
			{
				return BasicHelper.HasFlag<ALPrinterOverride.Options>(ALSetupSlot.PrinterOverrideValue, ALPrinterOverride.Options.PhysicalInventoryCount) && LicenseHelper.LicenseManager.HasFeature(typeof(ALPrinterOverrideINPIHeaderExt));
			}
		}

		// Token: 0x1700061E RID: 1566
		// (get) Token: 0x060010F8 RID: 4344 RVA: 0x00037570 File Offset: 0x00035770
		public static bool DevMode
		{
			get
			{
				return BasicHelper.HasFlag<ALDev.Options>(ALSetupSlot.DevModeValue, ALDev.Options.DevMode);
			}
		}

		// Token: 0x1700061F RID: 1567
		// (get) Token: 0x060010F9 RID: 4345 RVA: 0x0003757D File Offset: 0x0003577D
		public static bool SaveRendered
		{
			get
			{
				return BasicHelper.HasFlag<ALDev.Options>(ALSetupSlot.DevModeValue, ALDev.Options.SaveRendered);
			}
		}

		// Token: 0x17000620 RID: 1568
		// (get) Token: 0x060010FA RID: 4346 RVA: 0x0003758A File Offset: 0x0003578A
		public static bool AddLineNumber
		{
			get
			{
				return ALSetupSlot.SaveRendered && BasicHelper.HasFlag<ALDev.Options>(ALSetupSlot.DevModeValue, ALDev.Options.AddLineNumber);
			}
		}

		// Token: 0x17000621 RID: 1569
		// (get) Token: 0x060010FB RID: 4347 RVA: 0x000375A1 File Offset: 0x000357A1
		public static bool AddComments
		{
			get
			{
				return ALSetupSlot.SaveRendered && BasicHelper.HasFlag<ALDev.Options>(ALSetupSlot.DevModeValue, ALDev.Options.AddComments);
			}
		}

		// Token: 0x17000622 RID: 1570
		// (get) Token: 0x060010FC RID: 4348 RVA: 0x000375B8 File Offset: 0x000357B8
		public static bool AutoPrint
		{
			get
			{
				return BasicHelper.HasFlag<ALDev.Options>(ALSetupSlot.DevModeValue, ALDev.Options.AutoPrint);
			}
		}

		// Token: 0x17000623 RID: 1571
		// (get) Token: 0x060010FD RID: 4349 RVA: 0x000375C6 File Offset: 0x000357C6
		public static bool EnableLabelZoom
		{
			get
			{
				return BasicHelper.HasFlag<ALDev.Options>(ALSetupSlot.DevModeValue, ALDev.Options.LabelZoom);
			}
		}

		// Token: 0x17000624 RID: 1572
		// (get) Token: 0x060010FE RID: 4350 RVA: 0x000375D4 File Offset: 0x000357D4
		public static bool EnableMongoDb
		{
			get
			{
				return BasicHelper.HasFlag<ALDev.Options>(ALSetupSlot.DevModeValue, ALDev.Options.MongoDb);
			}
		}

		// Token: 0x17000625 RID: 1573
		// (get) Token: 0x060010FF RID: 4351 RVA: 0x000375E5 File Offset: 0x000357E5
		public static bool LoadExternalLibs
		{
			get
			{
				return BasicHelper.HasFlag<ALDev.Options>(ALSetupSlot.DevModeValue, ALDev.Options.LoadExternalLibs);
			}
		}

		// Token: 0x17000626 RID: 1574
		// (get) Token: 0x06001100 RID: 4352 RVA: 0x000375F3 File Offset: 0x000357F3
		public static bool ShowTemplate
		{
			get
			{
				return BasicHelper.HasFlag<ALDev.Options>(ALSetupSlot.DevModeValue, ALDev.Options.ShowTemplate);
			}
		}

		// Token: 0x06001101 RID: 4353 RVA: 0x00037604 File Offset: 0x00035804
		public static bool IsActive(Type type)
		{
			Enum @enum = AsgardUtils.ToEnumOption(type);
			return BasicHelper.HasFlag((@enum != null) ? @enum.GetType() : null, ALSetupSlot.GraphIntegrationValue, AsgardUtils.ToEnumOption(type)) && LicenseHelper.LicenseManager.HasFeature(type);
		}

		// Token: 0x06001102 RID: 4354 RVA: 0x00037638 File Offset: 0x00035838
		public void Prefetch()
		{
			PXDataRecord pxdataRecord = PXDatabase.SelectSingle<ALSetup>(new PXDataField[]
			{
				new PXDataField<ALSetup.labelaryAPI>(),
				new PXDataField<ALSetup.labelaryAPIKey>(),
				new PXDataField<ALSetup.defaultLanguage>(),
				new PXDataField<ALSetup.defaultMarginID>(),
				new PXDataField<ALSetup.defaultFormatID>(),
				new PXDataField<ALSetup.defaultCategoryID>(),
				new PXDataField<ALSetup.devMode>(),
				new PXDataField<ALSetup.enableCopiesOverride>(),
				new PXDataField<ALSetup.enablePrinterOverride>(),
				new PXDataField<ALSetup.printNodeAPI>(),
				new PXDataField<ALSetup.printNodeAPIKey>(ALSetupSlot.ALIAS_SE),
				new PXDataField<ALSetup.fixPackageLineNbr>(),
				new PXDataField<ALSetup.recordImportMode>(),
				new PXDataField<ALSetup.boxPrint>(),
				new PXDataField<ALSetup.boxPrintModelID>(),
				new PXDataField<ALSetup.ownShipment>(),
				new PXDataField<ALSetup.printOnConfirm>(),
				new PXDataField<ALSetup.printOnConfirmModelID>(),
				new PXDataField<ALSetup.labelZoomAPI>(),
				new PXDataField<ALSetup.labelZoomAPIKey>(),
				new PXDataField<ALSetup.mongoURL>(),
				new PXDataField<ALSetup.mongoOptions>(),
				new PXDataField<ALSetup.labelZoomCategoryID>(),
				new PXDataField<ALSetup.labelZoomImageSubstitutionID>(),
				new PXDataField<ALSetup.enableIntegration>(),
				new PXDataField<ALSetup.printProductionTicketViaCloud>(),
				new PXDataField<ALSetup.printProductionTicketModelID>(),
				new PXDataField<ALSetup.printProductionTicketRuleID>(),
				new PXDataField<ALSetup.generateShipment2D>(),
				new PXDataField<ALSetup.shipment2DModelID>(),
				new PXDataField<ALSetup.packingListReportID>(),
				new PXDataField<ALSetup.printCarrierLabelsViaCloud>(),
				new PXDataField<ALSetup.printCarrierLabelsModelID>()
			});
			this.values.Clear();
			int num = 0;
			bool flag = pxdataRecord == null;
			if (!flag)
			{
				this.values.Add(pxdataRecord.GetString(num++));
				this.values.Add(pxdataRecord.GetString(num++));
				this.values.Add(pxdataRecord.GetString(num++));
				this.values.Add(pxdataRecord.GetGuid(num++));
				this.values.Add(pxdataRecord.GetGuid(num++));
				this.values.Add(pxdataRecord.GetGuid(num++));
				this.values.Add(pxdataRecord.GetString(num++));
				this.values.Add(pxdataRecord.GetString(num++));
				this.values.Add(pxdataRecord.GetString(num++));
				this.values.Add(pxdataRecord.GetString(num++));
				this.values.Add(pxdataRecord.GetString(num++));
				this.values.Add(pxdataRecord.GetBoolean(num++).GetValueOrDefault());
				this.values.Add(pxdataRecord.GetInt32(num++));
				this.values.Add(pxdataRecord.GetBoolean(num++).GetValueOrDefault());
				this.values.Add(pxdataRecord.GetGuid(num++));
				this.values.Add(pxdataRecord.GetBoolean(num++).GetValueOrDefault());
				this.values.Add(pxdataRecord.GetBoolean(num++).GetValueOrDefault());
				this.values.Add(pxdataRecord.GetGuid(num++));
				this.values.Add(pxdataRecord.GetString(num++));
				this.values.Add(pxdataRecord.GetString(num++));
				this.values.Add(pxdataRecord.GetString(num++));
				this.values.Add(pxdataRecord.GetString(num++));
				this.values.Add(pxdataRecord.GetGuid(num++));
				this.values.Add(pxdataRecord.GetGuid(num++));
				this.values.Add(pxdataRecord.GetString(num++));
				this.values.Add(pxdataRecord.GetBoolean(num++).GetValueOrDefault());
				this.values.Add(pxdataRecord.GetGuid(num++));
				this.values.Add(pxdataRecord.GetGuid(num++));
				this.values.Add(pxdataRecord.GetBoolean(num++).GetValueOrDefault());
				this.values.Add(pxdataRecord.GetGuid(num++));
				this.values.Add(pxdataRecord.GetString(num++));
				this.values.Add(pxdataRecord.GetBoolean(num++).GetValueOrDefault());
				this.values.Add(pxdataRecord.GetGuid(num++));
			}
		}

		// Token: 0x04000713 RID: 1811
		private static readonly string ALIAS_SE = "ALSetup";

		// Token: 0x04000714 RID: 1812
		private readonly IList<object> values = new List<object>();
	}
}
