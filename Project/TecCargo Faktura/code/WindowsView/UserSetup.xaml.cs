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
    /// Interaction logic for UserSetup.xaml
    /// </summary>
    public partial class UserSetup : Window
    {
        public string initialerCont = "";

        public UserSetup()
        {
            InitializeComponent();

            TextBox_Initialer_0.Focus();
        }

        /// <summary>
        /// når man gemmer sine initialer
        /// skal den tjek om de er med store bogstaver og
        /// initialeren er på minimun 2 bogstaver
        /// </summary>
        private void Button_InitialerSave_Click(object sender, RoutedEventArgs e)
        {
            //vær sikker på at den er tom til at starte med
            initialerCont = "";
            //Hent bogstav fra textboxs
            for (int i = 0; i < 5; i++)
            {
                string input = (FindName("TextBox_Initialer_" + i) as TextBox).Text;

                if (input != null && input != "") //tilføj bogstav
                {
                    initialerCont += (FindName("TextBox_Initialer_" + i) as TextBox).Text.ToUpper();
                }
            }

            //tjek om der er min 2 bogstaver og max 5
            if (initialerCont.Length >= 2 && initialerCont.Length <= 5)
            {

                this.DialogResult = true;   
            }
        }

        /// <summary>
        /// når man skriver i en tekstbox skal den skifte til den næste
        /// og hvis man sletter en skal den slette resten til højre
        /// </summary>
        private void TextBox_Initialer_TextChanged(object sender, TextChangedEventArgs e)
        {
            if ((sender as TextBox).IsFocused) //skal kun virke på den man står på
            {
                //Find Id
                int textboxId = int.Parse((sender as TextBox).Name.Replace("TextBox_Initialer_", ""));


                //så der kun er et bogstav i textboxen
                if ((sender as TextBox).Text.Length > 1)
                {
                    (sender as TextBox).Text = (sender as TextBox).Text.Substring(0, 1);
                }

                //åben den næste textbox
                if ((sender as TextBox).Text.Length == 1 && textboxId != 4)
                {
                    string NextTextBoxName = "TextBox_Initialer_" + (textboxId + 1);
                    (FindName(NextTextBoxName) as TextBox).IsEnabled = true;
                    (FindName(NextTextBoxName) as TextBox).Focus();
                }
                else //luk for de ændre
                {
                    for (int i = textboxId + 1; i < 4; i++)
                    {
                        (FindName("TextBox_Initialer_" + i) as TextBox).IsEnabled = false;
                        (FindName("TextBox_Initialer_" + i) as TextBox).Text = "";

                    }
                }

                //sidste felt kun tal
                if (textboxId == 4)
                {
                    try
                    {
                        (sender as TextBox).Text = int.Parse((sender as TextBox).Text).ToString();
                        Button_Initialer_Save.Focus();
                    }
                    catch (Exception)
                    {
                        (sender as TextBox).Text = "";
                    }
                }

                //hvis tekstbox 3(2) er åben tillad gem (min 2 bogstaver)
                Button_Initialer_Save.IsEnabled = TextBox_Initialer_2.IsEnabled;

                //gør text upper case
                (sender as TextBox).Text = (sender as TextBox).Text.ToUpper();
            }
        }
    }
}
