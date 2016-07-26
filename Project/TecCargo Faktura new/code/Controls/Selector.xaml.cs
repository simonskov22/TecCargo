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
    /// Interaction logic for Selector.xaml
    /// </summary>
    public partial class Selector : UserControl
    {
        #region DependencyProperty

        const int id_HideAll = 0;
        const int id_ShowAll = 1;
        const int id_ShowAllAndCheck = 2;

        public static readonly DependencyProperty ItemsProperty =
            DependencyProperty.Register("Items", typeof(List<string>), typeof(Selector));
        public static readonly DependencyProperty MaxRowProperty =
            DependencyProperty.Register("MaxRow", typeof(int), typeof(Selector), new PropertyMetadata(-1));
        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register("Header", typeof(string), typeof(Selector), new PropertyMetadata(""));

        public List<string> Items
        {
            get { return (List<string>)GetValue(ItemsProperty); }
            set
            {
                SetValue(ItemsProperty, value);
                Update();
            }
        }
        public int MaxRow
        {
            get { return (int)GetValue(MaxRowProperty); }
            set
            {
                SetValue(MaxRowProperty, value);
                Update();
            }
        }
        public string Header
        {
            get { return (string)GetValue(HeaderProperty); }
            set{ SetValue(HeaderProperty, value); }
        }
        public bool IsChecked
        {
            get
            {
                if (this.SelectId == -1)
                    return false;
                else
                    return true;
            }
        }
      



        public int SelectId {
            get
            {
                int itemCount = _ItemsContains.Children.Count;

                for (int i = 0; i < itemCount; i++)
                {
                    CheckBoxButton itemButton = _ItemsContains.Children[i] as CheckBoxButton;

                    if (itemButton.Checked)
                    {
                        return i;
                    }

                }

                return -1;
            }
            set
            {
                for (int i = 0; i < _ItemsContains.Children.Count; i++)
                {
                    CheckBoxButton itemButton = _ItemsContains.Children[i] as CheckBoxButton;

                    if (i == value)
                    {
                        itemButton.Checked = true;
                    }
                    else
                        itemButton.Checked = false;
                }

                if (value < 0 || value >= _ItemsContains.Children.Count)
                    SetSelectionStatus(id_ShowAll);
                else
                    SetSelectionStatus(id_HideAll);
            }
        }
        public string SelectName
        {
            get
            {
                int itemCount = _ItemsContains.Children.Count;

                for (int i = 0; i < itemCount; i++)
                {
                    CheckBoxButton itemButton = _ItemsContains.Children[i] as CheckBoxButton;

                    if (itemButton.Checked)
                    {
                        return itemButton.Text;
                    }

                }

                return "";
            }
        }

        #endregion DependencyProperty

        public event EventHandler SelectionChanged;
        private bool ItemsIsShow = true;

       
        public Selector()
        {
            InitializeComponent();
            this.Items = new List<string>(); //gør så den ikke henter items fra andre seletor
        }

        /// <summary>
        /// opdatere item list
        /// </summary>
        public void Update()
        {
            int oldSelectId = this.SelectId;

            //nulstil grid
            _ItemsContains.Children.Clear();
            _ItemsContains.RowDefinitions.Clear();
            _ItemsContains.ColumnDefinitions.Clear();

            int itemCount = Items.Count;
            int rowId = 0;
            int columnId = 0;

            double ColumnCount = Math.Ceiling((double)itemCount / MaxRow);

            //opret rækker
            for (int i = 0; i < MaxRow ||(MaxRow == -1 && i < itemCount); i++)
            {
                _ItemsContains.RowDefinitions.Add(new RowDefinition());
                _ItemsContains.RowDefinitions[i].Height = GridLength.Auto;
            }
            
            //opret kolonner
            for (int i = 0; i < ColumnCount; i++)
            {
                _ItemsContains.ColumnDefinitions.Add(new ColumnDefinition());
                _ItemsContains.ColumnDefinitions[i].Width = GridLength.Auto;
            }
            //opret select knapper
            for (int i = 0; i < itemCount; i++)
            {
                CheckBoxButton newItemButton = new CheckBoxButton();
                

                newItemButton.Text = Items[i];
                newItemButton.Margin = new Thickness(10, 0, 0, 0);
                newItemButton.Width = Width;
                newItemButton.Click += _newItemButton_Click;
                newItemButton.ImageSize = 25;
                newItemButton.TextSize = 15;

                if (oldSelectId == i)
                    newItemButton.Checked = true;

                if (MaxRow != -1 && rowId == MaxRow)
                {
                    rowId = 0;
                    columnId++;
                }

                Grid.SetRow(newItemButton, rowId);
                Grid.SetColumn(newItemButton, columnId);
                _ItemsContains.Children.Add(newItemButton);

                rowId++;
            }

            this.SelectId = oldSelectId;
        }

        /// <summary>
        /// når man klikker på en item/checkbox
        /// skal den hakke valgte af og
        /// skjule/visse de andre valgt
        /// </summary>
        private void _newItemButton_Click(object sender, RoutedEventArgs e)
        {
            CheckBoxButton senderItemButton = sender as CheckBoxButton;
            int itemCount = _ItemsContains.Children.Count;// hvormange checkboxbutton/items der er
            bool checkStatus = senderItemButton.Checked;

            //MessageBox.Show(senderItemButton.Text + " " + checkStatus);

            //hvis checkboxbutton er hakket af og 
            //de andre items checkboxbutton er skjul 
            //skal den vise alle items
            if (!senderItemButton.Checked && !ItemsIsShow)
            {
                senderItemButton.Checked = true; //gør så den stadig er hakket af

                SetSelectionStatus(id_ShowAllAndCheck, senderItemButton);
            }
            //skjul alle items når den er hakket af
            else if(senderItemButton.Checked)
            {
                SetSelectionStatus(id_HideAll, senderItemButton);
            }
            else
            {
                SetSelectionStatus(id_ShowAll, senderItemButton);
            }

            _SelectionChanged_Event(); //sig der er sket ændringere
        }

        /// <summary>
        /// lav ekstra indstillinger
        /// </summary>
        private void _ItemsContains_Loaded(object sender, RoutedEventArgs e)
        {
            Update();
        }

        /// <summary>
        /// kald function hvis den er sat
        /// </summary>
        private void _SelectionChanged_Event()
        {
            if (this.SelectionChanged != null)
            {
                this.SelectionChanged(this, null);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void SetSelectionStatus(int statusId, CheckBoxButton senderButton = null)
        {
            bool buttoncheckStatus = false;

            switch (statusId)
            {
                case id_ShowAllAndCheck:
                    ItemsIsShow = true;//sig at items er vis
                    break;
                default:
                    ItemsIsShow = false;//sig at items er vis
                    break;
            }

            //hvis der ikke er sat nogen knap tjek om der en der er checked
            if (senderButton == null)
            {
                foreach (CheckBoxButton item in _ItemsContains.Children)
                {
                    if (item.Checked)
                    {
                        senderButton = item;
                        break;
                    }
                }
            }

            //hvis der en knap hent check status
            if (senderButton != null)
            {
                buttoncheckStatus = senderButton.Checked;
            }
            int loopRun = _ItemsContains.Children.Count;


            //tjek om der er en der er checked
            for (int i = 0; i < _ItemsContains.Children.Count; i++)
            {
                CheckBoxButton buttonItem = _ItemsContains.Children[i] as CheckBoxButton;
                buttonItem.Checked = false;

                if (id_HideAll != statusId) 
                    buttonItem.Visibility = Visibility.Visible;
                else
                    buttonItem.Visibility = Visibility.Collapsed;
                //buttonItem.Visibility = Visibility.Collapsed;
            }
            
            //om den stadig skal være checked
            if (senderButton != null)
            {
                senderButton.Visibility = Visibility.Visible;
                senderButton.Checked = buttoncheckStatus;
            }  
        }
    }
}
