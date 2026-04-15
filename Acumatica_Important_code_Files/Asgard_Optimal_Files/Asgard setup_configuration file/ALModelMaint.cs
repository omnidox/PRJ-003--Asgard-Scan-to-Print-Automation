using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using AA.Objects.Core;
using AA.Objects.Labels.LabelZoom;
using AA.Objects.License;
using Asgard.Labels.Abstractions.Helpers;
using Asgard.Labels.Abstractions.Interface;
using Asgard.Labels.Abstractions.Poco;
using Asgard.Labels.Impl;
using Asgard.Labels.Impl.Context;
using Asgard.Labels.Impl.Poco;
using PX.Common;
using PX.Data;
using PX.Data.BQL;
using PX.Data.BQL.Fluent;
using PX.Data.DependencyInjection;
using PX.Objects.Common;
using PX.SM;

namespace AA.Objects.Labels
{
	// Token: 0x02000133 RID: 307
	public class ALModelMaint : PXGraph<ALModelMaint, ALModel>, IGraphWithInitialization, PXImportAttribute.IPXPrepareItems, PXImportAttribute.IPXProcess
	{
		// Token: 0x17000584 RID: 1412
		// (get) Token: 0x06000D84 RID: 3460 RVA: 0x000237CE File Offset: 0x000219CE
		// (set) Token: 0x06000D85 RID: 3461 RVA: 0x000237D6 File Offset: 0x000219D6
		public MultiDuplicatesSearchEngine<ALModelExpr> DuplicateFinder { get; set; }

		// Token: 0x17000585 RID: 1413
		// (get) Token: 0x06000D86 RID: 3462 RVA: 0x000237DF File Offset: 0x000219DF
		// (set) Token: 0x06000D87 RID: 3463 RVA: 0x000237E7 File Offset: 0x000219E7
		[InjectDependency]
		private IALLicenseManagerFactory LicenseManagerFactory { get; set; }

		// Token: 0x17000586 RID: 1414
		// (get) Token: 0x06000D88 RID: 3464 RVA: 0x000237F0 File Offset: 0x000219F0
		internal IALLicenseManager LicenseManager
		{
			get
			{
				return this.LicenseManagerFactory.GetLicenseManager(ALConstants.ProductCode);
			}
		}

		// Token: 0x17000587 RID: 1415
		// (get) Token: 0x06000D89 RID: 3465 RVA: 0x00023802 File Offset: 0x00021A02
		// (set) Token: 0x06000D8A RID: 3466 RVA: 0x0002380A File Offset: 0x00021A0A
		[InjectDependency]
		private ILabelGenerator<IAcuLabelContext> _labelGenerator { get; set; }

		// Token: 0x17000588 RID: 1416
		// (get) Token: 0x06000D8B RID: 3467 RVA: 0x00023813 File Offset: 0x00021A13
		// (set) Token: 0x06000D8C RID: 3468 RVA: 0x0002381B File Offset: 0x00021A1B
		[InjectDependency]
		private IEntityContextFactory _entityContextFactory { get; set; }

		// Token: 0x17000589 RID: 1417
		// (get) Token: 0x06000D8D RID: 3469 RVA: 0x00023824 File Offset: 0x00021A24
		public bool HasExpressions
		{
			get
			{
				return this.Expressions.SelectMain(Array.Empty<object>()).Any<ALModelExpr>();
			}
		}

		// Token: 0x1700058A RID: 1418
		// (get) Token: 0x06000D8E RID: 3470 RVA: 0x0002383B File Offset: 0x00021A3B
		public bool HasGraphics
		{
			get
			{
				return this.Graphics.SelectMain(Array.Empty<object>()).Any<ALModelGraphic>();
			}
		}

		// Token: 0x06000D8F RID: 3471 RVA: 0x00023854 File Offset: 0x00021A54
		public void Initialize()
		{
			this.Action.AddMenuAction(this.DeleteRenderings);
			this.Action.AddMenuAction(this.ViewImage);
			this.Action.AddMenuAction(this.ChangeID);
			this.Action.AddMenuAction(this.ClearCache);
			this.Action.AddMenuAction(this.GenerateSource);
			this.Action.AddMenuAction(this.LoadLabelZoom);
			this.PrintAsPDF.IsMass = true;
			this.PrintAsZPL.IsMass = true;
			this.Render.IsMass = true;
		}

		// Token: 0x06000D90 RID: 3472 RVA: 0x0001ABA3 File Offset: 0x00018DA3
		[PXMergeAttributes(Method = 0)]
		[PXCustomizeBaseAttribute(typeof(PXUIFieldAttribute), "IsReadOnly", true)]
		protected virtual void _(Events.CacheAttached<ALPrintLog.recordID> e)
		{
		}

