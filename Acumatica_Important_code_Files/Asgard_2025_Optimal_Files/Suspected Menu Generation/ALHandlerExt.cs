using System;
using System.Collections.Generic;
using AA.Objects.Core;
using AA.Objects.Core.Mobile;
using AA.Objects.Labels.Mobile;
using AA.Objects.License;
using Asgard.Labels.Abstractions.Interface;
using Asgard.Labels.Abstractions.Service;
using Asgard.Labels.Impl.Context;
using Asgard.Labels.Impl.Language.MyScriban;
using Asgard.Labels.Impl.Poco;
using PX.Data;
using PX.Data.DependencyInjection;
using Scriban;

namespace AA.Objects.Labels.Integration
{
	// Token: 0x020001B7 RID: 439
	public abstract class ALHandlerExt<EGraph, EDoc> : PXGraphExtension<EGraph>, IGraphWithInitialization, ILabelHandler<EGraph> where EGraph : PXGraph, new() where EDoc : class, IBqlTable, new()
	{
		// Token: 0x17000681 RID: 1665
		// (get) Token: 0x06001307 RID: 4871 RVA: 0x00041F8C File Offset: 0x0004018C
		// (set) Token: 0x06001308 RID: 4872 RVA: 0x00041F94 File Offset: 0x00040194
		[InjectDependency]
		private IALLicenseManagerFactory LicenseManagerFactory { get; set; }

		// Token: 0x17000682 RID: 1666
		// (get) Token: 0x06001309 RID: 4873 RVA: 0x00041F9D File Offset: 0x0004019D
		private IALLicenseManager LicenseManager
		{
			get
			{
				return this.LicenseManagerFactory.GetLicenseManager(ALConstants.ProductCode);
			}
		}

		// Token: 0x17000683 RID: 1667
		// (get) Token: 0x0600130A RID: 4874 RVA: 0x00041FAF File Offset: 0x000401AF
		// (set) Token: 0x0600130B RID: 4875 RVA: 0x00041FB7 File Offset: 0x000401B7
		[InjectDependency]
		public IModelProvider ModelProvider { get; set; }

		// Token: 0x17000684 RID: 1668
		// (get) Token: 0x0600130C RID: 4876 RVA: 0x00041FC0 File Offset: 0x000401C0
		// (set) Token: 0x0600130D RID: 4877 RVA: 0x00041FC8 File Offset: 0x000401C8
		[InjectDependency]
		public IEntityContextFactory EntityContextFactory { get; set; }

		// Token: 0x17000685 RID: 1669
		// (get) Token: 0x0600130E RID: 4878 RVA: 0x00041FD1 File Offset: 0x000401D1
		// (set) Token: 0x0600130F RID: 4879 RVA: 0x00041FD9 File Offset: 0x000401D9
		[InjectDependency]
		public ILabelGenerator<IAcuLabelContext> LabelGenerator { get; set; }

		// Token: 0x17000686 RID: 1670
		// (get) Token: 0x06001310 RID: 4880 RVA: 0x00041FE2 File Offset: 0x000401E2
		public EGraph LabelGraph
		{
			get
			{
				return base.Base;
			}
		}

		// Token: 0x17000687 RID: 1671
		// (get) Token: 0x06001311 RID: 4881 RVA: 0x00041FEA File Offset: 0x000401EA
		// (set) Token: 0x06001312 RID: 4882 RVA: 0x00041FF2 File Offset: 0x000401F2
		public PXAction LabelFolder { get; set; }

		// Token: 0x06001313 RID: 4883 RVA: 0x00041FFC File Offset: 0x000401FC
		public override void Initialize()
		{
			base.Initialize();
			try
			{
				try
				{
					this.LicenseManager.Check();
				}
				catch
				{
				}
				bool flag = MobileUtils.IsMobile(base.Base);
				if (!flag)
				{
					this._labelHelper = new LabelConductor<EGraph>(this);
				}
			}
			catch (Exception ex)
			{
				PXTrace.WriteError(ex);
			}
		}

		// Token: 0x06001314 RID: 4884 RVA: 0x00042078 File Offset: 0x00040278
		protected void _(Events.RowSelected<EDoc> e)
		{
			EDoc row = e.Row;
			PXCache cache = e.Cache;
			bool flag = this._oldRow != null && !cache.ObjectsEqual(row, this._oldRow);
			if (flag)
			{
				this._oldRow = default(EDoc);
			}
			LabelConductor<EGraph> labelHelper = this._labelHelper;
			if (labelHelper != null)
			{
				labelHelper.ShowHideModels(row, this._oldRow);
			}
		}

