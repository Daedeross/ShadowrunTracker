namespace ShadowrunTracker.ViewModels.Traits
{
    using ReactiveUI;
    using ShadowrunTracker.Data.Traits;
    using ShadowrunTracker.Utils;
    using System.Collections.Generic;
    using System.Reactive.Disposables;

    public class LeveledTraitViewModel : TraitViewModel, ILeveledTraitViewModel
    {
        protected static readonly ISet<string> _leveledTraitRecordProperties;

        static LeveledTraitViewModel()
        {
            _leveledTraitRecordProperties = new HashSet<string>
            {
                nameof(BaseRating),
                nameof(BonusRating),
            };
            _leveledTraitRecordProperties.UnionWith(_traitRecordProperties);
        }

        public LeveledTraitViewModel(LeveledTrait record)
            : base(record)
        {
            m_BaseRating = record.BaseRating;
            m_BonusRating = record.BonusRating;

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
            try
            {
                PushUpdate = false;
                DoUpdate(record);
            }
            finally
            {
                PushUpdate = true;
            }
        }

        protected void DoUpdate(LeveledTrait record)
        {
            base.DoUpdate(record);
            BaseRating = record.BaseRating;
            BonusRating = record.BonusRating;
        }
    }
}
