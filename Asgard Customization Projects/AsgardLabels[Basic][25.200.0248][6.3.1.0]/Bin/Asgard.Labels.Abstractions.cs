public class IBarcodeProvider
	IBarcode GetBarcode(Object barcodeRef);
	IBarcodeOption[] GetOptions(Nullable<Guid> barcodeRef);



public class IColorProvider
	ValueTuple<IColor,IColor> GetColors(Object foreColorRef, Object backColorRef);
	IRuleDriven[] GetColorRules(Nullable<Guid> colorRef);



public class IConfigProvider
	Object GetConfig(Object configRef);
	T GetConfig(Object configRef);



public class IContentProvider
	IContent GetContent(Object contentRef);
	IContentElement[] GetContentElements(Nullable<Guid> contentRef);



public class IFileProvider
	IFileInfo[] GetFiles(Object fileRef);
	IFileInfo GetMainFile(Object fileRef);
	IFileInfo SaveFile(IFileInfo fileInfo, IPrinterFile printerFile, AAFileExistsAction existsAction);



public class IFontProvider
	IFont GetFont(Object fontRef);



public class IFormatProvider
	IFormat GetFormat(Object formatRef);
	IRuleDriven[] GetFormatRules(Nullable<Guid> formatRef);



public class IJustificationProvider
	IJustification GetJustification(Object justificationRef);



public class ILabelElementProvider
	ILabelElement GetLabelElement(Object elementRef);



public class IMarginProvider
	IMargin GetMargin(Object objectRef);



public class IModelProvider
	IModel GetModel(Object modelRef);
	IModel[] GetModels(Func<IModel,Boolean> predicate);



public class IPrinterFileProvider
	IPrinterFile GetPrinterFile(Object fileRef);
	IPrinterFileTransfer GetPrinterFileTransfer(Object fileRef, Object printerRef);
	IEnumerable<IPrinterFileWithData> GetPrinterFilesWithData(Object printerFileID);
	IRuleDriven[] GetPrinterFileRules(Nullable<Guid> fileRef);



public class IPrinterProvider
	IPrinter GetPrinter(Object printerRef);
	IPrinter[] GetPrinters(Func<IPrinter,Boolean> predicate);



public class IPrintNodeProvider
	String GetComputerState(IPrintNodePrinter printer);
	String GetComputerState(IPrintNodeComputer computer);
	String GetPrinterState(IPrintNodePrinter printer);
	IPrintNodeComputer GetPrintNodeComputer(Nullable<Int32> computerID);
	IEnumerable<IPrintNodeComputer> GetPrintNodeComputers();
	IPrintNodePrinter GetPrintNodePrinter(Nullable<Int32> printerId);
	IEnumerable<IPrintNodePrinter> GetPrintNodePrinters(Nullable<Int32> computerID);
	void Reset();



public class IRuleProvider
	IRule GetRule(Object ruleRef);
	IRuleDetail[] GetRuleDetails(Nullable<Guid> ruleRef);



public class ISequenceProvider
	ISequence GetSequence(Object sequenceRef);



public class IStandardProvider
	IStandard GetStandard(Object standardRef);
	IStandardIndentifier[] GetStandardIdentifiers(Nullable<Guid> standardRef);



public class ISubstitutionProvider
	ISubstitution GetSubstitution(Object substitutionRef);
	ISubstitutionDetail[] GetSubstitutionDetails(Nullable<Guid> substitutionRef);



public class IBarcodeCmd:IPrinterCmd<L,O>, IPrinterCmd, ILanguageDriven
	String BarcodeType;
	String Name;
	String Dimension;



public class CmdConstraintType:Enum, IComparable, IFormattable, IConvertible
	Int32 value__;
	CmdConstraintType Values;
	CmdConstraintType Range;
	CmdConstraintType Fixed;
	CmdConstraintType Other;
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



public class ICmdConstraint
	CmdConstraintType ConstraintType;
	Object DefaultValue;
	Object Clean(Object newValue);



public class ICmdConstraint:ICmdConstraint
	Exception CheckValid(IPrinterCmd<L,O> barcode, ICmdOption<L,O> option, Object newValue);



public class ICmdOption
	String Code;
	String Description;



public class ICmdOption:ICmdOption
	ICmdConstraint<L,O> Constraint;



public class IPrinterCmd:IPrinterCmd
	ICmdOption`2[] Options;
	ICmdOption<L,O> GetOption(String optionCode);



public class IPrinterCmd
	String Code;
	String Description;
	String Raw;
	Int32 NbOptions;
	String Render(ILabelContext lc, Object[] values);



public class IPrinterLanguage:ISelectable, IRenderer
	String StartLabel;
	String EndLabel;
	String StartCommand;
	String EndCommand;
	String ArgDelimiter;
	Boolean AllowsColors;
	String LineWrapValue;
	String Validate(ILabelContext lc, String text);
	String SetNbCopies(ILabelContext lc, String text, Int32 totalQty, Nullable<Int32> pauseCutValue, Nullable<Int32> replicateEachSerial, Nullable<Boolean> overridePauseCount);
	ISerialInfo GetSerialInfo(ILabelContext lc, String text);
	IList<IPrinterCmd> GetCommands();
	String HandleDensity(ILabelContext lc, String text);
	void AddFontsForRendering(ILabelContext lc);
	ValueTuple<String,String> GetExpression(ILabelContext lc, IExprRow exprRow);
	String DrawGraphic(ILabelContext lc, IModelGraphic graphic);
	FileResult GetPrintResult(ILabelContext labelContext, Int32 nbCopies);



public class IPrinterLanguage:IPrinterLanguage, ISelectable, IRenderer
	Boolean TryGetBarcode(String barcodeType, IBarcodeCmd`2& bc);



public class ISerialInfo
	Int32 NbCopies;
	Nullable<Int32> NbSerials;
	Nullable<Int32> PauseCutValue;
	Int32 NbLabels;
	Boolean IsMultipleCopies;
	Boolean HasValue;



public class ILanguageFactory
	IPrinterLanguage GetLanguage(String language);



public class IEventLogger
	void WriteError(String message, Object[] args);
	void WriteError(Exception e);
	void WriteInformation(String message, Object[] args);
	void WriteInformation(Exception e);
	void WriteVerbose(String message, Object[] args);
	void WriteVerbose(Exception e);
	void WriteWarning(String message, Object[] args);
	void WriteWarning(Exception e);



public class IAcuPrinter:IPrinter, IRenderableConfig, ICloudPrinter, IPrintNodePrinter, IPrintNodeObject, IEpsonPrinter, ILabelPrinter



public class IAcuPrintLog:IPrintLog
	Nullable<Int32> InventoryID;
	String EntityType;
	String GraphType;
	Nullable<Guid> NoteID;
	Nullable<Int32> OwnerID;
	Nullable<Guid> RefNoteID;
	Nullable<Int32> BAccountID;
	String BasedOnView;
	Nullable<Guid> UserID;



public class IArgHolder
	String Arg1;
	String Arg2;
	String Arg3;
	String Arg4;
	String Arg5;
	String Arg6;



public class IBarcode:IRenderableConfig, ILanguageDriven
	String BarcodeType;



public class IBarcodeable
	Nullable<Guid> BarcodeID;



public class IBarcodeOption:IRenderableChild<Nullable`1>, IRenderableChild
	String Option;
	String Value;



public class ICloudPrinter:IRenderableConfig, IPrintNodePrinter, IPrintNodeObject



public class IColor:IParent, IRenderableConfig, IRuleResult
	Nullable<Int32> Alpha;
	Nullable<Int32> Red;
	Nullable<Int32> Green;
	Nullable<Int32> Blue;



public class IColored
	Nullable<Guid> ForeColorID;
	Nullable<Guid> BackColorID;



public class IContent:IParent, IRenderableConfig
	Nullable<Guid> StandardID;



public class IContentable
	Nullable<Guid> ContentID;
	String Code;



public class IContentElement:IRenderableChild<Nullable`1>, IRenderableChild, IExprRow, IExpr, IDataDriven, IDesignable, IElementDriven
	String Identifier;
	String PrePostUsage;
	Nullable<Guid> PreHumanSequenceID;
	Nullable<Guid> PostHumanSequenceID;
	Nullable<Guid> PreExprSequenceID;
	Nullable<Guid> PostExprSequenceID;
	Nullable<Guid> BarcodeSequenceID;
	String HriUsage;
	Nullable<Guid> RuleID;
	Nullable<Boolean> ReverseRule;



public class IContextNamed
	String ContextName;



public class ICoordinate
	Nullable<Decimal> PosX;
	Nullable<Decimal> PosY;



public class IDataDriven:IDesignable
	String SchemaID;
	String BasedOn;
	String ExprType;
	String ExprValue;



public class IDensity
	String PrintDensityType;
	Nullable<Int32> PrintDensity;



public class IDesignable
	String SampleBasedOn;
	String SampleType;
	String SampleValue;



public class IElementDriven
	Nullable<Guid> LabelElementID;



public class IEpsonPrinter:IPrinter, IRenderableConfig
	String MediaType;
	String MediaForm;
	String MediaSource;
	String MediaShape;
	String EdgeDetection;
	String PrintMode;



public class IExpr:IDataDriven, IDesignable



public class IExprRow:IExpr, IDataDriven, IDesignable, IElementDriven
	String ExprCode;



public class IFileInfo
	Byte[] BinData;
	Nullable<Guid> UID;
	String Name;
	String FullName;
	String Comment;
	Nullable<DateTime> RevisionDate;
	Nullable<Int32> RevisionId;



public class IFont:IRenderableConfig, ILanguageDriven
	String FontCode;
	Nullable<Int32> Family;
	String Style;
	Nullable<Guid> FontFileID;
	Nullable<Decimal> Height;
	Nullable<Decimal> Width;
	String SizeUnit;



public class IFormat:IRuleResult, IParent, IRenderableConfig, IDensity
	Nullable<Decimal> Width;
	Nullable<Decimal> Height;
	String SizeUnit;
	String Rotation;
	Nullable<Guid> MarginID;



