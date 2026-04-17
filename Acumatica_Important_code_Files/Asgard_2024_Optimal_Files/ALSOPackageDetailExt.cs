using System;
using PX.Data;
using PX.Data.BQL;
using PX.Objects.SO;

namespace AA.Objects.AL.Integration.FixPackage
{
	// Token: 0x020002A4 RID: 676
	public sealed class ALSOPackageDetailExt : PXCacheExtension<SOPackageDetail>
	{
		// Token: 0x060019D1 RID: 6609 RVA: 0x0005EC14 File Offset: 0x0005CE14
		public static bool IsActive()
		{
			return ALSetupSlot.FixPackageLineNbr;
		}

		// Token: 0x17000820 RID: 2080
		// (get) Token: 0x060019D2 RID: 6610 RVA: 0x0005EC1B File Offset: 0x0005CE1B
		// (set) Token: 0x060019D3 RID: 6611 RVA: 0x0005EC23 File Offset: 0x0005CE23
		[PXMergeAttributes(Method = 2)]
		[PXCustomizeBaseAttribute(typeof(PXLineNbrAttribute), "ReuseGaps", true)]
		[PXCustomizeBaseAttribute(typeof(PXUIFieldAttribute), "Visible", true)]
		[PXCustomizeBaseAttribute(typeof(PXUIFieldAttribute), "IsReadOnly", true)]
		public int? LineNbr { get; set; }

		// Token: 0x02000A09 RID: 2569
		public abstract class feature : BqlType<IBqlBool, bool>.Operand<ALSOPackageDetailExt.feature>
		{
		}
	}
}
