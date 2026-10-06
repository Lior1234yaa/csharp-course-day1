# Day 1 — Object-Oriented Programming, Collections and Exception Handling

The first day of the course **"C# Programming in the .NET Framework — Updated Practical Training"**. We start with the language basics, build the object model, learn to manage data in the right collections, add LINQ and delegates, and finish with code that doesn't crash — using exceptions, debugging and clean code. Most of the day is hands-on: 4 labs and 14 exercises.

## What to install before the day

See [`../00-Setup/INSTALL.md`](../00-Setup/INSTALL.md). In short:

- .NET SDK 10 (`dotnet --version` prints `10.x`)
- Visual Studio 2022+ (workload ".NET desktop development") **or** VS Code with C# Dev Kit
- Git

Quick check:

```bash
dotnet new console -n Hello && cd Hello && dotnet run
```

## Learning goals

By the end of the day you will be able to:

1. Write a modern C# program: top-level statements, `var`, interpolation, `switch` expressions, nullable reference types.
2. Design classes with properties, constructors (including primary constructors) and `record`, and explain when each one fits.
3. Apply the four OOP principles in C#: `private`/`protected`, `virtual`/`override`/`abstract`, interfaces (including default members), `is`/`switch` patterns.
4. Choose the right collection (`List`, `Dictionary`, `HashSet`, `Queue`, `Stack`) based on Big-O, write a generic class with constraints, and expose collections safely (`IReadOnlyList<T>`).
5. Pass behavior with `Func`/`Action`, define and raise events, and write LINQ queries (`Where/Select/OrderBy/GroupBy/ToDictionary`) while understanding deferred execution.
6. Handle exceptions correctly: `try/catch/finally/when`, custom exceptions, `throw;`, `using`, guard clauses and `TryParse`.
7. Debug in Visual Studio: breakpoints (including conditional ones), Watch, Immediate, Call Stack, Exception Settings, Edit & Continue.
8. Identify and fix code "smells": naming, magic numbers, duplication, large classes — with SOLID and `dotnet format`.

## Schedule

| Time | Topic | Materials |
|---|---|---|
| 09:00–09:15 | Opening, introductions, environment setup | — |
| 09:15–09:50 | **Module 01** — C# warm-up: program, types, control flow, methods, nullable | [Notes/01](Notes/01-csharp-quickstart.md), [Demo Quickstart](Demos/Day1.Demo.Quickstart/) |
| 09:50–10:30 | **Module 02** — Classes, objects, properties, constructors, records | [Notes/02](Notes/02-classes-objects.md), [Demo Classes](Demos/Day1.Demo.Classes/) |
| 10:30–10:45 | Break | |
| 10:45–11:30 | **Lab 1** — Library system | [Labs/Lab1](Labs/Lab1-LibrarySystem/README.md) |
| 11:30–12:15 | **Module 03** — Encapsulation, inheritance, polymorphism, interfaces, abstraction | [Notes/03](Notes/03-oop-pillars.md), [Demo Polymorphism](Demos/Day1.Demo.Polymorphism/) |
| 12:15–13:00 | Lunch | |
| 13:00–14:00 | **Lab 2** — Shapes and employees (payroll) | [Labs/Lab2](Labs/Lab2-ShapesAndEmployees/README.md) |
| 14:00–14:30 | **Module 04** — Collections, generics, efficient data management | [Notes/04](Notes/04-collections.md), [Demo Collections](Demos/Day1.Demo.Collections/) |
| 14:30–14:45 | Break | |
| 14:45–15:15 | **Module 05** — Delegates, lambdas, events, LINQ | [Notes/05](Notes/05-delegates-lambdas-linq.md), [Demo LinqDelegates](Demos/Day1.Demo.LinqDelegates/) |
| 15:15–15:40 | **Modules 06 + 07** — Exceptions, debugging, clean code | [Notes/06](Notes/06-exceptions-debugging.md), [Notes/07](Notes/07-clean-code.md), [Demo Exceptions](Demos/Day1.Demo.Exceptions/) |
| 15:40–16:20 | **Lab 3 or Lab 4** (your choice; the other one is homework) | [Labs/Lab3](Labs/Lab3-InventoryLinq/README.md), [Labs/Lab4](Labs/Lab4-BankRobust/README.md) |
| 16:20–16:30 | Wrap-up, questions, preparing for Day 2 | |

Modules 06–07 are short in the lecture, and most of their material is practiced in Lab 4. At the end of the day you do either Lab 3 or Lab 4 in class, and the other one is homework.

## Modules (study materials)

