using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using AA.Objects.Core;
using AA.Objects.Core.PrintNode;
using AA.Objects.Labels.Language.Ezp;
using AA.Objects.Labels.Language.Zpl;
using Asgard.Labels.Abstractions.Interface;
using MongoDB.Bson.Serialization.Attributes;
using PX.Data;
using PX.Data.BQL;
using PX.Data.EP;
using PX.Data.ReferentialIntegrity.Attributes;
using PX.SM;

namespace AA.Objects.Labels
{
	// Token: 0x02000101 RID: 257
	[PXCacheName("Label Printer")]
	[PXPrimaryGraph(typeof(ALPrinterMaint))]
	[DebuggerDisplay("{Name} ({Description}), PrinterType={PrinterType}")]
	[Serializable]
	public class ALPrinter : PXBqlTable, IBqlTable, IBqlTableSystemDataStorage, INotable, IExportable, IRenderableConfig, IPrintNodePrinter, IPrintNodeObject, IEpsonPrinter, IPrinter, ILabelPrinter
	{
		// Token: 0x170003B9 RID: 953
		// (get) Token: 0x0600095C RID: 2396 RVA: 0x0001DCFE File Offset: 0x0001BEFE
		// (set) Token: 0x0600095D RID: 2397 RVA: 0x0001DD06 File Offset: 0x0001BF06
		[ALGuidID]
		public virtual Guid? PrinterID { get; set; }

		// Token: 0x170003BA RID: 954
		// (get) Token: 0x0600095E RID: 2398 RVA: 0x0001DD0F File Offset: 0x0001BF0F
		[BsonElement]
		public Guid? ID
		{
			get
			{
				return this.PrinterID;
			}
		}

		// Token: 0x170003BB RID: 955
		// (get) Token: 0x0600095F RID: 2399 RVA: 0x0001DD17 File Offset: 0x0001BF17
		// (set) Token: 0x06000960 RID: 2400 RVA: 0x0001DD1F File Offset: 0x0001BF1F
		[ALActive]
		public virtual bool? Active { get; set; }

		// Token: 0x170003BC RID: 956
		// (get) Token: 0x06000961 RID: 2401 RVA: 0x0001DD28 File Offset: 0x0001BF28
		// (set) Token: 0x06000962 RID: 2402 RVA: 0x0001DD30 File Offset: 0x0001BF30
		[ALSystem]
		public virtual bool? IsSystem { get; set; }

		// Token: 0x170003BD RID: 957
		// (get) Token: 0x06000963 RID: 2403 RVA: 0x0001DD39 File Offset: 0x0001BF39
		// (set) Token: 0x06000964 RID: 2404 RVA: 0x0001DD41 File Offset: 0x0001BF41
		[ALExport]
		public virtual bool? AllowExport { get; set; }

		// Token: 0x170003BE RID: 958
		// (get) Token: 0x06000965 RID: 2405 RVA: 0x0001DD4A File Offset: 0x0001BF4A
		// (set) Token: 0x06000966 RID: 2406 RVA: 0x0001DD52 File Offset: 0x0001BF52
		[PXDBBool]
		[PXDefault(false, PersistingCheck = 2)]
		[PXUIField(DisplayName = "Allow Other Size", Visibility = 7)]
		[PXUIVisible(typeof(Where<ALPrinter.printerType, NotEqual<ALConstants.Destinations._null>>))]
		[PXUIEnabled(typeof(Where<ALPrinter.printerType, NotEqual<ALConstants.Destinations._null>>))]
		public virtual bool? AllowOtherSize { get; set; }

		// Token: 0x170003BF RID: 959
		// (get) Token: 0x06000967 RID: 2407 RVA: 0x0001DD5B File Offset: 0x0001BF5B
		// (set) Token: 0x06000968 RID: 2408 RVA: 0x0001DD63 File Offset: 0x0001BF63
		[ALName(typeof(ALPrinter.name), typeof(ALPrinter.description), 50, true, IsKey = true, DisplayName = "Printer ID")]
		public virtual string Name { get; set; }

		// Token: 0x170003C0 RID: 960
		// (get) Token: 0x06000969 RID: 2409 RVA: 0x0001DD6C File Offset: 0x0001BF6C
		// (set) Token: 0x0600096A RID: 2410 RVA: 0x0001DD74 File Offset: 0x0001BF74
		[ALDescription]
		[PXFieldDescription]
		public virtual string Description { get; set; }

		// Token: 0x170003C1 RID: 961
		// (get) Token: 0x0600096B RID: 2411 RVA: 0x0001DD7D File Offset: 0x0001BF7D
		// (set) Token: 0x0600096C RID: 2412 RVA: 0x0001DD85 File Offset: 0x0001BF85
		[PXDBString(2, IsFixed = true)]
		[PXDefault("PN")]
		[PXUIField(DisplayName = "Printer Type", Visibility = 7, Required = true)]
		[ALPrinterTypeSelectable]
		public virtual string PrinterType { get; set; }

		// Token: 0x170003C2 RID: 962
		// (get) Token: 0x0600096D RID: 2413 RVA: 0x0001DD8E File Offset: 0x0001BF8E
		// (set) Token: 0x0600096E RID: 2414 RVA: 0x0001DD96 File Offset: 0x0001BF96
		[PXDBBool]
		[PXUIField(DisplayName = "Is Rendering", IsReadOnly = true)]
		public virtual bool? IsRendering { get; set; }

		// Token: 0x170003C3 RID: 963
		// (get) Token: 0x0600096F RID: 2415 RVA: 0x0001DD9F File Offset: 0x0001BF9F
		// (set) Token: 0x06000970 RID: 2416 RVA: 0x0001DDA7 File Offset: 0x0001BFA7
		[ALContentType]
		[PXDefault(typeof(Switch<Case<Where<Current<ALPrinter.isRendering>, Equal<True>>, ALContentType.png>, ALContentType.zpl>), PersistingCheck = 2)]
		public int? ContentType { get; set; }

		// Token: 0x170003C4 RID: 964
		// (get) Token: 0x06000971 RID: 2417 RVA: 0x0001DDB0 File Offset: 0x0001BFB0
		// (set) Token: 0x06000972 RID: 2418 RVA: 0x0001DDB8 File Offset: 0x0001BFB8
		[ALFormatIDForeign(typeof(Where<True, Equal<True>>))]
		[PXForeignReference(typeof(ALPrinter.FK.Format))]
		[PXDefault(PersistingCheck = 2)]
		[PXUIVisible(typeof(Where<ALPrinter.printerType, NotEqual<ALConstants.Destinations._null>>))]
		[PXUIEnabled(typeof(Where<ALPrinter.printerType, NotEqual<ALConstants.Destinations._null>>))]
		[PXUIRequired(typeof(Where<ALPrinter.printerType, NotEqual<ALConstants.Destinations._null>>))]
		public virtual Guid? FormatID { get; set; }

