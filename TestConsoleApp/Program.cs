using DataAccessLibary;
using MySqlConnector;
using RoeiVerenigingLibary;


namespace TestConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
        //Console.WriteLine("Hello, World!");
        //MemberService memberService = new MemberService(new MemberRepository());
        //Member tygo = memberService.Login("tygo@windesheim.be", "Test123$");
        //Console.WriteLine(tygo.FirstName);
        //Console.WriteLine(tygo.Roles.Count);
        //List<Member> member = memberService.GetMembers();
        //var reservation = new ReservationRepository();
        //var res = reservation.GetReservations();
      //  C: \Users\Gebruiker\Downloads\imageDB
       EmailToDb test = new EmailToDb();
           foreach (var attachment in test.AttachmentsList)
           {
               ImageRepository.Add(1, attachment.ContentStream);
           }

           var imaList = ImageRepository.get(1);
          var stream = imaList[0];
          var fileStream = File.Create($"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}/imageDb.png");
          stream.Seek(0, SeekOrigin.Begin);
         stream.CopyTo(fileStream);
          fileStream.Close();
        }
    }
}