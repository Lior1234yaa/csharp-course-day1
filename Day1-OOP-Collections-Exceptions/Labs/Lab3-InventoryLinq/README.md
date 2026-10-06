# Lab 3 — Inventory: Dictionary, HashSet, LINQ, Delegates and Events (60 minutes)

## Goal

Build a small inventory-management module that uses the "right" tool for each job: a `Dictionary` for lookup by SKU, a `HashSet` for categories, `Func`/`Action` parameters so you can write one flexible method instead of ten, a `LowStock` event that separates "what happened" from "who reacts", and LINQ reports that are pure functions.

## What you need to know first

Modules 04 and 05. Labs 1–2 are recommended.

## Getting started

```bash
cd Labs/Lab3-InventoryLinq/Starter
dotnet run
```

`Models.cs` (the `Product` record, `StockItem`, `LowStockEventArgs`) and `Program.cs` are already written. Your work is in `Inventory.cs` and `Reports.cs`.

## Steps

### Step 1 — `Inventory` (25 min)

1. **TODO 1** — Fields: a `Dictionary<string, StockItem>` and a `HashSet<string>`, both with `StringComparer.OrdinalIgnoreCase`.
2. **TODO 2** — `public event EventHandler<LowStockEventArgs>? LowStock;`
3. **TODO 3** — `Items` and `Categories` (read-only exposure).
4. **TODO 4–6** — `Add` (duplicate SKU → exception), `Get` (missing → `KeyNotFoundException`), `Receive`.
5. **TODO 7** — `Sell`: check the quantity, subtract it, and if `IsLow` — raise the event. Don't forget `?.Invoke`.
6. **TODO 8** — `Find(Func<StockItem, bool>)` — one line with `Where`.
7. **TODO 9** — `ApplyToCategory(string, Action<StockItem>)`.

### Step 2 — `Reports` (20 min)

8. **TODO 10** — `TotalValue` (`Sum`).
9. **TODO 11** — `ByCategory` — `GroupBy` + `Select` into `CategorySummary` + `OrderByDescending`.
10. **TODO 12** — `TopByValue` (`OrderByDescending` + `Take`).
11. **TODO 13** — `LowStock` (`Where` + `OrderBy` + `ThenBy`).
12. **TODO 14** — `PriceIndex` (`ToDictionary`).

### Step 3 — Wiring it up (15 min)

13. **TODO 16** — In `Program.cs`: subscribe to `LowStock` with a lambda that prints a warning. Run the program and check that the warnings appear during the sales.

## Acceptance criteria

- [ ] `inv.Sell("k-100", 1)` works even in lowercase.
- [ ] Selling 8 Keyboards (from 12 down to 4, threshold 5) prints `!! LOW STOCK: Keyboard (4 left, reorder at 5)`.
- [ ] Selling more than is in stock throws `InvalidOperationException` with a message that includes the available quantity — and the program keeps running.
- [ ] `Find(i => i.Product.Price < 50)` returns exactly the three cheap items (2 × Cable, Mouse Pad).
- [ ] `ByCategory` is sorted by value in descending order, with `Displays` first.
- [ ] `TopByValue(items, 3)` returns 3 items.
- [ ] `Reports` does not change anything in the inventory (no `set` / `Add` / `Remove` inside it).
- [ ] There is no `ToUpper()`/`ToLower()` in the code — comparison is done through `StringComparer`.

## Bonus

- **TODO 15** — `SkusInBoth(a, b)` using `HashSet.IntersectWith`.
- A second event subscriber that collects a list of reorders (`List<string>`), printed at the end.
- Demonstrate deferred execution: define `var low = Reports.LowStock(inv.Items);` **before** a sale and print it **after** — what happens? And what if you add `.ToList()`?
- Write one of the reports in query syntax (`from ... where ... select`).

## Hints

- `Dictionary.TryGetValue(key, out var item)` — lookup without an exception.
- `new Dictionary<string, StockItem>(StringComparer.OrdinalIgnoreCase)` — case-insensitive keys.
- `LowStock?.Invoke(this, new LowStockEventArgs(item));`
- `items.GroupBy(i => i.Product.Category).Select(g => new CategorySummary(g.Key, g.Count(), g.Sum(...), g.Sum(...)))`.
- In `ApplyToCategory`, call `.ToList()` before the `foreach`. If the action changes the collection while you are iterating over it, without this "snapshot" you will get `Collection was modified` (see deferred execution in Notes/05).
