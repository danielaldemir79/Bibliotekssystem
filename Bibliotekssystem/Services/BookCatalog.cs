using Bibliotekssystem.Models;

namespace Bibliotekssystem.Services
{
    public class BookCatalog
    {
        // Intern lista för att lagra böcker i katalogen
        private List<Book> _books = new();
        // Offentlig egenskap för att få tillgång till böckerna som en läsbar lista
        public IReadOnlyList<Book> Books => _books;

        // Metod för att lägga till en bok i katalogen
        public void AddBook(Book book)
        {   
            _books.Add(book);
        }

        // Metod för att ta bort en bok från katalogen
        public void RemoveBook(Book book)
        {
            _books.Remove(book);
        }

        // Metod för att hitta en bok baserat på dess ISBN
        public Book? FindByISBN(string isbn)
        {
            return _books.FirstOrDefault(b => b.ISBN == isbn);
        }

        // Metod för att hitta böcker baserat på titel
        public List<Book> FindByAuthor(string author)
        {
            return _books.Where(b => b.Author.Contains(author, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        // Metod för att hämta alla tillgängliga böcker i katalogen
        public List<Book> GetAvailableBooks()
        {
            return _books.Where(b => b.IsAvailable).ToList();
        }
    }
}
