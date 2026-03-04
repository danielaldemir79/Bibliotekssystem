using Microsoft.EntityFrameworkCore;
using Bibliotekssystem.Core.Models;
using Bibliotekssystem.Data;
using Bibliotekssystem.Data.Repositories;

namespace Biblioteksystem.Tests.Repositories
{
    public class LoanRepositoryTests
    {
        // Skapar en ny InMemory-databas för varje test så de inte påverkar varandra
        private LibraryContext CreateContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<LibraryContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            return new LibraryContext(options);
        }

        // Hjälpmetod - lägger in en bok och en medlem som vi kan använda i testerna
        private async Task<(Book book, Member member)> SeedBookAndMember(LibraryContext context)
        {
            var book = new Book { ISBN = "123", Title = "Testbok", Author = "Författare", PublishedYear = 2024 };
            var member = new Member { MemberId = "M001", Name = "Anna", Email = "anna@test.com" };
            context.Books.Add(book);
            context.Members.Add(member);
            await context.SaveChangesAsync();
            return (book, member);
        }

        [Fact]
        public async Task CreateLoanAsync_ShouldCreateLoanAndMarkBookUnavailable()
        {
            using var context = CreateContext("CreateLoan_Test");
            var (book, member) = await SeedBookAndMember(context);
            var repository = new LoanRepository(context);

            var loan = await repository.CreateLoanAsync(book.Id, member.Id);

            Assert.NotNull(loan);
            Assert.Equal(book.Id, loan.BookId);
            Assert.Equal(member.Id, loan.MemberId);

            var updatedBook = await context.Books.FindAsync(book.Id);
            Assert.False(updatedBook!.IsAvailable);
        }

        [Fact]
        public async Task CreateLoanAsync_UnavailableBook_ShouldThrowException()
        {
            using var context = CreateContext("CreateLoanUnavailable_Test");
            var (book, member) = await SeedBookAndMember(context);

            book.IsAvailable = false;
            await context.SaveChangesAsync();

            var repository = new LoanRepository(context);

            await Assert.ThrowsAsync<InvalidOperationException>(
                () => repository.CreateLoanAsync(book.Id, member.Id));
        }

        [Fact]
        public async Task ReturnLoanAsync_ShouldSetReturnDateAndMarkBookAvailable()
        {
            using var context = CreateContext("ReturnLoan_Test");
            var (book, member) = await SeedBookAndMember(context);
            var repository = new LoanRepository(context);

            var loan = await repository.CreateLoanAsync(book.Id, member.Id);
            var result = await repository.ReturnLoanAsync(loan.Id);

            Assert.True(result);

            var returnedLoan = await context.Loans.FindAsync(loan.Id);
            Assert.NotNull(returnedLoan!.ReturnDate);

            var updatedBook = await context.Books.FindAsync(book.Id);
            Assert.True(updatedBook!.IsAvailable);
        }

        [Fact]
        public async Task GetActiveLoansAsync_ShouldOnlyReturnNonReturnedLoans()
        {
            using var context = CreateContext("GetActiveLoans_Test");
            var (book, member) = await SeedBookAndMember(context);

            var book2 = new Book { ISBN = "456", Title = "Bok 2", Author = "B", PublishedYear = 2023 };
            context.Books.Add(book2);
            await context.SaveChangesAsync();

            context.Loans.AddRange(
                new Loan { BookId = book.Id, MemberId = member.Id, LoanDate = DateTime.Now, DueDate = DateTime.Now.AddDays(30), ReturnDate = null },
                new Loan { BookId = book2.Id, MemberId = member.Id, LoanDate = DateTime.Now, DueDate = DateTime.Now.AddDays(30), ReturnDate = DateTime.Now }
            );
            await context.SaveChangesAsync();

            var repository = new LoanRepository(context);
            var activeLoans = await repository.GetActiveLoansAsync();

            Assert.Single(activeLoans);
        }

        [Fact]
        public async Task GetOverdueLoansAsync_ShouldReturnOnlyOverdueLoans()
        {
            using var context = CreateContext("GetOverdueLoans_Test");
            var (book, member) = await SeedBookAndMember(context);

            var book2 = new Book { ISBN = "456", Title = "Bok 2", Author = "B", PublishedYear = 2023 };
            context.Books.Add(book2);
            await context.SaveChangesAsync();

            context.Loans.AddRange(
                new Loan { BookId = book.Id, MemberId = member.Id, LoanDate = DateTime.Now.AddDays(-60), DueDate = DateTime.Now.AddDays(-30), ReturnDate = null },
                new Loan { BookId = book2.Id, MemberId = member.Id, LoanDate = DateTime.Now, DueDate = DateTime.Now.AddDays(30), ReturnDate = null }
            );
            await context.SaveChangesAsync();

            var repository = new LoanRepository(context);
            var overdueLoans = await repository.GetOverdueLoansAsync();

            Assert.Single(overdueLoans);
        }

        [Fact]
        public async Task ReturnLoanAsync_AlreadyReturned_ShouldReturnFalse()
        {
            using var context = CreateContext("ReturnAlreadyReturned_Test");
            var (book, member) = await SeedBookAndMember(context);
            var repository = new LoanRepository(context);

            var loan = await repository.CreateLoanAsync(book.Id, member.Id);
            await repository.ReturnLoanAsync(loan.Id);

            var result = await repository.ReturnLoanAsync(loan.Id);

            Assert.False(result);
        }
    }
}
