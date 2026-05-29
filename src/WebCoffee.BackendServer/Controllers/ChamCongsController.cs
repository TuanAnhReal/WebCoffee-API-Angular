using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebCoffee.BackendServer.Services.ChamCongs;
using WebCoffee.ViewModels.Catalog.ChamCongs;
using WebCoffee.ViewModels.Common;

namespace WebCoffee.BackendServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChamCongsController : ControllerBase
    {
        private readonly IChamCongService _chamCongService;

        public ChamCongsController(IChamCongService chamCongService)
        {
            _chamCongService = chamCongService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _chamCongService.GetAllAsync();
            return Ok(ApiResponse<List<ChamCongVm>>.SuccessResult(result, "Lấy danh sách chấm công thành công"));
        }

        [HttpPost("check-in")]
        public async Task<IActionResult> CheckIn([FromBody] ChamCongVm request)
        {
            var isSuccess = await _chamCongService.CheckInAsync(request);
            if (!isSuccess) return BadRequest(ApiResponse.ErrorResult("Ghi nhận chấm công thất bại", 400));

            return Ok(ApiResponse.SuccessResult("Ghi nhận chấm công thành công!"));
        }
    }
}