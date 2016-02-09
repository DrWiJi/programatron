using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Globalization;

namespace FunctionsList
{
    public class WriteStrFunc : FunctionTemplate
    {
        public WriteStrFunc()
            : base()
        {
            Name = "выводСтрокой";
            Info = new FunctionHelpInfo("Ввод/вывод", ValueTypeEnum.None, "arg1,arg2,arg3,...", "Выводит в консоль все аргументы в текстовом представлении. Каждый аргумент выводится с новой строки.", 
                new List<ArgumentPrototypeInfo> { new ArgumentPrototypeInfo("Any",true,true,true,true,false)});
            Result = new FunctionResult();
            Result.ResultTypeEnum = ValueTypeEnum.None;
        }

        public override FunctionResult execute(List<FunctionArgument> args)
        {
            if (args.Count == 0)
                throw new WrongArgumentCountException("Функция не принимает нуль аргументов");
            foreach (FunctionArgument arg in args)
            {
                if(isString(arg.argument))
                    Console.WriteLine(getStringVariableWithoutQuotes(arg.argument));
                else if(isNumber(arg.argument))
                    Console.WriteLine(arg.argument);
                else if(!isIdentifier(arg.argument))
                    throw new WrongFormatArgumentException("Неверный формат входного аргумента " + arg.argument);
                else if (Variables.Storage.ContainsKey(arg.argument))
                {
                    if (isString(Variables.Storage[arg.argument]))
                        Console.WriteLine(getStringVariableWithoutQuotes(Variables.Storage[arg.argument]));
                    else
                        Console.WriteLine(Variables.Storage[arg.argument]);
                }
                else 
                    throw new UndefinedVariableException("Необявленная переменная " + arg.argument + ", либо не существует в данной области видимости");
            }
            return Result;
        }


    }

    public class ReadFunc:FunctionTemplate
    {
        public ReadFunc():base()
        {
            Name = "ввод";
            Info = new FunctionHelpInfo("Ввод/вывод", ValueTypeEnum.None, "arg0,arg1,arg2,arg3,... только переменные", "Ввод с клавиатуры значений и присвоение этих значений переменным; Всегда в порядке слева направо.",
                new List<ArgumentPrototypeInfo> { new ArgumentPrototypeInfo("", true, false, false, true, false) });
            Result = new FunctionResult();
            Result.ResultTypeEnum = ValueTypeEnum.None;
        }

        public override FunctionResult execute(List<FunctionArgument> args)
        {
            if (args.Count == 0)
                throw new WrongArgumentCountException("Функция ввод() не принимает нуль аргументов");
            foreach(FunctionArgument arg in args)
            {
                if (Variables.Storage.ContainsKey(arg.argument))
                    Variables.Storage[arg.argument] = Console.ReadLine();
                else
                    throw new UndefinedVariableException("Не определена в текущей области видимости переменная " + arg.argument);
            }
            return Result;
        }
    }
}