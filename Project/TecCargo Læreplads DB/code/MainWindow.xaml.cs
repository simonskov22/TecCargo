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
using System.IO;
using System.Data;
using System.Diagnostics;
using System.Timers;
using System.Windows.Threading;

namespace TecCargo_Læreplads_DB
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Global Variables

        private DispatcherTimer _timer;
        private bool saved = true;
        private bool isLoading = false;

        #endregion Global Variables

        public MainWindow()
        {
            InitializeComponent();

            DataContext = new Inc.Datagrid();

            LabelVersion_version.Content += Inc.Settings._version.ToString();
            LabelVersion_author.Content += Inc.Settings.CreateBy;
        }

        #region Button Click Events

        /// <summary>
        /// åbener et vindue hvor man kan
        /// vælge om vil oprette en eller
        /// rediger en gammel
        /// </summary>
        private void Button_ToolMenu_Click(object sender, RoutedEventArgs e)
        {
            //åben vindue
            View.SelectType startMenu = new View.SelectType();
            startMenu.showContinueButton = true;
            startMenu.Owner = this;
            startMenu.ShowDialog();

            //hvis man vælger opret ny eller rediger 
            //spørg om man vil gemme filen
            if (startMenu.DialogResult.HasValue && startMenu.DialogResult.Value)
            {
                if (!this.saved)
                {
                    MessageBoxResult result = MessageBox.Show("Gem lærepladsen?", "Gem", MessageBoxButton.YesNo);

                    if (result == MessageBoxResult.Yes)
                    {
                        SaveFile();
                    }
                }

                LoadFile(); //load inhold af den nye fil
            }
        }
        
        /// <summary>
        /// gem filen
        /// </summary>
        private void Button_ToolSave_Click(object sender, RoutedEventArgs e)
        {
            if (!this.saved)
            {
                SaveFile();
            }
        }
        
        /// <summary>
        /// åbner print vindue hvor man
        /// kan vælge print og antal
        /// </summary>
        private void Button_ToolPrint_Click(object sender, RoutedEventArgs e)
        {   
            SaveFile();//hver sikker på at den printer den nyeste fil

            //åben print vindue
            View.PrinterOptions selectPrinter = new View.PrinterOptions();
            selectPrinter.Owner = this;
            selectPrinter.ShowDialog();
        }
        
        /// <summary>
        /// åbner et vindue hvor man kan
        /// tilføj en fil eller link
        /// </summary>
        private void Button_AddFiles_Click(object sender, RoutedEventArgs e)
        {
            //åben vindue
            View.addFile addFile = new View.addFile();
            addFile.Owner = this;
            addFile.ShowDialog();

            //hvis man har valgt en fil eller link
            //skal det gemmes
            if (addFile.DialogResult.HasValue && addFile.DialogResult.Value)
            {
                Inc.Settings.fileInput.files.Add(addFile.fileInput);
            }

            loadFileList();//opdatere fil listen fra ny
        }

        /// <summary>
        /// en knap inde i listview
        /// den åben den valgte fil/link
        /// </summary>
        private void ListViewFileNames_OpenButton_Click(object sender, RoutedEventArgs e)
        {
            //find ud af hvad knap der er trykket på
            int index = ListViewFileNames.Items.IndexOf((sender as Button).DataContext);

            //tjek om den findes
            if (index != -1)
            {
                //hent fil/link oplysninger
                Model.FileClass.files selectFil = Inc.Settings.fileInput.files[index];
                string path = "";

                //hvis det er et link
                //tjek om det indeholder http://
                if (selectFil.link)
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
                }

                //åben fil/link
                Process wordProcess = new Process();
                wordProcess.StartInfo.FileName = path;
                wordProcess.StartInfo.UseShellExecute = true;
                wordProcess.Start();

            }
        }

        /// <summary>
        /// en knap inde i listview
        /// fjerner en fil/link fra listen
        /// </summary>
        private void ListViewFileNames_DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            //find ud af hvad knap der er trykket på
            int index = ListViewFileNames.Items.IndexOf((sender as Button).DataContext);

            //tjek om den findes
            if (index != -1)
            {
                //hent fil/oplysninger
                Model.FileClass.files input = Inc.Settings.fileInput.files[index];

                //hvis det er en fil
                //skal denn fil slettes
                if (!input.link)
                {
                    string pathFileAndName = Directory.GetCurrentDirectory() + @"\Files\" + input.path;

                    if (File.Exists(pathFileAndName))
                    {
                        File.Delete(pathFileAndName);
                    }
                }

                Inc.Settings.fileInput.files.RemoveAt(index); //fjern fra liste
                loadFileList();//opdater fil/link liste
            }
        }

        /// <summary>
        /// hvis der skal være kontak personer
        /// </summary>
        private void button_AddRow_Click(object sender, RoutedEventArgs e)
        {

            if ((sender as Button).Name == "Button_contact")
            {
                addDatagridRow(kontaktPer);
            }
            else
            {
                addDatagridRow(kandidater);
            }
        }

        /// <summary>
        /// tilføj valgte sporg til listview
        /// </summary>
        private void ListView_sporg_Add_Click(object sender, RoutedEventArgs e)
        {
            //hent id
            int index = combobox_sporg.SelectedIndex;

            //tjek om findes
            if (index != -1)
            {
                //tilføj valgte til listen
                string comboBoxVal = combobox_sporg.Text;
                listview classVal = new listview();
                classVal.name = comboBoxVal;
                ListView_sporg.Items.Add(classVal);

            }
        }
        
        /// <summary>
        /// en knap inde i listview
        /// fjern sporg fra liste
        /// </summary>
        private void ListView_sporg_Delete_Click(object sender, RoutedEventArgs e)
        {
            //hent id
            int index = ListView_sporg.Items.IndexOf((sender as Button).DataContext);

            //hvis findes slet fra listen
            if (index != -1)
            {
                ListView_sporg.Items.RemoveAt(index);
            }
        }

        /// <summary>
        /// tilføj valgte til listen
        /// </summary>
        private void ListView_straffeAttest_Add_Click(object sender, RoutedEventArgs e)
        {
            //hent id
            int index = combobox_straffeAttest.SelectedIndex;

            //hvis findes tilføj den til listen
            if (index != -1)
            {
                string comboBoxVal = combobox_straffeAttest.Text;
                listview classVal = new listview();
                classVal.name = comboBoxVal;
                ListView_straffeAttest.Items.Add(classVal);

            }
        }
        
        /// <summary>
        /// en knap inde i listview
        /// fjern valgte fra listen
        /// </summary>
        private void ListView_straffeAttest_Delete_Click(object sender, RoutedEventArgs e)
        {
            //find id
            int index = ListView_straffeAttest.Items.IndexOf((sender as Button).DataContext);

            //hvis findes slet
            if (index != -1)
            {
                ListView_straffeAttest.Items.RemoveAt(index);
            }
        }
        

        #endregion Button Click Events

        #region Events

        /// <summary>
        /// når vinduet er loaded
        /// åben et nyt vindue hvor man kan vælge 
        /// at åben en gemt fil
        /// </summary>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //åben vindue
            View.SelectType openMenu = new View.SelectType();
            openMenu.Owner = this;
            openMenu.ShowDialog();

            //hvis man har valgt at ændre i en fil
            //hent oplyninger i filen
            if (openMenu.DialogResult.HasValue && openMenu.DialogResult.Value)
            {
                LoadFile();
            }

            //vær sikker på at der minimum en række i datagrid
            if (kontaktPer.Items.Count == 0)
            {
                addDatagridRow(kontaktPer);
            }
            if (kandidater.Items.Count == 0)
            {
                addDatagridRow(kandidater);
            }


            SetSaveTimer(5);     //sæt auto gem timer
            SetChangeFunction(); //sæt change event på alle elementer der kan ændres i
        }

        /// <summary>
        /// når vinduet lukker ned spørg om vil gemme filen
        /// </summary>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!this.saved)
            {
                MessageBoxResult result = MessageBox.Show("Gem læreplads?", "Gem", MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                {
                    SaveFile();
                }
            }
        }

        #endregion Events

        #region Functions

        /// <summary>
        /// opdatere elementer med værdir der er i Inc.Settings.fileInput
        /// </summary>
        private void LoadFile()
        {
            this.isLoading = true;

            //sæt virsomhed oplysninger
            virkNavn.Text = Inc.Settings.fileInput.name;
            virkCVR.Text = Inc.Settings.fileInput.cvr;
            virkAddr.Text = Inc.Settings.fileInput.adresse;
            virkAndet.Text = Inc.Settings.fileInput.tjenestested;
            SetRichTextbox(virkProfil, Inc.Settings.fileInput.profil);

            //opret kontaktperson source liste
            List<Inc.person> itemSourceConPer = new List<Inc.person>();

            for (int i = 0; i < Inc.Settings.fileInput.contactPerson.Count; i++)
            {
                Inc.person newPerson = new Inc.person();
                newPerson.name = Inc.Settings.fileInput.contactPerson[i].name;
                newPerson.mobil = Inc.Settings.fileInput.contactPerson[i].mobile;
                newPerson.mail = Inc.Settings.fileInput.contactPerson[i].mail;
                newPerson.postion = Inc.Settings.fileInput.contactPerson[i].post;
                newPerson.print = Inc.Settings.fileInput.contactPerson[i].print;

                itemSourceConPer.Add(newPerson);
            }

            //sæt kontakt personer
            kontaktPer.ItemsSource = CollectionViewSource.GetDefaultView(itemSourceConPer);

            //hak af checkbokse (hvad uddannelseaftale indeholder)
            foreach (var item in Inc.Settings.fileInput.uddannelsesCheckBoxs)
            {
                (FindName(item) as CheckBox).IsChecked = true;
            }

            //sæt antal elever de tager
            textbox_elever_lager.Text = Inc.Settings.fileInput.uddannelses_lager;
            textbox_elever_lastbil.Text = Inc.Settings.fileInput.uddannelses_chauffor;
            textbox_elever_lufthavn.Text = Inc.Settings.fileInput.uddannelses_lufthavn;


            //hak af checkbokse (hvad uddannelseaftale type)
            foreach (var item in Inc.Settings.fileInput.agreementType)
            {
                (FindName(item) as CheckBox).IsChecked = true;
            }


            DateTime lastDay = new DateTime();  //Ansøgningsfrist
            DateTime startDate = new DateTime();//Start dato for uddannelsesaftale:

            //hvis datoerne ikke er sat
            if (!DateTime.TryParse(Inc.Settings.fileInput.firmReceive.lastDay, out lastDay))
            {
                lastDay = DateTime.Now;
            }

            if (!DateTime.TryParse(Inc.Settings.fileInput.firmReceive.StartDate, out startDate))
            {
                startDate = DateTime.Now;
            }


            //hak af checkbokse (hvad virksomheden vil modtage)
            foreach (var item in Inc.Settings.fileInput.firmReceive.Checkbox)
            {
                (FindName(item) as CheckBox).IsChecked = true;
            }

            //sæt forskelige tekstbokse
            textbox_andet.Text = Inc.Settings.fileInput.firmReceive.andet;
            textbox_antalAftaler.Text = Inc.Settings.fileInput.firmReceive.antalAftaler;
            datepicker_Frist.SelectedDate = lastDay;
            textbox_anatalKandidater.Text = Inc.Settings.fileInput.firmReceive.antalPersoner;
            datepicker_startDato.SelectedDate = startDate;
            SetRichTextbox(jobDes, Inc.Settings.fileInput.jobDescription);

            //sæt valgte sporg liste
            ListView_sporg.Items.Clear();
            foreach (var item in Inc.Settings.fileInput.language)
            {
                listview newView = new listview();
                newView.name = item;
                ListView_sporg.Items.Add(newView);
            }

            //sæt valgte straffeattest liste
            ListView_straffeAttest.Items.Clear();
            foreach (var item in Inc.Settings.fileInput.record)
            {
                listview newView = new listview();
                newView.name = item;
                ListView_straffeAttest.Items.Add(newView);
            }

            //sæt dropdown bokse
            combobox_alder.Text = Inc.Settings.fileInput.age;
            combobox_driver.Text = Inc.Settings.fileInput.driverLicense;
            combobox_fysiske.Text = Inc.Settings.fileInput.physical;
            combobox_matematik.Text = Inc.Settings.fileInput.math;

            //sæt tekstbokse
            SetRichTextbox(textbox_etvAndet, Inc.Settings.fileInput.other);
            SetRichTextbox(VUF, Inc.Settings.fileInput.VUF);
            SetRichTextbox(signedBy, Inc.Settings.fileInput.signedBy);

            //sæt kandiater
            List<Inc.person> itemSourceCandi = new List<Inc.person>();

            for (int i = 0; i < Inc.Settings.fileInput.candidates.Count; i++)
            {
                Inc.person newCandi = new Inc.person();
                newCandi.name = Inc.Settings.fileInput.candidates[i];

                itemSourceCandi.Add(newCandi);
            }

            kandidater.ItemsSource = CollectionViewSource.GetDefaultView(itemSourceCandi);

            loadFileList(); //opdater fil liste

            //så er minimum en række
            if (kontaktPer.Items.Count == 0)
            {
                addDatagridRow(kontaktPer);
            }
            if (kandidater.Items.Count == 0)
            {
                addDatagridRow(kandidater);
            }

            this.isLoading = false;
        }
        
        /// <summary>
        /// gem læreplads til fil
        /// </summary>
        private void SaveFile()
        {
            Model.FileClass.fileInput inputs = new Model.FileClass.fileInput();//opret tom input

            inputs.filename = Inc.Settings.fileInput.filename; //hent fil navn
            inputs.files = Inc.Settings.fileInput.files;    //hent fil/links

            //hent virksomhed oplyninger
            inputs.name = virkNavn.Text;
            inputs.cvr = virkCVR.Text;
            inputs.adresse = virkAddr.Text;
            inputs.tjenestested = virkAndet.Text;
            inputs.profil = GetRichTextbox(virkProfil);

            //hent kontakt personer
            for (int i = 0; i < kontaktPer.Items.Count; i++)
            {
                Model.FileClass.contactPerson conPer = new Model.FileClass.contactPerson();
                Inc.person dataRow = GetDataGridPersonRow(kontaktPer, i);

                conPer.name = dataRow.name;
                conPer.mobile = dataRow.mobil;
                conPer.mail = dataRow.mail;
                conPer.post = dataRow.postion;
                conPer.print = dataRow.print;

                if (conPer.name != null || conPer.mobile != null ||
                    conPer.mail != null || conPer.post != null)
                {
                    inputs.contactPerson.Add(conPer);
                }
            }

            //de checkbokse der er i uddannelseaftale indeholder
            string[] uddCheckBoxNames = { 
                "lagerHelp", "lagerLogistik", "logerTransport", 
                "lastbil", "lastbilGods", "lastbilFlytte",
                "lastbilRenovation", "lastbilKran", "lufthavn",
                "lufthavnBagage", "lufthavnCargo", "lufthavnAircraft",
                "lufthavnAirport", "lufthavnBrand", "lufthavnFuel",
                "lufthavnClean", "lufthavnGround", "lufthavnRampe" 
            };

            //hvis tjekket af gem navn
            foreach (var item in uddCheckBoxNames)
            {

                if ((FindName(item) as CheckBox).IsChecked.HasValue &&
                    (FindName(item) as CheckBox).IsChecked.Value)
                {

                    inputs.uddannelsesCheckBoxs.Add(item);
                }
            }

            //hent antal elev virksomheden vil have
            inputs.uddannelses_lager = textbox_elever_lager.Text;
            inputs.uddannelses_chauffor = textbox_elever_lastbil.Text;
            inputs.uddannelses_lufthavn = textbox_elever_lufthavn.Text;


            //de checkbokse der er i uddannelseaftale type
            string[] aftaleCheckBoxNames = { 
                "checkbox_aml", "checkbox_kombi", "checkbox_rest", 
                "checkbox_mester", "checkbox_kort", "checkbox_del"
            };

            //hvis tjekket af gem navn
            foreach (var item in aftaleCheckBoxNames)
            {
                if ((FindName(item) as CheckBox).IsChecked.HasValue &&
                    (FindName(item) as CheckBox).IsChecked.Value)
                {
                    inputs.agreementType.Add(item);
                }
            }


            //oplysninger om lærepladsen
            Model.FileClass.firmReceive firmReceive = new Model.FileClass.firmReceive();

            firmReceive.andet = textbox_andet.Text;
            firmReceive.antalAftaler = textbox_antalAftaler.Text;
            firmReceive.lastDay = GetDatePickerValue(datepicker_Frist);
            firmReceive.antalPersoner = textbox_anatalKandidater.Text;
            firmReceive.StartDate = GetDatePickerValue(datepicker_startDato);

            //checkbokse om hvad virksomheden vil modtage
            string[] firmReceiveCheckBox = { "checkbox_motiveret", "checkbox_cv", "checkbox_perTele", 
                                      "checkbox_perMeet", "checkbox_andet",
                                     };

            //hvis tjekket af gem navn
            foreach (var item in firmReceiveCheckBox)
            {

                if ((FindName(item) as CheckBox).IsChecked.HasValue &&
                    (FindName(item) as CheckBox).IsChecked.Value)
                {

                    firmReceive.Checkbox.Add(item);
                }
            }
            inputs.firmReceive = firmReceive;

            //hent valgte sporg
            foreach (var item in ListView_sporg.Items)
            {
                string value = (item as listview).name;

                if (!inputs.language.Contains(value))
                {
                    inputs.language.Add(value);
                }
            }

            //hent straffeattest
            foreach (var item in ListView_straffeAttest.Items)
            {
                string value = (item as listview).name;

                if (!inputs.record.Contains(value))
                {
                    inputs.record.Add(value);
                }
            }

            //hent tesktbokse teskt
            inputs.jobDescription = GetRichTextbox(jobDes);
            inputs.age = combobox_alder.Text;
            inputs.driverLicense = combobox_driver.Text;
            inputs.physical = combobox_fysiske.Text;
            inputs.math = combobox_matematik.Text;
            inputs.other = GetRichTextbox(textbox_etvAndet);
            inputs.VUF = GetRichTextbox(VUF);
            inputs.signedBy = GetRichTextbox(signedBy);

            //hent kandidater
            for (int i = 0; i < kandidater.Items.Count; i++)
            {
                Inc.person kandiPer = GetDataGridPersonRow(kandidater, i);
                if (kandiPer.name != "")
                {
                    inputs.candidates.Add(kandiPer.name);
                }
            }

            Inc.Settings.fileInput = inputs; //opdater nuværende fil

            //skriv til xml
            Model.FileClass func = new Model.FileClass();
            func.SaveFile();

            UpdateSaveStatus(true); //opdater gemt status
        }

        /// <summary>
        /// ændre billede på save knap
        /// og opdater this.saved
        /// </summary>
        private void UpdateSaveStatus(bool status)
        {
            string imageUrl = "";

            if (status)
            {
                imageUrl = "Images/save_check.png";
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
        /// tilføj en tom række til datagrid
        /// </summary>
        private void addDatagridRow(DataGrid datagrid)
        {
            var itemsView = datagrid.ItemsSource;

            (itemsView as System.ComponentModel.IEditableCollectionView).AddNew();
        }
        
        /// <summary>
        /// opdater file/link liste til nyeste
        /// </summary>
        private void loadFileList()
        {
            ListViewFileNames.Items.Clear();
            foreach (var item in Inc.Settings.fileInput.files)
            {
                listview newItem = new listview();
                newItem.name = item.name;

                ListViewFileNames.Items.Add(newItem);
            }
        }

        /// <summary>
        /// henter ToShortDateString af en datetimepicker 
        /// hvis den har en værdi
        /// </summary>
        private string GetDatePickerValue(DatePicker datetimepicker)
        {
            if (datetimepicker.SelectedDate.HasValue)
            {
                return datetimepicker.SelectedDate.Value.ToShortDateString();
            }
            else
            {
                return "";
            }
        }
        
        /// <summary>
        /// henter kontaktperson oplysninger ud fra id
        /// </summary>
        private Inc.person GetDataGridPersonRow(DataGrid datagrid, int row)
        {
            Inc.person itemsRow = (datagrid.Items[row] as Inc.person);
            
            return itemsRow;
        }
        
        /// <summary>
        /// henter tekst fra en RichTextBox
        /// </summary>
        private string GetRichTextbox(RichTextBox textbox)
        {
            TextRange textRange = new TextRange(
                textbox.Document.ContentStart,
                textbox.Document.ContentEnd
            );

            return textRange.Text;
        }

        /// <summary>
        /// giver tekst til en RichTextBox
        /// </summary>
        private void SetRichTextbox(RichTextBox textbox, string text)
        {
            textbox.Document.Blocks.Clear();
            textbox.AppendText(text.Trim());

        }


        #endregion Functions

        #region auto gemt Change event

        /// <summary>
        /// sæt auto gemt tid
        /// </summary>
        private void SetSaveTimer(int minutes)
        {
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMinutes(minutes);

            _timer.Tick += _timer_Save;
            _timer.Start();
        }
        
        /// <summary>
        /// timer function
        /// gemmer filen
        /// </summary>
        private void _timer_Save(object sender, EventArgs e)
        {
            if (!this.saved)
            {
                SaveFile();
            }

        }

        /// <summary>
        /// gør så hvis der sker ændre i elementer
        /// vil det kunne ses
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
        
        /// <summary>
        /// sæt filen som ikke gemt
        /// </summary>
        private void Field_Change(object sender, EventArgs e)
        {
            if (!this.isLoading)
            {
                UpdateSaveStatus(false);
            }

        }


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

        #endregion

        class listview
        {
            public string name { get; set; }
        }
    }
}
