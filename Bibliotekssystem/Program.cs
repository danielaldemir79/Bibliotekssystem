
using Bibliotekssystem.Helpers;

namespace Bibliotekssystem
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Library library = new Library(); //Skapar en instans av biblioteket

            RunMainMenu(library); //Kör huvudmenyn


        }

        public static void Menu() //Huvudmeny
        {
            
            ConsoleHelper.WriteHeader("Bibliotekssystem");
            Console.WriteLine("1. Visa alla böcker");
            Console.WriteLine("2. Sök bok");
            Console.WriteLine("3. Låna bok");
            Console.WriteLine("4. Returnera bok");
            Console.WriteLine("5. Visa medlemmar");
            Console.WriteLine("6. Statstik");
            
            ConsoleHelper.WriteHeader("Administration");
            Console.WriteLine("7. Lägg till bok");
            Console.WriteLine("8. Ta bort bok");
            Console.WriteLine("9. Lägg till medlem");
            Console.WriteLine("10.Ta bort medlem");
            Console.WriteLine();
            ConsoleHelper.WriteYellow("0. Avsluta");

        }


        static void RunMainMenu(Library library) //Huvudmeny logik
        {
            bool running = true;

            while (running)
            {
                Menu();
                Console.Write("\nVälj: ");
                int choice = InputHelper.GetInt();

                switch (choice)
                {
                    case 1:
                        ShowAllBooks(library);
                        break;
                    case 2:
                        SearchBook(library);
                        break;
                    case 3:
                        BorrowBook(library);
                        break;
                    case 4:
                        ReturnBook(library);
                        break;
                    case 5:
                        ShowMembers(library);
                        break;
                    case 6:
                        ShowStatistics(library);
                        break;
                    case 7:
                        AddBook(library);
                        break;
                    case 8:
                        RemoveBook(library);
                        break;
                    case 9:
                        AddMember(library);
                        break;
                    case 10:
                        RemoveMember(library);
                        break;
                    case 0:
                        running = false;
                        Console.Clear();
                        ConsoleHelper.WriteGreen("Avslutar programmet. Hej då!");
                        break;
                    default:
                        Console.Clear();
                        ConsoleHelper.WriteRed("Ogiltigt val. Försök igen.");
                        break;
                }
            }
        }



        // Visa alla böcker i biblioteket
        static void ShowAllBooks(Library library) 
        { 
            Console.Clear();
            ConsoleHelper.WriteGreen("Alla böcker i biblioteket");
            var books = library.BookCatalog.Books;

            if (books.Count == 0)
            {
                ConsoleHelper.WriteYellow("Inga böcker finns i biblioteket.");
                return;
            }

            foreach (var book in books)
            {
                Console.WriteLine($"{book.Title} av {book.Author} ({book.PublishedYear}) - ISBN: {book.ISBN} - {(book.IsAvailable ? "Tillgänglig" : "Utlånad")}");
            }
        }


        static void SearchBook(Library library) 
        { 
            Console.Clear();
            ConsoleHelper.WriteGreen("Sök bok");

            Console.Write("Ange sökord (titel, författare, ISBN): ");
            string searchTerm = InputHelper.GetString();

            var results = library.BookCatalog.SearchBooks(searchTerm);

            if (results.Count == 0)
            {
                ConsoleHelper.WriteYellow("Inga böcker matchade sökningen.");
                return;
            }   

            ConsoleHelper.WriteCyan($"Hittade {results.Count} böck(er):");
            
            foreach (var book in results)
            {
                Console.WriteLine($"{book.Title} av {book.Author} ({book.PublishedYear}) - ISBN: {book.ISBN} - {(book.IsAvailable ? "Tillgänglig" : "Utlånad")}");
            }

          
        }



        // Låna en bok från biblioteket
        static void BorrowBook(Library library) 
        { 
            Console.Clear();
            ConsoleHelper.WriteGreen("Låna bok");

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

            Console.Write("Ange ditt medlems-ID: ");
            string memberId = InputHelper.GetString();
            var member = library.MemberRegistry.FindById(memberId);

            if (member == null)
            {
                ConsoleHelper.WriteRed("Medlemmen med angivet medlems-ID hittades inte.");
                return;
            }

            var loan = library.LoanManager.CreateLoan(book, member);
            
            if (loan != null)
            {
                ConsoleHelper.WriteGreen($"Boken '{book.Title}' har lånats ut till medlemmen '{member.Name}'.");
                ConsoleHelper.WriteCyan($"Återlämningsdatum: {loan.DueDate.ToShortDateString()}");
            }
            else
            {
                ConsoleHelper.WriteRed("Kunde inte låna ut boken. Försök igen senare.");
            }

        }


        // Returnera en bok till biblioteket
        static void ReturnBook(Library library) 
        {
            Console.Clear();
            ConsoleHelper.WriteGreen("Returnera bok");

            Console.Write("Ange medlems-ID: ");
            string memberId = InputHelper.GetString();

            // Hitta medlemmen
            var member = library.MemberRegistry.FindById(memberId);
            
            if (member == null) // Om medlemmen inte hittas så avbryt
            {
                ConsoleHelper.WriteRed("Medlemmen hittades inte.");
                return;
            }

            // Hämta medlemmens aktiva lån
            var memberLoans = library.LoanManager.GetLoansByMember(member)
                .Where(l => !l.IsReturned).ToList();

            // Om inga aktiva lån finns så avbryt
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

            // Låt användaren välja
            Console.Write("\nVälj lån att returnera (nummer): ");
            int choice = InputHelper.GetInt();

            if (choice < 1 || choice > memberLoans.Count)
            {
                ConsoleHelper.WriteRed("Ogiltigt val.");
                return;
            }

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



        static void ShowMembers(Library library) 
        { 
            Console.Clear();
            ConsoleHelper.WriteGreen("Bibliotekets medlemmar");

            var members = library.MemberRegistry.Members;

            if (members.Count == 0)
            {
                ConsoleHelper.WriteYellow("Inga medlemmar finns i biblioteket.");
                return;
            }

            foreach (var member in members)
            {
                Console.WriteLine($"{member.Name} - Medlems-ID: {member.MemberId} - E-post: {member.Email} - Medlem sedan: {member.MemberSince.ToShortDateString()}");
            }

        }

        

        static void ShowStatistics(Library library) 
        { 
            Console.Clear();
            library.DisplayStatistics();

        }



        // Administrationsmetoder

       
        // Lägg till en bok i biblioteket
        static void AddBook(Library library) 
        {
            Console.Clear();
            ConsoleHelper.WriteGreen("Lägg till bok");

            Console.Write("Ange ISBN: ");
            string isbn = InputHelper.GetString();

            // Kolla om ISBN redan finns isåfall avbryt
            if (library.BookCatalog.FindByISBN(isbn) != null)
            {
                ConsoleHelper.WriteRed("En bok med detta ISBN finns redan.");
                return;
            }


            Console.Write("Ange titel: ");
            string title = InputHelper.GetString();

            Console.Write("Ange författare: ");
            string author = InputHelper.GetString();

            Console.Write("Ange utgivningsår: ");
            int publishedYear = InputHelper.GetInt();

            //Lägger till boken i bibliotekets katalog
            var newBook = new Models.Book(isbn, title, author, publishedYear);
            library.BookCatalog.AddBook(newBook);

            ConsoleHelper.WriteGreen($"Boken '{title}' har lagts till i biblioteket.");
        }
       
        
        static void RemoveBook(Library library) 
        { 
            Console.Clear();
            ConsoleHelper.WriteGreen("Ta bort bok");

            Console.Write("Ange ISBN på boken du vill ta bort: ");
            string isbn = InputHelper.GetString();

            // Hitta boken i katalogen
            var book = library.BookCatalog.FindByISBN(isbn);
            if (book == null) // Om boken inte hittas så avbryt
            {
                ConsoleHelper.WriteRed("Boken med angivet ISBN hittades inte.");
                return;
            }

            if (!book.IsAvailable) // Om boken är utlånad så avbryt
            {
                ConsoleHelper.WriteRed("Boken är för närvarande utlånad och kan inte tas bort.");
                return;
            }

            // Ta bort boken från bibliotekets katalog
            library.BookCatalog.RemoveBook(book);
            ConsoleHelper.WriteGreen($"Boken '{book.Title}' har tagits bort från biblioteket.");

        }

        
        
        // Lägg till en medlem i biblioteket
        static void AddMember(Library library) 
        { 
            Console.Clear();
            ConsoleHelper.WriteGreen("Lägg till medlem");

            Console.Write("Ange medlems-ID: ");
            string memberId = InputHelper.GetString();

            // Kolla om medlems-ID redan finns isåfall avbryt
            if (library.MemberRegistry.FindById(memberId) != null)
            {
                ConsoleHelper.WriteRed("En medlem med detta medlems-ID finns redan.");
                return;
            }

            Console.Write("Ange namn: ");
            string name = InputHelper.GetString();

            Console.Write("Ange e-post: ");
            string email = InputHelper.GetString();

            Console.Write("Ange medlemsdatum (YYYY-MM-DD): ");
            DateTime memberSince = InputHelper.GetDateTime();

            //Lägger till medlemmen i bibliotekets medlemsregister
            var newMember = new Models.Member(memberId, name, email, memberSince);
            library.MemberRegistry.AddMember(newMember);

            ConsoleHelper.WriteGreen($"Medlemmen '{name}' har lagts till i biblioteket.");
        }
        
        
        static void RemoveMember(Library library) 
        {
            Console.Clear();
            ConsoleHelper.WriteGreen("Ta bort medlem");

            Console.Write("Ange medlems-ID på medlemmen du vill ta bort: ");
            string memberId = InputHelper.GetString();

            // Hitta medlemmen i registret
            var member = library.MemberRegistry.FindById(memberId);

            if (member == null) // Om medlemmen inte hittas så avbryt
            {
                ConsoleHelper.WriteRed("Medlemmen med angivet medlems-ID hittades inte.");
                return;
            }

            var activeLoans = library.LoanManager.GetLoansByMember(member)
                .Where(l => !l.IsReturned).ToList();

            if (activeLoans.Count > 0) // Om medlemmen har aktiva lån så avbryt
            {
                ConsoleHelper.WriteRed("Medlemmen har aktiva lån och kan inte tas bort.");
                return;
            }

            // Ta bort medlemmen från bibliotekets medlemsregister
            library.MemberRegistry.RemoveMember(member);
            ConsoleHelper.WriteGreen($"Medlemmen '{member.Name}' har tagits bort från biblioteket.");
        }
    }
}
