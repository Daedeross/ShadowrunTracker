using System;
using System.Globalization;
using System.Windows.Data;

namespace ShadowrunTracker.Wpf.Converters
{
    [ValueConversion(typeof(int), typeof(string))]
    public class IntegerToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool showZero = parameter as bool? ?? false;
            var i = (int)value;
            if (i == 0 && showZero == false)
            {
                return string.Empty;
            }
            else
            {
                return ((int)value).ToString(culture);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string input = (string)value;
            return string.IsNullOrWhiteSpace(input)
                ? 0
                : int.Parse(input, culture);
        }
    }
}
