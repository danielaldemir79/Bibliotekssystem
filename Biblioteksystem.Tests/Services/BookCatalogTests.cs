using Bibliotekssystem.Models;
using Bibliotekssystem.Services;

namespace Biblioteksystem.Tests.Services
{
    public class BookCatalogTests
    {
        // Arrange av catalog redan här för att återanvända i flera tester 
        BookCatalog catalog = new BookCatalog();



        // ------------------------------------------
        // ADDBOOK / REMOVEBOOK-TESTER
        // ------------------------------------------

        [Theory]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(5)]
        public void AddBook_ShouldAddCorrectNumberOfBooks(int numberOfBooks)
        {

            // Act
            for (int i = 0; i < numberOfBooks; i++)
            {
                catalog.AddBook(new Book($"ISBN-{i}", $"Bok{i}", "Författare", 2020 + i));
            }

            // Assert
            Assert.Equal(numberOfBooks, catalog.Books.Count); // Kontrollera att antalet böcker i katalogen är korrekt
        }


        [Fact]
        public void RemoveBook_ShouldRemoveBookFromCatalog()
        {
            // Arrange
            var book = new Book("123", "Testbok", "Författare", 2024);
            catalog.AddBook(book);

            // Act
            catalog.RemoveBook(book);

            // Assert
            Assert.Empty(catalog.Books); // Kontrollera att katalogen är tom efter borttagning
            Assert.DoesNotContain(book, catalog.Books); //  Kontrollera att boken inte längre finns i katalogen
        }



        // ------------------------------------------
        // FINDBYISBN-TESTER
        // ------------------------------------------

        [Theory]
        [InlineData("1234", true)]   // Finns i katalogen
        [InlineData("nonexistent-isbn", false)]   // Finns inte
        [InlineData("", false)]                   // Tom sträng
        public void FindByISBN_ShouldReturnExpectedResult(string isbn, bool shouldFind)
        {
            // Arrange
            catalog.AddBook(new Book("1234", "Testbok", "Författare", 2024));

            // Act
            var result = catalog.FindByISBN(isbn);

            // Assert
            Assert.Equal(shouldFind, result != null); // Kontrollera om resultatet är som förväntat
        }



        // ------------------------------------------
        // GETAVAILABLEBOOKS-TESTER
        // ------------------------------------------

        [Theory]
        [InlineData(3, 0, 3)]  // 3 tillgängliga, 0 utlånade = 3 resultat
        [InlineData(2, 2, 2)]  // 2 tillgängliga, 2 utlånade = 2 resultat
        [InlineData(0, 3, 0)]  // 0 tillgängliga, 3 utlånade = 0 resultat
        public void GetAvailableBooks_ShouldReturnCorrectCount(int available, int borrowed, int expected)
        {
            // Arrange

            for (int i = 0; i < available; i++) // Lägg till tillgängliga böcker
            {
                catalog.AddBook(new Book($"A{i}", $"Tillgänglig{i}", "Författare", 2024));
            }

            for (int i = 0; i < borrowed; i++) // Lägg till utlånade böcker
            {
                var book = new Book($"B{i}", $"Utlånad{i}", "Författare", 2024);
                book.IsAvailable = false;
                catalog.AddBook(book);
            }

            // Act
            var result = catalog.GetAvailableBooks(); // Hämta tillgängliga böcker

            // Assert
            Assert.Equal(expected, result.Count); // Kontrollera att antalet tillgängliga böcker är korrekt 
        }



        // ------------------------------------------
        // SEARCHBOOKS-TESTER
        // ------------------------------------------

        [Theory]
        [InlineData("Tolkien", 1)]      // Matchar författare
        [InlineData("Sagan", 1)]        // Matchar titel
        [InlineData("Harry", 1)]        // Matchar annan titel
        [InlineData("1954", 1)]         // Matchar årtal
        [InlineData("blabla", 0)]       // Ingen match
        public void SearchBooks_ShouldReturnExpectedCount(string searchTerm, int expectedCount)
        {
            // Arrange
            catalog.AddBook(new Book("123", "Sagan om ringen", "Tolkien", 1954));
            catalog.AddBook(new Book("456", "Harry Potter", "Rowling", 1997));

            // Act
            var result = catalog.SearchBooks(searchTerm); // Sök efter böcker baserat på sökord

            // Assert
            Assert.Equal(expectedCount, result.Count); // Kontrollera att antalet matchande böcker är korrekt
        }




        // ------------------------------------------
        // SORTERING-TESTER
        // ------------------------------------------

        [Fact]
        public void GetBooksSortedByTitle_ShouldReturnAlphabeticalOrder()
        {
            // Arrange
            catalog.AddBook(new Book("1", "C-bok", "Författare", 2020));
            catalog.AddBook(new Book("2", "A-bok", "Författare", 2021));
            catalog.AddBook(new Book("3", "B-bok", "Författare", 2022));

            // Act
            var result = catalog.GetBooksSortedByTitle(); // Hämta böcker sorterade efter titel

            // Assert
            Assert.Equal("A-bok", result[0].Title); // Kontrollera att titlarna är i alfabetisk ordning
            Assert.Equal("B-bok", result[1].Title);
            Assert.Equal("C-bok", result[2].Title);
        }


        [Fact]
        public void GetBooksSortedByAuthor_ShouldReturnAlphabeticalOrder()
        {
            // Arrange
            catalog.AddBook(new Book("1", "Bok1", "Carlsson", 2020));
            catalog.AddBook(new Book("2", "Bok2", "Andersson", 2021));
            catalog.AddBook(new Book("3", "Bok3", "Bengtsson", 2022));

            // Act
            var result = catalog.GetBooksSortedByAuthor(); // Hämta böcker sorterade efter författare

            // Assert
            Assert.Equal("Andersson", result[0].Author); // Kontrollera att författarna är i alfabetisk ordning
            Assert.Equal("Bengtsson", result[1].Author);
            Assert.Equal("Carlsson", result[2].Author);
        }

        [Fact]
        public void GetBooksSortedByPublishedYear_ShouldReturnChronologicalOrder()
        {
            // Arrange
            catalog.AddBook(new Book("1", "Bok1", "Författare", 2022));
            catalog.AddBook(new Book("2", "Bok2", "Författare", 2020));
            catalog.AddBook(new Book("3", "Bok3", "Författare", 2021));

            // Act
            var result = catalog.GetBooksSortedByPublishedYear(); // Hämta böcker sorterade efter publiceringsår

            // Assert
            Assert.Equal(2020, result[0].PublishedYear); // Kontrollera att böckerna är i kronologisk ordning
            Assert.Equal(2021, result[1].PublishedYear);
            Assert.Equal(2022, result[2].PublishedYear);
        }

    }
}
