namespace ShadowrunTracker.ViewModels.Internal
{
    using ShadowrunTracker.Data;
    using System;

    internal static class Helpers
    {
        public static IParticipantInitiativeViewModel ToParticipant(this IPendingParticipantInitiativeViewModel pending, IViewModelFactory factory)
        {
            if (pending.InitiativeRoll is null)
            {
                throw new ArgumentNullException(nameof(pending.InitiativeRoll));
            }

            return factory.Create(pending.Character, new ParticipantInitiative { InitiativeRoll = pending.InitiativeRoll });
        }
    }
}
