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
using System.Data;

namespace TecCargo_Faktura.Controls
{
    /// <summary>
    /// Interaction logic for Address.xaml
    /// </summary>
    public partial class Address : UserControl
    {
        #region DependencyProperty

        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register("Header", typeof(string), typeof(Address));
        public static readonly DependencyProperty TextCheckProperty =
            DependencyProperty.Register("TextCheck", typeof(string), typeof(Address));
        public static readonly DependencyProperty IsCheckProperty =
            DependencyProperty.Register("HeaderIsCheckButton", typeof(bool), typeof(Address), new PropertyMetadata(false));
        public static readonly DependencyProperty CheckedProperty =
            DependencyProperty.Register("Checked", typeof(bool), typeof(Address), new PropertyMetadata(false));
        public static readonly DependencyProperty CheckedHeaderProperty =
            DependencyProperty.Register("CheckedHeader", typeof(bool), typeof(Address), new PropertyMetadata(false));
        public static readonly DependencyProperty CheckboxVisibilityProperty =
            DependencyProperty.Register("CheckboxVisibility", typeof(Visibility), typeof(Address), new PropertyMetadata(Visibility.Visible));
        public static readonly DependencyProperty IsUnLockProperty =
            DependencyProperty.Register("IsUnLock", typeof(bool), typeof(Address), new PropertyMetadata(true));
        public static readonly DependencyProperty ShowAlwaysPlaceholderOnTopProperty =
            DependencyProperty.Register("ShowAlwaysPlaceholderOnTop", typeof(bool), typeof(Address), new PropertyMetadata(false, PropertyChangedCallback));

        public bool IsUnLock
        {
            get { return (bool)GetValue(IsUnLockProperty); }
            set { SetValue(IsUnLockProperty, value); }
        }
        public Visibility CheckboxVisibility
        {
            get { return (Visibility)GetValue(CheckboxVisibilityProperty); }
            set { SetValue(CheckboxVisibilityProperty, value); }
        }
        public string Header
        {
            get { return (string)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }
        public string TextCheck
        {
            get { return (string)GetValue(TextCheckProperty); }
            set { SetValue(TextCheckProperty, value); }
        }
        public bool HeaderIsCheckButton
        {
            get { return (bool)GetValue(IsCheckProperty); }
            set { SetValue(IsCheckProperty, value); }
        }
        public bool Checked
        {
            get { return (bool)GetValue(CheckedProperty); }
            set { SetValue(CheckedProperty, value); }
        }
        public bool CheckedHeader
        {
            get { return (bool)GetValue(CheckedHeaderProperty); }
            set { SetValue(CheckedHeaderProperty, value); }
        }
        public bool ShowAlwaysPlaceholderOnTop
        {
            get { return (bool)GetValue(ShowAlwaysPlaceholderOnTopProperty); }
            set { SetValue(ShowAlwaysPlaceholderOnTopProperty, value); }
        }


        #endregion DependencyProperty

        public event RoutedEventHandler PayClick;

        public Address()
        {
            InitializeComponent();
            
        }

        private static void PropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            Address thisObject = dependencyObject as Address;
            Thickness cityTextMargin = thisObject.TextBlock_City.Margin;
            Thickness customerListMargin = thisObject.ListBox_Customer.Margin;
            if (thisObject.ShowAlwaysPlaceholderOnTop)
            {
                cityTextMargin.Top = 45;
                customerListMargin.Top = 88;

            }
            else
            {
                cityTextMargin.Top = 0;
                customerListMargin.Top = 45;
            }

            thisObject.TextBlock_City.Margin = cityTextMargin;
            thisObject.ListBox_Customer.Margin = customerListMargin;
        }

        public void SaveCustomer()
        {
            Class.XML_Files.Customer funcCustomer = new Class.XML_Files.Customer();
            Class.XML_Files.Customer.Layout CustomerInfo = new Class.XML_Files.Customer.Layout();

            CustomerInfo.ContactTlf = Textbox_ContactTlf.Text;
            CustomerInfo.FirmName = Textbox_FirmName.Text;
            CustomerInfo.ContactPer = Textbox_ContactPer.Text;
            CustomerInfo.Address = Textbox_Address.Text;
            CustomerInfo.ZipCode = Textbox_ZipCode.Text;

            funcCustomer.SaveCustomer(CustomerInfo);
        }

