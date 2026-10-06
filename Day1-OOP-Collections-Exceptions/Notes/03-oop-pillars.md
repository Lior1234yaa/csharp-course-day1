# Module 03 — OOP Principles: Encapsulation, Inheritance, Polymorphism, Interfaces and Abstraction

In the previous module we learned how to build a class. Now we'll learn how to build a **system** of classes that cooperate without knowing too much about each other. The four classic OOP principles are not a list to memorize but four answers to a single question: how do you write code that can be changed without breaking? We'll also learn the tools C# itself provides: access modifiers, `virtual`/`override`, `abstract`, `interface`, pattern matching, and overriding `ToString`/`Equals`.

## Encapsulation — information hiding

Encapsulation says: **an object's state changes only through its own methods**. Code that uses the class sees a small, clear "face" (API) and cannot put the object into an invalid state. If the balance is `private set`, there's no way to end up with an account with a negative balance without going through `Withdraw`, which checks. The real benefit: when you want to change the implementation (for example, store a history instead of a number), you change it in one place.

Access modifiers in C#:

| modifier | Accessible from... |
|---|---|
| `public` | Everywhere |
| `private` | Only the class itself (the default for members) |
| `protected` | The class and its derived classes |
| `internal` | All code in the same assembly (project) — the default for classes |
| `protected internal` | Derived classes or the same assembly |
| `private protected` | Derived classes within the same assembly |

Rule: start with `private` and widen access only when needed. Less public surface = fewer things that can break.

## Inheritance — "is a kind of"

Inheritance lets a class (the derived class) receive all the members of another class (the base class) and add to or change them. Use it when there is a genuine **is-a** relationship: a manager is an employee, a square is a rectangle. C# supports single inheritance only — a class inherits from one class (but can implement several interfaces).

```csharp
class Employee(int id, string name)
{
    public int Id { get; } = id;
    public string Name { get; } = name;

    public virtual decimal MonthlyPay() => 0;                  // virtual — may be overridden
    public virtual string Describe() => $"#{Id} {Name}";
}

class Manager(int id, string name, decimal salary, decimal bonus) : Employee(id, name)
{
    public override decimal MonthlyPay() => salary / 12 + bonus;
    public override string Describe() => base.Describe() + " (manager)";   // base — the parent's implementation
}

sealed class Intern(int id, string name) : Employee(id, name)   // sealed — cannot be inherited from
{
    public override decimal MonthlyPay() => 3_000;
}
```

- `virtual` in the base class says "derived classes may replace the implementation". Without `virtual`, the method is fixed.
- `override` in the derived class replaces it. If you write a method with the same name without `override`, you'll get a warning about **hiding** (`new`) — almost always a mistake.
- `base.X()` calls the parent's implementation — useful when you want to "extend" rather than "replace".
- `sealed` on a class prevents inheritance; on an `override` it prevents further overriding.
- The derived class's constructor must call the parent's constructor (`: base(...)` or via a primary constructor as shown above).

## Abstract classes

