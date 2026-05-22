using Microsoft.AspNetCore.Mvc;
using WebCoffee.BackendServer.Services.HoaDons;
using WebCoffee.ViewModels.Catalog.HoaDons;
using WebCoffee.ViewModels.Common;

namespace WebCoffee.BackendServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HoaDonsController : ControllerBase
    {
        private readonly IHoaDonService _hoaDonService;

        public HoaDonsController(IHoaDonService hoaDonService)
        {
            _hoaDonService = hoaDonService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _hoaDonService.GetAllAsync();
            return Ok(ApiResponse<List<HoaDonVm>>.SuccessResult(result, "Lấy danh sách hóa đơn thành công"));
        }

        [HttpGet("{soHd}")]
        public async Task<IActionResult> GetById(string soHd)
        {
            var result = await _hoaDonService.GetByIdAsync(soHd);
            if (result == null) return NotFound(ApiResponse.ErrorResult("Không tìm thấy hóa đơn", 404));

            return Ok(ApiResponse<HoaDonVm>.SuccessResult(result));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] HoaDonCreateRequest request)
        {
            var result = await _hoaDonService.CreateAsync(request);
            return Created(string.Empty, ApiResponse<HoaDonVm>.SuccessResult(result, "Tạo hóa đơn thành công", 201));
        }

        [HttpPut("{soHd}")]
        public async Task<IActionResult> Update(string soHd, [FromBody] HoaDonUpdateRequest request)
        {
            var isSuccess = await _hoaDonService.UpdateAsync(soHd, request);
            if (!isSuccess) return NotFound(ApiResponse.ErrorResult("Không tìm thấy hóa đơn để cập nhật", 404));

            return Ok(ApiResponse.SuccessResult("Cập nhật hóa đơn thành công"));
        }

        [HttpDelete("{soHd}")]
        public async Task<IActionResult> Delete(string soHd)
        {
            var isSuccess = await _hoaDonService.DeleteAsync(soHd);
            if (!isSuccess) return NotFound(ApiResponse.ErrorResult("Không tìm thấy hóa đơn để xóa", 404));

            return Ok(ApiResponse.SuccessResult("Xóa hóa đơn thành công"));
        }
    }
}