using System;
using System.Runtime.Serialization;

namespace ShadowrunTracker.Model
{
    [DataContract]
    public record DelayAction // : IEquatable<DelayAction>
    {
        [DataMember]
        public string? Name { get; init; }
        [DataMember]
        public int Value { get; init; }

        //public bool Equals(DelayAction other)
        //{
        //    if (ReferenceEquals(this, other))
        //    {
        //        return true;
        //    }
        //    if(other is null)
        //    {
        //        return false;
        //    }
        //    return Equals(Name, other.Name)
        //        && Equals(InitiativeCost, other.InitiativeCost);
        //}
    }
}
