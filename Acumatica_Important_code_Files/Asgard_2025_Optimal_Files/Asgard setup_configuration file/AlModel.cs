using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using AA.Objects.Core;
using AA.Objects.Labels.LabelZoom;
using Asgard.Labels.Abstractions.Interface;
using Asgard.Labels.Abstractions.Poco;
using MongoDB.Bson.Serialization.Attributes;
using PX.Data;
using PX.Data.BQL;
using PX.Data.EP;
using PX.Data.ReferentialIntegrity.Attributes;
using PX.Objects.CS;
using PX.SM;

namespace AA.Objects.Labels
{
	// Token: 0x020000FC RID: 252
	[PXCacheName("Label Model")]
	[PXPrimaryGraph(typeof(ALModelMaint))]
	[DebuggerDisplay("{Name} ({Description})")]
	[Serializable]
	public class ALModel : PXBqlTable, IBqlTable, IBqlTableSystemDataStorage, INotable, IExportable, IRenderableConfig, IImageStore, ISentByEvent, IModel, ILanguageDriven, IAcuScreenBased
	{
		// Token: 0x1700030C RID: 780
		// (get) Token: 0x06000817 RID: 2071 RVA: 0x0001D16E File Offset: 0x0001B36E
		// (set) Token: 0x06000818 RID: 2072 RVA: 0x0001D176 File Offset: 0x0001B376
		[ALGuidID]
		public virtual Guid? LabelID { get; set; }

		// Token: 0x1700030D RID: 781
		// (get) Token: 0x06000819 RID: 2073 RVA: 0x0001D17F File Offset: 0x0001B37F
		public Guid? ModelID
		{
			get
			{
				return this.LabelID;
			}
		}

		// Token: 0x1700030E RID: 782
		// (get) Token: 0x0600081A RID: 2074 RVA: 0x0001D187 File Offset: 0x0001B387
		public string BasedOnSchema
		{
			get
			{
				return this.BasedOnView;
			}
		}

		// Token: 0x1700030F RID: 783
		// (get) Token: 0x0600081B RID: 2075 RVA: 0x0001D18F File Offset: 0x0001B38F
		// (set) Token: 0x0600081C RID: 2076 RVA: 0x0001D197 File Offset: 0x0001B397
		public int? ZplEncoding
		{
			get
			{
				return this.Encoding;
			}
			set
			{
				this.Encoding = value;
			}
		}

		// Token: 0x17000310 RID: 784
		// (get) Token: 0x0600081D RID: 2077 RVA: 0x0001D17F File Offset: 0x0001B37F
		[BsonElement]
		public Guid? ID
		{
			get
			{
				return this.LabelID;
			}
		}

		// Token: 0x17000311 RID: 785
		// (get) Token: 0x0600081E RID: 2078 RVA: 0x0001D1A1 File Offset: 0x0001B3A1
		// (set) Token: 0x0600081F RID: 2079 RVA: 0x0001D1A9 File Offset: 0x0001B3A9
		[ALActive]
		public virtual bool? Active { get; set; }

		// Token: 0x17000312 RID: 786
		// (get) Token: 0x06000820 RID: 2080 RVA: 0x0001D1B2 File Offset: 0x0001B3B2
		// (set) Token: 0x06000821 RID: 2081 RVA: 0x0001D1BA File Offset: 0x0001B3BA
		[ALSystem]
		public virtual bool? IsSystem { get; set; }

		// Token: 0x17000313 RID: 787
		// (get) Token: 0x06000822 RID: 2082 RVA: 0x0001D1C3 File Offset: 0x0001B3C3
		// (set) Token: 0x06000823 RID: 2083 RVA: 0x0001D1CB File Offset: 0x0001B3CB
		[ALExport]
		public virtual bool? AllowExport { get; set; }

		// Token: 0x17000314 RID: 788
		// (get) Token: 0x06000824 RID: 2084 RVA: 0x0001D1D4 File Offset: 0x0001B3D4
		// (set) Token: 0x06000825 RID: 2085 RVA: 0x0001D1DC File Offset: 0x0001B3DC
		[ALName(typeof(ALModel.name), typeof(ALModel.description), 50, true, IsKey = true, DisplayName = "Model ID")]
		public virtual string Name { get; set; }

		// Token: 0x17000315 RID: 789
		// (get) Token: 0x06000826 RID: 2086 RVA: 0x0001D1E5 File Offset: 0x0001B3E5
		// (set) Token: 0x06000827 RID: 2087 RVA: 0x0001D1ED File Offset: 0x0001B3ED
		[ALDescription]
		[PXFieldDescription]
		public virtual string Description { get; set; }

		// Token: 0x17000316 RID: 790
		// (get) Token: 0x06000828 RID: 2088 RVA: 0x0001D1F6 File Offset: 0x0001B3F6
		// (set) Token: 0x06000829 RID: 2089 RVA: 0x0001D1FE File Offset: 0x0001B3FE
		[PXDBString(1, IsFixed = true)]
		[PXDefault("S")]
		[PXUIField(DisplayName = "Model Type", Visibility = 7)]
		[ALModelType.ALListAttribute]
		public virtual string ModelType { get; set; }

		// Token: 0x17000317 RID: 791
		// (get) Token: 0x0600082A RID: 2090 RVA: 0x0001D207 File Offset: 0x0001B407
		// (set) Token: 0x0600082B RID: 2091 RVA: 0x0001D20F File Offset: 0x0001B40F
		[ALCategoryIDForeign]
		[PXDefault(typeof(ALSetup.defaultCategoryID), PersistingCheck = 2)]
		[PXForeignReference(typeof(ALModel.FK.Category))]
		public virtual Guid? CategoryID { get; set; }

		// Token: 0x17000318 RID: 792
		// (get) Token: 0x0600082C RID: 2092 RVA: 0x0001D218 File Offset: 0x0001B418
		// (set) Token: 0x0600082D RID: 2093 RVA: 0x0001ABA3 File Offset: 0x00018DA3
		[BsonIgnore]
		string ISentByEvent.SubscriberType
		{
			get
			{
				return "ALLP";
			}
			set
			{
			}
		}

		// Token: 0x0600082E RID: 2094 RVA: 0x0001D17F File Offset: 0x0001B37F
		Guid? ISentByEvent.GetHandlerId()
		{
			return this.LabelID;
		}

		// Token: 0x17000319 RID: 793
		// (get) Token: 0x0600082F RID: 2095 RVA: 0x0001D21F File Offset: 0x0001B41F
		// (set) Token: 0x06000830 RID: 2096 RVA: 0x0001D227 File Offset: 0x0001B427
		[ALScreenID]
		[PXForeignReference(typeof(ALModel.FK.PortalMap))]
		[PXForeignReference(typeof(ALModel.FK.SiteMap))]
		[PXUIVisible(typeof(Where<True, Equal<True>>))]
		[PXUIEnabled(typeof(Where<True, Equal<True>>))]
		[PXUIRequired(typeof(Where<True, Equal<True>>))]
		public virtual string ScreenID { get; set; }

