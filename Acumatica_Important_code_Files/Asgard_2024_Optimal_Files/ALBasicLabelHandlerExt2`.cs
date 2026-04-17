using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using AA.Objects.AL.Integration.PrinterOverride;
using AA.Objects.AL.License;
using AA.Objects.AL.Mobile;
using PX.Data;
using PX.Data.DependencyInjection;
using PX.Data.WorkflowAPI;
using Scriban;

namespace AA.Objects.AL.Integration
{
	// Token: 0x0200029F RID: 671
	public abstract class ALBasicLabelHandlerExt<EGraph, EDoc> : PXGraphExtension<EGraph>, IGraphWithInitialization where EGraph : PXGraph where EDoc : class, IBqlTable, new()
	{
		// Token: 0x17000818 RID: 2072
		// (get) Token: 0x060019A7 RID: 6567 RVA: 0x0005DFAC File Offset: 0x0005C1AC
		// (set) Token: 0x060019A8 RID: 6568 RVA: 0x0005DFB4 File Offset: 0x0005C1B4
		[InjectDependency]
		private IALLicenseManagerFactory LicenseManagerFactory { get; set; }

		// Token: 0x17000819 RID: 2073
		// (get) Token: 0x060019A9 RID: 6569 RVA: 0x0005DFBD File Offset: 0x0005C1BD
		private IALLicenseManager LicenseManager
		{
			get
			{
				return this.LicenseManagerFactory.GetLicenseManager(ALConstants.ProductCode);
			}
		}

		// Token: 0x1700081A RID: 2074
		// (get) Token: 0x060019AA RID: 6570 RVA: 0x0005DFCF File Offset: 0x0005C1CF
		// (set) Token: 0x060019AB RID: 6571 RVA: 0x0005DFD7 File Offset: 0x0005C1D7
		[InjectDependency]
		private ILabelGenerator _labelGenerator { get; set; }

		// Token: 0x060019AC RID: 6572 RVA: 0x0005DFE0 File Offset: 0x0005C1E0
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
					this._connotation = 8;
				}
				this._isGIGraph = AsgardUtils.IsGI(base.Base);
				this._isMobileGraph = MobileUtils.IsMobile(base.Base);
				this._skipRuleEval = (this._isGIGraph || this._isMobileGraph);
				this._isRegularGraph = (!this._isGIGraph && !this._isMobileGraph);
				IEnumerable<Models.Model> models = this.GetModels();
				this.AddPrintActions(models);
				bool flag = this._isGIGraph || this._isMobileGraph;
				if (flag)
				{
					this.EnableOrDisableAll(true);
				}
			}
			catch (Exception ex)
			{
				PXTrace.WriteError(ex);
			}
		}

		// Token: 0x060019AD RID: 6573 RVA: 0x00019FF9 File Offset: 0x000181F9
		private void GIResultSelected(PXCache sender, PXRowSelectedEventArgs e)
		{
		}

		// Token: 0x060019AE RID: 6574 RVA: 0x0005E0C8 File Offset: 0x0005C2C8
		protected void _(Events.RowSelected<EDoc> e)
		{
			bool isRegularGraph = this._isRegularGraph;
			if (isRegularGraph)
			{
				EDoc row = e.Row;
				PXCache cache = e.Cache;
				bool flag = this._oldRow != null && !cache.ObjectsEqual(row, this._oldRow);
				if (flag)
				{
					this._oldRow = default(EDoc);
				}
				this.ShowHideModels(row, cache);
			}
			else
			{
				bool isMobileGraph = this._isMobileGraph;
				if (!isMobileGraph)
				{
					bool isGIGraph = this._isGIGraph;
					if (isGIGraph)
					{
					}
				}
			}
		}

		// Token: 0x060019AF RID: 6575 RVA: 0x0005E158 File Offset: 0x0005C358
		protected void _(Events.RowUpdated<EDoc> e)
		{
			bool isGIGraph = this._isGIGraph;
			if (isGIGraph)
			{
				EDoc row = e.Row;
				PXCache cache = e.Cache;
				this.ShowHideModels(row, cache);
			}
			else
			{
				this._oldRow = e.OldRow;
			}
		}

		// Token: 0x060019B0 RID: 6576 RVA: 0x0005E19C File Offset: 0x0005C39C
		protected void _(Events.RowPersisting<EDoc> e)
		{
			bool autoPrint = ALSetupSlot.AutoPrint;
			if (autoPrint)
			{
				this.PrintLabels(e.Row);
			}
		}

		// Token: 0x060019B1 RID: 6577 RVA: 0x0005E1C4 File Offset: 0x0005C3C4
		private void ShowHideModels(EDoc row, PXCache cache)
		{
			bool isMobileGraph = this._isMobileGraph;
			if (!isMobileGraph)
			{
				bool flag = cache.GetStatus(row) == 2;
				bool flag2 = row == null || (this._isRegularGraph && base.Base.UnattendedMode) || (this._isRegularGraph && flag) || this._connotation != null || PXLongOperation.IsLongOperationContext();
				if (flag2)
				{
					this.EnableOrDisableAll(false);
				}
				else
				{
					TemplateContext scribanContext = ScribanUtils.CreateContext(base.Base, row, this._oldRow, false, Array.Empty<object>());
					foreach (ModelAction<EDoc> modelAction in this._observableModels)
					{
						bool flag3 = true;
						Guid? drivenRuleID = modelAction.DrivenRuleID;
						try
						{
							IRule rule;
							bool flag4 = Rules.TryGetRule(drivenRuleID, out rule);
							bool flag5 = flag4 && rule.Active.GetValueOrDefault() && !string.IsNullOrEmpty(rule.Expression);
							if (flag5)
							{
								string expression = rule.Expression;
								flag3 = scribanContext.EvalExpr(expression, true);
								bool reverseRule = modelAction.ReverseRule;
								if (reverseRule)
								{
									flag3 = !flag3;
								}
								bool flag6 = !flag3;
								if (flag6)
								{
									PXTrace.WriteInformation("Expr. for model {0} is false: {1}", new object[]
									{
										modelAction.Model.Name,
										expression
									});
								}
							}
							ALBasicLabelHandlerExt<EGraph, EDoc>.ShowHide(modelAction, flag3);
						}
						catch (Exception ex)
						{
							PXTrace.WriteError(ex);
							modelAction.SetEnabled(false);
							modelAction.SetTooltip(ex.Message);
						}
					}
				}
			}
		}

		// Token: 0x060019B2 RID: 6578 RVA: 0x0005E394 File Offset: 0x0005C594
		private void PrintLabels(EDoc row)
		{
			bool flag = row == null;
			if (!flag)
			{
				TemplateContext scribanContext = ScribanUtils.CreateContext(base.Base, row, this._oldRow, false, Array.Empty<object>());
				string screenID = AsgardUtils.GetScreenID();
				int? baccountID = RuleUtils.GetBAccountID(base.Base, row);
				IEnumerable<AutoPrints.AutoPrintRule> autoPrintRulesByScreenID = AutoPrints.GetAutoPrintRulesByScreenID(screenID, baccountID);
				foreach (AutoPrints.AutoPrintRule autoPrintRule in autoPrintRulesByScreenID)
				{
					Models.Model model;
					bool flag2 = Models.TryGetModelByID(autoPrintRule.ModelID, out model);
					bool flag3 = !flag2;
					if (!flag3)
					{
						Guid? ruleID = autoPrintRule.RuleID;
						IRule rule;
						bool flag4 = Rules.TryGetRule(ruleID, out rule);
						bool flag5 = flag4 && rule.Active.GetValueOrDefault() && !string.IsNullOrEmpty(rule.Expression);
						if (flag5)
						{
							string expression = rule.Expression;
							bool flag6 = scribanContext.EvalExpr(expression, true);
							bool reverseRule = autoPrintRule.ReverseRule;
							if (reverseRule)
							{
								flag6 = !flag6;
							}
							bool flag7 = !flag6;
							if (flag7)
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

		// Token: 0x060019B3 RID: 6579 RVA: 0x0005E514 File Offset: 0x0005C714
		private void PrintLabel(EDoc row, AutoPrints.AutoPrintRule autoPrint, Models.Model model)
		{
			bool flag = model != null;
			if (flag)
			{
				using (PXTransactionScope pxtransactionScope = new PXTransactionScope())
				{
					BasicLabelGenerator basicLabelGenerator = new BasicLabelGenerator();
					bool flag2 = MobileUtils.IsMobile(base.Base);
					LabelContext labelContext;
					if (flag2)
					{
						labelContext = LabelContext.CreateMobilePrintContext(base.Base, row, model.ModelID, null);
					}
					else
					{
						labelContext = LabelContext.CreatePrintContext(base.Base.GetType(), row, model.ModelID, true, null);
					}
					labelContext.IsSilent = true;
					labelContext.IgnorePrinterMissing = true;
					PrintResults printResults = basicLabelGenerator.PrintLabels(labelContext);
					pxtransactionScope.Complete();
				}
			}
		}

		// Token: 0x060019B4 RID: 6580 RVA: 0x0005E5D8 File Offset: 0x0005C7D8
		private void EnableOrDisableAll(bool enableOrShow)
		{
			foreach (ModelAction<EDoc> modelAction in this._observableModels)
			{
				ALBasicLabelHandlerExt<EGraph, EDoc>.ShowHide(modelAction, enableOrShow);
			}
		}

		// Token: 0x060019B5 RID: 6581 RVA: 0x0005E62C File Offset: 0x0005C82C
		private static void ShowHide(ModelAction<EDoc> modelAction, bool enableOrShow)
		{
			modelAction.SetEnabled(enableOrShow);
			bool hideInstead = modelAction.HideInstead;
			if (hideInstead)
			{
				modelAction.SetVisible(enableOrShow);
			}
			modelAction.SetTooltip(modelAction.Model.Tooltip);
		}

		// Token: 0x060019B6 RID: 6582 RVA: 0x00019FF9 File Offset: 0x000181F9
		[PXButton(CommitChanges = true, MenuAutoOpen = true)]
		[PXUIField(DisplayName = "Asgard Labels")]
		public virtual void aLLabelFolder()
		{
		}

		// Token: 0x060019B7 RID: 6583 RVA: 0x0005E668 File Offset: 0x0005C868
		private IEnumerable<Models.Model> GetModels()
		{
			string regularGraphFromMobile = MobileUtils.GetRegularGraphFromMobile(base.Base);
			bool flag = regularGraphFromMobile != null;
			IEnumerable<Models.Model> enumerable;
			if (flag)
			{
				enumerable = Models.GetModelsByGraphType(regularGraphFromMobile, Models.IS_SINGLE_OR_GROUP);
			}
			else
			{
				string screenID = AsgardUtils.GetScreenID();
				enumerable = Models.GetModelsByScreenID(screenID, Models.IS_SINGLE_OR_GROUP);
			}
			IEnumerable<Models.Model> source = enumerable;
			Func<Models.Model, bool> predicate;
			if ((predicate = ALBasicLabelHandlerExt<EGraph, EDoc>.<>O.<0>__IsGroup) == null)
			{
				predicate = (ALBasicLabelHandlerExt<EGraph, EDoc>.<>O.<0>__IsGroup = new Func<Models.Model, bool>(ALBasicLabelHandlerExt<EGraph, EDoc>.IsGroup));
			}
			bool flag2 = source.Any(predicate);
			IEnumerable<Models.Model> source2 = enumerable;
			Func<Models.Model, string> keySelector;
			if ((keySelector = ALBasicLabelHandlerExt<EGraph, EDoc>.<>O.<1>__ModelOrder) == null)
			{
				keySelector = (ALBasicLabelHandlerExt<EGraph, EDoc>.<>O.<1>__ModelOrder = new Func<Models.Model, string>(ALBasicLabelHandlerExt<EGraph, EDoc>.ModelOrder));
			}
			return source2.OrderBy(keySelector).ToArray<Models.Model>();
		}

		// Token: 0x060019B8 RID: 6584 RVA: 0x0005E708 File Offset: 0x0005C908
		private static string ModelOrder(Models.Model model)
		{
			return model.Description;
		}

		// Token: 0x060019B9 RID: 6585 RVA: 0x0005E720 File Offset: 0x0005C920
		private static bool IsGroup(Models.Model model)
		{
			return model.ModelType == "G";
		}

		// Token: 0x060019BA RID: 6586 RVA: 0x0005E744 File Offset: 0x0005C944
		private void AddPrintActions(IEnumerable<Models.Model> models)
		{
			bool flag = !models.Any<Models.Model>();
			if (!flag)
			{
				PXCacheCollection caches = base.Base.Caches;
				PXCache pxcache = caches[typeof(EDoc)];
				Type[] extensionTypes = pxcache.GetExtensionTypes();
				Type left = Array.Find<Type>(extensionTypes, (Type type) => type.IsCompatibleWith(typeof(IPrintOption)));
				foreach (Models.Model model in models)
				{
					this.AddPrintAction(base.Base, model, left != null);
				}
			}
		}

		// Token: 0x060019BB RID: 6587 RVA: 0x0005E810 File Offset: 0x0005CA10
		private void AddPrintAction(PXGraph graph, Models.Model model, bool alwaysAllow)
		{
			IEnumerable<ModelPrinters.ModelPrinter> printers = from det in ModelPrinters.GetPrinters(model.ModelID)
			where det.Active.GetValueOrDefault()
			select det;
			Guid? printerID = BasicLabelUtils.ChoosePrinter<ModelPrinters.ModelPrinter>(graph, printers);
			IPrinter printer;
			Printers.TryGetPrinter(printerID, out printer);
			bool flag = model.ModelType == "S" && printer == null && !alwaysAllow;
			if (flag)
			{
				PXTrace.WriteInformation("Model {0} has no printer for you", new object[]
				{
					model.Name
				});
			}
			else
			{
				PXButtonDelegate handler = delegate(PXAdapter adapter)
				{
					PXLongOperation.StartOperation(this.Base.UID, delegate()
					{
						PXCacheCollection caches = this.Base.Caches;
						PXCache pxcache = caches[typeof(EDoc)];
						bool isDirty = pxcache.IsDirty;
						if (isDirty)
						{
							this.Base.Actions.PressSave();
						}
						object row = (pxcache != null) ? pxcache.Current : null;
						bool flag2 = MobileUtils.IsMobile(this.Base);
						LabelContext labelContext;
						if (flag2)
						{
							labelContext = LabelContext.CreateMobilePrintContext(this.Base, row, model.ModelID, adapter);
							labelContext.IsSilent = true;
						}
						else
						{
							labelContext = LabelContext.CreatePrintContext(graph.GetType(), row, model.ModelID, false, adapter);
						}
						try
						{
							this._labelGenerator.PrintLabels(labelContext);
						}
						catch (Exception ex)
						{
							PXTrace.WriteError(ex);
							throw;
						}
					});
					return adapter.Get();
				};
				this.DoAddPrintAction(model, handler, printer, "");
			}
		}

		// Token: 0x060019BC RID: 6588 RVA: 0x0005E8F0 File Offset: 0x0005CAF0
		private void DoAddPrintAction(Models.Model model, PXButtonDelegate handler, IPrinter printerToUse, string suffix)
		{
			string actionName = BasicLabelUtils.GetActionName(model.ModelID, suffix);
			string text = suffix + (string.IsNullOrEmpty(suffix) ? "" : " ") + model.Description;
			PXButtonAttribute pxbuttonAttribute = PXEventSubscriberAttribute.CreateInstance<PXButtonAttribute>(Array.Empty<object>());
			pxbuttonAttribute.DisplayOnMainToolbar = false;
			pxbuttonAttribute.PopupVisible = false;
			pxbuttonAttribute.Tooltip = ((this._connotation == null) ? model.Tooltip : "");
			PXUIFieldAttribute pxuifieldAttribute = new PXUIFieldAttribute
			{
				DisplayName = PXMessages.LocalizeNoPrefix(text),
				MapEnableRights = 1
			};
			PXEventSubscriberAttribute[] array = new PXEventSubscriberAttribute[]
			{
				pxbuttonAttribute,
				pxuifieldAttribute
			};
			PXAction<EDoc> pxaction = PXNamedAction<EDoc>.AddAction(base.Base, actionName, text, handler, array);
			pxaction.SetConnotation(this._connotation);
			this.ALLabelFolder.AddMenuAction(pxaction);
			ModelAction<EDoc> modelAction = new ModelAction<EDoc>(pxaction, model, printerToUse);
			modelAction.SetEnabled(false);
			this._observableModels.Add(modelAction);
		}

		// Token: 0x04000B47 RID: 2887
		public PXSetup<ALSetup> ALSetup;

		// Token: 0x04000B48 RID: 2888
		private readonly IList<ModelAction<EDoc>> _observableModels = new List<ModelAction<EDoc>>();

		// Token: 0x04000B49 RID: 2889
		private bool _isGIGraph;

		// Token: 0x04000B4A RID: 2890
		private bool _isMobileGraph;

		// Token: 0x04000B4B RID: 2891
		private bool _isRegularGraph;

		// Token: 0x04000B4C RID: 2892
		private bool _skipRuleEval;

		// Token: 0x04000B4E RID: 2894
		private ActionConnotation _connotation = 0;

		// Token: 0x04000B50 RID: 2896
		private EDoc _oldRow;

		// Token: 0x04000B51 RID: 2897
		public PXAction<EDoc> ALLabelFolder;

		// Token: 0x02000A02 RID: 2562
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x04001433 RID: 5171
			public static Func<Models.Model, bool> <0>__IsGroup;

			// Token: 0x04001434 RID: 5172
			public static Func<Models.Model, string> <1>__ModelOrder;
		}
	}
}
