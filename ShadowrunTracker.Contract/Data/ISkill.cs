using ShadowrunTracker.Model;

namespace ShadowrunTracker.Data
{
    public interface ISkill : ILeveledTrait
    {
        SR5Attribute LinkedAttribute { get; set; }
    }
}
