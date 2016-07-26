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
    /// Interaction logic for FileName.xaml
    /// </summary>
    public partial class FileName : Window
    {
        #region Global Variables

        private List<fileClass.FileInfoClass> filesInUse = new List<fileClass.FileInfoClass>(); //
        private fileClass.FileInfoSettings fileSetup = new fileClass.FileInfoSettings();        //

        #endregion Global Variables


        /// <summary>
        /// her bliver det sat hvilken fil type der bliver åbnet
        /// </summary>
        /// <param name="FileType">
        /// 1 = Rediger fragtbrev
        /// 2 = Åben fragtbrev i pdf version
        /// 3 = Lav en faktura
        /// 4 = Rediger faktura
        /// 5 = Åben faktura i pdf version
        /// </param>
        public FileName(int FileType)
        {
            InitializeComponent();

            switch (FileType)
            {
                case 1:
                    this.fileSetup.showFragtXML = true;
                    break;
                case 2:
                    this.fileSetup.showFragtPDF = true;
                    break;
                case 3:
                    this.fileSetup.showFragtClosed = true;
                    break;
                case 4:
                    this.fileSetup.showFaktXML = true;
                    break;
                case 5:
                    this.fileSetup.showFaktPDF = true;
                    break;
            }


            loadListView();
            /*

            //this.Title = _Title;

            //(ListViewOpenFileNames.View as GridView).Columns[2] = "12";
            GridView listHeaderText = new GridView();

            GridViewColumn gvc1 = new GridViewColumn();
            gvc1.DisplayMemberBinding = new Binding("Filnavn");
            gvc1.Header = "Filnavn";
            gvc1.Width = 200;
            listHeaderText.Columns.Add(gvc1);

            GridViewColumn gvc2 = new GridViewColumn();
            gvc2.DisplayMemberBinding = new Binding("EditDate");
            gvc2.Header = "Ændringsdato";
            gvc2.Width = 100;
            listHeaderText.Columns.Add(gvc2);

            //om der skal vise at der er lavet en pdf version
            //edit fragt / edit faktura
            if (FileType == 1 || FileType == 4)
            {
                GridViewColumn gvc3 = new GridViewColumn();
                gvc3.DisplayMemberBinding = new Binding("IsPdf");
                gvc3.Header = "PDF Version";
                gvc3.Width = 70;
                listHeaderText.Columns.Add(gvc3);
            }

            GridViewColumn gvc4 = new GridViewColumn();
            gvc4.DisplayMemberBinding = new Binding("creatorId");
            gvc4.Header = "Initialer";
            //gvc4.Width = 100;
            listHeaderText.Columns.Add(gvc4);

            ListViewOpenFileNames.View = listHeaderText;

            switch (FileType)
            {
                case 1: //Rediger fragtbrev
                    AllFragtSaveFiles();
                    break;
                case 2: //Åben PDF fragtbrev
                    AllFragtPdfFiles("Fragtbrev-");
                    break;
                case 3: //Ny faktura
                    NewFakturaFiles();
                    break;
                case 4: //Rediger faktura
                    AllFakturaSaveFiles();
                    break;
                case 5: //Åben PDF Faktura
                    AllFragtPdfFiles("Faktura-");
                    break;
                default:
                    break;
            }
            */
        }

        #region Functions

        /// <summary>
        /// hent filerne og vis dem i listen
        /// </summary>
        private void loadListView()
        {
            List<fileClass.FileInfoClass> allFiles = new fileClass().GetFileList();

            fileClass.FileInfoSettings setup = this.fileSetup;

            foreach (var file in allFiles)
            {
                if ((setup.showFragtXML) ||
                    (file.hasFragtPDF && setup.showFragtPDF) ||
                    (file.hasFaktura && setup.showFaktXML) ||
                    (file.hasFaktPDF && setup.showFaktPDF) ||
                    (file.fragtClosed && setup.showFragtClosed))
                {
                    this.filesInUse.Add(file);

                    ListViewOpenFileNames.Items.Add(SetUpListViewItem(file));

                }
            }
        }

        /// <summary>
        /// hent oplyninger om en fil f.eks er der lavet pdf
        /// </summary>
        private filenames SetUpListViewItem(fileClass.FileInfoClass file)
        {
            #region start
            DateTime lastWriteDate = new DateTime();

            if (this.fileSetup.showFragtXML || this.fileSetup.showFragtClosed)
            {
                lastWriteDate = file.dateXML_Fragt;
            }
            else if (this.fileSetup.showFragtPDF)
            {
                lastWriteDate = file.datePDF_Fragt;
            }
            else if (this.fileSetup.showFaktXML)
            {
                lastWriteDate = file.dateXML_Fakt;
            }
            else if (this.fileSetup.showFaktPDF)
            {
                lastWriteDate = file.datePDF_Fakt;
            }

            #endregion

            string fileName = "";
            string creators = "";
            if (this.fileSetup.showFragtXML || this.fileSetup.showFragtPDF || this.fileSetup.showFragtClosed)
            {
                fileName = "Fragtbrev-" + file.fragtNumb;
                creators = file.creatorsFragt;
            }
            else
            {
                fileName = "Faktura-" + file.fragtNumb;
                creators = file.creatorsFakt;
            }

            filenames newListViewItem = new filenames();
            newListViewItem.Filnavn = fileName;
            newListViewItem.EditDate = lastWriteDate.ToShortDateString();
            newListViewItem.creatorId = creators;
            newListViewItem.statusFragt = file.statusFragt;
            newListViewItem.statusFakt = file.statusFakt;

            return newListViewItem;
        }

        #endregion Functions

        #region Events

        /// <summary>
        /// åben valgte fil
        /// </summary>
        private void ButtonFileName_save_Click(object sender, RoutedEventArgs e)
        {
            if (TextBoxFileName_name.Content.ToString() != "" && TextBoxFileName_name.Content.ToString() != null)
            {
                this.DialogResult = true;
            }
        }

        /// <summary>
        /// når man vælger en fil
        /// hent fil navn til label
        /// så man kan åben den
        /// </summary>
        private void ListViewOpenFileNames_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //id på den valgte fil
            int selectId = ListViewOpenFileNames.SelectedIndex;

            //skriv fil navnet i textbox
            if (selectId >= 0 && selectId < ListViewOpenFileNames.Items.Count)
            {
                TextBoxFileName_name.Content = (ListViewOpenFileNames.Items[selectId] as filenames).Filnavn;

                //gør så man kan klikke på åben
                ButtonFileName_save.IsEnabled = true;
            }
            else
            {
                TextBoxFileName_name.Content = "";
                ButtonFileName_save.IsEnabled = false;
            }
        }

        /// <summary>
        /// ved at dobbeltklikke på en fil vil den åben den
        /// </summary>
        private void ListViewOpenFileNames_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (TextBoxFileName_name.Content.ToString() != "" && TextBoxFileName_name.Content.ToString() != null)
            {
                this.DialogResult = true;
            }
            else
            {
                //MessageBox.Show("Failed: " + TextBoxFileName_name.ContentStringFormat);
            }
        }

        /// <summary>
        /// gør det muligt at søge efter invoice nr.
        /// så man kan finde filen hurtigere
        /// </summary>
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string allowInput = "1234567890";
            string returnValue = "";

            for (int i = 0; i < (sender as TextBox).Text.Length; i++)
            {
                if (allowInput.Contains((sender as TextBox).Text.Substring(i, 1)))
                {
                    returnValue += (sender as TextBox).Text.Substring(i, 1);
                }
            }
            (sender as TextBox).Text = returnValue;


            ListViewOpenFileNames.SelectedIndex = -1;
            ListViewOpenFileNames.Items.Clear();

            fileClass.FileInfoSettings setup = this.fileSetup;

            foreach (var file in this.filesInUse)
            {
                if (file.fragtNumb.StartsWith(returnValue))
                {
                    ListViewOpenFileNames.Items.Add(SetUpListViewItem(file));
                }
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            windowTitle.Content = this.Title;
        }

        #endregion Events


        /*
        private string GetCreatorsIds(string filename) 
        {
            string folder = Models.ImportantData.g_FolderSave;
            FileStream documentRead;
            try
            {
                documentRead = new FileStream(folder + filename + ".xml", FileMode.Open, FileAccess.Read);
            }
            catch (Exception)
            {
                return "None";
            }
            
            DataSet ReadXML = new DataSet();
            ReadXML.ReadXml(documentRead);
            documentRead.Close();
            string allCratorIds = "";
            //Initialer
            try 
	        {	        
		        for (int i = 0; i < ReadXML.Tables["Initialer"].Rows.Count; i++)
                {
                    string creatorIdS = ReadXML.Tables["Initialer"].Rows[i].ItemArray[0].ToString();

                    allCratorIds += creatorIdS + ", ";
                }
	        }
	        catch (Exception)
	        {
	        }
            

            if (allCratorIds.Length > 2)
            {
                return allCratorIds.Substring(0,allCratorIds.Length-2);
            }
            else
            {
                return "None";
            }
        }

        private void AllFakturaSaveFiles()
        {

            string folder = Models.ImportantData.g_FolderSave;
            string folderPdf = Models.ImportantData.g_FolderPdf;

            //henter alle navne på de fragtbreve der er blevet gemt
            string[] fileNames = Directory.GetFiles(folder).Select(path => System.IO.Path.GetFileName(path)).ToArray();
            string[] lastEditTime = Directory.GetFiles(folder).Select(path => File.GetLastWriteTime(path).ToShortDateString()).ToArray();
            int fileCount = fileNames.Count();

            for (int i = 0; i < fileCount; i++)
            {
                //filen skal inde holde standart navn
                if (fileNames[i].StartsWith("Faktura-"))
                {
                    int number;

                    //så der kun vil være nummeret tilbage
                    int fileNameLent = fileNames[i].Length - 12;

                    bool isANumber = int.TryParse(fileNames[i].Substring(8, fileNameLent), out number);

                    //tjekker om det er et tal og om det er højre end startnumber
                    if (isANumber)
                    {
                        //tjek om den er lavet til pdf
                        string isPdfVer = "None";
                        if (File.Exists(folderPdf + "Faktura-" + number.ToString() + ".pdf"))
                        {
                            isPdfVer = "Ja";
                        }
                        else
                        {
                            isPdfVer = "Nej";
                        }
                        string theFilename = fileNames[i].Substring(0, fileNames[i].Length - 4);
                        //tilføj til list
                        ListViewOpenFileNames.Items.Add(new filenames { 
                            Filnavn = theFilename,
                            EditDate = lastEditTime[i],
                            IsPdf = isPdfVer,
                            creatorId = GetCreatorsIds(theFilename)
                        });
                    }
                }
            }
        }

        private void NewFakturaFiles()
        {

            string folder = Models.ImportantData.g_FolderSave;

            //henter alle navne på de fragtbreve der er blevet gemt
            string[] fileNames = Directory.GetFiles(folder).Select(path => System.IO.Path.GetFileName(path)).ToArray();
            string[] lastEditTime = Directory.GetFiles(folder).Select(path => File.GetLastWriteTime(path).ToShortDateString()).ToArray();
            int fileCount = fileNames.Count();

            for (int i = 0; i < fileCount; i++)
            {
                //filen skal inde holde standart navn
                if (fileNames[i].StartsWith("Fragtbrev-"))
                {
                    //MessageBox.Show(folder + fileNames[i]);
                    FileStream documentRead = new FileStream(folder + fileNames[i], FileMode.Open, FileAccess.Read);

                    DataSet ReadXML = new DataSet();

                    ReadXML.ReadXml(documentRead);
                    documentRead.Close();
                    bool CanCreateFaktura = bool.Parse(ReadXML.Tables["FileDone"].Rows[0].ItemArray[0].ToString());
                    if (CanCreateFaktura)
                    {
                        string theFilename = fileNames[i].Substring(0, fileNames[i].Length - 4);
                        ListViewOpenFileNames.Items.Add(new filenames { 
                            Filnavn = theFilename,
                            EditDate = lastEditTime[i],
                            creatorId = GetCreatorsIds(theFilename)
                        });
                    }
                }
            }

        }

        private void AllFragtPdfFiles(string fileStartsWith)
        {
            string folder = Models.ImportantData.g_FolderPdf;

            //henter alle navne på de fragtbreve der er blevet gemt
            string[] fileNames = Directory.GetFiles(folder).Select(path => System.IO.Path.GetFileName(path)).ToArray();
            string[] lastEditTime = Directory.GetFiles(folder).Select(path => File.GetLastWriteTime(path).ToShortDateString()).ToArray();
            int fileCount = fileNames.Count();

            for (int i = 0; i < fileCount; i++)
            {
                //filen skal inde holde standart navn
                if (fileNames[i].StartsWith(fileStartsWith))
                {
                    string theFilename = fileNames[i].Substring(0, fileNames[i].Length - 4);

                    ListViewOpenFileNames.Items.Add(new filenames {
                        Filnavn = theFilename, 
                        EditDate = lastEditTime[i],
                        creatorId = GetCreatorsIds(theFilename)
                    });
                }
            }

        }

        private void AllFragtSaveFiles()
        {

            string folder = Models.ImportantData.g_FolderSave;
            string folderPdf = Models.ImportantData.g_FolderPdf;
            
            //henter alle navne på de fragtbreve der er blevet gemt
            string[] fileNames = Directory.GetFiles(folder).Select(path => System.IO.Path.GetFileName(path)).ToArray();
            string[] lastEditTime = Directory.GetFiles(folder).Select(path => File.GetLastWriteTime(path).ToShortDateString()).ToArray();
            int fileCount = fileNames.Count();

            for (int i = 0; i < fileCount; i++)
            {
                //filen skal inde holde standart navn
                if (fileNames[i].StartsWith("Fragtbrev-"))
                {
                    int number;

                    //så der kun vil være nummeret tilbage
                    int fileNameLent = fileNames[i].Length - 14;

                    bool isANumber = int.TryParse(fileNames[i].Substring(10, fileNameLent), out number);

                    //tjekker om det er et tal og om det er højre end startnumber
                    if (isANumber)
                    {
                        //tjek om den er lavet til pdf
                        string ispdfS = "None";
                        if (File.Exists(folderPdf + "Fragtbrev-" + number.ToString() + ".pdf"))
                        {
                            ispdfS = "Ja";
                        }
                        else
                        {
                            ispdfS = "Nej";
                        }
                        string theFilename = fileNames[i].Substring(0, fileNames[i].Length -4);

                        ListViewOpenFileNames.Items.Add(new filenames {
                            Filnavn = theFilename, 
                            EditDate = lastEditTime[i],
                            IsPdf = ispdfS,                           
                            creatorId = GetCreatorsIds(theFilename)
                        });
                    }
                }
            }
        }
        */
    }

    class fileClass
    {
        public class FileInfoSettings
	    {
            private bool p_showFragtXML = false;
            private bool p_showFragtPDF = false;
            private bool p_showFragtClosed = false;
		    private bool p_showFaktXML = false;
		    private bool p_showFaktPDF = false;


            public bool showFragtXML { get { return p_showFragtXML; } set { p_showFragtXML = value; } }
            public bool showFragtPDF { get { return p_showFragtPDF; } set { p_showFragtPDF = value; } }
            public bool showFragtClosed { get { return p_showFragtClosed; } set { p_showFragtClosed = value; } }
		    public bool showFaktXML { get {return p_showFaktXML;} set {p_showFaktXML = value;} }
		    public bool showFaktPDF { get {return p_showFaktPDF;} set {p_showFaktPDF = value;} }

	    }

        public class FileInfoClass
        {
            public string fragtNumb { get; set; }
            public bool hasFaktura { get; set; }
            public bool hasFragtPDF { get; set; }
            public bool hasFaktPDF { get; set; }
            public bool fragtClosed { get; set; }
            public DateTime dateXML_Fragt { get; set; }
            public DateTime datePDF_Fragt { get; set; }
            public DateTime dateXML_Fakt { get; set; }
            public DateTime datePDF_Fakt { get; set; }
            public string creatorsFragt { get; set; }
            public string creatorsFakt { get; set; }
            public string statusFragt { get; set; }
            public string statusFakt { get; set; }
        }


        public List<FileInfoClass> GetFileList()
        {
            Class.XML_Files.Fragtbrev funcFragt = new Class.XML_Files.Fragtbrev();
            Class.XML_Files.Faktura funcFakt = new Class.XML_Files.Faktura();

            List<FileInfoClass> allFileList = new List<FileInfoClass>();

            string FolderPDF = Models.ImportantData.g_FolderPdf;
            string FolderXML = Models.ImportantData.g_FolderSave;

            //hent fil info
            List<string> fileNamesPdf = Directory.GetFiles(FolderPDF).Select(path => System.IO.Path.GetFileName(path)).ToList();
            List<string> fileNamesSave = Directory.GetFiles(FolderXML).Select(path => System.IO.Path.GetFileName(path)).ToList();
            List<DateTime> writeDatePDF = Directory.GetFiles(FolderPDF).Select(path => File.GetLastWriteTime(path)).ToList();
            List<DateTime> writeDateXML = Directory.GetFiles(FolderXML).Select(path => File.GetLastWriteTime(path)).ToList();

            int fileCount = fileNamesSave.Count();

            for (int i = 0; i < fileCount; i++)
            {
                if (fileNamesSave[i].Contains("Fragtbrev-"))
	            {
                    //find fraktbrev nummer
                    string fragtNr = "";
		            fragtNr = fileNamesSave[i].Replace("Fragtbrev-","");
		            fragtNr = fragtNr.Replace(".xml","");

                    //om der er lavet en faktura version
                    bool hasFaktura = fileNamesSave.Contains("Faktura-"+fragtNr+".xml");
                    bool hasFragtPDF = fileNamesPdf.Contains("Fragtbrev-"+fragtNr+".pdf");
                    bool hasFaktPDF = fileNamesPdf.Contains("Faktura-"+fragtNr+".pdf");

                    //hvem der oprettet/ændret filen
                    var fragtbrevRead = funcFragt.ReadFile("Fragtbrev-" + fragtNr);
                    string statusFragt = "";
                    string statusFakt = "Ikke oprrettet.";

                    if (fragtbrevRead.Close.IsClosed) {
                        statusFragt = "Afsluttet.";
                    }
                    else if (hasFragtPDF)
                    {
                        statusFragt = "Gemt som PDF.";
                    }
                    else
                    {
                        statusFragt = "Oprettet.";
                    }

                    string fragtCreators = "";
                    string faktCreators = "";

                    foreach (var creator in fragtbrevRead.Owners)
	                {
		                fragtCreators += creator + ", ";
	                }
                    fragtCreators = fragtCreators.Substring(0,fragtCreators.Length-2);

                    if(hasFaktura){
                        var fakturaRead = funcFakt.ReadFile("Faktura-"+fragtNr);

                        if (fakturaRead.IsClosed)
                        {
                            statusFakt = "Afsluttet.";
                        }
                        else if (hasFaktPDF)
                        {
                            statusFakt = "Gemt som PDF.";
                        }
                        else
                        {
                            statusFakt = "Oprettet.";
                        }

                        foreach (var creator in fakturaRead.Owners)
	                    {
		                    faktCreators += creator + ", ";
	                    }
                        faktCreators = faktCreators.Substring(0,faktCreators.Length-2);
                    }

                    //hent sidste ændrings dato
                    DateTime editDateFragtXML = writeDateXML[i];
                    DateTime editDateFaktXML = new DateTime();
                    DateTime editDateFragtPDF = new DateTime();
                    DateTime editDateFaktPDF = new DateTime();

                    if(hasFragtPDF)
                    {
                        int index = fileNamesPdf.IndexOf("Fragtbrev-"+fragtNr+".pdf");
                        editDateFragtPDF = writeDatePDF[index];
                    }
                    if(hasFaktura)
                    {
                        int index = fileNamesSave.IndexOf("Faktura-"+fragtNr+".xml");
                        editDateFaktXML = writeDateXML[index];
                    }
                    if(hasFaktPDF)
                    {
                        int index = fileNamesPdf.IndexOf("Faktura-"+fragtNr+".pdf");
                        editDateFaktPDF = writeDatePDF[index];
                    }

                    //
                    FileInfoClass newFileInfo = new FileInfoClass();
                    newFileInfo.fragtNumb = fragtNr;
                    newFileInfo.hasFragtPDF = hasFragtPDF;
                    newFileInfo.fragtClosed = fragtbrevRead.Close.IsClosed;
                    newFileInfo.hasFaktura = hasFaktura;
                    newFileInfo.hasFaktPDF = hasFaktPDF;
                    newFileInfo.creatorsFragt = fragtCreators;
                    newFileInfo.creatorsFakt = faktCreators;
                    newFileInfo.dateXML_Fragt = editDateFragtXML;
                    newFileInfo.dateXML_Fakt = editDateFaktXML;
                    newFileInfo.datePDF_Fragt = editDateFragtPDF;
                    newFileInfo.datePDF_Fakt = editDateFaktPDF;
                    newFileInfo.statusFragt = statusFragt;
                    newFileInfo.statusFakt = statusFakt;

                    allFileList.Add(newFileInfo);
	            }
            }

            //Send liste tilbage
            return allFileList;
        }
    }

    class filenames
    {
        public string Filnavn { get; set; }
        public string EditDate { get; set; }
        public string IsPdf { get; set; }
        public string creatorId { get; set; }
        public string statusFragt { get; set; }
        public string statusFakt { get; set; }
    }
}
