public class LogMessageBag:IReadOnlyList<LogMessage>, IReadOnlyCollection<LogMessage>, IEnumerable<LogMessage>, IEnumerable
	Int32 Count;
	LogMessage Item;
	Boolean HasErrors;
	void Add(LogMessage message);
	void AddRange(IEnumerable<LogMessage> messages);
	IEnumerator<LogMessage> GetEnumerator();
	IEnumerator System.Collections.IEnumerable.GetEnumerator();
	String ToString();



public class ScriptPrinter
	ScriptPrinterOptions Options;
	Boolean PreviousHasSpace;
	Boolean IsInWhileLoop;
	ScriptPrinter Write(ScriptNode node);
	ScriptPrinter Write(String text);
	ScriptPrinter Write(ScriptStringSlice slice);
	ScriptPrinter ExpectEos();
	ScriptPrinter ExpectSpace();
	ScriptPrinter WriteListWithCommas(IList<T> list);
	ScriptPrinter WriteEnterCode(Int32 escape);
	ScriptPrinter WriteExitCode(Int32 escape);
	void WriteBegin(ScriptNode node);
	void WriteEnd(ScriptNode node);
	Boolean IsFrontMarker(ScriptNode node);
	void HandleEos(ScriptNode node);
	Boolean IsBlockOrPage(ScriptNode node);
	void WriteTrivias(ScriptNode node, Boolean before);



public class ScriptPrinterOptions:ValueType
	ScriptMode Mode;
	Boolean Equals(Object obj);
	String ToString();
	Int32 GetHashCode();
	Int32 GetHashCodeOfPtr(IntPtr ptr);



public class Template
	String SourceFilePath;
	ScriptPage Page;
	Boolean HasErrors;
	LogMessageBag Messages;
	ParserOptions ParserOptions;
	LexerOptions LexerOptions;
	Template Parse(String text, String sourceFilePath, Nullable<ParserOptions> parserOptions, Nullable<LexerOptions> lexerOptions);
	Template ParseLiquid(String text, String sourceFilePath, Nullable<ParserOptions> parserOptions, Nullable<LexerOptions> lexerOptions);
	Object Evaluate(String expression, TemplateContext context);
	Object Evaluate(String expression, Object model, MemberRenamerDelegate memberRenamer, MemberFilterDelegate memberFilter);
	Object Evaluate(TemplateContext context);
	Object Evaluate(Object model, MemberRenamerDelegate memberRenamer, MemberFilterDelegate memberFilter);
	String Render(TemplateContext context);
	String Render(Object model, MemberRenamerDelegate memberRenamer, MemberFilterDelegate memberFilter);
	String ToText(ScriptPrinterOptions options);
	Object EvaluateAndRender(TemplateContext context, Boolean render);
	void CheckErrors();
	void ParseInternal(String text, String sourceFilePath);



public class TemplateContext:IFormatProvider
	RenderRuntimeExceptionDelegate RenderRuntimeExceptionDefault;
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
	Object GetOrSetValue(ScriptExpression targetExpression, Object valueToSet, Boolean setter);
	IListAccessor GetListAccessor(Object target);
	IListAccessor GetListAccessorImpl(Object target, Type type);
	void ResetPreviousNewLine();
	String GetTemplatePathFromName(String templateName, ScriptNode callerContext);
	String ConvertTemplateNameToPath(String templateName, ScriptNode callerContext);
	Template GetOrCreateTemplate(String templatePath, ScriptNode callerContext);
	Template CreateTemplate(String templatePath, ScriptNode callerContext);
	IReadOnlyList<ScriptVariable> PromoteScriptNamedArguments(ScriptNode scriptNode);
	String RenderTemplate(Template template, ScriptArray arguments, ScriptNode callerContext);
	Object GetFormat(Type formatType);
	Object IsEmpty(SourceSpan span, Object against);
	IList ToList(SourceSpan span, Object value);
	String ObjectToString(Object value, Boolean nested);
	String ObjectToStringImpl(Object value, Boolean nested);
	Boolean ToBool(SourceSpan span, Object value);
	Int32 ToInt(SourceSpan span, Object value);
	String GetTypeName(Object value);
	T ToObject(SourceSpan span, Object value);
	Object ToObject(SourceSpan span, Object value, Type destinationType);
	void PushGlobal(IScriptObject scriptObject);
	void PushGlobalOnly(IScriptObject scriptObject);
	VariableContext GetOrCreateGlobalContext(IScriptObject globalObject);
	IScriptObject PopGlobalOnly();
	IScriptObject PopGlobal();
	void PushLocal();
	void PopLocal();
	void SetValue(ScriptVariable variable, Object value, Boolean asReadOnly);
	void SetValue(ScriptVariable variable, Object value, Boolean asReadOnly, Boolean force);
	void DeleteValue(ScriptVariable variable);
	void SetReadOnly(ScriptVariable variable, Boolean isReadOnly);
	void SetLoopVariable(ScriptVariable variable, Object value);
	void PushLocalContext(ScriptObject locals);
	ScriptObject PopLocalContext();
	Object GetValue(ScriptVariable variable);
	Object GetValue(ScriptVariableGlobal variable);
	IScriptObject GetStoreForWrite(ScriptVariable variable);
	IEnumerable<IScriptObject> GetStoreForRead(ScriptVariable variable);
	void CheckVariableFound(ScriptVariable variable, Boolean found);
	void PushVariableScope(VariableScope scope);
	void PopVariableScope(VariableScope scope);
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
	Object Evaluate(ScriptNode scriptNode, Boolean aliasReturnedFunction);
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



public class LiquidTemplateContext:TemplateContext, IFormatProvider
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
	String GetTemplatePathFromName(String templateName, ScriptNode callerContext);
	IListAccessor GetListAccessor(Object target);
	IListAccessor GetListAccessorImpl(Object target, Type type);
	void ResetPreviousNewLine();
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
	void SetLoopVariable(ScriptVariable variable, Object value);
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
	Object Evaluate(ScriptNode scriptNode, Boolean aliasReturnedFunction);
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



public class ScriptAnonymousFunction:ScriptExpression
	SourceSpan Span;
	Int32 ChildrenCount;
	ScriptFunction Function;
	ScriptNode Parent;
	IEnumerable<ScriptNode> Children;
	ScriptNode GetChildrenImpl(Int32 index);
	void Accept(ScriptVisitor visitor);
	TResult Accept(ScriptVisitor<TResult> visitor);
	Object Evaluate(TemplateContext context);
	void PrintTo(ScriptPrinter printer);
	ScriptNode Clone();
	ScriptNode Clone(Boolean withTrivias);
	ScriptNode GetChildren(Int32 index);
	Boolean CanHaveLeadingTrivia();
	void ParentToThis(TSyntaxNode& set, TSyntaxNode node);
	String ToString();



public class ScriptArgumentBinary:ScriptExpression
	SourceSpan Span;
	Int32 ChildrenCount;
	ScriptBinaryOperator Operator;
	ScriptToken OperatorToken;
	ScriptNode Parent;
	IEnumerable<ScriptNode> Children;
	ScriptNode GetChildrenImpl(Int32 index);
	void Accept(ScriptVisitor visitor);
	TResult Accept(ScriptVisitor<TResult> visitor);
	Object Evaluate(TemplateContext context);
	void PrintTo(ScriptPrinter printer);
	ScriptNode Clone();
	ScriptNode Clone(Boolean withTrivias);
	ScriptNode GetChildren(Int32 index);
	Boolean CanHaveLeadingTrivia();
	void ParentToThis(TSyntaxNode& set, TSyntaxNode node);
	String ToString();



public class ScriptArrayInitializerExpression:ScriptExpression
	SourceSpan Span;
	Int32 ChildrenCount;
	ScriptToken OpenBracketToken;
	ScriptList<ScriptExpression> Values;
	ScriptToken CloseBracketToken;
	ScriptNode Parent;
	IEnumerable<ScriptNode> Children;
	ScriptNode GetChildrenImpl(Int32 index);
	void Accept(ScriptVisitor visitor);
	TResult Accept(ScriptVisitor<TResult> visitor);
	Object Evaluate(TemplateContext context);
	void PrintTo(ScriptPrinter printer);
	ScriptNode Clone();
	ScriptNode Clone(Boolean withTrivias);
	ScriptNode GetChildren(Int32 index);
	Boolean CanHaveLeadingTrivia();
	void ParentToThis(TSyntaxNode& set, TSyntaxNode node);
	String ToString();



public class ScriptAssignExpression:ScriptExpression
	SourceSpan Span;
	Int32 ChildrenCount;
	ScriptExpression Target;
	ScriptToken EqualToken;
	ScriptExpression Value;
	ScriptNode Parent;
	IEnumerable<ScriptNode> Children;
	ScriptNode GetChildrenImpl(Int32 index);
	void Accept(ScriptVisitor visitor);
	TResult Accept(ScriptVisitor<TResult> visitor);
	Object Evaluate(TemplateContext context);
	Object GetValueToSet(TemplateContext context);
	Boolean CanHaveLeadingTrivia();
	void PrintTo(ScriptPrinter printer);
	ScriptNode Clone();
	ScriptNode Clone(Boolean withTrivias);
	ScriptNode GetChildren(Int32 index);
	void ParentToThis(TSyntaxNode& set, TSyntaxNode node);
	String ToString();



public class ScriptBinaryExpression:ScriptExpression
	SourceSpan Span;
	Int32 ChildrenCount;
	ScriptExpression Left;
	ScriptBinaryOperator Operator;
	ScriptToken OperatorToken;
	String OperatorAsText;
	ScriptExpression Right;
	ScriptNode Parent;
	IEnumerable<ScriptNode> Children;
	ScriptNode GetChildrenImpl(Int32 index);
	void Accept(ScriptVisitor visitor);
	TResult Accept(ScriptVisitor<TResult> visitor);
	Object Evaluate(TemplateContext context);
	void PrintTo(ScriptPrinter printer);
	Boolean CanHaveLeadingTrivia();
	Object Evaluate(TemplateContext context, SourceSpan span, ScriptBinaryOperator op, Object leftValue, Object rightValue);
	Object Evaluate(TemplateContext context, SourceSpan span, ScriptBinaryOperator op, SourceSpan leftSpan, Object leftValue, SourceSpan rightSpan, Object rightValue);
	Object CalculateEmpty(TemplateContext context, SourceSpan span, ScriptBinaryOperator op, SourceSpan leftSpan, Object leftValue, SourceSpan rightSpan, Object rightValue);
	Object CalculateToString(TemplateContext context, SourceSpan span, ScriptBinaryOperator op, SourceSpan leftSpan, Object left, SourceSpan rightSpan, Object right);
	IEnumerable<Object> RangeInclude(Int64 left, Int64 right);
	IEnumerable<Object> RangeExclude(Int64 left, Int64 right);
	IEnumerable<Object> RangeInclude(BigInteger left, BigInteger right);
	IEnumerable<Object> RangeExclude(BigInteger left, BigInteger right);
	Object CalculateOthers(TemplateContext context, SourceSpan span, ScriptBinaryOperator op, SourceSpan leftSpan, Object leftValue, SourceSpan rightSpan, Object rightValue);
	Object CalculateInt(ScriptBinaryOperator op, SourceSpan span, Int32 left, Int32 right);
	Object FitToBestInteger(Object value);
	Object FitToBestInteger(Int64 longValue);
	Object FitToBestInteger(BigInteger bigInt);
	Object CalculateLongWithInt(ScriptBinaryOperator op, SourceSpan span, Int32 leftInt, Int32 rightInt);
	Object CalculateLong(ScriptBinaryOperator op, SourceSpan span, Int64 left, Int64 right);
	Object CalculateLong(ScriptBinaryOperator op, SourceSpan span, UInt64 left, UInt64 right);
	Object CalculateBigInteger(ScriptBinaryOperator op, SourceSpan span, BigInteger left, BigInteger right);
	Object CalculateBigIntegerNoFit(ScriptBinaryOperator op, SourceSpan span, BigInteger left, BigInteger right);
	Object CalculateDouble(ScriptBinaryOperator op, SourceSpan span, Double left, Double right);
	Object CalculateDecimal(ScriptBinaryOperator op, SourceSpan span, Decimal left, Decimal right);
	Object CalculateFloat(ScriptBinaryOperator op, SourceSpan span, Single left, Single right);
	Object CalculateDateTime(ScriptBinaryOperator op, SourceSpan span, DateTime left, DateTime right);
	Object CalculateDateTime(ScriptBinaryOperator op, SourceSpan span, DateTime left, TimeSpan right);
	Object CalculateBool(ScriptBinaryOperator op, SourceSpan span, Boolean left, Boolean right);
	ScriptNode Clone();
	ScriptNode Clone(Boolean withTrivias);
	ScriptNode GetChildren(Int32 index);
	void ParentToThis(TSyntaxNode& set, TSyntaxNode node);
	String ToString();



public class ScriptBlockStatement:ScriptStatement
	SourceSpan Span;
	Int32 ChildrenCount;
	ScriptList<ScriptStatement> Statements;
	Boolean CanSkipEvaluation;
	Boolean CanOutput;
	ScriptNode Parent;
	IEnumerable<ScriptNode> Children;
	ScriptNode GetChildrenImpl(Int32 index);
	void Accept(ScriptVisitor visitor);
	TResult Accept(ScriptVisitor<TResult> visitor);
	Object Evaluate(TemplateContext context);
	void PrintTo(ScriptPrinter printer);
	Boolean CanHaveLeadingTrivia();
	ScriptNode Clone();
	ScriptNode Clone(Boolean withTrivias);
	ScriptNode GetChildren(Int32 index);
	void ParentToThis(TSyntaxNode& set, TSyntaxNode node);
	String ToString();



public class ScriptBreakStatement:ScriptStatement
	SourceSpan Span;
	Int32 ChildrenCount;
	ScriptKeyword BreakKeyword;
	Boolean CanSkipEvaluation;
	Boolean CanOutput;
	ScriptNode Parent;
	IEnumerable<ScriptNode> Children;
	ScriptNode GetChildrenImpl(Int32 index);
	void Accept(ScriptVisitor visitor);
	TResult Accept(ScriptVisitor<TResult> visitor);
	Object Evaluate(TemplateContext context);
	void PrintTo(ScriptPrinter printer);
	ScriptNode Clone();
	ScriptNode Clone(Boolean withTrivias);
	ScriptNode GetChildren(Int32 index);
	Boolean CanHaveLeadingTrivia();
	void ParentToThis(TSyntaxNode& set, TSyntaxNode node);
	String ToString();



public class ScriptCaptureStatement:ScriptStatement
	SourceSpan Span;
	Int32 ChildrenCount;
	ScriptKeyword CaptureKeyword;
	ScriptExpression Target;
	ScriptBlockStatement Body;
	Boolean CanSkipEvaluation;
	Boolean CanOutput;
	ScriptNode Parent;
	IEnumerable<ScriptNode> Children;
	ScriptNode GetChildrenImpl(Int32 index);
	void Accept(ScriptVisitor visitor);
	TResult Accept(ScriptVisitor<TResult> visitor);
	Object Evaluate(TemplateContext context);
	void PrintTo(ScriptPrinter printer);
	ScriptNode Clone();
	ScriptNode Clone(Boolean withTrivias);
	ScriptNode GetChildren(Int32 index);
	Boolean CanHaveLeadingTrivia();
	void ParentToThis(TSyntaxNode& set, TSyntaxNode node);
	String ToString();



public class ScriptCaseStatement:ScriptConditionStatement
	SourceSpan Span;
	Int32 ChildrenCount;
	ScriptKeyword CaseKeyword;
	ScriptExpression Value;
	ScriptBlockStatement Body;
	Boolean CanSkipEvaluation;
	Boolean CanOutput;
	ScriptNode Parent;
	IEnumerable<ScriptNode> Children;
	ScriptNode GetChildrenImpl(Int32 index);
	void Accept(ScriptVisitor visitor);
	TResult Accept(ScriptVisitor<TResult> visitor);
	Object Evaluate(TemplateContext context);
	void PrintTo(ScriptPrinter printer);
	ScriptNode Clone();
	ScriptNode Clone(Boolean withTrivias);
	ScriptNode GetChildren(Int32 index);
	Boolean CanHaveLeadingTrivia();
	void ParentToThis(TSyntaxNode& set, TSyntaxNode node);
	String ToString();



public class ScriptConditionalExpression:ScriptExpression
	SourceSpan Span;
	Int32 ChildrenCount;
	ScriptExpression Condition;
	ScriptToken QuestionToken;
	ScriptExpression ThenValue;
	ScriptToken ColonToken;
	ScriptExpression ElseValue;
	ScriptNode Parent;
	IEnumerable<ScriptNode> Children;
	ScriptNode GetChildrenImpl(Int32 index);
	void Accept(ScriptVisitor visitor);
	TResult Accept(ScriptVisitor<TResult> visitor);
	Object Evaluate(TemplateContext context);
	void PrintTo(ScriptPrinter printer);
	Boolean CanHaveLeadingTrivia();
	ScriptNode Clone();
	ScriptNode Clone(Boolean withTrivias);
	ScriptNode GetChildren(Int32 index);
	void ParentToThis(TSyntaxNode& set, TSyntaxNode node);
	String ToString();



public class ScriptContinueStatement:ScriptStatement
	SourceSpan Span;
	Int32 ChildrenCount;
	ScriptKeyword ContinueKeyword;
	Boolean CanSkipEvaluation;
	Boolean CanOutput;
	ScriptNode Parent;
	IEnumerable<ScriptNode> Children;
	ScriptNode GetChildrenImpl(Int32 index);
	void Accept(ScriptVisitor visitor);
	TResult Accept(ScriptVisitor<TResult> visitor);
	Object Evaluate(TemplateContext context);
	void PrintTo(ScriptPrinter printer);
	ScriptNode Clone();
	ScriptNode Clone(Boolean withTrivias);
	ScriptNode GetChildren(Int32 index);
	Boolean CanHaveLeadingTrivia();
	void ParentToThis(TSyntaxNode& set, TSyntaxNode node);
	String ToString();



public class ScriptElseStatement:ScriptConditionStatement
	SourceSpan Span;
	Int32 ChildrenCount;
	ScriptKeyword ElseKeyword;
	ScriptBlockStatement Body;
	Boolean CanSkipEvaluation;
	Boolean CanOutput;
	ScriptNode Parent;
	IEnumerable<ScriptNode> Children;
	ScriptNode GetChildrenImpl(Int32 index);
	void Accept(ScriptVisitor visitor);
	TResult Accept(ScriptVisitor<TResult> visitor);
	Object Evaluate(TemplateContext context);
	void PrintTo(ScriptPrinter printer);
	ScriptNode Clone();
	ScriptNode Clone(Boolean withTrivias);
	ScriptNode GetChildren(Int32 index);
	Boolean CanHaveLeadingTrivia();
	void ParentToThis(TSyntaxNode& set, TSyntaxNode node);
	String ToString();