		// Token: 0x170003C5 RID: 965
		// (get) Token: 0x06000973 RID: 2419 RVA: 0x0001DDC1 File Offset: 0x0001BFC1
		// (set) Token: 0x06000974 RID: 2420 RVA: 0x0001DDC9 File Offset: 0x0001BFC9
		[ALMarginIDForeign(DisplayName = "Adjustment Margin")]
		[PXForeignReference(typeof(ALPrinter.FK.Margin))]
		public virtual Guid? MarginID { get; set; }

		// Token: 0x170003C6 RID: 966
		// (get) Token: 0x06000975 RID: 2421 RVA: 0x0001DDD2 File Offset: 0x0001BFD2
		// (set) Token: 0x06000976 RID: 2422 RVA: 0x0001DDDA File Offset: 0x0001BFDA
		[ALDeviceHubID]
		[PXUIVisible(typeof(Where<ALPrinter.printerType, Equal<ALConstants.Destinations.deviceHub>>))]
		[PXUIEnabled(typeof(Where<ALPrinter.printerType, Equal<ALConstants.Destinations.deviceHub>>))]
		[PXUIRequired(typeof(Where<ALPrinter.printerType, Equal<ALConstants.Destinations.deviceHub>>))]
		public string DeviceHubID { get; set; }

		// Token: 0x170003C7 RID: 967
		// (get) Token: 0x06000977 RID: 2423 RVA: 0x0001DDE3 File Offset: 0x0001BFE3
		// (set) Token: 0x06000978 RID: 2424 RVA: 0x0001DDEB File Offset: 0x0001BFEB
		[ALSMPrinterIDForeign(typeof(Where<SMPrinter.deviceHubID, Equal<Current<ALPrinter.deviceHubID>>, And<SMPrinter.isActive, Equal<True>>>))]
		[PXDefault(PersistingCheck = 2)]
		[PXForeignReference(typeof(ALPrinter.FK.AcuPrinter))]
		[PXUIVisible(typeof(Where<ALPrinter.printerType, Equal<ALConstants.Destinations.deviceHub>>))]
		[PXUIEnabled(typeof(Where<ALPrinter.printerType, Equal<ALConstants.Destinations.deviceHub>>))]
		[PXUIRequired(typeof(Where<ALPrinter.printerType, Equal<ALConstants.Destinations.deviceHub>>))]
		public Guid? AcuPrinterID { get; set; }

		// Token: 0x170003C8 RID: 968
		// (get) Token: 0x06000979 RID: 2425 RVA: 0x0001DDF4 File Offset: 0x0001BFF4
		// (set) Token: 0x0600097A RID: 2426 RVA: 0x0001DDFC File Offset: 0x0001BFFC
		[PXRSACryptString(IsUnicode = true, InputMask = "")]
		[PXUIField(DisplayName = "Cloud Print Specific API Key")]
		[PXDefault(PersistingCheck = 2)]
		[PXUIVisible(typeof(Where<ALPrinter.printerType, Equal<ACConstants.Destinations.printNode>>))]
		[PXUIEnabled(typeof(Where<ALPrinter.printerType, Equal<ACConstants.Destinations.printNode>>))]
		public virtual string PrintNodeAPIKey { get; set; }

		// Token: 0x170003C9 RID: 969
		// (get) Token: 0x0600097B RID: 2427 RVA: 0x0001DE05 File Offset: 0x0001C005
		// (set) Token: 0x0600097C RID: 2428 RVA: 0x0001DE0D File Offset: 0x0001C00D
		[ALPrintNodeComputerID(typeof(ALPrinter.printNodeAPIKey))]
		[PXDefault(PersistingCheck = 2)]
		[PXUIVisible(typeof(Where<ALPrinter.printerType, Equal<ACConstants.Destinations.printNode>>))]
		[PXUIEnabled(typeof(Where<ALPrinter.printerType, Equal<ACConstants.Destinations.printNode>>))]
		[PXUIRequired(typeof(Where<ALPrinter.printerType, Equal<ACConstants.Destinations.printNode>>))]
		public int? PrintNodeComputerID { get; set; }

		// Token: 0x170003CA RID: 970
		// (get) Token: 0x0600097D RID: 2429 RVA: 0x0001DE16 File Offset: 0x0001C016
		// (set) Token: 0x0600097E RID: 2430 RVA: 0x0001DE1E File Offset: 0x0001C01E
		[PXString]
		[PXUIField(DisplayName = "Computer Link", IsReadOnly = true)]
		[PXFormula(typeof(ALPrintNodeDeviceLink<ALPrinter.printNodeComputerID>))]
		[PXDefault(PersistingCheck = 2)]
		[PXUIVisible(typeof(Where<ALPrinter.printerType, Equal<ACConstants.Destinations.printNode>>))]
		public string PrintNodeComputerLink { get; set; }

		// Token: 0x170003CB RID: 971
		// (get) Token: 0x0600097F RID: 2431 RVA: 0x0001DE27 File Offset: 0x0001C027
		// (set) Token: 0x06000980 RID: 2432 RVA: 0x0001DE2F File Offset: 0x0001C02F
		[ALPrintNodeComputerState]
		[PXDefault(PersistingCheck = 2)]
		[PXUIVisible(typeof(Where<ALPrinter.printerType, Equal<ACConstants.Destinations.printNode>>))]
		public virtual string ComputerState { get; set; }

		// Token: 0x170003CC RID: 972
		// (get) Token: 0x06000981 RID: 2433 RVA: 0x0001DE38 File Offset: 0x0001C038
		// (set) Token: 0x06000982 RID: 2434 RVA: 0x0001DE40 File Offset: 0x0001C040
		[PXUIField(DisplayName = "Computer State Icon", IsReadOnly = true)]
		[PXImage]
		[PXDefault(PersistingCheck = 2)]
		[PXUIVisible(typeof(Where<ALPrinter.printerType, Equal<ACConstants.Destinations.printNode>>))]
		[PXDBCalced(typeof(Switch<Case<Where<ALPrinter.computerState, Equal<ACConstants.PrintNode.BqlComputerStatus.connected>>, ALStateIcon.connected, Case<Where<ALPrinter.computerState, Equal<ACConstants.PrintNode.BqlComputerStatus.disconnected>>, ALStateIcon.disconnected, Case<Where<ALPrinter.computerState, Equal<ACConstants.PrintNode.BqlComputerStatus.unknown>>, ALStateIcon.unknown>>>>), typeof(string))]
		public virtual string ComputerStateIcon { get; set; }

