using System;
using System.Collections.Generic;
using AA.Objects.AL.Integration.AutoPrint;
using AA.Objects.AL.Integration.BoxPrint;
using AA.Objects.AL.Integration.FixPackage;
using AA.Objects.AL.Integration.NbCopies;
using AA.Objects.AL.Integration.OwnShipment;
using AA.Objects.AL.Integration.PrinterOverride;
using AA.Objects.AL.License;
using PX.Common;
using PX.Data;
using PX.DbServices.QueryObjectModel;
using Scriban.Runtime;

namespace AA.Objects.AL
{
	// Token: 0x020001FC RID: 508
	public sealed class ALSetupSlot : IPrefetchable, IPXCompanyDependent
	{
		// Token: 0x170006EE RID: 1774
		// (get) Token: 0x060015C0 RID: 5568 RVA: 0x0004F31C File Offset: 0x0004D51C
		private static IList<object> Values
		{
			get
			{
				ALSetupSlot slot = PXDatabase.GetSlot<ALSetupSlot>(typeof(ALSetupSlot).FullName, new Type[]
				{
					typeof(ALSetup)
				});
				bool flag = slot == null;
				if (flag)
				{
					throw new PXException("Label Basic Preferences not found");
				}
				return slot.values;
			}
		}

		// Token: 0x170006EF RID: 1775
		// (get) Token: 0x060015C1 RID: 5569 RVA: 0x0004F370 File Offset: 0x0004D570
		public static string LabelaryAPI
		{
			get
			{
				return AsgardUtils.GetValue<string>(ALSetupSlot.Values, 0);
			}
		}

		// Token: 0x170006F0 RID: 1776
		// (get) Token: 0x060015C2 RID: 5570 RVA: 0x0004F37D File Offset: 0x0004D57D
		[ScriptMemberIgnore]
		public static string LabelaryAPIKey
		{
			get
			{
				return HiddenUtils.Decrypt(AsgardUtils.GetValue<string>(ALSetupSlot.Values, 1));
			}
		}

		// Token: 0x170006F1 RID: 1777
		// (get) Token: 0x060015C3 RID: 5571 RVA: 0x0004F38F File Offset: 0x0004D58F
		public static string DefaultLanguage
		{
			get
			{
				return AsgardUtils.GetValue<string>(ALSetupSlot.Values, 2);
			}
		}

		// Token: 0x170006F2 RID: 1778
		// (get) Token: 0x060015C4 RID: 5572 RVA: 0x0004F39C File Offset: 0x0004D59C
		public static Guid? DefaultMarginID
		{
			get
			{
				return AsgardUtils.GetValue<Guid?>(ALSetupSlot.Values, 3);
			}
		}

		// Token: 0x170006F3 RID: 1779
		// (get) Token: 0x060015C5 RID: 5573 RVA: 0x0004F3A9 File Offset: 0x0004D5A9
		public static Guid? DefaultFormatID
		{
			get
			{
				return AsgardUtils.GetValue<Guid?>(ALSetupSlot.Values, 4);
			}
		}

		// Token: 0x170006F4 RID: 1780
		// (get) Token: 0x060015C6 RID: 5574 RVA: 0x0004F3B6 File Offset: 0x0004D5B6
		public static Guid? DefaultCategoryID
		{
			get
			{
				return AsgardUtils.GetValue<Guid?>(ALSetupSlot.Values, 5);
			}
		}

		// Token: 0x170006F5 RID: 1781
		// (get) Token: 0x060015C7 RID: 5575 RVA: 0x0004F3C3 File Offset: 0x0004D5C3
		internal static string DevModeValue
		{
			get
			{
				return LicenseHelper.LicenseManager.HasFeature(typeof(ALSetup.devMode)) ? AsgardUtils.GetValue<string>(ALSetupSlot.Values, 6) : null;
			}
		}

		// Token: 0x170006F6 RID: 1782
		// (get) Token: 0x060015C8 RID: 5576 RVA: 0x0004F3E9 File Offset: 0x0004D5E9
		public static bool ShowAutomation
		{
			get
			{
				return LicenseHelper.LicenseManager.HasFeature(typeof(LicenseHelper.Features.ShowAutomation));
			}
		}

