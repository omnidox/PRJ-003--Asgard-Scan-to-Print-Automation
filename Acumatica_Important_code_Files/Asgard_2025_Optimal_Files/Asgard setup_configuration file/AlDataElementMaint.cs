using System;
using System.Collections;
using System.Linq;
using AA.Objects.Core;
using PX.Data;
using PX.SM;

namespace AA.Objects.Labels
{
	// Token: 0x02000125 RID: 293
	public class ALDataElementMaint : PXGraph<ALDataElementMaint, ALDataElement>
	{
		// Token: 0x17000573 RID: 1395
		// (get) Token: 0x06000D05 RID: 3333 RVA: 0x000210C9 File Offset: 0x0001F2C9
		// (set) Token: 0x06000D06 RID: 3334 RVA: 0x000210D1 File Offset: 0x0001F2D1
		[InjectDependency]
		private IEntityContextFactory _entityContextFactory { get; set; }

		// Token: 0x06000D07 RID: 3335 RVA: 0x000210DC File Offset: 0x0001F2DC
		public ALDataElementMaint()
		{
			this.Action.MenuAutoOpen = true;
			this.Action.AddMenuAction(this.ChangeID);
			this.Action.AddMenuAction(this.clearCache);
			this.UsedByModels.AllowUpdate = false;
			this.UsedByModels.AllowDelete = false;
			this.UsedByModels.AllowInsert = false;
		}

		// Token: 0x06000D08 RID: 3336 RVA: 0x00021148 File Offset: 0x0001F348
		public IEnumerable entityItemsBasedOn([PXString] string parent)
		{
			ALDataElement aldataElement = this.CurrentDocument.Current;
			string screenID = (aldataElement != null) ? aldataElement.ScreenID : null;
			ALDataElement aldataElement2 = this.CurrentDocument.Current;
			string exprType = (aldataElement2 != null) ? aldataElement2.ExprType : null;
			return this.GetBasedOnChoices(parent, screenID, exprType);
		}

		// Token: 0x06000D09 RID: 3337 RVA: 0x00021194 File Offset: 0x0001F394
		public IEnumerable entityItemsSampleBasedOn([PXString] string parent)
		{
			ALDataElement aldataElement = this.CurrentDocument.Current;
			string screenID = (aldataElement != null) ? aldataElement.ScreenID : null;
			ALDataElement aldataElement2 = this.CurrentDocument.Current;
			string exprType = (aldataElement2 != null) ? aldataElement2.SampleType : null;
			return this.GetBasedOnChoices(parent, screenID, exprType);
		}

		// Token: 0x06000D0A RID: 3338 RVA: 0x000211E0 File Offset: 0x0001F3E0
		private IEnumerable GetBasedOnChoices(string parent, string screenID, string exprType)
		{
			bool flag = !string.IsNullOrEmpty(parent);
			IEnumerable empty_LIST;
			if (flag)
			{
				empty_LIST = EntityContextFactory.EMPTY_LIST;
			}
			else
			{
				if (!(exprType == "S"))
				{
					if (exprType == "F")
					{
						return this._entityContextFactory.GetLibrariesAsEntityItems(null);
					}
				}
				else
				{
					bool flag2 = !string.IsNullOrEmpty(screenID);
					if (flag2)
					{
						IEntityContext contextByScreenID = this._entityContextFactory.GetContextByScreenID(screenID);
						return contextByScreenID.GetEntityItemsImplByScreen(null);
					}
				}
				empty_LIST = EntityContextFactory.EMPTY_LIST;
			}
			return empty_LIST;
		}

		// Token: 0x06000D0B RID: 3339 RVA: 0x00021268 File Offset: 0x0001F468
		public IEnumerable entityItemsExprValue([PXString] string parent)
		{
			ALDataElement aldataElement = this.CurrentDocument.Current;
			string screenID = (aldataElement != null) ? aldataElement.ScreenID : null;
			ALDataElement aldataElement2 = this.CurrentDocument.Current;
			string exprType = (aldataElement2 != null) ? aldataElement2.ExprType : null;
			ALDataElement aldataElement3 = this.CurrentDocument.Current;
			string basedOn = (aldataElement3 != null) ? aldataElement3.BasedOn : null;
			return this.GetExprValueChoices(ref parent, screenID, exprType, basedOn);
		}

