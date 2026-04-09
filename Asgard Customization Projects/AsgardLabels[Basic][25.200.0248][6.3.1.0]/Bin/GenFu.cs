public class ReflectionExtensions
	IEnumerable<PropertyInfo> GetAllProperties(TypeInfo type);



public class DefaultValueChecker
	Boolean HasValue(Object instance, PropertyInfo property);
	Boolean IsDefaultEnum(Type type, Object value);



public class PropertyType:Enum, IComparable, IFormattable, IConvertible
	Int32 value__;
	PropertyType FirstNames;
	PropertyType LastNames;
	PropertyType PersonTitles;
	PropertyType Words;
	PropertyType Titles;
	PropertyType Domains;
	PropertyType StreetNames;
	PropertyType CityNames;
	PropertyType CanadianProvinces;
	PropertyType CanadianProvinceAbreviations;
	PropertyType UsaStates;
	PropertyType UsaStateAbreviations;
	PropertyType MusicArtists;
	PropertyType MusicGenre;
	PropertyType MusicAlbums;
	PropertyType Ingredients;
	PropertyType CompanyNames;
	PropertyType Industries;
	PropertyType Drugs;
	PropertyType MedicalProcedures;
	PropertyType Injuries;
	PropertyType Genders;
	PropertyType Lorem;
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



public class DateRules:Enum, IComparable, IFormattable, IConvertible
	Int32 value__;
	DateRules FutureDates;
	DateRules Within1Year;
	DateRules Within10Years;
	DateRules Within25years;
	DateRules Within50Years;
	DateRules Within100Years;
	DateRules PastDate;
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



public class FillerManager
	void ResetFillers();
	void ResetFillers();
	void RegisterFiller(IPropertyFiller filler);
	IPropertyFiller GetFiller(PropertyInfo propertyInfo);
	IPropertyFiller GetMethodFiller(MethodInfo methodInfo);
	IPropertyFiller GetMatchingPropertyFiller(PropertyInfo propertyInfo, IDictionary<String,IPropertyFiller> propertyFillers);
	IPropertyFiller GetMatchingMethodFiller(MethodInfo methodInfo, IDictionary<String,IPropertyFiller> propertyFillers);
	Result GetGenericFiller();
	IPropertyFiller GetGenericFillerForType(Type t);



public class CustomFiller:PropertyFiller<T>, IPropertyFiller
	String[] PropertyNames;
	String[] ObjectTypeNames;
	Boolean IsGenericFiller;
	Type PropertyType;
	Object GetValue(Object instance);



public class CustomFiller:PropertyFiller<T2>, IPropertyFiller
	String[] PropertyNames;
	String[] ObjectTypeNames;
	Boolean IsGenericFiller;
	Type PropertyType;
	Object GetValue(Object instance);



public class DateTimeFiller:PropertyFiller<DateTime>, IPropertyFiller
	DateTime Min;
	DateTime Max;
	String[] PropertyNames;
	String[] ObjectTypeNames;
	Boolean IsGenericFiller;
	Type PropertyType;
	Object GetValue(Object instance);
	DateTime GetRandomDate();



public class DateTimeOffsetFiller:PropertyFiller<DateTimeOffset>, IPropertyFiller
	String[] PropertyNames;
	String[] ObjectTypeNames;
	Boolean IsGenericFiller;
	Type PropertyType;
	Object GetValue(Object instance);



public class BirthDateFiller:PropertyFiller<DateTime>, IPropertyFiller
	String[] PropertyNames;
	String[] ObjectTypeNames;
	Boolean IsGenericFiller;
	Type PropertyType;
	Object GetValue(Object instance);



public class DateTimeFillerExtensions
	GenFuConfigurator<T> AsPastDate(GenFuDateTimeConfigurator<T> configurator);
	GenFuConfigurator<T> AsFutureDate(GenFuDateTimeConfigurator<T> configurator);



public class DateTimeNullableFiller:PropertyFiller<Nullable`1>, IPropertyFiller
	DateTime Min;
	DateTime Max;
	Double SeedPercentage;
	String[] PropertyNames;
	String[] ObjectTypeNames;
	Boolean IsGenericFiller;
	Type PropertyType;
	Object GetValue(Object instance);



public class EnumFiller:PropertyFiller<Enum>, IPropertyFiller
	String[] PropertyNames;
	String[] ObjectTypeNames;
	Boolean IsGenericFiller;
	Type PropertyType;
	Object GetValue(Object instance);



public class IPropertyFiller
	String[] PropertyNames;
	String[] ObjectTypeNames;
	Boolean IsGenericFiller;
	Type PropertyType;
	Object GetValue(Object instance);



public class IntFiller:PropertyFiller<Int32>, IPropertyFiller
	Int32 Min;
	Int32 Max;
	String[] PropertyNames;
	String[] ObjectTypeNames;
	Boolean IsGenericFiller;
	Type PropertyType;
	Object GetValue(Object instance);



public class NullableIntFiller:PropertyFiller<Nullable`1>, IPropertyFiller
	Int32 Min;
	Int32 Max;
	String[] PropertyNames;
	String[] ObjectTypeNames;
	Boolean IsGenericFiller;
	Type PropertyType;
	Object GetValue(Object instance);



