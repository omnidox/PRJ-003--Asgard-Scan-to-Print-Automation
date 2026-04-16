using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using AA.Objects.Core;
using AA.Objects.Core.PrintNode;
using AA.Objects.Labels.Integration.NbCopies;
using AA.Objects.Labels.Integration.PrinterOverride;
using AA.Objects.License;
using Asgard.Labels.Abstractions.Context;
using Asgard.Labels.Abstractions.Destination;
using Asgard.Labels.Abstractions.Helpers;
using Asgard.Labels.Abstractions.Interface;
using Asgard.Labels.Abstractions.Language;
using Asgard.Labels.Abstractions.Poco;
using Asgard.Labels.Abstractions.Service;
using Asgard.Labels.Impl;
using Asgard.Labels.Impl.Context;
using Asgard.Labels.Impl.Language.MyScriban;
using Asgard.Labels.Impl.Language.Zpl;
using Asgard.Labels.Impl.Transformer;
using Autofac.Core;
using CommonServiceLocator;
using PX.Api;
using PX.Common;
using PX.Data;
using PX.Objects.CS;
using PX.Objects.IN;
using PX.SM;
using Scriban;
using Scriban.Parsing;
using Scriban.Runtime;

namespace AA.Objects.Labels
{
	// Token: 0x020000E0 RID: 224
	[DebuggerDisplay("AcuLabelContext: {Model.ModelType}/{Model.Name} ({Model.Description}), Format: {ModelFormat.Name}, ScreenID:{Model.ScreenID}")]
	public class AcuLabelContext : AbstractLabelContext<IPXResultset, ALModel, IAcuPrintLog>, IAcuLabelContext, ILabelContext<IPXResultset, ALModel, IAcuPrintLog>, ILabelContext, IRuleEvalContext, IFontProvider, IColorProvider, IFileProvider, IRuleProvider, ISubstitutionProvider, IJustificationProvider, IBarcodeProvider, IModelProvider, IContentProvider, IStandardProvider, ISequenceProvider, IPrinterFileProvider, IConfigProvider, Asgard.Labels.Abstractions.Service.IFormatProvider, ILanguageFactory, ILabelElementProvider, IMarginProvider, IEventLogger, IPrinterProvider, ILanguageDriven, IAcuFileProvider, ITemplateLoader
	{
		// Token: 0x1700011B RID: 283
		// (get) Token: 0x0600041A RID: 1050 RVA: 0x00018335 File Offset: 0x00016535
		private IALLicenseManager LicenseManager
		{
			get
			{
				return this.LabelGraph.LicenseManager;
			}
		}

		// Token: 0x1700011C RID: 284
		// (get) Token: 0x0600041B RID: 1051 RVA: 0x00018342 File Offset: 0x00016542
		// (set) Token: 0x0600041C RID: 1052 RVA: 0x0001834A File Offset: 0x0001654A
		public override IAcuPrintLog PrintLog { get; protected set; }

		// Token: 0x0600041D RID: 1053 RVA: 0x00018354 File Offset: 0x00016554
		public override TService Resolve<[Nullable(1)] TService>(params Parameter[] parameters)
		{
			bool flag = parameters != null && parameters.Length != 0;
			if (flag)
			{
				throw new ArgumentException("Parameters not supported in this context", "parameters");
			}
			TService instance = ServiceLocator.Current.GetInstance<TService>();
			if (instance != null)
			{
				return instance;
			}
			throw this.GetException("Service of type '{0}' not found", new object[]
			{
				typeof(TService).Name
			});
		}

		// Token: 0x1700011D RID: 285
		// (get) Token: 0x0600041E RID: 1054 RVA: 0x000183C0 File Offset: 0x000165C0
		private int DetailRowNbr
		{
			get
			{
				bool flag = base.SingleRow != null;
				int result;
				if (flag)
				{
					result = 0;
				}
				else
				{
					IRowIterator<IPXResultset> detailRows = AcuContextVariables.GetDetailRows(base.ScribanContext);
					result = ((detailRows != null) ? detailRows.RowNumber : 0);
				}
				return result;
			}
		}

		// Token: 0x1700011E RID: 286
		// (get) Token: 0x0600041F RID: 1055 RVA: 0x000183FC File Offset: 0x000165FC
		// (set) Token: 0x06000420 RID: 1056 RVA: 0x00018404 File Offset: 0x00016604
		private ALPrintLogMaint PrintLogGraph { get; set; }

		// Token: 0x1700011F RID: 287
		// (get) Token: 0x06000421 RID: 1057 RVA: 0x0001840D File Offset: 0x0001660D
		// (set) Token: 0x06000422 RID: 1058 RVA: 0x00018415 File Offset: 0x00016615
		public AcuUserInfo UserInfo { get; set; }

		// Token: 0x17000120 RID: 288
		// (get) Token: 0x06000423 RID: 1059 RVA: 0x0001841E File Offset: 0x0001661E
		// (set) Token: 0x06000424 RID: 1060 RVA: 0x00018426 File Offset: 0x00016626
		private ALLabelHandler LabelGraph { get; set; }

		// Token: 0x17000121 RID: 289
		// (get) Token: 0x06000425 RID: 1061 RVA: 0x0001842F File Offset: 0x0001662F
		// (set) Token: 0x06000426 RID: 1062 RVA: 0x00018437 File Offset: 0x00016637
		private UploadFileMaintenance FileMaintenance { get; set; }

		// Token: 0x17000122 RID: 290
		// (get) Token: 0x06000427 RID: 1063 RVA: 0x00018440 File Offset: 0x00016640
		public override bool IsDesignMode
		{
			get
			{
				return this.Graph == null || this.Graph.GetType() == typeof(ALModelMaint);
			}
		}

		// Token: 0x17000123 RID: 291
		// (get) Token: 0x06000428 RID: 1064 RVA: 0x00018467 File Offset: 0x00016667
		public bool IsGI
		{
			get
			{
				return AsgardCoreUtils.IsGI(this.Graph);
			}
		}

		// Token: 0x17000124 RID: 292
		// (get) Token: 0x06000429 RID: 1065 RVA: 0x00018474 File Offset: 0x00016674
		public bool IsImport
		{
			get
			{
				PXGraph graph = this.Graph;
				return graph != null && graph.IsImport;
			}
		}

		// Token: 0x17000125 RID: 293
		// (get) Token: 0x0600042A RID: 1066 RVA: 0x00018488 File Offset: 0x00016688
		public bool IsExport
		{
			get
			{
				PXGraph graph = this.Graph;
				return graph != null && graph.IsExport;
			}
		}

		// Token: 0x17000126 RID: 294
		// (get) Token: 0x0600042B RID: 1067 RVA: 0x0001849C File Offset: 0x0001669C
		public bool IsMobile
		{
			get
			{
				PXGraph graph = this.Graph;
				return graph != null && graph.IsMobile;
			}
		}

		// Token: 0x17000127 RID: 295
		// (get) Token: 0x0600042C RID: 1068 RVA: 0x000184B0 File Offset: 0x000166B0
		// (set) Token: 0x0600042D RID: 1069 RVA: 0x000184B8 File Offset: 0x000166B8
		public bool IgnoreOverride { get; set; } = false;

		// Token: 0x17000128 RID: 296
		// (get) Token: 0x0600042E RID: 1070 RVA: 0x000184C1 File Offset: 0x000166C1
		public bool IsContractBasedAPI
		{
			get
			{
				PXGraph graph = this.Graph;
				return graph != null && graph.IsContractBasedAPI;
			}
		}

		// Token: 0x17000129 RID: 297
		// (get) Token: 0x0600042F RID: 1071 RVA: 0x000184D5 File Offset: 0x000166D5
		// (set) Token: 0x06000430 RID: 1072 RVA: 0x000184DD File Offset: 0x000166DD
		public PXAdapter Adapter { get; set; }

		// Token: 0x1700012A RID: 298
		// (get) Token: 0x06000431 RID: 1073 RVA: 0x000184E6 File Offset: 0x000166E6
		// (set) Token: 0x06000432 RID: 1074 RVA: 0x000184EE File Offset: 0x000166EE
		public PXGraph Graph { get; protected set; }

		// Token: 0x1700012B RID: 299
		// (get) Token: 0x06000433 RID: 1075 RVA: 0x000184F7 File Offset: 0x000166F7
		public Type GraphType
		{
			get
			{
				PXGraph graph = this.Graph;
				return (graph != null) ? graph.GetType() : null;
			}
		}

		// Token: 0x1700012C RID: 300
		// (get) Token: 0x06000434 RID: 1076 RVA: 0x0001850C File Offset: 0x0001670C
		// (set) Token: 0x06000435 RID: 1077 RVA: 0x0001857C File Offset: 0x0001677C
		public override IEnumerable<IModelDetail> Expressions
		{
			get
			{
				bool flag = this._exprs == null;
				if (flag)
				{
					this._exprs = (from expr in this.LabelGraph.Expressions.Select(Array.Empty<object>()).FirstTableItems
					select expr).ToArray<IModelDetail>();
				}
				return this._exprs;
			}
			set
			{
				this._exprs = value;
			}
		}

