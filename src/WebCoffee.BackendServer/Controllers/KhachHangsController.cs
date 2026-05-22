using Microsoft.AspNetCore.Mvc;
using WebCoffee.BackendServer.Services.KhachHangs;
using WebCoffee.ViewModels.Catalog.KhachHangs;
using WebCoffee.ViewModels.Common;

namespace WebCoffee.BackendServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KhachHangsController : ControllerBase
    {
        private readonly IKhachHangService _khachHangService;

        public KhachHangsController(IKhachHangService khachHangService)
        {
            _khachHangService = khachHangService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _khachHangService.GetAllAsync();
            return Ok(ApiResponse<List<KhachHangVm>>.SuccessResult(result, "Lấy danh sách khách hàng thành công"));
        }

        [HttpGet("{maKh}")]
        public async Task<IActionResult> GetById(string maKh)
        {
            var result = await _khachHangService.GetByIdAsync(maKh);
            if (result == null) return NotFound(ApiResponse.ErrorResult("Không tìm thấy khách hàng", 404));

            return Ok(ApiResponse<KhachHangVm>.SuccessResult(result));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] KhachHangCreateRequest request)
        {
            var result = await _khachHangService.CreateAsync(request);
            return Created(string.Empty, ApiResponse<KhachHangVm>.SuccessResult(result, "Thêm khách hàng thành công", 201));
        }

        [HttpPut("{maKh}")]
        public async Task<IActionResult> Update(string maKh, [FromBody] KhachHangUpdateRequest request)
        {
            var isSuccess = await _khachHangService.UpdateAsync(maKh, request);
            if (!isSuccess) return NotFound(ApiResponse.ErrorResult("Không tìm thấy khách hàng để cập nhật", 404));

            return Ok(ApiResponse.SuccessResult("Cập nhật thành công"));
        }

        [HttpDelete("{maKh}")]
        public async Task<IActionResult> Delete(string maKh)
        {
            var isSuccess = await _khachHangService.DeleteAsync(maKh);
            if (!isSuccess) return NotFound(ApiResponse.ErrorResult("Không tìm thấy khách hàng để xóa", 404));

            return Ok(ApiResponse.SuccessResult("Xóa thành công"));
        }
    }
}