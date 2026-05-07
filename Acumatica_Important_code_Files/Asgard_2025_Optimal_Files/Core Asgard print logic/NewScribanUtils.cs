using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Asgard.Labels.Abstractions.Context;
using Asgard.Labels.Abstractions.Helpers;
using Asgard.Labels.Abstractions.Interface;
using Asgard.Labels.Impl.Context;
using Scriban;
using Scriban.Parsing;
using Scriban.Runtime;

namespace Asgard.Labels.Impl.Language.MyScriban
{
	// Token: 0x020000A7 RID: 167
	public static class NewScribanUtils
	{
		// Token: 0x17000140 RID: 320
		// (get) Token: 0x060004D5 RID: 1237 RVA: 0x00012B5A File Offset: 0x00010D5A
		public static MemberRenamerDelegate PascalCase
		{
			get
			{
				return (MemberInfo m) => m.Name;
			}
		}

		// Token: 0x060004D6 RID: 1238 RVA: 0x00012B7C File Offset: 0x00010D7C
		public static void SetGlobalValues(this TemplateContext templateContext, params object[] contextValues)
		{
			foreach (object value in contextValues)
			{
				templateContext.SetValue(value);
			}
		}

		// Token: 0x060004D7 RID: 1239 RVA: 0x00012BAC File Offset: 0x00010DAC
		public static void CheckTemplateErrors(this TemplateContext templateContext, string errorType, string name, Template template)
		{
			bool hasErrors = template.HasErrors;
			if (hasErrors)
			{
				ILabelContext labelContext = ContextVariables.GetLabelContext(templateContext);
				NewScribanUtils.ShowErrorLines(labelContext, template);
				string text = "\n\t" + string.Join("\n\t", from mess in template.Messages
				select mess.ToString());
				throw labelContext.GetException("Template for '{0}':'{1}' has errors: {2}", new object[]
				{
					errorType,
					name,
					text
				});
			}
		}

		// Token: 0x060004D8 RID: 1240 RVA: 0x00012C34 File Offset: 0x00010E34
		private static void ShowErrorLines(ILabelContext lc, Template template)
		{
			if (lc != null)
			{
				lc.WriteError(template.Page.ToString(), Array.Empty<object>());
			}
			LogMessageBag messages = template.Messages;
			foreach (LogMessage logMessage in messages)
			{
				if (lc != null)
				{
					lc.WriteError(logMessage.ToString(), Array.Empty<object>());
				}
			}
		}

		// Token: 0x060004D9 RID: 1241 RVA: 0x00012CB4 File Offset: 0x00010EB4
		public static T EvalExpr<T>(object obj, string scribanExpr, T defaultValue = default(T))
		{
			bool flag = obj != null;
			T result;
			if (flag)
			{
				BaseTemplateContext baseTemplateContext = new BaseTemplateContext();
				baseTemplateContext.SetValue("SINGLE", obj);
				T t = NewScribanUtils.EvalExpr<T>(baseTemplateContext, "SINGLE." + scribanExpr, default(T));
				result = t;
			}
			else
			{
				result = defaultValue;
			}
			return result;
		}

		// Token: 0x060004DA RID: 1242 RVA: 0x00012D08 File Offset: 0x00010F08
		public static T EvalExpr<T>(IRuleEvalContext context, string scribanExpr, T defaultValue = default(T))
		{
			return NewScribanUtils.EvalExpr<T>(context.ScribanContext, scribanExpr, defaultValue);
		}

		// Token: 0x060004DB RID: 1243 RVA: 0x00012D28 File Offset: 0x00010F28
		public static T EvalExpr<T>(TemplateContext scribanContext, string scribanExpr, T defaultValue = default(T))
		{
			bool flag = string.IsNullOrEmpty(scribanExpr);
			T result;
			if (flag)
			{
				result = defaultValue;
			}
			else
			{
				Type typeFromHandle = typeof(T);
				scribanExpr = scribanExpr.ToScriban();
				Template template = Template.Parse(scribanExpr, null, null, null);
				scribanContext.CheckTemplateErrors("Expression", scribanExpr, template);
				object value = template.Evaluate(scribanContext);
				try
				{
					object obj = BasicHelper.ChangeType(value, typeFromHandle, null);
					T t2;
					if (obj is T)
					{
						T t = (T)((object)obj);
						t2 = t;
					}
					else
					{
						t2 = defaultValue;
					}
					result = t2;
				}
				catch (Exception ex)
				{
					AATrace.WriteError(ex);
					throw new AAException(ex, "You are trying to convert expression '{0}' to type '{1}' but it is not possible", new object[]
					{
						scribanExpr,
						typeFromHandle.Name
					});
				}
			}
			return result;
		}

