using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using PX.Data;
using PX.Objects.Common.Abstractions;
using Scriban;
using Scriban.Parsing;
using Scriban.Runtime;
using Scriban.Syntax;

namespace AA.Objects.AL
{
	// Token: 0x020001EF RID: 495
	public static class ScribanUtils
	{
		// Token: 0x170006DB RID: 1755
		// (get) Token: 0x06001542 RID: 5442 RVA: 0x0004C4F8 File Offset: 0x0004A6F8
		public static MemberRenamerDelegate PascalCase
		{
			get
			{
				return (MemberInfo m) => m.Name;
			}
		}

		// Token: 0x06001543 RID: 5443 RVA: 0x0004C51C File Offset: 0x0004A71C
		public static void CheckTemplateErrors(string errorType, string name, Template template)
		{
			bool hasErrors = template.HasErrors;
			if (hasErrors)
			{
				ScribanUtils.ShowErrorLines(template);
				throw new PXException("Template for '{0}' '{1}' has errors: {2}", new object[]
				{
					errorType,
					name,
					template.Messages
				});
			}
		}

		// Token: 0x06001544 RID: 5444 RVA: 0x0004C560 File Offset: 0x0004A760
		private static void ShowErrorLines(Template template)
		{
			template.Page.ToString();
			LogMessageBag messages = template.Messages;
			foreach (LogMessage logMessage in messages)
			{
				TextPosition start = logMessage.Span.Start;
			}
		}

		// Token: 0x06001545 RID: 5445 RVA: 0x0004C5C8 File Offset: 0x0004A7C8
		public static bool SkipScribanTypes(ParameterInfo pi)
		{
			return pi.ParameterType != typeof(TemplateContext) && pi.ParameterType != typeof(SourceSpan);
		}

		// Token: 0x06001546 RID: 5446 RVA: 0x0004C60C File Offset: 0x0004A80C
		public static string GetArgName(ParameterInfo pi)
		{
			return pi.Name;
		}

		// Token: 0x06001547 RID: 5447 RVA: 0x0004C624 File Offset: 0x0004A824
		public static string GetDefaultValue(ParameterInfo pi)
		{
			bool flag = pi.DefaultValue == DBNull.Value || pi.DefaultValue == null;
			string result;
			if (flag)
			{
				result = "null";
			}
			else
			{
				object defaultValue = pi.DefaultValue;
				bool flag2 = defaultValue is string;
				if (flag2)
				{
					result = string.Format("'{0}'", defaultValue);
				}
				else
				{
					bool flag3 = defaultValue is bool;
					if (flag3)
					{
						result = defaultValue.ToString().ToLower();
					}
					else
					{
						result = defaultValue.ToString();
					}
				}
			}
			return result;
		}

		// Token: 0x06001548 RID: 5448 RVA: 0x0004C6A4 File Offset: 0x0004A8A4
		public static TemplateContext CreateContext(PXGraph rowGraph, object row, object oldRow, bool devMode, params object[] contextValues)
		{
			LabelTemplateContext labelTemplateContext = new LabelTemplateContext(rowGraph)
			{
				DevMode = devMode
			};
			IScriptObject globalContainer = ScribanUtils.GetGlobalContainer(rowGraph, row, oldRow, contextValues);
			labelTemplateContext.PushGlobal(globalContainer);
			ScribanUtils.LoadLibraries(globalContainer);
			return labelTemplateContext;
		}

		// Token: 0x06001549 RID: 5449 RVA: 0x0004C6E0 File Offset: 0x0004A8E0
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
			ScribanUtils.LoadLibraries(scriptObject);
			return baseTemplateContext;
		}

