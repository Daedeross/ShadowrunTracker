namespace ShadowrunTracker.ViewModels
{
    using DynamicData;
    using ReactiveUI;
    using ShadowrunTracker.Model;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reactive.Linq;

    public class DataEntryModalViewModel : DisposableModalViewModelBase<IList<object?>?>, IDataEntryModalViewModel
    {
        public DataEntryModalViewModel(string header, IList<EntryDatum> options)
        {
            Data = options.Select(d => new EntryDatumViewModel(d)).ToList<IEntryDatumViewModel>();
            m_Header = header;

            var dataValid = Data.AsObservableChangeSet()
                .AutoRefresh(vm => vm.IsValid)
                .ToCollection()
                .Select(c => c.All(vm => vm.IsValid));

            OkCommand = ReactiveCommand.Create(OnOk, dataValid, RxApp.MainThreadScheduler);
        }

        private string m_Header;
        public string Header
        {
            get => m_Header;
            set => this.RaiseAndSetIfChanged(ref m_Header, value);
        }

        public IList<IEntryDatumViewModel> Data { get; }

        protected override void OnCancel()
        {
            m_Complete.OnNext(null);
            m_Complete.OnCompleted();
        }

        protected override void OnOk()
        {
            m_Complete.OnNext(Data.Select(vm => vm.Value).ToList());
            m_Complete.OnCompleted();
        }
    }
}
