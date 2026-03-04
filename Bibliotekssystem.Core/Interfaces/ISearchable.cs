namespace Bibliotekssystem.Core.Interfaces
{
    // Interface för sökbar funktionalitet i bibliotekssystemet.
    public interface ISearchable
    {
        // Metod för att avgöra om ett objekt matchar ett givet sökord.
        bool Matches(string searchTerm);
    }
}