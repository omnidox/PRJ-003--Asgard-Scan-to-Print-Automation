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
using PX.SM;

namespace AA.Objects.Labels
{
	// Token: 0x0200010A RID: 266
	[PXPrimaryGraph(typeof(ALRuleMaint))]
	[PXCacheName("Rule")]
	[DebuggerDisplay("{Name} ({Description})")]
	[Serializable]
	public class ALRule : PXBqlTable, IBqlTable, IBqlTableSystemDataStorage, IRule, IParent, IRenderableConfig, IALMaster, INotable, IExportable, IAcuScreenBased
	{
		// Token: 0x17000494 RID: 1172
		// (get) Token: 0x06000B0E RID: 2830 RVA: 0x0001EBD7 File Offset: 0x0001CDD7
		// (set) Token: 0x06000B0F RID: 2831 RVA: 0x0001EBDF File Offset: 0x0001CDDF
		[ALGuidID]
		public virtual Guid? RuleID { get; set; }

		// Token: 0x17000495 RID: 1173
		// (get) Token: 0x06000B10 RID: 2832 RVA: 0x0001EBE8 File Offset: 0x0001CDE8
		[BsonElement]
		public Guid? ID
		{
			get
			{
				return this.RuleID;
			}
		}

		// Token: 0x17000496 RID: 1174
		// (get) Token: 0x06000B11 RID: 2833 RVA: 0x0001EBF0 File Offset: 0x0001CDF0
		// (set) Token: 0x06000B12 RID: 2834 RVA: 0x0001EBF8 File Offset: 0x0001CDF8
		[ALActive]
		public virtual bool? Active { get; set; }

		// Token: 0x17000497 RID: 1175
		// (get) Token: 0x06000B13 RID: 2835 RVA: 0x0001EC01 File Offset: 0x0001CE01
		// (set) Token: 0x06000B14 RID: 2836 RVA: 0x0001EC09 File Offset: 0x0001CE09
		[ALSystem]
		public virtual bool? IsSystem { get; set; }

		// Token: 0x17000498 RID: 1176
		// (get) Token: 0x06000B15 RID: 2837 RVA: 0x0001EC12 File Offset: 0x0001CE12
		// (set) Token: 0x06000B16 RID: 2838 RVA: 0x0001EC1A File Offset: 0x0001CE1A
		[ALExport]
		public virtual bool? AllowExport { get; set; }

		// Token: 0x17000499 RID: 1177
		// (get) Token: 0x06000B17 RID: 2839 RVA: 0x0001EC23 File Offset: 0x0001CE23
		// (set) Token: 0x06000B18 RID: 2840 RVA: 0x0001EC2B File Offset: 0x0001CE2B
		[ALComposite]
		public virtual bool? IsComposite { get; set; }

		// Token: 0x1700049A RID: 1178
		// (get) Token: 0x06000B19 RID: 2841 RVA: 0x0001EC34 File Offset: 0x0001CE34
		// (set) Token: 0x06000B1A RID: 2842 RVA: 0x0001EC3C File Offset: 0x0001CE3C
		[ALName(typeof(ALRule.name), typeof(ALRule.description), 50, true, IsKey = true, DisplayName = "Rule ID")]
		public virtual string Name { get; set; }

		// Token: 0x1700049B RID: 1179
		// (get) Token: 0x06000B1B RID: 2843 RVA: 0x0001EC45 File Offset: 0x0001CE45
		// (set) Token: 0x06000B1C RID: 2844 RVA: 0x0001EC4D File Offset: 0x0001CE4D
		[ALDescription]
		[PXFieldDescription]
		public virtual string Description { get; set; }

		// Token: 0x1700049C RID: 1180
		// (get) Token: 0x06000B1D RID: 2845 RVA: 0x0001EC56 File Offset: 0x0001CE56
		// (set) Token: 0x06000B1E RID: 2846 RVA: 0x0001EC5E File Offset: 0x0001CE5E
		[ALCategoryIDForeign]
		[PXDefault(typeof(ALSetup.defaultCategoryID), PersistingCheck = 2)]
		[PXForeignReference(typeof(ALRule.FK.Category))]
		public virtual Guid? CategoryID { get; set; }

