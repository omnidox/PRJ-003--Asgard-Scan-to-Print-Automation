using System;
using System.Diagnostics;
using AA.Objects.AL.LabelZoom;
using AA.Objects.AL.Language.Ezp;
using AA.Objects.AL.Language.Zpl;
using AA.Objects.AL.Subscribers;
using PX.Api;
using PX.BusinessProcess.DAC;
using PX.Data;
using PX.Data.BQL;
using PX.Data.EP;
using PX.Data.Maintenance.GI;
using PX.Data.ReferentialIntegrity.Attributes;
using PX.Objects.CS;
using PX.SM;

namespace AA.Objects.AL
{
	// Token: 0x020001AF RID: 431
	[PXCacheName("Label Model")]
	[PXPrimaryGraph(typeof(ALModelMaint))]
	[DebuggerDisplay("Model: {Name} ({Description})")]
	[Serializable]
	public class ALModel : PXBqlTable, IBqlTable, IBqlTableSystemDataStorage, INotable, IRuleDriven, IExportable, IRenderableConfig, IImageStore, ISentByEvent
	{
		// Token: 0x170004DB RID: 1243
		// (get) Token: 0x06000E9E RID: 3742 RVA: 0x00034A71 File Offset: 0x00032C71
		// (set) Token: 0x06000E9F RID: 3743 RVA: 0x00034A79 File Offset: 0x00032C79
		[ALGuidID]
		public virtual Guid? LabelID { get; set; }

		// Token: 0x170004DC RID: 1244
		// (get) Token: 0x06000EA0 RID: 3744 RVA: 0x00034A82 File Offset: 0x00032C82
		public Guid? ParentID
		{
			get
			{
				return this.LabelID;
			}
		}

		// Token: 0x170004DD RID: 1245
		// (get) Token: 0x06000EA1 RID: 3745 RVA: 0x00034A82 File Offset: 0x00032C82
		public Guid? ChildID
		{
			get
			{
				return this.LabelID;
			}
		}

		// Token: 0x170004DE RID: 1246
		// (get) Token: 0x06000EA2 RID: 3746 RVA: 0x00034A8C File Offset: 0x00032C8C
		public int? BAccountID
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170004DF RID: 1247
		// (get) Token: 0x06000EA3 RID: 3747 RVA: 0x00015E21 File Offset: 0x00014021
		public bool? DoThrow
		{
			get
			{
				return new bool?(false);
			}
		}

		// Token: 0x170004E0 RID: 1248
		// (get) Token: 0x06000EA4 RID: 3748 RVA: 0x00034A82 File Offset: 0x00032C82
		public Guid? ID
		{
			get
			{
				return this.LabelID;
			}
		}

		// Token: 0x170004E1 RID: 1249
		// (get) Token: 0x06000EA5 RID: 3749 RVA: 0x00034AA2 File Offset: 0x00032CA2
		// (set) Token: 0x06000EA6 RID: 3750 RVA: 0x00034AAA File Offset: 0x00032CAA
		[ALActive]
		public virtual bool? Active { get; set; }

		// Token: 0x170004E2 RID: 1250
		// (get) Token: 0x06000EA7 RID: 3751 RVA: 0x00034AB3 File Offset: 0x00032CB3
		// (set) Token: 0x06000EA8 RID: 3752 RVA: 0x00034ABB File Offset: 0x00032CBB
		[ALSystem]
		public virtual bool? IsSystem { get; set; }

		// Token: 0x170004E3 RID: 1251
		// (get) Token: 0x06000EA9 RID: 3753 RVA: 0x00034AC4 File Offset: 0x00032CC4
		// (set) Token: 0x06000EAA RID: 3754 RVA: 0x00034ACC File Offset: 0x00032CCC
		[ALExport]
		public virtual bool? AllowExport { get; set; }

		// Token: 0x170004E4 RID: 1252
		// (get) Token: 0x06000EAB RID: 3755 RVA: 0x00034AD5 File Offset: 0x00032CD5
		// (set) Token: 0x06000EAC RID: 3756 RVA: 0x00034ADD File Offset: 0x00032CDD
		[ALName(typeof(ALModel.name), typeof(ALModel.description), 50, IsKey = true, DisplayName = "Model ID")]
		public virtual string Name { get; set; }

		// Token: 0x170004E5 RID: 1253
		// (get) Token: 0x06000EAD RID: 3757 RVA: 0x00034AE6 File Offset: 0x00032CE6
		// (set) Token: 0x06000EAE RID: 3758 RVA: 0x00034AEE File Offset: 0x00032CEE
		[ALDescription]
		[PXFieldDescription]
		public virtual string Description { get; set; }

		// Token: 0x170004E6 RID: 1254
		// (get) Token: 0x06000EAF RID: 3759 RVA: 0x00034AF7 File Offset: 0x00032CF7
		// (set) Token: 0x06000EB0 RID: 3760 RVA: 0x00034AFF File Offset: 0x00032CFF
		[PXDBString(1, IsFixed = true)]
		[PXDefault("S")]
		[PXUIField(DisplayName = "Model Type", Visibility = 7)]
		[ALModelType.ALListAttribute]
		public virtual string ModelType { get; set; }

		// Token: 0x170004E7 RID: 1255
		// (get) Token: 0x06000EB1 RID: 3761 RVA: 0x00034B08 File Offset: 0x00032D08
		// (set) Token: 0x06000EB2 RID: 3762 RVA: 0x00034B10 File Offset: 0x00032D10
		[ALCategoryIDForeign]
		[PXDefault(typeof(ALSetup.defaultCategoryID), PersistingCheck = 2)]
		[PXForeignReference(typeof(ALModel.FK.Category))]
		public virtual Guid? CategoryID { get; set; }

		// Token: 0x170004E8 RID: 1256
		// (get) Token: 0x06000EB3 RID: 3763 RVA: 0x00034B19 File Offset: 0x00032D19
		// (set) Token: 0x06000EB4 RID: 3764 RVA: 0x00019FF9 File Offset: 0x000181F9
		string ISentByEvent.SubscriberType
		{
			get
			{
				return LabelPrintSubscriberHandlerFactory.TYPE;
			}
			set
			{
			}
		}

		// Token: 0x06000EB5 RID: 3765 RVA: 0x00034A82 File Offset: 0x00032C82
		Guid? ISentByEvent.GetHandlerId()
		{
			return this.LabelID;
		}

