
using System.ComponentModel.DataAnnotations;
using Bibliotekssystem.Core.Interfaces;

namespace Bibliotekssystem.Core.Models
{
    public class Book : ISearchable
    {
        // Primärnyckel - EF Core behöver en unik identifierare per rad i tabellen
        public int Id { get; set; }

        // [Required] = fältet får inte vara tomt (används för formulärvalidering i Blazor)
        // [StringLength] = max antal tecken (bra för validering + databaskolumnstorlek)
        [Required(ErrorMessage = "ISBN är obligatoriskt")]
        [StringLength(20, ErrorMessage = "ISBN får max vara 20 tecken")]
        public string ISBN { get; set; } = string.Empty;

        [Required(ErrorMessage = "Titel är obligatoriskt")]
        [StringLength(200, ErrorMessage = "Titel får max vara 200 tecken")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Författare är obligatoriskt")]
        [StringLength(100, ErrorMessage = "Författare får max vara 100 tecken")]
        public string Author { get; set; } = string.Empty;

        [Range(1000, 2100, ErrorMessage = "Utgivningsår måste vara mellan 1000 och 2100")]
        public int PublishedYear { get; set; }

        public bool IsAvailable { get; set; } = true;

        // Navigation property - en bok kan ha många lån (EF Core skapar relationen automatiskt)
        public ICollection<Loan> Loans { get; set; } = new List<Loan>();

        // Parameterlös konstruktor - krävs av EF Core för att skapa objekt från databasen
        public Book() { }

        // Konstruktor med parametrar - behåller vi för bakåtkompatibilitet med Del 1
        public Book(string isbn, string title, string author, int publishedYear, bool isAvailable = true)
        {
            ISBN = isbn;
            Title = title;
            Author = author;
            PublishedYear = publishedYear;
            IsAvailable = isAvailable;
        }

        public string GetInfo()
        {
            return $"ISBN: {ISBN}, Title: {Title}, Author: {Author}, Published Year: {PublishedYear}, Available: {IsAvailable}";
        }

        public bool Matches(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();
            return ISBN.ToLower().Contains(searchTerm) ||
                   Title.ToLower().Contains(searchTerm) ||
                   Author.ToLower().Contains(searchTerm) ||
                   PublishedYear.ToString().Contains(searchTerm);
        }
    }
}
