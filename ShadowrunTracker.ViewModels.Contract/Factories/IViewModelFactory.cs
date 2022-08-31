namespace ShadowrunTracker
{
    using DynamicData.Binding;
    using ShadowrunTracker.Data;
    using ShadowrunTracker.Model;
    using ShadowrunTracker.ViewModels;
    using System.Collections.Generic;

    public interface IViewModelFactory
    {
        T Create<T>() where T : class, IViewModel;

        IParticipantInitiativeViewModel Create(ICharacterViewModel character, ParticipantInitiative record);

        IRequestInitiativesViewModel Create(IObservableCollection<ICharacterViewModel> characters);

        ICombatRoundViewModel CreateRound(IEnumerable<IParticipantInitiativeViewModel>? participants = null, CombatRound? record = null);

        IInitiativePassViewModel CreatePass(IEnumerable<IParticipantInitiativeViewModel>? participants = null, InitiativePass? record = null);

        TViewModel Create<TViewModel, TRecord>(TRecord record)
            where TViewModel : IRecordViewModel<TRecord>
            where TRecord : RecordBase;

        public void Release(IViewModel viewModel);
    }
}
