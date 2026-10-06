# Cheat-Sheet — C#, .NET CLI and Visual Studio

A condensed reference sheet for the course. We recommend printing it (2 pages) or keeping it open in a tab.

## 1. C# syntax — the basics

| Topic | Example |
|------|-------|
| Variable with type inference | `var count = 5;` |
| Variable with explicit type | `int count = 5;` |
| Constant | `const double Pi = 3.14;` |
| Nullable | `string? name = null;` |
| Null check | `if (name is null) ...` / `name ?? "default"` / `name?.Length` |
| Condition | `if (x > 0) { } else if (x < 0) { } else { }` |
| Conditional expression | `var sign = x >= 0 ? "+" : "-";` |
| Loop | `for (int i = 0; i < 10; i++) { }` |
| foreach | `foreach (var item in items) { }` |
| while | `while (cond) { }` / `do { } while (cond);` |
| switch (expression) | `var s = x switch { 0 => "zero", > 0 => "pos", _ => "neg" };` |
| Pattern matching | `if (shape is Circle { Radius: > 10 } c) ...` |
| Method | `int Add(int a, int b) => a + b;` |
| Class | `public class Person { public string Name { get; set; } = ""; }` |
| Record | `public record Point(int X, int Y);` |
| Inheritance / implementing an interface | `class Dog : Animal, IComparable<Dog>` |
| Interface | `public interface IShape { double Area(); }` |
| Read-only property | `public int Id { get; init; }` |
| enum | `enum Color { Red, Green, Blue }` |
| Exception | `throw new ArgumentException("msg", nameof(arg));` |
| try/catch/finally | `try { } catch (IOException ex) when (ex.HResult == 1) { } finally { }` |
| using (releasing a resource) | `using var file = File.OpenRead(path);` |
| namespace (file-scoped) | `namespace MyApp.Models;` |
| Lambda | `x => x * 2` / `(a, b) => a + b` |
| Tuple | `(int min, int max) = GetRange();` |
| Generics | `T Max<T>(T a, T b) where T : IComparable<T>` |

## 2. Common types

| Type | Description | Default value |
|-------|-------|----------------|
| `int`, `long` | 32/64-bit integers | `0` |
| `double`, `decimal` | Floating point / money (exact) | `0` |
| `bool` | `true` / `false` | `false` |
| `char` | Single character `'a'` | `'\0'` |
| `string` | String (immutable) | `null` |
| `DateTime`, `DateOnly`, `TimeSpan` | Date/time, date only, duration | `default` |
| `Guid` | Unique identifier `Guid.NewGuid()` | `Guid.Empty` |
| `object` | The base of everything | `null` |
| `int[]`, `List<int>` | Fixed-size array / dynamic list | `null` |

Conversions: `int.Parse("42")`, `int.TryParse(s, out var n)`, `(int)3.7` (truncation), `Convert.ToInt32(x)`, `x.ToString()`.

## 3. String formatting

```csharp
var name = "Dana"; var price = 1234.5; var when = DateTime.Now;
Console.WriteLine($"Hello {name}, the price is {price:C}");  // interpolation + currency
Console.WriteLine($"{price:N2} | {price:F0} | {0.256:P1}");  // 1,234.50 | 1235 | 25.6%
Console.WriteLine($"{when:yyyy-MM-dd HH:mm} | {when:d}");    // custom date | short
Console.WriteLine($"{name,10}|{price,-10}|");                // padding (right / left)
Console.WriteLine($"{42:D5} | {255:X} | {1_000_000:N0}");   // 00042 | FF | 1,000,000
var multi = """
    Multi-line text (raw string literal)
    with no need to escape "quotes"
    """;
```

Useful: `string.Join(", ", list)`, `s.Split(',')`, `s.Trim()`, `s.ToUpper()`, `s.Contains("x")`, `s.StartsWith("a")`, `string.IsNullOrWhiteSpace(s)`, `s.Replace("a", "b")`, `s[..3]` / `s[^2..]` (ranges), `StringBuilder` for many concatenations.

## 4. Collections

| Type | When | Main operations |
|-------|-----|----------------|
| `T[]` | Fixed size, performance | `arr[i]`, `arr.Length` |
| `List<T>` | Growable list — the default choice | `Add`, `Remove`, `Insert`, `Count`, `list[i]`, `Contains`, `Sort` |
| `Dictionary<K,V>` | O(1) lookup by key | `dict[key] = v`, `TryGetValue`, `ContainsKey`, `Keys`, `Values` |
| `HashSet<T>` | Uniqueness, fast membership check | `Add` (returns bool), `Contains`, `UnionWith` |
| `Queue<T>` | FIFO | `Enqueue`, `Dequeue`, `Peek` |
| `Stack<T>` | LIFO | `Push`, `Pop`, `Peek` |
| `LinkedList<T>` | Insert/remove in the middle | `AddFirst`, `AddLast`, `Remove` |
| `SortedDictionary<K,V>` / `SortedSet<T>` | Always sorted | Like Dictionary/HashSet |
| `IEnumerable<T>` | "Something you can iterate over" — for parameters | `foreach`, LINQ |
| `IReadOnlyList<T>` | Expose without allowing changes | `Count`, indexer |
| `ConcurrentDictionary<K,V>`, `ConcurrentQueue<T>` | Multi-threading (Day 2) | `TryAdd`, `AddOrUpdate`, `TryDequeue` |

Short initialization: `List<int> nums = [1, 2, 3];` (collection expression), `var dict = new Dictionary<string, int> { ["a"] = 1 };`.

