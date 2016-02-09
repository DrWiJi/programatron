using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace FunctionsList
{
    public struct Functions
    {

    }

    public struct FunctionResult
    {
        public TypeEnum ResultTypeEnum;
        public String Result;
    }

    public abstract class FunctionTemplate
    {
        public String Name 
        {
            public get;
            protected set;
        }

        /// <summary>
        /// Input arguments count. -1 unlimited
        /// </summary>
        public int ArgsCount 
        {
            public get;
            protected set;
        }

        protected FunctionTemplate()
        {

        }

        public abstract FunctionResult execute(List<FunctionArgument> args);

        public FunctionHelpInfo Info;

        public FunctionResult Result;
    }

    public struct FunctionHelpInfo
    {
        public string Group
        {
            public get;
            private set;
        }
        public string Args
        {
            public get;
            private set;
        }
        public string Help
        {
            public get;
            private set;
        }

        public FunctionHelpInfo(String group,String args,String help)
        {
            Group = group;
            Help=help;
            Args=args;
        }
    }

    public struct FunctionArgument
    {
        public TypeEnum ArgumentTypeEnum;
        public String argument;
    }

    public enum TypeEnum
    {
        RealValue,//String or number
        Variable,//variable name
        None//don't return a value
    }
    
    class WriteStrFunc:FunctionTemplate
    {
        public WriteStrFunc():base()
        {
            Name = "выводСтрокой";
            ArgsCount = -1;
            Info = new FunctionHelpInfo("Ввод/вывод", "arg1,arg2,arg3,...", "Выводит в консоль все аргументы в текстовом представлении. Каждый аргумент выводится с новой строки.");
            Result = new FunctionResult();
            Result.ResultTypeEnum = TypeEnum.None;
        }

        public override FunctionResult execute(List<FunctionArgument> args)
        {
            foreach(FunctionArgument arg in args)
            {
                
            }
            return Result;
        }
    }
}