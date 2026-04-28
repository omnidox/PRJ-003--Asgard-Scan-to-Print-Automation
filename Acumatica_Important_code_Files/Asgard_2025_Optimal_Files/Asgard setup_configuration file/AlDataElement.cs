using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using AA.Objects.Core;
using Asgard.Labels.Abstractions.Interface;
using MongoDB.Bson.Serialization.Attributes;
using PX.Data;
using PX.Data.BQL;
using PX.Data.EP;
using PX.Data.ReferentialIntegrity.Attributes;

namespace AA.Objects.Labels
{
	// Token: 0x020000F1 RID: 241
	[PXCacheName("Label Data Element")]
	[PXPrimaryGraph(typeof(ALDataElementMaint))]
	[DebuggerDisplay("DataElement: {Name} ({Description})")]
	[Serializable]
	public class ALDataElement : PXBqlTable, IBqlTable, IBqlTableSystemDataStorage, ISortOrder, INotable, ILabelElement, IRenderableConfig, IExprRow, IExpr, IDataDriven, IDesignable, IElementDriven, IContentable, IImageDriven, IBarcodeable, ISubstitutable, IIterable, IArgHolder, IRenderableChild<Guid?>, IRenderableChild, IExportable, IAcuScreenBased
	{
		// Token: 0x17000213 RID: 531
		// (get) Token: 0x06000637 RID: 1591 RVA: 0x0001C0A1 File Offset: 0x0001A2A1
		// (set) Token: 0x06000638 RID: 1592 RVA: 0x0001C0A9 File Offset: 0x0001A2A9
		[PXBool]
		[PXUnboundDefault(false, PersistingCheck = 1)]
		[PXUIField(DisplayName = "Selected")]
		public virtual bool? Selected { get; set; }

		// Token: 0x17000214 RID: 532
		// (get) Token: 0x06000639 RID: 1593 RVA: 0x0001C0B2 File Offset: 0x0001A2B2
		// (set) Token: 0x0600063A RID: 1594 RVA: 0x0001C0BA File Offset: 0x0001A2BA
		public virtual Guid? LabelElementID
		{
			get
			{
				return this.RecordID;
			}
			set
			{
				this.RecordID = value;
			}
		}

		// Token: 0x17000215 RID: 533
		// (get) Token: 0x0600063B RID: 1595 RVA: 0x0001C0B2 File Offset: 0x0001A2B2
		[BsonElement]
		public Guid? ID
		{
			get
			{
				return this.RecordID;
			}
		}

		// Token: 0x17000216 RID: 534
		// (get) Token: 0x0600063C RID: 1596 RVA: 0x0001C0C4 File Offset: 0x0001A2C4
		[BsonElement]
		public Guid? ParentID
		{
			get
			{
				return this.SourceID;
			}
		}

		// Token: 0x17000217 RID: 535
		// (get) Token: 0x0600063D RID: 1597 RVA: 0x0001C0B2 File Offset: 0x0001A2B2
		[BsonElement]
		public Guid? ChildID
		{
			get
			{
				return this.RecordID;
			}
		}

		// Token: 0x17000218 RID: 536
		// (get) Token: 0x0600063E RID: 1598 RVA: 0x0001C0CC File Offset: 0x0001A2CC
		// (set) Token: 0x0600063F RID: 1599 RVA: 0x0001C0D4 File Offset: 0x0001A2D4
		[ALGuidID(IsKey = true)]
		public virtual Guid? RecordID { get; set; }

		// Token: 0x17000219 RID: 537
		// (get) Token: 0x06000640 RID: 1600 RVA: 0x0001C0DD File Offset: 0x0001A2DD
		// (set) Token: 0x06000641 RID: 1601 RVA: 0x0001C0E5 File Offset: 0x0001A2E5
		[ALActive]
		public virtual bool? Active { get; set; }

		// Token: 0x1700021A RID: 538
		// (get) Token: 0x06000642 RID: 1602 RVA: 0x0001C0EE File Offset: 0x0001A2EE
		// (set) Token: 0x06000643 RID: 1603 RVA: 0x0001C0F6 File Offset: 0x0001A2F6
		[ALSystem]
		public virtual bool? IsSystem { get; set; }

		// Token: 0x1700021B RID: 539
		// (get) Token: 0x06000644 RID: 1604 RVA: 0x0001C0FF File Offset: 0x0001A2FF
		// (set) Token: 0x06000645 RID: 1605 RVA: 0x0001C107 File Offset: 0x0001A307
		[PXDBBool]
		[PXDefault(true, PersistingCheck = 2)]
		[PXUIField(DisplayName = "Gen. Name", Visibility = 7)]
		public virtual bool? GenName { get; set; }

		// Token: 0x1700021C RID: 540
		// (get) Token: 0x06000646 RID: 1606 RVA: 0x0001C110 File Offset: 0x0001A310
		public virtual string ExprCode
		{
			get
			{
				return this.Name;
			}
		}

		// Token: 0x1700021D RID: 541
		// (get) Token: 0x06000647 RID: 1607 RVA: 0x0001C118 File Offset: 0x0001A318
		// (set) Token: 0x06000648 RID: 1608 RVA: 0x0001C120 File Offset: 0x0001A320
		[ALDataElementName(typeof(ALDataElement.name), 100)]
		public virtual string Name { get; set; }

