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

namespace TecCargo_Faktura.WindowsView
{
    /// <summary>
    /// Interaction logic for CloseWindowText.xaml
    /// </summary>
    public partial class CloseWindowText : Window
    {
        public string returnText = "";

        /// <summary>
        /// om den skal starte med en tekst
        /// </summary>
        /// <param name="text">start tekst</param>
        /// <param name="name">fragtbrev/faktura</param>
        public CloseWindowText(string text, string name)
        {
            InitializeComponent();

            contentText.Content += name;

            FlowDocument fDoneDocment = new FlowDocument();

            Paragraph filedoneText = new Paragraph();
            filedoneText.Inlines.Add(new Run(text));

            fDoneDocment.Blocks.Add(filedoneText);

            finishTextbox.Document = fDoneDocment;
        }

        /// <summary>
        /// gem kommentar
        /// </summary>
        private void SaveFinishTextButton_Click(object sender, RoutedEventArgs e)
        {
            this.returnText = new TextRange(finishTextbox.Document.ContentStart, finishTextbox.Document.ContentEnd).Text;
            this.DialogResult = true;
        }
    }
}
