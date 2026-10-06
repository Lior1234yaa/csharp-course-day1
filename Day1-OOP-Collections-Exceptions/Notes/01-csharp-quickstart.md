# Module 01 — C# Warm-up: Everything You Need to Get Started

Before diving into object-oriented programming, let's take a quick tour of "the language itself": what a C# program looks like, which types exist, how to write conditions, loops and methods, and what the `?` that appears after types means. If you have already programmed in another language (Java, JavaScript, Python), most of this will look familiar — pay special attention to the difference between value and reference types and to nullable reference types, because that is where most beginner bugs hide.

## First program: `Program.cs` and top-level statements

```bash
dotnet new console -n HelloApp
cd HelloApp
dotnet run
```

`dotnet new` creates two files: `HelloApp.csproj` (the project settings — which .NET version, which packages) and `Program.cs`. Since C# 9 you don't need a `Program` class and a `Main` method — you write the code directly:

```csharp
Console.WriteLine("Hello, .NET!");
```

This is called **top-level statements**. The compiler wraps the code in `Main` for us. The only rule: type declarations (classes, records) must come **after** all the statements. In larger projects you will still see `static void Main(string[] args)` — it's the same thing, just explicit.

Our `.csproj` looks like this:

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net10.0</TargetFramework>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
  </PropertyGroup>
</Project>
```

`ImplicitUsings` automatically imports `System`, `System.Collections.Generic`, `System.Linq` and more — which is why you won't see `using System;` at the top of the files. We'll explain `Nullable` later on.

## Variables and basic types

C# is a **statically typed** language: every variable has a type that is fixed at compile time.

```csharp
int age = 30;                 // 32-bit integer
long big = 10_000_000_000;    // 64-bit integer (underscores for readability)
float ratio = 0.75f;          // 32-bit floating point — about 7 precise digits (suffix f)
double price = 19.90;         // 64-bit floating point — about 15 digits (the default)
decimal money = 19.90m;       // exact decimal — for money! (suffix m)
bool isActive = true;
char initial = 'D';
string name = "Dana";
```

**Which fractional type should you choose?**

| | `float` | `double` | `decimal` |
|---|---|---|---|
| Size | 4 bytes | 8 bytes | 16 bytes |
| Precise digits | ~7 | ~15–16 | 28–29 |
| Suffix | `f` (required) | — (the default) | `m` (required) |
| When | Graphics, games (Unity), huge arrays | Measurements, scientific calculations | **Money** |

A number with a decimal point is a `double` by default, so `float f = 19.90;` and `decimal m = 19.90;` don't compile (`CS0664`) — you need `19.90f` or `19.90m`. `double` and `float` are stored in base 2, so `0.1 + 0.2 == 0.3` is `false`; with `decimal` (base 10) it is `true`. In everyday code: `double` for measurements, `decimal` for money, and `float` almost never.

**`var`** asks the compiler to infer the type from the value. The type is still fixed — `var x = 5;` followed by `x = "hi";` won't compile. Use `var` when the type is obvious from the right-hand side (`var list = new List<int>();`) and write the type explicitly when it isn't (`int count = GetCount();`).

### Value types vs. reference types

This is the single most important difference to understand on day one:

| | Value types | Reference types |
|---|---|---|
| Examples | `int`, `double`, `bool`, `char`, `decimal`, `DateTime`, `struct`, `enum` | `string`, arrays, `class`, `record`, `List<T>`, `object` |
| The variable holds | The value itself | The "address" of an object on the heap |
| Assignment `b = a` | Copies the value | Copies the reference — two names for the same object |
| Default value | `0`, `false`, etc. | `null` |

```csharp
int a = 5;
int b = a;      // copy
b++;            // a is still 5

