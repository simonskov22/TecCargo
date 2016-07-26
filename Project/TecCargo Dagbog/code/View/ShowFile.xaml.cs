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
using System.Diagnostics;
using RawPrint;

namespace TecCargo_Dagbog.View
{
    /// <summary>
    /// Interaction logic for ShowFile.xaml
    /// </summary>
    public partial class ShowFile : Window
    {
        #region Global variables

        private int logAdded = 0; //antal logs der er blivet addet

        #endregion Global variables

        public ShowFile()
        {
            InitializeComponent();

            LabelVersion_version.Content += Inc.Settings._version.ToString(); //Sæt program Version
            LabelVersion_author.Content += Inc.Settings.CreateBy; //Sæt programudvikler navne
        }

        #region Functions

        /// <summary>
        /// Henter data logs fra fil
        /// </summary>
        private void LoadFile()
        {
            //Hent class med functioner
            Model.FileClass.function funcFile = new Model.FileClass.function();

            //Sæt elev info
            Label_name.Content = Inc.Settings.fileInput.name;
            Label_cpr.Content = Inc.Settings.fileInput.cpr;

            //Sorter dagbog log efter dato
            Model.FileClass.function.sortDateArray orderLog = funcFile.DateSort(Inc.Settings.fileInput);

            //tilføj logs
            for (int i = 0; i < orderLog.dato.Count; i++)
            {

                if (orderLog.text[i] != "")
                {
                    AddLogs(orderLog.dato[i].ToShortDateString(), orderLog.text[i]);
                }
            }

            //opdater link/dokumenter liste
            for (int i = 0; i < Inc.Settings.fileInput.files.Count; i++)
            {
                UpdateLinkList(i);
            }
        }

