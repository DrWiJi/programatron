using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Popups;
using Windows.Storage;
using Windows.Storage.FileProperties;


// Документацию по шаблону элемента пустой страницы см. по адресу http://go.microsoft.com/fwlink/?LinkID=390556

namespace mobosiSema
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class addProgramPage : Page
    {
        public addProgramPage()
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
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {//создать программу но не открывать
            if(programNameTextBox.Text.Length>0)
            {
                ProgrammatronProgram a = new ProgrammatronProgram(programNameTextBox.Text,descriptionTextBox.Text,"");
                a.Id = 0;
                DataSaver.DS.table.Add(a);
                Frame.GoBack();
            }else
            {
                var msgbox = new MessageDialog("Введите название.");
                msgbox.Commands.Add(new UICommand(
                "Ok",
                new UICommandInvokedHandler(this.CommandInvokedHandler)));
                await msgbox.ShowAsync();
            }
        }

        public void CommandInvokedHandler(IUICommand command)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(PivotPage));
        }
    }

    public class DataSaver
    {
        public static DataSaver ds=null;
        public static DataSaver DS
        {
            get
            {
                if(ds==null)
                {
                    ds=new DataSaver();
                }
                return ds;
            }
        }
        public List<ProgrammatronProgram> table = new List<ProgrammatronProgram>();
        public async void save()
        {
            await writeXMLAsync();
        }

        public async void get()
        {
            await readXMLAsync();
        }

        private async Task writeXMLAsync()
        {
            var serializer = new DataContractSerializer(typeof(List<ProgrammatronProgram>));
            using (var stream = await ApplicationData.Current.LocalFolder.OpenStreamForWriteAsync(
                "data.xml", CreationCollisionOption.ReplaceExisting))
            {
                serializer.WriteObject(stream, table);
            }
        }
        private async Task readXMLAsync()
        {
            var serializer = new DataContractSerializer(typeof(List<ProgrammatronProgram>));

            // проверка наличия файла
            bool existed = await FileExists(ApplicationData.Current.LocalFolder, "data.xml");
            //restore();
            
            if (existed)
            {
                using (var stream = await ApplicationData.Current.LocalFolder.OpenStreamForReadAsync("data.xml"))
                {
                        table = (List<ProgrammatronProgram>)serializer.ReadObject(stream);
                }
            }
            else
            {
                restore();
            }
        }

        private async void restore()
        {
            var serializer1 = new DataContractSerializer(typeof(List<ProgrammatronProgram>));
            using (var stream1 = await ApplicationData.Current.LocalFolder.OpenStreamForWriteAsync(
                "data.xml", CreationCollisionOption.ReplaceExisting))
            {
                serializer1.WriteObject(stream1, table);
            }
        }
        // проверка наличия файла
        public async Task<bool> FileExists(StorageFolder folder, string fileName)
        {
            return (await folder.GetFilesAsync()).Any(x => x.Name == fileName);
        }
    }

    [DataContract(Name = "ProgrammatronProgram", Namespace = "local")]
    public class ProgrammatronProgram
    {
        [DataMember(Name = "NameProg")]
        public string Name 
        { 
            get; 
            set; 
        }
        [DataMember(Name = "Desrc")]
        public string Description
        {
            get;
            set;
        }
        [DataMember(Name = "Code")]
        public string Code
        {
            get;
            set;
        }

        public ProgrammatronProgram(string name,string description,string code)
        {
            Name = name;
            Description = description;
            Code = code;
        }

        public override string ToString()
        {
            return Name + "\n" + Description;
        }
        [DataMember(Name = "Id")]
        public int Id
        {
            get;
            set;
        }
    }
}
