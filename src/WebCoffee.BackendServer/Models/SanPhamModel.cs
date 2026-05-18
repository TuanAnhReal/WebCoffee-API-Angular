using Microsoft.AspNetCore.Mvc;
using WebCoffee.BackendServer.Services.SanPhams;
using WebCoffee.ViewModels.Catalog;

namespace WebCoffee.BackendServer.Models
{
    public class SanPhamModel
    {
        private readonly ISanPhamService _sanPhamService;

        // Inject Service vào để xử lý dữ liệu
        public SanPhamModel(ISanPhamService sanPhamService)
        {
            _sanPhamService = sanPhamService;
        }

        public async Task<IActionResult> GetAllProductsAsync()
        {
            var products = await _sanPhamService.GetAllAsync();
            return new OkObjectResult(products); // Trả về 200 OK kèm dữ liệu
        }

        public async Task<IActionResult> GetProductByIdAsync(string maSp)
        {
            var product = await _sanPhamService.GetByIdAsync(maSp);
            if (product == null)
            {
                return new NotFoundObjectResult(new { message = $"Không tìm thấy sản phẩm có mã: {maSp}" }); // Trả về 404
            }
            return new OkObjectResult(product);
        }

        public async Task<IActionResult> CreateProductAsync(SanPhamCreateRequest request)
        {
            if (string.IsNullOrEmpty(request.TenSP))
            {
                return new BadRequestObjectResult(new { message = "Tên sản phẩm không được để trống" }); // Trả về 400
            }

            var resultMaSp = await _sanPhamService.CreateAsync(request);
            if (string.IsNullOrEmpty(resultMaSp))
            {
                return new BadRequestObjectResult(new { message = "Thêm sản phẩm thất bại." });
            }

            return new OkObjectResult(new { message = "Thêm thành công!", maSP = resultMaSp });
        }

        public async Task<IActionResult> UpdateProductAsync(string maSp, SanPhamUpdateRequest request)
        {
            var affectedResult = await _sanPhamService.UpdateAsync(maSp, request);
            if (affectedResult == 0)
            {
                return new NotFoundObjectResult(new { message = $"Không tìm thấy sản phẩm có mã: {maSp} để cập nhật." });
            }
            return new OkObjectResult(new { message = "Cập nhật thành công!" });
        }

        public async Task<IActionResult> DeleteProductAsync(string maSp)
        {
            var affectedResult = await _sanPhamService.DeleteAsync(maSp);
            if (affectedResult == 0)
            {
                return new NotFoundObjectResult(new { message = $"Không tìm thấy sản phẩm có mã: {maSp} để xóa." });
            }
            return new OkObjectResult(new { message = "Xóa thành công!" });
        }
    }
}