		// Token: 0x1700031A RID: 794
		// (get) Token: 0x06000831 RID: 2097 RVA: 0x0001D230 File Offset: 0x0001B430
		// (set) Token: 0x06000832 RID: 2098 RVA: 0x0001D238 File Offset: 0x0001B438
		[ALGraphType(typeof(ALModel.screenID))]
		[PXDefault]
		[PXUIVisible(typeof(Where<True, Equal<True>>))]
		[PXUIEnabled(typeof(Where<True, Equal<True>>))]
		[PXUIRequired(typeof(Where<True, Equal<True>>))]
		public virtual string GraphType { get; set; }

		// Token: 0x1700031B RID: 795
		// (get) Token: 0x06000833 RID: 2099 RVA: 0x0001D241 File Offset: 0x0001B441
		// (set) Token: 0x06000834 RID: 2100 RVA: 0x0001D249 File Offset: 0x0001B449
		[ALViewField(typeof(ALModel.screenID), DisplayName = "Based On View")]
		[ALViewSelector(typeof(ALModel.graphType), ValidateValue = false)]
		[PXUIVisible(typeof(Where<ALModel.modelType, In3<ALModelType.single, ALModelType.snippet>>))]
		[PXUIEnabled(typeof(Where<ALModel.modelType, In3<ALModelType.single, ALModelType.snippet>>))]
		[PXUIRequired(typeof(Where<ALModel.modelType, In3<ALModelType.single, ALModelType.snippet>>))]
		public virtual string BasedOnView { get; set; }

		// Token: 0x1700031C RID: 796
		// (get) Token: 0x06000835 RID: 2101 RVA: 0x0001D252 File Offset: 0x0001B452
		// (set) Token: 0x06000836 RID: 2102 RVA: 0x0001D25A File Offset: 0x0001B45A
		[ALCloudLabelIDStandalone(typeof(ALModel.modelType))]
		[PXUIVisible(typeof(Where<ALModel.modelType, Equal<ALModelType.labelZoom>>))]
		[PXUIEnabled(typeof(Where<ALModel.modelType, Equal<ALModelType.labelZoom>>))]
		[PXUIRequired(typeof(Where<ALModel.modelType, Equal<ALModelType.labelZoom>>))]
		public virtual string CloudID { get; set; }

		// Token: 0x1700031D RID: 797
		// (get) Token: 0x06000837 RID: 2103 RVA: 0x0001D263 File Offset: 0x0001B463
		// (set) Token: 0x06000838 RID: 2104 RVA: 0x0001D26B File Offset: 0x0001B46B
		[ALFormatIDForeign(typeof(Where<True, Equal<True>>))]
		[PXForeignReference(typeof(ALModel.FK.Format))]
		[PXDefault(typeof(ALSetup.defaultFormatID), PersistingCheck = 2)]
		[PXUIVisible(typeof(Where<ALModel.modelType, In3<ALModelType.single, ALModelType.snippet, ALModelType.labelZoom>>))]
		[PXUIEnabled(typeof(Where<ALModel.modelType, In3<ALModelType.single, ALModelType.snippet>>))]
		[PXUIRequired(typeof(Where<ALModel.modelType, Equal<ALModelType.single>>))]
		public virtual Guid? FormatID { get; set; }

		// Token: 0x1700031E RID: 798
		// (get) Token: 0x06000839 RID: 2105 RVA: 0x0001D274 File Offset: 0x0001B474
		// (set) Token: 0x0600083A RID: 2106 RVA: 0x0001D27C File Offset: 0x0001B47C
		[ALMarginIDForeign]
		[PXForeignReference(typeof(ALModel.FK.Margin))]
		[PXDefault(typeof(ALSetup.defaultMarginID), PersistingCheck = 2)]
		[PXUIVisible(typeof(Where<ALModel.modelType, In3<ALModelType.single, ALModelType.snippet, ALModelType.labelZoom>>))]
		[PXUIEnabled(typeof(Where<ALModel.modelType, In3<ALModelType.single, ALModelType.snippet>>))]
		public virtual Guid? MarginID { get; set; }

		// Token: 0x1700031F RID: 799
		// (get) Token: 0x0600083B RID: 2107 RVA: 0x0001D285 File Offset: 0x0001B485
		// (set) Token: 0x0600083C RID: 2108 RVA: 0x0001D28D File Offset: 0x0001B48D
		[PXDBString(255, IsUnicode = true)]
		[PXUIField(DisplayName = "Image", Visible = false)]
		[BsonIgnore]
		public string ImageUrl { get; set; }

		// Token: 0x17000320 RID: 800
		// (get) Token: 0x0600083D RID: 2109 RVA: 0x0001D298 File Offset: 0x0001B498
		[PXBool]
		[PXUIField(Visibility = 3)]
		public virtual bool? ShowTemplate
		{
			[PXDependsOnFields(new Type[]
			{
				typeof(ALModel.modelType)
			})]
			get
			{
				return new bool?(AAConstants.ModelType.IsReal(this.ModelType) && ALSetupSlot.ShowTemplate);
			}
		}

		// Token: 0x17000321 RID: 801
		// (get) Token: 0x0600083E RID: 2110 RVA: 0x0001D2C4 File Offset: 0x0001B4C4
		[PXBool]
		[PXUIField(Visibility = 3)]
		public virtual bool? ShowExprs
		{
			[PXDependsOnFields(new Type[]
			{
				typeof(ALModel.modelType)
			})]
			get
			{
				return new bool?(AAConstants.ModelType.IsReal(this.ModelType) || this.ModelType == "N");
			}
		}

		// Token: 0x17000322 RID: 802
		// (get) Token: 0x0600083F RID: 2111 RVA: 0x0001D2FC File Offset: 0x0001B4FC
		[PXBool]
		[PXUIField(Visibility = 3)]
		public virtual bool? ShowRendered
		{
			[PXDependsOnFields(new Type[]
			{
				typeof(ALModel.modelType)
			})]
			get
			{
				return new bool?(this.ShowExprs.GetValueOrDefault() && ALSetupSlot.DevMode && ALSetupSlot.SaveRendered);
			}
		}

		// Token: 0x17000323 RID: 803
		// (get) Token: 0x06000840 RID: 2112 RVA: 0x0001D334 File Offset: 0x0001B534
		[PXBool]
		[PXUIField(Visibility = 3)]
		public virtual bool? ShowSetup
		{
			[PXDependsOnFields(new Type[]
			{
				typeof(ALModel.modelType)
			})]
			get
			{
				return new bool?(true);
			}
		}

