public class CopyContext
	void RecordCopy(Object original, Object copy);
	Boolean TryGetCopy(Object original, Object& result);
	void Reset();



public class DeepCopier
	T Copy(T original);
	T Copy(T original, CopyContext context);



public class Immutable
	Immutable<T> Create(T value);



public class Immutable:ValueType
	T Value;
	Boolean Equals(Object obj);
	String ToString();
	Int32 GetHashCode();
	Int32 GetHashCodeOfPtr(IntPtr ptr);



public class ImmutableAttribute:Attribute, _Attribute
	Object TypeId;
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
	Boolean Equals(Object obj);
	Int32 GetHashCode();
	Boolean Match(Object obj);
	Boolean IsDefaultAttribute();
	void System.Runtime.InteropServices._Attribute.GetTypeInfoCount(UInt32& pcTInfo);
	void System.Runtime.InteropServices._Attribute.GetTypeInfo(UInt32 iTInfo, UInt32 lcid, IntPtr ppTInfo);
	void System.Runtime.InteropServices._Attribute.GetIDsOfNames(Guid& riid, IntPtr rgszNames, UInt32 cNames, UInt32 lcid, IntPtr rgDispId);
	void System.Runtime.InteropServices._Attribute.Invoke(UInt32 dispIdMember, Guid& riid, UInt32 lcid, Int16 wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr);