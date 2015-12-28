using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
                    "\"", 
                    "\'", 
                    ",",
                    "="
                };
    }
}