		// Token: 0x1700021E RID: 542
		// (get) Token: 0x06000649 RID: 1609 RVA: 0x0001C129 File Offset: 0x0001A329
		// (set) Token: 0x0600064A RID: 1610 RVA: 0x0001C131 File Offset: 0x0001A331
		[ALDataSourceIDForeign(Visible = false)]
		[PXParent(typeof(ALDataElement.FK.Parent))]
		[PXDefault(typeof(ALDataSource.sourceID))]
		public virtual Guid? SourceID { get; set; }

		// Token: 0x1700021F RID: 543
		// (get) Token: 0x0600064B RID: 1611 RVA: 0x0001C13A File Offset: 0x0001A33A
		// (set) Token: 0x0600064C RID: 1612 RVA: 0x0001C142 File Offset: 0x0001A342
		[PXString]
		[PXFormula(typeof(Selector<ALDataElement.sourceID, ALDataSource.name>))]
		[PXUnboundDefault(typeof(Search<ALDataSource.name, Where<ALDataSource.sourceID, Equal<Current<ALDataElement.sourceID>>>>), SearchOnDefault = true)]
		public string SourceIDName { get; set; }

		// Token: 0x17000220 RID: 544
		// (get) Token: 0x0600064D RID: 1613 RVA: 0x0001C14B File Offset: 0x0001A34B
		public string SchemaID
		{
			get
			{
				return this.ScreenID;
			}
		}

		// Token: 0x17000221 RID: 545
		// (get) Token: 0x0600064E RID: 1614 RVA: 0x0001C153 File Offset: 0x0001A353
		// (set) Token: 0x0600064F RID: 1615 RVA: 0x0001C15B File Offset: 0x0001A35B
		[PXString(8, IsFixed = true, InputMask = "CC.CC.CC.CC")]
		[PXSiteMapNodeSelector]
		[PXUIField(DisplayName = "Screen", Visibility = 7)]
		[PXFormula(typeof(Selector<ALDataElement.sourceID, ALDataSource.screenID>))]
		[PXUnboundDefault(typeof(Search<ALDataSource.screenID, Where<ALDataSource.sourceID, Equal<Current<ALDataElement.sourceID>>>>), SearchOnDefault = true)]
		public string ScreenID { get; set; }

		// Token: 0x17000222 RID: 546
		// (get) Token: 0x06000650 RID: 1616 RVA: 0x0001C164 File Offset: 0x0001A364
		// (set) Token: 0x06000651 RID: 1617 RVA: 0x0001C16C File Offset: 0x0001A36C
		[PXString(256, IsUnicode = true)]
		[PXFormula(typeof(Selector<ALDataElement.sourceID, ALDataSource.graphType>))]
		[PXUnboundDefault(typeof(Search<ALDataSource.graphType, Where<ALDataSource.sourceID, Equal<Current<ALDataElement.sourceID>>>>), SearchOnDefault = true)]
		public virtual string GraphType { get; set; }

		// Token: 0x17000223 RID: 547
		// (get) Token: 0x06000652 RID: 1618 RVA: 0x0001C175 File Offset: 0x0001A375
		// (set) Token: 0x06000653 RID: 1619 RVA: 0x0001C17D File Offset: 0x0001A37D
		[PXDBInt]
		[PXUIField(DisplayName = "Line Nbr.", Visible = false)]
		[PXLineNbr(typeof(ALDataSource))]
		public virtual int? LineNbr { get; set; }

		// Token: 0x17000224 RID: 548
		// (get) Token: 0x06000654 RID: 1620 RVA: 0x0001C186 File Offset: 0x0001A386
		// (set) Token: 0x06000655 RID: 1621 RVA: 0x0001C18E File Offset: 0x0001A38E
		[PXDBInt]
		[PXUIField(DisplayName = "Line Order", Visible = false, Enabled = false)]
		public virtual int? SortOrder { get; set; }

		// Token: 0x17000225 RID: 549
		// (get) Token: 0x06000656 RID: 1622 RVA: 0x0001C110 File Offset: 0x0001A310
		public string Code
		{
			get
			{
				return this.Name;
			}
		}

		// Token: 0x17000226 RID: 550
		// (get) Token: 0x06000657 RID: 1623 RVA: 0x0001C197 File Offset: 0x0001A397
		public Guid? HolderSubstitutionID
		{
			get
			{
				return this.SubstitutionID;
			}
		}

		// Token: 0x17000227 RID: 551
		// (get) Token: 0x06000658 RID: 1624 RVA: 0x0001C1A0 File Offset: 0x0001A3A0
		public Guid? DataElementID
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000228 RID: 552
		// (get) Token: 0x06000659 RID: 1625 RVA: 0x0001C1B6 File Offset: 0x0001A3B6
		// (set) Token: 0x0600065A RID: 1626 RVA: 0x0001C1BE File Offset: 0x0001A3BE
		public virtual bool? AllowExport
		{
			get
			{
				return this.Active;
			}
			set
			{
			}
		}

		// Token: 0x17000229 RID: 553
		// (get) Token: 0x0600065B RID: 1627 RVA: 0x0001C1C2 File Offset: 0x0001A3C2
		// (set) Token: 0x0600065C RID: 1628 RVA: 0x0001C1CA File Offset: 0x0001A3CA
		[ALCategoryIDForeign]
		[PXDefault(typeof(ALSetup.defaultCategoryID), PersistingCheck = 2)]
		[PXForeignReference(typeof(ALDataElement.FK.Category))]
		public virtual Guid? CategoryID { get; set; }

