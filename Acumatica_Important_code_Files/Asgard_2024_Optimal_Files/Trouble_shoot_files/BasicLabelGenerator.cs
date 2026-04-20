using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AA.Objects.AL.Integration;
using AA.Objects.AL.Integration.NbCopies;
using AA.Objects.AL.Language;
using PX.Data;
using PX.Data.Description.GI;
using PX.Objects.IN;
using Scriban;

namespace AA.Objects.AL
{
	// Token: 0x020001A5 RID: 421
	public class BasicLabelGenerator : ILabelGenerator
	{
		// Token: 0x06000CE9 RID: 3305 RVA: 0x000333F0 File Offset: 0x000315F0
		public PrintResults PrintLabels(LabelContext labelContext)
		{
			labelContext.Check();
			PrintResults printResults = this.PrintLabelInternal(labelContext);
			labelContext.UpdateFeatureConsumption(typeof(ILabelGenerator), printResults.NbLabels);
			string printMessage = BasicLabelGenerator.GetPrintMessage(printResults);
			bool flag = !labelContext.IsSilent;
			if (flag)
			{
				throw new PXOperationCompletedException(printMessage);
			}
			PXTrace.WriteInformation(printMessage);
			return printResults;
		}

		// Token: 0x06000CEA RID: 3306 RVA: 0x00033450 File Offset: 0x00031650
		public static string GetPrintMessage(PrintResults printResults)
		{
			PrintResults summary = printResults.GetSummary();
			bool flag = summary.NbLabels > 0;
			string result;
			if (flag)
			{
				result = string.Format("Labels generated:\n\n{0}", string.Join<PrintResults.PrintResult>("\n", summary));
			}
			else
			{
				result = "No labels generated!";
			}
			return result;
		}

		// Token: 0x06000CEB RID: 3307 RVA: 0x0003349C File Offset: 0x0003169C
		private PrintResults PrintLabelInternal(LabelContext lc)
		{
			ALModel model = lc.Model;
			PXGraph graph = lc.Graph;
			PXCacheCollection caches = graph.Caches;
			lc.SaveRendered(null);
			PrintResults printResults = new PrintResults();
			bool flag = model.ModelType == "G";
			PrintResults result;
			if (flag)
			{
				PXResultset<ALModelChild> pxresultset = lc.LabelGraph.Children.Select(Array.Empty<object>());
				foreach (PXResult<ALModelChild> pxresult in pxresultset)
				{
					ALModelChild almodelChild = pxresult;
					LabelContext lc2 = LabelContext.CreateChildContext(lc, almodelChild.LabelChildID, false);
					PrintResults collection = this.PrintLabelInternal(lc2);
					printResults.AddRange(collection);
				}
				result = printResults;
			}
			else
			{
				string basedOnView = model.BasedOnView;
				bool flag2 = basedOnView != graph.PrimaryView;
				if (flag2)
				{
					PXGenericInqGrph pxgenericInqGrph = graph as PXGenericInqGrph;
					bool flag3 = pxgenericInqGrph != null;
					if (flag3)
					{
						string screenID = lc.Model.ScreenID;
						pxgenericInqGrph.PrepareCaches(HiddenUtils.GetGenericInquiryIDByScreenID(screenID).Value.ToString(), null, true);
						PXQueryDescription baseQueryDescription = pxgenericInqGrph.BaseQueryDescription;
						PXQueryDescription currentQueryDescription = HiddenUtils.GetCurrentQueryDescription(pxgenericInqGrph);
						PXQueryDescription pxqueryDescription = currentQueryDescription ?? baseQueryDescription;
						GenericResult[] basedOnResult = pxgenericInqGrph.DoSelect(pxqueryDescription, PXView.StartRow, PXView.MaximumRows).ToArray<GenericResult>();
						GenericResultCache cache = pxgenericInqGrph.Results.Cache;
						PrintResults collection2 = this.ParseAndPrintMultiple(lc, cache, basedOnResult);
						printResults.AddRange(collection2);
					}
					else
					{
						object singleRow = lc.SingleRow;
						bool flag4 = singleRow != null;
						object basedOnResult2;
						Type type;
						if (flag4)
						{
							basedOnResult2 = singleRow;
							IPXResultset ipxresultset = singleRow as IPXResultset;
							bool flag5 = ipxresultset != null;
							if (flag5)
							{
								type = ipxresultset.GetItemType(0);
							}
							else
							{
								type = singleRow.GetType();
								basedOnResult2 = new List<object>
								{
									singleRow
								};
							}
						}
						else
						{
							bool isDesignMode = lc.IsDesignMode;
							if (isDesignMode)
							{
								PXResultset<ALModel> pxresultset2 = new PXResultset<ALModel>();
								pxresultset2.Add(new PXResult<ALModel>(lc.Model));
								PXResultset<ALModel> pxresultset3 = pxresultset2;
								basedOnResult2 = pxresultset3;
								type = lc.Model.GetType();
							}
							else
							{
								ViewDef viewDefinition = ViewUtils.GetViewDefinition(graph, basedOnView);
								bool flag6 = viewDefinition == null;
								if (flag6)
								{
									throw new PXException("Could not find a view named '{0}' on graph '{1}'", new object[]
									{
										basedOnView,
										graph.GetType()
									});
								}
								IViewResult viewRow = ViewUtils.GetViewRow(graph, viewDefinition);
								type = AsgardUtils.GetItemType(viewRow);
								basedOnResult2 = viewRow.Result;
							}
						}
						PXCache detailCache = caches[type];
						PrintResults collection3 = this.ParseAndPrintMultiple(lc, detailCache, basedOnResult2);
						printResults.AddRange(collection3);
					}
				}
				else
				{
					PrintResults.PrintResult item = this.ParseAndPrint(lc);
					printResults.Add(item);
				}
				result = printResults;
			}
			return result;
		}

