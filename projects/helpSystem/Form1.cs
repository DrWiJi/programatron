using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FunctionsList;

namespace helpSystem
{
    public partial class helpSystemWindow : Form
    {
        SortedDictionary<string, string> functionsHelpInfo;
        public helpSystemWindow()
        {
            InitializeComponent();
            initialize();
            GenerateFunctionsHelp();
        }

        private void initialize()
        {
            functionsHelpInfo = new SortedDictionary<string, string>();
        }

        private void GenerateFunctionsHelp()
        {
            foreach(FunctionTemplate cur in Functions.Storage)
            {
                String title = String.Format("{0}({1})", cur.Name, cur.Info.Args);
                String descrRTF = "{\\rtf1\\ansi\\ansicpg1251\\deff0\\deflang1049{\\fonttbl{\\f0\\fnil\\fcharset204 Microsoft Sans Serif;}}\\viewkind4\\uc1\\pard\\f0{\\fs30\\b " + title + @"}\par{\fs14\i " + cur.Info.Group + @"}\par{\fs20 " + cur.Info.Help + @"}\par }";
                functionsHelpInfo.Add(title, descrRTF);
                if (!HelpGroupsTreeView.Nodes["ProgrammingNode"].Nodes["BasicFunctionsNode"].Nodes.ContainsKey(cur.Info.Group))
                {
                    HelpGroupsTreeView.Nodes["ProgrammingNode"].Nodes["BasicFunctionsNode"].Nodes.Add(cur.Info.Group, cur.Info.Group);
                }
                HelpGroupsTreeView.Nodes["ProgrammingNode"].Nodes["BasicFunctionsNode"].Nodes[cur.Info.Group].Nodes.Add(title);
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if(functionsHelpInfo.ContainsKey(HelpGroupsTreeView.SelectedNode.Text) )
            {
                HelpMonitorRichTextBox.Rtf = functionsHelpInfo[HelpGroupsTreeView.SelectedNode.Text];
            }
            else if (HelpGroupsTreeView.SelectedNode.Name == "HomePageNode")
            {
                HelpMonitorRichTextBox.Rtf = Properties.Resources.Система_помощи_по_языку_Программатрон_и_его_окружению;
            }
            else if (HelpGroupsTreeView.SelectedNode.Name == "ProgrammingNode")
            {
                HelpMonitorRichTextBox.Rtf = Properties.Resources.Справка_по_языку_Программатрон;
            }
            else if (HelpGroupsTreeView.SelectedNode.Name == "VariablesNode")
            {
                HelpMonitorRichTextBox.Rtf = Properties.Resources.Переменные;
            }
            else if (HelpGroupsTreeView.SelectedNode.Name == "IntroNode")
            {
                HelpMonitorRichTextBox.Rtf = Properties.Resources.Описание_синтаксиса_языка_Программатрон;
            }
            else
            {
                HelpMonitorRichTextBox.Rtf = "{\\rtf1\\ansi\\ansicpg1251\\deff0\\deflang1049{\\fonttbl{\\f0\\fnil\\fcharset204 Microsoft Sans Serif;}}\\viewkind4\\uc1\\pard\\f0\\fs17{\\fs15 Нет описания}}";
            }
        }

        private void helpSystemWindow_Load(object sender, EventArgs e)
        {

        }
    }
}
