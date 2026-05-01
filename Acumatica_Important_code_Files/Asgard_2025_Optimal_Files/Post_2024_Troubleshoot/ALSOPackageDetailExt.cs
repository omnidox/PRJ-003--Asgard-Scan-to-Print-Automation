using System;
using System.Runtime.CompilerServices;
using PX.Data;
using PX.Data.BQL;
using PX.Objects.SO;

namespace AA.Objects.Labels.Integration.FixPackage
{
	// Token: 0x02000202 RID: 514
	public sealed class ALSOPackageDetailExt : PXCacheExtension<SOPackageDetail>
	{
		// Token: 0x06001475 RID: 5237 RVA: 0x000436B6 File Offset: 0x000418B6
		public static bool IsActive()
		{
			return ALSetupSlot.FixPackageLineNbr;
		}

		// Token: 0x170006E8 RID: 1768
		// (get) Token: 0x06001476 RID: 5238 RVA: 0x000436BD File Offset: 0x000418BD
		// (set) Token: 0x06001477 RID: 5239 RVA: 0x000436C5 File Offset: 0x000418C5
		[PXMergeAttributes(Method = 2)]
		[PXCustomizeBaseAttribute(typeof(PXLineNbrAttribute), "ReuseGaps", true)]
		[PXCustomizeBaseAttribute(typeof(PXUIFieldAttribute), "Visible", true)]
		[PXCustomizeBaseAttribute(typeof(PXUIFieldAttribute), "IsReadOnly", true)]
		public int? LineNbr { get; set; }

		// Token: 0x020008FA RID: 2298
		[Nullable(new byte[]
		{
			0,
			1,
			0
		})]
		public abstract class feature : BqlType<IBqlBool, bool>.Operand<ALSOPackageDetailExt.feature>
		{
		}
	}
}
