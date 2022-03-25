using System;
using System.Collections.Generic;
using System.Text;

namespace ShadowrunTracker.Contract.Data
{
    public interface ICharacter
    {
        string Alias { get; set; }
        bool IsPlayer { get; set; }
        string Player { get; set; }

        decimal Essence { get; set; }

        #region Attributes

        int Body { get; set; }
        int Agility { get; set; }
        int Reaction { get; set; }
        int Strength { get; set; }
        int Charisma { get; set; }
        int Intuition { get; set; }
        int Logic { get; set; }
        int Willpower { get; set; }

        int BonusBody { get; set; }
        int BonusAgility { get; set; }
        int BonusReaction { get; set; }
        int BonusStrength { get; set; }
        int BonusCharisma { get; set; }
        int BonusIntuition { get; set; }
        int BonusLogic { get; set; }
        int BonusWillpower { get; set; }

        int AugmentedBody { get; }
        int AugmentedAgility { get; }
        int AugmentedReaction { get; }
        int AugmentedStrength { get; }
        int AugmentedCharisma { get; }
        int AugmentedIntuition { get; }
        int AugmentedLogic { get; }
        int AugmentedWillpower { get; }

        #endregion

        #region Special Attributes

        int Edge { get; set; }
        int Magic { get; set; }
        int Resonance { get; set; }

        int BonusEdge { get; set; }
        int BonusMagic { get; set; }
        int BonusResonance { get; set; }

        int AugmentedEdge { get; }
        int AugmentedMagic { get; }
        int AugmentedResonance { get; }

        #endregion

        #region Damage Track

        int BonusPhysicalBoxes { get; set; }
        int AugmentedPhysicalBoxes { get; }
        int PhysicalDamage { get; }

        int BonusStunBoxes { get; set; }
        int AugmentedStunBoxes { get; }
        int StunDamage { get; }

        bool PainEditor { get; set; }
        int PainResistence { get; set; }

        int WoundModifier { get; }
        int SpellsSustained { get; set; }

        int TotalPenalty { get; }

        #endregion

        #region Initiatives

        int BonusPhysicalInitiative { get; set; }
        int BonusPhysicalInitiativeDice { get; set; }
        int AugmentedPhysicalInitiative { get; }
        int AugmentedPhysicalInitiativeDice { get; }

        int BonusAstralInitiative { get; set; }
        int BonusAstralInitiativeDice { get; set; }
        int AugmentedAstralInitiative { get; }
        int AugmentedAstralInitiativeDice { get; }

        int BonusMatrixARInitiative { get; set; }
        int BonusMatrixARInitiativeDice { get; set; }
        int AugmentedMatrixARInitiative { get; }
        int AugmentedMatrixARInitiativeDice { get; }

        int BonusMatrixColdInitiative { get; set; }
        int BonusMatrixColdInitiativeDice { get; set; }
        int AugmentedMatrixColdInitiative { get; }
        int AugmentedMatrixColdInitiativeDice { get; }

        int BonusMatrixHotInitiative { get; set; }
        int BonusMatrixHotInitiativeDice { get; set; }
        int AugmentedMatrixHotInitiative { get; }
        int AugmentedMatrixHotInitiativeDice { get; }

        #endregion

        #region Limits

        int BonusPhysicalLimit { get; set; }
        int AugmentedPhysicalLimit { get; }

        int BonusMentalLimit { get; set; }
        int AugmentedMentalLimit { get; }

        int BonusSocialLimit { get; set; }
        int AugmentedSocialLimit { get; }

        int BonusAstralLimit { get; set; }
        int AugmentedAstralLimit { get; }

        #endregion

        #region Skills

        List<ISkill> Skills { get; }

        #endregion

        #region Improvements

        List<IImprovement> Improvements { get; }

        #endregion
    }
}
