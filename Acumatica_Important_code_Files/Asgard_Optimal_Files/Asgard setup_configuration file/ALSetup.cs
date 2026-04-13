using System;
using System.Runtime.CompilerServices;
using AA.Objects.Core;
using AA.Objects.Labels.Integration;
using AA.Objects.Labels.Integration.AutoPrint;
using AA.Objects.Labels.Integration.BoxPrint;
using AA.Objects.Labels.Integration.FixPackage;
using AA.Objects.Labels.Integration.NbCopies;
using AA.Objects.Labels.Integration.OwnShipment;
using AA.Objects.Labels.Integration.PrinterOverride;
using Asgard.Labels.Abstractions.Interface;
using Asgard.Labels.Impl.Poco;
using PX.Data;
using PX.Data.BQL;
using PX.Data.ReferentialIntegrity.Attributes;
using PX.Objects.CA;
using PX.Objects.Common;
using PX.SM;

namespace AA.Objects.Labels
{
	// Token: 0x0200010E RID: 270
	[PXPrimaryGraph(typeof(ALSetupMaint))]
	[PXCacheName("Label Basic Preferences")]
	[Serializable]
	public class ALSetup : PXBqlTable, IBqlTable, IBqlTableSystemDataStorage, INotable
	{
		// Token: 0x170004DF RID: 1247
		// (get) Token: 0x06000BA0 RID: 2976 RVA: 0x0001F0F0 File Offset: 0x0001D2F0
		// (set) Token: 0x06000BA1 RID: 2977 RVA: 0x0001F0F8 File Offset: 0x0001D2F8
		[PXDBString(100, IsUnicode = true)]
		[PXUIField(DisplayName = "Labelary API")]
		[PXDefault("https://stable.labelary.com/v1")]
		public virtual string LabelaryAPI { get; set; }

		// Token: 0x170004E0 RID: 1248
		// (get) Token: 0x06000BA2 RID: 2978 RVA: 0x0001F101 File Offset: 0x0001D301
		// (set) Token: 0x06000BA3 RID: 2979 RVA: 0x0001F109 File Offset: 0x0001D309
		[PXRSACryptString(IsUnicode = true, InputMask = "")]
		[PXUIField(DisplayName = "Labelary API Key")]
		[PXDefault("73tmw7yDSRBrSAaE5UjG")]
		public virtual string LabelaryAPIKey { get; set; }

		// Token: 0x170004E1 RID: 1249
		// (get) Token: 0x06000BA4 RID: 2980 RVA: 0x0001F112 File Offset: 0x0001D312
		// (set) Token: 0x06000BA5 RID: 2981 RVA: 0x0001F11A File Offset: 0x0001D31A
		[PXDBString(100, IsUnicode = true)]
		[PXUIField(DisplayName = "LabelZoom API")]
		[PXDefault("https://labelzoom.net/api/v2", PersistingCheck = 2)]
		[PXUIVisible(typeof(ALHasDevOption<ALDev.Operands.LabelZoom>))]
		public virtual string LabelZoomAPI { get; set; }

		// Token: 0x170004E2 RID: 1250
		// (get) Token: 0x06000BA6 RID: 2982 RVA: 0x0001F123 File Offset: 0x0001D323
		// (set) Token: 0x06000BA7 RID: 2983 RVA: 0x0001F12B File Offset: 0x0001D32B
		[PXRSACryptString(IsUnicode = true, InputMask = "")]
		[PXUIField(DisplayName = "LabelZoom API Key")]
		[PXDefault("eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiJ9.eyJ2ZXIiOm51bGwsInByb2QiOjMsInVzciI6IkFQSSBVU0VSIiwidmFyIjpudWxsLCJsaWMiOiI3MGRlNDBjYyIsInR5cCI6IkQiLCJzZWNyZXQiOiIyODk0Njk4NTUxMDRmM2MwMTNkMiIsImV4cCI6MTc0OTAwMTIwN30.OsoKzDAjcctfZ_VtInSY6aB3urlnul_-ZSIAOZ1M_X2_takhBkVNQAzer8oJuQiMXXoOyuT4CpoW39NrP8WTxmNQLzv0zZezyUOnX45XH1Kkdn_P02tODjmCCpMrg6DoWJsBSCvj1lqeHJI4DSdZUeCSeX-N-WnXidSb_0NOyN4", PersistingCheck = 2)]
		[PXUIVisible(typeof(ALHasDevOption<ALDev.Operands.LabelZoom>))]
		public virtual string LabelZoomAPIKey { get; set; }

		// Token: 0x170004E3 RID: 1251
		// (get) Token: 0x06000BA8 RID: 2984 RVA: 0x0001F134 File Offset: 0x0001D334
		// (set) Token: 0x06000BA9 RID: 2985 RVA: 0x0001F13C File Offset: 0x0001D33C
		[ALCategoryIDForeign(DisplayName = "LabelZoom Category")]
		[PXForeignReference(typeof(ALSetup.FK.LabelZoomCategory))]
		[PXUIVisible(typeof(ALHasDevOption<ALDev.Operands.LabelZoom>))]
		public virtual Guid? LabelZoomCategoryID { get; set; }