		// Token: 0x1700022A RID: 554
		// (get) Token: 0x0600065D RID: 1629 RVA: 0x0001C1D3 File Offset: 0x0001A3D3
		// (set) Token: 0x0600065E RID: 1630 RVA: 0x0001C1DB File Offset: 0x0001A3DB
		[PXString]
		[PXFormula(typeof(Selector<ALDataElement.categoryID, ALCategory.name>))]
		public string CategoryIDName { get; set; }

		// Token: 0x1700022B RID: 555
		// (get) Token: 0x0600065F RID: 1631 RVA: 0x0001C1E4 File Offset: 0x0001A3E4
		// (set) Token: 0x06000660 RID: 1632 RVA: 0x0001C1EC File Offset: 0x0001A3EC
		[ExprType]
		[ALExprType.ALListAttribute]
		[PXDefault]
		public virtual string ExprType { get; set; }

		// Token: 0x1700022C RID: 556
		// (get) Token: 0x06000661 RID: 1633 RVA: 0x0001C1F5 File Offset: 0x0001A3F5
		// (set) Token: 0x06000662 RID: 1634 RVA: 0x0001C1FD File Offset: 0x0001A3FD
		[ALBasedOn]
		[PXUIVisible(typeof(Where<ALDataElement.exprType, In3<ALExprType.function, ALExprType.screen>>))]
		[PXUIEnabled(typeof(Where<ALDataElement.exprType, In3<ALExprType.function, ALExprType.screen>>))]
		[PXUIRequired(typeof(Where<ALDataElement.exprType, In3<ALExprType.function, ALExprType.screen>>))]
		public virtual string BasedOn { get; set; }

		// Token: 0x1700022D RID: 557
		// (get) Token: 0x06000663 RID: 1635 RVA: 0x0001C206 File Offset: 0x0001A406
		// (set) Token: 0x06000664 RID: 1636 RVA: 0x0001C20E File Offset: 0x0001A40E
		[ALExprValue]
		[PXUIVisible(typeof(Where<ALDataElement.exprType, NotIn3<ALExprType.image, ALExprType.content, ALExprType.iterator>>))]
		[PXUIEnabled(typeof(Where<ALDataElement.exprType, NotIn3<ALExprType.image, ALExprType.content, ALExprType.iterator>>))]
		[PXUIRequired(typeof(Where<ALDataElement.basedOn, IsNotNull>))]
		public virtual string ExprValue { get; set; }

		// Token: 0x1700022E RID: 558
		// (get) Token: 0x06000665 RID: 1637 RVA: 0x0001C217 File Offset: 0x0001A417
		// (set) Token: 0x06000666 RID: 1638 RVA: 0x0001C21F File Offset: 0x0001A41F
		[ALDataElementArg]
		public virtual string Arg1 { get; set; }

		// Token: 0x1700022F RID: 559
		// (get) Token: 0x06000667 RID: 1639 RVA: 0x0001C228 File Offset: 0x0001A428
		// (set) Token: 0x06000668 RID: 1640 RVA: 0x0001C230 File Offset: 0x0001A430
		[ALDataElementArg]
		public virtual string Arg2 { get; set; }

		// Token: 0x17000230 RID: 560
		// (get) Token: 0x06000669 RID: 1641 RVA: 0x0001C239 File Offset: 0x0001A439
		// (set) Token: 0x0600066A RID: 1642 RVA: 0x0001C241 File Offset: 0x0001A441
		[ALDataElementArg]
		public virtual string Arg3 { get; set; }

		// Token: 0x17000231 RID: 561
		// (get) Token: 0x0600066B RID: 1643 RVA: 0x0001C24A File Offset: 0x0001A44A
		// (set) Token: 0x0600066C RID: 1644 RVA: 0x0001C252 File Offset: 0x0001A452
		[ALDataElementArg]
		public virtual string Arg4 { get; set; }

		// Token: 0x17000232 RID: 562
		// (get) Token: 0x0600066D RID: 1645 RVA: 0x0001C25B File Offset: 0x0001A45B
		// (set) Token: 0x0600066E RID: 1646 RVA: 0x0001C263 File Offset: 0x0001A463
		[ALDataElementArg]
		public virtual string Arg5 { get; set; }

		// Token: 0x17000233 RID: 563
		// (get) Token: 0x0600066F RID: 1647 RVA: 0x0001C26C File Offset: 0x0001A46C
		// (set) Token: 0x06000670 RID: 1648 RVA: 0x0001C274 File Offset: 0x0001A474
		[ALDataElementArg]
		public virtual string Arg6 { get; set; }

		// Token: 0x17000234 RID: 564
		// (get) Token: 0x06000671 RID: 1649 RVA: 0x0001C27D File Offset: 0x0001A47D
		// (set) Token: 0x06000672 RID: 1650 RVA: 0x0001C285 File Offset: 0x0001A485
		[ALDynamicUnboundArgName]
		public string ArgName1 { get; set; }

		// Token: 0x17000235 RID: 565
		// (get) Token: 0x06000673 RID: 1651 RVA: 0x0001C28E File Offset: 0x0001A48E
		// (set) Token: 0x06000674 RID: 1652 RVA: 0x0001C296 File Offset: 0x0001A496
		[ALDynamicUnboundArgName]
		public string ArgName2 { get; set; }

		// Token: 0x17000236 RID: 566
		// (get) Token: 0x06000675 RID: 1653 RVA: 0x0001C29F File Offset: 0x0001A49F
		// (set) Token: 0x06000676 RID: 1654 RVA: 0x0001C2A7 File Offset: 0x0001A4A7
		[ALDynamicUnboundArgName]
		public string ArgName3 { get; set; }

