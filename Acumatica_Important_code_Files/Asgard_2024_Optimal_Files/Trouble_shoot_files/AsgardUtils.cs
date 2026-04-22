using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Compilation;
using System.Web.Security;
using Fasterflect;
using Fasterflect.Extensions;
using Newtonsoft.Json;
using PX.Api.Services;
using PX.Common;
using PX.Data;
using PX.Data.BQL;
using PX.Objects.CS;
using PX.PushNotifications;
using PX.SM;

namespace AA.Objects.AL
{
	// Token: 0x020001CC RID: 460
	public static class AsgardUtils
	{
		// Token: 0x0600121C RID: 4636 RVA: 0x0003C848 File Offset: 0x0003AA48
		public static PXLoginScope EnsureLogin()
		{
			string text = "admin";
			bool flag = PXDatabase.Companies.Length != 0;
			if (flag)
			{
				string text2 = PXAccess.GetCompanyName();
				bool flag2 = string.IsNullOrEmpty(text2);
				if (flag2)
				{
					text2 = PXDatabase.Companies[0];
				}
				text = text + "@" + text2;
			}
			return new PXLoginScope(text, PXAccess.GetAdministratorRoles());
		}

		// Token: 0x0600121D RID: 4637 RVA: 0x0003C8A8 File Offset: 0x0003AAA8
		public static bool IsFilteredResult(FieldInfo fi)
		{
			bool flag = fi == null;
			return !flag && (AsgardUtils.IsProcessingView(fi) || AsgardUtils.HasPXFilterable(fi));
		}

		// Token: 0x0600121E RID: 4638 RVA: 0x0003C8DC File Offset: 0x0003AADC
		public static bool IsSpecialArgument(this string argName)
		{
			return argName != null && argName.StartsWith("_") && argName.EndsWith("_");
		}

		// Token: 0x0600121F RID: 4639 RVA: 0x0003C90C File Offset: 0x0003AB0C
		public static bool IsProcessingView(FieldInfo fi)
		{
			bool flag = fi == null;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = fi.FieldType.Implements(typeof(IPXProcessing));
				result = flag2;
			}
			return result;
		}