public class ScriptEndStatement:ScriptStatement
	SourceSpan Span;
	Int32 ChildrenCount;
	ScriptKeyword EndKeyword;
	Boolean ExpectEos;
	Boolean CanSkipEvaluation;
	Boolean CanOutput;
	ScriptNode Parent;
	IEnumerable<ScriptNode> Children;
	ScriptNode GetChildrenImpl(Int32 index);
	void Accept(ScriptVisitor visitor);
	TResult Accept(ScriptVisitor<TResult> visitor);
	Object Evaluate(TemplateContext context);
	void PrintTo(ScriptPrinter printer);
	ScriptNode Clone();
	ScriptNode Clone(Boolean withTrivias);
	ScriptNode GetChildren(Int32 index);
	Boolean CanHaveLeadingTrivia();
	void ParentToThis(TSyntaxNode& set, TSyntaxNode node);
	String ToString();



public class ScriptEscapeStatement:ScriptStatement, IScriptTerminal
	SourceSpan Span;
	Int32 ChildrenCount;
	ScriptTrivias Trivias;
	ScriptWhitespaceMode WhitespaceMode;
	String Indent;
	Boolean IsEntering;
	Boolean IsClosing;
	Int32 EscapeCount;
	Boolean CanSkipEvaluation;
	Boolean CanOutput;
	ScriptNode Parent;
	IEnumerable<ScriptNode> Children;
	ScriptNode GetChildrenImpl(Int32 index);
	void Accept(ScriptVisitor visitor);
	TResult Accept(ScriptVisitor<TResult> visitor);
	Object Evaluate(TemplateContext context);
	void PrintTo(ScriptPrinter printer);
	void WriteWhitespaceMode(ScriptPrinter printer);
	ScriptNode Clone();
	ScriptNode Clone(Boolean withTrivias);
	ScriptNode GetChildren(Int32 index);
	Boolean CanHaveLeadingTrivia();
	void ParentToThis(TSyntaxNode& set, TSyntaxNode node);
	String ToString();



public class ScriptExpressionStatement:ScriptStatement
	SourceSpan Span;
	Int32 ChildrenCount;
	ScriptExpression Expression;
	Boolean CanSkipEvaluation;
	Boolean CanOutput;
	ScriptNode Parent;
	IEnumerable<ScriptNode> Children;
	ScriptNode GetChildrenImpl(Int32 index);
	void Accept(ScriptVisitor visitor);
	TResult Accept(ScriptVisitor<TResult> visitor);
	Object Evaluate(TemplateContext context);
	void PrintTo(ScriptPrinter printer);
	ScriptNode Clone();
	ScriptNode Clone(Boolean withTrivias);
	ScriptNode GetChildren(Int32 index);
	Boolean CanHaveLeadingTrivia();
	void ParentToThis(TSyntaxNode& set, TSyntaxNode node);
	String ToString();



public class ScriptForStatement:ScriptLoopStatementBase, IScriptNamedArgumentContainer
	SourceSpan Span;
	Int32 ChildrenCount;
	ScriptKeyword ForOrTableRowKeyword;
	ScriptExpression Variable;
	ScriptKeyword InKeyword;
	ScriptExpression Iterator;
	ScriptList<ScriptNamedArgument> NamedArguments;
	ScriptBlockStatement Body;
	ScriptElseStatement Else;
	Boolean SetContinue;
	Boolean CanSkipEvaluation;
	Boolean CanOutput;
	ScriptNode Parent;
	IEnumerable<ScriptNode> Children;
	ScriptNode GetChildrenImpl(Int32 index);
	void Accept(ScriptVisitor visitor);
	TResult Accept(ScriptVisitor<TResult> visitor);
	Object LoopItem(TemplateContext context, LoopState state);
	ScriptVariable GetLoopVariable(TemplateContext context);
	Object EvaluateImpl(TemplateContext context);
	void PrintTo(ScriptPrinter printer);
	void ProcessArgument(TemplateContext context, ScriptNamedArgument argument);
	void BeforeLoop(TemplateContext context);
	LoopState CreateLoopState();
	Boolean ContinueLoop(TemplateContext context);
	void AfterLoop(TemplateContext context);
	Object Evaluate(TemplateContext context);
	ScriptNode Clone();
	ScriptNode Clone(Boolean withTrivias);
	ScriptNode GetChildren(Int32 index);
	Boolean CanHaveLeadingTrivia();
	void ParentToThis(TSyntaxNode& set, TSyntaxNode node);
	String ToString();



public class ScriptFrontMatter:ScriptStatement
	TextPosition TextPositionAfterEndMarker;
	SourceSpan Span;
	Int32 ChildrenCount;
	ScriptToken StartMarker;
	ScriptBlockStatement Statements;
	ScriptToken EndMarker;
	Boolean CanSkipEvaluation;
	Boolean CanOutput;
	ScriptNode Parent;
	IEnumerable<ScriptNode> Children;
	ScriptNode GetChildrenImpl(Int32 index);
	void Accept(ScriptVisitor visitor);
	TResult Accept(ScriptVisitor<TResult> visitor);
	Object Evaluate(TemplateContext context);
	void PrintTo(ScriptPrinter printer);
	ScriptNode Clone();
	ScriptNode Clone(Boolean withTrivias);
	ScriptNode GetChildren(Int32 index);
	Boolean CanHaveLeadingTrivia();
	void ParentToThis(TSyntaxNode& set, TSyntaxNode node);
	String ToString();



public class ScriptFunction:ScriptStatement, IScriptCustomFunction, IScriptFunctionInfo
	SourceSpan Span;
	Int32 ChildrenCount;
	ScriptKeyword FuncToken;
	ScriptNode NameOrDoToken;
	ScriptToken OpenParen;
	ScriptList<ScriptParameter> Parameters;
	ScriptToken CloseParen;
	ScriptToken EqualToken;
	ScriptStatement Body;
	Boolean IsAnonymous;
	Boolean HasParameters;
	Int32 RequiredParameterCount;
	Int32 ParameterCount;
	ScriptVarParamKind VarParamKind;
	Type ReturnType;
	Boolean CanSkipEvaluation;
	Boolean CanOutput;
	ScriptNode Parent;
	IEnumerable<ScriptNode> Children;
	ScriptNode GetChildrenImpl(Int32 index);
	void Accept(ScriptVisitor visitor);
	TResult Accept(ScriptVisitor<TResult> visitor);
	void UpdateReturnType();
	Object Evaluate(TemplateContext context);
	Boolean CanHaveLeadingTrivia();
	void PrintTo(ScriptPrinter printer);
	Object Invoke(TemplateContext context, ScriptNode callerContext, ScriptArray arguments, ScriptBlockStatement blockStatement);
	ScriptParameterInfo GetParameterInfo(Int32 index);
	ScriptNode Clone();
	ScriptNode Clone(Boolean withTrivias);
	ScriptNode GetChildren(Int32 index);
	void ParentToThis(TSyntaxNode& set, TSyntaxNode node);
	String ToString();



public class ScriptFunctionCall:ScriptExpression
	Int32 MaximumParameterCount;
	SourceSpan Span;
	Int32 ChildrenCount;
	ScriptExpression Target;
	ScriptToken OpenParent;
	ScriptList<ScriptExpression> Arguments;
	ScriptToken CloseParen;
	Boolean ExplicitCall;
	ScriptNode Parent;
	IEnumerable<ScriptNode> Children;
	ScriptNode GetChildrenImpl(Int32 index);
	void Accept(ScriptVisitor visitor);
	TResult Accept(ScriptVisitor<TResult> visitor);
	Boolean TryGetFunctionDeclaration(ScriptFunction& function);
	void AddArgument(ScriptExpression argument);
	ScriptExpression GetScientificExpression(TemplateContext context);
	Object Evaluate(TemplateContext context);
	void PrintTo(ScriptPrinter printer);
	Boolean CanHaveLeadingTrivia();
	Boolean IsFunction(Object target);
	Object Call(TemplateContext context, ScriptNode callerContext, Object functionObject, Boolean processPipeArguments, IReadOnlyList<ScriptExpression> arguments);
	Object Call(TemplateContext context, ScriptNode callerContext, IScriptCustomFunction function, ScriptArray arguments);
	UInt64 ProcessArguments(TemplateContext context, ScriptNode callerContext, IReadOnlyList<ScriptExpression> arguments, IScriptCustomFunction function, ScriptFunction scriptFunction, ScriptArray argumentValues);
	void SetArgumentValue(Int32 index, Object value, IScriptCustomFunction function, UInt64& argMask, ScriptArray argumentValues, Int32 parameterCount);
	void FillRemainingOptionalArguments(UInt64& argMask, Int32 startIndex, Int32 endIndex, IScriptCustomFunction function, ScriptArray argumentValues);
	Int32 GetParameterIndexByName(IScriptFunctionInfo functionInfo, String name);
	ScriptNode Clone();
	ScriptNode Clone(Boolean withTrivias);
	ScriptNode GetChildren(Int32 index);
	void ParentToThis(TSyntaxNode& set, TSyntaxNode node);
	String ToString();



public class ScriptIdentifier:ScriptVerbatim, IScriptTerminal
	SourceSpan Span;
	Int32 ChildrenCount;
	ScriptTrivias Trivias;
	String Value;
	ScriptNode Parent;
	IEnumerable<ScriptNode> Children;
	ScriptNode GetChildrenImpl(Int32 index);
	void Accept(ScriptVisitor visitor);
	TResult Accept(ScriptVisitor<TResult> visitor);
	Object Evaluate(TemplateContext context);
	void PrintTo(ScriptPrinter printer);
	ScriptNode Clone();
	ScriptNode Clone(Boolean withTrivias);
	ScriptNode GetChildren(Int32 index);
	Boolean CanHaveLeadingTrivia();
	void ParentToThis(TSyntaxNode& set, TSyntaxNode node);
	String ToString();



public class ScriptIfStatement:ScriptConditionStatement
	SourceSpan Span;
	Int32 ChildrenCount;
	ScriptKeyword ElseKeyword;
	ScriptKeyword IfKeyword;
	ScriptExpression Condition;
	ScriptBlockStatement Then;
	ScriptConditionStatement Else;
	Boolean IsElseIf;
	Boolean CanSkipEvaluation;
	Boolean CanOutput;
	ScriptNode Parent;
	IEnumerable<ScriptNode> Children;
	ScriptNode GetChildrenImpl(Int32 index);
	void Accept(ScriptVisitor visitor);
	TResult Accept(ScriptVisitor<TResult> visitor);
	Object Evaluate(TemplateContext context);
	void PrintTo(ScriptPrinter printer);
	ScriptNode Clone();
	ScriptNode Clone(Boolean withTrivias);
	ScriptNode GetChildren(Int32 index);
	Boolean CanHaveLeadingTrivia();
	void ParentToThis(TSyntaxNode& set, TSyntaxNode node);
	String ToString();



public class ScriptImportStatement:ScriptStatement
	SourceSpan Span;
	Int32 ChildrenCount;
	ScriptKeyword ImportKeyword;
	ScriptExpression Expression;
	Boolean CanSkipEvaluation;
	Boolean CanOutput;
	ScriptNode Parent;
	IEnumerable<ScriptNode> Children;
	ScriptNode GetChildrenImpl(Int32 index);
	void Accept(ScriptVisitor visitor);
	TResult Accept(ScriptVisitor<TResult> visitor);
	Object Evaluate(TemplateContext context);
	void PrintTo(ScriptPrinter printer);
	ScriptNode Clone();
	ScriptNode Clone(Boolean withTrivias);
	ScriptNode GetChildren(Int32 index);
	Boolean CanHaveLeadingTrivia();
	void ParentToThis(TSyntaxNode& set, TSyntaxNode node);
	String ToString();



public class ScriptIncrementDecrementExpression:ScriptUnaryExpression
	SourceSpan Span;
	Int32 ChildrenCount;
	Boolean Post;
	ScriptUnaryOperator Operator;
	ScriptToken OperatorToken;
	String OperatorAsText;
	ScriptExpression Right;
	ScriptNode Parent;
	IEnumerable<ScriptNode> Children;
	ScriptNode GetChildrenImpl(Int32 index);
	void Accept(ScriptVisitor visitor);
	TResult Accept(ScriptVisitor<TResult> visitor);
	Object Evaluate(TemplateContext context);
	void PrintTo(ScriptPrinter printer);
	void PrintOperator(ScriptPrinter printer);
	Object Evaluate(TemplateContext context, SourceSpan span, ScriptUnaryOperator op, Object value);
	ScriptUnaryExpression Wrap(ScriptUnaryOperator unaryOperator, ScriptToken unaryToken, ScriptExpression expression, Boolean transferTrivia);
	ScriptNode Clone();
	ScriptNode Clone(Boolean withTrivias);
	ScriptNode GetChildren(Int32 index);
	Boolean CanHaveLeadingTrivia();
	void ParentToThis(TSyntaxNode& set, TSyntaxNode node);
	String ToString();



public class ScriptIndexerExpression:ScriptExpression, IScriptVariablePath
	SourceSpan Span;
	Int32 ChildrenCount;
	ScriptExpression Target;
	ScriptToken OpenBracket;
	ScriptExpression Index;
	ScriptToken CloseBracket;
	ScriptNode Parent;
	IEnumerable<ScriptNode> Children;
	ScriptNode GetChildrenImpl(Int32 index);
	void Accept(ScriptVisitor visitor);
	TResult Accept(ScriptVisitor<TResult> visitor);
	Object Evaluate(TemplateContext context);
	Boolean CanHaveLeadingTrivia();
	void PrintTo(ScriptPrinter printer);
	Object GetValue(TemplateContext context);
	void SetValue(TemplateContext context, Object valueToSet);
	String GetFirstPath();
	Object GetOrSetValue(TemplateContext context, Object valueToSet, Boolean setter);
	ScriptNode Clone();
	ScriptNode Clone(Boolean withTrivias);
	ScriptNode GetChildren(Int32 index);
	void ParentToThis(TSyntaxNode& set, TSyntaxNode node);
	String ToString();



public class ScriptInterpolatedExpression:ScriptExpression
	SourceSpan Span;
	Int32 ChildrenCount;
	ScriptToken OpenBrace;
	ScriptExpression Expression;
	ScriptToken CloseBrace;
	ScriptNode Parent;
	IEnumerable<ScriptNode> Children;
	ScriptNode GetChildrenImpl(Int32 index);
	void Accept(ScriptVisitor visitor);
	TResult Accept(ScriptVisitor<TResult> visitor);
	Object Evaluate(TemplateContext context);
	void PrintTo(ScriptPrinter printer);
	ScriptNode Clone();
	ScriptNode Clone(Boolean withTrivias);
	ScriptNode GetChildren(Int32 index);
	Boolean CanHaveLeadingTrivia();
	void ParentToThis(TSyntaxNode& set, TSyntaxNode node);
	String ToString();



public class ScriptInterpolatedStringExpression:ScriptExpression
	SourceSpan Span;
	Int32 ChildrenCount;
	ScriptList<ScriptExpression> Parts;
	ScriptNode Parent;
	IEnumerable<ScriptNode> Children;
	ScriptNode GetChildrenImpl(Int32 index);
	void Accept(ScriptVisitor visitor);
	TResult Accept(ScriptVisitor<TResult> visitor);
	Object Evaluate(TemplateContext context);
	void PrintTo(ScriptPrinter printer);
	ScriptNode Clone();
	ScriptNode Clone(Boolean withTrivias);
	ScriptNode GetChildren(Int32 index);
	Boolean CanHaveLeadingTrivia();
	void ParentToThis(TSyntaxNode& set, TSyntaxNode node);
	String ToString();



public class ScriptIsEmptyExpression:ScriptMemberExpression, IScriptVariablePath
	SourceSpan Span;
	Int32 ChildrenCount;
	ScriptToken QuestionToken;
	ScriptExpression Target;
	ScriptToken DotToken;
	ScriptVariable Member;
	ScriptNode Parent;
	IEnumerable<ScriptNode> Children;
	ScriptNode GetChildrenImpl(Int32 index);
	void Accept(ScriptVisitor visitor);
	TResult Accept(ScriptVisitor<TResult> visitor);
	Object Evaluate(TemplateContext context);
	void PrintTo(ScriptPrinter printer);
	Boolean CanHaveLeadingTrivia();
	Object GetValue(TemplateContext context);
	void SetValue(TemplateContext context, Object valueToSet);
	String GetFirstPath();
	Object GetTargetObject(TemplateContext context, Boolean isSet);
	ScriptNode Clone();
	ScriptNode Clone(Boolean withTrivias);
	ScriptNode GetChildren(Int32 index);
	void ParentToThis(TSyntaxNode& set, TSyntaxNode node);
	String ToString();



public class ScriptKeyword:ScriptVerbatim, IScriptTerminal
	SourceSpan Span;
	Int32 ChildrenCount;
	ScriptTrivias Trivias;
	String Value;
	ScriptNode Parent;
	IEnumerable<ScriptNode> Children;
	ScriptNode GetChildrenImpl(Int32 index);
	void Accept(ScriptVisitor visitor);
	TResult Accept(ScriptVisitor<TResult> visitor);
	ScriptKeyword This();
	ScriptKeyword Func();
	ScriptKeyword Do();
	ScriptKeyword Break();
	ScriptKeyword Capture();
	ScriptKeyword Case();
	ScriptKeyword Continue();
	ScriptKeyword Else();
	ScriptKeyword End();
	ScriptKeyword If();
	ScriptKeyword In();
	ScriptKeyword For();
	ScriptKeyword Import();
	ScriptKeyword ReadOnly();
	ScriptKeyword Ret();
	ScriptKeyword TableRow();
	ScriptKeyword When();
	ScriptKeyword While();
	ScriptKeyword With();
	ScriptKeyword Wrap();
	Object Evaluate(TemplateContext context);
	void PrintTo(ScriptPrinter printer);
	ScriptNode Clone();
	ScriptNode Clone(Boolean withTrivias);
	ScriptNode GetChildren(Int32 index);
	Boolean CanHaveLeadingTrivia();
	void ParentToThis(TSyntaxNode& set, TSyntaxNode node);
	String ToString();



