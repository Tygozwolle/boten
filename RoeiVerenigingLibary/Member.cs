namespace RoeiVerenigingLibary;

public class Member
{
    public string FirstName { get; set; }
    public string LastName { get; set; }

    public string Email { get; set; }

    public Member(string firstName, string lastname, string email)
    {
        FirstName = firstName;
        LastName = lastname;
        Email = email;
    }
}