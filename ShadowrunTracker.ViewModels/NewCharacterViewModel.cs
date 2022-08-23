namespace ShadowrunTracker.ViewModels
{
    using ReactiveUI;
    using ShadowrunTracker.Model;
    using ShadowrunTracker.ViewModels.Traits;
    using System;
    using System.Collections.Generic;

    public class NewCharacterViewModel : ReusableModalViewModelBase<ICharacterViewModel?>, INewCharacterViewModel
    {
        private static IReadOnlyCollection<IInitiativeScoreViewModel> DefaultInitiatives()
        {
            return new List<InitiativeScoreViewModel>
            {
                new InitiativeScoreViewModel
                {
                    State = InitiativeState.Physical,
                    Score = 2,
                    Dice = 1,
                },
                new InitiativeScoreViewModel
                {
                    State = InitiativeState.Astral,
                    Score = 2,
                    Dice = 2,
                },
                new InitiativeScoreViewModel
                {
                    State = InitiativeState.MatrixAR,
                    Score = 2,
                    Dice = 1,
                },
                new InitiativeScoreViewModel
                {
                    State = InitiativeState.MatrixCold,
                    Score = 2,
                    Dice = 3,
                },
                new InitiativeScoreViewModel
                {
                    State = InitiativeState.MatrixHot,
                    Score = 2,
                    Dice = 4,
                }
            };
        }

        private readonly IViewModelFactory _viewModelFactory;

        private ICharacterViewModel? m_Character;
        public ICharacterViewModel? Character
        {
            get => m_Character;
            set => this.RaiseAndSetIfChanged(ref m_Character, value);
        }

        public IReadOnlyCollection<IInitiativeScoreViewModel> Initiatives { get; private set; } = DefaultInitiatives();

        public NewCharacterViewModel(IViewModelFactory viewModelFactory)
        {
            _viewModelFactory = viewModelFactory;
            OnReset();
        }

        protected override void OnStart()
        {
            Character = _viewModelFactory.Create<ICharacterViewModel>();
        }

        protected override ICharacterViewModel? OkResult()
        {
            SetInitiatives();
            return Character;
        }

        protected override ICharacterViewModel? CancelResult()
        {
            if (Character != null)
            {
                _viewModelFactory.Release(Character);
            }
            return default;
        }

        protected override void OnReset()
        {
            this.RaisePropertyChanging(nameof(Initiatives));
            Initiatives = DefaultInitiatives();
            this.RaisePropertyChanged(nameof(Initiatives));
        }

        private void SetInitiatives()
        {
            if (Character is null)
            {
                throw new ArgumentNullException(nameof(Character));
            }
            foreach (var init in Initiatives)
            {
                switch (init.State)
                {
                    case InitiativeState.Physical:
                        Character.Improve(nameof(Character.PhysicalInitiativeDice), c => c.PhysicalInitiativeDice, init.Dice);
                        Character.Improve(nameof(Character.PhysicalInitiative), c => c.PhysicalInitiative, init.Score);
                        break;
                    case InitiativeState.Astral:
                        Character.Improve(nameof(Character.AstralInitiativeDice), c => c.AstralInitiativeDice, init.Dice);
                        Character.Improve(nameof(Character.AstralInitiative), c => c.AstralInitiative, init.Score);
                        break;
                    case InitiativeState.MatrixAR:
                        Character.Improve(nameof(Character.MatrixARInitiativeDice), c => c.MatrixARInitiativeDice, init.Dice);
                        Character.Improve(nameof(Character.MatrixARInitiative), c => c.MatrixARInitiative, init.Score);
                        break;
                    case InitiativeState.MatrixCold:
                        Character.Improve(nameof(Character.MatrixColdInitiativeDice), c => c.MatrixColdInitiativeDice, init.Dice);
                        Character.Improve(nameof(Character.MatrixColdInitiative), c => c.MatrixColdInitiative, init.Score);
                        break;
                    case InitiativeState.MatrixHot:
                        Character.Improve(nameof(Character.MatrixHotInitiativeDice), c => c.MatrixHotInitiativeDice, init.Dice);
                        Character.Improve(nameof(Character.MatrixHotInitiative), c => c.MatrixHotInitiative, init.Score);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
