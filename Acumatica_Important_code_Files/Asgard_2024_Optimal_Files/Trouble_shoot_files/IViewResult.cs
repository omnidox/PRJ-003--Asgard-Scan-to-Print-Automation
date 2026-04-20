using System;
using System.Collections.Generic;
using PX.Data;

namespace AA.Objects.AL
{
	// Token: 0x020001E2 RID: 482
	public interface IViewResult
	{
		// Token: 0x170006A7 RID: 1703
		// (get) Token: 0x0600146A RID: 5226
		string FullName { get; }

		// Token: 0x170006A8 RID: 1704
		// (get) Token: 0x0600146B RID: 5227
		string InternalName { get; }

		// Token: 0x170006A9 RID: 1705
		// (get) Token: 0x0600146C RID: 5228
		ViewDef ViewDef { get; }

		// Token: 0x170006AA RID: 1706
		// (get) Token: 0x0600146D RID: 5229
		PXGraph Graph { get; }

		// Token: 0x170006AB RID: 1707
		// (get) Token: 0x0600146E RID: 5230
		IList<Type> ItemTypes { get; }

		// Token: 0x170006AC RID: 1708
		// (get) Token: 0x0600146F RID: 5231
		IList<PXCache> Caches { get; }

		// Token: 0x170006AD RID: 1709
		// (get) Token: 0x06001470 RID: 5232
		bool HasExtensions { get; }

		// Token: 0x170006AE RID: 1710
		// (get) Token: 0x06001471 RID: 5233
		object Result { get; }

		// Token: 0x170006AF RID: 1711
		// (get) Token: 0x06001472 RID: 5234
		bool Detail { get; }

		// Token: 0x170006B0 RID: 1712
		// (get) Token: 0x06001473 RID: 5235
		int TableCount { get; }

		// Token: 0x06001474 RID: 5236
		Type GetItemType(int tableNo);

		// Token: 0x06001475 RID: 5237
		PXCache GetCache(int tableNo);

		// Token: 0x06001476 RID: 5238
		IList<Type> GetExtensions(int tableNo);

		// Token: 0x06001477 RID: 5239
		IList<Type> GetExtensions(Type itemType);
	}
}
