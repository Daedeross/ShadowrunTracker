namespace ShadowrunTracker.ViewModels
{
    public interface IRecordViewModel<TRecord> : IViewModel, IHaveId
    {
        TRecord ToRecord();

        void Update(TRecord record);
    }
}
