using Microsoft.EntityFrameworkCore;
using Bibliotekssystem.Core.Models;

namespace Bibliotekssystem.Data.Repositories
{
    public class MemberRepository : IMemberRepository
    {
        private readonly LibraryContext _context;

        public MemberRepository(LibraryContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Member>> GetAllAsync()
        {
            return await _context.Members
                .Include(m => m.Loans)
                .ToListAsync();
        }

        public async Task<Member?> GetByIdAsync(int id)
        {
            return await _context.Members
                .Include(m => m.Loans)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<Member?> GetByMemberIdAsync(string memberId)
        {
            return await _context.Members
                .Include(m => m.Loans)
                .FirstOrDefaultAsync(m => m.MemberId == memberId);
        }

        // Lägger till en medlem, kontrollerar att MemberId och Email är unika
        public async Task AddAsync(Member member)
        {
            var existingId = await _context.Members
                .FirstOrDefaultAsync(m => m.MemberId == member.MemberId);
            if (existingId != null)
                throw new InvalidOperationException($"En medlem med ID '{member.MemberId}' finns redan.");

            var existingEmail = await _context.Members
                .FirstOrDefaultAsync(m => m.Email == member.Email);
            if (existingEmail != null)
                throw new InvalidOperationException($"En medlem med e-post '{member.Email}' finns redan.");

            _context.Members.Add(member);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Member member)
        {
            var duplicateEmail = await _context.Members
                .FirstOrDefaultAsync(m => m.Email == member.Email && m.Id != member.Id);
            if (duplicateEmail != null)
                throw new InvalidOperationException($"En annan medlem med e-post '{member.Email}' finns redan.");

            _context.Members.Update(member);
            await _context.SaveChangesAsync();
        }

        // Tar bort en medlem - blockeras om medlemmen har aktiva lån
        public async Task DeleteAsync(int id)
        {
            var member = await _context.Members
                .Include(m => m.Loans)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (member == null)
                throw new KeyNotFoundException($"Medlem med ID {id} hittades inte.");

            if (member.Loans.Any(l => l.ReturnDate == null))
                throw new InvalidOperationException("Medlemmen har aktiva lån och kan inte tas bort.");

            _context.Members.Remove(member);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Member>> SearchAsync(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();
            return await _context.Members
                .Where(m => m.Name.ToLower().Contains(searchTerm) ||
                            m.MemberId.ToLower().Contains(searchTerm) ||
                            m.Email.ToLower().Contains(searchTerm))
                .ToListAsync();
        }
    }
}