using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace programmatronCore
{
    class ProgrammatronTables
    {
        public static List<String> ReservedKeywords = new List<String>{ 
                  "функция", 
                  "переменная",
                  "вернуть",
                  "начало",
                  "конец",
                  "если",
                  "то",
                  "цикл"
                };

        public static List<String> ReservedSymbols = new List<String>{ 
                    "/", 
                    "*", 
                    "-", 
                    "+", 
                    ";", 
                    "(", 
                    ")", 
                    ",",
                    "="
                };

        public static List<String> ArithmeticOperations = new List<String>{ 
                    "/", 
                    "*", 
                    "-", 
                    "+", 
        };

        public static String SourceCodeEnd = "☭";
        public static Dictionary<VariableType,String> VariablesTypes = new Dictionary<VariableType,String>
        {
            {VariableType.integer,"целая"},
            {VariableType.fraction,"дробная"},
            {VariableType.characters,"строка"},
            {VariableType.universal,"переменная"}
        };
        public static Dictionary<String, TermType> arithmeticSymbolToTermType = new Dictionary<string, TermType>
        {
            {"*",TermType.mathMultiplication},
            {"/",TermType.mathDivision},
            {"+",TermType.mathAddition},
            {"-",TermType.mathSubtraction}
        };
    }
}