namespace ShadowrunTracker.ViewModels
{
    public interface IDisposableModalViewModel<TResult> : IModalViewModel
    {
        IObservable<TResult> Complete { get; }
    }
}
