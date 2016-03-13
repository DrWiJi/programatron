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
            NewProjectWindow wnd = new NewProjectWindow();
            wnd.Show();
            wnd.CreatedProject += LoadCurrentProject;
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
            //TO DO отправить в окно настройки IDE
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

        public void LoadCurrentProject()
        {
            ProjectManager.LoadProjectPropertiesFiles(ProjectManager.GetCurrentProjectPropertiesFullPath());
            sourceCodeBox.Text = ProjectManager.GetCurrentProjectListingFromProjectListingFile();
            UpdateTitle();
        }

        /// <summary>
        /// Open...
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton1_Click_2(object sender, EventArgs e)
        {
            //Temp
            LoadExistingProject();
        }

        private void LoadExistingProject()
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Programmatron project *.pgtp|*.pgtp";
            dlg.Title = "Выберите файл с проектом, который хотите открыть";
            dlg.InitialDirectory = IDEState.Settings.ProjectsPath;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                if (Path.GetDirectoryName(dlg.FileName) != IDEState.Settings.ProjectsPath)
                {
                    MessageBox.Show("Неверная директория. Выберите файл из стандартной директории с проектами.", "Внимание");
                    LoadExistingProject();
                    return;
                }
                OpenProjectByFileName(dlg.FileName);
            }
        }

        private void OpenProjectByFileName(String str)
        {
            ProjectManager.LoadProjectPropertiesFilesFromFullPath(str);
            sourceCodeBox.Text = ProjectManager.GetCurrentProjectListingFromProjectListingFile();
            UpdateTitle();
        }

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(ProjectManager.GetCurrentProjectListingFromProjectListingFile()!=sourceCodeBox.Text)
            {
                DialogResult result = MessageBox.Show("Сохранить изменения перед выходом?", "Выход", MessageBoxButtons.YesNoCancel);
                if(result == DialogResult.No)
                {
                    return;
                }
                else if(result == DialogResult.Yes)
                {
                    ProjectManager.SaveProjectListingFile(sourceCodeBox.Text);
                }
                else
                {
                    e.Cancel = true;
                    return;
                }
            }
            e.Cancel = false;
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            ProcessStartInfo HelpInfo = new ProcessStartInfo("helpSystem.exe");
            Process.Start(HelpInfo);
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
            SaveProjectPropertyFile(project);
            using (StreamWriter writer = new StreamWriter(GetProjectListingFullPath(project.ListingFileName), false,Encoding.Default))
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

        public static string GetProjectPropertiesFullPath(String ProjectName)
        {
            return IDEState.Settings.ProjectsPath + @"\" + ProjectName+".pgtp";
        }

        public static string GetCurrentProjectListingFullPath()
        {
            return IDEState.Settings.ProjectsPath + @"\" + IDEState.Settings.LastProject.ListingFileName;
        }

        public static string GetProjectListingFullPath(String ListingNameFile)
        {
            return IDEState.Settings.ProjectsPath + @"\" + ListingNameFile;
        }

        static public void LoadProjectPropertiesFiles(String ProjectPropertiesFilePath)
        {
            XmlSerializer ser = new XmlSerializer(typeof(ProgrammatronProject));
            using (StreamReader reader = new StreamReader(GetProjectPropertiesFullPath(ProjectPropertiesFilePath)))
            {
                IDEState.Settings.LastProject = ser.Deserialize(reader) as ProgrammatronProject;
                reader.Close();
            }
        }

        static public void LoadProjectPropertiesFilesFromFullPath(String ProjectPropertiesFilePath)
        {
            XmlSerializer ser = new XmlSerializer(typeof(ProgrammatronProject));
            using (StreamReader reader = new StreamReader(ProjectPropertiesFilePath))
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
            IDEState.RestoreDirectory(Path.GetDirectoryName(IDEState.SettingsFilePath));
            XmlSerializer ser = new XmlSerializer(typeof(ProgrammatronProject));
            using (StreamWriter writer = new StreamWriter(GetProjectPropertiesFullPath(project.ProjectName), false))
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
