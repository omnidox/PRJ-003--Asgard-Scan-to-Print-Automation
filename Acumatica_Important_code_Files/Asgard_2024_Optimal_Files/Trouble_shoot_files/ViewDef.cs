using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using PX.Data;
using PX.Objects.CS;

namespace AA.Objects.AL
{
	// Token: 0x020001D2 RID: 466
	[DebuggerDisplay("ViewDef: {GraphType.Name}.{InternalName} ({DisplayName})")]
	public sealed class ViewDef : IEquatable<ViewDef>, IEqualityComparer<ViewDef>
	{
		// Token: 0x17000673 RID: 1651
		// (get) Token: 0x060013DC RID: 5084 RVA: 0x00046535 File Offset: 0x00044735
		public Type GraphType { get; }

		// Token: 0x17000674 RID: 1652
		// (get) Token: 0x060013DD RID: 5085 RVA: 0x0004653D File Offset: 0x0004473D
		public string InternalName { get; }

		// Token: 0x17000675 RID: 1653
		// (get) Token: 0x060013DE RID: 5086 RVA: 0x00046545 File Offset: 0x00044745
		public string DisplayName { get; }

		// Token: 0x17000676 RID: 1654
		// (get) Token: 0x060013DF RID: 5087 RVA: 0x0004654D File Offset: 0x0004474D
		public string ExternalName { get; }

		// Token: 0x17000677 RID: 1655
		// (get) Token: 0x060013E0 RID: 5088 RVA: 0x00046555 File Offset: 0x00044755
		public string ItemTypeName
		{
			get
			{
				Type itemType = this.ItemType;
				return (itemType != null) ? itemType.FullName : null;
			}
		}

		// Token: 0x17000678 RID: 1656
		// (get) Token: 0x060013E1 RID: 5089 RVA: 0x00046569 File Offset: 0x00044769
		public Type ItemType { get; }

		// Token: 0x17000679 RID: 1657
		// (get) Token: 0x060013E2 RID: 5090 RVA: 0x00046571 File Offset: 0x00044771
		public Type[] ItemTypes { get; }

		// Token: 0x1700067A RID: 1658
		// (get) Token: 0x060013E3 RID: 5091 RVA: 0x00046579 File Offset: 0x00044779
		public bool IsUsable { get; }

		// Token: 0x1700067B RID: 1659
		// (get) Token: 0x060013E4 RID: 5092 RVA: 0x00046581 File Offset: 0x00044781
		public string FullName { get; }

		// Token: 0x1700067C RID: 1660
		// (get) Token: 0x060013E5 RID: 5093 RVA: 0x00046589 File Offset: 0x00044789
		public BqlCommand BqlSelect { get; }

		// Token: 0x1700067D RID: 1661
		// (get) Token: 0x060013E6 RID: 5094 RVA: 0x00046591 File Offset: 0x00044791
		// (set) Token: 0x060013E7 RID: 5095 RVA: 0x00046599 File Offset: 0x00044799
		public string DependsOn { get; set; }

		// Token: 0x1700067E RID: 1662
		// (get) Token: 0x060013E8 RID: 5096 RVA: 0x000465A2 File Offset: 0x000447A2
		// (set) Token: 0x060013E9 RID: 5097 RVA: 0x000465AA File Offset: 0x000447AA
		public bool? Detail { get; set; }

		// Token: 0x1700067F RID: 1663
		// (get) Token: 0x060013EA RID: 5098 RVA: 0x000465B3 File Offset: 0x000447B3
		public IList<ViewDependency> Dependencies { get; } = new List<ViewDependency>();

		// Token: 0x060013EB RID: 5099 RVA: 0x000465BC File Offset: 0x000447BC
		public ViewDef(PXGraph _graph, PXView pxView)
		{
			try
			{
				bool flag = string.IsNullOrEmpty((pxView != null) ? pxView.Name : null);
				if (flag)
				{
					throw new PXException("A view with a view name is required");
				}
				this.InternalName = pxView.Name;
				this.DisplayName = (ViewUtils.GetDisplayName(pxView) ?? this.InternalName);
				this.ItemType = _graph.GetItemType(this.InternalName);
				if (this.ItemType == null)
				{
					this.ItemType = pxView.GetItemType();
				}
				this.ItemTypes = pxView.GetItemTypes();
				this.IsUsable = this.CheckIsUsable(pxView);
				this.GraphType = GraphHelper.GetType(_graph.GetType().FullName);
				this.BqlSelect = pxView.BqlSelect;
				this.Detail = new bool?(false);
				Type bqlTarget = pxView.BqlTarget;
				Regex dependency_REGEX = ViewDef.DEPENDENCY_REGEX;
				BqlCommand bqlSelect = this.BqlSelect;
				MatchCollection matchCollection = dependency_REGEX.Matches((bqlSelect != null) ? bqlSelect.ToString() : null);
				bool flag2 = matchCollection.Count > 0;
				if (flag2)
				{
					for (int i = 0; i < matchCollection.Count; i++)
					{
						Match match = matchCollection[i];
						GroupCollection groups = match.Groups;
						string value = groups["dependencyType"].Value;
						string value2 = groups["itemType"].Value;
						string value3 = groups["fieldName"].Value;
						this.Add(value, value2, value3);
					}
				}
				bool flag3 = this.ItemType == typeof(CSAnswers);
				if (flag3)
				{
					this.Detail = new bool?(true);
					bool flag4 = bqlTarget != null && bqlTarget.IsGenericType && bqlTarget.GenericTypeArguments.Length != 0;
					if (flag4)
					{
						this.Add("Optional", bqlTarget.GenericTypeArguments[0].FullName, "NoteID");
					}
				}
				if (this.ItemType == null)
				{
					this.ItemType = ViewUtils.GetItemType(_graph, this.InternalName);
				}
				this.FullName = ViewUtils.GetViewName(this, null, true);
			}
			catch (Exception ex)
			{
				PXTrace.WriteError(ex);
				throw;
			}
		}