		// Token: 0x1700012D RID: 301
		// (get) Token: 0x06000436 RID: 1078 RVA: 0x00018588 File Offset: 0x00016788
		public override IEnumerable<IModelGraphic> Graphics
		{
			get
			{
				return (from gr in this.LabelGraph.Graphics.Select(Array.Empty<object>()).FirstTableItems
				select gr).ToArray<IModelGraphic>();
			}
		}

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x06000437 RID: 1079 RVA: 0x000185D8 File Offset: 0x000167D8
		public override IEnumerable<IRenderableChild<Guid?>> Children
		{
			get
			{
				return this.LabelGraph.Children.Select(Array.Empty<object>()).FirstTableItems.Cast<IRenderableChild<Guid?>>().ToArray<IRenderableChild<Guid?>>();
			}
		}

		// Token: 0x1700012F RID: 303
		// (get) Token: 0x06000438 RID: 1080 RVA: 0x000185FE File Offset: 0x000167FE
		public override IEnumerable<IFont> Fonts
		{
			get
			{
				return BasicLabelUtils.GetFontsWithFileID(this.LabelGraph.Expressions.Select(Array.Empty<object>()));
			}
		}

		// Token: 0x17000130 RID: 304
		// (get) Token: 0x06000439 RID: 1081 RVA: 0x0001861A File Offset: 0x0001681A
		public override IEnumerable<FontFile> FontFiles
		{
			get
			{
				return BasicLabelUtils.GetFontFiles(this);
			}
		}

		// Token: 0x17000131 RID: 305
		// (get) Token: 0x0600043A RID: 1082 RVA: 0x00018622 File Offset: 0x00016822
		// (set) Token: 0x0600043B RID: 1083 RVA: 0x00018634 File Offset: 0x00016834
		public override ALModel Model
		{
			get
			{
				return this.LabelGraph.Model.Current;
			}
			protected set
			{
				this.LabelGraph.Model.Current = value;
			}
		}

		// Token: 0x0600043C RID: 1084 RVA: 0x0001864C File Offset: 0x0001684C
		public override IIteratorContext<IPXResultset> GetIteratorContext(ILabelElement dataElement = null, ICoordinate coordinate = null)
		{
			string name = typeof(IIteratorContext).Name;
			IIteratorContext<IPXResultset> iteratorContext = base.ScribanContext.GetValue(name, true, null);
			bool flag = iteratorContext == null && dataElement != null;
			if (flag)
			{
				iteratorContext = new AcuIteratorContext(this, dataElement, coordinate);
				base.ScribanContext.CurrentGlobal.SetValue(name, iteratorContext, false);
			}
			return iteratorContext;
		}

		// Token: 0x17000132 RID: 306
		// (get) Token: 0x0600043D RID: 1085 RVA: 0x000186AB File Offset: 0x000168AB
		// (set) Token: 0x0600043E RID: 1086 RVA: 0x000186C4 File Offset: 0x000168C4
		public override IEnumerable DetailRows
		{
			get
			{
				IRowIterator<IPXResultset> detailRows = AcuContextVariables.GetDetailRows(base.ScribanContext);
				return (detailRows != null) ? detailRows.Rows : null;
			}
			set
			{
				AcuContextVariables.SetDetailRows(base.ScribanContext, value as IPXResultset);
			}
		}

		// Token: 0x17000133 RID: 307
		// (get) Token: 0x0600043F RID: 1087 RVA: 0x000186D8 File Offset: 0x000168D8
		// (set) Token: 0x06000440 RID: 1088 RVA: 0x0001871C File Offset: 0x0001691C
		public override object DetailRow
		{
			get
			{
				bool flag = base.SingleRow != null;
				object result;
				if (flag)
				{
					result = base.SingleRow;
				}
				else
				{
					IRowIterator<IPXResultset> detailRows = AcuContextVariables.GetDetailRows(base.ScribanContext);
					result = ((detailRows != null) ? detailRows.Row : null);
				}
				return result;
			}
			set
			{
				IRowIterator<IPXResultset> detailRows = AcuContextVariables.GetDetailRows(base.ScribanContext);
				detailRows.Row = value;
				base.ResetRenderedBody();
				this.ResetIterator();
				this.PrintLog = null;
				this._nextSerial = null;
				this._lastSerial = null;
			}
		}

		// Token: 0x06000441 RID: 1089 RVA: 0x0001876C File Offset: 0x0001696C
		private void ResetIterator()
		{
			this.IteratorRows = null;
			ContextVariables.ResetIterator(base.ScribanContext);
		}

		// Token: 0x17000134 RID: 308
		// (get) Token: 0x06000442 RID: 1090 RVA: 0x00018784 File Offset: 0x00016984
		// (set) Token: 0x06000443 RID: 1091 RVA: 0x000187AA File Offset: 0x000169AA
		public override IRowIterator<IPXResultset> RowIterator
		{
			get
			{
				return base.ScribanContext.GetValue("AL_RowIterator", true, null);
			}
			set
			{
				base.ScribanContext.SetValue("AL_RowIterator", value);
			}
		}

		// Token: 0x17000135 RID: 309
		// (get) Token: 0x06000444 RID: 1092 RVA: 0x000187BE File Offset: 0x000169BE
		// (set) Token: 0x06000445 RID: 1093 RVA: 0x000187D4 File Offset: 0x000169D4
		public override IEnumerable IteratorRows
		{
			get
			{
				IRowIterator<IPXResultset> rowIterator = this.RowIterator;
				return (rowIterator != null) ? rowIterator.Rows : null;
			}
			set
			{
				IPXResultset ipxresultset = value as IPXResultset;
				this.RowIterator = new ResultSetIterator(base.ScribanContext, ipxresultset);
				bool flag = ipxresultset == null || ipxresultset.GetRowCount() == 0;
				if (flag)
				{
					ContextVariables.SetIteratorPageNbr(base.ScribanContext, 1);
				}
			}
		}

		// Token: 0x17000136 RID: 310
		// (get) Token: 0x06000446 RID: 1094 RVA: 0x0001881E File Offset: 0x00016A1E
		// (set) Token: 0x06000447 RID: 1095 RVA: 0x00018832 File Offset: 0x00016A32
		public override object IteratorRow
		{
			get
			{
				IRowIterator<IPXResultset> rowIterator = this.RowIterator;
				return (rowIterator != null) ? rowIterator.Row : null;
			}
			set
			{
				this.RowIterator.Row = value;
				base.ResetRenderedBody();
			}
		}

		// Token: 0x17000137 RID: 311
		// (get) Token: 0x06000448 RID: 1096 RVA: 0x00018849 File Offset: 0x00016A49
		// (set) Token: 0x06000449 RID: 1097 RVA: 0x0001885D File Offset: 0x00016A5D
		public override IEnumerable PageRows
		{
			get
			{
				IRowIterator<IPXResultset> pageIterator = this.PageIterator;
				return (pageIterator != null) ? pageIterator.Rows : null;
			}
			set
			{
				this.PageIterator = new ResultSetIterator(base.ScribanContext, value as IPXResultset);
			}
		}

		// Token: 0x17000138 RID: 312
		// (get) Token: 0x0600044A RID: 1098 RVA: 0x00018878 File Offset: 0x00016A78
		// (set) Token: 0x0600044B RID: 1099 RVA: 0x0001889E File Offset: 0x00016A9E
		public override IRowIterator<IPXResultset> PageIterator
		{
			get
			{
				return base.ScribanContext.GetValue("AL_PageIterator", true, null);
			}
			set
			{
				base.ScribanContext.SetValue("AL_PageIterator", value);
			}
		}

		// Token: 0x17000139 RID: 313
		// (get) Token: 0x0600044C RID: 1100 RVA: 0x000188B2 File Offset: 0x00016AB2
		// (set) Token: 0x0600044D RID: 1101 RVA: 0x000188C6 File Offset: 0x00016AC6
		public override object PageRow
		{
			get
			{
				IRowIterator<IPXResultset> pageIterator = this.PageIterator;
				return (pageIterator != null) ? pageIterator.Row : null;
			}
			set
			{
				this.PageIterator.Row = value;
			}
		}

		// Token: 0x1700013A RID: 314
		// (get) Token: 0x0600044E RID: 1102 RVA: 0x000188D8 File Offset: 0x00016AD8
		public override bool SendPause
		{
			get
			{
				ALModel model = this.Model;
				int valueOrDefault = ((model != null) ? model.SendPauseEvery : null).GetValueOrDefault();
				int num = this.DetailRowNbr + 1;
				IRowIterator<IPXResultset> detailRows = AcuContextVariables.GetDetailRows(base.ScribanContext);
				int num2 = (detailRows != null) ? detailRows.RowCount : 0;
				return valueOrDefault > 0 && num % valueOrDefault == 0 && num < num2;
			}
		}

		// Token: 0x1700013B RID: 315
		// (get) Token: 0x0600044F RID: 1103 RVA: 0x00018944 File Offset: 0x00016B44
		public override object LabelRow
		{
			get
			{
				object row = base.Row;
				object obj = this.DetailRow;
				IPXResultset ipxresultset = obj as IPXResultset;
				bool flag = ipxresultset != null && this.DetailRowNbr > -1;
				if (flag)
				{
					obj = ipxresultset.GetItem(this.DetailRowNbr, 0);
				}
				object obj2 = obj ?? row;
				return PXResult.UnwrapMain(obj2);
			}
		}