		// Token: 0x170006F7 RID: 1783
		// (get) Token: 0x060015C9 RID: 5577 RVA: 0x0004F3FF File Offset: 0x0004D5FF
		public static bool EnableAutomation
		{
			get
			{
				return ALSetupSlot.ShowAutomation && LicenseHelper.LicenseManager.HasFeature(typeof(LicenseHelper.Features.EnableAutomation));
			}
		}

		// Token: 0x170006F8 RID: 1784
		// (get) Token: 0x060015CA RID: 5578 RVA: 0x0004F41F File Offset: 0x0004D61F
		internal static string EnableCopiesValue
		{
			get
			{
				return AsgardUtils.GetValue<string>(ALSetupSlot.Values, 7);
			}
		}

		// Token: 0x170006F9 RID: 1785
		// (get) Token: 0x060015CB RID: 5579 RVA: 0x0004F42C File Offset: 0x0004D62C
		internal static string PrinterOverrideValue
		{
			get
			{
				return AsgardUtils.GetValue<string>(ALSetupSlot.Values, 8);
			}
		}

		// Token: 0x170006FA RID: 1786
		// (get) Token: 0x060015CC RID: 5580 RVA: 0x0004F439 File Offset: 0x0004D639
		public static string PrintNodeAPI
		{
			get
			{
				return AsgardUtils.GetValue<string>(ALSetupSlot.Values, 9);
			}
		}

		// Token: 0x170006FB RID: 1787
		// (get) Token: 0x060015CD RID: 5581 RVA: 0x0004F447 File Offset: 0x0004D647
		[ScriptMemberIgnore]
		public static string PrintNodeAPIKey
		{
			get
			{
				return HiddenUtils.Decrypt(AsgardUtils.GetValue<string>(ALSetupSlot.Values, 10));
			}
		}

		// Token: 0x170006FC RID: 1788
		// (get) Token: 0x060015CE RID: 5582 RVA: 0x0004F45A File Offset: 0x0004D65A
		public static bool FixPackageLineNbr
		{
			get
			{
				return AsgardUtils.GetValue<bool>(ALSetupSlot.Values, 11) && LicenseHelper.LicenseManager.HasFeature(typeof(ALSOPackageDetailExt));
			}
		}

		// Token: 0x170006FD RID: 1789
		// (get) Token: 0x060015CF RID: 5583 RVA: 0x0004F481 File Offset: 0x0004D681
		public static int? RecordImportMode
		{
			get
			{
				return AsgardUtils.GetValue<int?>(ALSetupSlot.Values, 12);
			}
		}

		// Token: 0x170006FE RID: 1790
		// (get) Token: 0x060015D0 RID: 5584 RVA: 0x0004F48F File Offset: 0x0004D68F
		public static Guid? RenderingPrinterID
		{
			get
			{
				return AsgardUtils.GetValue<Guid?>(ALSetupSlot.Values, 13);
			}
		}

		// Token: 0x170006FF RID: 1791
		// (get) Token: 0x060015D1 RID: 5585 RVA: 0x0004F49D File Offset: 0x0004D69D
		public static bool BoxPrint
		{
			get
			{
				return AsgardUtils.GetValue<bool>(ALSetupSlot.Values, 14) && LicenseHelper.LicenseManager.HasFeature(typeof(ALBoxPrintSOShipmentEntryExt));
			}
		}

		// Token: 0x17000700 RID: 1792
		// (get) Token: 0x060015D2 RID: 5586 RVA: 0x0004F4C4 File Offset: 0x0004D6C4
		public static Guid? BoxPrintModelID
		{
			get
			{
				return AsgardUtils.GetValue<Guid?>(ALSetupSlot.Values, 15);
			}
		}

		// Token: 0x17000701 RID: 1793
		// (get) Token: 0x060015D3 RID: 5587 RVA: 0x0004F4D2 File Offset: 0x0004D6D2
		public static bool OwnShipment
		{
			get
			{
				return AsgardUtils.GetValue<bool>(ALSetupSlot.Values, 16) && LicenseHelper.LicenseManager.HasFeature(typeof(ALOwnShipmentSOShipmentEntryExt));
			}
		}