		// Token: 0x170004E4 RID: 1252
		// (get) Token: 0x06000BAA RID: 2986 RVA: 0x0001F145 File Offset: 0x0001D345
		// (set) Token: 0x06000BAB RID: 2987 RVA: 0x0001F14D File Offset: 0x0001D34D
		[ALSubstitutionIDForeign(typeof(Where<ALSubstitution.nbArgs, Equal<One>, And<ALSubstitution.returnTypeName, Equal<ALConstants.ReturnTypeName.System_String>>>), DisplayName = "LabelZoom Image Convertor")]
		[PXForeignReference(typeof(ALSetup.FK.LabelZoomImageSubstitution))]
		[PXDefault(PersistingCheck = 2)]
		[PXUIVisible(typeof(ALHasDevOption<ALDev.Operands.LabelZoom>))]
		public virtual Guid? LabelZoomImageSubstitutionID { get; set; }

		// Token: 0x170004E5 RID: 1253
		// (get) Token: 0x06000BAC RID: 2988 RVA: 0x0001F156 File Offset: 0x0001D356
		// (set) Token: 0x06000BAD RID: 2989 RVA: 0x0001F15E File Offset: 0x0001D35E
		[PXDBString(100, IsUnicode = true)]
		[PXUIField(DisplayName = "Mongo URL")]
		[PXDefault("mongodb://localhost:27017", PersistingCheck = 2)]
		[PXUIVisible(typeof(ALHasDevOption<ALDev.Operands.MongoDb>))]
		public virtual string MongoURL { get; set; }

		// Token: 0x170004E6 RID: 1254
		// (get) Token: 0x06000BAE RID: 2990 RVA: 0x0001F167 File Offset: 0x0001D367
		// (set) Token: 0x06000BAF RID: 2991 RVA: 0x0001F16F File Offset: 0x0001D36F
		[PXRSACryptString(IsUnicode = true, InputMask = "")]
		[PXUIField(DisplayName = "Mongo Options")]
		[PXDefault(PersistingCheck = 2)]
		[PXUIVisible(typeof(ALHasDevOption<ALDev.Operands.MongoDb>))]
		public virtual string MongoOptions { get; set; }

		// Token: 0x170004E7 RID: 1255
		// (get) Token: 0x06000BB0 RID: 2992 RVA: 0x0001F178 File Offset: 0x0001D378
		// (set) Token: 0x06000BB1 RID: 2993 RVA: 0x0001F180 File Offset: 0x0001D380
		[PXDBString(200, IsUnicode = true)]
		[ALTypeSelector(typeof(IGraphicCreator), DirtyRead = true)]
		[PXDefault(typeof(Constants.Impl.defaultGraphCreator), PersistingCheck = 2)]
		[PXUIField(DisplayName = "Zpl Graphic Creator")]
		public virtual string ZplGraphicCreator { get; set; }

		// Token: 0x170004E8 RID: 1256
		// (get) Token: 0x06000BB2 RID: 2994 RVA: 0x0001F189 File Offset: 0x0001D389
		// (set) Token: 0x06000BB3 RID: 2995 RVA: 0x0001F191 File Offset: 0x0001D391
		[ALLanguage(DisplayName = "Default Language")]
		public virtual string DefaultLanguage { get; set; }

		// Token: 0x170004E9 RID: 1257
		// (get) Token: 0x06000BB4 RID: 2996 RVA: 0x0001F19A File Offset: 0x0001D39A
		// (set) Token: 0x06000BB5 RID: 2997 RVA: 0x0001F1A2 File Offset: 0x0001D3A2
		[ALFormatIDForeign(typeof(Where<True, Equal<True>>), DisplayName = "Default Format")]
		[PXForeignReference(typeof(ALSetup.FK.Format))]
		public virtual Guid? DefaultFormatID { get; set; }

		// Token: 0x170004EA RID: 1258
		// (get) Token: 0x06000BB6 RID: 2998 RVA: 0x0001F1AB File Offset: 0x0001D3AB
		// (set) Token: 0x06000BB7 RID: 2999 RVA: 0x0001F1B3 File Offset: 0x0001D3B3
		[ALMarginIDForeign(DisplayName = "Default Margin")]
		[PXForeignReference(typeof(ALSetup.FK.Margin))]
		public virtual Guid? DefaultMarginID { get; set; }

		// Token: 0x170004EB RID: 1259
		// (get) Token: 0x06000BB8 RID: 3000 RVA: 0x0001F1BC File Offset: 0x0001D3BC
		// (set) Token: 0x06000BB9 RID: 3001 RVA: 0x0001F1C4 File Offset: 0x0001D3C4
		[ALCategoryIDForeign(DisplayName = "Default Category")]
		[PXForeignReference(typeof(ALSetup.FK.Category))]
		public virtual Guid? DefaultCategoryID { get; set; }

		// Token: 0x170004EC RID: 1260
		// (get) Token: 0x06000BBA RID: 3002 RVA: 0x0001F1CD File Offset: 0x0001D3CD
		// (set) Token: 0x06000BBB RID: 3003 RVA: 0x0001F1D5 File Offset: 0x0001D3D5
		[PXDBBool]
		[PXDefault(false, PersistingCheck = 2)]
		[PXUIField(DisplayName = "Generate Shipment 2D barcodes")]
		public bool? GenerateShipment2D { get; set; }

		// Token: 0x170004ED RID: 1261
		// (get) Token: 0x06000BBC RID: 3004 RVA: 0x0001F1DE File Offset: 0x0001D3DE
		// (set) Token: 0x06000BBD RID: 3005 RVA: 0x0001F1E6 File Offset: 0x0001D3E6
		[ALModelIDForeign(typeof(Where<True, Equal<True>>), DisplayName = "Shipment 2D Model")]
		[PXDefault(PersistingCheck = 2)]
		[PXUIVisible(typeof(Where<ALSetup.generateShipment2D, Equal<True>>))]
		[PXUIEnabled(typeof(Where<ALSetup.generateShipment2D, Equal<True>>))]
		[PXUIRequired(typeof(Where<ALSetup.generateShipment2D, Equal<True>>))]
		[PXForeignReference(typeof(ALSetup.FK.Shipment2DModel))]
		public Guid? Shipment2DModelID { get; set; }

