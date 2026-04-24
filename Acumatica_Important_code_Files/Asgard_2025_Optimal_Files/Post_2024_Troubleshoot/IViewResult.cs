using System;
using System.Collections.Generic;
using PX.Data;

namespace AA.Objects.Core
{
	// Token: 0x0200002C RID: 44
	public interface IViewResult
	{
		// Token: 0x17000062 RID: 98
		// (get) Token: 0x0600019E RID: 414
		string FullName { get; }

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x0600019F RID: 415
		string InternalName { get; }

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x060001A0 RID: 416
		ViewDef ViewDef { get; }

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x060001A1 RID: 417
		PXGraph Graph { get; }

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x060001A2 RID: 418
		IList<Type> ItemTypes { get; }

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x060001A3 RID: 419
		IList<PXCache> Caches { get; }

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x060001A4 RID: 420
		bool HasExtensions { get; }

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x060001A5 RID: 421
		object Result { get; }

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x060001A6 RID: 422
		bool Detail { get; }

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x060001A7 RID: 423
		int TableCount { get; }

		// Token: 0x060001A8 RID: 424
		Type GetItemType(int tableNo);

		// Token: 0x060001A9 RID: 425
		PXCache GetCache(int tableNo);

		// Token: 0x060001AA RID: 426
		IList<Type> GetExtensions(int tableNo);

		// Token: 0x060001AB RID: 427
		IList<Type> GetExtensions(Type itemType);
	}
}
