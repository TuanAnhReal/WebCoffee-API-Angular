using Microsoft.EntityFrameworkCore;
using WebCoffee.BackendServer.Data;
using WebCoffee.ViewModels.Catalog.Dashboard;

namespace WebCoffee.BackendServer.Services.Dashboard
{
    public interface IDashboardService
    {
        Task<DashboardVm> GetDashboardSummaryAsync();
    }

}