using WebCoffee.ViewModels.Catalog.Dashboard;

namespace WebCoffee.BackendServer.Services.Dashboard
{
    public interface IDashboardService
    {
        Task<DashboardSummaryVm> GetSummaryAsync();
        Task<DashboardKpiVm> GetKpiAsync();
        Task<DashboardListsVm> GetListsAsync();
        Task<List<decimal>> GetChartAsync();
    }
}
