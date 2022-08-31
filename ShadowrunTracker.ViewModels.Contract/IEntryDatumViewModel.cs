namespace ShadowrunTracker.ViewModels
{
    using ReactiveUI.Validation.Abstractions;

    public interface IEntryDatumViewModel : IViewModel, IValidatableViewModel
    {
        string Name { get; }
        string Text { get; set; }
        object? Value { get; }
        bool IsValid { get; }
    }
}
