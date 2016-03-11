using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.IO.Pipes;
using programmatronCore;
using System.Windows;
using System.IO;

namespace InterpretatorEnveronment
{
    public class Env
    {
        public static Log log;
        public static Reporter reporter;
    }
    public class Log
    {
        String logFilePath;
        NamedPipeClientStream logPipe;
        StreamWriter logFileStream;
        String currentTitle;
        String currentText;

        public Log()
        {
            initialize();
            if (Parameters.InterParameters.isDebug)
                tryConnectToIDE();
        }

        void initialize()
        {
            //генерирует путь в виде /Logs/01.08.2015 17-20.log
            prepareDirectory();
            logFilePath = generateLogFilePath();
            logFileStream = new StreamWriter(logFilePath,false);
            logPipe = new NamedPipeClientStream("programmatronDebugPipe");
        }

        void prepareDirectory()
        {
            if (!Directory.Exists("Logs"))
                Directory.CreateDirectory("Logs");
        }

        String generateLogFilePath()
        {
            return "Logs" + Path.DirectorySeparatorChar + DateTime.Now.ToShortDateString() + " " + DateTime.Now.Hour + "-" + DateTime.Now.Second + ".log";
        }

        void tryConnectToIDE()
        {
            try
            {
                logPipe.Connect(500);
            }
            catch(TimeoutException ex)
            {
                Console.WriteLine("Не удалось настроиться для работы в отладочном режиме c IDE. \nИнформация в буффере обмена.");
                Clipboard.SetText(ex.ToString());
            }
        }

        /// <summary>
        /// Рассылает сообщение по всем адресатам, согласно настройкам, заранее установленным в среде.
        /// </summary>
        /// <param name="title">Заголовок</param>
        /// <param name="text">Содержание</param>
        public void message(String title, String text)
        {
            String formatedMessage = prepareMessage(title,text);
            sendToLog();
            sendToIDEFormatedMessage(formatedMessage);
            sendToConsole();
        }

        /// <summary>
        /// Рассылает сообщение по всем адресатам, согласно настройкам, заранее установленным в среде.
        /// </summary>
        /// <param name="text">Содержание</param>
        public void message(String text)
        {
            message("", text);
        }

        private String prepareMessage(String title,String text)
        {
            currentText = text;
            currentTitle = title;
            return title + Constants.spacerBetweenTitleAndTextInMessage + text;
        }

        private void sendToLog()
        {
            if(Parameters.InterParameters.isSaveReports)
            {
                logFileStream.Write(DateTime.Now.ToString() + " ");
                if (currentTitle.Length != 0)
                    logFileStream.WriteLine(currentTitle);
                else
                    logFileStream.Write("\n");
                logFileStream.WriteLine(currentText);
                logFileStream.Flush();
            }
        }

        private void sendToIDEFormatedMessage(String message)
        {
            if(Parameters.InterParameters.isDebug && logPipe.IsConnected)
            {
                using(StreamWriter writer = new StreamWriter(logPipe))
                {
                    writer.WriteLine(message);
                    writer.Flush();
                }
            }
        }

        private void sendToConsole()
        {
            Console.Write(DateTime.Now.ToString() + " ");
            if (currentTitle.Length != 0)
                Console.WriteLine(currentTitle);
            else
                Console.Write("\n");
            Console.WriteLine(currentText);
        }
    }
}