		// Token: 0x17000237 RID: 567
		// (get) Token: 0x06000677 RID: 1655 RVA: 0x0001C2B0 File Offset: 0x0001A4B0
		// (set) Token: 0x06000678 RID: 1656 RVA: 0x0001C2B8 File Offset: 0x0001A4B8
		[ALDynamicUnboundArgName]
		public string ArgName4 { get; set; }

		// Token: 0x17000238 RID: 568
		// (get) Token: 0x06000679 RID: 1657 RVA: 0x0001C2C1 File Offset: 0x0001A4C1
		// (set) Token: 0x0600067A RID: 1658 RVA: 0x0001C2C9 File Offset: 0x0001A4C9
		[ALDynamicUnboundArgName]
		public string ArgName5 { get; set; }

		// Token: 0x17000239 RID: 569
		// (get) Token: 0x0600067B RID: 1659 RVA: 0x0001C2D2 File Offset: 0x0001A4D2
		// (set) Token: 0x0600067C RID: 1660 RVA: 0x0001C2DA File Offset: 0x0001A4DA
		[ALDynamicUnboundArgName]
		public string ArgName6 { get; set; }

		// Token: 0x1700023A RID: 570
		// (get) Token: 0x0600067D RID: 1661 RVA: 0x0001C2E3 File Offset: 0x0001A4E3
		// (set) Token: 0x0600067E RID: 1662 RVA: 0x0001C2EB File Offset: 0x0001A4EB
		[ALSampleType]
		[PXDefault(typeof(ALExprType.same), PersistingCheck = 2)]
		[PXUIVisible(typeof(Where<ALDataElement.exprType, NotIn3<ALExprType.image, ALExprType.content, ALExprType.iterator>>))]
		[PXUIEnabled(typeof(Where<ALDataElement.exprType, NotIn3<ALExprType.image, ALExprType.content, ALExprType.iterator, ALExprType.hard>>))]
		[PXUIRequired(typeof(Where<ALDataElement.exprType, NotIn3<ALExprType.image, ALExprType.content, ALExprType.iterator>>))]
		public virtual string SampleType { get; set; }

		// Token: 0x1700023B RID: 571
		// (get) Token: 0x0600067F RID: 1663 RVA: 0x0001C2F4 File Offset: 0x0001A4F4
		// (set) Token: 0x06000680 RID: 1664 RVA: 0x0001C2FC File Offset: 0x0001A4FC
		[ALSampleBasedOn]
		[PXUIVisible(typeof(Where<ALDataElement.exprType, In3<ALExprType.function, ALExprType.screen>, And<ALDataElement.sampleType, NotEqual<ALExprType.same>>>))]
		[PXUIEnabled(typeof(Where<ALDataElement.exprType, In3<ALExprType.function, ALExprType.screen>, And<ALDataElement.sampleType, NotEqual<ALExprType.same>>>))]
		public virtual string SampleBasedOn { get; set; }

		// Token: 0x1700023C RID: 572
		// (get) Token: 0x06000681 RID: 1665 RVA: 0x0001C305 File Offset: 0x0001A505
		// (set) Token: 0x06000682 RID: 1666 RVA: 0x0001C30D File Offset: 0x0001A50D
		[ALSampleValue]
		[PXUIVisible(typeof(Where<ALDataElement.exprType, NotIn3<ALExprType.image, ALExprType.content, ALExprType.iterator>, And<ALDataElement.sampleType, NotEqual<ALExprType.same>>>))]
		[PXUIEnabled(typeof(Where<ALDataElement.exprType, NotIn3<ALExprType.hard, ALExprType.image, ALExprType.content, ALExprType.iterator>, And<ALDataElement.sampleType, NotEqual<ALExprType.same>>>))]
		[PXUIRequired(typeof(Where<ALDataElement.sampleType, In3<ALExprType.screen, ALExprType.function>>))]
		public virtual string SampleValue { get; set; }

		// Token: 0x1700023D RID: 573
		// (get) Token: 0x06000683 RID: 1667 RVA: 0x0001C316 File Offset: 0x0001A516
		// (set) Token: 0x06000684 RID: 1668 RVA: 0x0001C31E File Offset: 0x0001A51E
		[ALContentIDForeign(typeof(Where<ALContent.screenID, Equal<Current<ALDataElement.screenID>>>))]
		[PXForeignReference(typeof(ALDataElement.FK.Content))]
		[PXDefault(PersistingCheck = 2)]
		[PXUIVisible(typeof(Where<ALDataElement.exprType, Equal<ALExprType.content>>))]
		[PXUIEnabled(typeof(Where<ALDataElement.exprType, Equal<ALExprType.content>>))]
		[PXUIRequired(typeof(Where<ALDataElement.exprType, Equal<ALExprType.content>>))]
		public virtual Guid? ContentID { get; set; }

		// Token: 0x1700023E RID: 574
		// (get) Token: 0x06000685 RID: 1669 RVA: 0x0001C327 File Offset: 0x0001A527
		// (set) Token: 0x06000686 RID: 1670 RVA: 0x0001C32F File Offset: 0x0001A52F
		[ALPrinterFileGUIDForeign(typeof(Where<ALPrinterFile.extension, ALContentType.isImage>))]
		[PXForeignReference(typeof(ALDataElement.FK.PrinterFile))]
		[PXDefault(PersistingCheck = 2)]
		[PXUIVisible(typeof(Where<ALDataElement.exprType, Equal<ALExprType.image>>))]
		[PXUIEnabled(typeof(Where<ALDataElement.exprType, Equal<ALExprType.image>>))]
		[PXUIRequired(typeof(Where<ALDataElement.exprType, Equal<ALExprType.image>>))]
		public virtual Guid? PrinterFileGUID { get; set; }

