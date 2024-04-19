
using DataAccessLibary;
using MySqlConnector;


using RoeiVerenigingLibary;


namespace TestConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            MemberService memberService = new MemberService(new MemberRepository());
            Member tygo = memberService.Login("simon@will.roeien.nl", "Test123$");
            Console.WriteLine(tygo.FirstName);
        }
    }
}