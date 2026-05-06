using System;
using System.Collections;
using System.Collections.Generic;
using AA.Objects.Core;
using Asgard.Labels.Abstractions.Helpers;
using Asgard.Labels.Abstractions.Interface;
using Asgard.Labels.Impl.Context;
using PX.Data;
using PX.SM;

namespace AA.Objects.Labels
{
	// Token: 0x0200013E RID: 318
	public class ALRuleMaint : PXGraph<ALRuleMaint, ALRule>
	{
		// Token: 0x17000590 RID: 1424
		// (get) Token: 0x06000E69 RID: 3689 RVA: 0x0002AA10 File Offset: 0x00028C10
		// (set) Token: 0x06000E6A RID: 3690 RVA: 0x0002AA18 File Offset: 0x00028C18
		[InjectDependency]
		private IPXPageIndexingService _pageIndexingService { get; set; }

		// Token: 0x17000591 RID: 1425
		// (get) Token: 0x06000E6B RID: 3691 RVA: 0x0002AA21 File Offset: 0x00028C21
		// (set) Token: 0x06000E6C RID: 3692 RVA: 0x0002AA29 File Offset: 0x00028C29
		[InjectDependency]
		private IEntityContextFactory _entityContextFactory { get; set; }

		// Token: 0x06000E6D RID: 3693 RVA: 0x0002AA34 File Offset: 0x00028C34
		public ALRuleMaint()
		{
			this.Action.MenuAutoOpen = true;
			this.Action.AddMenuAction(this.ChangeID);
			this.Action.AddMenuAction(this.ClearCache);
			this.Action.AddMenuAction(this.RefreshComposite);
		}

		// Token: 0x06000E6E RID: 3694 RVA: 0x0002AA8C File Offset: 0x00028C8C
		protected virtual void _(Events.RowSelected<ALRule> e)
		{
			ALRule row = e.Row;
			bool flag = row == null;
			if (!flag)
			{
				bool valueOrDefault = row.IsComposite.GetValueOrDefault();
				bool valueOrDefault2 = row.Active.GetValueOrDefault();
				this.Document.Cache.AllowDelete = !valueOrDefault2;
				this.Details.AllowDelete = valueOrDefault;
				this.Details.AllowInsert = valueOrDefault;
				this.Details.AllowUpdate = valueOrDefault;
				bool enabled = !string.IsNullOrEmpty(row.ScreenID);
				this.ViewScreen.SetEnabled(enabled);
				this.RefreshComposite.SetEnabled(valueOrDefault);
				this.DuplicateDeep.SetEnabled(valueOrDefault);
			}
		}

		// Token: 0x06000E6F RID: 3695 RVA: 0x0002AB44 File Offset: 0x00028D44
		protected virtual void _(Events.FieldUpdating<ALRule, ALRule.expression> e)
		{
			ALRule row = e.Row;
			bool flag = row == null || e.NewValue == null;
			if (!flag)
			{
				string primaryView = this._pageIndexingService.GetPrimaryView(row.GraphType);
				IEntityContext contextByScreenID = this._entityContextFactory.GetContextByScreenID(row.ScreenID);
				object obj = DataElementUtils.CleanupExpr(contextByScreenID, primaryView, e.NewValue, false);
				bool flag2 = e.NewValue.ToString() != (string)obj;
				if (flag2)
				{
					e.NewValue = obj;
				}
			}
		}

		// Token: 0x06000E70 RID: 3696 RVA: 0x0002ABCC File Offset: 0x00028DCC
		public IEnumerable entityItems(string parent)
		{
			ALRule alrule = this.Document.Current;
			string screenID = (alrule != null) ? alrule.ScreenID : null;
			IEntityContext contextByScreenID = this._entityContextFactory.GetContextByScreenID(screenID);
			return contextByScreenID.GetEntityItemsImplByScreen(parent);
		}

		// Token: 0x06000E71 RID: 3697 RVA: 0x0002AC0C File Offset: 0x00028E0C
		[PXUIField]
		[PXButton(MenuAutoOpen = true)]
		protected virtual IEnumerable action(PXAdapter adapter)
		{
			return adapter.Get();
		}