		// Token: 0x06000450 RID: 1104 RVA: 0x000189A4 File Offset: 0x00016BA4
		public static AcuLabelContext CreateSingleRowPrintContext(Type graphType, object row, object labelRow, Guid? modelID, int? bAccountID = null)
		{
			return new AcuLabelContext(graphType, row, modelID, false, false)
			{
				IsSilent = true,
				SingleRow = labelRow,
				IsAlwaysPrint = true,
				BAccountID = bAccountID
			};
		}

		// Token: 0x06000451 RID: 1105 RVA: 0x000189E4 File Offset: 0x00016BE4
		public static AcuLabelContext CreateSendRawContext(IPrinter printer, string language = "ZPL", string raw = null)
		{
			return new AcuLabelContext(printer, language, raw)
			{
				IsRaw = true
			};
		}

		// Token: 0x06000452 RID: 1106 RVA: 0x00018A08 File Offset: 0x00016C08
		public static AcuLabelContext CreateTestContext(PXGraph rowGraph, object row)
		{
			AcuTestContext acuTestContext = new AcuTestContext(rowGraph, row);
			TemplateContext templateContext = ScribanUtils.CreateContext(rowGraph, row, null, true, Array.Empty<object>());
			templateContext.SetValue(acuTestContext);
			acuTestContext.ScribanContext = templateContext;
			acuTestContext.ScribanContext.TemplateLoader = acuTestContext;
			return acuTestContext;
		}

		// Token: 0x06000453 RID: 1107 RVA: 0x00018A50 File Offset: 0x00016C50
		public override ILabelContext CreateRenderContext(string rendered, ContentFormat outputFormat = ContentFormat.PNG)
		{
			return new AcuLabelContext(null, ((IAcuLabelContext)this).GraphType ?? ((IAcuLabelContext)this).Graph.GetType(), ((ILabelContext)this).Row, ((ILabelContext<IPXResultset, ALModel, IAcuPrintLog>)this).Model.ModelID, true, ((IRuleEvalContext)this).ScribanContext, null, false)
			{
				Adapter = ((IAcuLabelContext)this).Adapter,
				FinalOutputFormat = outputFormat,
				TemplateBody = rendered
			};
		}

		// Token: 0x06000454 RID: 1108 RVA: 0x00018AB8 File Offset: 0x00016CB8
		public static AcuLabelContext CreateRenderContext(ALPrintLog printLog, string rendered, ContentFormat outputFormat = ContentFormat.PNG)
		{
			return new AcuLabelContext(printLog, rendered, outputFormat, null);
		}

		// Token: 0x06000455 RID: 1109 RVA: 0x00018AE0 File Offset: 0x00016CE0
		public static AcuLabelContext CreateReprintContext(ALPrintLog printLog, Guid? printerID = null)
		{
			return new AcuLabelContext(printLog, "", (ContentFormat)printLog.ContentType.Value, printerID);
		}

		// Token: 0x06000456 RID: 1110 RVA: 0x00018B10 File Offset: 0x00016D10
		public static AcuLabelContext CreateRenderContext(PXGraph graph, object row, Guid? modelID, PXAdapter adapter = null, ContentFormat outputFormat = ContentFormat.PNG)
		{
			return new AcuLabelContext(graph, row, modelID, true, false)
			{
				Adapter = adapter,
				FinalOutputFormat = outputFormat
			};
		}

		// Token: 0x06000457 RID: 1111 RVA: 0x00018B40 File Offset: 0x00016D40
		public static AcuLabelContext CreateMobilePrintContext(PXGraph mobileGraph, object row, Guid? modelID, PXAdapter adapter = null)
		{
			return new AcuLabelContext(mobileGraph, row, modelID, false, false)
			{
				Adapter = adapter
			};
		}

