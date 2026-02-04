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
        // ------------------------------------------
        // KONSTRUKTOR-TESTER
        // -------------------------------------------


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
            var book = new Book("12345", "Testbok", "Testförfattare", 2024, false);

            // Assert
            Assert.False(book.IsAvailable);
        }


        // ------------------------------------------
        // ISAVAILABLE-TESTER
        // ------------------------------------------

        // Testar att IsAvailable kan ändras
        [Theory]
        [InlineData(true, false)]
        [InlineData(false, true)]
        public void IsAvailable_CanBeChanged(bool initialValue, bool newValue)
        {
            // Arrange
            var book = new Book("123", "Testbok", "Testförfattare", 2024, initialValue);

            // Act
            book.IsAvailable = newValue;

            // Assert
            Assert.Equal(newValue, book.IsAvailable);
        }


        // ------------------------------------------
        // GETINFO-TESTER
        // ------------------------------------------

        [Fact]
        public void GetInfo_ShouldReturnFormattedString() // Testar att GetInfo returnerar korrekt formaterad sträng
        {
            // Arrange
            var book = new Book("12345", "Testbok", "Testförfattare", 2024);

            // Act
            var result = book.GetInfo();

            // Assert
            Assert.Contains("12345", result);
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

        // Tester olika sökord mot bokens egenskaper
        [Theory]
        [InlineData("Harry", true)]
        [InlineData("potter", true)]      // Case-insensitive
        [InlineData("Rowling", true)]
        [InlineData("12345", true)]       // ISBN
        [InlineData("1997", true)]        // År
        [InlineData("Tolkien", false)]    // Ingen match
        public void Matches_ShouldReturnExpectedResult(string searchTerm, bool expected)
        {
            // Arrange
            var book = new Book("12345", "Harry Potter", "J.K. Rowling", 1997);

            // Act
            var result = book.Matches(searchTerm);

            // Assert
            Assert.Equal(expected, result);
        }

    }
}
