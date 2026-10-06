namespace Day1.Lab3;

/// <summary>A product in the catalog — immutable data, hence a record.</summary>
public record Product(string Sku, string Name, string Category, decimal Price);

/// <summary>A stock item: a product + the quantity in the warehouse + the reorder threshold.</summary>
public class StockItem(Product product, int quantity, int reorderLevel)
{
    public Product Product { get; } = product;
    public int Quantity { get; internal set; } = quantity;
    public int ReorderLevel { get; } = reorderLevel;

    public decimal Value => Product.Price * Quantity;
    public bool IsLow => Quantity <= ReorderLevel;

    public override string ToString() =>
        $"{Product.Sku,-6} {Product.Name,-18} {Product.Category,-12} {Product.Price,8:N2} x{Quantity,4}  = {Value,10:N2}{(IsLow ? "  LOW" : "")}";
}

/// <summary>Data for the "low stock" event.</summary>
public class LowStockEventArgs(StockItem item) : EventArgs
{
    public StockItem Item { get; } = item;
}
