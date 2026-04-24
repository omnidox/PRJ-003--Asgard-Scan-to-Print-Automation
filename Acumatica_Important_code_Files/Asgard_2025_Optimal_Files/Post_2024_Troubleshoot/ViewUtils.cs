using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Asgard.Labels.Abstractions.Helpers;
using Fasterflect;
using PX.Data;
using PX.Objects.CM;
using PX.Objects.Common.Abstractions;

namespace AA.Objects.Core
{
	// Token: 0x02000028 RID: 40
	public static class ViewUtils
	{
		// Token: 0x06000157 RID: 343 RVA: 0x00007088 File Offset: 0x00005288
		public static PXSelectBase GetDataMember(PXGraph _graph, FieldInfo fi)
		{
			bool flag = fi == null;
			PXSelectBase result;
			if (flag)
			{
				result = null;
			}
			else
			{
				PXSelectBase pxselectBase = fi.GetValue(_graph) as PXSelectBase;
				result = pxselectBase;
			}
			return result;
		}

		// Token: 0x06000158 RID: 344 RVA: 0x000070B8 File Offset: 0x000052B8
		public static void ClearViews(PXGraph _graph)
		{
			bool flag = _graph == null;
			if (!flag)
			{
				Type type = _graph.GetType();
				string fullName = type.FullName;
				IEnumerable<ViewDef> enumerable;
				ViewUtils.VIEWS.TryRemove(fullName, out enumerable);
			}
		}

		// Token: 0x06000159 RID: 345 RVA: 0x000070ED File Offset: 0x000052ED
		public static void ClearAllViews()
		{
			ViewUtils.VIEWS.Clear();
		}

		// Token: 0x0600015A RID: 346 RVA: 0x000070FC File Offset: 0x000052FC
		public static ViewDef GetViewDefinition(string graphName, string viewName)
		{
			IEnumerable<ViewDef> source;
			bool flag = ViewUtils.VIEWS.TryGetValue(graphName, out source);
			ViewDef result;
			if (flag)
			{
				result = (from vd in source
				where viewName == vd.InternalName
				select vd).FirstOrDefault<ViewDef>();
			}
			else
			{
				Type type = GraphHelper.GetType(graphName);
				result = ViewUtils.GetViewDefinition(type, viewName);
			}
			return result;
		}

		// Token: 0x0600015B RID: 347 RVA: 0x00007160 File Offset: 0x00005360
		public static ViewDef GetViewDefinition(Type _graphType, string viewName)
		{
			return (from vd in ViewUtils.GetViewDefinitions(_graphType)
			where viewName == vd.InternalName
			select vd).FirstOrDefault<ViewDef>();
		}

		// Token: 0x0600015C RID: 348 RVA: 0x000071A0 File Offset: 0x000053A0
		public static IEnumerable<ViewDef> GetViewDefinitions(Type graphType)
		{
			bool flag = graphType == null;
			IEnumerable<ViewDef> result;
			if (flag)
			{
				result = Enumerable.Empty<ViewDef>();
			}
			else
			{
				string fullName = graphType.FullName;
				IEnumerable<ViewDef> enumerable;
				bool flag2 = ViewUtils.VIEWS.TryGetValue(fullName, out enumerable);
				if (flag2)
				{
					result = enumerable;
				}
				else
				{
					PXGraph graph = HiddenUtils.CreateInstance(graphType);
					result = ViewUtils.GetViewDefinitions(graph);
				}
			}
			return result;
		}

		// Token: 0x0600015D RID: 349 RVA: 0x000071F8 File Offset: 0x000053F8
		public static ViewDef GetViewDefinition(PXGraph _graph, string viewName)
		{
			return (from vd in ViewUtils.GetViewDefinitions(_graph)
			where viewName == vd.InternalName
			select vd).FirstOrDefault<ViewDef>();
		}

