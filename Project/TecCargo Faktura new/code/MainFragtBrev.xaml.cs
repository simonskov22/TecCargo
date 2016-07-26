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
using System.Data;

namespace TecCargo_Faktura
{
    /// <summary>
    /// Interaction logic for MainFragtBrev.xaml
    /// </summary>
    public partial class MainFragtBrev : Window
    {
        
        private List<string> creatorId = new List<string>();
        private bool newFragtbrev = false;
        private int thisInvoice = 1000;

        public MainFragtBrev(string creatorId, bool closeFragtBrev = false)
        {
            InitializeComponent();

            this.creatorId.Add(creatorId);

            //update save status
            Models.ImportantData.FileIsSaved = true;
            SaveStatus();

            //sæt default
            DatePickerGenerelt_Dato.SelectedDate = DateTime.Now;
            DatePickerGenerelt_Tid.Value = DateTime.Now;

            //load datagrid indhold
            //loadGodslinjeliste();

            DataContext = new Models.DataGridSources();
            //(DataGridGodslinjer.Columns[2] as DataGridComboBoxColumn).ItemsSource = (new string[] { "TDR", "PLL", "CLL", "PKK", "IBC" }).ToList();

            LabelVersion_version.Content += Models.ImportantData.PVersion.ToString();

            //om den læsser fra en fil eller laver en ny
            if (Models.ImportantData.LoadFormFile)
            {
                ReadSaveXML(Models.ImportantData.Filename);
            }
            else
            {
                string settingsFileName = Models.ImportantData.g_FolderDB + "Settings.xml";
                FileStream settingsFileR = new FileStream(settingsFileName, FileMode.Open, FileAccess.Read);
                DataSet settingsData = new DataSet();
                settingsData.ReadXml(settingsFileR);
                settingsFileR.Close();

                int invoice = int.Parse(settingsData.Tables[0].Rows[0]["NextInvoice"].ToString());
                //være sikker på at invoice ikke er brugt
                int fragtCount = Directory.GetFiles(Models.ImportantData.g_FolderSave).Length;

                for (int i = 0; i < fragtCount; i++)
                {
                    if (!File.Exists(Models.ImportantData.g_FolderSave + "Fragtbrev-" +invoice+ ".xml"))
                    {
                        break;
                    }
                    else
                    {
                        invoice++;
                    }
                }

                //set det nye navn
                Models.ImportantData.Filename = "Fragtbrev-" + invoice;

                //bruges til når der skal gemmes
                this.newFragtbrev = true;
            }

            //vis nanvet på filen i toppen af programmet
            this.Title  += " - " + Models.ImportantData.Filename;
            this.thisInvoice = int.Parse(Models.ImportantData.Filename.Replace("Fragtbrev-", ""));

            //MessageBox.Show(Models.ImportantData.UsePdfBold.ToString());
            CloseFragtbrevStatus();


            if (closeFragtBrev)
            {
                StatusCloseFragt();
            }
        }

        #region Information grup

        private void CheckboxInformation_Checked(object sender, RoutedEventArgs e)
        {
            if ((sender as CheckBox).IsChecked.Value && (sender as CheckBox).Name == CheckBoxAfsend_pay.Name)
            {
                CheckBoxModtag_pay.IsChecked = false;
                CheckBoxAndenB_pay.IsChecked = false;
            }
            else if ((sender as CheckBox).IsChecked.Value && (sender as CheckBox).Name == CheckBoxModtag_pay.Name)
            {
                CheckBoxAfsend_pay.IsChecked = false;
                CheckBoxAndenB_pay.IsChecked = false;
            }
            else if ((sender as CheckBox).IsChecked.Value && (sender as CheckBox).Name == CheckBoxAndenB_pay.Name)
            {
                CheckBoxAfsend_pay.IsChecked = false;
                CheckBoxModtag_pay.IsChecked = false;
            }
        }

        private void SetInformationList(string textInput, object listBoxObject, object[] textBoxFocus) 
        {
            string textInputNoSpace = textInput.Replace(" ", "");

            Class.XML_Files.Customer funcCustom = new Class.XML_Files.Customer();
            List<Class.XML_Files.Customer.Layout> CustomerList = funcCustom.ReadCustomer();
            List<string> ListOfCoustomerNames = new List<string>();

            for (int i = 0; i < CustomerList.Count; i++)
            {
                string konIdNoSpace = CustomerList[i].ContactTlf.Replace(" ", "");
                string firmaNoSpace = CustomerList[i].FirmName.Replace(" ", "");

                if (((textBoxFocus[0] as TextBox).IsFocused &&
                    konIdNoSpace.ToLower().StartsWith(textInputNoSpace.ToLower())) ||
                    ((textBoxFocus[1] as TextBox).IsFocused &&
                    firmaNoSpace.ToLower().StartsWith(textInputNoSpace.ToLower())))
                {
                    ListOfCoustomerNames.Add(CustomerList[i].FirmName);
                }
            }

            //tilføj firma/navne til sourcelist
            //hvis den ikke er tom
            if (ListOfCoustomerNames.Count != 0)
            { 
               (listBoxObject as ListBox).ItemsSource = ListOfCoustomerNames;
               (listBoxObject as ListBox).Visibility = Visibility.Visible;
            }
            else
            {
                (listBoxObject as ListBox).Visibility = Visibility.Hidden;
            }
        }
        private void TextBoxInformationFirmalist_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (((TextBox)sender).Name == TextBoxAfsend_customId.Name ||
                ((TextBox)sender).Name == TextBoxAfsend_firma.Name)
            {
                object[] textboxs = {
                    TextBoxAfsend_customId,
                    TextBoxAfsend_firma
                };

                SetInformationList(((TextBox)sender).Text, ListBoxAfsend_firmalist, textboxs);
            }
            else if (((TextBox)sender).Name == TextBoxModtag_customId.Name ||
                    ((TextBox)sender).Name == TextBoxModtag_firma.Name)
            {
                object[] textboxs = {
                    TextBoxModtag_customId,
                    TextBoxModtag_firma
                };
                SetInformationList(((TextBox)sender).Text, ListBoxModtag_firmalist, textboxs);
	        }
            else if (((TextBox)sender).Name == TextBoxAndenB_customId.Name ||
                    ((TextBox)sender).Name == TextBoxAndenB_firma.Name)
            {
                object[] textboxs = {
                    TextBoxAndenB_customId,
                    TextBoxAfsend_firma
                };
                SetInformationList(((TextBox)sender).Text, ListBoxAndenB_firmalist, textboxs);
	        }
        }

        private void TextBoxInformationKeyUpFunc (KeyEventArgs e, object listBoxObject, object[] setTextBoxs) 
        {
            if (e.Key == Key.Up || e.Key == Key.Down || e.Key == Key.Enter)
            {
                //hent den valgte og den højeste id på listen
                int selectNow = (listBoxObject as ListBox).SelectedIndex;
                int selectMax = (listBoxObject as ListBox).Items.Count - 1;

                if (e.Key == Key.Up)
                {
                    //så længe man ikke er på 0 kan man gå op
                    if (selectNow > 0)
                    {
                        (listBoxObject as ListBox).SelectedIndex = selectNow - 1;
                    }
                }
                else if (e.Key == Key.Down)
                {
                    //hvis man ikke valgt den højste kan man godt gå en tand ned
                    if (selectNow != selectMax)
                    {
                        (listBoxObject as ListBox).SelectedIndex = selectNow + 1;
                    }

                }
                //hvis man trykker på enter skal den hente oplysninger fra den valgte på listen
                else if (e.Key == Key.Enter)
                {
                    if (selectNow != -1 && (listBoxObject as ListBox).Visibility == Visibility.Visible)
                    {
                        Class.XML_Files.Customer funcCustom = new Class.XML_Files.Customer();
                        funcCustom.SetTextBox(
                            setTextBoxs,
                            (listBoxObject as ListBox).SelectedValue.ToString()
                        );
                        (listBoxObject as ListBox).Visibility = Visibility.Hidden;
                    }
                }
            }

                //esc fjerner listen
            else if (e.Key == Key.Escape)
            {
                (listBoxObject as ListBox).Visibility = Visibility.Hidden;
            }
        }
        private void TextBoxInformationFirmalist_KeyUp(object sender, KeyEventArgs e)
        {
            if (((TextBox)sender).Name == TextBoxAfsend_customId.Name ||
                ((TextBox)sender).Name == TextBoxAfsend_firma.Name)
            {
                object[] textboxs = {
                                TextBoxAfsend_customId,
                                TextBoxAfsend_firma,
                                TextBoxAfsend_pKont,
                                TextBoxAfsend_adresse,
                                TextBoxAfsend_post
                };

                TextBoxInformationKeyUpFunc(e, ListBoxAfsend_firmalist, textboxs);
            }
            else if (((TextBox)sender).Name == TextBoxModtag_customId.Name ||
                    ((TextBox)sender).Name == TextBoxModtag_firma.Name)
	        {
                object[] textboxs = {
                                TextBoxModtag_customId,
                                TextBoxModtag_firma,
                                TextBoxModtag_pKont,
                                TextBoxModtag_adresse,
                                TextBoxModtag_post
                };

        		 TextBoxInformationKeyUpFunc(e, ListBoxModtag_firmalist, textboxs);
	        }
            else if (((TextBox)sender).Name == TextBoxAndenB_customId.Name ||
                    ((TextBox)sender).Name == TextBoxAndenB_firma.Name)
            {
                object[] textboxs = {
                                TextBoxAndenB_customId,
                                TextBoxAndenB_firma,
                                TextBoxAndenB_pKont,
                                TextBoxAndenB_adresse,
                                TextBoxAndenB_post
                };
		        TextBoxInformationKeyUpFunc(e, ListBoxAndenB_firmalist, textboxs);
	        }
        }

