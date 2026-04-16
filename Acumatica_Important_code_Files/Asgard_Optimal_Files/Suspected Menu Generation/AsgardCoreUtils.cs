using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Web.Compilation;
using Asgard.Labels.Abstractions.Helpers;
using Fasterflect;
using Fasterflect.Extensions;
using PX.Common;
using PX.Data;
using PX.Objects.CR;
using PX.SM;

namespace AA.Objects.Core
{
	// Token: 0x0200001F RID: 31
	public static class AsgardCoreUtils
	{
		// Token: 0x060000BA RID: 186 RVA: 0x000044D8 File Offset: 0x000026D8
		public static string Signature(this MethodInfo mi)
		{
			Func<ParameterInfo, bool> paramSelector;
			if ((paramSelector = AsgardCoreUtils.<>O.<0>__Keep) == null)
			{
				paramSelector = (AsgardCoreUtils.<>O.<0>__Keep = new Func<ParameterInfo, bool>(AsgardCoreUtils.Keep));
			}
			return mi.Signature(paramSelector);
		}

		// Token: 0x060000BB RID: 187 RVA: 0x0000450C File Offset: 0x0000270C
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
				if ((selector = AsgardCoreUtils.<>O.<1>__ParamToString) == null)
				{
					selector = (AsgardCoreUtils.<>O.<1>__ParamToString = new Func<ParameterInfo, string>(AsgardCoreUtils.ParamToString));
				}
				IEnumerable<string> values = source.Select(selector);
				string text = string.Format("{1}({2}) -> {0}", AsgardCoreUtils.TypeToString(mi.ReturnType), mi.Name, string.Join(", ", values));
				result = text;
			}
			return result;
		}

		// Token: 0x060000BC RID: 188 RVA: 0x00004584 File Offset: 0x00002784
		public static string Signature(this MethodBase mi)
		{
			Func<ParameterInfo, bool> paramSelector;
			if ((paramSelector = AsgardCoreUtils.<>O.<0>__Keep) == null)
			{
				paramSelector = (AsgardCoreUtils.<>O.<0>__Keep = new Func<ParameterInfo, bool>(AsgardCoreUtils.Keep));
			}
			return mi.Signature(paramSelector);
		}

		// Token: 0x060000BD RID: 189 RVA: 0x000045B8 File Offset: 0x000027B8
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
				if ((selector = AsgardCoreUtils.<>O.<1>__ParamToString) == null)
				{
					selector = (AsgardCoreUtils.<>O.<1>__ParamToString = new Func<ParameterInfo, string>(AsgardCoreUtils.ParamToString));
				}
				IEnumerable<string> values = source.Select(selector);
				string text = string.Format("{0}({1})", mb.Name, string.Join(", ", values));
				result = text;
			}
			return result;
		}

		// Token: 0x060000BE RID: 190 RVA: 0x00004624 File Offset: 0x00002824
		public static bool Keep(ParameterInfo pi)
		{
			return true;
		}

		// Token: 0x060000BF RID: 191 RVA: 0x00004638 File Offset: 0x00002838
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

		// Token: 0x060000C0 RID: 192 RVA: 0x00004684 File Offset: 0x00002884
		public static string ParamToString(ParameterInfo pi)
		{
			string arg = "";
			bool flag = pi.DefaultValue != DBNull.Value;
			if (flag)
			{
				arg = " = " + AsgardCoreUtils.GetValueForTrace(pi.DefaultValue);
			}
			string arg2 = AsgardCoreUtils.TypeToString(pi.ParameterType);
			return string.Format("{0} {1}{2}", arg2, AsgardCoreUtils.FixName(pi), arg);
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x000046E8 File Offset: 0x000028E8
		public static string GetValueForTrace(object[] values, int index)
		{
			object value = (index < 0 || index > values.Length - 1) ? null : values[index];
			return AsgardCoreUtils.GetValueForTrace(value);
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x00004714 File Offset: 0x00002914
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

		// Token: 0x060000C3 RID: 195 RVA: 0x00004760 File Offset: 0x00002960
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
				text = text + "<" + string.Join(", ", AsgardCoreUtils.TypesToStrings(type.GenericTypeArguments)) + ">";
			}
			return text;
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x00004ADC File Offset: 0x00002CDC
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
				select AsgardCoreUtils.TypeToString(ty)).ToArray<string>();
				result = array;
			}
			return result;
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x00004B2C File Offset: 0x00002D2C
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
			Type result2;
			if (ipxresultset == null)
			{
				PXResult pxresult = result as PXResult;
				result2 = ((pxresult != null) ? AsgardCoreUtils.GetItemType(pxresult, 0) : ((result != null) ? result.GetType() : null));
			}
			else
			{
				result2 = ipxresultset.GetItemType(0);
			}
			return result2;
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x00004BAC File Offset: 0x00002DAC
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
					IList<Type> itemTypes = AsgardCoreUtils.GetItemTypes(gr);
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
							return AsgardCoreUtils.GetItemType(pxresult2, index);
						}
					}
				}
			}
			return type;
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x00004C84 File Offset: 0x00002E84
		public static Type GetItemType(PXResult row, int index)
		{
			return row.GetItemType(index);
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x00004CA0 File Offset: 0x00002EA0
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

		// Token: 0x060000C9 RID: 201 RVA: 0x00004CD8 File Offset: 0x00002ED8
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
					result = AsgardCoreUtils.GetItemTypes(list);
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

		// Token: 0x060000CA RID: 202 RVA: 0x00004DC4 File Offset: 0x00002FC4
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
					return AsgardCoreUtils.GetItemTypes(gr);
				}
				GenericResult genericResult = obj as GenericResult;
				bool flag3 = genericResult != null;
				if (flag3)
				{
					return AsgardCoreUtils.GetItemTypes(genericResult);
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

		// Token: 0x060000CB RID: 203 RVA: 0x00004EBC File Offset: 0x000030BC
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

		// Token: 0x060000CC RID: 204 RVA: 0x00004F1C File Offset: 0x0000311C
		public static IList<string> GetItemTypeNames(this IPXResultset rs)
		{
			IList<Type> itemTypes = rs.GetItemTypes();
			return (from it in itemTypes
			select it.Name).ToList<string>();
		}

		// Token: 0x060000CD RID: 205 RVA: 0x00004F64 File Offset: 0x00003164
		public static IList<string> GetItemTypeNames(this PXResult res)
		{
			IList<Type> itemTypes = res.GetItemTypes();
			return (from it in itemTypes
			select it.Name).ToList<string>();
		}

		// Token: 0x060000CE RID: 206 RVA: 0x00004FAC File Offset: 0x000031AC
		public static IList<string> GetItemTypeNames(this ViewDef viewDef)
		{
			Type[] itemTypes = viewDef.ItemTypes;
			return (from it in itemTypes
			select it.Name).ToList<string>();
		}

		// Token: 0x060000CF RID: 207 RVA: 0x00004FF4 File Offset: 0x000031F4
		public static string GetItemTypeName(object _row_)
		{
			Type itemType = AsgardCoreUtils.GetItemType(_row_, true);
			return (itemType != null) ? itemType.Name : null;
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x0000501C File Offset: 0x0000321C
		public static Type GetItemType(object _row_, bool silent = false)
		{
			Type itemType = AsgardCoreUtils.GetItemType(_row_);
			if (!(itemType == null) || silent)
			{
				return itemType;
			}
			throw new PXException("Cannot find Item Type for row of type '{0}'", new object[]
			{
				(_row_ != null) ? _row_.GetType() : null
			});
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x00005064 File Offset: 0x00003264
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

		// Token: 0x060000D2 RID: 210 RVA: 0x00005128 File Offset: 0x00003328
		public static int GetTableCount(this GenericResult gr)
		{
			return (gr != null) ? gr.Values.Values.Count : 0;
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x00005150 File Offset: 0x00003350
		public static int GetTableCount(this PXResult pxr)
		{
			return (pxr != null) ? pxr.GetType().GetGenericArguments().Length : 0;
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x00005178 File Offset: 0x00003378
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

		// Token: 0x060000D5 RID: 213 RVA: 0x000051BC File Offset: 0x000033BC
		public static Type GetType(string typename)
		{
			return (typename == null) ? null : (PXBuildManager.GetType(typename, false) ?? Type.GetType(typename));
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x000051E8 File Offset: 0x000033E8
		public static bool IsAttribute(string fieldName)
		{
			return fieldName != null && (fieldName.EndsWith("_Attributes") || fieldName.StartsWith("Attribute"));
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x00005220 File Offset: 0x00003420
		public static string GetAttributeID(string fieldName)
		{
			string result = null;
			bool flag = AsgardCoreUtils.IsAttribute(fieldName);
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

		// Token: 0x060000D8 RID: 216 RVA: 0x00005294 File Offset: 0x00003494
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

		// Token: 0x060000D9 RID: 217 RVA: 0x00005334 File Offset: 0x00003534
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

		// Token: 0x060000DA RID: 218 RVA: 0x000053CC File Offset: 0x000035CC
		public static string GetDisplayName(PXView view)
		{
			bool flag = view == null;
			string result;
			if (flag)
			{
				result = null;
			}
			else
			{
				PXGraph graph = view.Graph;
				string name = view.Name;
				FieldInfo field = AsgardCoreUtils.GetField(graph.GetType(), name);
				bool flag2 = field == null;
				if (flag2)
				{
					result = null;
				}
				else
				{
					PXViewNameAttribute pxviewNameAttribute = field.GetCustomAttributes().OfType<PXViewNameAttribute>().FirstOrDefault<PXViewNameAttribute>();
					string text = (pxviewNameAttribute != null) ? pxviewNameAttribute.GetName() : null;
					result = text;
				}
			}
			return result;
		}

		// Token: 0x060000DB RID: 219 RVA: 0x00005440 File Offset: 0x00003640
		public static FieldInfo GetField(Type _graphType, string memberName)
		{
			return AsgardCoreUtils.GetFields(_graphType, new string[]
			{
				memberName
			}).FirstOrDefault<FieldInfo>();
		}

		// Token: 0x060000DC RID: 220 RVA: 0x0000546C File Offset: 0x0000366C
		private static IEnumerable<FieldInfo> GetFields(Type _graphType, params string[] onlyMemberNames)
		{
			return (from fi in AsgardCoreUtils.GetFields(_graphType)
			where onlyMemberNames.Contains(fi.Name)
			select fi).ToArray<FieldInfo>();
		}

		// Token: 0x060000DD RID: 221 RVA: 0x000054AC File Offset: 0x000036AC
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
				if ((valueFactory = AsgardCoreUtils.<>O.<2>__GetFieldsInternal) == null)
				{
					valueFactory = (AsgardCoreUtils.<>O.<2>__GetFieldsInternal = new Func<Type, IEnumerable<FieldInfo>>(AsgardCoreUtils.GetFieldsInternal));
				}
				IEnumerable<FieldInfo> orAdd = CacheHelper2<Type, IEnumerable<FieldInfo>>.GetOrAdd(type, valueFactory);
				result = orAdd;
			}
			return result;
		}

		// Token: 0x060000DE RID: 222 RVA: 0x000054F8 File Offset: 0x000036F8
		private static IEnumerable<FieldInfo> GetFieldsInternal(Type type)
		{
			List<FieldInfo> list = new List<FieldInfo>();
			BindingFlags bindingAttr = BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy;
			FieldInfo[] fields = type.GetFields(bindingAttr);
			list.AddRange(fields);
			bool flag = typeof(PXGraph).IsAssignableFrom(type);
			if (flag)
			{
				foreach (Type type2 in AsgardCoreUtils.GetExtensions(type, true))
				{
					FieldInfo[] fields2 = type2.GetFields(bindingAttr);
					list.AddRange(fields2);
				}
			}
			list = list.Distinct<FieldInfo>().ToList<FieldInfo>();
			return list;
		}

		// Token: 0x060000DF RID: 223 RVA: 0x000055A4 File Offset: 0x000037A4
		public static List<Type> GetExtensions(Type tgraph, bool checkActive)
		{
			MethodBase currentMethod = MethodBase.GetCurrentMethod();
			string name = currentMethod.Name;
			Type[] parameterTypes = (from pi in currentMethod.GetParameters()
			select pi.ParameterType).ToArray<Type>();
			Type pxextensionManagerType = AsgardCoreUtils.GetPXExtensionManagerType();
			MethodInvoker methodInvoker = Reflect.Method(pxextensionManagerType, name, FasterflectFlags.StaticAnyVisibility, parameterTypes);
			return (List<Type>)methodInvoker(null, new object[]
			{
				tgraph,
				checkActive
			});
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x00005630 File Offset: 0x00003830
		public static Type GetPXExtensionManagerType()
		{
			Assembly assembly = typeof(PXException).Assembly;
			return assembly.GetType("PX.Data.PXExtensionManager", true);
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x00005660 File Offset: 0x00003860
		public static object[] GetKeys(PXCache cache, object row)
		{
			string[] source = (from key in cache.BqlKeys
			select key.Name).ToArray<string>();
			return (from key in source
			select cache.GetValue(row, key)).ToArray<object>();
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x000056D4 File Offset: 0x000038D4
		public static IDictionary<string, object> GetKeysAsDict(PXCache cache, object row)
		{
			return cache.BqlKeys.ToDictionary((Type fieldName) => fieldName.Name, (Type fieldName) => cache.GetValue(row, fieldName.Name));
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x00005738 File Offset: 0x00003938
		public static E FindCacheExtension<E>(object row)
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

		// Token: 0x060000E4 RID: 228 RVA: 0x000057B0 File Offset: 0x000039B0
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

		// Token: 0x060000E5 RID: 229 RVA: 0x000057E4 File Offset: 0x000039E4
		public static PXGraphExtension FindGraphExtension<G>(PXGraph graph)
		{
			PXGraphExtension[] graphExtensions = HiddenUtils.GetGraphExtensions(graph);
			return graphExtensions.FirstOrDefault((PXGraphExtension ext) => ext.GetType().IsCompatibleWith(typeof(G)));
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x00005824 File Offset: 0x00003A24
		public static string Decrypt(string source)
		{
			bool flag = string.IsNullOrEmpty(source);
			string result;
			if (flag)
			{
				result = source;
			}
			else
			{
				MethodBase currentMethod = MethodBase.GetCurrentMethod();
				string name = currentMethod.Name;
				Type[] parameterTypes = (from pi in currentMethod.GetParameters()
				select pi.ParameterType).ToArray<Type>();
				MethodInvoker methodInvoker = Reflect.Method(typeof(PXRSACryptStringAttribute), name, FasterflectFlags.StaticAnyVisibility, parameterTypes);
				string text = (string)methodInvoker(null, new object[]
				{
					source
				});
				result = text;
			}
			return result;
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x000058B8 File Offset: 0x00003AB8
		public static object GetValueFromCache(PXGraph graph, object row, Type fieldType)
		{
			Type itemType = BqlCommand.GetItemType(fieldType);
			PXCache pxcache = graph.Caches[itemType];
			bool flag = row != null && row.GetType() != itemType;
			if (flag)
			{
				row = null;
			}
			if (row == null)
			{
				row = pxcache.Current;
			}
			return AsgardCoreUtils.GetValueFromCache(pxcache, row, fieldType.Name);
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x00005918 File Offset: 0x00003B18
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

		// Token: 0x060000E9 RID: 233 RVA: 0x0000596C File Offset: 0x00003B6C
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

		// Token: 0x060000EA RID: 234 RVA: 0x000059DC File Offset: 0x00003BDC
		public static string GetSource()
		{
			return PXUrl.SiteUrlWithPath();
		}

		// Token: 0x060000EB RID: 235 RVA: 0x000059F4 File Offset: 0x00003BF4
		public static bool HasNote(PXGraph graph, object row)
		{
			Type type = row.GetType();
			PXCache pxcache = graph.Caches[type];
			return PXNoteAttribute.GetNoteIDIfExists(pxcache, row) != null;
		}

		// Token: 0x060000EC RID: 236 RVA: 0x00005A2C File Offset: 0x00003C2C
		public static CREmployee GetOwner(PXGraph graph, Guid? userID = null)
		{
			Guid value = userID.GetValueOrDefault();
			if (userID == null)
			{
				value = AsgardCoreUtils.GetUserID(graph);
				userID = new Guid?(value);
			}
			PXResultset<CREmployee> pxresultset = PXSelectBase<CREmployee, PXSelect<CREmployee, Where<CREmployee.userID, Equal<Required<CREmployee.userID>>>>.Config>.Select(graph, new object[]
			{
				userID
			});
			return pxresultset;
		}

		// Token: 0x060000ED RID: 237 RVA: 0x00005A7C File Offset: 0x00003C7C
		public static CREmployee GetOwner(PXGraph graph, int? ownerID = null)
		{
			int? num = ownerID;
			if (num == null)
			{
				ownerID = AsgardCoreUtils.GetOwnerID(graph, null);
			}
			PXResultset<CREmployee> pxresultset = PXSelectBase<CREmployee, PXSelect<CREmployee, Where<CREmployee.defContactID, Equal<Required<CREmployee.defContactID>>>>.Config>.Select(graph, new object[]
			{
				ownerID
			});
			return pxresultset;
		}

		// Token: 0x060000EE RID: 238 RVA: 0x00005AC8 File Offset: 0x00003CC8
		public static int? GetOwnerID(PXGraph graph, Guid? userID = null)
		{
			CREmployee owner = AsgardCoreUtils.GetOwner(graph, userID);
			return (owner != null) ? owner.DefContactID : null;
		}

		// Token: 0x060000EF RID: 239 RVA: 0x00005AF8 File Offset: 0x00003CF8
		public static Guid GetUserID(PXGraph graph)
		{
			return graph.Accessinfo.UserID;
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x00005B18 File Offset: 0x00003D18
		public static Users GetUser(PXGraph graph, Guid? userID = null)
		{
			Guid value = userID.GetValueOrDefault();
			if (userID == null)
			{
				value = AsgardCoreUtils.GetUserID(graph);
				userID = new Guid?(value);
			}
			return Users.PK.Find(graph, userID, 0);
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x00005B58 File Offset: 0x00003D58
		public static UserPreferences GetUserPreferences(PXGraph graph, Guid? userID = null)
		{
			Guid value = userID.GetValueOrDefault();
			if (userID == null)
			{
				value = AsgardCoreUtils.GetUserID(graph);
				userID = new Guid?(value);
			}
			return UserPreferences.PK.Find(graph, userID, 0);
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x00005B98 File Offset: 0x00003D98
		public static Type GetGraphType(PXGraph graph, Type graphTypeField, object row)
		{
			Type itemType = BqlCommand.GetItemType(graphTypeField);
			PXCache pxcache = graph.Caches[itemType];
			bool flag = row != null && row.GetType() != itemType;
			if (flag)
			{
				row = null;
			}
			if (row == null)
			{
				row = pxcache.Current;
			}
			string text = (string)AsgardCoreUtils.GetValueFromCache(pxcache, row, graphTypeField.Name);
			bool flag2 = string.IsNullOrEmpty(text);
			Type result;
			if (flag2)
			{
				result = null;
			}
			else
			{
				Type type = GraphHelper.GetType(text);
				result = type;
			}
			return result;
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x00005C18 File Offset: 0x00003E18
		public static bool IsFilteredGraph(PXGraph graph)
		{
			return AsgardCoreUtils.IsFilteredGraph((graph != null) ? graph.GetType() : null);
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x00005C3C File Offset: 0x00003E3C
		public static bool IsFilteredGraph(Type graphType)
		{
			return AsgardCoreUtils.GetFilteredView(graphType) != null;
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x00005C58 File Offset: 0x00003E58
		public static string GetFilterView(Type graphType)
		{
			Func<Type, string> valueFactory;
			if ((valueFactory = AsgardCoreUtils.<>O.<3>__GetFilterViewInternal) == null)
			{
				valueFactory = (AsgardCoreUtils.<>O.<3>__GetFilterViewInternal = new Func<Type, string>(AsgardCoreUtils.GetFilterViewInternal));
			}
			return CacheHelper2<Type, string>.GetOrAdd(graphType, valueFactory);
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x00005C8C File Offset: 0x00003E8C
		private static string GetFilterViewInternal(Type graphType)
		{
			IEnumerable<FieldInfo> fields = AsgardCoreUtils.GetFields(graphType);
			IEnumerable<FieldInfo> source = fields;
			Func<FieldInfo, bool> predicate;
			if ((predicate = AsgardCoreUtils.<>O.<4>__IsPXFilter) == null)
			{
				predicate = (AsgardCoreUtils.<>O.<4>__IsPXFilter = new Func<FieldInfo, bool>(AsgardCoreUtils.IsPXFilter));
			}
			FieldInfo fieldInfo = source.FirstOrDefault(predicate);
			return (fieldInfo != null) ? fieldInfo.Name : null;
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x00005CD4 File Offset: 0x00003ED4
		public static string GetFilteredView(Type graphType)
		{
			Func<Type, string> valueFactory;
			if ((valueFactory = AsgardCoreUtils.<>O.<5>__GetFilteredViewInternal) == null)
			{
				valueFactory = (AsgardCoreUtils.<>O.<5>__GetFilteredViewInternal = new Func<Type, string>(AsgardCoreUtils.GetFilteredViewInternal));
			}
			return CacheHelper2<Type, string>.GetOrAdd(graphType, valueFactory);
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x00005D08 File Offset: 0x00003F08
		private static string GetFilteredViewInternal(Type graphType)
		{
			IEnumerable<FieldInfo> fields = AsgardCoreUtils.GetFields(graphType);
			IEnumerable<FieldInfo> source = fields;
			Func<FieldInfo, bool> predicate;
			if ((predicate = AsgardCoreUtils.<>O.<4>__IsPXFilter) == null)
			{
				predicate = (AsgardCoreUtils.<>O.<4>__IsPXFilter = new Func<FieldInfo, bool>(AsgardCoreUtils.IsPXFilter));
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
				if ((predicate2 = AsgardCoreUtils.<>O.<6>__IsFilteredResult) == null)
				{
					predicate2 = (AsgardCoreUtils.<>O.<6>__IsFilteredResult = new Func<FieldInfo, bool>(AsgardCoreUtils.IsFilteredResult));
				}
				string text = (from fi in source2.Where(predicate2)
				select fi.Name).FirstOrDefault<string>();
				result = text;
			}
			return result;
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x00005DA0 File Offset: 0x00003FA0
		private static bool IsPXFilter(FieldInfo fi)
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

		// Token: 0x060000FA RID: 250 RVA: 0x00005DDC File Offset: 0x00003FDC
		private static bool IsFilteredResult(FieldInfo fi)
		{
			return !(fi == null) && (AsgardCoreUtils.IsProcessingView(fi) || AsgardCoreUtils.HasPXFilterable(fi));
		}

		// Token: 0x060000FB RID: 251 RVA: 0x00005E0C File Offset: 0x0000400C
		private static bool IsProcessingView(FieldInfo fi)
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

		// Token: 0x060000FC RID: 252 RVA: 0x00005E48 File Offset: 0x00004048
		private static bool HasPXFilterable(FieldInfo fi)
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

		// Token: 0x060000FD RID: 253 RVA: 0x00005E98 File Offset: 0x00004098
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
			string text = (string)AsgardCoreUtils.GetValueFromCache(pxcache, data, fieldType.Name);
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

		// Token: 0x060000FE RID: 254 RVA: 0x00005F18 File Offset: 0x00004118
		public static bool IsGI(PXGraph graph)
		{
			return graph is PXGenericInqGrph;
		}

		// Token: 0x04000047 RID: 71
		public const string ATTR_SUFFIX = "_Attributes";

		// Token: 0x04000048 RID: 72
		public const string PREFIX_UDF = "Attribute";

		// Token: 0x02000075 RID: 117
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x0400011E RID: 286
			public static Func<ParameterInfo, bool> <0>__Keep;

			// Token: 0x0400011F RID: 287
			public static Func<ParameterInfo, string> <1>__ParamToString;

			// Token: 0x04000120 RID: 288
			public static Func<Type, IEnumerable<FieldInfo>> <2>__GetFieldsInternal;

			// Token: 0x04000121 RID: 289
			public static Func<Type, string> <3>__GetFilterViewInternal;

			// Token: 0x04000122 RID: 290
			public static Func<FieldInfo, bool> <4>__IsPXFilter;

			// Token: 0x04000123 RID: 291
			public static Func<Type, string> <5>__GetFilteredViewInternal;

			// Token: 0x04000124 RID: 292
			public static Func<FieldInfo, bool> <6>__IsFilteredResult;
		}
	}
}