		// Token: 0x06000E72 RID: 3698 RVA: 0x0002AC24 File Offset: 0x00028E24
		[PXButton]
		[PXUIField(DisplayName = "View Screen")]
		protected void viewScreen()
		{
			ALRule alrule = this.Document.Current;
			bool flag = !string.IsNullOrEmpty((alrule != null) ? alrule.ScreenID : null);
			if (flag)
			{
				PXSiteMapNode pxsiteMapNode = PXSiteMap.Provider.FindSiteMapNodeByScreenID(alrule.ScreenID);
				bool flag2 = pxsiteMapNode != null && !string.IsNullOrEmpty(pxsiteMapNode.Url);
				if (flag2)
				{
					throw new PXRedirectToUrlException(pxsiteMapNode.Url, 2, "ViewScreen");
				}
			}
		}

		// Token: 0x06000E73 RID: 3699 RVA: 0x0002AC94 File Offset: 0x00028E94
		[PXUIField]
		[PXButton]
		protected virtual IEnumerable refreshComposite(PXAdapter adapter)
		{
			ALRule alrule = this.Document.Current;
			bool valueOrDefault = alrule.IsComposite.GetValueOrDefault();
			if (valueOrDefault)
			{
				ALRuleMaint alruleMaint = HiddenUtils.CreateInstance<ALRuleMaint>();
				alruleMaint.DoRefreshComposite(alrule);
				this.Actions.PressCancel();
			}
			return adapter.Get();
		}

		// Token: 0x06000E74 RID: 3700 RVA: 0x0002ACE8 File Offset: 0x00028EE8
		[PXUIField]
		[PXButton]
		protected void duplicate()
		{
			ALRule alrule = this.Document.Current;
			bool flag = alrule != null;
			if (flag)
			{
				this.DoDuplicate(alrule, false);
			}
		}

		// Token: 0x06000E75 RID: 3701 RVA: 0x0002AD18 File Offset: 0x00028F18
		[PXUIField]
		[PXButton]
		protected void duplicateDeep()
		{
			ALRule alrule = this.Document.Current;
			bool flag = alrule != null;
			if (flag)
			{
				this.DoDuplicate(alrule, true);
			}
		}

		// Token: 0x06000E76 RID: 3702 RVA: 0x0002AD48 File Offset: 0x00028F48
		private void DoDuplicate(ALRule currentRule, bool deep)
		{
			ALRuleMaint alruleMaint = HiddenUtils.CreateInstance<ALRuleMaint>();
			ALRule alrule = alruleMaint.DoDuplicateInternal(currentRule.RuleID, deep);
			alruleMaint.Actions.PressSave();
			ALRule alrule2 = alruleMaint.Document.Current = alruleMaint.Document.Search<ALRule.ruleID>(alrule.RuleID, Array.Empty<object>());
			throw new PXRedirectRequiredException(alruleMaint, true, "ViewDuplicate");
		}