		// Token: 0x17000324 RID: 804
		// (get) Token: 0x06000841 RID: 2113 RVA: 0x0001D34C File Offset: 0x0001B54C
		[PXBool]
		[PXUIField(Visibility = 3)]
		public virtual bool? ShowPrinters
		{
			[PXDependsOnFields(new Type[]
			{
				typeof(ALModel.modelType)
			})]
			get
			{
				return new bool?(AAConstants.ModelType.IsReal(this.ModelType) || this.ModelType == "P");
			}
		}

		// Token: 0x17000325 RID: 805
		// (get) Token: 0x06000842 RID: 2114 RVA: 0x0001D384 File Offset: 0x0001B584
		[PXBool]
		[PXUIField(Visibility = 3)]
		public virtual bool? ShowPrintLog
		{
			[PXDependsOnFields(new Type[]
			{
				typeof(ALModel.modelType)
			})]
			get
			{
				return new bool?(AAConstants.ModelType.IsReal(this.ModelType));
			}
		}

		// Token: 0x17000326 RID: 806
		// (get) Token: 0x06000843 RID: 2115 RVA: 0x0001D3A8 File Offset: 0x0001B5A8
		[PXBool]
		[PXUIField(Visibility = 3)]
		public virtual bool? ShowChildren
		{
			[PXDependsOnFields(new Type[]
			{
				typeof(ALModel.modelType)
			})]
			get
			{
				return new bool?(this.ModelType == "G");
			}
		}

		// Token: 0x17000327 RID: 807
		// (get) Token: 0x06000844 RID: 2116 RVA: 0x0001D3D0 File Offset: 0x0001B5D0
		[PXBool]
		[PXUIField(Visibility = 3)]
		public virtual bool? ShowUsedBy
		{
			[PXDependsOnFields(new Type[]
			{
				typeof(ALModel.modelType)
			})]
			get
			{
				return new bool?(this.ModelType == "N");
			}
		}

		// Token: 0x17000328 RID: 808
		// (get) Token: 0x06000845 RID: 2117 RVA: 0x0001D3F7 File Offset: 0x0001B5F7
		// (set) Token: 0x06000846 RID: 2118 RVA: 0x0001D3FF File Offset: 0x0001B5FF
		[PXDBBool]
		[PXDefault(true, PersistingCheck = 2)]
		[PXUIField(DisplayName = "Hide When In Group")]
		[PXUIVisible(typeof(Where<ALModel.modelType, Equal<ALModelType.single>>))]
		[PXUIEnabled(typeof(Where<ALModel.modelType, Equal<ALModelType.single>>))]
		public virtual bool? HideWhenInGroup { get; set; }

		// Token: 0x17000329 RID: 809
		// (get) Token: 0x06000847 RID: 2119 RVA: 0x0001D408 File Offset: 0x0001B608
		// (set) Token: 0x06000848 RID: 2120 RVA: 0x0001D410 File Offset: 0x0001B610
		[PXDBBool]
		[PXDefault(true, PersistingCheck = 2)]
		[PXUIField(DisplayName = "Ignore Rotation On Render")]
		[PXUIVisible(typeof(Where<ALModel.modelType, Equal<ALModelType.single>>))]
		[PXUIEnabled(typeof(Where<ALModel.modelType, Equal<ALModelType.single>>))]
		public virtual bool? IgnoreRotationOnRender { get; set; }

		// Token: 0x1700032A RID: 810
		// (get) Token: 0x06000849 RID: 2121 RVA: 0x0001D419 File Offset: 0x0001B619
		// (set) Token: 0x0600084A RID: 2122 RVA: 0x0001D421 File Offset: 0x0001B621
		[PXDBText(IsUnicode = true)]
		[PXUIField(DisplayName = "Tooltip", Visibility = 7)]
		[PXUIVisible(typeof(Where<ALModel.modelType, NotIn3<ALModelType.snippet, ALModelType.printerSetup>>))]
		[PXUIEnabled(typeof(Where<ALModel.modelType, NotIn3<ALModelType.snippet, ALModelType.printerSetup>>))]
		public virtual string Tooltip { get; set; }

		// Token: 0x1700032B RID: 811
		// (get) Token: 0x0600084B RID: 2123 RVA: 0x0001D42A File Offset: 0x0001B62A
		// (set) Token: 0x0600084C RID: 2124 RVA: 0x0001D432 File Offset: 0x0001B632
		[PXDBBool]
		[PXDefault(false, PersistingCheck = 2)]
		[PXUIField(DisplayName = "Not", Visibility = 7)]
		[PXUIVisible(typeof(Where<ALModel.modelType, In3<ALModelType.single, ALModelType.labelZoom>>))]
		[PXUIEnabled(typeof(Where<ALModel.modelType, In3<ALModelType.single, ALModelType.labelZoom>, And<ALModel.screenID, IsNotNull>>))]
		public virtual bool? ReverseFilter { get; set; }

		// Token: 0x1700032C RID: 812
		// (get) Token: 0x0600084D RID: 2125 RVA: 0x0001D43B File Offset: 0x0001B63B
		// (set) Token: 0x0600084E RID: 2126 RVA: 0x0001D443 File Offset: 0x0001B643
		[ALRuleIDForeign(typeof(Where<ALRule.screenID, Equal<Current<ALModel.screenID>>>), DisplayName = "Enabled when", Visibility = 7)]
		[PXForeignReference(typeof(ALModel.FK.FilterRule))]
		[PXUIVisible(typeof(Where<ALModel.modelType, In3<ALModelType.single, ALModelType.labelZoom>>))]
		[PXUIEnabled(typeof(Where<ALModel.modelType, In3<ALModelType.single, ALModelType.labelZoom>, And<ALModel.screenID, IsNotNull>>))]
		public virtual Guid? FilterRuleID { get; set; }

		// Token: 0x1700032D RID: 813
		// (get) Token: 0x0600084F RID: 2127 RVA: 0x0001D44C File Offset: 0x0001B64C
		// (set) Token: 0x06000850 RID: 2128 RVA: 0x0001D454 File Offset: 0x0001B654
		[PXDBBool]
		[PXDefault(false, PersistingCheck = 2)]
		[PXUIField(DisplayName = "Hide Instead")]
		[PXUIVisible(typeof(Where<ALModel.modelType, In3<ALModelType.single, ALModelType.labelZoom>>))]
		[PXUIEnabled(typeof(Where<ALModel.modelType, In3<ALModelType.single, ALModelType.labelZoom>, And<ALModel.screenID, IsNotNull>>))]
		public virtual bool? HideInstead { get; set; }

