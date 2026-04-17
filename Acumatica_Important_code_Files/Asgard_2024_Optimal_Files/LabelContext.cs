using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using AA.Objects.AL.Integration.NbCopies;
using AA.Objects.AL.Integration.PrinterOverride;
using AA.Objects.AL.Language;
using AA.Objects.AL.Language.Zpl;
using AA.Objects.AL.License;
using AA.Objects.AL.Mobile;
using PX.Api;
using PX.Common;
using PX.Data;
using PX.Objects.CR;
using PX.Objects.CS;
using PX.Objects.IN;
using PX.SM;
using Scriban;
using Scriban.Parsing;
using Scriban.Runtime;
using Scriban.Syntax;

namespace AA.Objects.AL
{
	// Token: 0x020001D0 RID: 464
	[DebuggerDisplay("Layout: {Model.ModelType}/{Model.Name} ({Model.Description}), Format: {ModelFormat.Name}, ScreenID:{Model.ScreenID}")]
	public sealed class LabelContext : ITemplateLoader, IRuleEvalContext
	{
		// Token: 0x17000630 RID: 1584
		// (get) Token: 0x06001326 RID: 4902 RVA: 0x00042FFA File Offset: 0x000411FA
		// (set) Token: 0x06001327 RID: 4903 RVA: 0x00043002 File Offset: 0x00041202
		private Type GraphType { get; set; }

		// Token: 0x17000631 RID: 1585
		// (get) Token: 0x06001328 RID: 4904 RVA: 0x0004300B File Offset: 0x0004120B
		private IALLicenseManager LicenseManager
		{
			get
			{
				return this.LabelGraph.LicenseManager;
			}
		}

		// Token: 0x17000632 RID: 1586
		// (get) Token: 0x06001329 RID: 4905 RVA: 0x00043018 File Offset: 0x00041218
		// (set) Token: 0x0600132A RID: 4906 RVA: 0x00043025 File Offset: 0x00041225
		private string FirstTemplate
		{
			get
			{
				return this._bodies.FirstOrDefault<string>();
			}
			set
			{
				this._bodies.Clear();
				this._bodies.Add(value);
			}
		}

		// Token: 0x17000633 RID: 1587
		// (get) Token: 0x0600132B RID: 4907 RVA: 0x00043044 File Offset: 0x00041244
		private int DetailRowNbr
		{
			get
			{
				bool flag = this.SingleRow != null;
				int result;
				if (flag)
				{
					result = 0;
				}
				else
				{
					ResultSetIterator detailRows = ContextVariables.GetDetailRows(this.ScribanContext);
					result = ((detailRows != null) ? detailRows.RowNumber : 0);
				}
				return result;
			}
		}

		// Token: 0x17000634 RID: 1588
		// (get) Token: 0x0600132C RID: 4908 RVA: 0x00043080 File Offset: 0x00041280
		internal ALPrintLogMaint PrintLogGraph { get; } = HiddenUtils.CreateInstance<ALPrintLogMaint>();

		// Token: 0x17000635 RID: 1589
		// (get) Token: 0x0600132D RID: 4909 RVA: 0x00043088 File Offset: 0x00041288
		// (set) Token: 0x0600132E RID: 4910 RVA: 0x00043090 File Offset: 0x00041290
		internal Guid UserID { get; private set; }

		// Token: 0x17000636 RID: 1590
		// (get) Token: 0x0600132F RID: 4911 RVA: 0x00043099 File Offset: 0x00041299
		// (set) Token: 0x06001330 RID: 4912 RVA: 0x000430A1 File Offset: 0x000412A1
		internal Users User { get; private set; }

		// Token: 0x17000637 RID: 1591
		// (get) Token: 0x06001331 RID: 4913 RVA: 0x000430AA File Offset: 0x000412AA
		// (set) Token: 0x06001332 RID: 4914 RVA: 0x000430B2 File Offset: 0x000412B2
		internal Guid? PrintStationID { get; private set; }

		// Token: 0x17000638 RID: 1592
		// (get) Token: 0x06001333 RID: 4915 RVA: 0x000430BB File Offset: 0x000412BB
		// (set) Token: 0x06001334 RID: 4916 RVA: 0x000430C3 File Offset: 0x000412C3
		internal CREmployee Owner { get; private set; }

		// Token: 0x17000639 RID: 1593
		// (get) Token: 0x06001335 RID: 4917 RVA: 0x000430CC File Offset: 0x000412CC
		// (set) Token: 0x06001336 RID: 4918 RVA: 0x000430D4 File Offset: 0x000412D4
		internal int? OwnerID { get; private set; }

		// Token: 0x1700063A RID: 1594
		// (get) Token: 0x06001337 RID: 4919 RVA: 0x000430DD File Offset: 0x000412DD
		// (set) Token: 0x06001338 RID: 4920 RVA: 0x000430E5 File Offset: 0x000412E5
		internal string LanguageCode { get; private set; }

		// Token: 0x1700063B RID: 1595
		// (get) Token: 0x06001339 RID: 4921 RVA: 0x000430EE File Offset: 0x000412EE
		internal bool IsRendered
		{
			get
			{
				return !this.TemplateBody.HasScriban();
			}
		}

		// Token: 0x1700063C RID: 1596
		// (get) Token: 0x0600133A RID: 4922 RVA: 0x000430FE File Offset: 0x000412FE
		internal string PrintOnOtherDensity
		{
			get
			{
				ALModel model = this.Model;
				return ((model != null) ? model.PrintOnOtherDensity : null) ?? "PA";
			}
		}

		// Token: 0x1700063D RID: 1597
		// (get) Token: 0x0600133B RID: 4923 RVA: 0x0004311B File Offset: 0x0004131B
		public bool IsDevMode
		{
			get
			{
				return ALSetupSlot.DevMode;
			}
		}

		// Token: 0x1700063E RID: 1598
		// (get) Token: 0x0600133C RID: 4924 RVA: 0x00043122 File Offset: 0x00041322
		public bool AddComments
		{
			get
			{
				return ALSetupSlot.AddComments;
			}
		}

		// Token: 0x1700063F RID: 1599
		// (get) Token: 0x0600133D RID: 4925 RVA: 0x00043129 File Offset: 0x00041329
		public bool IsSaveRendered
		{
			get
			{
				return ALSetupSlot.SaveRendered;
			}
		}

		// Token: 0x17000640 RID: 1600
		// (get) Token: 0x0600133E RID: 4926 RVA: 0x00043130 File Offset: 0x00041330
		// (set) Token: 0x0600133F RID: 4927 RVA: 0x00043138 File Offset: 0x00041338
		public bool IsRaw { get; private set; }

		// Token: 0x17000641 RID: 1601
		// (get) Token: 0x06001340 RID: 4928 RVA: 0x00043141 File Offset: 0x00041341
		// (set) Token: 0x06001341 RID: 4929 RVA: 0x00043149 File Offset: 0x00041349
		public bool IsSilent { get; set; }

		// Token: 0x17000642 RID: 1602
		// (get) Token: 0x06001342 RID: 4930 RVA: 0x00043152 File Offset: 0x00041352
		// (set) Token: 0x06001343 RID: 4931 RVA: 0x0004315A File Offset: 0x0004135A
		public bool IgnorePrinterMissing { get; set; } = false;

		// Token: 0x17000643 RID: 1603
		// (get) Token: 0x06001344 RID: 4932 RVA: 0x00043163 File Offset: 0x00041363
		// (set) Token: 0x06001345 RID: 4933 RVA: 0x0004316B File Offset: 0x0004136B
		public bool IsAlwaysPrint { get; set; }

		// Token: 0x17000644 RID: 1604
		// (get) Token: 0x06001346 RID: 4934 RVA: 0x00043174 File Offset: 0x00041374
		public bool IsDesignMode
		{
			get
			{
				return this.Graph == null || this.Graph.GetType() == typeof(ALModelMaint);
			}
		}

		// Token: 0x17000645 RID: 1605
		// (get) Token: 0x06001347 RID: 4935 RVA: 0x0004319B File Offset: 0x0004139B
		public bool IsGI
		{
			get
			{
				return AsgardUtils.IsGI(this.Graph);
			}
		}

		// Token: 0x17000646 RID: 1606
		// (get) Token: 0x06001348 RID: 4936 RVA: 0x000431A8 File Offset: 0x000413A8
		public bool DealingMode
		{
			get
			{
				ALModel model = this.Model;
				return model != null && model.DealingMode.GetValueOrDefault();
			}
		}

		// Token: 0x17000647 RID: 1607
		// (get) Token: 0x06001349 RID: 4937 RVA: 0x000431CF File Offset: 0x000413CF
		public bool IsSnippet
		{
			get
			{
				ALModel model = this.Model;
				return ((model != null) ? model.ModelType : null) == "N";
			}
		}

		// Token: 0x17000648 RID: 1608
		// (get) Token: 0x0600134A RID: 4938 RVA: 0x000431ED File Offset: 0x000413ED
		// (set) Token: 0x0600134B RID: 4939 RVA: 0x000431F5 File Offset: 0x000413F5
		public bool IsRender { get; private set; }

		// Token: 0x17000649 RID: 1609
		// (get) Token: 0x0600134C RID: 4940 RVA: 0x000431FE File Offset: 0x000413FE
		public bool IsCreatedFromSession
		{
			get
			{
				PXGraph graph = this.Graph;
				return graph != null && graph.IsCreatedFromSession;
			}
		}

		// Token: 0x1700064A RID: 1610
		// (get) Token: 0x0600134D RID: 4941 RVA: 0x00043212 File Offset: 0x00041412
		public bool IsImport
		{
			get
			{
				PXGraph graph = this.Graph;
				return graph != null && graph.IsImport;
			}
		}