		// Token: 0x170003CD RID: 973
		// (get) Token: 0x06000983 RID: 2435 RVA: 0x0001DE49 File Offset: 0x0001C049
		// (set) Token: 0x06000984 RID: 2436 RVA: 0x0001DE51 File Offset: 0x0001C051
		[ALPrintNodePrinterID(typeof(ALPrinter.printNodeComputerID), typeof(ALPrinter.printNodeAPIKey))]
		[PXDefault(PersistingCheck = 2)]
		[PXUIVisible(typeof(Where<ALPrinter.printerType, Equal<ACConstants.Destinations.printNode>>))]
		[PXUIEnabled(typeof(Where<ALPrinter.printerType, Equal<ACConstants.Destinations.printNode>>))]
		[PXUIRequired(typeof(Where<ALPrinter.printerType, Equal<ACConstants.Destinations.printNode>>))]
		public int? PrintNodePrinterID { get; set; }

		// Token: 0x170003CE RID: 974
		// (get) Token: 0x06000985 RID: 2437 RVA: 0x0001DE5A File Offset: 0x0001C05A
		// (set) Token: 0x06000986 RID: 2438 RVA: 0x0001DE62 File Offset: 0x0001C062
		[PXString]
		[PXFormula(typeof(ALPrintNodeDeviceLink<ALPrinter.printNodePrinterID>))]
		[PXDefault(PersistingCheck = 2)]
		[PXUIVisible(typeof(Where<ALPrinter.printerType, Equal<ACConstants.Destinations.printNode>>))]
		public string PrintNodePrinterLink { get; set; }

		// Token: 0x170003CF RID: 975
		// (get) Token: 0x06000987 RID: 2439 RVA: 0x0001DE6B File Offset: 0x0001C06B
		// (set) Token: 0x06000988 RID: 2440 RVA: 0x0001DE73 File Offset: 0x0001C073
		[ALPrintNodePrinterState]
		[PXDefault(PersistingCheck = 2)]
		[PXUIVisible(typeof(Where<ALPrinter.printerType, Equal<ACConstants.Destinations.printNode>>))]
		public virtual string PrinterState { get; set; }

		// Token: 0x170003D0 RID: 976
		// (get) Token: 0x06000989 RID: 2441 RVA: 0x0001DE7C File Offset: 0x0001C07C
		// (set) Token: 0x0600098A RID: 2442 RVA: 0x0001DE84 File Offset: 0x0001C084
		[PXUIField(DisplayName = "Printer State Icon", IsReadOnly = true)]
		[PXImage]
		[PXDefault(PersistingCheck = 2)]
		[PXUIVisible(typeof(Where<ALPrinter.printerType, Equal<ACConstants.Destinations.printNode>>))]
		[PXDBCalced(typeof(Switch<Case<Where<ALPrinter.printerState, Equal<ACConstants.PrintNode.BqlPrinterStatus.out_of_paper>>, ALStateIcon.out_of_paper, Case<Where<ALPrinter.printerState, Equal<ACConstants.PrintNode.BqlPrinterStatus.disconnected>>, ALStateIcon.disconnected, Case<Where<ALPrinter.printerState, Equal<ACConstants.PrintNode.BqlPrinterStatus.error>>, ALStateIcon.error, Case<Where<ALPrinter.printerState, Equal<ACConstants.PrintNode.BqlPrinterStatus.idle>>, ALStateIcon.idle, Case<Where<ALPrinter.printerState, Equal<ACConstants.PrintNode.BqlPrinterStatus.online>>, ALStateIcon.online, Case<Where<ALPrinter.printerState, Equal<ACConstants.PrintNode.BqlPrinterStatus.offline>>, ALStateIcon.offline, Case<Where<ALPrinter.printerState, Equal<ACConstants.PrintNode.BqlPrinterStatus.unknown>>, ALStateIcon.unknown>>>>>>>>), typeof(string))]
		public virtual string PrinterStateIcon { get; set; }

		// Token: 0x170003D1 RID: 977
		// (get) Token: 0x0600098B RID: 2443 RVA: 0x0001DE8D File Offset: 0x0001C08D
		// (set) Token: 0x0600098C RID: 2444 RVA: 0x0001DE95 File Offset: 0x0001C095
		[PXDBDate(PreserveTime = true, UseTimeZone = true, UseSmallDateTime = false, InputMask = "yyyy'-'MM'-'dd' 'HH':'mm':'ss.fffK", DisplayMask = "yyyy'-'MM'-'dd' 'HH':'mm':'ss.fffK")]
		[PXUIField(DisplayName = "State Date", IsReadOnly = true)]
		public virtual DateTime? StateDate { get; set; }

		// Token: 0x170003D2 RID: 978
		// (get) Token: 0x0600098D RID: 2445 RVA: 0x0001DE9E File Offset: 0x0001C09E
		// (set) Token: 0x0600098E RID: 2446 RVA: 0x0001DEA6 File Offset: 0x0001C0A6
		[ALPrintNodePrinterCapabilities]
		[PXDefault(PersistingCheck = 2)]
		[PXUIVisible(typeof(Where<ALPrinter.printerType, Equal<ACConstants.Destinations.printNode>>))]
		public virtual string Capabilities { get; set; }

		// Token: 0x170003D3 RID: 979
		// (get) Token: 0x0600098F RID: 2447 RVA: 0x0001DEAF File Offset: 0x0001C0AF
		// (set) Token: 0x06000990 RID: 2448 RVA: 0x0001DEB7 File Offset: 0x0001C0B7
		[ALScreenID]
		[PXForeignReference(typeof(ALPrinter.FK.PortalMap))]
		[PXForeignReference(typeof(ALPrinter.FK.SiteMap))]
		[PXUIVisible(typeof(Where<ALPrinter.printerType, Equal<ALConstants.Destinations.acuFile>>))]
		[PXUIEnabled(typeof(Where<ALPrinter.printerType, Equal<ALConstants.Destinations.acuFile>>))]
		[PXUIRequired(typeof(Where<ALPrinter.printerType, Equal<ALConstants.Destinations.acuFile>>))]
		public string ScreenID { get; set; }

