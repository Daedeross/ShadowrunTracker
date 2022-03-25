using ReactiveUI;
using ShadowrunTracker.Contract.Data;
using ShadowrunTracker.Contract.ViewModels;

namespace ShadowrunTracker.ViewModels.Traits
{
    public class LeveledTraitViewModel : TraitViewModel, ILeveledTraitViewModel
    {
        public LeveledTraitViewModel(ILeveledTrait trait)
            : base(trait)
        {

        }

        private int _rating;
        public int Rating
        {
            get => _rating;
            set => this.RaiseAndSetIfChanged(ref _rating, value);
        }

        private int _bonusRating;
        public int BonusRating
        {
            get => _bonusRating;
            set => this.RaiseAndSetIfChanged(ref _bonusRating, value);
        }

        public int AugmentedRating
        {
            get => _rating + _bonusRating;
            set
            {
                int bonus = value - _rating;
                if (bonus != _bonusRating)
                {
                    _bonusRating = bonus;
                    this.RaisePropertyChanged(nameof(BonusRating));
                    this.RaisePropertyChanged(nameof(AugmentedRating));
                }
            }
        }
    }
}
