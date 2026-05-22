using Microsoft.EntityFrameworkCore;
using WebCoffee.BackendServer.Data;

namespace WebCoffee.BackendServer.Services.BackgroundJobs
{
    public class TokenCleanupService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<TokenCleanupService> _logger;
        public TokenCleanupService(IServiceProvider serviceProvider, ILogger<TokenCleanupService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Tiến trình dọn dẹp Token rác đã khởi động.");
            var period = TimeSpan.FromHours(24);
            //TimeSpan period = TimeSpan.FromSeconds(30);
            using var timer = new PeriodicTimer(period);

            while (!stoppingToken.IsCancellationRequested && await timer.WaitForNextTickAsync(stoppingToken))
            {
                try
                {
                    await CleanUpExpiredTokensAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Lỗi xảy ra trong quá trình dọn dẹp Token.");
                }
            }
        }

        private async Task CleanUpExpiredTokensAsync()
        {
            _logger.LogInformation($"[{DateTime.Now}] Đang kiểm tra và dọn dẹp Token hết hạn/bị thu hồi...");

            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var deletedCount = await context.RefreshTokens
                    .Where(t => t.ExpiryDate <= DateTime.Now || t.IsRevoked)
                    .ExecuteDeleteAsync();

                if (deletedCount > 0)
                {
                    _logger.LogInformation($"[HOÀN TẤT] Đã dọn dẹp vĩnh viễn {deletedCount} token rác ra khỏi Database.");
                }
            }
        }
    }
}