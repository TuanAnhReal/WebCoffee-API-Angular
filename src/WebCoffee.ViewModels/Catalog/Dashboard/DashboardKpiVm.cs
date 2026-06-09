namespace WebCoffee.ViewModels.Catalog.Dashboard
{
    /// <summary>
    /// KPI snapshot cho Dashboard. Tất cả fields phải được Service gán đầy đủ.
    /// Frontend không cần tính toán thêm – chỉ hiển thị.
    /// </summary>
    public class DashboardKpiVm
    {
        /// Thời điểm snapshot (server time)
        public DateTime SnapshotAt { get; set; }

        // ── DOANH THU ──────────────────────────────────────────────────────────
        public decimal RevenueToday { get; set; }
        public decimal RevenueYesterday { get; set; }
        /// Tăng trưởng %, làm tròn 1 chữ số thập phân. Dương = tăng, âm = giảm.
        public double RevenueGrowth { get; set; }

        // ── CHI PHÍ & LỢI NHUẬN ───────────────────────────────────────────────
        public decimal CostToday { get; set; }
        public decimal ProfitToday { get; set; }
        public decimal ProfitYesterday { get; set; }
        public double ProfitGrowth { get; set; }

        // ── HÓA ĐƠN ───────────────────────────────────────────────────────────
        public int InvoiceToday { get; set; }
        public int InvoiceYesterday { get; set; }
        public double InvoiceGrowth { get; set; }
        /// Số hóa đơn "Chờ pha chế" quá 15 phút
        public int PendingOrders { get; set; }

        // ── KHÁCH HÀNG ────────────────────────────────────────────────────────
        public int CustomerToday { get; set; }
        public int CustomerYesterday { get; set; }
        public double CustomerGrowth { get; set; }

        // ── BÀN ───────────────────────────────────────────────────────────────
        public int TotalTables { get; set; }
        public int ActiveTables { get; set; }
        /// Số bàn đang phục vụ > 2 giờ (chưa thanh toán)
        public int LongServingTables { get; set; }

        // ── KHUYẾN MÃI ────────────────────────────────────────────────────────
        public int ActivePromotions { get; set; }
        public int PromotionProducts { get; set; }
        /// Số KM sắp hết hạn trong 24h tới
        public int ExpiringPromos { get; set; }
        public decimal DiscountAmountToday { get; set; }
        public decimal PromotionRevenueToday { get; set; }

        // ── CẢNH BÁO VẬN HÀNH ────────────────────────────────────────────────
        public List<DashboardAlertVm> Alerts { get; set; } = [];
    }

    public class DashboardAlertVm
    {
        /// Mã loại: "PENDING_ORDER" | "LONG_SERVING" | "EXPIRING_PROMO"
        public string Type { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public int Count { get; set; }
        public AlertSeverity Severity { get; set; }
    }

    public enum AlertSeverity { Info, Warning, Danger }
}
