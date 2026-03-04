using Microsoft.EntityFrameworkCore;
using Bibliotekssystem.Core.Models;

namespace Bibliotekssystem.Data.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly LibraryContext _context;

        public BookRepository(LibraryContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Book>> GetAllAsync()
        {
            return await _context.Books.ToListAsync();
        }

        public async Task<Book?> GetByIdAsync(int id)
        {
            return await _context.Books
                .Include(b => b.Loans)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<Book?> GetByISBNAsync(string isbn)
        {
            return await _context.Books
                .FirstOrDefaultAsync(b => b.ISBN == isbn);
        }

        // Lägger till en bok, kastar fel om ISBN redan finns
        public async Task AddAsync(Book book)
        {
            var existing = await _context.Books.FirstOrDefaultAsync(b => b.ISBN == book.ISBN);
            if (existing != null)
                throw new InvalidOperationException($"En bok med ISBN '{book.ISBN}' finns redan.");

            _context.Books.Add(book);
            await _context.SaveChangesAsync();
        }

        // Uppdaterar en bok, kontrollerar att ISBN inte krockar med annan bok
        public async Task UpdateAsync(Book book)
        {
            var duplicate = await _context.Books
                .FirstOrDefaultAsync(b => b.ISBN == book.ISBN && b.Id != book.Id);
            if (duplicate != null)
                throw new InvalidOperationException($"En annan bok med ISBN '{book.ISBN}' finns redan.");

            _context.Books.Update(book);
            await _context.SaveChangesAsync();
        }

        // Tar bort en bok - blockeras om boken har aktiva lån
        public async Task DeleteAsync(int id)
        {
            var book = await _context.Books
                .Include(b => b.Loans)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (book == null)
                throw new KeyNotFoundException($"Bok med ID {id} hittades inte.");

            if (book.Loans.Any(l => l.ReturnDate == null))
                throw new InvalidOperationException("Boken har aktiva lån och kan inte tas bort.");

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Book>> SearchAsync(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();
            return await _context.Books
                .Where(b => b.Title.ToLower().Contains(searchTerm) ||
                            b.Author.ToLower().Contains(searchTerm) ||
                            b.ISBN.ToLower().Contains(searchTerm) ||
                            b.PublishedYear.ToString().Contains(searchTerm))
                .ToListAsync();
        }
    }
}