		// Token: 0x06001220 RID: 4640 RVA: 0x0003C948 File Offset: 0x0003AB48
		public static bool HasPXFilterable(FieldInfo fi)
		{
			bool flag = fi == null;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = fi.GetCustomAttributes().Any((Attribute attr) => attr.GetType() == typeof(PXFilterableAttribute));
				result = flag2;
			}
			return result;
		}

		// Token: 0x06001221 RID: 4641 RVA: 0x0003C998 File Offset: 0x0003AB98
		public static bool IsPXFilter(FieldInfo fi)
		{
			bool flag = fi == null;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = fi.FieldType.IsCompatibleWith(typeof(PXFilter));
				result = flag2;
			}
			return result;
		}

		// Token: 0x06001222 RID: 4642 RVA: 0x0003C9D4 File Offset: 0x0003ABD4
		public static bool IsFilteredGraph(Type graphType)
		{
			return AsgardUtils.GetFilteredView(graphType) != null;
		}

		// Token: 0x06001223 RID: 4643 RVA: 0x0003C9F0 File Offset: 0x0003ABF0
		public static string GetFilteredView(Type graphType)
		{
			IEnumerable<FieldInfo> fields = AsgardUtils.GetFields(graphType);
			IEnumerable<FieldInfo> source = fields;
			Func<FieldInfo, bool> predicate;
			if ((predicate = AsgardUtils.<>O.<0>__IsPXFilter) == null)
			{
				predicate = (AsgardUtils.<>O.<0>__IsPXFilter = new Func<FieldInfo, bool>(AsgardUtils.IsPXFilter));
			}
			bool flag = source.Any(predicate);
			bool flag2 = !flag;
			string result;
			if (flag2)
			{
				result = null;
			}
			else
			{
				IEnumerable<FieldInfo> source2 = fields;
				Func<FieldInfo, bool> predicate2;
				if ((predicate2 = AsgardUtils.<>O.<1>__IsFilteredResult) == null)
				{
					predicate2 = (AsgardUtils.<>O.<1>__IsFilteredResult = new Func<FieldInfo, bool>(AsgardUtils.IsFilteredResult));
				}
				string text = (from fi in source2.Where(predicate2)
				select fi.Name).FirstOrDefault<string>();
				result = text;
			}
			return result;
		}

		// Token: 0x06001224 RID: 4644 RVA: 0x0003CA88 File Offset: 0x0003AC88
		public static bool IsGI(PXGraph graph)
		{
			return graph is PXGenericInqGrph;
		}

		// Token: 0x06001225 RID: 4645 RVA: 0x0003CAA4 File Offset: 0x0003ACA4
		public static bool IsProcessingGraph(PXGraph graph)
		{
			return AsgardUtils.GetProcessingView(graph) != null;
		}

		// Token: 0x06001226 RID: 4646 RVA: 0x0003CAC0 File Offset: 0x0003ACC0
		public static string GetProcessingView(PXGraph graph)
		{
			IEnumerable<FieldInfo> fields = AsgardUtils.GetFields(graph);
			IEnumerable<FieldInfo> source = fields;
			Func<FieldInfo, bool> predicate;
			if ((predicate = AsgardUtils.<>O.<2>__IsProcessingView) == null)
			{
				predicate = (AsgardUtils.<>O.<2>__IsProcessingView = new Func<FieldInfo, bool>(AsgardUtils.IsProcessingView));
			}
			return (from fi in source.Where(predicate)
			select fi.Name).FirstOrDefault<string>();
		}

		// Token: 0x06001227 RID: 4647 RVA: 0x0003CB28 File Offset: 0x0003AD28
		public static FieldInfo GetField(PXGraph _graph, string memberName)
		{
			return AsgardUtils.GetField(_graph.GetType(), memberName);
		}

		// Token: 0x06001228 RID: 4648 RVA: 0x0003CB48 File Offset: 0x0003AD48
		public static FieldInfo GetField(Type _graphType, string memberName)
		{
			return AsgardUtils.GetFields(_graphType, new string[]
			{
				memberName
			}).FirstOrDefault<FieldInfo>();
		}

		// Token: 0x06001229 RID: 4649 RVA: 0x0003CB74 File Offset: 0x0003AD74
		private static IEnumerable<FieldInfo> GetFields(Type _graphType, params string[] onlyMemberNames)
		{
			return (from fi in AsgardUtils.GetFields(_graphType)
			where onlyMemberNames.Contains(fi.Name)
			select fi).ToArray<FieldInfo>();
		}

		// Token: 0x0600122A RID: 4650 RVA: 0x0003CBB4 File Offset: 0x0003ADB4
		public static IEnumerable<FieldInfo> GetFields(PXGraph graph)
		{
			return AsgardUtils.GetFields((graph != null) ? graph.GetType() : null);
		}

		// Token: 0x0600122B RID: 4651 RVA: 0x0003CBD8 File Offset: 0x0003ADD8
		public static IEnumerable<FieldInfo> GetFields(Type type)
		{
			bool flag = type == null;
			IEnumerable<FieldInfo> result;
			if (flag)
			{
				result = Enumerable.Empty<FieldInfo>();
			}
			else
			{
				Func<Type, IEnumerable<FieldInfo>> valueFactory;
				if ((valueFactory = AsgardUtils.<>O.<3>__GetFieldsInternal) == null)
				{
					valueFactory = (AsgardUtils.<>O.<3>__GetFieldsInternal = new Func<Type, IEnumerable<FieldInfo>>(AsgardUtils.GetFieldsInternal));
				}
				IEnumerable<FieldInfo> orAdd = CacheHelper2<Type, IEnumerable<FieldInfo>>.GetOrAdd(type, valueFactory);
				result = orAdd;
			}
			return result;
		}

		// Token: 0x0600122C RID: 4652 RVA: 0x0003CC24 File Offset: 0x0003AE24
		private static IEnumerable<FieldInfo> GetFieldsInternal(Type type)
		{
			List<FieldInfo> list = new List<FieldInfo>();
			BindingFlags bindingAttr = BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy;
			FieldInfo[] fields = type.GetFields(bindingAttr);
			list.AddRange(fields);
			bool flag = typeof(PXGraph).IsAssignableFrom(type);
			if (flag)
			{
				foreach (Type type2 in HiddenUtils.GetExtensions(type, true))
				{
					FieldInfo[] fields2 = type2.GetFields(bindingAttr);
					list.AddRange(fields2);
				}
			}
			list = list.Distinct<FieldInfo>().ToList<FieldInfo>();
			return list;
		}

		// Token: 0x0600122D RID: 4653 RVA: 0x0003CCD0 File Offset: 0x0003AED0
		public static byte[] ReadFully(this Stream input)
		{
			byte[] array = new byte[16384];
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				int count;
				while ((count = input.Read(array, 0, array.Length)) > 0)
				{
					memoryStream.Write(array, 0, count);
				}
				result = memoryStream.ToArray();
			}
			return result;
		}

		// Token: 0x0600122E RID: 4654 RVA: 0x0003CD3C File Offset: 0x0003AF3C
		public static object RoundNearest(object value, object increment)
		{
			decimal d = (decimal)AsgardUtils.ChangeType(value, typeof(decimal), null);
			decimal num = (decimal)AsgardUtils.ChangeType(increment, typeof(decimal), null);
			bool flag = num == 0.0m;
			if (flag)
			{
				num = 1.0m;
			}
			decimal num2 = Math.Round(d / num) * num;
			return AsgardUtils.ChangeType(num2, value.GetType(), null);
		}

		// Token: 0x0600122F RID: 4655 RVA: 0x0003CDC8 File Offset: 0x0003AFC8
		public static string GetPrefix()
		{
			string result = string.Empty;
			bool flag = HttpContext.Current != null;
			if (flag)
			{
				FormsIdentity formsIdentity = HttpContext.Current.User.Identity as FormsIdentity;
				bool flag2 = formsIdentity != null;
				if (flag2)
				{
					result = Extensions.GetPrefix(formsIdentity);
				}
			}
			return result;
		}

		// Token: 0x06001230 RID: 4656 RVA: 0x0003CE18 File Offset: 0x0003B018
		public static bool ShouldReplace(string text)
		{
			return string.IsNullOrEmpty(text) || text.Length < 2;
		}

		// Token: 0x06001231 RID: 4657 RVA: 0x0003CE3E File Offset: 0x0003B03E
		public static void Deconstruct<TKey, TValue>(this KeyValuePair<TKey, TValue> kvp, out TKey key, out TValue val)
		{
			key = kvp.Key;
			val = kvp.Value;
		}

		// Token: 0x06001232 RID: 4658 RVA: 0x0003CE5B File Offset: 0x0003B05B
		public static void ShowEnableIf<Field>(PXCache cache, object row, bool enableIf, bool showIf) where Field : IBqlField
		{
			PXUIFieldAttribute.SetEnabled<Field>(cache, row, enableIf);
			PXUIFieldAttribute.SetVisible<Field>(cache, row, showIf);
		}

		// Token: 0x06001233 RID: 4659 RVA: 0x0003CE70 File Offset: 0x0003B070
		public static void ShowIf<Field>(PXCache cache, object row, bool showIf) where Field : IBqlField
		{
			AsgardUtils.ShowEnableIf<Field>(cache, row, showIf, showIf);
		}

		// Token: 0x06001234 RID: 4660 RVA: 0x0003CE80 File Offset: 0x0003B080
		public static void MakeRequired<Field>(PXCache cache, object data, bool condition) where Field : IBqlField
		{
			PXUIFieldAttribute.SetRequired<Field>(cache, condition);
			PXPersistingCheck pxpersistingCheck = condition ? 1 : 2;
			PXDefaultAttribute.SetPersistingCheck<Field>(cache, data, pxpersistingCheck);
		}

		// Token: 0x06001235 RID: 4661 RVA: 0x0003CEA7 File Offset: 0x0003B0A7
		public static void EnableIf<TNode>(PXAction<TNode> action, bool showIf) where TNode : class, IBqlTable, new()
		{
			action.SetEnabled(showIf);
		}

		// Token: 0x06001236 RID: 4662 RVA: 0x0003CEB2 File Offset: 0x0003B0B2
		public static void ShowOrHideGrid<Field>(PXCache cache, bool show, bool required = false) where Field : IBqlField
		{
			PXUIFieldAttribute.SetEnabled<Field>(cache, null, show || required);
			PXUIFieldAttribute.SetRequired<Field>(cache, show && required);
		}

		// Token: 0x06001237 RID: 4663 RVA: 0x0003CECC File Offset: 0x0003B0CC
		public static T CreateImpl<T>(string implCodeID) where T : class
		{
			Type typeFromHandle = typeof(T);
			string @namespace = typeFromHandle.Namespace;
			string text = typeFromHandle.Name;
			bool flag = text.StartsWith("I");
			if (flag)
			{
				text = text.Substring(1);
			}
			string text2 = @namespace + "." + implCodeID.Trim().ToLower().ToUpperFirst() + text;
			Type type = GraphHelper.GetType(text2);
			bool flag2 = type == null;
			if (flag2)
			{
				throw new PXException("No {0} type found for code '{1}' in namespace '{2}'", new object[]
				{
					text,
					implCodeID,
					@namespace
				});
			}
			return (T)((object)Activator.CreateInstance(type));
		}

		// Token: 0x06001238 RID: 4664 RVA: 0x0003CF78 File Offset: 0x0003B178
		public static T CreateImpl2<T>(string implCode) where T : ISelectable
		{
			IDictionary<string, string> impls = AsgardUtils.GetImpls<T>();
			string text = (from kvp in impls
			where kvp.Key == implCode
			select kvp.Value).FirstOrDefault<string>();
			Type type = GraphHelper.GetType(text);
			bool flag = type == null;
			if (flag)
			{
				Type typeFromHandle = typeof(T);
				string @namespace = typeFromHandle.Namespace;
				throw new PXException("No implementation of interface '{0}' found for code '{1}' in namespace '{2}'", new object[]
				{
					typeFromHandle.FullName,
					implCode,
					@namespace
				});
			}
			return (T)((object)Activator.CreateInstance(type));
		}

		// Token: 0x06001239 RID: 4665 RVA: 0x0003D040 File Offset: 0x0003B240
		private static IDictionary<string, string> GetImpls<T>() where T : ISelectable
		{
			Type typeFromHandle = typeof(T);
			IDictionary<string, string> dictionary;
			bool flag = !AsgardUtils.IMPLS_BY_INTERFACE.TryGetValue(typeFromHandle.FullName, out dictionary);
			if (flag)
			{
				IEnumerable<Type> implementations = AsgardUtils.GetImplementations<T>(true);
				IEnumerable<Type> source = implementations;
				Func<Type, ISelectable> selector;
				if ((selector = AsgardUtils.<>O.<4>__GetInstance) == null)
				{
					selector = (AsgardUtils.<>O.<4>__GetInstance = new Func<Type, ISelectable>(AsgardUtils.GetInstance));
				}
				ISelectable[] source2 = source.Select(selector).ToArray<ISelectable>();
				dictionary = source2.ToDictionary((ISelectable selectable) => selectable.Code, (ISelectable selectable) => selectable.GetType().FullName);
				AsgardUtils.IMPLS_BY_INTERFACE.Add(typeFromHandle.FullName, dictionary);
			}
			return dictionary;
		}

		// Token: 0x0600123A RID: 4666 RVA: 0x0003D108 File Offset: 0x0003B308
		private static ISelectable GetInstance(Type selectableType)
		{
			return (ISelectable)Activator.CreateInstance(selectableType);
		}

		// Token: 0x0600123B RID: 4667 RVA: 0x0003D128 File Offset: 0x0003B328
		public static void ApplyProperties(IEnumerable<CSAnswers> stepAttributes, object obj)
		{
			bool flag = obj == null;
			if (!flag)
			{
				Type type = obj.GetType();
				PropertyInfo[] properties = type.GetProperties();
				CSAttributeMaint graph = HiddenUtils.CreateInstance<CSAttributeMaint>();
				IEnumerable<Tuple<CSAnswers, CSAttribute>> source = AsgardUtils.MergeWithCSAttribute(graph, stepAttributes);
				string descPrefix = obj.GetType().Name;
				PXTrace.WriteInformation("Will try to apply attributes with Prefix '" + descPrefix + "'");
				PropertyInfo[] array = properties;
				for (int i = 0; i < array.Length; i++)
				{
					PropertyInfo prop = array[i];
					Tuple<CSAnswers, CSAttribute> tuple = source.FirstOrDefault((Tuple<CSAnswers, CSAttribute> ans) => ans.Item1.AttributeID.Equals(prop.Name) || ans.Item2.Description.Equals(descPrefix + "." + prop.Name));
					string text = (tuple != null) ? tuple.Item1.Value : null;
					bool flag2 = text != null;
					if (flag2)
					{
						AsgardUtils.SetValue(prop, obj, text);
					}
				}
			}
		}

		// Token: 0x0600123C RID: 4668 RVA: 0x0003D214 File Offset: 0x0003B414
		public static object SetValue(PropertyInfo prop, object obj, object attrValue)
		{
			Type propertyType = prop.PropertyType;
			object obj2 = AsgardUtils.ChangeType(attrValue, propertyType, null);
			prop.SetValue(obj, obj2);
			return obj2;
		}

		// Token: 0x0600123D RID: 4669 RVA: 0x0003D240 File Offset: 0x0003B440
		private static IEnumerable<Tuple<CSAnswers, CSAttribute>> MergeWithCSAttribute(PXGraph graph, IEnumerable<CSAnswers> stepAttributes)
		{
			Func<CSAnswers, Tuple<CSAnswers, CSAttribute>> selector = (CSAnswers x) => AsgardUtils.MergeWithCSAttribute(graph, x);
			return stepAttributes.Select(selector);
		}

		// Token: 0x0600123E RID: 4670 RVA: 0x0003D278 File Offset: 0x0003B478
		private static Tuple<CSAnswers, CSAttribute> MergeWithCSAttribute(PXGraph graph, CSAnswers answer)
		{
			CSAttribute item = PXSelectBase<CSAttribute, PXSelect<CSAttribute, Where<CSAttribute.attributeID, Equal<Required<CSAttribute.attributeID>>>>.Config>.Select(graph, new object[]
			{
				answer.AttributeID
			});
			return Tuple.Create<CSAnswers, CSAttribute>(answer, item);
		}

		// Token: 0x0600123F RID: 4671 RVA: 0x0003D2AC File Offset: 0x0003B4AC
		public static string ReplaceCR(this string str, string replaceBy)
		{
			bool flag = !string.IsNullOrEmpty(str);
			if (flag)
			{
				str = AsgardUtils.ANY_CR.Replace(str, replaceBy);
			}
			return str;
		}

		// Token: 0x06001240 RID: 4672 RVA: 0x0003D2DC File Offset: 0x0003B4DC
		public static string ReplaceCRDouble(this string str)
		{
			bool flag = !string.IsNullOrEmpty(str);
			if (flag)
			{
				str = AsgardUtils.DOUBLE_CR.Replace(str, "\r\n");
			}
			return str;
		}

		// Token: 0x06001241 RID: 4673 RVA: 0x0003D310 File Offset: 0x0003B510
		public static string CleanWhitespaces(this string str)
		{
			bool flag = !string.IsNullOrEmpty(str);
			if (flag)
			{
				str = Regex.Replace(str, "\\s+", " ");
			}
			return str;
		}

		// Token: 0x06001242 RID: 4674 RVA: 0x0003D344 File Offset: 0x0003B544
		public static IEnumerable<T> GetImplInstances<T>()
		{
			return new AsgardUtils.<GetImplInstances>d__45<T>(-2);
		}

		// Token: 0x06001243 RID: 4675 RVA: 0x0003D350 File Offset: 0x0003B550
		public static IEnumerable<Type> GetImplementations<T>(bool silent = true)
		{
			return AsgardUtils.GetImplementations(typeof(T), silent);
		}

		// Token: 0x06001244 RID: 4676 RVA: 0x0003D374 File Offset: 0x0003B574
		public static IEnumerable<Type> GetImplementations(Type interfaceType, bool silent = true)
		{
			Func<Type, IEnumerable<Type>> valueFactory = (Type a) => AsgardUtils.GetImplementationsInternal(interfaceType, silent);
			return CacheHelper2<Type, IEnumerable<Type>>.GetOrAdd(interfaceType, valueFactory);
		}

		// Token: 0x06001245 RID: 4677 RVA: 0x0003D3B8 File Offset: 0x0003B5B8
		private static IEnumerable<Type> GetImplementationsInternal(Type interfaceType, bool silent = true)
		{
			List<Type> list = new List<Type>();
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
			foreach (Assembly ass2 in from ass in assemblies
			where PXSubstManager.IsSuitableTypeExportAssembly(ass, true)
			select ass)
			{
				AsgardUtils.AddImplementationsInternalByAssembly(ass2, list, interfaceType, silent);
			}
			return list;
		}

		// Token: 0x06001246 RID: 4678 RVA: 0x0003D444 File Offset: 0x0003B644
		public static IEnumerable<Type> GetImplementationsByAssembly<T>(Assembly ass, bool silent = true)
		{
			List<Type> list = new List<Type>();
			AsgardUtils.AddImplementationsInternalByAssembly(ass, list, typeof(T), silent);
			return list;
		}

		// Token: 0x06001247 RID: 4679 RVA: 0x0003D470 File Offset: 0x0003B670
		private static void AddImplementationsInternalByAssembly(Assembly ass, List<Type> impls, Type interfaceType, bool silent)
		{
			Type[] array = null;
			try
			{
				array = ass.GetExportedTypes();
			}
			catch (Exception ex)
			{
				if (silent)
				{
					PXTrace.WriteError("An exception occured when loading assembly {0}: {1}", new object[]
					{
						ass.FullName,
						ex.Message
					});
					return;
				}
				throw;
			}
			bool flag = array != null;
			if (flag)
			{
				foreach (Type type in array)
				{
					bool flag2 = interfaceType != null && interfaceType.IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract;
					if (flag2)
					{
						impls.Add(type);
					}
				}
			}
		}

		// Token: 0x06001248 RID: 4680 RVA: 0x0003D528 File Offset: 0x0003B728
		public static Type FindType(string typeName)
		{
			bool flag = AsgardUtils.IsEmpty(typeName);
			Type result;
			if (flag)
			{
				result = null;
			}
			else
			{
				Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
				foreach (Assembly assembly in assemblies)
				{
					Type type = assembly.GetType(typeName);
					bool flag2 = type != null;
					if (flag2)
					{
						return type;
					}
				}
				result = null;
			}
			return result;
		}

		// Token: 0x06001249 RID: 4681 RVA: 0x0003D590 File Offset: 0x0003B790
		public static string ToLowerFirst(this string name)
		{
			bool flag = AsgardUtils.IsEmpty(name);
			string result;
			if (flag)
			{
				result = name;
			}
			else
			{
				result = char.ToLowerInvariant(name[0]).ToString() + ((name.Length > 1) ? name.Substring(1) : "");
			}
			return result;
		}

		// Token: 0x0600124A RID: 4682 RVA: 0x0003D5E4 File Offset: 0x0003B7E4
		public static string ToUpperFirst(this string name)
		{
			bool flag = AsgardUtils.IsEmpty(name);
			string result;
			if (flag)
			{
				result = name;
			}
			else
			{
				result = char.ToUpperInvariant(name[0]).ToString() + ((name.Length > 1) ? name.Substring(1) : "");
			}
			return result;
		}

		// Token: 0x0600124B RID: 4683 RVA: 0x0003D638 File Offset: 0x0003B838
		public static string SpreadCamelCase(this string str)
		{
			bool flag = AsgardUtils.IsEmpty(str);
			string result;
			if (flag)
			{
				result = str;
			}
			else
			{
				string text = Regex.Replace(Regex.Replace(str, "(\\P{Ll})(\\P{Ll}\\p{Ll})", "$1 $2"), "(\\p{Ll})(\\P{Ll})", "$1 $2");
				result = text;
			}
			return result;
		}

		// Token: 0x0600124C RID: 4684 RVA: 0x0003D67C File Offset: 0x0003B87C
		public static string SpreadCamelCase2(this string str)
		{
			bool flag = AsgardUtils.IsEmpty(str);
			string result;
			if (flag)
			{
				result = str;
			}
			else
			{
				string[] value = AsgardUtils.CAMELS2.Split(str);
				string text = string.Join(" ", value);
				result = text;
			}
			return result;
		}

		// Token: 0x0600124D RID: 4685 RVA: 0x0003D6B8 File Offset: 0x0003B8B8
		public static string CamelToUnderscore(this string str)
		{
			bool flag = AsgardUtils.IsEmpty(str);
			string result;
			if (flag)
			{
				result = str;
			}
			else
			{
				string text = string.Concat(str.Select((char x, int i) => (i > 0 && char.IsUpper(x)) ? ("_" + x.ToString()) : x.ToString()));
				result = text;
			}
			return result;
		}

		// Token: 0x0600124E RID: 4686 RVA: 0x0003D708 File Offset: 0x0003B908
		public static string Signature(this MethodInfo mi)
		{
			Func<ParameterInfo, bool> paramSelector;
			if ((paramSelector = AsgardUtils.<>O.<5>__Keep) == null)
			{
				paramSelector = (AsgardUtils.<>O.<5>__Keep = new Func<ParameterInfo, bool>(AsgardUtils.Keep));
			}
			return mi.Signature(paramSelector);
		}

		// Token: 0x0600124F RID: 4687 RVA: 0x0003D73C File Offset: 0x0003B93C
		public static string Signature(this MethodInfo mi, Func<ParameterInfo, bool> paramSelector)
		{
			bool flag = mi == null;
			string result;
			if (flag)
			{
				result = null;
			}
			else
			{
				IEnumerable<ParameterInfo> source = mi.GetParameters().Where(paramSelector);
				Func<ParameterInfo, string> selector;
				if ((selector = AsgardUtils.<>O.<6>__ParamToString) == null)
				{
					selector = (AsgardUtils.<>O.<6>__ParamToString = new Func<ParameterInfo, string>(AsgardUtils.ParamToString));
				}
				IEnumerable<string> values = source.Select(selector);
				string text = string.Format("{1}({2}) -> {0}", AsgardUtils.TypeToString(mi.ReturnType), mi.Name, string.Join(", ", values));
				result = text;
			}
			return result;
		}

		// Token: 0x06001250 RID: 4688 RVA: 0x0003D7B4 File Offset: 0x0003B9B4
		public static string Signature(this MethodBase mi)
		{
			Func<ParameterInfo, bool> paramSelector;
			if ((paramSelector = AsgardUtils.<>O.<5>__Keep) == null)
			{
				paramSelector = (AsgardUtils.<>O.<5>__Keep = new Func<ParameterInfo, bool>(AsgardUtils.Keep));
			}
			return mi.Signature(paramSelector);
		}

		// Token: 0x06001251 RID: 4689 RVA: 0x0003D7E8 File Offset: 0x0003B9E8
		public static string Signature(this MethodBase mb, Func<ParameterInfo, bool> paramSelector)
		{
			bool flag = mb == null;
			string result;
			if (flag)
			{
				result = null;
			}
			else
			{
				IEnumerable<ParameterInfo> source = mb.GetParameters().Where(paramSelector);
				Func<ParameterInfo, string> selector;
				if ((selector = AsgardUtils.<>O.<6>__ParamToString) == null)
				{
					selector = (AsgardUtils.<>O.<6>__ParamToString = new Func<ParameterInfo, string>(AsgardUtils.ParamToString));
				}
				IEnumerable<string> values = source.Select(selector);
				string text = string.Format("{0}({1})", mb.Name, string.Join(", ", values));
				result = text;
			}
			return result;
		}

		// Token: 0x06001252 RID: 4690 RVA: 0x0003D854 File Offset: 0x0003BA54
		public static bool Keep(ParameterInfo pi)
		{
			return true;
		}

		// Token: 0x06001253 RID: 4691 RVA: 0x0003D868 File Offset: 0x0003BA68
		public static string FixName(ParameterInfo pi)
		{
			string text = pi.Name;
			bool flag = text.StartsWith("_") && text.EndsWith("_");
			if (flag)
			{
				text = text.Substring(1, text.Length - 2);
			}
			return text;
		}

		// Token: 0x06001254 RID: 4692 RVA: 0x0003D8B4 File Offset: 0x0003BAB4
		public static string ParamToString(ParameterInfo pi)
		{
			string arg = "";
			bool flag = pi.DefaultValue != DBNull.Value;
			if (flag)
			{
				arg = " = " + AsgardUtils.GetValueForTrace(pi.DefaultValue);
			}
			string arg2 = AsgardUtils.TypeToString(pi.ParameterType);
			return string.Format("{0} {1}{2}", arg2, AsgardUtils.FixName(pi), arg);
		}

		// Token: 0x06001255 RID: 4693 RVA: 0x0003D918 File Offset: 0x0003BB18
		private static string TypeToString(Type type)
		{
			bool isArray = type.IsArray;
			bool flag = isArray;
			if (flag)
			{
				type = type.GetElementType();
			}
			Type underlyingType = Nullable.GetUnderlyingType(type);
			bool flag2 = underlyingType != null;
			bool flag3 = flag2;
			if (flag3)
			{
				type = underlyingType;
			}
			bool isGenericType = type.IsGenericType;
			string text = type.Name;
			bool flag4 = isGenericType;
			if (flag4)
			{
				text = text.Substring(0, text.IndexOf('`'));
			}
			string text2 = text;
			string text3 = text2;
			uint num = <PrivateImplementationDetails>.ComputeStringHash(text3);
			if (num <= 1615808600U)
			{
				if (num <= 697196164U)
				{
					if (num != 423635464U)
					{
						if (num != 679076413U)
						{
							if (num != 697196164U)
							{
								goto IL_306;
							}
							if (!(text3 == "Int64"))
							{
								goto IL_306;
							}
							text = "long";
							goto IL_306;
						}
						else if (!(text3 == "Char"))
						{
							goto IL_306;
						}
					}
					else if (!(text3 == "SByte"))
					{
						goto IL_306;
					}
				}
				else if (num <= 1323747186U)
				{
					if (num != 765439473U)
					{
						if (num != 1323747186U)
						{
							goto IL_306;
						}
						if (!(text3 == "UInt16"))
						{
							goto IL_306;
						}
						text = "ushort";
						goto IL_306;
					}
					else
					{
						if (!(text3 == "Int16"))
						{
							goto IL_306;
						}
						text = "short";
						goto IL_306;
					}
				}
				else if (num != 1324880019U)
				{
					if (num != 1615808600U)
					{
						goto IL_306;
					}
					if (!(text3 == "String"))
					{
						goto IL_306;
					}
				}
				else
				{
					if (!(text3 == "UInt64"))
					{
						goto IL_306;
					}
					text = "ulong";
					goto IL_306;
				}
			}
			else if (num <= 3409549631U)
			{
				if (num <= 2711245919U)
				{
					if (num != 2386971688U)
					{
						if (num != 2711245919U)
						{
							goto IL_306;
						}
						if (!(text3 == "Int32"))
						{
							goto IL_306;
						}
						text = "int";
						goto IL_306;
					}
					else if (!(text3 == "Double"))
					{
						goto IL_306;
					}
				}
				else if (num != 2779444460U)
				{
					if (num != 3409549631U)
					{
						goto IL_306;
					}
					if (!(text3 == "Byte"))
					{
						goto IL_306;
					}
				}
				else if (!(text3 == "Decimal"))
				{
					goto IL_306;
				}
			}
			else if (num <= 3851314394U)
			{
				if (num != 3538687084U)
				{
					if (num != 3851314394U)
					{
						goto IL_306;
					}
					if (!(text3 == "Object"))
					{
						goto IL_306;
					}
				}
				else
				{
					if (!(text3 == "UInt32"))
					{
						goto IL_306;
					}
					text = "uint";
					goto IL_306;
				}
			}
			else if (num != 3969205087U)
			{
				if (num != 4051133705U)
				{
					goto IL_306;
				}
				if (!(text3 == "Single"))
				{
					goto IL_306;
				}
				text = "float";
				goto IL_306;
			}
			else
			{
				if (!(text3 == "Boolean"))
				{
					goto IL_306;
				}
				text = "bool";
				goto IL_306;
			}
			text = text.ToLower();
			IL_306:
			bool flag5 = flag2;
			if (flag5)
			{
				text += "?";
			}
			bool flag6 = isArray;
			if (flag6)
			{
				text += "[]";
			}
			bool flag7 = isGenericType;
			if (flag7)
			{
				text = text + "<" + string.Join(", ", AsgardUtils.TypesToStrings(type.GenericTypeArguments)) + ">";
			}
			return text;
		}

		// Token: 0x06001256 RID: 4694 RVA: 0x0003DC94 File Offset: 0x0003BE94
		public static string[] TypesToStrings(Type[] types)
		{
			bool flag = types == null;
			string[] result;
			if (flag)
			{
				result = new string[0];
			}
			else
			{
				string[] array = (from ty in types
				select AsgardUtils.TypeToString(ty)).ToArray<string>();
				result = array;
			}
			return result;
		}

		// Token: 0x06001257 RID: 4695 RVA: 0x0003DCE4 File Offset: 0x0003BEE4
		public static string JoinForTrace(string separator, params object[] values)
		{
			bool flag = values == null || values.Length == 0;
			string result;
			if (flag)
			{
				result = string.Empty;
			}
			else
			{
				if (separator == null)
				{
					separator = string.Empty;
				}
				StringBuilder stringBuilder = AsgardUtils.StringBuilderCache.Acquire(16);
				string valueForTrace = AsgardUtils.GetValueForTrace(values, 0);
				stringBuilder.Append(valueForTrace);
				for (int i = 1; i < values.Length; i++)
				{
					stringBuilder.Append(separator);
					valueForTrace = AsgardUtils.GetValueForTrace(values, i);
					stringBuilder.Append(valueForTrace);
				}
				result = AsgardUtils.StringBuilderCache.GetStringAndRelease(stringBuilder);
			}
			return result;
		}

		// Token: 0x06001258 RID: 4696 RVA: 0x0003DD6C File Offset: 0x0003BF6C
		private static string GetValueForTrace(object[] values, int index)
		{
			object value = (index < 0 || index > values.Length - 1) ? null : values[index];
			return AsgardUtils.GetValueForTrace(value);
		}

		// Token: 0x06001259 RID: 4697 RVA: 0x0003DD98 File Offset: 0x0003BF98
		private static string GetValueForTrace(object value)
		{
			string text = ((value != null) ? value.ToString() : null) ?? "NULL";
			bool flag = value is string;
			if (flag)
			{
				text = "\"" + text + "\"";
			}
			return text;
		}

		// Token: 0x0600125A RID: 4698 RVA: 0x0003DDE4 File Offset: 0x0003BFE4
		public static string SplitAndGet(int? size, int index, string toSplit, ref string[] values, char delim = ',')
		{
			if (values == null)
			{
				values = AsgardUtils.Split(size, toSplit, ',');
			}
			if (index >= 0)
			{
				int? num = size;
				if (index < num.GetValueOrDefault() & num != null)
				{
					return values[index];
				}
			}
			return null;
		}

		// Token: 0x0600125B RID: 4699 RVA: 0x0003DE28 File Offset: 0x0003C028
		public static string[] Split(int? size, string toSplit, char delim = ',')
		{
			string[] array = new string[size.GetValueOrDefault()];
			bool flag = toSplit != null;
			if (flag)
			{
				string[] array2 = toSplit.Split(new char[]
				{
					delim
				});
				Array.Copy(array2, array, Math.Min(array2.Length, array.Length));
			}
			return array;
		}

		// Token: 0x0600125C RID: 4700 RVA: 0x0003DE78 File Offset: 0x0003C078
		public static bool IsNotEmpty(this IEnumerable<object> objs)
		{
			return AsgardUtils.IsNotEmpty(objs);
		}

		// Token: 0x0600125D RID: 4701 RVA: 0x0003DE90 File Offset: 0x0003C090
		public static T[] NonNulls<T>(params T[] objs)
		{
			Func<T, bool> predicate;
			if ((predicate = AsgardUtils.<NonNulls>O__74_0<T>.<0>__NotNull) == null)
			{
				predicate = (AsgardUtils.<NonNulls>O__74_0<T>.<0>__NotNull = new Func<T, bool>(AsgardUtils.NotNull<T>));
			}
			return objs.Where(predicate).ToArray<T>();
		}

		// Token: 0x0600125E RID: 4702 RVA: 0x0003DEC8 File Offset: 0x0003C0C8
		public static IEnumerable<T> NonNulls<T>(this IEnumerable<T> objs)
		{
			Func<T, bool> predicate;
			if ((predicate = AsgardUtils.<NonNulls>O__75_0<T>.<0>__NotNull) == null)
			{
				predicate = (AsgardUtils.<NonNulls>O__75_0<T>.<0>__NotNull = new Func<T, bool>(AsgardUtils.NotNull<T>));
			}
			return objs.Where(predicate).ToArray<T>();
		}

		// Token: 0x0600125F RID: 4703 RVA: 0x0003DF00 File Offset: 0x0003C100
		public static bool IsNull(object obj)
		{
			return obj == null;
		}

		// Token: 0x06001260 RID: 4704 RVA: 0x0003DF18 File Offset: 0x0003C118
		public static bool NotNull(object obj)
		{
			return obj != null;
		}

		// Token: 0x06001261 RID: 4705 RVA: 0x0003DF30 File Offset: 0x0003C130
		public static bool NotNull<T>(T obj)
		{
			return obj != null;
		}

		// Token: 0x06001262 RID: 4706 RVA: 0x0003DF4C File Offset: 0x0003C14C
		public static bool IsNotEmpty(object obj)
		{
			return !AsgardUtils.IsEmpty(obj);
		}

		// Token: 0x06001263 RID: 4707 RVA: 0x0003DF68 File Offset: 0x0003C168
		public static bool IsEmpty(object obj)
		{
			bool flag = obj == null || obj is DBNull;
			bool result;
			if (flag)
			{
				result = true;
			}
			else
			{
				bool flag2 = obj is string;
				if (flag2)
				{
					result = string.IsNullOrWhiteSpace(obj.ToString());
				}
				else
				{
					bool flag3 = obj is decimal;
					if (flag3)
					{
						result = 0m.Equals(obj);
					}
					else
					{
						bool flag4 = obj is float;
						if (flag4)
						{
							result = 0f.Equals(obj);
						}
						else
						{
							bool flag5 = obj is double;
							if (flag5)
							{
								result = 0.0.Equals(obj);
							}
							else
							{
								bool flag6 = obj is byte || obj is short || obj is int || obj is long;
								if (flag6)
								{
									result = 0.Equals(obj);
								}
								else
								{
									bool flag7 = obj.GetType().IsCompatibleWith(typeof(ICollection));
									if (flag7)
									{
										ICollection collection = (ICollection)obj;
										result = (collection.Count == 0);
									}
									else
									{
										bool flag8 = obj.GetType().IsCompatibleWith(typeof(IEnumerable));
										if (flag8)
										{
											IEnumerable source = (IEnumerable)obj;
											IEnumerable<object> source2 = source.Cast<object>();
											result = !source2.Any<object>();
										}
										else
										{
											result = false;
										}
									}
								}
							}
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06001264 RID: 4708 RVA: 0x0003E0CC File Offset: 0x0003C2CC
		public static object Cast(this Type Type, object data)
		{
			ParameterExpression parameterExpression = Expression.Parameter(typeof(object), "data");
			BlockExpression body = Expression.Block(new Expression[]
			{
				Expression.Convert(Expression.Convert(parameterExpression, data.GetType()), Type)
			});
			Delegate @delegate = Expression.Lambda(body, new ParameterExpression[]
			{
				parameterExpression
			}).Compile();
			return @delegate.DynamicInvoke(new object[]
			{
				data
			});
		}

		// Token: 0x06001265 RID: 4709 RVA: 0x0003E140 File Offset: 0x0003C340
		public static object FirstResultOrDefault(this object result)
		{
			bool flag = result == null;
			object result2;
			if (flag)
			{
				result2 = null;
			}
			else
			{
				IPXResultset ipxresultset = result as IPXResultset;
				bool flag2 = ipxresultset != null && ipxresultset.GetRowCount() > 0;
				if (flag2)
				{
					object item = ipxresultset.GetItem(0, 0);
					result2 = item;
				}
				else
				{
					bool flag3 = result.GetType().IsCompatibleWith(typeof(IEnumerable));
					if (flag3)
					{
						IEnumerable source = (IEnumerable)result;
						IEnumerable<object> source2 = source.Cast<object>();
						result2 = source2.FirstOrDefault<object>();
					}
					else
					{
						bool flag4 = result is IBqlTable;
						if (flag4)
						{
							result2 = result;
						}
						else
						{
							result2 = null;
						}
					}
				}
			}
			return result2;
		}

		// Token: 0x06001266 RID: 4710 RVA: 0x0003E1D4 File Offset: 0x0003C3D4
		public static object GetItem(IPXResultset rs, int rowNbr, int tableNbr)
		{
			bool flag = rs == null;
			object result;
			if (flag)
			{
				result = null;
			}
			else
			{
				int rowCount = rs.GetRowCount();
				bool flag2 = rowNbr < rowCount;
				if (flag2)
				{
					IList list = (IList)rs.GetCollection();
					object obj = list[rowNbr];
					PXResult<GenericResult> pxresult = obj as PXResult<GenericResult>;
					bool flag3 = pxresult != null;
					if (flag3)
					{
						GenericResult gr = pxresult;
						return gr.GetItem(tableNbr);
					}
					GenericResult genericResult = obj as GenericResult;
					bool flag4 = genericResult != null;
					if (flag4)
					{
						return genericResult.GetItem(tableNbr);
					}
					PXResult pxresult2 = obj as PXResult;
					bool flag5 = pxresult2 != null;
					if (flag5)
					{
						return pxresult2.GetItem(tableNbr);
					}
					object[] array = obj as object[];
					bool flag6 = array != null;
					if (flag6)
					{
						int num = array.Length;
						bool flag7 = tableNbr < num;
						if (flag7)
						{
							return array[tableNbr];
						}
					}
				}
				result = null;
			}
			return result;
		}

		// Token: 0x06001267 RID: 4711 RVA: 0x0003E2C0 File Offset: 0x0003C4C0
		public static object GetItem(this PXResult pxr, int tableNbr)
		{
			bool flag = pxr != null;
			if (flag)
			{
				int tableCount = pxr.GetTableCount();
				bool flag2 = tableNbr < tableCount;
				if (flag2)
				{
					return pxr[tableNbr];
				}
			}
			return null;
		}

		// Token: 0x06001268 RID: 4712 RVA: 0x0003E2FC File Offset: 0x0003C4FC
		public static object GetItem(this GenericResult gr, int tableNbr)
		{
			bool flag = gr != null;
			if (flag)
			{
				int tableCount = gr.GetTableCount();
				bool flag2 = tableNbr < tableCount;
				if (flag2)
				{
					return gr.Values.Values.Skip(tableNbr).FirstOrDefault<object>();
				}
			}
			return null;
		}

		// Token: 0x06001269 RID: 4713 RVA: 0x0003E348 File Offset: 0x0003C548
		public static string AppendNote(PXCache cache, object row, string noteToAppend)
		{
			string text = PXNoteAttribute.GetNote(cache, row);
			bool flag = !string.IsNullOrEmpty(text);
			if (flag)
			{
				text = text + Environment.NewLine + Environment.NewLine + noteToAppend;
			}
			else
			{
				text = noteToAppend;
			}
			PXNoteAttribute.SetNote(cache, row, text);
			return text;
		}

		// Token: 0x0600126A RID: 4714 RVA: 0x0003E394 File Offset: 0x0003C594
		public static Type GetItemType(object result)
		{
			IViewResult viewResult = result as IViewResult;
			bool flag = viewResult != null;
			if (flag)
			{
				result = viewResult.Result;
				bool flag2 = result is Func<object>;
				if (flag2)
				{
					return viewResult.ItemTypes[0];
				}
			}
			IPXResultset ipxresultset = result as IPXResultset;
			bool flag3 = ipxresultset != null;
			Type result2;
			if (flag3)
			{
				result2 = ipxresultset.GetItemType(0);
			}
			else
			{
				PXResult pxresult = result as PXResult;
				bool flag4 = pxresult != null;
				if (flag4)
				{
					result2 = AsgardUtils.GetItemType(pxresult, 0);
				}
				else
				{
					result2 = ((result != null) ? result.GetType() : null);
				}
			}
			return result2;
		}

		// Token: 0x0600126B RID: 4715 RVA: 0x0003E42C File Offset: 0x0003C62C
		public static Type GetItemType(this IPXResultset rs, int index)
		{
			Type type = rs.GetItemType(index);
			bool flag = type == null && rs.GetRowCount() > 0;
			if (flag)
			{
				IList list = (IList)rs.GetCollection();
				object obj = list[0];
				PXResult<GenericResult> pxresult = obj as PXResult<GenericResult>;
				bool flag2 = pxresult != null;
				if (flag2)
				{
					GenericResult gr = pxresult;
					IList<Type> itemTypes = AsgardUtils.GetItemTypes(gr);
					type = itemTypes[index];
				}
				else
				{
					GenericResult genericResult = obj as GenericResult;
					bool flag3 = genericResult != null;
					if (flag3)
					{
						IEnumerable<object> enumerable = genericResult.Values.Values.Skip(index);
						type = ((enumerable != null) ? enumerable.GetType() : null);
					}
					else
					{
						PXResult pxresult2 = obj as PXResult;
						bool flag4 = pxresult2 != null;
						if (flag4)
						{
							return AsgardUtils.GetItemType(pxresult2, index);
						}
					}
				}
			}
			return type;
		}

		// Token: 0x0600126C RID: 4716 RVA: 0x0003E504 File Offset: 0x0003C704
		public static Type GetItemType(PXResult row, int index)
		{
			return row.GetItemType(index);
		}

		// Token: 0x0600126D RID: 4717 RVA: 0x0003E520 File Offset: 0x0003C720
		public static IList<Type> GetItemTypes(this PXResult pxr)
		{
			bool flag = pxr == null;
			IList<Type> result;
			if (flag)
			{
				result = Enumerable.Empty<Type>().ToList<Type>();
			}
			else
			{
				Type[] array = pxr.Tables.ToArray<Type>();
				result = array;
			}
			return result;
		}

		// Token: 0x0600126E RID: 4718 RVA: 0x0003E558 File Offset: 0x0003C758
		public static IList<Type> GetItemTypes(this IPXResultset rs)
		{
			bool flag = rs == null;
			IList<Type> result;
			if (flag)
			{
				result = Enumerable.Empty<Type>().ToList<Type>();
			}
			else
			{
				bool flag2 = rs.GetRowCount() > 0;
				if (flag2)
				{
					IList list = (IList)rs.GetCollection();
					result = AsgardUtils.GetItemTypes(list);
				}
				else
				{
					int tableCount = rs.GetTableCount();
					IEnumerable<int> source = from x in Enumerable.Range(0, tableCount)
					select x - 1 + 1;
					List<Type> list2 = (from tableNo in source
					select rs.GetItemType(tableNo)).ToList<Type>();
					PXDelayedQuery delayedQuery = rs.GetDelayedQuery();
					if (delayedQuery != null)
					{
						PXView view = delayedQuery.View;
						Type[] array = (view != null) ? view.GetItemTypes() : null;
					}
					result = list2;
				}
			}
			return result;
		}

		// Token: 0x0600126F RID: 4719 RVA: 0x0003E644 File Offset: 0x0003C844
		public static IList<Type> GetItemTypes(IList list)
		{
			bool flag = list != null && list.Count > 0;
			if (flag)
			{
				object obj = list[0];
				Type type = obj.GetType();
				PXResult<GenericResult> pxresult = obj as PXResult<GenericResult>;
				bool flag2 = pxresult != null;
				if (flag2)
				{
					GenericResult gr = pxresult;
					return AsgardUtils.GetItemTypes(gr);
				}
				GenericResult genericResult = obj as GenericResult;
				bool flag3 = genericResult != null;
				if (flag3)
				{
					return AsgardUtils.GetItemTypes(genericResult);
				}
				IBqlTable bqlTable = obj as IBqlTable;
				bool flag4 = bqlTable != null;
				if (flag4)
				{
					return new Type[]
					{
						type
					};
				}
				bool isGenericType = type.IsGenericType;
				if (isGenericType)
				{
					Type[] genericArguments = type.GetGenericArguments();
					bool flag5 = genericArguments.All((Type genType) => genType.IsCompatibleWith(typeof(IBqlTable)));
					if (flag5)
					{
						return genericArguments;
					}
				}
			}
			return Enumerable.Empty<Type>().ToList<Type>();
		}

		// Token: 0x06001270 RID: 4720 RVA: 0x0003E73C File Offset: 0x0003C93C
		public static IList<Type> GetItemTypes(GenericResult gr)
		{
			bool flag = gr != null;
			IList<Type> result;
			if (flag)
			{
				ICollection<object> values = gr.Values.Values;
				Type[] array = (from jr in values
				select jr.GetType()).ToArray<Type>();
				result = array;
			}
			else
			{
				result = Enumerable.Empty<Type>().ToList<Type>();
			}
			return result;
		}

		// Token: 0x06001271 RID: 4721 RVA: 0x0003E79C File Offset: 0x0003C99C
		public static IList<string> GetItemTypeNames(this IPXResultset rs)
		{
			IList<Type> itemTypes = rs.GetItemTypes();
			return (from it in itemTypes
			select it.Name).ToList<string>();
		}

		// Token: 0x06001272 RID: 4722 RVA: 0x0003E7E4 File Offset: 0x0003C9E4
		public static IList<string> GetItemTypeNames(this PXResult res)
		{
			IList<Type> itemTypes = res.GetItemTypes();
			return (from it in itemTypes
			select it.Name).ToList<string>();
		}

		// Token: 0x06001273 RID: 4723 RVA: 0x0003E82C File Offset: 0x0003CA2C
		public static IList<string> GetItemTypeNames(this ViewDef viewDef)
		{
			Type[] itemTypes = viewDef.ItemTypes;
			return (from it in itemTypes
			select it.Name).ToList<string>();
		}

		// Token: 0x06001274 RID: 4724 RVA: 0x0003E874 File Offset: 0x0003CA74
		public static Type GetJoinedItemType(this ViewDef viewDef, string joinedTableName)
		{
			Type[] itemTypes = viewDef.ItemTypes;
			IList<string> itemTypeNames = viewDef.GetItemTypeNames();
			int num = itemTypeNames.IndexOf(joinedTableName);
			bool flag = num > 0;
			Type result;
			if (flag)
			{
				Type type = itemTypes[num];
				result = type;
			}
			else
			{
				result = itemTypes.First<Type>();
			}
			return result;
		}

		// Token: 0x06001275 RID: 4725 RVA: 0x0003E8B8 File Offset: 0x0003CAB8
		public static string GetItemTypeName(object _row_)
		{
			Type itemType = AsgardUtils.GetItemType(_row_, true);
			return (itemType != null) ? itemType.Name : null;
		}

		// Token: 0x06001276 RID: 4726 RVA: 0x0003E8E0 File Offset: 0x0003CAE0
		public static Type GetItemType(object _row_, bool silent = false)
		{
			Type itemType = AsgardUtils.GetItemType(_row_);
			bool flag = itemType == null && !silent;
			if (flag)
			{
				throw new PXException("Cannot find Item Type for row of type '{0}'", new object[]
				{
					(_row_ != null) ? _row_.GetType() : null
				});
			}
			return itemType;
		}

		// Token: 0x06001277 RID: 4727 RVA: 0x0003E930 File Offset: 0x0003CB30
		public static bool IsSame(PXGraph graph, IPXResultset rs1, IPXResultset rs2)
		{
			bool flag = rs1 == null || rs2 == null;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				int rowCount = rs1.GetRowCount();
				int rowCount2 = rs2.GetRowCount();
				bool flag2 = rowCount != rowCount2;
				if (flag2)
				{
					result = false;
				}
				else
				{
					IList<Type> itemTypes = rs1.GetItemTypes();
					IList<Type> itemTypes2 = rs2.GetItemTypes();
					bool flag3 = itemTypes == null || !itemTypes.Any<Type>() || itemTypes2 == null || !itemTypes2.Any<Type>();
					if (flag3)
					{
						result = false;
					}
					else
					{
						Type type = itemTypes.First<Type>();
						Type right = itemTypes2.First<Type>();
						bool flag4 = type != right;
						if (flag4)
						{
							result = false;
						}
						else
						{
							PXCache cache = graph.Caches[type];
							bool flag5 = true;
							for (int i = 0; i < rowCount; i++)
							{
								object item = rs1.GetItem(i, 0);
								object[] keys = AsgardUtils.GetKeys(cache, item);
								object item2 = rs2.GetItem(i, 0);
								object[] keys2 = AsgardUtils.GetKeys(cache, item2);
								flag5 = (flag5 && keys.SequenceEqual(keys2));
							}
							result = flag5;
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06001278 RID: 4728 RVA: 0x0003EA4C File Offset: 0x0003CC4C
		public static int GetTableCount(this IPXResultset rs)
		{
			int result = rs.GetTableCount();
			bool flag = rs.GetRowCount() > 0;
			if (flag)
			{
				IList list = (IList)rs.GetCollection();
				object obj = list[0];
				PXResult<GenericResult> pxresult = obj as PXResult<GenericResult>;
				bool flag2 = pxresult != null;
				if (flag2)
				{
					GenericResult gr = pxresult;
					result = gr.GetTableCount();
				}
				else
				{
					PXResult pxresult2 = obj as PXResult;
					bool flag3 = pxresult2 != null;
					if (flag3)
					{
						result = pxresult2.GetTableCount();
					}
					else
					{
						GenericResult genericResult = obj as GenericResult;
						bool flag4 = genericResult != null;
						if (flag4)
						{
							result = genericResult.GetTableCount();
						}
						else
						{
							object[] array = obj as object[];
							bool flag5 = array != null;
							if (flag5)
							{
								result = array.Length;
							}
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06001279 RID: 4729 RVA: 0x0003EB10 File Offset: 0x0003CD10
		public static int GetTableCount(this GenericResult gr)
		{
			bool flag = gr != null;
			int result;
			if (flag)
			{
				result = gr.Values.Values.Count;
			}
			else
			{
				result = 0;
			}
			return result;
		}

		// Token: 0x0600127A RID: 4730 RVA: 0x0003EB40 File Offset: 0x0003CD40
		public static int GetTableCount(this PXResult pxr)
		{
			bool flag = pxr != null;
			int result;
			if (flag)
			{
				result = pxr.GetType().GetGenericArguments().Length;
			}
			else
			{
				result = 0;
			}
			return result;
		}

		// Token: 0x0600127B RID: 4731 RVA: 0x0003EB6C File Offset: 0x0003CD6C
		public static PXCache AddOrGetCache(PXGraph graph, object row)
		{
			bool flag = graph == null || row == null;
			PXCache result;
			if (flag)
			{
				result = null;
			}
			else
			{
				bool flag2 = row is PXResult;
				if (flag2)
				{
					row = PXResult.UnwrapMain(row);
				}
				Type type = row.GetType();
				PXCache item = AsgardUtils.AddOrGetCache(graph, type).Item2;
				result = item;
			}
			return result;
		}

		// Token: 0x0600127C RID: 4732 RVA: 0x0003EBC0 File Offset: 0x0003CDC0
		public static ValueTuple<bool, PXCache> AddOrGetCache(PXGraph graph, Type itemType)
		{
			bool flag = itemType == null || graph == null;
			ValueTuple<bool, PXCache> result;
			if (flag)
			{
				result = new ValueTuple<bool, PXCache>(false, null);
			}
			else
			{
				bool flag2 = !itemType.Implements(typeof(IBqlTable));
				if (flag2)
				{
					throw new PXException("Type passed '{0}' in not an IBqlTable", new object[]
					{
						itemType.ToString()
					});
				}
				PXCache item;
				bool flag3 = graph.Caches.TryGetValue(itemType, out item);
				if (flag3)
				{
					result = new ValueTuple<bool, PXCache>(false, item);
				}
				else
				{
					Type type = typeof(PXCache).MakeGenericType(new Type[]
					{
						itemType
					});
					PXCache pxcache = (PXCache)Activator.CreateInstance(type, new object[]
					{
						graph
					});
					pxcache.Load();
					graph.Caches[itemType] = pxcache;
					result = new ValueTuple<bool, PXCache>(true, pxcache);
				}
			}
			return result;
		}

		// Token: 0x0600127D RID: 4733 RVA: 0x0003EC98 File Offset: 0x0003CE98
		public static void AddRowToCache(PXGraph graph, object row)
		{
			bool flag = row == null || graph == null;
			if (!flag)
			{
				bool flag2 = !(row is IBqlTable);
				if (flag2)
				{
					throw new PXException("Type passed '{0}' in not an IBqlTable", new object[]
					{
						row.GetType()
					});
				}
				PXCache pxcache = graph.Caches[row.GetType()];
				pxcache.Current = row;
				pxcache.SetStatus(row, 5);
			}
		}

		// Token: 0x0600127E RID: 4734 RVA: 0x0003ED08 File Offset: 0x0003CF08
		public static TOut GetCurrentValue<TOut>(PXGraph graph, Type field)
		{
			Type itemType = BqlCommand.GetItemType(field);
			PXCache pxcache = graph.Caches[itemType];
			return (TOut)((object)pxcache.GetValue(pxcache.Current, field.Name));
		}

		// Token: 0x0600127F RID: 4735 RVA: 0x0003ED48 File Offset: 0x0003CF48
		public static string AggregateJoin<TSource>(this IEnumerable<TSource> source, Func<TSource, string> func, string separator)
		{
			return source.Aggregate(new StringBuilder(), (StringBuilder current, TSource next) => current.Append((current.Length == 0) ? "" : separator).Append(func(next))).ToString();
		}

		// Token: 0x06001280 RID: 4736 RVA: 0x0003ED8C File Offset: 0x0003CF8C
		public static IList GetGenericList(Type itemType)
		{
			Type typeFromHandle = typeof(List<>);
			Type type = typeFromHandle.MakeGenericType(new Type[]
			{
				itemType
			});
			object obj = Activator.CreateInstance(type);
			return (IList)obj;
		}

		// Token: 0x06001281 RID: 4737 RVA: 0x0003EDC8 File Offset: 0x0003CFC8
		public static object GetRow(this IPXResultset rs, int rowNb, int tableNb)
		{
			object obj = rs.GetItem(rowNb, tableNb);
			int rowCount = rs.GetRowCount();
			bool flag = obj == null && rowCount > 0 && rowNb < rowCount;
			if (flag)
			{
				IList list = (IList)rs.GetCollection();
				object obj2 = list[rowNb];
				PXResult pxresult = obj2 as PXResult;
				bool flag2 = pxresult != null;
				if (flag2)
				{
					obj = pxresult[tableNb];
				}
				else
				{
					GenericResult genericResult = obj2 as GenericResult;
					bool flag3 = genericResult != null;
					if (flag3)
					{
						obj = genericResult.Values.Values.Skip(tableNb).FirstOrDefault<object>();
					}
				}
			}
			return obj;
		}

		// Token: 0x06001282 RID: 4738 RVA: 0x0003EE68 File Offset: 0x0003D068
		public static bool IsOnlyLetter(string value)
		{
			bool flag = string.IsNullOrEmpty(value);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				Func<char, bool> predicate;
				if ((predicate = AsgardUtils.<>O.<7>__IsLetter) == null)
				{
					predicate = (AsgardUtils.<>O.<7>__IsLetter = new Func<char, bool>(char.IsLetter));
				}
				result = value.All(predicate);
			}
			return result;
		}

		// Token: 0x06001283 RID: 4739 RVA: 0x0003EEAC File Offset: 0x0003D0AC
		public static bool HasLetter(string value)
		{
			bool result;
			if (!string.IsNullOrEmpty(value))
			{
				Func<char, bool> predicate;
				if ((predicate = AsgardUtils.<>O.<7>__IsLetter) == null)
				{
					predicate = (AsgardUtils.<>O.<7>__IsLetter = new Func<char, bool>(char.IsLetter));
				}
				result = value.Any(predicate);
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06001284 RID: 4740 RVA: 0x0003EEEC File Offset: 0x0003D0EC
		public static bool IsOnlyDigit(string value)
		{
			bool result;
			if (!string.IsNullOrEmpty(value))
			{
				Func<char, bool> predicate;
				if ((predicate = AsgardUtils.<>O.<8>__IsDigit) == null)
				{
					predicate = (AsgardUtils.<>O.<8>__IsDigit = new Func<char, bool>(char.IsDigit));
				}
				result = value.All(predicate);
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06001285 RID: 4741 RVA: 0x0003EF2C File Offset: 0x0003D12C
		public static bool HasDigit(string value)
		{
			bool result;
			if (!string.IsNullOrEmpty(value))
			{
				Func<char, bool> predicate;
				if ((predicate = AsgardUtils.<>O.<8>__IsDigit) == null)
				{
					predicate = (AsgardUtils.<>O.<8>__IsDigit = new Func<char, bool>(char.IsDigit));
				}
				result = value.Any(predicate);
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06001286 RID: 4742 RVA: 0x0003EF6C File Offset: 0x0003D16C
		public static bool IsGuid(string value)
		{
			Guid guid;
			return !string.IsNullOrEmpty(value) && Guid.TryParse(value, out guid);
		}

		// Token: 0x06001287 RID: 4743 RVA: 0x0003EF94 File Offset: 0x0003D194
		public static bool IsCompatibleWith(this Type toCheck, Type ofPotentialBase)
		{
			bool flag = toCheck.IsSubclassOf(ofPotentialBase);
			bool flag2 = toCheck.IsGenericType ? toCheck.IsSubclassOfRawGeneric(ofPotentialBase) : flag;
			bool flag3 = ofPotentialBase.IsAssignableFrom(toCheck);
			bool flag4 = toCheck == ofPotentialBase;
			bool flag5 = flag || flag2 || flag3 || flag4;
			bool result;
			if (flag5)
			{
				result = true;
			}
			else
			{
				Type[] interfaces = toCheck.GetInterfaces();
				foreach (Type type in interfaces)
				{
					bool flag6 = type.IsGenericType && type.GetGenericTypeDefinition() == ofPotentialBase;
					if (flag6)
					{
						return true;
					}
				}
				result = false;
			}
			return result;
		}

		// Token: 0x06001288 RID: 4744 RVA: 0x0003F038 File Offset: 0x0003D238
		public static bool IsInstanceOfGenericType(object instance, Type genericType)
		{
			Type type = instance.GetType();
			while (type != null)
			{
				bool flag = type.IsGenericType && type.GetGenericTypeDefinition() == genericType;
				if (flag)
				{
					return true;
				}
				type = type.BaseType;
			}
			return false;
		}

		// Token: 0x06001289 RID: 4745 RVA: 0x0003F088 File Offset: 0x0003D288
		public static E FindExtension<E>(object row)
		{
			bool flag = row is PXResult;
			if (flag)
			{
				row = PXResult.UnwrapMain(row);
			}
			bool flag2 = !(row is IBqlTable);
			E result;
			if (flag2)
			{
				result = default(E);
			}
			else
			{
				PXCacheExtension[] extensions = PXCacheEx.GetExtensions((IBqlTable)row);
				bool flag3 = extensions == null;
				if (flag3)
				{
					result = default(E);
				}
				else
				{
					E e = extensions.FindFirst<PXCacheExtension, E>();
					result = e;
				}
			}
			return result;
		}

		// Token: 0x0600128A RID: 4746 RVA: 0x0003F100 File Offset: 0x0003D300
		public static C FindFirst<T, C>(this IEnumerable<T> listOfTs)
		{
			bool flag = listOfTs == null;
			C result;
			if (flag)
			{
				result = default(C);
			}
			else
			{
				C c = listOfTs.OfType<C>().FirstOrDefault<C>();
				result = c;
			}
			return result;
		}

		// Token: 0x0600128B RID: 4747 RVA: 0x0003F134 File Offset: 0x0003D334
		public static bool IsBqlTable(this Type type)
		{
			return type.IsCompatibleWith(typeof(IBqlTable));
		}

		// Token: 0x0600128C RID: 4748 RVA: 0x0003F158 File Offset: 0x0003D358
		private static bool IsSubclassOfRawGeneric(this Type toCheck, Type baseType)
		{
			while (toCheck != typeof(object))
			{
				Type right = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
				bool flag = baseType == right;
				if (flag)
				{
					return true;
				}
				toCheck = toCheck.BaseType;
			}
			return false;
		}

		// Token: 0x0600128D RID: 4749 RVA: 0x0003F1B0 File Offset: 0x0003D3B0
		public static string GetFieldsAsString(this IEnumerable<string> fields)
		{
			return string.Join(",", fields);
		}

		// Token: 0x0600128E RID: 4750 RVA: 0x0003F1D0 File Offset: 0x0003D3D0
		public static T[] Concat<T>(params T[][] arrays)
		{
			T[] array = new T[arrays.Sum((T[] a) => a.Length)];
			int num = 0;
			foreach (T[] array2 in arrays)
			{
				array2.CopyTo(array, num);
				num += array2.Length;
			}
			return array;
		}

		// Token: 0x0600128F RID: 4751 RVA: 0x0003F23C File Offset: 0x0003D43C
		public static Func<T1, Action<T2>> Curry<T1, T2>(Action<T1, T2> function)
		{
			return (T1 a) => delegate(T2 b)
			{
				function(a, b);
			};
		}

		// Token: 0x06001290 RID: 4752 RVA: 0x0003F268 File Offset: 0x0003D468
		public static Func<T1, Func<T2, Action<T3>>> Curry<T1, T2, T3>(Action<T1, T2, T3> function)
		{
			return (T1 a) => (T2 b) => delegate(T3 c)
			{
				function(a, b, c);
			};
		}

		// Token: 0x06001291 RID: 4753 RVA: 0x0003F294 File Offset: 0x0003D494
		public static Func<T1, Func<T2, Func<T3, Action<T4>>>> Curry<T1, T2, T3, T4>(Action<T1, T2, T3, T4> function)
		{
			return (T1 a) => (T2 b) => (T3 c) => delegate(T4 d)
			{
				function(a, b, c, d);
			};
		}

		// Token: 0x06001292 RID: 4754 RVA: 0x0003F2C0 File Offset: 0x0003D4C0
		public static Func<T1, Func<T2, Func<T3, Func<T4, Action<T5>>>>> Curry<T1, T2, T3, T4, T5>(Action<T1, T2, T3, T4, T5> function)
		{
			return (T1 a) => (T2 b) => (T3 c) => (T4 d) => delegate(T5 e)
			{
				function(a, b, c, d, e);
			};
		}

		// Token: 0x06001293 RID: 4755 RVA: 0x0003F2EC File Offset: 0x0003D4EC
		public static Func<T1, Func<T2, TResult>> Curry<T1, T2, TResult>(Func<T1, T2, TResult> function)
		{
			return (T1 a) => (T2 b) => function(a, b);
		}

		// Token: 0x06001294 RID: 4756 RVA: 0x0003F318 File Offset: 0x0003D518
		public static Func<T1, Func<T2, Func<T3, TResult>>> Curry<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> function)
		{
			return (T1 a) => (T2 b) => (T3 c) => function(a, b, c);
		}

		// Token: 0x06001295 RID: 4757 RVA: 0x0003F344 File Offset: 0x0003D544
		public static Func<T1, Func<T2, Func<T3, Func<T4, TResult>>>> Curry<T1, T2, T3, T4, TResult>(Func<T1, T2, T3, T4, TResult> function)
		{
			return (T1 a) => (T2 b) => (T3 c) => (T4 d) => function(a, b, c, d);
		}

		// Token: 0x06001296 RID: 4758 RVA: 0x0003F370 File Offset: 0x0003D570
		public static Func<T1, Func<T2, Func<T3, Func<T4, Func<T5, TResult>>>>> Curry<T1, T2, T3, T4, T5, TResult>(Func<T1, T2, T3, T4, T5, TResult> function)
		{
			return (T1 a) => (T2 b) => (T3 c) => (T4 d) => (T5 e) => function(a, b, c, d, e);
		}

		// Token: 0x06001297 RID: 4759 RVA: 0x0003F39C File Offset: 0x0003D59C
		public static Action<T2> CurryAndCall<T1, T2>(Action<T1, T2> function, T1 arg1)
		{
			Func<T1, Action<T2>> func = AsgardUtils.Curry<T1, T2>(function);
			return func(arg1);
		}

		// Token: 0x06001298 RID: 4760 RVA: 0x0003F3C0 File Offset: 0x0003D5C0
		public static Action<T3> CurryAndCall<T1, T2, T3>(Action<T1, T2, T3> function, T1 arg1, T2 arg2)
		{
			Func<T1, Func<T2, Action<T3>>> func = AsgardUtils.Curry<T1, T2, T3>(function);
			return func(arg1)(arg2);
		}

		// Token: 0x06001299 RID: 4761 RVA: 0x0003F3E8 File Offset: 0x0003D5E8
		public static Func<T2, TResult> CurryAndCall<T1, T2, TResult>(Func<T1, T2, TResult> function, T1 arg1)
		{
			Func<T1, Func<T2, TResult>> func = AsgardUtils.Curry<T1, T2, TResult>(function);
			return func(arg1);
		}

		// Token: 0x0600129A RID: 4762 RVA: 0x0003F40C File Offset: 0x0003D60C
		public static Func<T3, TResult> CurryAndCall<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> function, T1 arg1, T2 arg2)
		{
			Func<T1, Func<T2, Func<T3, TResult>>> func = AsgardUtils.Curry<T1, T2, T3, TResult>(function);
			return func(arg1)(arg2);
		}

		// Token: 0x0600129B RID: 4763 RVA: 0x0003F434 File Offset: 0x0003D634
		public static Func<T4, TResult> CurryAndCall<T1, T2, T3, T4, TResult>(Func<T1, T2, T3, T4, TResult> function, T1 arg1, T2 arg2, T3 arg3)
		{
			Func<T1, Func<T2, Func<T3, Func<T4, TResult>>>> func = AsgardUtils.Curry<T1, T2, T3, T4, TResult>(function);
			return func(arg1)(arg2)(arg3);
		}

		// Token: 0x0600129C RID: 4764 RVA: 0x0003F464 File Offset: 0x0003D664
		public static Func<T5, TResult> CurryAndCall<T1, T2, T3, T4, T5, TResult>(Func<T1, T2, T3, T4, T5, TResult> function, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
		{
			Func<T1, Func<T2, Func<T3, Func<T4, Func<T5, TResult>>>>> func = AsgardUtils.Curry<T1, T2, T3, T4, T5, TResult>(function);
			return func(arg1)(arg2)(arg3)(arg4);
		}

		// Token: 0x0600129D RID: 4765 RVA: 0x0003F49C File Offset: 0x0003D69C
		public static string HoursToTimeSpanStr(decimal nbHours)
		{
			int minutes = (int)(nbHours * 60m);
			return new TimeSpan(0, minutes, 0).ToString("hh\\:mm");
		}

		// Token: 0x0600129E RID: 4766 RVA: 0x0003F4D8 File Offset: 0x0003D6D8
		public static int HoursToMinutes(decimal nbHours)
		{
			return (int)(nbHours * 60m);
		}

		// Token: 0x0600129F RID: 4767 RVA: 0x0003F500 File Offset: 0x0003D700
		public static int TimeSpanToMinutes(object timeSpanObj)
		{
			bool flag = timeSpanObj is string;
			if (flag)
			{
				string text = timeSpanObj.ToString();
				timeSpanObj = TimeSpan.FromHours(Convert.ToDouble(text.Split(new char[]
				{
					':'
				})[0])).Add(TimeSpan.FromMinutes(Convert.ToDouble(text.Split(new char[]
				{
					':'
				})[1])));
			}
			TimeSpan timeSpan;
			bool flag2;
			if (timeSpanObj is TimeSpan)
			{
				timeSpan = (TimeSpan)timeSpanObj;
				flag2 = true;
			}
			else
			{
				flag2 = false;
			}
			bool flag3 = flag2;
			int result;
			if (flag3)
			{
				int num = timeSpan.Hours * 60 + timeSpan.Minutes;
				result = num;
			}
			else
			{
				result = 0;
			}
			return result;
		}

		// Token: 0x060012A0 RID: 4768 RVA: 0x0003F5A8 File Offset: 0x0003D7A8
		public static TimeSpan GetSpan(DateTime? startDate, DateTime? endDate, TimeSpan defaultSpan = default(TimeSpan))
		{
			bool flag = startDate != null && endDate != null;
			TimeSpan result;
			if (flag)
			{
				result = endDate.Value.Subtract(startDate.Value);
			}
			else
			{
				result = defaultSpan;
			}
			return result;
		}

		// Token: 0x060012A1 RID: 4769 RVA: 0x0003F5EC File Offset: 0x0003D7EC
		public static bool IsPrimitive(object result)
		{
			bool flag = result == null;
			return flag || AsgardUtils.IsPrimitive(result.GetType());
		}

		// Token: 0x060012A2 RID: 4770 RVA: 0x0003F618 File Offset: 0x0003D818
		public static bool IsPrimitive(Type type)
		{
			bool flag = type == null;
			bool result;
			if (flag)
			{
				result = true;
			}
			else
			{
				bool flag2 = type.IsPrimitive || type.IsValueType || AsgardUtils.IsSimpleType(type);
				result = flag2;
			}
			return result;
		}

		// Token: 0x060012A3 RID: 4771 RVA: 0x0003F658 File Offset: 0x0003D858
		public static bool IsNumber(string result)
		{
			bool flag = !string.IsNullOrEmpty(result);
			if (flag)
			{
				bool flag2 = result.StartsWith("0");
				if (flag2)
				{
					return false;
				}
				decimal num;
				double num2;
				bool flag3 = decimal.TryParse(result, out num) || double.TryParse(result, out num2);
				if (flag3)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060012A4 RID: 4772 RVA: 0x0003F6AC File Offset: 0x0003D8AC
		public static bool IsSimpleType(Type type)
		{
			return type.IsPrimitive || AsgardUtils.SIMPLE_TYPES.Contains(type) || Convert.GetTypeCode(type) != TypeCode.Object || (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>) && AsgardUtils.IsSimpleType(type.GetGenericArguments()[0]));
		}

		// Token: 0x060012A5 RID: 4773 RVA: 0x0003F710 File Offset: 0x0003D910
		public static bool IsNumeric(this Type type)
		{
			TypeCode typeCode = Type.GetTypeCode(type);
			TypeCode typeCode2 = typeCode;
			bool result;
			if (typeCode2 != TypeCode.Object)
			{
				result = (typeCode2 - TypeCode.SByte <= 10);
			}
			else
			{
				bool flag = type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
				result = (flag && Nullable.GetUnderlyingType(type).IsNumeric());
			}
			return result;
		}

		// Token: 0x060012A6 RID: 4774 RVA: 0x0003F778 File Offset: 0x0003D978
		public static object ChangeType(object value, Type type, IFormatProvider provider = null)
		{
			bool flag = value == null;
			if (!flag)
			{
				try
				{
					Type type2 = value.GetType();
					bool flag2 = type2 == type;
					if (flag2)
					{
						return value;
					}
					bool isEnum = type.IsEnum;
					if (isEnum)
					{
						string text = value as string;
						bool flag3 = text != null;
						if (flag3)
						{
							return Enum.Parse(type, text);
						}
						bool flag4 = type2.IsNumeric();
						if (flag4)
						{
							value = Convert.ChangeType(value, typeof(long));
							return Enum.ToObject(type, (long)value);
						}
					}
					else
					{
						string text2;
						bool flag5;
						if (type == typeof(DateTime))
						{
							text2 = (value as string);
							flag5 = (text2 != null);
						}
						else
						{
							flag5 = false;
						}
						bool flag6 = flag5;
						if (flag6)
						{
							string[] formats = new string[]
							{
								"yyyyMMdd",
								"yyyyMMddHHmmss",
								"yyyyMMddHHmmss.fff"
							};
							bool flag7 = text2.Length == 8 || text2.Length == 14 || text2.Length == 18;
							if (flag7)
							{
								return DateTime.ParseExact(text2, formats, null, DateTimeStyles.NoCurrentDateDefault);
							}
							return DateTimeOffset.ParseExact(text2, new string[]
							{
								"yyyyMMdd zzz",
								"yyyyMMddHHmmss zzz",
								"yyyyMMddHHmmss.fff zzz"
							}, null, DateTimeStyles.AdjustToUniversal).DateTime;
						}
						else
						{
							string text3 = value as string;
							bool flag8 = text3 != null && type.IsAssignableFrom(typeof(ICollection)) && text3.Trim().StartsWith("[") && text3.Trim().EndsWith("]");
							if (flag8)
							{
								return JsonConvert.DeserializeAnonymousType<Type>(text3, type);
							}
							string text4 = value as string;
							bool flag9 = text4 != null && type.IsAssignableFrom(typeof(IDictionary)) && text4.Trim().StartsWith("{") && text4.Trim().EndsWith("}");
							if (flag9)
							{
								return JsonConvert.DeserializeAnonymousType<Type>(text4, type);
							}
							bool flag10 = value is IConvertible;
							if (flag10)
							{
								return Convert.ChangeType(value, type, provider);
							}
						}
					}
				}
				catch
				{
					throw;
				}
				throw new PXException("Unable to convert '{0}' to type '{1}'", new object[]
				{
					value,
					type.FullName
				});
			}
			return null;
		}

		// Token: 0x060012A7 RID: 4775 RVA: 0x0003F9FC File Offset: 0x0003DBFC
		public static IEnumerable<TResult> LeftJoin<TOuter, TInner, TResult>(this IEnumerable<TOuter> outer, IEnumerable<TInner> inner, Func<TOuter, object> outerKeySelector, Func<TInner, object> innerKeySelector, Func<TOuter, TInner, TResult> resultSelector)
		{
			return outer.GroupJoin(inner, outerKeySelector, innerKeySelector, (TOuter ou, IEnumerable<TInner> inns) => new
			{
				ou,
				inns
			}).SelectMany(z => z.inns.DefaultIfEmpty<TInner>(), (ou, TInner inn) => resultSelector(ou.ou, inn));
		}

		// Token: 0x060012A8 RID: 4776 RVA: 0x0003FA74 File Offset: 0x0003DC74
		public static IEnumerable<TResult> FullOuterGroupJoin<TOuter, TInner, TKey, TResult>(this IEnumerable<TOuter> outer, IEnumerable<TInner> inner, Func<TOuter, TKey> keySelectorOuter, Func<TInner, TKey> keySelectorInner, Func<IEnumerable<TOuter>, IEnumerable<TInner>, TKey, TResult> projection, IEqualityComparer<TKey> cmp = null)
		{
			if (cmp == null)
			{
				cmp = EqualityComparer<TKey>.Default;
			}
			ILookup<TKey, TOuter> outerLookup = outer.ToLookup(keySelectorOuter, cmp);
			ILookup<TKey, TInner> innerLookup = inner.ToLookup(keySelectorInner, cmp);
			HashSet<TKey> hashSet = new HashSet<TKey>(from p in outerLookup
			select p.Key, cmp);
			hashSet.UnionWith(from p in innerLookup
			select p.Key);
			return from key in hashSet
			let xa = outerLookup[key]
			let xb = innerLookup[key]
			select projection(xa, xb, key);
		}

		// Token: 0x060012A9 RID: 4777 RVA: 0x0003FB58 File Offset: 0x0003DD58
		public static IEnumerable<TResult> FullOuterJoin<TOuter, TInner, TKey, TResult>(this IEnumerable<TOuter> outer, IEnumerable<TInner> inner, Func<TOuter, TKey> keySelectorOuter, Func<TInner, TKey> keySelectorInner, Func<TOuter, TInner, TKey, TResult> projection, TOuter defaultOuter = default(TOuter), TInner defaultInner = default(TInner), IEqualityComparer<TKey> cmp = null)
		{
			if (cmp == null)
			{
				cmp = EqualityComparer<TKey>.Default;
			}
			ILookup<TKey, TOuter> outerLookup = outer.ToLookup(keySelectorOuter, cmp);
			ILookup<TKey, TInner> innerLookup = inner.ToLookup(keySelectorInner, cmp);
			HashSet<TKey> hashSet = new HashSet<TKey>(from p in outerLookup
			select p.Key, cmp);
			hashSet.UnionWith(from p in innerLookup
			select p.Key);
			return from key in hashSet
			from xa in outerLookup[key].DefaultIfEmpty(defaultOuter)
			from xb in innerLookup[key].DefaultIfEmpty(defaultInner)
			select projection(xa, xb, key);
		}

		// Token: 0x060012AA RID: 4778 RVA: 0x0003FC64 File Offset: 0x0003DE64
		public static void CopyAddress<FAddress, FAddressID, FRow, FRowID, TAddress, TRow, TAddressID>(PXCache fromCache, FRow fromRow, PXCache destCache, TRow destRow) where FAddress : class, IBqlTable, IAddress, new() where FAddressID : IBqlField where FRow : class, IBqlTable, new() where FRowID : IBqlField where TAddress : class, IBqlTable, IAddress, new() where TRow : class, IBqlTable, new() where TAddressID : IBqlField
		{
			FAddress faddress = PXSelectBase<FAddress, PXSelect<FAddress, Where<FAddressID, Equal<Current<FRowID>>>>.Config>.SelectSingleBound(fromCache.Graph, new object[]
			{
				fromRow
			}, Array.Empty<object>());
			TAddress taddress = Activator.CreateInstance<TAddress>();
			AsgardUtils.Copy(taddress, faddress);
			TAddress taddress2 = (TAddress)((object)destCache.Graph.Caches[typeof(TAddress)].Insert(taddress));
			string name = typeof(TAddressID).Name;
			destCache.SetValue(destRow, name, taddress2.AddressID);
		}

		// Token: 0x060012AB RID: 4779 RVA: 0x0003FD08 File Offset: 0x0003DF08
		public static void CopyContact<FContact, FContactID, FRow, FRowID, TContact, TRow, TContactID>(PXCache fromCache, FRow fromRow, PXCache destCache, TRow destRow) where FContact : class, IBqlTable, IContact, new() where FContactID : IBqlField where FRow : class, IBqlTable, new() where FRowID : IBqlField where TContact : class, IBqlTable, IContact, new() where TRow : class, IBqlTable, new() where TContactID : IBqlField
		{
			FContact fcontact = PXSelectBase<FContact, PXSelect<FContact, Where<FContactID, Equal<Current<FRowID>>>>.Config>.SelectSingleBound(fromCache.Graph, new object[]
			{
				fromRow
			}, Array.Empty<object>());
			TContact tcontact = Activator.CreateInstance<TContact>();
			AsgardUtils.Copy(tcontact, fcontact);
			TContact tcontact2 = (TContact)((object)destCache.Graph.Caches[typeof(TContact)].Insert(tcontact));
			string name = typeof(TContactID).Name;
			destCache.SetValue(destRow, name, tcontact2.ContactID);
		}

		// Token: 0x060012AC RID: 4780 RVA: 0x0003FDAC File Offset: 0x0003DFAC
		private static void Copy(IAddress dest, IAddress source)
		{
			dest.BAccountID = source.BAccountID;
			dest.BAccountAddressID = source.BAccountAddressID;
			dest.RevisionID = source.RevisionID;
			dest.IsDefaultAddress = source.IsDefaultAddress;
			dest.AddressLine1 = source.AddressLine1;
			dest.AddressLine2 = source.AddressLine2;
			dest.AddressLine3 = source.AddressLine3;
			dest.City = source.City;
			dest.CountryID = source.CountryID;
			dest.State = source.State;
			dest.PostalCode = source.PostalCode;
		}

		// Token: 0x060012AD RID: 4781 RVA: 0x0003FE4C File Offset: 0x0003E04C
		private static void Copy(IContact dest, IContact source)
		{
			dest.BAccountID = source.BAccountID;
			dest.BAccountContactID = source.BAccountContactID;
			dest.RevisionID = source.RevisionID;
			dest.IsDefaultContact = source.IsDefaultContact;
			dest.FullName = source.FullName;
			dest.Salutation = source.Salutation;
			dest.Title = source.Title;
			dest.Phone1 = source.Phone1;
			dest.Phone1Type = source.Phone1Type;
			dest.Phone2 = source.Phone2;
			dest.Phone2Type = source.Phone2Type;
			dest.Phone3 = source.Phone3;
			dest.Phone3Type = source.Phone3Type;
			dest.Fax = source.Fax;
			dest.FaxType = source.FaxType;
			dest.Email = source.Email;
		}

		// Token: 0x060012AE RID: 4782 RVA: 0x0003FF2A File Offset: 0x0003E12A
		public static IEnumerable<string> SplitToLines(this string input)
		{
			AsgardUtils.<SplitToLines>d__155 <SplitToLines>d__ = new AsgardUtils.<SplitToLines>d__155(-2);
			<SplitToLines>d__.<>3__input = input;
			return <SplitToLines>d__;
		}

		// Token: 0x060012AF RID: 4783 RVA: 0x0003FF3C File Offset: 0x0003E13C
		public static string ExtractMessage(Exception ex)
		{
			string text = ex.Message + ": ";
			PXOuterException ex2 = ex as PXOuterException;
			bool flag = ex2 != null && ex2.InnerMessages != null;
			if (flag)
			{
				foreach (string str in ex2.InnerMessages)
				{
					text = text + str + ", ";
				}
			}
			else
			{
				while (ex.InnerException != null)
				{
					text = text + ex.InnerException.Message + ", ";
					ex = ex.InnerException;
				}
			}
			return text.Trim(new char[]
			{
				' ',
				',',
				':'
			});
		}

		// Token: 0x060012B0 RID: 4784 RVA: 0x0003FFFC File Offset: 0x0003E1FC
		public static object GetValueFromCache(PXCache cache, object row, string fieldName)
		{
			bool flag = cache == null || row == null || string.IsNullOrEmpty(fieldName);
			object result;
			if (flag)
			{
				result = null;
			}
			else
			{
				object obj = cache.GetValue(row, fieldName);
				if (obj == null)
				{
					obj = cache.GetValueExt(row, fieldName);
				}
				if (obj == null)
				{
					obj = cache.GetValuePending(row, fieldName);
				}
				obj = PXFieldState.UnwrapValue(obj);
				result = obj;
			}
			return result;
		}

		// Token: 0x060012B1 RID: 4785 RVA: 0x00040050 File Offset: 0x0003E250
		public static void ForEach<T>(this IEnumerable<T> sequence, Action<int, T> action)
		{
			int num = 0;
			foreach (T arg in sequence)
			{
				action(num, arg);
				num++;
			}
		}

		// Token: 0x060012B2 RID: 4786 RVA: 0x000400A8 File Offset: 0x0003E2A8
		public static void ForEachWithPrev<T>(this IEnumerable<T> sequence, Action<int, T, T> action)
		{
			int num = 0;
			T arg = default(T);
			foreach (T t in sequence)
			{
				action(num, t, arg);
				num++;
				arg = t;
			}
		}

		// Token: 0x060012B3 RID: 4787 RVA: 0x00040108 File Offset: 0x0003E308
		public static string GetHttpErrorName(int httpCode)
		{
			return Enum.GetName(AsgardUtils.MissingHttpStatusValues.Contains(httpCode) ? typeof(AsgardUtils.MissingHttpStatus) : typeof(HttpStatusCode), httpCode);
		}

		// Token: 0x060012B4 RID: 4788 RVA: 0x0004014C File Offset: 0x0003E34C
		public static Type GetType(string typename)
		{
			bool flag = typename == null;
			Type result;
			if (flag)
			{
				result = null;
			}
			else
			{
				result = (PXBuildManager.GetType(typename, false) ?? Type.GetType(typename));
			}
			return result;
		}

		// Token: 0x060012B5 RID: 4789 RVA: 0x0004017C File Offset: 0x0003E37C
		public static IEnumerable GetViews(string graphTypeName)
		{
			AsgardUtils.<GetViews>d__164 <GetViews>d__ = new AsgardUtils.<GetViews>d__164(-2);
			<GetViews>d__.<>3__graphTypeName = graphTypeName;
			return <GetViews>d__;
		}

		// Token: 0x060012B6 RID: 4790 RVA: 0x0004018C File Offset: 0x0003E38C
		public static IEnumerable GetViews(Type graphType)
		{
			AsgardUtils.<GetViews>d__165 <GetViews>d__ = new AsgardUtils.<GetViews>d__165(-2);
			<GetViews>d__.<>3__graphType = graphType;
			return <GetViews>d__;
		}

		// Token: 0x060012B7 RID: 4791 RVA: 0x0004019C File Offset: 0x0003E39C
		public static string Merge(this IEnumerable<string> strs, string separator)
		{
			return string.Join(separator, strs);
		}

		// Token: 0x060012B8 RID: 4792 RVA: 0x000401B8 File Offset: 0x0003E3B8
		public static string Merge(this IEnumerable<string> strs)
		{
			return strs.Merge(string.Empty);
		}

		// Token: 0x060012B9 RID: 4793 RVA: 0x000401D8 File Offset: 0x0003E3D8
		public static string Merge(params string[] strs)
		{
			return strs.Merge(string.Empty);
		}

		// Token: 0x060012BA RID: 4794 RVA: 0x000401F8 File Offset: 0x0003E3F8
		public static string Prepend(this string str, params string[] sequences)
		{
			string text = str;
			foreach (string value in sequences.Reverse<string>())
			{
				text = text.Insert(0, value);
			}
			return text;
		}

		// Token: 0x060012BB RID: 4795 RVA: 0x00040254 File Offset: 0x0003E454
		public static string StringifyResult(PXGraph graph, object value)
		{
			bool flag = value == null || graph == null;
			string result;
			if (flag)
			{
				result = "NULL";
			}
			else
			{
				try
				{
					Type itemType = AsgardUtils.GetItemType(value);
					PXCache cache = graph.Caches[itemType];
					return AsgardUtils.StringifyRow(cache, value);
				}
				catch (PXException ex)
				{
					PXTrace.WriteError(ex);
				}
				result = "-- Bad result --";
			}
			return result;
		}

		// Token: 0x060012BC RID: 4796 RVA: 0x000402C0 File Offset: 0x0003E4C0
		private static string StringifyRow(PXCache cache, object value)
		{
			IPXResultset ipxresultset = value as IPXResultset;
			bool flag = ipxresultset != null;
			string result;
			if (flag)
			{
				result = AsgardUtils.StringifyPXResultset(ipxresultset);
			}
			else
			{
				PXResult pxresult = value as PXResult;
				bool flag2 = pxresult != null;
				if (flag2)
				{
					result = AsgardUtils.StringifyPXResult(cache, pxresult);
				}
				else
				{
					IBqlTable bqlTable = value as IBqlTable;
					bool flag3 = bqlTable != null;
					if (flag3)
					{
						result = AsgardUtils.StringifyBqlTable(cache, bqlTable);
					}
					else
					{
						Type itemType = AsgardUtils.GetItemType(value);
						result = "An object of type '" + itemType.Name + "' : value.ToString()";
					}
				}
			}
			return result;
		}

		// Token: 0x060012BD RID: 4797 RVA: 0x00040348 File Offset: 0x0003E548
		private static string StringifyPXResultset(IPXResultset rs)
		{
			IList<Type> itemTypes = rs.GetItemTypes();
			IEnumerable<string> values = from it in itemTypes
			select it.Name;
			string str = string.Join(", ", values);
			return "PXResultset: (" + str + ")";
		}

		// Token: 0x060012BE RID: 4798 RVA: 0x000403A4 File Offset: 0x0003E5A4
		private static string StringifyPXResult(PXCache cache, PXResult pxr)
		{
			Type[] tables = pxr.Tables;
			IEnumerable<string> values = from it in tables
			select it.Name;
			string text = string.Join(", ", values);
			string keys = AsgardUtils.GetKeys(cache, pxr, "|");
			return string.Concat(new string[]
			{
				"PXResult: (",
				keys,
				"), (",
				text,
				"/)"
			});
		}

		// Token: 0x060012BF RID: 4799 RVA: 0x0004042C File Offset: 0x0003E62C
		private static string StringifyBqlTable(PXCache cache, IBqlTable bql)
		{
			string name = bql.GetType().Name;
			string keys = AsgardUtils.GetKeys(cache, bql, "|");
			return name + ": (" + keys + ")";
		}

		// Token: 0x060012C0 RID: 4800 RVA: 0x00040468 File Offset: 0x0003E668
		public static string GetKeys(PXGraph graph, object row, string separator)
		{
			GenericResult genericResult = row as GenericResult;
			bool flag = genericResult != null;
			if (flag)
			{
				row = genericResult.Values.Values.FirstOrDefault<object>();
			}
			else
			{
				bool flag2 = row is PXResult;
				if (flag2)
				{
					row = PXResult.UnwrapMain(row);
				}
			}
			PXCache cache = ViewUtils.GetCache(graph, row);
			return AsgardUtils.GetKeys(cache, row, separator);
		}

		// Token: 0x060012C1 RID: 4801 RVA: 0x000404C8 File Offset: 0x0003E6C8
		public static string GetKeys(PXCache cache, object row, string separator)
		{
			GenericResult genericResult = row as GenericResult;
			bool flag = genericResult != null;
			if (flag)
			{
				row = genericResult.Values.Values.FirstOrDefault<object>();
			}
			else
			{
				PXResult pxresult = row as PXResult;
				bool flag2 = pxresult != null;
				if (flag2)
				{
					row = PXResult.UnwrapMain(pxresult);
				}
			}
			string[] value = (from key in AsgardUtils.GetKeys(cache, row)
			select (key != null) ? key.ToString() : null).ToArray<string>();
			return string.Join(separator, value);
		}

		// Token: 0x060012C2 RID: 4802 RVA: 0x00040554 File Offset: 0x0003E754
		private static object[] GetKeys(PXCache cache, object row)
		{
			string[] source = (from key in cache.BqlKeys
			select key.Name).ToArray<string>();
			return (from key in source
			select cache.GetValue(row, key)).ToArray<object>();
		}

		// Token: 0x060012C3 RID: 4803 RVA: 0x000405C8 File Offset: 0x0003E7C8
		public static string EnumToName<E>(E value) where E : Enum
		{
			return Enum.GetName(typeof(E), value);
		}

		// Token: 0x060012C4 RID: 4804 RVA: 0x000405F4 File Offset: 0x0003E7F4
		public static string AddLineNumbers(string str, int paddedLength = 4)
		{
			bool flag = string.IsNullOrEmpty(str);
			string result;
			if (flag)
			{
				result = str;
			}
			else
			{
				StringReader stringReader = new StringReader(str);
				int num = 1;
				StringBuilder stringBuilder = new StringBuilder(str.Length + 500);
				string str2;
				while ((str2 = stringReader.ReadLine()) != null)
				{
					string value = num++.ToString("D" + 3.ToString()) + ": " + str2 + "\n";
					stringBuilder.Append(value);
				}
				result = stringBuilder.ToString();
			}
			return result;
		}

		// Token: 0x060012C5 RID: 4805 RVA: 0x00040690 File Offset: 0x0003E890
		public static void CopyPropertiesTo<From, To>(From source, To dest)
		{
			List<PropertyInfo> list = (from x in typeof(From).GetProperties()
			where x.CanRead
			select x).ToList<PropertyInfo>();
			List<PropertyInfo> source2 = (from x in typeof(To).GetProperties()
			where x.CanWrite
			select x).ToList<PropertyInfo>();
			using (List<PropertyInfo>.Enumerator enumerator = list.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					PropertyInfo sourceProp = enumerator.Current;
					bool flag = source2.Any((PropertyInfo x) => x.Name == sourceProp.Name);
					if (flag)
					{
						PropertyInfo propertyInfo = source2.First((PropertyInfo x) => x.Name == sourceProp.Name);
						bool canWrite = propertyInfo.CanWrite;
						if (canWrite)
						{
							propertyInfo.SetValue(dest, sourceProp.GetValue(source, null), null);
						}
					}
				}
			}
		}

		// Token: 0x060012C6 RID: 4806 RVA: 0x000407B8 File Offset: 0x0003E9B8
		public static IDisposable GetUserScope()
		{
			string text = "admin";
			bool flag = PXDatabase.Companies.Length != 0;
			if (flag)
			{
				string text2 = PXAccess.GetCompanyName();
				bool flag2 = string.IsNullOrEmpty(text2);
				if (flag2)
				{
					text2 = PXDatabase.Companies[0];
				}
				text = text + "@" + text2;
			}
			return new PXLoginScope(text, Array.Empty<string>());
		}

		// Token: 0x060012C7 RID: 4807 RVA: 0x00040814 File Offset: 0x0003EA14
		public static T CreateDeepCopy<T>(T instance) where T : class
		{
			return PXReflectionSerializer.Clone<T>(instance);
		}

		// Token: 0x060012C8 RID: 4808 RVA: 0x00040830 File Offset: 0x0003EA30
		public static T CloneObjectWithIL<T>(T myObject)
		{
			Delegate @delegate;
			bool flag = !AsgardUtils._CACHED_IL.TryGetValue(typeof(T), out @delegate);
			if (flag)
			{
				DynamicMethod dynamicMethod = new DynamicMethod("DoClone", typeof(T), new Type[]
				{
					typeof(T)
				}, true);
				ConstructorInfo constructor = myObject.GetType().GetConstructor(Array.Empty<Type>());
				ILGenerator ilgenerator = dynamicMethod.GetILGenerator();
				ilgenerator.DeclareLocal(typeof(T));
				ilgenerator.Emit(OpCodes.Newobj, constructor);
				ilgenerator.Emit(OpCodes.Stloc_0);
				foreach (FieldInfo field in myObject.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
				{
					ilgenerator.Emit(OpCodes.Ldloc_0);
					ilgenerator.Emit(OpCodes.Ldarg_0);
					ilgenerator.Emit(OpCodes.Ldfld, field);
					ilgenerator.Emit(OpCodes.Stfld, field);
				}
				ilgenerator.Emit(OpCodes.Ldloc_0);
				ilgenerator.Emit(OpCodes.Ret);
				@delegate = dynamicMethod.CreateDelegate(typeof(Func<T, T>));
				AsgardUtils._CACHED_IL.Add(typeof(T), @delegate);
			}
			return ((Func<T, T>)@delegate)(myObject);
		}

		// Token: 0x060012C9 RID: 4809 RVA: 0x00040994 File Offset: 0x0003EB94
		public static Guid Int2Guid(int? intValue)
		{
			bool flag = intValue == null;
			Guid result;
			if (flag)
			{
				result = Guid.Empty;
			}
			else
			{
				byte[] array = new byte[16];
				BitConverter.GetBytes(intValue.Value).CopyTo(array, 0);
				result = new Guid(array);
			}
			return result;
		}

		// Token: 0x060012CA RID: 4810 RVA: 0x000409E0 File Offset: 0x0003EBE0
		public static int Guid2Int(Guid? guidValue)
		{
			bool flag = guidValue == null;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				byte[] value = guidValue.Value.ToByteArray();
				int num = BitConverter.ToInt32(value, 0);
				result = num;
			}
			return result;
		}

		// Token: 0x060012CB RID: 4811 RVA: 0x00040A20 File Offset: 0x0003EC20
		public static T GetValue<T>(IList<object> values, int index)
		{
			bool flag = values == null || !values.Any<object>() || values.Count < index + 1;
			T result;
			if (flag)
			{
				result = default(T);
			}
			else
			{
				object obj = values[index];
				bool flag2 = obj == null;
				if (flag2)
				{
					result = default(T);
				}
				else
				{
					result = (T)((object)obj);
				}
			}
			return result;
		}

		// Token: 0x060012CC RID: 4812 RVA: 0x00040A88 File Offset: 0x0003EC88
		public static bool IsHexa(IEnumerable<char> chars)
		{
			foreach (char c in chars)
			{
				bool flag = (c >= '0' && c <= '9') || (c >= 'a' && c <= 'f') || (c >= 'A' && c <= 'F');
				bool flag2 = !flag;
				if (flag2)
				{
					return false;
				}
			}
			return chars.Count<char>() % 2 == 0;
		}

		// Token: 0x060012CD RID: 4813 RVA: 0x00040B18 File Offset: 0x0003ED18
		public static string HexaToBinary(string hexa)
		{
			return string.Join(string.Empty, from c in hexa
			select Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0'));
		}

		// Token: 0x060012CE RID: 4814 RVA: 0x00040B5C File Offset: 0x0003ED5C
		public static bool IsBinary(string text)
		{
			return Regex.IsMatch(text, "^[01]+$");
		}

		// Token: 0x060012CF RID: 4815 RVA: 0x00040B7C File Offset: 0x0003ED7C
		public static string Truncate(this string value, int maxLength)
		{
			bool flag = string.IsNullOrEmpty(value);
			string result;
			if (flag)
			{
				result = value;
			}
			else
			{
				result = ((value.Length <= maxLength) ? value : value.Substring(0, maxLength));
			}
			return result;
		}

		// Token: 0x060012D0 RID: 4816 RVA: 0x00040BB0 File Offset: 0x0003EDB0
		public static Type GetGraphType(object data, PXEventSubscriberAttribute attr, PXGraph graph, Type fieldType)
		{
			Type type = attr.BqlTable;
			Type itemType = BqlCommand.GetItemType(fieldType);
			bool flag = type != itemType;
			if (flag)
			{
				type = itemType;
				data = null;
			}
			PXCache pxcache = graph.Caches[type];
			if (data == null)
			{
				data = pxcache.Current;
			}
			string text = (string)AsgardUtils.GetValueFromCache(pxcache, data, fieldType.Name);
			bool flag2 = string.IsNullOrEmpty(text);
			Type result;
			if (flag2)
			{
				result = null;
			}
			else
			{
				Type type2 = GraphHelper.GetType(text);
				result = type2;
			}
			return result;
		}

		// Token: 0x060012D1 RID: 4817 RVA: 0x00040C30 File Offset: 0x0003EE30
		public static string GetPlural(int nb)
		{
			return (nb > 1) ? "s" : "";
		}

		// Token: 0x060012D2 RID: 4818 RVA: 0x00040C54 File Offset: 0x0003EE54
		public static string GetVerb(int nb)
		{
			return (nb > 1) ? "have" : "has";
		}

		// Token: 0x060012D3 RID: 4819 RVA: 0x00040C78 File Offset: 0x0003EE78
		public static IEnumerable<Guid> GetFileIDs(Guid? noteID)
		{
			bool flag = noteID == null;
			IEnumerable<Guid> result;
			if (flag)
			{
				result = Enumerable.Empty<Guid>();
			}
			else
			{
				IEnumerable<PXDataRecord> enumerable = PXDatabase.SelectMulti<NoteDoc>(new PXDataField[]
				{
					new PXDataField<NoteDoc.fileID>(),
					new PXDataFieldValue<NoteDoc.noteID>(14, noteID)
				});
				List<Guid> list = new List<Guid>();
				foreach (PXDataRecord pxdataRecord in enumerable)
				{
					Guid value = pxdataRecord.GetGuid(0).Value;
					list.Add(value);
				}
				result = list;
			}
			return result;
		}

		// Token: 0x060012D4 RID: 4820 RVA: 0x00040D28 File Offset: 0x0003EF28
		public static string Strip(string expr, string startsWith, string endsWith)
		{
			bool flag = expr != null && expr.StartsWith(startsWith) && expr.EndsWith(endsWith);
			string result;
			if (flag)
			{
				string text = AsgardUtils.StripStart(expr, startsWith);
				text = AsgardUtils.StripEnd(text, endsWith);
				result = text;
			}
			else
			{
				result = expr;
			}
			return result;
		}

		// Token: 0x060012D5 RID: 4821 RVA: 0x00040D6C File Offset: 0x0003EF6C
		public static string StripEnd(string expr, string endsWith)
		{
			bool flag = expr != null && expr.EndsWith(endsWith);
			if (flag)
			{
				expr = expr.Substring(0, expr.Length - endsWith.Length);
			}
			return expr;
		}

		// Token: 0x060012D6 RID: 4822 RVA: 0x00040DA8 File Offset: 0x0003EFA8
		public static string StripStart(string expr, string startsWith)
		{
			bool flag = expr != null && expr.StartsWith(startsWith);
			if (flag)
			{
				expr = expr.Substring(startsWith.Length);
			}
			return expr;
		}

		// Token: 0x060012D7 RID: 4823 RVA: 0x00040DDC File Offset: 0x0003EFDC
		public static void ClearAndRefresh(params PXSelectBase[] queries)
		{
			EnumerableExtensions.ForEach<PXSelectBase>(queries, delegate(PXSelectBase qu)
			{
				qu.Cache.Clear();
			});
			EnumerableExtensions.ForEach<PXSelectBase>(queries, delegate(PXSelectBase qu)
			{
				qu.View.RequestRefresh();
			});
		}

		// Token: 0x060012D8 RID: 4824 RVA: 0x00040E38 File Offset: 0x0003F038
		public static IEnumerable<IEnumerable<T>> CartesianProduct<T>(this IEnumerable<IEnumerable<T>> sequences)
		{
			IEnumerable<IEnumerable<T>> seed = new IEnumerable<T>[]
			{
				Enumerable.Empty<T>()
			};
			return sequences.Aggregate(seed, (IEnumerable<IEnumerable<T>> accumulator, IEnumerable<T> sequence) => from accseq in accumulator
			from item in sequence
			select accseq.Concat(new T[]
			{
				item
			}));
		}

		// Token: 0x060012D9 RID: 4825 RVA: 0x00040E80 File Offset: 0x0003F080
		public static object Replace(object newValue, char oldChar, char newChar)
		{
			string text = newValue as string;
			bool flag = text != null;
			if (flag)
			{
				newValue = text.Replace(oldChar, newChar);
			}
			return newValue;
		}

		// Token: 0x060012DA RID: 4826 RVA: 0x00040EB0 File Offset: 0x0003F0B0
		public static object RemoveWhitespace(object newValue)
		{
			string text = newValue as string;
			bool flag = text != null;
			if (flag)
			{
				newValue = text.RemoveWhitespace();
			}
			return newValue;
		}

		// Token: 0x060012DB RID: 4827 RVA: 0x00040EDC File Offset: 0x0003F0DC
		public static string RemoveWhitespace(this string input)
		{
			return new string((from c in input.ToCharArray()
			where !char.IsWhiteSpace(c)
			select c).ToArray<char>());
		}

		// Token: 0x060012DC RID: 4828 RVA: 0x00040F24 File Offset: 0x0003F124
		public static string FromFontStyleNamesToStyleMulti(string fontStyleNames)
		{
			bool flag = !string.IsNullOrEmpty(fontStyleNames);
			string result;
			if (flag)
			{
				fontStyleNames = string.Join(", ", fontStyleNames.Split(new char[]
				{
					' '
				}));
				FontStyle fontStyle = (FontStyle)Enum.Parse(typeof(FontStyle), fontStyleNames);
				int[] values = fontStyle.ToIntArray();
				string text = string.Join<int>(",", values);
				result = text;
			}
			else
			{
				result = 0.ToString();
			}
			return result;
		}

		// Token: 0x060012DD RID: 4829 RVA: 0x00040FA0 File Offset: 0x0003F1A0
		public static FontStyle FromStyleMultiToFontStyle(string styleList)
		{
			bool flag = string.IsNullOrEmpty(styleList);
			FontStyle result;
			if (flag)
			{
				result = FontStyle.Regular;
			}
			else
			{
				IEnumerable<string> source = styleList.Split(new char[]
				{
					','
				});
				Func<string, int> selector;
				if ((selector = AsgardUtils.<>O.<9>__Parse) == null)
				{
					selector = (AsgardUtils.<>O.<9>__Parse = new Func<string, int>(int.Parse));
				}
				IEnumerable<FontStyle> source2 = from x in source.Select(selector)
				select (FontStyle)x;
				FontStyle fontStyle;
				if (!source2.Any<FontStyle>())
				{
					fontStyle = FontStyle.Regular;
				}
				else
				{
					fontStyle = source2.Aggregate((FontStyle x, FontStyle y) => x | y);
				}
				FontStyle fontStyle2 = fontStyle;
				result = fontStyle2;
			}
			return result;
		}

		// Token: 0x060012DE RID: 4830 RVA: 0x0004104C File Offset: 0x0003F24C
		public static bool HasFlag<E>(string options, int? flag) where E : struct, Enum
		{
			bool flag2 = string.IsNullOrEmpty(options) || flag == null;
			bool result;
			if (flag2)
			{
				result = false;
			}
			else
			{
				E e = AsgardUtils.FromIntListFlagsToEnum<E>(options);
				E flag3 = (E)((object)Enum.ToObject(typeof(E), flag.Value));
				result = AsgardUtils.HasFlag<E>(options, flag3);
			}
			return result;
		}

		// Token: 0x060012DF RID: 4831 RVA: 0x000410A8 File Offset: 0x0003F2A8
		public static bool HasFlag<E>(string options, E flag) where E : struct, Enum
		{
			E options2 = AsgardUtils.FromIntListFlagsToEnum<E>(options);
			return AsgardUtils.HasFlag<E>(options2, flag);
		}

		// Token: 0x060012E0 RID: 4832 RVA: 0x000410C8 File Offset: 0x0003F2C8
		public static bool HasFlag<E>(E options, E flag) where E : struct, Enum
		{
			Enum @enum = options;
			return @enum.HasFlag(flag);
		}

		// Token: 0x060012E1 RID: 4833 RVA: 0x000410F0 File Offset: 0x0003F2F0
		public static E AsEnum<E>(int? value, E defaultValue) where E : struct, Enum
		{
			bool flag = value == null;
			E result;
			if (flag)
			{
				result = defaultValue;
			}
			else
			{
				E e = (E)((object)Enum.ToObject(typeof(E), value.Value));
				result = e;
			}
			return result;
		}

		// Token: 0x060012E2 RID: 4834 RVA: 0x00041134 File Offset: 0x0003F334
		public static E FromIntListFlagsToEnum<E>(string multiValueIntList) where E : Enum
		{
			Type typeFromHandle = typeof(E);
			ValueTuple<Type, string> key = new ValueTuple<Type, string>(typeFromHandle, multiValueIntList);
			Func<ValueTuple<Type, string>, E> valueFactory = (ValueTuple<Type, string> k) => (E)((object)AsgardUtils.FromIntListFlagsToEnumInternal(k));
			return CacheHelper2<ValueTuple<Type, string>, E>.GetOrAdd(key, valueFactory);
		}

		// Token: 0x060012E3 RID: 4835 RVA: 0x00041188 File Offset: 0x0003F388
		public static object FromIntListFlagsToEnumInternal([TupleElementNames(new string[]
		{
			"enumType",
			"multiValueIntList"
		})] ValueTuple<Type, string> tuple)
		{
			bool flag = string.IsNullOrEmpty(tuple.Item2);
			object result;
			if (flag)
			{
				result = Enum.ToObject(tuple.Item1, 0);
			}
			else
			{
				IEnumerable<string> source = tuple.Item2.Split(new char[]
				{
					','
				});
				Func<string, int> selector;
				if ((selector = AsgardUtils.<>O.<9>__Parse) == null)
				{
					selector = (AsgardUtils.<>O.<9>__Parse = new Func<string, int>(int.Parse));
				}
				int[] source2 = source.Select(selector).Distinct<int>().ToArray<int>();
				int value = source2.Aggregate((int x, int y) => x | y);
				object obj = Enum.ToObject(tuple.Item1, value);
				result = obj;
			}
			return result;
		}

		// Token: 0x060012E4 RID: 4836 RVA: 0x00041230 File Offset: 0x0003F430
		public static int[] ToIntArray(this Enum flagEnum)
		{
			return (from i in flagEnum.ToString().Split(new string[]
			{
				", "
			}, StringSplitOptions.None)
			select (int)Enum.Parse(flagEnum.GetType(), i)).ToArray<int>();
		}

		// Token: 0x060012E5 RID: 4837 RVA: 0x00041284 File Offset: 0x0003F484
		public static bool HasIllegalCharacters(IRenderableConfig row)
		{
			bool flag = row == null;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				foreach (char value in AsgardUtils.INVALID)
				{
					bool flag2 = row.Name.Contains(value);
					if (flag2)
					{
						return true;
					}
				}
				result = false;
			}
			return result;
		}

		// Token: 0x060012E6 RID: 4838 RVA: 0x000412E0 File Offset: 0x0003F4E0
		public static string RemoveIllegalFileNameCharacters(string newValue)
		{
			bool flag = string.IsNullOrEmpty(newValue);
			string result;
			if (flag)
			{
				result = newValue;
			}
			else
			{
				string invalid = AsgardUtils.INVALID;
				for (int i = 0; i < invalid.Length; i++)
				{
					newValue = newValue.Replace(invalid[i].ToString(), "");
				}
				result = newValue;
			}
			return result;
		}

		// Token: 0x060012E7 RID: 4839 RVA: 0x0004133C File Offset: 0x0003F53C
		public static string ReplaceCharactersBy(char replaceBy, string newValue, params char[] chars)
		{
			bool flag = string.IsNullOrEmpty(newValue);
			string result;
			if (flag)
			{
				result = newValue;
			}
			else
			{
				foreach (char oldChar in chars)
				{
					newValue = newValue.Replace(oldChar, replaceBy);
				}
				result = newValue;
			}
			return result;
		}

		// Token: 0x060012E8 RID: 4840 RVA: 0x00041384 File Offset: 0x0003F584
		public static IPXResultset GetAsResultset(object basedOnResult)
		{
			Func<object> func = basedOnResult as Func<object>;
			bool flag = func != null;
			if (flag)
			{
				basedOnResult = func();
			}
			IPXResultset ipxresultset = basedOnResult as IPXResultset;
			bool flag2 = ipxresultset != null;
			IPXResultset result;
			if (flag2)
			{
				result = ipxresultset;
			}
			else
			{
				GenericResult[] array = basedOnResult as GenericResult[];
				bool flag3 = array != null;
				if (flag3)
				{
					PXResultset<GenericResult> pxresultset = new PXResultset<GenericResult>();
					foreach (GenericResult genericResult in array)
					{
						pxresultset.Add(new PXResult<GenericResult>(genericResult));
					}
					result = pxresultset;
				}
				else
				{
					IList list = basedOnResult as IList;
					bool flag4 = list != null;
					if (!flag4)
					{
						throw new PXException("Unable to handle the Result: {0}", new object[]
						{
							(basedOnResult == null) ? "null" : basedOnResult.GetType()
						});
					}
					Type[] typeArguments = AsgardUtils.GetItemTypes(list).ToArray<Type>();
					Type type = typeof(PXResultset).MakeGenericType(typeArguments);
					IList list2 = (IList)Activator.CreateInstance(type);
					Type type2 = typeof(PXResult).MakeGenericType(typeArguments);
					ConstructorInvoker constructorInvoker = Reflect.Constructor(type2, Array.Empty<Type>());
					foreach (object obj in list)
					{
						object value = constructorInvoker(new object[]
						{
							obj
						});
						list2.Add(value);
					}
					result = (IPXResultset)list2;
				}
			}
			return result;
		}

		// Token: 0x060012E9 RID: 4841 RVA: 0x00041520 File Offset: 0x0003F720
		public static IPXResultset GetResultsetPage(IPXResultset rs, int pageNbr, int pageSize)
		{
			bool flag = rs == null;
			IPXResultset result;
			if (flag)
			{
				result = null;
			}
			else
			{
				IPXResultset newResultset = AsgardUtils.GetNewResultset(rs);
				IList list = (IList)newResultset.GetCollection();
				List<object> source = ((IList)rs.GetCollection()).Cast<object>().ToList<object>();
				int count = (pageNbr - 1) * pageSize;
				List<object> list2 = source.Skip(count).Take(pageSize).ToList<object>();
				foreach (object value in list2)
				{
					list.Add(value);
				}
				result = newResultset;
			}
			return result;
		}

		// Token: 0x060012EA RID: 4842 RVA: 0x000415D8 File Offset: 0x0003F7D8
		public static IPXResultset GetNewResultset(IPXResultset rs)
		{
			bool flag = rs == null;
			IPXResultset result;
			if (flag)
			{
				result = null;
			}
			else
			{
				Type type = rs.GetType();
				IPXResultset ipxresultset = (IPXResultset)Activator.CreateInstance(type);
				result = ipxresultset;
			}
			return result;
		}

		// Token: 0x060012EB RID: 4843 RVA: 0x0004160C File Offset: 0x0003F80C
		public static IBqlTable[] GetResults(this PXResult pxr)
		{
			Type[] tables = pxr.Tables;
			return (from it in tables
			select PXResult.Unwrap(pxr, it)).ToArray<IBqlTable>();
		}

		// Token: 0x060012EC RID: 4844 RVA: 0x00041650 File Offset: 0x0003F850
		public static void SetImportFields(string importingViewName, IDictionary values, string viewName, object newValue, params Type[] fields)
		{
			bool flag = importingViewName != viewName;
			if (!flag)
			{
				foreach (Type type in fields)
				{
					string name = type.Name;
					bool flag2 = values.Contains(name);
					if (flag2)
					{
						values[name] = newValue;
					}
					else
					{
						bool flag3 = newValue != null;
						if (flag3)
						{
							values.Add(name, newValue);
						}
					}
				}
			}
		}

		// Token: 0x060012ED RID: 4845 RVA: 0x000416C0 File Offset: 0x0003F8C0
		public static string BytesToString(byte[] bytes)
		{
			bool flag = bytes == null || bytes.Length == 0;
			string result;
			if (flag)
			{
				result = string.Empty;
			}
			else
			{
				string text = BitConverter.ToString(bytes).Replace("-", "");
				result = text;
			}
			return result;
		}

		// Token: 0x060012EE RID: 4846 RVA: 0x00041704 File Offset: 0x0003F904
		public static IPXResultset CreateResultset<T0>(int size) where T0 : class, IBqlTable, new()
		{
			PXResultset<T0> pxr = new PXResultset<T0>();
			(from index in Enumerable.Range(0, size)
			select AsgardUtils.AddTo<T0>(index, pxr)).ToArray<object>();
			return pxr;
		}

		// Token: 0x060012EF RID: 4847 RVA: 0x0004174C File Offset: 0x0003F94C
		private static object AddTo<T0>(int index, PXResultset<T0> pxr) where T0 : class, IBqlTable, new()
		{
			T0 t = Activator.CreateInstance<T0>();
			pxr.Add(new PXResult<T0>(t));
			return null;
		}

		// Token: 0x060012F0 RID: 4848 RVA: 0x00041774 File Offset: 0x0003F974
		public static DateTime StartOfDay(this DateTime theDate)
		{
			return theDate.Date;
		}

		// Token: 0x060012F1 RID: 4849 RVA: 0x00041790 File Offset: 0x0003F990
		public static DateTime EndOfDay(this DateTime theDate)
		{
			return theDate.Date.AddDays(1.0).AddTicks(-1L);
		}

		// Token: 0x060012F2 RID: 4850 RVA: 0x000417C4 File Offset: 0x0003F9C4
		public static IEnumerable<E> ExplodeFlags<E>(E flags) where E : Enum
		{
			return (from E e in Enum.GetValues(typeof(E))
			where flags.HasFlag(e)
			select e).ToList<E>();
		}

		// Token: 0x060012F3 RID: 4851 RVA: 0x00041810 File Offset: 0x0003FA10
		public static string FormFieldKey(string viewName, string fieldName)
		{
			return viewName + "_" + fieldName;
		}

		// Token: 0x060012F4 RID: 4852 RVA: 0x00041830 File Offset: 0x0003FA30
		public static IDictionary GetPrimaryKey(PXGraph graph, Dictionary<KeyWithAlias, object> newRow)
		{
			string primaryViewName = graph.PrimaryView;
			PXView pxview = graph.Views[primaryViewName];
			PXCache cache = pxview.Cache;
			Dictionary<string, object> dictionary = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
			bool flag = newRow != null;
			if (flag)
			{
				var enumerable = from k in cache.Keys
				select new
				{
					KeyAlias = new KeyWithAlias(AsgardUtils.FormFieldKey(primaryViewName, k)),
					KeyName = k
				};
				foreach (var <>f__AnonymousType in enumerable)
				{
					string keyName = <>f__AnonymousType.KeyName;
					object obj;
					bool flag2 = newRow.TryGetValue(<>f__AnonymousType.KeyAlias, out obj);
					object obj2;
					if (flag2)
					{
						ValueWithInternal valueWithInternal = obj as ValueWithInternal;
						bool flag3 = valueWithInternal != null;
						if (flag3)
						{
							obj2 = valueWithInternal.GetValue();
						}
						else
						{
							obj2 = null;
						}
						if (obj2 == null)
						{
							obj2 = obj;
						}
					}
					else
					{
						obj2 = null;
					}
					dictionary.Add(keyName, obj2);
				}
			}
			return dictionary;
		}

		// Token: 0x060012F5 RID: 4853 RVA: 0x00041944 File Offset: 0x0003FB44
		public static string ToString(IDictionary<string, object> source)
		{
			if (source == null)
			{
				source = ImmutableDictionary<string, object>.Empty;
			}
			return JsonConvert.SerializeObject(source, 1);
		}

		// Token: 0x060012F6 RID: 4854 RVA: 0x00041969 File Offset: 0x0003FB69
		public static void EnableOrHide(PXAction action, bool isEnabled)
		{
			action.SetEnabled(isEnabled);
			action.SetVisible(isEnabled);
		}

		// Token: 0x060012F7 RID: 4855 RVA: 0x0004197C File Offset: 0x0003FB7C
		public static bool IsAttribute(string fieldName)
		{
			return fieldName != null && (fieldName.EndsWith("_Attributes") || fieldName.StartsWith("Attribute"));
		}

		// Token: 0x060012F8 RID: 4856 RVA: 0x000419B4 File Offset: 0x0003FBB4
		public static string GetAttributeID(string fieldName)
		{
			string result = null;
			bool flag = AsgardUtils.IsAttribute(fieldName);
			if (flag)
			{
				bool flag2 = fieldName.EndsWith("_Attributes");
				if (flag2)
				{
					result = fieldName.Substring(0, fieldName.Length - "_Attributes".Length);
				}
				else
				{
					bool flag3 = fieldName.StartsWith("Attribute");
					if (flag3)
					{
						result = fieldName.Substring("Attribute".Length);
					}
				}
			}
			return result;
		}

		// Token: 0x060012F9 RID: 4857 RVA: 0x00041A28 File Offset: 0x0003FC28
		public static string GetScreenID()
		{
			string screenID = PXContext.GetScreenID();
			return (screenID != null) ? screenID.Replace(".", "") : null;
		}

		// Token: 0x060012FA RID: 4858 RVA: 0x00041A58 File Offset: 0x0003FC58
		public static PXView AddOrGetView(PXGraph rowGraph, CSAttribute attr)
		{
			string objectName = attr.ObjectName;
			string fieldName = attr.FieldName;
			bool flag = string.IsNullOrEmpty(fieldName) || string.IsNullOrEmpty(fieldName);
			PXView result;
			if (flag)
			{
				result = null;
			}
			else
			{
				string text = string.Format("_Attr{0}_{1}_", attr.AttributeID, fieldName);
				bool flag2 = !rowGraph.Views.ContainsKey(text);
				PXView pxview;
				if (flag2)
				{
					BqlCommand bqlCommand = AsgardUtils.BuildSelect(rowGraph, objectName, fieldName);
					bool flag3 = bqlCommand == null;
					if (flag3)
					{
						return null;
					}
					pxview = new PXView(rowGraph, true, bqlCommand);
					rowGraph.Views[text] = pxview;
				}
				else
				{
					pxview = rowGraph.Views[text];
				}
				result = pxview;
			}
			return result;
		}

		// Token: 0x060012FB RID: 4859 RVA: 0x00041B08 File Offset: 0x0003FD08
		private static BqlCommand BuildSelect(PXGraph rowGraph, string objectName, string fieldName)
		{
			try
			{
				Type type = AsgardUtils.GetType(objectName);
				ValueTuple<bool, PXCache> valueTuple = AsgardUtils.AddOrGetCache(rowGraph, type);
				bool item = valueTuple.Item1;
				PXCache item2 = valueTuple.Item2;
				Type type2 = item2.BqlFields.FirstOrDefault((Type f) => string.Equals(fieldName, f.Name, StringComparison.OrdinalIgnoreCase));
				Type type3 = item2.BqlKeys.FirstOrDefault<Type>();
				BqlCommand bqlCommand = BqlCommand.CreateInstance(new Type[]
				{
					typeof(Search),
					type2
				});
				return bqlCommand.WhereAnd(BqlCommand.Compose(new Type[]
				{
					typeof(Where),
					type3,
					typeof(Equal),
					typeof(Required),
					type3
				}));
			}
			catch (Exception ex)
			{
				PXTrace.WriteError("Cannot create a BQL Selecto for Object Name '{0}' and Field '{1}'", new object[]
				{
					ex
				});
			}
			return null;
		}

		// Token: 0x060012FC RID: 4860 RVA: 0x00041C04 File Offset: 0x0003FE04
		public static Type MakeGenericType(params Type[] types)
		{
			int num = 0;
			return AsgardUtils.MakeGenericType(types, ref num);
		}

		// Token: 0x060012FD RID: 4861 RVA: 0x00041C20 File Offset: 0x0003FE20
		public static Type MakeGenericType(Type[] types, ref int index)
		{
			bool flag = types == null;
			if (flag)
			{
				throw new ArgumentNullException("types");
			}
			bool flag2 = types.Length == 0;
			if (flag2)
			{
				throw new ArgumentException("The types list is empty.");
			}
			bool flag3 = index >= types.Length;
			if (flag3)
			{
				throw new ArgumentOutOfRangeException("types", "The types list is not correct.");
			}
			Type type = types[index];
			index++;
			bool flag4 = !type.IsGenericTypeDefinition;
			Type result;
			if (flag4)
			{
				result = type;
			}
			else
			{
				Type[] array = new Type[type.GetGenericArguments().Length];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = AsgardUtils.MakeGenericType(types, ref index);
				}
				result = type.MakeGenericType(array);
			}
			return result;
		}

		// Token: 0x060012FE RID: 4862 RVA: 0x00041CD8 File Offset: 0x0003FED8
		public static PXAction AddCopyPasteAction(string viewName, string suffix, Type table, PXGraph graph, PXButtonDelegate handler)
		{
			string text = viewName + suffix;
			PXAction pxaction = (PXAction)Activator.CreateInstance(AsgardUtils.MakeGenericType(new Type[]
			{
				typeof(PXNamedAction),
				table
			}), new object[]
			{
				graph,
				text,
				handler
			});
			graph.Actions[text] = pxaction;
			Type type = graph.GetType();
			bool flag = AsgardUtils.IsInstanceOfGenericType(graph, typeof(PXGraph));
			if (flag)
			{
				FieldInfo field = type.GetField("CopyPaste", BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy);
				PXAction pxaction2 = ((field != null) ? field.GetValue(graph) : null) as PXAction;
				if (pxaction2 != null)
				{
					pxaction2.AddMenuAction(pxaction);
				}
			}
			return pxaction;
		}

		// Token: 0x060012FF RID: 4863 RVA: 0x00041D8E File Offset: 0x0003FF8E
		public static void DeleteFile(PXGraph graph, FileInfo fileInfo)
		{
			UploadFileMaintenance.DeleteFile(fileInfo.UID);
			AsgardUtils.RemoveTempNoteDoc(graph, fileInfo.UID);
		}

		// Token: 0x06001300 RID: 4864 RVA: 0x00041DAC File Offset: 0x0003FFAC
		private static void RemoveTempNoteDoc(PXGraph graph, Guid? fileID)
		{
			foreach (object obj in graph.Caches[typeof(NoteDoc)].Inserted)
			{
				NoteDoc noteDoc = (NoteDoc)obj;
				Guid? fileID2 = noteDoc.FileID;
				Guid? guid = fileID;
				bool flag = fileID2 != null == (guid != null);
				bool flag2 = flag && (fileID2 == null || fileID2.GetValueOrDefault() == guid.GetValueOrDefault());
				bool flag3 = !flag2;
				if (!flag3)
				{
					graph.Caches[typeof(NoteDoc)].Delete(noteDoc);
				}
			}
		}

		// Token: 0x04000861 RID: 2145
		private static readonly Regex ANY_CR = new Regex("\\r?\\n");

		// Token: 0x04000862 RID: 2146
		private static readonly Regex DOUBLE_CR = new Regex("\\r?\\n\\r?\\n");

		// Token: 0x04000863 RID: 2147
		public const string UNIX_CR = "\r";

		// Token: 0x04000864 RID: 2148
		public const string WIN_CR = "\r\n";

		// Token: 0x04000865 RID: 2149
		private static readonly Type[] SIMPLE_TYPES = new Type[]
		{
			typeof(Enum),
			typeof(string),
			typeof(decimal),
			typeof(DateTime),
			typeof(DateTimeOffset),
			typeof(TimeSpan),
			typeof(Guid)
		};

		// Token: 0x04000866 RID: 2150
		private static readonly IDictionary<string, IDictionary<string, string>> IMPLS_BY_INTERFACE = new Dictionary<string, IDictionary<string, string>>();

		// Token: 0x04000867 RID: 2151
		private static readonly Regex CAMELS2 = new Regex("(?<=[a-z])(?=[A-Z])|(?<=[A-Z])(?=[A-Z][a-z])|(?<=[0-9])(?=[a-z])|(?<=[0-9])(?=[A-Z])|(?<=[a-z])(?=[0-9])|(?<=[A-Z])(?=[0-9])");

		// Token: 0x04000868 RID: 2152
		public static readonly int[] MissingHttpStatusValues = (int[])Enum.GetValues(typeof(AsgardUtils.MissingHttpStatus));

		// Token: 0x04000869 RID: 2153
		private static readonly Dictionary<Type, Delegate> _CACHED_IL = new Dictionary<Type, Delegate>();

		// Token: 0x0400086A RID: 2154
		private static readonly string INVALID = new string(Path.GetInvalidFileNameChars().Concat(Path.GetInvalidPathChars()).Distinct<char>().ToArray<char>());

		// Token: 0x020008E4 RID: 2276
		public class Me : BqlType<IBqlGuid, Guid>.Constant<AsgardUtils.Me>
		{
			// Token: 0x0600275B RID: 10075 RVA: 0x0007AAB4 File Offset: 0x00078CB4
			public Me() : base(PXAccess.GetUserID())
			{
			}
		}

		// Token: 0x020008E5 RID: 2277
		public static class StringBuilderCache
		{
			// Token: 0x0600275C RID: 10076 RVA: 0x0007AAC4 File Offset: 0x00078CC4
			public static StringBuilder Acquire(int capacity = 16)
			{
				bool flag = capacity <= 360;
				if (flag)
				{
					StringBuilder cachedInstance = AsgardUtils.StringBuilderCache.CachedInstance;
					bool flag2 = cachedInstance != null;
					if (flag2)
					{
						bool flag3 = capacity <= cachedInstance.Capacity;
						if (flag3)
						{
							AsgardUtils.StringBuilderCache.CachedInstance = null;
							cachedInstance.Clear();
							return cachedInstance;
						}
					}
				}
				return new StringBuilder(capacity);
			}

			// Token: 0x0600275D RID: 10077 RVA: 0x0007AB24 File Offset: 0x00078D24
			public static void Release(StringBuilder sb)
			{
				bool flag = sb.Capacity <= 360;
				if (flag)
				{
					AsgardUtils.StringBuilderCache.CachedInstance = sb;
				}
			}

			// Token: 0x0600275E RID: 10078 RVA: 0x0007AB50 File Offset: 0x00078D50
			public static string GetStringAndRelease(StringBuilder sb)
			{
				string result = sb.ToString();
				AsgardUtils.StringBuilderCache.Release(sb);
				return result;
			}

			// Token: 0x04001190 RID: 4496
			private const int MAX_BUILDER_SIZE = 360;

			// Token: 0x04001191 RID: 4497
			private const int DefaultCapacity = 16;

			// Token: 0x04001192 RID: 4498
			[ThreadStatic]
			private static StringBuilder CachedInstance;
		}

		// Token: 0x020008E6 RID: 2278
		public enum MissingHttpStatus
		{
			// Token: 0x04001194 RID: 4500
			Processing = 102,
			// Token: 0x04001195 RID: 4501
			EarlyHints,
			// Token: 0x04001196 RID: 4502
			MultiStatus = 207,
			// Token: 0x04001197 RID: 4503
			AlreadyReported,
			// Token: 0x04001198 RID: 4504
			IMUsed = 226,
			// Token: 0x04001199 RID: 4505
			PermanentRedirect = 308,
			// Token: 0x0400119A RID: 4506
			MisdirectedRequest = 421,
			// Token: 0x0400119B RID: 4507
			UnprocessableEntity,
			// Token: 0x0400119C RID: 4508
			Locked,
			// Token: 0x0400119D RID: 4509
			FailedDependency,
			// Token: 0x0400119E RID: 4510
			PreconditionRequired = 428,
			// Token: 0x0400119F RID: 4511
			TooManyRequests,
			// Token: 0x040011A0 RID: 4512
			RequestHeaderFieldsTooLarge = 431,
			// Token: 0x040011A1 RID: 4513
			UnavailableForLegalReasons = 451,
			// Token: 0x040011A2 RID: 4514
			VariantAlsoNegotiates = 506,
			// Token: 0x040011A3 RID: 4515
			InsufficientStorage,
			// Token: 0x040011A4 RID: 4516
			LoopDetected,
			// Token: 0x040011A5 RID: 4517
			NotExtended = 510,
			// Token: 0x040011A6 RID: 4518
			NetworkAuthenticationRequired
		}

		// Token: 0x020008E7 RID: 2279
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x040011A7 RID: 4519
			public static Func<FieldInfo, bool> <0>__IsPXFilter;

			// Token: 0x040011A8 RID: 4520
			public static Func<FieldInfo, bool> <1>__IsFilteredResult;

			// Token: 0x040011A9 RID: 4521
			public static Func<FieldInfo, bool> <2>__IsProcessingView;

			// Token: 0x040011AA RID: 4522
			public static Func<Type, IEnumerable<FieldInfo>> <3>__GetFieldsInternal;

			// Token: 0x040011AB RID: 4523
			public static Func<Type, ISelectable> <4>__GetInstance;

			// Token: 0x040011AC RID: 4524
			public static Func<ParameterInfo, bool> <5>__Keep;

			// Token: 0x040011AD RID: 4525
			public static Func<ParameterInfo, string> <6>__ParamToString;

			// Token: 0x040011AE RID: 4526
			public static Func<char, bool> <7>__IsLetter;

			// Token: 0x040011AF RID: 4527
			public static Func<char, bool> <8>__IsDigit;

			// Token: 0x040011B0 RID: 4528
			public static Func<string, int> <9>__Parse;
		}

		// Token: 0x02000925 RID: 2341
		[CompilerGenerated]
		private static class <NonNulls>O__74_0<T>
		{
			// Token: 0x0400124E RID: 4686
			public static Func<T, bool> <0>__NotNull;
		}

		// Token: 0x02000926 RID: 2342
		[CompilerGenerated]
		private static class <NonNulls>O__75_0<T>
		{
			// Token: 0x0400124F RID: 4687
			public static Func<T, bool> <0>__NotNull;
		}
	}
}
