using Microsoft.EntityFrameworkCore;
using Bibliotekssystem.Core.Models;
using Bibliotekssystem.Data;
using Bibliotekssystem.Data.Repositories;

namespace Biblioteksystem.Tests.Repositories
{
    public class BookRepositoryTests
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
        public async Task AddAsync_ShouldSaveBookToDatabase()
        {
            using var context = CreateContext("AddBook_Test");
            var repository = new BookRepository(context);
            var book = new Book { ISBN = "123", Title = "Testbok", Author = "Författare", PublishedYear = 2024 };

            await repository.AddAsync(book);

            var savedBook = await context.Books.FirstOrDefaultAsync(b => b.ISBN == "123");
            Assert.NotNull(savedBook);
            Assert.Equal("Testbok", savedBook.Title);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllBooks()
        {
            using var context = CreateContext("GetAllBooks_Test");
            context.Books.AddRange(
                new Book { ISBN = "111", Title = "Bok 1", Author = "A", PublishedYear = 2020 },
                new Book { ISBN = "222", Title = "Bok 2", Author = "B", PublishedYear = 2021 }
            );
            await context.SaveChangesAsync();

            var repository = new BookRepository(context);
            var result = await repository.GetAllAsync();

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task SearchAsync_ShouldFindBooksByTitle()
        {
            using var context = CreateContext("SearchBooks_Test");
            context.Books.AddRange(
                new Book { ISBN = "111", Title = "Sagan om ringen", Author = "Tolkien", PublishedYear = 1954 },
                new Book { ISBN = "222", Title = "Hobbiten", Author = "Tolkien", PublishedYear = 1937 },
                new Book { ISBN = "333", Title = "1984", Author = "Orwell", PublishedYear = 1949 }
            );
            await context.SaveChangesAsync();

            var repository = new BookRepository(context);
            var result = await repository.SearchAsync("Tolkien");

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task AddAsync_DuplicateISBN_ShouldThrowException()
        {
            using var context = CreateContext("DuplicateISBN_Test");
            var repository = new BookRepository(context);

            var book1 = new Book { ISBN = "SAME", Title = "Bok 1", Author = "A", PublishedYear = 2020 };
            await repository.AddAsync(book1);

            var book2 = new Book { ISBN = "SAME", Title = "Bok 2", Author = "B", PublishedYear = 2021 };

            await Assert.ThrowsAsync<InvalidOperationException>(() => repository.AddAsync(book2));
        }

        [Fact]
        public async Task DeleteAsync_BookWithoutLoans_ShouldRemoveBook()
        {
            using var context = CreateContext("DeleteBook_Test");
            var repository = new BookRepository(context);

            var book = new Book { ISBN = "DEL", Title = "Ta bort mig", Author = "A", PublishedYear = 2024 };
            await repository.AddAsync(book);

            await repository.DeleteAsync(book.Id);

            var result = await context.Books.FindAsync(book.Id);
            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateBookProperties()
        {
            using var context = CreateContext("UpdateBook_Test");
            var repository = new BookRepository(context);

            var book = new Book { ISBN = "UPD", Title = "Gammal titel", Author = "A", PublishedYear = 2020 };
            await repository.AddAsync(book);

            book.Title = "Ny titel";
            book.PublishedYear = 2025;
            await repository.UpdateAsync(book);

            var updated = await context.Books.FindAsync(book.Id);
            Assert.Equal("Ny titel", updated!.Title);
            Assert.Equal(2025, updated.PublishedYear);
        }

        [Fact]
        public async Task UpdateAsync_DuplicateISBN_ShouldThrowException()
        {
            using var context = CreateContext("UpdateDuplicateISBN_Test");
            var repository = new BookRepository(context);

            var book1 = new Book { ISBN = "AAA", Title = "Bok 1", Author = "A", PublishedYear = 2020 };
            var book2 = new Book { ISBN = "BBB", Title = "Bok 2", Author = "B", PublishedYear = 2021 };
            await repository.AddAsync(book1);
            await repository.AddAsync(book2);

            book2.ISBN = "AAA";

            await Assert.ThrowsAsync<InvalidOperationException>(() => repository.UpdateAsync(book2));
        }

        [Fact]
        public async Task GetByISBNAsync_ShouldReturnCorrectBook()
        {
            using var context = CreateContext("GetByISBN_Test");
            context.Books.Add(new Book { ISBN = "978-TEST", Title = "ISBN-bok", Author = "A", PublishedYear = 2024 });
            await context.SaveChangesAsync();

            var repository = new BookRepository(context);
            var result = await repository.GetByISBNAsync("978-TEST");

            Assert.NotNull(result);
            Assert.Equal("ISBN-bok", result.Title);
        }
    }
}
