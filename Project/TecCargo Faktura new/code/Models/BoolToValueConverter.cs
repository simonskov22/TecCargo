using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace TecCargo_Faktura.Models
{
    public class BoolToVisibilityConverter : BoolToValueConverter<Visibility> { }
    public class BoolToIntConverter : BoolToValueConverter<int> { }
    public class BoolToBrushConverter : BoolToValueConverter<Brush> { }
    public class MultiBoolToVisibilityConverter : BoolToMultiValueConverter<Visibility> { }

    public class BoolToValueConverter<T> : IValueConverter
    {
        public T FalseValue { get; set; }
        public T TrueValue { get; set; }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return FalseValue;
            else
                return (bool)value ? TrueValue : FalseValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value != null ? value.Equals(TrueValue) : false;
        }
    }


    public class BoolToMultiValueConverter<T> : IMultiValueConverter
    {
        public T FalseValue { get; set; }
        public T TrueValue { get; set; }
        
        public bool UseAnd { get; set; }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            bool status = false;

            foreach (var value in values)
            {
                if (value is bool)
                {
                    if ((bool)value)
                    {
                        if (UseAnd)
                        {
                            status = true;
                            continue;
                        }
                        else {
                            return TrueValue;
                        }
                    }
                    else
                    {
                        status = false;
                    }
                }
            }
            if (UseAnd && status)
                return TrueValue;
            else
                return FalseValue;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
