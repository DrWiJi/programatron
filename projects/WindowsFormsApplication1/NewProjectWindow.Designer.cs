namespace ProgrammatronIDE
{
    partial class NewProjectWindow
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
            this.ProjectNameTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.CreateProjectButton = new System.Windows.Forms.Button();
            this.CancelButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ProjectNameTextBox
            // 
            this.ProjectNameTextBox.Location = new System.Drawing.Point(230, 6);
            this.ProjectNameTextBox.Name = "ProjectNameTextBox";
            this.ProjectNameTextBox.Size = new System.Drawing.Size(196, 20);
            this.ProjectNameTextBox.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(111, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Имя нового проекта";
            // 
            // CreateProjectButton
            // 
            this.CreateProjectButton.Location = new System.Drawing.Point(351, 41);
            this.CreateProjectButton.Name = "CreateProjectButton";
            this.CreateProjectButton.Size = new System.Drawing.Size(75, 23);
            this.CreateProjectButton.TabIndex = 2;
            this.CreateProjectButton.Text = "Создать";
            this.CreateProjectButton.UseVisualStyleBackColor = true;
            this.CreateProjectButton.Click += new System.EventHandler(this.CreateProjectButton_Click);
            // 
            // CancelButton
            // 
            this.CancelButton.Location = new System.Drawing.Point(270, 41);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(75, 23);
            this.CancelButton.TabIndex = 3;
            this.CancelButton.Text = "Отмена";
            this.CancelButton.UseVisualStyleBackColor = true;
            this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // NewProjectWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(438, 76);
            this.Controls.Add(this.CancelButton);
            this.Controls.Add(this.CreateProjectButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ProjectNameTextBox);
            this.MaximumSize = new System.Drawing.Size(454, 115);
            this.MinimumSize = new System.Drawing.Size(454, 115);
            this.Name = "NewProjectWindow";
            this.Text = "Создание нового проекта";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox ProjectNameTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button CreateProjectButton;
        private System.Windows.Forms.Button CancelButton;
    }
}