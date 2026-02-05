using Bibliotekssystem.Models;
using Bibliotekssystem.Services;

namespace Biblioteksystem.Tests.Services
{
    public class MemberRegistryTests
    {

        // ------------------------------------------
        // ADDMEMBER / REMOVEMEMBER-TESTER
        // ------------------------------------------

        [Theory]
        [InlineData(1)]
        [InlineData(3)]
        [InlineData(5)]
        public void AddMember_ShouldAddCorrectNumberOfMembers(int numberOfMembers)
        {
            // Arrange
            var registry = new MemberRegistry();

            // Act
            for (int i = 0; i < numberOfMembers; i++)  // Lägg till flera medlemmar
            {
                registry.AddMember(new Member($"M00{i}", $"Medlem{i}", $"medlem{i}@test.se", DateTime.Now));
            }

            // Assert
            Assert.Equal(numberOfMembers, registry.Members.Count); // Kontrollera att rätt antal medlemmar har lagts till
        }

        [Fact]
        public void RemoveMember_ShouldRemoveMemberFromRegistry()
        {
            // Arrange
            var registry = new MemberRegistry();
            var member = new Member("M001", "Daniel", "daniel@test.se", DateTime.Now);
            registry.AddMember(member);  // Lägg till medlem först

            // Act
            registry.RemoveMember(member); // Ta bort medlem

            // Assert
            Assert.Empty(registry.Members); // Kontrollera att registret är tomt
            Assert.DoesNotContain(member, registry.Members);  // Kontrollera att medlemmen inte finns kvar
        }


        // ------------------------------------------
        // FINDBYID-TESTER
        // ------------------------------------------

        [Theory]
        [InlineData("M001", true)]    // Finns i registret
        [InlineData("M999", false)]   // Finns inte
        [InlineData("", false)]       // Tom sträng
        public void FindById_ShouldReturnExpectedResult(string memberId, bool shouldFind)
        {
            // Arrange
            var registry = new MemberRegistry();
            registry.AddMember(new Member("M001", "Daniel", "daniel@test.se", DateTime.Now));

            // Act
            var result = registry.FindById(memberId);  // Sök efter medlem

            // Assert
            Assert.Equal(shouldFind, result != null);  // Kontrollera om resultatet stämmer överens med förväntningen
        }


        // ------------------------------------------
        // FINDBYEMAIL-TESTER
        // ------------------------------------------

        [Theory]
        [InlineData("daniel@test.se", true)]     // Exakt match
        [InlineData("DANIEL@TEST.SE", true)]     // Case-insensitive
        [InlineData("Daniel@Test.Se", true)]     // Blandad case
        [InlineData("other@test.se", false)]     // Finns inte
        [InlineData("", false)]                  // Tom sträng
        public void FindByEmail_ShouldReturnExpectedResult(string email, bool shouldFind)
        {
            // Arrange
            var registry = new MemberRegistry();
            registry.AddMember(new Member("M001", "Daniel", "daniel@test.se", DateTime.Now));

            // Act
            var result = registry.FindByEmail(email);  // Sök efter medlem via email

            // Assert
            Assert.Equal(shouldFind, result != null);  // Kontrollera om resultatet stämmer överens med förväntningen
        }


        // ------------------------------------------
        // GETALLMEMBERS-TESTER
        // ------------------------------------------

        [Theory]
        [InlineData(0)]
        [InlineData(2)]
        [InlineData(5)]
        public void GetAllMembers_ShouldReturnCorrectCount(int numberOfMembers)
        {
            // Arrange
            var registry = new MemberRegistry();
            for (int i = 0; i < numberOfMembers; i++)  // Lägg till flera medlemmar
            {
                registry.AddMember(new Member($"M00{i}", $"Medlem{i}", $"medlem{i}@test.se", DateTime.Now));
            }

            // Act
            var result = registry.GetAllMembers();  // Hämta alla medlemmar

            // Assert
            Assert.Equal(numberOfMembers, result.Count);  // Kontrollera att rätt antal medlemmar returnerades
        }


        // ------------------------------------------
        // SEARCHMEMBERS-TESTER
        // ------------------------------------------

        [Theory]
        [InlineData("Daniel", 1)]           // Matchar namn
        [InlineData("M001", 1)]             // Matchar medlems-ID
        [InlineData("daniel@test.se", 1)]   // Matchar email
        [InlineData("test.se", 2)]          // Matchar del av email (båda)
        [InlineData("XYZ999", 0)]           // Ingen match
        public void SearchMembers_ShouldReturnExpectedCount(string searchTerm, int expectedCount)
        {
            // Arrange
            var registry = new MemberRegistry();
            registry.AddMember(new Member("M001", "Daniel Aldemir", "daniel@test.se", DateTime.Now));
            registry.AddMember(new Member("M002", "Anna Svensson", "anna@test.se", DateTime.Now));

            // Act
            var result = registry.SearchMembers(searchTerm);  // Sök efter medlemmar baserat på sökord

            // Assert
            Assert.Equal(expectedCount, result.Count);  // Kontrollera att antalet matchande medlemmar är korrekt
        }
    }
}