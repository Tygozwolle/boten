using System.Reflection;

namespace RoeiVerenigingLibrary
{
    public class Member
    {
        public Member(int id, string firstName, string infix, string lastName, string email, List<string> roles,
            int level)
        {
            Id = id;
            FirstName = firstName;
            Infix = infix;
            LastName = lastName;
            Email = email;
            Roles = roles;
            Level = level;
        }
        public Member(int id, string firstName, string infix, string lastName, string email, 
            int level)
        {
            Id = id;
            FirstName = firstName;
            Infix = infix;
            LastName = lastName;
            Email = email;
            Roles = new List<string>();
            Level = level;
        }

        public int Id { get; set; }
        public string FirstName { get; set; }

        public string Infix { get; set; }
        public string LastName { get; set; }

        public string Email { get; set; }
        public int Level { get; set; }
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

        public string GetFullName()
        {
            return FirstName + " " + Infix + " " + LastName;
        }
        
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            Member other = (Member)obj;
            return Id == other.Id;
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}