public class ScriptLiteral:ScriptExpression, IScriptTerminal
	SourceSpan Span;
	Int32 ChildrenCount;
	ScriptTrivias Trivias;
	Object Value;
	ScriptLiteralStringQuoteType StringQuoteType;
	TokenType StringTokenType;
	ScriptNode Parent;
	IEnumerable<ScriptNode> Children;
	ScriptNode GetChildrenImpl(Int32 index);
	void Accept(ScriptVisitor visitor);
	TResult Accept(ScriptVisitor<TResult> visitor);
	Object Evaluate(TemplateContext context);
	Boolean IsPositiveInteger();
	void PrintTo(ScriptPrinter printer);
	String ToLiteral(ScriptLiteralStringQuoteType quoteType, TokenType stringTokenType, String input);
	String AppendDecimalPoint(String text, Boolean hasNaN);
	ScriptNode Clone();
	ScriptNode Clone(Boolean withTrivias);
	ScriptNode GetChildren(Int32 index);
	Boolean CanHaveLeadingTrivia();
	void ParentToThis(TSyntaxNode& set, TSyntaxNode node);
	String ToString();



public class ScriptMemberExpression:ScriptExpression, IScriptVariablePath
	SourceSpan Span;
	Int32 ChildrenCount;
	ScriptExpression Target;
	ScriptToken DotToken;
	ScriptVariable Member;
	ScriptNode Parent;
	IEnumerable<ScriptNode> Children;
	ScriptNode GetChildrenImpl(Int32 index);
	void Accept(ScriptVisitor visitor);
	TResult Accept(ScriptVisitor<TResult> visitor);
	Object Evaluate(TemplateContext context);
	void PrintTo(ScriptPrinter printer);
	Boolean CanHaveLeadingTrivia();
	Object GetValue(TemplateContext context);
	void SetValue(TemplateContext context, Object valueToSet);
	String GetFirstPath();
	Object GetTargetObject(TemplateContext context, Boolean isSet);
	ScriptNode Clone();
	ScriptNode Clone(Boolean withTrivias);
	ScriptNode GetChildren(Int32 index);
	void ParentToThis(TSyntaxNode& set, TSyntaxNode node);
	String ToString();



public class ScriptNamedArgument:ScriptExpression
	SourceSpan Span;
	Int32 ChildrenCount;
	ScriptVariable Name;
	ScriptToken ColonToken;
	ScriptExpression Value;
	ScriptNode Parent;
	IEnumerable<ScriptNode> Children;
	ScriptNode GetChildrenImpl(Int32 index);
	void Accept(ScriptVisitor visitor);
	TResult Accept(ScriptVisitor<TResult> visitor);
	Object Evaluate(TemplateContext context);
	void PrintTo(ScriptPrinter printer);
	ScriptNode Clone();
	ScriptNode Clone(Boolean withTrivias);
	ScriptNode GetChildren(Int32 index);
	Boolean CanHaveLeadingTrivia();
	void ParentToThis(TSyntaxNode& set, TSyntaxNode node);
	String ToString();



public class ScriptNestedExpression:ScriptExpression, IScriptVariablePath
	SourceSpan Span;
	Int32 ChildrenCount;
	ScriptToken OpenParen;
	ScriptExpression Expression;
	ScriptToken CloseParen;
	ScriptNode Parent;
	IEnumerable<ScriptNode> Children;
	ScriptNode GetChildrenImpl(Int32 index);
	void Accept(ScriptVisitor visitor);
	TResult Accept(ScriptVisitor<TResult> visitor);
	ScriptNestedExpression Wrap(ScriptExpression expression, Boolean transferTrivia);
	Object Evaluate(TemplateContext context);
	void PrintTo(ScriptPrinter printer);
	Object GetValue(TemplateContext context);
	void SetValue(TemplateContext context, Object valueToSet);
	String GetFirstPath();
	ScriptNode Clone();
	ScriptNode Clone(Boolean withTrivias);
	ScriptNode GetChildren(Int32 index);
	Boolean CanHaveLeadingTrivia();
	void ParentToThis(TSyntaxNode& set, TSyntaxNode node);
	String ToString();



public class ScriptNopStatement:ScriptStatement
	SourceSpan Span;
	Int32 ChildrenCount;
	Boolean CanSkipEvaluation;
	Boolean CanOutput;
	ScriptNode Parent;
	IEnumerable<ScriptNode> Children;
	ScriptNode GetChildrenImpl(Int32 index);
	void Accept(ScriptVisitor visitor);
	TResult Accept(ScriptVisitor<TResult> visitor);
	Object Evaluate(TemplateContext context);
	void PrintTo(ScriptPrinter printer);
	ScriptNode Clone();
	ScriptNode Clone(Boolean withTrivias);
	ScriptNode GetChildren(Int32 index);
	Boolean CanHaveLeadingTrivia();
	void ParentToThis(TSyntaxNode& set, TSyntaxNode node);
	String ToString();



public class ScriptObjectInitializerExpression:ScriptExpression
	SourceSpan Span;
	Int32 ChildrenCount;
	ScriptToken OpenBrace;
	ScriptList<ScriptObjectMember> Members;
	ScriptToken CloseBrace;
	ScriptNode Parent;
	IEnumerable<ScriptNode> Children;
	ScriptNode GetChildrenImpl(Int32 index);
	void Accept(ScriptVisitor visitor);
	TResult Accept(ScriptVisitor<TResult> visitor);
	Object Evaluate(TemplateContext context);
	void PrintTo(ScriptPrinter printer);
	ScriptNode Clone();
	ScriptNode Clone(Boolean withTrivias);
	ScriptNode GetChildren(Int32 index);
	Boolean CanHaveLeadingTrivia();
	void ParentToThis(TSyntaxNode& set, TSyntaxNode node);
	String ToString();



public class ScriptObjectMember:ScriptNode
	SourceSpan Span;
	Int32 ChildrenCount;
	ScriptExpression Name;
	ScriptToken ColonToken;
	ScriptExpression Value;
	ScriptNode Parent;
	IEnumerable<ScriptNode> Children;
	ScriptNode GetChildrenImpl(Int32 index);
	void Accept(ScriptVisitor visitor);
	TResult Accept(ScriptVisitor<TResult> visitor);
	Object Evaluate(TemplateContext context);
	void PrintTo(ScriptPrinter printer);
	ScriptNode Clone();
	ScriptNode Clone(Boolean withTrivias);
	ScriptNode GetChildren(Int32 index);
	Boolean CanHaveLeadingTrivia();
	void ParentToThis(TSyntaxNode& set, TSyntaxNode node);
	String ToString();



public class ScriptPage:ScriptNode
	SourceSpan Span;
	Int32 ChildrenCount;
	ScriptFrontMatter FrontMatter;
	ScriptBlockStatement Body;
	ScriptNode Parent;
	IEnumerable<ScriptNode> Children;
	ScriptNode GetChildrenImpl(Int32 index);
	void Accept(ScriptVisitor visitor);
	TResult Accept(ScriptVisitor<TResult> visitor);
	Object Evaluate(TemplateContext context);
	void PrintTo(ScriptPrinter printer);
	ScriptNode Clone();
	ScriptNode Clone(Boolean withTrivias);
	ScriptNode GetChildren(Int32 index);
	Boolean CanHaveLeadingTrivia();
	void ParentToThis(TSyntaxNode& set, TSyntaxNode node);
	String ToString();



public class ScriptParameter:ScriptNode
	SourceSpan Span;
	Int32 ChildrenCount;
	ScriptVariable Name;
	ScriptToken EqualOrTripleDotToken;
	ScriptLiteral DefaultValue;
	ScriptNode Parent;
	IEnumerable<ScriptNode> Children;
	ScriptNode GetChildrenImpl(Int32 index);
	void Accept(ScriptVisitor visitor);
	TResult Accept(ScriptVisitor<TResult> visitor);
	Object Evaluate(TemplateContext context);
	void PrintTo(ScriptPrinter printer);
	ScriptNode Clone();
	ScriptNode Clone(Boolean withTrivias);
	ScriptNode GetChildren(Int32 index);
	Boolean CanHaveLeadingTrivia();
	void ParentToThis(TSyntaxNode& set, TSyntaxNode node);
	String ToString();



public class ScriptPipeCall:ScriptExpression
	SourceSpan Span;
	Int32 ChildrenCount;
	ScriptExpression From;
	ScriptToken PipeToken;
	ScriptExpression To;
	ScriptNode Parent;
	IEnumerable<ScriptNode> Children;
	ScriptNode GetChildrenImpl(Int32 index);
	void Accept(ScriptVisitor visitor);
	TResult Accept(ScriptVisitor<TResult> visitor);
	Object Evaluate(TemplateContext context);
	Boolean CanHaveLeadingTrivia();
	void PrintTo(ScriptPrinter printer);
	ScriptNode Clone();
	ScriptNode Clone(Boolean withTrivias);
	ScriptNode GetChildren(Int32 index);
	void ParentToThis(TSyntaxNode& set, TSyntaxNode node);
	String ToString();



public class ScriptRawStatement:ScriptStatement
	SourceSpan Span;
	Int32 ChildrenCount;
	ScriptStringSlice Text;
	Boolean IsEscape;
	Boolean CanSkipEvaluation;
	Boolean CanOutput;
	ScriptNode Parent;
	IEnumerable<ScriptNode> Children;
	ScriptNode GetChildrenImpl(Int32 index);
	void Accept(ScriptVisitor visitor);
	TResult Accept(ScriptVisitor<TResult> visitor);
	Object Evaluate(TemplateContext context);
	void PrintTo(ScriptPrinter printer);
	ScriptNode Clone();
	ScriptNode Clone(Boolean withTrivias);
	ScriptNode GetChildren(Int32 index);
	Boolean CanHaveLeadingTrivia();
	void ParentToThis(TSyntaxNode& set, TSyntaxNode node);
	String ToString();



public class ScriptReadOnlyStatement:ScriptStatement
	SourceSpan Span;
	Int32 ChildrenCount;
	ScriptKeyword ReadOnlyKeyword;
	ScriptVariable Variable;
	Boolean CanSkipEvaluation;
	Boolean CanOutput;
	ScriptNode Parent;
	IEnumerable<ScriptNode> Children;
	ScriptNode GetChildrenImpl(Int32 index);
	void Accept(ScriptVisitor visitor);
	TResult Accept(ScriptVisitor<TResult> visitor);
	Object Evaluate(TemplateContext context);
	void PrintTo(ScriptPrinter printer);
	ScriptNode Clone();
	ScriptNode Clone(Boolean withTrivias);
	ScriptNode GetChildren(Int32 index);
	Boolean CanHaveLeadingTrivia();
	void ParentToThis(TSyntaxNode& set, TSyntaxNode node);
	String ToString();



public class ScriptReturnStatement:ScriptStatement
	SourceSpan Span;
	Int32 ChildrenCount;
	ScriptKeyword RetKeyword;
	ScriptExpression Expression;
	Boolean CanSkipEvaluation;
	Boolean CanOutput;
	ScriptNode Parent;
	IEnumerable<ScriptNode> Children;
	ScriptNode GetChildrenImpl(Int32 index);
	void Accept(ScriptVisitor visitor);
	TResult Accept(ScriptVisitor<TResult> visitor);
	Object Evaluate(TemplateContext context);
	void PrintTo(ScriptPrinter printer);
	ScriptNode Clone();
	ScriptNode Clone(Boolean withTrivias);
	ScriptNode GetChildren(Int32 index);
	Boolean CanHaveLeadingTrivia();
	void ParentToThis(TSyntaxNode& set, TSyntaxNode node);
	String ToString();



public class ScriptTableRowStatement:ScriptForStatement, IScriptNamedArgumentContainer
	SourceSpan Span;
	Int32 ChildrenCount;
	ScriptKeyword ForOrTableRowKeyword;
	ScriptExpression Variable;
	ScriptKeyword InKeyword;
	ScriptExpression Iterator;
	ScriptList<ScriptNamedArgument> NamedArguments;
	ScriptBlockStatement Body;
	ScriptElseStatement Else;
	Boolean SetContinue;
	Boolean CanSkipEvaluation;
	Boolean CanOutput;
	ScriptNode Parent;
	IEnumerable<ScriptNode> Children;
	ScriptNode GetChildrenImpl(Int32 index);
	void Accept(ScriptVisitor visitor);
	TResult Accept(ScriptVisitor<TResult> visitor);
	ScriptVariable GetLoopVariable(TemplateContext context);
	void ProcessArgument(TemplateContext context, ScriptNamedArgument argument);
	void BeforeLoop(TemplateContext context);
	void AfterLoop(TemplateContext context);
	Object LoopItem(TemplateContext context, LoopState state);
	LoopState CreateLoopState();
	Object EvaluateImpl(TemplateContext context);
	void PrintTo(ScriptPrinter printer);
	Boolean ContinueLoop(TemplateContext context);
	Object Evaluate(TemplateContext context);
	ScriptNode Clone();
	ScriptNode Clone(Boolean withTrivias);
	ScriptNode GetChildren(Int32 index);
	Boolean CanHaveLeadingTrivia();
	void ParentToThis(TSyntaxNode& set, TSyntaxNode node);
	String ToString();



public class ScriptThisExpression:ScriptExpression, IScriptVariablePath
	SourceSpan Span;
	Int32 ChildrenCount;
	ScriptKeyword ThisKeyword;
	ScriptNode Parent;
	IEnumerable<ScriptNode> Children;
	ScriptNode GetChildrenImpl(Int32 index);
	void Accept(ScriptVisitor visitor);
	TResult Accept(ScriptVisitor<TResult> visitor);
	Object Evaluate(TemplateContext context);
	void PrintTo(ScriptPrinter printer);
	Object GetValue(TemplateContext context);
	void SetValue(TemplateContext context, Object valueToSet);
	String GetFirstPath();
	ScriptNode Clone();
	ScriptNode Clone(Boolean withTrivias);
	ScriptNode GetChildren(Int32 index);
	Boolean CanHaveLeadingTrivia();
	void ParentToThis(TSyntaxNode& set, TSyntaxNode node);
	String ToString();



public class ScriptToken:ScriptVerbatim, IScriptTerminal
	SourceSpan Span;
	Int32 ChildrenCount;
	TokenType TokenType;
	ScriptTrivias Trivias;
	String Value;
	ScriptNode Parent;
	IEnumerable<ScriptNode> Children;
	ScriptNode GetChildrenImpl(Int32 index);
	void Accept(ScriptVisitor visitor);
	TResult Accept(ScriptVisitor<TResult> visitor);
	ScriptToken SemiColon();
	ScriptToken Arroba();
	ScriptToken Caret();
	ScriptToken DoubleCaret();
	ScriptToken Colon();
	ScriptToken Equal();
	ScriptToken Pipe();
	ScriptToken PipeGreater();
	ScriptToken Exclamation();
	ScriptToken DoubleAmp();
	ScriptToken DoublePipe();
	ScriptToken Amp();
	ScriptToken Question();
	ScriptToken DoubleQuestion();
	ScriptToken QuestionExclamation();
	ScriptToken CompareEqual();
	ScriptToken CompareNotEqual();
	ScriptToken CompareLess();
	ScriptToken CompareGreater();
	ScriptToken CompareLessOrEqual();
	ScriptToken CompareGreaterOrEqual();
	ScriptToken Divide();
	ScriptToken DivideEqual();
	ScriptToken DoubleDivide();
	ScriptToken DoubleDivideEqual();
	ScriptToken Star();
	ScriptToken StarEqual();
	ScriptToken Plus();
	ScriptToken PlusEqual();
	ScriptToken Minus();
	ScriptToken MinusEqual();
	ScriptToken Modulus();
	ScriptToken ModulusEqual();
	ScriptToken DoubleLess();
	ScriptToken DoubleGreater();
	ScriptToken Comma();
	ScriptToken Dot();
	ScriptToken DoubleDot();
	ScriptToken TripleDot();
	ScriptToken DoubleDotLess();
	ScriptToken OpenParen();
	ScriptToken CloseParen();
	ScriptToken OpenBrace();
	ScriptToken CloseBrace();
	ScriptToken OpenBracket();
	ScriptToken CloseBracket();
	ScriptToken OpenInterpBrace();
	ScriptToken CloseInterpBrace();
	Object Evaluate(TemplateContext context);
	void PrintTo(ScriptPrinter printer);
	ScriptNode Clone();
	ScriptNode Clone(Boolean withTrivias);
	ScriptNode GetChildren(Int32 index);
	Boolean CanHaveLeadingTrivia();
	void ParentToThis(TSyntaxNode& set, TSyntaxNode node);
	String ToString();



public class ScriptUnaryExpression:ScriptExpression
	SourceSpan Span;
	Int32 ChildrenCount;
	ScriptUnaryOperator Operator;
	ScriptToken OperatorToken;
	String OperatorAsText;
	ScriptExpression Right;
	ScriptNode Parent;
	IEnumerable<ScriptNode> Children;
	ScriptNode GetChildrenImpl(Int32 index);
	void Accept(ScriptVisitor visitor);
	TResult Accept(ScriptVisitor<TResult> visitor);
	Object Evaluate(TemplateContext context);
	void PrintTo(ScriptPrinter printer);
	Object Evaluate(TemplateContext context, SourceSpan span, ScriptUnaryOperator op, Object value);
	ScriptUnaryExpression Wrap(ScriptUnaryOperator unaryOperator, ScriptToken unaryToken, ScriptExpression expression, Boolean transferTrivia);
	ScriptNode Clone();
	ScriptNode Clone(Boolean withTrivias);
	ScriptNode GetChildren(Int32 index);
	Boolean CanHaveLeadingTrivia();
	void ParentToThis(TSyntaxNode& set, TSyntaxNode node);
	String ToString();



public class ScriptVariableGlobal:ScriptVariable, IScriptVariablePath, IEquatable<ScriptVariable>, IScriptTerminal
	SourceSpan Span;
	Int32 ChildrenCount;
	ScriptTrivias Trivias;
	String BaseName;
	String Name;
	ScriptVariableScope Scope;
	ScriptNode Parent;
	IEnumerable<ScriptNode> Children;
	ScriptNode GetChildrenImpl(Int32 index);
	void Accept(ScriptVisitor visitor);
	TResult Accept(ScriptVisitor<TResult> visitor);
	Object GetValue(TemplateContext context);
	ScriptVariable Create(String name, ScriptVariableScope scope);
	String GetFirstPath();
	Boolean Equals(ScriptVariable other);
	Boolean Equals(Object obj);
	Int32 GetHashCode();
	Object Evaluate(TemplateContext context);
	void SetValue(TemplateContext context, Object valueToSet);
	void PrintTo(ScriptPrinter printer);
	ScriptNode Clone();
	ScriptNode Clone(Boolean withTrivias);
	ScriptNode GetChildren(Int32 index);
	Boolean CanHaveLeadingTrivia();
	void ParentToThis(TSyntaxNode& set, TSyntaxNode node);
	String ToString();



