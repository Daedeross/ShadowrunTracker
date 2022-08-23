namespace ShadowrunTracker.Data.Traits
{
    using System.Runtime.Serialization;

    [DataContract]
    public class LeveledTrait : Trait
    {
        [DataMember]
        public int BaseRating { get; set; }
        [DataMember]
        public int BonusRating { get; set; }
    }
}
