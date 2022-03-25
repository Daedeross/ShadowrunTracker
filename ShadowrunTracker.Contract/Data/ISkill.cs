using ShadowrunTracker.Contract.Model;

namespace ShadowrunTracker.Contract.Data
{
    public interface ISkill : ILeveledTrait
    {
        SR5Attribute LinkedAttribute { get; set; }
    }
}
