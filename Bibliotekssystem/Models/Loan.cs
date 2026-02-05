namespace Bibliotekssystem.Models
{
    public class Loan : Interfaces.ISearchable
    {
        public Book Book { get; }
        public Member Member { get; }
        public DateTime LoanDate { get; }
        public DateTime DueDate { get; }
        public DateTime? ReturnDate { get; private set; }
        public bool IsReturned => ReturnDate.HasValue;
        public bool IsOverdue => !IsReturned && DateTime.Now > DueDate;

        public decimal CalculateLateFee(decimal feePerDay = 10m) // Standardavgift per dag är 10 kronor vid försening
        {
            if (!IsOverdue) return 0;                       // Om boken inte är försenad, ingen avgift
            int daysLate = (DateTime.Now - DueDate).Days;   // Beräkna antalet dagar som boken är försenad
            return daysLate * feePerDay;                    // Returnera den totala avgiften baserat på antalet försenade dagar
        }

        public Loan(Book book, Member member, DateTime loanDate, DateTime dueDate)
        {
            Book = book;
            Member = member;
            LoanDate = loanDate;
            DueDate = dueDate;
            ReturnDate = null;
        }

        public void ReturnBook()
        {
            // Sätter returdatumet till nuvarande datum och tid
            ReturnDate = DateTime.Now;
        }

        public string GetInfo()
        {
            // Returnerar en sträng med all information om lånet
            string returnInfo = IsReturned ? $"Returned on: {ReturnDate.Value.ToShortDateString()}" : "Not returned yet";
            return $"Book: [{Book.GetInfo()}], Member: [{Member.GetInfo()}], Loan Date: {LoanDate.ToShortDateString()}, Due Date: {DueDate.ToShortDateString()}, {returnInfo}";
        }

        public bool Matches(string searchTerm)
        {
            // Gör sökordet till gemener för att möjliggöra case-insensitive sökning
            searchTerm = searchTerm.ToLower();
            
            // Kontrollera om sökordet matchar någon av lånets egenskaper
            //Returnerar true om någon egenskap matchar sökordet
            return Book.Matches(searchTerm) ||
                   Member.Matches(searchTerm) ||
                   LoanDate.ToString("d").Contains(searchTerm) ||
                   DueDate.ToString("d").Contains(searchTerm) ||
                   (ReturnDate.HasValue && ReturnDate.Value.ToString("d").Contains(searchTerm));
        }

     
    }
}
