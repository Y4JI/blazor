namespace POSProductDashboard.Models;

public class Product
{
    public string UPC { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Cost { get; set; }
    public decimal SellingPrice { get; set; }
    public string Status { get; set; } = "Active"; // Active, Needs Review, Draft

    public decimal Margin => SellingPrice > 0 ? Math.Round(((SellingPrice - Cost) / SellingPrice) * 100, 1) : 0;

    public Product Clone() => new()
    {
        UPC = UPC,
        Description = Description,
        Cost = Cost,
        SellingPrice = SellingPrice,
        Status = Status
    };
}