		// Token: 0x0600015E RID: 350 RVA: 0x00007238 File Offset: 0x00005438
		public static IEnumerable<ViewDef> GetViewDefinitions(PXGraph _graph)
		{
			string fullName = _graph.GetType().FullName;
			ConcurrentDictionary<string, IEnumerable<ViewDef>> views = ViewUtils.VIEWS;
			string key = fullName;
			Func<string, PXGraph, IEnumerable<ViewDef>> valueFactory;
			if ((valueFactory = ViewUtils.<>O.<0>__GetViewDefinitionsInternal) == null)
			{
				valueFactory = (ViewUtils.<>O.<0>__GetViewDefinitionsInternal = new Func<string, PXGraph, IEnumerable<ViewDef>>(ViewUtils.GetViewDefinitionsInternal));
			}
			return views.GetOrAdd<PXGraph>(key, valueFactory, _graph);
		}

		// Token: 0x0600015F RID: 351 RVA: 0x00007280 File Offset: 0x00005480
		private static IEnumerable<ViewDef> GetViewDefinitionsInternal(string _, PXGraph _graph)
		{
			IList<ViewDef> list = new List<ViewDef>();
			IEnumerable<PXView> source = from kvp in ViewUtils.GetViews(_graph)
			select kvp.Value;
			Func<PXView, bool> predicate;
			if ((predicate = ViewUtils.<>O.<1>__IsUsable) == null)
			{
				predicate = (ViewUtils.<>O.<1>__IsUsable = new Func<PXView, bool>(ViewUtils.IsUsable));
			}
			IEnumerable<PXView> enumerable = source.Where(predicate);
			foreach (PXView pxView in enumerable)
			{
				ViewDef item = new ViewDef(pxView);
				list.Add(item);
			}
			Dictionary<string, ViewDef> dictionary = (from vd in list
			where vd.ItemTypeName != null
			group vd by vd.ItemTypeName).ToDictionary((IGrouping<string, ViewDef> g) => g.Key, (IGrouping<string, ViewDef> g) => g.First<ViewDef>());
			foreach (ViewDef viewDef in list)
			{
				bool flag = string.IsNullOrEmpty(viewDef.DependsOn) && viewDef.HasAtLeastOneDependency();
				if (flag)
				{
					string firstDependency = viewDef.GetFirstDependency();
					ViewDef viewDef2;
					bool flag2 = dictionary.TryGetValue(firstDependency, out viewDef2) && viewDef2.FullName != viewDef.FullName;
					if (flag2)
					{
						viewDef.DependsOn = viewDef2.FullName;
						viewDef.Detail = new bool?(viewDef.Detail.GetValueOrDefault() || ViewUtils.IsDetail(_graph, viewDef2, viewDef));
					}
				}
			}
			return list;
		}

		// Token: 0x06000160 RID: 352 RVA: 0x0000748C File Offset: 0x0000568C
		public static bool IsUsable(PXView view)
		{
			string name = view.Name;
			bool flag = string.IsNullOrEmpty(name) || name.StartsWith("_") || name.Contains("$") || name.Contains("_Wrapper.PX") || name.Contains(".Cst_") || name == "SiteMapSelector";
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				List<PXViewParameter> source = view.EnumParameters();
				bool flag2 = source.Any(delegate(PXViewParameter v)
				{
					IBqlParameter bql = v.Bql;
					return bql != null && bql.GetType().Name.StartsWith("Required");
				});
				result = !flag2;
			}
			return result;
		}

		// Token: 0x06000161 RID: 353 RVA: 0x00007530 File Offset: 0x00005730
		public static Dictionary<string, PXView> GetViews(PXGraph graph)
		{
			return new Dictionary<string, PXView>(graph.Views);
		}

		// Token: 0x06000162 RID: 354 RVA: 0x00007550 File Offset: 0x00005750
		public static PXView GetView(PXGraph graph, string viewName, bool silent = false)
		{
			PXView value = ViewUtils.GetViews(graph).FirstOrDefault((KeyValuePair<string, PXView> view) => view.Key == viewName).Value;
			if (value != null || silent)
			{
				return value;
			}
			throw new PXException("Cannot find a view named '{0}' in graph '{1}'", new object[]
			{
				viewName,
				graph.GetType().FullName
			});
		}

