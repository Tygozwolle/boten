using RoeiVerenigingLibrary.Exceptions;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;

namespace RoeiVerenigingLibrary
{
    public class MemberService(IMemberRepository memberRepository)
    {
        public Member Login(string email, string password)
        {
            if (!IsValid(email))
            {
                throw new InvalidEmailException();
            }

            Member? member;
            try
            {
                member = memberRepository.Get(email, CreatePasswordHash(password));
            }
            catch (Exception e)
            {
                throw new IncorrectEmailOrPasswordException();
            }

            if (member == null)
            {
                throw new IncorrectEmailOrPasswordException();
            }

            return member;
        }

        public Member GetById(int id)
        {
            Member? member;
            try
            {
                member = memberRepository.GetById(id);
            }
            catch (Exception)
            {
                //todo replace with new exception
                throw new IncorrectEmailOrPasswordException();
            }

            if (member == null)
            {
                //todo replace with new exception
                throw new IncorrectEmailOrPasswordException();
            }

            return member;
        }

        public Member Create(Member loggedInMember, string firstName, string infix, string lastName, string email,
            string password)
        {
            return Create(loggedInMember, firstName, infix, lastName, email, password, 1);
        }

        private Member Create(Member loggedInMember, string firstName, string infix, string lastName, string email,
            string password, int level)
        {
            if (!loggedInMember.Roles.Contains("beheerder"))
            {
                throw new IncorrectRightsExeption();
            }

            if (!IsValid(email))
            {
                throw new InvalidEmailException();
            }

            Member? member;
            try
            {
                member = memberRepository.Create(firstName, infix, lastName, email, CreatePasswordHash(password),
                    level);
            }
            catch (Exception)
            {
                throw new MemberAlreadyExistsException();
            }

            if (member == null)
            {
                throw new MemberAlreadyExistsException();
            }

            return member;
        }

        public Member Update(Member loggedInMember, int id, string firstName, string infix, string lastName,
            string email,
            int level)
        {
            if (!loggedInMember.Roles.Contains("beheerder"))
            {
                throw new IncorrectRightsExeption();
            }

            if (!IsValid(email))
            {
                throw new Exception("Dit is geen email adres");
            }

            if (level > 10)
            {
                throw new Exception("Niveau mag maximaal 10 zijn");
            }

            Member? member;
            try
            {
                member = memberRepository.Update(id, firstName, infix, lastName, email, level);
            }
            catch (Exception)
            {
                throw new CantAccesDatabaseException();
            }

            return member;
        }
        
        public Member Update(Member loggedInMember, string firstName, string infix, string lastName, string email)
        {
            if (!IsValid(email))
            {
                throw new Exception("Dit is geen email adres");
            }

            Member? member;
            try
            {
                member = memberRepository.Update(loggedInMember.Id, firstName, infix, lastName, email);
            }
            catch (Exception)
            {
                throw new CantAccesDatabaseException();
            }

            return member;
        }

        public void SetRoles(int memberId, List<string> roles)
        {
            memberRepository.RemoveRoles(memberId);
            foreach (string role in roles)
            {
                memberRepository.AddRole(memberId, role);
            }
        }

        public List<string> GetAvailableRoles()
        {
            return memberRepository.GetAvailableRoles();
        }

        public void ChangePassword(Member loggedInMember, string currentPassword, string newPassword,
            string newPasswordConfirm)
        {
            try
            {
                Login(loggedInMember.Email, currentPassword);
            }
            catch (IncorrectEmailOrPasswordException)
            {
                throw new IncorrectPasswordException();
            }

            if (newPassword != newPasswordConfirm)
            {
                throw new PasswordsDontMatchException();
            }

            try
            {
                memberRepository.ChangePassword(loggedInMember.Email, CreatePasswordHash(newPassword));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private static string CreatePasswordHash(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        private static bool IsValid(string email)
        {
            return MailAddress.TryCreate(email, out MailAddress? result);
        }

        public List<Member> GetMembers()
        {
            return memberRepository.GetMembers();
        }
    }
}