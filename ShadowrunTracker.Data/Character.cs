using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ShadowrunTracker.Data
{
    [DataContract]
    public class Character : ICharacter
    {
        [DataMember]
        public Guid Id { get; set; }

        [DataMember]
        public string Alias { get; set; } = string.Empty;

        [DataMember]
        public bool IsPlayer { get; set; }

        [DataMember]
        public string Player { get; set; } = string.Empty;

        [DataMember]
        public decimal Essence { get; set; }

        [DataMember]
        public int BaseBody { get; set; }

        [DataMember]
        public int BaseAgility { get; set; }

        [DataMember]
        public int BaseReaction { get; set; }

        [DataMember]
        public int BaseStrength { get; set; }

        [DataMember]
        public int BaseCharisma { get; set; }

        [DataMember]
        public int BaseIntuition { get; set; }

        [DataMember]
        public int BaseLogic { get; set; }

        [DataMember]
        public int BaseWillpower { get; set; }

        [DataMember]
        public int Edge { get; set; }

        [DataMember]
        public int Magic { get; set; }

        [DataMember]
        public int Resonance { get; set; }

        //[DataMember]
        //public int BonusPhysicalBoxes { get; set; }

        //[DataMember]
        //public int BonusStunBoxes { get; set; }

        [DataMember]
        public bool PainEditor { get; set; }

        [DataMember]
        public int PainResistence { get; set; }

        [DataMember]
        public int SpellsSustained { get; set; }

        [DataMember]
        public List<ISkill> Skills { get; set; } = new();

        [DataMember]
        public List<IImprovement> Improvements { get; set; } = new();
    }
}
