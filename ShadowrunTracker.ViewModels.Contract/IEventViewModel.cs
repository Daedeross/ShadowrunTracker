namespace ShadowrunTracker.ViewModels
{
    using ShadowrunTracker.Model;

    /// <summary>
    /// Represents a singe effect of an event on a character.
    /// </summary>
    public interface IEventViewModel : IViewModel
    {
        /// <summary>
        /// The kind of event to record.
        /// </summary>
        EventKind EventKind { get; set; }

        /// <summary>
        /// The name of the event, if any
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// The character the event applies to.
        /// </summary>
        ICharacterViewModel Character { get; set; }

        /// <summary>
        /// The kind of trait targeted, if any.
        /// </summary>
        TraitKind TargetKind { get; set; }

        /// <summary>
        /// The name of the trait targeted, if any.
        /// </summary>
        string Target { get; set; }
    }
}