public class ScriptVariableLocal:ScriptVariable, IScriptVariablePath, IEquatable<ScriptVariable>, IScriptTerminal
	SourceSpan Span;
	Int32 ChildrenCount;
	ScriptTrivias Trivias;
	String BaseName;
	String Name;
	ScriptVariableScope Scope;
	ScriptNode Parent;
	IEnumerable<ScriptNode> Children;
	ScriptNode GetChildrenImpl(Int32 index);
	void Accept(ScriptVisitor visitor);
	TResult Accept(ScriptVisitor<TResult> visitor);
	ScriptVariable Create(String name, ScriptVariableScope scope);
	String GetFirstPath();
	Boolean Equals(ScriptVariable other);
	Boolean Equals(Object obj);
	Int32 GetHashCode();
	Object Evaluate(TemplateContext context);
	Object GetValue(TemplateContext context);
	void SetValue(TemplateContext context, Object valueToSet);
	void PrintTo(ScriptPrinter printer);
	ScriptNode Clone();
	ScriptNode Clone(Boolean withTrivias);
	ScriptNode GetChildren(Int32 index);
	Boolean CanHaveLeadingTrivia();
	void ParentToThis(TSyntaxNode& set, TSyntaxNode node);
	String ToString();



public class ScriptWhenStatement:ScriptConditionStatement
	SourceSpan Span;
	Int32 ChildrenCount;
	ScriptKeyword WhenKeyword;
	ScriptList<ScriptExpression> Values;
	ScriptBlockStatement Body;
	ScriptConditionStatement Next;
	Boolean CanSkipEvaluation;
	Boolean CanOutput;
	ScriptNode Parent;
	IEnumerable<ScriptNode> Children;
	ScriptNode GetChildrenImpl(Int32 index);
	void Accept(ScriptVisitor visitor);
	TResult Accept(ScriptVisitor<TResult> visitor);
	Object Evaluate(TemplateContext context);
	void PrintTo(ScriptPrinter printer);
	ScriptNode Clone();
	ScriptNode Clone(Boolean withTrivias);
	ScriptNode GetChildren(Int32 index);
	Boolean CanHaveLeadingTrivia();
	void ParentToThis(TSyntaxNode& set, TSyntaxNode node);
	String ToString();



public class ScriptWhileStatement:ScriptLoopStatementBase
	SourceSpan Span;
	Int32 ChildrenCount;
	ScriptKeyword WhileKeyword;
	ScriptExpression Condition;
	ScriptBlockStatement Body;
	Boolean CanSkipEvaluation;
	Boolean CanOutput;
	ScriptNode Parent;
	IEnumerable<ScriptNode> Children;
	ScriptNode GetChildrenImpl(Int32 index);
	void Accept(ScriptVisitor visitor);
	TResult Accept(ScriptVisitor<TResult> visitor);
	Object LoopItem(TemplateContext context, LoopState state);
	Object EvaluateImpl(TemplateContext context);
	void PrintTo(ScriptPrinter printer);
	void BeforeLoop(TemplateContext context);
	LoopState CreateLoopState();
	Boolean ContinueLoop(TemplateContext context);
	void AfterLoop(TemplateContext context);
	Object Evaluate(TemplateContext context);
	ScriptNode Clone();
	ScriptNode Clone(Boolean withTrivias);
	ScriptNode GetChildren(Int32 index);
	Boolean CanHaveLeadingTrivia();
	void ParentToThis(TSyntaxNode& set, TSyntaxNode node);
	String ToString();



public class ScriptWithStatement:ScriptStatement
	SourceSpan Span;
	Int32 ChildrenCount;
	ScriptKeyword WithKeyword;
	ScriptExpression Name;
	ScriptBlockStatement Body;
	Boolean CanSkipEvaluation;
	Boolean CanOutput;
	ScriptNode Parent;
	IEnumerable<ScriptNode> Children;
	ScriptNode GetChildrenImpl(Int32 index);
	void Accept(ScriptVisitor visitor);
	TResult Accept(ScriptVisitor<TResult> visitor);
	Object Evaluate(TemplateContext context);
	void PrintTo(ScriptPrinter printer);
	ScriptNode Clone();
	ScriptNode Clone(Boolean withTrivias);
	ScriptNode GetChildren(Int32 index);
	Boolean CanHaveLeadingTrivia();
	void ParentToThis(TSyntaxNode& set, TSyntaxNode node);
	String ToString();



public class ScriptWrapStatement:ScriptStatement
	SourceSpan Span;
	Int32 ChildrenCount;
	ScriptKeyword WrapKeyword;
	ScriptExpression Target;
	ScriptBlockStatement Body;
	Boolean CanSkipEvaluation;
	Boolean CanOutput;
	ScriptNode Parent;
	IEnumerable<ScriptNode> Children;
	ScriptNode GetChildrenImpl(Int32 index);
	void Accept(ScriptVisitor visitor);
	TResult Accept(ScriptVisitor<TResult> visitor);
	Object Evaluate(TemplateContext context);
	void PrintTo(ScriptPrinter printer);
	ScriptNode Clone();
	ScriptNode Clone(Boolean withTrivias);
	ScriptNode GetChildren(Int32 index);
	Boolean CanHaveLeadingTrivia();
	void ParentToThis(TSyntaxNode& set, TSyntaxNode node);
	String ToString();



public class ScriptRewriter:ScriptVisitor<ScriptNode>
	Boolean CopyTrivias;
	ScriptNode Visit(ScriptAnonymousFunction node);
	ScriptNode Visit(ScriptArgumentBinary node);
	ScriptNode Visit(ScriptArrayInitializerExpression node);
	ScriptNode Visit(ScriptAssignExpression node);
	ScriptNode Visit(ScriptBinaryExpression node);
	ScriptNode Visit(ScriptBlockStatement node);
	ScriptNode Visit(ScriptBreakStatement node);
	ScriptNode Visit(ScriptCaptureStatement node);
	ScriptNode Visit(ScriptCaseStatement node);
	ScriptNode Visit(ScriptConditionalExpression node);
	ScriptNode Visit(ScriptContinueStatement node);
	ScriptNode Visit(ScriptElseStatement node);
	ScriptNode Visit(ScriptEndStatement node);
	ScriptNode Visit(ScriptEscapeStatement node);
	ScriptNode Visit(ScriptExpressionStatement node);
	ScriptNode Visit(ScriptForStatement node);
	ScriptNode Visit(ScriptFrontMatter node);
	ScriptNode Visit(ScriptFunction node);
	ScriptNode Visit(ScriptFunctionCall node);
	ScriptNode Visit(ScriptIdentifier node);
	ScriptNode Visit(ScriptIfStatement node);
	ScriptNode Visit(ScriptImportStatement node);
	ScriptNode Visit(ScriptIncrementDecrementExpression node);
	ScriptNode Visit(ScriptIndexerExpression node);
	ScriptNode Visit(ScriptInterpolatedExpression node);
	ScriptNode Visit(ScriptInterpolatedStringExpression node);
	ScriptNode Visit(ScriptIsEmptyExpression node);
	ScriptNode Visit(ScriptKeyword node);
	ScriptNode Visit(ScriptLiteral node);
	ScriptNode Visit(ScriptMemberExpression node);
	ScriptNode Visit(ScriptNamedArgument node);
	ScriptNode Visit(ScriptNestedExpression node);
	ScriptNode Visit(ScriptNopStatement node);
	ScriptNode Visit(ScriptObjectInitializerExpression node);
	ScriptNode Visit(ScriptObjectMember node);
	ScriptNode Visit(ScriptPage node);
	ScriptNode Visit(ScriptParameter node);
	ScriptNode Visit(ScriptPipeCall node);
	ScriptNode Visit(ScriptRawStatement node);
	ScriptNode Visit(ScriptReadOnlyStatement node);
	ScriptNode Visit(ScriptReturnStatement node);
	ScriptNode Visit(ScriptTableRowStatement node);
	ScriptNode Visit(ScriptThisExpression node);
	ScriptNode Visit(ScriptToken node);
	ScriptNode Visit(ScriptUnaryExpression node);
	ScriptNode Visit(ScriptWhenStatement node);
	ScriptNode Visit(ScriptWhileStatement node);
	ScriptNode Visit(ScriptWithStatement node);
	ScriptNode Visit(ScriptWrapStatement node);
	ScriptNode Visit(ScriptNode node);
	ScriptNode Visit(ScriptVariableGlobal node);
	ScriptNode Visit(ScriptVariableLocal node);
	ScriptList<TNode> VisitAll(ScriptList<TNode> nodes);
	ScriptNode DefaultVisit(ScriptNode node);



public class ScriptVisitor
	void Visit(ScriptAnonymousFunction node);
	void Visit(ScriptArgumentBinary node);
	void Visit(ScriptArrayInitializerExpression node);
	void Visit(ScriptAssignExpression node);
	void Visit(ScriptBinaryExpression node);
	void Visit(ScriptBlockStatement node);
	void Visit(ScriptBreakStatement node);
	void Visit(ScriptCaptureStatement node);
	void Visit(ScriptCaseStatement node);
	void Visit(ScriptConditionalExpression node);
	void Visit(ScriptContinueStatement node);
	void Visit(ScriptElseStatement node);
	void Visit(ScriptEndStatement node);
	void Visit(ScriptEscapeStatement node);
	void Visit(ScriptExpressionStatement node);
	void Visit(ScriptForStatement node);
	void Visit(ScriptFrontMatter node);
	void Visit(ScriptFunction node);
	void Visit(ScriptFunctionCall node);
	void Visit(ScriptIdentifier node);
	void Visit(ScriptIfStatement node);
	void Visit(ScriptImportStatement node);
	void Visit(ScriptIncrementDecrementExpression node);
	void Visit(ScriptIndexerExpression node);
	void Visit(ScriptInterpolatedExpression node);
	void Visit(ScriptInterpolatedStringExpression node);
	void Visit(ScriptIsEmptyExpression node);
	void Visit(ScriptKeyword node);
	void Visit(ScriptLiteral node);
	void Visit(ScriptMemberExpression node);
	void Visit(ScriptNamedArgument node);
	void Visit(ScriptNestedExpression node);
	void Visit(ScriptNopStatement node);
	void Visit(ScriptObjectInitializerExpression node);
	void Visit(ScriptObjectMember node);
	void Visit(ScriptPage node);
	void Visit(ScriptParameter node);
	void Visit(ScriptPipeCall node);
	void Visit(ScriptRawStatement node);
	void Visit(ScriptReadOnlyStatement node);
	void Visit(ScriptReturnStatement node);
	void Visit(ScriptTableRowStatement node);
	void Visit(ScriptThisExpression node);
	void Visit(ScriptToken node);
	void Visit(ScriptUnaryExpression node);
	void Visit(ScriptVariableGlobal node);
	void Visit(ScriptVariableLocal node);
	void Visit(ScriptWhenStatement node);
	void Visit(ScriptWhileStatement node);
	void Visit(ScriptWithStatement node);
	void Visit(ScriptWrapStatement node);
	void Visit(ScriptNode node);
	void Visit(ScriptList list);
	void DefaultVisit(ScriptNode node);



public class ScriptVisitor
	TResult Visit(ScriptAnonymousFunction node);
	TResult Visit(ScriptArgumentBinary node);
	TResult Visit(ScriptArrayInitializerExpression node);
	TResult Visit(ScriptAssignExpression node);
	TResult Visit(ScriptBinaryExpression node);
	TResult Visit(ScriptBlockStatement node);
	TResult Visit(ScriptBreakStatement node);
	TResult Visit(ScriptCaptureStatement node);
	TResult Visit(ScriptCaseStatement node);
	TResult Visit(ScriptConditionalExpression node);
	TResult Visit(ScriptContinueStatement node);
	TResult Visit(ScriptElseStatement node);
	TResult Visit(ScriptEndStatement node);
	TResult Visit(ScriptEscapeStatement node);
	TResult Visit(ScriptExpressionStatement node);
	TResult Visit(ScriptForStatement node);
	TResult Visit(ScriptFrontMatter node);
	TResult Visit(ScriptFunction node);
	TResult Visit(ScriptFunctionCall node);
	TResult Visit(ScriptIdentifier node);
	TResult Visit(ScriptIfStatement node);
	TResult Visit(ScriptImportStatement node);
	TResult Visit(ScriptIncrementDecrementExpression node);
	TResult Visit(ScriptIndexerExpression node);
	TResult Visit(ScriptInterpolatedExpression node);
	TResult Visit(ScriptInterpolatedStringExpression node);
	TResult Visit(ScriptIsEmptyExpression node);
	TResult Visit(ScriptKeyword node);
	TResult Visit(ScriptLiteral node);
	TResult Visit(ScriptMemberExpression node);
	TResult Visit(ScriptNamedArgument node);
	TResult Visit(ScriptNestedExpression node);
	TResult Visit(ScriptNopStatement node);
	TResult Visit(ScriptObjectInitializerExpression node);
	TResult Visit(ScriptObjectMember node);
	TResult Visit(ScriptPage node);
	TResult Visit(ScriptParameter node);
	TResult Visit(ScriptPipeCall node);
	TResult Visit(ScriptRawStatement node);
	TResult Visit(ScriptReadOnlyStatement node);
	TResult Visit(ScriptReturnStatement node);
	TResult Visit(ScriptTableRowStatement node);
	TResult Visit(ScriptThisExpression node);
	TResult Visit(ScriptToken node);
	TResult Visit(ScriptUnaryExpression node);
	TResult Visit(ScriptVariableGlobal node);
	TResult Visit(ScriptVariableLocal node);
	TResult Visit(ScriptWhenStatement node);
	TResult Visit(ScriptWhileStatement node);
	TResult Visit(ScriptWithStatement node);
	TResult Visit(ScriptWrapStatement node);
	TResult Visit(ScriptNode node);
	TResult DefaultVisit(ScriptNode node);



public class IScriptTerminal
	ScriptTrivias Trivias;



public class ScriptBinaryOperator:Enum, IComparable, IFormattable, IConvertible
	Int32 value__;
	ScriptBinaryOperator None;
	ScriptBinaryOperator EmptyCoalescing;
	ScriptBinaryOperator NotEmptyCoalescing;
	ScriptBinaryOperator Or;
	ScriptBinaryOperator And;
	ScriptBinaryOperator BinaryOr;
	ScriptBinaryOperator BinaryAnd;
	ScriptBinaryOperator CompareEqual;
	ScriptBinaryOperator CompareNotEqual;
	ScriptBinaryOperator CompareLessOrEqual;
	ScriptBinaryOperator CompareGreaterOrEqual;
	ScriptBinaryOperator CompareLess;
	ScriptBinaryOperator CompareGreater;
	ScriptBinaryOperator LiquidContains;
	ScriptBinaryOperator LiquidStartsWith;
	ScriptBinaryOperator LiquidEndsWith;
	ScriptBinaryOperator LiquidHasKey;
	ScriptBinaryOperator LiquidHasValue;
	ScriptBinaryOperator Add;
	ScriptBinaryOperator Subtract;
	ScriptBinaryOperator Substract;
	ScriptBinaryOperator Multiply;
	ScriptBinaryOperator Divide;
	ScriptBinaryOperator DivideRound;
	ScriptBinaryOperator Modulus;
	ScriptBinaryOperator ShiftLeft;
	ScriptBinaryOperator ShiftRight;
	ScriptBinaryOperator Power;
	ScriptBinaryOperator RangeInclude;
	ScriptBinaryOperator RangeExclude;
	ScriptBinaryOperator Custom;
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



public class ScriptBinaryOperatorExtensions
	ScriptToken ToToken(ScriptBinaryOperator op);
	TokenType ToTokenType(ScriptBinaryOperator op);
	String ToText(ScriptBinaryOperator op);



public class ScriptExpression:ScriptNode
	SourceSpan Span;
	ScriptNode Parent;
	Int32 ChildrenCount;
	IEnumerable<ScriptNode> Children;
	Object Evaluate(TemplateContext context);
	ScriptNode Clone();
	ScriptNode Clone(Boolean withTrivias);
	ScriptNode GetChildren(Int32 index);
	ScriptNode GetChildrenImpl(Int32 index);
	Boolean CanHaveLeadingTrivia();
	void PrintTo(ScriptPrinter printer);
	void Accept(ScriptVisitor visitor);
	TResult Accept(ScriptVisitor<TResult> visitor);
	void ParentToThis(TSyntaxNode& set, TSyntaxNode node);
	String ToString();



public class ScriptLiteralStringQuoteType:Enum, IComparable, IFormattable, IConvertible
	Int32 value__;
	ScriptLiteralStringQuoteType DoubleQuote;
	ScriptLiteralStringQuoteType SimpleQuote;
	ScriptLiteralStringQuoteType Verbatim;
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



public class ScriptUnaryOperator:Enum, IComparable, IFormattable, IConvertible
	Int32 value__;
	ScriptUnaryOperator None;
	ScriptUnaryOperator Not;
	ScriptUnaryOperator Negate;
	ScriptUnaryOperator Plus;
	ScriptUnaryOperator FunctionAlias;
	ScriptUnaryOperator FunctionParametersExpand;
	ScriptUnaryOperator Increment;
	ScriptUnaryOperator Decrement;
	ScriptUnaryOperator Custom;
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



public class ScriptUnaryOperatorExtensions
	String ToText(ScriptUnaryOperator op);



public class ScriptVariable:ScriptExpression, IScriptVariablePath, IEquatable<ScriptVariable>, IScriptTerminal
	ScriptVariableLocal Arguments;
	ScriptVariableLocal BlockDelegate;
	ScriptVariableLocal Continue;
	ScriptVariableGlobal ForObject;
	ScriptVariableGlobal TablerowObject;
	ScriptVariableGlobal WhileObject;
	SourceSpan Span;
	ScriptTrivias Trivias;
	String BaseName;
	String Name;
	ScriptVariableScope Scope;
	ScriptNode Parent;
	Int32 ChildrenCount;
	IEnumerable<ScriptNode> Children;
	ScriptVariable Create(String name, ScriptVariableScope scope);
	String GetFirstPath();
	Boolean Equals(ScriptVariable other);
	Boolean Equals(Object obj);
	Int32 GetHashCode();
	Object Evaluate(TemplateContext context);
	Object GetValue(TemplateContext context);
	void SetValue(TemplateContext context, Object valueToSet);
	void PrintTo(ScriptPrinter printer);
	ScriptNode Clone();
	ScriptNode Clone(Boolean withTrivias);
	ScriptNode GetChildren(Int32 index);
	ScriptNode GetChildrenImpl(Int32 index);
	Boolean CanHaveLeadingTrivia();
	void Accept(ScriptVisitor visitor);
	TResult Accept(ScriptVisitor<TResult> visitor);
	void ParentToThis(TSyntaxNode& set, TSyntaxNode node);
	String ToString();



