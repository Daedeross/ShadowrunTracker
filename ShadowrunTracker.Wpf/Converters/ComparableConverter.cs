using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ShadowrunTracker.Wpf.Converters
{
    public enum Comparison
    {
        Ne = 0,
        Eq = 1,
        Lt = 2,
        Gt = 3,
        LtAnd = 4,
        GtAnd = 5,
    }

    public class ComparableConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length < 2)
            {
                return false;
            }

            if (values[0] is IComparable left && values[1] is IComparable right)
            {
                return (Comparison)parameter switch
                {
                    Comparison.Ne    => left.CompareTo(right) != 0,
                    Comparison.Eq    => left.CompareTo(right) == 0,
                    Comparison.Lt    => left.CompareTo(right) == -1,
                    Comparison.Gt    => left.CompareTo(right) == 1,
                    Comparison.LtAnd => left.CompareTo(right) != 1,
                    Comparison.GtAnd => left.CompareTo(right) != -1,
                    _ => false,
                };
            }

            return false;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
