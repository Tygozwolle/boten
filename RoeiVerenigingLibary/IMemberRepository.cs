namespace RoeiVerenigingLibary;

public interface IMemberRepository
{
    public Member Get(string email, string passwordHash);

    public Member Create(string firstName, string infix, string lastName, string email, string passwordHash);
    public List<Member> GetMembers();
}