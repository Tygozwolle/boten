using DataAccessLibrary;
using MySqlConnector;
using RoeiVerenigingLibrary;
using RoeiVerenigingLibrary.Model;
using System.Diagnostics;
using RoeiVerenigingLibrary.Model;
using RoeiVerenigingLibrary.Services;

namespace TestConsoleApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {

            // EventService eventService = new EventService(new EventRepository());
            // Event ev = eventService.GetEventById(1);
            // List<Event> evs = eventService.GetEventsFromPastMonths(3);
            // foreach (var e in evs)
            // {
            //     Console.WriteLine(e.Name);
            // }
            //
            //
            //
            // ev.AddParticipantsFromDatabase(new EventResultRepository());
            // foreach (var participant in ev.Participants)
            // {
            //     Console.WriteLine("Event: " + participant.EventId + ", " + participant.FirstName + " " +
            //                       participant.Infix + " " + participant.LastName + ": " +
            //                       participant.ResultTime);
            //     participant.ResultTime = new TimeSpan(1, 8, 1);
            //     participant.saveResultTime(new EventResultRepository());
            // }


            //
            // var EventRepository = new EventRepository();
            // var events = EventRepository.Get(1);
            // var list = EventRepository.GetEventReservationsIds(events);
            // foreach (var VARIABLE in list)
            // {
            //     Console.Write($"{VARIABLE},");
            // }
            // Console.WriteLine(events);
            

            //Config.DBAdress = "adress";
            //Config.SetDBPassword("secret");
            //Config.SetDBUsername("username");
            //Config.SetDBPort("22");
            //Config.SetControlUsername("admin");
            //Config.SetControlPassword("password");
            //Console.WriteLine(Config.GetDBAdress());
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
            //DamageRepository damageRepository = new DamageRepository();
            //Stopwatch st = new Stopwatch();
            //st.Start();
            //Damage test = damageRepository.GetById(1);
            //st.Stop();
            //Console.WriteLine(st);
            //test.ToString();
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