		// Token: 0x17000702 RID: 1794
		// (get) Token: 0x060015D4 RID: 5588 RVA: 0x0004F4F9 File Offset: 0x0004D6F9
		public static bool PrintOnConfirm
		{
			get
			{
				return AsgardUtils.GetValue<bool>(ALSetupSlot.Values, 17) && LicenseHelper.LicenseManager.HasFeature(typeof(ALPrintOnConfirmSOShipmentEntryExt));
			}
		}

		// Token: 0x17000703 RID: 1795
		// (get) Token: 0x060015D5 RID: 5589 RVA: 0x0004F520 File Offset: 0x0004D720
		public static Guid? PrintOnConfirmModelID
		{
			get
			{
				return AsgardUtils.GetValue<Guid?>(ALSetupSlot.Values, 18);
			}
		}

		// Token: 0x17000704 RID: 1796
		// (get) Token: 0x060015D6 RID: 5590 RVA: 0x0004F52E File Offset: 0x0004D72E
		public static string PrinterName
		{
			get
			{
				return AsgardUtils.GetValue<string>(ALSetupSlot.Values, 19);
			}
		}

		// Token: 0x17000705 RID: 1797
		// (get) Token: 0x060015D7 RID: 5591 RVA: 0x0004F53C File Offset: 0x0004D73C
		public static string PrinterType
		{
			get
			{
				return AsgardUtils.GetValue<string>(ALSetupSlot.Values, 20);
			}
		}

		// Token: 0x17000706 RID: 1798
		// (get) Token: 0x060015D8 RID: 5592 RVA: 0x0004F54A File Offset: 0x0004D74A
		public static string LabelZoomAPI
		{
			get
			{
				return AsgardUtils.GetValue<string>(ALSetupSlot.Values, 21);
			}
		}

		// Token: 0x17000707 RID: 1799
		// (get) Token: 0x060015D9 RID: 5593 RVA: 0x0004F558 File Offset: 0x0004D758
		[ScriptMemberIgnore]
		public static string LabelZoomAPIKey
		{
			get
			{
				return HiddenUtils.Decrypt(AsgardUtils.GetValue<string>(ALSetupSlot.Values, 22));
			}
		}

		// Token: 0x17000708 RID: 1800
		// (get) Token: 0x060015DA RID: 5594 RVA: 0x0004F56B File Offset: 0x0004D76B
		public static bool CopiesOverrideSO
		{
			get
			{
				return AsgardUtils.HasFlag<ALNbCopies.Options>(ALSetupSlot.EnableCopiesValue, ALNbCopies.Options.SalesOrderLine) && LicenseHelper.LicenseManager.HasFeature(typeof(ALNbCopiesSOLineExt));
			}
		}

		// Token: 0x17000709 RID: 1801
		// (get) Token: 0x060015DB RID: 5595 RVA: 0x0004F591 File Offset: 0x0004D791
		public static bool CopiesOverrideSH
		{
			get
			{
				return AsgardUtils.HasFlag<ALNbCopies.Options>(ALSetupSlot.EnableCopiesValue, ALNbCopies.Options.ShipmentLine) && LicenseHelper.LicenseManager.HasFeature(typeof(ALNbCopiesSOShipLineExt));
			}
		}

		// Token: 0x1700070A RID: 1802
		// (get) Token: 0x060015DC RID: 5596 RVA: 0x0004F5B7 File Offset: 0x0004D7B7
		public static bool CopiesOverridePO
		{
			get
			{
				return AsgardUtils.HasFlag<ALNbCopies.Options>(ALSetupSlot.EnableCopiesValue, ALNbCopies.Options.PurchaseOrderLine) && LicenseHelper.LicenseManager.HasFeature(typeof(ALNbCopiesPOLineExt));
			}
		}

