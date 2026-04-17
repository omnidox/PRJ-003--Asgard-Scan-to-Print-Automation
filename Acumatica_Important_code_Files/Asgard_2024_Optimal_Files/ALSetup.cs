using System;
using AA.Objects.AL.Integration.AutoPrint;
using AA.Objects.AL.Integration.BoxPrint;
using AA.Objects.AL.Integration.FixPackage;
using AA.Objects.AL.Integration.NbCopies;
using AA.Objects.AL.Integration.OwnShipment;
using AA.Objects.AL.Integration.PrinterOverride;
using PX.Data;
using PX.Data.BQL;
using PX.Data.ReferentialIntegrity.Attributes;

namespace AA.Objects.AL
{
	// Token: 0x020001B5 RID: 437
	[PXPrimaryGraph(typeof(ALSetupMaint))]
	[PXCacheName("Label Basic Preferences")]
	[Serializable]
	public class ALSetup : PXBqlTable, IBqlTable, IBqlTableSystemDataStorage, INotable
	{
		// Token: 0x170005AA RID: 1450
		// (get) Token: 0x06001024 RID: 4132 RVA: 0x00035931 File Offset: 0x00033B31
		// (set) Token: 0x06001025 RID: 4133 RVA: 0x00035939 File Offset: 0x00033B39
		[PXDBString(100, IsUnicode = true)]
		[PXUIField(DisplayName = "Labelary API")]
		[PXDefault("https://stable.labelary.com/v1")]
		public virtual string LabelaryAPI { get; set; }

		// Token: 0x170005AB RID: 1451
		// (get) Token: 0x06001026 RID: 4134 RVA: 0x00035942 File Offset: 0x00033B42
		// (set) Token: 0x06001027 RID: 4135 RVA: 0x0003594A File Offset: 0x00033B4A
		[PXRSACryptString(IsUnicode = true, InputMask = "")]
		[PXUIField(DisplayName = "Labelary API Key")]
		[PXDefault("73tmw7yDSRBrSAaE5UjG")]
		public virtual string LabelaryAPIKey { get; set; }

		// Token: 0x170005AC RID: 1452
		// (get) Token: 0x06001028 RID: 4136 RVA: 0x00035953 File Offset: 0x00033B53
		// (set) Token: 0x06001029 RID: 4137 RVA: 0x0003595B File Offset: 0x00033B5B
		[PXDBString(100, IsUnicode = true)]
		[PXUIField(DisplayName = "LabelZoom API")]
		[PXDefault("https://labelzoom.net/api/v2", PersistingCheck = 2)]
		public virtual string LabelZoomAPI { get; set; }

		// Token: 0x170005AD RID: 1453
		// (get) Token: 0x0600102A RID: 4138 RVA: 0x00035964 File Offset: 0x00033B64
		// (set) Token: 0x0600102B RID: 4139 RVA: 0x0003596C File Offset: 0x00033B6C
		[PXRSACryptString(IsUnicode = true, InputMask = "")]
		[PXUIField(DisplayName = "LabelZoom API Key")]
		[PXDefault("eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiJ9.eyJ2ZXIiOm51bGwsInByb2QiOjMsInVzciI6IkFQSSBVU0VSIiwidmFyIjpudWxsLCJsaWMiOiI3MGRlNDBjYyIsInR5cCI6IkQiLCJzZWNyZXQiOiIyODk0Njk4NTUxMDRmM2MwMTNkMiIsImV4cCI6MTc0NTk1NDM4OH0.C4V48OSmJW_4duFRd58NxrygDicujazAadqgXEx1LEB2lhVzcToAt7JsSGGqnJcR3rRNCQ9IbhqR9Spt0ue_1-iLk9NTiN9RaSTZ3phK3-caP4Z5RDXBiw7-9szlcDauPjhUcTrLHIJ7fRDEcRpm_sYAT7QhYj1E0I8i0xmMNE4", PersistingCheck = 2)]
		public virtual string LabelZoomAPIKey { get; set; }

		// Token: 0x170005AE RID: 1454
		// (get) Token: 0x0600102C RID: 4140 RVA: 0x00035975 File Offset: 0x00033B75
		// (set) Token: 0x0600102D RID: 4141 RVA: 0x0003597D File Offset: 0x00033B7D
		[ALPrinterIDForeign(typeof(Where<ALPrinter.printerType, Equal<LabelaryDestination.code>>), DisplayName = "Rendering Printer")]
		[PXForeignReference(typeof(ALSetup.FK.Printer))]
		public virtual Guid? RenderingPrinterID { get; set; }