public class NullableUIntFiller:PropertyFiller<Nullable`1>, IPropertyFiller
	UInt32 Min;
	UInt32 Max;
	String[] PropertyNames;
	String[] ObjectTypeNames;
	Boolean IsGenericFiller;
	Type PropertyType;
	Object GetValue(Object instance);



public class ShortFiller:PropertyFiller<Int16>, IPropertyFiller
	Int16 Min;
	Int16 Max;
	String[] PropertyNames;
	String[] ObjectTypeNames;
	Boolean IsGenericFiller;
	Type PropertyType;
	Object GetValue(Object instance);



public class NullableShortFiller:PropertyFiller<Nullable`1>, IPropertyFiller
	Int16 Min;
	Int16 Max;
	String[] PropertyNames;
	String[] ObjectTypeNames;
	Boolean IsGenericFiller;
	Type PropertyType;
	Object GetValue(Object instance);



public class NullableUShortFiller:PropertyFiller<Nullable`1>, IPropertyFiller
	UInt16 Min;
	UInt16 Max;
	String[] PropertyNames;
	String[] ObjectTypeNames;
	Boolean IsGenericFiller;
	Type PropertyType;
	Object GetValue(Object instance);



public class LongFiller:PropertyFiller<Int64>, IPropertyFiller
	Int32 Min;
	Int32 Max;
	String[] PropertyNames;
	String[] ObjectTypeNames;
	Boolean IsGenericFiller;
	Type PropertyType;
	Object GetValue(Object instance);



public class NullableLongFiller:PropertyFiller<Nullable`1>, IPropertyFiller
	Int32 Min;
	Int32 Max;
	String[] PropertyNames;
	String[] ObjectTypeNames;
	Boolean IsGenericFiller;
	Type PropertyType;
	Object GetValue(Object instance);



public class NullableULongFiller:PropertyFiller<Nullable`1>, IPropertyFiller
	Int32 Min;
	Int32 Max;
	String[] PropertyNames;
	String[] ObjectTypeNames;
	Boolean IsGenericFiller;
	Type PropertyType;
	Object GetValue(Object instance);



public class DecimalFiller:PropertyFiller<Decimal>, IPropertyFiller
	Decimal Min;
	Decimal Max;
	String[] PropertyNames;
	String[] ObjectTypeNames;
	Boolean IsGenericFiller;
	Type PropertyType;
	Object GetValue(Object instance);



public class NullableDecimalFiller:PropertyFiller<Nullable`1>, IPropertyFiller
	Decimal Min;
	Decimal Max;
	String[] PropertyNames;
	String[] ObjectTypeNames;
	Boolean IsGenericFiller;
	Type PropertyType;
	Object GetValue(Object instance);



public class DoubleFiller:PropertyFiller<Double>, IPropertyFiller
	Double Min;
	Double Max;
	String[] PropertyNames;
	String[] ObjectTypeNames;
	Boolean IsGenericFiller;
	Type PropertyType;
	Object GetValue(Object instance);