		// Token: 0x1700023F RID: 575
		// (get) Token: 0x06000687 RID: 1671 RVA: 0x0001C338 File Offset: 0x0001A538
		// (set) Token: 0x06000688 RID: 1672 RVA: 0x0001C340 File Offset: 0x0001A540
		[ALModelIDForeign(typeof(Where<ALModel.modelType, Equal<ALModelType.snippet>, And<ALModel.screenID, Equal<Current<ALDataElement.screenID>>>>), DisplayName = "Snippet")]
		[PXDefault(PersistingCheck = 2)]
		[PXUIVisible(typeof(Where<ALDataElement.exprType, Equal<ALExprType.iterator>>))]
		[PXUIEnabled(typeof(Where<ALDataElement.exprType, Equal<ALExprType.iterator>>))]
		[PXUIRequired(typeof(Where<ALDataElement.exprType, Equal<ALExprType.iterator>>))]
		public virtual Guid? SnippetID { get; set; }

		// Token: 0x17000240 RID: 576
		// (get) Token: 0x06000689 RID: 1673 RVA: 0x0001C349 File Offset: 0x0001A549
		// (set) Token: 0x0600068A RID: 1674 RVA: 0x0001C351 File Offset: 0x0001A551
		[PXDBBool]
		[PXDefault(false, PersistingCheck = 2)]
		[PXUIField(DisplayName = "Do Substitute")]
		public virtual bool? DoSubstitute { get; set; }

		// Token: 0x17000241 RID: 577
		// (get) Token: 0x0600068B RID: 1675 RVA: 0x0001C35A File Offset: 0x0001A55A
		// (set) Token: 0x0600068C RID: 1676 RVA: 0x0001C362 File Offset: 0x0001A562
		[ALSubstitutionIDForeign(typeof(Where<ALSubstitution.nbArgs, Greater<Zero>, Or<ALSubstitution.isComposite, Equal<True>>>))]
		[PXForeignReference(typeof(ALDataElement.FK.Substitution))]
		[PXDefault(PersistingCheck = 2)]
		[PXUIEnabled(typeof(Where<ALDataElement.doSubstitute, Equal<True>>))]
		[PXUIRequired(typeof(Where<ALDataElement.doSubstitute, Equal<True>>))]
		public virtual Guid? SubstitutionID { get; set; }

		// Token: 0x17000242 RID: 578
		// (get) Token: 0x0600068D RID: 1677 RVA: 0x0001C36B File Offset: 0x0001A56B
		// (set) Token: 0x0600068E RID: 1678 RVA: 0x0001C373 File Offset: 0x0001A573
		[ALBarcodeIDForeign(typeof(Where<True, Equal<True>>))]
		[PXForeignReference(typeof(ALDataElement.FK.Barcode))]
		[PXUIVisible(typeof(Where<ALDataElement.exprType, NotEqual<ALExprType.image>>))]
		[PXUIEnabled(typeof(Where<ALDataElement.exprType, NotEqual<ALExprType.image>>))]
		public virtual Guid? BarcodeID { get; set; }

		// Token: 0x17000243 RID: 579
		// (get) Token: 0x0600068F RID: 1679 RVA: 0x0001C37C File Offset: 0x0001A57C
		// (set) Token: 0x06000690 RID: 1680 RVA: 0x0001C384 File Offset: 0x0001A584
		[ALExprValue(DisplayName = "Default Value")]
		[PXUIVisible(typeof(Where<ALDataElement.exprType, In3<ALExprType.screen, ALExprType.function, ALExprType.content>>))]
		[PXUIEnabled(typeof(Where<ALDataElement.exprType, In3<ALExprType.screen, ALExprType.function, ALExprType.content>>))]
		public virtual string DefaultValue { get; set; }

		// Token: 0x17000244 RID: 580
		// (get) Token: 0x06000691 RID: 1681 RVA: 0x0001C38D File Offset: 0x0001A58D
		// (set) Token: 0x06000692 RID: 1682 RVA: 0x0001C395 File Offset: 0x0001A595
		[ALDescription(PersistingCheck = 2)]
		[PXFieldDescription]
		public virtual string Description { get; set; }

		// Token: 0x17000245 RID: 581
		// (get) Token: 0x06000693 RID: 1683 RVA: 0x0001C39E File Offset: 0x0001A59E
		// (set) Token: 0x06000694 RID: 1684 RVA: 0x0001C3A6 File Offset: 0x0001A5A6
		[PXNote]
		public virtual Guid? NoteID { get; set; }

		// Token: 0x17000246 RID: 582
		// (get) Token: 0x06000695 RID: 1685 RVA: 0x0001C3AF File Offset: 0x0001A5AF
		// (set) Token: 0x06000696 RID: 1686 RVA: 0x0001C3B7 File Offset: 0x0001A5B7
		[PXDBCreatedByID]
		public virtual Guid? CreatedByID { get; set; }

		// Token: 0x17000247 RID: 583
		// (get) Token: 0x06000697 RID: 1687 RVA: 0x0001C3C0 File Offset: 0x0001A5C0
		// (set) Token: 0x06000698 RID: 1688 RVA: 0x0001C3C8 File Offset: 0x0001A5C8
		[PXDBCreatedByScreenID]
		public virtual string CreatedByScreenID { get; set; }

