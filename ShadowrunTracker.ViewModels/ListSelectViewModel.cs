namespace ShadowrunTracker.ViewModels
{
    using ReactiveUI;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Input;

    public class ListSelectViewModel : DisposableModalViewModelBase<string?>, IListSelectViewModel
    {
        public ListSelectViewModel(string header, Func<CancellationToken, Task<IList<string>>>? refresh = null)
        {
            m_ListRefresh = refresh;
            m_Header = header;

            var okCanExecute = this.WhenAnyValue(x => x.SelectedItem, s => !string.IsNullOrWhiteSpace(s));
            var refreshCanExecute = this.WhenAnyValue(x => x.ListRefresh)
                .Select(f => f != null);

            OkCommand = ReactiveCommand.Create(OnOk, okCanExecute)
                .DisposeWith(_disposables);
            Refresh = ReactiveCommand.CreateFromTask(RefreshAsync, refreshCanExecute)
                .DisposeWith(_disposables);
        }

        private Func<CancellationToken, Task<IList<string>>>? m_ListRefresh;
        public Func<CancellationToken, Task<IList<string>>>? ListRefresh
        {
            get => m_ListRefresh;
            set => this.RaiseAndSetIfChanged(ref m_ListRefresh, value);
        }

        private string m_Header;
        public string Header
        {
            get => m_Header;
            set => this.RaiseAndSetIfChanged(ref m_Header, value);
        }

        private List<string> _list = new();
        public ICollection<string> List => _list;

        private string? m_SelectedItem;
        public string? SelectedItem
        {
            get => m_SelectedItem;
            set => this.RaiseAndSetIfChanged(ref m_SelectedItem, value);
        }

        public ICommand Refresh { get; }

        public async Task RefreshAsync(CancellationToken cancellationToken)
        {
            if (m_ListRefresh is null)
            {
                return;
            }
            var list = await m_ListRefresh(cancellationToken);
            _list = list.ToList(); // make copy of list

            this.RaisePropertyChanged(nameof(List));
        }

        protected override void OnCancel()
        {
            m_Complete.OnNext(null);
            m_Complete.OnCompleted();
        }

        protected override void OnOk()
        {
            m_Complete.OnNext(SelectedItem);    // null-checked by canExecute
            m_Complete.OnCompleted();
        }
    }
}