public class IFormatDriven
	Nullable<Guid> FormatID;



public class IGraphicCreator
	String ImageToLanguage(ILabelContext context, Byte[] imageBytes);
	Boolean IsSupported(ILabelContext context, Format format);



public class IGraphicCreatorFactory
	IGraphicCreator GetGraphicCreator(ILabelContext lc, Format fileFormat);



public class IImageDriven
	Nullable<Guid> PrinterFileGUID;



public class IIterable
	Nullable<Guid> SnippetID;



public class IJustification:IRenderableConfig
	String Alignment;
	Nullable<Decimal> FromX;
	Nullable<Decimal> ToX;
	Nullable<Int32> MaxLines;
	String SizeUnit;
	Nullable<Decimal> SpaceBetweenLines;
	Nullable<Decimal> HangingIndent;



public class IJustifyable
	Nullable<Guid> JustificationID;



public class ILabelElement:IRenderableConfig, IExprRow, IExpr, IDataDriven, IDesignable, IElementDriven, IContentable, IImageDriven, IBarcodeable, ISubstitutable, IIterable, IArgHolder
	Nullable<Boolean> GenName;



public class ILabelPrinter
	String Drive;
	Nullable<Boolean> SupportsLongFiles;
	Nullable<Int32> Encoding;
	Nullable<Boolean> PushFonts;



public class ILanguageDriven
	String Language;



public class IMargin:IRenderableConfig
	String SizeUnit;
	Nullable<Decimal> Left;
	Nullable<Decimal> Right;
	Nullable<Decimal> Top;
	Nullable<Decimal> Bottom;



public class IModel:IRenderableConfig, ILanguageDriven
	String ModelType;
	Nullable<Int32> SendPauseEvery;
	Nullable<Guid> FormatID;
	Nullable<Guid> MarginID;
	Nullable<Boolean> DealingMode;
	String PrintOnOtherDensity;
	Nullable<Boolean> IgnoreRotationOnRender;
	Nullable<Int32> ZplEncoding;
	String BasedOnSchema;
	Nullable<Guid> PrintRuleID;
	Nullable<Boolean> ReversePrint;
	String NbCopiesExpr;
	String DealingCountExpr;
	String CloudID;
	Nullable<Boolean> MergeDetails;
	Nullable<Decimal> DefaultSize;
	String SizeUnit;



public class IModelDetail:IExprRow, IExpr, IDataDriven, IDesignable, IElementDriven, IContentable, IBarcodeable, IOrientable, IReversable, IColored, IRuleDriven, IRenderableChild<Nullable`1>, IRenderableChild, IJustifyable, ICoordinate
	Nullable<Guid> FontID;
	String HexEncoding;
	Nullable<Boolean> ValueRequired;



public class IModelGraphic:IReversable, IColored, IRenderableChild<Nullable`1>, IRenderableChild
	String GraphicType;
	Nullable<Decimal> FromX;
	Nullable<Decimal> FromY;
	Nullable<Decimal> ToX;
	Nullable<Decimal> ToY;
	Nullable<Int32> Thickness;
	String SizeUnit;
	Nullable<Int32> Rounding;



public class IOrientable
	String Orientation;



public class IParent:IRenderableConfig
	Nullable<Boolean> IsComposite;



public class IPdfFonts



public class IPdfOptions
	PdfPageSize PageSizeEnum;
	PdfPageOrientation PageOrientationEnum;
	PdfPageHAlign PageHAlignEnum;
	PdfPageVAlign PageVAlignEnum;



public class IPrinter:IRenderableConfig
	String PrinterType;
	Nullable<Boolean> IsRendering;
	Nullable<Guid> PrintStationID;
	Nullable<Guid> FormatID;
	Nullable<Guid> MarginID;
	Nullable<Boolean> IsEpson;
	Nullable<Int32> ContentType;
	Nullable<Guid> AcuPrinterID;
	String FieldName;



public class IPrinterFile:IRuleResult, IParent, IRenderableConfig
	String FileName;
	String ShortFileName;
	String Extension;
	Nullable<Int32> Size;
	String FontStyle;
	Nullable<Guid> NoteID;



public class IPrinterFileTransfer:IRenderableChild<Nullable`1>, IRenderableChild
	Nullable<Guid> PrinterID;
	Nullable<Guid> PrinterFileID;
	String SentAs;



public class IPrinterFileWithData:IPrinterFile, IRuleResult, IParent, IRenderableConfig
	Nullable<Guid> FileID;
	Nullable<Int32> FileRevisionID;
	Byte[] BinData;



public class IPrintLog
	Nullable<Int32> RecordID;
	Nullable<Int32> ContentType;
	String ImageUrl;
	String LabelFilename;
	String LabelKey;
	String LotSerialNbr;
	Nullable<Guid> ModelFormatID;
	Nullable<Guid> ModelID;
	Nullable<Guid> ModelMarginID;
	Nullable<Int32> NbCopies;
	Nullable<Guid> PrinterFormatID;
	Nullable<Guid> PrinterID;
	Nullable<Guid> PrinterMarginID;
	Nullable<Int64> PrintJobID;
	Nullable<Guid> PrintStationID;
	String SchemaID;



public class IPrintNodeComputer:IPrintNodeObject
	Nullable<Int32> ComputerID;
	String PrintNodeAPIKey;
	String State;



public class IPrintNodeObject
	String Name;



public class IPrintNodePrinter:IPrintNodeObject
	Nullable<Int32> PrintNodePrinterID;
	Nullable<Int32> PrintNodeComputerID;
	String PrintNodeAPIKey;
	String PrinterState;



public class IRenderableChild:IRenderableChild
	T ChildID;



public class IRenderableChild
	Nullable<Guid> ParentID;
	Nullable<Int32> LineNbr;
	Nullable<Int32> SortOrder;
	Nullable<Boolean> Active;
	Nullable<DateTime> CreatedDateTime;
	Nullable<DateTime> LastModifiedDateTime;



public class IRenderableConfig
	Nullable<Guid> ID;
	String Name;
	String Description;
	Nullable<Boolean> Active;
	Nullable<DateTime> CreatedDateTime;
	Nullable<DateTime> LastModifiedDateTime;



public class IRenderableConfigKey
	Object Key;



public class IReversable
	Nullable<Boolean> ReverseDots;



public class IRowIterator
	Coll Rows;
	Int32 RowCount;
	Object Row;
	Int32 RowNumber;



public class IRule:IParent, IRenderableConfig
	String Expression;



public class IRuleDetail:IRenderableChild<Nullable`1>, IRenderableChild
	String OpenBracket;
	String CloseBracket;
	String Operation;
	Nullable<Boolean> Reverse;



public class IRuleDriven:IRenderableChild<Nullable`1>, IRenderableChild
	Nullable<Int32> BAccountID;
	Nullable<Guid> RuleID;
	Nullable<Boolean> ReverseRule;
	Nullable<Boolean> DoThrow;
	String Message;



public class IRuleResult:IParent, IRenderableConfig



public class IScribanLib
	String Prefix;
	ScriptMemberImportFlags ImportFlags;



public class ISelectable
	String Code;
	String Description;



public class ISequence:IRenderableConfig
	Byte[] Data;



public class IStandard:IRenderableConfig



public class IStandardIndentifier:IRenderableConfig
	String Identifier;
	String Regex;



public class ISubstitutable:IRenderableConfig
	Nullable<Guid> HolderSubstitutionID;
	Nullable<Boolean> DoSubstitute;



public class ISubstitution:IParent, IRenderableConfig
	String TypeName;
	String FunctionName;
	String InternalName;
	String ExternalName;
	Nullable<Int16> NbArgs;
	String ArgNames;
	String DefValues;
	String ReturnTypeName;



public class ISubstitutionDetail:IRenderableChild<Nullable`1>, IRenderableChild, ISubstitutable, IRenderableConfig, IArgHolder



public class AAException:Exception, ISerializable, _Exception
	UInt32 ExceptionNumber;
	String Message;
	String MessageNoNumber;
	String MessageNoPrefix;
	String MessagePrefix;
	IDictionary Data;
	Exception InnerException;
	MethodBase TargetSite;
	String StackTrace;
	String HelpLink;
	String Source;
	Int32 HResult;
	Exception ExtractInner(Exception exception);
	String GetLocalizedMessage(String message);
	void GetObjectData(SerializationInfo info, StreamingContext context);
	Exception PreserveStack(Exception exception);
	Exception GetBaseException();
	void SetErrorCode(Int32 hr);
	String ToString();
	Object DeepCopyStackTrace(Object currentStackTrace);
	Object DeepCopyDynamicMethods(Object currentDynamicMethods);
	void GetStackTracesDeepCopy(Object& currentStackTrace, Object& dynamicMethodArray);
	void RestoreExceptionDispatchInfo(ExceptionDispatchInfo exceptionDispatchInfo);
	String InternalToString();
	Type GetType();
	String GetMessageFromNativeResources(ExceptionMessageKind kind);
	void AddExceptionDataForRestrictedErrorInfo(String restrictedError, String restrictedErrorReference, String restrictedCapabilitySid, Object restrictedErrorObject, Boolean hasrestrictedLanguageErrorObject);
	Boolean TryGetRestrictedLanguageErrorObject(Object& restrictedErrorObject);
	Exception PrepForRemoting();
	void SaveStackTracesFromDeepCopy(Exception exception, Object currentStackTrace, Object dynamicMethodArray);
	void InternalPreserveStackTrace();