        /// <summary>
        /// Tilføjer en log som man skal kunne læse
        /// </summary>
        /// <param name="dato">Dato`en</param>
        /// <param name="textlog">Log tekst</param>
        private void AddLogs(string dato, string textlog)
        {
            //opretter en grid som vil indeholde log elementer
            Grid gridBox = new Grid();
            gridBox.Background = Brushes.LightGray;

            //sæt rækker på grid
            for (int i = 0; i < 3; i++)
            {
                RowDefinition newRow = new RowDefinition();
                newRow.Height = GridLength.Auto;
                gridBox.RowDefinitions.Add(newRow);
            }
            //start grid række id
            int RowIndex = gridBox.RowDefinitions.Count - 3;


            //Sæt dato`en
            Label dateLabel_Text = new Label();
            dateLabel_Text.Content = dato;
            dateLabel_Text.FontWeight = FontWeights.Bold;
            dateLabel_Text.Margin = new Thickness(5, 10, 0, 0);
            dateLabel_Text.VerticalAlignment = VerticalAlignment.Top;
            dateLabel_Text.HorizontalAlignment = HorizontalAlignment.Left;

            //Sæt log tekst
            AccessText accessFreeText = new AccessText();
            accessFreeText.TextWrapping = TextWrapping.WrapWithOverflow;
            accessFreeText.Text = textlog;

            Label freeTextLabel = new Label();
            freeTextLabel.Content = accessFreeText;
            freeTextLabel.Width = 790;
            freeTextLabel.MaxWidth = 790;
            freeTextLabel.MinHeight = 30;
            freeTextLabel.Margin = new Thickness(5, 0, 5, 5);
            freeTextLabel.FontWeight = FontWeights.Normal;
            freeTextLabel.VerticalAlignment = VerticalAlignment.Top;
            freeTextLabel.HorizontalAlignment = HorizontalAlignment.Left;
            freeTextLabel.Background = Brushes.White;

            //gør så links/dokumenter kommer til at
            //stå lige ved siden af hinanden
            WrapPanel LinkBoxWrapPanel = new WrapPanel();
            LinkBoxWrapPanel.Name = "WrapPanel_AddFile_" + logAdded;
            LinkBoxWrapPanel.Margin = new Thickness(10);
            LinkBoxWrapPanel.MaxWidth = 790;
            LinkBoxWrapPanel.VerticalAlignment = VerticalAlignment.Top;
            LinkBoxWrapPanel.HorizontalAlignment = HorizontalAlignment.Left;
            this.RegisterName("WrapPanel_AddFile_" + logAdded, LinkBoxWrapPanel);

            //sæt elemter på de rigtige rækker i grid
            Grid.SetRow(dateLabel_Text, RowIndex);
            Grid.SetRow(freeTextLabel, RowIndex + 1);
            Grid.SetRow(LinkBoxWrapPanel, RowIndex + 2);

            gridBox.Children.Add(dateLabel_Text);
            gridBox.Children.Add(freeTextLabel);
            gridBox.Children.Add(LinkBoxWrapPanel);

            //lav border på gridbox
            Border borderBox = new Border();
            borderBox.BorderThickness = new Thickness(1, 1, 1, 1);
            borderBox.Margin = new Thickness(0, 10, 0, 0);
            borderBox.BorderBrush = Brushes.Black;
            borderBox.Child = gridBox;

            //tilføj gridbox til den store grid
            Grid_FreeText.RowDefinitions.Add(new RowDefinition());

            int gridRows = Grid_FreeText.RowDefinitions.Count - 1;
            Grid.SetRow(borderBox, gridRows);
            Grid_FreeText.Children.Add(borderBox);

            logAdded++;
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
            foreach (var item in Inc.Settings.fileInput.files[Index])
            {

                TextBlock newContent = new TextBlock();
                newContent.Text = item.name;
                newContent.TextDecorations = TextDecorations.Underline;


                Button newLink = new Button();
                newLink.Content = newContent;
                newLink.Name = "Button_OpenLink_" + Index + "_" + fileIndex;
                newLink.Padding = new Thickness(0);
                newLink.Margin = new Thickness(0);
                newLink.BorderThickness = new Thickness(0);
                newLink.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF2C72B8"));
                newLink.Background = Brushes.Transparent;
                newLink.Click += newLink_Click;


                if (wrapPanelObject.Children.Count != 0)
                {
                    Label labelComma = new Label();
                    labelComma.Content = ", ";
                    labelComma.Padding = new Thickness(0);
                    labelComma.Margin = new Thickness(0);
                    wrapPanelObject.Children.Add(labelComma);
                }

                wrapPanelObject.Children.Add(newLink);
                fileIndex++;
            }
        }


        #endregion Functions

        #region Events
        //load log file
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadFile();
        }
        #endregion Events


        #region Button Click Events

        private void Button_ToolStart_Click(object sender, RoutedEventArgs e)
        {
            View.SelectType selectType = new View.SelectType();
            selectType.IsDialog = true;
            selectType.showContinueButton = true;
            selectType.ShowInTaskbar = false;
            selectType.Owner = this;
            selectType.ShowDialog();
            
            if (selectType.DialogResult.HasValue && selectType.DialogResult.Value)
            {
                this.Close();
            }
        }
        private void Button_ToolPrint_Click(object sender, RoutedEventArgs e)
        {
            View.PrinterOptions selectPrinter = new PrinterOptions();
            selectPrinter.Owner = this;
            selectPrinter.ShowDialog();
        }

        //åbener link/dokument
        private void newLink_Click(object sender, RoutedEventArgs e)
        {
            Button thisButton = (sender as Button);

            string buttonName = thisButton.Name.Replace("Button_OpenLink_", "");
            int scoreId = buttonName.IndexOf("_");
            int FieldIndex = int.Parse(buttonName.Substring(0, scoreId));
            int FileIndex = int.Parse(buttonName.Substring(scoreId + 1));


            Model.FileClass.Links selectFil = Inc.Settings.fileInput.files[FieldIndex][FileIndex];
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

        #endregion Button Click Events
    }
}