		// Token: 0x1700070B RID: 1803
		// (get) Token: 0x060015DD RID: 5597 RVA: 0x0004F5DD File Offset: 0x0004D7DD
		public static bool CopiesOverridePR
		{
			get
			{
				return AsgardUtils.HasFlag<ALNbCopies.Options>(ALSetupSlot.EnableCopiesValue, ALNbCopies.Options.PurchaseReceiptLine) && LicenseHelper.LicenseManager.HasFeature(typeof(ALNbCopiesPOReceiptLineExt));
			}
		}

		// Token: 0x1700070C RID: 1804
		// (get) Token: 0x060015DE RID: 5598 RVA: 0x0004F604 File Offset: 0x0004D804
		public static bool CopiesOverrideKA
		{
			get
			{
				return AsgardUtils.HasFlag<ALNbCopies.Options>(ALSetupSlot.EnableCopiesValue, ALNbCopies.Options.KitAssemblyLine) && LicenseHelper.LicenseManager.HasFeature(typeof(ALNbCopiesINKitTranSplitExt));
			}
		}

		// Token: 0x1700070D RID: 1805
		// (get) Token: 0x060015DF RID: 5599 RVA: 0x0004F62B File Offset: 0x0004D82B
		public static bool CopiesOverrideSP
		{
			get
			{
				return AsgardUtils.HasFlag<ALNbCopies.Options>(ALSetupSlot.EnableCopiesValue, ALNbCopies.Options.ShipmentPackage) && LicenseHelper.LicenseManager.HasFeature(typeof(ALNbCopiesSOPackageDetailExt));
			}
		}

		// Token: 0x1700070E RID: 1806
		// (get) Token: 0x060015E0 RID: 5600 RVA: 0x0004F651 File Offset: 0x0004D851
		public static bool CopiesOverrideLO
		{
			get
			{
				return AsgardUtils.HasFlag<ALNbCopies.Options>(ALSetupSlot.EnableCopiesValue, ALNbCopies.Options.WarehouseLocation) && LicenseHelper.LicenseManager.HasFeature(typeof(ALNbCopiesINLocationExt));
			}
		}

		// Token: 0x1700070F RID: 1807
		// (get) Token: 0x060015E1 RID: 5601 RVA: 0x0004F678 File Offset: 0x0004D878
		public static bool CopiesOverrideAM
		{
			get
			{
				return AsgardUtils.HasFlag<ALNbCopies.Options>(ALSetupSlot.EnableCopiesValue, ALNbCopies.Options.ProductionOrder) && LicenseHelper.LicenseManager.HasFeature(typeof(ALNbCopiesAMProdItemSplitExt));
			}
		}

		// Token: 0x17000710 RID: 1808
		// (get) Token: 0x060015E2 RID: 5602 RVA: 0x0004F6A2 File Offset: 0x0004D8A2
		public static bool CopiesOverrideIA
		{
			get
			{
				return AsgardUtils.HasFlag<ALNbCopies.Options>(ALSetupSlot.EnableCopiesValue, ALNbCopies.Options.InventoryAdjustment) && LicenseHelper.LicenseManager.HasFeature(typeof(ALNbCopiesINAdjustmentEntryExt));
			}
		}

		// Token: 0x17000711 RID: 1809
		// (get) Token: 0x060015E3 RID: 5603 RVA: 0x0004F6CC File Offset: 0x0004D8CC
		public static bool CopiesOverrideII
		{
			get
			{
				return AsgardUtils.HasFlag<ALNbCopies.Options>(ALSetupSlot.EnableCopiesValue, ALNbCopies.Options.InventoryIssue) && LicenseHelper.LicenseManager.HasFeature(typeof(ALNbCopiesINIssueEntryExt));
			}
		}

		// Token: 0x17000712 RID: 1810
		// (get) Token: 0x060015E4 RID: 5604 RVA: 0x0004F6F6 File Offset: 0x0004D8F6
		public static bool CopiesOverrideIR
		{
			get
			{
				return AsgardUtils.HasFlag<ALNbCopies.Options>(ALSetupSlot.EnableCopiesValue, ALNbCopies.Options.InventoryReceipt) && LicenseHelper.LicenseManager.HasFeature(typeof(ALNbCopiesINReceiptEntryExt));
			}
		}

