namespace RoeiVerenigingLibary;

public class Member
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    
    public string Infix { get; set; }
    public string LastName { get; set; }

    public string Email { get; set; }
    public List<string> Roles {get;}

    public Member(int id,string firstName, string infix, string lastname, string email, List<string> roles)
    {
        Id = id;
        FirstName = firstName;
        Infix = infix;
        LastName = lastname;
        Email = email;
        Roles = roles;
    }
}