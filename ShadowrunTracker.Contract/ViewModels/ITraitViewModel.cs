namespace ShadowrunTracker.Contract.ViewModels
{
    /// <summary>
    /// Base interface for all traits other than Attributes (for now),
    /// including gear
    /// </summary>
    public interface ITraitViewModel : IViewModel
    {
        string Name { get; set; }

        string Description { get; set; }

        string Notes { get; set; }

        string Source { get; set; }

        int Page { get; set; }
    }
}