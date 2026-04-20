using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Fasterflect;
using PX.Data;
using PX.Objects.CM;
using PX.Objects.Common.Abstractions;

namespace AA.Objects.AL
{
	// Token: 0x020001D5 RID: 469
	public static class ViewUtils
	{
		// Token: 0x06001413 RID: 5139 RVA: 0x00046CFC File Offset: 0x00044EFC
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
				FieldInfo field = AsgardUtils.GetField(graph, name);
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

		// Token: 0x06001414 RID: 5140 RVA: 0x00046D6C File Offset: 0x00044F6C
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

		// Token: 0x06001415 RID: 5141 RVA: 0x00046D9C File Offset: 0x00044F9C
		public static ViewDef GetViewDefinition(string graphName, string viewName)
		{
			Type type = GraphHelper.GetType(graphName);
			return ViewUtils.GetViewDefinition(type, viewName);
		}

		// Token: 0x06001416 RID: 5142 RVA: 0x00046DBC File Offset: 0x00044FBC
		public static ViewDef GetViewDefinition(Type _graphType, string viewName)
		{
			PXGraph graph = HiddenUtils.CreateInstance(_graphType);
			return ViewUtils.GetViewDefinition(graph, viewName);
		}

		// Token: 0x06001417 RID: 5143 RVA: 0x00046DE0 File Offset: 0x00044FE0
		public static ViewDef GetViewDefinition(PXGraph _graph, string viewName)
		{
			return ViewUtils.GetViewDefinitions(_graph, new string[]
			{
				viewName
			}).FirstOrDefault<ViewDef>();
		}

		// Token: 0x06001418 RID: 5144 RVA: 0x00046E0C File Offset: 0x0004500C
		public static IEnumerable<ViewDef> GetViewDefinitions(PXGraph _graph, params string[] onlyViewNames)
		{
			return (from vd in ViewUtils.GetViewDefinitions(_graph)
			where onlyViewNames.Contains(vd.InternalName)
			select vd).ToArray<ViewDef>();
		}

		// Token: 0x06001419 RID: 5145 RVA: 0x00046E4C File Offset: 0x0004504C
		public static IEnumerable<ViewDef> GetViewDefinitions(Type _graphType)
		{
			PXGraph graph = HiddenUtils.CreateInstance(_graphType);
			return ViewUtils.GetViewDefinitions(graph);
		}

		// Token: 0x0600141A RID: 5146 RVA: 0x00046E6C File Offset: 0x0004506C
		public static IEnumerable<ViewDef> GetViewDefinitions(PXGraph _graph)
		{
			bool flag = _graph == null;
			IEnumerable<ViewDef> result;
			if (flag)
			{
				result = Enumerable.Empty<ViewDef>();
			}
			else
			{
				Type type = _graph.GetType();
				string fullName = type.FullName;
				ConcurrentDictionary<string, IEnumerable<ViewDef>> views = ViewUtils.VIEWS;
				string key = fullName;
				Func<string, PXGraph, IEnumerable<ViewDef>> valueFactory;
				if ((valueFactory = ViewUtils.<>O.<0>__GetViewDefinitionsInternal) == null)
				{
					valueFactory = (ViewUtils.<>O.<0>__GetViewDefinitionsInternal = new Func<string, PXGraph, IEnumerable<ViewDef>>(ViewUtils.GetViewDefinitionsInternal));
				}
				IEnumerable<ViewDef> orAdd = views.GetOrAdd<PXGraph>(key, valueFactory, _graph);
				result = orAdd;
			}
			return result;
		}

		// Token: 0x0600141B RID: 5147 RVA: 0x00046ECC File Offset: 0x000450CC
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
				ViewDef item = new ViewDef(_graph, pxView);
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

		// Token: 0x0600141C RID: 5148 RVA: 0x000470DC File Offset: 0x000452DC
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

		// Token: 0x0600141D RID: 5149 RVA: 0x00047180 File Offset: 0x00045380
		public static Dictionary<string, PXView> GetViews(PXGraph graph)
		{
			return new Dictionary<string, PXView>(graph.Views);
		}

		// Token: 0x0600141E RID: 5150 RVA: 0x000471A0 File Offset: 0x000453A0
		public static PXView GetView(PXGraph graph, string viewName, bool silent = false)
		{
			PXView value = ViewUtils.GetViews(graph).FirstOrDefault((KeyValuePair<string, PXView> view) => view.Key == viewName).Value;
			bool flag = value == null && !silent;
			if (flag)
			{
				throw new PXException("Cannot find a view named '{0}' in graph '{1}'", new object[]
				{
					viewName,
					graph.GetType().FullName
				});
			}
			return value;
		}

