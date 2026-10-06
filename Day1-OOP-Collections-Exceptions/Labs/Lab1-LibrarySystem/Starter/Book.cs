namespace Day1.Lab1;

/// <summary>A book in the library.</summary>
public class Book
{
    // TODO 1: Add properties:
    //   - Isbn (string, read-only — set in the constructor)
    //   - Title (string), Author (string), Year (int)
    //   - IsBorrowed (bool) — public get, private set
    //   - Age (int, computed): how many years have passed since the publication year

    // TODO 2: A constructor that takes isbn, title, author, year and checks that none of them is empty/unreasonable
    //         (year between 1450 and the current year). Throw ArgumentException on invalid input.
    public Book(string isbn, string title, string author, int year)
    {
        throw new NotImplementedException();
    }

    // TODO 3: Borrow() and Return() methods:
    //   Borrow — if the book is already borrowed, throw InvalidOperationException; otherwise mark it as borrowed.
    //   Return — if the book is not borrowed, throw InvalidOperationException; otherwise mark it as available.
    public void Borrow() => throw new NotImplementedException();
    public void Return() => throw new NotImplementedException();

    // TODO 4: Override ToString to return, for example:  "[978-1] Clean Code / Robert Martin (2008) — available"
    public override string ToString() => throw new NotImplementedException();
}
