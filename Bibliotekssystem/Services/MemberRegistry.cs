using Bibliotekssystem.Models;

namespace Bibliotekssystem.Services
{
    public class MemberRegistry
    {
        // Intern lista för att lagra medlemmar
        private List<Member> _members = new();

        // Offentlig egenskap för att få tillgång till medlemmarna som en läsbar lista
        public IReadOnlyList<Member> Members => _members;

        public void AddMember(Member member)
        {
            // Lägg till en medlem i registret
            _members.Add(member);
        }

        public void RemoveMember(Member member)
        {   // Ta bort en medlem från registret
            _members.Remove(member);
        }

        public Member? FindById(string memberId)
        {
            // Hitta en medlem baserat på dess medlems-ID
            return _members.FirstOrDefault(m => m.MemberId == memberId);
        }

        public Member? FindByEmail(string email)
        {
            // Hitta en medlem baserat på dess e-postadress
            return _members.FirstOrDefault(m => m.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        }

        public List<Member> GetAllMembers()
        {
            // Returnerar alla medlemmar i registret
            return _members.ToList();
        }
    }
}
