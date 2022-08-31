namespace ShadowrunTracker.ViewModels
{
    using ShadowrunTracker.Data;
    using System;

    public class PlayerEncounterViewModel : EncounterViewModelBase, IPlayerEncounterViewModel
    {
        public PlayerEncounterViewModel(IViewModelFactory viewModelFactory, IDataStore<Guid> store, Encounter record)
            : base(viewModelFactory, store, record)
        {
        }

    }
}
