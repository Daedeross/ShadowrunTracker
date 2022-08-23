namespace ShadowrunTracker.ViewModels
{
    using System.Windows.Input;

    public interface IModalViewModel: IViewModel, ICancelable, IDisposable
    {
        ICommand OkCommand { get; }

        ICommand CancelCommand { get; }
    }
}