		// Token: 0x170003D4 RID: 980
		// (get) Token: 0x06000991 RID: 2449 RVA: 0x0001DEC0 File Offset: 0x0001C0C0
		// (set) Token: 0x06000992 RID: 2450 RVA: 0x0001DEC8 File Offset: 0x0001C0C8
		[ALGraphType(typeof(ALPrinter.screenID))]
		[PXDefault(PersistingCheck = 2)]
		[PXUIVisible(typeof(Where<ALPrinter.printerType, Equal<ALConstants.Destinations.acuFile>>))]
		[PXUIEnabled(typeof(Where<ALPrinter.printerType, Equal<ALConstants.Destinations.acuFile>>))]
		[PXUIRequired(typeof(Where<ALPrinter.printerType, Equal<ALConstants.Destinations.acuFile>>))]
		public string GraphType { get; set; }

		// Token: 0x170003D5 RID: 981
		// (get) Token: 0x06000993 RID: 2451 RVA: 0x0001DED1 File Offset: 0x0001C0D1
		// (set) Token: 0x06000994 RID: 2452 RVA: 0x0001DED9 File Offset: 0x0001C0D9
		[PXDBString(128, IsUnicode = true)]
		[PXUIField(DisplayName = "Field Name", Visibility = 7)]
		[PXDefault(PersistingCheck = 2)]
		[PXUIVisible(typeof(Where<ALPrinter.printerType, Equal<ALConstants.Destinations.acuFile>>))]
		[PXUIEnabled(typeof(Where<ALPrinter.printerType, Equal<ALConstants.Destinations.acuFile>>))]
		[PXUIRequired(typeof(Where<ALPrinter.printerType, Equal<ALConstants.Destinations.acuFile>>))]
		public string FieldName { get; set; }

		// Token: 0x170003D6 RID: 982
		// (get) Token: 0x06000995 RID: 2453 RVA: 0x0001DEE2 File Offset: 0x0001C0E2
		// (set) Token: 0x06000996 RID: 2454 RVA: 0x0001DEEA File Offset: 0x0001C0EA
		[ALPrintStationIDForeign]
		[PXForeignReference(typeof(ALPrinter.FK.PrintStation))]
		[PXUIVisible(typeof(Where<ALPrinter.printerType, NotIn3<ALConstants.Destinations.labelary, ALConstants.Destinations._null>>))]
		[PXUIEnabled(typeof(Where<ALPrinter.printerType, NotIn3<ALConstants.Destinations.labelary, ALConstants.Destinations._null>>))]
		public virtual Guid? PrintStationID { get; set; }

		// Token: 0x170003D7 RID: 983
		// (get) Token: 0x06000997 RID: 2455 RVA: 0x0001DEF3 File Offset: 0x0001C0F3
		// (set) Token: 0x06000998 RID: 2456 RVA: 0x0001DEFB File Offset: 0x0001C0FB
		[ALDriveName]
		[ALPrinterDrive.ALListAttribute]
		[PXDefault(PersistingCheck = 2)]
		[PXUIVisible(typeof(Where<ALPrinter.printerType, NotEqual<ALConstants.Destinations._null>>))]
		[PXUIEnabled(typeof(Where<ALPrinter.printerType, NotEqual<ALConstants.Destinations._null>>))]
		[PXUIRequired(typeof(Where<ALPrinter.printerType, Equal<ALConstants.Destinations.labelary>>))]
		public virtual string Drive { get; set; }

		// Token: 0x170003D8 RID: 984
		// (get) Token: 0x06000999 RID: 2457 RVA: 0x0001DF04 File Offset: 0x0001C104
		// (set) Token: 0x0600099A RID: 2458 RVA: 0x0001DF0C File Offset: 0x0001C10C
		[PXDBBool]
		[PXDefault(false, PersistingCheck = 2)]
		[PXUIField(DisplayName = "Use Long Files")]
		[PXUIVisible(typeof(Where<ALPrinter.printerType, NotEqual<ALConstants.Destinations._null>>))]
		[PXUIEnabled(typeof(Where<ALPrinter.printerType, NotEqual<ALConstants.Destinations._null>>))]
		public bool? SupportsLongFiles { get; set; }

		// Token: 0x170003D9 RID: 985
		// (get) Token: 0x0600099B RID: 2459 RVA: 0x0001DF15 File Offset: 0x0001C115
		// (set) Token: 0x0600099C RID: 2460 RVA: 0x0001DF1D File Offset: 0x0001C11D
		[PXDBInt]
		[PXUIField(DisplayName = "Encoding")]
		[ALEncoding.ALListAttribute]
		[PXDefault(PersistingCheck = 2)]
		[PXUIVisible(typeof(Where<ALPrinter.printerType, NotEqual<ALConstants.Destinations._null>>))]
		[PXUIEnabled(typeof(Where<ALPrinter.printerType, NotEqual<ALConstants.Destinations._null>>))]
		public virtual int? Encoding { get; set; }

		// Token: 0x170003DA RID: 986
		// (get) Token: 0x0600099D RID: 2461 RVA: 0x0001DF26 File Offset: 0x0001C126
		// (set) Token: 0x0600099E RID: 2462 RVA: 0x0001DF2E File Offset: 0x0001C12E
		[PXDBBool]
		[PXDefault(false, PersistingCheck = 2)]
		[PXUIField(DisplayName = "Push Fonts")]
		[PXUIVisible(typeof(Where<ALPrinter.printerType, NotEqual<ALConstants.Destinations._null>>))]
		[PXUIEnabled(typeof(Where<ALPrinter.printerType, NotEqual<ALConstants.Destinations._null>>))]
		public bool? PushFonts { get; set; }

		// Token: 0x170003DB RID: 987
		// (get) Token: 0x0600099F RID: 2463 RVA: 0x0001DF37 File Offset: 0x0001C137
		// (set) Token: 0x060009A0 RID: 2464 RVA: 0x0001DF3F File Offset: 0x0001C13F
		[PXDBBool]
		[PXDefault(false, PersistingCheck = 2)]
		[PXUIField(DisplayName = "Is Epson")]
		[PXUIVisible(typeof(Where<ALPrinter.printerType, NotIn3<ALConstants.Destinations.labelary, ALConstants.Destinations._null>>))]
		[PXUIEnabled(typeof(Where<ALPrinter.printerType, NotIn3<ALConstants.Destinations.labelary, ALConstants.Destinations._null>>))]
		public bool? IsEpson { get; set; }