		// Token: 0x060004DC RID: 1244 RVA: 0x00012DF4 File Offset: 0x00010FF4
		public static TemplateContext CreateTestContext(params object[] contextValues)
		{
			BaseTemplateContext baseTemplateContext = new BaseTemplateContext
			{
				DevMode = true
			};
			ScriptObject scriptObject = new ScriptObject();
			foreach (object value in contextValues)
			{
				scriptObject.SetValue(value);
			}
			baseTemplateContext.PushGlobal(scriptObject);
			NewScribanUtils.LoadLibraries(scriptObject);
			return baseTemplateContext;
		}

		// Token: 0x060004DD RID: 1245 RVA: 0x00012E50 File Offset: 0x00011050
		public static void LoadLibraries(IScriptObject container)
		{
			IEnumerable<Type> implementations = BasicHelper.GetImplementations(typeof(IScribanLib), false);
			foreach (Type type in implementations)
			{
				ValueTuple<string, ScriptMemberImportFlags> libInfo = BasicHelper.GetLibInfo(type);
				string item = libInfo.Item1;
				ScriptMemberImportFlags item2 = libInfo.Item2;
				bool flag = string.IsNullOrEmpty(item);
				if (!flag)
				{
					IEnumerable<ScriptMemberImportFlags> enumerable = BasicHelper.ExplodeFlags<ScriptMemberImportFlags>(item2);
					foreach (ScriptMemberImportFlags scriptMemberImportFlags in enumerable)
					{
						MemberRenamerDelegate renamer = (scriptMemberImportFlags == ScriptMemberImportFlags.Method) ? NewScribanUtils.PascalCase : null;
						NewScribanUtils.Import(type, container, item, scriptMemberImportFlags, null, renamer);
					}
				}
			}
		}

		// Token: 0x060004DE RID: 1246 RVA: 0x00012F34 File Offset: 0x00011134
		public static void Import<FuncLib>(IScriptObject container, ScriptMemberImportFlags flags, MemberFilterDelegate filter = null, MemberRenamerDelegate renamer = null)
		{
			string name = typeof(FuncLib).Name;
			NewScribanUtils.Import<FuncLib>(container, name, flags, filter, renamer);
		}

		// Token: 0x060004DF RID: 1247 RVA: 0x00012F5D File Offset: 0x0001115D
		public static void Import<FuncLib>(IScriptObject container, string name, ScriptMemberImportFlags flags, MemberFilterDelegate filter = null, MemberRenamerDelegate renamer = null)
		{
			NewScribanUtils.Import(typeof(FuncLib), container, name, flags, filter, renamer);
		}

		// Token: 0x060004E0 RID: 1248 RVA: 0x00012F78 File Offset: 0x00011178
		public static void Import(Type libType, IScriptObject container, string name, ScriptMemberImportFlags flags, MemberFilterDelegate filter = null, MemberRenamerDelegate renamer = null)
		{
			ScriptObject scriptObject = new ScriptObject();
			container.SetValue(name, scriptObject, true);
			scriptObject.Import(libType, flags, filter, renamer);
		}

		// Token: 0x060004E1 RID: 1249 RVA: 0x00012FA4 File Offset: 0x000111A4
		public static IEnumerable<MethodInfo> GetExposedMethods<L>(L lib) where L : IScribanLib, IScriptObject
		{
			Type type = lib.GetType();
			MethodInfo[] methods = type.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Static | BindingFlags.Public);
			string me = MethodBase.GetCurrentMethod().Name;
			IEnumerable<MethodInfo> source = methods;
			Func<MethodInfo, bool> predicate;
			if ((predicate = NewScribanUtils.<>O.<0>__IsNoArg) == null)
			{
				predicate = (NewScribanUtils.<>O.<0>__IsNoArg = new Func<MethodInfo, bool>(NewScribanUtils.IsNoArg));
			}
			return (from mi in source.Where(predicate)
			where mi.Name != me
			select mi).ToArray<MethodInfo>();
		}

		// Token: 0x060004E2 RID: 1250 RVA: 0x0001301C File Offset: 0x0001121C
		private static bool IsNoArg(MethodInfo mi)
		{
			ParameterInfo[] parameters = mi.GetParameters();
			int num = parameters.Length;
			return num == 0 || (num == 1 && parameters[0].ParameterType == typeof(TemplateContext));
		}

		// Token: 0x02000126 RID: 294
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x0400036A RID: 874
			public static Func<MethodInfo, bool> <0>__IsNoArg;
		}
	}
}
