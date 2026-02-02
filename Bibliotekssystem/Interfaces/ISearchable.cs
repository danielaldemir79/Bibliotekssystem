using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bibliotekssystem.Interfaces
{
    // Interface för sökbar funktionalitet i bibliotekssystemet.
    public interface ISearchable
    {
        // Metod för att avgöra om ett objekt matchar ett givet sökord.
        bool Matches(string searchTerm);
    }
}