		// Token: 0x170003DC RID: 988
		// (get) Token: 0x060009A1 RID: 2465 RVA: 0x0001DF48 File Offset: 0x0001C148
		// (set) Token: 0x060009A2 RID: 2466 RVA: 0x0001DF50 File Offset: 0x0001C150
		[PXDBString(3, IsFixed = true)]
		[PXUIField(DisplayName = "Media Type")]
		[PXDefault(PersistingCheck = 2)]
		[ALMediaCoatingType.ALListAttribute]
		[PXUIVisible(typeof(Where<ALPrinter.isEpson, Equal<True>>))]
		[PXUIEnabled(typeof(Where<ALPrinter.isEpson, Equal<True>>))]
		[PXUIRequired(typeof(Where<ALPrinter.isEpson, Equal<True>>))]
		public virtual string MediaType { get; set; }

		// Token: 0x170003DD RID: 989
		// (get) Token: 0x060009A3 RID: 2467 RVA: 0x0001DF59 File Offset: 0x0001C159
		// (set) Token: 0x060009A4 RID: 2468 RVA: 0x0001DF61 File Offset: 0x0001C161
		[PXDBString(2, IsFixed = true)]
		[PXUIField(DisplayName = "Media Form")]
		[PXDefault(PersistingCheck = 2)]
		[ALMediaForm.ALListAttribute]
		[PXUIVisible(typeof(Where<ALPrinter.isEpson, Equal<True>>))]
		[PXUIEnabled(typeof(Where<ALPrinter.isEpson, Equal<True>>))]
		[PXUIRequired(typeof(Where<ALPrinter.isEpson, Equal<True>>))]
		public virtual string MediaForm { get; set; }

		// Token: 0x170003DE RID: 990
		// (get) Token: 0x060009A5 RID: 2469 RVA: 0x0001DF6A File Offset: 0x0001C16A
		// (set) Token: 0x060009A6 RID: 2470 RVA: 0x0001DF72 File Offset: 0x0001C172
		[PXDBString(2, IsFixed = true)]
		[PXUIField(DisplayName = "Media Source")]
		[PXDefault(PersistingCheck = 2)]
		[ALMediaSource.ALListAttribute]
		[PXUIVisible(typeof(Where<ALPrinter.isEpson, Equal<True>>))]
		[PXUIEnabled(typeof(Where<ALPrinter.isEpson, Equal<True>>))]
		[PXUIRequired(typeof(Where<ALPrinter.isEpson, Equal<True>>))]
		public virtual string MediaSource { get; set; }

		// Token: 0x170003DF RID: 991
		// (get) Token: 0x060009A7 RID: 2471 RVA: 0x0001DF7B File Offset: 0x0001C17B
		// (set) Token: 0x060009A8 RID: 2472 RVA: 0x0001DF83 File Offset: 0x0001C183
		[PXDBString(2, IsFixed = true)]
		[PXUIField(DisplayName = "Media Shape")]
		[PXDefault(PersistingCheck = 2)]
		[ALMediaShape.ALListAttribute]
		[PXUIVisible(typeof(Where<ALPrinter.isEpson, Equal<True>>))]
		[PXUIEnabled(typeof(Where<ALPrinter.isEpson, Equal<True>>))]
		[PXUIRequired(typeof(Where<ALPrinter.isEpson, Equal<True>>))]
		public virtual string MediaShape { get; set; }

		// Token: 0x170003E0 RID: 992
		// (get) Token: 0x060009A9 RID: 2473 RVA: 0x0001DF8C File Offset: 0x0001C18C
		// (set) Token: 0x060009AA RID: 2474 RVA: 0x0001DF94 File Offset: 0x0001C194
		[PXDBString(1, IsFixed = true)]
		[PXUIField(DisplayName = "Edge Detection")]
		[PXDefault(PersistingCheck = 2)]
		[ALEdgeDetection.ALListAttribute]
		[PXUIVisible(typeof(Where<ALPrinter.isEpson, Equal<True>>))]
		[PXUIEnabled(typeof(Where<ALPrinter.isEpson, Equal<True>>))]
		[PXUIRequired(typeof(Where<ALPrinter.isEpson, Equal<True>>))]
		public virtual string EdgeDetection { get; set; }

		// Token: 0x170003E1 RID: 993
		// (get) Token: 0x060009AB RID: 2475 RVA: 0x0001DF9D File Offset: 0x0001C19D
		// (set) Token: 0x060009AC RID: 2476 RVA: 0x0001DFA5 File Offset: 0x0001C1A5
		[PXDBString(1, IsFixed = true)]
		[PXUIField(DisplayName = "Print Mode")]
		[PXDefault(PersistingCheck = 2)]
		[ALPrintMode.ALListAttribute]
		[PXUIVisible(typeof(Where<ALPrinter.isEpson, Equal<True>>))]
		[PXUIEnabled(typeof(Where<ALPrinter.isEpson, Equal<True>>))]
		[PXUIRequired(typeof(Where<ALPrinter.isEpson, Equal<True>>))]
		public virtual string PrintMode { get; set; }

		// Token: 0x170003E2 RID: 994
		// (get) Token: 0x060009AD RID: 2477 RVA: 0x0001DFB0 File Offset: 0x0001C1B0
		[PXBool]
		[PXUIField(Visibility = 3)]
		public virtual bool? ShowFileTransfers
		{
			[PXDependsOnFields(new Type[]
			{
				typeof(ALPrinter.printerType)
			})]
			get
			{
				return new bool?(this.PrinterType != "LA" && this.PrinterType != "PP" && this.PrinterType != "NU");
			}
		}

		// Token: 0x170003E3 RID: 995
		// (get) Token: 0x060009AE RID: 2478 RVA: 0x0001E000 File Offset: 0x0001C200
		[PXBool]
		[PXUIField(Visibility = 3)]
		public virtual bool? ShowChildren
		{
			[PXDependsOnFields(new Type[]
			{
				typeof(ALPrinter.printerType)
			})]
			get
			{
				return new bool?(this.PrinterType == "PP");
			}
		}

		// Token: 0x170003E4 RID: 996
		// (get) Token: 0x060009AF RID: 2479 RVA: 0x0001E028 File Offset: 0x0001C228
		[PXBool]
		[PXUIField(Visibility = 3)]
		public virtual bool? ShowPrintJobs
		{
			[PXDependsOnFields(new Type[]
			{
				typeof(ALPrinter.printerType)
			})]
			get
			{
				return new bool?(this.PrinterType == "PN");
			}
		}

