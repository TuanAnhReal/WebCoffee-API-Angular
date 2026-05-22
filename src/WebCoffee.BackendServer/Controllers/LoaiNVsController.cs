using Microsoft.AspNetCore.Mvc;
using WebCoffee.BackendServer.Services.LoaiNVs;
using WebCoffee.ViewModels.Common;
using WebCoffee.ViewModels.Catalog.LoaiNVs;

namespace WebCoffee.BackendServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoaiNVsController : ControllerBase
    {
        private readonly ILoaiNVService _loaiNVService;

        public LoaiNVsController(ILoaiNVService loaiNVService)
        {
            _loaiNVService = loaiNVService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _loaiNVService.GetAllAsync();
            return Ok(ApiResponse<List<LoaiNVVm>>.SuccessResult(result, "Lấy danh sách loại nhân viên thành công"));
        }

        [HttpGet("{maLoaiNv}")]
        public async Task<IActionResult> GetById(string maLoaiNv)
        {
            var result = await _loaiNVService.GetByIdAsync(maLoaiNv);
            if (result == null) return NotFound(ApiResponse.ErrorResult("Không tìm thấy loại nhân viên", 404));

            return Ok(ApiResponse<LoaiNVVm>.SuccessResult(result));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] LoaiNVCreateRequest request)
        {
            var result = await _loaiNVService.CreateAsync(request);
            return Created(string.Empty, ApiResponse<LoaiNVVm>.SuccessResult(result, "Thêm loại nhân viên thành công", 201));
        }

        [HttpPut("{maLoaiNv}")]
        public async Task<IActionResult> Update(string maLoaiNv, [FromBody] LoaiNVUpdateRequest request)
        {
            var isSuccess = await _loaiNVService.UpdateAsync(maLoaiNv, request);
            if (!isSuccess) return NotFound(ApiResponse.ErrorResult("Không tìm thấy loại nhân viên để cập nhật", 404));

            return Ok(ApiResponse.SuccessResult("Cập nhật thành công"));
        }

        [HttpDelete("{maLoaiNv}")]
        public async Task<IActionResult> Delete(string maLoaiNv)
        {
            var isSuccess = await _loaiNVService.DeleteAsync(maLoaiNv);
            if (!isSuccess) return NotFound(ApiResponse.ErrorResult("Không tìm thấy loại nhân viên để xóa", 404));

            return Ok(ApiResponse.SuccessResult("Xóa thành công"));
        }
    }
}