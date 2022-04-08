using ShadowrunTracker.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ShadowrunTracker.Wpf.Helpers
{
    public class DialogTemplateSelector : DataTemplateSelector
    {
        public DataTemplate? ConfirmTemplate { get; set; }
        public DataTemplate? HostTemplate { get; set; }

        public override DataTemplate? SelectTemplate(object item, DependencyObject container)
        {
            return item switch
            {
                string => ConfirmTemplate,
                IViewModel => HostTemplate,
                _ => null
            };
        }
    }
}
