# Module 05 — Delegates, Lambdas, Events and LINQ

So far we have passed **data** to methods. In this module we learn to pass **behavior**: "filter this list by this condition", "when stock runs low, notify me", "sort by this field". In C# this is done with delegates and lambdas, and on top of them sit events and LINQ — the tool that turns 20 lines of loops into one readable line. We finish with a few functional programming principles that make code easier to test and understand.

## Delegates — a method as a value

A delegate is a type that describes a method **signature**: which parameters it takes and what it returns. A variable of a delegate type holds a reference to a method, and you can "call" it.

```csharp
delegate int MathOp(int a, int b);            // define the type

static int Add(int a, int b) => a + b;

MathOp op = Add;                              // the method is a value
Console.WriteLine(op(2, 3));                  // 5

static int Apply(int x, int y, MathOp op) => op(x, y);
Console.WriteLine(Apply(10, 4, (a, b) => a - b));   // 6 — passing behavior
```

In practice you rarely define your own delegates, because .NET provides ready-made generic ones:

| Type | Signature | Example |
|---|---|---|
| `Action` | No parameters, no return value | `Action say = () => Console.WriteLine("hi");` |
| `Action<T1, T2...>` | Up to 16 parameters, no return value | `Action<string> log = m => Console.WriteLine(m);` |
| `Func<T1..., TResult>` | Parameters + return value (the last type argument) | `Func<int, int, int> add = (a, b) => a + b;` |
| `Predicate<T>` | `T` → `bool` | `Predicate<int> isEven = n => n % 2 == 0;` |

A delegate is **multicast**: `+=` adds another method, and invoking it calls all of them in order. This is the foundation of events.

## Lambdas

A lambda is an anonymous method written inline: `(parameters) => expression` or `(parameters) => { body }`. The compiler infers the parameter types from the delegate it is assigned to.

```csharp
Func<int, int> square = x => x * x;                 // one parameter — no parentheses
Func<int, int, int> max = (a, b) => a > b ? a : b;
Action<string> shout = s => { var u = s.ToUpper(); Console.WriteLine(u + "!"); };
Func<int> answer = () => 42;                        // no parameters
```

### Closures

A lambda can use variables from the scope in which it was created — and "remember" them even after the method that created it has returned:

```csharp
static Func<int> MakeCounter()
{
    int count = 0;
    return () => ++count;      // count is "captured" — it lives as long as the lambda lives
}

var next = MakeCounter();
Console.WriteLine($"{next()} {next()} {next()}");   // 1 2 3
```

This is powerful, but note: the lambda captures the **variable**, not its value. If the variable changes after the lambda is created, the lambda sees the new value.

## Events — publisher/subscriber

An event is a multicast delegate with protection: from the outside you can only subscribe (`+=`) and unsubscribe (`-=`), and only the class that declared it can raise it. This is how an object announces "something happened" without knowing who is listening.

```csharp
class LowStockEventArgs(string sku, int quantity) : EventArgs
{
    public string Sku { get; } = sku;
    public int Quantity { get; } = quantity;
}

class Inventory
{
    public event EventHandler<LowStockEventArgs>? LowStock;     // the convention: (sender, e)

    public void Sell(string sku, int qty)
    {
        // ... decrease stock ...
        if (remaining <= threshold)
            LowStock?.Invoke(this, new LowStockEventArgs(sku, remaining));   // ?. — in case there are no subscribers
    }
}

var inv = new Inventory();
inv.LowStock += (sender, e) => Console.WriteLine($"reorder {e.Sku}! only {e.Quantity} left");
inv.LowStock += (_, e) => emailService.Notify(e.Sku);       // a second subscriber — Inventory doesn't know about it
```

The .NET convention: `EventHandler<TEventArgs>`, parameters `(object? sender, TEventArgs e)`, and an event name in the past or present tense (`Changed`, `Clicked`, `LowStock`). When you work with WPF/WinForms on Day 3, every button click is an event like this.

Remember to unsubscribe (`-=`) from long-lived objects: a publisher that holds a reference to a subscriber prevents it from being collected by the GC.

