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

namespace TecCargo_Læreplads_DB.View
{
    /// <summary>
    /// Interaction logic for SelectType.xaml
    /// </summary>
    public partial class SelectType : Window
    {

        #region Global Variables

        //om man skal have mulighed for at forsætte med det man er igan med
        public bool showContinueButton = false;

        #endregion Global Variables

        public SelectType()
        {
            InitializeComponent();

        }

        #region Button Click Event

        private void button_click_showfilelist(object sender, RoutedEventArgs e)
        {

            View.OpenList showList = new OpenList();
            showList.Owner = this;
            showList.ShowDialog();
            if (showList.DialogResult.HasValue && showList.DialogResult.Value) {

                this.DialogResult = true;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Inc.Settings.fileInput = new Model.FileClass.fileInput();
            this.DialogResult = true;
        }


        private void Button_Continue_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void Button_Close_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        #endregion Button Click Event

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.showContinueButton)
            {
                Button_Continue.Visibility = Visibility.Visible;
            }
        }

    }
}
