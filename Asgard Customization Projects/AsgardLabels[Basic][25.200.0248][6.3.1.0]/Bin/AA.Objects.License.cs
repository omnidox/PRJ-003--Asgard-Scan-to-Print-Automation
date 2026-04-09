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
	String FieldName;
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



public class ALFeatureCodeAttribute:ALAggregateAttribute, _Attribute, IPXInterfaceField, IPXCommandPreparingSubscriber, IPXRowSelectingSubscriber
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
	String FieldName;
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



public class ALGuidIDAttribute:ALAggregateAttribute, _Attribute, IPXInterfaceField, IPXCommandPreparingSubscriber, IPXRowSelectingSubscriber
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
	String FieldName;
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
	String FieldName;
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
	Int32 FieldOrdinal;
	Type CacheExtensionType;
	PXAttributeLevel AttributeLevel;
	Object TypeId;
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



public class ALLicenseProductIDForeignAttribute:ALIDForeignAttribute<productID,description,code>, _Attribute, IPXInterfaceField, IPXCommandPreparingSubscriber, IPXRowSelectingSubscriber, IPXRowSelectedSubscriber
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
	String FieldName;
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
	Int32 FieldOrdinal;
	Type CacheExtensionType;
	PXAttributeLevel AttributeLevel;
	Object TypeId;
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



public class ALNameAttribute:ALAggregateAttribute, _Attribute, IPXInterfaceField, IPXCommandPreparingSubscriber, IPXRowSelectingSubscriber, IPXFieldVerifyingSubscriber
	Int32 NAME_LENGTH;
	Int32 LONG_NAME_LENGTH;
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
	String FieldName;
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



public class ALLicense:PXBqlTable, IBqlTableSystemDataStorage, IBqlTable, INotable
	String TypeName;
	String ApiKey;
	String SharedKey;
	String LicenseManager;
	String DatabaseID;
	String DatabaseName;
	String FullServerName;
	String HostName;
	String InstanceID;
	String IPAddress;
	String InstallationID;
	String InstallationDate;
	String PrinterInfo;
	Nullable<Int32> CurrentCompany;
	Nullable<Guid> NoteID;
	Nullable<Guid> CreatedByID;
	String CreatedByScreenID;
	Nullable<DateTime> CreatedDateTime;
	Nullable<Guid> LastModifiedByID;
	String LastModifiedByScreenID;
	Nullable<DateTime> LastModifiedDateTime;
	Byte[] tstamp;
	PXBqlTableSystemData& PX.Data.IBqlTableSystemDataStorage.GetBqlTableSystemData();



public class ALLicenseProduct:PXBqlTable, IBqlTableSystemDataStorage, IBqlTable, INotable
	Nullable<Guid> ProductID;
	String Code;
	String LicenseID;
	String Description;
	String PublicKey;
	String LicenseData;
	String LicType;
	String Status;
	Nullable<DateTime> StartDate;
	Nullable<DateTime> EndDate;
	Nullable<DateTime> LastCheckDate;
	Nullable<Int32> DaysRemaining;
	String FirstName;
	String LastName;
	String Email;
	String Company;
	String Phone;
	String Metadata;
	String CustomFields;
	String Reference;
	String Address;
	Nullable<Guid> NoteID;
	Nullable<Guid> CreatedByID;
	String CreatedByScreenID;
	Nullable<DateTime> CreatedDateTime;
	Nullable<Guid> LastModifiedByID;
	String LastModifiedByScreenID;
	Nullable<DateTime> LastModifiedDateTime;
	Byte[] tstamp;
	PXBqlTableSystemData& PX.Data.IBqlTableSystemDataStorage.GetBqlTableSystemData();



public class ALLicenseProductFeature:PXBqlTable, IBqlTableSystemDataStorage, IBqlTable
	Nullable<Guid> ProductID;
	String Code;
	String Description;
	String FeatureType;
	Nullable<DateTime> ExpiryDate;
	Nullable<Boolean> IsExpired;
	Nullable<Boolean> AllowUnlimitedConsumptions;
	Nullable<Boolean> AllowOverages;
	Nullable<Int32> MaxOverages;
	Nullable<Int32> LocalConsumption;
	Nullable<Int32> TotalConsumption;
	Nullable<Int32> MaxConsumption;
	String ConsumptionPeriod;
	Nullable<Guid> NoteID;
	Nullable<Guid> CreatedByID;
	String CreatedByScreenID;
	Nullable<DateTime> CreatedDateTime;
	Nullable<Guid> LastModifiedByID;
	String LastModifiedByScreenID;
	Nullable<DateTime> LastModifiedDateTime;
	Byte[] tstamp;
	PXBqlTableSystemData& PX.Data.IBqlTableSystemDataStorage.GetBqlTableSystemData();



public class ALConstants
	String ProductCode;



public class ALMessages
	String Prefix;
	String ValueCannotBefound;
	String ObjectCannotBeFoundByCode;
	String ALLicenseNotFound;
	String ALLicenseProductNotFound;
	String CallHasWrongReturnType;
	String LicValueOutsideRange;
	String LicenseNotValid;
	String LicenseIDNotEntered;
	String LicenseNotActivated;
	String LicenseFeatureConsumptionValue;
	String LicenseFeatureNotConsumable;
	String NoLicenseFound;
	String LicenseRequiresApiKey;
	String LicenseRequiresSharedKey;
	String NoFeatureFoundFor;
	String CantExecuteWithoutLicenseManager;
	String CantConsumeFeature;
	String CantLoadFeaturesWithoutLicenseManager;
	String NoLicenseInstalled;
	String LicenseManagerIsConnected;
	String CannotConvertLicenseField;



public class ALInfo
	String LicenseManager;
	String HostName;
	String FullServerName;
	String DatabaseName;
	String DatabaseID;
	String InstanceID;
	String IPAddress;
	Int32 CurrentCompany;
	String InstallationDate;
	String InstallationID;
	Instance GetInstance();