		// Token: 0x06000458 RID: 1112 RVA: 0x00018B68 File Offset: 0x00016D68
		private static object GetFilter(PXGraph mobileGraph)
		{
			string filterView = AsgardCoreUtils.GetFilterView(mobileGraph.GetType());
			bool flag = filterView != null;
			object result;
			if (flag)
			{
				PXView pxview = mobileGraph.Views[filterView];
				Type itemType = pxview.GetItemType();
				PXCache pxcache = mobileGraph.Caches[itemType];
				object obj = (pxcache != null) ? pxcache.Current : null;
				result = obj;
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06000459 RID: 1113 RVA: 0x00018BCC File Offset: 0x00016DCC
		public static AcuLabelContext CreatePrintContext(Type rowGraphType, object row, Guid? modelID, bool ignorePrinterMissing = false, PXAdapter adapter = null)
		{
			return new AcuLabelContext(rowGraphType, row, modelID, false, ignorePrinterMissing)
			{
				Adapter = adapter
			};
		}

		// Token: 0x0600045A RID: 1114 RVA: 0x00018BF4 File Offset: 0x00016DF4
		public static AcuLabelContext CreateChildContext(ILabelContext parent, Guid? labelChildID, bool ignorePrinterMissing = false)
		{
			IAcuLabelContext acuLabelContext = parent as IAcuLabelContext;
			return new AcuLabelContext(acuLabelContext.Graph, parent.Row, labelChildID, parent.IsRender, ignorePrinterMissing)
			{
				IsAlwaysPrint = parent.IsAlwaysPrint,
				IsSilent = parent.IsSilent,
				IgnorePrinterMissing = parent.IgnorePrinterMissing,
				Adapter = acuLabelContext.Adapter
			};
		}

		// Token: 0x0600045B RID: 1115 RVA: 0x00018C5C File Offset: 0x00016E5C
		protected AcuLabelContext(IPrinter printer, string languageCode, string raw) : this()
		{
			base.Language = (languageCode ?? (base.GetConfig<string>("DefaultLanguage") ?? "ZPL"));
			base.TemplateBody = raw;
			base.Printer = printer;
		}

		// Token: 0x0600045C RID: 1116 RVA: 0x00018C96 File Offset: 0x00016E96
		private AcuLabelContext(Type rowGraphType, object row, Guid? modelID, bool forRender, bool ignorePrinterMissing = false) : this(null, rowGraphType, row, modelID, forRender, ignorePrinterMissing)
		{
		}

		// Token: 0x0600045D RID: 1117 RVA: 0x00018CA8 File Offset: 0x00016EA8
		private AcuLabelContext(PXGraph rowGraph, object row, Guid? modelID, bool forRender, bool ignorePrinterMissing = false) : this(rowGraph, null, row, modelID, forRender, ignorePrinterMissing)
		{
		}

		// Token: 0x0600045E RID: 1118 RVA: 0x00018CBC File Offset: 0x00016EBC
		private AcuLabelContext(PXGraph rowGraph, Type rowGraphType, object row, Guid? modelID, bool forRender, bool ignorePrinterMissing = false) : this(rowGraph, rowGraphType, row, modelID, forRender, null, null, ignorePrinterMissing)
		{
		}

		// Token: 0x0600045F RID: 1119 RVA: 0x00018CDC File Offset: 0x00016EDC
		private AcuLabelContext(PXGraph rowGraph, Type rowGraphType, object row, Guid? modelID, bool forRender, TemplateContext context, ILabelContext parentContext = null, bool ignorePrinterMissing = false) : this()
		{
			base.IsRender = forRender;
			base.IgnorePrinterMissing = ignorePrinterMissing;
			this.HandleRow(row);
			this.HandleGraph(rowGraph, rowGraphType);
			this.HandleModel(modelID, context);
			bool flag = this.Model == null;
			if (flag)
			{
				throw this.GetException("No label found for id '{0}'", new object[]
				{
					modelID
				});
			}
			bool flag2 = row is PXResult;
			if (flag2)
			{
				row = PXResult.UnwrapMain(row);
			}
			AcuLabelContext acuLabelContext = (AcuLabelContext)parentContext;
			int? num = (acuLabelContext != null) ? acuLabelContext.BAccountID : null;
			base.BAccountID = ((num != null) ? num : AsgardUtils.GetBAccountID(this.Graph, row));
			string modelType = this.Model.ModelType;
			string a = modelType;
			if (!(a == "G"))
			{
				if (!(a == "S") && !(a == "L"))
				{
					if (a == "N")
					{
						base.ModelFormat = (((parentContext != null) ? parentContext.ModelFormat : null) ?? DefaultFormat.DEFAULT_FORMAT);
						base.ModelMargin = ((parentContext != null) ? parentContext.ModelMargin : null);
						base.Printer = ((parentContext != null) ? parentContext.Printer : null);
						base.PrinterFormat = ((parentContext != null) ? parentContext.PrinterFormat : null);
						base.PrinterMargin = ((parentContext != null) ? parentContext.PrinterMargin : null);
						bool flag3 = base.Printer == null && base.IsRender;
						if (flag3)
						{
							Guid? printerID = this.FindRenderer(base.CurrentFormat, base.FinalOutputFormat);
							IAcuPrinter printer;
							Printers.TryGetValue(printerID, out printer);
							base.Printer = printer;
						}
						this.VerifyModelFormat();
					}
				}
				else
				{
					this.HandleFormat(null);
					this.HandleMargin();
					this.VerifyModelFormat();
					this.HandlePrinter(null);
				}
			}
			bool flag4 = this.Model.ModelType != "N";
			if (flag4)
			{
				base.ScribanContext.SetGlobalValues(new object[]
				{
					base.ModelFormat,
					base.ModelMargin,
					this.UserInfo.User,
					base.Printer
				});
			}
			this.CheckOtherDensity();
			base.TemplateBody = "{{}}";
		}

		// Token: 0x06000460 RID: 1120 RVA: 0x00018F40 File Offset: 0x00017140
		private void CheckOtherDensity()
		{
			bool flag = !this.IsDesignMode && this.Model.PrintOnOtherDensity == "FA" && !base.IsSameDensity;
			if (flag)
			{
				throw this.GetException("Model '{0}' is not allowed to print on a different printer density", new object[]
				{
					this.Model.Description
				});
			}
		}

		// Token: 0x06000461 RID: 1121 RVA: 0x00018FA0 File Offset: 0x000171A0
		private AcuLabelContext(ALPrintLog printLog, string rendered, ContentFormat outputFormat = ContentFormat.PNG, Guid? printerID = null) : this()
		{
			base.FinalOutputFormat = outputFormat;
			base.IsRender = true;
			base.FirstTemplate = rendered;
			base.Row = printLog;
			this.HandleGraph(null, typeof(ALPrintLogMaint));
			this.PrintLog = printLog;
			this.HandleModel(printLog.ModelID, null);
			this.HandleFormat(printLog);
			this.HandleMargin();
			Guid? guid = printerID;
			Guid? printerID2 = (guid != null) ? guid : printLog.PrinterID;
			this.HandlePrinter(printerID2);
			guid = printLog.PrinterFormatID;
			Guid? guid2;
			if (guid == null)
			{
				IPrinter printer = base.Printer;
				guid2 = ((printer != null) ? printer.FormatID : null);
			}
			else
			{
				guid2 = guid;
			}
			Guid? parentID = guid2;
			base.PrinterFormat = RuleUtils.FORMAT_FACTORY.GetValueByRules(this, parentID);
			base.ScribanContext.SetGlobalValues(new object[]
			{
				base.ModelFormat,
				base.ModelMargin,
				this.UserInfo.User,
				base.Printer
			});
		}

		// Token: 0x06000462 RID: 1122 RVA: 0x000190A8 File Offset: 0x000172A8
		private AcuLabelContext()
		{
			this.PrintLogGraph = HiddenUtils.CreateInstance<ALPrintLogMaint>();
			this.UserInfo = AcuUserInfo.Create(this.PrintLogGraph);
			this.LabelGraph = HiddenUtils.CreateInstance<ALLabelHandler>();
			this.FileMaintenance = this.PrintLogGraph.FileMaintenance.Value;
		}

		// Token: 0x06000463 RID: 1123 RVA: 0x00019108 File Offset: 0x00017308
		private void HandleGraph(PXGraph rowGraph, Type rowGraphType)
		{
			this.Graph = rowGraph;
			bool flag = this.Graph == null && rowGraphType == null;
			if (flag)
			{
				throw this.GetException("Cannot find a graph to print the label for Model '{0}'", new object[]
				{
					this.Model.Description
				});
			}
			bool flag2 = this.Graph == null;
			if (flag2)
			{
				Type type = GraphHelper.GetType(rowGraphType.FullName);
				bool flag3 = type == null;
				if (flag3)
				{
					throw this.GetException("Cannot find a graph to print the label for Model '{0}'", new object[]
					{
						this.Model.Description
					});
				}
				this.Graph = HiddenUtils.CreateInstance(type);
			}
			ViewUtils.SetDocumentCurrent(this.Graph, base.Row);
		}

		// Token: 0x06000464 RID: 1124 RVA: 0x000191BC File Offset: 0x000173BC
		private void HandleModel(Guid? modelID, TemplateContext context = null)
		{
			bool flag = this.Model == null || this.Model.ModelID != modelID;
			if (flag)
			{
				this.Model = ALModel.PK.Find(this.LabelGraph, modelID);
				bool flag2 = base.Row is IModel && base.Row != this.Model;
				if (flag2)
				{
					base.Row = this.Model;
				}
			}
			ALModel model = this.Model;
			base.MergeDetails = ((model != null) ? model.MergeDetails : null).GetValueOrDefault();
			ALModel model2 = this.Model;
			base.PrintDetails = ((model2 != null) ? model2.PrintDetails : null).GetValueOrDefault();
			ALModel model3 = this.Model;
			string language;
			if ((language = ((model3 != null) ? model3.Language : null)) == null)
			{
				language = (base.GetConfig<string>("DefaultLanguage") ?? "ZPL");
			}
			base.Language = language;
			if (context == null)
			{
				context = ScribanUtils.CreateContext(this.Graph, base.Row, null, base.IsDevMode, new object[]
				{
					this.Model
				});
			}
			context.SetValue(this);
			base.ScribanContext = context;
			base.ScribanContext.TemplateLoader = this;
			this.CheckHasIterator(context);
		}

		// Token: 0x06000465 RID: 1125 RVA: 0x0001933C File Offset: 0x0001753C
		private void CheckHasIterator(TemplateContext context)
		{
			bool flag = this.Model != null && AAConstants.ModelType.IsReal(this.Model.ModelType);
			if (flag)
			{
				IEnumerable<PXResult<ALModelExpr>> source = this.LabelGraph.Expressions.Select(Array.Empty<object>());
				Func<PXResult<ALModelExpr>, bool> predicate;
				if ((predicate = AcuLabelContext.<>O.<0>__CheckHasIterator) == null)
				{
					predicate = (AcuLabelContext.<>O.<0>__CheckHasIterator = new Func<PXResult<ALModelExpr>, bool>(AcuLabelContext.CheckHasIterator));
				}
				bool hasIterator = source.Any(predicate);
				ContextVariables.SetHasIterator(context, hasIterator);
			}
		}

		// Token: 0x06000466 RID: 1126 RVA: 0x000193AC File Offset: 0x000175AC
		private static bool CheckHasIterator(PXResult<ALModelExpr> res)
		{
			ALModelExpr almodelExpr = PXResult.Unwrap<ALModelExpr>(res);
			bool flag = almodelExpr == null;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				ALDataElement aldataElement = PXResult.Unwrap<ALDataElement>(res);
				bool flag2 = ((aldataElement != null) ? aldataElement.ExprType : null) == "E";
				result = flag2;
			}
			return result;
		}

		// Token: 0x06000467 RID: 1127 RVA: 0x000193F4 File Offset: 0x000175F4
		protected override Guid? ChoosePrinter()
		{
			bool isRender = base.IsRender;
			Guid? result;
			if (isRender)
			{
				Guid? guid = this.FindRenderer(base.CurrentFormat, base.FinalOutputFormat);
				this.ValidatePrinter(guid, "You have to configure a Rendering Printer in the Label Basic Preferences", Array.Empty<object>());
				result = guid;
			}
			else
			{
				IPrinterOverride printerOverride = AsgardCoreUtils.FindCacheExtension<IPrinterOverride>(base.Row);
				bool flag = printerOverride != null && !this.IgnoreOverride;
				if (flag)
				{
					Guid? usrALPrinterID = printerOverride.UsrALPrinterID;
					this.ValidatePrinter(usrALPrinterID, "A Model Printer must be defined for you for Model '{0}'", new object[]
					{
						this.Model.Description
					});
					this.UpdateFeatureConsumption(typeof(IPrinterOverride), 1);
					result = usrALPrinterID;
				}
				else
				{
					IModelDestination[] details = ModelPrinters.GetDetails(this.Model.LabelID);
					bool flag2 = !details.Any<IModelDestination>();
					if (flag2)
					{
						bool ignorePrinterMissing = base.IgnorePrinterMissing;
						if (!ignorePrinterMissing)
						{
							throw this.GetException("A Model Printer must be defined for you for Model '{0}'", new object[]
							{
								this.Model.Description
							});
						}
						this.WriteWarning("The automated Label Printing was ignored because no printer is available for model '{0}' ", new object[]
						{
							this.Model.Description
						});
						result = null;
					}
					else
					{
						result = BasicLabelUtils.ChoosePrinter<IModelDestination>(this.LabelGraph, this.UserInfo, details, base.Row);
					}
				}
			}
			return result;
		}

		// Token: 0x06000468 RID: 1128 RVA: 0x0001953C File Offset: 0x0001773C
		private Guid? FindRenderer(ContentFormat from, ContentFormat to)
		{
			bool isRender = base.IsRender;
			if (isRender)
			{
				bool flag = from == ContentFormat.Unknown && this.Model.Language == "PDF";
				if (flag)
				{
					from = ContentFormat.PDF;
				}
				else
				{
					bool flag2 = from == ContentFormat.Unknown;
					if (flag2)
					{
						from = ContentFormat.ZPL;
					}
				}
				to = ContentFormat.PNG;
			}
			IEnumerable<SetupRenderers.Renderer> source = SetupRenderers.FindRenderers(from, to);
			SetupRenderers.Renderer renderer = source.FirstOrDefault<SetupRenderers.Renderer>();
			return (renderer != null) ? renderer.RenderingPrinterID : null;
		}

