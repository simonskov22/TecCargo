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

namespace TecCargo_Faktura.Controls
{
    /// <summary>
    /// Interaction logic for Textbox.xaml
    /// </summary>
    public partial class MyTextbox : UserControl
    {
        #region DependencyProperty

        public static readonly DependencyProperty TextSizeProperty =
            DependencyProperty.Register("TextSize", typeof(string), typeof(MyTextbox), new PropertyMetadata("20"));
        public static readonly DependencyProperty PlaceholderProperty =
            DependencyProperty.Register("Placeholder", typeof(string), typeof(MyTextbox));
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(MyTextbox));

        public static readonly DependencyProperty IsIntProperty =
            DependencyProperty.Register("IsInt", typeof(bool), typeof(MyTextbox), new PropertyMetadata(false));
        public static readonly DependencyProperty IsDoubleProperty =
            DependencyProperty.Register("IsDouble", typeof(bool), typeof(MyTextbox), new PropertyMetadata(false));
        public static readonly DependencyProperty MaxLengthProperty =
            DependencyProperty.Register("MaxLength", typeof(int), typeof(MyTextbox), new PropertyMetadata(-1));
        public static readonly DependencyProperty IsUnLockProperty =
            DependencyProperty.Register("IsUnLock", typeof(bool), typeof(MyTextbox), new PropertyMetadata(true));
        public static readonly DependencyProperty ShowAlwaysPlaceholderOnTopProperty =
            DependencyProperty.Register("ShowAlwaysPlaceholderOnTop", typeof(bool), typeof(MyTextbox), new PropertyMetadata(false, PropertyChangedCallback));

        public static readonly DependencyProperty RequireEnableRedBorderProperty =
            DependencyProperty.Register("RequireEnableRedBorder", typeof(bool), typeof(MyTextbox), new PropertyMetadata(false));
        public static readonly DependencyProperty RequireMaxLengthProperty =
            DependencyProperty.Register("RequireMaxLength", typeof(int), typeof(MyTextbox), new PropertyMetadata(-1));
        public static readonly DependencyProperty RequireMinimumLengthBorderProperty =
            DependencyProperty.Register("RequireMinimumLength", typeof(int), typeof(MyTextbox), new PropertyMetadata(-1));
        private static void PropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            
            MyTextbox thisTextbox = dependencyObject as MyTextbox;
            thisTextbox.UpdateHoverSize();
            Thickness textboxMargin = thisTextbox.Margin;

            //gem margin top engang så den sider rigtig
            if (thisTextbox.marginTop == -1)
                thisTextbox.marginTop = textboxMargin.Top;

            //flyt på textboxen
            if (thisTextbox.ShowAlwaysPlaceholderOnTop)
                textboxMargin.Top = 37 + thisTextbox.marginTop;
            else
                textboxMargin.Top = thisTextbox.marginTop;

            
            thisTextbox.Margin = textboxMargin;