		// Token: 0x170003E5 RID: 997
		// (get) Token: 0x060009B0 RID: 2480 RVA: 0x0001E050 File Offset: 0x0001C250
		[PXBool]
		[PXUIField(Visibility = 3)]
		public virtual bool? ShowCapabilities
		{
			[PXDependsOnFields(new Type[]
			{
				typeof(ALPrinter.printerType)
			})]
			get
			{
				return new bool?(this.PrinterType == "PN");
			}
		}

		// Token: 0x170003E6 RID: 998
		// (get) Token: 0x060009B1 RID: 2481 RVA: 0x0001E077 File Offset: 0x0001C277
		// (set) Token: 0x060009B2 RID: 2482 RVA: 0x0001E07F File Offset: 0x0001C27F
		[PXNote(DescriptionField = typeof(ALPrinter.description))]
		public virtual Guid? NoteID { get; set; }

		// Token: 0x170003E7 RID: 999
		// (get) Token: 0x060009B3 RID: 2483 RVA: 0x0001E088 File Offset: 0x0001C288
		// (set) Token: 0x060009B4 RID: 2484 RVA: 0x0001E090 File Offset: 0x0001C290
		[PXDBCreatedByID]
		public virtual Guid? CreatedByID { get; set; }

		// Token: 0x170003E8 RID: 1000
		// (get) Token: 0x060009B5 RID: 2485 RVA: 0x0001E099 File Offset: 0x0001C299
		// (set) Token: 0x060009B6 RID: 2486 RVA: 0x0001E0A1 File Offset: 0x0001C2A1
		[PXDBCreatedByScreenID]
		public virtual string CreatedByScreenID { get; set; }

		// Token: 0x170003E9 RID: 1001
		// (get) Token: 0x060009B7 RID: 2487 RVA: 0x0001E0AA File Offset: 0x0001C2AA
		// (set) Token: 0x060009B8 RID: 2488 RVA: 0x0001E0B2 File Offset: 0x0001C2B2
		[PXDBCreatedDateTime]
		[PXUIField(DisplayName = "Created On", Enabled = false, IsReadOnly = true)]
		public virtual DateTime? CreatedDateTime { get; set; }

		// Token: 0x170003EA RID: 1002
		// (get) Token: 0x060009B9 RID: 2489 RVA: 0x0001E0BB File Offset: 0x0001C2BB
		// (set) Token: 0x060009BA RID: 2490 RVA: 0x0001E0C3 File Offset: 0x0001C2C3
		[PXDBLastModifiedByID]
		public virtual Guid? LastModifiedByID { get; set; }

		// Token: 0x170003EB RID: 1003
		// (get) Token: 0x060009BB RID: 2491 RVA: 0x0001E0CC File Offset: 0x0001C2CC
		// (set) Token: 0x060009BC RID: 2492 RVA: 0x0001E0D4 File Offset: 0x0001C2D4
		[PXDBLastModifiedByScreenID]
		public virtual string LastModifiedByScreenID { get; set; }

		// Token: 0x170003EC RID: 1004
		// (get) Token: 0x060009BD RID: 2493 RVA: 0x0001E0DD File Offset: 0x0001C2DD
		// (set) Token: 0x060009BE RID: 2494 RVA: 0x0001E0E5 File Offset: 0x0001C2E5
		[PXDBLastModifiedDateTime]
		[PXUIField(DisplayName = "Last Modified On", Enabled = false, IsReadOnly = true)]
		public virtual DateTime? LastModifiedDateTime { get; set; }

		// Token: 0x170003ED RID: 1005
		// (get) Token: 0x060009BF RID: 2495 RVA: 0x0001E0EE File Offset: 0x0001C2EE
		// (set) Token: 0x060009C0 RID: 2496 RVA: 0x0001E0F6 File Offset: 0x0001C2F6
		[PXDBTimestamp]
		public virtual byte[] tstamp { get; set; }

		// Token: 0x02000597 RID: 1431
		public class PK : PrimaryKeyOf<ALPrinter>.By<ALPrinter.printerID>
		{
			// Token: 0x06001C38 RID: 7224 RVA: 0x000583B5 File Offset: 0x000565B5
			public static ALPrinter Find(PXGraph graph, Guid? printerID)
			{
				return PrimaryKeyOf<ALPrinter>.By<ALPrinter.printerID>.FindBy(graph, printerID, 0);
			}
		}

		// Token: 0x02000598 RID: 1432
		public static class FK
		{
			// Token: 0x020009FA RID: 2554
			public class Format : PrimaryKeyOf<ALFormat>.By<ALFormat.formatID>.ForeignKeyOf<ALPrinter>.By<ALPrinter.formatID>
			{
			}

			// Token: 0x020009FB RID: 2555
			public class Margin : PrimaryKeyOf<ALMargin>.By<ALMargin.marginID>.ForeignKeyOf<ALPrinter>.By<ALPrinter.marginID>
			{
			}

			// Token: 0x020009FC RID: 2556
			public class AcuPrinter : PrimaryKeyOf<SMPrinter>.By<SMPrinter.printerID>.ForeignKeyOf<ALPrinter>.By<ALPrinter.acuPrinterID>
			{
			}

			// Token: 0x020009FD RID: 2557
			public class PortalMap : PrimaryKeyOf<PX.SM.PortalMap>.By<PX.SM.PortalMap.nodeID>.ForeignKeyOf<ALPrinter>.By<ALPrinter.screenID>
			{
			}

			// Token: 0x020009FE RID: 2558
			public class SiteMap : PrimaryKeyOf<PX.SM.SiteMap>.By<PX.SM.SiteMap.nodeID>.ForeignKeyOf<ALPrinter>.By<ALPrinter.screenID>
			{
			}

			// Token: 0x020009FF RID: 2559
			public class PrintStation : PrimaryKeyOf<ALPrintStation>.By<ALPrintStation.printStationID>.ForeignKeyOf<ALPrinter>.By<ALPrinter.printStationID>
			{
			}
		}

		// Token: 0x02000599 RID: 1433
		[Nullable(new byte[]
		{
			0,
			1,
			0
		})]
		public abstract class printerID : BqlType<IBqlGuid, Guid>.Field<ALPrinter.printerID>
		{
		}

		// Token: 0x0200059A RID: 1434
		[Nullable(new byte[]
		{
			0,
			1,
			0
		})]
		public abstract class active : BqlType<IBqlBool, bool>.Field<ALPrinter.active>
		{
		}