## 5. LINQ — the 15 most important methods

```csharp
var people = new List<Person> { /* ... */ };
people.Where(p => p.Age >= 18)                       // filtering
      .Select(p => p.Name)                           // transformation/projection
      .OrderBy(p => p.Age).ThenByDescending(p => p.Name)
      .ToList();                                     // materialize (also ToArray / ToDictionary)
people.First(p => p.Id == 7);                        // the first — exception if none
people.FirstOrDefault(p => p.Id == 7);               // the first — null if none
people.Single(p => p.Id == 7);                       // exactly one
people.Any(p => p.Age > 90);  people.All(p => p.Age > 0);
people.Count(p => p.City == "TLV");
people.Sum(p => p.Salary);  people.Average(p => p.Age);  people.Max(p => p.Age);
people.GroupBy(p => p.City).Select(g => new { g.Key, Count = g.Count() });
people.Skip(10).Take(5);                             // paging
people.Select(p => p.City).Distinct();
people.SelectMany(p => p.Phones);                    // flatten nested lists
people.Zip(scores, (p, s) => (p.Name, s));           // combine "side by side"
people.Chunk(100);                                   // split into batches
```

Remember: LINQ is **lazy** — the query only runs when you iterate over the result (`foreach`, `ToList`, `Count`...). Don't iterate over the same query twice without `ToList()`.

## 6. async / await — patterns

```csharp
// Signature: Task (no value) / Task<T> (with a value); the method name ends with Async
async Task<string> DownloadAsync(HttpClient http, string url, CancellationToken ct = default)
{
    var response = await http.GetAsync(url, ct);
    response.EnsureSuccessStatusCode();
    return await response.Content.ReadAsStringAsync(ct);
}

// Concurrency: start everything, wait for all of them
var tasks = urls.Select(u => DownloadAsync(http, u));
string[] pages = await Task.WhenAll(tasks);

// Cancellation with a timeout
using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
try { await DownloadAsync(http, url, cts.Token); }
catch (OperationCanceledException) { Console.WriteLine("timeout"); }

// JSON (System.Text.Json)
var user = await http.GetFromJsonAsync<User>(url);          // System.Net.Http.Json
var json = JsonSerializer.Serialize(user, new JsonSerializerOptions { WriteIndented = true });
var back = JsonSerializer.Deserialize<User>(json);

// Heavy (CPU) work — don't block the UI
var result = await Task.Run(() => HeavyCompute(data));
```

Rules of thumb: use `await`, not `.Result` / `.Wait()` (deadlock!); `async void` only in event handlers; pass the `CancellationToken` along; in WPF, returning to the UI thread happens automatically after `await`.

## 7. .NET CLI

| Command | What it does |
|-------|---------|
| `dotnet new list` | List templates |
| `dotnet new console -n App` | Console project |
| `dotnet new classlib -n Lib` / `dotnet new xunit -n App.Tests` | Library / tests |
| `dotnet new wpf -n App` | WPF project (Windows) |
| `dotnet run` / `dotnet run -- a b` | Run (with arguments) |
| `dotnet build` / `dotnet build -c Release` | Build |
| `dotnet test` | Run tests |
| `dotnet watch run` | Automatically re-run on every change |
| `dotnet add package X` / `dotnet add reference ../Lib/Lib.csproj` | Dependencies |
| `dotnet format` | Format code |
| `dotnet --list-sdks` / `dotnet --info` | Installation info |

## 8. Keyboard shortcuts — Visual Studio and VS Code

| Action | Visual Studio | VS Code |
|-------|---------------|---------|
| Run with debugger | `F5` | `F5` |
| Run without debugger | `Ctrl+F5` | `Ctrl+F5` |
| Breakpoint (add/remove) | `F9` | `F9` |
| Step Over (next line) | `F10` | `F10` |
| Step Into (enter the method) | `F11` | `F11` |
| Step Out (leave the method) | `Shift+F11` | `Shift+F11` |
| Continue | `F5` | `F5` |
| Quick Actions / quick fix (missing using, refactoring) | `Ctrl+.` | `Ctrl+.` |
| Format document | `Ctrl+K, Ctrl+D` | `Shift+Alt+F` |
| Find a file / symbol across the project | `Ctrl+T` | `Ctrl+P` (file), `Ctrl+T` (symbol) |
| Build | `Ctrl+Shift+B` | `Ctrl+Shift+B` (task) |
| Go to definition | `F12` | `F12` |
| Find all references | `Shift+F12` | `Shift+F12` |
| Rename | `Ctrl+R, Ctrl+R` / `F2` | `F2` |
| Comment / uncomment | `Ctrl+K, Ctrl+C` / `Ctrl+K, Ctrl+U` | `Ctrl+/` |
| Search in all files | `Ctrl+Shift+F` | `Ctrl+Shift+F` |
| Command palette | `Ctrl+Q` (search) | `Ctrl+Shift+P` |
| Terminal | Ctrl + ` (backtick; Developer PowerShell) | Ctrl + ` (backtick) |
| Solution Explorer / Explorer | `Ctrl+Alt+L` | `Ctrl+Shift+E` |
| Copilot Chat | `Ctrl+\, C` | `Ctrl+Alt+I` |
| Show IntelliSense | `Ctrl+Space` | `Ctrl+Space` |

On macOS, replace `Ctrl` with `Cmd` in most VS Code shortcuts.

Code snippets in Visual Studio — type them and then press `Tab` twice: `cw` (Console.WriteLine), `prop` (property), `ctor` (constructor), `for`, `foreach`, `try`, `if`, `class`.
