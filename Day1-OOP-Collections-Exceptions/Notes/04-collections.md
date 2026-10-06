# Module 04 — Collections, Generics and Efficient Data Management

Almost every program manages "a lot of something": a list of books, inventory by SKU, a queue of tasks. .NET ships with a rich set of collections in `System.Collections.Generic`, and each one is good at something different. In this module we'll learn the important ones, understand **how to choose** (the Big-O table is the tool), write our own generic class, and talk about how to expose collections to the outside world without losing control.

## Arrays

An array is a contiguous block of memory of **fixed** size. It is the fastest for access by index, but you can't add elements to it.

```csharp
int[] scores = [90, 75, 88];             // collection expression (C# 12)
int[] zeros = new int[5];                // 5 zeros
Console.WriteLine(scores[0]);            // 90 — indexes start at 0
Console.WriteLine(scores[^1]);           // 88 — from the end
int[] middle = scores[1..];              // range: from index 1 to the end
Array.Sort(scores);

int[,] grid = { { 1, 2, 3 }, { 4, 5, 6 } };      // two-dimensional (matrix): 2 rows, 3 columns
Console.WriteLine(grid[1, 2]);                   // 6
Console.WriteLine(grid.GetLength(0));            // 2

int[][] jagged = [[1], [2, 3], [4, 5, 6]];       // "jagged": an array of arrays of different lengths
Console.WriteLine(jagged[2].Length);             // 3
```

A two-dimensional array suits a rectangular matrix (a game board); a jagged array — when each row has a different length. Arrays are reference types: passing one to a method does not copy it.

## `List<T>` — the dynamic list

`List<T>` is an array that knows how to grow. It's the collection you'll use the most.

```csharp
var names = new List<string> { "Dana", "Yossi" };
names.Add("Noa");
names.Insert(0, "Avi");
names.Remove("Yossi");                  // by value (true/false)
names.RemoveAt(0);                      // by index
bool has = names.Contains("Noa");       // O(n) — a scan
int idx = names.IndexOf("Noa");
names.Sort();
Console.WriteLine($"{names.Count} items: {string.Join(", ", names)}");
```

`Count` (not `Length`) is the number of elements; `Capacity` is how much space has been allocated. When the List fills up, it allocates an array twice the size and copies — so if you know in advance how many elements there will be, `new List<int>(capacity: 100_000)` saves allocations.

## `Dictionary<TKey, TValue>` — lookup by key

A dictionary maps a key to a value with constant-time lookup (on average) — it doesn't matter whether there are 10 or 10 million elements. It is **the** tool for lookup "by ID".

```csharp
var stock = new Dictionary<string, int>
{
    ["apple"] = 10,
    ["banana"] = 0,
};
stock["cherry"] = 25;                             // add or update
stock["apple"] += 5;
if (stock.TryGetValue("kiwi", out int qty)) { }   // safe lookup — no exception
bool exists = stock.ContainsKey("apple");
stock.Remove("banana");
foreach (var (fruit, count) in stock) Console.WriteLine($"{fruit}: {count}");

var byName = new Dictionary<string, Product>(StringComparer.OrdinalIgnoreCase);   // case-insensitive key
```

`stock["kiwi"]` on a key that doesn't exist throws `KeyNotFoundException` — hence `TryGetValue`. The key must be stable: don't use an object whose `GetHashCode` changes (module 03).

## `HashSet<T>`, `Queue<T>`, `Stack<T>`

- **`HashSet<T>`** — a set without duplicates, O(1) membership check. "Have we already seen this ID?" `Add` returns `false` if the element already exists. Set operations: `UnionWith`, `IntersectWith`, `ExceptWith`.
- **`Queue<T>`** — a queue: first in, first out (FIFO). `Enqueue`, `Dequeue`, `Peek`. Processing tasks in order of arrival.
- **`Stack<T>`** — a stack: last in, first out (LIFO). `Push`, `Pop`, `Peek`. Undo, "back" navigation, parsing.

