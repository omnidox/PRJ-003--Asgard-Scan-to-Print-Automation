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

namespace AA.Objects.Labels.Integration.BoxPrint
{
	// Token: 0x020001CC RID: 460
	public class ALBoxPrintSOShipmentEntryExt : PXGraphExtension<SOShipmentEntry>
	{
		// Token: 0x0600134F RID: 4943 RVA: 0x000428C4 File Offset: 0x00040AC4
		public static bool IsActive()
		{
			return ALSetupSlot.IsActive(typeof(SOShipmentEntry)) && ALSetupSlot.BoxPrint;
		}

		// Token: 0x17000690 RID: 1680
		// (get) Token: 0x06001350 RID: 4944 RVA: 0x000428DF File Offset: 0x00040ADF
		// (set) Token: 0x06001351 RID: 4945 RVA: 0x000428E7 File Offset: 0x00040AE7
		[InjectDependency]
		private IALLicenseManager _licenseManager { get; set; }

		// Token: 0x17000691 RID: 1681
		// (get) Token: 0x06001352 RID: 4946 RVA: 0x000428F0 File Offset: 0x00040AF0
		// (set) Token: 0x06001353 RID: 4947 RVA: 0x000428F8 File Offset: 0x00040AF8
		[InjectDependency]
		private ILabelGenerator<IAcuLabelContext> _labelGenerator { get; set; }

		// Token: 0x17000692 RID: 1682
		// (get) Token: 0x06001354 RID: 4948 RVA: 0x00042901 File Offset: 0x00040B01
		// (set) Token: 0x06001355 RID: 4949 RVA: 0x00042909 File Offset: 0x00040B09
		[InjectDependency]
		private IModelProvider ModelProvider { get; set; }

		// Token: 0x06001356 RID: 4950 RVA: 0x00042914 File Offset: 0x00040B14
		protected virtual void _(Events.FieldUpdated<SOPackageDetail, SOPackageDetail.confirmed> e)
		{
			SOPackageDetail row = e.Row;
			SOShipment soshipment = base.Base.CurrentDocument.Current;
			bool flag = soshipment == null || row == null || !ALSetupSlot.BoxPrint;
			if (!flag)
			{
				Guid? boxPrintModelID = ALSetupSlot.BoxPrintModelID;
				IModel model = this.ModelProvider.GetModel(boxPrintModelID);
				bool flag2 = model == null;
				if (!flag2)
				{
					bool flag3 = (bool)e.NewValue;
					if (flag3)
					{
						AcuLabelContext labelContext = AcuLabelContext.CreateSingleRowPrintContext(base.Base.GetType(), soshipment, row, boxPrintModelID, soshipment.CustomerID);
						PrintResults printResults = this._labelGenerator.PrintLabels(labelContext);
						this._licenseManager.UpdateFeatureConsumption(base.GetType(), printResults.NbLabels);
					}
				}
			}
		}

		// Token: 0x0200089F RID: 2207
		[Nullable(new byte[]
		{
			0,
			1,
			0
		})]
		public abstract class feature : BqlType<IBqlBool, bool>.Operand<ALBoxPrintSOShipmentEntryExt.feature>
		{
		}
	}
}
