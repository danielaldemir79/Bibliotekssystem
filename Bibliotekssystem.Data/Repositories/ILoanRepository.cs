using Bibliotekssystem.Core.Models;

namespace Bibliotekssystem.Data.Repositories
{
    // Kontrakt för lånehantering - hanterar utlåning och retur av böcker
    public interface ILoanRepository
    {
        Task<IEnumerable<Loan>> GetAllAsync();
        Task<Loan?> GetByIdAsync(int id);
        Task<IEnumerable<Loan>> GetActiveLoansAsync();
        Task<IEnumerable<Loan>> GetOverdueLoansAsync();
        Task<IEnumerable<Loan>> GetLoansByMemberAsync(int memberId);
        Task<IEnumerable<Loan>> GetLoansByBookAsync(int bookId);
        Task<Loan> CreateLoanAsync(int bookId, int memberId, int loanDays = 30);
        Task<bool> ReturnLoanAsync(int loanId);
        Task<IEnumerable<Loan>> SearchAsync(string searchTerm);
    }
}