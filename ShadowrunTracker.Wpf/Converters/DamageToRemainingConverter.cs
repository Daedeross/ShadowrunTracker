using System;
using System.Globalization;
using System.Windows.Data;

namespace ShadowrunTracker.Wpf.Converters
{
    internal class DamageToRemainingConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            int damage = (int)values[0];
            int max = (int)values[1];
            int remaining = max - damage;
            if (parameter is string format)
            {
                return string.Format(format, damage, max, remaining);
            }
            else
            {
                return $"{remaining} / {max}";
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
