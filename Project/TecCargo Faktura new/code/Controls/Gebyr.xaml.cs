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

namespace TecCargo_Faktura.Controls
{
    /// <summary>
    /// Interaction logic for Gebyr.xaml
    /// </summary>
    public partial class Gebyr : UserControl
    {
        public static readonly DependencyProperty ShowAlwaysPlaceholderOnTopProperty =
              DependencyProperty.Register("ShowAlwaysPlaceholderOnTop", typeof(bool), typeof(Gebyr), new PropertyMetadata(false));

        public bool ShowAlwaysPlaceholderOnTop
        {
            get { return (bool)GetValue(ShowAlwaysPlaceholderOnTopProperty); }
            set { SetValue(ShowAlwaysPlaceholderOnTopProperty, value); }
        }
        public EventHandler ValueChange;

        public Gebyr()
        {
            InitializeComponent();
        }

        private void ValueHasChanges()
        {
            if (ValueChange != null)
            {
                ValueChange(this, new EventArgs());
            }
        }

        private void CheckBoxButton_Gebyr_Click(object sender, RoutedEventArgs e)
        {
            ValueHasChanges();
        }

        private void MyTextbox_Gebyr_TextChanged(object sender, TextChangedEventArgs e)
        {
            ValueHasChanges();
        }
    }
}
