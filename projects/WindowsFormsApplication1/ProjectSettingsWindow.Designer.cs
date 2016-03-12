namespace ProgrammatronIDE
{
    partial class ProjectSettingsWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SettingsTab = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.OkSettingsButton = new System.Windows.Forms.Button();
            this.AcceptSettingsButton = new System.Windows.Forms.Button();
            this.CloseSettingsButton = new System.Windows.Forms.Button();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.label1 = new System.Windows.Forms.Label();
            this.VisibleProjectnameDifferentCheckBox = new System.Windows.Forms.CheckBox();
            this.ListingNameDifferentCkechBox = new System.Windows.Forms.CheckBox();
            this.QuickLightStartCkeckBox = new System.Windows.Forms.CheckBox();
            this.SaveReportsCkeckBox = new System.Windows.Forms.CheckBox();
            this.ListingFileNameTextBox = new System.Windows.Forms.TextBox();
            this.VisibleNameTextBox = new System.Windows.Forms.TextBox();
            this.ProjectNameTextBox = new System.Windows.Forms.TextBox();
            this.SettingsTab.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // SettingsTab
            // 
            this.SettingsTab.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SettingsTab.Controls.Add(this.tabPage1);
            this.SettingsTab.Controls.Add(this.tabPage2);
            this.SettingsTab.Controls.Add(this.tabPage3);
            this.SettingsTab.Location = new System.Drawing.Point(2, 12);
            this.SettingsTab.Name = "SettingsTab";
            this.SettingsTab.SelectedIndex = 0;
            this.SettingsTab.Size = new System.Drawing.Size(382, 317);
            this.SettingsTab.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.ListingNameDifferentCkechBox);
            this.tabPage1.Controls.Add(this.ListingFileNameTextBox);
            this.tabPage1.Controls.Add(this.VisibleProjectnameDifferentCheckBox);
            this.tabPage1.Controls.Add(this.VisibleNameTextBox);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.ProjectNameTextBox);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(374, 291);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Основные параметры";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.QuickLightStartCkeckBox);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(374, 291);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Параметры запуска";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // OkSettingsButton
            // 
            this.OkSettingsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.OkSettingsButton.Location = new System.Drawing.Point(271, 335);
            this.OkSettingsButton.Name = "OkSettingsButton";
            this.OkSettingsButton.Size = new System.Drawing.Size(109, 23);
            this.OkSettingsButton.TabIndex = 1;
            this.OkSettingsButton.Text = "Ok";
            this.OkSettingsButton.UseVisualStyleBackColor = true;
            this.OkSettingsButton.Click += new System.EventHandler(this.OkSettingsButton_Click);
            // 
            // AcceptSettingsButton
            // 
            this.AcceptSettingsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.AcceptSettingsButton.Location = new System.Drawing.Point(156, 335);
            this.AcceptSettingsButton.Name = "AcceptSettingsButton";
            this.AcceptSettingsButton.Size = new System.Drawing.Size(109, 23);
            this.AcceptSettingsButton.TabIndex = 2;
            this.AcceptSettingsButton.Text = "Применить";
            this.AcceptSettingsButton.UseVisualStyleBackColor = true;
            this.AcceptSettingsButton.Click += new System.EventHandler(this.AcceptSettingsButton_Click);
            // 
            // CloseSettingsButton
            // 
            this.CloseSettingsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CloseSettingsButton.Location = new System.Drawing.Point(41, 335);
            this.CloseSettingsButton.Name = "CloseSettingsButton";
            this.CloseSettingsButton.Size = new System.Drawing.Size(109, 23);
            this.CloseSettingsButton.TabIndex = 3;
            this.CloseSettingsButton.Text = "Закрыть";
            this.CloseSettingsButton.UseVisualStyleBackColor = true;
            this.CloseSettingsButton.Click += new System.EventHandler(this.CloseSettingsButton_Click);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.SaveReportsCkeckBox);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(374, 291);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Параметры выполнения";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Имя проекта";
            // 
            // VisibleProjectnameDifferentCheckBox
            // 
            this.VisibleProjectnameDifferentCheckBox.AutoSize = true;
            this.VisibleProjectnameDifferentCheckBox.Location = new System.Drawing.Point(9, 33);
            this.VisibleProjectnameDifferentCheckBox.Name = "VisibleProjectnameDifferentCheckBox";
            this.VisibleProjectnameDifferentCheckBox.Size = new System.Drawing.Size(168, 17);
            this.VisibleProjectnameDifferentCheckBox.TabIndex = 4;
            this.VisibleProjectnameDifferentCheckBox.Text = "Выводимое имя отличается";
            this.VisibleProjectnameDifferentCheckBox.UseVisualStyleBackColor = true;
            this.VisibleProjectnameDifferentCheckBox.CheckedChanged += new System.EventHandler(this.VisibleProjectnameDifferentCheckBox_CheckedChanged);
            // 
            // ListingNameDifferentCkechBox
            // 
            this.ListingNameDifferentCkechBox.AutoSize = true;
            this.ListingNameDifferentCkechBox.Location = new System.Drawing.Point(9, 59);
            this.ListingNameDifferentCkechBox.Name = "ListingNameDifferentCkechBox";
            this.ListingNameDifferentCkechBox.Size = new System.Drawing.Size(187, 17);
            this.ListingNameDifferentCkechBox.TabIndex = 6;
            this.ListingNameDifferentCkechBox.Text = "Имя файла с кодом отличается";
            this.ListingNameDifferentCkechBox.UseVisualStyleBackColor = true;
            this.ListingNameDifferentCkechBox.CheckedChanged += new System.EventHandler(this.ListingNameDifferentCkechBox_CheckedChanged);
            // 
            // QuickLightStartCkeckBox
            // 
            this.QuickLightStartCkeckBox.AutoSize = true;
            this.QuickLightStartCkeckBox.Location = new System.Drawing.Point(6, 3);
            this.QuickLightStartCkeckBox.Name = "QuickLightStartCkeckBox";
            this.QuickLightStartCkeckBox.Size = new System.Drawing.Size(103, 17);
            this.QuickLightStartCkeckBox.TabIndex = 0;
            this.QuickLightStartCkeckBox.Text = "Быстрый старт";
            this.QuickLightStartCkeckBox.UseVisualStyleBackColor = true;
            // 
            // SaveReportsCkeckBox
            // 
            this.SaveReportsCkeckBox.AutoSize = true;
            this.SaveReportsCkeckBox.Location = new System.Drawing.Point(6, 3);
            this.SaveReportsCkeckBox.Name = "SaveReportsCkeckBox";
            this.SaveReportsCkeckBox.Size = new System.Drawing.Size(158, 17);
            this.SaveReportsCkeckBox.TabIndex = 2;
            this.SaveReportsCkeckBox.Text = "Сохранение логов работы";
            this.SaveReportsCkeckBox.UseVisualStyleBackColor = true;
            // 
            // ListingFileNameTextBox
            // 
            this.ListingFileNameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ListingFileNameTextBox.Location = new System.Drawing.Point(202, 59);
            this.ListingFileNameTextBox.Name = "ListingFileNameTextBox";
            this.ListingFileNameTextBox.Size = new System.Drawing.Size(163, 20);
            this.ListingFileNameTextBox.TabIndex = 5;
            this.ListingFileNameTextBox.TextChanged += new System.EventHandler(this.ListingFileNameTextBox_TextChanged);
            this.ListingFileNameTextBox.Leave += new System.EventHandler(this.ListingFileNameTextBox_Leave);
            this.ListingFileNameTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.ListingFileNameTextBox_Validating);
            // 
            // VisibleNameTextBox
            // 
            this.VisibleNameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.VisibleNameTextBox.Location = new System.Drawing.Point(202, 33);
            this.VisibleNameTextBox.Name = "VisibleNameTextBox";
            this.VisibleNameTextBox.Size = new System.Drawing.Size(163, 20);
            this.VisibleNameTextBox.TabIndex = 3;
            this.VisibleNameTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.ProjectNameTextBox_Validating);
            // 
            // ProjectNameTextBox
            // 
            this.ProjectNameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ProjectNameTextBox.Location = new System.Drawing.Point(202, 6);
            this.ProjectNameTextBox.Name = "ProjectNameTextBox";
            this.ProjectNameTextBox.Size = new System.Drawing.Size(163, 20);
            this.ProjectNameTextBox.TabIndex = 0;
            this.ProjectNameTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.ProjectNameTextBox_Validating);
            // 
            // ProjectSettingsWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 361);
            this.Controls.Add(this.CloseSettingsButton);
            this.Controls.Add(this.AcceptSettingsButton);
            this.Controls.Add(this.OkSettingsButton);
            this.Controls.Add(this.SettingsTab);
            this.MaximumSize = new System.Drawing.Size(400, 400);
            this.MinimumSize = new System.Drawing.Size(400, 400);
            this.Name = "ProjectSettingsWindow";
            this.Text = "Настройки текущего проекта";
            this.SettingsTab.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl SettingsTab;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Button OkSettingsButton;
        private System.Windows.Forms.Button AcceptSettingsButton;
        private System.Windows.Forms.Button CloseSettingsButton;
        private System.Windows.Forms.CheckBox VisibleProjectnameDifferentCheckBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox ListingNameDifferentCkechBox;
        private System.Windows.Forms.CheckBox QuickLightStartCkeckBox;
        private System.Windows.Forms.CheckBox SaveReportsCkeckBox;
        private System.Windows.Forms.TextBox ListingFileNameTextBox;
        private System.Windows.Forms.TextBox VisibleNameTextBox;
        private System.Windows.Forms.TextBox ProjectNameTextBox;
    }
}