		// Token: 0x06000D91 RID: 3473 RVA: 0x000238F8 File Offset: 0x00021AF8
		protected virtual void _(Events.RowSelected<ALModel> e)
		{
			ALModel row = e.Row;
			bool flag = row == null || this.IsCopyPasteContext;
			if (!flag)
			{
				PXCache cache = e.Cache;
				bool flag2 = row.ModelType == "G";
				bool flag3 = row.ModelType == "L" && !string.IsNullOrEmpty(row.CloudID);
				bool flag4 = row.ModelType == "P";
				bool hasExpressions = this.HasExpressions;
				bool hasGraphics = this.HasGraphics;
				bool enabled = hasExpressions || hasGraphics;
				bool flag5 = !flag2 && !flag4 && hasExpressions;
				bool devMode = ALSetupSlot.DevMode;
				bool valueOrDefault = row.IsSystem.GetValueOrDefault();
				bool isImport = this.IsImport;
				bool flag6 = !valueOrDefault || devMode || isImport;
				bool enabled2 = !string.IsNullOrEmpty(row.ScreenID);
				bool flag7 = valueOrDefault && !devMode && !this.IsImport;
				this.HandleAllowEditDetail(flag7);
				AsgardUtils.ShowEnableIf<ALModel.language>(cache, row, !hasExpressions, true);
				AsgardUtils.ShowEnableIf<ALModel.body>(cache, row, !flag7, true);
				AsgardUtils.ShowEnableIf<ALModel.modelType>(cache, row, !flag3, true);
				this.Render.SetEnabled(enabled);
				this.GenerateSource.SetEnabled(flag5);
				this.ViewImage.SetEnabled(flag5);
				this.PrintAsZPL.SetEnabled(flag5);
				this.PrintAsPDF.SetEnabled(flag5);
				this.ViewScreen.SetEnabled(enabled2);
				this.ViewImage.SetEnabled(flag5);
				this.ChangeID.SetEnabled(flag6);
				bool flag8 = ALSetupSlot.LabelZoomAPIKey != null;
				this.LoadLabelZoom.SetEnabled(flag8);
				this.LoadLabelZoomDetails.SetEnabled(flag8 && flag3);
				this.ClearLabelZoomDetails.SetEnabled(flag8 && flag3);
				this.LoadLabelZoom.SetVisible(flag8);
				this.LoadLabelZoomDetails.SetVisible(flag8 && flag3);
				this.ClearLabelZoomDetails.SetVisible(flag8 && flag3);
				AsgardUtils.EnableOrHide(this.LoadDataElements, flag6 && flag5);
				IFormat item = BasicLabelUtils.GetFormats(row, null).Item1;
				bool flag9 = row.Language == "EZP" && AAConstants.Rotation.HasRotation(item.Rotation);
				if (flag9)
				{
					cache.RaiseExceptionHandling<ALModel.formatID>(e.Row, item.ID, new PXSetPropertyException(row, "Rotation with colors is not supported", 2));
				}
				ALModel almodel = this.Model.Current;
				bool flag10 = almodel.Language == "EZP";
				bool flag11 = almodel.Language == "ZPL";
				bool flag12 = almodel.Language == "PDF";
				bool flag13 = flag11 || flag10;
				bool flag14 = flag10 || flag12;
				AsgardUtils.ShowEnableIf<ALModelGraphic.backColorID>(this.Graphics.Cache, null, flag14, flag14);
				AsgardUtils.ShowEnableIf<ALModelGraphic.foreColorID>(this.Graphics.Cache, null, flag14, flag14);
				AsgardUtils.ShowEnableIf<ALModelExpr.backColorID>(this.Expressions.Cache, null, flag14, flag14);
				AsgardUtils.ShowEnableIf<ALModelExpr.foreColorID>(this.Expressions.Cache, null, flag14, flag14);
				AsgardUtils.ShowEnableIf<ALModelExpr.reverseDots>(this.Expressions.Cache, null, flag13, flag13);
				AsgardUtils.ShowEnableIf<ALModelExpr.hexEncoding>(this.Expressions.Cache, null, flag13, flag13);
				this.AllowDetailUpdate(flag6);
				bool flag15 = flag3;
				if (flag15)
				{
					this.Graphics.AllowInsert = false;
					this.Expressions.AllowInsert = false;
				}
				bool enableIf = this.IsEnableMerge(row);
				AsgardUtils.ShowEnableIf<ALModel.mergeDetails>(cache, row, enableIf, true);
			}
		}

		// Token: 0x06000D92 RID: 3474 RVA: 0x00023C84 File Offset: 0x00021E84
		protected virtual void _(Events.FieldUpdated<ALModel, ALModel.language> e)
		{
			ALModel row = e.Row;
			bool flag = row == null;
			if (!flag)
			{
				bool flag2 = e.NewValue as string == "PDF";
				if (flag2)
				{
					PXCache cache = e.Cache;
					cache.SetValueExt<ALModel.dealingMode>(row, false);
					cache.SetValueExt<ALModel.dealingCountExpr>(row, null);
				}
			}
		}

		// Token: 0x06000D93 RID: 3475 RVA: 0x00023CE0 File Offset: 0x00021EE0
		private bool IsEnableMerge(ALModel row)
		{
			string basedOnView = row.BasedOnView;
			string screenID = row.ScreenID;
			bool flag = string.IsNullOrEmpty(basedOnView) || string.IsNullOrEmpty(screenID);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				try
				{
					IEntityContext contextByScreenID = this._entityContextFactory.GetContextByScreenID(screenID);
					PXGraph graph = contextByScreenID.Graph;
					string text = (graph != null) ? graph.PrimaryView : null;
					return text != null && basedOnView != text;
				}
				catch (Exception ex)
				{
					PXTrace.WriteError(ex);
				}
				result = false;
			}
			return result;
		}

		// Token: 0x06000D94 RID: 3476 RVA: 0x00023D74 File Offset: 0x00021F74
		private void HandleAllowEditDetail(bool isLocked)
		{
			this.Graphics.AllowUpdate = (this.Graphics.AllowInsert = (this.Graphics.AllowDelete = !isLocked));
			this.Expressions.AllowUpdate = (this.Expressions.AllowInsert = (this.Expressions.AllowDelete = !isLocked));
			this.Printers.AllowUpdate = (this.Printers.AllowInsert = (this.Printers.AllowDelete = !isLocked));
		}