		// Token: 0x0600154A RID: 5450 RVA: 0x0004C73C File Offset: 0x0004A93C
		public static void LoadLibraries(IScriptObject container)
		{
			bool loadExternalLibs = ALSetupSlot.LoadExternalLibs;
			IEnumerable<Type> enumerable;
			if (loadExternalLibs)
			{
				enumerable = AsgardUtils.GetImplementations(typeof(ISubstitution), false);
			}
			else
			{
				enumerable = AsgardUtils.GetImplementationsByAssembly<ISubstitution>(Assembly.GetCallingAssembly(), false);
			}
			foreach (Type type in enumerable)
			{
				ValueTuple<string, ScriptMemberImportFlags> libInfo = ScribanUtils.GetLibInfo(type);
				string item = libInfo.Item1;
				ScriptMemberImportFlags item2 = libInfo.Item2;
				bool flag = string.IsNullOrEmpty(item);
				if (!flag)
				{
					IEnumerable<ScriptMemberImportFlags> enumerable2 = AsgardUtils.ExplodeFlags<ScriptMemberImportFlags>(item2);
					foreach (ScriptMemberImportFlags scriptMemberImportFlags in enumerable2)
					{
						MemberRenamerDelegate renamer = (scriptMemberImportFlags == ScriptMemberImportFlags.Method) ? ScribanUtils.PascalCase : null;
						ScribanUtils.Import(type, container, item, scriptMemberImportFlags, null, renamer);
					}
				}
			}
		}

		// Token: 0x0600154B RID: 5451 RVA: 0x0004C83C File Offset: 0x0004AA3C
		public static void Import<FuncLib>(IScriptObject container, ScriptMemberImportFlags flags, MemberFilterDelegate filter = null, MemberRenamerDelegate renamer = null)
		{
			string name = typeof(FuncLib).Name;
			ScribanUtils.Import<FuncLib>(container, name, flags, filter, renamer);
		}

		// Token: 0x0600154C RID: 5452 RVA: 0x0004C865 File Offset: 0x0004AA65
		public static void Import<FuncLib>(IScriptObject container, string name, ScriptMemberImportFlags flags, MemberFilterDelegate filter = null, MemberRenamerDelegate renamer = null)
		{
			ScribanUtils.Import(typeof(FuncLib), container, name, flags, filter, renamer);
		}

		// Token: 0x0600154D RID: 5453 RVA: 0x0004C880 File Offset: 0x0004AA80
		public static void Import(Type libType, IScriptObject container, string name, ScriptMemberImportFlags flags, MemberFilterDelegate filter = null, MemberRenamerDelegate renamer = null)
		{
			ScriptObject scriptObject = new ScriptObject();
			container.SetValue(name, scriptObject, true);
			scriptObject.Import(libType, flags, filter, renamer);
		}

		// Token: 0x0600154E RID: 5454 RVA: 0x0004C8AC File Offset: 0x0004AAAC
		private static IScriptObject GetGlobalContainer(PXGraph rowGraph, object row, object oldRow, params object[] contextValues)
		{
			ScriptObject scriptObject = new ScriptObject
			{
				{
					"AL_RowGraph",
					rowGraph
				},
				{
					"AL_Row",
					row
				},
				{
					rowGraph.PrimaryView,
					row
				},
				{
					"AL_OldRow",
					oldRow
				},
				{
					"Old" + rowGraph.PrimaryView,
					oldRow
				},
				{
					"AL_LineWrap",
					"\\&"
				}
			};
			foreach (object value in contextValues)
			{
				scriptObject.SetValue(value);
			}
			return scriptObject;
		}

		// Token: 0x0600154F RID: 5455 RVA: 0x0004C944 File Offset: 0x0004AB44
		public static void SetGlobalValues(this TemplateContext templateContext, params object[] contextValues)
		{
			foreach (object value in contextValues)
			{
				templateContext.SetValue(value);
			}
		}

		// Token: 0x06001550 RID: 5456 RVA: 0x0004C974 File Offset: 0x0004AB74
		public static void SetGlobalValue(this TemplateContext templateContext, PXGraph docGraph, object doc)
		{
			ViewDef viewDefinition = ViewUtils.GetViewDefinition(docGraph, docGraph.PrimaryView);
			ViewResult viewResult = new ViewResult(viewDefinition, docGraph, doc);
			ScribanUtils.SetGlobalValue(templateContext, viewResult);
		}

		// Token: 0x06001551 RID: 5457 RVA: 0x0004C9A0 File Offset: 0x0004ABA0
		public static void SetGlobalValue(TemplateContext context, IViewResult viewResult)
		{
			ViewDef viewDef = viewResult.ViewDef;
			object result = viewResult.Result;
			string viewName = ViewUtils.GetViewName(viewDef, null, true);
			context.SetValue(viewName, result);
			ScribanUtils.FillAttributes(context, viewResult);
		}

