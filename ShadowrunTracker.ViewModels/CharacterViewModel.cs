using ReactiveUI;
using ShadowrunTracker.Contract;
using ShadowrunTracker.Contract.Data;
using ShadowrunTracker.Contract.Model;
using ShadowrunTracker.Contract.ViewModels;
using ShadowrunTracker.ViewModels.Traits;
using System;
using System.Collections.ObjectModel;
using System.Linq;

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

        public CharacterViewModel(IRoller roller)
        {
            _roller = roller ?? throw new ArgumentNullException(nameof(roller));

            Skills = new ObservableCollection<ISkillViewModel>();
            Improvements = new ObservableCollection<IImprovementViewModel>();
        }

        public CharacterViewModel(IRoller roller, ICharacter loader)
        {
            _roller = roller ?? throw new ArgumentNullException(nameof(roller));

            m_Alias = loader.Alias;
            m_IsPlayer = loader.IsPlayer;
            m_Player = loader.Player;

            m_Essence = loader.Essence;

            m_Body = loader.Body;
            m_Agility = loader.Agility;
            m_Reaction = loader.Reaction;
            m_Strength = loader.Strength;
            m_Charisma = loader.Charisma;
            m_Intuition = loader.Intuition;
            m_Logic = loader.Logic;
            m_Willpower = loader.Willpower;

            m_BonusBody = loader.BonusBody;
            m_BonusAgility = loader.BonusAgility;
            m_BonusReaction = loader.BonusReaction;
            m_BonusStrength = loader.BonusStrength;
            m_BonusCharisma = loader.BonusCharisma;
            m_BonusIntuition = loader.BonusIntuition;
            m_BonusLogic = loader.BonusLogic;
            m_BonusWillpower = loader.BonusWillpower;

            m_Edge = loader.Edge;
            m_Magic = loader.Magic;
            m_Resonance = loader.Resonance;

            m_BonusEdge = loader.BonusEdge;
            m_BonusMagic = loader.BonusMagic;
            m_BonusResonance = loader.BonusResonance;

            m_BonusPhysicalBoxes = loader.BonusPhysicalBoxes;

            m_BonusStunBoxes = loader.BonusStunBoxes;

            m_PainEditor = loader.PainEditor;
            m_PainResistence = loader.PainResistence;

            m_SpellsSustained = loader.SpellsSustained;

            m_BonusPhysicalInitiative = loader.BonusPhysicalInitiative;
            m_BonusPhysicalInitiativeDice = loader.BonusPhysicalInitiativeDice;

            m_BonusAstralInitiative = loader.BonusAstralInitiative;
            m_BonusAstralInitiativeDice = loader.BonusAstralInitiativeDice;

            m_BonusMatrixARInitiative = loader.BonusMatrixARInitiative;
            m_BonusMatrixARInitiativeDice = loader.BonusMatrixARInitiativeDice;

            m_BonusMatrixColdInitiative = loader.BonusMatrixColdInitiative;
            m_BonusMatrixColdInitiativeDice = loader.BonusMatrixColdInitiativeDice;

            m_BonusMatrixHotInitiative = loader.BonusMatrixHotInitiative;
            m_BonusMatrixHotInitiativeDice = loader.BonusMatrixHotInitiativeDice;


            m_BonusPhysicalLimit = loader.BonusPhysicalLimit;
            m_BonusMentalLimit = loader.BonusMentalLimit;
            m_BonusSocialLimit = loader.BonusSocialLimit;
            m_BonusAstralLimit = loader.BonusAstralLimit;

            Skills = new ObservableCollection<ISkillViewModel>(loader.Skills.Select(s => (ISkillViewModel)new SkillViewModel(s)));
            Improvements = new ObservableCollection<IImprovementViewModel>();
            //Improvements = new ObservableCollection<IImprovementViewModel>(loader.Improvements.Select(i => (IImprovementViewModel)new ImprovementViewModel(i)));
        }

        #region Properties

        private string m_Alias = string.Empty;
        public string Alias
        {
            get => m_Alias;
            set => this.RaiseAndSetIfChanged(ref m_Alias, value);
        }

        private bool m_IsPlayer = false;
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

        private decimal m_Essence = 6m;
        public decimal Essence
        {
            get => m_Essence;
            set => this.RaiseAndSetIfChanged(ref m_Essence,
                                             value,
                                             nameof(Essence),
                                             nameof(AugmentedSocialLimit));
        }

        #region Attributes

        private int m_Body;
        public int Body
        {
            get => m_Body;
            set => this.RaiseAndSetIfChanged(ref m_Body,
                                             value,
                                             nameof(Body),
                                             nameof(AugmentedBody),
                                             nameof(AugmentedPhysicalLimit),
                                             nameof(AugmentedPhysicalBoxes),
                                             nameof(AugmentedPhysicalInitiative),
                                             nameof(AugmentedAstralInitiative),
                                             nameof(AugmentedMatrixARInitiative),
                                             nameof(AugmentedMatrixColdInitiative),
                                             nameof(AugmentedMatrixHotInitiative),
                                             nameof(CurrentInitiative));
        }

        private int m_Agility;
        public int Agility
        {
            get => m_Agility;
            set => this.RaiseAndSetIfChanged(ref m_Agility,
                                             value,
                                             nameof(Agility),
                                             nameof(AugmentedAgility));
        }

        private int m_Reaction;
        public int Reaction
        {
            get => m_Reaction;
            set => this.RaiseAndSetIfChanged(ref m_Reaction,
                                             value,
                                             nameof(Reaction),
                                             nameof(AugmentedReaction),
                                             nameof(AugmentedPhysicalLimit),
                                             nameof(AugmentedPhysicalInitiative),
                                             nameof(AugmentedMatrixARInitiative),
                                             nameof(CurrentInitiative));
        }

        private int m_Strength;
        public int Strength
        {
            get => m_Strength;
            set => this.RaiseAndSetIfChanged(ref m_Strength,
                                             value,
                                             nameof(Strength),
                                             nameof(AugmentedStrength),
                                             nameof(AugmentedPhysicalLimit));
        }

        private int m_Charisma;
        public int Charisma
        {
            get => m_Charisma;
            set => this.RaiseAndSetIfChanged(ref m_Charisma,
                                             value,
                                             nameof(Charisma),
                                             nameof(AugmentedCharisma),
                                             nameof(AugmentedSocialLimit));
        }

        private int m_Intuition;
        public int Intuition
        {
            get => m_Intuition;
            set => this.RaiseAndSetIfChanged(ref m_Intuition,
                                             value,
                                             nameof(Intuition),
                                             nameof(AugmentedIntuition),
                                             nameof(AugmentedMentalLimit),
                                             nameof(AugmentedAstralLimit),
                                             nameof(AugmentedPhysicalInitiative),
                                             nameof(AugmentedAstralInitiative),
                                             nameof(AugmentedMatrixARInitiative),
                                             nameof(AugmentedMatrixColdInitiative),
                                             nameof(AugmentedMatrixHotInitiative),
                                             nameof(CurrentInitiative));
        }

        private int m_Logic;
        public int Logic
        {
            get => m_Logic;
            set => this.RaiseAndSetIfChanged(ref m_Logic,
                                             value,
                                             nameof(Logic),
                                             nameof(AugmentedLogic),
                                             nameof(AugmentedMentalLimit));
        }

        private int m_Willpower;
        public int Willpower
        {
            get => m_Willpower;
            set => this.RaiseAndSetIfChanged(ref m_Willpower,
                                             value,
                                             nameof(Willpower),
                                             nameof(AugmentedWillpower),
                                             nameof(AugmentedStunBoxes),
                                             nameof(AugmentedMentalLimit),
                                             nameof(AugmentedSocialLimit),
                                             nameof(AugmentedAstralLimit),
                                             nameof(WoundModifier),
                                             nameof(TotalPenalty),
                                             nameof(AugmentedPhysicalInitiative),
                                             nameof(AugmentedAstralInitiative),
                                             nameof(AugmentedMatrixARInitiative),
                                             nameof(AugmentedMatrixColdInitiative),
                                             nameof(AugmentedMatrixHotInitiative),
                                             nameof(CurrentInitiative));
        }


        private int m_BonusBody;
        public int BonusBody
        {
            get => m_BonusBody;
            set => this.RaiseAndSetIfChanged(ref m_BonusBody,
                                             value,
                                             nameof(BonusBody),
                                             nameof(AugmentedBody),
                                             nameof(AugmentedPhysicalLimit),
                                             nameof(AugmentedPhysicalBoxes),
                                             nameof(WoundModifier),
                                             nameof(TotalPenalty),
                                             nameof(AugmentedPhysicalInitiative),
                                             nameof(AugmentedAstralInitiative),
                                             nameof(AugmentedMatrixARInitiative),
                                             nameof(AugmentedMatrixColdInitiative),
                                             nameof(AugmentedMatrixHotInitiative),
                                             nameof(CurrentInitiative));
        }

        private int m_BonusAgility;
        public int BonusAgility
        {
            get => m_BonusAgility;
            set => this.RaiseAndSetIfChanged(ref m_BonusAgility,
                                             value,
                                             nameof(BonusAgility),
                                             nameof(AugmentedAgility));
        }

        private int m_BonusReaction;
        public int BonusReaction
        {
            get => m_BonusReaction;
            set => this.RaiseAndSetIfChanged(ref m_BonusReaction,
                                             value,
                                             nameof(BonusReaction),
                                             nameof(AugmentedReaction),
                                             nameof(AugmentedPhysicalLimit),
                                             nameof(AugmentedPhysicalInitiative),
                                             nameof(AugmentedMatrixARInitiative),
                                             nameof(CurrentInitiative));
        }

        private int m_BonusStrength;
        public int BonusStrength
        {
            get => m_BonusStrength;
            set => this.RaiseAndSetIfChanged(ref m_BonusStrength,
                                             value,
                                             nameof(BonusStrength),
                                             nameof(AugmentedStrength),
                                             nameof(AugmentedPhysicalLimit));
        }

        private int m_BonusCharisma;
        public int BonusCharisma
        {
            get => m_BonusCharisma;
            set => this.RaiseAndSetIfChanged(ref m_BonusCharisma,
                                             value,
                                             nameof(BonusCharisma),
                                             nameof(AugmentedCharisma),
                                             nameof(AugmentedSocialLimit));
        }

        private int m_BonusIntuition;
        public int BonusIntuition
        {
            get => m_BonusIntuition;
            set => this.RaiseAndSetIfChanged(ref m_BonusIntuition,
                                             value,
                                             nameof(BonusIntuition),
                                             nameof(AugmentedIntuition),
                                             nameof(AugmentedMentalLimit),
                                             nameof(AugmentedAstralLimit),
                                             nameof(AugmentedPhysicalInitiative),
                                             nameof(AugmentedAstralInitiative),
                                             nameof(AugmentedMatrixARInitiative),
                                             nameof(AugmentedMatrixColdInitiative),
                                             nameof(AugmentedMatrixHotInitiative),
                                             nameof(CurrentInitiative));
        }

        private int m_BonusLogic;
        public int BonusLogic
        {
            get => m_BonusLogic;
            set => this.RaiseAndSetIfChanged(ref m_BonusLogic,
                                             value,
                                             nameof(BonusLogic),
                                             nameof(AugmentedLogic),
                                             nameof(AugmentedMentalLimit));
        }

        private int m_BonusWillpower;
        public int BonusWillpower
        {
            get => m_BonusWillpower;
            set => this.RaiseAndSetIfChanged(ref m_BonusWillpower,
                                             value,
                                             nameof(BonusWillpower),
                                             nameof(AugmentedWillpower),
                                             nameof(AugmentedStunBoxes),
                                             nameof(AugmentedMentalLimit),
                                             nameof(AugmentedSocialLimit),
                                             nameof(AugmentedAstralLimit),
                                             nameof(WoundModifier),
                                             nameof(TotalPenalty),
                                             nameof(AugmentedPhysicalInitiative),
                                             nameof(AugmentedAstralInitiative),
                                             nameof(AugmentedMatrixARInitiative),
                                             nameof(AugmentedMatrixColdInitiative),
                                             nameof(AugmentedMatrixHotInitiative),
                                             nameof(CurrentInitiative));
        }

        public int AugmentedBody => m_Body + m_BonusBody;

        public int AugmentedAgility => m_Agility + m_BonusAgility;

        public int AugmentedReaction => m_Reaction + m_BonusReaction;

        public int AugmentedStrength => m_Strength + m_BonusStrength;

        public int AugmentedCharisma => m_Charisma + m_BonusCharisma;

        public int AugmentedIntuition => m_Intuition + m_BonusIntuition;

        public int AugmentedLogic => m_Logic + m_BonusLogic;

        public int AugmentedWillpower => m_Willpower + m_BonusWillpower;


        #endregion

        #region Special Attributes

        private int m_Edge;
        public int Edge
        {
            get => m_Edge;
            set => this.RaiseAndSetIfChanged(ref m_Edge, value);
        }

        private int m_Magic;
        public int Magic
        {
            get => m_Magic;
            set => this.RaiseAndSetIfChanged(ref m_Magic, value);
        }

        private int m_Resonance;
        public int Resonance
        {
            get => m_Resonance;
            set => this.RaiseAndSetIfChanged(ref m_Resonance, value);
        }


        private int m_BonusEdge;
        public int BonusEdge
        {
            get => m_BonusEdge;
            set => this.RaiseAndSetIfChanged(ref m_BonusEdge, value, nameof(BonusEdge), nameof(AugmentedEdge));
        }

        private int m_BonusMagic;
        public int BonusMagic
        {
            get => m_BonusMagic;
            set => this.RaiseAndSetIfChanged(ref m_BonusMagic, value, nameof(BonusMagic), nameof(AugmentedMagic));
        }

        private int m_BonusResonance;
        public int BonusResonance
        {
            get => m_BonusResonance;
            set => this.RaiseAndSetIfChanged(ref m_BonusResonance, value, nameof(BonusResonance), nameof(AugmentedResonance));
        }


        public int AugmentedEdge => m_Edge + m_BonusEdge;

        public int AugmentedMagic => m_Magic + m_BonusMagic;

        public int AugmentedResonance => m_Resonance + m_BonusResonance;

        #endregion

        #region Damage Track

        private int m_BonusPhysicalBoxes;
        public int BonusPhysicalBoxes
        {
            get => m_BonusPhysicalBoxes;
            set => this.RaiseAndSetIfChanged(ref m_BonusPhysicalBoxes,
                                             value,
                                             nameof(BonusPhysicalBoxes),
                                             nameof(AugmentedPhysicalBoxes),
                                             nameof(WoundModifier),
                                             nameof(TotalPenalty),
                                             nameof(AugmentedPhysicalInitiative),
                                             nameof(AugmentedAstralInitiative),
                                             nameof(AugmentedMatrixARInitiative),
                                             nameof(AugmentedMatrixColdInitiative),
                                             nameof(AugmentedMatrixHotInitiative),
                                             nameof(CurrentInitiative));
        }

        public int AugmentedPhysicalBoxes => DamageTrackBase + Convert.ToInt32(Math.Ceiling(AugmentedBody / 2f)) + m_BonusPhysicalBoxes;

        private int m_PhysicalDamage;
        public int PhysicalDamage
        {
            get => m_PhysicalDamage;
            set => this.RaiseAndSetIfChanged(ref m_PhysicalDamage,
                                             value,
                                             nameof(PhysicalDamage),
                                             nameof(WoundModifier),
                                             nameof(TotalPenalty),
                                             nameof(AugmentedPhysicalInitiative),
                                             nameof(AugmentedAstralInitiative),
                                             nameof(AugmentedMatrixARInitiative),
                                             nameof(AugmentedMatrixColdInitiative),
                                             nameof(AugmentedMatrixHotInitiative),
                                             nameof(CurrentInitiative));
        }

        private int m_BonusStunBoxes;
        public int BonusStunBoxes
        {
            get => m_BonusStunBoxes;
            set => this.RaiseAndSetIfChanged(ref m_BonusStunBoxes,
                                             value,
                                             nameof(BonusStunBoxes),
                                             nameof(AugmentedStunBoxes),
                                             nameof(WoundModifier),
                                             nameof(TotalPenalty),
                                             nameof(AugmentedPhysicalInitiative),
                                             nameof(AugmentedAstralInitiative),
                                             nameof(AugmentedMatrixARInitiative),
                                             nameof(AugmentedMatrixColdInitiative),
                                             nameof(AugmentedMatrixHotInitiative),
                                             nameof(CurrentInitiative));
        }

        public int AugmentedStunBoxes => DamageTrackBase + Convert.ToInt32(Math.Ceiling(AugmentedWillpower / 2f)) + m_BonusStunBoxes;

        private int m_StunDamage;
        public int StunDamage
        {
            get => m_StunDamage;
            set => this.RaiseAndSetIfChanged(ref m_StunDamage,
                                             value,
                                             nameof(StunDamage),
                                             nameof(WoundModifier),
                                             nameof(TotalPenalty),
                                             nameof(AugmentedPhysicalInitiative),
                                             nameof(AugmentedAstralInitiative),
                                             nameof(AugmentedMatrixARInitiative),
                                             nameof(AugmentedMatrixColdInitiative),
                                             nameof(AugmentedMatrixHotInitiative),
                                             nameof(CurrentInitiative));
        }

        private bool m_PainEditor;
        public bool PainEditor
        {
            get => m_PainEditor;
            set => this.RaiseAndSetIfChanged(ref m_PainEditor,
                                             value,
                                             nameof(PainEditor),
                                             nameof(WoundModifier),
                                             nameof(TotalPenalty),
                                             nameof(AugmentedPhysicalInitiative),
                                             nameof(AugmentedAstralInitiative),
                                             nameof(AugmentedMatrixARInitiative),
                                             nameof(AugmentedMatrixColdInitiative),
                                             nameof(AugmentedMatrixHotInitiative),
                                             nameof(CurrentInitiative));
        }

        private int m_PainResistence;
        public int PainResistence
        {
            get => m_PainResistence;
            set => this.RaiseAndSetIfChanged(ref m_PainResistence,
                                             value,
                                             nameof(PainResistence),
                                             nameof(WoundModifier),
                                             nameof(TotalPenalty),
                                             nameof(AugmentedPhysicalInitiative),
                                             nameof(AugmentedAstralInitiative),
                                             nameof(AugmentedMatrixARInitiative),
                                             nameof(AugmentedMatrixColdInitiative),
                                             nameof(AugmentedMatrixHotInitiative),
                                             nameof(CurrentInitiative));
        }

        public int WoundModifier => m_PainEditor
            ? 0
            : -(m_PhysicalDamage - m_PainResistence) / DamageTrackRow - (m_StunDamage - m_PainResistence) / DamageTrackRow;


        private int m_SpellsSustained;
        public int SpellsSustained
        {
            get => m_SpellsSustained;
            set => this.RaiseAndSetIfChanged(ref m_SpellsSustained,
                                             value,
                                             nameof(SpellsSustained),
                                             nameof(TotalPenalty),
                                             nameof(AugmentedPhysicalInitiative),
                                             nameof(AugmentedAstralInitiative),
                                             nameof(AugmentedMatrixARInitiative),
                                             nameof(AugmentedMatrixColdInitiative),
                                             nameof(AugmentedMatrixHotInitiative),
                                             nameof(CurrentInitiative));
        }

        public int TotalPenalty => WoundModifier + SpellsSustained * PenaltyPerSpell;

        #endregion

        #region Initiatives

        private int m_BonusPhysicalInitiative;
        public int BonusPhysicalInitiative
        {
            get => m_BonusPhysicalInitiative;
            set => this.RaiseAndSetIfChanged(ref m_BonusPhysicalInitiative,
                                             value,
                                             nameof(BonusPhysicalInitiative),
                                             nameof(AugmentedPhysicalInitiative),
                                             nameof(CurrentInitiative));
        }

        private int m_BonusPhysicalInitiativeDice;
        public int BonusPhysicalInitiativeDice
        {
            get => m_BonusPhysicalInitiativeDice;
            set => this.RaiseAndSetIfChanged(ref m_BonusPhysicalInitiativeDice,
                                             value,
                                             nameof(BonusPhysicalInitiativeDice),
                                             nameof(AugmentedPhysicalInitiativeDice),
                                             nameof(CurrentInitiativeDice));
        }

        public int AugmentedPhysicalInitiative => AugmentedReaction + AugmentedIntuition + m_BonusPhysicalInitiative + TotalPenalty;

        public int AugmentedPhysicalInitiativeDice => BasePhysicalInitiativeDice + m_BonusPhysicalInitiativeDice;


        private int m_BonusAstralInitiative;
        public int BonusAstralInitiative
        {
            get => m_BonusAstralInitiative;
            set => this.RaiseAndSetIfChanged(ref m_BonusAstralInitiative,
                                             value,
                                             nameof(BonusAstralInitiative),
                                             nameof(AugmentedAstralInitiative),
                                             nameof(CurrentInitiative));
        }

        private int m_BonusAstralInitiativeDice;
        public int BonusAstralInitiativeDice
        {
            get => m_BonusAstralInitiativeDice;
            set => this.RaiseAndSetIfChanged(ref m_BonusAstralInitiativeDice,
                                             value,
                                             nameof(BonusAstralInitiativeDice),
                                             nameof(AugmentedAstralInitiativeDice),
                                             nameof(CurrentInitiativeDice));
        }

        public int AugmentedAstralInitiative => AugmentedIntuition * 2 + BonusAstralInitiative + TotalPenalty;
        public int AugmentedAstralInitiativeDice => BaseAstralInitiativeDice + m_BonusAstralInitiativeDice;


        private int m_BonusMatrixARInitiative;
        public int BonusMatrixARInitiative
        {
            get => m_BonusMatrixARInitiative;
            set => this.RaiseAndSetIfChanged(ref m_BonusMatrixARInitiative,
                                             value,
                                             nameof(BonusMatrixARInitiative),
                                             nameof(AugmentedMatrixARInitiative),
                                             nameof(CurrentInitiative));
        }

        private int m_BonusMatrixARInitiativeDice;
        public int BonusMatrixARInitiativeDice
        {
            get => m_BonusMatrixARInitiativeDice;
            set => this.RaiseAndSetIfChanged(ref m_BonusMatrixARInitiativeDice,
                                             value,
                                             nameof(BonusMatrixARInitiativeDice),
                                             nameof(AugmentedMatrixARInitiativeDice),
                                             nameof(CurrentInitiativeDice));
        }

        public int AugmentedMatrixARInitiative => AugmentedPhysicalInitiative + m_BonusMatrixARInitiative; // Penalty already applied to Physical Initiative

        public int AugmentedMatrixARInitiativeDice => AugmentedPhysicalInitiativeDice + m_BonusMatrixARInitiativeDice;


        private int m_BonusMatrixColdInitiative;
        public int BonusMatrixColdInitiative
        {
            get => m_BonusMatrixColdInitiative;
            set => this.RaiseAndSetIfChanged(ref m_BonusMatrixColdInitiative,
                                             value,
                                             nameof(BonusMatrixColdInitiative),
                                             nameof(AugmentedMatrixColdInitiative),
                                             nameof(CurrentInitiative));
        }

        private int m_BonusMatrixColdInitiativeDice;
        public int BonusMatrixColdInitiativeDice
        {
            get => m_BonusMatrixColdInitiativeDice;
            set => this.RaiseAndSetIfChanged(ref m_BonusMatrixColdInitiativeDice,
                                             value,
                                             nameof(BonusMatrixColdInitiativeDice),
                                             nameof(AugmentedMatrixColdInitiativeDice),
                                             nameof(CurrentInitiativeDice));
        }

        public int AugmentedMatrixColdInitiative => AugmentedIntuition + m_BonusMatrixColdInitiative + TotalPenalty;

        public int AugmentedMatrixColdInitiativeDice => BaseMatrixColdInitiativeDice + m_BonusMatrixColdInitiativeDice;


        private int m_BonusMatrixHotInitiative;
        public int BonusMatrixHotInitiative
        {
            get => m_BonusMatrixHotInitiative;
            set => this.RaiseAndSetIfChanged(ref m_BonusMatrixHotInitiative,
                                             value,
                                             nameof(BonusMatrixHotInitiative),
                                             nameof(AugmentedMatrixHotInitiative),
                                             nameof(CurrentInitiative));
        }

        private int m_BonusMatrixHotInitiativeDice;
        public int BonusMatrixHotInitiativeDice
        {
            get => m_BonusMatrixHotInitiativeDice;
            set => this.RaiseAndSetIfChanged(ref m_BonusMatrixHotInitiativeDice,
                                             value,
                                             nameof(BonusMatrixHotInitiativeDice),
                                             nameof(AugmentedMatrixHotInitiativeDice),
                                             nameof(CurrentInitiativeDice));
        }

        public int AugmentedMatrixHotInitiative => AugmentedIntuition + m_BonusMatrixHotInitiative + TotalPenalty;

        public int AugmentedMatrixHotInitiativeDice => BaseMatrixHotInitiativeDice + m_BonusMatrixHotInitiativeDice;

        private InitiativeState m_CurrentState;
        public InitiativeState CurrentState
        {
            get => m_CurrentState;
            set
            {
                this.RaiseAndSetIfChanged(ref m_CurrentState,
                                          value,
                                          nameof(CurrentState),
                                          nameof(CurrentInitiative),
                                          nameof(CurrentInitiativeDice));
            }
        }

        public int CurrentInitiative =>
            CurrentState switch
            {
                InitiativeState.Physical => AugmentedPhysicalInitiative,
                InitiativeState.Astral => AugmentedAstralInitiative,
                InitiativeState.MatrixAR => AugmentedMatrixARInitiative,
                InitiativeState.MatrixCold => AugmentedMatrixColdInitiative,
                InitiativeState.MatrixHot => AugmentedMatrixHotInitiative,
                _ => 0
            };

        public int CurrentInitiativeDice =>
            CurrentState switch
            {
                InitiativeState.Physical => AugmentedPhysicalInitiativeDice,
                InitiativeState.Astral => AugmentedAstralInitiativeDice,
                InitiativeState.MatrixAR => AugmentedMatrixARInitiativeDice,
                InitiativeState.MatrixCold => AugmentedMatrixColdInitiativeDice,
                InitiativeState.MatrixHot => AugmentedMatrixHotInitiativeDice,
                _ => 0
            };

        #endregion

        #region Limits

        private int m_BonusPhysicalLimit;
        public int BonusPhysicalLimit
        {
            get => m_BonusPhysicalLimit;
            set => this.RaiseAndSetIfChanged(ref m_BonusPhysicalLimit, value, nameof(BonusPhysicalLimit), nameof(AugmentedPhysicalLimit));
        }

        public int AugmentedPhysicalLimit => Convert.ToInt32(Math.Ceiling((AugmentedStrength * 2.0 + AugmentedBody + AugmentedReaction) / 3.0)) + m_BonusPhysicalLimit;


        private int m_BonusMentalLimit;
        public int BonusMentalLimit
        {
            get => m_BonusMentalLimit;
            set => this.RaiseAndSetIfChanged(ref m_BonusMentalLimit, value, nameof(BonusMentalLimit), nameof(AugmentedMentalLimit));
        }

        public int AugmentedMentalLimit => Convert.ToInt32(Math.Ceiling((AugmentedLogic * 2.0 + AugmentedIntuition + AugmentedWillpower) / 3.0)) + m_BonusMentalLimit;

        private int m_BonusSocialLimit;
        public int BonusSocialLimit
        {
            get => m_BonusSocialLimit;
            set => this.RaiseAndSetIfChanged(ref m_BonusSocialLimit, value, nameof(BonusSocialLimit), nameof(AugmentedSocialLimit));
        }

        public int AugmentedSocialLimit => Convert.ToInt32(Math.Ceiling((AugmentedCharisma * 2.0 + AugmentedWillpower + Math.Ceiling(Convert.ToDouble(Essence))) / 3.0)) + m_BonusMentalLimit;

        private int m_BonusAstralLimit;
        public int BonusAstralLimit
        {
            get => m_BonusAstralLimit;
            set => this.RaiseAndSetIfChanged(ref m_BonusAstralLimit, value, nameof(BonusAstralLimit), nameof(AugmentedAstralLimit));
        }

        public int AugmentedAstralLimit => Math.Max(AugmentedMentalLimit, AugmentedSocialLimit) + m_BonusAstralLimit;

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
                    roll.DiceUsed = AugmentedPhysicalInitiativeDice;
                    roll.ScoreUsed = AugmentedPhysicalInitiative;
                    break;
                case InitiativeState.Astral:
                    roll.DiceUsed = AugmentedAstralInitiativeDice;
                    roll.ScoreUsed = AugmentedAstralInitiative;
                    break;
                case InitiativeState.MatrixAR:
                    roll.DiceUsed = AugmentedMatrixARInitiativeDice;
                    roll.ScoreUsed = AugmentedMatrixARInitiative;
                    break;
                case InitiativeState.MatrixCold:
                    roll.DiceUsed = AugmentedMatrixColdInitiativeDice;
                    roll.ScoreUsed = AugmentedMatrixColdInitiative;
                    break;
                case InitiativeState.MatrixHot:
                    roll.DiceUsed = AugmentedMatrixHotInitiativeDice;
                    roll.ScoreUsed = AugmentedMatrixHotInitiative;
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
            if (total > AugmentedStunBoxes)
            {
                StunDamage = AugmentedStunBoxes;
                ApplyPhysicalDamage(total - AugmentedStunBoxes);
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