		// Token: 0x170004EE RID: 1262
		// (get) Token: 0x06000BBE RID: 3006 RVA: 0x0001F1EF File Offset: 0x0001D3EF
		// (set) Token: 0x06000BBF RID: 3007 RVA: 0x0001F1F7 File Offset: 0x0001D3F7
		[PXDBString(8, IsFixed = true, InputMask = "CC.CC.CC.CC")]
		[PXUIField(DisplayName = "Packing Slip Report ID", Visibility = 7)]
		[PXSelector(typeof(Search<SiteMap.screenID, Where<SiteMap.screenID, Like<PXModule.so_>, And<SiteMap.url, Like<urlReports>>>, OrderBy<Asc<SiteMap.screenID>>>), new Type[]
		{
			typeof(SiteMap.screenID),
			typeof(SiteMap.title)
		}, Headers = new string[]
		{
			"Report ID",
			"Report Name"
		}, DescriptionField = typeof(SiteMap.title))]
		[PXDefault(PersistingCheck = 2)]
		[PXUIVisible(typeof(Where<ALSetup.generateShipment2D, Equal<True>>))]
		[PXUIEnabled(typeof(Where<ALSetup.generateShipment2D, Equal<True>>))]
		public string PackingListReportID { get; set; }

		// Token: 0x170004EF RID: 1263
		// (get) Token: 0x06000BC0 RID: 3008 RVA: 0x0001F200 File Offset: 0x0001D400
		// (set) Token: 0x06000BC1 RID: 3009 RVA: 0x0001F208 File Offset: 0x0001D408
		[ALMultiOptions(DisplayName = "Label Qty/Copies For")]
		[ALNbCopiesOptionsMultiDropDown]
		public virtual string EnableCopiesOverride { get; set; }

		// Token: 0x170004F0 RID: 1264
		// (get) Token: 0x06000BC2 RID: 3010 RVA: 0x0001F211 File Offset: 0x0001D411
		// (set) Token: 0x06000BC3 RID: 3011 RVA: 0x0001F219 File Offset: 0x0001D419
		[ALMultiOptions(DisplayName = "Enable For")]
		[ALPrinterOverrideOptionsMultiDropDown]
		public virtual string EnablePrinterOverride { get; set; }

		// Token: 0x170004F1 RID: 1265
		// (get) Token: 0x06000BC4 RID: 3012 RVA: 0x0001F222 File Offset: 0x0001D422
		// (set) Token: 0x06000BC5 RID: 3013 RVA: 0x0001F22A File Offset: 0x0001D42A
		[PXDBShort]
		[PXDefault(30)]
		[PXUIField(DisplayName = "Nb of Days to keep old label files")]
		public virtual short? NbDaysToKeep { get; set; }

		// Token: 0x170004F2 RID: 1266
		// (get) Token: 0x06000BC6 RID: 3014 RVA: 0x0001F233 File Offset: 0x0001D433
		// (set) Token: 0x06000BC7 RID: 3015 RVA: 0x0001F23B File Offset: 0x0001D43B
		[ALMultiOptions(DisplayName = "Options (Might slow down server)")]
		[ALDevOptionsMultiDropDown]
		[PXUIEnabled(typeof(ALHasFeatureByField<ALSetup.devMode>))]
		public virtual string DevMode { get; set; }

		// Token: 0x170004F3 RID: 1267
		// (get) Token: 0x06000BC8 RID: 3016 RVA: 0x0001F244 File Offset: 0x0001D444
		// (set) Token: 0x06000BC9 RID: 3017 RVA: 0x0001F24C File Offset: 0x0001D44C
		[ALRecordImportMode]
		[PXDefault(3, PersistingCheck = 2)]
		public virtual int? RecordImportMode { get; set; }

		// Token: 0x170004F4 RID: 1268
		// (get) Token: 0x06000BCA RID: 3018 RVA: 0x0001F255 File Offset: 0x0001D455
		// (set) Token: 0x06000BCB RID: 3019 RVA: 0x0001F25D File Offset: 0x0001D45D
		[PXDBString(100, IsUnicode = true)]
		[PXUIField(DisplayName = "Asgard Cloud API")]
		[PXDefault("https://api.printnode.com", PersistingCheck = 2)]
		public virtual string PrintNodeAPI { get; set; }

		// Token: 0x170004F5 RID: 1269
		// (get) Token: 0x06000BCC RID: 3020 RVA: 0x0001F266 File Offset: 0x0001D466
		// (set) Token: 0x06000BCD RID: 3021 RVA: 0x0001F26E File Offset: 0x0001D46E
		[PXRSACryptString(IsUnicode = true, InputMask = "")]
		[PXUIField(DisplayName = "Asgard Cloud API Key")]
		[PXDefault(PersistingCheck = 2)]
		public virtual string PrintNodeAPIKey { get; set; }