		// Token: 0x06001552 RID: 5458 RVA: 0x0004C9D8 File Offset: 0x0004ABD8
		public static void FillAttributes(TemplateContext context, IViewResult viewResult)
		{
			ContextVariables.GetRowGraph(context);
			object row = ScribanUtils.GetRow(context, viewResult);
			bool flag = row == null;
			if (flag)
			{
			}
		}

		// Token: 0x06001553 RID: 5459 RVA: 0x0004CA04 File Offset: 0x0004AC04
		private static object GetRow(TemplateContext context, IViewResult viewResult)
		{
			object obj = ContextVariables.GetDetailRow(context);
			bool flag = obj != null;
			object result;
			if (flag)
			{
				result = obj;
			}
			else
			{
				obj = viewResult.Result.FirstResultOrDefault();
				result = obj;
			}
			return result;
		}

		// Token: 0x06001554 RID: 5460 RVA: 0x0004CA38 File Offset: 0x0004AC38
		public static bool HasValue(this TemplateContext context, string name)
		{
			return context.GetValue(name, true, null) != null;
		}

		// Token: 0x06001555 RID: 5461 RVA: 0x0004CA58 File Offset: 0x0004AC58
		public static bool HasValue<T>(this TemplateContext context)
		{
			return context.GetValue(typeof(T).Name, true, default(T)) != null;
		}

		// Token: 0x06001556 RID: 5462 RVA: 0x0004CA94 File Offset: 0x0004AC94
		public static T GetValue<T>(this TemplateContext context, bool allowNull = false)
		{
			return context.GetValue(typeof(T).Name, allowNull, default(T));
		}

		// Token: 0x06001557 RID: 5463 RVA: 0x0004CAC8 File Offset: 0x0004ACC8
		public static T GetValue<T>(this TemplateContext context, string name, bool allowNull = false, T defaultValue = default(T))
		{
			object value = context.GetValue(new ScriptVariableGlobal(name));
			bool flag = value == null;
			T result;
			if (flag)
			{
				bool flag2 = !allowNull;
				if (flag2)
				{
					throw new PXException("No value named '{0}' found in TemplateContext", new object[]
					{
						name
					});
				}
				result = defaultValue;
			}
			else
			{
				T t;
				bool flag3 = value.TryCast(out t);
				bool flag4 = !flag3;
				if (flag4)
				{
					throw new PXException("A value named '{0}' has a type {1} which is not comaptible with {2}", new object[]
					{
						name,
						value.GetType().Name,
						typeof(T).Name
					});
				}
				result = t;
			}
			return result;
		}

		// Token: 0x06001558 RID: 5464 RVA: 0x0004CB64 File Offset: 0x0004AD64
		public static void SetValue(this IScriptObject container, object value)
		{
			bool flag = value != null;
			if (flag)
			{
				IContextNamed contextNamed = value as IContextNamed;
				bool flag2 = contextNamed != null;
				string member;
				if (flag2)
				{
					member = contextNamed.ContextName;
				}
				else
				{
					member = value.GetType().Name;
				}
				container.SetValue(member, value, false);
			}
		}

		// Token: 0x06001559 RID: 5465 RVA: 0x0004CBB0 File Offset: 0x0004ADB0
		public static void SetValue(this TemplateContext context, string name, object value)
		{
			string arg = (value == null) ? "NULL" : value.GetType().FullName;
			object arg2 = ScribanUtils.ToString(context, value);
			PXTrace.WriteInformation(string.Format("Saving '{0}' : {1} (Type is {2})", name, arg2, arg));
			try
			{
				context.SetValue(new ScriptVariableGlobal(name), value, false);
			}
			catch (Exception ex)
			{
				PXTrace.WriteError(ex);
				throw;
			}
		}

		// Token: 0x0600155A RID: 5466 RVA: 0x0004CC20 File Offset: 0x0004AE20
		public static void SetValue(this TemplateContext context, object value)
		{
			bool flag = value != null;
			if (flag)
			{
				IContextNamed contextNamed = value as IContextNamed;
				bool flag2 = contextNamed != null;
				string name;
				if (flag2)
				{
					name = contextNamed.ContextName;
				}
				else
				{
					name = value.GetType().Name;
				}
				context.SetValue(new ScriptVariableGlobal(name), value, false);
			}
		}