		// Token: 0x06000CEC RID: 3308 RVA: 0x00033768 File Offset: 0x00031968
		public virtual PrintResults ParseAndPrintMultiple(LabelContext lc, PXCache detailCache, object basedOnResult)
		{
			IPXResultset asResultset = AsgardUtils.GetAsResultset(basedOnResult);
			IList list = (IList)asResultset.GetCollection();
			lc.DetailRows = asResultset;
			PrintResults printResults = new PrintResults();
			bool flag = list != null;
			if (flag)
			{
				int count = list.Count;
				bool flag2 = count > 0;
				if (flag2)
				{
					for (int i = 0; i < count; i++)
					{
						object obj = list[i];
						IBqlTable bqlTable = PXResult.UnwrapMain(obj);
						detailCache.Current = bqlTable;
						ILSDetail ilsdetail = bqlTable as ILSDetail;
						PXResult pxresult;
						bool flag3;
						if (ilsdetail != null)
						{
							pxresult = (obj as PXResult);
							flag3 = (pxresult != null);
						}
						else
						{
							flag3 = false;
						}
						bool flag4 = flag3;
						if (flag4)
						{
							IBqlTable[] results = pxresult.GetResults();
							ILSMaster ilsmaster = (from row in results
							where row is ILSMaster && !(row is ILSDetail)
							select row).Cast<ILSMaster>().FirstOrDefault<ILSMaster>();
							bool flag5 = ilsmaster != null;
							if (flag5)
							{
								PXCache pxcache = detailCache.Graph.Caches[ilsmaster.GetType()];
								bool flag6 = pxcache != null && pxcache.Current != ilsmaster;
								if (flag6)
								{
									pxcache.Current = ilsmaster;
								}
							}
						}
						lc.DetailRow = obj;
						PrintResults.PrintResult item = this.ParseAndPrint(lc);
						printResults.Add(item);
					}
				}
			}
			return printResults;
		}

