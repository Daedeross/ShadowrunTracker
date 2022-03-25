using ShadowrunTracker.Contract.Model;

namespace ShadowrunTracker.Contract.ViewModels
{
    public interface IImprovementViewModel : IViewModel
    {
        /// <summary>
        /// The name of the imporvement. For display purposes only.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// The type of trait the imporvement targets (Attribute or Skill)
        /// </summary>
        TraitKind TargetKind { get; set; }

        /// <summary>
        /// The name of the target Trait
        /// </summary>
        string Target { get; set; }

        /// <summary>
        /// The bonus (positive) or penalty (negative) to apply to the target trait.
        /// </summary>
        int Value { get; set; }

        /// <summary>
        /// True if the improvement has a valid target and is applied.
        /// </summary>
        bool IsValid { get; }

        /// <summary>
        /// To be called when the improvement is added to the character
        /// </summary>
        /// <param name="character"></param>
        /// <returns>True if the improvement is valid and applied, otherwise false.</returns>
        bool OnAdd(ICharacterViewModel character);

        /// <summary>
        /// To be called when the improvement is added to the character
        /// </summary>
        /// <param name="character"></param>
        void OnRemove(ICharacterViewModel character);
    }
}
