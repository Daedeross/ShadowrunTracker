﻿using ShadowrunTracker.Model;
using System.Runtime.Serialization;

namespace ShadowrunTracker.Data
{
    [DataContract]
    public class Improvement : IImprovement
    {
        [DataMember]
        public string Name { get; set; } = string.Empty;

        [DataMember]
        public TraitKind TargetKind { get; set; }

        [DataMember]
        public string Target { get; set; } = string.Empty;

        [DataMember]
        public int Value { get; set; }
    }
}