		// Token: 0x0200059B RID: 1435
		[Nullable(new byte[]
		{
			0,
			1,
			0
		})]
		public abstract class isSystem : BqlType<IBqlBool, bool>.Field<ALPrinter.isSystem>
		{
		}

		// Token: 0x0200059C RID: 1436
		[Nullable(new byte[]
		{
			0,
			1,
			0
		})]
		public abstract class allowExport : BqlType<IBqlBool, bool>.Field<ALPrinter.allowExport>
		{
		}

		// Token: 0x0200059D RID: 1437
		[Nullable(new byte[]
		{
			0,
			1,
			0
		})]
		public abstract class allowOtherSize : BqlType<IBqlBool, bool>.Field<ALPrinter.allowOtherSize>
		{
		}

		// Token: 0x0200059E RID: 1438
		[Nullable(new byte[]
		{
			0,
			1,
			1,
			0
		})]
		public abstract class name : BqlType<IBqlString, string>.Field<ALPrinter.name>
		{
		}

		// Token: 0x0200059F RID: 1439
		[Nullable(new byte[]
		{
			0,
			1,
			1,
			0
		})]
		public abstract class description : BqlType<IBqlString, string>.Field<ALPrinter.description>
		{
		}

		// Token: 0x020005A0 RID: 1440
		[Nullable(new byte[]
		{
			0,
			1,
			1,
			0
		})]
		public abstract class printerType : BqlType<IBqlString, string>.Field<ALPrinter.printerType>
		{
		}

		// Token: 0x020005A1 RID: 1441
		[Nullable(new byte[]
		{
			0,
			1,
			0
		})]
		public abstract class isRendering : BqlType<IBqlBool, bool>.Field<ALPrinter.isRendering>
		{
		}

		// Token: 0x020005A2 RID: 1442
		[Nullable(new byte[]
		{
			0,
			1,
			0
		})]
		public abstract class contentType : BqlType<IBqlInt, int>.Field<ALPrinter.contentType>
		{
		}

		// Token: 0x020005A3 RID: 1443
		[Nullable(new byte[]
		{
			0,
			1,
			0
		})]
		public abstract class formatID : BqlType<IBqlGuid, Guid>.Field<ALPrinter.formatID>
		{
		}

		// Token: 0x020005A4 RID: 1444
		[Nullable(new byte[]
		{
			0,
			1,
			0
		})]
		public abstract class marginID : BqlType<IBqlGuid, Guid>.Field<ALPrinter.marginID>
		{
		}

		// Token: 0x020005A5 RID: 1445
		[Nullable(new byte[]
		{
			0,
			1,
			1,
			0
		})]
		public abstract class deviceHubID : BqlType<IBqlString, string>.Field<ALPrinter.deviceHubID>
		{
		}

		// Token: 0x020005A6 RID: 1446
		[Nullable(new byte[]
		{
			0,
			1,
			0
		})]
		public abstract class acuPrinterID : BqlType<IBqlGuid, Guid>.Field<ALPrinter.acuPrinterID>
		{
		}

		// Token: 0x020005A7 RID: 1447
		[Nullable(new byte[]
		{
			0,
			1,
			1,
			0
		})]
		public abstract class printNodeAPIKey : BqlType<IBqlString, string>.Field<ALPrinter.printNodeAPIKey>
		{
		}

		// Token: 0x020005A8 RID: 1448
		[Nullable(new byte[]
		{
			0,
			1,
			0
		})]
		public abstract class printNodeComputerID : BqlType<IBqlInt, int>.Field<ALPrinter.printNodeComputerID>
		{
		}

		// Token: 0x020005A9 RID: 1449
		[Nullable(new byte[]
		{
			0,
			1,
			1,
			0
		})]
		public abstract class printNodeComputerLink : BqlType<IBqlString, string>.Field<ALPrinter.printNodeComputerLink>
		{
		}

		// Token: 0x020005AA RID: 1450
		[Nullable(new byte[]
		{
			0,
			1,
			1,
			0
		})]
		public abstract class computerState : BqlType<IBqlString, string>.Field<ALPrinter.computerState>
		{
		}

		// Token: 0x020005AB RID: 1451
		[Nullable(new byte[]
		{
			0,
			1,
			1,
			0
		})]
		public abstract class computerStateIcon : BqlType<IBqlString, string>.Field<ALPrinter.computerStateIcon>
		{
		}

		// Token: 0x020005AC RID: 1452
		[Nullable(new byte[]
		{
			0,
			1,
			0
		})]
		public abstract class printNodePrinterID : BqlType<IBqlInt, int>.Field<ALPrinter.printNodePrinterID>
		{
		}

		// Token: 0x020005AD RID: 1453
		[Nullable(new byte[]
		{
			0,
			1,
			1,
			0
		})]
		public abstract class printNodePrinterLink : BqlType<IBqlString, string>.Field<ALPrinter.printNodePrinterLink>
		{
		}

		// Token: 0x020005AE RID: 1454
		[Nullable(new byte[]
		{
			0,
			1,
			1,
			0
		})]
		public abstract class printerState : BqlType<IBqlString, string>.Field<ALPrinter.printerState>
		{
		}

		// Token: 0x020005AF RID: 1455
		[Nullable(new byte[]
		{
			0,
			1,
			1,
			0
		})]
		public abstract class printerStateIcon : BqlType<IBqlString, string>.Field<ALPrinter.printerStateIcon>
		{
		}

		// Token: 0x020005B0 RID: 1456
		[Nullable(new byte[]
		{
			0,
			1,
			0
		})]
		public abstract class stateDate : BqlType<IBqlDateTime, DateTime>.Field<ALPrinter.stateDate>
		{
		}

		// Token: 0x020005B1 RID: 1457
		[Nullable(new byte[]
		{
			0,
			1,
			1,
			0
		})]
		public abstract class capabilities : BqlType<IBqlString, string>.Field<ALPrinter.capabilities>
		{
		}

		// Token: 0x020005B2 RID: 1458
		[Nullable(new byte[]
		{
			0,
			1,
			1,
			0
		})]
		public abstract class screenID : BqlType<IBqlString, string>.Field<ALPrinter.screenID>
		{
		}

		// Token: 0x020005B3 RID: 1459
		[Nullable(new byte[]
		{
			0,
			1,
			1,
			0
		})]
		public abstract class graphType : BqlType<IBqlString, string>.Field<ALPrinter.graphType>
		{
		}

