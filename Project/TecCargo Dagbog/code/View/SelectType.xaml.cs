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
    /// Interaction logic for SelectType.xaml
    /// </summary>
    public partial class SelectType : Window
    {
        #region Global Variables
        
        //om man skal have mulighed for at forsætte med det man er igan med
        public bool showContinueButton = false;

        //om den skal return en værdi
        public bool IsDialog = false;

        #endregion

        public SelectType()
        {
            InitializeComponent();
        }

        #region Button Click Event

        private void Button_Continue_Click(object sender, RoutedEventArgs e)
        {
            CloseStatus(false);
        }
        private void Button_CreateNew_Click(object sender, RoutedEventArgs e)
        {

            Inc.Settings.fileInput = new Model.FileClass.fileInput();
            MainWindow mainView = new MainWindow();
            mainView.Show();

            CloseStatus(true);
        }
        private void Button_OpenSave_Click(object sender, RoutedEventArgs e)
        {
            View.OpenList openlist = new OpenList();
            openlist.Owner = this;
            openlist.ShowDialog();

            if (openlist.DialogResult.HasValue && openlist.DialogResult.Value)
            {
                MainWindow mainView = new MainWindow();
                mainView.LoadFromFile = true;
                mainView.Show();
                CloseStatus(true);
            }
        }
        private void Button_ShowSave_Click(object sender, RoutedEventArgs e)
        {
            View.OpenList openlist = new OpenList();
            openlist.Owner = this;
            openlist.ShowDialog();

            if (openlist.DialogResult.HasValue && openlist.DialogResult.Value)
            {
                View.ShowFile showFile = new ShowFile();
                showFile.Show();
                CloseStatus(true);
            }
        }

        private void Button_Close_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        #endregion Button Click Event

        #region Functions

        /// <summary>
        /// hvordan den skal lukke dette vindue
        /// 
        /// hvis this.IsDialog er true bliver den lukket som en dialog
        /// </summary>
        private void CloseStatus(bool value) 
        {
            if(this.IsDialog){
                this.DialogResult = value;
            }
            else
            {
                this.Close();
            }
        }


        #endregion Functions

        #region Events

        //om man skal kunne se forsæt knap
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.showContinueButton)
            {
                button_showContinue.Visibility = Visibility.Visible;
            }
        }

        #endregion Events

        
    }
}
