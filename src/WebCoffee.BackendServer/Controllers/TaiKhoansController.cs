using Microsoft.AspNetCore.Mvc;
using WebCoffee.BackendServer.Services.TaiKhoans;
using WebCoffee.ViewModels.Common;
using WebCoffee.ViewModels.System.TaiKhoans;

namespace WebCoffee.BackendServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaiKhoansController : ControllerBase
    {
        private readonly ITaiKhoanService _taiKhoanService;

        public TaiKhoansController(ITaiKhoanService taiKhoanService)
        {
            _taiKhoanService = taiKhoanService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _taiKhoanService.GetAllAsync();
            return Ok(ApiResponse<List<TaiKhoanVm>>.SuccessResult(result));
        }

        [HttpGet("{tenDangNhap}")]
        public async Task<IActionResult> GetById(string tenDangNhap)
        {
            var result = await _taiKhoanService.GetByIdAsync(tenDangNhap);
            if (result == null) return NotFound(ApiResponse.ErrorResult("Không tìm thấy tài khoản", 404));

            return Ok(ApiResponse<TaiKhoanVm>.SuccessResult(result));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TaiKhoanCreateRequest request)
        {
            var result = await _taiKhoanService.CreateAsync(request);
            if (result == null) return BadRequest(ApiResponse.ErrorResult("Tên đăng nhập đã tồn tại", 400));

            return Created(string.Empty, ApiResponse<TaiKhoanVm>.SuccessResult(result, "Tạo tài khoản thành công", 201));
        }

        [HttpPut("{tenDangNhap}")]
        public async Task<IActionResult> Update(string tenDangNhap, [FromBody] TaiKhoanUpdateRequest request)
        {
            var isSuccess = await _taiKhoanService.UpdateAsync(tenDangNhap, request);
            if (!isSuccess) return NotFound(ApiResponse.ErrorResult("Không tìm thấy tài khoản", 404));

            return Ok(ApiResponse.SuccessResult("Cập nhật thành công"));
        }

        [HttpDelete("{tenDangNhap}")]
        public async Task<IActionResult> Delete(string tenDangNhap)
        {
            var isSuccess = await _taiKhoanService.DeleteAsync(tenDangNhap);
            if (!isSuccess) return NotFound(ApiResponse.ErrorResult("Không tìm thấy tài khoản", 404));

            return Ok(ApiResponse.SuccessResult("Xóa thành công"));
        }
    }
}