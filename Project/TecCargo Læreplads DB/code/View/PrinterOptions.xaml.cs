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
using System.Drawing.Printing;
using System.IO;
using RawPrint;

namespace TecCargo_Læreplads_DB.View
{
    /// <summary>
    /// Interaction logic for PrinterOptions.xaml
    /// </summary>
    public partial class PrinterOptions : Window
    {
        public PrinterOptions()
        {
            InitializeComponent();
        }

        #region Button Click Events

        private void Button_Print_Click(object sender, RoutedEventArgs e)
        {
            Model.FilePDF funcPDF = new Model.FilePDF();

            int maxItems = ComboBox_Printer.Items.Count;     //Antal printer fundet
            int index = ComboBox_Printer.SelectedIndex;     //valgte printer id

            //check om valgte printer id findes og om antal kopirer er et tal
            if (index > -1 && index < maxItems && IsNumeric(Textbox_Copy.Text))
            {
                //gem som favorit printer
                Inc.Settings.Gerenal.printer = ComboBox_Printer.Items[index].ToString();
                Model.FileClass.SaveGeneral();//gem general indstiliger

                funcPDF.CreateDocument();//opret pdf version af læreplads log

                //tjek at den er oprettet
                string FilePath = Directory.GetCurrentDirectory() + "\\" + Model.FilePDF.FileName;

                if (File.Exists(FilePath))
                {
                    int copyCount = int.Parse(Textbox_Copy.Text); //antal kopirer

                    //print pdf version
                    for (int i = 0; i < copyCount; i++)
                    {
                        Printer.PrintFile(Inc.Settings.Gerenal.printer, FilePath, "TECcargo Læreplads");
                    }

                    File.Delete(FilePath);//slet pdf version
                    this.DialogResult = true;//afslut vindue
                }
            }
        }

        private void Button_Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
        
        //Ændre antal kopirer
        private void Button_CopyChangeNumber_Click(object sender, RoutedEventArgs e)
        {
            Button thisButton = (sender as Button);
            int numberNow = 1;

            if (IsNumeric(Textbox_Copy.Text))
            {
                numberNow = int.Parse(Textbox_Copy.Text);

                if (thisButton.Content.ToString() == "5")
                {
                    numberNow++;
                }
                else
                {
                    if (numberNow > 1)
                    {
                        numberNow--;
                    }
                }
            }

            Textbox_Copy.Text = numberNow.ToString();
        }

        #endregion Button Click Events

        #region Events

        //Sæt farvorit printer
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            int selectedPrinter = 0;

            for (int i = 0; i < PrinterSettings.InstalledPrinters.Count; i++)
            {
                if (PrinterSettings.InstalledPrinters[i] == Inc.Settings.Gerenal.printer)
                {
                    selectedPrinter = i;
                }

                ComboBox_Printer.Items.Add(PrinterSettings.InstalledPrinters[i]);
            }
            ComboBox_Printer.SelectedIndex = selectedPrinter;
        }

        //gør så man kun kan skrive tal i antal kopirer
        private void Textbox_Copy_KeyUp(object sender, KeyEventArgs e)
        {

            string text = Textbox_Copy.Text;

            for (int i = 1; i < text.Length; i++)
            {

                if (!IsNumeric(text.Substring(0, i)))
                {
                    Textbox_Copy.Text = text.Substring(0, i - 1);
                    break;
                }
            }

        }
        
        //gør så man kun kan skrive tal i antal kopirer
        private void Textbox_Copy_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Textbox_Copy.Text.Length == 0)
            {
                Textbox_Copy.Text = "1";
            }
        }
        
        #endregion Events

        #region Functions

        /// <summary>
        /// tjekker om en string er et tal i int format
        /// </summary>
        private bool IsNumeric(string text)
        {
            string letterAllow = "0123456789";

            for (int i = 0; i < text.Length; i++)
            {
                if (!letterAllow.Contains(text.Substring(i, 1)))
                {
                    return false;
                }
            }

            return true;
        }
        #endregion Functions

    }
}
