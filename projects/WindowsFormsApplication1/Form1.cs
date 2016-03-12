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
        public MainWindow()
        {
            InitializeComponent();
            //TO DO сделать проверку на кол-во экземпляров? лончер?
            InitIDE();
        }

        private void InitIDE()
        {
            InitSettings();
            InitProject();
        }

        private void InitSettings()
        {
            IDEState.InitWorkingDirsStore();
            IDEState.InitSettings();
        }

        private void InitProject()
        {
            ProgrammatronProject project;
            if(File.Exists(IDEState.Settings.ProjectsPath+@"\"+IDEState.Settings.LastProject.GetProjectPropertiesFileName()))
            {
                DialogResult needOpenLastProject = MessageBox.Show("Открыть последний проект?","Вопрос",MessageBoxButtons.YesNo);
                if (needOpenLastProject == DialogResult.Yes)
                {
                    project = IDEState.Settings.LastProject;
                }
                else
                {
                    //create default and use
                    //TO DO
                    project = new ProgrammatronProject();
                    ProjectManager.CreateProjectFiles(project);
                }
                
            }
            else
            {
                project = new ProgrammatronProject();
                ProjectManager.CreateProjectFiles(project);
            }
            LoadProgrammatronProject(project);
            UpdateTitle();
        }

        private void UpdateTitle()
        {
            this.Text = "Programmatron IDE - " + IDEState.Settings.LastProject.VisibleProjectName;
        }

        private void LoadProgrammatronProject(ProgrammatronProject project)
        {
            sourceCodeBox.Text = ProjectManager.GetCurrentProjectListingFromProjectListingFile();
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
        /// Execute
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            ProjectManager.SaveProjectListingFile(sourceCodeBox.Text);
            ProcessStartInfo executeProcessInfo = new ProcessStartInfo("programmatronExecutable", GenerateArgsForInterpreter());
            Process.Start(executeProcessInfo);
        }

        private String GenerateArgsForInterpreter()
        {
            String quickRunFlag = "";
            if (IDEState.Settings.LastProject.QuickLightRun)
                quickRunFlag = " -quickLightRun";
            String saveReportsFlag = "";
            if (IDEState.Settings.LastProject.SaveReports)
                saveReportsFlag = " -saveReports";
            return "\"" + ProjectManager.GetCurrentProjectListingFullPath() + "\"" + " -debug" + quickRunFlag + saveReportsFlag;
        }

        private void MainWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            //TO DO перенести в событие FormClosing и предложить сохранить перед выходом
            IDEState.SaveSettings();
        }

        /// <summary>
        /// Save
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            ProjectManager.SaveProjectListingFile(sourceCodeBox.Text);
            ProjectManager.SaveProjectPropertyFile(IDEState.Settings.LastProject);
        }

        /// <summary>
        /// Settings... 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton1_Click_1(object sender, EventArgs e)
        {
            ProjectSettingsWindow wnd = new ProjectSettingsWindow();
            wnd.ProjectNameChanged += UpdateTitle;
            wnd.Show();

        }
    }

    class IDEState
    {
        public static IDESettings Settings;
        public static string SettingsFilePath = @"IDE\Settings.xml";
        //этот файл нужен для чистки системы от того, чем насрёт IDE. Хранит все дирректории, которые создаются для работы в системе
        public static string WorkingDirectoriesListPath = @"work_dirs.list";
        public static string publicDefaultProjectsPath = Environment.GetEnvironmentVariable("SYSTEMDRIVE") + Environment.GetEnvironmentVariable("HOMEPATH")+ @"\Programmatron Projects";
        public static string publicDefaultListingFileName = "program.pgt";
        static public void InitSettings()
        {
            Settings = new IDESettings();
            RestoreDirectory(publicDefaultProjectsPath);
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

        public static void InitWorkingDirsStore()
        {
            File.Create(WorkingDirectoriesListPath).Close();
        }

        public static void RestoreDirectory(String dirName)
        {
            if (!Directory.Exists(dirName))
            {
                Directory.CreateDirectory(dirName);
                using (StreamWriter writer = new StreamWriter(WorkingDirectoriesListPath,true))
                {
                    writer.WriteLine(dirName);
                    writer.Close();
                }
            }
        }

        private static void RestoreSettings()
        {
            Settings = new IDESettings();
            SaveSettings();
        }

        static public void SaveSettings()
        {
            RestoreDirectory(Path.GetDirectoryName(SettingsFilePath));
            //File.Create(SettingsFilePath);
            XmlSerializer ser = new XmlSerializer(typeof(IDESettings));
            using (StreamWriter writer = new StreamWriter(SettingsFilePath,false))
            {
                ser.Serialize(writer, Settings);
                writer.Close();
            }
        }
    }

    class ProjectManager
    {
        static public void CreateProjectFiles(ProgrammatronProject project)
        {
            IDEState.RestoreDirectory(IDEState.publicDefaultProjectsPath);
            SaveProjectPropertyFile(IDEState.Settings.LastProject);
            using (StreamWriter writer = new StreamWriter(GetCurrentProjectListingFullPath(), false,Encoding.Default))
            {
                writer.Write("");
                writer.Close();
            }
        }

        static public void RemoveProjectFiles(ProgrammatronProject project)
        {
            if (Directory.Exists(IDEState.publicDefaultProjectsPath))
            {
                File.Delete(GetCurrentProjectListingFullPath());
                File.Delete(GetCurrentProjectPropertiesFullPath());
            }
        }

        public static string GetCurrentProjectPropertiesFullPath()
        {
            return IDEState.Settings.ProjectsPath + @"\" + IDEState.Settings.LastProject.GetProjectPropertiesFileName();
        }

        public static string GetCurrentProjectListingFullPath()
        {
            return IDEState.Settings.ProjectsPath + @"\" + IDEState.Settings.LastProject.ListingFileName;
        }

        static public void LoadProjectPropertiesFiles(String ProjectPropertiesFilePath)
        {
            XmlSerializer ser = new XmlSerializer(typeof(ProgrammatronProject));
            using (StreamReader reader = new StreamReader(GetCurrentProjectPropertiesFullPath()))
            {
                IDEState.Settings.LastProject = ser.Deserialize(reader) as ProgrammatronProject;
                reader.Close();
            }
        }

        static public String GetCurrentProjectListingFromProjectListingFile()
        {
            String listing = "";
            using (StreamReader reader = new StreamReader(GetCurrentProjectListingFullPath(), Encoding.Default))
            {
                listing = reader.ReadToEnd();
                reader.Close();
            }
            return listing;
        }

        static public void SaveProjectPropertyFile(ProgrammatronProject project)
        {
            IDEState.RestoreDirectory(Path.GetDirectoryName(GetCurrentProjectPropertiesFullPath()));
            XmlSerializer ser = new XmlSerializer(typeof(ProgrammatronProject));
            using (StreamWriter writer = new StreamWriter(GetCurrentProjectPropertiesFullPath(), false))
            {
                ser.Serialize(writer, project);
                writer.Close();
            }
        }

        static public void SaveProjectListingFile(String code)
        {
            IDEState.RestoreDirectory(IDEState.publicDefaultProjectsPath);
            using (StreamWriter writer = new StreamWriter(GetCurrentProjectListingFullPath(), false, Encoding.Default))
            {
                writer.Write(code);
                writer.Close();
            }
        }
    }
}
