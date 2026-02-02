using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bibliotekssystem.Helpers
{
    public static class InputHelper
    {
        //Metoder för att hantera olika typer av inmatningar från användaren
        public static int GetInt()
        {
            while (true)
            {
                var input = Console.ReadLine();

                if (int.TryParse(input, out int result))
                {
                    return result;
                }

                ConsoleHelper.WriteRed("Ogiltig inmatning. Vänligen ange ett heltal.");
            }
        }

        public static string GetString()
        {
            while (true)
            {
                var input = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(input))
                {
                    return input;
                }

                ConsoleHelper.WriteRed("Ogiltig inmatning. Vänligen ange en giltig text.");
            }
        }

        public static DateTime GetDateTime()
        {
            while (true)
            {
                var input = Console.ReadLine();

                if (DateTime.TryParse(input, out DateTime result))
                {
                    return result;
                }

                ConsoleHelper.WriteRed("Ogiltig inmatning. Vänligen ange ett giltigt datum (t.ex. 2024-12-31).");
            }
        }

        
    }
}