		// Token: 0x170005AF RID: 1455
		// (get) Token: 0x0600102E RID: 4142 RVA: 0x00035986 File Offset: 0x00033B86
		// (set) Token: 0x0600102F RID: 4143 RVA: 0x0003598E File Offset: 0x00033B8E
		[PXDBString(200, IsUnicode = true)]
		[ALTypeSelector(typeof(IZplGraphicCreator), DirtyRead = true)]
		[PXUIField(DisplayName = "Zpl Graphic Creator")]
		public virtual string ZplGraphicCreator { get; set; }

		// Token: 0x170005B0 RID: 1456
		// (get) Token: 0x06001030 RID: 4144 RVA: 0x00035997 File Offset: 0x00033B97
		// (set) Token: 0x06001031 RID: 4145 RVA: 0x0003599F File Offset: 0x00033B9F
		[ALLanguage(DisplayName = "Default Language")]
		public virtual string DefaultLanguage { get; set; }

		// Token: 0x170005B1 RID: 1457
		// (get) Token: 0x06001032 RID: 4146 RVA: 0x000359A8 File Offset: 0x00033BA8
		// (set) Token: 0x06001033 RID: 4147 RVA: 0x000359B0 File Offset: 0x00033BB0
		[ALFormatIDForeign(typeof(Where<True, Equal<True>>), DisplayName = "Default Format")]
		[PXForeignReference(typeof(ALSetup.FK.Format))]
		public virtual Guid? DefaultFormatID { get; set; }

		// Token: 0x170005B2 RID: 1458
		// (get) Token: 0x06001034 RID: 4148 RVA: 0x000359B9 File Offset: 0x00033BB9
		// (set) Token: 0x06001035 RID: 4149 RVA: 0x000359C1 File Offset: 0x00033BC1
		[ALMarginIDForeign(DisplayName = "Default Margin")]
		[PXForeignReference(typeof(ALSetup.FK.Margin))]
		public virtual Guid? DefaultMarginID { get; set; }

		// Token: 0x170005B3 RID: 1459
		// (get) Token: 0x06001036 RID: 4150 RVA: 0x000359CA File Offset: 0x00033BCA
		// (set) Token: 0x06001037 RID: 4151 RVA: 0x000359D2 File Offset: 0x00033BD2
		[ALCategoryIDForeign(DisplayName = "Default Category")]
		[PXForeignReference(typeof(ALSetup.FK.Category))]
		public virtual Guid? DefaultCategoryID { get; set; }

		// Token: 0x170005B4 RID: 1460
		// (get) Token: 0x06001038 RID: 4152 RVA: 0x000359DB File Offset: 0x00033BDB
		// (set) Token: 0x06001039 RID: 4153 RVA: 0x000359E3 File Offset: 0x00033BE3
		[ALMultiOptions(DisplayName = "Manual Label Qty/Copies For")]
		[ALNbCopiesOptionsMultiDropDown]
		public virtual string EnableCopiesOverride { get; set; }

		// Token: 0x170005B5 RID: 1461
		// (get) Token: 0x0600103A RID: 4154 RVA: 0x000359EC File Offset: 0x00033BEC
		// (set) Token: 0x0600103B RID: 4155 RVA: 0x000359F4 File Offset: 0x00033BF4
		[ALMultiOptions(DisplayName = "Enable Printer Override For")]
		[ALPrinterOverrideOptionsMultiDropDown]
		public virtual string EnablePrinterOverride { get; set; }

		// Token: 0x170005B6 RID: 1462
		// (get) Token: 0x0600103C RID: 4156 RVA: 0x000359FD File Offset: 0x00033BFD
		// (set) Token: 0x0600103D RID: 4157 RVA: 0x00035A05 File Offset: 0x00033C05
		[PXDBShort]
		[PXDefault(30)]
		[PXUIField(DisplayName = "Nb of Days to keep old label files")]
		public virtual short? NbDaysToKeep { get; set; }

