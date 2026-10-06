# Exercises — Day 1

Short exercises (5–15 minutes each), grouped by module. Difficulty: ★ easy, ★★ medium, ★★★ challenging.
Solve each exercise in a new project of your own:

```bash
dotnet new console -n Ex03
cd Ex03
dotnet run
```

We'll go over the solutions together in class.

## Module 01 — C# warm-up

### Exercise 1 ★ — Temperature converter
Write two methods: `CelsiusToFahrenheit(double c)` and `FahrenheitToCelsius(double f)` (expression-bodied). Print a table from −10 to 40 degrees Celsius in steps of 10, using interpolation and formatting with one digit after the decimal point (`{x:F1}`).

### Exercise 2 ★ — FizzBuzz with a switch expression
For each number from 1 to 20: "FizzBuzz" if divisible by 15, "Fizz" if divisible by 3, "Buzz" if divisible by 5, otherwise the number itself. Use a `switch` expression with a tuple pattern: `(n % 3, n % 5) switch { (0, 0) => ..., (0, _) => ... }`.

## Module 02 — Classes and objects

### Exercise 3 ★ — A `Rectangle` class
A class with properties `Width`, `Height` (read-only, set in a constructor that validates they are positive), a computed property `Area`, a method `Scale(double factor)` that returns a **new** rectangle, and `ToString()`. Create two rectangles and print them.

### Exercise 4 ★★ — `record` vs. `class` and a static counter
a. Define `record Point(int X, int Y)` and `class PointClass` with the same fields. Create two equal instances of each and print `==` and `Equals`. Explain the difference in a comment.
b. Add to `PointClass` a static counter `Created` that is incremented in every constructor call, and a static method `Origin()` that returns (0,0).
c. Use `with` to create a shifted point from the record.

## Module 03 — OOP principles

### Exercise 5 ★★ — Vehicles
An abstract class `Vehicle` (`Name`, abstract `Wheels`, `virtual Describe()`), with derived classes `Car`, `Motorcycle`, `Truck` (`LoadCapacity`). An interface `IElectric { int BatteryKwh { get; } }` that one kind of `Car` implements (`ElectricCar : Car, IElectric`). Iterate over a `List<Vehicle>` and print `Describe()`; then print only the electric ones (`OfType<IElectric>()`).

### Exercise 6 ★★ — Pattern matching
A method `string Classify(object o)` that returns a description for: negative `int` / positive `int` / empty `string` / other `string` / `double` / `int[]` with 0 elements / other `int[]` / `null` / anything else. Use a `switch` expression with type patterns, property patterns (`{ Length: 0 }`) and `when`. Run it on a mixed `object[]` array.

## Module 04 — Collections and generics

### Exercise 7 ★ — Word count
Take a sentence (hard-coded), split it into words (`Split`), and count how many times each word appears using a `Dictionary<string, int>` (case-insensitive). Print the results sorted by descending frequency, then alphabetically.

### Exercise 8 ★★ — A generic `MyStack<T>`
Implement a stack on top of an internal array: `Push`, `Pop` (throws `InvalidOperationException` when empty), `Peek`, `Count`, `IsEmpty`. The array doubles in size when it fills up. Add the constraint `where T : notnull`. Test it with `int` and with `string`.

### Exercise 9 ★★ — The right collection
For each scenario, choose a collection and justify your choice in a comment, then implement it: (a) unique visitor IDs from a list with duplicates — how many are unique? (b) a print queue — documents are handled in order of arrival. (c) an "Undo" history. (d) fast lookup of a student by national ID number. (e) a list of grades for computing an average.

## Module 05 — Delegates, Lambdas, LINQ

### Exercise 10 ★ — LINQ on numbers
For `int[] nums = [5, 3, 8, 1, 9, 2, 7, 4, 6, 10]`: the sum of the squares of the even numbers, the three largest numbers, whether any number is > 9, the average of the odd numbers, and a string of the numbers sorted (`string.Join`). Each answer in a single LINQ line.

### Exercise 11 ★★ — Delegates and an event
a. A method `Func<int, int> Compose(Func<int, int> f, Func<int, int> g)` that returns `x => g(f(x))`. Test it with `x + 1` and `x * 2`.
b. A `Counter` class with `Increment()` and an event `ThresholdReached` (`EventHandler<int>`) that is raised when the counter reaches `Threshold`. Subscribe with a lambda and print.

## Module 06 — Exceptions and debugging

### Exercise 12 ★★ — Safe input
A method `int ReadInt(string prompt, int min, int max)` that asks the user for a number in a loop until the input is valid (`TryParse` + range). On EOF (`ReadLine()` returns `null`) — throw `EndOfStreamException`. Define `class ValidationException : Exception` with an `Input` property, and throw it from `Validate(int age)` if the age is not between 0 and 120. Catch it in `Main` and print a friendly message.

### Exercise 13 ★★★ — Execution order
A class `Step(string name) : IDisposable` that prints in its constructor and in `Dispose`. Write a method containing: `using var a = new Step("A");` and then `try { using var b = new Step("B"); throw new InvalidOperationException(); } catch { Console.WriteLine("catch"); } finally { Console.WriteLine("finally"); }`. **Before** running it, write in a comment the order in which you expect the lines to be printed. Run it and check. Add `catch (Exception ex) when (Log(ex))` with a `Log` that prints and returns `false` — when does it run?

## Module 07 — Clean code

### Exercise 14 ★★ — Refactoring
Here is a method. Rewrite it: clear names, constants instead of magic numbers, an `enum` for the customer type, extract small methods, and remove duplication. The behavior must stay identical.

```csharp
static double c(double a, int t, bool m)
{
    double r = 0;
    if (t == 1) { r = a * 0.9; if (m) r = r - 5; }
    else if (t == 2) { r = a * 0.8; if (m) r = r - 5; }
    else { r = a; if (m) r = r - 5; }
    if (r < 0) r = 0;
    return r * 1.18;
}
```
