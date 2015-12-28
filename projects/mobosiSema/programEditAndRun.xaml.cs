using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Threading.Tasks;
using System.Threading;
using programmatronCore;
// Документацию по шаблону элемента пустой страницы см. по адресу http://go.microsoft.com/fwlink/?LinkID=390556

namespace mobosiSema
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class programEditAndRun : Page
    {
        public programEditAndRun()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Вызывается перед отображением этой страницы во фрейме.
        /// </summary>
        /// <param name="e">Данные события, описывающие, каким образом была достигнута эта страница.
        /// Этот параметр обычно используется для настройки страницы.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            codeProg.Text = Current.prog.Code;
            InputOutput.get = getCommand;
            InputOutput.put = logAppend;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Current.prog.Code = codeProg.Text;
            DataSaver.DS.save();
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            log.Text = "";
            LexemAnalyzer analyzer = new LexemAnalyzer(codeProg.Text);
            SyntaxTreeGenerator interpretator = new SyntaxTreeGenerator(analyzer.analize());
            interpretator.generateTree();
            await interpretator.doCode();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            
            Commands.Queue.Add(command1.Text);
        }
        public async Task<string> logAppend(string a)
        {
            log.Text += a;
            return "sss";
        }
        public async Task<string> getCommand()
        {
            while (Commands.Queue.Count== 0)
            {

                    await Task.Delay(50);

            }
            string result = Commands.Queue[0];
            Commands.Queue.RemoveAt(0);
            return result;
        }
    }
    public static class Commands
    {
      static public List<string>  Queue = new List<string>();
    }
    public static class InputOutput
    {
        static public Func<string,Task<string>> put;
        static public Func<Task<string>> get; 
    }
}
