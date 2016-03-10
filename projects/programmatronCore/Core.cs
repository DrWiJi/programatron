//..begin "File Description"
/*--------------------------------------------------------------------------------*
   Filename:  Core.cs
   Tool:      objectiF, CSharpSSvr V7.2.24
 *--------------------------------------------------------------------------------*/
//..end "File Description"


using System;
using System.Collections.Generic;
using System.Globalization;
using InterpretatorEnveronment;
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
		            InterpretatorEnveronment.Env.reporter.describeError("Невозможно запустить синтаксический анализ. Вероятно, исходный код пуст. Напишите код, а затем запускайте.", 101000001, 0, "Пустой код");
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
                InterpretatorEnveronment.Env.reporter.describeError("Сгенерирована пустая лексема", 101000002, currentStringNumber, "Внутренняя ошибка");
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
	
	public class Term
	{
		Term parent;
		TermType type;
		List<Term> childs;
		Lexem value;
		Lexem name;
		bool valueIsExpression;
		bool valueIsAssigned;
		VariableType variableType;
		
		public bool ValueIsExpression
		{
			get
			{
				return valueIsExpression; 
			}
			
			set
			{
				valueIsExpression = value; 
			}
		}
		
		public Lexem Name
		{
			get
			{
			    return name;
			}
			
			set
			{
			    this.name = value;
			}
		}
		
		public Term(TermType type)
		{
		    initialize();
		    this.type = type;
		}
		
		public Term(Term parent, TermType type, VariableType varType)
		{
		    initialize();
		    this.parent = parent;
		    this.type = type;
		    this.variableType = varType;
		}
		
		public Term(Term parent, TermType type, Lexem value)
		{
		    initialize();
		    this.parent = parent;
		    this.type = type;
		    this.value = value;
		    valueIsAssigned = true;
		
		}
		
		public Term(Term parent, TermType type)
		{
		    initialize();
		    this.parent = parent;
		    this.type = type;
		}
		
		public Term(Term parent)
		{
		    initialize();
		    this.parent = parent;
		}
		
		public Term()
		{
		    initialize();
		}
		
		void initialize()
		{
		    parent = null;
		    type = TermType.none;
		    variableType = VariableType.none;
		    childs = new List<Term>();
		    value = null;
		    valueIsExpression = false;
		}
		
		public bool ValueIsAssigned
		{
			get
			{
				return valueIsAssigned; 
			}
		}
		
		public Lexem Value
		{
			get
			{
				return value; 
			}
			
			set
			{
			    this.value = value;
			    valueIsAssigned = true;
			}
		}
		
		public void addChild(Term child)
		{
		    child.parent = this;
		    childs.Add(child);
		}
		
		public Term getChildAt(int index)
		{
		    return childs[index];
		}
		
		public Term this[int index]
		{
			get
			{
				return childs[index]; 
			}
		}
		
		public VariableType VariableType
		{
			get
			{
				return variableType; 
			}
		}
		
		public TermType Type
		{
			get
			{
				return type; 
			}
			
			set
			{
				type = value; 
			}
		}
		
		public List<Term> Childs
		{
			get
			{
			    return childs;
			}
		}
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
		argumentList,
		argumentListAddition,
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
		Term headOfSyntaxTree;
		List<Lexem> lexems;
		
		//TO DO переместить в Term
		
		Term currentHead;
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
		    headOfSyntaxTree = new Term(TermType.program);
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
		    headOfSyntaxTree = new Term(TermType.program);
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
		        //TO DO ... временное решение, в последсвии удалить отсюда и распихать по классу Term кучу функций инициализации//нет ничего более постоянного, чем временное
		        int oldLexemIndex = lexemIndex;
		        typeOfNextTerm = getTermTypeInBlockByCurrentLexemIndex(oldLexemIndex, out lexemIndex);
		
		        if (typeOfNextTerm == TermType.variableDefinition)
		        {
		            Term child = new Term(currentHead, TermType.variableDefinition, getVariableTypeFromDefinition(oldLexemIndex));
		            if (child.VariableType == VariableType.none)
		            {
		                //TO DO добавить в лог внутреннюю обработку рапортов и здесь сделать поддержку обработки ошибок
                        InterpretatorEnveronment.Env.reporter.describeError(String.Format("Несуществующий тип указан в определении переменной {0}", child.Value), 100010003, lexems[lexemIndex].StringNumber, "Ошибка определения");
		            }
		            child.Name = getVariableNameFromDefinition(oldLexemIndex);
		            currentHead.addChild(child);
		        }
		        else if (typeOfNextTerm == TermType.variableDefinitionAssignment)
		        {
		            Term child = new Term(currentHead, TermType.variableDefinitionAssignment, getVariableTypeFromDefinitionAssignment(oldLexemIndex));
		            ///////TO DO refactoring
		            int parseStartIndex = oldLexemIndex + 3;
		            int parseEndIndex = lexemIndex - 1;
		            if (child.VariableType == VariableType.none)
		            {
		                //TO DO добавить в лог внутреннюю обработку рапортов и здесь сделать поддержку обработки ошибок
                        InterpretatorEnveronment.Env.reporter.describeError(String.Format("Несуществующий тип указан в определении и присвоении переменной {0}", child.Value), 100010003, lexems[lexemIndex].StringNumber, "Ошибка определения");
                    }
		            child.Name = getVariableNameFromDefinition(oldLexemIndex);
		            parseValueLexemToTree(child, parseStartIndex, parseEndIndex);
		            currentHead.addChild(child);
		
		        }
		        else if (typeOfNextTerm == TermType.variableAssignment)
		        {
		            Term child = new Term(currentHead, TermType.variableAssignment);
		            ////////TO DO refactoring
		            int parseStartIndex = oldLexemIndex + 2;
		            int parseEndIndex = lexemIndex - 1;
		            child.Name = getVariableNameFromAssignment(oldLexemIndex);
		            parseValueLexemToTree(child, parseStartIndex, parseEndIndex);
		            currentHead.addChild(child);
		        }
		        else if(typeOfNextTerm == TermType.functionCall)
		        {
		            Term child = new Term();
		            child.Name = getFunctionNameFromCall(oldLexemIndex);
		            child.Type = TermType.functionCall;
		            parseArgumentListFromFunctionCall(child, oldLexemIndex+2, lexemIndex-2);
		            currentHead.addChild(child);
		        }
		        else if (typeOfNextTerm == TermType.error)
		        {
                    InterpretatorEnveronment.Env.reporter.describeError("Неизвестная синтаксическая структура", 100110004, lexems[lexemIndex].StringNumber, "Синтаксическая ошибка");
		        }
		         lexemIndex++;
		    } while ((typeOfNextTerm != TermType.none || typeOfNextTerm != TermType.error) && lexemIndex < lexems.Count - 1);
		}
		
		private void parseArgumentListFromFunctionCall(Term functionCallTerm,int start,int end)
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
		    if (lexemIsIdentifier(startIndex) && lexemIsOpenParentheses(startIndex + 1) && lexemsIsArgumentList(startIndex + 2, endIndex - 2) && lexemIsClosedParentheses(endIndex - 1) && lexemIsSemicolon(endIndex))
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
		
		private void parseValueLexemToTree(Term root, int startIndex, int endIndex)
		{
		    Term subTerm = new Term();
		    bool inParentheses = parenthesesCorrection(ref startIndex, ref endIndex);
		    if (endIndex == startIndex && (valueIsString(startIndex) || valueIsNumber(startIndex)))
		    {
		        subTerm.Value = lexems[startIndex];
		        subTerm.Type = TermType.atomicValue;
		        root.addChild(subTerm);
		        return;
		    }
		    else if(endIndex == startIndex && lexemIsIdentifier(endIndex))
		    {
		        subTerm.Name = lexems[endIndex];
		        subTerm.Type = TermType.variableValue;
		        root.addChild(subTerm);
		        return;
		    }
		    int basicPoint = getBasicPoint(startIndex, endIndex);
		    int subBasicPoint = getSubBasicPoint(startIndex, endIndex);
		    if (basicPoint != -1)
		    {
		        parseLeftRightTerms(startIndex, endIndex,ref subTerm, basicPoint);
		        root.addChild(subTerm);
		        return;
		    }else if(subBasicPoint != -1)
		    {
		        parseLeftRightTerms(startIndex, endIndex,ref subTerm, subBasicPoint);
		        root.addChild(subTerm);
		        return;
		    }
		}
		
		private void parseLeftRightTerms(int startIndex, int endIndex,ref Term subTerm, int basicPoint)
		{
		    parseValueLexemToTree(subTerm, startIndex, basicPoint - 1);
		    parseValueLexemToTree(subTerm, basicPoint + 1, endIndex);
		    subTerm.Type = ProgrammatronTables.arithmeticSymbolToTermType[lexems[basicPoint]];
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
                InterpretatorEnveronment.Env.log.message("Исправьте указанную ошибку, затем снова запустите");
            }
		}
		
		/// <summary>
		/// Функция запускается во время выполнения кода. TO DO выделить функции выполнения кода в отдельный класс
		/// </summary>
		private void doProcessing(Term workingTerm)
		{
            try
            {
                if (workingTerm.Type == TermType.program)
                {
                    //TO DO вынести этот код отдельно ПОСЛЕ выделения всей функции в отдельный класс
                    for (int i = 0; i < workingTerm.Childs.Count; i++)
                    {
                        doProcessing(workingTerm.Childs[i]);
                    }
#if DEBUG
		        foreach (KeyValuePair<string, string> current in Variables.Storage)
		            VMEnv.Env.log.message(current.Key + "=" + current.Value);
#endif

                }
                if (workingTerm.Type == TermType.variableAssignment)
                {
                    if (Variables.Storage.ContainsKey(workingTerm.Name))
                    {
                        Variables.Storage[workingTerm.Name] = calculateValue(workingTerm);
                    }
                    else
                    {
                        InterpretatorEnveronment.Env.reporter.describeError("Неизвестный идентификатор " + workingTerm.Name, 100100005, workingTerm.Name.StringNumber, "Ошибка присвоения");
                    }
                }
                if (workingTerm.Type == TermType.variableDefinition)
                {
                    Variables.Storage.Add(workingTerm.Name, "");
                }
                if (workingTerm.Type == TermType.variableDefinitionAssignment)
                {
                    Variables.Storage.Add(workingTerm.Name, calculateValue(workingTerm));
                }
                if (workingTerm.Type == TermType.functionCall)
                {
                    functionCaller(workingTerm);
                }
            }
            catch(Exception ex)
            {
                InterpretatorEnveronment.Env.log.message("Выполнение прервано на этапе обработки "+workingTerm.Type.ToString());
                throw ex;
            }
		}
		
		void functionCaller(Term term)
		{
            List<FunctionArgument> args = new List<FunctionArgument>();
            foreach(Term cur in term.Childs)
            {
                FunctionArgument arg=new FunctionArgument();
                arg.argument = calculateValue(cur);
                if(cur.Type==TermType.atomicValue)
                {
                    if (isString(arg.argument))
                        arg.ArgumentTypeEnum=ValueTypeEnum.String;
                    else
                        arg.ArgumentTypeEnum=ValueTypeEnum.Number;
                }else if(cur.Type==TermType.variableValue){
                    arg.ArgumentTypeEnum=ValueTypeEnum.Variable;
                    arg.argument = cur.Name;
                }
                else
                {
                    arg.ArgumentTypeEnum = ValueTypeEnum.Variable;
                }
                args.Add(arg);
            }
            FunctionResult result;
            try
            {
                result = Functions.CallFunction(args, term.Name);
            }catch (WrongArgumentCountException ex)
            {
                Env.reporter.describeError(ex.Message, 100100007, term.Name.StringNumber, "Неверное количество аргументов");
                throw ex;
            }catch(WrongFormatArgumentException ex)
            {
                Env.reporter.describeError(ex.Message, 100100009, term.Name.StringNumber, "Неверный формат аргументов");
                throw ex;
            }catch(UndefinedVariableException ex)
            {
                Env.reporter.describeError(ex.Message, 100100010, term.Name.StringNumber, "Не определена переменная");
                throw ex;
            }
            catch(NotImplementedException ex)
            {
		        InterpretatorEnveronment.Env.reporter.describeError(ex.Message,100100008,term.Name.StringNumber,"Несуществующая функция");
                throw ex;
		    }
            catch(Exception ex)
            {
                throw ex;
            }
		}
		
		String calculateValue(Term term)
		{
		    if (term.Type == TermType.variableAssignment || term.Type == TermType.variableDefinitionAssignment)
		    {
                if (term.Childs.Count == 1)
                    return calculateValue(term.Childs[0]);
                else
                {
                    InterpretatorEnveronment.Env.reporter.describeError("Неверно построено синтаксическое дерево. Обратитесь в поддержку.", 100119999, term.Childs[0].Name.StringNumber, "Внутренняя ошибка");
                    return "";
                }
            }
		    else if (term.Type == TermType.mathAddition)
		    {
		        //TO DO добавить поддержку длинной арифметики
		        return calculateOperation(term, operationAdd);
		    }
		    else if (term.Type == TermType.mathMultiplication)
		    {
		        //TO DO добавить поддержку длинной арифметики
		        return calculateOperation(term, operationMul);
		    }
		    else if (term.Type == TermType.mathSubtraction)
		    {
		        //TO DO добавить поддержку длинной арифметики
		        return calculateOperation(term, operationDec);
		    }
		    else if (term.Type == TermType.mathDivision)
		    {
		        //TO DO добавить поддержку длинной арифметики
		        return calculateOperation(term, operationDiv);
		    }
		    else if (term.Type == TermType.atomicValue)
		    {
		        return term.Value;
		    }
		    else if(term.Type == TermType.variableValue)
		    {
		        if(Variables.Storage.ContainsKey(term.Name))
		        {
		            return Variables.Storage[term.Name];
		        }
		        else
		        {
                    InterpretatorEnveronment.Env.reporter.describeError("Неизвестный идентификатор " + term.Name, 100100005, term.Name.StringNumber, "Неверный идентификатор");
                    return "";
                }
		    }
		    else
		    {
                InterpretatorEnveronment.Env.reporter.describeError("Выполнение оператора не реализовано " + term.Name, 100100005, term.Name.StringNumber, "Выполнение не реализовано");
                return "";
            }
		}
		
        //TO DO Декостелировать тут
		private string calculateOperation(Term term, Func<String, String, String> operation)
		{
		    if (term.Childs.Count == 2)
		    {
		        String leftValue = calculateValue(term.Childs[0]);
		        String rightValue = calculateValue(term.Childs[1]);
		        return operation(leftValue, rightValue);
		    }
		    if(term.Childs.Count == 1&&(term.Type==TermType.mathSubtraction||term.Type==TermType.mathAddition))
		    {
		        String leftValue = "0";
		        String rightValue = calculateValue(term.Childs[0]);
		        return operation(leftValue,rightValue);
		    }
		    if (term.Childs.Count == 1 && (term.Type == TermType.mathDivision))
		    {
		        String leftValue = "1";
		        String rightValue = calculateValue(term.Childs[0]);
		        return operation(leftValue,rightValue);
		    }
            InterpretatorEnveronment.Env.reporter.describeError("Неверно построено синтаксическое дерево. Обратитесь в поддержку.", 100119999, term.Childs[0].Name.StringNumber, "Внутренняя ошибка");
            return "";
        }
		
		private String operationMul(String leftValue, String rightValue)
		{
		
		    float leftFloatValue = 0;
		    float rightFloatValue = 0;
		    int leftIntValue = 0;
		    int rightIntValue = 0;
		    if (variableValueIsString(leftValue) || variableValueIsString(rightValue))
		    {
                InterpretatorEnveronment.Env.reporter.describeError("Выполнение оператора не реализовано для " + leftValue + rightValue+". Нельзя умножать строки", 100100005, -1, "Выполнение не реализовано");
            }
		    if (float.TryParse(leftValue, NumberStyles.Any, new CultureInfo("en-US"), out leftFloatValue) && float.TryParse(rightValue, NumberStyles.Any, new CultureInfo("en-US"), out rightFloatValue))
		    {
		        return (leftFloatValue * rightFloatValue).ToString(new CultureInfo("en-US"));
		    }

            InterpretatorEnveronment.Env.reporter.describeError("Аргументы не подходят для указанной операции для " + leftValue + rightValue, 100100005, -1, "Неверные аргументы");
            return "";
        }
		
		private String operationDiv(String leftValue, String rightValue)
		{
		
		    float leftFloatValue = 0;
		    float rightFloatValue = 0;
		    int leftIntValue = 0;
		    int rightIntValue = 0;
		    if (variableValueIsString(leftValue) || variableValueIsString(rightValue))
		    {
		        //что-то из операндов строки, складываем как строки
		        throw new ArgumentException("Нельзя делить строки");
		    }
		    if (float.TryParse(leftValue, NumberStyles.Any, new CultureInfo("en-US"), out leftFloatValue) && float.TryParse(rightValue, NumberStyles.Any, new CultureInfo("en-US"), out rightFloatValue))
		    {
		        return (leftFloatValue / rightFloatValue).ToString(new CultureInfo("en-US"));
		    }

            InterpretatorEnveronment.Env.reporter.describeError("Аргументы не подходят для указанной операции для " + leftValue + rightValue, 100100005, -1, "Неверные аргументы");
            return "";
        }
		
		private String operationDec(String leftValue, String rightValue)
		{
		
		    float leftFloatValue = 0;
		    float rightFloatValue = 0;
		    int leftIntValue = 0;
		    int rightIntValue = 0;
		    if (variableValueIsString(leftValue) || variableValueIsString(rightValue))
		    {
		        //что-то из операндов строки, складываем как строки
                InterpretatorEnveronment.Env.reporter.describeError("Выполнение оператора не реализовано для " + leftValue + rightValue + ". Нельзя умножать строки", 100100005, -1, "Выполнение не реализовано");
            }
		    if (float.TryParse(leftValue, NumberStyles.Any, new CultureInfo("en-US"), out leftFloatValue) && float.TryParse(rightValue, NumberStyles.Any, new CultureInfo("en-US"), out rightFloatValue))
		    {
		        return (leftFloatValue - rightFloatValue).ToString(new CultureInfo("en-US"));
		    }
            InterpretatorEnveronment.Env.reporter.describeError("Аргументы не подходят для указанной операции для " + leftValue + rightValue, 100100005, -1, "Неверные аргументы");
            return "";
        }
		
		private String operationAdd(String leftValue, String rightValue)
		{
		
		    float leftFloatValue = 0;
		    float rightFloatValue = 0;
		    int leftIntValue = 0;
		    int rightIntValue = 0;
		    if (variableValueIsString(leftValue) || variableValueIsString(rightValue))
		    {
		        //что-то из операндов строки, складываем как строки
		        
		        return buildStringSumm(leftValue, rightValue);//TO DO
		    }
		    if (float.TryParse(leftValue, NumberStyles.Any, new CultureInfo("en-US"), out leftFloatValue) && float.TryParse(rightValue, NumberStyles.Any, new CultureInfo("en-US"), out rightFloatValue))
		    {
		        return (leftFloatValue + rightFloatValue).ToString(new CultureInfo("en-US"));
		    }

            InterpretatorEnveronment.Env.reporter.describeError("Аргументы не подходят для указанной операции для " + leftValue + rightValue, 100100005, -1, "Неверные аргументы");
            return "";
        }
		
		String buildStringSumm(String a,String b)
		{
            if (a.Length == 0)
                return b;
            if (b.Length == 0)
                return a;
		    String result = "";
		    int i;
		    if(a[0]=='\"')
		    {
		        result += a[0];
		        i=1;
		        while(a[i]!='\"')
		        {
		            result += a[i];
		            i++;
		        }
		    }
		    else
		    {
		        result += '\"';
		        i=0;
		        while(i<a.Length)
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
		
		String getStringVariableWithoutQuotes(String s)
		{
            if (s.Length == 0)
                return "";
		    if(s[0]=='\"'&&s[s.Length-1]=='\"')
		    {
		        return s.Remove(0,1).Remove(s.Length - 2, 1);
		    }
		    return s;
		}
	}
}