        private void TextBoxInformationFirmalist_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if ((sender as ListBox).SelectedIndex != -1)
            {
                Class.XML_Files.Customer funcCustom = new Class.XML_Files.Customer();
                if (((ListBox)sender).Name == ListBoxAfsend_firmalist.Name)
                {
                    object[] textboxs = {
                        TextBoxAfsend_customId,
                        TextBoxAfsend_firma,
                        TextBoxAfsend_pKont,
                        TextBoxAfsend_adresse,
                        TextBoxAfsend_post
                    };

                    funcCustom.SetTextBox(
                        textboxs,
                        ListBoxAfsend_firmalist.SelectedValue.ToString()
                    );

                }
                else if (((ListBox)sender).Name == ListBoxModtag_firmalist.Name)
                {
                    object[] textboxs = {
                        TextBoxModtag_customId,
                        TextBoxModtag_firma,
                        TextBoxModtag_pKont,
                        TextBoxModtag_adresse,
                        TextBoxModtag_post
                    };

                    funcCustom.SetTextBox(
                        textboxs,
                        ListBoxModtag_firmalist.SelectedValue.ToString()
                    );
                }
                else if (((ListBox)sender).Name == ListBoxAndenB_firmalist.Name)
                {
                    object[] textboxs = {
                        TextBoxAndenB_customId,
                        TextBoxAndenB_firma,
                        TextBoxAndenB_pKont,
                        TextBoxAndenB_adresse,
                        TextBoxAndenB_post
                    };
                    funcCustom.SetTextBox(
                        textboxs,
                        ListBoxAndenB_firmalist.SelectedValue.ToString()
                    );
                }

               
            }

