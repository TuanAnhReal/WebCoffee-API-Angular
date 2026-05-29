using Microsoft.EntityFrameworkCore;
using WebCoffee.BackendServer.Data;
using WebCoffee.BackendServer.Data.Entities;
using WebCoffee.ViewModels.System.Auth;

namespace WebCoffee.BackendServer.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _configuration;

        public AuthService(AppDbContext context, ITokenService tokenService, IConfiguration configuration)
        {
            _context = context;
            _tokenService = tokenService;
            _configuration = configuration;
        }

        public async Task<LoginResponse> AuthenticateAsync(LoginRequest request)
        {
            var account = await _context.TaiKhoans.Include(x => x.PhanQuyen)
                .FirstOrDefaultAsync(x => x.TenDangNhap == request.TenDangNhap);

            if (account == null || account.TrangThaiTK != "Hoạt động") return null;

            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(request.MatKhau, account.MatKhau);
            if (!isPasswordValid) return null;

            var roleName = account.PhanQuyen?.TenPQ ?? "User";

            // 1. Tạo Token
            var accessToken = _tokenService.GenerateAccessToken(account, roleName);
            var refreshToken = _tokenService.GenerateRefreshToken();

            // 2. Lưu Refresh Token vào Database
            double rtDays = Convert.ToDouble(_configuration["Jwt:RefreshTokenDays"] ?? "7");
            var rfEntity = new RefreshToken
            {
                Token = refreshToken,
                TenDangNhap = account.TenDangNhap,
                ExpiryDate = DateTime.Now.AddDays(rtDays),
                IsRevoked = false,
                CreatedAt = DateTime.Now
            };

            _context.RefreshTokens.Add(rfEntity);
            await _context.SaveChangesAsync();

            // 3. Trả về cả Access + Refresh Token
            double expireMinutes = Convert.ToDouble(_configuration["Jwt:AccessTokenMinutes"] ?? "15");
            return new LoginResponse
            {
                Token = accessToken,
                RefreshToken = refreshToken,
                Username = account.TenDangNhap,
                Role = roleName,
                Expiration = DateTime.Now.AddMinutes(expireMinutes)
            };
        }

        public async Task<LoginResponse> RenewTokenAsync(RefreshTokenRequest request)
        {
            // 1. Dịch ngược Access Token cũ để lấy Username
            var principal = _tokenService.GetPrincipalFromExpiredToken(request.AccessToken);
            var username = principal.Identity?.Name; // Username

            // 2. Kiểm tra Refresh Token trong Database
            var storedToken = await _context.RefreshTokens
                .FirstOrDefaultAsync(x => x.Token == request.RefreshToken && x.TenDangNhap == username);

            if (storedToken == null || storedToken.IsRevoked || storedToken.ExpiryDate <= DateTime.Now)
            {
                return null; // Token không hợp lệ, bị thu hồi hoặc đã hết hạn
            }

            // 3. Lấy thông tin user để tạo Token mới
            var account = await _context.TaiKhoans.Include(x => x.PhanQuyen).FirstOrDefaultAsync(x => x.TenDangNhap == username);
            var roleName = account?.PhanQuyen?.TenPQ ?? "User";

            var newAccessToken = _tokenService.GenerateAccessToken(account, roleName);
            var newRefreshToken = _tokenService.GenerateRefreshToken();

            // 4. Thu hồi Token cũ, lưu Token mới
            storedToken.IsRevoked = true;

            double rtDays = Convert.ToDouble(_configuration["Jwt:RefreshTokenDays"] ?? "7");
            var newRfEntity = new RefreshToken
            {
                Token = newRefreshToken,
                TenDangNhap = account.TenDangNhap,
                ExpiryDate = DateTime.Now.AddDays(rtDays),
                IsRevoked = false,
                CreatedAt = DateTime.Now
            };

            _context.RefreshTokens.Add(newRfEntity);
            await _context.SaveChangesAsync();

            double expireMinutes = Convert.ToDouble(_configuration["Jwt:AccessTokenMinutes"] ?? "15");
            return new LoginResponse
            {
                Token = newAccessToken,
                RefreshToken = newRefreshToken,
                Username = account.TenDangNhap,
                Role = roleName,
                Expiration = DateTime.Now.AddMinutes(expireMinutes)
            };
        }

        public async Task<bool> RevokeTokenAsync(string username)
        {
            // Tìm tất cả các Refresh Token đang hoạt động của User này và thu hồi
            var tokens = await _context.RefreshTokens
                .Where(x => x.TenDangNhap == username && !x.IsRevoked)
                .ToListAsync();

            if (!tokens.Any()) return false;

            foreach (var token in tokens)
            {
                token.IsRevoked = true;
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ChangePasswordAsync(string username, ChangePasswordRequest request)
        {
            var account = await _context.TaiKhoans.FirstOrDefaultAsync(x => x.TenDangNhap == username);
            if (account == null) return false;

            // Kiểm tra mật khẩu cũ
            if (!BCrypt.Net.BCrypt.Verify(request.OldPassword, account.MatKhau)) return false;

            account.MatKhau = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
            _context.TaiKhoans.Update(account);

            await _context.RefreshTokens
                .Where(x => x.TenDangNhap == username && !x.IsRevoked)
                .ExecuteUpdateAsync(s => s.SetProperty(b => b.IsRevoked, true));

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DangKyTaiKhoanAsync(DangKyRequest request)
        {
            var checkTrungTen = await _context.TaiKhoans.AnyAsync(x => x.TenDangNhap == request.TenDangNhap);
            if (checkTrungTen) return false;

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    string maNhanVienLienKet = request.MaNV;
                    if (string.IsNullOrEmpty(maNhanVienLienKet))
                    {
                        maNhanVienLienKet = "NV" + DateTime.Now.ToString("yyMMdd") + new Random().Next(10, 99);

                        var chuoiTen = request.HoVaTen.Trim().Split(' ');
                        string tenNV = chuoiTen.Last();
                        string hoNV = string.Join(" ", chuoiTen.Take(chuoiTen.Length - 1));
                        if (string.IsNullOrEmpty(hoNV)) hoNV = "Nguyễn";

                        string maLoaiNhanVienMacDinh = "LNV02";
                        if (request.MaPQ == "PQ_ADMIN" || request.MaPQ == "Quản trị viên")
                        {
                            maLoaiNhanVienMacDinh = "LNV01";
                        }

                        var nhanVienMoi = new NhanVien
                        {
                            MaNV = maNhanVienLienKet,
                            HoNV = hoNV,
                            TenNV = tenNV,
                            TrangThaiNV = "Đang làm",

                            MaLoaiNV = maLoaiNhanVienMacDinh
                        };

                        _context.NhanViens.Add(nhanVienMoi);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        var kiemTraNhanVien = await _context.NhanViens.AnyAsync(x => x.MaNV == maNhanVienLienKet);
                        if (!kiemTraNhanVien) throw new Exception("Mã nhân viên chỉ định không tồn tại.");

                        var kiemTraTaiKhoan = await _context.TaiKhoans.AnyAsync(x => x.MaNV == maNhanVienLienKet);
                        if (kiemTraTaiKhoan) throw new Exception("Nhân viên này đã có tài khoản.");
                    }

                    var taiKhoanMoi = new TaiKhoan
                    {
                        TenDangNhap = request.TenDangNhap,
                        MatKhau = BCrypt.Net.BCrypt.HashPassword(request.MatKhau),
                        MaNV = maNhanVienLienKet,
                        MaPQ = request.MaPQ,
                        TrangThaiTK = "Hoạt động"
                    };

                    _context.TaiKhoans.Add(taiKhoanMoi);
                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();
                    return true;
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }
    }
}