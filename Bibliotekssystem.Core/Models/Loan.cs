using System.ComponentModel.DataAnnotations;
using Bibliotekssystem.Core.Interfaces;

namespace Bibliotekssystem.Core.Models
{
    public class Loan : ISearchable
    {
        // Primärnyckel
        public int Id { get; set; }

        // Foreign keys - talar om vilken bok och medlem lånet tillhör
        // EF Core använder dessa för att skapa relationer mellan tabeller
        [Required]
        public int BookId { get; set; } // Foreign key till Book-tabellen

        [Required]
        public int MemberId { get; set; } // Foreign key till Member-tabellen

        public DateTime LoanDate { get; set; } = DateTime.Now;

        public DateTime DueDate { get; set; }

        public DateTime? ReturnDate { get; set; }

        // Beräknade properties - sparas INTE i databasen, räknas ut varje gång
        public bool IsReturned => ReturnDate.HasValue;
        public bool IsOverdue => !IsReturned && DateTime.Now > DueDate;

        // Navigation properties - EF Core fyller i dessa automatiskt från databasen
        public Book Book { get; set; } = null!;
        public Member Member { get; set; } = null!;

        // Parameterlös konstruktor - krävs av EF Core
        public Loan() { }

        // Bakåtkompatibel konstruktor
        public Loan(int bookId, int memberId, DateTime loanDate, DateTime dueDate)
        {
            BookId = bookId;
            MemberId = memberId;
            LoanDate = loanDate;
            DueDate = dueDate;
        }

        public decimal CalculateLateFee(decimal feePerDay = 10m)
        {
            if (!IsOverdue) return 0;
            int daysLate = (DateTime.Now - DueDate).Days;
            return daysLate * feePerDay;
        }

        public void ReturnBook()
        {
            ReturnDate = DateTime.Now;
        }

        public string GetInfo()
        {
            string returnInfo = IsReturned
                ? $"Returned on: {ReturnDate!.Value.ToShortDateString()}"
                : "Not returned yet";
            return $"Book ID: {BookId}, Member ID: {MemberId}, Loan Date: {LoanDate.ToShortDateString()}, Due Date: {DueDate.ToShortDateString()}, {returnInfo}";
        }

        public bool Matches(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();
            return (Book?.Matches(searchTerm) ?? false) ||
                   (Member?.Matches(searchTerm) ?? false) ||
                   LoanDate.ToString("d").Contains(searchTerm) ||
                   DueDate.ToString("d").Contains(searchTerm) ||
                   (ReturnDate.HasValue && ReturnDate.Value.ToString("d").Contains(searchTerm));
        }
    }
}