public class ALLicenseMaint:PXGraph<ALLicenseMaint,ALLicense>
	PXSetup<ALLicense> License;
	PXSelect<ALLicenseProduct> LicenseProducts;
	PXSelect<ALLicenseProductFeature,Where`2> LicenseProductFeatures;
	PXSelectReadonly<ALLicenseProduct,Where`2> CurrentProduct;
	PXSelectJoin<ALLicenseProductFeature,LeftJoin`2,Where`2> FeaturesByProduct;
	PXAction<ALLicense> Activate;
	PXAction<ALLicense> LoadFeatures;
	PXAction<ALLicense> ReloadLicense;
	PXAction<ALLicense> TestConnection;
	PXSave<ALLicense> Save;
	PXCancel<ALLicense> Cancel;
	PXInsert<ALLicense> Insert;
	PXDelete<ALLicense> Delete;
	PXArchive<ALLicense> Archive;
	PXExtract<ALLicense> Extract;
	PXCopyPasteAction<ALLicense> CopyPaste;
	PXFirst<ALLicense> First;
	PXPrevious<ALLicense> Previous;
	PXNext<ALLicense> Next;
	PXLast<ALLicense> Last;
	Boolean IsReusableGraph;
	Boolean IsCreatedFromSession;
	Int32 ReuseCount;
	Boolean ShouldSaveVersionModified;
	Boolean IsInVersionModifiedState;
	DateTime CreateTime;
	Boolean IsImport;
	Boolean IsExport;
	Boolean IsContractBasedAPI;
	Boolean IsDacBasedOdataAPI;
	Boolean IsCopyPasteContext;
	Dictionary<Type,GetDefaultDelegate> Defaults;
	PXCacheCollection Caches;
	PXActionCollection Actions;
	PXViewCollection Views;
	Dictionary<PXView,String> ViewNames;
	PXTypedViewCollection TypedViews;
	String WorkflowID;
	String WorkflowStepID;
	IEnumerable<String> NavigationParams;
	Boolean UnattendedMode;
	Boolean IsSessionReadOnly;
	PXGraphPrototype Prototype;
	String StatePrefix;
	Boolean PreserveErrorInfo;
	PXErrorInfo[] NonUIErrors;
	Boolean IsMobile;
	Boolean IsImportFromExcel;
	Nullable<Boolean> IsArchiveContext;
	AccessInfo Accessinfo;
	Object UID;
	CultureInfo Culture;
	Byte[] TimeStamp;
	Boolean IsProcessing;
	IGraphLongOperationManager LongOperationManager;
	Boolean IsInitializing;
	String PrimaryView;
	Type PrimaryItemType;
	Boolean IsDirty;
	RowSelectingEvents RowSelecting;
	RowSelectedEvents RowSelected;
	RowInsertingEvents RowInserting;
	RowInsertedEvents RowInserted;
	RowUpdatingEvents RowUpdating;
	RowUpdatedEvents RowUpdated;
	RowDeletingEvents RowDeleting;
	RowDeletedEvents RowDeleted;
	RowPersistingEvents RowPersisting;
	RowPersistedEvents RowPersisted;
	CommandPreparingEvents CommandPreparing;
	FieldDefaultingEvents FieldDefaulting;
	FieldUpdatingEvents FieldUpdating;
	FieldVerifyingEvents FieldVerifying;
	FieldUpdatedEvents FieldUpdated;
	FieldSelectingEvents FieldSelecting;
	ExceptionHandlingEvents ExceptionHandling;
	ISqlDialect SqlDialect;
	List<SidePanelAction> SidePanelActions;
	IALLicenseManager GetLicenseManager(String productCode);
	void _(RowSelected<ALLicenseProduct> e);
	IEnumerable activate(PXAdapter adapter);
	IEnumerable loadFeatures(PXAdapter adapter);
	IEnumerable reloadLicense(PXAdapter adapter);
	IEnumerable testConnection(PXAdapter adapter);
	void _(FieldSelecting<ALLicenseProduct,licType> e);
	void _(FieldSelecting<ALLicenseProduct,status> e);
	void _(FieldSelecting<ALLicenseProduct,startDate> e);
	void _(FieldSelecting<ALLicenseProduct,endDate> e);
	void _(FieldSelecting<ALLicenseProduct,daysRemaining> e);
	void _(FieldSelecting<ALLicenseProduct,lastCheckDate> e);
	void _(FieldSelecting<ALLicenseProduct,firstName> e);
	void _(FieldSelecting<ALLicenseProduct,lastName> e);
	void _(FieldSelecting<ALLicenseProduct,email> e);
	void _(FieldSelecting<ALLicenseProduct,company> e);
	void _(FieldSelecting<ALLicenseProduct,phone> e);
	void _(FieldSelecting<ALLicenseProduct,metadata> e);
	void _(FieldSelecting<ALLicenseProduct,reference> e);
	void _(FieldSelecting<ALLicenseProduct,customFields> e);
	void _(FieldSelecting<ALLicenseProduct,address> e);
	void _(FieldSelecting<ALLicenseProductFeature,featureType> e);
	void _(FieldSelecting<ALLicenseProductFeature,description> e);
	void _(FieldSelecting<ALLicenseProductFeature,expiryDate> e);
	void _(FieldSelecting<ALLicenseProductFeature,isExpired> e);
	void _(FieldSelecting<ALLicenseProductFeature,localConsumption> e);
	void _(FieldSelecting<ALLicenseProductFeature,allowOverages> e);
	void _(FieldSelecting<ALLicenseProductFeature,allowUnlimitedConsumptions> e);
	void _(FieldSelecting<ALLicenseProductFeature,maxOverages> e);
	void _(FieldSelecting<ALLicenseProductFeature,totalConsumption> e);
	void _(FieldSelecting<ALLicenseProductFeature,maxConsumption> e);
	void _(FieldSelecting<ALLicenseProductFeature,consumptionPeriod> e);
	Boolean CanClipboardCopyPaste();
	void RaiseBeforeCommit();
	void RaiseAfterPersist();
	void RaiseClear(PXClearOption option);
	void RaiseReuseInitialize();
	void RaiseRequestCompleted();
	void Persist();
	Boolean PrePersist();
	void PerformPersist(IPersistPerformer persister);
	void PreCommit();
	void PostPersist();
	Int32 Persist(Type cacheType, PXDBOperation operation);
	Boolean AllowSelect(String viewName);
	Boolean AllowUpdate(String viewName);
	Boolean UpdateRights(String viewName);
	Boolean AllowInsert(String viewName);
	Boolean AllowDelete(String viewName);
	Boolean GetUpdatable(String viewName);
	String[] GetKeyNames(String viewName);
	Boolean HasException();
	Boolean HasGraphSpecificFields(Type itemType);
	String[] GetParameterNames(String viewName);
	void ValidateDataConsistency();
	void SetDataConsistencyIssue(String name, String diagnosticDetails, Boolean dumpAllCaches);
	void SetDataConsistencyIssue(String name, String text, String diagnosticDetails, Boolean dumpAllCaches);
	DacDescriptor GetDacDescriptor(IBqlTable dac, DacDescriptorCreationOptions customDescriptorCreationOptions);
	IEnumerable<PXDataRecord> ProviderSelect(BqlCommand command, Int32 topCount, PXDataValue[] pars);
	IEnumerable<PXDataRecord> ProviderSelect(BqlCommand command, Int32 topCount, PXView view, PXDataValue[] pars);
	PXDataRecord ProviderSelectSingle(PXDataField[] pars);
	IEnumerable<PXDataRecord> ProviderSelectMulti(PXDataField[] pars);
	Boolean ProviderInsert(PXDataFieldAssign[] pars);
	Boolean ProviderUpdate(PXDataFieldParam[] pars);
	Boolean ProviderDelete(PXDataFieldRestrict[] pars);
	Boolean ProviderArchive(Type table, PXDataFieldRestrict[] pars);
	Boolean ProviderExtract(Type table, PXDataFieldRestrict[] pars);
	PXDataRecord ProviderSelectSingle(Type table, PXDataField[] pars);
	IEnumerable<PXDataRecord> ProviderSelectMulti(Type table, PXDataField[] pars);
	Boolean ProviderInsert(Type table, PXDataFieldAssign[] pars);
	Boolean ProviderUpdate(Type table, PXDataFieldParam[] pars);
	Boolean ProviderDelete(Type table, PXDataFieldRestrict[] pars);
	Boolean ProviderEnsure(Type table, PXDataFieldAssign[] values, PXDataField[] pars);
	void RaisePXGraphViewChanged(String viewName);
	void RaiseBeforeUnloadEvent();
	void RaiseInitialized();
	void RaisePrepare();
	void CloneEntitiesFromParent(ExportTemplate template);
	PxDataSet ResetEntitiesToParent(ExportTemplate template);
	FileInfo GetCurrentEntityAsXml(ExportTemplate template);
	YaqlCondition GetFilterConditionForAdditionalRecordsForExport(Dictionary<String,Object> keys);
	Dictionary<String,PerTableReport> ImportEntitiesFromXml(Byte[] fileContent, RecordImportMode overwriteMode, DataUploader& dl);
	void ThrowWithoutRollback(Exception ex);
	String <ImportEntitiesFromXml>g__GetRowsStr|417_0(Dictionary<String,HashSet`1> Rows);
	IDisposable UnderDifferentStatePrefix(String newPrefix);
	AlteredDescriptor GetAlteredAttributes(Type itemType);
	Graph CreateInstance();
	Lazy<Graph> CreateLazyInstance();
	IQueryable<T> Select();
	IQueryable<T> SelectReadOnly();
	Graph CreateInstance(String prefix);
	void RestrictViewFields(String view, IEnumerable<Type> fields);
	void RestrictViewFields(String view, Type[] fields);
	void RestrictViewFields(String view, IEnumerable<String> fields, Boolean collectDependencies);
	PXGraph CreateInstance(Type graphType);
	Type _GetWrapperType(Type gt);
	PXGraph CreateInstance(Type graphType, String prefix);
	List<Type> _GetExtensions(Type tgraph);
	GraphStaticInfo InitGraphStaticInfo(Type tgraph, List<Type> extensions, Dictionary<String,Type> inactiveViews, Dictionary<String,String> inactiveActions);
	ValueTuple<MethodInfo,Type> GetHandler(Type methodDeclaringType, Type fieldDeclaringType, FieldInfo actionField, ILookup<Type,MethodInfo> methods);
	Type GetActionHandlerDelegateType(MethodInfo method);
	void CopyPasteGetScript(Boolean isImportSimple, List<Command> script, List<Container> containers);
	List<KeyValuePair`2> AdjustApiScript(List<KeyValuePair`2> fieldsByView);
	Int32 CopyPasteCommitChanges(String viewName, OrderedDictionary keys, OrderedDictionary vals);
	void InitScopedAccessInfoProperties();
	void SetOffline();
	void LoadQueryCache();
	void ClearSessionQueryCache();
	void UnloadQueryCache(IPXSessionState session);
	void SetValueExt(String viewName, Object data, String fieldName, Object value);
	void SetValue(String viewName, Object data, String fieldName, Object value);
	Object GetValueExt(String viewName, Object data, String fieldName);
	Object GetStateExt(String viewName, Object data, String fieldName);
	Object GetValue(String viewName, Object data, String fieldName);
	String[] GetFieldNames(String viewName);
	PXEventSubscriberAttribute[] GetAttributes(String viewName, String name);
	PXEntryStatus GetStatus(String viewName);
	Type GetItemType(String viewName);
	TNode GetDefault();
	PXGraphExtension GetExtension(Int32 at);
	Extension GetExtension();
	void InitCacheMapping(Dictionary<Type,Type> map);
	IEnumerable<Extension> FindAllImplementations();
	Extension FindImplementation();
	IEnumerable<String> GetViewNames();
	KeyValuePair`2[] GetSortColumnsWithUpdateSelect(String viewName);
	KeyValuePair`2[] GetSortColumnsWithUpdateSelectWithoutExtSort(String viewName);
	KeyValuePair`2[] GetSortColumns(String viewName);
	KeyValuePair`2[] GetDefaultSortColumns(String viewName);
	void Configure(PXScreenConfiguration graph);
	PXCacheCollection CreateCacheCollection();
	PXViewCollection CreateViewCollection();
	void SelectTimeStamp();
	IEnumerable ExecuteSelect(String viewName, Object[] parameters, Object[] searches, String[] sortcolumns, Boolean[] descendings, PXFilterRow[] filters, Int32& startRow, Int32 maximumRows, Int32& totalRows);
	IEnumerable ExecuteSelect(String viewName, Object[] currents, Object[] parameters, Object[] searches, String[] sortcolumns, Boolean[] descendings, PXFilterRow[] filters, Int32& startRow, Int32 maximumRows, Int32& totalRows);
	IEnumerable ExecuteSelect(String viewName, Object[] currents, Object[] parameters, Object[] searches, String[] sortcolumns, Boolean[] descendings, PXFilterRow[] filters, Int32& startRow, Int32 maximumRows, Int32& totalRows, Boolean skipHints);
	void _RecordCachedSlot(Type entity, Object slot, Func<Object> getter);
	Boolean _HasChangedSlots();
	Int32 ExecuteUpdate(String viewName, IDictionary keys, IDictionary values, Object[] parameters);
	void _ProcessTail(String viewName, IDictionary values, Object[] parameters);
	void _UpdatePrimaryView(String viewName);
	Int32 ExecuteDelete(String viewName, IDictionary keys, IDictionary values, Object[] parameters);
	Int32 ExecuteInsert(String viewName, IDictionary values, Object[] parameters);
	void Unload();
	void EnsureIfArchived(PXView view);
	void EnsureIfArchived(String viewName);
	void EnsureIfArchived();
	PXCache _GetReadonlyCache(Type key);
	void Load();
	void Clear();
	void Clear(PXClearOption option, String str);
	void Clear(PXClearOption option);
	void RaiseBeforePersist();



public class LicenseHelper
	IALLicenseManager LicenseManager;
	void RecurringTask(Action action, Int32 seconds, CancellationToken token);



public class AbstractLicenseManager:IALLicenseManager
	String ProductCode;
	IALLicense ActivateLicense(ALLicenseProduct product);
	IALLicense ReloadLicense(ALLicenseProduct product);
	IALLicenseFeature GetFeature(String feature);
	IALLicense GetLicense(Boolean throwIfNull);
	void LoadFeatures();
	void Check();
	Boolean IsConnected(Boolean throwExceptions);
	Boolean TryGetFeature(String feature, IALLicenseFeature& licenseFeature);
	void UpdateFeatureConsumption(String feature, Int32 count);
	void CheckFeatureConsumption(String feature, Int32 count);
	T GetValue(String licField);
	Boolean HasFeature(String feature);
	IALLicenseFeature GetFeature(Type feature);
	Boolean TryGetFeature(Type feature, IALLicenseFeature& licenseFeature);
	Boolean HasFeature(Type feature);
	void UpdateFeatureConsumption(Type feature, Int32 count);
	void CheckFeatureConsumption(Type feature, Int32 count);
	void CheckLimit(String licenseField, T value, Validator<T> validator);



public class DBFeature
	String Code;
	String Name;



public class IALCustomField
	String Name;
	Object Value;



public class IALLicense
	String ProductCode;
	Boolean Valid;
	String LicenseType;
	String Status;
	Nullable<DateTime> StartDate;
	Nullable<DateTime> EndDate;
	Nullable<DateTime> LastCheckDate;
	Nullable<Int32> DaysRemaining;
	IALLicenseOwner Owner;
	IALCustomField[] CustomFields;



public class IALLicenseFeature
	Nullable<Boolean> AllowOverages;
	String Metadata;
	String ConsumptionPeriod;
	Nullable<Boolean> ResetConsumption;
	Nullable<Boolean> AllowUnlimitedConsumptions;
	Nullable<DateTime> ExpiryDate;
	Nullable<Int32> LocalConsumption;
	Nullable<Int32> TotalConsumption;
	Nullable<Int32> MaxConsumption;
	String FeatureType;
	String Name;
	String Code;
	Nullable<Int32> MaxOverages;
	Nullable<Boolean> IsExpired;



public class IALLicenseManager
	String ProductCode;
	IALLicense ActivateLicense(ALLicenseProduct product);
	IALLicense ReloadLicense(ALLicenseProduct product);
	IALLicense GetLicense(Boolean throwIfNull);
	void Check();
	IALLicenseFeature GetFeature(Type feature);
	IALLicenseFeature GetFeature(String feature);
	Boolean TryGetFeature(Type feature, IALLicenseFeature& licenseFeature);
	Boolean TryGetFeature(String feature, IALLicenseFeature& licenseFeature);
	Boolean HasFeature(Type feature);
	Boolean HasFeature(String feature);
	void UpdateFeatureConsumption(Type feature, Int32 count);
	void UpdateFeatureConsumption(String feature, Int32 count);
	void CheckFeatureConsumption(Type feature, Int32 count);
	void CheckFeatureConsumption(String feature, Int32 count);
	void CheckLimit(String licenseField, T value, Validator<T> validator);
	void LoadFeatures();
	Boolean IsConnected(Boolean throwExceptions);
	T GetValue(String licenseField);



public class IALLicenseManagerFactory
	IALLicenseManager GetLicenseManager(String productCode);



public class IALLicenseOwner
	String FirstName;
	String LastName;
	String Email;
	String Company;
	String Address;
	String Phone;
	String Status;
	String Metadata;
	String Reference;



public class LicenseManagerFactoryRegistration:Module, IModule
	void Load(ContainerBuilder builder);
	void Configure(IComponentRegistryBuilder componentRegistry);
	void AttachToComponentRegistration(IComponentRegistryBuilder componentRegistry, IComponentRegistration registration);
	void AttachToRegistrationSource(IComponentRegistryBuilder componentRegistry, IRegistrationSource registrationSource);



public class ALLicenseProductSlot:IPrefetchable, IPXCompanyDependent
	IEnumerable<PXDataRecord> GetData();
	void Prefetch();
	Boolean TryGetLicenseProduct(String productCode, LicenseProduct& value);
	LicenseProduct GetLicenseProduct(String productCode);
	void Reset();



public class ALLicenseSlot:IPrefetchable, IPXCompanyDependent
	String ApiKey;
	String SharedKey;
	void Prefetch();



public class Validators
	Boolean IsGreaterThanOrEqualsTo(IALLicenseManager manager, String licenseField, T value);
	Int32 CompareTo(IALLicenseManager manager, String licenseField, T value, IComparer<T> comparer);
	Int32 CompareTo(IALLicenseManager manager, String licenseField, T value);
	Boolean IsEqual(IALLicenseManager manager, String licenseField, T value);
	Boolean IsBetween(IALLicenseManager manager, String licenseField, T from, T to);
	Boolean IsGreaterThan(IALLicenseManager manager, String licenseField, T value);
	Boolean IsLessThan(IALLicenseManager manager, String licenseField, T value);
	Boolean IsLessThanOrEqualsTo(IALLicenseManager manager, String licenseField, T value);
	Boolean IsNotNull(IALLicenseManager manager, String licenseField);
	Boolean IsNull(IALLicenseManager manager, String licenseField);
	Boolean IsContainedWithinList(IALLicenseManager manager, String licenseField, T value);
	Boolean StartsWith(IALLicenseManager manager, String licenseField, String value);
	Boolean EndsWith(IALLicenseManager manager, String licenseField, String value);
	Boolean Contains(IALLicenseManager manager, String licenseField, String value);



public class [nested] FK



public class [nested] typeName:Field<IBqlString,String,typeName>, IBqlOperand, IImplement<IBqlString>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<typeName>, IBqlField



public class [nested] apiKey:Field<IBqlString,String,apiKey>, IBqlOperand, IImplement<IBqlString>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<apiKey>, IBqlField



public class [nested] sharedKey:Field<IBqlString,String,sharedKey>, IBqlOperand, IImplement<IBqlString>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<sharedKey>, IBqlField



public class [nested] licenseManager:Field<IBqlString,String,licenseManager>, IBqlOperand, IImplement<IBqlString>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<licenseManager>, IBqlField



public class [nested] databaseID:Field<IBqlString,String,databaseID>, IBqlOperand, IImplement<IBqlString>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<databaseID>, IBqlField



public class [nested] databaseName:Field<IBqlString,String,databaseName>, IBqlOperand, IImplement<IBqlString>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<databaseName>, IBqlField



public class [nested] fullServerName:Field<IBqlString,String,fullServerName>, IBqlOperand, IImplement<IBqlString>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<fullServerName>, IBqlField



public class [nested] hostName:Field<IBqlString,String,hostName>, IBqlOperand, IImplement<IBqlString>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<hostName>, IBqlField



public class [nested] instanceID:Field<IBqlString,String,instanceID>, IBqlOperand, IImplement<IBqlString>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<instanceID>, IBqlField



public class [nested] iPAddress:Field<IBqlString,String,iPAddress>, IBqlOperand, IImplement<IBqlString>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<iPAddress>, IBqlField



public class [nested] installationID:Field<IBqlString,String,installationID>, IBqlOperand, IImplement<IBqlString>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<installationID>, IBqlField



public class [nested] installationDate:Field<IBqlString,String,installationDate>, IBqlOperand, IImplement<IBqlString>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<installationDate>, IBqlField



public class [nested] printerInfo:Field<IBqlString,String,printerInfo>, IBqlOperand, IImplement<IBqlString>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<printerInfo>, IBqlField



public class [nested] currentCompany:Field<IBqlInt,Int32,currentCompany>, IBqlOperand, IImplement<IBqlInt>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<currentCompany>, IBqlField



public class [nested] noteID:Field<IBqlGuid,Guid,noteID>, IBqlOperand, IImplement<IBqlGuid>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<noteID>, IBqlField



public class [nested] createdByID:Field<IBqlGuid,Guid,createdByID>, IBqlOperand, IImplement<IBqlGuid>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<createdByID>, IBqlField



public class [nested] createdByScreenID:Field<IBqlString,String,createdByScreenID>, IBqlOperand, IImplement<IBqlString>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<createdByScreenID>, IBqlField



public class [nested] createdDateTime:Field<IBqlDateTime,DateTime,createdDateTime>, IBqlOperand, IImplement<IBqlDateTime>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<createdDateTime>, IBqlField



public class [nested] lastModifiedByID:Field<IBqlGuid,Guid,lastModifiedByID>, IBqlOperand, IImplement<IBqlGuid>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<lastModifiedByID>, IBqlField



public class [nested] lastModifiedByScreenID:Field<IBqlString,String,lastModifiedByScreenID>, IBqlOperand, IImplement<IBqlString>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<lastModifiedByScreenID>, IBqlField



public class [nested] lastModifiedDateTime:Field<IBqlDateTime,DateTime,lastModifiedDateTime>, IBqlOperand, IImplement<IBqlDateTime>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<lastModifiedDateTime>, IBqlField



public class [nested] Tstamp:Field<IBqlByteArray,Byte[],Tstamp>, IBqlOperand, IImplement<IBqlByteArray>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<Tstamp>, IBqlField



public class [nested] PK:By<ALLicenseProduct,productID>, IFilledWith<IBqlField,productID>, ITypeArrayOf<IBqlField>, IsNotEmpty, IPrimaryKeyOf<ALLicenseProduct>, IPrimaryKey
	ALLicenseProduct Find(PXGraph graph, Nullable<Guid> productID);
	ALLicenseProduct Find(PXGraph graph, ALLicenseProduct item, PKFindOptions options);
	ALLicenseProduct FindBy(PXGraph graph, Object key, PKFindOptions options);
	ALLicenseProduct FindBy(PXGraph graph, Object key, Boolean isDirtyKey);
	void StoreResult(PXGraph graph, ALLicenseProduct item, Boolean forDirtySelect);
	IBqlTable PX.Data.ReferentialIntegrity.Attributes.IPrimaryKey.Find(PXGraph graph, IBqlTable item, PKFindOptions options);
	ALLicenseProduct PX.Data.ReferentialIntegrity.Attributes.IPrimaryKeyOf<TTable>.Find(PXGraph graph, ALLicenseProduct item, PKFindOptions options);
	IBqlTable PX.Data.ReferentialIntegrity.Attributes.IPrimaryKey.Find(PXGraph graph, PKFindOptions options, Object[] keys);
	ALLicenseProduct PX.Data.ReferentialIntegrity.Attributes.IPrimaryKeyOf<TTable>.Find(PXGraph graph, PKFindOptions options, Object[] keys);
	void PX.Data.ReferentialIntegrity.Attributes.IPrimaryKey.StoreResult(PXGraph graph, IBqlTable item, Boolean forDirtySelect);
	void PX.Data.ReferentialIntegrity.Attributes.IPrimaryKeyOf<TTable>.StoreResult(PXGraph graph, ALLicenseProduct item, Boolean forDirtySelect);



public class [nested] UK:By<ALLicenseProduct,code>, IFilledWith<IBqlField,code>, ITypeArrayOf<IBqlField>, IsNotEmpty, IPrimaryKeyOf<ALLicenseProduct>, IPrimaryKey
	ALLicenseProduct Find(PXGraph graph, String productCode);
	ALLicenseProduct Find(PXGraph graph, ALLicenseProduct item, PKFindOptions options);
	ALLicenseProduct FindBy(PXGraph graph, Object key, PKFindOptions options);
	ALLicenseProduct FindBy(PXGraph graph, Object key, Boolean isDirtyKey);
	void StoreResult(PXGraph graph, ALLicenseProduct item, Boolean forDirtySelect);
	IBqlTable PX.Data.ReferentialIntegrity.Attributes.IPrimaryKey.Find(PXGraph graph, IBqlTable item, PKFindOptions options);
	ALLicenseProduct PX.Data.ReferentialIntegrity.Attributes.IPrimaryKeyOf<TTable>.Find(PXGraph graph, ALLicenseProduct item, PKFindOptions options);
	IBqlTable PX.Data.ReferentialIntegrity.Attributes.IPrimaryKey.Find(PXGraph graph, PKFindOptions options, Object[] keys);
	ALLicenseProduct PX.Data.ReferentialIntegrity.Attributes.IPrimaryKeyOf<TTable>.Find(PXGraph graph, PKFindOptions options, Object[] keys);
	void PX.Data.ReferentialIntegrity.Attributes.IPrimaryKey.StoreResult(PXGraph graph, IBqlTable item, Boolean forDirtySelect);
	void PX.Data.ReferentialIntegrity.Attributes.IPrimaryKeyOf<TTable>.StoreResult(PXGraph graph, ALLicenseProduct item, Boolean forDirtySelect);



public class [nested] FK



public class [nested] productID:Field<IBqlGuid,Guid,productID>, IBqlOperand, IImplement<IBqlGuid>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<productID>, IBqlField



public class [nested] code:Field<IBqlString,String,code>, IBqlOperand, IImplement<IBqlString>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<code>, IBqlField



public class [nested] licenseID:Field<IBqlString,String,licenseID>, IBqlOperand, IImplement<IBqlString>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<licenseID>, IBqlField



public class [nested] description:Field<IBqlString,String,description>, IBqlOperand, IImplement<IBqlString>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<description>, IBqlField



public class [nested] publicKey:Field<IBqlString,String,publicKey>, IBqlOperand, IImplement<IBqlString>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<publicKey>, IBqlField



public class [nested] licenseData:Field<IBqlString,String,licenseData>, IBqlOperand, IImplement<IBqlString>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<licenseData>, IBqlField



public class [nested] licType:Field<IBqlString,String,licType>, IBqlOperand, IImplement<IBqlString>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<licType>, IBqlField



public class [nested] status:Field<IBqlString,String,status>, IBqlOperand, IImplement<IBqlString>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<status>, IBqlField



public class [nested] startDate:Field<IBqlDateTime,DateTime,startDate>, IBqlOperand, IImplement<IBqlDateTime>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<startDate>, IBqlField



public class [nested] endDate:Field<IBqlDateTime,DateTime,endDate>, IBqlOperand, IImplement<IBqlDateTime>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<endDate>, IBqlField



public class [nested] lastCheckDate:Field<IBqlDateTime,DateTime,lastCheckDate>, IBqlOperand, IImplement<IBqlDateTime>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<lastCheckDate>, IBqlField



public class [nested] daysRemaining:Field<IBqlInt,Int32,daysRemaining>, IBqlOperand, IImplement<IBqlInt>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<daysRemaining>, IBqlField



public class [nested] firstName:Field<IBqlString,String,firstName>, IBqlOperand, IImplement<IBqlString>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<firstName>, IBqlField



public class [nested] lastName:Field<IBqlString,String,lastName>, IBqlOperand, IImplement<IBqlString>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<lastName>, IBqlField



public class [nested] email:Field<IBqlString,String,email>, IBqlOperand, IImplement<IBqlString>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<email>, IBqlField



public class [nested] company:Field<IBqlString,String,company>, IBqlOperand, IImplement<IBqlString>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<company>, IBqlField



public class [nested] phone:Field<IBqlString,String,phone>, IBqlOperand, IImplement<IBqlString>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<phone>, IBqlField



public class [nested] metadata:Field<IBqlString,String,metadata>, IBqlOperand, IImplement<IBqlString>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<metadata>, IBqlField



public class [nested] customFields:Field<IBqlString,String,customFields>, IBqlOperand, IImplement<IBqlString>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<customFields>, IBqlField



public class [nested] reference:Field<IBqlString,String,reference>, IBqlOperand, IImplement<IBqlString>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<reference>, IBqlField



public class [nested] address:Field<IBqlString,String,address>, IBqlOperand, IImplement<IBqlString>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<address>, IBqlField



public class [nested] noteID:Field<IBqlGuid,Guid,noteID>, IBqlOperand, IImplement<IBqlGuid>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<noteID>, IBqlField



public class [nested] createdByID:Field<IBqlGuid,Guid,createdByID>, IBqlOperand, IImplement<IBqlGuid>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<createdByID>, IBqlField



public class [nested] createdByScreenID:Field<IBqlString,String,createdByScreenID>, IBqlOperand, IImplement<IBqlString>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<createdByScreenID>, IBqlField



public class [nested] createdDateTime:Field<IBqlDateTime,DateTime,createdDateTime>, IBqlOperand, IImplement<IBqlDateTime>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<createdDateTime>, IBqlField



public class [nested] lastModifiedByID:Field<IBqlGuid,Guid,lastModifiedByID>, IBqlOperand, IImplement<IBqlGuid>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<lastModifiedByID>, IBqlField



public class [nested] lastModifiedByScreenID:Field<IBqlString,String,lastModifiedByScreenID>, IBqlOperand, IImplement<IBqlString>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<lastModifiedByScreenID>, IBqlField



public class [nested] lastModifiedDateTime:Field<IBqlDateTime,DateTime,lastModifiedDateTime>, IBqlOperand, IImplement<IBqlDateTime>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<lastModifiedDateTime>, IBqlField



public class [nested] Tstamp:Field<IBqlByteArray,Byte[],Tstamp>, IBqlOperand, IImplement<IBqlByteArray>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<Tstamp>, IBqlField



public class [nested] PK:By<ALLicenseProductFeature,productID,code>, IArrayConcat<IBqlField,Empty,IFilledWith`2>, IArrayConcat<IBqlField>, IArrayConcat, ITypeArrayOf<IBqlField>, IPrimaryKeyOf<ALLicenseProductFeature>, IPrimaryKey
	ALLicenseProductFeature Find(PXGraph graph, Nullable<Guid> productID, String code);
	ALLicenseProductFeature FindBy(PXGraph graph, Object keyComponent1, Object keyComponent2, PKFindOptions options);
	ALLicenseProductFeature Find(PXGraph graph, ALLicenseProductFeature item, PKFindOptions options);
	ALLicenseProductFeature FindImpl(PXGraph graph, PKFindOptions options, Object[] keys);
	void StoreResult(PXGraph graph, ALLicenseProductFeature item, Boolean forDirtySelect);
	ALLicenseProductFeature SelectEntity(PXGraph graph, Object[] keys, Boolean isReadOnly);
	IBqlTable PX.Data.ReferentialIntegrity.Attributes.IPrimaryKey.Find(PXGraph graph, IBqlTable item, PKFindOptions options);
	ALLicenseProductFeature PX.Data.ReferentialIntegrity.Attributes.IPrimaryKeyOf<TTable>.Find(PXGraph graph, ALLicenseProductFeature item, PKFindOptions options);
	IBqlTable PX.Data.ReferentialIntegrity.Attributes.IPrimaryKey.Find(PXGraph graph, PKFindOptions options, Object[] keys);
	ALLicenseProductFeature PX.Data.ReferentialIntegrity.Attributes.IPrimaryKeyOf<TTable>.Find(PXGraph graph, PKFindOptions options, Object[] keys);
	void PX.Data.ReferentialIntegrity.Attributes.IPrimaryKey.StoreResult(PXGraph graph, IBqlTable item, Boolean forDirtySelect);
	void PX.Data.ReferentialIntegrity.Attributes.IPrimaryKeyOf<TTable>.StoreResult(PXGraph graph, ALLicenseProductFeature item, Boolean forDirtySelect);



public class [nested] FK



public class [nested] productID:Field<IBqlGuid,Guid,productID>, IBqlOperand, IImplement<IBqlGuid>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<productID>, IBqlField



public class [nested] code:Field<IBqlString,String,code>, IBqlOperand, IImplement<IBqlString>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<code>, IBqlField



public class [nested] description:Field<IBqlString,String,description>, IBqlOperand, IImplement<IBqlString>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<description>, IBqlField



public class [nested] featureType:Field<IBqlString,String,featureType>, IBqlOperand, IImplement<IBqlString>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<featureType>, IBqlField



public class [nested] expiryDate:Field<IBqlDateTime,DateTime,expiryDate>, IBqlOperand, IImplement<IBqlDateTime>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<expiryDate>, IBqlField



public class [nested] isExpired:Field<IBqlBool,Boolean,isExpired>, IBqlOperand, IImplement<IBqlBool>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<isExpired>, IBqlField



public class [nested] allowUnlimitedConsumptions:Field<IBqlBool,Boolean,allowUnlimitedConsumptions>, IBqlOperand, IImplement<IBqlBool>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<allowUnlimitedConsumptions>, IBqlField



public class [nested] allowOverages:Field<IBqlBool,Boolean,allowOverages>, IBqlOperand, IImplement<IBqlBool>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<allowOverages>, IBqlField



public class [nested] maxOverages:Field<IBqlInt,Int32,maxOverages>, IBqlOperand, IImplement<IBqlInt>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<maxOverages>, IBqlField



public class [nested] localConsumption:Field<IBqlInt,Int32,localConsumption>, IBqlOperand, IImplement<IBqlInt>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<localConsumption>, IBqlField



public class [nested] totalConsumption:Field<IBqlInt,Int32,totalConsumption>, IBqlOperand, IImplement<IBqlInt>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<totalConsumption>, IBqlField



public class [nested] maxConsumption:Field<IBqlInt,Int32,maxConsumption>, IBqlOperand, IImplement<IBqlInt>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<maxConsumption>, IBqlField



public class [nested] consumptionPeriod:Field<IBqlString,String,consumptionPeriod>, IBqlOperand, IImplement<IBqlString>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<consumptionPeriod>, IBqlField



public class [nested] noteID:Field<IBqlGuid,Guid,noteID>, IBqlOperand, IImplement<IBqlGuid>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<noteID>, IBqlField



public class [nested] createdByID:Field<IBqlGuid,Guid,createdByID>, IBqlOperand, IImplement<IBqlGuid>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<createdByID>, IBqlField



public class [nested] createdByScreenID:Field<IBqlString,String,createdByScreenID>, IBqlOperand, IImplement<IBqlString>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<createdByScreenID>, IBqlField



public class [nested] createdDateTime:Field<IBqlDateTime,DateTime,createdDateTime>, IBqlOperand, IImplement<IBqlDateTime>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<createdDateTime>, IBqlField



public class [nested] lastModifiedByID:Field<IBqlGuid,Guid,lastModifiedByID>, IBqlOperand, IImplement<IBqlGuid>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<lastModifiedByID>, IBqlField



public class [nested] lastModifiedByScreenID:Field<IBqlString,String,lastModifiedByScreenID>, IBqlOperand, IImplement<IBqlString>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<lastModifiedByScreenID>, IBqlField



public class [nested] lastModifiedDateTime:Field<IBqlDateTime,DateTime,lastModifiedDateTime>, IBqlOperand, IImplement<IBqlDateTime>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<lastModifiedDateTime>, IBqlField



public class [nested] Tstamp:Field<IBqlByteArray,Byte[],Tstamp>, IBqlOperand, IImplement<IBqlByteArray>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<Tstamp>, IBqlField



public class [nested] ALViewName



public class [nested] ALDACName
	String ALLicense;
	String ALLicenseProduct;
	String ALLicenseProductFeature;



public class [nested] ALDropDown



public class [nested] InfoType:Enum, IComparable, IFormattable, IConvertible
	Int32 value__;
	InfoType LicenseManager;
	InfoType DatabaseID;
	InfoType DatabaseName;
	InfoType FullServerName;
	InfoType HostName;
	InfoType InstanceID;
	InfoType IPAddress;
	InfoType InstallationID;
	InfoType InstallationDate;
	InfoType PrinterInfo;
	InfoType CurrentCompany;
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



public class [nested] BqlInfo:BqlType<IBqlNull,InfoType>
	Boolean MatchType(Type operandType);



public class [nested] InfoTypes



public class [nested] Field:BqlFormulaEvaluator<IType>, IBqlCreator, IBqlVerifier, IBqlOperand
	Object Evaluate(PXCache cache, Object item, Dictionary<Type,Object> pars);
	Boolean AppendExpression(SQLExpression& exp, PXGraph graph, BqlCommandInfo info, Selection selection);
	void Verify(PXCache cache, Object item, List<Object> pars, Nullable`1& result, Object& value);
	Object Calculate(PXCache cache, Object item);
	void Verify(PXCache cache, Object item, IBqlCreator formula, Nullable`1& result, Object& value);
	Boolean IsContextualFormula(PXCache cache, Type Condition);
	Boolean GetOperandExpression(SQLExpression& exp, IBqlCreator& operand, PXGraph graph, BqlCommandInfo info, Selection selection);



public class [nested] Features



public class [nested] LicenseProduct
	Nullable<Guid> ProductID;
	String Code;
	String Description;
	String LicenseID;



public class [nested] Validator:MulticastDelegate, ICloneable, ISerializable
	MethodInfo Method;
	Object Target;
	Boolean Invoke(IALLicenseManager manager, String licenseField, T value);
	IAsyncResult BeginInvoke(IALLicenseManager manager, String licenseField, T value, AsyncCallback callback, Object object);
	Boolean EndInvoke(IAsyncResult result);
	Boolean IsUnmanagedFunctionPtr();
	Boolean InvocationListLogicallyNull();
	void GetObjectData(SerializationInfo info, StreamingContext context);
	Boolean Equals(Object obj);
	MulticastDelegate NewMulticastDelegate(Object[] invocationList, Int32 invocationCount);
	void StoreDynamicMethod(MethodInfo dynamicMethod);
	Delegate CombineImpl(Delegate follow);
	Delegate RemoveImpl(Delegate value);
	Delegate[] GetInvocationList();
	Int32 GetHashCode();
	Object GetTarget();
	MethodInfo GetMethodImpl();
	Object DynamicInvoke(Object[] args);
	Object DynamicInvokeImpl(Object[] args);
	Delegate Combine(Delegate a, Delegate b);
	Delegate Combine(Delegate[] delegates);
	Delegate Remove(Delegate source, Delegate value);
	Delegate RemoveAll(Delegate source, Delegate value);
	Object Clone();
	Delegate CreateDelegate(Type type, Object target, String method);
	Delegate CreateDelegate(Type type, Object target, String method, Boolean ignoreCase);
	Delegate CreateDelegate(Type type, Object target, String method, Boolean ignoreCase, Boolean throwOnBindFailure);
	Delegate CreateDelegate(Type type, Type target, String method);
	Delegate CreateDelegate(Type type, Type target, String method, Boolean ignoreCase);
	Delegate CreateDelegate(Type type, Type target, String method, Boolean ignoreCase, Boolean throwOnBindFailure);
	Delegate CreateDelegate(Type type, MethodInfo method, Boolean throwOnBindFailure);
	Delegate CreateDelegate(Type type, Object firstArgument, MethodInfo method, Boolean throwOnBindFailure);
	Delegate CreateDelegateNoSecurityCheck(Type type, Object target, RuntimeMethodHandle method);
	Delegate CreateDelegateNoSecurityCheck(RuntimeType type, Object firstArgument, MethodInfo method);
	Delegate CreateDelegate(Type type, MethodInfo method);
	Delegate UnsafeCreateDelegate(RuntimeType rtType, RuntimeMethodInfo rtMethod, Object firstArgument, DelegateBindingFlags flags);
	IntPtr GetCallStub(IntPtr methodPtr);
	Boolean CompareUnmanagedFunctionPtrs(Delegate d1, Delegate d2);
	Delegate CreateDelegate(Type type, Object firstArgument, MethodInfo method);
	Delegate CreateDelegateInternal(RuntimeType rtType, RuntimeMethodInfo rtMethod, Object firstArgument, DelegateBindingFlags flags, StackCrawlMark& stackMark);
	MulticastDelegate InternalAllocLike(Delegate d);
	Boolean InternalEqualTypes(Object a, Object b);
	IntPtr GetMulticastInvoke();
	IntPtr GetInvokeMethod();
	IRuntimeMethodInfo FindMethodHandle();
	Boolean InternalEqualMethodHandles(Delegate left, Delegate right);
	IntPtr AdjustTarget(Object target, IntPtr methodPtr);



public class [nested] Product:By<ALLicenseProduct,productID,ALLicenseProductFeature,productID>, IBqlUnary, IBqlCreator, IBqlVerifier, IBqlCustomPredicate, IForeignKeyBetween<ALLicenseProductFeature,ALLicenseProduct>, IForeignKey, IForeignKeyFrom<ALLicenseProductFeature>, IForeignKeyTo<ALLicenseProduct>, IFilledWith<IFieldsRelation,IsRelatedTo`1>, ITypeArrayOf<IFieldsRelation>, IsNotEmpty
	ReadOnlyDictionary<Type,Type> FieldsMapping;
	Type ParentTable;
	Type ChildTable;
	ALLicenseProduct FindParent(PXGraph graph, ALLicenseProductFeature child, PKFindOptions options);
	void CollectReference();
	ALLicenseProduct FindParent(PXGraph graph, ALLicenseProductFeature child, PKFindOptions options);
	IEnumerable<ALLicenseProductFeature> SelectChildren(PXGraph graph, ALLicenseProduct parent);
	Boolean Match(PXGraph graph, ALLicenseProduct parent, ALLicenseProductFeature child);
	IBqlTable PX.Data.ReferentialIntegrity.Attributes.IForeignKey.FindParent(PXGraph graph, IBqlTable child, PKFindOptions options);
	IEnumerable<IBqlTable> PX.Data.ReferentialIntegrity.Attributes.IForeignKey.SelectChildren(PXGraph graph, IBqlTable parent);
	IBqlTable PX.Data.ReferentialIntegrity.Attributes.IForeignKeyFrom<TChildTable>.FindParent(PXGraph graph, ALLicenseProductFeature child, PKFindOptions options);
	IEnumerable<ALLicenseProductFeature> PX.Data.ReferentialIntegrity.Attributes.IForeignKeyFrom<TChildTable>.SelectChildren(PXGraph graph, IBqlTable parent);
	ALLicenseProduct PX.Data.ReferentialIntegrity.Attributes.IForeignKeyTo<TParentTable>.FindParent(PXGraph graph, IBqlTable child, PKFindOptions options);
	IEnumerable<IBqlTable> PX.Data.ReferentialIntegrity.Attributes.IForeignKeyTo<TParentTable>.SelectChildren(PXGraph graph, ALLicenseProduct parent);
	ALLicenseProduct PX.Data.ReferentialIntegrity.Attributes.IForeignKeyBetween<TChildTable,TParentTable>.FindParent(PXGraph graph, ALLicenseProductFeature child, PKFindOptions options);
	IEnumerable<ALLicenseProductFeature> PX.Data.ReferentialIntegrity.Attributes.IForeignKeyBetween<TChildTable,TParentTable>.SelectChildren(PXGraph graph, ALLicenseProduct parent);
	void Verify(PXCache cache, Object item, List<Object> pars, Nullable`1& result, Object& value);
	Boolean AppendExpression(SQLExpression& exp, PXGraph graph, BqlCommandInfo info, Selection selection);



public class [nested] LicenseManager:Constant<IBqlNull,InfoType,LicenseManager>, IBqlOperand, IImplement<IBqlNull>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<LicenseManager>, IConstant<InfoType>, IConstant, IBqlAggregatedOperand, IBqlCreator, IBqlVerifier
	InfoType Value;
	void PX.Data.IBqlVerifier.Verify(PXCache cache, Object item, List<Object> pars, Nullable`1& result, Object& value);
	Boolean PX.Data.IBqlCreator.AppendExpression(SQLExpression& exp, PXGraph graph, BqlCommandInfo info, Selection selection);



public class [nested] DatabaseID:Constant<IBqlNull,InfoType,DatabaseID>, IBqlOperand, IImplement<IBqlNull>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<DatabaseID>, IConstant<InfoType>, IConstant, IBqlAggregatedOperand, IBqlCreator, IBqlVerifier
	InfoType Value;
	void PX.Data.IBqlVerifier.Verify(PXCache cache, Object item, List<Object> pars, Nullable`1& result, Object& value);
	Boolean PX.Data.IBqlCreator.AppendExpression(SQLExpression& exp, PXGraph graph, BqlCommandInfo info, Selection selection);



public class [nested] DatabaseName:Constant<IBqlNull,InfoType,DatabaseName>, IBqlOperand, IImplement<IBqlNull>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<DatabaseName>, IConstant<InfoType>, IConstant, IBqlAggregatedOperand, IBqlCreator, IBqlVerifier
	InfoType Value;
	void PX.Data.IBqlVerifier.Verify(PXCache cache, Object item, List<Object> pars, Nullable`1& result, Object& value);
	Boolean PX.Data.IBqlCreator.AppendExpression(SQLExpression& exp, PXGraph graph, BqlCommandInfo info, Selection selection);



public class [nested] FullServerName:Constant<IBqlNull,InfoType,FullServerName>, IBqlOperand, IImplement<IBqlNull>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<FullServerName>, IConstant<InfoType>, IConstant, IBqlAggregatedOperand, IBqlCreator, IBqlVerifier
	InfoType Value;
	void PX.Data.IBqlVerifier.Verify(PXCache cache, Object item, List<Object> pars, Nullable`1& result, Object& value);
	Boolean PX.Data.IBqlCreator.AppendExpression(SQLExpression& exp, PXGraph graph, BqlCommandInfo info, Selection selection);



public class [nested] HostName:Constant<IBqlNull,InfoType,HostName>, IBqlOperand, IImplement<IBqlNull>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<HostName>, IConstant<InfoType>, IConstant, IBqlAggregatedOperand, IBqlCreator, IBqlVerifier
	InfoType Value;
	void PX.Data.IBqlVerifier.Verify(PXCache cache, Object item, List<Object> pars, Nullable`1& result, Object& value);
	Boolean PX.Data.IBqlCreator.AppendExpression(SQLExpression& exp, PXGraph graph, BqlCommandInfo info, Selection selection);



public class [nested] InstanceID:Constant<IBqlNull,InfoType,InstanceID>, IBqlOperand, IImplement<IBqlNull>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<InstanceID>, IConstant<InfoType>, IConstant, IBqlAggregatedOperand, IBqlCreator, IBqlVerifier
	InfoType Value;
	void PX.Data.IBqlVerifier.Verify(PXCache cache, Object item, List<Object> pars, Nullable`1& result, Object& value);
	Boolean PX.Data.IBqlCreator.AppendExpression(SQLExpression& exp, PXGraph graph, BqlCommandInfo info, Selection selection);



public class [nested] IPAddress:Constant<IBqlNull,InfoType,IPAddress>, IBqlOperand, IImplement<IBqlNull>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<IPAddress>, IConstant<InfoType>, IConstant, IBqlAggregatedOperand, IBqlCreator, IBqlVerifier
	InfoType Value;
	void PX.Data.IBqlVerifier.Verify(PXCache cache, Object item, List<Object> pars, Nullable`1& result, Object& value);
	Boolean PX.Data.IBqlCreator.AppendExpression(SQLExpression& exp, PXGraph graph, BqlCommandInfo info, Selection selection);



public class [nested] InstallationID:Constant<IBqlNull,InfoType,InstallationID>, IBqlOperand, IImplement<IBqlNull>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<InstallationID>, IConstant<InfoType>, IConstant, IBqlAggregatedOperand, IBqlCreator, IBqlVerifier
	InfoType Value;
	void PX.Data.IBqlVerifier.Verify(PXCache cache, Object item, List<Object> pars, Nullable`1& result, Object& value);
	Boolean PX.Data.IBqlCreator.AppendExpression(SQLExpression& exp, PXGraph graph, BqlCommandInfo info, Selection selection);



public class [nested] InstallationDate:Constant<IBqlNull,InfoType,InstallationDate>, IBqlOperand, IImplement<IBqlNull>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<InstallationDate>, IConstant<InfoType>, IConstant, IBqlAggregatedOperand, IBqlCreator, IBqlVerifier
	InfoType Value;
	void PX.Data.IBqlVerifier.Verify(PXCache cache, Object item, List<Object> pars, Nullable`1& result, Object& value);
	Boolean PX.Data.IBqlCreator.AppendExpression(SQLExpression& exp, PXGraph graph, BqlCommandInfo info, Selection selection);



public class [nested] PrinterInfo:Constant<IBqlNull,InfoType,PrinterInfo>, IBqlOperand, IImplement<IBqlNull>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<PrinterInfo>, IConstant<InfoType>, IConstant, IBqlAggregatedOperand, IBqlCreator, IBqlVerifier
	InfoType Value;
	void PX.Data.IBqlVerifier.Verify(PXCache cache, Object item, List<Object> pars, Nullable`1& result, Object& value);
	Boolean PX.Data.IBqlCreator.AppendExpression(SQLExpression& exp, PXGraph graph, BqlCommandInfo info, Selection selection);



public class [nested] CurrentCompany:Constant<IBqlNull,InfoType,CurrentCompany>, IBqlOperand, IImplement<IBqlNull>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<CurrentCompany>, IConstant<InfoType>, IConstant, IBqlAggregatedOperand, IBqlCreator, IBqlVerifier
	InfoType Value;
	void PX.Data.IBqlVerifier.Verify(PXCache cache, Object item, List<Object> pars, Nullable`1& result, Object& value);
	Boolean PX.Data.IBqlCreator.AppendExpression(SQLExpression& exp, PXGraph graph, BqlCommandInfo info, Selection selection);



public class [nested] ShowAutomation:Operand<IBqlBool,Boolean,ShowAutomation>, IBqlOperand, IImplement<IBqlBool>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<ShowAutomation>



public class [nested] EnableAutomation:Operand<IBqlBool,Boolean,EnableAutomation>, IBqlOperand, IImplement<IBqlBool>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<EnableAutomation>



public class [nested] AddComments:Operand<IBqlBool,Boolean,AddComments>, IBqlOperand, IImplement<IBqlBool>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<AddComments>