| # | Module | Topics |
|---|---|---|
| 01 | [C# warm-up](Notes/01-csharp-quickstart.md) | Program.cs, `dotnet new/run`, value vs reference, `var`, strings, control flow, methods, nullable, namespaces, VS/VS Code |
| 02 | [Classes and objects](Notes/02-classes-objects.md) | class/object/`new`, fields vs properties (auto/init/required/computed), constructors and primary constructors, `this`, static, records, object initializers |
| 03 | [OOP principles](Notes/03-oop-pillars.md) | access modifiers, `base`/`virtual`/`override`/`sealed`/`abstract`, polymorphism, interfaces + default members, abstraction, composition, `is`/`as`/patterns, `ToString`/`Equals`/`GetHashCode` |
| 04 | [Collections and generics](Notes/04-collections.md) | arrays, `List`, `Dictionary`, `HashSet`, `Queue`/`Stack`, `IEnumerable`/`yield`, `Repository<T>` + constraints, Big-O table, `IReadOnlyList`, capacity, `Span<T>` |
| 05 | [Delegates, Lambdas, LINQ](Notes/05-delegates-lambdas-linq.md) | delegates, `Func`/`Action`/`Predicate`, closures, events, LINQ, method vs query syntax, deferred execution, functional style |
| 06 | [Exceptions and debugging](Notes/06-exceptions-debugging.md) | try/catch/finally, `when`, custom exceptions, `throw;`, `using`/IDisposable, guard clauses, TryParse, debugger tools, `Debug.Assert` |
| 07 | [Clean code](Notes/07-clean-code.md) | naming, small methods, SOLID, DRY, constants/enums, comments, `.editorconfig`/`dotnet format`, refactoring, reusable code |

## Demos (live coding)

Each demo is a standalone console project: `cd Demos/<Project> && dotnet run`.

| Project | Module | What it shows |
|---|---|---|
| [Day1.Demo.Quickstart](Demos/Day1.Demo.Quickstart/) | 01 | value/reference, `var`, switch expression, methods, nullable |
| [Day1.Demo.Classes](Demos/Day1.Demo.Classes/) | 02 | properties, constructors, primary constructor, static, record vs class |
| [Day1.Demo.Polymorphism](Demos/Day1.Demo.Polymorphism/) | 03 | abstract/virtual/override, interfaces, pattern matching, Equals, composition |
| [Day1.Demo.Collections](Demos/Day1.Demo.Collections/) | 04 | all the collections, `yield`, `Repository<T>`, `IReadOnlyList`, capacity, `Span` |
| [Day1.Demo.LinqDelegates](Demos/Day1.Demo.LinqDelegates/) | 05 | delegates, closures, events, LINQ, deferred execution |
| [Day1.Demo.Exceptions](Demos/Day1.Demo.Exceptions/) | 06 | try/catch/finally/when, `throw;`, using, guard clauses, TryParse, Debug.Assert |

## Exercises

[Exercises/README.md](Exercises/README.md) — 14 short exercises (★–★★★) by module. Solve each exercise in a new project of your own (`dotnet new console -n Ex03`); we go over the solutions together in class.

## Labs

| Lab | Duration | Topic | What you practice |
|---|---|---|---|
| [Lab 1 — Library system](Labs/Lab1-LibrarySystem/README.md) | 45 min | `Book`, `Library`, `List<Book>` | properties, constructors with validation, search, borrow/return, `IReadOnlyList` |
| [Lab 2 — Shapes and employees](Labs/Lab2-ShapesAndEmployees/README.md) | 60 min | Shape hierarchy + payroll with `IPayable` | abstract, virtual/override, interfaces, polymorphism, pattern matching |
| [Lab 3 — Inventory with LINQ](Labs/Lab3-InventoryLinq/README.md) | 60 min | `Inventory` + `Reports` | `Dictionary`, `HashSet`, `Func`/`Action` as parameters, `LowStock` event, LINQ, deferred execution |
| [Lab 4 — Robust bank](Labs/Lab4-BankRobust/README.md) | 60 min | A bug-ridden starter to fix | custom exceptions, guard clauses, `TryParse`, `using`/`finally`, debugging in Visual Studio |

Each lab has a `README.md` (instructions, acceptance criteria, hints) and a `Starter/` folder (a starter project that compiles, with `TODO`s).

### How to work on a lab

1. Read the lab's `README.md` all the way through before you start, especially the acceptance criteria.
2. Copy the `Starter` folder to a working folder of your own and work on the copy, so the original stays clean:

```powershell
Copy-Item -Recurse Labs\Lab1-LibrarySystem\Starter C:\my-work\Day1-Lab1
cd C:\my-work\Day1-Lab1
dotnet run
```

3. Work step by step, and run (`dotnet run`) after each step.
4. Stuck? First the hints in the README, then the module's Notes, then your neighbor, and then the instructor.
5. Finished early? Every lab has a bonus.

## Slides

[Slides/Day1.pptx](Slides/Day1.pptx)

## Preparing for Day 2

Day 2 is about concurrent work and communicating with the world outside your program: threads, `Task` and `async`/`await`, synchronization between threads, REST calls with `HttpClient`, and JSON. Make sure Lab 3 and Lab 4 are complete.
