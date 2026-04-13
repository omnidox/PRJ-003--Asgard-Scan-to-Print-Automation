using System;
using System.Runtime.CompilerServices;
using PX.Data;
using PX.Data.BQL;
using PX.Objects.SO;

namespace AA.Objects.Labels.Integration.NbCopies
{
	// Token: 0x020001E6 RID: 486
	public sealed class ALNbCopiesSOPackageDetailExt : PXCacheExtension<SOPackageDetail>, ILabelOption<SOPackageDetail>, ILabelOption
	{
		// Token: 0x060013F6 RID: 5110 RVA: 0x00042FC5 File Offset: 0x000411C5
		public static bool IsActive()
		{
			return ALSetupSlot.CopiesOverrideSP;
		}

		// Token: 0x170006D4 RID: 1748
		// (get) Token: 0x060013F7 RID: 5111 RVA: 0x00042FCC File Offset: 0x000411CC
		// (set) Token: 0x060013F8 RID: 5112 RVA: 0x00042FD4 File Offset: 0x000411D4
		[ALPrintLabel]
		[PXUIVisible(typeof(ALHasFlag<ALNbCopies.Operands.ShipmentPackage>))]
		public bool? UsrALPrintLabel { get; set; }

		// Token: 0x170006D5 RID: 1749
		// (get) Token: 0x060013F9 RID: 5113 RVA: 0x00042FDD File Offset: 0x000411DD
		// (set) Token: 0x060013FA RID: 5114 RVA: 0x00042FE5 File Offset: 0x000411E5
		[ALNbCopies]
		[PXUIVisible(typeof(ALHasFlag<ALNbCopies.Operands.ShipmentPackage>))]
		public int? UsrALNbrOfCopies { get; set; }

		// Token: 0x170006D6 RID: 1750
		// (get) Token: 0x060013FB RID: 5115 RVA: 0x00042FEE File Offset: 0x000411EE
		// (set) Token: 0x060013FC RID: 5116 RVA: 0x00042FF6 File Offset: 0x000411F6
		[ALQtyOnLabel]
		[PXUIVisible(typeof(ALHasFlag<ALNbCopies.Operands.ShipmentPackage>))]
		public decimal? UsrALLabelQty { get; set; }

		// Token: 0x170006D7 RID: 1751
		// (get) Token: 0x060013FD RID: 5117 RVA: 0x00042FFF File Offset: 0x000411FF
		// (set) Token: 0x060013FE RID: 5118 RVA: 0x00043007 File Offset: 0x00041207
		[ALBoxDescriptor]
		[PXUIVisible(typeof(ALHasFlag<ALNbCopies.Operands.ShipmentPackage>))]
		public string UsrALBoxXofY { get; set; }

		// Token: 0x020008F0 RID: 2288
		[Nullable(new byte[]
		{
			0,
			1,
			0
		})]
		public abstract class feature : BqlType<IBqlBool, bool>.Operand<ALNbCopiesSOPackageDetailExt.feature>
		{
		}

		// Token: 0x020008F1 RID: 2289
		[Nullable(new byte[]
		{
			0,
			1,
			0
		})]
		public abstract class usrALPrintLabel : BqlType<IBqlBool, bool>.Field<ALNbCopiesSOPackageDetailExt.usrALPrintLabel>
		{
		}

		// Token: 0x020008F2 RID: 2290
		[Nullable(new byte[]
		{
			0,
			1,
			0
		})]
		public abstract class usrALNbrOfCopies : BqlType<IBqlInt, int>.Field<ALNbCopiesSOPackageDetailExt.usrALNbrOfCopies>
		{
		}

		// Token: 0x020008F3 RID: 2291
		[Nullable(new byte[]
		{
			0,
			1,
			0
		})]
		public abstract class usrALLabelQty : BqlType<IBqlDecimal, decimal>.Field<ALNbCopiesSOPackageDetailExt.usrALLabelQty>
		{
		}

		// Token: 0x020008F4 RID: 2292
		[Nullable(new byte[]
		{
			0,
			1,
			1,
			0
		})]
		public abstract class usrALBoxXofY : BqlType<IBqlString, string>.Field<ALNbCopiesSOPackageDetailExt.usrALBoxXofY>
		{
		}
	}
}
