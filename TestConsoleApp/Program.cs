
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
            Member tygo = memberService.Login("tygo@windesheim.be", "Test123$");
            Console.WriteLine(tygo.FirstName);
            Console.WriteLine(tygo.Roles.Count);


            Member newMember = memberService.Create(tygo, "gerard", "Jonh", "hoi@doei.be", "Test123$");
            Console.WriteLine(newMember.FirstName);
        }
    }
}