		// Token: 0x06000D95 RID: 3477 RVA: 0x00023E0C File Offset: 0x0002200C
		private void AllowDetailUpdate(bool updatable)
		{
			PXCache cache = this.Graphics.Cache;
			PXCache cache2 = this.Expressions.Cache;
			PXCache pxcache = cache;
			PXCache pxcache2 = cache;
			cache.AllowDelete = updatable;
			pxcache2.AllowInsert = updatable;
			pxcache.AllowUpdate = updatable;
			PXCache pxcache3 = cache2;
			PXCache pxcache4 = cache2;
			cache2.AllowDelete = updatable;
			pxcache4.AllowInsert = updatable;
			pxcache3.AllowUpdate = updatable;
		}

		// Token: 0x06000D96 RID: 3478 RVA: 0x00023E6C File Offset: 0x0002206C
		protected virtual void _(Events.RowPersisting<ALModel> e)
		{
			ALModel row = e.Row;
			bool flag = row == null;
			if (flag)
			{
			}
		}

		// Token: 0x06000D97 RID: 3479 RVA: 0x00023E8C File Offset: 0x0002208C
		protected virtual void _(Events.FieldUpdating<ALModelExpr, ALModelExpr.dataElementID> e)
		{
			ALModelExpr row = e.Row;
			bool flag = e.NewValue == null;
			if (!flag)
			{
				ALDataElement aldataElement = ALDataElement.PK.Find(this, e.NewValue as Guid?);
				bool flag2 = ((aldataElement != null) ? aldataElement.ExprType : null) == "P";
				if (flag2)
				{
					PXCache cache = e.Cache;
					cache.SetValueExt<ALModelExpr.posX>(row, null);
					cache.SetValueExt<ALModelExpr.posY>(row, null);
				}
			}
		}

		// Token: 0x06000D98 RID: 3480 RVA: 0x00023F00 File Offset: 0x00022100
		protected virtual void _(Events.FieldDefaulting<ALModelExpr, ALModelExpr.exprCode> e)
		{
			ALModelExpr row = e.Row;
			ALModel almodel = (ALModel)this.Model.Cache.Current;
			bool flag = row == null || row.LineNbr == null || string.IsNullOrEmpty((almodel != null) ? almodel.Name : null);
			if (!flag)
			{
				PXCache cache = e.Cache;
				e.NewValue = string.Format("{0}-{1:D3}", almodel.Name.Truncate(36), row.LineNbr);
				e.Cancel = true;
			}
		}

		// Token: 0x06000D99 RID: 3481 RVA: 0x00023F94 File Offset: 0x00022194
		protected virtual void _(Events.FieldUpdating<ALModelExpr, ALModelExpr.exprValue> e)
		{
			ALModelExpr row = e.Row;
			bool flag = row == null;
			if (!flag)
			{
				PXCache cache = e.Cache;
				object newValue = e.NewValue;
				bool flag2 = newValue != null && newValue.ToString().IsScribanExpression();
				if (flag2)
				{
					bool flag3 = row.ExprType == "H";
					if (flag3)
					{
						cache.SetValueExt<ALModelExpr.exprType>(row, "S");
					}
					e.NewValue = e.NewValue.ToString().StripDoubleBraces();
				}
			}
		}

		// Token: 0x06000D9A RID: 3482 RVA: 0x00024014 File Offset: 0x00022214
		protected virtual void _(Events.RowSelected<ALModelExpr> e)
		{
			ALModelExpr row = e.Row;
			bool flag = row == null;
			if (!flag)
			{
				PXCache cache = e.Cache;
				ALModel almodel = this.Model.Current;
				bool flag2 = almodel.Language == "EZP";
				bool flag3 = almodel.ModelType != "L";
				bool flag4 = row.ExprType == null && row.ExprCode != null;
				bool enableIf = flag3 || flag4;
				AsgardUtils.ShowEnableIf<ALModelExpr.dataElementID>(cache, row, enableIf, true);
				IFont font;
				Fonts.TryGetValue(row.FontID, out font);
				bool flag5 = font != null && font.Language != almodel.Language;
				if (flag5)
				{
					cache.RaiseExceptionHandling<ALModelExpr.fontID>(e.Row, row.FontID, new PXSetPropertyException(row, "Font does not match this model language and will be ignored", 2));
				}
			}
		}

		// Token: 0x06000D9B RID: 3483 RVA: 0x000240EC File Offset: 0x000222EC
		protected virtual void _(Events.RowSelected<ALModelGraphic> e)
		{
			ALModelGraphic row = e.Row;
			bool flag = row == null;
			if (!flag)
			{
				PXCache cache = e.Cache;
				bool flag2 = row.GraphicType == "V";
				if (flag2)
				{
					AsgardUtils.ShowEnableIf<ALModelGraphic.toX>(cache, row, false, true);
				}
				bool flag3 = row.GraphicType == "H";
				if (flag3)
				{
					AsgardUtils.ShowEnableIf<ALModelGraphic.toY>(cache, row, false, true);
				}
			}
		}