public class NullableDoubleFiller:PropertyFiller<Nullable`1>, IPropertyFiller
	Double Min;
	Double Max;
	String[] PropertyNames;
	String[] ObjectTypeNames;
	Boolean IsGenericFiller;
	Type PropertyType;
	Object GetValue(Object instance);



public class AgeFiller:PropertyFiller<Int32>, IPropertyFiller
	String[] PropertyNames;
	String[] ObjectTypeNames;
	Boolean IsGenericFiller;
	Type PropertyType;
	Object GetValue(Object instance);



public class PriceFiller:PropertyFiller<Decimal>, IPropertyFiller
	String[] PropertyNames;
	String[] ObjectTypeNames;
	Boolean IsGenericFiller;
	Type PropertyType;
	Object GetValue(Object instance);



public class PropertyFiller:IPropertyFiller
	String[] PropertyNames;
	String[] ObjectTypeNames;
	Boolean IsGenericFiller;
	Type PropertyType;
	void AddAllBaseTypes(String propertyName, Type objectType);
	Object GetValue(Object instance);



public class StringFillerExtensions
	GenFuConfigurator<T> AsEmailAddress(GenFuStringConfigurator<T> configurator);
	GenFuConfigurator<T> AsEmailAddressForDomain(GenFuStringConfigurator<T> configurator, String domain);
	GenFuConfigurator<T> AsTwitterHandle(GenFuStringConfigurator<T> configurator);
	GenFuConfigurator<T> AsArticleTitle(GenFuStringConfigurator<T> configurator);
	GenFuConfigurator<T> AsPhoneNumber(GenFuStringConfigurator<T> configurator);
	GenFuConfigurator<T> AsFirstName(GenFuStringConfigurator<T> configurator);
	GenFuConfigurator<T> AsPersonTitle(GenFuStringConfigurator<T> configurator);
	GenFuConfigurator<T> AsLastName(GenFuStringConfigurator<T> configurator);
	GenFuConfigurator<T> AsAddress(GenFuStringConfigurator<T> configurator);
	GenFuConfigurator<T> AsAddressLine2(GenFuStringConfigurator<T> configurator);
	GenFuConfigurator<T> AsCity(GenFuStringConfigurator<T> configurator);
	GenFuConfigurator<T> AsCanadianProvince(GenFuStringConfigurator<T> configurator);
	GenFuConfigurator<T> AsUsaState(GenFuStringConfigurator<T> configurator);
	GenFuConfigurator<T> AsMusicArtistName(GenFuStringConfigurator<T> configurator);
	GenFuConfigurator<T> AsMusicGenreName(GenFuStringConfigurator<T> configurator);
	GenFuConfigurator<T> AsMusicGenreDescription(GenFuStringConfigurator<T> configurator);
	GenFuConfigurator<T> AsLoremIpsumWords(GenFuStringConfigurator<T> configurator, Int32 numberOfWords);
	GenFuConfigurator<T> AsLoremIpsumSentences(GenFuStringConfigurator<T> configurator, Int32 numberOfSentences);
	GenFuConfigurator<T> AsPlaceholderImage(GenFuStringConfigurator<T> configurator, Int32 width, Int32 height, String text, String backgroundColor, String textColor, ImgFormat format);



public class StringFiller:PropertyFiller<String>, IPropertyFiller
	String[] PropertyNames;
	String[] ObjectTypeNames;
	Boolean IsGenericFiller;
	Type PropertyType;
	Object GetValue(Object instance);



public class ArticleTitleFiller:PropertyFiller<String>, IPropertyFiller
	String[] PropertyNames;
	String[] ObjectTypeNames;
	Boolean IsGenericFiller;
	Type PropertyType;
	Object GetValue(Object instance);



public class FirstNameFiller:PropertyFiller<String>, IPropertyFiller
	String[] PropertyNames;
	String[] ObjectTypeNames;
	Boolean IsGenericFiller;
	Type PropertyType;
	Object GetValue(Object instance);



public class PersonTitleFiller:PropertyFiller<String>, IPropertyFiller
	String[] PropertyNames;
	String[] ObjectTypeNames;
	Boolean IsGenericFiller;
	Type PropertyType;
	Object GetValue(Object instance);



public class LastNameFiller:PropertyFiller<String>, IPropertyFiller
	String[] PropertyNames;
	String[] ObjectTypeNames;
	Boolean IsGenericFiller;
	Type PropertyType;
	Object GetValue(Object instance);



public class EmailFiller:PropertyFiller<String>, IPropertyFiller
	String[] PropertyNames;
	String[] ObjectTypeNames;
	Boolean IsGenericFiller;
	Type PropertyType;
	Object GetValue(Object instance);



public class TwitterFiller:PropertyFiller<String>, IPropertyFiller
	String[] PropertyNames;
	String[] ObjectTypeNames;
	Boolean IsGenericFiller;
	Type PropertyType;
	Object GetValue(Object instance);



public class AddressFiller:PropertyFiller<String>, IPropertyFiller
	String[] PropertyNames;
	String[] ObjectTypeNames;
	Boolean IsGenericFiller;
	Type PropertyType;
	Object GetValue(Object instance);



public class AddressLine2Filler:PropertyFiller<String>, IPropertyFiller
	String[] PropertyNames;
	String[] ObjectTypeNames;
	Boolean IsGenericFiller;
	Type PropertyType;
	Object GetValue(Object instance);



public class CityFiller:PropertyFiller<String>, IPropertyFiller
	String[] PropertyNames;
	String[] ObjectTypeNames;
	Boolean IsGenericFiller;
	Type PropertyType;
	Object GetValue(Object instance);



public class StateFiller:PropertyFiller<String>, IPropertyFiller
	String[] PropertyNames;
	String[] ObjectTypeNames;
	Boolean IsGenericFiller;
	Type PropertyType;
	Object GetValue(Object instance);



public class StateAbreviationFiller:PropertyFiller<String>, IPropertyFiller
	String[] PropertyNames;
	String[] ObjectTypeNames;
	Boolean IsGenericFiller;
	Type PropertyType;
	Object GetValue(Object instance);



public class ProvinceFiller:PropertyFiller<String>, IPropertyFiller
	String[] PropertyNames;
	String[] ObjectTypeNames;
	Boolean IsGenericFiller;
	Type PropertyType;
	Object GetValue(Object instance);



public class ZipCodeFiller:PropertyFiller<String>, IPropertyFiller
	String[] PropertyNames;
	String[] ObjectTypeNames;
	Boolean IsGenericFiller;
	Type PropertyType;
	Object GetValue(Object instance);



public class PostalCodeFiller:PropertyFiller<String>, IPropertyFiller
	String[] PropertyNames;
	String[] ObjectTypeNames;
	Boolean IsGenericFiller;
	Type PropertyType;
	Object GetValue(Object instance);



public class PhoneNumberFiller:PropertyFiller<String>, IPropertyFiller
	String[] PropertyNames;
	String[] ObjectTypeNames;
	Boolean IsGenericFiller;
	Type PropertyType;
	Object GetValue(Object instance);



public class MusicAlbumTitleFiller:PropertyFiller<String>, IPropertyFiller
	String[] PropertyNames;
	String[] ObjectTypeNames;
	Boolean IsGenericFiller;
	Type PropertyType;
	Object GetValue(Object instance);



public class MusicArtistNameFiller:PropertyFiller<String>, IPropertyFiller
	String[] PropertyNames;
	String[] ObjectTypeNames;
	Boolean IsGenericFiller;
	Type PropertyType;
	Object GetValue(Object instance);



public class MusicGenreNameFiller:PropertyFiller<String>, IPropertyFiller
	String[] PropertyNames;
	String[] ObjectTypeNames;
	Boolean IsGenericFiller;
	Type PropertyType;
	Object GetValue(Object instance);



public class MusicGenreDescriptionFiller:PropertyFiller<String>, IPropertyFiller
	String[] PropertyNames;
	String[] ObjectTypeNames;
	Boolean IsGenericFiller;
	Type PropertyType;
	Object GetValue(Object instance);



public class GenericFillerDefaults
	void SetMinInt(Int32 min);
	void SetMaxInt(Int32 max);
	void SetMinShort(Int16 min);
	void SetMaxShort(Int16 max);
	void SetMinDecimal(Decimal min);
	void SetMaxDecimal(Decimal max);
	void SetMinDateTime(DateTime minValue);
	void SetMaxDateTime(DateTime maxValue);
	void SetSeedPercentage(Double value);
	DateTime GetMinDateTime();
	DateTime GetMaxDateTime();



public class A:GenFu
	T New();
	Object New(Type type);
	T New(T instance);
	Object New(Object instance);
	List<T> ListOf();
	List<Object> ListOf(Type type);
	List<T> ListOf(Int32 itemCount);
	List<Object> ListOf(Type type, Int32 itemCount);
	GenFuConfigurator Configure();
	GenFuConfigurator<T> Configure();
	GenFuConfigurator Set();
	GenFuConfigurator<T> Set();
	GenFuDefaulturator Default();
	void Reset();
	void Reset();
	void ListCount(Int32 count);



public class Eh:GenFu
	T New();
	Object New(Type type);
	T New(T instance);
	Object New(Object instance);
	List<T> ListOf();
	List<Object> ListOf(Type type);
	List<T> ListOf(Int32 itemCount);
	List<Object> ListOf(Type type, Int32 itemCount);
	GenFuConfigurator Configure();
	GenFuConfigurator<T> Configure();
	GenFuConfigurator Set();
	GenFuConfigurator<T> Set();
	GenFuDefaulturator Default();
	void Reset();
	void Reset();
	void ListCount(Int32 count);



public class GenFu
	DateTime MinDateTime;
	DateTime MaxDateTime;
	Random Random;
	T New();
	Object New(Type type);
	T New(T instance);
	Object New(Object instance);
	List<T> ListOf();
	List<Object> ListOf(Type type);
	List<T> ListOf(Int32 itemCount);
	List<Object> ListOf(Type type, Int32 itemCount);
	List<Object> BuildList(Type type, Int32 itemCount);
	void SetPropertyValue(Object instance, PropertyInfo property);
	void CallSetterMethod(Object instance, MethodInfo method);
	GenFuConfigurator Configure();
	GenFuConfigurator<T> Configure();
	GenFuConfigurator Set();
	GenFuConfigurator<T> Set();
	GenFuDefaulturator Default();
	void Reset();
	void Reset();
	void ListCount(Int32 count);



public class GenFuComplexPropertyConfigurator:GenFuConfigurator<T>
	MemberInfo PropertyInfo;
	GenFu GenFu;
	FillerManager Maggie;
	GenFuConfigurator<T> WithRandom(IList<T2> values);
	GenFuConfigurator<T> WithRandom(IEnumerable<T2> values);
	GenFuConfigurator<T> WithRandom(T2[] values);
	GenFuConfigurator<T> Fill(Expression<Func`2> expression, T2 value);
	GenFuConfigurator<T> Fill(Expression<Func`2> expression, Func<T2> filler);
	GenFuConfigurator<T> Fill(Expression<Func`2> expression, Func<T,T2> filler);
	GenFuIntegerConfigurator<T> Fill(Expression<Func`2> expression);
	GenFuShortConfigurator<T> Fill(Expression<Func`2> expression);
	GenFuDecimalConfigurator<T> Fill(Expression<Func`2> expression);
	GenFuStringConfigurator<T> Fill(Expression<Func`2> expression);
	GenFuDateTimeConfigurator<T> Fill(Expression<Func`2> expression);
	GenFuComplexPropertyConfigurator<T,T2> Fill(Expression<Func`2> expression);
	GenFuConfigurator<T> MethodFill(Expression<Action`1> expression, Func<T2> filler);
	GenFuComplexPropertyConfigurator<T,T2> MethodFill(Expression<Action`1> expression);
	GenFuConfigurator Fill(String propertyName, Func<T> filler);
	GenFuConfigurator Fill(Expression<Func`2> expression, T2 value);
	GenFuConfigurator Fill(Expression<Func`2> expression, Func<T2> filler);
	GenFuConfigurator Data(PropertyType propertyType, String filename);