		// Token: 0x1700049D RID: 1181
		// (get) Token: 0x06000B1F RID: 2847 RVA: 0x0001EC67 File Offset: 0x0001CE67
		// (set) Token: 0x06000B20 RID: 2848 RVA: 0x0001EC6F File Offset: 0x0001CE6F
		[ALScreenID]
		[PXForeignReference(typeof(ALRule.FK.PortalMap))]
		[PXForeignReference(typeof(ALRule.FK.SiteMap))]
		public virtual string ScreenID { get; set; }

		// Token: 0x1700049E RID: 1182
		// (get) Token: 0x06000B21 RID: 2849 RVA: 0x0001EC78 File Offset: 0x0001CE78
		// (set) Token: 0x06000B22 RID: 2850 RVA: 0x0001EC80 File Offset: 0x0001CE80
		[ALGraphType(typeof(ALRule.screenID))]
		[PXDefault]
		public virtual string GraphType { get; set; }

		// Token: 0x1700049F RID: 1183
		// (get) Token: 0x06000B23 RID: 2851 RVA: 0x0001EC89 File Offset: 0x0001CE89
		// (set) Token: 0x06000B24 RID: 2852 RVA: 0x0001EC91 File Offset: 0x0001CE91
		[ALExprValue]
		[PXUIEnabled(typeof(Where<ALRule.isComposite, NotEqual<True>>))]
		public virtual string Expression { get; set; }

		// Token: 0x170004A0 RID: 1184
		// (get) Token: 0x06000B25 RID: 2853 RVA: 0x0001EC9C File Offset: 0x0001CE9C
		[PXBool]
		[PXUIField(Visibility = 3)]
		public virtual bool? ShowChildren
		{
			[PXDependsOnFields(new Type[]
			{
				typeof(ALRule.isComposite)
			})]
			get
			{
				return new bool?(this.IsComposite.GetValueOrDefault());
			}
		}

		// Token: 0x170004A1 RID: 1185
		// (get) Token: 0x06000B26 RID: 2854 RVA: 0x0001ECC4 File Offset: 0x0001CEC4
		[PXBool]
		[PXUIField(Visibility = 3)]
		public virtual bool? ShowExpr
		{
			[PXDependsOnFields(new Type[]
			{
				typeof(ALRule.showChildren)
			})]
			get
			{
				return !this.ShowChildren;
			}
		}

		// Token: 0x170004A2 RID: 1186
		// (get) Token: 0x06000B27 RID: 2855 RVA: 0x0001ED00 File Offset: 0x0001CF00
		[PXBool]
		[PXUIField(Visibility = 3)]
		public virtual bool? ShowUsedBy
		{
			get
			{
				return new bool?(true);
			}
		}

		// Token: 0x170004A3 RID: 1187
		// (get) Token: 0x06000B28 RID: 2856 RVA: 0x0001ED18 File Offset: 0x0001CF18
		// (set) Token: 0x06000B29 RID: 2857 RVA: 0x0001ED20 File Offset: 0x0001CF20
		[PXNote]
		public virtual Guid? NoteID { get; set; }

		// Token: 0x170004A4 RID: 1188
		// (get) Token: 0x06000B2A RID: 2858 RVA: 0x0001ED29 File Offset: 0x0001CF29
		// (set) Token: 0x06000B2B RID: 2859 RVA: 0x0001ED31 File Offset: 0x0001CF31
		[PXDBCreatedByID]
		public virtual Guid? CreatedByID { get; set; }

		// Token: 0x170004A5 RID: 1189
		// (get) Token: 0x06000B2C RID: 2860 RVA: 0x0001ED3A File Offset: 0x0001CF3A
		// (set) Token: 0x06000B2D RID: 2861 RVA: 0x0001ED42 File Offset: 0x0001CF42
		[PXDBCreatedByScreenID]
		[PXUIField(DisplayName = "Screen ID", Enabled = false)]
		public virtual string CreatedByScreenID { get; set; }

		// Token: 0x170004A6 RID: 1190
		// (get) Token: 0x06000B2E RID: 2862 RVA: 0x0001ED4B File Offset: 0x0001CF4B
		// (set) Token: 0x06000B2F RID: 2863 RVA: 0x0001ED53 File Offset: 0x0001CF53
		[PXDBCreatedDateTime]
		[PXUIField(DisplayName = "Created On", Enabled = false)]
		public virtual DateTime? CreatedDateTime { get; set; }

