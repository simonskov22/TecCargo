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
using Microsoft.Win32;
using System.IO;

namespace TecCargo_Dagbog.View
{
    /// <summary>
    /// Interaction logic for AddFile.xaml
    /// </summary>
    public partial class AddFile : Window
    {

        #region Global Functions

        public Model.FileClass.Links linkInput = new Model.FileClass.Links();//Fil class

        #endregion Global Functions

        public AddFile()
        {
            InitializeComponent();
        }

        #region Button Click Events

        private void Button_AddFile_Click(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog 
            OpenFileDialog dialog = new OpenFileDialog();


            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dialog.ShowDialog();


            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 

                labelFile.Content = dialog.FileName;

            }
        }

        //tilføj link/dokument
        private void Button_Add_Click(object sender, RoutedEventArgs e)
        {
            bool error = false;
            string message = ""; //Fejl besked

            if (textboxName.Text.Length == 0)
            {
                error = true;
                message += "Navn ikke angivet.\n";
            }

            if (radioFil.IsChecked.HasValue && radioFil.IsChecked.Value)
            {
                if (labelFile.Content.ToString().Length == 0)
                {
                    error = true;
                    message += "Fil ikke angivet.\n";
                }
                else if (!File.Exists(labelFile.Content.ToString()))
                {
                    error = true;
                    message += "Filen kunne ikke findes.\n";
                }

                if (!error)
                {

                    string fileDir = "Files\\"; //mappe som filen bliver gemt i
                    string sourceFileName = labelFile.Content.ToString();//filplacering
                    string sourceOnlyNameAndExt = System.IO.Path.GetFileName(sourceFileName);//Fil navn med .ext
                    string sourceOnlyName = System.IO.Path.GetFileNameWithoutExtension(sourceFileName); //Filnavn uden .ext

                    string fileExt = sourceOnlyNameAndExt.Substring(sourceOnlyName.Length);//fil type/.ext

                    //vær sikker på at fil navnet ikke findes i forvejen
                    List<string> files = Directory.GetFiles(fileDir).ToList();
                    int fileIndex = files.Count + 1;

                    string destFilename = "File-" + fileIndex + fileExt;

                    foreach (var item in files)
                    {
                        destFilename = "File-" + fileIndex + fileExt;
                        if (!File.Exists(fileDir + destFilename))
                        {
                            break;
                        }
                        else
                        {
                            fileIndex++;
                        }
                    }

                    //flyt filen til fil mappe
                    File.Copy(sourceFileName, fileDir + destFilename);
                    linkInput.path = destFilename;
                }
            }
            else
            {
                if (textboxUrl.Text.Length == 0)
                {
                    error = true;
                    message += "URL ikke angivet.\n";
                }
                else
                {
                    linkInput.path = textboxUrl.Text;
                    linkInput.isLink = true;
                }
            }

            if (error)
            {
                MessageBox.Show(message);
            }
            else
            {
                linkInput.name = textboxName.Text;
                this.DialogResult = true;
            }
        }

        private void Button_Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        #endregion Button Click Events

        #region Events

        //Om det er en URL eller fil
        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (this.IsLoaded)
            {
                if ((sender as RadioButton).Name == "radioFil" && (sender as RadioButton).IsChecked.HasValue &&
                    (sender as RadioButton).IsChecked.Value)
                {
                    buttonFile.Visibility = Visibility.Visible;
                    labelFile.Visibility = Visibility.Visible;
                    labelUrl.Visibility = Visibility.Collapsed;
                    textboxUrl.Visibility = Visibility.Collapsed;
                }
                else
                {
                    buttonFile.Visibility = Visibility.Collapsed;
                    labelFile.Visibility = Visibility.Collapsed;
                    labelUrl.Visibility = Visibility.Visible;
                    textboxUrl.Visibility = Visibility.Visible;
                }
            }
        }

        #endregion Events

    }
}
