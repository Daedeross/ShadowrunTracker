using ShadowrunTracker.Contract.Model;

namespace ShadowrunTracker.Contract.Data
{
    public interface IImprovement
    {
        string Name { get; set; }
        TraitKind TargetKind { get; set; }
        string Target { get; set; }
        int Value { get; set; }
    }
}