		// Token: 0x06000469 RID: 1129 RVA: 0x000195B8 File Offset: 0x000177B8
		public override string Load(TemplateContext context, SourceSpan callerSpan, string snippetName)
		{
			ALModel almodel = PXSelectBase<ALModel, PXSelect<ALModel, Where<ALModel.name, Equal<Required<ALModel.name>>>>.Config>.Select(this.LabelGraph, new object[]
			{
				snippetName
			});
			bool flag = almodel != null;
			if (flag)
			{
				LayoutZpl layout = ContextVariables.GetLayout(base.ScribanContext);
				AcuLabelContext value = context.GetValue(false);
				AcuLabelContext acuLabelContext = new AcuLabelContext(AcuContextVariables.GetRowGraph(context), ContextVariables.GetRow(context), almodel.LabelID, value.IsRender, false);
				ContextVariables.SetLayout(acuLabelContext.ScribanContext, layout);
				string renderedTemplate = acuLabelContext.GetRenderedTemplate();
				return renderedTemplate.Replace(ZplCmd.START.Raw, "").Replace(ZplCmd.END.Raw, "");
			}
			throw this.GetException("A Snippet by the name of '{0}' cannot be found", new object[]
			{
				snippetName
			});
		}

		// Token: 0x0600046A RID: 1130 RVA: 0x00019680 File Offset: 0x00017880
		public override RenderResult RenderAsOutput()
		{
			RenderResult renderResult = base.RenderAsOutput();
			ISet<string> warnings = renderResult.Warnings;
			bool isDevMode = base.IsDevMode;
			if (isDevMode)
			{
				this.Model.Message = (warnings.Any<string>() ? string.Join("\n", warnings) : null);
				this.Model = this.LabelGraph.Model.Update(this.Model);
				this.LabelGraph.Actions.PressSave();
			}
			return renderResult;
		}

		// Token: 0x0600046B RID: 1131 RVA: 0x00019700 File Offset: 0x00017900
		public override FileResult RenderAndSaveAsUrl(IPrintLog log = null)
		{
			FileResult result;
			try
			{
				FileResult fileResult = base.RenderAndSaveAsUrl(log);
				result = fileResult;
			}
			catch (Exception ex)
			{
				bool isDevMode = base.IsDevMode;
				if (isDevMode)
				{
					this.Model.Message = AsgardUtils.ExtractMessage(ex);
					this.Model = this.LabelGraph.Model.Update(this.Model);
					this.LabelGraph.Actions.PressSave();
				}
				throw ex;
			}
			return result;
		}

		// Token: 0x0600046C RID: 1132 RVA: 0x0001977C File Offset: 0x0001797C
		public override void SaveRendered(string rendered)
		{
			bool flag = !base.IsSaveRendered;
			if (!flag)
			{
				try
				{
					bool flag2 = string.IsNullOrEmpty(rendered);
					if (flag2)
					{
						this.Model.Rendered = null;
						this._renderingStep = 0;
					}
					else
					{
						this._renderingStep++;
						string name = this.Graph.GetType().Name;
						PXCache pxcache = this.Graph.Caches[typeof(ALFormat)];
						string format = "---- Step:{0}, Model:{1}, Format:{2} (W:{3},H:{4} {5}, {6} {7}), User:{8}, Graph:{9}, Time:{10:yyyy-MM-ddTHH:mm:ss}";
						object[] array = new object[11];
						array[0] = this._renderingStep;
						array[1] = this.Model.Name;
						array[2] = base.ModelFormat.Name;
						array[3] = base.ModelFormat.Width;
						array[4] = base.ModelFormat.Height;
						array[5] = base.ModelFormat.SizeUnit;
						array[6] = base.ModelFormat.PrintDensity;
						array[7] = base.ModelFormat.PrintDensityType;
						int num = 8;
						string fullUserName = PXAccess.GetFullUserName();
						array[num] = ((fullUserName != null) ? fullUserName.Trim() : null);
						array[9] = name;
						array[10] = DateTime.Now;
						string str = string.Format(format, array);
						bool flag3 = base.ModelMargin != null;
						if (flag3)
						{
							str += string.Format(", Margin:{0} (L:{1},R:{2},T:{3},B:{4} {5})", new object[]
							{
								base.ModelMargin.Name,
								base.ModelMargin.Left,
								base.ModelMargin.Right,
								base.ModelMargin.Top,
								base.ModelMargin.Bottom,
								base.ModelMargin.SizeUnit
							});
						}
						object row = base.Row;
						bool flag4 = !this.IsDesignMode;
						if (flag4)
						{
							string text = AsgardUtils.StringifyResult(this.Graph, row);
							bool flag5 = this.DetailRow != null;
							if (flag5)
							{
								object detailRow = this.DetailRow;
								int detailRowNbr = this.DetailRowNbr;
								string arg = AsgardUtils.StringifyResult(this.Graph, detailRow);
								str += string.Format(", Row:{0}, Detail:{1}, Row Nbr:{2}", text, arg, detailRowNbr);
							}
							else
							{
								str = str + ", Row:" + text;
							}
						}
						bool flag6 = base.Printer != null;
						if (flag6)
						{
							str = str + ", Printer:" + base.Printer.Name;
						}
						str += " ----";
						string text2 = rendered;
						bool config = base.GetConfig<bool>("AddLineNumber");
						if (config)
						{
							text2 = BasicHelper.AddLineNumbers(text2, 4);
						}
						string str2 = str + "\n" + text2 + "\n";
						ALModel model = this.Model;
						model.Rendered += str2;
					}
					this.Model = this.LabelGraph.Model.Update(this.Model);
					this.LabelGraph.Actions.PressSave();
				}
				catch (Exception e)
				{
					this.WriteError(e);
				}
			}
		}

		// Token: 0x0600046D RID: 1133 RVA: 0x00019ABC File Offset: 0x00017CBC
		public override void SaveFileInfo(IFileInfo fileInfo, string fieldName, bool saveAsUrl = false)
		{
			bool isDesignMode = this.IsDesignMode;
			object obj;
			PXCache pxcache;
			if (isDesignMode)
			{
				obj = this.Model;
				pxcache = this.LabelGraph.Model.Cache;
			}
			else
			{
				obj = this.LabelRow;
				pxcache = this.Graph.Caches[obj.GetType()];
			}
			IImageStore imageStore = obj as IImageStore;
			bool flag = fieldName == null && imageStore != null;
			if (flag)
			{
				fieldName = "ImageUrl";
			}
			Type type = obj.GetType();
			bool flag2 = !pxcache.Fields.Contains(fieldName);
			if (flag2)
			{
				throw this.GetException("No field named '{0}' in cache '{1}'", new object[]
				{
					fieldName,
					type.Name
				});
			}
			pxcache.AllowUpdate = true;
			string text;
			if (saveAsUrl)
			{
				text = PXUrl.SiteUrlWithPath() + "/Frames/GetFile.ashx?fileID=" + fileInfo.UID.Value;
			}
			else
			{
				text = fileInfo.Name;
			}
			bool flag3 = fieldName != null;
			if (flag3)
			{
				pxcache.SetValueExt(obj, fieldName, text);
				pxcache.Update(obj);
			}
			pxcache.Graph.Persist();
		}

		// Token: 0x0600046E RID: 1134 RVA: 0x00019BDC File Offset: 0x00017DDC
		public override FileResult SaveFileToPrintLog(FileResult printResult, string prefix = null)
		{
			FileResult currentResult;
			using (PXTransactionScope pxtransactionScope = new PXTransactionScope())
			{
				PXTransactionScope.SetSuppressWorkflow(true);
				base.CurrentResult = printResult;
				base.FinalOutputFormat = (ContentFormat)((base.Printer != null) ? base.Printer.ContentType.Value : ((int)printResult.Format));
				base.CurrentResult = this.DoSavePrintLog(printResult, prefix);
				this.PrintLog = (ALPrintLog)base.CurrentResult.Log;
				IFormat modelFormat = base.ModelFormat;
				bool flag = AAConstants.Rotation.HasRotation((modelFormat != null) ? modelFormat.Rotation : null);
				bool flag2 = !base.IsSameDensity;
				if (flag2)
				{
					string printOnOtherDensity = this.Model.PrintOnOtherDensity;
					string a = printOnOtherDensity;
					if (!(a == "NG") && !(a == "NA"))
					{
						if (a == "DF" || a == "DA")
						{
							base.FinalOutputFormat = ContentFormat.PDF;
							ITransformer instance = ZplToPdf.INSTANCE;
							FileResult printResult2 = instance.Transform(this, base.CurrentResult);
							base.CurrentResult = this.DoSavePrintLog(printResult2, instance.GetType().Name);
						}
					}
					else
					{
						base.FinalOutputFormat = ContentFormat.PNG;
						ITransformer instance = ZplToPng.INSTANCE;
						FileResult printResult3 = instance.Transform(this, base.CurrentResult);
						base.CurrentResult = this.DoSavePrintLog(printResult3, instance.GetType().Name);
						base.FinalOutputFormat = ContentFormat.PDF;
						instance = PngToPdf.INSTANCE;
						FileResult printResult4 = instance.Transform(this, base.CurrentResult);
						base.CurrentResult = this.DoSavePrintLog(printResult4, instance.GetType().Name);
					}
				}
				bool flag3 = flag && base.CurrentResult.Format == ContentFormat.ZPL && base.FinalOutputFormat == ContentFormat.ZPL;
				if (flag3)
				{
					ITransformer instance = ZplToGraphicToZpl.INSTANCE;
					FileResult printResult5 = instance.Transform(this, base.CurrentResult);
					base.CurrentResult = this.DoSavePrintLog(printResult5, instance.GetType().Name);
				}
				bool flag4 = base.CurrentResult.Format == ContentFormat.ZPL && base.FinalOutputFormat == ContentFormat.PNG;
				if (flag4)
				{
					FileResult fileResult = ZplToPng.INSTANCE.Transform(this, base.CurrentResult);
					bool flag5 = flag;
					if (flag5)
					{
						IFormat modelFormat2 = base.ModelFormat;
						fileResult = ImageRotation.RotatePrintResult((modelFormat2 != null) ? modelFormat2.Rotation : null, fileResult);
					}
					ITransformer instance = ZplToPng.INSTANCE;
					base.CurrentResult = this.DoSavePrintLog(fileResult, instance.GetType().Name);
				}
				bool flag6 = base.CurrentResult.Format == ContentFormat.ZPL && base.FinalOutputFormat == ContentFormat.PDF;
				if (flag6)
				{
					ITransformer instance = ZplToPdf.INSTANCE;
					FileResult printResult6 = instance.Transform(this, base.CurrentResult);
					base.CurrentResult = this.DoSavePrintLog(printResult6, instance.GetType().Name);
				}
				bool flag7 = base.CurrentResult.Format == ContentFormat.ZPL && base.FinalOutputFormat == ContentFormat.SBPL;
				if (flag7)
				{
					ITransformer instance = ZplToSbpl.INSTANCE;
					FileResult printResult7 = instance.Transform(this, base.CurrentResult);
					base.CurrentResult = this.DoSavePrintLog(printResult7, instance.GetType().Name);
				}
				bool flag8 = base.CurrentResult.Format == ContentFormat.PNG && base.FinalOutputFormat == ContentFormat.ZPL;
				if (flag8)
				{
					ITransformer instance = PngToZpl.INSTANCE;
					FileResult printResult8 = instance.Transform(this, base.CurrentResult);
					base.CurrentResult = this.DoSavePrintLog(printResult8, instance.GetType().Name);
				}
				pxtransactionScope.Complete();
				currentResult = base.CurrentResult;
			}
			return currentResult;
		}