		// Token: 0x1700032E RID: 814
		// (get) Token: 0x06000851 RID: 2129 RVA: 0x0001D45D File Offset: 0x0001B65D
		// (set) Token: 0x06000852 RID: 2130 RVA: 0x0001D465 File Offset: 0x0001B665
		[PXDBBool]
		[PXDefault(false, PersistingCheck = 2)]
		[PXUIField(DisplayName = "Not", Visibility = 7)]
		[PXUIVisible(typeof(Where<ALModel.modelType, In3<ALModelType.single, ALModelType.snippet, ALModelType.labelZoom>>))]
		[PXUIEnabled(typeof(Where<ALModel.modelType, In3<ALModelType.single, ALModelType.snippet, ALModelType.labelZoom>, And<ALModel.screenID, IsNotNull>>))]
		public virtual bool? ReversePrint { get; set; }

		// Token: 0x1700032F RID: 815
		// (get) Token: 0x06000853 RID: 2131 RVA: 0x0001D46E File Offset: 0x0001B66E
		// (set) Token: 0x06000854 RID: 2132 RVA: 0x0001D476 File Offset: 0x0001B676
		[ALRuleIDForeign(typeof(Where<ALRule.screenID, Equal<Current<ALModel.screenID>>>), DisplayName = "Prints when", Visibility = 7)]
		[PXForeignReference(typeof(ALModel.FK.PrintRule))]
		[PXDefault(typeof(ALModel.filterRuleID), PersistingCheck = 2)]
		[PXUIVisible(typeof(Where<ALModel.modelType, In3<ALModelType.single, ALModelType.snippet, ALModelType.labelZoom>>))]
		[PXUIEnabled(typeof(Where<ALModel.modelType, In3<ALModelType.single, ALModelType.snippet, ALModelType.labelZoom>, And<ALModel.screenID, IsNotNull>>))]
		public virtual Guid? PrintRuleID { get; set; }

		// Token: 0x17000330 RID: 816
		// (get) Token: 0x06000855 RID: 2133 RVA: 0x0001D47F File Offset: 0x0001B67F
		// (set) Token: 0x06000856 RID: 2134 RVA: 0x0001D487 File Offset: 0x0001B687
		[ALLanguage]
		[PXUIVisible(typeof(Where<ALModel.modelType, NotIn3<ALModelType.group, ALModelType.printerSetup, ALModelType.snippet>>))]
		[PXUIEnabled(typeof(Where<ALModel.modelType, NotIn3<ALModelType.group, ALModelType.printerSetup, ALModelType.snippet>>))]
		public virtual string Language { get; set; }

		// Token: 0x17000331 RID: 817
		// (get) Token: 0x06000857 RID: 2135 RVA: 0x0001D490 File Offset: 0x0001B690
		// (set) Token: 0x06000858 RID: 2136 RVA: 0x0001D498 File Offset: 0x0001B698
		[PXDBInt]
		[PXUIField(DisplayName = "Encoding")]
		[ALEncoding.ALListAttribute]
		[PXDefault(PersistingCheck = 2)]
		[PXUIVisible(typeof(Where<ALModel.language, In3<ALConstants.Languages.zpl, ALConstants.Languages.ezp>, And<ALModel.modelType, Equal<ALModelType.single>>>))]
		[PXUIEnabled(typeof(Where<ALModel.language, In3<ALConstants.Languages.zpl, ALConstants.Languages.ezp>, And<ALModel.modelType, Equal<ALModelType.single>>>))]
		public virtual int? Encoding { get; set; }

		// Token: 0x17000332 RID: 818
		// (get) Token: 0x06000859 RID: 2137 RVA: 0x0001D4A1 File Offset: 0x0001B6A1
		// (set) Token: 0x0600085A RID: 2138 RVA: 0x0001D4A9 File Offset: 0x0001B6A9
		[PXDBText(IsUnicode = true)]
		[PXDefault(PersistingCheck = 2)]
		[PXUIField(DisplayName = "Template")]
		[PXUIVisible(typeof(Where<ALModel.modelType, NotIn3<ALModelType.group, ALModelType.printerSetup>>))]
		[PXUIEnabled(typeof(Where<True, Equal<False>>))]
		[Obsolete]
		[BsonIgnore]
		public virtual string Body { get; set; }

		// Token: 0x17000333 RID: 819
		// (get) Token: 0x0600085B RID: 2139 RVA: 0x0001D4B2 File Offset: 0x0001B6B2
		// (set) Token: 0x0600085C RID: 2140 RVA: 0x0001D4BA File Offset: 0x0001B6BA
		[PXDBText(IsUnicode = true)]
		[PXDefault(PersistingCheck = 2)]
		[PXUIField(DisplayName = "Rendered", IsReadOnly = true)]
		[BsonIgnore]
		public virtual string Rendered { get; set; }

		// Token: 0x17000334 RID: 820
		// (get) Token: 0x0600085D RID: 2141 RVA: 0x0001D4C3 File Offset: 0x0001B6C3
		// (set) Token: 0x0600085E RID: 2142 RVA: 0x0001D4CB File Offset: 0x0001B6CB
		[PXDBText(IsUnicode = true)]
		[PXDefault(PersistingCheck = 2)]
		[PXUIField(DisplayName = "Warnings", Enabled = false)]
		[PXUIVisible(typeof(Where<ALModel.modelType, NotIn3<ALModelType.group, ALModelType.printerSetup>>))]
		[PXUIEnabled(typeof(Where<ALModel.modelType, NotIn3<ALModelType.group, ALModelType.printerSetup>>))]
		[BsonIgnore]
		public virtual string Message { get; set; }

		// Token: 0x17000335 RID: 821
		// (get) Token: 0x0600085F RID: 2143 RVA: 0x0001D4D4 File Offset: 0x0001B6D4
		// (set) Token: 0x06000860 RID: 2144 RVA: 0x0001D4DC File Offset: 0x0001B6DC
		[PXDBDecimal(4, MinValue = 0.0)]
		[PXUIField(DisplayName = "Default Size")]
		[PXUIVisible(typeof(Where<ALModel.language, Equal<ALConstants.Languages.pdf>>))]
		[PXUIEnabled(typeof(Where<ALModel.language, Equal<ALConstants.Languages.pdf>>))]
		public virtual decimal? DefaultSize { get; set; }

