# Module 02 — Classes, Objects, Constructors and Properties

Object-oriented programming (OOP) is a way of organizing software around **objects**: units that bundle together data (state) and the behavior (methods) that operates on that data. Instead of a function `Deposit(account, amount)` that receives a "data structure" and modifies it, we have an `account` object that knows how to deposit into itself — and only it decides whether the deposit is valid. In this module we'll learn the building blocks: classes, objects, constructors and properties. In the next module we'll see the four principles (encapsulation, inheritance, polymorphism, abstraction) that turn this into good design.

## Class and object

A **class** (`class`) is a template — "this is what a bank account looks like". An **object** (an instance) is a specific bank account created from the template with `new`. One class, any number of objects, each with its own state.

```csharp
class BankAccount
{
    private decimal _balance;                 // field — the internal state, private

    public string Owner { get; set; } = "";   // property — the public face

    public void Deposit(decimal amount)       // method — behavior
    {
        if (amount <= 0) throw new ArgumentOutOfRangeException(nameof(amount));
        _balance += amount;
    }

    public decimal GetBalance() => _balance;
}

var acc = new BankAccount { Owner = "Dana" };   // creation + object initializer
acc.Deposit(500);
Console.WriteLine($"{acc.Owner}: {acc.GetBalance()}");
```

Conventions: private fields in `_camelCase`, everything public in `PascalCase`. One class per file, with the file named after the class.

## Fields vs. properties

A **field** is a variable inside the object. A **property** looks like a field from the outside (`acc.Balance`), but behind it are `get`/`set` methods that you control. Why not just public fields? Because a property lets you:

- validate values (a `set` that checks);
- compute a value when it is read instead of storing it;
- expose read-only access;
- change the implementation in the future without breaking code that uses the class.

```csharp
class Product
{
    // auto-property: the compiler creates a hidden field
    public string Name { get; set; } = "";

    // public read, write only from inside the class
    public decimal Price { get; private set; }

    // full property with validation
    private int _stock;
    public int Stock
    {
        get => _stock;
        set => _stock = value >= 0 ? value : throw new ArgumentOutOfRangeException(nameof(value));
    }

    // computed property — no field, calculated on every read
    public decimal PriceWithVat => Price * 1.18m;

    // init-only: can be set only at creation (object initializer / constructor), read-only afterwards
    public string Sku { get; init; } = "";

    // required: must be set at creation, otherwise a compile error
    public required string Category { get; init; }
}

var p = new Product { Name = "Keyboard", Sku = "K-1", Category = "Peripherals" };
// p.Sku = "X";   // error: init-only
```

`required` + `init` is the modern combination for "an object that must be valid from the moment it is created, and then doesn't change" — without writing a constructor with 6 parameters.

## Constructors

A constructor is the method that runs on `new`. Its job: bring the object into a valid state. If you don't write a constructor, there is a default parameterless one.

```csharp
class BankAccount
{
    public string Id { get; }          // read-only — set only in the constructor
    public string Owner { get; set; }
    public decimal Balance { get; private set; }

    public BankAccount(string id, string owner)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);
        Id = id;
        Owner = owner;
    }

    // overload that chains to the first constructor with this(...)
    public BankAccount(string id, string owner, decimal initialDeposit) : this(id, owner)
    {
        Deposit(initialDeposit);
    }

    public void Deposit(decimal amount) { /* ... */ }
}

var a1 = new BankAccount("IL-1", "Dana");
var a2 = new BankAccount("IL-2", "Yossi", 1_000);
```

### Primary constructors (C# 12)

When the constructor only copies parameters into fields, you can shorten it: the parameters are written in the class header and are available throughout the class body.

```csharp
class TemperatureSensor(string location, double initial)
{
    private readonly List<double> _readings = [initial];

    public string Location => location;          // the parameter is "captured"
    public double Last => _readings[^1];
    public void Read(double value) => _readings.Add(value);
}
```

Note: primary constructor parameters are not properties — they aren't visible from the outside unless you expose them. In a `record` (covered below) they do become properties automatically.

## `this`

`this` is the reference to the current object. You use it when names collide (`this.name = name;`), for constructor chaining (`: this(...)`), or to pass the object itself somewhere else (`registry.Add(this)`). In most modern code you don't need to write it.

## Static members

`static` belongs to **the class** rather than to a specific object. Instance counters, constants, "pure" helper methods and factory methods are the classic cases:

```csharp
class BankAccount
{
    public const decimal MinDeposit = 10m;              // constant — implicitly static
    public static int Count { get; private set; }       // shared by all instances

    public BankAccount(string id, string owner) { /*...*/ Count++; }

    public static BankAccount OpenSavings(string owner) => new(NextId(), owner);   // factory
    private static string NextId() => $"IL-{Count + 1:D4}";
}

Console.WriteLine(BankAccount.Count);       // through the class, not through an object
```

