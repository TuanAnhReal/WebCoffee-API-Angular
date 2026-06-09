using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebCoffee.BackendServer.Services.Dashboard;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class DashboardController : ControllerBase
{
    private readonly IDashboardService _dashboardService;

    public DashboardController(IDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    /// <summary>
    /// Endpoint chính: trả KPI + Chart + Lists trong 1 call.
    /// Frontend nên ưu tiên dùng endpoint này.
    /// </summary>
    [HttpGet("summary")]
    public async Task<IActionResult> GetSummary()
        => Ok(await _dashboardService.GetSummaryAsync());

    /// Endpoint riêng lẻ – giữ lại để tương thích ngược
    [HttpGet("kpi")]
    public async Task<IActionResult> GetKpi()
        => Ok(await _dashboardService.GetKpiAsync());

    [HttpGet("lists")]
    public async Task<IActionResult> GetLists()
        => Ok(await _dashboardService.GetListsAsync());

    [HttpGet("chart")]
    public async Task<IActionResult> GetChart()
        => Ok(await _dashboardService.GetChartAsync());
}