		// Token: 0x170004F6 RID: 1270
		// (get) Token: 0x06000BCE RID: 3022 RVA: 0x0001F277 File Offset: 0x0001D477
		// (set) Token: 0x06000BCF RID: 3023 RVA: 0x0001F27F File Offset: 0x0001D47F
		[PXDBBool]
		[PXDefault(false, PersistingCheck = 2)]
		[PXUIField(DisplayName = "Fix Package LineNbr")]
		[PXUIEnabled(typeof(Where<ALHasFeature<ALSOPackageDetailExt.feature>>))]
		public bool? FixPackageLineNbr { get; set; }

		// Token: 0x170004F7 RID: 1271
		// (get) Token: 0x06000BD0 RID: 3024 RVA: 0x0001F288 File Offset: 0x0001D488
		// (set) Token: 0x06000BD1 RID: 3025 RVA: 0x0001F290 File Offset: 0x0001D490
		[PXDBBool]
		[PXDefault(false, PersistingCheck = 2)]
		[PXUIField(DisplayName = "Print on Box Confirmed")]
		[PXUIEnabled(typeof(Where<ALHasFeature<ALBoxPrintSOShipmentEntryExt.feature>>))]
		public bool? BoxPrint { get; set; }

		// Token: 0x170004F8 RID: 1272
		// (get) Token: 0x06000BD2 RID: 3026 RVA: 0x0001F299 File Offset: 0x0001D499
		// (set) Token: 0x06000BD3 RID: 3027 RVA: 0x0001F2A1 File Offset: 0x0001D4A1
		[ALModelIDForeign(typeof(Where<ALModel.screenID, Equal<ACConstants.ScreenIDs.Shipments>, And<ALModel.basedOnView, Contains<ALConstants.ViewNames.Packages>, And<ALModel.modelType, In3<ALModelType.group, ALModelType.single>>>>), DisplayName = "Box Confirmed Model")]
		[PXDefault(PersistingCheck = 2)]
		[PXUIVisible(typeof(Where<ALSetup.boxPrint, Equal<True>>))]
		[PXUIEnabled(typeof(Where<ALSetup.boxPrint, Equal<True>, And<ALHasFeature<ALBoxPrintSOShipmentEntryExt.feature>>>))]
		[PXUIRequired(typeof(Where<ALSetup.boxPrint, Equal<True>>))]
		[PXForeignReference(typeof(ALSetup.FK.BoxPrintModel))]
		public Guid? BoxPrintModelID { get; set; }

		// Token: 0x170004F9 RID: 1273
		// (get) Token: 0x06000BD4 RID: 3028 RVA: 0x0001F2AA File Offset: 0x0001D4AA
		// (set) Token: 0x06000BD5 RID: 3029 RVA: 0x0001F2B2 File Offset: 0x0001D4B2
		[PXDBBool]
		[PXDefault(false, PersistingCheck = 2)]
		[PXUIField(DisplayName = "Own Shipment on Confirm")]
		[PXUIEnabled(typeof(Where<ALHasFeature<ALOwnShipmentSOShipmentEntryExt.feature>>))]
		public bool? OwnShipment { get; set; }

		// Token: 0x170004FA RID: 1274
		// (get) Token: 0x06000BD6 RID: 3030 RVA: 0x0001F2BB File Offset: 0x0001D4BB
		// (set) Token: 0x06000BD7 RID: 3031 RVA: 0x0001F2C3 File Offset: 0x0001D4C3
		[PXDBBool]
		[PXDefault(false, PersistingCheck = 2)]
		[PXUIField(DisplayName = "Print on Shipment Confirmed")]
		[PXUIEnabled(typeof(Where<ALHasFeature<ALPrintOnConfirmSOShipmentEntryExt.feature>>))]
		public bool? PrintOnConfirm { get; set; }

		// Token: 0x170004FB RID: 1275
		// (get) Token: 0x06000BD8 RID: 3032 RVA: 0x0001F2CC File Offset: 0x0001D4CC
		// (set) Token: 0x06000BD9 RID: 3033 RVA: 0x0001F2D4 File Offset: 0x0001D4D4
		[ALModelIDForeign(typeof(Where<ALModel.screenID, Equal<ACConstants.ScreenIDs.Shipments>, And<ALModel.modelType, In3<ALModelType.group, ALModelType.single>>>), DisplayName = "Shipment Confirmed Model")]
		[PXDefault(PersistingCheck = 2)]
		[PXUIVisible(typeof(Where<ALSetup.printOnConfirm, Equal<True>>))]
		[PXUIEnabled(typeof(Where<ALSetup.printOnConfirm, Equal<True>, And<ALHasFeature<ALPrintOnConfirmSOShipmentEntryExt.feature>>>))]
		[PXUIRequired(typeof(Where<ALSetup.printOnConfirm, Equal<True>>))]
		[PXForeignReference(typeof(ALSetup.FK.PrintOnConfirmModel))]
		public Guid? PrintOnConfirmModelID { get; set; }

		// Token: 0x170004FC RID: 1276
		// (get) Token: 0x06000BDA RID: 3034 RVA: 0x0001F2DD File Offset: 0x0001D4DD
		// (set) Token: 0x06000BDB RID: 3035 RVA: 0x0001F2E5 File Offset: 0x0001D4E5
		[ALMultiOptions(DisplayName = "Enable For")]
		[ALEnableIntegrationMultiDropDown]
		public virtual string EnableIntegration { get; set; }