		// Token: 0x17000248 RID: 584
		// (get) Token: 0x06000699 RID: 1689 RVA: 0x0001C3D1 File Offset: 0x0001A5D1
		// (set) Token: 0x0600069A RID: 1690 RVA: 0x0001C3D9 File Offset: 0x0001A5D9
		[PXDBCreatedDateTime]
		[PXUIField(DisplayName = "Created On", Enabled = false, IsReadOnly = true)]
		public virtual DateTime? CreatedDateTime { get; set; }

		// Token: 0x17000249 RID: 585
		// (get) Token: 0x0600069B RID: 1691 RVA: 0x0001C3E2 File Offset: 0x0001A5E2
		// (set) Token: 0x0600069C RID: 1692 RVA: 0x0001C3EA File Offset: 0x0001A5EA
		[PXDBLastModifiedByID]
		public virtual Guid? LastModifiedByID { get; set; }

		// Token: 0x1700024A RID: 586
		// (get) Token: 0x0600069D RID: 1693 RVA: 0x0001C3F3 File Offset: 0x0001A5F3
		// (set) Token: 0x0600069E RID: 1694 RVA: 0x0001C3FB File Offset: 0x0001A5FB
		[PXDBLastModifiedByScreenID]
		public virtual string LastModifiedByScreenID { get; set; }

		// Token: 0x1700024B RID: 587
		// (get) Token: 0x0600069F RID: 1695 RVA: 0x0001C404 File Offset: 0x0001A604
		// (set) Token: 0x060006A0 RID: 1696 RVA: 0x0001C40C File Offset: 0x0001A60C
		[PXDBLastModifiedDateTime]
		[PXUIField(DisplayName = "Last Modified On", Enabled = false, IsReadOnly = true)]
		public virtual DateTime? LastModifiedDateTime { get; set; }

		// Token: 0x1700024C RID: 588
		// (get) Token: 0x060006A1 RID: 1697 RVA: 0x0001C415 File Offset: 0x0001A615
		// (set) Token: 0x060006A2 RID: 1698 RVA: 0x0001C41D File Offset: 0x0001A61D
		[PXDBTimestamp]
		public virtual byte[] tstamp { get; set; }

		// Token: 0x02000409 RID: 1033
		public class PK : PrimaryKeyOf<ALDataElement>.By<ALDataElement.recordID>
		{
			// Token: 0x06001AA7 RID: 6823 RVA: 0x0005751D File Offset: 0x0005571D
			public static ALDataElement Find(PXGraph graph, Guid? recordID)
			{
				return PrimaryKeyOf<ALDataElement>.By<ALDataElement.recordID>.FindBy(graph, recordID, 0);
			}
		}

		// Token: 0x0200040A RID: 1034
		public static class FK
		{
			// Token: 0x020009CF RID: 2511
			public class Parent : PrimaryKeyOf<ALDataSource>.By<ALDataSource.sourceID>.ForeignKeyOf<ALDataElement>.By<ALDataElement.sourceID>
			{
			}

			// Token: 0x020009D0 RID: 2512
			public class Substitution : PrimaryKeyOf<ALSubstitution>.By<ALSubstitution.substitutionID>.ForeignKeyOf<ALDataElement>.By<ALDataElement.substitutionID>
			{
			}

			// Token: 0x020009D1 RID: 2513
			public class Content : PrimaryKeyOf<ALContent>.By<ALContent.contentID>.ForeignKeyOf<ALDataElement>.By<ALDataElement.contentID>
			{
			}

			// Token: 0x020009D2 RID: 2514
			public class Barcode : PrimaryKeyOf<ALBarcode>.By<ALBarcode.barcodeID>.ForeignKeyOf<ALDataElement>.By<ALDataElement.barcodeID>
			{
			}

			// Token: 0x020009D3 RID: 2515
			public class PrinterFile : PrimaryKeyOf<ALPrinterFile>.By<ALPrinterFile.printerFileGUID>.ForeignKeyOf<ALDataElement>.By<ALDataElement.printerFileGUID>
			{
			}

			// Token: 0x020009D4 RID: 2516
			public class Category : PrimaryKeyOf<ALCategory>.By<ALCategory.categoryID>.ForeignKeyOf<ALDataElement>.By<ALDataElement.categoryID>
			{
			}
		}

		// Token: 0x0200040B RID: 1035
		[Nullable(new byte[]
		{
			0,
			1,
			0
		})]
		public abstract class selected : BqlType<IBqlBool, bool>.Field<ALDataElement.selected>
		{
		}

		// Token: 0x0200040C RID: 1036
		[Nullable(new byte[]
		{
			0,
			1,
			0
		})]
		public abstract class recordID : BqlType<IBqlGuid, Guid>.Field<ALDataElement.recordID>
		{
		}

		// Token: 0x0200040D RID: 1037
		[Nullable(new byte[]
		{
			0,
			1,
			0
		})]
		public abstract class active : BqlType<IBqlBool, bool>.Field<ALDataElement.active>
		{
		}

		// Token: 0x0200040E RID: 1038
		[Nullable(new byte[]
		{
			0,
			1,
			0
		})]
		public abstract class isSystem : BqlType<IBqlBool, bool>.Field<ALDataElement.isSystem>
		{
		}

		// Token: 0x0200040F RID: 1039
		[Nullable(new byte[]
		{
			0,
			1,
			0
		})]
		public abstract class genName : BqlType<IBqlBool, bool>.Field<ALDataElement.genName>
		{
		}

		// Token: 0x02000410 RID: 1040
		[Nullable(new byte[]
		{
			0,
			1,
			1,
			0
		})]
		public abstract class name : BqlType<IBqlString, string>.Field<ALDataElement.name>
		{
		}

