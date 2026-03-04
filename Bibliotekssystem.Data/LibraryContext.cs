using Microsoft.EntityFrameworkCore;
using Bibliotekssystem.Core.Models;

namespace Bibliotekssystem.Data
{
    public class LibraryContext : DbContext
    {
        // DbSet = en tabell i databasen. Varje DbSet blir en tabell.
        public DbSet<Book> Books { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Loan> Loans { get; set; }

        // Konstruktor som tar emot options - gör det möjligt att konfigurera
        // databasen utifrån (t.ex. SQLite i produktion, InMemory i tester)
        public LibraryContext(DbContextOptions<LibraryContext> options)
            : base(options)
        {
        }

        // Här konfigurerar vi relationer och regler för databasen
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // === BOOK-tabellen ===
            modelBuilder.Entity<Book>(entity =>
            {
                // ISBN måste vara unikt i databasen
                entity.HasIndex(b => b.ISBN).IsUnique();

            });


            // === MEMBER-tabellen ===
            modelBuilder.Entity<Member>(entity =>
            {
                // MemberId måste vara unikt
                entity.HasIndex(m => m.MemberId).IsUnique();

                // Email måste vara unikt
                entity.HasIndex(m => m.Email).IsUnique();
            });

            // === LOAN-tabellen (relationer) ===
            modelBuilder.Entity<Loan>(entity =>
            {
                // Ett lån tillhör EN bok, en bok kan ha MÅNGA lån
                entity.HasOne(l => l.Book)
                      .WithMany(b => b.Loans)
                      .HasForeignKey(l => l.BookId)
                      .OnDelete(DeleteBehavior.Restrict);

                // Ett lån tillhör EN medlem, en medlem kan ha MÅNGA lån
                entity.HasOne(l => l.Member)
                      .WithMany(m => m.Loans)
                      .HasForeignKey(l => l.MemberId)
                      .OnDelete(DeleteBehavior.Restrict);

                // Beräknade properties ska INTE sparas i databasen
                entity.Ignore(l => l.IsReturned);
                entity.Ignore(l => l.IsOverdue);
            });

            // === SEED DATA (startdata i databasen) ===
            modelBuilder.Entity<Book>().HasData(
                new Book { Id = 1, ISBN = "978-91-0-123456-7", Title = "Sagan om ringen", Author = "J.R.R. Tolkien", PublishedYear = 1954, IsAvailable = false },
                new Book { Id = 2, ISBN = "978-91-0-654321-8", Title = "Hobbiten", Author = "J.R.R. Tolkien", PublishedYear = 1937, IsAvailable = true },
                new Book { Id = 3, ISBN = "978-91-0-111111-9", Title = "1984", Author = "George Orwell", PublishedYear = 1949, IsAvailable = false },
                new Book { Id = 4, ISBN = "978-91-0-222222-0", Title = "Djurfarmen", Author = "George Orwell", PublishedYear = 1945, IsAvailable = true },
                new Book { Id = 5, ISBN = "978-91-0-333333-1", Title = "Pippi Långstrump", Author = "Astrid Lindgren", PublishedYear = 1945, IsAvailable = true },
                new Book { Id = 6, ISBN = "978-91-0-444444-2", Title = "Bröderna Lejonhjärta", Author = "Astrid Lindgren", PublishedYear = 1973, IsAvailable = true },
                new Book { Id = 7, ISBN = "978-91-0-555555-3", Title = "Röda rummet", Author = "August Strindberg", PublishedYear = 1879, IsAvailable = true },
                new Book { Id = 8, ISBN = "978-91-0-666666-4", Title = "Doktor Glas", Author = "Hjalmar Söderberg", PublishedYear = 1905, IsAvailable = false },
                new Book { Id = 9, ISBN = "978-91-0-777777-5", Title = "Herr Arnes penningar", Author = "Selma Lagerlöf", PublishedYear = 1904, IsAvailable = true },
                new Book { Id = 10, ISBN = "978-91-0-888888-6", Title = "Gösta Berlings saga", Author = "Selma Lagerlöf", PublishedYear = 1891, IsAvailable = true },
                new Book { Id = 11, ISBN = "978-91-0-999999-7", Title = "Kallocain", Author = "Karin Boye", PublishedYear = 1940, IsAvailable = true },
                new Book { Id = 12, ISBN = "978-91-0-101010-8", Title = "Nils Holgerssons underbara resa", Author = "Selma Lagerlöf", PublishedYear = 1907, IsAvailable = false },
                new Book { Id = 13, ISBN = "978-91-0-121212-9", Title = "Mio, min Mio", Author = "Astrid Lindgren", PublishedYear = 1954, IsAvailable = true },
                new Book { Id = 14, ISBN = "978-91-0-131313-0", Title = "Ronja Rövardotter", Author = "Astrid Lindgren", PublishedYear = 1981, IsAvailable = true },
                new Book { Id = 15, ISBN = "978-91-0-141414-1", Title = "Markurells i Wadköping", Author = "Hjalmar Bergman", PublishedYear = 1919, IsAvailable = true }
            );

            modelBuilder.Entity<Member>().HasData(
                new Member { Id = 1, MemberId = "M001", Name = "Anna Svensson", Email = "anna@example.com", MemberSince = new DateTime(2023, 1, 15) },
                new Member { Id = 2, MemberId = "M002", Name = "Erik Johansson", Email = "erik@example.com", MemberSince = new DateTime(2023, 6, 20) },
                new Member { Id = 3, MemberId = "M003", Name = "Maria Andersson", Email = "maria@example.com", MemberSince = new DateTime(2024, 2, 10) },
                new Member { Id = 4, MemberId = "M004", Name = "Karl Pettersson", Email = "karl@example.com", MemberSince = new DateTime(2024, 8, 5) },
                new Member { Id = 5, MemberId = "M005", Name = "Lisa Nilsson", Email = "lisa@example.com", MemberSince = new DateTime(2024, 1, 12) },
                new Member { Id = 6, MemberId = "M006", Name = "Oscar Lindqvist", Email = "oscar@example.com", MemberSince = new DateTime(2024, 3, 8) },
                new Member { Id = 7, MemberId = "M007", Name = "Emma Bergström", Email = "emma@example.com", MemberSince = new DateTime(2024, 5, 22) },
                new Member { Id = 8, MemberId = "M008", Name = "Gustav Eriksson", Email = "gustav@example.com", MemberSince = new DateTime(2024, 9, 1) }
            );

            // Startlån - några aktiva och några returnerade
            modelBuilder.Entity<Loan>().HasData(
                // Aktiva lån - 2 i tid, 2 försenade
                new Loan { Id = 1, BookId = 1, MemberId = 1, LoanDate = new DateTime(2025, 2, 25), DueDate = new DateTime(2025, 3, 27) },
                new Loan { Id = 2, BookId = 3, MemberId = 2, LoanDate = new DateTime(2025, 3, 1), DueDate = new DateTime(2025, 3, 31) },
                // Försenade lån - olika grader av försening
                new Loan { Id = 3, BookId = 8, MemberId = 5, LoanDate = new DateTime(2025, 2, 1), DueDate = new DateTime(2025, 3, 3) },
                new Loan { Id = 4, BookId = 12, MemberId = 3, LoanDate = new DateTime(2025, 1, 10), DueDate = new DateTime(2025, 2, 9) },
                // Returnerade lån
                new Loan { Id = 5, BookId = 2, MemberId = 1, LoanDate = new DateTime(2024, 12, 1), DueDate = new DateTime(2024, 12, 31), ReturnDate = new DateTime(2024, 12, 20) },
                new Loan { Id = 6, BookId = 5, MemberId = 4, LoanDate = new DateTime(2025, 1, 5), DueDate = new DateTime(2025, 2, 4), ReturnDate = new DateTime(2025, 1, 28) },
                new Loan { Id = 7, BookId = 7, MemberId = 6, LoanDate = new DateTime(2025, 1, 15), DueDate = new DateTime(2025, 2, 14), ReturnDate = new DateTime(2025, 2, 10) },
                new Loan { Id = 8, BookId = 4, MemberId = 7, LoanDate = new DateTime(2025, 2, 1), DueDate = new DateTime(2025, 3, 3), ReturnDate = new DateTime(2025, 2, 25) }
            );
        }
    }
}