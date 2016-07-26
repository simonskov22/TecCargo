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

namespace TecCargo_Dagbog.View
{
    /// <summary>
    /// Interaction logic for OpenList.xaml
    /// </summary>
    public partial class OpenList : Window
    {
        #region Global Variables

        List<Model.FileClass.fileInput> _files = new List<Model.FileClass.fileInput>();

        #endregion Global Variables
        
        public OpenList()
        {
            InitializeComponent();

            Model.FileClass function = new Model.FileClass();
            _files = function.GetFiles(); //hent dagbøger


            loadFilesList(""); //hvis alle dagbøger
        }

        #region Button Click Events

        //åben dagbog
        private void ButtonFileName_save_Click(object sender, RoutedEventArgs e)
        {
            int index = ListViewOpenFileNames.SelectedIndex;

            if (index != -1)
            {
                Inc.Settings.fileInput = _files[index];

                this.DialogResult = true;
            }
        }

        private void ButtonFileName_cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
        
        #endregion Button Click Events

        #region Events

        //hvis der er valgt en dagbog så skal man kunne klikke på knappen åben
        private void ListViewOpenFileNames_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TextBoxFileName_name.Content = (ListViewOpenFileNames.SelectedValue as fileinfo).name;

            if (ListViewOpenFileNames.SelectedIndex != -1)
            {
                ButtonFileName_save.IsEnabled = true;
            }
            else
            {
                ButtonFileName_save.IsEnabled = false;
            }
        }

        //åben hvis man dobbel klikker på en dagbog
        private void ListViewOpenFileNames_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            int index = ListViewOpenFileNames.SelectedIndex;

            if (index != -1)
            {
                Inc.Settings.fileInput = _files[index];


                this.DialogResult = true;
            }
        }

        //søg efter en dagbog
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string text = (sender as TextBox).Text.ToLower();
            loadFilesList(text);
        }

        #endregion Events

        #region Functions

        /// <summary>
        /// hvis kun bestemte dagbøger
        /// </summary>
        /// <param name="contains">Søg</param>
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

        class fileinfo
        {
            public string name { get; set; }
        }
    }
}
