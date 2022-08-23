namespace ShadowrunTracker.ViewModels
{
    using ReactiveUI;
    using System;
    using System.Reactive.Disposables;
    using System.Windows.Input;

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

            NewEncounter = ReactiveCommand.Create(NewEncounterExecute)
                .DisposeWith(_disposables);
        }

        private void NewEncounterExecute()
        {
            var encounter = _viewModelFactory.Create<IEncounterViewModel>();
            if (encounter is IDisposable disposable)
            {
                _disposables.Add(disposable);
            }

            if (CurrentEncounter is IDisposable oldEncounter)
            {
                _disposables.Remove(oldEncounter);
                oldEncounter.Dispose();
            }

            CurrentEncounter = encounter;
        }
    }
}