		// Token: 0x0600046F RID: 1135 RVA: 0x00019F7C File Offset: 0x0001817C
		protected override FileResult DoSavePrintLog(FileResult printResult, string prefix = null)
		{
			PXCache cache = this.PrintLogGraph.Document.Cache;
			ContentFormat format = printResult.Format;
			bool flag = this.PrintLog != null;
			IAcuPrintLog acuPrintLog;
			string text;
			if (flag)
			{
				acuPrintLog = this.PrintLog;
				text = printResult.FullName;
				int finalOutputFormat = (int)base.FinalOutputFormat;
				int? contentType = acuPrintLog.ContentType;
				bool flag2;
				if (this.Graph.GetType() != this.PrintLogGraph.GetType())
				{
					int num = finalOutputFormat;
					int? num2 = contentType;
					flag2 = !(num == num2.GetValueOrDefault() & num2 != null);
				}
				else
				{
					flag2 = false;
				}
				bool flag3 = flag2;
				if (flag3)
				{
					acuPrintLog.ContentType = new int?(finalOutputFormat);
					acuPrintLog = this.PrintLogGraph.Document.Update((ALPrintLog)acuPrintLog);
				}
			}
			else
			{
				ALPrintLog alprintLog = new ALPrintLog();
				alprintLog.ModelID = this.Model.LabelID;
				IFormat modelFormat = base.ModelFormat;
				alprintLog.ModelFormatID = ((modelFormat != null) ? modelFormat.ID : null);
				IMargin modelMargin = base.ModelMargin;
				alprintLog.ModelMarginID = ((modelMargin != null) ? modelMargin.ID : null);
				string screenID;
				if ((screenID = this.Model.ScreenID) == null)
				{
					screenID = (PXContext.GetScreenID() ?? "AL201000");
				}
				alprintLog.ScreenID = screenID;
				IFormat printerFormat = base.PrinterFormat;
				alprintLog.PrinterFormatID = ((printerFormat != null) ? printerFormat.ID : null);
				IMargin printerMargin = base.PrinterMargin;
				alprintLog.PrinterMarginID = ((printerMargin != null) ? printerMargin.ID : null);
				IPrinter printer = base.Printer;
				alprintLog.PrinterID = ((printer != null) ? printer.ID : null);
				alprintLog.PrintStationID = this.UserInfo.UserPrintStationID;
				alprintLog.OwnerID = this.UserInfo.OwnerID;
				alprintLog.UserID = new Guid?(this.UserInfo.UserID);
				alprintLog.BAccountID = base.BAccountID;
				alprintLog.LabelKey = AsgardUtils.GetKeys(this.Graph, this.LabelRow, "/") + AcuLabelContext.IteratorKey(this);
				alprintLog.NbCopies = new int?(printResult.NbCopies);
				alprintLog.RefNoteID = this.GetRefNoteID();
				alprintLog.InventoryID = this.GetInventoryID();
				alprintLog.LotSerialNbr = this.GetLotSerialNbr();
				alprintLog.ContentType = new int?((int)base.FinalOutputFormat);
				acuPrintLog = alprintLog;
				acuPrintLog = this.PrintLogGraph.Document.Insert((ALPrintLog)acuPrintLog);
				this.PrintLogGraph.Persist();
				text = this.GetLabelFilename(this.PrintLogGraph, acuPrintLog, new ContentFormat?(format));
				acuPrintLog.LabelFilename = text;
				acuPrintLog = this.PrintLogGraph.Document.Update((ALPrintLog)acuPrintLog);
				string fullName = CustomizedTypeManager.GetTypeNotCustomized(this.Graph).FullName;
				FileUtils.UpdateNoteRecord(this.PrintLogGraph, (ALPrintLog)acuPrintLog, fullName, acuPrintLog.RecordID);
			}
			text = BasicHelper.RemoveIllegalFileNameCharacters(text);
			bool flag4 = prefix == null;
			if (flag4)
			{
				ContentFormat contentFormat = format;
				ContentFormat contentFormat2 = contentFormat;
				if (contentFormat2 != ContentFormat.PNG)
				{
					prefix = "Asgard Label";
				}
				else
				{
					prefix = "Rendered";
				}
			}
			FileInfo labelFile = FileUtils.SaveFileToRow(this.FileMaintenance, printResult.BinData, text, cache, acuPrintLog, prefix);
			this.PrintLogGraph.Persist();
			IFileInfo file = AsgardUtils.ToIFileInfo(labelFile);
			return new FileResult(file, acuPrintLog, new int?(printResult.NbCopies));
		}

		// Token: 0x06000470 RID: 1136 RVA: 0x0001A2E8 File Offset: 0x000184E8
		private static string IteratorKey(ILabelContext labelContext)
		{
			TemplateContext scribanContext = labelContext.ScribanContext;
			bool flag = !AcuContextVariables.HasIteratorByPage(scribanContext);
			string result;
			if (flag)
			{
				result = string.Empty;
			}
			else
			{
				int iteratorPageNbr = ContextVariables.GetIteratorPageNbr(scribanContext);
				result = string.Format("/P{0}", iteratorPageNbr);
			}
			return result;
		}

		// Token: 0x06000471 RID: 1137 RVA: 0x0001A330 File Offset: 0x00018530
		private string GetLotSerialNbr()
		{
			try
			{
				bool isGI = this.IsGI;
				if (!isGI)
				{
					PXCache pxcache = this.Graph.Caches[this.LabelRow.GetType()];
					return (string)pxcache.GetValue(this.LabelRow, typeof(INLotSerialStatus.lotSerialNbr).Name);
				}
			}
			catch
			{
			}
			return null;
		}

		// Token: 0x06000472 RID: 1138 RVA: 0x0001A3AC File Offset: 0x000185AC
		private int? GetInventoryID()
		{
			try
			{
				int? result = null;
				bool isGI = this.IsGI;
				if (!isGI)
				{
					PXCache pxcache = this.Graph.Caches[this.LabelRow.GetType()];
					result = (int?)pxcache.GetValue(this.LabelRow, typeof(InventoryItem.inventoryID).Name);
					bool flag = result == null;
					if (flag)
					{
						IEnumerable<BaseInventoryAttribute> attributesOfType = pxcache.GetAttributesOfType<BaseInventoryAttribute>(this.LabelRow, null);
						BaseInventoryAttribute baseInventoryAttribute = attributesOfType.FirstOrDefault<BaseInventoryAttribute>();
						string text = (baseInventoryAttribute != null) ? baseInventoryAttribute.FieldName : null;
						bool flag2 = text != null;
						if (flag2)
						{
							result = (int?)pxcache.GetValue(this.LabelRow, text);
						}
					}
				}
				return result;
			}
			catch
			{
			}
			return null;
		}

