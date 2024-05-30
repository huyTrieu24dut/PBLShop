namespace PBLShop.ViewModels
{
    public class ChartDataVM
    {
        public int TotalRevenue { get; set; } = 0;
        public int TotalProduct { get; set; } = 0;
        public int TotalInvoice { get; set; } = 0;
        public List<ProductData>? ProductsData { get; set; }
        public List<TurnoverData>? TurnoverData { get; set; }
        public List<GrowthData>? GrowthData { get; set; }
    }

    public class ProductData
    {
        public string? Name { get; set; }
        public int Quantity { get; set; }
    }

    public class TurnoverData
    {
        public string? Product { get; set; }
        public int Revenue { get; set; }
    }

    public class GrowthData
    {
        public string? Month { get; set; }
        public int TotalProducts { get; set; }
        public int TotalRevenue { get; set; }
    }
}
