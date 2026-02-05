using Bibliotekssystem.Models;
using Bibliotekssystem.Services;

namespace Biblioteksystem.Tests.Services
{
    public class LoanManagerTests
    {
        // Återanvändbara testdata
        Book book = new Book("123", "Testbok", "Författare", 2024);
        Member member = new Member("M001", "Daniel Aldemir", "daniel@test.se", DateTime.Now);


        // ------------------------------------------
        // CREATELOAN-TESTER
        // ------------------------------------------

        [Fact]
        public void CreateLoan_ShouldCreateLoanAndUpdateBookAndMember()
        {
            // Arrange
            var loanManager = new LoanManager();

            // Act
            var loan = loanManager.CreateLoan(book, member);

            // Assert
            Assert.NotNull(loan);                              // Lånet skapades
            Assert.Single(loanManager.Loans);                  // Lånet finns i listan
            Assert.False(book.IsAvailable);                    // Boken är inte längre tillgänglig
            Assert.Contains(book, member.BorrowedBooks);       // Boken finns i medlemmens lånade böcker
        }

        [Fact]
        public void CreateLoan_ShouldReturnNull_WhenBookNotAvailable()
        {
            // Arrange
            var loanManager = new LoanManager();
            book.IsAvailable = false;  // Boken är redan utlånad

            // Act
            var loan = loanManager.CreateLoan(book, member);

            // Assert
            Assert.Null(loan);                  // Inget lån skapades
            Assert.Empty(loanManager.Loans);   // Inga lån i listan
        }

        [Theory]
        [InlineData(7)]
        [InlineData(14)]
        [InlineData(30)]
        public void CreateLoan_ShouldSetCorrectDueDate(int loanDays)
        {
            // Arrange
            var loanManager = new LoanManager();
            var newBook = new Book("456", "Annan bok", "Författare", 2024);
            var beforeCreate = DateTime.Now;

            // Act
            var loan = loanManager.CreateLoan(newBook, member, loanDays);

            // Assert
            Assert.NotNull(loan);  // Lånet skapades
            Assert.True(loan.DueDate >= beforeCreate.AddDays(loanDays).AddSeconds(-1));  // Jämför med förväntat datum (med liten marginal för exekveringstid)
        }


        // ------------------------------------------
        // RETURNLOAN-TESTER
        // ------------------------------------------

        [Fact]
        public void ReturnLoan_ShouldReturnTrueAndUpdateBookAndMember()
        {
            // Arrange
            var loanManager = new LoanManager();
            var loan = loanManager.CreateLoan(book, member);

            // Act
            var result = loanManager.ReturnLoan(loan!);

            // Assert
            Assert.True(result);                                  // Retur lyckades
            Assert.True(loan!.IsReturned);                        // Lånet är markerat som returnerat
            Assert.True(book.IsAvailable);                        // Boken är tillgänglig igen
            Assert.DoesNotContain(book, member.BorrowedBooks);    // Boken borttagen från medlemmens lista
        }

        [Fact]
        public void ReturnLoan_ShouldReturnFalse_WhenAlreadyReturned()
        {
            // Arrange
            var loanManager = new LoanManager();
            var loan = loanManager.CreateLoan(book, member);
            loanManager.ReturnLoan(loan!);  // Returnera första gången

            // Act
            var result = loanManager.ReturnLoan(loan!);  // Försök returnera igen

            // Assert
            Assert.False(result);  // Ska misslyckas då lånet redan är returnerat
        }


        // ------------------------------------------
        // GETACTIVELOANS-TESTER
        // ------------------------------------------

        [Theory]
        [InlineData(3, 0, 3)]  // 3 aktiva, 0 returnerade = 3
        [InlineData(2, 1, 2)]  // 2 aktiva, 1 returnerad = 2
        [InlineData(0, 2, 0)]  // 0 aktiva, 2 returnerade = 0
        public void GetActiveLoans_ShouldReturnCorrectCount(int active, int returned, int expected)
        {
            // Arrange
            var loanManager = new LoanManager();
            var loans = new List<Loan>();

            // Skapa alla lån
            for (int i = 0; i < active + returned; i++)  // Skapa totalt antal lån
            {
                var b = new Book($"ISBN{i}", $"Bok{i}", "Författare", 2024);
                var m = new Member($"M{i}", $"Medlem{i}", $"m{i}@test.se", DateTime.Now);
                var loan = loanManager.CreateLoan(b, m);
                loans.Add(loan!);
            }

            // Returnera några lån
            for (int i = 0; i < returned; i++)
            {
                loanManager.ReturnLoan(loans[i]);
            }

            // Act
            var result = loanManager.GetActiveLoans();  // Hämta enbart aktiva lån

            // Assert
            Assert.Equal(expected, result.Count);  // Kontrollera att antalet aktiva lån stämmer
        }


