using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using AA.Objects.AL.Integration;
using AA.Objects.AL.LabelZoom;
using AA.Objects.AL.License;
using PX.Api;
using PX.BusinessProcess.DAC;
using PX.BusinessProcess.UI;
using PX.Common;
using PX.Data;
using PX.Data.BQL;
using PX.Data.BQL.Fluent;
using PX.Data.DependencyInjection;
using PX.Data.Maintenance.GI;
using PX.Objects.Common;
using PX.SM;
using Scriban;

namespace AA.Objects.AL
{
	// Token: 0x020001C7 RID: 455
	public class ALModelMaint : PXGraph<ALModelMaint, ALModel>, IGraphWithInitialization, PXImportAttribute.IPXPrepareItems, PXImportAttribute.IPXProcess
	{
		// Token: 0x17000629 RID: 1577
		// (get) Token: 0x0600117B RID: 4475 RVA: 0x00038196 File Offset: 0x00036396
		// (set) Token: 0x0600117C RID: 4476 RVA: 0x0003819E File Offset: 0x0003639E
		public MultiDuplicatesSearchEngine<ALModelExpr> DuplicateFinder { get; set; }

		// Token: 0x1700062A RID: 1578
		// (get) Token: 0x0600117D RID: 4477 RVA: 0x000381A7 File Offset: 0x000363A7
		// (set) Token: 0x0600117E RID: 4478 RVA: 0x000381AF File Offset: 0x000363AF
		[InjectDependency]
		private IALLicenseManagerFactory LicenseManagerFactory { get; set; }

		// Token: 0x1700062B RID: 1579
		// (get) Token: 0x0600117F RID: 4479 RVA: 0x000381B8 File Offset: 0x000363B8
		internal IALLicenseManager LicenseManager
		{
			get
			{
				return this.LicenseManagerFactory.GetLicenseManager(ALConstants.ProductCode);
			}
		}

		// Token: 0x1700062C RID: 1580
		// (get) Token: 0x06001180 RID: 4480 RVA: 0x000381CA File Offset: 0x000363CA
		// (set) Token: 0x06001181 RID: 4481 RVA: 0x000381D2 File Offset: 0x000363D2
		[InjectDependency]
		private ILabelGenerator _labelGenerator { get; set; }

		// Token: 0x1700062D RID: 1581
		// (get) Token: 0x06001182 RID: 4482 RVA: 0x000381DB File Offset: 0x000363DB
		// (set) Token: 0x06001183 RID: 4483 RVA: 0x000381E3 File Offset: 0x000363E3
		[InjectDependency]
		private IEntityContextFactory _entityContextFactory { get; set; }

		// Token: 0x06001184 RID: 4484 RVA: 0x000381EC File Offset: 0x000363EC
		public void Initialize()
		{
			this.Action.AddMenuAction(this.DeleteRenderings);
			this.Action.AddMenuAction(this.ViewImage);
			this.Action.AddMenuAction(this.ChangeID);
			this.Action.AddMenuAction(this.GenComponents);
			this.Action.AddMenuAction(this.ClearCache);
			this.Action.AddMenuAction(this.GenerateSource);
			this.Action.AddMenuAction(this.LoadLabelZoom);
			this.PrintAsPDF.IsMass = true;
			this.PrintAsZPL.IsMass = true;
			this.Render.IsMass = true;
		}

		// Token: 0x06001185 RID: 4485 RVA: 0x00019FF9 File Offset: 0x000181F9
		[PXMergeAttributes(Method = 0)]
		[PXCustomizeBaseAttribute(typeof(PXUIFieldAttribute), "IsReadOnly", true)]
		protected virtual void _(Events.CacheAttached<ALPrintLog.recordID> e)
		{
		}

		// Token: 0x06001186 RID: 4486 RVA: 0x000382A0 File Offset: 0x000364A0
		protected virtual void _(Events.RowSelected<ALModel> e)
		{
			ALModel row = e.Row;
			bool flag = row == null || this.IsCopyPasteContext;
			if (!flag)
			{
				PXCache cache = e.Cache;
				bool flag2 = row.ModelType == "G";
				bool flag3 = row.ModelType == "P";
				bool flag4 = true;
				bool flag5 = !flag2 && !flag3 && flag4;
				bool devMode = ALSetupSlot.DevMode;
				bool valueOrDefault = row.IsSystem.GetValueOrDefault();
				bool isImport = this.IsImport;
				bool flag6 = !valueOrDefault || devMode || isImport;
				bool enabled = !string.IsNullOrEmpty(row.ScreenID);
				bool flag7 = valueOrDefault && !devMode && !this.IsImport;
				this.HandleAllowEditDetail(flag7);
				AsgardUtils.ShowEnableIf<ALModel.language>(cache, row, !flag4, true);
				AsgardUtils.ShowEnableIf<ALModel.body>(cache, row, !flag7, true);
				this.Render.SetEnabled(flag4);
				this.GenerateSource.SetEnabled(flag5);
				this.ViewImage.SetEnabled(flag4);
				this.PrintAsZPL.SetEnabled(flag4);
				this.PrintAsPDF.SetEnabled(flag4);
				this.ViewScreen.SetEnabled(enabled);
				this.ViewImage.SetEnabled(flag5);
				this.ChangeID.SetEnabled(flag6);
				bool enabled2 = ALSetupSlot.LabelZoomAPIKey != null;
				this.LoadLabelZoom.SetEnabled(enabled2);
				AsgardUtils.EnableOrHide(this.FindDataElements, flag6);
				AsgardUtils.EnableOrHide(this.MoveDown, flag6);
				AsgardUtils.EnableOrHide(this.MoveUp, flag6);
				AsgardUtils.EnableOrHide(this.MoveLeft, flag6);
				AsgardUtils.EnableOrHide(this.MoveRight, flag6);
				AsgardUtils.EnableOrHide(this.LoadDataElements, flag6 && flag5);
				PXUIFieldAttribute.SetVisible<ALModel.moveBy>(cache, row, flag6);
				PXUIFieldAttribute.SetVisible<ALModel.sizeUnit>(cache, row, flag6);
				bool enabled3 = !string.IsNullOrEmpty(row.TriggerField) && !string.IsNullOrEmpty(row.TriggerValue) && ALSetupSlot.EnableAutomation;
				this.GenComponents.SetEnabled(enabled3);
				this.GenComponents.SetVisible(flag5 && ALSetupSlot.EnableAutomation);
				Tuple<string, string>[] fields = BasicLabelUtils.GetFields(row);
				PXStringListAttribute.SetList<ALModel.triggerField>(cache, row, fields);
				Tuple<string, string>[] fieldValues = BasicLabelUtils.GetFieldValues(row);
				IFormat item = BasicLabelUtils.GetFormats(row, null).Item1;
				PXStringListAttribute.SetList<ALModel.triggerValue>(cache, row, fieldValues);
				bool flag8 = row.Language == "EZP" && ALRotation.HasRotation(item.Rotation);
				if (flag8)
				{
					cache.RaiseExceptionHandling<ALModel.formatID>(e.Row, item.FormatID, new PXSetPropertyException(row, "Rotation with colors is not supported", 2));
				}
				this.AllowDetailUpdate(flag6);
			}
		}

