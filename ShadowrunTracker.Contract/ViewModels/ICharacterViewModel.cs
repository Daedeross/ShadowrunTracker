using ShadowrunTracker.Data;
using ShadowrunTracker.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Text;

namespace ShadowrunTracker.ViewModels
{
    public interface ICharacterViewModel : IViewModel, ICanSave
    {
        Guid Id { get; }

        string Alias { get; set; }
        bool IsPlayer { get; set; }
        string Player { get; set; }

        decimal Essence { get; set; }

        #region Attributes

        int BaseBody { get; set; }
        int BaseAgility { get; set; }
        int BaseReaction { get; set; }
        int BaseStrength { get; set; }
        int BaseCharisma { get; set; }
        int BaseIntuition { get; set; }
        int BaseLogic { get; set; }
        int BaseWillpower { get; set; }

        int BonusBody { get; set; }
        int BonusAgility { get; set; }
        int BonusReaction { get; set; }
        int BonusStrength { get; set; }
        int BonusCharisma { get; set; }
        int BonusIntuition { get; set; }
        int BonusLogic { get; set; }
        int BonusWillpower { get; set; }

        int Body { get; }
        int Agility { get; }
        int Reaction { get; }
        int Strength { get; }
        int Charisma { get; }
        int Intuition { get; }
        int Logic { get; }
        int Willpower { get; }

        #endregion

        #region Special Attributes

        int BaseEdge { get; set; }
        int BaseMagic { get; set; }
        int BaseResonance { get; set; }

        int BonusEdge { get; set; }
        int BonusMagic { get; set; }
        int BonusResonance { get; set; }

        int Edge { get; }
        int Magic { get; }
        int Resonance { get; }

        int EdgePoints { get; set; }

        #endregion

        #region Damage Track

        int BonusPhysicalBoxes { get; set; }
        int PhysicalBoxes { get; }
        int PhysicalDamage { get; }

        int BonusStunBoxes { get; set; }
        int StunBoxes { get; }
        int StunDamage { get; }

        bool PainEditor { get; set; }
        int PainResistence { get; set; }

        int WoundModifier { get; }
        int SpellsSustained { get; set; }

        int TotalPenalty { get; }

        //ObservableCollection<bool> PhysicalDamageTrack { get; }

        //ObservableCollection<bool> StunDamageTrack { get; }

        #endregion

        #region Initiatives

        int BonusPhysicalInitiative { get; set; }
        int BonusPhysicalInitiativeDice { get; set; }
        int PhysicalInitiative { get; }
        int PhysicalInitiativeDice { get; }

        int BonusAstralInitiative { get; set; }
        int BonusAstralInitiativeDice { get; set; }
        int AstralInitiative { get; }
        int AstralInitiativeDice { get; }

        int BonusMatrixARInitiative { get; set; }
        int BonusMatrixARInitiativeDice { get; set; }
        int MatrixARInitiative { get; }
        int MatrixARInitiativeDice { get; }

        int BonusMatrixColdInitiative { get; set; }
        int BonusMatrixColdInitiativeDice { get; set; }
        int MatrixColdInitiative { get; }
        int MatrixColdInitiativeDice { get; }

        int BonusMatrixHotInitiative { get; set; }
        int BonusMatrixHotInitiativeDice { get; set; }
        int MatrixHotInitiative { get; }
        int MatrixHotInitiativeDice { get; }

        InitiativeState CurrentState { get; set; }

        /// <summary>
        /// Get the current flat initiative value based on <see cref="CurrentState"/>.
        /// </summary>
        int CurrentInitiative { get; }
        /// <summary>
        /// Get the current number of initiative dice based on <see cref="CurrentState"/>
        /// </summary>
        int CurrentInitiativeDice { get; }

        #endregion

        #region Limits

        int BonusPhysicalLimit { get; set; }
        int PhysicalLimit { get; }

        int BonusMentalLimit { get; set; }
        int MentalLimit { get; }

        int BonusSocialLimit { get; set; }
        int SocialLimit { get; }

        int BonusAstralLimit { get; set; }
        int AstralLimit { get; }

        #endregion

        #region Skills

        ObservableCollection<ISkillViewModel> Skills { get; }

        #endregion

        #region Improvements

        ObservableCollection<IImprovementViewModel> Improvements { get; }

        #endregion

        #region Methods

        InitiativeRoll RollInitiative(bool blitz = false);

        void ApplyPhysicalDamage(int damage);

        void ApplyStunDamage(int damage);


        void ApplyPhysicalHealing(int healing);

        void ApplyStunHealing(int healing);

        #endregion
    }
}
