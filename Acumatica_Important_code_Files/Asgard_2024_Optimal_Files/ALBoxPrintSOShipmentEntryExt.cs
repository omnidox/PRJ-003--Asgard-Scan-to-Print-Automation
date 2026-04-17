using System;
using AA.Objects.AL.License;
using PX.Data;
using PX.Data.BQL;
using PX.Objects.SO;

namespace AA.Objects.AL.Integration.BoxPrint
{
	// Token: 0x020002A2 RID: 674
	public class ALBoxPrintSOShipmentEntryExt : PXGraphExtension<SOShipmentEntry>
	{
		// Token: 0x060019C3 RID: 6595 RVA: 0x0005EA70 File Offset: 0x0005CC70
		public static bool IsActive()
		{
			return ALSetupSlot.BoxPrint;
		}

		// Token: 0x1700081C RID: 2076
		// (get) Token: 0x060019C4 RID: 6596 RVA: 0x0005EA77 File Offset: 0x0005CC77
		// (set) Token: 0x060019C5 RID: 6597 RVA: 0x0005EA7F File Offset: 0x0005CC7F
		[InjectDependency]
		private IALLicenseManager _licenseManager { get; set; }

		// Token: 0x1700081D RID: 2077
		// (get) Token: 0x060019C6 RID: 6598 RVA: 0x0005EA88 File Offset: 0x0005CC88
		// (set) Token: 0x060019C7 RID: 6599 RVA: 0x0005EA90 File Offset: 0x0005CC90
		[InjectDependency]
		private ILabelGenerator _labelGenerator { get; set; }

		// Token: 0x060019C8 RID: 6600 RVA: 0x0005EA9C File Offset: 0x0005CC9C
		protected virtual void _(Events.FieldUpdated<SOPackageDetail, SOPackageDetail.confirmed> e)
		{
			SOPackageDetail row = e.Row;
			SOShipment soshipment = base.Base.CurrentDocument.Current;
			bool flag = soshipment == null || row == null || !ALSetupSlot.BoxPrint;
			if (!flag)
			{
				Guid? boxPrintModelID = ALSetupSlot.BoxPrintModelID;
				Models.Model model;
				Models.TryGetModelByID(boxPrintModelID, out model);
				bool flag2 = model == null;
				if (!flag2)
				{
					bool flag3 = (bool)e.NewValue;
					if (flag3)
					{
						LabelContext labelContext = LabelContext.CreateSingleRowPrintContext(base.Base.GetType(), soshipment, row, boxPrintModelID, soshipment.CustomerID);
						PrintResults printResults = this._labelGenerator.PrintLabels(labelContext);
						this._licenseManager.UpdateFeatureConsumption(base.GetType(), printResults.NbLabels);
					}
				}
			}
		}

		// Token: 0x02000A07 RID: 2567
		public abstract class feature : BqlType<IBqlBool, bool>.Operand<ALBoxPrintSOShipmentEntryExt.feature>
		{
		}
	}
}
