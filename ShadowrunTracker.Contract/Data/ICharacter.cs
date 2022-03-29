using System;
using System.Collections.Generic;
using System.Text;

namespace ShadowrunTracker.Data
{
    public interface ICharacter
    {
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

        #endregion

        #region Special Attributes

        int Edge { get; set; }
        int Magic { get; set; }
        int Resonance { get; set; }

        #endregion

        #region Damage Track

        //int BonusPhysicalBoxes { get; set; }
        //int BonusStunBoxes { get; set; }
        bool PainEditor { get; set; }
        int PainResistence { get; set; }
        int SpellsSustained { get; set; }

        #endregion

        #region Skills

        List<ISkill> Skills { get; }

        #endregion

        #region Improvements

        List<IImprovement> Improvements { get; }

        #endregion
    }
}
