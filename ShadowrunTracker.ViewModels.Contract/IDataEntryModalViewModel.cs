namespace ShadowrunTracker.ViewModels
{
    using System.Collections.Generic;

    public interface IDataEntryModalViewModel : IDisposableModalViewModel<IList<object?>?>
    {
        public string Header { get; set; }

        IList<IEntryDatumViewModel> Data { get; }
    }
}
