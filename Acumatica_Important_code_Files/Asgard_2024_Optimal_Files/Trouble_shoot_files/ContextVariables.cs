using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using AA.Objects.AL.Language;
using PX.Data;
using PX.Objects.CR;
using PX.SM;
using Scriban;
using Scriban.Runtime;

namespace AA.Objects.AL
{
	// Token: 0x0200010B RID: 267
	public class ContextVariables : ScriptObject, ISubstitution
	{
		// Token: 0x17000332 RID: 818
		// (get) Token: 0x0600088B RID: 2187 RVA: 0x0001F59B File Offset: 0x0001D79B
		public string Prefix
		{
			get
			{
				return "ctx";
			}
		}

		// Token: 0x17000333 RID: 819
		// (get) Token: 0x0600088C RID: 2188 RVA: 0x0001F5A2 File Offset: 0x0001D7A2
		public ScriptMemberImportFlags ImportFlags
		{
			get
			{
				return ScriptMemberImportFlags.Method;
			}
		}

		// Token: 0x0600088D RID: 2189 RVA: 0x0001F5A8 File Offset: 0x0001D7A8
		public static IEnumerable<MethodInfo> GetExposedMethods()
		{
			MethodInfo[] methods = typeof(ContextVariables).GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Static | BindingFlags.Public);
			string me = MethodBase.GetCurrentMethod().Name;
			IEnumerable<MethodInfo> source = methods;
			Func<MethodInfo, bool> predicate;
			if ((predicate = ContextVariables.<>O.<0>__IsNoArg) == null)
			{
				predicate = (ContextVariables.<>O.<0>__IsNoArg = new Func<MethodInfo, bool>(ContextVariables.IsNoArg));
			}
			return (from mi in source.Where(predicate)
			where mi.Name != me
			select mi).ToArray<MethodInfo>();
		}

		// Token: 0x0600088E RID: 2190 RVA: 0x0001F61C File Offset: 0x0001D81C
		private static bool IsNoArg(MethodInfo mi)
		{
			ParameterInfo[] parameters = mi.GetParameters();
			int num = parameters.Length;
			return num == 0 || (num == 1 && parameters[0].ParameterType == typeof(TemplateContext));
		}

		// Token: 0x0600088F RID: 2191 RVA: 0x0001F660 File Offset: 0x0001D860
		public static PXGraph GetRowGraph(TemplateContext context)
		{
			return context.GetValue("AL_RowGraph", false, null);
		}

		// Token: 0x06000890 RID: 2192 RVA: 0x0001F680 File Offset: 0x0001D880
		public static object GetRow(TemplateContext context)
		{
			return context.GetValue("AL_Row", false, null);
		}

		// Token: 0x06000891 RID: 2193 RVA: 0x0001F6A0 File Offset: 0x0001D8A0
		public static object GetOldRow(TemplateContext context)
		{
			return context.GetValue("AL_OldRow", true, null);
		}

		// Token: 0x06000892 RID: 2194 RVA: 0x0001F6C0 File Offset: 0x0001D8C0
		public static ResultSetIterator GetDetailRows(TemplateContext context)
		{
			return context.GetValue("ALDetailRows", true, null);
		}

		// Token: 0x06000893 RID: 2195 RVA: 0x0001F6E4 File Offset: 0x0001D8E4
		public static void SetDetailRows(TemplateContext context, IPXResultset rows)
		{
			ResultSetIterator value = new ResultSetIterator(context, rows);
			context.SetValue("ALDetailRows", value);
		}

		// Token: 0x06000894 RID: 2196 RVA: 0x0001F708 File Offset: 0x0001D908
		public static object GetDetailRow(TemplateContext context)
		{
			ResultSetIterator value = context.GetValue("ALDetailRows", true, null);
			return (value != null) ? value.Row : null;
		}

		// Token: 0x06000895 RID: 2197 RVA: 0x0001F734 File Offset: 0x0001D934
		public static bool HasIterator(TemplateContext context)
		{
			return context.GetValue("AL_HasIterator", true, false);
		}