            //om de skal være synlige
            if (thisTextbox.ShowAlwaysPlaceholderOnTop)
            {
                thisTextbox.placeholder.Visibility = Visibility.Collapsed;
                thisTextbox._hovertext.Visibility = Visibility.Visible;
            }
            else
            {
                //gør så den vis og skjule placeholder alt efter om der er skrevet noget
                Binding textEmpty = new Binding("Text.IsEmpty");
                textEmpty.ElementName = "Input";
                textEmpty.Converter = thisTextbox.FindResource("ConverterPlaceholder") as IValueConverter;

                thisTextbox.placeholder.SetBinding(VisibilityProperty, textEmpty);
                thisTextbox._hovertext.Visibility = Visibility.Collapsed;
            }
        }

        public string TextSize
        {
            get { return (string)GetValue(TextSizeProperty); }
            set { SetValue(TextSizeProperty, value); }
        }
        public string Placeholder
        {
            get { return (string)GetValue(PlaceholderProperty); }
            set { SetValue(PlaceholderProperty, value); }
        }
        public string Text
        {
            get { return Input.Text; }
            set { SetValue(TextProperty, value); }
        }
        public bool IsInt
        {
            get { return (bool)GetValue(IsIntProperty); }
            set { SetValue(IsIntProperty, value); }
        }
        public bool IsDouble
        {
            get { return (bool)GetValue(IsDoubleProperty); }
            set { SetValue(IsDoubleProperty, value); }
        }
        public bool IsUnLock
        {
            get { return (bool)GetValue(IsUnLockProperty); }
            set { SetValue(IsUnLockProperty, value); }
        }
        public bool ShowAlwaysPlaceholderOnTop
        {
            get { return (bool)GetValue(ShowAlwaysPlaceholderOnTopProperty); }
            set { SetValue(ShowAlwaysPlaceholderOnTopProperty, value); }
        }

        private double marginTop = -1;
        public int MaxLength
        {
            get { return (int)GetValue(MaxLengthProperty); }
            set { SetValue(MaxLengthProperty, value); }
        }
        public bool RequireEnableRedBorder
        {
            get { return (bool)GetValue(RequireEnableRedBorderProperty); }
            set { SetValue(RequireEnableRedBorderProperty, value); }
        }
        public int RequireMaxLength
        {
            get { return (int)GetValue(RequireMaxLengthProperty); }
            set { SetValue(RequireMaxLengthProperty, value); }
        }
        public int RequireMinimumLength
        {
            get { return (int)GetValue(RequireMinimumLengthBorderProperty); }
            set { SetValue(RequireMinimumLengthBorderProperty, value); }
        }

        #endregion

        private delegate void NoArgDelegate();
        public event TextChangedEventHandler TextChanged;
        private bool textHasChange = false;
        private bool textHasLostFocus = false;
        public MyTextbox()
        {
            InitializeComponent();

        }

        private void CheckRequirement()
        {
            if ((RequireMaxLength == -1 && RequireMinimumLength == -1) || !textHasChange || !textHasLostFocus)
                return;

            if (RequireMaxLength != -1 && this.Text.Length > RequireMaxLength)
                this.RequireEnableRedBorder = true;
            else if (RequireMinimumLength != -1 && this.Text.Length < RequireMinimumLength)
                this.RequireEnableRedBorder = true;
            else
                this.RequireEnableRedBorder = false;
        }

        /// <summary>
        /// gør det mulig at lave regler 
        /// for IsInt og isDouble
        /// </summary>
        private void Input_KeyDown(object sender, KeyEventArgs e)
        {
            if (IsInt || IsDouble)
            {
                if (!((e.Key >= Key.D0 && e.Key <= Key.D9) ||
                    (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) ||
                    (IsDouble &&
                    (e.Key == Key.OemPeriod ||
                    e.Key == Key.OemComma ||
                    e.Key == Key.Decimal))))
                {
                    e.Handled = true; //tillad ikke key
                }
                else if (IsDouble && !IsInt)
                {
                    if (!(IsDouble && Input_KeyDownCommaPeriod(e.Key)))
                    {
                        e.Handled = true; //tillad ikke key
                    }
                }
            }


            if (MaxLength != -1 && (sender as TextBox).Text.Length >= MaxLength)
            {
                e.Handled = true; //tillad ikke key
            }
        }

        /// <summary>
        /// hvad man kan kunne skrive
        /// når det er en IsDouble tekstboks
        /// </summary>
        private bool Input_KeyDownCommaPeriod(Key eKey)
        {
            string inputText = Input.Text;

            int periodFound = 0; //antal punktummer
            int commaFound = 0;//antal kommaer
            int charFound = 0; //antal bogstaver efter punktum/komma

            //hent info om inputText
            for (int i = 0; i < inputText.Length; i++)
            {
                string oneLetter = inputText.Substring(i, 1);

                if (oneLetter == ".")
                {
                    periodFound++;
                    charFound = 0;
                }
                else if (oneLetter == ",")
                {
                    commaFound++;
                    charFound = 0;
                }
                else
                {
                    charFound++;
                }
            }

            if (eKey == Key.OemPeriod) //Lav regler for punktum
            {
                if (commaFound == 1)
                    return false;
                else if ((periodFound == 0 && charFound > 3))
                    return false;
                else if ((periodFound >= 1 && charFound != 3))
                    return false;
                else if (charFound > 3)
                    return false;
                else
                    return true;
            }
            else if (eKey == Key.OemComma || eKey == Key.Decimal) //lav regler for komma
            {
                if (commaFound == 1)
                    return false;
                else if (periodFound >= 1 && charFound != 3)
                    return false;
                else
                    return true;
            }


            if (commaFound == 1 && charFound == 2) //gør at man ikke kan skrive mere end 2 bogstaver
                return false;
            else if (periodFound != 0 && charFound == 3) //gør at man ikke kan skrive mere end 3 bogstaver
                return false;
            else
                return true;
        }

        /// <summary>
        /// lav ekstra indstillinger
        /// </summary>
        private void _Textbox_Loaded(object sender, RoutedEventArgs e)
        {
            //fjern padding længde fra elementer
            double removeLengt = this.Padding.Left + this.Padding.Right;
            placeholder.Width -= removeLengt;
            Input.Width -= removeLengt;
            _hovertext.Width -= removeLengt;

            //giv padding til elementer
            Thickness paddingPlace = placeholder.Padding;
            Thickness paddingInput = placeholder.Padding;

            paddingPlace.Left += 5;
            paddingPlace.Top += 2;
            paddingPlace.Bottom += 2;

            paddingInput.Top += 2;
            paddingInput.Bottom += 2;

            placeholder.Padding = paddingPlace;
            Input.Padding = paddingInput;
            

            //tilføj textchange event
            if (TextChanged != null)
                Input.TextChanged += TextChanged;

            UpdateHoverSize();
        }
        private void UpdateHoverSize()
        {
            //sæt til at være over tekst
            Visibility hoverDefault = _hovertext.Visibility;
            _hovertext.Visibility = Visibility.Visible;
            double hoverMarginTop = (37 * -1);
            this._hovertext.Margin = new Thickness(0, hoverMarginTop, 0, 0);
            _hovertext.Visibility = hoverDefault;
        }

        /// <summary>
        /// hvis header over tekst
        /// så man kan se hvad man
        /// har skrevet høre til
        /// </summary>
        private void _Textbox_MouseEnter(object sender, MouseEventArgs e)
        {
            //hover tekst skal kun visse når der er skrevet i textboxen
            if ((_Textbox.IsMouseOver && this.Text.Length != 0 && !_hovertext.IsMouseOver) || this.ShowAlwaysPlaceholderOnTop)
                _hovertext.Visibility = Visibility.Visible;
            else
               _hovertext.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Gør så når man låser tekstboksen
        /// og der er ikke skrevet i boksen
        /// så vil man stadig kunne se header
        /// tekst
        /// </summary>
        private void _Grid_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var funcClass = new Class.Functions.others();

            if ((sender as Grid).IsEnabled)
            {
                placeholder.Foreground = funcClass.ColorBrushHex("#FF5489BD");
                placeholder.Background = funcClass.ColorBrushHex("#FFE2E0E0");
                Canvas.SetZIndex(placeholder, 0);
            }
            else
            {
                placeholder.Foreground = funcClass.ColorBrushHex("#FF88AED4");
                placeholder.Background = funcClass.ColorBrushHex("#FFEEEBEB");
                Canvas.SetZIndex(placeholder, 100);
            }
        }

        private void placeholder_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (!this.ShowAlwaysPlaceholderOnTop || (sender as TextBlock).Visibility == Visibility.Collapsed)
                return;
            else
                (sender as TextBlock).Visibility = Visibility.Collapsed;
        }

        private void Input_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.textHasChange = true;
            CheckRequirement();
        }

        private void Input_LostFocus(object sender, RoutedEventArgs e)
        {
            this.textHasLostFocus = true;
            CheckRequirement();
        }

        private void _Textbox_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this.ShowAlwaysPlaceholderOnTop)
            {
                PropertyChangedCallback(this, new DependencyPropertyChangedEventArgs());
                _hovertext.Visibility = Visibility.Visible;
            }
        }
    }
}
