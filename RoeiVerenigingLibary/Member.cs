namespace RoeiVerenigingLibary;

public class Member
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }

    public string Email { get; set; }

    public Member(int id,string firstName, string lastname, string email)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastname;
        Email = email;
    }
}