		// Token: 0x06000D9C RID: 3484 RVA: 0x00024158 File Offset: 0x00022358
		protected virtual void _(Events.FieldUpdating<ALModelGraphic, ALModelGraphic.fromX> e)
		{
			ALModelGraphic row = e.Row;
			bool flag = row == null;
			if (!flag)
			{
				bool flag2 = row.GraphicType == "V";
				if (flag2)
				{
					e.Cache.SetValueExt<ALModelGraphic.toX>(row, e.NewValue);
				}
			}
		}

		// Token: 0x06000D9D RID: 3485 RVA: 0x000241A4 File Offset: 0x000223A4
		protected virtual void _(Events.FieldUpdating<ALModelGraphic, ALModelGraphic.fromY> e)
		{
			ALModelGraphic row = e.Row;
			bool flag = row == null;
			if (!flag)
			{
				bool flag2 = row.GraphicType == "H";
				if (flag2)
				{
					e.Cache.SetValueExt<ALModelGraphic.toY>(row, e.NewValue);
				}
			}
		}

		// Token: 0x06000D9E RID: 3486 RVA: 0x000241F0 File Offset: 0x000223F0
		protected virtual void _(Events.RowSelected<ALModelPrinter> e)
		{
			ALModelPrinter row = e.Row;
			ALModel almodel = this.Model.Current;
			bool flag = row == null || almodel == null;
			if (!flag)
			{
				bool flag2 = almodel.ModelType != "P" && BasicLabelUtils.IsCompatible(almodel, row.PrinterID);
				if (flag2)
				{
					ValueTuple<IFormat, IFormat> formats = BasicLabelUtils.GetFormats(almodel, row.PrinterID);
					IFormat item = formats.Item1;
					IFormat item2 = formats.Item2;
					e.Cache.RaiseExceptionHandling<ALModelPrinter.printerID>(e.Row, row.PrinterID, new PXSetPropertyException(row, "Printer Format '{0}' is not compatible with Model Format '{1}'", 2, new object[]
					{
						(item2 != null) ? item2.Name : null,
						(item != null) ? item.Name : null
					}));
				}
			}
		}

		// Token: 0x06000D9F RID: 3487 RVA: 0x000242B8 File Offset: 0x000224B8
		public virtual bool HasExprRecords()
		{
			bool flag = this.Expressions.Current != null;
			return flag || ((this.Model.Cache.GetStatus(this.Model.Current) == 2) ? this.Expressions.Cache.IsDirty : (this.Expressions.Select(Array.Empty<object>()).Count > 0));
		}

		// Token: 0x06000DA0 RID: 3488 RVA: 0x0001ABA3 File Offset: 0x00018DA3
		[PXUIField]
		[PXButton(MenuAutoOpen = true)]
		protected virtual void action()
		{
		}

		// Token: 0x06000DA1 RID: 3489 RVA: 0x0002432C File Offset: 0x0002252C
		[PXUIField]
		[PXButton(Tooltip = "Will render this model using the Sample data and show you the result an an image")]
		protected virtual IEnumerable render(PXAdapter adapter)
		{
			this.Save.Press();
			ALModel almodel = this.Model.Current;
			bool flag = almodel != null;
			if (flag)
			{
				try
				{
					this.DoRender(almodel);
				}
				finally
				{
					this.Actions.PressCancel();
				}
			}
			return adapter.Get();
		}

		// Token: 0x06000DA2 RID: 3490 RVA: 0x00024394 File Offset: 0x00022594
		private void DoRender(ALModel model)
		{
			AcuLabelContext acuLabelContext = AcuLabelContext.CreateRenderContext(this, model, model.LabelID, null, ContentFormat.PNG);
			acuLabelContext.RenderAndSaveAsUrl(null);
		}

		// Token: 0x06000DA3 RID: 3491 RVA: 0x000243BC File Offset: 0x000225BC
		[PXUIField]
		[PXButton(Tooltip = "You can print a sample of this model")]
		protected virtual IEnumerable printAsZPL(PXAdapter adapter)
		{
			this.Save.Press();
			ALModel model = this.Model.Current;
			bool flag = model != null;
			if (flag)
			{
				ALModelMaint graph = HiddenUtils.CreateInstance<ALModelMaint>();
				PXAdapter newAdapter = new PXAdapter(adapter.View);
				PXAdapter.Copy(adapter, newAdapter);
				PXLongOperation.StartOperation(this, delegate()
				{
					graph.DoPrintAsZpl(model, newAdapter);
				});
			}
			return adapter.Get();
		}

		// Token: 0x06000DA4 RID: 3492 RVA: 0x00024450 File Offset: 0x00022650
		[PXUIField]
		[PXButton(Tooltip = "You can print a sample of the rendering of this model")]
		protected virtual IEnumerable printAsPDF(PXAdapter adapter)
		{
			this.Save.Press();
			ALModel model = this.Model.Current;
			bool flag = model != null;
			if (flag)
			{
				ALModelMaint graph = HiddenUtils.CreateInstance<ALModelMaint>();
				PXAdapter newAdapter = new PXAdapter(adapter.View);
				PXAdapter.Copy(adapter, newAdapter);
				PXLongOperation.StartOperation(this, delegate()
				{
					graph.DoPrintAsPdf(model, newAdapter);
				});
			}
			return adapter.Get();
		}

