//..begin "File Description"
/*--------------------------------------------------------------------------------*
   Filename:  Core.cs
   Tool:      objectiF, CSharpSSvr V7.2.24
 *--------------------------------------------------------------------------------*/
//..end "File Description"


using System;
using System.Collections.Generic;
using System.Globalization;
using InterpretatorEnvironment;
using FunctionsList;

namespace programmatronCore
{	
	public class LexemAnalyzer
	{
		String sourceCode;
		List<Lexem> lexems;
		int i;
        int oldIndex = 0;
        int currentStringNumber=1;
		bool findQuote = false;
		
		public LexemAnalyzer(String sourceCode)
		{
		    initialize();
		    this.sourceCode = sourceCode;
		}
		
		public LexemAnalyzer()
		{
		    initialize();
		    this.sourceCode = "";
		}
		
		void initialize()
		{
		    lexems = new List<Lexem>();
		    i = 0;
		}
		
		public List<Lexem> analizeFrom(String otherCode)
		{
		    this.sourceCode = otherCode;
		    return analize();
		}
		
		public List<Lexem> analize()
		{
		    try
		    {
		        checkArgs();
		        analizeReady();
		        while (canTakeLexem())
		        {
		            while(isNeedSkipSymbol())
		            {
                        checkNextString();
		                i++;
		            }
                    oldIndex = i;
		            takeLexem();
		            while (!endOfSourceCode() && isNeedSkipSymbol())
                    {
                        checkNextString();
		                i++;
		            }
		        }
		    }
		    catch (Exception ex)
		    {
		        if (ex is ArgumentException)
		            InterpretatorEnvironment.Env.reporter.describeError("Невозможно запустить синтаксический анализ. Вероятно, исходный код пуст. Напишите код, а затем запускайте.", 101000001, 0, "Пустой код");
		    }
		    finally
		    {
		        lexems.Add(new Lexem(currentStringNumber+1,i,i,ProgrammatronTables.SourceCodeEnd));
		    }
		    return this.lexems;
		
		}

        private void checkNextString()
        {
            if (symbolIsStringEnd())
            {
                currentStringNumber++;
            }
        }
		
		void checkArgs()
		{
		    if (lexems == null || sourceCode == null || sourceCode.Length == 0)
		    {
		        throw new ArgumentException("Нулевой(ые) аргумент(ы) или пустой исходный код. Работа невозможна.");
		    }
		}
		
		void analizeReady()
		{
		    i = 0;
		    lexems.Clear();
		}
		
		bool canTakeLexem()
		{
		    return i < sourceCode.Length;
		}
		
		bool isNeedSkipSymbol()
		{
		    return sourceCode[i] == '\n' || sourceCode[i] == '\t' || sourceCode[i] == '\r' || sourceCode[i] == ' ';
		}
        
        bool symbolIsStringEnd()
        {
            return sourceCode[i] == '\n';
        }
		
		void takeLexem()
		{
		    String currentLexem = "";
		    do
		    {
		        currentLexem += sourceCode[i];
		        checkQuote();
		        if (findQuote)
		        {
		            i++;
		            while (findQuote)
		            {
		                checkQuote();
		                currentLexem += sourceCode[i];
		                i++;
		            }
		            continue;
		        }
		        if (endOfSourceCode())
		        {
		            break;
		        }
		        if (symbolIsReserved())
		        {
		            i++;
		            break;
		        }
		        i++;
		    } while (symbolIsNotEndOfLexem());
		
		    if (currentLexem.Length == 0)
		    {
                InterpretatorEnvironment.Env.reporter.describeError("Сгенерирована пустая лексема", 101000002, currentStringNumber, "Внутренняя ошибка");
		    }
		    lexems.Add(new Lexem(currentStringNumber,oldIndex,i,currentLexem));
		}
		
		bool endOfSourceCode()
		{
		    return i >= sourceCode.Length;
		}
		
		bool checkQuote()
		{
		    if (sourceCode[i] == '\"')
		    {
		        findQuote = !findQuote;
		    }
		    return findQuote;
		}
		
		bool symbolIsEndOfLexem()
		{
		    return (ProgrammatronTables.ReservedSymbols.Contains(sourceCode[i].ToString()) || sourceCode[i] == ' ' || sourceCode[i] == '\0');
		}
		
		bool symbolIsReserved()
		{
		    return ProgrammatronTables.ReservedSymbols.Contains(sourceCode[i].ToString());
		}
		
		bool symbolIsNotEndOfLexem()
		{
		    return !symbolIsEndOfLexem();
		}
		
		List<Lexem> Lexems
		{
			get
			{
				return lexems; 
			}
		}
	}

    public abstract class SyntaxTreeNode
    {
        protected List<SyntaxTreeNode> _Childs;

        public List<SyntaxTreeNode> Childs
        {
            get { return _Childs; }
        }

        public SyntaxTreeNode Parent;
        public SyntaxTreeNode()
        {
            _Childs = new List<SyntaxTreeNode>();
        }
        public virtual String Execute()
        {
            return "";//TODO реализовать режим отладки из IDE
        }