		// Token: 0x170004E9 RID: 1257
		// (get) Token: 0x06000EB6 RID: 3766 RVA: 0x00034B20 File Offset: 0x00032D20
		// (set) Token: 0x06000EB7 RID: 3767 RVA: 0x00034B28 File Offset: 0x00032D28
		[ALScreenID]
		[PXForeignReference(typeof(ALModel.FK.PortalMap))]
		[PXForeignReference(typeof(ALModel.FK.SiteMap))]
		[PXUIVisible(typeof(Where<True, Equal<True>>))]
		[PXUIEnabled(typeof(Where<True, Equal<True>>))]
		[PXUIRequired(typeof(Where<True, Equal<True>>))]
		public virtual string ScreenID { get; set; }

		// Token: 0x170004EA RID: 1258
		// (get) Token: 0x06000EB8 RID: 3768 RVA: 0x00034B31 File Offset: 0x00032D31
		// (set) Token: 0x06000EB9 RID: 3769 RVA: 0x00034B39 File Offset: 0x00032D39
		[ALGraphType(typeof(ALModel.screenID))]
		[PXDefault]
		[PXUIVisible(typeof(Where<True, Equal<True>>))]
		[PXUIEnabled(typeof(Where<True, Equal<True>>))]
		[PXUIRequired(typeof(Where<True, Equal<True>>))]
		public virtual string GraphType { get; set; }

		// Token: 0x170004EB RID: 1259
		// (get) Token: 0x06000EBA RID: 3770 RVA: 0x00034B42 File Offset: 0x00032D42
		// (set) Token: 0x06000EBB RID: 3771 RVA: 0x00034B4A File Offset: 0x00032D4A
		[ALViewField(typeof(ALModel.screenID), typeof(ALModel.graphType), false, DisplayName = "Based On View")]
		[PXUIVisible(typeof(Where<ALModel.modelType, In3<ALModelType.single, ALModelType.snippet>>))]
		[PXUIEnabled(typeof(Where<ALModel.modelType, In3<ALModelType.single, ALModelType.snippet>>))]
		[PXUIRequired(typeof(Where<ALModel.modelType, In3<ALModelType.single, ALModelType.snippet>>))]
		public virtual string BasedOnView { get; set; }

		// Token: 0x170004EC RID: 1260
		// (get) Token: 0x06000EBC RID: 3772 RVA: 0x00034B53 File Offset: 0x00032D53
		// (set) Token: 0x06000EBD RID: 3773 RVA: 0x00034B5B File Offset: 0x00032D5B
		[ALCloudLabelIDStandalone(typeof(ALModel.modelType))]
		[PXUIVisible(typeof(Where<ALModel.modelType, Equal<ALModelType.labelZoom>>))]
		[PXUIEnabled(typeof(Where<ALModel.modelType, Equal<ALModelType.labelZoom>>))]
		[PXUIRequired(typeof(Where<ALModel.modelType, Equal<ALModelType.labelZoom>>))]
		public virtual string CloudID { get; set; }

		// Token: 0x170004ED RID: 1261
		// (get) Token: 0x06000EBE RID: 3774 RVA: 0x00034B64 File Offset: 0x00032D64
		// (set) Token: 0x06000EBF RID: 3775 RVA: 0x00034B6C File Offset: 0x00032D6C
		[ALFormatIDForeign(typeof(Where<True, Equal<True>>))]
		[PXForeignReference(typeof(ALModel.FK.Format))]
		[PXDefault(typeof(ALSetup.defaultFormatID), PersistingCheck = 2)]
		[PXUIVisible(typeof(Where<ALModel.modelType, Equal<ALModelType.single>>))]
		[PXUIEnabled(typeof(Where<ALModel.modelType, Equal<ALModelType.single>>))]
		[PXUIRequired(typeof(Where<ALModel.modelType, Equal<ALModelType.single>>))]
		public virtual Guid? FormatID { get; set; }

		// Token: 0x170004EE RID: 1262
		// (get) Token: 0x06000EC0 RID: 3776 RVA: 0x00034B75 File Offset: 0x00032D75
		// (set) Token: 0x06000EC1 RID: 3777 RVA: 0x00034B7D File Offset: 0x00032D7D
		[ALMarginIDForeign]
		[PXForeignReference(typeof(ALModel.FK.Margin))]
		[PXDefault(typeof(ALSetup.defaultMarginID), PersistingCheck = 2)]
		[PXUIVisible(typeof(Where<ALModel.modelType, Equal<ALModelType.single>>))]
		[PXUIEnabled(typeof(Where<ALModel.modelType, Equal<ALModelType.single>>))]
		public virtual Guid? MarginID { get; set; }

		// Token: 0x170004EF RID: 1263
		// (get) Token: 0x06000EC2 RID: 3778 RVA: 0x00034B86 File Offset: 0x00032D86
		// (set) Token: 0x06000EC3 RID: 3779 RVA: 0x00034B8E File Offset: 0x00032D8E
		[PXDBString(255, IsUnicode = true)]
		[PXUIField(DisplayName = "Image", Visible = false)]
		public string ImageUrl { get; set; }