int[] arr1 = [1, 2, 3];
int[] arr2 = arr1;   // the same array!
arr2[0] = 99;        // arr1[0] is now 99
```

`string` is a reference type, but it is **immutable**: every operation such as `ToUpper()` returns a new string. That's why `s.ToUpper();` without an assignment does nothing.

## Strings and interpolation

```csharp
string first = "Dana", last = "Cohen";
string full = $"{first} {last}";                  // string interpolation
string padded = $"{first,-10}|{42,5}";             // alignment: width 10 left-aligned, 5 right-aligned
string money = $"{1234.5:N2}";                     // "1,234.50"
string pct = $"{0.256:P1}";                        // "25.6%"
string multi = """
    First line
    Second line
    """;                                           // raw string literal (C# 11)
bool same = string.Equals(first, "dana", StringComparison.OrdinalIgnoreCase);
```

Useful operations: `Length`, `Contains`, `StartsWith`, `Split`, `Trim`, `Replace`, `Substring`, `string.Join(", ", list)`, `string.IsNullOrWhiteSpace(s)`.

## Control flow

```csharp
if (age >= 18 && isActive) Console.WriteLine("adult");
else if (age >= 13) Console.WriteLine("teen");
else Console.WriteLine("child");

// switch expression — returns a value, must cover every case (_ = default)
string grade = score switch
{
    >= 90 => "A",
    >= 80 => "B",
    _ => "F",
};

for (int i = 0; i < 3; i++) { }
foreach (var item in items) { }        // the preferred way to iterate over a collection
while (condition) { }
do { } while (condition);
```

`break` exits the loop, `continue` skips to the next iteration. Ternary operator: `var label = x > 0 ? "pos" : "neg";`.

## Methods

```csharp
static int Add(int x, int y) => x + y;                         // expression-bodied
static string Greet(string who, string greeting = "Hello")      // optional parameter
    => $"{greeting}, {who}!";

static bool TryDivide(double x, double y, out double result)   // out — an extra return value
{
    if (y == 0) { result = 0; return false; }
    result = x / y;
    return true;
}

static (int Min, int Max) MinMax(int[] values) => (values.Min(), values.Max());  // tuple

Greet("Yossi");                        // Hello, Yossi!
Greet("Yossi", greeting: "Hi");        // named argument
var (min, max) = MinMax([4, 9, 1]);    // deconstruction
```

With top-level statements you can define methods directly in the file (local functions). `static` means the method doesn't access variables from the surrounding code — a good habit for clarity.

## Nullable reference types

In modern .NET, `string name` means "a string that is never `null`", and `string? name` means "might be `null`". The compiler tracks this and warns you if you access something that might be `null` without checking:

```csharp
string? input = Console.ReadLine();        // ReadLine returns null at end of input
int len = input.Length;                    // warning CS8602: dereference of a possibly null reference

int len2 = input?.Length ?? 0;             // ?. returns null if input is null; ?? supplies a default
if (input is not null) Console.WriteLine(input.Length);   // after the check — no warning
string safe = input ?? "";                 // ?? — "if null, then"
string forced = input!;                    // ! — "I promise this isn't null" (avoid!)
```

This isn't just a cosmetic warning: `NullReferenceException` is the most common exception in .NET, and this mechanism prevents most of them before the program even runs. The rule: don't silence nullable warnings — fix them.

## Namespaces and using

```csharp
namespace MyShop.Inventory;     // file-scoped namespace — the whole file belongs to it

public class Product { }
```

By convention, the folder structure mirrors the namespace. To use a type from another namespace: `using MyShop.Inventory;` at the top of the file, or the fully qualified name `MyShop.Inventory.Product`.

## Visual Studio / VS Code — the basics

- **Visual Studio**: File → New → Project → Console App. `F5` runs with the debugger, `Ctrl+F5` without. `Ctrl+.` for fix suggestions (Quick Actions), `Ctrl+K, Ctrl+D` to format code, `F12` to go to definition.
- **VS Code**: install the **C# Dev Kit** extension. Run `dotnet new console`, open the folder, press `F5` (a `launch.json` is created automatically). Use the integrated terminal (`` Ctrl+` ``) for `dotnet run`.
- Both tools use the same SDK and the same `dotnet` CLI — the project is identical.

