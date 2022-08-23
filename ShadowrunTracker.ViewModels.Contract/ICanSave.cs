namespace ShadowrunTracker.ViewModels
{
    using System.Windows.Input;

    public interface ICanSave : IViewModel
    {
        public bool IsChanged { get; set; }

        ICommand SaveCommand { get; }

        public void Save();
    }
}
