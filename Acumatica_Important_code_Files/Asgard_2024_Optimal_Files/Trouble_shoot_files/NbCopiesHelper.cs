using System;
using PX.Data;

namespace AA.Objects.AL.Integration.NbCopies
{
	// Token: 0x020002D7 RID: 727
	public static class NbCopiesHelper
	{
		// Token: 0x06001AF6 RID: 6902 RVA: 0x0005F6F0 File Offset: 0x0005D8F0
		public static bool CheckLineDoPrint(LabelContext lc)
		{
			object labelRow = lc.LabelRow;
			ILabelOption labelOption = AsgardUtils.FindExtension<ILabelOption>(labelRow);
			bool flag = labelOption != null;
			return !flag || labelOption.UsrALPrintLabel.GetValueOrDefault();
		}

		// Token: 0x06001AF7 RID: 6903 RVA: 0x0005F730 File Offset: 0x0005D930
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
				ILabelOption labelOption = AsgardUtils.FindExtension<ILabelOption>(row);
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
				ILabelOption labelOption2 = AsgardUtils.FindExtension<ILabelOption>(pxresult);
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