public class AAMessages
	String Prefix;
	String NoTypeFoundFor;
	String NoImplOfInterface;
	String NoLabel;
	String DontKnowHowToConvertType;
	String NotProperFormat;
	String ValueNotCompatible;
	String NoValueInContextNamed;
	String DontKnowHowToRenderThisLanguage;
	String CannotConvertRotationToZpl;
	String CommandOnlySupport;
	String CommandOptionNotValid;
	String CommandOptionCannotCompare;
	String RotationWithColorNotSupported;
	String CommandOptionRegexNoMatch;
	String MissingDataElement;
	String FillAllArgs;
	String FontFileNotConfigured;
	String CouldNotFind;
	String NoDataFoundForImage;
	String AnImageIsMissing;
	String NoImageFound;
	String MissingImage;
	String CommandOptionLength;
	String MissingExprType;
	String SubstitutionChildNotFound;
	String ServiceNotFound;
	String ContentWithCodeNotFound;
	String CouldNotFindContent;
	String CouldNotFindContentElement;
	String ErrorWithContent;
	String CannotFindSequenceInContent;
	String RenderingWithCompositeRequires;
	String CannotConvertExpression;
	String TemplateHasErrors;
	String ExpectedScriptForStatement;
	String ErrorWithScriptNode;
	String APIUrlRequiredFor;
	String APIKeyRequiredFor;
	String ValidateAlert;
	String RendererMaxCalls;
	String CannotTransform;
	String FileNotFound;
	String PrinterFileNotFoundForDataElement;
	String OnlyScribanLibs;
	String MPCLDoesNotSupportBarcodeType;
	String NoContextName;
	String InvalidSizeUnitConversionFromTo;
	String YouCantPeekNextSerial;
	String CallHasWrongReturnType;
	String CtorHasWrongReturnType;
	String UnexpectedContentType;
	String BarcodeHasNoHeight;
	String Warnings;
	String HttpError;
	String UnableToFindExtension;
	String UnableToDetermineImageType;
	String HttpRequestCompleted;
	String CannotFindFormat;
	String NoRowToPrint;
	String PleaseDefinePrinter;
	String YouHaveToConfigureRenderingPrinter;
	String PrinterDetailMustBeDefined;
	String LabelPrintingIgnored;
	String CannotConvertFileToZpl;
	String TypeNotFound;
	String MissingRenderingPrinter;
	String GraphicCreatorNotDefined;
	String RequiredExpressionValue;
	String LabelsGenerated;
	String NoLabelGenerated;
	String MergingNotSupportedForFormat;
	String FontFileNotFoundForFont;