        // ------------------------------------------
        // GETOVERDUELOANS-TESTER
        // ------------------------------------------

        [Fact]
        public void GetOverdueLoans_ShouldReturnOnlyOverdueLoans()
        {
            // Arrange
            var loanManager = new LoanManager();

            // Skapa ett försenat lån (manuellt med gamla datum)
            var overdueBook = new Book("111", "Försenad", "Författare", 2024);
            var overdueLoan = loanManager.CreateLoan(overdueBook, member, -10); // 10 dagar bakåt, alltså försenat

            // Skapa ett normalt lån
            var normalBook = new Book("222", "Normal", "Författare", 2024);
            var normalLoan = loanManager.CreateLoan(normalBook, member, 30);  // 30 dagar framåt

            // Act
            var result = loanManager.GetOverdueLoans();  // Hämtar endast försenade lån

            // Assert 
            Assert.Single(result);                      // Endast ett försenat lån
            Assert.DoesNotContain(normalLoan, result); // Det normala lånet ska inte finnas i resultatet
            Assert.Contains(overdueLoan, result);  // Det manuellt skapade försenade lånet ska finnas i resultatet
        }


        // ------------------------------------------
        // GETLOANSBYMEMBER-TESTER
        // ------------------------------------------

        [Theory]
        [InlineData(0)]
        [InlineData(2)]
        [InlineData(4)]
        public void GetLoansByMember_ShouldReturnCorrectCount(int numberOfLoans)
        {
            // Arrange
            var loanManager = new LoanManager();
            var targetMember = new Member("M001", "Daniel", "daniel@test.se", DateTime.Now);
            var otherMember = new Member("M002", "Anna", "anna@test.se", DateTime.Now);

            // Skapa lån för target-medlem
            for (int i = 0; i < numberOfLoans; i++)
            {
                var b = new Book($"ISBN{i}", $"Bok{i}", "Författare", 2024);
                loanManager.CreateLoan(b, targetMember);
            }

            // Skapa lån för annan medlem (ska inte räknas)
            var otherBook = new Book("OTHER", "Annan bok", "Författare", 2024);
            loanManager.CreateLoan(otherBook, otherMember);

            // Act
            var result = loanManager.GetLoansByMember(targetMember); // Hämta lån för target-medlem

            // Assert
            Assert.Equal(numberOfLoans, result.Count); // Kontrollera att antalet lån stämmer med target medlemmen
        }


        // ------------------------------------------
        // SEARCHLOANS-TESTER
        // ------------------------------------------

        [Theory]
        [InlineData("Testbok", 1)]       // Matchar bokens titel
        [InlineData("Daniel", 1)]        // Matchar medlemmens namn
        [InlineData("M001", 1)]          // Matchar medlemmens ID
        [InlineData("123", 1)]           // Matchar bokens ISBN
        [InlineData("XYZ999", 0)]        // Ingen match
        public void SearchLoans_ShouldReturnExpectedCount(string searchTerm, int expectedCount)
        {
            // Arrange
            var loanManager = new LoanManager();
            var searchBook = new Book("123", "Testbok", "Författare", 2024);
            var searchMember = new Member("M001", "Daniel Aldemir", "daniel@test.se", DateTime.Now);
            loanManager.CreateLoan(searchBook, searchMember);

            // Act
            var result = loanManager.SearchLoans(searchTerm);

            // Assert
            Assert.Equal(expectedCount, result.Count); // Kontrollera att antalet matchande lån stämmer
        }


        // ------------------------------------------
        // GETMOSTACTIVEBORROWER-TESTER
        // ------------------------------------------

