using DataAccessLibary;
using MySqlConnector;
using RoeiVerenigingLibary;
using System.Diagnostics;


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

            //   ImageRepository imageRepository = new ImageRepository();
            //       EmailToDb.GetImagesFromEmail(imageRepository);
            //     var imaList = imageRepository.get(1);
            //     
            //   var test =  imageRepository.get(1);
            DamageRepository damageRepository = new DamageRepository();
            var st = new Stopwatch();
            st.Start();
           var test = damageRepository.GetById(1);
            st.Stop();
            Console.WriteLine(st);
            test.ToString();
            //var stream = imaList[0];
            //var fileStream = File.Create($"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}/imageDb.png");
            //stream.Seek(0, SeekOrigin.Begin);
            //stream.CopyTo(fileStream);
            //fileStream.Close();


            //DamageService service = new DamageService(new DamageRepository());
            //service.Update(1, true, true, "boot heel");
            // service.GetAllDamageReports();
        }
    }
}