using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Controls.Primitives;


namespace TecCargo_Faktura.Class
{
    class Functions
    {
        public class Windows
        {
            /// <summary>
            /// åbener et nyt fragtbrev
            /// og lukker nuværende vindue
            /// </summary>
            public void NewFragtbrev(object windowD, bool closeFragtbrev = false)
            {
                //så det bliver en nyt fragt brev
                Models.ImportantData.Filename = "";
                Models.ImportantData.LoadFormFile = false;

                WindowsView.UserSetup addIni = new WindowsView.UserSetup();
                addIni.ShowDialog();

                if (addIni.DialogResult.HasValue && addIni.DialogResult.Value)
                {
                    //vis fragtbrev og luk start menu
                    MainFragtBrev FragtbrevWindow = new MainFragtBrev(addIni.initialerCont, closeFragtbrev);
                    FragtbrevWindow.Show();

                    (windowD as Window).Close();
                }
            }

            /// <summary>
            /// åbener en ny faktura
            /// og lukker nuværende vindue
            /// </summary>
            public void NewFaktura(object windowD)
            {
                FileName openfile = new FileName(3);
                openfile.Title = "Opret Faktura";

                openfile.ShowDialog();

                if (openfile.DialogResult.HasValue && openfile.DialogResult.Value)
                {
                    int FilenameLent = openfile.TextBoxFileName_name.Content.ToString().Length;
                    string OnlyNumberFragt = openfile.TextBoxFileName_name.Content.ToString().Substring(10, FilenameLent - 10);

                    //gem filnavn og gør så man henter den
                    Models.ImportantData.LoadFormFile = false;
                    Models.ImportantData.fragtBrevNumber = OnlyNumberFragt;

                    if (File.Exists(Models.ImportantData.g_FolderSave + "Faktura-" + OnlyNumberFragt + ".xml"))
                    {
                        MessageBoxResult createNew = MessageBox.Show("Du er ved at oprette en ny faktura der findes i forvejen.\n Vil du slette den gamle?", "Ny Faktura", MessageBoxButton.YesNo, MessageBoxImage.Information, MessageBoxResult.Yes);
                        if (createNew == MessageBoxResult.Yes)
                        {
                            File.Delete(Models.ImportantData.g_FolderSave + "Faktura-" + OnlyNumberFragt + ".xml");
                        }
                        else
                        {
                            return;
                        }
                    }

                    //vis Faktura og luk start menu
                    WindowsView.UserSetup addIni = new WindowsView.UserSetup();
                    addIni.ShowDialog();

                    if (addIni.DialogResult.HasValue && addIni.DialogResult.Value)
                    {
                        MainFaktura fakturaWindow = new MainFaktura(addIni.initialerCont, openfile.TextBoxFileName_name.Content.ToString());
                        
                        try
                        {
                            fakturaWindow.Show();
                        }
                        catch (Exception e)
                        {
                            MessageBox.Show("Ny Faktura Kunne ikke åbnes: " + e.Message);
                        }

                        (windowD as Window).Close();
                    }
                }
            }

            /// <summary>
            /// åbener fil list hvor man 
            /// kan vælge den fil man vil ændre i
            /// som så vil blive åbnet
            /// </summary>
            public void EditFile(object windowD, string titleD, int OpenType, bool isFragtbrev = true)
            {
                XML_Files.Fragtbrev funcFragt = new XML_Files.Fragtbrev();
                FileName openfile = new FileName(OpenType);
                openfile.Title = titleD;

                openfile.ShowDialog();

                if (openfile.DialogResult.HasValue && openfile.DialogResult.Value)
                {
                    //gem filnavn og gør så man henter den
                    Models.ImportantData.Filename = openfile.TextBoxFileName_name.Content.ToString();
                    Models.ImportantData.LoadFormFile = true;
                    if (isFragtbrev)
                    {
                        //hvis fragtbrevet er lukket skal man ikke kunne åben den
                        funcFragt.ReadFile(Models.ImportantData.Filename);
                        if (Models.ImportantData.closeFragtbrevBool)
                        {
                            MessageBox.Show("Kan ikke rediger fragtbrev, når det er afsluttet.", "Fragtbrev Afsluttet", MessageBoxButton.OK, MessageBoxImage.Stop, MessageBoxResult.OK);
                            return;
                        }
                    }
                    WindowsView.UserSetup addIni = new WindowsView.UserSetup();
                    addIni.ShowDialog();
                    if (addIni.DialogResult.HasValue && addIni.DialogResult.Value)
                    {
                        if (isFragtbrev)
                        {
                            //vis fragtbrev og luk start menu
                            MainFragtBrev FragtbrevWindow = new MainFragtBrev(addIni.initialerCont);
                            try
                            {
                                FragtbrevWindow.Show();
                            }
                            catch (Exception)
                            {
                            }
                        }
                        else
                        {
                            MainFaktura fakturaWindow = new MainFaktura(addIni.initialerCont);

                            try
                            {
                                fakturaWindow.Show();
                            }
                            catch (Exception)
                            {
                            }
                        }

                        (windowD as Window).Close();
                    }
                }
            }

