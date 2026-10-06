// =====================================================================
// Day1.Demo.Collections — collections and generics
// What the demo shows:
//   * arrays: one-dimensional, two-dimensional, jagged, collection expressions
//   * List<T>, Dictionary<K,V>, HashSet<T>, Queue<T>, Stack<T>
//   * IEnumerable<T> and foreach, yield return
//   * a generic class Repository<T> with a constraint (where T : IEntity)
//   * IReadOnlyList<T> — exposing a collection without allowing changes
//   * efficient data management: in-memory index, capacity, Span<T>
// Run:  dotnet run
// =====================================================================

Console.WriteLine("=== 1. Arrays ===");
int[] scores = [90, 75, 88];
int[,] grid = { { 1, 2, 3 }, { 4, 5, 6 } };
int[][] jagged = [[1], [2, 3], [4, 5, 6]];
Console.WriteLine($"scores.Length={scores.Length}, first={scores[0]}, last={scores[^1]}");
Console.WriteLine($"grid[1,2]={grid[1, 2]}, rows={grid.GetLength(0)}, cols={grid.GetLength(1)}");
Console.WriteLine($"jagged[2].Length={jagged[2].Length}");
Array.Sort(scores);
Console.WriteLine($"sorted: {string.Join(", ", scores)}");

Console.WriteLine("\n=== 2. List<T> ===");
var names = new List<string> { "Dana", "Yossi" };
names.Add("Noa");
names.Insert(0, "Avi");
names.Remove("Yossi");
Console.WriteLine($"count={names.Count}, contains Noa? {names.Contains("Noa")}, index of Noa = {names.IndexOf("Noa")}");
Console.WriteLine(string.Join(" | ", names));

Console.WriteLine("\n=== 3. Dictionary<K,V> ===");
var stock = new Dictionary<string, int>
{
    ["apple"] = 10,
    ["banana"] = 0,
};
stock["cherry"] = 25;               // add/update
stock["apple"] += 5;
if (stock.TryGetValue("banana", out int bananas)) Console.WriteLine($"banana: {bananas}");
Console.WriteLine($"has 'kiwi'? {stock.ContainsKey("kiwi")}");
foreach (var (key, value) in stock) Console.WriteLine($"  {key} -> {value}");

Console.WriteLine("\n=== 4. HashSet / Queue / Stack ===");
var tags = new HashSet<string> { "c#", "dotnet", "c#" };   // the duplicate is silently ignored
Console.WriteLine($"tags: {string.Join(",", tags)} (count={tags.Count}), added 'oop'? {tags.Add("oop")}, added again? {tags.Add("oop")}");

var queue = new Queue<string>();
queue.Enqueue("first"); queue.Enqueue("second");
Console.WriteLine($"queue dequeue → {queue.Dequeue()} (FIFO)");

var stack = new Stack<string>();
stack.Push("first"); stack.Push("second");
Console.WriteLine($"stack pop → {stack.Pop()} (LIFO)");

Console.WriteLine("\n=== 5. IEnumerable<T> and yield ===");
foreach (var n in EvenNumbers(10)) Console.Write($"{n} ");
Console.WriteLine();
IEnumerable<int> lazy = EvenNumbers(1_000_000);   // not computed until you iterate over it
Console.WriteLine($"first 3 of a million: {string.Join(",", lazy.Take(3))}");

Console.WriteLine("\n=== 6. Generic Repository<T> ===");
var repo = new Repository<Customer>();
repo.Add(new Customer(1, "Dana"));
repo.Add(new Customer(2, "Yossi"));
repo.Add(new Customer(3, "Noa"));
Console.WriteLine($"GetById(2) = {repo.GetById(2)?.Name}");
Console.WriteLine($"GetById(99) = {(repo.GetById(99) is null ? "null" : "found")}");
IReadOnlyList<Customer> all = repo.All;   // the caller can see, but can't add/remove
Console.WriteLine($"count via IReadOnlyList = {all.Count}");
// all.Add(...) — doesn't exist on IReadOnlyList — encapsulation of the collection

Console.WriteLine("\n=== 7. Efficiency: capacity and Span<T> ===");
var big = new List<int>(capacity: 100_000);   // avoids repeated reallocations
for (int i = 0; i < 100_000; i++) big.Add(i);
Console.WriteLine($"big.Count={big.Count}, Capacity={big.Capacity}");

int[] data = [1, 2, 3, 4, 5, 6];
Span<int> middle = data.AsSpan(2, 2);          // a "window" over the array without copying
middle[0] = 30;
Console.WriteLine($"after Span write: {string.Join(",", data)}");

// ------------------------------------------------------------------
static IEnumerable<int> EvenNumbers(int max)
{
    for (int i = 0; i <= max; i += 2)
        yield return i;   // returns one item at a time, on demand
}

interface IEntity { int Id { get; } }

record Customer(int Id, string Name) : IEntity;

// a generic class with a constraint: T must implement IEntity so we can access Id
class Repository<T> where T : IEntity
{
    private readonly List<T> _items = [];
    private readonly Dictionary<int, T> _byId = [];   // index for O(1) lookup

    public IReadOnlyList<T> All => _items;
    public int Count => _items.Count;

    public void Add(T item)
    {
        if (_byId.ContainsKey(item.Id)) throw new InvalidOperationException($"Id {item.Id} already exists");
        _items.Add(item);
        _byId[item.Id] = item;
    }

    public T? GetById(int id) => _byId.TryGetValue(id, out var item) ? item : default;

    public bool Remove(int id)
    {
        if (!_byId.Remove(id, out var item)) return false;
        _items.Remove(item);
        return true;
    }
}