		// Token: 0x170005B7 RID: 1463
		// (get) Token: 0x0600103E RID: 4158 RVA: 0x00035A0E File Offset: 0x00033C0E
		// (set) Token: 0x0600103F RID: 4159 RVA: 0x00035A16 File Offset: 0x00033C16
		[ALMultiOptions(DisplayName = "Options (Might slow down server)")]
		[ALDevOptionsMultiDropDown]
		[PXUIEnabled(typeof(ALHasFeatureByField<ALSetup.devMode>))]
		public virtual string DevMode { get; set; }

		// Token: 0x170005B8 RID: 1464
		// (get) Token: 0x06001040 RID: 4160 RVA: 0x00035A1F File Offset: 0x00033C1F
		// (set) Token: 0x06001041 RID: 4161 RVA: 0x00035A27 File Offset: 0x00033C27
		[ALRecordImportMode]
		[PXDefault(3, PersistingCheck = 2)]
		public virtual int? RecordImportMode { get; set; }

		// Token: 0x170005B9 RID: 1465
		// (get) Token: 0x06001042 RID: 4162 RVA: 0x00035A30 File Offset: 0x00033C30
		// (set) Token: 0x06001043 RID: 4163 RVA: 0x00035A38 File Offset: 0x00033C38
		[PXDBString(100, IsUnicode = true)]
		[PXUIField(DisplayName = "Asgard Cloud Print API")]
		[PXDefault("https://api.printnode.com", PersistingCheck = 2)]
		public virtual string PrintNodeAPI { get; set; }

		// Token: 0x170005BA RID: 1466
		// (get) Token: 0x06001044 RID: 4164 RVA: 0x00035A41 File Offset: 0x00033C41
		// (set) Token: 0x06001045 RID: 4165 RVA: 0x00035A49 File Offset: 0x00033C49
		[PXRSACryptString(IsUnicode = true, InputMask = "")]
		[PXUIField(DisplayName = "Asgard Cloud Print API Key")]
		[PXDefault("", PersistingCheck = 2)]
		public virtual string PrintNodeAPIKey { get; set; }

		// Token: 0x170005BB RID: 1467
		// (get) Token: 0x06001046 RID: 4166 RVA: 0x00035A52 File Offset: 0x00033C52
		// (set) Token: 0x06001047 RID: 4167 RVA: 0x00035A5A File Offset: 0x00033C5A
		[PXDBBool]
		[PXDefault(false, PersistingCheck = 2)]
		[PXUIField(DisplayName = "Fix Package LineNbr")]
		[PXUIEnabled(typeof(Where<ALHasFeature<ALSOPackageDetailExt.feature>>))]
		public bool? FixPackageLineNbr { get; set; }

		// Token: 0x170005BC RID: 1468
		// (get) Token: 0x06001048 RID: 4168 RVA: 0x00035A63 File Offset: 0x00033C63
		// (set) Token: 0x06001049 RID: 4169 RVA: 0x00035A6B File Offset: 0x00033C6B
		[PXDBBool]
		[PXDefault(false, PersistingCheck = 2)]
		[PXUIField(DisplayName = "Print on Box Confirmed")]
		[PXUIEnabled(typeof(Where<ALHasFeature<ALBoxPrintSOShipmentEntryExt.feature>>))]
		public bool? BoxPrint { get; set; }

		// Token: 0x170005BD RID: 1469
		// (get) Token: 0x0600104A RID: 4170 RVA: 0x00035A74 File Offset: 0x00033C74
		// (set) Token: 0x0600104B RID: 4171 RVA: 0x00035A7C File Offset: 0x00033C7C
		[ALModelIDForeign(typeof(Where<ALModel.screenID, Equal<ALConstants.ScreenIDs.Shipments>, And<ALModel.basedOnView, Contains<ALConstants.ViewNames.Packages>, And<ALModel.modelType, In3<ALModelType.group, ALModelType.single>>>>), DisplayName = "Box Confirmed Model")]
		[PXDefault(PersistingCheck = 2)]
		[PXUIVisible(typeof(Where<ALSetup.boxPrint, Equal<True>>))]
		[PXUIEnabled(typeof(Where<ALSetup.boxPrint, Equal<True>, And<ALHasFeature<ALBoxPrintSOShipmentEntryExt.feature>>>))]
		[PXUIRequired(typeof(Where<ALSetup.boxPrint, Equal<True>>))]
		[PXForeignReference(typeof(ALSetup.FK.BoxPrintModel))]
		public Guid? BoxPrintModelID { get; set; }

