namespace RoeiVerenigingLibary;

public interface IMemberRepository
{
    public Member Get(string email, string passwordHash);

    public Member Create(string firstName, string infix, string lastName, string email, string passwordHash);
    public Member Create(string firstName, string infix, string lastName, string email, string passwordHash, int level);
    public void ChangePassword(string email, string newPasswordHash);
    public List<Member> GetMembers();
}