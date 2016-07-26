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

            public void EditFaktura(object windowD)
            {
                FileName openfile = new FileName(4);
                openfile.Title = "Åben Faktura";

                openfile.ShowDialog();

                if (openfile.DialogResult.HasValue && openfile.DialogResult.Value)
                {
                    //gem filnavn og gør så man henter den
                    Models.ImportantData.Filename = openfile.TextBoxFileName_name.Content.ToString();
                    Models.ImportantData.LoadFormFile = true;

                    //vis Faktura og luk start menu
                    WindowsView.UserSetup addIni = new WindowsView.UserSetup();
                    addIni.ShowDialog();

                    if (addIni.DialogResult.HasValue && addIni.DialogResult.Value)
                    {
                        MainFaktura fakturaWindow = new MainFaktura(addIni.initialerCont);
                        fakturaWindow.Show();
                    }

                    (windowD as Window).Close();
                }
            }

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
            private void dgLoadId(object sender)
            {
                for (int i = 0; i <= (sender as DataGrid).Items.Count; i++)
                {
                    //MessageBox.Show("row = " + i + " column = " + 0 + " Text =" + (i + 1));
                    SetCellValueTextBlock((sender as DataGrid), i, 0, (i + 1).ToString());
                }
            }
            void DataGridTransport_Pakker_Loaded(object sender, RoutedEventArgs e)
            {
                dgLoadId(sender);
            }
            void datagridView_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
            {
                dgLoadId(sender);
            }

            /// <summary>
            /// Hvad der skal står i pakke headers
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

                //dgTextBoxID.CellStyle.Setters(FrameworkElement.HorizontalAlignment, HorizontalAlignment.Center);
                datagridView.Columns.Add(dgTextBoxID);
            

                //combobox next array count up
                int itemsourceSelector = 0;

                for (int i = 0; i < headerNames.Count(); i++)
                {
                    //combobox
                    if (!textBoxBool[i])
                    {
                        if (ReadOnly != null)
                        {
                            datagridView.Columns.Add(
                                DGComboLookNormal(
                                    headerNames[i],
                                    bindingsName[i],
                                    widthSize[i],
                                    itemsSourceArray[itemsourceSelector].ToList(),
                                    ShowCombox[itemsourceSelector],
                                    comboboxSelectChange[itemsourceSelector],
                                    ReadOnly[i]
                                )
                            );
                        }
                        else
                        {
                            datagridView.Columns.Add(
                                   DGComboLookNormal(
                                       headerNames[i],
                                       bindingsName[i],
                                       widthSize[i],
                                       itemsSourceArray[itemsourceSelector].ToList(),
                                       ShowCombox[itemsourceSelector],
                                       comboboxSelectChange[itemsourceSelector]
                                   )
                               );
                        }

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

                // Create The Column
                if (buttonClick != null)
                {
                DataGridTemplateColumn buttonColumn = new DataGridTemplateColumn();

                FrameworkElementFactory buttonFactory = new FrameworkElementFactory(typeof(Button));
                buttonFactory.SetValue(Button.ContentProperty, "Fjern");
                buttonFactory.AddHandler(Button.ClickEvent, buttonClick);


                DataTemplate buttonTemplate = new DataTemplate();
                buttonTemplate.VisualTree = buttonFactory;
                // Set the Templates to the Column
                buttonColumn.CellTemplate = buttonTemplate;
                buttonColumn.CellEditingTemplate = buttonTemplate;
                buttonColumn.Width = 50;

                datagridView.Columns.Add(buttonColumn);
                    }
            }

            public string GetDataGridCell(DataGrid datagrid, int row, int column)
            {

                datagrid.SelectAllCells();
                datagrid.ClipboardCopyMode = DataGridClipboardCopyMode.IncludeHeader;
                ApplicationCommands.Copy.Execute(null, datagrid);

                datagrid.UnselectAllCells();
                String result = (string)Clipboard.GetData(DataFormats.CommaSeparatedValue);

                string[] Lines = result.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
                string[] Fields;
                Fields = Lines[0].Split(new char[] { ',' });
                int Cols = Fields.GetLength(0);

                DataTable dtable = new DataTable();
                for (int i = 0; i < Cols; i++)
                    dtable.Columns.Add(Fields[i].ToUpper(), typeof(string));
                DataRow Row;
                for (int i = 1; i < Lines.GetLength(0) - 1; i++)
                {
                    Fields = Lines[i].Split(new char[] { ',' });
                    Row = dtable.NewRow();
                    for (int f = 0; f < Cols; f++)
                    {
                        Row[f] = Fields[f];
                    }
                    dtable.Rows.Add(Row);

                }
                object[] columnsArray = dtable.Rows[row].ItemArray;

                return columnsArray[column].ToString();

            }

            public string GetDgComboboxData(DataGrid datagrid, int rows, int columns, bool idOrValue)
            {
                int itemsCount = datagrid.Items.Count;

                if (rows >= 0 && rows < itemsCount)
                {
                    object item = datagrid.Items[rows];

                    datagrid.ScrollIntoView(item);

                    FrameworkElement getcontentfr = datagrid.Columns[columns].GetCellContent(item);
                    ComboBox myComboBox = FindVisualChild<ComboBox>(getcontentfr);
                    int comboboxId = comboboxId = myComboBox.SelectedIndex;


                    if (!idOrValue && comboboxId != -1)
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

            public void SetCellValueTextBlock(DataGrid datagrid, int row, int column, string text)
            {
                try
                {
                    object item = datagrid.Items[row];
                    (datagrid.Columns[column].GetCellContent(item) as TextBlock).Text = text;
                }
                catch (Exception e)
                {
                    //MessageBox.Show(e.Message);
                }
            }

            public string GetCellValueComboBox(DataGrid datagrid, int row, int column, bool textFormat = false)
            {
                object item = datagrid.Items[row];

                int comboxId = (datagrid.Columns[column].GetCellContent(item) as ComboBox).SelectedIndex;

                if (textFormat)
                {
                    string comboboxText = "";

                    //tjekker om der er valgt en type
                    if (comboxId != -1)
                    {
                        //Substring(36) gør så man ikke får af vide hvad type combobox det er
                        comboboxText = (datagrid.Columns[column].GetCellContent(item) as ComboBox).Items[comboxId].ToString(); // .Substring(37);
                    }

                    return comboboxText;
                }

                return comboxId.ToString();
            }

            /// <summary>
            /// Gør så man kan se at det er en combobox
            /// </summary>
            /// <param name="headerName">Header navn</param>
            /// <param name="binding">Variable</param>
            /// <param name="WidthSize">Størrelese</param>
            /// <param name="itemsource">Indhold</param>
            /// <returns>DataGridTemplateColumn</returns>
            private DataGridTemplateColumn DGComboLookNormal(string headerName, string binding, double WidthSize, List<string> itemsource, bool dgVisibility = true, SelectionChangedEventHandler selectTrigger = null, bool ReadOnly = false)
            {
                // Create The Column
                DataGridTemplateColumn ComboColumn = new DataGridTemplateColumn();
                ComboColumn.Header = headerName;

                // Create the ComboBox
                System.Windows.Data.Binding comboBind = new System.Windows.Data.Binding(binding);
                comboBind.Mode = System.Windows.Data.BindingMode.OneWay;

                FrameworkElementFactory comboFactory = new FrameworkElementFactory(typeof(ComboBox));
                comboFactory.SetValue(ComboBox.IsTextSearchEnabledProperty, true);
                comboFactory.SetValue(ComboBox.ItemsSourceProperty, itemsource);
                comboFactory.SetBinding(ComboBox.SelectedItemProperty, comboBind);

                if (ReadOnly)
                {
                    comboFactory.SetValue(ComboBox.IsEnabledProperty, !ReadOnly);
                    comboFactory.SetValue(ComboBox.ForegroundProperty, Brushes.Gray);
                }

                if (selectTrigger != null)
                {
                    comboFactory.AddHandler(ComboBox.SelectionChangedEvent, selectTrigger);
                }


                DataTemplate comboTemplate = new DataTemplate();
                comboTemplate.VisualTree = comboFactory;
                // Set the Templates to the Column
                ComboColumn.CellTemplate = comboTemplate;
                ComboColumn.CellEditingTemplate = comboTemplate;
                ComboColumn.Width = WidthSize;

                if (!dgVisibility)
                {
                    ComboColumn.Visibility = Visibility.Hidden;
                }

                return ComboColumn;
            }

            public void GetCellValue()
            {
                /*
                 object item = dgObject.Items[0];

                Type itemType = dgObject.Columns[1].GetCellContent(item).GetType();

                string comboxId = dgObject.Columns[1].GetCellContent(item).GetType().ToString();

                if (itemType == typeof(TextBlock))
                {
                    MessageBox.Show("1");
                }
                else
                {
                    MessageBox.Show("2");
                }

                MessageBox.Show(comboxId);



                int dgRowsCount = dgObject.Items.Count;
                int dgColumnCount = dgObject.Columns.Count;
                for (int i = 0; i < dgRowsCount; i++)
                {
                    for (int a = 0; a < dgColumnCount; a++)
                    {
                        if (itemType == typeof(TextBlock))
                        {
                            MessageBox.Show("1");
                        }
                        else
                        {
                            MessageBox.Show("2");
                        }
                    }
                }
                
                //MessageBox.Show("klik");
                int dataGridRowsCount = DataGridGodslinjer.Items.Count;
                var itemsView = DataGridGodslinjer.ItemsSource;
                string[] dgCellTextValues = null;
                string[] dgCellComboValues = null;
                for (int i = 0; i < dataGridRowsCount - 1; i++)
                {
                    bool isNewRow = true;

                    switch (ComboBoxGodslinjer_TransportType.SelectedIndex)
                    {
                        case 0: //Kurrer
                            dgCellTextValues = new string[] { 
                            funcDG.GetCellValueTextBlock(DataGridGodslinjer, i, 1),
                            funcDG.GetCellValueTextBlock(DataGridGodslinjer, i, 2),
                            funcDG.GetCellValueTextBlock(DataGridGodslinjer, i, 4),
                            funcDG.GetCellValueTextBlock(DataGridGodslinjer, i, 5),
                            funcDG.GetCellValueTextBlock(DataGridGodslinjer, i, 6)
                        };

                            dgCellComboValues = new string[] { 
                            funcDG.GetDgComboboxData(DataGridGodslinjer, i, 3, false)
                        };
                            break;
                        case 1: //Pakke
                            dgCellTextValues = new string[] { 
                            funcDG.GetCellValueTextBlock(DataGridGodslinjer, i, 1),
                            funcDG.GetCellValueTextBlock(DataGridGodslinjer, i, 2),
                            funcDG.GetCellValueTextBlock(DataGridGodslinjer, i, 4),
                            funcDG.GetCellValueTextBlock(DataGridGodslinjer, i, 6),
                            funcDG.GetCellValueTextBlock(DataGridGodslinjer, i, 7)
                        };

                            dgCellComboValues = new string[] { 
                            funcDG.GetDgComboboxData(DataGridGodslinjer, i, 3, false),
                            funcDG.GetDgComboboxData(DataGridGodslinjer, i, 5, false)
                        };
                            break;
                        case 2: //Gods
                            dgCellTextValues = new string[] { 
                            funcDG.GetCellValueTextBlock(DataGridGodslinjer, i, 1),
                            funcDG.GetCellValueTextBlock(DataGridGodslinjer, i, 2),
                            funcDG.GetCellValueTextBlock(DataGridGodslinjer, i, 4),
                            funcDG.GetCellValueTextBlock(DataGridGodslinjer, i, 6),
                            funcDG.GetCellValueTextBlock(DataGridGodslinjer, i, 7)
                        };

                            dgCellComboValues = new string[] { 
                            funcDG.GetDgComboboxData(DataGridGodslinjer, i, 3, false),
                            funcDG.GetDgComboboxData(DataGridGodslinjer, i, 5, false)
                        };
                            break;
                    }

                    for (int a = 0; a < dgCellTextValues.Count(); a++)
                    {
                        if (dgCellTextValues[a] != "" && dgCellTextValues[a] != "0")
                        {
                            isNewRow = false;
                            break;
                        }
                    }
                    for (int a = 0; a < dgCellComboValues.Count(); a++)
                    {
                        if (!isNewRow || (dgCellComboValues[a] != "" && dgCellComboValues[a] != "-1"))
                        {
                            isNewRow = false;
                            break;
                        }
                    }

                    if (isNewRow)
                    {
                        (itemsView as System.ComponentModel.IEditableCollectionView).RemoveAt(i);
                        i--;
                        dataGridRowsCount--;
                    }

                    //update Itemsource for at være sikker på at Class.Functions.GetDgComboboxData virker
                    DataGridGodslinjer.ItemsSource = CollectionViewSource.GetDefaultView(itemsView);*/
                
            
                 
                
            }

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
            
            public void SaveXmlToDatabase(string Filnavn, DataSet Information)
            {

                string folder = Models.ImportantData.g_FolderDB;

                FileStream documentWrite = new FileStream(folder + Filnavn + ".xml", FileMode.Create, FileAccess.Write);

                Information.WriteXml(documentWrite);
                documentWrite.Close();
            }

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

            //læs database med kunde informationer
            public DataSet ReadXmlFormDatabase(string Filnavn)
            {

                string folder = Models.ImportantData.g_FolderDB;

                FileStream documentRead = new FileStream(folder + Filnavn + ".xml", FileMode.Open, FileAccess.Read);

                DataSet ReadXML = new DataSet();

                ReadXML.ReadXml(documentRead);
                documentRead.Close();

                return ReadXML;
            }

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
            /// Udskriver om det er et tal eller ej
            /// </summary>
            /// <param name="number">Tal i String format</param>
            /// <returns></returns>
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

            public bool isANumber(string number, out double isNumber)
            {
                if (double.TryParse(number, out isNumber))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            public double GetPriceFromFile(string fileName)
            {
                string pathXML = Models.ImportantData.g_FolderSave + fileName + ".xml";
                string pathPDF = Models.ImportantData.g_FolderPdf  + fileName + ".pdf";

                double price = 0;

                if (File.Exists(pathXML) && File.Exists(pathPDF))
                {
                    FileStream fileXmlStream = new FileStream(pathXML, FileMode.Open);
                    DataSet xmlRead = new DataSet();
                    xmlRead.ReadXml(fileXmlStream);
                    fileXmlStream.Close();

                    double tryPrice = 0;
                    double.TryParse(xmlRead.Tables["FakturaInfo"].Rows[0].ItemArray[0].ToString(), out tryPrice);

                    price = tryPrice;
                }
                return price;
            }

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