		// Token: 0x06001315 RID: 4885 RVA: 0x000420F4 File Offset: 0x000402F4
		protected void _(Events.RowUpdated<EDoc> e)
		{
			LabelConductor<EGraph> labelHelper = this._labelHelper;
			bool flag = labelHelper != null && labelHelper.IsGI;
			if (flag)
			{
				EDoc row = e.Row;
				PXCache cache = e.Cache;
				this._labelHelper.ShowHideModels(row, null);
			}
			else
			{
				this._oldRow = e.OldRow;
			}
		}

		// Token: 0x06001316 RID: 4886 RVA: 0x0004214C File Offset: 0x0004034C
		protected void _(Events.RowPersisting<EDoc> e)
		{
			bool autoPrint = ALSetupSlot.AutoPrint;
			if (autoPrint)
			{
				this.PrintLabels(e.Row);
			}
		}

		// Token: 0x06001317 RID: 4887 RVA: 0x00042174 File Offset: 0x00040374
		private void PrintLabels(EDoc row)
		{
			bool flag = row == null || this._labelHelper == null;
			if (!flag)
			{
				TemplateContext scribanContext = ScribanUtils.CreateContext(base.Base, row, this._oldRow, false, Array.Empty<object>());
				string screenID = AsgardUtils.GetScreenID();
				int? baccountID = AsgardUtils.GetBAccountID(base.Base, row);
				IEnumerable<AutoPrints.AutoPrintRule> autoPrintRulesByScreenID = AutoPrints.GetAutoPrintRulesByScreenID(screenID, baccountID);
				foreach (AutoPrints.AutoPrintRule autoPrintRule in autoPrintRulesByScreenID)
				{
					IModel model = this.ModelProvider.GetModel(autoPrintRule.ModelID);
					bool flag2 = model == null;
					if (!flag2)
					{
						Guid? ruleID = autoPrintRule.RuleID;
						IRule rule;
						bool flag3 = Rules.TryGetValue(ruleID, out rule);
						bool flag4 = flag3 && rule.Active.GetValueOrDefault() && !string.IsNullOrEmpty(rule.Expression);
						if (flag4)
						{
							string expression = rule.Expression;
							bool flag5 = NewScribanUtils.EvalExpr<bool>(scribanContext, expression, true);
							bool reverseRule = autoPrintRule.ReverseRule;
							if (reverseRule)
							{
								flag5 = !flag5;
							}
							bool flag6 = !flag5;
							if (flag6)
							{
								PXTrace.WriteInformation("Expr. for AutoPrint {0} is false: {1}", new object[]
								{
									autoPrintRule.Name,
									expression
								});
								continue;
							}
						}
						this.PrintLabel(row, autoPrintRule, model);
					}
				}
			}
		}

		// Token: 0x06001318 RID: 4888 RVA: 0x00042308 File Offset: 0x00040508
		private void PrintLabel(EDoc row, AutoPrints.AutoPrintRule autoPrint, IModel model)
		{
			bool flag = model != null;
			if (flag)
			{
				using (PXTransactionScope pxtransactionScope = new PXTransactionScope())
				{
					AcuLabelGenerator acuLabelGenerator = new AcuLabelGenerator();
					AcuLabelContext acuLabelContext = MobileUtils.IsMobile(base.Base) ? AcuLabelContext.CreateMobilePrintContext(base.Base, row, model.ID, null) : AcuLabelContext.CreatePrintContext(base.Base.GetType(), row, model.ID, true, null);
					acuLabelContext.IsSilent = true;
					acuLabelContext.IgnorePrinterMissing = true;
					PrintResults printResults = acuLabelGenerator.PrintLabels(acuLabelContext);
					pxtransactionScope.Complete();
				}
			}
		}

		// Token: 0x040007A3 RID: 1955
		public PXSetup<ALSetup> ALSetup;

		// Token: 0x040007A7 RID: 1959
		private LabelConductor<EGraph> _labelHelper;

		// Token: 0x040007AA RID: 1962
		private EDoc _oldRow;
	}
}
