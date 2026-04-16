using System;
using AA.Objects.License;
using PX.Data;

namespace AA.Objects.Labels
{
	// Token: 0x020000E3 RID: 227
	public class ALLabelHandler : PXGraph
	{
		// Token: 0x1700013C RID: 316
		// (get) Token: 0x06000498 RID: 1176 RVA: 0x0001B1F4 File Offset: 0x000193F4
		// (set) Token: 0x06000499 RID: 1177 RVA: 0x0001B1FC File Offset: 0x000193FC
		[InjectDependency]
		private IALLicenseManagerFactory LicenseManagerFactory { get; set; }

		// Token: 0x1700013D RID: 317
		// (get) Token: 0x0600049A RID: 1178 RVA: 0x0001B205 File Offset: 0x00019405
		internal IALLicenseManager LicenseManager
		{
			get
			{
				return this.LicenseManagerFactory.GetLicenseManager(ALConstants.ProductCode);
			}
		}

		// Token: 0x040000D2 RID: 210
		public PXSelect<ALModel> Model;

		// Token: 0x040000D3 RID: 211
		public PXSelect<ALModelExpr, Where<ALModelExpr.labelID, Equal<Current<ALModel.labelID>>, And<ALModelExpr.active, Equal<True>>>, OrderBy<Asc<ALModelExpr.sortOrder, Asc<ALModelExpr.lineNbr>>>> Expressions;

		// Token: 0x040000D4 RID: 212
		public PXSelect<ALModelChild, Where<ALModelChild.labelID, Equal<Current<ALModel.labelID>>, And<ALModelChild.active, Equal<True>>>, OrderBy<Asc<ALModelChild.sortOrder, Asc<ALModelChild.lineNbr>>>> Children;

		// Token: 0x040000D5 RID: 213
		public PXSelect<ALModelGraphic, Where<ALModelGraphic.modelID, Equal<Current<ALModel.labelID>>, And<ALModelGraphic.active, Equal<True>>>, OrderBy<Asc<ALModelGraphic.sortOrder, Asc<ALModelGraphic.lineNbr>>>> Graphics;

		// Token: 0x040000D6 RID: 214
		public PXSetup<ALSetup> Setup;
	}
}