public class GenFuConfigurator
	GenFu GenFu;
	FillerManager Maggie;
	GenFuConfigurator Fill(String propertyName, Func<T> filler);
	GenFuConfigurator Fill(Expression<Func`2> expression, T2 value);
	GenFuConfigurator Fill(Expression<Func`2> expression, Func<T2> filler);
	GenFuConfigurator Data(PropertyType propertyType, String filename);



public class GenFuConfigurator:GenFuConfigurator
	GenFu GenFu;
	FillerManager Maggie;
	PropertyInfo GetPropertyInfoFromExpression(Expression<Func`2> expression);
	MethodInfo GetMethodInfoFromExpression(Expression<Action`1> expression);
	GenFuConfigurator Fill(Expression<Func`2> expression, T2 value);
	GenFuConfigurator Fill(Expression<Func`2> expression, Func<T2> filler);
	GenFuConfigurator Fill(Expression<Func`2> expression, Func<T,T2> filler);
	GenFuIntegerConfigurator<T> Fill(Expression<Func`2> expression);
	GenFuShortConfigurator<T> Fill(Expression<Func`2> expression);
	GenFuDecimalConfigurator<T> Fill(Expression<Func`2> expression);
	GenFuStringConfigurator<T> Fill(Expression<Func`2> expression);
	GenFuDateTimeConfigurator<T> Fill(Expression<Func`2> expression);
	GenFuComplexPropertyConfigurator<T,T2> Fill(Expression<Func`2> expression);
	GenFuConfigurator MethodFill(Expression<Action`1> expression, Func<T2> filler);
	GenFuComplexPropertyConfigurator<T,T2> MethodFill(Expression<Action`1> expression);
	GenFuConfigurator Fill(String propertyName, Func<T> filler);
	GenFuConfigurator Fill(Expression<Func`2> expression, T2 value);
	GenFuConfigurator Fill(Expression<Func`2> expression, Func<T2> filler);
	GenFuConfigurator Data(PropertyType propertyType, String filename);



public class GenFuDateTimeConfigurator:GenFuConfigurator<T>
	MemberInfo PropertyInfo;
	GenFu GenFu;
	FillerManager Maggie;
	GenFuConfigurator<T> Fill(Expression<Func`2> expression, T2 value);
	GenFuConfigurator<T> Fill(Expression<Func`2> expression, Func<T2> filler);
	GenFuConfigurator<T> Fill(Expression<Func`2> expression, Func<T,T2> filler);
	GenFuIntegerConfigurator<T> Fill(Expression<Func`2> expression);
	GenFuShortConfigurator<T> Fill(Expression<Func`2> expression);
	GenFuDecimalConfigurator<T> Fill(Expression<Func`2> expression);
	GenFuStringConfigurator<T> Fill(Expression<Func`2> expression);
	GenFuDateTimeConfigurator Fill(Expression<Func`2> expression);
	GenFuComplexPropertyConfigurator<T,T2> Fill(Expression<Func`2> expression);
	GenFuConfigurator<T> MethodFill(Expression<Action`1> expression, Func<T2> filler);
	GenFuComplexPropertyConfigurator<T,T2> MethodFill(Expression<Action`1> expression);
	GenFuConfigurator Fill(String propertyName, Func<T> filler);
	GenFuConfigurator Fill(Expression<Func`2> expression, T2 value);
	GenFuConfigurator Fill(Expression<Func`2> expression, Func<T2> filler);
	GenFuConfigurator Data(PropertyType propertyType, String filename);



