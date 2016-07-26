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
    /// Interaction logic for PrisList.xaml
    /// </summary>
    public partial class PrisList : UserControl
    {
        //public double subtotal = 0;
        public double rabatProcent = 0;

        public struct pricelistItem
        {
            public string name { get; set; }
            public string price { get; set; }
            public bool isResult { get; set; }
            public bool isHeader { get; set; }
            public bool isSpace { get; set; }
        }

        public PrisList()
        {
            InitializeComponent();
            
        }


        public List<pricelistItem> items = new List<pricelistItem>();

        public void Update()
        {
            _itemssource.Items.Clear();

            double subtotal = 0;
            double tax = 0;
            double total = 0;

            foreach (var priceItem in this.items)
            {
                double price = 0;
                double.TryParse(priceItem.price, out price);
                subtotal += price;
                
                _itemssource.Items.Add(priceItem);
            }

            tax = subtotal * 0.25;
            total = subtotal + tax;

            _itemssource.Items.Add(new pricelistItem() { name = "Subtotal", price = subtotal.ToString("N2"), isResult = true });
            _itemssource.Items.Add(new pricelistItem() { name = "Tax", price = tax.ToString("N2"), isResult = true });

            if (rabatProcent > 0)
            {
                double rabat = (rabatProcent / 100) * total;
                total -= rabat;
                _itemssource.Items.Add(new pricelistItem() { name = "Rabat ("+ rabatProcent + "%)", price = rabat.ToString("N2"), isResult = true });
            }

            _itemssource.Items.Add(new pricelistItem() { name = "Total", price = total.ToString("N2"), isResult = true });

            
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Update();
        }
    }
}
