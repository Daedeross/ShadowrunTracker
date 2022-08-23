namespace ShadowrunTracker.ViewModels
{
    using ShadowrunTracker.Data.Traits;
    using ShadowrunTracker.Model;

    public interface ISkillViewModel : ILeveledTraitViewModel, IRecordViewModel<Skill>
    {
        SR5Attribute LinkedAttribute { get; set; }
    }
}
