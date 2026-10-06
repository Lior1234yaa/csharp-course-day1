# Module 06 — Exception Handling and Debugging Strategies

Software fails: a file doesn't exist, the user typed "abc" instead of a number, the network went down, and someone tried to withdraw more than the account holds. The difference between good and bad software is not that the good one never fails, but that it fails **in a controlled way**: a clear message, a consistent state, resources released. In this module we learn the .NET exception mechanism, when to use it and when not to, and then the tool that will save you the most hours in your career — the debugger.

## What is an exception

An exception is an object that describes an error, and it is "thrown" (`throw`) from the place where the error was detected. Throwing stops normal execution and climbs up the call stack until someone "catches" it (`catch`). If nobody catches it, the program crashes with a stack trace. All exceptions inherit from `System.Exception`, and each one has a `Message`, a `StackTrace` and an `InnerException` (the exception that caused it).

Common exceptions you will meet: `NullReferenceException` (accessing null), `ArgumentException` / `ArgumentNullException` / `ArgumentOutOfRangeException` (invalid parameter), `InvalidOperationException` (the operation isn't valid in the current state), `FormatException` (parsing), `KeyNotFoundException`, `IndexOutOfRangeException`, `IOException`, `HttpRequestException`.

## `try` / `catch` / `finally`

```csharp
try
{
    var text = File.ReadAllText(path);
    var count = int.Parse(text);
    Console.WriteLine(100 / count);
}
catch (FileNotFoundException ex)             // specific first
{
    Console.WriteLine($"missing file: {ex.FileName}");
}
catch (FormatException)                       // the variable is optional
{
    Console.WriteLine("file does not contain a number");
}
catch (Exception ex)                          // general — last
{
    Console.WriteLine($"unexpected: {ex.GetType().Name}: {ex.Message}");
    throw;                                    // see below
}
finally
{
    Console.WriteLine("runs always — with or without exception");   // cleanup
}
```

- The first matching `catch` (in order, including inheritance) handles it. That's why specific comes before general — the compiler even warns you if the order is reversed.
- `finally` always runs: after success, after a `catch`, after a `return` inside the `try`, and even after `continue`. This is the place to release resources.
- You can have `try/finally` without `catch` — "I don't handle it, but I clean up".

### Exception filters — `when`

A filter lets you catch only when a condition holds, without catching and rethrowing:

```csharp
catch (InsufficientFundsException ex) when (ex.Shortfall > 1000)
{
    EscalateToManager(ex);
}
catch (InsufficientFundsException ex)
{
    Console.WriteLine(ex.Message);
}
catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.NotFound) { }
```

A subtle advantage: when the filter returns `false`, the stack is not "unwound" — and in the debugger you'll see the original throw location. A common logging trick: `catch (Exception ex) when (Log(ex))` where `Log` returns `false` — it logs without catching.

## `throw;` vs. `throw ex;`

When you catch an exception and want to pass it on (after logging it, for example):

```csharp
catch (Exception ex)
{
    logger.Log(ex);
    throw;          // correct: preserves the original stack trace
    // throw ex;    // wrong: the stack trace starts here — we've lost where it really happened (CA2200)
}
```

If you want to wrap it in your own exception, pass the original as the `InnerException`: `throw new DataAccessException("failed to load orders", ex);`. That way no information is lost.

## Custom exceptions

Define your own exception when the handler has **something to do** with the information: a business error type, accompanying data.

```csharp
public class BankException(string message) : Exception(message);   // base for all domain exceptions

public class InsufficientFundsException(string accountId, decimal requested, decimal available)
    : BankException($"Insufficient funds in {accountId}: requested {requested:N2}, available {available:N2}")
{
    public string AccountId { get; } = accountId;
    public decimal Requested { get; } = requested;
    public decimal Available { get; } = available;
    public decimal Shortfall => Requested - Available;
}
```

Conventions: an `Exception` suffix, inherit from `Exception` (not `ApplicationException`), a clear message built in the constructor, properties for the data. A hierarchy (`BankException` as the base) allows a single `catch (BankException)` for all business errors, plus a specific `catch` when needed.

## `using` and `IDisposable`

Objects that hold an external resource — a file, a DB connection, a socket, `HttpClient` — implement `IDisposable`. You must call `Dispose()` when you're done, **even if an exception occurred**. `using` does exactly that: a `try/finally` that calls `Dispose`.

```csharp
using (var writer = new StreamWriter("log.txt"))
{
    writer.WriteLine("hello");
}   // Dispose() here — the file is closed and the buffer is flushed

using var reader = new StreamReader("data.csv");   // using declaration (C# 8): Dispose at the end of the enclosing block
var header = reader.ReadLine();
```

Without `using`, a file may stay open (and another program won't be able to access it), or the buffer may never be written to disk (the log is "empty"). When you write a class that holds an `IDisposable` — implement `IDisposable` yourself and pass the `Dispose` call on.

## Guard clauses — fail early

Check parameters **at the entry** of a method and throw immediately. The error is caught close to its source, and the rest of the method is free of `if`s:

```csharp
public void Transfer(Account? from, Account? to, decimal amount)
{
    ArgumentNullException.ThrowIfNull(from);
    ArgumentNullException.ThrowIfNull(to);
    ArgumentOutOfRangeException.ThrowIfNegativeOrZero(amount);
    if (ReferenceEquals(from, to)) throw new ArgumentException("same account", nameof(to));
    // from here on — everything is valid
}

public Book(string isbn, string title)
{
    ArgumentException.ThrowIfNullOrWhiteSpace(isbn);
    ArgumentException.ThrowIfNullOrEmpty(title);
    // ...
}
```

The static methods `ThrowIfNull`, `ThrowIfNullOrEmpty`, `ThrowIfNullOrWhiteSpace`, `ThrowIfNegative`, `ThrowIfNegativeOrZero`, `ThrowIfGreaterThan` etc. (from .NET 6–8) save lines and automatically use the parameter name in the message.

## When **not** to throw: the TryParse pattern

Exceptions are (relatively) expensive and are meant for **exceptional** situations. Invalid user input is not exceptional — it's expected. That's why .NET has `Parse`/`TryParse` pairs:

```csharp
Console.Write("age: ");
var line = Console.ReadLine();
if (int.TryParse(line, out int age) && age is >= 0 and <= 120)
    Console.WriteLine($"ok, {age}");
else
    Console.WriteLine("please enter a number between 0 and 120");
```

Same idea: `Dictionary.TryGetValue`, `Dictionary.TryAdd`, `Queue.TryDequeue`, `Enum.TryParse`, `DateTime.TryParse`. And for your own methods: if failure is a routine outcome (a search that found nothing), return `bool`/`null`; if it's a rule violation (withdrawing more than the balance, an invalid parameter) — throw.

A typical input loop: read, `TryParse`, if invalid — print a message and read again; if `ReadLine()` returns `null` (end of input) — exit gracefully.

## Debugging strategies in Visual Studio

`Console.WriteLine` is not debugging. The debugger lets you pause the program, inspect any variable, and step forward line by line.

| Tool | How | When |
|---|---|---|
| **Breakpoint** | `F9` on a line, `F5` to run until it | "What's happening here?" |
| **Step Over / Into / Out** | `F10` / `F11` / `Shift+F11` | Line by line; step into a method; step out of it |
| **Conditional breakpoint** | Right-click the breakpoint → Conditions: `amount > 1000` or Hit Count | The bug only happens on iteration 347 |
| **Watch / Locals / Autos** | Debug → Windows → Watch; add an expression (`from.Balance`, `list.Count`) | Track values while stepping |
| **Immediate Window** | `Ctrl+Alt+I`; type expressions: `_accounts.Keys`, `Calc(5)` | Test a hypothesis without changing code |
| **Call Stack** | `Ctrl+Alt+C` | "How did I get here?" — double-click jumps to a frame |
| **Exception Settings** | `Ctrl+Alt+E`; check an exception type → break when thrown | Stop **where it's thrown**, not in the catch |
| **Edit & Continue** | Change code while the program is paused, then continue | Fix and test without restarting |
| **DataTips / Pin** | Hover over a variable; pin it with the pin icon | A quick peek |
| **Run to Cursor** | `Ctrl+F10` | "Run up to here" without a breakpoint |

Strategy: (1) **reproduce** the bug deterministically; (2) form a hypothesis about where it is — set a breakpoint **before** the suspicious spot; (3) `F10` with a Watch on the variables involved until the value "goes bad"; (4) fix it, run again, and ideally — write a test (Day 4). When there's an exception — Exception Settings + Call Stack show the source of the problem, not just the symptom.

In VS Code: the same concepts under Run and Debug (`F5`), with an automatically generated `launch.json`. `dotnet run` on its own does not run a debugger.

## `Debug.Assert` and logging

- **`Debug.Assert(condition, "message")`** — checks an assumption during development. If the condition is `false`, the debugger stops. In Release the line is **removed** entirely — zero cost. Use it for internal invariants ("the balance can't be negative here"), not for input validation.
- **`Debug.WriteLine`** — prints to the debugger's Output window, in Debug builds only.
- **Logging** (`ILogger` from `Microsoft.Extensions.Logging`, Day 4) — for production. Levels: Trace/Debug/Information/Warning/Error/Critical. Log the whole exception (`logger.LogError(ex, "transfer failed")`), not just `ex.Message`.

## Common mistakes

- **An empty `catch (Exception) { }`** — "swallows" errors; the software keeps running in a broken state and someone discovers it a week later. At least log it, and usually `throw;`.
- **`throw ex;`** — loses the stack trace.
- **Exceptions for control flow** — `try { int.Parse } catch` in a loop. Use `TryParse`.
- **A `catch` that's too broad at a low level** — catch what you know how to handle; let the rest bubble up to whoever does.
- **No `using`** on files/connections — resource leaks, locked files, an empty log.
- **An uninformative message** — `throw new Exception("error")`. Say what, where, and which value.
- **`Debug.Assert` for input validation** — it disappears in Release and the check won't run.
- **Debugging with `Console.WriteLine`** when you have a debugger — slow, clutters the code, and gets forgotten in production.

## Summary

- `try/catch/finally`: specific before general, `finally` always runs, `when` for filtering.
- `throw;` preserves the stack; custom exceptions carry data and share a common base.
- `using` for every `IDisposable` — guaranteed release.
- Guard clauses (`ArgumentException.ThrowIfNullOrEmpty`...) at the entry; `TryParse` for expected input; exceptions for exceptional situations.
- Debugger: breakpoints (including conditional ones), Step, Watch, Immediate, Call Stack, Exception Settings, Edit & Continue.
- `Debug.Assert` for internal assumptions during development; logging for production.

## Further reading

- [Exceptions and exception handling](https://learn.microsoft.com/dotnet/csharp/fundamentals/exceptions/)
- [Best practices for exceptions](https://learn.microsoft.com/dotnet/standard/exceptions/best-practices-for-exceptions)
- [Exception filters (when)](https://learn.microsoft.com/dotnet/csharp/language-reference/keywords/when)
- [using statement](https://learn.microsoft.com/dotnet/csharp/language-reference/statements/using)
- [Implement a Dispose method](https://learn.microsoft.com/dotnet/standard/garbage-collection/implementing-dispose)
- [Debugging in Visual Studio](https://learn.microsoft.com/visualstudio/debugger/)
- [Navigate through code with the debugger](https://learn.microsoft.com/visualstudio/debugger/navigating-through-code-with-the-debugger)
- [Manage exceptions with the debugger](https://learn.microsoft.com/visualstudio/debugger/managing-exceptions-with-the-debugger)
