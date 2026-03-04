using System.ComponentModel.DataAnnotations;
using Bibliotekssystem.Core.Interfaces;

namespace Bibliotekssystem.Core.Models
{
    public class Member : ISearchable
    {
        // Primärnyckel
        public int Id { get; set; }

        [Required(ErrorMessage = "Medlems-ID är obligatoriskt")]
        [StringLength(20, ErrorMessage = "Medlems-ID får max vara 20 tecken")]
        public string MemberId { get; set; } = string.Empty;

        [Required(ErrorMessage = "Namn är obligatoriskt")]
        [StringLength(100, ErrorMessage = "Namn får max vara 100 tecken")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "E-post är obligatoriskt")]
        [EmailAddress(ErrorMessage = "Ogiltig e-postadress")]
        [StringLength(150, ErrorMessage = "E-post får max vara 150 tecken")]
        public string Email { get; set; } = string.Empty;

        public DateTime MemberSince { get; set; } = DateTime.Now;

        // Navigation property - en medlem kan ha många lån
        public ICollection<Loan> Loans { get; set; } = new List<Loan>();

        // Parameterlös konstruktor - krävs av EF Core
        public Member() { }

        // Bakåtkompatibel konstruktor från Del 1
        public Member(string memberId, string name, string email, DateTime memberSince)
        {
            MemberId = memberId;
            Name = name;
            Email = email;
            MemberSince = memberSince;
        }

        public string GetInfo()
        {
            return $"Member ID: {MemberId}, Name: {Name}, Email: {Email}, Member Since: {MemberSince.ToShortDateString()}";
        }

        public bool Matches(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();
            return MemberId.ToLower().Contains(searchTerm) ||
                   Name.ToLower().Contains(searchTerm) ||
                   Email.ToLower().Contains(searchTerm) ||
                   MemberSince.ToString("d").Contains(searchTerm);
        }
    }
}