		// Token: 0x0600155B RID: 5467 RVA: 0x0004CC70 File Offset: 0x0004AE70
		private static object ToString(TemplateContext context, object value)
		{
			bool flag = value == null;
			object result;
			if (flag)
			{
				result = "NULL";
			}
			else
			{
				Type type = value.GetType();
				bool flag2 = type.IsPrimitive || type.IsValueType;
				if (flag2)
				{
					result = value.ToString();
				}
				else
				{
					Type type2 = AsgardUtils.GetItemType(value);
					bool flag3 = false;
					IPXResultset ipxresultset = value as IPXResultset;
					bool flag4 = ipxresultset != null;
					if (flag4)
					{
						flag3 = true;
						value = ipxresultset.GetCollection();
					}
					bool flag5 = type2.IsGenericType && type2.IsCompatibleWith(typeof(ICollection)) && type2.GetGenericArguments().Count<Type>() == 1 && type2.GetGenericArguments()[0].IsCompatibleWith(typeof(IBqlTable));
					if (flag5)
					{
						type2 = type2.GetGenericArguments()[0];
						flag3 = true;
					}
					PXMappedCacheExtension pxmappedCacheExtension;
					bool flag6;
					if (type2.IsCompatibleWith(typeof(PXMappedCacheExtension)))
					{
						pxmappedCacheExtension = (value as PXMappedCacheExtension);
						flag6 = (pxmappedCacheExtension != null);
					}
					else
					{
						flag6 = false;
					}
					bool flag7 = flag6;
					if (flag7)
					{
						value = pxmappedCacheExtension.Base;
						type2 = AsgardUtils.GetItemType(value);
					}
					bool flag8 = type2.BaseType.IsGenericType && type2.BaseType.IsCompatibleWith(typeof(PXCacheExtension)) && type2.BaseType.GetGenericArguments().Count<Type>() == 1 && type2.BaseType.GetGenericArguments()[0].IsCompatibleWith(typeof(IBqlTable));
					if (flag8)
					{
						PropertyInfo property = type2.GetProperty("Base", BindingFlags.Instance | BindingFlags.NonPublic);
						type2 = type2.BaseType.GetGenericArguments()[0];
						value = ((property != null) ? property.GetValue(value) : null);
					}
					bool flag9 = type2.IsCompatibleWith(typeof(IBqlTable));
					if (flag9)
					{
						PXCache cache = context.GetCache(type2, true);
						bool flag10 = cache != null;
						EntityHelper entityHelper;
						string value2;
						if (flag10)
						{
							PXGraph graph = cache.Graph;
							entityHelper = new EntityHelper(graph);
							value2 = EntityHelper.GetFriendlyEntityName(type2);
						}
						else
						{
							PXGraph rowGraph = ContextVariables.GetRowGraph(context);
							entityHelper = new EntityHelper(rowGraph);
							value2 = type2.Name;
						}
						StringBuilder stringBuilder = new StringBuilder();
						bool flag11 = flag3;
						ICollection collection;
						if (flag11)
						{
							stringBuilder.Append("Coll. of ");
							collection = (ICollection)value;
						}
						else
						{
							collection = new object[]
							{
								value
							};
						}
						stringBuilder.Append(value2).Append(" (");
						foreach (object obj in collection)
						{
							object obj2 = obj;
							bool flag12 = obj2 is PXResult;
							if (flag12)
							{
								obj2 = PXResult.UnwrapMain(obj2);
							}
							object[] entityRowKeys = entityHelper.GetEntityRowKeys(type2, obj2);
							string value3 = string.Join(", ", entityRowKeys);
							stringBuilder.Append(value3);
							stringBuilder.Append("/");
						}
						StringBuilder stringBuilder2 = stringBuilder;
						int length = stringBuilder2.Length;
						stringBuilder2.Length = length - 1;
						stringBuilder.Append(")");
						result = stringBuilder.ToString();
					}
					else
					{
						result = ((value != null) ? value.ToString() : null);
					}
				}
			}
			return result;
		}

