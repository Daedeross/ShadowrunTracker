namespace ShadowrunTracker.Data.Traits
{
    using System.Runtime.Serialization;

    [DataContract]
    [TypeDiscriminator(7)]
    public class LeveledTrait : Trait
    {
        [DataMember]
        public int BaseRating { get; set; }
        [DataMember]
        public int BonusRating { get; set; }
    }
}
