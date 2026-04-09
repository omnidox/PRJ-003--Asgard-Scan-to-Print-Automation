public class DitheredImageProvider
	Int32 ConvertByteToGrayscale(Int32 pixelToConvert);
	void GetDitheredImage(ZebraImageInternal image, Stream outputStream);
	void GetDitheredImage(Int32 width, Int32 height, ZebraImageInternal image, Stream outputStream);



public class FileUtilities
	PrinterFilePath ParseDriveAndExtension(String printerDriveAndFileName);
	String TruncateAndReplaceSpaces(String fileName);
	String ChangeExtension(String filePath, String newExtension);
	String GetFileNameOnPrinter(String filePath);
	String GetFileNameOnPrinterLinkOS(String filePath);



public class PrinterFilePath
	String Drive;
	String Extension;
	String FileName;
	String ToString();



public class StringUtilities
	String CRLF;
	String LF;
	Int32 IndexOf(String inputString, String[] searchPatterns, Int32 start);
	String[] Split(String input, String delimiter);
	String Join(String[] strings, String delimiter);
	Int32 CountSubstringOccurences(String stringToSearch, String substring);
	String StripQuotes(String str);
	String PadWithChar(String initialString, Char padding, Int32 lengthWithPadding, Boolean padInFront);
	Boolean DoesPrefixExistInArray(String[] prefixes, String value);
	String ByteArrayToHexString(Byte[] byteArray);
	Byte[] HexToByteArray(String hexString);
	List<String> ToList(String[] array);
	List<String> ToList(String[][] twoDimensionalStringArray);
	String StringPadToPlaces(Int32 howManyPlaces, String filler, String stringToPad, Boolean padOnEnd);
	String StringPadToPlaces(Int32 howManyPlaces, Char filler, String stringToPad, Boolean padOnEnd);
	Byte[] ByteArrayPadToPlaces(Int32 howManyPlaces, Byte[] byteArrayToPad);
	String ConvertTo8dot3(String fileNameOnPrinter);
	String ConvertTo16dot3(String fileNameOnPrinter);
	String ConvertToNdot3(String fileNameOnPrinter, Int32 maxFileNameLength);
	String ConvertToXdot3(String fileNameOnPrinter, Int32 maxFileNameLength);
	String StringPadToPlaces(Int32 howManyPlaces, String filler, String stringToPad);
	Int64 StringToLong(String stringWithLeadingIntegers);
	Dictionary<String,String> ConvertKeyValueJsonToMap(Byte[] jsonUtf8Bytes);
	Dictionary<String,String> ConvertKeyValueJsonToMap(String jsonString);
	String Repeat(String s, Int32 num);
	String GetStringValueForKey(Dictionary<String,String> map, String key);
	Int32 GetIntValueForKey(Dictionary<String,String> map, String key);
	Double ConvertStringToDouble(String value);
	String ConvertDoubleToString(Double value);



public class ZebraImage:ZebraImageInternal, ZebraImageI, IDisposable
	Int32 Height;
	Int32 Width;
	Int32[] GetRow(Int32 rowIndex);
	void GetRGB(Int32 startX, Int32 startY, Int32 width, Int32 height, Int32[] rgbArray, Int32 offset, Int32 scanSize);
	Boolean ScaleImage(Int32 width, Int32 height);
	Byte[] GetDitheredB64EncodedPng();
	void WriteDitheredPng(Stream destinationStream);
	void RemovePixelPaddingFromRaster(Int32 width, Int32 height, Stream ditheredBytes, Image bufferedImage, Int32 rowWidthInBytes);
	void Dispose();



public class ZebraImageI:IDisposable
	Int32 Height;
	Int32 Width;



public class ZebraImageInternal:ZebraImageI, IDisposable
	Byte[] GetDitheredB64EncodedPng();
	Int32[] GetRow(Int32 row);
	Boolean ScaleImage(Int32 width, Int32 height);
	void WriteDitheredPng(Stream destinationStream);



public class ZPLUtilities
	Byte ZPL_INTERNAL_FORMAT_PREFIX_CHAR;
	Byte ZPL_INTERNAL_COMMAND_PREFIX_CHAR;
	Byte ZPL_INTERNAL_DELIMITER_CHAR;
	String ZPL_INTERNAL_FORMAT_PREFIX;
	String ZPL_INTERNAL_COMMAND_PREFIX;
	String ZPL_INTERNAL_DELIMITER;
	String PRINTER_INFO;
	String PRINTER_STATUS;
	String PRINTER_CONFIG_LABEL;
	String PRINTER_DIRECTORY_LABEL;
	String PRINTER_NETWORK_CONFIG_LABEL;
	String PRINTER_CALIBRATE;
	String PRINTER_RESET;
	String PRINTER_RESET_NETWORK;
	String PRINTER_RESTORE_DEFAULTS;
	String PRINTER_GET_SUPER_HOST_STATUS;
	String PRINTER_GET_STORAGE_INFO_COMMAND;
	String FILE_DRIVE_INFO_SETTING_NAME;
	String FILE_DRIVE_LISTING_SETTING_NAME;
	String DecorateWithCommandPrefix(String command);
	String DecorateWithFormatPrefix(String format);
	String ReplaceAllWithInternalDelimeter(String format);
	String ReplaceAllWithInternalCharacters(String format);
	String ReplaceInternalCharactersWithReadableCharacters(String zpl);
	Byte[] ReplaceInternalCharactersWithReadableCharacters(Byte[] zplBytes);
	void ReplaceInternalCharactersWithReadableCharacters(Stream destination, Stream zplBytes);
	Int32 GetDpmm(String tildeHiResponse);
	String[] FilterFileList(String[] fileList, String filter);
	String CreateFileNameRegex(Match matcher);
	String GetDYPrefix(Char driveLetter, String fileName, Char format, String extension, Int32 numberOfBytesInFile, String bytesPerRow);
	Boolean IsValidZplFirmware(String fwVersion);



public class PdfTransformer:AbstractTransformer, ITransformer, ISelectable
	ITransformer TO_PCL;
	ITransformer TO_XPS;
	ITransformer TO_DOC;
	ITransformer TO_DOCX;
	ITransformer TO_HTML;
	ITransformer TO_SVG;
	ITransformer TO_XLSX;
	ITransformer TO_POSTSCRIPT;
	ITransformer TO_OFD;
	ITransformer TO_PPTX;
	ITransformer TO_PNG;
	ITransformer TO_JPEG;
	ITransformer TO_BMP;
	String Code;
	String Description;
	FileResult Transform(ILabelContext lc, Object from);
	void CheckName(FileResult result);
	Image GetImage(Byte[] bytes);



public class PngToPdf:ITransformer, ISelectable
	ITransformer INSTANCE;
	String Code;
	String Description;
	FileResult Transform(ILabelContext lc, Object from);



public class ZplToPdf:AbstractTransformer, ITransformer, ISelectable
	ITransformer INSTANCE;
	String Code;
	String Description;
	FileResult Transform(ILabelContext lc, Object from);
	void CheckName(FileResult result);
	Image GetImage(Byte[] bytes);



public class label
	Decimal version;
	String name;
	String description;
	DateTime created;
	DateTime modified;
	String author;
	String layoutFile;
	String variablesFile;
	String settingsFile;



public class layout
	layoutObject[] object;



public class settings
	settingsPrinter printer;
	settingsLabel label;
	settingsPrint print;



public class settingsPrinter
	String name;
	UInt16 dpi;



public class settingsLabel
	Byte width;
	Byte height;
	String unit;
	String orientation;



public class settingsPrint
	Byte darkness;
	Byte speed;



public class variables
	variablesVariable[] variable;



public class variablesVariable
	String name;
	String type;
	String default;
	String prompt;
	String id;



public class layout
	layoutLabel label;
	Decimal version;



public class layoutLabel
	layoutLabelBackground background;
	layoutLabelObject[] object;
	Byte width;
	Byte height;
	String unit;
	String orientation;



public class layoutLabelBackground
	String color;



public class layoutLabelObject
	layoutLabelObjectPosition position;
	layoutLabelObjectSize size;
	layoutLabelObjectShape shape;
	String source;
	layoutLabelObjectBarcode barcode;
	layoutLabelObjectFont font;
	String color;
	String data;
	String id;
	String type;



public class layoutLabelObjectPosition
	Byte x;
	Byte y;



public class layoutLabelObjectSize
	Byte width;
	Byte height;



public class layoutLabelObjectShape
	String type;
	String borderColor;
	Byte borderWidth;



public class layoutLabelObjectBarcode
	String symbology;
	Boolean checkdigit;
	Boolean humanReadable;



public class layoutLabelObjectFont
	String family;
	Byte size;
	Boolean bold;
	Boolean italic;
	Boolean underline;



public class settings
	settingsPrinter printer;
	settingsLabel label;
	String encoding;



public class settingsPrinter
	String name;
	UInt16 dpi;
	Byte darkness;
	Byte speed;



public class settingsLabel
	settingsLabelWidth width;
	settingsLabelHeight height;
	settingsLabelGap gap;
	String orientation;



public class settingsLabelWidth
	String unit;
	Byte Value;



public class settingsLabelHeight
	String unit;
	Byte Value;



public class settingsLabelGap
	String unit;
	Byte Value;



public class template
	templateMetadata metadata;
	String layoutRef;
	String variablesRef;
	String settingsRef;
	Decimal version;



public class templateMetadata
	String author;
	DateTime created;
	DateTime modified;
	Decimal version;
	String description;



public class variables
	variablesVariable[] variable;



public class variablesVariable
	String name;
	String type;
	String format;
	variablesVariableLength length;
	String default;
	String prompt;
	Boolean required;
	Boolean requiredSpecified;
	String id;



public class variablesVariableLength
	Byte min;
	Byte max;



public class AbstractTransformer:ITransformer, ISelectable
	String Code;
	String Description;
	FileResult Transform(ILabelContext lc, Object from);
	void CheckName(FileResult result);
	Image GetImage(Byte[] bytes);



public class ImageRotation:AbstractTransformer, ITransformer, ISelectable
	String Code;
	String Description;
	FileResult Transform(ILabelContext lc, Object from);
	FileResult RotatePrintResult(String rotationOrOrientation, Object from);
	Image Rotate(String rotationOrOrientation, Object from);
	ValueTuple<FileResult,Image> DoRotate(String rotationOrOrientation, Object from);
	void CheckName(FileResult result);
	Image GetImage(Byte[] bytes);



public class ITransformer:ISelectable
	FileResult Transform(ILabelContext lc, Object from);



public class PdfMerger:ITransformer, ISelectable
	String Code;
	String Description;
	FileResult Transform(ILabelContext lc, Object toMerge);



public class PdfRotator:AbstractTransformer, ITransformer, ISelectable
	String Code;
	String Description;
	FileResult Transform(ILabelContext lc, Object from);
	void CheckName(FileResult result);
	Image GetImage(Byte[] bytes);



public class PngToZpl:AbstractTransformer, ITransformer, ISelectable
	ITransformer INSTANCE;
	String Code;
	String Description;
	FileResult Transform(ILabelContext lc, Object from);
	void CheckName(FileResult result);
	Image GetImage(Byte[] bytes);



public class ZplToGraphicToZpl:AbstractTransformer, ITransformer, ISelectable
	ITransformer INSTANCE;
	String Code;
	String Description;
	FileResult Transform(ILabelContext labelContext, Object from);
	void CheckName(FileResult result);
	Image GetImage(Byte[] bytes);



public class ZplToPng:AbstractTransformer, ITransformer, ISelectable
	ITransformer INSTANCE;
	String Code;
	String Description;
	FileResult Transform(ILabelContext lc, Object from);
	void CheckName(FileResult result);
	Image GetImage(Byte[] bytes);



public class ZplToSbpl:AbstractTransformer, ITransformer, ISelectable
	ITransformer INSTANCE;
	String Code;
	String Description;
	FileResult Transform(ILabelContext lc, Object from);
	void CheckName(FileResult result);
	Image GetImage(Byte[] bytes);



public class Constants