		// Token: 0x06000E77 RID: 3703 RVA: 0x0002ADB4 File Offset: 0x00028FB4
		private ALRule DoDuplicateInternal(Guid? ruleID, bool deep = false)
		{
			PXCache cache = this.Document.Cache;
			ALRule alrule = ALRule.PK.Find(this, ruleID);
			ALRule alrule2 = PXCache<ALRule>.CreateCopy(alrule);
			cache.SetDefaultExt<ALRule.ruleID>(alrule2);
			bool flag = alrule2.CategoryID == null;
			if (flag)
			{
				cache.SetDefaultExt<ALRule.categoryID>(alrule2);
			}
			string name = ALRuleMaint.GetName(alrule2.Name);
			alrule2.Name = name;
			alrule2.Description = ("Copy of " + alrule2.Description).Truncate(256);
			alrule2.NoteID = null;
			alrule2.tstamp = null;
			ALRule alrule3 = this.Document.Insert(alrule2);
			alrule3 = this.Document.Search<ALRule.ruleID>(alrule3.RuleID, Array.Empty<object>());
			Guid? ruleID2 = alrule3.RuleID;
			bool valueOrDefault = alrule.IsComposite.GetValueOrDefault();
			if (valueOrDefault)
			{
				PXResultset<ALRuleDetail> pxresultset = new PXSelectJoin<ALRuleDetail, LeftJoin<ALRule, On<ALRuleDetail.FK.Child>>, Where<ALRuleDetail.ruleID, Equal<Required<ALRuleDetail.ruleID>>>>(this).Select(new object[]
				{
					ruleID
				});
				foreach (PXResult<ALRuleDetail> pxresult in pxresultset)
				{
					ALRuleDetail alruleDetail = PXResult.Unwrap<ALRuleDetail>(pxresult);
					ALRule alrule4 = PXResult.Unwrap<ALRule>(pxresult);
					Guid? ruleID3 = (alrule4 != null) ? alrule4.RuleID : null;
					bool flag2 = ruleID3 == null;
					if (flag2)
					{
						throw new PXException("A rule child is missing for Rule {0} and line {1}", new object[]
						{
							alrule.Name,
							alruleDetail.LineNbr
						});
					}
					ALRuleDetail alruleDetail2 = PXCache<ALRuleDetail>.CreateCopy(alruleDetail);
					if (deep)
					{
						ALRule alrule5 = this.DoDuplicateInternal(ruleID3, deep);
						alruleDetail2.ChildRuleID = alrule5.RuleID;
					}
					alruleDetail2.RuleID = ruleID2;
					alruleDetail2.NoteID = null;
					alruleDetail2.tstamp = null;
					ALRuleDetail alruleDetail3 = this.Details.Insert(alruleDetail2);
				}
			}
			return alrule3;
		}

		// Token: 0x06000E78 RID: 3704 RVA: 0x0002AFD8 File Offset: 0x000291D8
		private static string GetName(string name)
		{
			ICollection<string> names = Rules.GetNames();
			for (int i = 1; i < 100; i++)
			{
				string text = string.Format("{0}-{1}", name, i);
				bool flag = !names.Contains(text);
				if (flag)
				{
					return text;
				}
			}
			throw new PXException("Could not get a new name for duplicate of {0} {1}", new object[]
			{
				"Rule",
				name
			});
		}

		// Token: 0x06000E79 RID: 3705 RVA: 0x0002B048 File Offset: 0x00029248
		private void DoRefreshComposite(IRule currentRule)
		{
			AcuLabelContext lc = AcuLabelContext.CreateTestContext(null, null);
			IEnumerable<IRule> dependencies = RuleUtils.GetDependencies(lc, currentRule);
			List<IRule> list = new List<IRule>(dependencies)
			{
				currentRule
			};
			foreach (IRule rule in list)
			{
				ALRule alrule = ALRule.PK.Find(this, rule.ID);
				this.Document.Current = alrule;
				string expression = RuleUtils.GetExpression(lc, rule);
				bool flag = expression != alrule.Expression;
				if (flag)
				{
					alrule.Expression = expression;
					this.Document.Cache.Update(alrule);
					this.Document.Cache.PersistUpdated(alrule);
				}
			}
			Rules.Reset();
			RuleDetails.Reset();
		}

		// Token: 0x06000E7A RID: 3706 RVA: 0x0002B130 File Offset: 0x00029330
		[PXUIField]
		[PXButton]
		public virtual IEnumerable clearCache(PXAdapter adapter)
		{
			Rules.Reset();
			RuleDetails.Reset();
			return adapter.Get();
		}

		// Token: 0x0400068F RID: 1679
		[ALImportExportAll]
		[PXViewName("Rules")]
		[PXCopyPasteHiddenFields(new Type[]
		{
			typeof(ALRule.ruleID)
		})]
		public PXSelect<ALRule> Document;

		// Token: 0x04000690 RID: 1680
		public PXSelect<ALRule, Where<ALRule.ruleID, Equal<Current<ALRule.ruleID>>>> CurrentDocument;

		// Token: 0x04000691 RID: 1681
		[PXViewName("Rule Details")]
		public PXOrderedSelect<ALRule, ALRuleDetail, Where<ALRuleDetail.ruleID, Equal<Current<ALRule.ruleID>>>, OrderBy<Asc<ALRuleDetail.sortOrder, Asc<ALRuleDetail.lineNbr>>>> Details;