public class BasicHelper
	IEnumerable<String> INCLUDED_NAMES;
	Int32[] MissingHttpStatusValues;
	String UNIX_CR;
	String WIN_CR;
	String EXT_PNG;
	String EXT_JPG;
	String EXT_TTF;
	String EXT_OTF;
	String EXT_FNT;
	String EXT_ZPL;
	String EXT_PDF;
	String EXT_FMT;
	String EXT_GRF;
	String DOUBLE_OPEN_BRACE;
	String DOUBLE_CLOSE_BRACE;
	String SINGLE_OPEN_BRACE;
	String SINGLE_CLOSE_BRACE;
	String ESCAPE_START;
	String ESCAPE_END;
	Char ESCAPE_CHAR;
	String OPEN_DOUBLE_PAREN;
	String CLOSE_DOUBLE_PAREN;
	String OPEN_SINGLE_PAREN;
	String CLOSE_SINGLE_PAREN;
	String OPEN_SINGLE_BRACKET;
	String CLOSE_SINGLE_BRACKET;
	String SINGLE_QUOTE;
	String DOUBLE_QUOTE;
	Func<IModel,Boolean> IS_GROUP;
	Func<IModel,Boolean> IS_SINGLE;
	Func<IModel,Boolean> IS_SINGLE_OR_GROUP;
	Func<IModel,Boolean> IS_ACTIVE;
	TimeSpan GetSpan(Nullable<DateTime> startDate, Nullable<DateTime> endDate, TimeSpan defaultSpan);
	IEnumerable<TResult> LeftJoin(IEnumerable<TOuter> outer, IEnumerable<TInner> inner, Func<TOuter,Object> outerKeySelector, Func<TInner,Object> innerKeySelector, Func<TOuter,TInner,TResult> resultSelector);
	IEnumerable<TResult> FullOuterGroupJoin(IEnumerable<TOuter> outer, IEnumerable<TInner> inner, Func<TOuter,TKey> keySelectorOuter, Func<TInner,TKey> keySelectorInner, Func<IEnumerable`1,IEnumerable`1,TKey,TResult> projection, IEqualityComparer<TKey> cmp);
	IEnumerable<TResult> FullOuterJoin(IEnumerable<TOuter> outer, IEnumerable<TInner> inner, Func<TOuter,TKey> keySelectorOuter, Func<TInner,TKey> keySelectorInner, Func<TOuter,TInner,TKey,TResult> projection, TOuter defaultOuter, TInner defaultInner, IEqualityComparer<TKey> cmp);
	IEnumerable<String> SplitToLines(String input);
	void ForEach(IEnumerable<T> sequence, Action<Int32,T> action);
	void ForEachWithPrev(IEnumerable<T> sequence, Action<Int32,T,T> action);
	String AddLineNumbers(String str, Int32 paddedLength);
	T CloneObjectWithIL(T myObject);
	String ToString(IDictionary<String,Object> source);
	Type MakeGenericType(Type[] types);
	Type MakeGenericType(Type[] types, Int32& index);
	String StripAllSpaces(String str);
	Boolean HasSpace(String str);
	Boolean IsRendering(IPrinter printer);
	String StringifyItem(Object item);
	Boolean Contains(String str, String stringToSearch, StringComparison comparisonType);
	Boolean HasValue(String str);
	Boolean IsNullOrEmpty(String value);
	Boolean IsTrue(Nullable<Boolean> value);
	Boolean IsFalse(Nullable<Boolean> value);
	void ReadAs(XmlReader reader, Object target, Expression<Func`1> propSetter, T defaultValue);
	void Read(XmlReader reader, Object target, PropertyInfo prop, Object defaultValue);
	void ReadAs(XmlNode node, Object target, Expression<Func`1> propSetter, T defaultValue);
	void Read(XmlNode node, Object target, PropertyInfo prop, Object defaultValue);
	void ReadAll(XmlNode node, Object target, String[] exceptNames);
	String NullIfEmpty(String str);
	Boolean IsNullOrEmpty(Nullable<Guid> value);
	String EncodeToBase36(Int32 number);
	Int32 DecodeFromBase36(String base36);
	void ReverseArray(Int32[] arr);
	void ReverseArray(Byte[] arr);
	Func<T,Boolean> And(Func`2[] predicates);
	Predicate<T> Or(Predicate`1[] predicates);
	String GetDisplayDescription(E enumValue);
	String GetDisplayDescription(Type type);
	T ChangeType(Object value, IFormatProvider provider);
	Object ChangeType(Object value, Type type, IFormatProvider provider);
	Boolean IsPrimitive(Object result);
	Boolean IsPrimitive(Type type);
	Boolean IsNumber(String result);
	Boolean IsSimpleType(Type type);
	Boolean IsNumeric(Type type);
	Object RoundNearest(Object value, Object increment);
	Boolean TryCast(Object obj, T& result);
	String ReplaceCR(String str, String replaceBy);
	String ReplaceCRDouble(String str);
	String CleanWhitespaces(String str);
	String BytesToString(Byte[] bytes);
	T Choose(Object first, Object second);
	Boolean IsHexa(IEnumerable<Char> chars);
	String GetHttpErrorName(Int32 httpCode);
	String EnumToName(E value);
	String HexaToBinary(String hexa);
	Boolean IsBinary(String text);
	String Truncate(String value, Int32 maxLength);
	IEnumerable<Int32> Range(Int32 from, Int32 count);
	IEnumerable<ValueTuple`2> Coordinates(Int32 fromCol, Int32 nbCols, Int32 fromRow, Int32 nbRows);
	IEnumerable<ValueTuple`2> GetTuples(IEnumerable<Int32> rows, Int32 col);
	T[] Concat(T[][] arrays);
	Func<T1,Action`1> Curry(Action<T1,T2> function);
	Func<T1,Func`2> Curry(Action<T1,T2,T3> function);
	Func<T1,Func`2> Curry(Action<T1,T2,T3,T4> function);
	Func<T1,Func`2> Curry(Action<T1,T2,T3,T4,T5> function);
	Func<T1,Func`2> Curry(Func<T1,T2,TResult> function);
	Func<T1,Func`2> Curry(Func<T1,T2,T3,TResult> function);
	Func<T1,Func`2> Curry(Func<T1,T2,T3,T4,TResult> function);
	Func<T1,Func`2> Curry(Func<T1,T2,T3,T4,T5,TResult> function);
	Action<T2> CurryAndCall(Action<T1,T2> function, T1 arg1);
	Action<T3> CurryAndCall(Action<T1,T2,T3> function, T1 arg1, T2 arg2);
	Func<T2,TResult> CurryAndCall(Func<T1,T2,TResult> function, T1 arg1);
	Func<T3,TResult> CurryAndCall(Func<T1,T2,T3,TResult> function, T1 arg1, T2 arg2);
	Func<T4,TResult> CurryAndCall(Func<T1,T2,T3,T4,TResult> function, T1 arg1, T2 arg2, T3 arg3);
	Func<T5,TResult> CurryAndCall(Func<T1,T2,T3,T4,T5,TResult> function, T1 arg1, T2 arg2, T3 arg3, T4 arg4);
	Boolean IsOnlyLetter(String value);
	Boolean HasLetter(String value);
	Boolean IsOnlyDigit(String value);
	Boolean HasDigit(String value);
	Boolean IsGuid(String value);
	String AggregateJoin(IEnumerable<TSource> source, Func<TSource,String> func, String separator);
	IList GetGenericList(Type itemType);
	String SplitAndGet(Nullable<Int32> size, Int32 index, String toSplit, Char delim);
	String[] Split(Nullable<Int32> size, String toSplit, Char delim);
	String GetAsBase64(IFileInfo labelFile);
	String GetAsBase64(Byte[] bytes);
	String ConvertRGBToHex(Byte A, Byte R, Byte G, Byte B);
	void SetARGBColor(IColor currentColor, String hex);
	IEnumerable<Color> GetAllColors();
	DateTime StartOfDay(DateTime theDate);
	DateTime EndOfDay(DateTime theDate);
	IEnumerable<E> ExplodeFlags(E flags);
	IEnumerable<T> GetImplInstances();
	Type FindType(String typeName);
	Object Replace(Object newValue, Char oldChar, Char newChar);
	Object RemoveWhitespace(Object newValue);
	String RemoveWhitespace(String input);
	String FromFontStyleNamesToStyleMulti(String fontStyleNames);
	FontStyle FromStyleMultiToFontStyle(String styleList);
	Boolean HasFlag(String options, Nullable<Int64> flag);
	Boolean HasFlag(Type enumType, String options, Enum flag);
	Boolean HasFlag(String options, E flag);
	Boolean HasFlag(E options, E flag);
	E AsEnum(Nullable<Int32> value, E defaultValue);
	E FromIntListFlagsToEnum(String multiValueIntList);
	Enum FromIntListFlagsToEnum(Type enumType, String multiValueIntList);
	Object FromIntListFlagsToEnumInternal(ValueTuple<Type,String> tuple);
	Int64 CombineFlags(Int64 accum, Int32 bitSet);
	Int32[] ToIntArray(E flagEnum);
	Int32 FlagNameToBitSet(E flagEnum, String flagName);
	Int32 FlagValueToBitSet(E item);
	Int32 FlagValueToBitSet(Int64 flagValue);
	Guid Int2Guid(Nullable<Int32> intValue);
	Int32 Guid2Int(Nullable<Guid> guidValue);
	T GetValue(IList<Object> values, Int32 index);
	ValueTuple`2[] FindTuples(SortBy sortBy);
	ValueTuple`2[] FindTuples(Type CodeType, Type DescType, SortBy sortBy);
	ValueTuple`2[] FindTuplesInternal(ThreeArgs args);
	ValueTuple`2[] FindTuplesEnumInternal(ThreeArgs args);
	String GetDesc(ThreeArgs args, Int32 intVal);
	IEnumerable<FieldInfo> FindConstants(Type type);
	IEnumerable<IEnumerable`1> CartesianProduct(IEnumerable<IEnumerable`1> sequences);
	Byte[] ReadFully(Stream input);
	Object SetValue(PropertyInfo prop, Object obj, Object attrValue);
	String SpreadCamelCase(String str);
	String SpreadCamelCase2(String str);
	String CamelToUnderscore(String str);
	Object Cast(Type Type, Object data);
	String GetFieldsAsString(IEnumerable<String> fields);
	String HoursToTimeSpanStr(Decimal nbHours);
	Int32 HoursToMinutes(Decimal nbHours);
	Int32 TimeSpanToMinutes(Object timeSpanObj);
	IEnumerable<Type> GetImplementations();
	ValueTuple<String,ScriptMemberImportFlags> GetLibInfo(Type funcLib);
	IScribanLib CreateLibrary(Type funcLib);
	MethodInfo[] GetLibraryMethods(Type funcLib);
	Func<IModel,Boolean> GetByModelType(String modelType);
	Boolean IsSingle(IModel model);
	Boolean IsGroup(IModel model);
	T CreateImpl(String implCodeID);
	T CreateImpl2(String implCode);
	IDictionary<String,String> GetImpls();
	ISelectable GetInstance(Type selectableType);
	IEnumerable<Type> GetImplementations(Boolean silent);
	IEnumerable<Type> GetImplementations(Type interfaceType, Boolean silent);
	IEnumerable<Type> GetImplementationsInternal(Type interfaceType, Boolean silent);
	Boolean KeepAssembly(Assembly assembly);
	IEnumerable<Type> GetImplementationsByAssembly(Assembly ass, Boolean silent);
	void AddImplementationsInternalByAssembly(Assembly ass, List<Type> impls, Type interfaceType, Boolean silent);
	IEnumerable<Type> GetByInterface(Type interfaceType, Boolean silent);
	IEnumerable<Type> GetByInterfaceInternal(Type interfaceType, Boolean silent);
	String ToLowerFirst(String name);
	String ToUpperFirst(String name);
	Boolean IsNotEmpty(IEnumerable<Object> objs);
	T[] NonNulls(T[] objs);
	IEnumerable<T> NonNulls(IEnumerable<T> objs);
	Boolean IsNull(Object obj);
	Boolean NotNull(Object obj);
	Boolean NotNull(T obj);
	Boolean IsNotEmpty(Object obj);
	Boolean IsEmpty(Object obj);
	Boolean IsCompatibleWith(Type toCheck, Type ofPotentialBase);
	Boolean IsInstanceOfGenericType(Object instance, Type genericType);
	Boolean IsSubclassOfRawGeneric(Type toCheck, Type baseType);
	String GetExtension(String fileName);
	String StripExtension(String fileName);
	String AsName(ContentFormat value);
	String AsExtension(ContentFormat value);
	String GetAccept(ContentFormat value, Boolean zplAsText);
	ContentFormat AsOutputFormat(Byte[] bytes);
	ContentFormat AsOutputFormat(IFileInfo fi);
	ContentFormat AsOutputFormat(Nullable<Int32> value);
	ContentFormat AsOutputFormat(String extension);
	Boolean IsFont(IFileInfo fi);
	Boolean IsFont(String name);
	Boolean IsImage(String name);
	Boolean IsImage(Byte[] data);
	Boolean IsImage(IFileInfo fi);
	Boolean IsTtf(IFileInfo fi);
	Boolean IsOtf(IFileInfo fi);
	Boolean IsPng(IFileInfo fi);
	Boolean IsJpg(IFileInfo fi);
	Boolean IsZpl(IFileInfo fi);
	Boolean IsPdf(IFileInfo fi);
	Boolean IsFmt(IFileInfo fi);
	Boolean IsFileFormat(IFileInfo fi, String fileFormat);
	Boolean IsFileFormat(IFileInfo fi, Format fileFormat);
	Boolean IsTtf(String name);
	Boolean IsOtf(String name);
	Boolean IsPng(String name);
	Boolean IsJpg(String name);
	Boolean IsZpl(String name);
	Boolean IsPdf(String name);
	Boolean IsFmt(String name);
	Boolean IsFileFormat(String name, Format fileFormat);
	Boolean IsPng(Byte[] bytes);
	Boolean IsJpg(Byte[] bytes);
	Boolean IsZpl(Byte[] bytes);
	Boolean IsPdf(Byte[] bytes);
	Boolean IsFileFormat(Byte[] bytes, Format fileFormat);
	String GetExtension(Byte[] fileData);
	Format GetFileFormat(Stream stream);
	Boolean HasZplCommand(String exprValue);
	Boolean HasZpl(String body);
	Format GetFileFormat(Byte[] fileData);
	Boolean IsBase64String(String base64);
	Boolean HasScriban(String text);
	Boolean HasMoreToRender(String text);
	String EscapeExpression(String expr, Int32 level);
	String StripQuotes(String expr);
	String StripDoubleQuotes(String expr);
	String StripParentheses(String expr);
	String StripBrackets(String expr);
	String StripDoubleParentheses(String expr);
	String StripDoubleBraces(String expr);
	String StripSingleBraces(String expr);
	String ToScribanCR(String expr);
	String ToScriban(String expr);
	String InDoubleParenth(String data);
	String InSingleParenth(String data);
	String InQuotes(Object expr);
	String InBrackets(Object expr);
	String ToPipe(String expr);
	String SurroundBy(Object expr, String startsWith, String endsWith);
	Boolean IsQuoted(String expr);
	Boolean IsScribanBraced(String expr);
	Boolean IsBracketed(String expr);
	Boolean IsSurroundedBy(String expr, String startsWith, String endsWith);
	Boolean IsScribanExpression(String expr);
	Boolean ContainsAll(String expr, String[] parts);
	String ReplaceParenthesesByBrackets(String expr);
	String Strip(String expr, String startsWith, String endsWith);
	String StripEnd(String expr, String endsWith);
	String StripStart(String expr, String startsWith);
	Boolean HasIllegalCharacters(IRenderableConfig row);
	String RemoveIllegalFileNameCharacters(String newValue);
	String ReplaceCharactersBy(Char replaceBy, String newValue, Char[] chars);
	void CopyPropertiesTo(Object source, Object dest);
	String Merge(IEnumerable<String> strs, String separator);
	String Merge(IEnumerable<String> strs);
	String Merge(String[] strs);
	String Prepend(String str, String[] sequences);
	String GetPlural(Int32 nb);
	String GetVerb(Int32 nb);
	String GetObjectName(ILabelPrinter printer, IPrinterFile printerFile);
	String GetObjectName(ILabelPrinter printer, IPrinterFile printerFile, String extension);
	String GetObjectNameNoExt(ILabelPrinter printer, IPrinterFile printerFile);
	String GetObjectName(String drive, String filename, String extension);



public class ByteHelper
	String ToBase64(Byte[] bytes);
	Byte[] FromBase64(String base64);
	String ToHexFromBytes(Byte[] bytes);
	Byte[] ToBytesFromHex(String hex);
	Byte[] EncodeBytes(String hex);
	Byte[] HexToBytes(String hex);
	String BytesToHex(Byte[] bytes);
	Int32 GetHexVal(Char hex);



public class CacheHelper2
	V GetOrAdd(K key, Func<K,V> valueFactory);
	V TryRemove(K key, Func<K,V> valueFactory);
	void Set(K key, Func<K,V> valueFactory, V value);
	V GetOrAdd(K key, Func<K,FArg,V> valueFactory, FArg factoryArgument);
	void Clear(Func<K,V> valueFactory);
	void ClearAll();
	ConcurrentDictionary<K,V> CreateCache(Object cacheKey);



public class Crc16
	UInt16 Compute(Byte[] bytes);
	String ComputeHex(Byte[] bytes);
	String ComputeHex(Object value);



public class Crc32
	UInt32 Compute(Byte[] data);
	String ComputeHex(Byte[] bytes);