		// Token: 0x06000DA5 RID: 3493 RVA: 0x000244E4 File Offset: 0x000226E4
		[PXUIField]
		[PXButton(IsLockedOnToolbar = true, Tooltip = "Click here to open the image in new window.", CommitChanges = true)]
		protected void viewImage()
		{
			ALModel almodel = this.CurrentModel.Current;
			bool flag = ((almodel != null) ? almodel.ImageUrl : null) == null;
			if (flag)
			{
				return;
			}
			Guid value = Guid.Parse(almodel.ImageUrl.Substring(almodel.ImageUrl.LastIndexOf('=') + 1));
			throw new PXRedirectToFileException(new Guid?(value), false);
		}

		// Token: 0x06000DA6 RID: 3494 RVA: 0x00024544 File Offset: 0x00022744
		[PXButton(IsLockedOnToolbar = true, Tooltip = "Click here to open the screen related to this model")]
		[PXUIField]
		protected void viewScreen()
		{
			ALModel almodel = this.Model.Current;
			bool flag = string.IsNullOrEmpty((almodel != null) ? almodel.ScreenID : null);
			if (flag)
			{
				return;
			}
			PXSiteMapNode pxsiteMapNode = PXSiteMap.Provider.FindSiteMapNodeByScreenID(almodel.ScreenID);
			bool flag2 = !string.IsNullOrEmpty((pxsiteMapNode != null) ? pxsiteMapNode.Url : null);
			if (flag2)
			{
				throw new PXRedirectToUrlException((pxsiteMapNode != null) ? pxsiteMapNode.Url : null, 2, "ViewScreen");
			}
			throw new PXException("Cannot Site Map Node for screen '{0}'", new object[]
			{
				almodel.ScreenID
			});
		}

		// Token: 0x06000DA7 RID: 3495 RVA: 0x000245D4 File Offset: 0x000227D4
		[PXButton]
		[PXUIField]
		protected void ViewLabelChild()
		{
			ALModelChild almodelChild = this.Children.Current;
			bool flag = almodelChild == null;
			if (flag)
			{
				return;
			}
			ALModelMaint almodelMaint = HiddenUtils.CreateInstance<ALModelMaint>();
			almodelMaint.Model.Current = almodelMaint.Model.Search<ALModel.labelID>(almodelChild.LabelChildID, Array.Empty<object>());
			throw new PXRedirectRequiredException(almodelMaint, true, "ViewLabelChild");
		}

		// Token: 0x06000DA8 RID: 3496 RVA: 0x00024638 File Offset: 0x00022838
		[PXUIField]
		[PXButton]
		protected virtual IEnumerable LoadChildren(PXAdapter adapter)
		{
			this.Save.Press();
			ALModel model = this.Model.Current;
			bool flag = model != null;
			if (flag)
			{
				PXLongOperation.StartOperation(this, delegate()
				{
					ALModelMaint.DoLoadChildren(model);
				});
			}
			return adapter.Get();
		}

		// Token: 0x06000DA9 RID: 3497 RVA: 0x00024698 File Offset: 0x00022898
		[PXUIField]
		[PXButton]
		protected virtual IEnumerable deleteRenderings(PXAdapter adapter)
		{
			PXToggleAsyncDelegate pxtoggleAsyncDelegate;
			if ((pxtoggleAsyncDelegate = ALModelMaint.<>O.<0>__DoDeleteRenderings) == null)
			{
				pxtoggleAsyncDelegate = (ALModelMaint.<>O.<0>__DoDeleteRenderings = new PXToggleAsyncDelegate(BasicLabelUtils.DoDeleteRenderings));
			}
			PXLongOperation.StartOperation(this, pxtoggleAsyncDelegate);
			return adapter.Get();
		}

		// Token: 0x06000DAA RID: 3498 RVA: 0x000246D4 File Offset: 0x000228D4
		[PXUIField]
		[PXButton]
		public virtual IEnumerable clearCache(PXAdapter adapter)
		{
			Models.Reset();
			IEntityContextFactory entityContextFactory = this._entityContextFactory;
			if (entityContextFactory != null)
			{
				entityContextFactory.ClearCaches();
			}
			return adapter.Get();
		}

		// Token: 0x06000DAB RID: 3499 RVA: 0x00024704 File Offset: 0x00022904
		[PXUIField]
		[PXButton]
		public virtual IEnumerable generateSource(PXAdapter adapter)
		{
			ALModel almodel = this.Model.Current;
			string text = (almodel != null) ? almodel.ScreenID : null;
			bool flag = string.IsNullOrEmpty(text);
			if (flag)
			{
				return adapter.Get();
			}
			ALDataSourceMaint aldataSourceMaint = HiddenUtils.CreateInstance<ALDataSourceMaint>();
			ALDataSource orAdd = aldataSourceMaint.GetOrAdd(text);
			throw new PXRedirectRequiredException(aldataSourceMaint, true, "ALDataSource");
		}

		// Token: 0x06000DAC RID: 3500 RVA: 0x0002475C File Offset: 0x0002295C
		[PXUIField(DisplayName = "Add Data Elements")]
		[PXButton]
		public virtual IEnumerable loadDataElements(PXAdapter adapter)
		{
			bool flag = this.DataElementFilter.View.AskExt() == 1;
			if (flag)
			{
				List<ALDataElement> list = (from x in GraphHelper.RowCast<ALDataElement>(this.DataElementFilter.Select(Array.Empty<object>()))
				where x.Selected.GetValueOrDefault()
				select x).ToList<ALDataElement>();
				foreach (ALDataElement aldataElement in list)
				{
					this.Expressions.Insert(new ALModelExpr
					{
						DataElementID = aldataElement.RecordID
					});
				}
			}
			return adapter.Get();
		}

