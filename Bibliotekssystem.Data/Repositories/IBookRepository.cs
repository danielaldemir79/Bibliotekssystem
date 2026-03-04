using Bibliotekssystem.Core.Models;

namespace Bibliotekssystem.Data.Repositories
{
    // Kontrakt för bokhantering - definierar vilka operationer som finns
    public interface IBookRepository
    {
        Task<IEnumerable<Book>> GetAllAsync();
        Task<Book?> GetByIdAsync(int id);
        Task<Book?> GetByISBNAsync(string isbn);
        Task AddAsync(Book book);
        Task UpdateAsync(Book book);
        Task DeleteAsync(int id);
        Task<IEnumerable<Book>> SearchAsync(string searchTerm);
    }
}