        /// <summary>
        /// Hent liste med mulige kunder
        /// </summary>
        public void Textbox_GetList_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!this.IsUnLock)
            {
                ListBox_Customer.Items.Clear();
                ListBox_Customer.Visibility = Visibility.Collapsed;
                return;
            }

            var xmlClass = new Class.XML_Files.Customer();

            ListBox_Customer.Items.Clear();
            string thisMyTextbox = (((sender as TextBox).Parent as Grid).Parent as MyTextbox).Name;
            string inputText = (sender as TextBox).Text;
            var kundeDB = xmlClass.ReadCustomer();
            bool isTelefonId = Textbox_ContactTlf.Name == thisMyTextbox;
            
            //hvor listen skal være
            if (isTelefonId)
                Grid.SetRow(ListBox_Customer, 1);
            else
                Grid.SetRow(ListBox_Customer, 2);

            //tjek om der finde kunder med det man søger på
            foreach (var kunde in kundeDB)
            {
                string inputTextNoSpaceLow = inputText.Replace(" ", "").ToLower();
                string telefonNoSpaceLow = kunde.ContactTlf.Replace(" ", "").ToLower();
                string firmNameNoSpaceLow = kunde.FirmName.Replace(" ", "").ToLower();

                if ((isTelefonId && telefonNoSpaceLow.Contains(inputTextNoSpaceLow)) ||
                    (!isTelefonId && firmNameNoSpaceLow.Contains(inputTextNoSpaceLow)))
                    ListBox_Customer.Items.Add(kunde.FirmName);                
            }

