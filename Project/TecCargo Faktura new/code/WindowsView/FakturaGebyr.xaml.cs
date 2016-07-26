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
    /// Interaction logic for FakturaGebyr.xaml
    /// </summary>
    public partial class FakturaGebyr : Window
    {

        public FakturaGebyr()
        {
            InitializeComponent();

            loadGebyr(true);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_Save(object sender, RoutedEventArgs e)
        {
            loadGebyr();

            this.DialogResult = true;
        }

        private void CheckBoxGebyr_Button_Click(object sender, RoutedEventArgs e)
        {
            int buttonId = int.Parse((sender as Button).Name.Replace("CheckBoxGebyr_Button_", ""));
            
            activeBools[buttonId] = !activeBools[buttonId];

            if (activeBools[buttonId])
            {
                (sender as Button).Background = Brushes.Green;
            }
            else
            {
                (sender as Button).Background = Brushes.Red;
            }

            loadGebyr();
        }

        private void loadGebyr(bool loadTextFirst = false) {
            int[] textboxFieldN = {
                 0,
                 1,
                 6,
                 7,
                 9,
                 10,
                 11
            };
            if (loadTextFirst)
            {
                goto SetTextbox;
            }

            Gebyrbool:

            for (int i = 0; i < activeBools.Count(); i++)
            {
                if (activeBools[i])
                {
                    (FindName("CheckBoxGebyr_Button_" + i) as Button).Background = Brushes.Green;
                    
                    for (int a = 0; a < textboxFieldN.Count(); a++)
                    {
                        if (textboxFieldN[a] == i)
                        {
                            Values[a] = (FindName("CheckBoxGebyr_TextBox_" + i) as TextBox).Text;
                            (FindName("CheckBoxGebyr_TextBox_" + i) as TextBox).IsEnabled = true;
                        }
                    }
                }
                else
                {
                    (FindName("CheckBoxGebyr_Button_" + i) as Button).Background = Brushes.Red;

                    for (int a = 0; a < textboxFieldN.Count(); a++)
                    {
                        if (textboxFieldN[a] == i)
                        {
                            Values[a] = "0";
                            (FindName("CheckBoxGebyr_TextBox_" + i) as TextBox).IsEnabled = false;
                        }
                    }
                }
            }

            SetTextbox:
            for (int i = 0; i < textboxFieldN.Count(); i++)
            {
                (FindName("CheckBoxGebyr_TextBox_" + textboxFieldN[i]) as TextBox).Text = Values[i];
            }

            if (loadTextFirst)
            {
                loadTextFirst = false;
                goto Gebyrbool;
            }
        }

        public bool[] activeBools = {  
                Models.FakturaPrisliste.EkstraGebyr.chauffoer,
                Models.FakturaPrisliste.EkstraGebyr.flytteTilaeg,
                Models.FakturaPrisliste.EkstraGebyr.adrTilaeg,
                Models.FakturaPrisliste.EkstraGebyr.aftenNat,
                Models.FakturaPrisliste.EkstraGebyr.weekend,
                Models.FakturaPrisliste.EkstraGebyr.yederzone,
                Models.FakturaPrisliste.EkstraGebyr.byttePalle,
                Models.FakturaPrisliste.EkstraGebyr.smsService,
                Models.FakturaPrisliste.EkstraGebyr.adresseKorrektion,
                Models.FakturaPrisliste.EkstraGebyr.broAfgrif,
                Models.FakturaPrisliste.EkstraGebyr.vejAfgrif,
                Models.FakturaPrisliste.EkstraGebyr.faergeAfgrif
            };

        public string[] Values = {
            Models.FakturaPrisliste.EkstraGebyr.medHelper.ToString(),
            Models.FakturaPrisliste.EkstraGebyr.flyttePrEnhed.ToString(),
            Models.FakturaPrisliste.EkstraGebyr.byttePallePrPalle.ToString(),
            Models.FakturaPrisliste.EkstraGebyr.smsAdvisering.ToString(),
            Models.FakturaPrisliste.EkstraGebyr.broAfgrifD.ToString(),
            Models.FakturaPrisliste.EkstraGebyr.vejAfgrifD.ToString(),
            Models.FakturaPrisliste.EkstraGebyr.faergeAfgrifD.ToString()
        };
    }
}

