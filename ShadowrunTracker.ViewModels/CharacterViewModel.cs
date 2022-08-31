namespace ShadowrunTracker.ViewModels
{
    using ReactiveUI;
    using ShadowrunTracker.Data;
    using ShadowrunTracker.Data.Traits;
    using ShadowrunTracker.Model;
    using ShadowrunTracker.Utils;
    using ShadowrunTracker.ViewModels.Internal;
    using ShadowrunTracker.ViewModels.Traits;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Reactive.Disposables;
    using System.Windows.Input;

    public class CharacterViewModel : CanRequestConfirmationBase, ICharacterViewModel
    {
        public const int BlitzDice = 5;
        public const int DamageTrackBase = 8;
        public const int DamageTrackRow = 3;
        public const int BasePhysicalInitiativeDice = 1;
        public const int BaseAstralInitiativeDice = 2;
        public const int BaseMatrixColdInitiativeDice = 3;
        public const int BaseMatrixHotInitiativeDice = 4;
        public const int PenaltyPerSpell = -2;

        private static readonly ISet<string> RecordProperties = new HashSet<string>()
        {
            nameof(Id),
            nameof(Alias),
            nameof(IsPlayer),
            nameof(Player),
            nameof(Essence),
            nameof(BaseBody),
            nameof(BaseAgility),
            nameof(BaseReaction),
            nameof(BaseStrength),
            nameof(BaseCharisma),
            nameof(BaseIntuition),
            nameof(BaseLogic),
            nameof(BaseWillpower),
            nameof(Edge),
            nameof(Magic),
            nameof(Resonance),
            nameof(PainEditor),
            nameof(PainResistence),
            nameof(SpellsSustained),
            nameof(Skills),
            nameof(Improvements)
        };

        private readonly Dictionary<IImprovementViewModel, BonusHandler> _bonusHandlers;
        private readonly IDataStore<Guid> _store;
        private readonly IViewModelFactory _viewModelFactory;

        private bool _pushUpdate = true;
        private string? _filename;

        private IRoller _roller;
        /// <summary>
        /// Public to allow to be injected;
        /// </summary>
        public IRoller Roller { get => _roller ?? ShadowrunTracker.Utils.Roller.Default; set => _roller = value; }

        public bool IsChanged { get; set; }

        public CharacterViewModel(IRoller roller, IDataStore<Guid> store, IViewModelFactory viewModelFactory)
        {
            _roller = roller ?? throw new ArgumentNullException(nameof(roller));
            _store = store;
            _viewModelFactory = viewModelFactory;

            _id = Guid.NewGuid();
            m_Alias = string.Empty;
            m_IsPlayer = false;
            m_Player = string.Empty;

            m_Essence = 6m;

            m_BaseBody = 1;
            m_BaseAgility = 1;
            m_BaseReaction = 1;
            m_BaseStrength = 1;
            m_BaseCharisma = 1;
            m_BaseIntuition = 1;
            m_BaseLogic = 1;
            m_BaseWillpower = 1;

            m_BaseEdge = 1;
            m_BaseMagic = 0;
            m_BaseResonance = 0;

            m_PainEditor = false;
            m_PainResistence = 0;
            m_SpellsSustained = 0;

            Skills = new ObservableCollection<ISkillViewModel>();
            Improvements = new ObservableCollection<IImprovementViewModel>();
            Improvements.CollectionChanged += OnImprovementsChanged;
            _bonusHandlers = Improvements.ToDictionary(
                imp => imp,
                imp => new BonusHandler(this, imp));

            SaveCommand = ReactiveCommand.Create(Save)
                .DisposeWith(_disposables);
            RemoveCharacter = ReactiveCommand.Create(RemoveCharacterExecute)
                .DisposeWith(_disposables);

            PropertyChanged += OnPropertyChanged;
        }

        public CharacterViewModel(IRoller roller, IDataStore<Guid> store, IViewModelFactory viewModelFactory, [NotNull] Character record)
        {
            if (record is null)
            {
                throw new ArgumentNullException(nameof(record));
            }

            _roller = roller ?? throw new ArgumentNullException(nameof(roller));
            _store = store;
            _viewModelFactory = viewModelFactory;

            _id = record.Id == Guid.Empty ? Guid.NewGuid() : record.Id;
            m_Alias = record.Alias ?? string.Empty;
            m_IsPlayer = record.IsPlayer;
            m_Player = record.Player ?? string.Empty;

            m_Essence = record.Essence;

            m_BaseBody = record.BaseBody;
            m_BaseAgility = record.BaseAgility;
            m_BaseReaction = record.BaseReaction;
            m_BaseStrength = record.BaseStrength;
            m_BaseCharisma = record.BaseCharisma;
            m_BaseIntuition = record.BaseIntuition;
            m_BaseLogic = record.BaseLogic;
            m_BaseWillpower = record.BaseWillpower;

            m_BaseEdge = record.Edge;
            m_BaseMagic = record.Magic;
            m_BaseResonance = record.Resonance;

            m_PainEditor = record.PainEditor;
            m_PainResistence = record.PainResistence;
            m_SpellsSustained = record.SpellsSustained;

            Skills = new ObservableCollection<ISkillViewModel>(record.Skills.Select(s => (ISkillViewModel)new SkillViewModel(s)));
            Improvements = new ObservableCollection<IImprovementViewModel>(record.Improvements.Select(i => (IImprovementViewModel)new ImprovementViewModel(i)));
            Improvements.CollectionChanged += OnImprovementsChanged;
            _bonusHandlers = Improvements.ToDictionary(
                imp => imp,
                imp => new BonusHandler(this, imp));

            SaveCommand = ReactiveCommand.Create(Save)
                .DisposeWith(_disposables);
            RemoveCharacter = ReactiveCommand.Create(RemoveCharacterExecute)
                .DisposeWith(_disposables);

            PropertyChanged += OnPropertyChanged;
        }

        private void OnImprovementsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                foreach (IImprovementViewModel item in e.OldItems)
                {
                    if (_bonusHandlers.Remove(item, out var handler))
                    {
                        handler.Dispose();
                    }
                }
            }
            if (e.NewItems != null)
            {
                foreach (IImprovementViewModel item in e.NewItems)
                {
                    _bonusHandlers.Add(item, new BonusHandler(this, item));
                }
            }
        }

        #region Properties
        #region Core

        private Guid _id;
        public Guid Id => _id;

        private string m_Alias = string.Empty;
        public string Alias
        {
            get => m_Alias;
            set => this.SetAndRaiseIfChanged(ref m_Alias, value);
        }

        private bool m_IsPlayer = false;
        public bool IsPlayer
        {
            get => m_IsPlayer;
            set => this.SetAndRaiseIfChanged(ref m_IsPlayer, value);
        }

        private string m_Player = string.Empty;
        public string Player
        {
            get => m_Player;
            set => this.SetAndRaiseIfChanged(ref m_Player, value);
        }

        private decimal m_Essence = 6m;
        public decimal Essence
        {
            get => m_Essence;
            set => this.SetAndRaiseIfChanged(ref m_Essence, value);
        }

        #endregion

        #region Attributes

        private int m_BaseBody;
        public int BaseBody
        {
            get => m_BaseBody;
            set => this.SetAndRaiseIfChanged(ref m_BaseBody, value);
        }

        private int m_BaseAgility;
        public int BaseAgility
        {
            get => m_BaseAgility;
            set => this.SetAndRaiseIfChanged(ref m_BaseAgility, value);
        }

        private int m_BaseReaction;
        public int BaseReaction
        {
            get => m_BaseReaction;
            set => this.SetAndRaiseIfChanged(ref m_BaseReaction, value);
        }

        private int m_BaseStrength;
        public int BaseStrength
        {
            get => m_BaseStrength;
            set => this.SetAndRaiseIfChanged(ref m_BaseStrength, value);
        }

        private int m_BaseCharisma;
        public int BaseCharisma
        {
            get => m_BaseCharisma;
            set => this.SetAndRaiseIfChanged(ref m_BaseCharisma, value);
        }

        private int m_BaseIntuition;
        public int BaseIntuition
        {
            get => m_BaseIntuition;
            set => this.SetAndRaiseIfChanged(ref m_BaseIntuition, value);
        }

        private int m_BaseLogic;
        public int BaseLogic
        {
            get => m_BaseLogic;
            set => this.SetAndRaiseIfChanged(ref m_BaseLogic, value);
        }

        private int m_BaseWillpower;
        public int BaseWillpower
        {
            get => m_BaseWillpower;
            set => this.SetAndRaiseIfChanged(ref m_BaseWillpower, value);
        }


        private int m_BonusBody;
        public int BonusBody
        {
            get => m_BonusBody;
            set => this.SetAndRaiseIfChanged(ref m_BonusBody, value);
        }

        private int m_BonusAgility;
        public int BonusAgility
        {
            get => m_BonusAgility;
            set => this.SetAndRaiseIfChanged(ref m_BonusAgility, value);
        }

        private int m_BonusReaction;
        public int BonusReaction
        {
            get => m_BonusReaction;
            set => this.SetAndRaiseIfChanged(ref m_BonusReaction, value);
        }

        private int m_BonusStrength;
        public int BonusStrength
        {
            get => m_BonusStrength;
            set => this.SetAndRaiseIfChanged(ref m_BonusStrength, value);
        }

        private int m_BonusCharisma;
        public int BonusCharisma
        {
            get => m_BonusCharisma;
            set => this.SetAndRaiseIfChanged(ref m_BonusCharisma, value);
        }

        private int m_BonusIntuition;
        public int BonusIntuition
        {
            get => m_BonusIntuition;
            set => this.SetAndRaiseIfChanged(ref m_BonusIntuition, value);
        }

        private int m_BonusLogic;
        public int BonusLogic
        {
            get => m_BonusLogic;
            set => this.SetAndRaiseIfChanged(ref m_BonusLogic, value);
        }

        private int m_BonusWillpower;
        public int BonusWillpower
        {
            get => m_BonusWillpower;
            set => this.SetAndRaiseIfChanged(ref m_BonusWillpower, value);
        }

        [DependsOn(nameof(BaseBody))]
        [DependsOn(nameof(BonusBody))]
        public int Body => m_BaseBody + m_BonusBody;

        [DependsOn(nameof(BaseAgility))]
        [DependsOn(nameof(BonusAgility))]
        public int Agility => m_BaseAgility + m_BonusAgility;

        [DependsOn(nameof(BaseReaction))]
        [DependsOn(nameof(BonusReaction))]
        public int Reaction => m_BaseReaction + m_BonusReaction;

        [DependsOn(nameof(BaseStrength))]
        [DependsOn(nameof(BonusStrength))]
        public int Strength => m_BaseStrength + m_BonusStrength;

        [DependsOn(nameof(BaseWillpower))]
        [DependsOn(nameof(BonusWillpower))]
        public int Willpower => m_BaseWillpower + m_BonusWillpower;

        [DependsOn(nameof(BaseIntuition))]
        [DependsOn(nameof(BonusIntuition))]
        public int Intuition => m_BaseIntuition + m_BonusIntuition;

        [DependsOn(nameof(BaseLogic))]
        [DependsOn(nameof(BonusLogic))]
        public int Logic => m_BaseLogic + m_BonusLogic;

        [DependsOn(nameof(BaseCharisma))]
        [DependsOn(nameof(BonusCharisma))]
        public int Charisma => m_BaseCharisma + m_BonusCharisma;

        #endregion

        #region Special Attributes

        private int m_BaseEdge;
        public int BaseEdge
        {
            get => m_BaseEdge;
            set => this.SetAndRaiseIfChanged(ref m_BaseEdge, value);
        }

        private int m_BaseMagic;
        public int BaseMagic
        {
            get => m_BaseMagic;
            set => this.SetAndRaiseIfChanged(ref m_BaseMagic, value);
        }

        private int m_BaseResonance;
        public int BaseResonance
        {
            get => m_BaseResonance;
            set => this.SetAndRaiseIfChanged(ref m_BaseResonance, value);
        }


        private int m_BonusEdge;
        public int BonusEdge
        {
            get => m_BonusEdge;
            set => this.SetAndRaiseIfChanged(ref m_BonusEdge, value);
        }

        private int m_BonusMagic;
        public int BonusMagic
        {
            get => m_BonusMagic;
            set => this.SetAndRaiseIfChanged(ref m_BonusMagic, value);
        }

        private int m_BonusResonance;
        public int BonusResonance
        {
            get => m_BonusResonance;
            set => this.SetAndRaiseIfChanged(ref m_BonusResonance, value);
        }

        [DependsOn(nameof(BaseEdge))]
        [DependsOn(nameof(BonusEdge))]
        public int Edge => m_BaseEdge + m_BonusEdge;

        [DependsOn(nameof(BaseMagic))]
        [DependsOn(nameof(BonusMagic))]
        public int Magic => m_BaseMagic + m_BonusMagic;

        [DependsOn(nameof(BaseResonance))]
        [DependsOn(nameof(BonusResonance))]
        public int Resonance => m_BaseResonance + m_BonusResonance;

        private int m_EdgePoints;
        public int EdgePoints
        {
            get => m_EdgePoints;
            set => this.SetAndRaiseIfChanged(ref m_EdgePoints, value);
        }

        #endregion

        #region Matrix Attributes


        #endregion

        #region Damage Track

        private int m_BonusPhysicalBoxes;
        public int BonusPhysicalBoxes
        {
            get => m_BonusPhysicalBoxes;
            set => this.SetAndRaiseIfChanged(ref m_BonusPhysicalBoxes, value);
        }

        [DependsOn(nameof(Body))]
        [DependsOn(nameof(BonusPhysicalBoxes))]
        public int PhysicalBoxes => DamageTrackBase + Convert.ToInt32(Math.Ceiling(Body / 2f)) + m_BonusPhysicalBoxes;

        private int m_PhysicalDamage;
        public int PhysicalDamage
        {
            get => m_PhysicalDamage;
            set => this.SetAndRaiseIfChanged(ref m_PhysicalDamage, value);
        }

        private int m_BonusStunBoxes;
        public int BonusStunBoxes
        {
            get => m_BonusStunBoxes;
            set => this.SetAndRaiseIfChanged(ref m_BonusStunBoxes, value);
        }

        [DependsOn(nameof(Willpower))]
        [DependsOn(nameof(BonusStunBoxes))]
        public int StunBoxes => DamageTrackBase + Convert.ToInt32(Math.Ceiling(Willpower / 2f)) + m_BonusStunBoxes;

        private int m_StunDamage;
        public int StunDamage
        {
            get => m_StunDamage;
            set => this.SetAndRaiseIfChanged(ref m_StunDamage, value);
        }

        private bool m_PainEditor;
        public bool PainEditor
        {
            get => m_PainEditor;
            set => this.SetAndRaiseIfChanged(ref m_PainEditor, value);
        }

        private int m_PainResistence;
        public int PainResistence
        {
            get => m_PainResistence;
            set => this.SetAndRaiseIfChanged(ref m_PainResistence, value);
        }

        [DependsOn(nameof(PhysicalDamage))]
        [DependsOn(nameof(StunDamage))]
        [DependsOn(nameof(PainEditor))]
        [DependsOn(nameof(PainResistence))]
        public int WoundModifier => m_PainEditor
            ? 0
            : -(m_PhysicalDamage - m_PainResistence) / DamageTrackRow - (m_StunDamage - m_PainResistence) / DamageTrackRow;


        private int m_SpellsSustained;
        public int SpellsSustained
        {
            get => m_SpellsSustained;
            set => this.SetAndRaiseIfChanged(ref m_SpellsSustained, value);
        }

        [DependsOn(nameof(WoundModifier))]
        [DependsOn(nameof(SpellsSustained))]
        public int TotalPenalty => WoundModifier + SpellsSustained * PenaltyPerSpell;

        #endregion

        #region Initiatives

        private int m_BonusPhysicalInitiative;
        public int BonusPhysicalInitiative
        {
            get => m_BonusPhysicalInitiative;
            set => this.SetAndRaiseIfChanged(ref m_BonusPhysicalInitiative, value);
        }

        private int m_BonusPhysicalInitiativeDice;
        public int BonusPhysicalInitiativeDice
        {
            get => m_BonusPhysicalInitiativeDice;
            set => this.SetAndRaiseIfChanged(ref m_BonusPhysicalInitiativeDice, value);
        }

        [DependsOn(nameof(Reaction))]
        [DependsOn(nameof(Intuition))]
        [DependsOn(nameof(BonusPhysicalInitiative))]
        [DependsOn(nameof(TotalPenalty))]
        public int PhysicalInitiative => Reaction + Intuition + m_BonusPhysicalInitiative + TotalPenalty;

        [DependsOn(nameof(BonusPhysicalInitiativeDice))]
        public int PhysicalInitiativeDice => BasePhysicalInitiativeDice + m_BonusPhysicalInitiativeDice;


        private int m_BonusAstralInitiative;
        public int BonusAstralInitiative
        {
            get => m_BonusAstralInitiative;
            set => this.SetAndRaiseIfChanged(ref m_BonusAstralInitiative, value);
        }

        private int m_BonusAstralInitiativeDice;
        public int BonusAstralInitiativeDice
        {
            get => m_BonusAstralInitiativeDice;
            set => this.SetAndRaiseIfChanged(ref m_BonusAstralInitiativeDice, value);
        }

        [DependsOn(nameof(Intuition))]
        [DependsOn(nameof(BonusAstralInitiative))]
        [DependsOn(nameof(TotalPenalty))]
        public int AstralInitiative => Intuition * 2 + BonusAstralInitiative + TotalPenalty;

        [DependsOn(nameof(BonusAstralInitiativeDice))]
        public int AstralInitiativeDice => BaseAstralInitiativeDice + m_BonusAstralInitiativeDice;


        private int m_BonusMatrixARInitiative;
        public int BonusMatrixARInitiative
        {
            get => m_BonusMatrixARInitiative;
            set => this.SetAndRaiseIfChanged(ref m_BonusMatrixARInitiative, value);
        }

        private int m_BonusMatrixARInitiativeDice;
        public int BonusMatrixARInitiativeDice
        {
            get => m_BonusMatrixARInitiativeDice;
            set => this.SetAndRaiseIfChanged(ref m_BonusMatrixARInitiativeDice, value);
        }

        [DependsOn(nameof(PhysicalInitiative))]
        [DependsOn(nameof(BonusMatrixARInitiative))]
        public int MatrixARInitiative => PhysicalInitiative + m_BonusMatrixARInitiative; // Penalty already applied to Physical Initiative

        [DependsOn(nameof(BonusMatrixARInitiativeDice))]
        public int MatrixARInitiativeDice => PhysicalInitiativeDice + m_BonusMatrixARInitiativeDice;


        private int m_BonusMatrixColdInitiative;
        public int BonusMatrixColdInitiative
        {
            get => m_BonusMatrixColdInitiative;
            set => this.SetAndRaiseIfChanged(ref m_BonusMatrixColdInitiative, value);
        }

        private int m_BonusMatrixColdInitiativeDice;
        public int BonusMatrixColdInitiativeDice
        {
            get => m_BonusMatrixColdInitiativeDice;
            set => this.SetAndRaiseIfChanged(ref m_BonusMatrixColdInitiativeDice, value);
        }

        [DependsOn(nameof(Intuition))]
        [DependsOn(nameof(BonusMatrixColdInitiative))]
        [DependsOn(nameof(TotalPenalty))]
        public int MatrixColdInitiative => Intuition + m_BonusMatrixColdInitiative + TotalPenalty;

        [DependsOn(nameof(BonusMatrixColdInitiativeDice))]
        public int MatrixColdInitiativeDice => BaseMatrixColdInitiativeDice + m_BonusMatrixColdInitiativeDice;


        private int m_BonusMatrixHotInitiative;
        public int BonusMatrixHotInitiative
        {
            get => m_BonusMatrixHotInitiative;
            set => this.SetAndRaiseIfChanged(ref m_BonusMatrixHotInitiative, value);
        }

        private int m_BonusMatrixHotInitiativeDice;
        public int BonusMatrixHotInitiativeDice
        {
            get => m_BonusMatrixHotInitiativeDice;
            set => this.SetAndRaiseIfChanged(ref m_BonusMatrixHotInitiativeDice, value);
        }

        [DependsOn(nameof(Intuition))]
        [DependsOn(nameof(BonusMatrixHotInitiative))]
        [DependsOn(nameof(TotalPenalty))]
        public int MatrixHotInitiative => Intuition + m_BonusMatrixHotInitiative + TotalPenalty;

        [DependsOn(nameof(BonusMatrixHotInitiativeDice))]
        public int MatrixHotInitiativeDice => BaseMatrixHotInitiativeDice + m_BonusMatrixHotInitiativeDice;

        private InitiativeState m_CurrentState = InitiativeState.Physical;
        public InitiativeState CurrentState
        {
            get => m_CurrentState;
            set
            {
                this.SetAndRaiseIfChanged(ref m_CurrentState, value);
            }
        }

        [DependsOn(nameof(CurrentState))]
        [DependsOn(nameof(PhysicalInitiative))]
        [DependsOn(nameof(AstralInitiative))]
        [DependsOn(nameof(MatrixARInitiative))]
        [DependsOn(nameof(MatrixColdInitiative))]
        [DependsOn(nameof(MatrixHotInitiative))]
        public int CurrentInitiative =>
            CurrentState switch
            {
                InitiativeState.Physical => PhysicalInitiative,
                InitiativeState.Astral => AstralInitiative,
                InitiativeState.MatrixAR => MatrixARInitiative,
                InitiativeState.MatrixCold => MatrixColdInitiative,
                InitiativeState.MatrixHot => MatrixHotInitiative,
                _ => 0
            };

        [DependsOn(nameof(CurrentState))]
        [DependsOn(nameof(PhysicalInitiativeDice))]
        [DependsOn(nameof(AstralInitiativeDice))]
        [DependsOn(nameof(MatrixARInitiativeDice))]
        [DependsOn(nameof(MatrixColdInitiativeDice))]
        [DependsOn(nameof(MatrixHotInitiativeDice))]
        public int CurrentInitiativeDice =>
            CurrentState switch
            {
                InitiativeState.Physical => PhysicalInitiativeDice,
                InitiativeState.Astral => AstralInitiativeDice,
                InitiativeState.MatrixAR => MatrixARInitiativeDice,
                InitiativeState.MatrixCold => MatrixColdInitiativeDice,
                InitiativeState.MatrixHot => MatrixHotInitiativeDice,
                _ => 0
            };

        #endregion

        #region Limits

        private int m_BonusPhysicalLimit;
        public int BonusPhysicalLimit
        {
            get => m_BonusPhysicalLimit;
            set => this.SetAndRaiseIfChanged(ref m_BonusPhysicalLimit, value);
        }

        [DependsOn(nameof(Strength))]
        [DependsOn(nameof(Body))]
        [DependsOn(nameof(Reaction))]
        [DependsOn(nameof(BonusPhysicalLimit))]
        public int PhysicalLimit => Convert.ToInt32(Math.Ceiling((Strength * 2.0 + Body + Reaction) / 3.0)) + m_BonusPhysicalLimit;


        private int m_BonusMentalLimit;
        public int BonusMentalLimit
        {
            get => m_BonusMentalLimit;
            set => this.SetAndRaiseIfChanged(ref m_BonusMentalLimit, value);
        }

        [DependsOn(nameof(Logic))]
        [DependsOn(nameof(Intuition))]
        [DependsOn(nameof(Willpower))]
        [DependsOn(nameof(BonusMentalLimit))]
        public int MentalLimit => Convert.ToInt32(Math.Ceiling((Logic * 2.0 + Intuition + Willpower) / 3.0)) + m_BonusMentalLimit;

        private int m_BonusSocialLimit;
        public int BonusSocialLimit
        {
            get => m_BonusSocialLimit;
            set => this.SetAndRaiseIfChanged(ref m_BonusSocialLimit, value);
        }

        [DependsOn(nameof(Charisma))]
        [DependsOn(nameof(Willpower))]
        [DependsOn(nameof(Essence))]
        [DependsOn(nameof(BonusSocialLimit))]
        public int SocialLimit => Convert.ToInt32(Math.Ceiling((Charisma * 2.0 + Willpower + Math.Ceiling(Convert.ToDouble(Essence))) / 3.0)) + m_BonusMentalLimit;

        private int m_BonusAstralLimit;
        public int BonusAstralLimit
        {
            get => m_BonusAstralLimit;
            set => this.SetAndRaiseIfChanged(ref m_BonusAstralLimit, value);
        }

        [DependsOn(nameof(BonusAstralLimit))]
        [DependsOn(nameof(MentalLimit))]
        [DependsOn(nameof(SocialLimit))]
        public int AstralLimit => Math.Max(MentalLimit, SocialLimit) + m_BonusAstralLimit;

        #endregion

        #region Skills

        public ObservableCollection<ISkillViewModel> Skills { get; private set; }

        #endregion

        #region Improvements

        public ObservableCollection<IImprovementViewModel> Improvements { get; private set; }

        #endregion

        #endregion

        #region Methods

        public InitiativeRoll RollInitiative(bool blitz = false)
        {
            var roll = new InitiativeRoll
            {
                CurrentState = CurrentState,
            };

            switch (CurrentState)
            {
                case InitiativeState.Physical:
                    roll.DiceUsed = PhysicalInitiativeDice;
                    roll.ScoreUsed = PhysicalInitiative;
                    break;
                case InitiativeState.Astral:
                    roll.DiceUsed = AstralInitiativeDice;
                    roll.ScoreUsed = AstralInitiative;
                    break;
                case InitiativeState.MatrixAR:
                    roll.DiceUsed = MatrixARInitiativeDice;
                    roll.ScoreUsed = MatrixARInitiative;
                    break;
                case InitiativeState.MatrixCold:
                    roll.DiceUsed = MatrixColdInitiativeDice;
                    roll.ScoreUsed = MatrixColdInitiative;
                    break;
                case InitiativeState.MatrixHot:
                    roll.DiceUsed = MatrixHotInitiativeDice;
                    roll.ScoreUsed = MatrixHotInitiative;
                    break;
                default:
                    return roll;
            }

            if (blitz)
            {
                roll.Blitzed = true;
                roll.DiceUsed = BlitzDice;
            }

            roll.Result = _roller.RollDice(roll.DiceUsed).Sum + roll.ScoreUsed;

            return roll;
        }

        public void ApplyPhysicalDamage(int damage)
        {
            PhysicalDamage += damage;
        }

        public void ApplyStunDamage(int damage)
        {
            if (damage < 1)
            {
                return;
            }

            var total = StunDamage + damage;
            if (total > StunBoxes)
            {
                StunDamage = StunBoxes;
                ApplyPhysicalDamage(total - StunBoxes);
            }
            else
            {
                StunDamage = total;
            }
        }

        public void ApplyPhysicalHealing(int healing)
        {
            if (healing < 1)
            {
                return;
            }

            PhysicalDamage = Math.Max(0, PhysicalDamage - healing);
        }

        public void ApplyStunHealing(int healing)
        {
            if (healing < 1)
            {
                return;
            }

            StunDamage = Math.Max(0, StunDamage - healing);
        }

        #endregion

        #region ICanSave

        public ICommand SaveCommand { get; }

        public void Save()
        {
            Interactions.SaveDialog
                .Handle(new SaveContext(ToRecord(), IsChanged, _filename))
                .Subscribe(s =>
                {
                    if (s != null)
                    {
                        _filename = s;
                        IsChanged = false;
                    }
                })
                .DisposeWith(_disposables);
        }

        RecordBase IRecordViewModel.Record => ToRecord();

        public Character Record => ToRecord();

        public Character ToRecord()
        {
            return this.ToModel();
        }

        public void Update(Character record)
        {
            try
            {
                _pushUpdate = false;

                if (record.Id != Id)
                {
                    throw new ArgumentException("Record and VM Ids do not match");
                }

                Alias = record.Alias ?? string.Empty;
                IsPlayer = record.IsPlayer;
                Player = record.Player ?? string.Empty;
                Essence = record.Essence;
                BaseBody = record.BaseBody;
                BaseAgility = record.BaseAgility;
                BaseReaction = record.BaseReaction;
                BaseStrength = record.BaseStrength;
                BaseCharisma = record.BaseCharisma;
                BaseIntuition = record.BaseIntuition;
                BaseLogic = record.BaseLogic;
                BaseWillpower = record.BaseWillpower;
                m_BaseEdge = record.Edge;
                m_BaseMagic = record.Magic;
                m_BaseResonance = record.Resonance;
                PainEditor = record.PainEditor;
                PainResistence = record.PainResistence;
                SpellsSustained = record.SpellsSustained;
                UpdateSkills(record.Skills);
                UpdateImprovements(record.Improvements);
            }
            finally
            {
                _pushUpdate = true;
            }
        }

        private void UpdateSkills(IEnumerable<Skill> incomming)
        {
            var oldMap = Skills.ToDictionary(p => p.Id);
            var newMap = incomming.ToDictionary(p => p.Id); ;

            var removed = oldMap.Values.Where(vm => !newMap.ContainsKey(vm.Id));
            var added = newMap.Values.Where(record => !oldMap.ContainsKey(record.Id));
            var update = newMap.Join(oldMap, kvp => kvp.Key, kvp => kvp.Key, (inc, old) => (inc.Value, old.Value));

            RemoveSkills(removed);

            foreach (var (record, viewModel) in update)
            {
                viewModel.Update(record);
            }

            AddSkillsFromRecords(added);
        }

        private void AddSkillsFromRecords(IEnumerable<Skill> added)
        {
            if (added.Any())
            {
                foreach (var record in added)
                {
                    Skills.Add(_store.CreateOrUpdate(record, s => new SkillViewModel(s)));
                }
            }
        }

        private void RemoveSkills(IEnumerable<ISkillViewModel> removed)
        {
            if (removed.Any())
            {
                foreach (var vm in removed)
                {
                    Skills.Remove(vm);
                }
            }
        }
        private void UpdateImprovements(IEnumerable<Improvement> incomming)
        {
            var oldMap = Improvements.ToDictionary(p => p.Id);
            var newMap = incomming.ToDictionary(p => p.Id); ;

            var removed = oldMap.Values.Where(vm => !newMap.ContainsKey(vm.Id));
            var added = newMap.Values.Where(record => !oldMap.ContainsKey(record.Id));
            var update = newMap.Join(oldMap, kvp => kvp.Key, kvp => kvp.Key, (inc, old) => (inc.Value, old.Value));

            RemoveImprovements(removed);

            foreach (var (record, viewModel) in update)
            {
                viewModel.Update(record);
            }

            AddImprovementsFromRecords(added);
        }

        private void AddImprovementsFromRecords(IEnumerable<Improvement> added)
        {
            if (added.Any())
            {
                foreach (var record in added)
                {
                    Improvements.Add(_store.CreateOrUpdate(record, s => _viewModelFactory.Create<IImprovementViewModel, Improvement>(s)));
                }
            }
        }

        private void RemoveImprovements(IEnumerable<IImprovementViewModel> removed)
        {
            if (removed.Any())
            {
                foreach (var vm in removed)
                {
                    Improvements.Remove(vm);
                }
            }
        }

        #endregion

        #region Commands

        public ICommand RemoveCharacter { get; }

        private void RemoveCharacterExecute()
        {
            RequestConfirmation($"Remove {Alias} from the encounter?", Delete);
        }

        #endregion

        #region Events

        public event EventHandler<RemoveCharacterEventArgs>? Remove;

        public void Delete()
        {
            Remove?.Invoke(this, new RemoveCharacterEventArgs(this));
        }
        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_pushUpdate && ReferenceEquals(this, sender) && RecordProperties.Contains(e.PropertyName))
            {
                this.RaisePropertyChanged(nameof(Record));
            }
        }

        #endregion

        private bool disposedValue;

        protected override void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    this.PropertyChanged -= OnPropertyChanged;
                }

                disposedValue = true;
            }
            base.Dispose(disposing);
        }
    }
}