		// Token: 0x170004A7 RID: 1191
		// (get) Token: 0x06000B30 RID: 2864 RVA: 0x0001ED5C File Offset: 0x0001CF5C
		// (set) Token: 0x06000B31 RID: 2865 RVA: 0x0001ED64 File Offset: 0x0001CF64
		[PXDBLastModifiedByID]
		public virtual Guid? LastModifiedByID { get; set; }

		// Token: 0x170004A8 RID: 1192
		// (get) Token: 0x06000B32 RID: 2866 RVA: 0x0001ED6D File Offset: 0x0001CF6D
		// (set) Token: 0x06000B33 RID: 2867 RVA: 0x0001ED75 File Offset: 0x0001CF75
		[PXDBLastModifiedByScreenID]
		public virtual string LastModifiedByScreenID { get; set; }

		// Token: 0x170004A9 RID: 1193
		// (get) Token: 0x06000B34 RID: 2868 RVA: 0x0001ED7E File Offset: 0x0001CF7E
		// (set) Token: 0x06000B35 RID: 2869 RVA: 0x0001ED86 File Offset: 0x0001CF86
		[PXDBLastModifiedDateTime]
		[PXUIField(DisplayName = "Last Modified On", Enabled = false)]
		public virtual DateTime? LastModifiedDateTime { get; set; }

		// Token: 0x170004AA RID: 1194
		// (get) Token: 0x06000B36 RID: 2870 RVA: 0x0001ED8F File Offset: 0x0001CF8F
		// (set) Token: 0x06000B37 RID: 2871 RVA: 0x0001ED97 File Offset: 0x0001CF97
		[PXDBTimestamp]
		public virtual byte[] tstamp { get; set; }

		// Token: 0x02000678 RID: 1656
		public class PK : PrimaryKeyOf<ALRule>.By<ALRule.ruleID>
		{
			// Token: 0x06001D1B RID: 7451 RVA: 0x00058BF3 File Offset: 0x00056DF3
			public static ALRule Find(PXGraph graph, Guid? ruleID)
			{
				return PrimaryKeyOf<ALRule>.By<ALRule.ruleID>.FindBy(graph, ruleID, 0);
			}
		}

		// Token: 0x02000679 RID: 1657
		public static class FK
		{
			// Token: 0x02000A0D RID: 2573
			public class PortalMap : PrimaryKeyOf<PX.SM.PortalMap>.By<PX.SM.PortalMap.nodeID>.ForeignKeyOf<ALRule>.By<ALRule.screenID>
			{
			}

			// Token: 0x02000A0E RID: 2574
			public class SiteMap : PrimaryKeyOf<PX.SM.SiteMap>.By<PX.SM.SiteMap.nodeID>.ForeignKeyOf<ALRule>.By<ALRule.screenID>
			{
			}

			// Token: 0x02000A0F RID: 2575
			public class Category : PrimaryKeyOf<ALCategory>.By<ALCategory.categoryID>.ForeignKeyOf<ALRule>.By<ALRule.categoryID>
			{
			}
		}

		// Token: 0x0200067A RID: 1658
		[Nullable(new byte[]
		{
			0,
			1,
			0
		})]
		public abstract class ruleID : BqlType<IBqlGuid, Guid>.Field<ALRule.ruleID>
		{
		}

		// Token: 0x0200067B RID: 1659
		[Nullable(new byte[]
		{
			0,
			1,
			0
		})]
		public abstract class active : BqlType<IBqlBool, bool>.Field<ALRule.active>
		{
		}

		// Token: 0x0200067C RID: 1660
		[Nullable(new byte[]
		{
			0,
			1,
			0
		})]
		public abstract class isSystem : BqlType<IBqlBool, bool>.Field<ALRule.isSystem>
		{
		}

		// Token: 0x0200067D RID: 1661
		[Nullable(new byte[]
		{
			0,
			1,
			0
		})]
		public abstract class allowExport : BqlType<IBqlBool, bool>.Field<ALRule.allowExport>
		{
		}

		// Token: 0x0200067E RID: 1662
		[Nullable(new byte[]
		{
			0,
			1,
			0
		})]
		public abstract class isComposite : BqlType<IBqlBool, bool>.Field<ALRule.isComposite>
		{
		}