            /// <summary>
            /// åbner et fragtbrev hvor man 
            /// kan afslutte fragtbrevet
            /// </summary>
            public void CloseFragtbrev(object windowD)
            {
                FileName openfile = new FileName(2);
                openfile.Title = "Afslut Fragtbrev";

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
                        MainFragtBrev FragtbrevWindow = new MainFragtBrev(addIni.initialerCont,true);

                        try
                        {
                            FragtbrevWindow.Show();
                        }
                        catch (Exception)
                        {
                        }
                        (windowD as Window).Close();
                    }
                    
                }
            }

            /// <summary>
            /// åbner en liste med pdf`er hvor
            /// hvis man vælger en vil den åben den
            /// </summary>
            public void OpenPDF(object windowD, string titleD, int OpenType)
            {
                FileName openfil = new FileName(OpenType);
                openfil.Title = titleD;

                openfil.ShowDialog();

                if (openfil.DialogResult.HasValue && openfil.DialogResult.Value)
                {

                    try
                    {
                        System.Diagnostics.Process.Start(Models.ImportantData.g_FolderPdf + openfil.TextBoxFileName_name.Content + ".pdf");
                    }
                    catch (Exception error)
                    {
                        MessageBox.Show("ERROR:\n" + error.Message);
                    }
                }
            }
        }

        public class DatagridFunc
        { 
            /// <summary>
            /// giver hver række et nummer
            /// </summary>
            private void dgLoadId(object sender)
            {
                for (int i = 0; i <= (sender as DataGrid).Items.Count; i++)
                {
                    //MessageBox.Show("row = " + i + " column = " + 0 + " Text =" + (i + 1));
                    SetCellValueTextBlock((sender as DataGrid), i, 0, (i + 1).ToString());
                }
            }

            /// <summary>
            /// når den datagrid bliver loaded opdaterer række id
            /// </summary>
            void DataGridTransport_Pakker_Loaded(object sender, RoutedEventArgs e)
            {
                dgLoadId(sender);
            }

            /// <summary>
            /// når den datagrid bliver ændret opdaterer række id
            /// </summary>
            void datagridView_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
            {
                dgLoadId(sender);
            }

            /// <summary>
            /// ændre en datagrid til nye kolonner
            /// </summary>
            /// <param name="transportType">
            /// 0 = skjult DataGrid
            /// 1 = Kurrer;
            /// 2 = Pakke;
            /// 3 = Gods;
            /// </param>
            public void DataGridPakkerLoadType(DataGrid datagridView, string[] headerNames, string[] bindingsName, double[] widthSize, string[][] itemsSourceArray, bool[] textBoxBool, bool[] ShowCombox, SelectionChangedEventHandler[] comboboxSelectChange, bool[] ReadOnly = null, RoutedEventHandler buttonClick = null)
            {
                //nulstil
                datagridView.Columns.Clear();
                datagridView.Loaded += DataGridTransport_Pakker_Loaded;
                datagridView.RowEditEnding += datagridView_RowEditEnding;

                //Lav Id header
                DataGridTextColumn dgTextBoxID = new DataGridTextColumn();

                System.Windows.Data.Binding bindingValueId = new System.Windows.Data.Binding("idNumber");

                dgTextBoxID.Header = "ID";
                dgTextBoxID.Binding = bindingValueId;
                dgTextBoxID.MinWidth = 20;
                dgTextBoxID.IsReadOnly = true;
                dgTextBoxID.Foreground = Brushes.Gray;
                dgTextBoxID.FontWeight = FontWeights.Bold;

                //tilføj element til datagrid
                datagridView.Columns.Add(dgTextBoxID);


                int itemsourceSelector = 0; //tæller til næste combobox

                for (int i = 0; i < headerNames.Count(); i++)
                {
                    //tjek om det er en combobox
                    if (!textBoxBool[i])
                    {
                        bool isReadOnly = false;
                        if (ReadOnly != null) isReadOnly = ReadOnly[i];

                        datagridView.Columns.Add(
                            DGComboLookNormal(
                                headerNames[i],
                                bindingsName[i],
                                widthSize[i],
                                itemsSourceArray[itemsourceSelector].ToList(),
                                ShowCombox[itemsourceSelector],
                                comboboxSelectChange[itemsourceSelector],
                                isReadOnly
                            )
                        );   

                        itemsourceSelector++;
                    }
                    //TextBox
                    else
                    {
                        DataGridTextColumn dgTextBox = new DataGridTextColumn();

                        System.Windows.Data.Binding bindingValue = new System.Windows.Data.Binding(bindingsName[i]);
                        bindingValue.ConverterCulture = new System.Globalization.CultureInfo("da-DK");

                        dgTextBox.Header = headerNames[i];
                        dgTextBox.Binding = bindingValue;
                        dgTextBox.Width = widthSize[i];

                        //tjek om det skal være read only og
                        //skift tekst farve hvis er
                        if (ReadOnly != null)
                        {
                        
                            if (ReadOnly[i])
                            {
                                dgTextBox.IsReadOnly = ReadOnly[i];
                                dgTextBox.Foreground = Brushes.Gray;
                            }
                        }

                        datagridView.Columns.Add(dgTextBox);
                    }
                }

                // lav EN fjern knap i datagrid
                if (buttonClick != null)
                {
                    DataGridTemplateColumn buttonColumn = new DataGridTemplateColumn();

                    FrameworkElementFactory buttonFactory = new FrameworkElementFactory(typeof(Button));
                    buttonFactory.SetValue(Button.ContentProperty, "Fjern");
                    buttonFactory.AddHandler(Button.ClickEvent, buttonClick);


                    DataTemplate buttonTemplate = new DataTemplate();
                    buttonTemplate.VisualTree = buttonFactory;

                    // sæt template til kolonne
                    buttonColumn.CellTemplate = buttonTemplate;
                    buttonColumn.CellEditingTemplate = buttonTemplate;
                    buttonColumn.Width = 50;

                    datagridView.Columns.Add(buttonColumn);
                }
            }

            /// <summary>
            /// hent en combobox i datagrid værdi
            /// </summary>
            public string GetDgComboboxData(DataGrid datagrid, int rows, int columns, bool returnId)
            {
                int itemsCount = datagrid.Items.Count;

                //vær sikker på at rækken findes
                if (rows >= 0 && rows < itemsCount)
                {
                    object item = datagrid.Items[rows];

                    datagrid.ScrollIntoView(item);

                    FrameworkElement getcontentfr = datagrid.Columns[columns].GetCellContent(item);
                    ComboBox myComboBox = FindVisualChild<ComboBox>(getcontentfr);
                    int comboboxId = comboboxId = myComboBox.SelectedIndex;


                    if (!returnId && comboboxId != -1)
                    {
                        return myComboBox.Text;
                    }
                    else
                    {
                        return myComboBox.SelectedIndex.ToString();
                    }
                }
                else
                {
                    return "0";
                }
            }

            /// <summary>
            /// giv en combobox i datagrid en værdi
            /// </summary>
            public void SetDgComboboxData(DataGrid datagrid, int rows, int columns, string text)
            {
                int itemsCount = datagrid.Items.Count;

                if (rows >= 0 && rows < itemsCount)
                {
                    object item = datagrid.Items[rows];

                    datagrid.ScrollIntoView(item);

                    FrameworkElement getcontentfr = datagrid.Columns[columns].GetCellContent(item);
                    ComboBox myComboBox = FindVisualChild<ComboBox>(getcontentfr);
                    myComboBox.SelectedIndex = 2;
                }
            }
            
            /// <summary>
            /// finder et element i en datagrid/listview
            /// 
            /// denne function er fundet på nettet
            /// </summary>
            private T FindVisualChild<T>(DependencyObject depObj) where T : DependencyObject
            {
                if (depObj != null)
                {
                    for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                    {
                        DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                        if (child != null && child is T)
                        {
                            return (T)child;
                        }

                        T childItem = FindVisualChild<T>(child);
                        if (childItem != null) return childItem;
                    }
                }
                return null;
            }


            /// <summary>
            /// henter det tekst der 
            /// står en en datagrid textbox
            /// </summary>
            public string GetCellValueTextBlock(DataGrid datagrid, int row, int column)
            {
                string ValueTextBlock = "";

                try
                {
                    object item = datagrid.Items[row];
                    ValueTextBlock = (datagrid.Columns[column].GetCellContent(item) as TextBlock).Text;
                }
                catch (Exception)
                {
                }

                return ValueTextBlock;
            }

            /// <summary>
            /// giver en tekst til en textbox 
            /// i en datagrid
            /// </summary>
            public void SetCellValueTextBlock(DataGrid datagrid, int row, int column, string text)
            {
                try
                {
                    object item = datagrid.Items[row];
                    (datagrid.Columns[column].GetCellContent(item) as TextBlock).Text = text;
                }
                catch (Exception){}
            }

            /// <summary>
            /// Gør så man kan se at det er en 
            /// combobox der er i en datagrid
            /// </summary>
            private DataGridTemplateColumn DGComboLookNormal(string headerName, string binding, double WidthSize, List<string> itemsource, bool dgVisibility = true, SelectionChangedEventHandler selectTrigger = null, bool ReadOnly = false)
            {
                //opret kolonner
                DataGridTemplateColumn ComboColumn = new DataGridTemplateColumn();
                ComboColumn.Header = headerName;

                System.Windows.Data.Binding comboBind = new System.Windows.Data.Binding(binding);
                comboBind.Mode = System.Windows.Data.BindingMode.OneWay;

                //opret combobox
                FrameworkElementFactory comboFactory = new FrameworkElementFactory(typeof(ComboBox));
                comboFactory.SetValue(ComboBox.IsTextSearchEnabledProperty, true);
                comboFactory.SetValue(ComboBox.ItemsSourceProperty, itemsource);
                comboFactory.SetBinding(ComboBox.SelectedItemProperty, comboBind);

                //hvis der skal være readonly
                //skift tekst farve
                if (ReadOnly)
                {
                    comboFactory.SetValue(ComboBox.IsEnabledProperty, !ReadOnly);
                    comboFactory.SetValue(ComboBox.ForegroundProperty, Brushes.Gray);
                }

                //om der skal ske noget når
                //man skifter selection
                if (selectTrigger != null)
                {
                    comboFactory.AddHandler(ComboBox.SelectionChangedEvent, selectTrigger);
                }

                //gør så man kan sætte comboboxen ind i datagrid
                DataTemplate comboTemplate = new DataTemplate();
                comboTemplate.VisualTree = comboFactory;

                ComboColumn.CellTemplate = comboTemplate;
                ComboColumn.CellEditingTemplate = comboTemplate;
                ComboColumn.Width = WidthSize;

                if (!dgVisibility)
                {
                    ComboColumn.Visibility = Visibility.Hidden;
                }

                return ComboColumn;
            }

            /// <summary>
            /// fjerner valgte række i datagrid
            /// hvor der også er mulighed for opdatere 
            /// datagrid id hvis den bruger det
            /// </summary>
            public void RemoveItemDG(DataGrid dgObject, int removeId, bool updateNumber = true)
            {
                int maxRows = dgObject.Items.Count;

                if (removeId < maxRows && removeId >= 0)
                {
                    var itemsView = dgObject.ItemsSource;

                    (itemsView as System.ComponentModel.IEditableCollectionView).RemoveAt(removeId);

                    if (updateNumber)
                    {
                        dgLoadId(dgObject);
                    }
                }
            }

            /// <summary>
            /// fjerner en værdi i en itemssource
            /// </summary>
            public List<string> removeIteamFromList(ItemCollection itemsList, int removeId)
            {
                List<string> itemsSourceV = new List<string>();
                for (int i = 0; i < itemsList.Count; i++)
                {
                    if (i != removeId)
                    {
                        itemsSourceV.Add(itemsList[i].ToString());
                    }
                }
                itemsSourceV.Sort();
                itemsSourceV.Reverse();
                return itemsSourceV;
            }
        }

        public class others
        {
            /// <summary>
            /// Laver en farve fra hex til en brushes
            /// </summary>
            /// <param name="hexColor">#FFFFFFFF</param>
            /// <returns>Brushes</returns>
            public Brush ColorBrushHex(string hexColor)
            {
                var converter = new System.Windows.Media.BrushConverter();
                var brush = (Brush)converter.ConvertFromString(hexColor);

                return brush;
            }


            /// <summary>
            /// læser xml filer og sender tilbage som en dataset
            /// </summary>
            public DataSet ReadXmlFormDatabase(string Filnavn)
            {
                string folder = Models.ImportantData.g_FolderDB;

                FileStream documentRead = new FileStream(folder + Filnavn + ".xml", FileMode.Open, FileAccess.Read);

                DataSet ReadXML = new DataSet();

                ReadXML.ReadXml(documentRead);
                documentRead.Close();

                return ReadXML;
            }

            /// <summary>
            /// tjek om filen man prøver at 
            /// åben bliver brugt af et
            /// andet program
            /// </summary>
            public bool IsFileLocked(FileInfo file)
            {
                FileStream stream = null;

                try
                {
                    stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
                }
                catch (IOException)
                {
                    //the file is unavailable because it is:
                    //still being written to
                    //or being processed by another thread
                    //or does not exist (has already been processed)
                    return true;
                }
                finally
                {
                    if (stream != null)
                        stream.Close();
                }

                //file is not locked
                return false;
            }

            /// <summary>
            /// tjekker om en string 
            /// er et tal eller ej
            /// </summary>
            public bool isANumber(string number)
            {
                double isNumber = 0;

                if (double.TryParse(number, out isNumber))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            /// <summary>
            /// tjekker om en string er et tal 
            /// og hvis den er giver talet tilbage
            /// </summary>
            public bool isANumber(string number, out double outNumber)
            {
                if (double.TryParse(number, out outNumber))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            /// <summary>
            /// henter hvad det har kostet 
            /// at lave/køre med denne faktura
            /// </summary>
            public double GetPriceFromFile(string fileName)
            {
                double price = 0;

                //tjek om fakturaen er afsluttet
                string pathXML = Models.ImportantData.g_FolderSave + fileName + ".xml";
                string pathPDF = Models.ImportantData.g_FolderPdf  + fileName + ".pdf";

                if (File.Exists(pathXML) && File.Exists(pathPDF))
                {
                    //henter xml filen og gemmer den som dataset
                    FileStream fileXmlStream = new FileStream(pathXML, FileMode.Open);
                    DataSet xmlRead = new DataSet();
                    xmlRead.ReadXml(fileXmlStream);
                    fileXmlStream.Close();

                    //prøv at hente prisen fra xml filen
                    double tryPrice = 0;
                    double.TryParse(xmlRead.Tables["FakturaInfo"].Rows[0].ItemArray[0].ToString(), out tryPrice);

                    price = tryPrice;
                }
                return price;
            }

            /// <summary>
            /// henter by navn udfra post nummeret
            /// </summary>
            public string GetCityFormZip(string zipcode)
            {
                if (zipcode.Length == 4)
                {
                    DataSet postNumbDataSet = this.ReadXmlFormDatabase("postnummerfil");

                    int postNumbCount = postNumbDataSet.Tables[2].Rows.Count;

                    //tjek om postnummere er ens og hent navnet på byen
                    for (int i = 0; i < postNumbCount; i++)
                    {
                        string dataPost = postNumbDataSet.Tables[2].Rows[i].ItemArray[0].ToString();

                        if (zipcode == dataPost)
                        {
                            return postNumbDataSet.Tables[2].Rows[i].ItemArray[1].ToString();
                        }
                    }
                }
                return "";
            }
        }
    }
}
