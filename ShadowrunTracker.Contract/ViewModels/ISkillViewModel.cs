using ShadowrunTracker.Model;

namespace ShadowrunTracker.ViewModels
{
    public interface ISkillViewModel : ILeveledTraitViewModel
    {
        SR5Attribute LinkedAttribute { get; set; }
    }
}
