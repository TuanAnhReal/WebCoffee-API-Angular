using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebCoffee.BackendServer.Services.CaLams;
using WebCoffee.ViewModels.Catalog.CaLams;
using WebCoffee.ViewModels.Common;

namespace WebCoffee.BackendServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CaLamsController : ControllerBase
    {
        private readonly ICaLamService _caLamService;

        public CaLamsController(ICaLamService caLamService)
        {
            _caLamService = caLamService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _caLamService.GetAllAsync();
            return Ok(ApiResponse<List<CaLamVm>>.SuccessResult(result, "Lấy danh sách ca làm việc thành công"));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CaLamVm request)
        {
            var isSuccess = await _caLamService.CreateAsync(request);
            if (!isSuccess) return BadRequest(ApiResponse.ErrorResult("Thêm ca làm việc thất bại", 400));

            return Ok(ApiResponse<CaLamVm>.SuccessResult(request, "Thêm ca làm việc thành công", 201));
        }
    }
}