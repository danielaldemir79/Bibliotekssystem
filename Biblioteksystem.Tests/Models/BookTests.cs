using Bibliotekssystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteksystem.Tests.Models
{
    public class BookTests
    {
        [Fact]  // Testar att konstruktorn sätter egenskaper korrekt
        public void Constructor_ShouldSetPropertiesCorrectly()
        {
            // Arrange & Act
            var book = new Book("123", "Testbok", "Testförfattare", 2024);

            // Assert
            Assert.Equal("123", book.ISBN);
            Assert.Equal("Testbok", book.Title);
            Assert.Equal("Testförfattare", book.Author);
            Assert.Equal(2024, book.PublishedYear);
            Assert.True(book.IsAvailable);
        }

        [Fact]
        public void Constructor_WithIsAvailableFalse_ShouldSetIsAvailableToFalse() // Testar att konstruktorn sätter IsAvailable korrekt när false anges
        {
            // Arrange & Act
            var book = new Book("978-91-0-012345-6", "Testbok", "Testförfattare", 2024, false);

            // Assert
            Assert.False(book.IsAvailable);
        }


        // ------------------------------------------
        // ISAVAILABLE-TESTER
        // ------------------------------------------

        [Fact]
        public void IsAvailable_ShouldBeTrueForNewBook() // Testar att IsAvailable är true för en ny bok
        {
            // Arrange & Act
            var book = new Book("978-91-0-012345-6", "Testbok", "Testförfattare", 2024);

            // Assert
            Assert.True(book.IsAvailable);
        }

        [Fact]
        public void IsAvailable_CanBeSetToFalse() // Testar att IsAvailable kan sättas till false
        {
            // Arrange
            var book = new Book("978-91-0-012345-6", "Testbok", "Testförfattare", 2024);

            // Act
            book.IsAvailable = false;

            // Assert
            Assert.False(book.IsAvailable);
        }


        // ------------------------------------------
        // GETINFO-TESTER
        // ------------------------------------------

        [Fact]
        public void GetInfo_ShouldReturnFormattedString() // Testar att GetInfo returnerar korrekt formaterad sträng
        {
            // Arrange
            var book = new Book("978-91-0-012345-6", "Testbok", "Testförfattare", 2024);

            // Act
            var result = book.GetInfo();

            // Assert
            Assert.Contains("978-91-0-012345-6", result);
            Assert.Contains("Testbok", result);
            Assert.Contains("Testförfattare", result);
            Assert.Contains("2024", result);
        }

        [Fact]
        public void GetInfo_ShouldShowAvailableStatus() // Testar att GetInfo visar korrekt tillgänglighetsstatus
        {
            // Arrange
            var availableBook = new Book("123", "Bok1", "Författare", 2024, true);
            var unavailableBook = new Book("456", "Bok2", "Författare", 2024, false);

            // Act
            var availableResult = availableBook.GetInfo();
            var unavailableResult = unavailableBook.GetInfo();

            // Assert
            Assert.Contains("True", availableResult);
            Assert.Contains("False", unavailableResult);
        }


        // ------------------------------------------
        // MATCHES-TESTER (Sökfunktion)
        // ------------------------------------------

        [Fact]
        public void Matches_ShouldReturnTrueForMatchingTitle() // Testar att Matches returnerar true för matchande titel
        {
            // Arrange
            var book = new Book("12345", "Harry Potter", "J.K. Rowling", 1997);

            // Act & Assert
            Assert.True(book.Matches("Harry"));
            Assert.True(book.Matches("potter")); // Case-insensitive
        }

        [Fact]
        public void Matches_ShouldReturnTrueForMatchingAuthor() // Testar att Matches returnerar true för matchande författare
        {
            // Arrange
            var book = new Book("12345", "Harry Potter", "J.K. Rowling", 1997);

            // Act & Assert
            Assert.True(book.Matches("Rowling"));
            Assert.True(book.Matches("j.k.")); // Case-insensitive
        }

        [Fact]
        public void Matches_ShouldReturnTrueForMatchingISBN() // Testar att Matches returnerar true för matchande ISBN
        {
            // Arrange
            var book = new Book("12345", "Harry Potter", "J.K. Rowling", 1997);

            // Act & Assert
            Assert.True(book.Matches("123"));
            Assert.True(book.Matches("345")); // Delvis match

        }

        [Fact]
        public void Matches_ShouldReturnTrueForMatchingYear() // Testar att Matches returnerar true för matchande publiceringsår
        {
            // Arrange
            var book = new Book("12345", "Harry Potter", "J.K. Rowling", 1997);

            // Act & Assert
            Assert.True(book.Matches("1997"));
        }

        [Fact]
        public void Matches_ShouldReturnFalseForNonMatchingSearchTerm() // Testar att Matches returnerar false för icke-matchande sökord
        {
            // Arrange
            var book = new Book("978-91-0-012345-6", "Harry Potter", "J.K. Rowling", 1997);

            // Act & Assert
            Assert.False(book.Matches("Tolkien"));
            Assert.False(book.Matches("Sagan om ringen"));
        }

    }
}
