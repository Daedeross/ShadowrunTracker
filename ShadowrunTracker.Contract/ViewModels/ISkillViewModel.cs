using ShadowrunTracker.Contract.Model;

namespace ShadowrunTracker.Contract.ViewModels
{
    public interface ISkillViewModel : ILeveledTraitViewModel
    {
        SR5Attribute LinkedAttribute { get; set; }
    }
}