        protected string MathCalculatingOperation()
        {
            if (_Childs.Count == 2)
            {
                String leftValue = _Childs[0].Execute();
                String rightValue = _Childs[1].Execute();
                return MathOperation(leftValue, rightValue);
            }
            if (_Childs.Count == 1 && (this is MathSubstraction || this is MathAddition))
            {
                String leftValue = "0";
                String rightValue = _Childs[0].Execute();
                return MathOperation(leftValue, rightValue);
            }
            if (_Childs.Count == 1 && (this is MathDivision))
            {
                String leftValue = "1";
                String rightValue = _Childs[0].Execute();
                return MathOperation(leftValue, rightValue);
            }
            InterpretatorEnvironment.Env.reporter.describeError("Неверно построено синтаксическое дерево. Обратитесь в поддержку.", 100119999, _Childs[0].GetStringNumber(), "Внутренняя ошибка");
            return "";
        }

        public virtual String MathOperation(String left, String right)
        {
            return "";
        }

        protected String buildStringSumm(String a, String b)
        {
            if (a.Length == 0)
                return b;
            if (b.Length == 0)
                return a;
            String result = "";
            int i;
            if (a[0] == '\"')
            {
                result += a[0];
                i = 1;
                while (a[i] != '\"')
                {
                    result += a[i];
                    i++;
                }
            }
            else
            {
                result += '\"';
                i = 0;
                while (i < a.Length)
                {
                    result += a[i];
                    i++;
                }
            }
            if (b[0] == '\"')
            {
                i = 1;
                while (i < b.Length)
                {
                    result += b[i];
                    i++;
                }
            }
            else
            {
                i = 0;
                while (i < b.Length)
                {
                    result += b[i];
                    i++;
                }
                result += '\"';
            }
            return result;
        }

        protected String getStringVariableWithoutQuotes(String s)
        {
            if (s.Length == 0)
                return "";
            if (s[0] == '\"' && s[s.Length - 1] == '\"')
            {
                return s.Remove(0, 1).Remove(s.Length - 2, 1);
            }
            return s;
        }

        protected bool variableValueIsString(String value)
        {
            if (value.Length == 0)
                return false;
            return value[0] == '\"' && value[value.Length - 1] == '\"';
        }

        public virtual int GetStringNumber()
        {
            return _Childs[0].GetStringNumber();
        }
    }

    public class VariableDefinition:SyntaxTreeNode
    {
        Lexem _VariableName;

        public Lexem VariableName { get { return _VariableName; } }

        public VariableDefinition(Lexem VarName):base()
        {
            _VariableName = VarName;
        }

        public override String Execute()
        {
            String Result = base.Execute();
            Variables.Storage.Add(VariableName.Value, "");
            return Result;
        }

        public override int GetStringNumber()
        {
            return _VariableName.StringNumber;
        }
    }

    public class VariableAssignment:SyntaxTreeNode
    {
        private Lexem _VariableName;

        public Lexem VariableName
        {
            get { return _VariableName; }
        }

        public VariableAssignment(Lexem VarName):base()
        {
            _VariableName = VarName;
        }

        public override string Execute()
        {
            String Result = base.Execute();
            if (Variables.Storage.ContainsKey(_VariableName))
            {
                Result = Variables.Storage[_VariableName] = _Childs[0].Execute();
            }
            else
            {
                Env.reporter.describeError("Неизвестный идентификатор " + _VariableName, 100100005, VariableName.StringNumber, "Ошибка присвоения");
            }
            return Result;
        }

        public override int GetStringNumber()
        {
            return _VariableName.StringNumber;
        }
    }

    public class VariableDefinitionAssignment : SyntaxTreeNode
    {
        private Lexem _VariableName;

        public Lexem VariableName
        {
            get { return _VariableName; }
        }

        public VariableDefinitionAssignment(Lexem VarName)
            : base()
        {
            _VariableName = VarName;
        }

        public override string Execute()
        {
            String Result = base.Execute();
            Result = _Childs[0].Execute();
            Variables.Storage.Add(_VariableName, Result);
            return Result;
        }

        public override int GetStringNumber()
        {
            return _VariableName.StringNumber;
        }
    }

    public class ProgramNode:SyntaxTreeNode
    {
        public ProgramNode():base()
        {

        }
        public override string Execute()
        {
            String Result = base.Execute();
            foreach(var child in _Childs)
            {
                Result = child.Execute();
            }
            return Result;
        }
    }

    public class MathAddition:SyntaxTreeNode
    {
        public MathAddition()
        {

        }

        public override string Execute()
        {
            String Result = base.Execute();
            Result = base.MathCalculatingOperation();
            return Result;
        }

