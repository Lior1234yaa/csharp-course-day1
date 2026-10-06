namespace Day1.Lab1;

/// <summary>The library's book collection and the operations on it.</summary>
public class Library
{
    private readonly List<Book> _books = [];

    // TODO 5: Expose the books as IReadOnlyList<Book> (without allowing changes from outside)
    public IReadOnlyList<Book> Books => throw new NotImplementedException();

    public int Count => _books.Count;

    // TODO 6: AddBook — if a book with the same ISBN already exists, throw InvalidOperationException
    public void AddBook(Book book) => throw new NotImplementedException();

    // TODO 7: Search by part of the title (case-insensitive). Return all matches.
    public List<Book> SearchByTitle(string text) => throw new NotImplementedException();

    // TODO 8: Search by author (exact match, case-insensitive)
    public List<Book> SearchByAuthor(string author) => throw new NotImplementedException();

    // TODO 9: FindByIsbn — returns the book, or null if not found
    public Book? FindByIsbn(string isbn) => throw new NotImplementedException();

    // TODO 10: Borrow / Return by ISBN. If the book does not exist — return false.
    //          If it exists — call book.Borrow()/Return() and return true.
    public bool Borrow(string isbn) => throw new NotImplementedException();
    public bool Return(string isbn) => throw new NotImplementedException();

    // TODO 11: A list of all available (not borrowed) books, sorted by title
    public List<Book> GetAvailable() => throw new NotImplementedException();

    // Bonus: how many books are currently borrowed? Which is the oldest book?
}
