namespace WebCoffee.ViewModels.Catalog.Dashboard
{
    public class DashboardVm
    {
        // KPI Chính
        public decimal RevenueToday { get; set; }
        public double RevenueGrowth { get; set; }

        public int InvoiceToday { get; set; }
        public double InvoiceGrowth { get; set; }

        public int CustomerToday { get; set; }
        public double CustomerGrowth { get; set; }

        public decimal ProfitToday { get; set; }
        public double ProfitGrowth { get; set; }

        // KPI Bàn
        public int ActiveTables { get; set; }
        public int TotalTables { get; set; }

        // KPI Khuyến mãi
        public int ActivePromotions { get; set; }
        public int PromotionProducts { get; set; }
        public decimal DiscountAmountToday { get; set; }
        public decimal PromotionRevenueToday { get; set; }

        // Dashboard Lists
        public List<TopProductVm> TopProducts { get; set; } = [];
        public List<TopPromotionVm> TopPromotions { get; set; } = [];
        public List<TableStatusVm> TableStatuses { get; set; } = [];
        public List<RecentInvoiceVm> RecentInvoices { get; set; } = [];

        // Chart
        public List<decimal> RevenueChart { get; set; } = [];
    }

    public class TopProductVm
    {
        public string ProductName { get; set; } = "";
        public int Quantity { get; set; }
        public decimal Revenue { get; set; }
    }

    public class TopPromotionVm
    {
        public string PromoName { get; set; } = "";
        public int OrdersCount { get; set; }
        public decimal Revenue { get; set; }
    }

    public class TableStatusVm
    {
        public string TableName { get; set; } = "";
        public string Status { get; set; } = "";
        public string ServingTime { get; set; } = "";
    }

    public class RecentInvoiceVm
    {
        public string InvoiceId { get; set; } = "";
        public decimal Amount { get; set; }
        public string Status { get; set; } = "";
    }
}