		// Token: 0x17000336 RID: 822
		// (get) Token: 0x06000861 RID: 2145 RVA: 0x0001D4E5 File Offset: 0x0001B6E5
		// (set) Token: 0x06000862 RID: 2146 RVA: 0x0001D4ED File Offset: 0x0001B6ED
		[ALSizeUnit(Visibility = 7)]
		[ALSizeUnit.ALListAttribute]
		[PXDefault("PT", PersistingCheck = 2)]
		[PXUIVisible(typeof(Where<ALModel.language, Equal<ALConstants.Languages.pdf>>))]
		[PXUIEnabled(typeof(Where<ALModel.language, Equal<ALConstants.Languages.pdf>>))]
		public virtual string SizeUnit { get; set; }

		// Token: 0x17000337 RID: 823
		// (get) Token: 0x06000863 RID: 2147 RVA: 0x0001D4F6 File Offset: 0x0001B6F6
		// (set) Token: 0x06000864 RID: 2148 RVA: 0x0001D4FE File Offset: 0x0001B6FE
		[PXDBBool]
		[PXDefault(false, PersistingCheck = 2)]
		[PXUIField(DisplayName = "Dealing Mode")]
		[PXUIVisible(typeof(Where<ALModel.language, In3<ALConstants.Languages.zpl, ALConstants.Languages.ezp>, And<ALModel.modelType, Equal<ALModelType.single>>>))]
		[PXUIEnabled(typeof(Where<ALModel.language, In3<ALConstants.Languages.zpl, ALConstants.Languages.ezp>, And<ALModel.modelType, Equal<ALModelType.single>>>))]
		public virtual bool? DealingMode { get; set; }

		// Token: 0x17000338 RID: 824
		// (get) Token: 0x06000865 RID: 2149 RVA: 0x0001D507 File Offset: 0x0001B707
		// (set) Token: 0x06000866 RID: 2150 RVA: 0x0001D50F File Offset: 0x0001B70F
		[ALExprValue(DisplayName = "Dealing Count Expr.")]
		[PXUIVisible(typeof(Where<ALModel.dealingMode, Equal<True>>))]
		[PXUIEnabled(typeof(Where<ALModel.dealingMode, Equal<True>>))]
		[PXUIRequired(typeof(Where<ALModel.dealingMode, Equal<True>>))]
		public virtual string DealingCountExpr { get; set; }

		// Token: 0x17000339 RID: 825
		// (get) Token: 0x06000867 RID: 2151 RVA: 0x0001D518 File Offset: 0x0001B718
		// (set) Token: 0x06000868 RID: 2152 RVA: 0x0001D520 File Offset: 0x0001B720
		[ALExprValue(DisplayName = "Nb. of Copies Expr.")]
		[PXUIVisible(typeof(Where<ALModel.modelType, Equal<ALModelType.single>>))]
		[PXUIEnabled(typeof(Where<ALModel.modelType, Equal<ALModelType.single>>))]
		public virtual string NbCopiesExpr { get; set; }

		// Token: 0x1700033A RID: 826
		// (get) Token: 0x06000869 RID: 2153 RVA: 0x0001D529 File Offset: 0x0001B729
		// (set) Token: 0x0600086A RID: 2154 RVA: 0x0001D531 File Offset: 0x0001B731
		[PXDBInt(MinValue = 0)]
		[PXDefault(0, PersistingCheck = 2)]
		[PXUIField(DisplayName = "Pause every X row(s)")]
		[PXUIVisible(typeof(Where<ALModel.language, In3<ALConstants.Languages.zpl, ALConstants.Languages.ezp>, And<ALModel.modelType, Equal<ALModelType.single>>>))]
		[PXUIEnabled(typeof(Where<ALModel.language, In3<ALConstants.Languages.zpl, ALConstants.Languages.ezp>, And<ALModel.modelType, Equal<ALModelType.single>>>))]
		public virtual int? SendPauseEvery { get; set; }

		// Token: 0x1700033B RID: 827
		// (get) Token: 0x0600086B RID: 2155 RVA: 0x0001D53A File Offset: 0x0001B73A
		// (set) Token: 0x0600086C RID: 2156 RVA: 0x0001D542 File Offset: 0x0001B742
		[ALNumberingSequence(DisplayName = "Numbering Sequence")]
		[PXForeignReference(typeof(ALModel.FK.CSNumbering))]
		[PXUIVisible(typeof(Where<ALModel.language, In3<ALConstants.Languages.zpl, ALConstants.Languages.ezp>, And<ALModel.modelType, Equal<ALModelType.single>>>))]
		[PXUIEnabled(typeof(Where<ALModel.language, In3<ALConstants.Languages.zpl, ALConstants.Languages.ezp>, And<ALModel.modelType, Equal<ALModelType.single>>>))]
		public virtual string NumberingID { get; set; }

		// Token: 0x1700033C RID: 828
		// (get) Token: 0x0600086D RID: 2157 RVA: 0x0001D54B File Offset: 0x0001B74B
		// (set) Token: 0x0600086E RID: 2158 RVA: 0x0001D553 File Offset: 0x0001B753
		[ALOnOtherDensity]
		[PXDefault("PA", PersistingCheck = 2)]
		[PXUIVisible(typeof(Where<ALModel.language, In3<ALConstants.Languages.zpl, ALConstants.Languages.ezp>, And<ALModel.modelType, Equal<ALModelType.single>>>))]
		[PXUIEnabled(typeof(Where<ALModel.language, In3<ALConstants.Languages.zpl, ALConstants.Languages.ezp>, And<ALModel.modelType, Equal<ALModelType.single>>>))]
		public virtual string PrintOnOtherDensity { get; set; }

		// Token: 0x1700033D RID: 829
		// (get) Token: 0x0600086F RID: 2159 RVA: 0x0001D55C File Offset: 0x0001B75C
		// (set) Token: 0x06000870 RID: 2160 RVA: 0x0001D564 File Offset: 0x0001B764
		[PXString(IsUnicode = true)]
		[PXUIField(DisplayName = "Action Name", IsReadOnly = true)]
		[PXFormula(typeof(ALModelActionName<ALModel.labelID>))]
		[BsonIgnore]
		public virtual string ActionName { get; set; }

		// Token: 0x1700033E RID: 830
		// (get) Token: 0x06000871 RID: 2161 RVA: 0x0001D56D File Offset: 0x0001B76D
		// (set) Token: 0x06000872 RID: 2162 RVA: 0x0001D575 File Offset: 0x0001B775
		[PXDBBool]
		[PXUIField(DisplayName = "Merge Detail Labels")]
		[PXDefault(false, PersistingCheck = 2)]
		public virtual bool? MergeDetails { get; set; }

		// Token: 0x1700033F RID: 831
		// (get) Token: 0x06000873 RID: 2163 RVA: 0x0001D57E File Offset: 0x0001B77E
		// (set) Token: 0x06000874 RID: 2164 RVA: 0x0001D586 File Offset: 0x0001B786
		[PXDBBool]
		[PXUIField(DisplayName = "Print Separately")]
		[PXDefault(false, PersistingCheck = 2)]
		[PXUIVisible(typeof(Where<ALModel.mergeDetails, Equal<True>>))]
		[PXUIEnabled(typeof(Where<ALModel.mergeDetails, Equal<True>>))]
		public virtual bool? PrintDetails { get; set; }