		// Token: 0x06000DAD RID: 3501 RVA: 0x00024830 File Offset: 0x00022A30
		[PXUIField]
		[PXButton]
		protected virtual IEnumerable loadLabelZoom(PXAdapter adapter)
		{
			LabelZoomProcessor labelZoomProcessor = new LabelZoomProcessor();
			labelZoomProcessor.DoLoadLabelZoomHeaders();
			return adapter.Get();
		}

		// Token: 0x06000DAE RID: 3502 RVA: 0x00024858 File Offset: 0x00022A58
		[PXUIField]
		[PXButton]
		protected virtual IEnumerable loadLabelZoomDetails(PXAdapter adapter)
		{
			this.Save.Press();
			ALModel almodel = this.Model.Current;
			bool flag = almodel != null && almodel.ModelType == "L" && !string.IsNullOrEmpty(almodel.CloudID);
			if (flag)
			{
				LabelZoomProcessor labelZoomProcessor = new LabelZoomProcessor();
				DBOperationResults dboperationResults = labelZoomProcessor.DoLoadLabelZoomDetails(almodel);
				this.Expressions.View.RequestRefresh();
				this.Graphics.View.RequestRefresh();
				this.Actions.PressCancel();
				throw new PXOperationCompletedException(dboperationResults.ToString());
			}
			return adapter.Get();
		}

		// Token: 0x06000DAF RID: 3503 RVA: 0x00024900 File Offset: 0x00022B00
		[PXUIField]
		[PXButton]
		protected virtual IEnumerable clearLabelZoomDetails(PXAdapter adapter)
		{
			this.Save.Press();
			ALModel almodel = this.Model.Current;
			bool flag = almodel != null && almodel.ModelType == "L" && !string.IsNullOrEmpty(almodel.CloudID);
			if (flag)
			{
				LabelZoomProcessor labelZoomProcessor = new LabelZoomProcessor();
				DBOperationResults dboperationResults = labelZoomProcessor.DoClearDetails(almodel);
				this.Expressions.View.RequestRefresh();
				this.Graphics.View.RequestRefresh();
				throw new PXOperationCompletedException(dboperationResults.ToString());
			}
			return adapter.Get();
		}

		// Token: 0x06000DB0 RID: 3504 RVA: 0x0002499C File Offset: 0x00022B9C
		private void DoPrintAsZpl(ALModel model, PXAdapter adapter)
		{
			this.Model.Current = model;
			AcuLabelContext labelContext = AcuLabelContext.CreatePrintContext(base.GetType(), model, model.LabelID, false, adapter);
			AcuLabelGenerator acuLabelGenerator = new AcuLabelGenerator();
			acuLabelGenerator.PrintLabels(labelContext);
			this.Cancel.Press();
		}

		// Token: 0x06000DB1 RID: 3505 RVA: 0x000249E8 File Offset: 0x00022BE8
		private void DoPrintAsPdf(ALModel model, PXAdapter adapter)
		{
			this.Model.Current = model;
			bool flag = !BasicLabelUtils.HasFile(model, ContentFormat.PNG);
			if (flag)
			{
				this.DoRender(model);
			}
			FileInfo file = BasicLabelUtils.GetFile(model, ContentFormat.PNG);
			bool flag2 = file == null;
			if (flag2)
			{
				throw new PXException("Can't find a Rendering file to print");
			}
			AcuLabelContext acuLabelContext = AcuLabelContext.CreatePrintContext(base.GetType(), model, model.LabelID, false, adapter);
			FileResult printResult = PngToPdf.INSTANCE.Transform(acuLabelContext, file.BinData);
			acuLabelContext.Print(printResult);
			this.Cancel.Press();
		}

		// Token: 0x06000DB2 RID: 3506 RVA: 0x00024A78 File Offset: 0x00022C78
		private static void DoLoadChildren(ALModel label)
		{
			ALModelMaint almodelMaint = HiddenUtils.CreateInstance<ALModelMaint>();
			almodelMaint.Model.Current = label;
			PXResultset<ALModel> pxresultset = almodelMaint.PossibleChildren.Select(new object[]
			{
				label.ScreenID
			});
			foreach (PXResult<ALModel> pxresult in pxresultset)
			{
				ALModel almodel = pxresult;
				ALModelChild almodelChild = almodelMaint.FindChild.Select(new object[]
				{
					almodel.LabelID
				});
				bool flag = almodelChild == null;
				if (flag)
				{
					ALModelChild almodelChild2 = (ALModelChild)almodelMaint.Children.Cache.CreateInstance();
					almodelChild2.LabelID = label.LabelID;
					almodelChild2.LabelChildID = almodel.LabelID;
					almodelChild2.Active = new bool?(true);
					ALModelChild almodelChild3 = almodelMaint.Children.Insert(almodelChild2);
				}
			}
			almodelMaint.Actions.PressSave();
			almodelMaint.Children.View.RequestRefresh();
		}

		// Token: 0x06000DB3 RID: 3507 RVA: 0x00024B98 File Offset: 0x00022D98
		public virtual bool PrepareImportRow(string viewName, IDictionary keys, IDictionary values)
		{
			return true;
		}

		// Token: 0x06000DB4 RID: 3508 RVA: 0x0001ABA3 File Offset: 0x00018DA3
		public virtual void ImportDone(PXImportAttribute.ImportMode.Value mode)
		{
		}

		// Token: 0x06000DB5 RID: 3509 RVA: 0x00024BAC File Offset: 0x00022DAC
		public bool RowImported(string viewName, object row, object oldRow)
		{
			return oldRow == null;
		}

