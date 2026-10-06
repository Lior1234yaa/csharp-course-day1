# Lab 1 — Library System (45 minutes)

## Goal

Build a small, well-organized object model: a `Book` class with properties and a constructor that validates its input, and a `Library` class that manages a `List<Book>` — adding, searching, borrowing and returning. By the end of the lab you should be able to explain why an object's state should change only through its own methods.

## What you need to know first

- Module 01 (C# warm-up) and Module 02 (classes, objects, properties, constructors).
- Module 04 (List<T>) helps, but you can get by with `foreach` alone.

## Getting started

```bash
cd Labs/Lab1-LibrarySystem/Starter
dotnet run
```

The project already compiles, but every method throws `NotImplementedException`. Work through the `// TODO` comments in order.

## Steps

### Step 1 — `Book` (15 min)

1. **TODO 1** — Add the properties: `Isbn` (read-only), `Title`, `Author`, `Year`, `IsBorrowed` (`private set`), and a computed `Age`.
2. **TODO 2** — Write the constructor. Make sure the strings are not empty (`ArgumentException.ThrowIfNullOrWhiteSpace`) and that the year makes sense (`ArgumentOutOfRangeException`).
3. **TODO 3** — `Borrow()` and `Return()`. Think: what should happen when someone tries to borrow a book that is already borrowed? (Hint: that breaks a rule — throw `InvalidOperationException`.)
4. **TODO 4** — `ToString()` in the format `[isbn] Title / Author (Year) — available|BORROWED`.

### Step 2 — `Library` (20 min)

5. **TODO 5** — Expose the books as `IReadOnlyList<Book>`. Why not `List<Book>`?
6. **TODO 6** — `AddBook`, which prevents duplicate ISBNs.
7. **TODO 7–9** — Searches: by part of the title, by author, by ISBN (returns `null` if there is no match).
8. **TODO 10** — `Borrow(isbn)` / `Return(isbn)` — return `false` if the book does not exist; otherwise call the book's own method.
9. **TODO 11** — `GetAvailable()`, sorted by title.

### Step 3 — Wiring it up (10 min)

10. **TODO 12** — Uncomment the lines in `Seed` and run the program. Try every menu option, including borrowing the same book twice.

## Acceptance criteria

- [ ] `dotnet build` passes with no errors and no warnings.
- [ ] Creating `new Book("", "x", "y", 2000)` throws `ArgumentException`.
- [ ] Creating a book with the year 3000 throws `ArgumentOutOfRangeException`.
- [ ] Borrowing a book twice prints `Error: ... already borrowed` and does not crash the program.
- [ ] `SearchByTitle("CLEAN")` finds "Clean Code" (case-insensitive).
- [ ] `GetAvailable()` does not include borrowed books, and is sorted by title.
- [ ] `Books` is declared as `IReadOnlyList<Book>`, and you cannot call `library.Books.Add(...)`.

## Bonus

- `BorrowedCount` — how many books are currently borrowed.
- `Oldest` — the oldest book (hint: `MinBy`).
- Add a `DueDate` (return date) that `Borrow` sets to 14 days from now, and an `IsOverdue` property.

## Hints

- `string.Contains(text, StringComparison.OrdinalIgnoreCase)` — case-insensitive search.
- `_books.FirstOrDefault(b => b.Isbn == isbn)` returns `null` when there is no match.
- `List<T>` implements `IReadOnlyList<T>`, so `public IReadOnlyList<Book> Books => _books;` works.
- `DateTime.Now.Year` — the current year.