		// Token: 0x170004FD RID: 1277
		// (get) Token: 0x06000BDC RID: 3036 RVA: 0x0001F2EE File Offset: 0x0001D4EE
		// (set) Token: 0x06000BDD RID: 3037 RVA: 0x0001F2F6 File Offset: 0x0001D4F6
		[PXDBBool]
		[PXDefault(false, PersistingCheck = 2)]
		[PXUIField(DisplayName = "Print Production Ticket Via Cloud")]
		public bool? PrintProductionTicketViaCloud { get; set; }

		// Token: 0x170004FE RID: 1278
		// (get) Token: 0x06000BDE RID: 3038 RVA: 0x0001F2FF File Offset: 0x0001D4FF
		// (set) Token: 0x06000BDF RID: 3039 RVA: 0x0001F307 File Offset: 0x0001D507
		[ALModelIDForeign(typeof(Where<ALModel.screenID, Equal<ACConstants.ScreenIDs.ProductionOrders>, And<ALModel.modelType, Equal<ALModelType.printerSetup>, And<ALModel.active, Equal<True>>>>), DisplayName = "Production Ticket Printer Setup")]
		[PXDefault(PersistingCheck = 2)]
		[PXUIVisible(typeof(Where<ALSetup.printProductionTicketViaCloud, Equal<True>>))]
		[PXUIEnabled(typeof(Where<ALSetup.printProductionTicketViaCloud, Equal<True>>))]
		[PXUIRequired(typeof(Where<ALSetup.printProductionTicketViaCloud, Equal<True>>))]
		[PXForeignReference(typeof(ALSetup.FK.PrintTicketModel))]
		public Guid? PrintProductionTicketModelID { get; set; }

		// Token: 0x170004FF RID: 1279
		// (get) Token: 0x06000BE0 RID: 3040 RVA: 0x0001F310 File Offset: 0x0001D510
		// (set) Token: 0x06000BE1 RID: 3041 RVA: 0x0001F318 File Offset: 0x0001D518
		[ALRuleIDForeign(typeof(Where<ALRule.screenID, Equal<ACConstants.ScreenIDs.ProductionOrders>, And<ALRule.active, Equal<True>>>), DisplayName = "Production Ticket Print Rule")]
		[PXDefault(PersistingCheck = 2)]
		[PXUIVisible(typeof(Where<ALSetup.printProductionTicketViaCloud, Equal<True>>))]
		[PXUIEnabled(typeof(Where<ALSetup.printProductionTicketViaCloud, Equal<True>>))]
		[PXForeignReference(typeof(ALSetup.FK.PrintTicketRule))]
		public Guid? PrintProductionTicketRuleID { get; set; }

		// Token: 0x17000500 RID: 1280
		// (get) Token: 0x06000BE2 RID: 3042 RVA: 0x0001F321 File Offset: 0x0001D521
		// (set) Token: 0x06000BE3 RID: 3043 RVA: 0x0001F329 File Offset: 0x0001D529
		[PXDBBool]
		[PXDefault(false, PersistingCheck = 2)]
		[PXUIField(DisplayName = "Print Carrier Labels Via Cloud")]
		public bool? PrintCarrierLabelsViaCloud { get; set; }

		// Token: 0x17000501 RID: 1281
		// (get) Token: 0x06000BE4 RID: 3044 RVA: 0x0001F332 File Offset: 0x0001D532
		// (set) Token: 0x06000BE5 RID: 3045 RVA: 0x0001F33A File Offset: 0x0001D53A
		[ALModelIDForeign(typeof(Where<ALModel.screenID, Equal<ACConstants.ScreenIDs.Shipments>, And<ALModel.modelType, Equal<ALModelType.printerSetup>, And<ALModel.active, Equal<True>>>>), DisplayName = "Carrier Labels Printer Setup")]
		[PXDefault(PersistingCheck = 2)]
		[PXUIVisible(typeof(Where<ALSetup.printCarrierLabelsViaCloud, Equal<True>>))]
		[PXUIEnabled(typeof(Where<ALSetup.printCarrierLabelsViaCloud, Equal<True>>))]
		[PXUIRequired(typeof(Where<ALSetup.printCarrierLabelsViaCloud, Equal<True>>))]
		[PXForeignReference(typeof(ALSetup.FK.PrintCarrierModel))]
		public Guid? PrintCarrierLabelsModelID { get; set; }

		// Token: 0x17000502 RID: 1282
		// (get) Token: 0x06000BE6 RID: 3046 RVA: 0x0001F343 File Offset: 0x0001D543
		// (set) Token: 0x06000BE7 RID: 3047 RVA: 0x0001F34B File Offset: 0x0001D54B
		[PXNote]
		public virtual Guid? NoteID { get; set; }

		// Token: 0x17000503 RID: 1283
		// (get) Token: 0x06000BE8 RID: 3048 RVA: 0x0001F354 File Offset: 0x0001D554
		// (set) Token: 0x06000BE9 RID: 3049 RVA: 0x0001F35C File Offset: 0x0001D55C
		[PXDBCreatedByID]
		public virtual Guid? CreatedByID { get; set; }

		// Token: 0x17000504 RID: 1284
		// (get) Token: 0x06000BEA RID: 3050 RVA: 0x0001F365 File Offset: 0x0001D565
		// (set) Token: 0x06000BEB RID: 3051 RVA: 0x0001F36D File Offset: 0x0001D56D
		[PXDBCreatedByScreenID]
		public virtual string CreatedByScreenID { get; set; }

		// Token: 0x17000505 RID: 1285
		// (get) Token: 0x06000BEC RID: 3052 RVA: 0x0001F376 File Offset: 0x0001D576
		// (set) Token: 0x06000BED RID: 3053 RVA: 0x0001F37E File Offset: 0x0001D57E
		[PXDBCreatedDateTime]
		public virtual DateTime? CreatedDateTime { get; set; }