        public override string MathOperation(string left, string right)
        {
            float leftFloatValue = 0;
            float rightFloatValue = 0;
            int leftIntValue = 0;
            int rightIntValue = 0;
            if (base.variableValueIsString(left) || variableValueIsString(right))
            {
                //что-то из операндов строки, складываем как строки
                return buildStringSumm(left, right);
            }
            if (float.TryParse(left, NumberStyles.Any, new CultureInfo("en-US"), out leftFloatValue) && float.TryParse(right, NumberStyles.Any, new CultureInfo("en-US"), out rightFloatValue))
            {
                return (leftFloatValue + rightFloatValue).ToString(new CultureInfo("en-US"));
            }

            Env.reporter.describeError("Аргументы не подходят для указанной операции для " + left + right, 100100005, -1, "Неверные аргументы");
            return "";
        }

    }

    public class MathSubstraction : SyntaxTreeNode
    {
        public MathSubstraction()
        {

        }

        public override string Execute()
        {
            String Result = base.Execute();
            Result = base.MathCalculatingOperation();
            return Result;
        }

        public override string MathOperation(string left, string right)
        {
            float leftFloatValue = 0;
            float rightFloatValue = 0;
            int leftIntValue = 0;
            int rightIntValue = 0;
            if (base.variableValueIsString(left) || variableValueIsString(right))
            {
                //что-то из операндов строки, складываем как строки
                return buildStringSumm(left, right);
            }
            if (float.TryParse(left, NumberStyles.Any, new CultureInfo("en-US"), out leftFloatValue) && float.TryParse(right, NumberStyles.Any, new CultureInfo("en-US"), out rightFloatValue))
            {
                return (leftFloatValue - rightFloatValue).ToString(new CultureInfo("en-US"));
            }

            Env.reporter.describeError("Аргументы не подходят для указанной операции для " + left + right, 100100005, -1, "Неверные аргументы");
            return "";
        }

    }

    public class MathDivision : SyntaxTreeNode
    {
        public MathDivision()
        {

        }

        public override string Execute()
        {
            String Result = base.Execute();
            Result = base.MathCalculatingOperation();
            return Result;
        }

        public override string MathOperation(string left, string right)
        {
            float leftFloatValue = 0;
		    float rightFloatValue = 0;
		    int leftIntValue = 0;
		    int rightIntValue = 0;
            if (variableValueIsString(left) || variableValueIsString(right))
		    {
		        //что-то из операндов строки, складываем как строки
		        throw new ArgumentException("Нельзя делить строки");
		    }
            if (float.TryParse(left, NumberStyles.Any, new CultureInfo("en-US"), out leftFloatValue) && float.TryParse(right, NumberStyles.Any, new CultureInfo("en-US"), out rightFloatValue))
		    {
		        return (leftFloatValue / rightFloatValue).ToString(new CultureInfo("en-US"));
		    }

            Env.reporter.describeError("Аргументы не подходят для указанной операции для " + left + right, 100100005, -1, "Неверные аргументы");
            return "";
        }

    }

    public class MathMultiplication : SyntaxTreeNode
    {
        public MathMultiplication()
        {

        }

        public override string Execute()
        {
            String Result = base.Execute();
            Result = base.MathCalculatingOperation();
            return Result;
        }

        public override string MathOperation(string left, string right)
        {
            float leftFloatValue = 0;
            float rightFloatValue = 0;
            int leftIntValue = 0;
            int rightIntValue = 0;
            if (variableValueIsString(left) || variableValueIsString(right))
            {
                //что-то из операндов строки, складываем как строки
                throw new ArgumentException("Нельзя делить строки");
            }
            if (float.TryParse(left, NumberStyles.Any, new CultureInfo("en-US"), out leftFloatValue) && float.TryParse(right, NumberStyles.Any, new CultureInfo("en-US"), out rightFloatValue))
            {
                return (leftFloatValue * rightFloatValue).ToString(new CultureInfo("en-US"));
            }

            Env.reporter.describeError("Аргументы не подходят для указанной операции для " + left + right, 100100005, -1, "Неверные аргументы");
            return "";
        }
    }
	
    /// <summary>
    /// Узел дерева, когда используется значение переменной в качесве операнда
    /// </summary>
    public class VariableValue : SyntaxTreeNode
    {
        Lexem _VariableName;

        public Lexem VariableName { get { return _VariableName; } }

        public VariableValue(Lexem VariableName):base()
        {
            _VariableName = VariableName;
        }

        public override int GetStringNumber()
        {
            return _VariableName.StringNumber;
        }

        public override string Execute()
        {
            String Result = base.Execute();
            if (Variables.Storage.ContainsKey(_VariableName))
            {
                Result = Variables.Storage[_VariableName];
            }
            else
            {
                Env.reporter.describeError("Неизвестный идентификатор " + _VariableName, 100100005, _VariableName.StringNumber, "Неверный идентификатор");
                Result = "";
            }
            return Result;
        }
    }

    public class AtomicValue : SyntaxTreeNode
    {
        private Lexem _Value;

        public Lexem Value
        {
            get { return _Value; }
            set { _Value = value; }
        }

        public AtomicValue(Lexem Value)
        {
            this._Value = Value;
        }

        public override string Execute()
        {
            return _Value.Value;
        }

        public override int GetStringNumber()
        {
            return _Value.StringNumber;
        }
    }

