namespace RoeiVerenigingLibary;

public class Member
{
    public int Id { get; set; }
    public string FirstName { get; set; }

    public string Infix { get; set; }
    public string LastName { get; set; }

    public string Email { get; set; }
    public  int Level { get; set; }
    public List<string> Roles { get; }

    public string RolesString
    {
        get
        {
            string result = "";
            foreach (string role in Roles)
            {
                result = result.Insert(result.Length, $"{role} {Environment.NewLine}");
            }

            result = result.Trim();
            return result;
        }
    }

    public string FullName
    {
        get { return $"{FirstName} {Infix} {LastName}"; }
    }

    public Member(int id, string firstName, string infix, string lastName, string email, List<string> roles, int level)
    {
        Id = id;
        FirstName = firstName;
        Infix = infix;
        LastName = lastName;
        Email = email;
        Roles = roles;
        Level = level;
    }
}