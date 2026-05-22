using Microsoft.AspNetCore.Mvc;
using WebCoffee.BackendServer.Services.NhanViens;
using WebCoffee.ViewModels.Catalog.NhanViens;
using WebCoffee.ViewModels.Common;

namespace WebCoffee.BackendServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NhanViensController : ControllerBase
    {
        private readonly INhanVienService _nhanVienService;

        public NhanViensController(INhanVienService nhanVienService)
        {
            _nhanVienService = nhanVienService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _nhanVienService.GetAllAsync();
            return Ok(ApiResponse<List<NhanVienVm>>.SuccessResult(result, "Lấy danh sách nhân viên thành công"));
        }

        [HttpGet("{maNv}")]
        public async Task<IActionResult> GetById(string maNv)
        {
            var result = await _nhanVienService.GetByIdAsync(maNv);
            if (result == null) return NotFound(ApiResponse.ErrorResult("Không tìm thấy nhân viên", 404));

            return Ok(ApiResponse<NhanVienVm>.SuccessResult(result));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] NhanVienCreateRequest request)
        {
            var result = await _nhanVienService.CreateAsync(request);
            return Created(string.Empty, ApiResponse<NhanVienVm>.SuccessResult(result, "Thêm nhân viên thành công", 201));
        }

        [HttpPut("{maNv}")]
        public async Task<IActionResult> Update(string maNv, [FromBody] NhanVienUpdateRequest request)
        {
            var isSuccess = await _nhanVienService.UpdateAsync(maNv, request);
            if (!isSuccess) return NotFound(ApiResponse.ErrorResult("Không tìm thấy nhân viên để cập nhật", 404));

            return Ok(ApiResponse.SuccessResult("Cập nhật thành công"));
        }

        [HttpDelete("{maNv}")]
        public async Task<IActionResult> Delete(string maNv)
        {
            var isSuccess = await _nhanVienService.DeleteAsync(maNv);
            if (!isSuccess) return NotFound(ApiResponse.ErrorResult("Không tìm thấy nhân viên để xóa", 404));

            return Ok(ApiResponse.SuccessResult("Xóa thành công"));
        }
    }
}