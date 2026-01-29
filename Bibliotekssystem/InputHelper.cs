using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bibliotekssystem
{
    public class InputHelper
    {
        public int GetInt()
        {
            while (true)
            {
                var input = Console.ReadLine();

                if (int.TryParse(input, out int result))
                {
                    return result;
                }

                Console.WriteLine("Ogiltig inmatning. Vänligen ange ett heltal.");
            }
        }

        public string GetString()
        {
            while (true)
            {
                var input = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(input))
                {
                    return input;
                }

                Console.WriteLine("Ogiltig inmatning. Vänligen ange en giltig text.");
            }
        }

        public DateTime GetDateTime()
        {
            while (true)
            {
                var input = Console.ReadLine();

                if (DateTime.TryParse(input, out DateTime result))
                {
                    return result;
                }

                Console.WriteLine("Ogiltig inmatning. Vänligen ange ett giltigt datum (t.ex. 2024-12-31).");
            }
        }

        
    }
}
