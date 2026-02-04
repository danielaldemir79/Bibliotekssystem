
using Bibliotekssystem.Helpers;

namespace Bibliotekssystem
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Library library = new Library();
            RunMainMenu(library);
        }

        static void RunMainMenu(Library library)
        {
            bool running = true;

            while (running)
            {
                MenuHandler.ShowMenu();
                Console.Write("\nVälj: ");
                int choice = InputHelper.GetInt();

                switch (choice)
                {
                    case 1: MenuHandler.ShowAllBooks(library); break;
                    case 2: MenuHandler.SearchBook(library); break;
                    case 3: MenuHandler.BorrowBook(library); break;
                    case 4: MenuHandler.ReturnBook(library); break;
                    case 5: MenuHandler.ShowMembers(library); break;
                    case 6: MenuHandler.ShowStatistics(library); break;
                    case 7: MenuHandler.AddBook(library); break;
                    case 8: MenuHandler.RemoveBook(library); break;
                    case 9: MenuHandler.AddMember(library); break;
                    case 10: MenuHandler.RemoveMember(library); break;
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
    }
}