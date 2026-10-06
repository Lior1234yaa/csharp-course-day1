namespace Day1.Lab3;

/// <summary>Reports — pure functions over IEnumerable&lt;StockItem&gt; (they do not change anything).</summary>
public static class Reports
{
    // TODO 10: TotalValue — the sum of Value over all items
    public static decimal TotalValue(IEnumerable<StockItem> items) => throw new NotImplementedException();

    // TODO 11: ByCategory — for each category: number of items, total quantity, total value. Sorted by value, descending.
    //          Return an IEnumerable of the CategorySummary record (defined below).
    public static IEnumerable<CategorySummary> ByCategory(IEnumerable<StockItem> items) => throw new NotImplementedException();

    // TODO 12: TopByValue(items, n) — the n most valuable items in stock (by Value)
    public static IEnumerable<StockItem> TopByValue(IEnumerable<StockItem> items, int n) => throw new NotImplementedException();

    // TODO 13: LowStock — all items where IsLow, sorted by quantity ascending and then by name
    public static IEnumerable<StockItem> LowStock(IEnumerable<StockItem> items) => throw new NotImplementedException();

    // TODO 14: PriceIndex — a Dictionary<string, decimal> from SKU to price (ToDictionary)
    public static Dictionary<string, decimal> PriceIndex(IEnumerable<StockItem> items) => throw new NotImplementedException();

    // TODO 15 (bonus): SkusInBoth — a HashSet of the SKUs that appear in both list a and list b (IntersectWith / Intersect)
    public static HashSet<string> SkusInBoth(IEnumerable<StockItem> a, IEnumerable<StockItem> b) => throw new NotImplementedException();
}

public record CategorySummary(string Category, int ProductCount, int TotalQuantity, decimal TotalValue);
