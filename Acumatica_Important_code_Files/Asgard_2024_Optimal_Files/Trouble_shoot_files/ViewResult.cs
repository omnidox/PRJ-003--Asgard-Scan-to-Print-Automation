using System;
using System.Collections.Generic;
using System.Linq;
using PX.Data;

namespace AA.Objects.AL
{
	// Token: 0x020001D4 RID: 468
	public class ViewResult : IViewResult
	{
		// Token: 0x06001400 RID: 5120 RVA: 0x00046ACB File Offset: 0x00044CCB
		public ViewResult(ViewDef viewDef, PXGraph graph) : this(viewDef, graph, false)
		{
		}

		// Token: 0x06001401 RID: 5121 RVA: 0x00046AD8 File Offset: 0x00044CD8
		private ViewResult(ViewDef viewDef, PXGraph graph, bool delayed) : this(viewDef, graph, delayed ? (() => ViewUtils.ViewSelect(graph, viewDef.InternalName)) : ViewUtils.ViewSelect(graph, viewDef.InternalName))
		{
		}

		// Token: 0x06001402 RID: 5122 RVA: 0x00046B34 File Offset: 0x00044D34
		public ViewResult(ViewDef viewDef, PXGraph graph, object result)
		{
			this.ViewDef = viewDef;
			this.InternalName = viewDef.InternalName;
			this.FullName = ViewUtils.GetViewName(viewDef, null, true);
			this.Detail = viewDef.Detail.GetValueOrDefault();
			this.Graph = graph;
			this.Result = result;
			bool flag = this.Result == null && this.Detail;
			if (flag)
			{
				this.Result = new List<object>();
			}
			PXView view = ViewUtils.GetView(graph, this.InternalName, false);
			this.ItemTypes = ViewUtils.GetItemTypes(view, this.Result);
			this.Caches = (from it in this.ItemTypes
			select ViewUtils.GetCache(this.Graph, it)).ToList<PXCache>();
		}

		// Token: 0x17000684 RID: 1668
		// (get) Token: 0x06001403 RID: 5123 RVA: 0x00046BF0 File Offset: 0x00044DF0
		public string FullName { get; }

		// Token: 0x17000685 RID: 1669
		// (get) Token: 0x06001404 RID: 5124 RVA: 0x00046BF8 File Offset: 0x00044DF8
		public string InternalName { get; }

		// Token: 0x17000686 RID: 1670
		// (get) Token: 0x06001405 RID: 5125 RVA: 0x00046C00 File Offset: 0x00044E00
		public ViewDef ViewDef { get; }

		// Token: 0x17000687 RID: 1671
		// (get) Token: 0x06001406 RID: 5126 RVA: 0x00046C08 File Offset: 0x00044E08
		public PXGraph Graph { get; }

		// Token: 0x17000688 RID: 1672
		// (get) Token: 0x06001407 RID: 5127 RVA: 0x00046C10 File Offset: 0x00044E10
		public int TableCount
		{
			get
			{
				return this.ItemTypes.Count;
			}
		}

		// Token: 0x17000689 RID: 1673
		// (get) Token: 0x06001408 RID: 5128 RVA: 0x00046C1D File Offset: 0x00044E1D
		public IList<Type> ItemTypes { get; }

		// Token: 0x1700068A RID: 1674
		// (get) Token: 0x06001409 RID: 5129 RVA: 0x00046C25 File Offset: 0x00044E25
		public IList<PXCache> Caches { get; }

		// Token: 0x1700068B RID: 1675
		// (get) Token: 0x0600140A RID: 5130 RVA: 0x00046C2D File Offset: 0x00044E2D
		public object Result { get; }

		// Token: 0x1700068C RID: 1676
		// (get) Token: 0x0600140B RID: 5131 RVA: 0x00046C35 File Offset: 0x00044E35
		public bool Detail { get; }

		// Token: 0x1700068D RID: 1677
		// (get) Token: 0x0600140C RID: 5132 RVA: 0x00046C3D File Offset: 0x00044E3D
		public bool HasExtensions
		{
			get
			{
				return this.ItemTypes.SelectMany((Type it) => this.GetExtensions(it)).Any<Type>();
			}
		}

		// Token: 0x0600140D RID: 5133 RVA: 0x00046C5C File Offset: 0x00044E5C
		public Type GetItemType(int tableNo)
		{
			return this.ItemTypes.Skip(tableNo).FirstOrDefault<Type>();
		}

		// Token: 0x0600140E RID: 5134 RVA: 0x00046C80 File Offset: 0x00044E80
		public PXCache GetCache(int tableNo)
		{
			return this.Caches.Skip(tableNo).FirstOrDefault<PXCache>();
		}

		// Token: 0x0600140F RID: 5135 RVA: 0x00046CA4 File Offset: 0x00044EA4
		public IList<Type> GetExtensions(int tableNo)
		{
			return this.GetExtensions(this.GetItemType(tableNo));
		}

		// Token: 0x06001410 RID: 5136 RVA: 0x00046CC4 File Offset: 0x00044EC4
		public IList<Type> GetExtensions(Type itemType)
		{
			return ViewUtils.GetExtensions(this.Graph, itemType);
		}
	}
}
