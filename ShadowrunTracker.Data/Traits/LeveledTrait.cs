namespace ShadowrunTracker.Data.Traits
{
    using System.Runtime.Serialization;

    [DataContract]
    public class LeveledTrait : Trait
    {
        [DataMember]
        public int Rating { get; set; }
        //[DataMember]
        //public int BonusRating { get; set; }
    }
}
