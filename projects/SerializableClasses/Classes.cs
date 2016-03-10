using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Xml;

namespace SerializableClasses
{

    [Serializable]
    public class IDESettings
    {
        public String LastFileName;
        public IDESettings()
        {
            LastFileName = "program.pgt";
        }
        /// <summary>
        /// Use for first run
        /// </summary>
        /// <param name="listingFileName"></param>
        public IDESettings(string listingFileName)
        {
            LastFileName = listingFileName;
        }
    }


}
