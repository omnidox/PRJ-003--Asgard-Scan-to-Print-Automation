using System;
using System.Collections;
using PX.Data;
using PX.Objects.SO;

namespace AA.Objects.Labels.Integration.NbCopies
{
	// Token: 0x020001F5 RID: 501
	public class ALNbCopiesSOShipmentEntryExt : PXGraphExtension<SOShipmentEntry>
	{
		// Token: 0x06001449 RID: 5193 RVA: 0x00043389 File Offset: 0x00041589
		public static bool IsActive()
		{
			return ALSetupSlot.CopiesOverrideSH || ALSetupSlot.CopiesOverrideSP;
		}

		// Token: 0x0600144A RID: 5194 RVA: 0x0004339C File Offset: 0x0004159C
		[PXProcessButton]
		[PXUIField(DisplayName = "Toggle Label Print")]
		public virtual IEnumerable aLToggleSelected(PXAdapter adapter)
		{
			NbCopiesHelper.ToggleLabelPrint<SOShipLine>(base.Base.Transactions);
			return adapter.Get();
		}

		// Token: 0x0600144B RID: 5195 RVA: 0x000433C8 File Offset: 0x000415C8
		[PXProcessButton]
		[PXUIField(DisplayName = "Toggle Label Print")]
		public virtual IEnumerable aLToggleSelectedSP(PXAdapter adapter)
		{
			NbCopiesHelper.ToggleLabelPrint<SOPackageDetailEx>(base.Base.Packages);
			return adapter.Get();
		}

		// Token: 0x0400082F RID: 2095
		public PXSetup<ALSetup> ALSetup;

		// Token: 0x04000830 RID: 2096
		public PXAction<SOShipment> ALToggleSelected;

		// Token: 0x04000831 RID: 2097
		public PXAction<SOShipment> ALToggleSelectedSP;
	}
}
