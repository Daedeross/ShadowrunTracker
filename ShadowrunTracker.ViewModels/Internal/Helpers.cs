namespace ShadowrunTracker.ViewModels.Internal
{
    using System;

    internal static class Helpers
    {
        public static IParticipantInitiativeViewModel ToParticipant(this IPendingParticipantInitiativeViewModel pending, IDataStore<Guid> store)
        {
            if (pending.InitiativeRoll is null)
            {
                throw new ArgumentNullException(nameof(pending.InitiativeRoll));
            }

            return new ParticipantInitiativeViewModel(store, pending.Character, pending.InitiativeRoll)
            {
                SeizedInitiative = pending.SiezeInitiative
            };
        }
    }
}
