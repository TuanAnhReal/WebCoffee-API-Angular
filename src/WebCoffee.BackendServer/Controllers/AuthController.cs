using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebCoffee.BackendServer.Helpers;
using WebCoffee.BackendServer.Services.Auth;
using WebCoffee.ViewModels.Common;
using WebCoffee.ViewModels.System.Auth;

namespace WebCoffee.BackendServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ICurrentUserService _currentUserService;

        public AuthController(IAuthService authService, ICurrentUserService currentUserService)
        {
            _authService = authService;
            _currentUserService = currentUserService;
        }

        // 1. API ĐĂNG NHẬP (Lấy cả Access Token & Refresh Token)
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var result = await _authService.AuthenticateAsync(request);

            if (result == null)
                return Unauthorized(ApiResponse<object>.ErrorResult("Tài khoản hoặc mật khẩu không chính xác."));

            return Ok(ApiResponse<LoginResponse>.SuccessResult(result, "Đăng nhập thành công!"));
        }

        // 2. API LẤY THÔNG TIN CÁ NHÂN (Yêu cầu phải có Token)
        [Authorize]
        [HttpGet("me")]
        public IActionResult GetMe()
        {
            var profile = new
            {
                Username = _currentUserService.UserName,
                Role = _currentUserService.Role,
                MaNV = _currentUserService.MaNV
            };

            return Ok(new { success = true, data = profile });
        }

        // 3. API LÀM MỚI TOKEN (Khi Access Token hết hạn)
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            if (string.IsNullOrEmpty(request.AccessToken) || string.IsNullOrEmpty(request.RefreshToken))
                return BadRequest(new { success = false, message = "Dữ liệu token không hợp lệ." });

            try
            {
                var result = await _authService.RenewTokenAsync(request);

                if (result == null)
                    return Unauthorized(new { success = false, message = "Refresh token không tồn tại, đã hết hạn hoặc bị thu hồi. Vui lòng đăng nhập lại!" });

                return Ok(new { success = true, message = "Làm mới Token thành công!", data = result });
            }
            catch (Exception)
            {
                // Bắt lỗi khi Access Token bị biến dạng/sai cấu trúc
                return Unauthorized(new { success = false, message = "Access Token không hợp lệ." });
            }
        }

        // 4. API ĐĂNG XUẤT (Thu hồi Token)
        [Authorize]
        [HttpPost("revoke")]
        public async Task<IActionResult> Revoke()
        {
            // Tự động lấy Username từ Token hiện tại nhờ CurrentUserService
            var username = _currentUserService.UserName;
            if (string.IsNullOrEmpty(username)) return Unauthorized();

            var result = await _authService.RevokeTokenAsync(username);
            if (!result)
                return BadRequest(new { success = false, message = "Không có phiên đăng nhập nào cần thu hồi." });

            return Ok(new { success = true, message = "Đăng xuất thành công, đã thu hồi quyền truy cập!" });
        }

        // 5. API ĐỔI MẬT KHẨU
        [Authorize]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            if (request.NewPassword != request.ConfirmNewPassword)
                return BadRequest(new { success = false, message = "Xác nhận mật khẩu không khớp." });

            var username = _currentUserService.UserName;
            if (string.IsNullOrEmpty(username)) return Unauthorized();

            var result = await _authService.ChangePasswordAsync(username, request);

            if (!result)
                return BadRequest(new { success = false, message = "Mật khẩu cũ không chính xác hoặc lỗi hệ thống." });

            return Ok(new { success = true, message = "Đổi mật khẩu thành công. Tất cả các thiết bị đã bị đăng xuất, vui lòng đăng nhập lại!" });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] DangKyRequest request)
        {
            try
            {
                var ketQua = await _authService.DangKyTaiKhoanAsync(request);
                if (!ketQua) return BadRequest(ApiResponse.ErrorResult("Tên đăng nhập đã có người sử dụng."));

                return Ok(ApiResponse.SuccessResult("Tạo tài khoản hệ thống thành công!"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse.ErrorResult(ex.Message));
            }
        }
    }
}