A class that is entirely static (`static class MathUtils`) cannot be instantiated — it's just a "toolbox" of methods. `Math`, `Console` and `File` are like that.

## Object initializers

A convenient syntax for setting properties right after creation. It works together with `init`/`required`:

```csharp
var order = new Order
{
    Id = 42,
    Customer = "Dana",
    Lines = [new("pen", 2), new("book", 1)],   // collection expression (C# 12)
};
```

## `record` vs. `class`

A `record` is a class for which the compiler automatically writes: a constructor, properties, **value-based** `Equals`/`GetHashCode`, a readable `ToString`, `with` and deconstruction. It is meant for "data" — objects identified by their content rather than by their identity.

```csharp
record Person(string Name, int Age);          // positional record — one line

var p1 = new Person("Dana", 30);
var p2 = new Person("Dana", 30);
Console.WriteLine(p1 == p2);                   // True — value equality
Console.WriteLine(p1);                         // Person { Name = Dana, Age = 30 }
var older = p1 with { Age = 31 };              // copy with a change; p1 is unchanged
var (name, age) = older;                       // deconstruction
```

| | `class` | `record` |
|---|---|---|
| Equality | By reference (the same object) | By value (the same data) |
| Intended for | Entities with identity and changing state (`BankAccount`, `Order`) | Data/values (`Point`, `Money`, DTO, query result) |
| Modification | Properties with `set` | Usually immutable, `with` to create a copy |
| `ToString` | The type name (unless overridden) | All the properties |

Rule of thumb: if two objects with the same data are "the same thing" — record. If identity matters (two bank accounts with the same balance are still different accounts) — class. There is also `record struct` for value types.

## Object lifecycle

What actually happens when you write `var acc = new BankAccount("IL-1", "Dana");`? First, memory is allocated on the heap, large enough for all the class's fields. The fields get default values (`0`, `false`, `null`), then the initializers you wrote on the declaration line run (`= [];`, `= "";`), and only then the constructor body. Finally, the reference is returned and stored in `acc`. The object lives as long as someone holds a reference to it; when there are no more references, the **Garbage Collector** frees the memory whenever it sees fit — there is no `delete` in C#. That's why we don't worry about freeing memory, but we do take care of releasing external **resources** (files, connections) via `IDisposable` — more on that in module 06.

It's important to understand the implication of "reference": if we pass `acc` to a method and the method calls `acc.Deposit(100)`, the caller will see the change too — there is only one object. By contrast, a `record struct` or an `int` passed to a method is copied, and the method works on the copy.

## More on `required` vs. a constructor

When should you choose a constructor and when `required`? A constructor fits when there is **logic** at creation — validation, computation, registration — or when there are few mandatory parameters. `required` + `init` fits objects that are mainly "data containers" with many fields, where an object initializer is more readable than a long parameter list whose order is easy to mix up. You can also combine them: a constructor for the essential parameters and `init` properties for everything else.

## Common mistakes

- **Public fields** (`public int Age;`) — you lose control. Always use a property.
- **A constructor that doesn't validate**, followed by methods that check `if (Name == null)` everywhere. Validate once, at the entry point.
- **A public `set` on everything** — if a value shouldn't change after creation, use `{ get; }` or `init`.
- **Confusing `static` and instance** — `Count++` inside a static method can't access `this`; an instance method can access static members, but not the other way around.
- **A record with a public `set`** — loses the benefit (immutability + a stable hash). If you need mutation — use a class.
- **A constructor that does heavy work** (network call, DB) — a constructor should be fast and predictable. Heavy work belongs in a separate method or a factory.
- **`==` between classes** when you mean content — for a `class` this compares references. Use a record, or an overridden `Equals` (module 03).

## Summary

- Class = template; object = instance with its own state. `new` invokes a constructor that is responsible for bringing the object into a valid state.
- Properties instead of public fields: `{ get; set; }`, `{ get; private set; }`, `{ get; init; }`, `required`, and computed (`=>`).
- Constructors: overloads with `: this(...)`, and primary constructors when the constructor is trivial.
- `static` — belongs to the class: constants, counters, factory methods.
- `record` for data (value equality, `with`), `class` for entities with identity.

## Further reading

- [Classes, structs, and records](https://learn.microsoft.com/dotnet/csharp/fundamentals/types/classes)
- [Properties](https://learn.microsoft.com/dotnet/csharp/programming-guide/classes-and-structs/properties)
- [Constructors](https://learn.microsoft.com/dotnet/csharp/programming-guide/classes-and-structs/constructors)
- [Primary constructors](https://learn.microsoft.com/dotnet/csharp/whats-new/tutorials/primary-constructors)
- [Records](https://learn.microsoft.com/dotnet/csharp/language-reference/builtin-types/record)
- [required modifier](https://learn.microsoft.com/dotnet/csharp/language-reference/keywords/required)
