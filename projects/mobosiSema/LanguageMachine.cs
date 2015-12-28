using System;
using System.Collections.Generic;
using System.ComponentModel;

using System.Linq;
using System.Text;
using VMenvironment;

namespace programmatronCore
{
    public class Constants
    {
        public static String defaultExecuteFilePath = "program.pgt";
        public static String spacerBetweenTitleAndTextInMessage = "\\\\//";
    }

    public class Parameters
    {
        public static virtualMachineParameters virtualMachineParameters;
        public static String PRODUCTFULLNAME = "Programmatron Interpretator(C) Alexeev Sergey, Volgograd 2015";
        public static String PRODUCTVERSION = "Alpha 0.2.3";
    }

    public struct virtualMachineParameters
    {
        public String executableFilePath;
        public bool isDebug;
        public bool isIgnoreWarnings;
        public bool isSaveReports;

        public virtualMachineParameters(string executableFilePath, bool debugMode, bool ignoreWarnings, bool saveReports)
        {
            this.executableFilePath = executableFilePath;
            this.isSaveReports = saveReports;
            this.isIgnoreWarnings = ignoreWarnings;
            this.isDebug = debugMode;
        }

        override public String ToString()
        {
            return executableFilePath + " Debug: " + isDebug.ToString() + " Ignore Warnings: " + isIgnoreWarnings.ToString() + " Save Reports: " + isSaveReports.ToString();
        }
        public static bool operator==(virtualMachineParameters a, virtualMachineParameters b)
        {
            return a.isDebug == b.isDebug && a.isIgnoreWarnings == b.isIgnoreWarnings && a.isSaveReports == b.isSaveReports && a.executableFilePath == b.executableFilePath;
        }
        public static bool operator!=(virtualMachineParameters a, virtualMachineParameters b)
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
        protected String description, stringWithSituation;
        protected int situationCode, numberOfStringWithSituation;
        public SiruationInSourceCodeReport(String description, int situationCode,int numberOfStringWithSituation, String stringWithSituation)
        {
            this.description = description;
            this.situationCode = situationCode;
            this.numberOfStringWithSituation = numberOfStringWithSituation;
            this.stringWithSituation = stringWithSituation;
        }

        public int code
        {
            get { return situationCode; }
        }
    }



}