		// Token: 0x0600155C RID: 5468 RVA: 0x0004CF8C File Offset: 0x0004B18C
		public static bool TryCast<T>(this object obj, out T result)
		{
			bool flag = obj is T;
			bool result2;
			if (flag)
			{
				result = (T)((object)obj);
				result2 = true;
			}
			else
			{
				result = default(T);
				result2 = false;
			}
			return result2;
		}

		// Token: 0x0600155D RID: 5469 RVA: 0x0004CFC4 File Offset: 0x0004B1C4
		public static PXCache GetCache(this TemplateContext context, Type cacheType, bool silent = false)
		{
			bool flag = cacheType == null && !silent;
			if (flag)
			{
				throw new PXException("No cache type provided");
			}
			string fullName = (cacheType != null) ? cacheType.FullName : null;
			PXGraph rowGraph = ContextVariables.GetRowGraph(context);
			PXCacheCollection caches = rowGraph.Caches;
			PXCache value = caches.FirstOrDefault((KeyValuePair<Type, PXCache> cac) => cac.Key.FullName.EndsWith(fullName)).Value;
			bool flag2 = value == null && !silent;
			if (flag2)
			{
				throw new PXException("No cache named or related to '{0}' found", new object[]
				{
					fullName
				});
			}
			return value;
		}

		// Token: 0x0600155E RID: 5470 RVA: 0x0004D068 File Offset: 0x0004B268
		public static IEnumerable<string> GetMembers(object row)
		{
			GenericResult genericResult = row as GenericResult;
			bool flag = genericResult == null || genericResult.Values.Count == 0;
			IEnumerable<string> result;
			if (flag)
			{
				result = Enumerable.Empty<string>();
			}
			else
			{
				IDictionary<string, object> values = genericResult.Values;
				IEnumerable<Type> source = values.Select(delegate(KeyValuePair<string, object> vkp)
				{
					object value = vkp.Value;
					return (value != null) ? value.GetType() : null;
				});
				Func<Type, bool> predicate;
				if ((predicate = ScribanUtils.<>O.<0>__IsBqlTable) == null)
				{
					predicate = (ScribanUtils.<>O.<0>__IsBqlTable = new Func<Type, bool>(AsgardUtils.IsBqlTable));
				}
				List<Type> source2 = source.Where(predicate).ToList<Type>();
				string[] array = (from it in source2
				select it.Name).ToArray<string>();
				result = array;
			}
			return result;
		}

		// Token: 0x0600155F RID: 5471 RVA: 0x0004D128 File Offset: 0x0004B328
		public static T EvalExpr<T>(this TemplateContext scribanContext, string expr, T defaultValue = default(T))
		{
			bool flag = string.IsNullOrEmpty(expr);
			T result;
			if (flag)
			{
				result = defaultValue;
			}
			else
			{
				Type typeFromHandle = typeof(T);
				expr = expr.ToScriban();
				Template template = Template.Parse(expr, null, null, null);
				ScribanUtils.CheckTemplateErrors("Expression", expr, template);
				object value = template.Evaluate(scribanContext);
				try
				{
					object obj = AsgardUtils.ChangeType(value, typeFromHandle, null);
					T t;
					bool flag2;
					if (obj is T)
					{
						t = (T)((object)obj);
						flag2 = true;
					}
					else
					{
						flag2 = false;
					}
					bool flag3 = flag2;
					if (flag3)
					{
						result = t;
					}
					else
					{
						result = defaultValue;
					}
				}
				catch (Exception ex)
				{
					PXTrace.WriteError(ex);
					throw new PXException("You are trying to convert expression '{0}' to type '{1}' but it is not possible", new object[]
					{
						expr,
						typeFromHandle.Name
					});
				}
			}
			return result;
		}

		// Token: 0x06001560 RID: 5472 RVA: 0x0004D1FC File Offset: 0x0004B3FC
		public static T EvalExpr<T>(object obj, string scribanExpr, T defaultValue = default(T))
		{
			bool flag = obj != null;
			T result;
			if (flag)
			{
				BaseTemplateContext baseTemplateContext = new BaseTemplateContext();
				baseTemplateContext.SetValue("SINGLE", obj);
				T t = baseTemplateContext.EvalExpr("SINGLE." + scribanExpr, default(T));
				result = t;
			}
			else
			{
				result = defaultValue;
			}
			return result;
		}