public class Ensure
	Nullable<T> HasValue(Nullable<T> value, String paramName);
	T IsBetween(T value, T min, T max, String paramName);
	T IsEqualTo(T value, T comparand, String paramName);
	T IsGreaterThan(T value, T comparand, String paramName);
	T IsGreaterThanOrEqualTo(T value, T comparand, String paramName);
	Int32 IsGreaterThanOrEqualToZero(Int32 value, String paramName);
	Int64 IsGreaterThanOrEqualToZero(Int64 value, String paramName);
	TimeSpan IsGreaterThanOrEqualToZero(TimeSpan value, String paramName);
	Int32 IsGreaterThanZero(Int32 value, String paramName);
	Int64 IsGreaterThanZero(Int64 value, String paramName);
	Double IsGreaterThanZero(Double value, String paramName);
	TimeSpan IsGreaterThanZero(TimeSpan value, String paramName);
	TimeSpan IsInfiniteOrGreaterThanOrEqualToZero(TimeSpan value, String paramName);
	TimeSpan IsInfiniteOrGreaterThanZero(TimeSpan value, String paramName);
	T IsNotNull(T value, String paramName);
	IEnumerable<T> IsNotNullAndDoesNotContainAnyNulls(IEnumerable<T> values, String paramName);
	String IsNotNullOrEmpty(String value, String paramName);
	IEnumerable<T> IsNotNullOrEmpty(IEnumerable<T> value, String paramName);
	T IsNull(T value, String paramName);
	Nullable<T> IsNullOrBetween(Nullable<T> value, T min, T max, String paramName);
	Nullable<Int32> IsNullOrGreaterThanOrEqualToZero(Nullable<Int32> value, String paramName);
	Nullable<Int64> IsNullOrGreaterThanOrEqualToZero(Nullable<Int64> value, String paramName);
	Nullable<Int32> IsNullOrGreaterThanZero(Nullable<Int32> value, String paramName);
	Nullable<Int64> IsNullOrGreaterThanZero(Nullable<Int64> value, String paramName);
	Nullable<TimeSpan> IsNullOrGreaterThanZero(Nullable<TimeSpan> value, String paramName);
	Nullable<TimeSpan> IsNullOrInfiniteOrGreaterThanOrEqualToZero(Nullable<TimeSpan> value, String paramName);
	String IsNullOrNotEmpty(String value, String paramName);
	Nullable<TimeSpan> IsNullOrValidTimeout(Nullable<TimeSpan> value, String paramName);
	TimeSpan IsValidTimeout(TimeSpan value, String paramName);
	void That(Boolean assertion, String message);
	void That(Boolean assertion, String message, String paramName);
	T That(T value, Func<T,Boolean> assertion, String paramName, String message);



public class FontHelper
	Font LoadFontFromBytes(Byte[] fontData, Single size);



public class AAStackTrace
	Func<Int32,Object> GetThreadLockObject;
	StackTrace GetStackTrace(Int32 skipFrames, Boolean needFileInfo);



public class AATrace
	ILogger Logger;
	Boolean u003cCreateBackEndu003eg__LogExceptionu007c7_0(Exception e, String message);
	LogEventLevel GetLogEventLevel(SourceLevels level);
	String GetTraceEventType(TraceEventType eventType);
	void UseSerilog(ILogger logger);
	ILogger WithSourceLocation(String caller, String path, Int32 line);
	ILogger WithStack();
	void Write(TraceEventType type, LogEventLevel level, Exception e);
	void Write(TraceEventType type, LogEventLevel level, Exception exception, String message, Object[] args);
	void Write(Event item);
	void WriteError(String message);
	void WriteError(String format, Object[] args);
	void WriteError(Exception exception, String format, Object[] args);
	void WriteError(Exception e);
	void WriteImpl(Event item, LogEvent original);
	Boolean IsExcluded(Exception exception, String stackTrace, ValueTuple`4[] conditions);
	Boolean IsExcluded(Exception exception, String stackTrace, ValueTuple`3[] conditions);
	void WriteInformation(String message);
	void WriteInformation(String format, Object[] args);
	void WriteInformation(Exception e);
	void WriteVerbose(String message);
	void WriteVerbose(String format, Object[] args);
	void WriteVerbose(Exception e);
	void WriteWarning(String message);
	void WriteWarning(String format, Object[] args);
	void WriteWarning(Exception e);



public class ReflectHelper
	T CreateInstanceGeneric(Type type, Type[] genericTypes, Type[] paramTypes, Object[] args);
	Object CreateInstanceGeneric(Type type, Type[] genericTypes, Type[] paramTypes, Object[] args);
	Object CallInstanceGenericMethod(Object instance, String methodName, Type[] genericTypes, Type[] paramTypes, Object[] args);
	T CallInstanceMethod(MethodBase m, Object instance, Boolean instanceInCallingSignature, Object[] parameters);
	T CallInstanceMethod(Object instance, String methodName, Type[] paramTypes, Object[] parameters);
	Object CallInstanceMethod(Object instance, String methodName, Type[] paramTypes, Object[] parameters);
	T CallStaticMethod(MethodBase m, Type type, Object[] parameters);
	ValueTuple<String,Type[]> GetMethodInfo(MethodBase m, Int32 skip);
	Object GetField(Object instance, String fieldName);
	Object GetProperty(Object instance, String propName);
	void SetField(Object instance, String fieldName, Object value);
	void SetProperty(Object instance, String propName, Object value);
	List<T> GetPropertyAsList(Object instance, String propName);



public class StringBuilderCache
	StringBuilder Acquire(Int32 capacity);
	void Release(StringBuilder sb);
	String GetStringAndRelease(StringBuilder sb);



public class AAContextHelper
	IMargin EMPTY_MARGIN;
	ValueTuple<String,String> DOUBLE_NULL;
	ValueTuple<String,String,String> TRIPLE_NULL;
	Decimal POINT_PER_IN;
	String ROW;
	String DETAIL_ROWS;
	String LINE_WRAP;
	Double GetDensityRatio(ILabelContext lc);
	Double GetDensityRatio(IFormat density1, IFormat density2);
	Boolean IsSameDensity(ILabelContext lc);
	Decimal GetInches(Decimal length, String sizeUnit);
	Decimal GetPixels(Decimal length, String sizeUnit);
	Decimal GetPoints(Decimal length, String sizeUnit);
	Decimal GetMms(Decimal length, String sizeUnit);
	ValueTuple<Decimal,Decimal,Decimal,Decimal> GetInches(IFormat format, IMargin margin);
	ValueTuple<Decimal,Decimal,Decimal,Decimal> GetMms(IFormat format, IMargin margin);
	ValueTuple<Int32,Int32,Int32,Int32> GetDots(IDensity density, IMargin margin);
	Int32 GetDots(Nullable<Decimal> length, String sizeUnit, IDensity density);
	Int32 GetDots(Nullable<Decimal> length, String sizeUnit, String densityType, Int32 densityVal);
	Int32 GetDots(IFormat density);
	Nullable<Int32> GetWidthDots(IFormat modelFormat);
	Nullable<Int32> GetHeightDots(IFormat modelFormat);
	Nullable<Int32> GetDots(Nullable<Decimal> size, String sizeUnit, String densityType, Nullable<Int32> density);
	Int32 GetDpmm(IFormat density);
	Int32 GetDpmm(String densityType, Int32 densityVal);
	Nullable<Double> GetRatio(String arg);
	String HandleScreen(ILabelContext lc, IExpr expr);
	String HandleScript(ILabelContext lc, IExpr expr);
	String HandleFunction(ILabelContext lc, IExpr expr, IArgHolder argHolder);
	void HandleArg(StringBuilder sb, IArgHolder argHolder, String arg);
	String[] GetArgs(IArgHolder argHolder);
	ValueTuple<String,String,String> GetTypeAndValue(ILabelContext lc, IExpr expr);
	String AppendLineWrap(ILabelContext lc, IModelDetail expr, String exprValue);
	ValueTuple<String,String> HandleRule(ILabelContext lc, IRuleDriven expr);
	String If(ILabelContext lc, IRule rule, Boolean reverse);
	String Endif(ILabelContext lc, IRule rule);
	String HandleSubstitution(ILabelContext lc, ISubstitutable substitutable, String exprValue);
	String GetSubstitution(ILabelContext lc, ISubstitutable substitutable, ISubstitution substitution, Boolean asPipe);
	String GetChildSubstitution(ILabelContext lc, ISubstitutable substitutable, ISubstitution substitution, ISubstitutionDetail detail);
	IMargin ToUnit(IMargin margin, String sizeUnit);
	IMargin Add(IMargin m1, IMargin m2);
	String CreateFunctionCall(String prefix, Delegate del, Object[] args);
	Object CheckArgument(Object arg);
	String HandleIterator(ILabelContext lc, ILabelElement dataElement, ICoordinate expr);
	IMargin Add(IMargin modelMargin, IFormat modelFormat, IMargin printerMargin, IFormat printerFormat);
	ILabelContext GetLabelContext(TemplateContext context);
	Boolean HasValue(TemplateContext context, String name);
	Boolean HasValue(TemplateContext context);
	T GetValue(TemplateContext context, Boolean allowNull);
	T GetValue(TemplateContext context, String name, Boolean allowNull, T defaultValue);
	void SetValue(IScriptObject container, Object value);
	void SetValue(IScriptObject container, String name, Object value);
	void SetValue(TemplateContext context, Object value);
	void SetValue(TemplateContext context, String name, Object value);
	String GetName(Object value);



public class IIteratorContext:IIteratorContext
	IIteratorPage<Coll> NextPage();



public class IIteratorContext
	Int32 PageSize;
	Int32 NbColumns;
	Int32 NbRows;
	ICoordinate Coordinate;
	Decimal VerticalOffset;
	Decimal HorizontalOffset;
	Boolean VerticalFirst;
	Int32 PageNbr;
	Int32 NbPages;
	Boolean IsByPage;
	Boolean HasMorePages;
	Int32 TotalRowCount;
	Int32 IteratorRowNbr;
	Int32 PageRowNbr;
	LayoutZpl Layout;
	TemplateContext ScribanContext;
	void End();
	IIteratorPage NextPage();
	void SetPageRowNbr(Int32 recNumber);



