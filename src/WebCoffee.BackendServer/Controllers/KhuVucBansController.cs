using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebCoffee.BackendServer.Services.KhuVucBans;
using WebCoffee.ViewModels.Catalog.KhuVucBans;

namespace WebCoffee.BackendServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KhuVucBansController : ControllerBase
    {
        private readonly IKhuVucBanService _khuVucBanService;

        public KhuVucBansController(IKhuVucBanService khuVucBanService)
        {
            _khuVucBanService = khuVucBanService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _khuVucBanService.GetAllWithBansAsync();
            return Ok(result);
        }

        [HttpPost("khu-vucs")]
        public async Task<IActionResult> CreateKhuVuc([FromBody] KhuVucCreateRequest request)
        {
            var result = await _khuVucBanService.CreateKhuVucAsync(request);
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }

        [HttpPut("khu-vucs/{soKV}")]
        public async Task<IActionResult> UpdateKhuVuc(string soKV, [FromBody] KhuVucUpdateRequest request)
        {
            var result = await _khuVucBanService.UpdateKhuVucAsync(soKV, request);
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }

        [HttpDelete("khu-vucs/{soKV}")]
        public async Task<IActionResult> DeleteKhuVuc(string soKV)
        {
            var result = await _khuVucBanService.DeleteKhuVucAsync(soKV);
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }

        [HttpPost("bans")]
        public async Task<IActionResult> CreateBan([FromBody] BanCreateRequest request)
        {
            var result = await _khuVucBanService.CreateBanAsync(request);
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }

        [HttpPut("bans/{soBan}")]
        public async Task<IActionResult> UpdateBan(string soBan, [FromBody] BanUpdateRequest request)
        {
            var result = await _khuVucBanService.UpdateBanAsync(soBan, request);
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }

        [HttpDelete("bans/{soBan}")]
        public async Task<IActionResult> DeleteBan(string soBan)
        {
            var result = await _khuVucBanService.DeleteBanAsync(soBan);
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }
    }
}