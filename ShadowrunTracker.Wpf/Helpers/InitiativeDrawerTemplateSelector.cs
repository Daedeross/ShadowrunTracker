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
    public class InitiativeDrawerTemplateSelector : DataTemplateSelector
    {
        public DataTemplate? DamageHealingTemplate { get; set; }
        public DataTemplate? DelayActionTemplate { get; set; }

        public override DataTemplate? SelectTemplate(object item, DependencyObject container)
        {
            return item switch
            {
                IDamageHealingViewModel => DamageHealingTemplate,
                IDelayActionViewModel => DelayActionTemplate,
                _ => null
            };
        }
    }
}
