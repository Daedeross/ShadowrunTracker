using ShadowrunTracker.Model;

namespace ShadowrunTracker.Data
{
    public interface IImprovement
    {
        string Name { get; set; }
        TraitKind TargetKind { get; set; }
        string Target { get; set; }
        int Value { get; set; }
    }
}