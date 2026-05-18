using Microsoft.AspNetCore.Mvc;
using WebCoffee.BackendServer.Models;
using WebCoffee.ViewModels.Catalog;

namespace WebCoffee.BackendServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SanPhamsController : ControllerBase
    {
        private readonly SanPhamModel _sanPhamModel;
        public SanPhamsController(SanPhamModel sanPhamModel)
        {
            _sanPhamModel = sanPhamModel;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
            => await _sanPhamModel.GetAllProductsAsync();

        [HttpGet("{maSp}")]
        public async Task<IActionResult> GetById(string maSp)
            => await _sanPhamModel.GetProductByIdAsync(maSp);

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SanPhamCreateRequest request)
            => await _sanPhamModel.CreateProductAsync(request);

        [HttpPut("{maSp}")]
        public async Task<IActionResult> Update(string maSp, [FromBody] SanPhamUpdateRequest request)
            => await _sanPhamModel.UpdateProductAsync(maSp, request);

        [HttpDelete("{maSp}")]
        public async Task<IActionResult> Delete(string maSp)
            => await _sanPhamModel.DeleteProductAsync(maSp);
    }
}