		// Token: 0x06000DB6 RID: 3510 RVA: 0x00024BC4 File Offset: 0x00022DC4
		public bool RowImporting(string viewName, object row)
		{
			return row == null;
		}

		// Token: 0x06000DB7 RID: 3511 RVA: 0x0001ABA3 File Offset: 0x00018DA3
		public void PrepareItems(string viewName, IEnumerable items)
		{
		}

		// Token: 0x040005F5 RID: 1525
		[ALImportExportAll]
		[ALImportLabelZoom]
		[PXViewName("Label Models")]
		[PXCopyPasteHiddenFields(new Type[]
		{
			typeof(ALModel.labelID),
			typeof(ALModel.imageUrl),
			typeof(ALModel.rendered),
			typeof(ALModel.message)
		})]
		public PXSelect<ALModel> Model;

		// Token: 0x040005F6 RID: 1526
		public PXSelect<ALModel, Where<ALModel.labelID, Equal<Current<ALModel.labelID>>>> CurrentModel;

		// Token: 0x040005F7 RID: 1527
		[PXCopyPasteHiddenView]
		public PXSetup<ALSetup> LabelSetup;

		// Token: 0x040005F8 RID: 1528
		[PXImport(typeof(ALModel))]
		[PXViewName("Model Printers")]
		public PXOrderedSelect<ALModel, ALModelPrinter, Where<ALModelPrinter.labelID, Equal<Current<ALModel.labelID>>>, OrderBy<Asc<ALModelPrinter.sortOrder, Asc<ALModelPrinter.lineNbr>>>> Printers;

		// Token: 0x040005F9 RID: 1529
		public PXSelectReadonly<ALModelPrinter, Where<ALModelPrinter.labelID, Equal<Current<ALModel.labelID>>>, OrderBy<Asc<ALModelPrinter.sortOrder, Asc<ALModelPrinter.lineNbr>>>> PrintersReadOnly;

		// Token: 0x040005FA RID: 1530
		[PXImport(typeof(ALModel))]
		[PXViewName("Model Expressions")]
		public PXOrderedSelect<ALModel, ALModelExpr, Where<ALModelExpr.labelID, Equal<Current<ALModel.labelID>>>, OrderBy<Asc<ALModelExpr.sortOrder, Asc<ALModelExpr.lineNbr>>>> Expressions;

		// Token: 0x040005FB RID: 1531
		[PXImport(typeof(ALModel))]
		[PXViewName("Model Children")]
		public PXOrderedSelect<ALModel, ALModelChild, LeftJoin<ALModel, On<ALModel.labelID, Equal<ALModelChild.labelChildID>>>, Where<ALModelChild.labelID, Equal<Current<ALModel.labelID>>>, OrderBy<Asc<ALModelChild.sortOrder, Asc<ALModelChild.lineNbr>>>> Children;

		// Token: 0x040005FC RID: 1532
		[PXImport(typeof(ALModel))]
		[PXViewName("Model Graphics")]
		public PXOrderedSelect<ALModel, ALModelGraphic, Where<ALModelGraphic.modelID, Equal<Current<ALModel.labelID>>>, OrderBy<Asc<ALModelGraphic.sortOrder, Asc<ALModelGraphic.lineNbr>>>> Graphics;

		// Token: 0x040005FD RID: 1533
		[PXCopyPasteHiddenView]
		public PXSelect<ALPrintLog, Where<ALPrintLog.modelID, Equal<Current<ALModel.labelID>>>, OrderBy<Desc<ALPrintLog.createdDateTime>>> PrintLog;

		// Token: 0x040005FE RID: 1534
		[PXCopyPasteHiddenView]
		public PXSelect<ALModel, Where<ALModel.screenID, Equal<Required<ALModel.screenID>>, And<ALModel.modelType, Equal<ALModelType.single>, And<ALModel.active, Equal<True>>>>> PossibleChildren;

		// Token: 0x040005FF RID: 1535
		[PXCopyPasteHiddenView]
		public PXSelect<ALModelChild, Where<ALModelChild.labelID, Equal<Current<ALModel.labelID>>, And<ALModelChild.labelChildID, Equal<Required<ALModelChild.labelChildID>>>>> FindChild;

		// Token: 0x04000600 RID: 1536
		[PXCopyPasteHiddenView]
		public PXFilter<ALDataElementFilter> DataElementFilter;

