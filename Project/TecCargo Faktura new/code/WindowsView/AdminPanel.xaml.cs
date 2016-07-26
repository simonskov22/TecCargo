﻿using System;
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
using System.Windows.Controls.DataVisualization.Charting;
using PieControls;

namespace TecCargo_Faktura.WindowsView
{
    /// <summary>
    /// Interaction logic for AdminPanel.xaml
    /// </summary>
    public partial class AdminPanel : Window
    {
        private bool closeOnX = true;
        
        private bool useBoldSettings = false;
        private List<FileInfoClass> fileInfoList = new FileInfoClass().GetFileList();
        private FileInfoKeyClass fileInfoValues = new FileInfoKeyClass();

        public AdminPanel()
        {
            InitializeComponent();
            DataContext = new Models.DataGridSources();
            //LoadPieChartData();
            LabelVersion_version.Content += Models.ImportantData.PVersion.ToString();


            LoadFilesList();

            loadCustomerList();

            //om der skal bruges fed skrift på det man skriver i pdf
            FileStream readSettingsFile = new FileStream(Models.ImportantData.g_FolderDB + "Settings.xml", FileMode.Open, FileAccess.Read);

            DataSet readSettings = new DataSet();
            readSettings.ReadXml(readSettingsFile);
            readSettingsFile.Close();

            useBoldSettings = bool.Parse(readSettings.Tables[0].Rows[0]["UseBold"].ToString());
            if (useBoldSettings)
            {
                var ColorConverter = new BrushConverter();
                ButtonAdmin_UseBoldPDF.Background = (Brush)ColorConverter.ConvertFrom("#FF2A8AEA");
                
            }
            CheckDataFolder();
        }

        #region Læs filer info

        private void loadCustomerList()
        {
            Class.XML_Files.Customer funcCustom = new Class.XML_Files.Customer();
            List<Class.XML_Files.Customer.Layout> customList = funcCustom.ReadCustomer();
                    
            var newItemsSourceCustom = new List<Models.AdminKundeDataClass>();

            int datasetTableCount = customList.Count;
            for (int i = 0; i < datasetTableCount; i++)
            {
                var customerDataItems = new Models.AdminKundeDataClass();

                customerDataItems.KontaktNumber = customList[i].ContactTlf;
                customerDataItems.FirmaName = customList[i].FirmName;
                customerDataItems.Adresse = customList[i].Address;
                customerDataItems.PostNumber = customList[i].ZipCode;

                newItemsSourceCustom.Add(customerDataItems);
            }

            if (datasetTableCount == 0)
            {
                ButtonAdminCustom_deleteAll.IsEnabled = false;
            }

            DataGridAdmin_CustomerData.ItemsSource = CollectionViewSource.GetDefaultView(newItemsSourceCustom);
            DataGridAdmin_CustomerData.Items.Refresh();
        }

        #endregion

        #region Buttons for datagrid

        private void LoadFilesList()
        {
            int fileCount = fileInfoList.Count;
            List<Models.emptyValues> fragtSaveSource = new List<Models.emptyValues>();
            List<Models.emptyValues> fragtPdfSource = new List<Models.emptyValues>();
            List<Models.emptyValues> faktSaveSource = new List<Models.emptyValues>();
            List<Models.emptyValues> faktPdfSource = new List<Models.emptyValues>();

            #region Hent itemsSource
            for (int i = 0; i < fileCount; i++)
            {
                Models.emptyValues newFileValue = new Models.emptyValues();
                newFileValue.value1 = fileInfoList[i].Filename;
                newFileValue.value2 = fileInfoList[i].CreateDateXML.ToShortDateString();
                newFileValue.value3 = fileInfoList[i].CreateDateXML.ToShortDateString();


                if (fileInfoList[i].FileType == fileInfoValues.Type_Fragt )
                {
                    //hvis der kun er en save fil
                    if (fileInfoList[i].Status == fileInfoValues.Status_Open)
                    {
                        fragtSaveSource.Add(newFileValue);
                    }
                    else
                    {
                        newFileValue.value3 = fileInfoList[i].CreateDatePDF.ToShortDateString();
                        fragtPdfSource.Add(newFileValue);
                        fragtSaveSource.Add(newFileValue);
                    }
                }
                else
                {
                    //hvis der kun er en save fil
                    if (fileInfoList[i].Status == fileInfoValues.Status_Open)
                    {
                        faktSaveSource.Add(newFileValue);
                    }
                    else
                    {
                        newFileValue.value3 = fileInfoList[i].CreateDatePDF.ToShortDateString();
                        faktPdfSource.Add(newFileValue);
                        faktSaveSource.Add(newFileValue);
                    }
                }
            }
            #endregion

            DataGridAdmin_FragtXmlData.ItemsSource = fragtSaveSource;
            DataGridAdmin_FragtPdfData.ItemsSource = fragtPdfSource;
            DataGridAdmin_FakturaXmlData.ItemsSource = faktSaveSource;
            DataGridAdmin_FakturaPdfData.ItemsSource = faktPdfSource;
        }

        private void Button_Click_Custom_Delete(object sender, RoutedEventArgs e)
        {
            Class.Functions.DatagridFunc funcDG = new Class.Functions.DatagridFunc();
            Class.XML_Files.Customer funcCustomer = new Class.XML_Files.Customer();

            int datagridRowsCount = DataGridAdmin_CustomerData.Items.Count;

            var item = ((Button)sender).DataContext;
            int removeIndex = DataGridAdmin_CustomerData.Items.IndexOf(item);

            string firmaName = (DataGridAdmin_CustomerData.Items[removeIndex] as Models.AdminKundeDataClass).FirmaName;

            var itemsView = DataGridAdmin_CustomerData.Items;

            (itemsView as System.ComponentModel.IEditableCollectionView).RemoveAt(removeIndex);

            //læs kunde information fra fil

            FileStream readDocument = new FileStream(Models.ImportantData.g_FolderData + "KundeInfo.xml", FileMode.Open, FileAccess.Read);
            DataSet customerRead = new DataSet();
            customerRead.ReadXml(readDocument);
            readDocument.Close();

            List<Class.XML_Files.Customer.Layout> CustomerList = funcCustomer.ReadCustomer();

            for (int i = 0; i < CustomerList.Count; i++)
            {
                if (firmaName == CustomerList[i].FirmName)
                {
                    customerRead.Tables[CustomerList[i].TableId].Rows[CustomerList[i].RowId].Delete();
                    break;
                }
            }


            //gem den nye kunde info fil
            FileStream readDocumentW = new FileStream(Models.ImportantData.g_FolderData + "KundeInfo.xml", FileMode.Create, FileAccess.Write);
            customerRead.WriteXml(readDocumentW);
            readDocumentW.Close();

            loadCustomerList();
        }
        private void Button_Click_Custom_DeleteAll(object sender, RoutedEventArgs e)
        {
            //læs kunde information fra fil

            FileStream readDocument = new FileStream(Models.ImportantData.g_FolderData + "KundeInfo.xml", FileMode.Open, FileAccess.Read);
            DataSet customerRead = new DataSet();
            customerRead.ReadXml(readDocument);
            readDocument.Close();

            int tableCount = customerRead.Tables.Count;
            for (int i = 0; i < tableCount; i++)
            {
                customerRead.Tables.RemoveAt(0);
            }
            FileStream readDocumentW = new FileStream(Models.ImportantData.g_FolderData + "KundeInfo.xml", FileMode.Create, FileAccess.Write);
            customerRead.WriteXml(readDocumentW);
            readDocumentW.Close();

            loadCustomerList();
        }

        private void Button_Click_fragtbrevPdf_Delete(object sender, RoutedEventArgs e)
        {
            Class.Functions.DatagridFunc funcDG = new Class.Functions.DatagridFunc();
            int datagridRowsCount = DataGridAdmin_FragtPdfData.Items.Count;

            var item = ((Button)sender).DataContext;
            int removeIndex = DataGridAdmin_FragtPdfData.Items.IndexOf(item);

            string filename = (DataGridAdmin_FragtPdfData.Items[removeIndex] as Models.emptyValues).value1;
            File.Delete(Models.ImportantData.g_FolderPdf + filename + ".pdf");

            var itemsView = DataGridAdmin_FragtPdfData.Items;

            (itemsView as System.ComponentModel.IEditableCollectionView).RemoveAt(removeIndex);
        }
        private void Button_Click_fragtbrevPdf_DeleteAll(object sender, RoutedEventArgs e)
        {
            Class.Functions.DatagridFunc funcDG = new Class.Functions.DatagridFunc();
            int datagridRowsCount = DataGridAdmin_FragtPdfData.Items.Count;

            for (int i = 0; i < datagridRowsCount; i++)
            {
                string filename = (DataGridAdmin_FragtPdfData.Items[i] as Models.emptyValues).value1;

                File.Delete(Models.ImportantData.g_FolderPdf + filename + ".pdf");
            }

            DataGridAdmin_FragtPdfData.ItemsSource = new List<Models.emptyValues>();
        }

