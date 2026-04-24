using System;
using System.Collections.Generic;
using System.Linq;
using PX.Data;

namespace AA.Objects.Core
{
	// Token: 0x02000027 RID: 39
	public class ViewResult : IViewResult
	{
		// Token: 0x06000144 RID: 324 RVA: 0x00006E57 File Offset: 0x00005057
		public ViewResult(ViewDef viewDef, PXGraph graph) : this(viewDef, graph, false)
		{
		}

		// Token: 0x06000145 RID: 325 RVA: 0x00006E64 File Offset: 0x00005064
		private ViewResult(ViewDef viewDef, PXGraph graph, bool delayed) : this(viewDef, graph, delayed ? (() => ViewUtils.ViewSelect(graph, viewDef.InternalName)) : ViewUtils.ViewSelect(graph, viewDef.InternalName))
		{
		}

		// Token: 0x06000146 RID: 326 RVA: 0x00006EC0 File Offset: 0x000050C0
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

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x06000147 RID: 327 RVA: 0x00006F7C File Offset: 0x0000517C
		public string FullName { get; }

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x06000148 RID: 328 RVA: 0x00006F84 File Offset: 0x00005184
		public string InternalName { get; }

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x06000149 RID: 329 RVA: 0x00006F8C File Offset: 0x0000518C
		public ViewDef ViewDef { get; }

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x0600014A RID: 330 RVA: 0x00006F94 File Offset: 0x00005194
		public PXGraph Graph { get; }

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x0600014B RID: 331 RVA: 0x00006F9C File Offset: 0x0000519C
		public int TableCount
		{
			get
			{
				return this.ItemTypes.Count;
			}
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x0600014C RID: 332 RVA: 0x00006FA9 File Offset: 0x000051A9
		public IList<Type> ItemTypes { get; }

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x0600014D RID: 333 RVA: 0x00006FB1 File Offset: 0x000051B1
		public IList<PXCache> Caches { get; }

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x0600014E RID: 334 RVA: 0x00006FB9 File Offset: 0x000051B9
		public object Result { get; }

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x0600014F RID: 335 RVA: 0x00006FC1 File Offset: 0x000051C1
		public bool Detail { get; }

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x06000150 RID: 336 RVA: 0x00006FC9 File Offset: 0x000051C9
		public bool HasExtensions
		{
			get
			{
				return this.ItemTypes.SelectMany((Type it) => this.GetExtensions(it)).Any<Type>();
			}
		}

		// Token: 0x06000151 RID: 337 RVA: 0x00006FE8 File Offset: 0x000051E8
		public Type GetItemType(int tableNo)
		{
			return this.ItemTypes.Skip(tableNo).FirstOrDefault<Type>();
		}

		// Token: 0x06000152 RID: 338 RVA: 0x0000700C File Offset: 0x0000520C
		public PXCache GetCache(int tableNo)
		{
			return this.Caches.Skip(tableNo).FirstOrDefault<PXCache>();
		}

		// Token: 0x06000153 RID: 339 RVA: 0x00007030 File Offset: 0x00005230
		public IList<Type> GetExtensions(int tableNo)
		{
			return this.GetExtensions(this.GetItemType(tableNo));
		}

		// Token: 0x06000154 RID: 340 RVA: 0x00007050 File Offset: 0x00005250
		public IList<Type> GetExtensions(Type itemType)
		{
			return ViewUtils.GetExtensions(this.Graph, itemType);
		}
	}
}
