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
    /// Interaction logic for CheckBoxButton.xaml
    /// </summary>
    public partial class CheckBoxButton : UserControl
    {
        #region DependencyProperty

        public static readonly DependencyProperty TextSizeProperty =
            DependencyProperty.Register("TextSize", typeof(int), typeof(CheckBoxButton), new PropertyMetadata(20));
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(CheckBoxButton), new PropertyMetadata("noName"));
        public static readonly DependencyProperty CheckProperty =
            DependencyProperty.Register("Checked", typeof(bool), typeof(CheckBoxButton), new PropertyMetadata(false));
        public static readonly DependencyProperty ImageSizeProperty =
            DependencyProperty.Register("ImageSize", typeof(int), typeof(CheckBoxButton), new PropertyMetadata(35));

        public int TextSize
        {
            get { return (int)GetValue(TextSizeProperty); }
            set { SetValue(TextSizeProperty, value); }
        }
        public int ImageSize
        {
            get { return (int)GetValue(ImageSizeProperty); }
            set { SetValue(ImageSizeProperty, value); }
        }
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        public bool Checked
        {
            get { return (bool)GetValue(CheckProperty); }
            set { SetValue(CheckProperty, value); }
        }

        #endregion DependencyProperty

        public event RoutedEventHandler Click;
        public event EventHandler Changed;

        public CheckBoxButton()
        {
            InitializeComponent();
            
            _Button.Click += _Button_Click;
        }

        /// <summary>
        /// ændre check status og kald 
        /// click function hvis sat
        /// </summary>
        private void _Button_Click(object sender, RoutedEventArgs e)
        {
            this.Checked = !this.Checked;

            if (Changed != null)
                Changed(sender, null);

            if (this.Click != null)
                this.Click(this, e);

        }

        /// <summary>
        /// lav ekstra indstilinger
        /// </summary>
        private void _CheckBoxButton_Loaded(object sender, RoutedEventArgs e)
        {
            _Button.Width = Width - 20;
        }
    }

    public class CheckBoxButtonImage : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is bool)
            {
                bool IsCheck = (bool)value;

                if (IsCheck)
                {
                    return "/TecCargo Faktura System;component/Images/Icons/Check.png";
                }
                else
                {
                    return "/TecCargo Faktura System;component/Images/Icons/UnCheck.png";
                }
            }
            return "";
        }
        public object ConvertBack(object value, Type targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