public class GenFuDecimalConfigurator:GenFuConfigurator<T>
	MemberInfo PropertyInfo;
	GenFu GenFu;
	FillerManager Maggie;
	GenFuConfigurator<T> WithinRange(Int32 min, Int32 max);
	GenFuConfigurator<T> WithRandom(Decimal[] values);
	GenFuConfigurator<T> WithRandom(List<Decimal> values);
	GenFuConfigurator<T> WithRandom(IEnumerable<Decimal> values);
	GenFuConfigurator<T> Fill(Expression<Func`2> expression, T2 value);
	GenFuConfigurator<T> Fill(Expression<Func`2> expression, Func<T2> filler);
	GenFuConfigurator<T> Fill(Expression<Func`2> expression, Func<T,T2> filler);
	GenFuIntegerConfigurator<T> Fill(Expression<Func`2> expression);
	GenFuShortConfigurator<T> Fill(Expression<Func`2> expression);
	GenFuDecimalConfigurator Fill(Expression<Func`2> expression);
	GenFuStringConfigurator<T> Fill(Expression<Func`2> expression);
	GenFuDateTimeConfigurator<T> Fill(Expression<Func`2> expression);
	GenFuComplexPropertyConfigurator<T,T2> Fill(Expression<Func`2> expression);
	GenFuConfigurator<T> MethodFill(Expression<Action`1> expression, Func<T2> filler);
	GenFuComplexPropertyConfigurator<T,T2> MethodFill(Expression<Action`1> expression);
	GenFuConfigurator Fill(String propertyName, Func<T> filler);
	GenFuConfigurator Fill(Expression<Func`2> expression, T2 value);
	GenFuConfigurator Fill(Expression<Func`2> expression, Func<T2> filler);
	GenFuConfigurator Data(PropertyType propertyType, String filename);



public class GenFuDefaulturator
	GenFu GenFu;
	FillerManager FillerManager;
	GenFuDefaulturator MaxInt(Int32 max);
	GenFuDefaulturator MinInt(Int32 min);
	GenFuDefaulturator MaxShort(Int16 max);
	GenFuDefaulturator MinShort(Int16 min);
	GenFuDefaulturator IntRange(Int32 min, Int32 max);
	GenFuDefaulturator ListCount(Int32 count);
	GenFuDefaulturator DateRange(DateTime minDateTime, DateTime maxDateTime);



public class GenFuIntegerConfigurator:GenFuConfigurator<T>
	MemberInfo PropertyInfo;
	GenFu GenFu;
	FillerManager Maggie;
	GenFuConfigurator<T> WithinRange(Int32 min, Int32 max);
	GenFuConfigurator<T> WithRandom(Int32[] values);
	GenFuConfigurator<T> WithRandom(List<Int32> values);
	GenFuConfigurator<T> WithRandom(IEnumerable<Int32> values);
	GenFuConfigurator<T> Fill(Expression<Func`2> expression, T2 value);
	GenFuConfigurator<T> Fill(Expression<Func`2> expression, Func<T2> filler);
	GenFuConfigurator<T> Fill(Expression<Func`2> expression, Func<T,T2> filler);
	GenFuIntegerConfigurator Fill(Expression<Func`2> expression);
	GenFuShortConfigurator<T> Fill(Expression<Func`2> expression);
	GenFuDecimalConfigurator<T> Fill(Expression<Func`2> expression);
	GenFuStringConfigurator<T> Fill(Expression<Func`2> expression);
	GenFuDateTimeConfigurator<T> Fill(Expression<Func`2> expression);
	GenFuComplexPropertyConfigurator<T,T2> Fill(Expression<Func`2> expression);
	GenFuConfigurator<T> MethodFill(Expression<Action`1> expression, Func<T2> filler);
	GenFuComplexPropertyConfigurator<T,T2> MethodFill(Expression<Action`1> expression);
	GenFuConfigurator Fill(String propertyName, Func<T> filler);
	GenFuConfigurator Fill(Expression<Func`2> expression, T2 value);
	GenFuConfigurator Fill(Expression<Func`2> expression, Func<T2> filler);
	GenFuConfigurator Data(PropertyType propertyType, String filename);



