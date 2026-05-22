using Microsoft.AspNetCore.Mvc;
using WebCoffee.BackendServer.Services.PhanQuyens;
using WebCoffee.ViewModels.Common;
using WebCoffee.ViewModels.System.PhanQuyens;

namespace WebCoffee.BackendServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhanQuyensController : ControllerBase
    {
        private readonly IPhanQuyenService _phanQuyenService;

        public PhanQuyensController(IPhanQuyenService phanQuyenService)
        {
            _phanQuyenService = phanQuyenService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _phanQuyenService.GetAllAsync();
            return Ok(ApiResponse<List<PhanQuyenVm>>.SuccessResult(result));
        }

        [HttpGet("{maPq}")]
        public async Task<IActionResult> GetById(string maPq)
        {
            var result = await _phanQuyenService.GetByIdAsync(maPq);
            if (result == null) return NotFound(ApiResponse.ErrorResult("Không tìm thấy phân quyền", 404));

            return Ok(ApiResponse<PhanQuyenVm>.SuccessResult(result));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PhanQuyenCreateRequest request)
        {
            var result = await _phanQuyenService.CreateAsync(request);
            return Created(string.Empty, ApiResponse<PhanQuyenVm>.SuccessResult(result, "Tạo phân quyền thành công", 201));
        }

        [HttpPut("{maPq}")]
        public async Task<IActionResult> Update(string maPq, [FromBody] PhanQuyenUpdateRequest request)
        {
            var isSuccess = await _phanQuyenService.UpdateAsync(maPq, request);
            if (!isSuccess) return NotFound(ApiResponse.ErrorResult("Không tìm thấy phân quyền", 404));

            return Ok(ApiResponse.SuccessResult("Cập nhật thành công"));
        }

        [HttpDelete("{maPq}")]
        public async Task<IActionResult> Delete(string maPq)
        {
            var isSuccess = await _phanQuyenService.DeleteAsync(maPq);
            if (!isSuccess) return NotFound(ApiResponse.ErrorResult("Không tìm thấy phân quyền", 404));

            return Ok(ApiResponse.SuccessResult("Xóa thành công"));
        }
    }
}