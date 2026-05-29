using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebCoffee.BackendServer.Data;
using WebCoffee.BackendServer.Data.Entities;
using WebCoffee.ViewModels.Catalog.ChamCongs;

namespace WebCoffee.BackendServer.Services.ChamCongs
{
    public class ChamCongService : IChamCongService
    {
        private readonly AppDbContext _context;

        public ChamCongService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<ChamCongVm>> GetAllAsync()
        {
            return await _context.ChamCongs
                .Include(x => x.NhanVien)
                .Include(x => x.CaLam)
                .OrderByDescending(x => x.TgChamCong)
                .Select(x => new ChamCongVm
                {
                    MaChamCong = x.MaChamCong,
                    MaNV = x.MaNV,
                    TenNhanVien = x.NhanVien != null ? (x.NhanVien.HoNV + " " + x.NhanVien.TenNV) : "Nhan vien an danh",
                    MaCaLam = x.MaCaLam,
                    TenCa = x.CaLam != null ? x.CaLam.TenCa : "Chua ro ca",
                    TgChamCong = x.TgChamCong
                }).ToListAsync();
        }

        public async Task<bool> CheckInAsync(ChamCongVm request)
        {
            var chamCong = new ChamCong
            {
                MaChamCong = "CC_" + DateTime.Now.ToString("yyyyMMdd_HHmmss"),
                MaNV = request.MaNV,
                MaCaLam = request.MaCaLam,
                TgChamCong = DateTime.Now
            };

            _context.ChamCongs.Add(chamCong);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}