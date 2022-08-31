namespace ShadowrunTracker.ViewModels
{
    public interface IReusableModalViewModel<TOutput> : IModalViewModel
    {
        IObservable<TOutput> Start();
    }
}
