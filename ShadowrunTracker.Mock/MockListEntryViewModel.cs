namespace ShadowrunTracker.Mock
{
    using ShadowrunTracker.ViewModels;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Windows.Input;

    public class MockListEntryViewModel : IDataEntryModalViewModel

    {
        public string Header { get; set; } = "Mock Entry Header";

        public IList<IEntryDatumViewModel> Data { get; } = new List<IEntryDatumViewModel>
        {
            new EntryDatumViewModel(new Model.EntryDatum("First Item")),
            new EntryDatumViewModel(new Model.EntryDatum("Other Item")),
            new EntryDatumViewModel(new Model.EntryDatum("This is the last Item")),
        };

        public IObservable<IList<object?>?> Complete => throw new NotImplementedException();

        public ICommand OkCommand => throw new NotImplementedException();

        public ICommand CancelCommand => throw new NotImplementedException();

        public event PropertyChangedEventHandler PropertyChanged;

        public void Cancel()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