		// Token: 0x170005BE RID: 1470
		// (get) Token: 0x0600104C RID: 4172 RVA: 0x00035A85 File Offset: 0x00033C85
		// (set) Token: 0x0600104D RID: 4173 RVA: 0x00035A8D File Offset: 0x00033C8D
		[PXDBBool]
		[PXDefault(false, PersistingCheck = 2)]
		[PXUIField(DisplayName = "Own Shipment on Confirm")]
		[PXUIEnabled(typeof(Where<ALHasFeature<ALOwnShipmentSOShipmentEntryExt.feature>>))]
		public bool? OwnShipment { get; set; }

		// Token: 0x170005BF RID: 1471
		// (get) Token: 0x0600104E RID: 4174 RVA: 0x00035A96 File Offset: 0x00033C96
		// (set) Token: 0x0600104F RID: 4175 RVA: 0x00035A9E File Offset: 0x00033C9E
		[PXDBBool]
		[PXDefault(false, PersistingCheck = 2)]
		[PXUIField(DisplayName = "Print on Shipment Confirmed")]
		[PXUIEnabled(typeof(Where<ALHasFeature<ALPrintOnConfirmSOShipmentEntryExt.feature>>))]
		public bool? PrintOnConfirm { get; set; }

		// Token: 0x170005C0 RID: 1472
		// (get) Token: 0x06001050 RID: 4176 RVA: 0x00035AA7 File Offset: 0x00033CA7
		// (set) Token: 0x06001051 RID: 4177 RVA: 0x00035AAF File Offset: 0x00033CAF
		[ALModelIDForeign(typeof(Where<ALModel.screenID, Equal<ALConstants.ScreenIDs.Shipments>, And<ALModel.modelType, In3<ALModelType.group, ALModelType.single>>>), DisplayName = "Shipment Confirmed Model")]
		[PXDefault(PersistingCheck = 2)]
		[PXUIVisible(typeof(Where<ALSetup.printOnConfirm, Equal<True>>))]
		[PXUIEnabled(typeof(Where<ALSetup.printOnConfirm, Equal<True>, And<ALHasFeature<ALPrintOnConfirmSOShipmentEntryExt.feature>>>))]
		[PXUIRequired(typeof(Where<ALSetup.printOnConfirm, Equal<True>>))]
		[PXForeignReference(typeof(ALSetup.FK.PrintOnConfirmModel))]
		public Guid? PrintOnConfirmModelID { get; set; }

		// Token: 0x170005C1 RID: 1473
		// (get) Token: 0x06001052 RID: 4178 RVA: 0x00035AB8 File Offset: 0x00033CB8
		// (set) Token: 0x06001053 RID: 4179 RVA: 0x00035AC0 File Offset: 0x00033CC0
		[PXNote]
		public virtual Guid? NoteID { get; set; }

		// Token: 0x170005C2 RID: 1474
		// (get) Token: 0x06001054 RID: 4180 RVA: 0x00035AC9 File Offset: 0x00033CC9
		// (set) Token: 0x06001055 RID: 4181 RVA: 0x00035AD1 File Offset: 0x00033CD1
		[PXDBCreatedByID]
		public virtual Guid? CreatedByID { get; set; }

		// Token: 0x170005C3 RID: 1475
		// (get) Token: 0x06001056 RID: 4182 RVA: 0x00035ADA File Offset: 0x00033CDA
		// (set) Token: 0x06001057 RID: 4183 RVA: 0x00035AE2 File Offset: 0x00033CE2
		[PXDBCreatedByScreenID]
		public virtual string CreatedByScreenID { get; set; }

		// Token: 0x170005C4 RID: 1476
		// (get) Token: 0x06001058 RID: 4184 RVA: 0x00035AEB File Offset: 0x00033CEB
		// (set) Token: 0x06001059 RID: 4185 RVA: 0x00035AF3 File Offset: 0x00033CF3
		[PXDBCreatedDateTime]
		public virtual DateTime? CreatedDateTime { get; set; }

		// Token: 0x170005C5 RID: 1477
		// (get) Token: 0x0600105A RID: 4186 RVA: 0x00035AFC File Offset: 0x00033CFC
		// (set) Token: 0x0600105B RID: 4187 RVA: 0x00035B04 File Offset: 0x00033D04
		[PXDBLastModifiedByID]
		public virtual Guid? LastModifiedByID { get; set; }