public class GenFuShortConfigurator:GenFuConfigurator<T>
	MemberInfo PropertyInfo;
	GenFu GenFu;
	FillerManager Maggie;
	GenFuConfigurator<T> WithinRange(Int16 min, Int16 max);
	GenFuConfigurator<T> WithRandom(Int16[] values);
	GenFuConfigurator<T> WithRandom(List<Int16> values);
	GenFuConfigurator<T> WithRandom(IEnumerable<Int16> values);
	GenFuConfigurator<T> Fill(Expression<Func`2> expression, T2 value);
	GenFuConfigurator<T> Fill(Expression<Func`2> expression, Func<T2> filler);
	GenFuConfigurator<T> Fill(Expression<Func`2> expression, Func<T,T2> filler);
	GenFuIntegerConfigurator<T> Fill(Expression<Func`2> expression);
	GenFuShortConfigurator Fill(Expression<Func`2> expression);
	GenFuDecimalConfigurator<T> Fill(Expression<Func`2> expression);
	GenFuStringConfigurator<T> Fill(Expression<Func`2> expression);
	GenFuDateTimeConfigurator<T> Fill(Expression<Func`2> expression);
	GenFuComplexPropertyConfigurator<T,T2> Fill(Expression<Func`2> expression);
	GenFuConfigurator<T> MethodFill(Expression<Action`1> expression, Func<T2> filler);
	GenFuComplexPropertyConfigurator<T,T2> MethodFill(Expression<Action`1> expression);
	GenFuConfigurator Fill(String propertyName, Func<T> filler);
	GenFuConfigurator Fill(Expression<Func`2> expression, T2 value);
	GenFuConfigurator Fill(Expression<Func`2> expression, Func<T2> filler);
	GenFuConfigurator Data(PropertyType propertyType, String filename);



public class GenFuStringConfigurator:GenFuConfigurator<T>
	MemberInfo PropertyInfo;
	GenFu GenFu;
	FillerManager Maggie;
	GenFuConfigurator<T> WithRandom(String[] values);
	GenFuConfigurator<T> WithRandom(List<String> values);
	GenFuConfigurator<T> WithRandom(IEnumerable<String> values);
	GenFuConfigurator<T> Fill(Expression<Func`2> expression, T2 value);
	GenFuConfigurator<T> Fill(Expression<Func`2> expression, Func<T2> filler);
	GenFuConfigurator<T> Fill(Expression<Func`2> expression, Func<T,T2> filler);
	GenFuIntegerConfigurator<T> Fill(Expression<Func`2> expression);
	GenFuShortConfigurator<T> Fill(Expression<Func`2> expression);
	GenFuDecimalConfigurator<T> Fill(Expression<Func`2> expression);
	GenFuStringConfigurator Fill(Expression<Func`2> expression);
	GenFuDateTimeConfigurator<T> Fill(Expression<Func`2> expression);
	GenFuComplexPropertyConfigurator<T,T2> Fill(Expression<Func`2> expression);
	GenFuConfigurator<T> MethodFill(Expression<Action`1> expression, Func<T2> filler);
	GenFuComplexPropertyConfigurator<T,T2> MethodFill(Expression<Action`1> expression);
	GenFuConfigurator Fill(String propertyName, Func<T> filler);
	GenFuConfigurator Fill(Expression<Func`2> expression, T2 value);
	GenFuConfigurator Fill(Expression<Func`2> expression, Func<T2> filler);
	GenFuConfigurator Data(PropertyType propertyType, String filename);



