using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using Asgard.Labels.Abstractions.Helpers;
using PX.Data;
using PX.Objects.CS;

namespace AA.Objects.Core
{
	// Token: 0x02000025 RID: 37
	[DebuggerDisplay("ViewDef: {GraphType.Name}.{InternalName} ({DisplayName})")]
	public sealed class ViewDef : IEquatable<ViewDef>, IEqualityComparer<ViewDef>
	{
		// Token: 0x17000039 RID: 57
		// (get) Token: 0x06000120 RID: 288 RVA: 0x000068EE File Offset: 0x00004AEE
		public Type GraphType { get; }

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x06000121 RID: 289 RVA: 0x000068F6 File Offset: 0x00004AF6
		public string InternalName { get; }

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x06000122 RID: 290 RVA: 0x000068FE File Offset: 0x00004AFE
		public string DisplayName { get; }

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x06000123 RID: 291 RVA: 0x00006906 File Offset: 0x00004B06
		public string ExternalName { get; }

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x06000124 RID: 292 RVA: 0x0000690E File Offset: 0x00004B0E
		public string ItemTypeName
		{
			get
			{
				Type itemType = this.ItemType;
				return (itemType != null) ? itemType.FullName : null;
			}
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x06000125 RID: 293 RVA: 0x00006922 File Offset: 0x00004B22
		public Type ItemType { get; }

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x06000126 RID: 294 RVA: 0x0000692A File Offset: 0x00004B2A
		public Type[] ItemTypes { get; }

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x06000127 RID: 295 RVA: 0x00006932 File Offset: 0x00004B32
		public bool IsUsable { get; }

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x06000128 RID: 296 RVA: 0x0000693A File Offset: 0x00004B3A
		public string FullName { get; }

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x06000129 RID: 297 RVA: 0x00006942 File Offset: 0x00004B42
		public BqlCommand BqlSelect { get; }

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x0600012A RID: 298 RVA: 0x0000694A File Offset: 0x00004B4A
		// (set) Token: 0x0600012B RID: 299 RVA: 0x00006952 File Offset: 0x00004B52
		public string DependsOn { get; set; }

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x0600012C RID: 300 RVA: 0x0000695B File Offset: 0x00004B5B
		// (set) Token: 0x0600012D RID: 301 RVA: 0x00006963 File Offset: 0x00004B63
		public bool? Detail { get; set; }

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x0600012E RID: 302 RVA: 0x0000696C File Offset: 0x00004B6C
		public IList<ViewDependency> Dependencies { get; } = new List<ViewDependency>();

		// Token: 0x0600012F RID: 303 RVA: 0x00006974 File Offset: 0x00004B74
		public ViewDef(PXView pxView)
		{
			try
			{
				bool flag = string.IsNullOrEmpty((pxView != null) ? pxView.Name : null);
				if (flag)
				{
					throw new PXException("A view with a view name is required");
				}
				this.InternalName = pxView.Name;
				this.DisplayName = (AsgardCoreUtils.GetDisplayName(pxView) ?? this.InternalName);
				this.ItemType = pxView.GetItemType();
				this.ItemTypes = pxView.GetItemTypes();
				this.IsUsable = this.CheckIsUsable(pxView);
				this.GraphType = GraphHelper.GetType(pxView.Graph.GetType().FullName);
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
					this.ItemType = ViewUtils.GetItemType(pxView.Graph, this.InternalName);
				}
				this.FullName = ViewUtils.GetViewName(this, null, true);
			}
			catch (Exception ex)
			{
				PXTrace.WriteError(ex);
				throw;
			}
		}

		// Token: 0x06000130 RID: 304 RVA: 0x00006B9C File Offset: 0x00004D9C
		private bool CheckIsUsable(PXView view)
		{
			return ViewUtils.IsUsable(view) && this.ItemType.IsCompatibleWith(typeof(IBqlTable));
		}

		// Token: 0x06000131 RID: 305 RVA: 0x00006BCE File Offset: 0x00004DCE
		public void Add(string dependType, string itemType, string fieldName)
		{
			this.Add(new ViewDependency(dependType, itemType, fieldName));
		}

		// Token: 0x06000132 RID: 306 RVA: 0x00006BE0 File Offset: 0x00004DE0
		public void Add(ViewDependency dependency)
		{
			this.Dependencies.Add(dependency);
		}

		// Token: 0x06000133 RID: 307 RVA: 0x00006BF0 File Offset: 0x00004DF0
		public bool HasAtLeastOneDependency()
		{
			return (from dep in this.Dependencies
			select dep.ItemTypeName).Distinct<string>().Count<string>() > 0;
		}

		// Token: 0x06000134 RID: 308 RVA: 0x00006C3C File Offset: 0x00004E3C
		public string GetFirstDependency()
		{
			return (from dep in this.Dependencies
			select dep.ItemTypeName).Distinct<string>().FirstOrDefault<string>();
		}

		// Token: 0x06000135 RID: 309 RVA: 0x00006C84 File Offset: 0x00004E84
		public IEnumerable<string> GetDependencies()
		{
			return (from dep in this.Dependencies
			select dep.ItemTypeName).Distinct<string>();
		}

		// Token: 0x06000136 RID: 310 RVA: 0x00006CC8 File Offset: 0x00004EC8
		public int GetDependencyCount()
		{
			return (from dep in this.Dependencies
			select dep.ItemTypeName).Distinct<string>().Count<string>();
		}

		// Token: 0x06000137 RID: 311 RVA: 0x00006D10 File Offset: 0x00004F10
		public override bool Equals(object obj)
		{
			return this.Equals(obj as ViewDef);
		}

		// Token: 0x06000138 RID: 312 RVA: 0x00006D30 File Offset: 0x00004F30
		public bool Equals(ViewDef other)
		{
			return other != null && EqualityComparer<bool?>.Default.Equals(this.Detail, other.Detail) && this.FullName == other.FullName;
		}

		// Token: 0x06000139 RID: 313 RVA: 0x00006D74 File Offset: 0x00004F74
		public override int GetHashCode()
		{
			int num = 219003723;
			num = num * -1521134295 + EqualityComparer<bool?>.Default.GetHashCode(this.Detail);
			return num * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.FullName);
		}

		// Token: 0x0600013A RID: 314 RVA: 0x00006DC0 File Offset: 0x00004FC0
		public bool Equals(ViewDef x, ViewDef y)
		{
			return x.Equals(y);
		}

		// Token: 0x0600013B RID: 315 RVA: 0x00006DDC File Offset: 0x00004FDC
		public int GetHashCode(ViewDef obj)
		{
			return obj.GetHashCode();
		}

		// Token: 0x04000054 RID: 84
		private static readonly Regex DEPENDENCY_REGEX = new Regex("\\[PX\\.Data\\.(?<dependencyType>Current|Optional)`1\\[(?<itemType>[A-Za-z0-9.]+)\\+(?<fieldName>[A-Za-z0-9]+)\\]\\]");

		// Token: 0x04000055 RID: 85
		private const string DEPEND_TYPE = "dependencyType";

		// Token: 0x04000056 RID: 86
		private const string ITEM_TYPE = "itemType";

		// Token: 0x04000057 RID: 87
		private const string FIELD_NAME = "fieldName";

		// Token: 0x04000058 RID: 88
		private const string TYPE_CHARS = "[A-Za-z0-9.]+";

		// Token: 0x04000059 RID: 89
		private const string FIELD_CHARS = "[A-Za-z0-9]+";
	}
}