public class IIteratorPage:IIteratorPage
	Coll Results;



public class IIteratorPage
	IEnumerable Results;
	Int32 PageNbr;
	IList List;
	Int32 Count;



public class ILabelContext:ILabelContext, IRuleEvalContext, IFontProvider, IColorProvider, IFileProvider, IRuleProvider, ISubstitutionProvider, IJustificationProvider, IBarcodeProvider, IModelProvider, IContentProvider, IStandardProvider, ISequenceProvider, IPrinterFileProvider, IConfigProvider, IFormatProvider, ILanguageFactory, ILabelElementProvider, IMarginProvider, IEventLogger, IPrinterProvider, ILanguageDriven
	IRowIterator<Coll> PageIterator;
	IRowIterator<Coll> RowIterator;
	ModelType Model;
	PrintLogType PrintLog;
	IIteratorContext<Coll> GetIteratorContext(ILabelElement dataElement, ICoordinate coordinate);
	ILabelContext CreateIteratorContext(ILabelContext parent, Nullable<Guid> snippetID);



public class ILabelContext:IRuleEvalContext, IFontProvider, IColorProvider, IFileProvider, IRuleProvider, ISubstitutionProvider, IJustificationProvider, IBarcodeProvider, IModelProvider, IContentProvider, IStandardProvider, ISequenceProvider, IPrinterFileProvider, IConfigProvider, IFormatProvider, ILanguageFactory, ILabelElementProvider, IMarginProvider, IEventLogger, IPrinterProvider, ILanguageDriven
	IModel Model;
	IPrintLog PrintLog;
	Object PageRow;
	Object IteratorRow;
	IEnumerable DetailRows;
	IEnumerable PageRows;
	IEnumerable IteratorRows;
	Double DensityRatio;
	Object DetailRow;
	IEnumerable<IModelDetail> Expressions;
	IEnumerable<IModelGraphic> Graphics;
	IEnumerable<FontFile> FontFiles;
	IEnumerable<IFont> Fonts;
	IEnumerable<IRenderableChild`1> Children;
	ContentFormat FinalOutputFormat;
	FileResult CurrentResult;
	ContentFormat CurrentFormat;
	Boolean IgnorePrinterMissing;
	Boolean IsAlwaysPrint;
	Boolean IsRaw;
	Boolean IsRender;
	Boolean IsRendered;
	Boolean IsSameDensity;
	Boolean IsSaveRendered;
	Boolean IsSilent;
	Boolean IsSnippet;
	Boolean AddComments;
	Boolean DealingMode;
	Boolean IsDevMode;
	Boolean RawExpressionsOnly;
	Boolean MergeDetails;
	Object LabelRow;
	IFormat ModelFormat;
	IMargin ModelMargin;
	IPdfOptions PdfOptions;
	IPrinter Printer;
	IFormat PrinterFormat;
	IPrinterLanguage PrinterLanguage;
	IMargin PrinterMargin;
	Object Row;
	Boolean SendPause;
	Object SingleRow;
	String TemplateBody;
	Boolean HandleIteratorRecord(IIteratorContext iteratorContext, IIteratorPage page, ILabelContext snippetLC, List<String> snippets, Int32 recNumber, Int32 colNbr, Int32 rowNbr);
	TService Resolve(Parameter[] parameters);
	IDestination GetDestination();
	IDestination GetDestination(IPrinter printer);
	ValueTuple<Int32,Int32> GetGotoDots(Decimal x, Decimal y);
	Int32 GetNbCopies();
	Int32 GetDealingCount();
	Nullable<Int32> GetNextSerial();
	String GetRenderedTemplate();
	ISerialInfo GetSerialInfo(String content);
	Nullable<Int32> PeekNextSerial();
	void Print(IFileInfo fi, IPrintLog logRow, Nullable<Int32> nbCopies);
	void Print(FileResult printResult);
	FileResult RenderAndSaveAsUrl(IPrintLog log);
	RenderResult RenderAsOutput();
	Object GetFileServiceReference(Object row);
	FileResult SaveToPrintLog(FileResult printResult, String fieldName, Boolean saveAsUrl);
	void SaveFileInfo(IFileInfo fileInfo, String fieldName, Boolean saveAsUrl);
	FileResult SaveFileToPrintLog(FileResult printResult, String prefix);
	Int32 GetMaxWidthDots();
	Int32 GetMaxHeightDots();
	ZplEncoding GetEncoding();
	IMargin CalcMargin();
	IFormat GetFormat();
	IIteratorContext GetIteratorContext(ILabelElement dataElement, ICoordinate coordinate);
	ILabelContext CreateIteratorContext(ILabelContext parent, Nullable<Guid> snippetID);
	void SaveRendered(String rendered);
	Boolean IteratorHasMorePages();
	void PrepareForNextPage();
	void EndIterator();
	Exception GetException(String message, Object[] args);
	Exception GetException(Exception inner, String message, Object[] args);
	IRenderer GetRenderer();
	ILabelContext CreateRenderContext(String zpl, ContentFormat outputFormat);
	String DropDownToText(Type CodeType, Type DescType, String dropDownValue);
	ValueTuple`2[] DropDownToTexts(Type CodeType, Type DescType);
	String Stringify(Object item);
	IGraphicCreator GetGraphicCreator(Format fileFormat);
	T GetArgValueAs(IArgHolder argHolder, Int32 argNbr, T defaultValue);
	void EndMerge();



public class IRenderer
	RenderResult RenderAsOutput(ILabelContext context);
	Boolean SupportsRendering(ILabelContext context, ContentFormat from, ContentFormat to);



public class IRuleEvalContext
	Nullable<Int32> BAccountID;
	Boolean IsDesignMode;
	TemplateContext ScribanContext;



public class LayoutPdf
	Decimal MAX_POINTS;
	Decimal PageWidthPoints;
	Decimal PageHeightPoints;
	Decimal MarginLeftPoints;
	Decimal MarginTopPoints;
	Decimal MarginRightPoints;
	Decimal MarginBottomPoints;
	Decimal ContentWidthPoints;
	Decimal ContentHeightPoints;
	ICoordinate Offset;
	Decimal CalcColPercToPoints(Decimal colPerc);
	Decimal CalcRowPercToPoints(Decimal rowPerc);
	Decimal CalcColPointsToPoints(Decimal colPoints);
	Decimal CalcRowPointsToPoints(Decimal rowPoints);
	ValueTuple<Single,Single> ConvertToPoints(Decimal percX, Decimal percY);
	ICoordinate AddOffset(ICoordinate baseCoord, ICoordinate offset);
	Decimal GetMaxWidthPoints();
	Decimal GetMaxHeightPoints();



public class LayoutZpl
	Int32 MAX_DOTS;
	Int32 MaxWidthDots;
	Int32 MaxHeightDots;
	ICoordinate Offset;
	Int32 OffsetLeftDots;
	Int32 OffsetTopDots;
	Int32 MarginLeftDots;
	Int32 MarginRightDots;
	Int32 MarginCenterDots;
	Int32 MarginTopDots;
	Int32 Column;
	Int32 Row;
	Int32 FontHeight;
	ICoordinate AddOffset(ICoordinate expr, ICoordinate offset);
	ValueTuple<Int32,Int32> Goto(Int32 colDots, Int32 rowDots, VerticalAlign vAlign, HorizontalAlign hAlign);
	ValueTuple<Int32,Int32> GotoH(Int32 colDots, HorizontalAlign hAlign);
	ValueTuple<Int32,Int32> GotoV(Int32 rowDots, VerticalAlign vAlign);
	Int32 CalcRow(Decimal rowNumber);
	Int32 CalcCol(Decimal colNumber);
	Int32 CalcColPercToDots(Decimal percentage, Boolean addMargin);
	Int32 CalcColDotsToDots(Int32 dots);
	Int32 CalcRowPercToDots(Decimal percentage, Boolean addMargin);
	Decimal CalcColDotsToPerc(Int32 colDots);
	Int32 CalcRowDotsToDots(Int32 dots);
	Decimal CalcRowDotsToPerc(Int32 rowDots);
	Boolean PositionRecalculated(ICoordinate coordinate);
	Decimal AdjustCol(Decimal fromCol, HorizontalAlign hAlign);
	Decimal AdjustRow(Decimal fromRow, VerticalAlign vAlign);
	void SavePosition();
	void RestorePosition();
	void Save(String[] keys);
	void Save(String key);
	void Restore(String[] keys);
	void Restore(String key);
	void SetCurrent(Int32 colDots, Int32 rowDots);
	void SetCurrent(String fontName, Nullable<Int32> fontHeight, Nullable<Int32> fontWidth);
	LayoutZpl Set(String key, Object value);
	LayoutZpl SetSaved(String key, Object value);
	LayoutZpl Set(IDictionary<String,Object> dict, String key, Object value);
	T Get(String key);
	T GetSaved(String key);
	T Get(IDictionary<String,Object> dict, String key);
	String ToString();
	ICoordinate GetDotsCoordinate(Nullable<Decimal> xPos, Nullable<Decimal> yPos);
	Nullable<Int32> GetWidthDots(ICoordinate fromDots, ICoordinate toDots);
	Nullable<Int32> GetWidthDots(Int32 xDots1, Int32 xDots2);
	Nullable<Int32> GetHeightDots(ICoordinate fromDots, ICoordinate toDots);
	Nullable<Int32> GetHeightDots(Int32 yDots1, Int32 yDots2);
	ValueTuple<Nullable`1,Nullable`1> GetFontSize(String fontName, Nullable<Decimal> height, Nullable<Decimal> width, String sizeUnit, Double densityRatio);



