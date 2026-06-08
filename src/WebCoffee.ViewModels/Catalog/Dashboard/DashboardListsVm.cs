using System.Collections.Generic;

namespace WebCoffee.ViewModels.Catalog.Dashboard
{
    public class DashboardListsVm
    {
        public int ActiveTables { get; set; }
        public int TotalTables { get; set; }
        public List<TopProductVm> TopProducts { get; set; } = new List<TopProductVm>();
        public List<TopPromotionVm> TopPromotions { get; set; } = new List<TopPromotionVm>();
        public List<TableStatusVm> TableStatuses { get; set; } = new List<TableStatusVm>();
        public List<RecentInvoiceVm> RecentInvoices { get; set; } = new List<RecentInvoiceVm>();
    }
}