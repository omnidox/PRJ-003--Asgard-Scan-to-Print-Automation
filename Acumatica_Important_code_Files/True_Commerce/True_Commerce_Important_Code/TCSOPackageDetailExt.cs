using System;
using PX.Data;
using PX.Objects.SO;

namespace TCAddon
{
	// Token: 0x02000033 RID: 51
	[PXCacheName("SO Package Detail")]
	public sealed class TCSOPackageDetailExt : PXCacheExtension<SOPackageDetail>
	{
		// Token: 0x17000109 RID: 265
		// (get) Token: 0x06000303 RID: 771 RVA: 0x000148A1 File Offset: 0x00012AA1
		// (set) Token: 0x06000304 RID: 772 RVA: 0x000148A9 File Offset: 0x00012AA9
		[PXDBString(50, IsUnicode = true)]
		[PXUIField(DisplayName = "Pallet ID")]
		public string UsrTCPalletID { get; set; }

		// Token: 0x1700010A RID: 266
		// (get) Token: 0x06000305 RID: 773 RVA: 0x000148B2 File Offset: 0x00012AB2
		// (set) Token: 0x06000306 RID: 774 RVA: 0x000148BA File Offset: 0x00012ABA
		[PXDBString(50, IsUnicode = true)]
		[PXUIField(DisplayName = "GS1-128")]
		public string UsrTCUCC128 { get; set; }

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x06000307 RID: 775 RVA: 0x000148C3 File Offset: 0x00012AC3
		// (set) Token: 0x06000308 RID: 776 RVA: 0x000148CB File Offset: 0x00012ACB
		[PXDBString(50, IsUnicode = true)]
		[PXUIField(DisplayName = "Pallet GS1-128")]
		public string UsrTCUCC128P { get; set; }

		// Token: 0x1700010C RID: 268
		// (get) Token: 0x06000309 RID: 777 RVA: 0x000148D4 File Offset: 0x00012AD4
		// (set) Token: 0x0600030A RID: 778 RVA: 0x000148DC File Offset: 0x00012ADC
		[PXDBString(20, IsUnicode = true)]
		[PXUIField(DisplayName = "Print Status", Enabled = false)]
		public string UsrTCLabelPrintStatus { get; set; }

		// Token: 0x1700010D RID: 269
		// (get) Token: 0x0600030B RID: 779 RVA: 0x000148E5 File Offset: 0x00012AE5
		// (set) Token: 0x0600030C RID: 780 RVA: 0x000148ED File Offset: 0x00012AED
		[PXDBDateAndTime]
		[PXUIField(DisplayName = "Last Print Time", Enabled = false)]
		public DateTime? UsrTCLabelPrintDate { get; set; }

		// Token: 0x1700010E RID: 270
		// (get) Token: 0x0600030D RID: 781 RVA: 0x000148F6 File Offset: 0x00012AF6
		// (set) Token: 0x0600030E RID: 782 RVA: 0x000148FE File Offset: 0x00012AFE
		[PXBool]
		[PXUIField(DisplayName = "Selected", Enabled = true)]
		[PXUnboundDefault(false)]
		public bool? UsrTCSelected { get; set; }

		// Token: 0x0600030F RID: 783 RVA: 0x000023C9 File Offset: 0x000005C9
		public static bool IsActive()
		{
			return true;
		}

		// Token: 0x020001C9 RID: 457
		public abstract class usrTCPalletID : IBqlField, IBqlOperand
		{
		}

		// Token: 0x020001CA RID: 458
		public abstract class usrTCUCC128 : IBqlField, IBqlOperand
		{
		}

		// Token: 0x020001CB RID: 459
		public abstract class usrTCUCC128P : IBqlField, IBqlOperand
		{
		}

		// Token: 0x020001CC RID: 460
		public abstract class usrTCLabelPrintStatus : IBqlField, IBqlOperand
		{
		}

		// Token: 0x020001CD RID: 461
		public abstract class usrTCLabelPrintDate : IBqlField, IBqlOperand
		{
		}

		// Token: 0x020001CE RID: 462
		public abstract class usrTCSelected : IBqlField, IBqlOperand
		{
		}
	}
}
