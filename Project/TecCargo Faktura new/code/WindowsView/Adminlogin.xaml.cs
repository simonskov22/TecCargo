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

namespace TecCargo_Faktura.WindowsView
{
    /// <summary>
    /// Interaction logic for Adminlogin.xaml
    /// </summary>
    public partial class Adminlogin : Window
    {
        public Adminlogin()
        {
            InitializeComponent();
        }
        private void Button_Click_Login(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void Button_Click_Cancel(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            TextboxAdmin_Username.Text = Models.ImportantData.AdminUsername;
            TextboxAdmin_Password.Password = Models.ImportantData.AdminPassword;

            this.DialogResult = true;
        }
    }
}
