using Bibliotekssystem.Services;
using Bibliotekssystem.Helpers;

namespace Bibliotekssystem
{
    public class Library
    {
        // Huvudklassen för bibliotekssystemet som hanterar bokkatalogen.

        //BookCatalog för att hantera böcker
        public BookCatalog BookCatalog { get; }
        // MemberRegistry för att hantera medlemmar
        public MemberRegistry MemberRegistry { get; }
        // LoanManager för att hantera lån av böcker
        public LoanManager LoanManager { get; }

        public Library()
        {
            BookCatalog = new BookCatalog();
            MemberRegistry = new MemberRegistry();
            LoanManager = new LoanManager();
        }

        // Metod för att visa statistik om biblioteket
        public void DisplayStatistics()
        {
            ConsoleHelper.WriteGreen("=== Biblioteksstatistik ===");
            Console.WriteLine($"Antal böcker: {BookCatalog.Books.Count}");
            Console.WriteLine($"Tillgängliga böcker: {BookCatalog.GetAvailableBooks().Count}");
            Console.WriteLine($"Antal medlemmar: {MemberRegistry.Members.Count}");
            Console.WriteLine($"Aktiva lån: {LoanManager.GetActiveLoans().Count}");
            Console.WriteLine($"Försenade lån: {LoanManager.GetOverdueLoans().Count}");

            // Mest aktiva låntagaren
            var mostActive = LoanManager.GetMostActiveBorrower();
            if (mostActive != null)
            {
                Console.WriteLine($"Mest aktiva låntagare: {mostActive.Name}");
            }
            else
            {
                Console.WriteLine("Mest aktiva låntagare: Inga lån registrerade");
            }
        }
    }
}
