using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using programmatronCore;
using testing;
using testingDLL;
using InterpretatorEnvironment;
using System.Windows;

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
            Env.log.message("Запуск...");
            if (Parameters.InterParameters.executableFilePath!=null&&File.Exists(Parameters.InterParameters.executableFilePath))
            {
                executeCode(getSourceCode());
            }
            else
            {
                Env.log.message(Parameters.InterParameters.executableFilePath);
                Env.log.message("!!! Указанный файл не существует или вовсе не был указан. !!!");
                Env.log.message("Попытка открытия стандартного расположения...");
                if(File.Exists(Constants.defaultExecuteFilePath))
                {
                    Parameters.InterParameters.executableFilePath = Constants.defaultExecuteFilePath;
                    executeCode(getSourceCode());
                }
                else
                {
                    Env.log.message("Открытие стандартного расположения не удалось. \nОткрыть и запустить файл с кодом на Программатроне можно просто перетаскивая их на этот исполняемый файл или\nОткрыть с помощью>>Другое");
                    String en = Console.ReadLine();
                }
            }
#endif
        }

        static void executeCode(String code)
        {
            if(Parameters.InterParameters.isQuickLightRun)
                Console.Clear();
            LexemAnalyzer sa = new LexemAnalyzer(code);
            List<Lexem> list = sa.analize();
            SyntaxTreeGenerator gen = new SyntaxTreeGenerator(list);
            gen.generateTree();
            gen.doCode();
            Env.log.message("Выполнение завершено, нажмите любую клавишу...");
            Console.ReadKey();
        }

        static String getSourceCode()
        {
            String result = "";
            using (StreamReader reader = new StreamReader(Parameters.InterParameters.executableFilePath,Encoding.Default))
            {
                result = reader.ReadToEnd();
            }
            return result;
        }

        static void applicationInitialize(string[] args)
        {
            fillInterpretatorParameters(args);
            Env.log = new Log();
            Env.reporter = new Reporter();
            Env.Debug = new DebugMode();
        }

#if DEBUG
        public static void fillVirtualMachineParameters(string[] args)
#else
        static void fillInterpretatorParameters(string[] args)
#endif
        {
            //TO DO пересмотреть систему флагов
            Parameters.InterParameters.executableFilePath = getExecutableFilePathFromArgsOrDefault(args);
            Parameters.InterParameters.isDebug = getAvailabilityNormalFlagFromArgs("-debug",args);
            Parameters.InterParameters.isIgnoreWarnings = getAvailabilityNormalFlagFromArgs("-ignoreWarnings", args);
            Parameters.InterParameters.isSaveReports = getAvailabilityNormalFlagFromArgs("-saveReports", args);
            Parameters.InterParameters.isQuickLightRun = getAvailabilityNormalFlagFromArgs("-quickLightRun",args);
        }

        static String getExecutableFilePathFromArgsOrDefault(string[] args)
        {
            foreach(String cur in args)
            {
                if(File.Exists(cur)&&Path.GetExtension(cur)==".pgt")
                {
                    return cur;
                }
            }
            return Constants.defaultExecuteFilePath;
        }

        static bool getAvailabilityNormalFlagFromArgs(String flagName, string[] args)
        {
            return Array.Exists<string>(args, s => s == flagName);
        }
    }
}