		// Token: 0x02000411 RID: 1041
		[Nullable(new byte[]
		{
			0,
			1,
			0
		})]
		public abstract class sourceID : BqlType<IBqlGuid, Guid>.Field<ALDataElement.sourceID>
		{
		}

		// Token: 0x02000412 RID: 1042
		[Nullable(new byte[]
		{
			0,
			1,
			1,
			0
		})]
		public abstract class sourceIDName : BqlType<IBqlString, string>.Field<ALDataElement.sourceIDName>
		{
		}

		// Token: 0x02000413 RID: 1043
		[Nullable(new byte[]
		{
			0,
			1,
			1,
			0
		})]
		public abstract class screenID : BqlType<IBqlString, string>.Field<ALDataElement.screenID>
		{
		}

		// Token: 0x02000414 RID: 1044
		[Nullable(new byte[]
		{
			0,
			1,
			1,
			0
		})]
		public abstract class graphType : BqlType<IBqlString, string>.Field<ALDataElement.graphType>
		{
		}

		// Token: 0x02000415 RID: 1045
		[Nullable(new byte[]
		{
			0,
			1,
			0
		})]
		public abstract class lineNbr : BqlType<IBqlInt, int>.Field<ALDataElement.lineNbr>
		{
		}

		// Token: 0x02000416 RID: 1046
		[Nullable(new byte[]
		{
			0,
			1,
			0
		})]
		public abstract class sortOrder : BqlType<IBqlInt, int>.Field<ALDataElement.sortOrder>
		{
		}

		// Token: 0x02000417 RID: 1047
		[Nullable(new byte[]
		{
			0,
			1,
			1,
			0
		})]
		public abstract class code : BqlType<IBqlString, string>.Field<ALDataElement.code>
		{
		}

		// Token: 0x02000418 RID: 1048
		[Nullable(new byte[]
		{
			0,
			1,
			0
		})]
		public abstract class categoryID : BqlType<IBqlGuid, Guid>.Field<ALDataElement.categoryID>
		{
		}

		// Token: 0x02000419 RID: 1049
		[Nullable(new byte[]
		{
			0,
			1,
			1,
			0
		})]
		public abstract class categoryIDName : BqlType<IBqlString, string>.Field<ALDataElement.categoryIDName>
		{
		}

		// Token: 0x0200041A RID: 1050
		[Nullable(new byte[]
		{
			0,
			1,
			1,
			0
		})]
		public abstract class exprType : BqlType<IBqlString, string>.Field<ALDataElement.exprType>
		{
		}

		// Token: 0x0200041B RID: 1051
		[Nullable(new byte[]
		{
			0,
			1,
			1,
			0
		})]
		public abstract class basedOn : BqlType<IBqlString, string>.Field<ALDataElement.basedOn>
		{
		}

		// Token: 0x0200041C RID: 1052
		[Nullable(new byte[]
		{
			0,
			1,
			1,
			0
		})]
		public abstract class exprValue : BqlType<IBqlString, string>.Field<ALDataElement.exprValue>
		{
		}

		// Token: 0x0200041D RID: 1053
		[Nullable(new byte[]
		{
			0,
			1,
			1,
			0
		})]
		public abstract class arg1 : BqlType<IBqlString, string>.Field<ALDataElement.arg1>
		{
		}

		// Token: 0x0200041E RID: 1054
		[Nullable(new byte[]
		{
			0,
			1,
			1,
			0
		})]
		public abstract class arg2 : BqlType<IBqlString, string>.Field<ALDataElement.arg2>
		{
		}

		// Token: 0x0200041F RID: 1055
		[Nullable(new byte[]
		{
			0,
			1,
			1,
			0
		})]
		public abstract class arg3 : BqlType<IBqlString, string>.Field<ALDataElement.arg3>
		{
		}

		// Token: 0x02000420 RID: 1056
		[Nullable(new byte[]
		{
			0,
			1,
			1,
			0
		})]
		public abstract class arg4 : BqlType<IBqlString, string>.Field<ALDataElement.arg4>
		{
		}

		// Token: 0x02000421 RID: 1057
		[Nullable(new byte[]
		{
			0,
			1,
			1,
			0
		})]
		public abstract class arg5 : BqlType<IBqlString, string>.Field<ALDataElement.arg5>
		{
		}

		// Token: 0x02000422 RID: 1058
		[Nullable(new byte[]
		{
			0,
			1,
			1,
			0
		})]
		public abstract class arg6 : BqlType<IBqlString, string>.Field<ALDataElement.arg6>
		{
		}

		// Token: 0x02000423 RID: 1059
		[Nullable(new byte[]
		{
			0,
			1,
			1,
			0
		})]
		public abstract class argName1 : BqlType<IBqlString, string>.Field<ALDataElement.argName1>
		{
		}

		// Token: 0x02000424 RID: 1060
		[Nullable(new byte[]
		{
			0,
			1,
			1,
			0
		})]
		public abstract class argName2 : BqlType<IBqlString, string>.Field<ALDataElement.argName2>
		{
		}

		// Token: 0x02000425 RID: 1061
		[Nullable(new byte[]
		{
			0,
			1,
			1,
			0
		})]
		public abstract class argName3 : BqlType<IBqlString, string>.Field<ALDataElement.argName3>
		{
		}

		// Token: 0x02000426 RID: 1062
		[Nullable(new byte[]
		{
			0,
			1,
			1,
			0
		})]
		public abstract class argName4 : BqlType<IBqlString, string>.Field<ALDataElement.argName4>
		{
		}