public class DBOperationResults
	ILabelContext LabelContext;
	Boolean ShowDetails;
	Boolean ShowZero;
	Func<IEnumerable`1,ILabelContext,String> ShowDetailsFunc;
	void AddRange(DBOperationResults results);
	void AddInsert(Object item);
	void AddUpdate(Object item);
	void AddDelete(Object item);
	void AddSkip(Object item);
	String ToString();
	void ShowCount(StringBuilder strBuilder, String prefix, IEnumerable<Object> details);
	void DoShowDetails(StringBuilder strBuilder, IEnumerable<Object> details);
	String Stringify(IEnumerable<Object> details, ILabelContext context);



public class FileMetadata
	String FullName;
	String ContentType;
	String FileServiceRef;
	Nullable<DateTime> RevisionDate;
	Nullable<Int32> RevisionId;
	Nullable<Int32> Size;
	String FileID;
	String Comment;
	FM ToSave(IFileInfo fi, Object fileRef);
	String GetContentType(IFileInfo file);



public class GraphicFromCoordinate:AbstractCoordinate, ICoordinate, IEqualityComparer<ICoordinate>
	Nullable<Decimal> PosX;
	Nullable<Decimal> PosY;
	Boolean Equals(ICoordinate other);
	Int32 GetHashCode();
	Boolean Equals(ICoordinate x, ICoordinate y);
	Int32 GetHashCode(ICoordinate obj);



public class GraphicToCoordinate:AbstractCoordinate, ICoordinate, IEqualityComparer<ICoordinate>
	Nullable<Decimal> PosX;
	Nullable<Decimal> PosY;
	Boolean Equals(ICoordinate other);
	Int32 GetHashCode();
	Boolean Equals(ICoordinate x, ICoordinate y);
	Int32 GetHashCode(ICoordinate obj);



public class GuidGuid:IComparable, IComparable<GuidGuid>, IEquatable<GuidGuid>, IFormattable
	Nullable<Guid> Guid1;
	Nullable<Guid> Guid2;
	Int32 CompareTo(Object value);
	Int32 CompareTo(GuidGuid value);
	Boolean Equals(Object obj);
	Boolean Equals(GuidGuid other);
	Int32 GetHashCode();
	String ToString();
	String ToString(String format, IFormatProvider formatProvider);



public class GuidLine:IComparable, IComparable<GuidLine>, IEquatable<GuidLine>, IFormattable
	Nullable<Guid> ID;
	Nullable<Int32> Line;
	Int32 CompareTo(Object value);
	Int32 CompareTo(GuidLine value);
	Boolean Equals(Object obj);
	Boolean Equals(GuidLine other);
	Int32 GetHashCode();
	String ToString();
	String ToString(String format);
	String ToString(String format, IFormatProvider formatProvider);



public class GuidString:IComparable, IComparable<GuidString>, IEquatable<GuidString>, IFormattable
	Nullable<Guid> ID;
	String Code;
	Int32 CompareTo(Object value);
	Int32 CompareTo(GuidString value);
	Boolean Equals(Object obj);
	Boolean Equals(GuidString other);
	Int32 GetHashCode();
	String ToString();
	String ToString(String format, IFormatProvider formatProvider);



public class PrintResults:List<PrintResult>, IList<PrintResult>, ICollection<PrintResult>, IEnumerable<PrintResult>, IEnumerable, IList, ICollection, IReadOnlyList<PrintResult>, IReadOnlyCollection<PrintResult>
	PrintResult EMPTY;
	Int32 NbLabels;
	Int32 NbPrinters;
	Int32 Capacity;
	Int32 Count;
	PrintResult Item;
	void Add(PrintResult item);
	PrintResults GetSummary();
	void Add(PrintResult item);
	Int32 System.Collections.IList.Add(Object item);
	void AddRange(IEnumerable<PrintResult> collection);
	ReadOnlyCollection<PrintResult> AsReadOnly();
	Int32 BinarySearch(Int32 index, Int32 count, PrintResult item, IComparer<PrintResult> comparer);
	Int32 BinarySearch(PrintResult item);
	Int32 BinarySearch(PrintResult item, IComparer<PrintResult> comparer);
	void Clear();
	Boolean Contains(PrintResult item);
	Boolean System.Collections.IList.Contains(Object item);
	void CopyTo(PrintResult[] array);
	void System.Collections.ICollection.CopyTo(Array array, Int32 arrayIndex);
	void CopyTo(Int32 index, PrintResult[] array, Int32 arrayIndex, Int32 count);
	void CopyTo(PrintResult[] array, Int32 arrayIndex);
	Boolean Exists(Predicate<PrintResult> match);
	PrintResult Find(Predicate<PrintResult> match);
	List<PrintResult> FindAll(Predicate<PrintResult> match);
	Int32 FindIndex(Predicate<PrintResult> match);
	Int32 FindIndex(Int32 startIndex, Predicate<PrintResult> match);
	Int32 FindIndex(Int32 startIndex, Int32 count, Predicate<PrintResult> match);
	PrintResult FindLast(Predicate<PrintResult> match);
	Int32 FindLastIndex(Predicate<PrintResult> match);
	Int32 FindLastIndex(Int32 startIndex, Predicate<PrintResult> match);
	Int32 FindLastIndex(Int32 startIndex, Int32 count, Predicate<PrintResult> match);
	void ForEach(Action<PrintResult> action);
	Enumerator<PrintResult> GetEnumerator();
	IEnumerator<PrintResult> System.Collections.Generic.IEnumerable<T>.GetEnumerator();
	IEnumerator System.Collections.IEnumerable.GetEnumerator();
	List<PrintResult> GetRange(Int32 index, Int32 count);
	Int32 IndexOf(PrintResult item);
	Int32 System.Collections.IList.IndexOf(Object item);
	Int32 IndexOf(PrintResult item, Int32 index);
	Int32 IndexOf(PrintResult item, Int32 index, Int32 count);
	void Insert(Int32 index, PrintResult item);
	void System.Collections.IList.Insert(Int32 index, Object item);
	void InsertRange(Int32 index, IEnumerable<PrintResult> collection);
	Int32 LastIndexOf(PrintResult item);
	Int32 LastIndexOf(PrintResult item, Int32 index);
	Int32 LastIndexOf(PrintResult item, Int32 index, Int32 count);
	Boolean Remove(PrintResult item);
	void System.Collections.IList.Remove(Object item);
	Int32 RemoveAll(Predicate<PrintResult> match);
	void RemoveAt(Int32 index);
	void RemoveRange(Int32 index, Int32 count);
	void Reverse();
	void Reverse(Int32 index, Int32 count);
	void Sort();
	void Sort(IComparer<PrintResult> comparer);
	void Sort(Int32 index, Int32 count, IComparer<PrintResult> comparer);
	void Sort(Comparison<PrintResult> comparison);
	PrintResult[] ToArray();
	void TrimExcess();
	Boolean TrueForAll(Predicate<PrintResult> match);
	IList<PrintResult> Synchronized(List<PrintResult> list);
	List<TOutput> ConvertAll(Converter<PrintResult,TOutput> converter);



public class AbstractCmd:IPrinterCmd<L,O>, IPrinterCmd
	IDictionary<String,IBarcodeCmd`2> BARCODES;
	String Language;
	String ArgDelimiter;
	String StartCommand;
	String EndCommand;
	String Raw;
	String Code;
	String Description;
	ICmdOption`2[] Options;
	Int32 NbOptions;
	ICmdOption<L,O> GetOption(String optionCode);
	String ToString();
	String Render(ILabelContext lc, Object[] values);
	String ToString(ICmdOption`2[] options);
	Object AsString(ICmdOption<L,O> opt);
	String ClearDelimiters(String rendered);
	void AddBarcode(IBarcodeCmd<L,O> barcode);
	Boolean TryGetBarcode(String barcodeType, IBarcodeCmd`2& barcode);
	IEnumerable<IBarcodeCmd`2> GetBarcodes();
	T ConvertConstraint(ICmdConstraint<L1,O1> constraint);



public class AbstractConstraint:ICmdConstraint<L,O>, ICmdConstraint
	Object DefaultValue;
	CmdConstraintType ConstraintType;
	Exception CheckValid(IPrinterCmd<L,O> barcode, ICmdOption<L,O> option, Object newValue);
	Object Clean(Object oldValue);
	C WithDefault(Object defaultValue);



public class AbstractLanguage:IPrinterLanguage<L,O>, IPrinterLanguage, ISelectable, IRenderer
	Boolean AllowsColors;
	String LineWrapValue;
	String StartLabel;
	String EndLabel;
	String StartCommand;
	String EndCommand;
	String ArgDelimiter;
	String Code;
	String Description;
	Boolean TryGetBarcode(String barcodeType, IBarcodeCmd`2& bc);
	IList<IPrinterCmd> GetCommands();
	void AddCommand(IPrinterCmd cmd);
	String SetNbCopies(ILabelContext lc, String text, Int32 totalQty, Nullable<Int32> pauseCutValue, Nullable<Int32> replicateEachSerial, Nullable<Boolean> overridePauseCount);
	String AddPause(ILabelContext lc, String text);
	Int32 FindAfter(String text, IPrinterCmd cmd);
	Int32 FindBefore(String text, IPrinterCmd cmd);
	String HandleDensity(ILabelContext lc, String text);
	String HandleFonts(ILabelContext lc, String text);
	void AddFontsForRendering(ILabelContext lc);
	ISerialInfo GetSerialInfo(ILabelContext lc, String text);
	RenderResult RenderAsOutput(ILabelContext lc);
	Boolean SupportsRendering(ILabelContext lc, ContentFormat from, ContentFormat to);
	String HandleScript(ILabelContext lc, IExpr expr);
	String HandleIterator(ILabelContext lc, IExprRow exprRow, ILabelElement dataElement);
	String HandleHard(String exprValue);
	String HandleScreen(ILabelContext lc, IExpr expr);
	String HandleFunction(ILabelContext lc, ILabelElement dataElement, IExpr expr);
	String HandleImage(ILabelContext lc, ILabelElement dataElement, IImageDriven imageDriven, IOrientable orientable);
	String HandleContent(ILabelContext lc, IContentable contentable);
	String HandleSubstitution(ILabelContext lc, ISubstitutable substitutable, String exprValue);
	ValueTuple<String,String> GetExpression(ILabelContext lc, IExprRow exprRow);
	ValueTuple<String,String> HandleRule(ILabelContext lc, IModelDetail modelExpr);
	String HandleBarcode(ILabelContext lc, IModelDetail modelExpr, IBarcodeable barcodable, String exprValue);
	ValueTuple<String,String> HandleHexEncoding(ILabelContext lc, String exprValue, IModelDetail modelExpr, String fieldDataCmd, String fieldSepCmd);
	String HandleGoto(ILabelContext lc, String exprType, IModelDetail modelExpr);
	String HandleColor(ILabelContext lc, IModelDetail modelExpr);
	String HandleFont(ILabelContext lc, IModelDetail modelExpr);
	String HandleJustification(ILabelContext lc, IModelDetail modelExpr);
	String HandleFieldReverse(ILabelContext lc, IModelDetail modelExpr);
	String HandleOrientation(ILabelContext lc, IModelDetail modelExpr, Boolean v);
	ValueTuple<String,String> HandleFieldData(ILabelContext lc, String exprValue);
	String HandleLineWrap(ILabelContext lc, String exprValue, IModelDetail modelExpr);
	String DrawGraphic(ILabelContext lc, IModelGraphic graphic);
	FileResult GetPrintResult(ILabelContext labelContext, Int32 nbCopies);
	String Validate(ILabelContext lc, String text);



public class AbstractOption:ICmdOption<L,O>, ICmdOption
	String Code;
	String Description;
	ICmdConstraint<L,O> Constraint;
	C WithDefault(Object defaultValue);
	String ToString();



public class AbstractRangeConstraint:AbstractConstraint<L,O>, ICmdConstraint<L,O>, ICmdConstraint
	Object DefaultValue;
	CmdConstraintType ConstraintType;
	Object Clean(Object newValue);
	Exception CheckValid(IPrinterCmd<L,O> barcode, ICmdOption<L,O> option, Object newValue);
	Boolean IsIncrByOne();
	Boolean DoCheckValid(Object newValue);
	String ValuesToString();
	String ToString();
	C WithDefault(Object defaultValue);



public class LanguageFactory:ILanguageFactory
	IPrinterLanguage GetLanguage(String language);
	IPrinterLanguage GetLanguageInternal(String language);



public class LengthConstraint:ICmdConstraint<L,O>, ICmdConstraint
	Object DefaultValue;
	CmdConstraintType ConstraintType;
	LengthConstraint WithMin(Int32 min);
	LengthConstraint WithMax(Int32 max);
	LengthConstraint WithMinMax(Int32 min, Int32 max);
	Object Clean(Object oldValue);
	Exception CheckValid(IPrinterCmd<L,O> barcode, ICmdOption<L,O> option, Object oldValue);
	String ToString();



public class RegexConstraint:ICmdConstraint<L,O>, ICmdConstraint
	Object DefaultValue;
	CmdConstraintType ConstraintType;
	Object Clean(Object oldValue);
	Exception CheckValid(IPrinterCmd<L,O> barcode, ICmdOption<L,O> option, Object oldValue);
	String ToString();



public class ZplAssignFontCmd:ZplCmd, IPrinterCmd<ZplLanguage,ZplOption>, IPrinterCmd
	String Language;
	String ArgDelimiter;
	String StartCommand;
	String EndCommand;
	String Raw;
	String Code;
	String Description;
	ICmdOption`2[] Options;
	Int32 NbOptions;
	String GetCommand(ILabelContext lc, String fontCode, String objectName, Boolean standAlone);
	String WrapStartEnd(String[] cmds);
	ICmdOption<ZplLanguage,ZplOption> GetOption(String optionCode);
	String ToString();
	String Render(ILabelContext lc, Object[] values);
	String ToString(ICmdOption`2[] options);
	String ClearDelimiters(String rendered);
	void AddBarcode(IBarcodeCmd<ZplLanguage,ZplOption> barcode);
	Boolean TryGetBarcode(String barcodeType, IBarcodeCmd`2& barcode);
	IEnumerable<IBarcodeCmd`2> GetBarcodes();
	T ConvertConstraint(ICmdConstraint<L1,O1> constraint);



public class ZplBarcodeCmd:ZplCmd, IPrinterCmd<ZplLanguage,ZplOption>, IPrinterCmd, IBarcodeCmd<ZplLanguage,ZplOption>, ILanguageDriven
	String PREFIX;
	String BarcodeType;
	String Name;
	String Dimension;
	String Code;
	String Language;
	String ArgDelimiter;
	String StartCommand;
	String EndCommand;
	String Raw;
	String Description;
	ICmdOption`2[] Options;
	Int32 NbOptions;
	String WrapStartEnd(String[] cmds);
	ICmdOption<ZplLanguage,ZplOption> GetOption(String optionCode);
	String ToString();
	String Render(ILabelContext lc, Object[] values);
	String ToString(ICmdOption`2[] options);
	String ClearDelimiters(String rendered);
	void AddBarcode(IBarcodeCmd<ZplLanguage,ZplOption> barcode);
	Boolean TryGetBarcode(String barcodeType, IBarcodeCmd`2& barcode);
	IEnumerable<IBarcodeCmd`2> GetBarcodes();
	T ConvertConstraint(ICmdConstraint<L1,O1> constraint);



public class ZplChangeEncodingCmd:ZplCmd, IPrinterCmd<ZplLanguage,ZplOption>, IPrinterCmd
	String Language;
	String ArgDelimiter;
	String StartCommand;
	String EndCommand;
	String Raw;
	String Code;
	String Description;
	ICmdOption`2[] Options;
	Int32 NbOptions;
	String Command(ILabelContext lc, ZplEncoding encoding);
	String WrapStartEnd(String[] cmds);
	ICmdOption<ZplLanguage,ZplOption> GetOption(String optionCode);
	String ToString();
	String Render(ILabelContext lc, Object[] values);
	String ToString(ICmdOption`2[] options);
	String ClearDelimiters(String rendered);
	void AddBarcode(IBarcodeCmd<ZplLanguage,ZplOption> barcode);
	Boolean TryGetBarcode(String barcodeType, IBarcodeCmd`2& barcode);
	IEnumerable<IBarcodeCmd`2> GetBarcodes();
	T ConvertConstraint(ICmdConstraint<L1,O1> constraint);



public class ZplDeleteObjectCmd:ZplCmd, IPrinterCmd<ZplLanguage,ZplOption>, IPrinterCmd
	String Language;
	String ArgDelimiter;
	String StartCommand;
	String EndCommand;
	String Raw;
	String Code;
	String Description;
	ICmdOption`2[] Options;
	Int32 NbOptions;
	String GetCommand(ILabelContext lc, IPrinterFile printerFile);
	String GetCommand(ILabelContext lc, String objectName);
	String WrapStartEnd(String[] cmds);
	ICmdOption<ZplLanguage,ZplOption> GetOption(String optionCode);
	String ToString();
	String Render(ILabelContext lc, Object[] values);
	String ToString(ICmdOption`2[] options);
	String ClearDelimiters(String rendered);
	void AddBarcode(IBarcodeCmd<ZplLanguage,ZplOption> barcode);
	Boolean TryGetBarcode(String barcodeType, IBarcodeCmd`2& barcode);
	IEnumerable<IBarcodeCmd`2> GetBarcodes();
	T ConvertConstraint(ICmdConstraint<L1,O1> constraint);



public class ZplDownloadFontCommands



public class ZplDownloadGraphicCmd:ZplTildeCmd, IPrinterCmd<ZplLanguage,ZplOption>, IPrinterCmd
	String StartCommand;
	String EndCommand;
	String Language;
	String ArgDelimiter;
	String Raw;
	String Code;
	String Description;
	ICmdOption`2[] Options;
	Int32 NbOptions;
	String Command(ILabelContext lc, IPrinterFile printerFile, Byte[] data);
	String ClearDelimiters(String rendered);
	String WrapStartEnd(String[] cmds);
	ICmdOption<ZplLanguage,ZplOption> GetOption(String optionCode);
	String ToString();
	String Render(ILabelContext lc, Object[] values);
	String ToString(ICmdOption`2[] options);
	void AddBarcode(IBarcodeCmd<ZplLanguage,ZplOption> barcode);
	Boolean TryGetBarcode(String barcodeType, IBarcodeCmd`2& barcode);
	IEnumerable<IBarcodeCmd`2> GetBarcodes();
	T ConvertConstraint(ICmdConstraint<L1,O1> constraint);



public class ZplDownloadObjectBinCmd
	Byte ZPL_INTERNAL_COMMAND_PREFIX_CHAR;
	Byte ZPL_INTERNAL_DELIMITER_CHAR;
	Byte[] GetCmdBytesForFont(Byte[] binData, String pathOnPrinter, Char downloadFormatCode, String fileExtensionCode);
	Byte[] GetCmdBytesForPng(Byte[] binData, String pathOnPrinter, Char downloadFormatCode, String fileExtensionCode);
	String GetCorrectedFileName(PrinterFilePath parsedPath);



public class ZplDownloadObjectCmd:ZplTildeCmd, IPrinterCmd<ZplLanguage,ZplOption>, IPrinterCmd
	String StartCommand;
	String EndCommand;
	String Language;
	String ArgDelimiter;
	String Raw;
	String Code;
	String Description;
	ICmdOption`2[] Options;
	Int32 NbOptions;
	String TrueOrOpenTypeFont(ILabelPrinter printer, IPrinterFile printerFile, Byte[] ttfOtfData, Boolean standAlone, Boolean zipFirst);
	String UncompressedPngAscii(ILabelPrinter printer, IPrinterFile printerFile, Byte[] data, Boolean zipFirst);
	String UncompressedPngB64(ILabelPrinter printer, IPrinterFile printerFile, Byte[] data, Boolean zipFirst);
	String ClearDelimiters(String rendered);
	String WrapStartEnd(String[] cmds);
	ICmdOption<ZplLanguage,ZplOption> GetOption(String optionCode);
	String ToString();
	String Render(ILabelContext lc, Object[] values);
	String ToString(ICmdOption`2[] options);
	void AddBarcode(IBarcodeCmd<ZplLanguage,ZplOption> barcode);
	Boolean TryGetBarcode(String barcodeType, IBarcodeCmd`2& barcode);
	IEnumerable<IBarcodeCmd`2> GetBarcodes();
	T ConvertConstraint(ICmdConstraint<L1,O1> constraint);



public class ZplFieldBlockCmd:ZplCmd, IPrinterCmd<ZplLanguage,ZplOption>, IPrinterCmd
	String Language;
	String ArgDelimiter;
	String StartCommand;
	String EndCommand;
	String Raw;
	String Code;
	String Description;
	ICmdOption`2[] Options;
	Int32 NbOptions;
	String Justify(ILabelContext lc, Int32 widthOfTextBlockDots, Int32 maxNbLines, Int32 addOrRemoveDotsBetweenLines, String justification, Int32 hangingIndentDots);
	String WrapStartEnd(String[] cmds);
	ICmdOption<ZplLanguage,ZplOption> GetOption(String optionCode);
	String ToString();
	String Render(ILabelContext lc, Object[] values);
	String ToString(ICmdOption`2[] options);
	String ClearDelimiters(String rendered);
	void AddBarcode(IBarcodeCmd<ZplLanguage,ZplOption> barcode);
	Boolean TryGetBarcode(String barcodeType, IBarcodeCmd`2& barcode);
	IEnumerable<IBarcodeCmd`2> GetBarcodes();
	T ConvertConstraint(ICmdConstraint<L1,O1> constraint);



public class ZplGraphicFieldCmd:ZplCmd, IPrinterCmd<ZplLanguage,ZplOption>, IPrinterCmd
	String COMPRESSION_ASCII;
	String COMPRESSION_BINARY;
	String COMPRESSION_COMPRESSED;
	String Language;
	String ArgDelimiter;
	String StartCommand;
	String EndCommand;
	String Raw;
	String Code;
	String Description;
	ICmdOption`2[] Options;
	Int32 NbOptions;
	String WrapStartEnd(String[] cmds);
	ICmdOption<ZplLanguage,ZplOption> GetOption(String optionCode);
	String ToString();
	String Render(ILabelContext lc, Object[] values);
	String ToString(ICmdOption`2[] options);
	String ClearDelimiters(String rendered);
	void AddBarcode(IBarcodeCmd<ZplLanguage,ZplOption> barcode);
	Boolean TryGetBarcode(String barcodeType, IBarcodeCmd`2& barcode);
	IEnumerable<IBarcodeCmd`2> GetBarcodes();
	T ConvertConstraint(ICmdConstraint<L1,O1> constraint);



public class ZplMeasurementUnitCmd:ZplCmd, IPrinterCmd<ZplLanguage,ZplOption>, IPrinterCmd
	Char UNIT_DOTS;
	Char UNIT_INCH;
	Char UNIT_MM;
	String Language;
	String ArgDelimiter;
	String StartCommand;
	String EndCommand;
	String Raw;
	String Code;
	String Description;
	ICmdOption`2[] Options;
	Int32 NbOptions;
	String WrapStartEnd(String[] cmds);
	ICmdOption<ZplLanguage,ZplOption> GetOption(String optionCode);
	String ToString();
	String Render(ILabelContext lc, Object[] values);
	String ToString(ICmdOption`2[] options);
	String ClearDelimiters(String rendered);
	void AddBarcode(IBarcodeCmd<ZplLanguage,ZplOption> barcode);
	Boolean TryGetBarcode(String barcodeType, IBarcodeCmd`2& barcode);
	IEnumerable<IBarcodeCmd`2> GetBarcodes();
	T ConvertConstraint(ICmdConstraint<L1,O1> constraint);



public class ZplPauseCmd:ZplCmd, IPrinterCmd<ZplLanguage,ZplOption>, IPrinterCmd
	String Language;
	String ArgDelimiter;
	String StartCommand;
	String EndCommand;
	String Raw;
	String Code;
	String Description;
	ICmdOption`2[] Options;
	Int32 NbOptions;
	String WrapStartEnd(String[] cmds);
	ICmdOption<ZplLanguage,ZplOption> GetOption(String optionCode);
	String ToString();
	String Render(ILabelContext lc, Object[] values);
	String ToString(ICmdOption`2[] options);
	String ClearDelimiters(String rendered);
	void AddBarcode(IBarcodeCmd<ZplLanguage,ZplOption> barcode);
	Boolean TryGetBarcode(String barcodeType, IBarcodeCmd`2& barcode);
	IEnumerable<IBarcodeCmd`2> GetBarcodes();
	T ConvertConstraint(ICmdConstraint<L1,O1> constraint);



public class ZplPrintDirectoryCmd:ZplCmd, IPrinterCmd<ZplLanguage,ZplOption>, IPrinterCmd
	String Language;
	String ArgDelimiter;
	String StartCommand;
	String EndCommand;
	String Raw;
	String Code;
	String Description;
	ICmdOption`2[] Options;
	Int32 NbOptions;
	String GetCommand(ILabelContext lc, String drive, String shortFilename, String extension);
	String WrapStartEnd(String[] cmds);
	ICmdOption<ZplLanguage,ZplOption> GetOption(String optionCode);
	String ToString();
	String Render(ILabelContext lc, Object[] values);
	String ToString(ICmdOption`2[] options);
	String ClearDelimiters(String rendered);
	void AddBarcode(IBarcodeCmd<ZplLanguage,ZplOption> barcode);
	Boolean TryGetBarcode(String barcodeType, IBarcodeCmd`2& barcode);
	IEnumerable<IBarcodeCmd`2> GetBarcodes();
	T ConvertConstraint(ICmdConstraint<L1,O1> constraint);



public class ZplPrintQuantityCmd:ZplCmd, IPrinterCmd<ZplLanguage,ZplOption>, IPrinterCmd
	String Language;
	String ArgDelimiter;
	String StartCommand;
	String EndCommand;
	String Raw;
	String Code;
	String Description;
	ICmdOption`2[] Options;
	Int32 NbOptions;
	String WrapStartEnd(String[] cmds);
	ICmdOption<ZplLanguage,ZplOption> GetOption(String optionCode);
	String ToString();
	String Render(ILabelContext lc, Object[] values);
	String ToString(ICmdOption`2[] options);
	String ClearDelimiters(String rendered);
	void AddBarcode(IBarcodeCmd<ZplLanguage,ZplOption> barcode);
	Boolean TryGetBarcode(String barcodeType, IBarcodeCmd`2& barcode);
	IEnumerable<IBarcodeCmd`2> GetBarcodes();
	T ConvertConstraint(ICmdConstraint<L1,O1> constraint);



public class ZplRFIDCmd:ZplCmd, IPrinterCmd<ZplLanguage,ZplOption>, IPrinterCmd
	String Language;
	String ArgDelimiter;
	String StartCommand;
	String EndCommand;
	String Raw;
	String Code;
	String Description;
	ICmdOption`2[] Options;
	Int32 NbOptions;
	String WriteHexa(ILabelContext lc, String hexa);
	String WriteAscii(ILabelContext lc, String text);
	String WrapStartEnd(String[] cmds);
	ICmdOption<ZplLanguage,ZplOption> GetOption(String optionCode);
	String ToString();
	String Render(ILabelContext lc, Object[] values);
	String ToString(ICmdOption`2[] options);
	String ClearDelimiters(String rendered);
	void AddBarcode(IBarcodeCmd<ZplLanguage,ZplOption> barcode);
	Boolean TryGetBarcode(String barcodeType, IBarcodeCmd`2& barcode);
	IEnumerable<IBarcodeCmd`2> GetBarcodes();
	T ConvertConstraint(ICmdConstraint<L1,O1> constraint);



public class ZplUseImageCmd:ZplCmd, IPrinterCmd<ZplLanguage,ZplOption>, IPrinterCmd
	String Language;
	String ArgDelimiter;
	String StartCommand;
	String EndCommand;
	String Raw;
	String Code;
	String Description;
	ICmdOption`2[] Options;
	Int32 NbOptions;
	String UseImage(ILabelContext lc, IPrinterFile printerFile);
	String UseImage(ILabelContext lc, String objectName);
	String WrapStartEnd(String[] cmds);
	ICmdOption<ZplLanguage,ZplOption> GetOption(String optionCode);
	String ToString();
	String Render(ILabelContext lc, Object[] values);
	String ToString(ICmdOption`2[] options);
	String ClearDelimiters(String rendered);
	void AddBarcode(IBarcodeCmd<ZplLanguage,ZplOption> barcode);
	Boolean TryGetBarcode(String barcodeType, IBarcodeCmd`2& barcode);
	IEnumerable<IBarcodeCmd`2> GetBarcodes();
	T ConvertConstraint(ICmdConstraint<L1,O1> constraint);



public class DownloadFormat
	Char A_UNCOMPRESSED_ASCII_ZB64;
	Char B_UNCOMPRESSED_BIN;
	Char C_AR_COMP;
	Char P_PNG_ZB64;
	ZplConstraint CONSTRAINT;



public class FileExtension
	String B_BITMAP;
	String E_TRUETYPEEXTENSION_TTE;
	String G_RAWBITMAP_GRF;
	String P_STOREASCOMPRESSED_PNG;
	String T_TRUETYPE_TTF_OPENTYPE_OTF;
	String X_PAINTBRUSH_PCX;
	String NRD_NONREADABLEFILE_NRD;
	String PAC_PROTECTEDACCESSCREDENTIAL_PAC;
	String C_USERDEFINEDMENUFILE_WML;
	String F_USERDEFINEDWEBPAGEFILE_HTM;
	String H_PRINTERFEEDBACKFILE_GET;
	ZplConstraint CONSTRAINT;



public class ZebraACSCompressionHelper
	IEnumerable<String> Split(String str, Int32 chunkSize);
	String Compress(String hexData, Int32 bytesPerRow);
	String Uncompress(String compressedHexData, Int32 bytesPerRow);
	String GetZebraCharCount(Int32 charRepeatCount);



public class ZebraB64CompHelper
	String Compress(String hexData);
	String Compress(Byte[] bytes);
	Byte[] Uncompress(String hexData);



public class ZebraCompHelper
	String Compress(Byte[] bytes, CompressionType compressionType);



public class ZebraZ64CompHelper
	String Compress(String hexData);
	String Compress(Byte[] bytes);
	Byte[] Uncompress(String hexData);
	Byte[] Inflate(Byte[] data);
	Byte[] Deflate(Byte[] data, CompressionLevel compressionLevel);
	Byte GetCompressionHeader(CompressionLevel level);



public class ZplFunctions:ScriptObject, IDictionary<String,Object>, ICollection<KeyValuePair`2>, IEnumerable<KeyValuePair`2>, IEnumerable, IScriptObject, IDictionary, ICollection, IFormattable, IScribanLib
	String Prefix;
	ScriptMemberImportFlags ImportFlags;
	Int32 Count;
	Boolean IsReadOnly;
	Object Item;
	ICollection<String> Keys;
	ICollection<Object> Values;
	String ChangeFont(TemplateContext scribanContext, String fontName);
	String LineH(TemplateContext context, Decimal yPos, Nullable<Decimal> fromXPos, Nullable<Decimal> toXPos, Int32 thickness, String color, Int32 rounding);
	String LineV(TemplateContext context, Decimal xPos, Nullable<Decimal> fromYPos, Nullable<Decimal> toYPos, Int32 thickness, String color, Int32 rounding);
	String Box(TemplateContext context, Nullable<Decimal> fromXPos, Nullable<Decimal> fromYPos, Nullable<Decimal> toXPos, Nullable<Decimal> toYPos, Int32 thickness, String color, Int32 rounding);
	String BoxInternal(TemplateContext context, Decimal fromXPos, Decimal fromYPos, Decimal toXPos, Decimal toYPos, Int32 thickness, String color, Int32 rounding);
	String Diagonal(TemplateContext context, Nullable<Decimal> fromXPos, Nullable<Decimal> fromYPos, Nullable<Decimal> toXPos, Nullable<Decimal> toYPos, Nullable<Int32> thickness, String color);
	String Circle(TemplateContext context, Nullable<Decimal> fromXPos, Nullable<Decimal> fromYPos, Nullable<Decimal> diameter, Nullable<Int32> thickness, String color);
	String Ellipse(TemplateContext context, Nullable<Decimal> fromXPos, Nullable<Decimal> fromYPos, Nullable<Decimal> toXPos, Nullable<Decimal> toYPos, Nullable<Int32> thickness, String color);
	String Move(TemplateContext context, Int32 colDots, Int32 rowDots, VerticalAlign vAlign, HorizontalAlign hAlign);
	String MoveV(TemplateContext context, Int32 rowDots, VerticalAlign vAlign);
	String MoveH(TemplateContext context, Int32 colDots, HorizontalAlign hAlign);
	String FieldOrigin(TemplateContext context, Int32 colDots, Int32 rowDots);
	String GetSerialNumber(TemplateContext context, String startingValue, Int32 incDecValue, Boolean addLeadingZeros);
	String CR(TemplateContext context);
	String FieldBlock(TemplateContext context, String justification, Nullable<Decimal> fromX, Nullable<Decimal> ToX, Nullable<Int32> maxNbLines, String sizeUnit, Nullable<Decimal> addDelSpacesDots, Nullable<Decimal> hangingIndentDots);
	String PrintQuantity(TemplateContext context, Int32 nbCopies);
	void SavePosition(TemplateContext context);
	String RestorePosition(TemplateContext context);
	String Color(TemplateContext context, IColor foreColor, IColor backColor);
	String ChangeFont(TemplateContext context, IFont font);
	String Font(TemplateContext context, String fontCode, Nullable<Decimal> height, Nullable<Decimal> width, String sizeUnit);
	String FontByFileName(TemplateContext context, Nullable<Guid> fontFileID, Nullable<Decimal> height, Nullable<Decimal> width, String sizeUnit);
	String FieldData(TemplateContext context, String text);
	String FieldHex(TemplateContext context, Char hexPrefix);
	String RFIDDefine(TemplateContext context, Int32 totalBitSize, Int32[] partitionSizes);
	String RFIDWrite(TemplateContext context, String text);
	String RFIDWriteHexa(TemplateContext context, String hexa);
	String RFIDWriteAscii(TemplateContext context, String text);
	String RenderBoxColor(TemplateContext context, Decimal fromXPos, Decimal fromYPos, Decimal width, Decimal height, Int32 r, Int32 g, Int32 b);
	String RenderColorImage(TemplateContext context, Decimal fromXPos, Decimal fromYPos, Decimal mag, Byte[] image);
	String GetOrientation(ICoordinate fromCoord, ICoordinate toCoord);
	String Comment(String comment, Object[] args);
	String ToBarcode(TemplateContext context, Object fieldValue, String barcodeName);
	String RenderBarcode(ILabelContext lc, IBarcode barcode, IBarcodeOption[] options);
	IBarcodeOption FixDots(Nullable<Double> densityRatio, IBarcode barcode, IBarcodeOption option);
	String BarcodeToImage(TemplateContext context, String symbology, String text, Nullable<Decimal> scaleX, Nullable<Decimal> scaleY, String rotate, String includeText);
	String UseFontOnce(TemplateContext context, String fontName, String orientation, Nullable<Decimal> height, Nullable<Decimal> width, String sizeUnit);
	void System.Collections.IDictionary.Add(Object key, Object value);
	void Clear();
	Boolean System.Collections.IDictionary.Contains(Object key);
	IDictionaryEnumerator System.Collections.IDictionary.GetEnumerator();
	void System.Collections.IDictionary.Remove(Object key);
	void System.Collections.ICollection.CopyTo(Array array, Int32 index);
	IEnumerable<String> GetMembers();
	Boolean Contains(String member);
	Boolean TryGetValue(TemplateContext context, SourceSpan span, String member, Object& value);
	T GetSafeValue(String name, T defaultValue);
	Boolean System.Collections.Generic.IDictionary<System.String,System.Object>.TryGetValue(String key, Object& value);
	Boolean CanWrite(String member);
	Boolean TrySetValue(TemplateContext context, SourceSpan span, String member, Object value, Boolean readOnly);
	void SetValue(String member, Object value, Boolean readOnly);
	void Add(String key, Object value);
	Boolean ContainsKey(String key);
	Boolean Remove(String member);
	void SetReadOnly(String member, Boolean readOnly);
	String ToString(String format, IFormatProvider formatProvider);
	String ToString(IFormatProvider formatProvider);
	String ToString();
	void CopyTo(ScriptObject dest);
	IScriptObject Clone(Boolean deep);
	IEnumerator<KeyValuePair`2> GetEnumerator();
	IEnumerator System.Collections.IEnumerable.GetEnumerator();
	ScriptObject From(Object obj);
	Boolean IsImportable(Object obj);
	void System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.Add(KeyValuePair<String,Object> item);
	Boolean System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.Contains(KeyValuePair<String,Object> item);
	void System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.CopyTo(KeyValuePair`2[] array, Int32 arrayIndex);
	Boolean System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.Remove(KeyValuePair<String,Object> item);



public class ZplHandler
	String HandleOrientation(ILabelContext lc, IOrientable expr, Boolean reset);
	String HandleOrientation(ILabelContext lc, String orientation, Boolean reset);
	String HandleFieldReverse(ILabelContext lc, IReversable expr);
	String HandleColor(ILabelContext lc, IColored colored);
	Boolean HandleHexEncoding(ILabelContext lc, String fieldDataCmd, String fieldSepCmd, String text, String hexEncoding, String& fieldCommand);
	String ReplaceHex(String text, String hexEncoding);
	ValueTuple<String,String> HandleFieldData(ILabelContext lc, String exprValue);
	String HandleGoto(ILabelContext lc, IModelDetail expr);
	String HandleJustification(ILabelContext lc, IModelDetail expr);
	String HandleFont(ILabelContext lc, IModelDetail expr);
	String HandleGraphicDrawing(ILabelContext lc, IModelGraphic graphic);
	String HandleComment(ILabelContext lc, String commentText, Object[] args);
	String HandleBarcode(ILabelContext lc, IBarcodeable expr, String exprValue);



public class ZplCmd:AbstractCmd<ZplLanguage,ZplOption>, IPrinterCmd<ZplLanguage,ZplOption>, IPrinterCmd
	ZplCmd START;
	ZplCmd END;
	ZplCmd FIELD_SEPARATOR;
	ZplCmd LABEL_HOME;
	ZplCmd LABEL_LENGTH;
	ZplCmd LABEL_SHIFT;
	ZplCmd LABEL_TOP;
	ZplCmd PRINT_WIDTH;
	ZplCmd GRAPHIC_BOX;
	ZplCmd RENDER_COLOR_BOX;
	ZplCmd RENDER_COLOR_IMAGE;
	ZplCmd GRAPHIC_CIRCLE;
	ZplCmd GRAPHIC_DIAGONAL;
	ZplCmd GRAPHIC_ELLIPSE;
	ZplCmd GRAPHIC_FIELD;
	ZplCmd FIELD_ORIGIN;
	ZplCmd SERIAL_NUMBER;
	ZplCmd COMMENT;
	ZplCmd FIELD_DATA;
	ZplCmd CHANGE_FONT;
	ZplCmd USE_FONT_BY_NAME;
	ZplCmd USE_FONT_ONCE;
	ZplCmd BC_DEFAULT;
	ZplCmd FIELD_HEX;
	ZplCmd PRINT_DIRECTORY;
	ZplCmd FIELD_REVERSE;
	ZplCmd FIELD_ORIENTATION;
	ZplCmd SET_UNITS;
	ZplCmd RFID_DEFINE;
	ZplCmd CHANGE_CARET;
	ZplCmd CHANGE_DELIMITER;
	ZplCmd PRINT_CONFIG;
	ZplCmd WRITE_QUERY;
	ZplCmd CHANGE_TILDE;
	ZplCmd CANCEL_ALL;
	ZplCmd RESET_OPTIONAL_MEMORY;
	ZplCmd SET_MEDIA_SENSOR_CALIBRATION;
	ZplCmd ENABLE_COMMUNICATIONS_DIAGNOSTICS;
	ZplCmd DISABLE_DIAGNOSTICS;
	ZplCmd PRINT_QTY;
	ZplCmd PAUSE;
	ZplCmd CHANGE_ENCODING;
	ZplCmd DOWNLOAD_OBJECT;
	String Language;
	String ArgDelimiter;
	String StartCommand;
	String EndCommand;
	String Raw;
	String Code;
	String Description;
	ICmdOption`2[] Options;
	Int32 NbOptions;
	String WrapStartEnd(String[] cmds);
	ICmdOption<ZplLanguage,ZplOption> GetOption(String optionCode);
	String ToString();
	String Render(ILabelContext lc, Object[] values);
	String ToString(ICmdOption`2[] options);
	String ClearDelimiters(String rendered);
	void AddBarcode(IBarcodeCmd<ZplLanguage,ZplOption> barcode);
	Boolean TryGetBarcode(String barcodeType, IBarcodeCmd`2& barcode);
	IEnumerable<IBarcodeCmd`2> GetBarcodes();
	T ConvertConstraint(ICmdConstraint<L1,O1> constraint);



public class ZplConstraint:AbstractRangeConstraint<ZplLanguage,ZplOption>, ICmdConstraint<ZplLanguage,ZplOption>, ICmdConstraint
	Char YES;
	Char NO;
	ZplConstraint YN_Y;
	ZplConstraint YN_N;
	ZplConstraint N_N;
	ZplConstraint CODABLOCK_MODE;
	ZplConstraint UCC_MODE;
	ZplConstraint MAXICODE_MODE;
	ZplConstraint MICROPDF_MODE;
	ZplConstraint CODE49_MODE;
	ZplConstraint FONT_CODE;
	ZplConstraint CODE49_PRINT_ABN;
	ZplConstraint CHECK_DIGIT_MSI;
	ZplConstraint CORNER_ROUNDING;
	ZplConstraint JUSTIFICATION;
	ZplConstraint RANGE_1_10;
	ZplConstraint RANGE_1_8;
	ZplConstraint RANGE_1_8_1_1;
	ZplConstraint RANGE_0_300;
	ZplConstraint RANGE_1_26;
	ZplConstraint RANGE_0_7;
	ZplConstraint RANGE_9_49;
	ZplConstraint DOTS_MAX;
	ZplConstraint DOTS_9999;
	ZplConstraint PLUS_MINUS_9999;
	ZplConstraint PLUS_MINUS_120;
	ZplConstraint DOTS_4095;
	ZplConstraint DOTS_255;
	ZplConstraint RGB;
	ZplConstraint DOTS_10;
	ZplConstraint DIAGONAL;
	ZplConstraint N1TO99_999_999;
	ZplConstraint N0TO99_999_999;
	ZplConstraint TLC39_RATIO_2TO3;
	ZplConstraint RSS_SYMBOLOGY;
	ZplConstraint QR_MODEL;
	ZplConstraint QR_EC;
	ZplConstraint PDF417_SEC;
	ZplConstraint PDF417_COLS;
	ZplConstraint PDF417_ROWS;
	ZplConstraint CODABLOCK_HEIGHT;
	ZplConstraint CODABLOCK_CHARS;
	ZplConstraint CODABLOCK_ROWS;
	ZplConstraint RSS_DOTS_MAX;
	ZplConstraint RSS_MAG_FACTOR;
	ZplConstraint RSS_SEPARATOR_HEIGHT;
	ZplConstraint RSS_SEG_WIDTH;
	ZplConstraint POSTAL_TYPE;
	ZplConstraint CODABAR_ABCD_A;
	ZplConstraint DATA_MATRIX_QUALITY;
	ZplConstraint DATA_MATRIX_FORMATID;
	ZplConstraint DATA_MATRIX_ASPECTRATIO;
	ZplConstraint DATA_MATRIX_ESCAPE;
	ZplConstraint GF_COMPRESSION;
	ZplConstraint UNIT_RENDER_RESOLUTION;
	ZplConstraint UNIT_PRINT_RESOLUTION;
	LengthConstraint<ZplLanguage,ZplOption> FILE_EXTENSION;
	ZplCmd DefaultSetBy;
	Object DefaultValue;
	CmdConstraintType ConstraintType;
	Char GetYesNo(Nullable<Boolean> val);
	ZplConstraint WithDefault(Object defaultValue);
	Object Clean(Object newValue);
	Exception CheckValid(IPrinterCmd<ZplLanguage,ZplOption> barcode, ICmdOption<ZplLanguage,ZplOption> option, Object newValue);
	String ToString();



public class ZplLanguage:AbstractLanguage<ZplLanguage,ZplOption>, IPrinterLanguage<ZplLanguage,ZplOption>, IPrinterLanguage, ISelectable, IRenderer
	String CODE;
	String ARG_DELIMITER;
	String START_COMMAND;
	String END_COMMAND;
	String Code;
	String Description;
	String StartLabel;
	String EndLabel;
	String StartCommand;
	String EndCommand;
	String ArgDelimiter;
	String LineWrapValue;
	Boolean AllowsColors;
	String GetNbCopiesExpr();
	String Validate(ILabelContext lc, String text);
	void AddCommand(ILabelContext lc, String& text, Int32& insertPoint, ZplCmd command, Object[] values);
	String InsertFontDownload(ILabelContext lc, String text);
	void AddFontsForRendering(ILabelContext lc);
	Boolean ContainsFontDownload(String text);
	Boolean ContainsFontReference(String text);
	String Cleanup(String text);
	Boolean WithinIteratorSnippet(ILabelContext lc);
	Boolean TryGetBarcode(String barcodeType, IBarcodeCmd`2& bc);
	String SetNbCopies(ILabelContext lc, String text, Int32 totalQty, Nullable<Int32> pauseCutValue, Nullable<Int32> replicateEachSerial, Nullable<Boolean> overridePauseCount);
	ISerialInfo GetSerialInfo(ILabelContext lc, String text);
	Nullable<Int32> GetGroupValue(Match match, Int32 matchNbr);
	String AddPause(ILabelContext lc, String text);
	String HandleDensity(ILabelContext lc, String text);
	String HandleFonts(ILabelContext lc, String text);
	String GetSendFontCmd(ILabelContext lc, IPrinterFile file);
	String GetAssignCmd(ILabelContext lc, IFont font, IPrinterFile printerFile);
	ValueTuple<String,String> HandleHexEncoding(ILabelContext lc, String exprValue, IModelDetail modelExpr, String fieldDataCmd, String fieldSepCmd);
	String HandleGoto(ILabelContext lc, String exprType, IModelDetail modelExpr);
	String HandleColor(ILabelContext lc, IModelDetail modelExpr);
	String HandleFont(ILabelContext lc, IModelDetail modelExpr);
	String HandleJustification(ILabelContext lc, IModelDetail modelExpr);
	String HandleFieldReverse(ILabelContext lc, IModelDetail modelExpr);
	String HandleOrientation(ILabelContext lc, IModelDetail modelExpr, Boolean reset);
	ValueTuple<String,String> HandleFieldData(ILabelContext lc, String exprValue);
	String HandleLineWrap(ILabelContext lc, String exprValue, IModelDetail modelExpr);
	String HandleBarcode(ILabelContext lc, IModelDetail modelExpr, IBarcodeable barcodable, String exprValue);
	String DrawGraphic(ILabelContext lc, IModelGraphic graphic);
	RenderResult RenderAsOutput(ILabelContext lc);
	IList<IPrinterCmd> GetCommands();
	void AddCommand(IPrinterCmd cmd);
	Int32 FindAfter(String text, IPrinterCmd cmd);
	Int32 FindBefore(String text, IPrinterCmd cmd);
	Boolean SupportsRendering(ILabelContext lc, ContentFormat from, ContentFormat to);
	String HandleScript(ILabelContext lc, IExpr expr);
	String HandleIterator(ILabelContext lc, IExprRow exprRow, ILabelElement dataElement);
	String HandleHard(String exprValue);
	String HandleScreen(ILabelContext lc, IExpr expr);
	String HandleFunction(ILabelContext lc, ILabelElement dataElement, IExpr expr);
	String HandleImage(ILabelContext lc, ILabelElement dataElement, IImageDriven imageDriven, IOrientable orientable);
	String HandleContent(ILabelContext lc, IContentable contentable);
	String HandleSubstitution(ILabelContext lc, ISubstitutable substitutable, String exprValue);
	ValueTuple<String,String> GetExpression(ILabelContext lc, IExprRow exprRow);
	ValueTuple<String,String> HandleRule(ILabelContext lc, IModelDetail modelExpr);
	FileResult GetPrintResult(ILabelContext labelContext, Int32 nbCopies);



public class ZplOption:AbstractOption<ZplLanguage,ZplOption>, ICmdOption<ZplLanguage,ZplOption>, ICmdOption
	ZplOption ORIENTATION;
	ZplOption ORIENTATION_A;
	ZplOption ORIENTATION_JUST_NORMAL;
	ZplOption ORIENTATION_DEFAULT_NORMAL;
	ZplOption ORIENTATION_RSS;
	ZplOption HEIGHT_MAX;
	ZplOption WIDTH_MAX;
	ZplOption HEIGHT_9999;
	ZplOption RSS_SYMBOLOGY;
	ZplOption RSS_MAG_FACTOR;
	ZplOption RSS_SEPARATOR_HEIGHT;
	ZplOption RSS_BC_HEIGHT;
	ZplOption RSS_SEG_WIDTH;
	ZplOption PRINT_INT_LINE_N;
	ZplOption PRINT_INT_LINE_Y;
	ZplOption PRINT_INT_LINE_CODE49;
	ZplOption PRINT_INT_LINE_ABOVE_N;
	ZplOption PRINT_INT_LINE_ABOVE_Y;
	ZplOption POSTAL_TYPE;
	ZplOption CHECK_DIGIT_YNY;
	ZplOption CHECK_DIGIT_YNN;
	ZplOption CHECK_DIGIT_N;
	ZplOption CHECK_DIGIT_MSI;
	ZplOption CODABLOCK_MODE;
	ZplOption CODE49_MODE;
	ZplOption UCC_MODE;
	ZplOption MAXICODE_MODE;
	ZplOption MICROPDF_MODE;
	ZplOption PDF417_SEC;
	ZplOption PDF417_COLS;
	ZplOption PDF417_ROWS;
	ZplOption CODABLOCK_SEC;
	ZplOption CODABLOCK_HEIGHT;
	ZplOption CODABLOCK_CHARS;
	ZplOption CODABLOCK_ROWS;
	ZplOption GF_COMPRESSION;
	ZplOption OBJECT_NAME;
	ZplOption FONT_CODE;
	String HEIGHT_CODE;
	String WIDTH_CODE;
	String WIDTH_TLC39_CODE1;
	String WIDTH_TLC39_CODE2;
	String HEIGHT_TLC39_CODE1;
	String HEIGHT_TLC39_CODE2;
	String Code;
	String Description;
	ICmdConstraint<ZplLanguage,ZplOption> Constraint;
	C WithDefault(Object defaultValue);
	String ToString();



public class ZplTildeCmd:ZplCmd, IPrinterCmd<ZplLanguage,ZplOption>, IPrinterCmd
	String StartCommand;
	String EndCommand;
	String Language;
	String ArgDelimiter;
	String Raw;
	String Code;
	String Description;
	ICmdOption`2[] Options;
	Int32 NbOptions;
	String ClearDelimiters(String rendered);
	String WrapStartEnd(String[] cmds);
	ICmdOption<ZplLanguage,ZplOption> GetOption(String optionCode);
	String ToString();
	String Render(ILabelContext lc, Object[] values);
	String ToString(ICmdOption`2[] options);
	void AddBarcode(IBarcodeCmd<ZplLanguage,ZplOption> barcode);
	Boolean TryGetBarcode(String barcodeType, IBarcodeCmd`2& barcode);
	IEnumerable<IBarcodeCmd`2> GetBarcodes();
	T ConvertConstraint(ICmdConstraint<L1,O1> constraint);



public class TsplCmd:AbstractCmd<TsplLanguage,TsplOption>, IPrinterCmd<TsplLanguage,TsplOption>, IPrinterCmd
	String Language;
	String ArgDelimiter;
	String StartCommand;
	String EndCommand;
	String Raw;
	String Code;
	String Description;
	ICmdOption`2[] Options;
	Int32 NbOptions;
	ICmdOption<TsplLanguage,TsplOption> GetOption(String optionCode);
	String ToString();
	String Render(ILabelContext lc, Object[] values);
	String ToString(ICmdOption`2[] options);
	String ClearDelimiters(String rendered);
	void AddBarcode(IBarcodeCmd<TsplLanguage,TsplOption> barcode);
	Boolean TryGetBarcode(String barcodeType, IBarcodeCmd`2& barcode);
	IEnumerable<IBarcodeCmd`2> GetBarcodes();
	T ConvertConstraint(ICmdConstraint<L1,O1> constraint);



public class TsplConstraint:AbstractRangeConstraint<TsplLanguage,TsplOption>, ICmdConstraint<TsplLanguage,TsplOption>, ICmdConstraint
	TsplCmd DefaultSetBy;
	Object DefaultValue;
	CmdConstraintType ConstraintType;
	TsplConstraint WithDefault(Object defaultValue);
	Object Clean(Object newValue);
	Exception CheckValid(IPrinterCmd<TsplLanguage,TsplOption> barcode, ICmdOption<TsplLanguage,TsplOption> option, Object newValue);
	String ToString();



public class TsplLanguage:AbstractLanguage<TsplLanguage,TsplOption>, IPrinterLanguage<TsplLanguage,TsplOption>, IPrinterLanguage, ISelectable, IRenderer
	String CODE;
	String Code;
	String Description;
	String StartLabel;
	String EndLabel;
	String StartCommand;
	String EndCommand;
	String ArgDelimiter;
	Boolean AllowsColors;
	String LineWrapValue;
	String Validate(ILabelContext lc, String text);
	Boolean TryGetBarcode(String barcodeType, IBarcodeCmd`2& bc);
	IList<IPrinterCmd> GetCommands();
	void AddCommand(IPrinterCmd cmd);
	String SetNbCopies(ILabelContext lc, String text, Int32 totalQty, Nullable<Int32> pauseCutValue, Nullable<Int32> replicateEachSerial, Nullable<Boolean> overridePauseCount);
	String AddPause(ILabelContext lc, String text);
	Int32 FindAfter(String text, IPrinterCmd cmd);
	Int32 FindBefore(String text, IPrinterCmd cmd);
	String HandleDensity(ILabelContext lc, String text);
	String HandleFonts(ILabelContext lc, String text);
	void AddFontsForRendering(ILabelContext lc);
	ISerialInfo GetSerialInfo(ILabelContext lc, String text);
	RenderResult RenderAsOutput(ILabelContext lc);
	Boolean SupportsRendering(ILabelContext lc, ContentFormat from, ContentFormat to);
	String HandleScript(ILabelContext lc, IExpr expr);
	String HandleIterator(ILabelContext lc, IExprRow exprRow, ILabelElement dataElement);
	String HandleHard(String exprValue);
	String HandleScreen(ILabelContext lc, IExpr expr);
	String HandleFunction(ILabelContext lc, ILabelElement dataElement, IExpr expr);
	String HandleImage(ILabelContext lc, ILabelElement dataElement, IImageDriven imageDriven, IOrientable orientable);
	String HandleContent(ILabelContext lc, IContentable contentable);
	String HandleSubstitution(ILabelContext lc, ISubstitutable substitutable, String exprValue);
	ValueTuple<String,String> GetExpression(ILabelContext lc, IExprRow exprRow);
	ValueTuple<String,String> HandleRule(ILabelContext lc, IModelDetail modelExpr);
	String HandleBarcode(ILabelContext lc, IModelDetail modelExpr, IBarcodeable barcodable, String exprValue);
	ValueTuple<String,String> HandleHexEncoding(ILabelContext lc, String exprValue, IModelDetail modelExpr, String fieldDataCmd, String fieldSepCmd);
	String HandleGoto(ILabelContext lc, String exprType, IModelDetail modelExpr);
	String HandleColor(ILabelContext lc, IModelDetail modelExpr);
	String HandleFont(ILabelContext lc, IModelDetail modelExpr);
	String HandleJustification(ILabelContext lc, IModelDetail modelExpr);
	String HandleFieldReverse(ILabelContext lc, IModelDetail modelExpr);
	String HandleOrientation(ILabelContext lc, IModelDetail modelExpr, Boolean v);
	ValueTuple<String,String> HandleFieldData(ILabelContext lc, String exprValue);
	String HandleLineWrap(ILabelContext lc, String exprValue, IModelDetail modelExpr);
	String DrawGraphic(ILabelContext lc, IModelGraphic graphic);
	FileResult GetPrintResult(ILabelContext labelContext, Int32 nbCopies);



public class TsplOption:AbstractOption<TsplLanguage,TsplOption>, ICmdOption<TsplLanguage,TsplOption>, ICmdOption
	String Code;
	String Description;
	ICmdConstraint<TsplLanguage,TsplOption> Constraint;
	C WithDefault(Object defaultValue);
	String ToString();



public class StarplBarcodeCmd:StarplCmd, IPrinterCmd<StarplLanguage,StarplOption>, IPrinterCmd, IBarcodeCmd<StarplLanguage,StarplOption>, ILanguageDriven
	String PREFIX;
	String BarcodeType;
	String Name;
	String Dimension;
	String Code;
	String Language;
	String ArgDelimiter;
	String StartCommand;
	String EndCommand;
	String Raw;
	String Description;
	ICmdOption`2[] Options;
	Int32 NbOptions;
	String Render(ILabelContext lc, Object[] values);
	ICmdOption<StarplLanguage,StarplOption> GetOption(String optionCode);
	String ToString();
	String ToString(ICmdOption`2[] options);
	String ClearDelimiters(String rendered);
	void AddBarcode(IBarcodeCmd<StarplLanguage,StarplOption> barcode);
	Boolean TryGetBarcode(String barcodeType, IBarcodeCmd`2& barcode);
	IEnumerable<IBarcodeCmd`2> GetBarcodes();
	T ConvertConstraint(ICmdConstraint<L1,O1> constraint);



public class StarplCmd:AbstractCmd<StarplLanguage,StarplOption>, IPrinterCmd<StarplLanguage,StarplOption>, IPrinterCmd
	StarplCmd MAGNIFY_CMD;
	StarplCmd FONT_CMD;
	StarplCmd COMMENT_CMD;
	StarplCmd ALIGN_CMD;
	StarplCmd INVERT_CMD;
	StarplCmd UNDERLINE_CMD;
	StarplCmd UPPERLINE_CMD;
	StarplCmd NEGATIVE_CMD;
	StarplCmd PLAIN_CMD;
	String Language;
	String ArgDelimiter;
	String StartCommand;
	String EndCommand;
	String Raw;
	String Code;
	String Description;
	ICmdOption`2[] Options;
	Int32 NbOptions;
	String Render(ILabelContext lc, Object[] values);
	ValueTuple<String,Object> GetTuple(String parameter, Object value);
	ICmdOption<StarplLanguage,StarplOption> GetOption(String optionCode);
	String ToString();
	String ToString(ICmdOption`2[] options);
	String ClearDelimiters(String rendered);
	void AddBarcode(IBarcodeCmd<StarplLanguage,StarplOption> barcode);
	Boolean TryGetBarcode(String barcodeType, IBarcodeCmd`2& barcode);
	IEnumerable<IBarcodeCmd`2> GetBarcodes();
	T ConvertConstraint(ICmdConstraint<L1,O1> constraint);



public class StarplConstraint:AbstractRangeConstraint<StarplLanguage,StarplOption>, ICmdConstraint<StarplLanguage,StarplOption>, ICmdConstraint
	StarplConstraint FONT_A_OR_B;
	StarplConstraint DOTS_MAX;
	StarplConstraint ALIGNMENT;
	StarplConstraint ON_OFF;
	StarplCmd DefaultSetBy;
	Object DefaultValue;
	CmdConstraintType ConstraintType;
	StarplConstraint WithDefault(Object defaultValue);
	Object Clean(Object newValue);
	Exception CheckValid(IPrinterCmd<StarplLanguage,StarplOption> barcode, ICmdOption<StarplLanguage,StarplOption> option, Object newValue);
	String ToString();



public class StarplLanguage:AbstractLanguage<StarplLanguage,StarplOption>, IPrinterLanguage<StarplLanguage,StarplOption>, IPrinterLanguage, ISelectable, IRenderer
	String CODE;
	String Code;
	String Description;
	String StartLabel;
	String EndLabel;
	String StartCommand;
	String EndCommand;
	String ArgDelimiter;
	Boolean AllowsColors;
	String LineWrapValue;
	String Validate(ILabelContext lc, String text);
	Boolean TryGetBarcode(String barcodeType, IBarcodeCmd`2& bc);
	IList<IPrinterCmd> GetCommands();
	void AddCommand(IPrinterCmd cmd);
	String SetNbCopies(ILabelContext lc, String text, Int32 totalQty, Nullable<Int32> pauseCutValue, Nullable<Int32> replicateEachSerial, Nullable<Boolean> overridePauseCount);
	String AddPause(ILabelContext lc, String text);
	Int32 FindAfter(String text, IPrinterCmd cmd);
	Int32 FindBefore(String text, IPrinterCmd cmd);
	String HandleDensity(ILabelContext lc, String text);
	String HandleFonts(ILabelContext lc, String text);
	void AddFontsForRendering(ILabelContext lc);
	ISerialInfo GetSerialInfo(ILabelContext lc, String text);
	RenderResult RenderAsOutput(ILabelContext lc);
	Boolean SupportsRendering(ILabelContext lc, ContentFormat from, ContentFormat to);
	String HandleScript(ILabelContext lc, IExpr expr);
	String HandleIterator(ILabelContext lc, IExprRow exprRow, ILabelElement dataElement);
	String HandleHard(String exprValue);
	String HandleScreen(ILabelContext lc, IExpr expr);
	String HandleFunction(ILabelContext lc, ILabelElement dataElement, IExpr expr);
	String HandleImage(ILabelContext lc, ILabelElement dataElement, IImageDriven imageDriven, IOrientable orientable);
	String HandleContent(ILabelContext lc, IContentable contentable);
	String HandleSubstitution(ILabelContext lc, ISubstitutable substitutable, String exprValue);
	ValueTuple<String,String> GetExpression(ILabelContext lc, IExprRow exprRow);
	ValueTuple<String,String> HandleRule(ILabelContext lc, IModelDetail modelExpr);
	String HandleBarcode(ILabelContext lc, IModelDetail modelExpr, IBarcodeable barcodable, String exprValue);
	ValueTuple<String,String> HandleHexEncoding(ILabelContext lc, String exprValue, IModelDetail modelExpr, String fieldDataCmd, String fieldSepCmd);
	String HandleGoto(ILabelContext lc, String exprType, IModelDetail modelExpr);
	String HandleColor(ILabelContext lc, IModelDetail modelExpr);
	String HandleFont(ILabelContext lc, IModelDetail modelExpr);
	String HandleJustification(ILabelContext lc, IModelDetail modelExpr);
	String HandleFieldReverse(ILabelContext lc, IModelDetail modelExpr);
	String HandleOrientation(ILabelContext lc, IModelDetail modelExpr, Boolean v);
	ValueTuple<String,String> HandleFieldData(ILabelContext lc, String exprValue);
	String HandleLineWrap(ILabelContext lc, String exprValue, IModelDetail modelExpr);
	String DrawGraphic(ILabelContext lc, IModelGraphic graphic);
	FileResult GetPrintResult(ILabelContext labelContext, Int32 nbCopies);



public class StarplOption:AbstractOption<StarplLanguage,StarplOption>, ICmdOption<StarplLanguage,StarplOption>, ICmdOption
	StarplOption ON_OFF;
	String Code;
	String Description;
	ICmdConstraint<StarplLanguage,StarplOption> Constraint;
	C WithDefault(Object defaultValue);
	String ToString();



public class SbplLanguage:AbstractLanguage<SbplLanguage,SbplOption>, IPrinterLanguage<SbplLanguage,SbplOption>, IPrinterLanguage, ISelectable, IRenderer
	String CODE;
	String Code;
	String Description;
	String StartLabel;
	String EndLabel;
	String StartCommand;
	String EndCommand;
	String ArgDelimiter;
	Boolean AllowsColors;
	String LineWrapValue;
	String Validate(ILabelContext lc, String text);
	Boolean TryGetBarcode(String barcodeType, IBarcodeCmd`2& bc);
	IList<IPrinterCmd> GetCommands();
	void AddCommand(IPrinterCmd cmd);
	String SetNbCopies(ILabelContext lc, String text, Int32 totalQty, Nullable<Int32> pauseCutValue, Nullable<Int32> replicateEachSerial, Nullable<Boolean> overridePauseCount);
	String AddPause(ILabelContext lc, String text);
	Int32 FindAfter(String text, IPrinterCmd cmd);
	Int32 FindBefore(String text, IPrinterCmd cmd);
	String HandleDensity(ILabelContext lc, String text);
	String HandleFonts(ILabelContext lc, String text);
	void AddFontsForRendering(ILabelContext lc);
	ISerialInfo GetSerialInfo(ILabelContext lc, String text);
	RenderResult RenderAsOutput(ILabelContext lc);
	Boolean SupportsRendering(ILabelContext lc, ContentFormat from, ContentFormat to);
	String HandleScript(ILabelContext lc, IExpr expr);
	String HandleIterator(ILabelContext lc, IExprRow exprRow, ILabelElement dataElement);
	String HandleHard(String exprValue);
	String HandleScreen(ILabelContext lc, IExpr expr);
	String HandleFunction(ILabelContext lc, ILabelElement dataElement, IExpr expr);
	String HandleImage(ILabelContext lc, ILabelElement dataElement, IImageDriven imageDriven, IOrientable orientable);
	String HandleContent(ILabelContext lc, IContentable contentable);
	String HandleSubstitution(ILabelContext lc, ISubstitutable substitutable, String exprValue);
	ValueTuple<String,String> GetExpression(ILabelContext lc, IExprRow exprRow);
	ValueTuple<String,String> HandleRule(ILabelContext lc, IModelDetail modelExpr);
	String HandleBarcode(ILabelContext lc, IModelDetail modelExpr, IBarcodeable barcodable, String exprValue);
	ValueTuple<String,String> HandleHexEncoding(ILabelContext lc, String exprValue, IModelDetail modelExpr, String fieldDataCmd, String fieldSepCmd);
	String HandleGoto(ILabelContext lc, String exprType, IModelDetail modelExpr);
	String HandleColor(ILabelContext lc, IModelDetail modelExpr);
	String HandleFont(ILabelContext lc, IModelDetail modelExpr);
	String HandleJustification(ILabelContext lc, IModelDetail modelExpr);
	String HandleFieldReverse(ILabelContext lc, IModelDetail modelExpr);
	String HandleOrientation(ILabelContext lc, IModelDetail modelExpr, Boolean v);
	ValueTuple<String,String> HandleFieldData(ILabelContext lc, String exprValue);
	String HandleLineWrap(ILabelContext lc, String exprValue, IModelDetail modelExpr);
	String DrawGraphic(ILabelContext lc, IModelGraphic graphic);
	FileResult GetPrintResult(ILabelContext labelContext, Int32 nbCopies);



public class SbplOption:AbstractOption<SbplLanguage,SbplOption>, ICmdOption<SbplLanguage,SbplOption>, ICmdOption
	String Code;
	String Description;
	ICmdConstraint<SbplLanguage,SbplOption> Constraint;
	C WithDefault(Object defaultValue);
	String ToString();



public class PresLanguage:AbstractLanguage<PresLanguage,PresOption>, IPrinterLanguage<PresLanguage,PresOption>, IPrinterLanguage, ISelectable, IRenderer
	String CODE;
	String Code;
	String Description;
	String StartLabel;
	String EndLabel;
	String StartCommand;
	String EndCommand;
	String ArgDelimiter;
	Boolean AllowsColors;
	String LineWrapValue;
	String Validate(ILabelContext context, String text);
	Boolean TryGetBarcode(String barcodeType, IBarcodeCmd`2& bc);
	IList<IPrinterCmd> GetCommands();
	void AddCommand(IPrinterCmd cmd);
	String SetNbCopies(ILabelContext lc, String text, Int32 totalQty, Nullable<Int32> pauseCutValue, Nullable<Int32> replicateEachSerial, Nullable<Boolean> overridePauseCount);
	String AddPause(ILabelContext lc, String text);
	Int32 FindAfter(String text, IPrinterCmd cmd);
	Int32 FindBefore(String text, IPrinterCmd cmd);
	String HandleDensity(ILabelContext lc, String text);
	String HandleFonts(ILabelContext lc, String text);
	void AddFontsForRendering(ILabelContext lc);
	ISerialInfo GetSerialInfo(ILabelContext lc, String text);
	RenderResult RenderAsOutput(ILabelContext lc);
	Boolean SupportsRendering(ILabelContext lc, ContentFormat from, ContentFormat to);
	String HandleScript(ILabelContext lc, IExpr expr);
	String HandleIterator(ILabelContext lc, IExprRow exprRow, ILabelElement dataElement);
	String HandleHard(String exprValue);
	String HandleScreen(ILabelContext lc, IExpr expr);
	String HandleFunction(ILabelContext lc, ILabelElement dataElement, IExpr expr);
	String HandleImage(ILabelContext lc, ILabelElement dataElement, IImageDriven imageDriven, IOrientable orientable);
	String HandleContent(ILabelContext lc, IContentable contentable);
	String HandleSubstitution(ILabelContext lc, ISubstitutable substitutable, String exprValue);
	ValueTuple<String,String> GetExpression(ILabelContext lc, IExprRow exprRow);
	ValueTuple<String,String> HandleRule(ILabelContext lc, IModelDetail modelExpr);
	String HandleBarcode(ILabelContext lc, IModelDetail modelExpr, IBarcodeable barcodable, String exprValue);
	ValueTuple<String,String> HandleHexEncoding(ILabelContext lc, String exprValue, IModelDetail modelExpr, String fieldDataCmd, String fieldSepCmd);
	String HandleGoto(ILabelContext lc, String exprType, IModelDetail modelExpr);
	String HandleColor(ILabelContext lc, IModelDetail modelExpr);
	String HandleFont(ILabelContext lc, IModelDetail modelExpr);
	String HandleJustification(ILabelContext lc, IModelDetail modelExpr);
	String HandleFieldReverse(ILabelContext lc, IModelDetail modelExpr);
	String HandleOrientation(ILabelContext lc, IModelDetail modelExpr, Boolean v);
	ValueTuple<String,String> HandleFieldData(ILabelContext lc, String exprValue);
	String HandleLineWrap(ILabelContext lc, String exprValue, IModelDetail modelExpr);
	String DrawGraphic(ILabelContext lc, IModelGraphic graphic);
	FileResult GetPrintResult(ILabelContext labelContext, Int32 nbCopies);



public class PresOption:AbstractOption<PresLanguage,PresOption>, ICmdOption<PresLanguage,PresOption>, ICmdOption
	String Code;
	String Description;
	ICmdConstraint<PresLanguage,PresOption> Constraint;
	C WithDefault(Object defaultValue);
	String ToString();



public class IPdfBrush



public class IPdfCanvas
	void Save();
	void Restore();
	void TranslateTransform(Single x, Single y);
	void RotateTransform(Single degrees);
	void DrawLine(IPdfPen pen, Single x1, Single y1, Single x2, Single y2);
	void DrawRectangle(IPdfPen pen, Single x, Single y, Single width, Single height);
	void DrawRectangle(IPdfBrush brush, Single x, Single y, Single width, Single height);
	void DrawRectangle(IPdfPen pen, IPdfBrush brush, Single x, Single y, Single width, Single height);
	void DrawEllipse(IPdfPen pen, Single x, Single y, Single width, Single height);
	void DrawEllipse(IPdfBrush brush, Single x, Single y, Single width, Single height);
	void DrawEllipse(IPdfPen pen, IPdfBrush brush, Single x, Single y, Single width, Single height);
	void DrawPath(IPdfPen pen, IPdfPath path);
	void DrawPath(IPdfBrush brush, IPdfPath path);
	void DrawPath(IPdfPen pen, IPdfBrush brush, IPdfPath path);
	void DrawString(String text, IPdfFont font, IPdfBrush brush, Single x, Single y);
	void DrawString(String text, IPdfFont font, IPdfBrush brush, Single x, Single y, Single width, Single height, IPdfStringFormat format);
	void DrawString(String text, IPdfFont font, IPdfBrush brush, RectangleF rectangle, IPdfStringFormat format);
	void DrawImage(IPdfImage image, Single x, Single y);
	void DrawImage(IPdfImage image, Single x, Single y, Single width, Single height);



public class IPdfDocument:IDisposable, IContextNamed
	Int32 PageCount;
	IPdfPage AddPage(Single widthPoints, Single heightPoints);
	IPdfPage GetPage(Int32 index);
	void SaveToStream(Stream stream);
	Image SaveAsImage(Int32 pageIndex, Int32 dpiX, Int32 dpiY);
	IPdfPage AddPage(SizeF pageSize);



public class IPdfFactory
	IPdfDocument CreateDocument();
	IPdfPen CreatePen(IColor color, Single width);
	IPdfBrush CreateBrush(IColor color);
	IPdfPath CreatePath();
	IPdfStringFormat CreateStringFormat();
	IPdfFont CreateStandardFont(Family family, FontStyle style, Single size);
	IPdfFont CreateTrueTypeFont(Byte[] fontData, Single size);
	IPdfFont CreateTrueTypeFont(Font systemFont, Single size);
	IPdfImage LoadImage(Byte[] imageData);



public class IPdfFont
	Single Size;
	SizeF MeasureString(String text);



public class IPdfImage:IDisposable
	Single Width;
	Single Height;



public class IPdfPage:IContextNamed
	IPdfCanvas Canvas;
	Single Width;
	Single Height;
	void SetRotation(Int32 degrees);



public class IPdfPath:IDisposable
	void AddLine(Single x1, Single y1, Single x2, Single y2);
	void AddArc(Single x, Single y, Single width, Single height, Single startAngle, Single sweepAngle);
	void CloseFigure();



public class IPdfPen
	Single Width;



public class IPdfStringFormat
	AAPdfTextAlignment Alignment;
	Single LineSpacing;



public class PdfStandardFont:Enum, IComparable, IFormattable, IConvertible
	Int32 value__;
	PdfStandardFont Helvetica;
	PdfStandardFont TimesRoman;
	PdfStandardFont Courier;
	PdfStandardFont Symbol;
	PdfStandardFont ZapfDingbats;
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



public class AAPdfTextAlignment:Enum, IComparable, IFormattable, IConvertible
	Int32 value__;
	AAPdfTextAlignment Left;
	AAPdfTextAlignment Center;
	AAPdfTextAlignment Right;
	AAPdfTextAlignment Justify;
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



public class PdfLanguage:AbstractLanguage<PdfLanguage,PdfOption>, IPrinterLanguage<PdfLanguage,PdfOption>, IPrinterLanguage, ISelectable, IRenderer
	String CODE;
	String Code;
	String Description;
	String StartLabel;
	String EndLabel;
	String StartCommand;
	String EndCommand;
	String ArgDelimiter;
	Boolean AllowsColors;
	String LineWrapValue;
	String Validate(ILabelContext context, String text);
	RenderResult RenderAsOutput(ILabelContext lc);
	FileResult GetPrintResult(ILabelContext labelContext, Int32 nbCopies);
	void ApplyRotation(IPdfCanvas canvas, IPdfPage page, Int32 degrees, IFormat format);
	ValueTuple<String,String> GetExpression(ILabelContext lc, IExprRow exprRow);
	String HandleHard(String exprValue);
	IPdfDocument GetPdfDocument(ILabelContext lc);
	IPdfCanvas GetCanvas(ILabelContext lc);
	LayoutPdf GetLayout(ILabelContext lc);
	IPdfPage GetPage(ILabelContext lc);
	IPdfPage AddPage(ILabelContext lc, IPdfDocument pdfDoc);
	String DrawString(ILabelContext lc, IModelDetail expr, String text);
	String DrawBarcode(ILabelContext lc, IModelDetail expr, String text, IBarcode barcode, IBarcodeOption[] options);
	Boolean NeedsRotation(String orientation);
	void ApplyImageRotation(IPdfCanvas canvas, String orientation, Single x, Single y, Single width, Single height);
	String DrawImage(ILabelContext lc, IModelDetail expr, IImageDriven imageDriven, IOrientable orientable);
	String DrawGraphic(ILabelContext lc, IModelGraphic graphic);
	void DrawRoundedRectangle(IPdfCanvas canvas, IPdfPen pen, IPdfBrush brush, Single minX, Single minY, Single width, Single height, Int32 zplRoundingValue);
	Single ConvertZplRoundingToRadius(Int32 zplRounding, Single width, Single height);
	void RenderIterator(ILabelContext lc, IModelDetail expr);
	void DrawJustifiedString(ILabelContext lc, String text, IPdfFont font, IPdfBrush brush, IJustification justification, Single xPoints, Single yPoints);
	IPdfFont GetPdfFont(ILabelContext lc, IModelDetail expr);
	IPdfBrush CreatePdfBrush(IColor color);
	IPdfPen CreatePdfPen(IColor color, Decimal thickness);
	Single GetRotationAngle(String orientation);
	Boolean TryGetBarcode(String barcodeType, IBarcodeCmd`2& bc);
	IList<IPrinterCmd> GetCommands();
	void AddCommand(IPrinterCmd cmd);
	String SetNbCopies(ILabelContext lc, String text, Int32 totalQty, Nullable<Int32> pauseCutValue, Nullable<Int32> replicateEachSerial, Nullable<Boolean> overridePauseCount);
	String AddPause(ILabelContext lc, String text);
	Int32 FindAfter(String text, IPrinterCmd cmd);
	Int32 FindBefore(String text, IPrinterCmd cmd);
	String HandleDensity(ILabelContext lc, String text);
	String HandleFonts(ILabelContext lc, String text);
	void AddFontsForRendering(ILabelContext lc);
	ISerialInfo GetSerialInfo(ILabelContext lc, String text);
	Boolean SupportsRendering(ILabelContext lc, ContentFormat from, ContentFormat to);
	String HandleScript(ILabelContext lc, IExpr expr);
	String HandleIterator(ILabelContext lc, IExprRow exprRow, ILabelElement dataElement);
	String HandleScreen(ILabelContext lc, IExpr expr);
	String HandleFunction(ILabelContext lc, ILabelElement dataElement, IExpr expr);
	String HandleImage(ILabelContext lc, ILabelElement dataElement, IImageDriven imageDriven, IOrientable orientable);
	String HandleContent(ILabelContext lc, IContentable contentable);
	String HandleSubstitution(ILabelContext lc, ISubstitutable substitutable, String exprValue);
	ValueTuple<String,String> HandleRule(ILabelContext lc, IModelDetail modelExpr);
	String HandleBarcode(ILabelContext lc, IModelDetail modelExpr, IBarcodeable barcodable, String exprValue);
	ValueTuple<String,String> HandleHexEncoding(ILabelContext lc, String exprValue, IModelDetail modelExpr, String fieldDataCmd, String fieldSepCmd);
	String HandleGoto(ILabelContext lc, String exprType, IModelDetail modelExpr);
	String HandleColor(ILabelContext lc, IModelDetail modelExpr);
	String HandleFont(ILabelContext lc, IModelDetail modelExpr);
	String HandleJustification(ILabelContext lc, IModelDetail modelExpr);
	String HandleFieldReverse(ILabelContext lc, IModelDetail modelExpr);
	String HandleOrientation(ILabelContext lc, IModelDetail modelExpr, Boolean v);
	ValueTuple<String,String> HandleFieldData(ILabelContext lc, String exprValue);
	String HandleLineWrap(ILabelContext lc, String exprValue, IModelDetail modelExpr);



public class PdfOption:AbstractOption<PdfLanguage,PdfOption>, ICmdOption<PdfLanguage,PdfOption>, ICmdOption
	String Code;
	String Description;
	ICmdConstraint<PdfLanguage,PdfOption> Constraint;
	C WithDefault(Object defaultValue);
	String ToString();



public class SpirePdfFactory:IPdfFactory
	IPdfDocument CreateDocument();
	IPdfPen CreatePen(IColor color, Single width);
	IPdfBrush CreateBrush(IColor color);
	IPdfPath CreatePath();
	IPdfStringFormat CreateStringFormat();
	IPdfFont CreateStandardFont(Family fontFamily, FontStyle fontStyle, Single size);
	IPdfFont CreateTrueTypeFont(Byte[] fontData, Single size);
	IPdfFont CreateTrueTypeFont(Font systemFont, Single size);
	IPdfImage LoadImage(Byte[] imageData);
	Font LoadFontFromBytes(Byte[] fontData, Single size);



public class SharpPdfFactory:IPdfFactory
	IPdfDocument CreateDocument();
	IPdfPen CreatePen(IColor color, Single width);
	IPdfBrush CreateBrush(IColor color);
	IPdfPath CreatePath();
	IPdfStringFormat CreateStringFormat();
	IPdfFont CreateStandardFont(Family fontFamily, FontStyle fontStyle, Single size);
	IPdfFont CreateTrueTypeFont(Byte[] fontData, Single size);
	IPdfFont CreateTrueTypeFont(Font systemFont, Single size);
	IPdfImage LoadImage(Byte[] imageData);
	XFontStyle ConvertFontStyle(FontStyle style);



public class CustomFontResolver:IFontResolver
	String RegisterFont(Byte[] fontData);
	FontResolverInfo ResolveTypeface(String familyName, Boolean isBold, Boolean isItalic);
	Byte[] GetFont(String faceName);



public class ArrayFunctions:ArrayFunctions, IDictionary<String,Object>, ICollection<KeyValuePair`2>, IEnumerable<KeyValuePair`2>, IEnumerable, IScriptObject, IDictionary, ICollection, IFormattable, IScribanLib
	String PREFIX;
	String Prefix;
	ScriptMemberImportFlags ImportFlags;
	Int32 Count;
	Boolean IsReadOnly;
	Object Item;
	ICollection<String> Keys;
	ICollection<Object> Values;
	ScriptArray ToArray(Object arg1, Object arg2, Object arg3, Object arg4, Object arg5, Object arg6);
	Object AtIndex(IEnumerable list, Int32 index);
	IEnumerable Add(IEnumerable list, Object value);
	IEnumerable AddRange(IEnumerable list1, IEnumerable list2);
	IEnumerable Compact(IEnumerable list);
	IEnumerable Concat(IEnumerable list1, IEnumerable list2);
	Object Cycle(TemplateContext context, SourceSpan span, IList list, Object group);
	Boolean Any(TemplateContext context, SourceSpan span, IEnumerable list, Object function, Object[] args);
	ScriptRange Each(TemplateContext context, SourceSpan span, IEnumerable list, Object function);
	ScriptRange Filter(TemplateContext context, SourceSpan span, IEnumerable list, Object function);
	Object First(IEnumerable list);
	IEnumerable InsertAt(IEnumerable list, Int32 index, Object value);
	String Join(TemplateContext context, SourceSpan span, IEnumerable list, String delimiter, Object function);
	Object Last(IEnumerable list);
	IEnumerable Limit(IEnumerable list, Int32 count);
	IEnumerable Map(TemplateContext context, SourceSpan span, Object list, String member);
	IEnumerable Offset(IEnumerable list, Int32 index);
	IList RemoveAt(IList list, Int32 index);
	IEnumerable Reverse(IEnumerable list);
	Int32 Size(IEnumerable list);
	IEnumerable Sort(TemplateContext context, SourceSpan span, Object list, String member);
	IEnumerable Uniq(IEnumerable list);
	Boolean Contains(IEnumerable list, Object item);
	void System.Collections.IDictionary.Add(Object key, Object value);
	void Clear();
	Boolean System.Collections.IDictionary.Contains(Object key);
	IDictionaryEnumerator System.Collections.IDictionary.GetEnumerator();
	void System.Collections.IDictionary.Remove(Object key);
	void System.Collections.ICollection.CopyTo(Array array, Int32 index);
	IEnumerable<String> GetMembers();
	Boolean Contains(String member);
	Boolean TryGetValue(TemplateContext context, SourceSpan span, String member, Object& value);
	T GetSafeValue(String name, T defaultValue);
	Boolean System.Collections.Generic.IDictionary<System.String,System.Object>.TryGetValue(String key, Object& value);
	Boolean CanWrite(String member);
	Boolean TrySetValue(TemplateContext context, SourceSpan span, String member, Object value, Boolean readOnly);
	void SetValue(String member, Object value, Boolean readOnly);
	void Add(String key, Object value);
	Boolean ContainsKey(String key);
	Boolean Remove(String member);
	void SetReadOnly(String member, Boolean readOnly);
	String ToString(String format, IFormatProvider formatProvider);
	String ToString(IFormatProvider formatProvider);
	String ToString();
	void CopyTo(ScriptObject dest);
	IScriptObject Clone(Boolean deep);
	IEnumerator<KeyValuePair`2> GetEnumerator();
	IEnumerator System.Collections.IEnumerable.GetEnumerator();
	ScriptObject From(Object obj);
	Boolean IsImportable(Object obj);
	void System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.Add(KeyValuePair<String,Object> item);
	Boolean System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.Contains(KeyValuePair<String,Object> item);
	void System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.CopyTo(KeyValuePair`2[] array, Int32 arrayIndex);
	Boolean System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.Remove(KeyValuePair<String,Object> item);



public class AsciiValues:ScriptObject, IDictionary<String,Object>, ICollection<KeyValuePair`2>, IEnumerable<KeyValuePair`2>, IEnumerable, IScriptObject, IDictionary, ICollection, IFormattable, IScribanLib
	String NUL;
	String SOH;
	String STX;
	String ETX;
	String EOT;
	String ENQ;
	String ACK;
	String BEL;
	String BS;
	String TAB;
	String LF;
	String VT;
	String FF;
	String CR;
	String SO;
	String SI;
	String DLE;
	String DC1;
	String DC2;
	String DC3;
	String DC4;
	String NAK;
	String SYN;
	String ETB;
	String CAN;
	String EM;
	String SUB;
	String ESC;
	String FS;
	String GS;
	String RS;
	String US;
	String SP;
	String DEL;
	String Prefix;
	ScriptMemberImportFlags ImportFlags;
	Int32 Count;
	Boolean IsReadOnly;
	Object Item;
	ICollection<String> Keys;
	ICollection<Object> Values;
	void System.Collections.IDictionary.Add(Object key, Object value);
	void Clear();
	Boolean System.Collections.IDictionary.Contains(Object key);
	IDictionaryEnumerator System.Collections.IDictionary.GetEnumerator();
	void System.Collections.IDictionary.Remove(Object key);
	void System.Collections.ICollection.CopyTo(Array array, Int32 index);
	IEnumerable<String> GetMembers();
	Boolean Contains(String member);
	Boolean TryGetValue(TemplateContext context, SourceSpan span, String member, Object& value);
	T GetSafeValue(String name, T defaultValue);
	Boolean System.Collections.Generic.IDictionary<System.String,System.Object>.TryGetValue(String key, Object& value);
	Boolean CanWrite(String member);
	Boolean TrySetValue(TemplateContext context, SourceSpan span, String member, Object value, Boolean readOnly);
	void SetValue(String member, Object value, Boolean readOnly);
	void Add(String key, Object value);
	Boolean ContainsKey(String key);
	Boolean Remove(String member);
	void SetReadOnly(String member, Boolean readOnly);
	String ToString(String format, IFormatProvider formatProvider);
	String ToString(IFormatProvider formatProvider);
	String ToString();
	void CopyTo(ScriptObject dest);
	IScriptObject Clone(Boolean deep);
	IEnumerator<KeyValuePair`2> GetEnumerator();
	IEnumerator System.Collections.IEnumerable.GetEnumerator();
	ScriptObject From(Object obj);
	Boolean IsImportable(Object obj);
	void System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.Add(KeyValuePair<String,Object> item);
	Boolean System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.Contains(KeyValuePair<String,Object> item);
	void System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.CopyTo(KeyValuePair`2[] array, Int32 arrayIndex);
	Boolean System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.Remove(KeyValuePair<String,Object> item);



public class BaseTemplateContext:TemplateContext, IFormatProvider
	Boolean DevMode;
	CultureInfo CurrentCulture;
	ITemplateLoader TemplateLoader;
	Boolean IsLiquid;
	Boolean AutoIndent;
	Boolean IndentOnEmptyLines;
	Boolean IndentWithInclude;
	Int32 LimitToString;
	Int32 ObjectRecursionLimit;
	String NewLine;
	ScriptLang Language;
	CancellationToken CancellationToken;
	ParserOptions TemplateLoaderParserOptions;
	LexerOptions TemplateLoaderLexerOptions;
	MemberRenamerDelegate MemberRenamer;
	MemberFilterDelegate MemberFilter;
	Int32 LoopLimit;
	Nullable<Int32> LoopLimitQueryable;
	Int32 RecursiveLimit;
	Boolean EnableOutput;
	IScriptOutput Output;
	Boolean UseScientific;
	Boolean ErrorForStatementFunctionAsExpression;
	ScriptObject BuiltinObject;
	IScriptObject CurrentGlobal;
	Dictionary<String,Template> CachedTemplates;
	String CurrentSourceFile;
	TryGetVariableDelegate TryGetVariable;
	RenderRuntimeExceptionDelegate RenderRuntimeException;
	TryGetMemberDelegate TryGetMember;
	Dictionary<Object,Object> Tags;
	Int32 GlobalCount;
	Int32 OutputCount;
	Int32 CultureCount;
	Int32 SourceFileCount;
	TimeSpan RegexTimeOut;
	Boolean StrictVariables;
	Boolean EnableBreakAndContinueAsReturnOutsideLoop;
	Boolean EnableRelaxedTargetAccess;
	Boolean EnableRelaxedMemberAccess;
	Boolean EnableRelaxedFunctionAccess;
	Boolean EnableRelaxedIndexerAccess;
	Boolean EnableNullIndexer;
	ScriptNode CurrentNode;
	SourceSpan CurrentSpan;
	String CurrentIndent;
	Object Evaluate(ScriptNode scriptNode, Boolean aliasReturnedFunction);
	Object LogBefore(ScriptNode scriptNode);
	void LogAfter(Object key, Object value);
	void SetLoopVariable(ScriptVariable variable, Object value);
	Boolean FailedTryGetMember(TemplateContext context, SourceSpan span, Object target, String member, Object& value);
	String FailedRenderRuntimeException(ScriptRuntimeException exception);
	IListAccessor GetListAccessor(Object target);
	IListAccessor GetListAccessorImpl(Object target, Type type);
	void ResetPreviousNewLine();
	String GetTemplatePathFromName(String templateName, ScriptNode callerContext);
	String ConvertTemplateNameToPath(String templateName, ScriptNode callerContext);
	Template GetOrCreateTemplate(String templatePath, ScriptNode callerContext);
	Template CreateTemplate(String templatePath, ScriptNode callerContext);
	String RenderTemplate(Template template, ScriptArray arguments, ScriptNode callerContext);
	Object GetFormat(Type formatType);
	Object IsEmpty(SourceSpan span, Object against);
	IList ToList(SourceSpan span, Object value);
	String ObjectToString(Object value, Boolean nested);
	Boolean ToBool(SourceSpan span, Object value);
	Int32 ToInt(SourceSpan span, Object value);
	String GetTypeName(Object value);
	T ToObject(SourceSpan span, Object value);
	Object ToObject(SourceSpan span, Object value, Type destinationType);
	void PushGlobal(IScriptObject scriptObject);
	void PushGlobalOnly(IScriptObject scriptObject);
	IScriptObject PopGlobalOnly();
	IScriptObject PopGlobal();
	void PushLocal();
	void PopLocal();
	void SetValue(ScriptVariable variable, Object value, Boolean asReadOnly);
	void SetValue(ScriptVariable variable, Object value, Boolean asReadOnly, Boolean force);
	void DeleteValue(ScriptVariable variable);
	void SetReadOnly(ScriptVariable variable, Boolean isReadOnly);
	Object GetValue(ScriptVariable variable);
	Object GetValue(ScriptVariableGlobal variable);
	ValueTuple<Boolean,Type> <ToObject>g__GetNullableInfo|273_0(Type destinationType);
	void CheckAbort();
	void PushCulture(CultureInfo culture);
	CultureInfo PopCulture();
	void PushPipeArguments();
	void ClearPipeArguments();
	List<ScriptExpression> GetOrCreateListOfScriptExpressions(Int32 capacity);
	void ReleaseListOfScriptExpressions(List<ScriptExpression> list);
	Object[] GetOrCreateReflectionArguments(Int32 length);
	void ReleaseReflectionArguments(Object[] reflectionArguments);
	void PopPipeArguments();
	void PushSourceFile(String sourceFile);
	String PopSourceFile();
	Object GetValue(ScriptExpression target);
	void SetValue(ScriptVariable variable, Boolean value);
	void Import(SourceSpan span, Object objectToImport);
	void SetValue(ScriptExpression target, Object value);
	void PushOutput();
	void PushOutput(IScriptOutput output);
	IScriptOutput PopOutput();
	TemplateContext Write(SourceSpan span, Object textAsObject);
	TemplateContext Write(String text);
	TemplateContext WriteLine();
	TemplateContext Write(ScriptStringSlice slice);
	TemplateContext Write(String text, Int32 startIndex, Int32 count);
	Object Evaluate(ScriptNode scriptNode);
	IObjectAccessor GetMemberAccessor(Object target);
	void Reset();
	IObjectAccessor GetMemberAccessorImpl(Object target);
	ScriptObject GetDefaultBuiltinObject();
	void EnterRecursive(ScriptNode node);
	void ExitRecursive(ScriptNode node);
	void EnterFunction(ScriptNode caller);
	void ExitFunction(ScriptNode caller);
	void EnterLoop(ScriptLoopStatementBase loop);
	void OnEnterLoop(ScriptLoopStatementBase loop);
	void ExitLoop(ScriptLoopStatementBase loop);
	void OnExitLoop(ScriptLoopStatementBase loop);
	Boolean StepLoop(ScriptLoopStatementBase loop, LoopType loopType);
	Boolean OnStepLoop(ScriptLoopStatementBase loop);
	void PushCase(Object caseValue);
	Object PeekCase();
	Object PopCase();



public class DateTimeFunctions:DateTimeFunctions, IDictionary<String,Object>, ICollection<KeyValuePair`2>, IEnumerable<KeyValuePair`2>, IEnumerable, IScriptObject, IDictionary, ICollection, IFormattable, IScriptCustomFunction, IScriptFunctionInfo, IScribanLib
	String PREFIX;
	String Prefix;
	ScriptMemberImportFlags ImportFlags;
	String Format;
	Int32 RequiredParameterCount;
	Int32 ParameterCount;
	ScriptVarParamKind VarParamKind;
	Type ReturnType;
	Int32 Count;
	Boolean IsReadOnly;
	Object Item;
	ICollection<String> Keys;
	ICollection<Object> Values;
	String ToString(TemplateContext context, Nullable<DateTime> date, String pattern, String culture);
	DateTime Now();
	DateTime AddDays(DateTime date, Double days);
	DateTime AddMonths(DateTime date, Int32 months);
	DateTime AddYears(DateTime date, Int32 years);
	DateTime AddHours(DateTime date, Double hours);
	DateTime AddMinutes(DateTime date, Double minutes);
	DateTime AddSeconds(DateTime date, Double seconds);
	DateTime AddMilliseconds(DateTime date, Double millis);
	Nullable<DateTime> Parse(TemplateContext context, String text, String pattern, String culture);
	String ParseToString(TemplateContext context, String text, String output_pattern, String output_culture, String input_pattern, String input_culture);
	IScriptObject Clone(Boolean deep);
	String ToString(Nullable<DateTime> datetime, String pattern, CultureInfo culture);
	Object Invoke(TemplateContext context, ScriptNode callerContext, ScriptArray arguments, ScriptBlockStatement blockStatement);
	ScriptParameterInfo GetParameterInfo(Int32 index);
	void System.Collections.IDictionary.Add(Object key, Object value);
	void Clear();
	Boolean System.Collections.IDictionary.Contains(Object key);
	IDictionaryEnumerator System.Collections.IDictionary.GetEnumerator();
	void System.Collections.IDictionary.Remove(Object key);
	void System.Collections.ICollection.CopyTo(Array array, Int32 index);
	IEnumerable<String> GetMembers();
	Boolean Contains(String member);
	Boolean TryGetValue(TemplateContext context, SourceSpan span, String member, Object& value);
	T GetSafeValue(String name, T defaultValue);
	Boolean System.Collections.Generic.IDictionary<System.String,System.Object>.TryGetValue(String key, Object& value);
	Boolean CanWrite(String member);
	Boolean TrySetValue(TemplateContext context, SourceSpan span, String member, Object value, Boolean readOnly);
	void SetValue(String member, Object value, Boolean readOnly);
	void Add(String key, Object value);
	Boolean ContainsKey(String key);
	Boolean Remove(String member);
	void SetReadOnly(String member, Boolean readOnly);
	String ToString(String format, IFormatProvider formatProvider);
	String ToString(IFormatProvider formatProvider);
	String ToString();
	void CopyTo(ScriptObject dest);
	IEnumerator<KeyValuePair`2> GetEnumerator();
	IEnumerator System.Collections.IEnumerable.GetEnumerator();
	ScriptObject From(Object obj);
	Boolean IsImportable(Object obj);
	void System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.Add(KeyValuePair<String,Object> item);
	Boolean System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.Contains(KeyValuePair<String,Object> item);
	void System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.CopyTo(KeyValuePair`2[] array, Int32 arrayIndex);
	Boolean System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.Remove(KeyValuePair<String,Object> item);



public class GenFuFunctions:ScriptObject, IDictionary<String,Object>, ICollection<KeyValuePair`2>, IEnumerable<KeyValuePair`2>, IEnumerable, IScriptObject, IDictionary, ICollection, IFormattable, IScribanLib
	String Prefix;
	ScriptMemberImportFlags ImportFlags;
	Int32 Count;
	Boolean IsReadOnly;
	Object Item;
	ICollection<String> Keys;
	ICollection<Object> Values;
	T GetMock(TemplateContext context);
	Object GetMock(TemplateContext context, String fullTypeName);
	Object CreateInstance(TemplateContext context, String fullTypeName, Boolean setRandomData);
	Object[] CreateArray(TemplateContext context, String fullTypeName, Boolean setRandomData, Int32 size);
	void System.Collections.IDictionary.Add(Object key, Object value);
	void Clear();
	Boolean System.Collections.IDictionary.Contains(Object key);
	IDictionaryEnumerator System.Collections.IDictionary.GetEnumerator();
	void System.Collections.IDictionary.Remove(Object key);
	void System.Collections.ICollection.CopyTo(Array array, Int32 index);
	IEnumerable<String> GetMembers();
	Boolean Contains(String member);
	Boolean TryGetValue(TemplateContext context, SourceSpan span, String member, Object& value);
	T GetSafeValue(String name, T defaultValue);
	Boolean System.Collections.Generic.IDictionary<System.String,System.Object>.TryGetValue(String key, Object& value);
	Boolean CanWrite(String member);
	Boolean TrySetValue(TemplateContext context, SourceSpan span, String member, Object value, Boolean readOnly);
	void SetValue(String member, Object value, Boolean readOnly);
	void Add(String key, Object value);
	Boolean ContainsKey(String key);
	Boolean Remove(String member);
	void SetReadOnly(String member, Boolean readOnly);
	String ToString(String format, IFormatProvider formatProvider);
	String ToString(IFormatProvider formatProvider);
	String ToString();
	void CopyTo(ScriptObject dest);
	IScriptObject Clone(Boolean deep);
	IEnumerator<KeyValuePair`2> GetEnumerator();
	IEnumerator System.Collections.IEnumerable.GetEnumerator();
	ScriptObject From(Object obj);
	Boolean IsImportable(Object obj);
	void System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.Add(KeyValuePair<String,Object> item);
	Boolean System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.Contains(KeyValuePair<String,Object> item);
	void System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.CopyTo(KeyValuePair`2[] array, Int32 arrayIndex);
	Boolean System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.Remove(KeyValuePair<String,Object> item);



public class HtmlFunctions:HtmlFunctions, IDictionary<String,Object>, ICollection<KeyValuePair`2>, IEnumerable<KeyValuePair`2>, IEnumerable, IScriptObject, IDictionary, ICollection, IFormattable, IScribanLib
	String B64;
	String Prefix;
	ScriptMemberImportFlags ImportFlags;
	Int32 Count;
	Boolean IsReadOnly;
	Object Item;
	ICollection<String> Keys;
	ICollection<Object> Values;
	String ToDataScheme(TemplateContext context, Object data);
	MemoryStream ToStream(Image image);
	String Strip(TemplateContext context, String text);
	String Escape(String text);
	String NewlineToBr(String text);
	String UrlEncode(String text);
	String UrlEscape(String text);
	void System.Collections.IDictionary.Add(Object key, Object value);
	void Clear();
	Boolean System.Collections.IDictionary.Contains(Object key);
	IDictionaryEnumerator System.Collections.IDictionary.GetEnumerator();
	void System.Collections.IDictionary.Remove(Object key);
	void System.Collections.ICollection.CopyTo(Array array, Int32 index);
	IEnumerable<String> GetMembers();
	Boolean Contains(String member);
	Boolean TryGetValue(TemplateContext context, SourceSpan span, String member, Object& value);
	T GetSafeValue(String name, T defaultValue);
	Boolean System.Collections.Generic.IDictionary<System.String,System.Object>.TryGetValue(String key, Object& value);
	Boolean CanWrite(String member);
	Boolean TrySetValue(TemplateContext context, SourceSpan span, String member, Object value, Boolean readOnly);
	void SetValue(String member, Object value, Boolean readOnly);
	void Add(String key, Object value);
	Boolean ContainsKey(String key);
	Boolean Remove(String member);
	void SetReadOnly(String member, Boolean readOnly);
	String ToString(String format, IFormatProvider formatProvider);
	String ToString(IFormatProvider formatProvider);
	String ToString();
	void CopyTo(ScriptObject dest);
	IScriptObject Clone(Boolean deep);
	IEnumerator<KeyValuePair`2> GetEnumerator();
	IEnumerator System.Collections.IEnumerable.GetEnumerator();
	ScriptObject From(Object obj);
	Boolean IsImportable(Object obj);
	void System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.Add(KeyValuePair<String,Object> item);
	Boolean System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.Contains(KeyValuePair<String,Object> item);
	void System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.CopyTo(KeyValuePair`2[] array, Int32 arrayIndex);
	Boolean System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.Remove(KeyValuePair<String,Object> item);



public class JsonFunctions:ScriptObject, IDictionary<String,Object>, ICollection<KeyValuePair`2>, IEnumerable<KeyValuePair`2>, IEnumerable, IScriptObject, IDictionary, ICollection, IFormattable, IScribanLib
	String Prefix;
	ScriptMemberImportFlags ImportFlags;
	Int32 Count;
	Boolean IsReadOnly;
	Object Item;
	ICollection<String> Keys;
	ICollection<Object> Values;
	String ToJson(TemplateContext context, Object obj);
	Object FromJson(TemplateContext context, String json, String fullTypeName);
	void System.Collections.IDictionary.Add(Object key, Object value);
	void Clear();
	Boolean System.Collections.IDictionary.Contains(Object key);
	IDictionaryEnumerator System.Collections.IDictionary.GetEnumerator();
	void System.Collections.IDictionary.Remove(Object key);
	void System.Collections.ICollection.CopyTo(Array array, Int32 index);
	IEnumerable<String> GetMembers();
	Boolean Contains(String member);
	Boolean TryGetValue(TemplateContext context, SourceSpan span, String member, Object& value);
	T GetSafeValue(String name, T defaultValue);
	Boolean System.Collections.Generic.IDictionary<System.String,System.Object>.TryGetValue(String key, Object& value);
	Boolean CanWrite(String member);
	Boolean TrySetValue(TemplateContext context, SourceSpan span, String member, Object value, Boolean readOnly);
	void SetValue(String member, Object value, Boolean readOnly);
	void Add(String key, Object value);
	Boolean ContainsKey(String key);
	Boolean Remove(String member);
	void SetReadOnly(String member, Boolean readOnly);
	String ToString(String format, IFormatProvider formatProvider);
	String ToString(IFormatProvider formatProvider);
	String ToString();
	void CopyTo(ScriptObject dest);
	IScriptObject Clone(Boolean deep);
	IEnumerator<KeyValuePair`2> GetEnumerator();
	IEnumerator System.Collections.IEnumerable.GetEnumerator();
	ScriptObject From(Object obj);
	Boolean IsImportable(Object obj);
	void System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.Add(KeyValuePair<String,Object> item);
	Boolean System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.Contains(KeyValuePair<String,Object> item);
	void System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.CopyTo(KeyValuePair`2[] array, Int32 arrayIndex);
	Boolean System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.Remove(KeyValuePair<String,Object> item);



public class LabelBuiltinFunctions:ScriptObject, IDictionary<String,Object>, ICollection<KeyValuePair`2>, IEnumerable<KeyValuePair`2>, IEnumerable, IScriptObject, IDictionary, ICollection, IFormattable
	Int32 Count;
	Boolean IsReadOnly;
	Object Item;
	ICollection<String> Keys;
	ICollection<Object> Values;
	void System.Collections.IDictionary.Add(Object key, Object value);
	void Clear();
	Boolean System.Collections.IDictionary.Contains(Object key);
	IDictionaryEnumerator System.Collections.IDictionary.GetEnumerator();
	void System.Collections.IDictionary.Remove(Object key);
	void System.Collections.ICollection.CopyTo(Array array, Int32 index);
	IEnumerable<String> GetMembers();
	Boolean Contains(String member);
	Boolean TryGetValue(TemplateContext context, SourceSpan span, String member, Object& value);
	T GetSafeValue(String name, T defaultValue);
	Boolean System.Collections.Generic.IDictionary<System.String,System.Object>.TryGetValue(String key, Object& value);
	Boolean CanWrite(String member);
	Boolean TrySetValue(TemplateContext context, SourceSpan span, String member, Object value, Boolean readOnly);
	void SetValue(String member, Object value, Boolean readOnly);
	void Add(String key, Object value);
	Boolean ContainsKey(String key);
	Boolean Remove(String member);
	void SetReadOnly(String member, Boolean readOnly);
	String ToString(String format, IFormatProvider formatProvider);
	String ToString(IFormatProvider formatProvider);
	String ToString();
	void CopyTo(ScriptObject dest);
	IScriptObject Clone(Boolean deep);
	IEnumerator<KeyValuePair`2> GetEnumerator();
	IEnumerator System.Collections.IEnumerable.GetEnumerator();
	ScriptObject From(Object obj);
	Boolean IsImportable(Object obj);
	void System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.Add(KeyValuePair<String,Object> item);
	Boolean System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.Contains(KeyValuePair<String,Object> item);
	void System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.CopyTo(KeyValuePair`2[] array, Int32 arrayIndex);
	Boolean System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.Remove(KeyValuePair<String,Object> item);



public class LabelFunctions:ScriptObject, IDictionary<String,Object>, ICollection<KeyValuePair`2>, IEnumerable<KeyValuePair`2>, IEnumerable, IScriptObject, IDictionary, ICollection, IFormattable, IScribanLib
	String Prefix;
	ScriptMemberImportFlags ImportFlags;
	Int32 Count;
	Boolean IsReadOnly;
	Object Item;
	ICollection<String> Keys;
	ICollection<Object> Values;
	String GetContent(TemplateContext context, Object contentIDOrName, Object barcodeIDOrName);
	Boolean IsNeeded(Boolean isForBarcode, String sequenceUsage);
	Boolean TryGetSequence(ILabelContext lc, IContent content, Nullable<Guid> sequenceID, String& sequence);
	void System.Collections.IDictionary.Add(Object key, Object value);
	void Clear();
	Boolean System.Collections.IDictionary.Contains(Object key);
	IDictionaryEnumerator System.Collections.IDictionary.GetEnumerator();
	void System.Collections.IDictionary.Remove(Object key);
	void System.Collections.ICollection.CopyTo(Array array, Int32 index);
	IEnumerable<String> GetMembers();
	Boolean Contains(String member);
	Boolean TryGetValue(TemplateContext context, SourceSpan span, String member, Object& value);
	T GetSafeValue(String name, T defaultValue);
	Boolean System.Collections.Generic.IDictionary<System.String,System.Object>.TryGetValue(String key, Object& value);
	Boolean CanWrite(String member);
	Boolean TrySetValue(TemplateContext context, SourceSpan span, String member, Object value, Boolean readOnly);
	void SetValue(String member, Object value, Boolean readOnly);
	void Add(String key, Object value);
	Boolean ContainsKey(String key);
	Boolean Remove(String member);
	void SetReadOnly(String member, Boolean readOnly);
	String ToString(String format, IFormatProvider formatProvider);
	String ToString(IFormatProvider formatProvider);
	String ToString();
	void CopyTo(ScriptObject dest);
	IScriptObject Clone(Boolean deep);
	IEnumerator<KeyValuePair`2> GetEnumerator();
	IEnumerator System.Collections.IEnumerable.GetEnumerator();
	ScriptObject From(Object obj);
	Boolean IsImportable(Object obj);
	void System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.Add(KeyValuePair<String,Object> item);
	Boolean System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.Contains(KeyValuePair<String,Object> item);
	void System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.CopyTo(KeyValuePair`2[] array, Int32 arrayIndex);
	Boolean System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.Remove(KeyValuePair<String,Object> item);



public class MathFunctions:MathFunctions, IDictionary<String,Object>, ICollection<KeyValuePair`2>, IEnumerable<KeyValuePair`2>, IEnumerable, IScriptObject, IDictionary, ICollection, IFormattable, IScribanLib
	String PREFIX;
	String Prefix;
	ScriptMemberImportFlags ImportFlags;
	Int32 Count;
	Boolean IsReadOnly;
	Object Item;
	ICollection<String> Keys;
	ICollection<Object> Values;
	Object Abs(TemplateContext context, SourceSpan span, Object value);
	Double Ceil(Double value);
	Object DividedBy(TemplateContext context, SourceSpan span, Double value, Object divisor);
	Double Floor(Double value);
	String Format(TemplateContext context, SourceSpan span, Object value, String format, String culture);
	Boolean IsNumber(Object value);
	Object Minus(TemplateContext context, SourceSpan span, Object value, Object with);
	Object Modulo(TemplateContext context, SourceSpan span, Object value, Object with);
	Object Plus(TemplateContext context, SourceSpan span, Object value, Object with);
	Double Round(Double value, Int32 precision);
	Object Times(TemplateContext context, SourceSpan span, Object value, Object with);
	String Uuid();
	Object Random(TemplateContext context, SourceSpan span, Int32 minValue, Int32 maxValue);
	void System.Collections.IDictionary.Add(Object key, Object value);
	void Clear();
	Boolean System.Collections.IDictionary.Contains(Object key);
	IDictionaryEnumerator System.Collections.IDictionary.GetEnumerator();
	void System.Collections.IDictionary.Remove(Object key);
	void System.Collections.ICollection.CopyTo(Array array, Int32 index);
	IEnumerable<String> GetMembers();
	Boolean Contains(String member);
	Boolean TryGetValue(TemplateContext context, SourceSpan span, String member, Object& value);
	T GetSafeValue(String name, T defaultValue);
	Boolean System.Collections.Generic.IDictionary<System.String,System.Object>.TryGetValue(String key, Object& value);
	Boolean CanWrite(String member);
	Boolean TrySetValue(TemplateContext context, SourceSpan span, String member, Object value, Boolean readOnly);
	void SetValue(String member, Object value, Boolean readOnly);
	void Add(String key, Object value);
	Boolean ContainsKey(String key);
	Boolean Remove(String member);
	void SetReadOnly(String member, Boolean readOnly);
	String ToString(String format, IFormatProvider formatProvider);
	String ToString(IFormatProvider formatProvider);
	String ToString();
	void CopyTo(ScriptObject dest);
	IScriptObject Clone(Boolean deep);
	IEnumerator<KeyValuePair`2> GetEnumerator();
	IEnumerator System.Collections.IEnumerable.GetEnumerator();
	ScriptObject From(Object obj);
	Boolean IsImportable(Object obj);
	void System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.Add(KeyValuePair<String,Object> item);
	Boolean System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.Contains(KeyValuePair<String,Object> item);
	void System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.CopyTo(KeyValuePair`2[] array, Int32 arrayIndex);
	Boolean System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.Remove(KeyValuePair<String,Object> item);



public class NewScribanUtils
	MemberRenamerDelegate PascalCase;
	void SetGlobalValues(TemplateContext templateContext, Object[] contextValues);
	void CheckTemplateErrors(TemplateContext templateContext, String errorType, String name, Template template);
	void ShowErrorLines(ILabelContext lc, Template template);
	T EvalExpr(Object obj, String scribanExpr, T defaultValue);
	T EvalExpr(IRuleEvalContext context, String scribanExpr, T defaultValue);
	T EvalExpr(TemplateContext scribanContext, String scribanExpr, T defaultValue);
	TemplateContext CreateTestContext(Object[] contextValues);
	void LoadLibraries(IScriptObject container);
	void Import(IScriptObject container, ScriptMemberImportFlags flags, MemberFilterDelegate filter, MemberRenamerDelegate renamer);
	void Import(IScriptObject container, String name, ScriptMemberImportFlags flags, MemberFilterDelegate filter, MemberRenamerDelegate renamer);
	void Import(Type libType, IScriptObject container, String name, ScriptMemberImportFlags flags, MemberFilterDelegate filter, MemberRenamerDelegate renamer);
	IEnumerable<MethodInfo> GetExposedMethods(L lib);
	Boolean IsNoArg(MethodInfo mi);



public class ObjectFunctions:ObjectFunctions, IDictionary<String,Object>, ICollection<KeyValuePair`2>, IEnumerable<KeyValuePair`2>, IEnumerable, IScriptObject, IDictionary, ICollection, IFormattable, IScribanLib
	String Prefix;
	ScriptMemberImportFlags ImportFlags;
	Int32 Count;
	Boolean IsReadOnly;
	Object Item;
	ICollection<String> Keys;
	ICollection<Object> Values;
	Object Default(Object value, Object default);
	Object Eval(TemplateContext context, SourceSpan span, Object value);
	Object EvalTemplate(TemplateContext context, SourceSpan span, Object value);
	String Format(TemplateContext context, SourceSpan span, Object value, String format, String culture);
	Boolean HasKey(IDictionary<String,Object> value, String key);
	Boolean HasValue(IDictionary<String,Object> value, String key);
	ScriptArray Keys(TemplateContext context, Object value);
	Int32 Size(Object value);
	String Typeof(Object value);
	String Kind(TemplateContext context, Object value);
	ScriptArray Values(TemplateContext context, Object value);
	void System.Collections.IDictionary.Add(Object key, Object value);
	void Clear();
	Boolean System.Collections.IDictionary.Contains(Object key);
	IDictionaryEnumerator System.Collections.IDictionary.GetEnumerator();
	void System.Collections.IDictionary.Remove(Object key);
	void System.Collections.ICollection.CopyTo(Array array, Int32 index);
	IEnumerable<String> GetMembers();
	Boolean Contains(String member);
	Boolean TryGetValue(TemplateContext context, SourceSpan span, String member, Object& value);
	T GetSafeValue(String name, T defaultValue);
	Boolean System.Collections.Generic.IDictionary<System.String,System.Object>.TryGetValue(String key, Object& value);
	Boolean CanWrite(String member);
	Boolean TrySetValue(TemplateContext context, SourceSpan span, String member, Object value, Boolean readOnly);
	void SetValue(String member, Object value, Boolean readOnly);
	void Add(String key, Object value);
	Boolean ContainsKey(String key);
	Boolean Remove(String member);
	void SetReadOnly(String member, Boolean readOnly);
	String ToString(String format, IFormatProvider formatProvider);
	String ToString(IFormatProvider formatProvider);
	String ToString();
	void CopyTo(ScriptObject dest);
	IScriptObject Clone(Boolean deep);
	IEnumerator<KeyValuePair`2> GetEnumerator();
	IEnumerator System.Collections.IEnumerable.GetEnumerator();
	ScriptObject From(Object obj);
	Boolean IsImportable(Object obj);
	void System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.Add(KeyValuePair<String,Object> item);
	Boolean System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.Contains(KeyValuePair<String,Object> item);
	void System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.CopyTo(KeyValuePair`2[] array, Int32 arrayIndex);
	Boolean System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.Remove(KeyValuePair<String,Object> item);



public class RegexFunctions:RegexFunctions, IDictionary<String,Object>, ICollection<KeyValuePair`2>, IEnumerable<KeyValuePair`2>, IEnumerable, IScriptObject, IDictionary, ICollection, IFormattable, IScribanLib
	String Prefix;
	ScriptMemberImportFlags ImportFlags;
	Int32 Count;
	Boolean IsReadOnly;
	Object Item;
	ICollection<String> Keys;
	ICollection<Object> Values;
	String Escape(String pattern);
	ScriptArray Match(TemplateContext context, String text, String pattern, String options);
	ScriptArray Matches(TemplateContext context, String text, String pattern, String options);
	String Replace(TemplateContext context, String text, String pattern, String replace, String options);
	ScriptArray Split(TemplateContext context, String text, String pattern, String options);
	String Unescape(String pattern);
	void System.Collections.IDictionary.Add(Object key, Object value);
	void Clear();
	Boolean System.Collections.IDictionary.Contains(Object key);
	IDictionaryEnumerator System.Collections.IDictionary.GetEnumerator();
	void System.Collections.IDictionary.Remove(Object key);
	void System.Collections.ICollection.CopyTo(Array array, Int32 index);
	IEnumerable<String> GetMembers();
	Boolean Contains(String member);
	Boolean TryGetValue(TemplateContext context, SourceSpan span, String member, Object& value);
	T GetSafeValue(String name, T defaultValue);
	Boolean System.Collections.Generic.IDictionary<System.String,System.Object>.TryGetValue(String key, Object& value);
	Boolean CanWrite(String member);
	Boolean TrySetValue(TemplateContext context, SourceSpan span, String member, Object value, Boolean readOnly);
	void SetValue(String member, Object value, Boolean readOnly);
	void Add(String key, Object value);
	Boolean ContainsKey(String key);
	Boolean Remove(String member);
	void SetReadOnly(String member, Boolean readOnly);
	String ToString(String format, IFormatProvider formatProvider);
	String ToString(IFormatProvider formatProvider);
	String ToString();
	void CopyTo(ScriptObject dest);
	IScriptObject Clone(Boolean deep);
	IEnumerator<KeyValuePair`2> GetEnumerator();
	IEnumerator System.Collections.IEnumerable.GetEnumerator();
	ScriptObject From(Object obj);
	Boolean IsImportable(Object obj);
	void System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.Add(KeyValuePair<String,Object> item);
	Boolean System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.Contains(KeyValuePair<String,Object> item);
	void System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.CopyTo(KeyValuePair`2[] array, Int32 arrayIndex);
	Boolean System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.Remove(KeyValuePair<String,Object> item);



public class ScribanCmd:AbstractCmd<ScribanLanguage,ScribanOption>, IPrinterCmd<ScribanLanguage,ScribanOption>, IPrinterCmd
	String Language;
	String ArgDelimiter;
	String StartCommand;
	String EndCommand;
	String Raw;
	String Code;
	String Description;
	ICmdOption`2[] Options;
	Int32 NbOptions;
	String GetCode(Type funcLib, String methodName);
	String GetDescription(Type funcLib, String methodName);
	ScribanOption[] GetOptions(Type funcLib, String methodName);
	MethodInfo GetMethodInfo(Type funcLib, String methodName);
	ICmdOption<ScribanLanguage,ScribanOption> GetOption(String optionCode);
	String ToString();
	String Render(ILabelContext lc, Object[] values);
	String ToString(ICmdOption`2[] options);
	String ClearDelimiters(String rendered);
	void AddBarcode(IBarcodeCmd<ScribanLanguage,ScribanOption> barcode);
	Boolean TryGetBarcode(String barcodeType, IBarcodeCmd`2& barcode);
	IEnumerable<IBarcodeCmd`2> GetBarcodes();
	T ConvertConstraint(ICmdConstraint<L1,O1> constraint);



public class ScribanConstraint:AbstractConstraint<ScribanLanguage,ScribanOption>, ICmdConstraint<ScribanLanguage,ScribanOption>, ICmdConstraint
	Object DefaultValue;
	CmdConstraintType ConstraintType;
	Exception CheckValid(IPrinterCmd<ScribanLanguage,ScribanOption> barcode, ICmdOption<ScribanLanguage,ScribanOption> option, Object newValue);
	Object Clean(Object oldValue);
	C WithDefault(Object defaultValue);



public class ScribanLanguage:AbstractLanguage<ScribanLanguage,ScribanOption>, IPrinterLanguage<ScribanLanguage,ScribanOption>, IPrinterLanguage, ISelectable, IRenderer
	String CODE;
	String Code;
	String Description;
	String StartLabel;
	String EndLabel;
	String StartCommand;
	String EndCommand;
	String ArgDelimiter;
	Boolean AllowsColors;
	String LineWrapValue;
	String Validate(ILabelContext lc, String text);
	Boolean TryGetBarcode(String barcodeType, IBarcodeCmd`2& bc);
	IList<IPrinterCmd> GetCommands();
	void AddCommand(IPrinterCmd cmd);
	String SetNbCopies(ILabelContext lc, String text, Int32 totalQty, Nullable<Int32> pauseCutValue, Nullable<Int32> replicateEachSerial, Nullable<Boolean> overridePauseCount);
	String AddPause(ILabelContext lc, String text);
	Int32 FindAfter(String text, IPrinterCmd cmd);
	Int32 FindBefore(String text, IPrinterCmd cmd);
	String HandleDensity(ILabelContext lc, String text);
	String HandleFonts(ILabelContext lc, String text);
	void AddFontsForRendering(ILabelContext lc);
	ISerialInfo GetSerialInfo(ILabelContext lc, String text);
	RenderResult RenderAsOutput(ILabelContext lc);
	Boolean SupportsRendering(ILabelContext lc, ContentFormat from, ContentFormat to);
	String HandleScript(ILabelContext lc, IExpr expr);
	String HandleIterator(ILabelContext lc, IExprRow exprRow, ILabelElement dataElement);
	String HandleHard(String exprValue);
	String HandleScreen(ILabelContext lc, IExpr expr);
	String HandleFunction(ILabelContext lc, ILabelElement dataElement, IExpr expr);
	String HandleImage(ILabelContext lc, ILabelElement dataElement, IImageDriven imageDriven, IOrientable orientable);
	String HandleContent(ILabelContext lc, IContentable contentable);
	String HandleSubstitution(ILabelContext lc, ISubstitutable substitutable, String exprValue);
	ValueTuple<String,String> GetExpression(ILabelContext lc, IExprRow exprRow);
	ValueTuple<String,String> HandleRule(ILabelContext lc, IModelDetail modelExpr);
	String HandleBarcode(ILabelContext lc, IModelDetail modelExpr, IBarcodeable barcodable, String exprValue);
	ValueTuple<String,String> HandleHexEncoding(ILabelContext lc, String exprValue, IModelDetail modelExpr, String fieldDataCmd, String fieldSepCmd);
	String HandleGoto(ILabelContext lc, String exprType, IModelDetail modelExpr);
	String HandleColor(ILabelContext lc, IModelDetail modelExpr);
	String HandleFont(ILabelContext lc, IModelDetail modelExpr);
	String HandleJustification(ILabelContext lc, IModelDetail modelExpr);
	String HandleFieldReverse(ILabelContext lc, IModelDetail modelExpr);
	String HandleOrientation(ILabelContext lc, IModelDetail modelExpr, Boolean v);
	ValueTuple<String,String> HandleFieldData(ILabelContext lc, String exprValue);
	String HandleLineWrap(ILabelContext lc, String exprValue, IModelDetail modelExpr);
	String DrawGraphic(ILabelContext lc, IModelGraphic graphic);
	FileResult GetPrintResult(ILabelContext labelContext, Int32 nbCopies);



public class ScribanOption:AbstractOption<ScribanLanguage,ScribanOption>, ICmdOption<ScribanLanguage,ScribanOption>, ICmdOption
	String Code;
	String Description;
	ICmdConstraint<ScribanLanguage,ScribanOption> Constraint;
	C WithDefault(Object defaultValue);
	String ToString();



public class StringFunctions:StringFunctions, IDictionary<String,Object>, ICollection<KeyValuePair`2>, IEnumerable<KeyValuePair`2>, IEnumerable, IScriptObject, IDictionary, ICollection, IFormattable, IScribanLib
	String Prefix;
	ScriptMemberImportFlags ImportFlags;
	Int32 Count;
	Boolean IsReadOnly;
	Object Item;
	ICollection<String> Keys;
	ICollection<Object> Values;
	String Escape(String text);
	String Append(String text, String with);
	String Capitalize(String text);
	String Capitalizewords(String text);
	Boolean Contains(String text, String value);
	Boolean Empty(String text);
	Boolean Whitespace(String text);
	String Downcase(String text);
	Boolean EndsWith(String text, String value);
	Boolean EqualsIgnoreCase(String text, String value);
	String Handleize(String text);
	String Literal(String text);
	String LStrip(String text);
	String Pluralize(Int32 number, String singular, String plural);
	String Prepend(String text, String by);
	String Remove(String text, String remove);
	String RemoveFirst(String text, String remove);
	String RemoveLast(String text, String remove);
	String Replace(String text, String match, String replace);
	String ReplaceFirst(String text, String match, String replace, Boolean fromEnd);
	String RStrip(String text);
	Int32 Size(String text);
	String Slice(String text, Int32 start, Nullable<Int32> length);
	String Slice1(String text, Int32 start, Int32 length);
	IEnumerable Split(String text, String match);
	Boolean StartsWith(String text, String value);
	String Strip(String text);
	String StripNewlines(String text);
	Object ToInt(TemplateContext context, String text);
	Object ToLong(TemplateContext context, String text);
	Object ToFloat(TemplateContext context, String text);
	Object ToDouble(TemplateContext context, String text);
	String Truncate(String text, Int32 length, String ellipsis);
	String Truncatewords(String text, Int32 count, String ellipsis);
	String Upcase(String text);
	String Md5(String text);
	String Sha1(String text);
	String Sha256(String text);
	String Sha512(String text);
	String HmacSha1(String text, String secretKey);
	String HmacSha256(String text, String secretKey);
	String HmacSha512(String text, String secretKey);
	String PadLeft(String text, Int32 width);
	String PadRight(String text, Int32 width);
	String Base64Encode(String text);
	String Base64Decode(String text);
	Int32 IndexOf(String text, String search, Nullable<Int32> startIndex, Nullable<Int32> count, String stringComparison);
	void System.Collections.IDictionary.Add(Object key, Object value);
	void Clear();
	Boolean System.Collections.IDictionary.Contains(Object key);
	IDictionaryEnumerator System.Collections.IDictionary.GetEnumerator();
	void System.Collections.IDictionary.Remove(Object key);
	void System.Collections.ICollection.CopyTo(Array array, Int32 index);
	IEnumerable<String> GetMembers();
	Boolean Contains(String member);
	Boolean TryGetValue(TemplateContext context, SourceSpan span, String member, Object& value);
	T GetSafeValue(String name, T defaultValue);
	Boolean System.Collections.Generic.IDictionary<System.String,System.Object>.TryGetValue(String key, Object& value);
	Boolean CanWrite(String member);
	Boolean TrySetValue(TemplateContext context, SourceSpan span, String member, Object value, Boolean readOnly);
	void SetValue(String member, Object value, Boolean readOnly);
	void Add(String key, Object value);
	Boolean ContainsKey(String key);
	Boolean Remove(String member);
	void SetReadOnly(String member, Boolean readOnly);
	String ToString(String format, IFormatProvider formatProvider);
	String ToString(IFormatProvider formatProvider);
	String ToString();
	void CopyTo(ScriptObject dest);
	IScriptObject Clone(Boolean deep);
	IEnumerator<KeyValuePair`2> GetEnumerator();
	IEnumerator System.Collections.IEnumerable.GetEnumerator();
	ScriptObject From(Object obj);
	Boolean IsImportable(Object obj);
	void System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.Add(KeyValuePair<String,Object> item);
	Boolean System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.Contains(KeyValuePair<String,Object> item);
	void System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.CopyTo(KeyValuePair`2[] array, Int32 arrayIndex);
	Boolean System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.Remove(KeyValuePair<String,Object> item);



public class TimeSpanFunctions:TimeSpanFunctions, IDictionary<String,Object>, ICollection<KeyValuePair`2>, IEnumerable<KeyValuePair`2>, IEnumerable, IScriptObject, IDictionary, ICollection, IFormattable, IScribanLib
	String Prefix;
	ScriptMemberImportFlags ImportFlags;
	Int32 Count;
	Boolean IsReadOnly;
	Object Item;
	ICollection<String> Keys;
	ICollection<Object> Values;
	TimeSpan FromDays(Double days);
	TimeSpan FromHours(Double hours);
	TimeSpan FromMinutes(Double minutes);
	TimeSpan FromSeconds(Double seconds);
	TimeSpan FromMilliseconds(Double millis);
	TimeSpan Parse(String text);
	void System.Collections.IDictionary.Add(Object key, Object value);
	void Clear();
	Boolean System.Collections.IDictionary.Contains(Object key);
	IDictionaryEnumerator System.Collections.IDictionary.GetEnumerator();
	void System.Collections.IDictionary.Remove(Object key);
	void System.Collections.ICollection.CopyTo(Array array, Int32 index);
	IEnumerable<String> GetMembers();
	Boolean Contains(String member);
	Boolean TryGetValue(TemplateContext context, SourceSpan span, String member, Object& value);
	T GetSafeValue(String name, T defaultValue);
	Boolean System.Collections.Generic.IDictionary<System.String,System.Object>.TryGetValue(String key, Object& value);
	Boolean CanWrite(String member);
	Boolean TrySetValue(TemplateContext context, SourceSpan span, String member, Object value, Boolean readOnly);
	void SetValue(String member, Object value, Boolean readOnly);
	void Add(String key, Object value);
	Boolean ContainsKey(String key);
	Boolean Remove(String member);
	void SetReadOnly(String member, Boolean readOnly);
	String ToString(String format, IFormatProvider formatProvider);
	String ToString(IFormatProvider formatProvider);
	String ToString();
	void CopyTo(ScriptObject dest);
	IScriptObject Clone(Boolean deep);
	IEnumerator<KeyValuePair`2> GetEnumerator();
	IEnumerator System.Collections.IEnumerable.GetEnumerator();
	ScriptObject From(Object obj);
	Boolean IsImportable(Object obj);
	void System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.Add(KeyValuePair<String,Object> item);
	Boolean System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.Contains(KeyValuePair<String,Object> item);
	void System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.CopyTo(KeyValuePair`2[] array, Int32 arrayIndex);
	Boolean System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.Remove(KeyValuePair<String,Object> item);



public class MpcBarcodeCmd:MpcCmd, IPrinterCmd<MpcLanguage,MpcOption>, IPrinterCmd, IBarcodeCmd<MpcLanguage,MpcOption>, ILanguageDriven
	String PREFIX;
	String BarcodeType;
	String Name;
	String Dimension;
	String Code;
	String Language;
	String ArgDelimiter;
	String StartCommand;
	String EndCommand;
	String Raw;
	String Description;
	ICmdOption`2[] Options;
	Int32 NbOptions;
	ICmdOption<MpcLanguage,MpcOption> GetOption(String optionCode);
	String ToString();
	String Render(ILabelContext lc, Object[] values);
	String ToString(ICmdOption`2[] options);
	String ClearDelimiters(String rendered);
	void AddBarcode(IBarcodeCmd<MpcLanguage,MpcOption> barcode);
	Boolean TryGetBarcode(String barcodeType, IBarcodeCmd`2& barcode);
	IEnumerable<IBarcodeCmd`2> GetBarcodes();
	T ConvertConstraint(ICmdConstraint<L1,O1> constraint);



public class MpcCmd:AbstractCmd<MpcLanguage,MpcOption>, IPrinterCmd<MpcLanguage,MpcOption>, IPrinterCmd
	String Language;
	String ArgDelimiter;
	String StartCommand;
	String EndCommand;
	String Raw;
	String Code;
	String Description;
	ICmdOption`2[] Options;
	Int32 NbOptions;
	ICmdOption<MpcLanguage,MpcOption> GetOption(String optionCode);
	String ToString();
	String Render(ILabelContext lc, Object[] values);
	String ToString(ICmdOption`2[] options);
	String ClearDelimiters(String rendered);
	void AddBarcode(IBarcodeCmd<MpcLanguage,MpcOption> barcode);
	Boolean TryGetBarcode(String barcodeType, IBarcodeCmd`2& barcode);
	IEnumerable<IBarcodeCmd`2> GetBarcodes();
	T ConvertConstraint(ICmdConstraint<L1,O1> constraint);



public class MpcConstraint:AbstractConstraint<MpcLanguage,MpcOption>, ICmdConstraint<MpcLanguage,MpcOption>, ICmdConstraint
	Object DefaultValue;
	CmdConstraintType ConstraintType;
	Exception CheckValid(IPrinterCmd<MpcLanguage,MpcOption> barcode, ICmdOption<MpcLanguage,MpcOption> option, Object newValue);
	Object Clean(Object oldValue);
	C WithDefault(Object defaultValue);



public class MpcLanguage:AbstractLanguage<MpcLanguage,MpcOption>, IPrinterLanguage<MpcLanguage,MpcOption>, IPrinterLanguage, ISelectable, IRenderer
	String CODE;
	String Code;
	String Description;
	String StartLabel;
	String EndLabel;
	String StartCommand;
	String EndCommand;
	String ArgDelimiter;
	Boolean AllowsColors;
	String LineWrapValue;
	String Validate(ILabelContext lc, String text);
	String HandleBarcode(ILabelContext lc, IModelDetail modelExpr, IBarcodeable expr, String exprValue);
	Object GetBarcodeType(ILabelContext lc, IBarcode barcode);
	String GetGS1SubType(IBarcode barcode);
	String GetFixedOrVariable(IBarcode barcode);
	String HandleHard(ILabelContext lc, IModelDetail modelExpr, String text);
	ValueTuple<Int32,Int32> GetRowAndColumn(ILabelContext lc, IModelDetail expr);
	Boolean TryGetBarcode(String barcodeType, IBarcodeCmd`2& bc);
	IList<IPrinterCmd> GetCommands();
	void AddCommand(IPrinterCmd cmd);
	String SetNbCopies(ILabelContext lc, String text, Int32 totalQty, Nullable<Int32> pauseCutValue, Nullable<Int32> replicateEachSerial, Nullable<Boolean> overridePauseCount);
	String AddPause(ILabelContext lc, String text);
	Int32 FindAfter(String text, IPrinterCmd cmd);
	Int32 FindBefore(String text, IPrinterCmd cmd);
	String HandleDensity(ILabelContext lc, String text);
	String HandleFonts(ILabelContext lc, String text);
	void AddFontsForRendering(ILabelContext lc);
	ISerialInfo GetSerialInfo(ILabelContext lc, String text);
	RenderResult RenderAsOutput(ILabelContext lc);
	Boolean SupportsRendering(ILabelContext lc, ContentFormat from, ContentFormat to);
	String HandleScript(ILabelContext lc, IExpr expr);
	String HandleIterator(ILabelContext lc, IExprRow exprRow, ILabelElement dataElement);
	String HandleHard(String exprValue);
	String HandleScreen(ILabelContext lc, IExpr expr);
	String HandleFunction(ILabelContext lc, ILabelElement dataElement, IExpr expr);
	String HandleImage(ILabelContext lc, ILabelElement dataElement, IImageDriven imageDriven, IOrientable orientable);
	String HandleContent(ILabelContext lc, IContentable contentable);
	String HandleSubstitution(ILabelContext lc, ISubstitutable substitutable, String exprValue);
	ValueTuple<String,String> GetExpression(ILabelContext lc, IExprRow exprRow);
	ValueTuple<String,String> HandleRule(ILabelContext lc, IModelDetail modelExpr);
	ValueTuple<String,String> HandleHexEncoding(ILabelContext lc, String exprValue, IModelDetail modelExpr, String fieldDataCmd, String fieldSepCmd);
	String HandleGoto(ILabelContext lc, String exprType, IModelDetail modelExpr);
	String HandleColor(ILabelContext lc, IModelDetail modelExpr);
	String HandleFont(ILabelContext lc, IModelDetail modelExpr);
	String HandleJustification(ILabelContext lc, IModelDetail modelExpr);
	String HandleFieldReverse(ILabelContext lc, IModelDetail modelExpr);
	String HandleOrientation(ILabelContext lc, IModelDetail modelExpr, Boolean v);
	ValueTuple<String,String> HandleFieldData(ILabelContext lc, String exprValue);
	String HandleLineWrap(ILabelContext lc, String exprValue, IModelDetail modelExpr);
	String DrawGraphic(ILabelContext lc, IModelGraphic graphic);
	FileResult GetPrintResult(ILabelContext labelContext, Int32 nbCopies);



public class MpcOption:AbstractOption<MpcLanguage,MpcOption>, ICmdOption<MpcLanguage,MpcOption>, ICmdOption
	String Code;
	String Description;
	ICmdConstraint<MpcLanguage,MpcOption> Constraint;
	C WithDefault(Object defaultValue);
	String ToString();



public class IplLanguage:AbstractLanguage<IplLanguage,IplOption>, IPrinterLanguage<IplLanguage,IplOption>, IPrinterLanguage, ISelectable, IRenderer
	String CODE;
	String Code;
	String Description;
	String StartLabel;
	String EndLabel;
	String StartCommand;
	String EndCommand;
	String ArgDelimiter;
	Boolean AllowsColors;
	String LineWrapValue;
	String Validate(ILabelContext context, String text);
	Boolean TryGetBarcode(String barcodeType, IBarcodeCmd`2& bc);
	IList<IPrinterCmd> GetCommands();
	void AddCommand(IPrinterCmd cmd);
	String SetNbCopies(ILabelContext lc, String text, Int32 totalQty, Nullable<Int32> pauseCutValue, Nullable<Int32> replicateEachSerial, Nullable<Boolean> overridePauseCount);
	String AddPause(ILabelContext lc, String text);
	Int32 FindAfter(String text, IPrinterCmd cmd);
	Int32 FindBefore(String text, IPrinterCmd cmd);
	String HandleDensity(ILabelContext lc, String text);
	String HandleFonts(ILabelContext lc, String text);
	void AddFontsForRendering(ILabelContext lc);
	ISerialInfo GetSerialInfo(ILabelContext lc, String text);
	RenderResult RenderAsOutput(ILabelContext lc);
	Boolean SupportsRendering(ILabelContext lc, ContentFormat from, ContentFormat to);
	String HandleScript(ILabelContext lc, IExpr expr);
	String HandleIterator(ILabelContext lc, IExprRow exprRow, ILabelElement dataElement);
	String HandleHard(String exprValue);
	String HandleScreen(ILabelContext lc, IExpr expr);
	String HandleFunction(ILabelContext lc, ILabelElement dataElement, IExpr expr);
	String HandleImage(ILabelContext lc, ILabelElement dataElement, IImageDriven imageDriven, IOrientable orientable);
	String HandleContent(ILabelContext lc, IContentable contentable);
	String HandleSubstitution(ILabelContext lc, ISubstitutable substitutable, String exprValue);
	ValueTuple<String,String> GetExpression(ILabelContext lc, IExprRow exprRow);
	ValueTuple<String,String> HandleRule(ILabelContext lc, IModelDetail modelExpr);
	String HandleBarcode(ILabelContext lc, IModelDetail modelExpr, IBarcodeable barcodable, String exprValue);
	ValueTuple<String,String> HandleHexEncoding(ILabelContext lc, String exprValue, IModelDetail modelExpr, String fieldDataCmd, String fieldSepCmd);
	String HandleGoto(ILabelContext lc, String exprType, IModelDetail modelExpr);
	String HandleColor(ILabelContext lc, IModelDetail modelExpr);
	String HandleFont(ILabelContext lc, IModelDetail modelExpr);
	String HandleJustification(ILabelContext lc, IModelDetail modelExpr);
	String HandleFieldReverse(ILabelContext lc, IModelDetail modelExpr);
	String HandleOrientation(ILabelContext lc, IModelDetail modelExpr, Boolean v);
	ValueTuple<String,String> HandleFieldData(ILabelContext lc, String exprValue);
	String HandleLineWrap(ILabelContext lc, String exprValue, IModelDetail modelExpr);
	String DrawGraphic(ILabelContext lc, IModelGraphic graphic);
	FileResult GetPrintResult(ILabelContext labelContext, Int32 nbCopies);



public class IplOption:AbstractOption<IplLanguage,IplOption>, ICmdOption<IplLanguage,IplOption>, ICmdOption
	String Code;
	String Description;
	ICmdConstraint<IplLanguage,IplOption> Constraint;
	C WithDefault(Object defaultValue);
	String ToString();



public class FglCmd:AbstractCmd<FglLanguage,FglOption>, IPrinterCmd<FglLanguage,FglOption>, IPrinterCmd
	String Language;
	String ArgDelimiter;
	String StartCommand;
	String EndCommand;
	String Raw;
	String Code;
	String Description;
	ICmdOption`2[] Options;
	Int32 NbOptions;
	ICmdOption<FglLanguage,FglOption> GetOption(String optionCode);
	String ToString();
	String Render(ILabelContext lc, Object[] values);
	String ToString(ICmdOption`2[] options);
	String ClearDelimiters(String rendered);
	void AddBarcode(IBarcodeCmd<FglLanguage,FglOption> barcode);
	Boolean TryGetBarcode(String barcodeType, IBarcodeCmd`2& barcode);
	IEnumerable<IBarcodeCmd`2> GetBarcodes();
	T ConvertConstraint(ICmdConstraint<L1,O1> constraint);



public class FglConstraint:AbstractRangeConstraint<FglLanguage,FglOption>, ICmdConstraint<FglLanguage,FglOption>, ICmdConstraint
	Object DefaultValue;
	CmdConstraintType ConstraintType;
	Object Clean(Object newValue);
	Exception CheckValid(IPrinterCmd<FglLanguage,FglOption> barcode, ICmdOption<FglLanguage,FglOption> option, Object newValue);
	String ToString();
	C WithDefault(Object defaultValue);



public class FglLanguage:AbstractLanguage<FglLanguage,FglOption>, IPrinterLanguage<FglLanguage,FglOption>, IPrinterLanguage, ISelectable, IRenderer
	String CODE;
	String Code;
	String Description;
	String StartLabel;
	String EndLabel;
	String StartCommand;
	String EndCommand;
	String ArgDelimiter;
	Boolean AllowsColors;
	String LineWrapValue;
	String Validate(ILabelContext context, String text);
	Boolean TryGetBarcode(String barcodeType, IBarcodeCmd`2& bc);
	IList<IPrinterCmd> GetCommands();
	void AddCommand(IPrinterCmd cmd);
	String SetNbCopies(ILabelContext lc, String text, Int32 totalQty, Nullable<Int32> pauseCutValue, Nullable<Int32> replicateEachSerial, Nullable<Boolean> overridePauseCount);
	String AddPause(ILabelContext lc, String text);
	Int32 FindAfter(String text, IPrinterCmd cmd);
	Int32 FindBefore(String text, IPrinterCmd cmd);
	String HandleDensity(ILabelContext lc, String text);
	String HandleFonts(ILabelContext lc, String text);
	void AddFontsForRendering(ILabelContext lc);
	ISerialInfo GetSerialInfo(ILabelContext lc, String text);
	RenderResult RenderAsOutput(ILabelContext lc);
	Boolean SupportsRendering(ILabelContext lc, ContentFormat from, ContentFormat to);
	String HandleScript(ILabelContext lc, IExpr expr);
	String HandleIterator(ILabelContext lc, IExprRow exprRow, ILabelElement dataElement);
	String HandleHard(String exprValue);
	String HandleScreen(ILabelContext lc, IExpr expr);
	String HandleFunction(ILabelContext lc, ILabelElement dataElement, IExpr expr);
	String HandleImage(ILabelContext lc, ILabelElement dataElement, IImageDriven imageDriven, IOrientable orientable);
	String HandleContent(ILabelContext lc, IContentable contentable);
	String HandleSubstitution(ILabelContext lc, ISubstitutable substitutable, String exprValue);
	ValueTuple<String,String> GetExpression(ILabelContext lc, IExprRow exprRow);
	ValueTuple<String,String> HandleRule(ILabelContext lc, IModelDetail modelExpr);
	String HandleBarcode(ILabelContext lc, IModelDetail modelExpr, IBarcodeable barcodable, String exprValue);
	ValueTuple<String,String> HandleHexEncoding(ILabelContext lc, String exprValue, IModelDetail modelExpr, String fieldDataCmd, String fieldSepCmd);
	String HandleGoto(ILabelContext lc, String exprType, IModelDetail modelExpr);
	String HandleColor(ILabelContext lc, IModelDetail modelExpr);
	String HandleFont(ILabelContext lc, IModelDetail modelExpr);
	String HandleJustification(ILabelContext lc, IModelDetail modelExpr);
	String HandleFieldReverse(ILabelContext lc, IModelDetail modelExpr);
	String HandleOrientation(ILabelContext lc, IModelDetail modelExpr, Boolean v);
	ValueTuple<String,String> HandleFieldData(ILabelContext lc, String exprValue);
	String HandleLineWrap(ILabelContext lc, String exprValue, IModelDetail modelExpr);
	String DrawGraphic(ILabelContext lc, IModelGraphic graphic);
	FileResult GetPrintResult(ILabelContext labelContext, Int32 nbCopies);



public class FglOption:AbstractOption<FglLanguage,FglOption>, ICmdOption<FglLanguage,FglOption>, ICmdOption
	String Code;
	String Description;
	ICmdConstraint<FglLanguage,FglOption> Constraint;
	C WithDefault(Object defaultValue);
	String ToString();



public class EzpBarcodeCmd:EzpCmd, IPrinterCmd<EzpLanguage,EzpOption>, IPrinterCmd, IBarcodeCmd<EzpLanguage,EzpOption>, ILanguageDriven
	String BarcodeType;
	String Name;
	String Dimension;
	String Code;
	String Language;
	String ArgDelimiter;
	String StartCommand;
	String EndCommand;
	String Raw;
	String Description;
	ICmdOption`2[] Options;
	Int32 NbOptions;
	EzpOption[] ToEzp(ICmdOption`2[] cmdArgs);
	EzpConstraint ToEzp(ICmdConstraint<ZplLanguage,ZplOption> zplConstraint);
	ICmdOption<EzpLanguage,EzpOption> GetOption(String optionCode);
	String ToString();
	String Render(ILabelContext lc, Object[] values);
	String ToString(ICmdOption`2[] options);
	String ClearDelimiters(String rendered);
	void AddBarcode(IBarcodeCmd<EzpLanguage,EzpOption> barcode);
	Boolean TryGetBarcode(String barcodeType, IBarcodeCmd`2& barcode);
	IEnumerable<IBarcodeCmd`2> GetBarcodes();
	T ConvertConstraint(ICmdConstraint<L1,O1> constraint);



public class EzpColorCmd:EzpCmd, IPrinterCmd<EzpLanguage,EzpOption>, IPrinterCmd
	EzpConstraint RGBA;
	String Language;
	String ArgDelimiter;
	String StartCommand;
	String EndCommand;
	String Raw;
	String Code;
	String Description;
	ICmdOption`2[] Options;
	Int32 NbOptions;
	String ChangeColor(ILabelContext lc, IColor fore, IColor back, String foreReversal, String backReversal);
	ICmdOption<EzpLanguage,EzpOption> GetOption(String optionCode);
	String ToString();
	String Render(ILabelContext lc, Object[] values);
	String ToString(ICmdOption`2[] options);
	String ClearDelimiters(String rendered);
	void AddBarcode(IBarcodeCmd<EzpLanguage,EzpOption> barcode);
	Boolean TryGetBarcode(String barcodeType, IBarcodeCmd`2& barcode);
	IEnumerable<IBarcodeCmd`2> GetBarcodes();
	T ConvertConstraint(ICmdConstraint<L1,O1> constraint);



public class EzpOperationCmd:EzpCmd, IPrinterCmd<EzpLanguage,EzpOption>, IPrinterCmd
	String Language;
	String ArgDelimiter;
	String StartCommand;
	String EndCommand;
	String Raw;
	String Code;
	String Description;
	ICmdOption`2[] Options;
	Int32 NbOptions;
	String Render(ILabelContext lc, Object[] values);
	String SetMediaType(ILabelContext lc, String mediaType);
	String SetMediaForm(ILabelContext lc, String mediaForm);
	String SetMediaSource(ILabelContext lc, String mediaSource);
	String SetMediaShape(ILabelContext lc, String mediaShape);
	String SetEdgeDetection(ILabelContext lc, String edgeDetection);
	String SetLabelWidth(ILabelContext lc, Int32 dots);
	String SetLabelLength(ILabelContext lc, Int32 dots);
	String SetMediaGap(ILabelContext lc, Int32 dots);
	String SetPrintMode(ILabelContext lc, String printMode);
	String SetFormatResolution(ILabelContext lc, Int32 dpi);
	String SetPrintResolution(ILabelContext lc, Int32 dpi);
	ICmdOption<EzpLanguage,EzpOption> GetOption(String optionCode);
	String ToString();
	String ToString(ICmdOption`2[] options);
	String ClearDelimiters(String rendered);
	void AddBarcode(IBarcodeCmd<EzpLanguage,EzpOption> barcode);
	Boolean TryGetBarcode(String barcodeType, IBarcodeCmd`2& barcode);
	IEnumerable<IBarcodeCmd`2> GetBarcodes();
	T ConvertConstraint(ICmdConstraint<L1,O1> constraint);



public class ImageCorrection
	String Saturation;
	String BandingReduction;
	String ColorCorrectionMode;
	String InkProfileAndBrightness;
	String EnableDisableFeatherEdge;
	String SpotColorList;
	String ToneYellow;
	String ToneMagenta;
	String ToneCyan;
	String Contrast;
	String RatioBlackToComposite;
	String PrintQuality;
	String Brightness;
	String YShiftUserAdjustment;



public class LabelMedia
	String MediaCoatingType;
	String MediaForm;
	String EdgeDetection;
	String MediaSource;
	String MediaShape;



public class LabelResolution
	String BackgroundImage;
	String PrintResolutionMagnification;
	String PrintResolution;
	String FormatBase;
	String PrintResolutionReplacedPrinter;



public class MediaSetting
	String LeftGap;
	String LabelLength;
	String LabelWidth;
	String GapBetweenLabels;



public class OperationMode
	String BarcodeSize;
	String BarWidth;
	String AliasForDrive;
	String CodeConversionTable;
	String LabelBackgroundImage;
	String LabelPaperEdge;
	String LabelMedia;
	String LabelResolution;
	String MediaSetting;
	String ImageCorrection;
	String PrintMode;



public class PrintMode
	String PrintOperationMode;
	String PrintSpeed;
	String FlushOntoPaperMode;
	String BasicPrinterUnitSystem;
	String PrintingDirection;



public class EzpCmd:AbstractCmd<EzpLanguage,EzpOption>, IPrinterCmd<EzpLanguage,EzpOption>, IPrinterCmd
	String Language;
	String ArgDelimiter;
	String StartCommand;
	String EndCommand;
	String Raw;
	String Code;
	String Description;
	ICmdOption`2[] Options;
	Int32 NbOptions;
	ICmdOption<EzpLanguage,EzpOption> GetOption(String optionCode);
	String ToString();
	String Render(ILabelContext lc, Object[] values);
	String ToString(ICmdOption`2[] options);
	String ClearDelimiters(String rendered);
	void AddBarcode(IBarcodeCmd<EzpLanguage,EzpOption> barcode);
	Boolean TryGetBarcode(String barcodeType, IBarcodeCmd`2& barcode);
	IEnumerable<IBarcodeCmd`2> GetBarcodes();
	T ConvertConstraint(ICmdConstraint<L1,O1> constraint);



public class EzpConstraint:AbstractRangeConstraint<EzpLanguage,EzpOption>, ICmdConstraint<EzpLanguage,EzpOption>, ICmdConstraint
	Object DefaultValue;
	CmdConstraintType ConstraintType;
	Object Clean(Object newValue);
	Exception CheckValid(IPrinterCmd<EzpLanguage,EzpOption> barcode, ICmdOption<EzpLanguage,EzpOption> option, Object newValue);
	String ToString();
	C WithDefault(Object defaultValue);



public class EzpLanguage:AbstractLanguage<EzpLanguage,EzpOption>, IPrinterLanguage<EzpLanguage,EzpOption>, IPrinterLanguage, ISelectable, IRenderer
	String CODE;
	String ARG_DELIMITER;
	String START_COMMAND;
	String END_COMMAND;
	IPrinterLanguage<ZplLanguage,ZplOption> Zpl;
	Boolean AllowsColors;
	String StartLabel;
	String EndLabel;
	String StartCommand;
	String EndCommand;
	String ArgDelimiter;
	String Code;
	String Description;
	String LineWrapValue;
	String Validate(ILabelContext lc, String text);
	Boolean TryGetBarcode(String barcodeType, IBarcodeCmd`2& bc);
	ValueTuple<String,String> GetExpression(ILabelContext lc, IExprRow exprRow);
	RenderResult RenderAsOutput(ILabelContext lc);
	String DrawGraphic(ILabelContext lc, IModelGraphic graphic);
	IList<IPrinterCmd> GetCommands();
	void AddCommand(IPrinterCmd cmd);
	String SetNbCopies(ILabelContext lc, String text, Int32 totalQty, Nullable<Int32> pauseCutValue, Nullable<Int32> replicateEachSerial, Nullable<Boolean> overridePauseCount);
	String AddPause(ILabelContext lc, String text);
	Int32 FindAfter(String text, IPrinterCmd cmd);
	Int32 FindBefore(String text, IPrinterCmd cmd);
	String HandleDensity(ILabelContext lc, String text);
	String HandleFonts(ILabelContext lc, String text);
	void AddFontsForRendering(ILabelContext lc);
	ISerialInfo GetSerialInfo(ILabelContext lc, String text);
	Boolean SupportsRendering(ILabelContext lc, ContentFormat from, ContentFormat to);
	String HandleScript(ILabelContext lc, IExpr expr);
	String HandleIterator(ILabelContext lc, IExprRow exprRow, ILabelElement dataElement);
	String HandleHard(String exprValue);
	String HandleScreen(ILabelContext lc, IExpr expr);
	String HandleFunction(ILabelContext lc, ILabelElement dataElement, IExpr expr);
	String HandleImage(ILabelContext lc, ILabelElement dataElement, IImageDriven imageDriven, IOrientable orientable);
	String HandleContent(ILabelContext lc, IContentable contentable);
	String HandleSubstitution(ILabelContext lc, ISubstitutable substitutable, String exprValue);
	ValueTuple<String,String> HandleRule(ILabelContext lc, IModelDetail modelExpr);
	String HandleBarcode(ILabelContext lc, IModelDetail modelExpr, IBarcodeable barcodable, String exprValue);
	ValueTuple<String,String> HandleHexEncoding(ILabelContext lc, String exprValue, IModelDetail modelExpr, String fieldDataCmd, String fieldSepCmd);
	String HandleGoto(ILabelContext lc, String exprType, IModelDetail modelExpr);
	String HandleColor(ILabelContext lc, IModelDetail modelExpr);
	String HandleFont(ILabelContext lc, IModelDetail modelExpr);
	String HandleJustification(ILabelContext lc, IModelDetail modelExpr);
	String HandleFieldReverse(ILabelContext lc, IModelDetail modelExpr);
	String HandleOrientation(ILabelContext lc, IModelDetail modelExpr, Boolean v);
	ValueTuple<String,String> HandleFieldData(ILabelContext lc, String exprValue);
	String HandleLineWrap(ILabelContext lc, String exprValue, IModelDetail modelExpr);
	FileResult GetPrintResult(ILabelContext labelContext, Int32 nbCopies);



public class EzpOption:AbstractOption<EzpLanguage,EzpOption>, ICmdOption<EzpLanguage,EzpOption>, ICmdOption
	String Code;
	String Description;
	ICmdConstraint<EzpLanguage,EzpOption> Constraint;
	C WithDefault(Object defaultValue);
	String ToString();



public class EplLanguage:AbstractLanguage<EplLanguage,EplOption>, IPrinterLanguage<EplLanguage,EplOption>, IPrinterLanguage, ISelectable, IRenderer
	String CODE;
	String Code;
	String Description;
	String StartLabel;
	String EndLabel;
	String StartCommand;
	String EndCommand;
	String ArgDelimiter;
	Boolean AllowsColors;
	String LineWrapValue;
	String Validate(ILabelContext context, String text);
	Boolean TryGetBarcode(String barcodeType, IBarcodeCmd`2& bc);
	IList<IPrinterCmd> GetCommands();
	void AddCommand(IPrinterCmd cmd);
	String SetNbCopies(ILabelContext lc, String text, Int32 totalQty, Nullable<Int32> pauseCutValue, Nullable<Int32> replicateEachSerial, Nullable<Boolean> overridePauseCount);
	String AddPause(ILabelContext lc, String text);
	Int32 FindAfter(String text, IPrinterCmd cmd);
	Int32 FindBefore(String text, IPrinterCmd cmd);
	String HandleDensity(ILabelContext lc, String text);
	String HandleFonts(ILabelContext lc, String text);
	void AddFontsForRendering(ILabelContext lc);
	ISerialInfo GetSerialInfo(ILabelContext lc, String text);
	RenderResult RenderAsOutput(ILabelContext lc);
	Boolean SupportsRendering(ILabelContext lc, ContentFormat from, ContentFormat to);
	String HandleScript(ILabelContext lc, IExpr expr);
	String HandleIterator(ILabelContext lc, IExprRow exprRow, ILabelElement dataElement);
	String HandleHard(String exprValue);
	String HandleScreen(ILabelContext lc, IExpr expr);
	String HandleFunction(ILabelContext lc, ILabelElement dataElement, IExpr expr);
	String HandleImage(ILabelContext lc, ILabelElement dataElement, IImageDriven imageDriven, IOrientable orientable);
	String HandleContent(ILabelContext lc, IContentable contentable);
	String HandleSubstitution(ILabelContext lc, ISubstitutable substitutable, String exprValue);
	ValueTuple<String,String> GetExpression(ILabelContext lc, IExprRow exprRow);
	ValueTuple<String,String> HandleRule(ILabelContext lc, IModelDetail modelExpr);
	String HandleBarcode(ILabelContext lc, IModelDetail modelExpr, IBarcodeable barcodable, String exprValue);
	ValueTuple<String,String> HandleHexEncoding(ILabelContext lc, String exprValue, IModelDetail modelExpr, String fieldDataCmd, String fieldSepCmd);
	String HandleGoto(ILabelContext lc, String exprType, IModelDetail modelExpr);
	String HandleColor(ILabelContext lc, IModelDetail modelExpr);
	String HandleFont(ILabelContext lc, IModelDetail modelExpr);
	String HandleJustification(ILabelContext lc, IModelDetail modelExpr);
	String HandleFieldReverse(ILabelContext lc, IModelDetail modelExpr);
	String HandleOrientation(ILabelContext lc, IModelDetail modelExpr, Boolean v);
	ValueTuple<String,String> HandleFieldData(ILabelContext lc, String exprValue);
	String HandleLineWrap(ILabelContext lc, String exprValue, IModelDetail modelExpr);
	String DrawGraphic(ILabelContext lc, IModelGraphic graphic);
	FileResult GetPrintResult(ILabelContext labelContext, Int32 nbCopies);



public class EplOption:AbstractOption<EplLanguage,EplOption>, ICmdOption<EplLanguage,EplOption>, ICmdOption
	String Code;
	String Description;
	ICmdConstraint<EplLanguage,EplOption> Constraint;
	C WithDefault(Object defaultValue);
	String ToString();



public class DplLanguage:AbstractLanguage<DplLanguage,DplOption>, IPrinterLanguage<DplLanguage,DplOption>, IPrinterLanguage, ISelectable, IRenderer
	String CODE;
	String Code;
	String Description;
	String StartLabel;
	String EndLabel;
	String StartCommand;
	String EndCommand;
	String ArgDelimiter;
	Boolean AllowsColors;
	String LineWrapValue;
	String Validate(ILabelContext context, String text);
	Boolean TryGetBarcode(String barcodeType, IBarcodeCmd`2& bc);
	IList<IPrinterCmd> GetCommands();
	void AddCommand(IPrinterCmd cmd);
	String SetNbCopies(ILabelContext lc, String text, Int32 totalQty, Nullable<Int32> pauseCutValue, Nullable<Int32> replicateEachSerial, Nullable<Boolean> overridePauseCount);
	String AddPause(ILabelContext lc, String text);
	Int32 FindAfter(String text, IPrinterCmd cmd);
	Int32 FindBefore(String text, IPrinterCmd cmd);
	String HandleDensity(ILabelContext lc, String text);
	String HandleFonts(ILabelContext lc, String text);
	void AddFontsForRendering(ILabelContext lc);
	ISerialInfo GetSerialInfo(ILabelContext lc, String text);
	RenderResult RenderAsOutput(ILabelContext lc);
	Boolean SupportsRendering(ILabelContext lc, ContentFormat from, ContentFormat to);
	String HandleScript(ILabelContext lc, IExpr expr);
	String HandleIterator(ILabelContext lc, IExprRow exprRow, ILabelElement dataElement);
	String HandleHard(String exprValue);
	String HandleScreen(ILabelContext lc, IExpr expr);
	String HandleFunction(ILabelContext lc, ILabelElement dataElement, IExpr expr);
	String HandleImage(ILabelContext lc, ILabelElement dataElement, IImageDriven imageDriven, IOrientable orientable);
	String HandleContent(ILabelContext lc, IContentable contentable);
	String HandleSubstitution(ILabelContext lc, ISubstitutable substitutable, String exprValue);
	ValueTuple<String,String> GetExpression(ILabelContext lc, IExprRow exprRow);
	ValueTuple<String,String> HandleRule(ILabelContext lc, IModelDetail modelExpr);
	String HandleBarcode(ILabelContext lc, IModelDetail modelExpr, IBarcodeable barcodable, String exprValue);
	ValueTuple<String,String> HandleHexEncoding(ILabelContext lc, String exprValue, IModelDetail modelExpr, String fieldDataCmd, String fieldSepCmd);
	String HandleGoto(ILabelContext lc, String exprType, IModelDetail modelExpr);
	String HandleColor(ILabelContext lc, IModelDetail modelExpr);
	String HandleFont(ILabelContext lc, IModelDetail modelExpr);
	String HandleJustification(ILabelContext lc, IModelDetail modelExpr);
	String HandleFieldReverse(ILabelContext lc, IModelDetail modelExpr);
	String HandleOrientation(ILabelContext lc, IModelDetail modelExpr, Boolean v);
	ValueTuple<String,String> HandleFieldData(ILabelContext lc, String exprValue);
	String HandleLineWrap(ILabelContext lc, String exprValue, IModelDetail modelExpr);
	String DrawGraphic(ILabelContext lc, IModelGraphic graphic);
	FileResult GetPrintResult(ILabelContext labelContext, Int32 nbCopies);



public class DplOption:AbstractOption<DplLanguage,DplOption>, ICmdOption<DplLanguage,DplOption>, ICmdOption
	String Code;
	String Description;
	ICmdConstraint<DplLanguage,DplOption> Constraint;
	C WithDefault(Object defaultValue);
	String ToString();



public class BplLanguage:AbstractLanguage<BplLanguage,BplOption>, IPrinterLanguage<BplLanguage,BplOption>, IPrinterLanguage, ISelectable, IRenderer
	String CODE;
	String Code;
	String Description;
	String StartLabel;
	String EndLabel;
	String StartCommand;
	String EndCommand;
	String ArgDelimiter;
	Boolean AllowsColors;
	String LineWrapValue;
	String Validate(ILabelContext context, String text);
	Boolean TryGetBarcode(String barcodeType, IBarcodeCmd`2& bc);
	IList<IPrinterCmd> GetCommands();
	void AddCommand(IPrinterCmd cmd);
	String SetNbCopies(ILabelContext lc, String text, Int32 totalQty, Nullable<Int32> pauseCutValue, Nullable<Int32> replicateEachSerial, Nullable<Boolean> overridePauseCount);
	String AddPause(ILabelContext lc, String text);
	Int32 FindAfter(String text, IPrinterCmd cmd);
	Int32 FindBefore(String text, IPrinterCmd cmd);
	String HandleDensity(ILabelContext lc, String text);
	String HandleFonts(ILabelContext lc, String text);
	void AddFontsForRendering(ILabelContext lc);
	ISerialInfo GetSerialInfo(ILabelContext lc, String text);
	RenderResult RenderAsOutput(ILabelContext lc);
	Boolean SupportsRendering(ILabelContext lc, ContentFormat from, ContentFormat to);
	String HandleScript(ILabelContext lc, IExpr expr);
	String HandleIterator(ILabelContext lc, IExprRow exprRow, ILabelElement dataElement);
	String HandleHard(String exprValue);
	String HandleScreen(ILabelContext lc, IExpr expr);
	String HandleFunction(ILabelContext lc, ILabelElement dataElement, IExpr expr);
	String HandleImage(ILabelContext lc, ILabelElement dataElement, IImageDriven imageDriven, IOrientable orientable);
	String HandleContent(ILabelContext lc, IContentable contentable);
	String HandleSubstitution(ILabelContext lc, ISubstitutable substitutable, String exprValue);
	ValueTuple<String,String> GetExpression(ILabelContext lc, IExprRow exprRow);
	ValueTuple<String,String> HandleRule(ILabelContext lc, IModelDetail modelExpr);
	String HandleBarcode(ILabelContext lc, IModelDetail modelExpr, IBarcodeable barcodable, String exprValue);
	ValueTuple<String,String> HandleHexEncoding(ILabelContext lc, String exprValue, IModelDetail modelExpr, String fieldDataCmd, String fieldSepCmd);
	String HandleGoto(ILabelContext lc, String exprType, IModelDetail modelExpr);
	String HandleColor(ILabelContext lc, IModelDetail modelExpr);
	String HandleFont(ILabelContext lc, IModelDetail modelExpr);
	String HandleJustification(ILabelContext lc, IModelDetail modelExpr);
	String HandleFieldReverse(ILabelContext lc, IModelDetail modelExpr);
	String HandleOrientation(ILabelContext lc, IModelDetail modelExpr, Boolean v);
	ValueTuple<String,String> HandleFieldData(ILabelContext lc, String exprValue);
	String HandleLineWrap(ILabelContext lc, String exprValue, IModelDetail modelExpr);
	String DrawGraphic(ILabelContext lc, IModelGraphic graphic);
	FileResult GetPrintResult(ILabelContext labelContext, Int32 nbCopies);



public class BplOption:AbstractOption<BplLanguage,BplOption>, ICmdOption<BplLanguage,BplOption>, ICmdOption
	String Code;
	String Description;
	ICmdConstraint<BplLanguage,BplOption> Constraint;
	C WithDefault(Object defaultValue);
	String ToString();



public class ImageUtils
	Byte[] ConvertStringToImage(ILabelContext lc, IFormat format, IFont font, String text, Color textColor, Color backgroundColor, ImageFormat imageFormat);
	Byte[] ResizeOrRotate(IFileInfo fi, Nullable<Double> ratio, String orientation);
	Image ResizeImageHighQuality(Image image, Nullable<Double> ratio);
	ImageFormat GetImageFormat(String fileName);
	Bitmap MakeGrayscale3(Bitmap original);



public class AbstractGraphicCreator:IGraphicCreator
	String ImageToLanguage(ILabelContext context, Byte[] imageBytes);
	Boolean IsSupported(ILabelContext context, Format format);



public class AcumaticaFilePrinter:AbstractPrintDestination, IDestination, ISelectable
	String CODE;
	String Code;
	String Description;
	IPrinter Printer;
	FileResult DoPrint(ILabelContext lc, FileResult printResult);
	FileResult Print(ILabelContext lc, FileResult printResult);
	FileResult HandleTransformations(ILabelContext lc, FileResult printResult);



public class GraphicCreatorByLabelary:AbstractGraphicCreator, IGraphicCreator
	String ImageToLanguage(ILabelContext lc, Byte[] imageBytes);
	Boolean IsSupported(ILabelContext context, Format format);



public class GraphicCreatorByLabelZoom:AbstractGraphicCreator, IGraphicCreator
	String ImageToLanguage(ILabelContext lc, Byte[] imageBytes);
	Boolean IsSupported(ILabelContext context, Format format);



public class GraphicCreatorFactory:IGraphicCreatorFactory
	IGraphicCreator GetGraphicCreator(ILabelContext lc, Format _fileFormat);
	IGraphicCreator GetGraphicCreatorInternal(String creatorTypeName);
	IGraphicCreator GetGraphicCreatorInternal(Type creatorType);



public class PdfToImage:AbstractPrintDestination, IDestination, ISelectable, IRenderer
	String CODE;
	String Code;
	String Description;
	IPrinter Printer;
	FileResult DoPrint(ILabelContext lc, FileResult printResult);
	RenderResult RenderAsOutput(ILabelContext lc);
	Boolean SupportsRendering(ILabelContext context, ContentFormat from, ContentFormat to);
	FileResult Print(ILabelContext lc, FileResult printResult);
	FileResult HandleTransformations(ILabelContext lc, FileResult printResult);



public class LabelaryDestination:AbstractPrintDestination, IDestination, ISelectable, IRenderer
	String CODE;
	String Code;
	String Description;
	IPrinter Printer;
	FileResult DoPrint(ILabelContext lc, FileResult printResult);
	RenderResult RenderAsOutput(ILabelContext lc);
	Boolean SupportsRendering(ILabelContext context, ContentFormat from, ContentFormat to);
	FileResult Print(ILabelContext lc, FileResult printResult);
	FileResult HandleTransformations(ILabelContext lc, FileResult printResult);



public class LabelaryUtils
	String DEFAULT_BASE_URL;
	String DEFAULT_API_KEY;
	void Validate(ILabelContext lc);
	RenderResult Render(ILabelContext lc);
	void FillMissingResults(RenderResult renderResult);
	RestRequest GetSingleRestRequest(IFormat format, String rendered);
	IList<LabelRequest> GetRestRequests(ILabelContext lc, RenderResult renderResult);
	RestClient GetRestClient(ILabelContext lc);
	void SetPdfOptions(RestClient restClient, IPdfOptions pdfOptions);
	void HandleResponse(ILabelContext lc, RestClient restClient, LabelRequest labelRequest, RestResponse response, RenderResult renderResult);
	String DecimalToString(Decimal dec);
	String ImageBytesToZpl(ILabelContext lc, Byte[] imageBytes, ContentFormat output);
	String TtfToZpl(ILabelContext lc, IPrinterFile printerFile, Byte[] fontBytes, String fontName, String charSubset);
	String TtfToZpl(Byte[] fontBytes, String objectName, String fontName, String charSubset);
	String ToZpl(String zpl);



public class LabelRequest
	RestRequest Request;
	Int32 LabelNbr;



public class LabelZoomDestination:AbstractPrintDestination, IDestination, ISelectable, IRenderer
	String CODE;
	String Code;
	String Description;
	IPrinter Printer;
	FileResult DoPrint(ILabelContext lc, FileResult printResult);
	RenderResult RenderAsOutput(ILabelContext lc);
	Boolean SupportsRendering(ILabelContext context, ContentFormat from, ContentFormat to);
	FileResult Print(ILabelContext lc, FileResult printResult);
	FileResult HandleTransformations(ILabelContext lc, FileResult printResult);



public class LabelZoomHeader:IXmlSerializable
	Nullable<Guid> Id;
	String Name;
	String Description;
	XmlSchema GetSchema();
	void ReadXml(XmlReader reader);
	String ToString();
	void WriteXml(XmlWriter writer);



public class LabelZoomOptions
	Label label;
	Pdf pdf;
	Zpl zpl;
	List<IDictionary`2> data;
	Int32 dpi;
	Int32 rotation;
	Int32 scaling;
	String colorMode;
	Int32 darkness;
	Position position;



public class PdfConversion:Enum, IComparable, IFormattable, IConvertible
	Int32 value__;
	PdfConversion IMAGE;
	PdfConversion NATIVE;
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



public class ColorMode:Enum, IComparable, IFormattable, IConvertible
	Int32 value__;
	ColorMode BW;
	ColorMode GRAYSCALE;
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



public class Label
	Int32 width;
	Int32 height;



public class Pdf
	String conversionMode;
	Int32 pageNumber;



public class Position
	Int32 x;
	Int32 y;



public class Zpl
	List<String> commandsToIgnore;



public class LabelZoomUtils
	String DEFAULT_BASE_URL;
	String DEFAULT_API_KEY;
	String RENDER_VARIABLES;
	void Validate(ILabelContext lc);
	IEnumerable<LabelZoomHeader> GetLabels(ILabelContext lc);
	LabelZoomLabel GetLabel(ILabelContext lc, String labelID);
	RenderResult Render(ILabelContext lc);
	RenderResult Convert(ILabelContext lc, Object source, ContentFormat sourceFormat, ContentFormat targetFormat);
	void HandleSource(RestClient restClient, RestRequest request, Object source, ContentFormat sourceFormat);
	RenderResult Generate(ILabelContext lc);
	void SetEncoding(RenderResult renderResult, RestResponse response);
	RestClient GetRestClient(ILabelContext lc, String accept, String contentType);
	T HandleResponse(ILabelContext lc, RestClient restClient, RestRequest request, RestResponse response, JsonSerializerSettings settings);
	String DecimalToString(Decimal dec);
	String ImageToZpl(ILabelContext lc, Byte[] imageBytes);
	String GetDescription(Component lzElement);
	void Append(StringBuilder sb, Component lzElement, String propName);
	Decimal Percentage(LabelZoomLabel lzModel, Int32 value, Int32 total);
	String ToString(LabelZoomHeader lzModel);



public class NullDestination:AbstractPrintDestination, IDestination, ISelectable
	String CODE;
	String DESCRIPTION;
	String Code;
	String Description;
	IPrinter Printer;
	FileResult DoPrint(ILabelContext lc, FileResult printResult);
	FileResult Print(ILabelContext lc, FileResult printResult);
	FileResult HandleTransformations(ILabelContext lc, FileResult printResult);



public class PrintPipeline:AbstractPrintDestination, IDestination, ISelectable
	String CODE;
	String Code;
	String Description;
	IPrinter Printer;
	FileResult DoPrint(ILabelContext lc, FileResult printResult);
	IEnumerable<IPrinter> GetPrinterChildren();
	FileResult Print(ILabelContext lc, FileResult printResult);
	FileResult HandleTransformations(ILabelContext lc, FileResult printResult);



public class AddressBlock:Component
	Nullable<Boolean> UseLocalizedAddressFormat;
	Nullable<Boolean> ShowContactName;
	Nullable<Boolean> ShowCountry;
	String StaticAddressFormat;
	LZType Type;
	Nullable<Guid> Id;
	Int32 X;
	Int32 Y;
	Nullable<Decimal> FontSize;
	String Font;
	Nullable<Boolean> BlankWhenNull;
	Nullable<Decimal> Rotation;
	String Expression;
	Nullable<Decimal> HorizontalScaling;
	Nullable<Boolean> Reverse;
	Nullable<Justification> Justification;
	Nullable<PositioningMode> PositioningMode;



public class Barcode:Component
	Nullable<BarcodeStyle> BarcodeStyle;
	Nullable<BarcodeSize> Size;
	Nullable<Int32> DataSize;
	Int32 Height;
	Nullable<Boolean> HumanReadableEnabled;
	Nullable<HumanReadablePosition> HumanReadablePosition;
	Nullable<Boolean> AutoSize;
	Nullable<Boolean> CheckDigit;
	String UccCaseMode;
	LZType Type;
	Nullable<Guid> Id;
	Int32 X;
	Int32 Y;
	Nullable<Decimal> FontSize;
	String Font;
	Nullable<Boolean> BlankWhenNull;
	Nullable<Decimal> Rotation;
	String Expression;
	Nullable<Decimal> HorizontalScaling;
	Nullable<Boolean> Reverse;
	Nullable<Justification> Justification;
	Nullable<PositioningMode> PositioningMode;



public class Component
	LZType Type;
	Nullable<Guid> Id;
	Int32 X;
	Int32 Y;
	Nullable<Decimal> FontSize;
	String Font;
	Nullable<Boolean> BlankWhenNull;
	Nullable<Decimal> Rotation;
	String Expression;
	Nullable<Decimal> HorizontalScaling;
	Nullable<Boolean> Reverse;
	Nullable<Justification> Justification;
	Nullable<PositioningMode> PositioningMode;



public class DataCommand
	String Language;
	String Expression;



public class Elements
	List<Component> Components;



public class Graphic:Component
	Nullable<Int32> Thickness;
	LZType Type;
	Nullable<Guid> Id;
	Int32 X;
	Int32 Y;
	Nullable<Decimal> FontSize;
	String Font;
	Nullable<Boolean> BlankWhenNull;
	Nullable<Decimal> Rotation;
	String Expression;
	Nullable<Decimal> HorizontalScaling;
	Nullable<Boolean> Reverse;
	Nullable<Justification> Justification;
	Nullable<PositioningMode> PositioningMode;



public class Image:Graphic
	String Src;
	Nullable<Decimal> VerticalScaling;
	Nullable<Int32> Thickness;
	LZType Type;
	Nullable<Guid> Id;
	Int32 X;
	Int32 Y;
	Nullable<Decimal> FontSize;
	String Font;
	Nullable<Boolean> BlankWhenNull;
	Nullable<Decimal> Rotation;
	String Expression;
	Nullable<Decimal> HorizontalScaling;
	Nullable<Boolean> Reverse;
	Nullable<Justification> Justification;
	Nullable<PositioningMode> PositioningMode;



public class LabelZoomLabel:LabelZoomHeader, IXmlSerializable
	DataCommand DataCommand;
	Nullable<Int32> Dpi;
	Nullable<LabelUom> Uom;
	Int32 Height;
	Int32 Width;
	Nullable<Int32> MarginLeft;
	Nullable<Int32> MarginRight;
	Nullable<Int32> MarginTop;
	Nullable<Int32> MarginBottom;
	Nullable<Boolean> HighResMode;
	Nullable<LabelOrientation> Orientation;
	String SchemaLocation;
	String SchemaVersion;
	List<Layer> Layers;
	Nullable<Guid> Id;
	String Name;
	String Description;
	void ReadXml(XmlReader reader);
	XmlSchema GetSchema();
	String ToString();
	void WriteXml(XmlWriter writer);



public class Layer
	String Name;
	Elements Elements;



public class Line:Graphic
	LineOrientation Orientation;
	Nullable<Int32> Length;
	Nullable<Int32> Thickness;
	LZType Type;
	Nullable<Guid> Id;
	Int32 X;
	Int32 Y;
	Nullable<Decimal> FontSize;
	String Font;
	Nullable<Boolean> BlankWhenNull;
	Nullable<Decimal> Rotation;
	String Expression;
	Nullable<Decimal> HorizontalScaling;
	Nullable<Boolean> Reverse;
	Nullable<Justification> Justification;
	Nullable<PositioningMode> PositioningMode;



public class Rectangle:Graphic
	Nullable<Int32> Width;
	Nullable<Int32> Height;
	Nullable<Int32> Thickness;
	LZType Type;
	Nullable<Guid> Id;
	Int32 X;
	Int32 Y;
	Nullable<Decimal> FontSize;
	String Font;
	Nullable<Boolean> BlankWhenNull;
	Nullable<Decimal> Rotation;
	String Expression;
	Nullable<Decimal> HorizontalScaling;
	Nullable<Boolean> Reverse;
	Nullable<Justification> Justification;
	Nullable<PositioningMode> PositioningMode;



public class StaticText:Component
	LZType Type;
	Nullable<Guid> Id;
	Int32 X;
	Int32 Y;
	Nullable<Decimal> FontSize;
	String Font;
	Nullable<Boolean> BlankWhenNull;
	Nullable<Decimal> Rotation;
	String Expression;
	Nullable<Decimal> HorizontalScaling;
	Nullable<Boolean> Reverse;
	Nullable<Justification> Justification;
	Nullable<PositioningMode> PositioningMode;



public class VariableText:Component
	Nullable<Boolean> AutoSize;
	Nullable<Int32> Height;
	Nullable<Int32> Width;
	Nullable<HorizontalAlignment> HorizontalAlignment;
	LZType Type;
	Nullable<Guid> Id;
	Int32 X;
	Int32 Y;
	Nullable<Decimal> FontSize;
	String Font;
	Nullable<Boolean> BlankWhenNull;
	Nullable<Decimal> Rotation;
	String Expression;
	Nullable<Decimal> HorizontalScaling;
	Nullable<Boolean> Reverse;
	Nullable<Justification> Justification;
	Nullable<PositioningMode> PositioningMode;



public class AbstractLabelContext:ILabelContext<Coll,ModelType,PrintLogType>, ILabelContext, IRuleEvalContext, IFontProvider, IColorProvider, IFileProvider, IRuleProvider, ISubstitutionProvider, IJustificationProvider, IBarcodeProvider, IModelProvider, IContentProvider, IStandardProvider, ISequenceProvider, IPrinterFileProvider, IConfigProvider, IFormatProvider, ILanguageFactory, ILabelElementProvider, IMarginProvider, IEventLogger, IPrinterProvider, ILanguageDriven
	ModelType Model;
	PrintLogType PrintLog;
	String Language;
	Boolean IsRaw;
	Boolean IsSilent;
	Boolean IgnorePrinterMissing;
	Boolean DealingMode;
	Boolean IsRender;
	Boolean IsAlwaysPrint;
	Boolean RawExpressionsOnly;
	Boolean MergeDetails;
	Boolean PrintDetails;
	FileResult MergeResult;
	String TemplateBody;
	Double DensityRatio;
	Boolean IsSameDensity;
	Boolean IsSnippet;
	Boolean IsRendered;
	Boolean IsDevMode;
	Boolean AddComments;
	Boolean IsSaveRendered;
	Object SingleRow;
	Object Row;
	ContentFormat FinalOutputFormat;
	FileResult CurrentResult;
	ContentFormat CurrentFormat;
	IPdfOptions PdfOptions;
	IFormat ModelFormat;
	IMargin ModelMargin;
	TemplateContext ScribanContext;
	IPrinter Printer;
	IFormat PrinterFormat;
	IMargin PrinterMargin;
	Nullable<Int32> BAccountID;
	Boolean IsDesignMode;
	IPrinterLanguage PrinterLanguage;
	IEnumerable DetailRows;
	IEnumerable PageRows;
	IEnumerable IteratorRows;
	IRowIterator<Coll> PageIterator;
	IRowIterator<Coll> RowIterator;
	Object PageRow;
	Object IteratorRow;
	Object DetailRow;
	Object LabelRow;
	Boolean SendPause;
	IEnumerable<IModelDetail> Expressions;
	IEnumerable<IModelGraphic> Graphics;
	IEnumerable<IRenderableChild`1> Children;
	IEnumerable<FontFile> FontFiles;
	IEnumerable<IFont> Fonts;
	void ResetRenderedBody();
	String GetRenderedTemplate();
	FileResult SaveToPrintLog(FileResult printResult, String fieldName, Boolean saveAsUrl);
	Int32 GetMaxWidthDots();
	Int32 GetMaxHeightDots();
	ZplEncoding GetEncoding();
	IFormat GetFormat();
	Boolean IteratorHasMorePages();
	void PrepareForNextPage();
	void EndIterator();
	IFont GetFont(Object fontRef);
	ValueTuple<IColor,IColor> GetColors(Object foreColorRef, Object backColorRef);
	IRuleDriven[] GetColorRules(Nullable<Guid> colorRef);
	IFileInfo[] GetFiles(Object fileRef);
	IFileInfo GetMainFile(Object fileRef);
	IRule GetRule(Object ruleRef);
	IRuleDetail[] GetRuleDetails(Nullable<Guid> ruleRef);
	ISubstitution GetSubstitution(Object substitutionRef);
	ISubstitutionDetail[] GetSubstitutionDetails(Nullable<Guid> substitutionRef);
	IBarcode GetBarcode(Object barcodeRef);
	IBarcodeOption[] GetOptions(Nullable<Guid> barcodeRef);
	IModel GetModel(Object modelRef);
	IModel[] GetModels(Func<IModel,Boolean> predicate);
	IJustification GetJustification(Object justificationRef);
	IContent GetContent(Object contentRef);
	IContentElement[] GetContentElements(Nullable<Guid> contentRef);
	IStandard GetStandard(Object standardRef);
	IStandardIndentifier[] GetStandardIdentifiers(Nullable<Guid> standardRef);
	ISequence GetSequence(Object sequenceRef);
	IPrinterFile GetPrinterFile(Object fileRef);
	IPrinterFileTransfer GetPrinterFileTransfer(Object fileRef, Object printerRef);
	IEnumerable<IPrinterFileWithData> GetPrinterFilesWithData(Object printerFileID);
	IRuleDriven[] GetPrinterFileRules(Nullable<Guid> fileRef);
	Object GetConfig(Object configRef);
	T GetConfig(Object configRef);
	void HandlePrinter(Nullable<Guid> printerID);
	void HandlePrinterMargin();
	void ValidatePrinter(Nullable<Guid> printerID, String format, Object[] args);
	void HandleMargin();
	void VerifyModelFormat();
	void HandleFormat(IPrintLog printLog);
	void HandleRow(Object row);
	IFileInfo SaveFile(IFileInfo fileInfo, IPrinterFile printerFile, AAFileExistsAction existsAction);
	IMargin GetMargin(Object objectRef);
	IFormat GetFormat(Object formatRef);
	IRuleDriven[] GetFormatRules(Nullable<Guid> formatRef);
	IPrinterLanguage GetLanguage(String language);
	IGraphicCreator GetGraphicCreator(Format fileFormat);
	IPrinter GetPrinter(Object printerRef);
	IPrinter[] GetPrinters(Func<IPrinter,Boolean> predicate);
	ILabelElement GetLabelElement(Object elementRef);
	String Stringify(Object value);
	IIteratorContext Asgard.Labels.Abstractions.Context.ILabelContext.GetIteratorContext(ILabelElement dataElement, ICoordinate coordinate);
	void DoRenderAsLanguage();
	void DoRenderViaAsgard();
	void DoRenderViaLabelZoom();
	ValueTuple<Int32,Int32> GetGotoDots(Decimal x, Decimal y);
	FileResult RenderAndSaveAsUrl(IPrintLog log);
	RenderResult RenderAsOutput();
	Exception GetException(String message, Object[] args);
	Exception GetException(Exception inner, String message, Object[] args);
	void WriteError(String message, Object[] args);
	void WriteError(Exception e);
	void WriteInformation(String message, Object[] args);
	void WriteInformation(Exception e);
	void WriteVerbose(String message, Object[] args);
	void WriteVerbose(Exception e);
	void WriteWarning(String message, Object[] args);
	void WriteWarning(Exception e);
	String DropDownToText(Type CodeType, Type DescType, String dropDownValue);
	Nullable<Int32> PeekNextSerial();
	ISerialInfo GetSerialInfo(String content);
	Int32 GetNbCopies();
	Int32 GetDealingCount();
	Nullable<Int32> GetNextSerial();
	IMargin CalcMargin();
	IDestination GetDestination();
	IDestination GetDestination(IPrinter printer);
	void Print(IFileInfo fi, IPrintLog logRow, Nullable<Int32> nbCopies);
	IRenderer GetRenderer();
	String GetPath(TemplateContext context, SourceSpan callerSpan, String templateName);
	void Merge(FileResult printResult);
	FileResult Merge(FileResult current, FileResult printResult);
	FileResult MergeZpl(FileResult current, FileResult printResult);
	FileResult MergePdf(FileResult current, FileResult printResult);
	void EndMerge();
	void Print(FileResult printResult);
	Boolean PrinterSupportsRendering(IPrinter printer, ILabelContext lc, ContentFormat from, ContentFormat to);
	void Print(IDestination destination, FileResult printResult);
	FileResult DoSavePrintLog(FileResult printResult, String prefix);
	Nullable<Int32> GetDealingCountOverride();
	Nullable<Int32> GetNbCopiesOverride();
	void FindNextSerial();
	Nullable<Guid> ChoosePrinter();
	String Load(TemplateContext context, SourceSpan callerSpan, String snippetName);
	void SaveFileInfo(IFileInfo fileInfo, String fieldName, Boolean saveAsUrl);
	FileResult SaveFileToPrintLog(FileResult printResult, String prefix);
	TService Resolve(Parameter[] parameters);
	void SaveRendered(String rendered);
	ILabelContext<Coll,ModelType,PrintLogType> CreateIteratorContext(ILabelContext<Coll,ModelType,PrintLogType> parent, Nullable<Guid> snippetID);
	IIteratorContext<Coll> GetIteratorContext(ILabelElement dataElement, ICoordinate coordinate);
	ILabelContext CreateIteratorContext(ILabelContext parent, Nullable<Guid> snippetID);
	Boolean HandleIteratorRecord(IIteratorContext iteratorContext, IIteratorPage page, ILabelContext snippetLC, List<String> snippets, Int32 recNumber, Int32 colNbr, Int32 rowNbr);
	ILabelContext CreateRenderContext(String zpl, ContentFormat outputFormat);
	Object GetFileServiceReference(Object row);
	ValueTuple`2[] DropDownToTexts(Type CodeType, Type DescType);
	T GetArgValueAs(IArgHolder argHolder, Int32 argNbr, T defaultValue);



public class ContextHelper
	String GetPrintMessage(PrintResults printResults);
	String HandleContent(ILabelContext lc, IContentable modelExpr);
	String HandleImage(ILabelContext lc, IImageDriven imageSource, ILabelElement dataElement, IOrientable orientable);



public class ILabelGenerator
	PrintResults PrintLabels(C labelContext);



public class RuleUtils
	IRuleDrivenFactory<IRuleDriven,IColor> COLOR_FACTORY;
	IRuleDrivenFactory<IRuleDriven,IFormat> FORMAT_FACTORY;
	IRuleDrivenFactory<IRuleDriven,IPrinterFile> PRINTERFILE_FACTORY;
	RResult GetValueByRules(ILabelContext lc, IRuleDrivenFactory<RDriven,RResult> factory, Nullable<Guid> parentID);
	Boolean MatchingBAccount(ILabelContext lc, RDriven cr);
	Boolean HasBAccount(RDriven cr);
	Boolean HasRule(RDriven cr);
	Boolean Fallthrough(RDriven cr);
	RResult SelectAndFindMatching(ILabelContext lc, IRuleDrivenFactory<RDriven,RResult> factory, IEnumerable<RDriven> rules, Func`2[] matchers);
	RResult FindMatching(ILabelContext lc, IRuleDrivenFactory<RDriven,RResult> factory, IEnumerable<RDriven> rules);
	Boolean IsMatch(ILabelContext lc, RDriven ruleDriven);
	IEnumerable<IRule> GetDependencies(ILabelContext lc, IRule rule);
	String GetExpression(ILabelContext lc, IRule rule);
	Boolean EvalRule(ILabelContext lc, Nullable<Guid> ruleID, Boolean reverse);
	Boolean EvalRule(ILabelContext lc, IRule rule, Boolean reverse);
	String CreateExpr(ILabelContext lc, IRuleDetail[] details);



public class ContextVariables:ScriptObject, IDictionary<String,Object>, ICollection<KeyValuePair`2>, IEnumerable<KeyValuePair`2>, IEnumerable, IScriptObject, IDictionary, ICollection, IFormattable, IScribanLib
	String PREFIX;
	String ROW_GRAPH;
	String OLD_ROW;
	String ROW_ITERATOR;
	String PAGE_ITERATOR;
	String Prefix;
	ScriptMemberImportFlags ImportFlags;
	Int32 Count;
	Boolean IsReadOnly;
	Object Item;
	ICollection<String> Keys;
	ICollection<Object> Values;
	IEnumerable<MethodInfo> GetExposedMethods();
	Boolean IsNoArg(MethodInfo mi);
	Object GetRow(TemplateContext context);
	Object GetOldRow(TemplateContext context);
	Boolean HasIterator(TemplateContext context);
	void SetHasIterator(TemplateContext context, Boolean hasIterator);
	void SetIteratorTotalRowCount(TemplateContext context, Int32 nbRows);
	Int32 GetIteratorTotalRowCount(TemplateContext context);
	Boolean HasIteratorRows(TemplateContext context);
	void SetIteratorRowNbr(TemplateContext context, Int32 rowNumber);
	Int32 GetIteratorRowNbr(TemplateContext context);
	Int32 GetIteratorPageSize(TemplateContext context);
	void SetIteratorPageSize(TemplateContext context, Int32 pageSize);
	Int32 GetIteratorNbPages(TemplateContext context);
	void SetIteratorNbPages(TemplateContext context, Int32 nbRows);
	Int32 GetIteratorPageNbr(TemplateContext context);
	void SetIteratorPageNbr(TemplateContext context, Int32 nbRows);
	Int32 GetRowCount(TemplateContext context);
	void SetRowCount(TemplateContext context, Int32 rowCount);
	Int32 GetLabelCount(TemplateContext context);
	void SetLabelCount(TemplateContext context, Int32 labelCount);
	ILabelContext GetLabelContext(TemplateContext context);
	IModel GetModel(TemplateContext context);
	IPrinter GetPrinter(TemplateContext context);
	IFormat GetModelFormat(TemplateContext context);
	void SetModelFormat(TemplateContext context, IFormat format);
	IMargin GetModelMargin(TemplateContext context);
	void SetModelMargin(TemplateContext context, IMargin margin);
	IFormat GetPrinterFormat(TemplateContext context);
	void SetPrinterFormat(TemplateContext context, IFormat format);
	IMargin GetPrinterMargin(TemplateContext context);
	void SetPrinterMargin(TemplateContext context, IMargin margin);
	IPrinterLanguage GetPrinterLanguage(TemplateContext context);
	String GetLanguageCode(TemplateContext context);
	Nullable<Int32> GetBAccountID(TemplateContext context);
	ContentFormat FinalOutputFormat(TemplateContext context);
	IPdfOptions PdfOptions(TemplateContext context);
	Object SingleRow(TemplateContext context);
	Boolean SendPause(TemplateContext context);
	Boolean IsRendered(TemplateContext context);
	Boolean IsSingleRow(TemplateContext context);
	Boolean IsRaw(TemplateContext context);
	Boolean IsSaveRendered(TemplateContext context);
	Boolean IsSnippet(TemplateContext context);
	Boolean IsRender(TemplateContext context);
	Boolean IsSilent(TemplateContext context);
	Boolean IsAlwaysPrint(TemplateContext context);
	Boolean IsDesignMode(TemplateContext context);
	Boolean IsDealingMode(TemplateContext context);
	LayoutZpl GetLayout(TemplateContext context);
	void SetLayout(TemplateContext context, LayoutZpl layout);
	Int32 GetNbCopies(TemplateContext context);
	Int32 GetDealingCount(TemplateContext context);
	Nullable<Int32> GetNextSerial(TemplateContext context);
	Nullable<Int32> PeekNextSerial(TemplateContext context);
	void ResetIterator(TemplateContext context);
	String Image(TemplateContext context, String fileName, String orientation, Nullable<Double> ratio, String sizeUnit, Nullable<Format> _outputFormat);
	String ConvertAndSaveImage(TemplateContext context, IFileInfo[] files, IPrinterFile printerFile, Byte[] bytes, String filename, Nullable<Format> _outputFormat);
	ValueTuple<Byte[],String> ImageToBytes(TemplateContext context, IFileInfo[] files, String fileName, String orientation, Nullable`1& ratio, String& sizeUnit, Nullable<Format> _outputFormat);
	IFileInfo FindFile(IFileInfo[] files, String name);
	String AddToName(String name, Nullable<Double> ratio, String orientation);
	String GetRowImage(TemplateContext context, Object row, String fileName, Boolean mandatory, String orientation, Nullable<Double> ratio, String sizeUnit, Nullable<Format> outputFormat);
	void System.Collections.IDictionary.Add(Object key, Object value);
	void Clear();
	Boolean System.Collections.IDictionary.Contains(Object key);
	IDictionaryEnumerator System.Collections.IDictionary.GetEnumerator();
	void System.Collections.IDictionary.Remove(Object key);
	void System.Collections.ICollection.CopyTo(Array array, Int32 index);
	IEnumerable<String> GetMembers();
	Boolean Contains(String member);
	Boolean TryGetValue(TemplateContext context, SourceSpan span, String member, Object& value);
	T GetSafeValue(String name, T defaultValue);
	Boolean System.Collections.Generic.IDictionary<System.String,System.Object>.TryGetValue(String key, Object& value);
	Boolean CanWrite(String member);
	Boolean TrySetValue(TemplateContext context, SourceSpan span, String member, Object value, Boolean readOnly);
	void SetValue(String member, Object value, Boolean readOnly);
	void Add(String key, Object value);
	Boolean ContainsKey(String key);
	Boolean Remove(String member);
	void SetReadOnly(String member, Boolean readOnly);
	String ToString(String format, IFormatProvider formatProvider);
	String ToString(IFormatProvider formatProvider);
	String ToString();
	void CopyTo(ScriptObject dest);
	IScriptObject Clone(Boolean deep);
	IEnumerator<KeyValuePair`2> GetEnumerator();
	IEnumerator System.Collections.IEnumerable.GetEnumerator();
	ScriptObject From(Object obj);
	Boolean IsImportable(Object obj);
	void System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.Add(KeyValuePair<String,Object> item);
	Boolean System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.Contains(KeyValuePair<String,Object> item);
	void System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.CopyTo(KeyValuePair`2[] array, Int32 arrayIndex);
	Boolean System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.Remove(KeyValuePair<String,Object> item);



public class BarcodeImageFormat:Enum, IComparable, IFormattable, IConvertible
	Int32 value__;
	BarcodeImageFormat Png;
	BarcodeImageFormat Jpeg;
	BarcodeImageFormat Bmp;
	BarcodeImageFormat Gif;
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



public class BarcodeTextPosition:Enum, IComparable, IFormattable, IConvertible
	Int32 value__;
	BarcodeTextPosition None;
	BarcodeTextPosition Above;
	BarcodeTextPosition Below;
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



public class IBarcodeFactory
	String[] SupportedTypes;
	IBarcodeImage CreateBarcode(String text, IBarcode barcode, IBarcodeOption[] options);
	Boolean SupportsType(String barcodeType);



public class IBarcodeImage:IDisposable
	Int32 Width;
	Int32 Height;
	Byte[] GetImageBytes(BarcodeImageFormat format);
	Image GetImage();



public class IBarcodeSettings
	String BarcodeType;
	Nullable<Int32> Width;
	Nullable<Int32> Height;
	Nullable<Int32> ModuleWidth;
	Nullable<Int32> QuietZone;
	Nullable<Boolean> ShowText;
	String FontName;
	Nullable<Single> FontSize;
	Nullable<Color> ForeColor;
	Nullable<Color> BackColor;
	Nullable<BarcodeTextPosition> TextPosition;
	Nullable<Int32> ErrorCorrectionLevel;



public class [nested] layoutObject
	String type;
	String id;
	layoutObjectPosition position;
	String source;
	layoutObjectSize size;
	layoutObjectBarcode barcode;
	String data;
	String text;
	layoutObjectFont font;



public class [nested] layoutObjectPosition
	Byte x;
	Byte y;
	String unit;



public class [nested] layoutObjectSize
	Byte width;
	Byte height;



public class [nested] layoutObjectBarcode
	String symbology;
	Boolean humanReadable;



public class [nested] layoutObjectFont
	String name;
	Byte size;
	Boolean bold;



public class [nested] Impl



public class [nested] Rotation
	PdfPageRotateAngle GetRotationSpire(String rotation);
	String ToZpl(String rotation);



public class [nested] LabelZoom
	String ToRotation(Nullable<LabelOrientation> orientation);
	String ToOrientation(Nullable<Decimal> rotation);
	String ToSizeUnit(Nullable<LabelUom> uom);



public class [nested] PrintResult
	Int32 NbLabels;
	IPrinter Printer;
	IModel Model;
	String ToString();



public class [nested] UnboundedTrueType:ZplTildeCmd, IPrinterCmd<ZplLanguage,ZplOption>, IPrinterCmd
	String StartCommand;
	String EndCommand;
	String Language;
	String ArgDelimiter;
	String Raw;
	String Code;
	String Description;
	ICmdOption`2[] Options;
	Int32 NbOptions;
	String GetCommand(ILabelContext lc, IPrinterFile printerFile, Byte[] ttfOtfData);
	String ClearDelimiters(String rendered);
	String WrapStartEnd(String[] cmds);
	ICmdOption<ZplLanguage,ZplOption> GetOption(String optionCode);
	String ToString();
	String Render(ILabelContext lc, Object[] values);
	String ToString(ICmdOption`2[] options);
	void AddBarcode(IBarcodeCmd<ZplLanguage,ZplOption> barcode);
	Boolean TryGetBarcode(String barcodeType, IBarcodeCmd`2& barcode);
	IEnumerable<IBarcodeCmd`2> GetBarcodes();
	T ConvertConstraint(ICmdConstraint<L1,O1> constraint);



public class [nested] BoundedTrueType:ZplTildeCmd, IPrinterCmd<ZplLanguage,ZplOption>, IPrinterCmd
	String StartCommand;
	String EndCommand;
	String Language;
	String ArgDelimiter;
	String Raw;
	String Code;
	String Description;
	ICmdOption`2[] Options;
	Int32 NbOptions;
	String GetCommand(ILabelContext lc, IPrinterFile printerFile, Byte[] ttfOtfData);
	String ClearDelimiters(String rendered);
	String WrapStartEnd(String[] cmds);
	ICmdOption<ZplLanguage,ZplOption> GetOption(String optionCode);
	String ToString();
	String Render(ILabelContext lc, Object[] values);
	String ToString(ICmdOption`2[] options);
	void AddBarcode(IBarcodeCmd<ZplLanguage,ZplOption> barcode);
	Boolean TryGetBarcode(String barcodeType, IBarcodeCmd`2& barcode);
	IEnumerable<IBarcodeCmd`2> GetBarcodes();
	T ConvertConstraint(ICmdConstraint<L1,O1> constraint);



public class [nested] Intellifont:ZplTildeCmd, IPrinterCmd<ZplLanguage,ZplOption>, IPrinterCmd
	String StartCommand;
	String EndCommand;
	String Language;
	String ArgDelimiter;
	String Raw;
	String Code;
	String Description;
	ICmdOption`2[] Options;
	Int32 NbOptions;
	String GetCommand(ILabelContext lc, IPrinterFile printerFile, Byte[] ttfOtfData);
	String ClearDelimiters(String rendered);
	String WrapStartEnd(String[] cmds);
	ICmdOption<ZplLanguage,ZplOption> GetOption(String optionCode);
	String ToString();
	String Render(ILabelContext lc, Object[] values);
	String ToString(ICmdOption`2[] options);
	void AddBarcode(IBarcodeCmd<ZplLanguage,ZplOption> barcode);
	Boolean TryGetBarcode(String barcodeType, IBarcodeCmd`2& barcode);
	IEnumerable<IBarcodeCmd`2> GetBarcodes();
	T ConvertConstraint(ICmdConstraint<L1,O1> constraint);



public class [nested] Justification
	String LEFT;
	String CENTER;
	String RIGHT;
	String JUSTIFY;



public class [nested] Operation
	String WRITE;
	String READ;
	String SET_PASSWORD;



public class [nested] Format
	String ASCII;
	String HEXA;
	String EPC;



public class [nested] MemoryBank
	String EPC_96;
	String EPC_AUTO_PC;
	String RESERVED;
	String EPC;
	String TID;
	String USER;



public class [nested] CompressionType:Enum, IComparable, IFormattable, IConvertible
	Int32 value__;
	CompressionType B64;
	CompressionType Z64;
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



public class [nested] LINE_COLORS
	ZplConstraint CONSTRAINT;
	String BLACK;
	String WHITE;



public class [nested] DIAG_LEANING
	ZplConstraint CONSTRAINT;
	String RIGHT_LEANING;
	String LEFT_LEANING;



public class [nested] ORIENTATION
	ZplConstraint CONSTRAINT;
	ZplConstraint DEFAULT_NORMAL;
	ZplConstraint JUST_NORMAL;
	ZplConstraint RSS;
	String NORMAL_000;
	String TOP_BOTTOM_090;
	String RIGHT_TO_LEFT_180;
	String BOTTOM_UP_270;
	Int32 ToDegree(String orientation);



public class [nested] UNIT
	ZplConstraint UNIT_CONSTRAINT;
	ZplConstraint BASE_DPI_CONSTRAINT;
	ZplConstraint PRINTER_DPI_CONSTRAINT;
	String DOT;
	String INCH;
	String MM;
	String _150;
	String _200;
	String _300;
	String _600;



public class [nested] UNIT
	StarplConstraint UNIT_CONSTRAINT;
	String INCH;
	String MM;



public class [nested] Reversal
	String SPECIFIED_BY_FIELD_REVERSE;
	String CANCELLED;
	String SPECIFIED;
	EzpConstraint CONSTRAINT;



public class [nested] IRuleDrivenFactory
	String Name;
	IEnumerable<RDriven> GetRules(ILabelContext lc, Nullable<Guid> ID);
	RResult GetValue(ILabelContext lc, Nullable<Guid> ID);
	RResult GetValueByRules(ILabelContext lc, Nullable<Guid> parentID);



public class [nested] AbstractFactory:IRuleDrivenFactory<RDriven,RResult>
	String Name;
	IEnumerable<RDriven> GetRules(ILabelContext lc, Nullable<Guid> id);
	RResult GetValue(ILabelContext lc, Nullable<Guid> id);
	RResult GetValueByRules(ILabelContext lc, Nullable<Guid> parentID);



public class [nested] ColorFactory:AbstractFactory<IRuleDriven,IColor>, IRuleDrivenFactory<IRuleDriven,IColor>
	String Name;
	IColor GetValue(ILabelContext lc, Nullable<Guid> id);
	IEnumerable<IRuleDriven> GetRules(ILabelContext lc, Nullable<Guid> id);
	IColor GetValueByRules(ILabelContext lc, Nullable<Guid> parentID);



public class [nested] FormatFactory:AbstractFactory<IRuleDriven,IFormat>, IRuleDrivenFactory<IRuleDriven,IFormat>
	String Name;
	IFormat GetValue(ILabelContext lc, Nullable<Guid> id);
	IEnumerable<IRuleDriven> GetRules(ILabelContext lc, Nullable<Guid> id);
	IFormat GetValueByRules(ILabelContext lc, Nullable<Guid> parentID);



public class [nested] PrintFileFactory:AbstractFactory<IRuleDriven,IPrinterFile>, IRuleDrivenFactory<IRuleDriven,IPrinterFile>
	String Name;
	IPrinterFile GetValue(ILabelContext lc, Nullable<Guid> id);
	IEnumerable<IRuleDriven> GetRules(ILabelContext lc, Nullable<Guid> id);
	IPrinterFile GetValueByRules(ILabelContext lc, Nullable<Guid> parentID);



public class [nested] defaultGraphCreator:Constant<IBqlString,String,defaultGraphCreator>, IBqlOperand, IImplement<IBqlString>, IImplement<IBqlCastableTo`1>, IShouldBeReplacedWith<defaultGraphCreator>, IConstant<String>, IConstant, IBqlAggregatedOperand, IBqlCreator, IBqlVerifier
	String Value;
	void PX.Data.IBqlVerifier.Verify(PXCache cache, Object item, List<Object> pars, Nullable`1& result, Object& value);
	Boolean PX.Data.IBqlCreator.AppendExpression(SQLExpression& exp, PXGraph graph, BqlCommandInfo info, Selection selection);



public class [nested] LZType:Enum, IComparable, IFormattable, IConvertible
	Int32 value__;
	LZType StaticText;
	LZType VariableText;
	LZType AddressBlock;
	LZType Barcode;
	LZType Line;
	LZType Rectangle;
	LZType Image;
	LZType Ellipse;
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



public class [nested] LabelOrientation:Enum, IComparable, IFormattable, IConvertible
	Int32 value__;
	LabelOrientation Portrait;
	LabelOrientation Landscape;
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



public class [nested] LabelUom:Enum, IComparable, IFormattable, IConvertible
	Int32 value__;
	LabelUom Inches;
	LabelUom Millimeters;
	LabelUom Centimeters;
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



public class [nested] Justification:Enum, IComparable, IFormattable, IConvertible
	Int32 value__;
	Justification Left;
	Justification Right;
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



public class [nested] HorizontalAlignment:Enum, IComparable, IFormattable, IConvertible
	Int32 value__;
	HorizontalAlignment Left;
	HorizontalAlignment Right;
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



public class [nested] PositioningMode:Enum, IComparable, IFormattable, IConvertible
	Int32 value__;
	PositioningMode Origin;
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



public class [nested] HumanReadablePosition:Enum, IComparable, IFormattable, IConvertible
	Int32 value__;
	HumanReadablePosition Below;
	HumanReadablePosition Above;
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



public class [nested] BarcodeStyle:Enum, IComparable, IFormattable, IConvertible
	Int32 value__;
	BarcodeStyle Aztec;
	BarcodeStyle Aztec2;
	BarcodeStyle ANSICodabar;
	BarcodeStyle Code11;
	BarcodeStyle Code39;
	BarcodeStyle Code49;
	BarcodeStyle Code93;
	BarcodeStyle Code128;
	BarcodeStyle CODABLOCK;
	BarcodeStyle DataMatrix;
	BarcodeStyle EAN_8;
	BarcodeStyle EAN_13;
	BarcodeStyle GS1DataBar;
	BarcodeStyle Interleaved2of5;
	BarcodeStyle Industrial2of5;
	BarcodeStyle LOGMARS;
	BarcodeStyle MSI;
	BarcodeStyle Micro_PDF417;
	BarcodeStyle PlanetCode;
	BarcodeStyle PDF417;
	BarcodeStyle PostNet;
	BarcodeStyle Plessey;
	BarcodeStyle QRCode;
	BarcodeStyle Standard2of5;
	BarcodeStyle UPSMaxiCode;
	BarcodeStyle UPC_A;
	BarcodeStyle UPC_E;
	BarcodeStyle UPC_EAN;
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



public class [nested] BarcodeSize:Enum, IComparable, IFormattable, IConvertible
	Int32 value__;
	BarcodeSize Smallest;
	BarcodeSize Smaller;
	BarcodeSize Normal;
	BarcodeSize Large;
	BarcodeSize XLarge;
	BarcodeSize XXLarge;
	BarcodeSize XXXLarge;
	BarcodeSize XXXXLarge;
	BarcodeSize XXXXXLarge;
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



public class [nested] LineOrientation:Enum, IComparable, IFormattable, IConvertible
	Int32 value__;
	LineOrientation Vertical;
	LineOrientation Horizontal;
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