CLI commands worth remembering: `dotnet new console -n Name`, `dotnet run`, `dotnet build`, `dotnet add package X`, `dotnet --version`.

## `enum` and `struct` in brief

Two value types you will define yourself and meet already today. An **`enum`** is a closed set of named values — instead of "magic numbers" like `status == 2` you write `status == OrderStatus.Shipped`. The compiler prevents invalid values, the IDE autocompletes, and the code explains itself. A **`struct`** is like a small class that is copied by value (like `int`), and suits small, immutable data such as a point or a color. Most of the time you'll write a `class` or a `record`; `struct` is reserved for cases where performance matters.

```csharp
enum OrderStatus { Pending, Paid, Shipped, Cancelled }

OrderStatus status = OrderStatus.Paid;
if (status is OrderStatus.Paid or OrderStatus.Shipped) Console.WriteLine("money received");
Console.WriteLine((int)status);                 // 1 — the numeric value behind the scenes
Console.WriteLine(Enum.Parse<OrderStatus>("Shipped"));

readonly record struct Point(int X, int Y);     // a small, immutable value type
```

## How to read a compiler error

The C# compiler is your best friend: most mistakes are caught before the program even runs. An error message looks like this: `Program.cs(12,9): error CS0029: Cannot implicitly convert type 'string' to 'int'`. Read it from the end back to the start: what the problem is (an invalid conversion), the code (`CS0029` — you can search for exactly that in a search engine or in the documentation), and where (line 12, column 9). In Visual Studio, double-clicking the error in the Error List jumps to the line, and `Ctrl+.` often offers an automatic fix. Warnings don't stop the build, but it's worth treating them as errors — especially nullable warnings.

## Common mistakes

- **`==` between `double`s** — `0.1 + 0.2 == 0.3` is `false`. Use `decimal` for money; to compare `double`s, compare the difference against a small threshold.
- **Modifying a string "in place"** — `name.ToUpper();` doesn't change `name`. You need `name = name.ToUpper();`.
- **`int` overflow** — `int.MaxValue + 1` wraps around to a negative number without an error (unless you use `checked`). For large numbers: `long`.
- **`Console.ReadLine()` returns `string?`** — always handle `null` (end of input) and non-numeric input (`int.TryParse`, module 06).
- **Silencing nullable warnings with `!`** — almost always hides a real bug.
- **`var` for everything** — `var result = Process();` tells the reader nothing. Write the type when it isn't obvious from the line.
- **Forgetting the `m` on a `decimal`** — `decimal x = 19.90;` doesn't compile; you need `19.90m`.

## Summary

- `dotnet new console` + `dotnet run` — that's all you need to get started. Top-level statements save you the `Main` ceremony.
- Value vs. reference: an `int` is copied, an array/class instance is shared. This will explain a lot of behavior later today.
- Interpolation `$"..."` with format specifiers (`:N2`, `,-10`) is the way to build strings.
- `switch` expressions, `foreach`, expression-bodied methods, `out` and tuples — the modern syntax is short and readable.
- `string?` vs. `string`: the compiler helps you avoid `NullReferenceException`. Let it.

## Further reading

- [A tour of the C# language](https://learn.microsoft.com/dotnet/csharp/tour-of-csharp/)
- [Top-level statements](https://learn.microsoft.com/dotnet/csharp/fundamentals/program-structure/top-level-statements)
- [Value types and reference types](https://learn.microsoft.com/dotnet/csharp/language-reference/builtin-types/value-types)
- [Nullable reference types](https://learn.microsoft.com/dotnet/csharp/nullable-references)
- [String interpolation](https://learn.microsoft.com/dotnet/csharp/language-reference/tokens/interpolated)
- [dotnet CLI overview](https://learn.microsoft.com/dotnet/core/tools/)
