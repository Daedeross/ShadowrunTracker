using ReactiveUI;
using ShadowrunTracker.Contract.Data;
using ShadowrunTracker.Contract.Model;
using ShadowrunTracker.Contract.ViewModels;

namespace ShadowrunTracker.ViewModels.Traits
{
    public class SkillViewModel : LeveledTraitViewModel, ILeveledTraitViewModel
    {
        public SkillViewModel(ISkill skill)
            : base(skill)
        {
            _linkedAttribute = skill.LinkedAttribute;
        }

        private SR5Attribute _linkedAttribute;
        public SR5Attribute LinkedAttribute
        {
            get => _linkedAttribute;
            set => this.RaiseAndSetIfChanged(ref _linkedAttribute, value);
        }
    }
}
