using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Xml.Serialization;
using System.Xml;
using System.IO;
using SerializableClasses;

namespace ProgrammatronIDE
{
    public partial class MainWindow : Form
    {
        ProgrammatronProject project;
        
        public MainWindow()
        {
            InitializeComponent();
            //TO DO сделать проверку на кол-во экземпляров? лончер?
            InitIDE();
        }

        private void InitIDE()
        {
            InitVariables();
        }

        private void InitVariables()
        {
            project = new ProgrammatronProject();
            IDEState.InitSettings();
        }

        private void inTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void sourceCodeBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// executeButtonClicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            ProcessStartInfo executeProcessInfo = new ProcessStartInfo("programmatronExecutable", " -debug");
        }

        private void MainWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            IDEState.SaveSettings();
        }
    }

    class ProgrammatronProject
    {
        //path to source code file
        String listingFileName;
    }

    class IDEState
    {
        static public IDESettings Settings;
        static public string SettingsFilePath = @"IDE/Settings.xml";
        static public string DefaultListingFileName = "program.pgt";
        static public void InitSettings()
        {
            if(File.Exists(SettingsFilePath))
            {
                if (File.ReadAllText(SettingsFilePath).Length != 0)
                {
                    System.Xml.Serialization.XmlSerializer ser = new XmlSerializer(typeof(IDESettings));
                    using (StreamReader stream = new StreamReader(SettingsFilePath))
                    {
                        Settings = ser.Deserialize(stream) as IDESettings;
                        stream.Close();
                    }
                }
                else
                    RestoreSettings();
            }
            else
            {
                RestoreSettings();
            }
        }

        private static void RestoreSettings()
        {
            Settings = new IDESettings(DefaultListingFileName);
            SaveSettings();
        }

        static public void SaveSettings()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(SettingsFilePath));
            //File.Create(SettingsFilePath);
            XmlSerializer ser = new XmlSerializer(typeof(IDESettings));
            using (StreamWriter writer = new StreamWriter(SettingsFilePath,false))
            {
                ser.Serialize(writer, Settings);
                writer.Close();
            }
        }
    }
}