		// Token: 0x17000340 RID: 832
		// (get) Token: 0x06000875 RID: 2165 RVA: 0x0001D58F File Offset: 0x0001B78F
		// (set) Token: 0x06000876 RID: 2166 RVA: 0x0001D597 File Offset: 0x0001B797
		[PXNote(DescriptionField = typeof(ALModel.description))]
		public virtual Guid? NoteID { get; set; }

		// Token: 0x17000341 RID: 833
		// (get) Token: 0x06000877 RID: 2167 RVA: 0x0001D5A0 File Offset: 0x0001B7A0
		// (set) Token: 0x06000878 RID: 2168 RVA: 0x0001D5A8 File Offset: 0x0001B7A8
		[PXDBCreatedByID]
		public virtual Guid? CreatedByID { get; set; }

		// Token: 0x17000342 RID: 834
		// (get) Token: 0x06000879 RID: 2169 RVA: 0x0001D5B1 File Offset: 0x0001B7B1
		// (set) Token: 0x0600087A RID: 2170 RVA: 0x0001D5B9 File Offset: 0x0001B7B9
		[PXDBCreatedByScreenID]
		public virtual string CreatedByScreenID { get; set; }

		// Token: 0x17000343 RID: 835
		// (get) Token: 0x0600087B RID: 2171 RVA: 0x0001D5C2 File Offset: 0x0001B7C2
		// (set) Token: 0x0600087C RID: 2172 RVA: 0x0001D5CA File Offset: 0x0001B7CA
		[PXDBCreatedDateTime]
		[PXUIField(DisplayName = "Created On", Enabled = false, IsReadOnly = true)]
		public virtual DateTime? CreatedDateTime { get; set; }

		// Token: 0x17000344 RID: 836
		// (get) Token: 0x0600087D RID: 2173 RVA: 0x0001D5D3 File Offset: 0x0001B7D3
		// (set) Token: 0x0600087E RID: 2174 RVA: 0x0001D5DB File Offset: 0x0001B7DB
		[PXDBLastModifiedByID]
		public virtual Guid? LastModifiedByID { get; set; }

		// Token: 0x17000345 RID: 837
		// (get) Token: 0x0600087F RID: 2175 RVA: 0x0001D5E4 File Offset: 0x0001B7E4
		// (set) Token: 0x06000880 RID: 2176 RVA: 0x0001D5EC File Offset: 0x0001B7EC
		[PXDBLastModifiedByScreenID]
		public virtual string LastModifiedByScreenID { get; set; }

		// Token: 0x17000346 RID: 838
		// (get) Token: 0x06000881 RID: 2177 RVA: 0x0001D5F5 File Offset: 0x0001B7F5
		// (set) Token: 0x06000882 RID: 2178 RVA: 0x0001D5FD File Offset: 0x0001B7FD
		[PXDBLastModifiedDateTime]
		[PXUIField(DisplayName = "Last Modified On", Enabled = false, IsReadOnly = true)]
		public virtual DateTime? LastModifiedDateTime { get; set; }

		// Token: 0x17000347 RID: 839
		// (get) Token: 0x06000883 RID: 2179 RVA: 0x0001D606 File Offset: 0x0001B806
		// (set) Token: 0x06000884 RID: 2180 RVA: 0x0001D60E File Offset: 0x0001B80E
		[PXDBTimestamp]
		public virtual byte[] tstamp { get; set; }

		// Token: 0x04000268 RID: 616
		public const string ENABLED_WHEN = "Enabled when";

		// Token: 0x04000269 RID: 617
		public const string PRINTS_WHEN = "Prints when";

		// Token: 0x020004FA RID: 1274
		public class PK : PrimaryKeyOf<ALModel>.By<ALModel.labelID>
		{
			// Token: 0x06001B99 RID: 7065 RVA: 0x00057DD0 File Offset: 0x00055FD0
			public static ALModel Find(PXGraph graph, Guid? labelID)
			{
				return PrimaryKeyOf<ALModel>.By<ALModel.labelID>.FindBy(graph, labelID, 0);
			}
		}

		// Token: 0x020004FB RID: 1275
		public static class FK
		{
			// Token: 0x020009E2 RID: 2530
			public class Format : PrimaryKeyOf<ALFormat>.By<ALFormat.formatID>.ForeignKeyOf<ALModel>.By<ALModel.formatID>
			{
			}

			// Token: 0x020009E3 RID: 2531
			public class PortalMap : PrimaryKeyOf<PX.SM.PortalMap>.By<PX.SM.PortalMap.nodeID>.ForeignKeyOf<ALModel>.By<ALModel.screenID>
			{
			}

			// Token: 0x020009E4 RID: 2532
			public class SiteMap : PrimaryKeyOf<PX.SM.SiteMap>.By<PX.SM.SiteMap.nodeID>.ForeignKeyOf<ALModel>.By<ALModel.screenID>
			{
			}

			// Token: 0x020009E5 RID: 2533
			public class FilterRule : PrimaryKeyOf<ALRule>.By<ALRule.ruleID>.ForeignKeyOf<ALModel>.By<ALModel.filterRuleID>
			{
			}

			// Token: 0x020009E6 RID: 2534
			public class PrintRule : PrimaryKeyOf<ALRule>.By<ALRule.ruleID>.ForeignKeyOf<ALModel>.By<ALModel.printRuleID>
			{
			}

			// Token: 0x020009E7 RID: 2535
			public class Margin : PrimaryKeyOf<ALMargin>.By<ALMargin.marginID>.ForeignKeyOf<ALModel>.By<ALModel.marginID>
			{
			}

			// Token: 0x020009E8 RID: 2536
			public class Category : PrimaryKeyOf<ALCategory>.By<ALCategory.categoryID>.ForeignKeyOf<ALModel>.By<ALModel.categoryID>
			{
			}

			// Token: 0x020009E9 RID: 2537
			public class CSNumbering : PrimaryKeyOf<Numbering>.By<Numbering.numberingID>.ForeignKeyOf<ALModel>.By<ALModel.numberingID>
			{
			}
		}

		// Token: 0x020004FC RID: 1276
		[Nullable(new byte[]
		{
			0,
			1,
			0
		})]
		public abstract class labelID : BqlType<IBqlGuid, Guid>.Field<ALModel.labelID>
		{
		}

