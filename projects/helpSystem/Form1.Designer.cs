namespace helpSystem
{
    partial class helpSystemWindow
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Главная");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Базовые функции");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("Введение");
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("Переменные");
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("Типы и данные");
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("Язык", new System.Windows.Forms.TreeNode[] {
            treeNode2,
            treeNode3,
            treeNode4,
            treeNode5});
            this.HelpGroupsTreeView = new System.Windows.Forms.TreeView();
            this.label1 = new System.Windows.Forms.Label();
            this.HelpMonitorRichTextBox = new System.Windows.Forms.RichTextBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // HelpGroupsTreeView
            // 
            this.HelpGroupsTreeView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.HelpGroupsTreeView.Location = new System.Drawing.Point(3, 3);
            this.HelpGroupsTreeView.Name = "HelpGroupsTreeView";
            treeNode1.Checked = true;
            treeNode1.Name = "HomePageNode";
            treeNode1.Text = "Главная";
            treeNode1.ToolTipText = "Базовая информация";
            treeNode2.Name = "BasicFunctionsNode";
            treeNode2.Text = "Базовые функции";
            treeNode2.ToolTipText = "Описния базовых функций в языке программатрон";
            treeNode3.Name = "IntroNode";
            treeNode3.Text = "Введение";
            treeNode4.Name = "VariablesNode";
            treeNode4.Text = "Переменные";
            treeNode5.Name = "TypesAndDataNode";
            treeNode5.Text = "Типы и данные";
            treeNode6.Name = "ProgrammingNode";
            treeNode6.Text = "Язык";
            treeNode6.ToolTipText = "Описания, помощь в использовании данной системы";
            this.HelpGroupsTreeView.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode6});
            this.HelpGroupsTreeView.Size = new System.Drawing.Size(233, 287);
            this.HelpGroupsTreeView.TabIndex = 0;
            this.HelpGroupsTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(0, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Разделы";
            // 
            // HelpMonitorRichTextBox
            // 
            this.HelpMonitorRichTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.HelpMonitorRichTextBox.Location = new System.Drawing.Point(3, 3);
            this.HelpMonitorRichTextBox.Name = "HelpMonitorRichTextBox";
            this.HelpMonitorRichTextBox.ReadOnly = true;
            this.HelpMonitorRichTextBox.Size = new System.Drawing.Size(470, 287);
            this.HelpMonitorRichTextBox.TabIndex = 2;
            this.HelpMonitorRichTextBox.Text = "";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(12, 25);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.HelpGroupsTreeView);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.HelpMonitorRichTextBox);
            this.splitContainer1.Size = new System.Drawing.Size(719, 293);
            this.splitContainer1.SplitterDistance = 239;
            this.splitContainer1.TabIndex = 3;
            // 
            // helpSystemWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(743, 330);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.label1);
            this.MinimumSize = new System.Drawing.Size(472, 266);
            this.Name = "helpSystemWindow";
            this.Text = "Помощь";
            this.Load += new System.EventHandler(this.helpSystemWindow_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView HelpGroupsTreeView;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RichTextBox HelpMonitorRichTextBox;
        private System.Windows.Forms.SplitContainer splitContainer1;
    }
}