		// Token: 0x02000427 RID: 1063
		[Nullable(new byte[]
		{
			0,
			1,
			1,
			0
		})]
		public abstract class argName5 : BqlType<IBqlString, string>.Field<ALDataElement.argName5>
		{
		}

		// Token: 0x02000428 RID: 1064
		[Nullable(new byte[]
		{
			0,
			1,
			1,
			0
		})]
		public abstract class argName6 : BqlType<IBqlString, string>.Field<ALDataElement.argName6>
		{
		}

		// Token: 0x02000429 RID: 1065
		[Nullable(new byte[]
		{
			0,
			1,
			1,
			0
		})]
		public abstract class sampleType : BqlType<IBqlString, string>.Field<ALDataElement.sampleType>
		{
		}

		// Token: 0x0200042A RID: 1066
		[Nullable(new byte[]
		{
			0,
			1,
			1,
			0
		})]
		public abstract class sampleBasedOn : BqlType<IBqlString, string>.Field<ALDataElement.sampleBasedOn>
		{
		}

		// Token: 0x0200042B RID: 1067
		[Nullable(new byte[]
		{
			0,
			1,
			1,
			0
		})]
		public abstract class sampleValue : BqlType<IBqlString, string>.Field<ALDataElement.sampleValue>
		{
		}

		// Token: 0x0200042C RID: 1068
		[Nullable(new byte[]
		{
			0,
			1,
			0
		})]
		public abstract class contentID : BqlType<IBqlGuid, Guid>.Field<ALDataElement.contentID>
		{
		}

		// Token: 0x0200042D RID: 1069
		[Nullable(new byte[]
		{
			0,
			1,
			0
		})]
		public abstract class printerFileGUID : BqlType<IBqlGuid, Guid>.Field<ALDataElement.printerFileGUID>
		{
		}

		// Token: 0x0200042E RID: 1070
		[Nullable(new byte[]
		{
			0,
			1,
			0
		})]
		public abstract class snippetID : BqlType<IBqlGuid, Guid>.Field<ALDataElement.snippetID>
		{
		}

		// Token: 0x0200042F RID: 1071
		[Nullable(new byte[]
		{
			0,
			1,
			0
		})]
		public abstract class doSubstitute : BqlType<IBqlBool, bool>.Field<ALDataElement.doSubstitute>
		{
		}

		// Token: 0x02000430 RID: 1072
		[Nullable(new byte[]
		{
			0,
			1,
			0
		})]
		public abstract class substitutionID : BqlType<IBqlGuid, Guid>.Field<ALDataElement.substitutionID>
		{
		}

		// Token: 0x02000431 RID: 1073
		[Nullable(new byte[]
		{
			0,
			1,
			0
		})]
		public abstract class barcodeID : BqlType<IBqlGuid, Guid>.Field<ALDataElement.barcodeID>
		{
		}

		// Token: 0x02000432 RID: 1074
		[Nullable(new byte[]
		{
			0,
			1,
			1,
			0
		})]
		public abstract class defaultValue : BqlType<IBqlString, string>.Field<ALDataElement.defaultValue>
		{
		}

		// Token: 0x02000433 RID: 1075
		[Nullable(new byte[]
		{
			0,
			1,
			1,
			0
		})]
		public abstract class description : BqlType<IBqlString, string>.Field<ALDataElement.description>
		{
		}

		// Token: 0x02000434 RID: 1076
		[Nullable(new byte[]
		{
			0,
			1,
			0
		})]
		public abstract class noteID : BqlType<IBqlGuid, Guid>.Field<ALDataElement.noteID>
		{
		}

		// Token: 0x02000435 RID: 1077
		[Nullable(new byte[]
		{
			0,
			1,
			0
		})]
		public abstract class createdByID : BqlType<IBqlGuid, Guid>.Field<ALDataElement.createdByID>
		{
		}

		// Token: 0x02000436 RID: 1078
		[Nullable(new byte[]
		{
			0,
			1,
			1,
			0
		})]
		public abstract class createdByScreenID : BqlType<IBqlString, string>.Field<ALDataElement.createdByScreenID>
		{
		}

		// Token: 0x02000437 RID: 1079
		[Nullable(new byte[]
		{
			0,
			1,
			0
		})]
		public abstract class createdDateTime : BqlType<IBqlDateTime, DateTime>.Field<ALDataElement.createdDateTime>
		{
		}

		// Token: 0x02000438 RID: 1080
		[Nullable(new byte[]
		{
			0,
			1,
			0
		})]
		public abstract class lastModifiedByID : BqlType<IBqlGuid, Guid>.Field<ALDataElement.lastModifiedByID>
		{
		}

		// Token: 0x02000439 RID: 1081
		[Nullable(new byte[]
		{
			0,
			1,
			1,
			0
		})]
		public abstract class lastModifiedByScreenID : BqlType<IBqlString, string>.Field<ALDataElement.lastModifiedByScreenID>
		{
		}

		// Token: 0x0200043A RID: 1082
		[Nullable(new byte[]
		{
			0,
			1,
			0
		})]
		public abstract class lastModifiedDateTime : BqlType<IBqlDateTime, DateTime>.Field<ALDataElement.lastModifiedDateTime>
		{
		}

		// Token: 0x0200043B RID: 1083
		[Nullable(new byte[]
		{
			0,
			1,
			1,
			0
		})]
		public abstract class Tstamp : BqlType<IBqlByteArray, byte[]>.Field<ALDataElement.Tstamp>
		{
		}
	}
}
