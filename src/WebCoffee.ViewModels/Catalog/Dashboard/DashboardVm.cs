namespace WebCoffee.ViewModels.Catalog.Dashboard
{
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