		// Token: 0x06001561 RID: 5473 RVA: 0x0004D250 File Offset: 0x0004B450
		public static bool HasScriban(this string text)
		{
			bool flag = string.IsNullOrEmpty(text);
			return !flag && ((text.Contains("{{") && text.Contains("}}")) || (text.Contains("{%") && text.Contains("}%")));
		}

		// Token: 0x06001562 RID: 5474 RVA: 0x0004D2AC File Offset: 0x0004B4AC
		public static bool HasMoreToRender(this string text)
		{
			return text.Contains("{{") && text.Contains("}}");
		}

		// Token: 0x06001563 RID: 5475 RVA: 0x0004D2DC File Offset: 0x0004B4DC
		public static string EscapeExpression(string expr, int level = 1)
		{
			string str = new string('%', level);
			string startsWith = "{" + str + "{";
			string endsWith = "}" + str + "}";
			return BasicLabelUtils.SurroundBy(expr, startsWith, endsWith);
		}

		// Token: 0x06001564 RID: 5476 RVA: 0x0004D324 File Offset: 0x0004B524
		public static string Stringify(object value)
		{
			bool flag = value == null;
			string result;
			if (flag)
			{
				result = "NULL";
			}
			else
			{
				ScriptNode scriptNode = value as ScriptNode;
				bool flag2 = scriptNode != null;
				if (flag2)
				{
					result = scriptNode.ToString();
				}
				else
				{
					Type type = value.GetType();
					string text = value as string;
					bool flag3 = text != null;
					if (flag3)
					{
						text = text.ReplaceCR(" + ");
						text = text.CleanWhitespaces();
						result = "\"" + text + "\"";
					}
					else
					{
						bool flag4 = type.IsPrimitive || type.IsValueType;
						if (flag4)
						{
							result = value.ToString();
						}
						else
						{
							DynamicCustomFunction dynamicCustomFunction = value as DynamicCustomFunction;
							bool flag5 = dynamicCustomFunction != null;
							if (flag5)
							{
								MethodInfo method = dynamicCustomFunction.Method;
								result = "Method call to '" + method.Signature() + "'";
							}
							else
							{
								bool flag6 = false;
								Type itemType = AsgardUtils.GetItemType(value);
								Type type2 = typeof(object);
								bool flag7 = itemType.IsCompatibleWith(typeof(ICollection));
								if (flag7)
								{
									flag6 = true;
								}
								bool flag8 = itemType.IsGenericType && itemType.IsCompatibleWith(typeof(ICollection)) && itemType.GetGenericArguments().Count<Type>() == 1;
								if (flag8)
								{
									type2 = itemType.GetGenericArguments()[0];
								}
								StringBuilder stringBuilder = new StringBuilder();
								bool flag9 = flag6;
								if (flag9)
								{
									stringBuilder.Append("Coll. of " + type2.Name);
									ICollection collection = (ICollection)value;
									bool flag10 = collection.Count > 0;
									if (flag10)
									{
										stringBuilder.Append("[\n");
										foreach (object value2 in collection)
										{
											stringBuilder.Append(ScribanUtils.StringifyRow(value2));
											stringBuilder.Append("\n");
										}
										stringBuilder.Length -= 2;
										stringBuilder.Append("]");
										result = stringBuilder.ToString();
									}
									else
									{
										stringBuilder.Append(": empty!");
										result = stringBuilder.ToString();
									}
								}
								else
								{
									result = ScribanUtils.StringifyRow(value);
								}
							}
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06001565 RID: 5477 RVA: 0x0004D584 File Offset: 0x0004B784
		private static string StringifyRow(object value)
		{
			bool flag = value == null;
			string result;
			if (flag)
			{
				result = "NULL";
			}
			else
			{
				GenericResult genericResult = value as GenericResult;
				bool flag2 = genericResult != null;
				if (flag2)
				{
					result = ScribanUtils.StringifyGenericResult(genericResult);
				}
				else
				{
					PXResult pxresult = value as PXResult;
					bool flag3 = pxresult != null;
					if (flag3)
					{
						result = ScribanUtils.StringifyPXResult(pxresult);
					}
					else
					{
						IBqlTable bqlTable = value as IBqlTable;
						bool flag4 = bqlTable != null;
						if (flag4)
						{
							result = ScribanUtils.StringifyBqlTable(bqlTable);
						}
						else
						{
							Type itemType = AsgardUtils.GetItemType(value);
							result = "An object of type '" + itemType.Name + "' : value.ToString()";
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06001566 RID: 5478 RVA: 0x0004D61C File Offset: 0x0004B81C
		private static string StringifyGenericResult(GenericResult gr)
		{
			IEnumerable<string> members = ScribanUtils.GetMembers(gr);
			return string.Format("GenericResult: ({0})", string.Join(", ", members));
		}

		// Token: 0x06001567 RID: 5479 RVA: 0x0004D64C File Offset: 0x0004B84C
		private static string StringifyPXResult(PXResult pxr)
		{
			IEnumerable<string> members = ScribanUtils.GetMembers(pxr);
			return string.Format("PXResult: ({0})", string.Join(", ", members));
		}

		// Token: 0x06001568 RID: 5480 RVA: 0x0004D67C File Offset: 0x0004B87C
		private static string StringifyBqlTable(IBqlTable bql)
		{
			string name = bql.GetType().Name;
			IDocumentKey documentKey = bql as IDocumentKey;
			bool flag = documentKey != null;
			string arg;
			if (flag)
			{
				arg = string.Format("{0}: {1}/{2}", name, (documentKey != null) ? documentKey.DocType : null, (documentKey != null) ? documentKey.RefNbr : null);
			}
			else
			{
				arg = bql.ToString();
			}
			return string.Format("{0}: {1}", name, arg);
		}

		// Token: 0x06001569 RID: 5481 RVA: 0x0004D6E8 File Offset: 0x0004B8E8
		public static IEnumerable<Type> GetImplementations()
		{
			return AsgardUtils.GetImplementations(typeof(ISubstitution), true);
		}

		// Token: 0x0600156A RID: 5482 RVA: 0x0004D70C File Offset: 0x0004B90C
		[return: TupleElementNames(new string[]
		{
			"Prefix",
			"ImportFlags"
		})]
		public static ValueTuple<string, ScriptMemberImportFlags> GetLibInfo(Type funcLib)
		{
			ISubstitution substitution = ScribanUtils.CreateLibrary(funcLib);
			return new ValueTuple<string, ScriptMemberImportFlags>(substitution.Prefix, substitution.ImportFlags);
		}

		// Token: 0x0600156B RID: 5483 RVA: 0x0004D738 File Offset: 0x0004B938
		private static ISubstitution CreateLibrary(Type funcLib)
		{
			return (ISubstitution)Activator.CreateInstance(funcLib);
		}

		// Token: 0x0600156C RID: 5484 RVA: 0x0004D758 File Offset: 0x0004B958
		public static MethodInfo[] GetLibraryMethods(Type funcLib)
		{
			return (from mi in funcLib.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy)
			orderby mi.Name
			select mi).ToArray<MethodInfo>();
		}

		// Token: 0x04000900 RID: 2304
		public const string DOUBLE_OPEN_BRACE = "{{";

		// Token: 0x04000901 RID: 2305
		public const string DOUBLE_CLOSE_BRACE = "}}";

		// Token: 0x04000902 RID: 2306
		public const string ESCAPE_START = "{";

		// Token: 0x04000903 RID: 2307
		public const string ESCAPE_END = "}";

		// Token: 0x04000904 RID: 2308
		public const char ESCAPE_CHAR = '%';

		// Token: 0x04000905 RID: 2309
		public const string STANDARD_DETAIL_VIEW = "ALDetails";

		// Token: 0x04000906 RID: 2310
		public const string STANDARD_ALLOC_VIEW = "ALAllocations";

		// Token: 0x0200094F RID: 2383
		public static class Errors
		{
			// Token: 0x040012BD RID: 4797
			public const string TemplateHasErrors = "Template has errors : \n{0}";
		}

		// Token: 0x02000950 RID: 2384
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x040012BE RID: 4798
			public static Func<Type, bool> <0>__IsBqlTable;
		}
	}
}
