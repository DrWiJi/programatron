using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace programmatronCore
{
    public class LexemAnalyzer
    {
        String sourceCode;
        List<String> lexems;
        int i;

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
                while(canTakeLexem())
                {
                    takeLexem();
                    while(!endOfSourceCode() && isNeedSkipSymbol() )
                    {
                        i++;
                    }
                }
            }
            catch(Exception ex)
            {
                
            }
            
            return this.lexems;
        }

        void checkArgs()
        {
            if(lexems==null||sourceCode==null||sourceCode.Length==0)
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

        void takeLexem()
        {
            String currentLexem = "";
            do
            {
                currentLexem += sourceCode[i];
                if(endOfSourceCode())
                {
                    break;
                }
                if(symbolIsReserved())
                {
                    i++;
                    break;
                }
                i++;
            } while (symbolIsNotEndOfLexem());

            if(currentLexem.Length==0)
            {
                throw new ArgumentException("Сгенерирована пустая лексема. Позиция i = "+i+". Существующих лексем - " + lexems.Count+".");
            }
            lexems.Add(currentLexem);
        }

        bool endOfSourceCode()
        {
            return i >= sourceCode.Length;
        }

        bool symbolIsEndOfLexem()
        {
            return ProgrammatronTables.ReservedSymbols.Any(ch => ch == sourceCode[i].ToString()) || sourceCode[i] == ' ' || sourceCode[i] == '\0';
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

    public class Term
    {
        Term parent;
        TermType type;
        List<Term> childs;
        String value;
        VariableType variableType;

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

        public Term (Term parent, TermType type, String value)
        {
            initialize();
            this.parent = parent;
            this.type = type;
            this.value = value;
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
        }

        public String Value
        {
            get { return value; }
        }

        public void addChild(Term child)
        {
            childs.Add(child);
        }

        public Term getChildAt(int index)
        {
            return childs[index];
        }

        public VariableType VariableType
        {
            get { return variableType; }
        }

        public TermType Type
        {
            get { return type; }
        }
    }

    public enum TermType
    {
        variableDefinition,
        variableAssignment,
        functionCall,
        functionArgumentsList,
        functionArgument,
        program,
        mathAddition,
        mathSubtraction,
        mathDivision,
        mathMultiplication,
        value,
        identifier,
        dataType,
        block,
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
            lexems = null;
        }

        public void generateTree()
        {

        }

        public void generateTreeFromLexems(List<String> otherLexems)
        {

        }
    }

}