public class ScriptVariableScope:Enum, IComparable, IFormattable, IConvertible
	Int32 value__;
	ScriptVariableScope Global;
	ScriptVariableScope Local;
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



public class IScriptConvertibleFrom
	Boolean TryConvertFrom(TemplateContext context, SourceSpan span, Object value);



public class IScriptConvertibleTo
	Boolean TryConvertTo(TemplateContext context, SourceSpan span, Type type, Object& value);



public class IScriptCustomBinaryOperation
	Boolean TryEvaluate(TemplateContext context, SourceSpan span, ScriptBinaryOperator op, SourceSpan leftSpan, Object leftValue, SourceSpan rightSpan, Object rightValue, Object& result);



public class IScriptCustomImplicitMultiplyPrecedence



public class IScriptCustomType:IScriptCustomTypeInfo, IScriptCustomBinaryOperation, IScriptCustomUnaryOperation, IScriptConvertibleTo



public class IScriptCustomTypeInfo
	String TypeName;



public class IScriptCustomUnaryOperation
	Boolean TryEvaluate(TemplateContext context, SourceSpan span, ScriptUnaryOperator op, Object rightValue, Object& result);



public class IScriptNamedArgumentContainer
	ScriptList<ScriptNamedArgument> NamedArguments;



public class IScriptVariablePath
	Object GetValue(TemplateContext context);
	void SetValue(TemplateContext context, Object valueToSet);
	String GetFirstPath();



public class IScriptVisitorContext
	ScriptNode Current;
	ScriptNode Parent;
	IEnumerable<ScriptNode> Ancestors;



public class ScriptArgumentException:Exception, ISerializable, _Exception
	Int32 ArgumentIndex;
	String Message;
	IDictionary Data;
	Exception InnerException;
	MethodBase TargetSite;
	String StackTrace;
	String HelpLink;
	String Source;
	Int32 HResult;
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
	void GetObjectData(SerializationInfo info, StreamingContext context);
	Exception PrepForRemoting();
	void SaveStackTracesFromDeepCopy(Exception exception, Object currentStackTrace, Object dynamicMethodArray);
	void InternalPreserveStackTrace();



public class ScriptFormatter:ScriptRewriter
	ScriptFormatterOptions Options;
	Boolean CopyTrivias;
	ScriptNode Format(ScriptNode node);
	ScriptNode Visit(ScriptNode node);
	ScriptNode Visit(ScriptAssignExpression node);
	ScriptNode Visit(ScriptPipeCall node);
	ScriptNode Visit(ScriptBinaryExpression node);
	ScriptNode Visit(ScriptExpressionStatement node);
	ScriptNode Visit(ScriptFunctionCall node);
	ScriptNode Visit(ScriptFunction node);
	ScriptExpression DeNestExpression(ScriptExpression expr);
	Boolean HasSimilarPrecedenceThanMultiply(ScriptBinaryOperator op);
	ScriptNode Visit(ScriptAnonymousFunction node);
	ScriptNode Visit(ScriptArgumentBinary node);
	ScriptNode Visit(ScriptArrayInitializerExpression node);
	ScriptNode Visit(ScriptBlockStatement node);
	ScriptNode Visit(ScriptBreakStatement node);
	ScriptNode Visit(ScriptCaptureStatement node);
	ScriptNode Visit(ScriptCaseStatement node);
	ScriptNode Visit(ScriptConditionalExpression node);
	ScriptNode Visit(ScriptContinueStatement node);
	ScriptNode Visit(ScriptElseStatement node);
	ScriptNode Visit(ScriptEndStatement node);
	ScriptNode Visit(ScriptEscapeStatement node);
	ScriptNode Visit(ScriptForStatement node);
	ScriptNode Visit(ScriptFrontMatter node);
	ScriptNode Visit(ScriptIdentifier node);
	ScriptNode Visit(ScriptIfStatement node);
	ScriptNode Visit(ScriptImportStatement node);
	ScriptNode Visit(ScriptIncrementDecrementExpression node);
	ScriptNode Visit(ScriptIndexerExpression node);
	ScriptNode Visit(ScriptInterpolatedExpression node);
	ScriptNode Visit(ScriptInterpolatedStringExpression node);
	ScriptNode Visit(ScriptIsEmptyExpression node);
	ScriptNode Visit(ScriptKeyword node);
	ScriptNode Visit(ScriptLiteral node);
	ScriptNode Visit(ScriptMemberExpression node);
	ScriptNode Visit(ScriptNamedArgument node);
	ScriptNode Visit(ScriptNestedExpression node);
	ScriptNode Visit(ScriptNopStatement node);
	ScriptNode Visit(ScriptObjectInitializerExpression node);
	ScriptNode Visit(ScriptObjectMember node);
	ScriptNode Visit(ScriptPage node);
	ScriptNode Visit(ScriptParameter node);
	ScriptNode Visit(ScriptRawStatement node);
	ScriptNode Visit(ScriptReadOnlyStatement node);
	ScriptNode Visit(ScriptReturnStatement node);
	ScriptNode Visit(ScriptTableRowStatement node);
	ScriptNode Visit(ScriptThisExpression node);
	ScriptNode Visit(ScriptToken node);
	ScriptNode Visit(ScriptUnaryExpression node);
	ScriptNode Visit(ScriptWhenStatement node);
	ScriptNode Visit(ScriptWhileStatement node);
	ScriptNode Visit(ScriptWithStatement node);
	ScriptNode Visit(ScriptWrapStatement node);
	ScriptNode Visit(ScriptVariableGlobal node);
	ScriptNode Visit(ScriptVariableLocal node);
	ScriptList<TNode> VisitAll(ScriptList<TNode> nodes);
	ScriptNode DefaultVisit(ScriptNode node);



public class ScriptFormatterExtensions
	ScriptNode Format(ScriptNode node, ScriptFormatterOptions options);
	Boolean HasFlags(ScriptFormatterFlags input, ScriptFormatterFlags flags);



public class ScriptFormatterFlags:Enum, IComparable, IFormattable, IConvertible
	Int32 value__;
	ScriptFormatterFlags None;
	ScriptFormatterFlags ExplicitParenthesis;
	ScriptFormatterFlags AddSpaceBetweenOperators;
	ScriptFormatterFlags RemoveExistingTrivias;
	ScriptFormatterFlags CompressSpaces;
	ScriptFormatterFlags MinimizeParenthesisNesting;
	ScriptFormatterFlags Clean;
	ScriptFormatterFlags ExplicitClean;
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



public class ScriptFormatterOptions:ValueType
	ScriptLang Language;
	ScriptFormatterFlags Flags;
	TemplateContext Context;
	Boolean Equals(Object obj);
	String ToString();
	Int32 GetHashCode();
	Int32 GetHashCodeOfPtr(IntPtr ptr);



public class ScriptList:ScriptNode
	SourceSpan Span;
	Int32 Count;
	Int32 ChildrenCount;
	ScriptNode Item;
	ScriptNode Parent;
	IEnumerable<ScriptNode> Children;
	ScriptNode GetChildrenImpl(Int32 index);
	Object Evaluate(TemplateContext context);
	ScriptNode Clone();
	ScriptNode Clone(Boolean withTrivias);
	ScriptNode GetChildren(Int32 index);
	Boolean CanHaveLeadingTrivia();
	void PrintTo(ScriptPrinter printer);
	void Accept(ScriptVisitor visitor);
	TResult Accept(ScriptVisitor<TResult> visitor);
	void ParentToThis(TSyntaxNode& set, TSyntaxNode node);
	String ToString();



public class ScriptList:ScriptList, IList<TScriptNode>, ICollection<TScriptNode>, IEnumerable<TScriptNode>, IEnumerable, IReadOnlyList<TScriptNode>, IReadOnlyCollection<TScriptNode>
	SourceSpan Span;
	Boolean IsReadOnly;
	TScriptNode Item;
	Int32 Count;
	Int32 ChildrenCount;
	ScriptNode Item;
	ScriptNode Parent;
	IEnumerable<ScriptNode> Children;
	void Add(TScriptNode node);
	void AddRange(IEnumerable<TScriptNode> nodes);
	void Clear();
	Boolean Contains(TScriptNode item);
	void CopyTo(TScriptNode[] array, Int32 arrayIndex);
	Boolean Remove(TScriptNode item);
	Object Evaluate(TemplateContext context);
	TScriptNode GetChildren(Int32 index);
	ScriptNode GetChildrenImpl(Int32 index);
	void PrintTo(ScriptPrinter printer);
	void Accept(ScriptVisitor visitor);
	TResult Accept(ScriptVisitor<TResult> visitor);
	Enumerator<TScriptNode> GetEnumerator();
	IEnumerator<TScriptNode> System.Collections.Generic.IEnumerable<TScriptNode>.GetEnumerator();
	IEnumerator System.Collections.IEnumerable.GetEnumerator();
	Int32 IndexOf(TScriptNode item);
	void Insert(Int32 index, TScriptNode item);
	void RemoveAt(Int32 index);
	void AssertNoParent(ScriptNode node);
	ScriptNode Clone();
	ScriptNode Clone(Boolean withTrivias);
	ScriptNode GetChildren(Int32 index);
	Boolean CanHaveLeadingTrivia();
	void ParentToThis(TSyntaxNode& set, TSyntaxNode node);
	String ToString();



public class ScriptNode
	SourceSpan Span;
	ScriptNode Parent;
	Int32 ChildrenCount;
	IEnumerable<ScriptNode> Children;
	Object Evaluate(TemplateContext context);
	ScriptNode Clone();
	ScriptNode Clone(Boolean withTrivias);
	ScriptNode GetChildren(Int32 index);
	ScriptNode GetChildrenImpl(Int32 index);
	Boolean CanHaveLeadingTrivia();
	void PrintTo(ScriptPrinter printer);
	void Accept(ScriptVisitor visitor);
	TResult Accept(ScriptVisitor<TResult> visitor);
	void ParentToThis(TSyntaxNode& set, TSyntaxNode node);
	String ToString();



public class ScriptNodeExtensions
	ScriptNode FindFirstTerminal(ScriptNode node);
	ScriptNode FindLastTerminal(ScriptNode node);
	T RemoveLeadingSpace(T node);
	T RemoveTrailingSpace(T node);
	void MoveLeadingTriviasTo(ScriptNode node, T destinationNode);
	void MoveTrailingTriviasTo(ScriptNode node, T destinationNode, Boolean before);
	void AddLeadingSpace(IScriptTerminal node);
	void AddCommaAfter(IScriptTerminal node);
	void AddSemiColonAfter(IScriptTerminal node);
	void AddSpaceAfter(IScriptTerminal node);
	void AddTrivia(IScriptTerminal node, ScriptTrivia trivia, Boolean before);
	void InsertTrivia(IScriptTerminal node, ScriptTrivia trivia, Boolean before);
	void AddTrivias(IScriptTerminal node, T trivias, Boolean before);
	Boolean HasLeadingSpaceTrivias(IScriptTerminal node);
	Boolean HasTrailingSpaceTrivias(IScriptTerminal node);
	Boolean HasTrivia(IScriptTerminal node, ScriptTriviaType triviaType, Boolean before);
	Boolean HasTriviaEndOfStatement(IScriptTerminal node, Boolean before);



public class ScriptParameterContainerExtensions
	void AddParameter(IScriptNamedArgumentContainer container, ScriptNamedArgument argument);
	void Write(ScriptPrinter printer, List<ScriptNamedArgument> parameters);



public class ScriptRuntimeException:Exception, ISerializable, _Exception
	SourceSpan Span;
	String Message;
	Boolean EnableDisplayInnerException;
	String OriginalMessage;
	IDictionary Data;
	Exception InnerException;
	MethodBase TargetSite;
	String StackTrace;
	String HelpLink;
	String Source;
	Int32 HResult;
	String ToString();
	Exception GetBaseException();
	void SetErrorCode(Int32 hr);
	Object DeepCopyStackTrace(Object currentStackTrace);
	Object DeepCopyDynamicMethods(Object currentDynamicMethods);
	void GetStackTracesDeepCopy(Object& currentStackTrace, Object& dynamicMethodArray);
	void RestoreExceptionDispatchInfo(ExceptionDispatchInfo exceptionDispatchInfo);
	String InternalToString();
	Type GetType();
	String GetMessageFromNativeResources(ExceptionMessageKind kind);
	void AddExceptionDataForRestrictedErrorInfo(String restrictedError, String restrictedErrorReference, String restrictedCapabilitySid, Object restrictedErrorObject, Boolean hasrestrictedLanguageErrorObject);
	Boolean TryGetRestrictedLanguageErrorObject(Object& restrictedErrorObject);
	void GetObjectData(SerializationInfo info, StreamingContext context);
	Exception PrepForRemoting();
	void SaveStackTracesFromDeepCopy(Exception exception, Object currentStackTrace, Object dynamicMethodArray);
	void InternalPreserveStackTrace();



public class ScriptAbortException:ScriptRuntimeException, ISerializable, _Exception
	CancellationToken CancellationToken;
	SourceSpan Span;
	String Message;
	String OriginalMessage;
	IDictionary Data;
	Exception InnerException;
	MethodBase TargetSite;
	String StackTrace;
	String HelpLink;
	String Source;
	Int32 HResult;
	String ToString();
	Exception GetBaseException();
	void SetErrorCode(Int32 hr);
	Object DeepCopyStackTrace(Object currentStackTrace);
	Object DeepCopyDynamicMethods(Object currentDynamicMethods);
	void GetStackTracesDeepCopy(Object& currentStackTrace, Object& dynamicMethodArray);
	void RestoreExceptionDispatchInfo(ExceptionDispatchInfo exceptionDispatchInfo);
	String InternalToString();
	Type GetType();
	String GetMessageFromNativeResources(ExceptionMessageKind kind);
	void AddExceptionDataForRestrictedErrorInfo(String restrictedError, String restrictedErrorReference, String restrictedCapabilitySid, Object restrictedErrorObject, Boolean hasrestrictedLanguageErrorObject);
	Boolean TryGetRestrictedLanguageErrorObject(Object& restrictedErrorObject);
	void GetObjectData(SerializationInfo info, StreamingContext context);
	Exception PrepForRemoting();
	void SaveStackTracesFromDeepCopy(Exception exception, Object currentStackTrace, Object dynamicMethodArray);
	void InternalPreserveStackTrace();



public class ScriptParserRuntimeException:ScriptRuntimeException, ISerializable, _Exception
	LogMessageBag ParserMessages;
	String Message;
	SourceSpan Span;
	String OriginalMessage;
	IDictionary Data;
	Exception InnerException;
	MethodBase TargetSite;
	String StackTrace;
	String HelpLink;
	String Source;
	Int32 HResult;
	String ToString();
	Exception GetBaseException();
	void SetErrorCode(Int32 hr);
	Object DeepCopyStackTrace(Object currentStackTrace);
	Object DeepCopyDynamicMethods(Object currentDynamicMethods);
	void GetStackTracesDeepCopy(Object& currentStackTrace, Object& dynamicMethodArray);
	void RestoreExceptionDispatchInfo(ExceptionDispatchInfo exceptionDispatchInfo);
	String InternalToString();
	Type GetType();
	String GetMessageFromNativeResources(ExceptionMessageKind kind);
	void AddExceptionDataForRestrictedErrorInfo(String restrictedError, String restrictedErrorReference, String restrictedCapabilitySid, Object restrictedErrorObject, Boolean hasrestrictedLanguageErrorObject);
	Boolean TryGetRestrictedLanguageErrorObject(Object& restrictedErrorObject);
	void GetObjectData(SerializationInfo info, StreamingContext context);
	Exception PrepForRemoting();
	void SaveStackTracesFromDeepCopy(Exception exception, Object currentStackTrace, Object dynamicMethodArray);
	void InternalPreserveStackTrace();



public class ScriptStringSlice:ValueType, IEquatable<ScriptStringSlice>, IComparable<ScriptStringSlice>, IComparable<String>
	String FullText;
	Int32 Index;
	Int32 Length;
	ScriptStringSlice Empty;
	Char Item;
	String Substring(Int32 index);
	String ToString();
	Boolean Equals(ScriptStringSlice other);
	Boolean Equals(Object obj);
	Int32 GetHashCode();
	Int32 CompareTo(ScriptStringSlice other);
	Int32 CompareTo(String other);
	ScriptStringSlice TrimStart();
	ScriptStringSlice TrimEnd();
	ScriptStringSlice TrimEndKeepNewLine();
	Int32 GetHashCodeOfPtr(IntPtr ptr);



public class ScriptStringSliceExtensions
	ScriptStringSlice Slice(String text, Int32 index);
	ScriptStringSlice Slice(String text, Int32 index, Int32 length);



public class ScriptTypeNameAttribute:Attribute, _Attribute
	String TypeName;
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



public class ScriptSyntaxAttribute:ScriptTypeNameAttribute, _Attribute
	String Example;
	String TypeName;
	Object TypeId;
	ScriptSyntaxAttribute Get(Object obj);
	ScriptSyntaxAttribute Get(Type type);
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



public class ScriptTrivia:ValueType
	SourceSpan Span;
	ScriptTriviaType Type;
	ScriptStringSlice Text;
	ScriptTrivia Space;
	ScriptTrivia Comma;
	ScriptTrivia SemiColon;
	ScriptTrivia WithText(ScriptStringSlice text);
	void Write(ScriptPrinter printer);
	String ToString();
	Boolean Equals(Object obj);
	Int32 GetHashCode();
	Int32 GetHashCodeOfPtr(IntPtr ptr);



public class ScriptTrivias
	List<ScriptTrivia> Before;
	List<ScriptTrivia> After;



