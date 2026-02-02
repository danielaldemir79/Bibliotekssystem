namespace Bibliotekssystem.Models
{
    public class Book : Interfaces.ISearchable
    {

        public string ISBN { get; }
        public string Title { get; set; }
        public string Author { get; set; }
        public int PublishedYear { get; set; }
        public bool IsAvailable { get; set; }

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
            // Returnerar en sträng med all information om boken
            return $"ISBN: {ISBN}, Title: {Title}, Author: {Author}, Published Year: {PublishedYear}, Available: {IsAvailable}";
        }

        public bool Matches(string searchTerm)
        {
            // Gör sökordet till gemener för att möjliggöra case-insensitive sökning
            searchTerm = searchTerm.ToLower();

            // Kontrollera om sökordet matchar någon av bokens egenskaper
            //Returnerar true om någon egenskap matchar sökordet
            return ISBN.ToLower().Contains(searchTerm) ||
                   Title.ToLower().Contains(searchTerm) ||
                   Author.ToLower().Contains(searchTerm) ||
                   PublishedYear.ToString().Contains(searchTerm);
        }
    }
}
