using DataAccessLibary;
using Microsoft.Extensions.Configuration;
using Moq;
using RoeiVerenigingLibary;
using RoeiVerenigingLibary.Exceptions;

public class ReservationUnitTests
{
    [SetUp]
    public void setup(){}

    
    [Test]
    public void CreateReservationSuccesfull()
    {
        var startTime = new DateTime(2024, 04, 02, 3, 00, 00);
        var endTime = new DateTime(2024, 04, 02, 4, 00, 00);
        var loggedInMember = new Member(1, "Rick", "", "Hesp", "123@windesheim.be", new List<string>());
        var reservation = new Reservation(loggedInMember, 3, startTime, endTime);
        var reservationRepository = new Mock<IReservationRepository>();
        reservationRepository.Setup(x => x.CreateReservation(loggedInMember, 3, startTime, endTime))
            .Returns(reservation);
        var reservationService = new ReservationService(new ReservationRepository());
        var result = reservationService.Create(loggedInMember, 3, startTime, endTime);
        
        Assert.That(Is.Equals(result, reservation));
    }

    // [TestCase(new DateTime(2024, 04, 02, 2, 00, 00), new DateTime(2024, 04, 02, 2, 00, 00))]
    // [TestCase(new DateTime(2022, 04, 02, 2, 00, 00), new DateTime(2024, 04, 02, 2, 00, 00))]
    
    [Test]
    public void TrowsInvalidTimeException()
    {
        var startTime = new DateTime(2024, 04, 02, 2, 00, 00);
        var endTime = new DateTime(2024, 04, 02, 2, 00, 00);
        
        var service = new ReservationService(new ReservationRepository());
        // var error = service.TimeChecker(startTime, endTime);
        Assert.Throws<InvalidTimeException>(() => service.TimeChecker(startTime, endTime));
    }
    
    [Test]
    public void reservationOnlyLastsTwoHours()
    {
        
        
    }

    [Test]
    public void OnlyTwoReservationsPerMember()
    {
        
    }
    
        
//supporting functions
    
}