using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using programmatronCore;
using testing;
using testingDLL;
using VMenvironment;

namespace languageVM
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
#if DEBUG
            Log log = new Log();
            VMenvironment.Environment.log = log;
            ProgramTester test1 = new ProgramTester();
            test1.startProgramTests();
            Console.ReadKey();
            iniParserTester test2 = new iniParserTester();
            test2.runAllTests();
            Console.ReadKey();
            Parameters.virtualMachineParameters.isDebug = true;
            Parameters.virtualMachineParameters.isSaveReports = true;
            log.message("Заголовок сообщение","Первое тестовое сообщение. 123!;%");
            log.message("Система лога корректно работает");
            Console.ReadKey();
#else
            applicationInitialize(args);
            Console.WriteLine("ЗАПУСК");
            if (Parameters.virtualMachineParameters.executableFilePath!=null&&File.Exists(Parameters.virtualMachineParameters.executableFilePath))
            {
                executeCode(getSourceCode());
            }
            else
            {
                Console.WriteLine("!!! Указанный файл не существует или вовсе не был указан. !!!");
                Console.WriteLine("Попытка открытия стандартного расположения...");
                if(File.Exists(Constants.defaultExecuteFilePath))
                {
                    Parameters.virtualMachineParameters.executableFilePath = Constants.defaultExecuteFilePath;
                    executeCode(getSourceCode());
                }
                else
                {
                    Console.WriteLine("Открытие стандартного расположения не удалось. \nОткрыть и запустить файл с кодом на Программатроне можно просто перетаскивая их на этот исполняемый файл или\nОткрыть с помощью>>Другое");
                    String en = Console.ReadLine();
                }
            }
#endif
        }

        static void executeCode(String code)
        {
            LexemAnalyzer sa = new LexemAnalyzer(code);
            List<String> list = sa.analize();
            SyntaxTreeGenerator gen = new SyntaxTreeGenerator(list);
            gen.generateTree();
            gen.doCode();
            Console.WriteLine("Выполнение завершено, нажмите любую клавишу...");
            Console.ReadKey();
        }

        static String getSourceCode()
        {
            String result = "";
            using (StreamReader reader = new StreamReader(Parameters.virtualMachineParameters.executableFilePath,Encoding.Default))
            {
                result = reader.ReadToEnd();
            }
            return result;
        }

        static void applicationInitialize(string[] args)
        {
            fillVirtualMachineParameters(args);
            VMenvironment.Environment.log = new Log();
            VMenvironment.Environment.reporter = new Reporter();
        }

#if DEBUG
        public static void fillVirtualMachineParameters(string[] args)
#else
        static void fillVirtualMachineParameters(string[] args)
#endif
        {
            //пересмотреть систему флагов на ПК платформе
            Parameters.virtualMachineParameters.executableFilePath = getExecutableFilePathFromArgsOrDefault(args);
            Parameters.virtualMachineParameters.isDebug = getAvailabilityNormalFlagFromArgs("-debug",args);
            Parameters.virtualMachineParameters.isIgnoreWarnings = getAvailabilityNormalFlagFromArgs("-ignoreWarnings", args);
            Parameters.virtualMachineParameters.isSaveReports = getAvailabilityNormalFlagFromArgs("-saveReports", args);
        }

        static String getExecutableFilePathFromArgsOrDefault(string[] args)
        {
            if(args.Length>=1&&checkArgumentLikeFilePath(args[0]))
            {
                return args[0];
            }
            else
            {
                return Constants.defaultExecuteFilePath;
            }
        }

        static bool checkArgumentLikeFilePath(String argument)
        {
            return File.Exists(argument);
        }

        static bool getAvailabilityNormalFlagFromArgs(String flagName, string[] args)
        {
            return Array.Exists<string>(args, s => s == flagName);
        }
    }
}
