using System.Net.Mail;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using System.Text;
using RoeiVerenigingLibary.Exceptions;

namespace RoeiVerenigingLibary;

public class MemberService
{
    private readonly IMemberRepository _memberRepository;

    public MemberService(IMemberRepository repository)
    {
        _memberRepository = repository;
    }

    public Member Login(string email, string password)
    {
        if (!IsValid(email))
        {
            throw new Exception("Dit is geen email adres");
        }

        Member? member;
        try
        {
            member = _memberRepository.Get(email, CreatePasswordHash(password));
        }
        catch (Exception)
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
            member = _memberRepository.Create(firstName, infix, lastName, email, CreatePasswordHash(password));
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

    /**
     * This method updates the members data based on the given id. this can only be done if the loggedInMember is an admin
     */
    public Member Update(Member loggedInMember, int id, string firstName, string infix, string lastName, string email)
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
            member = _memberRepository.Update(id, firstName, infix, lastName, email);
        }
        catch (Exception)
        {
            //todo make other exception
            throw new MemberAlreadyExistsException();
        }

        return member;
    }

    /**
     * This method updates the members data based on the loggedInMember
     */
    public Member Update(Member loggedInMember, string firstName, string infix, string lastName, string email)
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
            member = _memberRepository.Update(loggedInMember.Id, firstName, infix, lastName, email);
        }
        catch (Exception)
        {
            //todo make other exception
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
            _memberRepository.ChangePassword(loggedInMember.Email, CreatePasswordHash(newPassword));
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
        return _memberRepository.GetMembers();
    }
}