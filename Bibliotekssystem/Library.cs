using Bibliotekssystem.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bibliotekssystem
{
    public class Library
    {
        public BookCatalog BookCatalog { get; }
        public MemberRegistry MemberRegistry { get; }
        public LoanManager LoanManager { get; }

        public Library()
        {
            BookCatalog = new BookCatalog();
            MemberRegistry = new MemberRegistry();
            LoanManager = new LoanManager();
        }

        public void DisplayStatistics()
        {
            Console.WriteLine("=== Biblioteksstatistik ===");
            Console.WriteLine($"Antal böcker: {BookCatalog.Books.Count}");
            Console.WriteLine($"Tillgängliga böcker: {BookCatalog.GetAvailableBooks().Count}");
            Console.WriteLine($"Antal medlemmar: {MemberRegistry.Members.Count}");
            Console.WriteLine($"Aktiva lån: {LoanManager.GetActiveLoans().Count}");
            Console.WriteLine($"Försenade lån: {LoanManager.GetOverdueLoans().Count}");
        }
    }
}
