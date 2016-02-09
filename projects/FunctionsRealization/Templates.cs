using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Globalization;

namespace FunctionsList
{

    public class Variables
    {
        public static Dictionary<String, String> Storage=new Dictionary<string,string>();
    }

    public class Functions
    {
        public static List<FunctionTemplate> Storage = new List<FunctionTemplate> { new WriteStrFunc(), new ReadFunc()};

        public static FunctionResult CallFunction(List<FunctionArgument> args,String name)
        {
            FunctionResult res = new FunctionResult(); ;
            try
            {
                foreach(FunctionTemplate func in Storage)
                {
                    if(ArgumentsMatch(func,args)&&name==func.Name)
                    {
                        res = func.execute(args);
                        return res;
                    }
                }
                throw new NotImplementedException("Отсутствует функция с таким именем или нет перегрузки с такими аргументами: "+name);
            }
            catch(Exception ex)
            {
                res = new FunctionResult();
                res.ResultTypeEnum = ValueTypeEnum.None;
                throw ex;
            }
            return res;
        }
        
        private static bool ArgumentsMatch(FunctionTemplate func, List<FunctionArgument> args)
        {
            List<ArgumentPrototypeInfo> mask = func.Info.RequiredArgumentsFormats;
            if(mask.Count==0)
            {
                throw new Exception("Отсутствует определение аргументов функции. Внутренняя ошибка");
            }
            int j = 0;
            int i = 0;
            if(mask.Count==0&&args.Count==0)
            {
                return true;
            }
            else if(mask.Count==0&&args.Count!=0)
            {
                return false;
            }
            while(i<mask.Count)
            {
                if(!ArgumentMatch(mask[j],args[i]))
                {
                    return false;
                }
                if(!mask[j].InfiniteArgumentsCount)
                {
                    j++;
                }
                i++;
            }
            return true;
        }

        private static bool ArgumentMatch(ArgumentPrototypeInfo PrototypeInfo,FunctionArgument arg)
        {
            if(PrototypeInfo.CanBeNumber&&argumentIsNumber(arg))
            {
                return true;
            }
            else if(PrototypeInfo.CanBeString&&argumentIsString(arg))
            {
                return true;
            }
            else if(PrototypeInfo.CanBeVariable&&argumentIsVariable(arg))
            {
                return true;
            }
            return false;
        }

        private static bool argumentIsVariable(FunctionArgument arg)
        {
            return arg.ArgumentTypeEnum == ValueTypeEnum.Variable;
        }

        private static bool argumentIsString(FunctionArgument arg)
        {
            return arg.ArgumentTypeEnum == ValueTypeEnum.String;
        }

        private static bool argumentIsNumber(FunctionArgument arg)
        {
            return arg.ArgumentTypeEnum == ValueTypeEnum.Number;
        }
    }

    public struct ArgumentPrototypeInfo
    {
        public String Name;
        public bool InfiniteArgumentsCount;
        public bool CanBeNumber;
        public bool CanBeString;
        public bool CanBeVariable;
        public bool CanNotBe;
        public ArgumentPrototypeInfo(String name,bool infiniteCount,bool canBeNumber,bool canBeString,bool canBeVariable,bool canNotBe)
        {
            Name = name;
            InfiniteArgumentsCount = infiniteCount;
            CanBeString = canBeString;
            CanBeNumber = canBeNumber; ;
            CanBeVariable = canBeVariable;
            CanNotBe = canNotBe;
        }
    }

    public struct FunctionResult
    {
        public ValueTypeEnum ResultTypeEnum;
        public String Result;
    }

    /// <summary>
    /// Main function template
    /// </summary>
    public abstract class FunctionTemplate
    {
        public String Name
        {
            get;
            set;
        }

        /// <summary>
        /// Input arguments count. -1 unlimited
        /// </summary>
        public int ArgsCount
        {
             get{
                if(Info.RequiredArgumentsFormats.Exists(info=>info.InfiniteArgumentsCount==true))
                {
                    return -1;
                }
                else
                {
                    return Info.RequiredArgumentsFormats.Count;
                }
            }
        }

        protected FunctionTemplate()
        {

        }

        /// <summary>
        /// Реализовать в потомке. Здесь должны проводиться все проверки, выполнение и обработка. Можно использовать исключения, они будут 
        /// обработаны как ошибки в основном коде, код ошибки сгенерируется автоматически, содержание же необхождимо указать самостоятельно.
        /// </summary>
        /// <param name="args">Список аргументов, упакованные в структуры</param>
        /// <returns>Результат выполнения функции. Может быть как реальным значением, так и именем переменной, либо ничем.</returns>
        public abstract FunctionResult execute(List<FunctionArgument> args);

        public FunctionHelpInfo Info;

        public FunctionResult Result;

        public static bool isString(String value)
        {
            return value[0] == '\"' && value[value.Length - 1] == '\"';
        }

        public static bool isNumber(String str)
        {
            int useless = 0;
            float uselss = 0.0f;
            return int.TryParse(str, NumberStyles.Any, new CultureInfo("en-US"), out useless) || float.TryParse(str, NumberStyles.Any, new CultureInfo("en-US"), out uselss);
        }

        public static bool isIdentifier(String lexem)
        {
            return lexem[0] == '_' || char.IsLetter(lexem, 0);
        }

        public static String getStringVariableWithoutQuotes(String s)
        {
            if (s.Length == 0)
                return "";
            if (s[0] == '\"' && s[s.Length - 1] == '\"')
            {
                return s.Remove(0, 1).Remove(s.Length - 2, 1);
            }
            return s;
        }
    }

    public struct FunctionHelpInfo
    {
        public string Group;
        public string Args;
        public string Help;

        public List<ArgumentPrototypeInfo> RequiredArgumentsFormats;

        public ValueTypeEnum ReturnValueType;

        public FunctionHelpInfo(String group, ValueTypeEnum returnValueType, String args, String help, List<ArgumentPrototypeInfo> requiredArgumentsFormats)
        {
            Group = group;
            Help = help;
            Args = args;
            RequiredArgumentsFormats = requiredArgumentsFormats;
            ReturnValueType = returnValueType;
        }
    }

    public struct FunctionArgument
    {
        public ValueTypeEnum ArgumentTypeEnum;
        public String argument;
    }

    /// <summary>
    /// Перечисляет варианты значений, приходящих с запросами на выполнение функций из ядра, а также возвращаемых после выполнения
    /// </summary>
    public enum ValueTypeEnum
    {
        String,
        Number,
        Variable,//variable name
        None,//don't return a value
    }
}