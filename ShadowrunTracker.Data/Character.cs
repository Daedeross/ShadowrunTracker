using ShadowrunTracker.Data.Traits;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace ShadowrunTracker.Data
{
    [DataContract]
    public class Character : ICharacter
    {
        [DataMember]
        public Guid Id { get; set; }

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

        [DataMember(Name = nameof(Skills))]
        public List<Skill> _skills { get; set; } = new();
        [IgnoreDataMember]
        public List<ISkill> Skills
        {
            get => _skills.Cast<ISkill>().ToList();
            set => _skills = value.Select(s => new Skill(s)).ToList();
        }

        [DataMember(Name = nameof(Improvements))]
        public List<Improvement> _improvements { get; set; } = new();

        [IgnoreDataMember]
        public List<IImprovement> Improvements
        {
            get => _improvements.Cast<IImprovement>().ToList();
            set => _improvements = value.Select(s => new Improvement(s)).ToList();
        }
    }
}