public class ScriptTriviaType:Enum, IComparable, IFormattable, IConvertible
	Int32 value__;
	ScriptTriviaType Empty;
	ScriptTriviaType Whitespace;
	ScriptTriviaType WhitespaceFull;
	ScriptTriviaType Comment;
	ScriptTriviaType Comma;
	ScriptTriviaType CommentMulti;
	ScriptTriviaType NewLine;
	ScriptTriviaType SemiColon;
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



public class ScriptTriviaTypeExtensions
	Boolean IsSpace(ScriptTriviaType triviaType);
	Boolean IsNewLine(ScriptTriviaType triviaType);
	Boolean IsSpaceOrNewLine(ScriptTriviaType triviaType);



public class ScriptVerbatim:ScriptNode, IScriptTerminal
	SourceSpan Span;
	ScriptTrivias Trivias;
	String Value;
	ScriptNode Parent;
	Int32 ChildrenCount;
	IEnumerable<ScriptNode> Children;
	Object Evaluate(TemplateContext context);
	void PrintTo(ScriptPrinter printer);
	ScriptNode Clone();
	ScriptNode Clone(Boolean withTrivias);
	ScriptNode GetChildren(Int32 index);
	ScriptNode GetChildrenImpl(Int32 index);
	Boolean CanHaveLeadingTrivia();
	void Accept(ScriptVisitor visitor);
	TResult Accept(ScriptVisitor<TResult> visitor);
	void ParentToThis(TSyntaxNode& set, TSyntaxNode node);
	String ToString();



public class ScriptConditionStatement:ScriptStatement
	SourceSpan Span;
	Boolean CanSkipEvaluation;
	Boolean CanOutput;
	ScriptNode Parent;
	Int32 ChildrenCount;
	IEnumerable<ScriptNode> Children;
	Object Evaluate(TemplateContext context);
	ScriptNode Clone();
	ScriptNode Clone(Boolean withTrivias);
	ScriptNode GetChildren(Int32 index);
	ScriptNode GetChildrenImpl(Int32 index);
	Boolean CanHaveLeadingTrivia();
	void PrintTo(ScriptPrinter printer);
	void Accept(ScriptVisitor visitor);
	TResult Accept(ScriptVisitor<TResult> visitor);
	void ParentToThis(TSyntaxNode& set, TSyntaxNode node);
	String ToString();



public class ScriptFlowState:Enum, IComparable, IFormattable, IConvertible
	Int32 value__;
	ScriptFlowState None;
	ScriptFlowState Break;
	ScriptFlowState Continue;
	ScriptFlowState Return;
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



public class ScriptLoopStatementBase:ScriptStatement
	SourceSpan Span;
	Boolean CanSkipEvaluation;
	Boolean CanOutput;
	ScriptNode Parent;
	Int32 ChildrenCount;
	IEnumerable<ScriptNode> Children;
	void BeforeLoop(TemplateContext context);
	Object LoopItem(TemplateContext context, LoopState state);
	LoopState CreateLoopState();
	Boolean ContinueLoop(TemplateContext context);
	void AfterLoop(TemplateContext context);
	Object Evaluate(TemplateContext context);
	Object EvaluateImpl(TemplateContext context);
	ScriptNode Clone();
	ScriptNode Clone(Boolean withTrivias);
	ScriptNode GetChildren(Int32 index);
	ScriptNode GetChildrenImpl(Int32 index);
	Boolean CanHaveLeadingTrivia();
	void PrintTo(ScriptPrinter printer);
	void Accept(ScriptVisitor visitor);
	TResult Accept(ScriptVisitor<TResult> visitor);
	void ParentToThis(TSyntaxNode& set, TSyntaxNode node);
	String ToString();



public class ScriptStatement:ScriptNode
	SourceSpan Span;
	Boolean CanSkipEvaluation;
	Boolean CanOutput;
	ScriptNode Parent;
	Int32 ChildrenCount;
	IEnumerable<ScriptNode> Children;
	Object Evaluate(TemplateContext context);
	ScriptNode Clone();
	ScriptNode Clone(Boolean withTrivias);
	ScriptNode GetChildren(Int32 index);
	ScriptNode GetChildrenImpl(Int32 index);
	Boolean CanHaveLeadingTrivia();
	void PrintTo(ScriptPrinter printer);
	void Accept(ScriptVisitor visitor);
	TResult Accept(ScriptVisitor<TResult> visitor);
	void ParentToThis(TSyntaxNode& set, TSyntaxNode node);
	String ToString();



public class ScriptWhitespaceMode:Enum, IComparable, IFormattable, IConvertible
	Int32 value__;
	ScriptWhitespaceMode None;
	ScriptWhitespaceMode Greedy;
	ScriptWhitespaceMode NonGreedy;
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



public class DynamicCustomFunction:IScriptCustomFunction, IScriptFunctionInfo
	MethodInfo Method;
	Int32 RequiredParameterCount;
	Int32 ParameterCount;
	ScriptVarParamKind VarParamKind;
	Type ReturnType;
	Object Tag;
	ArgumentValue GetValueFromNamedArgument(TemplateContext context, ScriptNode callerContext, ScriptNamedArgument namedArg);
	Object Invoke(TemplateContext context, ScriptNode callerContext, ScriptArray arguments, ScriptBlockStatement blockStatement);
	ScriptParameterInfo GetParameterInfo(Int32 index);
	DynamicCustomFunction Create(Object target, MethodInfo method);
	DynamicCustomFunction Create(Delegate del);



public class DelegateCustomFunction:DynamicCustomFunction, IScriptCustomFunction, IScriptFunctionInfo
	MethodInfo Method;
	Object Target;
	Int32 RequiredParameterCount;
	Int32 ParameterCount;
	ScriptVarParamKind VarParamKind;
	Type ReturnType;
	Object Tag;
	Object Invoke(TemplateContext context, ScriptNode callerContext, ScriptArray scriptArguments, ScriptBlockStatement blockStatement);
	DelegateCustomFunction Create(Action action);
	DelegateCustomFunction Create(Action<T> action);
	DelegateCustomFunction Create(Action<T1,T2> action);
	DelegateCustomFunction Create(Action<T1,T2,T3> action);
	DelegateCustomFunction Create(Action<T1,T2,T3,T4> action);
	DelegateCustomFunction Create(Action<T1,T2,T3,T4,T5> action);
	DelegateCustomFunction CreateFunc(Func<TResult> func);
	DelegateCustomFunction CreateFunc(Func<T1,TResult> func);
	DelegateCustomFunction CreateFunc(Func<T1,T2,TResult> func);
	DelegateCustomFunction CreateFunc(Func<T1,T2,T3,TResult> func);
	DelegateCustomFunction CreateFunc(Func<T1,T2,T3,T4,TResult> func);
	DelegateCustomFunction CreateFunc(Func<T1,T2,T3,T4,T5,TResult> func);
	Object InvokeImpl(TemplateContext context, SourceSpan span, Object[] arguments);
	Object[] PrepareArguments(TemplateContext context, ScriptNode callerContext, ScriptArray scriptArguments, Array& paramsArguments);
	ArgumentValue GetValueFromNamedArgument(TemplateContext context, ScriptNode callerContext, ScriptNamedArgument namedArg);
	ScriptParameterInfo GetParameterInfo(Int32 index);
	DynamicCustomFunction Create(Object target, MethodInfo method);
	DynamicCustomFunction Create(Delegate del);



public class DelegateCustomAction:DelegateCustomFunction, IScriptCustomFunction, IScriptFunctionInfo
	MethodInfo Method;
	Action Func;
	Object Target;
	Int32 RequiredParameterCount;
	Int32 ParameterCount;
	ScriptVarParamKind VarParamKind;
	Type ReturnType;
	Object Tag;
	Object InvokeImpl(TemplateContext context, SourceSpan span, Object[] arguments);
	Object Invoke(TemplateContext context, ScriptNode callerContext, ScriptArray scriptArguments, ScriptBlockStatement blockStatement);
	DelegateCustomFunction Create(Action action);
	DelegateCustomFunction Create(Action<T> action);
	DelegateCustomFunction Create(Action<T1,T2> action);
	DelegateCustomFunction Create(Action<T1,T2,T3> action);
	DelegateCustomFunction Create(Action<T1,T2,T3,T4> action);
	DelegateCustomFunction Create(Action<T1,T2,T3,T4,T5> action);
	DelegateCustomFunction CreateFunc(Func<TResult> func);
	DelegateCustomFunction CreateFunc(Func<T1,TResult> func);
	DelegateCustomFunction CreateFunc(Func<T1,T2,TResult> func);
	DelegateCustomFunction CreateFunc(Func<T1,T2,T3,TResult> func);
	DelegateCustomFunction CreateFunc(Func<T1,T2,T3,T4,TResult> func);
	DelegateCustomFunction CreateFunc(Func<T1,T2,T3,T4,T5,TResult> func);
	ArgumentValue GetValueFromNamedArgument(TemplateContext context, ScriptNode callerContext, ScriptNamedArgument namedArg);
	ScriptParameterInfo GetParameterInfo(Int32 index);
	DynamicCustomFunction Create(Object target, MethodInfo method);
	DynamicCustomFunction Create(Delegate del);



public class EmptyScriptObject:IScriptObject
	EmptyScriptObject Default;
	Int32 Count;
	Boolean IsReadOnly;
	IEnumerable<String> GetMembers();
	Boolean Contains(String member);
	Boolean TryGetValue(TemplateContext context, SourceSpan span, String member, Object& value);
	Boolean CanWrite(String member);
	Boolean TrySetValue(TemplateContext context, SourceSpan span, String member, Object value, Boolean readOnly);
	Boolean Remove(String member);
	void SetReadOnly(String member, Boolean readOnly);
	IScriptObject Clone(Boolean deep);
	String ToString();



public class IListAccessor
	Int32 GetLength(TemplateContext context, SourceSpan span, Object target);
	Object GetValue(TemplateContext context, SourceSpan span, Object target, Int32 index);
	void SetValue(TemplateContext context, SourceSpan span, Object target, Int32 index, Object value);



public class IObjectAccessor
	Boolean HasIndexer;
	Type IndexType;
	Int32 GetMemberCount(TemplateContext context, SourceSpan span, Object target);
	IEnumerable<String> GetMembers(TemplateContext context, SourceSpan span, Object target);
	Boolean HasMember(TemplateContext context, SourceSpan span, Object target, String member);
	Boolean TryGetValue(TemplateContext context, SourceSpan span, Object target, String member, Object& value);
	Boolean TrySetValue(TemplateContext context, SourceSpan span, Object target, String member, Object value);
	Boolean TryGetItem(TemplateContext context, SourceSpan span, Object target, Object index, Object& value);
	Boolean TrySetItem(TemplateContext context, SourceSpan span, Object target, Object index, Object value);



public class IScriptCustomFunction:IScriptFunctionInfo
	Object Invoke(TemplateContext context, ScriptNode callerContext, ScriptArray arguments, ScriptBlockStatement blockStatement);



public class ScriptVarParamKind:Enum, IComparable, IFormattable, IConvertible
	Int32 value__;
	ScriptVarParamKind None;
	ScriptVarParamKind Direct;
	ScriptVarParamKind LastParameter;
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



public class IScriptFunctionInfo
	Int32 RequiredParameterCount;
	Int32 ParameterCount;
	ScriptVarParamKind VarParamKind;
	Type ReturnType;
	ScriptParameterInfo GetParameterInfo(Int32 index);



public class ScriptFunctionInfoExtensions
	Boolean IsParameterType(IScriptFunctionInfo functionInfo, Int32 index);



public class ScriptParameterInfo:ValueType, IEquatable<ScriptParameterInfo>
	Type ParameterType;
	String Name;
	Boolean HasDefaultValue;
	Object DefaultValue;
	Boolean Equals(ScriptParameterInfo other);
	Boolean Equals(Object obj);
	Int32 GetHashCode();
	String ToString();
	Int32 GetHashCodeOfPtr(IntPtr ptr);



public class IScriptObject
	Int32 Count;
	Boolean IsReadOnly;
	IEnumerable<String> GetMembers();
	Boolean Contains(String member);
	Boolean TryGetValue(TemplateContext context, SourceSpan span, String member, Object& value);
	Boolean CanWrite(String member);
	Boolean TrySetValue(TemplateContext context, SourceSpan span, String member, Object value, Boolean readOnly);
	Boolean Remove(String member);
	void SetReadOnly(String member, Boolean readOnly);
	IScriptObject Clone(Boolean deep);



public class IScriptOutput
	void Write(String text, Int32 offset, Int32 count);



public class ScriptOutputExtensions
	void Write(IScriptOutput scriptOutput, String text);
	void Write(IScriptOutput scriptOutput, ScriptStringSlice text);



public class IScriptTransformable
	Type ElementType;
	Boolean CanTransform(Type transformType);
	Boolean Visit(TemplateContext context, SourceSpan span, Func<Object,Boolean> visit);
	Object Transform(TemplateContext context, SourceSpan span, Func<Object,Object> apply, Type destType);



public class ITemplateLoader
	String GetPath(TemplateContext context, SourceSpan callerSpan, String templateName);
	String Load(TemplateContext context, SourceSpan callerSpan, String templatePath);



public class MemberFilterDelegate:MulticastDelegate, ICloneable, ISerializable
	MethodInfo Method;
	Object Target;
	Boolean Invoke(MemberInfo member);
	IAsyncResult BeginInvoke(MemberInfo member, AsyncCallback callback, Object object);
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



public class MemberRenamerDelegate:MulticastDelegate, ICloneable, ISerializable
	MethodInfo Method;
	Object Target;
	String Invoke(MemberInfo member);
	IAsyncResult BeginInvoke(MemberInfo member, AsyncCallback callback, Object object);
	String EndInvoke(IAsyncResult result);
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



public class ScriptArray:IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable, IList, ICollection, IScriptObject, IScriptCustomBinaryOperation, IScriptTransformable
	Int32 Capacity;
	Boolean IsReadOnly;
	ScriptObject ScriptObject;
	Int32 Count;
	T Item;
	Type ElementType;
	IScriptObject Clone(Boolean deep);
	T[] ToArray();
	void Add(T item);
	void AddRange(IEnumerable<T> items);
	Int32 System.Collections.IList.Add(Object value);
	Boolean System.Collections.IList.Contains(Object value);
	void Clear();
	Int32 System.Collections.IList.IndexOf(Object value);
	void System.Collections.IList.Insert(Int32 index, Object value);
	Boolean Contains(T item);
	void CopyTo(T[] array, Int32 arrayIndex);
	void CopyTo(Int32 index, T[] array, Int32 arrayIndex, Int32 count);
	Int32 IndexOf(T item);
	void Insert(Int32 index, T item);
	void System.Collections.IList.Remove(Object value);
	void RemoveAt(Int32 index);
	Boolean Remove(T item);
	Enumerator<T> GetEnumerator();
	IEnumerator<T> System.Collections.Generic.IEnumerable<T>.GetEnumerator();
	IEnumerator System.Collections.IEnumerable.GetEnumerator();
	void System.Collections.ICollection.CopyTo(Array array, Int32 index);
	IEnumerable<String> GetMembers();
	Boolean Contains(String member);
	Boolean TryGetValue(TemplateContext context, SourceSpan span, String member, Object& value);
	Boolean CanWrite(String member);
	Boolean TrySetValue(TemplateContext context, SourceSpan span, String member, Object value, Boolean readOnly);
	Boolean Remove(String member);
	void SetReadOnly(String member, Boolean readOnly);
	Boolean TryEvaluate(TemplateContext context, SourceSpan span, ScriptBinaryOperator op, SourceSpan leftSpan, Object leftValue, SourceSpan rightSpan, Object rightValue, Object& result);
	ScriptArray TryGetArray(Object rightValue);
	Boolean CompareTo(TemplateContext context, SourceSpan span, ScriptBinaryOperator op, ScriptArray left, ScriptArray right);
	Boolean CanTransform(Type transformType);
	Boolean Visit(TemplateContext context, SourceSpan span, Func<Object,Boolean> visit);
	Object Transform(TemplateContext context, SourceSpan span, Func<Object,Object> apply, Type destType);



public class ScriptArray:ScriptArray<Object>, IList<Object>, ICollection<Object>, IEnumerable<Object>, IEnumerable, IList, ICollection, IScriptObject, IScriptCustomBinaryOperation, IScriptTransformable
	Int32 Capacity;
	Boolean IsReadOnly;
	ScriptObject ScriptObject;
	Int32 Count;
	Object Item;
	Type ElementType;
	IScriptObject Clone(Boolean deep);
	Object[] ToArray();
	void Add(Object item);
	void AddRange(IEnumerable<Object> items);
	Int32 System.Collections.IList.Add(Object value);
	Boolean System.Collections.IList.Contains(Object value);
	void Clear();
	Int32 System.Collections.IList.IndexOf(Object value);
	void System.Collections.IList.Insert(Int32 index, Object value);
	Boolean Contains(Object item);
	void CopyTo(Object[] array, Int32 arrayIndex);
	void CopyTo(Int32 index, Object[] array, Int32 arrayIndex, Int32 count);
	Int32 IndexOf(Object item);
	void Insert(Int32 index, Object item);
	void System.Collections.IList.Remove(Object value);
	void RemoveAt(Int32 index);
	Boolean Remove(Object item);
	Enumerator<Object> GetEnumerator();
	IEnumerator<Object> System.Collections.Generic.IEnumerable<T>.GetEnumerator();
	IEnumerator System.Collections.IEnumerable.GetEnumerator();
	void System.Collections.ICollection.CopyTo(Array array, Int32 index);
	IEnumerable<String> GetMembers();
	Boolean Contains(String member);
	Boolean TryGetValue(TemplateContext context, SourceSpan span, String member, Object& value);
	Boolean CanWrite(String member);
	Boolean TrySetValue(TemplateContext context, SourceSpan span, String member, Object value, Boolean readOnly);
	Boolean Remove(String member);
	void SetReadOnly(String member, Boolean readOnly);
	Boolean TryEvaluate(TemplateContext context, SourceSpan span, ScriptBinaryOperator op, SourceSpan leftSpan, Object leftValue, SourceSpan rightSpan, Object rightValue, Object& result);
	Boolean CanTransform(Type transformType);
	Boolean Visit(TemplateContext context, SourceSpan span, Func<Object,Boolean> visit);
	Object Transform(TemplateContext context, SourceSpan span, Func<Object,Object> apply, Type destType);



public class ScriptLazy:IScriptCustomFunction, IScriptFunctionInfo
	Int32 RequiredParameterCount;
	Int32 ParameterCount;
	ScriptVarParamKind VarParamKind;
	Type ReturnType;
	ScriptParameterInfo GetParameterInfo(Int32 index);
	Object Invoke(TemplateContext context, ScriptNode callerContext, ScriptArray arguments, ScriptBlockStatement blockStatement);