		// Token: 0x170004F0 RID: 1264
		// (get) Token: 0x06000EC4 RID: 3780 RVA: 0x00034B98 File Offset: 0x00032D98
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
				return new bool?(this.ModelType != "G" && this.ModelType != "P" && ALSetupSlot.DevMode);
			}
		}

		// Token: 0x170004F1 RID: 1265
		// (get) Token: 0x06000EC5 RID: 3781 RVA: 0x00034BDC File Offset: 0x00032DDC
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
				return new bool?(this.ModelType != "G" && this.ModelType != "P");
			}
		}

		// Token: 0x170004F2 RID: 1266
		// (get) Token: 0x06000EC6 RID: 3782 RVA: 0x00034C18 File Offset: 0x00032E18
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

		// Token: 0x170004F3 RID: 1267
		// (get) Token: 0x06000EC7 RID: 3783 RVA: 0x00034C50 File Offset: 0x00032E50
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

		// Token: 0x170004F4 RID: 1268
		// (get) Token: 0x06000EC8 RID: 3784 RVA: 0x00034C68 File Offset: 0x00032E68
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
				return new bool?(this.ModelType == "S" || this.ModelType == "P");
			}
		}

		// Token: 0x170004F5 RID: 1269
		// (get) Token: 0x06000EC9 RID: 3785 RVA: 0x00034CA4 File Offset: 0x00032EA4
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
				return new bool?(this.ModelType == "S");
			}
		}

		// Token: 0x170004F6 RID: 1270
		// (get) Token: 0x06000ECA RID: 3786 RVA: 0x00034CCC File Offset: 0x00032ECC
		[PXBool]
		[PXUIField(Visibility = 3)]
		public virtual bool? ShowAutomation
		{
			[PXDependsOnFields(new Type[]
			{
				typeof(ALModel.modelType)
			})]
			get
			{
				return new bool?(this.ModelType == "S" || (this.ModelType == "G" && ALSetupSlot.ShowAutomation));
			}
		}

		// Token: 0x170004F7 RID: 1271
		// (get) Token: 0x06000ECB RID: 3787 RVA: 0x00034D14 File Offset: 0x00032F14
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

		// Token: 0x170004F8 RID: 1272
		// (get) Token: 0x06000ECC RID: 3788 RVA: 0x00034D3C File Offset: 0x00032F3C
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

		// Token: 0x170004F9 RID: 1273
		// (get) Token: 0x06000ECD RID: 3789 RVA: 0x00034D63 File Offset: 0x00032F63
		// (set) Token: 0x06000ECE RID: 3790 RVA: 0x00034D6B File Offset: 0x00032F6B
		[PXDBBool]
		[PXDefault(true, PersistingCheck = 2)]
		[PXUIField(DisplayName = "Hide When In Group")]
		[PXUIVisible(typeof(Where<ALModel.modelType, Equal<ALModelType.single>>))]
		[PXUIEnabled(typeof(Where<ALModel.modelType, Equal<ALModelType.single>>))]
		public virtual bool? HideWhenInGroup { get; set; }

		// Token: 0x170004FA RID: 1274
		// (get) Token: 0x06000ECF RID: 3791 RVA: 0x00034D74 File Offset: 0x00032F74
		// (set) Token: 0x06000ED0 RID: 3792 RVA: 0x00034D7C File Offset: 0x00032F7C
		[PXDBBool]
		[PXDefault(true, PersistingCheck = 2)]
		[PXUIField(DisplayName = "Ignore Rotation On Render")]
		[PXUIVisible(typeof(Where<ALModel.modelType, Equal<ALModelType.single>>))]
		[PXUIEnabled(typeof(Where<ALModel.modelType, Equal<ALModelType.single>>))]
		public virtual bool? IgnoreRotationOnRender { get; set; }

		// Token: 0x170004FB RID: 1275
		// (get) Token: 0x06000ED1 RID: 3793 RVA: 0x00034D85 File Offset: 0x00032F85
		// (set) Token: 0x06000ED2 RID: 3794 RVA: 0x00034D8D File Offset: 0x00032F8D
		[PXDBText(IsUnicode = true)]
		[PXUIField(DisplayName = "Tooltip", Visibility = 7)]
		[PXUIVisible(typeof(Where<ALModel.modelType, NotIn3<ALModelType.snippet, ALModelType.printerSetup>>))]
		[PXUIEnabled(typeof(Where<ALModel.modelType, NotIn3<ALModelType.snippet, ALModelType.printerSetup>>))]
		public virtual string Tooltip { get; set; }

		// Token: 0x170004FC RID: 1276
		// (get) Token: 0x06000ED3 RID: 3795 RVA: 0x00034D96 File Offset: 0x00032F96
		public Guid? DrivenRuleID
		{
			get
			{
				return this.FilterRuleID;
			}
		}

		// Token: 0x170004FD RID: 1277
		// (get) Token: 0x06000ED4 RID: 3796 RVA: 0x00034D9E File Offset: 0x00032F9E
		public bool? ReverseRule
		{
			get
			{
				return this.ReverseFilter;
			}
		}

		// Token: 0x170004FE RID: 1278
		// (get) Token: 0x06000ED5 RID: 3797 RVA: 0x00034DA6 File Offset: 0x00032FA6
		// (set) Token: 0x06000ED6 RID: 3798 RVA: 0x00034DAE File Offset: 0x00032FAE
		[PXDBBool]
		[PXDefault(false, PersistingCheck = 2)]
		[PXUIField(DisplayName = "Not", Visibility = 7)]
		[PXUIVisible(typeof(Where<ALModel.modelType, Equal<ALModelType.single>>))]
		[PXUIEnabled(typeof(Where<ALModel.modelType, Equal<ALModelType.single>, And<ALModel.screenID, IsNotNull>>))]
		public virtual bool? ReverseFilter { get; set; }

		// Token: 0x170004FF RID: 1279
		// (get) Token: 0x06000ED7 RID: 3799 RVA: 0x00034DB7 File Offset: 0x00032FB7
		// (set) Token: 0x06000ED8 RID: 3800 RVA: 0x00034DBF File Offset: 0x00032FBF
		[ALRuleIDForeign(typeof(Where<ALRule.screenID, Equal<Current<ALModel.screenID>>>), DisplayName = "Enabled when", Visibility = 7)]
		[PXForeignReference(typeof(ALModel.FK.FilterRule))]
		[PXUIVisible(typeof(Where<ALModel.modelType, Equal<ALModelType.single>>))]
		[PXUIEnabled(typeof(Where<ALModel.modelType, Equal<ALModelType.single>, And<ALModel.screenID, IsNotNull>>))]
		public virtual Guid? FilterRuleID { get; set; }

		// Token: 0x17000500 RID: 1280
		// (get) Token: 0x06000ED9 RID: 3801 RVA: 0x00034DC8 File Offset: 0x00032FC8
		// (set) Token: 0x06000EDA RID: 3802 RVA: 0x00034DD0 File Offset: 0x00032FD0
		[PXDBBool]
		[PXDefault(false, PersistingCheck = 2)]
		[PXUIField(DisplayName = "Hide Instead")]
		[PXUIVisible(typeof(Where<ALModel.modelType, Equal<ALModelType.single>>))]
		[PXUIEnabled(typeof(Where<ALModel.modelType, Equal<ALModelType.single>, And<ALModel.screenID, IsNotNull>>))]
		public virtual bool? HideInstead { get; set; }

		// Token: 0x17000501 RID: 1281
		// (get) Token: 0x06000EDB RID: 3803 RVA: 0x00034DD9 File Offset: 0x00032FD9
		// (set) Token: 0x06000EDC RID: 3804 RVA: 0x00034DE1 File Offset: 0x00032FE1
		[PXDBBool]
		[PXDefault(false, PersistingCheck = 2)]
		[PXUIField(DisplayName = "Not", Visibility = 7)]
		[PXUIVisible(typeof(Where<ALModel.modelType, In3<ALModelType.single, ALModelType.snippet>>))]
		[PXUIEnabled(typeof(Where<ALModel.modelType, In3<ALModelType.single, ALModelType.snippet>, And<ALModel.screenID, IsNotNull>>))]
		public virtual bool? ReversePrint { get; set; }

		// Token: 0x17000502 RID: 1282
		// (get) Token: 0x06000EDD RID: 3805 RVA: 0x00034DEA File Offset: 0x00032FEA
		// (set) Token: 0x06000EDE RID: 3806 RVA: 0x00034DF2 File Offset: 0x00032FF2
		[ALRuleIDForeign(typeof(Where<ALRule.screenID, Equal<Current<ALModel.screenID>>>), DisplayName = "Prints when", Visibility = 7)]
		[PXForeignReference(typeof(ALModel.FK.PrintRule))]
		[PXDefault(typeof(ALModel.filterRuleID), PersistingCheck = 2)]
		[PXUIVisible(typeof(Where<ALModel.modelType, In3<ALModelType.single, ALModelType.snippet>>))]
		[PXUIEnabled(typeof(Where<ALModel.modelType, In3<ALModelType.single, ALModelType.snippet>, And<ALModel.screenID, IsNotNull>>))]
		public virtual Guid? PrintRuleID { get; set; }

		// Token: 0x17000503 RID: 1283
		// (get) Token: 0x06000EDF RID: 3807 RVA: 0x00034DFB File Offset: 0x00032FFB
		// (set) Token: 0x06000EE0 RID: 3808 RVA: 0x00034E03 File Offset: 0x00033003
		[ALLanguage]
		[PXUIVisible(typeof(Where<ALModel.modelType, NotIn3<ALModelType.group, ALModelType.printerSetup, ALModelType.snippet>>))]
		[PXUIEnabled(typeof(Where<ALModel.modelType, NotIn3<ALModelType.group, ALModelType.printerSetup, ALModelType.snippet>>))]
		public virtual string Language { get; set; }

		// Token: 0x17000504 RID: 1284
		// (get) Token: 0x06000EE1 RID: 3809 RVA: 0x00034E0C File Offset: 0x0003300C
		// (set) Token: 0x06000EE2 RID: 3810 RVA: 0x00034E14 File Offset: 0x00033014
		[PXDBInt]
		[PXUIField(DisplayName = "Encoding")]
		[ALEncoding.ALListAttribute]
		[PXDefault(PersistingCheck = 2)]
		[PXUIVisible(typeof(Where<ALModel.language, In3<ZplLanguage.value, EzpLanguage.value>, And<ALModel.modelType, Equal<ALModelType.single>>>))]
		[PXUIEnabled(typeof(Where<ALModel.language, In3<ZplLanguage.value, EzpLanguage.value>, And<ALModel.modelType, Equal<ALModelType.single>>>))]
		public virtual int? Encoding { get; set; }

		// Token: 0x17000505 RID: 1285
		// (get) Token: 0x06000EE3 RID: 3811 RVA: 0x00034E1D File Offset: 0x0003301D
		// (set) Token: 0x06000EE4 RID: 3812 RVA: 0x00034E25 File Offset: 0x00033025
		[PXDBString(1, IsFixed = true)]
		[PXDefault("P")]
		[PXUIField(DisplayName = "Position Type")]
		[PXUIVisible(typeof(Where<ALModel.modelType, Equal<ALModelType.single>>))]
		[PXUIEnabled(typeof(Where<ALModel.modelType, Equal<ALModelType.single>>))]
		[ALLayoutType.ALListAttribute]
		public virtual string LayoutType { get; set; }

		// Token: 0x17000506 RID: 1286
		// (get) Token: 0x06000EE5 RID: 3813 RVA: 0x00034E2E File Offset: 0x0003302E
		// (set) Token: 0x06000EE6 RID: 3814 RVA: 0x00034E36 File Offset: 0x00033036
		[PXDBText(IsUnicode = true)]
		[PXDefault(PersistingCheck = 2)]
		[PXUIField(DisplayName = "Template")]
		[PXUIVisible(typeof(Where<ALModel.modelType, NotIn3<ALModelType.group, ALModelType.printerSetup>>))]
		[PXUIEnabled(typeof(Where<True, Equal<False>>))]
		[Obsolete]
		public virtual string Body { get; set; }

		// Token: 0x17000507 RID: 1287
		// (get) Token: 0x06000EE7 RID: 3815 RVA: 0x00034E3F File Offset: 0x0003303F
		// (set) Token: 0x06000EE8 RID: 3816 RVA: 0x00034E47 File Offset: 0x00033047
		[PXDBText(IsUnicode = true)]
		[PXDefault(PersistingCheck = 2)]
		[PXUIField(DisplayName = "Rendered", IsReadOnly = true)]
		public virtual string Rendered { get; set; }

		// Token: 0x17000508 RID: 1288
		// (get) Token: 0x06000EE9 RID: 3817 RVA: 0x00034E50 File Offset: 0x00033050
		// (set) Token: 0x06000EEA RID: 3818 RVA: 0x00034E58 File Offset: 0x00033058
		[PXDBText(IsUnicode = true)]
		[PXDefault(PersistingCheck = 2)]
		[PXUIField(DisplayName = "Warnings", Enabled = false)]
		[PXUIVisible(typeof(Where<ALModel.modelType, NotIn3<ALModelType.group, ALModelType.printerSetup>>))]
		[PXUIEnabled(typeof(Where<ALModel.modelType, NotIn3<ALModelType.group, ALModelType.printerSetup>>))]
		public virtual string Message { get; set; }

		// Token: 0x17000509 RID: 1289
		// (get) Token: 0x06000EEB RID: 3819 RVA: 0x00034E61 File Offset: 0x00033061
		// (set) Token: 0x06000EEC RID: 3820 RVA: 0x00034E69 File Offset: 0x00033069
		[PXDBBool]
		[PXDefault(false, PersistingCheck = 2)]
		[PXUIField(DisplayName = "Dealing Mode")]
		[PXUIVisible(typeof(Where<ALModel.modelType, Equal<ALModelType.single>>))]
		[PXUIEnabled(typeof(Where<ALModel.modelType, Equal<ALModelType.single>>))]
		public virtual bool? DealingMode { get; set; }

		// Token: 0x1700050A RID: 1290
		// (get) Token: 0x06000EED RID: 3821 RVA: 0x00034E72 File Offset: 0x00033072
		// (set) Token: 0x06000EEE RID: 3822 RVA: 0x00034E7A File Offset: 0x0003307A
		[ALExprValue(DisplayName = "Dealing Count Expr.")]
		[PXUIVisible(typeof(Where<ALModel.dealingMode, Equal<True>>))]
		[PXUIEnabled(typeof(Where<ALModel.dealingMode, Equal<True>>))]
		[PXUIRequired(typeof(Where<ALModel.dealingMode, Equal<True>>))]
		public virtual string DealingCountExpr { get; set; }

		// Token: 0x1700050B RID: 1291
		// (get) Token: 0x06000EEF RID: 3823 RVA: 0x00034E83 File Offset: 0x00033083
		// (set) Token: 0x06000EF0 RID: 3824 RVA: 0x00034E8B File Offset: 0x0003308B
		[ALExprValue(DisplayName = "Nb. of Copies Expr.")]
		[PXUIVisible(typeof(Where<ALModel.modelType, Equal<ALModelType.single>>))]
		[PXUIEnabled(typeof(Where<ALModel.modelType, Equal<ALModelType.single>>))]
		public virtual string NbCopiesExpr { get; set; }

		// Token: 0x1700050C RID: 1292
		// (get) Token: 0x06000EF1 RID: 3825 RVA: 0x00034E94 File Offset: 0x00033094
		// (set) Token: 0x06000EF2 RID: 3826 RVA: 0x00034E9C File Offset: 0x0003309C
		[PXDBDecimal(4, MinValue = 0.0)]
		[PXUIField(DisplayName = "Move By")]
		public virtual decimal? MoveBy { get; set; }

		// Token: 0x1700050D RID: 1293
		// (get) Token: 0x06000EF3 RID: 3827 RVA: 0x00034EA5 File Offset: 0x000330A5
		// (set) Token: 0x06000EF4 RID: 3828 RVA: 0x00034EAD File Offset: 0x000330AD
		[ALSizeUnit]
		[ALSizeUnit.ALListWithDotAttribute]
		[PXDefault("MM", PersistingCheck = 2)]
		public virtual string SizeUnit { get; set; }

		// Token: 0x1700050E RID: 1294
		// (get) Token: 0x06000EF5 RID: 3829 RVA: 0x00034EB6 File Offset: 0x000330B6
		// (set) Token: 0x06000EF6 RID: 3830 RVA: 0x00034EBE File Offset: 0x000330BE
		[PXDBInt(MinValue = 0)]
		[PXDefault(0, PersistingCheck = 2)]
		[PXUIField(DisplayName = "Pause every X row(s)")]
		[PXUIVisible(typeof(Where<ALModel.modelType, In3<ALModelType.single, ALModelType.group>>))]
		[PXUIEnabled(typeof(Where<ALModel.modelType, In3<ALModelType.single, ALModelType.group>>))]
		public virtual int? SendPauseEvery { get; set; }

		// Token: 0x1700050F RID: 1295
		// (get) Token: 0x06000EF7 RID: 3831 RVA: 0x00034EC7 File Offset: 0x000330C7
		// (set) Token: 0x06000EF8 RID: 3832 RVA: 0x00034ECF File Offset: 0x000330CF
		[ALPrintMode.ALListAttribute]
		public virtual string PrintMode { get; set; }

		// Token: 0x17000510 RID: 1296
		// (get) Token: 0x06000EF9 RID: 3833 RVA: 0x00034ED8 File Offset: 0x000330D8
		// (set) Token: 0x06000EFA RID: 3834 RVA: 0x00034EE0 File Offset: 0x000330E0
		[ALNumberingSequence(DisplayName = "Numbering Sequence")]
		[PXForeignReference(typeof(ALModel.FK.CSNumbering))]
		[PXDefault("", PersistingCheck = 2)]
		[PXUIVisible(typeof(Where<ALModel.modelType, Equal<ALModelType.single>>))]
		[PXUIEnabled(typeof(Where<ALModel.modelType, Equal<ALModelType.single>>))]
		public virtual string NumberingID { get; set; }

		// Token: 0x17000511 RID: 1297
		// (get) Token: 0x06000EFB RID: 3835 RVA: 0x00034EE9 File Offset: 0x000330E9
		// (set) Token: 0x06000EFC RID: 3836 RVA: 0x00034EF1 File Offset: 0x000330F1
		[ALOnOtherDensity]
		[PXDefault("PA", PersistingCheck = 2)]
		[PXUIVisible(typeof(Where<ALModel.modelType, Equal<ALModelType.single>>))]
		[PXUIEnabled(typeof(Where<ALModel.modelType, Equal<ALModelType.single>>))]
		public virtual string PrintOnOtherDensity { get; set; }

		// Token: 0x17000512 RID: 1298
		// (get) Token: 0x06000EFD RID: 3837 RVA: 0x00034EFA File Offset: 0x000330FA
		// (set) Token: 0x06000EFE RID: 3838 RVA: 0x00034F02 File Offset: 0x00033102
		[PXString(IsUnicode = true)]
		[PXUIField(DisplayName = "Action Name", IsReadOnly = true)]
		[PXFormula(typeof(ALModelActionName<ALModel.labelID>))]
		public virtual string ActionName { get; set; }

		// Token: 0x17000513 RID: 1299
		// (get) Token: 0x06000EFF RID: 3839 RVA: 0x00034F0B File Offset: 0x0003310B
		// (set) Token: 0x06000F00 RID: 3840 RVA: 0x00034F13 File Offset: 0x00033113
		[PXDBString(64, InputMask = "", IsUnicode = true)]
		[PXUIField(DisplayName = "Trigger On Field")]
		[PXStringList]
		[PXDefault(PersistingCheck = 2)]
		[PXUIVisible(typeof(Where<True, Equal<False>>))]
		public virtual string TriggerField { get; set; }

		// Token: 0x17000514 RID: 1300
		// (get) Token: 0x06000F01 RID: 3841 RVA: 0x00034F1C File Offset: 0x0003311C
		// (set) Token: 0x06000F02 RID: 3842 RVA: 0x00034F24 File Offset: 0x00033124
		[PXDBString(1, IsFixed = true)]
		[PXUIField(DisplayName = "Trigger On Value")]
		[PXStringList]
		[PXUIEnabled(typeof(Where<ALModel.triggerField, IsNotNull>))]
		[PXDefault(PersistingCheck = 2)]
		[PXUIVisible(typeof(Where<True, Equal<False>>))]
		public virtual string TriggerValue { get; set; }

		// Token: 0x17000515 RID: 1301
		// (get) Token: 0x06000F03 RID: 3843 RVA: 0x00034F2D File Offset: 0x0003312D
		// (set) Token: 0x06000F04 RID: 3844 RVA: 0x00034F35 File Offset: 0x00033135
		[PXDBGuid(false)]
		[PXUIField(DisplayName = "Trigger Generic Inquiry", IsReadOnly = true)]
		[PXSelector(typeof(Search<GIDesign.designID>), DescriptionField = typeof(GIDesign.name), SubstituteKey = typeof(GIDesign.name))]
		[PXForeignReference(typeof(ALModel.FK.Design), 2)]
		[PXDefault(PersistingCheck = 2)]
		[PXUIVisible(typeof(Where<True, Equal<False>>))]
		public Guid? TriggerDesignID { get; set; }

		// Token: 0x17000516 RID: 1302
		// (get) Token: 0x06000F05 RID: 3845 RVA: 0x00034F3E File Offset: 0x0003313E
		// (set) Token: 0x06000F06 RID: 3846 RVA: 0x00034F46 File Offset: 0x00033146
		[PXDBGuid(false)]
		[PXUIField(DisplayName = "Trigger Data Provider", IsReadOnly = true)]
		[PXSelector(typeof(Search<SYProvider.providerID>), DescriptionField = typeof(SYProvider.name), SubstituteKey = typeof(SYProvider.name))]
		[PXForeignReference(typeof(ALModel.FK.Provider), 2)]
		[PXDefault(PersistingCheck = 2)]
		[PXUIVisible(typeof(Where<True, Equal<False>>))]
		public Guid? TriggerProviderID { get; set; }

		// Token: 0x17000517 RID: 1303
		// (get) Token: 0x06000F07 RID: 3847 RVA: 0x00034F4F File Offset: 0x0003314F
		// (set) Token: 0x06000F08 RID: 3848 RVA: 0x00034F57 File Offset: 0x00033157
		[PXDBGuid(false)]
		[PXUIField(DisplayName = "Trigger Import Scenario", IsReadOnly = true)]
		[PXSelector(typeof(Search<SYMapping.mappingID>), DescriptionField = typeof(SYMapping.name), SubstituteKey = typeof(SYMapping.name))]
		[PXForeignReference(typeof(ALModel.FK.Mapping), 2)]
		[PXDefault(PersistingCheck = 2)]
		[PXUIVisible(typeof(Where<True, Equal<False>>))]
		public Guid? TriggerMappingID { get; set; }

		// Token: 0x17000518 RID: 1304
		// (get) Token: 0x06000F09 RID: 3849 RVA: 0x00034F60 File Offset: 0x00033160
		// (set) Token: 0x06000F0A RID: 3850 RVA: 0x00034F68 File Offset: 0x00033168
		[PXDBGuid(false)]
		[PXUIField(DisplayName = "Trigger Business Event", IsReadOnly = true)]
		[PXSelector(typeof(Search<BPEvent.eventID>), DescriptionField = typeof(BPEvent.name), SubstituteKey = typeof(BPEvent.name))]
		[PXForeignReference(typeof(ALModel.FK.Event), 2)]
		[PXDefault(PersistingCheck = 2)]
		[PXUIVisible(typeof(Where<True, Equal<False>>))]
		public Guid? TriggerEventID { get; set; }

		// Token: 0x17000519 RID: 1305
		// (get) Token: 0x06000F0B RID: 3851 RVA: 0x00034F71 File Offset: 0x00033171
		// (set) Token: 0x06000F0C RID: 3852 RVA: 0x00034F79 File Offset: 0x00033179
		[PXNote(DescriptionField = typeof(ALModel.description))]
		public virtual Guid? NoteID { get; set; }

		// Token: 0x1700051A RID: 1306
		// (get) Token: 0x06000F0D RID: 3853 RVA: 0x00034F82 File Offset: 0x00033182
		// (set) Token: 0x06000F0E RID: 3854 RVA: 0x00034F8A File Offset: 0x0003318A
		[PXDBCreatedByID]
		public virtual Guid? CreatedByID { get; set; }

		// Token: 0x1700051B RID: 1307
		// (get) Token: 0x06000F0F RID: 3855 RVA: 0x00034F93 File Offset: 0x00033193
		// (set) Token: 0x06000F10 RID: 3856 RVA: 0x00034F9B File Offset: 0x0003319B
		[PXDBCreatedByScreenID]
		public virtual string CreatedByScreenID { get; set; }

		// Token: 0x1700051C RID: 1308
		// (get) Token: 0x06000F11 RID: 3857 RVA: 0x00034FA4 File Offset: 0x000331A4
		// (set) Token: 0x06000F12 RID: 3858 RVA: 0x00034FAC File Offset: 0x000331AC
		[PXDBCreatedDateTime]
		[PXUIField(DisplayName = "Created On", Enabled = false, IsReadOnly = true)]
		public virtual DateTime? CreatedDateTime { get; set; }

		// Token: 0x1700051D RID: 1309
		// (get) Token: 0x06000F13 RID: 3859 RVA: 0x00034FB5 File Offset: 0x000331B5
		// (set) Token: 0x06000F14 RID: 3860 RVA: 0x00034FBD File Offset: 0x000331BD
		[PXDBLastModifiedByID]
		public virtual Guid? LastModifiedByID { get; set; }

		// Token: 0x1700051E RID: 1310
		// (get) Token: 0x06000F15 RID: 3861 RVA: 0x00034FC6 File Offset: 0x000331C6
		// (set) Token: 0x06000F16 RID: 3862 RVA: 0x00034FCE File Offset: 0x000331CE
		[PXDBLastModifiedByScreenID]
		public virtual string LastModifiedByScreenID { get; set; }

		// Token: 0x1700051F RID: 1311
		// (get) Token: 0x06000F17 RID: 3863 RVA: 0x00034FD7 File Offset: 0x000331D7
		// (set) Token: 0x06000F18 RID: 3864 RVA: 0x00034FDF File Offset: 0x000331DF
		[PXDBLastModifiedDateTime]
		[PXUIField(DisplayName = "Last Modified On", Enabled = false, IsReadOnly = true)]
		public virtual DateTime? LastModifiedDateTime { get; set; }

		// Token: 0x17000520 RID: 1312
		// (get) Token: 0x06000F19 RID: 3865 RVA: 0x00034FE8 File Offset: 0x000331E8
		// (set) Token: 0x06000F1A RID: 3866 RVA: 0x00034FF0 File Offset: 0x000331F0
		[PXDBTimestamp]
		public virtual byte[] tstamp { get; set; }

		// Token: 0x04000594 RID: 1428
		public const string ENABLED_WHEN = "Enabled when";

		// Token: 0x04000595 RID: 1429
		public const string PRINTS_WHEN = "Prints when";

		// Token: 0x02000778 RID: 1912
		public class PK : PrimaryKeyOf<ALModel>.By<ALModel.labelID>
		{
			// Token: 0x060025DD RID: 9693 RVA: 0x00079BF8 File Offset: 0x00077DF8
			public static ALModel Find(PXGraph graph, Guid? labelID)
			{
				return PrimaryKeyOf<ALModel>.By<ALModel.labelID>.FindBy(graph, labelID, 0);
			}
		}

		// Token: 0x02000779 RID: 1913
		public static class FK
		{
			// Token: 0x02000B99 RID: 2969
			public class Format : PrimaryKeyOf<ALFormat>.By<ALFormat.formatID>.ForeignKeyOf<ALModel>.By<ALModel.formatID>
			{
			}

			// Token: 0x02000B9A RID: 2970
			public class PortalMap : PrimaryKeyOf<PX.SM.PortalMap>.By<PX.SM.PortalMap.nodeID>.ForeignKeyOf<ALModel>.By<ALModel.screenID>
			{
			}

			// Token: 0x02000B9B RID: 2971
			public class SiteMap : PrimaryKeyOf<PX.SM.SiteMap>.By<PX.SM.SiteMap.nodeID>.ForeignKeyOf<ALModel>.By<ALModel.screenID>
			{
			}

			// Token: 0x02000B9C RID: 2972
			public class FilterRule : PrimaryKeyOf<ALRule>.By<ALRule.ruleID>.ForeignKeyOf<ALModel>.By<ALModel.filterRuleID>
			{
			}

			// Token: 0x02000B9D RID: 2973
			public class PrintRule : PrimaryKeyOf<ALRule>.By<ALRule.ruleID>.ForeignKeyOf<ALModel>.By<ALModel.printRuleID>
			{
			}

			// Token: 0x02000B9E RID: 2974
			public class Margin : PrimaryKeyOf<ALMargin>.By<ALMargin.marginID>.ForeignKeyOf<ALModel>.By<ALModel.marginID>
			{
			}

			// Token: 0x02000B9F RID: 2975
			public class Category : PrimaryKeyOf<ALCategory>.By<ALCategory.categoryID>.ForeignKeyOf<ALModel>.By<ALModel.categoryID>
			{
			}

			// Token: 0x02000BA0 RID: 2976
			public class CSNumbering : PrimaryKeyOf<Numbering>.By<Numbering.numberingID>.ForeignKeyOf<ALModel>.By<ALModel.numberingID>
			{
			}

			// Token: 0x02000BA1 RID: 2977
			public class Design : PrimaryKeyOf<GIDesign>.By<GIDesign.designID>.ForeignKeyOf<ALModel>.By<ALModel.triggerDesignID>
			{
			}

			// Token: 0x02000BA2 RID: 2978
			public class Provider : PrimaryKeyOf<SYProvider>.By<SYProvider.providerID>.ForeignKeyOf<ALModel>.By<ALModel.triggerProviderID>
			{
			}

			// Token: 0x02000BA3 RID: 2979
			public class Mapping : PrimaryKeyOf<SYMapping>.By<SYMapping.mappingID>.ForeignKeyOf<ALModel>.By<ALModel.triggerMappingID>
			{
			}

			// Token: 0x02000BA4 RID: 2980
			public class Event : PrimaryKeyOf<BPEvent>.By<BPEvent.eventID>.ForeignKeyOf<ALModel>.By<ALModel.triggerEventID>
			{
			}
		}

		// Token: 0x0200077A RID: 1914
		public abstract class labelID : BqlType<IBqlGuid, Guid>.Field<ALModel.labelID>
		{
		}

		// Token: 0x0200077B RID: 1915
		public abstract class active : BqlType<IBqlBool, bool>.Field<ALModel.active>
		{
		}

		// Token: 0x0200077C RID: 1916
		public abstract class isSystem : BqlType<IBqlBool, bool>.Field<ALModel.isSystem>
		{
		}

		// Token: 0x0200077D RID: 1917
		public abstract class allowExport : BqlType<IBqlBool, bool>.Field<ALModel.allowExport>
		{
		}

		// Token: 0x0200077E RID: 1918
		public abstract class name : BqlType<IBqlString, string>.Field<ALModel.name>
		{
		}

		// Token: 0x0200077F RID: 1919
		public abstract class description : BqlType<IBqlString, string>.Field<ALModel.description>
		{
		}

		// Token: 0x02000780 RID: 1920
		public abstract class modelType : BqlType<IBqlString, string>.Field<ALModel.modelType>
		{
		}

		// Token: 0x02000781 RID: 1921
		public abstract class categoryID : BqlType<IBqlGuid, Guid>.Field<ALModel.categoryID>
		{
		}

		// Token: 0x02000782 RID: 1922
		public abstract class screenID : BqlType<IBqlString, string>.Field<ALModel.screenID>
		{
		}

		// Token: 0x02000783 RID: 1923
		public abstract class graphType : BqlType<IBqlString, string>.Field<ALModel.graphType>
		{
		}

		// Token: 0x02000784 RID: 1924
		public abstract class basedOnView : BqlType<IBqlString, string>.Field<ALModel.basedOnView>
		{
		}

		// Token: 0x02000785 RID: 1925
		public abstract class cloudID : BqlType<IBqlString, string>.Field<ALModel.cloudID>
		{
		}

		// Token: 0x02000786 RID: 1926
		public abstract class formatID : BqlType<IBqlGuid, Guid>.Field<ALModel.formatID>
		{
		}

		// Token: 0x02000787 RID: 1927
		public abstract class marginID : BqlType<IBqlGuid, Guid>.Field<ALModel.marginID>
		{
		}

		// Token: 0x02000788 RID: 1928
		public abstract class imageUrl : BqlType<IBqlString, string>.Field<ALModel.imageUrl>
		{
		}

		// Token: 0x02000789 RID: 1929
		public abstract class showTemplate : BqlType<IBqlBool, bool>.Field<ALModel.showTemplate>
		{
		}

		// Token: 0x0200078A RID: 1930
		public abstract class showExprs : BqlType<IBqlBool, bool>.Field<ALModel.showExprs>
		{
		}

		// Token: 0x0200078B RID: 1931
		public abstract class showRendered : BqlType<IBqlBool, bool>.Field<ALModel.showRendered>
		{
		}

		// Token: 0x0200078C RID: 1932
		public abstract class showSetup : BqlType<IBqlBool, bool>.Field<ALModel.showSetup>
		{
		}

		// Token: 0x0200078D RID: 1933
		public abstract class showPrinters : BqlType<IBqlBool, bool>.Field<ALModel.showPrinters>
		{
		}

		// Token: 0x0200078E RID: 1934
		public abstract class showPrintLog : BqlType<IBqlBool, bool>.Field<ALModel.showPrintLog>
		{
		}

		// Token: 0x0200078F RID: 1935
		public abstract class showAutomation : BqlType<IBqlBool, bool>.Field<ALModel.showAutomation>
		{
		}

		// Token: 0x02000790 RID: 1936
		public abstract class showChildren : BqlType<IBqlBool, bool>.Field<ALModel.showChildren>
		{
		}

		// Token: 0x02000791 RID: 1937
		public abstract class showUsedBy : BqlType<IBqlBool, bool>.Field<ALModel.showUsedBy>
		{
		}

		// Token: 0x02000792 RID: 1938
		public abstract class hideWhenInGroup : BqlType<IBqlBool, bool>.Field<ALModel.hideWhenInGroup>
		{
		}

		// Token: 0x02000793 RID: 1939
		public abstract class ignoreRotationOnRender : BqlType<IBqlBool, bool>.Field<ALModel.ignoreRotationOnRender>
		{
		}

		// Token: 0x02000794 RID: 1940
		public abstract class tooltip : BqlType<IBqlString, string>.Field<ALModel.tooltip>
		{
		}

		// Token: 0x02000795 RID: 1941
		public abstract class reverseFilter : BqlType<IBqlBool, bool>.Field<ALModel.reverseFilter>
		{
		}

		// Token: 0x02000796 RID: 1942
		public abstract class filterRuleID : BqlType<IBqlGuid, Guid>.Field<ALModel.filterRuleID>
		{
		}

		// Token: 0x02000797 RID: 1943
		public abstract class hideInstead : BqlType<IBqlBool, bool>.Field<ALModel.hideInstead>
		{
		}

		// Token: 0x02000798 RID: 1944
		public abstract class reversePrint : BqlType<IBqlBool, bool>.Field<ALModel.reversePrint>
		{
		}

		// Token: 0x02000799 RID: 1945
		public abstract class printRuleID : BqlType<IBqlGuid, Guid>.Field<ALModel.printRuleID>
		{
		}

		// Token: 0x0200079A RID: 1946
		public abstract class language : BqlType<IBqlString, string>.Field<ALModel.language>
		{
		}

		// Token: 0x0200079B RID: 1947
		public abstract class encoding : BqlType<IBqlInt, int>.Field<ALModel.encoding>
		{
		}

		// Token: 0x0200079C RID: 1948
		public abstract class layoutType : BqlType<IBqlString, string>.Field<ALModel.layoutType>
		{
		}

		// Token: 0x0200079D RID: 1949
		public abstract class body : BqlType<IBqlString, string>.Field<ALModel.body>
		{
		}

		// Token: 0x0200079E RID: 1950
		public abstract class rendered : BqlType<IBqlString, string>.Field<ALModel.rendered>
		{
		}

		// Token: 0x0200079F RID: 1951
		public abstract class message : BqlType<IBqlString, string>.Field<ALModel.message>
		{
		}

		// Token: 0x020007A0 RID: 1952
		public abstract class dealingMode : BqlType<IBqlBool, bool>.Field<ALModel.dealingMode>
		{
		}

		// Token: 0x020007A1 RID: 1953
		public abstract class dealingCountExpr : BqlType<IBqlString, string>.Field<ALModel.dealingCountExpr>
		{
		}

		// Token: 0x020007A2 RID: 1954
		public abstract class nbCopiesExpr : BqlType<IBqlString, string>.Field<ALModel.nbCopiesExpr>
		{
		}

		// Token: 0x020007A3 RID: 1955
		public abstract class moveBy : BqlType<IBqlDecimal, decimal>.Field<ALModel.moveBy>
		{
		}

		// Token: 0x020007A4 RID: 1956
		public abstract class sizeUnit : BqlType<IBqlString, string>.Field<ALModel.sizeUnit>
		{
		}

		// Token: 0x020007A5 RID: 1957
		public abstract class sendPauseEvery : BqlType<IBqlInt, int>.Field<ALModel.sendPauseEvery>
		{
		}

		// Token: 0x020007A6 RID: 1958
		public abstract class printMode : BqlType<IBqlString, string>.Field<ALModel.printMode>
		{
		}

		// Token: 0x020007A7 RID: 1959
		public abstract class numberingID : BqlType<IBqlString, string>.Field<ALModel.numberingID>
		{
		}

		// Token: 0x020007A8 RID: 1960
		public abstract class printOnOtherDensity : BqlType<IBqlString, string>.Field<ALModel.sizeUnit>
		{
		}

		// Token: 0x020007A9 RID: 1961
		public abstract class actionName : BqlType<IBqlString, string>.Field<ALModel.actionName>
		{
		}

		// Token: 0x020007AA RID: 1962
		public abstract class triggerField : BqlType<IBqlString, string>.Field<ALModel.triggerField>
		{
		}

		// Token: 0x020007AB RID: 1963
		public abstract class triggerValue : BqlType<IBqlString, string>.Field<ALModel.triggerValue>
		{
		}

		// Token: 0x020007AC RID: 1964
		public abstract class triggerDesignID : BqlType<IBqlGuid, Guid>.Field<ALModel.triggerDesignID>
		{
		}

		// Token: 0x020007AD RID: 1965
		public abstract class triggerProviderID : BqlType<IBqlGuid, Guid>.Field<ALModel.triggerProviderID>
		{
		}

		// Token: 0x020007AE RID: 1966
		public abstract class triggerMappingID : BqlType<IBqlGuid, Guid>.Field<ALModel.triggerMappingID>
		{
		}

		// Token: 0x020007AF RID: 1967
		public abstract class triggerEventID : BqlType<IBqlGuid, Guid>.Field<ALModel.triggerEventID>
		{
		}

		// Token: 0x020007B0 RID: 1968
		public abstract class noteID : BqlType<IBqlGuid, Guid>.Field<ALModel.noteID>
		{
		}

		// Token: 0x020007B1 RID: 1969
		public abstract class createdByID : BqlType<IBqlGuid, Guid>.Field<ALModel.createdByID>
		{
		}

		// Token: 0x020007B2 RID: 1970
		public abstract class createdByScreenID : BqlType<IBqlString, string>.Field<ALModel.createdByScreenID>
		{
		}

		// Token: 0x020007B3 RID: 1971
		public abstract class createdDateTime : BqlType<IBqlDateTime, DateTime>.Field<ALModel.createdDateTime>
		{
		}

		// Token: 0x020007B4 RID: 1972
		public abstract class lastModifiedByID : BqlType<IBqlGuid, Guid>.Field<ALModel.lastModifiedByID>
		{
		}

		// Token: 0x020007B5 RID: 1973
		public abstract class lastModifiedByScreenID : BqlType<IBqlString, string>.Field<ALModel.lastModifiedByScreenID>
		{
		}

		// Token: 0x020007B6 RID: 1974
		public abstract class lastModifiedDateTime : BqlType<IBqlDateTime, DateTime>.Field<ALModel.lastModifiedDateTime>
		{
		}

		// Token: 0x020007B7 RID: 1975
		public abstract class Tstamp : BqlType<IBqlByteArray, byte[]>.Field<ALModel.Tstamp>
		{
		}
	}
}