		// Token: 0x06000473 RID: 1139 RVA: 0x0001A494 File Offset: 0x00018694
		private void UpdatePrintLog(FileResult printResult)
		{
			ALPrintLog alprintLog = (ALPrintLog)printResult.Log;
			object remoteResult = printResult.RemoteResult;
			IPrintNodePrintJob printNodePrintJob;
			bool flag;
			if (alprintLog != null)
			{
				printNodePrintJob = (remoteResult as IPrintNodePrintJob);
				flag = (printNodePrintJob != null);
			}
			else
			{
				flag = false;
			}
			bool flag2 = flag;
			if (flag2)
			{
				ALPrintJob alprintJob = new ALPrintJob
				{
					PrintJobID = new long?(printNodePrintJob.ID),
					PrintNodePrinterID = printNodePrintJob.PrinterID,
					PrintNodeComputerID = printNodePrintJob.ComputerID,
					PrintLogID = alprintLog.RecordID,
					Title = printNodePrintJob.Title,
					Source = printNodePrintJob.Source,
					ReceivedAt = new DateTime?(printNodePrintJob.CreateTimestamp)
				};
				this.InsertPrintJob(alprintJob, alprintLog);
				ALPrintLog alprintLog2 = alprintLog;
				int? printLogID = alprintJob.PrintLogID;
				alprintLog2.PrintJobID = ((printLogID != null) ? new long?((long)printLogID.GetValueOrDefault()) : null);
				this.PrintLogGraph.Document.Update(alprintLog);
				this.PrintLogGraph.Persist();
			}
		}

		// Token: 0x06000474 RID: 1140 RVA: 0x0001A598 File Offset: 0x00018798
		private string GetLabelFilename(PXGraph graph, object row, ContentFormat? useFormat = null)
		{
			string defaultFilename = BasicLabelUtils.GetDefaultFilename(graph, row, this.Model);
			string str = BasicHelper.AsExtension(useFormat ?? base.FinalOutputFormat);
			string expr = defaultFilename.Trim() + "." + str;
			return BasicHelper.SurroundBy(expr, "AL-label-", null);
		}

