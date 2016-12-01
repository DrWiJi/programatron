//..begin "File Description"
/*--------------------------------------------------------------------------------*
   Filename:  LanguageMachine.cs
   Tool:      objectiF, CSharpSSvr V7.2.24
 *--------------------------------------------------------------------------------*/
//..end "File Description"

using System;
using System.Collections.Generic;
using InterpretatorEnvironment;


namespace programmatronCore
{	
	public class Constants
	{
		public static String defaultExecuteFilePath = "program.pgt";
		public static String spacerBetweenTitleAndTextInMessage = "\\\\//";
	}
	
	public class Parameters
	{
		public static InterpretatorParameters InterParameters;
		public static String PRODUCTFULLNAME = "Programmatron Interpretator(C) Alexeev Sergey, Volgograd 2015-2016";
		public static String PRODUCTVERSION = "Alpha 0.3.0";
	}
	
	public struct InterpretatorParameters
	{
		public String executableFilePath;
		public bool isDebug;
		public bool isIgnoreWarnings;
		public bool isSaveReports;
        public bool isQuickLightRun;
		
		public InterpretatorParameters(string executableFilePath, bool debugMode, bool ignoreWarnings, bool saveReports,bool quickLightRun)
		{
		    this.executableFilePath = executableFilePath;
		    this.isSaveReports = saveReports;
		    this.isIgnoreWarnings = ignoreWarnings;
		    this.isDebug = debugMode;
            this.isQuickLightRun = quickLightRun;
		}
		
		public override String ToString()
		{
		    return executableFilePath + " Debug: " + isDebug.ToString() + " Ignore Warnings: " + isIgnoreWarnings.ToString() + " Save Reports: " + isSaveReports.ToString()+" Quick run: " + isQuickLightRun;
		}
		
		public static bool operator==(InterpretatorParameters a, InterpretatorParameters b)
		{
		    return a.isDebug == b.isDebug && a.isIgnoreWarnings == b.isIgnoreWarnings && a.isSaveReports == b.isSaveReports && a.executableFilePath == b.executableFilePath;
		}
		
		public static bool operator!=(InterpretatorParameters a, InterpretatorParameters b)
		{
		    return !(a==b);
		}
		
		public override bool Equals(object o)
		{
		    return true;
		}
		
		public override int GetHashCode()
		{
		    return executableFilePath.GetHashCode();
		}
	}
	
	public abstract class SiruationInSourceCodeReport
	{
		protected String description;
		protected String stringWithSituation;
		protected int situationCode;
		protected int numberOfStringWithSituation;
		
		public SiruationInSourceCodeReport(String description, int situationCode,int numberOfStringWithSituation, String stringWithSituation)
		{
		    this.description = description;
		    this.situationCode = situationCode;
		    this.numberOfStringWithSituation = numberOfStringWithSituation;
		    this.stringWithSituation = stringWithSituation;
		}
		
		public int code
		{
			get
			{
				return situationCode; 
			}
		}
	}
	
	/// <summary>
	/// Класс описания сообщения о ситуации, требующей внимания в коде. Генерируется методом-фермой.
	/// </summary>
	public class WarningReport
	:	SiruationInSourceCodeReport
	{
		public static WarningReport create(String description,int code,int numberOfString, String stringWithWarning)
		{
		    WarningReport report = new WarningReport(description, code, numberOfString, stringWithWarning);
		    InterpretatorEnvironment.Env.log.message("Warning", report.ToString());
		    return report;
		}
		
		private WarningReport(String description,int code,int numberOfString, String stringWithWarning):base(description,code,numberOfString,stringWithWarning)
		{
		    
		}
		
		public override String ToString()
		{
		    return "Требует внимание ситуация номер " + situationCode.ToString() + " в строке " + numberOfStringWithSituation.ToString() + ". " + description;
		}
	}
	
	/// <summary>
	/// Класс описания ошибки в коде. Объект создается методом-фермой.
	/// </summary>
	public class ErrorReport
	:	SiruationInSourceCodeReport
	{
		public static ErrorReport create(String description,int code,int numberOfString, String stringWithError)
		{
		    ErrorReport report = new ErrorReport(description, code, numberOfString, stringWithError);
		    InterpretatorEnvironment.Env.log.message("Error", report.ToString());
		    return report;
		}
		
		private ErrorReport(String description,int code,int numberOfString, String stringWithError):base(description,code,numberOfString,stringWithError)
		{
		    
		}
		
		public override String ToString()
		{
		    return "Ошибка с кодом " + situationCode.ToString() + " в строке " + numberOfStringWithSituation.ToString() + ": " + description;
		}
		
		/// <summary>
		/// Возвращает расширенное описание ошибки, куда входит: код ошибки, номер строки, сама строка, описание.
		/// </summary>
		/// <returns>Расширенное описание ошибки</returns>
		public String getExtendDescription()
		{
		    return "Ошибка " + situationCode.ToString() + " в строке номер " + numberOfStringWithSituation.ToString() + ": " + stringWithSituation + ". " + description;
		}
	}
	
	/// <summary>
	/// Класс описания сообщения с информацией в процессе обработки кода. Генерируется методом-фермой.
	/// </summary>
	public class InfoReport
	:	SiruationInSourceCodeReport
	{
		private InfoReport(String description,int numberOfString,String pointedString):base(description,0,numberOfString,pointedString)
		{
		
		}
		
		public static InfoReport create(String description,int numberOfString,String pointedString)
		{
		    InfoReport report = new InfoReport(description, numberOfString, pointedString);
		    InterpretatorEnvironment.Env.log.message("Info",report.ToString());
		    return report;
		}
		
		public override String ToString()
		{
		    return description + " Строка " + numberOfStringWithSituation + ", " + numberOfStringWithSituation;
		}
	}
	
	public class Reporter
	{
		List<SiruationInSourceCodeReport> reports;
		
		public Reporter()
		{
		    initialize();
		}
		
		void initialize()
		{
		    reports = new List<SiruationInSourceCodeReport>();
		}
		
		public void describeError(String descr,int code, int stringNumber, String error)
		{
		    ErrorReport report = ErrorReport.create(descr, code, stringNumber,error);
		    reports.Add(report);
		}
		
		public void deleteAllReportsWithCode(int code)
		{
		    reports.RemoveAll(rep => rep.code == code);
		}
	}

    public class Lexem
    {
        public int StringNumber
        {
            get;
            private set;
        }
        public int StartSymbolNumber
        {
            get;
            private set;
        }
        public int EndSymbolNumber
        {
            get;
            private set;
        }
        public String PageName
        {
            get;
            set;
        }
        public String Value;
        public Lexem(int stringNumber,int start,int end)
        {
            StringNumber = stringNumber;
            StartSymbolNumber = start;
            EndSymbolNumber = end;
        }
        public Lexem(int stringNumber,int start,int end,String value):this(stringNumber,start,end)
        {
            Value = value;
            validateData();
        }
        public override string ToString()
        {
            return Value;
        }

        public void setOtherLexem(String value,int start,int end, int stringNumber)
        {
            StringNumber = stringNumber;
            StartSymbolNumber = start;
            EndSymbolNumber = end;
            Value = value;
            validateData();
        }

        public static implicit operator String(Lexem lex)
        {
            return lex.Value;
        }

        public Char this[int i]
        {
            get { return Value[i]; }
        }

        public int Length
        {
            get {return Value.Length; }
        }


        private void validateData()
        {
            if(Value.Length==0)
            {
                throw new ArgumentException("Обнаружена пустая лексема", "Lexem.Value", null);
            }
        }
    }


}