		// Token: 0x0200067F RID: 1663
		[Nullable(new byte[]
		{
			0,
			1,
			1,
			0
		})]
		public abstract class name : BqlType<IBqlString, string>.Field<ALRule.name>
		{
		}

		// Token: 0x02000680 RID: 1664
		[Nullable(new byte[]
		{
			0,
			1,
			1,
			0
		})]
		public abstract class description : BqlType<IBqlString, string>.Field<ALRule.description>
		{
		}

		// Token: 0x02000681 RID: 1665
		[Nullable(new byte[]
		{
			0,
			1,
			0
		})]
		public abstract class categoryID : BqlType<IBqlGuid, Guid>.Field<ALRule.categoryID>
		{
		}

		// Token: 0x02000682 RID: 1666
		[Nullable(new byte[]
		{
			0,
			1,
			1,
			0
		})]
		public abstract class screenID : BqlType<IBqlString, string>.Field<ALRule.screenID>
		{
		}

		// Token: 0x02000683 RID: 1667
		[Nullable(new byte[]
		{
			0,
			1,
			1,
			0
		})]
		public abstract class graphType : BqlType<IBqlString, string>.Field<ALRule.graphType>
		{
		}

		// Token: 0x02000684 RID: 1668
		[Nullable(new byte[]
		{
			0,
			1,
			1,
			0
		})]
		public abstract class expression : BqlType<IBqlString, string>.Field<ALRule.expression>
		{
		}

		// Token: 0x02000685 RID: 1669
		[Nullable(new byte[]
		{
			0,
			1,
			0
		})]
		public abstract class showChildren : BqlType<IBqlBool, bool>.Field<ALRule.showChildren>
		{
		}

		// Token: 0x02000686 RID: 1670
		[Nullable(new byte[]
		{
			0,
			1,
			0
		})]
		public abstract class showExpr : BqlType<IBqlBool, bool>.Field<ALRule.showExpr>
		{
		}

		// Token: 0x02000687 RID: 1671
		[Nullable(new byte[]
		{
			0,
			1,
			0
		})]
		public abstract class showUsedBy : BqlType<IBqlBool, bool>.Field<ALRule.showUsedBy>
		{
		}

		// Token: 0x02000688 RID: 1672
		[Nullable(new byte[]
		{
			0,
			1,
			0
		})]
		public abstract class noteID : BqlType<IBqlGuid, Guid>.Field<ALRule.noteID>
		{
		}

		// Token: 0x02000689 RID: 1673
		[Nullable(new byte[]
		{
			0,
			1,
			0
		})]
		public abstract class createdByID : BqlType<IBqlGuid, Guid>.Field<ALRule.createdByID>
		{
		}

		// Token: 0x0200068A RID: 1674
		[Nullable(new byte[]
		{
			0,
			1,
			1,
			0
		})]
		public abstract class createdByScreenID : BqlType<IBqlString, string>.Field<ALRule.createdByScreenID>
		{
		}

		// Token: 0x0200068B RID: 1675
		[Nullable(new byte[]
		{
			0,
			1,
			0
		})]
		public abstract class createdDateTime : BqlType<IBqlDateTime, DateTime>.Field<ALRule.createdDateTime>
		{
		}

		// Token: 0x0200068C RID: 1676
		[Nullable(new byte[]
		{
			0,
			1,
			0
		})]
		public abstract class lastModifiedByID : BqlType<IBqlGuid, Guid>.Field<ALRule.lastModifiedByID>
		{
		}

		// Token: 0x0200068D RID: 1677
		[Nullable(new byte[]
		{
			0,
			1,
			1,
			0
		})]
		public abstract class lastModifiedByScreenID : BqlType<IBqlString, string>.Field<ALRule.lastModifiedByScreenID>
		{
		}

		// Token: 0x0200068E RID: 1678
		[Nullable(new byte[]
		{
			0,
			1,
			0
		})]
		public abstract class lastModifiedDateTime : BqlType<IBqlDateTime, DateTime>.Field<ALRule.lastModifiedDateTime>
		{
		}

		// Token: 0x0200068F RID: 1679
		[Nullable(new byte[]
		{
			0,
			1,
			1,
			0
		})]
		public abstract class Tstamp : BqlType<IBqlByteArray, byte[]>.Field<ALRule.Tstamp>
		{
		}
	}
}