		// Token: 0x06000D0C RID: 3340 RVA: 0x000212D0 File Offset: 0x0001F4D0
		public IEnumerable entityItemsSampleValue([PXString] string parent)
		{
			ALDataElement aldataElement = this.CurrentDocument.Current;
			string screenID = (aldataElement != null) ? aldataElement.ScreenID : null;
			ALDataElement aldataElement2 = this.CurrentDocument.Current;
			string exprType = (aldataElement2 != null) ? aldataElement2.SampleType : null;
			ALDataElement aldataElement3 = this.CurrentDocument.Current;
			string basedOn = (aldataElement3 != null) ? aldataElement3.SampleBasedOn : null;
			return this.GetExprValueChoices(ref parent, screenID, exprType, basedOn);
		}

		// Token: 0x06000D0D RID: 3341 RVA: 0x00021338 File Offset: 0x0001F538
		private IEnumerable GetExprValueChoices(ref string parent, string screenID, string exprType, string basedOn)
		{
			bool flag = string.IsNullOrEmpty(basedOn);
			IEnumerable empty_LIST;
			if (flag)
			{
				empty_LIST = EntityContextFactory.EMPTY_LIST;
			}
			else
			{
				if (parent == null)
				{
					parent = basedOn;
				}
				if (!(exprType == "S"))
				{
					if (exprType == "F")
					{
						return this._entityContextFactory.GetLibrariesAsEntityItems(basedOn);
					}
				}
				else
				{
					bool flag2 = !string.IsNullOrEmpty(screenID);
					if (flag2)
					{
						IEntityContext contextByScreenID = this._entityContextFactory.GetContextByScreenID(screenID);
						return contextByScreenID.GetEntityItemsImplByScreen(parent);
					}
				}
				empty_LIST = EntityContextFactory.EMPTY_LIST;
			}
			return empty_LIST;
		}

		// Token: 0x06000D0E RID: 3342 RVA: 0x0001ABA3 File Offset: 0x00018DA3
		[PXMergeAttributes(Method = 0)]
		[PXCustomizeBaseAttribute(typeof(ALNameAttribute), "IsKey", true)]
		protected virtual void _(Events.CacheAttached<ALDataElement.name> e)
		{
		}

		// Token: 0x06000D0F RID: 3343 RVA: 0x0001ABA3 File Offset: 0x00018DA3
		[PXMergeAttributes(Method = 0)]
		[PXCustomizeBaseAttribute(typeof(ALDataSourceIDForeignAttribute), "Visible", true)]
		protected virtual void _(Events.CacheAttached<ALDataElement.sourceID> e)
		{
		}

		// Token: 0x06000D10 RID: 3344 RVA: 0x0001ABA3 File Offset: 0x00018DA3
		[PXMergeAttributes(Method = 0)]
		[PXCustomizeBaseAttribute(typeof(ALGuidIDAttribute), "IsKey", false)]
		protected virtual void _(Events.CacheAttached<ALDataElement.recordID> e)
		{
		}

		// Token: 0x06000D11 RID: 3345 RVA: 0x0001ABA3 File Offset: 0x00018DA3
		[PXMergeAttributes(Method = 0)]
		[PXRemoveBaseAttribute(typeof(PXDefaultAttribute))]
		[PXDefault(false, PersistingCheck = 2)]
		[PXCustomizeBaseAttribute(typeof(PXUIFieldAttribute), "Enabled", false)]
		protected virtual void _(Events.CacheAttached<ALDataElement.genName> e)
		{
		}

		// Token: 0x06000D12 RID: 3346 RVA: 0x000213CC File Offset: 0x0001F5CC
		protected virtual void _(Events.RowSelected<ALDataElement> e)
		{
			ALDataElement row = e.Row;
			bool flag = row == null;
			if (!flag)
			{
				bool flag2 = UsedDataElements.IsUsedBy(row.RecordID).Any<UsedDataElements.DataElement>();
				PXCache cache = e.Cache;
				PXUIFieldAttribute.SetEnabled<ALDataElement.exprType>(cache, row, !flag2);
			}
		}

		// Token: 0x06000D13 RID: 3347 RVA: 0x00021410 File Offset: 0x0001F610
		protected virtual void _(Events.RowDeleting<ALDataElement> e)
		{
			UsedDataElements.ThrowIfUsed(e.Row);
		}

		// Token: 0x06000D14 RID: 3348 RVA: 0x00021420 File Offset: 0x0001F620
		protected virtual void _(Events.FieldUpdated<ALDataElement, ALDataElement.sourceID> e)
		{
			ALDataElement row = e.Row;
			bool flag = row == null;
			if (!flag)
			{
				PXCache cache = e.Cache;
				Guid? sourceID = e.NewValue as Guid?;
				ALDataSource aldataSource = ALDataSource.PK.Find(this, sourceID);
				bool flag2 = aldataSource != null;
				if (flag2)
				{
					this.CurrentDataSource.Current = aldataSource;
					PXLineNbrAttribute pxlineNbrAttribute = cache.GetAttributes<ALDataElement.lineNbr>().OfType<PXLineNbrAttribute>().FirstOrDefault<PXLineNbrAttribute>();
					row.LineNbr = (int?)PXLineNbrAttribute.NewLineNbr<ALDataElement.lineNbr>(cache, row);
				}
			}
		}

