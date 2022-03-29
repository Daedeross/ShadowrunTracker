using ReactiveUI;
using ShadowrunTracker;
using ShadowrunTracker.Data;
using ShadowrunTracker.Model;
using ShadowrunTracker.ViewModels;
using ShadowrunTracker.Utils;
using ShadowrunTracker.ViewModels.Traits;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Collections.Specialized;
using System.Collections.Generic;
using ShadowrunTracker.ViewModels.Internal;
using ShadowrunTools.Model;

namespace ShadowrunTracker.ViewModels
{
    public class CharacterViewModel : ReactiveObject, ICharacterViewModel
    {
        const int DamageTrackBase = 8;
        const int DamageTrackRow = 3;
        const int BasePhysicalInitiativeDice = 1;
        const int BaseAstralInitiativeDice = 2;
        const int BaseMatrixColdInitiativeDice = 3;
        const int BaseMatrixHotInitiativeDice = 4;
        const int PenaltyPerSpell = -2;

        private readonly IRoller _roller;
        private readonly Dictionary<IImprovementViewModel, BonusHandler> _bonusHandlers;

        public CharacterViewModel(IRoller roller)
        {
            _roller = roller ?? throw new ArgumentNullException(nameof(roller));

            m_Alias    = string.Empty;
            m_IsPlayer = false;
            m_Player   = string.Empty;

            m_Essence = 6m;

            m_BaseBody      = 1;
            m_BaseAgility   = 1;
            m_BaseReaction  = 1;
            m_BaseStrength  = 1;
            m_BaseCharisma  = 1;
            m_BaseIntuition = 1;
            m_BaseLogic     = 1;
            m_BaseWillpower = 1;

            m_BaseEdge      = 1;
            m_BaseMagic     = 0;
            m_BaseResonance = 0;

            m_PainEditor      = false;
            m_PainResistence  = 0;
            m_SpellsSustained = 0;

            Skills = new ObservableCollection<ISkillViewModel>();
            Improvements = new ObservableCollection<IImprovementViewModel>();
            Improvements.CollectionChanged += OnImprovementsChanged;
            _bonusHandlers = Improvements.ToDictionary(
                imp => imp,
                imp => new BonusHandler(this, imp));
        }

        public CharacterViewModel(IRoller roller, ICharacter loader)
        {
            _roller = roller ?? throw new ArgumentNullException(nameof(roller));

            m_Alias = loader.Alias;
            m_IsPlayer = loader.IsPlayer;
            m_Player = loader.Player;

            m_Essence = loader.Essence;

            m_BaseBody      = loader.BaseBody;
            m_BaseAgility   = loader.BaseAgility;
            m_BaseReaction  = loader.BaseReaction;
            m_BaseStrength  = loader.BaseStrength;
            m_BaseCharisma  = loader.BaseCharisma;
            m_BaseIntuition = loader.BaseIntuition;
            m_BaseLogic     = loader.BaseLogic;
            m_BaseWillpower = loader.BaseWillpower;

            m_BaseEdge      = loader.Edge;
            m_BaseMagic     = loader.Magic;
            m_BaseResonance = loader.Resonance;

            m_PainEditor      = loader.PainEditor;
            m_PainResistence  = loader.PainResistence;
            m_SpellsSustained = loader.SpellsSustained;

            Skills       = new ObservableCollection<ISkillViewModel>(loader.Skills.Select(s => (ISkillViewModel)new SkillViewModel(s)));
            Improvements = new ObservableCollection<IImprovementViewModel>(loader.Improvements.Select(i => (IImprovementViewModel)new ImprovementViewModel(i)));
            Improvements.CollectionChanged += OnImprovementsChanged;
            _bonusHandlers = Improvements.ToDictionary(
                imp => imp,
                imp => new BonusHandler(this, imp));
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
        public int MatrixHotInitiative => Intuition + m_BonusMatrixHotInitiative + TotalPenalty;

        [DependsOn(nameof(BonusMatrixHotInitiativeDice))]
        public int MatrixHotInitiativeDice => BaseMatrixHotInitiativeDice + m_BonusMatrixHotInitiativeDice;

        private InitiativeState m_CurrentState;
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

        public InitiativeRoll RollInitiative()
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

            roll.Result = _roller.RollDice(roll.DiceUsed).Sum + roll.ScoreUsed;

            return roll;
        }

        public void ApplyPhysicalDamage(int damage)
        {
            PhysicalDamage += damage;
        }

        public void ApplyMentalDamage(int damage)
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

        #region Commands

        #endregion
    }
}
