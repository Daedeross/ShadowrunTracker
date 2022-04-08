using ShadowrunTracker.Model;
using ShadowrunTracker.Data;
using ShadowrunTracker.Utils;

namespace ShadowrunTracker.ViewModels.Traits
{
    public class LeveledTraitViewModel : TraitViewModel, ILeveledTraitViewModel
    {
        public LeveledTraitViewModel(ILeveledTrait trait)
            : base(trait)
        {
            m_BaseRating = trait.Rating;
        }

        private int m_BaseRating;
        public int BaseRating
        {
            get => m_BaseRating;
            set => this.SetAndRaiseIfChanged(ref m_BaseRating, value);
        }

        private int m_BonusRating;
        public int BonusRating
        {
            get => m_BonusRating;
            set => this.SetAndRaiseIfChanged(ref m_BonusRating, value);
        }

        [DependsOn(nameof(BaseRating))]
        [DependsOn(nameof(BonusRating))]
        public int AugmentedRating => m_BaseRating + m_BonusRating;
    }
}
