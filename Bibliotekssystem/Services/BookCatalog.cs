using Bibliotekssystem.Models;

namespace Bibliotekssystem.Services
{
    public class BookCatalog
    {
        private List<Book> _books = new();
        public IReadOnlyList<Book> Books => _books;

        public void AddBook(Book book)
        {
            _books.Add(book);
        }

        public void RemoveBook(Book book)
        {
            _books.Remove(book);
        }

        public Book? FindByISBN(string isbn)
        {
            return _books.FirstOrDefault(b => b.ISBN == isbn);
        }

        public List<Book> FindByAuthor(string author)
        {
            return _books.Where(b => b.Author.Contains(author, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        public List<Book> GetAvailableBooks()
        {
            return _books.Where(b => b.IsAvailable).ToList();
        }
    }
}
