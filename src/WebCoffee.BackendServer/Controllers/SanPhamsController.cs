using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebCoffee.BackendServer.Services.SanPhams;
using WebCoffee.ViewModels.Catalog.SanPhams;
using WebCoffee.ViewModels.Common;

namespace WebCoffee.BackendServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SanPhamsController : ControllerBase
    {
        private readonly ISanPhamService _sanPhamService;

        public SanPhamsController(ISanPhamService sanPhamService)
        {
            _sanPhamService = sanPhamService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _sanPhamService.GetAllAsync();

            return Ok(
                ApiResponse<List<SanPhamVm>>
                .SuccessResult(result, "Lấy danh sách sản phẩm thành công"));
        }

        [HttpGet("{maSp}")]
        public async Task<IActionResult> GetById(string maSp)
        {
            var result = await _sanPhamService.GetByIdAsync(maSp);

            if (result == null)
            {
                return NotFound(
                    ApiResponse<object>
                    .ErrorResult("Không tìm thấy sản phẩm.", 404));
            }

            return Ok(
                ApiResponse<SanPhamVm>
                .SuccessResult(result, "Lấy thông tin sản phẩm thành công"));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] SanPhamCreateRequest request)
        {
            var result = await _sanPhamService.CreateAsync(request);

            return CreatedAtAction(
                nameof(GetById),
                new { maSp = result.MaSp },
                ApiResponse<SanPhamVm>
                .SuccessResult(result, "Thêm sản phẩm thành công", 201));
        }

        [HttpPut("{maSp}")]
        public async Task<IActionResult> Update(
            string maSp,
            [FromForm] SanPhamUpdateRequest request)
        {
            var success = await _sanPhamService.UpdateAsync(maSp, request);

            if (!success)
            {
                return NotFound(
                    ApiResponse<object>
                    .ErrorResult("Không tìm thấy sản phẩm để cập nhật.", 404));
            }

            return Ok(
                ApiResponse<object>
                .SuccessResult(null, "Cập nhật sản phẩm thành công"));
        }

        [HttpDelete("{maSp}")]
        public async Task<IActionResult> Delete(string maSp)
        {
            var success = await _sanPhamService.DeleteAsync(maSp);

            if (!success)
            {
                return NotFound(
                    ApiResponse<object>
                    .ErrorResult("Không tìm thấy sản phẩm để xóa.", 404));
            }

            return Ok(
                ApiResponse<object>
                .SuccessResult(null, "Xóa sản phẩm thành công"));
        }
    }
}