		// Token: 0x1700064B RID: 1611
		// (get) Token: 0x0600134E RID: 4942 RVA: 0x00043226 File Offset: 0x00041426
		public bool IsExport
		{
			get
			{
				PXGraph graph = this.Graph;
				return graph != null && graph.IsExport;
			}
		}

		// Token: 0x1700064C RID: 1612
		// (get) Token: 0x0600134F RID: 4943 RVA: 0x0004323A File Offset: 0x0004143A
		public bool IsMobile
		{
			get
			{
				PXGraph graph = this.Graph;
				return graph != null && graph.IsMobile;
			}
		}

		// Token: 0x1700064D RID: 1613
		// (get) Token: 0x06001350 RID: 4944 RVA: 0x0004324E File Offset: 0x0004144E
		public bool IsContractBasedAPI
		{
			get
			{
				PXGraph graph = this.Graph;
				return graph != null && graph.IsContractBasedAPI;
			}
		}

		// Token: 0x1700064E RID: 1614
		// (get) Token: 0x06001351 RID: 4945 RVA: 0x00043262 File Offset: 0x00041462
		// (set) Token: 0x06001352 RID: 4946 RVA: 0x0004326A File Offset: 0x0004146A
		public object SingleRow { get; set; }

		// Token: 0x1700064F RID: 1615
		// (get) Token: 0x06001353 RID: 4947 RVA: 0x00043273 File Offset: 0x00041473
		// (set) Token: 0x06001354 RID: 4948 RVA: 0x0004327B File Offset: 0x0004147B
		public object Row { get; private set; }

		// Token: 0x17000650 RID: 1616
		// (get) Token: 0x06001355 RID: 4949 RVA: 0x00043284 File Offset: 0x00041484
		public LabelContext Parent { get; }

		// Token: 0x17000651 RID: 1617
		// (get) Token: 0x06001356 RID: 4950 RVA: 0x0004328C File Offset: 0x0004148C
		public ALLabelHandler LabelGraph { get; } = HiddenUtils.CreateInstance<ALLabelHandler>();

		// Token: 0x17000652 RID: 1618
		// (get) Token: 0x06001357 RID: 4951 RVA: 0x00043294 File Offset: 0x00041494
		public UploadFileMaintenance FileMaintenance
		{
			get
			{
				return this.PrintLogGraph.FileMaintenance.Value;
			}
		}

		// Token: 0x17000653 RID: 1619
		// (get) Token: 0x06001358 RID: 4952 RVA: 0x000432A6 File Offset: 0x000414A6
		// (set) Token: 0x06001359 RID: 4953 RVA: 0x000432AE File Offset: 0x000414AE
		public int? BAccountID { get; private set; }

		// Token: 0x17000654 RID: 1620
		// (get) Token: 0x0600135A RID: 4954 RVA: 0x000432B7 File Offset: 0x000414B7
		// (set) Token: 0x0600135B RID: 4955 RVA: 0x000432BF File Offset: 0x000414BF
		public PXAdapter Adapter { get; private set; }

		// Token: 0x17000655 RID: 1621
		// (get) Token: 0x0600135C RID: 4956 RVA: 0x000432C8 File Offset: 0x000414C8
		// (set) Token: 0x0600135D RID: 4957 RVA: 0x000432D0 File Offset: 0x000414D0
		public OutputFormat FinalOutputFormat { get; private set; } = OutputFormat.ZPL;

		// Token: 0x17000656 RID: 1622
		// (get) Token: 0x0600135E RID: 4958 RVA: 0x000432D9 File Offset: 0x000414D9
		// (set) Token: 0x0600135F RID: 4959 RVA: 0x000432E1 File Offset: 0x000414E1
		public IPdfOptions PdfOptions { get; private set; }

		// Token: 0x17000657 RID: 1623
		// (get) Token: 0x06001360 RID: 4960 RVA: 0x000432EA File Offset: 0x000414EA
		// (set) Token: 0x06001361 RID: 4961 RVA: 0x000432F2 File Offset: 0x000414F2
		public IFormat ModelFormat { get; private set; }

		// Token: 0x17000658 RID: 1624
		// (get) Token: 0x06001362 RID: 4962 RVA: 0x000432FB File Offset: 0x000414FB
		// (set) Token: 0x06001363 RID: 4963 RVA: 0x00043303 File Offset: 0x00041503
		public IMargin ModelMargin { get; private set; }

		// Token: 0x17000659 RID: 1625
		// (get) Token: 0x06001364 RID: 4964 RVA: 0x0004330C File Offset: 0x0004150C
		// (set) Token: 0x06001365 RID: 4965 RVA: 0x00043314 File Offset: 0x00041514
		public PXGraph Graph { get; private set; }

		// Token: 0x1700065A RID: 1626
		// (get) Token: 0x06001366 RID: 4966 RVA: 0x0004331D File Offset: 0x0004151D
		// (set) Token: 0x06001367 RID: 4967 RVA: 0x00043325 File Offset: 0x00041525
		public ALPrintLog PrintLog { get; private set; }

		// Token: 0x1700065B RID: 1627
		// (get) Token: 0x06001368 RID: 4968 RVA: 0x0004332E File Offset: 0x0004152E
		// (set) Token: 0x06001369 RID: 4969 RVA: 0x00043336 File Offset: 0x00041536
		public TemplateContext ScribanContext { get; private set; }

		// Token: 0x1700065C RID: 1628
		// (get) Token: 0x0600136A RID: 4970 RVA: 0x0004333F File Offset: 0x0004153F
		// (set) Token: 0x0600136B RID: 4971 RVA: 0x00043347 File Offset: 0x00041547
		public IPrinter Printer { get; private set; }

		// Token: 0x1700065D RID: 1629
		// (get) Token: 0x0600136C RID: 4972 RVA: 0x00043350 File Offset: 0x00041550
		// (set) Token: 0x0600136D RID: 4973 RVA: 0x00043358 File Offset: 0x00041558
		public IFormat PrinterFormat { get; private set; }

		// Token: 0x1700065E RID: 1630
		// (get) Token: 0x0600136E RID: 4974 RVA: 0x00043361 File Offset: 0x00041561
		// (set) Token: 0x0600136F RID: 4975 RVA: 0x00043369 File Offset: 0x00041569
		public IMargin PrinterMargin { get; private set; }

		// Token: 0x1700065F RID: 1631
		// (get) Token: 0x06001370 RID: 4976 RVA: 0x00043372 File Offset: 0x00041572
		public IPrinterLanguage PrinterLanguage
		{
			get
			{
				return LanguageFactory.GetLanguage(this.LanguageCode);
			}
		}

		// Token: 0x17000660 RID: 1632
		// (get) Token: 0x06001371 RID: 4977 RVA: 0x0004337F File Offset: 0x0004157F
		public Lazy<double> DensityRatio
		{
			get
			{
				return new Lazy<double>(() => BasicLabelUtils.GetDensityRatio(this));
			}
		}

		// Token: 0x17000661 RID: 1633
		// (get) Token: 0x06001372 RID: 4978 RVA: 0x00043392 File Offset: 0x00041592
		public Lazy<bool> IsSameDensity
		{
			get
			{
				return new Lazy<bool>(() => BasicLabelUtils.IsSameDensity(this));
			}
		}

		// Token: 0x17000662 RID: 1634
		// (get) Token: 0x06001373 RID: 4979 RVA: 0x000433A5 File Offset: 0x000415A5
		public Lazy<PXResultset<ALModelExpr>> Expressions
		{
			get
			{
				return new Lazy<PXResultset<ALModelExpr>>(() => this.LabelGraph.Expressions.Select(Array.Empty<object>()));
			}
		}

		// Token: 0x17000663 RID: 1635
		// (get) Token: 0x06001374 RID: 4980 RVA: 0x000433B8 File Offset: 0x000415B8
		public Lazy<IEnumerable<IFont>> Fonts
		{
			get
			{
				return new Lazy<IEnumerable<IFont>>(() => BasicLabelUtils.GetFontsWithFileID(this.Expressions.Value));
			}
		}

		// Token: 0x17000664 RID: 1636
		// (get) Token: 0x06001375 RID: 4981 RVA: 0x000433CB File Offset: 0x000415CB
		public Lazy<IEnumerable<IPrinterFileWithData>> Images
		{
			get
			{
				return new Lazy<IEnumerable<IPrinterFileWithData>>(() => BasicLabelUtils.GetImages(this, this.Expressions.Value));
			}
		}

		// Token: 0x17000665 RID: 1637
		// (get) Token: 0x06001376 RID: 4982 RVA: 0x000433DE File Offset: 0x000415DE
		public Lazy<IEnumerable<FontFile>> FontFiles
		{
			get
			{
				return new Lazy<IEnumerable<FontFile>>(() => BasicLabelUtils.GetFontFiles(this));
			}
		}

		// Token: 0x17000666 RID: 1638
		// (get) Token: 0x06001377 RID: 4983 RVA: 0x000433F1 File Offset: 0x000415F1
		// (set) Token: 0x06001378 RID: 4984 RVA: 0x00043403 File Offset: 0x00041603
		public ALModel Model
		{
			get
			{
				return this.LabelGraph.Model.Current;
			}
			private set
			{
				this.LabelGraph.Model.Current = value;
			}
		}

		// Token: 0x17000667 RID: 1639
		// (get) Token: 0x06001379 RID: 4985 RVA: 0x00043418 File Offset: 0x00041618
		public string RenderedTemplate
		{
			get
			{
				this.DoRenderAsLanguage();
				return this._bodies.Last<string>();
			}
		}

		// Token: 0x0600137A RID: 4986 RVA: 0x0004343C File Offset: 0x0004163C
		public string GetRenderedTemplate()
		{
			this.DoRenderAsLanguage();
			return this.TemplateBody;
		}