        private void Button_Click_FakturaPdf_Delete(object sender, RoutedEventArgs e)
        {

            Class.Functions.DatagridFunc funcDG = new Class.Functions.DatagridFunc();
            int datagridRowsCount = DataGridAdmin_FakturaPdfData.Items.Count;

            var item = ((Button)sender).DataContext;
            int removeIndex = DataGridAdmin_FakturaPdfData.Items.IndexOf(item);

            string filename = (DataGridAdmin_FakturaPdfData.Items[removeIndex] as Models.emptyValues).value1;
            File.Delete(Models.ImportantData.g_FolderPdf + filename + ".pdf");

            var itemsView = DataGridAdmin_FakturaPdfData.Items;

            (itemsView as System.ComponentModel.IEditableCollectionView).RemoveAt(removeIndex);
        }
        private void Button_Click_FakturaPdf_DeleteAll(object sender, RoutedEventArgs e)
        {
            Class.Functions.DatagridFunc funcDG = new Class.Functions.DatagridFunc();
            int datagridRowsCount = DataGridAdmin_FakturaPdfData.Items.Count;

            for (int i = 0; i < datagridRowsCount; i++)
            {
                string filename = (DataGridAdmin_FakturaPdfData.Items[i] as Models.emptyValues).value1;

                File.Delete(Models.ImportantData.g_FolderPdf + filename + ".pdf");
            }

            DataGridAdmin_FakturaPdfData.ItemsSource = new List<Models.emptyValues>();
        }

        private void Button_Click_fragtbrevXml_Delete(object sender, RoutedEventArgs e)
        {
            Class.Functions.DatagridFunc funcDG = new Class.Functions.DatagridFunc();
            int datagridRowsCount = DataGridAdmin_FragtXmlData.Items.Count;

            var item = ((Button)sender).DataContext;
            int removeIndex = DataGridAdmin_FragtXmlData.Items.IndexOf(item);

            string filename = (DataGridAdmin_FragtXmlData.Items[removeIndex] as Models.emptyValues).value1;
            File.Delete(Models.ImportantData.g_FolderData + filename + ".xml");

            var itemsView = DataGridAdmin_FragtXmlData.Items;

            (itemsView as System.ComponentModel.IEditableCollectionView).RemoveAt(removeIndex);
        }
        private void Button_Click_fragtbrevXml_DeleteAll(object sender, RoutedEventArgs e)
        {
            Class.Functions.DatagridFunc funcDG = new Class.Functions.DatagridFunc();
            int datagridRowsCount = DataGridAdmin_FragtXmlData.Items.Count;

            for (int i = 0; i < datagridRowsCount; i++)
            {
                string filename = (DataGridAdmin_FragtXmlData.Items[i] as Models.emptyValues).value1;

                File.Delete(Models.ImportantData.g_FolderData + filename + ".xml");
            }

            DataGridAdmin_FragtXmlData.ItemsSource = new List<Models.emptyValues>();
        }

        private void Button_Click_FakturaXml_Delete(object sender, RoutedEventArgs e)
        {
            Class.Functions.DatagridFunc funcDG = new Class.Functions.DatagridFunc();
            int datagridRowsCount = DataGridAdmin_FakturaXmlData.Items.Count;

            var item = ((Button)sender).DataContext;
            int removeIndex = DataGridAdmin_FakturaXmlData.Items.IndexOf(item);

            string filename = (DataGridAdmin_FakturaXmlData.Items[removeIndex] as Models.emptyValues).value1;
            File.Delete(Models.ImportantData.g_FolderData + filename + ".xml");

            var itemsView = DataGridAdmin_FakturaXmlData.Items;

            (itemsView as System.ComponentModel.IEditableCollectionView).RemoveAt(removeIndex);
        }
        private void Button_Click_FakturaXml_DeleteAll(object sender, RoutedEventArgs e)
        {
            Class.Functions.DatagridFunc funcDG = new Class.Functions.DatagridFunc();
            int datagridRowsCount = DataGridAdmin_FakturaXmlData.Items.Count;

            for (int i = 0; i < datagridRowsCount; i++)
            {
                string filename = (DataGridAdmin_FakturaXmlData.Items[i] as Models.emptyValues).value1;

                File.Delete(Models.ImportantData.g_FolderData + filename + ".xml");
            }

            DataGridAdmin_FakturaXmlData.ItemsSource = new List<Models.emptyValues>();
        }

        #endregion Buttons for datagrid

        private void radioUpdatePieSelect_Checked(object sender, RoutedEventArgs e)
        {
            if ((sender as RadioButton).IsChecked.HasValue && (sender as RadioButton).IsChecked.Value)
            {
                LoadPieChartData();
            }

        }

        private void datePickDateUpdate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((sender as DatePicker).IsLoaded)
            {
                LoadPieChartData();   
            }
        }

        private void radioUpdateFaktPSelect_Checked(object sender, RoutedEventArgs e)
        {
            if (!this.IsLoaded)
            {
                return;
            }
            int index = int.Parse((sender as RadioButton).Name.Replace("radioSelectFaktp_",""));

            Class.Functions.DatagridFunc funcDG = new Class.Functions.DatagridFunc();

            switch (index)
            {
                case 0:
                case 1:
                    gridFaktMonth.Visibility = Visibility.Visible;
                    gridFaktYear.Visibility = Visibility.Visible;

                    int listItemCount = listviewFaktPrice_years.Items.Count;
                    for (int i = 0; i < listItemCount; i++)
                    {
                        listviewFaktPrice_years.ItemsSource = funcDG.removeIteamFromList(listviewFaktPrice_years.Items, 0);
                    }
                    
                    //Hent alle år til faktura pris
                    List<string> yearItemsSource = new List<string>();
                    for (int i = 0; i < fileInfoList.Count; i++)
                    {
                        if (
                            fileInfoList[i].FileType == fileInfoValues.Type_Faktura &&
                            fileInfoList[i].Status == fileInfoValues.Status_Closed &&
                            !yearItemsSource.Contains(fileInfoList[i].CreateYear.ToString())
                            )
                        {
                            yearItemsSource.Add(fileInfoList[i].CreateYear.ToString());
                        }
                    }
                    if (index == 1)
                    {
                        yearItemsSource.Add("Alle");
                    }
                    yearItemsSource.Sort();
                    yearItemsSource.Reverse();
                    comboboxFaktPrice_years.ItemsSource = yearItemsSource;
                    comboboxFaktPrice_years.SelectedIndex = 0;
                    break;
                case 2:
                    gridFaktMonth.Visibility = Visibility.Collapsed;
                    gridFaktYear.Visibility = Visibility.Collapsed;
                    break;
            }
        }