		// Token: 0x06000D15 RID: 3349 RVA: 0x000214A0 File Offset: 0x0001F6A0
		[PXUIField]
		[PXButton(MenuAutoOpen = true)]
		protected virtual IEnumerable action(PXAdapter adapter)
		{
			return adapter.Get();
		}

		// Token: 0x06000D16 RID: 3350 RVA: 0x000214B8 File Offset: 0x0001F6B8
		[PXUIField]
		[PXButton]
		public virtual IEnumerable ClearCache(PXAdapter adapter)
		{
			IEntityContextFactory entityContextFactory = this._entityContextFactory;
			if (entityContextFactory != null)
			{
				entityContextFactory.ClearCaches();
			}
			return adapter.Get();
		}

		// Token: 0x04000581 RID: 1409
		[ALImportExportAll]
		[PXCopyPasteHiddenFields(new Type[]
		{
			typeof(ALDataElement.recordID)
		})]
		public PXSelect<ALDataElement> Document;

		// Token: 0x04000582 RID: 1410
		public PXSelect<ALDataElement, Where<ALDataElement.recordID, Equal<Current<ALDataElement.recordID>>>> CurrentDocument;

		// Token: 0x04000583 RID: 1411
		public PXSelect<ALDataSource, Where<ALDataSource.sourceID, Equal<Current<ALDataElement.sourceID>>>> CurrentDataSource;

		// Token: 0x04000584 RID: 1412
		[PXCopyPasteHiddenView]
		public PXSelectJoin<ALModelExpr, LeftJoin<ALModel, On<ALModel.labelID, Equal<ALModelExpr.labelID>>, LeftJoin<ALDataElement, On<ALDataElement.recordID, Equal<ALModelExpr.dataElementID>>>>, Where<ALModelExpr.dataElementID, Equal<Current<ALDataElement.recordID>>>, OrderBy<Asc<ALModel.name, Asc<ALModelExpr.lineNbr>>>> UsedByModels;

		// Token: 0x04000585 RID: 1413
		[PXCopyPasteHiddenView]
		public PXSelectJoin<ALContentElement, LeftJoin<ALContent, On<ALContent.contentID, Equal<ALContentElement.contentID>>>, Where<ALContentElement.dataElementID, Equal<Current<ALDataElement.recordID>>>, OrderBy<Asc<ALContent.name, Asc<ALContentElement.lineNbr>>>> UsedByContents;

		// Token: 0x04000586 RID: 1414
		[PXCopyPasteHiddenView]
		public PXSetup<ALSetup> LabelSetup;

		// Token: 0x04000588 RID: 1416
		[PXCopyPasteHiddenView]
		public PXSelect<CacheEntityItem, Where<CacheEntityItem.path, Equal<CacheEntityItem.path>>, OrderBy<Asc<CacheEntityItem.number>>> EntityItemsBasedOn;

		// Token: 0x04000589 RID: 1417
		[PXCopyPasteHiddenView]
		public PXSelect<CacheEntityItem, Where<CacheEntityItem.path, Equal<CacheEntityItem.path>>, OrderBy<Asc<CacheEntityItem.number>>> EntityItemsExprValue;

		// Token: 0x0400058A RID: 1418
		[PXCopyPasteHiddenView]
		public PXSelect<CacheEntityItem, Where<CacheEntityItem.path, Equal<CacheEntityItem.path>>, OrderBy<Asc<CacheEntityItem.number>>> EntityItemsSampleBasedOn;

		// Token: 0x0400058B RID: 1419
		[PXCopyPasteHiddenView]
		public PXSelect<CacheEntityItem, Where<CacheEntityItem.path, Equal<CacheEntityItem.path>>, OrderBy<Asc<CacheEntityItem.number>>> EntityItemsSampleValue;

		// Token: 0x0400058C RID: 1420
		public ALChangeID<ALDataElement, ALDataElement.name> ChangeID;

		// Token: 0x0400058D RID: 1421
		public PXAction<ALDataElement> Action;

		// Token: 0x0400058E RID: 1422
		public PXAction<ALDataSource> clearCache;
	}
}