		// Token: 0x17000668 RID: 1640
		// (get) Token: 0x0600137B RID: 4987 RVA: 0x0004345B File Offset: 0x0004165B
		// (set) Token: 0x0600137C RID: 4988 RVA: 0x00043468 File Offset: 0x00041668
		public string TemplateBody
		{
			get
			{
				return this._bodies.LastOrDefault<string>();
			}
			set
			{
				string item = this.PrinterLanguage.Validate(this, value);
				this._bodies.Add(item);
			}
		}

		// Token: 0x17000669 RID: 1641
		// (get) Token: 0x0600137D RID: 4989 RVA: 0x00043491 File Offset: 0x00041691
		// (set) Token: 0x0600137E RID: 4990 RVA: 0x000434AA File Offset: 0x000416AA
		public IPXResultset DetailRows
		{
			get
			{
				ResultSetIterator detailRows = ContextVariables.GetDetailRows(this.ScribanContext);
				return (detailRows != null) ? detailRows.Rows : null;
			}
			set
			{
				ContextVariables.SetDetailRows(this.ScribanContext, value);
			}
		}

		// Token: 0x1700066A RID: 1642
		// (get) Token: 0x0600137F RID: 4991 RVA: 0x000434BC File Offset: 0x000416BC
		// (set) Token: 0x06001380 RID: 4992 RVA: 0x00043500 File Offset: 0x00041700
		public object DetailRow
		{
			get
			{
				bool flag = this.SingleRow != null;
				object result;
				if (flag)
				{
					result = this.SingleRow;
				}
				else
				{
					ResultSetIterator detailRows = ContextVariables.GetDetailRows(this.ScribanContext);
					result = ((detailRows != null) ? detailRows.Row : null);
				}
				return result;
			}
			set
			{
				ResultSetIterator detailRows = ContextVariables.GetDetailRows(this.ScribanContext);
				detailRows.Row = value;
				this.ResetRenderedBody();
				this.ResetIterator();
				this.PrintLog = null;
				this._nextSerial = null;
				this._lastSerial = null;
			}
		}

		// Token: 0x06001381 RID: 4993 RVA: 0x00043550 File Offset: 0x00041750
		private void ResetIterator()
		{
			this.IteratorRows = null;
			ContextVariables.ResetIterator(this.ScribanContext);
		}

		// Token: 0x1700066B RID: 1643
		// (get) Token: 0x06001382 RID: 4994 RVA: 0x00043568 File Offset: 0x00041768
		// (set) Token: 0x06001383 RID: 4995 RVA: 0x0004358E File Offset: 0x0004178E
		public ResultSetIterator RowIterator
		{
			get
			{
				return this.ScribanContext.GetValue("AL_RowIterator", true, null);
			}
			set
			{
				this.ScribanContext.SetValue("AL_RowIterator", value);
			}
		}

		// Token: 0x1700066C RID: 1644
		// (get) Token: 0x06001384 RID: 4996 RVA: 0x000435A2 File Offset: 0x000417A2
		// (set) Token: 0x06001385 RID: 4997 RVA: 0x000435B8 File Offset: 0x000417B8
		public IPXResultset IteratorRows
		{
			get
			{
				ResultSetIterator rowIterator = this.RowIterator;
				return (rowIterator != null) ? rowIterator.Rows : null;
			}
			set
			{
				this.RowIterator = new ResultSetIterator(this.ScribanContext, value);
				bool flag = value == null || value.GetRowCount() == 0;
				if (flag)
				{
					ContextVariables.SetIteratorPageNbr(this.ScribanContext, 1);
				}
			}
		}

		// Token: 0x1700066D RID: 1645
		// (get) Token: 0x06001386 RID: 4998 RVA: 0x000435FB File Offset: 0x000417FB
		// (set) Token: 0x06001387 RID: 4999 RVA: 0x0004360F File Offset: 0x0004180F
		public object IteratorRow
		{
			get
			{
				ResultSetIterator rowIterator = this.RowIterator;
				return (rowIterator != null) ? rowIterator.Row : null;
			}
			set
			{
				this.RowIterator.Row = value;
				this.ResetRenderedBody();
			}
		}

		// Token: 0x1700066E RID: 1646
		// (get) Token: 0x06001388 RID: 5000 RVA: 0x00043626 File Offset: 0x00041826
		// (set) Token: 0x06001389 RID: 5001 RVA: 0x0004363A File Offset: 0x0004183A
		public IPXResultset PageRows
		{
			get
			{
				ResultSetIterator pageIterator = this.PageIterator;
				return (pageIterator != null) ? pageIterator.Rows : null;
			}
			set
			{
				this.PageIterator = new ResultSetIterator(this.ScribanContext, value);
			}
		}

		// Token: 0x1700066F RID: 1647
		// (get) Token: 0x0600138A RID: 5002 RVA: 0x00043650 File Offset: 0x00041850
		// (set) Token: 0x0600138B RID: 5003 RVA: 0x00043676 File Offset: 0x00041876
		public ResultSetIterator PageIterator
		{
			get
			{
				return this.ScribanContext.GetValue("AL_PageIterator", true, null);
			}
			set
			{
				this.ScribanContext.SetValue("AL_PageIterator", value);
			}
		}

		// Token: 0x17000670 RID: 1648
		// (get) Token: 0x0600138C RID: 5004 RVA: 0x0004368A File Offset: 0x0004188A
		// (set) Token: 0x0600138D RID: 5005 RVA: 0x0004369E File Offset: 0x0004189E
		public object PageRow
		{
			get
			{
				ResultSetIterator pageIterator = this.PageIterator;
				return (pageIterator != null) ? pageIterator.Row : null;
			}
			set
			{
				this.PageIterator.Row = value;
			}
		}

		// Token: 0x17000671 RID: 1649
		// (get) Token: 0x0600138E RID: 5006 RVA: 0x000436B0 File Offset: 0x000418B0
		public bool SendPause
		{
			get
			{
				ALModel model = this.Model;
				int valueOrDefault = ((model != null) ? model.SendPauseEvery : null).GetValueOrDefault();
				int num = this.DetailRowNbr + 1;
				ResultSetIterator detailRows = ContextVariables.GetDetailRows(this.ScribanContext);
				int num2 = (detailRows != null) ? detailRows.RowCount : 0;
				return valueOrDefault > 0 && num % valueOrDefault == 0 && num < num2;
			}
		}