public class ScriptMemberIgnoreAttribute:Attribute, _Attribute
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



public class ScriptMemberImportFlags:Enum, IComparable, IFormattable, IConvertible
	Int32 value__;
	ScriptMemberImportFlags Field;
	ScriptMemberImportFlags Property;
	ScriptMemberImportFlags Method;
	ScriptMemberImportFlags MethodInstance;
	ScriptMemberImportFlags All;
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



public class ScriptObject:IDictionary<String,Object>, ICollection<KeyValuePair`2>, IEnumerable<KeyValuePair`2>, IEnumerable, IScriptObject, IDictionary, ICollection, IFormattable
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
	Boolean IsSimpleKey(String key);
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



public class ScriptObjectExtensions
	void AssertNotReadOnly(IScriptObject scriptObject);
	void Import(IScriptObject script, Object obj, MemberFilterDelegate filter, MemberRenamerDelegate renamer);
	Boolean TryGetValue(IScriptObject this, String key, Object& value);
	void SetValue(IScriptObject this, String member, Object value, Boolean readOnly);
	void Import(IScriptObject this, IScriptObject other);
	void ImportDictionary(IScriptObject this, IDictionary dictionary);
	ScriptObject GetScriptObject(IScriptObject this);
	void ImportMember(IScriptObject script, Object obj, String memberName, String exportName);
	void Import(IScriptObject script, Object obj, ScriptMemberImportFlags flags, MemberFilterDelegate filter, MemberRenamerDelegate renamer);
	void Import(IScriptObject script, String member, Delegate function);
	Object ConvertValue(Object value);



public class ScriptRange:IList<Object>, ICollection<Object>, IEnumerable<Object>, IEnumerable, IList, ICollection, IScriptTransformable, IScriptCustomBinaryOperation
	IEnumerable Values;
	Type ElementType;
	Int32 Count;
	Boolean IsReadOnly;
	Object Item;
	IEnumerator<Object> GetEnumerator();
	IEnumerator System.Collections.IEnumerable.GetEnumerator();
	Boolean CanTransform(Type transformType);
	Boolean Visit(TemplateContext context, SourceSpan span, Func<Object,Boolean> visit);
	Object Transform(TemplateContext context, SourceSpan span, Func<Object,Object> apply, Type destType);
	IEnumerable TransformImpl(Func<Object,Object> apply);
	ScriptRange Offset(IEnumerable list, Int32 index);
	IEnumerable OffsetImpl(IEnumerable list, Int32 index);
	ScriptRange Limit(IEnumerable list, Int32 count);
	IEnumerable LimitImpl(IEnumerable list, Int32 count);
	ScriptRange Compact(IEnumerable list);
	ScriptRange Uniq(IEnumerable list);
	ScriptRange Reverse(IEnumerable list);
	IEnumerable CompactImpl(IEnumerable list);
	ScriptRange BinaryOr(IEnumerable<Object> left, IEnumerable<Object> right);
	ScriptRange BinaryAnd(IEnumerable<Object> left, IEnumerable<Object> right);
	ScriptRange ShiftLeft(IEnumerable left, Object value);
	IEnumerable ShiftLeftImpl(IEnumerable left, Object value);
	ScriptRange ShiftRight(Object value, IEnumerable right);
	IEnumerable ShiftRightImpl(Object value, IEnumerable right);
	ScriptRange Multiply(IEnumerable left, Int32 count);
	IEnumerable MultiplyImpl(IEnumerable left, Int32 count);
	ScriptRange Divide(IEnumerable left, Int32 count);
	ScriptRange Modulus(IEnumerable left, Int32 count);
	IEnumerable DivideImpl(IEnumerable left, Int32 count);
	IEnumerable ModulusImpl(IEnumerable left, Int32 modulus);
	ScriptRange Concat(IEnumerable left, IEnumerable right);
	IEnumerable ConcatImpl(IEnumerable left, IEnumerable right);
	Boolean TryEvaluate(TemplateContext context, SourceSpan span, ScriptBinaryOperator op, SourceSpan leftSpan, Object leftValue, SourceSpan rightSpan, Object rightValue, Object& result);
	IEnumerable<Object> TryGetRange(Object rightValue);
	Boolean CompareTo(TemplateContext context, SourceSpan span, ScriptBinaryOperator op, IEnumerable<Object> left, IEnumerable<Object> right);
	void Add(Object item);
	Int32 AddImpl(Object item);
	Int32 System.Collections.IList.Add(Object value);
	void Clear();
	Boolean Contains(Object item);
	void CopyTo(Object[] array, Int32 arrayIndex);
	Boolean Remove(Object item);
	void System.Collections.ICollection.CopyTo(Array array, Int32 index);
	Int32 IndexOf(Object item);
	void Insert(Int32 index, Object item);
	void System.Collections.IList.Remove(Object value);
	void RemoveAt(Int32 index);



public class StandardMemberRenamer
	MemberRenamerDelegate Default;
	String Rename(MemberInfo member);
	String Rename(String name);



public class StringBuilderOutput:IScriptOutput
	StringBuilder Builder;
	void Write(String text, Int32 offset, Int32 count);
	StringBuilderOutput GetThreadInstance();
	String ToString();



public class TextWriterOutput:IScriptOutput
	TextWriter Writer;
	void Write(String text, Int32 offset, Int32 count);
	String ToString();



public class ArrayAccessor:IListAccessor, IObjectAccessor
	ArrayAccessor Default;
	Boolean HasIndexer;
	Type IndexType;
	Int32 GetLength(TemplateContext context, SourceSpan span, Object target);
	Object GetValue(TemplateContext context, SourceSpan span, Object target, Int32 index);
	void SetValue(TemplateContext context, SourceSpan span, Object target, Int32 index, Object value);
	Int32 GetMemberCount(TemplateContext context, SourceSpan span, Object target);
	IEnumerable<String> GetMembers(TemplateContext context, SourceSpan span, Object target);
	Boolean HasMember(TemplateContext context, SourceSpan span, Object target, String member);
	Boolean TryGetValue(TemplateContext context, SourceSpan span, Object target, String member, Object& value);
	Boolean TrySetValue(TemplateContext context, SourceSpan span, Object target, String member, Object value);
	Boolean TryGetItem(TemplateContext context, SourceSpan span, Object target, Object index, Object& value);
	Boolean TrySetItem(TemplateContext context, SourceSpan span, Object target, Object index, Object value);



public class DictionaryAccessor:IObjectAccessor
	DictionaryAccessor Default;
	Boolean HasIndexer;
	Type IndexType;
	Boolean TryGet(Object target, IObjectAccessor& accessor);
	Int32 GetMemberCount(TemplateContext context, SourceSpan span, Object target);
	IEnumerable<String> GetMembers(TemplateContext context, SourceSpan span, Object target);
	Boolean HasMember(TemplateContext context, SourceSpan span, Object target, String member);
	Boolean TryGetValue(TemplateContext context, SourceSpan span, Object target, String member, Object& value);
	Boolean TrySetValue(TemplateContext context, SourceSpan span, Object target, String member, Object value);
	Boolean TryGetItem(TemplateContext context, SourceSpan span, Object target, Object index, Object& value);
	Boolean TrySetItem(TemplateContext context, SourceSpan span, Object target, Object index, Object value);



public class ListAccessor:IListAccessor, IObjectAccessor
	ListAccessor Default;
	Boolean HasIndexer;
	Type IndexType;
	Int32 GetLength(TemplateContext context, SourceSpan span, Object target);
	Object GetValue(TemplateContext context, SourceSpan span, Object target, Int32 index);
	void SetValue(TemplateContext context, SourceSpan span, Object target, Int32 index, Object value);
	Int32 GetMemberCount(TemplateContext context, SourceSpan span, Object target);
	IEnumerable<String> GetMembers(TemplateContext context, SourceSpan span, Object target);
	Boolean HasMember(TemplateContext context, SourceSpan span, Object target, String member);
	Boolean TryGetValue(TemplateContext context, SourceSpan span, Object target, String member, Object& value);
	Boolean TrySetValue(TemplateContext context, SourceSpan span, Object target, String member, Object value);
	Boolean TryGetItem(TemplateContext context, SourceSpan span, Object target, Object index, Object& value);
	Boolean TrySetItem(TemplateContext context, SourceSpan span, Object target, Object index, Object value);



public class NullAccessor:IObjectAccessor
	NullAccessor Default;
	Boolean HasIndexer;
	Type IndexType;
	Int32 GetMemberCount(TemplateContext context, SourceSpan span, Object target);
	IEnumerable<String> GetMembers(TemplateContext context, SourceSpan span, Object target);
	Boolean HasMember(TemplateContext context, SourceSpan span, Object target, String member);
	Boolean TryGetValue(TemplateContext context, SourceSpan span, Object target, String member, Object& value);
	Boolean TrySetValue(TemplateContext context, SourceSpan span, Object target, String member, Object value);
	Boolean TryGetItem(TemplateContext context, SourceSpan span, Object target, Object index, Object& value);
	Boolean TrySetItem(TemplateContext context, SourceSpan span, Object target, Object index, Object value);



public class ScriptObjectAccessor:IObjectAccessor
	IObjectAccessor Default;
	Boolean HasIndexer;
	Type IndexType;
	Int32 GetMemberCount(TemplateContext context, SourceSpan span, Object target);
	IEnumerable<String> GetMembers(TemplateContext context, SourceSpan span, Object target);
	Boolean HasMember(TemplateContext context, SourceSpan span, Object target, String member);
	Boolean TryGetValue(TemplateContext context, SourceSpan span, Object target, String member, Object& value);
	Boolean TrySetValue(TemplateContext context, SourceSpan span, Object target, String member, Object value);
	Boolean TryGetItem(TemplateContext context, SourceSpan span, Object target, Object index, Object& value);
	Boolean TrySetItem(TemplateContext context, SourceSpan span, Object target, Object index, Object value);



public class StringAccessor:IListAccessor, IObjectAccessor
	StringAccessor Default;
	Boolean HasIndexer;
	Type IndexType;
	Int32 GetLength(TemplateContext context, SourceSpan span, Object target);
	Object GetValue(TemplateContext context, SourceSpan span, Object target, Int32 index);
	void SetValue(TemplateContext context, SourceSpan span, Object target, Int32 index, Object value);
	Int32 GetMemberCount(TemplateContext context, SourceSpan span, Object target);
	IEnumerable<String> GetMembers(TemplateContext context, SourceSpan span, Object target);
	Boolean HasMember(TemplateContext context, SourceSpan span, Object target, String member);
	Boolean TryGetValue(TemplateContext context, SourceSpan span, Object target, String member, Object& value);
	Boolean TrySetValue(TemplateContext context, SourceSpan span, Object target, String member, Object value);
	Boolean TryGetItem(TemplateContext context, SourceSpan span, Object target, Object index, Object& value);
	Boolean TrySetItem(TemplateContext context, SourceSpan span, Object target, Object index, Object value);



public class TypedObjectAccessor:IObjectAccessor
	Type IndexType;
	Boolean HasIndexer;
	Int32 GetMemberCount(TemplateContext context, SourceSpan span, Object target);
	IEnumerable<String> GetMembers(TemplateContext context, SourceSpan span, Object target);
	Boolean HasMember(TemplateContext context, SourceSpan span, Object target, String member);
	Boolean TryGetValue(TemplateContext context, SourceSpan span, Object target, String member, Object& value);
	Boolean TryGetItem(TemplateContext context, SourceSpan span, Object target, Object index, Object& value);
	Boolean TrySetItem(TemplateContext context, SourceSpan span, Object target, Object index, Object value);
	Boolean TrySetValue(TemplateContext context, SourceSpan span, Object target, String member, Object value);
	void PrepareMembers();
	String Rename(MemberInfo member);



public class Lexer:IEnumerable<Token>, IEnumerable
	LexerOptions Options;
	String Text;
	String SourcePath;
	Boolean HasErrors;
	IEnumerable<LogMessage> Errors;
	Enumerator GetEnumerator();
	Boolean MoveNext();
	Boolean TryParseFrontMatterMarker();
	Boolean IsCodeEnterOrEscape(TokenType& whitespaceMode);
	void ReadCodeEnterOrEscape();
	Boolean TryReadLiquidCommentOrRaw(TextPosition codeEnterStart, TextPosition codeEnterEnd);
	void SkipSpaces();
	void PeekSkipSpaces(Int32& i);
	Boolean TryMatchPeek(String text, Int32 offset, Int32& offsetOut);
	Boolean TryMatch(String text);
	Boolean IsCodeExit();
	void ReadCodeExitOrEscape();
	Boolean ReadRaw();
	Boolean ReadCode();
	Boolean TryMatchCustomToken(TextPosition start);
	Boolean ReadCodeLiquid();
	Boolean ConsumeWhitespace(Boolean stopAtNewLine, TextPosition& lastSpace, Boolean keepNewLine);
	Boolean IsNewLine(Char c);
	void ReadIdentifier(Boolean special);
	Boolean IsFirstIdentifierLetter(Char c);
	Boolean IsIdentifierLetter(Char c);
	void ReadNumber();
	Boolean IsNumberPostFix(Char c);
	void ReadHexa(TextPosition start);
	void ReadBinary(TextPosition start);
	void ReadString();
	void ReadVerbatimString();
	void ReadInterpolatedString();
	void ReadComment();
	Char PeekChar(Int32 count);
	void NextChar();
	IEnumerator<Token> System.Collections.Generic.IEnumerable<Scriban.Parsing.Token>.GetEnumerator();
	IEnumerator System.Collections.IEnumerable.GetEnumerator();
	void AddError(String message, TextPosition start, TextPosition end);
	void Reset();
	Boolean IsWhitespace(Char c);



public class LexerOptions:ValueType
	LexerOptions Default;
	String DefaultFrontMatterMarker;
	ScriptMode Mode;
	ScriptLang Lang;
	String FrontMatterMarker;
	Boolean EnableIncludeImplicitString;
	TextPosition StartPosition;
	Boolean KeepTrivia;
	TryMatchCustomTokenDelegate TryMatchCustomToken;
	Boolean Equals(Object obj);
	String ToString();
	Int32 GetHashCode();
	Int32 GetHashCodeOfPtr(IntPtr ptr);



public class TryMatchCustomTokenDelegate:MulticastDelegate, ICloneable, ISerializable
	MethodInfo Method;
	Object Target;
	Boolean Invoke(String text, TextPosition position, Int32& length, TokenType& tokenType);
	IAsyncResult BeginInvoke(String text, TextPosition position, Int32& length, TokenType& tokenType, AsyncCallback callback, Object object);
	Boolean EndInvoke(Int32& length, TokenType& tokenType, IAsyncResult result);
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



public class LogMessage
	ParserMessageType Type;
	SourceSpan Span;
	String Message;
	String ToString();



public class ParserMessageType:Enum, IComparable, IFormattable, IConvertible
	Int32 value__;
	ParserMessageType Error;
	ParserMessageType Warning;
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



public class Parser
	ParserOptions Options;
	LogMessageBag Messages;
	Boolean HasErrors;
	SourceSpan CurrentSpan;
	Int32 ExpressionLevel;
	ScriptPage Run();
	void PushTokenToTrivia();
	ScriptStringSlice GetAsStringSlice(Token token);
	T Open(T element);
	T Open();
	void FlushTrivias(IScriptTerminal element, Boolean isBefore);
	T Close(T node);
	void FlushTriviasToLastTerminal();
	String GetAsText(Token localToken);
	String GetAsTextForLog(Token localToken);
	Boolean MatchText(Token localToken, String text);
	void NextToken();
	void PushTrivia(Token token);
	Token PeekToken();
	ScriptIdentifier ParseIdentifier();
	Boolean IsHidden(TokenType tokenType);
	void LogError(String text, Boolean isFatal);
	void LogError(Token tokenArg, String text, Boolean isFatal);
	SourceSpan GetSpanForToken(Token tokenArg);
	void LogError(SourceSpan span, String text, Boolean isFatal);
	void LogError(ScriptNode node, String message, Boolean isFatal);
	void LogError(ScriptNode node, SourceSpan span, String message, Boolean isFatal);
	void Log(LogMessage logMessage, Boolean isFatal);
	Boolean TryBinaryOperator(ScriptBinaryOperator& binaryOperator, Int32& precedence);
	ScriptExpression ParseExpressionAsVariableOrStringOrExpression(ScriptNode parentNode);
	ScriptExpression ParseExpression(ScriptNode parentNode, ScriptExpression parentExpression, Int32 precedence, ParseExpressionMode mode, Boolean allowAssignment);
	Boolean IsCurrentPipeOrExpressionContinuation();
	ScriptExpression ParseArrayInitializer();
	ScriptExpression ParseObjectInitializer();
	ScriptExpression ParseParenthesis();
	ScriptInterpolatedStringExpression ParseInterpolatedString();
	ScriptExpression ParseInterpolatedExpression();
	ScriptToken ParseToken(TokenType tokenType);
	void ExpectAndParseTokenTo(ScriptToken existingToken, TokenType expectedTokenType);
	ScriptKeyword ExpectAndParseKeywordTo(ScriptKeyword existingKeyword);
	ScriptExpression ParseIncrementDecrementExpression();
	ScriptExpression ParseUnaryExpression();
	ScriptExpression TransformKeyword(ScriptExpression leftOperand);
	void TransformLiquidFunctionCallToScriban(ScriptFunctionCall functionCall);
	void EnterExpression();
	ScriptExpression ExpectAndParseExpression(ScriptNode parentNode, ScriptExpression parentExpression, Int32 newPrecedence, String message, ParseExpressionMode mode, Boolean allowAssignment);
	ScriptExpression ExpectAndParseExpressionAndAnonymous(ScriptNode parentNode, ParseExpressionMode mode);
	Boolean IsStartOfExpression();
	Boolean TryGetCompoundAssignmentOperator(ScriptToken& scriptToken, TokenType& tokenType);
	Boolean IsStartingAsUnaryExpression();
	Boolean TryLiquidBinaryOperator(ScriptBinaryOperator& binaryOperator, Int32& precedence);
	Int32 GetDefaultBinaryOperatorPrecedence(ScriptBinaryOperator op);
	Int32 GetDefaultUnaryOperatorPrecedence(ScriptUnaryOperator op);
	Boolean IsPreviousCharWhitespace();
	Boolean IsNextCharWhitespace();
	void LeaveExpression();
	ScriptBlockStatement ParseBlockStatement(ScriptNode parentStatement, Boolean parseEndOfStatementAfterEnd);
	Boolean TryParseStatement(ScriptNode parent, Boolean parseEndOfStatementAfterEnd, ScriptStatement& statement, Boolean& hasEnd);
	ScriptCaptureStatement ParseCaptureStatement();
	ScriptEscapeStatement ParseEscapeStatement();
	ScriptCaseStatement ParseCaseStatement();
	ScriptConditionStatement ParseElseStatement(Boolean isElseIf);
	ScriptStatement ParseExpressionStatement();
	T ParseForStatement();
	ScriptIfStatement ParseIfStatement(Boolean invert, ScriptKeyword elseKeyword);
	ScriptRawStatement ParseRawStatement();
	ScriptWhenStatement ParseWhenStatement();
	void CheckNotInCase(ScriptNode parent, Token token);
	ScriptVariable ExpectAndParseVariable(ScriptNode parentNode);
	Boolean ExpectEndOfStatement();
	ScriptStatement FindFirstStatementExpectingEnd();
	Boolean ExpectStatementEnd(ScriptNode scriptNode);
	void ParseLiquidStatement(String identifier, ScriptNode parent, ScriptStatement& statement, Boolean& hasEnd, Boolean& nextStatement);
	ScriptExpressionStatement ParseLiquidCycleStatement();
	ScriptStatement ParseLiquidExpressionStatement(ScriptNode parent);
	ScriptStatement ParseLiquidIfChanged();
	ScriptStatement ParseLiquidIncDecStatement(Boolean isDec);
	ScriptStatement ParseLiquidIncludeStatement();
	void ParseScribanStatement(String identifier, ScriptNode parent, Boolean parseEndOfStatementAfterEnd, ScriptStatement& statement, Boolean& hasEnd, Boolean& nextStatement);
	ScriptEndStatement ParseEndStatement(Boolean parseEndOfStatementAfterEnd);
	ScriptFunction ParseFunctionStatement(Boolean isAnonymous);
	ScriptImportStatement ParseImportStatement();
	ScriptReadOnlyStatement ParseReadOnlyStatement();
	ScriptReturnStatement ParseReturnStatement();
	ScriptWhileStatement ParseWhileStatement();
	ScriptWithStatement ParseWithStatement();
	ScriptWrapStatement ParseWrapStatement();
	void FixRawStatementAfterFrontMatter(ScriptPage page);
	Boolean IsScribanKeyword(String text);
	ScriptExpression ParseVariableOrLiteral();
	ScriptLiteral ParseFloat();
	ScriptLiteral ParseImplicitString();
	ScriptLiteral ParseInteger();
	ScriptLiteral ParseHexaInteger();
	ScriptLiteral ParseBinaryInteger();
	ScriptLiteral ParseString();
	ScriptLiteral ParseInterpolatedStringPart();
	ScriptExpression ParseVariable();
	ScriptLiteral ParseVerbatimString();
	String ConvertFromUtf32(Int32 utf32);
	Boolean IsVariableOrLiteral(Token token);