		// Token: 0x170005C6 RID: 1478
		// (get) Token: 0x0600105C RID: 4188 RVA: 0x00035B0D File Offset: 0x00033D0D
		// (set) Token: 0x0600105D RID: 4189 RVA: 0x00035B15 File Offset: 0x00033D15
		[PXDBLastModifiedByScreenID]
		public virtual string LastModifiedByScreenID { get; set; }

		// Token: 0x170005C7 RID: 1479
		// (get) Token: 0x0600105E RID: 4190 RVA: 0x00035B1E File Offset: 0x00033D1E
		// (set) Token: 0x0600105F RID: 4191 RVA: 0x00035B26 File Offset: 0x00033D26
		[PXDBLastModifiedDateTime]
		public virtual DateTime? LastModifiedDateTime { get; set; }

		// Token: 0x170005C8 RID: 1480
		// (get) Token: 0x06001060 RID: 4192 RVA: 0x00035B2F File Offset: 0x00033D2F
		// (set) Token: 0x06001061 RID: 4193 RVA: 0x00035B37 File Offset: 0x00033D37
		[PXDBTimestamp]
		public virtual byte[] tstamp { get; set; }

		// Token: 0x0200083E RID: 2110
		public static class FK
		{
			// Token: 0x02000BBB RID: 3003
			public class Printer : PrimaryKeyOf<ALPrinter>.By<ALPrinter.printerID>.ForeignKeyOf<ALSetup>.By<ALSetup.renderingPrinterID>
			{
			}

			// Token: 0x02000BBC RID: 3004
			public class Margin : PrimaryKeyOf<ALMargin>.By<ALMargin.marginID>.ForeignKeyOf<ALSetup>.By<ALSetup.defaultMarginID>
			{
			}

			// Token: 0x02000BBD RID: 3005
			public class Format : PrimaryKeyOf<ALFormat>.By<ALFormat.formatID>.ForeignKeyOf<ALSetup>.By<ALSetup.defaultFormatID>
			{
			}

			// Token: 0x02000BBE RID: 3006
			public class Category : PrimaryKeyOf<ALCategory>.By<ALCategory.categoryID>.ForeignKeyOf<ALSetup>.By<ALSetup.defaultCategoryID>
			{
			}

			// Token: 0x02000BBF RID: 3007
			public class BoxPrintModel : PrimaryKeyOf<ALModel>.By<ALModel.labelID>.ForeignKeyOf<ALSetup>.By<ALSetup.boxPrintModelID>
			{
			}

			// Token: 0x02000BC0 RID: 3008
			public class PrintOnConfirmModel : PrimaryKeyOf<ALModel>.By<ALModel.labelID>.ForeignKeyOf<ALSetup>.By<ALSetup.printOnConfirmModelID>
			{
			}
		}

		// Token: 0x0200083F RID: 2111
		public abstract class labelaryAPI : BqlType<IBqlString, string>.Field<ALSetup.labelaryAPI>
		{
		}

		// Token: 0x02000840 RID: 2112
		public abstract class labelaryAPIKey : BqlType<IBqlString, string>.Field<ALSetup.labelaryAPIKey>
		{
		}

		// Token: 0x02000841 RID: 2113
		public abstract class labelZoomAPI : BqlType<IBqlString, string>.Field<ALSetup.labelZoomAPI>
		{
		}

		// Token: 0x02000842 RID: 2114
		public abstract class labelZoomAPIKey : BqlType<IBqlString, string>.Field<ALSetup.labelZoomAPIKey>
		{
		}

		// Token: 0x02000843 RID: 2115
		public abstract class renderingPrinterID : BqlType<IBqlGuid, Guid>.Field<ALSetup.renderingPrinterID>
		{
		}

		// Token: 0x02000844 RID: 2116
		public abstract class zplGraphicCreator : BqlType<IBqlString, string>.Field<ALSetup.zplGraphicCreator>
		{
		}

		// Token: 0x02000845 RID: 2117
		public abstract class defaultLanguage : BqlType<IBqlString, string>.Field<ALSetup.defaultLanguage>
		{
		}

		// Token: 0x02000846 RID: 2118
		public abstract class defaultFormatID : BqlType<IBqlGuid, Guid>.Field<ALSetup.defaultFormatID>
		{
		}

