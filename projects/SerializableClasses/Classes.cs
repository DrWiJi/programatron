using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Xml;

namespace SerializableClasses
{
    [Serializable]
    public class ProgrammatronProject
    {
        //path to source code file
        public String ListingFileName;
        public String ProjectName;
        public ProgrammatronProject()
        {
            ListingFileName = "program.pgt";
            ProjectName = "default project";
        }

        public String GetProjectPropertiesFileName()
        {
            return ProjectName + @".pgtp";
        }
    }

    [Serializable]
    public class IDESettings
    {
        public ProgrammatronProject LastProject;
        public String ProjectsPath;
        public IDESettings()
        {
            LastProject = new ProgrammatronProject();
            ProjectsPath = Environment.GetEnvironmentVariable("SYSTEMDRIVE") + Environment.GetEnvironmentVariable("HOMEPATH") + @"\Programmatron Projects";
        }
        /// <summary>
        /// Use for first run
        /// </summary>
        /// <param name="listingFileName"></param>
        public IDESettings(ProgrammatronProject listingFileName,String projectsPath)
        {
            LastProject = listingFileName;
            ProjectsPath = projectsPath;
        }
    }


}