		// Token: 0x06001187 RID: 4487 RVA: 0x00038550 File Offset: 0x00036750
		private void HandleAllowEditDetail(bool isLocked)
		{
			this.Graphics.AllowUpdate = (this.Graphics.AllowInsert = (this.Graphics.AllowDelete = !isLocked));
			this.Expressions.AllowUpdate = (this.Expressions.AllowInsert = (this.Expressions.AllowDelete = !isLocked));
			this.Printers.AllowUpdate = (this.Printers.AllowInsert = (this.Printers.AllowDelete = !isLocked));
		}

		// Token: 0x06001188 RID: 4488 RVA: 0x000385E8 File Offset: 0x000367E8
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

		// Token: 0x06001189 RID: 4489 RVA: 0x00038648 File Offset: 0x00036848
		protected virtual void _(Events.FieldUpdated<ALModel, ALModel.layoutType> e)
		{
			bool flag = e.Row != null && e.NewValue != null;
			if (flag)
			{
				this.RecalulatePositions(e.Row, e.NewValue.ToString());
			}
		}

		// Token: 0x0600118A RID: 4490 RVA: 0x00038688 File Offset: 0x00036888
		protected virtual void _(Events.FieldUpdated<ALModel, ALModel.sizeUnit> e)
		{
			e.Cache.SetDefaultExt<ALModel.moveBy>(e.Row);
		}

		// Token: 0x0600118B RID: 4491 RVA: 0x000386A0 File Offset: 0x000368A0
		protected virtual void _(Events.FieldDefaulting<ALModel, ALModel.moveBy> e)
		{
			ALModel row = e.Row;
			bool flag = row == null || row.SizeUnit == null;
			if (!flag)
			{
				string sizeUnit = row.SizeUnit;
				string a = sizeUnit;
				if (!(a == "MM"))
				{
					if (!(a == "CM"))
					{
						if (!(a == "IN"))
						{
							if (a == "DT")
							{
								e.NewValue = 8.0m;
							}
						}
						else
						{
							e.NewValue = 0.125m;
						}
					}
					else
					{
						e.NewValue = 0.1m;
					}
				}
				else
				{
					e.NewValue = 1.0m;
				}
				e.Cancel = (e.NewValue != null);
			}
		}

		// Token: 0x0600118C RID: 4492 RVA: 0x00038780 File Offset: 0x00036980
		protected virtual void _(Events.RowPersisting<ALModel> e)
		{
			ALModel row = e.Row;
			bool flag = row == null;
			if (flag)
			{
			}
		}

		// Token: 0x0600118D RID: 4493 RVA: 0x000387A0 File Offset: 0x000369A0
		protected virtual void _(Events.RowSelected<ALModelExpr> e)
		{
			ALModelExpr row = e.Row;
			bool flag = row == null || this.IsCopyPasteContext;
			if (!flag)
			{
				PXCache cache = e.Cache;
				bool flag2 = row.SampleType == "M";
				AsgardUtils.ShowEnableIf<ALModelExpr.sampleValue>(cache, row, !flag2, true);
			}
		}

