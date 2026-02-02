using Bibliotekssystem.Models;

namespace Bibliotekssystem.Services
{
    public class LoanManager
    {
        // Intern lista för att lagra lån
        private List<Loan> _loans = new();
        public IReadOnlyList<Loan> Loans => _loans;

        // Metod för att skapa ett nytt lån
        //Om boken inte är tillgänglig returneras null
        public Loan? CreateLoan(Book book, Member member, int loanDays = 30)
        {
            // Kontrollera om boken är tillgänglig för utlåning
            if (!book.IsAvailable)
                return null;

            // Skapa ett nytt lån och uppdatera bokens tillgänglighet och medlemmens lånade böcker
            var loan = new Loan(book, member, DateTime.Now, DateTime.Now.AddDays(loanDays));
            _loans.Add(loan);

            book.IsAvailable = false;
            member.AddBorrowedBook(book);

            // Returnera det skapade lånet
            return loan;
        }


        // Metod för att returnera en bok
        public bool ReturnLoan(Loan loan)
        {
            // Kontrollera om lånet redan är returnerat
            if (loan.IsReturned)
                return false;

            // Markera lånet som returnerat, uppdatera bokens tillgänglighet
            // och ta bort boken från medlemmens lånade böcker
            loan.ReturnBook();
            loan.Book.IsAvailable = true;
            loan.Member.RemoveBorrowedBook(loan.Book);

            // Returnera true för att indikera att returprocessen lyckades
            return true;
        }

        public List<Loan> GetActiveLoans()
        {
            // Returnerar alla lån som inte har returnerats än
            return _loans.Where(l => !l.IsReturned).ToList();
        }

        public List<Loan> GetOverdueLoans()
        {
            // Returnerar alla lån som är försenade
            return _loans.Where(l => l.IsOverdue).ToList();
        }

        public List<Loan> GetLoansByMember(Member member)
        {
            // Returnerar alla lån för en specifik medlem
            return _loans.Where(l => l.Member == member).ToList();
        }

        public List<Loan> SearchLoans(string searchTerm)
        {
            // Returnerar alla lån som matchar sökordet
            return _loans.Where(l => l.Matches(searchTerm)).ToList();
        }

        // Returnerar medlemmen med flest lån (mest aktiva låntagaren)
        public Member? GetMostActiveBorrower()
        {

            return _loans
                .Where(l => !l.IsReturned)           // 1. Ta bara aktiva lån
                .GroupBy(l => l.Member)              // 2. Gruppera efter medlem
                .OrderByDescending(g => g.Count())   // 3. Sortera med flest lån först
                .Select(g => g.Key)                  // 4. Plocka ut medlemmen
                .FirstOrDefault();                   // 5. Ta första (den med flest)
        }
    }
}