public class AbstractPrintDestination:IDestination, ISelectable
	IPrinter Printer;
	String Code;
	String Description;
	FileResult Print(ILabelContext lc, FileResult printResult);
	FileResult DoPrint(ILabelContext lc, FileResult printResult);
	FileResult HandleTransformations(ILabelContext lc, FileResult printResult);



public class IDestination:ISelectable
	IPrinter Printer;
	FileResult Print(ILabelContext lc, FileResult result);



public class AADropDown



public class AAConstants



public class AAFileExistsAction:Enum, IComparable, IFormattable, IConvertible
	Int32 value__;
	AAFileExistsAction ThrowException;
	AAFileExistsAction CreateVersion;
	AAFileExistsAction ReturnExisting;
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



public class AAFileInfo:IFileInfo
	Byte[] BinData;
	Nullable<Guid> UID;
	String FullName;
	String Comment;
	String Name;
	Nullable<DateTime> RevisionDate;
	Nullable<Int32> RevisionId;
	String GetShortName(String fullName);



public class AbstractCoordinate:ICoordinate, IEqualityComparer<ICoordinate>
	Nullable<Decimal> PosX;
	Nullable<Decimal> PosY;
	Boolean Equals(ICoordinate other);
	Int32 GetHashCode();
	Boolean Equals(ICoordinate x, ICoordinate y);
	Int32 GetHashCode(ICoordinate obj);



public class ContentFormat:Enum, IComparable, IFormattable, IConvertible
	Int32 value__;
	ContentFormat Unknown;
	ContentFormat BMP;
	ContentFormat DPL;
	ContentFormat EPL;
	ContentFormat PDF;
	ContentFormat GIF;
	ContentFormat IPL;
	ContentFormat JSON;
	ContentFormat JPG;
	ContentFormat FMT;
	ContentFormat PNG;
	ContentFormat SBPL;
	ContentFormat XML;
	ContentFormat ZPL;
	ContentFormat PCX;
	ContentFormat GRF;
	ContentFormat FP;
	ContentFormat NV;
	ContentFormat B64;
	ContentFormat PCL5;
	ContentFormat PCL6;
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



public class Coordinate:AbstractCoordinate, ICoordinate, IEqualityComparer<ICoordinate>
	ICoordinate EMPTY;
	Nullable<Decimal> PosX;
	Nullable<Decimal> PosY;
	Boolean Equals(ICoordinate other);
	Int32 GetHashCode();
	Boolean Equals(ICoordinate x, ICoordinate y);
	Int32 GetHashCode(ICoordinate obj);



public class DefaultFormat:DefaultRenderableParent, IRenderableConfig, IParent, IFormat, IRuleResult, IDensity
	IFormat DEFAULT_FORMAT;
	Nullable<Decimal> Width;
	Nullable<Decimal> Height;
	String SizeUnit;
	String PrintDensityType;
	Nullable<Int32> PrintDensity;
	String Rotation;
	Nullable<Guid> MarginID;
	Nullable<Boolean> IsComposite;
	Nullable<Guid> ID;
	String Name;
	String Description;
	Nullable<Boolean> Active;
	Nullable<DateTime> CreatedDateTime;
	Nullable<DateTime> LastModifiedDateTime;



public class DefaultMargin:DefaultRenderableConfig, IRenderableConfig, IMargin
	String SizeUnit;
	Nullable<Decimal> Left;
	Nullable<Decimal> Right;
	Nullable<Decimal> Top;
	Nullable<Decimal> Bottom;
	Nullable<Guid> ID;
	String Name;
	String Description;
	Nullable<Boolean> Active;
	Nullable<DateTime> CreatedDateTime;
	Nullable<DateTime> LastModifiedDateTime;
	IMargin ToUnit(String unit);



public class DefaultRenderableConfig:IRenderableConfig
	Nullable<Guid> ID;
	String Name;
	String Description;
	Nullable<Boolean> Active;
	Nullable<DateTime> CreatedDateTime;
	Nullable<DateTime> LastModifiedDateTime;



public class DefaultRenderableParent:DefaultRenderableConfig, IRenderableConfig, IParent
	Nullable<Boolean> IsComposite;
	Nullable<Guid> ID;
	String Name;
	String Description;
	Nullable<Boolean> Active;
	Nullable<DateTime> CreatedDateTime;
	Nullable<DateTime> LastModifiedDateTime;



public class FileResult
	Object RemoteResult;
	IPrintLog Log;
	IFileInfo File;
	Byte[] BinData;
	Nullable<Guid> UID;
	String Name;
	String FullName;
	Int32 Size;
	String Extension;
	ContentFormat Format;
	Int32 NbCopies;
	IFileInfo GetFileInfo(Byte[] data);
	void Deconstruct(IPrintLog& logRow, IFileInfo& labelFile);
	String GetDebuggerDisplay();



public class FontFile
	IFont Font;
	IPrinterFile File;



public class HardColor:IColor, IParent, IRenderableConfig, IRuleResult
	String Name;
	Nullable<Guid> ID;
	Nullable<Int32> Alpha;
	Nullable<Int32> Red;
	Nullable<Int32> Green;
	Nullable<Int32> Blue;
	String Description;
	Nullable<Boolean> Active;
	Nullable<Boolean> IsComposite;
	Nullable<DateTime> CreatedDateTime;
	Nullable<DateTime> LastModifiedDateTime;



public class ImageFile
	Nullable<Format> GetFormat(String format);
	Format Default(Nullable<Format> outputFormat, Format defaultValue);



public class RenderResult:IEnumerable<Byte[]>, IEnumerable, ISerialInfo
	ISet<String> Warnings;
	Encoding Encoding;
	Int32 NbCopies;
	Nullable<Int32> NbSerials;
	Nullable<Int32> PauseCutValue;
	Int32 NbLabels;
	Boolean IsMultipleCopies;
	Boolean HasValue;
	Byte[] Item;
	void AddWarning(String warning);
	void AddWarnings(IEnumerable<String> warnings);
	void Set(Int32 labelNbr, Byte[] labelData);
	String GetAsString(Int32 labelNbr);
	IEnumerator<Byte[]> GetEnumerator();
	IEnumerator System.Collections.IEnumerable.GetEnumerator();



public class SerialInfo:ISerialInfo
	ISerialInfo EMPTY;
	Int32 NbCopies;
	Nullable<Int32> NbSerials;
	Nullable<Int32> PauseCutValue;
	Int32 NbLabels;
	Boolean IsMultipleCopies;
	Boolean HasValue;



public class SortBy:Enum, IComparable, IFormattable, IConvertible
	Int32 value__;
	SortBy Code;
	SortBy Description;
	SortBy ClassOrder;
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



public class ThreeArgs:IEquatable<ThreeArgs>
	Type CodeType;
	Type DescType;
	SortBy SortBy;
	Boolean Equals(Object obj);
	Boolean Equals(ThreeArgs other);
	Int32 GetHashCode();
	String ToString();



public class [nested] Family:Enum, IComparable, IFormattable, IConvertible
	Int32 value__;
	Family Helvetica;
	Family Courier;
	Family TimesRoman;
	Family Symbol;
	Family ZapfDingbats;
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



public class [nested] PdfPageSize:Enum, IComparable, IFormattable, IConvertible
	Int32 value__;
	PdfPageSize None;
	PdfPageSize Letter;
	PdfPageSize Legal;
	PdfPageSize A4;
	PdfPageSize A5;
	PdfPageSize A6;
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



public class [nested] PdfPageOrientation:Enum, IComparable, IFormattable, IConvertible
	Int32 value__;
	PdfPageOrientation Portrait;
	PdfPageOrientation Landscape;
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



public class [nested] PdfPageHAlign:Enum, IComparable, IFormattable, IConvertible
	Int32 value__;
	PdfPageHAlign Left;
	PdfPageHAlign Right;
	PdfPageHAlign Center;
	PdfPageHAlign Justify;
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



public class [nested] PdfPageVAlign:Enum, IComparable, IFormattable, IConvertible
	Int32 value__;
	PdfPageVAlign Top;
	PdfPageVAlign Bottom;
	PdfPageVAlign Center;
	PdfPageVAlign Justify;
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



public class [nested] MissingHttpStatus:Enum, IComparable, IFormattable, IConvertible
	Int32 value__;
	MissingHttpStatus Processing;
	MissingHttpStatus EarlyHints;
	MissingHttpStatus MultiStatus;
	MissingHttpStatus AlreadyReported;
	MissingHttpStatus IMUsed;
	MissingHttpStatus PermanentRedirect;
	MissingHttpStatus MisdirectedRequest;
	MissingHttpStatus UnprocessableEntity;
	MissingHttpStatus Locked;
	MissingHttpStatus FailedDependency;
	MissingHttpStatus PreconditionRequired;
	MissingHttpStatus TooManyRequests;
	MissingHttpStatus RequestHeaderFieldsTooLarge;
	MissingHttpStatus UnavailableForLegalReasons;
	MissingHttpStatus VariantAlsoNegotiates;
	MissingHttpStatus InsufficientStorage;
	MissingHttpStatus LoopDetected;
	MissingHttpStatus NotExtended;
	MissingHttpStatus NetworkAuthenticationRequired;
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



public class [nested] Cache:ICache<K,V>
	V GetOrAdd(K key, Func<K,V> valueFactory);



public class [nested] Event
	TraceEventType EventType;
	DateTime RaiseDateTime;
	String Message;
	String StackTrace;
	String Source;
	String AdditionalInfo;
	Exception TraceException;
	ValueTuple<Exception,String> AggregateException(Exception error, String stackTrace);
	LogEvent CreateLogEvent(LogEventLevel level, Exception exception, String message, String stackTrace);
	LogEvent CreateLogEvent(LogEventLevel level, Exception exception, MessageTemplate messageTemplate, IEnumerable<LogEventProperty> properties, String stackTrace);
	CultureInfo GetMessageFormattingCulture();
	TraceEventType GetTraceEventType(LogEventLevel level);
	String RenderMessage(LogEvent logEvent);
	String ToString();
	String ToString(Boolean withStackTrace);



public class [nested] ContentType
	String ZPL;
	String PDF;
	String PNG;



