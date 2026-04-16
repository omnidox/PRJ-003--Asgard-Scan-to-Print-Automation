using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AA.Objects.Core;
using AA.Objects.Labels.Integration;
using AA.Objects.Labels.Integration.NbCopies;
using Asgard.Labels.Abstractions.Interface;
using Asgard.Labels.Abstractions.Language;
using Asgard.Labels.Abstractions.Poco;
using Asgard.Labels.Impl.Context;
using Asgard.Labels.Impl.Poco;
using PX.Data;
using PX.Data.Description.GI;
using PX.Objects.IN;

namespace AA.Objects.Labels
{
	// Token: 0x0200004E RID: 78
	public class AcuLabelGenerator : ILabelGenerator<IAcuLabelContext>
	{
		// Token: 0x060002D3 RID: 723 RVA: 0x000140A8 File Offset: 0x000122A8
		public PrintResults PrintLabels(IAcuLabelContext labelContext)
		{
			PrintResults printResults = this.PrintLabelInternal(labelContext);
			string printMessage = ContextHelper.GetPrintMessage(printResults);
			bool flag = !labelContext.IsSilent;
			if (flag)
			{
				throw new PXOperationCompletedException(printMessage);
			}
			PXTrace.WriteInformation(printMessage);
			return printResults;
		}

		// Token: 0x060002D4 RID: 724 RVA: 0x000140E8 File Offset: 0x000122E8
		private PrintResults PrintLabelInternal(IAcuLabelContext lc)
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
				IEnumerable<IRenderableChild<Guid?>> children = lc.Children;
				foreach (IRenderableChild<Guid?> renderableChild in children)
				{
					AcuLabelContext lc2 = AcuLabelContext.CreateChildContext(lc, renderableChild.ChildID, false);
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
								PXResultset<ALModel> pxresultset = new PXResultset<ALModel>();
								pxresultset.Add(new PXResult<ALModel>(lc.Model));
								PXResultset<ALModel> pxresultset2 = pxresultset;
								basedOnResult2 = pxresultset2;
								type = lc.Model.GetType();
							}
							else
							{
								ViewDef viewDefinition = ViewUtils.GetViewDefinition(graph, basedOnView);
								bool flag6 = viewDefinition == null;
								if (flag6)
								{
									throw lc.GetException("Could not find a view named '{0}' on graph '{1}'", new object[]
									{
										basedOnView,
										graph.GetType()
									});
								}
								IViewResult viewRow = ViewUtils.GetViewRow(graph, viewDefinition);
								type = AsgardCoreUtils.GetItemType(viewRow);
								basedOnResult2 = viewRow.Result;
							}
						}
						PXCache detailCache = caches[type];
						PrintResults collection3 = this.ParseAndPrintMultiple(lc, detailCache, basedOnResult2);
						printResults.AddRange(collection3);
						bool mergeDetails = lc.MergeDetails;
						if (mergeDetails)
						{
							lc.EndMerge();
						}
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

		// Token: 0x060002D5 RID: 725 RVA: 0x000143B4 File Offset: 0x000125B4
		private PrintResults ParseAndPrintMultiple(IAcuLabelContext lc, PXCache detailCache, object basedOnResult)
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

		// Token: 0x060002D6 RID: 726 RVA: 0x00014518 File Offset: 0x00012718
		private PrintResults.PrintResult ParseAndPrint(IAcuLabelContext labelContext)
		{
			object row = labelContext.Row;
			bool flag = row == null;
			if (flag)
			{
				throw labelContext.GetException("A row is needed to print a label", Array.Empty<object>());
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
						if (!labelContext.IgnorePrinterMissing)
						{
							throw labelContext.GetException("A Model Printer must be defined for you for Model '{0}'", new object[]
							{
								labelContext.Model.Description
							});
						}
						result = PrintResults.EMPTY;
					}
					else
					{
						IPrinterLanguage printerLanguage = labelContext.PrinterLanguage;
						FileResult printResult = printerLanguage.GetPrintResult(labelContext, nbCopies);
						FileResult fileResult = labelContext.SaveFileToPrintLog(printResult, null);
						labelContext.Print(fileResult);
						int num = fileResult.NbCopies;
						try
						{
							while (labelContext.IteratorHasMorePages())
							{
								PrintResults.PrintResult printResult2 = this.ParseAndPrint(labelContext);
								num += printResult2.NbLabels;
							}
						}
						finally
						{
							labelContext.EndIterator();
						}
						this.UpdatePrinted(labelContext);
						result = new PrintResults.PrintResult(num, labelContext.Printer, labelContext.Model);
					}
				}
			}
			return result;
		}

		// Token: 0x060002D7 RID: 727 RVA: 0x00014670 File Offset: 0x00012870
		private void UpdatePrinted(IAcuLabelContext labelContext)
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
					Type itemType = AsgardCoreUtils.GetItemType(row);
					PXCache cache = graph.Caches[itemType];
					string keys = AsgardUtils.GetKeys(cache, row, ", ");
					labelContext.WriteError(labelContext.GetException(ex, "Cannot update row with key '{0}', error is {1}", new object[]
					{
						keys,
						ex.Message
					}));
				}
			}
		}

		// Token: 0x060002D8 RID: 728 RVA: 0x00014714 File Offset: 0x00012914
		private bool CheckDoPrint(IAcuLabelContext lc)
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