		// Token: 0x17000506 RID: 1286
		// (get) Token: 0x06000BEE RID: 3054 RVA: 0x0001F387 File Offset: 0x0001D587
		// (set) Token: 0x06000BEF RID: 3055 RVA: 0x0001F38F File Offset: 0x0001D58F
		[PXDBLastModifiedByID]
		public virtual Guid? LastModifiedByID { get; set; }

		// Token: 0x17000507 RID: 1287
		// (get) Token: 0x06000BF0 RID: 3056 RVA: 0x0001F398 File Offset: 0x0001D598
		// (set) Token: 0x06000BF1 RID: 3057 RVA: 0x0001F3A0 File Offset: 0x0001D5A0
		[PXDBLastModifiedByScreenID]
		public virtual string LastModifiedByScreenID { get; set; }

		// Token: 0x17000508 RID: 1288
		// (get) Token: 0x06000BF2 RID: 3058 RVA: 0x0001F3A9 File Offset: 0x0001D5A9
		// (set) Token: 0x06000BF3 RID: 3059 RVA: 0x0001F3B1 File Offset: 0x0001D5B1
		[PXDBLastModifiedDateTime]
		public virtual DateTime? LastModifiedDateTime { get; set; }

		// Token: 0x17000509 RID: 1289
		// (get) Token: 0x06000BF4 RID: 3060 RVA: 0x0001F3BA File Offset: 0x0001D5BA
		// (set) Token: 0x06000BF5 RID: 3061 RVA: 0x0001F3C2 File Offset: 0x0001D5C2
		[PXDBTimestamp]
		public virtual byte[] tstamp { get; set; }

		// Token: 0x020006C5 RID: 1733
		public static class FK
		{
			// Token: 0x02000A13 RID: 2579
			public class Margin : PrimaryKeyOf<ALMargin>.By<ALMargin.marginID>.ForeignKeyOf<ALSetup>.By<ALSetup.defaultMarginID>
			{
			}

			// Token: 0x02000A14 RID: 2580
			public class Format : PrimaryKeyOf<ALFormat>.By<ALFormat.formatID>.ForeignKeyOf<ALSetup>.By<ALSetup.defaultFormatID>
			{
			}

			// Token: 0x02000A15 RID: 2581
			public class Category : PrimaryKeyOf<ALCategory>.By<ALCategory.categoryID>.ForeignKeyOf<ALSetup>.By<ALSetup.defaultCategoryID>
			{
			}

			// Token: 0x02000A16 RID: 2582
			public class LabelZoomCategory : PrimaryKeyOf<ALCategory>.By<ALCategory.categoryID>.ForeignKeyOf<ALSetup>.By<ALSetup.labelZoomCategoryID>
			{
			}

			// Token: 0x02000A17 RID: 2583
			public class BoxPrintModel : PrimaryKeyOf<ALModel>.By<ALModel.labelID>.ForeignKeyOf<ALSetup>.By<ALSetup.boxPrintModelID>
			{
			}

			// Token: 0x02000A18 RID: 2584
			public class PrintOnConfirmModel : PrimaryKeyOf<ALModel>.By<ALModel.labelID>.ForeignKeyOf<ALSetup>.By<ALSetup.printOnConfirmModelID>
			{
			}

			// Token: 0x02000A19 RID: 2585
			public class LabelZoomImageSubstitution : PrimaryKeyOf<ALSubstitution>.By<ALSubstitution.substitutionID>.ForeignKeyOf<ALSetup>.By<ALSetup.labelZoomImageSubstitutionID>
			{
			}

			// Token: 0x02000A1A RID: 2586
			public class PrintTicketModel : PrimaryKeyOf<ALModel>.By<ALModel.labelID>.ForeignKeyOf<ALSetup>.By<ALSetup.printProductionTicketModelID>
			{
			}

			// Token: 0x02000A1B RID: 2587
			public class PrintTicketRule : PrimaryKeyOf<ALRule>.By<ALRule.ruleID>.ForeignKeyOf<ALSetup>.By<ALSetup.printProductionTicketRuleID>
			{
			}

			// Token: 0x02000A1C RID: 2588
			public class Shipment2DModel : PrimaryKeyOf<ALModel>.By<ALModel.labelID>.ForeignKeyOf<ALSetup>.By<ALSetup.shipment2DModelID>
			{
			}

			// Token: 0x02000A1D RID: 2589
			public class PrintCarrierModel : PrimaryKeyOf<ALModel>.By<ALModel.labelID>.ForeignKeyOf<ALSetup>.By<ALSetup.printCarrierLabelsModelID>
			{
			}
		}

		// Token: 0x020006C6 RID: 1734
		[Nullable(new byte[]
		{
			0,
			1,
			1,
			0
		})]
		public abstract class labelaryAPI : BqlType<IBqlString, string>.Field<ALSetup.labelaryAPI>
		{
		}

		// Token: 0x020006C7 RID: 1735
		[Nullable(new byte[]
		{
			0,
			1,
			1,
			0
		})]
		public abstract class labelaryAPIKey : BqlType<IBqlString, string>.Field<ALSetup.labelaryAPIKey>
		{
		}

		// Token: 0x020006C8 RID: 1736
		[Nullable(new byte[]
		{
			0,
			1,
			1,
			0
		})]
		public abstract class labelZoomAPI : BqlType<IBqlString, string>.Field<ALSetup.labelZoomAPI>
		{
		}

