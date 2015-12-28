/*
 * Ядро интерпретатора программатрона
 * Автор - Алексеев Сергей aka DrWiJi (Tontu) 2serales96@mail.ru
 * Волгоград, 2015, все права защищены.
 * 
 * При поддержке ВолгГТУ
 * 
 * 
 * Наиважнейшие модули, обеспечивающие работу программатрона на машине: лексический анализатор,
 * синтаксический анализатор, сам интерпретатор и вся инфраструктура к ним.
 * 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Threading.Tasks;
using mobosiSema;
using Windows.UI.Popups;
namespace programmatronCore
{
    public class LexemAnalyzer
    {
        String sourceCode;
        List<String> lexems;
        int i;
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
            lexems = new List<String>();
            i = 0;
        }

        public List<String> analizeFrom(String otherCode)
        {
            this.sourceCode = otherCode;
            return analize();
        }

        public List<String> analize()
        {
            try
            {
                checkArgs();
                analizeReady();
                while (canTakeLexem())
                {
                    while(isNeedSkipSymbol())
                    {
                        i++;
                    }
                    takeLexem();
                    while (!endOfSourceCode() && isNeedSkipSymbol())
                    {
                        i++;
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex is ArgumentException)
                {
                    string s = "Программа пуста. Напишите какой-нибудь код, а только затем запускайте.";
                    Log.printError(s);
                }
                    //VMenvironment.Environment.reporter.describeError("Невозможно запустить синтаксический анализ. Вероятно, исходный код пуст. Напишите код, а затем запускайте.", 10000001, 0, "");
            }
            finally
            {
                lexems.Add(ProgrammatronTables.SourceCodeEnd);
            }
            return this.lexems;

        }

        void checkArgs()
        {
            if (lexems == null || sourceCode == null || sourceCode.Length == 0)
            {
                Log.printError("Нулевой(ые) аргумент(ы) или пустой исходный код. Работа невозможна.");
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
                Log.printError("Сгенерирована пустая лексема. Позиция i = " + i + ". Существующих лексем - " + lexems.Count + ".");
            }
            lexems.Add(currentLexem);
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
            return (ProgrammatronTables.ReservedSymbols.Any(ch => ch == sourceCode[i].ToString()) || sourceCode[i] == ' ' || sourceCode[i] == '\0');
        }

        bool symbolIsReserved()
        {
            return ProgrammatronTables.ReservedSymbols.Any(ch => ch == sourceCode[i].ToString());
        }

        bool symbolIsNotEndOfLexem()
        {
            return !symbolIsEndOfLexem();
        }

        List<String> Lexems
        {
            get { return lexems; }
        }
    }

    public static class Log
    {
        public static async void printError(string s)
        {
            MessageDialog dlg = new MessageDialog(s);
            await dlg.ShowAsync();
        }
    }

    public class Term
    {
        Term parent;
        TermType type;
        List<Term> childs;
        String value;
        String name;
        bool valueIsExpression;
        bool valueIsAssigned;
        VariableType variableType;

        public bool ValueIsExpression
        {
            get { return valueIsExpression; }
            set { valueIsExpression = value; }
        }

        public String Name
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

        public Term(Term parent, TermType type, String value)
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
            get { return valueIsAssigned; }
        }

        public String Value
        {
            get { return value; }
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
            get { return childs[index]; }
        }

        public VariableType VariableType
        {
            get { return variableType; }
        }

        public TermType Type
        {
            get { return type; }
            set { type = value; }
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
        //упростить существенно, оставить только нетерминалы
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

        List<String> lexems;

        Dictionary<String, String> variablesValues;//TO DO переместить в Term

        Term currentHead;

        int lexemIndex = 0;

        public SyntaxTreeGenerator(List<String> lexems)
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
            lexems = new List<String>();
            variablesValues = new Dictionary<String, String>();
        }

        public void generateTreeFromLexems(List<String> otherLexems)
        {
            this.lexems = otherLexems;
            generateTree();
        }

        public void generateTree()
        {
            //clear();
            headOfSyntaxTree = new Term(TermType.program);
            currentHead = headOfSyntaxTree;
            variablesValues = new Dictionary<String, String>();
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
                        Log.printError("Неверный тип переменной");
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
                        Log.printError("Неверный тип переменной");
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
                    Log.printError("Ошибка интерпретируемого кода. Невозможно определить тип выражения.");
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

        private String getFunctionNameFromCall(int startIndex)
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

        private String getVariableNameFromAssignment(int startLexemIndex)
        {
            return lexems[startLexemIndex];
        }

        private String getVariableNameFromDefinition(int startLexemIndex)
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

            //TO DO отрефакторить и отладить

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

        private bool valueIsNumber(int index)
        {
            bool result = isNumber(lexems[index]);
            return result;
        }

        bool isNumber(String str)
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
            return lexems[index][0] == '\"' && lexems[index][lexems[index].Length - 1] == '\"';
        }
        private bool variableValueIsString(String value)
        {
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
            return ProgrammatronTables.ArithmeticOperations.Any(s => s == lexems[index]);
        }

        //TO DO вынести код ниже вообще в отдельный класс выполнителя программы
        public async Task<int> doCode()
        {
            doProcessing(headOfSyntaxTree);
            return 0;
        }

        /// <summary>
        /// Функция запускается во время выполнения кода. TO DO выделить функции выполнения кода в отдельный класс
        /// </summary>
        private async void doProcessing(Term workingTerm)
        {
            if (workingTerm.Type == TermType.program)
            {
                //TO DO вынести этот код отдельно ПОСЛЕ выделения всей функции в отдельный класс
                for (int i = 0; i < workingTerm.Childs.Count; i++)
                {
                    doProcessing(workingTerm.Childs[i]);
                }

            }
            if (workingTerm.Type == TermType.variableAssignment)
            {
                if (variablesValues.ContainsKey(workingTerm.Name))
                {
                    variablesValues[workingTerm.Name] = calculateValue(workingTerm);
                }
                else
                {
                    //VMenvironment.Environment.log.message("Проиводится присвоение несуществующей переменной " + workingTerm.Name);
                    Log.printError("Присвоение несуществующих переменных");
                }
            }
            if (workingTerm.Type == TermType.variableDefinition)
            {
                variablesValues.Add(workingTerm.Name, "");
            }
            if (workingTerm.Type == TermType.variableDefinitionAssignment)
            {
                variablesValues.Add(workingTerm.Name, calculateValue(workingTerm));
            }
            if(workingTerm.Type == TermType.functionCall)
            {
                await functionCaller(workingTerm);
            }
        }

        bool isQ = true;
        async Task functionCaller(Term term)
        {
            //TO DO !!! ГОВНОКОД убрать
            
            if(term.Name == "вывод")
            {
                while(!isQ)
                { await Task.Delay(50); }
                foreach(Term cur in term.Childs)
                {
                    
                    string s =await InputOutput.put(getStringVariableWithoutQuotes(calculateValue(cur)));
                }
            }
            else if(term.Name == "ввод")
            {
                if(term.Childs.Count==1)
                {
                    if(term.Childs[0].Type == TermType.variableValue)
                    {
                        if(variablesValues.ContainsKey(term.Childs[0].Name))
                        {
                            isQ = false;
                            String str = await InputOutput.get();
                            isQ = true;
                            if(isNumber(str))
                               variablesValues[term.Childs[0].Name] = str;
                            else
                                variablesValues[term.Childs[0].Name] = "\""+str+"\"";
                            
                        }
                    }
                    else
                    {
                        Log.printError("Нельзя присваивать значения ничему, кроме переменных");
                    }
                }
                else
                {
                    Log.printError("Неверное количество аргументов");
                }
            }
            else if (term.Name == "выводСтрокой")
            {
                while (!isQ)
                {await Task.Delay(50);} 
                foreach (Term cur in term.Childs)
                {
                    string s = await InputOutput.put(getStringVariableWithoutQuotes(calculateValue(cur))+"\n");
                }
            }
            else
            {
                Log.printError("Отсутствует такая функция в области видимости");
            }
        }

        //TO DO добавить обработку ошибок во время построения дерева плюс расширить  лексемы - пусть лексема будет не только 
        //строкой
        //но и пусть хранит строку и все необходимые атрибуты, чтобы потом можно было легко отлавливать ошибки в исполняемом коде
        //и корректно их выводить
        String calculateValue(Term term)
        {
            if (term.Type == TermType.variableAssignment || term.Type == TermType.variableDefinitionAssignment)
            {
                if (term.Childs.Count == 1)
                    return calculateValue(term.Childs[0]);
                else
                {
                    Log.printError("Ошибка в построении синтаксического дерева. В присвоении неверное количество ветвлений.");
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
                if(variablesValues.ContainsKey(term.Name))
                {
                    return variablesValues[term.Name];
                }
                else
                {
                    Log.printError("Неизвестный идентификатор.");
                    return "";
                }
            }
            else
            {
                Log.printError("Не реализовано выполнение дерева по операции.");
                return "";
            }
        }

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
            Log.printError("Неверно построено синтаксическое дерево.");
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
                Log.printError("Нельзя умножать строки");
            }
            if (float.TryParse(leftValue, NumberStyles.Any, new CultureInfo("en-US"), out leftFloatValue) && float.TryParse(rightValue, NumberStyles.Any, new CultureInfo("en-US"), out rightFloatValue))
            {
                return (leftFloatValue * rightFloatValue).ToString(new CultureInfo("en-US"));
            }

            Log.printError("Недопустимая операция. Аргументы не подходят.");
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
                Log.printError("Нельзя делить строки");
            }
            if (float.TryParse(leftValue, NumberStyles.Any, new CultureInfo("en-US"), out leftFloatValue) && float.TryParse(rightValue, NumberStyles.Any, new CultureInfo("en-US"), out rightFloatValue))
            {
                return (leftFloatValue / rightFloatValue).ToString(new CultureInfo("en-US"));
            }

            Log.printError("Недопустимая операция. Аргументы не подходят.");
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
                Log.printError("Нельзя вычитать строки");
            }
            if (float.TryParse(leftValue, NumberStyles.Any, new CultureInfo("en-US"), out leftFloatValue) && float.TryParse(rightValue, NumberStyles.Any, new CultureInfo("en-US"), out rightFloatValue))
            {
                return (leftFloatValue - rightFloatValue).ToString(new CultureInfo("en-US"));
            }
            Log.printError("Недопустимая операция. Аргументы не подходят.");
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
            Log.printError("Недопустимая операция. Аргументы не подходят.");
            return "";
            
        }
        String buildStringSumm(String a,String b)
        {
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

            if(s[0]=='\"'&&s[s.Length-1]=='\"')
            {
                return s.Remove(0,1).Remove(s.Length - 2, 1);
            }
            return s;
        }
    }
}