		// Token: 0x17000713 RID: 1811
		// (get) Token: 0x060015E5 RID: 5605 RVA: 0x0004F720 File Offset: 0x0004D920
		public static bool CopiesOverrideIT
		{
			get
			{
				return AsgardUtils.HasFlag<ALNbCopies.Options>(ALSetupSlot.EnableCopiesValue, ALNbCopies.Options.InventoryTransfer) && LicenseHelper.LicenseManager.HasFeature(typeof(ALNbCopiesINTransferEntryExt));
			}
		}

		// Token: 0x17000714 RID: 1812
		// (get) Token: 0x060015E6 RID: 5606 RVA: 0x0004F74A File Offset: 0x0004D94A
		public static bool CopiesOverridePIReview
		{
			get
			{
				return AsgardUtils.HasFlag<ALNbCopies.Options>(ALSetupSlot.EnableCopiesValue, ALNbCopies.Options.PhysicalInventoryReview) && LicenseHelper.LicenseManager.HasFeature(typeof(ALNbCopiesINPIReviewExt));
			}
		}

		// Token: 0x17000715 RID: 1813
		// (get) Token: 0x060015E7 RID: 5607 RVA: 0x0004F774 File Offset: 0x0004D974
		public static bool CopiesOverridePICount
		{
			get
			{
				return AsgardUtils.HasFlag<ALNbCopies.Options>(ALSetupSlot.EnableCopiesValue, ALNbCopies.Options.PhysicalInventoryCount) && LicenseHelper.LicenseManager.HasFeature(typeof(ALNbCopiesINPICountEntryExt));
			}
		}

		// Token: 0x17000716 RID: 1814
		// (get) Token: 0x060015E8 RID: 5608 RVA: 0x0004F79E File Offset: 0x0004D99E
		public static bool PrinterOverrideSI
		{
			get
			{
				return AsgardUtils.HasFlag<ALPrinterOverride.Options>(ALSetupSlot.PrinterOverrideValue, ALPrinterOverride.Options.StockItem) && LicenseHelper.LicenseManager.HasFeature(typeof(ALPrinterOverrideInventoryItemExt));
			}
		}

		// Token: 0x17000717 RID: 1815
		// (get) Token: 0x060015E9 RID: 5609 RVA: 0x0004F7C4 File Offset: 0x0004D9C4
		public static bool PrinterOverrideNS
		{
			get
			{
				return AsgardUtils.HasFlag<ALPrinterOverride.Options>(ALSetupSlot.PrinterOverrideValue, ALPrinterOverride.Options.NonStockItem) && LicenseHelper.LicenseManager.HasFeature(typeof(ALPrinterOverrideInventoryItemExt));
			}
		}

		// Token: 0x17000718 RID: 1816
		// (get) Token: 0x060015EA RID: 5610 RVA: 0x0004F7EA File Offset: 0x0004D9EA
		public static bool PrinterOverrideTI
		{
			get
			{
				return AsgardUtils.HasFlag<ALPrinterOverride.Options>(ALSetupSlot.PrinterOverrideValue, ALPrinterOverride.Options.TemplateItem) && LicenseHelper.LicenseManager.HasFeature(typeof(ALPrinterOverrideInventoryItemExt));
			}
		}

		// Token: 0x17000719 RID: 1817
		// (get) Token: 0x060015EB RID: 5611 RVA: 0x0004F810 File Offset: 0x0004DA10
		public static bool PrinterOverridePR
		{
			get
			{
				return AsgardUtils.HasFlag<ALPrinterOverride.Options>(ALSetupSlot.PrinterOverrideValue, ALPrinterOverride.Options.PurchaseReceipt) && LicenseHelper.LicenseManager.HasFeature(typeof(ALPrinterOverridePOReceiptExt));
			}
		}

