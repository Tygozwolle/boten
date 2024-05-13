namespace RoeiVerenigingLibary;

public interface IMemberRepository
{
    public Member Get(string email, string passwordHash);

    public Member Create(string firstName, string infix, string lastName, string email, string passwordHash);

    public Member Update(int id, string firstName, string infix, string lastName, string email);
    public void ChangePassword(string email, string newPasswordHash);
    public List<Member> GetMembers();
}