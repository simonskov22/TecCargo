using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.ComponentModel;


namespace TecCargo_Faktura.Models
{
    public class DateTimePicker : Button
    {

        public DateTimePicker()
        {
           if (DesignerProperties.GetIsInDesignMode(this))
           {
               this.Source = new BitmapImage(new Uri("Images/Icons/Close.png", UriKind.Relative));
           }
        }

        public ImageSource Source
        {
            get { return base.GetValue(SourceProperty) as ImageSource; }
            set { base.SetValue(SourceProperty, value); }
        }

        public static readonly DependencyProperty SourceProperty =
          DependencyProperty.Register("Source", typeof(ImageSource), typeof(DateTimePicker));


    }
}
