using System;

namespace ShadowrunTracker.ViewModels.Internal
{
    internal static class Helpers
    {
        public static IParticipantInitiativeViewModel ToParticipant(this IPendingParticipantInitiativeViewModel pending)
        {
            if (pending.InitiativeRoll is null)
            {
                throw new ArgumentNullException(nameof(pending.InitiativeRoll));
            }

            return new ParticipantInitiativeViewModel(pending.Character, pending.InitiativeRoll)
            {
                SeizedInitiative = pending.SiezeInitiative
            };
        }
    }
}
