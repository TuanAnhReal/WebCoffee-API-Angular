using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebCoffee.BackendServer.Data;
using WebCoffee.BackendServer.Data.Entities;
using WebCoffee.ViewModels.Catalog.CaLams;

namespace WebCoffee.BackendServer.Services.CaLams
{
    public class CaLamService : ICaLamService
    {
        private readonly AppDbContext _context;

        public CaLamService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<CaLamVm>> GetAllAsync()
        {
            return await _context.CaLams
                .Select(x => new CaLamVm
                {
                    MaCaLam = x.MaCaLam,
                    TenCa = x.TenCa,
                    TgVaoCa = x.TgVaoCa,
                    TgRaCa = x.TgRaCa
                }).ToListAsync();
        }

        public async Task<bool> CreateAsync(CaLamVm request)
        {
            var caLam = new CaLam
            {
                MaCaLam = request.MaCaLam,
                TenCa = request.TenCa,
                TgVaoCa = request.TgVaoCa,
                TgRaCa = request.TgRaCa
            };

            _context.CaLams.Add(caLam);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}