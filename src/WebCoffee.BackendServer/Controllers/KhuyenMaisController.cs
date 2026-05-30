using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebCoffee.BackendServer.Services.KhuyenMais;
using WebCoffee.ViewModels.Catalog.KhuyenMais;
using WebCoffee.ViewModels.Common;

namespace WebCoffee.BackendServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KhuyenMaisController : ControllerBase
    {
        private readonly IKhuyenMaiService _khuyenMaiService;

        public KhuyenMaisController(IKhuyenMaiService khuyenMaiService)
        {
            _khuyenMaiService = khuyenMaiService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _khuyenMaiService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{maKm}")]
        public async Task<IActionResult> GetById(string maKm)
        {
            var result = await _khuyenMaiService.GetByIdAsync(maKm);
            return HandleResult(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] KhuyenMaiCreateRequest request)
        {
            var result = await _khuyenMaiService.CreateAsync(request);
            if (!result.Success) return HandleResult(result);

            return CreatedAtAction(nameof(GetById), new { maKm = result.Data?.MaKM }, result);
        }

        [HttpPut("{maKm}")]
        public async Task<IActionResult> Update(string maKm, [FromBody] KhuyenMaiUpdateRequest request)
        {
            var result = await _khuyenMaiService.UpdateAsync(maKm, request);
            return HandleResult(result);
        }

        [HttpDelete("{maKm}")]
        public async Task<IActionResult> Delete(string maKm)
        {
            var result = await _khuyenMaiService.DeleteAsync(maKm);
            return HandleResult(result);
        }

        [HttpPost("{maKm}/san-phams")]
        public async Task<IActionResult> GanSanPham(string maKm, [FromBody] GanSanPhamKhuyenMaiRequest request)
        {
            var result = await _khuyenMaiService.GanSanPhamAsync(maKm, request);
            return HandleResult(result);
        }

        [HttpDelete("{maKm}/san-phams/{maSp}")]
        public async Task<IActionResult> XoaSanPham(string maKm, string maSp)
        {
            var result = await _khuyenMaiService.XoaSanPhamAsync(maKm, maSp);
            return HandleResult(result);
        }

        [HttpGet("{maKm}/san-phams")]
        public async Task<IActionResult> GetSanPhams(string maKm)
        {
            // Tùy chọn kiểm tra tồn tại KM trước khi lấy list sản phẩm
            var checkResult = await _khuyenMaiService.GetByIdAsync(maKm);
            if (!checkResult.Success) return HandleResult(checkResult);

            var result = await _khuyenMaiService.GetSanPhamsByKhuyenMaiAsync(maKm);
            return Ok(result);
        }

        // Hàm helper xử lý chung cho HTTP Response
        private IActionResult HandleResult(ServiceResult result)
        {
            if (result.Success) return Ok(result);

            return result.ErrorCode switch
            {
                ErrorCodes.NotFound => NotFound(result),
                ErrorCodes.Validation => BadRequest(result),
                ErrorCodes.Conflict => Conflict(result),
                _ => StatusCode(500, result)
            };
        }

        private IActionResult HandleResult<T>(ServiceResult<T> result)
        {
            if (result.Success) return Ok(result);

            return result.ErrorCode switch
            {
                ErrorCodes.NotFound => NotFound(result),
                ErrorCodes.Validation => BadRequest(result),
                ErrorCodes.Conflict => Conflict(result),
                _ => StatusCode(500, result)
            };
        }
    }
}