        private void radioButtonSearch_Checked(object sender, RoutedEventArgs e)
        {
            if (!this.IsLoaded)
            {
                return;
            }

            listViewSearch_Output.Visibility = Visibility.Collapsed;
            int index = int.Parse((sender as RadioButton).Name.Replace("radioButtonSearch_",""));
            

            gridSearch_KontP.Visibility = Visibility.Collapsed;
            gridSearch_Date.Visibility = Visibility.Collapsed;
            gridSearch_Post.Visibility = Visibility.Collapsed;

            switch (index)
            {
                case 0:
                    gridSearch_KontP.Visibility = Visibility.Visible;
                    break;
                case 1:
                    gridSearch_Date.Visibility = Visibility.Visible;
                    break;
                case 2:
                    gridSearch_Post.Visibility = Visibility.Visible;
                    break;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadPieChartData();
            LoadChartPieFragtFakt_Status();
            loadStatusList();
            UpdateFakturaPriceChart();
        }

        #region functions

        private void loadStatusList()
        {
            listViewFragt_NotDone.Items.Clear();
            listViewFakt_NotDone.Items.Clear();

            List<int> faktInvoiceList = new List<int>();
            //Hent alle faktura invoice
            for (int i = 0; i < fileInfoList.Count; i++)
            {
                if (fileInfoList[i].FileType == fileInfoValues.Type_Faktura)
                {
                    faktInvoiceList.Add(fileInfoList[i].Invoice);
                }
            }

            for (int i = 0; i < fileInfoList.Count; i++)
            {
                if (fileInfoList[i].Status == fileInfoValues.Status_Open ||
                    fileInfoList[i].Status == fileInfoValues.Status_Middle)
                {
                    string status = "";

                    if (fileInfoList[i].Status == fileInfoValues.Status_Open)
                    {
                        status = "Oprettet.";
                    }
                    else
                    {
                        status = "Åben.";
                    }
                    string owners = "";

                    for (int a = 0; a < fileInfoList[i].FileOwners.Count; a++)
                    {
                        owners += ", " + fileInfoList[i].FileOwners[a];
                    }

                    Models.emptyValues newEmptyV = new Models.emptyValues()
                    {
                        value1 = fileInfoList[i].Filename,
                        value2 = fileInfoList[i].CreateDateXML.ToShortDateString(),
                        value3 = owners.Substring(2),
                        value4 = status

                    };

                    if (fileInfoList[i].FileType == fileInfoValues.Type_Fragt)
                    {
                        listViewFragt_NotDone.Items.Add(newEmptyV);
                    }
                    else
                    {
                        listViewFakt_NotDone.Items.Add(newEmptyV);
                    }
                }
                else if (fileInfoList[i].FileType == fileInfoValues.Type_Fragt &&
                    !faktInvoiceList.Contains(fileInfoList[i].Invoice))
                {

                    string owners = "";
                    for (int a = 0; a < fileInfoList[i].FileOwners.Count; a++)
                    {
                        owners += ", " + fileInfoList[i].FileOwners[a];
                    }
                    Models.emptyValues newEmptyV = new Models.emptyValues()
                    {
                        value1 = fileInfoList[i].Filename.Replace("Fragtbrev","Faktura"),
                        value2 = fileInfoList[i].CreateDateXML.ToShortDateString(),
                        value3 = owners.Substring(2),
                        value4 = "Oprettet."

                    };
                    listViewFakt_NotDone.Items.Add(newEmptyV);
                }
            }
        }
        private void LoadChartPieFragtFakt_Status() 
        {

            int[] fragtDoneList = { 0, 0, 0 };
            int[] fakturaDoneList = { 0, 0, 0 };
            List<KeyValuePair<string, double>>[] pieCollectionArray = {
                new List<KeyValuePair<string, double>>(), 
                new List<KeyValuePair<string, double>>()
            };
            List<int> faktInvoiceList = new List<int>();
            //Hent alle faktura invoice
            for (int i = 0; i < fileInfoList.Count; i++)
            {
                if (fileInfoList[i].FileType == fileInfoValues.Type_Faktura)
                {
                    faktInvoiceList.Add(fileInfoList[i].Invoice);
                }
            }

            for (int i = 0; i < fileInfoList.Count; i++)
            {
                //find ud af hvormange fragtbrev der ikke er færdige
                if (fileInfoList[i].FileType == fileInfoValues.Type_Fragt)
                {
                    if (fileInfoList[i].Status == fileInfoValues.Status_Open)
                    {
                        fragtDoneList[0]++;
                    }
                    else if (fileInfoList[i].Status == fileInfoValues.Status_Middle)
                    {
                        fragtDoneList[1]++;   
                    }
                    else
                    {
                        fragtDoneList[2]++;

                        //hvis den ikke er oprettet som faktura endnu
                        //skal den står som åben i faktura status
                        if (!faktInvoiceList.Contains(fileInfoList[i].Invoice))
                        {
                            fakturaDoneList[0]++;
                        }
                    }
                }
                else
                {
                    if (fileInfoList[i].Status == fileInfoValues.Status_Open)
                    {
                        fakturaDoneList[0]++;
                    }
                    else if (fileInfoList[i].Status == fileInfoValues.Status_Middle)
                    {
                        fakturaDoneList[1]++;
                    }
                    else
                    {
                        fakturaDoneList[2]++;
                    }
                }
            }

            pieCollectionArray[0].Add(new KeyValuePair<string, double>("Oprettet", fragtDoneList[0]));
            pieCollectionArray[0].Add(new KeyValuePair<string, double>("Åben (Gemt som PDF)", fragtDoneList[1]));
            pieCollectionArray[0].Add(new KeyValuePair<string, double>("Afsluttet", fragtDoneList[2]));

            pieCollectionArray[1].Add(new KeyValuePair<string, double>("Oprettet", fakturaDoneList[0]));
            pieCollectionArray[1].Add(new KeyValuePair<string, double>("Åben (Gemt som PDF)", fakturaDoneList[1]));
            pieCollectionArray[1].Add(new KeyValuePair<string, double>("Afsluttet", fakturaDoneList[2]));

            ((PieSeries)pieFragt_Status.Series[0]).ItemsSource = pieCollectionArray[0];
            ((PieSeries)pieFaktura_Status.Series[0]).ItemsSource = pieCollectionArray[1];


        }
        private void UpdateFakturaPriceChart() 
        {
            chartFaktura_Price.Series.Clear();

            //find ud af hvilken radiobutton/type der er valgt
            int selectRadio = 0;
            for (int i = 0; i < 3; i++)
            {
                if ((this.FindName("radioSelectFaktp_" + i)as RadioButton).IsChecked.HasValue && 
                    (this.FindName("radioSelectFaktp_" + i)as RadioButton).IsChecked.Value)
                {
                    selectRadio = i;
                    break;
                }
            }
            
            string[] monthNameList = { 
                "Januar", "Februar", "Marts",
                "April", "Maj", "Juni", 
                "Juli", "August", "September",
                "Oktober", "November", "December" 
            };
            List<int> monthAllow = new List<int>();
            List<string> monthName = new List<string>();
            List<string> yearAllow = new List<string>();

            List<string> priceListCategory = new List<string>();
            List<string> priceListName = new List<string>();
            List<List<double>> priceListValues = new List<List<double>>();

            //find hvilken måneder der skal vise for
            if (selectRadio == 0 || selectRadio == 1)
            {
                string monthCheckboxName = "checkboxFaktPriceM_";
                for (int i = 0; i < 12; i++)
                {
                    if ((this.FindName(monthCheckboxName + i) as CheckBox).IsChecked.HasValue && (this.FindName(monthCheckboxName + i) as CheckBox).IsChecked.Value)
                    {
                        monthAllow.Add(i + 1);
                        monthName.Add(monthNameList[i]);
                    }
                }
            }

            switch (selectRadio)
            {
                #region Weeks
                case 0:

                    //find ud af vilket år den skal tage fra
                    yearAllow.Add(listviewFaktPrice_years.Items[0].ToString());
                   
                    //tilføj dage
                    for (int i = 0; i < monthAllow.Count; i++)
                    {
                        priceListCategory.Add(monthName[i]);
                        priceListValues.Add(new List<double>());
                        for (int a = 0; a < 5; a++)
                        {
                            priceListValues[i].Add(0);
                        }
                    }

                    for (int i = 0; i < 5; i++)
                    {
                        if (i == 4)
                        {
                            priceListName.Add("Dag : " + ((i + 1) * 7 - 6) + "-31" );
                        }
                        else
                        {
                            priceListName.Add("Dag : " + ((i + 1) * 7 - 6) + "-" + ((i + 1) * 7));
                        }
                        
                    }

                    for (int i = 0; i < fileInfoList.Count; i++)
                    {
                        if (
                            fileInfoList[i].FileType == fileInfoValues.Type_Faktura &&
                            fileInfoList[i].Status == fileInfoValues.Status_Closed &&
                            yearAllow.Contains(fileInfoList[i].CreateYear.ToString()) &&
                            monthAllow.Contains(fileInfoList[i].CreateMonth)
                            )
                        {
                            //tjek om kategori er blivet oprettet og find id
                            int categoryIndex = priceListCategory.FindIndex(item => item.ToLower() == fileInfoList[i].CreateMonthName.ToLower());
 

                            int dayIndex = int.Parse(fileInfoList[i].CreateDatePDF.ToString("dd"));
                            if (dayIndex <= 7) { dayIndex = 0; }
                            else if (dayIndex <= 14) { dayIndex = 1; }
                            else if (dayIndex <= 21) { dayIndex = 2; }
                            else if (dayIndex <= 28) { dayIndex = 3; }
                            else { dayIndex = 4; }

                            priceListValues[categoryIndex][dayIndex] += fileInfoList[i].Price;
                        }
                    }

                    break;
                #endregion
                #region Month
                case 1:
                    //hent månederne
                    priceListName = monthName;

                    //find hvilken år der skal vise for
                    for (int i = 0; i < listviewFaktPrice_years.Items.Count; i++)
                    {
                        if (listviewFaktPrice_years.Items[0].ToString() == "Alle")
                        {
                            for (int a = 0; a < fileInfoList.Count; a++)
                            {
                                if (fileInfoList[a].FileType == fileInfoValues.Type_Faktura &&
                                    fileInfoList[a].Status == fileInfoValues.Status_Closed &&
                                    !priceListCategory.Contains(fileInfoList[a].CreateYear.ToString()))
                                {
                                    priceListCategory.Add(fileInfoList[a].CreateYear.ToString());
                                    priceListValues.Add(new List<double>());

                                    for (int b = 0; b < monthAllow.Count; b++)
                                    {
                                        priceListValues[priceListValues.Count - 1].Add(0);
                                    }
                                }
                            }
                        }
                        else
                        {
                            priceListCategory.Add(listviewFaktPrice_years.Items[i].ToString());
                            priceListValues.Add(new List<double>());

                            for (int a = 0; a < monthAllow.Count; a++)
                            {
                                priceListValues[i].Add(0);
                            }
                        }
                        //hvilken år der er tilladt
                        yearAllow.Add(listviewFaktPrice_years.Items[i].ToString());
                    }
                    priceListCategory.Sort();
                    priceListCategory.Reverse();

                    //Lav chart
                    List<KeyValuePair<string, double>> faktItemsSource = new List<KeyValuePair<string, double>>();
                    List<int> monthUsed = new List<int>();

                    //Opret valgte kategorier
                    for (int i = 0; i < fileInfoList.Count; i++)
                    {
                        //Kun for faktura som er i pdf
                        //og om månederne og år passer
                        if (fileInfoList[i].FileType == fileInfoValues.Type_Faktura &&
                            fileInfoList[i].Status == fileInfoValues.Status_Closed &&
                            monthAllow.Contains(fileInfoList[i].CreateMonth) &&
                            (yearAllow.Contains(fileInfoList[i].CreateYear.ToString()) || yearAllow.Contains("Alle")))
                        {
                            //tjek om kategori er blivet oprettet og find id
                            int categoryIndex = priceListCategory.FindIndex(item => item == fileInfoList[i].CreateYear.ToString());

                            int monthIndex = monthAllow.FindIndex(item => item == fileInfoList[i].CreateMonth);
                            priceListValues[categoryIndex][monthIndex] += fileInfoList[i].Price;
                        }
                    }
                    break;
                #endregion
                #region Years
                case 2:
                    priceListCategory.Add("Alle");
                    priceListValues.Add(new List<double>());

                    //find hvilken år der skal vise for
                    for (int a = 0; a < fileInfoList.Count; a++)
                    {
                        if (fileInfoList[a].FileType == fileInfoValues.Type_Faktura &&
                            fileInfoList[a].Status == fileInfoValues.Status_Closed && 
                            !priceListName.Contains(fileInfoList[a].CreateYear.ToString()))
                        {
                            priceListName.Add(fileInfoList[a].CreateYear.ToString());
                        }
                    }
                    priceListName.Sort();
                    priceListName.Reverse();

                    for (int i = 0; i < priceListName.Count; i++)
                    {
                        priceListValues[0].Add(0);
                    }

                    //Opret valgte kategorier
                    for (int i = 0; i < fileInfoList.Count; i++)
                    {
                        //Kun for faktura som er i pdf
                        //og om månederne og år passer
                        if (
                            fileInfoList[i].FileType == fileInfoValues.Type_Faktura &&
                            fileInfoList[i].Status == fileInfoValues.Status_Closed
                            )
                        {
                            //tjek om kategori er blivet oprettet og find id
                            int yearIndex = priceListName.FindIndex(item => item == fileInfoList[i].CreateYear.ToString());
                            double priceAdd = fileInfoList[i].Price;
                            priceListValues[0][yearIndex] += priceAdd;
                        }
                    }
                    break;
                #endregion
            }
            int categoryCount = priceListCategory.Count;
            List<List<KeyValuePair<string, double>>> fakturaPriceItemssource = new List<List<KeyValuePair<string, double>>>();

            for (int i = 0; i < categoryCount; i++)
            {
                fakturaPriceItemssource.Add(new List<KeyValuePair<string, double>>());

                for (int a = 0; a < priceListName.Count; a++)
                {
                    fakturaPriceItemssource[i].Add(new KeyValuePair<string, double>(priceListName[a], priceListValues[i][a]));
                }

            }

            //lav kategorier
            for (int i = 0; i < categoryCount; i++)
            {
                ColumnSeries newColumnSeries = new ColumnSeries();
                newColumnSeries.Title = priceListCategory[i];
                newColumnSeries.IndependentValueBinding = new Binding("Key");
                newColumnSeries.DependentValueBinding = new Binding("Value");

                chartFaktura_Price.Series.Add(newColumnSeries);
                ((ColumnSeries)chartFaktura_Price.Series[i]).ItemsSource = fakturaPriceItemssource[i];
            }
        }
        private void LoadPieChartData()
        {
            //Skal kun køre hvis vinduet er loaded
            if (!this.IsLoaded)
            {
                return;
            }
            string thisFolder = Directory.GetCurrentDirectory() + @"\";

            List<string>[] dateNamePDF = { 
                                             new List<string>(), 
                                             new List<string>() 
                                         };
            List<double>[] dateCountPDF = { 
                                             new List<double>(), 
                                             new List<double>() 
                                         };
            List<double> fakturaDateCount = new List<double>();


            List<KeyValuePair<string, double>>[] pieCollectionArray = {
                new List<KeyValuePair<string, double>>(), 
                new List<KeyValuePair<string, double>>(),
            };
            
            #region Fragtbrev og faktura lavet på en bestem dato

            //Radio buttons
            //0 = fragtbrev
            //1 = faktura
            bool[][][] radioButton = 
            {
                new bool[][]{
                    new bool[] { radioSelectFragtUge.IsChecked.HasValue, radioSelectFragtUge.IsChecked.Value },
                    new bool[] { radioSelectFragtMouth.IsChecked.HasValue, radioSelectFragtMouth.IsChecked.Value }
                },
                new bool[][]{
                    new bool[] { radioSelectFakturaUge.IsChecked.HasValue , radioSelectFakturaUge.IsChecked.Value },
                    new bool[] { radioSelectFakturaMouth.IsChecked.HasValue, radioSelectFakturaMouth.IsChecked.Value }
                }
            };
             
            //indefor hvad tids rum
            DateTime[] startDate = new DateTime[2];
            DateTime[] endDate = new DateTime[2];

            try
            {
                startDate[0] = datePickDateFragtS.SelectedDate.Value;
                endDate[0] = datePickDateFragtE.SelectedDate.Value;
                startDate[1] = datePickDateFakturaS.SelectedDate.Value;
                endDate[1] = datePickDateFakturaE.SelectedDate.Value;

                endDate[0] = endDate[0].AddDays(1).AddMilliseconds(-1);
                endDate[1] = endDate[1].AddDays(1).AddMilliseconds(-1);
            }
            catch (Exception)
            {
                return;
            }
            
            for (int i = 0; i < fileInfoList.Count; i++)
            {
                //MessageBox.Show(fileInfoList[i].Filename + "\n" + fileInfoList[i].FileType + " == " + fileInfoValues.Type_Fragt+"\n"+
                  //  fileInfoList[i].Status + " == " + fileInfoValues.Status_Closed);

                if (fileInfoList[i].Status == fileInfoValues.Status_Closed)
                {
                    //År filen blev lavet
                    string dateListInfo = fileInfoList[i].CreateYear + " | ";
                    int selectId = 0; //0 = Fragtbrev ||| 1 = Faktura
                    
                    if (fileInfoList[i].FileType == fileInfoValues.Type_Fragt)
                    {
                    //    MessageBox.Show("1");
                        selectId = 0;
                    }
                    else
                    {
                        //MessageBox.Show("2");
                        selectId = 1;
                    }

                    //tjek om tiden IKKE er inden for det man har valgt
                    if (DateTime.Compare(fileInfoList[i].CreateDatePDF, startDate[selectId]) < 0 || DateTime.Compare(fileInfoList[i].CreateDatePDF, endDate[selectId]) > 0)
                    {
                        break;
                    }

                    //Radio button hvad der skal visse
                    if (radioButton[selectId][0][0] && radioButton[selectId][0][1]) //Uge
                    {
                        dateListInfo += "Uge: " + GetWeekNumber(fileInfoList[i].CreateDatePDF).ToString();
                    }
                    else if (radioButton[selectId][1][0] && radioButton[selectId][1][1]) //Måned
                    {
                        dateListInfo += fileInfoList[i].CreateMonthName;
                    }
                    else //År
                    {
                        dateListInfo = dateListInfo.Substring(0, dateListInfo.Length - 3);
                    }

                    //tjek om dateListInfo kategori er lavet
                    int dateIndex = 0;
                    if (!dateNamePDF[selectId].Contains(dateListInfo))
                    {
                        dateNamePDF[selectId].Add(dateListInfo);
                        dateIndex = dateNamePDF[selectId].Count - 1;
                        dateCountPDF[selectId].Add(0);
                    }
                    else
                    {
                        dateIndex = dateNamePDF[selectId].FindIndex(item => item == dateListInfo);
                    }

                    dateCountPDF[selectId][dateIndex]++;

                }
            }
            pieCollectionArray[0] = GetPieChartData(dateNamePDF[0].ToArray(), dateCountPDF[0].ToArray());
            pieCollectionArray[1] = GetPieChartData(dateNamePDF[1].ToArray(), dateCountPDF[1].ToArray());

            #endregion
            #region Set chart

            ((PieSeries)pieFragt_PdfDone.Series[0]).ItemsSource = pieCollectionArray[0];
            ((PieSeries)pieFaktura_PdfDone.Series[0]).ItemsSource = pieCollectionArray[1];
            
            #endregion
        }
        private void CheckDataFolder()
        { 
            DataSet settingsData = new DataSet();
            string settingsFileName = Models.ImportantData.g_FolderDB + "Settings.xml";

            FileStream settingsFileR = new FileStream(settingsFileName, FileMode.Open, FileAccess.Read);
            settingsData.ReadXml(settingsFileR);
            settingsFileR.Close();

            string savePath = settingsData.Tables[0].Rows[0]["SavePath"].ToString();

            if (savePath != "None")
            {
                ButtonAdmin_ChangeFolderDefault.Visibility = Visibility.Visible;
            }
        }

        private int GetWeekNumber(DateTime date) 
        {
            var currentCulture = System.Globalization.CultureInfo.CurrentCulture;
            int weekNo = currentCulture.Calendar.GetWeekOfYear(
                            date,
                            currentCulture.DateTimeFormat.CalendarWeekRule,
                            currentCulture.DateTimeFormat.FirstDayOfWeek);
            return weekNo;
        }
        
        private List<string> AddIteamFromList(ItemCollection itemsList, string value)
        {
            List<string> itemsSourceV = new List<string>();
            for (int i = 0; i < itemsList.Count; i++)
            {
                    itemsSourceV.Add(itemsList[i].ToString());
            }
            itemsSourceV.Add(value);

            itemsSourceV.Sort();
            itemsSourceV.Reverse();

            return itemsSourceV;
        }
        private List<KeyValuePair<string, double>> GetPieChartData(string[] Names, double[] values) 
        {
            //find ud af value helt tal
            double valueAll = 0;
            for (int i = 0; i < values.Count(); i++)
            {
                valueAll += values[i];
            }

            //lav list
            List<KeyValuePair<string, double>> pieData = new List<KeyValuePair<string, double>>();
            for (int i = 0; i < Names.Count(); i++)
            {
                double procent = (values[i] / valueAll) * 100;  
 
                pieData.Add(new KeyValuePair<string, double> (Names[i] + " (" + procent.ToString("F") + "%)", values[i]));
            }

            return pieData;
        }

        #endregion

        #region onClick Functions

        private void ButtonSearchFragt_Click(object sender, RoutedEventArgs e)
        {
            listViewSearch_Output.Items.Clear();

            //Find ud af vil type søg der skal bruges
            string radioButtonName = "radioButtonSearch_";
            int radioIndex = 0;
            for (int i = 0; i < 3; i++)
            {
                if ((FindName(radioButtonName + i) as RadioButton).IsChecked.HasValue && (FindName(radioButtonName + i) as RadioButton).IsChecked.Value)
                {
                    radioIndex = i;
                    break;
                }
            }

            string kontaktP = "";
            int post = -1;
            DateTime selectDateS = new DateTime();
            DateTime selectDateE = new DateTime();
            switch (radioIndex)
            {
                case 0:
                    kontaktP = textBoxFragt_KontP.Text;
                    break;
                case 1:
                    selectDateS = DateTime.Parse(datepickerSearchS.SelectedDate.Value.ToShortDateString());
                    selectDateE = DateTime.Parse(datepickerSearchE.SelectedDate.Value.ToShortDateString());
                    break;
                case 2:
                    int.TryParse(textBoxFragt_post.Text, out post);
                    break;

            }
            for (int i = 0; i < fileInfoList.Count; i++)
            {
                if (fileInfoList[i].FileType == fileInfoValues.Type_Fragt)
                {
                    bool isFound = false;

                    switch (radioIndex)
                    {
                        case 0:
                            if (fileInfoList[i].KontaktPersonAfsend.ToLower() == kontaktP.ToLower() || fileInfoList[i].KontaktPersonModtag.ToLower() == kontaktP.ToLower())
                            {
                                isFound = true;
                            }
                            break;
                        case 1:
                            if (fileInfoList[i].CreateDateXML >= selectDateS && fileInfoList[i].CreateDateXML <= selectDateE)
                            {
                                isFound = true;
                            }
                            break;
                        case 2:
                            if ((post != -1) && (fileInfoList[i].PostAfsend == post || fileInfoList[i].PostModtag == post))
                            {
                                isFound = true;
                            }
                            break;

                    }
                    if (isFound)
                    {
                        string owners = "";
                        for (int a = 0; a < fileInfoList[i].FileOwners.Count; a++)
                        {
                            owners += ", " + fileInfoList[i].FileOwners[a];
                        }

                        //find status for fragtbrev
                        string fragtStatus = "Ikke Oprettet.";
                        string faktStatus = "Ikke Oprettet.";
                        if (fileInfoList[i].Status == fileInfoValues.Status_Closed)
                        {
                            fragtStatus = "Afsluttet.";
                            faktStatus = "Oprettet.";
                            //find status for faktura
                            for (int a = 0; a < fileInfoList.Count; a++)
                            {
                                if (fileInfoList[a].FileType == fileInfoValues.Type_Faktura && fileInfoList[i].Invoice == fileInfoList[a].Invoice)
                                {
                                    
                                    if (fileInfoList[a].Status == fileInfoValues.Status_Closed)
                                    {
                                        faktStatus = "Afsluttet.";
                                    }
                                    else if (fileInfoList[a].Status == fileInfoValues.Status_Middle)
                                    {
                                        faktStatus = "Åben.";
                                    }
                                    else
                                    {
                                        faktStatus = "Oprettet.";
                                    }

                                    for (int b = 0; b < fileInfoList[a].FileOwners.Count; b++)
                                    {
                                        if (!fileInfoList[i].FileOwners.Contains(fileInfoList[a].FileOwners[b]))
                                        {
                                            owners += ", " + fileInfoList[a].FileOwners[b];
                                        }
                                    }
                                    break;
                                }
                            }
                        }
                        else if (fileInfoList[i].Status == fileInfoValues.Status_Middle)
                        {
                            fragtStatus = "Åben.";
                        }
                        else
                        {
                            fragtStatus = "Oprettet.";
                        }


                        listViewSearch_Output.Items.Add(new Models.emptyValues() {
                            value1 = fileInfoList[i].Invoice.ToString(),
                            value2 = fileInfoList[i].CreateDateXML.ToShortDateString(),
                            value3 = owners.Substring(2),
                            value4 = fragtStatus,
                            value5 = faktStatus
                        });
                    }
                }
            }

            listViewSearch_Output.Visibility = Visibility.Visible;
        }
        private void listviewFragtSearchButton_PDF_Click(object sender, RoutedEventArgs e)
        {
            var item = ((Button)sender).DataContext;
            int Index = listViewSearch_Output.Items.IndexOf(item);
            Models.emptyValues classValues = listViewSearch_Output.Items[Index] as Models.emptyValues;

            if (File.Exists(Models.ImportantData.g_FolderPdf + "Fragtbrev-" + classValues.value1 + ".pdf"))
            {
                System.Diagnostics.Process.Start(Models.ImportantData.g_FolderPdf + "Fragtbrev-" + classValues.value1 + ".pdf");
            }
        }
        private void listviewFaktSearchButton_PDF_Click(object sender, RoutedEventArgs e)
        {
            var item = ((Button)sender).DataContext;
            int Index = listViewSearch_Output.Items.IndexOf(item);
            Models.emptyValues classValues = listViewSearch_Output.Items[Index] as Models.emptyValues;

            if (File.Exists(Models.ImportantData.g_FolderPdf + "Faktura-" + classValues.value1 + ".pdf"))
            {
                System.Diagnostics.Process.Start(Models.ImportantData.g_FolderPdf + "Faktura-" + classValues.value1 + ".pdf");
            }
        }
        
        private void ButtonFaktPriceChart_Click(object sender, RoutedEventArgs e)
        {
            UpdateFakturaPriceChart();
        } 
        private void listviewFaktPrice_years_button_removeClick(object sender, RoutedEventArgs e)
        {
            Class.Functions.DatagridFunc funcDG = new Class.Functions.DatagridFunc();

            var item = ((Button) sender).DataContext;
            int removeIndex = listviewFaktPrice_years.Items.IndexOf(item);
            string removeName = listviewFaktPrice_years.Items[removeIndex].ToString();
            listviewFaktPrice_years.ItemsSource = funcDG.removeIteamFromList(listviewFaktPrice_years.Items, removeIndex);

            var itemsSourceList = AddIteamFromList(comboboxFaktPrice_years.Items, removeName);
            itemsSourceList.Sort();
            itemsSourceList.Reverse();
            comboboxFaktPrice_years.ItemsSource = itemsSourceList;
        }
        private void buttonFaktPrice_years_Click(object sender, RoutedEventArgs e)
        {
            Class.Functions.DatagridFunc funcDG = new Class.Functions.DatagridFunc();

            //hent id på combox og hvis den ikke har et resulat stop
            int yearsComboId = comboboxFaktPrice_years.SelectedIndex;

            if (yearsComboId == -1)
            {
                return;
            }
            int radioIndex = 0;
            for (int i = 0; i < 2; i++)
            {
                if ((FindName("radioSelectFaktp_" + i) as RadioButton).IsChecked.HasValue && (FindName("radioSelectFaktp_" + i) as RadioButton).IsChecked.Value)
                {
                    radioIndex = i;
                    break;
                }   
            }

            //hent året og tilføj det til listview
            string yearsComboName = comboboxFaktPrice_years.SelectedValue.ToString();
            int rowCount = listviewFaktPrice_years.Items.Count;

            switch (radioIndex)
            {
                case 0: //Kan kun være en valgt som ikke er alle
                    if (yearsComboName == "Alle")
                    {
                        return;
                    }

                    
                    for (int i = 0; i < rowCount; i++)
                    {
                        comboboxFaktPrice_years.ItemsSource = AddIteamFromList(comboboxFaktPrice_years.Items, listviewFaktPrice_years.Items[0].ToString());
                        listviewFaktPrice_years.ItemsSource = funcDG.removeIteamFromList(listviewFaktPrice_years.Items, 0);
                    }

                    break;

                case 1:
                    if (yearsComboName == "Alle")
                    {
                        //fjern alle values fra listbox og tilføj dem til combobox
                        for (int i = 0; i < rowCount; i++)
                        {
                            comboboxFaktPrice_years.ItemsSource = AddIteamFromList(comboboxFaktPrice_years.Items, listviewFaktPrice_years.Items[0].ToString());
                            listviewFaktPrice_years.ItemsSource = funcDG.removeIteamFromList(listviewFaktPrice_years.Items, 0);
                        }
                    }
                    if (listviewFaktPrice_years.Items.Count == 1 && listviewFaktPrice_years.Items[0].ToString() == "Alle")
                    {
                        //fjern alle fra listview og tilføj den til combobox
                        listviewFaktPrice_years.ItemsSource = funcDG.removeIteamFromList(listviewFaktPrice_years.Items, 0);
                        comboboxFaktPrice_years.ItemsSource = AddIteamFromList(comboboxFaktPrice_years.Items, "Alle");
                        comboboxFaktPrice_years.Items.Refresh();
                        //find id på det valgt igen nu hvor alle er blivet tilføjet
                        int comboboxCount = comboboxFaktPrice_years.Items.Count;
                        for (int i = 0; i < comboboxCount; i++)
                        {
                            if (comboboxFaktPrice_years.Items[i].ToString() == yearsComboName)
                            {
                                yearsComboId = i;
                                break;
                            }
                        }
                    }
                    break;
            }

            //tilføj ny value og fjern den gamle fra combobox
            comboboxFaktPrice_years.ItemsSource = funcDG.removeIteamFromList(comboboxFaktPrice_years.Items, yearsComboId);
            listviewFaktPrice_years.ItemsSource = AddIteamFromList(listviewFaktPrice_years.Items, yearsComboName);

            comboboxFaktPrice_years.SelectedIndex = 0;
        }
        private void ButtonAdmin_Startmenu_Click(object sender, RoutedEventArgs e)
        {
            this.closeOnX = false;

            MainWindow startMenu = new MainWindow();
            startMenu.Show();
            this.Close();
        }
        private void ButtonAdmin_UseBoldPDF_Click(object sender, RoutedEventArgs e)
        {
            //Sæt farve HEX
            var ColorConverter = new BrushConverter();

            //læs settings fil
            FileStream readSettingsFile = new FileStream(Models.ImportantData.g_FolderDB + "Settings.xml", FileMode.Open, FileAccess.ReadWrite);

            DataSet readSettings = new DataSet();
            readSettings.ReadXml(readSettingsFile);
            readSettingsFile.Close();

            if (useBoldSettings)
            {
                useBoldSettings = false;
                readSettings.Tables["Settings"].Rows[0][0] = "false";

                ButtonAdmin_UseBoldPDF.Background = (Brush)ColorConverter.ConvertFrom("#FFDDDDDD");
            }
            else
            {
                useBoldSettings = true;
                readSettings.Tables["Settings"].Rows[0][0] = "true";

                ButtonAdmin_UseBoldPDF.Background = (Brush)ColorConverter.ConvertFrom("#FF2A8AEA");
            }
            readSettings.Tables["Settings"].AcceptChanges();

            //Skriv til settings fil
            FileStream WriteSettingsFile = new FileStream(Models.ImportantData.g_FolderDB + "Settings.xml", FileMode.Create, FileAccess.Write);

            readSettings.WriteXml(WriteSettingsFile);
            WriteSettingsFile.Close();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            LoadPieChartData();
        }
        private void ButtonAdmin_ResetToNew_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Indstil som ny?", "Reset", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {

                try
                {
                    string folderSave = Models.ImportantData.g_FolderSave;
                    string folderPDF = Models.ImportantData.g_FolderPdf;
                    string folderDB = Models.ImportantData.g_FolderData;

                    //Slet save files
                    foreach (var filename in Directory.GetFiles(folderSave))
                    {
                        File.Delete(filename);
                    }
                    //Slet PDF files
                    foreach (var filename in Directory.GetFiles(folderPDF))
                    {
                        File.Delete(filename);
                    }

                    // kunde database
                    DataSet createDbSet = new DataSet();
                    createDbSet.DataSetName = "KundeInfo";

                    FileStream createNewDb = new FileStream(folderDB + "KundeInfo.xml", FileMode.Create, FileAccess.Write);
                    createDbSet.WriteXml(createNewDb);
                    createNewDb.Close();

                    DataSet settingsSet = new DataSet();
                    FileStream settingsR = new FileStream(Models.ImportantData.g_FolderDB + "Settings.xml", FileMode.Open, FileAccess.Read);
                    settingsSet.ReadXml(settingsR);
                    settingsR.Close();
                    settingsSet.Tables[0].Rows[0]["NextInvoice"] = "1000";

                    FileStream settingsW = new FileStream(Models.ImportantData.g_FolderDB + "Settings.xml", FileMode.Create, FileAccess.Write);
                    settingsSet.WriteXml(settingsW);
                    settingsW.Close();

                    fileInfoList = new FileInfoClass().GetFileList();
                    LoadFilesList();

                    loadCustomerList();

                    ButtonFilesUpdate.RaiseEvent(new RoutedEventArgs(System.Windows.Controls.Primitives.ButtonBase.ClickEvent));

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Fejl: " + ex.Message + "\n\nPrøv igen.");
                }
                finally
                {
                    MessageBox.Show("Nu indstillet som ny.");
                }
            }
        }
        private void ButtonAdmin_ChangeFolder_Click(object sender, RoutedEventArgs e)
        {
            Class.XML_Files.Customer funcCustomer = new Class.XML_Files.Customer();

            //Hent harddisk bogstav
            string hddLetter = System.IO.Path.GetPathRoot(Directory.GetCurrentDirectory());
            //MessageBox.Show(hddLetter);

            var folderDialog = new System.Windows.Forms.FolderBrowserDialog();
            folderDialog.Description = "Opret eller vælg en mappe, som fragtbreve og faktura vil blive gemt i.";
            folderDialog.SelectedPath = Directory.GetCurrentDirectory();

            System.Windows.Forms.DialogResult folderResult = folderDialog.ShowDialog();
            if (folderResult == System.Windows.Forms.DialogResult.OK)
            {
                string selectPath = folderDialog.SelectedPath;

                MessageBoxResult hideMsg = MessageBox.Show("Skjult mappen : \"" + selectPath +  "\"?", "Skjul Mappe", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes);

                if (hideMsg == MessageBoxResult.Yes)
                {
                    DirectoryInfo folderInfo = new DirectoryInfo(selectPath);
                    folderInfo.Attributes = FileAttributes.Directory | FileAttributes.Hidden;

                    //om mappen indeholder undermapper
                    int subfolderCount = Directory.GetDirectories(selectPath).Length;
                    int subFilesCount = Directory.GetFiles(selectPath).Length;
                    if (subfolderCount != 0 || subFilesCount != 0)
                    {
                        MessageBoxResult hideMsgSubF = MessageBox.Show("Mappen indeholder undermapper/filer.\nHvis du skjuler mappen, kan nogle programmer måske ikke virke.\nForsæt?", "Indeholder undermapper/filer", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.Yes);
                        if (hideMsgSubF == MessageBoxResult.No)
                        {
                            folderInfo.Attributes = FileAttributes.Directory | FileAttributes.Normal;
                        }
                    }
                }
                else 
                {
                    DirectoryInfo folderInfo = new DirectoryInfo(selectPath);
                    folderInfo.Attributes = FileAttributes.Directory | FileAttributes.Normal;
                }
                //Opret save og pdf mapper
                string saveFolderName = @"\Gemte Filer\";
                string PdfFolderName = @"\Pdf\";
                string DbFolderName = @"\Data\";
                if (!Directory.Exists(selectPath + saveFolderName))
                {
                    DirectoryInfo di = Directory.CreateDirectory(selectPath + saveFolderName);
                    di.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
                }

                if (!Directory.Exists(selectPath + PdfFolderName))
                {
                    DirectoryInfo di = Directory.CreateDirectory(selectPath + PdfFolderName);
                    di.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
                }

                if (!Directory.Exists(selectPath + DbFolderName))
                {
                    DirectoryInfo di = Directory.CreateDirectory(selectPath + DbFolderName);
                    di.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
                }

                //flyt filer (save og pdf)

                //Flyt kunde dataInfo


                List<Class.XML_Files.Customer.Layout> allOldCustomer = funcCustomer.ReadCustomer(Models.ImportantData.g_FolderData);
                List<Class.XML_Files.Customer.Layout> allNewCustomer = funcCustomer.ReadCustomer(selectPath + DbFolderName);

                int oldCustomerCount = allOldCustomer.Count;

                for (int i = 0; i < oldCustomerCount; i++)
                {
                    funcCustomer.SaveCustomer(allOldCustomer[i], selectPath + DbFolderName);
                }

                string error = "Kunne ikke flytte:\n\n";
                bool errorFound = false;

                string[] saveFileN_Names = Directory.GetFiles(selectPath + saveFolderName).Select(id => System.IO.Path.GetFileName(id)).ToArray();
                
                string[] saveFileNamesSrc = Directory.GetFiles(Models.ImportantData.g_FolderSave);
                int saveFilesCount = saveFileNamesSrc.Length;
                for (int i = 0; i < saveFilesCount; i++)
                {
                    string fileName = System.IO.Path.GetFileName(saveFileNamesSrc[i]);
                    
                    if (!saveFileN_Names.Contains(fileName))
                    {
                        
                        File.Move(saveFileNamesSrc[i], selectPath + saveFolderName + fileName);
                    }
                    else
                    {
                        error += saveFileNamesSrc[i] + "\n";
                        errorFound = true;
                    }
                    
                }

                string[] pdfFileN_Names = Directory.GetFiles(selectPath + PdfFolderName).Select(id => System.IO.Path.GetFileName(id)).ToArray();
                string[] pdfFileNamesSrc = Directory.GetFiles(Models.ImportantData.g_FolderPdf);
                int pdfFilesCount = pdfFileNamesSrc.Length;
                for (int i = 0; i < pdfFilesCount; i++)
                {
                    string fileName = System.IO.Path.GetFileName(pdfFileNamesSrc[i]);
                    if (!pdfFileN_Names.Contains(fileName))
                    {
                        File.Move(pdfFileNamesSrc[i], selectPath + PdfFolderName + fileName);
                    }
                    else
                    {
                        error += pdfFileNamesSrc[i] + "\n";
                        errorFound = true;
                    }
                }

                if (errorFound)
                {
                    MessageBox.Show(error);
                }

                //updatere mappe navne til de nye
                Models.ImportantData.g_FolderPdf = selectPath + @"\Pdf\";
                Models.ImportantData.g_FolderSave = selectPath + @"\Gemte filer\";
                Models.ImportantData.g_FolderData = selectPath + @"\Data\";

                //Gem den nye sti
                DataSet settingsData = new DataSet();
                string settingsFileName = Models.ImportantData.g_FolderDB + "Settings.xml";

                FileStream settingsFileR = new FileStream(settingsFileName, FileMode.Open, FileAccess.Read);
                settingsData.ReadXml(settingsFileR);
                settingsFileR.Close();

                settingsData.Tables[0].Rows[0]["SavePath"] = selectPath;

                FileStream settingsFileW = new FileStream(settingsFileName, FileMode.Create, FileAccess.Write);
                settingsData.WriteXml(settingsFileW);
                settingsFileW.Close();

                ButtonAdmin_ChangeFolderDefault.Visibility = Visibility.Visible;
            }
        }
        private void ButtonAdmin_ChangeFolderDefault_Click(object sender, RoutedEventArgs e)
        {
            Class.XML_Files.Customer funcCustomer = new Class.XML_Files.Customer();

            //Opret save og pdf mapper
            string defaultSave = Directory.GetCurrentDirectory() + @"\Gemte Filer\";
            string defaultPDF = Directory.GetCurrentDirectory() + @"\Pdf\";
            string defaultDB = Directory.GetCurrentDirectory() + @"\Data\";


            if (!Directory.Exists(defaultSave))
            {
                DirectoryInfo di = Directory.CreateDirectory(defaultSave);
                di.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
            }

            if (!Directory.Exists(defaultPDF))
            {
                DirectoryInfo di = Directory.CreateDirectory(defaultPDF);
                di.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
            }

            if (!Directory.Exists(defaultDB))
            {
                DirectoryInfo di = Directory.CreateDirectory(defaultDB);
                di.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
            }

            //flyt filer (save og pdf)

            //Flyt kunde dataInfo
            List<Class.XML_Files.Customer.Layout> allOldCustomer = funcCustomer.ReadCustomer(Models.ImportantData.g_FolderData);
            List<Class.XML_Files.Customer.Layout> allNewCustomer = funcCustomer.ReadCustomer(defaultDB);

            int oldCustomerCount = allOldCustomer.Count;

            for (int i = 0; i < oldCustomerCount; i++)
            {
                funcCustomer.SaveCustomer(allOldCustomer[i], defaultDB);
            }

            string error = "Kunne ikke flytte:\n";
            bool errorFound = false;

            string[] saveFileD_Names = Directory.GetFiles(defaultSave).Select(id => System.IO.Path.GetFileName(id)).ToArray();
            string[] saveFileNamesSrc = Directory.GetFiles(Models.ImportantData.g_FolderSave);
            int saveFilesCount = saveFileNamesSrc.Length;
            for (int i = 0; i < saveFilesCount; i++)
            {
                string fileName = System.IO.Path.GetFileName(saveFileNamesSrc[i]);

                if (!saveFileD_Names.Contains(fileName))
                {
                    File.Move(saveFileNamesSrc[i], defaultSave + fileName);
                }
                else
                {
                    error += saveFileNamesSrc[i] + "\n";
                    errorFound = true;
                }
            }

            string[] pdfFileD_Names = Directory.GetFiles(defaultPDF).Select(id => System.IO.Path.GetFileName(id)).ToArray();
            string[] pdfFileNamesSrc = Directory.GetFiles(Models.ImportantData.g_FolderPdf);
            int pdfFilesCount = pdfFileNamesSrc.Length;
            for (int i = 0; i < pdfFilesCount; i++)
            {
                string fileName = System.IO.Path.GetFileName(pdfFileNamesSrc[i]);
                if (!pdfFileD_Names.Contains(fileName))
                {
                    File.Move(pdfFileNamesSrc[i], defaultPDF + fileName);
                }
                else
                {
                    error += pdfFileNamesSrc[i] + "\n";
                    errorFound = true;
                }
            }

            if (errorFound)
            {
                MessageBox.Show(error);
            }

            //updatere mappe navne til de nye
            Models.ImportantData.g_FolderPdf = defaultPDF;
            Models.ImportantData.g_FolderSave = defaultSave;
            Models.ImportantData.g_FolderData = defaultDB;

            //Gem den nye sti
            DataSet settingsData = new DataSet();
            string settingsFileName = Models.ImportantData.g_FolderDB + "Settings.xml";

            FileStream settingsFileR = new FileStream(settingsFileName, FileMode.Open, FileAccess.Read);
            settingsData.ReadXml(settingsFileR);
            settingsFileR.Close();

            settingsData.Tables[0].Rows[0]["SavePath"] = "None";

            FileStream settingsFileW = new FileStream(settingsFileName, FileMode.Create, FileAccess.Write);
            settingsData.WriteXml(settingsFileW);
            settingsFileW.Close();

            ButtonAdmin_ChangeFolderDefault.Visibility = Visibility.Collapsed;
        }
        private void ButtonFilesUpdate_Click(object sender, RoutedEventArgs e)
        {
            this.fileInfoList = new FileInfoClass().GetFileList();

            loadStatusList();
            LoadChartPieFragtFakt_Status();
            UpdateFakturaPriceChart();
            LoadPieChartData();
        }
        #endregion

        private class FileInfoKeyClass
        {
            public int Type_Fragt { get { return 0; } }
            public int Type_Faktura { get { return 1; } }

            public int Status_Open { get { return 0; } }
            public int Status_Middle { get { return 1; } }
            public int Status_Closed { get { return 2; } }


        }

        private class FileInfoClass
        {
            private FileInfoKeyClass keyvalues = new FileInfoKeyClass();

            private string priName;
            private int priFileType;
            private int priStatus;
            private int priPostA;
            private int priPostM;
            private double priPrice;
            private int priMonthNo;
            private string priMonthName;
            private int priYear;
            private DateTime priCreateDate;
            private DateTime priCreateDateXML;
            private List<string> priCreator;
            private string priKontaktP_Afsend;
            private string priKontaktP_Modtag;
            private int priInvoice;

            public string Filename { get { return priName; } }
            public string KontaktPersonAfsend
            {
                get
                {
                    string varName = "None";
                    if (priFileType == keyvalues.Type_Fragt)
                    {
                        varName = priKontaktP_Afsend;
                    }
                    return varName;
                }
            }
            public string KontaktPersonModtag
            {
                get
                {
                    string varName = "None";
                    if (priFileType == keyvalues.Type_Fragt)
                    {
                        varName = priKontaktP_Modtag;
                    }
                    return varName;
                }
            }
            public int FileType { get { return priFileType; } }
            public int Status { get { return priStatus; } }
            public int PostAfsend { get { return priPostA; } }
            public int PostModtag { get { return priPostM; } }
            public double Price
            {
                get
                {
                    double varPrice = -1;
                    if (priFileType == keyvalues.Type_Faktura && priStatus == keyvalues.Status_Closed)
                    {
                        varPrice = priPrice;
                    }
                    return varPrice;
                }
            }
            public int CreateMonth { get { return priMonthNo; } }
            public string CreateMonthName { get { return priMonthName; } }
            public int CreateYear { get { return priYear; } }
            public DateTime CreateDatePDF { get { return priCreateDate; } }
            public DateTime CreateDateXML { get { return priCreateDateXML; } }
            public List<string> FileOwners { get { return priCreator; } }
            public int Invoice { get { return priInvoice; } }

            public List<FileInfoClass> GetFileList()
            {
                List<FileInfoClass> allFileList = new List<FileInfoClass>();

                string FolderPDF = Models.ImportantData.g_FolderPdf;
                string FolderXML = Models.ImportantData.g_FolderSave;

                //hent fil info
                List<string> fileNamesPdf = Directory.GetFiles(FolderPDF).Select(path => System.IO.Path.GetFileName(path)).ToList();
                List<string> fileNamesSave = Directory.GetFiles(FolderXML).Select(path => System.IO.Path.GetFileName(path)).ToList();
                List<DateTime> createDatePDF = Directory.GetFiles(FolderPDF).Select(path => File.GetLastWriteTime(path)).ToList();
                List<DateTime> createDateXML = Directory.GetFiles(FolderXML).Select(path => File.GetLastWriteTime(path)).ToList();

                #region Opret info for en fil

                int fileCount = fileNamesSave.Count();

                for (int i = 0; i < fileCount; i++)
                {
                    FileInfoClass newFileInfo = new FileInfoClass();

                    //Filnavn uden fil type (.pdf/.xml)
                    string filename = fileNamesSave[i].Substring(0, fileNamesSave[i].Length - 4);

                    //om der er lavet en pdf version af filen
                    bool pdfFound = false;
                    int pdfIndex = 0;
                    if (fileNamesPdf.Contains(filename + ".pdf"))
                    {
                        pdfIndex = fileNamesPdf.FindIndex(fileItem => fileItem == filename + ".pdf");
                        pdfFound = true;
                    }

                    //hvad type fil det er (fragtbrev/faktura)
                    int filetypeId = 0;
                    string[] FileTypeName = { "Fragtbrev-", "Faktura-" };
                    bool[] fileTypeBool = new bool[FileTypeName.Count()];
                    for (int a = 0; a < FileTypeName.Count(); a++)
                    {
                        if (filename.StartsWith(FileTypeName[a]))
                        {
                            filetypeId = a;
                            fileTypeBool[a] = true;
                        }
                        else
                        {
                            fileTypeBool[a] = false;
                        }
                    }

                    //hent hvem der har lavet eller ændret på den
                    FileStream readFileStream = new FileStream(FolderXML + fileNamesSave[i], FileMode.Open, FileAccess.Read);
                    DataSet readXML = new DataSet();
                    readXML.ReadXml(readFileStream);
                    readFileStream.Close();

                    List<string> creatorPerson = new List<string>();
                    int creatorPersonCount = readXML.Tables["Initialer"].Rows.Count;

                    for (int a = 0; a < creatorPersonCount; a++)
                    {
                        creatorPerson.Add(readXML.Tables["Initialer"].Rows[0].ItemArray[0].ToString());
                    }


                    //faktura hent prisen
                    //Fragtbrev ellers find ud af om fragtbrevet er lukket
                    double price = 0;
                    bool fragtDone = false;
                    bool faktDone = false;
                    string kontaktP_Afsend = "None";
                    string kontaktP_Modtag = "None";
                    int post_Afsend = -1;
                    int post_Modtag = -1;
                    int invoice = -1;
                    if (fileTypeBool[1])
                    {
                        price = double.Parse(readXML.Tables["FakturaInfo"].Rows[0].ItemArray[0].ToString());
                        try
                        {
                            faktDone = bool.Parse(readXML.Tables["FakturaInfo"].Rows[0].ItemArray[1].ToString());
                        }
                        catch (Exception)
                        {
                        }

                        invoice = int.Parse(filename.Replace("Faktura-", ""));
                        
                    }
                    else
                    {
                        int postA = -1;
                        int postM = -1;

                        fragtDone = bool.Parse(readXML.Tables["FileDone"].Rows[0].ItemArray[0].ToString());
                        kontaktP_Afsend = readXML.Tables["Afsender"].Rows[0].ItemArray[5].ToString();
                        
                        int.TryParse(readXML.Tables["Afsender"].Rows[0].ItemArray[3].ToString(), out postA);
                        post_Afsend = postA;
                        kontaktP_Modtag = readXML.Tables["Modtager"].Rows[0].ItemArray[5].ToString();
                        int.TryParse(readXML.Tables["Modtager"].Rows[0].ItemArray[3].ToString(),out postM);
                        post_Modtag = postM;


                        invoice = int.Parse(filename.Replace("Fragtbrev-", ""));
                    }

                    //Find ud af hvad status er for fillen
                    int status = 0;

                    if (pdfFound)
                    {
                        if ((fileTypeBool[0] && !fragtDone) || (fileTypeBool[1] && !faktDone))
                        {
                            status = 1;
                        }
                        else
                        {
                            status = 2;
                        }
                    }

                    //Find opret dato, måned og år
                    DateTime createdate = DateTime.MinValue;
                    DateTime createdateSave = createDateXML[i];
                    if (pdfFound)
                    {
                        createdate = createDatePDF[pdfIndex];
                    }
                    int monthNo = createdate.Month;
                    string monthName = createdate.ToString("MMMM");
                    int year = createdate.Year;

                    //Set data
                    newFileInfo.priName = filename;
                    newFileInfo.priFileType = filetypeId;
                    newFileInfo.priStatus = status;
                    newFileInfo.priKontaktP_Afsend = kontaktP_Afsend;
                    newFileInfo.priKontaktP_Modtag = kontaktP_Modtag;
                    newFileInfo.priPostA = post_Afsend;
                    newFileInfo.priPostM = post_Modtag;
                    newFileInfo.priPrice = price;
                    newFileInfo.priMonthNo = monthNo;
                    newFileInfo.priMonthName = monthName;
                    newFileInfo.priYear = year;
                    newFileInfo.priCreateDate = createdate;
                    newFileInfo.priCreateDateXML = createdateSave;
                    newFileInfo.priCreator = creatorPerson;
                    newFileInfo.priInvoice = invoice;

                    allFileList.Add(newFileInfo);
                }
                #endregion

                //Send liste tilbage
                return allFileList;
            }

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (this.closeOnX)
            {
                MainWindow startmenu = new MainWindow();
                startmenu.Show();
            }
        }
    }
}