using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;

namespace TecCargo_Læreplads_DB.View
{
    /// <summary>
    /// Interaction logic for OpenList.xaml
    /// </summary>
    public partial class OpenList : Window
    {
        #region Global Variables

        List<Model.FileClass.fileInput> _files = new List<Model.FileClass.fileInput>();

        #endregion Global Variables

        #region Button Click Events

        //åben læreplads
        private void ButtonFileName_save_Click(object sender, RoutedEventArgs e)
        {

            int index = ListViewOpenFileNames.SelectedIndex;

            if(index != -1)
            {
                Inc.Settings.fileInput = _files[index];

                this.DialogResult = true;
            }
        }

        private void ButtonFileName_cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        //slet valgte læreplads
        private void ListViewButton_Delete_Click(object sender, RoutedEventArgs e)
        {
            //hent id
            int index = ListViewOpenFileNames.Items.IndexOf((sender as Button).DataContext);

            //hvis den findes slet fil
            if (index != -1 && index <= ListViewOpenFileNames.Items.Count)
            {
                string pathAndFilename = Directory.GetCurrentDirectory() + @"\Saves\" + _files[index].filename;
                string pathFile = Directory.GetCurrentDirectory() + @"\Files\";

                if (File.Exists(pathAndFilename))
                {

                    foreach (var file in _files[index].files)
                    {
                        if (!file.link && File.Exists(pathFile + file.path))
                        {
                            File.Delete(pathFile + file.path);
                        }
                    }

                    File.Delete(pathAndFilename);
                    _files.RemoveAt(index);
                    loadFilesList(TextBox_Search.Text);
                }
            }
        }
    
        #endregion Button Click Events
        
        #region Events
        //hvis der er valgt en læreplad så skal man kunne klikke på knappen åben
        private void ListViewOpenFileNames_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = ListViewOpenFileNames.SelectedIndex;

            if (index > -1 && index < ListViewOpenFileNames.Items.Count)
            {
                TextBoxFileName_name.Content = (ListViewOpenFileNames.Items[index] as fileinfo).name;
                ButtonFileName_save.IsEnabled = true;
            }
            else
            {
                ButtonFileName_save.IsEnabled = false;
            }
        }

        //åben hvis man dobbel klikker på en læreplads
        private void ListViewOpenFileNames_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            int index = ListViewOpenFileNames.SelectedIndex;

            if (index != -1)
            {
                Inc.Settings.fileInput = _files[index];


                this.DialogResult = true;
            }
        }

        //søg efter en læreplads
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string text = (sender as TextBox).Text.ToLower();
            loadFilesList(text);
        }

        #endregion Events
        
        #region Functions

        /// <summary>
        /// hvis kun bestemte lærepads
        /// </summary>
        /// <param name="contains">fil navn</param>
        private void loadFilesList(string contains)
        {
            ListViewOpenFileNames.Items.Clear();

            for (int i = 0; i < _files.Count; i++)
            {

                if (_files[i].name.ToLower().Contains(contains))
                {
                    fileinfo info = new fileinfo();
                    info.name = _files[i].name;
                    if (info.name.Length == 0)
                    {
                        info.name = "Intet navn";
                    }

                    ListViewOpenFileNames.Items.Add(info);
                }

            }
        }

        #endregion Functions

        public OpenList()
        {
            InitializeComponent();

            Model.FileClass function = new Model.FileClass();
            _files =  function.GetFiles();


            loadFilesList("");
        }

        class fileinfo {
            public string name { get; set; }
        }

       
    }
}