		// Token: 0x04000692 RID: 1682
		[PXCopyPasteHiddenView]
		public PXSelect<ALModel, Where<ALModel.filterRuleID, Equal<Optional<ALRule.ruleID>>, Or<ALModel.printRuleID, Equal<Optional<ALRule.ruleID>>>>, OrderBy<Asc<ALModel.name>>> UsedByModels;

		// Token: 0x04000693 RID: 1683
		[PXCopyPasteHiddenView]
		public PXSelectJoin<ALModelExpr, InnerJoin<ALModel, On<ALModelExpr.FK.Parent>>, Where<ALModelExpr.ruleID, Equal<Current<ALRule.ruleID>>>, OrderBy<Asc<ALModel.name, Asc<ALModelExpr.lineNbr>>>> UsedByExprs;

		// Token: 0x04000694 RID: 1684
		[PXCopyPasteHiddenView]
		public PXSelectJoin<ALRuleDetail, InnerJoin<ALRule, On<ALRuleDetail.FK.Parent>>, Where<ALRuleDetail.childRuleID, Equal<Current<ALRule.ruleID>>>, OrderBy<Asc<ALRule.name, Asc<ALRuleDetail.lineNbr>>>> UsedByComposites;

		// Token: 0x04000695 RID: 1685
		[PXCopyPasteHiddenView]
		public PXSelectJoin<ALColorRule, InnerJoin<ALColor, On<ALColorRule.FK.Parent>>, Where<ALColorRule.ruleID, Equal<Current<ALRule.ruleID>>>, OrderBy<Asc<ALColor.name, Asc<ALColorRule.lineNbr>>>> UsedByColors;

		// Token: 0x04000696 RID: 1686
		[PXCopyPasteHiddenView]
		public PXSelectJoin<ALFormatRule, InnerJoin<ALFormat, On<ALFormatRule.FK.Parent>>, Where<ALFormatRule.ruleID, Equal<Current<ALRule.ruleID>>>, OrderBy<Asc<ALFormat.name, Asc<ALFormatRule.lineNbr>>>> UsedByFormats;

		// Token: 0x04000697 RID: 1687
		[PXCopyPasteHiddenView]
		public PXSelectJoin<ALContentElement, InnerJoin<ALContent, On<ALContentElement.FK.Parent>>, Where<ALContentElement.ruleID, Equal<Current<ALRule.ruleID>>>, OrderBy<Asc<ALContent.name, Asc<ALContentElement.lineNbr>>>> UsedByContentElements;

		// Token: 0x04000698 RID: 1688
		[PXCopyPasteHiddenView]
		public PXSelect<ALAutoPrint, Where<ALAutoPrint.ruleID, Equal<Current<ALRule.ruleID>>>, OrderBy<Asc<ALAutoPrint.name>>> UsedByAutoPrints;

		// Token: 0x04000699 RID: 1689
		[PXCopyPasteHiddenView]
		public PXSelect<CacheEntityItem, Where<CacheEntityItem.path, Equal<CacheEntityItem.path>>, OrderBy<Asc<CacheEntityItem.number>>> EntityItems;

		// Token: 0x0400069A RID: 1690
		[PXCopyPasteHiddenView]
		public PXSetup<ALSetup> LabelSetup;

		// Token: 0x0400069D RID: 1693
		public ALChangeID<ALRule, ALRule.name> ChangeID;

		// Token: 0x0400069E RID: 1694
		public PXAction<ALRule> Action;

		// Token: 0x0400069F RID: 1695
		public PXAction<ALRule> ViewScreen;

		// Token: 0x040006A0 RID: 1696
		public PXAction<ALRule> RefreshComposite;

		// Token: 0x040006A1 RID: 1697
		public PXAction<ALRule> Duplicate;

		// Token: 0x040006A2 RID: 1698
		public PXAction<ALRule> DuplicateDeep;

		// Token: 0x040006A3 RID: 1699
		public PXAction<ALRule> ClearCache;
	}
}
