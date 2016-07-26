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
using Spire.Pdf;
using System.Windows.Xps.Packaging;
using System.Threading;

using Dialog = System.Windows.Forms;

namespace TecCargo_Faktura.Controls
{
    /// <summary>
    /// Interaction logic for MyDocumentViewer.xaml
    /// </summary>
    public partial class MyDocumentViewer : UserControl
    {

        public static readonly DependencyProperty IsLoadingProperty =
            DependencyProperty.Register("IsLoading", typeof(bool), typeof(MyDocumentViewer), new PropertyMetadata(false));

        public bool IsLoading {
            get {return (bool)GetValue(IsLoadingProperty); }
            set { SetValue(IsLoadingProperty, value); }

        }

        public bool FileIsCreated = false;

        public string Document = "";
        public event RoutedEventHandler ReloadClick;

        public MyDocumentViewer()
        {
            InitializeComponent();
        }

        public void Button_ReloadDocument_Click(object sender, RoutedEventArgs e)
        {
            if (ReloadClick != null)
                ReloadClick(sender, e);
        }
        private void Button_DownloadPdf_Click(object sender, RoutedEventArgs e)
        {
            Dialog.FolderBrowserDialog folderDialog = new Dialog.FolderBrowserDialog();
            folderDialog.Description = "Vælg den mappe som filen vil blive gemt i.";
            folderDialog.ShowDialog();
            
            //tjek at mappen findes
            if (!Directory.Exists(folderDialog.SelectedPath))
                return;

            //find pdf fil navn og kun navnet og .pdf
            string filename = "";
            for (int i = Document.Length -1; i > 0; i--)
            {
                string letter = Document.Substring(i, 1);

                if (letter == @"\" || letter == "/")
                {
                    filename = Document.Substring(i);
                    break;
                }
            }

            //kopir filen til valgte mappe
            File.Copy(Document, folderDialog.SelectedPath + filename);
        }


        public void ReloadDocument()
        {
            if (!this.FileIsCreated)
                return;

            Thread LoadFile = new Thread(() => 
            {
                string filename = this.Document;
                filename = filename.Replace(".pdf", "");
                try {
                    if (File.Exists(filename + ".xps"))
                        File.Delete(filename + ".xps");

                    PdfDocument pdfConvert = new PdfDocument();
                    pdfConvert.LoadFromFile(filename + ".pdf");
                    pdfConvert.SaveToFile(filename + ".xps", FileFormat.XPS);
                }
                catch (Exception)
                {
                    return;
                }

                XpsDocument xpsDoc = new XpsDocument(filename + ".xps", FileAccess.Read);                

                Dispatcher.InvokeAsync(() =>
                {
                    docviewer.Document = xpsDoc.GetFixedDocumentSequence();
                    IsLoading = false;
                });                                
            });

            LoadFile.Start();
        }
    }
}