		// Token: 0x02000847 RID: 2119
		public abstract class defaultMarginID : BqlType<IBqlGuid, Guid>.Field<ALSetup.defaultMarginID>
		{
		}

		// Token: 0x02000848 RID: 2120
		public abstract class defaultCategoryID : BqlType<IBqlGuid, Guid>.Field<ALSetup.defaultCategoryID>
		{
		}

		// Token: 0x02000849 RID: 2121
		public abstract class enableCopiesOverride : BqlType<IBqlString, string>.Field<ALSetup.enableCopiesOverride>
		{
		}

		// Token: 0x0200084A RID: 2122
		public abstract class enablePrinterOverride : BqlType<IBqlString, string>.Field<ALSetup.enablePrinterOverride>
		{
		}

		// Token: 0x0200084B RID: 2123
		public abstract class nbDaysToKeep : BqlType<IBqlShort, short>.Field<ALSetup.nbDaysToKeep>
		{
		}

		// Token: 0x0200084C RID: 2124
		public abstract class devMode : BqlType<IBqlString, string>.Field<ALSetup.devMode>
		{
		}

		// Token: 0x0200084D RID: 2125
		public abstract class recordImportMode : BqlType<IBqlInt, int>.Field<ALSetup.recordImportMode>
		{
		}

		// Token: 0x0200084E RID: 2126
		public abstract class printNodeAPI : BqlType<IBqlString, string>.Field<ALSetup.printNodeAPI>
		{
		}

		// Token: 0x0200084F RID: 2127
		public abstract class printNodeAPIKey : BqlType<IBqlString, string>.Field<ALSetup.printNodeAPIKey>
		{
		}

		// Token: 0x02000850 RID: 2128
		public abstract class fixPackageLineNbr : BqlType<IBqlBool, bool>.Field<ALSetup.fixPackageLineNbr>
		{
		}

		// Token: 0x02000851 RID: 2129
		public abstract class boxPrint : BqlType<IBqlBool, bool>.Field<ALSetup.boxPrint>
		{
		}

		// Token: 0x02000852 RID: 2130
		public abstract class boxPrintModelID : BqlType<IBqlGuid, Guid>.Field<ALSetup.boxPrintModelID>
		{
		}

		// Token: 0x02000853 RID: 2131
		public abstract class ownShipment : BqlType<IBqlBool, bool>.Field<ALSetup.ownShipment>
		{
		}

		// Token: 0x02000854 RID: 2132
		public abstract class printOnConfirm : BqlType<IBqlBool, bool>.Field<ALSetup.printOnConfirm>
		{
		}

		// Token: 0x02000855 RID: 2133
		public abstract class printOnConfirmModelID : BqlType<IBqlGuid, Guid>.Field<ALSetup.printOnConfirmModelID>
		{
		}

		// Token: 0x02000856 RID: 2134
		public abstract class noteID : BqlType<IBqlGuid, Guid>.Field<ALSetup.noteID>
		{
		}

		// Token: 0x02000857 RID: 2135
		public abstract class createdByID : BqlType<IBqlGuid, Guid>.Field<ALSetup.createdByID>
		{
		}

		// Token: 0x02000858 RID: 2136
		public abstract class createdByScreenID : BqlType<IBqlString, string>.Field<ALSetup.createdByScreenID>
		{
		}

		// Token: 0x02000859 RID: 2137
		public abstract class createdDateTime : BqlType<IBqlDateTime, DateTime>.Field<ALSetup.createdDateTime>
		{
		}

		// Token: 0x0200085A RID: 2138
		public abstract class lastModifiedByID : BqlType<IBqlGuid, Guid>.Field<ALSetup.lastModifiedByID>
		{
		}

		// Token: 0x0200085B RID: 2139
		public abstract class lastModifiedByScreenID : BqlType<IBqlString, string>.Field<ALSetup.lastModifiedByScreenID>
		{
		}

		// Token: 0x0200085C RID: 2140
		public abstract class lastModifiedDateTime : BqlType<IBqlDateTime, DateTime>.Field<ALSetup.lastModifiedDateTime>
		{
		}

		// Token: 0x0200085D RID: 2141
		public abstract class Tstamp : BqlType<IBqlByteArray, byte[]>.Field<ALSetup.Tstamp>
		{
		}
	}
}
