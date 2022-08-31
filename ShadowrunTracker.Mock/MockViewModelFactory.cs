using DynamicData.Binding;
using ShadowrunTracker.Data;
using ShadowrunTracker.ViewModels;
using System;
using System.Collections.Generic;

namespace ShadowrunTracker.Mock
{
    public class MockViewModelFactory : IViewModelFactory
    {
        public static IViewModelFactory Instance { get; } = new MockViewModelFactory();

        public IRequestInitiativesViewModel Create(IObservableCollection<ICharacterViewModel> characters)
        {
            throw new NotImplementedException();
        }

        T IViewModelFactory.Create<T>()
        {
            throw new NotImplementedException();
        }

        public ICombatRoundViewModel CreateRound(IEnumerable<IParticipantInitiativeViewModel> characters)
        {
            return new MockCombatRoundViewModel();
        }

        public void Release(IViewModel viewModel)
        {
            if (viewModel is IDisposable d)
            {
                d.Dispose();
            }
        }

        public TViewModel Create<TViewModel, TRecord>(TRecord record)
            where TViewModel : IRecordViewModel<TRecord>
            where TRecord : RecordBase
        {
            throw new NotImplementedException();
        }

        public IPlayerEncounterViewModel CreatePlayerEncounter(Encounter encounter)
        {
            throw new NotImplementedException();
        }

        public IInitiativePassViewModel CreatePass(IEnumerable<IParticipantInitiativeViewModel> participants)
        {
            throw new NotImplementedException();
        }

        public IParticipantInitiativeViewModel Create(ICharacterViewModel character, ParticipantInitiative record)
        {
            throw new NotImplementedException();
        }

        public ICombatRoundViewModel CreateRound(IEnumerable<IParticipantInitiativeViewModel>? participants = null, CombatRound? record = null)
        {
            throw new NotImplementedException();
        }

        public IInitiativePassViewModel CreatePass(IEnumerable<IParticipantInitiativeViewModel>? participants = null, InitiativePass? record = null)
        {
            throw new NotImplementedException();
        }
    }
}