		// Token: 0x020006C9 RID: 1737
		[Nullable(new byte[]
		{
			0,
			1,
			1,
			0
		})]
		public abstract class labelZoomAPIKey : BqlType<IBqlString, string>.Field<ALSetup.labelZoomAPIKey>
		{
		}

		// Token: 0x020006CA RID: 1738
		[Nullable(new byte[]
		{
			0,
			1,
			0
		})]
		public abstract class labelZoomCategoryID : BqlType<IBqlGuid, Guid>.Field<ALSetup.labelZoomCategoryID>
		{
		}

		// Token: 0x020006CB RID: 1739
		[Nullable(new byte[]
		{
			0,
			1,
			0
		})]
		public abstract class labelZoomImageSubstitutionID : BqlType<IBqlGuid, Guid>.Field<ALSetup.labelZoomImageSubstitutionID>
		{
		}

		// Token: 0x020006CC RID: 1740
		[Nullable(new byte[]
		{
			0,
			1,
			1,
			0
		})]
		public abstract class mongoURL : BqlType<IBqlString, string>.Field<ALSetup.mongoURL>
		{
		}

		// Token: 0x020006CD RID: 1741
		[Nullable(new byte[]
		{
			0,
			1,
			1,
			0
		})]
		public abstract class mongoOptions : BqlType<IBqlString, string>.Field<ALSetup.mongoOptions>
		{
		}

		// Token: 0x020006CE RID: 1742
		[Nullable(new byte[]
		{
			0,
			1,
			1,
			0
		})]
		public abstract class zplGraphicCreator : BqlType<IBqlString, string>.Field<ALSetup.zplGraphicCreator>
		{
		}

		// Token: 0x020006CF RID: 1743
		[Nullable(new byte[]
		{
			0,
			1,
			1,
			0
		})]
		public abstract class defaultLanguage : BqlType<IBqlString, string>.Field<ALSetup.defaultLanguage>
		{
		}

		// Token: 0x020006D0 RID: 1744
		[Nullable(new byte[]
		{
			0,
			1,
			0
		})]
		public abstract class defaultFormatID : BqlType<IBqlGuid, Guid>.Field<ALSetup.defaultFormatID>
		{
		}

		// Token: 0x020006D1 RID: 1745
		[Nullable(new byte[]
		{
			0,
			1,
			0
		})]
		public abstract class defaultMarginID : BqlType<IBqlGuid, Guid>.Field<ALSetup.defaultMarginID>
		{
		}

		// Token: 0x020006D2 RID: 1746
		[Nullable(new byte[]
		{
			0,
			1,
			0
		})]
		public abstract class defaultCategoryID : BqlType<IBqlGuid, Guid>.Field<ALSetup.defaultCategoryID>
		{
		}

		// Token: 0x020006D3 RID: 1747
		[Nullable(new byte[]
		{
			0,
			1,
			0
		})]
		public abstract class generateShipment2D : BqlType<IBqlBool, bool>.Field<ALSetup.generateShipment2D>
		{
		}

		// Token: 0x020006D4 RID: 1748
		[Nullable(new byte[]
		{
			0,
			1,
			0
		})]
		public abstract class shipment2DModelID : BqlType<IBqlGuid, Guid>.Field<ALSetup.shipment2DModelID>
		{
		}

		// Token: 0x020006D5 RID: 1749
		[Nullable(new byte[]
		{
			0,
			1,
			1,
			0
		})]
		public abstract class packingListReportID : BqlType<IBqlString, string>.Field<ALSetup.packingListReportID>
		{
		}

		// Token: 0x020006D6 RID: 1750
		[Nullable(new byte[]
		{
			0,
			1,
			1,
			0
		})]
		public abstract class enableCopiesOverride : BqlType<IBqlString, string>.Field<ALSetup.enableCopiesOverride>
		{
		}

		// Token: 0x020006D7 RID: 1751
		[Nullable(new byte[]
		{
			0,
			1,
			1,
			0
		})]
		public abstract class enablePrinterOverride : BqlType<IBqlString, string>.Field<ALSetup.enablePrinterOverride>
		{
		}

		// Token: 0x020006D8 RID: 1752
		[Nullable(new byte[]
		{
			0,
			1,
			0
		})]
		public abstract class nbDaysToKeep : BqlType<IBqlShort, short>.Field<ALSetup.nbDaysToKeep>
		{
		}

		// Token: 0x020006D9 RID: 1753
		[Nullable(new byte[]
		{
			0,
			1,
			1,
			0
		})]
		public abstract class devMode : BqlType<IBqlString, string>.Field<ALSetup.devMode>
		{
		}

		// Token: 0x020006DA RID: 1754
		[Nullable(new byte[]
		{
			0,
			1,
			0
		})]
		public abstract class recordImportMode : BqlType<IBqlInt, int>.Field<ALSetup.recordImportMode>
		{
		}

		// Token: 0x020006DB RID: 1755
		[Nullable(new byte[]
		{
			0,
			1,
			1,
			0
		})]
		public abstract class printNodeAPI : BqlType<IBqlString, string>.Field<ALSetup.printNodeAPI>
		{
		}

		// Token: 0x020006DC RID: 1756
		[Nullable(new byte[]
		{
			0,
			1,
			1,
			0
		})]
		public abstract class printNodeAPIKey : BqlType<IBqlString, string>.Field<ALSetup.printNodeAPIKey>
		{
		}

