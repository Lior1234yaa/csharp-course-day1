# Module 07 — Clean, Maintainable and Reusable Code

Code is read far more often than it is written. Every line you write today, someone — probably you, six months from now — will have to understand, change and fix. "Clean code" is not about beauty but about **cost**: how long it takes to understand what the code does, and how safe it is to change it. In this module we gather simple, practical principles, see them in before/after examples, and connect them to what we learned today: interfaces, generics and collections are the tools that make code reusable.

## Naming

A name is the first and most important piece of documentation. A good name saves a comment.

- **Describe intent**, not implementation: `elapsedDays`, not `d`; `IsEligibleForDiscount`, not `Check`.
- **C# conventions**: `PascalCase` for classes, methods and properties; `camelCase` for parameters and local variables; `_camelCase` for private fields; an `I` prefix for interfaces; methods returning `bool` phrased as a question (`IsEmpty`, `HasItems`, `CanWithdraw`); async methods with an `Async` suffix.
- **No abbreviations** that not everyone knows (`cust`, `mgr`) and no Hungarian notation (`strName`, `iCount`).
- **Length according to scope**: `i` in a 3-line loop is fine; `i` as a class field is not.

```csharp
// Before
static double c(double a, int t, bool m) { ... }

// After
static decimal CalculatePriceWithVat(decimal basePrice, CustomerType customer, bool isMember) { ... }
```

## Small methods that do one thing

A method should be readable and understandable without scrolling. Rule of thumb: up to ~20 lines, one or two levels of indentation, and a name that describes exactly what it does. If you need "And" in the name (`ValidateAndSave`) — those are two methods. Extract Method (`Ctrl+R, Ctrl+M` in Visual Studio) is the most common refactoring.

```csharp
// Before: everything in one place
public void ProcessOrder(Order order)
{
    if (order.Lines.Count == 0) throw new ArgumentException("empty");
    decimal total = 0;
    foreach (var l in order.Lines) total += l.Price * l.Quantity;
    if (order.Customer.IsVip) total *= 0.9m;
    _db.Save(order, total);
    _mailer.Send(order.Customer.Email, $"Order {order.Id}: {total:N2}");
}

// After: three questions, three answers
public void ProcessOrder(Order order)
{
    EnsureNotEmpty(order);
    decimal total = CalculateTotal(order);
    _db.Save(order, total);
    NotifyCustomer(order, total);
}
```

## Magic numbers → constants and enums

`if (status == 2)` or `price * 1.18` mean nothing to the reader, and when the VAT rate changes you'll have to search for `1.18` across the whole project (and miss the `1.18m` in another file).

```csharp
const decimal VatRate = 1.18m;
const int RegularHoursPerMonth = 160;
static readonly TimeSpan LoanPeriod = TimeSpan.FromDays(14);     // readonly for types that can't be const

enum OrderStatus { Pending, Paid, Shipped, Cancelled }
if (order.Status == OrderStatus.Shipped) { }
```

`const` for values known at compile time (numbers, strings); `static readonly` for objects. `enum` for a closed set of options — the compiler checks it, the IDE autocompletes it, and a `switch` can warn about a missing case.

## DRY — Don't Repeat Yourself

When the same piece of code appears twice, a bug gets fixed in only one place. Extract a method, a base class, or — usually best — a method that takes a delegate (Module 05). But beware of the other extreme: two pieces of code that look similar but change for different reasons should not be merged. DRY is about **knowledge**, not text.

## SOLID in brief

Five principles of object-oriented design. You don't need to memorize them — you need to recognize when they're being violated:

- **S — Single Responsibility**: a class has one reason to change. `Order` shouldn't know how to send email; `OrderService` shouldn't know how to format HTML. Warning sign: a class with "Manager"/"Helper" in its name and 40 methods.
- **O — Open/Closed**: open for extension, closed for modification. Adding a new employee type = a new class, not another `case` in a `switch` (Module 03). Polymorphism and interfaces.
- **L — Liskov Substitution**: a derived class must behave like its base class from the point of view of whoever uses it. If `Square : Rectangle` breaks `SetWidth` — the inheritance is wrong.
- **I — Interface Segregation**: small, focused interfaces. `IReadOnlyRepository` and `IWritableRepository` instead of a single 12-method interface where most implementers throw `NotSupportedException` from half of them.
- **D — Dependency Inversion**: depend on abstractions, not implementations. `OrderService(ILogger logger)`, not `new FileLogger()` inside the class. This is what lets you swap implementations and test with a fake. On Day 4 we'll see Dependency Injection, which does this automatically.

## Comments

A good comment explains **why**, not **what**. `i++; // increments i` is noise; `// the vendor returns 429 above 10 requests per second, hence the delay` is gold. If you need a comment to explain what the code does — improve the code (a name, an extracted method). `///` comments (XML doc) on a public API show up in IntelliSense and are worth it. Delete commented-out code — that's what git is for.

## Code style: `.editorconfig` and `dotnet format`

A team needs one style, and a tool that enforces it — not arguments about whitespace in code review. An `.editorconfig` file at the project root defines the rules (indentation, brace placement, `var`, `using` ordering, and even static analysis rules), and Visual Studio, VS Code and Rider honor it automatically.