            //hvis der ikke er nogle i listen, skjul listbox
            if (ListBox_Customer.Items.Count == 0)
                ListBox_Customer.Visibility = Visibility.Collapsed;
            else
                ListBox_Customer.Visibility = Visibility.Visible;

        }

        /// <summary>
        /// 
        /// </summary>
        private void ListBox_Customer_GetInfoData(object sender, EventArgs e)
        {
            SetCustomerData();
        }

        /// <summary>
        /// styr listboxen når man er i textboxen
        /// </summary>
        private void Textbox_Contact_KeyDown(object sender, KeyEventArgs e)
        {
            int currentID = ListBox_Customer.SelectedIndex;
            int scrollID = currentID;
            
            if (e.Key == Key.Down) //gå ned
            {
                currentID++;
                scrollID = currentID + 1;
            }
            else if (e.Key == Key.Up) //gå op
            {
                currentID--;
                scrollID = currentID - 1;
            }
            else if (e.Key == Key.Enter) //hent valgte info
            {
                SetCustomerData();
                return;
            }
            else if (e.Key == Key.Escape) //skjul listbox
            {
                ListBox_Customer.Visibility = Visibility.Collapsed;
                return;
            }
            else return;

            //man skal ikke scroll ud af scopet
            if (scrollID == ListBox_Customer.Items.Count)
                scrollID--;
            else if (scrollID == -1)
                scrollID++;
            
            //scroll til valgte
            if (currentID < ListBox_Customer.Items.Count && currentID > -1)
            {
                ListBox_Customer.SelectedIndex = currentID;
                ListBox_Customer.ScrollIntoView(ListBox_Customer.Items[scrollID]);
            }
        }

        /// <summary>
        /// hent kunde information udfra 
        /// hvad man har valgt i listbox
        /// </summary>
        private void SetCustomerData()
        {
            int index = ListBox_Customer.SelectedIndex;
            int itemCount = ListBox_Customer.Items.Count;
            string selectedName = ListBox_Customer.SelectedValue.ToString();

            //tjek at index ikke ligger uden for scope
            if (index < 0 || index >= itemCount)
                return;


            var xmlClass = new Class.XML_Files.Customer();
            var kundeDB = xmlClass.ReadCustomer(); //hent all kunder i databasen

            //hent kunde index udfra firma navn
            int kundeId = kundeDB.FindIndex(p => p.FirmName == selectedName);
            if (kundeId == -1)
                return;

            //sæt kunde information
            Textbox_ContactTlf.Text = kundeDB[kundeId].ContactTlf;
            Textbox_FirmName.Text = kundeDB[kundeId].FirmName;
            Textbox_ContactPer.Text = kundeDB[kundeId].ContactPer;
            Textbox_Address.Text = kundeDB[kundeId].Address;
            Textbox_ZipCode.Text = kundeDB[kundeId].ZipCode;

            //skjul listbox
            ListBox_Customer.Visibility = Visibility.Collapsed;
        }
        
        /// <summary>
        /// Hent by navn
        /// </summary>
        private void Textbox_ZipCode_KeyUp(object sender, KeyEventArgs e)
        {
            SetCityName();
        }

        /// <summary>
        /// Hent by navn ud fra hvad der
        /// er skrevet i Textbox_ZipCode
        /// </summary>
        private void SetCityName()
        {
            //post nummeret skal være på 4 tal
            if (Textbox_ZipCode.Text.Length != 4)
            {
                TextBlock_City.Text = "";
                return;
            }

            var funcClass = new Class.Functions.others();

            //hent by navn
            string cityName = funcClass.GetCityFormZip(Textbox_ZipCode.Text);
            TextBlock_City.Text = cityName;
        }


        /// <summary>
        /// lav ekstra indstillinger
        /// </summary>
        private void _Address_Loaded(object sender, RoutedEventArgs e)
        {
            MakeHeader(); //Lav header

            CheckBox_Pay.Click += this.PayClick;
        }

        /// <summary>
        /// Opret header alt efter om det
        /// er en checkbox eller label
        /// </summary>
        private void MakeHeader()
        {
            //IsUnLock binding
            Binding IsEnabledBind = new Binding("IsUnLock");
            IsEnabledBind.ElementName = "_Address";
            IsEnabledBind.Mode = BindingMode.OneWay;

            if (this.HeaderIsCheckButton)
            {
                CheckBoxButton headerItem = new CheckBoxButton();
                headerItem.Text = this.Header;
                headerItem.FontWeight = FontWeights.Bold;
                headerItem.VerticalAlignment = VerticalAlignment.Top;
                headerItem.HorizontalAlignment = HorizontalAlignment.Left;
                headerItem.Height = 45;
                headerItem.Checked = CheckedHeader;
                headerItem.Click += headerItem_Click;
                headerItem.SetBinding(IsEnabledProperty, IsEnabledBind);

                Grid.SetRow(headerItem, 0);
                Grid_Contain.Children.Add(headerItem);

                headerItem_Click(headerItem, new RoutedEventArgs());
            }
            else
            {
                Label headerItem = new Label();
                headerItem.Content = this.Header;
                headerItem.FontWeight = FontWeights.Bold;
                headerItem.FontSize = 20;
                headerItem.VerticalAlignment = VerticalAlignment.Top;
                headerItem.HorizontalAlignment = HorizontalAlignment.Left;
                headerItem.Height = 45;
                headerItem.SetBinding(IsEnabledProperty, IsEnabledBind);
                Grid.SetRow(headerItem, 0);
                Grid_Contain.Children.Add(headerItem);
            }
        }

        /// <summary>
        /// visser/skjuler felter
        /// </summary>
        private void headerItem_Click(object sender, RoutedEventArgs e)
        {
            CheckedHeader = (sender as CheckBoxButton).Checked;

            Visibility visiItem = Visibility.Collapsed;

            if (CheckedHeader)
                visiItem = Visibility.Visible;

            Textbox_FirmName.Visibility = visiItem;
            Textbox_ContactTlf.Visibility = visiItem;
            Textbox_ContactPer.Visibility = visiItem;
            Textbox_Address.Visibility = visiItem;
            TextBlock_City.Visibility = visiItem;
            Textbox_ZipCode.Visibility = visiItem;
            CheckBox_Pay.Visibility = visiItem;

        }

        /// <summary>
        /// Hent by navn
        /// </summary>
        private void Textbox_ZipCode_TextChanged(object sender, TextChangedEventArgs e)
        {
            SetCityName();
        }

        private void ListBox_Customer_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (!Textbox_FirmName.IsFocused && !Textbox_ContactTlf.IsFocused && !(sender as ListBox).IsFocused)
                (sender as ListBox).Visibility = Visibility.Collapsed;
        }
    }
}
