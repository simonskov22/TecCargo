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
    /// Interaction logic for TextBlock.xaml
    /// </summary>
    public partial class MyTextBlock : UserControl
    {
        #region DependencyProperty

        public static readonly DependencyProperty TextSizeProperty =
            DependencyProperty.Register("TextSize", typeof(string), typeof(MyTextBlock), new PropertyMetadata("15"));
        public static readonly DependencyProperty PlaceTextProperty =
            DependencyProperty.Register("PlaceTextSize", typeof(string), typeof(MyTextBlock), new PropertyMetadata("20"));
        public static readonly DependencyProperty PlaceholderProperty =
            DependencyProperty.Register("Placeholder", typeof(string), typeof(MyTextBlock));
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(MyTextBlock), new PropertyMetadata(""));

        public static readonly DependencyProperty IsUnLockProperty =
            DependencyProperty.Register("IsUnLock", typeof(bool), typeof(MyTextBlock), new PropertyMetadata(true));
        public static readonly DependencyProperty ShowAlwaysPlaceholderOnTopProperty =
            DependencyProperty.Register("ShowAlwaysPlaceholderOnTop", typeof(bool), typeof(MyTextBlock), new PropertyMetadata(false, PropertyChangedCallback));


        public static readonly DependencyProperty RequireEnableRedBorderProperty =
            DependencyProperty.Register("RequireEnableRedBorder", typeof(bool), typeof(MyTextBlock), new PropertyMetadata(false));
        public static readonly DependencyProperty RequireMaxLengthProperty =
            DependencyProperty.Register("RequireMaxLength", typeof(int), typeof(MyTextBlock), new PropertyMetadata(-1));
        public static readonly DependencyProperty RequireMinimumLengthBorderProperty =
            DependencyProperty.Register("RequireMinimumLength", typeof(int), typeof(MyTextBlock), new PropertyMetadata(-1));

        private static void PropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            MyTextBlock thisTextbox = dependencyObject as MyTextBlock;
            thisTextbox.UpdateHoverSize();
            Thickness textboxMargin = thisTextbox.Margin;
            thisTextbox._hovertext.Visibility = Visibility.Visible;

            //gem margin top engang så den sider rigtig
            if (thisTextbox.marginTop == -1)
                thisTextbox.marginTop = textboxMargin.Top;

            //flyt på textboxen
            if (thisTextbox.ShowAlwaysPlaceholderOnTop)
                textboxMargin.Top = thisTextbox._hovertext.ActualHeight + thisTextbox.marginTop;
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
                textEmpty.Converter = thisTextbox.FindResource("PlaceholderConverter") as IValueConverter;

                thisTextbox.placeholder.SetBinding(VisibilityProperty, textEmpty);
                thisTextbox._hovertext.Visibility = Visibility.Collapsed;
            }
        }
        public string TextSize
        {
            get { return (string)GetValue(TextSizeProperty); }
            set { SetValue(TextSizeProperty, value); }
        }
        public string PlaceTextSize
        {
            get { return (string)GetValue(PlaceTextProperty); }
            set { SetValue(PlaceTextProperty, value); }
        }
        public string Placeholder
        {
            get { return (string)GetValue(PlaceholderProperty); }
            set { SetValue(PlaceholderProperty, value); }
        }
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
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

        #endregion DependencyProperty

        private delegate void NoArgDelegate();
        private double marginTop = -1;

        private bool textHasChange = false;
        private bool textHasLostFocus = false;

        public MyTextBlock()
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
        /// lav ekstra indstillinger
        /// </summary>
        private void _Textblock_Loaded(object sender, RoutedEventArgs e)
        {
            //double removeLengt = this.Padding.Left + this.Padding.Right;
            //_hovertext.Width -= 20;
            textboxScroll.Height -= 10;
            placeholder.Height -= 10;
            Input.MinHeight -= 10;
        }
        private void UpdateHoverSize()
        {
            //sæt til at være over tekst
            _hovertext.Visibility = Visibility.Visible;
            string hoverText = _hovertext.Content.ToString();
            _hovertext.Content = "GetHeight";
            this._hovertext.Dispatcher.Invoke(DispatcherPriority.Render, (NoArgDelegate)delegate { });
            double hoverMarginTop = (_hovertext.ActualHeight * -1)+6;
            this._hovertext.Margin = new Thickness(0, hoverMarginTop, 0, 0);
            _hovertext.Content = hoverText;
            this._hovertext.Dispatcher.Invoke(DispatcherPriority.Render, (NoArgDelegate)delegate { });

            
        }

        /// <summary>
        /// gør så hvis man laver et enter
        /// og man er på den sidste linje
        /// så skal den scroll til buden
        /// </summary>
        private void Input_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (!this.IsLoaded) return;

            int lastLineId = Input.LineCount -1;
            int currentLineId = Input.CaretIndex / 2;
                        
            if(lastLineId == currentLineId)
                textboxScroll.ScrollToBottom();
        }
        
        /// <summary>
        /// hvis header over tekst
        /// så man kan se hvad man
        /// har skrevet høre til
        /// </summary>
        private void _Textblock_MouseEnter(object sender, MouseEventArgs e)
        {
            //hover tekst skal kun visse når der er skrevet i textboxen
            if ((_Textblock.IsMouseOver && Input.Text.Length != 0 && !_hovertext.IsMouseOver) || this.ShowAlwaysPlaceholderOnTop)
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
        
    }
}
