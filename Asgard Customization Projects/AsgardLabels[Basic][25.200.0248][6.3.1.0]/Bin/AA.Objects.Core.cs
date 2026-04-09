public class ALAbstractEnumIntListAttribute:ALAbstractIntListAttribute, _Attribute, IPXFieldSelectingSubscriber, IPXLocalizableList
	Boolean IsDirty;
	Boolean IsLocalizable;
	Boolean ExclusiveValues;
	Dictionary<Int32,String> ValueLabelDic;
	PXAttributeLevel AttributeLevel;
	Type BqlTable;
	Type CacheExtensionType;
	String FieldName;
	Int32 FieldOrdinal;
	Object TypeId;
	ValueTuple`2[] GetTuples();
	E[] CheckValues(E[] values);
	String GetDescription(E item);
	ValueTuple`2[] FindTuples(Type CodeType, Type DescType, Boolean sortByDesc);
	IEnumerable<FieldInfo> FindConstants(Type type);
	void SetList(PXCache cache, Object data, Int32[] allowedValues, String[] allowedLabels);
	void SetList(PXCache cache, Object data, ValueTuple`2[] valuesToLabels);
	void CacheAttached(PXCache sender);
	void FieldSelecting(PXCache sender, PXFieldSelectingEventArgs e);
	Tuple<Int32,String> Pair(Int32 value, String label);
	Int32 GetHashCode();
	Boolean Equals(Object obj);
	PXEventSubscriberAttribute Clone(PXAttributeLevel attributeLevel);
	Type GetBaseCacheWithTableAttr(Type cache);
	void SetBqlTable(Type bqlTable);
	void InjectAttributeDependencies(PXCache cache);
	void InvokeCacheAttached(PXCache cache);
	void GetSubscriber(List<ISubscriber> subscribers);
	T CreateInstance(Object[] constructorArgs);
	PXEventSubscriberAttribute CreateInstance(Type t, Object[] constructorArgs);
	String ToString();
	Attribute[] GetCustomAttributes(MemberInfo element, Type type);
	Attribute[] GetCustomAttributes(MemberInfo element, Type type, Boolean inherit);
	Attribute[] GetCustomAttributes(MemberInfo element);
	Attribute[] GetCustomAttributes(MemberInfo element, Boolean inherit);
	Boolean IsDefined(MemberInfo element, Type attributeType);
	Boolean IsDefined(MemberInfo element, Type attributeType, Boolean inherit);
	Attribute GetCustomAttribute(MemberInfo element, Type attributeType);
	Attribute GetCustomAttribute(MemberInfo element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(ParameterInfo element);
	Attribute[] GetCustomAttributes(ParameterInfo element, Type attributeType);
	Attribute[] GetCustomAttributes(ParameterInfo element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(ParameterInfo element, Boolean inherit);
	Boolean IsDefined(ParameterInfo element, Type attributeType);
	Boolean IsDefined(ParameterInfo element, Type attributeType, Boolean inherit);
	Attribute GetCustomAttribute(ParameterInfo element, Type attributeType);
	Attribute GetCustomAttribute(ParameterInfo element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(Module element, Type attributeType);
	Attribute[] GetCustomAttributes(Module element);
	Attribute[] GetCustomAttributes(Module element, Boolean inherit);
	Attribute[] GetCustomAttributes(Module element, Type attributeType, Boolean inherit);
	Boolean IsDefined(Module element, Type attributeType);
	Boolean IsDefined(Module element, Type attributeType, Boolean inherit);
	Attribute GetCustomAttribute(Module element, Type attributeType);
	Attribute GetCustomAttribute(Module element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(Assembly element, Type attributeType);
	Attribute[] GetCustomAttributes(Assembly element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(Assembly element);
	Attribute[] GetCustomAttributes(Assembly element, Boolean inherit);
	Boolean IsDefined(Assembly element, Type attributeType);
	Boolean IsDefined(Assembly element, Type attributeType, Boolean inherit);
	Attribute GetCustomAttribute(Assembly element, Type attributeType);
	Attribute GetCustomAttribute(Assembly element, Type attributeType, Boolean inherit);
	Boolean Match(Object obj);
	Boolean IsDefaultAttribute();
	void System.Runtime.InteropServices._Attribute.GetTypeInfoCount(UInt32& pcTInfo);
	void System.Runtime.InteropServices._Attribute.GetTypeInfo(UInt32 iTInfo, UInt32 lcid, IntPtr ppTInfo);
	void System.Runtime.InteropServices._Attribute.GetIDsOfNames(Guid& riid, IntPtr rgszNames, UInt32 cNames, UInt32 lcid, IntPtr rgDispId);
	void System.Runtime.InteropServices._Attribute.Invoke(UInt32 dispIdMember, Guid& riid, UInt32 lcid, Int16 wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr);



public class ALAbstractIntListAttribute:PXIntListAttribute, _Attribute, IPXFieldSelectingSubscriber, IPXLocalizableList
	Int32[] EMPTY_INT;
	String[] EMPTY;
	Boolean IsDirty;
	Boolean IsLocalizable;
	Boolean ExclusiveValues;
	Dictionary<Int32,String> ValueLabelDic;
	PXAttributeLevel AttributeLevel;
	Type BqlTable;
	Type CacheExtensionType;
	String FieldName;
	Int32 FieldOrdinal;
	Object TypeId;
	void CheckValuesAndLabels(Int32[] values, String[] labels);
	void CreateNeutralLabels();
	ValueTuple`2[] GetTuples();
	ValueTuple`2[] FindTuples(Type CodeType, Type DescType, Boolean sortByDesc);
	IEnumerable<FieldInfo> FindConstants(Type type);
	void SetList(PXCache cache, Object data, Int32[] allowedValues, String[] allowedLabels);
	void SetList(PXCache cache, Object data, ValueTuple`2[] valuesToLabels);
	void CacheAttached(PXCache sender);
	void FieldSelecting(PXCache sender, PXFieldSelectingEventArgs e);
	Tuple<Int32,String> Pair(Int32 value, String label);
	Int32 GetHashCode();
	Boolean Equals(Object obj);
	PXEventSubscriberAttribute Clone(PXAttributeLevel attributeLevel);
	Type GetBaseCacheWithTableAttr(Type cache);
	void SetBqlTable(Type bqlTable);
	void InjectAttributeDependencies(PXCache cache);
	void InvokeCacheAttached(PXCache cache);
	void GetSubscriber(List<ISubscriber> subscribers);
	T CreateInstance(Object[] constructorArgs);
	PXEventSubscriberAttribute CreateInstance(Type t, Object[] constructorArgs);
	String ToString();
	Attribute[] GetCustomAttributes(MemberInfo element, Type type);
	Attribute[] GetCustomAttributes(MemberInfo element, Type type, Boolean inherit);
	Attribute[] GetCustomAttributes(MemberInfo element);
	Attribute[] GetCustomAttributes(MemberInfo element, Boolean inherit);
	Boolean IsDefined(MemberInfo element, Type attributeType);
	Boolean IsDefined(MemberInfo element, Type attributeType, Boolean inherit);
	Attribute GetCustomAttribute(MemberInfo element, Type attributeType);
	Attribute GetCustomAttribute(MemberInfo element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(ParameterInfo element);
	Attribute[] GetCustomAttributes(ParameterInfo element, Type attributeType);
	Attribute[] GetCustomAttributes(ParameterInfo element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(ParameterInfo element, Boolean inherit);
	Boolean IsDefined(ParameterInfo element, Type attributeType);
	Boolean IsDefined(ParameterInfo element, Type attributeType, Boolean inherit);
	Attribute GetCustomAttribute(ParameterInfo element, Type attributeType);
	Attribute GetCustomAttribute(ParameterInfo element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(Module element, Type attributeType);
	Attribute[] GetCustomAttributes(Module element);
	Attribute[] GetCustomAttributes(Module element, Boolean inherit);
	Attribute[] GetCustomAttributes(Module element, Type attributeType, Boolean inherit);
	Boolean IsDefined(Module element, Type attributeType);
	Boolean IsDefined(Module element, Type attributeType, Boolean inherit);
	Attribute GetCustomAttribute(Module element, Type attributeType);
	Attribute GetCustomAttribute(Module element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(Assembly element, Type attributeType);
	Attribute[] GetCustomAttributes(Assembly element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(Assembly element);
	Attribute[] GetCustomAttributes(Assembly element, Boolean inherit);
	Boolean IsDefined(Assembly element, Type attributeType);
	Boolean IsDefined(Assembly element, Type attributeType, Boolean inherit);
	Attribute GetCustomAttribute(Assembly element, Type attributeType);
	Attribute GetCustomAttribute(Assembly element, Type attributeType, Boolean inherit);
	Boolean Match(Object obj);
	Boolean IsDefaultAttribute();
	void System.Runtime.InteropServices._Attribute.GetTypeInfoCount(UInt32& pcTInfo);
	void System.Runtime.InteropServices._Attribute.GetTypeInfo(UInt32 iTInfo, UInt32 lcid, IntPtr ppTInfo);
	void System.Runtime.InteropServices._Attribute.GetIDsOfNames(Guid& riid, IntPtr rgszNames, UInt32 cNames, UInt32 lcid, IntPtr rgDispId);
	void System.Runtime.InteropServices._Attribute.Invoke(UInt32 dispIdMember, Guid& riid, UInt32 lcid, Int16 wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr);



public class ALAbstractMultiEnumStringListAttribute:ALAbstractStringListAttribute, _Attribute, IPXFieldSelectingSubscriber, IPXLocalizableList
	Boolean IsDirty;
	Boolean IsLocalizable;
	Boolean IsLocalized;
	Boolean SortByValues;
	Boolean MultiSelect;
	Boolean ExclusiveValues;
	Type BqlField;
	Dictionary<String,String> ValueLabelDic;
	PXAttributeLevel AttributeLevel;
	Type BqlTable;
	Type CacheExtensionType;
	String FieldName;
	Int32 FieldOrdinal;
	Object TypeId;
	ValueTuple`2[] GetTuples();
	String GetDescription(E item);
	String[] GetAllowedValues(PXCache cache);
	String[] SplitMultiSelectValues(String values);
	String GetLocalizedLabel(PXCache cache, Object row);
	String GetLocalizedLabel(PXCache cache, Object row, String value);
	void SetLocalizable(PXCache cache, Object data, Boolean isLocalizable);
	void SetList(PXCache cache, Object data, PXStringListAttribute listSource);
	void SetList(PXCache cache, Object data, String[] allowedValues, String[] allowedLabels);
	void SetList(PXCache cache, Object data, Tuple`2[] valuesToLabels);
	void SetList(PXCache cache, Object data, ValueTuple`2[] valuesToLabels);
	void SetList(PXCache cache, Object data, String field, String[] allowedValues, String[] allowedLabels);
	void SetList(PXCache cache, Object data, String field, PXStringListAttribute listSource);
	void SetList(PXCache cache, Object data, String field, Tuple`2[] valuesToLabels);
	void SetList(PXCache cache, Object data, String field, ValueTuple`2[] valuesToLabels);
	void SetListInternal(IEnumerable<PXStringListAttribute> attributes, String[] allowedValues, String[] allowedLabels, PXCache cache);
	void AppendList(PXCache cache, Object data, ValueTuple`2[] valuesToLabels);
	void AppendList(PXCache cache, Object data, String[] allowedValues, String[] allowedLabels);
	void AppendList(PXCache cache, Object data, String field, String[] allowedValues, String[] allowedLabels);
	void SetExclusiveValues(PXCache cache, Object data, Boolean exclusiveValues);
	void SetExclusiveValues(PXCache cache, Object data, String field, Boolean exclusiveValues);
	void FieldSelecting(PXCache sender, PXFieldSelectingEventArgs e);
	void OrderByCommandPreparing(PXCache sender, PXCommandPreparingEventArgs e);
	void CacheAttached(PXCache sender);
	void MultiSelectFieldUpdating(PXCache sender, PXFieldUpdatingEventArgs e);
	void RemoveDisabledValues(String cacheName);
	void TryLocalize(PXCache sender);
	void RipDynamicLabels(String[] dynamicAllowedLabels, PXCache sender);
	Tuple<String,String> Pair(String value, String label);
	Int32 GetHashCode();
	Boolean Equals(Object obj);
	PXEventSubscriberAttribute Clone(PXAttributeLevel attributeLevel);
	Type GetBaseCacheWithTableAttr(Type cache);
	void SetBqlTable(Type bqlTable);
	void InjectAttributeDependencies(PXCache cache);
	void InvokeCacheAttached(PXCache cache);
	void GetSubscriber(List<ISubscriber> subscribers);
	T CreateInstance(Object[] constructorArgs);
	PXEventSubscriberAttribute CreateInstance(Type t, Object[] constructorArgs);
	String ToString();
	Attribute[] GetCustomAttributes(MemberInfo element, Type type);
	Attribute[] GetCustomAttributes(MemberInfo element, Type type, Boolean inherit);
	Attribute[] GetCustomAttributes(MemberInfo element);
	Attribute[] GetCustomAttributes(MemberInfo element, Boolean inherit);
	Boolean IsDefined(MemberInfo element, Type attributeType);
	Boolean IsDefined(MemberInfo element, Type attributeType, Boolean inherit);
	Attribute GetCustomAttribute(MemberInfo element, Type attributeType);
	Attribute GetCustomAttribute(MemberInfo element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(ParameterInfo element);
	Attribute[] GetCustomAttributes(ParameterInfo element, Type attributeType);
	Attribute[] GetCustomAttributes(ParameterInfo element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(ParameterInfo element, Boolean inherit);
	Boolean IsDefined(ParameterInfo element, Type attributeType);
	Boolean IsDefined(ParameterInfo element, Type attributeType, Boolean inherit);
	Attribute GetCustomAttribute(ParameterInfo element, Type attributeType);
	Attribute GetCustomAttribute(ParameterInfo element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(Module element, Type attributeType);
	Attribute[] GetCustomAttributes(Module element);
	Attribute[] GetCustomAttributes(Module element, Boolean inherit);
	Attribute[] GetCustomAttributes(Module element, Type attributeType, Boolean inherit);
	Boolean IsDefined(Module element, Type attributeType);
	Boolean IsDefined(Module element, Type attributeType, Boolean inherit);
	Attribute GetCustomAttribute(Module element, Type attributeType);
	Attribute GetCustomAttribute(Module element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(Assembly element, Type attributeType);
	Attribute[] GetCustomAttributes(Assembly element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(Assembly element);
	Attribute[] GetCustomAttributes(Assembly element, Boolean inherit);
	Boolean IsDefined(Assembly element, Type attributeType);
	Boolean IsDefined(Assembly element, Type attributeType, Boolean inherit);
	Attribute GetCustomAttribute(Assembly element, Type attributeType);
	Attribute GetCustomAttribute(Assembly element, Type attributeType, Boolean inherit);
	Boolean Match(Object obj);
	Boolean IsDefaultAttribute();
	void System.Runtime.InteropServices._Attribute.GetTypeInfoCount(UInt32& pcTInfo);
	void System.Runtime.InteropServices._Attribute.GetTypeInfo(UInt32 iTInfo, UInt32 lcid, IntPtr ppTInfo);
	void System.Runtime.InteropServices._Attribute.GetIDsOfNames(Guid& riid, IntPtr rgszNames, UInt32 cNames, UInt32 lcid, IntPtr rgDispId);
	void System.Runtime.InteropServices._Attribute.Invoke(UInt32 dispIdMember, Guid& riid, UInt32 lcid, Int16 wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr);



public class ALAbstractStringListAttribute:PXStringListAttribute, _Attribute, IPXFieldSelectingSubscriber, IPXLocalizableList
	String[] EMPTY;
	Boolean IsDirty;
	Boolean IsLocalizable;
	Boolean IsLocalized;
	Boolean SortByValues;
	Boolean MultiSelect;
	Boolean ExclusiveValues;
	Type BqlField;
	Dictionary<String,String> ValueLabelDic;
	PXAttributeLevel AttributeLevel;
	Type BqlTable;
	Type CacheExtensionType;
	String FieldName;
	Int32 FieldOrdinal;
	Object TypeId;
	void CheckValuesAndLabels(String[] values, String[] labels);
	void CreateNeutralLabels();
	ValueTuple`2[] GetTuples();
	String[] GetAllowedValues(PXCache cache);
	String[] SplitMultiSelectValues(String values);
	String GetLocalizedLabel(PXCache cache, Object row);
	String GetLocalizedLabel(PXCache cache, Object row, String value);
	void SetLocalizable(PXCache cache, Object data, Boolean isLocalizable);
	void SetList(PXCache cache, Object data, PXStringListAttribute listSource);
	void SetList(PXCache cache, Object data, String[] allowedValues, String[] allowedLabels);
	void SetList(PXCache cache, Object data, Tuple`2[] valuesToLabels);
	void SetList(PXCache cache, Object data, ValueTuple`2[] valuesToLabels);
	void SetList(PXCache cache, Object data, String field, String[] allowedValues, String[] allowedLabels);
	void SetList(PXCache cache, Object data, String field, PXStringListAttribute listSource);
	void SetList(PXCache cache, Object data, String field, Tuple`2[] valuesToLabels);
	void SetList(PXCache cache, Object data, String field, ValueTuple`2[] valuesToLabels);
	void SetListInternal(IEnumerable<PXStringListAttribute> attributes, String[] allowedValues, String[] allowedLabels, PXCache cache);
	void AppendList(PXCache cache, Object data, ValueTuple`2[] valuesToLabels);
	void AppendList(PXCache cache, Object data, String[] allowedValues, String[] allowedLabels);
	void AppendList(PXCache cache, Object data, String field, String[] allowedValues, String[] allowedLabels);
	void SetExclusiveValues(PXCache cache, Object data, Boolean exclusiveValues);
	void SetExclusiveValues(PXCache cache, Object data, String field, Boolean exclusiveValues);
	void FieldSelecting(PXCache sender, PXFieldSelectingEventArgs e);
	void OrderByCommandPreparing(PXCache sender, PXCommandPreparingEventArgs e);
	void CacheAttached(PXCache sender);
	void MultiSelectFieldUpdating(PXCache sender, PXFieldUpdatingEventArgs e);
	void RemoveDisabledValues(String cacheName);
	void TryLocalize(PXCache sender);
	void RipDynamicLabels(String[] dynamicAllowedLabels, PXCache sender);
	Tuple<String,String> Pair(String value, String label);
	Int32 GetHashCode();
	Boolean Equals(Object obj);
	PXEventSubscriberAttribute Clone(PXAttributeLevel attributeLevel);
	Type GetBaseCacheWithTableAttr(Type cache);
	void SetBqlTable(Type bqlTable);
	void InjectAttributeDependencies(PXCache cache);
	void InvokeCacheAttached(PXCache cache);
	void GetSubscriber(List<ISubscriber> subscribers);
	T CreateInstance(Object[] constructorArgs);
	PXEventSubscriberAttribute CreateInstance(Type t, Object[] constructorArgs);
	String ToString();
	Attribute[] GetCustomAttributes(MemberInfo element, Type type);
	Attribute[] GetCustomAttributes(MemberInfo element, Type type, Boolean inherit);
	Attribute[] GetCustomAttributes(MemberInfo element);
	Attribute[] GetCustomAttributes(MemberInfo element, Boolean inherit);
	Boolean IsDefined(MemberInfo element, Type attributeType);
	Boolean IsDefined(MemberInfo element, Type attributeType, Boolean inherit);
	Attribute GetCustomAttribute(MemberInfo element, Type attributeType);
	Attribute GetCustomAttribute(MemberInfo element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(ParameterInfo element);
	Attribute[] GetCustomAttributes(ParameterInfo element, Type attributeType);
	Attribute[] GetCustomAttributes(ParameterInfo element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(ParameterInfo element, Boolean inherit);
	Boolean IsDefined(ParameterInfo element, Type attributeType);
	Boolean IsDefined(ParameterInfo element, Type attributeType, Boolean inherit);
	Attribute GetCustomAttribute(ParameterInfo element, Type attributeType);
	Attribute GetCustomAttribute(ParameterInfo element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(Module element, Type attributeType);
	Attribute[] GetCustomAttributes(Module element);
	Attribute[] GetCustomAttributes(Module element, Boolean inherit);
	Attribute[] GetCustomAttributes(Module element, Type attributeType, Boolean inherit);
	Boolean IsDefined(Module element, Type attributeType);
	Boolean IsDefined(Module element, Type attributeType, Boolean inherit);
	Attribute GetCustomAttribute(Module element, Type attributeType);
	Attribute GetCustomAttribute(Module element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(Assembly element, Type attributeType);
	Attribute[] GetCustomAttributes(Assembly element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(Assembly element);
	Attribute[] GetCustomAttributes(Assembly element, Boolean inherit);
	Boolean IsDefined(Assembly element, Type attributeType);
	Boolean IsDefined(Assembly element, Type attributeType, Boolean inherit);
	Attribute GetCustomAttribute(Assembly element, Type attributeType);
	Attribute GetCustomAttribute(Assembly element, Type attributeType, Boolean inherit);
	Boolean Match(Object obj);
	Boolean IsDefaultAttribute();
	void System.Runtime.InteropServices._Attribute.GetTypeInfoCount(UInt32& pcTInfo);
	void System.Runtime.InteropServices._Attribute.GetTypeInfo(UInt32 iTInfo, UInt32 lcid, IntPtr ppTInfo);
	void System.Runtime.InteropServices._Attribute.GetIDsOfNames(Guid& riid, IntPtr rgszNames, UInt32 cNames, UInt32 lcid, IntPtr rgDispId);
	void System.Runtime.InteropServices._Attribute.Invoke(UInt32 dispIdMember, Guid& riid, UInt32 lcid, Int16 wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr);



public class ALActiveAttribute:ALAggregateAttribute, _Attribute, IPXInterfaceField, IPXCommandPreparingSubscriber, IPXRowSelectingSubscriber
	Boolean IsDirty;
	Type BqlField;
	Boolean CacheGlobal;
	Object Constant;
	Type DescriptionField;
	Type SubstituteKey;
	Boolean DirtyRead;
	String DisplayName;
	Boolean Enabled;
	Boolean IsReadOnly;
	PXErrorHandling ErrorHandling;
	PXErrorLevel ErrorLevel;
	String ErrorText;
	Object ErrorValue;
	String FieldClass;
	Boolean Filterable;
	Boolean IsDBField;
	Boolean IsFixed;
	Boolean IsKey;
	PXCacheRights MapEnableRights;
	PXCacheRights MapViewRights;
	PXPersistingCheck PersistingCheck;
	Boolean Required;
	Int32 TabOrder;
	Boolean ValidateValue;
	Boolean SuppressVerify;
	Boolean ViewRights;
	PXUIVisibility Visibility;
	Boolean Visible;
	Type BqlTable;
	String FieldName;
	Int32 FieldOrdinal;
	Type CacheExtensionType;
	PXAttributeLevel AttributeLevel;
	Object TypeId;
	void CacheAttached(PXCache sender);
	void OnRowSelected(PXCache sender, PXRowSelectedEventArgs e);
	void CommandPreparing(PXCache sender, PXCommandPreparingEventArgs e);
	void ForceEnabled();
	void GetSubscriber(List<ISubscriber> subscribers);
	void Initialize();
	void RowSelecting(PXCache sender, PXRowSelectingEventArgs e);
	PXEventSubscriberAttribute[] GetAttributes();
	PXEventSubscriberAttribute[] GetAggregatedAttributes();
	T GetAttribute();
	PXEventSubscriberAttribute Clone(PXAttributeLevel attributeLevel);
	void InjectAttributeDependencies(PXCache cache);
	void SetBqlTable(Type bqlTable);
	Boolean ChildrenAttributesComeFirstFor();
	Int32 GetHashCode();
	Boolean Equals(Object obj);
	Type GetBaseCacheWithTableAttr(Type cache);
	void InvokeCacheAttached(PXCache cache);
	T CreateInstance(Object[] constructorArgs);
	PXEventSubscriberAttribute CreateInstance(Type t, Object[] constructorArgs);
	String ToString();
	Attribute[] GetCustomAttributes(MemberInfo element, Type type);
	Attribute[] GetCustomAttributes(MemberInfo element, Type type, Boolean inherit);
	Attribute[] GetCustomAttributes(MemberInfo element);
	Attribute[] GetCustomAttributes(MemberInfo element, Boolean inherit);
	Boolean IsDefined(MemberInfo element, Type attributeType);
	Boolean IsDefined(MemberInfo element, Type attributeType, Boolean inherit);
	Attribute GetCustomAttribute(MemberInfo element, Type attributeType);
	Attribute GetCustomAttribute(MemberInfo element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(ParameterInfo element);
	Attribute[] GetCustomAttributes(ParameterInfo element, Type attributeType);
	Attribute[] GetCustomAttributes(ParameterInfo element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(ParameterInfo element, Boolean inherit);
	Boolean IsDefined(ParameterInfo element, Type attributeType);
	Boolean IsDefined(ParameterInfo element, Type attributeType, Boolean inherit);
	Attribute GetCustomAttribute(ParameterInfo element, Type attributeType);
	Attribute GetCustomAttribute(ParameterInfo element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(Module element, Type attributeType);
	Attribute[] GetCustomAttributes(Module element);
	Attribute[] GetCustomAttributes(Module element, Boolean inherit);
	Attribute[] GetCustomAttributes(Module element, Type attributeType, Boolean inherit);
	Boolean IsDefined(Module element, Type attributeType);
	Boolean IsDefined(Module element, Type attributeType, Boolean inherit);
	Attribute GetCustomAttribute(Module element, Type attributeType);
	Attribute GetCustomAttribute(Module element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(Assembly element, Type attributeType);
	Attribute[] GetCustomAttributes(Assembly element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(Assembly element);
	Attribute[] GetCustomAttributes(Assembly element, Boolean inherit);
	Boolean IsDefined(Assembly element, Type attributeType);
	Boolean IsDefined(Assembly element, Type attributeType, Boolean inherit);
	Attribute GetCustomAttribute(Assembly element, Type attributeType);
	Attribute GetCustomAttribute(Assembly element, Type attributeType, Boolean inherit);
	Boolean Match(Object obj);
	Boolean IsDefaultAttribute();
	void System.Runtime.InteropServices._Attribute.GetTypeInfoCount(UInt32& pcTInfo);
	void System.Runtime.InteropServices._Attribute.GetTypeInfo(UInt32 iTInfo, UInt32 lcid, IntPtr ppTInfo);
	void System.Runtime.InteropServices._Attribute.GetIDsOfNames(Guid& riid, IntPtr rgszNames, UInt32 cNames, UInt32 lcid, IntPtr rgDispId);
	void System.Runtime.InteropServices._Attribute.Invoke(UInt32 dispIdMember, Guid& riid, UInt32 lcid, Int16 wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr);



public class ALAggregateAttribute:PXAggregateAttribute, _Attribute, IPXInterfaceField, IPXCommandPreparingSubscriber, IPXRowSelectingSubscriber
	Boolean IsDirty;
	Type BqlField;
	Boolean CacheGlobal;
	Object Constant;
	Type DescriptionField;
	Type SubstituteKey;
	Boolean DirtyRead;
	String DisplayName;
	Boolean Enabled;
	Boolean IsReadOnly;
	PXErrorHandling ErrorHandling;
	PXErrorLevel ErrorLevel;
	String ErrorText;
	Object ErrorValue;
	String FieldClass;
	Boolean Filterable;
	Boolean IsDBField;
	Boolean IsFixed;
	Boolean IsKey;
	PXCacheRights MapEnableRights;
	PXCacheRights MapViewRights;
	PXPersistingCheck PersistingCheck;
	Boolean Required;
	Int32 TabOrder;
	Boolean ValidateValue;
	Boolean SuppressVerify;
	Boolean ViewRights;
	PXUIVisibility Visibility;
	Boolean Visible;
	Type BqlTable;
	String FieldName;
	Int32 FieldOrdinal;
	Type CacheExtensionType;
	PXAttributeLevel AttributeLevel;
	Object TypeId;
	void CommandPreparing(PXCache sender, PXCommandPreparingEventArgs e);
	void ForceEnabled();
	void GetSubscriber(List<ISubscriber> subscribers);
	void Initialize();
	void RowSelecting(PXCache sender, PXRowSelectingEventArgs e);
	PXEventSubscriberAttribute[] GetAttributes();
	PXEventSubscriberAttribute[] GetAggregatedAttributes();
	T GetAttribute();
	PXEventSubscriberAttribute Clone(PXAttributeLevel attributeLevel);
	void InjectAttributeDependencies(PXCache cache);
	void CacheAttached(PXCache sender);
	void SetBqlTable(Type bqlTable);
	Boolean ChildrenAttributesComeFirstFor();
	Int32 GetHashCode();
	Boolean Equals(Object obj);
	Type GetBaseCacheWithTableAttr(Type cache);
	void InvokeCacheAttached(PXCache cache);
	T CreateInstance(Object[] constructorArgs);
	PXEventSubscriberAttribute CreateInstance(Type t, Object[] constructorArgs);
	String ToString();
	Attribute[] GetCustomAttributes(MemberInfo element, Type type);
	Attribute[] GetCustomAttributes(MemberInfo element, Type type, Boolean inherit);
	Attribute[] GetCustomAttributes(MemberInfo element);
	Attribute[] GetCustomAttributes(MemberInfo element, Boolean inherit);
	Boolean IsDefined(MemberInfo element, Type attributeType);
	Boolean IsDefined(MemberInfo element, Type attributeType, Boolean inherit);
	Attribute GetCustomAttribute(MemberInfo element, Type attributeType);
	Attribute GetCustomAttribute(MemberInfo element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(ParameterInfo element);
	Attribute[] GetCustomAttributes(ParameterInfo element, Type attributeType);
	Attribute[] GetCustomAttributes(ParameterInfo element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(ParameterInfo element, Boolean inherit);
	Boolean IsDefined(ParameterInfo element, Type attributeType);
	Boolean IsDefined(ParameterInfo element, Type attributeType, Boolean inherit);
	Attribute GetCustomAttribute(ParameterInfo element, Type attributeType);
	Attribute GetCustomAttribute(ParameterInfo element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(Module element, Type attributeType);
	Attribute[] GetCustomAttributes(Module element);
	Attribute[] GetCustomAttributes(Module element, Boolean inherit);
	Attribute[] GetCustomAttributes(Module element, Type attributeType, Boolean inherit);
	Boolean IsDefined(Module element, Type attributeType);
	Boolean IsDefined(Module element, Type attributeType, Boolean inherit);
	Attribute GetCustomAttribute(Module element, Type attributeType);
	Attribute GetCustomAttribute(Module element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(Assembly element, Type attributeType);
	Attribute[] GetCustomAttributes(Assembly element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(Assembly element);
	Attribute[] GetCustomAttributes(Assembly element, Boolean inherit);
	Boolean IsDefined(Assembly element, Type attributeType);
	Boolean IsDefined(Assembly element, Type attributeType, Boolean inherit);
	Attribute GetCustomAttribute(Assembly element, Type attributeType);
	Attribute GetCustomAttribute(Assembly element, Type attributeType, Boolean inherit);
	Boolean Match(Object obj);
	Boolean IsDefaultAttribute();
	void System.Runtime.InteropServices._Attribute.GetTypeInfoCount(UInt32& pcTInfo);
	void System.Runtime.InteropServices._Attribute.GetTypeInfo(UInt32 iTInfo, UInt32 lcid, IntPtr ppTInfo);
	void System.Runtime.InteropServices._Attribute.GetIDsOfNames(Guid& riid, IntPtr rgszNames, UInt32 cNames, UInt32 lcid, IntPtr rgDispId);
	void System.Runtime.InteropServices._Attribute.Invoke(UInt32 dispIdMember, Guid& riid, UInt32 lcid, Int16 wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr);



public class ALCodeAttribute:ALNameAttribute, _Attribute, IPXInterfaceField, IPXCommandPreparingSubscriber, IPXRowSelectingSubscriber, IPXFieldVerifyingSubscriber
	Boolean IsDirty;
	Int32 Length;
	Type BqlField;
	Boolean CacheGlobal;
	Object Constant;
	Type DescriptionField;
	Type SubstituteKey;
	Boolean DirtyRead;
	String DisplayName;
	Boolean Enabled;
	Boolean IsReadOnly;
	PXErrorHandling ErrorHandling;
	PXErrorLevel ErrorLevel;
	String ErrorText;
	Object ErrorValue;
	String FieldClass;
	Boolean Filterable;
	Boolean IsDBField;
	Boolean IsFixed;
	Boolean IsKey;
	PXCacheRights MapEnableRights;
	PXCacheRights MapViewRights;
	PXPersistingCheck PersistingCheck;
	Boolean Required;
	Int32 TabOrder;
	Boolean ValidateValue;
	Boolean SuppressVerify;
	Boolean ViewRights;
	PXUIVisibility Visibility;
	Boolean Visible;
	Type BqlTable;
	String FieldName;
	Int32 FieldOrdinal;
	Type CacheExtensionType;
	PXAttributeLevel AttributeLevel;
	Object TypeId;
	void FieldVerifying(PXCache sender, PXFieldVerifyingEventArgs e);
	void CommandPreparing(PXCache sender, PXCommandPreparingEventArgs e);
	void ForceEnabled();
	void GetSubscriber(List<ISubscriber> subscribers);
	void Initialize();
	void RowSelecting(PXCache sender, PXRowSelectingEventArgs e);
	PXEventSubscriberAttribute[] GetAttributes();
	PXEventSubscriberAttribute[] GetAggregatedAttributes();
	T GetAttribute();
	PXEventSubscriberAttribute Clone(PXAttributeLevel attributeLevel);
	void InjectAttributeDependencies(PXCache cache);
	void CacheAttached(PXCache sender);
	void SetBqlTable(Type bqlTable);
	Boolean ChildrenAttributesComeFirstFor();
	Int32 GetHashCode();
	Boolean Equals(Object obj);
	Type GetBaseCacheWithTableAttr(Type cache);
	void InvokeCacheAttached(PXCache cache);
	T CreateInstance(Object[] constructorArgs);
	PXEventSubscriberAttribute CreateInstance(Type t, Object[] constructorArgs);
	String ToString();
	Attribute[] GetCustomAttributes(MemberInfo element, Type type);
	Attribute[] GetCustomAttributes(MemberInfo element, Type type, Boolean inherit);
	Attribute[] GetCustomAttributes(MemberInfo element);
	Attribute[] GetCustomAttributes(MemberInfo element, Boolean inherit);
	Boolean IsDefined(MemberInfo element, Type attributeType);
	Boolean IsDefined(MemberInfo element, Type attributeType, Boolean inherit);
	Attribute GetCustomAttribute(MemberInfo element, Type attributeType);
	Attribute GetCustomAttribute(MemberInfo element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(ParameterInfo element);
	Attribute[] GetCustomAttributes(ParameterInfo element, Type attributeType);
	Attribute[] GetCustomAttributes(ParameterInfo element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(ParameterInfo element, Boolean inherit);
	Boolean IsDefined(ParameterInfo element, Type attributeType);
	Boolean IsDefined(ParameterInfo element, Type attributeType, Boolean inherit);
	Attribute GetCustomAttribute(ParameterInfo element, Type attributeType);
	Attribute GetCustomAttribute(ParameterInfo element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(Module element, Type attributeType);
	Attribute[] GetCustomAttributes(Module element);
	Attribute[] GetCustomAttributes(Module element, Boolean inherit);
	Attribute[] GetCustomAttributes(Module element, Type attributeType, Boolean inherit);
	Boolean IsDefined(Module element, Type attributeType);
	Boolean IsDefined(Module element, Type attributeType, Boolean inherit);
	Attribute GetCustomAttribute(Module element, Type attributeType);
	Attribute GetCustomAttribute(Module element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(Assembly element, Type attributeType);
	Attribute[] GetCustomAttributes(Assembly element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(Assembly element);
	Attribute[] GetCustomAttributes(Assembly element, Boolean inherit);
	Boolean IsDefined(Assembly element, Type attributeType);
	Boolean IsDefined(Assembly element, Type attributeType, Boolean inherit);
	Attribute GetCustomAttribute(Assembly element, Type attributeType);
	Attribute GetCustomAttribute(Assembly element, Type attributeType, Boolean inherit);
	Boolean Match(Object obj);
	Boolean IsDefaultAttribute();
	void System.Runtime.InteropServices._Attribute.GetTypeInfoCount(UInt32& pcTInfo);
	void System.Runtime.InteropServices._Attribute.GetTypeInfo(UInt32 iTInfo, UInt32 lcid, IntPtr ppTInfo);
	void System.Runtime.InteropServices._Attribute.GetIDsOfNames(Guid& riid, IntPtr rgszNames, UInt32 cNames, UInt32 lcid, IntPtr rgDispId);
	void System.Runtime.InteropServices._Attribute.Invoke(UInt32 dispIdMember, Guid& riid, UInt32 lcid, Int16 wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr);



public class ALDescriptionAttribute:ALAggregateAttribute, _Attribute, IPXInterfaceField, IPXCommandPreparingSubscriber, IPXRowSelectingSubscriber
	Boolean IsDirty;
	Type BqlField;
	Boolean CacheGlobal;
	Object Constant;
	Type DescriptionField;
	Type SubstituteKey;
	Boolean DirtyRead;
	String DisplayName;
	Boolean Enabled;
	Boolean IsReadOnly;
	PXErrorHandling ErrorHandling;
	PXErrorLevel ErrorLevel;
	String ErrorText;
	Object ErrorValue;
	String FieldClass;
	Boolean Filterable;
	Boolean IsDBField;
	Boolean IsFixed;
	Boolean IsKey;
	PXCacheRights MapEnableRights;
	PXCacheRights MapViewRights;
	PXPersistingCheck PersistingCheck;
	Boolean Required;
	Int32 TabOrder;
	Boolean ValidateValue;
	Boolean SuppressVerify;
	Boolean ViewRights;
	PXUIVisibility Visibility;
	Boolean Visible;
	Type BqlTable;
	String FieldName;
	Int32 FieldOrdinal;
	Type CacheExtensionType;
	PXAttributeLevel AttributeLevel;
	Object TypeId;
	void CommandPreparing(PXCache sender, PXCommandPreparingEventArgs e);
	void ForceEnabled();
	void GetSubscriber(List<ISubscriber> subscribers);
	void Initialize();
	void RowSelecting(PXCache sender, PXRowSelectingEventArgs e);
	PXEventSubscriberAttribute[] GetAttributes();
	PXEventSubscriberAttribute[] GetAggregatedAttributes();
	T GetAttribute();
	PXEventSubscriberAttribute Clone(PXAttributeLevel attributeLevel);
	void InjectAttributeDependencies(PXCache cache);
	void CacheAttached(PXCache sender);
	void SetBqlTable(Type bqlTable);
	Boolean ChildrenAttributesComeFirstFor();
	Int32 GetHashCode();
	Boolean Equals(Object obj);
	Type GetBaseCacheWithTableAttr(Type cache);
	void InvokeCacheAttached(PXCache cache);
	T CreateInstance(Object[] constructorArgs);
	PXEventSubscriberAttribute CreateInstance(Type t, Object[] constructorArgs);
	String ToString();
	Attribute[] GetCustomAttributes(MemberInfo element, Type type);
	Attribute[] GetCustomAttributes(MemberInfo element, Type type, Boolean inherit);
	Attribute[] GetCustomAttributes(MemberInfo element);
	Attribute[] GetCustomAttributes(MemberInfo element, Boolean inherit);
	Boolean IsDefined(MemberInfo element, Type attributeType);
	Boolean IsDefined(MemberInfo element, Type attributeType, Boolean inherit);
	Attribute GetCustomAttribute(MemberInfo element, Type attributeType);
	Attribute GetCustomAttribute(MemberInfo element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(ParameterInfo element);
	Attribute[] GetCustomAttributes(ParameterInfo element, Type attributeType);
	Attribute[] GetCustomAttributes(ParameterInfo element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(ParameterInfo element, Boolean inherit);
	Boolean IsDefined(ParameterInfo element, Type attributeType);
	Boolean IsDefined(ParameterInfo element, Type attributeType, Boolean inherit);
	Attribute GetCustomAttribute(ParameterInfo element, Type attributeType);
	Attribute GetCustomAttribute(ParameterInfo element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(Module element, Type attributeType);
	Attribute[] GetCustomAttributes(Module element);
	Attribute[] GetCustomAttributes(Module element, Boolean inherit);
	Attribute[] GetCustomAttributes(Module element, Type attributeType, Boolean inherit);
	Boolean IsDefined(Module element, Type attributeType);
	Boolean IsDefined(Module element, Type attributeType, Boolean inherit);
	Attribute GetCustomAttribute(Module element, Type attributeType);
	Attribute GetCustomAttribute(Module element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(Assembly element, Type attributeType);
	Attribute[] GetCustomAttributes(Assembly element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(Assembly element);
	Attribute[] GetCustomAttributes(Assembly element, Boolean inherit);
	Boolean IsDefined(Assembly element, Type attributeType);
	Boolean IsDefined(Assembly element, Type attributeType, Boolean inherit);
	Attribute GetCustomAttribute(Assembly element, Type attributeType);
	Attribute GetCustomAttribute(Assembly element, Type attributeType, Boolean inherit);
	Boolean Match(Object obj);
	Boolean IsDefaultAttribute();
	void System.Runtime.InteropServices._Attribute.GetTypeInfoCount(UInt32& pcTInfo);
	void System.Runtime.InteropServices._Attribute.GetTypeInfo(UInt32 iTInfo, UInt32 lcid, IntPtr ppTInfo);
	void System.Runtime.InteropServices._Attribute.GetIDsOfNames(Guid& riid, IntPtr rgszNames, UInt32 cNames, UInt32 lcid, IntPtr rgDispId);
	void System.Runtime.InteropServices._Attribute.Invoke(UInt32 dispIdMember, Guid& riid, UInt32 lcid, Int16 wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr);



public class ALIDForeignAttribute:ALAggregateAttribute, _Attribute, IPXInterfaceField, IPXCommandPreparingSubscriber, IPXRowSelectingSubscriber, IPXRowSelectedSubscriber
	Boolean IsDirty;
	Boolean IgnoreMissing;
	Boolean IgnoreDeactivated;
	Type BqlField;
	Boolean CacheGlobal;
	Object Constant;
	Type DescriptionField;
	Type SubstituteKey;
	Boolean DirtyRead;
	String DisplayName;
	Boolean Enabled;
	Boolean IsReadOnly;
	PXErrorHandling ErrorHandling;
	PXErrorLevel ErrorLevel;
	String ErrorText;
	Object ErrorValue;
	String FieldClass;
	Boolean Filterable;
	Boolean IsDBField;
	Boolean IsFixed;
	Boolean IsKey;
	PXCacheRights MapEnableRights;
	PXCacheRights MapViewRights;
	PXPersistingCheck PersistingCheck;
	Boolean Required;
	Int32 TabOrder;
	Boolean ValidateValue;
	Boolean SuppressVerify;
	Boolean ViewRights;
	PXUIVisibility Visibility;
	Boolean Visible;
	Type BqlTable;
	String FieldName;
	Int32 FieldOrdinal;
	Type CacheExtensionType;
	PXAttributeLevel AttributeLevel;
	Object TypeId;
	Boolean IsMissing(PXCache cache, Nullable<Guid> id);
	Boolean IsDeactivated(PXCache cache, Nullable<Guid> id);
	void RowSelected(PXCache cache, PXRowSelectedEventArgs e);
	void CommandPreparing(PXCache sender, PXCommandPreparingEventArgs e);
	void ForceEnabled();
	void GetSubscriber(List<ISubscriber> subscribers);
	void Initialize();
	void RowSelecting(PXCache sender, PXRowSelectingEventArgs e);
	PXEventSubscriberAttribute[] GetAttributes();
	PXEventSubscriberAttribute[] GetAggregatedAttributes();
	T GetAttribute();
	PXEventSubscriberAttribute Clone(PXAttributeLevel attributeLevel);
	void InjectAttributeDependencies(PXCache cache);
	void CacheAttached(PXCache sender);
	void SetBqlTable(Type bqlTable);
	Boolean ChildrenAttributesComeFirstFor();
	Int32 GetHashCode();
	Boolean Equals(Object obj);
	Type GetBaseCacheWithTableAttr(Type cache);
	void InvokeCacheAttached(PXCache cache);
	T CreateInstance(Object[] constructorArgs);
	PXEventSubscriberAttribute CreateInstance(Type t, Object[] constructorArgs);
	String ToString();
	Attribute[] GetCustomAttributes(MemberInfo element, Type type);
	Attribute[] GetCustomAttributes(MemberInfo element, Type type, Boolean inherit);
	Attribute[] GetCustomAttributes(MemberInfo element);
	Attribute[] GetCustomAttributes(MemberInfo element, Boolean inherit);
	Boolean IsDefined(MemberInfo element, Type attributeType);
	Boolean IsDefined(MemberInfo element, Type attributeType, Boolean inherit);
	Attribute GetCustomAttribute(MemberInfo element, Type attributeType);
	Attribute GetCustomAttribute(MemberInfo element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(ParameterInfo element);
	Attribute[] GetCustomAttributes(ParameterInfo element, Type attributeType);
	Attribute[] GetCustomAttributes(ParameterInfo element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(ParameterInfo element, Boolean inherit);
	Boolean IsDefined(ParameterInfo element, Type attributeType);
	Boolean IsDefined(ParameterInfo element, Type attributeType, Boolean inherit);
	Attribute GetCustomAttribute(ParameterInfo element, Type attributeType);
	Attribute GetCustomAttribute(ParameterInfo element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(Module element, Type attributeType);
	Attribute[] GetCustomAttributes(Module element);
	Attribute[] GetCustomAttributes(Module element, Boolean inherit);
	Attribute[] GetCustomAttributes(Module element, Type attributeType, Boolean inherit);
	Boolean IsDefined(Module element, Type attributeType);
	Boolean IsDefined(Module element, Type attributeType, Boolean inherit);
	Attribute GetCustomAttribute(Module element, Type attributeType);
	Attribute GetCustomAttribute(Module element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(Assembly element, Type attributeType);
	Attribute[] GetCustomAttributes(Assembly element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(Assembly element);
	Attribute[] GetCustomAttributes(Assembly element, Boolean inherit);
	Boolean IsDefined(Assembly element, Type attributeType);
	Boolean IsDefined(Assembly element, Type attributeType, Boolean inherit);
	Attribute GetCustomAttribute(Assembly element, Type attributeType);
	Attribute GetCustomAttribute(Assembly element, Type attributeType, Boolean inherit);
	Boolean Match(Object obj);
	Boolean IsDefaultAttribute();
	void System.Runtime.InteropServices._Attribute.GetTypeInfoCount(UInt32& pcTInfo);
	void System.Runtime.InteropServices._Attribute.GetTypeInfo(UInt32 iTInfo, UInt32 lcid, IntPtr ppTInfo);
	void System.Runtime.InteropServices._Attribute.GetIDsOfNames(Guid& riid, IntPtr rgszNames, UInt32 cNames, UInt32 lcid, IntPtr rgDispId);
	void System.Runtime.InteropServices._Attribute.Invoke(UInt32 dispIdMember, Guid& riid, UInt32 lcid, Int16 wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr);



public class ALMultiOptionsAttribute:ALAggregateAttribute, _Attribute, IPXInterfaceField, IPXCommandPreparingSubscriber, IPXRowSelectingSubscriber
	Boolean IsDirty;
	Type BqlField;
	Boolean CacheGlobal;
	Object Constant;
	Type DescriptionField;
	Type SubstituteKey;
	Boolean DirtyRead;
	String DisplayName;
	Boolean Enabled;
	Boolean IsReadOnly;
	PXErrorHandling ErrorHandling;
	PXErrorLevel ErrorLevel;
	String ErrorText;
	Object ErrorValue;
	String FieldClass;
	Boolean Filterable;
	Boolean IsDBField;
	Boolean IsFixed;
	Boolean IsKey;
	PXCacheRights MapEnableRights;
	PXCacheRights MapViewRights;
	PXPersistingCheck PersistingCheck;
	Boolean Required;
	Int32 TabOrder;
	Boolean ValidateValue;
	Boolean SuppressVerify;
	Boolean ViewRights;
	PXUIVisibility Visibility;
	Boolean Visible;
	Type BqlTable;
	String FieldName;
	Int32 FieldOrdinal;
	Type CacheExtensionType;
	PXAttributeLevel AttributeLevel;
	Object TypeId;
	void CommandPreparing(PXCache sender, PXCommandPreparingEventArgs e);
	void ForceEnabled();
	void GetSubscriber(List<ISubscriber> subscribers);
	void Initialize();
	void RowSelecting(PXCache sender, PXRowSelectingEventArgs e);
	PXEventSubscriberAttribute[] GetAttributes();
	PXEventSubscriberAttribute[] GetAggregatedAttributes();
	T GetAttribute();
	PXEventSubscriberAttribute Clone(PXAttributeLevel attributeLevel);
	void InjectAttributeDependencies(PXCache cache);
	void CacheAttached(PXCache sender);
	void SetBqlTable(Type bqlTable);
	Boolean ChildrenAttributesComeFirstFor();
	Int32 GetHashCode();
	Boolean Equals(Object obj);
	Type GetBaseCacheWithTableAttr(Type cache);
	void InvokeCacheAttached(PXCache cache);
	T CreateInstance(Object[] constructorArgs);
	PXEventSubscriberAttribute CreateInstance(Type t, Object[] constructorArgs);
	String ToString();
	Attribute[] GetCustomAttributes(MemberInfo element, Type type);
	Attribute[] GetCustomAttributes(MemberInfo element, Type type, Boolean inherit);
	Attribute[] GetCustomAttributes(MemberInfo element);
	Attribute[] GetCustomAttributes(MemberInfo element, Boolean inherit);
	Boolean IsDefined(MemberInfo element, Type attributeType);
	Boolean IsDefined(MemberInfo element, Type attributeType, Boolean inherit);
	Attribute GetCustomAttribute(MemberInfo element, Type attributeType);
	Attribute GetCustomAttribute(MemberInfo element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(ParameterInfo element);
	Attribute[] GetCustomAttributes(ParameterInfo element, Type attributeType);
	Attribute[] GetCustomAttributes(ParameterInfo element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(ParameterInfo element, Boolean inherit);
	Boolean IsDefined(ParameterInfo element, Type attributeType);
	Boolean IsDefined(ParameterInfo element, Type attributeType, Boolean inherit);
	Attribute GetCustomAttribute(ParameterInfo element, Type attributeType);
	Attribute GetCustomAttribute(ParameterInfo element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(Module element, Type attributeType);
	Attribute[] GetCustomAttributes(Module element);
	Attribute[] GetCustomAttributes(Module element, Boolean inherit);
	Attribute[] GetCustomAttributes(Module element, Type attributeType, Boolean inherit);
	Boolean IsDefined(Module element, Type attributeType);
	Boolean IsDefined(Module element, Type attributeType, Boolean inherit);
	Attribute GetCustomAttribute(Module element, Type attributeType);
	Attribute GetCustomAttribute(Module element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(Assembly element, Type attributeType);
	Attribute[] GetCustomAttributes(Assembly element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(Assembly element);
	Attribute[] GetCustomAttributes(Assembly element, Boolean inherit);
	Boolean IsDefined(Assembly element, Type attributeType);
	Boolean IsDefined(Assembly element, Type attributeType, Boolean inherit);
	Attribute GetCustomAttribute(Assembly element, Type attributeType);
	Attribute GetCustomAttribute(Assembly element, Type attributeType, Boolean inherit);
	Boolean Match(Object obj);
	Boolean IsDefaultAttribute();
	void System.Runtime.InteropServices._Attribute.GetTypeInfoCount(UInt32& pcTInfo);
	void System.Runtime.InteropServices._Attribute.GetTypeInfo(UInt32 iTInfo, UInt32 lcid, IntPtr ppTInfo);
	void System.Runtime.InteropServices._Attribute.GetIDsOfNames(Guid& riid, IntPtr rgszNames, UInt32 cNames, UInt32 lcid, IntPtr rgDispId);
	void System.Runtime.InteropServices._Attribute.Invoke(UInt32 dispIdMember, Guid& riid, UInt32 lcid, Int16 wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr);



public class ALNameAttribute:ALAggregateAttribute, _Attribute, IPXInterfaceField, IPXCommandPreparingSubscriber, IPXRowSelectingSubscriber, IPXFieldVerifyingSubscriber
	Int32 NAME_LENGTH;
	Int32 LONG_NAME_LENGTH;
	Boolean IsDirty;
	Int32 Length;
	Type BqlField;
	Boolean CacheGlobal;
	Object Constant;
	Type DescriptionField;
	Type SubstituteKey;
	Boolean DirtyRead;
	String DisplayName;
	Boolean Enabled;
	Boolean IsReadOnly;
	PXErrorHandling ErrorHandling;
	PXErrorLevel ErrorLevel;
	String ErrorText;
	Object ErrorValue;
	String FieldClass;
	Boolean Filterable;
	Boolean IsDBField;
	Boolean IsFixed;
	Boolean IsKey;
	PXCacheRights MapEnableRights;
	PXCacheRights MapViewRights;
	PXPersistingCheck PersistingCheck;
	Boolean Required;
	Int32 TabOrder;
	Boolean ValidateValue;
	Boolean SuppressVerify;
	Boolean ViewRights;
	PXUIVisibility Visibility;
	Boolean Visible;
	Type BqlTable;
	String FieldName;
	Int32 FieldOrdinal;
	Type CacheExtensionType;
	PXAttributeLevel AttributeLevel;
	Object TypeId;
	void FieldVerifying(PXCache sender, PXFieldVerifyingEventArgs e);
	void CommandPreparing(PXCache sender, PXCommandPreparingEventArgs e);
	void ForceEnabled();
	void GetSubscriber(List<ISubscriber> subscribers);
	void Initialize();
	void RowSelecting(PXCache sender, PXRowSelectingEventArgs e);
	PXEventSubscriberAttribute[] GetAttributes();
	PXEventSubscriberAttribute[] GetAggregatedAttributes();
	T GetAttribute();
	PXEventSubscriberAttribute Clone(PXAttributeLevel attributeLevel);
	void InjectAttributeDependencies(PXCache cache);
	void CacheAttached(PXCache sender);
	void SetBqlTable(Type bqlTable);
	Boolean ChildrenAttributesComeFirstFor();
	Int32 GetHashCode();
	Boolean Equals(Object obj);
	Type GetBaseCacheWithTableAttr(Type cache);
	void InvokeCacheAttached(PXCache cache);
	T CreateInstance(Object[] constructorArgs);
	PXEventSubscriberAttribute CreateInstance(Type t, Object[] constructorArgs);
	String ToString();
	Attribute[] GetCustomAttributes(MemberInfo element, Type type);
	Attribute[] GetCustomAttributes(MemberInfo element, Type type, Boolean inherit);
	Attribute[] GetCustomAttributes(MemberInfo element);
	Attribute[] GetCustomAttributes(MemberInfo element, Boolean inherit);
	Boolean IsDefined(MemberInfo element, Type attributeType);
	Boolean IsDefined(MemberInfo element, Type attributeType, Boolean inherit);
	Attribute GetCustomAttribute(MemberInfo element, Type attributeType);
	Attribute GetCustomAttribute(MemberInfo element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(ParameterInfo element);
	Attribute[] GetCustomAttributes(ParameterInfo element, Type attributeType);
	Attribute[] GetCustomAttributes(ParameterInfo element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(ParameterInfo element, Boolean inherit);
	Boolean IsDefined(ParameterInfo element, Type attributeType);
	Boolean IsDefined(ParameterInfo element, Type attributeType, Boolean inherit);
	Attribute GetCustomAttribute(ParameterInfo element, Type attributeType);
	Attribute GetCustomAttribute(ParameterInfo element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(Module element, Type attributeType);
	Attribute[] GetCustomAttributes(Module element);
	Attribute[] GetCustomAttributes(Module element, Boolean inherit);
	Attribute[] GetCustomAttributes(Module element, Type attributeType, Boolean inherit);
	Boolean IsDefined(Module element, Type attributeType);
	Boolean IsDefined(Module element, Type attributeType, Boolean inherit);
	Attribute GetCustomAttribute(Module element, Type attributeType);
	Attribute GetCustomAttribute(Module element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(Assembly element, Type attributeType);
	Attribute[] GetCustomAttributes(Assembly element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(Assembly element);
	Attribute[] GetCustomAttributes(Assembly element, Boolean inherit);
	Boolean IsDefined(Assembly element, Type attributeType);
	Boolean IsDefined(Assembly element, Type attributeType, Boolean inherit);
	Attribute GetCustomAttribute(Assembly element, Type attributeType);
	Attribute GetCustomAttribute(Assembly element, Type attributeType, Boolean inherit);
	Boolean Match(Object obj);
	Boolean IsDefaultAttribute();
	void System.Runtime.InteropServices._Attribute.GetTypeInfoCount(UInt32& pcTInfo);
	void System.Runtime.InteropServices._Attribute.GetTypeInfo(UInt32 iTInfo, UInt32 lcid, IntPtr ppTInfo);
	void System.Runtime.InteropServices._Attribute.GetIDsOfNames(Guid& riid, IntPtr rgszNames, UInt32 cNames, UInt32 lcid, IntPtr rgDispId);
	void System.Runtime.InteropServices._Attribute.Invoke(UInt32 dispIdMember, Guid& riid, UInt32 lcid, Int16 wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr);



public class ALPrinterTypeSelectableAttribute:ALSelectableAttribute<IDestination>, _Attribute, IPXFieldSelectingSubscriber, IPXLocalizableList
	Boolean IsDirty;
	Boolean IsLocalizable;
	Boolean IsLocalized;
	Boolean SortByValues;
	Boolean MultiSelect;
	Boolean ExclusiveValues;
	Type BqlField;
	Dictionary<String,String> ValueLabelDic;
	PXAttributeLevel AttributeLevel;
	Type BqlTable;
	Type CacheExtensionType;
	String FieldName;
	Int32 FieldOrdinal;
	Object TypeId;
	ValueTuple`2[] GetTuples();
	Boolean IsAccepted(ISelectable selectable);
	String[] GetAllowedValues(PXCache cache);
	String[] SplitMultiSelectValues(String values);
	String GetLocalizedLabel(PXCache cache, Object row);
	String GetLocalizedLabel(PXCache cache, Object row, String value);
	void SetLocalizable(PXCache cache, Object data, Boolean isLocalizable);
	void SetList(PXCache cache, Object data, PXStringListAttribute listSource);
	void SetList(PXCache cache, Object data, String[] allowedValues, String[] allowedLabels);
	void SetList(PXCache cache, Object data, Tuple`2[] valuesToLabels);
	void SetList(PXCache cache, Object data, ValueTuple`2[] valuesToLabels);
	void SetList(PXCache cache, Object data, String field, String[] allowedValues, String[] allowedLabels);
	void SetList(PXCache cache, Object data, String field, PXStringListAttribute listSource);
	void SetList(PXCache cache, Object data, String field, Tuple`2[] valuesToLabels);
	void SetList(PXCache cache, Object data, String field, ValueTuple`2[] valuesToLabels);
	void SetListInternal(IEnumerable<PXStringListAttribute> attributes, String[] allowedValues, String[] allowedLabels, PXCache cache);
	void AppendList(PXCache cache, Object data, ValueTuple`2[] valuesToLabels);
	void AppendList(PXCache cache, Object data, String[] allowedValues, String[] allowedLabels);
	void AppendList(PXCache cache, Object data, String field, String[] allowedValues, String[] allowedLabels);
	void SetExclusiveValues(PXCache cache, Object data, Boolean exclusiveValues);
	void SetExclusiveValues(PXCache cache, Object data, String field, Boolean exclusiveValues);
	void FieldSelecting(PXCache sender, PXFieldSelectingEventArgs e);
	void OrderByCommandPreparing(PXCache sender, PXCommandPreparingEventArgs e);
	void CacheAttached(PXCache sender);
	void MultiSelectFieldUpdating(PXCache sender, PXFieldUpdatingEventArgs e);
	void RemoveDisabledValues(String cacheName);
	void TryLocalize(PXCache sender);
	void RipDynamicLabels(String[] dynamicAllowedLabels, PXCache sender);
	Tuple<String,String> Pair(String value, String label);
	Int32 GetHashCode();
	Boolean Equals(Object obj);
	PXEventSubscriberAttribute Clone(PXAttributeLevel attributeLevel);
	Type GetBaseCacheWithTableAttr(Type cache);
	void SetBqlTable(Type bqlTable);
	void InjectAttributeDependencies(PXCache cache);
	void InvokeCacheAttached(PXCache cache);
	void GetSubscriber(List<ISubscriber> subscribers);
	T CreateInstance(Object[] constructorArgs);
	PXEventSubscriberAttribute CreateInstance(Type t, Object[] constructorArgs);
	String ToString();
	Attribute[] GetCustomAttributes(MemberInfo element, Type type);
	Attribute[] GetCustomAttributes(MemberInfo element, Type type, Boolean inherit);
	Attribute[] GetCustomAttributes(MemberInfo element);
	Attribute[] GetCustomAttributes(MemberInfo element, Boolean inherit);
	Boolean IsDefined(MemberInfo element, Type attributeType);
	Boolean IsDefined(MemberInfo element, Type attributeType, Boolean inherit);
	Attribute GetCustomAttribute(MemberInfo element, Type attributeType);
	Attribute GetCustomAttribute(MemberInfo element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(ParameterInfo element);
	Attribute[] GetCustomAttributes(ParameterInfo element, Type attributeType);
	Attribute[] GetCustomAttributes(ParameterInfo element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(ParameterInfo element, Boolean inherit);
	Boolean IsDefined(ParameterInfo element, Type attributeType);
	Boolean IsDefined(ParameterInfo element, Type attributeType, Boolean inherit);
	Attribute GetCustomAttribute(ParameterInfo element, Type attributeType);
	Attribute GetCustomAttribute(ParameterInfo element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(Module element, Type attributeType);
	Attribute[] GetCustomAttributes(Module element);
	Attribute[] GetCustomAttributes(Module element, Boolean inherit);
	Attribute[] GetCustomAttributes(Module element, Type attributeType, Boolean inherit);
	Boolean IsDefined(Module element, Type attributeType);
	Boolean IsDefined(Module element, Type attributeType, Boolean inherit);
	Attribute GetCustomAttribute(Module element, Type attributeType);
	Attribute GetCustomAttribute(Module element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(Assembly element, Type attributeType);
	Attribute[] GetCustomAttributes(Assembly element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(Assembly element);
	Attribute[] GetCustomAttributes(Assembly element, Boolean inherit);
	Boolean IsDefined(Assembly element, Type attributeType);
	Boolean IsDefined(Assembly element, Type attributeType, Boolean inherit);
	Attribute GetCustomAttribute(Assembly element, Type attributeType);
	Attribute GetCustomAttribute(Assembly element, Type attributeType, Boolean inherit);
	Boolean Match(Object obj);
	Boolean IsDefaultAttribute();
	void System.Runtime.InteropServices._Attribute.GetTypeInfoCount(UInt32& pcTInfo);
	void System.Runtime.InteropServices._Attribute.GetTypeInfo(UInt32 iTInfo, UInt32 lcid, IntPtr ppTInfo);
	void System.Runtime.InteropServices._Attribute.GetIDsOfNames(Guid& riid, IntPtr rgszNames, UInt32 cNames, UInt32 lcid, IntPtr rgDispId);
	void System.Runtime.InteropServices._Attribute.Invoke(UInt32 dispIdMember, Guid& riid, UInt32 lcid, Int16 wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr);



public class ALTypeDropDownAttribute:ALAbstractStringListAttribute, _Attribute, IPXFieldSelectingSubscriber, IPXLocalizableList
	Boolean IsDirty;
	Boolean IsLocalizable;
	Boolean IsLocalized;
	Boolean SortByValues;
	Boolean MultiSelect;
	Boolean ExclusiveValues;
	Type BqlField;
	Dictionary<String,String> ValueLabelDic;
	PXAttributeLevel AttributeLevel;
	Type BqlTable;
	Type CacheExtensionType;
	String FieldName;
	Int32 FieldOrdinal;
	Object TypeId;
	ValueTuple`2[] GetTuples();
	ALSelectorRecord ToRecord(Type type);
	String[] GetAllowedValues(PXCache cache);
	String[] SplitMultiSelectValues(String values);
	String GetLocalizedLabel(PXCache cache, Object row);
	String GetLocalizedLabel(PXCache cache, Object row, String value);
	void SetLocalizable(PXCache cache, Object data, Boolean isLocalizable);
	void SetList(PXCache cache, Object data, PXStringListAttribute listSource);
	void SetList(PXCache cache, Object data, String[] allowedValues, String[] allowedLabels);
	void SetList(PXCache cache, Object data, Tuple`2[] valuesToLabels);
	void SetList(PXCache cache, Object data, ValueTuple`2[] valuesToLabels);
	void SetList(PXCache cache, Object data, String field, String[] allowedValues, String[] allowedLabels);
	void SetList(PXCache cache, Object data, String field, PXStringListAttribute listSource);
	void SetList(PXCache cache, Object data, String field, Tuple`2[] valuesToLabels);
	void SetList(PXCache cache, Object data, String field, ValueTuple`2[] valuesToLabels);
	void SetListInternal(IEnumerable<PXStringListAttribute> attributes, String[] allowedValues, String[] allowedLabels, PXCache cache);
	void AppendList(PXCache cache, Object data, ValueTuple`2[] valuesToLabels);
	void AppendList(PXCache cache, Object data, String[] allowedValues, String[] allowedLabels);
	void AppendList(PXCache cache, Object data, String field, String[] allowedValues, String[] allowedLabels);
	void SetExclusiveValues(PXCache cache, Object data, Boolean exclusiveValues);
	void SetExclusiveValues(PXCache cache, Object data, String field, Boolean exclusiveValues);
	void FieldSelecting(PXCache sender, PXFieldSelectingEventArgs e);
	void OrderByCommandPreparing(PXCache sender, PXCommandPreparingEventArgs e);
	void CacheAttached(PXCache sender);
	void MultiSelectFieldUpdating(PXCache sender, PXFieldUpdatingEventArgs e);
	void RemoveDisabledValues(String cacheName);
	void TryLocalize(PXCache sender);
	void RipDynamicLabels(String[] dynamicAllowedLabels, PXCache sender);
	Tuple<String,String> Pair(String value, String label);
	Int32 GetHashCode();
	Boolean Equals(Object obj);
	PXEventSubscriberAttribute Clone(PXAttributeLevel attributeLevel);
	Type GetBaseCacheWithTableAttr(Type cache);
	void SetBqlTable(Type bqlTable);
	void InjectAttributeDependencies(PXCache cache);
	void InvokeCacheAttached(PXCache cache);
	void GetSubscriber(List<ISubscriber> subscribers);
	T CreateInstance(Object[] constructorArgs);
	PXEventSubscriberAttribute CreateInstance(Type t, Object[] constructorArgs);
	String ToString();
	Attribute[] GetCustomAttributes(MemberInfo element, Type type);
	Attribute[] GetCustomAttributes(MemberInfo element, Type type, Boolean inherit);
	Attribute[] GetCustomAttributes(MemberInfo element);
	Attribute[] GetCustomAttributes(MemberInfo element, Boolean inherit);
	Boolean IsDefined(MemberInfo element, Type attributeType);
	Boolean IsDefined(MemberInfo element, Type attributeType, Boolean inherit);
	Attribute GetCustomAttribute(MemberInfo element, Type attributeType);
	Attribute GetCustomAttribute(MemberInfo element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(ParameterInfo element);
	Attribute[] GetCustomAttributes(ParameterInfo element, Type attributeType);
	Attribute[] GetCustomAttributes(ParameterInfo element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(ParameterInfo element, Boolean inherit);
	Boolean IsDefined(ParameterInfo element, Type attributeType);
	Boolean IsDefined(ParameterInfo element, Type attributeType, Boolean inherit);
	Attribute GetCustomAttribute(ParameterInfo element, Type attributeType);
	Attribute GetCustomAttribute(ParameterInfo element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(Module element, Type attributeType);
	Attribute[] GetCustomAttributes(Module element);
	Attribute[] GetCustomAttributes(Module element, Boolean inherit);
	Attribute[] GetCustomAttributes(Module element, Type attributeType, Boolean inherit);
	Boolean IsDefined(Module element, Type attributeType);
	Boolean IsDefined(Module element, Type attributeType, Boolean inherit);
	Attribute GetCustomAttribute(Module element, Type attributeType);
	Attribute GetCustomAttribute(Module element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(Assembly element, Type attributeType);
	Attribute[] GetCustomAttributes(Assembly element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(Assembly element);
	Attribute[] GetCustomAttributes(Assembly element, Boolean inherit);
	Boolean IsDefined(Assembly element, Type attributeType);
	Boolean IsDefined(Assembly element, Type attributeType, Boolean inherit);
	Attribute GetCustomAttribute(Assembly element, Type attributeType);
	Attribute GetCustomAttribute(Assembly element, Type attributeType, Boolean inherit);
	Boolean Match(Object obj);
	Boolean IsDefaultAttribute();
	void System.Runtime.InteropServices._Attribute.GetTypeInfoCount(UInt32& pcTInfo);
	void System.Runtime.InteropServices._Attribute.GetTypeInfo(UInt32 iTInfo, UInt32 lcid, IntPtr ppTInfo);
	void System.Runtime.InteropServices._Attribute.GetIDsOfNames(Guid& riid, IntPtr rgszNames, UInt32 cNames, UInt32 lcid, IntPtr rgDispId);
	void System.Runtime.InteropServices._Attribute.Invoke(UInt32 dispIdMember, Guid& riid, UInt32 lcid, Int16 wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr);



public class ALSelectableAttribute:ALAbstractStringListAttribute, _Attribute, IPXFieldSelectingSubscriber, IPXLocalizableList
	Boolean IsDirty;
	Boolean IsLocalizable;
	Boolean IsLocalized;
	Boolean SortByValues;
	Boolean MultiSelect;
	Boolean ExclusiveValues;
	Type BqlField;
	Dictionary<String,String> ValueLabelDic;
	PXAttributeLevel AttributeLevel;
	Type BqlTable;
	Type CacheExtensionType;
	String FieldName;
	Int32 FieldOrdinal;
	Object TypeId;
	ValueTuple`2[] GetTuples();
	Boolean IsAccepted(ISelectable selectable);
	ISelectable GetInstance(Type langType);
	String[] GetAllowedValues(PXCache cache);
	String[] SplitMultiSelectValues(String values);
	String GetLocalizedLabel(PXCache cache, Object row);
	String GetLocalizedLabel(PXCache cache, Object row, String value);
	void SetLocalizable(PXCache cache, Object data, Boolean isLocalizable);
	void SetList(PXCache cache, Object data, PXStringListAttribute listSource);
	void SetList(PXCache cache, Object data, String[] allowedValues, String[] allowedLabels);
	void SetList(PXCache cache, Object data, Tuple`2[] valuesToLabels);
	void SetList(PXCache cache, Object data, ValueTuple`2[] valuesToLabels);
	void SetList(PXCache cache, Object data, String field, String[] allowedValues, String[] allowedLabels);
	void SetList(PXCache cache, Object data, String field, PXStringListAttribute listSource);
	void SetList(PXCache cache, Object data, String field, Tuple`2[] valuesToLabels);
	void SetList(PXCache cache, Object data, String field, ValueTuple`2[] valuesToLabels);
	void SetListInternal(IEnumerable<PXStringListAttribute> attributes, String[] allowedValues, String[] allowedLabels, PXCache cache);
	void AppendList(PXCache cache, Object data, ValueTuple`2[] valuesToLabels);
	void AppendList(PXCache cache, Object data, String[] allowedValues, String[] allowedLabels);
	void AppendList(PXCache cache, Object data, String field, String[] allowedValues, String[] allowedLabels);
	void SetExclusiveValues(PXCache cache, Object data, Boolean exclusiveValues);
	void SetExclusiveValues(PXCache cache, Object data, String field, Boolean exclusiveValues);
	void FieldSelecting(PXCache sender, PXFieldSelectingEventArgs e);
	void OrderByCommandPreparing(PXCache sender, PXCommandPreparingEventArgs e);
	void CacheAttached(PXCache sender);
	void MultiSelectFieldUpdating(PXCache sender, PXFieldUpdatingEventArgs e);
	void RemoveDisabledValues(String cacheName);
	void TryLocalize(PXCache sender);
	void RipDynamicLabels(String[] dynamicAllowedLabels, PXCache sender);
	Tuple<String,String> Pair(String value, String label);
	Int32 GetHashCode();
	Boolean Equals(Object obj);
	PXEventSubscriberAttribute Clone(PXAttributeLevel attributeLevel);
	Type GetBaseCacheWithTableAttr(Type cache);
	void SetBqlTable(Type bqlTable);
	void InjectAttributeDependencies(PXCache cache);
	void InvokeCacheAttached(PXCache cache);
	void GetSubscriber(List<ISubscriber> subscribers);
	T CreateInstance(Object[] constructorArgs);
	PXEventSubscriberAttribute CreateInstance(Type t, Object[] constructorArgs);
	String ToString();
	Attribute[] GetCustomAttributes(MemberInfo element, Type type);
	Attribute[] GetCustomAttributes(MemberInfo element, Type type, Boolean inherit);
	Attribute[] GetCustomAttributes(MemberInfo element);
	Attribute[] GetCustomAttributes(MemberInfo element, Boolean inherit);
	Boolean IsDefined(MemberInfo element, Type attributeType);
	Boolean IsDefined(MemberInfo element, Type attributeType, Boolean inherit);
	Attribute GetCustomAttribute(MemberInfo element, Type attributeType);
	Attribute GetCustomAttribute(MemberInfo element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(ParameterInfo element);
	Attribute[] GetCustomAttributes(ParameterInfo element, Type attributeType);
	Attribute[] GetCustomAttributes(ParameterInfo element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(ParameterInfo element, Boolean inherit);
	Boolean IsDefined(ParameterInfo element, Type attributeType);
	Boolean IsDefined(ParameterInfo element, Type attributeType, Boolean inherit);
	Attribute GetCustomAttribute(ParameterInfo element, Type attributeType);
	Attribute GetCustomAttribute(ParameterInfo element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(Module element, Type attributeType);
	Attribute[] GetCustomAttributes(Module element);
	Attribute[] GetCustomAttributes(Module element, Boolean inherit);
	Attribute[] GetCustomAttributes(Module element, Type attributeType, Boolean inherit);
	Boolean IsDefined(Module element, Type attributeType);
	Boolean IsDefined(Module element, Type attributeType, Boolean inherit);
	Attribute GetCustomAttribute(Module element, Type attributeType);
	Attribute GetCustomAttribute(Module element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(Assembly element, Type attributeType);
	Attribute[] GetCustomAttributes(Assembly element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(Assembly element);
	Attribute[] GetCustomAttributes(Assembly element, Boolean inherit);
	Boolean IsDefined(Assembly element, Type attributeType);
	Boolean IsDefined(Assembly element, Type attributeType, Boolean inherit);
	Attribute GetCustomAttribute(Assembly element, Type attributeType);
	Attribute GetCustomAttribute(Assembly element, Type attributeType, Boolean inherit);
	Boolean Match(Object obj);
	Boolean IsDefaultAttribute();
	void System.Runtime.InteropServices._Attribute.GetTypeInfoCount(UInt32& pcTInfo);
	void System.Runtime.InteropServices._Attribute.GetTypeInfo(UInt32 iTInfo, UInt32 lcid, IntPtr ppTInfo);
	void System.Runtime.InteropServices._Attribute.GetIDsOfNames(Guid& riid, IntPtr rgszNames, UInt32 cNames, UInt32 lcid, IntPtr rgDispId);
	void System.Runtime.InteropServices._Attribute.Invoke(UInt32 dispIdMember, Guid& riid, UInt32 lcid, Int16 wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr);



public class ALSelectorRecord:PXBqlTable, IBqlTableSystemDataStorage, IBqlTable
	String Value;
	String Description;
	PXBqlTableSystemData& PX.Data.IBqlTableSystemDataStorage.GetBqlTableSystemData();



public class ALSMPrinterIDForeignAttribute:ALAggregateAttribute, _Attribute, IPXInterfaceField, IPXCommandPreparingSubscriber, IPXRowSelectingSubscriber
	Boolean IsDirty;
	Type BqlField;
	Boolean CacheGlobal;
	Object Constant;
	Type DescriptionField;
	Type SubstituteKey;
	Boolean DirtyRead;
	String DisplayName;
	Boolean Enabled;
	Boolean IsReadOnly;
	PXErrorHandling ErrorHandling;
	PXErrorLevel ErrorLevel;
	String ErrorText;
	Object ErrorValue;
	String FieldClass;
	Boolean Filterable;
	Boolean IsDBField;
	Boolean IsFixed;
	Boolean IsKey;
	PXCacheRights MapEnableRights;
	PXCacheRights MapViewRights;
	PXPersistingCheck PersistingCheck;
	Boolean Required;
	Int32 TabOrder;
	Boolean ValidateValue;
	Boolean SuppressVerify;
	Boolean ViewRights;
	PXUIVisibility Visibility;
	Boolean Visible;
	Type BqlTable;
	String FieldName;
	Int32 FieldOrdinal;
	Type CacheExtensionType;
	PXAttributeLevel AttributeLevel;
	Object TypeId;
	void CommandPreparing(PXCache sender, PXCommandPreparingEventArgs e);
	void ForceEnabled();
	void GetSubscriber(List<ISubscriber> subscribers);
	void Initialize();
	void RowSelecting(PXCache sender, PXRowSelectingEventArgs e);
	PXEventSubscriberAttribute[] GetAttributes();
	PXEventSubscriberAttribute[] GetAggregatedAttributes();
	T GetAttribute();
	PXEventSubscriberAttribute Clone(PXAttributeLevel attributeLevel);
	void InjectAttributeDependencies(PXCache cache);
	void CacheAttached(PXCache sender);
	void SetBqlTable(Type bqlTable);
	Boolean ChildrenAttributesComeFirstFor();
	Int32 GetHashCode();
	Boolean Equals(Object obj);
	Type GetBaseCacheWithTableAttr(Type cache);
	void InvokeCacheAttached(PXCache cache);
	T CreateInstance(Object[] constructorArgs);
	PXEventSubscriberAttribute CreateInstance(Type t, Object[] constructorArgs);
	String ToString();
	Attribute[] GetCustomAttributes(MemberInfo element, Type type);
	Attribute[] GetCustomAttributes(MemberInfo element, Type type, Boolean inherit);
	Attribute[] GetCustomAttributes(MemberInfo element);
	Attribute[] GetCustomAttributes(MemberInfo element, Boolean inherit);
	Boolean IsDefined(MemberInfo element, Type attributeType);
	Boolean IsDefined(MemberInfo element, Type attributeType, Boolean inherit);
	Attribute GetCustomAttribute(MemberInfo element, Type attributeType);
	Attribute GetCustomAttribute(MemberInfo element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(ParameterInfo element);
	Attribute[] GetCustomAttributes(ParameterInfo element, Type attributeType);
	Attribute[] GetCustomAttributes(ParameterInfo element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(ParameterInfo element, Boolean inherit);
	Boolean IsDefined(ParameterInfo element, Type attributeType);
	Boolean IsDefined(ParameterInfo element, Type attributeType, Boolean inherit);
	Attribute GetCustomAttribute(ParameterInfo element, Type attributeType);
	Attribute GetCustomAttribute(ParameterInfo element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(Module element, Type attributeType);
	Attribute[] GetCustomAttributes(Module element);
	Attribute[] GetCustomAttributes(Module element, Boolean inherit);
	Attribute[] GetCustomAttributes(Module element, Type attributeType, Boolean inherit);
	Boolean IsDefined(Module element, Type attributeType);
	Boolean IsDefined(Module element, Type attributeType, Boolean inherit);
	Attribute GetCustomAttribute(Module element, Type attributeType);
	Attribute GetCustomAttribute(Module element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(Assembly element, Type attributeType);
	Attribute[] GetCustomAttributes(Assembly element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(Assembly element);
	Attribute[] GetCustomAttributes(Assembly element, Boolean inherit);
	Boolean IsDefined(Assembly element, Type attributeType);
	Boolean IsDefined(Assembly element, Type attributeType, Boolean inherit);
	Attribute GetCustomAttribute(Assembly element, Type attributeType);
	Attribute GetCustomAttribute(Assembly element, Type attributeType, Boolean inherit);
	Boolean Match(Object obj);
	Boolean IsDefaultAttribute();
	void System.Runtime.InteropServices._Attribute.GetTypeInfoCount(UInt32& pcTInfo);
	void System.Runtime.InteropServices._Attribute.GetTypeInfo(UInt32 iTInfo, UInt32 lcid, IntPtr ppTInfo);
	void System.Runtime.InteropServices._Attribute.GetIDsOfNames(Guid& riid, IntPtr rgszNames, UInt32 cNames, UInt32 lcid, IntPtr rgDispId);
	void System.Runtime.InteropServices._Attribute.Invoke(UInt32 dispIdMember, Guid& riid, UInt32 lcid, Int16 wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr);



public class ALStateIcon



public class ALTypeSelectorAttribute:PXCustomSelectorAttribute, _Attribute, IPXFieldVerifyingSubscriber, IPXFieldSelectingSubscriber, IPXDependsOnFields
	Boolean ValidateValue;
	Boolean IsDirty;
	Boolean ExcludeFromReferenceGeneratingProcess;
	Boolean IsPrimaryViewCompatible;
	Boolean ShowWarningForNotExistsOnSelect;
	String CustomMessageElementDoesntExist;
	String CustomMessageValueDoesntExist;
	String CustomMessageElementDoesntExistOrNoRights;
	String CustomMessageValueDoesntExistOrNoRights;
	Boolean CacheGlobal;
	Type DescriptionField;
	String DescriptionDisplayName;
	Boolean ShowPopupWarning;
	Boolean ShowPopupMessage;
	Type FilterEntity;
	Type SubstituteKey;
	Type Field;
	Boolean DirtyRead;
	Boolean Filterable;
	String[] Headers;
	Type ValueField;
	PXSelectorMode SelectorMode;
	BqlCommand PrimarySelect;
	BqlCommand OriginalSelect;
	Int32 ParsCount;
	Boolean SuppressUnconditionalSelect;
	String ViewName;
	PXAttributeLevel AttributeLevel;
	Type BqlTable;
	Type CacheExtensionType;
	String FieldName;
	Int32 FieldOrdinal;
	Object TypeId;
	void FieldVerifying(PXCache sender, PXFieldVerifyingEventArgs e);
	IEnumerable GetRecords();
	Func<Type,Boolean> GetPredicate(Type inter);
	Boolean IsImplementationOfInterface(Type inter, Type type);
	ALSelectorRecord GetRecord(Type type);
	String GetCCDisplayTypeName(Type type);
	String GetSMDisplayTypeName(Type type);
	Boolean <get_MatchingValues>b__9_0(Type ty);
	PXView GetView(PXCache cache, BqlCommand select, Boolean isReadOnly);
	PXView GetUnconditionalView(PXCache cache);
	void CacheAttached(PXCache sender);
	void CreateView(PXCache sender);
	void EmitDescriptionFieldAlias(PXCache sender, String alias);
	void EmitColumnForDescriptionField(PXCache sender);
	BqlCommand GetSelect();
	void SubscribeToFeatureSet();
	void SetFieldList(Type[] fieldList);
	BqlCommand WhereAnd(PXCache sender, Type whr);
	String GenerateViewName();
	BqlCommand BuildNaturalSelect(Boolean cacheGlobal, Type substituteKey);
	Object SelectSingleBound(PXView view, Object[] currents, Object[] pars);
	Object SelectSingle(PXView view, Object[] pars);
	Object SelectSingle(PXCache cache, Object data, String field, Object value);
	Object SelectSingle(PXCache cache, Object data, String field);
	Object[] MakeParameters(Object lastParameter, Boolean includeLookupJoins);
	ViewWithParameters GetViewWithParameters(PXCache cache, Object lastParameter, Boolean includeLookupJoins);
	Object Select(PXCache cache, Object data, String field);
	Object SelectFirst(PXCache cache, Object data);
	Object SelectFirst(PXCache cache, Object data, String field);
	Object SelectLast(PXCache cache, Object data);
	Object SelectLast(PXCache cache, Object data, String field);
	Object Select(PXCache cache, Object data, String field, Object value);
	GlobalDictionary GetGlobalCache();
	void AppendOtherValues(Dictionary<String,Object> values, PXCache cache, Object row);
	Object CreateGlobalCacheKey(PXCache cache, Object row, Object keyValue);
	Boolean CanCacheGlobal(PXCache foreignCache);
	Object GetItem(PXCache cache, PXSelectorAttribute attr, Object data, Object key);
	Object GetItem(PXCache cache, PXSelectorAttribute attr, Object data, Object key, Boolean unconditionally);
	IBqlTable GetReferencedDacWithoutSelectorCacheUsage(PXCache cache, Object row, Object foreignKeyValue);
	Object GetItemUnconditionally(PXCache cache, PXSelectorAttribute attr, Object key);
	void ClearGlobalCache();
	void ClearGlobalCache(Byte keysCount);
	void ClearGlobalCache(Type table);
	void ClearGlobalCache(Type table, Byte keysCount);
	Object GetField(PXCache cache, Object data, String field, Object value, String foreignField);
	void CheckIntegrityAndPutGlobal(GlobalDictionary globalDictionary, PXCache foreignCache, String foreignField, Object foreignRow, Object ownKey, Boolean isRowDeleted);
	Type GetItemType(PXCache cache, String field);
	List<Object> SelectAll(PXCache cache, Object data);
	List<Object> SelectAll(PXCache cache, String fieldname, Object data);
	Object Select(PXCache cache, Object data);
	Object Select(PXCache cache, Object data, Object value);
	void SetColumns(PXCache cache, Object data, String field, String[] fieldList, String[] headerList);
	void SetColumns(PXCache cache, String field, String[] fieldList, String[] headerList);
	void SetColumns(String[] fieldList, String[] headerList);
	void SetColumns(PXCache cache, Object data, Type[] fieldList, String[] headerList);
	void SetColumns(PXCache cache, Type[] fieldList, String[] headerList);
	void StoreCached(PXCache cache, Object data, Object item);
	void StoreCached(PXCache cache, Object data, Object item, Boolean clearCache);
	void StoreResult(PXCache cache, Object data, IBqlTable selectResult);
	void StoreResult(PXCache cache, IBqlTable selectResult);
	void StoreResult(PXCache cache, Object data, List<Object> selectResult);
	void CheckAndRaiseForeignKeyException(PXCache sender, Object Row, Type fieldType, Type searchType, String customMessage);
	ISet<Type> GetDependencies(PXCache sender);
	Boolean SplitFieldNames(String fieldName, String& outerField, String& innerField);
	void Verify(PXCache sender, PXFieldVerifyingEventArgs e, Object& item);
	String[] hasRestrictedAccess(PXCache sender, BqlCommand command, Object row);
	void throwNoItem(String[] restricted, Boolean external, Object value);
	void throwNoItem(String[] restricted, Boolean external, Object value, IBqlTable row);
	void FieldSelecting(PXCache sender, PXFieldSelectingEventArgs e);
	void DescriptionFieldSelecting(PXCache sender, PXFieldSelectingEventArgs e, Object key, Boolean& deleted);
	void DescriptionFieldSelecting(PXCache sender, PXFieldSelectingEventArgs e, String alias);
	void readItem(PXCache sender, Object row, Object key, PXCache& itemCache, Object& item, Boolean& deleted);
	void cacheOnReadItem(GlobalDictionary dict, PXCache foreignCache, Object foreignItem, Boolean isItemDeleted);
	void OnItemCached(PXCache foreignCache, Object foreignItem, Boolean isItemDeleted);
	SubstituteKeyInfo getSubstituteKeyMask(PXCache sender);
	String getDescriptionName(PXCache sender, Nullable`1& length);
	String _GetSlotName(Type type, Byte keysCount);
	void SelfRowSelecting(PXCache sender, PXRowSelectingEventArgs e);
	void SubstituteKeyFieldSelecting(PXCache sender, PXFieldSelectingEventArgs e);
	void SubstituteKeyFieldUpdating(PXCache sender, PXFieldUpdatingEventArgs e);
	void SubstituteKeyCommandPreparing(PXCache sender, PXCommandPreparingEventArgs e);
	Boolean ShouldPrepareCommandForSubstituteKey(PXCommandPreparingEventArgs e);
	void DescriptionFieldCommandPreparing(PXCache sender, PXCommandPreparingEventArgs e);
	void ForeignTableRowPersisted(PXCache sender, PXRowPersistedEventArgs e);
	void ReadDeletedFieldVerifying(PXCache sender, PXFieldVerifyingEventArgs e);
	void SetBqlTable(Type bqlTable);
	List<KeyValuePair`2> GetSelectorFields(Type table);
	void populateFields(PXCache sender, Boolean bypassInit);
	void findFieldsHeaders(PXCache sender);
	void CreateFilter(PXGraph graph);
	Int32 GetHashCode();
	Boolean Equals(Object obj);
	PXEventSubscriberAttribute Clone(PXAttributeLevel attributeLevel);
	Type GetBaseCacheWithTableAttr(Type cache);
	void InjectAttributeDependencies(PXCache cache);
	void InvokeCacheAttached(PXCache cache);
	void GetSubscriber(List<ISubscriber> subscribers);
	T CreateInstance(Object[] constructorArgs);
	PXEventSubscriberAttribute CreateInstance(Type t, Object[] constructorArgs);
	String ToString();
	Attribute[] GetCustomAttributes(MemberInfo element, Type type);
	Attribute[] GetCustomAttributes(MemberInfo element, Type type, Boolean inherit);
	Attribute[] GetCustomAttributes(MemberInfo element);
	Attribute[] GetCustomAttributes(MemberInfo element, Boolean inherit);
	Boolean IsDefined(MemberInfo element, Type attributeType);
	Boolean IsDefined(MemberInfo element, Type attributeType, Boolean inherit);
	Attribute GetCustomAttribute(MemberInfo element, Type attributeType);
	Attribute GetCustomAttribute(MemberInfo element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(ParameterInfo element);
	Attribute[] GetCustomAttributes(ParameterInfo element, Type attributeType);
	Attribute[] GetCustomAttributes(ParameterInfo element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(ParameterInfo element, Boolean inherit);
	Boolean IsDefined(ParameterInfo element, Type attributeType);
	Boolean IsDefined(ParameterInfo element, Type attributeType, Boolean inherit);
	Attribute GetCustomAttribute(ParameterInfo element, Type attributeType);
	Attribute GetCustomAttribute(ParameterInfo element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(Module element, Type attributeType);
	Attribute[] GetCustomAttributes(Module element);
	Attribute[] GetCustomAttributes(Module element, Boolean inherit);
	Attribute[] GetCustomAttributes(Module element, Type attributeType, Boolean inherit);
	Boolean IsDefined(Module element, Type attributeType);
	Boolean IsDefined(Module element, Type attributeType, Boolean inherit);
	Attribute GetCustomAttribute(Module element, Type attributeType);
	Attribute GetCustomAttribute(Module element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(Assembly element, Type attributeType);
	Attribute[] GetCustomAttributes(Assembly element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(Assembly element);
	Attribute[] GetCustomAttributes(Assembly element, Boolean inherit);
	Boolean IsDefined(Assembly element, Type attributeType);
	Boolean IsDefined(Assembly element, Type attributeType, Boolean inherit);
	Attribute GetCustomAttribute(Assembly element, Type attributeType);
	Attribute GetCustomAttribute(Assembly element, Type attributeType, Boolean inherit);
	Boolean Match(Object obj);
	Boolean IsDefaultAttribute();
	void System.Runtime.InteropServices._Attribute.GetTypeInfoCount(UInt32& pcTInfo);
	void System.Runtime.InteropServices._Attribute.GetTypeInfo(UInt32 iTInfo, UInt32 lcid, IntPtr ppTInfo);
	void System.Runtime.InteropServices._Attribute.GetIDsOfNames(Guid& riid, IntPtr rgszNames, UInt32 cNames, UInt32 lcid, IntPtr rgDispId);
	void System.Runtime.InteropServices._Attribute.Invoke(UInt32 dispIdMember, Guid& riid, UInt32 lcid, Int16 wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr);



public class ALViewFieldAttribute:ALAggregateAttribute, _Attribute, IPXInterfaceField, IPXCommandPreparingSubscriber, IPXRowSelectingSubscriber, IPXFieldDefaultingSubscriber
	Boolean IsDirty;
	Type BqlField;
	Boolean CacheGlobal;
	Object Constant;
	Type DescriptionField;
	Type SubstituteKey;
	Boolean DirtyRead;
	String DisplayName;
	Boolean Enabled;
	Boolean IsReadOnly;
	PXErrorHandling ErrorHandling;
	PXErrorLevel ErrorLevel;
	String ErrorText;
	Object ErrorValue;
	String FieldClass;
	Boolean Filterable;
	Boolean IsDBField;
	Boolean IsFixed;
	Boolean IsKey;
	PXCacheRights MapEnableRights;
	PXCacheRights MapViewRights;
	PXPersistingCheck PersistingCheck;
	Boolean Required;
	Int32 TabOrder;
	Boolean ValidateValue;
	Boolean SuppressVerify;
	Boolean ViewRights;
	PXUIVisibility Visibility;
	Boolean Visible;
	Type BqlTable;
	String FieldName;
	Int32 FieldOrdinal;
	Type CacheExtensionType;
	PXAttributeLevel AttributeLevel;
	Object TypeId;
	void CacheAttached(PXCache sender);
	void DependencyUpdated(PXCache sender, PXFieldUpdatedEventArgs e);
	void FieldDefaulting(PXCache sender, PXFieldDefaultingEventArgs e);
	void CommandPreparing(PXCache sender, PXCommandPreparingEventArgs e);
	void ForceEnabled();
	void GetSubscriber(List<ISubscriber> subscribers);
	void Initialize();
	void RowSelecting(PXCache sender, PXRowSelectingEventArgs e);
	PXEventSubscriberAttribute[] GetAttributes();
	PXEventSubscriberAttribute[] GetAggregatedAttributes();
	T GetAttribute();
	PXEventSubscriberAttribute Clone(PXAttributeLevel attributeLevel);
	void InjectAttributeDependencies(PXCache cache);
	void SetBqlTable(Type bqlTable);
	Boolean ChildrenAttributesComeFirstFor();
	Int32 GetHashCode();
	Boolean Equals(Object obj);
	Type GetBaseCacheWithTableAttr(Type cache);
	void InvokeCacheAttached(PXCache cache);
	T CreateInstance(Object[] constructorArgs);
	PXEventSubscriberAttribute CreateInstance(Type t, Object[] constructorArgs);
	String ToString();
	Attribute[] GetCustomAttributes(MemberInfo element, Type type);
	Attribute[] GetCustomAttributes(MemberInfo element, Type type, Boolean inherit);
	Attribute[] GetCustomAttributes(MemberInfo element);
	Attribute[] GetCustomAttributes(MemberInfo element, Boolean inherit);
	Boolean IsDefined(MemberInfo element, Type attributeType);
	Boolean IsDefined(MemberInfo element, Type attributeType, Boolean inherit);
	Attribute GetCustomAttribute(MemberInfo element, Type attributeType);
	Attribute GetCustomAttribute(MemberInfo element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(ParameterInfo element);
	Attribute[] GetCustomAttributes(ParameterInfo element, Type attributeType);
	Attribute[] GetCustomAttributes(ParameterInfo element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(ParameterInfo element, Boolean inherit);
	Boolean IsDefined(ParameterInfo element, Type attributeType);
	Boolean IsDefined(ParameterInfo element, Type attributeType, Boolean inherit);
	Attribute GetCustomAttribute(ParameterInfo element, Type attributeType);
	Attribute GetCustomAttribute(ParameterInfo element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(Module element, Type attributeType);
	Attribute[] GetCustomAttributes(Module element);
	Attribute[] GetCustomAttributes(Module element, Boolean inherit);
	Attribute[] GetCustomAttributes(Module element, Type attributeType, Boolean inherit);
	Boolean IsDefined(Module element, Type attributeType);
	Boolean IsDefined(Module element, Type attributeType, Boolean inherit);
	Attribute GetCustomAttribute(Module element, Type attributeType);
	Attribute GetCustomAttribute(Module element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(Assembly element, Type attributeType);
	Attribute[] GetCustomAttributes(Assembly element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(Assembly element);
	Attribute[] GetCustomAttributes(Assembly element, Boolean inherit);
	Boolean IsDefined(Assembly element, Type attributeType);
	Boolean IsDefined(Assembly element, Type attributeType, Boolean inherit);
	Attribute GetCustomAttribute(Assembly element, Type attributeType);
	Attribute GetCustomAttribute(Assembly element, Type attributeType, Boolean inherit);
	Boolean Match(Object obj);
	Boolean IsDefaultAttribute();
	void System.Runtime.InteropServices._Attribute.GetTypeInfoCount(UInt32& pcTInfo);
	void System.Runtime.InteropServices._Attribute.GetTypeInfo(UInt32 iTInfo, UInt32 lcid, IntPtr ppTInfo);
	void System.Runtime.InteropServices._Attribute.GetIDsOfNames(Guid& riid, IntPtr rgszNames, UInt32 cNames, UInt32 lcid, IntPtr rgDispId);
	void System.Runtime.InteropServices._Attribute.Invoke(UInt32 dispIdMember, Guid& riid, UInt32 lcid, Int16 wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr);



public class ALViewSelectorAttribute:PXCustomSelectorAttribute, _Attribute, IPXFieldVerifyingSubscriber, IPXFieldSelectingSubscriber, IPXDependsOnFields, IPXFieldDefaultingSubscriber
	Boolean ValidateValue;
	Boolean IsDirty;
	Boolean ExcludeFromReferenceGeneratingProcess;
	Boolean IsPrimaryViewCompatible;
	Boolean ShowWarningForNotExistsOnSelect;
	String CustomMessageElementDoesntExist;
	String CustomMessageValueDoesntExist;
	String CustomMessageElementDoesntExistOrNoRights;
	String CustomMessageValueDoesntExistOrNoRights;
	Boolean CacheGlobal;
	Type DescriptionField;
	String DescriptionDisplayName;
	Boolean ShowPopupWarning;
	Boolean ShowPopupMessage;
	Type FilterEntity;
	Type SubstituteKey;
	Type Field;
	Boolean DirtyRead;
	Boolean Filterable;
	String[] Headers;
	Type ValueField;
	PXSelectorMode SelectorMode;
	BqlCommand PrimarySelect;
	BqlCommand OriginalSelect;
	Int32 ParsCount;
	Boolean SuppressUnconditionalSelect;
	String ViewName;
	PXAttributeLevel AttributeLevel;
	Type BqlTable;
	Type CacheExtensionType;
	String FieldName;
	Int32 FieldOrdinal;
	Object TypeId;
	void CacheAttached(PXCache sender);
	IEnumerable GetRecords();
	void FieldSelecting(PXCache sender, PXFieldSelectingEventArgs e);
	IEnumerable HandleGraphChange(Object row);
	void FieldDefaulting(PXCache sender, PXFieldDefaultingEventArgs e);
	IEnumerable _GetRecordsInternal(Object row);
	Boolean KeepView(ViewDef view);
	Type GetGraphType(Object data);
	void DescriptionFieldSelecting(PXCache sender, PXFieldSelectingEventArgs e, String alias);
	PXView GetView(PXCache cache, BqlCommand select, Boolean isReadOnly);
	PXView GetUnconditionalView(PXCache cache);
	void FieldVerifying(PXCache sender, PXFieldVerifyingEventArgs e);
	void CreateView(PXCache sender);
	void EmitDescriptionFieldAlias(PXCache sender, String alias);
	void EmitColumnForDescriptionField(PXCache sender);
	BqlCommand GetSelect();
	void SubscribeToFeatureSet();
	void SetFieldList(Type[] fieldList);
	BqlCommand WhereAnd(PXCache sender, Type whr);
	String GenerateViewName();
	BqlCommand BuildNaturalSelect(Boolean cacheGlobal, Type substituteKey);
	Object SelectSingleBound(PXView view, Object[] currents, Object[] pars);
	Object SelectSingle(PXView view, Object[] pars);
	Object SelectSingle(PXCache cache, Object data, String field, Object value);
	Object SelectSingle(PXCache cache, Object data, String field);
	Object[] MakeParameters(Object lastParameter, Boolean includeLookupJoins);
	ViewWithParameters GetViewWithParameters(PXCache cache, Object lastParameter, Boolean includeLookupJoins);
	Object Select(PXCache cache, Object data, String field);
	Object SelectFirst(PXCache cache, Object data);
	Object SelectFirst(PXCache cache, Object data, String field);
	Object SelectLast(PXCache cache, Object data);
	Object SelectLast(PXCache cache, Object data, String field);
	Object Select(PXCache cache, Object data, String field, Object value);
	GlobalDictionary GetGlobalCache();
	void AppendOtherValues(Dictionary<String,Object> values, PXCache cache, Object row);
	Object CreateGlobalCacheKey(PXCache cache, Object row, Object keyValue);
	Boolean CanCacheGlobal(PXCache foreignCache);
	Object GetItem(PXCache cache, PXSelectorAttribute attr, Object data, Object key);
	Object GetItem(PXCache cache, PXSelectorAttribute attr, Object data, Object key, Boolean unconditionally);
	IBqlTable GetReferencedDacWithoutSelectorCacheUsage(PXCache cache, Object row, Object foreignKeyValue);
	Object GetItemUnconditionally(PXCache cache, PXSelectorAttribute attr, Object key);
	void ClearGlobalCache();
	void ClearGlobalCache(Byte keysCount);
	void ClearGlobalCache(Type table);
	void ClearGlobalCache(Type table, Byte keysCount);
	Object GetField(PXCache cache, Object data, String field, Object value, String foreignField);
	void CheckIntegrityAndPutGlobal(GlobalDictionary globalDictionary, PXCache foreignCache, String foreignField, Object foreignRow, Object ownKey, Boolean isRowDeleted);
	Type GetItemType(PXCache cache, String field);
	List<Object> SelectAll(PXCache cache, Object data);
	List<Object> SelectAll(PXCache cache, String fieldname, Object data);
	Object Select(PXCache cache, Object data);
	Object Select(PXCache cache, Object data, Object value);
	void SetColumns(PXCache cache, Object data, String field, String[] fieldList, String[] headerList);
	void SetColumns(PXCache cache, String field, String[] fieldList, String[] headerList);
	void SetColumns(String[] fieldList, String[] headerList);
	void SetColumns(PXCache cache, Object data, Type[] fieldList, String[] headerList);
	void SetColumns(PXCache cache, Type[] fieldList, String[] headerList);
	void StoreCached(PXCache cache, Object data, Object item);
	void StoreCached(PXCache cache, Object data, Object item, Boolean clearCache);
	void StoreResult(PXCache cache, Object data, IBqlTable selectResult);
	void StoreResult(PXCache cache, IBqlTable selectResult);
	void StoreResult(PXCache cache, Object data, List<Object> selectResult);
	void CheckAndRaiseForeignKeyException(PXCache sender, Object Row, Type fieldType, Type searchType, String customMessage);
	ISet<Type> GetDependencies(PXCache sender);
	Boolean SplitFieldNames(String fieldName, String& outerField, String& innerField);
	void Verify(PXCache sender, PXFieldVerifyingEventArgs e, Object& item);
	String[] hasRestrictedAccess(PXCache sender, BqlCommand command, Object row);
	void throwNoItem(String[] restricted, Boolean external, Object value);
	void throwNoItem(String[] restricted, Boolean external, Object value, IBqlTable row);
	void DescriptionFieldSelecting(PXCache sender, PXFieldSelectingEventArgs e, Object key, Boolean& deleted);
	void readItem(PXCache sender, Object row, Object key, PXCache& itemCache, Object& item, Boolean& deleted);
	void cacheOnReadItem(GlobalDictionary dict, PXCache foreignCache, Object foreignItem, Boolean isItemDeleted);
	void OnItemCached(PXCache foreignCache, Object foreignItem, Boolean isItemDeleted);
	SubstituteKeyInfo getSubstituteKeyMask(PXCache sender);
	String getDescriptionName(PXCache sender, Nullable`1& length);
	String _GetSlotName(Type type, Byte keysCount);
	void SelfRowSelecting(PXCache sender, PXRowSelectingEventArgs e);
	void SubstituteKeyFieldSelecting(PXCache sender, PXFieldSelectingEventArgs e);
	void SubstituteKeyFieldUpdating(PXCache sender, PXFieldUpdatingEventArgs e);
	void SubstituteKeyCommandPreparing(PXCache sender, PXCommandPreparingEventArgs e);
	Boolean ShouldPrepareCommandForSubstituteKey(PXCommandPreparingEventArgs e);
	void DescriptionFieldCommandPreparing(PXCache sender, PXCommandPreparingEventArgs e);
	void ForeignTableRowPersisted(PXCache sender, PXRowPersistedEventArgs e);
	void ReadDeletedFieldVerifying(PXCache sender, PXFieldVerifyingEventArgs e);
	void SetBqlTable(Type bqlTable);
	List<KeyValuePair`2> GetSelectorFields(Type table);
	void populateFields(PXCache sender, Boolean bypassInit);
	void findFieldsHeaders(PXCache sender);
	void CreateFilter(PXGraph graph);
	Int32 GetHashCode();
	Boolean Equals(Object obj);
	PXEventSubscriberAttribute Clone(PXAttributeLevel attributeLevel);
	Type GetBaseCacheWithTableAttr(Type cache);
	void InjectAttributeDependencies(PXCache cache);
	void InvokeCacheAttached(PXCache cache);
	void GetSubscriber(List<ISubscriber> subscribers);
	T CreateInstance(Object[] constructorArgs);
	PXEventSubscriberAttribute CreateInstance(Type t, Object[] constructorArgs);
	String ToString();
	Attribute[] GetCustomAttributes(MemberInfo element, Type type);
	Attribute[] GetCustomAttributes(MemberInfo element, Type type, Boolean inherit);
	Attribute[] GetCustomAttributes(MemberInfo element);
	Attribute[] GetCustomAttributes(MemberInfo element, Boolean inherit);
	Boolean IsDefined(MemberInfo element, Type attributeType);
	Boolean IsDefined(MemberInfo element, Type attributeType, Boolean inherit);
	Attribute GetCustomAttribute(MemberInfo element, Type attributeType);
	Attribute GetCustomAttribute(MemberInfo element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(ParameterInfo element);
	Attribute[] GetCustomAttributes(ParameterInfo element, Type attributeType);
	Attribute[] GetCustomAttributes(ParameterInfo element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(ParameterInfo element, Boolean inherit);
	Boolean IsDefined(ParameterInfo element, Type attributeType);
	Boolean IsDefined(ParameterInfo element, Type attributeType, Boolean inherit);
	Attribute GetCustomAttribute(ParameterInfo element, Type attributeType);
	Attribute GetCustomAttribute(ParameterInfo element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(Module element, Type attributeType);
	Attribute[] GetCustomAttributes(Module element);
	Attribute[] GetCustomAttributes(Module element, Boolean inherit);
	Attribute[] GetCustomAttributes(Module element, Type attributeType, Boolean inherit);
	Boolean IsDefined(Module element, Type attributeType);
	Boolean IsDefined(Module element, Type attributeType, Boolean inherit);
	Attribute GetCustomAttribute(Module element, Type attributeType);
	Attribute GetCustomAttribute(Module element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(Assembly element, Type attributeType);
	Attribute[] GetCustomAttributes(Assembly element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(Assembly element);
	Attribute[] GetCustomAttributes(Assembly element, Boolean inherit);
	Boolean IsDefined(Assembly element, Type attributeType);
	Boolean IsDefined(Assembly element, Type attributeType, Boolean inherit);
	Attribute GetCustomAttribute(Assembly element, Type attributeType);
	Attribute GetCustomAttribute(Assembly element, Type attributeType, Boolean inherit);
	Boolean Match(Object obj);
	Boolean IsDefaultAttribute();
	void System.Runtime.InteropServices._Attribute.GetTypeInfoCount(UInt32& pcTInfo);
	void System.Runtime.InteropServices._Attribute.GetTypeInfo(UInt32 iTInfo, UInt32 lcid, IntPtr ppTInfo);
	void System.Runtime.InteropServices._Attribute.GetIDsOfNames(Guid& riid, IntPtr rgszNames, UInt32 cNames, UInt32 lcid, IntPtr rgDispId);
	void System.Runtime.InteropServices._Attribute.Invoke(UInt32 dispIdMember, Guid& riid, UInt32 lcid, Int16 wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr);



public class ACConstants
	String DATE_FORMAT_WITH_MS;
	String CLOUD_DEVICE_HUB_ID;
	Int32 DEFAULT_DESC_LENGTH;
	String SHIPMENTS_SCREEN_ID;
	String PRODORDER_SCREEN_ID;



public class ACMessages
	String Prefix;
	String Warning;
	String TypePassedNotIBqlTable;
	String ValueCannotBefound;
	String ValuesLabelsLengthDoNoMatch;
	String DownloadVirtualPrinterTooltip;
	String DownloadTooltip;
	String DownloadMessage;
	String DownloadWindows;
	String DownloadOsx;
	String DownloadVirtualPrinter;
	String CloudPrintCannotFindComputer;
	String CloudPrintCannotFindPrinter;
	String ForeignIDMissing;
	String ForeignIDInactive;
	String ViewWithNameRequired;
	String ViewMissingInGraph;
	String ViewSearchCannotFind;
	String ViewSearchCallError;
	String ViewSelectCallError;
	String ViewSelectCannotFind;
	String ViewMethodMissingInGraph;
	String CannotFindItemType;
	String CannotFindCache;
	String NoCacheForItemType;
	String CannotFindItemTypeInRow;
	String NoSiteMapNodeForScreen;
	String NoGraphTypeOnSiteMapNodeForScreen;
	String CacheCannotBefound;



public class AsgardCoreUtils
	String ATTR_SUFFIX;
	String PREFIX_UDF;
	String Signature(MethodInfo mi);
	String Signature(MethodInfo mi, Func<ParameterInfo,Boolean> paramSelector);
	String Signature(MethodBase mi);
	String Signature(MethodBase mb, Func<ParameterInfo,Boolean> paramSelector);
	Boolean Keep(ParameterInfo pi);
	String FixName(ParameterInfo pi);
	String ParamToString(ParameterInfo pi);
	String GetValueForTrace(Object[] values, Int32 index);
	String GetValueForTrace(Object value);
	String TypeToString(Type type);
	String[] TypesToStrings(Type[] types);
	Type GetItemType(Object result);
	Type GetItemType(IPXResultset rs, Int32 index);
	Type GetItemType(PXResult row, Int32 index);
	IList<Type> GetItemTypes(PXResult pxr);
	IList<Type> GetItemTypes(IPXResultset rs);
	IList<Type> GetItemTypes(IList list);
	IList<Type> GetItemTypes(GenericResult gr);
	IList<String> GetItemTypeNames(IPXResultset rs);
	IList<String> GetItemTypeNames(PXResult res);
	IList<String> GetItemTypeNames(ViewDef viewDef);
	String GetItemTypeName(Object _row_);
	Type GetItemType(Object _row_, Boolean silent);
	Int32 GetTableCount(IPXResultset rs);
	Int32 GetTableCount(GenericResult gr);
	Int32 GetTableCount(PXResult pxr);
	Type GetJoinedItemType(ViewDef viewDef, String joinedTableName);
	Type GetType(String typename);
	Boolean IsAttribute(String fieldName);
	String GetAttributeID(String fieldName);
	Object GetRow(IPXResultset rs, Int32 rowNb, Int32 tableNb);
	Object FirstResultOrDefault(Object result);
	String GetDisplayName(PXView view);
	FieldInfo GetField(Type _graphType, String memberName);
	IEnumerable<FieldInfo> GetFields(Type _graphType, String[] onlyMemberNames);
	IEnumerable<FieldInfo> GetFields(Type type);
	IEnumerable<FieldInfo> GetFieldsInternal(Type type);
	List<Type> GetExtensions(Type tgraph, Boolean checkActive);
	Type GetPXExtensionManagerType();
	Object[] GetKeys(PXCache cache, Object row);
	IDictionary<String,Object> GetKeysAsDict(PXCache cache, Object row);
	E FindCacheExtension(Object row);
	C FindFirst(IEnumerable<T> listOfTs);
	PXGraphExtension FindGraphExtension(PXGraph graph);
	String Decrypt(String source);
	Object GetValueFromCache(PXGraph graph, Object row, Type fieldType);
	Object GetValueFromCache(PXCache cache, Object row, String fieldName);
	void AddRowToCache(PXGraph graph, Object row);
	String GetSource();
	Boolean HasNote(PXGraph graph, Object row);
	CREmployee GetOwner(PXGraph graph, Nullable<Guid> userID);
	CREmployee GetOwner(PXGraph graph, Nullable<Int32> ownerID);
	Nullable<Int32> GetOwnerID(PXGraph graph, Nullable<Guid> userID);
	Guid GetUserID(PXGraph graph);
	Users GetUser(PXGraph graph, Nullable<Guid> userID);
	UserPreferences GetUserPreferences(PXGraph graph, Nullable<Guid> userID);
	Type GetGraphType(PXGraph graph, Type graphTypeField, Object row);
	Boolean IsFilteredGraph(PXGraph graph);
	Boolean IsFilteredGraph(Type graphType);
	String GetFilterView(Type graphType);
	String GetFilterViewInternal(Type graphType);
	String GetFilteredView(Type graphType);
	String GetFilteredViewInternal(Type graphType);
	Boolean IsPXFilter(FieldInfo fi);
	Boolean IsFilteredResult(FieldInfo fi);
	Boolean IsProcessingView(FieldInfo fi);
	Boolean HasPXFilterable(FieldInfo fi);
	Type GetGraphType(Object data, PXEventSubscriberAttribute attr, PXGraph graph, Type fieldType);
	Boolean IsGI(PXGraph graph);



public class AttrControlType
	Nullable<Int32> Text;
	Nullable<Int32> Combo;
	Nullable<Int32> MultiSelectCombo;
	Nullable<Int32> Checkbox;
	Nullable<Int32> Datetime;
	Nullable<Int32> Selector;



public class CoreScribanUtils
	Boolean SkipScribanTypes(ParameterInfo pi);
	String GetArgName(ParameterInfo pi);
	String GetDefaultValue(ParameterInfo pi);



public class EntityContextFactory:IEntityContextFactory
	IEnumerable<CacheEntityItem> EMPTY_LIST;
	String[] ScreenIDs;
	IEntityContext GetContextByScreenID(String screenID);
	IEnumerable<CacheEntityItem> GetLibrariesAsEntityItems(String parent);
	void ClearCaches();
	IEntityContext GetInstanceByScreenID(String screenID);
	IEnumerable<CacheEntityItem> GetLibrariesAsEntityItemsInternal(String parent);
	IEnumerable<CacheEntityItem> GetLibraries(IEnumerable<Type> impls);
	IEnumerable<CacheEntityItem> GetMethods(String funcLibPrefix, Type funcLib);
	void Start();



public class HiddenUtils
	PXSiteMapNode FindSiteMapNodeByScreenIDUnsecure(String screenID);
	PXQueryDescription GetCurrentQueryDescription(PXGenericInqGrph instance);
	Nullable<Guid> GetGenericInquiryIDByScreenID(String screenID);
	void ClearCachesForPXCache();
	void ResetFeaturesForPXExtensionManager();
	String GetParentVariableValue(Nullable<Guid> noteId, String variableName);
	IReadOnlyList<CssVariable> GetCssVariables(String theme);
	IEnumerable<Guid> GetNotUsedDependantsNoteIds(Guid noteId);
	PXGraphExtension[] GetGraphExtensions(PXGraph graph);
	Graph CreateInstance();
	PXGraph CreateInstance(Type graphType);
	void RemoveCurrentAndOptional(List<Type> decomposedOn, Dictionary<String,Type> views);



public class SnakeEnumConverter
	String ToUpperSnakeCase(Nullable<Int32> enumValue);
	String ToUpperSnakeCase(TEnum enumValue);
	String ConvertEnumNameToUpperSnakeCase(String pascalCaseString);
	TEnum FromUpperSnakeCase(String upperSnakeCaseValue);
	String ConvertUpperSnakeCaseToPascalCase(String upperSnakeCaseString);



public class ViewDef:IEquatable<ViewDef>, IEqualityComparer<ViewDef>
	Type GraphType;
	String InternalName;
	String DisplayName;
	String ExternalName;
	String ItemTypeName;
	Type ItemType;
	Type[] ItemTypes;
	Boolean IsUsable;
	String FullName;
	BqlCommand BqlSelect;
	String DependsOn;
	Nullable<Boolean> Detail;
	IList<ViewDependency> Dependencies;
	Boolean CheckIsUsable(PXView view);
	void Add(String dependType, String itemType, String fieldName);
	void Add(ViewDependency dependency);
	Boolean HasAtLeastOneDependency();
	String GetFirstDependency();
	IEnumerable<String> GetDependencies();
	Int32 GetDependencyCount();
	Boolean Equals(Object obj);
	Boolean Equals(ViewDef other);
	Int32 GetHashCode();
	Boolean Equals(ViewDef x, ViewDef y);
	Int32 GetHashCode(ViewDef obj);



public class ViewDependency
	String DependType;
	Type ItemType;
	String ItemTypeName;
	String FieldName;



public class ViewResult:IViewResult
	String FullName;
	String InternalName;
	ViewDef ViewDef;
	PXGraph Graph;
	Int32 TableCount;
	IList<Type> ItemTypes;
	IList<PXCache> Caches;
	Object Result;
	Boolean Detail;
	Boolean HasExtensions;
	Type GetItemType(Int32 tableNo);
	PXCache GetCache(Int32 tableNo);
	IList<Type> GetExtensions(Int32 tableNo);
	IList<Type> GetExtensions(Type itemType);
	PXCache <.ctor>b__2_0(Type it);
	IEnumerable<Type> <get_HasExtensions>b__30_0(Type it);



public class ViewUtils
	PXSelectBase GetDataMember(PXGraph _graph, FieldInfo fi);
	void ClearViews(PXGraph _graph);
	void ClearAllViews();
	ViewDef GetViewDefinition(String graphName, String viewName);
	ViewDef GetViewDefinition(Type _graphType, String viewName);
	IEnumerable<ViewDef> GetViewDefinitions(Type graphType);
	ViewDef GetViewDefinition(PXGraph _graph, String viewName);
	IEnumerable<ViewDef> GetViewDefinitions(PXGraph _graph);
	IEnumerable<ViewDef> GetViewDefinitionsInternal(String _, PXGraph _graph);
	Boolean IsUsable(PXView view);
	Dictionary<String,PXView> GetViews(PXGraph graph);
	PXView GetView(PXGraph graph, String viewName, Boolean silent);
	Boolean IsDetail(PXGraph graph, ViewDef parentViewDef, ViewDef viewDef);
	Boolean IsParent(PXCache parentCache, PXCache childCache);
	IViewResult GetViewRow(PXGraph docGraph, ViewDef viewDef);
	IEnumerable<IViewResult> GetViewRows(PXGraph docGraph, IEnumerable<ViewDef> viewDefs);
	void TryRedirect(String graphTypeName, String keys);
	PXGraph GetGraph(String graphTypeName);
	ValueTuple<PXGraph,Object> GetGraphAndDoc(String graphTypeName, String keys);
	Object SearchSpecificDocument(PXGraph graph, String keysAsStr);
	void SetDocumentCurrent(PXGraph graph, Object document);
	PXEntryStatus GetStatus(PXGraph graph, String viewName);
	String[] GetFieldNames(PXGraph graph, String viewName);
	Type GetItemType(PXGraph graph, String viewName);
	Object ViewSearch(PXGraph graph, String viewName, Object[] keys);
	Boolean TakesField(Type type);
	Object ViewSelect(PXGraph graph, String viewName);
	String GetRowName(ViewDef view, Type itemType, Boolean shortSuffix);
	String GetRowName(ViewDef view, String alias);
	String GetViewName(ViewDef view, Type itemType, Boolean shortSuffix);
	String GetViewName(String internalName, String viewItemType, Type itemType, Boolean shortSuffix);
	String GetViewName(ViewDef view, String suffix);
	String GetIteratorName(ViewDef view, Type itemType, Boolean shortSuffix);
	String GetIteratorName(ViewDef view, String alias);
	String GetSuffix(String viewItemType, Type itemType, Boolean shortSuffix);
	String GetLongSuffix(String itemTypeName);
	String GetSuffix(String suffix);
	IEnumerable<PXFieldState> GetFields(Type graphType, Type itemType);
	IEnumerable<PXFieldState> GetFieldsInternal(String key);
	Boolean HasJoins(PXGraph graph, String viewName);
	IEnumerable<Type> GetJoinedTypes(Type graphType, String viewName);
	IEnumerable<Type> GetJoinedTypesInternal(String key);
	PXSelectBase GetSelectFromGraph(PXGraph graph, String viewName, Boolean silent);
	PXCache GetCache(PXGraph graph, String viewName);
	IList<Type> GetItemTypes(PXView view, Object result);
	Boolean IsTable(Type type);
	PXCache GetCache(PXGraph graph, Object row);
	PXCache GetCache(PXGraph graph, Type it);
	IList<Type> GetExtensions(PXGraph graph, Type itemType);



public class IAcuScreenBased
	String GraphType;
	String ScreenID;



public class IEntityContext:IAcuScreenBased
	Type RealGraphType;
	PXGraph Graph;
	IEnumerable<ViewDef> ViewDefinitions;
	IEnumerable<PXCacheInfo> CacheInfos;
	String PrimaryView;
	Type PrimaryItemType;
	String PrimaryDisplayName;
	Nullable<Guid> GIDesignID;
	PXQueryDescription GIQueryDesc;
	GIDescription GIDesc;
	Boolean IsGIScreen;
	IEnumerable<PXFieldState> GetFieldStates(String viewName);
	IEnumerable<CacheEntityItem> GetEntityItemsImplByScreen(String parent);
	String GetFieldDescription(String viewName, String exprValue);
	void ClearCaches();



public class IEntityContextFactory
	String[] ScreenIDs;
	IEntityContext GetContextByScreenID(String screenID);
	IEnumerable<CacheEntityItem> GetLibrariesAsEntityItems(String parent);
	void ClearCaches();



public class IViewResult
	String FullName;
	String InternalName;
	ViewDef ViewDef;
	PXGraph Graph;
	IList<Type> ItemTypes;
	IList<PXCache> Caches;
	Boolean HasExtensions;
	Object Result;
	Boolean Detail;
	Int32 TableCount;
	Type GetItemType(Int32 tableNo);
	PXCache GetCache(Int32 tableNo);
	IList<Type> GetExtensions(Int32 tableNo);
	IList<Type> GetExtensions(Type itemType);



public class AcuRenderableChild:IRenderableChild<T>, IRenderableChild
	Nullable<Guid> ParentID;
	T ChildID;
	Nullable<Int32> LineNbr;
	Nullable<Int32> SortOrder;
	Nullable<Boolean> Active;
	Nullable<DateTime> CreatedDateTime;
	Nullable<DateTime> LastModifiedDateTime;
	Nullable<Guid> ID;



public class AcuRenderableConfig:IRenderableConfig
	Nullable<Guid> ID;
	String Name;
	String Description;
	Nullable<Boolean> Active;
	Nullable<DateTime> CreatedDateTime;
	Nullable<DateTime> LastModifiedDateTime;



public class AcuRenderableParent:AcuRenderableConfig, IRenderableConfig, IParent
	Nullable<Boolean> IsComposite;
	Nullable<Guid> ID;
	String Name;
	String Description;
	Nullable<Boolean> Active;
	Nullable<DateTime> CreatedDateTime;
	Nullable<DateTime> LastModifiedDateTime;



public class AcuRuleDriven:AcuRenderableChild<Nullable`1>, IRenderableChild<Nullable`1>, IRenderableChild, IRuleDriven
	Nullable<Guid> RuleID;
	Nullable<Boolean> ReverseRule;
	Nullable<Int32> BAccountID;
	Nullable<Boolean> DoThrow;
	String Message;
	Nullable<Guid> ParentID;
	Nullable<Guid> ChildID;
	Nullable<Int32> LineNbr;
	Nullable<Int32> SortOrder;
	Nullable<Boolean> Active;
	Nullable<DateTime> CreatedDateTime;
	Nullable<DateTime> LastModifiedDateTime;
	Nullable<Guid> ID;



public class Printer:AcuRenderableConfig, IRenderableConfig, IAcuPrinter, IPrinter, ICloudPrinter, IPrintNodePrinter, IPrintNodeObject, IEpsonPrinter, ILabelPrinter
	String PrinterType;
	Nullable<Boolean> IsRendering;
	String Drive;
	Nullable<Boolean> SupportsLongFiles;
	Nullable<Guid> PrintStationID;
	Nullable<Guid> FormatID;
	Nullable<Guid> MarginID;
	Nullable<Int32> Encoding;
	Nullable<Boolean> PushFonts;
	Nullable<Boolean> IsEpson;
	String MediaType;
	String MediaForm;
	String MediaSource;
	String MediaShape;
	String EdgeDetection;
	String PrintMode;
	Nullable<Int32> ContentType;
	Nullable<Guid> AcuPrinterID;
	Nullable<Int32> PrintNodePrinterID;
	Nullable<Int32> PrintNodeComputerID;
	String PrintNodeAPIKey;
	String FieldName;
	String PrinterState;
	String Capabilities;
	Nullable<Guid> ID;
	String Name;
	String Description;
	Nullable<Boolean> Active;
	Nullable<DateTime> CreatedDateTime;
	Nullable<DateTime> LastModifiedDateTime;



public class MobileUtils
	Boolean IsMobile(Type tgraph);
	Boolean IsMobile(PXGraph graph);
	Type GetRegularGraphFromMobile(Type tgraph);



public class EzeepHelper
	String DEFAULT_PRINT_URL;
	String DEFAULT_ENTITY_URL;
	String DEFAULT_API_KEY;
	ValueTuple<RestClient,RestRequest> GetClientRequest(String resource, String specificAPIKey, Method method, Boolean doAuth);
	T HandleResponse(RestClient restClient, RestRequest request, RestResponse response, HttpStatusCode[] expectedCodes);



public class ALPrintNodeAccountIDAttribute:ALAggregateAttribute, _Attribute, IPXInterfaceField, IPXCommandPreparingSubscriber, IPXRowSelectingSubscriber
	Boolean IsDirty;
	Type BqlField;
	Boolean CacheGlobal;
	Object Constant;
	Type DescriptionField;
	Type SubstituteKey;
	Boolean DirtyRead;
	String DisplayName;
	Boolean Enabled;
	Boolean IsReadOnly;
	PXErrorHandling ErrorHandling;
	PXErrorLevel ErrorLevel;
	String ErrorText;
	Object ErrorValue;
	String FieldClass;
	Boolean Filterable;
	Boolean IsDBField;
	Boolean IsFixed;
	Boolean IsKey;
	PXCacheRights MapEnableRights;
	PXCacheRights MapViewRights;
	PXPersistingCheck PersistingCheck;
	Boolean Required;
	Int32 TabOrder;
	Boolean ValidateValue;
	Boolean SuppressVerify;
	Boolean ViewRights;
	PXUIVisibility Visibility;
	Boolean Visible;
	Type BqlTable;
	String FieldName;
	Int32 FieldOrdinal;
	Type CacheExtensionType;
	PXAttributeLevel AttributeLevel;
	Object TypeId;
	void CommandPreparing(PXCache sender, PXCommandPreparingEventArgs e);
	void ForceEnabled();
	void GetSubscriber(List<ISubscriber> subscribers);
	void Initialize();
	void RowSelecting(PXCache sender, PXRowSelectingEventArgs e);
	PXEventSubscriberAttribute[] GetAttributes();
	PXEventSubscriberAttribute[] GetAggregatedAttributes();
	T GetAttribute();
	PXEventSubscriberAttribute Clone(PXAttributeLevel attributeLevel);
	void InjectAttributeDependencies(PXCache cache);
	void CacheAttached(PXCache sender);
	void SetBqlTable(Type bqlTable);
	Boolean ChildrenAttributesComeFirstFor();
	Int32 GetHashCode();
	Boolean Equals(Object obj);
	Type GetBaseCacheWithTableAttr(Type cache);
	void InvokeCacheAttached(PXCache cache);
	T CreateInstance(Object[] constructorArgs);
	PXEventSubscriberAttribute CreateInstance(Type t, Object[] constructorArgs);
	String ToString();
	Attribute[] GetCustomAttributes(MemberInfo element, Type type);
	Attribute[] GetCustomAttributes(MemberInfo element, Type type, Boolean inherit);
	Attribute[] GetCustomAttributes(MemberInfo element);
	Attribute[] GetCustomAttributes(MemberInfo element, Boolean inherit);
	Boolean IsDefined(MemberInfo element, Type attributeType);
	Boolean IsDefined(MemberInfo element, Type attributeType, Boolean inherit);
	Attribute GetCustomAttribute(MemberInfo element, Type attributeType);
	Attribute GetCustomAttribute(MemberInfo element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(ParameterInfo element);
	Attribute[] GetCustomAttributes(ParameterInfo element, Type attributeType);
	Attribute[] GetCustomAttributes(ParameterInfo element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(ParameterInfo element, Boolean inherit);
	Boolean IsDefined(ParameterInfo element, Type attributeType);
	Boolean IsDefined(ParameterInfo element, Type attributeType, Boolean inherit);
	Attribute GetCustomAttribute(ParameterInfo element, Type attributeType);
	Attribute GetCustomAttribute(ParameterInfo element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(Module element, Type attributeType);
	Attribute[] GetCustomAttributes(Module element);
	Attribute[] GetCustomAttributes(Module element, Boolean inherit);
	Attribute[] GetCustomAttributes(Module element, Type attributeType, Boolean inherit);
	Boolean IsDefined(Module element, Type attributeType);
	Boolean IsDefined(Module element, Type attributeType, Boolean inherit);
	Attribute GetCustomAttribute(Module element, Type attributeType);
	Attribute GetCustomAttribute(Module element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(Assembly element, Type attributeType);
	Attribute[] GetCustomAttributes(Assembly element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(Assembly element);
	Attribute[] GetCustomAttributes(Assembly element, Boolean inherit);
	Boolean IsDefined(Assembly element, Type attributeType);
	Boolean IsDefined(Assembly element, Type attributeType, Boolean inherit);
	Attribute GetCustomAttribute(Assembly element, Type attributeType);
	Attribute GetCustomAttribute(Assembly element, Type attributeType, Boolean inherit);
	Boolean Match(Object obj);
	Boolean IsDefaultAttribute();
	void System.Runtime.InteropServices._Attribute.GetTypeInfoCount(UInt32& pcTInfo);
	void System.Runtime.InteropServices._Attribute.GetTypeInfo(UInt32 iTInfo, UInt32 lcid, IntPtr ppTInfo);
	void System.Runtime.InteropServices._Attribute.GetIDsOfNames(Guid& riid, IntPtr rgszNames, UInt32 cNames, UInt32 lcid, IntPtr rgDispId);
	void System.Runtime.InteropServices._Attribute.Invoke(UInt32 dispIdMember, Guid& riid, UInt32 lcid, Int16 wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr);



public class ALPrintNodeComputerIDAttribute:ALAggregateAttribute, _Attribute, IPXInterfaceField, IPXCommandPreparingSubscriber, IPXRowSelectingSubscriber
	Boolean IsDirty;
	Type BqlField;
	Boolean CacheGlobal;
	Object Constant;
	Type DescriptionField;
	Type SubstituteKey;
	Boolean DirtyRead;
	String DisplayName;
	Boolean Enabled;
	Boolean IsReadOnly;
	PXErrorHandling ErrorHandling;
	PXErrorLevel ErrorLevel;
	String ErrorText;
	Object ErrorValue;
	String FieldClass;
	Boolean Filterable;
	Boolean IsDBField;
	Boolean IsFixed;
	Boolean IsKey;
	PXCacheRights MapEnableRights;
	PXCacheRights MapViewRights;
	PXPersistingCheck PersistingCheck;
	Boolean Required;
	Int32 TabOrder;
	Boolean ValidateValue;
	Boolean SuppressVerify;
	Boolean ViewRights;
	PXUIVisibility Visibility;
	Boolean Visible;
	Type BqlTable;
	String FieldName;
	Int32 FieldOrdinal;
	Type CacheExtensionType;
	PXAttributeLevel AttributeLevel;
	Object TypeId;
	void CommandPreparing(PXCache sender, PXCommandPreparingEventArgs e);
	void ForceEnabled();
	void GetSubscriber(List<ISubscriber> subscribers);
	void Initialize();
	void RowSelecting(PXCache sender, PXRowSelectingEventArgs e);
	PXEventSubscriberAttribute[] GetAttributes();
	PXEventSubscriberAttribute[] GetAggregatedAttributes();
	T GetAttribute();
	PXEventSubscriberAttribute Clone(PXAttributeLevel attributeLevel);
	void InjectAttributeDependencies(PXCache cache);
	void CacheAttached(PXCache sender);
	void SetBqlTable(Type bqlTable);
	Boolean ChildrenAttributesComeFirstFor();
	Int32 GetHashCode();
	Boolean Equals(Object obj);
	Type GetBaseCacheWithTableAttr(Type cache);
	void InvokeCacheAttached(PXCache cache);
	T CreateInstance(Object[] constructorArgs);
	PXEventSubscriberAttribute CreateInstance(Type t, Object[] constructorArgs);
	String ToString();
	Attribute[] GetCustomAttributes(MemberInfo element, Type type);
	Attribute[] GetCustomAttributes(MemberInfo element, Type type, Boolean inherit);
	Attribute[] GetCustomAttributes(MemberInfo element);
	Attribute[] GetCustomAttributes(MemberInfo element, Boolean inherit);
	Boolean IsDefined(MemberInfo element, Type attributeType);
	Boolean IsDefined(MemberInfo element, Type attributeType, Boolean inherit);
	Attribute GetCustomAttribute(MemberInfo element, Type attributeType);
	Attribute GetCustomAttribute(MemberInfo element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(ParameterInfo element);
	Attribute[] GetCustomAttributes(ParameterInfo element, Type attributeType);
	Attribute[] GetCustomAttributes(ParameterInfo element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(ParameterInfo element, Boolean inherit);
	Boolean IsDefined(ParameterInfo element, Type attributeType);
	Boolean IsDefined(ParameterInfo element, Type attributeType, Boolean inherit);
	Attribute GetCustomAttribute(ParameterInfo element, Type attributeType);
	Attribute GetCustomAttribute(ParameterInfo element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(Module element, Type attributeType);
	Attribute[] GetCustomAttributes(Module element);
	Attribute[] GetCustomAttributes(Module element, Boolean inherit);
	Attribute[] GetCustomAttributes(Module element, Type attributeType, Boolean inherit);
	Boolean IsDefined(Module element, Type attributeType);
	Boolean IsDefined(Module element, Type attributeType, Boolean inherit);
	Attribute GetCustomAttribute(Module element, Type attributeType);
	Attribute GetCustomAttribute(Module element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(Assembly element, Type attributeType);
	Attribute[] GetCustomAttributes(Assembly element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(Assembly element);
	Attribute[] GetCustomAttributes(Assembly element, Boolean inherit);
	Boolean IsDefined(Assembly element, Type attributeType);
	Boolean IsDefined(Assembly element, Type attributeType, Boolean inherit);
	Attribute GetCustomAttribute(Assembly element, Type attributeType);
	Attribute GetCustomAttribute(Assembly element, Type attributeType, Boolean inherit);
	Boolean Match(Object obj);
	Boolean IsDefaultAttribute();
	void System.Runtime.InteropServices._Attribute.GetTypeInfoCount(UInt32& pcTInfo);
	void System.Runtime.InteropServices._Attribute.GetTypeInfo(UInt32 iTInfo, UInt32 lcid, IntPtr ppTInfo);
	void System.Runtime.InteropServices._Attribute.GetIDsOfNames(Guid& riid, IntPtr rgszNames, UInt32 cNames, UInt32 lcid, IntPtr rgDispId);
	void System.Runtime.InteropServices._Attribute.Invoke(UInt32 dispIdMember, Guid& riid, UInt32 lcid, Int16 wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr);



public class ALPrintNodeComputerIDStandaloneAttribute:ALAggregateAttribute, _Attribute, IPXInterfaceField, IPXCommandPreparingSubscriber, IPXRowSelectingSubscriber
	Boolean IsDirty;
	Type BqlField;
	Boolean CacheGlobal;
	Object Constant;
	Type DescriptionField;
	Type SubstituteKey;
	Boolean DirtyRead;
	String DisplayName;
	Boolean Enabled;
	Boolean IsReadOnly;
	PXErrorHandling ErrorHandling;
	PXErrorLevel ErrorLevel;
	String ErrorText;
	Object ErrorValue;
	String FieldClass;
	Boolean Filterable;
	Boolean IsDBField;
	Boolean IsFixed;
	Boolean IsKey;
	PXCacheRights MapEnableRights;
	PXCacheRights MapViewRights;
	PXPersistingCheck PersistingCheck;
	Boolean Required;
	Int32 TabOrder;
	Boolean ValidateValue;
	Boolean SuppressVerify;
	Boolean ViewRights;
	PXUIVisibility Visibility;
	Boolean Visible;
	Type BqlTable;
	String FieldName;
	Int32 FieldOrdinal;
	Type CacheExtensionType;
	PXAttributeLevel AttributeLevel;
	Object TypeId;
	void CommandPreparing(PXCache sender, PXCommandPreparingEventArgs e);
	void ForceEnabled();
	void GetSubscriber(List<ISubscriber> subscribers);
	void Initialize();
	void RowSelecting(PXCache sender, PXRowSelectingEventArgs e);
	PXEventSubscriberAttribute[] GetAttributes();
	PXEventSubscriberAttribute[] GetAggregatedAttributes();
	T GetAttribute();
	PXEventSubscriberAttribute Clone(PXAttributeLevel attributeLevel);
	void InjectAttributeDependencies(PXCache cache);
	void CacheAttached(PXCache sender);
	void SetBqlTable(Type bqlTable);
	Boolean ChildrenAttributesComeFirstFor();
	Int32 GetHashCode();
	Boolean Equals(Object obj);
	Type GetBaseCacheWithTableAttr(Type cache);
	void InvokeCacheAttached(PXCache cache);
	T CreateInstance(Object[] constructorArgs);
	PXEventSubscriberAttribute CreateInstance(Type t, Object[] constructorArgs);
	String ToString();
	Attribute[] GetCustomAttributes(MemberInfo element, Type type);
	Attribute[] GetCustomAttributes(MemberInfo element, Type type, Boolean inherit);
	Attribute[] GetCustomAttributes(MemberInfo element);
	Attribute[] GetCustomAttributes(MemberInfo element, Boolean inherit);
	Boolean IsDefined(MemberInfo element, Type attributeType);
	Boolean IsDefined(MemberInfo element, Type attributeType, Boolean inherit);
	Attribute GetCustomAttribute(MemberInfo element, Type attributeType);
	Attribute GetCustomAttribute(MemberInfo element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(ParameterInfo element);
	Attribute[] GetCustomAttributes(ParameterInfo element, Type attributeType);
	Attribute[] GetCustomAttributes(ParameterInfo element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(ParameterInfo element, Boolean inherit);
	Boolean IsDefined(ParameterInfo element, Type attributeType);
	Boolean IsDefined(ParameterInfo element, Type attributeType, Boolean inherit);
	Attribute GetCustomAttribute(ParameterInfo element, Type attributeType);
	Attribute GetCustomAttribute(ParameterInfo element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(Module element, Type attributeType);
	Attribute[] GetCustomAttributes(Module element);
	Attribute[] GetCustomAttributes(Module element, Boolean inherit);
	Attribute[] GetCustomAttributes(Module element, Type attributeType, Boolean inherit);
	Boolean IsDefined(Module element, Type attributeType);
	Boolean IsDefined(Module element, Type attributeType, Boolean inherit);
	Attribute GetCustomAttribute(Module element, Type attributeType);
	Attribute GetCustomAttribute(Module element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(Assembly element, Type attributeType);
	Attribute[] GetCustomAttributes(Assembly element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(Assembly element);
	Attribute[] GetCustomAttributes(Assembly element, Boolean inherit);
	Boolean IsDefined(Assembly element, Type attributeType);
	Boolean IsDefined(Assembly element, Type attributeType, Boolean inherit);
	Attribute GetCustomAttribute(Assembly element, Type attributeType);
	Attribute GetCustomAttribute(Assembly element, Type attributeType, Boolean inherit);
	Boolean Match(Object obj);
	Boolean IsDefaultAttribute();
	void System.Runtime.InteropServices._Attribute.GetTypeInfoCount(UInt32& pcTInfo);
	void System.Runtime.InteropServices._Attribute.GetTypeInfo(UInt32 iTInfo, UInt32 lcid, IntPtr ppTInfo);
	void System.Runtime.InteropServices._Attribute.GetIDsOfNames(Guid& riid, IntPtr rgszNames, UInt32 cNames, UInt32 lcid, IntPtr rgDispId);
	void System.Runtime.InteropServices._Attribute.Invoke(UInt32 dispIdMember, Guid& riid, UInt32 lcid, Int16 wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr);



public class ALPrintNodeComputerSelectorAttribute:PXCustomSelectorAttribute, _Attribute, IPXFieldVerifyingSubscriber, IPXFieldSelectingSubscriber, IPXDependsOnFields
	Boolean ValidateValue;
	Boolean IsDirty;
	Boolean ExcludeFromReferenceGeneratingProcess;
	Boolean IsPrimaryViewCompatible;
	Boolean ShowWarningForNotExistsOnSelect;
	String CustomMessageElementDoesntExist;
	String CustomMessageValueDoesntExist;
	String CustomMessageElementDoesntExistOrNoRights;
	String CustomMessageValueDoesntExistOrNoRights;
	Boolean CacheGlobal;
	Type DescriptionField;
	String DescriptionDisplayName;
	Boolean ShowPopupWarning;
	Boolean ShowPopupMessage;
	Type FilterEntity;
	Type SubstituteKey;
	Type Field;
	Boolean DirtyRead;
	Boolean Filterable;
	String[] Headers;
	Type ValueField;
	PXSelectorMode SelectorMode;
	BqlCommand PrimarySelect;
	BqlCommand OriginalSelect;
	Int32 ParsCount;
	Boolean SuppressUnconditionalSelect;
	String ViewName;
	PXAttributeLevel AttributeLevel;
	Type BqlTable;
	Type CacheExtensionType;
	String FieldName;
	Int32 FieldOrdinal;
	Object TypeId;
	void FieldVerifying(PXCache sender, PXFieldVerifyingEventArgs e);
	IEnumerable GetRecords();
	IEnumerable<IPrintNodeComputer> GetComputers(String apiKey);
	void DescriptionFieldSelecting(PXCache sender, PXFieldSelectingEventArgs e, String alias);
	String GetAPIKey(Object data);
	PXView GetView(PXCache cache, BqlCommand select, Boolean isReadOnly);
	PXView GetUnconditionalView(PXCache cache);
	void CacheAttached(PXCache sender);
	void CreateView(PXCache sender);
	void EmitDescriptionFieldAlias(PXCache sender, String alias);
	void EmitColumnForDescriptionField(PXCache sender);
	BqlCommand GetSelect();
	void SubscribeToFeatureSet();
	void SetFieldList(Type[] fieldList);
	BqlCommand WhereAnd(PXCache sender, Type whr);
	String GenerateViewName();
	BqlCommand BuildNaturalSelect(Boolean cacheGlobal, Type substituteKey);
	Object SelectSingleBound(PXView view, Object[] currents, Object[] pars);
	Object SelectSingle(PXView view, Object[] pars);
	Object SelectSingle(PXCache cache, Object data, String field, Object value);
	Object SelectSingle(PXCache cache, Object data, String field);
	Object[] MakeParameters(Object lastParameter, Boolean includeLookupJoins);
	ViewWithParameters GetViewWithParameters(PXCache cache, Object lastParameter, Boolean includeLookupJoins);
	Object Select(PXCache cache, Object data, String field);
	Object SelectFirst(PXCache cache, Object data);
	Object SelectFirst(PXCache cache, Object data, String field);
	Object SelectLast(PXCache cache, Object data);
	Object SelectLast(PXCache cache, Object data, String field);
	Object Select(PXCache cache, Object data, String field, Object value);
	GlobalDictionary GetGlobalCache();
	void AppendOtherValues(Dictionary<String,Object> values, PXCache cache, Object row);
	Object CreateGlobalCacheKey(PXCache cache, Object row, Object keyValue);
	Boolean CanCacheGlobal(PXCache foreignCache);
	Object GetItem(PXCache cache, PXSelectorAttribute attr, Object data, Object key);
	Object GetItem(PXCache cache, PXSelectorAttribute attr, Object data, Object key, Boolean unconditionally);
	IBqlTable GetReferencedDacWithoutSelectorCacheUsage(PXCache cache, Object row, Object foreignKeyValue);
	Object GetItemUnconditionally(PXCache cache, PXSelectorAttribute attr, Object key);
	void ClearGlobalCache();
	void ClearGlobalCache(Byte keysCount);
	void ClearGlobalCache(Type table);
	void ClearGlobalCache(Type table, Byte keysCount);
	Object GetField(PXCache cache, Object data, String field, Object value, String foreignField);
	void CheckIntegrityAndPutGlobal(GlobalDictionary globalDictionary, PXCache foreignCache, String foreignField, Object foreignRow, Object ownKey, Boolean isRowDeleted);
	Type GetItemType(PXCache cache, String field);
	List<Object> SelectAll(PXCache cache, Object data);
	List<Object> SelectAll(PXCache cache, String fieldname, Object data);
	Object Select(PXCache cache, Object data);
	Object Select(PXCache cache, Object data, Object value);
	void SetColumns(PXCache cache, Object data, String field, String[] fieldList, String[] headerList);
	void SetColumns(PXCache cache, String field, String[] fieldList, String[] headerList);
	void SetColumns(String[] fieldList, String[] headerList);
	void SetColumns(PXCache cache, Object data, Type[] fieldList, String[] headerList);
	void SetColumns(PXCache cache, Type[] fieldList, String[] headerList);
	void StoreCached(PXCache cache, Object data, Object item);
	void StoreCached(PXCache cache, Object data, Object item, Boolean clearCache);
	void StoreResult(PXCache cache, Object data, IBqlTable selectResult);
	void StoreResult(PXCache cache, IBqlTable selectResult);
	void StoreResult(PXCache cache, Object data, List<Object> selectResult);
	void CheckAndRaiseForeignKeyException(PXCache sender, Object Row, Type fieldType, Type searchType, String customMessage);
	ISet<Type> GetDependencies(PXCache sender);
	Boolean SplitFieldNames(String fieldName, String& outerField, String& innerField);
	void Verify(PXCache sender, PXFieldVerifyingEventArgs e, Object& item);
	String[] hasRestrictedAccess(PXCache sender, BqlCommand command, Object row);
	void throwNoItem(String[] restricted, Boolean external, Object value);
	void throwNoItem(String[] restricted, Boolean external, Object value, IBqlTable row);
	void FieldSelecting(PXCache sender, PXFieldSelectingEventArgs e);
	void DescriptionFieldSelecting(PXCache sender, PXFieldSelectingEventArgs e, Object key, Boolean& deleted);
	void readItem(PXCache sender, Object row, Object key, PXCache& itemCache, Object& item, Boolean& deleted);
	void cacheOnReadItem(GlobalDictionary dict, PXCache foreignCache, Object foreignItem, Boolean isItemDeleted);
	void OnItemCached(PXCache foreignCache, Object foreignItem, Boolean isItemDeleted);
	SubstituteKeyInfo getSubstituteKeyMask(PXCache sender);
	String getDescriptionName(PXCache sender, Nullable`1& length);
	String _GetSlotName(Type type, Byte keysCount);
	void SelfRowSelecting(PXCache sender, PXRowSelectingEventArgs e);
	void SubstituteKeyFieldSelecting(PXCache sender, PXFieldSelectingEventArgs e);
	void SubstituteKeyFieldUpdating(PXCache sender, PXFieldUpdatingEventArgs e);
	void SubstituteKeyCommandPreparing(PXCache sender, PXCommandPreparingEventArgs e);
	Boolean ShouldPrepareCommandForSubstituteKey(PXCommandPreparingEventArgs e);
	void DescriptionFieldCommandPreparing(PXCache sender, PXCommandPreparingEventArgs e);
	void ForeignTableRowPersisted(PXCache sender, PXRowPersistedEventArgs e);
	void ReadDeletedFieldVerifying(PXCache sender, PXFieldVerifyingEventArgs e);
	void SetBqlTable(Type bqlTable);
	List<KeyValuePair`2> GetSelectorFields(Type table);
	void populateFields(PXCache sender, Boolean bypassInit);
	void findFieldsHeaders(PXCache sender);
	void CreateFilter(PXGraph graph);
	Int32 GetHashCode();
	Boolean Equals(Object obj);
	PXEventSubscriberAttribute Clone(PXAttributeLevel attributeLevel);
	Type GetBaseCacheWithTableAttr(Type cache);
	void InjectAttributeDependencies(PXCache cache);
	void InvokeCacheAttached(PXCache cache);
	void GetSubscriber(List<ISubscriber> subscribers);
	T CreateInstance(Object[] constructorArgs);
	PXEventSubscriberAttribute CreateInstance(Type t, Object[] constructorArgs);
	String ToString();
	Attribute[] GetCustomAttributes(MemberInfo element, Type type);
	Attribute[] GetCustomAttributes(MemberInfo element, Type type, Boolean inherit);
	Attribute[] GetCustomAttributes(MemberInfo element);
	Attribute[] GetCustomAttributes(MemberInfo element, Boolean inherit);
	Boolean IsDefined(MemberInfo element, Type attributeType);
	Boolean IsDefined(MemberInfo element, Type attributeType, Boolean inherit);
	Attribute GetCustomAttribute(MemberInfo element, Type attributeType);
	Attribute GetCustomAttribute(MemberInfo element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(ParameterInfo element);
	Attribute[] GetCustomAttributes(ParameterInfo element, Type attributeType);
	Attribute[] GetCustomAttributes(ParameterInfo element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(ParameterInfo element, Boolean inherit);
	Boolean IsDefined(ParameterInfo element, Type attributeType);
	Boolean IsDefined(ParameterInfo element, Type attributeType, Boolean inherit);
	Attribute GetCustomAttribute(ParameterInfo element, Type attributeType);
	Attribute GetCustomAttribute(ParameterInfo element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(Module element, Type attributeType);
	Attribute[] GetCustomAttributes(Module element);
	Attribute[] GetCustomAttributes(Module element, Boolean inherit);
	Attribute[] GetCustomAttributes(Module element, Type attributeType, Boolean inherit);
	Boolean IsDefined(Module element, Type attributeType);
	Boolean IsDefined(Module element, Type attributeType, Boolean inherit);
	Attribute GetCustomAttribute(Module element, Type attributeType);
	Attribute GetCustomAttribute(Module element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(Assembly element, Type attributeType);
	Attribute[] GetCustomAttributes(Assembly element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(Assembly element);
	Attribute[] GetCustomAttributes(Assembly element, Boolean inherit);
	Boolean IsDefined(Assembly element, Type attributeType);
	Boolean IsDefined(Assembly element, Type attributeType, Boolean inherit);
	Attribute GetCustomAttribute(Assembly element, Type attributeType);
	Attribute GetCustomAttribute(Assembly element, Type attributeType, Boolean inherit);
	Boolean Match(Object obj);
	Boolean IsDefaultAttribute();
	void System.Runtime.InteropServices._Attribute.GetTypeInfoCount(UInt32& pcTInfo);
	void System.Runtime.InteropServices._Attribute.GetTypeInfo(UInt32 iTInfo, UInt32 lcid, IntPtr ppTInfo);
	void System.Runtime.InteropServices._Attribute.GetIDsOfNames(Guid& riid, IntPtr rgszNames, UInt32 cNames, UInt32 lcid, IntPtr rgDispId);
	void System.Runtime.InteropServices._Attribute.Invoke(UInt32 dispIdMember, Guid& riid, UInt32 lcid, Int16 wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr);



public class ALPrintNodeComputerSelectorStandaloneAttribute:PXCustomSelectorAttribute, _Attribute, IPXFieldVerifyingSubscriber, IPXFieldSelectingSubscriber, IPXDependsOnFields
	Boolean ValidateValue;
	Boolean IsDirty;
	Boolean ExcludeFromReferenceGeneratingProcess;
	Boolean IsPrimaryViewCompatible;
	Boolean ShowWarningForNotExistsOnSelect;
	String CustomMessageElementDoesntExist;
	String CustomMessageValueDoesntExist;
	String CustomMessageElementDoesntExistOrNoRights;
	String CustomMessageValueDoesntExistOrNoRights;
	Boolean CacheGlobal;
	Type DescriptionField;
	String DescriptionDisplayName;
	Boolean ShowPopupWarning;
	Boolean ShowPopupMessage;
	Type FilterEntity;
	Type SubstituteKey;
	Type Field;
	Boolean DirtyRead;
	Boolean Filterable;
	String[] Headers;
	Type ValueField;
	PXSelectorMode SelectorMode;
	BqlCommand PrimarySelect;
	BqlCommand OriginalSelect;
	Int32 ParsCount;
	Boolean SuppressUnconditionalSelect;
	String ViewName;
	PXAttributeLevel AttributeLevel;
	Type BqlTable;
	Type CacheExtensionType;
	String FieldName;
	Int32 FieldOrdinal;
	Object TypeId;
	void FieldVerifying(PXCache sender, PXFieldVerifyingEventArgs e);
	IEnumerable GetRecords();
	void DescriptionFieldSelecting(PXCache sender, PXFieldSelectingEventArgs e, String alias);
	PXView GetView(PXCache cache, BqlCommand select, Boolean isReadOnly);
	PXView GetUnconditionalView(PXCache cache);
	void CacheAttached(PXCache sender);
	void CreateView(PXCache sender);
	void EmitDescriptionFieldAlias(PXCache sender, String alias);
	void EmitColumnForDescriptionField(PXCache sender);
	BqlCommand GetSelect();
	void SubscribeToFeatureSet();
	void SetFieldList(Type[] fieldList);
	BqlCommand WhereAnd(PXCache sender, Type whr);
	String GenerateViewName();
	BqlCommand BuildNaturalSelect(Boolean cacheGlobal, Type substituteKey);
	Object SelectSingleBound(PXView view, Object[] currents, Object[] pars);
	Object SelectSingle(PXView view, Object[] pars);
	Object SelectSingle(PXCache cache, Object data, String field, Object value);
	Object SelectSingle(PXCache cache, Object data, String field);
	Object[] MakeParameters(Object lastParameter, Boolean includeLookupJoins);
	ViewWithParameters GetViewWithParameters(PXCache cache, Object lastParameter, Boolean includeLookupJoins);
	Object Select(PXCache cache, Object data, String field);
	Object SelectFirst(PXCache cache, Object data);
	Object SelectFirst(PXCache cache, Object data, String field);
	Object SelectLast(PXCache cache, Object data);
	Object SelectLast(PXCache cache, Object data, String field);
	Object Select(PXCache cache, Object data, String field, Object value);
	GlobalDictionary GetGlobalCache();
	void AppendOtherValues(Dictionary<String,Object> values, PXCache cache, Object row);
	Object CreateGlobalCacheKey(PXCache cache, Object row, Object keyValue);
	Boolean CanCacheGlobal(PXCache foreignCache);
	Object GetItem(PXCache cache, PXSelectorAttribute attr, Object data, Object key);
	Object GetItem(PXCache cache, PXSelectorAttribute attr, Object data, Object key, Boolean unconditionally);
	IBqlTable GetReferencedDacWithoutSelectorCacheUsage(PXCache cache, Object row, Object foreignKeyValue);
	Object GetItemUnconditionally(PXCache cache, PXSelectorAttribute attr, Object key);
	void ClearGlobalCache();
	void ClearGlobalCache(Byte keysCount);
	void ClearGlobalCache(Type table);
	void ClearGlobalCache(Type table, Byte keysCount);
	Object GetField(PXCache cache, Object data, String field, Object value, String foreignField);
	void CheckIntegrityAndPutGlobal(GlobalDictionary globalDictionary, PXCache foreignCache, String foreignField, Object foreignRow, Object ownKey, Boolean isRowDeleted);
	Type GetItemType(PXCache cache, String field);
	List<Object> SelectAll(PXCache cache, Object data);
	List<Object> SelectAll(PXCache cache, String fieldname, Object data);
	Object Select(PXCache cache, Object data);
	Object Select(PXCache cache, Object data, Object value);
	void SetColumns(PXCache cache, Object data, String field, String[] fieldList, String[] headerList);
	void SetColumns(PXCache cache, String field, String[] fieldList, String[] headerList);
	void SetColumns(String[] fieldList, String[] headerList);
	void SetColumns(PXCache cache, Object data, Type[] fieldList, String[] headerList);
	void SetColumns(PXCache cache, Type[] fieldList, String[] headerList);
	void StoreCached(PXCache cache, Object data, Object item);
	void StoreCached(PXCache cache, Object data, Object item, Boolean clearCache);
	void StoreResult(PXCache cache, Object data, IBqlTable selectResult);
	void StoreResult(PXCache cache, IBqlTable selectResult);
	void StoreResult(PXCache cache, Object data, List<Object> selectResult);
	void CheckAndRaiseForeignKeyException(PXCache sender, Object Row, Type fieldType, Type searchType, String customMessage);
	ISet<Type> GetDependencies(PXCache sender);
	Boolean SplitFieldNames(String fieldName, String& outerField, String& innerField);
	void Verify(PXCache sender, PXFieldVerifyingEventArgs e, Object& item);
	String[] hasRestrictedAccess(PXCache sender, BqlCommand command, Object row);
	void throwNoItem(String[] restricted, Boolean external, Object value);
	void throwNoItem(String[] restricted, Boolean external, Object value, IBqlTable row);
	void FieldSelecting(PXCache sender, PXFieldSelectingEventArgs e);
	void DescriptionFieldSelecting(PXCache sender, PXFieldSelectingEventArgs e, Object key, Boolean& deleted);
	void readItem(PXCache sender, Object row, Object key, PXCache& itemCache, Object& item, Boolean& deleted);
	void cacheOnReadItem(GlobalDictionary dict, PXCache foreignCache, Object foreignItem, Boolean isItemDeleted);
	void OnItemCached(PXCache foreignCache, Object foreignItem, Boolean isItemDeleted);
	SubstituteKeyInfo getSubstituteKeyMask(PXCache sender);
	String getDescriptionName(PXCache sender, Nullable`1& length);
	String _GetSlotName(Type type, Byte keysCount);
	void SelfRowSelecting(PXCache sender, PXRowSelectingEventArgs e);
	void SubstituteKeyFieldSelecting(PXCache sender, PXFieldSelectingEventArgs e);
	void SubstituteKeyFieldUpdating(PXCache sender, PXFieldUpdatingEventArgs e);
	void SubstituteKeyCommandPreparing(PXCache sender, PXCommandPreparingEventArgs e);
	Boolean ShouldPrepareCommandForSubstituteKey(PXCommandPreparingEventArgs e);
	void DescriptionFieldCommandPreparing(PXCache sender, PXCommandPreparingEventArgs e);
	void ForeignTableRowPersisted(PXCache sender, PXRowPersistedEventArgs e);
	void ReadDeletedFieldVerifying(PXCache sender, PXFieldVerifyingEventArgs e);
	void SetBqlTable(Type bqlTable);
	List<KeyValuePair`2> GetSelectorFields(Type table);
	void populateFields(PXCache sender, Boolean bypassInit);
	void findFieldsHeaders(PXCache sender);
	void CreateFilter(PXGraph graph);
	Int32 GetHashCode();
	Boolean Equals(Object obj);
	PXEventSubscriberAttribute Clone(PXAttributeLevel attributeLevel);
	Type GetBaseCacheWithTableAttr(Type cache);
	void InjectAttributeDependencies(PXCache cache);
	void InvokeCacheAttached(PXCache cache);
	void GetSubscriber(List<ISubscriber> subscribers);
	T CreateInstance(Object[] constructorArgs);
	PXEventSubscriberAttribute CreateInstance(Type t, Object[] constructorArgs);
	String ToString();
	Attribute[] GetCustomAttributes(MemberInfo element, Type type);
	Attribute[] GetCustomAttributes(MemberInfo element, Type type, Boolean inherit);
	Attribute[] GetCustomAttributes(MemberInfo element);
	Attribute[] GetCustomAttributes(MemberInfo element, Boolean inherit);
	Boolean IsDefined(MemberInfo element, Type attributeType);
	Boolean IsDefined(MemberInfo element, Type attributeType, Boolean inherit);
	Attribute GetCustomAttribute(MemberInfo element, Type attributeType);
	Attribute GetCustomAttribute(MemberInfo element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(ParameterInfo element);
	Attribute[] GetCustomAttributes(ParameterInfo element, Type attributeType);
	Attribute[] GetCustomAttributes(ParameterInfo element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(ParameterInfo element, Boolean inherit);
	Boolean IsDefined(ParameterInfo element, Type attributeType);
	Boolean IsDefined(ParameterInfo element, Type attributeType, Boolean inherit);
	Attribute GetCustomAttribute(ParameterInfo element, Type attributeType);
	Attribute GetCustomAttribute(ParameterInfo element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(Module element, Type attributeType);
	Attribute[] GetCustomAttributes(Module element);
	Attribute[] GetCustomAttributes(Module element, Boolean inherit);
	Attribute[] GetCustomAttributes(Module element, Type attributeType, Boolean inherit);
	Boolean IsDefined(Module element, Type attributeType);
	Boolean IsDefined(Module element, Type attributeType, Boolean inherit);
	Attribute GetCustomAttribute(Module element, Type attributeType);
	Attribute GetCustomAttribute(Module element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(Assembly element, Type attributeType);
	Attribute[] GetCustomAttributes(Assembly element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(Assembly element);
	Attribute[] GetCustomAttributes(Assembly element, Boolean inherit);
	Boolean IsDefined(Assembly element, Type attributeType);
	Boolean IsDefined(Assembly element, Type attributeType, Boolean inherit);
	Attribute GetCustomAttribute(Assembly element, Type attributeType);
	Attribute GetCustomAttribute(Assembly element, Type attributeType, Boolean inherit);
	Boolean Match(Object obj);
	Boolean IsDefaultAttribute();
	void System.Runtime.InteropServices._Attribute.GetTypeInfoCount(UInt32& pcTInfo);
	void System.Runtime.InteropServices._Attribute.GetTypeInfo(UInt32 iTInfo, UInt32 lcid, IntPtr ppTInfo);
	void System.Runtime.InteropServices._Attribute.GetIDsOfNames(Guid& riid, IntPtr rgszNames, UInt32 cNames, UInt32 lcid, IntPtr rgDispId);
	void System.Runtime.InteropServices._Attribute.Invoke(UInt32 dispIdMember, Guid& riid, UInt32 lcid, Int16 wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr);



public class ALPrintNodeComputerStateAttribute:ALAggregateAttribute, _Attribute, IPXInterfaceField, IPXCommandPreparingSubscriber, IPXRowSelectingSubscriber
	Boolean IsDirty;
	Type BqlField;
	Boolean CacheGlobal;
	Object Constant;
	Type DescriptionField;
	Type SubstituteKey;
	Boolean DirtyRead;
	String DisplayName;
	Boolean Enabled;
	Boolean IsReadOnly;
	PXErrorHandling ErrorHandling;
	PXErrorLevel ErrorLevel;
	String ErrorText;
	Object ErrorValue;
	String FieldClass;
	Boolean Filterable;
	Boolean IsDBField;
	Boolean IsFixed;
	Boolean IsKey;
	PXCacheRights MapEnableRights;
	PXCacheRights MapViewRights;
	PXPersistingCheck PersistingCheck;
	Boolean Required;
	Int32 TabOrder;
	Boolean ValidateValue;
	Boolean SuppressVerify;
	Boolean ViewRights;
	PXUIVisibility Visibility;
	Boolean Visible;
	Type BqlTable;
	String FieldName;
	Int32 FieldOrdinal;
	Type CacheExtensionType;
	PXAttributeLevel AttributeLevel;
	Object TypeId;
	void CommandPreparing(PXCache sender, PXCommandPreparingEventArgs e);
	void ForceEnabled();
	void GetSubscriber(List<ISubscriber> subscribers);
	void Initialize();
	void RowSelecting(PXCache sender, PXRowSelectingEventArgs e);
	PXEventSubscriberAttribute[] GetAttributes();
	PXEventSubscriberAttribute[] GetAggregatedAttributes();
	T GetAttribute();
	PXEventSubscriberAttribute Clone(PXAttributeLevel attributeLevel);
	void InjectAttributeDependencies(PXCache cache);
	void CacheAttached(PXCache sender);
	void SetBqlTable(Type bqlTable);
	Boolean ChildrenAttributesComeFirstFor();
	Int32 GetHashCode();
	Boolean Equals(Object obj);
	Type GetBaseCacheWithTableAttr(Type cache);
	void InvokeCacheAttached(PXCache cache);
	T CreateInstance(Object[] constructorArgs);
	PXEventSubscriberAttribute CreateInstance(Type t, Object[] constructorArgs);
	String ToString();
	Attribute[] GetCustomAttributes(MemberInfo element, Type type);
	Attribute[] GetCustomAttributes(MemberInfo element, Type type, Boolean inherit);
	Attribute[] GetCustomAttributes(MemberInfo element);
	Attribute[] GetCustomAttributes(MemberInfo element, Boolean inherit);
	Boolean IsDefined(MemberInfo element, Type attributeType);
	Boolean IsDefined(MemberInfo element, Type attributeType, Boolean inherit);
	Attribute GetCustomAttribute(MemberInfo element, Type attributeType);
	Attribute GetCustomAttribute(MemberInfo element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(ParameterInfo element);
	Attribute[] GetCustomAttributes(ParameterInfo element, Type attributeType);
	Attribute[] GetCustomAttributes(ParameterInfo element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(ParameterInfo element, Boolean inherit);
	Boolean IsDefined(ParameterInfo element, Type attributeType);
	Boolean IsDefined(ParameterInfo element, Type attributeType, Boolean inherit);
	Attribute GetCustomAttribute(ParameterInfo element, Type attributeType);
	Attribute GetCustomAttribute(ParameterInfo element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(Module element, Type attributeType);
	Attribute[] GetCustomAttributes(Module element);
	Attribute[] GetCustomAttributes(Module element, Boolean inherit);
	Attribute[] GetCustomAttributes(Module element, Type attributeType, Boolean inherit);
	Boolean IsDefined(Module element, Type attributeType);
	Boolean IsDefined(Module element, Type attributeType, Boolean inherit);
	Attribute GetCustomAttribute(Module element, Type attributeType);
	Attribute GetCustomAttribute(Module element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(Assembly element, Type attributeType);
	Attribute[] GetCustomAttributes(Assembly element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(Assembly element);
	Attribute[] GetCustomAttributes(Assembly element, Boolean inherit);
	Boolean IsDefined(Assembly element, Type attributeType);
	Boolean IsDefined(Assembly element, Type attributeType, Boolean inherit);
	Attribute GetCustomAttribute(Assembly element, Type attributeType);
	Attribute GetCustomAttribute(Assembly element, Type attributeType, Boolean inherit);
	Boolean Match(Object obj);
	Boolean IsDefaultAttribute();
	void System.Runtime.InteropServices._Attribute.GetTypeInfoCount(UInt32& pcTInfo);
	void System.Runtime.InteropServices._Attribute.GetTypeInfo(UInt32 iTInfo, UInt32 lcid, IntPtr ppTInfo);
	void System.Runtime.InteropServices._Attribute.GetIDsOfNames(Guid& riid, IntPtr rgszNames, UInt32 cNames, UInt32 lcid, IntPtr rgDispId);
	void System.Runtime.InteropServices._Attribute.Invoke(UInt32 dispIdMember, Guid& riid, UInt32 lcid, Int16 wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr);



public class ALPrintNodeContentType



public class ALPrintNodePrinterCapabilitiesAttribute:ALAggregateAttribute, _Attribute, IPXInterfaceField, IPXCommandPreparingSubscriber, IPXRowSelectingSubscriber
	Boolean IsDirty;
	Type BqlField;
	Boolean CacheGlobal;
	Object Constant;
	Type DescriptionField;
	Type SubstituteKey;
	Boolean DirtyRead;
	String DisplayName;
	Boolean Enabled;
	Boolean IsReadOnly;
	PXErrorHandling ErrorHandling;
	PXErrorLevel ErrorLevel;
	String ErrorText;
	Object ErrorValue;
	String FieldClass;
	Boolean Filterable;
	Boolean IsDBField;
	Boolean IsFixed;
	Boolean IsKey;
	PXCacheRights MapEnableRights;
	PXCacheRights MapViewRights;
	PXPersistingCheck PersistingCheck;
	Boolean Required;
	Int32 TabOrder;
	Boolean ValidateValue;
	Boolean SuppressVerify;
	Boolean ViewRights;
	PXUIVisibility Visibility;
	Boolean Visible;
	Type BqlTable;
	String FieldName;
	Int32 FieldOrdinal;
	Type CacheExtensionType;
	PXAttributeLevel AttributeLevel;
	Object TypeId;
	void CommandPreparing(PXCache sender, PXCommandPreparingEventArgs e);
	void ForceEnabled();
	void GetSubscriber(List<ISubscriber> subscribers);
	void Initialize();
	void RowSelecting(PXCache sender, PXRowSelectingEventArgs e);
	PXEventSubscriberAttribute[] GetAttributes();
	PXEventSubscriberAttribute[] GetAggregatedAttributes();
	T GetAttribute();
	PXEventSubscriberAttribute Clone(PXAttributeLevel attributeLevel);
	void InjectAttributeDependencies(PXCache cache);
	void CacheAttached(PXCache sender);
	void SetBqlTable(Type bqlTable);
	Boolean ChildrenAttributesComeFirstFor();
	Int32 GetHashCode();
	Boolean Equals(Object obj);
	Type GetBaseCacheWithTableAttr(Type cache);
	void InvokeCacheAttached(PXCache cache);
	T CreateInstance(Object[] constructorArgs);
	PXEventSubscriberAttribute CreateInstance(Type t, Object[] constructorArgs);
	String ToString();
	Attribute[] GetCustomAttributes(MemberInfo element, Type type);
	Attribute[] GetCustomAttributes(MemberInfo element, Type type, Boolean inherit);
	Attribute[] GetCustomAttributes(MemberInfo element);
	Attribute[] GetCustomAttributes(MemberInfo element, Boolean inherit);
	Boolean IsDefined(MemberInfo element, Type attributeType);
	Boolean IsDefined(MemberInfo element, Type attributeType, Boolean inherit);
	Attribute GetCustomAttribute(MemberInfo element, Type attributeType);
	Attribute GetCustomAttribute(MemberInfo element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(ParameterInfo element);
	Attribute[] GetCustomAttributes(ParameterInfo element, Type attributeType);
	Attribute[] GetCustomAttributes(ParameterInfo element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(ParameterInfo element, Boolean inherit);
	Boolean IsDefined(ParameterInfo element, Type attributeType);
	Boolean IsDefined(ParameterInfo element, Type attributeType, Boolean inherit);
	Attribute GetCustomAttribute(ParameterInfo element, Type attributeType);
	Attribute GetCustomAttribute(ParameterInfo element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(Module element, Type attributeType);
	Attribute[] GetCustomAttributes(Module element);
	Attribute[] GetCustomAttributes(Module element, Boolean inherit);
	Attribute[] GetCustomAttributes(Module element, Type attributeType, Boolean inherit);
	Boolean IsDefined(Module element, Type attributeType);
	Boolean IsDefined(Module element, Type attributeType, Boolean inherit);
	Attribute GetCustomAttribute(Module element, Type attributeType);
	Attribute GetCustomAttribute(Module element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(Assembly element, Type attributeType);
	Attribute[] GetCustomAttributes(Assembly element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(Assembly element);
	Attribute[] GetCustomAttributes(Assembly element, Boolean inherit);
	Boolean IsDefined(Assembly element, Type attributeType);
	Boolean IsDefined(Assembly element, Type attributeType, Boolean inherit);
	Attribute GetCustomAttribute(Assembly element, Type attributeType);
	Attribute GetCustomAttribute(Assembly element, Type attributeType, Boolean inherit);
	Boolean Match(Object obj);
	Boolean IsDefaultAttribute();
	void System.Runtime.InteropServices._Attribute.GetTypeInfoCount(UInt32& pcTInfo);
	void System.Runtime.InteropServices._Attribute.GetTypeInfo(UInt32 iTInfo, UInt32 lcid, IntPtr ppTInfo);
	void System.Runtime.InteropServices._Attribute.GetIDsOfNames(Guid& riid, IntPtr rgszNames, UInt32 cNames, UInt32 lcid, IntPtr rgDispId);
	void System.Runtime.InteropServices._Attribute.Invoke(UInt32 dispIdMember, Guid& riid, UInt32 lcid, Int16 wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr);



public class ALPrintNodePrinterIDAttribute:ALAggregateAttribute, _Attribute, IPXInterfaceField, IPXCommandPreparingSubscriber, IPXRowSelectingSubscriber
	Boolean IsDirty;
	Type BqlField;
	Boolean CacheGlobal;
	Object Constant;
	Type DescriptionField;
	Type SubstituteKey;
	Boolean DirtyRead;
	String DisplayName;
	Boolean Enabled;
	Boolean IsReadOnly;
	PXErrorHandling ErrorHandling;
	PXErrorLevel ErrorLevel;
	String ErrorText;
	Object ErrorValue;
	String FieldClass;
	Boolean Filterable;
	Boolean IsDBField;
	Boolean IsFixed;
	Boolean IsKey;
	PXCacheRights MapEnableRights;
	PXCacheRights MapViewRights;
	PXPersistingCheck PersistingCheck;
	Boolean Required;
	Int32 TabOrder;
	Boolean ValidateValue;
	Boolean SuppressVerify;
	Boolean ViewRights;
	PXUIVisibility Visibility;
	Boolean Visible;
	Type BqlTable;
	String FieldName;
	Int32 FieldOrdinal;
	Type CacheExtensionType;
	PXAttributeLevel AttributeLevel;
	Object TypeId;
	void CommandPreparing(PXCache sender, PXCommandPreparingEventArgs e);
	void ForceEnabled();
	void GetSubscriber(List<ISubscriber> subscribers);
	void Initialize();
	void RowSelecting(PXCache sender, PXRowSelectingEventArgs e);
	PXEventSubscriberAttribute[] GetAttributes();
	PXEventSubscriberAttribute[] GetAggregatedAttributes();
	T GetAttribute();
	PXEventSubscriberAttribute Clone(PXAttributeLevel attributeLevel);
	void InjectAttributeDependencies(PXCache cache);
	void CacheAttached(PXCache sender);
	void SetBqlTable(Type bqlTable);
	Boolean ChildrenAttributesComeFirstFor();
	Int32 GetHashCode();
	Boolean Equals(Object obj);
	Type GetBaseCacheWithTableAttr(Type cache);
	void InvokeCacheAttached(PXCache cache);
	T CreateInstance(Object[] constructorArgs);
	PXEventSubscriberAttribute CreateInstance(Type t, Object[] constructorArgs);
	String ToString();
	Attribute[] GetCustomAttributes(MemberInfo element, Type type);
	Attribute[] GetCustomAttributes(MemberInfo element, Type type, Boolean inherit);
	Attribute[] GetCustomAttributes(MemberInfo element);
	Attribute[] GetCustomAttributes(MemberInfo element, Boolean inherit);
	Boolean IsDefined(MemberInfo element, Type attributeType);
	Boolean IsDefined(MemberInfo element, Type attributeType, Boolean inherit);
	Attribute GetCustomAttribute(MemberInfo element, Type attributeType);
	Attribute GetCustomAttribute(MemberInfo element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(ParameterInfo element);
	Attribute[] GetCustomAttributes(ParameterInfo element, Type attributeType);
	Attribute[] GetCustomAttributes(ParameterInfo element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(ParameterInfo element, Boolean inherit);
	Boolean IsDefined(ParameterInfo element, Type attributeType);
	Boolean IsDefined(ParameterInfo element, Type attributeType, Boolean inherit);
	Attribute GetCustomAttribute(ParameterInfo element, Type attributeType);
	Attribute GetCustomAttribute(ParameterInfo element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(Module element, Type attributeType);
	Attribute[] GetCustomAttributes(Module element);
	Attribute[] GetCustomAttributes(Module element, Boolean inherit);
	Attribute[] GetCustomAttributes(Module element, Type attributeType, Boolean inherit);
	Boolean IsDefined(Module element, Type attributeType);
	Boolean IsDefined(Module element, Type attributeType, Boolean inherit);
	Attribute GetCustomAttribute(Module element, Type attributeType);
	Attribute GetCustomAttribute(Module element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(Assembly element, Type attributeType);
	Attribute[] GetCustomAttributes(Assembly element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(Assembly element);
	Attribute[] GetCustomAttributes(Assembly element, Boolean inherit);
	Boolean IsDefined(Assembly element, Type attributeType);
	Boolean IsDefined(Assembly element, Type attributeType, Boolean inherit);
	Attribute GetCustomAttribute(Assembly element, Type attributeType);
	Attribute GetCustomAttribute(Assembly element, Type attributeType, Boolean inherit);
	Boolean Match(Object obj);
	Boolean IsDefaultAttribute();
	void System.Runtime.InteropServices._Attribute.GetTypeInfoCount(UInt32& pcTInfo);
	void System.Runtime.InteropServices._Attribute.GetTypeInfo(UInt32 iTInfo, UInt32 lcid, IntPtr ppTInfo);
	void System.Runtime.InteropServices._Attribute.GetIDsOfNames(Guid& riid, IntPtr rgszNames, UInt32 cNames, UInt32 lcid, IntPtr rgDispId);
	void System.Runtime.InteropServices._Attribute.Invoke(UInt32 dispIdMember, Guid& riid, UInt32 lcid, Int16 wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr);



public class ALPrintNodePrinterIDStandaloneAttribute:ALAggregateAttribute, _Attribute, IPXInterfaceField, IPXCommandPreparingSubscriber, IPXRowSelectingSubscriber
	Boolean IsDirty;
	Type BqlField;
	Boolean CacheGlobal;
	Object Constant;
	Type DescriptionField;
	Type SubstituteKey;
	Boolean DirtyRead;
	String DisplayName;
	Boolean Enabled;
	Boolean IsReadOnly;
	PXErrorHandling ErrorHandling;
	PXErrorLevel ErrorLevel;
	String ErrorText;
	Object ErrorValue;
	String FieldClass;
	Boolean Filterable;
	Boolean IsDBField;
	Boolean IsFixed;
	Boolean IsKey;
	PXCacheRights MapEnableRights;
	PXCacheRights MapViewRights;
	PXPersistingCheck PersistingCheck;
	Boolean Required;
	Int32 TabOrder;
	Boolean ValidateValue;
	Boolean SuppressVerify;
	Boolean ViewRights;
	PXUIVisibility Visibility;
	Boolean Visible;
	Type BqlTable;
	String FieldName;
	Int32 FieldOrdinal;
	Type CacheExtensionType;
	PXAttributeLevel AttributeLevel;
	Object TypeId;
	void CommandPreparing(PXCache sender, PXCommandPreparingEventArgs e);
	void ForceEnabled();
	void GetSubscriber(List<ISubscriber> subscribers);
	void Initialize();
	void RowSelecting(PXCache sender, PXRowSelectingEventArgs e);
	PXEventSubscriberAttribute[] GetAttributes();
	PXEventSubscriberAttribute[] GetAggregatedAttributes();
	T GetAttribute();
	PXEventSubscriberAttribute Clone(PXAttributeLevel attributeLevel);
	void InjectAttributeDependencies(PXCache cache);
	void CacheAttached(PXCache sender);
	void SetBqlTable(Type bqlTable);
	Boolean ChildrenAttributesComeFirstFor();
	Int32 GetHashCode();
	Boolean Equals(Object obj);
	Type GetBaseCacheWithTableAttr(Type cache);
	void InvokeCacheAttached(PXCache cache);
	T CreateInstance(Object[] constructorArgs);
	PXEventSubscriberAttribute CreateInstance(Type t, Object[] constructorArgs);
	String ToString();
	Attribute[] GetCustomAttributes(MemberInfo element, Type type);
	Attribute[] GetCustomAttributes(MemberInfo element, Type type, Boolean inherit);
	Attribute[] GetCustomAttributes(MemberInfo element);
	Attribute[] GetCustomAttributes(MemberInfo element, Boolean inherit);
	Boolean IsDefined(MemberInfo element, Type attributeType);
	Boolean IsDefined(MemberInfo element, Type attributeType, Boolean inherit);
	Attribute GetCustomAttribute(MemberInfo element, Type attributeType);
	Attribute GetCustomAttribute(MemberInfo element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(ParameterInfo element);
	Attribute[] GetCustomAttributes(ParameterInfo element, Type attributeType);
	Attribute[] GetCustomAttributes(ParameterInfo element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(ParameterInfo element, Boolean inherit);
	Boolean IsDefined(ParameterInfo element, Type attributeType);
	Boolean IsDefined(ParameterInfo element, Type attributeType, Boolean inherit);
	Attribute GetCustomAttribute(ParameterInfo element, Type attributeType);
	Attribute GetCustomAttribute(ParameterInfo element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(Module element, Type attributeType);
	Attribute[] GetCustomAttributes(Module element);
	Attribute[] GetCustomAttributes(Module element, Boolean inherit);
	Attribute[] GetCustomAttributes(Module element, Type attributeType, Boolean inherit);
	Boolean IsDefined(Module element, Type attributeType);
	Boolean IsDefined(Module element, Type attributeType, Boolean inherit);
	Attribute GetCustomAttribute(Module element, Type attributeType);
	Attribute GetCustomAttribute(Module element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(Assembly element, Type attributeType);
	Attribute[] GetCustomAttributes(Assembly element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(Assembly element);
	Attribute[] GetCustomAttributes(Assembly element, Boolean inherit);
	Boolean IsDefined(Assembly element, Type attributeType);
	Boolean IsDefined(Assembly element, Type attributeType, Boolean inherit);
	Attribute GetCustomAttribute(Assembly element, Type attributeType);
	Attribute GetCustomAttribute(Assembly element, Type attributeType, Boolean inherit);
	Boolean Match(Object obj);
	Boolean IsDefaultAttribute();
	void System.Runtime.InteropServices._Attribute.GetTypeInfoCount(UInt32& pcTInfo);
	void System.Runtime.InteropServices._Attribute.GetTypeInfo(UInt32 iTInfo, UInt32 lcid, IntPtr ppTInfo);
	void System.Runtime.InteropServices._Attribute.GetIDsOfNames(Guid& riid, IntPtr rgszNames, UInt32 cNames, UInt32 lcid, IntPtr rgDispId);
	void System.Runtime.InteropServices._Attribute.Invoke(UInt32 dispIdMember, Guid& riid, UInt32 lcid, Int16 wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr);



public class ALPrintNodePrinterSelectorAttribute:PXCustomSelectorAttribute, _Attribute, IPXFieldVerifyingSubscriber, IPXFieldSelectingSubscriber, IPXDependsOnFields
	Boolean ValidateValue;
	Boolean IsDirty;
	Boolean ExcludeFromReferenceGeneratingProcess;
	Boolean IsPrimaryViewCompatible;
	Boolean ShowWarningForNotExistsOnSelect;
	String CustomMessageElementDoesntExist;
	String CustomMessageValueDoesntExist;
	String CustomMessageElementDoesntExistOrNoRights;
	String CustomMessageValueDoesntExistOrNoRights;
	Boolean CacheGlobal;
	Type DescriptionField;
	String DescriptionDisplayName;
	Boolean ShowPopupWarning;
	Boolean ShowPopupMessage;
	Type FilterEntity;
	Type SubstituteKey;
	Type Field;
	Boolean DirtyRead;
	Boolean Filterable;
	String[] Headers;
	Type ValueField;
	PXSelectorMode SelectorMode;
	BqlCommand PrimarySelect;
	BqlCommand OriginalSelect;
	Int32 ParsCount;
	Boolean SuppressUnconditionalSelect;
	String ViewName;
	PXAttributeLevel AttributeLevel;
	Type BqlTable;
	Type CacheExtensionType;
	String FieldName;
	Int32 FieldOrdinal;
	Object TypeId;
	void FieldVerifying(PXCache sender, PXFieldVerifyingEventArgs e);
	Nullable<Int32> GetComputerID(Object data);
	IEnumerable GetRecords();
	IEnumerable<IPrintNodePrinter> GetPrinters(Nullable<Int32> computerID, String apiKey);
	void DescriptionFieldSelecting(PXCache sender, PXFieldSelectingEventArgs e, String alias);
	String GetAPIKey(Object data);
	PXView GetView(PXCache cache, BqlCommand select, Boolean isReadOnly);
	PXView GetUnconditionalView(PXCache cache);
	void CacheAttached(PXCache sender);
	void CreateView(PXCache sender);
	void EmitDescriptionFieldAlias(PXCache sender, String alias);
	void EmitColumnForDescriptionField(PXCache sender);
	BqlCommand GetSelect();
	void SubscribeToFeatureSet();
	void SetFieldList(Type[] fieldList);
	BqlCommand WhereAnd(PXCache sender, Type whr);
	String GenerateViewName();
	BqlCommand BuildNaturalSelect(Boolean cacheGlobal, Type substituteKey);
	Object SelectSingleBound(PXView view, Object[] currents, Object[] pars);
	Object SelectSingle(PXView view, Object[] pars);
	Object SelectSingle(PXCache cache, Object data, String field, Object value);
	Object SelectSingle(PXCache cache, Object data, String field);
	Object[] MakeParameters(Object lastParameter, Boolean includeLookupJoins);
	ViewWithParameters GetViewWithParameters(PXCache cache, Object lastParameter, Boolean includeLookupJoins);
	Object Select(PXCache cache, Object data, String field);
	Object SelectFirst(PXCache cache, Object data);
	Object SelectFirst(PXCache cache, Object data, String field);
	Object SelectLast(PXCache cache, Object data);
	Object SelectLast(PXCache cache, Object data, String field);
	Object Select(PXCache cache, Object data, String field, Object value);
	GlobalDictionary GetGlobalCache();
	void AppendOtherValues(Dictionary<String,Object> values, PXCache cache, Object row);
	Object CreateGlobalCacheKey(PXCache cache, Object row, Object keyValue);
	Boolean CanCacheGlobal(PXCache foreignCache);
	Object GetItem(PXCache cache, PXSelectorAttribute attr, Object data, Object key);
	Object GetItem(PXCache cache, PXSelectorAttribute attr, Object data, Object key, Boolean unconditionally);
	IBqlTable GetReferencedDacWithoutSelectorCacheUsage(PXCache cache, Object row, Object foreignKeyValue);
	Object GetItemUnconditionally(PXCache cache, PXSelectorAttribute attr, Object key);
	void ClearGlobalCache();
	void ClearGlobalCache(Byte keysCount);
	void ClearGlobalCache(Type table);
	void ClearGlobalCache(Type table, Byte keysCount);
	Object GetField(PXCache cache, Object data, String field, Object value, String foreignField);
	void CheckIntegrityAndPutGlobal(GlobalDictionary globalDictionary, PXCache foreignCache, String foreignField, Object foreignRow, Object ownKey, Boolean isRowDeleted);
	Type GetItemType(PXCache cache, String field);
	List<Object> SelectAll(PXCache cache, Object data);
	List<Object> SelectAll(PXCache cache, String fieldname, Object data);
	Object Select(PXCache cache, Object data);
	Object Select(PXCache cache, Object data, Object value);
	void SetColumns(PXCache cache, Object data, String field, String[] fieldList, String[] headerList);
	void SetColumns(PXCache cache, String field, String[] fieldList, String[] headerList);
	void SetColumns(String[] fieldList, String[] headerList);
	void SetColumns(PXCache cache, Object data, Type[] fieldList, String[] headerList);
	void SetColumns(PXCache cache, Type[] fieldList, String[] headerList);
	void StoreCached(PXCache cache, Object data, Object item);
	void StoreCached(PXCache cache, Object data, Object item, Boolean clearCache);
	void StoreResult(PXCache cache, Object data, IBqlTable selectResult);
	void StoreResult(PXCache cache, IBqlTable selectResult);
	void StoreResult(PXCache cache, Object data, List<Object> selectResult);
	void CheckAndRaiseForeignKeyException(PXCache sender, Object Row, Type fieldType, Type searchType, String customMessage);
	ISet<Type> GetDependencies(PXCache sender);
	Boolean SplitFieldNames(String fieldName, String& outerField, String& innerField);
	void Verify(PXCache sender, PXFieldVerifyingEventArgs e, Object& item);
	String[] hasRestrictedAccess(PXCache sender, BqlCommand command, Object row);
	void throwNoItem(String[] restricted, Boolean external, Object value);
	void throwNoItem(String[] restricted, Boolean external, Object value, IBqlTable row);
	void FieldSelecting(PXCache sender, PXFieldSelectingEventArgs e);
	void DescriptionFieldSelecting(PXCache sender, PXFieldSelectingEventArgs e, Object key, Boolean& deleted);
	void readItem(PXCache sender, Object row, Object key, PXCache& itemCache, Object& item, Boolean& deleted);
	void cacheOnReadItem(GlobalDictionary dict, PXCache foreignCache, Object foreignItem, Boolean isItemDeleted);
	void OnItemCached(PXCache foreignCache, Object foreignItem, Boolean isItemDeleted);
	SubstituteKeyInfo getSubstituteKeyMask(PXCache sender);
	String getDescriptionName(PXCache sender, Nullable`1& length);
	String _GetSlotName(Type type, Byte keysCount);
	void SelfRowSelecting(PXCache sender, PXRowSelectingEventArgs e);
	void SubstituteKeyFieldSelecting(PXCache sender, PXFieldSelectingEventArgs e);
	void SubstituteKeyFieldUpdating(PXCache sender, PXFieldUpdatingEventArgs e);
	void SubstituteKeyCommandPreparing(PXCache sender, PXCommandPreparingEventArgs e);
	Boolean ShouldPrepareCommandForSubstituteKey(PXCommandPreparingEventArgs e);
	void DescriptionFieldCommandPreparing(PXCache sender, PXCommandPreparingEventArgs e);
	void ForeignTableRowPersisted(PXCache sender, PXRowPersistedEventArgs e);
	void ReadDeletedFieldVerifying(PXCache sender, PXFieldVerifyingEventArgs e);
	void SetBqlTable(Type bqlTable);
	List<KeyValuePair`2> GetSelectorFields(Type table);
	void populateFields(PXCache sender, Boolean bypassInit);
	void findFieldsHeaders(PXCache sender);
	void CreateFilter(PXGraph graph);
	Int32 GetHashCode();
	Boolean Equals(Object obj);
	PXEventSubscriberAttribute Clone(PXAttributeLevel attributeLevel);
	Type GetBaseCacheWithTableAttr(Type cache);
	void InjectAttributeDependencies(PXCache cache);
	void InvokeCacheAttached(PXCache cache);
	void GetSubscriber(List<ISubscriber> subscribers);
	T CreateInstance(Object[] constructorArgs);
	PXEventSubscriberAttribute CreateInstance(Type t, Object[] constructorArgs);
	String ToString();
	Attribute[] GetCustomAttributes(MemberInfo element, Type type);
	Attribute[] GetCustomAttributes(MemberInfo element, Type type, Boolean inherit);
	Attribute[] GetCustomAttributes(MemberInfo element);
	Attribute[] GetCustomAttributes(MemberInfo element, Boolean inherit);
	Boolean IsDefined(MemberInfo element, Type attributeType);
	Boolean IsDefined(MemberInfo element, Type attributeType, Boolean inherit);
	Attribute GetCustomAttribute(MemberInfo element, Type attributeType);
	Attribute GetCustomAttribute(MemberInfo element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(ParameterInfo element);
	Attribute[] GetCustomAttributes(ParameterInfo element, Type attributeType);
	Attribute[] GetCustomAttributes(ParameterInfo element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(ParameterInfo element, Boolean inherit);
	Boolean IsDefined(ParameterInfo element, Type attributeType);
	Boolean IsDefined(ParameterInfo element, Type attributeType, Boolean inherit);
	Attribute GetCustomAttribute(ParameterInfo element, Type attributeType);
	Attribute GetCustomAttribute(ParameterInfo element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(Module element, Type attributeType);
	Attribute[] GetCustomAttributes(Module element);
	Attribute[] GetCustomAttributes(Module element, Boolean inherit);
	Attribute[] GetCustomAttributes(Module element, Type attributeType, Boolean inherit);
	Boolean IsDefined(Module element, Type attributeType);
	Boolean IsDefined(Module element, Type attributeType, Boolean inherit);
	Attribute GetCustomAttribute(Module element, Type attributeType);
	Attribute GetCustomAttribute(Module element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(Assembly element, Type attributeType);
	Attribute[] GetCustomAttributes(Assembly element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(Assembly element);
	Attribute[] GetCustomAttributes(Assembly element, Boolean inherit);
	Boolean IsDefined(Assembly element, Type attributeType);
	Boolean IsDefined(Assembly element, Type attributeType, Boolean inherit);
	Attribute GetCustomAttribute(Assembly element, Type attributeType);
	Attribute GetCustomAttribute(Assembly element, Type attributeType, Boolean inherit);
	Boolean Match(Object obj);
	Boolean IsDefaultAttribute();
	void System.Runtime.InteropServices._Attribute.GetTypeInfoCount(UInt32& pcTInfo);
	void System.Runtime.InteropServices._Attribute.GetTypeInfo(UInt32 iTInfo, UInt32 lcid, IntPtr ppTInfo);
	void System.Runtime.InteropServices._Attribute.GetIDsOfNames(Guid& riid, IntPtr rgszNames, UInt32 cNames, UInt32 lcid, IntPtr rgDispId);
	void System.Runtime.InteropServices._Attribute.Invoke(UInt32 dispIdMember, Guid& riid, UInt32 lcid, Int16 wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr);



public class ALPrintNodePrinterSelectorStandaloneAttribute:PXCustomSelectorAttribute, _Attribute, IPXFieldVerifyingSubscriber, IPXFieldSelectingSubscriber, IPXDependsOnFields
	Boolean ValidateValue;
	Boolean IsDirty;
	Boolean ExcludeFromReferenceGeneratingProcess;
	Boolean IsPrimaryViewCompatible;
	Boolean ShowWarningForNotExistsOnSelect;
	String CustomMessageElementDoesntExist;
	String CustomMessageValueDoesntExist;
	String CustomMessageElementDoesntExistOrNoRights;
	String CustomMessageValueDoesntExistOrNoRights;
	Boolean CacheGlobal;
	Type DescriptionField;
	String DescriptionDisplayName;
	Boolean ShowPopupWarning;
	Boolean ShowPopupMessage;
	Type FilterEntity;
	Type SubstituteKey;
	Type Field;
	Boolean DirtyRead;
	Boolean Filterable;
	String[] Headers;
	Type ValueField;
	PXSelectorMode SelectorMode;
	BqlCommand PrimarySelect;
	BqlCommand OriginalSelect;
	Int32 ParsCount;
	Boolean SuppressUnconditionalSelect;
	String ViewName;
	PXAttributeLevel AttributeLevel;
	Type BqlTable;
	Type CacheExtensionType;
	String FieldName;
	Int32 FieldOrdinal;
	Object TypeId;
	void FieldVerifying(PXCache sender, PXFieldVerifyingEventArgs e);
	Nullable<Int32> GetComputerID(Object data);
	IEnumerable GetRecords();
	void DescriptionFieldSelecting(PXCache sender, PXFieldSelectingEventArgs e, String alias);
	PXView GetView(PXCache cache, BqlCommand select, Boolean isReadOnly);
	PXView GetUnconditionalView(PXCache cache);
	void CacheAttached(PXCache sender);
	void CreateView(PXCache sender);
	void EmitDescriptionFieldAlias(PXCache sender, String alias);
	void EmitColumnForDescriptionField(PXCache sender);
	BqlCommand GetSelect();
	void SubscribeToFeatureSet();
	void SetFieldList(Type[] fieldList);
	BqlCommand WhereAnd(PXCache sender, Type whr);
	String GenerateViewName();
	BqlCommand BuildNaturalSelect(Boolean cacheGlobal, Type substituteKey);
	Object SelectSingleBound(PXView view, Object[] currents, Object[] pars);
	Object SelectSingle(PXView view, Object[] pars);
	Object SelectSingle(PXCache cache, Object data, String field, Object value);
	Object SelectSingle(PXCache cache, Object data, String field);
	Object[] MakeParameters(Object lastParameter, Boolean includeLookupJoins);
	ViewWithParameters GetViewWithParameters(PXCache cache, Object lastParameter, Boolean includeLookupJoins);
	Object Select(PXCache cache, Object data, String field);
	Object SelectFirst(PXCache cache, Object data);
	Object SelectFirst(PXCache cache, Object data, String field);
	Object SelectLast(PXCache cache, Object data);
	Object SelectLast(PXCache cache, Object data, String field);
	Object Select(PXCache cache, Object data, String field, Object value);
	GlobalDictionary GetGlobalCache();
	void AppendOtherValues(Dictionary<String,Object> values, PXCache cache, Object row);
	Object CreateGlobalCacheKey(PXCache cache, Object row, Object keyValue);
	Boolean CanCacheGlobal(PXCache foreignCache);
	Object GetItem(PXCache cache, PXSelectorAttribute attr, Object data, Object key);
	Object GetItem(PXCache cache, PXSelectorAttribute attr, Object data, Object key, Boolean unconditionally);
	IBqlTable GetReferencedDacWithoutSelectorCacheUsage(PXCache cache, Object row, Object foreignKeyValue);
	Object GetItemUnconditionally(PXCache cache, PXSelectorAttribute attr, Object key);
	void ClearGlobalCache();
	void ClearGlobalCache(Byte keysCount);
	void ClearGlobalCache(Type table);
	void ClearGlobalCache(Type table, Byte keysCount);
	Object GetField(PXCache cache, Object data, String field, Object value, String foreignField);
	void CheckIntegrityAndPutGlobal(GlobalDictionary globalDictionary, PXCache foreignCache, String foreignField, Object foreignRow, Object ownKey, Boolean isRowDeleted);
	Type GetItemType(PXCache cache, String field);
	List<Object> SelectAll(PXCache cache, Object data);
	List<Object> SelectAll(PXCache cache, String fieldname, Object data);
	Object Select(PXCache cache, Object data);
	Object Select(PXCache cache, Object data, Object value);
	void SetColumns(PXCache cache, Object data, String field, String[] fieldList, String[] headerList);
	void SetColumns(PXCache cache, String field, String[] fieldList, String[] headerList);
	void SetColumns(String[] fieldList, String[] headerList);
	void SetColumns(PXCache cache, Object data, Type[] fieldList, String[] headerList);
	void SetColumns(PXCache cache, Type[] fieldList, String[] headerList);
	void StoreCached(PXCache cache, Object data, Object item);
	void StoreCached(PXCache cache, Object data, Object item, Boolean clearCache);
	void StoreResult(PXCache cache, Object data, IBqlTable selectResult);
	void StoreResult(PXCache cache, IBqlTable selectResult);
	void StoreResult(PXCache cache, Object data, List<Object> selectResult);
	void CheckAndRaiseForeignKeyException(PXCache sender, Object Row, Type fieldType, Type searchType, String customMessage);
	ISet<Type> GetDependencies(PXCache sender);
	Boolean SplitFieldNames(String fieldName, String& outerField, String& innerField);
	void Verify(PXCache sender, PXFieldVerifyingEventArgs e, Object& item);
	String[] hasRestrictedAccess(PXCache sender, BqlCommand command, Object row);
	void throwNoItem(String[] restricted, Boolean external, Object value);
	void throwNoItem(String[] restricted, Boolean external, Object value, IBqlTable row);
	void FieldSelecting(PXCache sender, PXFieldSelectingEventArgs e);
	void DescriptionFieldSelecting(PXCache sender, PXFieldSelectingEventArgs e, Object key, Boolean& deleted);
	void readItem(PXCache sender, Object row, Object key, PXCache& itemCache, Object& item, Boolean& deleted);
	void cacheOnReadItem(GlobalDictionary dict, PXCache foreignCache, Object foreignItem, Boolean isItemDeleted);
	void OnItemCached(PXCache foreignCache, Object foreignItem, Boolean isItemDeleted);
	SubstituteKeyInfo getSubstituteKeyMask(PXCache sender);
	String getDescriptionName(PXCache sender, Nullable`1& length);
	String _GetSlotName(Type type, Byte keysCount);
	void SelfRowSelecting(PXCache sender, PXRowSelectingEventArgs e);
	void SubstituteKeyFieldSelecting(PXCache sender, PXFieldSelectingEventArgs e);
	void SubstituteKeyFieldUpdating(PXCache sender, PXFieldUpdatingEventArgs e);
	void SubstituteKeyCommandPreparing(PXCache sender, PXCommandPreparingEventArgs e);
	Boolean ShouldPrepareCommandForSubstituteKey(PXCommandPreparingEventArgs e);
	void DescriptionFieldCommandPreparing(PXCache sender, PXCommandPreparingEventArgs e);
	void ForeignTableRowPersisted(PXCache sender, PXRowPersistedEventArgs e);
	void ReadDeletedFieldVerifying(PXCache sender, PXFieldVerifyingEventArgs e);
	void SetBqlTable(Type bqlTable);
	List<KeyValuePair`2> GetSelectorFields(Type table);
	void populateFields(PXCache sender, Boolean bypassInit);
	void findFieldsHeaders(PXCache sender);
	void CreateFilter(PXGraph graph);
	Int32 GetHashCode();
	Boolean Equals(Object obj);
	PXEventSubscriberAttribute Clone(PXAttributeLevel attributeLevel);
	Type GetBaseCacheWithTableAttr(Type cache);
	void InjectAttributeDependencies(PXCache cache);
	void InvokeCacheAttached(PXCache cache);
	void GetSubscriber(List<ISubscriber> subscribers);
	T CreateInstance(Object[] constructorArgs);
	PXEventSubscriberAttribute CreateInstance(Type t, Object[] constructorArgs);
	String ToString();
	Attribute[] GetCustomAttributes(MemberInfo element, Type type);
	Attribute[] GetCustomAttributes(MemberInfo element, Type type, Boolean inherit);
	Attribute[] GetCustomAttributes(MemberInfo element);
	Attribute[] GetCustomAttributes(MemberInfo element, Boolean inherit);
	Boolean IsDefined(MemberInfo element, Type attributeType);
	Boolean IsDefined(MemberInfo element, Type attributeType, Boolean inherit);
	Attribute GetCustomAttribute(MemberInfo element, Type attributeType);
	Attribute GetCustomAttribute(MemberInfo element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(ParameterInfo element);
	Attribute[] GetCustomAttributes(ParameterInfo element, Type attributeType);
	Attribute[] GetCustomAttributes(ParameterInfo element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(ParameterInfo element, Boolean inherit);
	Boolean IsDefined(ParameterInfo element, Type attributeType);
	Boolean IsDefined(ParameterInfo element, Type attributeType, Boolean inherit);
	Attribute GetCustomAttribute(ParameterInfo element, Type attributeType);
	Attribute GetCustomAttribute(ParameterInfo element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(Module element, Type attributeType);
	Attribute[] GetCustomAttributes(Module element);
	Attribute[] GetCustomAttributes(Module element, Boolean inherit);
	Attribute[] GetCustomAttributes(Module element, Type attributeType, Boolean inherit);
	Boolean IsDefined(Module element, Type attributeType);
	Boolean IsDefined(Module element, Type attributeType, Boolean inherit);
	Attribute GetCustomAttribute(Module element, Type attributeType);
	Attribute GetCustomAttribute(Module element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(Assembly element, Type attributeType);
	Attribute[] GetCustomAttributes(Assembly element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(Assembly element);
	Attribute[] GetCustomAttributes(Assembly element, Boolean inherit);
	Boolean IsDefined(Assembly element, Type attributeType);
	Boolean IsDefined(Assembly element, Type attributeType, Boolean inherit);
	Attribute GetCustomAttribute(Assembly element, Type attributeType);
	Attribute GetCustomAttribute(Assembly element, Type attributeType, Boolean inherit);
	Boolean Match(Object obj);
	Boolean IsDefaultAttribute();
	void System.Runtime.InteropServices._Attribute.GetTypeInfoCount(UInt32& pcTInfo);
	void System.Runtime.InteropServices._Attribute.GetTypeInfo(UInt32 iTInfo, UInt32 lcid, IntPtr ppTInfo);
	void System.Runtime.InteropServices._Attribute.GetIDsOfNames(Guid& riid, IntPtr rgszNames, UInt32 cNames, UInt32 lcid, IntPtr rgDispId);
	void System.Runtime.InteropServices._Attribute.Invoke(UInt32 dispIdMember, Guid& riid, UInt32 lcid, Int16 wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr);



public class ALPrintNodePrinterStateAttribute:ALAggregateAttribute, _Attribute, IPXInterfaceField, IPXCommandPreparingSubscriber, IPXRowSelectingSubscriber
	Boolean IsDirty;
	Type BqlField;
	Boolean CacheGlobal;
	Object Constant;
	Type DescriptionField;
	Type SubstituteKey;
	Boolean DirtyRead;
	String DisplayName;
	Boolean Enabled;
	Boolean IsReadOnly;
	PXErrorHandling ErrorHandling;
	PXErrorLevel ErrorLevel;
	String ErrorText;
	Object ErrorValue;
	String FieldClass;
	Boolean Filterable;
	Boolean IsDBField;
	Boolean IsFixed;
	Boolean IsKey;
	PXCacheRights MapEnableRights;
	PXCacheRights MapViewRights;
	PXPersistingCheck PersistingCheck;
	Boolean Required;
	Int32 TabOrder;
	Boolean ValidateValue;
	Boolean SuppressVerify;
	Boolean ViewRights;
	PXUIVisibility Visibility;
	Boolean Visible;
	Type BqlTable;
	String FieldName;
	Int32 FieldOrdinal;
	Type CacheExtensionType;
	PXAttributeLevel AttributeLevel;
	Object TypeId;
	void CommandPreparing(PXCache sender, PXCommandPreparingEventArgs e);
	void ForceEnabled();
	void GetSubscriber(List<ISubscriber> subscribers);
	void Initialize();
	void RowSelecting(PXCache sender, PXRowSelectingEventArgs e);
	PXEventSubscriberAttribute[] GetAttributes();
	PXEventSubscriberAttribute[] GetAggregatedAttributes();
	T GetAttribute();
	PXEventSubscriberAttribute Clone(PXAttributeLevel attributeLevel);
	void InjectAttributeDependencies(PXCache cache);
	void CacheAttached(PXCache sender);
	void SetBqlTable(Type bqlTable);
	Boolean ChildrenAttributesComeFirstFor();
	Int32 GetHashCode();
	Boolean Equals(Object obj);
	Type GetBaseCacheWithTableAttr(Type cache);
	void InvokeCacheAttached(PXCache cache);
	T CreateInstance(Object[] constructorArgs);
	PXEventSubscriberAttribute CreateInstance(Type t, Object[] constructorArgs);
	String ToString();
	Attribute[] GetCustomAttributes(MemberInfo element, Type type);
	Attribute[] GetCustomAttributes(MemberInfo element, Type type, Boolean inherit);
	Attribute[] GetCustomAttributes(MemberInfo element);
	Attribute[] GetCustomAttributes(MemberInfo element, Boolean inherit);
	Boolean IsDefined(MemberInfo element, Type attributeType);
	Boolean IsDefined(MemberInfo element, Type attributeType, Boolean inherit);
	Attribute GetCustomAttribute(MemberInfo element, Type attributeType);
	Attribute GetCustomAttribute(MemberInfo element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(ParameterInfo element);
	Attribute[] GetCustomAttributes(ParameterInfo element, Type attributeType);
	Attribute[] GetCustomAttributes(ParameterInfo element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(ParameterInfo element, Boolean inherit);
	Boolean IsDefined(ParameterInfo element, Type attributeType);
	Boolean IsDefined(ParameterInfo element, Type attributeType, Boolean inherit);
	Attribute GetCustomAttribute(ParameterInfo element, Type attributeType);
	Attribute GetCustomAttribute(ParameterInfo element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(Module element, Type attributeType);
	Attribute[] GetCustomAttributes(Module element);
	Attribute[] GetCustomAttributes(Module element, Boolean inherit);
	Attribute[] GetCustomAttributes(Module element, Type attributeType, Boolean inherit);
	Boolean IsDefined(Module element, Type attributeType);
	Boolean IsDefined(Module element, Type attributeType, Boolean inherit);
	Attribute GetCustomAttribute(Module element, Type attributeType);
	Attribute GetCustomAttribute(Module element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(Assembly element, Type attributeType);
	Attribute[] GetCustomAttributes(Assembly element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(Assembly element);
	Attribute[] GetCustomAttributes(Assembly element, Boolean inherit);
	Boolean IsDefined(Assembly element, Type attributeType);
	Boolean IsDefined(Assembly element, Type attributeType, Boolean inherit);
	Attribute GetCustomAttribute(Assembly element, Type attributeType);
	Attribute GetCustomAttribute(Assembly element, Type attributeType, Boolean inherit);
	Boolean Match(Object obj);
	Boolean IsDefaultAttribute();
	void System.Runtime.InteropServices._Attribute.GetTypeInfoCount(UInt32& pcTInfo);
	void System.Runtime.InteropServices._Attribute.GetTypeInfo(UInt32 iTInfo, UInt32 lcid, IntPtr ppTInfo);
	void System.Runtime.InteropServices._Attribute.GetIDsOfNames(Guid& riid, IntPtr rgszNames, UInt32 cNames, UInt32 lcid, IntPtr rgDispId);
	void System.Runtime.InteropServices._Attribute.Invoke(UInt32 dispIdMember, Guid& riid, UInt32 lcid, Int16 wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr);



public class ALPrintNodePrintJobIDAttribute:ALAggregateAttribute, _Attribute, IPXInterfaceField, IPXCommandPreparingSubscriber, IPXRowSelectingSubscriber
	String DISPLAY_NAME;
	Boolean IsDirty;
	Type BqlField;
	Boolean CacheGlobal;
	Object Constant;
	Type DescriptionField;
	Type SubstituteKey;
	Boolean DirtyRead;
	String DisplayName;
	Boolean Enabled;
	Boolean IsReadOnly;
	PXErrorHandling ErrorHandling;
	PXErrorLevel ErrorLevel;
	String ErrorText;
	Object ErrorValue;
	String FieldClass;
	Boolean Filterable;
	Boolean IsDBField;
	Boolean IsFixed;
	Boolean IsKey;
	PXCacheRights MapEnableRights;
	PXCacheRights MapViewRights;
	PXPersistingCheck PersistingCheck;
	Boolean Required;
	Int32 TabOrder;
	Boolean ValidateValue;
	Boolean SuppressVerify;
	Boolean ViewRights;
	PXUIVisibility Visibility;
	Boolean Visible;
	Type BqlTable;
	String FieldName;
	Int32 FieldOrdinal;
	Type CacheExtensionType;
	PXAttributeLevel AttributeLevel;
	Object TypeId;
	void CommandPreparing(PXCache sender, PXCommandPreparingEventArgs e);
	void ForceEnabled();
	void GetSubscriber(List<ISubscriber> subscribers);
	void Initialize();
	void RowSelecting(PXCache sender, PXRowSelectingEventArgs e);
	PXEventSubscriberAttribute[] GetAttributes();
	PXEventSubscriberAttribute[] GetAggregatedAttributes();
	T GetAttribute();
	PXEventSubscriberAttribute Clone(PXAttributeLevel attributeLevel);
	void InjectAttributeDependencies(PXCache cache);
	void CacheAttached(PXCache sender);
	void SetBqlTable(Type bqlTable);
	Boolean ChildrenAttributesComeFirstFor();
	Int32 GetHashCode();
	Boolean Equals(Object obj);
	Type GetBaseCacheWithTableAttr(Type cache);
	void InvokeCacheAttached(PXCache cache);
	T CreateInstance(Object[] constructorArgs);
	PXEventSubscriberAttribute CreateInstance(Type t, Object[] constructorArgs);
	String ToString();
	Attribute[] GetCustomAttributes(MemberInfo element, Type type);
	Attribute[] GetCustomAttributes(MemberInfo element, Type type, Boolean inherit);
	Attribute[] GetCustomAttributes(MemberInfo element);
	Attribute[] GetCustomAttributes(MemberInfo element, Boolean inherit);
	Boolean IsDefined(MemberInfo element, Type attributeType);
	Boolean IsDefined(MemberInfo element, Type attributeType, Boolean inherit);
	Attribute GetCustomAttribute(MemberInfo element, Type attributeType);
	Attribute GetCustomAttribute(MemberInfo element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(ParameterInfo element);
	Attribute[] GetCustomAttributes(ParameterInfo element, Type attributeType);
	Attribute[] GetCustomAttributes(ParameterInfo element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(ParameterInfo element, Boolean inherit);
	Boolean IsDefined(ParameterInfo element, Type attributeType);
	Boolean IsDefined(ParameterInfo element, Type attributeType, Boolean inherit);
	Attribute GetCustomAttribute(ParameterInfo element, Type attributeType);
	Attribute GetCustomAttribute(ParameterInfo element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(Module element, Type attributeType);
	Attribute[] GetCustomAttributes(Module element);
	Attribute[] GetCustomAttributes(Module element, Boolean inherit);
	Attribute[] GetCustomAttributes(Module element, Type attributeType, Boolean inherit);
	Boolean IsDefined(Module element, Type attributeType);
	Boolean IsDefined(Module element, Type attributeType, Boolean inherit);
	Attribute GetCustomAttribute(Module element, Type attributeType);
	Attribute GetCustomAttribute(Module element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(Assembly element, Type attributeType);
	Attribute[] GetCustomAttributes(Assembly element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(Assembly element);
	Attribute[] GetCustomAttributes(Assembly element, Boolean inherit);
	Boolean IsDefined(Assembly element, Type attributeType);
	Boolean IsDefined(Assembly element, Type attributeType, Boolean inherit);
	Attribute GetCustomAttribute(Assembly element, Type attributeType);
	Attribute GetCustomAttribute(Assembly element, Type attributeType, Boolean inherit);
	Boolean Match(Object obj);
	Boolean IsDefaultAttribute();
	void System.Runtime.InteropServices._Attribute.GetTypeInfoCount(UInt32& pcTInfo);
	void System.Runtime.InteropServices._Attribute.GetTypeInfo(UInt32 iTInfo, UInt32 lcid, IntPtr ppTInfo);
	void System.Runtime.InteropServices._Attribute.GetIDsOfNames(Guid& riid, IntPtr rgszNames, UInt32 cNames, UInt32 lcid, IntPtr rgDispId);
	void System.Runtime.InteropServices._Attribute.Invoke(UInt32 dispIdMember, Guid& riid, UInt32 lcid, Int16 wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr);



public class ALPrintNodePrintJobStateAttribute:ALAggregateAttribute, _Attribute, IPXInterfaceField, IPXCommandPreparingSubscriber, IPXRowSelectingSubscriber
	Boolean IsDirty;
	Type BqlField;
	Boolean CacheGlobal;
	Object Constant;
	Type DescriptionField;
	Type SubstituteKey;
	Boolean DirtyRead;
	String DisplayName;
	Boolean Enabled;
	Boolean IsReadOnly;
	PXErrorHandling ErrorHandling;
	PXErrorLevel ErrorLevel;
	String ErrorText;
	Object ErrorValue;
	String FieldClass;
	Boolean Filterable;
	Boolean IsDBField;
	Boolean IsFixed;
	Boolean IsKey;
	PXCacheRights MapEnableRights;
	PXCacheRights MapViewRights;
	PXPersistingCheck PersistingCheck;
	Boolean Required;
	Int32 TabOrder;
	Boolean ValidateValue;
	Boolean SuppressVerify;
	Boolean ViewRights;
	PXUIVisibility Visibility;
	Boolean Visible;
	Type BqlTable;
	String FieldName;
	Int32 FieldOrdinal;
	Type CacheExtensionType;
	PXAttributeLevel AttributeLevel;
	Object TypeId;
	void CommandPreparing(PXCache sender, PXCommandPreparingEventArgs e);
	void ForceEnabled();
	void GetSubscriber(List<ISubscriber> subscribers);
	void Initialize();
	void RowSelecting(PXCache sender, PXRowSelectingEventArgs e);
	PXEventSubscriberAttribute[] GetAttributes();
	PXEventSubscriberAttribute[] GetAggregatedAttributes();
	T GetAttribute();
	PXEventSubscriberAttribute Clone(PXAttributeLevel attributeLevel);
	void InjectAttributeDependencies(PXCache cache);
	void CacheAttached(PXCache sender);
	void SetBqlTable(Type bqlTable);
	Boolean ChildrenAttributesComeFirstFor();
	Int32 GetHashCode();
	Boolean Equals(Object obj);
	Type GetBaseCacheWithTableAttr(Type cache);
	void InvokeCacheAttached(PXCache cache);
	T CreateInstance(Object[] constructorArgs);
	PXEventSubscriberAttribute CreateInstance(Type t, Object[] constructorArgs);
	String ToString();
	Attribute[] GetCustomAttributes(MemberInfo element, Type type);
	Attribute[] GetCustomAttributes(MemberInfo element, Type type, Boolean inherit);
	Attribute[] GetCustomAttributes(MemberInfo element);
	Attribute[] GetCustomAttributes(MemberInfo element, Boolean inherit);
	Boolean IsDefined(MemberInfo element, Type attributeType);
	Boolean IsDefined(MemberInfo element, Type attributeType, Boolean inherit);
	Attribute GetCustomAttribute(MemberInfo element, Type attributeType);
	Attribute GetCustomAttribute(MemberInfo element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(ParameterInfo element);
	Attribute[] GetCustomAttributes(ParameterInfo element, Type attributeType);
	Attribute[] GetCustomAttributes(ParameterInfo element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(ParameterInfo element, Boolean inherit);
	Boolean IsDefined(ParameterInfo element, Type attributeType);
	Boolean IsDefined(ParameterInfo element, Type attributeType, Boolean inherit);
	Attribute GetCustomAttribute(ParameterInfo element, Type attributeType);
	Attribute GetCustomAttribute(ParameterInfo element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(Module element, Type attributeType);
	Attribute[] GetCustomAttributes(Module element);
	Attribute[] GetCustomAttributes(Module element, Boolean inherit);
	Attribute[] GetCustomAttributes(Module element, Type attributeType, Boolean inherit);
	Boolean IsDefined(Module element, Type attributeType);
	Boolean IsDefined(Module element, Type attributeType, Boolean inherit);
	Attribute GetCustomAttribute(Module element, Type attributeType);
	Attribute GetCustomAttribute(Module element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(Assembly element, Type attributeType);
	Attribute[] GetCustomAttributes(Assembly element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(Assembly element);
	Attribute[] GetCustomAttributes(Assembly element, Boolean inherit);
	Boolean IsDefined(Assembly element, Type attributeType);
	Boolean IsDefined(Assembly element, Type attributeType, Boolean inherit);
	Attribute GetCustomAttribute(Assembly element, Type attributeType);
	Attribute GetCustomAttribute(Assembly element, Type attributeType, Boolean inherit);
	Boolean Match(Object obj);
	Boolean IsDefaultAttribute();
	void System.Runtime.InteropServices._Attribute.GetTypeInfoCount(UInt32& pcTInfo);
	void System.Runtime.InteropServices._Attribute.GetTypeInfo(UInt32 iTInfo, UInt32 lcid, IntPtr ppTInfo);
	void System.Runtime.InteropServices._Attribute.GetIDsOfNames(Guid& riid, IntPtr rgszNames, UInt32 cNames, UInt32 lcid, IntPtr rgDispId);
	void System.Runtime.InteropServices._Attribute.Invoke(UInt32 dispIdMember, Guid& riid, UInt32 lcid, Int16 wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr);



public class EzeepDestination:AbstractPrintDestination, IDestination, ISelectable
	String CODE;
	String Code;
	String Description;
	IPrinter Printer;
	FileResult DoPrint(ILabelContext lc, FileResult printResult);
	FileResult DoPrint(IPrintNodePrinter pnp, FileResult printResult);
	FileResult Print(ILabelContext lc, FileResult printResult);
	FileResult HandleTransformations(ILabelContext lc, FileResult printResult);



public class ALPrintNodeDeviceLink:BqlFormulaEvaluator<DeviceID>, IBqlCreator, IBqlVerifier, IBqlOperand
	Object Evaluate(PXCache cache, Object item, Dictionary<Type,Object> pars);
	Boolean AppendExpression(SQLExpression& exp, PXGraph graph, BqlCommandInfo info, Selection selection);
	void Verify(PXCache cache, Object item, List<Object> pars, Nullable`1& result, Object& value);
	Object Calculate(PXCache cache, Object item);
	void Verify(PXCache cache, Object item, IBqlCreator formula, Nullable`1& result, Object& value);
	Boolean IsContextualFormula(PXCache cache, Type Condition);
	Boolean GetOperandExpression(SQLExpression& exp, IBqlCreator& operand, PXGraph graph, BqlCommandInfo info, Selection selection);



public class IPrintNodePrintJob
	Int64 ID;
	Nullable<Int32> PrinterID;
	Nullable<Int32> ComputerID;
	String Title;
	String Source;
	String State;
	String ContentType;
	DateTime ExpireAt;
	DateTime CreateTimestamp;



public class PrintNodeDestination:AbstractPrintDestination, IDestination, ISelectable
	String CODE;
	String Code;
	String Description;
	IPrinter Printer;
	FileResult DoPrint(ILabelContext lc, FileResult printResult);
	FileResult DoPrint(IPrintNodePrinter pnp, FileResult printResult);
	FileResult Print(ILabelContext lc, FileResult printResult);
	FileResult HandleTransformations(ILabelContext lc, FileResult printResult);



public class PrintNodeHelper
	String WHOAMI;
	String DOWNLOAD_URL;
	String DEFAULT_BASE_URL;
	String DEFAULT_API_KEY;
	String SPEC_API_KEY;
	IDictionary<String,Object> Validate(String apiKey);
	ALUnboundPrintNodeComputer GetComputer(IPrintNodePrinter printer);
	ALUnboundPrintNodeComputer GetComputer(String apiKey, Nullable<Int32> computerID);
	IEnumerable<ALUnboundPrintNodeComputer> GetComputers(String apiKey, Nullable`1[] computerIDs);
	ALUnboundPrintNodePrinter GetPrinter(IPrintNodePrinter printer);
	ALUnboundPrintNodePrinter GetPrinter(String apiKey, Nullable<Int32> printerID);
	IEnumerable<ALUnboundPrintNodePrinter> GetPrinters(String apiKey, Nullable`1[] printerIDs);
	void AddLimit(RestRequest request, Int32 limit);
	void OrderBy(RestRequest request, Boolean asc);
	void GetPageAfter(RestRequest request, Int32 lastID);
	IEnumerable<ALUnboundPrintNodePrinter> GetPrintersByComputers(String apiKey, Nullable`1[] computerIDs);
	IEnumerable<ALUnboundPrintNodePrinter> GetPrintersByComputersInternal(String apiKey, Nullable`1[] computerIDs);
	IEnumerable<IPrintNodePrintJob> GetPrintJobsByPrinter(String apiKey, Nullable<Int32> printNodePrinterID);
	IEnumerable<IPrintNodePrintJob> GetPrintJobs(String apiKey, Nullable`1[] printJobIDs);
	String PostPrintJob(PrintNodePrintJobRequest printJobRequest, IPrintNodePrinter printer);
	void DeletePrintJobs(Nullable`1[] printJobIDs);
	void DeletePrintJobsByPrinter(IPrintNodePrinter printer, Nullable`1[] printJobIDs);
	IList<PrintNodePrintJobState[]> GetPrintJobStates(String apiKey, Nullable`1[] printJobIDs);
	void Ping();
	void CheckAuth();
	ValueTuple<RestClient,RestRequest> GetClientRequest(String resource, String specificAPIKey, Method method, Boolean doAuth);
	T HandleResponse(RestClient restClient, RestRequest request, RestResponse response, HttpStatusCode[] expectedCodes);
	String GetAsList(String setFormat, Nullable`1[] ids);
	String GetAsList2(String setFormat, Nullable`1[] ids1, Nullable`1[] ids2);
	String GetContentType(IPrintNodePrinter printer, IFileInfo labelFile);
	FileResult DoPrint(IPrintNodePrinter pnp, FileResult printResult, Options options);



public class ALUnboundPrintNodeComputer:PXBqlTable, IBqlTableSystemDataStorage, IBqlTable, IPrintNodeComputer, IPrintNodeObject
	Int32 STATE_LENGTH;
	Nullable<Int32> ComputerID;
	ComputerStatus Status;
	Nullable<Int32> ID;
	String Name;
	String State;
	String Inet;
	String PrintNodeAPIKey;
	PXBqlTableSystemData& PX.Data.IBqlTableSystemDataStorage.GetBqlTableSystemData();



public class ALUnboundPrintNodePrinter:PXBqlTable, IBqlTableSystemDataStorage, IBqlTable, IPrintNodePrinter, IPrintNodeObject
	Int32 STATE_LENGTH;
	String STATE_UNKNOWN;
	Nullable<Int32> PrintNodePrinterID;
	String PrinterState;
	PrinterStatus Status;
	ComputerStatus ComputerStatus;
	Nullable<Int32> ID;
	String Name;
	String Description;
	String State;
	ALUnboundPrintNodeComputer Computer;
	IDictionary<String,Object> Capabilities;
	String PrintNodeAPIKey;
	Nullable<Int32> PrintNodeComputerID;
	String ComputerName;
	String ComputerInet;
	String ComputerState;
	PXBqlTableSystemData& PX.Data.IBqlTableSystemDataStorage.GetBqlTableSystemData();



public class PrintNodePrintJob:IPrintNodePrintJob
	ALUnboundPrintNodePrinter Printer;
	Int64 ID;
	String Title;
	String State;
	String ContentType;
	String Source;
	DateTime ExpireAt;
	DateTime CreateTimestamp;
	String ApiKey;
	Nullable<Int32> PrinterID;
	Nullable<Int32> ComputerID;



public class PrintNodePrintJobRequest
	Int32 printerId;
	String source;
	String title;
	String contentType;
	String content;
	Nullable<Int32> expireAfter;
	Int32 qty;
	Authentication authentication;
	Options options;



public class PrintNodePrintJobState
	Int32 LENGTH;
	Int64 PrintJobId;
	String State;
	Nullable<DateTime> CreateTimestamp;
	String Message;



public class PrintNodeWebhook
	Int64 ID;
	String Secret;
	String Url;
	String Messages;



public class [nested] value:Field<IBqlString,String,value>, IBqlOperand, IImplement<IBqlString>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<value>, IBqlField



public class [nested] description:Field<IBqlString,String,description>, IBqlOperand, IImplement<IBqlString>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<description>, IBqlField



public class [nested] new_:Constant<IBqlString,String,new_>, IBqlOperand, IImplement<IBqlString>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<new_>, IConstant<String>, IConstant, IBqlAggregatedOperand, IBqlCreator, IBqlVerifier
	String Value;
	void PX.Data.IBqlVerifier.Verify(PXCache cache, Object item, List<Object> pars, Nullable`1& result, Object& value);
	Boolean PX.Data.IBqlCreator.AppendExpression(SQLExpression& exp, PXGraph graph, BqlCommandInfo info, Selection selection);



public class [nested] sent_to_client:Constant<IBqlString,String,sent_to_client>, IBqlOperand, IImplement<IBqlString>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<sent_to_client>, IConstant<String>, IConstant, IBqlAggregatedOperand, IBqlCreator, IBqlVerifier
	String Value;
	void PX.Data.IBqlVerifier.Verify(PXCache cache, Object item, List<Object> pars, Nullable`1& result, Object& value);
	Boolean PX.Data.IBqlCreator.AppendExpression(SQLExpression& exp, PXGraph graph, BqlCommandInfo info, Selection selection);



public class [nested] queued:Constant<IBqlString,String,queued>, IBqlOperand, IImplement<IBqlString>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<queued>, IConstant<String>, IConstant, IBqlAggregatedOperand, IBqlCreator, IBqlVerifier
	String Value;
	void PX.Data.IBqlVerifier.Verify(PXCache cache, Object item, List<Object> pars, Nullable`1& result, Object& value);
	Boolean PX.Data.IBqlCreator.AppendExpression(SQLExpression& exp, PXGraph graph, BqlCommandInfo info, Selection selection);



public class [nested] in_progress:Constant<IBqlString,String,in_progress>, IBqlOperand, IImplement<IBqlString>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<in_progress>, IConstant<String>, IConstant, IBqlAggregatedOperand, IBqlCreator, IBqlVerifier
	String Value;
	void PX.Data.IBqlVerifier.Verify(PXCache cache, Object item, List<Object> pars, Nullable`1& result, Object& value);
	Boolean PX.Data.IBqlCreator.AppendExpression(SQLExpression& exp, PXGraph graph, BqlCommandInfo info, Selection selection);



public class [nested] done:Constant<IBqlString,String,done>, IBqlOperand, IImplement<IBqlString>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<done>, IConstant<String>, IConstant, IBqlAggregatedOperand, IBqlCreator, IBqlVerifier
	String Value;
	void PX.Data.IBqlVerifier.Verify(PXCache cache, Object item, List<Object> pars, Nullable`1& result, Object& value);
	Boolean PX.Data.IBqlCreator.AppendExpression(SQLExpression& exp, PXGraph graph, BqlCommandInfo info, Selection selection);



public class [nested] expired:Constant<IBqlString,String,expired>, IBqlOperand, IImplement<IBqlString>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<expired>, IConstant<String>, IConstant, IBqlAggregatedOperand, IBqlCreator, IBqlVerifier
	String Value;
	void PX.Data.IBqlVerifier.Verify(PXCache cache, Object item, List<Object> pars, Nullable`1& result, Object& value);
	Boolean PX.Data.IBqlCreator.AppendExpression(SQLExpression& exp, PXGraph graph, BqlCommandInfo info, Selection selection);



public class [nested] connected:Constant<IBqlString,String,connected>, IBqlOperand, IImplement<IBqlString>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<connected>, IConstant<String>, IConstant, IBqlAggregatedOperand, IBqlCreator, IBqlVerifier
	String Value;
	void PX.Data.IBqlVerifier.Verify(PXCache cache, Object item, List<Object> pars, Nullable`1& result, Object& value);
	Boolean PX.Data.IBqlCreator.AppendExpression(SQLExpression& exp, PXGraph graph, BqlCommandInfo info, Selection selection);



public class [nested] idle:Constant<IBqlString,String,idle>, IBqlOperand, IImplement<IBqlString>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<idle>, IConstant<String>, IConstant, IBqlAggregatedOperand, IBqlCreator, IBqlVerifier
	String Value;
	void PX.Data.IBqlVerifier.Verify(PXCache cache, Object item, List<Object> pars, Nullable`1& result, Object& value);
	Boolean PX.Data.IBqlCreator.AppendExpression(SQLExpression& exp, PXGraph graph, BqlCommandInfo info, Selection selection);



public class [nested] out_of_paper:Constant<IBqlString,String,out_of_paper>, IBqlOperand, IImplement<IBqlString>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<out_of_paper>, IConstant<String>, IConstant, IBqlAggregatedOperand, IBqlCreator, IBqlVerifier
	String Value;
	void PX.Data.IBqlVerifier.Verify(PXCache cache, Object item, List<Object> pars, Nullable`1& result, Object& value);
	Boolean PX.Data.IBqlCreator.AppendExpression(SQLExpression& exp, PXGraph graph, BqlCommandInfo info, Selection selection);



public class [nested] disconnected:Constant<IBqlString,String,disconnected>, IBqlOperand, IImplement<IBqlString>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<disconnected>, IConstant<String>, IConstant, IBqlAggregatedOperand, IBqlCreator, IBqlVerifier
	String Value;
	void PX.Data.IBqlVerifier.Verify(PXCache cache, Object item, List<Object> pars, Nullable`1& result, Object& value);
	Boolean PX.Data.IBqlCreator.AppendExpression(SQLExpression& exp, PXGraph graph, BqlCommandInfo info, Selection selection);



public class [nested] error:Constant<IBqlString,String,error>, IBqlOperand, IImplement<IBqlString>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<error>, IConstant<String>, IConstant, IBqlAggregatedOperand, IBqlCreator, IBqlVerifier
	String Value;
	void PX.Data.IBqlVerifier.Verify(PXCache cache, Object item, List<Object> pars, Nullable`1& result, Object& value);
	Boolean PX.Data.IBqlCreator.AppendExpression(SQLExpression& exp, PXGraph graph, BqlCommandInfo info, Selection selection);



public class [nested] offline:Constant<IBqlString,String,offline>, IBqlOperand, IImplement<IBqlString>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<offline>, IConstant<String>, IConstant, IBqlAggregatedOperand, IBqlCreator, IBqlVerifier
	String Value;
	void PX.Data.IBqlVerifier.Verify(PXCache cache, Object item, List<Object> pars, Nullable`1& result, Object& value);
	Boolean PX.Data.IBqlCreator.AppendExpression(SQLExpression& exp, PXGraph graph, BqlCommandInfo info, Selection selection);



public class [nested] online:Constant<IBqlString,String,online>, IBqlOperand, IImplement<IBqlString>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<online>, IConstant<String>, IConstant, IBqlAggregatedOperand, IBqlCreator, IBqlVerifier
	String Value;
	void PX.Data.IBqlVerifier.Verify(PXCache cache, Object item, List<Object> pars, Nullable`1& result, Object& value);
	Boolean PX.Data.IBqlCreator.AppendExpression(SQLExpression& exp, PXGraph graph, BqlCommandInfo info, Selection selection);



public class [nested] unknown:Constant<IBqlString,String,unknown>, IBqlOperand, IImplement<IBqlString>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<unknown>, IConstant<String>, IConstant, IBqlAggregatedOperand, IBqlCreator, IBqlVerifier
	String Value;
	void PX.Data.IBqlVerifier.Verify(PXCache cache, Object item, List<Object> pars, Nullable`1& result, Object& value);
	Boolean PX.Data.IBqlCreator.AppendExpression(SQLExpression& exp, PXGraph graph, BqlCommandInfo info, Selection selection);



public class [nested] SizeUnit



public class [nested] ReturnTypes



public class [nested] ScreenIDs



public class [nested] cloudDeviceHubID:Constant<IBqlString,String,cloudDeviceHubID>, IBqlOperand, IImplement<IBqlString>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<cloudDeviceHubID>, IConstant<String>, IConstant, IBqlAggregatedOperand, IBqlCreator, IBqlVerifier
	String Value;
	void PX.Data.IBqlVerifier.Verify(PXCache cache, Object item, List<Object> pars, Nullable`1& result, Object& value);
	Boolean PX.Data.IBqlCreator.AppendExpression(SQLExpression& exp, PXGraph graph, BqlCommandInfo info, Selection selection);



public class [nested] Destinations



public class [nested] PrintNode
	PrinterStatus ToPrinterStatus(String state);
	ComputerStatus ToComputerStatus(String state);
	JobStatus ToJobStatus(String state);



public class [nested] ACViewName



public class [nested] ACDACName



public class [nested] ACCommand



public class [nested] ALListAttribute:ALAbstractStringListAttribute, _Attribute, IPXFieldSelectingSubscriber, IPXLocalizableList
	Boolean IsDirty;
	Boolean IsLocalizable;
	Boolean IsLocalized;
	Boolean SortByValues;
	Boolean MultiSelect;
	Boolean ExclusiveValues;
	Type BqlField;
	Dictionary<String,String> ValueLabelDic;
	PXAttributeLevel AttributeLevel;
	Type BqlTable;
	Type CacheExtensionType;
	String FieldName;
	Int32 FieldOrdinal;
	Object TypeId;
	ValueTuple`2[] GetTuples();
	String[] GetAllowedValues(PXCache cache);
	String[] SplitMultiSelectValues(String values);
	String GetLocalizedLabel(PXCache cache, Object row);
	String GetLocalizedLabel(PXCache cache, Object row, String value);
	void SetLocalizable(PXCache cache, Object data, Boolean isLocalizable);
	void SetList(PXCache cache, Object data, PXStringListAttribute listSource);
	void SetList(PXCache cache, Object data, String[] allowedValues, String[] allowedLabels);
	void SetList(PXCache cache, Object data, Tuple`2[] valuesToLabels);
	void SetList(PXCache cache, Object data, ValueTuple`2[] valuesToLabels);
	void SetList(PXCache cache, Object data, String field, String[] allowedValues, String[] allowedLabels);
	void SetList(PXCache cache, Object data, String field, PXStringListAttribute listSource);
	void SetList(PXCache cache, Object data, String field, Tuple`2[] valuesToLabels);
	void SetList(PXCache cache, Object data, String field, ValueTuple`2[] valuesToLabels);
	void SetListInternal(IEnumerable<PXStringListAttribute> attributes, String[] allowedValues, String[] allowedLabels, PXCache cache);
	void AppendList(PXCache cache, Object data, ValueTuple`2[] valuesToLabels);
	void AppendList(PXCache cache, Object data, String[] allowedValues, String[] allowedLabels);
	void AppendList(PXCache cache, Object data, String field, String[] allowedValues, String[] allowedLabels);
	void SetExclusiveValues(PXCache cache, Object data, Boolean exclusiveValues);
	void SetExclusiveValues(PXCache cache, Object data, String field, Boolean exclusiveValues);
	void FieldSelecting(PXCache sender, PXFieldSelectingEventArgs e);
	void OrderByCommandPreparing(PXCache sender, PXCommandPreparingEventArgs e);
	void CacheAttached(PXCache sender);
	void MultiSelectFieldUpdating(PXCache sender, PXFieldUpdatingEventArgs e);
	void RemoveDisabledValues(String cacheName);
	void TryLocalize(PXCache sender);
	void RipDynamicLabels(String[] dynamicAllowedLabels, PXCache sender);
	Tuple<String,String> Pair(String value, String label);
	Int32 GetHashCode();
	Boolean Equals(Object obj);
	PXEventSubscriberAttribute Clone(PXAttributeLevel attributeLevel);
	Type GetBaseCacheWithTableAttr(Type cache);
	void SetBqlTable(Type bqlTable);
	void InjectAttributeDependencies(PXCache cache);
	void InvokeCacheAttached(PXCache cache);
	void GetSubscriber(List<ISubscriber> subscribers);
	T CreateInstance(Object[] constructorArgs);
	PXEventSubscriberAttribute CreateInstance(Type t, Object[] constructorArgs);
	String ToString();
	Attribute[] GetCustomAttributes(MemberInfo element, Type type);
	Attribute[] GetCustomAttributes(MemberInfo element, Type type, Boolean inherit);
	Attribute[] GetCustomAttributes(MemberInfo element);
	Attribute[] GetCustomAttributes(MemberInfo element, Boolean inherit);
	Boolean IsDefined(MemberInfo element, Type attributeType);
	Boolean IsDefined(MemberInfo element, Type attributeType, Boolean inherit);
	Attribute GetCustomAttribute(MemberInfo element, Type attributeType);
	Attribute GetCustomAttribute(MemberInfo element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(ParameterInfo element);
	Attribute[] GetCustomAttributes(ParameterInfo element, Type attributeType);
	Attribute[] GetCustomAttributes(ParameterInfo element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(ParameterInfo element, Boolean inherit);
	Boolean IsDefined(ParameterInfo element, Type attributeType);
	Boolean IsDefined(ParameterInfo element, Type attributeType, Boolean inherit);
	Attribute GetCustomAttribute(ParameterInfo element, Type attributeType);
	Attribute GetCustomAttribute(ParameterInfo element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(Module element, Type attributeType);
	Attribute[] GetCustomAttributes(Module element);
	Attribute[] GetCustomAttributes(Module element, Boolean inherit);
	Attribute[] GetCustomAttributes(Module element, Type attributeType, Boolean inherit);
	Boolean IsDefined(Module element, Type attributeType);
	Boolean IsDefined(Module element, Type attributeType, Boolean inherit);
	Attribute GetCustomAttribute(Module element, Type attributeType);
	Attribute GetCustomAttribute(Module element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(Assembly element, Type attributeType);
	Attribute[] GetCustomAttributes(Assembly element, Type attributeType, Boolean inherit);
	Attribute[] GetCustomAttributes(Assembly element);
	Attribute[] GetCustomAttributes(Assembly element, Boolean inherit);
	Boolean IsDefined(Assembly element, Type attributeType);
	Boolean IsDefined(Assembly element, Type attributeType, Boolean inherit);
	Attribute GetCustomAttribute(Assembly element, Type attributeType);
	Attribute GetCustomAttribute(Assembly element, Type attributeType, Boolean inherit);
	Boolean Match(Object obj);
	Boolean IsDefaultAttribute();
	void System.Runtime.InteropServices._Attribute.GetTypeInfoCount(UInt32& pcTInfo);
	void System.Runtime.InteropServices._Attribute.GetTypeInfo(UInt32 iTInfo, UInt32 lcid, IntPtr ppTInfo);
	void System.Runtime.InteropServices._Attribute.GetIDsOfNames(Guid& riid, IntPtr rgszNames, UInt32 cNames, UInt32 lcid, IntPtr rgDispId);
	void System.Runtime.InteropServices._Attribute.Invoke(UInt32 dispIdMember, Guid& riid, UInt32 lcid, Int16 wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr);



public class [nested] OperatingSystem:Enum, IComparable, IFormattable, IConvertible
	Int32 value__;
	OperatingSystem Windows;
	OperatingSystem Osx;
	UInt64 ToUInt64(Object value);
	Object Parse(Type enumType, String value);
	Object Parse(Type enumType, String value, Boolean ignoreCase);
	Type GetUnderlyingType(Type enumType);
	Array GetValues(Type enumType);
	UInt64[] InternalGetValues(RuntimeType enumType);
	String GetName(Type enumType, Object value);
	String[] GetNames(Type enumType);
	String[] InternalGetNames(RuntimeType enumType);
	Object ToObject(Type enumType, Object value);
	Boolean IsDefined(Type enumType, Object value);
	String Format(Type enumType, Object value, String format);
	Object GetValue();
	Int32 GetHashCode();
	String ToString();
	String ToString(String format, IFormatProvider provider);
	Int32 CompareTo(Object target);
	String ToString(IFormatProvider provider);
	Boolean HasFlag(Enum flag);
	TypeCode GetTypeCode();
	Boolean System.IConvertible.ToBoolean(IFormatProvider provider);
	Char System.IConvertible.ToChar(IFormatProvider provider);
	SByte System.IConvertible.ToSByte(IFormatProvider provider);
	Byte System.IConvertible.ToByte(IFormatProvider provider);
	Int16 System.IConvertible.ToInt16(IFormatProvider provider);
	UInt16 System.IConvertible.ToUInt16(IFormatProvider provider);
	Int32 System.IConvertible.ToInt32(IFormatProvider provider);
	UInt32 System.IConvertible.ToUInt32(IFormatProvider provider);
	Int64 System.IConvertible.ToInt64(IFormatProvider provider);
	UInt64 System.IConvertible.ToUInt64(IFormatProvider provider);
	Single System.IConvertible.ToSingle(IFormatProvider provider);
	Double System.IConvertible.ToDouble(IFormatProvider provider);
	Decimal System.IConvertible.ToDecimal(IFormatProvider provider);
	DateTime System.IConvertible.ToDateTime(IFormatProvider provider);
	Object System.IConvertible.ToType(Type type, IFormatProvider provider);
	Object ToObject(Type enumType, SByte value);
	Object ToObject(Type enumType, Int16 value);
	Object ToObject(Type enumType, Int32 value);
	Object ToObject(Type enumType, Byte value);
	Object ToObject(Type enumType, UInt16 value);
	Object ToObject(Type enumType, UInt32 value);
	Object ToObject(Type enumType, Int64 value);
	Object ToObject(Type enumType, UInt64 value);
	Boolean TryParse(String value, TEnum& result);
	Boolean TryParse(String value, Boolean ignoreCase, TEnum& result);
	Boolean Equals(Object obj);
	RuntimeType InternalGetUnderlyingType(RuntimeType enumType);
	String ToString(String format);
	Int32 GetHashCodeOfPtr(IntPtr ptr);



public class [nested] iD:Field<IBqlInt,Int32,iD>, IBqlOperand, IImplement<IBqlInt>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<iD>, IBqlField



public class [nested] name:Field<IBqlString,String,name>, IBqlOperand, IImplement<IBqlString>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<name>, IBqlField



public class [nested] state:Field<IBqlString,String,state>, IBqlOperand, IImplement<IBqlString>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<state>, IBqlField



public class [nested] inet:Field<IBqlString,String,inet>, IBqlOperand, IImplement<IBqlString>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<inet>, IBqlField



public class [nested] iD:Field<IBqlInt,Int32,iD>, IBqlOperand, IImplement<IBqlInt>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<iD>, IBqlField



public class [nested] name:Field<IBqlString,String,name>, IBqlOperand, IImplement<IBqlString>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<name>, IBqlField



public class [nested] description:Field<IBqlString,String,description>, IBqlOperand, IImplement<IBqlString>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<description>, IBqlField



public class [nested] state:Field<IBqlString,String,state>, IBqlOperand, IImplement<IBqlString>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<state>, IBqlField



public class [nested] printNodeAPIKey:Field<IBqlString,String,printNodeAPIKey>, IBqlOperand, IImplement<IBqlString>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<printNodeAPIKey>, IBqlField



public class [nested] printNodeComputerID:Field<IBqlInt,Int32,printNodeComputerID>, IBqlOperand, IImplement<IBqlInt>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<printNodeComputerID>, IBqlField



public class [nested] computerName:Field<IBqlString,String,computerName>, IBqlOperand, IImplement<IBqlString>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<computerName>, IBqlField



public class [nested] computerInet:Field<IBqlString,String,computerInet>, IBqlOperand, IImplement<IBqlString>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<computerInet>, IBqlField



public class [nested] computerState:Field<IBqlString,String,computerState>, IBqlOperand, IImplement<IBqlString>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<computerState>, IBqlField



public class [nested] CONTENT_TYPE
	String PDF_BASE64;
	String RAW_BASE64;



public class [nested] Options
	Nullable<Int32> copies;
	String dpi;
	String pages;
	String duplex;
	String paper;
	String bin;
	Nullable<Boolean> color;
	Nullable<Boolean> collate;
	Nullable<Boolean> rotate;
	Nullable<Boolean> fit_to_page;
	Nullable<Int32> nup;
	String media;



public class [nested] Authentication
	String type;
	Credentials credentials;



public class [nested] Credentials
	String user;
	String pass;



public class [nested] dot:Constant<IBqlString,String,dot>, IBqlOperand, IImplement<IBqlString>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<dot>, IConstant<String>, IConstant, IBqlAggregatedOperand, IBqlCreator, IBqlVerifier
	String Value;
	void PX.Data.IBqlVerifier.Verify(PXCache cache, Object item, List<Object> pars, Nullable`1& result, Object& value);
	Boolean PX.Data.IBqlCreator.AppendExpression(SQLExpression& exp, PXGraph graph, BqlCommandInfo info, Selection selection);



public class [nested] point:Constant<IBqlString,String,point>, IBqlOperand, IImplement<IBqlString>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<point>, IConstant<String>, IConstant, IBqlAggregatedOperand, IBqlCreator, IBqlVerifier
	String Value;
	void PX.Data.IBqlVerifier.Verify(PXCache cache, Object item, List<Object> pars, Nullable`1& result, Object& value);
	Boolean PX.Data.IBqlCreator.AppendExpression(SQLExpression& exp, PXGraph graph, BqlCommandInfo info, Selection selection);



public class [nested] _void:Constant<IBqlString,String,_void>, IBqlOperand, IImplement<IBqlString>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<_void>, IConstant<String>, IConstant, IBqlAggregatedOperand, IBqlCreator, IBqlVerifier
	String Value;
	void PX.Data.IBqlVerifier.Verify(PXCache cache, Object item, List<Object> pars, Nullable`1& result, Object& value);
	Boolean PX.Data.IBqlCreator.AppendExpression(SQLExpression& exp, PXGraph graph, BqlCommandInfo info, Selection selection);



public class [nested] Shipments:Constant<IBqlString,String,Shipments>, IBqlOperand, IImplement<IBqlString>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<Shipments>, IConstant<String>, IConstant, IBqlAggregatedOperand, IBqlCreator, IBqlVerifier
	String Value;
	void PX.Data.IBqlVerifier.Verify(PXCache cache, Object item, List<Object> pars, Nullable`1& result, Object& value);
	Boolean PX.Data.IBqlCreator.AppendExpression(SQLExpression& exp, PXGraph graph, BqlCommandInfo info, Selection selection);



public class [nested] ProductionOrders:Constant<IBqlString,String,ProductionOrders>, IBqlOperand, IImplement<IBqlString>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<ProductionOrders>, IConstant<String>, IConstant, IBqlAggregatedOperand, IBqlCreator, IBqlVerifier
	String Value;
	void PX.Data.IBqlVerifier.Verify(PXCache cache, Object item, List<Object> pars, Nullable`1& result, Object& value);
	Boolean PX.Data.IBqlCreator.AppendExpression(SQLExpression& exp, PXGraph graph, BqlCommandInfo info, Selection selection);



public class [nested] printNode:Constant<IBqlString,String,printNode>, IBqlOperand, IImplement<IBqlString>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<printNode>, IConstant<String>, IConstant, IBqlAggregatedOperand, IBqlCreator, IBqlVerifier
	String Value;
	void PX.Data.IBqlVerifier.Verify(PXCache cache, Object item, List<Object> pars, Nullable`1& result, Object& value);
	Boolean PX.Data.IBqlCreator.AppendExpression(SQLExpression& exp, PXGraph graph, BqlCommandInfo info, Selection selection);



public class [nested] PrinterStatus:Enum, IComparable, IFormattable, IConvertible
	Int32 value__;
	PrinterStatus unknown;
	PrinterStatus out_of_paper;
	PrinterStatus disconnected;
	PrinterStatus error;
	PrinterStatus idle;
	PrinterStatus offline;
	PrinterStatus online;
	UInt64 ToUInt64(Object value);
	Object Parse(Type enumType, String value);
	Object Parse(Type enumType, String value, Boolean ignoreCase);
	Type GetUnderlyingType(Type enumType);
	Array GetValues(Type enumType);
	UInt64[] InternalGetValues(RuntimeType enumType);
	String GetName(Type enumType, Object value);
	String[] GetNames(Type enumType);
	String[] InternalGetNames(RuntimeType enumType);
	Object ToObject(Type enumType, Object value);
	Boolean IsDefined(Type enumType, Object value);
	String Format(Type enumType, Object value, String format);
	Object GetValue();
	Int32 GetHashCode();
	String ToString();
	String ToString(String format, IFormatProvider provider);
	Int32 CompareTo(Object target);
	String ToString(IFormatProvider provider);
	Boolean HasFlag(Enum flag);
	TypeCode GetTypeCode();
	Boolean System.IConvertible.ToBoolean(IFormatProvider provider);
	Char System.IConvertible.ToChar(IFormatProvider provider);
	SByte System.IConvertible.ToSByte(IFormatProvider provider);
	Byte System.IConvertible.ToByte(IFormatProvider provider);
	Int16 System.IConvertible.ToInt16(IFormatProvider provider);
	UInt16 System.IConvertible.ToUInt16(IFormatProvider provider);
	Int32 System.IConvertible.ToInt32(IFormatProvider provider);
	UInt32 System.IConvertible.ToUInt32(IFormatProvider provider);
	Int64 System.IConvertible.ToInt64(IFormatProvider provider);
	UInt64 System.IConvertible.ToUInt64(IFormatProvider provider);
	Single System.IConvertible.ToSingle(IFormatProvider provider);
	Double System.IConvertible.ToDouble(IFormatProvider provider);
	Decimal System.IConvertible.ToDecimal(IFormatProvider provider);
	DateTime System.IConvertible.ToDateTime(IFormatProvider provider);
	Object System.IConvertible.ToType(Type type, IFormatProvider provider);
	Object ToObject(Type enumType, SByte value);
	Object ToObject(Type enumType, Int16 value);
	Object ToObject(Type enumType, Int32 value);
	Object ToObject(Type enumType, Byte value);
	Object ToObject(Type enumType, UInt16 value);
	Object ToObject(Type enumType, UInt32 value);
	Object ToObject(Type enumType, Int64 value);
	Object ToObject(Type enumType, UInt64 value);
	Boolean TryParse(String value, TEnum& result);
	Boolean TryParse(String value, Boolean ignoreCase, TEnum& result);
	Boolean Equals(Object obj);
	RuntimeType InternalGetUnderlyingType(RuntimeType enumType);
	String ToString(String format);
	Int32 GetHashCodeOfPtr(IntPtr ptr);



public class [nested] ComputerStatus:Enum, IComparable, IFormattable, IConvertible
	Int32 value__;
	ComputerStatus unknown;
	ComputerStatus connected;
	ComputerStatus disconnected;
	UInt64 ToUInt64(Object value);
	Object Parse(Type enumType, String value);
	Object Parse(Type enumType, String value, Boolean ignoreCase);
	Type GetUnderlyingType(Type enumType);
	Array GetValues(Type enumType);
	UInt64[] InternalGetValues(RuntimeType enumType);
	String GetName(Type enumType, Object value);
	String[] GetNames(Type enumType);
	String[] InternalGetNames(RuntimeType enumType);
	Object ToObject(Type enumType, Object value);
	Boolean IsDefined(Type enumType, Object value);
	String Format(Type enumType, Object value, String format);
	Object GetValue();
	Int32 GetHashCode();
	String ToString();
	String ToString(String format, IFormatProvider provider);
	Int32 CompareTo(Object target);
	String ToString(IFormatProvider provider);
	Boolean HasFlag(Enum flag);
	TypeCode GetTypeCode();
	Boolean System.IConvertible.ToBoolean(IFormatProvider provider);
	Char System.IConvertible.ToChar(IFormatProvider provider);
	SByte System.IConvertible.ToSByte(IFormatProvider provider);
	Byte System.IConvertible.ToByte(IFormatProvider provider);
	Int16 System.IConvertible.ToInt16(IFormatProvider provider);
	UInt16 System.IConvertible.ToUInt16(IFormatProvider provider);
	Int32 System.IConvertible.ToInt32(IFormatProvider provider);
	UInt32 System.IConvertible.ToUInt32(IFormatProvider provider);
	Int64 System.IConvertible.ToInt64(IFormatProvider provider);
	UInt64 System.IConvertible.ToUInt64(IFormatProvider provider);
	Single System.IConvertible.ToSingle(IFormatProvider provider);
	Double System.IConvertible.ToDouble(IFormatProvider provider);
	Decimal System.IConvertible.ToDecimal(IFormatProvider provider);
	DateTime System.IConvertible.ToDateTime(IFormatProvider provider);
	Object System.IConvertible.ToType(Type type, IFormatProvider provider);
	Object ToObject(Type enumType, SByte value);
	Object ToObject(Type enumType, Int16 value);
	Object ToObject(Type enumType, Int32 value);
	Object ToObject(Type enumType, Byte value);
	Object ToObject(Type enumType, UInt16 value);
	Object ToObject(Type enumType, UInt32 value);
	Object ToObject(Type enumType, Int64 value);
	Object ToObject(Type enumType, UInt64 value);
	Boolean TryParse(String value, TEnum& result);
	Boolean TryParse(String value, Boolean ignoreCase, TEnum& result);
	Boolean Equals(Object obj);
	RuntimeType InternalGetUnderlyingType(RuntimeType enumType);
	String ToString(String format);
	Int32 GetHashCodeOfPtr(IntPtr ptr);



public class [nested] JobStatus:Enum, IComparable, IFormattable, IConvertible
	Int32 value__;
	JobStatus unknown;
	JobStatus _new;
	JobStatus sent_to_client;
	JobStatus queued;
	JobStatus in_progress;
	JobStatus error;
	JobStatus done;
	JobStatus expired;
	UInt64 ToUInt64(Object value);
	Object Parse(Type enumType, String value);
	Object Parse(Type enumType, String value, Boolean ignoreCase);
	Type GetUnderlyingType(Type enumType);
	Array GetValues(Type enumType);
	UInt64[] InternalGetValues(RuntimeType enumType);
	String GetName(Type enumType, Object value);
	String[] GetNames(Type enumType);
	String[] InternalGetNames(RuntimeType enumType);
	Object ToObject(Type enumType, Object value);
	Boolean IsDefined(Type enumType, Object value);
	String Format(Type enumType, Object value, String format);
	Object GetValue();
	Int32 GetHashCode();
	String ToString();
	String ToString(String format, IFormatProvider provider);
	Int32 CompareTo(Object target);
	String ToString(IFormatProvider provider);
	Boolean HasFlag(Enum flag);
	TypeCode GetTypeCode();
	Boolean System.IConvertible.ToBoolean(IFormatProvider provider);
	Char System.IConvertible.ToChar(IFormatProvider provider);
	SByte System.IConvertible.ToSByte(IFormatProvider provider);
	Byte System.IConvertible.ToByte(IFormatProvider provider);
	Int16 System.IConvertible.ToInt16(IFormatProvider provider);
	UInt16 System.IConvertible.ToUInt16(IFormatProvider provider);
	Int32 System.IConvertible.ToInt32(IFormatProvider provider);
	UInt32 System.IConvertible.ToUInt32(IFormatProvider provider);
	Int64 System.IConvertible.ToInt64(IFormatProvider provider);
	UInt64 System.IConvertible.ToUInt64(IFormatProvider provider);
	Single System.IConvertible.ToSingle(IFormatProvider provider);
	Double System.IConvertible.ToDouble(IFormatProvider provider);
	Decimal System.IConvertible.ToDecimal(IFormatProvider provider);
	DateTime System.IConvertible.ToDateTime(IFormatProvider provider);
	Object System.IConvertible.ToType(Type type, IFormatProvider provider);
	Object ToObject(Type enumType, SByte value);
	Object ToObject(Type enumType, Int16 value);
	Object ToObject(Type enumType, Int32 value);
	Object ToObject(Type enumType, Byte value);
	Object ToObject(Type enumType, UInt16 value);
	Object ToObject(Type enumType, UInt32 value);
	Object ToObject(Type enumType, Int64 value);
	Object ToObject(Type enumType, UInt64 value);
	Boolean TryParse(String value, TEnum& result);
	Boolean TryParse(String value, Boolean ignoreCase, TEnum& result);
	Boolean Equals(Object obj);
	RuntimeType InternalGetUnderlyingType(RuntimeType enumType);
	String ToString(String format);
	Int32 GetHashCodeOfPtr(IntPtr ptr);



public class [nested] BqlPrinterStatus
	String OUT_OF_PAPER;
	String DISCONNECTED;
	String ERROR;
	String IDLE;
	String OFFLINE;
	String ONLINE;
	String UNKNOWN;



public class [nested] BqlComputerStatus
	String CONNECTED;
	String DISCONNECTED;
	String UNKNOWN;



public class [nested] BqlJobStatus
	String NEW;
	String SENT_TO_CLIENT;
	String QUEUED;
	String IN_PROGRESS;
	String ERROR;
	String DONE;
	String EXPIRED;
	String UNKNOWN;



public class [nested] out_of_paper:Constant<IBqlString,String,out_of_paper>, IBqlOperand, IImplement<IBqlString>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<out_of_paper>, IConstant<String>, IConstant, IBqlAggregatedOperand, IBqlCreator, IBqlVerifier
	String Value;
	void PX.Data.IBqlVerifier.Verify(PXCache cache, Object item, List<Object> pars, Nullable`1& result, Object& value);
	Boolean PX.Data.IBqlCreator.AppendExpression(SQLExpression& exp, PXGraph graph, BqlCommandInfo info, Selection selection);



public class [nested] disconnected:Constant<IBqlString,String,disconnected>, IBqlOperand, IImplement<IBqlString>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<disconnected>, IConstant<String>, IConstant, IBqlAggregatedOperand, IBqlCreator, IBqlVerifier
	String Value;
	void PX.Data.IBqlVerifier.Verify(PXCache cache, Object item, List<Object> pars, Nullable`1& result, Object& value);
	Boolean PX.Data.IBqlCreator.AppendExpression(SQLExpression& exp, PXGraph graph, BqlCommandInfo info, Selection selection);



public class [nested] error:Constant<IBqlString,String,error>, IBqlOperand, IImplement<IBqlString>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<error>, IConstant<String>, IConstant, IBqlAggregatedOperand, IBqlCreator, IBqlVerifier
	String Value;
	void PX.Data.IBqlVerifier.Verify(PXCache cache, Object item, List<Object> pars, Nullable`1& result, Object& value);
	Boolean PX.Data.IBqlCreator.AppendExpression(SQLExpression& exp, PXGraph graph, BqlCommandInfo info, Selection selection);



public class [nested] idle:Constant<IBqlString,String,idle>, IBqlOperand, IImplement<IBqlString>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<idle>, IConstant<String>, IConstant, IBqlAggregatedOperand, IBqlCreator, IBqlVerifier
	String Value;
	void PX.Data.IBqlVerifier.Verify(PXCache cache, Object item, List<Object> pars, Nullable`1& result, Object& value);
	Boolean PX.Data.IBqlCreator.AppendExpression(SQLExpression& exp, PXGraph graph, BqlCommandInfo info, Selection selection);



public class [nested] offline:Constant<IBqlString,String,offline>, IBqlOperand, IImplement<IBqlString>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<offline>, IConstant<String>, IConstant, IBqlAggregatedOperand, IBqlCreator, IBqlVerifier
	String Value;
	void PX.Data.IBqlVerifier.Verify(PXCache cache, Object item, List<Object> pars, Nullable`1& result, Object& value);
	Boolean PX.Data.IBqlCreator.AppendExpression(SQLExpression& exp, PXGraph graph, BqlCommandInfo info, Selection selection);



public class [nested] online:Constant<IBqlString,String,online>, IBqlOperand, IImplement<IBqlString>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<online>, IConstant<String>, IConstant, IBqlAggregatedOperand, IBqlCreator, IBqlVerifier
	String Value;
	void PX.Data.IBqlVerifier.Verify(PXCache cache, Object item, List<Object> pars, Nullable`1& result, Object& value);
	Boolean PX.Data.IBqlCreator.AppendExpression(SQLExpression& exp, PXGraph graph, BqlCommandInfo info, Selection selection);



public class [nested] unknown:Constant<IBqlString,String,unknown>, IBqlOperand, IImplement<IBqlString>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<unknown>, IConstant<String>, IConstant, IBqlAggregatedOperand, IBqlCreator, IBqlVerifier
	String Value;
	void PX.Data.IBqlVerifier.Verify(PXCache cache, Object item, List<Object> pars, Nullable`1& result, Object& value);
	Boolean PX.Data.IBqlCreator.AppendExpression(SQLExpression& exp, PXGraph graph, BqlCommandInfo info, Selection selection);



public class [nested] connected:Constant<IBqlString,String,connected>, IBqlOperand, IImplement<IBqlString>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<connected>, IConstant<String>, IConstant, IBqlAggregatedOperand, IBqlCreator, IBqlVerifier
	String Value;
	void PX.Data.IBqlVerifier.Verify(PXCache cache, Object item, List<Object> pars, Nullable`1& result, Object& value);
	Boolean PX.Data.IBqlCreator.AppendExpression(SQLExpression& exp, PXGraph graph, BqlCommandInfo info, Selection selection);



public class [nested] disconnected:Constant<IBqlString,String,disconnected>, IBqlOperand, IImplement<IBqlString>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<disconnected>, IConstant<String>, IConstant, IBqlAggregatedOperand, IBqlCreator, IBqlVerifier
	String Value;
	void PX.Data.IBqlVerifier.Verify(PXCache cache, Object item, List<Object> pars, Nullable`1& result, Object& value);
	Boolean PX.Data.IBqlCreator.AppendExpression(SQLExpression& exp, PXGraph graph, BqlCommandInfo info, Selection selection);



public class [nested] unknown:Constant<IBqlString,String,unknown>, IBqlOperand, IImplement<IBqlString>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<unknown>, IConstant<String>, IConstant, IBqlAggregatedOperand, IBqlCreator, IBqlVerifier
	String Value;
	void PX.Data.IBqlVerifier.Verify(PXCache cache, Object item, List<Object> pars, Nullable`1& result, Object& value);
	Boolean PX.Data.IBqlCreator.AppendExpression(SQLExpression& exp, PXGraph graph, BqlCommandInfo info, Selection selection);



public class [nested] new_:Constant<IBqlString,String,new_>, IBqlOperand, IImplement<IBqlString>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<new_>, IConstant<String>, IConstant, IBqlAggregatedOperand, IBqlCreator, IBqlVerifier
	String Value;
	void PX.Data.IBqlVerifier.Verify(PXCache cache, Object item, List<Object> pars, Nullable`1& result, Object& value);
	Boolean PX.Data.IBqlCreator.AppendExpression(SQLExpression& exp, PXGraph graph, BqlCommandInfo info, Selection selection);



public class [nested] sent_to_client:Constant<IBqlString,String,sent_to_client>, IBqlOperand, IImplement<IBqlString>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<sent_to_client>, IConstant<String>, IConstant, IBqlAggregatedOperand, IBqlCreator, IBqlVerifier
	String Value;
	void PX.Data.IBqlVerifier.Verify(PXCache cache, Object item, List<Object> pars, Nullable`1& result, Object& value);
	Boolean PX.Data.IBqlCreator.AppendExpression(SQLExpression& exp, PXGraph graph, BqlCommandInfo info, Selection selection);



public class [nested] queued:Constant<IBqlString,String,queued>, IBqlOperand, IImplement<IBqlString>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<queued>, IConstant<String>, IConstant, IBqlAggregatedOperand, IBqlCreator, IBqlVerifier
	String Value;
	void PX.Data.IBqlVerifier.Verify(PXCache cache, Object item, List<Object> pars, Nullable`1& result, Object& value);
	Boolean PX.Data.IBqlCreator.AppendExpression(SQLExpression& exp, PXGraph graph, BqlCommandInfo info, Selection selection);



public class [nested] in_progress:Constant<IBqlString,String,in_progress>, IBqlOperand, IImplement<IBqlString>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<in_progress>, IConstant<String>, IConstant, IBqlAggregatedOperand, IBqlCreator, IBqlVerifier
	String Value;
	void PX.Data.IBqlVerifier.Verify(PXCache cache, Object item, List<Object> pars, Nullable`1& result, Object& value);
	Boolean PX.Data.IBqlCreator.AppendExpression(SQLExpression& exp, PXGraph graph, BqlCommandInfo info, Selection selection);



public class [nested] error:Constant<IBqlString,String,error>, IBqlOperand, IImplement<IBqlString>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<error>, IConstant<String>, IConstant, IBqlAggregatedOperand, IBqlCreator, IBqlVerifier
	String Value;
	void PX.Data.IBqlVerifier.Verify(PXCache cache, Object item, List<Object> pars, Nullable`1& result, Object& value);
	Boolean PX.Data.IBqlCreator.AppendExpression(SQLExpression& exp, PXGraph graph, BqlCommandInfo info, Selection selection);



public class [nested] done:Constant<IBqlString,String,done>, IBqlOperand, IImplement<IBqlString>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<done>, IConstant<String>, IConstant, IBqlAggregatedOperand, IBqlCreator, IBqlVerifier
	String Value;
	void PX.Data.IBqlVerifier.Verify(PXCache cache, Object item, List<Object> pars, Nullable`1& result, Object& value);
	Boolean PX.Data.IBqlCreator.AppendExpression(SQLExpression& exp, PXGraph graph, BqlCommandInfo info, Selection selection);



public class [nested] expired:Constant<IBqlString,String,expired>, IBqlOperand, IImplement<IBqlString>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<expired>, IConstant<String>, IConstant, IBqlAggregatedOperand, IBqlCreator, IBqlVerifier
	String Value;
	void PX.Data.IBqlVerifier.Verify(PXCache cache, Object item, List<Object> pars, Nullable`1& result, Object& value);
	Boolean PX.Data.IBqlCreator.AppendExpression(SQLExpression& exp, PXGraph graph, BqlCommandInfo info, Selection selection);



public class [nested] unknown:Constant<IBqlString,String,unknown>, IBqlOperand, IImplement<IBqlString>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<unknown>, IConstant<String>, IConstant, IBqlAggregatedOperand, IBqlCreator, IBqlVerifier
	String Value;
	void PX.Data.IBqlVerifier.Verify(PXCache cache, Object item, List<Object> pars, Nullable`1& result, Object& value);
	Boolean PX.Data.IBqlCreator.AppendExpression(SQLExpression& exp, PXGraph graph, BqlCommandInfo info, Selection selection);