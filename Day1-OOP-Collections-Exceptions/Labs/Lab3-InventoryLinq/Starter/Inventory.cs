namespace Day1.Lab3;

public class Inventory
{
    // TODO 1: A private Dictionary<string, StockItem> field keyed by SKU (case-insensitive key:
    //         new Dictionary<string, StockItem>(StringComparer.OrdinalIgnoreCase))
    //         and a HashSet<string> field of categories (also OrdinalIgnoreCase).

    // TODO 2: A LowStock event of type EventHandler<LowStockEventArgs>

    // TODO 3: Items — IEnumerable<StockItem> of all items; Categories — IReadOnlySet<string>
    public IEnumerable<StockItem> Items => throw new NotImplementedException();
    public IReadOnlySet<string> Categories => throw new NotImplementedException();

    // TODO 4: Add — adds a new item. Duplicate SKU → InvalidOperationException. Updates the category set.
    public void Add(Product product, int quantity, int reorderLevel = 5) => throw new NotImplementedException();

    // TODO 5: Get — returns a StockItem by SKU, or throws KeyNotFoundException with a clear message.
    public StockItem Get(string sku) => throw new NotImplementedException();

    // TODO 6: Receive(sku, qty) — receive stock (qty must be positive).
    public void Receive(string sku, int quantity) => throw new NotImplementedException();

    // TODO 7: Sell(sku, qty) — a sale. Not enough stock → InvalidOperationException.
    //         After subtracting: if the item dropped to the threshold (IsLow) — raise the LowStock event.
    public void Sell(string sku, int quantity) => throw new NotImplementedException();

    // TODO 8: Find — takes a Func<StockItem, bool> and returns all items that match the condition.
    public IEnumerable<StockItem> Find(Func<StockItem, bool> predicate) => throw new NotImplementedException();

    // TODO 9: ApplyToCategory — takes a category name and an Action<StockItem> and runs it on every item in the category.
    public void ApplyToCategory(string category, Action<StockItem> action) => throw new NotImplementedException();
}
