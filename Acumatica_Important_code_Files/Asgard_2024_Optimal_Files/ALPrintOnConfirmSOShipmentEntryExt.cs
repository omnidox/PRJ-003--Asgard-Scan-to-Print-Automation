using System;
using AA.Objects.AL.License;
using PX.Data;
using PX.Data.BQL;
using PX.Objects.SO;

namespace AA.Objects.AL.Integration.AutoPrint
{
	// Token: 0x020002A3 RID: 675
	public class ALPrintOnConfirmSOShipmentEntryExt : PXGraphExtension<SOShipmentEntry>
	{
		// Token: 0x060019CA RID: 6602 RVA: 0x0005EB4E File Offset: 0x0005CD4E
		public static bool IsActive()
		{
			return ALSetupSlot.PrintOnConfirm;
		}

		// Token: 0x1700081E RID: 2078
		// (get) Token: 0x060019CB RID: 6603 RVA: 0x0005EB55 File Offset: 0x0005CD55
		// (set) Token: 0x060019CC RID: 6604 RVA: 0x0005EB5D File Offset: 0x0005CD5D
		[InjectDependency]
		private IALLicenseManager _licenseManager { get; set; }

		// Token: 0x1700081F RID: 2079
		// (get) Token: 0x060019CD RID: 6605 RVA: 0x0005EB66 File Offset: 0x0005CD66
		// (set) Token: 0x060019CE RID: 6606 RVA: 0x0005EB6E File Offset: 0x0005CD6E
		[InjectDependency]
		private ILabelGenerator _labelGenerator { get; set; }

		// Token: 0x060019CF RID: 6607 RVA: 0x0005EB78 File Offset: 0x0005CD78
		[PXOverride]
		public virtual void MarkConfirmed(SOShipment shipment, Action<SOShipment> baseMethod)
		{
			if (baseMethod != null)
			{
				baseMethod(shipment);
			}
			Guid? printOnConfirmModelID = ALSetupSlot.PrintOnConfirmModelID;
			Models.Model model;
			Models.TryGetModelByID(printOnConfirmModelID, out model);
			bool flag = model == null;
			if (!flag)
			{
				try
				{
					LabelContext labelContext = LabelContext.CreatePrintContext(base.Base.GetType(), shipment, printOnConfirmModelID, false, null);
					labelContext.IsSilent = true;
					PrintResults printResults = this._labelGenerator.PrintLabels(labelContext);
					this._licenseManager.UpdateFeatureConsumption(base.GetType(), printResults.NbLabels);
				}
				catch (Exception ex)
				{
					PXTrace.WriteError(ex);
				}
			}
		}

		// Token: 0x02000A08 RID: 2568
		public abstract class feature : BqlType<IBqlBool, bool>.Operand<ALPrintOnConfirmSOShipmentEntryExt.feature>
		{
		}
	}
}
