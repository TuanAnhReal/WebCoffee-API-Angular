using Microsoft.AspNetCore.Mvc;
using WebCoffee.BackendServer.Services.LoaiSPs;
using WebCoffee.ViewModels.Catalog.LoaiSPs;
using WebCoffee.ViewModels.Common;

namespace WebCoffee.BackendServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoaiSPsController : ControllerBase
    {
        private readonly ILoaiSPService _loaiSPService;

        public LoaiSPsController(ILoaiSPService loaiSPService)
        {
            _loaiSPService = loaiSPService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _loaiSPService.GetAllAsync();
            return Ok(ApiResponse<List<LoaiSPVm>>.SuccessResult(result, "Lấy danh sách loại sản phẩm thành công"));
        }

        [HttpGet("{maLoaiSp}")]
        public async Task<IActionResult> GetById(string maLoaiSp)
        {
            var result = await _loaiSPService.GetByIdAsync(maLoaiSp);
            if (result == null) return NotFound(ApiResponse.ErrorResult("Không tìm thấy loại sản phẩm", 404));

            return Ok(ApiResponse<LoaiSPVm>.SuccessResult(result));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] LoaiSPCreateRequest request)
        {
            var result = await _loaiSPService.CreateAsync(request);
            return Created(string.Empty, ApiResponse<LoaiSPVm>.SuccessResult(result, "Thêm loại sản phẩm thành công", 201));
        }

        [HttpPut("{maLoaiSp}")]
        public async Task<IActionResult> Update(string maLoaiSp, [FromBody] LoaiSPUpdateRequest request)
        {
            var isSuccess = await _loaiSPService.UpdateAsync(maLoaiSp, request);
            if (!isSuccess) return NotFound(ApiResponse.ErrorResult("Không tìm thấy loại sản phẩm để cập nhật", 404));

            return Ok(ApiResponse.SuccessResult("Cập nhật thành công"));
        }

        [HttpDelete("{maLoaiSp}")]
        public async Task<IActionResult> Delete(string maLoaiSp)
        {
            var isSuccess = await _loaiSPService.DeleteAsync(maLoaiSp);
            if (!isSuccess) return NotFound(ApiResponse.ErrorResult("Không tìm thấy loại sản phẩm để xóa", 404));

            return Ok(ApiResponse.SuccessResult("Xóa thành công"));
        }
    }
}