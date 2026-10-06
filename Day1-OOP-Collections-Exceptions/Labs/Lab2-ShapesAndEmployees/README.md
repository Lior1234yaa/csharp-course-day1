# Lab 2 — Shapes and Employees: Inheritance, Abstraction and Polymorphism (60 minutes)

## Goal

Build two small hierarchies and get hands-on experience with the four core concepts: an abstract class that defines a contract, inheritance that extends behavior (`base`), an interface that lets you treat different objects in a uniform way, and polymorphism — one call, different behavior depending on the actual type.

## What you need to know first

Modules 02 and 03. Lab 1 is recommended.

## Getting started

```bash
cd Labs/Lab2-ShapesAndEmployees/Starter
dotnet run
```

## Steps

### Part A — Shapes (15 min)

1. **TODO 1** — Make `Shape` `abstract`, with abstract `Name`, `Area()` and `Perimeter()` members and a shared `ToString()`.
2. **TODO 2** — `Circle`, `Rectangle`, `Triangle`. For the triangle, check in the constructor that the sides are valid (the triangle inequality) — otherwise throw `ArgumentException`.
3. **TODO 3** — `Square : Rectangle`, marked `sealed`. Notice how little code it needs.
4. **TODO 11** — In `Program.cs`: a `List<Shape>`, printing, the sum of the areas, and an attempt to create an invalid triangle inside a `try/catch`.

### Part B — Payroll (45 min)

5. **TODO 4** — An `IPayable` interface with `PayeeName`, `CalculateMonthlyPay()`, and a default member `PaySlip()`.
6. **TODO 5** — An abstract `Employee` class that implements `IPayable`. `CalculateMonthlyPay` stays abstract; `Describe()` is `virtual`.
7. **TODO 6** — `SalariedEmployee`: annual salary / 12.
8. **TODO 7** — `HourlyEmployee`: 160 regular hours; anything beyond that is paid at 1.5x. Override `Describe()` and use `base.Describe()`.
9. **TODO 8** — `Manager : SalariedEmployee` with `Bonus` and `Reports`. `CalculateMonthlyPay` = `base` + bonus.
10. **TODO 9** — `Contractor`, which implements `IPayable` **without** inheriting from `Employee`. Ask yourself: why?
11. **TODO 10** — `Payroll` with a `List<IPayable>`: `Add`, `TotalMonthly`, `PrintSlips`.
12. **TODO 12** — In `Program.cs`: create the employees and the contractor, print the pay slips, and then print `Describe()` for the employees only (`is Employee e`).

## Acceptance criteria

- [ ] `new Shape()` does not compile (the class is abstract).
- [ ] `Triangle(1, 1, 10)` throws `ArgumentException`; `Triangle(3, 4, 5)` has an area of 6.
- [ ] `Square(2)` prints `Square: area=4.00, perimeter=8.00` — without rewriting `Area()` in Square.
- [ ] `Yossi` (₪80/hour, 170 hours) gets 14,000 (160×80 + 10×80×1.5).
- [ ] `Noa` (360,000 annual + 2,000 bonus) gets 32,000.
- [ ] `Payroll` accepts both a `Contractor` and an `Employee` in the same list, and the grand total is correct (78,000 with the sample data).
- [ ] `PaySlip()` is not implemented in any class — only in the interface.
- [ ] The contractor does not appear in the `Describe()` loop.

## Bonus

- A `switch` with pattern matching that classifies each `IPayable` as: "senior manager" (more than one direct report), "manager", "hourly with overtime", "regular employee", "external". Pay attention to the order of the cases.
- Add `interface IDrawable { void Draw(); }` with an ASCII implementation for `Rectangle` and `Square` only — a shape can implement several interfaces.
- `Payroll.Employees` — a property that returns only the employees (`OfType<Employee>()`).

## Hints

- A primary constructor that passes values to the base: `class Square(double side) : Rectangle(side, side)`.
- A default member in an interface: `string PaySlip() => $"...";` — just a body inside the interface.
- To call `PaySlip()` on a `SalariedEmployee` you need a variable of type `IPayable`.
- `Math.Min` / `Math.Max` help you compute regular/overtime hours without an `if`.
