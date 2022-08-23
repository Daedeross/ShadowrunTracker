#nullable disable

using ReactiveUI;
using ShadowrunTracker.ViewModels;
using ShadowrunTracker.Wpf.Helpers;
using System.Reactive.Disposables;
using System.Windows.Input;

namespace ShadowrunTracker.Wpf.Views
{
    /// <summary>
    /// Interaction logic for NewParticipantView.xaml
    /// </summary>
    public partial class NewParticipantView : ReactiveUserControl<INewCharacterViewModel>
    {
        public NewParticipantView()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
                this.OneWayBind(ViewModel, vm => vm.Character, v => v.CharacterHost.ViewModel)
                    .DisposeWith(d);
            });
        }

        private void OnNumericKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            EventHelpers.OnNumericKeyboardFocus(sender, e);
        }
    }
}