public class ParserOptions:ValueType
	Nullable<Int32> ExpressionDepthLimit;
	Boolean LiquidFunctionsToScriban;
	Boolean ParseFloatAsDecimal;
	Boolean Equals(Object obj);
	String ToString();
	Int32 GetHashCode();
	Int32 GetHashCodeOfPtr(IntPtr ptr);



public class ScriptLang:Enum, IComparable, IFormattable, IConvertible
	Int32 value__;
	ScriptLang Default;
	ScriptLang Liquid;
	ScriptLang Scientific;
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



public class ScriptMode:Enum, IComparable, IFormattable, IConvertible
	Int32 value__;
	ScriptMode Default;
	ScriptMode FrontMatterOnly;
	ScriptMode FrontMatterAndContent;
	ScriptMode ScriptOnly;
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



public class SourceSpan:ValueType
	String FileName;
	Boolean IsEmpty;
	TextPosition Start;
	TextPosition End;
	Int32 Length;
	String ToString();
	String ToStringSimple();
	Boolean Equals(Object obj);
	Int32 GetHashCode();
	Int32 GetHashCodeOfPtr(IntPtr ptr);



public class TextPosition:ValueType, IEquatable<TextPosition>
	TextPosition Eof;
	Int32 Offset;
	Int32 Column;
	Int32 Line;
	TextPosition NextColumn(Int32 offset);
	TextPosition NextLine(Int32 offset);
	String ToString();
	String ToStringSimple();
	Boolean Equals(TextPosition other);
	Boolean Equals(Object obj);
	Int32 GetHashCode();
	Int32 GetHashCodeOfPtr(IntPtr ptr);



public class Token:ValueType, IEquatable<Token>
	TokenType Type;
	TextPosition Start;
	TextPosition End;
	Token Eof;
	String ToString();
	String GetText(String text);
	Boolean Match(String textToMatch, String lexerText);
	Boolean Equals(Token other);
	Boolean Equals(Object obj);
	Int32 GetHashCode();
	Int32 GetHashCodeOfPtr(IntPtr ptr);



public class TokenType:Enum, IComparable, IFormattable, IConvertible
	Int32 value__;
	TokenType Invalid;
	TokenType FrontMatterMarker;
	TokenType CodeEnter;
	TokenType LiquidTagEnter;
	TokenType CodeExit;
	TokenType LiquidTagExit;
	TokenType Raw;
	TokenType Escape;
	TokenType EscapeEnter;
	TokenType EscapeExit;
	TokenType NewLine;
	TokenType Whitespace;
	TokenType WhitespaceFull;
	TokenType Comment;
	TokenType CommentMulti;
	TokenType IdentifierSpecial;
	TokenType Identifier;
	TokenType Integer;
	TokenType HexaInteger;
	TokenType BinaryInteger;
	TokenType Float;
	TokenType String;
	TokenType InterpolatedString;
	TokenType BeginInterpolatedString;
	TokenType ContinuationInterpolatedString;
	TokenType EndingInterpolatedString;
	TokenType ImplicitString;
	TokenType VerbatimString;
	TokenType SemiColon;
	TokenType Arroba;
	TokenType Caret;
	TokenType DoubleCaret;
	TokenType Colon;
	TokenType Equal;
	TokenType VerticalBar;
	TokenType PipeGreater;
	TokenType Exclamation;
	TokenType DoubleAmp;
	TokenType DoubleVerticalBar;
	TokenType Amp;
	TokenType Question;
	TokenType DoubleQuestion;
	TokenType QuestionDot;
	TokenType QuestionExclamation;
	TokenType DoubleEqual;
	TokenType ExclamationEqual;
	TokenType Less;
	TokenType Greater;
	TokenType LessEqual;
	TokenType GreaterEqual;
	TokenType Divide;
	TokenType DivideEqual;
	TokenType DoubleDivide;
	TokenType DoubleDivideEqual;
	TokenType Asterisk;
	TokenType AsteriskEqual;
	TokenType Plus;
	TokenType PlusEqual;
	TokenType DoublePlus;
	TokenType Minus;
	TokenType MinusEqual;
	TokenType DoubleMinus;
	TokenType Percent;
	TokenType PercentEqual;
	TokenType DoubleLessThan;
	TokenType DoubleGreaterThan;
	TokenType Comma;
	TokenType Dot;
	TokenType DoubleDot;
	TokenType TripleDot;
	TokenType DoubleDotLess;
	TokenType OpenParen;
	TokenType CloseParen;
	TokenType OpenBrace;
	TokenType CloseBrace;
	TokenType OpenBracket;
	TokenType CloseBracket;
	TokenType OpenInterpolatedBrace;
	TokenType CloseInterpolatedBrace;
	TokenType Custom;
	TokenType Custom1;
	TokenType Custom2;
	TokenType Custom3;
	TokenType Custom4;
	TokenType Custom5;
	TokenType Custom6;
	TokenType Custom7;
	TokenType Custom8;
	TokenType Custom9;
	TokenType Eof;
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



public class TokenTypeExtensions
	Boolean HasText(TokenType type);
	String ToText(TokenType type);
	Boolean IsStringToken(TokenType token);
	Boolean IsInterpolationStringToken(TokenType token);



public class CharHelper
	Boolean IsHexa(Char c);
	Boolean TryParseDigit(Char c, Int32& value);
	Boolean TryHexaToInt(Char c, Int32& value);
	Boolean IsBinary(Char c);



public class ReflectionHelper
	Boolean IsPrimitiveOrDecimal(Type type);
	Boolean IsNumber(Type type);
	Type GetBaseOrInterface(Type type, Type lookInterfaceType);
	String ScriptPrettyName(Type type);



public class ArrayFunctions:ScriptObject, IDictionary<String,Object>, ICollection<KeyValuePair`2>, IEnumerable<KeyValuePair`2>, IEnumerable, IScriptObject, IDictionary, ICollection, IFormattable
	Int32 Count;
	Boolean IsReadOnly;
	Object Item;
	ICollection<String> Keys;
	ICollection<Object> Values;
	IEnumerable Add(IEnumerable list, Object value);
	IEnumerable AddRange(IEnumerable list1, IEnumerable list2);
	IEnumerable Compact(IEnumerable list);
	IEnumerable Concat(IEnumerable list1, IEnumerable list2);
	Object Cycle(TemplateContext context, SourceSpan span, IList list, Object group);
	Boolean Any(TemplateContext context, SourceSpan span, IEnumerable list, Object function, Object[] args);
	ScriptRange Each(TemplateContext context, SourceSpan span, IEnumerable list, Object function);
	IEnumerable EachInternal(TemplateContext context, ScriptNode callerContext, SourceSpan span, IEnumerable list, IScriptCustomFunction function, Type destType);
	ScriptRange Filter(TemplateContext context, SourceSpan span, IEnumerable list, Object function);
	IEnumerable FilterInternal(TemplateContext context, ScriptNode callerContext, SourceSpan span, IEnumerable list, IScriptCustomFunction function, Type destType);
	Object First(IEnumerable list);
	IEnumerable InsertAt(IEnumerable list, Int32 index, Object value);
	String Join(TemplateContext context, SourceSpan span, IEnumerable list, String delimiter, Object function);
	Object Last(IEnumerable list);
	IEnumerable Limit(IEnumerable list, Int32 count);
	IEnumerable Map(TemplateContext context, SourceSpan span, Object list, String member);
	IEnumerable MapImpl(TemplateContext context, SourceSpan span, Object list, String member);
	IEnumerable Offset(IEnumerable list, Int32 index);
	IList RemoveAt(IList list, Int32 index);
	IEnumerable Reverse(IEnumerable list);
	Int32 Size(IEnumerable list);
	IEnumerable Sort(TemplateContext context, SourceSpan span, Object list, String member);
	IEnumerable Uniq(IEnumerable list);
	Boolean Contains(IEnumerable list, Object item);
	Boolean CompareEnum(Enum e, Object item);
	ScriptRange ApplyFunction(TemplateContext context, SourceSpan span, IEnumerable list, Object function, ListProcessor impl);
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



public class BuiltinFunctions:ScriptObject, IDictionary<String,Object>, ICollection<KeyValuePair`2>, IEnumerable<KeyValuePair`2>, IEnumerable, IScriptObject, IDictionary, ICollection, IFormattable
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



public class DateTimeFunctions:ScriptObject, IDictionary<String,Object>, ICollection<KeyValuePair`2>, IEnumerable<KeyValuePair`2>, IEnumerable, IScriptObject, IDictionary, ICollection, IFormattable, IScriptCustomFunction, IScriptFunctionInfo
	ScriptVariable DateVariable;
	String DefaultFormat;
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
	DateTime Now();
	DateTime AddDays(DateTime date, Double days);
	DateTime AddMonths(DateTime date, Int32 months);
	DateTime AddYears(DateTime date, Int32 years);
	DateTime AddHours(DateTime date, Double hours);
	DateTime AddMinutes(DateTime date, Double minutes);
	DateTime AddSeconds(DateTime date, Double seconds);
	DateTime AddMilliseconds(DateTime date, Double millis);
	String ParseCustomFormat(CultureInfo culture, String pattern, CultureInfo& cultureOverride);
	Nullable<DateTime> ParseDateTime(TemplateContext context, String text, String pattern, String culture);
	Nullable<DateTime> Parse(TemplateContext context, String text, String pattern, String culture);
	String ParseToString(TemplateContext context, String text, String output_pattern, String output_culture, String input_pattern, String input_culture);
	IScriptObject Clone(Boolean deep);
	String ToString(Nullable<DateTime> datetime, String pattern, CultureInfo culture);
	Object Invoke(TemplateContext context, ScriptNode callerContext, ScriptArray arguments, ScriptBlockStatement blockStatement);
	ScriptParameterInfo GetParameterInfo(Int32 index);
	void CreateImportFunctions();
	String ToStringTrampoline(TemplateContext context, Nullable<DateTime> date, String pattern, String culture);
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



public class HtmlFunctions:ScriptObject, IDictionary<String,Object>, ICollection<KeyValuePair`2>, IEnumerable<KeyValuePair`2>, IEnumerable, IScriptObject, IDictionary, ICollection, IFormattable
	Int32 Count;
	Boolean IsReadOnly;
	Object Item;
	ICollection<String> Keys;
	ICollection<Object> Values;
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



public class IncludeFunction:IScriptCustomFunction, IScriptFunctionInfo
	Int32 RequiredParameterCount;
	Int32 ParameterCount;
	ScriptVarParamKind VarParamKind;
	Type ReturnType;
	Object Invoke(TemplateContext context, ScriptNode callerContext, ScriptArray arguments, ScriptBlockStatement blockStatement);
	ScriptParameterInfo GetParameterInfo(Int32 index);
	Int32 GetParameterIndexByName(String name);



public class IncludeJoinFunction:IScriptCustomFunction, IScriptFunctionInfo
	Int32 RequiredParameterCount;
	Int32 ParameterCount;
	ScriptVarParamKind VarParamKind;
	Type ReturnType;
	Object Invoke(TemplateContext context, ScriptNode callerContext, ScriptArray arguments, ScriptBlockStatement blockStatement);
	ScriptParameterInfo GetParameterInfo(Int32 index);
	String RenderComponent(TemplateContext context, ScriptNode callerContext, ScriptArray arguments, String component);



public class LiquidBuiltinsFunctions:ScriptObject, IDictionary<String,Object>, ICollection<KeyValuePair`2>, IEnumerable<KeyValuePair`2>, IEnumerable, IScriptObject, IDictionary, ICollection, IFormattable
	Int32 Count;
	Boolean IsReadOnly;
	Object Item;
	ICollection<String> Keys;
	ICollection<Object> Values;
	Boolean TryLiquidToScriban(String liquidBuiltin, String& target, String& member);
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



public class MathFunctions:ScriptObject, IDictionary<String,Object>, ICollection<KeyValuePair`2>, IEnumerable<KeyValuePair`2>, IEnumerable, IScriptObject, IDictionary, ICollection, IFormattable
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



public class ObjectFunctions:ScriptObject, IDictionary<String,Object>, ICollection<KeyValuePair`2>, IEnumerable<KeyValuePair`2>, IEnumerable, IScriptObject, IDictionary, ICollection, IFormattable
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



public class RegexFunctions:ScriptObject, IDictionary<String,Object>, ICollection<KeyValuePair`2>, IEnumerable<KeyValuePair`2>, IEnumerable, IScriptObject, IDictionary, ICollection, IFormattable
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
	RegexOptions GetOptions(String options);
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



public class StringFunctions:ScriptObject, IDictionary<String,Object>, ICollection<KeyValuePair`2>, IEnumerable<KeyValuePair`2>, IEnumerable, IScriptObject, IDictionary, ICollection, IFormattable
	Int32 Count;
	Boolean IsReadOnly;
	Object Item;
	ICollection<String> Keys;
	ICollection<Object> Values;
	StringBuilder GetTempStringBuilder();
	void ReleaseBuilder(StringBuilder builder);
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
	String Hash(HashAlgorithm algo, String text);
	String PadLeft(String text, Int32 width);
	String PadRight(String text, Int32 width);
	String Base64Encode(String text);
	String Base64Decode(String text);
	Int32 IndexOf(String text, String search, Nullable<Int32> startIndex, Nullable<Int32> count, String stringComparison);
	StringComparison GetComparison(String stringComparison, StringComparison defaultValue, Boolean throwExceptions);
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



public class TimeSpanFunctions:ScriptObject, IDictionary<String,Object>, ICollection<KeyValuePair`2>, IEnumerable<KeyValuePair`2>, IEnumerable, IScriptObject, IDictionary, ICollection, IFormattable
	TimeSpan Zero;
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



public class [nested] TryGetMemberDelegate:MulticastDelegate, ICloneable, ISerializable
	MethodInfo Method;
	Object Target;
	Boolean Invoke(TemplateContext context, SourceSpan span, Object target, String member, Object& value);
	IAsyncResult BeginInvoke(TemplateContext context, SourceSpan span, Object target, String member, Object& value, AsyncCallback callback, Object object);
	Boolean EndInvoke(Object& value, IAsyncResult result);
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



public class [nested] TryGetVariableDelegate:MulticastDelegate, ICloneable, ISerializable
	MethodInfo Method;
	Object Target;
	Boolean Invoke(TemplateContext context, SourceSpan span, ScriptVariable variable, Object& value);
	IAsyncResult BeginInvoke(TemplateContext context, SourceSpan span, ScriptVariable variable, Object& value, AsyncCallback callback, Object object);
	Boolean EndInvoke(Object& value, IAsyncResult result);
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



public class [nested] RenderRuntimeExceptionDelegate:MulticastDelegate, ICloneable, ISerializable
	MethodInfo Method;
	Object Target;
	String Invoke(ScriptRuntimeException exception);
	IAsyncResult BeginInvoke(ScriptRuntimeException exception, AsyncCallback callback, Object object);
	String EndInvoke(IAsyncResult result);
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



public class [nested] Enumerator:ValueType, IEnumerator<TScriptNode>, IDisposable, IEnumerator
	TScriptNode Current;
	Boolean MoveNext();
	void Reset();
	void Dispose();
	Boolean Equals(Object obj);
	String ToString();
	Int32 GetHashCode();
	Int32 GetHashCodeOfPtr(IntPtr ptr);



public class [nested] Enumerator:ValueType, IEnumerator<Token>, IDisposable, IEnumerator
	Token Current;
	Boolean MoveNext();
	void Reset();
	void Dispose();
	Boolean Equals(Object obj);
	String ToString();
	Int32 GetHashCode();
	Int32 GetHashCodeOfPtr(IntPtr ptr);