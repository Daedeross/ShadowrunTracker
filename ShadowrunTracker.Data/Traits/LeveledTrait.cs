using ShadowrunTracker.Data;
using System.Runtime.Serialization;

namespace ShadowrunTracker.Data.Traits
{
    [DataContract]
    public class LeveledTrait : Trait, ILeveledTrait
    {
        [DataMember]
        public int Rating { get; set; }
        //[DataMember]
        //public int BonusRating { get; set; }
    }
}
