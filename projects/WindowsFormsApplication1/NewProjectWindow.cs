using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SerializableClasses;

namespace ProgrammatronIDE
{
    public partial class NewProjectWindow : Form
    {

        public delegate void CreatedProjectHandler();
        public event CreatedProjectHandler CreatedProject;
        public NewProjectWindow()
        {
            InitializeComponent();
        }

        private void CreateProjectButton_Click(object sender, EventArgs e)
        {
            if (ValidatingFileName(ProjectNameTextBox.Text))
            {
                ProgrammatronProject project = new ProgrammatronProject();
                project.ListingFileName = ProjectNameTextBox.Text + ".pgt";
                project.ProjectName = ProjectNameTextBox.Text;
                project.VisibleProjectName = ProjectNameTextBox.Text;
                ProjectManager.CreateProjectFiles(project);
                IDEState.Settings.LastProject = project;
                CreatedProject();
                Close();
            }
            else
            {
                MessageBox.Show("Неверный формат имени проекта. Разрешаются только буквы, цифры и пробелы.","Внимание");
            }
        }

        private bool ValidatingFileName(String str)
        {
            foreach (char cur in str)
            {
                if (!(cur == ' ' || Char.IsLetterOrDigit(cur)))
                {
                    return false;
                }
            }
            return true;
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
