# Lab 4 — A Robust Bank: Exceptions, Validation and Debugging (60 minutes)

## Goal

This time you don't write from scratch — you **fix**. The Starter is a small bank that compiles and runs, but contains at least 8 bugs: logic bugs, one that loses the stack trace, one that leaves a file empty, and two that crash the program on unexpected input. You will practice the Visual Studio debugging tools (breakpoints, Watch, Call Stack, Exception Settings), build a hierarchy of custom exceptions, and turn the program into one that does not crash on any input.

## What you need to know first

Module 06 (exceptions and debugging) and Module 07 (clean code). Lab 1 helps — it has a similar model.

## Getting started

```bash
cd Labs/Lab4-BankRobust/Starter
dotnet run
```

Then type:

```text
open acc1 Dana
deposit acc1 100
withdraw acc1 80
show acc1
```

Something wrong? Good. Let's begin.

## Steps

### Step 1 — Bug hunt (25 min)

Work with the debugger, not with `Console.WriteLine`. For each bug, write down: where it is, what the symptom is, and what the fix is.

1. Run the sequence above. If you got `not found` — why did `Open` find the account but `Get` did not? (Hint: Immediate Window → `_accounts.Keys`.)
2. Put a breakpoint in `Account.Withdraw` and step through it line by line (F10) with a Watch on `Balance`. What is the correct order of the operations?
3. Make 4 deposits and then `show` — how many history lines are printed? (There should be 3.)
4. Open two accounts, do a `transfer`, and then `list`. Did the money move?
5. Try `deposit acc1 0`. Should that succeed?
6. Finish with `quit` and open `transactions.log`. What is in it?
7. Exception Settings → check `BankException`. Make a withdrawal that fails and look at the Call Stack. Where did `Account.Withdraw` disappear to? (Hint: `throw ex;`)

### Step 2 — Robustness against bad input (15 min)

8. **TODO B** — Try `deposit acc1 abc`. The program crashed with a `FormatException`. Replace `decimal.Parse` with a helper function `ParseAmount` that uses `TryParse`.
9. **TODO C** — Try `deposit acc1` (without an amount). `IndexOutOfRangeException`. Add a guard clause `RequireArgs(parts, n)` and a matching `catch`.
10. Make sure that `processed + failed` always equals the number of commands typed. Where should `processed++` go? (Hint: `finally`.)

### Step 3 — Custom exceptions and `using` (20 min)

11. **TODO A** — In `Exceptions.cs`, build the hierarchy: `AccountNotFoundException`, `InsufficientFundsException` (with `Requested`, `Available`, `Shortfall`), `InvalidAmountException`. Use them in `Account` and `Bank`.
12. In `Program`, add a `catch (InsufficientFundsException ex)` **before** the `catch (BankException)` that also prints `short by ...`.
13. Wrap `TransactionLog` in a `using` so that the file is always closed.
14. Add guard clauses (`ArgumentException.ThrowIfNullOrWhiteSpace`) to the `Account` constructor and to `Bank.Open`.

## Acceptance criteria

- [ ] `deposit 100` → `withdraw 80` leaves a balance of 20 (not -60).
- [ ] `withdraw 500` on a balance of 70 prints `error: Insufficient funds ... (short by 430.00)`.
- [ ] `deposit acc1 abc`, `deposit acc1`, `deposit acc1 0`, `foo` — all print a message and do not crash the program.
- [ ] `transfer acc1 acc2 50` really transfers the money (check with `list`).
- [ ] `show` prints at most 3 history lines.
- [ ] `deposit ACC1 ...` and `deposit acc1 ...` go to the same account.
- [ ] After `quit`, `transactions.log` contains all the operations.
- [ ] `Bank.Withdraw` uses `throw;` and not `throw ex;`.
- [ ] `processed + failed` = the number of commands.
- [ ] There are at least 3 exception classes that inherit from `BankException`, and at least one of them has an extra property.

## Bonus

- Rollback in `Transfer`: if the deposit to the target account fails, return the money to the source account (`try/catch` + `throw;`).
- Prevent a transfer from an account to itself.
- Conditional breakpoint: stop in `Transfer` only when `amount > 40`.
- Add `Debug.Assert(Balance >= 0)` at the end of `Withdraw` — and check that it does not run with `dotnet run -c Release`.

## Hints

- `decimal.TryParse(text, NumberStyles.Number, CultureInfo.InvariantCulture, out var amount)` — so that `10.5` works on every machine.
- `new Dictionary<string, Account>(StringComparer.OrdinalIgnoreCase)`.
- `continue` inside a `catch` does **not** skip the `finally`.
- In Visual Studio: **Debug → Windows → Exception Settings**, search for your exception's name or check "Common Language Runtime Exceptions".
- `using (var log = new TransactionLog(...)) { ... }` — with the whole loop inside.
