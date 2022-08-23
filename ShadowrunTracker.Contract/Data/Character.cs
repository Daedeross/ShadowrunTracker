namespace ShadowrunTracker.Data
{
    using ShadowrunTracker.Data.Traits;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    public class Character : RecordBase
    {
        [DataMember]
        public string? Alias { get; set; }

        [DataMember]
        public bool IsPlayer { get; set; }

        [DataMember]
        public string? Player { get; set; }

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

        [DataMember]
        public bool PainEditor { get; set; }

        [DataMember]
        public int PainResistence { get; set; }

        [DataMember]
        public int SpellsSustained { get; set; }

        [DataMember]
        public List<Skill> Skills { get; set; } = new List<Skill>();

        [DataMember]
        public List<Improvement> Improvements { get; set; } = new List<Improvement>();
    }
}