		// Token: 0x020006DD RID: 1757
		[Nullable(new byte[]
		{
			0,
			1,
			0
		})]
		public abstract class fixPackageLineNbr : BqlType<IBqlBool, bool>.Field<ALSetup.fixPackageLineNbr>
		{
		}

		// Token: 0x020006DE RID: 1758
		[Nullable(new byte[]
		{
			0,
			1,
			0
		})]
		public abstract class boxPrint : BqlType<IBqlBool, bool>.Field<ALSetup.boxPrint>
		{
		}

		// Token: 0x020006DF RID: 1759
		[Nullable(new byte[]
		{
			0,
			1,
			0
		})]
		public abstract class boxPrintModelID : BqlType<IBqlGuid, Guid>.Field<ALSetup.boxPrintModelID>
		{
		}

		// Token: 0x020006E0 RID: 1760
		[Nullable(new byte[]
		{
			0,
			1,
			0
		})]
		public abstract class ownShipment : BqlType<IBqlBool, bool>.Field<ALSetup.ownShipment>
		{
		}

		// Token: 0x020006E1 RID: 1761
		[Nullable(new byte[]
		{
			0,
			1,
			0
		})]
		public abstract class printOnConfirm : BqlType<IBqlBool, bool>.Field<ALSetup.printOnConfirm>
		{
		}

		// Token: 0x020006E2 RID: 1762
		[Nullable(new byte[]
		{
			0,
			1,
			0
		})]
		public abstract class printOnConfirmModelID : BqlType<IBqlGuid, Guid>.Field<ALSetup.printOnConfirmModelID>
		{
		}

		// Token: 0x020006E3 RID: 1763
		[Nullable(new byte[]
		{
			0,
			1,
			1,
			0
		})]
		public abstract class enableIntegration : BqlType<IBqlString, string>.Field<ALSetup.enableIntegration>
		{
		}

		// Token: 0x020006E4 RID: 1764
		[Nullable(new byte[]
		{
			0,
			1,
			0
		})]
		public abstract class printProductionTicketViaCloud : BqlType<IBqlBool, bool>.Field<ALSetup.printProductionTicketViaCloud>
		{
		}

		// Token: 0x020006E5 RID: 1765
		[Nullable(new byte[]
		{
			0,
			1,
			0
		})]
		public abstract class printProductionTicketModelID : BqlType<IBqlGuid, Guid>.Field<ALSetup.printProductionTicketModelID>
		{
		}

		// Token: 0x020006E6 RID: 1766
		[Nullable(new byte[]
		{
			0,
			1,
			0
		})]
		public abstract class printProductionTicketRuleID : BqlType<IBqlGuid, Guid>.Field<ALSetup.printProductionTicketRuleID>
		{
		}

		// Token: 0x020006E7 RID: 1767
		[Nullable(new byte[]
		{
			0,
			1,
			0
		})]
		public abstract class printCarrierLabelsViaCloud : BqlType<IBqlBool, bool>.Field<ALSetup.printCarrierLabelsViaCloud>
		{
		}

		// Token: 0x020006E8 RID: 1768
		[Nullable(new byte[]
		{
			0,
			1,
			0
		})]
		public abstract class printCarrierLabelsModelID : BqlType<IBqlGuid, Guid>.Field<ALSetup.printCarrierLabelsModelID>
		{
		}

		// Token: 0x020006E9 RID: 1769
		[Nullable(new byte[]
		{
			0,
			1,
			0
		})]
		public abstract class noteID : BqlType<IBqlGuid, Guid>.Field<ALSetup.noteID>
		{
		}

		// Token: 0x020006EA RID: 1770
		[Nullable(new byte[]
		{
			0,
			1,
			0
		})]
		public abstract class createdByID : BqlType<IBqlGuid, Guid>.Field<ALSetup.createdByID>
		{
		}

		// Token: 0x020006EB RID: 1771
		[Nullable(new byte[]
		{
			0,
			1,
			1,
			0
		})]
		public abstract class createdByScreenID : BqlType<IBqlString, string>.Field<ALSetup.createdByScreenID>
		{
		}

		// Token: 0x020006EC RID: 1772
		[Nullable(new byte[]
		{
			0,
			1,
			0
		})]
		public abstract class createdDateTime : BqlType<IBqlDateTime, DateTime>.Field<ALSetup.createdDateTime>
		{
		}

		// Token: 0x020006ED RID: 1773
		[Nullable(new byte[]
		{
			0,
			1,
			0
		})]
		public abstract class lastModifiedByID : BqlType<IBqlGuid, Guid>.Field<ALSetup.lastModifiedByID>
		{
		}

		// Token: 0x020006EE RID: 1774
		[Nullable(new byte[]
		{
			0,
			1,
			1,
			0
		})]
		public abstract class lastModifiedByScreenID : BqlType<IBqlString, string>.Field<ALSetup.lastModifiedByScreenID>
		{
		}

		// Token: 0x020006EF RID: 1775
		[Nullable(new byte[]
		{
			0,
			1,
			0
		})]
		public abstract class lastModifiedDateTime : BqlType<IBqlDateTime, DateTime>.Field<ALSetup.lastModifiedDateTime>
		{
		}

		// Token: 0x020006F0 RID: 1776
		[Nullable(new byte[]
		{
			0,
			1,
			1,
			0
		})]
		public abstract class Tstamp : BqlType<IBqlByteArray, byte[]>.Field<ALSetup.Tstamp>
		{
		}
	}
}
