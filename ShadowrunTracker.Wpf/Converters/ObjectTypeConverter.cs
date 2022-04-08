using System;
using System.Globalization;
using System.Windows.Data;

namespace ShadowrunTracker.Wpf.Converters
{
    [ValueConversion(typeof(object), typeof(Type))]
    internal class ObjectTypeConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object parameter, CultureInfo culture)
        {
            return value?.GetType() ?? null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
