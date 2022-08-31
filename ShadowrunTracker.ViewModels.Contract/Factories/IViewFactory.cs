namespace ShadowrunTracker
{
    using ReactiveUI;

    public interface IViewFactory
    {
        IViewFor<T> For<T>(string? viewName)
            where T : class;
    }
}
