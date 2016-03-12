using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace ProgrammatronIDE
{
    public partial class ProjectSettingsWindow : Form
    {
        public ProjectSettingsWindow()
        {
            InitializeComponent();
            InitSettingsFields();
        }

        int InvalidFieldsCount = 0;

        void InitSettingsFields()
        {
            ListingFileNameTextBox.Text = IDEState.Settings.LastProject.ListingFileName;
            ProjectNameTextBox.Text = IDEState.Settings.LastProject.ProjectName;
            QuickLightStartCkeckBox.Checked = IDEState.Settings.LastProject.QuickLightRun;
            SaveReportsCkeckBox.Checked = IDEState.Settings.LastProject.SaveReports;
            VisibleNameTextBox.Text = IDEState.Settings.LastProject.VisibleProjectName;
            VisibleProjectnameDifferentCheckBox.Checked = IDEState.Settings.LastProject.ProjectName != IDEState.Settings.LastProject.VisibleProjectName;
            ListingNameDifferentCkechBox.Checked = IDEState.Settings.LastProject.ListingFileName != IDEState.Settings.LastProject.ProjectName;
        }

        private void CloseSettingsButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void AcceptSettingsButton_Click(object sender, EventArgs e)
        {
            if (!IsAllFiledsCorrect())
            {
                MessageBox.Show("Исправьте формат полей, затем сохранение станет возможным. Подробнее о допустимых форматах полей в справке.");
                return;
            }
            SaveChangedProjectSettings();
        }

        private void SaveChangedProjectSettings()
        {
            String SourceCode = "";
            SourceCode = File.ReadAllText(ProjectManager.GetCurrentProjectListingFullPath(),Encoding.Default);
            ProjectManager.RemoveProjectFiles(IDEState.Settings.LastProject);
            IDEState.Settings.LastProject.ListingFileName = ListingFileNameTextBox.Text;
            IDEState.Settings.LastProject.ProjectName = ProjectNameTextBox.Text;
            IDEState.Settings.LastProject.QuickLightRun = QuickLightStartCkeckBox.Checked;
            IDEState.Settings.LastProject.SaveReports = SaveReportsCkeckBox.Checked;
            IDEState.Settings.LastProject.VisibleProjectName = VisibleNameTextBox.Text;
            ProjectManager.CreateProjectFiles(IDEState.Settings.LastProject);
            File.WriteAllText(ProjectManager.GetCurrentProjectListingFullPath(), SourceCode, Encoding.Default);
            ProjectNameChanged();
        }

        private void OkSettingsButton_Click(object sender, EventArgs e)
        {
            if (!IsAllFiledsCorrect())
            {
                MessageBox.Show("Исправьте формат полей, сохранение невозможно. Подробнее о допустимых форматах полей в справке.");
                return;
            }
            SaveChangedProjectSettings();
            this.Close();
        }

        private void ListingNameDifferentCkechBox_CheckedChanged(object sender, EventArgs e)
        {
            ListingFileNameTextBox.Enabled = ListingNameDifferentCkechBox.Checked;
            if (ListingNameDifferentCkechBox.Checked == false)
                ListingFileNameTextBox.Text = ProjectNameTextBox.Text;
            ModFormListingFileText();
        }

        private void VisibleProjectnameDifferentCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            VisibleNameTextBox.Enabled = VisibleProjectnameDifferentCheckBox.Checked;
            if (VisibleProjectnameDifferentCheckBox.Checked == false)
                VisibleNameTextBox.Text = ProjectNameTextBox.Text;
        }

        private bool ValidatingFileName(String str)
        {
            foreach(char cur in str)
            {
                if(!(cur==' '||Char.IsLetterOrDigit(cur)))
                {
                    return false;
                }
            }
            return true;
        }

        private void ProjectNameTextBox_Validating(object sender, CancelEventArgs e)
        {
            TextBox box = sender as TextBox;
            if(!ValidatingFileName(box.Text))
            {
                e.Cancel = true;
                box.Focus();
                MessageBox.Show("Недопустимый формат имени. Имя должно стотоять только из пробелов, цифр и букв.", "Недопустимый формат");
            }
            e.Cancel = false;
        }

        bool IsAllFiledsCorrect()
        {
            return ValidatingFileName(VisibleNameTextBox.Text) &&
                ValidatingFileName(ProjectNameTextBox.Text) &&
                ValidatingFileName(Path.GetFileNameWithoutExtension(ListingFileNameTextBox.Text));
        }

        private void ListingFileNameTextBox_Leave(object sender, EventArgs e)
        {
            ModFormListingFileText();
        }

        private void ModFormListingFileText()
        {
            if (Path.GetExtension(ListingFileNameTextBox.Text) != ".pgt")
                ListingFileNameTextBox.Text = ListingFileNameTextBox.Text + ".pgt";
        }

        public delegate void ProjectNameChangedDelegate();
        public event ProjectNameChangedDelegate ProjectNameChanged;

        private void ListingFileNameTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void ListingFileNameTextBox_Validating(object sender, CancelEventArgs e)
        {
            if (!ValidatingFileName(Path.GetFileNameWithoutExtension(ListingFileNameTextBox.Text)))
            {
                e.Cancel = true;
                ListingFileNameTextBox.Focus();
                MessageBox.Show("Недопустимый формат имени файла. Имя должно стотоять только из пробелов, цифр и букв, в конце должно быть расширение (добавляется автоматически)", "Недопустимый формат");
            }
            e.Cancel = false;
        }
    }
}