    public class Block : SyntaxTreeNode
    {
        public Block():base()
        {

        }

        public override string Execute()
        {
            String Result = base.Execute();
            foreach (var child in _Childs)
            {
                Result = child.Execute();
            }
            return Result;
        }
    }

    public class Error : SyntaxTreeNode
    {
        private Lexem _WrongLexem;

        public Lexem WrongLexem
        {
            get { return _WrongLexem; }
            set { _WrongLexem = value; }
        }


        public Error(Lexem WrongLexem):base()
        {
            _WrongLexem = WrongLexem;
        }

        public override string Execute()
        {
            return base.Execute();
        }

        public override int GetStringNumber()
        {
            if (_Childs.Count != 0)
                return _Childs[0].GetStringNumber();
            else
                return 0;
        }
    }

    public class FunctionCall : SyntaxTreeNode
    {
        private Lexem _FunctionName;

        public Lexem FunctionName
        {
            get { return _FunctionName; }
        }
        

        public FunctionCall(Lexem FuncName):base()
        {
            _FunctionName = FuncName;
        }

        public override string Execute()
        {
            List<FunctionArgument> args = new List<FunctionArgument>();
            foreach (var cur in _Childs)
            {
                FunctionArgument arg = new FunctionArgument();
                if (cur is AtomicValue)
                {
                    arg.argument = cur.Execute();
                    if (variableValueIsString(arg.argument))
                    {
                        arg.ArgumentTypeEnum = ValueTypeEnum.String;
                    }
                    else
                    {
                        arg.ArgumentTypeEnum = ValueTypeEnum.Number;
                    }
                }
                else if (cur is VariableValue)
                {
                    arg.ArgumentTypeEnum = ValueTypeEnum.Variable;
                    arg.argument = cur.Execute();
                    arg.argument = (cur as VariableValue).VariableName;
                }
                else
                {
                    arg.ArgumentTypeEnum = ValueTypeEnum.Variable;
                    arg.argument = cur.Execute();
                }
                args.Add(arg);
            }
            FunctionResult result;
            try
            {
                result = Functions.CallFunction(args, _FunctionName);
                return result.Result;
            }
            catch (WrongArgumentCountException ex)
            {
                Env.reporter.describeError(ex.Message, 100100007, _FunctionName.StringNumber, "Неверное количество аргументов");
                throw ex;
            }
            catch (WrongFormatArgumentException ex)
            {
                Env.reporter.describeError(ex.Message, 100100009, _FunctionName.StringNumber, "Неверный формат аргументов");
                throw ex;
            }
            catch (UndefinedVariableException ex)
            {
                Env.reporter.describeError(ex.Message, 100100010, _FunctionName.StringNumber, "Не определена переменная");
                throw ex;
            }
            catch (NotImplementedException ex)
            {
                InterpretatorEnvironment.Env.reporter.describeError(ex.Message, 100100008, _FunctionName.StringNumber, "Несуществующая функция");
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    public class ArgumentList : SyntaxTreeNode
    {

    }

	public enum TermType
	{
		///упростить существенно, оставить только нетерминалы
		variableDefinition,
		variableAssignment,
		variableDefinitionAssignment,
		program,
		mathAddition,
		mathSubtraction,
		mathDivision,
		mathMultiplication,
		variableValue,
		atomicValue,
		block,
		error,
		functionCall,

		none
	}
	
	public enum VariableType
	{
		universal,
		integer,
		characters,
		fraction,
		none
	}
	
	public class SyntaxTreeGenerator
	{
		SyntaxTreeNode headOfSyntaxTree;
		List<Lexem> lexems;
        SyntaxTreeNode currentHead;
		int lexemIndex = 0;
		
		public SyntaxTreeGenerator(List<Lexem> lexems)
		{
		    initialize();
		    this.lexems = lexems;
		}
		
		public SyntaxTreeGenerator()
		{
		    initialize();
		}
		
		void initialize()
		{
		    headOfSyntaxTree = new ProgramNode();
		    currentHead = headOfSyntaxTree;
		    lexems = new List<Lexem>();
		    Variables.Storage = new Dictionary<String, String>();
		}
		
		public void generateTreeFromLexems(List<Lexem> otherLexems)
		{
		    this.lexems = otherLexems;
		    generateTree();
		}
		
		public void generateTree()
		{
		    //clear();
            headOfSyntaxTree = new ProgramNode();
		    currentHead = headOfSyntaxTree;
		    Variables.Storage = new Dictionary<String, String>();
		    //
		    buildProgramTree();
		    //startProgram();
		}
		
		private void buildProgramTree()
		{
		    TermType typeOfNextTerm;
		    do
		    {
		        //TODO ... временное решение, в последсвии удалить отсюда и распихать по классу Term кучу функций инициализации//нет ничего более постоянного, чем временное
		        int oldLexemIndex = lexemIndex;
		        typeOfNextTerm = getTermTypeInBlockByCurrentLexemIndex(oldLexemIndex, out lexemIndex);
		
		        if (typeOfNextTerm == TermType.variableDefinition)
		        {
                    ExtractVariableDefinition(oldLexemIndex);
		        }
		        else if (typeOfNextTerm == TermType.variableDefinitionAssignment)
		        {
                    ExtractVariableDefinitionAssignment(oldLexemIndex);
		        }
		        else if (typeOfNextTerm == TermType.variableAssignment)
		        {
                    ExctractVariableAssingment(oldLexemIndex);
		        }
		        else if(typeOfNextTerm == TermType.functionCall)
		        {
                    ExtractFunctionCall(oldLexemIndex);
		        }
		        else if (typeOfNextTerm == TermType.error)
		        {
                    if (lexemIndex == -1)
                        lexemIndex = 0;
                    InterpretatorEnvironment.Env.reporter.describeError("Неизвестная синтаксическая структура", 100110004, lexems[lexemIndex].StringNumber, "Синтаксическая ошибка");
                    return;
		        }
		        lexemIndex++;
		    } while ((typeOfNextTerm != TermType.none || typeOfNextTerm != TermType.error) && lexemIndex < lexems.Count - 1);
		}

        private void ExtractFunctionCall(int oldLexemIndex)
        {
            FunctionCall child = new FunctionCall(getFunctionNameFromCall(oldLexemIndex));
            parseArgumentListFromFunctionCall(child, oldLexemIndex + 2, lexemIndex - 2);
            currentHead.Childs.Add(child);
        }

        private void ExctractVariableAssingment(int oldLexemIndex)
        {
            int parseStartIndex = oldLexemIndex + 2;
            int parseEndIndex = lexemIndex - 1;
            VariableAssignment child = new VariableAssignment(getVariableNameFromAssignment(oldLexemIndex));
            parseValueLexemToTree(child, parseStartIndex, parseEndIndex);
            currentHead.Childs.Add(child);
        }

        private void ExtractVariableDefinitionAssignment(int oldLexemIndex)
        {
            int parseStartIndex = oldLexemIndex + 3;
            int parseEndIndex = lexemIndex - 1;
            VariableDefinitionAssignment child = new VariableDefinitionAssignment(getVariableNameFromDefinition(oldLexemIndex));
            ///////TODO refactoring

            if (getVariableTypeFromDefinitionAssignment(oldLexemIndex) == VariableType.none)
            {
                //TODO добавить в лог внутреннюю обработку рапортов и здесь сделать поддержку обработки ошибок
                InterpretatorEnvironment.Env.reporter.describeError(String.Format("Несуществующий тип указан в определении и присвоении переменной {0}", child.VariableName), 100010003, lexems[lexemIndex].StringNumber, "Ошибка определения");
            }
            parseValueLexemToTree(child, parseStartIndex, parseEndIndex);
            currentHead.Childs.Add(child);
        }

        private void ExtractVariableDefinition(int oldLexemIndex)
        {
            VariableDefinition child = new VariableDefinition(getVariableNameFromDefinition(oldLexemIndex));
            if (getVariableTypeFromDefinition(oldLexemIndex) == VariableType.none)
            {
                //TODO добавить в лог внутреннюю обработку рапортов и здесь сделать поддержку обработки ошибок
                InterpretatorEnvironment.Env.reporter.describeError(String.Format("Несуществующий тип указан в определении переменной {0}", child.VariableName), 100010003, lexems[lexemIndex].StringNumber, "Ошибка определения");
            }
            currentHead.Childs.Add(child);
            child.Parent = currentHead;
        }
		
		private void parseArgumentListFromFunctionCall(FunctionCall functionCallTerm,int start,int end)
		{
		    int leftComma=-1, rightComma = -1;
		    leftComma = findFirstSymbol(",", start, end);
		    rightComma = findFirstSymbol(",", start + 1, end);
		    if(leftComma!=-1&&rightComma!=-1&&leftComma!=rightComma)
		    {
		        parseValueLexemToTree(functionCallTerm, leftComma + 1, rightComma - 1);
		        parseArgumentListFromFunctionCall(functionCallTerm, rightComma+1,end);
		    }
		    if(leftComma==rightComma&&rightComma!=-1)
		    {
		        parseValueLexemToTree(functionCallTerm, start, rightComma-1);
		        parseArgumentListFromFunctionCall(functionCallTerm, rightComma + 1, end);
		    }
		    if(leftComma==-1&&rightComma==-1)
		    {
		        parseValueLexemToTree(functionCallTerm, start, end);
		    }
		}
		
		private Lexem getFunctionNameFromCall(int startIndex)
		{
		    return lexems[startIndex];
		}
		
		private TermType getTermTypeInBlockByCurrentLexemIndex(int startIndex, out int endIndex)
		{
		    int oldIndex = lexemIndex;
		    int nearestSemicolon = oldIndex;
		    nearestSemicolon = getNearestSemicolon(nearestSemicolon);
		    if (lexemIsVariableDefinition(oldIndex, nearestSemicolon))
		    {
		        endIndex = nearestSemicolon;
		        return TermType.variableDefinition;
		    }
		    if (lexemIsVariableDefinitionAndAssignment(oldIndex, nearestSemicolon))
		    {
		        endIndex = nearestSemicolon;
		        return TermType.variableDefinitionAssignment;
		    }
		    if (lexemsIsVariableAssignment(oldIndex, nearestSemicolon))
		    {
		        endIndex = nearestSemicolon;
		        return TermType.variableAssignment;
		    }
		    if(lexemsIsFunctionCall(oldIndex,nearestSemicolon))
		    {
		        endIndex = nearestSemicolon;
		        return TermType.functionCall;
		    }
		    if (!canToNextStep())
		    {
		        endIndex = -1;
		        return TermType.none;
		    }
		    else
		    {
		        endIndex = -1;
		        return TermType.error;
		    }
		}
		
		private bool lexemsIsFunctionCall(int startIndex, int endIndex)
		{
            if(lexemIsIdentifier(startIndex) && lexemIsOpenParentheses(startIndex+1) && lexemIsClosedParentheses(startIndex+2))
            {
                return true;
            }
		    if (lexemIsIdentifier(startIndex) && lexemIsOpenParentheses(startIndex + 1) && lexemsIsArgumentList(startIndex + 2, endIndex - 2) && lexemIsClosedParentheses(endIndex - 1))
		    {
		        return true;
		    }
		    return false;
		}
		
		private bool lexemsIsArgumentList(int start, int end)
		{

		    int leftComma = findFirstSymbol(",",start,end);
		    int rightComma = findFirstSymbol(",", start+1, end);
		    if (leftComma != -1 && rightComma != -1 && leftComma != rightComma&&lexemsIsValue(leftComma+1,rightComma-1)&&lexemsIsArgumentList(rightComma+1,end))
		    {
		        return true;
		    }
		    if (leftComma == rightComma && rightComma != -1&&lexemsIsValue(start,rightComma-1)&&lexemsIsArgumentList(rightComma+1,end))
		    {
		        return true;
		    }
		    if (leftComma == -1 && rightComma == -1&&lexemsIsValue(start,end))
		    {
		        return true;
		    }
		    return false;
		}
		
		private int findFirstSymbol(String symbol,int start,int end)
		{
		    int i=start;
		    while(i<=end&&i<lexems.Count)
		    {
		        if(lexems[i]==symbol)
		        {
		            return i;
		        }
		        i++;
		    }
		    return -1;
		}
		
		private int getNearestSemicolon(int start)
		{
		    return findFirstSymbol(";",start,lexems.Count-1);
		}
		
		private Lexem getVariableNameFromAssignment(int startLexemIndex)
		{
		    return lexems[startLexemIndex];
		}
		
		private Lexem getVariableNameFromDefinition(int startLexemIndex)
		{
		    return lexems[startLexemIndex + 1];
		}
		
		private List<String> getVariableValueFromAssignment(int startLexemIndex)
		{
		    return getListWithValueLexems(startLexemIndex, 2);
		}
		
		private List<string> getListWithValueLexems(int startLexemIndex, int beginValueIndexShift)
		{
		    List<String> result = new List<string>();
		    startLexemIndex += beginValueIndexShift;
		    while (!lexemIsSemicolon(startLexemIndex))
		    {
		        result.Add(lexems[startLexemIndex]);
		        startLexemIndex++;
		    }
		    return result;
		}
		
		private List<String> getVariableValueFromDefinitionAssignment(int startLexemIndex)
		{
		    return getListWithValueLexems(startLexemIndex, 3);
		}
		
		private VariableType getVariableTypeFromDefinitionAssignment(int startLexemIndex)
		{
		    if (lexems[startLexemIndex] == ProgrammatronTables.VariablesTypes[VariableType.universal])
		    {
		        // переделать. добавить поддержку строгой типизации
		        // TO DO решить, без строгой типизации или с ней
		        return VariableType.universal;
		    }
		    else
		    {
		        return VariableType.none;
		    }
		}
		
		private VariableType getVariableTypeFromDefinition(int startTermIndex)
		{
		    if (lexems[startTermIndex] == ProgrammatronTables.VariablesTypes[VariableType.universal])
		    {// переделать. добавить поддержку строгой типизации
		        // TO DO решить, без строгой типизации или с ней
		        return VariableType.universal;
		    }
		    else
		    {
		        return VariableType.none;
		    }
		}
		
		private void parseValueLexemToTree(SyntaxTreeNode root, int startIndex, int endIndex)
		{
		    bool inParentheses = parenthesesCorrection(ref startIndex, ref endIndex);
		    if (endIndex == startIndex && (valueIsString(startIndex) || valueIsNumber(startIndex)))
		    {
                root.Childs.Add(new AtomicValue(lexems[startIndex]));
		        return;
		    }
		    else if(endIndex == startIndex && lexemIsIdentifier(endIndex))
		    {
                root.Childs.Add(new VariableValue(lexems[endIndex]));
		        return;
		    }
            else if (lexemsIsFunctionCall(startIndex, endIndex))
            {
                root.Childs.Add(new FunctionCall(lexems[startIndex]));
                return;
            }
            else
            {
                ExtractLeftAndRightParts(root, startIndex, endIndex);
            }
		}

        private void ExtractLeftAndRightParts(SyntaxTreeNode root, int startIndex, int endIndex)
        {
            int basicPoint = getBasicPoint(startIndex, endIndex);
            int subBasicPoint = getSubBasicPoint(startIndex, endIndex);
            if (basicPoint != -1)
            {
                root.Childs.Add(parseLeftRightNodes(startIndex, endIndex, basicPoint));
                return;
            }
            else if (subBasicPoint != -1)
            {
                root.Childs.Add(parseLeftRightNodes(startIndex, endIndex, subBasicPoint));
                return;
            }
        }
		
		private SyntaxTreeNode parseLeftRightNodes(int startIndex, int endIndex, int basicPoint)
		{
		    switch (ProgrammatronTables.arithmeticSymbolToTermType[lexems[basicPoint]])
            {
                case TermType.mathAddition:
                    MathAddition mathA = new MathAddition();
                    fillAllAriphmeticNodes(mathA,startIndex,endIndex,basicPoint);
                    return mathA;
                case TermType.mathDivision:
                    MathDivision mathD = new MathDivision();
                    fillAllAriphmeticNodes(mathD, startIndex, endIndex, basicPoint);
                    return mathD;
                case TermType.mathMultiplication:
                    MathMultiplication mathM = new MathMultiplication();
                    fillAllAriphmeticNodes(mathM, startIndex, endIndex, basicPoint);
                    return mathM;
                case TermType.mathSubtraction:
                    MathSubstraction mathS = new MathSubstraction();
                    fillAllAriphmeticNodes(mathS, startIndex, endIndex, basicPoint);
                    return mathS;
            }
            throw new Exception();
		}

        private void fillAllAriphmeticNodes(SyntaxTreeNode subNode, int startIndex, int endIndex, int basicPoint)
        {
		    parseValueLexemToTree(subNode, startIndex, basicPoint - 1);
		    parseValueLexemToTree(subNode, basicPoint + 1, endIndex);
        }
		
		private int getBasicPoint(int startIndex, int endIndex)
		{
		    int i = endIndex;
		    int parenthesesCounter = 0;
		    for (; i >= startIndex; i--)
		    {
		        if (lexems[i] == ")")
		            parenthesesCounter++;
		        else if (lexems[i] == "(")
		            parenthesesCounter--;
		        if ((lexems[i] == "+" || lexems[i] == "-") && parenthesesCounter == 0)
		        {
		            return i;
		        }
		    }
		    return -1;
		}
		
		private bool parenthesesCorrection(ref int startIndex, ref int endIndex)
		{
		    if (lexems[startIndex] == "(" && lexems[endIndex] == ")")
		    {
		        startIndex++;
		        endIndex--;
		        return true;
		    }
		    return false;
		}
		
		private int getSubBasicPoint(int startIndex, int endIndex)
		{
		    int i = endIndex;
		    int parenthesesCounter = 0;
		    for (; i >= startIndex; i--)
		    {
		        if (lexems[i] == ")")
		            parenthesesCounter++;
		        else if (lexems[i] == "(")
		            parenthesesCounter--;
		        if ((lexems[i] == "*" || lexems[i] == "/") && parenthesesCounter == 0)
		        {
		            return i;
		        }
		    }
		    return -1;
		}
		
		private bool lexemsIsVariableAssignment(int startIndex, int endIndex)
		{
		    if (lexemIsIdentifier(startIndex) && lexemIsEqualSign(startIndex + 1) && lexemsIsValue(startIndex + 2, endIndex - 1) && lexemIsSemicolon(endIndex))
		    {
		        return true;
		    }
		    return false;
		}
		
		private bool lexemIsVariableDefinition(int startIndex, int endIndex)
		{
		    if (lexemIsType(startIndex) && lexemIsIdentifier(startIndex + 1) && lexemIsSemicolon(endIndex) && endIndex == startIndex + 2)
		    {
		        return true;
		    }
		    return false;
		}
		
		private bool lexemIsSemicolon(int index)
		{
		    return lexems[index] == ";";
		}
		
		private bool lexemIsVariableDefinitionAndAssignment(int startIndex, int endIndex)
		{
		    int index = startIndex;
		    if (lexemIsType(startIndex) && lexemIsIdentifier(startIndex + 1) && lexemIsEqualSign(startIndex + 2) && lexemsIsValue(startIndex + 3, endIndex - 1) && lexemIsSemicolon(endIndex))
		    {
		        return true;
		    }
		    return false;
		}
		
		private bool lexemsIsValue(int startIndex, int endIndex)
		{
		    //TO DO подумать над этим методом и постараться сократить
		    int oldLexemIndex = startIndex;
		    if (startIndex == endIndex && valueIsString(endIndex))
		    {
		        return true;
		    }
		    if (startIndex == endIndex && valueIsNumber(endIndex))
		    {
		        return true;
		    }
		    int basicPoint = getBasicPoint(startIndex,endIndex);
		    if (basicPoint!=-1 && lexemsIsValue(startIndex,basicPoint-1) && lexemsIsValue(basicPoint+1, endIndex))
		    {
		        return true;
		    }
		    int subBasicPoint = getSubBasicPoint(startIndex, endIndex);
		    if (subBasicPoint != -1 && lexemsIsValue(startIndex, subBasicPoint - 1) && lexemsIsValue(subBasicPoint + 1, endIndex))
		    {
		        return true;
		    }
		    if (valueInParenthneses(startIndex, endIndex))
		    {
		        return true;
		    }
		    if (startIndex == endIndex && lexemIsIdentifier(endIndex))
		    {
		        return true;
		    }
		    if(lexemIsArithmeticOparation(startIndex)&&valueIsNumber(endIndex))
		    {
		        return true;
		    }
            if(lexemsIsFunctionCall(startIndex,endIndex))
            {
                return true;
            }
		    return false;
		}
		
		private void backToStartIndexLexems(int index)
		{
		    lexemIndex = index;
		}
		
		private void fillValueLexemsList(ref List<String> list, int start, int end)
		{
		    for (int i = start; i < end; i++)
		    {
		        list.Add(lexems[i]);
		    }
		}
		
		private bool valueInParenthneses(int startIndex, int endIndex)
		{
		    //переделать совсем код проверки везде
		    if (lexems[startIndex] == "(" && lexems[endIndex] == ")")
		    {
		        if(lexemsIsValue(startIndex + 1, endIndex - 1))
		        {
		            return true;
		        }
		    }
		    return false;
		}
		
		bool valueIsNumber(int index)
		{
		    bool result = isNumber(lexems[index]);
		    return result;
		}
		
		public static bool isNumber(String str)
		{
		    int useless = 0;
		    float uselss = 0.0f;
		    return int.TryParse(str, NumberStyles.Any, new CultureInfo("en-US"), out useless) || float.TryParse(str, NumberStyles.Any, new CultureInfo("en-US"), out uselss);
		}
		
		private bool valueIsExtention(int startIndex, int endIndex)
		{
		    if (lexemIsArithmeticOparation(startIndex) && lexemsIsValue(startIndex + 1, endIndex))
		    {
		        return true;
		    }
		    return false;
		}
		
		private bool lexemIsClosedParentheses(int index)
		{
		    return lexems[index] == ")";
		}
		
		private bool lexemIsOpenParentheses(int index)
		{
		    return lexems[index] == "(";
		}
		
		private bool valueIsString(int index)
		{
            return isString(lexems[index]);
		}

        public static bool isString(String value)
        {
            return value[0] == '\"' && value[value.Length - 1] == '\"';
        }
		
		private bool variableValueIsString(String value)
		{
            if (value.Length == 0)
                return false;
		    return value[0] == '\"' && value[value.Length - 1] == '\"';
		}
		
		private bool lexemIsEqualSign(int index)
		{
		    return lexems[index] == "=";
		}
		
		private bool stepToNextLexemIfCan()
		{
		    if (canToNextStep())
		    {
		        lexemIndex++;
		        return true;
		    }
		    else
		    {
		        return false;
		    }
		}
		
		private bool canToNextStep()
		{
		    return lexemIndex + 1 < lexems.Count;
		}
		
		private bool stepToNextLexem()
		{
		    lexemIndex++;
		    return true;
		}
		
		private bool lexemIsIdentifier(int index)
		{
		    return isIdentifier(lexems[index]);
		}
		
		private bool isIdentifier(String lexem)
		{
		    return lexem[0] == '_' || char.IsLetter(lexem, 0);
		}
		
		private bool lexemIsType(int index)
		{
		    return ProgrammatronTables.VariablesTypes.ContainsValue(lexems[index]);
		}
		
		private bool lexemIsArithmeticOparation(int index)
		{
		    return ProgrammatronTables.ArithmeticOperations.Contains(lexems[index]);
		}
		
		//TO DO вынести код ниже вообще в отдельный класс выполнителя программы
		
		public void doCode()
		{
            try
            {
                doProcessing(headOfSyntaxTree);
            }
            catch(Exception ex)
            {
                InterpretatorEnvironment.Env.log.message("Исправьте указанную ошибку, затем снова запустите");
            }
		}
		
		/// <summary>
		/// Функция запускается во время выполнения кода. TO DO выделить функции выполнения кода в отдельный класс
		/// </summary>
		private void doProcessing(SyntaxTreeNode workingTerm)
		{
            try
            {
                workingTerm.Execute();
#if DEBUG
		        foreach (KeyValuePair<string, string> current in Variables.Storage)
		            VMEnv.Env.log.message(current.Key + "=" + current.Value);
#endif


            }
            catch(Exception ex)
            {
                Env.log.message("Выполнение прервано на этапе обработки. "+ex.ToString());
                throw ex;
            }
		}
	}
}