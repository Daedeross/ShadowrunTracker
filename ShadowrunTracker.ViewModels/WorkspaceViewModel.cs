using Castle.Core;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace ShadowrunTracker.ViewModels
{
    public class WorkspaceViewModel : ViewModelBase, IWorkspaceViewModel
    {
        private readonly IViewModelFactory _viewModelFactory;

        private IEncounterViewModel? m_CurrentEncounter;
        public IEncounterViewModel? CurrentEncounter
        {
            get => m_CurrentEncounter;
            set => this.RaiseAndSetIfChanged(ref m_CurrentEncounter, value);
        }

        public ICommand NewEncounter { get; }

        public WorkspaceViewModel(IViewModelFactory viewModelFactory)
        {
            _viewModelFactory = viewModelFactory;

            var newEncounter = ReactiveCommand.Create(NewEncounterExecute);

            _disposables.Add(newEncounter);

            NewEncounter = newEncounter;
        }

        private void NewEncounterExecute()
        {
            CurrentEncounter = _viewModelFactory.Create<IEncounterViewModel>();
        }
    }
}
