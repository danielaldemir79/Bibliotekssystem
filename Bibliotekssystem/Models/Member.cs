namespace Bibliotekssystem.Models
{
    public class Member
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
            return $"Member ID: {MemberId}, Name: {Name}, Email: {Email}, Member Since: {MemberSince.ToShortDateString()}, Borrowed Books Count: {BorrowedBooks.Count}";
        }

        // Interna metoder som bara LoanManager använder
        internal void AddBorrowedBook(Book book) => _borrowedBooks.Add(book);
        internal void RemoveBorrowedBook(Book book) => _borrowedBooks.Remove(book);
    }
}