public class ResourceLoader
	List<String> Data(PropertyType propertyType);
	List<String> LoadStrings(String resourceName);
	void ReplacePropertyData(PropertyType propertyType, String filename);



public class BaseValueGenerator
	String Word();
	T GetRandomValue(T[] values);
	T GetRandomValue(List<T> values);
	T GetRandomValue(IEnumerable<T> values);



public class CalendarDate:BaseValueGenerator
	DateTime Date(DateTime earliestDate, DateTime latestDate);
	DateTime Date(DateRules rules);
	DateTime DateTimeFill(DateTime min, DateTime max);
	String Word();
	T GetRandomValue(T[] values);
	T GetRandomValue(List<T> values);
	T GetRandomValue(IEnumerable<T> values);



public class ContactInformation:BaseValueGenerator
	String Email(String domain);
	String Email();
	String Twitter();
	String PhoneNumber();
	String Word();
	T GetRandomValue(T[] values);
	T GetRandomValue(List<T> values);
	T GetRandomValue(IEnumerable<T> values);



public class Names:BaseValueGenerator
	String Title();
	String LastName();
	String FirstName();
	String PersonTitle();
	String UserName();
	String FullName();
	String Word();
	T GetRandomValue(T[] values);
	T GetRandomValue(List<T> values);
	T GetRandomValue(IEnumerable<T> values);



public class Qualities:BaseValueGenerator
	String Gender();
	String Word();
	T GetRandomValue(T[] values);
	T GetRandomValue(List<T> values);
	T GetRandomValue(IEnumerable<T> values);



public class Album:BaseValueGenerator
	String AlbumArtUrl;
	String Title();
	String Word();
	T GetRandomValue(T[] values);
	T GetRandomValue(List<T> values);
	T GetRandomValue(IEnumerable<T> values);



public class Artist:BaseValueGenerator
	String Name();
	String Word();
	T GetRandomValue(T[] values);
	T GetRandomValue(List<T> values);
	T GetRandomValue(IEnumerable<T> values);



public class Genre:BaseValueGenerator
	String Name();
	String Description();
	String Word();
	T GetRandomValue(T[] values);
	T GetRandomValue(List<T> values);
	T GetRandomValue(IEnumerable<T> values);



public class Drugs:BaseValueGenerator
	String Drug();
	String Word();
	T GetRandomValue(T[] values);
	T GetRandomValue(List<T> values);
	T GetRandomValue(IEnumerable<T> values);



public class Injuries:BaseValueGenerator
	String Injury();
	String Word();
	T GetRandomValue(T[] values);
	T GetRandomValue(List<T> values);
	T GetRandomValue(IEnumerable<T> values);



public class MedicalProcedures:BaseValueGenerator
	String MedicalProcedure();
	String Word();
	T GetRandomValue(T[] values);
	T GetRandomValue(List<T> values);
	T GetRandomValue(IEnumerable<T> values);



public class Lorem:BaseValueGenerator
	String GenerateWord();
	String GenerateWords(Int32 numberOfWords, Int32 commaPosition);
	String GenerateSentences(Int32 numberOfSentences);
	StringBuilder GenerateSentence(StringBuilder sentenceBuilder);
	String Word();
	T GetRandomValue(T[] values);
	T GetRandomValue(List<T> values);
	T GetRandomValue(IEnumerable<T> values);



public class Domains:BaseValueGenerator
	String DomainName();
	String Word();
	T GetRandomValue(T[] values);
	T GetRandomValue(List<T> values);
	T GetRandomValue(IEnumerable<T> values);



public class NetworkAddress:BaseValueGenerator
	String IPAddress();
	String MacAddress();
	String IPv6Address();
	String Word();
	T GetRandomValue(T[] values);
	T GetRandomValue(List<T> values);
	T GetRandomValue(IEnumerable<T> values);



public class Address:BaseValueGenerator
	Random random;
	String AddressLine();
	String AddressLine2();
	String City();
	String UsaState();
	String UsaStateAbreviation();
	String CanadianProvince();
	String CanadianProvinceAbreviation();
	String ZipCode();
	String PostalCode();
	String Word();
	T GetRandomValue(T[] values);
	T GetRandomValue(List<T> values);
	T GetRandomValue(IEnumerable<T> values);



public class LatLong:BaseValueGenerator
	String LatitudeAndLongitude();
	String Word();
	T GetRandomValue(T[] values);
	T GetRandomValue(List<T> values);
	T GetRandomValue(IEnumerable<T> values);



public class Ingredients:BaseValueGenerator
	String Ingredient();
	String Word();
	T GetRandomValue(T[] values);
	T GetRandomValue(List<T> values);
	T GetRandomValue(IEnumerable<T> values);



public class StaticRandom
	Random Instance;



public class StringBuilderExtensions
	StringBuilder AppendWhen(StringBuilder sb, String value, Boolean predicate);
	StringBuilder BuildFor(StringBuilder sb, Int32 times, Func<StringBuilder,StringBuilder> fn);
	StringBuilder BuildFor(StringBuilder sb, Int32 times, Func<StringBuilder,Int32,StringBuilder> fn);



public class StringExtensions
	String UppercaseFirst(String s);



public class PlaceholditUrlBuilder
	String UrlFor(Int32 width, Int32 height, String text, String backgroundColor, String textColor, ImgFormat format);