		// Token: 0x04000601 RID: 1537
		[Nullable(new byte[]
		{
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			1,
			0,
			0,
			1,
			1,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			1,
			0,
			0,
			1,
			1,
			0,
			1,
			1,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			1,
			0,
			0,
			1,
			1,
			0,
			1,
			1,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			1,
			0,
			0,
			1,
			1,
			0,
			1,
			1,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			1,
			0,
			0,
			0,
			1,
			1,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			1,
			0,
			0,
			1,
			1,
			0,
			1,
			1,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			1,
			0,
			0,
			1,
			1,
			0,
			1,
			1,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			1,
			0,
			0,
			1,
			1,
			0,
			1,
			1,
			0,
			1,
			1,
			0,
			1,
			1,
			0
		})]
		[PXCopyPasteHiddenView]
		public FbqlSelect<SelectFromBase<ALDataElement, TypeArrayOf<IFbqlJoin>.Append<TypeArrayOf<IFbqlJoin>.Empty, FbqlJoins.Inner<ALDataSource>.On<ALDataElement.FK.Parent>>>.Where<BqlChainableConditionMirror<TypeArrayOf<IBqlBinary>.Append<TypeArrayOf<IBqlBinary>.Append<TypeArrayOf<IBqlBinary>.Append<TypeArrayOf<IBqlBinary>.Append<TypeArrayOf<IBqlBinary>.Append<TypeArrayOf<IBqlBinary>.Append<TypeArrayOf<IBqlBinary>.Append<TypeArrayOf<IBqlBinary>.FilledWith<And<Compare<ALDataSource.screenID, Equal<BqlField<ALModel.screenID, IBqlString>.FromCurrent>>>>, And<BqlChainableConditionBase<TypeArrayOf<IBqlBinary>.FilledWith<And<Compare<Current<ALDataElementFilter.exprType>, IsNull>>>>.Or<BqlOperand<ALDataElement.exprType, IBqlString>.IsEqual<BqlField<ALDataElementFilter.exprType, IBqlString>.FromCurrent>>>>, And<BqlChainableConditionBase<TypeArrayOf<IBqlBinary>.FilledWith<And<Compare<Current<ALDataElementFilter.basedOn>, IsNull>>>>.Or<BqlOperand<ALDataElement.basedOn, IBqlString>.Contains<BqlField<ALDataElementFilter.basedOn, IBqlString>.FromCurrent>>>>, And<BqlChainableConditionBase<TypeArrayOf<IBqlBinary>.FilledWith<And<Compare<Current<ALDataElementFilter.exprValue>, IsNull>>>>.Or<BqlOperand<ALDataElement.exprValue, IBqlString>.Contains<BqlField<ALDataElementFilter.exprValue, IBqlString>.FromCurrent>>>>, And<BqlChainableConditionBase<TypeArrayOf<IBqlBinary>.FilledWith<And<Compare<Current<ALDataElementFilter.withBarcode>, Equal<False>>>>>.Or<BqlOperand<ALDataElement.barcodeID, IBqlGuid>.IsNotNull>>>, And<BqlChainableConditionBase<TypeArrayOf<IBqlBinary>.FilledWith<And<Compare<Current<ALDataElementFilter.categoryID>, IsNull>>>>.Or<BqlOperand<ALDataElement.categoryID, IBqlGuid>.IsEqual<BqlField<ALDataElementFilter.categoryID, IBqlGuid>.FromCurrent>>>>, And<BqlChainableConditionBase<TypeArrayOf<IBqlBinary>.FilledWith<And<Compare<Current<ALDataElementFilter.contentID>, IsNull>>>>.Or<BqlOperand<ALDataElement.contentID, IBqlGuid>.IsEqual<BqlField<ALDataElementFilter.contentID, IBqlGuid>.FromCurrent>>>>, And<BqlChainableConditionBase<TypeArrayOf<IBqlBinary>.FilledWith<And<Compare<Current<ALDataElementFilter.substitutionID>, IsNull>>>>.Or<BqlOperand<ALDataElement.substitutionID, IBqlGuid>.IsEqual<BqlField<ALDataElementFilter.substitutionID, IBqlGuid>.FromCurrent>>>>>.And<BqlOperand<ALDataSource.screenID, IBqlString>.IsEqual<BqlField<ALModel.screenID, IBqlString>.FromCurrent>>>, ALDataElement>.View SelectedDataElements;

		// Token: 0x04000602 RID: 1538
		[PXCopyPasteHiddenView]
		public PXSelect<ALDataElement, Where<ALDataElement.snippetID, Equal<Current<ALModel.labelID>>>, OrderBy<Asc<ALDataElement.name>>> UsedByDataElements;

		// Token: 0x04000607 RID: 1543
		public ALChangeID<ALModel, ALModel.name> ChangeID;

		// Token: 0x04000608 RID: 1544
		public PXAction<ALModel> Action;

		// Token: 0x04000609 RID: 1545
		public PXAction<ALModel> Render;

		// Token: 0x0400060A RID: 1546
		public PXAction<ALModel> PrintAsZPL;

		// Token: 0x0400060B RID: 1547
		public PXAction<ALModel> PrintAsPDF;

		// Token: 0x0400060C RID: 1548
		public PXAction<ALModel> ViewImage;

		// Token: 0x0400060D RID: 1549
		public PXAction<ALModel> ViewScreen;

		// Token: 0x0400060E RID: 1550
		public PXAction<ALModel> viewLabelChild;

		// Token: 0x0400060F RID: 1551
		public PXAction<ALModel> loadChildren;

		// Token: 0x04000610 RID: 1552
		public PXAction<ALModel> DeleteRenderings;

		// Token: 0x04000611 RID: 1553
		public PXAction<ALModel> ClearCache;

		// Token: 0x04000612 RID: 1554
		public PXAction<ALModel> GenerateSource;

		// Token: 0x04000613 RID: 1555
		public PXAction<ALModel> LoadDataElements;

		// Token: 0x04000614 RID: 1556
		public PXAction<ALModel> LoadLabelZoom;

		// Token: 0x04000615 RID: 1557
		public PXAction<ALModel> LoadLabelZoomDetails;

		// Token: 0x04000616 RID: 1558
		public PXAction<ALModel> ClearLabelZoomDetails;

		// Token: 0x02000788 RID: 1928
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x04000BF1 RID: 3057
			public static PXToggleAsyncDelegate <0>__DoDeleteRenderings;
		}
	}
}
