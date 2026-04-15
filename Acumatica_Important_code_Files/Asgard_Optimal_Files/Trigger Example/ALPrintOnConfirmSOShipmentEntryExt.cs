using System;
using System.Runtime.CompilerServices;
using AA.Objects.License;
using Asgard.Labels.Abstractions.Interface;
using Asgard.Labels.Abstractions.Service;
using Asgard.Labels.Impl.Context;
using Asgard.Labels.Impl.Poco;
using PX.Data;
using PX.Data.BQL;
using PX.Objects.SO;

namespace AA.Objects.Labels.Integration.AutoPrint
{
	// Token: 0x020001CD RID: 461
	public class ALPrintOnConfirmSOShipmentEntryExt : PXGraphExtension<SOShipmentEntry>
	{
		// Token: 0x06001358 RID: 4952 RVA: 0x000429CF File Offset: 0x00040BCF
		public static bool IsActive()
		{
			return ALSetupSlot.IsActive(typeof(SOShipmentEntry)) && ALSetupSlot.PrintOnConfirm;
		}

		// Token: 0x17000693 RID: 1683
		// (get) Token: 0x06001359 RID: 4953 RVA: 0x000429EA File Offset: 0x00040BEA
		// (set) Token: 0x0600135A RID: 4954 RVA: 0x000429F2 File Offset: 0x00040BF2
		[InjectDependency]
		private IALLicenseManager _licenseManager { get; set; }

		// Token: 0x17000694 RID: 1684
		// (get) Token: 0x0600135B RID: 4955 RVA: 0x000429FB File Offset: 0x00040BFB
		// (set) Token: 0x0600135C RID: 4956 RVA: 0x00042A03 File Offset: 0x00040C03
		[InjectDependency]
		private ILabelGenerator<IAcuLabelContext> _labelGenerator { get; set; }

		// Token: 0x17000695 RID: 1685
		// (get) Token: 0x0600135D RID: 4957 RVA: 0x00042A0C File Offset: 0x00040C0C
		// (set) Token: 0x0600135E RID: 4958 RVA: 0x00042A14 File Offset: 0x00040C14
		[InjectDependency]
		private IModelProvider ModelProvider { get; set; }

		// Token: 0x0600135F RID: 4959 RVA: 0x00042A20 File Offset: 0x00040C20
		[PXOverride]
		public virtual void MarkConfirmed(SOShipment shipment, Action<SOShipment> baseMethod)
		{
			if (baseMethod != null)
			{
				baseMethod(shipment);
			}
			Guid? printOnConfirmModelID = ALSetupSlot.PrintOnConfirmModelID;
			IModel model = this.ModelProvider.GetModel(printOnConfirmModelID);
			bool flag = model == null;
			if (!flag)
			{
				try
				{
					AcuLabelContext acuLabelContext = AcuLabelContext.CreatePrintContext(base.Base.GetType(), shipment, printOnConfirmModelID, false, null);
					acuLabelContext.IsSilent = true;
					PrintResults printResults = this._labelGenerator.PrintLabels(acuLabelContext);
					this._licenseManager.UpdateFeatureConsumption(base.GetType(), printResults.NbLabels);
				}
				catch (Exception ex)
				{
					PXTrace.WriteError(ex);
				}
			}
		}

		// Token: 0x020008A0 RID: 2208
		[Nullable(new byte[]
		{
			0,
			1,
			0
		})]
		public abstract class feature : BqlType<IBqlBool, bool>.Operand<ALPrintOnConfirmSOShipmentEntryExt.feature>
		{
		}
	}
}
