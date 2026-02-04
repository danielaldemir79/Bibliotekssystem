using Bibliotekssystem.Models;

namespace Biblioteksystem.Tests.Models
{
    public class MemberTests
    {
        // ------------------------------------------
        // KONSTRUKTOR-TESTER
        // ------------------------------------------

        [Fact]
        public void Constructor_ShouldSetPropertiesCorrectly() // Test för att verifiera att konstruktorn sätter egenskaper korrekt
        {
            // Arrange
            var memberSince = DateTime.Now;

            // Act
            var member = new Member("M001", "Daniel Aldemir", "daniel@test.se", memberSince);

            // Assert
            Assert.Equal("M001", member.MemberId);
            Assert.Equal("Daniel Aldemir", member.Name);
            Assert.Equal("daniel@test.se", member.Email);
            Assert.Equal(memberSince, member.MemberSince);
            Assert.Empty(member.BorrowedBooks);
        }


        // ------------------------------------------
        // BORROWEDBOOKS-TESTER
        // ------------------------------------------

        [Fact]
        // Test för att verifiera att en ny medlem har en tom lista för lånade böcker
        public void BorrowedBooks_ShouldBeEmptyForNewMember()
        {
            // Arrange & Act
            var member = new Member("M001", "Daniel Aldemir", "daniel@test.se", DateTime.Now);

            // Assert
            Assert.Empty(member.BorrowedBooks);
            Assert.Equal(0, member.BorrowedBooks.Count);
        }

        [Fact]
        // Test för att verifiera att en bok läggs till korrekt i listan över lånade böcker
        public void AddBorrowedBook_ShouldAddBookToList()
        {
            // Arrange
            var member = new Member("M001", "Daniel Aldemir", "daniel@test.se", DateTime.Now);
            var book = new Book("123", "Testbok", "Författare", 2024);

            // Act
            member.AddBorrowedBook(book);

            // Assert
            Assert.Single(member.BorrowedBooks);
            Assert.Contains(book, member.BorrowedBooks);
        }

        [Fact]

        // Test för att verifiera att en bok tas bort korrekt från listan över lånade böcker
        public void RemoveBorrowedBook_ShouldRemoveBookFromList()
        {
            // Arrange
            var member = new Member("M001", "Daniel Aldemir", "daniel@test.se", DateTime.Now);
            var book = new Book("123", "Testbok", "Författare", 2024);
            member.AddBorrowedBook(book);

            // Act
            member.RemoveBorrowedBook(book);

            // Assert
            Assert.Empty(member.BorrowedBooks);
            Assert.DoesNotContain(book, member.BorrowedBooks);
        }


        // ------------------------------------------
        // GETINFO-TESTER
        // ------------------------------------------

        [Fact]
        // Test för att verifiera att GetInfo returnerar korrekt formaterad sträng
        public void GetInfo_ShouldReturnFormattedString()
        {
            // Arrange
            var member = new Member("M001", "Daniel Aldemir", "daniel@test.se", DateTime.Now);

            // Act
            var result = member.GetInfo();

            // Assert
            Assert.Contains("M001", result);
            Assert.Contains("Daniel Aldemir", result);
            Assert.Contains("daniel@test.se", result);
        }

        [Fact]
        public void GetInfo_ShouldShowBorrowedBooksCount()
        // Test för att verifiera att GetInfo visar korrekt antal lånade böcker
        {
            // Arrange
            var member = new Member("M001", "Daniel Aldemir", "daniel@test.se", DateTime.Now);
            var book = new Book("123", "Testbok", "Författare", 2024);
            member.AddBorrowedBook(book);

            // Act
            var result = member.GetInfo();

            // Assert
            Assert.Contains("1", result); // Antal lånade böcker
        }


        // ------------------------------------------
        // MATCHES-TESTER (Sökfunktion)
        // ------------------------------------------

        [Theory]
        [InlineData("M001", true)]              // Matchar MemberId
        [InlineData("Daniel", true)]            // Matchar delar av namn
        [InlineData("aldemir", true)]           // Case-insensitive
        [InlineData("daniel@test.se", true)]    // Matchar e-post
        [InlineData("test.se", true)]           // Matchar del av e-post
        [InlineData("XYZ999", false)]           // Ingen match
        [InlineData("someone@else.com", false)] // Ingen match
        public void Matches_ShouldReturnExpectedResult(string searchTerm, bool expected)
        {
            // Arrange
            var member = new Member("M001", "Daniel Aldemir", "daniel@test.se", DateTime.Now);

            // Act
            var result = member.Matches(searchTerm);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}