## Extension methods — how LINQ "attaches" to every collection

`Where`, `Select` and the other operators are not methods of `List<T>` — they are **extension methods**: static methods that the compiler lets you call as if they belonged to the type. That is how LINQ works on any `IEnumerable<T>` without changing a single class. You can write your own to add operations to types you don't own (for example `string` or `DateTime`), or to keep your classes small. The rules: a `static` class, a `static` method, and a first parameter marked with `this`.

```csharp
static class StringExtensions
{
    public static string Truncate(this string s, int max) =>
        s.Length <= max ? s : s[..max] + "…";

    public static bool IsBlank(this string? s) => string.IsNullOrWhiteSpace(s);
}

Console.WriteLine("Hello, world".Truncate(5));    // Hello…
if (input.IsBlank()) { }                          // works on null too
```

Don't overdo it: extension methods suit general-purpose helper operations, not business logic that belongs in the class itself.

## Delegate or interface?

Both let you "inject behavior". Rule of thumb: when you need **a single operation** (a filter condition, a callback, a comparison) — use a delegate (`Func`/`Action`), because it is easy to pass as an inline lambda. When you need **a group of related operations** or state (a logger with `Log` and `Flush`, a repository with `Get`/`Add`/`Remove`) — use an interface. LINQ chose delegates because every operator needs exactly one function; `IComparer<T>` is an interface because sorting is a concept that deserves a name and reusable implementations. And if you're unsure — start with `Func`, and switch to an interface when the second operation shows up.

## LINQ — queries over collections

LINQ (Language Integrated Query) is a set of extension methods on `IEnumerable<T>` that take lambdas. Instead of a loop + `if` + a temporary list, you describe **what** you want:

```csharp
record Order(int Id, string Customer, string Category, decimal Total);
List<Order> orders = [ /* ... */ ];

// Filtering and projection
var names = orders.Where(o => o.Total > 100).Select(o => o.Customer);

// Sorting
var sorted = orders.OrderByDescending(o => o.Total).ThenBy(o => o.Customer);

// Grouping
foreach (var g in orders.GroupBy(o => o.Category))
    Console.WriteLine($"{g.Key}: {g.Count()} orders, {g.Sum(o => o.Total):N2}");

// Single element
var first = orders.First(o => o.Category == "Books");            // throws if none
var maybe = orders.FirstOrDefault(o => o.Total > 5000);          // null if none
var only = orders.Single(o => o.Id == 3);                        // throws if none or if more than one

// Checks and aggregation
bool any = orders.Any(o => o.Total > 1000);
bool all = orders.All(o => o.Total > 0);
int n = orders.Count(o => o.Customer == "Dana");
decimal max = orders.Max(o => o.Total), avg = orders.Average(o => o.Total);

// Converting to collections
Dictionary<string, decimal> perCustomer = orders
    .GroupBy(o => o.Customer)
    .ToDictionary(g => g.Key, g => g.Sum(o => o.Total));
List<int> ids = orders.Select(o => o.Id).ToList();
HashSet<string> categories = orders.Select(o => o.Category).ToHashSet();

// More useful ones: Skip/Take (paging), Distinct, OrderBy(x => x) , Reverse, Zip, Chunk, MinBy/MaxBy, Aggregate
var page2 = orders.Skip(10).Take(10);
```

### Method syntax vs. query syntax

There are two syntaxes for the same thing. **Method syntax** (above) is the common one; **query syntax** resembles SQL and is sometimes more readable for joins and complex groupings:

```csharp
var q = from o in orders
        where o.Category == "Electronics"
        orderby o.Total descending
        select new { o.Id, o.Customer };      // anonymous type
```

The compiler translates query syntax into method calls. Pick one and be consistent within a project.

### Deferred execution

This is the most important thing to understand about LINQ: `Where`, `Select`, `OrderBy` etc. **do not run** when you write them. They build a "plan", and that plan executes only when someone iterates over the result (`foreach`, `ToList`, `Count`, `First`...).

