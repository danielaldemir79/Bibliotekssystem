using Bibliotekssystem.Models;

namespace Bibliotekssystem.Services
{
    public class LoanManager
    {
        private List<Loan> _loans = new();
        public IReadOnlyList<Loan> Loans => _loans;

        public Loan? CreateLoan(Book book, Member member, int loanDays = 30)
        {
            if (!book.IsAvailable)
                return null;

            var loan = new Loan(book, member, DateTime.Now, DateTime.Now.AddDays(loanDays));
            _loans.Add(loan);

            book.IsAvailable = false;
            member.AddBorrowedBook(book);

            return loan;
        }

        public bool ReturnLoan(Loan loan)
        {
            if (loan.IsReturned)
                return false;

            loan.ReturnBook();
            loan.Book.IsAvailable = true;
            loan.Member.RemoveBorrowedBook(loan.Book);

            return true;
        }

        public List<Loan> GetActiveLoans()
        {
            return _loans.Where(l => !l.IsReturned).ToList();
        }

        public List<Loan> GetOverdueLoans()
        {
            return _loans.Where(l => l.IsOverdue).ToList();
        }

        public List<Loan> GetLoansByMember(Member member)
        {
            return _loans.Where(l => l.Member == member).ToList();
        }
    }
}
