using POSProductDashboard.Models;

namespace POSProductDashboard.Services;

public class ProductService
{
    private readonly List<Product> _products = new();
    private readonly object _lock = new();

    public int TotalCompanies { get; set; } = 8;
    public int SeedTotalProductsOffset { get; set; } = 1278; // 1278 + 6 items = 1,284 as shown in Figma
    public int SeedNeedsReviewOffset { get; set; } = 22; // 22 + 1 item = 23 as shown in Figma

    public ProductService()
    {
        // Seed initial sample data matching the exact Figma screenshots
        _products.AddRange(new List<Product>
        {
            new Product { UPC = "072225243105", Description = "Organic Honey Almond Granola (16oz)", Cost = 3.20m, SellingPrice = 5.99m, Status = "Active" },
            new Product { UPC = "041196910242", Description = "Whole Milk Greek Yogurt (32oz)", Cost = 1.80m, SellingPrice = 3.49m, Status = "Active" },
            new Product { UPC = "028400091496", Description = "Classic Potato Chips Salted XL", Cost = 1.10m, SellingPrice = 2.29m, Status = "Active" },
            new Product { UPC = "012000000133", Description = "Diet Cola Soda Cans (12-pack)", Cost = 4.50m, SellingPrice = 7.99m, Status = "Needs Review" },
            new Product { UPC = "073110250041", Description = "Stainless Steel Kitchen Scissors", Cost = 5.40m, SellingPrice = 11.99m, Status = "Active" },
            new Product { UPC = "049000028914", Description = "Sparkling Water Lime (8-pack)", Cost = 3.10m, SellingPrice = 5.49m, Status = "Draft" }
        });
    }

    public List<Product> GetAll()
    {
        lock (_lock)
        {
            return _products.Select(p => p.Clone()).ToList();
        }
    }

    public Product? GetByUpc(string upc)
    {
        lock (_lock)
        {
            return _products.FirstOrDefault(p => p.UPC == upc)?.Clone();
        }
    }

    public int GetCatalogCount()
    {
        lock (_lock)
        {
            return _products.Count;
        }
    }

    public int GetDisplayTotalProducts()
    {
        lock (_lock)
        {
            return SeedTotalProductsOffset + _products.Count;
        }
    }

    public int GetDisplayNeedsReviewCount()
    {
        lock (_lock)
        {
            return SeedNeedsReviewOffset + _products.Count(p => p.Status == "Needs Review");
        }
    }

    public List<Product> GetRecent(int count = 5)
    {
        lock (_lock)
        {
            return _products.Take(count).Select(p => p.Clone()).ToList();
        }
    }

    public void Add(Product product)
    {
        lock (_lock)
        {
            _products.Insert(0, product.Clone());
        }
    }

    public void Update(string originalUpc, Product updatedProduct)
    {
        lock (_lock)
        {
            var existing = _products.FirstOrDefault(p => p.UPC == originalUpc);
            if (existing != null)
            {
                existing.UPC = updatedProduct.UPC;
                existing.Description = updatedProduct.Description;
                existing.Cost = updatedProduct.Cost;
                existing.SellingPrice = updatedProduct.SellingPrice;
                existing.Status = updatedProduct.Status;
            }
        }
    }

    public void Delete(string upc)
    {
        lock (_lock)
        {
            var product = _products.FirstOrDefault(p => p.UPC == upc);
            if (product != null)
            {
                _products.Remove(product);
            }
        }
    }
}