```csharp
var seen = new HashSet<int>();
foreach (var id in [17, 3, 17]) if (!seen.Add(id)) Console.WriteLine($"duplicate {id}");

var tasks = new Queue<string>(["build", "test", "deploy"]);
while (tasks.Count > 0) Console.WriteLine(tasks.Dequeue());

var undo = new Stack<string>();
undo.Push("typed a"); undo.Push("bold");
Console.WriteLine(undo.Pop());      // bold
```

## `IEnumerable<T>` and `foreach`

All collections implement `IEnumerable<T>` — the interface that says "you can iterate over me with `foreach`". It's also the type you should accept as a parameter when all the method needs is to iterate over the elements: it will then work with an array, a `List`, a `HashSet`, a LINQ result — anything.

You can also produce an `IEnumerable<T>` yourself with `yield return` — the elements are produced **on demand**, one at a time:

```csharp
static IEnumerable<int> Evens(int max)
{
    for (int i = 0; i <= max; i += 2)
        yield return i;                        // "return this, and continue from here next time"
}

foreach (var n in Evens(1_000_000).Take(3)) Console.WriteLine(n);   // only 3 are computed
```

This is the basis of LINQ's deferred execution (module 05). Important: `IEnumerable<T>` is only "the ability to iterate" — it has no `Count`, indexer or `Add`. If you need those, ask for an `IReadOnlyList<T>` or a `List<T>`.

## Generics — code that works for any type

`List<T>` is **generic**: `T` is a placeholder for a type chosen at the point of use. Without generics we would need `IntList`, `StringList`... or a `List` of `object` with casting everywhere (as in .NET 1.0). Generics give you both type safety and performance.

You can write generic classes and methods yourself:

```csharp
interface IEntity { int Id { get; } }

class Repository<T> where T : IEntity            // constraint: T must implement IEntity
{
    private readonly List<T> _items = [];
    private readonly Dictionary<int, T> _byId = [];   // index — O(1) lookup by Id

    public IReadOnlyList<T> All => _items;

    public void Add(T item)
    {
        if (!_byId.TryAdd(item.Id, item))           // possible because T is an IEntity
            throw new InvalidOperationException($"Id {item.Id} exists");
        _items.Add(item);
    }

    public T? GetById(int id) => _byId.GetValueOrDefault(id);
}

record Customer(int Id, string Name) : IEntity;
var repo = new Repository<Customer>();
```

Common constraints: `where T : class` (reference types only), `where T : struct`, `where T : new()` (has a parameterless constructor), `where T : IComparable<T>` (can be compared), `where T : notnull`. Without a constraint, all you can do with `T` is what you can do with `object`.

A generic method: `static T Max<T>(T a, T b) where T : IComparable<T> => a.CompareTo(b) >= 0 ? a : b;` — the compiler infers `T` from the arguments.

## Choosing the right collection

| Need | Collection | Access by index | Lookup by value/key | Add at end | Remove |
|---|---|---|---|---|---|
| Fixed size, fast | `T[]` | O(1) | O(n) | — | — |
| General ordered list | `List<T>` | O(1) | O(n) | O(1)* | O(n) |
| Key → value | `Dictionary<K,V>` | — | O(1) | O(1)* | O(1) |
| Uniqueness / membership | `HashSet<T>` | — | O(1) | O(1)* | O(1) |
| FIFO | `Queue<T>` | — | — | O(1)* | O(1) from the front |
| LIFO | `Stack<T>` | — | — | O(1)* | O(1) from the top |
| Always sorted by key | `SortedDictionary<K,V>` | — | O(log n) | O(log n) | O(log n) |

`*` — amortized: occasionally there is a reallocation, but on average it's constant.

The deciding questions: Do you need order? Do you need lookup by key? Are there duplicates? How many elements? A `List<T>` with `Contains` in a loop over 100,000 elements = 10 billion comparisons. A `HashSet` = 100,000. That's the difference between a second and minutes.

## Immutability and `IReadOnlyList<T>`

When a class holds a private `List<T>` and exposes it as `public List<T> Items { get; }`, anyone can call `obj.Items.Clear()` — encapsulation is broken. The solution: expose a **read-only interface**.

