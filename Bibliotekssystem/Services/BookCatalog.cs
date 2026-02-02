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

        // Metod för att hämta alla tillgängliga böcker i katalogen
        public List<Book> GetAvailableBooks()
        {
            return _books.Where(b => b.IsAvailable).ToList();
        }

        // Metod för att söka böcker baserat på ett sökord
        //returnerar en lista med böcker som matchar sökordet när Matches-metoden i Book-klassen blir true 
        public List<Book> SearchBooks(string searchTerm)
        {
            return _books.Where(b => b.Matches(searchTerm)).ToList();
        }


        // Metoder för att sortera böcker baserat på olika kriterier
        
        // Returnerar böcker sorterade alfabetiskt efter titel
        public List<Book> GetBooksSortedByTitle()
        {
            return _books.OrderBy(b => b.Title).ToList();
        }
        // Returnerar böcker sorterade alfabetiskt efter författare
        public List<Book> GetBooksSortedByAuthor()
        {
            return _books.OrderBy(b => b.Author).ToList();
        }
        // Returnerar böcker sorterade efter utgivningsår (äldst först)
        public List<Book> GetBooksSortedByPublishedYear()
        {
            return _books.OrderBy(b => b.PublishedYear).ToList();
        }

    }
}
