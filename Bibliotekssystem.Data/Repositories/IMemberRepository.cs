using Bibliotekssystem.Core.Models;

namespace Bibliotekssystem.Data.Repositories
{
    // Kontrakt för medlemshantering - definierar vilka operationer som finns
    public interface IMemberRepository
    {
        Task<IEnumerable<Member>> GetAllAsync();
        Task<Member?> GetByIdAsync(int id);
        Task<Member?> GetByMemberIdAsync(string memberId);
        Task AddAsync(Member member);
        Task UpdateAsync(Member member);
        Task DeleteAsync(int id);
        Task<IEnumerable<Member>> SearchAsync(string searchTerm);
    }
}