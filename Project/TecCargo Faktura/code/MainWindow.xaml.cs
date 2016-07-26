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
namespace TecCargo_Faktura
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        #region Global Variables

        Class.Functions.Windows FuncWin = new Class.Functions.Windows();//function class til at åben vinduer

        #endregion Global Variables

        public MainWindow()
        {
            InitializeComponent();


            updateFolderSettings();

            labelVersion_version.Content += Models.ImportantData.PVersion.ToString();

            //tjek om vigtige mapper findes
            //hvis de ikke gør opret dem
            string[] folders = { 
                Models.ImportantData.g_FolderPdf, 
                Models.ImportantData.g_FolderSave,
                Models.ImportantData.g_FolderData
            };

            for (int i = 0; i < folders.Count(); i++)
            {
                if (!Directory.Exists(folders[i]))
                {
                    DirectoryInfo di = Directory.CreateDirectory(folders[i]);
                    di.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
                }
            }
        }

        #region Functions

        /// <summary>
        /// tjek om den folder der er valgt i Settings.xml
        /// findes og hent næste invoice nummer
        /// </summary>
        private void updateFolderSettings()
        {
            //hent og læs indstilinger filen
            string settingsFileName = Models.ImportantData.g_FolderDB + "Settings.xml";
            FileStream settingsFile = new FileStream(settingsFileName, FileMode.Open, FileAccess.Read);
            DataSet settingsData = new DataSet();
            settingsData.ReadXml(settingsFile);
            settingsFile.Close();

            //hent harddisk bogstav
            string hddLetter = System.IO.Path.GetPathRoot(Directory.GetCurrentDirectory());
            string newHddPath = settingsData.Tables[0].Rows[0]["SavePath"].ToString();
            bool useDefaultP = bool.Parse(settingsData.Tables[0].Rows[0]["UseDefault"].ToString());
            bool newPathFound = true;
            string newHddLetterChange = hddLetter + newHddPath.Substring(3);

            //Hvis der er lavet en ny sti som findes skal skift om til den 
            //ellers skal den bruge default
            if (Directory.Exists(newHddPath) && newHddPath != "Zone")//Prøv den der står i filen
            {
                Models.ImportantData.g_FolderPdf = newHddPath + @"\Pdf\";
                Models.ImportantData.g_FolderSave = newHddPath + @"\Gemte filer\";
                Models.ImportantData.g_FolderData = newHddPath + @"\Data\";
            }
            else if (Directory.Exists(newHddLetterChange) && newHddPath != "Zone")//Prøv med det bogstav programmet ligger i
            {
                Models.ImportantData.g_FolderPdf = newHddLetterChange + @"\Pdf\";
                Models.ImportantData.g_FolderSave = newHddLetterChange + @"\Gemte filer\";
                Models.ImportantData.g_FolderData = newHddLetterChange + @"\Data\";
	        }
            else
            {
                Models.ImportantData.g_FolderPdf = Directory.GetCurrentDirectory() + @"\Pdf\";
                Models.ImportantData.g_FolderSave = Directory.GetCurrentDirectory() + @"\Gemte filer\";
                Models.ImportantData.g_FolderData = Directory.GetCurrentDirectory() + @"\Data\";

                settingsData.Tables[0].Rows[0]["UseDefault"] = "True";
                FileStream settingsFileW = new FileStream(settingsFileName, FileMode.Create, FileAccess.Write);
                settingsData.WriteXml(settingsFileW);
                settingsFileW.Close();

                newPathFound = false;
            }

            #region Opdater Sti

            //Updater data til den nye sti
            if (useDefaultP && newPathFound)
            {
                string defaultPDF = Directory.GetCurrentDirectory() + @"\Pdf\";
                string defaultSave = Directory.GetCurrentDirectory() + @"\Gemte filer\";

                //flyt filer (save og pdf)
                string error = "Kunne ikke flytte:\n";
                bool errorFound = false;

                string[] saveFileN_Names = Directory.GetFiles(Models.ImportantData.g_FolderSave).Select(id => System.IO.Path.GetFileName(id)).ToArray();
                string[] saveFileN = Directory.GetFiles(Models.ImportantData.g_FolderSave);
                string[] saveFileD = Directory.GetFiles(defaultSave);
                int saveFilesCount = saveFileD.Length;
                for (int i = 0; i < saveFilesCount; i++)
                {
                    string fileNameD = System.IO.Path.GetFileName(saveFileD[i]);
                    
                    if (!saveFileN_Names.Contains(fileNameD))
                    {
                        File.Move(saveFileD[i], Models.ImportantData.g_FolderSave + fileNameD);
                    }
                    else
                    {
                        error += saveFileD[i] + "\n";
                        errorFound = true;
                    }
                    
                    
                }

                string[] pdfFileN_Names = Directory.GetFiles(Models.ImportantData.g_FolderSave).Select(id => System.IO.Path.GetFileName(id)).ToArray();
                string[] pdfFileN = Directory.GetFiles(Models.ImportantData.g_FolderPdf);
                string[] pdfFileD = Directory.GetFiles(defaultPDF);
                int pdfFilesCount = pdfFileD.Length;
                for (int i = 0; i < pdfFilesCount; i++)
                {
                    string fileNameD = System.IO.Path.GetFileName(pdfFileD[i]);

                    if (!pdfFileN_Names.Contains(fileNameD))
                    {
                        File.Move(pdfFileD[i], Models.ImportantData.g_FolderPdf + fileNameD);
                    }
                    else
                    {
                        error += pdfFileD[i] + "\n";
                        errorFound = true;
                    }
                }

                if (errorFound)
                {
                    MessageBox.Show(error);
                }
            }
            #endregion

            #region Next Invoice
                                    
            string[] fragtNames =   Directory.GetFiles(Models.ImportantData.g_FolderSave).Select(item => System.IO.Path.GetFileName(item)).ToArray();
            int nextInvoice = 1000;
            int.TryParse(settingsData.Tables[0].Rows[0]["NextInvoice"].ToString(), out nextInvoice);

            for (int i = 0; i < fragtNames.Count(); i++)
            {
                if (fragtNames[i].StartsWith("Fragtbrev-"))
                {

                    int invoice = 0;
                    int.TryParse(fragtNames[i].Substring(10, (fragtNames[i].Length - 14)), out invoice);

                    if (invoice >= nextInvoice)
                    {
                        nextInvoice = invoice +1;
                    }
                }
            }

            //Gemt det nye invoice tal
            settingsData.Tables[0].Rows[0]["NextInvoice"] = nextInvoice;
            FileStream settingsFileInvoiceW = new FileStream(settingsFileName, FileMode.Create, FileAccess.Write);
            settingsData.WriteXml(settingsFileInvoiceW);
            settingsFileInvoiceW.Close();

            #endregion
        }

        #endregion Functions

        #region Button Click Events

        /// <summary>
        /// opret nyt fragtbrev
        /// </summary>
        private void newFagt_click(object sender, RoutedEventArgs e)
        {
            Models.ImportantData.closeFragtbrevBool = false;
            Models.ImportantData.closeFragtbrevText = "";
            FuncWin.NewFragtbrev(this);
        }

        /// <summary>
        /// åben fragtbrev pdf mappe
        /// </summary>
        private void Button_click_openFragtmappe(object sender, RoutedEventArgs e)
        {
            FuncWin.OpenPDF(this, "Åben Fragtbrev PDF", 2);
        }

        /// <summary>
        /// Rediger fragtbrev
        /// </summary>
        private void editFragt_click(object sender, RoutedEventArgs e)
        {
            FuncWin.EditFile(this, "Åben Fragtbrev", 1);
        }

        /// <summary>
        /// opret tom pdf version af fragtbrev
        /// </summary>
        private void Button_Click_FragtbrevPrint(object sender, RoutedEventArgs e)
        {
            //så det med sikker hed er den nyeste version der bliver printet
            var fragtBrevInfo = new Class.PdfCreate.FragtBrevLayout();

            fragtBrevInfo.SaveTo = Models.ImportantData.g_FolderDB;
            fragtBrevInfo.pdfName = "Fragtbrev-Skabelon";
            fragtBrevInfo.emptyPDF = true;
            fragtBrevInfo.fragtNumber = "0000";

            //fragtBrevInfo.Afsender.ankomst = "1:00";
            //fragtBrevInfo.Afsender.afgang = "2:00";

            //fragtBrevInfo.modtager.ankomst = "4:37";
            //fragtBrevInfo.modtager.afgang = "8:25";

            try
            {
                //tjek om filen findes
                string filname = Directory.GetCurrentDirectory() + @"\database\Fragtbrev-Skabelon.pdf";

                if (File.Exists(filname))
                {
                    Class.Functions.others funcOthers = new Class.Functions.others();
                    if (!funcOthers.IsFileLocked(new FileInfo(filname)))
                    {
                        //slet gamle pdf
                        File.Delete(filname);
                    }
                }
                Class.PdfCreate.FragtBrev createFragt = new Class.PdfCreate.FragtBrev();
                createFragt.SaveFragtBrev(fragtBrevInfo);
            }
            catch (Exception)
            {
                //MessageBox.Show(error.Message);
            }

            try
            {
                System.Diagnostics.Process.Start(Models.ImportantData.g_FolderDB + "Fragtbrev-Skabelon.pdf");
            }
            catch (Exception error)
            {
                MessageBox.Show("ERROR:\n" + error.Message);
            }


            /*
            //udskriv til printer


            System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo();
            info.Verb = "print";
            info.FileName = Directory.GetCurrentDirectory() + Models.ImportantData.FolderNameForDatabase + @"\Fragtbrev-Skabelon.pdf";
            info.CreateNoWindow = true;
            info.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;

            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo = info;
            p.Start();

            p.WaitForInputIdle();
            System.Threading.Thread.Sleep(3000);
            if (false == p.CloseMainWindow())
                p.Kill();
             * */
        }

        /// <summary>
        /// åben login til admin kontrolpanel
        /// hvis login er rigtigt åben kontrolpanel
        /// </summary>
        private void ToolsbarButton_Admin_Click(object sender, RoutedEventArgs e)
        {
            WindowsView.Adminlogin adminLogin = new WindowsView.Adminlogin();
            adminLogin.ShowDialog();

            if (adminLogin.DialogResult.HasValue && adminLogin.DialogResult.Value)
            {
                string UserName = adminLogin.TextboxAdmin_Username.Text,
                    UserPass = adminLogin.TextboxAdmin_Password.Password;

                if (UserName == Models.ImportantData.AdminUsername &&
                    UserPass == Models.ImportantData.AdminPassword)
                {
                    WindowsView.AdminPanel adminPanel = new WindowsView.AdminPanel();
                    adminPanel.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Kan ikke logge ind.");
                }

            }

        }

        /// <summary>
        /// opret faktura
        /// </summary>
        private void newFaktura_click(object sender, RoutedEventArgs e)
        {
            FuncWin.NewFaktura(this);
        }

        /// <summary>
        /// rediger faktura
        /// </summary>
        private void EditFaktura_click(object sender, RoutedEventArgs e)
        {
            FuncWin.EditFile(this, "Åben Faktura", 4, false);
        }

        /// <summary>
        /// åben faktura pdf mappe
        /// </summary>
        private void Button_click_openFakturamappe(object sender, RoutedEventArgs e)
        {
            FuncWin.OpenPDF(this, "Åben Faktura PDF", 5);
        }

        /// <summary>
        /// afslut fragtbrev
        /// </summary>
        private void Button_Click_CloseFragtbrev(object sender, RoutedEventArgs e)
        {
            FuncWin.CloseFragtbrev(this);
        }

        #endregion Button Click Events
    }
}
