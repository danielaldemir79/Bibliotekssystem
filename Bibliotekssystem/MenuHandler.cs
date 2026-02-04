using Bibliotekssystem.Models;

namespace Bibliotekssystem.Helpers
{
    // ============================================
    // MENUHANDLER - Hanterar alla menyval
    // ============================================
    public static class MenuHandler
    {
        // ------------------------------------------
        // HUVUDMENY
        // ------------------------------------------

        // Visar huvudmenyn med alla tillgängliga val
        public static void ShowMenu()
        {
            Console.WriteLine();
            ConsoleHelper.WriteYellow("===Bibliotekssystem===");
            Console.WriteLine("1. Visa alla böcker");
            Console.WriteLine("2. Sök bok");
            Console.WriteLine("3. Låna bok");
            Console.WriteLine("4. Returnera bok");

            ConsoleHelper.WriteHeader("Administration");
            Console.WriteLine("5. Visa medlemmar");
            Console.WriteLine("6. Statistik");
            Console.WriteLine("7. Lägg till bok");
            Console.WriteLine("8. Ta bort bok");
            Console.WriteLine("9. Lägg till medlem");
            Console.WriteLine("10. Ta bort medlem");
            Console.WriteLine();
            ConsoleHelper.WriteYellow("0. Avsluta");
        }


        // ------------------------------------------
        // BOKHANTERING
        // ------------------------------------------

        // Visar alla böcker med valfri sortering (titel, författare, år)
        public static void ShowAllBooks(Library library)
        {
            Console.Clear();
            ConsoleHelper.WriteGreen("Alla böcker i biblioteket");

            // Visa sorteringsalternativ
            Console.WriteLine("\nSortera efter:");
            Console.WriteLine("1. Titel");
            Console.WriteLine("2. Författare");
            Console.WriteLine("3. Utgivningsår");
            Console.Write("\nVälj: ");
            int sortChoice = InputHelper.GetInt();

            // Hämta böcker baserat på sorteringsval
            var books = sortChoice switch
            {
                1 => library.BookCatalog.GetBooksSortedByTitle(),
                2 => library.BookCatalog.GetBooksSortedByAuthor(),
                3 => library.BookCatalog.GetBooksSortedByPublishedYear(),
                _ => library.BookCatalog.Books.ToList()
            };

            // Kontrollera om biblioteket har böcker
            if (books.Count == 0)
            {
                ConsoleHelper.WriteYellow("Inga böcker finns i biblioteket.");
                return;
            }

            // Skriv ut varje bok
            Console.WriteLine();
            foreach (var book in books)
            {
                string status = book.IsAvailable ? "Tillgänglig" : "Utlånad";
                Console.WriteLine($"{book.Title} av {book.Author} ({book.PublishedYear}) - ISBN: {book.ISBN} - {status}");
            }
        }


        // Söker efter böcker baserat på titel, författare eller ISBN
        public static void SearchBook(Library library)
        {
            Console.Clear();
            ConsoleHelper.WriteGreen("Sök bok");

            // Hämta sökord från användaren
            Console.Write("Ange sökord (titel, författare, ISBN): ");
            string searchTerm = InputHelper.GetString();

            // Sök i bokkatalogen
            var results = library.BookCatalog.SearchBooks(searchTerm);

            // Kontrollera om sökningen gav resultat
            if (results.Count == 0)
            {
                ConsoleHelper.WriteYellow("Inga böcker matchade sökningen.");
                return;
            }

            // Visa sökresultat
            ConsoleHelper.WriteCyan($"Hittade {results.Count} böck(er):");
            foreach (var book in results)
            {
                string status = book.IsAvailable ? "Tillgänglig" : "Utlånad";
                Console.WriteLine($"{book.Title} av {book.Author} ({book.PublishedYear}) - ISBN: {book.ISBN} - {status}");
            }
        }