		// Token: 0x0600118E RID: 4494 RVA: 0x000387F0 File Offset: 0x000369F0
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
					cache.SetValueExt<ALModelExpr.toX>(row, null);
				}
			}
		}

		// Token: 0x0600118F RID: 4495 RVA: 0x00038870 File Offset: 0x00036A70
		protected virtual void _(Events.FieldUpdating<ALModelExpr, ALModelExpr.justification> e)
		{
			ALModelExpr row = e.Row;
			bool flag;
			if (row != null && e.NewValue != null)
			{
				object newValue = e.NewValue;
				flag = (((newValue != null) ? newValue.ToString() : null) != "N");
			}
			else
			{
				flag = false;
			}
			bool flag2 = flag;
			if (!flag2)
			{
				PXCache cache = e.Cache;
				cache.SetValueExt<ALModelExpr.justMaxLines>(row, null);
				cache.SetValueExt<ALModelExpr.toX>(row, null);
			}
		}

		// Token: 0x06001190 RID: 4496 RVA: 0x000388D0 File Offset: 0x00036AD0
		protected virtual void _(Events.FieldUpdating<ALModelExpr, ALModelExpr.exprType> e)
		{
			ALModelExpr row = e.Row;
			bool flag = row == null;
			if (!flag)
			{
				PXCache cache = e.Cache;
				object newValue = e.NewValue;
				string text = (newValue != null) ? newValue.ToString() : null;
				object oldValue = e.OldValue;
				string a = (oldValue != null) ? oldValue.ToString() : null;
				bool flag2 = text != null;
				if (flag2)
				{
					bool flag3 = text != "I" && a == "I";
					if (flag3)
					{
						cache.SetValueExt<ALModelExpr.doSubstitute>(row, false);
						cache.SetValueExt<ALModelExpr.substitutionID>(row, null);
					}
					bool flag4 = (text == "I" || text == "H" || text == "E" || text == "C") && row.SampleType != "M";
					if (flag4)
					{
						cache.SetValueExt<ALModelExpr.sampleType>(row, "M");
					}
					bool flag5 = text == "F" && text != row.SampleType;
					if (flag5)
					{
						cache.SetValueExt<ALModelExpr.sampleType>(row, text);
					}
					bool flag6 = text == "C";
					if (flag6)
					{
						cache.SetValueExt<ALModelExpr.sampleValue>(row, null);
					}
				}
			}
		}

		// Token: 0x06001191 RID: 4497 RVA: 0x00038A10 File Offset: 0x00036C10
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
					e.NewValue = e.NewValue.ToString().StripBraces();
				}
				bool flag4 = e.NewValue != null && row.SampleType == "M";
				if (flag4)
				{
					cache.SetValueExt<ALModelExpr.sampleValue>(row, null);
				}
			}
		}

		// Token: 0x06001192 RID: 4498 RVA: 0x00038AC0 File Offset: 0x00036CC0
		protected virtual void _(Events.FieldUpdating<ALModelExpr, ALModelExpr.sampleType> e)
		{
			ALModelExpr row = e.Row;
			bool flag = row == null;
			if (!flag)
			{
				PXCache cache = e.Cache;
				object newValue = e.NewValue;
				string a = (newValue != null) ? newValue.ToString() : null;
				bool flag2 = a == "M";
				if (flag2)
				{
					cache.SetValueExt<ALModelExpr.sampleValue>(row, null);
				}
			}
		}

		// Token: 0x06001193 RID: 4499 RVA: 0x00038B18 File Offset: 0x00036D18
		protected virtual void _(Events.FieldUpdated<ALModelExpr, ALModelExpr.substitutionID> e)
		{
			ALModelExpr row = e.Row;
			bool flag = row == null;
			if (!flag)
			{
				bool flag2 = e.NewValue != null;
				if (flag2)
				{
					object newValue = e.NewValue;
					bool flag3;
					if (newValue is Guid)
					{
						Guid guid = (Guid)newValue;
						flag3 = !row.DoSubstitute.GetValueOrDefault();
					}
					else
					{
						flag3 = false;
					}
					bool flag4 = flag3;
					if (flag4)
					{
						e.Cache.SetValueExt<ALModelExpr.doSubstitute>(row, true);
					}
				}
				else
				{
					bool valueOrDefault = row.DoSubstitute.GetValueOrDefault();
					if (valueOrDefault)
					{
						e.Cache.SetValueExt<ALModelExpr.doSubstitute>(row, false);
					}
				}
			}
		}

		// Token: 0x06001194 RID: 4500 RVA: 0x00038BC0 File Offset: 0x00036DC0
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

		// Token: 0x06001195 RID: 4501 RVA: 0x00038C2C File Offset: 0x00036E2C
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

		// Token: 0x06001196 RID: 4502 RVA: 0x00038C78 File Offset: 0x00036E78
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

		// Token: 0x06001197 RID: 4503 RVA: 0x00038CC4 File Offset: 0x00036EC4
		protected virtual void _(Events.RowSelected<ALModelPrinter> e)
		{
			ALModelPrinter row = e.Row;
			ALModel almodel = this.Model.Current;
			bool flag = row == null || almodel == null;
			if (!flag)
			{
				bool flag2 = !BasicLabelUtils.IsCompatible(almodel, row.PrinterID);
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

		// Token: 0x06001198 RID: 4504 RVA: 0x00038D7C File Offset: 0x00036F7C
		public virtual bool HasExprRecords()
		{
			bool flag = this.Expressions.Current != null;
			bool result;
			if (flag)
			{
				result = true;
			}
			else
			{
				bool flag2 = this.Model.Cache.GetStatus(this.Model.Current) == 2;
				if (flag2)
				{
					result = this.Expressions.Cache.IsDirty;
				}
				else
				{
					result = (this.Expressions.Select(Array.Empty<object>()).Count > 0);
				}
			}
			return result;
		}

		// Token: 0x06001199 RID: 4505 RVA: 0x00019FF9 File Offset: 0x000181F9
		[PXUIField]
		[PXButton(MenuAutoOpen = true)]
		protected virtual void action()
		{
		}

		// Token: 0x0600119A RID: 4506 RVA: 0x00038DF4 File Offset: 0x00036FF4
		[PXUIField]
		[PXButton(Tooltip = "Will render this model using the Sample data and show you the result an an image")]
		protected virtual IEnumerable render(PXAdapter adapter)
		{
			this.Save.Press();
			ALModel almodel = this.Model.Current;
			bool flag = almodel != null;
			if (flag)
			{
				this.DoRender(almodel);
			}
			return adapter.Get();
		}

		// Token: 0x0600119B RID: 4507 RVA: 0x00038E38 File Offset: 0x00037038
		private void DoRender(ALModel model)
		{
			LabelContext labelContext = LabelContext.CreateRenderContext(this, model, model.LabelID, null, OutputFormat.PNG);
			labelContext.RenderAndSaveAsUrl(null);
			this.Cancel.Press();
		}

		// Token: 0x0600119C RID: 4508 RVA: 0x00038E6C File Offset: 0x0003706C
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

		// Token: 0x0600119D RID: 4509 RVA: 0x00038F00 File Offset: 0x00037100
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

		// Token: 0x0600119E RID: 4510 RVA: 0x00038F94 File Offset: 0x00037194
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

		// Token: 0x0600119F RID: 4511 RVA: 0x00038FF0 File Offset: 0x000371F0
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

		// Token: 0x060011A0 RID: 4512 RVA: 0x00039080 File Offset: 0x00037280
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

		// Token: 0x060011A1 RID: 4513 RVA: 0x000390E4 File Offset: 0x000372E4
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

		// Token: 0x060011A2 RID: 4514 RVA: 0x00039144 File Offset: 0x00037344
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

		// Token: 0x060011A3 RID: 4515 RVA: 0x00039180 File Offset: 0x00037380
		[PXButton(ImageKey = "ArrowUp")]
		[PXUIField(DisplayName = "")]
		public virtual IEnumerable moveUp(PXAdapter adapter)
		{
			this.AdjustPosition(this.Model.Current, this.Expressions.Current, ALModelMaint.Direction.Up);
			return adapter.Get();
		}

		// Token: 0x060011A4 RID: 4516 RVA: 0x000391B8 File Offset: 0x000373B8
		[PXButton(ImageKey = "ArrowDown")]
		[PXUIField(DisplayName = "")]
		public virtual IEnumerable moveDown(PXAdapter adapter)
		{
			this.AdjustPosition(this.Model.Current, this.Expressions.Current, ALModelMaint.Direction.Down);
			return adapter.Get();
		}

		// Token: 0x060011A5 RID: 4517 RVA: 0x000391F0 File Offset: 0x000373F0
		[PXButton(ImageKey = "ArrowLeft")]
		[PXUIField(DisplayName = "")]
		public virtual IEnumerable moveLeft(PXAdapter adapter)
		{
			this.AdjustPosition(this.Model.Current, this.Expressions.Current, ALModelMaint.Direction.Left);
			return adapter.Get();
		}

		// Token: 0x060011A6 RID: 4518 RVA: 0x00039228 File Offset: 0x00037428
		[PXButton(ImageKey = "ArrowRight")]
		[PXUIField(DisplayName = "")]
		public virtual IEnumerable moveRight(PXAdapter adapter)
		{
			this.AdjustPosition(this.Model.Current, this.Expressions.Current, ALModelMaint.Direction.Right);
			return adapter.Get();
		}

		// Token: 0x060011A7 RID: 4519 RVA: 0x00039260 File Offset: 0x00037460
		[PXUIField(DisplayName = "Generate B. Event Components")]
		[PXButton]
		public virtual IEnumerable genComponents(PXAdapter adapter)
		{
			bool flag = this.Model.Current != null;
			if (flag)
			{
				this.Save.Press();
				ALModel model = this.Model.Current;
				ALBPEventHelper eventHelper = HiddenUtils.CreateInstance<ALBPEventHelper>();
				PXLongOperation.StartOperation(this, delegate()
				{
					eventHelper.GenerateTriggerComponents(model);
				});
			}
			return adapter.Get();
		}

		// Token: 0x060011A8 RID: 4520 RVA: 0x000392D0 File Offset: 0x000374D0
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

		// Token: 0x060011A9 RID: 4521 RVA: 0x00039300 File Offset: 0x00037500
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
			ALDataSource aldataSource = aldataSourceMaint.Document.Search<ALDataSource.screenID>(text, Array.Empty<object>());
			bool flag2 = aldataSource == null;
			if (flag2)
			{
				aldataSource = new ALDataSource
				{
					ScreenID = text
				};
				aldataSource = aldataSourceMaint.Document.Insert(aldataSource);
				aldataSourceMaint.Actions.PressSave();
			}
			aldataSourceMaint.Document.Current = aldataSource;
			throw new PXRedirectRequiredException(aldataSourceMaint, true, "ALDataSource");
		}

		// Token: 0x060011AA RID: 4522 RVA: 0x000393A8 File Offset: 0x000375A8
		[PXUIField]
		[PXButton]
		public virtual IEnumerable ViewGenInquiry(PXAdapter adapter)
		{
			ALModel almodel = this.Model.Current;
			bool flag = almodel == null || almodel.TriggerDesignID == null;
			IEnumerable result;
			if (flag)
			{
				result = adapter.Get();
			}
			else
			{
				using (new PXPreserveScope())
				{
					GenericInquiryDesigner genericInquiryDesigner = HiddenUtils.CreateInstance<GenericInquiryDesigner>();
					genericInquiryDesigner.Designs.Current = genericInquiryDesigner.Designs.Search<GIDesign.designID>(this.Model.Current.TriggerDesignID, Array.Empty<object>());
					throw new PXRedirectRequiredException(genericInquiryDesigner, true, "Generic Inquiry");
				}
			}
			return result;
		}

		// Token: 0x060011AB RID: 4523 RVA: 0x00039458 File Offset: 0x00037658
		[PXUIField]
		[PXButton]
		public virtual IEnumerable ViewProvider(PXAdapter adapter)
		{
			ALModel almodel = this.Model.Current;
			bool flag = almodel == null || almodel.TriggerProviderID == null;
			IEnumerable result;
			if (flag)
			{
				result = adapter.Get();
			}
			else
			{
				using (new PXPreserveScope())
				{
					SYProviderMaint syproviderMaint = HiddenUtils.CreateInstance<SYProviderMaint>();
					syproviderMaint.Providers.Current = syproviderMaint.Providers.Search<SYProvider.providerID>(this.Model.Current.TriggerProviderID, Array.Empty<object>());
					throw new PXRedirectRequiredException(syproviderMaint, true, "Data Provider");
				}
			}
			return result;
		}

		// Token: 0x060011AC RID: 4524 RVA: 0x00039508 File Offset: 0x00037708
		[PXUIField]
		[PXButton]
		public virtual IEnumerable ViewImpScenario(PXAdapter adapter)
		{
			ALModel almodel = this.Model.Current;
			bool flag = almodel == null || almodel.TriggerMappingID == null;
			IEnumerable result;
			if (flag)
			{
				result = adapter.Get();
			}
			else
			{
				using (new PXPreserveScope())
				{
					SYImportMaint syimportMaint = HiddenUtils.CreateInstance<SYImportMaint>();
					syimportMaint.Mappings.Current = syimportMaint.Mappings.Search<SYMapping.mappingID>(this.Model.Current.TriggerMappingID, Array.Empty<object>());
					throw new PXRedirectRequiredException(syimportMaint, true, "Import Scenario");
				}
			}
			return result;
		}

		// Token: 0x060011AD RID: 4525 RVA: 0x000395B8 File Offset: 0x000377B8
		[PXUIField]
		[PXButton]
		public virtual IEnumerable ViewBusEvent(PXAdapter adapter)
		{
			ALModel almodel = this.Model.Current;
			bool flag = almodel == null || almodel.TriggerEventID == null;
			IEnumerable result;
			if (flag)
			{
				result = adapter.Get();
			}
			else
			{
				using (new PXPreserveScope())
				{
					BusinessProcessEventMaint businessProcessEventMaint = HiddenUtils.CreateInstance<BusinessProcessEventMaint>();
					businessProcessEventMaint.Events.Current = businessProcessEventMaint.Events.Search<BPEvent.eventID>(this.Model.Current.TriggerEventID, Array.Empty<object>());
					throw new PXRedirectRequiredException(businessProcessEventMaint, true, "Business Event");
				}
			}
			return result;
		}

		// Token: 0x060011AE RID: 4526 RVA: 0x00039668 File Offset: 0x00037868
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

		// Token: 0x060011AF RID: 4527 RVA: 0x0003973C File Offset: 0x0003793C
		[PXUIField(DisplayName = "Link To Data Elements")]
		[PXButton]
		public virtual IEnumerable findDataElements(PXAdapter adapter)
		{
			ALModel almodel = this.Model.Current;
			string text = (almodel != null) ? almodel.ScreenID : null;
			bool flag = string.IsNullOrEmpty(text);
			IEnumerable result;
			if (flag)
			{
				result = adapter.Get();
			}
			else
			{
				ALDataSourceMaint aldataSourceMaint = HiddenUtils.CreateInstance<ALDataSourceMaint>();
				ALDataSource aldataSource = aldataSourceMaint.Document.Search<ALDataSource.screenID>(text, Array.Empty<object>());
				bool flag2 = aldataSource == null && aldataSourceMaint.Document.Ask("Would you like", 1) == 1;
				if (flag2)
				{
					aldataSource = new ALDataSource
					{
						ScreenID = text
					};
					aldataSource = aldataSourceMaint.Document.Insert(aldataSource);
					aldataSourceMaint.Actions.PressSave();
				}
				bool flag3 = aldataSource != null;
				if (flag3)
				{
					ALModelExprSelect almodelExprSelect = new ALModelExprSelect(this);
					almodelExprSelect.WhereAnd<Where<ALModelExpr.dataElementID, IsNull, And<ALModelExpr.active, Equal<True>>>>();
					PXResultset<ALModelExpr> pxresultset = almodelExprSelect.Select(Array.Empty<object>());
					foreach (PXResult<ALModelExpr> pxresult in pxresultset)
					{
						ALModelExpr almodelExpr = pxresult;
						ALDataElement aldataElement;
						aldataSourceMaint.FindAndInsertDataElement(aldataSource, almodelExpr, false, out aldataElement);
						bool flag4 = aldataElement == null;
						if (!flag4)
						{
							almodelExpr.DataElementID = aldataElement.RecordID;
							almodelExprSelect.Update(almodelExpr);
						}
					}
				}
				this.Expressions.View.RequestRefresh();
				result = adapter.Get();
			}
			return result;
		}

		// Token: 0x060011B0 RID: 4528 RVA: 0x000398A0 File Offset: 0x00037AA0
		[PXUIField]
		[PXButton]
		protected virtual IEnumerable loadLabelZoom(PXAdapter adapter)
		{
			PXToggleAsyncDelegate pxtoggleAsyncDelegate;
			if ((pxtoggleAsyncDelegate = ALModelMaint.<>O.<1>__DoLoadLabelZoom) == null)
			{
				pxtoggleAsyncDelegate = (ALModelMaint.<>O.<1>__DoLoadLabelZoom = new PXToggleAsyncDelegate(ALModelMaint.DoLoadLabelZoom));
			}
			PXLongOperation.StartOperation(this, pxtoggleAsyncDelegate);
			return adapter.Get();
		}

		// Token: 0x060011B1 RID: 4529 RVA: 0x000398DC File Offset: 0x00037ADC
		private static void DoLoadLabelZoom()
		{
			IEnumerable<ALUnboundLabelZoomHeader> labels = LabelZoomUtils.GetLabels();
			bool flag = labels != null && labels.Any<ALUnboundLabelZoomHeader>();
			if (flag)
			{
				ALModelMaint almodelMaint = HiddenUtils.CreateInstance<ALModelMaint>();
				PXCache cache = almodelMaint.Model.Cache;
				foreach (ALUnboundLabelZoomHeader alunboundLabelZoomHeader in labels)
				{
					Guid? id = alunboundLabelZoomHeader.ID;
					bool flag2 = id == null;
					if (!flag2)
					{
						try
						{
							Match match = ALModelMaint.SCREEN_REGEX.Match(alunboundLabelZoomHeader.Name);
							bool flag3 = !match.Success && !string.IsNullOrEmpty(alunboundLabelZoomHeader.Description);
							if (flag3)
							{
								ALModelMaint.SCREEN_REGEX.Match(alunboundLabelZoomHeader.Description);
							}
							bool flag4 = !match.Success;
							string screenID;
							if (flag4)
							{
								PXTrace.WriteWarning("Could not find a Screen ID in LabelZoom '{0}'", new object[]
								{
									ALModelMaint.ToString(alunboundLabelZoomHeader)
								});
								screenID = "SO302000";
							}
							else
							{
								screenID = match.Groups[1].Value;
							}
							IFormat dummy = Formats.GetDummy("FIXME");
							ALModel almodel = almodelMaint.Model.Search<ALModel.cloudID>(id.ToString(), Array.Empty<object>());
							bool flag5 = almodel == null && !string.IsNullOrEmpty(alunboundLabelZoomHeader.Name);
							if (flag5)
							{
								almodel = new ALModel
								{
									CloudID = id.ToString(),
									ModelType = "L",
									Active = new bool?(true),
									IsSystem = new bool?(false),
									AllowExport = new bool?(true),
									ScreenID = screenID
								};
								cache.SetDefaultExt<ALModel.graphType>(almodel);
								cache.SetDefaultExt<ALModel.basedOnView>(almodel);
								almodel = (ALModel)cache.Insert(almodel);
							}
							almodel.Name = (alunboundLabelZoomHeader.Name ?? id.ToString());
							almodel.Description = (alunboundLabelZoomHeader.Description ?? id.ToString());
							almodel.ScreenID = screenID;
							cache.Update(almodel);
							almodelMaint.Actions.PressSave();
						}
						catch (Exception ex)
						{
							PXTrace.WriteError("Error loading LabelZoom {0}", new object[]
							{
								ALModelMaint.ToString(alunboundLabelZoomHeader)
							});
							PXTrace.WriteError(ex);
						}
					}
				}
			}
		}

		// Token: 0x060011B2 RID: 4530 RVA: 0x00039B88 File Offset: 0x00037D88
		private static string ToString(ALUnboundLabelZoomHeader lzModel)
		{
			return string.Format("LabelZoomHeader: {0} ({1}), ID={2}", lzModel.Name, lzModel.Description, lzModel.ID);
		}

		// Token: 0x060011B3 RID: 4531 RVA: 0x00039BBC File Offset: 0x00037DBC
		private void DoPrintAsZpl(ALModel model, PXAdapter adapter)
		{
			this.Model.Current = model;
			LabelContext labelContext = LabelContext.CreatePrintContext(base.GetType(), model, model.LabelID, false, adapter);
			BasicLabelGenerator basicLabelGenerator = new BasicLabelGenerator();
			basicLabelGenerator.PrintLabels(labelContext);
			this.Cancel.Press();
		}

		// Token: 0x060011B4 RID: 4532 RVA: 0x00039C08 File Offset: 0x00037E08
		private void DoPrintAsPdf(ALModel model, PXAdapter adapter)
		{
			this.Model.Current = model;
			bool flag = !BasicLabelUtils.HasFile(model, OutputFormat.PNG);
			if (flag)
			{
				this.DoRender(model);
			}
			FileInfo file = BasicLabelUtils.GetFile(model, OutputFormat.PNG);
			bool flag2 = file == null;
			if (flag2)
			{
				throw new PXException("Can't find a Rendering file to print");
			}
			LabelContext labelContext = LabelContext.CreatePrintContext(base.GetType(), model, model.LabelID, false, adapter);
			FileResult result = PngToPdf.INSTANCE.Transform(labelContext, file);
			IDestination destination = labelContext.GetDestination();
			destination.Print(labelContext, result);
			this.Cancel.Press();
		}

		// Token: 0x060011B5 RID: 4533 RVA: 0x00039C9C File Offset: 0x00037E9C
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

		// Token: 0x060011B6 RID: 4534 RVA: 0x00039DBC File Offset: 0x00037FBC
		private void RecalulatePositions(ALModel model, string layoutType)
		{
			PXResultset<ALModelExpr> pxresultset = this.Expressions.Select(Array.Empty<object>());
			PXResultset<ALModelGraphic> pxresultset2 = this.Graphics.Select(Array.Empty<object>());
			bool flag = !pxresultset.Any<PXResult<ALModelExpr>>() && !pxresultset2.Any<PXResult<ALModelGraphic>>();
			if (!flag)
			{
				LabelContext labelContext = LabelContext.CreateRenderContext(this, model, model.LabelID, null, OutputFormat.PNG);
				TemplateContext scribanContext = labelContext.ScribanContext;
				Layout layout = ContextVariables.GetLayout(scribanContext);
				foreach (PXResult<ALModelExpr> pxresult in pxresultset)
				{
					ALModelExpr almodelExpr = pxresult;
					bool flag2 = layout.PositionRecalculated(almodelExpr, layoutType);
					if (flag2)
					{
						this.Expressions.Update(almodelExpr);
					}
				}
				foreach (PXResult<ALModelGraphic> pxresult2 in pxresultset2)
				{
					ALModelGraphic almodelGraphic = pxresult2;
					GraphicFromCoordinate coordinate = new GraphicFromCoordinate(almodelGraphic);
					GraphicToCoordinate coordinate2 = new GraphicToCoordinate(almodelGraphic);
					bool flag3 = layout.PositionRecalculated(coordinate, layoutType) || layout.PositionRecalculated(coordinate2, layoutType);
					if (flag3)
					{
						this.Graphics.Update(almodelGraphic);
					}
				}
			}
		}

		// Token: 0x060011B7 RID: 4535 RVA: 0x00039F10 File Offset: 0x00038110
		private void AdjustPosition(ALModel model, ALModelExpr expr, ALModelMaint.Direction direction)
		{
			bool flag = model == null || expr == null || model.FormatID == null;
			if (!flag)
			{
				bool flag2;
				if (model.SizeUnit != null && model.MoveBy != null)
				{
					decimal? moveBy = model.MoveBy;
					decimal d = 0m;
					flag2 = (moveBy.GetValueOrDefault() == d & moveBy != null);
				}
				else
				{
					flag2 = true;
				}
				bool flag3 = flag2;
				if (flag3)
				{
					model.SizeUnit = "MM";
					model.MoveBy = new decimal?(1);
				}
				this.DoAdjustPosition(model, expr, direction);
				this.Render.Press();
			}
		}

		// Token: 0x060011B8 RID: 4536 RVA: 0x00039FBC File Offset: 0x000381BC
		private void DoAdjustPosition(ALModel model, ALModelExpr expr, ALModelMaint.Direction direction)
		{
			IFormat value = RuleUtils.FORMAT_FACTORY.GetValue(model.FormatID);
			int dots = ALSizeUnit.GetDots(new decimal?(model.MoveBy.Value), model.SizeUnit, value);
			bool flag = model.LayoutType == "D";
			decimal num;
			if (flag)
			{
				num = dots;
			}
			else
			{
				num = 1.0m;
				Margins.Margin margin;
				Margins.TryGetMargin(model.MarginID, out margin);
				TemplateContext context = ScribanUtils.CreateContext(this, expr, null, true, new object[]
				{
					model,
					value,
					margin,
					this.LabelSetup.Select(Array.Empty<object>())
				});
				Layout layout = new Layout(context);
				if (direction > ALModelMaint.Direction.Down)
				{
					if (direction - ALModelMaint.Direction.Right <= 1)
					{
						num = layout.CalcColDotsToPerc(dots);
					}
				}
				else
				{
					num = layout.CalcRowDotsToPerc(dots);
				}
			}
			decimal? num2 = expr.PosX;
			decimal num3 = num2.GetValueOrDefault();
			if (num2 == null)
			{
				num3 = 0.0m;
				decimal? num4 = new decimal?(num3);
				expr.PosX = num4;
			}
			num2 = expr.PosY;
			num3 = num2.GetValueOrDefault();
			if (num2 == null)
			{
				num3 = 0.0m;
				decimal? num4 = new decimal?(num3);
				expr.PosY = num4;
			}
			switch (direction)
			{
			case ALModelMaint.Direction.Up:
			{
				num2 = expr.PosY;
				num3 = num;
				decimal? posY;
				if (num2 == null)
				{
					decimal? num4 = null;
					posY = num4;
				}
				else
				{
					posY = new decimal?(num2.GetValueOrDefault() - num3);
				}
				expr.PosY = posY;
				break;
			}
			case ALModelMaint.Direction.Down:
			{
				num2 = expr.PosY;
				num3 = num;
				decimal? posY2;
				if (num2 == null)
				{
					decimal? num4 = null;
					posY2 = num4;
				}
				else
				{
					posY2 = new decimal?(num2.GetValueOrDefault() + num3);
				}
				expr.PosY = posY2;
				break;
			}
			case ALModelMaint.Direction.Right:
			{
				num2 = expr.PosX;
				num3 = num;
				decimal? posX;
				if (num2 == null)
				{
					decimal? num4 = null;
					posX = num4;
				}
				else
				{
					posX = new decimal?(num2.GetValueOrDefault() + num3);
				}
				expr.PosX = posX;
				break;
			}
			case ALModelMaint.Direction.Left:
			{
				num2 = expr.PosX;
				num3 = num;
				decimal? posX2;
				if (num2 == null)
				{
					decimal? num4 = null;
					posX2 = num4;
				}
				else
				{
					posX2 = new decimal?(num2.GetValueOrDefault() - num3);
				}
				expr.PosX = posX2;
				break;
			}
			}
			this.Expressions.Update(expr);
		}

		// Token: 0x060011B9 RID: 4537 RVA: 0x0003A240 File Offset: 0x00038440
		protected virtual Type[] GetAlternativeKeyFields()
		{
			return new Type[]
			{
				typeof(ALModelExpr.exprCode)
			};
		}

		// Token: 0x1700062E RID: 1582
		// (get) Token: 0x060011BA RID: 4538 RVA: 0x0003A268 File Offset: 0x00038468
		private bool DontUpdateExistRecords
		{
			get
			{
				object obj;
				bool flag = !base.IsImportFromExcel || !PXExecutionContext.Current.Bag.TryGetValue("_DONT_UPDATE_EXIST_RECORDS", out obj);
				return !flag && true.Equals(obj);
			}
		}

		// Token: 0x060011BB RID: 4539 RVA: 0x0003A2B4 File Offset: 0x000384B4
		public virtual bool PrepareImportRow(string viewName, IDictionary keys, IDictionary values)
		{
			AsgardUtils.SetImportFields(viewName, values, "Children", this.Model.Current.LabelID, new Type[]
			{
				typeof(ALModelChild.labelID)
			});
			AsgardUtils.SetImportFields(viewName, values, "Expressions", this.Model.Current.LabelID, new Type[]
			{
				typeof(ALModelExpr.labelID)
			});
			AsgardUtils.SetImportFields(viewName, values, "Printers", this.Model.Current.LabelID, new Type[]
			{
				typeof(ALModelPrinter.labelID)
			});
			AsgardUtils.SetImportFields(viewName, values, "Graphics", this.Model.Current.LabelID, new Type[]
			{
				typeof(ALModelGraphic.modelID)
			});
			AsgardUtils.SetImportFields(viewName, values, "Model", null, new Type[]
			{
				typeof(ALModel.rendered),
				typeof(ALModel.message)
			});
			bool flag = string.Compare(viewName, "Expressions", true) != 0;
			bool result;
			if (flag)
			{
				result = true;
			}
			else
			{
				bool dontUpdateExistRecords = this.DontUpdateExistRecords;
				if (dontUpdateExistRecords)
				{
					result = true;
				}
				else
				{
					bool flag2 = this.DuplicateFinder == null;
					if (flag2)
					{
						ALModelExpr[] array = this.Expressions.SelectMain(Array.Empty<object>());
						this.DuplicateFinder = new MultiDuplicatesSearchEngine<ALModelExpr>(this.Expressions.Cache, this.GetAlternativeKeyFields(), array);
					}
					ALModelExpr almodelExpr = this.DuplicateFinder.Find(values);
					bool flag3 = almodelExpr != null;
					if (flag3)
					{
						this.DuplicateFinder.RemoveItem(almodelExpr);
						bool flag4 = !keys.Contains("lineNbr");
						if (flag4)
						{
							keys.Add("LineNbr", almodelExpr.LineNbr);
						}
						else
						{
							keys["LineNbr"] = almodelExpr.LineNbr;
						}
					}
					else
					{
						bool flag5 = keys.Contains("lineNbr");
						if (flag5)
						{
							bool flag6 = false;
							object obj = keys["lineNbr"];
							bool flag7 = this.Expressions.Cache.RaiseFieldUpdating<ALModelExpr.lineNbr>(null, ref obj) && obj is int;
							if (flag7)
							{
								int value = (int)obj;
								ALModelExpr almodelExpr2 = new ALModelExpr
								{
									LabelID = this.Model.Current.LabelID,
									LineNbr = new int?(value)
								};
								flag6 = (this.Expressions.Cache.Locate(almodelExpr2) != null);
							}
							bool flag8 = flag6;
							if (flag8)
							{
								keys.Remove("lineNbr");
							}
						}
					}
					result = true;
				}
			}
			return result;
		}

		// Token: 0x060011BC RID: 4540 RVA: 0x0003A55B File Offset: 0x0003875B
		protected static void CorrectKey(string name, object value, IDictionary keys, IDictionary values)
		{
			ALModelMaint.CorrectKey(name, value, keys);
			ALModelMaint.CorrectKey(name, value, values);
		}

		// Token: 0x060011BD RID: 4541 RVA: 0x0003A570 File Offset: 0x00038770
		protected static void CorrectKey(string name, object value, IDictionary dict)
		{
			bool flag = dict.Contains(name);
			if (flag)
			{
				dict[name] = value;
			}
			else
			{
				dict.Add(name, value);
			}
		}

		// Token: 0x060011BE RID: 4542 RVA: 0x00019FF9 File Offset: 0x000181F9
		public virtual void ImportDone(PXImportAttribute.ImportMode.Value mode)
		{
		}

		// Token: 0x060011BF RID: 4543 RVA: 0x0003A5A0 File Offset: 0x000387A0
		public bool RowImported(string viewName, object row, object oldRow)
		{
			return oldRow == null;
		}

		// Token: 0x060011C0 RID: 4544 RVA: 0x0003A5B8 File Offset: 0x000387B8
		public bool RowImporting(string viewName, object row)
		{
			ALModel almodel;
			bool flag;
			if (viewName == "Model")
			{
				almodel = (row as ALModel);
				flag = (almodel != null);
			}
			else
			{
				flag = false;
			}
			bool flag2 = flag;
			if (flag2)
			{
				almodel.Rendered = null;
			}
			return row == null;
		}

		// Token: 0x060011C1 RID: 4545 RVA: 0x00019FF9 File Offset: 0x000181F9
		public void PrepareItems(string viewName, IEnumerable items)
		{
		}

		// Token: 0x040007F5 RID: 2037
		private static readonly Regex SCREEN_REGEX = new Regex("([A-Z]{2}[0-9]{6})");

		// Token: 0x040007F6 RID: 2038
		[ALImportExportAll]
		[PXViewName("Label Models")]
		[PXCopyPasteHiddenFields(new Type[]
		{
			typeof(ALModel.labelID),
			typeof(ALModel.showAutomation),
			typeof(ALModel.showChildren),
			typeof(ALModel.showExprs),
			typeof(ALModel.showPrinters),
			typeof(ALModel.showPrintLog),
			typeof(ALModel.showRendered),
			typeof(ALModel.showTemplate),
			typeof(ALModel.showUsedBy),
			typeof(ALModel.triggerDesignID),
			typeof(ALModel.triggerEventID),
			typeof(ALModel.triggerMappingID),
			typeof(ALModel.triggerProviderID),
			typeof(ALModel.imageUrl),
			typeof(ALModel.rendered),
			typeof(ALModel.message),
			typeof(ALModel.actionName)
		})]
		public PXSelect<ALModel> Model;

		// Token: 0x040007F7 RID: 2039
		public PXSelect<ALModel, Where<ALModel.labelID, Equal<Current<ALModel.labelID>>>> CurrentModel;

		// Token: 0x040007F8 RID: 2040
		[PXCopyPasteHiddenView]
		public PXSetup<ALSetup> LabelSetup;

		// Token: 0x040007F9 RID: 2041
		[PXImport(typeof(ALModel))]
		public ALModelPrinterSelect Printers;

		// Token: 0x040007FA RID: 2042
		public PXSelectReadonly<ALModelPrinter, Where<ALModelPrinter.labelID, Equal<Current<ALModel.labelID>>>, OrderBy<Asc<ALModelPrinter.sortOrder, Asc<ALModelPrinter.lineNbr>>>> PrintersReadOnly;

		// Token: 0x040007FB RID: 2043
		[PXImport(typeof(ALModel))]
		public ALModelExprSelect Expressions;

		// Token: 0x040007FC RID: 2044
		[PXCopyPasteHiddenView]
		public PXSelect<ALModelExpr, Where<ALModelExpr.exprCode, Equal<Required<ALModelExpr.exprCode>>>> ExprByCode;

		// Token: 0x040007FD RID: 2045
		public ALModelChildSelect Children;

		// Token: 0x040007FE RID: 2046
		[PXImport(typeof(ALModel))]
		public ALModelGraphicSelect Graphics;

		// Token: 0x040007FF RID: 2047
		[PXCopyPasteHiddenView]
		public PXSelect<ALPrintLog, Where<ALPrintLog.modelID, Equal<Current<ALModel.labelID>>>, OrderBy<Desc<ALPrintLog.createdDateTime>>> PrintLog;

		// Token: 0x04000800 RID: 2048
		[PXCopyPasteHiddenView]
		public PXSelect<ALModel, Where<ALModel.screenID, Equal<Required<ALModel.screenID>>, And<ALModel.modelType, Equal<ALModelType.single>, And<ALModel.active, Equal<True>>>>> PossibleChildren;

		// Token: 0x04000801 RID: 2049
		[PXCopyPasteHiddenView]
		public PXSelect<ALModelChild, Where<ALModelChild.labelID, Equal<Current<ALModel.labelID>>, And<ALModelChild.labelChildID, Equal<Required<ALModelChild.labelChildID>>>>> FindChild;

		// Token: 0x04000802 RID: 2050
		[PXCopyPasteHiddenView]
		public PXFilter<ALDataElementFilter> DataElementFilter;

		// Token: 0x04000803 RID: 2051
		[PXCopyPasteHiddenView]
		public FbqlSelect<SelectFromBase<ALDataElement, TypeArrayOf<IFbqlJoin>.Append<TypeArrayOf<IFbqlJoin>.Empty, FbqlJoins.Inner<ALDataSource>.On<ALDataElement.FK.Parent>>>.Where<BqlChainableConditionMirror<TypeArrayOf<IBqlBinary>.Append<TypeArrayOf<IBqlBinary>.Append<TypeArrayOf<IBqlBinary>.Append<TypeArrayOf<IBqlBinary>.Append<TypeArrayOf<IBqlBinary>.Append<TypeArrayOf<IBqlBinary>.Append<TypeArrayOf<IBqlBinary>.Append<TypeArrayOf<IBqlBinary>.FilledWith<And<Compare<ALDataSource.screenID, Equal<BqlField<ALModel.screenID, IBqlString>.FromCurrent>>>>, And<BqlChainableConditionBase<TypeArrayOf<IBqlBinary>.FilledWith<And<Compare<Current<ALDataElementFilter.exprType>, IsNull>>>>.Or<BqlOperand<ALDataElement.exprType, IBqlString>.IsEqual<BqlField<ALDataElementFilter.exprType, IBqlString>.FromCurrent>>>>, And<BqlChainableConditionBase<TypeArrayOf<IBqlBinary>.FilledWith<And<Compare<Current<ALDataElementFilter.basedOn>, IsNull>>>>.Or<BqlOperand<ALDataElement.basedOn, IBqlString>.Contains<BqlField<ALDataElementFilter.basedOn, IBqlString>.FromCurrent>>>>, And<BqlChainableConditionBase<TypeArrayOf<IBqlBinary>.FilledWith<And<Compare<Current<ALDataElementFilter.exprValue>, IsNull>>>>.Or<BqlOperand<ALDataElement.exprValue, IBqlString>.Contains<BqlField<ALDataElementFilter.exprValue, IBqlString>.FromCurrent>>>>, And<BqlChainableConditionBase<TypeArrayOf<IBqlBinary>.FilledWith<And<Compare<Current<ALDataElementFilter.withBarcode>, Equal<False>>>>>.Or<BqlOperand<ALDataElement.barcodeID, IBqlGuid>.IsNotNull>>>, And<BqlChainableConditionBase<TypeArrayOf<IBqlBinary>.FilledWith<And<Compare<Current<ALDataElementFilter.categoryID>, IsNull>>>>.Or<BqlOperand<ALDataElement.categoryID, IBqlGuid>.IsEqual<BqlField<ALDataElementFilter.categoryID, IBqlGuid>.FromCurrent>>>>, And<BqlChainableConditionBase<TypeArrayOf<IBqlBinary>.FilledWith<And<Compare<Current<ALDataElementFilter.contentID>, IsNull>>>>.Or<BqlOperand<ALDataElement.contentID, IBqlGuid>.IsEqual<BqlField<ALDataElementFilter.contentID, IBqlGuid>.FromCurrent>>>>, And<BqlChainableConditionBase<TypeArrayOf<IBqlBinary>.FilledWith<And<Compare<Current<ALDataElementFilter.substitutionID>, IsNull>>>>.Or<BqlOperand<ALDataElement.substitutionID, IBqlGuid>.IsEqual<BqlField<ALDataElementFilter.substitutionID, IBqlGuid>.FromCurrent>>>>>.And<BqlOperand<ALDataSource.screenID, IBqlString>.IsEqual<BqlField<ALModel.screenID, IBqlString>.FromCurrent>>>, ALDataElement>.View SelectedDataElements;

		// Token: 0x04000804 RID: 2052
		[PXCopyPasteHiddenView]
		public PXSelect<ALDataElement, Where<ALDataElement.snippetID, Equal<Current<ALModel.labelID>>>, OrderBy<Asc<ALDataElement.name>>> UsedByDataElements;

		// Token: 0x04000809 RID: 2057
		public ALChangeID<ALModel, ALModel.name> ChangeID;

		// Token: 0x0400080A RID: 2058
		public PXAction<ALModel> Action;

		// Token: 0x0400080B RID: 2059
		public PXAction<ALModel> Render;

		// Token: 0x0400080C RID: 2060
		public PXAction<ALModel> PrintAsZPL;

		// Token: 0x0400080D RID: 2061
		public PXAction<ALModel> PrintAsPDF;

		// Token: 0x0400080E RID: 2062
		public PXAction<ALModel> ViewImage;

		// Token: 0x0400080F RID: 2063
		public PXAction<ALModel> ViewScreen;

		// Token: 0x04000810 RID: 2064
		public PXAction<ALModel> viewLabelChild;

		// Token: 0x04000811 RID: 2065
		public PXAction<ALModel> loadChildren;

		// Token: 0x04000812 RID: 2066
		public PXAction<ALModel> DeleteRenderings;

		// Token: 0x04000813 RID: 2067
		public PXAction<ALModel> MoveUp;

		// Token: 0x04000814 RID: 2068
		public PXAction<ALModel> MoveDown;

		// Token: 0x04000815 RID: 2069
		public PXAction<ALModel> MoveLeft;

		// Token: 0x04000816 RID: 2070
		public PXAction<ALModel> MoveRight;

		// Token: 0x04000817 RID: 2071
		public PXAction<ALModel> GenComponents;

		// Token: 0x04000818 RID: 2072
		public PXAction<ALModel> ClearCache;

		// Token: 0x04000819 RID: 2073
		public PXAction<ALModel> GenerateSource;

		// Token: 0x0400081A RID: 2074
		public PXAction<ALModel> viewGenInquiry;

		// Token: 0x0400081B RID: 2075
		public PXAction<ALModel> viewProvider;

		// Token: 0x0400081C RID: 2076
		public PXAction<ALModel> viewImpScenario;

		// Token: 0x0400081D RID: 2077
		public PXAction<ALModel> viewBusEvent;

		// Token: 0x0400081E RID: 2078
		public PXAction<ALModel> LoadDataElements;

		// Token: 0x0400081F RID: 2079
		public PXAction<ALModel> FindDataElements;

		// Token: 0x04000820 RID: 2080
		public PXAction<ALModel> LoadLabelZoom;

		// Token: 0x020008C9 RID: 2249
		public enum Direction
		{
			// Token: 0x0400115C RID: 4444
			Up,
			// Token: 0x0400115D RID: 4445
			Down,
			// Token: 0x0400115E RID: 4446
			Right,
			// Token: 0x0400115F RID: 4447
			Left
		}

		// Token: 0x020008CA RID: 2250
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x04001160 RID: 4448
			public static PXToggleAsyncDelegate <0>__DoDeleteRenderings;

			// Token: 0x04001161 RID: 4449
			public static PXToggleAsyncDelegate <1>__DoLoadLabelZoom;
		}
	}
}
