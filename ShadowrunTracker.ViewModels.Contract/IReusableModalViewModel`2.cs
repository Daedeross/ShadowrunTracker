namespace ShadowrunTracker.ViewModels
{
    using System.Windows.Input;

    public interface IReusableModalViewModel<TInput, TOutput> : IModalViewModel
    {
        IObservable<TOutput> Start(TInput input);
    }
}