		// Token: 0x0600141F RID: 5151 RVA: 0x0004721C File Offset: 0x0004541C
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
				Type type = AsgardUtils.FindType(itemTypeName);
				Type type2 = AsgardUtils.FindType(itemTypeName2);
				PXCache pxcache = graph.Caches[type];
				PXCache pxcache2 = graph.Caches[type2];
				result = (pxcache != pxcache2 && ViewUtils.IsParent(pxcache, pxcache2));
			}
			return result;
		}

		// Token: 0x06001420 RID: 5152 RVA: 0x00047290 File Offset: 0x00045490
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

		// Token: 0x06001421 RID: 5153 RVA: 0x0004736C File Offset: 0x0004556C
		public static IViewResult GetViewRow(PXGraph docGraph, ViewDef viewDef)
		{
			return new ViewResult(viewDef, docGraph);
		}

		// Token: 0x06001422 RID: 5154 RVA: 0x00047388 File Offset: 0x00045588
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

		// Token: 0x06001423 RID: 5155 RVA: 0x000473EC File Offset: 0x000455EC
		public static void TryRedirect(string graphTypeName, string keys)
		{
			Type type = GraphHelper.GetType(graphTypeName);
			PXGraph pxgraph = HiddenUtils.CreateInstance(type);
			object document = ViewUtils.SearchSpecificDocument(pxgraph, keys);
			ViewUtils.SetDocumentCurrent(pxgraph, document);
			PXRedirectHelper.TryRedirect(pxgraph, 3);
		}

		// Token: 0x06001424 RID: 5156 RVA: 0x00047420 File Offset: 0x00045620
		public static PXGraph GetGraph(string graphTypeName)
		{
			Type type = GraphHelper.GetType(graphTypeName);
			return HiddenUtils.CreateInstance(type);
		}

		// Token: 0x06001425 RID: 5157 RVA: 0x00047444 File Offset: 0x00045644
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

		// Token: 0x06001426 RID: 5158 RVA: 0x0004747C File Offset: 0x0004567C
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

		// Token: 0x06001427 RID: 5159 RVA: 0x00047504 File Offset: 0x00045704
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

		// Token: 0x06001428 RID: 5160 RVA: 0x0004753C File Offset: 0x0004573C
		public static PXEntryStatus GetStatus(PXGraph graph, string viewName)
		{
			PXCache cache = ViewUtils.GetCache(graph, viewName);
			bool flag = cache.Current != null;
			PXEntryStatus result;
			if (flag)
			{
				result = cache.GetStatus(cache.Current);
			}
			else
			{
				result = 0;
			}
			return result;
		}

		// Token: 0x06001429 RID: 5161 RVA: 0x00047574 File Offset: 0x00045774
		public static string[] GetFieldNames(PXGraph graph, string viewName)
		{
			return graph.GetFieldNames(viewName);
		}

		// Token: 0x0600142A RID: 5162 RVA: 0x00047590 File Offset: 0x00045790
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

		// Token: 0x0600142B RID: 5163 RVA: 0x000475C8 File Offset: 0x000457C8
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

		// Token: 0x0600142C RID: 5164 RVA: 0x0004774C File Offset: 0x0004594C
		private static bool TakesField(Type type)
		{
			return type.IsCompatibleWith(typeof(IBqlField));
		}

		// Token: 0x0600142D RID: 5165 RVA: 0x00047770 File Offset: 0x00045970
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

		// Token: 0x0600142E RID: 5166 RVA: 0x00047928 File Offset: 0x00045B28
		public static string GetRowName(ViewDef view, Type itemType = null, bool shortSuffix = true)
		{
			return view.Detail.GetValueOrDefault() ? ViewUtils.GetIteratorName(view, itemType, shortSuffix) : ViewUtils.GetViewName(view, itemType, shortSuffix);
		}

		// Token: 0x0600142F RID: 5167 RVA: 0x0004795C File Offset: 0x00045B5C
		public static string GetRowName(ViewDef view, string alias)
		{
			return view.Detail.GetValueOrDefault() ? ViewUtils.GetIteratorName(view, alias) : ViewUtils.GetViewName(view, alias);
		}

		// Token: 0x06001430 RID: 5168 RVA: 0x00047990 File Offset: 0x00045B90
		public static string GetViewName(ViewDef view, Type itemType = null, bool shortSuffix = true)
		{
			return ViewUtils.GetViewName(view.InternalName, view.ItemTypeName, itemType, shortSuffix);
		}

		// Token: 0x06001431 RID: 5169 RVA: 0x000479B8 File Offset: 0x00045BB8
		public static string GetViewName(string internalName, string viewItemType = null, Type itemType = null, bool shortSuffix = true)
		{
			bool flag = string.IsNullOrEmpty(internalName);
			string result;
			if (flag)
			{
				result = null;
			}
			else
			{
				result = internalName + ViewUtils.GetSuffix(viewItemType, itemType, shortSuffix);
			}
			return result;
		}

		// Token: 0x06001432 RID: 5170 RVA: 0x000479EC File Offset: 0x00045BEC
		public static string GetViewName(ViewDef view, string suffix)
		{
			string internalName = view.InternalName;
			bool flag = string.IsNullOrEmpty(internalName);
			string result;
			if (flag)
			{
				result = null;
			}
			else
			{
				result = internalName + ViewUtils.GetSuffix(suffix);
			}
			return result;
		}

		// Token: 0x06001433 RID: 5171 RVA: 0x00047A20 File Offset: 0x00045C20
		public static string GetIteratorName(ViewDef view, Type itemType = null, bool shortSuffix = true)
		{
			string viewName = ViewUtils.GetViewName(view, null, true);
			return viewName + "Row" + ViewUtils.GetSuffix(view.ItemTypeName, itemType, shortSuffix);
		}

		// Token: 0x06001434 RID: 5172 RVA: 0x00047A58 File Offset: 0x00045C58
		public static string GetIteratorName(ViewDef view, string alias)
		{
			string viewName = ViewUtils.GetViewName(view, null, true);
			return viewName + "Row" + ViewUtils.GetSuffix(alias);
		}

		// Token: 0x06001435 RID: 5173 RVA: 0x00047A88 File Offset: 0x00045C88
		private static string GetSuffix(string viewItemType, Type itemType, bool shortSuffix = true)
		{
			string text = (itemType != null) ? itemType.FullName : null;
			return (itemType == null || text.Equals(viewItemType)) ? "" : (shortSuffix ? ViewUtils.GetSuffix(itemType.Name) : ViewUtils.GetLongSuffix(text));
		}

		// Token: 0x06001436 RID: 5174 RVA: 0x00047AD8 File Offset: 0x00045CD8
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

		// Token: 0x06001437 RID: 5175 RVA: 0x00047B34 File Offset: 0x00045D34
		private static string GetSuffix(string suffix)
		{
			bool flag = string.IsNullOrEmpty(suffix);
			string result;
			if (flag)
			{
				result = "";
			}
			else
			{
				result = "As" + suffix;
			}
			return result;
		}

		// Token: 0x06001438 RID: 5176 RVA: 0x00047B64 File Offset: 0x00045D64
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

		// Token: 0x06001439 RID: 5177 RVA: 0x00047BD0 File Offset: 0x00045DD0
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

		// Token: 0x0600143A RID: 5178 RVA: 0x00047C50 File Offset: 0x00045E50
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
				bool flag2 = selectFromGraph == null;
				result = (!flag2 && selectFromGraph.GetType().Name.Contains("PXSelectJoin"));
			}
			return result;
		}

		// Token: 0x0600143B RID: 5179 RVA: 0x00047CA0 File Offset: 0x00045EA0
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

		// Token: 0x0600143C RID: 5180 RVA: 0x00047D08 File Offset: 0x00045F08
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

		// Token: 0x0600143D RID: 5181 RVA: 0x00047D98 File Offset: 0x00045F98
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

		// Token: 0x0600143E RID: 5182 RVA: 0x00047EA0 File Offset: 0x000460A0
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
			bool flag2 = pxcache == null;
			if (flag2)
			{
				throw new PXException("Cannot find a cache for a view named '{0}' in graph '{1}'", new object[]
				{
					viewName,
					graph.GetType().Name
				});
			}
			return pxcache;
		}

		// Token: 0x0600143F RID: 5183 RVA: 0x00047F28 File Offset: 0x00046128
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

		// Token: 0x06001440 RID: 5184 RVA: 0x00047FDC File Offset: 0x000461DC
		public static bool IsTable(Type type)
		{
			return type != null && type.IsCompatibleWith(typeof(IBqlTable));
		}

		// Token: 0x06001441 RID: 5185 RVA: 0x00048004 File Offset: 0x00046204
		public static PXCache GetCache(PXGraph graph, object row)
		{
			return ViewUtils.GetCache(graph, (row != null) ? row.GetType() : null);
		}

		// Token: 0x06001442 RID: 5186 RVA: 0x00048028 File Offset: 0x00046228
		public static PXCache GetCache(PXGraph graph, Type it)
		{
			bool flag = graph == null || it == null;
			PXCache result;
			if (flag)
			{
				result = null;
			}
			else
			{
				result = graph.Caches[it];
			}
			return result;
		}

		// Token: 0x06001443 RID: 5187 RVA: 0x0004805C File Offset: 0x0004625C
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

		// Token: 0x040008AE RID: 2222
		private static readonly List<Type> NO_TYPES = Enumerable.Empty<Type>().ToList<Type>();

		// Token: 0x040008AF RID: 2223
		private const string ITERATOR_SUFFIX = "Row";

		// Token: 0x040008B0 RID: 2224
		private static readonly ConcurrentDictionary<string, IEnumerable<ViewDef>> VIEWS = new ConcurrentDictionary<string, IEnumerable<ViewDef>>();

		// Token: 0x02000931 RID: 2353
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x0400126F RID: 4719
			public static Func<string, PXGraph, IEnumerable<ViewDef>> <0>__GetViewDefinitionsInternal;

			// Token: 0x04001270 RID: 4720
			public static Func<PXView, bool> <1>__IsUsable;

			// Token: 0x04001271 RID: 4721
			public static Func<Type, bool> <2>__TakesField;

			// Token: 0x04001272 RID: 4722
			public static Func<string, IEnumerable<PXFieldState>> <3>__GetFieldsInternal;

			// Token: 0x04001273 RID: 4723
			public static Func<string, IEnumerable<Type>> <4>__GetJoinedTypesInternal;

			// Token: 0x04001274 RID: 4724
			public static Func<Type, bool> <5>__IsTable;
		}
	}
}