        // Lägger till en ny bok i biblioteket
        public static void AddBook(Library library)
        {
            Console.Clear();
            ConsoleHelper.WriteGreen("Lägg till bok");

            // Hämta ISBN och kontrollera att det är unikt
            Console.Write("Ange ISBN: ");
            string isbn = InputHelper.GetString();

            if (library.BookCatalog.FindByISBN(isbn) != null)
            {
                ConsoleHelper.WriteRed("En bok med detta ISBN finns redan.");
                return;
            }

            // Hämta resterande bokinformation
            Console.Write("Ange titel: ");
            string title = InputHelper.GetString();

            Console.Write("Ange författare: ");
            string author = InputHelper.GetString();

            Console.Write("Ange utgivningsår: ");
            int publishedYear = InputHelper.GetInt();

            // Skapa och lägg till boken
            var newBook = new Book(isbn, title, author, publishedYear);
            library.BookCatalog.AddBook(newBook);

            ConsoleHelper.WriteGreen($"Boken '{title}' har lagts till i biblioteket.");
        }


        // Tar bort en bok från biblioteket (endast om den inte är utlånad)
        public static void RemoveBook(Library library)
        {
            Console.Clear();
            ConsoleHelper.WriteGreen("Ta bort bok");

            // Hämta ISBN från användaren
            Console.Write("Ange ISBN på boken du vill ta bort: ");
            string isbn = InputHelper.GetString();

            // Försök hitta boken
            var book = library.BookCatalog.FindByISBN(isbn);
            if (book == null)
            {
                ConsoleHelper.WriteRed("Boken med angivet ISBN hittades inte.");
                return;
            }

            // Kontrollera att boken inte är utlånad
            if (!book.IsAvailable)
            {
                ConsoleHelper.WriteRed("Boken är för närvarande utlånad och kan inte tas bort.");
                return;
            }

            // Ta bort boken
            library.BookCatalog.RemoveBook(book);
            ConsoleHelper.WriteGreen($"Boken '{book.Title}' har tagits bort från biblioteket.");
        }


        // ------------------------------------------
        // LÅNEHANTERING
        // ------------------------------------------

        // Lånar ut en bok till en medlem
        public static void BorrowBook(Library library)
        {
            Console.Clear();
            ConsoleHelper.WriteGreen("Låna bok");

            // Hämta och validera boken
            Console.Write("Ange ISBN på boken du vill låna: ");
            string isbn = InputHelper.GetString();

            var book = library.BookCatalog.FindByISBN(isbn);
            if (book == null)
            {
                ConsoleHelper.WriteRed("Boken med angivet ISBN hittades inte.");
                return;
            }

            if (!book.IsAvailable)
            {
                ConsoleHelper.WriteRed("Boken är för närvarande utlånad.");
                return;
            }

            // Hämta och validera medlemmen
            Console.Write("Ange ditt medlems-ID: ");
            string memberId = InputHelper.GetString();
            var member = library.MemberRegistry.FindById(memberId);

            if (member == null)
            {
                ConsoleHelper.WriteRed("Medlemmen med angivet medlems-ID hittades inte.");
                return;
            }

            // Skapa lånet
            var loan = library.LoanManager.CreateLoan(book, member);

            if (loan != null)
            {
                ConsoleHelper.WriteGreen($"Boken '{book.Title}' har lånats ut till '{member.Name}'.");
                ConsoleHelper.WriteCyan($"Återlämningsdatum: {loan.DueDate.ToShortDateString()}");
            }
            else
            {
                ConsoleHelper.WriteRed("Kunde inte låna ut boken. Försök igen senare.");
            }
        }


        // Returnerar en bok som en medlem har lånat
        public static void ReturnBook(Library library)
        {
            Console.Clear();
            ConsoleHelper.WriteGreen("Returnera bok");

            // Hämta och validera medlemmen
            Console.Write("Ange medlems-ID: ");
            string memberId = InputHelper.GetString();

            var member = library.MemberRegistry.FindById(memberId);
            if (member == null)
            {
                ConsoleHelper.WriteRed("Medlemmen hittades inte.");
                return;
            }

            // Hämta medlemmens aktiva lån
            var memberLoans = library.LoanManager.GetLoansByMember(member)
                .Where(l => !l.IsReturned).ToList();

            if (memberLoans.Count == 0)
            {
                ConsoleHelper.WriteYellow("Medlemmen har inga aktiva lån.");
                return;
            }

            // Visa aktiva lån med nummer
            ConsoleHelper.WriteCyan("Aktiva lån:");
            for (int i = 0; i < memberLoans.Count; i++)
            {
                var loan = memberLoans[i];
                Console.WriteLine($"{i + 1}. {loan.Book.Title} - Förfaller: {loan.DueDate.ToShortDateString()}");
            }

            // Låt användaren välja lån att returnera
            Console.Write("\nVälj lån att returnera (nummer): ");
            int choice = InputHelper.GetInt();

            if (choice < 1 || choice > memberLoans.Count)
            {
                ConsoleHelper.WriteRed("Ogiltigt val.");
                return;
            }

            // Returnera valt lån
            var loanToReturn = memberLoans[choice - 1];
            bool success = library.LoanManager.ReturnLoan(loanToReturn);

            if (success)
            {
                ConsoleHelper.WriteGreen($"Boken '{loanToReturn.Book.Title}' har returnerats.");
            }
            else
            {
                ConsoleHelper.WriteRed("Kunde inte returnera boken. Försök igen.");
            }
        }


