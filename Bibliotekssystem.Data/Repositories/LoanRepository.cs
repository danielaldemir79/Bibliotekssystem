using Microsoft.EntityFrameworkCore;
using Bibliotekssystem.Core.Models;

namespace Bibliotekssystem.Data.Repositories
{
    public class LoanRepository : ILoanRepository
    {
        private readonly LibraryContext _context;

        public LoanRepository(LibraryContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Loan>> GetAllAsync()
        {
            return await _context.Loans
                .Include(l => l.Book)
                .Include(l => l.Member)
                .ToListAsync();
        }

        public async Task<Loan?> GetByIdAsync(int id)
        {
            return await _context.Loans
                .Include(l => l.Book)
                .Include(l => l.Member)
                .FirstOrDefaultAsync(l => l.Id == id);
        }

        public async Task<IEnumerable<Loan>> GetActiveLoansAsync()
        {
            return await _context.Loans
                .Include(l => l.Book)
                .Include(l => l.Member)
                .Where(l => l.ReturnDate == null)
                .ToListAsync();
        }

        public async Task<IEnumerable<Loan>> GetOverdueLoansAsync()
        {
            return await _context.Loans
                .Include(l => l.Book)
                .Include(l => l.Member)
                .Where(l => l.ReturnDate == null && l.DueDate < DateTime.Now)
                .ToListAsync();
        }

        public async Task<IEnumerable<Loan>> GetLoansByMemberAsync(int memberId)
        {
            return await _context.Loans
                .Include(l => l.Book)
                .Include(l => l.Member)
                .Where(l => l.MemberId == memberId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Loan>> GetLoansByBookAsync(int bookId)
        {
            return await _context.Loans
                .Include(l => l.Book)
                .Include(l => l.Member)
                .Where(l => l.BookId == bookId)
                .ToListAsync();
        }

        // Skapar ett nytt lån - validerar att bok finns och är tillgänglig, och att medlem finns
        public async Task<Loan> CreateLoanAsync(int bookId, int memberId, int loanDays = 30)
        {
            var book = await _context.Books.FindAsync(bookId);
            if (book == null)
                throw new KeyNotFoundException($"Bok med ID {bookId} hittades inte.");

            if (!book.IsAvailable)
                throw new InvalidOperationException($"Boken '{book.Title}' är redan utlånad.");

            var member = await _context.Members.FindAsync(memberId);
            if (member == null)
                throw new KeyNotFoundException($"Medlem med ID {memberId} hittades inte.");

            var loan = new Loan
            {
                BookId = bookId,
                MemberId = memberId,
                LoanDate = DateTime.Now,
                DueDate = DateTime.Now.AddDays(loanDays)
            };

            // Markera boken som utlånad
            book.IsAvailable = false;

            _context.Loans.Add(loan);
            await _context.SaveChangesAsync();

            loan.Book = book;
            loan.Member = member;

            return loan;
        }

        // Returnerar ett lån - sätter returdatum och markerar boken som tillgänglig
        public async Task<bool> ReturnLoanAsync(int loanId)
        {
            var loan = await _context.Loans
                .Include(l => l.Book)
                .FirstOrDefaultAsync(l => l.Id == loanId);

            if (loan == null)
                throw new KeyNotFoundException($"Lån med ID {loanId} hittades inte.");

            if (loan.ReturnDate != null)
                return false;

            loan.ReturnDate = DateTime.Now;
            loan.Book.IsAvailable = true;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Loan>> SearchAsync(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();
            return await _context.Loans
                .Include(l => l.Book)
                .Include(l => l.Member)
                .Where(l => l.Book.Title.ToLower().Contains(searchTerm) ||
                            l.Book.Author.ToLower().Contains(searchTerm) ||
                            l.Member.Name.ToLower().Contains(searchTerm) ||
                            l.Member.MemberId.ToLower().Contains(searchTerm))
                .ToListAsync();
        }
    }
}