```csharp
class Library
{
    private readonly List<Book> _books = [];
    public IReadOnlyList<Book> Books => _books;       // can see, can't change
    public void Add(Book b) { /* validation */ _books.Add(b); }
}
```

`List<T>` implements `IReadOnlyList<T>`, so there is no copying — just a restricted API. Other options: `IReadOnlyCollection<T>`, `IReadOnlyDictionary<K,V>`, `_books.AsReadOnly()` (a wrapper), and `ImmutableList<T>` from `System.Collections.Immutable` when you need a collection that truly cannot be changed (every "change" returns a new collection). Arrays and records are also part of the story: a `record` with an `IReadOnlyList<T>` is a fully immutable piece of data.

## Efficient data management

A few principles that save a lot of runtime — and bugs:

1. **In-memory index**: if you look things up by key frequently, keep a `Dictionary` alongside the `List` (as in `Repository<T>` above). Memory is cheap; scans are expensive.
2. **Capacity**: `new List<T>(n)` / `new Dictionary<K,V>(n)` when you know the size. Saves doublings and copies.
3. **Don't copy for no reason**: `ToList()` on every line creates a copy. Use it only when you need a "snapshot" or when you iterate over the collection more than once.
4. **`StringBuilder`** for building a string in a loop — `+=` on a string creates a new string every time.
5. **`Span<T>`** — a "window" over an array (or part of one) without copying. Useful for high-performance text/binary processing: `data.AsSpan(2, 4)`. You won't need it day to day, but it's worth knowing it's there — and that many .NET APIs accept it.
6. **Measure before optimizing**: `Stopwatch` or BenchmarkDotNet. Most code doesn't need optimization — it needs the right collection.

## Common mistakes

- **Modifying a collection inside a `foreach`** over it — `InvalidOperationException: Collection was modified`. Iterate over a copy (`.ToList()`) or use `RemoveAll(predicate)`.
- **`List.Contains` / `IndexOf` in a loop** over large collections — O(n²). Use `HashSet`/`Dictionary`.
- **`dict[key]` without checking** — `KeyNotFoundException`. Use `TryGetValue` / `GetValueOrDefault`.
- **Exposing a public `List<T>`** — anyone can empty it. Use `IReadOnlyList<T>`.
- **A key that changes** in a `Dictionary`/`HashSet` — the element "disappears". Use immutable keys (string, int, record).
- **An `IEnumerable` consumed twice** — if it is a LINQ result, the query will run twice (and may sometimes give different results). Call `ToList()` once.
- **LINQ's `Count()` instead of a List's `Count`** — the former may scan the entire collection.

## Summary

- An array for fixed size; `List<T>` for a general list; `Dictionary` for lookup by key; `HashSet` for uniqueness; `Queue`/`Stack` for processing order.
- `IEnumerable<T>` is the common denominator — accept it as a parameter, produce it with `yield`.
- Generics (`Repository<T> where T : IEntity`) = one piece of code for all types, with type safety.
- The Big-O table is the tool for choosing. Repeated lookup = an index (`Dictionary`).
- Expose collections as `IReadOnlyList<T>`; set capacity when you know it; don't copy for no reason.

## Further reading

- [Collections (C#)](https://learn.microsoft.com/dotnet/csharp/language-reference/builtin-types/collections)
- [Arrays](https://learn.microsoft.com/dotnet/csharp/language-reference/builtin-types/arrays)
- [Generic classes and methods](https://learn.microsoft.com/dotnet/csharp/fundamentals/types/generics)
- [Constraints on type parameters](https://learn.microsoft.com/dotnet/csharp/programming-guide/generics/constraints-on-type-parameters)
- [Iterators (yield)](https://learn.microsoft.com/dotnet/csharp/iterators)
- [Selecting a collection class](https://learn.microsoft.com/dotnet/standard/collections/selecting-a-collection-class)
- [Memory<T> and Span<T> usage guidelines](https://learn.microsoft.com/dotnet/standard/memory-and-spans/memory-t-usage-guidelines)
