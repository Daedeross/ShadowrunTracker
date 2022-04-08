using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace ShadowrunTracker.Wpf.Converters
{
    public class IndexFromItemConverter : IMultiValueConverter
    {
        //public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        //{
        //    return ((ItemsControl)parameter).ItemContainerGenerator.IndexFromContainer((DependencyObject)value);
        //}

        //public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        //{
        //    return ((ItemsControl)parameter).ItemContainerGenerator.ContainerFromIndex((int)value);
        //}

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return ((ItemsControl)values[0]).ItemContainerGenerator.IndexFromContainer((DependencyObject)values[1]);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
