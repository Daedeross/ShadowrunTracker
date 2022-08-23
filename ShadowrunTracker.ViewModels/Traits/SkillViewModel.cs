namespace ShadowrunTracker.ViewModels.Traits
{
    using ReactiveUI;
    using ShadowrunTracker;
    using ShadowrunTracker.Data.Traits;
    using ShadowrunTracker.Model;
    using System;

    public class SkillViewModel : LeveledTraitViewModel, ISkillViewModel
    {
        public SkillViewModel(Skill skill)
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

        public Skill ToRecord()
        {
            return this.ToModel();
        }

        public void Update(Skill record)
        {
            base.Update(record);
            LinkedAttribute = record.LinkedAttribute;
        }
    }
}
