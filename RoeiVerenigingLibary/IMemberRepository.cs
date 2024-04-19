namespace RoeiVerenigingLibary;

public interface IMemberRepository
{
    public Member login(String email, String password);
}