		// Token: 0x1700071A RID: 1818
		// (get) Token: 0x060015EC RID: 5612 RVA: 0x0004F836 File Offset: 0x0004DA36
		public static bool PrinterOverrideIA
		{
			get
			{
				return AsgardUtils.HasFlag<ALPrinterOverride.Options>(ALSetupSlot.PrinterOverrideValue, ALPrinterOverride.Options.InventoryAdjustment) && LicenseHelper.LicenseManager.HasFeature(typeof(ALPrinterOverrideINRegisterExt));
			}
		}

		// Token: 0x1700071B RID: 1819
		// (get) Token: 0x060015ED RID: 5613 RVA: 0x0004F860 File Offset: 0x0004DA60
		public static bool PrinterOverrideII
		{
			get
			{
				return AsgardUtils.HasFlag<ALPrinterOverride.Options>(ALSetupSlot.PrinterOverrideValue, ALPrinterOverride.Options.InventoryIssue) && LicenseHelper.LicenseManager.HasFeature(typeof(ALPrinterOverrideINRegisterExt));
			}
		}

		// Token: 0x1700071C RID: 1820
		// (get) Token: 0x060015EE RID: 5614 RVA: 0x0004F887 File Offset: 0x0004DA87
		public static bool PrinterOverrideIR
		{
			get
			{
				return AsgardUtils.HasFlag<ALPrinterOverride.Options>(ALSetupSlot.PrinterOverrideValue, ALPrinterOverride.Options.InventoryReceipt) && LicenseHelper.LicenseManager.HasFeature(typeof(ALPrinterOverrideINRegisterExt));
			}
		}

		// Token: 0x1700071D RID: 1821
		// (get) Token: 0x060015EF RID: 5615 RVA: 0x0004F8AE File Offset: 0x0004DAAE
		public static bool PrinterOverrideIT
		{
			get
			{
				return AsgardUtils.HasFlag<ALPrinterOverride.Options>(ALSetupSlot.PrinterOverrideValue, ALPrinterOverride.Options.InventoryTransfer) && LicenseHelper.LicenseManager.HasFeature(typeof(ALPrinterOverrideINRegisterExt));
			}
		}

		// Token: 0x1700071E RID: 1822
		// (get) Token: 0x060015F0 RID: 5616 RVA: 0x0004F8D5 File Offset: 0x0004DAD5
		public static bool PrinterOverrideAM
		{
			get
			{
				return AsgardUtils.HasFlag<ALPrinterOverride.Options>(ALSetupSlot.PrinterOverrideValue, ALPrinterOverride.Options.ProductionOrder) && LicenseHelper.LicenseManager.HasFeature(typeof(ALPrinterOverrideAMProdItemExt));
			}
		}

		// Token: 0x1700071F RID: 1823
		// (get) Token: 0x060015F1 RID: 5617 RVA: 0x0004F8FF File Offset: 0x0004DAFF
		public static bool PrinterOverridePIReview
		{
			get
			{
				return AsgardUtils.HasFlag<ALPrinterOverride.Options>(ALSetupSlot.PrinterOverrideValue, ALPrinterOverride.Options.PhysicalInventoryReview) && LicenseHelper.LicenseManager.HasFeature(typeof(ALPrinterOverrideINPIHeaderExt));
			}
		}

		// Token: 0x17000720 RID: 1824
		// (get) Token: 0x060015F2 RID: 5618 RVA: 0x0004F929 File Offset: 0x0004DB29
		public static bool PrinterOverridePICount
		{
			get
			{
				return AsgardUtils.HasFlag<ALPrinterOverride.Options>(ALSetupSlot.PrinterOverrideValue, ALPrinterOverride.Options.PhysicalInventoryCount) && LicenseHelper.LicenseManager.HasFeature(typeof(ALPrinterOverrideINPIHeaderExt));
			}
		}

		// Token: 0x17000721 RID: 1825
		// (get) Token: 0x060015F3 RID: 5619 RVA: 0x0004F953 File Offset: 0x0004DB53
		public static bool DevMode
		{
			get
			{
				return AsgardUtils.HasFlag<ALDev.Options>(ALSetupSlot.DevModeValue, ALDev.Options.DevMode);
			}
		}

