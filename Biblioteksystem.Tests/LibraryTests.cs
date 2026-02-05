using Bibliotekssystem;
using Bibliotekssystem.Models;

namespace Biblioteksystem.Tests
{
    public class LibraryTests
    {
        // ------------------------------------------
        // KONSTRUKTOR-TESTER
        // ------------------------------------------


        [Fact]
        public void Constructor_ShouldInitializeAllServices() //Verifiera att alla services initieras korrekt
        {
            // Act
            var library = new Library();

            // Assert
            Assert.NotNull(library.BookCatalog);        // Verifiera att BookCatalog är initierad
            Assert.NotNull(library.MemberRegistry);     // Verifiera att MemberRegistry är initierad 
            Assert.NotNull(library.LoanManager);        // Verifiera att LoanManager är initierad   
        }

        [Fact]
        public void Constructor_ShouldStartWithEmptyCollections() //Verifiera att alla collections startar tomma
        {
            // Act
            var library = new Library();

            // Assert
            Assert.Empty(library.BookCatalog.Books);    // Verifiera att BookCatalog startar tom
            Assert.Empty(library.MemberRegistry.Members);  // Verifiera att MemberRegistry startar tom
            Assert.Empty(library.LoanManager.Loans);    // Verifiera att LoanManager startar tom
        }


        // ------------------------------------------
        // INTEGRATION-TESTER (Services samarbetar)
        // ------------------------------------------

        [Fact]
        public void Library_ShouldAllowFullLoanWorkflow()  //Verifierar att ett lån kan skapas och uppdatera alla services korrekt
        {
            // Arrange
            var library = new Library();
            var book = new Book("123", "Testbok", "Författare", 2024);
            var member = new Member("M001", "Daniel", "daniel@test.se", DateTime.Now);

            library.BookCatalog.AddBook(book);
            library.MemberRegistry.AddMember(member);

            // Act - Skapa lån
            var loan = library.LoanManager.CreateLoan(book, member);

            // Assert - Verifiera att allt uppdaterades korrekt
            Assert.NotNull(loan);
            Assert.False(book.IsAvailable);
            Assert.Single(library.LoanManager.GetActiveLoans());
            Assert.Contains(book, member.BorrowedBooks);
        }

        [Fact]
        public void Library_ShouldAllowFullReturnWorkflow()
        {
            // Arrange
            var library = new Library();
            var book = new Book("123", "Testbok", "Författare", 2024);
            var member = new Member("M001", "Daniel", "daniel@test.se", DateTime.Now);

            library.BookCatalog.AddBook(book);
            library.MemberRegistry.AddMember(member);
            var loan = library.LoanManager.CreateLoan(book, member);

            // Act - Returnera lån
            var result = library.LoanManager.ReturnLoan(loan!);

            // Assert - Verifiera att allt uppdaterades korrekt
            Assert.True(result);
            Assert.True(book.IsAvailable);
            Assert.Empty(library.LoanManager.GetActiveLoans());
            Assert.Empty(member.BorrowedBooks);
        }


        // ------------------------------------------
        // STATISTIK-TESTER (via services)
        // ------------------------------------------

        [Fact]
        public void Library_ShouldTrackCorrectStatistics()
        {
            // Arrange
            var library = new Library();

            // Lägg till böcker
            library.BookCatalog.AddBook(new Book("1", "Bok1", "F1", 2020));
            library.BookCatalog.AddBook(new Book("2", "Bok2", "F2", 2021));
            library.BookCatalog.AddBook(new Book("3", "Bok3", "F3", 2022));

            // Lägg till medlemmar
            var member1 = new Member("M001", "Daniel", "daniel@test.se", DateTime.Now);
            var member2 = new Member("M002", "Anna", "anna@test.se", DateTime.Now);
            library.MemberRegistry.AddMember(member1);
            library.MemberRegistry.AddMember(member2);

            // Skapa lån
            library.LoanManager.CreateLoan(library.BookCatalog.FindByISBN("1")!, member1);
            library.LoanManager.CreateLoan(library.BookCatalog.FindByISBN("2")!, member2);

            // Assert
            Assert.Equal(3, library.BookCatalog.Books.Count);                   // Totalt 3 böcker
            Assert.Equal(1, library.BookCatalog.GetAvailableBooks().Count);     // 1 tillgänglig
            Assert.Equal(2, library.MemberRegistry.Members.Count);              // 2 medlemmar
            Assert.Equal(2, library.LoanManager.GetActiveLoans().Count);        // 2 aktiva lån
        }

        [Fact]
        public void Library_ShouldFindMostActiveBorrower()
        {
            // Arrange
            var library = new Library();
            var member1 = new Member("M001", "Daniel", "daniel@test.se", DateTime.Now);
            var member2 = new Member("M002", "Anna", "anna@test.se", DateTime.Now);

            library.MemberRegistry.AddMember(member1);
            library.MemberRegistry.AddMember(member2);

            // Daniel lånar 1 bok
            var book1 = new Book("1", "Bok1", "F", 2024);
            library.BookCatalog.AddBook(book1);
            library.LoanManager.CreateLoan(book1, member1);

            // Anna lånar 2 böcker (mest aktiv)
            var book2 = new Book("2", "Bok2", "F", 2024);
            var book3 = new Book("3", "Bok3", "F", 2024);
            library.BookCatalog.AddBook(book2);
            library.BookCatalog.AddBook(book3);
            library.LoanManager.CreateLoan(book2, member2);
            library.LoanManager.CreateLoan(book3, member2);

            // Act
            var mostActive = library.LoanManager.GetMostActiveBorrower();

            // Assert
            Assert.NotNull(mostActive);
            Assert.Equal("M002", mostActive.MemberId);  // Anna är mest aktiv
        }
    }
}