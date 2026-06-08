namespace WebCoffee.ViewModels.Catalog.Dashboard
{
    public class DashboardKpiVm
    {
        public decimal RevenueToday { get; set; }
        public double RevenueGrowth { get; set; }

        public int InvoiceToday { get; set; }
        public double InvoiceGrowth { get; set; }

        public int CustomerToday { get; set; }
        public double CustomerGrowth { get; set; }

        public decimal ProfitToday { get; set; }
        public double ProfitGrowth { get; set; }

        public int ActiveTables { get; set; }
        public int TotalTables { get; set; }

        public int ActivePromotions { get; set; }
        public int PromotionProducts { get; set; }
        public decimal DiscountAmountToday { get; set; }
        public decimal PromotionRevenueToday { get; set; }

        public List<string> Alerts { get; set; } = new List<string>();
    }
}