using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bibliotekssystem
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

        public void BorrowBook(Book book)
        {
            if (book.IsAvailable)
            {
                _borrowedBooks.Add(book);
                book.IsAvailable = false;
            }
        }

        public void ReturnBook(Book book)
        {
            bool wasRemoved = _borrowedBooks.Remove(book);
            if (wasRemoved)
            {
                book.IsAvailable = true;
            }
        }
    }
}
