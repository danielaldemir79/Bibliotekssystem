
namespace Bibliotekssystem
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Library library = new Library();

            RunMainMenu(library);


        }

        public static void Menu()
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


        static void RunMainMenu(Library library)
        {
            var input = new InputHelper();
            bool running = true;

            while (running)
            {
                Menu();
                Console.Write("\nVälj: ");
                int choice = input.GetInt();

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


        // Platshållare för metoderna - implementera en i taget
        static void ShowAllBooks(Library library) { }
        static void SearchBook(Library library) { }
        static void BorrowBook(Library library) { }
        static void ReturnBook(Library library) { }
        static void ShowMembers(Library library) { }
        static void ShowStatistics(Library library) { }
        static void AddBook(Library library) { }
        static void RemoveBook(Library library) { }
        static void AddMember(Library library) { }
        static void RemoveMember(Library library) { }
    }
}
