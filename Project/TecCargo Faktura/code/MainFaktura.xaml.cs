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
using System.Reflection;

namespace TecCargo_Faktura
{ 
    /// <summary>
    /// Interaction logic for MainFaktura.xaml
    /// </summary>
    public partial class MainFaktura : Window
    {
        #region Global Variables

        public static closeFragtBrevData fragtBrevImportData = new closeFragtBrevData(); //data fra fragtbrev
        //private Class.PdfCreate.AdresseInfo fakturaInfo;
        private Class.PdfCreate.AdresseInfo afsenderInfo = new Class.PdfCreate.AdresseInfo();
        private Class.PdfCreate.AdresseInfo modtagerInfo = new Class.PdfCreate.AdresseInfo();
        private List<string> creatorId = new List<string>(); //hvem der har arbejde på denne fragtbrev
        private bool savePaper = true;  //om man skal spare på papiret

        #endregion Global Variables

        /// <summary>
        /// her bliver startup indstillinger sat
        /// om den skal læse fra fragtbrev eller ej
        /// </summary>
        /// <param name="creatorId">initialer</param>
        /// <param name="fragtbrevName">null = ændre i gammel faktura</param>
        public MainFaktura(string creatorId ,string fragtbrevName = null)
        {
            InitializeComponent();

            Models.ImportantData.closeFakturaBool = false;
            Models.ImportantData.closeFakturaText = "";

            this.creatorId.Add(creatorId);

            //update save status
            Models.ImportantData.FileIsSaved = true;
            SaveStatus();

            //nulstil ekstra tillæg
            Models.FakturaPrisliste.EkstraGebyr.chauffoer = false;
            Models.FakturaPrisliste.EkstraGebyr.flytteTilaeg = false;
            Models.FakturaPrisliste.EkstraGebyr.adrTilaeg = false;
            Models.FakturaPrisliste.EkstraGebyr.aftenNat = false;
            Models.FakturaPrisliste.EkstraGebyr.weekend = false;
            Models.FakturaPrisliste.EkstraGebyr.yederzone = false;
            Models.FakturaPrisliste.EkstraGebyr.byttePalle = false;
            Models.FakturaPrisliste.EkstraGebyr.smsService = false;
            Models.FakturaPrisliste.EkstraGebyr.adresseKorrektion = false;
            Models.FakturaPrisliste.EkstraGebyr.broAfgrif = false;
            Models.FakturaPrisliste.EkstraGebyr.vejAfgrif = false;
            Models.FakturaPrisliste.EkstraGebyr.faergeAfgrif = false;

            LabelVersion_version.Content += Models.ImportantData.PVersion.ToString();

            DataContext = new Models.DataGridSources();
            ComboBoxTransport_type1.ItemsSource = (new string[] { "Transport 1","Kurertransport","Pakketransport","Godstransport"}).ToList();


            if (fragtbrevName != null)
	        {
                ReadFromFragtbrev(fragtbrevName);

                //set det nye navn
                Models.ImportantData.Filename = fragtbrevName.Replace("Fragtbrev-", "Faktura-");
            }
            //om den læsser fra en fil
            else if (Models.ImportantData.LoadFormFile)
            {
                ReadSaveXML(Models.ImportantData.Filename);
            }
            else
            {
                MessageBox.Show("Noget gik gal.");
                this.Close();
            }

            Models.ImportantData.fragtBrevNumber = Models.ImportantData.Filename.Replace("Faktura-", "");
            this.Title = Models.ImportantData.Filename + " | Fragtbrev " + Models.ImportantData.fragtBrevNumber;

            UpdateEkstraGebyr();    //tjek om der er nogle ekstra tillæg på
            CloseFakturaStatus();   //tjek om fakturaen er lukket
        }
        
        
        #region ToolBar

        /// <summary>
        /// vil vise et vindue med ekstra tillæg
        /// </summary>
        private void ButtonToolsbar_EkstraGebyr(object sender, RoutedEventArgs e)
        {
            if (ComboBoxTransport_type1.SelectedIndex == 2)
            {
                MessageBox.Show("Du kan kun bruge ekstra tillæg på kurre og godstransport.");
                return;
            }
            var eGebyr = new WindowsView.FakturaGebyr();

            eGebyr.ShowDialog();

            //hvis man har lavet ændringer i ekstra gebyr vinduet
            //hent disse ændringer
            if (eGebyr.DialogResult.HasValue && eGebyr.DialogResult.Value)
            {

                Models.FakturaPrisliste.EkstraGebyr.chauffoer = eGebyr.activeBools[0];
                Models.FakturaPrisliste.EkstraGebyr.flytteTilaeg = eGebyr.activeBools[1];
                Models.FakturaPrisliste.EkstraGebyr.adrTilaeg = eGebyr.activeBools[2];
                Models.FakturaPrisliste.EkstraGebyr.aftenNat = eGebyr.activeBools[3];
                Models.FakturaPrisliste.EkstraGebyr.weekend = eGebyr.activeBools[4];
                Models.FakturaPrisliste.EkstraGebyr.yederzone = eGebyr.activeBools[5];
                Models.FakturaPrisliste.EkstraGebyr.byttePalle = eGebyr.activeBools[6];
                Models.FakturaPrisliste.EkstraGebyr.smsService = eGebyr.activeBools[7];
                Models.FakturaPrisliste.EkstraGebyr.adresseKorrektion = eGebyr.activeBools[8];
                Models.FakturaPrisliste.EkstraGebyr.broAfgrif = eGebyr.activeBools[9];
                Models.FakturaPrisliste.EkstraGebyr.vejAfgrif = eGebyr.activeBools[10];
                Models.FakturaPrisliste.EkstraGebyr.faergeAfgrif = eGebyr.activeBools[11];

                //prøv at hente det man har skrevet i tekstboksne
                try
                {
                    Models.FakturaPrisliste.EkstraGebyr.medHelper = int.Parse(eGebyr.Values[0]);
                    Models.FakturaPrisliste.EkstraGebyr.flyttePrEnhed = int.Parse(eGebyr.Values[1]);
                    Models.FakturaPrisliste.EkstraGebyr.byttePallePrPalle = int.Parse(eGebyr.Values[2]);
                    Models.FakturaPrisliste.EkstraGebyr.smsAdvisering = int.Parse(eGebyr.Values[3]);
                    Models.FakturaPrisliste.EkstraGebyr.broAfgrifD = double.Parse(eGebyr.Values[4]);
                    Models.FakturaPrisliste.EkstraGebyr.vejAfgrifD = double.Parse(eGebyr.Values[5]);
                    Models.FakturaPrisliste.EkstraGebyr.faergeAfgrifD = double.Parse(eGebyr.Values[6]);
                }
                catch (Exception)
                {
                    Models.FakturaPrisliste.EkstraGebyr.medHelper = 0;
                    Models.FakturaPrisliste.EkstraGebyr.flyttePrEnhed = 0;
                    Models.FakturaPrisliste.EkstraGebyr.byttePallePrPalle = 0;
                    Models.FakturaPrisliste.EkstraGebyr.smsAdvisering = 0;
                    Models.FakturaPrisliste.EkstraGebyr.broAfgrifD = 0;
                    Models.FakturaPrisliste.EkstraGebyr.vejAfgrifD = 0;
                    Models.FakturaPrisliste.EkstraGebyr.faergeAfgrifD = 0;

                    MessageBox.Show("Kunne ikke gemme alle dine ekstra tillæg.");
                }

                UpdateEkstraGebyr();

            }
        }

        /// <summary>
        /// Gem faktura
        /// </summary>
        private void ButtonToolsbar_saveXML(object sender, RoutedEventArgs e)
        {
            SaveToEditXML();
        }

