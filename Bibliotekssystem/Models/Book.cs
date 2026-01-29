namespace Bibliotekssystem.Models
{
    public class Book
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
            return $"ISBN: {ISBN}, Title: {Title}, Author: {Author}, Published Year: {PublishedYear}, Available: {IsAvailable}";
        }
    }
}
