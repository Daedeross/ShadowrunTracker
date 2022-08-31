namespace ShadowrunTracker.ViewModels
{
    using ShadowrunTracker.Data;

    public interface IRecordViewModel: IViewModel, IHaveId
    {
        RecordBase Record { get; }
    }
}