		// Token: 0x06000163 RID: 355 RVA: 0x000075C0 File Offset: 0x000057C0
		private static bool IsDetail(PXGraph graph, ViewDef parentViewDef, ViewDef viewDef)
		{
			string itemTypeName = parentViewDef.ItemTypeName;
			string itemTypeName2 = viewDef.ItemTypeName;
			bool flag = itemTypeName == null || itemTypeName2 == null;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				Type type = BasicHelper.FindType(itemTypeName);
				Type type2 = BasicHelper.FindType(itemTypeName2);
				PXCache pxcache = graph.Caches[type];
				PXCache pxcache2 = graph.Caches[type2];
				result = (pxcache != pxcache2 && ViewUtils.IsParent(pxcache, pxcache2));
			}
			return result;
		}

		// Token: 0x06000164 RID: 356 RVA: 0x00007634 File Offset: 0x00005834
		private static bool IsParent(PXCache parentCache, PXCache childCache)
		{
			List<Type> bqlKeys = parentCache.BqlKeys;
			List<Type> bqlKeys2 = childCache.BqlKeys;
			IEnumerable<PXParentAttribute> source = childCache.GetAttributesReadonly(null).OfType<PXParentAttribute>();
			Type parentType = parentCache.GetItemType();
			Type grandParentType = parentType.BaseType;
			Type itemType = childCache.GetItemType();
			bool flag = parentType.IsCompatibleWith(typeof(IDocumentKey)) && (itemType.IsCompatibleWith(typeof(IDocumentTran)) || itemType.IsCompatibleWith(typeof(ITranTax)));
			bool result;
			if (flag)
			{
				result = true;
			}
			else
			{
				bool flag2 = bqlKeys2.Count >= bqlKeys.Count;
				if (flag2)
				{
					bool flag3 = source.Any((PXParentAttribute parentAttr) => parentAttr.ParentType == parentType || (grandParentType != null && parentAttr.ParentType == grandParentType));
					result = flag3;
				}
				else
				{
					result = false;
				}
			}
			return result;
		}

		// Token: 0x06000165 RID: 357 RVA: 0x00007710 File Offset: 0x00005910
		public static IViewResult GetViewRow(PXGraph docGraph, ViewDef viewDef)
		{
			return new ViewResult(viewDef, docGraph);
		}

		// Token: 0x06000166 RID: 358 RVA: 0x0000772C File Offset: 0x0000592C
		public static IEnumerable<IViewResult> GetViewRows(PXGraph docGraph, IEnumerable<ViewDef> viewDefs)
		{
			List<IViewResult> list = new List<IViewResult>();
			foreach (ViewDef viewDef in viewDefs)
			{
				IViewResult viewRow = ViewUtils.GetViewRow(docGraph, viewDef);
				list.Add(viewRow);
			}
			return list;
		}

		// Token: 0x06000167 RID: 359 RVA: 0x00007790 File Offset: 0x00005990
		public static void TryRedirect(string graphTypeName, string keys)
		{
			Type type = GraphHelper.GetType(graphTypeName);
			PXGraph pxgraph = HiddenUtils.CreateInstance(type);
			object document = ViewUtils.SearchSpecificDocument(pxgraph, keys);
			ViewUtils.SetDocumentCurrent(pxgraph, document);
			PXRedirectHelper.TryRedirect(pxgraph, 3);
		}

		// Token: 0x06000168 RID: 360 RVA: 0x000077C4 File Offset: 0x000059C4
		public static PXGraph GetGraph(string graphTypeName)
		{
			Type type = GraphHelper.GetType(graphTypeName);
			return HiddenUtils.CreateInstance(type);
		}

		// Token: 0x06000169 RID: 361 RVA: 0x000077E8 File Offset: 0x000059E8
		[return: TupleElementNames(new string[]
		{
			"graph",
			"doc"
		})]
		public static ValueTuple<PXGraph, object> GetGraphAndDoc(string graphTypeName, string keys)
		{
			Type type = GraphHelper.GetType(graphTypeName);
			PXGraph pxgraph = HiddenUtils.CreateInstance(type);
			object obj = ViewUtils.SearchSpecificDocument(pxgraph, keys);
			ViewUtils.SetDocumentCurrent(pxgraph, obj);
			return new ValueTuple<PXGraph, object>(pxgraph, obj);
		}

		// Token: 0x0600016A RID: 362 RVA: 0x00007820 File Offset: 0x00005A20
		public static object SearchSpecificDocument(PXGraph graph, string keysAsStr)
		{
			string primaryView = graph.PrimaryView;
			object[] array = (from str in keysAsStr.Split(new char[]
			{
				'|',
				';',
				'/'
			})
			select str.Trim()).ToArray<object>();
			PXCache primaryCache = GraphHelper.GetPrimaryCache(graph);
			KeysCollection keys = primaryCache.Keys;
			array = array.Take(keys.Count).ToArray<object>();
			return ViewUtils.ViewSearch(graph, primaryView, array);
		}

		// Token: 0x0600016B RID: 363 RVA: 0x000078A8 File Offset: 0x00005AA8
		public static void SetDocumentCurrent(PXGraph graph, object document)
		{
			PXCache primaryCache = GraphHelper.GetPrimaryCache(graph);
			Type itemType = primaryCache.GetItemType();
			bool flag = document.GetType() == itemType;
			if (flag)
			{
				primaryCache.Current = document;
			}
		}

		// Token: 0x0600016C RID: 364 RVA: 0x000078E0 File Offset: 0x00005AE0
		public static PXEntryStatus GetStatus(PXGraph graph, string viewName)
		{
			PXCache cache = ViewUtils.GetCache(graph, viewName);
			return (cache.Current != null) ? cache.GetStatus(cache.Current) : 0;
		}

		// Token: 0x0600016D RID: 365 RVA: 0x00007914 File Offset: 0x00005B14
		public static string[] GetFieldNames(PXGraph graph, string viewName)
		{
			return graph.GetFieldNames(viewName);
		}

		// Token: 0x0600016E RID: 366 RVA: 0x00007930 File Offset: 0x00005B30
		public static Type GetItemType(PXGraph graph, string viewName)
		{
			Type result = null;
			try
			{
				result = graph.GetItemType(viewName);
			}
			catch
			{
			}
			return result;
		}

		// Token: 0x0600016F RID: 367 RVA: 0x00007968 File Offset: 0x00005B68
		public static object ViewSearch(PXGraph graph, string viewName, object[] keys)
		{
			int nbKeys = keys.Length;
			PXSelectBase dataMember = GraphHelper.GetDataMember(graph, viewName);
			bool flag = dataMember == null;
			if (flag)
			{
				throw new PXException("Cannot find a view named '{0}' in graph '{1}'", new object[]
				{
					viewName,
					graph.GetType().FullName
				});
			}
			PXCache cache = dataMember.Cache;
			List<Type> bqlKeys = cache.BqlKeys;
			MethodInfo methodInfo = dataMember.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public).Where(delegate(MethodInfo m)
			{
				bool result2;
				if (m.Name == "Search" && m.IsGenericMethodDefinition && m.GetGenericArguments().Length == nbKeys)
				{
					IEnumerable<Type> genericArguments = m.GetGenericArguments();
					Func<Type, bool> predicate;
					if ((predicate = ViewUtils.<>O.<2>__TakesField) == null)
					{
						predicate = (ViewUtils.<>O.<2>__TakesField = new Func<Type, bool>(ViewUtils.TakesField));
					}
					result2 = genericArguments.All(predicate);
				}
				else
				{
					result2 = false;
				}
				return result2;
			}).FirstOrDefault<MethodInfo>();
			bool flag2 = methodInfo == null;
			if (flag2)
			{
				throw new PXException("Cannot find method 'Search' on view '{0}' of type '{1}'", new object[]
				{
					viewName,
					dataMember.GetType().Name
				});
			}
			object result;
			try
			{
				methodInfo = methodInfo.MakeGenericMethod(bqlKeys.ToArray());
				List<object> list = new List<object>();
				list.AddRange(keys);
				list.Add(new object[0]);
				object obj = methodInfo.Invoke(dataMember, list.ToArray());
				IPXResultset ipxresultset = obj as IPXResultset;
				bool flag3 = ipxresultset != null;
				if (flag3)
				{
					obj = ipxresultset.GetRow(0, 0);
				}
				result = obj;
			}
			catch (Exception innerException)
			{
				TargetInvocationException ex = innerException as TargetInvocationException;
				bool flag4 = ex != null && innerException.InnerException != null;
				if (flag4)
				{
					innerException = innerException.InnerException;
				}
				throw new PXException(innerException, "Error calling method 'Search' on view '{0}' of type '{1}'", new object[]
				{
					viewName,
					dataMember.GetType().Name
				});
			}
			return result;
		}

		// Token: 0x06000170 RID: 368 RVA: 0x00007AEC File Offset: 0x00005CEC
		private static bool TakesField(Type type)
		{
			return type.IsCompatibleWith(typeof(IBqlField));
		}

		// Token: 0x06000171 RID: 369 RVA: 0x00007B10 File Offset: 0x00005D10
		public static object ViewSelect(PXGraph graph, string viewName)
		{
			PXSelectBase selectFromGraph = ViewUtils.GetSelectFromGraph(graph, viewName, false);
			bool flag = selectFromGraph == null;
			object result;
			if (flag)
			{
				PXTrace.WriteError("View {0} not found", new object[]
				{
					viewName
				});
				result = null;
			}
			else
			{
				PXCache cache = selectFromGraph.Cache;
				Type type = cache.GetItemType();
				Type type2 = PXViewExtensionsForMobile.CacheType(selectFromGraph.View);
				bool flag2 = type != type2;
				if (flag2)
				{
					type = type2;
				}
				Type type3 = typeof(PXSelectBase).MakeGenericType(new Type[]
				{
					type
				});
				Type resultType = typeof(PXResultset).MakeGenericType(new Type[]
				{
					type
				});
				MethodInfo methodInfo = (from meth in type3.GetMethods(BindingFlags.Instance | BindingFlags.Public)
				where meth.Name == "Select" && meth.ReturnType == resultType
				select meth).FirstOrDefault<MethodInfo>();
				bool flag3 = methodInfo == null;
				if (flag3)
				{
					throw new PXException("Cannot find method 'Select' on view '{0}' of type '{1}'", new object[]
					{
						viewName,
						selectFromGraph.GetType().Name
					});
				}
				try
				{
					object obj = methodInfo.Invoke(selectFromGraph, new object[]
					{
						new object[0]
					});
					object obj2 = obj.FirstResultOrDefault();
					bool flag4 = obj2 != null && !cache.ObjectsEqual(obj2, cache.Current);
					if (flag4)
					{
						cache.Current = obj2;
					}
					result = obj;
				}
				catch (Exception innerException)
				{
					TargetInvocationException ex = innerException as TargetInvocationException;
					bool flag5 = ex != null && innerException.InnerException != null;
					if (flag5)
					{
						innerException = innerException.InnerException;
					}
					throw new PXException(innerException, "Error calling method 'Select' on view '{0}' of type '{1}'", new object[]
					{
						viewName,
						selectFromGraph.GetType().Name
					});
				}
			}
			return result;
		}

		// Token: 0x06000172 RID: 370 RVA: 0x00007CC8 File Offset: 0x00005EC8
		public static string GetRowName(ViewDef view, Type itemType = null, bool shortSuffix = true)
		{
			return view.Detail.GetValueOrDefault() ? ViewUtils.GetIteratorName(view, itemType, shortSuffix) : ViewUtils.GetViewName(view, itemType, shortSuffix);
		}

		// Token: 0x06000173 RID: 371 RVA: 0x00007CFC File Offset: 0x00005EFC
		public static string GetRowName(ViewDef view, string alias)
		{
			return view.Detail.GetValueOrDefault() ? ViewUtils.GetIteratorName(view, alias) : ViewUtils.GetViewName(view, alias);
		}

		// Token: 0x06000174 RID: 372 RVA: 0x00007D30 File Offset: 0x00005F30
		public static string GetViewName(ViewDef view, Type itemType = null, bool shortSuffix = true)
		{
			return ViewUtils.GetViewName(view.InternalName, view.ItemTypeName, itemType, shortSuffix);
		}

		// Token: 0x06000175 RID: 373 RVA: 0x00007D58 File Offset: 0x00005F58
		public static string GetViewName(string internalName, string viewItemType = null, Type itemType = null, bool shortSuffix = true)
		{
			return string.IsNullOrEmpty(internalName) ? null : (internalName + ViewUtils.GetSuffix(viewItemType, itemType, shortSuffix));
		}

		// Token: 0x06000176 RID: 374 RVA: 0x00007D88 File Offset: 0x00005F88
		public static string GetViewName(ViewDef view, string suffix)
		{
			string internalName = view.InternalName;
			return string.IsNullOrEmpty(internalName) ? null : (internalName + ViewUtils.GetSuffix(suffix));
		}

		// Token: 0x06000177 RID: 375 RVA: 0x00007DB8 File Offset: 0x00005FB8
		public static string GetIteratorName(ViewDef view, Type itemType = null, bool shortSuffix = true)
		{
			string viewName = ViewUtils.GetViewName(view, null, true);
			return viewName + "Row" + ViewUtils.GetSuffix(view.ItemTypeName, itemType, shortSuffix);
		}

		// Token: 0x06000178 RID: 376 RVA: 0x00007DF0 File Offset: 0x00005FF0
		public static string GetIteratorName(ViewDef view, string alias)
		{
			string viewName = ViewUtils.GetViewName(view, null, true);
			return viewName + "Row" + ViewUtils.GetSuffix(alias);
		}

		// Token: 0x06000179 RID: 377 RVA: 0x00007E20 File Offset: 0x00006020
		private static string GetSuffix(string viewItemType, Type itemType, bool shortSuffix = true)
		{
			string text = (itemType != null) ? itemType.FullName : null;
			return (itemType == null || text.Equals(viewItemType)) ? "" : (shortSuffix ? ViewUtils.GetSuffix(itemType.Name) : ViewUtils.GetLongSuffix(text));
		}

		// Token: 0x0600017A RID: 378 RVA: 0x00007E70 File Offset: 0x00006070
		private static string GetLongSuffix(string itemTypeName)
		{
			bool flag = string.IsNullOrEmpty(itemTypeName);
			string result;
			if (flag)
			{
				result = null;
			}
			else
			{
				string[] source = itemTypeName.Split(new char[]
				{
					'.'
				});
				int num = source.Count<string>();
				string str = string.Join("", source.Skip(num - 2));
				result = "As" + str;
			}
			return result;
		}

		// Token: 0x0600017B RID: 379 RVA: 0x00007ECC File Offset: 0x000060CC
		private static string GetSuffix(string suffix)
		{
			return string.IsNullOrEmpty(suffix) ? "" : ("As" + suffix);
		}

		// Token: 0x0600017C RID: 380 RVA: 0x00007EF8 File Offset: 0x000060F8
		public static IEnumerable<PXFieldState> GetFields(Type graphType, Type itemType)
		{
			bool flag = graphType == null || itemType == null;
			IEnumerable<PXFieldState> result;
			if (flag)
			{
				result = Enumerable.Empty<PXFieldState>();
			}
			else
			{
				string text = graphType.FullName + "+" + itemType.FullName;
				string key = text;
				Func<string, IEnumerable<PXFieldState>> valueFactory;
				if ((valueFactory = ViewUtils.<>O.<3>__GetFieldsInternal) == null)
				{
					valueFactory = (ViewUtils.<>O.<3>__GetFieldsInternal = new Func<string, IEnumerable<PXFieldState>>(ViewUtils.GetFieldsInternal));
				}
				IEnumerable<PXFieldState> orAdd = CacheHelper2<string, IEnumerable<PXFieldState>>.GetOrAdd(key, valueFactory);
				result = orAdd;
			}
			return result;
		}

		// Token: 0x0600017D RID: 381 RVA: 0x00007F64 File Offset: 0x00006164
		private static IEnumerable<PXFieldState> GetFieldsInternal(string key)
		{
			string[] array = key.Split(new char[]
			{
				'+'
			});
			string text = array[0];
			string text2 = array[1];
			Type type = GraphHelper.GetType(text);
			PXGraph pxgraph = HiddenUtils.CreateInstance(type);
			bool flag = pxgraph == null || string.IsNullOrEmpty(text2);
			IEnumerable<PXFieldState> result;
			if (flag)
			{
				result = Enumerable.Empty<PXFieldState>();
			}
			else
			{
				Type type2 = GraphHelper.GetType(text2);
				Type[] array2 = new Type[]
				{
					type2
				};
				PXFieldState[] fields = PXFieldState.GetFields(pxgraph, array2, false);
				result = fields;
			}
			return result;
		}

		// Token: 0x0600017E RID: 382 RVA: 0x00007FE4 File Offset: 0x000061E4
		public static bool HasJoins(PXGraph graph, string viewName)
		{
			bool flag = graph == null || viewName == null;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				PXSelectBase selectFromGraph = ViewUtils.GetSelectFromGraph(graph, viewName, true);
				result = (selectFromGraph != null && selectFromGraph.GetType().Name.Contains("PXSelectJoin"));
			}
			return result;
		}

		// Token: 0x0600017F RID: 383 RVA: 0x00008030 File Offset: 0x00006230
		public static IEnumerable<Type> GetJoinedTypes(Type graphType, string viewName)
		{
			bool flag = graphType == null || string.IsNullOrEmpty(viewName);
			IEnumerable<Type> result;
			if (flag)
			{
				result = Enumerable.Empty<Type>();
			}
			else
			{
				string text = graphType.FullName + "+" + viewName;
				string key = text;
				Func<string, IEnumerable<Type>> valueFactory;
				if ((valueFactory = ViewUtils.<>O.<4>__GetJoinedTypesInternal) == null)
				{
					valueFactory = (ViewUtils.<>O.<4>__GetJoinedTypesInternal = new Func<string, IEnumerable<Type>>(ViewUtils.GetJoinedTypesInternal));
				}
				IEnumerable<Type> orAdd = CacheHelper2<string, IEnumerable<Type>>.GetOrAdd(key, valueFactory);
				result = orAdd;
			}
			return result;
		}

		// Token: 0x06000180 RID: 384 RVA: 0x00008098 File Offset: 0x00006298
		private static IEnumerable<Type> GetJoinedTypesInternal(string key)
		{
			string[] array = key.Split(new char[]
			{
				'+'
			});
			string text = array[0];
			string text2 = array[1];
			Type type = GraphHelper.GetType(text);
			PXGraph pxgraph = HiddenUtils.CreateInstance(type);
			bool flag = pxgraph == null || string.IsNullOrEmpty(text2);
			IEnumerable<Type> result;
			if (flag)
			{
				result = Enumerable.Empty<Type>();
			}
			else
			{
				PXSelectBase selectFromGraph = ViewUtils.GetSelectFromGraph(pxgraph, text2, true);
				bool flag2 = selectFromGraph == null;
				if (flag2)
				{
					result = Enumerable.Empty<Type>();
				}
				else
				{
					PXView view = selectFromGraph.View;
					Type[] itemTypes = view.GetItemTypes();
					result = itemTypes;
				}
			}
			return result;
		}

		// Token: 0x06000181 RID: 385 RVA: 0x00008128 File Offset: 0x00006328
		private static PXSelectBase GetSelectFromGraph(PXGraph graph, string viewName, bool silent = false)
		{
			PXView view = ViewUtils.GetView(graph, viewName, silent);
			PXViewCollection views = graph.Views;
			Type type = views.GetType();
			MethodInvoker methodInvoker = Reflect.Method(type, "GetExternalMember", FasterflectFlags.InstanceAnyVisibility, new Type[]
			{
				typeof(PXView)
			});
			bool flag = methodInvoker == null;
			PXSelectBase result;
			if (flag)
			{
				if (!silent)
				{
					throw new PXException("Cannot find method 'GetExternalMember' on PXViewCollection '{0}' of graph '{1}'", new object[]
					{
						viewName,
						graph.GetType().Name
					});
				}
				result = null;
			}
			else
			{
				PXSelectBase pxselectBase = (PXSelectBase)methodInvoker(views, new object[]
				{
					view
				});
				bool flag2 = pxselectBase == null;
				if (flag2)
				{
					MemberGetter memberGetter = Reflect.FieldGetter(type, "_Members");
					Dictionary<PXView, PXSelectBase> source = (Dictionary<PXView, PXSelectBase>)memberGetter(views);
					pxselectBase = source.FirstOrDefault((KeyValuePair<PXView, PXSelectBase> kvp) => kvp.Key.Name == viewName).Value;
				}
				result = pxselectBase;
			}
			return result;
		}

		// Token: 0x06000182 RID: 386 RVA: 0x00008228 File Offset: 0x00006428
		public static PXCache GetCache(PXGraph graph, string viewName)
		{
			Type itemType = ViewUtils.GetItemType(graph, viewName);
			bool flag = itemType == null;
			if (flag)
			{
				throw new PXException("Cannot find an item type for a view named '{0}' in graph '{1}'", new object[]
				{
					viewName,
					graph.GetType().Name
				});
			}
			PXCache pxcache = graph.Caches[itemType];
			PXCache pxcache2 = pxcache;
			if (pxcache2 == null)
			{
				throw new PXException("Cannot find a cache for a view named '{0}' in graph '{1}'", new object[]
				{
					viewName,
					graph.GetType().Name
				});
			}
			return pxcache2;
		}

		// Token: 0x06000183 RID: 387 RVA: 0x000082A8 File Offset: 0x000064A8
		public static IList<Type> GetItemTypes(PXView view, object result)
		{
			List<Type> list = view.GetItemTypes().ToList<Type>();
			ICollection<object> collection;
			bool flag;
			if (list[0] == typeof(GenericResult))
			{
				collection = (result as ICollection<object>);
				if (collection != null)
				{
					flag = collection.Any<object>();
					goto IL_38;
				}
			}
			flag = false;
			IL_38:
			bool flag2 = flag;
			if (flag2)
			{
				GenericResult genericResult = (GenericResult)collection.First<object>();
				IDictionary<string, object> values = genericResult.Values;
				IEnumerable<Type> source = values.Select(delegate(KeyValuePair<string, object> vkp)
				{
					object value = vkp.Value;
					return (value != null) ? value.GetType() : null;
				});
				Func<Type, bool> predicate;
				if ((predicate = ViewUtils.<>O.<5>__IsTable) == null)
				{
					predicate = (ViewUtils.<>O.<5>__IsTable = new Func<Type, bool>(ViewUtils.IsTable));
				}
				list = source.Where(predicate).ToList<Type>();
			}
			return list;
		}

		// Token: 0x06000184 RID: 388 RVA: 0x0000835C File Offset: 0x0000655C
		public static bool IsTable(Type type)
		{
			return type != null && type.IsCompatibleWith(typeof(IBqlTable));
		}

		// Token: 0x06000185 RID: 389 RVA: 0x00008384 File Offset: 0x00006584
		public static PXCache GetCache(PXGraph graph, object row)
		{
			return ViewUtils.GetCache(graph, (row != null) ? row.GetType() : null);
		}

		// Token: 0x06000186 RID: 390 RVA: 0x000083A8 File Offset: 0x000065A8
		public static PXCache GetCache(PXGraph graph, Type it)
		{
			return (graph == null || it == null) ? null : graph.Caches[it];
		}

		// Token: 0x06000187 RID: 391 RVA: 0x000083D8 File Offset: 0x000065D8
		public static IList<Type> GetExtensions(PXGraph graph, Type itemType)
		{
			PXCache cache = ViewUtils.GetCache(graph, itemType);
			bool flag = cache == null;
			if (flag)
			{
				throw new PXException("No cache for item type '{0}' in graph '{1}'", new object[]
				{
					itemType.FullName,
					graph.GetType().FullName
				});
			}
			Type[] extensionTypes = cache.GetExtensionTypes();
			List<Type> list = (extensionTypes != null) ? extensionTypes.ToList<Type>() : null;
			return list ?? ViewUtils.NO_TYPES;
		}

		// Token: 0x04000072 RID: 114
		private static readonly List<Type> NO_TYPES = Enumerable.Empty<Type>().ToList<Type>();

		// Token: 0x04000073 RID: 115
		private const string ITERATOR_SUFFIX = "Row";

		// Token: 0x04000074 RID: 116
		private static readonly ConcurrentDictionary<string, IEnumerable<ViewDef>> VIEWS = new ConcurrentDictionary<string, IEnumerable<ViewDef>>();

		// Token: 0x02000085 RID: 133
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x0400017F RID: 383
			public static Func<string, PXGraph, IEnumerable<ViewDef>> <0>__GetViewDefinitionsInternal;

			// Token: 0x04000180 RID: 384
			public static Func<PXView, bool> <1>__IsUsable;

			// Token: 0x04000181 RID: 385
			public static Func<Type, bool> <2>__TakesField;

			// Token: 0x04000182 RID: 386
			public static Func<string, IEnumerable<PXFieldState>> <3>__GetFieldsInternal;

			// Token: 0x04000183 RID: 387
			public static Func<string, IEnumerable<Type>> <4>__GetJoinedTypesInternal;

			// Token: 0x04000184 RID: 388
			public static Func<Type, bool> <5>__IsTable;
		}
	}
}
