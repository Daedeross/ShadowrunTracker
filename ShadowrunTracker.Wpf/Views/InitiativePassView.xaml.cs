#nullable disable

using ReactiveUI;
using ShadowrunTracker.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace ShadowrunTracker.Wpf.Views
{
    /// <summary>
    /// Interaction logic for InitiativePassView.xaml
    /// </summary>
    public partial class InitiativePassView : ReactiveUserControl<IInitiativePassViewModel>
    {
        public InitiativePassView()
        {
            InitializeComponent();

            DataContext = ViewModel;

            this.WhenActivated(d =>
            {
                this.DataContext = ViewModel;
            });
        }

        private void DrawerHost_DrawerClosing(object sender, MaterialDesignThemes.Wpf.DrawerClosingEventArgs e)
        {
            switch (e.Dock)
            {
                case Dock.Left:
                    break;
                case Dock.Top:
                    break;
                case Dock.Right:
                    if (DataContext is IInitiativePassViewModel vm)
                    {
                        vm.RightFlyoutContext?.Cancel();
                    }
                    break;
                case Dock.Bottom:
                    break;
                default:
                    break;
            }
        }

        private void Track_MouseMove(object sender, MouseEventArgs e)
        {
            if (sender is ItemsControl itemsControl)
            {
                foreach (var item in itemsControl.Items)
                {
                    if (item is IHoverable hoverable)
                    {
                        var container = (UIElement)itemsControl.ItemContainerGenerator.ContainerFromItem(item);
                        if (container.IsMouseOver)
                        {
                            hoverable.IsHovered = true;
                        }
                        else
                        {
                            hoverable.IsHovered = false;
                        }
                    }
                }
            }
        }

        private void Track_MouseLeave(object sender, MouseEventArgs e)
        {
            if (sender is ItemsControl itemsControl)
            {
                foreach (var item in itemsControl.Items)
                {
                    if (item is IHoverable hoverable)
                    {
                        hoverable.IsHovered = false;
                    }
                }
            }
        }
    }
}
