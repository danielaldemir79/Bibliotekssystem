using Bibliotekssystem.Models;


namespace Biblioteksystem.Tests.Models
{
    public class LoanTests
    {
        // Arrange på dessa här i klassen för att återanvända i testerna.
        Book book = new Book("123", "Testbok", "Testförfattare", 2024);
        Member member = new Member("M001", "Daniel Aldemir", "danielaldemir@test.se", DateTime.Now);
       



        // ------------------------------------------
        // KONSTRUKTOR-TESTER
        // ------------------------------------------------

        [Fact]
        public void Constructor_ShouldSetPropertiesCorrectly() // Test för att verifiera att konstruktorn sätter egenskaper korrekt
        {
            // Arrange
            var loanDate = DateTime.Now;
            var dueDate = loanDate.AddDays(30);

            // Act
            var loan = new Loan(book, member, loanDate, dueDate);

            // Assert
            Assert.Equal(book, loan.Book);
            Assert.Equal(member, loan.Member);
            Assert.Equal(loanDate, loan.LoanDate);
            Assert.Equal(dueDate, loan.DueDate);
            Assert.Null(loan.ReturnDate);

        }

        // ------------------------------------------
        // ISRETURNED-TESTER
        // ---------------------------------------------------

        [Fact]
        public void IsReturned_ShouldReturnFalse_WhenReturnDateIsNull()
        {
            // Arrange
            var loan = new Loan(book, member, DateTime.Now, DateTime.Now.AddDays(30));

            // Act & Assert
            Assert.False(loan.IsReturned);  // Verifierar att IsReturned returnerar false när ReturnDate är null
        }

        [Fact]
        public void IsReturned_ShouldReturnTrue_WhenReturnDateIsSet() 
        {

            var loan = new Loan(book, member, DateTime.Now, DateTime.Now.AddDays(30));

            // Act
            loan.ReturnBook();

            // Assert
            Assert.True(loan.IsReturned);    // Verifierar att IsReturned returnerar true efter att boken returnerats
            Assert.NotNull(loan.ReturnDate); // Verifierar att ReturnDate inte är null efter att boken returnerats
        }


        // ------------------------------------------
        // ISOVERDUE-TESTER
        // ----------------------------------------------
        [Fact]
        public void IsOverdue_ShouldReturnFalse_WhenDueDateIsInFuture()
        {
            // Arrange
            var loan = new Loan(book, member, DateTime.Now, DateTime.Now.AddDays(14));

            // Act & Assert
            Assert.False(loan.IsOverdue); // Verifierar att IsOverdue returnerar false när förfallodatumet är i framtiden
        }

        [Fact]
        public void IsOverdue_ShouldReturnTrue_WhenDueDateHasPassed()
        {
            // Arrange
            // Skapar ett lån med förfallodatum i det förflutna
            var loan = new Loan(book, member, DateTime.Now.AddDays(-30), DateTime.Now.AddDays(-1));

            // Act & Assert
            Assert.True(loan.IsOverdue); // Verifierar att IsOverdue returnerar true när förfallodatumet har passerat
        }

        [Fact]
        public void IsOverdue_ShouldReturnFalse_WhenBookIsReturned()
        {
            // Arrange - Skapa ett försenat lån
            var loan = new Loan(book, member, DateTime.Now.AddDays(-30), DateTime.Now.AddDays(-1));

            // Act - Returnera boken
            loan.ReturnBook();

            // Assert - Ska inte vara överdue efter retur
            Assert.False(loan.IsOverdue);
        }


        // ------------------------------------------
        // RETURNBOOK-TESTER
        // ------------------------------------------

        [Fact]
        public void ReturnBook_ShouldSetReturnDateToNow()
        {
            // Arrange
            var loan = new Loan(book, member, DateTime.Now, DateTime.Now.AddDays(30));
            var beforeReturn = DateTime.Now;  // Spara tid före retur

            // Act
            loan.ReturnBook();

            // Assert
            Assert.NotNull(loan.ReturnDate);  // Verifierar att ReturnDate inte är null efter retur
            Assert.True(loan.ReturnDate >= beforeReturn); // Verifierar att ReturnDate är satt till en tid efter beforeReturn
        }



        // ------------------------------------------
        // MATCHES-TESTER (Sökfunktion)
        // ------------------------------------------

        [Theory]
        [InlineData("Testbok", true)]       // Matchar bokens titel
        [InlineData("Författare", true)]    // Matchar bokens författare
        [InlineData("123", true)]           // Matchar bokens ISBN
        [InlineData("Daniel Aldemir", true)]   // Matchar medlemmens namn
        [InlineData("M001", true)]          // Matchar medlemmens ID
        [InlineData("danielaldemir@test.se", true)] // Matchar medlemmens e-post
        [InlineData("XYZ999", false)]       // Ingen match
        public void Matches_ShouldReturnExpectedResult(string searchTerm, bool expected)
        {
            // Arrange
            var loan = new Loan(book, member, DateTime.Now, DateTime.Now.AddDays(30));

            // Act
            var result = loan.Matches(searchTerm);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}