		// Token: 0x06000CED RID: 3309 RVA: 0x000338CC File Offset: 0x00031ACC
		protected virtual PrintResults.PrintResult ParseAndPrint(LabelContext labelContext)
		{
			object row = labelContext.Row;
			bool flag = row == null;
			if (flag)
			{
				throw new PXException("A row is needed to print a label");
			}
			ALModel model = labelContext.Model;
			bool flag2 = labelContext.IsAlwaysPrint || this.CheckDoPrint(labelContext);
			bool flag3 = !flag2;
			PrintResults.PrintResult result;
			if (flag3)
			{
				result = PrintResults.EMPTY;
			}
			else
			{
				int nbCopies = labelContext.GetNbCopies();
				bool flag4 = nbCopies == 0;
				if (flag4)
				{
					result = PrintResults.EMPTY;
				}
				else
				{
					bool flag5 = labelContext.Printer == null;
					if (flag5)
					{
						bool ignorePrinterMissing = labelContext.IgnorePrinterMissing;
						if (!ignorePrinterMissing)
						{
							throw new PXException("A Label Printer must be defined for you under Model '{0}'", new object[]
							{
								labelContext.Model.Description
							});
						}
						result = PrintResults.EMPTY;
					}
					else
					{
						IPrinterLanguage printerLanguage = labelContext.PrinterLanguage;
						string text = labelContext.GetRenderedTemplate();
						printerLanguage.HandleFonts(labelContext, text);
						TemplateContext scribanContext = labelContext.ScribanContext;
						int dealingCount = labelContext.GetDealingCount();
						int num = nbCopies;
						bool flag6 = labelContext.DealingMode && dealingCount > 0;
						if (flag6)
						{
							num *= dealingCount;
						}
						labelContext.CheckFeatureConsumption(typeof(ILabelGenerator), num);
						text = printerLanguage.SetNbCopies(labelContext, text, num, null, new int?(nbCopies), null);
						bool sendPause = labelContext.SendPause;
						if (sendPause)
						{
							text = printerLanguage.AddPause(labelContext, text);
						}
						Template template = Template.Parse(text, null, null, null);
						ScribanUtils.CheckTemplateErrors("Model", model.Name, template);
						string text2 = template.Render(scribanContext);
						labelContext.SaveRendered(text2);
						byte[] bytes = Encoding.Default.GetBytes(text2);
						FileResult printResult = new FileResult(bytes, new int?(num), null);
						FileResult printResult2 = labelContext.SaveFileToPrintLog(printResult, null);
						labelContext.Print(printResult2);
						int num2 = num;
						try
						{
							while (labelContext.IteratorHasMorePages())
							{
								PrintResults.PrintResult printResult3 = this.ParseAndPrint(labelContext);
								num2 += printResult3.NbLabels;
							}
						}
						finally
						{
							labelContext.EndIterator();
						}
						this.UpdatePrinted(labelContext);
						result = new PrintResults.PrintResult(num2, labelContext.Printer, labelContext.Model);
					}
				}
			}
			return result;
		}

		// Token: 0x06000CEE RID: 3310 RVA: 0x00033B14 File Offset: 0x00031D14
		private void UpdatePrinted(LabelContext labelContext)
		{
			object row = labelContext.Row;
			PXGraph graph = labelContext.Graph;
			ILabelUpdater labelUpdater;
			bool flag;
			if (row != null)
			{
				labelUpdater = (graph as ILabelUpdater);
				flag = (labelUpdater != null);
			}
			else
			{
				flag = false;
			}
			bool flag2 = flag;
			if (flag2)
			{
				try
				{
					labelUpdater.UpdatePrinted(row);
				}
				catch (Exception ex)
				{
					Type itemType = AsgardUtils.GetItemType(row);
					PXCache cache = graph.Caches[itemType];
					string keys = AsgardUtils.GetKeys(cache, row, ", ");
					PXTrace.WriteError(new PXException(ex, "Cannot update row with key '{0}', error is {1}", new object[]
					{
						keys,
						ex.Message
					}));
				}
			}
		}

		// Token: 0x06000CEF RID: 3311 RVA: 0x00033BB8 File Offset: 0x00031DB8
		protected virtual bool CheckDoPrint(LabelContext lc)
		{
			ALModel model = lc.Model;
			bool flag = true;
			bool flag2 = !lc.IsDesignMode;
			if (flag2)
			{
				flag = RuleUtils.EvalRule(lc, model.FilterRuleID, model.ReverseFilter.GetValueOrDefault());
				bool flag3 = flag;
				if (flag3)
				{
					flag = RuleUtils.EvalRule(lc, model.PrintRuleID, model.ReversePrint.GetValueOrDefault());
				}
			}
			bool flag4 = flag;
			if (flag4)
			{
				flag = NbCopiesHelper.CheckLineDoPrint(lc);
			}
			return flag;
		}
	}
}
