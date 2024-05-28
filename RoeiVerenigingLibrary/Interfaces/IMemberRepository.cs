namespace RoeiVerenigingLibrary
{
    public interface IMemberRepository
    {
        public Member Get(string email, string passwordHash);

        public Member GetById(int id);
        public Member Update(int id, string firstName, string infix, string lastName, string email);

        public Member Update(int id, string firstName, string infix, string lastName, string email, int level);

        public Member Create(string firstName, string infix, string lastName, string email, string passwordHash, int level);
        public void ChangePassword(string email, string newPasswordHash);
        public List<Member> GetMembers();

        public void AddRole(int memberId, string role);
        public void RemoveRoles(int memberId);

        public List<string> GetAvailableRoles();
    }
}