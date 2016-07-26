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
    /// Interaction logic for AdminKode.xaml
    /// </summary>
    public partial class AdminKode : Window
    {
        public AdminKode(string ekstraText = "")
        {
            InitializeComponent();
            labelInfoText.Content = ekstraText;
        }

        private void SaveFinishTextButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
