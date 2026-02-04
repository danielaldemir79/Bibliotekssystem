namespace Bibliotekssystem.Models
{
    public class Member : Interfaces.ISearchable
    {
 
        public string MemberId { get; }
        public string Name { get; set; } 
        public string Email { get; set; }
        public DateTime MemberSince { get; set; }
        private List<Book> _borrowedBooks = new();
        public IReadOnlyList<Book> BorrowedBooks => _borrowedBooks;

        public Member(string memberId, string name, string email, DateTime memberSince)
        {
            MemberId = memberId;
            Name = name;
            Email = email;
            MemberSince = memberSince;
        }

        public string GetInfo()
        {
            // Returnerar en sträng med all information om medlemmen
            return $"Member ID: {MemberId}, Name: {Name}, Email: {Email}, Member Since: {MemberSince.ToShortDateString()}, Borrowed Books Count: {BorrowedBooks.Count}";
        }

        // Interna metoder som bara LoanManager använder
        public void AddBorrowedBook(Book book) => _borrowedBooks.Add(book);
        public void RemoveBorrowedBook(Book book) => _borrowedBooks.Remove(book);

        public bool Matches(string searchTerm)
        {
            // Gör sökordet till gemener för att möjliggöra case-insensitive sökning
            searchTerm = searchTerm.ToLower();
            
            // Kontrollera om sökordet matchar någon av medlemmens egenskaper
            //Returnerar true om någon egenskap matchar sökordet
            return MemberId.ToLower().Contains(searchTerm) ||
                   Name.ToLower().Contains(searchTerm) ||
                   Email.ToLower().Contains(searchTerm) ||
                   MemberSince.ToString("d").Contains(searchTerm);
        }
    }
}
