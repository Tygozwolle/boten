namespace RoeiVerenigingLibary;

public interface IMemberRepository
{
    public Member Get(string email, string passwordHash);

    public Member Create(string firstName, string infix, string lastName, string email, string passwordHash);

    public void ChangePassword(string email, string newPasswordHash);
}