```bash
dotnet new editorconfig       # creates a file with the .NET defaults
dotnet format                 # formats the whole project according to the rules
dotnet format --verify-no-changes   # in CI: fails if anything isn't formatted
```

An example of a few lines from an `.editorconfig`:

```text
[*.cs]
indent_size = 4
csharp_style_var_when_type_is_apparent = true:suggestion
csharp_prefer_braces = true:warning
dotnet_diagnostic.CA2200.severity = error     # throw ex; becomes an error
```

Also add `<TreatWarningsAsErrors>true</TreatWarningsAsErrors>` to the `.csproj` in new projects — warnings only pile up if you let them.

## Refactoring — before and after

Refactoring is changing the **structure** of code without changing its **behavior**. You do it in small steps, with tests (or at least a run) after every step.

```csharp
// Before
static double c(double a, int t, bool m)
{
    double r = 0;
    if (t == 1) { r = a * 0.9; if (m) r = r - 5; }
    else if (t == 2) { r = a * 0.8; if (m) r = r - 5; }
    else { r = a; if (m) r = r - 5; }
    if (r < 0) r = 0;
    return r * 1.18;
}

// After
enum CustomerType { Regular, Silver, Gold }
const decimal VatRate = 1.18m, MemberDiscount = 5m, SilverFactor = 0.9m, GoldFactor = 0.8m;

static decimal CalculatePriceWithVat(decimal basePrice, CustomerType customer, bool isMember)
{
    decimal price = ApplyCustomerDiscount(basePrice, customer);
    if (isMember) price -= MemberDiscount;
    price = Math.Max(price, 0);
    return price * VatRate;
}

static decimal ApplyCustomerDiscount(decimal price, CustomerType customer) => customer switch
{
    CustomerType.Silver => price * SilverFactor,
    CustomerType.Gold => price * GoldFactor,
    _ => price,
};
```

What changed: names, an `enum` instead of 1/2, constants, the duplicated `if (m) r -= 5` was pulled out, a `switch` expression instead of an `if` chain, `decimal` for money. The behavior is identical — Exercise 14 checks this.

More everyday refactorings: Extract Method, Rename (`F2` / `Ctrl+R, Ctrl+R`), Inline Variable, Replace Conditional with Polymorphism, Introduce Parameter Object (one record instead of 6 parameters), Guard Clauses instead of nested `if`s.

## Reusable code: interfaces and generics

The tools we learned today are exactly what makes code reusable:

- An **interface** separates "what" from "how": a `Payroll` that works with `IPayable` will serve employees, contractors and, in the future, robots.
- **Generics** separate the algorithm from the type: one `Repository<T>` for all entities; one `Find(Func<T, bool>)` for all conditions.
- **Delegates** separate the framework from the details: `Retry(Action op, int times)` wraps any operation.
- **Records + pure functions** are easy to compose and test.

```csharp
static T Retry<T>(Func<T> operation, int attempts = 3)       // reusable: any operation, any type
{
    for (int i = 1; ; i++)
    {
        try { return operation(); }
        catch (Exception) when (i < attempts) { Thread.Sleep(100 * i); }
    }
}

var data = Retry(() => client.GetString(url));
```

## Common mistakes

- **Premature optimization** — "clever", unreadable code for performance nobody measured.
- **Premature abstraction** — an interface for every class, generics for something with a single use. The rule: abstract after the second case, not before the first.
- **"God" classes** that know everything (`AppManager`).
- **Boolean parameters** — `Save(order, true, false)`: what does that mean? Use an enum or two well-named methods.
- **Comments instead of names** — `int d; // days since last login` → `int daysSinceLastLogin`.
- **Dead code** and `// TODO`s from two years ago — delete them. git remembers.
- **Breaking the project's style** because "that's how I'm used to it" — consistency matters more than personal preference.

## Summary

- Names that describe intent; small methods; constants and enums instead of magic numbers; DRY is about knowledge.
- SOLID: one responsibility, extension without modification, derived classes that behave like their base, small interfaces, dependency on abstractions.
- Comments for "why"; `.editorconfig` + `dotnet format` for a uniform style.
- Refactor in small steps and test after every step.
- Interfaces, generics and delegates are the tools for code you can reuse.

## Further reading

- [C# identifier naming rules and conventions](https://learn.microsoft.com/dotnet/csharp/fundamentals/coding-style/identifier-names)
- [C# coding conventions](https://learn.microsoft.com/dotnet/csharp/fundamentals/coding-style/coding-conventions)
- [Code-style rules (.editorconfig)](https://learn.microsoft.com/dotnet/fundamentals/code-analysis/style-rules/)
- [dotnet format](https://learn.microsoft.com/dotnet/core/tools/dotnet-format)
- [Code analysis in .NET](https://learn.microsoft.com/dotnet/fundamentals/code-analysis/overview)
- [Refactor code in Visual Studio](https://learn.microsoft.com/visualstudio/ide/refactoring-in-visual-studio)
- [Architectural principles (SOLID, DRY)](https://learn.microsoft.com/dotnet/architecture/modern-web-apps-azure/architectural-principles)
