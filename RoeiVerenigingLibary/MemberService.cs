using System.Net.Mail;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using System.Text;
using RoeiVerenigingLibary.Exceptions;

namespace RoeiVerenigingLibary;

public class MemberService(IMemberRepository repository)
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
            member = repository.Get(email, CreatePasswordHash(password));
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

    public Member Create(Member loggedInMember, string firstName, string infix, string lastName, string email,
        string password)
    {
        return Create(loggedInMember, firstName, infix, lastName, email, password, 1);
    }

    public Member Create(Member loggedInMember, string firstName, string infix, string lastName, string email,
        string password, int level)
    {
        if (!loggedInMember.Roles.Contains("beheerder"))
        {
            throw new IncorrectRightsExeption();
        }

        if (!IsValid(email))
        {
            throw new Exception("Dit is geen email adres");
        }

        Member? member;
        try
        {
            member = repository.Create(firstName, infix, lastName, email, CreatePasswordHash(password), level);
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
            repository.ChangePassword(loggedInMember.Email, CreatePasswordHash(newPassword));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    private static String CreatePasswordHash(string password)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }
    }

    private static bool IsValid(string email)
    {
        return MailAddress.TryCreate(email, out var result);
    }

    public List<Member> GetMembers()
    {
        return repository.GetMembers();
    }
}