		// Token: 0x060013EC RID: 5100 RVA: 0x000467F4 File Offset: 0x000449F4
		private bool CheckIsUsable(PXView view)
		{
			bool flag = !ViewUtils.IsUsable(view);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				Type itemType = this.ItemType;
				bool flag2 = itemType == null || !itemType.IsCompatibleWith(typeof(IBqlTable));
				result = !flag2;
			}
			return result;
		}

		// Token: 0x060013ED RID: 5101 RVA: 0x00046841 File Offset: 0x00044A41
		public void Add(string dependType, string itemType, string fieldName)
		{
			this.Add(new ViewDependency(dependType, itemType, fieldName));
		}

		// Token: 0x060013EE RID: 5102 RVA: 0x00046853 File Offset: 0x00044A53
		public void Add(ViewDependency dependency)
		{
			this.Dependencies.Add(dependency);
		}

		// Token: 0x060013EF RID: 5103 RVA: 0x00046864 File Offset: 0x00044A64
		public bool HasAtLeastOneDependency()
		{
			return (from dep in this.Dependencies
			select dep.ItemTypeName).Distinct<string>().Count<string>() > 0;
		}

		// Token: 0x060013F0 RID: 5104 RVA: 0x000468B0 File Offset: 0x00044AB0
		public string GetFirstDependency()
		{
			return (from dep in this.Dependencies
			select dep.ItemTypeName).Distinct<string>().FirstOrDefault<string>();
		}

		// Token: 0x060013F1 RID: 5105 RVA: 0x000468F8 File Offset: 0x00044AF8
		public IEnumerable<string> GetDependencies()
		{
			return (from dep in this.Dependencies
			select dep.ItemTypeName).Distinct<string>();
		}

		// Token: 0x060013F2 RID: 5106 RVA: 0x0004693C File Offset: 0x00044B3C
		public int GetDependencyCount()
		{
			return (from dep in this.Dependencies
			select dep.ItemTypeName).Distinct<string>().Count<string>();
		}

		// Token: 0x060013F3 RID: 5107 RVA: 0x00046984 File Offset: 0x00044B84
		public override bool Equals(object obj)
		{
			return this.Equals(obj as ViewDef);
		}

		// Token: 0x060013F4 RID: 5108 RVA: 0x000469A4 File Offset: 0x00044BA4
		public bool Equals(ViewDef other)
		{
			return other != null && EqualityComparer<bool?>.Default.Equals(this.Detail, other.Detail) && this.FullName == other.FullName;
		}

		// Token: 0x060013F5 RID: 5109 RVA: 0x000469E8 File Offset: 0x00044BE8
		public override int GetHashCode()
		{
			int num = 219003723;
			num = num * -1521134295 + EqualityComparer<bool?>.Default.GetHashCode(this.Detail);
			return num * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.FullName);
		}

		// Token: 0x060013F6 RID: 5110 RVA: 0x00046A34 File Offset: 0x00044C34
		public bool Equals(ViewDef x, ViewDef y)
		{
			return x.Equals(y);
		}

		// Token: 0x060013F7 RID: 5111 RVA: 0x00046A50 File Offset: 0x00044C50
		public int GetHashCode(ViewDef obj)
		{
			return obj.GetHashCode();
		}

		// Token: 0x04000890 RID: 2192
		private static readonly Regex DEPENDENCY_REGEX = new Regex("\\[PX\\.Data\\.(?<dependencyType>Current|Optional)`1\\[(?<itemType>[A-Za-z0-9.]+)\\+(?<fieldName>[A-Za-z0-9]+)\\]\\]");

		// Token: 0x04000891 RID: 2193
		private const string DEPEND_TYPE = "dependencyType";

		// Token: 0x04000892 RID: 2194
		private const string ITEM_TYPE = "itemType";

		// Token: 0x04000893 RID: 2195
		private const string FIELD_NAME = "fieldName";

		// Token: 0x04000894 RID: 2196
		private const string TYPE_CHARS = "[A-Za-z0-9.]+";

		// Token: 0x04000895 RID: 2197
		private const string FIELD_CHARS = "[A-Za-z0-9]+";
	}
}
