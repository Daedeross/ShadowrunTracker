namespace ShadowrunTracker.ViewModels
{
    using ReactiveUI;
    using ShadowrunTracker.Data;
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

        public NewCharacterViewModel(IViewModelFactory viewModelFactory)
        {
            _viewModelFactory = viewModelFactory;
            OnReset();
        }

        #region Character Base Properties

        private string m_Alias = string.Empty;
        public string Alias
        {
            get => m_Alias;
            set => this.RaiseAndSetIfChanged(ref m_Alias, value);
        }

        private bool m_IsPlayer;
        public bool IsPlayer
        {
            get => m_IsPlayer;
            set => this.RaiseAndSetIfChanged(ref m_IsPlayer, value);
        }

        private string m_Player = string.Empty;
        public string Player
        {
            get => m_Player;
            set => this.RaiseAndSetIfChanged(ref m_Player, value);
        }

        private int m_BaseEdge;
        public int BaseEdge
        {
            get => m_BaseEdge;
            set => this.RaiseAndSetIfChanged(ref m_BaseEdge, value);
        }

        private int m_BaseBody;
        public int BaseBody
        {
            get => m_BaseBody;
            set => this.RaiseAndSetIfChanged(ref m_BaseBody, value);
        }

        private int m_BaseAgility;
        public int BaseAgility
        {
            get => m_BaseAgility;
            set => this.RaiseAndSetIfChanged(ref m_BaseAgility, value);
        }

        private int m_BaseReaction;
        public int BaseReaction
        {
            get => m_BaseReaction;
            set => this.RaiseAndSetIfChanged(ref m_BaseReaction, value);
        }

        private int m_BaseStrength;
        public int BaseStrength
        {
            get => m_BaseStrength;
            set => this.RaiseAndSetIfChanged(ref m_BaseStrength, value);
        }

        private int m_BaseCharisma;
        public int BaseCharisma
        {
            get => m_BaseCharisma;
            set => this.RaiseAndSetIfChanged(ref m_BaseCharisma, value);
        }

        private int m_BaseIntuition;
        public int BaseIntuition
        {
            get => m_BaseIntuition;
            set => this.RaiseAndSetIfChanged(ref m_BaseIntuition, value);
        }

        private int m_BaseLogic;
        public int BaseLogic
        {
            get => m_BaseLogic;
            set => this.RaiseAndSetIfChanged(ref m_BaseLogic, value);
        }

        private int m_BaseWillpower;
        public int BaseWillpower
        {
            get => m_BaseWillpower;
            set => this.RaiseAndSetIfChanged(ref m_BaseWillpower, value);
        }

        private bool m_PainEditor;
        public bool PainEditor
        {
            get => m_PainEditor;
            set => this.RaiseAndSetIfChanged(ref m_PainEditor, value);
        }

        private int m_PainResistence;
        public int PainResistence
        {
            get => m_PainResistence;
            set => this.RaiseAndSetIfChanged(ref m_PainResistence, value);
        }
        #endregion

        public IReadOnlyCollection<IInitiativeScoreViewModel> Initiatives { get; private set; } = DefaultInitiatives();

        protected override void OnStart()
        {
            Alias = string.Empty;

        }

        protected override ICharacterViewModel? OkResult()
        {
            var loader = new Character
            {
                Alias = Alias,
                IsPlayer = IsPlayer,
                Player = Player,
                BaseAgility = BaseAgility,
                BaseBody = BaseBody,
                BaseCharisma = BaseCharisma,
                BaseIntuition = BaseIntuition,
                BaseLogic = BaseLogic,
                BaseReaction = BaseReaction,
                BaseStrength = BaseStrength,
                BaseWillpower = BaseWillpower,
                Edge = BaseEdge,
                Essence = 6,
                PainEditor = PainEditor,
                PainResistence = PainResistence,
            };

            SetInitiatives(loader);

            var character = _viewModelFactory.Create<ICharacterViewModel, Character>(loader);
            return character;
        }

        protected override ICharacterViewModel? CancelResult()
        {
            return default;
        }

        protected override void OnReset()
        {
            Alias = string.Empty;
            IsPlayer = false;
            Player = string.Empty;
            BaseEdge = 1;
            BaseBody = 1;
            BaseAgility = 1;
            BaseReaction = 1;
            BaseStrength = 1;
            BaseCharisma = 1;
            BaseIntuition = 1;
            BaseLogic = 1;
            BaseWillpower = 1;
            PainEditor = false;
            PainResistence = 0;

            this.RaisePropertyChanging(nameof(Initiatives));
            Initiatives = DefaultInitiatives();
            this.RaisePropertyChanged(nameof(Initiatives));
        }

        private void SetInitiatives(Character character)
        {
            if (character is null)
            {
                throw new ArgumentNullException(nameof(character));
            }

            var improvements = new List<Improvement>();

            int physScore = 0;
            int physDice = 0;

            foreach (var init in Initiatives)
            {
                switch (init.State)
                {
                    case InitiativeState.Physical:
                        character.Improve(nameof(ICharacterViewModel.PhysicalInitiativeDice), c => CharacterViewModel.BasePhysicalInitiativeDice, init.Dice);
                        character.Improve(nameof(ICharacterViewModel.PhysicalInitiative), c => c.BaseReaction + c.BaseIntuition, init.Score);
                        physScore = init.Score;
                        physDice = init.Dice;
                        break;
                    case InitiativeState.Astral:
                        character.Improve(nameof(ICharacterViewModel.AstralInitiativeDice), c => CharacterViewModel.BaseAstralInitiativeDice, init.Dice);
                        character.Improve(nameof(ICharacterViewModel.AstralInitiative), c => 2 * c.BaseIntuition, init.Score);
                        break;
                    case InitiativeState.MatrixAR:
                        character.Improve(nameof(ICharacterViewModel.MatrixARInitiativeDice), c => physDice , init.Dice);
                        character.Improve(nameof(ICharacterViewModel.MatrixARInitiative), c => physScore, init.Score);
                        break;
                    case InitiativeState.MatrixCold:
                        character.Improve(nameof(ICharacterViewModel.MatrixColdInitiativeDice), c => CharacterViewModel.BaseMatrixColdInitiativeDice, init.Dice);
                        character.Improve(nameof(ICharacterViewModel.MatrixColdInitiative), c => c.BaseIntuition, init.Score);
                        break;
                    case InitiativeState.MatrixHot:
                        character.Improve(nameof(ICharacterViewModel.MatrixHotInitiativeDice), c => CharacterViewModel.BaseMatrixHotInitiativeDice, init.Dice);
                        character.Improve(nameof(ICharacterViewModel.MatrixHotInitiative), c => c.BaseIntuition, init.Score);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
