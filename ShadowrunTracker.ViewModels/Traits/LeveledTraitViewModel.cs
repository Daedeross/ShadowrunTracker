namespace ShadowrunTracker.ViewModels.Traits
{
    using ReactiveUI;
    using ShadowrunTracker.Data.Traits;
    using ShadowrunTracker.Utils;
    using System.Reactive.Disposables;

    public class LeveledTraitViewModel : TraitViewModel, ILeveledTraitViewModel
    {
        public LeveledTraitViewModel(LeveledTrait trait)
            : base(trait)
        {
            m_BaseRating = trait.BaseRating;
            m_BonusRating = trait.BonusRating;

            _augmentedRating = this.WhenAnyValue(x => x.BaseRating, x => x.BonusRating, (baseRating, bonusRating) => baseRating + bonusRating)
                .ToProperty(this, x => x.AugmentedRating)
                .DisposeWith(_disposables);
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

        private ObservableAsPropertyHelper<int> _augmentedRating;
        public int AugmentedRating => _augmentedRating.Value;

        protected void Update(LeveledTrait record)
        {
            base.Update(record);
            BaseRating = record.BaseRating;
            BonusRating = record.BonusRating;
        }
    }
}
