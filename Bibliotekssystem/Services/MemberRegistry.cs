using Bibliotekssystem.Models;

namespace Bibliotekssystem.Services
{
    public class MemberRegistry
    {
        private List<Member> _members = new();
        public IReadOnlyList<Member> Members => _members;

        public void AddMember(Member member)
        {
            _members.Add(member);
        }

        public void RemoveMember(Member member)
        {
            _members.Remove(member);
        }

        public Member? FindById(string memberId)
        {
            return _members.FirstOrDefault(m => m.MemberId == memberId);
        }

        public Member? FindByEmail(string email)
        {
            return _members.FirstOrDefault(m => m.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        }

        public List<Member> GetAllMembers()
        {
            return _members.ToList();
        }
    }
}