        // ------------------------------------------
        // MEDLEMSHANTERING
        // ------------------------------------------

        // Visar alla medlemmar i biblioteket
        public static void ShowMembers(Library library)
        {
            Console.Clear();
            ConsoleHelper.WriteGreen("Bibliotekets medlemmar");

            var members = library.MemberRegistry.Members;

            // Kontrollera om det finns medlemmar
            if (members.Count == 0)
            {
                ConsoleHelper.WriteYellow("Inga medlemmar finns i biblioteket.");
                return;
            }

            // Visa varje medlem
            foreach (var member in members)
            {
                Console.WriteLine($"{member.Name} - ID: {member.MemberId} - E-post: {member.Email} - Medlem sedan: {member.MemberSince.ToShortDateString()}");
            }
        }


        // Lägger till en ny medlem i biblioteket
        public static void AddMember(Library library)
        {
            Console.Clear();
            ConsoleHelper.WriteGreen("Lägg till medlem");

            // Hämta medlems-ID och kontrollera att det är unikt
            Console.Write("Ange medlems-ID: ");
            string memberId = InputHelper.GetString();

            if (library.MemberRegistry.FindById(memberId) != null)
            {
                ConsoleHelper.WriteRed("En medlem med detta medlems-ID finns redan.");
                return;
            }

            // Hämta resterande medlemsinformation
            Console.Write("Ange namn: ");
            string name = InputHelper.GetString();

            Console.Write("Ange e-post: ");
            string email = InputHelper.GetString();

            Console.Write("Ange medlemsdatum (YYYY-MM-DD): ");
            DateTime memberSince = InputHelper.GetDateTime();

            // Skapa och lägg till medlemmen
            var newMember = new Member(memberId, name, email, memberSince);
            library.MemberRegistry.AddMember(newMember);

            ConsoleHelper.WriteGreen($"Medlemmen '{name}' har lagts till i biblioteket.");
        }


        // Tar bort en medlem (endast om den inte har aktiva lån)
        public static void RemoveMember(Library library)
        {
            Console.Clear();
            ConsoleHelper.WriteGreen("Ta bort medlem");

            // Hämta medlems-ID från användaren
            Console.Write("Ange medlems-ID på medlemmen du vill ta bort: ");
            string memberId = InputHelper.GetString();

            // Försök hitta medlemmen
            var member = library.MemberRegistry.FindById(memberId);
            if (member == null)
            {
                ConsoleHelper.WriteRed("Medlemmen med angivet medlems-ID hittades inte.");
                return;
            }

            // Kontrollera att medlemmen inte har aktiva lån
            var activeLoans = library.LoanManager.GetLoansByMember(member)
                .Where(l => !l.IsReturned).ToList();

            if (activeLoans.Count > 0)
            {
                ConsoleHelper.WriteRed("Medlemmen har aktiva lån och kan inte tas bort.");
                return;
            }

            // Ta bort medlemmen
            library.MemberRegistry.RemoveMember(member);
            ConsoleHelper.WriteGreen($"Medlemmen '{member.Name}' har tagits bort från biblioteket.");
        }


        // ------------------------------------------
        // STATISTIK
        // ------------------------------------------

        // Visar statistik över biblioteket
        public static void ShowStatistics(Library library)
        {
            Console.Clear();
            library.DisplayStatistics();
        }
    }
}