		// Token: 0x06000475 RID: 1141 RVA: 0x0001A5FC File Offset: 0x000187FC
		private Guid? GetRefNoteID()
		{
			Guid? result;
			try
			{
				Guid? refNoteIDSilent = this.GetRefNoteIDSilent(this.LabelRow);
				bool flag = refNoteIDSilent == null;
				if (flag)
				{
					refNoteIDSilent = this.GetRefNoteIDSilent(base.Row);
				}
				result = new Guid?(refNoteIDSilent.Value);
			}
			catch
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06000476 RID: 1142 RVA: 0x0001A664 File Offset: 0x00018864
		private Guid? GetRefNoteIDSilent(object row)
		{
			bool flag = row == null;
			Guid? result;
			if (flag)
			{
				result = null;
			}
			else
			{
				try
				{
					Type type = row.GetType();
					PXCache pxcache = this.Graph.Caches[type];
					Guid? noteIDIfExists = PXNoteAttribute.GetNoteIDIfExists(pxcache, row);
					result = noteIDIfExists;
				}
				catch
				{
					result = null;
				}
			}
			return result;
		}

		// Token: 0x06000477 RID: 1143 RVA: 0x0001A6D4 File Offset: 0x000188D4
		protected override void FindNextSerial()
		{
			string numberingID = this.Model.NumberingID;
			bool flag = string.IsNullOrEmpty(numberingID);
			if (flag)
			{
				throw this.GetException("Model '{0}' requires a Numbering Sequence", new object[]
				{
					this.Model.Name
				});
			}
			NumberingSequence numberingSequence = AutoNumberAttribute.GetNumberingSequence(numberingID, null, new DateTime?(DateTime.Now));
			bool flag2 = numberingSequence == null;
			if (flag2)
			{
				throw this.GetException("A Sequence was not found for Numbering '{0}' and Model '{1}'", new object[]
				{
					this.Model.NumberingID,
					this.Model.Name
				});
			}
			int nbCopies = this.GetNbCopies();
			string lastNbr = numberingSequence.LastNbr;
			int? numberingSEQ = numberingSequence.NumberingSEQ;
			int num = nbCopies * numberingSequence.NbrStep.GetValueOrDefault(1);
			string text = AutoNumberAttribute.NextNumber(lastNbr, num);
			bool flag3 = !BasicHelper.IsNumber(text);
			if (flag3)
			{
				throw this.GetException("Only a numeric Sequence is supported for Numbering '{0}' and Model '{1}'", new object[]
				{
					numberingID,
					this.Model.Name
				});
			}
			bool flag4 = text.CompareTo(numberingSequence.EndNbr) >= 0;
			if (flag4)
			{
				throw this.GetException("Cannot generate the next number for the {0} sequence because it is expired.", new object[]
				{
					numberingID
				});
			}
			this._nextSerial = new int?(int.Parse(text));
			int? nextSerial = this._nextSerial;
			int num2 = num;
			this._lastSerial = ((nextSerial != null) ? new int?(nextSerial.GetValueOrDefault() + num2 - 1) : null);
			DateTime now = DateTime.Now;
			Guid userID = PXAccess.GetUserID();
			bool flag5 = lastNbr == numberingSequence.StartNbr;
			if (flag5)
			{
				PXDatabase.Update<NumberingSequence>(new PXDataFieldParam[]
				{
					new PXDataFieldAssign<NumberingSequence.lastNbr>(text),
					new PXDataFieldAssign<NumberingSequence.createdDateTime>(4, now),
					new PXDataFieldAssign<NumberingSequence.lastModifiedDateTime>(4, now),
					new PXDataFieldAssign<NumberingSequence.createdByID>(14, userID),
					new PXDataFieldAssign<NumberingSequence.lastModifiedByID>(14, userID),
					new PXDataFieldRestrict<NumberingSequence.numberingID>(numberingID),
					new PXDataFieldRestrict<NumberingSequence.numberingSEQ>(numberingSEQ),
					PXDataFieldRestrict.OperationSwitchAllowed
				});
			}
			else
			{
				bool flag6 = !PXDatabase.Update<NumberingSequence>(new PXDataFieldParam[]
				{
					new PXDataFieldAssign<NumberingSequence.lastNbr>(text),
					new PXDataFieldAssign<NumberingSequence.createdDateTime>(4, now),
					new PXDataFieldAssign<NumberingSequence.lastModifiedDateTime>(4, now),
					new PXDataFieldAssign<NumberingSequence.createdByID>(14, userID),
					new PXDataFieldAssign<NumberingSequence.lastModifiedByID>(14, userID),
					new PXDataFieldRestrict<NumberingSequence.numberingID>(numberingID),
					new PXDataFieldRestrict<NumberingSequence.numberingSEQ>(numberingSEQ),
					new PXDataFieldRestrict<NumberingSequence.lastNbr>(lastNbr),
					PXDataFieldRestrict.OperationSwitchAllowed
				});
				if (flag6)
				{
					PXDatabase.Update<NumberingSequence>(new PXDataFieldParam[]
					{
						new PXDataFieldAssign<NumberingSequence.nbrStep>(numberingSequence.NbrStep),
						new PXDataFieldAssign<NumberingSequence.createdDateTime>(4, now),
						new PXDataFieldAssign<NumberingSequence.lastModifiedDateTime>(4, now),
						new PXDataFieldAssign<NumberingSequence.createdByID>(14, userID),
						new PXDataFieldAssign<NumberingSequence.lastModifiedByID>(14, userID),
						new PXDataFieldRestrict<NumberingSequence.numberingID>(numberingID),
						new PXDataFieldRestrict<NumberingSequence.numberingSEQ>(numberingSEQ)
					});
					using (PXDataRecord pxdataRecord = PXDatabase.SelectSingle<NumberingSequence>(new PXDataField[]
					{
						new PXDataField<NumberingSequence.lastNbr>(),
						new PXDataFieldValue<NumberingSequence.numberingID>(numberingID),
						new PXDataFieldValue<NumberingSequence.numberingSEQ>(numberingSEQ)
					}))
					{
						bool flag7 = pxdataRecord != null;
						if (flag7)
						{
							string @string = pxdataRecord.GetString(0);
							int num3 = nbCopies * numberingSequence.NbrStep.GetValueOrDefault(1);
							string text2 = AutoNumberAttribute.NextNumber(@string, num3);
							bool flag8 = text2.CompareTo(numberingSequence.EndNbr) >= 0;
							if (flag8)
							{
								throw this.GetException("Cannot generate the next number for the {0} sequence because it is expired.", new object[]
								{
									numberingID
								});
							}
						}
					}
					PXDatabase.Update<NumberingSequence>(new PXDataFieldParam[]
					{
						new PXDataFieldAssign<NumberingSequence.lastNbr>(text),
						new PXDataFieldAssign<NumberingSequence.createdDateTime>(4, now),
						new PXDataFieldAssign<NumberingSequence.lastModifiedDateTime>(4, now),
						new PXDataFieldAssign<NumberingSequence.createdByID>(14, userID),
						new PXDataFieldAssign<NumberingSequence.lastModifiedByID>(14, userID),
						new PXDataFieldRestrict<NumberingSequence.numberingID>(numberingID),
						new PXDataFieldRestrict<NumberingSequence.numberingSEQ>(numberingSEQ)
					});
				}
			}
		}

		// Token: 0x06000478 RID: 1144 RVA: 0x0001AB20 File Offset: 0x00018D20
		protected override int? GetNbCopiesOverride()
		{
			ILabelOption labelOption = AsgardCoreUtils.FindCacheExtension<ILabelOption>(this.DetailRow);
			bool flag = labelOption == null;
			if (flag)
			{
				labelOption = AsgardCoreUtils.FindCacheExtension<ILabelOption>(base.Row);
			}
			bool flag2 = labelOption != null;
			if (flag2)
			{
				this.UpdateFeatureConsumption(typeof(ILabelOption), 1);
			}
			return (labelOption != null) ? labelOption.UsrALNbrOfCopies : null;
		}

		// Token: 0x06000479 RID: 1145 RVA: 0x0001AB88 File Offset: 0x00018D88
		protected override int? GetDealingCountOverride()
		{
			return null;
		}

		// Token: 0x0600047A RID: 1146 RVA: 0x0001ABA3 File Offset: 0x00018DA3
		internal void Check()
		{
		}

		// Token: 0x0600047B RID: 1147 RVA: 0x0001ABA6 File Offset: 0x00018DA6
		internal void UpdateFeatureConsumption(Type type, int nbLabels = 1)
		{
			this.LicenseManager.UpdateFeatureConsumption(type, nbLabels);
		}

		// Token: 0x0600047C RID: 1148 RVA: 0x0001ABB7 File Offset: 0x00018DB7
		internal void CheckFeatureConsumption(Type type, int nextQty)
		{
			this.LicenseManager.CheckFeatureConsumption(type, nextQty);
		}

		// Token: 0x0600047D RID: 1149 RVA: 0x0001ABC8 File Offset: 0x00018DC8
		public void InsertPrintJob(ALPrintJob printJob, IPrintLog logRow)
		{
			ALPrintJobMaint alprintJobMaint = HiddenUtils.CreateInstance<ALPrintJobMaint>();
			printJob = alprintJobMaint.Document.Insert(printJob);
			alprintJobMaint.Actions.PressSave();
			long? recordID = printJob.RecordID;
			logRow.PrintJobID = recordID;
			logRow = this.PrintLogGraph.Document.Update((ALPrintLog)logRow);
			this.PrintLogGraph.Actions.PressSave();
		}

		// Token: 0x0600047E RID: 1150 RVA: 0x0001AC30 File Offset: 0x00018E30
		public override Exception GetException(string message, params object[] args)
		{
			return (args == null || args.Length == 0) ? new PXException(message) : new PXException(message, args);
		}

		// Token: 0x0600047F RID: 1151 RVA: 0x0001AC58 File Offset: 0x00018E58
		public override Exception GetException(Exception inner, string message, params object[] args)
		{
			return (args == null || args.Length == 0) ? new PXException(inner, message, Array.Empty<object>()) : new PXException(inner, message, args);
		}

		// Token: 0x06000480 RID: 1152 RVA: 0x0001AC87 File Offset: 0x00018E87
		public override void WriteError(string message, params object[] args)
		{
			PXTrace.WriteError(message, args);
		}

		// Token: 0x06000481 RID: 1153 RVA: 0x0001AC92 File Offset: 0x00018E92
		public override void WriteError(Exception e)
		{
			PXTrace.WriteError(e);
		}

		// Token: 0x06000482 RID: 1154 RVA: 0x0001AC9C File Offset: 0x00018E9C
		public override void WriteInformation(string message, params object[] args)
		{
			PXTrace.WriteInformation(message, args);
		}

		// Token: 0x06000483 RID: 1155 RVA: 0x0001ACA7 File Offset: 0x00018EA7
		public override void WriteInformation(Exception e)
		{
			PXTrace.WriteInformation(e);
		}

		// Token: 0x06000484 RID: 1156 RVA: 0x0001ACB1 File Offset: 0x00018EB1
		public override void WriteVerbose(string message, params object[] args)
		{
			PXTrace.WriteVerbose(message, args);
		}

		// Token: 0x06000485 RID: 1157 RVA: 0x0001ACBC File Offset: 0x00018EBC
		public override void WriteVerbose(Exception e)
		{
			PXTrace.WriteVerbose(e);
		}

		// Token: 0x06000486 RID: 1158 RVA: 0x0001ACC6 File Offset: 0x00018EC6
		public override void WriteWarning(string message, params object[] args)
		{
			PXTrace.WriteWarning(message, args);
		}

		// Token: 0x06000487 RID: 1159 RVA: 0x0001ACD1 File Offset: 0x00018ED1
		public override void WriteWarning(Exception e)
		{
			PXTrace.WriteWarning(e);
		}

		// Token: 0x06000488 RID: 1160 RVA: 0x0001ACDC File Offset: 0x00018EDC
		public override ILabelContext<IPXResultset, ALModel, IAcuPrintLog> CreateIteratorContext(ILabelContext<IPXResultset, ALModel, IAcuPrintLog> parent, Guid? snippetID)
		{
			IAcuLabelContext acuLabelContext = parent as IAcuLabelContext;
			return new AcuLabelContext(acuLabelContext.Graph, null, parent.Row, snippetID, parent.IsRender, parent.ScribanContext, parent, false)
			{
				IsAlwaysPrint = parent.IsAlwaysPrint,
				IsSilent = parent.IsSilent,
				IgnorePrinterMissing = parent.IgnorePrinterMissing,
				Adapter = acuLabelContext.Adapter
			};
		}

		// Token: 0x06000489 RID: 1161 RVA: 0x0001AD4C File Offset: 0x00018F4C
		public override ILabelContext CreateIteratorContext(ILabelContext parent, Guid? snippetID)
		{
			AcuLabelContext parent2 = parent as AcuLabelContext;
			return this.CreateIteratorContext(parent2, snippetID);
		}

		// Token: 0x0600048A RID: 1162 RVA: 0x0001AD70 File Offset: 0x00018F70
		public override bool HandleIteratorRecord(IIteratorContext iteratorContext, IIteratorPage page, ILabelContext snippetLC, List<string> snippets, int recNumber, int colNbr, int rowNbr)
		{
			AcuLabelContext acuLabelContext = snippetLC as AcuLabelContext;
			ICoordinate coordinate = iteratorContext.Coordinate;
			TemplateContext scribanContext = iteratorContext.ScribanContext;
			scribanContext.PushGlobal(new ScriptObject());
			Coordinate offset = new Coordinate(new decimal?(colNbr * iteratorContext.HorizontalOffset), new decimal?(rowNbr * iteratorContext.VerticalOffset));
			iteratorContext.SetPageRowNbr(recNumber);
			PXGraph graph = acuLabelContext.Graph;
			bool result;
			try
			{
				object obj = page.List[recNumber];
				snippetLC.IteratorRow = obj;
				snippetLC.PageRow = obj;
				IBqlTable bqlTable = PXResult.UnwrapMain(obj);
				PXCache pxcache = graph.Caches[bqlTable.GetType()];
				pxcache.Current = bqlTable;
				ICoordinate offset2 = LayoutZpl.AddOffset(coordinate, offset);
				iteratorContext.Layout.Offset = offset2;
				string renderedTemplate = snippetLC.GetRenderedTemplate();
				bool flag = string.IsNullOrEmpty(renderedTemplate);
				if (flag)
				{
					result = false;
				}
				else
				{
					snippets.Add(renderedTemplate);
					result = true;
				}
			}
			finally
			{
				IScriptObject scriptObject = scribanContext.PopGlobal();
			}
			return result;
		}

		// Token: 0x0600048B RID: 1163 RVA: 0x0001AE8C File Offset: 0x0001908C
		public override object GetFileServiceReference(object row)
		{
			Guid? noteID = AcuFunctions.GetNoteID(base.ScribanContext, row);
			return noteID;
		}

		// Token: 0x0600048C RID: 1164 RVA: 0x0001AEB4 File Offset: 0x000190B4
		[return: TupleElementNames(new string[]
		{
			"code",
			"desc"
		})]
		public override ValueTuple<string, string>[] DropDownToTexts(Type CodeType, Type DescType)
		{
			return BasicHelper.FindTuples(CodeType, DescType, SortBy.Description);
		}

		// Token: 0x0600048D RID: 1165 RVA: 0x0001AED0 File Offset: 0x000190D0
		public override string Stringify(object value)
		{
			return (this.Graph != null) ? AsgardUtils.StringifyResult(this.Graph, value) : base.Stringify(value);
		}

		// Token: 0x0600048E RID: 1166 RVA: 0x0001AF00 File Offset: 0x00019100
		public override T GetArgValueAs<T>(IArgHolder argHolder, int argNbr, T defaultValue = default(T))
		{
			bool flag = argHolder == null || this.Graph == null;
			T result;
			if (flag)
			{
				result = defaultValue;
			}
			else
			{
				PXCache cache = this.Graph.Caches[argHolder.GetType()];
				T argValueAs = DataElementUtils.GetArgValueAs<T>(cache, argHolder, argNbr, defaultValue);
				result = argValueAs;
			}
			return result;
		}

		// Token: 0x0600048F RID: 1167 RVA: 0x0001AF4C File Offset: 0x0001914C
		public IFileInfo AttachToRow(IFileInfo fileInfo, AAFileExistsAction existsAction, PXGraph graph, object row)
		{
			return this.Resolve<IAcuFileProvider>(Array.Empty<Parameter>()).AttachToRow(fileInfo, existsAction, graph, row);
		}

		// Token: 0x06000490 RID: 1168 RVA: 0x0001AF75 File Offset: 0x00019175
		protected override void Print(IDestination destination, FileResult printResult)
		{
			printResult = destination.Print(this, printResult);
			this.UpdatePrintLog(printResult);
		}

		// Token: 0x040000CD RID: 205
		private IEnumerable<IModelDetail> _exprs;

		// Token: 0x02000341 RID: 833
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x04000B61 RID: 2913
			public static Func<PXResult<ALModelExpr>, bool> <0>__CheckHasIterator;
		}
	}
}