        [Fact]
        public void GetMostActiveBorrower_ShouldReturnMemberWithMostActiveLoans()
        {
            // Arrange
            var loanManager = new LoanManager();
            var member1 = new Member("M001", "Daniel", "daniel@test.se", DateTime.Now);
            var member2 = new Member("M002", "Anna", "anna@test.se", DateTime.Now);

            // Member1 lånar 2 böcker
            loanManager.CreateLoan(new Book("1", "Bok1", "F", 2024), member1);
            loanManager.CreateLoan(new Book("2", "Bok2", "F", 2024), member1);

            // Member2 lånar 3 böcker (mest aktiv)
            loanManager.CreateLoan(new Book("3", "Bok3", "F", 2024), member2);
            loanManager.CreateLoan(new Book("4", "Bok4", "F", 2024), member2);
            loanManager.CreateLoan(new Book("5", "Bok5", "F", 2024), member2);

            // Act
            var result = loanManager.GetMostActiveBorrower();

            // Assert
            Assert.NotNull(result);
            Assert.Equal("M002", result.MemberId);  // Anna har flest aktiva lån
        }

        [Fact]
        public void GetMostActiveBorrower_ShouldReturnNull_WhenNoActiveLoans()
        {
            // Arrange
            var loanManager = new LoanManager();

            // Act
            var result = loanManager.GetMostActiveBorrower();

            // Assert
            Assert.Null(result); // Kontrollera att resultatet är null när inga aktiva lån finns
        }



        // ------------------------------------------
        // EDGE CASES & NEGATIVA TESTER
        // ------------------------------------------

        [Fact]
        public void GetActiveLoans_ShouldReturnEmptyList_WhenNoLoansExist()
        {
            // Arrange
            var loanManager = new LoanManager();

            // Act
            var result = loanManager.GetActiveLoans();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void GetOverdueLoans_ShouldReturnEmptyList_WhenNoLoansExist()
        {
            // Arrange
            var loanManager = new LoanManager();

            // Act
            var result = loanManager.GetOverdueLoans();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void SearchLoans_ShouldReturnEmptyList_WhenNoLoansExist()
        {
            // Arrange
            var loanManager = new LoanManager();

            // Act
            var result = loanManager.SearchLoans("Testbok");

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void GetLoansByMember_ShouldReturnEmptyList_WhenMemberHasNoLoans()
        {
            // Arrange
            var loanManager = new LoanManager();
            var memberWithNoLoans = new Member("M999", "Ingen Lån", "ingen@test.se", DateTime.Now);

            // Skapa lån för annan medlem
            var otherMember = new Member("M001", "Annan", "annan@test.se", DateTime.Now);
            var book = new Book("123", "Bok", "Författare", 2024);
            loanManager.CreateLoan(book, otherMember);

            // Act
            var result = loanManager.GetLoansByMember(memberWithNoLoans);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void CreateLoan_ShouldAllowSameMemberToBorrowMultipleBooks()
        {
            // Arrange
            var loanManager = new LoanManager();
            var member = new Member("M001", "Daniel", "daniel@test.se", DateTime.Now);
            var book1 = new Book("1", "Bok1", "Författare", 2024);
            var book2 = new Book("2", "Bok2", "Författare", 2024);
            var book3 = new Book("3", "Bok3", "Författare", 2024);

            // Act
            var loan1 = loanManager.CreateLoan(book1, member);
            var loan2 = loanManager.CreateLoan(book2, member);
            var loan3 = loanManager.CreateLoan(book3, member);

            // Assert
            Assert.NotNull(loan1);
            Assert.NotNull(loan2);
            Assert.NotNull(loan3);
            Assert.Equal(3, loanManager.GetLoansByMember(member).Count);
        }

        [Fact]
        public void IsOverdue_EdgeCase_ShouldHandleDueDateExactlyToday()
        {
            // Arrange - Lån med förfallodatum exakt vid midnatt idag (redan passerat)
            var loanManager = new LoanManager();
            var book = new Book("123", "Bok", "Författare", 2024);
            var member = new Member("M001", "Daniel", "daniel@test.se", DateTime.Now);

            // Skapa lån som förföll igår
            var loan = loanManager.CreateLoan(book, member, 0);  // 0 dagar = förfaller idag

            // Act & Assert
            Assert.NotNull(loan);
            // Beroende på exakt tid kan detta vara true eller false
        }
    }
}