Sometimes the base class has no meaning on its own: what is "just a shape"? An `abstract class` cannot be instantiated (`new Shape()` doesn't compile), and it can contain `abstract` methods — a declaration without a body that every derived class **must** implement.

```csharp
abstract class Shape
{
    public abstract double Area();                        // must be implemented
    public virtual string Describe() => $"{GetType().Name} with area {Area():F2}";   // shared
}

class Circle(double r) : Shape
{
    public override double Area() => Math.PI * r * r;
}
```

`Describe` uses `Area`, which doesn't exist yet — that's fine: at runtime the derived class's version will be called. This is a powerful pattern: the base defines the skeleton, the derived classes fill in the gaps.

## Polymorphism — one call, many behaviors

Polymorphism ("many forms") is the result of `virtual`/`override`: a variable of the base type can hold any derived type, and the method call is dispatched to the implementation of the **actual** type at runtime.

```csharp
List<Shape> shapes = [new Circle(1), new Rectangle(2, 3), new Square(2)];
foreach (var s in shapes)
    Console.WriteLine(s.Describe());          // each one according to its own type
double total = shapes.Sum(s => s.Area());
```

The printing code doesn't know, and doesn't need to know, which shapes exist. When you add a `Triangle` tomorrow, this loop won't change. This is the **Open/Closed Principle**: open for extension, closed for modification. The opposite is a `switch` on "kind" everywhere — every new shape requires updating all the switches.

## Interfaces — a contract without an implementation

An interface (`interface`) defines **what** an object can do, without saying **how**. A class can implement several interfaces, and an interface doesn't impose a hierarchy: `Contractor` and `Employee` can both be `IPayable` without a common parent.

```csharp
interface IPayable
{
    string PayeeName { get; }
    decimal MonthlyPay();
    string PaySlip() => $"{PayeeName}: {MonthlyPay():N2}";   // default member (C# 8+)
}

class Contractor(string company, decimal fee) : IPayable
{
    public string PayeeName => company;
    public decimal MonthlyPay() => fee;
}

List<IPayable> payroll = [new Manager(1, "Noa", 360_000, 2_000), new Contractor("Acme", 12_000)];
foreach (var p in payroll) Console.WriteLine(p.PaySlip());
```

- Interface names start with `I`. Interface members are public automatically.
- **Default interface members** let you provide a default implementation — useful for adding a method to an existing interface without breaking its implementers. They are accessible only through the interface type. Don't turn this into a substitute for a base class.
- Common interfaces in .NET: `IComparable<T>` (sorting), `IEquatable<T>`, `IEnumerable<T>` (foreach), `IDisposable` (releasing resources).

### Interface or abstract class?

| | `abstract class` | `interface` |
|---|---|---|
| Inheritance | Only one | As many as you like |
| Contains | Fields, constructors, shared implementation | A contract (+ default members) |
| Relationship | Strong is-a, shared code | can-do, a capability |
| Example | `Shape`, `Employee` | `IPayable`, `IDisposable`, `IComparable` |

You'll often combine them: an interface for the contract, and an abstract class that partially implements it for the convenience of derived classes.

## Abstraction — expose only what matters

Abstraction is the idea behind interfaces and abstract classes: **code depends on the contract, not on the implementation**. `Payroll` depends on `IPayable`, not on `Manager`. The result: you can swap an implementation (console logger → file logger → fake logger for tests) without touching the code that uses it. This is the foundation of Dependency Injection, which we'll meet on Day 4.

## Composition over inheritance

Inheritance is the strongest coupling between two classes — the derived class depends on every detail of the parent. Often **composition** is better: "has a" instead of "is a". Instead of `class OrderService : ConsoleLogger` (an order service is a logger?!) you write `class OrderService(ILogger logger)` — it has a logger, and any logger will do.

```csharp
interface ILogger { void Log(string message); }
class ConsoleLogger : ILogger { public void Log(string m) => Console.WriteLine(m); }

class OrderService(ILogger logger)          // has-a
{
    public void Place(string id) { logger.Log($"placing {id}"); }
}
```

Rule of thumb: inheritance for a genuine domain model (shapes, employees) and for framework classes designed for it; composition for everything else.

## `is`, `as` and pattern matching

Sometimes you really do need to know the actual type. The modern tools:

```csharp
if (shape is Circle c) Console.WriteLine(c.Radius);     // check + cast + new variable
var rect = shape as Rectangle;                           // null if it doesn't match (doesn't throw)
var forced = (Rectangle)shape;                           // throws InvalidCastException if it doesn't match

string label = shape switch
{
    Circle { Radius: > 10 } => "big circle",             // property pattern
    Circle c => $"circle r={c.Radius}",
    Rectangle { Width: var w, Height: var h } when w == h => "square-ish",
    null => "nothing",
    _ => "other",
};
```

Order matters in a `switch`: specific cases before general ones, otherwise the compiler will complain about an unreachable case. And if you find yourself doing a `switch` on types everywhere — you may be missing a `virtual` method.

## `ToString`, `Equals`, `GetHashCode`

Every class inherits three methods from `object` that are worth knowing:

- **`ToString()`** — the default returns the type name. Override it to get useful output in `Console.WriteLine(obj)` and in the debugger.
- **`Equals(object)`** and **`GetHashCode()`** — the default compares references. If objects are "equal by content" (for example `Money(10, "ILS")`), override **both together**: `Dictionary` and `HashSet` use `GetHashCode` to find the "bucket" and then `Equals` — if they are inconsistent, the collection will behave strangely.

```csharp
class Money(decimal amount, string currency)
{
    public decimal Amount => amount;
    public string Currency => currency;

    public override bool Equals(object? obj) =>
        obj is Money o && o.Amount == Amount && o.Currency == Currency;
    public override int GetHashCode() => HashCode.Combine(Amount, Currency);
    public override string ToString() => $"{Amount} {Currency}";
}
```

...or simply `record Money(decimal Amount, string Currency);` — and the compiler does it for you. That's the main reason records exist.

## Common mistakes

- **Inheritance for code reuse** without an is-a relationship. If `Car : Engine` sounds weird — it is weird. Use composition.
- **Forgetting `virtual`** — then `override` doesn't compile; or forgetting `override` — then the new method "hides" rather than overrides, and polymorphism doesn't work.
- **Deep hierarchies** (5 levels) — hard to follow. 2–3 levels is almost always enough.
- **`Equals` without `GetHashCode`** — the compiler warns (CS0659), and rightly so.
- **`(Circle)shape` without checking** — `InvalidCastException`. Prefer the `is` pattern.
- **An interface with 15 methods** — implementers are forced to implement everything. Keep interfaces small and focused (Interface Segregation, module 07).
- **`protected` fields** — derived classes come to depend on the internal implementation. Prefer `protected` methods/properties.

## Summary

- **Encapsulation**: private state, a small public API. `private` by default.
- **Inheritance**: `virtual`/`override`/`base`/`abstract`/`sealed`. Only for genuine is-a relationships; composition for everything else.
- **Polymorphism**: a variable of the base type, behavior according to the actual type. Replaces `switch` on kinds.
- **Interfaces**: a contract without implementation, multiple interfaces, default members. **Abstraction**: depend on the contract, not the implementation.
- `is`/`as`/`switch` patterns when you need the type; `ToString`/`Equals`/`GetHashCode` — or a record.

## Further reading

- [Object-Oriented programming (C#)](https://learn.microsoft.com/dotnet/csharp/fundamentals/object-oriented/)
- [Inheritance](https://learn.microsoft.com/dotnet/csharp/fundamentals/object-oriented/inheritance)
- [Polymorphism](https://learn.microsoft.com/dotnet/csharp/fundamentals/object-oriented/polymorphism)
- [Interfaces](https://learn.microsoft.com/dotnet/csharp/fundamentals/types/interfaces)
- [Access modifiers](https://learn.microsoft.com/dotnet/csharp/programming-guide/classes-and-structs/access-modifiers)
- [Pattern matching](https://learn.microsoft.com/dotnet/csharp/fundamentals/functional/pattern-matching)
- [Equality comparisons](https://learn.microsoft.com/dotnet/csharp/programming-guide/statements-expressions-operators/equality-comparisons)