		// Token: 0x06000896 RID: 2198 RVA: 0x0001F755 File Offset: 0x0001D955
		internal static void SetHasIterator(TemplateContext context, bool hasIterator)
		{
			context.SetValue("AL_HasIterator", hasIterator);
		}

		// Token: 0x06000897 RID: 2199 RVA: 0x0001F76C File Offset: 0x0001D96C
		public static bool HasIteratorByPage(TemplateContext context)
		{
			LabelContext labelContext = ContextVariables.GetLabelContext(context);
			bool flag = !ContextVariables.HasIterator(context);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				IteratorContext iteratorContext = labelContext.GetIteratorContext(null, null);
				bool flag2 = iteratorContext != null && iteratorContext.IsByPage;
				result = flag2;
			}
			return result;
		}

		// Token: 0x06000898 RID: 2200 RVA: 0x0001F7B1 File Offset: 0x0001D9B1
		public static void SetIteratorTotalRowCount(TemplateContext context, int nbRows)
		{
			context.SetValue("AL_IteratorTotalRowCount", nbRows);
		}

		// Token: 0x06000899 RID: 2201 RVA: 0x0001F7C8 File Offset: 0x0001D9C8
		public static int GetIteratorTotalRowCount(TemplateContext context)
		{
			return context.GetValue("AL_IteratorTotalRowCount", true, -1);
		}

		// Token: 0x0600089A RID: 2202 RVA: 0x0001F7E8 File Offset: 0x0001D9E8
		public static bool HasIteratorRows(TemplateContext context)
		{
			return ContextVariables.GetIteratorTotalRowCount(context) > 0;
		}

		// Token: 0x0600089B RID: 2203 RVA: 0x0001F803 File Offset: 0x0001DA03
		public static void SetIteratorRowNbr(TemplateContext context, int rowNumber)
		{
			context.SetValue("AL_IteratorRowNbr", rowNumber);
		}

		// Token: 0x0600089C RID: 2204 RVA: 0x0001F818 File Offset: 0x0001DA18
		public static int GetIteratorRowNbr(TemplateContext context)
		{
			return context.GetValue("AL_IteratorRowNbr", true, -1);
		}

		// Token: 0x0600089D RID: 2205 RVA: 0x0001F838 File Offset: 0x0001DA38
		public static int GetIteratorPageSize(TemplateContext context)
		{
			return context.GetValue("AL_IteratorPageSize", true, -1);
		}

		// Token: 0x0600089E RID: 2206 RVA: 0x0001F857 File Offset: 0x0001DA57
		public static void SetIteratorPageSize(TemplateContext context, int pageSize)
		{
			context.SetValue("AL_IteratorPageSize", pageSize);
		}

		// Token: 0x0600089F RID: 2207 RVA: 0x0001F86C File Offset: 0x0001DA6C
		public static int GetIteratorNbPages(TemplateContext context)
		{
			return context.GetValue("AL_IteratorNbPages", true, -1);
		}

		// Token: 0x060008A0 RID: 2208 RVA: 0x0001F88B File Offset: 0x0001DA8B
		public static void SetIteratorNbPages(TemplateContext context, int nbRows)
		{
			context.SetValue("AL_IteratorNbPages", nbRows);
		}

		// Token: 0x060008A1 RID: 2209 RVA: 0x0001F8A0 File Offset: 0x0001DAA0
		public static int GetIteratorPageNbr(TemplateContext context)
		{
			return context.GetValue("AL_IteratorPageNbr", true, -1);
		}

		// Token: 0x060008A2 RID: 2210 RVA: 0x0001F8BF File Offset: 0x0001DABF
		public static void SetIteratorPageNbr(TemplateContext context, int nbRows)
		{
			context.SetValue("AL_IteratorPageNbr", nbRows);
		}

		// Token: 0x060008A3 RID: 2211 RVA: 0x0001F8D4 File Offset: 0x0001DAD4
		public static int GetRowCount(TemplateContext context)
		{
			return context.GetValue("AL_RowCount", true, 0);
		}

		// Token: 0x060008A4 RID: 2212 RVA: 0x0001F8F3 File Offset: 0x0001DAF3
		public static void SetRowCount(TemplateContext context, int rowCount)
		{
			context.SetValue("AL_RowCount", rowCount);
		}

		// Token: 0x060008A5 RID: 2213 RVA: 0x0001F908 File Offset: 0x0001DB08
		public static int GetLabelCount(TemplateContext context)
		{
			return context.GetValue("AL_LabelCount", true, 0);
		}

		// Token: 0x060008A6 RID: 2214 RVA: 0x0001F927 File Offset: 0x0001DB27
		public static void SetLabelCount(TemplateContext context, int labelCount)
		{
			context.SetValue("AL_LabelCount", labelCount);
		}

		// Token: 0x060008A7 RID: 2215 RVA: 0x0001F93C File Offset: 0x0001DB3C
		public static LabelContext GetLabelContext(TemplateContext context)
		{
			return context.GetValue(true);
		}

		// Token: 0x060008A8 RID: 2216 RVA: 0x0001F958 File Offset: 0x0001DB58
		public static ALModel GetModel(TemplateContext context)
		{
			return context.GetValue(true);
		}

		// Token: 0x060008A9 RID: 2217 RVA: 0x0001F974 File Offset: 0x0001DB74
		public static IFormat GetModelFormat(TemplateContext context)
		{
			LabelContext labelContext = ContextVariables.GetLabelContext(context);
			return (labelContext != null) ? labelContext.ModelFormat : null;
		}

		// Token: 0x060008AA RID: 2218 RVA: 0x0001F99C File Offset: 0x0001DB9C
		public static IMargin GetMargin(TemplateContext context)
		{
			return context.GetValue(true);
		}

		// Token: 0x060008AB RID: 2219 RVA: 0x0001F9B8 File Offset: 0x0001DBB8
		public static Guid? GetUserID(TemplateContext context)
		{
			LabelContext labelContext = ContextVariables.GetLabelContext(context);
			return (labelContext != null) ? new Guid?(labelContext.UserID) : null;
		}

		// Token: 0x060008AC RID: 2220 RVA: 0x0001F9EC File Offset: 0x0001DBEC
		public static Users GetUser(TemplateContext context)
		{
			LabelContext labelContext = ContextVariables.GetLabelContext(context);
			return (labelContext != null) ? labelContext.User : null;
		}

		// Token: 0x060008AD RID: 2221 RVA: 0x0001FA14 File Offset: 0x0001DC14
		public static Guid? GetPrintStationID(TemplateContext context)
		{
			LabelContext labelContext = ContextVariables.GetLabelContext(context);
			return (labelContext != null) ? labelContext.PrintStationID : null;
		}

		// Token: 0x060008AE RID: 2222 RVA: 0x0001FA44 File Offset: 0x0001DC44
		public static CREmployee GetOwner(TemplateContext context)
		{
			LabelContext labelContext = ContextVariables.GetLabelContext(context);
			return (labelContext != null) ? labelContext.Owner : null;
		}

		// Token: 0x060008AF RID: 2223 RVA: 0x0001FA6C File Offset: 0x0001DC6C
		public static int? GetOwnerID(TemplateContext context)
		{
			LabelContext labelContext = ContextVariables.GetLabelContext(context);
			return (labelContext != null) ? labelContext.OwnerID : null;
		}

		// Token: 0x060008B0 RID: 2224 RVA: 0x0001FA9C File Offset: 0x0001DC9C
		public static ALModelPrinter GetUserPrinter(TemplateContext context)
		{
			return context.GetValue(true);
		}

		// Token: 0x060008B1 RID: 2225 RVA: 0x0001FAB8 File Offset: 0x0001DCB8
		public static IPrinter GetPrinter(TemplateContext context)
		{
			LabelContext labelContext = ContextVariables.GetLabelContext(context);
			return (labelContext != null) ? labelContext.Printer : null;
		}

		// Token: 0x060008B2 RID: 2226 RVA: 0x0001FAE0 File Offset: 0x0001DCE0
		public static IFormat GetPrinterFormat(TemplateContext context)
		{
			LabelContext labelContext = ContextVariables.GetLabelContext(context);
			return (labelContext != null) ? labelContext.PrinterFormat : null;
		}

		// Token: 0x060008B3 RID: 2227 RVA: 0x0001FB08 File Offset: 0x0001DD08
		public static IMargin GetPrinterMargin(TemplateContext context)
		{
			LabelContext labelContext = ContextVariables.GetLabelContext(context);
			return (labelContext != null) ? labelContext.PrinterMargin : null;
		}

		// Token: 0x060008B4 RID: 2228 RVA: 0x0001FB30 File Offset: 0x0001DD30
		public static IPrinterLanguage GetPrinterLanguage(TemplateContext context)
		{
			LabelContext labelContext = ContextVariables.GetLabelContext(context);
			return (labelContext != null) ? labelContext.PrinterLanguage : null;
		}

		// Token: 0x060008B5 RID: 2229 RVA: 0x0001FB58 File Offset: 0x0001DD58
		public static string GetLanguageCode(TemplateContext context)
		{
			LabelContext labelContext = ContextVariables.GetLabelContext(context);
			return (labelContext != null) ? labelContext.LanguageCode : null;
		}

		// Token: 0x060008B6 RID: 2230 RVA: 0x0001FB80 File Offset: 0x0001DD80
		public static int? GetBAccountID(TemplateContext context)
		{
			LabelContext labelContext = ContextVariables.GetLabelContext(context);
			return (labelContext != null) ? labelContext.BAccountID : null;
		}

		// Token: 0x060008B7 RID: 2231 RVA: 0x0001FBB0 File Offset: 0x0001DDB0
		public static OutputFormat FinalOutputFormat(TemplateContext context)
		{
			LabelContext labelContext = ContextVariables.GetLabelContext(context);
			return (labelContext != null) ? labelContext.FinalOutputFormat : OutputFormat.Unknown;
		}

		// Token: 0x060008B8 RID: 2232 RVA: 0x0001FBD8 File Offset: 0x0001DDD8
		public static IPdfOptions PdfOptions(TemplateContext context)
		{
			LabelContext labelContext = ContextVariables.GetLabelContext(context);
			return (labelContext != null) ? labelContext.PdfOptions : null;
		}

		// Token: 0x060008B9 RID: 2233 RVA: 0x0001FC00 File Offset: 0x0001DE00
		public static object SingleRow(TemplateContext context)
		{
			LabelContext labelContext = ContextVariables.GetLabelContext(context);
			return (labelContext != null) ? labelContext.SingleRow : null;
		}

		// Token: 0x060008BA RID: 2234 RVA: 0x0001FC28 File Offset: 0x0001DE28
		public bool SendPause(TemplateContext context)
		{
			LabelContext labelContext = ContextVariables.GetLabelContext(context);
			return labelContext != null && labelContext.SendPause;
		}

		// Token: 0x060008BB RID: 2235 RVA: 0x0001FC50 File Offset: 0x0001DE50
		public static bool IsRendered(TemplateContext context)
		{
			LabelContext labelContext = ContextVariables.GetLabelContext(context);
			return labelContext != null && labelContext.IsRendered;
		}

		// Token: 0x060008BC RID: 2236 RVA: 0x0001FC78 File Offset: 0x0001DE78
		public static bool IsCreatedFromSession(TemplateContext context)
		{
			LabelContext labelContext = ContextVariables.GetLabelContext(context);
			return labelContext != null && labelContext.IsCreatedFromSession;
		}

		// Token: 0x060008BD RID: 2237 RVA: 0x0001FCA0 File Offset: 0x0001DEA0
		public static bool IsImport(TemplateContext context)
		{
			LabelContext labelContext = ContextVariables.GetLabelContext(context);
			return labelContext != null && labelContext.IsImport;
		}

		// Token: 0x060008BE RID: 2238 RVA: 0x0001FCC8 File Offset: 0x0001DEC8
		public static bool IsExport(TemplateContext context)
		{
			LabelContext labelContext = ContextVariables.GetLabelContext(context);
			return labelContext != null && labelContext.IsExport;
		}

		// Token: 0x060008BF RID: 2239 RVA: 0x0001FCF0 File Offset: 0x0001DEF0
		public static bool IsMobile(TemplateContext context)
		{
			LabelContext labelContext = ContextVariables.GetLabelContext(context);
			return labelContext != null && labelContext.IsMobile;
		}

		// Token: 0x060008C0 RID: 2240 RVA: 0x0001FD18 File Offset: 0x0001DF18
		public static bool IsContractBasedAPI(TemplateContext context)
		{
			LabelContext labelContext = ContextVariables.GetLabelContext(context);
			return labelContext != null && labelContext.IsContractBasedAPI;
		}

		// Token: 0x060008C1 RID: 2241 RVA: 0x0001FD40 File Offset: 0x0001DF40
		public static bool IsSingleRow(TemplateContext context)
		{
			LabelContext labelContext = ContextVariables.GetLabelContext(context);
			return ((labelContext != null) ? labelContext.SingleRow : null) != null;
		}

		// Token: 0x060008C2 RID: 2242 RVA: 0x0001FD68 File Offset: 0x0001DF68
		public static bool IsRaw(TemplateContext context)
		{
			LabelContext labelContext = ContextVariables.GetLabelContext(context);
			return labelContext != null && labelContext.IsRaw;
		}

		// Token: 0x060008C3 RID: 2243 RVA: 0x0001FD90 File Offset: 0x0001DF90
		public static bool IsDevMode(TemplateContext context)
		{
			LabelContext labelContext = ContextVariables.GetLabelContext(context);
			return labelContext != null && labelContext.IsDevMode;
		}

		// Token: 0x060008C4 RID: 2244 RVA: 0x0001FDB8 File Offset: 0x0001DFB8
		public static bool IsSaveRendered(TemplateContext context)
		{
			LabelContext labelContext = ContextVariables.GetLabelContext(context);
			return labelContext != null && labelContext.IsSaveRendered;
		}

		// Token: 0x060008C5 RID: 2245 RVA: 0x0001FDE0 File Offset: 0x0001DFE0
		public static bool IsSnippet(TemplateContext context)
		{
			LabelContext labelContext = ContextVariables.GetLabelContext(context);
			return labelContext != null && labelContext.IsSnippet;
		}

		// Token: 0x060008C6 RID: 2246 RVA: 0x0001FE08 File Offset: 0x0001E008
		public static bool IsRender(TemplateContext context)
		{
			LabelContext labelContext = ContextVariables.GetLabelContext(context);
			return labelContext != null && labelContext.IsRender;
		}

		// Token: 0x060008C7 RID: 2247 RVA: 0x0001FE30 File Offset: 0x0001E030
		public static bool IsSilent(TemplateContext context)
		{
			LabelContext labelContext = ContextVariables.GetLabelContext(context);
			return labelContext != null && labelContext.IsSilent;
		}

		// Token: 0x060008C8 RID: 2248 RVA: 0x0001FE58 File Offset: 0x0001E058
		public static bool IsAlwaysPrint(TemplateContext context)
		{
			LabelContext labelContext = ContextVariables.GetLabelContext(context);
			return labelContext != null && labelContext.IsAlwaysPrint;
		}

		// Token: 0x060008C9 RID: 2249 RVA: 0x0001FE80 File Offset: 0x0001E080
		public static bool IsDesignMode(TemplateContext context)
		{
			LabelContext labelContext = ContextVariables.GetLabelContext(context);
			return labelContext != null && labelContext.IsDesignMode;
		}

		// Token: 0x060008CA RID: 2250 RVA: 0x0001FEA8 File Offset: 0x0001E0A8
		public static bool IsDealingMode(TemplateContext context)
		{
			LabelContext labelContext = ContextVariables.GetLabelContext(context);
			return labelContext != null && labelContext.DealingMode;
		}

		// Token: 0x060008CB RID: 2251 RVA: 0x0001FED0 File Offset: 0x0001E0D0
		public static bool IsGI(TemplateContext context)
		{
			LabelContext labelContext = ContextVariables.GetLabelContext(context);
			return labelContext != null && labelContext.IsGI;
		}

		// Token: 0x060008CC RID: 2252 RVA: 0x0001FEF8 File Offset: 0x0001E0F8
		public static Layout GetLayout(TemplateContext context)
		{
			Layout layout = context.GetValue(true);
			bool flag = layout == null;
			if (flag)
			{
				layout = new Layout(context);
				ContextVariables.SetLayout(context, layout);
			}
			return layout;
		}

		// Token: 0x060008CD RID: 2253 RVA: 0x0001FF2C File Offset: 0x0001E12C
		public static void SetLayout(TemplateContext context, Layout layout)
		{
			context.CurrentGlobal.SetValue(layout);
		}

		// Token: 0x060008CE RID: 2254 RVA: 0x0001FF3C File Offset: 0x0001E13C
		public static int GetNbCopies(TemplateContext context)
		{
			LabelContext labelContext = ContextVariables.GetLabelContext(context);
			return (labelContext != null) ? labelContext.GetNbCopies() : 1;
		}

		// Token: 0x060008CF RID: 2255 RVA: 0x0001FF64 File Offset: 0x0001E164
		public static int GetDealingCount(TemplateContext context)
		{
			LabelContext labelContext = ContextVariables.GetLabelContext(context);
			return (labelContext != null) ? labelContext.GetDealingCount() : 0;
		}

		// Token: 0x060008D0 RID: 2256 RVA: 0x0001FF8C File Offset: 0x0001E18C
		public static int? GetNextSerial(TemplateContext context)
		{
			LabelContext labelContext = ContextVariables.GetLabelContext(context);
			return labelContext.GetNextSerial();
		}

		// Token: 0x060008D1 RID: 2257 RVA: 0x0001FFB0 File Offset: 0x0001E1B0
		public static int? PeekNextSerial(TemplateContext context)
		{
			LabelContext labelContext = ContextVariables.GetLabelContext(context);
			return labelContext.PeekNextSerial();
		}

		// Token: 0x060008D2 RID: 2258 RVA: 0x0001FFD1 File Offset: 0x0001E1D1
		public static void ResetIterator(TemplateContext context)
		{
			ContextVariables.SetIteratorPageNbr(context, -1);
			ContextVariables.SetIteratorNbPages(context, -1);
			ContextVariables.SetIteratorTotalRowCount(context, -1);
			ContextVariables.SetIteratorRowNbr(context, 0);
		}

		// Token: 0x04000311 RID: 785
		public const string PREFIX = "ctx";

		// Token: 0x04000312 RID: 786
		internal const string ROW_GRAPH = "AL_RowGraph";

		// Token: 0x04000313 RID: 787
		internal const string ROW = "AL_Row";

		// Token: 0x04000314 RID: 788
		internal const string OLD_ROW = "AL_OldRow";

		// Token: 0x04000315 RID: 789
		internal const string DETAIL_ROWS = "ALDetailRows";

		// Token: 0x04000316 RID: 790
		internal const string LINE_WRAP = "AL_LineWrap";

		// Token: 0x04000317 RID: 791
		internal const string LINE_WRAP_VALUE = "\\&";

		// Token: 0x04000318 RID: 792
		internal const string ROW_ITERATOR = "AL_RowIterator";

		// Token: 0x04000319 RID: 793
		internal const string PAGE_ITERATOR = "AL_PageIterator";

		// Token: 0x0400031A RID: 794
		private const string ITERATOR_TOTAL_ROW_COUNT = "AL_IteratorTotalRowCount";

		// Token: 0x0400031B RID: 795
		private const string ITERATOR_PAGE_SIZE = "AL_IteratorPageSize";

		// Token: 0x0400031C RID: 796
		private const string ITERATOR_NB_PAGES = "AL_IteratorNbPages";

		// Token: 0x0400031D RID: 797
		private const string ITERATOR_PAGE_NBR = "AL_IteratorPageNbr";

		// Token: 0x0400031E RID: 798
		private const string ITERATOR_ROW_NBR = "AL_IteratorRowNbr";

		// Token: 0x0400031F RID: 799
		private const string ITERATOR_HAS = "AL_HasIterator";

		// Token: 0x04000320 RID: 800
		private const string LABEL_COUNT = "AL_LabelCount";

		// Token: 0x04000321 RID: 801
		private const string ROW_COUNT = "AL_RowCount";

		// Token: 0x020005B8 RID: 1464
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x04000F75 RID: 3957
			public static Func<MethodInfo, bool> <0>__IsNoArg;
		}
	}
}
