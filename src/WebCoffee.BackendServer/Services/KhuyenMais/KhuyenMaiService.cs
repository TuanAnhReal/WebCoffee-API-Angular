using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebCoffee.BackendServer.Data;
using WebCoffee.BackendServer.Data.Entities;
using WebCoffee.ViewModels.Catalog.KhuyenMais;
using WebCoffee.ViewModels.Common;

namespace WebCoffee.BackendServer.Services.KhuyenMais
{
    public class KhuyenMaiService : IKhuyenMaiService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public KhuyenMaiService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ServiceResult<List<KhuyenMaiVm>>> GetAllAsync()
        {
            var entities = await _context.KhuyenMais.ToListAsync();
            var data = _mapper.Map<List<KhuyenMaiVm>>(entities);

            return new ServiceResult<List<KhuyenMaiVm>>
            {
                Success = true,
                Data = data
            };
        }

        public async Task<ServiceResult<KhuyenMaiVm>> GetByIdAsync(string maKm)
        {
            var entity = await _context.KhuyenMais.FindAsync(maKm);
            if (entity == null)
            {
                return new ServiceResult<KhuyenMaiVm>
                {
                    Success = false,
                    ErrorCode = ErrorCodes.NotFound,
                    Message = "Không tìm thấy khuyến mãi."
                };
            }

            return new ServiceResult<KhuyenMaiVm>
            {
                Success = true,
                Data = _mapper.Map<KhuyenMaiVm>(entity)
            };
        }

        public async Task<ServiceResult<KhuyenMaiVm>> CreateAsync(KhuyenMaiCreateRequest request)
        {
            var entity = _mapper.Map<KhuyenMai>(request);

            entity.MaKM = "KM" + DateTime.Now.ToString("HHmmss");

            _context.KhuyenMais.Add(entity);
            await _context.SaveChangesAsync();

            return new ServiceResult<KhuyenMaiVm>
            {
                Success = true,
                Message = "Thêm khuyến mãi thành công.",
                Data = _mapper.Map<KhuyenMaiVm>(entity)
            };
        }

        public async Task<ServiceResult> UpdateAsync(string maKm, KhuyenMaiUpdateRequest request)
        {
            var entity = await _context.KhuyenMais.FindAsync(maKm);
            if (entity == null)
            {
                return new ServiceResult
                {
                    Success = false,
                    ErrorCode = ErrorCodes.NotFound,
                    Message = "Không tìm thấy khuyến mãi để cập nhật."
                };
            }

            _mapper.Map(request, entity);
            _context.KhuyenMais.Update(entity);
            await _context.SaveChangesAsync();

            return new ServiceResult
            {
                Success = true,
                Message = "Cập nhật khuyến mãi thành công."
            };
        }

        public async Task<ServiceResult> DeleteAsync(string maKm)
        {
            var entity = await _context.KhuyenMais.FindAsync(maKm);
            if (entity == null)
            {
                return new ServiceResult
                {
                    Success = false,
                    ErrorCode = ErrorCodes.NotFound,
                    Message = "Không tìm thấy khuyến mãi để xóa."
                };
            }

            _context.KhuyenMais.Remove(entity);
            await _context.SaveChangesAsync();

            return new ServiceResult
            {
                Success = true,
                Message = "Xóa khuyến mãi thành công."
            };
        }

        public async Task<ServiceResult> GanSanPhamAsync(string maKm, GanSanPhamKhuyenMaiRequest request)
        {
            var khuyenMaiExists = await _context.KhuyenMais.AnyAsync(x => x.MaKM == maKm);
            if (!khuyenMaiExists)
            {
                return new ServiceResult
                {
                    Success = false,
                    ErrorCode = ErrorCodes.NotFound,
                    Message = "Không tìm thấy khuyến mãi."
                };
            }

            var uniqueMaSPs = request.MaSanPhams.Distinct().ToList();

            var existingProductsCount = await _context.SanPhams
                .CountAsync(x => uniqueMaSPs.Contains(x.MaSP));

            if (existingProductsCount != uniqueMaSPs.Count)
            {
                return new ServiceResult
                {
                    Success = false,
                    ErrorCode = ErrorCodes.Validation,
                    Message = "Một hoặc nhiều sản phẩm không tồn tại trong hệ thống."
                };
            }

            // Truy vấn tập hợp để tìm các sản phẩm ĐÃ ĐƯỢC GÁN vào BẤT KỲ khuyến mãi nào
            var existedMappings = await _context.SanPham_KhuyenMais
                .Where(x => uniqueMaSPs.Contains(x.MaSP))
                .Select(x => x.MaSP)
                .ToListAsync();

            if (existedMappings.Any())
            {
                return new ServiceResult
                {
                    Success = false,
                    ErrorCode = ErrorCodes.Conflict,
                    Message = $"Các sản phẩm sau đã thuộc một khuyến mãi khác: {string.Join(", ", existedMappings)}"
                };
            }

            var entities = uniqueMaSPs.Select(maSp => new SanPham_KhuyenMai
            {
                MaKM = maKm,
                MaSP = maSp
            }).ToList();

            await _context.SanPham_KhuyenMais.AddRangeAsync(entities);
            await _context.SaveChangesAsync();

            return new ServiceResult
            {
                Success = true,
                Message = "Gán sản phẩm vào khuyến mãi thành công."
            };
        }

        public async Task<ServiceResult> XoaSanPhamAsync(string maKm, string maSp)
        {
            var entity = await _context.SanPham_KhuyenMais
                .FirstOrDefaultAsync(x => x.MaKM == maKm && x.MaSP == maSp);

            if (entity == null)
            {
                return new ServiceResult
                {
                    Success = false,
                    ErrorCode = ErrorCodes.NotFound,
                    Message = "Không tìm thấy sản phẩm trong khuyến mãi này."
                };
            }

            _context.SanPham_KhuyenMais.Remove(entity);
            await _context.SaveChangesAsync();

            return new ServiceResult
            {
                Success = true,
                Message = "Xóa sản phẩm khỏi khuyến mãi thành công."
            };
        }

        public async Task<ServiceResult<List<KhuyenMaiSanPhamVm>>> GetSanPhamsByKhuyenMaiAsync(string maKm)
        {
            var query = from sp in _context.SanPhams
                        join spkm in _context.SanPham_KhuyenMais on sp.MaSP equals spkm.MaSP
                        join km in _context.KhuyenMais on spkm.MaKM equals km.MaKM
                        where km.MaKM == maKm
                        select new { sp, km };

            var data = await query.ToListAsync();

            var resultList = data.Select(x =>
            {
                decimal giaSauKM = x.sp.DonGia;
                decimal giaTriKM = x.km.GiaTriKM ?? 0;

                if (x.km.LoaiKM == "PERCENT")
                {
                    giaSauKM = x.sp.DonGia - (x.sp.DonGia * giaTriKM / 100);
                }
                else if (x.km.LoaiKM == "AMOUNT")
                {
                    giaSauKM = x.sp.DonGia - giaTriKM;
                }

                if (giaSauKM < 0) giaSauKM = 0;

                return new KhuyenMaiSanPhamVm
                {
                    MaSP = x.sp.MaSP,
                    TenSP = x.sp.TenSP,
                    DonGia = x.sp.DonGia,
                    LoaiKM = x.km.LoaiKM,
                    GiaTriKM = giaTriKM,
                    GiaSauKhuyenMai = giaSauKM
                };
            }).ToList();

            return new ServiceResult<List<KhuyenMaiSanPhamVm>>
            {
                Success = true,
                Data = resultList
            };
        }
    }
}