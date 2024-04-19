namespace RoeiVerenigingLibary;

public interface IMemberRepository
{
    public Member Get(string email, string passwordHash);

    public Member Create(string firstName, string lastName, string email, string passwordHash);
}