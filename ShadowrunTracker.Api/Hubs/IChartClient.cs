using ShadowrunTracker.Api.Models;

namespace ShadowrunTracker.Api.Hubs
{
    public interface IChartClient
    {
        Task BroadcastChartData(List<ChartModel> data);
        Task TransferChartData(List<ChartModel> data);
    }
}