public class [nested] PrintDensity
	String DPMM_06;
	String DPMM_08;
	String DPMM_12;
	String DPMM_24;
	String DPMM_48;
	String INKJET_100;
	String INKJET_200;
	String INKJET_300;
	String INKJET_600;
	String INKJET_1200;
	String INKJET_2400;



public class [nested] PrintDensityType
	String DPMM;
	String DPI;



public class [nested] MediaCoatingType
	String MattePaper;
	String Synthetic;
	String GlossyPaper;
	String GlossyFilm;
	String HighGlossyPaper;
	String PlainPaper;
	String TexturePaper;
	String Wristband;



public class [nested] MediaForm
	String ContinuousPaper;
	String DieCutLabel;
	String ContinuousLabel;
	String Wristband;



public class [nested] MediaShape
	String RollPaper;
	String FanfoldPaper;



public class [nested] MediaSource
	String InternalRoll;
	String ExternalFeed;



public class [nested] EdgeDetection
	String BlackMark;
	String Gap;
	String None;



public class [nested] PrintMode
	String TearOff;
	String PeelOff;
	String Rewind;
	String Applicator;
	String Cutter;
	String DelayedCutter;
	String RFID;
	String Kiosk;



public class [nested] SizeUnit
	String IN;
	String CM;
	String MM;
	String DOT;
	String POINT;
	String PICA;
	String PIXEL;
	String PERC;



public class [nested] JustificationNoNone
	String Left;
	String Center;
	String Right;
	String Full;



public class [nested] Justification
	String None;
	String Left;
	String Center;
	String Right;
	String Full;



public class [nested] PrinterFileStatus
	String New;
	String Processed;



public class [nested] PrinterDrive
	String Dram;
	String InternalFlash;
	String FlashCard;
	String CompactFlash;



public class [nested] Rotation
	String Deg000;
	String Deg090;
	String Deg180;
	String Deg270;



public class [nested] Orientation
	String NORMAL;
	String TOP_BOTTOM;
	String RIGHT_TO_LEFT;
	String BOTTOM_UP;



public class [nested] Command
	String SetupPasteLine;
	String SetupResetOrder;



public class [nested] ModelType
	String Single;
	String Group;
	String Snippet;
	String PrinterSetup;
	String LabelZoom;



public class [nested] Classification
	String Test;
	String Demo;
	String Customer;
	String Industry;
	String Retailer;
	String Other;



public class [nested] AndOr
	String And;
	String Or;



public class [nested] BarcodeDimension
	String One;
	String Two;
	String Composite;
	String Postal;
	String Other;



public class [nested] ExprType
	String Hard;
	String Screen;
	String Function;
	String Content;
	String Image;
	String Iterator;
	String Same;
	String Script;



public class [nested] AggregateBy
	String PrinterID;
	String ModelID;
	String ScreenID;
	String UserID;
	String ContentType;
	String NbCopies;



public class [nested] AggregatePeriod
	String DayOfYear;
	String Week;
	String Month;
	String DayOfWeek;
	String Quarter;



public class [nested] RegexValidation
	String None;
	String Warning;
	String Error;



public class [nested] LayoutType
	String Dots;
	String Perc;



public class [nested] OnEventType
	String None;
	String Insert;
	String Update;
	String Both;



public class [nested] GraphicType
	String Vertical;
	String Horizontal;
	String Box;
	String Ellipse;
	String Circle;
	String Diagonal;



public class [nested] GraphicRounding
	String None;
	String One;
	String Two;
	String Three;
	String Four;
	String Five;
	String Six;
	String Seven;
	String Full;



public class [nested] SequenceType
	String StringSeq;
	String AsciiDec;



public class [nested] ALPrintNodeContentType
	String RAW_BASE64;
	String PDF_BASE64;



public class [nested] AnswerAttributeType
	String Text;
	String Decimal;
	String Integer;
	String Date;
	String Bool;
	String Selector;
	String DropDown;
	String Regex;



public class [nested] AnswerType
	String Value;
	String Calc;



public class [nested] SequenceUsage
	String None;
	String Human;
	String Barcode;
	String Both;



public class [nested] OnOtherDensity
	String PrintAsIs;
	String AdjustSize;
	String ToPdf;
	String ToPdfAdjustRatio;
	String ToPngPdf;
	String Fail;



public class [nested] HexEncoding
	String None;
	String Underscore;
	String Backslash;



public class [nested] SizeUnit
	String IN;
	String CM;
	String MM;
	String DOT;
	String POINT;
	String PICA;
	String PERC;
	String PIXEL;



public class [nested] PrintDensityType
	String DPMM;
	String DPI;



public class [nested] PrintDensity
	Int32 DPMM_06;
	Int32 DPMM_08;
	Int32 DPMM_12;
	Int32 DPMM_24;
	Int32 DPMM_48;
	Int32 INKJET_100;
	Int32 INKJET_200;
	Int32 INKJET_300;
	Int32 INKJET_600;
	Int32 INKJET_1200;
	Int32 INKJET_2400;



public class [nested] OnOtherDensity
	String PrintAsIs;
	String AdjustSize;
	String ToPdf;
	String ToPdfAdjustRatio;
	String ToPngPdf;
	String ToPngPdfAdjustRatio;
	String Fail;



public class [nested] ModelType
	String Single;
	String Group;
	String Snippet;
	String PrinterSetup;
	String LabelZoom;
	Boolean IsReal(String modelType);



public class [nested] BarcodeDimension
	String One;
	String Two;
	String Postal;
	String Composite;
	String Other;



public class [nested] ExprType
	String Hard;
	String Screen;
	String Function;
	String Content;
	String Image;
	String Iterator;
	String Same;
	String Script;



public class [nested] HexEncoding
	String None;
	String Underscore;
	String Backslash;
	Boolean HasEncoding(String hexEncoding);



public class [nested] GraphicType
	String Vertical;
	String Horizontal;
	String Box;
	String Circle;
	String Ellipse;
	String Diagonal;



public class [nested] Colors
	IColor BLACK;
	IColor WHITE;
	IColor TRANSPARENT;
	IColor RED;
	IColor LIME;
	IColor BLUE;
	IColor YELLOW;
	IColor MAGENTA;
	IColor FUCHSIA;
	IColor CYAN;
	IColor AQUA;



public class [nested] Substitutions



public class [nested] Labelary



public class [nested] SequenceUsage
	String None;
	String Human;
	String Barcode;
	String Both;
	Boolean IsEmpty(String seqUsage);



public class [nested] AndOr
	String And;
	String Or;
	String GetBooleanOperator(String operation);



public class [nested] AggregateBy
	String PrinterID;
	String ModelID;
	String ScreenID;
	String UserID;
	String ContentType;
	String NbCopies;



public class [nested] AggregatePeriod
	String DayOfYear;
	String Week;
	String Month;
	String DayOfWeek;
	String Quarter;



public class [nested] JustificationVal
	String None;
	String Left;
	String Center;
	String Right;
	String Full;



public class [nested] RegexValidation
	String None;
	String Warning;
	String Error;



public class [nested] PrintMode
	String TearOff;
	String PeelOff;
	String Rewind;
	String Applicator;
	String Cutter;
	String DelayedCutter;
	String RFID;
	String ReservedL;
	String ReservedU;
	String Kiosk;



public class [nested] MediaShape
	String RollPaper;
	String FanfoldPaper;



public class [nested] MediaForm
	String ContinuousPaper;
	String DieCutLabel;
	String ContinuousLabel;
	String Wristband;



public class [nested] MediaCoatingType
	String MattePaper;
	String Synthetic;
	String GlossyPaper;
	String GlossyFilm;
	String HighGlossyPaper;
	String PlainPaper;
	String TexturePaper;
	String Wristband;



public class [nested] MediaSource
	String InternalRoll;
	String ExternalFeed;



public class [nested] EdgeDetection
	String BlackMark;
	String Gap;
	String None;



public class [nested] SequenceType
	String StringSeq;
	String AsciiDec;



public class [nested] Rotation
	String Deg000;
	String Deg090;
	String Deg180;
	String Deg270;
	Boolean HasRotation(String rotation);
	Int32 GetRotationDegrees(String rotation);
	Boolean IsXYFlipped(String rotation);
	void HandleRotation(String rotation, Nullable`1& h, Nullable`1& w);
	IFormat Unrotate(IFormat format);
	void Swap(Nullable`1& h, Nullable`1& w);



public class [nested] ZplEncoding:Enum, IComparable, IFormattable, IConvertible
	Int32 value__;
	ZplEncoding USA_1;
	ZplEncoding USA_2;
	ZplEncoding UK_2;
	ZplEncoding Holland;
	ZplEncoding Denmark_Norway;
	ZplEncoding Sweden_Finland;
	ZplEncoding Germany;
	ZplEncoding France_1;
	ZplEncoding France_2;
	ZplEncoding Italy;
	ZplEncoding Spain;
	ZplEncoding Miscellaneous;
	ZplEncoding Japan;
	ZplEncoding Page_850;
	ZplEncoding Double_Byte_Asian;
	ZplEncoding Shift_JIS;
	ZplEncoding EUC_JP;
	ZplEncoding UCS_2;
	ZplEncoding Single_Byte_Asian;
	ZplEncoding Multibyte_Asian;
	ZplEncoding Page_1252;
	ZplEncoding Unicode_UTF8;
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



public class [nested] Format:Enum, IComparable, IFormattable, IConvertible
	Int32 value__;
	Format unknown;
	Format bmp;
	Format jpg;
	Format gif;
	Format tiff;
	Format png;
	Format pdf;
	Format zpl;
	Format b64;
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



public class [nested] ArgNames
	String CURRENT_ROW;
	String CURRENT_ROWS;
	String VIEW_NAME;
	String FIELD_NAME;
	String CURRENT_FIELD_VALUE;
	String ITERATOR_NAME;
	String FILL_ME;



public class [nested] PrintQuality
	String Grayscale;
	String Bitonal;