        /// <summary>
        /// Åben start menu
        /// </summary>
        private void ButtonAdmin_Startmenu_Click(object sender, RoutedEventArgs e)
        {
            if (!Models.ImportantData.FileIsSaved)
            {
                MessageBoxResult result = MessageBox.Show("Gem ændringer?", "Gem", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
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

        /// <summary>
        /// vis dropdown liste med flere knapper
        /// </summary>
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

        static class ToolsBarMenu_Class
        {
            public static bool BoolFiler = false;
        }

        /// <summary>
        /// skjul filer dropdown liste nå 
        /// mussen går ud af listen
        /// </summary>
        private void ToolsbarMenu_Filer_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!ToolsbarButton_Filer.IsMouseOver && !ToolsbarMenu_Filer.IsMouseOver)
            {
                ToolsBarMenu_Class.BoolFiler = false;
                ToolsbarMenu_Filer.Visibility = Visibility.Hidden;
            }
        }

        /// <summary>
        /// opret et nyt fragtbrev
        /// </summary>
        private void ToolbarButton_Click_NewFragtbrev(object sender, MouseButtonEventArgs e)
        {
            if (!Models.ImportantData.FileIsSaved)
            {
                MessageBoxResult result = MessageBox.Show("Gem ændringer?", "Gem", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
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
            funcWin.NewFragtbrev(this);
        }

        /// <summary>
        /// opret nyt faktura
        /// </summary>
        private void ToolbarButton_Click_NewFaktura(object sender, MouseButtonEventArgs e)
        {
            if (!Models.ImportantData.FileIsSaved)
            {
                MessageBoxResult result = MessageBox.Show("Gem ændringer?", "Gem", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
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

        /// <summary>
        /// åben faktura
        /// </summary>
        private void ToolbarButton_Click_OpenFaktura(object sender, MouseButtonEventArgs e)
        {

            if (!Models.ImportantData.FileIsSaved)
            {
                MessageBoxResult result = MessageBox.Show("Gem ændringer?", "Gem", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
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
            funcWin.EditFile(this, "Åben Fragtbrev", 1);
        }

        /// <summary>
        /// åben fragtbrev
        /// </summary>
        private void ToolbarButton_Click_OpenFragtbrev(object sender, MouseButtonEventArgs e)
        {
            if (!Models.ImportantData.FileIsSaved)
            {
                MessageBoxResult result = MessageBox.Show("Gem ændringer?", "Gem", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
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
            funcWin.EditFile(this, "Åben Faktura", 4);
        }

        /// <summary>
        /// gem faktura
        /// </summary>
        private void ToolbarButton_Click_SaveFaktura(object sender, MouseButtonEventArgs e)
        {
            SaveToEditXML();
        }

        /// <summary>
        /// opret faktura pdf
        /// </summary>
        private void ToolbarButton_Click_ExportPDF(object sender, MouseButtonEventArgs e)
        {
            //gemmer så man kan ændre den senere
            SaveToEditXML();

            if (allowExePdf())
            {
                Class.PdfCreate.Faktura createFakt = new Class.PdfCreate.Faktura();
                //bruges til at tjekke om filen er oprettet
                bool PdfIsCreated = true;

                //tjek om filen findes
                string folder = Models.ImportantData.g_FolderPdf;

                if (File.Exists(folder + Models.ImportantData.Filename + ".pdf"))
                {
                    MessageBoxResult resultFileExists = MessageBox.Show("Du er ved at oprette en PDF der findes i forvejen.\nErstat filen?", "PDF Findes", MessageBoxButton.YesNo);
                    if (resultFileExists == MessageBoxResult.Yes)
                    {
                        //tjek om man kan få adgang til 
                        //filen så man kan slet den
                        Class.Functions.others funcOthers = new Class.Functions.others();
                        bool fileIsLook = funcOthers.IsFileLocked(new FileInfo(folder + Models.ImportantData.Filename + ".pdf"));

                        if (fileIsLook)
                        {
                            MessageBox.Show("Filen bliver brugt af en anden proces.\nKan ikke gemme filen.");
                            PdfIsCreated = false;
                        }
                        else
                        {
                            File.Delete(folder + @"\" + Models.ImportantData.Filename + ".pdf"); //slet fil
                            createFakt.CreateFaktura(CreateFakturaLayoutPdf()); //opret pdf
                        }
                    }
                    else
                    {
                        PdfIsCreated = false;
                    }
                }
                else
                {
                    createFakt.CreateFaktura(CreateFakturaLayoutPdf()); //opret pdf
                }

                //når pdf`en er blevet oprettet
                if (PdfIsCreated)
                {
                    MessageBoxResult result = MessageBox.Show(Models.ImportantData.Filename + ".pdf er nu gemt.\nGå til startmenu?", "PDF Oprettet", MessageBoxButton.YesNo);
                    if (result == MessageBoxResult.Yes)
                    {
                        this.Close();
                    }
                }
            }
        }

        /// <summary>
        /// åben vindue med prisen i alt
        /// </summary>
        private void ButtonToolsbar_PrislistOpen(object sender, RoutedEventArgs e)
        {
            var itemsSource = loadPrisList();

            if (itemsSource == null)
            {
                return;
            }

            WindowsView.prisListe openList = new WindowsView.prisListe(itemsSource);
            openList.Show();
        }

        /// <summary>
        /// om ekstra tillæg skal
        /// være synlig på siden
        /// </summary>
        private void Button_EkstraGebyr_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (GroupBoxEkstraGebyr.Visibility == Visibility.Collapsed)
            {
                GroupBoxEkstraGebyr.Visibility = Visibility.Visible;
                loadGebyr();
            }
            else
            {
                GroupBoxEkstraGebyr.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// om prisliten skal være
        /// synlig på siden
        /// </summary>
        private void Button_Prislist_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (GroupBoxPrislist.Visibility == Visibility.Collapsed)
            {
                GroupBoxPrislist.Visibility = Visibility.Visible;
                datagridPricelist.ItemsSource = loadPrisList();
            }
            else
            {
                GroupBoxPrislist.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// om man skal spare på papiret 
        /// </summary>
        private void ButtonTool_Savepaper_Click(object sender, RoutedEventArgs e)
        {
            this.savePaper = !this.savePaper;

            if (this.savePaper)
            {
                ButtonTool_Savepaper.Background = Brushes.Green;
            }
            else
            {
                ButtonTool_Savepaper.Background = Brushes.Red;
            }
        }

        #endregion ToolBar

        #region Transport Type

        /// <summary>
        /// sæt transport type og hvad der skal vise 
        /// på den næste transport dropdown liste
        /// </summary>
        private void ComboBoxTransport_type1_SelectChanged(object sender, SelectionChangedEventArgs e)
        {
            //nulstil pakker og skjul pakke listen
            DataGridTransport_Pakker.ItemsSource = new List<Models.FakturaTransportDataClass>();
            DataGridTransport_Pakker.Visibility = Visibility.Hidden;

            //type 3 skal ikke vise når der bliver ændret
            ComboBoxTransport_type2.Visibility = Visibility.Hidden;
            ComboBoxTransport_type3.Visibility = Visibility.Hidden;
            LabelTransport_kilometer.Visibility = Visibility.Hidden;
            TextBoxTransport_kilometer.Visibility = Visibility.Hidden;

            int selectId = ComboBoxTransport_type1.SelectedIndex;
            string[] itemsSources = { "" };

            //hvad itemssoucre der skal bruges
            switch (selectId)
            {
                case 1:
                    itemsSources = new string[] { "Transport 2", "GoRush", "GoFlex", "GoVIP" };
                    break;
                case 2:
                    GroupBoxEkstraGebyr.Visibility = Visibility.Collapsed;
                    Button_EkstraGebyr_Show.IsEnabled = false;
                    itemsSources = new string[] { "Transport 2", "GoPlus", "GoGreen" };
                    break;
                case 3:
                    itemsSources = new string[] { "Transport 2", "GoFull", "GoPart" };
                    break;
            }

            //man skal have valgt en transport type før den viser den næste
            if (selectId > 0)
            {
                ComboBoxTransport_type2.ItemsSource = itemsSources.ToList();
                ComboBoxTransport_type2.SelectedIndex = 0;
                ComboBoxTransport_type2.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// sæt bil type og hvad der skal vise
        /// på den næste transport dropdown liste
        /// 
        /// hvis pakketransport er valgt vis datagrid
        /// </summary>
        private void ComboBoxTransport_type2_SelectChanged(object sender, SelectionChangedEventArgs e)
        {
            //nulstil pakker og skjul pakke listen
            DataGridTransport_Pakker.ItemsSource = new List<Models.FakturaTransportDataClass>();
            DataGridTransport_Pakker.Visibility = Visibility.Hidden;

            ComboBoxTransport_type3.Visibility = Visibility.Hidden;
            LabelTransport_kilometer.Visibility = Visibility.Hidden;
            TextBoxTransport_kilometer.Visibility = Visibility.Hidden;

            int selectIdOfType1 = ComboBoxTransport_type1.SelectedIndex;
            int selectIdOfType2 = ComboBoxTransport_type2.SelectedIndex;
            string[] itemsSources = { "" };


            if (selectIdOfType2 != 0)
            {

                //hvad itemssoucre der skal bruges
                switch (selectIdOfType1)
                {
                    case 1:
                        itemsSources = new string[] { "Transport 3", "Gruppe 1", "Gruppe 2", "Gruppe 3", "Gruppe 4" };
                        break;

                    case 2:
                    case 3:
                        DataGridPakkerLoadType(selectIdOfType1);
                        break;
                }

                //hvis man har valgt kurertransport og valgt bil
                //type skal den vise combobox 3(pakke type)
                if (selectIdOfType2 != 0 && selectIdOfType1 == 1)
                {
                    ComboBoxTransport_type3.ItemsSource = itemsSources.ToList();
                    ComboBoxTransport_type3.SelectedIndex = 0;
                    ComboBoxTransport_type3.Visibility = Visibility.Visible;
                }
                else
                {
                    ComboBoxTransport_type3.Visibility = Visibility.Hidden;
                }
            }
        }

        /// <summary>
        /// sæt pakke type og kilometer
        /// vis datagrid
        /// </summary>
        private void ComboBoxTransport_type3_SelectChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboBoxTransport_type3.SelectedIndex == 0)
            {
                //nulstil pakker og skjul pakke listen
                DataGridTransport_Pakker.ItemsSource = new List<Models.FakturaTransportDataClass>();
                DataGridTransport_Pakker.Visibility = Visibility.Hidden;
            }


            LabelTransport_kilometer.Visibility = Visibility.Hidden;
            TextBoxTransport_kilometer.Visibility = Visibility.Hidden;


            if (ComboBoxTransport_type3.SelectedIndex > 0)
            {
                int selectTransportType = ComboBoxTransport_type1.SelectedIndex;

                //hvis kurertrnapsort er valgt skal 
                //den også vise antal kilometer
                if (selectTransportType == 1)
                {
                    LabelTransport_kilometer.Visibility = Visibility.Visible;
                    TextBoxTransport_kilometer.Visibility = Visibility.Visible;
                }

                //opret datagrid
                DataGridPakkerLoadType(selectTransportType);
            }
        }


        /// <summary>
        /// find prisen ved pakketransport 
        /// ud fra hvad pakke størrelse
        /// der er valgt
        /// </summary>
        public void ComboBoxTransportGD_pakketransport_SelectChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox senderObject = (sender as ComboBox);

            int selectPakkeSizeId = senderObject.SelectedIndex;
            int selectPakkeTypeId = ComboBoxTransport_type2.SelectedIndex - 1;
            if (selectPakkeSizeId < 0)
            {
                return;
            }
            int selectRow = DataGridTransport_Pakker.Items.IndexOf(DataGridTransport_Pakker.CurrentItem);

            double price = Models.FakturaPrisliste.pakkePriser.Prices[selectPakkeSizeId][selectPakkeTypeId];
            Class.Functions.DatagridFunc funcDG = new Class.Functions.DatagridFunc();
            funcDG.SetCellValueTextBlock(DataGridTransport_Pakker, selectRow, 3, price.ToString("F"));
        }

        /// <summary>
        /// find prisen ved godstransport
        /// ud fra takst og beregning type
        /// </summary>
        private void ComboBoxTransportGD_GodsTakst_SelectChanged(object sender, SelectionChangedEventArgs e)
        {
            Class.Functions.DatagridFunc funcDG = new Class.Functions.DatagridFunc();
            ComboBox senderObject = (sender as ComboBox);

            //om der er valgt en takst
            int comboBoxSelext = senderObject.SelectedIndex + 1;
            if (comboBoxSelext <= 0)
            {
                return;
            }

            int selectRow = DataGridTransport_Pakker.Items.IndexOf(DataGridTransport_Pakker.CurrentItem);

            int takst = comboBoxSelext;
            double kiloB = 0; //7
            double kiloR = 0; //8
            double price = 0; //9

            double.TryParse(funcDG.GetCellValueTextBlock(DataGridTransport_Pakker, selectRow, 7), out kiloB);
            double.TryParse(funcDG.GetCellValueTextBlock(DataGridTransport_Pakker, selectRow, 8), out kiloR);

            if (kiloR >= kiloB)
            {
                price = GodsTakstBeregn(takst, kiloR);
            }
            else
            {
                price = GodsTakstBeregn(takst, kiloB);
            }

            funcDG.SetCellValueTextBlock(DataGridTransport_Pakker, selectRow, 9, price.ToString("F"));

            datagridPricelist.ItemsSource = loadPrisList();
        }

        /// <summary>
        /// find prisen ved godstransport
        /// ud fra takst og beregning type
        /// </summary>
        private void ComboBoxTransportGD_GodsKiloBeregnT_SelectChanged(object sender, SelectionChangedEventArgs e)
        {
            Class.Functions.DatagridFunc funcDG = new Class.Functions.DatagridFunc();
            ComboBox senderObject = (sender as ComboBox);

            //om der er valgt en takst
            int comboBoxSelext = senderObject.SelectedIndex;
            int selectRow = DataGridTransport_Pakker.Items.IndexOf(DataGridTransport_Pakker.CurrentItem);
            if (comboBoxSelext < 0)
            {
                funcDG.SetCellValueTextBlock(DataGridTransport_Pakker, selectRow, 7, "0");
                return;
            }

            double rumfangL = 0;
            double rumfangB = 0;
            double rumfangH = 0;
            double.TryParse(funcDG.GetCellValueTextBlock(DataGridTransport_Pakker, selectRow, 4), out rumfangL);
            double.TryParse(funcDG.GetCellValueTextBlock(DataGridTransport_Pakker, selectRow, 5), out rumfangB);
            double.TryParse(funcDG.GetCellValueTextBlock(DataGridTransport_Pakker, selectRow, 6), out rumfangH);

            double kiloB = BeregnKiloGods(comboBoxSelext, new double[] { rumfangL, rumfangB, rumfangH });
            double kiloR = 0;
            double.TryParse(funcDG.GetCellValueTextBlock(DataGridTransport_Pakker, selectRow, 8), out kiloR);

            funcDG.SetCellValueTextBlock(DataGridTransport_Pakker, selectRow, 7, kiloB.ToString());

            //opdater prisen
            int takstSelectId = int.Parse(funcDG.GetDgComboboxData(DataGridTransport_Pakker, selectRow, 1, false));

            //tjek hvilken pris er højst
            if (kiloB >= kiloR)
            {
                funcDG.SetCellValueTextBlock(DataGridTransport_Pakker, selectRow, 9, GodsTakstBeregn(takstSelectId, kiloB).ToString("F"));
            }
            else
            {
                funcDG.SetCellValueTextBlock(DataGridTransport_Pakker, selectRow, 9, GodsTakstBeregn(takstSelectId, kiloR).ToString("F"));
            }

            datagridPricelist.ItemsSource = loadPrisList();
        }


        /// <summary>
        /// Hvad type transport der er valgt
        /// </summary>
        /// <param name="transportType">
        /// 0 = skjult DataGrid
        /// 1 = Kurrer;
        /// 2 = Pakke;
        /// 3 = Gods;
        /// </param>
        private void DataGridPakkerLoadType(int transportType)
        {

            if (transportType == 0)
            {
                DataGridTransport_Pakker.Visibility = Visibility.Hidden;
                return;
            }
            else
            {
                DataGridTransport_Pakker.Visibility = Visibility.Visible;
            }

            string[] headerNames = { "" };
            string[] bindingsName = { "" };
            bool[] isTextboxs = { false };
            bool[] isReadOnly = null;
            string[][] itemsSourceArray = { null };
            double[] widthSize = { 0 };
            bool[] showCombobox = { false };
            SelectionChangedEventHandler[] comboboxSelectChange = { null };


            switch (transportType)
            {
                case 1:

                    headerNames = new string[] { "Beskrivelse", "Kilo" };
                    bindingsName = new string[] { "DecText", "RealKilo" };
                    isTextboxs = new bool[] { true, true };
                    isReadOnly = new bool[] { true, true };
                    widthSize = new double[] { 200, 50 };

                    break;
                case 2:

                    headerNames = new string[] { "Pakke Type", "Kilo", "Pris" };
                    bindingsName = new string[] { "PakkeStr", "RealKilo", "Prices" };
                    isTextboxs = new bool[] { false, true, true };
                    isReadOnly = new bool[] { true, true, true };
                    itemsSourceArray = new string[][] {
                        new string[] { "XS","S","M","L","XL","2XL","3XL" }
                    };
                    widthSize = new double[] { 50, 50, 50 };
                    showCombobox = new bool[] { true };
                    comboboxSelectChange = new SelectionChangedEventHandler[] { ComboBoxTransportGD_pakketransport_SelectChanged };

                    break;
                case 3:

                    headerNames = new string[] { "Takst", "Pakke type", "Beregningstype", "Længde", "Bredde", "Højde", "Beregningsvægt", "Reelle vægt", "Pris" };
                    bindingsName = new string[] { "Takst", "PakkeStr", "BeregnType", "LengthL", "WidthtB", "HeightH", "BeregnKilo", "RealKilo", "Prices" };
                    isTextboxs = new bool[] { false, false, false, true, true, true, true, true, true };
                    isReadOnly = new bool[] { false, true, false, true, true, true, true, true, true };
                    itemsSourceArray = new string[][] {
                        new string[] { "1","2","3","4","5","6","7","8","9","10" },
                        new string[] { "LDM", "PLL", "M\u00b3" },
                        new string[] { "Ladmeter", "Palleplads","Volume" }
                    };
                    widthSize = new double[] { 50, 100, 70, 50, 50, 50, 50, 100, 100 };
                    showCombobox = new bool[] { true, true, true };
                    comboboxSelectChange = new SelectionChangedEventHandler[] { ComboBoxTransportGD_GodsTakst_SelectChanged, 
                        null,
                        ComboBoxTransportGD_GodsKiloBeregnT_SelectChanged };
                    break;

            }

            Class.Functions.DatagridFunc funcDG = new Class.Functions.DatagridFunc();
            funcDG.DataGridPakkerLoadType(DataGridTransport_Pakker, headerNames, bindingsName, widthSize, itemsSourceArray, isTextboxs, showCombobox, comboboxSelectChange, isReadOnly);
        }

        /// <summary>
        /// når man klikker på datagrid/pakker
        /// skal den ændre til ikke gemt
        /// </summary>
        private void Grid_GotFocus(object sender, RoutedEventArgs e)
        {
            Models.ImportantData.FileIsSaved = false;
            SaveStatus();
        }

        #endregion Transport Type
        
        #region Andet

        /// <summary>
        /// tilføj eller fjern bemærkning
        /// </summary>
        private void CheckBoxEkstratext_Button_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button).Background == Brushes.Green)
            {
                (sender as Button).Content = "Fjern Bemærkning";
                (sender as Button).Background = Brushes.Red;
                GroupBoxEkstraText.Visibility = Visibility.Visible;
            }
            else
            {
                (sender as Button).Content = "Tilføj Bemærkning";
                (sender as Button).Background = Brushes.Green;
                GroupBoxEkstraText.Visibility = Visibility.Collapsed;
                RichTextBoxEktra_Comment.Document = new FlowDocument();
            }
        }


        #endregion Andet

        #region Post nummer

        /// <summary>
        /// tjek om det nummer der bliver skrevet er en by
        /// </summary>
        private void TextBoxFaktura_post_TextChanged(object sender, TextChangedEventArgs e)
        {
            //kun tjek i post hvis man har skrevet 4 tal
            if ((sender as TextBox).Text.Length == 4)
            {
                Class.Functions.others funcOthers = new Class.Functions.others();
                DataSet postNumbDataSet = funcOthers.ReadXmlFormDatabase("postnummerfil");

                int postNumbCount = postNumbDataSet.Tables[2].Rows.Count;

                //tjek om postnummere er ens og hent navnet på byen
                for (int i = 0; i < postNumbCount; i++)
                {
                    string dataPost = postNumbDataSet.Tables[2].Rows[i].ItemArray[0].ToString();

                    if ((sender as TextBox).Text == dataPost)
                    {
                        LabelFaktura_bynavn.Content = postNumbDataSet.Tables[2].Rows[i].ItemArray[1].ToString();
                        break;
                    }
                    else
                    {
                        LabelFaktura_bynavn.Content = "";
                    }
                }
            }
        }

        #endregion Post nummer

        #region Ekstra Tillæg

        /// <summary>
        /// når man trykker på en ekstra tillæg
        /// knap med teskt box så kan man sætte
        /// en værdi
        /// </summary>
        /// <param name="nameSet">navn på knap/tekstbox</param>
        /// <param name="valueSet">tal</param>
        public static void SetValueekstraGebyr(string nameSet, object valueSet)
        {

            //hvis valueSet er ingenting stop
            if (valueSet == null || valueSet.ToString() == "")
            {
                return;
            }

            Type type = typeof(Models.FakturaPrisliste.EkstraGebyr); // Get type pointer
            FieldInfo[] fields = type.GetFields(); // Obtain all fields
            foreach (var field in fields) // Loop through fields
            {
                string name = field.Name; // Get string name

                if (name.ToLower() == nameSet.ToLower())
                {
                    object temp = field.GetValue(null);
                    bool failedConvert = false;
                    if (temp is int)
                    {
                        int valueOut = 0;

                        if (!int.TryParse(valueSet.ToString(), out valueOut))
                        {
                            failedConvert = true;
                        }

                        valueSet = valueOut.ToString();
                        field.SetValue(name, int.Parse(valueSet.ToString()));
                    }
                    else if (temp is double)
                    {
                        double valueOut = 0;

                        if (!double.TryParse(valueSet.ToString(), out valueOut))
                        {
                            failedConvert = true;
                        }

                        valueSet = valueOut;
                        field.SetValue(name, double.Parse(valueSet.ToString()));
                    }
                    else if (temp is bool)
                    {
                        field.SetValue(name, bool.Parse(valueSet.ToString()));
                    }

                    if (failedConvert)
                    {
                        MessageBox.Show("Failed!");
                    }

                    break;
                }
            }
        }

        /// <summary>
        /// ekstra tillæg
        /// Hent det tal der står i tekstbuksen ved siden af knappen
        /// </summary>
        public static object GetValueekstraGebyr(string nameGet)
        {

            Type type = typeof(Models.FakturaPrisliste.EkstraGebyr); // Get type pointer
            FieldInfo[] fields = type.GetFields(); // Obtain all fields
            object returnValue = null;
            foreach (var field in fields) // Loop through fields
            {
                string name = field.Name; // Get string name

                if (name.ToLower() == nameGet.ToLower())
                {
                    returnValue = field.GetValue(null);
                    break;
                }
            }

            return returnValue;
        }

        /// <summary>
        /// når man går ud af en ekstra tillæg tekstboks
        /// så vil den opdatere prisliten
        /// </summary>
        private void CheckBoxGebyr_TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            //find id
            int textBoxId = int.Parse((sender as TextBox).Name.Replace("CheckBoxGebyr_TextBox_", ""));

            //hvis knappen er aktiv kør loadgebyr
            //og textboxen er i focus
            if ((FindName("CheckBoxGebyr_Button_" + textBoxId) as Button).Background == Brushes.Green)
            {
                loadGebyr();
            }
        }

        /// <summary>
        /// hent ekstra tillæg tekstbox til prislisten
        /// </summary>
        private void loadGebyr(bool loadTextFirst = false)
        {
            string[] classBoolNames = {  
                "chauffoer", "flytteTilaeg", "adrTilaeg",
                "aftenNat", "weekend","yederzone",
                "byttePalle", "smsService", "adresseKorrektion",
                "broAfgrif", "vejAfgrif", "faergeAfgrif"
            };

            string[] classValueNames = {
                "medHelper",
                "flyttePrEnhed",
                "byttePallePrPalle",
                "smsAdvisering",
                "broAfgrifD",
                "vejAfgrifD",
                "faergeAfgrifD"
            };


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

            for (int i = 0; i < classBoolNames.Count(); i++)
            {
                if ((FindName("CheckBoxGebyr_Button_" + i) as Button).Background == Brushes.Green)
                {
                    SetValueekstraGebyr(classBoolNames[i], true);

                    for (int a = 0; a < textboxFieldN.Count(); a++)
                    {
                        if (textboxFieldN[a] == i)
                        {
                            SetValueekstraGebyr(classValueNames[a], (FindName("CheckBoxGebyr_TextBox_" + i) as TextBox).Text);
                            (FindName("CheckBoxGebyr_TextBox_" + i) as TextBox).IsEnabled = true;
                        }
                    }
                }
                else
                {
                    SetValueekstraGebyr(classBoolNames[i], false);

                    for (int a = 0; a < textboxFieldN.Count(); a++)
                    {
                        if (textboxFieldN[a] == i)
                        {
                            SetValueekstraGebyr(classValueNames[a], 0);
                            (FindName("CheckBoxGebyr_TextBox_" + i) as TextBox).IsEnabled = false;
                        }
                    }
                }
            }

        SetTextbox:
            for (int i = 0; i < textboxFieldN.Count(); i++)
            {
                (FindName("CheckBoxGebyr_TextBox_" + textboxFieldN[i]) as TextBox).Text = GetValueekstraGebyr(classValueNames[i]).ToString();
            }

            if (loadTextFirst)
            {
                loadTextFirst = false;
                goto Gebyrbool;
            }

            //opdater prislist
            datagridPricelist.ItemsSource = loadPrisList();
        }

        /// <summary>
        /// når man trykke enter på en af ekstra tillæg tekstboks
        /// så vil den opdatere prisliten
        /// </summary>
        private void CheckBoxGebyr_TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                loadGebyr();
            }
        }

        /// <summary>
        /// ændre fave på knap i ekstra tillæg
        /// og opdater prisliten
        /// </summary>
        private void CheckBoxGebyr_Button_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button).Background == Brushes.Red)
            {
                (sender as Button).Background = Brushes.Green;
            }
            else
            {
                (sender as Button).Background = Brushes.Red;
            }

            loadGebyr();
        }

        /// <summary>
        /// hvis man har åbenet ekstra tillæg eller på anden
        /// måde ændre i nogle bools til ekstra tillæg så knap
        /// farven ikke har skiftet kan det ske her
        /// </summary>
        private void UpdateEkstraGebyr()
        {
            bool[] activeBools = {  
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

            for (int i = 0; i < activeBools.Count(); i++)
            {
                if (activeBools[i])
                {
                    (FindName("CheckBoxGebyr_Button_" + i) as Button).Background = Brushes.Green;
                }
                else
                {
                    (FindName("CheckBoxGebyr_Button_" + i) as Button).Background = Brushes.Red;
                }
            }

            loadGebyr(true);
        }

        #endregion Ekstra Tillæg

        #region Prislisten

        /// <summary>
        /// hent itemsource til prislisten datagrid
        /// </summary>
        private List<Models.FakturaPricelistDataClass> loadPrisList()
        {
            Class.Functions.DatagridFunc funcDG = new Class.Functions.DatagridFunc();
            var itemsourceDone = new List<Models.FakturaPricelistDataClass>();
            var addItemClass = new Models.FakturaPricelistDataClass();
            //find transport type
            int[] priceClass = new int[] { ComboBoxTransport_type1.SelectedIndex, ComboBoxTransport_type2.SelectedIndex - 1, ComboBoxTransport_type3.SelectedIndex - 1 };

            //stop hvis der ikke er valgt transport typpe
            if (priceClass[0] <= 0 || priceClass[1] < 0)
            {
                //MessageBox.Show(priceClass[0].ToString() + " " + priceClass[1].ToString());
                return null;
            }

            #region nettofragt

            double nettofragt = 0;
            double totalprice = 0;
            double pakkerServiceprice = 0;

            switch (priceClass[0])
            {
                //kurrer
                case 1:

                    //hvis govip er valgt skift til gorush
                    if (priceClass[1] == 2)
                    {
                        priceClass[1] = 0;
                    }
                    //stop hvis der ikke valgt en bil gruppe
                    if (priceClass[2] < 0)
                    {
                        return null;
                    }

                    double startGebyrN = Models.FakturaPrisliste.kurerPriser.startgebyr[priceClass[1]][priceClass[2]];
                    addItemClass.name = "Startgebyr";
                    addItemClass.price = startGebyrN.ToString("F");
                    itemsourceDone.Add(addItemClass);
                    addItemClass = new Models.FakturaPricelistDataClass();

                    nettofragt += startGebyrN;

                    if (TextBoxTransport_kilometer.Text != "" && TextBoxTransport_kilometer.Text != "0")
                    {
                        double kilometerT = 0;
                        double.TryParse(TextBoxTransport_kilometer.Text, out kilometerT);

                        double kilometerTAll = kilometerT * Models.FakturaPrisliste.kurerPriser.kilometer[priceClass[1]][priceClass[2]];

                        addItemClass.name = "Kilometertakst " + kilometerT + "Km";
                        addItemClass.price = kilometerTAll.ToString("F");
                        itemsourceDone.Add(addItemClass);
                        addItemClass = new Models.FakturaPricelistDataClass();

                        nettofragt += kilometerTAll;
                    }

                    //om nettofragt er over minimum pris
                    //hvis ikke tilføj hvad der mangler
                    if (nettofragt < Models.FakturaPrisliste.kurerPriser.minimun[priceClass[1]][priceClass[2]])
                    {
                        double miniminPrice = Models.FakturaPrisliste.kurerPriser.minimun[priceClass[1]][priceClass[2]];
                        miniminPrice -= nettofragt;

                        addItemClass.name = "Minimun pris pr. tur";
                        addItemClass.price = miniminPrice.ToString("F");
                        itemsourceDone.Add(addItemClass);
                        addItemClass = new Models.FakturaPricelistDataClass();

                        //add minimum pris til nettofragt
                        nettofragt += miniminPrice;
                    }

                    totalprice += nettofragt;
                    break;
                //pakke
                case 2:
                    int pakkerService = 0;
                    for (int i = 0; i < DataGridTransport_Pakker.Items.Count; i++)
                    {
                        double pakkeprice = 0;
                        if (double.TryParse(funcDG.GetCellValueTextBlock(DataGridTransport_Pakker, i, 3), out pakkeprice))
                        {
                            nettofragt += pakkeprice;
                            pakkerService++;
                        }
                    }

                    addItemClass.name = "Pakker " + pakkerService;
                    addItemClass.price = nettofragt.ToString("F");
                    itemsourceDone.Add(addItemClass);
                    addItemClass = new Models.FakturaPricelistDataClass();

                    pakkerServiceprice = pakkerService * Models.FakturaPrisliste.pakkePriser.pakkeGebyr[priceClass[1]];

                    nettofragt += pakkerServiceprice;

                    priceClass[1] = 0;
                    priceClass[2] = 3;

                    totalprice += nettofragt;
                    break;
                //gods
                case 3:

                    int pakkerCount = 0;
                    for (int i = 0; i < DataGridTransport_Pakker.Items.Count; i++)
                    {
                        double pakkeprice = 0;
                        if (double.TryParse(funcDG.GetCellValueTextBlock(DataGridTransport_Pakker, i, 9), out pakkeprice))
                        {
                            nettofragt += pakkeprice;
                            pakkerCount++;
                        }
                    }
                    addItemClass.name = "Pakker " + pakkerCount;
                    addItemClass.price = nettofragt.ToString("F");
                    itemsourceDone.Add(addItemClass);
                    addItemClass = new Models.FakturaPricelistDataClass();

                    priceClass[1] = 0;
                    priceClass[2] = 3;

                    totalprice += nettofragt;
                    break;
            }

            itemsourceDone.Add(addItemClass);
            addItemClass = new Models.FakturaPricelistDataClass();
            #endregion nettofragt

            #region gebyr

            //MessageBox.Show(priceClass[0].ToString());
            bool timespace = false;
            if (priceClass[0] == 1 || priceClass[0] == 3)
            {
                if (fragtBrevImportData.tidsforbrugL != "" && fragtBrevImportData.tidsforbrugU != "" && fragtBrevImportData.tidsforbrugV != "")
                {
                    int tidforbrugL = 0;
                    int tidforbrugU = 0;
                    int tidforbrugV = 0;
                    int.TryParse(fragtBrevImportData.tidsforbrugL, out tidforbrugL);
                    int.TryParse(fragtBrevImportData.tidsforbrugU, out tidforbrugU);
                    int.TryParse(fragtBrevImportData.tidsforbrugV, out tidforbrugV);

                    int timeTotal = tidforbrugL + tidforbrugU + tidforbrugV;
                    /*
                    if (timeTotal > 20 && priceClass[0] == 1)
                    {
                        timeTotal -= 20;
                    }
                    */
                    if (timeTotal > 20)
                    {
                        timespace = true;
                        timeTotal -= 20;
                        double tidforbrugPrice = timeTotal * Models.FakturaPrisliste.kurerPriser.ekstraTidforbrug[priceClass[1]][priceClass[2]];

                        addItemClass.name = "Tidsforbrug " + timeTotal + "min";
                        addItemClass.price = tidforbrugPrice.ToString("F");
                        itemsourceDone.Add(addItemClass);
                        addItemClass = new Models.FakturaPricelistDataClass();

                        totalprice += tidforbrugPrice;
                    }
                }

                if (Models.FakturaPrisliste.EkstraGebyr.chauffoer)
                {
                    timespace = true;

                    int tidforbrugH = 0;
                    int.TryParse(fragtBrevImportData.tidsforbrugH, out tidforbrugH);
                    double countHelpTime = Math.Ceiling(tidforbrugH / 30f);
                    double allHelperTime = 30 * countHelpTime;
                    double helperPrice = (countHelpTime * Models.FakturaPrisliste.EkstraGebyr.medHelper) * Models.FakturaPrisliste.kurerPriser.medhjaelper[priceClass[1]][priceClass[2]];

                    addItemClass.name = "Chaufførmedhjælper";
                    addItemClass.price = helperPrice.ToString("F");
                    itemsourceDone.Add(addItemClass);
                    addItemClass = new Models.FakturaPricelistDataClass();

                    totalprice += helperPrice;
                }

                //om der skal være et mellerum efter tidsforbrug
                if (timespace)
                {
                    itemsourceDone.Add(addItemClass);
                    addItemClass = new Models.FakturaPricelistDataClass();
                }

                bool gebyrspace = false; //hvis der bliver add et gebyr add mellrum
                if (Models.FakturaPrisliste.EkstraGebyr.flytteTilaeg)
                {
                    gebyrspace = true;
                    double flyttePrice = Models.FakturaPrisliste.kurerPriser.flytte[priceClass[1]][priceClass[2]] * Models.FakturaPrisliste.EkstraGebyr.flyttePrEnhed;

                    addItemClass.name = "Flytte tillæg";
                    addItemClass.price = flyttePrice.ToString("F");
                    itemsourceDone.Add(addItemClass);
                    addItemClass = new Models.FakturaPricelistDataClass();

                    totalprice += flyttePrice;

                }
                if (Models.FakturaPrisliste.EkstraGebyr.adrTilaeg)
                {
                    gebyrspace = true;
                    double adrTPrice = Models.FakturaPrisliste.kurerPriser.adr[priceClass[1]][priceClass[2]];

                    addItemClass.name = "ADR-tillæg";
                    addItemClass.price = adrTPrice.ToString("F");
                    itemsourceDone.Add(addItemClass);
                    addItemClass = new Models.FakturaPricelistDataClass();

                    totalprice += adrTPrice;
                }
                if (Models.FakturaPrisliste.EkstraGebyr.aftenNat)
                {
                    gebyrspace = true;
                    double aftenPrice = Models.FakturaPrisliste.kurerPriser.aftenOgNat[priceClass[1]][priceClass[2]];

                    addItemClass.name = "Aften- og nattillæg (18:00-06:00)";
                    addItemClass.price = aftenPrice.ToString("F");
                    itemsourceDone.Add(addItemClass);
                    addItemClass = new Models.FakturaPricelistDataClass();

                    totalprice += aftenPrice;
                }
                if (Models.FakturaPrisliste.EkstraGebyr.weekend)
                {
                    gebyrspace = true;
                    double weekPrice = Models.FakturaPrisliste.kurerPriser.weekend[priceClass[1]][priceClass[2]];

                    addItemClass.name = "Weekendtillæg (lørdag-søndag)";
                    addItemClass.price = weekPrice.ToString("F");
                    itemsourceDone.Add(addItemClass);
                    addItemClass = new Models.FakturaPricelistDataClass();

                    totalprice += weekPrice;
                }
                if (Models.FakturaPrisliste.EkstraGebyr.yederzone)
                {
                    gebyrspace = true;
                    double yderPrice = nettofragt * (Models.FakturaPrisliste.kurerPriser.yderzone[priceClass[1]][priceClass[2]] / 100);

                    addItemClass.name = "Yderzonetillæg";
                    addItemClass.price = yderPrice.ToString("F");
                    itemsourceDone.Add(addItemClass);
                    addItemClass = new Models.FakturaPricelistDataClass();

                    totalprice += yderPrice;
                }
                if (Models.FakturaPrisliste.EkstraGebyr.byttePalle)
                {
                    gebyrspace = true;
                    double pallePrice = Models.FakturaPrisliste.kurerPriser.byttePalle[priceClass[1]][priceClass[2]] * Models.FakturaPrisliste.EkstraGebyr.byttePallePrPalle;

                    addItemClass.name = "Byttepalletillæg";
                    addItemClass.price = pallePrice.ToString("F");
                    itemsourceDone.Add(addItemClass);
                    addItemClass = new Models.FakturaPricelistDataClass();

                    totalprice += pallePrice;
                }
                if (Models.FakturaPrisliste.EkstraGebyr.smsService)
                {
                    gebyrspace = true;
                    double smsPrice = Models.FakturaPrisliste.kurerPriser.smsService[priceClass[1]][priceClass[2]] * Models.FakturaPrisliste.EkstraGebyr.smsAdvisering;

                    addItemClass.name = "SMS servicetillæg";
                    addItemClass.price = smsPrice.ToString("F");
                    itemsourceDone.Add(addItemClass);
                    addItemClass = new Models.FakturaPricelistDataClass();

                    totalprice += smsPrice;
                }
                if (Models.FakturaPrisliste.EkstraGebyr.adresseKorrektion)
                {
                    gebyrspace = true;
                    double adresseKPrice = Models.FakturaPrisliste.kurerPriser.addresseKirrektion[priceClass[1]][priceClass[2]];

                    addItemClass.name = "Adresse korrektion";
                    addItemClass.price = adresseKPrice.ToString("F");
                    itemsourceDone.Add(addItemClass);
                    addItemClass = new Models.FakturaPricelistDataClass();

                    totalprice += adresseKPrice;
                }

                if (Models.FakturaPrisliste.EkstraGebyr.broAfgrif)
                {
                    gebyrspace = true;
                    double broKPrice = Models.FakturaPrisliste.EkstraGebyr.broAfgrifD;

                    addItemClass.name = "Bro afgift";
                    addItemClass.price = broKPrice.ToString("F");
                    itemsourceDone.Add(addItemClass);
                    addItemClass = new Models.FakturaPricelistDataClass();

                    totalprice += broKPrice;
                }
                if (Models.FakturaPrisliste.EkstraGebyr.vejAfgrif)
                {
                    gebyrspace = true;
                    double vejKPrice = Models.FakturaPrisliste.EkstraGebyr.vejAfgrifD;

                    addItemClass.name = "Vej afgift";
                    addItemClass.price = vejKPrice.ToString("F");
                    itemsourceDone.Add(addItemClass);
                    addItemClass = new Models.FakturaPricelistDataClass();

                    totalprice += vejKPrice;
                }
                if (Models.FakturaPrisliste.EkstraGebyr.faergeAfgrif)
                {
                    gebyrspace = true;
                    double faergeKPrice = Models.FakturaPrisliste.EkstraGebyr.faergeAfgrifD;

                    addItemClass.name = "Færge afgift";
                    addItemClass.price = faergeKPrice.ToString("F");
                    itemsourceDone.Add(addItemClass);
                    addItemClass = new Models.FakturaPricelistDataClass();

                    totalprice += faergeKPrice;
                }

                if (gebyrspace)
                {
                    itemsourceDone.Add(addItemClass);
                    addItemClass = new Models.FakturaPricelistDataClass();
                }
            }

            //gebyr
            if (priceClass[0] == 2)
            {

                addItemClass.name = "Servicegebyr pr. pakke";
                addItemClass.price = pakkerServiceprice.ToString("F");
                itemsourceDone.Add(addItemClass);
                addItemClass = new Models.FakturaPricelistDataClass();
            }
            if (priceClass[0] == 1 || priceClass[0] == 3)
            {
                double gebyrBraend = nettofragt * (Models.FakturaPrisliste.kurerPriser.braendstof[priceClass[1]][priceClass[2]] / 100);

                addItemClass.name = "Brændstofgebyr Beregnes af nettofragt";
                addItemClass.price = gebyrBraend.ToString("F");
                itemsourceDone.Add(addItemClass);
                addItemClass = new Models.FakturaPricelistDataClass();

                totalprice += gebyrBraend;

                double gebyrMiljo = nettofragt * (Models.FakturaPrisliste.kurerPriser.miljoegebyr[priceClass[1]][priceClass[2]] / 100);

                addItemClass.name = "Miljøgebyr beregnes af nettofragt";
                addItemClass.price = gebyrMiljo.ToString("F");
                itemsourceDone.Add(addItemClass);
                addItemClass = new Models.FakturaPricelistDataClass();

                totalprice += gebyrMiljo;

                double adminGebyr = Models.FakturaPrisliste.kurerPriser.Adminnistrationsgebyr[priceClass[1]][priceClass[2]];

                addItemClass.name = "Adminnistrationsgebyr pr. faktura";
                addItemClass.price = adminGebyr.ToString("F");
                itemsourceDone.Add(addItemClass);
                addItemClass = new Models.FakturaPricelistDataClass();

                totalprice += adminGebyr;
            }
            itemsourceDone.Add(addItemClass);
            addItemClass = new Models.FakturaPricelistDataClass();

            itemsourceDone.Add(addItemClass);
            addItemClass = new Models.FakturaPricelistDataClass();

            addItemClass.name = "Subtotal";
            addItemClass.price = totalprice.ToString("F");
            itemsourceDone.Add(addItemClass);
            addItemClass = new Models.FakturaPricelistDataClass();

            double taxTotal = (totalprice * 0.25);
            addItemClass.name = "Tax";
            addItemClass.price = taxTotal.ToString("F");
            itemsourceDone.Add(addItemClass);
            addItemClass = new Models.FakturaPricelistDataClass();
            totalprice += taxTotal;

            double rabat = 0;
            if (fragtBrevImportData.rabat != "" && fragtBrevImportData.rabat != "0")
            {
                double rabatProcent = 0;
                double.TryParse(fragtBrevImportData.rabat, out rabatProcent);
                rabat = (totalprice / 100) * rabatProcent;

                addItemClass.name = "Rabat";
                addItemClass.price = rabat.ToString("F");
                itemsourceDone.Add(addItemClass);
                addItemClass = new Models.FakturaPricelistDataClass();
            }


            //med moms

            totalprice -= rabat;

            addItemClass.name = "Total";
            addItemClass.price = totalprice.ToString("F");
            itemsourceDone.Add(addItemClass);
            addItemClass = new Models.FakturaPricelistDataClass();

            #endregion

            //MessageBox.Show(itemsourceDone[0].name);

            //Update Prisen bruges til når der skal gemmes
            fragtBrevImportData.priceAll = totalprice.ToString("F");

            return itemsourceDone;
        }

        /// <summary>
        /// opdater prisliten
        /// </summary>
        private void CheckBoxPrisUpdate_Button_Click(object sender, RoutedEventArgs e)
        {
            datagridPricelist.ItemsSource = loadPrisList();
        }

        #endregion prislist

        #region Afslut faktura

        /// <summary>
        /// tjekker om fakturen er afsluttet og hvis er
        /// gør så man ikke kan ændre i fakturen mere
        /// </summary>
        private void CloseFakturaStatus()
        {
            if (Models.ImportantData.closeFakturaBool)
            {
                ButtonTool_CloseFragtbrev.Background = Brushes.Green;

                DataGridTransport_Pakker.IsEnabled = false;
                CheckBoxEkstraText_Button_1.IsEnabled = false;
                RichTextBoxEktra_Comment.IsEnabled = false;
                GroupBoxEkstraGebyr.IsEnabled = false;
                ButtonToolsbar_EkstraGebyrB.IsEnabled = false;
            }
            else
            {
                ButtonTool_CloseFragtbrev.Background = Brushes.Red;

                DataGridTransport_Pakker.IsEnabled = true;
                CheckBoxEkstraText_Button_1.IsEnabled = true;
                RichTextBoxEktra_Comment.IsEnabled = true;
                GroupBoxEkstraGebyr.IsEnabled = true;
                ButtonToolsbar_EkstraGebyrB.IsEnabled = true;
            }
        }

        /// <summary>
        /// gem fakturen som pdf og lås fakturen
        /// </summary>
        private void ButtonTool_CloseFragtbrev_Click(object sender, RoutedEventArgs e)
        {
            Models.ImportantData.closeFakturaBool = !Models.ImportantData.closeFakturaBool;

            //gem som pdf
            if (Models.ImportantData.closeFakturaBool)
            {
                //lav faktura
                if (allowExePdf())
                {
                    Class.PdfCreate.Faktura createFakt = new Class.PdfCreate.Faktura();
                    //bruges til at tjekke om filen er oprettet
                    bool PdfIsCreated = true;

                    //tjek om filen findes
                    string folder = Models.ImportantData.g_FolderPdf;

                    if (File.Exists(folder + Models.ImportantData.Filename + ".pdf"))
                    {
                        Class.Functions.others funcOthers = new Class.Functions.others();
                        if (funcOthers.IsFileLocked(new FileInfo(folder + Models.ImportantData.Filename + ".pdf")))
                        {
                            MessageBox.Show("Filen bliver brugt af en anden proces.\nKan ikke gemme filen som PDF.");
                            PdfIsCreated = false;
                        }
                        else
                        {
                            //slet gamle pdf
                            File.Delete(folder + Models.ImportantData.Filename + ".pdf");

                            //opret ny pdf
                            createFakt.CreateFaktura(CreateFakturaLayoutPdf());
                        }
                    }
                    else
                    {
                        //opret pdf
                        createFakt.CreateFaktura(CreateFakturaLayoutPdf());
                    }

                    //hvis filen er blivet lavet
                    if (PdfIsCreated)
                    {
                        //visser en besked med pdf navn og om man vil starte på et nyt
                        MessageBoxResult result = MessageBox.Show(Models.ImportantData.Filename + ".pdf er nu gemt.\nGå til startmenu?", "PDF Oprettet", MessageBoxButton.YesNo);
                        if (result == MessageBoxResult.Yes)
                        {
                            this.Close();
                        }
                    }
                }
            }

            CloseFakturaStatus();
            SaveToEditXML();
        }


        /// <summary>
        /// åben vindue hvor man kan tilføje en kommentar til fakturaen
        /// </summary>
        private void Button_Click_closeFrabrevText(object sender, RoutedEventArgs e)
        {
            WindowsView.CloseWindowText textwin = new WindowsView.CloseWindowText(Models.ImportantData.closeFakturaText, "faktura");
            textwin.ShowDialog();

            if (textwin.DialogResult.HasValue && textwin.DialogResult.Value)
            {
                Models.ImportantData.closeFakturaText = textwin.returnText;
                SaveToEditXML();
            }
        }

        #endregion Afslut faktura


        #region Functions

        /// <summary>
        /// Beregn gods kilo
        /// </summary>
        /// <param name="type">
        /// 0 = Ladmeted
        /// 1 = Palleplads
        /// 2 = Volume
        /// </param>
        /// <returns>Kilo</returns>
        private double BeregnKiloGods(int type, double[] values)
        {
            double returnValue = 0;

            switch (type)
            {
                case 0: //ladmeter
                    double enLadmeter = 1500 / 100;
                    returnValue = enLadmeter * values[0];
                    break;
                case 1: //Palleplads

                    returnValue = 600;

                    break;
                case 2: //Volume

                    returnValue = (values[0] * values[1] * values[2] * 250) / 1000000;

                    break;
            }

            return returnValue;
        }

        /// <summary>
        /// takst zone beregn prisen
        /// </summary>
        private double GodsTakstBeregn(int takst, double kilo)
        {
            //så takst passer til array
            takst--;
            if (takst < 0)
            {
                return 0;
            }

            //find ud af kilo id
            int kiloId = Models.FakturaPrisliste.godsPriser.Kilos.Count() -1;
            for (int i = 0; i < Models.FakturaPrisliste.godsPriser.Kilos.Count(); i++)
            {
                if (kilo <= Models.FakturaPrisliste.godsPriser.Kilos[i])
                {
                    kiloId = i;
                    break;
                }
            }

            //hent start pris
            double startPrice = Models.FakturaPrisliste.godsPriser.mainPrice[kiloId];

            //find procent
            double procentKategori = Math.Ceiling((kiloId + 1) / 5f);
            int procentKategoriId = 0;
            int.TryParse(procentKategori.ToString(), out procentKategoriId);

            double procentPrice = (startPrice / 100) * Models.FakturaPrisliste.godsPriser.PriceProcent[procentKategoriId - 1][takst];

            return startPrice + procentPrice;
        }

        /// <summary>
        /// om gem knappen skal være aktiv
        /// </summary>
        private void SaveStatus()
        {
            Class.Functions.others funcOthers = new Class.Functions.others();
            if (Models.ImportantData.FileIsSaved)
            {
                toolbarButton_Save.IsEnabled = false;
                toolbarButton_SaveImage.OpacityMask = funcOthers.ColorBrushHex("#7FFFFFFF");

                toolbarButton_SaveList.IsEnabled = false;
            }
            else
            {
                toolbarButton_Save.IsEnabled = true;
                toolbarButton_SaveImage.OpacityMask = funcOthers.ColorBrushHex("#FFFFFFFF");

                toolbarButton_SaveList.IsEnabled = true;
            }
        }

        /// <summary>
        /// henter fragtbrev oplyninger 
        /// og indsætter dem i elementer
        /// </summary>
        private void ReadFromFragtbrev(string filname, List<Models.FakturaTransportDataClass> fakturaSaveDg = null)
        {
            Class.Functions.others funcOther = new Class.Functions.others();
            Class.XML_Files.Fragtbrev fragtFunction = new Class.XML_Files.Fragtbrev();
            Class.XML_Files.Fragtbrev.Layout fragtInfo = fragtFunction.ReadFile(filname);

            #region Defualt

            fragtBrevImportData.tidsforbrugL = fragtInfo.Close.TimeL;
            fragtBrevImportData.tidsforbrugU = fragtInfo.Close.TimeA;
            fragtBrevImportData.tidsforbrugV = fragtInfo.Close.TimeV;
            fragtBrevImportData.tidsforbrugH = fragtInfo.Close.TimeH;
            fragtBrevImportData.dato = fragtInfo.Close.DateDay;
            fragtBrevImportData.time = fragtInfo.Close.DateTime;
            fragtBrevImportData.rabat = fragtInfo.Close.Rabt;
            TextBoxTransport_kilometer.Text = fragtInfo.Close.Kilometer;
            fragtBrevImportData.fragtbrevName = filname;

            DatePickerGenerelt_Dato.SelectedDate = fragtBrevImportData.dato;
            DatePickerGenerelt_Tid.Value = fragtBrevImportData.time;
            TextBoxGenerelt_Rabat.Text = fragtBrevImportData.rabat;
            TextBoxTidsforbrug_Load.Text = fragtBrevImportData.tidsforbrugL;
            TextBoxTidsforbrug_Unload.Text = fragtBrevImportData.tidsforbrugU;
            TextBoxTidsforbrug_Vente.Text = fragtBrevImportData.tidsforbrugV;
            TextBoxTidsforbrug_Helper.Text = fragtBrevImportData.tidsforbrugH;

            #endregion

            #region Adresse

            if (fragtInfo.Afsender.Betaler)
            {
                TextBoxFaktura_customId.Text = fragtInfo.Afsender.Kontakt;
                TextBoxFaktura_firma.Text = fragtInfo.Afsender.Firma;
                TextBoxFaktura_adresse.Text = fragtInfo.Afsender.Adresse;
                TextBoxFaktura_post.Text = fragtInfo.Afsender.Post;
            }
            else if (fragtInfo.Modtager.Betaler)
            {
                TextBoxFaktura_customId.Text = fragtInfo.Modtager.Kontakt;
                TextBoxFaktura_firma.Text = fragtInfo.Modtager.Firma;
                TextBoxFaktura_adresse.Text = fragtInfo.Modtager.Adresse;
                TextBoxFaktura_post.Text = fragtInfo.Modtager.Post;
            }
            else
            {
                TextBoxFaktura_customId.Text = fragtInfo.AndenBetaler.Kontakt;
                TextBoxFaktura_firma.Text = fragtInfo.AndenBetaler.Firma;
                TextBoxFaktura_adresse.Text = fragtInfo.AndenBetaler.Adresse;
                TextBoxFaktura_post.Text = fragtInfo.AndenBetaler.Post;
            }

            this.afsenderInfo.Name = fragtInfo.Afsender.Firma;
            this.afsenderInfo.Adresse = fragtInfo.Afsender.Adresse;
            this.afsenderInfo.Post = fragtInfo.Afsender.Post + " " + funcOther.GetCityFormZip(fragtInfo.Afsender.Post);
            this.afsenderInfo.Telefon = fragtInfo.Afsender.Kontakt;

            this.modtagerInfo.Name = fragtInfo.Modtager.Firma;
            this.modtagerInfo.Adresse = fragtInfo.Modtager.Adresse;
            this.modtagerInfo.Post = fragtInfo.Modtager.Post + " " + funcOther.GetCityFormZip(fragtInfo.Modtager.Post);
            this.modtagerInfo.Telefon = fragtInfo.Modtager.Kontakt;



            #endregion

            #region Pakker
            ComboBoxTransport_type1.SelectedIndex = fragtInfo.Transport1 + 1;
            ComboBoxTransport_type2.SelectedIndex = fragtInfo.Transport2;
            ComboBoxTransport_type3.SelectedIndex = fragtInfo.Transport3;

            
            //Hent pakker

            int pakkeCount = fragtInfo.Godslinjer.Count;
            var itemsSourceList = new List<Models.FakturaTransportDataClass>();

            switch (fragtInfo.Transport1)
            {
                #region KurrerTransport

                case 0:
                    for (int i = 0; i < pakkeCount; i++)
                    {

                        int pakkeAddCount = fragtInfo.Godslinjer[i].Antal;
                        double pakkeKilo = fragtInfo.Godslinjer[i].Kilo;
                        string decText = fragtInfo.Godslinjer[i].Indhold;

                        for (int a = 0; a < pakkeAddCount; a++)
                        {
                            var newItemClass = new Models.FakturaTransportDataClass();

                            newItemClass.DecText = decText;
                            newItemClass.RealKilo = pakkeKilo / pakkeAddCount;

                            itemsSourceList.Add(newItemClass);
                        }
                    }

                    break;
                #endregion
                #region PakkeTransport
                case 1:
                    for (int i = 0; i < pakkeCount; i++)
                    {

                        int pakkeAddCount = fragtInfo.Godslinjer[i].Antal;
                        double pakkeKilo = fragtInfo.Godslinjer[i].Kilo;
                        string pakkeSize = fragtInfo.Godslinjer[i].Size;

                        for (int a = 0; a < pakkeAddCount; a++)
                        {
                            var newItemClass = new Models.FakturaTransportDataClass();

                            newItemClass.PakkeStr = pakkeSize;
                            newItemClass.RealKilo = pakkeKilo / pakkeAddCount;

                            //find pris
                            string[] pakkeSizeNames = { "xs", "s", "m", "l", "xl", "2xl", "3xl" };
                            for (int b = 0; b <= pakkeSizeNames.Count() - 1; b++)
                            {
                                if (pakkeSize.ToLower() == pakkeSizeNames[b].ToLower())
                                {
                                    newItemClass.Prices = Models.FakturaPrisliste.pakkePriser.Prices[b][fragtInfo.Transport2 -1];
                                    break;
                                }
                            }

                            itemsSourceList.Add(newItemClass);
                        }
                    }
                    break;
                #endregion
                #region GodsTransport
                case 2:
                    //tjek om pakke indhold er blevet opdatetret og stop hvis det er

                    if (fakturaSaveDg != null)
                    {
                        int pakkeCountAll = 0;
                        for (int i = 0; i < pakkeCount; i++)
                        {
                            int antal = fragtInfo.Godslinjer[i].Antal;
                            pakkeCountAll += antal;
                        }

                        if (fakturaSaveDg.Count != pakkeCountAll)
                        {
                            MessageBox.Show("Pakker passer ikke\n" + fakturaSaveDg.Count + " " + pakkeCountAll + "\n Opret ny faktura.");
                            return;
                        }
                    }

                    int fakturaItemCount = 0;
                    for (int i = 0; i < pakkeCount; i++)
                    {
                        int antal = fragtInfo.Godslinjer[i].Antal;

                        if (antal == 0)
                        {
                            break;
                        }

                        double pakkeKilo = fragtInfo.Godslinjer[i].Kilo;
                        string pakketype = fragtInfo.Godslinjer[i].Size;
                        string rumfang = fragtInfo.Godslinjer[i].Rumfang;

                        //Find lænde, højde og bredde fra rumfang
                        int newLine = 0;
                        string[] rumfangArray = { "", "", "" };
                        for (int b = 0; b < rumfang.Length; b++)
                        {
                            if (rumfang.Substring(b, 1).ToLower() == "x" || rumfang.Substring(b, 1) == "*")
                            {
                                newLine++;
                            }
                            else
                            {
                                rumfangArray[newLine] += rumfang.Substring(b, 1);
                            }
                        }
                        double lengthRumfang = 0, widthtRumfang = 0, heightRumfang = 0;
                        double.TryParse(rumfangArray[0], out lengthRumfang);
                        double.TryParse(rumfangArray[1], out widthtRumfang);
                        double.TryParse(rumfangArray[2], out heightRumfang);

                        for (int b = 0; b < antal; b++)
                        {
                            var newItemClass = new Models.FakturaTransportDataClass();

                            newItemClass.BeregnType = "";
                            newItemClass.PakkeStr = pakketype;
                            newItemClass.BeregnKilo = 0;
                            newItemClass.RealKilo = pakkeKilo / antal;

                            newItemClass.LengthL = lengthRumfang;
                            newItemClass.WidthtB = widthtRumfang;
                            newItemClass.HeightH = heightRumfang;

                            if (fakturaSaveDg != null)
                            {
                                newItemClass.Takst = fakturaSaveDg[fakturaItemCount].Takst;
                                newItemClass.BeregnType = fakturaSaveDg[fakturaItemCount].BeregnType;
                                newItemClass.BeregnKilo = fakturaSaveDg[fakturaItemCount].BeregnKilo;
                                newItemClass.RealKilo = fakturaSaveDg[fakturaItemCount].RealKilo;
                                newItemClass.Prices = fakturaSaveDg[fakturaItemCount].Prices;

                                fakturaItemCount++;
                            }
                            itemsSourceList.Add(newItemClass);


                        }
                    }
                    break;
                    #endregion
            }

            DataGridTransport_Pakker.ItemsSource = itemsSourceList;
            DataGridTransport_Pakker.Items.Refresh();

            #endregion


        }

        /// <summary>
        /// Gem oplysninger til xml fil så
        /// man kan forsætte på filen senere
        /// </summary>
        private void SaveToEditXML()
        {
            Class.Functions.others funcOthers = new Class.Functions.others();
            Class.Functions.DatagridFunc funcDG = new Class.Functions.DatagridFunc();
            Class.XML_Files.Faktura.Layout fakturaInfo = new Class.XML_Files.Faktura.Layout();
            Class.XML_Files.Faktura fakturaSave = new Class.XML_Files.Faktura();
            
            loadPrisList();

            #region Generelt

            fakturaInfo.FragtName =  fragtBrevImportData.fragtbrevName;
            fakturaInfo.Invoice = Models.ImportantData.fragtBrevNumber;
            fakturaInfo.TransportId = ComboBoxTransport_type1.SelectedIndex;
            fakturaInfo.Price = double.Parse(fragtBrevImportData.priceAll);

            if (CheckBoxEkstraText_Button_1.Background == Brushes.Red)
            {
                fakturaInfo.UseComment = true;
            }
            else
            {
                fakturaInfo.UseComment = false;
            }
            string EkstraComment = new TextRange(RichTextBoxEktra_Comment.Document.ContentStart, RichTextBoxEktra_Comment.Document.ContentEnd).Text;
            fakturaInfo.CommentTekst = EkstraComment;

            //Initialer
            int ownersCount = this.creatorId.Count;
            for (int i = 0; i < this.creatorId.Count; i++)
            {
                fakturaInfo.Owners.Add(this.creatorId[i]);
            }
            #endregion

            #region Gods pakker
            //hvis det er  godstransport skal den tilføj nogle ekstra kolonner
            if (fakturaInfo.TransportId == 3)
            {
                //hent godstransport ekstra kolonner data
                int PakkeCount = DataGridTransport_Pakker.Items.Count;
                for (int i = 0; i < PakkeCount; i++)
                {
                    Class.XML_Files.Faktura.GodsTransport newGodsPack = new Class.XML_Files.Faktura.GodsTransport();
                    string Takst = "", 
                        beregning = "", 
                        Bkilo = "", 
                        Rkilo = "",
                        Pris = "";

                    Takst = funcDG.GetDgComboboxData(DataGridTransport_Pakker, i, 1, false);
                    beregning = funcDG.GetDgComboboxData(DataGridTransport_Pakker, i, 3, false);
                    Bkilo = funcDG.GetCellValueTextBlock(DataGridTransport_Pakker, i, 7);
                    Rkilo = funcDG.GetCellValueTextBlock(DataGridTransport_Pakker, i, 8);
                    Pris = funcDG.GetCellValueTextBlock(DataGridTransport_Pakker, i, 9);

                    newGodsPack.Takst = Takst;
                    newGodsPack.BeregnType = beregning;
                    newGodsPack.BeregnKilo = double.Parse(Bkilo);
                    newGodsPack.ReelleKilo = double.Parse(Rkilo);
                    newGodsPack.Price = double.Parse(Pris);

                    fakturaInfo.GodsPackets.Add(newGodsPack);
                }
            }
            #endregion

            #region Gebyr

            fakturaInfo.Gebyr.Helper = Models.FakturaPrisliste.EkstraGebyr.chauffoer;
            fakturaInfo.Gebyr.Flytte = Models.FakturaPrisliste.EkstraGebyr.flytteTilaeg;
            fakturaInfo.Gebyr.ADR = Models.FakturaPrisliste.EkstraGebyr.adrTilaeg;
            fakturaInfo.Gebyr.AftenOgNat = Models.FakturaPrisliste.EkstraGebyr.aftenNat;
            fakturaInfo.Gebyr.Weekend = Models.FakturaPrisliste.EkstraGebyr.weekend;
            fakturaInfo.Gebyr.Yderzone = Models.FakturaPrisliste.EkstraGebyr.yederzone;
            fakturaInfo.Gebyr.Byttepalle = Models.FakturaPrisliste.EkstraGebyr.byttePalle;
            fakturaInfo.Gebyr.SMS = Models.FakturaPrisliste.EkstraGebyr.smsService;
            fakturaInfo.Gebyr.AdresseK = Models.FakturaPrisliste.EkstraGebyr.adresseKorrektion;
            fakturaInfo.Gebyr.Bro = Models.FakturaPrisliste.EkstraGebyr.broAfgrif;
            fakturaInfo.Gebyr.Vej = Models.FakturaPrisliste.EkstraGebyr.vejAfgrif;
            fakturaInfo.Gebyr.Faerge = Models.FakturaPrisliste.EkstraGebyr.faergeAfgrif;

            fakturaInfo.Gebyr.HelperN = Models.FakturaPrisliste.EkstraGebyr.medHelper;
            fakturaInfo.Gebyr.FlytteN = Models.FakturaPrisliste.EkstraGebyr.flyttePrEnhed;
            fakturaInfo.Gebyr.ByttepalleN = Models.FakturaPrisliste.EkstraGebyr.byttePallePrPalle;
            fakturaInfo.Gebyr.SMS_N = Models.FakturaPrisliste.EkstraGebyr.smsAdvisering;
            fakturaInfo.Gebyr.BroPrice = Models.FakturaPrisliste.EkstraGebyr.broAfgrifD;
            fakturaInfo.Gebyr.VejPrice = Models.FakturaPrisliste.EkstraGebyr.vejAfgrifD;
            fakturaInfo.Gebyr.FaergePrice = Models.FakturaPrisliste.EkstraGebyr.faergeAfgrifD;
            #endregion

            fakturaSave.SaveFile(fakturaInfo);

            Models.ImportantData.FileIsSaved = true;
            SaveStatus();
        }

        /// <summary>
        /// forsæt på en faktra
        /// henter oplysninger fra en faktura 
        /// og indsætter dem i elementer
        /// </summary>
        private void ReadSaveXML(string filname)
        {
            Class.XML_Files.Faktura.Layout fakturaInfo = new Class.XML_Files.Faktura.Layout();
            Class.XML_Files.Faktura fakturaRead = new Class.XML_Files.Faktura();
            fakturaInfo = fakturaRead.ReadFile(filname);

            #region Generelt

            fragtBrevImportData.fragtbrevName = fakturaInfo.FragtName;
            Models.ImportantData.fragtBrevNumber = fakturaInfo.Invoice;
            ComboBoxTransport_type1.SelectedIndex = fakturaInfo.TransportId;

            //bemærkninger
            if (fakturaInfo.UseComment)
            {
                FlowDocument fDocment = new FlowDocument();
                CheckBoxEkstraText_Button_1.Background = Brushes.Red;
                CheckBoxEkstraText_Button_1.Content = "Fjern Bemærkning";
                GroupBoxEkstraText.Visibility = Visibility.Visible;

                Paragraph fdText = new Paragraph();
                fdText.Inlines.Add(new Run(fakturaInfo.CommentTekst));

                fDocment.Blocks.Add(fdText);

                RichTextBoxEktra_Comment.Document = fDocment;
            }

            //Initialer
            int ownersCount = fakturaInfo.Owners.Count;
            for (int i = 0; i < ownersCount; i++)
            {
                //tjek om initialerne er blivet tilføjet
                if (!this.creatorId.Contains(fakturaInfo.Owners[i]))
                {
                    this.creatorId.Add(fakturaInfo.Owners[i]); 
                }
            }

            #endregion

            #region Transport Pakker

            //hvis godstransport skal den tilføje nogle ekstra kolonner værdier
            if (fakturaInfo.TransportId == 3)
            {
                List<Models.FakturaTransportDataClass> itemsSourceList = new List<Models.FakturaTransportDataClass>();
                int godsCount = fakturaInfo.GodsPackets.Count;
                for (int i = 0; i < godsCount; i++)
                {
                    Models.FakturaTransportDataClass newItemClass = new Models.FakturaTransportDataClass();

                    newItemClass.Takst = fakturaInfo.GodsPackets[i].Takst;
                    newItemClass.BeregnType = fakturaInfo.GodsPackets[i].BeregnType;
                    newItemClass.BeregnKilo = fakturaInfo.GodsPackets[i].BeregnKilo;
                    newItemClass.RealKilo = fakturaInfo.GodsPackets[i].ReelleKilo;
                    newItemClass.Prices = fakturaInfo.GodsPackets[i].Price;

                    itemsSourceList.Add(newItemClass);
                }
                ReadFromFragtbrev(fragtBrevImportData.fragtbrevName, itemsSourceList);
            }
            else
            {
                ReadFromFragtbrev(fragtBrevImportData.fragtbrevName);
            }

            #endregion

            #region Gebyr

            Models.FakturaPrisliste.EkstraGebyr.chauffoer = fakturaInfo.Gebyr.Helper;
            Models.FakturaPrisliste.EkstraGebyr.flytteTilaeg = fakturaInfo.Gebyr.Flytte;
            Models.FakturaPrisliste.EkstraGebyr.adrTilaeg = fakturaInfo.Gebyr.ADR;
            Models.FakturaPrisliste.EkstraGebyr.aftenNat = fakturaInfo.Gebyr.AftenOgNat;
            Models.FakturaPrisliste.EkstraGebyr.weekend = fakturaInfo.Gebyr.Weekend;
            Models.FakturaPrisliste.EkstraGebyr.yederzone = fakturaInfo.Gebyr.Yderzone;
            Models.FakturaPrisliste.EkstraGebyr.byttePalle = fakturaInfo.Gebyr.Byttepalle;
            Models.FakturaPrisliste.EkstraGebyr.smsService = fakturaInfo.Gebyr.SMS;
            Models.FakturaPrisliste.EkstraGebyr.adresseKorrektion = fakturaInfo.Gebyr.AdresseK;
            Models.FakturaPrisliste.EkstraGebyr.broAfgrif = fakturaInfo.Gebyr.Bro;
            Models.FakturaPrisliste.EkstraGebyr.vejAfgrif = fakturaInfo.Gebyr.Vej;
            Models.FakturaPrisliste.EkstraGebyr.faergeAfgrif = fakturaInfo.Gebyr.Faerge;

            Models.FakturaPrisliste.EkstraGebyr.medHelper = fakturaInfo.Gebyr.HelperN;
            Models.FakturaPrisliste.EkstraGebyr.flyttePrEnhed = fakturaInfo.Gebyr.FlytteN;
            Models.FakturaPrisliste.EkstraGebyr.byttePallePrPalle = fakturaInfo.Gebyr.ByttepalleN;
            Models.FakturaPrisliste.EkstraGebyr.smsAdvisering = fakturaInfo.Gebyr.SMS_N;
            Models.FakturaPrisliste.EkstraGebyr.broAfgrifD = fakturaInfo.Gebyr.BroPrice;
            Models.FakturaPrisliste.EkstraGebyr.vejAfgrifD = fakturaInfo.Gebyr.VejPrice;
            Models.FakturaPrisliste.EkstraGebyr.faergeAfgrifD = fakturaInfo.Gebyr.FaergePrice;
            #endregion

        }

        /// <summary>
        /// opret pdf data 
        /// udfra faktura data
        /// 
        /// som vil blive brugt når pdf skal oprettes
        /// </summary>
        /// <returns></returns>
        private Class.PdfCreate.FakturaLayout CreateFakturaLayoutPdf()
        {
            Class.Functions.DatagridFunc funcDG = new Class.Functions.DatagridFunc();
            var FakturaLayout = new Class.PdfCreate.FakturaLayout();

            #region Generelt Info

            //fjern mellemrum fra kunde id/ faktura telefon
            string kundeId = "";
            for (int i = 0; i < TextBoxFaktura_customId.Text.Length; i++)
            {
                if (TextBoxFaktura_customId.Text.Substring(i,1) != " ")
                {
                    kundeId += TextBoxFaktura_customId.Text.Substring(i, 1);
                }
            }

            FakturaLayout.PdfName = Models.ImportantData.Filename;

            FakturaLayout.Dato = DateTime.Now.ToShortDateString();
            FakturaLayout.KundeID = kundeId;
            FakturaLayout.Invoice = Models.ImportantData.fragtBrevNumber;
            FakturaLayout.FragtNumber = Models.ImportantData.fragtBrevNumber;

            double rabatd;
            if (double.TryParse(fragtBrevImportData.rabat, out rabatd))
            {
                FakturaLayout.Rabat = rabatd;
            }
            else
            {
                FakturaLayout.Rabat = 0;
            }

            FakturaLayout.LeveringDag = fragtBrevImportData.dato.ToString("dddd d. MMM");
            FakturaLayout.LeveringTid = fragtBrevImportData.time.ToShortTimeString();

            #endregion Generelt Info

            #region Faktura Adresse Info

            FakturaLayout.FakturaAdresse.Name = TextBoxFaktura_firma.Text;
            FakturaLayout.FakturaAdresse.Adresse = TextBoxFaktura_adresse.Text;
            FakturaLayout.FakturaAdresse.Post = TextBoxFaktura_post.Text + " " + LabelFaktura_bynavn.Content;
            FakturaLayout.FakturaAdresse.Telefon = TextBoxFaktura_customId.Text;

            FakturaLayout.AfsenderAdresse = this.afsenderInfo;
            FakturaLayout.ModtagerAdresse = this.modtagerInfo;

            #endregion Faktura Adresse Info

            #region Produkter/Pakker

            //text på Transport Type
            FakturaLayout.TransportType_1_String = ComboBoxTransport_type1.SelectedValue.ToString();
            FakturaLayout.TransportType_2_String = ComboBoxTransport_type2.SelectedValue.ToString();

            //ID på Transport Type
            int TransportType_1_Id = ComboBoxTransport_type1.SelectedIndex - 1,
                TransportType_2_Id = ComboBoxTransport_type2.SelectedIndex - 1,
                TransportType_3_Id = 3;

            //skal kun hente fra combobox 3 hvis kurre transport er valgt
            if (TransportType_1_Id == 0)
            {
                FakturaLayout.TransportType_3_String = " " + ComboBoxTransport_type3.SelectedValue.ToString();
                TransportType_3_Id = ComboBoxTransport_type3.SelectedIndex - 1;
            }

            //hvis goVIP er valgt brug gorush prist liste
            if(TransportType_1_Id == 0 && TransportType_2_Id == 2)
            {
                TransportType_2_Id = 0;
            }

            if (TransportType_1_Id == 2)
            {
                TransportType_2_Id = 0;
                TransportType_3_Id = 3;
            }

            //hvilken transport type der er valgt
            int transportSelectId = ComboBoxTransport_type1.SelectedIndex;

            //antal pakker/produkter
            int pakkeCount = DataGridTransport_Pakker.Items.Count;

            //pakker der er helt udflydt og kilo ialt
            int pakkeCountTotal = 0;
            double KiloTotal = 0;

            double nettofragt = 0;

                #region Kategori - Pakker

                for (int i = 0; i < pakkeCount; i++)
                {
                    string categori = "Pakke", informationText = "None", kiloText = "None", prisText = "None";

                    if (transportSelectId == 1)
                    {
                        #region kurrertransport
                        string infoText = funcDG.GetCellValueTextBlock(DataGridTransport_Pakker, i, 1);

                        kiloText = funcDG.GetCellValueTextBlock(DataGridTransport_Pakker, i, 2);
                        prisText = "-";

                        if (infoText != "" && kiloText != "")
                        {
                            informationText = infoText + " " + DecimalTwo(kiloText) + " KG";
                        }
                        #endregion
                    }
                    else if (transportSelectId == 2)
                    {
                        #region pakketransport
                        string pakkeSize = funcDG.GetDgComboboxData(DataGridTransport_Pakker, i, 1, false);
                        kiloText = funcDG.GetCellValueTextBlock(DataGridTransport_Pakker, i, 2);
                        prisText = funcDG.GetCellValueTextBlock(DataGridTransport_Pakker, i, 3);

                        if (pakkeSize != "" && kiloText != "" && prisText != "")
                        {
                            informationText = "Pakke str." + pakkeSize + " " + DecimalTwo(kiloText) + " KG";
                        }
                        #endregion
                    }
                    else if (transportSelectId == 3)
                    {
                        #region godstransport
                        string beregningType = "None";
                        string takstZone = funcDG.GetDgComboboxData(DataGridTransport_Pakker, i, 1, false);
                        int beregningTypeId = int.Parse(funcDG.GetDgComboboxData(DataGridTransport_Pakker, i, 3, true));
                        string lengthL = funcDG.GetCellValueTextBlock(DataGridTransport_Pakker, i, 4);
                        string widthtB = funcDG.GetCellValueTextBlock(DataGridTransport_Pakker, i, 5);
                        string heightH = funcDG.GetCellValueTextBlock(DataGridTransport_Pakker, i, 6);
                        string beregnKilo = funcDG.GetCellValueTextBlock(DataGridTransport_Pakker, i, 7);
                        string reelleKilo = funcDG.GetCellValueTextBlock(DataGridTransport_Pakker, i, 8);
                        prisText = funcDG.GetCellValueTextBlock(DataGridTransport_Pakker, i, 9);

                        double beregnKiloD, ReelleKiloD, prisTextD;

                        bool isBeregnKiloD = double.TryParse(beregnKilo, out beregnKiloD);
                        bool isReelleKiloD = double.TryParse(reelleKilo, out ReelleKiloD);
                        bool isprisTextD = double.TryParse(prisText, out prisTextD);

                        if (isBeregnKiloD && isReelleKiloD)
                        {
                            if (beregnKiloD > ReelleKiloD)
                            {
                                kiloText = beregnKiloD.ToString();
                            }
                            else
                            {
                                kiloText = ReelleKiloD.ToString();
                            }
                        }
                        switch (beregningTypeId)
                        {
                            case 0:
                                beregningType = "LDM";
                                break;
                            case 1:
                                beregningType = "PLL";
                                break;
                            case 2:
                                beregningType = "M\u00b3";
                                break;
                        }
                        if (isprisTextD)
                        {
                            nettofragt += prisTextD;
                        }

                        if (takstZone != "" && beregningType != "" && lengthL != "" && widthtB != "" && heightH != "" &&
                            beregnKilo != "" && reelleKilo != "" && prisText != "")
                        {
                            informationText = "Takstzone:" + takstZone + ", L:" + lengthL + ", B:" + widthtB + ", H:" + heightH + ", Beregningstype: " + beregningType + " Fragtpligtigvægt " + DecimalTwo(kiloText) + " KG";
                        }
                        #endregion
                    }

                    if (informationText != "None")
                    {
                        FakturaAddToCategori(categori, informationText, prisText, FakturaLayout, out FakturaLayout);
                        pakkeCountTotal++;
                        double kiloDouble;
                        bool isKilo = double.TryParse(kiloText, out kiloDouble);

                        if (isKilo)
                        {
                            KiloTotal += kiloDouble;
                        }
                    }
                }

                FakturaLayout.pakkeCount = pakkeCountTotal;
                FakturaLayout.Kilo = KiloTotal;


                #endregion Kategori - Pakker

                #region Kategori - Nettofragt

                //beregning nettofragt
                if (transportSelectId == 1)
                {
                    //tjek om kilometer er et tal

                    double kilometerDouble;
                    bool isKilometerNumber = double.TryParse(TextBoxTransport_kilometer.Text, out kilometerDouble);

                    if (isKilometerNumber)
                    {
                        double startGebyr = Models.FakturaPrisliste.kurerPriser.startgebyr[TransportType_2_Id][TransportType_3_Id];
                        double kilometerPris = kilometerDouble * Models.FakturaPrisliste.kurerPriser.kilometer[TransportType_2_Id][TransportType_3_Id];

                        FakturaAddToCategori("Nettofragt", "Startgebyr (inkl. 20min. læssetid)", startGebyr.ToString(), FakturaLayout, out FakturaLayout);
                        FakturaAddToCategori("Nettofragt", "Kilometertakst. (" + kilometerDouble.ToString() + "km)", kilometerPris.ToString(), FakturaLayout, out FakturaLayout);

                        //tjek om det er mindre en minimum
                        nettofragt = startGebyr + kilometerPris;
                        double minimunNettofragtPris = Models.FakturaPrisliste.kurerPriser.minimun[TransportType_2_Id][TransportType_3_Id];

                        if (nettofragt < minimunNettofragtPris)
                        {
                            double nettoFragTotal = minimunNettofragtPris - nettofragt;

                            FakturaAddToCategori("Nettofragt", "Minimun pris pr. tur", nettoFragTotal.ToString(), FakturaLayout, out FakturaLayout);

                            //add minimun pris til nettofragt
                            nettofragt += nettoFragTotal;
                        }

                    }
                }

                #endregion Kategori - Nettofragt

                #region Kategori - Tid / Minut

            if (transportSelectId == 1 || transportSelectId == 3)
            {
                //
                // Tid/ minut
                //

                //hvis der er blevet brugt mere end 20 min
                int countTime = int.Parse(fragtBrevImportData.tidsforbrugL) + int.Parse(fragtBrevImportData.tidsforbrugU) + int.Parse(fragtBrevImportData.tidsforbrugV);
                if (countTime > 20)
                {
                    int ekstraTime = countTime - 20;
                    double timePrice = (Models.FakturaPrisliste.kurerPriser.ekstraTidforbrug[TransportType_2_Id][TransportType_3_Id] * ekstraTime);

                    FakturaAddToCategori("Tid / Minut", "Tidsforbrug (" + ekstraTime.ToString() + "min)", timePrice.ToString(), FakturaLayout, out FakturaLayout);
                }

                //medhjælper
                if (Models.FakturaPrisliste.EkstraGebyr.chauffoer)
                {
                    int helperTime = int.Parse(fragtBrevImportData.tidsforbrugH);
                    double countHelpTime = Math.Ceiling(helperTime / 30f);
                    double allHelperTime = 30 * countHelpTime;

                    double helperPrice = (Models.FakturaPrisliste.kurerPriser.medhjaelper[TransportType_2_Id][TransportType_3_Id] * countHelpTime) * Models.FakturaPrisliste.EkstraGebyr.medHelper;

                    FakturaAddToCategori("Tid / Minut", "Medhjælper (" + allHelperTime + " min)", helperPrice.ToString(), FakturaLayout, out FakturaLayout);
                }
            }

            #endregion Kategori - Tid / Minut

                #region Kategori - Tillæg for særlige ydelser
                //
                // Tillæg for særlige ydelser
                //
            if (transportSelectId == 1 || transportSelectId == 3)
            {
                if (Models.FakturaPrisliste.EkstraGebyr.flytteTilaeg)
                {
                    double flyttePrice = Models.FakturaPrisliste.kurerPriser.flytte[TransportType_2_Id][TransportType_3_Id] * Models.FakturaPrisliste.EkstraGebyr.flyttePrEnhed;

                    FakturaAddToCategori("Tillæg for særlige ydelser", "Flytte tilæg", flyttePrice.ToString(), FakturaLayout, out FakturaLayout);
                }
                if (Models.FakturaPrisliste.EkstraGebyr.adrTilaeg)
                {
                    double adrTPrice = Models.FakturaPrisliste.kurerPriser.adr[TransportType_2_Id][TransportType_3_Id];

                    FakturaAddToCategori("Tillæg for særlige ydelser", "ADR-tilæg", adrTPrice.ToString(), FakturaLayout, out FakturaLayout);
                }
                if (Models.FakturaPrisliste.EkstraGebyr.aftenNat)
                {
                    double aftenPrice = Models.FakturaPrisliste.kurerPriser.aftenOgNat[TransportType_2_Id][TransportType_3_Id];

                    FakturaAddToCategori("Tillæg for særlige ydelser", "Aften- og nattillæg (18:00-06:00)", aftenPrice.ToString(), FakturaLayout, out FakturaLayout);
                }
                if (Models.FakturaPrisliste.EkstraGebyr.weekend)
                {
                    double weekPrice = Models.FakturaPrisliste.kurerPriser.weekend[TransportType_2_Id][TransportType_3_Id];

                    FakturaAddToCategori("Tillæg for særlige ydelser", "Weekendtillæg (lørdag-søndag)", weekPrice.ToString(), FakturaLayout, out FakturaLayout);
                }
                if (Models.FakturaPrisliste.EkstraGebyr.yederzone)
                {
                    double yderPrice = nettofragt * (Models.FakturaPrisliste.kurerPriser.yderzone[TransportType_2_Id][TransportType_3_Id] / 100);

                    FakturaAddToCategori("Tillæg for særlige ydelser", "Yderzonetillæg", yderPrice.ToString(), FakturaLayout, out FakturaLayout);
                }
                if (Models.FakturaPrisliste.EkstraGebyr.byttePalle)
                {
                    double pallePrice = Models.FakturaPrisliste.kurerPriser.byttePalle[TransportType_2_Id][TransportType_3_Id] * Models.FakturaPrisliste.EkstraGebyr.byttePallePrPalle;

                    FakturaAddToCategori("Tillæg for særlige ydelser", "Byttepalletillæg ", pallePrice.ToString(), FakturaLayout, out FakturaLayout);

                }
                if (Models.FakturaPrisliste.EkstraGebyr.smsService)
                {
                    double smsPrice = Models.FakturaPrisliste.kurerPriser.smsService[TransportType_2_Id][TransportType_3_Id] * Models.FakturaPrisliste.EkstraGebyr.smsAdvisering;

                    FakturaAddToCategori("Tillæg for særlige ydelser", "SMS servicetillæg", smsPrice.ToString(), FakturaLayout, out FakturaLayout);

                }
                if (Models.FakturaPrisliste.EkstraGebyr.adresseKorrektion)
                {
                    double adresseKPrice = Models.FakturaPrisliste.kurerPriser.addresseKirrektion[TransportType_2_Id][TransportType_3_Id];

                    FakturaAddToCategori("Tillæg for særlige ydelser", "Adresse korrektion", adresseKPrice.ToString(), FakturaLayout, out FakturaLayout);
                }

                if (Models.FakturaPrisliste.EkstraGebyr.broAfgrif)
                {
                    double broKPrice = Models.FakturaPrisliste.EkstraGebyr.broAfgrifD;

                    FakturaAddToCategori("Tillæg for særlige ydelser", "Bro afgift", broKPrice.ToString(), FakturaLayout, out FakturaLayout);
                }
                if (Models.FakturaPrisliste.EkstraGebyr.vejAfgrif)
                {
                    double vejKPrice = Models.FakturaPrisliste.EkstraGebyr.vejAfgrifD;

                    FakturaAddToCategori("Tillæg for særlige ydelser", "Vej afgift", vejKPrice.ToString(), FakturaLayout, out FakturaLayout);
                }
                if (Models.FakturaPrisliste.EkstraGebyr.faergeAfgrif)
                {
                    double faergeKPrice = Models.FakturaPrisliste.EkstraGebyr.faergeAfgrifD;

                    FakturaAddToCategori("Tillæg for særlige ydelser", "Færge afgift", faergeKPrice.ToString(), FakturaLayout, out FakturaLayout);
                }
            }

                #endregion Kategori - Tillæg for særlige ydelser

                #region Kategori - Gebyr

                //
                //gebyr
                //

                //beregnings service gebyr
                if (transportSelectId == 2)
                {
                    double serviceGebyr = pakkeCountTotal * Models.FakturaPrisliste.pakkePriser.pakkeGebyr[TransportType_2_Id];

                    FakturaAddToCategori("Gebyr", "Servicegebyr pr. pakke", serviceGebyr.ToString(), FakturaLayout, out FakturaLayout);
                }

                if (transportSelectId == 1 || transportSelectId == 3)
                {
                    double gebyrBraend = nettofragt * (Models.FakturaPrisliste.kurerPriser.braendstof[TransportType_2_Id][TransportType_3_Id] / 100);

                    double gebyrMiljo = nettofragt * (Models.FakturaPrisliste.kurerPriser.miljoegebyr[TransportType_2_Id][TransportType_3_Id] / 100);
                    string adminGebyr = Models.FakturaPrisliste.kurerPriser.Adminnistrationsgebyr[TransportType_2_Id][TransportType_3_Id].ToString();

                    FakturaAddToCategori("Gebyr", "Brændstofgebyr Beregnes af nettofragt", gebyrBraend.ToString(), FakturaLayout, out FakturaLayout);
                    FakturaAddToCategori("Gebyr", "Miljøgebyr beregnes af nettofragt", gebyrMiljo.ToString(), FakturaLayout, out FakturaLayout);
                    FakturaAddToCategori("Gebyr", "Adminnistrationsgebyr pr. faktura", adminGebyr, FakturaLayout, out FakturaLayout);
                }
                #endregion Kategori - Gebyr

                #region Kategori- Etv. Bemærkninger

            string EkstraComment = new TextRange(RichTextBoxEktra_Comment.Document.ContentStart, RichTextBoxEktra_Comment.Document.ContentEnd).Text;

            if (CheckBoxEkstraText_Button_1.Background == Brushes.Red)
            {
                FakturaAddToCategori("Etv. Bemærkninger", EkstraComment, "", FakturaLayout, out FakturaLayout);
            }    

            #endregion Kategori- Etv. Bemærkninger
            
            #endregion Produkter/Pakker

            return FakturaLayout;
        }

        /// <summary>
        /// lav et tal til 2 decimaler
        /// </summary>
        private string DecimalTwo(string value)
        {
            double doubleValue = 0;
            double.TryParse(value, out doubleValue);

            return doubleValue.ToString("#.##");
        }

        /// <summary>
        /// om man skal kunne oprette en pdf
        /// tjekker om alle felter er udflydt
        /// </summary>
        private bool allowExePdf()
        {
            Class.Functions.DatagridFunc funcDG = new Class.Functions.DatagridFunc();
            //hvis false vil man ikke kunne gemme til pdf
            bool AllDone = true;

            string ErrorText = "## Fejl ##\n";

            //tjek faktura adresse
            if (TextBoxFaktura_customId.Text == "" || TextBoxFaktura_firma.Text == "" ||
                TextBoxFaktura_adresse.Text == "" || TextBoxFaktura_post.Text == "")
            {
                AllDone = false;
                ErrorText += "\nTjek Faktura addresse.";
            }

            
            //tjek Transport type
            bool checkforPackets = true;
            if (ComboBoxTransport_type1.SelectedIndex <= 0 || ComboBoxTransport_type2.SelectedIndex <= 0)
            {
                checkforPackets = false;
                AllDone = false;
                ErrorText += "\nTjek Transport type.";
            }
            //transport type kun for kurrer transport
            if (checkforPackets && ComboBoxTransport_type1.SelectedIndex == 1 && ComboBoxTransport_type3.SelectedIndex <= 0)
            {
                checkforPackets = false;
                AllDone = false;
                ErrorText += "\nTjek Transport type.";   
            }


            //find ud af om er minimum en pakke

            int pakkeRowCount = DataGridTransport_Pakker.Items.Count;
            bool miniumOneRow = false;
            bool miniumOneRowNotDone = false;

            if (checkforPackets)
            {
                if(ComboBoxTransport_type1.SelectedIndex == 1)
                {
                    for (int i = 0; i < pakkeRowCount; i++)
			        {
			            string refNumber = funcDG.GetCellValueTextBlock(DataGridTransport_Pakker,i,0),
                            decText = funcDG.GetCellValueTextBlock(DataGridTransport_Pakker,i,1),
                            kilo = funcDG.GetCellValueTextBlock(DataGridTransport_Pakker,i,2);
                        //tjek om der er minimum en der er helt udflydt
                        if (refNumber != "" && decText != "" && (kilo != "" || kilo != "0"))
                        {
                            miniumOneRow = true;
                        }

                        //hvis der er en pakke og der så er et flet der ikke er helt udflydt skal man kunne vælge mellem at forsætte eller stoppe 
                        else if (miniumOneRow && (refNumber == "" || decText == "" || (kilo == "" || kilo == "0")))
                        {
                            miniumOneRowNotDone = true;
                            break;
                        }
			        }
                }
                else if(ComboBoxTransport_type1.SelectedIndex == 2)
                {
                    for (int i = 0; i < pakkeRowCount; i++)
			        {
			            string refNumber = funcDG.GetCellValueTextBlock(DataGridTransport_Pakker,i,0),
                            pakkeStr = funcDG.GetDgComboboxData(DataGridTransport_Pakker,i,1, false),
                            kilo = funcDG.GetCellValueTextBlock(DataGridTransport_Pakker,i,2),
                            prices = funcDG.GetCellValueTextBlock(DataGridTransport_Pakker,i,3);

                        //tjek om der er minimum en der er helt udflydt
                        if (refNumber != "" && pakkeStr != "" && (kilo != "" || kilo != "0") && (prices != "" || prices != "0"))
                        {
                            miniumOneRow = true;
                        }

                        //hvis der er en pakke og der så er et flet der ikke er helt udflydt skal man kunne vælge mellem at forsætte eller stoppe 
                        else if (miniumOneRow && (refNumber == "" || pakkeStr == "" || (kilo == "" || kilo == "0") ||  (prices == "" || prices == "0")))
                        {
                            miniumOneRowNotDone = true;
                            break;
                        }
			        }
                }
                else
                {
                    for (int i = 0; i < pakkeRowCount; i++)
                    {
			            string refNumber = funcDG.GetCellValueTextBlock(DataGridTransport_Pakker,i,0),
                            takst = funcDG.GetDgComboboxData(DataGridTransport_Pakker,i,1, false),
                            pakkeStr = funcDG.GetDgComboboxData(DataGridTransport_Pakker,i,2,false),
                            beregnType = funcDG.GetDgComboboxData(DataGridTransport_Pakker,i,3,false),
                            lengthL = funcDG.GetCellValueTextBlock(DataGridTransport_Pakker,i,4),
                            widthtB = funcDG.GetCellValueTextBlock(DataGridTransport_Pakker,i,5),
                            heightH = funcDG.GetCellValueTextBlock(DataGridTransport_Pakker,i,6),
                            beregnKilo = funcDG.GetCellValueTextBlock(DataGridTransport_Pakker,i,7),
                            realKilo = funcDG.GetCellValueTextBlock(DataGridTransport_Pakker,i,8),
                            prices = funcDG.GetCellValueTextBlock(DataGridTransport_Pakker,i,9);

                        //tjek om der er minimum en der er helt udflydt
                        if (refNumber != "" && takst != "" && pakkeStr != "" && beregnType != "" && (lengthL != "" || lengthL != "0") &&
                            (widthtB != "" || widthtB != "0") && (heightH != "" || heightH != "0") && (beregnKilo != "" || beregnKilo != "0") &&
                             (realKilo != "" || realKilo != "0") && (prices != "" || prices != "0"))
                        {
                            miniumOneRow = true;
                        }

                        //hvis der er en pakke og der så er et flet der ikke er helt udflydt skal man kunne vælge mellem at forsætte eller stoppe 
                        else if (miniumOneRow && (refNumber == "" && takst == "" && pakkeStr == "" && beregnType == "" && (lengthL == "" || lengthL == "0") &&
                            (widthtB == "" || widthtB == "0") && (heightH == "" || heightH == "0") && (beregnKilo == "" || beregnKilo == "0") &&
                             (realKilo == "" || realKilo == "0") && (prices == "" || prices == "0")))
                        {
                            miniumOneRowNotDone = true;
                            break;
                        }
			        }
                }

                //hvis der ikke er minimun en row
                if (!miniumOneRow)
                {
                    AllDone = false;
                    ErrorText += "\nTjek Pakker.";
                }
            }

            if (AllDone)
            {
                //hvis der er fleter i godslinjer/pakker der ikke er færdige skal man have muligheden for at stoppe og ændre dem
                if (miniumOneRowNotDone)
                {
                    MessageBoxResult result = MessageBox.Show("Ikke alle felter er udflydt i dine pakker.\nVil du forsætte?", "Opret PDF", MessageBoxButton.YesNo);
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



        /// <summary>
        /// Tilføj eller Opret ny kategori i pdf
        /// </summary>
        /// <param name="kategori">Navn på kategori</param>
        /// <param name="information">Tekst på det er</param>
        /// <param name="prices">Prisen</param>
        /// <param name="FakturaInfo">FakturaLayout</param> 
        /// <param name="FakturaInfoOut">Udskriv til FakturaLayout</param>
        private void FakturaAddToCategori(string kategori, string information, string prices, Class.PdfCreate.FakturaLayout FakturaInfo, out Class.PdfCreate.FakturaLayout FakturaInfoOut)
        {
            //find ud af hvad kategori det er
            //tæl hvor man der er nu
            //kateCount vil starte på 0
            int kateCount = FakturaInfo.PakkeInfo.Count;
            int kateId = -1;

            //hvis kategori findes i forvejen hent index
            if (FakturaInfo.PakkeCategoryName.Exists(item => item.ToString() == kategori))
            {
                kateId = FakturaInfo.PakkeCategoryName.FindIndex(item => item.ToString() == kategori);
            }
            else{
                FakturaInfo.PakkeCategoryName.Add(kategori);
                kateId = kateCount;
                FakturaInfo.PakkeInfo.Add(new List<Class.PdfCreate.PakkeInfo>());
            }

            Class.PdfCreate.PakkeInfo newPakkeInfo = new Class.PdfCreate.PakkeInfo();
            newPakkeInfo.description = information;
            newPakkeInfo.prices = prices;

            if (this.savePaper)
            {
                if (FakturaInfo.PakkeInfo[kateId].Exists(item => (item as Class.PdfCreate.PakkeInfo).description == newPakkeInfo.description && (item as Class.PdfCreate.PakkeInfo).prices == newPakkeInfo.prices))
                {
                    int pakkeIndex = FakturaInfo.PakkeInfo[kateId].FindIndex(item => (item as Class.PdfCreate.PakkeInfo).description == newPakkeInfo.description && (item as Class.PdfCreate.PakkeInfo).prices == newPakkeInfo.prices);
                    FakturaInfo.PakkeInfo[kateId][pakkeIndex].count++;
                }
                else
                {
                    FakturaInfo.PakkeInfo[kateId].Add(newPakkeInfo);
                }
            }
            else
            {
                FakturaInfo.PakkeInfo[kateId].Add(newPakkeInfo);
            }


            FakturaInfoOut = FakturaInfo;
        }

        #endregion Functions

        #region Events

        /// <summary>
        /// åben start menu når dette vindeu lukker
        /// </summary>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (Models.ImportantData.openStartMenuOnClose)
            {
                MainWindow startmenu = new MainWindow();
                startmenu.Show();
            }
        }

        /// <summary>
        /// når vinduet er loade tillad at den 
        /// kan gå tilbage til startmenu 
        /// når dette vindue lukker
        /// </summary>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Models.ImportantData.openStartMenuOnClose = true;
        }

        #endregion Events
    }

    public class closeFragtBrevData 
    {
        public string fragtbrevName,
            rabat,
            tidsforbrugL,
            tidsforbrugV,
            tidsforbrugU,
            tidsforbrugH,
            priceAll;

        public DateTime dato,
            time;
    }
}




