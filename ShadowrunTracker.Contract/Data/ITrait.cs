namespace ShadowrunTracker.Contract.Data
{
    public interface ITrait
    {
        string Name { get; set; }

        string Description { get; set; }

        string Notes { get; set; }

        string Source { get; set; }

        int Page { get; set; }
    }
}
