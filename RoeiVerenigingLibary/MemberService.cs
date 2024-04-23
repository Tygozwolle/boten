using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using System.Text;
using RoeiVerenigingLibary.Exceptions;

namespace RoeiVerenigingLibary;

public class MemberService
{

    private IMemberRepository _memberRepository;

    public MemberService(IMemberRepository repository)
    {
        _memberRepository = repository;
    }
    public Member Login(string email, string password)
    {
        Member? member;
        try
        {
            member = _memberRepository.Get(email, CreatePasswordHash(password));
        }
        catch (Exception)
        {
            throw new IncorrectEmailOrPasswordException();
        }

        if(member == null)
            throw new IncorrectEmailOrPasswordException();


        return member;
    }

    public Member Create(Member LoggedInMember, string firstName, string lastName, string email, string password)
    {
        if(!LoggedInMember.Roles.Contains("beheerder"))
        {
            throw new IncorrectRightsExeption();
        }

        Member? member;
        try
        {
            member = _memberRepository.Create(firstName, lastName, email, CreatePasswordHash(password));
        }
        catch (Exception)
        {
            throw new MemberAlreadyExistsException();
        }
        if(member == null)
        {
            throw new MemberAlreadyExistsException();
        }
        return member;
    }

    private static String CreatePasswordHash(string password)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }
    }
}