public class Resources
	ResourceManager ResourceManager;
	CultureInfo Culture;
	String CanadianProvinceAbreviations;
	String CanadianProvinceNames;
	String CityNames;
	String CompanyNames;
	String DomainNames;
	String Drugs;
	String FirstNames;
	String Genders;
	String Industries;
	String Ingredients;
	String Injuries;
	String LastNames;
	String Lorem;
	String MedicalProcedures;
	String MusicAlbums;
	String MusicArtists;
	String PersonTitles;
	String StreetNames;
	String Titles;
	String USAStateAbreviations;
	String USAStateNames;
	String Words;



public class BooleanFiller:PropertyFiller<Boolean>, IPropertyFiller
	String[] PropertyNames;
	String[] ObjectTypeNames;
	Boolean IsGenericFiller;
	Type PropertyType;
	Object GetValue(Object instance);



public class NullableBooleanFiller:PropertyFiller<Nullable`1>, IPropertyFiller
	String[] PropertyNames;
	String[] ObjectTypeNames;
	Boolean IsGenericFiller;
	Type PropertyType;
	Object GetValue(Object instance);



public class CharFiller:PropertyFiller<Char>, IPropertyFiller
	String[] PropertyNames;
	String[] ObjectTypeNames;
	Boolean IsGenericFiller;
	Type PropertyType;
	Object GetValue(Object instance);



public class NullableCharFiller:PropertyFiller<Nullable`1>, IPropertyFiller
	String[] PropertyNames;
	String[] ObjectTypeNames;
	Boolean IsGenericFiller;
	Type PropertyType;
	Object GetValue(Object instance);



public class DateTimeAdapterFiller:DateTimeNullableFiller, IPropertyFiller
	DateTime Min;
	DateTime Max;
	Double SeedPercentage;
	String[] PropertyNames;
	String[] ObjectTypeNames;
	Boolean IsGenericFiller;
	Type PropertyType;
	Object GetValue(Object instance);



public class DrugFiller:PropertyFiller<String>, IPropertyFiller
	String[] PropertyNames;
	String[] ObjectTypeNames;
	Boolean IsGenericFiller;
	Type PropertyType;
	Object GetValue(Object instance);



public class ImgFormat:Enum, IComparable, IFormattable, IConvertible
	Int32 value__;
	ImgFormat PNG;
	ImgFormat JPG;
	ImgFormat JPEG;
	ImgFormat GIF;
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



public class InjuryFiller:PropertyFiller<String>, IPropertyFiller
	String[] PropertyNames;
	String[] ObjectTypeNames;
	Boolean IsGenericFiller;
	Type PropertyType;
	Object GetValue(Object instance);



public class MedicalProcedureFiller:PropertyFiller<String>, IPropertyFiller
	String[] PropertyNames;
	String[] ObjectTypeNames;
	Boolean IsGenericFiller;
	Type PropertyType;
	Object GetValue(Object instance);



public class PersonFiller



public class USASocialSecurityNumberFiller:PropertyFiller<String>, IPropertyFiller
	String[] PropertyNames;
	String[] ObjectTypeNames;
	Boolean IsGenericFiller;
	Type PropertyType;
	Object GetValue(Object instance);
	String RandomDigit();



public class [nested] Defaults
	Double MIN_DOUBLE;
	Double MAX_DOUBLE;
	Int16 MIN_SHORT;
	Int16 MAX_SHORT;
	UInt16 MIN_USHORT;
	UInt16 MAX_USHORT;
	Decimal MIN_DECIMAL;
	Decimal MAX_DECIMAL;
	DateTime MIN_DATETIME;
	DateTime MAX_DATETIME;
	Double SEED_PERCENTAGE;
	Int32 MIN_INT;
	Int32 MAX_INT;
	UInt32 MIN_UINT;
	UInt32 MAX_UINT;
	Int32 LIST_COUNT;
	String FILE_DOMAIN_NAMES;
	String FILE_FIRST_NAMES;
	String FILE_LAST_NAMES;
	String FILE_PERSON_TITLES;
	String FILE_TITLES;
	String FILE_WORDS;
	String FILE_STREET_NAMES;
	String FILE_CITY_NAMES;
	String FILE_USA_STATE_NAMES;
	String FILE_USA_STATE_ABREVIATIONS;
	String FILE_CDN_PROVINCE_NAMES;
	String FILE_CDN_PROVINCE_ABREVIATIONS;
	String FILE_MUSIC_ARTIST;
	String FILE_MUSIC_ALBUM;
	String FILE_INGREDIENTS;
	String FILE_COMPANY_NAMES;
	String FILE_INDUSTRIES;
	String FILE_INJURIES;
	String FILE_GENDERS;
	String FILE_DRUGS;
	String FILE_LOREM;
	String STRING_LOADFAIL;



public class [nested] SexFiller:PropertyFiller<String>, IPropertyFiller
	String[] PropertyNames;
	String[] ObjectTypeNames;
	Boolean IsGenericFiller;
	Type PropertyType;
	Object GetValue(Object instance);



public class [nested] GenderFiller:PropertyFiller<String>, IPropertyFiller
	String[] PropertyNames;
	String[] ObjectTypeNames;
	Boolean IsGenericFiller;
	Type PropertyType;
	Object GetValue(Object instance);