		// Token: 0x020004FD RID: 1277
		[Nullable(new byte[]
		{
			0,
			1,
			0
		})]
		public abstract class active : BqlType<IBqlBool, bool>.Field<ALModel.active>
		{
		}

		// Token: 0x020004FE RID: 1278
		[Nullable(new byte[]
		{
			0,
			1,
			0
		})]
		public abstract class isSystem : BqlType<IBqlBool, bool>.Field<ALModel.isSystem>
		{
		}

		// Token: 0x020004FF RID: 1279
		[Nullable(new byte[]
		{
			0,
			1,
			0
		})]
		public abstract class allowExport : BqlType<IBqlBool, bool>.Field<ALModel.allowExport>
		{
		}

		// Token: 0x02000500 RID: 1280
		[Nullable(new byte[]
		{
			0,
			1,
			1,
			0
		})]
		public abstract class name : BqlType<IBqlString, string>.Field<ALModel.name>
		{
		}

		// Token: 0x02000501 RID: 1281
		[Nullable(new byte[]
		{
			0,
			1,
			1,
			0
		})]
		public abstract class description : BqlType<IBqlString, string>.Field<ALModel.description>
		{
		}

		// Token: 0x02000502 RID: 1282
		[Nullable(new byte[]
		{
			0,
			1,
			1,
			0
		})]
		public abstract class modelType : BqlType<IBqlString, string>.Field<ALModel.modelType>
		{
		}

		// Token: 0x02000503 RID: 1283
		[Nullable(new byte[]
		{
			0,
			1,
			0
		})]
		public abstract class categoryID : BqlType<IBqlGuid, Guid>.Field<ALModel.categoryID>
		{
		}

		// Token: 0x02000504 RID: 1284
		[Nullable(new byte[]
		{
			0,
			1,
			1,
			0
		})]
		public abstract class screenID : BqlType<IBqlString, string>.Field<ALModel.screenID>
		{
		}

		// Token: 0x02000505 RID: 1285
		[Nullable(new byte[]
		{
			0,
			1,
			1,
			0
		})]
		public abstract class graphType : BqlType<IBqlString, string>.Field<ALModel.graphType>
		{
		}

		// Token: 0x02000506 RID: 1286
		[Nullable(new byte[]
		{
			0,
			1,
			1,
			0
		})]
		public abstract class basedOnView : BqlType<IBqlString, string>.Field<ALModel.basedOnView>
		{
		}

		// Token: 0x02000507 RID: 1287
		[Nullable(new byte[]
		{
			0,
			1,
			1,
			0
		})]
		public abstract class cloudID : BqlType<IBqlString, string>.Field<ALModel.cloudID>
		{
		}

		// Token: 0x02000508 RID: 1288
		[Nullable(new byte[]
		{
			0,
			1,
			0
		})]
		public abstract class formatID : BqlType<IBqlGuid, Guid>.Field<ALModel.formatID>
		{
		}

		// Token: 0x02000509 RID: 1289
		[Nullable(new byte[]
		{
			0,
			1,
			0
		})]
		public abstract class marginID : BqlType<IBqlGuid, Guid>.Field<ALModel.marginID>
		{
		}

		// Token: 0x0200050A RID: 1290
		[Nullable(new byte[]
		{
			0,
			1,
			1,
			0
		})]
		public abstract class imageUrl : BqlType<IBqlString, string>.Field<ALModel.imageUrl>
		{
		}

		// Token: 0x0200050B RID: 1291
		[Nullable(new byte[]
		{
			0,
			1,
			0
		})]
		public abstract class showTemplate : BqlType<IBqlBool, bool>.Field<ALModel.showTemplate>
		{
		}

		// Token: 0x0200050C RID: 1292
		[Nullable(new byte[]
		{
			0,
			1,
			0
		})]
		public abstract class showExprs : BqlType<IBqlBool, bool>.Field<ALModel.showExprs>
		{
		}

		// Token: 0x0200050D RID: 1293
		[Nullable(new byte[]
		{
			0,
			1,
			0
		})]
		public abstract class showRendered : BqlType<IBqlBool, bool>.Field<ALModel.showRendered>
		{
		}

		// Token: 0x0200050E RID: 1294
		[Nullable(new byte[]
		{
			0,
			1,
			0
		})]
		public abstract class showSetup : BqlType<IBqlBool, bool>.Field<ALModel.showSetup>
		{
		}

		// Token: 0x0200050F RID: 1295
		[Nullable(new byte[]
		{
			0,
			1,
			0
		})]
		public abstract class showPrinters : BqlType<IBqlBool, bool>.Field<ALModel.showPrinters>
		{
		}

		// Token: 0x02000510 RID: 1296
		[Nullable(new byte[]
		{
			0,
			1,
			0
		})]
		public abstract class showPrintLog : BqlType<IBqlBool, bool>.Field<ALModel.showPrintLog>
		{
		}

		// Token: 0x02000511 RID: 1297
		[Nullable(new byte[]
		{
			0,
			1,
			0
		})]
		public abstract class showChildren : BqlType<IBqlBool, bool>.Field<ALModel.showChildren>
		{
		}

		// Token: 0x02000512 RID: 1298
		[Nullable(new byte[]
		{
			0,
			1,
			0
		})]
		public abstract class showUsedBy : BqlType<IBqlBool, bool>.Field<ALModel.showUsedBy>
		{
		}

		// Token: 0x02000513 RID: 1299
		[Nullable(new byte[]
		{
			0,
			1,
			0
		})]
		public abstract class hideWhenInGroup : BqlType<IBqlBool, bool>.Field<ALModel.hideWhenInGroup>
		{
		}

		// Token: 0x02000514 RID: 1300
		[Nullable(new byte[]
		{
			0,
			1,
			0
		})]
		public abstract class ignoreRotationOnRender : BqlType<IBqlBool, bool>.Field<ALModel.ignoreRotationOnRender>
		{
		}

		// Token: 0x02000515 RID: 1301
		[Nullable(new byte[]
		{
			0,
			1,
			1,
			0
		})]
		public abstract class tooltip : BqlType<IBqlString, string>.Field<ALModel.tooltip>
		{
		}

		// Token: 0x02000516 RID: 1302
		[Nullable(new byte[]
		{
			0,
			1,
			0
		})]
		public abstract class reverseFilter : BqlType<IBqlBool, bool>.Field<ALModel.reverseFilter>
		{
		}

		// Token: 0x02000517 RID: 1303
		[Nullable(new byte[]
		{
			0,
			1,
			0
		})]
		public abstract class filterRuleID : BqlType<IBqlGuid, Guid>.Field<ALModel.filterRuleID>
		{
		}

		// Token: 0x02000518 RID: 1304
		[Nullable(new byte[]
		{
			0,
			1,
			0
		})]
		public abstract class hideInstead : BqlType<IBqlBool, bool>.Field<ALModel.hideInstead>
		{
		}

