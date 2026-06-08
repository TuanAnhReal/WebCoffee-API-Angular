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

    [HttpGet("kpi")]
    public async Task<IActionResult> GetKpi() => Ok(await _dashboardService.GetKpiAsync());

    [HttpGet("lists")]
    public async Task<IActionResult> GetLists() => Ok(await _dashboardService.GetListsAsync());

    [HttpGet("chart")]
    public async Task<IActionResult> GetChart() => Ok(await _dashboardService.GetChartAsync());
}