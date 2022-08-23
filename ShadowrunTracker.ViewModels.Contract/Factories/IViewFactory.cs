namespace ShadowrunTracker
{
    using ReactiveUI;

    public interface IViewFactory
    {
        IViewFor<T> For<T>()
            where T : class;
    }
}