            ListBoxAfsend_firmalist.Visibility = Visibility.Hidden;
            ListBoxModtag_firmalist.Visibility = Visibility.Hidden;
            ListBoxAndenB_firmalist.Visibility = Visibility.Hidden;
        }
        
        //bliver også brugt på modtager
        private void Window_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //lister for firma navne i afsender og modtager
            if (!ListBoxAfsend_firmalist.IsFocused || !ListBoxModtag_firmalist.IsFocused || !ListBoxAndenB_firmalist.IsFocused)
            {
                ListBoxAfsend_firmalist.Visibility = Visibility.Hidden;
                ListBoxModtag_firmalist.Visibility = Visibility.Hidden;
                ListBoxAndenB_firmalist.Visibility = Visibility.Hidden;
            }
        }
      
        private void SetInformationPostNummer(string inputText, object labelObject) 
        {
            //kun tjek i post hvis man har skrevet 4 tal
            if (inputText.Length == 4)
            {
                Class.Functions.others funcOthers = new Class.Functions.others();
                DataSet postNumbDataSet = funcOthers.ReadXmlFormDatabase("postnummerfil");

                int postNumbCount = postNumbDataSet.Tables[2].Rows.Count;

                //tjek om postnummere er ens og hent navnet på byen
                for (int i = 0; i < postNumbCount; i++)
                {
                    string dataPost = postNumbDataSet.Tables[2].Rows[i].ItemArray[0].ToString();

                    if (inputText == dataPost)
                    {
                        (labelObject as Label).Content = postNumbDataSet.Tables[2].Rows[i].ItemArray[1].ToString();
                        break;
                    }
                    else
                    {
                        (labelObject as Label).Content = "";
                    }
                }
            }
        }
        private void TextBoxInformationPost_TextChanged (object sender, TextChangedEventArgs e)
        {
            if (((TextBox)sender).Name == TextBoxAfsend_post.Name)
            {
                SetInformationPostNummer((sender as TextBox).Text, LabelAfsend_bynavn);
            }
            else if (((TextBox)sender).Name == TextBoxModtag_post.Name)
            {
                SetInformationPostNummer((sender as TextBox).Text, LabelModtag_bynavn);
            }
            else if (((TextBox)sender).Name == TextBoxAndenB_post.Name)
            {
                SetInformationPostNummer((sender as TextBox).Text, LabelAndenB_bynavn);
            }
            
        }

        #endregion

        #region Generelt groupbox indhold

        private void CheckBoxGenerelt_efterkrav_Click(object sender, RoutedEventArgs e)
        {
            if (CheckBoxGenerelt_efterkrav.IsChecked.Value)
            {
                TextBoxGenerelt_efterkrav.IsEnabled = true;
                TextBoxGenerelt_forsikring.IsEnabled = true;
                TextBoxGenerelt_praemie.IsEnabled = true;
                TextBoxGenerelt_ialt.IsEnabled = true;
            }
            else
            {
                TextBoxGenerelt_efterkrav.IsEnabled = false;
                TextBoxGenerelt_forsikring.IsEnabled = false;
                TextBoxGenerelt_praemie.IsEnabled = false;
                TextBoxGenerelt_ialt.IsEnabled = false;
            }
        }

        #endregion Generelt groupbox indhold

        #region Godslinjer

        //transport type vælg næste
        private void GodsDataGridComboBoxTransport_SelectNext_stat_1(object sender, SelectionChangedEventArgs e)
        {
            string[] godslinjerPakkeType = { "" };

            int selectIitemsId = ((ComboBox)sender).SelectedIndex;

            switch (selectIitemsId)
            {
                case 0:
                    godslinjerPakkeType = new string[] { "Ingen", "GoRush", "GoFlex", "GoVIP" };
                    break;
                case 1:
                    godslinjerPakkeType = new string[] { "Ingen", "GoPlus", "GoGreen" };
                    break;
                case 2:
                    godslinjerPakkeType = new string[] { "Ingen", "GoFull", "GoPart" };
                    break;
            }


            ComboBoxGodslinjer_Pakketype.Visibility = Visibility.Visible;
            ComboBoxGodslinjer_Pakketype.ItemsSource = godslinjerPakkeType.ToList();
            ComboBoxGodslinjer_Pakketype.SelectedIndex = 0;
            DataGridGodslinjer.Visibility = Visibility.Hidden;
        }
        
        private void GodsDataGridComboBoxTransport_SelectNext_stat_2(object sender, SelectionChangedEventArgs e)
        {
            string[] godslinjerPakkeType = { "" };
            ComboBoxGodslinjer_Biltype.Visibility = Visibility.Hidden;

            int selectIitemsId = ComboBoxGodslinjer_TransportType.SelectedIndex;

            switch (selectIitemsId)
            {
                case 0:
                    if ((sender as ComboBox).SelectedIndex > 0)
                    {
                        ComboBoxGodslinjer_Biltype.Visibility = Visibility.Visible;
                        ComboBoxGodslinjer_Biltype.SelectedIndex = 0;
                        DataGridGodslinjer.Visibility = Visibility.Hidden;
                    }
                    return;
                case 1:
                    godslinjerPakkeType = new string[] { "XS", "S", "M", "L", "XL", "2XL", "3XL" };
                    break;
                case 2:
                    godslinjerPakkeType = new string[] { "LDM", "PLL", "M\u00b3", };
                    break;
                default:
                    MessageBox.Show(selectIitemsId.ToString());
                    break;
            }

            if (godslinjerPakkeType[0] == "")
            {
                DataGridGodslinjer.Visibility = Visibility.Hidden;
            }
            else
            {
                DataGridGodslinjer.Visibility = Visibility.Visible;
                loadGodslinjeliste();
            }
        }
        
        private void GodsDataGridComboBoxTransport_SelectNext_stat_3(object sender, SelectionChangedEventArgs e)
        {
            if ((sender as ComboBox).SelectedIndex > 0)
            {
                DataGridGodslinjer.Visibility = Visibility.Visible;
                loadGodslinjeliste();
            }
            else
            {
                DataGridGodslinjer.Visibility = Visibility.Hidden; 
            }
        }
        
        #endregion Godslinjer

        #region functions

        private void loadGodslinjeliste()
        {
            int transportType1 = ComboBoxGodslinjer_TransportType.SelectedIndex;
            //int transportType2 = ComboBoxGodslinjer_Pakketype.SelectedIndex;
            //int transportType3 = ComboBoxGodslinjer_Biltype.SelectedIndex;
            string[] transportSourceList = { "" };

            string[] headerNames = null;
            string[] bindingsName = null;
            bool[] isTextboxs = null;
            string[][] itemsSourceArray = null;
            double[] widthSize = null;
            bool[] showCombobox = null;
            SelectionChangedEventHandler[] comboboxSelectChange = null;

            switch (transportType1)
            {
                case 0:
                    headerNames = new string[] { "Mrk./Nr.", "Antal", "Art", "Indhold", "Vægt", "Rumfang (LxBxH)" };
                    bindingsName = new string[] { "AdresseNumber", "Antal", "Art", "Indhold", "Weight", "Rumfang" };
                    isTextboxs = new bool[] { true, true, false, true, true, true, true };
                    itemsSourceArray = new string[][] {
                                new string[] { "TDR","PLL","CLL","PKK","IBC" }
                            };
                    widthSize = new double[] { 150, 50, 60, 150, 70, 115 };
                    showCombobox = new bool[] { true, false };
                    comboboxSelectChange = new SelectionChangedEventHandler[] { null, null };
                    break;
                case 1: 

                    headerNames = new string[] { "Mrk./Nr.", "Antal", "Art", "Indhold", "Transport type", "Vægt", "Rumfang (LxBxH)" };
                    bindingsName = new string[] { "AdresseNumber", "Antal", "Art", "Indhold", "PakkeStr", "Weight", "Rumfang" };
                    isTextboxs = new bool[] { true, true, false, true, false, true, true };
                    itemsSourceArray = new string[][] {
                                new string[] { "TDR","PLL","CLL","PKK","IBC" },
                                new string[] { "XS", "S", "M", "L", "XL", "2XL", "3XL" }
                            };
                    widthSize = new double[] { 150, 50, 60, 150, 100, 70, 115 };
                    showCombobox = new bool[] { true, true };
                    comboboxSelectChange = new SelectionChangedEventHandler[] { null, null };
                    break;
                case 2:
                    headerNames = new string[] { "Mrk./Nr.", "Antal", "Art", "Indhold", "Transport type", "Vægt", "Rumfang (LxBxH)" };
                    bindingsName = new string[] { "AdresseNumber", "Antal", "Art", "Indhold", "PakkeStr", "Weight", "Rumfang" };
                    isTextboxs = new bool[] { true, true, false, true, false, true, true };
                    itemsSourceArray = new string[][] {
                                new string[] { "TDR","PLL","CLL","PKK","IBC" },
                                new string[] { "LDM", "PLL", "M\u00b3", }
                            };
                    widthSize = new double[] { 150, 70, 60, 150, 100, 70, 115 };
                    showCombobox = new bool[] { true, true };
                    comboboxSelectChange = new SelectionChangedEventHandler[] { null, null };
                    break;
                default:
                    MessageBox.Show(transportType1.ToString());
                    break;
            }
            Class.Functions.DatagridFunc funcDG = new Class.Functions.DatagridFunc();
            funcDG.DataGridPakkerLoadType(DataGridGodslinjer, headerNames, bindingsName, widthSize, itemsSourceArray, isTextboxs, showCombobox, comboboxSelectChange, null, new RoutedEventHandler(ButtonDG_Remove_Click));
        
        }

        private void ButtonDG_Remove_Click(object sender, RoutedEventArgs e)
        {
            Class.Functions.DatagridFunc funcDG = new Class.Functions.DatagridFunc();

            var item = ((Button)sender).DataContext;
            int Index = DataGridGodslinjer.Items.IndexOf(item);

            funcDG.RemoveItemDG(DataGridGodslinjer, Index);
        }

        //finder fragt nummeret fra navnet
        private string NewFragtbrevNumber()
        {
            //lænden på nummeret
            int nameOfNumberLen = Models.ImportantData.Filename.Length - 10;

            //kun nummeret
            string nameOfNumber = Models.ImportantData.Filename.Substring(10, nameOfNumberLen);

            return nameOfNumber;
        }
        
        
        #endregion functions

        #region toolbar
        
        #region function

        private void SaveStatus()
        {
            Class.Functions.others funcOthers = new Class.Functions.others();
            if (Models.ImportantData.FileIsSaved)
            {
                toolbarButton_save.IsEnabled = false;
                toolbarButton_saveImage.OpacityMask = funcOthers.ColorBrushHex("#7FFFFFFF");

                toolbarButtonListSave.IsEnabled = false;
            }
            else
            {
                toolbarButton_save.IsEnabled = true;
                toolbarButton_saveImage.OpacityMask = funcOthers.ColorBrushHex("#FFFFFFFF");

                toolbarButtonListSave.IsEnabled = true;
            }
        }

        ///<summary>
        ///tjek om man skal kunne oprette pdf fil
        /// </summary>
        private bool AllowPdfSave()
        {
            Class.Functions.DatagridFunc funcDG = new Class.Functions.DatagridFunc();
            //hvis false vil man ikke kunne gemme til pdf
            bool AllDone = true;

            string ErrorText = "## Fejl ##\n";

            //tjek afsender adresse
            if (TextBoxAfsend_adresse.Text == "" || TextBoxAfsend_customId.Text == "" ||
                TextBoxAfsend_firma.Text == "" || TextBoxAfsend_post.Text == "")
            {
                AllDone = false;
                ErrorText += "\nTjek Afsender.";
            }

            //tjek modtager adresse
            if (TextBoxModtag_adresse.Text == "" || TextBoxModtag_customId.Text == "" ||
                TextBoxModtag_firma.Text == "" || TextBoxModtag_post.Text == "")
            {
                AllDone = false;
                ErrorText += "\nTjek Modtager.";
            }

            //tjek om der er valgt en der betaler
            if (
                    (!CheckBoxAfsend_pay.IsChecked.Value && !CheckBoxModtag_pay.IsChecked.Value && 
                    buttonAndenB.Background == Brushes.Red)
                    ||
                    (!CheckBoxAfsend_pay.IsChecked.Value && !CheckBoxModtag_pay.IsChecked.Value && 
                    !CheckBoxAndenB_pay.IsChecked.Value && buttonAndenB.Background == Brushes.Green)
                )
            {
                AllDone = false;
                ErrorText += "\nTjek Hvem der betaler.";   
            }

            //tjek om genere info er sat
            if (TextBoxGenerelt_reference.Text == "" || TextBoxGenerelt_rute1.Text == "" ||
                DatePickerGenerelt_dag1.Text == "")
            {
                AllDone = false;
                ErrorText += "\nTjek Generelt.";
            }

            //tjek om efterkrav er sat
            if (CheckBoxGenerelt_efterkrav.IsChecked.Value)
            {
                if (TextBoxGenerelt_efterkrav.Text == "" || TextBoxGenerelt_forsikring.Text == "" ||
                    TextBoxGenerelt_ialt.Text == "" || TextBoxGenerelt_praemie.Text == "")
                {
                    AllDone = false;
                    ErrorText += "\nTjek Efterkrav.";
                }
            }

            bool transportTypeOk = true;
            if (ComboBoxGodslinjer_TransportType.SelectedIndex == -1 || ComboBoxGodslinjer_Pakketype.SelectedIndex == -1)
            {
                AllDone = false;
                ErrorText += "\nTjek Transport type.";   
                transportTypeOk = false;
            }

            //find ud af om er minimum en pakke

            int pakkeRowCount = DataGridGodslinjer.Items.Count;
            bool miniumOneRow = false;
            bool miniumOneRowNotDone = false;

            if (transportTypeOk)
            {
                switch (ComboBoxGodslinjer_TransportType.SelectedIndex)
                {
                    case 0: //Kurrer

                        for (int i = 0; i < pakkeRowCount - 1; i++)
                        {
                            string adresse, antal, art, indhold, veagt, rumfangt;

                            adresse = funcDG.GetCellValueTextBlock(DataGridGodslinjer, i, 1);
                            antal = funcDG.GetCellValueTextBlock(DataGridGodslinjer, i, 2);
                            art = funcDG.GetDgComboboxData(DataGridGodslinjer, i, 3, false);
                            indhold = funcDG.GetCellValueTextBlock(DataGridGodslinjer, i, 4);
                            veagt = funcDG.GetCellValueTextBlock(DataGridGodslinjer, i, 5);
                            rumfangt = funcDG.GetCellValueTextBlock(DataGridGodslinjer, i, 6);

                            //tjek om der er minimum en der er helt udflydt
                            if (adresse != "" && (antal != "" || antal != "0") && art != "" && indhold != "" &&
                               ((veagt != "" || veagt != "0") || rumfangt != ""))
                            {
                                miniumOneRow = true;
                            }
                            //hvis der er en pakke og der så er et flet der ikke er helt udflydt skal man kunne vælge mellem at forsætte og stoppe 
                            else if (miniumOneRow && (adresse == "" || (antal == "" || antal == "0") || art == ""
                                || indhold == "" || ((veagt == "" || veagt == "0")) || rumfangt == ""))
                            {
                                miniumOneRowNotDone = true;
                                break;
                            }
                        }
                        break;

                    case 1:  // pakke
                        for (int i = 0; i < pakkeRowCount - 1; i++)
                        {
                            string adresse, antal, art, indhold, transport, veagt, rumfangt;

                            adresse = funcDG.GetCellValueTextBlock(DataGridGodslinjer, i, 1);
                            antal = funcDG.GetCellValueTextBlock(DataGridGodslinjer, i, 2);
                            art = funcDG.GetDgComboboxData(DataGridGodslinjer, i, 3, false);
                            indhold = funcDG.GetCellValueTextBlock(DataGridGodslinjer, i, 4);
                            transport = funcDG.GetDgComboboxData(DataGridGodslinjer, i, 5, false);
                            veagt = funcDG.GetCellValueTextBlock(DataGridGodslinjer, i, 6);
                            rumfangt = funcDG.GetCellValueTextBlock(DataGridGodslinjer, i, 7);

                            //tjek om der er minimum en der er helt udflydt
                            if (adresse != "" && (antal != "" || antal != "0") && art != "" && indhold != "" &&
                                transport != "" && ((veagt != "" || veagt != "0") || rumfangt != ""))
                            {
                                miniumOneRow = true;
                            }
                            //hvis der er en pakke og der så er et flet der ikke er helt udflydt skal man kunne vælge mellem at forsætte og stoppe 
                            else if (miniumOneRow && (adresse == "" || (antal == "" || antal == "0") || art == ""
                                || indhold == "" || transport == "" || ((veagt == "" || veagt == "0")) || rumfangt == ""))
                            {
                                miniumOneRowNotDone = true;
                                break;
                            }
                        }
                        break;
                    case 2: //Gods
                        for (int i = 0; i < pakkeRowCount - 1; i++)
                        {
                            string adresse, antal, art, indhold, transport, veagt, rumfangt;

                            adresse = funcDG.GetCellValueTextBlock(DataGridGodslinjer, i, 1);
                            antal = funcDG.GetCellValueTextBlock(DataGridGodslinjer, i, 2);
                            art = funcDG.GetDgComboboxData(DataGridGodslinjer, i, 3, false);
                            indhold = funcDG.GetCellValueTextBlock(DataGridGodslinjer, i, 4);
                            transport = funcDG.GetDgComboboxData(DataGridGodslinjer, i, 5, false);
                            veagt = funcDG.GetCellValueTextBlock(DataGridGodslinjer, i, 6);
                            rumfangt = funcDG.GetCellValueTextBlock(DataGridGodslinjer, i, 7);

                            //tjek om der er minimum en der er helt udflydt
                            if (!miniumOneRow && (adresse != "" &&
                                !(antal == "" || antal == "0") &&
                                art != "" && indhold != "" &&
                                transport != "" && ((veagt != "" || veagt != "0") && rumfangt != "")))
                            {
                                miniumOneRow = true;
                            }
                            //hvis der er en pakke og der så er et flet der ikke er helt udflydt skal man kunne vælge mellem at forsætte og stoppe 
                            else if (miniumOneRow && (adresse == "" || art == ""
                                || indhold == "" || transport == "" || ((veagt == "" || veagt == "0")) || rumfangt == ""))
                            {
                                miniumOneRowNotDone = true;
                                break;
                            }
                        }

                        break;
                }

                //hvis der ikke er minimun en row
                if (!miniumOneRow)
                {
                    AllDone = false;
                    ErrorText += "\nTjek Godslinjer.";
                }
            }

            if (AllDone)
            {
                //hvis der er fleter i godslinjer/pakker der ikke er færdige skal man have muligheden for at stoppe og ændre dem
                if (miniumOneRowNotDone)
                {
                    MessageBoxResult result = MessageBox.Show("Ikke alle felter er udflydt i godslinjer.\nVil du forsætte?", "Opret PDF", MessageBoxButton.YesNo);
                    if (result == MessageBoxResult.Yes)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return true;
                }
            }
            else
            {
                MessageBox.Show(ErrorText);
                return false;
            }


            
        }

        //resetter alle inpus
        private void ResetFragtbrevInpus() 
        {
            if (Models.ImportantData.FileIsSaved)
            {
                MessageBoxResult result = MessageBox.Show("Opret nyt fragtbrev?", "Nyt Fragtbrev", MessageBoxButton.YesNo,MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    SaveToEditXML();
                }
                else
                {
                    return;
                }
            }
            else
            {
                MessageBoxResult result = MessageBox.Show("Gem nuværende fragtbrev?", "Gem", MessageBoxButton.YesNoCancel,MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    SaveToEditXML();
                }
                else if (result == MessageBoxResult.Cancel)
                {
                    return;
                }
            }
            //så det bliver en nyt fragt brev
            Models.ImportantData.Filename = "";
            Models.ImportantData.LoadFormFile = false;

            Models.ImportantData.openStartMenuOnClose = false;

            //vis fragtbrev og luk start menu
            WindowsView.UserSetup addIni = new WindowsView.UserSetup();
            addIni.ShowDialog();

            if (addIni.DialogResult.HasValue && addIni.DialogResult.Value)
            {
                MainFragtBrev FragtbrevWindow = new MainFragtBrev(addIni.initialerCont);

                try
                {
                    FragtbrevWindow.Show();
                    this.Close();
                }
                catch (Exception)
                {
                }
            }


        }
        
        ///<summary>
        ///Gem oplysninger til xml fil så man kan forsætte på filen
        /// </summary>
        public void SaveToEditXML()
        {
            Class.Functions.others funcOthers = new Class.Functions.others();
            Class.Functions.DatagridFunc funcDG = new Class.Functions.DatagridFunc();
            Class.XML_Files.Fragtbrev.Layout fragtInfo = new Class.XML_Files.Fragtbrev.Layout();
            Class.XML_Files.Fragtbrev fragtFunction = new Class.XML_Files.Fragtbrev();

            //Gem næste invoice hvis dette er en ny fil
            if (this.newFragtbrev)
            {
                string settingsFileName = Models.ImportantData.g_FolderDB + "Settings.xml";
                FileStream settingsFileR = new FileStream(settingsFileName, FileMode.Open, FileAccess.Read);
                DataSet settingsData = new DataSet();
                settingsData.ReadXml(settingsFileR);
                settingsFileR.Close();

                //Gem næste Invoice nummer
                settingsData.Tables[0].Rows[0]["NextInvoice"] = this.thisInvoice + 1;
                FileStream settingsFileW = new FileStream(settingsFileName, FileMode.Create, FileAccess.Write);
                settingsData.WriteXml(settingsFileW);
                settingsFileW.Close();

                this.newFragtbrev = false;
            }

            #region Default

            string EkstraComment = new TextRange(RichTextBoxEktra_Comment.Document.ContentStart, RichTextBoxEktra_Comment.Document.ContentEnd).Text;
            fragtInfo.CommentTekst = EkstraComment;

            //Initialer
            for (int i = 0; i < this.creatorId.Count; i++)
            {
                if (!fragtInfo.Owners.Contains(this.creatorId[i]))
                {
                    fragtInfo.Owners.Add(this.creatorId[i]);
                }
                
            }

            #endregion

            #region Adresse
            
            fragtInfo.Afsender.Kontakt          =   TextBoxAfsend_customId.Text;
            fragtInfo.Afsender.Firma            =   TextBoxAfsend_firma.Text;
            fragtInfo.Afsender.Adresse          =   TextBoxAfsend_adresse.Text;
            fragtInfo.Afsender.Post             =   TextBoxAfsend_post.Text;
            fragtInfo.Afsender.Betaler          =   CheckBoxAfsend_pay.IsChecked.Value;
            fragtInfo.Afsender.KontaktPerson    =   TextBoxAfsend_pKont.Text;

            fragtInfo.Modtager.Kontakt          =   TextBoxModtag_customId.Text;
            fragtInfo.Modtager.Firma            =   TextBoxModtag_firma.Text;
            fragtInfo.Modtager.Adresse          =   TextBoxModtag_adresse.Text;
            fragtInfo.Modtager.Post             =   TextBoxModtag_post.Text;
            fragtInfo.Modtager.Betaler          =   CheckBoxModtag_pay.IsChecked.Value;
            fragtInfo.Modtager.KontaktPerson    =   TextBoxModtag_pKont.Text;


            fragtInfo.AndenBetaler.Kontakt          =   TextBoxAndenB_customId.Text;
            fragtInfo.AndenBetaler.Firma            =   TextBoxAndenB_firma.Text;
            fragtInfo.AndenBetaler.Adresse          =   TextBoxAndenB_adresse.Text;
            fragtInfo.AndenBetaler.Post             =   TextBoxAndenB_post.Text;
            fragtInfo.AndenBetaler.Betaler          =   CheckBoxAndenB_pay.IsChecked.Value;
            fragtInfo.AndenBetaler.KontaktPerson    =   TextBoxAndenB_pKont.Text;

            fragtInfo.UseAndenBetaler = (buttonAndenB.Background == Brushes.Green);

            #endregion

            #region Generelt
            
            fragtInfo.Generelt.Refercence = TextBoxGenerelt_reference.Text;
            fragtInfo.Generelt.Fragtmand = TextBoxGenerelt_fragtmand.Text;
            fragtInfo.Generelt.Forsikringstype = ComboBoxGenerelt_type.SelectedIndex;
            fragtInfo.Generelt.Date1 = DatePickerGenerelt_dag1.Text;
            fragtInfo.Generelt.Rute1 = TextBoxGenerelt_rute1.Text;
            fragtInfo.Generelt.Date2 = DatePickerGenerelt_dag2.Text;
            fragtInfo.Generelt.Rute2 = TextBoxGenerelt_rute2.Text;
            fragtInfo.Generelt.UseEfterkrav = CheckBoxGenerelt_efterkrav.IsChecked.Value;
            fragtInfo.Generelt.Efterkrav = TextBoxGenerelt_efterkrav.Text;
            fragtInfo.Generelt.ForSum = TextBoxGenerelt_forsikring.Text;
            fragtInfo.Generelt.Praemie = TextBoxGenerelt_praemie.Text;
            fragtInfo.Generelt.All = TextBoxGenerelt_ialt.Text;
            
            #endregion

            #region Pakker

            fragtInfo.Transport1 = ComboBoxGodslinjer_TransportType.SelectedIndex;
            fragtInfo.Transport2 = ComboBoxGodslinjer_Pakketype.SelectedIndex;
            fragtInfo.Transport3 = ComboBoxGodslinjer_Biltype.SelectedIndex;

            if (fragtInfo.Transport1 >= -1 &&
                fragtInfo.Transport2 >= 0)
            {
                int pakkeCount = DataGridGodslinjer.Items.Count;

                switch (fragtInfo.Transport1)
                {
                    case 0:
                        if (fragtInfo.Transport3 <= 0)
                        {
                            break;
                        }
                        for (int i = 0; i < pakkeCount; i++)
                        {
                            Class.XML_Files.Fragtbrev.Godslinjer newGodsPack = new Class.XML_Files.Fragtbrev.Godslinjer();

                            //hent data
                            string adresse = funcDG.GetCellValueTextBlock(DataGridGodslinjer, i, 1),
                                antal = funcDG.GetCellValueTextBlock(DataGridGodslinjer, i, 2),
                                art = funcDG.GetDgComboboxData(DataGridGodslinjer, i, 3, false),
                                indhold = funcDG.GetCellValueTextBlock(DataGridGodslinjer, i, 4),
                                veagt = funcDG.GetCellValueTextBlock(DataGridGodslinjer, i, 5),
                                rumfangt = funcDG.GetCellValueTextBlock(DataGridGodslinjer, i, 6);

                            int antalOk = 0;
                            double KiloOk = 0;
                            int.TryParse(antal, out antalOk);
                            double.TryParse(veagt, out KiloOk);

                            newGodsPack.Adresse = adresse;
                            newGodsPack.Antal = antalOk;
                            newGodsPack.Art = art;
                            newGodsPack.Indhold = indhold;
                            newGodsPack.Kilo = KiloOk;
                            newGodsPack.Rumfang = rumfangt;

                            fragtInfo.Godslinjer.Add(newGodsPack);
                        }

                        break;
                    case 1:
                    case 2:
                        for (int i = 0; i < pakkeCount; i++)
                        {
                            Class.XML_Files.Fragtbrev.Godslinjer newGodsPack = new Class.XML_Files.Fragtbrev.Godslinjer();
                            //hent data
                            string adresse = funcDG.GetCellValueTextBlock(DataGridGodslinjer, i, 1),
                                antal = funcDG.GetCellValueTextBlock(DataGridGodslinjer, i, 2),
                                art = funcDG.GetDgComboboxData(DataGridGodslinjer, i, 3, false),
                                indhold = funcDG.GetCellValueTextBlock(DataGridGodslinjer, i, 4),
                                transport = funcDG.GetDgComboboxData(DataGridGodslinjer, i, 5, false),
                                veagt = funcDG.GetCellValueTextBlock(DataGridGodslinjer, i, 6),
                                rumfangt = funcDG.GetCellValueTextBlock(DataGridGodslinjer, i, 7);

                            int antalOk = 0;
                            double KiloOk = 0;
                            int.TryParse(antal, out antalOk);
                            double.TryParse(veagt, out KiloOk);

                            newGodsPack.Adresse = adresse;
                            newGodsPack.Antal = antalOk;
                            newGodsPack.Art = art;
                            newGodsPack.Indhold = indhold;
                            newGodsPack.Size = transport;
                            newGodsPack.Kilo = KiloOk;
                            newGodsPack.Rumfang = rumfangt;

                            fragtInfo.Godslinjer.Add(newGodsPack);
                        }
                        break;
                }
            }
            #endregion

            #region ByttePalle
            
            fragtInfo.UseByttePalle = CheckBoxByttepalle_active.IsChecked.Value;
            fragtInfo.Palle1 = TextBoxByttepalle_palle1_1.Text;
            fragtInfo.Palle2 = TextBoxByttepalle_palle1_2.Text;
            fragtInfo.Palle3 = TextBoxByttepalle_palle1_4.Text;
            
            #endregion

            #region Afslut Fragtbrev

            fragtInfo.Close.TimeL = TextBoxTidsforbrug_Load.Text;
            fragtInfo.Close.TimeA = TextBoxTidsforbrug_Unload.Text;
            fragtInfo.Close.TimeV = TextBoxTidsforbrug_Vente.Text;
            fragtInfo.Close.TimeH = TextBoxTidsforbrug_Helper.Text;
            fragtInfo.Close.DateDay = DatePickerGenerelt_Dato.SelectedDate.Value;
            fragtInfo.Close.DateTime = DatePickerGenerelt_Tid.Value.Value;
            fragtInfo.Close.Rabt = TextBoxGenerelt_Rabat.Text;
            fragtInfo.Close.Kilometer = TextBoxTransport_kilometer.Text;
            
            #endregion

            fragtFunction.SaveFile(fragtInfo);

            //kunde information
            #region Kunde database Save

            Class.XML_Files.Customer customFunc = new Class.XML_Files.Customer();

            Class.XML_Files.Customer.Layout[] textboxInfo = {
                new Class.XML_Files.Customer.Layout(),
                new Class.XML_Files.Customer.Layout(),
                new Class.XML_Files.Customer.Layout()
            };
            textboxInfo[0].ContactTlf = TextBoxAfsend_customId.Text;
            textboxInfo[0].FirmName = TextBoxAfsend_firma.Text;
            textboxInfo[0].ContactPer = TextBoxAfsend_pKont.Text;
            textboxInfo[0].Address = TextBoxAfsend_adresse.Text;
            textboxInfo[0].ZipCode = TextBoxAfsend_post.Text;

            textboxInfo[1].ContactTlf = TextBoxModtag_customId.Text;
            textboxInfo[1].FirmName = TextBoxModtag_firma.Text;
            textboxInfo[1].ContactPer = TextBoxModtag_pKont.Text;
            textboxInfo[1].Address = TextBoxModtag_adresse.Text;
            textboxInfo[1].ZipCode = TextBoxModtag_post.Text;

            textboxInfo[2].ContactTlf = TextBoxAndenB_customId.Text;
            textboxInfo[2].FirmName = TextBoxAndenB_firma.Text;
            textboxInfo[2].ContactPer = TextBoxAndenB_pKont.Text;
            textboxInfo[2].Address = TextBoxAndenB_adresse.Text;
            textboxInfo[2].ZipCode = TextBoxAndenB_post.Text;

            foreach (var information in textboxInfo)
            {
                if (
                    information.ContactTlf.Length > 0 &&
                    information.FirmName.Length > 0 &&
                    information.ContactPer.Length >= 0 &&
                    information.Address.Length > 0 &&
                    information.ZipCode.Length == 4
                    )
                {
                    customFunc.SaveCustomer(information);
                }
            }

            #endregion  Kunde database Save

            Models.ImportantData.FileIsSaved = true;
            SaveStatus();
        }

        ///<summary>
        ///så man kan læse en xml fil man har gemt
        /// </summary>
        private void ReadSaveXML(string filname)
        {
            Class.XML_Files.Fragtbrev.Layout fragtInfo = new Class.XML_Files.Fragtbrev.Layout();
            Class.XML_Files.Fragtbrev fragtFunction = new Class.XML_Files.Fragtbrev();
            fragtInfo = fragtFunction.ReadFile(filname);

            #region Defualt

            FlowDocument fDocment = new FlowDocument();

            Paragraph fdText = new Paragraph();
            fdText.Inlines.Add(new Run(fragtInfo.CommentTekst));

            fDocment.Blocks.Add(fdText);

            RichTextBoxEktra_Comment.Document = fDocment;

            //Initialer
            for (int i = 0; i < fragtInfo.Owners.Count; i++)
            {
                this.creatorId.Add(fragtInfo.Owners[i]);
            }

            #endregion

            #region Adresse

            //Afsender
            TextBoxAfsend_customId.Text = fragtInfo.Afsender.Kontakt;
            TextBoxAfsend_firma.Text = fragtInfo.Afsender.Firma;
            TextBoxAfsend_adresse.Text = fragtInfo.Afsender.Adresse;
            TextBoxAfsend_post.Text = fragtInfo.Afsender.Post;
            TextBoxAfsend_pKont.Text = fragtInfo.Afsender.KontaktPerson;
            CheckBoxAfsend_pay.IsChecked = fragtInfo.Afsender.Betaler;

            //Modtager
            TextBoxModtag_customId.Text = fragtInfo.Modtager.Kontakt;
            TextBoxModtag_firma.Text = fragtInfo.Modtager.Firma;
            TextBoxModtag_adresse.Text = fragtInfo.Modtager.Adresse;
            TextBoxModtag_post.Text = fragtInfo.Modtager.Post;
            TextBoxModtag_pKont.Text = fragtInfo.Modtager.KontaktPerson;
            CheckBoxModtag_pay.IsChecked = fragtInfo.Modtager.Betaler;

            //Anden betaler
            TextBoxAndenB_customId.Text = fragtInfo.AndenBetaler.Kontakt;
            TextBoxAndenB_firma.Text = fragtInfo.AndenBetaler.Firma;
            TextBoxAndenB_adresse.Text = fragtInfo.AndenBetaler.Adresse;
            TextBoxAndenB_post.Text = fragtInfo.AndenBetaler.Post;
            TextBoxAndenB_pKont.Text = fragtInfo.AndenBetaler.KontaktPerson;
            CheckBoxAndenB_pay.IsChecked = fragtInfo.AndenBetaler.Betaler;

            if (fragtInfo.UseAndenBetaler)
            {
                buttonAndenB.Background = Brushes.Green;
                GroupBoxAndenB.Visibility = Visibility.Visible;
            }

            #endregion

            #region Generelt

            TextBoxGenerelt_reference.Text = fragtInfo.Generelt.Refercence;
            TextBoxGenerelt_fragtmand.Text = fragtInfo.Generelt.Fragtmand;
            ComboBoxGenerelt_type.SelectedIndex = fragtInfo.Generelt.Forsikringstype;
            DatePickerGenerelt_dag1.Text = fragtInfo.Generelt.Date1;
            TextBoxGenerelt_rute1.Text = fragtInfo.Generelt.Rute1;
            DatePickerGenerelt_dag2.Text = fragtInfo.Generelt.Date2;
            TextBoxGenerelt_rute2.Text = fragtInfo.Generelt.Rute2;
            CheckBoxGenerelt_efterkrav.IsChecked = fragtInfo.Generelt.UseEfterkrav;
            TextBoxGenerelt_efterkrav.Text = fragtInfo.Generelt.Efterkrav;
            TextBoxGenerelt_forsikring.Text = fragtInfo.Generelt.ForSum;
            TextBoxGenerelt_praemie.Text = fragtInfo.Generelt.Praemie;
            TextBoxGenerelt_ialt.Text = fragtInfo.Generelt.All;

            #endregion

            #region Godslinjer

            ComboBoxGodslinjer_TransportType.SelectedIndex = fragtInfo.Transport1;
            ComboBoxGodslinjer_Pakketype.SelectedIndex = fragtInfo.Transport2;
            ComboBoxGodslinjer_Biltype.SelectedIndex = fragtInfo.Transport3;


            int pakkeCount = fragtInfo.Godslinjer.Count;

            var newItemsSourceGods = new List<Models.godsIndholdData>();


            switch (fragtInfo.Transport1)
            {
                case 0: //kurrer
                    for (int i = 0; i < pakkeCount; i++)
                    {
                        var godsIndholdItems = new Models.godsIndholdData();
                        //set values

                        godsIndholdItems.AdresseNumber = fragtInfo.Godslinjer[i].Adresse;
                        godsIndholdItems.Indhold = fragtInfo.Godslinjer[i].Indhold;
                        godsIndholdItems.Antal = fragtInfo.Godslinjer[i].Antal;

                        godsIndholdItems.Art = fragtInfo.Godslinjer[i].Art;
                        godsIndholdItems.Rumfang = fragtInfo.Godslinjer[i].Rumfang;
                        godsIndholdItems.Weight = fragtInfo.Godslinjer[i].Kilo;

                        //add pakke
                        newItemsSourceGods.Add(godsIndholdItems);
                    }
                    break;

                case 1: //Pakke
                case 2: //Gods
                    for (int i = 0; i < pakkeCount; i++)
                    {
                        var godsIndholdItems = new Models.godsIndholdData();
                        //set values

                        godsIndholdItems.AdresseNumber = fragtInfo.Godslinjer[i].Adresse;
                        godsIndholdItems.Indhold = fragtInfo.Godslinjer[i].Indhold;
                        godsIndholdItems.Antal = fragtInfo.Godslinjer[i].Antal;
                        godsIndholdItems.PakkeStr = fragtInfo.Godslinjer[i].Size;
                        godsIndholdItems.Art = fragtInfo.Godslinjer[i].Art;
                        godsIndholdItems.Rumfang = fragtInfo.Godslinjer[i].Rumfang;
                        godsIndholdItems.Weight = fragtInfo.Godslinjer[i].Kilo;

                        //add pakke
                        newItemsSourceGods.Add(godsIndholdItems);
                    }
                    break;
            }

            DataGridGodslinjer.ItemsSource = CollectionViewSource.GetDefaultView(newItemsSourceGods);
            DataGridGodslinjer.Items.Refresh();

            #endregion Godslinjer

            #region ByttePaller

            CheckBoxByttepalle_active.IsChecked = fragtInfo.UseByttePalle;
            TextBoxByttepalle_palle1_1.Text = fragtInfo.Palle1;
            TextBoxByttepalle_palle1_2.Text = fragtInfo.Palle2;
            TextBoxByttepalle_palle1_4.Text = fragtInfo.Palle3;

            #endregion

            #region Afslut Fragtbrev

            TextBoxTidsforbrug_Load.Text = fragtInfo.Close.TimeL;
            TextBoxTidsforbrug_Unload.Text = fragtInfo.Close.TimeA;
            TextBoxTidsforbrug_Vente.Text = fragtInfo.Close.TimeV;
            TextBoxTidsforbrug_Helper.Text = fragtInfo.Close.TimeH;
            DatePickerGenerelt_Dato.SelectedDate = fragtInfo.Close.DateDay;
            DatePickerGenerelt_Tid.Value = fragtInfo.Close.DateTime;
            TextBoxGenerelt_Rabat.Text = fragtInfo.Close.Rabt;
            TextBoxTransport_kilometer.Text = fragtInfo.Close.Kilometer;

            #endregion
        }

        //hent data til pdf
        private Class.PdfCreate.FragtBrevLayout PdfInformationGet()
        {
            Class.Functions.DatagridFunc funcDG = new Class.Functions.DatagridFunc();
            var fragtBrevInfo = new Class.PdfCreate.FragtBrevLayout();

            fragtBrevInfo.pdfName = Models.ImportantData.Filename;

            #region Adresse information

            //Afsender info
            fragtBrevInfo.Afsender.Telefon = TextBoxAfsend_customId.Text;
            fragtBrevInfo.Afsender.Adresse = TextBoxAfsend_adresse.Text;
            fragtBrevInfo.Afsender.Name = TextBoxAfsend_firma.Text;
            fragtBrevInfo.Afsender.Post = TextBoxAfsend_post.Text + " " + LabelAfsend_bynavn.Content;


            //Modtager info
            fragtBrevInfo.Modtager.Name = TextBoxModtag_firma.Text;
            fragtBrevInfo.Modtager.Adresse = TextBoxModtag_adresse.Text;
            fragtBrevInfo.Modtager.Post = TextBoxModtag_post.Text + " " + LabelModtag_bynavn.Content;
            fragtBrevInfo.Modtager.Telefon = TextBoxModtag_customId.Text;


            //om det er afsender eller modtager der er kundet nummeret
            if (CheckBoxAfsend_pay.IsChecked.Value || CheckBoxAndenB_pay.IsChecked.Value)
            {
                fragtBrevInfo.kundeNumber = TextBoxAfsend_customId.Text;
            }
            else
            {
                fragtBrevInfo.kundeNumber = TextBoxModtag_customId.Text;
            }

            #endregion Adresse information

            #region Generelt info
            //Generelt info
            fragtBrevInfo.fragtNumber = NewFragtbrevNumber();
            fragtBrevInfo.referenceText = TextBoxGenerelt_reference.Text;
            fragtBrevInfo.fragtmand = TextBoxGenerelt_fragtmand.Text;
            fragtBrevInfo.typeABCD = ComboBoxGenerelt_type.SelectedIndex;
            fragtBrevInfo.date1 = DatePickerGenerelt_dag1.Text;
            fragtBrevInfo.date2 = DatePickerGenerelt_dag2.Text;
            fragtBrevInfo.rute1 = TextBoxGenerelt_rute1.Text;
            fragtBrevInfo.rute2 = TextBoxGenerelt_rute2.Text;

            if (CheckBoxGenerelt_efterkrav.IsChecked.Value)
            {
                fragtBrevInfo.efterKrav = TextBoxGenerelt_efterkrav.Text;
                fragtBrevInfo.iAltSum = TextBoxGenerelt_ialt.Text;
                fragtBrevInfo.forsikringSum = TextBoxGenerelt_forsikring.Text;
                fragtBrevInfo.praemieSum = TextBoxGenerelt_praemie.Text;
            }
            else
            {
                fragtBrevInfo.efterKrav = "";
                fragtBrevInfo.iAltSum = "";
                fragtBrevInfo.forsikringSum = "";
                fragtBrevInfo.praemieSum = "";
            }


            //om det skal være franko eller ufranko
            fragtBrevInfo.senderPay = (CheckBoxAfsend_pay.IsChecked.Value || CheckBoxAndenB_pay.IsChecked.Value) ? true : false;

            #endregion Generelt info

            #region Byttepaller
            //Bytte paller
            if (CheckBoxByttepalle_active.IsChecked.Value)
            {
                fragtBrevInfo.isByttePalle = true;
            }

            //henter inputs som skal være et tal
            string[] palleValues = { TextBoxByttepalle_palle1_1.Text, TextBoxByttepalle_palle1_2.Text, TextBoxByttepalle_palle1_4.Text };

            for (int i = 0; i < 3; i++)
            {
                int palleValue;

                bool isANumber = int.TryParse(palleValues[i], out palleValue);

                if (isANumber)
                {
                    fragtBrevInfo.palleNumber[i] = palleValue;
                }
                else
                {
                    fragtBrevInfo.palleNumber[i] = 0;
                }

            }

            #endregion Byttepaller

            #region Godslinjer
            //godslinjer

            //hent transport type
            int transportTypeId = ComboBoxGodslinjer_TransportType.SelectedIndex;
            int transportTypeId2 = ComboBoxGodslinjer_Pakketype.SelectedIndex -1;
            string transportPakkeType = ComboBoxGodslinjer_Pakketype.SelectedValue.ToString();

            fragtBrevInfo.pakkeTransport = transportTypeId;
            fragtBrevInfo.pakkeTypeText = transportPakkeType;

            //tranpsort type start punkt
            int transportStart = 0;
            //find ud af hvad transport type det er 
            switch (transportTypeId)
            {
                case 0:
                    fragtBrevInfo.transportTypeUse[transportTypeId2] = true;
                    transportStart = 3;
                    break;

                case 1:
                    fragtBrevInfo.transportTypeUse[transportTypeId2 + 7] = true;
                    transportStart = 9;
                    break;
                    
                case 2:
                    fragtBrevInfo.transportTypeUse[transportTypeId2 + 16] = true;
                    transportStart = 18;
                    break;
            }


            //antal godlinjer pakker
            int godsCount = DataGridGodslinjer.Items.Count;

            switch (transportTypeId)
            {
                case 0:
                    for (int i = 0; i < godsCount; i++)
                    {
                        //henter values fra godslinjer
                        string pakkeAdresse,
                                pakkeAntal,
                                pakkeArt,
                                pakkeIndhold,
                                pakkeWidth,
                                pakkeRumfang;

                        int bilPakkeId;


                        pakkeAdresse = funcDG.GetCellValueTextBlock(DataGridGodslinjer, i, 1);
                        pakkeAntal = funcDG.GetCellValueTextBlock(DataGridGodslinjer, i, 2);
                        pakkeArt = funcDG.GetDgComboboxData(DataGridGodslinjer, i, 3, false);
                        pakkeIndhold = funcDG.GetCellValueTextBlock(DataGridGodslinjer, i, 4);
                        pakkeWidth = funcDG.GetCellValueTextBlock(DataGridGodslinjer, i, 5);
                        pakkeRumfang = funcDG.GetCellValueTextBlock(DataGridGodslinjer, i, 6);

                        bilPakkeId = ComboBoxGodslinjer_Biltype.SelectedIndex - 1;

                        if (pakkeAdresse != "" && pakkeAntal != "" &&
                            pakkeArt != "" && pakkeIndhold != "")
                        {
                            fragtBrevInfo.pakkeAdresseAndNumber.Add(pakkeAdresse);
                            fragtBrevInfo.pakkeART.Add(pakkeArt);
                            fragtBrevInfo.pakkeCount.Add(int.Parse(pakkeAntal));
                            fragtBrevInfo.pakkeIndhold.Add(pakkeIndhold);
                            fragtBrevInfo.transportTypeUse[transportStart + bilPakkeId] = true;

                            //hvis rumfangt felter ikke er tom hen text
                            if (pakkeRumfang != "")
                            {
                                fragtBrevInfo.pakkeRumOrWidth.Add(false);
                                fragtBrevInfo.pakkeRumfang.Add(pakkeRumfang);

                                double widthInKilo = 0;
                                double.TryParse(pakkeWidth, out widthInKilo);

                                //tjek om vægt er udflydt
                                if (widthInKilo != 0)
                                {
                                    fragtBrevInfo.pakkeRumOrWidthBoth.Add(true);
                                    fragtBrevInfo.pakkeWidth.Add(widthInKilo);
                                }
                                else
                                {
                                    fragtBrevInfo.pakkeRumOrWidthBoth.Add(false);
                                    fragtBrevInfo.pakkeWidth.Add(0);
                                }


                            }
                            else
                            {
                                double widthInKilo = 0;
                                double.TryParse(pakkeWidth, out widthInKilo);

                                fragtBrevInfo.pakkeRumOrWidth.Add(true);
                                fragtBrevInfo.pakkeWidth.Add(widthInKilo);
                                fragtBrevInfo.pakkeRumfang.Add("");


                                fragtBrevInfo.pakkeRumOrWidthBoth.Add(false);
                            }
                        }
                    }
                    break;
                case 1:
                    for (int i = 0; i < godsCount; i++)
                    {
                        //henter values fra godslinjer
                        string pakkeAdresse,
                                pakkeAntal,
                                pakkeArt,
                                pakkeIndhold,
                                pakkeWidth,
                                pakkeRumfang;
                        int bilPakkeId;

                        pakkeAdresse = funcDG.GetCellValueTextBlock(DataGridGodslinjer, i, 1);
                        pakkeAntal = funcDG.GetCellValueTextBlock(DataGridGodslinjer, i, 2);
                        pakkeArt = funcDG.GetDgComboboxData(DataGridGodslinjer, i, 3, false);
                        pakkeIndhold = funcDG.GetCellValueTextBlock(DataGridGodslinjer, i, 4);
                        bilPakkeId = int.Parse(funcDG.GetDgComboboxData(DataGridGodslinjer, i, 5, true));
                        pakkeWidth = funcDG.GetCellValueTextBlock(DataGridGodslinjer, i, 6);
                        pakkeRumfang = funcDG.GetCellValueTextBlock(DataGridGodslinjer, i, 7);


                        if (pakkeAdresse != "" && pakkeAntal != "" &&
                            pakkeArt != "" && pakkeIndhold != "")
                        {
                            fragtBrevInfo.pakkeAdresseAndNumber.Add(pakkeAdresse);
                            fragtBrevInfo.pakkeART.Add(pakkeArt);
                            fragtBrevInfo.pakkeCount.Add(int.Parse(pakkeAntal));
                            fragtBrevInfo.pakkeIndhold.Add(pakkeIndhold);

                            //hvis rumfangt felter ikke er tom hen text
                            if (pakkeRumfang != "")
                            {
                                fragtBrevInfo.pakkeRumOrWidth.Add(false);
                                fragtBrevInfo.pakkeRumfang.Add(pakkeRumfang);

                                double widthInKilo = 0;
                                double.TryParse(pakkeWidth, out widthInKilo);

                                //tjek om vægt er udflydt
                                if (widthInKilo != 0)
                                {
                                    fragtBrevInfo.pakkeRumOrWidthBoth.Add(true);
                                    fragtBrevInfo.pakkeWidth.Add(widthInKilo);
                                }
                                else
                                {
                                    fragtBrevInfo.pakkeRumOrWidthBoth.Add(false);
                                    fragtBrevInfo.pakkeWidth.Add(0);
                                }
                            }
                            else
                            {
                                double widthInKilo = 0;
                                double.TryParse(pakkeWidth, out widthInKilo);

                                fragtBrevInfo.pakkeRumOrWidth.Add(true);
                                fragtBrevInfo.pakkeWidth.Add(widthInKilo);
                                fragtBrevInfo.pakkeRumfang.Add("");

                                fragtBrevInfo.pakkeRumOrWidthBoth.Add(false);
                            }

                            //hvis man ikke har valgt en bilpakke skal den ikke tilføje den
                            if (bilPakkeId != -1)
                            {
                                fragtBrevInfo.transportTypeUse[transportStart + bilPakkeId] = true;
                            }
                        }
                    }
                    break;
                case 2:
                    for (int i = 0; i < godsCount; i++)
                    {
                        //henter values fra godslinjer
                        string pakkeAdresse,
                                pakkeAntal,
                                pakkeArt,
                                pakkeIndhold,
                                pakkeWidth,
                                pakkeRumfang;
                        int bilPakkeId;

                        pakkeAdresse = funcDG.GetCellValueTextBlock(DataGridGodslinjer, i, 1);
                        pakkeAntal = funcDG.GetCellValueTextBlock(DataGridGodslinjer, i, 2);
                        pakkeArt = funcDG.GetDgComboboxData(DataGridGodslinjer, i, 3, false);
                        pakkeIndhold = funcDG.GetCellValueTextBlock(DataGridGodslinjer, i, 4);
                        bilPakkeId = int.Parse(funcDG.GetDgComboboxData(DataGridGodslinjer, i, 5, true));
                        pakkeWidth = funcDG.GetCellValueTextBlock(DataGridGodslinjer, i, 6);
                        pakkeRumfang = funcDG.GetCellValueTextBlock(DataGridGodslinjer, i, 7);


                        if (pakkeAdresse != "" &&
                            !(pakkeAntal == "" || pakkeAntal == "0") &&
                            pakkeArt != "" && pakkeIndhold != "")
                        {
                            fragtBrevInfo.pakkeAdresseAndNumber.Add(pakkeAdresse);
                            fragtBrevInfo.pakkeART.Add(pakkeArt);
                            fragtBrevInfo.pakkeIndhold.Add(pakkeIndhold);

                            //Beregn antal pakker
                            int countAntal = 0;
                            int.TryParse(pakkeAntal, out countAntal);
                            fragtBrevInfo.pakkeCount.Add(countAntal);

                            //hvis rumfangt felter ikke er tom hen text
                            if (pakkeRumfang != "")
                            {
                                fragtBrevInfo.pakkeRumOrWidth.Add(false);
                                fragtBrevInfo.pakkeRumfang.Add(pakkeRumfang);

                                double widthInKilo = 0;
                                double.TryParse(pakkeWidth, out widthInKilo);

                                //tjek om vægt er udflydt
                                if (widthInKilo != 0)
                                {
                                    fragtBrevInfo.pakkeRumOrWidthBoth.Add(true);
                                    fragtBrevInfo.pakkeWidth.Add(widthInKilo);
                                }
                                else
                                {
                                    fragtBrevInfo.pakkeRumOrWidthBoth.Add(false);
                                    fragtBrevInfo.pakkeWidth.Add(0);
                                }
                            }
                            else
                            {
                                double widthInKilo = 0;
                                double.TryParse(pakkeWidth, out widthInKilo);

                                fragtBrevInfo.pakkeRumOrWidth.Add(true);
                                fragtBrevInfo.pakkeWidth.Add(widthInKilo);
                                fragtBrevInfo.pakkeRumfang.Add("");

                                fragtBrevInfo.pakkeRumOrWidthBoth.Add(false);
                            }

                            //hvis man ikke har valgt en bilpakke skal den ikke tilføje den
                            if (bilPakkeId != -1)
                            {
                                fragtBrevInfo.transportTypeUse[transportStart + bilPakkeId] = true;
                            }
                        }
                    }
                    break;
            }

            #endregion Godslinjer

            fragtBrevInfo.EkstraComment = new TextRange(RichTextBoxEktra_Comment.Document.ContentStart, RichTextBoxEktra_Comment.Document.ContentEnd).Text;

            //Initialer
            string allCreatorId = "";
            for (int i = 0; i < this.creatorId.Count; i++)
            {
                allCreatorId += this.creatorId[i] + ", ";
            }
            fragtBrevInfo.creatorIds = allCreatorId.Substring(0, allCreatorId.Length - 2);
            
            return fragtBrevInfo;
        }

        #endregion

        #region buttons

        //åben et gemt fragt brev
        private void Button_Click_OpenFragt(object sender, MouseButtonEventArgs e)
        {
            if (!Models.ImportantData.FileIsSaved)
            {
                MessageBoxResult result = MessageBox.Show("Gem nuværende fragtbrev?", "Gem", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    SaveToEditXML();
                }
                else if (result == MessageBoxResult.Cancel)
                {
                    return;
                }
            }

            FileName openfile = new FileName(2);
            openfile.ShowDialog();

            if (openfile.DialogResult.HasValue && openfile.DialogResult.Value)
            {
                //gem filnavn og gør så man henter den
                Models.ImportantData.Filename = openfile.TextBoxFileName_name.Content.ToString();
                Models.ImportantData.LoadFormFile = true;

                //vis fragtbrev og luk start menu
                WindowsView.UserSetup addIni = new WindowsView.UserSetup();
                addIni.ShowDialog();

                if (addIni.DialogResult.HasValue && addIni.DialogResult.Value)
                {
                    MainFragtBrev FragtbrevWindow = new MainFragtBrev(addIni.initialerCont);

                    try
                    {
                        Models.ImportantData.openStartMenuOnClose = false;
                        FragtbrevWindow.Show();
                        this.Close();
                    }
                    catch (Exception)
                    {
                    }
                }
            }
        }

        private void Button_Click_SaveToXML(object sender, MouseButtonEventArgs e)
        {
            SaveToEditXML();
        }

        private void Button_Click_ResetAll(object sender, MouseButtonEventArgs e)
        {
            ResetFragtbrevInpus();
        }
        
        

        private void Button_Click_SaveToPdf(object sender, MouseButtonEventArgs e)
        {
            //gemmer så man kan ændre den senere
            SaveToEditXML();

            if (AllowPdfSave())
            {
                Class.PdfCreate.FragtBrev createFragt = new Class.PdfCreate.FragtBrev();
                //bruges til at tjekke om filen er oprettet
                bool PdfIsCreated = true;

                var fragtBrevInfo = PdfInformationGet();

                //tjek om filen findes
                string folder = Models.ImportantData.g_FolderPdf;

                if (File.Exists(folder + Models.ImportantData.Filename + ".pdf"))
                {
                    MessageBoxResult resultFileExists = MessageBox.Show("Du er ved at oprette en PDF der findes i forvejen.\nErstat filen?", "PDF Findes", MessageBoxButton.YesNo);
                    if (resultFileExists == MessageBoxResult.Yes)
                    {
                        Class.Functions.others funcOthers = new Class.Functions.others();
                        if (funcOthers.IsFileLocked(new FileInfo(folder + Models.ImportantData.Filename + ".pdf")))
                        {
                            MessageBox.Show("Filen bliver brugt af en anden proces.\nKan ikke gemme filen.");
                            PdfIsCreated = false;
                        }
                        else
                        {
                            //slet gamle pdf
                            File.Delete(folder + fragtBrevInfo.pdfName);

                            //opret ny pdf
                            createFragt.SaveFragtBrev(fragtBrevInfo);
                        }
                    }
                    else
                    {
                        PdfIsCreated = false;
                    }
                }
                else
                {
                    //opret pdf
                    createFragt.SaveFragtBrev(fragtBrevInfo);
                }

                //hvis filen er blivet lavet
                if (PdfIsCreated)
                {
                    //visser en besked med pdf navn og om man vil starte på et nyt
                    MessageBoxResult result = MessageBox.Show(fragtBrevInfo.pdfName + ".pdf er nu gemt.", "PDF Oprettet", MessageBoxButton.OK);
                    if (result == MessageBoxResult.OK)
                    {
                        this.Close();
                    }
                }
            }
        }

        private void Button_Click_NewFaktura(object sender, MouseButtonEventArgs e)
        {
            if (!Models.ImportantData.FileIsSaved)
            {
                MessageBoxResult result = MessageBox.Show("Gem nuværende fragtbrev?", "Gem", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    SaveToEditXML();
                }
                else if (result == MessageBoxResult.Cancel)
                {
                    return;
                }
            }

            Models.ImportantData.openStartMenuOnClose = false;
            Class.Functions.Windows funcWin = new Class.Functions.Windows();
            funcWin.NewFaktura(this);
        }

        private void Button_Click_OpenFaktura(object sender, MouseButtonEventArgs e)
        {

            if (!Models.ImportantData.FileIsSaved)
            {
                MessageBoxResult result = MessageBox.Show("Gem nuværende fragtbrev?", "Gem", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    SaveToEditXML();
                }
                else if (result == MessageBoxResult.Cancel)
                {
                    return;
                }
            }
            Class.Functions.Windows funcWin = new Class.Functions.Windows();
            funcWin.EditFile(this, "Åben Fragtbrev", 1);
        }

        #endregion buttons


        private void ToolsbarButton_Filer_Click(object sender, RoutedEventArgs e)
        {
            if (ToolsBarMenu_Class.BoolFiler)
            {
                ToolsBarMenu_Class.BoolFiler = false;
                ToolsbarMenu_Filer.Visibility = Visibility.Hidden;
            }
            else
            {
                ToolsBarMenu_Class.BoolFiler = true;
                ToolsbarMenu_Filer.Visibility = Visibility.Visible;
            }
        }

        static class ToolsBarMenu_Class {
            public static bool BoolFiler = false;
        }


        private void ToolsbarMenu_Filer_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!ToolsbarButton_Filer.IsMouseOver && !ToolsbarMenu_Filer.IsMouseOver)
            {
                ToolsBarMenu_Class.BoolFiler = false;
                ToolsbarMenu_Filer.Visibility = Visibility.Hidden;
            }

        }

        private void ButtonAdmin_Startmenu_Click(object sender, RoutedEventArgs e)
        {
            if (!Models.ImportantData.FileIsSaved)
            {
                MessageBoxResult result = MessageBox.Show("Gem nuværende fragtbrev?", "Gem", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    SaveToEditXML();
                }
                else if (result == MessageBoxResult.Cancel)
                {
                    return;
                }
            }
            this.Close();
        }

        #endregion toolbar

        #region Afslut fragtbrev

        private void StatusCloseFragt()
        {
            ScrollviewBox.ScrollToBottom();

            toolbarButton_PDF.Visibility = Visibility.Hidden;
            toolbarList_PDF.Visibility = Visibility.Collapsed;

            GroupBoxAfsender.IsEnabled = false;
            GroupBoxModtager.IsEnabled = false;
            GroupBoxGenerelt.IsEnabled = false;
            GroupBoxByttepaller.IsEnabled = false;
            GroupBoxGodslinjer.IsEnabled = false;
            GroupBoxEvtComment.IsEnabled = false;

            if (ComboBoxGodslinjer_TransportType.SelectedIndex == 1)
            {
                GroupBoxTidforbrug.Visibility = Visibility.Hidden;
                LabelTransport_kilometer.Visibility = Visibility.Hidden;
                TextBoxTransport_kilometer.Visibility = Visibility.Hidden;
            }
            else if (ComboBoxGodslinjer_TransportType.SelectedIndex == 0)
            {
                LabelTransport_kilometer.Visibility = Visibility.Visible;
                TextBoxTransport_kilometer.Visibility = Visibility.Visible;
            }
            else if (ComboBoxGodslinjer_TransportType.SelectedIndex == 2)
            {
                LabelTransport_kilometer.Visibility = Visibility.Hidden;
                TextBoxTransport_kilometer.Visibility = Visibility.Hidden;
            }

            ButtonTool_CloseFragtbrev.IsEnabled = true;
            ButtonTool_CloseFragtbrev.Visibility = Visibility.Visible;
            ButtonTool_CloseFragtbrevComment.IsEnabled = true;
            ButtonTool_CloseFragtbrevComment.Visibility = Visibility.Visible;
            CloseFragtbrevBox.IsEnabled = true;
            CloseFragtbrevBox.Visibility = Visibility.Visible;
        }

        private void Button_Click_closeFrabrevText(object sender, RoutedEventArgs e)
        {
            WindowsView.CloseWindowText textwin = new WindowsView.CloseWindowText(Models.ImportantData.closeFragtbrevText, "fragtbrev");
            textwin.ShowDialog();

            if (textwin.DialogResult.HasValue && textwin.DialogResult.Value)
            {
                Models.ImportantData.closeFragtbrevText = textwin.returnText;
                SaveToEditXML();
            }
        }

        private void CloseFragtbrevStatus()
        {
            if (Models.ImportantData.closeFragtbrevBool)
            {
                ButtonTool_CloseFragtbrev.Background = Brushes.Green;
            }
            else
            {
                ButtonTool_CloseFragtbrev.Background = Brushes.Red;
            }
        }

        private void ButtonTool_CloseFragtbrev_Click(object sender, RoutedEventArgs e)
        {
            //hvis deres findes en faktura pdf version tillad ikke at åben faktura uden end en admin kode
            if (Models.ImportantData.closeFragtbrevBool)
            {
                bool deleteFaktura = false;
                if (File.Exists(Models.ImportantData.g_FolderPdf + "Faktura-"+this.thisInvoice+".pdf") ||
                    File.Exists(Models.ImportantData.g_FolderSave + "Faktura-"+this.thisInvoice+".xml"))
                {
                    deleteFaktura = true;
                }

                if(deleteFaktura)
                {
                    WindowsView.AdminKode deleteFakturaPass = new WindowsView.AdminKode("Der blev fundet en faktura.\nHvis du forsætter, vil denne faktura blive slettet.");
                    deleteFakturaPass.ShowDialog();
                    if (deleteFakturaPass.DialogResult.HasValue && deleteFakturaPass.DialogResult.Value)
                    {
                        if (deleteFakturaPass.password_AdminKode.Password == Models.ImportantData.Password)
                        {
                            try
                            {
                                if (File.Exists(Models.ImportantData.g_FolderPdf + "Faktura-" + this.thisInvoice + ".pdf"))
	                            {
		                            File.Delete(Models.ImportantData.g_FolderPdf + "Faktura-" + this.thisInvoice + ".pdf");
	                            }
                                
                                File.Delete(Models.ImportantData.g_FolderSave + "Faktura-" + this.thisInvoice + ".xml");
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Kunne ikke gennemføre handlingen.\nFejlmeddelelse:\n\n" + ex.Message);
                                return;
                            }
                        }
                        else
                        {
                            return;
                        }
                    }
                    else
                    {
                        return;
                    }
                }
            }
            Models.ImportantData.closeFragtbrevBool = !Models.ImportantData.closeFragtbrevBool;

            CloseFragtbrevStatus();
            SaveToEditXML();
        }

        #endregion Afslut fragtbrev

        private void WindowFragt_index_Closed(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (Models.ImportantData.openStartMenuOnClose)
            {
                MainWindow startmenu = new MainWindow();
                startmenu.Show();
            }
        }

        private void WindowFragt_index_Loaded(object sender, RoutedEventArgs e)
        {
            Models.ImportantData.openStartMenuOnClose = true;
        }

        private void WindowFragt_index_GotFocus(object sender, RoutedEventArgs e)
        {
            Models.ImportantData.FileIsSaved = false;
            SaveStatus();
        }

        private void buttonAndenB_Click(object sender, RoutedEventArgs e)
        {
            if (buttonAndenB.Background == Brushes.Green)
            {
                buttonAndenB.Content = "Tilføj anden betaler";
                buttonAndenB.Background = Brushes.Red;
                GroupBoxAndenB.Visibility = Visibility.Collapsed;
            }
            else
            {
                buttonAndenB.Content = "Fjern anden betaler";
                buttonAndenB.Background = Brushes.Green;
                GroupBoxAndenB.Visibility = Visibility.Visible;
            }
        }


    }
}