		// Token: 0x020005B4 RID: 1460
		[Nullable(new byte[]
		{
			0,
			1,
			1,
			0
		})]
		public abstract class fieldName : BqlType<IBqlString, string>.Field<ALPrinter.fieldName>
		{
		}

		// Token: 0x020005B5 RID: 1461
		[Nullable(new byte[]
		{
			0,
			1,
			0
		})]
		public abstract class printStationID : BqlType<IBqlGuid, Guid>.Field<ALPrinter.printStationID>
		{
		}

		// Token: 0x020005B6 RID: 1462
		[Nullable(new byte[]
		{
			0,
			1,
			1,
			0
		})]
		public abstract class drive : BqlType<IBqlString, string>.Field<ALPrinter.drive>
		{
		}

		// Token: 0x020005B7 RID: 1463
		[Nullable(new byte[]
		{
			0,
			1,
			0
		})]
		public abstract class supportsLongFiles : BqlType<IBqlBool, bool>.Field<ALPrinter.supportsLongFiles>
		{
		}

		// Token: 0x020005B8 RID: 1464
		[Nullable(new byte[]
		{
			0,
			1,
			0
		})]
		public abstract class encoding : BqlType<IBqlInt, int>.Field<ALPrinter.encoding>
		{
		}

		// Token: 0x020005B9 RID: 1465
		[Nullable(new byte[]
		{
			0,
			1,
			0
		})]
		public abstract class pushFonts : BqlType<IBqlBool, bool>.Field<ALPrinter.pushFonts>
		{
		}

		// Token: 0x020005BA RID: 1466
		[Nullable(new byte[]
		{
			0,
			1,
			0
		})]
		public abstract class isEpson : BqlType<IBqlBool, bool>.Field<ALPrinter.isEpson>
		{
		}

		// Token: 0x020005BB RID: 1467
		[Nullable(new byte[]
		{
			0,
			1,
			1,
			0
		})]
		public abstract class mediaType : BqlType<IBqlString, string>.Field<ALPrinter.mediaType>
		{
		}

		// Token: 0x020005BC RID: 1468
		[Nullable(new byte[]
		{
			0,
			1,
			1,
			0
		})]
		public abstract class mediaForm : BqlType<IBqlString, string>.Field<ALPrinter.mediaForm>
		{
		}

		// Token: 0x020005BD RID: 1469
		[Nullable(new byte[]
		{
			0,
			1,
			1,
			0
		})]
		public abstract class mediaSource : BqlType<IBqlString, string>.Field<ALPrinter.mediaSource>
		{
		}

		// Token: 0x020005BE RID: 1470
		[Nullable(new byte[]
		{
			0,
			1,
			1,
			0
		})]
		public abstract class mediaShape : BqlType<IBqlString, string>.Field<ALPrinter.mediaShape>
		{
		}

		// Token: 0x020005BF RID: 1471
		[Nullable(new byte[]
		{
			0,
			1,
			1,
			0
		})]
		public abstract class edgeDetection : BqlType<IBqlString, string>.Field<ALPrinter.edgeDetection>
		{
		}

		// Token: 0x020005C0 RID: 1472
		[Nullable(new byte[]
		{
			0,
			1,
			1,
			0
		})]
		public abstract class printMode : BqlType<IBqlString, string>.Field<ALPrinter.printMode>
		{
		}

		// Token: 0x020005C1 RID: 1473
		[Nullable(new byte[]
		{
			0,
			1,
			0
		})]
		public abstract class showFileTransfers : BqlType<IBqlBool, bool>.Field<ALPrinter.showFileTransfers>
		{
		}

		// Token: 0x020005C2 RID: 1474
		[Nullable(new byte[]
		{
			0,
			1,
			0
		})]
		public abstract class showChildren : BqlType<IBqlBool, bool>.Field<ALPrinter.showChildren>
		{
		}

		// Token: 0x020005C3 RID: 1475
		[Nullable(new byte[]
		{
			0,
			1,
			0
		})]
		public abstract class showPrintJobs : BqlType<IBqlBool, bool>.Field<ALPrinter.showPrintJobs>
		{
		}

		// Token: 0x020005C4 RID: 1476
		[Nullable(new byte[]
		{
			0,
			1,
			0
		})]
		public abstract class showCapabilities : BqlType<IBqlBool, bool>.Field<ALPrinter.showCapabilities>
		{
		}

		// Token: 0x020005C5 RID: 1477
		[Nullable(new byte[]
		{
			0,
			1,
			0
		})]
		public abstract class noteID : BqlType<IBqlGuid, Guid>.Field<ALPrinter.noteID>
		{
		}

		// Token: 0x020005C6 RID: 1478
		[Nullable(new byte[]
		{
			0,
			1,
			0
		})]
		public abstract class createdByID : BqlType<IBqlGuid, Guid>.Field<ALPrinter.createdByID>
		{
		}

		// Token: 0x020005C7 RID: 1479
		[Nullable(new byte[]
		{
			0,
			1,
			1,
			0
		})]
		public abstract class createdByScreenID : BqlType<IBqlString, string>.Field<ALPrinter.createdByScreenID>
		{
		}

		// Token: 0x020005C8 RID: 1480
		[Nullable(new byte[]
		{
			0,
			1,
			0
		})]
		public abstract class createdDateTime : BqlType<IBqlDateTime, DateTime>.Field<ALPrinter.createdDateTime>
		{
		}

		// Token: 0x020005C9 RID: 1481
		[Nullable(new byte[]
		{
			0,
			1,
			0
		})]
		public abstract class lastModifiedByID : BqlType<IBqlGuid, Guid>.Field<ALPrinter.lastModifiedByID>
		{
		}

		// Token: 0x020005CA RID: 1482
		[Nullable(new byte[]
		{
			0,
			1,
			1,
			0
		})]
		public abstract class lastModifiedByScreenID : BqlType<IBqlString, string>.Field<ALPrinter.lastModifiedByScreenID>
		{
		}

		// Token: 0x020005CB RID: 1483
		[Nullable(new byte[]
		{
			0,
			1,
			0
		})]
		public abstract class lastModifiedDateTime : BqlType<IBqlDateTime, DateTime>.Field<ALPrinter.lastModifiedDateTime>
		{
		}

		// Token: 0x020005CC RID: 1484
		[Nullable(new byte[]
		{
			0,
			1,
			1,
			0
		})]
		public abstract class Tstamp : BqlType<IBqlByteArray, byte[]>.Field<ALPrinter.Tstamp>
		{
		}
	}
}
