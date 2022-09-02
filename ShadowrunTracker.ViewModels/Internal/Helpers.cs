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

            var roll = pending.InitiativeRoll;
            roll.SiezedInitiative = pending.SiezeInitiative;
            roll.Blitzed = pending.Blitz;

            return factory.Create(pending.Character, new ParticipantInitiative { InitiativeRoll = roll });
        }
    }
}
