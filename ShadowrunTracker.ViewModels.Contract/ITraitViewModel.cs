namespace ShadowrunTracker.ViewModels
{
    using ShadowrunTracker.Data.Traits;

    /// <summary>
    /// Base interface for all traits other than Attributes (for now),
    /// including gear
    /// </summary>
    public interface ITraitViewModel : IViewModel, IHaveId
    {
        string Name { get; set; }

        string Description { get; set; }

        string Notes { get; set; }

        string Source { get; set; }

        int Page { get; set; }
    }
}