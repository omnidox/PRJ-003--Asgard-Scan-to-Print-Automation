using System;
using AA.Objects.Core;
using Asgard.Labels.Abstractions.Context;
using PX.Data;

namespace AA.Objects.Labels.Integration.NbCopies
{
	// Token: 0x020001F6 RID: 502
	public static class NbCopiesHelper
	{
		// Token: 0x0600144D RID: 5197 RVA: 0x000433F4 File Offset: 0x000415F4
		public static bool CheckLineDoPrint(ILabelContext lc)
		{
			object labelRow = lc.LabelRow;
			ILabelOption labelOption = AsgardCoreUtils.FindCacheExtension<ILabelOption>(labelRow);
			return labelOption == null || labelOption.UsrALPrintLabel.GetValueOrDefault();
		}

		// Token: 0x0600144E RID: 5198 RVA: 0x00043428 File Offset: 0x00041628
		public static void ToggleLabelPrint<Table>(PXSelectBase<Table> pxSelect) where Table : class, IBqlTable, new()
		{
			bool flag = pxSelect.Cache.Current == null;
			bool flag2;
			if (flag)
			{
				flag2 = false;
			}
			else
			{
				object row = pxSelect.Cache.Current;
				ILabelOption labelOption = AsgardCoreUtils.FindCacheExtension<ILabelOption>(row);
				bool flag3 = labelOption == null;
				if (flag3)
				{
					return;
				}
				flag2 = labelOption.UsrALPrintLabel.GetValueOrDefault();
			}
			bool flag4 = flag2;
			foreach (PXResult<Table> pxresult in pxSelect.Select(Array.Empty<object>()))
			{
				ILabelOption labelOption2 = AsgardCoreUtils.FindCacheExtension<ILabelOption>(pxresult);
				bool flag5 = labelOption2 == null;
				if (!flag5)
				{
					labelOption2.UsrALPrintLabel = new bool?(!flag4);
					pxSelect.Cache.Update(pxresult);
				}
			}
		}
	}
}