```csharp
var numbers = new List<int> { 1, 2, 3 };
var evens = numbers.Where(n => n % 2 == 0);    // nothing has happened yet
numbers.Add(4);
Console.WriteLine(string.Join(",", evens));     // 2,4 — the query runs now and sees the 4

var snapshot = numbers.Where(n => n % 2 == 0).ToList();   // ToList executes and freezes the result
```

Consequences: (1) a query that is consumed twice runs twice; (2) changes to the source after the query is defined affect the result; (3) an exception in a lambda is thrown when the query is consumed, not when it is defined. When you need a stable result — `ToList()` / `ToArray()`. Operators like `Count()`, `Sum()`, `First()`, `ToDictionary()` are "immediate" — they execute right away.

## Functional style

LINQ and lambdas invite a programming style that makes code easier to test and understand:

- **Pure functions**: same inputs → same output, with no side effects (they don't change state or write to the console/a file). Easy to test, easy to compose, thread-safe. `Reports.TotalValue(items)` in Lab 3 is one.
- **Immutability**: instead of modifying an object, create a new one. `record` with `with`, `IReadOnlyList<T>`, `ImmutableList<T>`. Less "who changed this?".
- **Composition**: small methods that take and return `Func`/`IEnumerable`, chained together: `orders.Where(IsRecent).Select(ToSummary).OrderBy(s => s.Date)`.
- **Separate computation from I/O**: compute the result in a pure function, and print/save it in an outer layer.

```csharp
record Cart(IReadOnlyList<Item> Items) { public decimal Total => Items.Sum(i => i.Price); }

static Cart ApplyDiscount(Cart cart, decimal pct) =>            // pure: doesn't touch cart
    cart with { Items = cart.Items.Select(i => i with { Price = i.Price * (1 - pct) }).ToList() };
```

You don't need to make all your code functional; but when a method can be pure — it's better if it is.

## Common mistakes

- **A query consumed several times** — runs several times. Store it once with `ToList()`.
- **`First()` without a check** — `InvalidOperationException: Sequence contains no elements`. Use `FirstOrDefault` + a `null` check, or `Any()` first.
- **Raising an event without `?.`** — `NullReferenceException` when there are no subscribers.
- **A lambda that modifies an outer variable** inside `Select`/`Where` — side effects in a query are confusing and run multiple times. LINQ is for computation; use a `foreach` loop for side effects.
- **Capturing a loop variable** in an old-style `for` — all the lambdas see the last value. In a modern `foreach` each iteration gets a fresh variable; in `for` it doesn't.
- **`OrderBy` inside a loop** — an O(n log n) sort on every iteration. Sort once.
- **12-step LINQ chains on a single line** — break them into named intermediate variables.

## Summary

- delegate = a method as a value. `Func`/`Action`/`Predicate` cover all needs; a lambda is the way to write them inline.
- Closures capture variables, not values.
- `event EventHandler<T>` + `?.Invoke` — a publisher that doesn't know its subscribers.
- LINQ: `Where/Select/OrderBy/GroupBy/First/Any/ToDictionary` — describe **what**, not **how**.
- Deferred execution: the query runs when consumed. `ToList()` to freeze it.
- Pure functions + immutability = code that is easy to test and understand.

## Further reading

- [Delegates](https://learn.microsoft.com/dotnet/csharp/programming-guide/delegates/)
- [Lambda expressions](https://learn.microsoft.com/dotnet/csharp/language-reference/operators/lambda-expressions)
- [Events](https://learn.microsoft.com/dotnet/csharp/programming-guide/events/)
- [LINQ overview](https://learn.microsoft.com/dotnet/csharp/linq/)
- [Standard query operators](https://learn.microsoft.com/dotnet/csharp/linq/standard-query-operators/)
- [Deferred execution](https://learn.microsoft.com/dotnet/csharp/linq/get-started/introduction-to-linq-queries#deferred-execution)
- [Functional programming in C#](https://learn.microsoft.com/dotnet/csharp/fundamentals/functional/)
