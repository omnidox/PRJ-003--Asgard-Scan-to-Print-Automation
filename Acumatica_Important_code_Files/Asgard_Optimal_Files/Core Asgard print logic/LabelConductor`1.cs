using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using AA.Objects.Core;
using AA.Objects.Core.Mobile;
using AA.Objects.Labels.Integration;
using AA.Objects.Labels.Integration.PrinterOverride;
using Asgard.Labels.Abstractions.Helpers;
using Asgard.Labels.Abstractions.Interface;
using Asgard.Labels.Abstractions.Poco;
using Asgard.Labels.Impl.Context;
using Asgard.Labels.Impl.Language.MyScriban;
using Asgard.Labels.Impl.Poco;
using PX.Data;
using PX.Objects.SO.WMS;
using Scriban;

namespace AA.Objects.Labels.Mobile
{
	// Token: 0x020001AD RID: 429
	public class LabelConductor<TGraph> where TGraph : PXGraph, new()
	{
		// Token: 0x17000668 RID: 1640
		// (get) Token: 0x060012BB RID: 4795 RVA: 0x00040FF5 File Offset: 0x0003F1F5
		// (set) Token: 0x060012BC RID: 4796 RVA: 0x00040FFD File Offset: 0x0003F1FD
		protected ILabelHandler<TGraph> LabelHandler { get; set; }

		// Token: 0x17000669 RID: 1641
		// (get) Token: 0x060012BD RID: 4797 RVA: 0x00041006 File Offset: 0x0003F206
		// (set) Token: 0x060012BE RID: 4798 RVA: 0x0004100E File Offset: 0x0003F20E
		protected IEnumerable<ModelAction> ObservableModels { get; set; } = Enumerable.Empty<ModelAction>();

		// Token: 0x1700066A RID: 1642
		// (get) Token: 0x060012BF RID: 4799 RVA: 0x00041017 File Offset: 0x0003F217
		// (set) Token: 0x060012C0 RID: 4800 RVA: 0x0004101F File Offset: 0x0003F21F
		public PXGraph Graph { get; set; }

		// Token: 0x1700066B RID: 1643
		// (get) Token: 0x060012C1 RID: 4801 RVA: 0x00041028 File Offset: 0x0003F228
		// (set) Token: 0x060012C2 RID: 4802 RVA: 0x00041030 File Offset: 0x0003F230
		public bool IsGI { get; set; }

		// Token: 0x1700066C RID: 1644
		// (get) Token: 0x060012C3 RID: 4803 RVA: 0x00041039 File Offset: 0x0003F239
		// (set) Token: 0x060012C4 RID: 4804 RVA: 0x00041041 File Offset: 0x0003F241
		public bool IsMobile { get; set; }

		// Token: 0x1700066D RID: 1645
		// (get) Token: 0x060012C5 RID: 4805 RVA: 0x0004104A File Offset: 0x0003F24A
		// (set) Token: 0x060012C6 RID: 4806 RVA: 0x00041052 File Offset: 0x0003F252
		public bool IsRegular { get; set; }

		// Token: 0x1700066E RID: 1646
		// (get) Token: 0x060012C7 RID: 4807 RVA: 0x0004105B File Offset: 0x0003F25B
		// (set) Token: 0x060012C8 RID: 4808 RVA: 0x00041063 File Offset: 0x0003F263
		public bool IsFiltered { get; set; }

		// Token: 0x1700066F RID: 1647
		// (get) Token: 0x060012C9 RID: 4809 RVA: 0x0004106C File Offset: 0x0003F26C
		// (set) Token: 0x060012CA RID: 4810 RVA: 0x00041074 File Offset: 0x0003F274
		public bool IsMobileFiltered { get; set; }

		// Token: 0x17000670 RID: 1648
		// (get) Token: 0x060012CB RID: 4811 RVA: 0x0004107D File Offset: 0x0003F27D
		// (set) Token: 0x060012CC RID: 4812 RVA: 0x00041085 File Offset: 0x0003F285
		public bool IsRegularFiltered { get; set; }

		// Token: 0x17000671 RID: 1649
		// (get) Token: 0x060012CD RID: 4813 RVA: 0x0004108E File Offset: 0x0003F28E
		// (set) Token: 0x060012CE RID: 4814 RVA: 0x00041096 File Offset: 0x0003F296
		public bool IsAPI { get; set; }

		// Token: 0x060012CF RID: 4815 RVA: 0x000410A0 File Offset: 0x0003F2A0
		internal LabelConductor(ILabelHandler<TGraph> labelHandler)
		{
			this.LabelHandler = labelHandler;
			this.Graph = labelHandler.LabelGraph;
			this.IsGI = AsgardCoreUtils.IsGI(this.Graph);
			this.IsMobile = MobileUtils.IsMobile(this.Graph);
			this.IsFiltered = AsgardCoreUtils.IsFilteredGraph(this.Graph);
			this.IsRegular = (!this.IsGI && !this.IsMobile);
			this.IsMobileFiltered = (this.IsMobile && this.IsFiltered);
			this.IsRegularFiltered = (this.IsRegular && this.IsFiltered);
			this.IsAPI = (this.Graph.IsContractBasedAPI || this.Graph.IsImport || this.Graph.IsExport);
			bool flag = this.Graph is PickPackShip.Host;
			bool flag2 = (this.IsRegular || this.IsGI || this.IsRegularFiltered || this.IsMobileFiltered) && !flag;
			if (flag2)
			{
				this.ObservableModels = this.AddPrintActions();
			}
			bool flag3 = this.IsGI || this.IsRegularFiltered || this.IsAPI || this.Graph.UnattendedMode;
			if (flag3)
			{
				this.EnableOrDisableAll(true);
			}
		}

		// Token: 0x060012D0 RID: 4816 RVA: 0x00041208 File Offset: 0x0003F408
		private IEnumerable<ModelAction> AddPrintActions()
		{
			IEnumerable<IAcuModel> models = this.GetModels();
			bool flag = !models.Any<IAcuModel>();
			IEnumerable<ModelAction> result;
			if (flag)
			{
				result = Enumerable.Empty<ModelAction>();
			}
			else
			{
				PXCacheCollection caches = this.Graph.Caches;
				PXCache pxcache = caches[this.Graph.PrimaryItemType];
				Type[] extensionTypes = pxcache.GetExtensionTypes();
				Type left = Array.Find<Type>(extensionTypes, (Type type) => type.IsCompatibleWith(typeof(IPrinterOverride)));
				List<ModelAction> list = new List<ModelAction>();
				foreach (IAcuModel model in models)
				{
					ModelAction modelAction = this.AddPrintAction(model, left != null);
					bool flag2 = modelAction != null;
					if (flag2)
					{
						list.Add(modelAction);
					}
				}
				result = list;
			}
			return result;
		}

		// Token: 0x060012D1 RID: 4817 RVA: 0x000412FC File Offset: 0x0003F4FC
		internal void ShowHideModels(object row, object oldRow)
		{
			bool flag = this.IsAPI || this.Graph.UnattendedMode;
			if (!flag)
			{
				bool flag2 = row == null;
				if (!flag2)
				{
					PXCache pxcache = this.Graph.Caches[row.GetType()];
					bool flag3 = pxcache.GetStatus(row) == 2;
					bool flag4 = this.IsRegular && flag3;
					if (flag4)
					{
						this.EnableOrDisableAll(false);
					}
					else
					{
						TemplateContext scribanContext = ScribanUtils.CreateContext(this.Graph, row, oldRow, false, Array.Empty<object>());
						foreach (ModelAction modelAction in this.ObservableModels)
						{
							bool flag5 = true;
							Guid? ruleID = modelAction.RuleID;
							try
							{
								IRule rule;
								bool flag6 = Rules.TryGetValue(ruleID, out rule);
								bool flag7 = flag6 && rule.Active.GetValueOrDefault() && !string.IsNullOrEmpty(rule.Expression);
								if (flag7)
								{
									string expression = rule.Expression;
									flag5 = NewScribanUtils.EvalExpr<bool>(scribanContext, expression, true);
									bool reverseRule = modelAction.ReverseRule;
									if (reverseRule)
									{
										flag5 = !flag5;
									}
									bool flag8 = !flag5;
									if (flag8)
									{
										PXTrace.WriteInformation("Expr. for model {0} is false: {1}", new object[]
										{
											modelAction.Model.Name,
											expression
										});
									}
								}
								this.ShowHide(modelAction, flag5);
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
		}

		// Token: 0x060012D2 RID: 4818 RVA: 0x000414BC File Offset: 0x0003F6BC
		internal void EnableOrDisableAll(bool enableOrShow)
		{
			foreach (ModelAction modelAction in this.ObservableModels)
			{
				this.ShowHide(modelAction, enableOrShow);
			}
		}

		// Token: 0x060012D3 RID: 4819 RVA: 0x00041510 File Offset: 0x0003F710
		internal void ShowHide(ModelAction modelAction, bool enableOrShow)
		{
			modelAction.SetEnabled(enableOrShow);
			bool flag = modelAction.HideInstead || (this.IsMobile && !enableOrShow);
			if (flag)
			{
				modelAction.SetVisible(enableOrShow);
			}
			modelAction.SetTooltip(modelAction.Model.Tooltip);
		}

		// Token: 0x060012D4 RID: 4820 RVA: 0x00041560 File Offset: 0x0003F760
		private IEnumerable<IAcuModel> GetModels()
		{
			bool isMobile = this.IsMobile;
			Func<IModel, bool> func;
			if (isMobile)
			{
				Type type = this.Graph.GetType();
				Type regularGraphFromMobile = MobileUtils.GetRegularGraphFromMobile(type);
				func = AsgardUtils.GetByGraphType(regularGraphFromMobile.FullName);
			}
			else
			{
				string screenID = AsgardUtils.GetScreenID();
				func = AsgardUtils.GetByScreen(screenID);
			}
			func = BasicHelper.And<IModel>(new Func<IModel, bool>[]
			{
				func,
				BasicHelper.IS_SINGLE_OR_GROUP
			});
			func = BasicHelper.And<IModel>(new Func<IModel, bool>[]
			{
				func,
				BasicHelper.IS_ACTIVE
			});
			IModel[] models = this.LabelHandler.ModelProvider.GetModels(func);
			bool flag = models.Any(BasicHelper.IS_GROUP);
			IEnumerable<IModel> source = models;
			Func<IModel, string> keySelector;
			if ((keySelector = LabelConductor<TGraph>.<>O.<0>__DefaultModelOrder) == null)
			{
				keySelector = (LabelConductor<TGraph>.<>O.<0>__DefaultModelOrder = new Func<IModel, string>(LabelConductor<TGraph>.DefaultModelOrder<IModel>));
			}
			return source.OrderBy(keySelector).Cast<IAcuModel>().ToArray<IAcuModel>();
		}

		// Token: 0x060012D5 RID: 4821 RVA: 0x00041634 File Offset: 0x0003F834
		public IEnumerable<IAcuModel> GetModels(Func<IModel, bool> predicate = null)
		{
			string screenID = AsgardUtils.GetScreenID();
			Func<IModel, bool> byScreen = AsgardUtils.GetByScreen(screenID);
			predicate = BasicHelper.And<IModel>(new Func<IModel, bool>[]
			{
				byScreen,
				predicate
			});
			return this.LabelHandler.ModelProvider.GetModels(predicate).Cast<IAcuModel>().ToArray<IAcuModel>();
		}

		// Token: 0x060012D6 RID: 4822 RVA: 0x00041688 File Offset: 0x0003F888
		private static string DefaultModelOrder<M>(M model) where M : IModel
		{
			return model.Description;
		}

		// Token: 0x060012D7 RID: 4823 RVA: 0x000416A8 File Offset: 0x0003F8A8
		private ModelAction AddPrintAction(IAcuModel model, bool alwaysAllow)
		{
			ILabelGenerator<IAcuLabelContext> labelGenerator = this.LabelHandler.LabelGenerator;
			IEnumerable<IModelDestination> printers = from det in ModelPrinters.GetDetails(model.ID)
			where det.Active.GetValueOrDefault()
			select det;
			AcuUserInfo userInfo = AcuUserInfo.Create(this.Graph);
			Guid? printerID = BasicLabelUtils.ChoosePrinter<IModelDestination>(this.Graph, userInfo, printers, null);
			IAcuPrinter acuPrinter;
			Printers.TryGetValue(printerID, out acuPrinter);
			bool flag = AAConstants.ModelType.IsReal(model.ModelType) && acuPrinter == null && !alwaysAllow;
			ModelAction result;
			if (flag)
			{
				PXTrace.WriteInformation("Model {0} has no printer for you", new object[]
				{
					model.Name
				});
				result = null;
			}
			else
			{
				PXButtonDelegate handler = delegate(PXAdapter adapter)
				{
					PXLongOperation.StartOperation(this.Graph.UID, delegate()
					{
						PXCacheCollection caches = this.Graph.Caches;
						PXCache pxcache = caches[this.Graph.PrimaryItemType];
						bool flag2 = pxcache.IsDirty && pxcache.AllowUpdate && !adapter.ExternalCall;
						if (flag2)
						{
							this.Graph.Actions.PressSave();
						}
						object row = (pxcache != null) ? pxcache.Current : null;
						bool isMobile = this.IsMobile;
						AcuLabelContext acuLabelContext;
						if (isMobile)
						{
							acuLabelContext = AcuLabelContext.CreateMobilePrintContext(this.Graph, row, model.ID, adapter);
							acuLabelContext.IsSilent = true;
						}
						else
						{
							acuLabelContext = AcuLabelContext.CreatePrintContext(this.Graph.GetType(), row, model.ID, false, adapter);
							acuLabelContext.IsSilent = (this.IsAPI || this.Graph.UnattendedMode);
						}
						try
						{
							labelGenerator.PrintLabels(acuLabelContext);
						}
						catch (Exception ex)
						{
							PXTrace.WriteError(ex);
							throw;
						}
					});
					return adapter.Get();
				};
				ModelAction modelAction = this.DoAddPrintAction(model, handler, acuPrinter, "");
				result = modelAction;
			}
			return result;
		}

		// Token: 0x060012D8 RID: 4824 RVA: 0x000417AC File Offset: 0x0003F9AC
		private ModelAction DoAddPrintAction(IAcuModel model, PXButtonDelegate handler, IPrinter printerToUse, string suffix)
		{
			string actionName = BasicLabelUtils.GetActionName(model.ID, suffix);
			string text = suffix + (string.IsNullOrEmpty(suffix) ? "" : " ") + model.Description;
			PXButtonAttribute pxbuttonAttribute = PXEventSubscriberAttribute.CreateInstance<PXButtonAttribute>(Array.Empty<object>());
			pxbuttonAttribute.DisplayOnMainToolbar = false;
			pxbuttonAttribute.PopupVisible = false;
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
			PXAction pxaction = PXNamedAction.AddAction(this.Graph, this.Graph.PrimaryItemType, actionName, text, handler, array);
			PXAction orCreateFolder = this.GetOrCreateFolder();
			orCreateFolder.AddMenuAction(pxaction);
			ModelAction modelAction = new ModelAction(pxaction, model, printerToUse);
			modelAction.SetEnabled(false);
			return modelAction;
		}

		// Token: 0x060012D9 RID: 4825 RVA: 0x0004187C File Offset: 0x0003FA7C
		private PXAction GetOrCreateFolder()
		{
			PXAction pxaction = this.LabelHandler.LabelFolder;
			bool flag = pxaction == null;
			if (flag)
			{
				PXButtonAttribute pxbuttonAttribute = PXEventSubscriberAttribute.CreateInstance<PXButtonAttribute>(Array.Empty<object>());
				pxbuttonAttribute.MenuAutoOpen = true;
				pxbuttonAttribute.CommitChanges = true;
				pxaction = PXNamedAction.AddAction(this.Graph, this.Graph.PrimaryItemType, "ALLabelFolder", "Asgard Labels", null, new PXEventSubscriberAttribute[]
				{
					pxbuttonAttribute
				});
				pxaction.SetEnabled(true);
				pxaction.SetVisible(true);
				this.LabelHandler.LabelFolder = pxaction;
			}
			return pxaction;
		}

		// Token: 0x060012DA RID: 4826 RVA: 0x0004190C File Offset: 0x0003FB0C
		internal void PrintLabels(object row, object oldRow)
		{
			bool flag = row == null;
			if (!flag)
			{
				TemplateContext scribanContext = ScribanUtils.CreateContext(this.Graph, row, oldRow, false, Array.Empty<object>());
				string screenID = AsgardUtils.GetScreenID();
				int? baccountID = AsgardUtils.GetBAccountID(this.Graph, row);
				IEnumerable<AutoPrints.AutoPrintRule> autoPrintRulesByScreenID = AutoPrints.GetAutoPrintRulesByScreenID(screenID, baccountID);
				foreach (AutoPrints.AutoPrintRule autoPrintRule in autoPrintRulesByScreenID)
				{
					IModel model = this.LabelHandler.ModelProvider.GetModel(autoPrintRule.ModelID);
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

		// Token: 0x060012DB RID: 4827 RVA: 0x00041A74 File Offset: 0x0003FC74
		private void PrintLabel(object row, AutoPrints.AutoPrintRule autoPrint, IModel model)
		{
			bool flag = model != null;
			if (flag)
			{
				using (PXTransactionScope pxtransactionScope = new PXTransactionScope())
				{
					AcuLabelGenerator acuLabelGenerator = new AcuLabelGenerator();
					AcuLabelContext acuLabelContext = this.IsMobile ? AcuLabelContext.CreateMobilePrintContext(this.Graph, row, model.ID, null) : AcuLabelContext.CreatePrintContext(this.Graph.GetType(), row, model.ID, true, null);
					acuLabelContext.IsSilent = true;
					acuLabelContext.IgnorePrinterMissing = true;
					PrintResults printResults = acuLabelGenerator.PrintLabels(acuLabelContext);
					pxtransactionScope.Complete();
				}
			}
		}

		// Token: 0x0200085B RID: 2139
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x04000E3F RID: 3647
			public static Func<IModel, string> <0>__DefaultModelOrder;
		}
	}
}
