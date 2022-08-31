namespace ShadowrunTracker.ViewModels
{
    using System.Collections.Generic;
    using System.Windows.Input;

    public interface IListSelectViewModel : IDisposableModalViewModel<string?>
    {
        public string Header { get; set; }

        public ICollection<string> List { get; }

        string? SelectedItem { get; set; }

        ICommand Refresh { get; }
    }
}
