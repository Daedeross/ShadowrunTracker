namespace ShadowrunTracker.ViewModels
{
    public interface IRecordViewModel<TRecord> : IRecordViewModel
    {
        TRecord Record { get; }

        TRecord ToRecord();

        void Update(TRecord record);
    }
}