		// Token: 0x17000722 RID: 1826
		// (get) Token: 0x060015F4 RID: 5620 RVA: 0x0004F960 File Offset: 0x0004DB60
		public static bool SaveRendered
		{
			get
			{
				return AsgardUtils.HasFlag<ALDev.Options>(ALSetupSlot.DevModeValue, ALDev.Options.SaveRendered);
			}
		}

		// Token: 0x17000723 RID: 1827
		// (get) Token: 0x060015F5 RID: 5621 RVA: 0x0004F96D File Offset: 0x0004DB6D
		public static bool AddLineNumber
		{
			get
			{
				return ALSetupSlot.SaveRendered && AsgardUtils.HasFlag<ALDev.Options>(ALSetupSlot.DevModeValue, ALDev.Options.AddLineNumber);
			}
		}

		// Token: 0x17000724 RID: 1828
		// (get) Token: 0x060015F6 RID: 5622 RVA: 0x0004F984 File Offset: 0x0004DB84
		public static bool AddComments
		{
			get
			{
				return ALSetupSlot.SaveRendered && AsgardUtils.HasFlag<ALDev.Options>(ALSetupSlot.DevModeValue, ALDev.Options.AddComments);
			}
		}

		// Token: 0x17000725 RID: 1829
		// (get) Token: 0x060015F7 RID: 5623 RVA: 0x0004F99B File Offset: 0x0004DB9B
		public static bool AutoPrint
		{
			get
			{
				return AsgardUtils.HasFlag<ALDev.Options>(ALSetupSlot.DevModeValue, ALDev.Options.AutoPrint);
			}
		}

		// Token: 0x17000726 RID: 1830
		// (get) Token: 0x060015F8 RID: 5624 RVA: 0x0004F9A9 File Offset: 0x0004DBA9
		public static bool LoadExternalLibs
		{
			get
			{
				return AsgardUtils.HasFlag<ALDev.Options>(ALSetupSlot.DevModeValue, ALDev.Options.LoadExternalLibs);
			}
		}

		// Token: 0x060015F9 RID: 5625 RVA: 0x0004F9B8 File Offset: 0x0004DBB8
		public void Prefetch()
		{
			PXDataRecord pxdataRecord = PXDatabase.SelectSingle<ALSetup>(Yaql.join<ALPrinter>(Yaql.eq<ALPrinter.printerID, ALSetup.renderingPrinterID>(ALSetupSlot.ALIAS_PR, ALSetupSlot.ALIAS_SE), 1), new PXDataField[]
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
				new PXDataField<ALSetup.renderingPrinterID>(),
				new PXDataField<ALSetup.boxPrint>(),
				new PXDataField<ALSetup.boxPrintModelID>(),
				new PXDataField<ALSetup.ownShipment>(),
				new PXDataField<ALSetup.printOnConfirm>(),
				new PXDataField<ALSetup.printOnConfirmModelID>(),
				new PXDataField<ALPrinter.name>(),
				new PXDataField<ALPrinter.printerType>(),
				new PXDataField<ALSetup.labelZoomAPI>(),
				new PXDataField<ALSetup.labelZoomAPIKey>()
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
				this.values.Add(pxdataRecord.GetGuid(num++));
				this.values.Add(pxdataRecord.GetBoolean(num++).GetValueOrDefault());
				this.values.Add(pxdataRecord.GetGuid(num++));
				this.values.Add(pxdataRecord.GetBoolean(num++).GetValueOrDefault());
				this.values.Add(pxdataRecord.GetBoolean(num++).GetValueOrDefault());
				this.values.Add(pxdataRecord.GetGuid(num++));
				this.values.Add(pxdataRecord.GetString(num++));
				this.values.Add(pxdataRecord.GetString(num++));
				this.values.Add(pxdataRecord.GetString(num++));
				this.values.Add(pxdataRecord.GetString(num++));
			}
		}

		// Token: 0x04000916 RID: 2326
		private static readonly string ALIAS_SE = typeof(ALSetup).Name;

		// Token: 0x04000917 RID: 2327
		private static readonly string ALIAS_PR = typeof(ALPrinter).Name;

		// Token: 0x04000918 RID: 2328
		private readonly IList<object> values = new List<object>();
	}
}
