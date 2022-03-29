using ReactiveUI;
using ShadowrunTracker.Data;
using ShadowrunTracker.Model;

namespace ShadowrunTracker.ViewModels.Traits
{
    public class SkillViewModel : LeveledTraitViewModel, ISkillViewModel
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
