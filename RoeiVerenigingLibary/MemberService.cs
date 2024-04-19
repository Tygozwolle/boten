namespace RoeiVerenigingLibary;

public class MemberService
{

    private IMemberRepository _memberRepository;

    public MemberService(IMemberRepository repository)
    {
        _memberRepository = repository;
    }
    public Member login(string email, string password)
    {
        Member? member;
        try
        {
            member = _memberRepository.login(email, password);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }




        return member;
    }
}