		// Token: 0x17000672 RID: 1650
		// (get) Token: 0x0600138F RID: 5007 RVA: 0x0004371C File Offset: 0x0004191C
		public object LabelRow
		{
			get
			{
				object row = this.Row;
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

		// Token: 0x06001390 RID: 5008 RVA: 0x0004377C File Offset: 0x0004197C
		public static LabelContext CreateSingleRowPrintContext(Type graphType, object row, object labelRow, Guid? modelID, int? bAccountID = null)
		{
			return new LabelContext(graphType, row, modelID, false, false)
			{
				IsSilent = true,
				SingleRow = labelRow,
				IsAlwaysPrint = true,
				BAccountID = bAccountID
			};
		}

		// Token: 0x06001391 RID: 5009 RVA: 0x000437BC File Offset: 0x000419BC
		public static LabelContext CreateSendRawContext(IPrinter printer, string language = "ZPL", string raw = null)
		{
			return new LabelContext(printer, language, raw)
			{
				IsRaw = true
			};
		}

		// Token: 0x06001392 RID: 5010 RVA: 0x000437E0 File Offset: 0x000419E0
		public static LabelContext CreateTestContext(PXGraph rowGraph, object row)
		{
			LabelContext labelContext = new LabelContext(null, null, null);
			TemplateContext templateContext = ScribanUtils.CreateContext(rowGraph, row, null, true, Array.Empty<object>());
			templateContext.SetValue(labelContext);
			labelContext.ScribanContext = templateContext;
			labelContext.ScribanContext.TemplateLoader = labelContext;
			return labelContext;
		}

		// Token: 0x06001393 RID: 5011 RVA: 0x00043828 File Offset: 0x00041A28
		public static LabelContext CreateRenderContext(LabelContext lc, string rendered, OutputFormat outputFormat = OutputFormat.PNG)
		{
			return new LabelContext(null, lc.GraphType ?? lc.Graph.GetType(), lc.Row, lc.Model.LabelID, true, lc.ScribanContext, null, false)
			{
				Adapter = lc.Adapter,
				FinalOutputFormat = outputFormat,
				TemplateBody = rendered
			};
		}

		// Token: 0x06001394 RID: 5012 RVA: 0x00043890 File Offset: 0x00041A90
		public static LabelContext CreateRenderContext(ALPrintLog printLog, string rendered, OutputFormat outputFormat = OutputFormat.PNG)
		{
			return new LabelContext(printLog, rendered, outputFormat, null);
		}

		// Token: 0x06001395 RID: 5013 RVA: 0x000438B8 File Offset: 0x00041AB8
		public static LabelContext CreateReprintContext(ALPrintLog printLog, Guid? printerID = null)
		{
			return new LabelContext(printLog, "", (OutputFormat)printLog.ContentType.Value, printerID);
		}

		// Token: 0x06001396 RID: 5014 RVA: 0x000438E8 File Offset: 0x00041AE8
		public static LabelContext CreateRenderContext(PXGraph graph, object row, Guid? modelID, PXAdapter adapter = null, OutputFormat outputFormat = OutputFormat.PNG)
		{
			return new LabelContext(graph, row, modelID, true, false)
			{
				Adapter = adapter,
				FinalOutputFormat = outputFormat
			};
		}

		// Token: 0x06001397 RID: 5015 RVA: 0x00043918 File Offset: 0x00041B18
		public static LabelContext CreateMobilePrintContext(PXGraph mobileGraph, object row, Guid? modelID, PXAdapter adapter = null)
		{
			string regularGraphFromMobile = MobileUtils.GetRegularGraphFromMobile(mobileGraph);
			Type type = GraphHelper.GetType(regularGraphFromMobile);
			return new LabelContext(type, row, modelID, false, false)
			{
				Adapter = adapter
			};
		}

		// Token: 0x06001398 RID: 5016 RVA: 0x0004394C File Offset: 0x00041B4C
		public static LabelContext CreatePrintContext(Type rowGraphType, object row, Guid? modelID, bool ignorePrinterMissing = false, PXAdapter adapter = null)
		{
			return new LabelContext(rowGraphType, row, modelID, false, ignorePrinterMissing)
			{
				Adapter = adapter
			};
		}

		// Token: 0x06001399 RID: 5017 RVA: 0x00043974 File Offset: 0x00041B74
		public static LabelContext CreateChildContext(LabelContext parent, Guid? labelChildID, bool ignorePrinterMissing = false)
		{
			return new LabelContext(parent.Graph, parent.Row, labelChildID, parent.IsRender, ignorePrinterMissing)
			{
				IsAlwaysPrint = parent.IsAlwaysPrint,
				IsSilent = parent.IsSilent,
				IgnorePrinterMissing = parent.IgnorePrinterMissing,
				Adapter = parent.Adapter
			};
		}

		// Token: 0x0600139A RID: 5018 RVA: 0x000439D8 File Offset: 0x00041BD8
		public static LabelContext CreateIteratorContext(LabelContext parent, Guid? snippetID)
		{
			return new LabelContext(parent.Graph, null, parent.Row, snippetID, parent.IsRender, parent.ScribanContext, parent, false)
			{
				IsAlwaysPrint = parent.IsAlwaysPrint,
				IsSilent = parent.IsSilent,
				IgnorePrinterMissing = parent.IgnorePrinterMissing,
				Adapter = parent.Adapter
			};
		}

		// Token: 0x0600139B RID: 5019 RVA: 0x00043A44 File Offset: 0x00041C44
		private LabelContext(IPrinter printer, string languageCode, string raw)
		{
			this.LanguageCode = languageCode;
			this.TemplateBody = raw;
			this.PrepareUser();
			this.Printer = printer;
		}

		// Token: 0x0600139C RID: 5020 RVA: 0x00043AA9 File Offset: 0x00041CA9
		private LabelContext(Type rowGraphType, object row, Guid? modelID, bool forRender, bool ignorePrinterMissing = false) : this(null, rowGraphType, row, modelID, forRender, ignorePrinterMissing)
		{
		}

		// Token: 0x0600139D RID: 5021 RVA: 0x00043ABB File Offset: 0x00041CBB
		private LabelContext(PXGraph rowGraph, object row, Guid? modelID, bool forRender, bool ignorePrinterMissing = false) : this(rowGraph, null, row, modelID, forRender, ignorePrinterMissing)
		{
		}

		// Token: 0x0600139E RID: 5022 RVA: 0x00043AD0 File Offset: 0x00041CD0
		private LabelContext(PXGraph rowGraph, Type rowGraphType, object row, Guid? modelID, bool forRender, bool ignorePrinterMissing = false) : this(rowGraph, rowGraphType, row, modelID, forRender, null, null, ignorePrinterMissing)
		{
		}

		// Token: 0x0600139F RID: 5023 RVA: 0x00043AF0 File Offset: 0x00041CF0
		private LabelContext(PXGraph rowGraph, Type rowGraphType, object row, Guid? modelID, bool forRender, TemplateContext context, LabelContext parentContext = null, bool ignorePrinterMissing = false)
		{
			this.Parent = parentContext;
			this.IsRender = forRender;
			this.IgnorePrinterMissing = ignorePrinterMissing;
			this.HandleRow(row);
			this.HandleGraph(rowGraph, rowGraphType);
			this.HandleModel(modelID, context);
			bool flag = this.Model == null;
			if (flag)
			{
				throw new PXException("No label found for id '{0}'", new object[]
				{
					modelID
				});
			}
			bool flag2 = row is PXResult;
			if (flag2)
			{
				row = PXResult.UnwrapMain(row);
			}
			int? num = (parentContext != null) ? parentContext.BAccountID : null;
			this.BAccountID = ((num != null) ? num : RuleUtils.GetBAccountID(this.Graph, row));
			this.PrepareUser();
			string modelType = this.Model.ModelType;
			string a = modelType;
			if (!(a == "G"))
			{
				if (!(a == "S"))
				{
					if (a == "N")
					{
						this.ModelFormat = ((parentContext != null) ? parentContext.ModelFormat : null);
						this.ModelMargin = ((parentContext != null) ? parentContext.ModelMargin : null);
						this.Printer = ((parentContext != null) ? parentContext.Printer : null);
						this.PrinterFormat = ((parentContext != null) ? parentContext.PrinterFormat : null);
						this.PrinterMargin = ((parentContext != null) ? parentContext.PrinterMargin : null);
						bool flag3 = this.ModelFormat == null;
						if (flag3)
						{
							IFormat modelFormat;
							Formats.TryGetFormat(ALSetupSlot.DefaultFormatID, out modelFormat);
							this.ModelFormat = modelFormat;
						}
						if (this.ModelFormat == null)
						{
							this.ModelFormat = Formats.Format.DEFAULT_FORMAT;
						}
						bool flag4 = this.ModelMargin == null;
						if (flag4)
						{
							Margins.Margin modelMargin;
							Margins.TryGetMargin(ALSetupSlot.DefaultMarginID, out modelMargin);
							this.ModelMargin = modelMargin;
						}
						bool flag5 = this.Printer == null && forRender;
						if (flag5)
						{
							IPrinter printer;
							Printers.TryGetPrinter(ALSetupSlot.RenderingPrinterID, out printer);
							this.Printer = printer;
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
			bool flag6 = this.Model.ModelType != "N";
			if (flag6)
			{
				this.ScribanContext.SetGlobalValues(new object[]
				{
					this.ModelFormat,
					this.ModelMargin,
					this.User,
					this.Printer
				});
			}
			this.CheckOtherDensity();
			this.TemplateBody = "{{}}";
		}

		// Token: 0x060013A0 RID: 5024 RVA: 0x00043DC4 File Offset: 0x00041FC4
		private void CheckOtherDensity()
		{
			bool flag = !this.IsDesignMode && this.PrintOnOtherDensity == "FA" && !this.IsSameDensity.Value;
			if (flag)
			{
				throw new PXException("Model '{0}' is not allowed to print on a different printer density", new object[]
				{
					this.Model.Description
				});
			}
		}

		// Token: 0x060013A1 RID: 5025 RVA: 0x00043E24 File Offset: 0x00042024
		private LabelContext(ALPrintLog printLog, string rendered, OutputFormat outputFormat = OutputFormat.PNG, Guid? printerID = null)
		{
			this.FinalOutputFormat = outputFormat;
			this.IsRender = true;
			this.FirstTemplate = rendered;
			this.Row = printLog;
			this.HandleGraph(null, typeof(ALPrintLogMaint));
			this.PrintLog = printLog;
			this.HandleModel(printLog.ModelID, null);
			this.HandleFormat(printLog);
			this.HandleMargin();
			this.PrepareUser();
			Guid? guid = printerID;
			Guid? printerID2 = (guid != null) ? guid : printLog.PrinterID;
			this.HandlePrinter(printerID2);
			guid = printLog.PrinterFormatID;
			Guid? guid2;
			if (guid == null)
			{
				IPrinter printer = this.Printer;
				guid2 = ((printer != null) ? printer.FormatID : null);
			}
			else
			{
				guid2 = guid;
			}
			Guid? parentID = guid2;
			this.PrinterFormat = RuleUtils.FORMAT_FACTORY.GetValueByRules(this, parentID);
			this.ScribanContext.SetGlobalValues(new object[]
			{
				this.ModelFormat,
				this.ModelMargin,
				this.User,
				this.Printer
			});
		}

		// Token: 0x060013A2 RID: 5026 RVA: 0x00043F60 File Offset: 0x00042160
		private void ResetRenderedBody()
		{
			string firstTemplate = this.FirstTemplate;
			this.FirstTemplate = firstTemplate;
		}

		// Token: 0x060013A3 RID: 5027 RVA: 0x00043F80 File Offset: 0x00042180
		private void PrepareUser()
		{
			this.UserID = BasicLabelUtils.GetUserID(this.LabelGraph);
			this.User = BasicLabelUtils.GetUser(this.LabelGraph, new Guid?(this.UserID));
			this.PrintStationID = BasicLabelUtils.GetPrintStationID(this.LabelGraph, new Guid?(this.UserID));
			this.Owner = BasicLabelUtils.GetOwner(this.LabelGraph, new Guid?(this.UserID));
			CREmployee owner = this.Owner;
			this.OwnerID = ((owner != null) ? owner.DefContactID : null);
		}

		// Token: 0x060013A4 RID: 5028 RVA: 0x00044018 File Offset: 0x00042218
		private void HandlePrinter(Guid? printerID = null)
		{
			Guid? guid = printerID;
			if (guid == null)
			{
				printerID = this.ChoosePrinter();
			}
			IPrinter printer;
			Printers.TryGetPrinter(printerID, out printer);
			this.Printer = printer;
			RuleUtils.IRuleDrivenFactory<FormatRules.FormatRule, IFormat> format_FACTORY = RuleUtils.FORMAT_FACTORY;
			IPrinter printer2 = this.Printer;
			this.PrinterFormat = format_FACTORY.GetValueByRules(this, (printer2 != null) ? printer2.FormatID : null);
			this.HandlePrinterMargin();
		}

		// Token: 0x060013A5 RID: 5029 RVA: 0x00044080 File Offset: 0x00042280
		private void HandlePrinterMargin()
		{
			IFormat printerFormat = this.PrinterFormat;
			bool flag = printerFormat != null && printerFormat.MarginID != null;
			if (flag)
			{
				Margins.Margin printerMargin;
				Margins.TryGetMargin(this.PrinterFormat.MarginID, out printerMargin);
				this.PrinterMargin = printerMargin;
			}
		}

		// Token: 0x060013A6 RID: 5030 RVA: 0x000440CC File Offset: 0x000422CC
		private void HandleFormat(ALPrintLog printLog = null)
		{
			bool flag = printLog != null;
			if (flag)
			{
				this.ModelFormat = RuleUtils.FORMAT_FACTORY.GetValueByRules(this, printLog.ModelFormatID);
				IFormat modelFormat = this.ModelFormat;
				bool flag2 = ALRotation.GetRotationDegrees((modelFormat != null) ? modelFormat.Rotation : null) != 0;
				if (flag2)
				{
					IFormat modelFormat2 = Formats.Format.Unrotate(this.ModelFormat);
					this.ModelFormat = modelFormat2;
				}
			}
			if (this.ModelFormat == null)
			{
				this.ModelFormat = RuleUtils.FORMAT_FACTORY.GetValueByRules(this, this.Model.FormatID);
			}
		}

		// Token: 0x060013A7 RID: 5031 RVA: 0x00044158 File Offset: 0x00042358
		private void HandleRow(object row)
		{
			this.Row = row;
			bool flag = this.Row == null;
			if (flag)
			{
				throw new PXException("A row is required to print the label for Model '{0}'", new object[]
				{
					this.Model.Description
				});
			}
		}

		// Token: 0x060013A8 RID: 5032 RVA: 0x0004419C File Offset: 0x0004239C
		private void HandleGraph(PXGraph rowGraph, Type rowGraphType)
		{
			this.Graph = rowGraph;
			this.GraphType = rowGraphType;
			bool flag = this.Graph == null && this.GraphType == null;
			if (flag)
			{
				throw new PXException("Cannot find a graph to print the label for Model '{0}'", new object[]
				{
					this.Model.Description
				});
			}
			bool flag2 = this.Graph == null;
			if (flag2)
			{
				Type type = GraphHelper.GetType(this.GraphType.FullName);
				bool flag3 = type == null;
				if (flag3)
				{
					throw new PXException("Cannot find a graph to print the label for Model '{0}'", new object[]
					{
						this.Model.Description
					});
				}
				this.Graph = HiddenUtils.CreateInstance(type);
			}
			ViewUtils.SetDocumentCurrent(this.Graph, this.Row);
		}

		// Token: 0x060013A9 RID: 5033 RVA: 0x00044260 File Offset: 0x00042460
		private void HandleModel(Guid? modelID, TemplateContext context = null)
		{
			this.Model = ALModel.PK.Find(this.LabelGraph, modelID);
			ALModel model = this.Model;
			this.LanguageCode = ((model != null) ? model.Language : null);
			this.LabelGraph.Model.Current = this.Model;
			if (context == null)
			{
				context = ScribanUtils.CreateContext(this.Graph, this.Row, null, this.IsDevMode, new object[]
				{
					this.Model
				});
			}
			context.SetValue(this);
			this.ScribanContext = context;
			this.ScribanContext.TemplateLoader = this;
			bool flag = this.Model != null && this.Model.ModelType == "S";
			if (flag)
			{
				IEnumerable<PXResult<ALModelExpr>> source = this.LabelGraph.Expressions.Select(Array.Empty<object>());
				Func<PXResult<ALModelExpr>, bool> predicate;
				if ((predicate = LabelContext.<>O.<0>__CheckHasIterator) == null)
				{
					predicate = (LabelContext.<>O.<0>__CheckHasIterator = new Func<PXResult<ALModelExpr>, bool>(LabelContext.CheckHasIterator));
				}
				bool hasIterator = source.Any(predicate);
				ContextVariables.SetHasIterator(context, hasIterator);
			}
		}

		// Token: 0x060013AA RID: 5034 RVA: 0x00044360 File Offset: 0x00042560
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

		// Token: 0x060013AB RID: 5035 RVA: 0x000443A8 File Offset: 0x000425A8
		private void HandleMargin()
		{
			bool flag = this.Model.MarginID != null;
			if (flag)
			{
				Margins.Margin margin;
				Margins.TryGetMargin(this.Model.MarginID, out margin);
				bool flag2 = margin != null;
				if (flag2)
				{
					this.ModelMargin = margin;
				}
			}
			else
			{
				bool flag3 = this.Model.FormatID != null;
				if (flag3)
				{
					IFormat format;
					Formats.TryGetFormat(this.Model.FormatID, out format);
					bool flag4 = format != null && format.MarginID != null;
					if (flag4)
					{
						Margins.Margin modelMargin;
						Margins.TryGetMargin((format != null) ? format.MarginID : null, out modelMargin);
						this.ModelMargin = modelMargin;
					}
					bool valueOrDefault = ((format != null) ? format.IsComposite : null).GetValueOrDefault();
					if (valueOrDefault)
					{
						this.ModelFormat = RuleUtils.FORMAT_FACTORY.GetValueByRules(this, this.Model.FormatID);
						IFormat modelFormat = this.ModelFormat;
						bool flag5 = modelFormat != null && modelFormat.MarginID != null;
						if (flag5)
						{
							Margins.Margin margin2;
							Margins.TryGetMargin(this.ModelFormat.MarginID, out margin2);
							bool flag6 = margin2 != null;
							if (flag6)
							{
								this.ModelMargin = margin2;
							}
						}
					}
				}
			}
		}

		// Token: 0x060013AC RID: 5036 RVA: 0x00044504 File Offset: 0x00042704
		private void VerifyModelFormat()
		{
			bool flag = this.ModelFormat == null;
			if (flag)
			{
				throw new PXException("Cannot find a format for Model '{0}'", new object[]
				{
					this.Model.Description
				});
			}
		}

		// Token: 0x060013AD RID: 5037 RVA: 0x00044540 File Offset: 0x00042740
		private Guid? ChoosePrinter()
		{
			bool isRender = this.IsRender;
			Guid? result;
			if (isRender)
			{
				Guid? renderingPrinterID = ALSetupSlot.RenderingPrinterID;
				this.ValidatePrinter(renderingPrinterID, "You have to configure a Rendering Printer in the Label Basic Preferences", Array.Empty<object>());
				result = renderingPrinterID;
			}
			else
			{
				IPrintOption printOption = AsgardUtils.FindExtension<IPrintOption>(this.Row);
				bool flag = printOption != null;
				if (flag)
				{
					Guid? usrALPrinterID = printOption.UsrALPrinterID;
					this.ValidatePrinter(usrALPrinterID, "A Label Printer must be defined in Screen '{0}' when using Printer Override", new object[]
					{
						this.Model.ScreenID
					});
					this.UpdateFeatureConsumption(typeof(IPrintOption), 1);
					result = usrALPrinterID;
				}
				else
				{
					ModelPrinters.ModelPrinter[] printers = ModelPrinters.GetPrinters(this.Model.LabelID);
					bool flag2 = !printers.Any<ModelPrinters.ModelPrinter>();
					if (flag2)
					{
						bool ignorePrinterMissing = this.IgnorePrinterMissing;
						if (!ignorePrinterMissing)
						{
							throw new PXException("A Label Printer must be defined for you under Model '{0}'", new object[]
							{
								this.Model.Description
							});
						}
						PXTrace.WriteWarning("The automated Label Printing was ignored because no printer is available for model '{0}' ", new object[]
						{
							this.Model.Description
						});
						result = null;
					}
					else
					{
						result = BasicLabelUtils.ChoosePrinter<ModelPrinters.ModelPrinter>(this, this.LabelGraph, printers);
					}
				}
			}
			return result;
		}

		// Token: 0x060013AE RID: 5038 RVA: 0x00044664 File Offset: 0x00042864
		private void ValidatePrinter(Guid? printerID, string format, params object[] args)
		{
			IPrinter printer;
			Printers.TryGetPrinter(printerID, out printer);
			bool flag = printer == null;
			if (flag)
			{
				throw new PXException(format, args);
			}
		}

		// Token: 0x060013AF RID: 5039 RVA: 0x0004468C File Offset: 0x0004288C
		[return: TupleElementNames(new string[]
		{
			"colDots",
			"rowDots"
		})]
		public ValueTuple<int, int> GetGotoDots(decimal x, decimal y)
		{
			ALModel model = this.Model;
			string text = ((model != null) ? model.LayoutType : null) ?? "P";
			Layout layout = ContextVariables.GetLayout(this.ScribanContext);
			string text2 = text;
			string a = text2;
			int item;
			int item2;
			if (!(a == "P"))
			{
				item = layout.CalcColDotsToDots((int)x);
				item2 = layout.CalcRowDotsToDots((int)y);
			}
			else
			{
				item = layout.CalcColPercToDots(x, true);
				item2 = layout.CalcRowPercToDots(y, true);
			}
			return new ValueTuple<int, int>(item, item2);
		}

		// Token: 0x060013B0 RID: 5040 RVA: 0x00044718 File Offset: 0x00042918
		public string GetPath(TemplateContext context, SourceSpan callerSpan, string templateName)
		{
			return templateName;
		}

		// Token: 0x060013B1 RID: 5041 RVA: 0x0004472C File Offset: 0x0004292C
		public string Load(TemplateContext context, SourceSpan callerSpan, string snippetName)
		{
			ALModel almodel = PXSelectBase<ALModel, PXSelect<ALModel, Where<ALModel.name, Equal<Required<ALModel.name>>>>.Config>.Select(this.LabelGraph, new object[]
			{
				snippetName
			});
			bool flag = almodel != null;
			if (flag)
			{
				Layout layout = ContextVariables.GetLayout(this.ScribanContext);
				LabelContext value = context.GetValue(false);
				LabelContext labelContext = new LabelContext(ContextVariables.GetRowGraph(context), ContextVariables.GetRow(context), almodel.LabelID, value.IsRender, false);
				ContextVariables.SetLayout(labelContext.ScribanContext, layout);
				string renderedTemplate = labelContext.GetRenderedTemplate();
				return renderedTemplate.Replace(ZplCmd.START.Raw(), "").Replace(ZplCmd.END.Raw(), "");
			}
			throw new PXException("A Snippet by the name of '{0}' cannot be found", new object[]
			{
				snippetName
			});
		}

		// Token: 0x060013B2 RID: 5042 RVA: 0x000447F3 File Offset: 0x000429F3
		public ValueTask<string> LoadAsync(TemplateContext context, SourceSpan callerSpan, string templatePath)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060013B3 RID: 5043 RVA: 0x000447FC File Offset: 0x000429FC
		public IDestination GetDestination()
		{
			bool flag = this.Printer == null && this.Model.ModelType == "S" && !this.IgnorePrinterMissing;
			if (flag)
			{
				throw new PXException("Please define a printer in the Printers section");
			}
			bool flag2 = this.Printer == null;
			IDestination result;
			if (flag2)
			{
				result = null;
			}
			else
			{
				IDestination destination = this.GetDestination(this.Printer);
				result = destination;
			}
			return result;
		}

		// Token: 0x060013B4 RID: 5044 RVA: 0x0004486C File Offset: 0x00042A6C
		public IDestination GetDestination(IPrinter printer)
		{
			IDestination destination = AsgardUtils.CreateImpl2<IDestination>(printer.PrinterType);
			destination.Printer = printer;
			return destination;
		}

		// Token: 0x060013B5 RID: 5045 RVA: 0x00044894 File Offset: 0x00042A94
		public RenderResult RenderAsOutput()
		{
			this.DoRenderAsLanguage();
			IPrinterLanguage printerLanguage = this.PrinterLanguage;
			RenderResult renderResult = printerLanguage.RenderAsOutput(this);
			ISet<string> warnings = renderResult.Warnings;
			bool flag = warnings.Any<string>();
			if (flag)
			{
				PXTrace.WriteWarning(string.Join(",", warnings));
			}
			bool isDevMode = this.IsDevMode;
			if (isDevMode)
			{
				this.Model.Message = (warnings.Any<string>() ? string.Join("\n", warnings) : null);
				this.Model = this.LabelGraph.Model.Update(this.Model);
				this.LabelGraph.Actions.PressSave();
			}
			return renderResult;
		}

		// Token: 0x060013B6 RID: 5046 RVA: 0x00044944 File Offset: 0x00042B44
		public FileResult RenderAndSaveAsUrl(ALPrintLog log = null)
		{
			FileResult result;
			try
			{
				RenderResult source = this.RenderAsOutput();
				byte[] data = source.FirstOrDefault<byte[]>();
				FileResult fileResult = new FileResult(data, new int?(1), log);
				FileResult fileResult2 = this.SaveFile(fileResult, null, true);
				result = fileResult;
			}
			catch (Exception ex)
			{
				bool isDevMode = this.IsDevMode;
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

		// Token: 0x060013B7 RID: 5047 RVA: 0x000449E8 File Offset: 0x00042BE8
		internal void SaveRendered(string rendered)
		{
			bool flag = !this.IsSaveRendered;
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
						array[2] = this.ModelFormat.Name;
						array[3] = this.ModelFormat.Width;
						array[4] = this.ModelFormat.Height;
						array[5] = this.ModelFormat.SizeUnit;
						array[6] = this.ModelFormat.PrintDensity;
						array[7] = this.ModelFormat.PrintDensityType;
						int num = 8;
						string fullUserName = PXAccess.GetFullUserName();
						array[num] = ((fullUserName != null) ? fullUserName.Trim() : null);
						array[9] = name;
						array[10] = DateTime.Now;
						string str = string.Format(format, array);
						bool flag3 = this.ModelMargin != null;
						if (flag3)
						{
							str += string.Format(", Margin:{0} (L:{1},R:{2},T:{3},B:{4} {5})", new object[]
							{
								this.ModelMargin.Name,
								this.ModelMargin.Left,
								this.ModelMargin.Right,
								this.ModelMargin.Top,
								this.ModelMargin.Bottom,
								this.ModelMargin.SizeUnit
							});
						}
						object row = this.Row;
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
						bool flag6 = this.Printer != null;
						if (flag6)
						{
							str = str + ", Printer:" + this.Printer.Name;
						}
						str += " ----";
						string text2 = rendered;
						bool addLineNumber = ALSetupSlot.AddLineNumber;
						if (addLineNumber)
						{
							text2 = AsgardUtils.AddLineNumbers(text2, 4);
						}
						string str2 = str + "\n" + text2 + "\n";
						ALModel model = this.Model;
						model.Rendered += str2;
					}
					this.Model = this.LabelGraph.Model.Update(this.Model);
					this.LabelGraph.Actions.PressSave();
				}
				catch (Exception ex)
				{
					PXTrace.WriteError(ex);
				}
			}
		}

		// Token: 0x060013B8 RID: 5048 RVA: 0x00044D20 File Offset: 0x00042F20
		public FileResult SaveFile(FileResult printResult, string fieldName = null, bool saveAsUrl = false)
		{
			bool flag = printResult.Log == null;
			if (flag)
			{
				printResult = this.SaveFileToPrintLog(printResult, null);
			}
			else
			{
				bool flag2 = printResult.UID == null;
				if (flag2)
				{
					printResult = this.DoSavePrintLog(printResult, null);
				}
			}
			bool flag3 = !string.IsNullOrEmpty(fieldName) || saveAsUrl;
			if (flag3)
			{
				this.SaveFileInfo(printResult, fieldName, saveAsUrl);
			}
			return printResult;
		}

		// Token: 0x060013B9 RID: 5049 RVA: 0x00044D94 File Offset: 0x00042F94
		public void SaveFileInfo(FileInfo fileInfo, string fieldName, bool saveAsUrl = false)
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
				throw new PXException("No field named '{0}' in cache '{1}'", new object[]
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

		// Token: 0x060013BA RID: 5050 RVA: 0x00044EB4 File Offset: 0x000430B4
		public FileResult SaveFileToPrintLog(FileResult printResult, string prefix = null)
		{
			FileResult result;
			using (PXTransactionScope pxtransactionScope = new PXTransactionScope())
			{
				PXTransactionScope.SetSuppressWorkflow(true);
				this.FinalOutputFormat = (OutputFormat)((this.Printer != null) ? this.Printer.ContentType.Value : ((int)printResult.Format));
				FileResult fileResult = this.DoSavePrintLog(printResult, prefix);
				this.PrintLog = fileResult;
				FileResult fileResult2 = fileResult;
				IFormat modelFormat = this.ModelFormat;
				bool flag = ALRotation.HasRotation((modelFormat != null) ? modelFormat.Rotation : null);
				bool flag2 = !this.IsSameDensity.Value;
				if (flag2)
				{
					string printOnOtherDensity = this.PrintOnOtherDensity;
					string a = printOnOtherDensity;
					if (!(a == "NG") && !(a == "NA"))
					{
						if (a == "DF" || a == "DA")
						{
							this.FinalOutputFormat = OutputFormat.PDF;
							ITransformer instance = ZplToPdf.INSTANCE;
							FileResult printResult2 = instance.Transform(this, fileResult2);
							fileResult2 = this.DoSavePrintLog(printResult2, instance.GetType().Name);
						}
					}
					else
					{
						this.FinalOutputFormat = OutputFormat.PNG;
						ITransformer instance = ZplToPng.INSTANCE;
						FileResult printResult3 = instance.Transform(this, fileResult2);
						fileResult2 = this.DoSavePrintLog(printResult3, instance.GetType().Name);
						this.FinalOutputFormat = OutputFormat.PDF;
						instance = PngToPdf.INSTANCE;
						FileResult printResult4 = instance.Transform(this, fileResult2);
						fileResult2 = this.DoSavePrintLog(printResult4, instance.GetType().Name);
					}
				}
				bool flag3 = flag && fileResult2.Format == OutputFormat.ZPL && this.FinalOutputFormat == OutputFormat.ZPL;
				if (flag3)
				{
					ITransformer instance = ZplToGraphicToZpl.INSTANCE;
					FileResult printResult5 = instance.Transform(this, fileResult2);
					fileResult2 = this.DoSavePrintLog(printResult5, instance.GetType().Name);
				}
				bool flag4 = fileResult2.Format == OutputFormat.ZPL && this.FinalOutputFormat == OutputFormat.PNG;
				if (flag4)
				{
					FileResult fileResult3 = ZplToPng.INSTANCE.Transform(this, fileResult2);
					bool flag5 = flag;
					if (flag5)
					{
						IFormat modelFormat2 = this.ModelFormat;
						fileResult3 = ImageRotation.RotatePrintResult((modelFormat2 != null) ? modelFormat2.Rotation : null, fileResult3);
					}
					ITransformer instance = ZplToPng.INSTANCE;
					fileResult2 = this.DoSavePrintLog(fileResult3, instance.GetType().Name);
				}
				bool flag6 = fileResult2.Format == OutputFormat.ZPL && this.FinalOutputFormat == OutputFormat.PDF;
				if (flag6)
				{
					ITransformer instance = ZplToPdf.INSTANCE;
					FileResult printResult6 = instance.Transform(this, fileResult2);
					fileResult2 = this.DoSavePrintLog(printResult6, instance.GetType().Name);
				}
				bool flag7 = fileResult2.Format == OutputFormat.ZPL && this.FinalOutputFormat == OutputFormat.SBPL;
				if (flag7)
				{
					ITransformer instance = ZplToSbpl.INSTANCE;
					FileResult printResult7 = instance.Transform(this, fileResult2);
					fileResult2 = this.DoSavePrintLog(printResult7, instance.GetType().Name);
				}
				bool flag8 = fileResult2.Format == OutputFormat.PNG && this.FinalOutputFormat == OutputFormat.ZPL;
				if (flag8)
				{
					ITransformer instance = PngToZpl.INSTANCE;
					FileResult printResult8 = instance.Transform(this, fileResult2);
					fileResult2 = this.DoSavePrintLog(printResult8, instance.GetType().Name);
				}
				pxtransactionScope.Complete();
				result = fileResult2;
			}
			return result;
		}

		// Token: 0x060013BB RID: 5051 RVA: 0x000451DC File Offset: 0x000433DC
		private FileResult DoSavePrintLog(FileResult printResult, string prefix = null)
		{
			PXCache cache = this.PrintLogGraph.Document.Cache;
			OutputFormat format = printResult.Format;
			bool flag = this.PrintLog != null;
			ALPrintLog alprintLog;
			string text;
			if (flag)
			{
				alprintLog = this.PrintLog;
				text = printResult.FullName;
				int finalOutputFormat = (int)this.FinalOutputFormat;
				int? contentType = alprintLog.ContentType;
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
					alprintLog.ContentType = new int?(finalOutputFormat);
					alprintLog = this.PrintLogGraph.Document.Update(alprintLog);
				}
			}
			else
			{
				ALPrintLog alprintLog2 = new ALPrintLog();
				alprintLog2.ModelID = this.Model.LabelID;
				IFormat modelFormat = this.ModelFormat;
				alprintLog2.ModelFormatID = ((modelFormat != null) ? modelFormat.FormatID : null);
				IMargin modelMargin = this.ModelMargin;
				alprintLog2.ModelMarginID = ((modelMargin != null) ? modelMargin.MarginID : null);
				string screenID;
				if ((screenID = this.Model.ScreenID) == null)
				{
					screenID = (PXContext.GetScreenID() ?? "AL201000");
				}
				alprintLog2.ScreenID = screenID;
				IFormat printerFormat = this.PrinterFormat;
				alprintLog2.PrinterFormatID = ((printerFormat != null) ? printerFormat.FormatID : null);
				IMargin printerMargin = this.PrinterMargin;
				alprintLog2.PrinterMarginID = ((printerMargin != null) ? printerMargin.MarginID : null);
				IPrinter printer = this.Printer;
				alprintLog2.PrinterID = ((printer != null) ? printer.PrinterID : null);
				alprintLog2.PrintStationID = this.PrintStationID;
				alprintLog2.OwnerID = this.OwnerID;
				alprintLog2.UserID = new Guid?(this.UserID);
				alprintLog2.BAccountID = this.BAccountID;
				alprintLog2.LabelKey = BasicLabelUtils.GetKeys(this.Graph, this.LabelRow, "/") + LabelContext.IteratorKey(this);
				alprintLog2.NbCopies = new int?(printResult.NbCopies);
				alprintLog2.RefNoteID = this.GetRefNoteID();
				alprintLog2.InventoryID = this.GetInventoryID();
				alprintLog2.LotSerialNbr = this.GetLotSerialNbr();
				alprintLog2.ContentType = new int?((int)this.FinalOutputFormat);
				alprintLog = alprintLog2;
				alprintLog = this.PrintLogGraph.Document.Insert(alprintLog);
				this.PrintLogGraph.Persist();
				text = this.GetLabelFilename(this.PrintLogGraph, alprintLog, new OutputFormat?(format));
				alprintLog.LabelFilename = text;
				alprintLog = this.PrintLogGraph.Document.Update(alprintLog);
				string fullName = CustomizedTypeManager.GetTypeNotCustomized(this.Graph).FullName;
				FileUtils.UpdateNoteRecord(this.PrintLogGraph, alprintLog, fullName, alprintLog.RecordID);
			}
			text = AsgardUtils.RemoveIllegalFileNameCharacters(text);
			bool flag4 = prefix == null;
			if (flag4)
			{
				OutputFormat outputFormat = format;
				OutputFormat outputFormat2 = outputFormat;
				if (outputFormat2 != OutputFormat.PNG)
				{
					prefix = "Asgard Label";
				}
				else
				{
					prefix = "Rendered";
				}
			}
			FileInfo file = FileUtils.SaveFileToRow(this.FileMaintenance, printResult.BinData, text, cache, alprintLog, prefix);
			this.PrintLogGraph.Persist();
			return new FileResult(file, alprintLog, new int?(printResult.NbCopies));
		}

		// Token: 0x060013BC RID: 5052 RVA: 0x0004551C File Offset: 0x0004371C
		private static string IteratorKey(LabelContext labelContext)
		{
			TemplateContext scribanContext = labelContext.ScribanContext;
			bool flag = !ContextVariables.HasIteratorByPage(scribanContext);
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

		// Token: 0x060013BD RID: 5053 RVA: 0x00045564 File Offset: 0x00043764
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

		// Token: 0x060013BE RID: 5054 RVA: 0x000455E0 File Offset: 0x000437E0
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

		// Token: 0x060013BF RID: 5055 RVA: 0x000456C8 File Offset: 0x000438C8
		public void Print(FileResult printResult)
		{
			bool flag = this.Printer == null;
			if (flag)
			{
				this.HandlePrinter(null);
			}
			IDestination destination = this.GetDestination();
			bool flag2 = destination == null && this.IgnorePrinterMissing;
			if (flag2)
			{
				PXTrace.WriteWarning("The automated Label Printing was ignored because no printer is available for model '{0}' ", new object[]
				{
					this.Model.Description
				});
			}
			else
			{
				bool flag3 = this.Printer != null && printResult.Log.PrinterID == null;
				if (flag3)
				{
					printResult.Log.PrinterID = this.Printer.PrinterID;
					printResult.Log.PrintStationID = this.Printer.PrintStationID;
				}
				destination.Print(this, printResult);
				this.PrintLogGraph.Document.Update(printResult.Log);
				this.PrintLogGraph.Persist();
			}
		}

		// Token: 0x060013C0 RID: 5056 RVA: 0x000457B4 File Offset: 0x000439B4
		public void Print(FileInfo fi, ALPrintLog logRow = null, int? nbCopies = null)
		{
			this.Print(new FileResult(fi, logRow, nbCopies));
		}

		// Token: 0x060013C1 RID: 5057 RVA: 0x000457C8 File Offset: 0x000439C8
		private string GetLabelFilename(PXGraph graph, object row, OutputFormat? useFormat = null)
		{
			string defaultFilename = BasicLabelUtils.GetDefaultFilename(graph, row, this.Model);
			string str = ALContentType.AsExtension(useFormat ?? this.FinalOutputFormat);
			string expr = defaultFilename.Trim() + "." + str;
			return BasicLabelUtils.SurroundBy(expr, "AL-label-", null);
		}

		// Token: 0x060013C2 RID: 5058 RVA: 0x0004582C File Offset: 0x00043A2C
		private void DoRenderAsLanguage()
		{
			bool isRendered = this.IsRendered;
			if (!isRendered)
			{
				this.SaveRendered(null);
				string text = this.TemplateBody;
				List<string> list = new List<string>();
				PXResultset<ALModelGraphic> pxresultset = this.LabelGraph.Graphics.Select(Array.Empty<object>());
				foreach (PXResult<ALModelGraphic> pxresult in pxresultset)
				{
					ALModelGraphic almodelGraphic = pxresult;
					ValueTuple<string, string> valueTuple = BasicLabelUtils.HandleFieldReverse(this, almodelGraphic);
					string item = valueTuple.Item1;
					string item2 = valueTuple.Item2;
					ValueTuple<string, string> valueTuple2 = BasicLabelUtils.HandleColor(this, almodelGraphic);
					string item3 = valueTuple2.Item1;
					string item4 = valueTuple2.Item2;
					ValueTuple<string, string> valueTuple3 = BasicLabelUtils.HandleGraphic(this, almodelGraphic);
					string item5 = valueTuple3.Item1;
					string item6 = valueTuple3.Item2;
					string[] value = AsgardUtils.NonNulls<string>(new string[]
					{
						item,
						item2,
						item3,
						item4,
						item5,
						item6
					});
					string item7 = string.Join(string.Empty, value);
					list.Add(item7);
				}
				TemplateContext scribanContext = this.ScribanContext;
				IPrinterLanguage printerLanguage = this.PrinterLanguage;
				foreach (PXResult<ALModelExpr> pxresult2 in this.Expressions.Value)
				{
					ALModelExpr almodelExpr = pxresult2;
					ValueTuple<string, string> expression = printerLanguage.GetExpression(this, almodelExpr);
					string item8 = expression.Item1;
					string item9 = expression.Item2;
					scribanContext.SetValue(new ScriptVariableGlobal("Expr" + almodelExpr.LineNbr.ToString()), item9, false);
					list.Add(item9);
				}
				bool flag = list.Any<string>();
				if (flag)
				{
					string value2 = list.Merge();
					int startIndex = text.IndexOf("{{");
					text = text.Insert(startIndex, value2);
				}
				Template template = Template.Parse(text, null, null, null);
				int num = 1;
				ScribanUtils.CheckTemplateErrors(string.Format("Parse {0}", num), this.Model.Name, template);
				this.TemplateBody = template.Render(scribanContext);
				this.SaveRendered(this.TemplateBody);
				while (this.TemplateBody.HasMoreToRender())
				{
					Template template2 = Template.Parse(this.TemplateBody, null, null, null);
					num++;
					ScribanUtils.CheckTemplateErrors(string.Format("Parse {0}", num), this.Model.Name, template2);
					this.TemplateBody = template2.Render(this.ScribanContext);
					this.SaveRendered(this.TemplateBody);
				}
			}
		}

		// Token: 0x060013C3 RID: 5059 RVA: 0x00045B00 File Offset: 0x00043D00
		private Guid? GetRefNoteID()
		{
			Guid? result;
			try
			{
				Guid? refNoteIDSilent = this.GetRefNoteIDSilent(this.LabelRow);
				bool flag = refNoteIDSilent == null;
				if (flag)
				{
					refNoteIDSilent = this.GetRefNoteIDSilent(this.Row);
				}
				result = new Guid?(refNoteIDSilent.Value);
			}
			catch
			{
				result = null;
			}
			return result;
		}

		// Token: 0x060013C4 RID: 5060 RVA: 0x00045B68 File Offset: 0x00043D68
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

		// Token: 0x060013C5 RID: 5061 RVA: 0x00045BD8 File Offset: 0x00043DD8
		public int? PeekNextSerial()
		{
			bool isDesignMode = this.IsDesignMode;
			int? result;
			if (isDesignMode)
			{
				result = new int?(999);
			}
			else
			{
				bool flag = this._nextSerial == null;
				if (flag)
				{
					throw new PXException("You can call PeekNextSerial() if you have not called GetNextSerial() first");
				}
				result = this._nextSerial;
			}
			return result;
		}

		// Token: 0x060013C6 RID: 5062 RVA: 0x00045C28 File Offset: 0x00043E28
		public int? GetNextSerial()
		{
			bool isDesignMode = this.IsDesignMode;
			int? result;
			if (isDesignMode)
			{
				result = new int?(999);
			}
			else
			{
				string numberingID = this.Model.NumberingID;
				bool flag = this._nextSerial != null;
				if (flag)
				{
					this._nextSerial++;
					int? nextSerial = this._nextSerial;
					int? num = this._lastSerial;
					bool flag2 = nextSerial.GetValueOrDefault() > num.GetValueOrDefault() & (nextSerial != null & num != null);
					if (flag2)
					{
						this._nextSerial = null;
						this._lastSerial = null;
						return this.GetNextSerial();
					}
				}
				else
				{
					bool flag3 = string.IsNullOrEmpty(numberingID);
					if (flag3)
					{
						throw new PXException("Model '{0}' requires a Numbering Sequence", new object[]
						{
							this.Model.Name
						});
					}
					NumberingSequence numberingSequence = AutoNumberAttribute.GetNumberingSequence(numberingID, null, new DateTime?(DateTime.Now));
					bool flag4 = numberingSequence == null;
					if (flag4)
					{
						throw new PXException("A Sequence was not found for Numbering '{0}' and Model '{1}'", new object[]
						{
							this.Model.NumberingID,
							this.Model.Name
						});
					}
					int nbCopies = this.GetNbCopies();
					string lastNbr = numberingSequence.LastNbr;
					int? numberingSEQ = numberingSequence.NumberingSEQ;
					int num2 = nbCopies * numberingSequence.NbrStep.GetValueOrDefault(1);
					string text = AutoNumberAttribute.NextNumber(lastNbr, num2);
					bool flag5 = !AsgardUtils.IsNumber(text);
					if (flag5)
					{
						throw new PXException("Only a numeric Sequence is supported for Numbering '{0}' and Model '{1}'", new object[]
						{
							numberingID,
							this.Model.Name
						});
					}
					bool flag6 = text.CompareTo(numberingSequence.EndNbr) >= 0;
					if (flag6)
					{
						throw new PXException("Cannot generate the next number for the {0} sequence because it is expired.", new object[]
						{
							numberingID
						});
					}
					this._nextSerial = new int?(int.Parse(text));
					int? num = this._nextSerial;
					int num3 = num2;
					this._lastSerial = ((num != null) ? new int?(num.GetValueOrDefault() + num3 - 1) : null);
					DateTime now = DateTime.Now;
					Guid userID = PXAccess.GetUserID();
					bool flag7 = lastNbr == numberingSequence.StartNbr;
					if (flag7)
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
						bool flag8 = !PXDatabase.Update<NumberingSequence>(new PXDataFieldParam[]
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
						if (flag8)
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
								bool flag9 = pxdataRecord != null;
								if (flag9)
								{
									string @string = pxdataRecord.GetString(0);
									int num4 = nbCopies * numberingSequence.NbrStep.GetValueOrDefault(1);
									string text2 = AutoNumberAttribute.NextNumber(@string, num4);
									bool flag10 = text2.CompareTo(numberingSequence.EndNbr) >= 0;
									if (flag10)
									{
										throw new PXException("Cannot generate the next number for the {0} sequence because it is expired.", new object[]
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
				result = new int?(this._nextSerial.Value);
			}
			return result;
		}

		// Token: 0x060013C7 RID: 5063 RVA: 0x00046154 File Offset: 0x00044354
		public int GetNbCopies()
		{
			bool isRender = this.IsRender;
			int result;
			if (isRender)
			{
				result = 1;
			}
			else
			{
				int? num = null;
				num = this.GetNbCopiesOverride();
				bool flag;
				if (num != null)
				{
					int? num2 = num;
					int num3 = 0;
					flag = (num2.GetValueOrDefault() <= num3 & num2 != null);
				}
				else
				{
					flag = true;
				}
				bool flag2 = flag;
				if (flag2)
				{
					num = new int?(this.ScribanContext.EvalExpr(this.Model.NbCopiesExpr, 1));
				}
				result = num.GetValueOrDefault(1);
			}
			return result;
		}

		// Token: 0x060013C8 RID: 5064 RVA: 0x000461D8 File Offset: 0x000443D8
		public int? GetNbCopiesOverride()
		{
			ILabelOption labelOption = AsgardUtils.FindExtension<ILabelOption>(this.DetailRow);
			bool flag = labelOption == null;
			if (flag)
			{
				labelOption = AsgardUtils.FindExtension<ILabelOption>(this.Row);
			}
			bool flag2 = labelOption != null;
			if (flag2)
			{
				this.UpdateFeatureConsumption(typeof(ILabelOption), 1);
			}
			return (labelOption != null) ? labelOption.UsrALNbrOfCopies : null;
		}

		// Token: 0x060013C9 RID: 5065 RVA: 0x00046240 File Offset: 0x00044440
		public int GetDealingCount()
		{
			bool flag = !this.DealingMode;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				int? num = null;
				bool flag2 = !this.IsDesignMode;
				if (flag2)
				{
					num = this.GetDealingCountOverride();
					bool flag3 = num == null;
					if (flag3)
					{
						num = new int?(this.ScribanContext.EvalExpr(this.Model.DealingCountExpr, 0));
					}
				}
				result = num.GetValueOrDefault();
			}
			return result;
		}

		// Token: 0x060013CA RID: 5066 RVA: 0x000462BC File Offset: 0x000444BC
		public int? GetDealingCountOverride()
		{
			return null;
		}

		// Token: 0x060013CB RID: 5067 RVA: 0x000462D8 File Offset: 0x000444D8
		public ISerialInfo GetSerialInfo(string content = null)
		{
			if (content == null)
			{
				content = this.RenderedTemplate;
			}
			return this.PrinterLanguage.GetSerialInfo(this, content);
		}

		// Token: 0x060013CC RID: 5068 RVA: 0x00019FF9 File Offset: 0x000181F9
		internal void Check()
		{
		}

		// Token: 0x060013CD RID: 5069 RVA: 0x00046304 File Offset: 0x00044504
		internal void UpdateFeatureConsumption(Type type, int nbLabels = 1)
		{
			this.LicenseManager.UpdateFeatureConsumption(type, nbLabels);
		}

		// Token: 0x060013CE RID: 5070 RVA: 0x00046315 File Offset: 0x00044515
		internal void CheckFeatureConsumption(Type type, int nextQty)
		{
			this.LicenseManager.CheckFeatureConsumption(type, nextQty);
		}

		// Token: 0x060013CF RID: 5071 RVA: 0x00046328 File Offset: 0x00044528
		internal IteratorContext GetIteratorContext(ALDataElement dataElement = null, ICoordinate coordinate = null)
		{
			string contextName = IteratorContext.GetContextName();
			IteratorContext iteratorContext = this.ScribanContext.GetValue(contextName, true, null);
			bool flag = iteratorContext == null && dataElement != null;
			if (flag)
			{
				iteratorContext = new IteratorContext(this, dataElement, coordinate);
				this.ScribanContext.CurrentGlobal.SetValue(contextName, iteratorContext, false);
			}
			return iteratorContext;
		}

		// Token: 0x060013D0 RID: 5072 RVA: 0x00046380 File Offset: 0x00044580
		internal bool IteratorHasMorePages()
		{
			bool flag = !ContextVariables.HasIterator(this.ScribanContext);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				IteratorContext iteratorContext = this.GetIteratorContext(null, null);
				bool flag2 = iteratorContext != null && iteratorContext.HasMorePages;
				bool flag3 = flag2;
				if (flag3)
				{
					this.PrepareForNextPage();
				}
				result = flag2;
			}
			return result;
		}

		// Token: 0x060013D1 RID: 5073 RVA: 0x000463D0 File Offset: 0x000445D0
		internal void PrepareForNextPage()
		{
			bool flag = ContextVariables.HasIterator(this.ScribanContext);
			if (flag)
			{
				this.PrintLog = null;
				this.ResetRenderedBody();
			}
		}

		// Token: 0x060013D2 RID: 5074 RVA: 0x00046400 File Offset: 0x00044600
		internal void EndIterator()
		{
			bool flag = ContextVariables.HasIterator(this.ScribanContext);
			if (flag)
			{
				IteratorContext iteratorContext = this.GetIteratorContext(null, null);
				if (iteratorContext != null)
				{
					iteratorContext.End();
				}
			}
		}

		// Token: 0x0400086C RID: 2156
		private readonly IList<string> _bodies = new List<string>(5);

		// Token: 0x0400086D RID: 2157
		private int _renderingStep;

		// Token: 0x0400086E RID: 2158
		private int? _nextSerial;

		// Token: 0x0400086F RID: 2159
		private int? _lastSerial;

		// Token: 0x0200092E RID: 2350
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x04001267 RID: 4711
			public static Func<PXResult<ALModelExpr>, bool> <0>__CheckHasIterator;
		}
	}
}
