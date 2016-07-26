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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.IO;
using System.Diagnostics;

namespace TecCargo_Dagbog
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region global variabler

        //links/Dokument holder for nuværende elev
        private List<List<Model.FileClass.Links>> FilesList = new List<List<Model.FileClass.Links>>();
        
        private DispatcherTimer _timer;
        int freeTextCount = 0;  //Antal teskt bokse
        bool saved = true; //Save status
        bool isLoading = true;

        //Om det er en ny eller gamle elev
        //Bliver sat inden vindue bliver åbenet
        public bool LoadFromFile = false;

        #endregion  global variabler

        public MainWindow()
        {
            InitializeComponent();

            
            LabelVersion_version.Content += Inc.Settings._version.ToString(); //Sæt program Version
            LabelVersion_author.Content += Inc.Settings.CreateBy; //Sæt programudvikler navne

            AddFreeText(); //tilføj et teskt felt

            SetSaveTimer(5); //sæt til at gemme automatisk hver 5 minut
        }

        #region Events

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //Hent dagbog data når vinduet er loaded
            //ellers kan der ske fjel når der tilføjes nye tekst bokse
            if (this.LoadFromFile)
            {
                LoadFile();
            }

            //Gør så når man ændre på noget skal dagbogen vise som ikke gemt
            SetChangeFunction();

            this.isLoading = false;
        }

        //spørg om man vil gemme dagbogen vis man lukket vinduet ned
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!this.saved)
            {
                MessageBoxResult result = MessageBox.Show("Gem Dagbog?", "Gem", MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                {
                    SaveFile();
                }
            }
        }

        /// <summary>
        /// åbener et vindue hvor man har muligt hed 
        /// for at tilføje et link eller dokument
        /// </summary>
        private void AddFilesButton_Click(object sender, RoutedEventArgs e)
        {

            View.AddFile uploadFile = new View.AddFile();
            uploadFile.Owner = this;
            uploadFile.ShowDialog();

            if (uploadFile.DialogResult.HasValue && uploadFile.DialogResult.Value)
            {
                UpdateSaveStatus(false);
                string fileLinkName = uploadFile.linkInput.name;
                Button buttonObject = (sender as Button);
                int buttonIndex = int.Parse(buttonObject.Name.Replace("Button_AddFile_", ""));

                this.FilesList[buttonIndex].Add(uploadFile.linkInput);

                UpdateLinkList(buttonIndex);
            }
        }

        /// <summary>
        /// en lille kanp der fjner et link/dokument igen
        /// </summary>
        private void newLinkRemove_Click(object sender, RoutedEventArgs e)
        {
            Button thisButton = (sender as Button);

            string buttonName = thisButton.Name.Replace("Button_RemoveLink_", "");
            int scoreId = buttonName.IndexOf("_");
            int FieldIndex = int.Parse(buttonName.Substring(0, scoreId));
            int FileIndex = int.Parse(buttonName.Substring(scoreId + 1));

            if (!this.FilesList[FieldIndex][FileIndex].isLink)
            {
                string filePath = Directory.GetCurrentDirectory() + @"\Files\" + this.FilesList[FieldIndex][FileIndex].path;

                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }

            this.FilesList[FieldIndex].RemoveAt(FileIndex);
            UpdateLinkList(FieldIndex);

            UpdateSaveStatus(false);
        }

        /// <summary>
        /// åbner dokument eller link
        /// </summary>
        private void newLink_Click(object sender, RoutedEventArgs e)
        {
            Button thisButton = (sender as Button);

            string buttonName = thisButton.Name.Replace("Button_OpenLink_", "");
            int scoreId = buttonName.IndexOf("_");
            int FieldIndex = int.Parse(buttonName.Substring(0, scoreId));
            int FileIndex = int.Parse(buttonName.Substring(scoreId + 1));


            Model.FileClass.Links selectFil = this.FilesList[FieldIndex][FileIndex];
            string path = "";

            if (selectFil.isLink)
            {
                if (!selectFil.path.ToLower().StartsWith("http://") || !selectFil.path.ToLower().StartsWith("https://"))
                {
                    //MessageBox.Show(selectFil.path);
                    path = "http://" + selectFil.path;
                }
            }
            else
            {
                path = Directory.GetCurrentDirectory() + @"\Files\" + selectFil.path;

                if (!File.Exists(path))
                {
                    return;
                }
            }

            Process wordProcess = new Process();
            wordProcess.StartInfo.FileName = path;
            wordProcess.StartInfo.UseShellExecute = true;
            wordProcess.Start();
        }

        #endregion Events

        #region Button Click Functions

        //Åben En menu
        private void Button_ToolStart_Click(object sender, RoutedEventArgs e)
        {
            //Hvis ikke gemt spørg om man vil
            if(!this.saved)
            {
                MessageBoxResult result = MessageBox.Show("Gem Dagbog?", "Gem", MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                {
                    SaveFile();
                }

            }

            //Åben et lille vindue menu
            //hvor man kan vælge hvad man nu vil fortage sig
            View.SelectType selectType = new View.SelectType();
            selectType.IsDialog = true;
            selectType.showContinueButton = true;
            selectType.ShowInTaskbar = false;
            selectType.Owner = this;
            selectType.ShowDialog();

            //hvis man vælger noget lukker det dette vindue
            if (selectType.DialogResult.HasValue && selectType.DialogResult.Value)
            {
                this.Close();
            }
        }

        //Gem fil
        private void Button_ToolSave_Click(object sender, RoutedEventArgs e)
        {
            //Gem fil
            if (!this.saved)
            {
                SaveFile();
            }
        }

        //Bliver ikke brugt
        private void Button_ToolNew_Click(object sender, RoutedEventArgs e)
        {
            if(!this.saved)
            {
                MessageBoxResult result = MessageBox.Show("Gem?", "Gem", MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                {
                    SaveFile();
                }
            }
            Inc.Settings.fileInput = new Model.FileClass.fileInput();

            RemoveAllFreeText();
            AddFreeText();
            LoadFile();
        }

        //Bliver ikke brugt
        private void Button_ToolEdit_Click(object sender, RoutedEventArgs e)
        {
            if (!this.saved)
            {
                MessageBoxResult result = MessageBox.Show("Gem?", "Gem", MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                {
                    SaveFile();
                }
            }
            View.OpenList showList = new View.OpenList();

            showList.ShowDialog();
            if (showList.DialogResult.HasValue && showList.DialogResult.Value)
            {

                RemoveAllFreeText();
                AddFreeText();
                LoadFile();
            }
        }

        //Tilføj et nyt log/ tekst felt 
        private void Button_AddTextField_Click(object sender, RoutedEventArgs e)
        {
            AddFreeText();
        }

        #endregion Button Click Functions

        #region Auto Save

        /// <summary>
        /// Denne function opret automatisk gem ver minut
        /// </summary>
        /// <param name="minutes">antal minuter der skal gå før den gemmer</param>
        private void SetSaveTimer(int minutes)
        {
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMinutes(minutes);

            _timer.Tick += _timer_Save;
            _timer.Start();
        }
        
        /*
         *  timer/  autosave function call
         *  Gemmer dagbogen
         */
        private void _timer_Save(object sender, EventArgs e)
        {
            if (!this.saved)
            {
                SaveFile();
            }

        }
        
        #endregion

        #region File has change

        /// <summary>
        /// giver ver element en change event
        /// så hvis man ændre i noget bliver det opdaget
        /// </summary>
        private void SetChangeFunction()
        {
            foreach (TextBox item in FindVisualChildren<TextBox>(this))
            {
                item.KeyUp += Field_Change;
            }
            foreach (RichTextBox item in FindVisualChildren<RichTextBox>(this))
            {
                item.KeyUp += Field_Change;
            }
            foreach (CheckBox item in FindVisualChildren<CheckBox>(this))
            {
                item.Checked += Field_Change;
            }
            foreach (DatePicker item in FindVisualChildren<DatePicker>(this))
            {
                item.SelectedDateChanged += Field_Change;
            }
            foreach (ComboBox item in FindVisualChildren<ComboBox>(this))
            {
                item.SelectionChanged += Field_Change;
            }
            foreach (DataGrid item in FindVisualChildren<DataGrid>(this))
            {
                item.BeginningEdit += Field_Change;
            }
            foreach (ListView item in FindVisualChildren<ListView>(this))
            {
                ((System.Collections.Specialized.INotifyCollectionChanged)item.Items).CollectionChanged += Field_Change;
            }

        }

        private void Field_Change(object sender, EventArgs e)
        {
            //MessageBox.Show("IsLoading: " + this.isLoading.ToString());
            if (!this.isLoading)
            {
                UpdateSaveStatus(false);
            }

        }

        /// <summary>
        /// Gør det muligt at hente flere element af samme slag
        /// </summary>
        private IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }

        #endregion  File has change

        #region Functioner

        /// <summary>
        /// Gør så man kan give en RichTextBox noget tekst uden
        /// formeget kode
        /// </summary>
        /// <param name="textbox">Element af textbox</param>
        /// <param name="text">Tekst texbox skal indehode</param>
        private void SetRichTextbox(RichTextBox textbox, string text)
        {
            textbox.Document.Blocks.Clear();
            textbox.AppendText(text);

        }

        /// <summary>
        /// Gør så man kan hente en RichTextBox tekst uden
        /// formeget kode
        /// </summary>
        /// <param name="textbox">Element af textbox</param>
        /// <returns>RichTextBox Teskt</returns>
        private string GetRichTextbox(RichTextBox textbox)
        {
            TextRange textRange = new TextRange(
                textbox.Document.ContentStart,
                textbox.Document.ContentEnd
            );

            return textRange.Text;
        }

        /// <summary>
        /// Ændre save billede icon
        /// </summary>
        private void UpdateSaveStatus(bool status)
        {
            string imageUrl = "";

            if (status)
            {
                imageUrl = "Images/save_checkv2.png";
            }
            else
            {

                imageUrl = "Images/save_uncheck.png";
            }
            BitmapImage bitImage = new BitmapImage();
            bitImage.BeginInit();
            bitImage.UriSource = new Uri(imageUrl, UriKind.Relative);
            bitImage.EndInit();

            imageSave.Stretch = Stretch.Fill;
            imageSave.Source = bitImage;
            this.saved = status;
        }

        /// <summary>
        /// Henter data fra fil og indlæser dem
        /// </summary>
        private void LoadFile()
        {
            //Hent class med functioner
            Model.FileClass.function funcFile = new Model.FileClass.function();

            //Sæt elev info
            TextBox_name.Text = Inc.Settings.fileInput.name;
            TextBox_cpr.Text = Inc.Settings.fileInput.cpr;

            //Sorter dagbog log efter dato
            Model.FileClass.function.sortDateArray daglog = funcFile.DateSort(Inc.Settings.fileInput);
            this.FilesList = Inc.Settings.fileInput.files; //Hent links/Dokumenter for elev

            /*
             * Tilføj en nye tekst boks for hver log der er
             */
            for (int i = 0; i < daglog.dato.Count; i++)
            {
                AddFreeText();

                DatePicker datePickerDato = this.FindName("DatePicker_Dato_" + i) as DatePicker;
                RichTextBox RichTextBoxFreeText = this.FindName("RichTextBox_FreeText_" + i) as RichTextBox;

                DateTime selectDate = daglog.dato[i];

                datePickerDato.SelectedDate = selectDate;
                SetRichTextbox(RichTextBoxFreeText, daglog.text[i]);
                UpdateLinkList(i);

            }
        }

        /// <summary>
        /// Gemmer dagbog data til fil
        /// </summary>
        private void SaveFile()
        {
            Model.FileClass funcFile = new Model.FileClass();

            //Henter elev info
            Inc.Settings.fileInput.name = TextBox_name.Text;
            Inc.Settings.fileInput.cpr = TextBox_cpr.Text;


            Inc.Settings.fileInput.dato = new List<string>();       //Vil indeholde dato`er
            Inc.Settings.fileInput.freeText = new List<string>();   //Vil indeholde log Tekst
            Inc.Settings.fileInput.files = this.FilesList;          //Vil indeholde links/Dokumenter


            /*
             * Henter dato`er og logs
             * 
             * hvis log er sat
             */
            for (int i = 0; i < this.freeTextCount; i++)
            {
                DatePicker datePickerDato = this.FindName("DatePicker_Dato_" + i) as DatePicker;
                RichTextBox RichTextBoxFreeText = FindName("RichTextBox_FreeText_" + i) as RichTextBox;


                if (RichTextBoxFreeText != null)
                {
                    string RichTextBoxFreeText_String = GetRichTextbox(RichTextBoxFreeText);
                    RichTextBoxFreeText_String.Trim();

                    if (RichTextBoxFreeText_String.Length > 0)
                    {
                        string datePickerDato_String = DateTime.Now.ToString();

                        if (datePickerDato.SelectedDate.HasValue)
                        {
                            datePickerDato_String = datePickerDato.SelectedDate.Value.ToString();
                        }

                        Inc.Settings.fileInput.dato.Add(datePickerDato_String);
                        Inc.Settings.fileInput.freeText.Add(RichTextBoxFreeText_String);
                    }
                }
            }


            funcFile.SaveFile(); //Gemmer til fil
            UpdateSaveStatus(true); //opdater billede icon på save knap
        }

        /// <summary>
        /// Tilføjer et nye teskt felt
        /// 
        /// det vil sige at der bliver tilføjet
        /// En datepicker
        /// En richtextbox
        /// og nogle label`er
        /// </summary>
        private void AddFreeText()
        {
            //Tilføjer en liste så man bare kan bruge eks.
            //FilesList[id].add(<Model.FileClass.Links>)
            FilesList.Add(new List<Model.FileClass.Links>());

            //hent grid element som får de nye element
            //label, datepicker og richtextbox
            Grid gridBox = Grid_FreeText;

            //Indsæt rækker i grid så det ikke kommer til at se forkert ud
            for (int i = 0; i < 4; i++)
            {
                RowDefinition newRow = new RowDefinition();
                newRow.Height = GridLength.Auto;
                gridBox.RowDefinitions.Add(newRow);
            }
            //hvad der er den førte række
            //Vil ikke være 0 data denne grid og inde holder
            //log elementer fra andre dage
            int RowStart = gridBox.RowDefinitions.Count - 4;
            int nameId = this.freeTextCount; //Id for datapicker og richtextbox
            this.freeTextCount++; //sæt id for næste AddFreeText()

            //Info label Dato
            Label dateLabel = new Label();
            dateLabel.Content = "Dato:";
            dateLabel.Name = "DateLabel_Dato_" + nameId;
            dateLabel.FontWeight = FontWeights.Bold;
            dateLabel.Margin = new Thickness(0, 10, 0, 0);
            dateLabel.VerticalAlignment = VerticalAlignment.Top;
            dateLabel.HorizontalAlignment = HorizontalAlignment.Left;
            this.RegisterName("DateLabel_Dato_" + nameId, dateLabel);

            //Datepicker Dato
            DatePicker datePicker = new DatePicker();
            datePicker.Name = "DatePicker_Dato_" + nameId;
            datePicker.Width = 150;
            datePicker.Height = 24;
            datePicker.Margin = new Thickness(50, 10, 0, 0);
            datePicker.VerticalAlignment = VerticalAlignment.Top;
            datePicker.HorizontalAlignment = HorizontalAlignment.Left;
            datePicker.SelectedDate = DateTime.Now;
            this.RegisterName("DatePicker_Dato_" + nameId, datePicker);

            //RichTextBox log tekst
            RichTextBox freeTextRichTextBox = new RichTextBox();
            freeTextRichTextBox.Name = "RichTextBox_FreeText_" + nameId;
            freeTextRichTextBox.Margin = new Thickness(0, 10, 0, 0);
            freeTextRichTextBox.Document = new FlowDocument();
            freeTextRichTextBox.Width = 800;
            freeTextRichTextBox.Height = 100;
            freeTextRichTextBox.VerticalAlignment = VerticalAlignment.Top;
            freeTextRichTextBox.HorizontalAlignment = HorizontalAlignment.Left;
            this.RegisterName("RichTextBox_FreeText_" + nameId, freeTextRichTextBox);

            //RichTextBox skal kun have et lille mellemrum
            Style noSpaceStyle = new Style(typeof(Paragraph));
            noSpaceStyle.Setters.Add(new Setter(Paragraph.MarginProperty, new Thickness(0)));
            freeTextRichTextBox.Resources.Add(typeof(Paragraph), noSpaceStyle);

            //Knap til at tilføje fil
            Button AddFilesButton = new Button();
            AddFilesButton.Name = "Button_AddFile_" + nameId;
            AddFilesButton.Content = "Tilføj Fil/Link";
            AddFilesButton.Width = 95;
            AddFilesButton.VerticalAlignment = VerticalAlignment.Top;
            AddFilesButton.HorizontalAlignment = HorizontalAlignment.Left;
            AddFilesButton.Margin = new Thickness(0, 5, 0, 0);
            AddFilesButton.Padding = new Thickness(10, 5, 10, 5);
            AddFilesButton.Click += AddFilesButton_Click;
            this.RegisterName("Button_AddFile_" + nameId, AddFilesButton);

            //gør så links/dokumenter kommer til at
            //stå lige ved siden af hinanden
            WrapPanel LinkBoxWrapPanel = new WrapPanel();
            LinkBoxWrapPanel.Name = "WrapPanel_AddFile_" + nameId;
            LinkBoxWrapPanel.Margin = new Thickness(0, 5, 0, 0);
            LinkBoxWrapPanel.MaxWidth = 800;
            LinkBoxWrapPanel.VerticalAlignment = VerticalAlignment.Top;
            LinkBoxWrapPanel.HorizontalAlignment = HorizontalAlignment.Left;
            this.RegisterName("WrapPanel_AddFile_" + nameId, LinkBoxWrapPanel);

            //sæt elemter på de rigtige rækker i grid
            Grid.SetRow(dateLabel, RowStart);
            Grid.SetRow(datePicker, RowStart);

            Grid.SetRow(freeTextRichTextBox, RowStart + 1);
            Grid.SetRow(AddFilesButton, RowStart + 2);
            Grid.SetRow(LinkBoxWrapPanel, RowStart + 3);

            gridBox.Children.Add(dateLabel);
            gridBox.Children.Add(datePicker);

            gridBox.Children.Add(freeTextRichTextBox);
            gridBox.Children.Add(AddFilesButton);
            gridBox.Children.Add(LinkBoxWrapPanel);
        }

        /// <summary>
        /// hvis er link er tilføjet eller fjernet
        /// knap liste
        /// </summary>
        /// <param name="Index">Dato log  nameId</param>
        private void UpdateLinkList(int Index)
        {
            WrapPanel wrapPanelObject = FindName("WrapPanel_AddFile_" + Index) as WrapPanel;
            wrapPanelObject.Children.Clear();

            int fileIndex = 0;
            foreach (var item in this.FilesList[Index])
            {

                TextBlock newContent = new TextBlock();
                newContent.Text = item.name;
                newContent.TextDecorations = TextDecorations.Underline;


                Button newLinkRemove = new Button();
                newLinkRemove.Content = "X";
                newLinkRemove.Name = "Button_RemoveLink_" + Index + "_" + fileIndex;
                newLinkRemove.Padding = new Thickness(0);
                newLinkRemove.Margin = new Thickness(0);
                newLinkRemove.BorderThickness = new Thickness(0);
                newLinkRemove.Background = Brushes.Transparent;
                newLinkRemove.FontWeight = FontWeights.Bold;
                newLinkRemove.Foreground = Brushes.Red;
                newLinkRemove.Click += newLinkRemove_Click;

                Button newLink = new Button();
                newLink.Content = newContent;
                newLink.Name = "Button_OpenLink_" + Index + "_" + fileIndex;
                newLink.Padding = new Thickness(0);
                newLink.Margin = new Thickness(0);
                newLink.BorderThickness = new Thickness(0);
                newLink.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF2C72B8"));
                newLink.Background = Brushes.Transparent;
                newLink.Click += newLink_Click;

                DockPanel linkDock = new DockPanel();
                linkDock.Children.Add(newLink);
                linkDock.Children.Add(newLinkRemove);

                if (wrapPanelObject.Children.Count != 0)
                {
                    Label labelComma = new Label();
                    labelComma.Content = ", ";
                    labelComma.Padding = new Thickness(0);
                    labelComma.Margin = new Thickness(0);
                    wrapPanelObject.Children.Add(labelComma);
                }

                wrapPanelObject.Children.Add(linkDock);
                fileIndex++;
            }
        }

        /// <summary>
        /// Fjerner alle logs elementer
        /// </summary>
        private void RemoveAllFreeText()
        {
            for (int i = 0; i < this.freeTextCount; i++)
            {

                Label LabelDato = this.FindName("DateLabel_Dato_" + i) as Label;
                DatePicker datePickerDato = this.FindName("DatePicker_Dato_" + i) as DatePicker;
                RichTextBox RichTextBoxFreeText = this.FindName("RichTextBox_FreeText_" + i) as RichTextBox;


                if (LabelDato != null && datePickerDato != null && RichTextBoxFreeText != null)
                {
                    this.UnregisterName("DateLabel_Dato_" + i);
                    this.UnregisterName("DatePicker_Dato_" + i);
                    this.UnregisterName("RichTextBox_FreeText_" + i);

                    Grid_FreeText.Children.Remove(LabelDato);
                    Grid_FreeText.Children.Remove(datePickerDato);
                    Grid_FreeText.Children.Remove(RichTextBoxFreeText);
                }
            }

            this.freeTextCount = 0;
        }

        #endregion Functioner

    }
}