		// Token: 0x02000519 RID: 1305
		[Nullable(new byte[]
		{
			0,
			1,
			0
		})]
		public abstract class reversePrint : BqlType<IBqlBool, bool>.Field<ALModel.reversePrint>
		{
		}

		// Token: 0x0200051A RID: 1306
		[Nullable(new byte[]
		{
			0,
			1,
			0
		})]
		public abstract class printRuleID : BqlType<IBqlGuid, Guid>.Field<ALModel.printRuleID>
		{
		}

		// Token: 0x0200051B RID: 1307
		[Nullable(new byte[]
		{
			0,
			1,
			1,
			0
		})]
		public abstract class language : BqlType<IBqlString, string>.Field<ALModel.language>
		{
		}

		// Token: 0x0200051C RID: 1308
		[Nullable(new byte[]
		{
			0,
			1,
			0
		})]
		public abstract class encoding : BqlType<IBqlInt, int>.Field<ALModel.encoding>
		{
		}

		// Token: 0x0200051D RID: 1309
		[Nullable(new byte[]
		{
			0,
			1,
			1,
			0
		})]
		public abstract class body : BqlType<IBqlString, string>.Field<ALModel.body>
		{
		}

		// Token: 0x0200051E RID: 1310
		[Nullable(new byte[]
		{
			0,
			1,
			1,
			0
		})]
		public abstract class rendered : BqlType<IBqlString, string>.Field<ALModel.rendered>
		{
		}

		// Token: 0x0200051F RID: 1311
		[Nullable(new byte[]
		{
			0,
			1,
			1,
			0
		})]
		public abstract class message : BqlType<IBqlString, string>.Field<ALModel.message>
		{
		}

		// Token: 0x02000520 RID: 1312
		[Nullable(new byte[]
		{
			0,
			1,
			0
		})]
		public abstract class defaultSize : BqlType<IBqlDecimal, decimal>.Field<ALModel.defaultSize>
		{
		}

		// Token: 0x02000521 RID: 1313
		[Nullable(new byte[]
		{
			0,
			1,
			1,
			0
		})]
		public abstract class sizeUnit : BqlType<IBqlString, string>.Field<ALModel.sizeUnit>
		{
		}

		// Token: 0x02000522 RID: 1314
		[Nullable(new byte[]
		{
			0,
			1,
			0
		})]
		public abstract class dealingMode : BqlType<IBqlBool, bool>.Field<ALModel.dealingMode>
		{
		}

		// Token: 0x02000523 RID: 1315
		[Nullable(new byte[]
		{
			0,
			1,
			1,
			0
		})]
		public abstract class dealingCountExpr : BqlType<IBqlString, string>.Field<ALModel.dealingCountExpr>
		{
		}

		// Token: 0x02000524 RID: 1316
		[Nullable(new byte[]
		{
			0,
			1,
			1,
			0
		})]
		public abstract class nbCopiesExpr : BqlType<IBqlString, string>.Field<ALModel.nbCopiesExpr>
		{
		}

		// Token: 0x02000525 RID: 1317
		[Nullable(new byte[]
		{
			0,
			1,
			0
		})]
		public abstract class sendPauseEvery : BqlType<IBqlInt, int>.Field<ALModel.sendPauseEvery>
		{
		}

		// Token: 0x02000526 RID: 1318
		[Nullable(new byte[]
		{
			0,
			1,
			1,
			0
		})]
		public abstract class numberingID : BqlType<IBqlString, string>.Field<ALModel.numberingID>
		{
		}

		// Token: 0x02000527 RID: 1319
		[Nullable(new byte[]
		{
			0,
			1,
			1,
			0
		})]
		public abstract class printOnOtherDensity : BqlType<IBqlString, string>.Field<ALModel.printOnOtherDensity>
		{
		}

		// Token: 0x02000528 RID: 1320
		[Nullable(new byte[]
		{
			0,
			1,
			1,
			0
		})]
		public abstract class actionName : BqlType<IBqlString, string>.Field<ALModel.actionName>
		{
		}

		// Token: 0x02000529 RID: 1321
		[Nullable(new byte[]
		{
			0,
			1,
			0
		})]
		public abstract class mergeDetails : BqlType<IBqlBool, bool>.Field<ALModel.mergeDetails>
		{
		}

		// Token: 0x0200052A RID: 1322
		[Nullable(new byte[]
		{
			0,
			1,
			0
		})]
		public abstract class printDetails : BqlType<IBqlBool, bool>.Field<ALModel.printDetails>
		{
		}

		// Token: 0x0200052B RID: 1323
		[Nullable(new byte[]
		{
			0,
			1,
			0
		})]
		public abstract class noteID : BqlType<IBqlGuid, Guid>.Field<ALModel.noteID>
		{
		}

		// Token: 0x0200052C RID: 1324
		[Nullable(new byte[]
		{
			0,
			1,
			0
		})]
		public abstract class createdByID : BqlType<IBqlGuid, Guid>.Field<ALModel.createdByID>
		{
		}

		// Token: 0x0200052D RID: 1325
		[Nullable(new byte[]
		{
			0,
			1,
			1,
			0
		})]
		public abstract class createdByScreenID : BqlType<IBqlString, string>.Field<ALModel.createdByScreenID>
		{
		}

		// Token: 0x0200052E RID: 1326
		[Nullable(new byte[]
		{
			0,
			1,
			0
		})]
		public abstract class createdDateTime : BqlType<IBqlDateTime, DateTime>.Field<ALModel.createdDateTime>
		{
		}

		// Token: 0x0200052F RID: 1327
		[Nullable(new byte[]
		{
			0,
			1,
			0
		})]
		public abstract class lastModifiedByID : BqlType<IBqlGuid, Guid>.Field<ALModel.lastModifiedByID>
		{
		}

		// Token: 0x02000530 RID: 1328
		[Nullable(new byte[]
		{
			0,
			1,
			1,
			0
		})]
		public abstract class lastModifiedByScreenID : BqlType<IBqlString, string>.Field<ALModel.lastModifiedByScreenID>
		{
		}

		// Token: 0x02000531 RID: 1329
		[Nullable(new byte[]
		{
			0,
			1,
			0
		})]
		public abstract class lastModifiedDateTime : BqlType<IBqlDateTime, DateTime>.Field<ALModel.lastModifiedDateTime>
		{
		}

		// Token: 0x02000532 RID: 1330
		[Nullable(new byte[]
		{
			0,
			1,
			1,
			0
		})]
		public abstract class Tstamp : BqlType<IBqlByteArray, byte[]>.Field<ALModel.Tstamp>
		{
		}
	}
}
