using Microsoft.EntityFrameworkCore;
using Bibliotekssystem.Core.Models;
using Bibliotekssystem.Data;
using Bibliotekssystem.Data.Repositories;

namespace Biblioteksystem.Tests.Repositories
{
    public class MemberRepositoryTests
    {
        // Skapar en ny InMemory-databas för varje test så de inte påverkar varandra
        private LibraryContext CreateContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<LibraryContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            return new LibraryContext(options);
        }

        [Fact]
        public async Task AddAsync_DuplicateMemberId_ShouldThrowException()
        {
            using var context = CreateContext("DuplicateMemberId_Test");
            var repository = new MemberRepository(context);

            var member1 = new Member { MemberId = "M001", Name = "Anna", Email = "anna@test.com" };
            await repository.AddAsync(member1);

            var member2 = new Member { MemberId = "M001", Name = "Erik", Email = "erik@test.com" };

            await Assert.ThrowsAsync<InvalidOperationException>(() => repository.AddAsync(member2));
        }

        [Fact]
        public async Task GetByMemberIdAsync_ShouldReturnCorrectMember()
        {
            using var context = CreateContext("GetByMemberId_Test");
            context.Members.Add(new Member { MemberId = "M001", Name = "Anna Svensson", Email = "anna@test.com" });
            await context.SaveChangesAsync();

            var repository = new MemberRepository(context);
            var result = await repository.GetByMemberIdAsync("M001");

            Assert.NotNull(result);
            Assert.Equal("Anna Svensson", result.Name);
        }

        [Fact]
        public async Task DeleteAsync_MemberWithActiveLoans_ShouldThrowException()
        {
            using var context = CreateContext("DeleteMemberWithLoans_Test");

            var book = new Book { ISBN = "123", Title = "Testbok", Author = "A", PublishedYear = 2024 };
            var member = new Member { MemberId = "M001", Name = "Anna", Email = "anna@test.com" };
            context.Books.Add(book);
            context.Members.Add(member);
            await context.SaveChangesAsync();

            context.Loans.Add(new Loan
            {
                BookId = book.Id,
                MemberId = member.Id,
                LoanDate = DateTime.Now,
                DueDate = DateTime.Now.AddDays(30),
                ReturnDate = null
            });
            await context.SaveChangesAsync();

            var repository = new MemberRepository(context);

            await Assert.ThrowsAsync<InvalidOperationException>(() => repository.DeleteAsync(member.Id));
        }
    }
}
