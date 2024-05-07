using Microsoft.Extensions.Configuration;
using Moq;
using RoeiVerenigingLibary;
using RoeiVerenigingLibary.Exceptions;


namespace RoeiVerenigingUnitTests
{
    public class ReservationTests
    {
       [Test]
        public void ReservationSuccesfull()
        {
            //Arrange
            var member = new Member(1, "simon", "van den", "Berg", "simon@windesheim.nl", new List<string>());
            var ReservationRepository = new Mock<IReservationRepository>();
            Reservation reservation = new Reservation(member, 4, new DateTime(3), new DateTime(4));
            ReservationRepository.Setup(x => x.CreateReservation(It.IsAny<Member>(), It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(reservation);
            var ReservationService = new ReservationService(ReservationRepository.Object);
            //Act
             var result = ReservationService.Create(member, 4, new DateTime(3), new DateTime(4));
            //Assert
             Assert.That(Is.Equals(result, reservation));
        }
        [Test]
        public void TimeChecker_BothTimesValidAndStartBeforeEnd_ReturnsTrue()
        {
            // Arrange
            var start = new DateTime(2024, 5, 7, 14, 0, 0);
            var end = new DateTime(2024, 5, 7, 15, 0, 0);
            var checker = new ReservationService(new Mock<IReservationRepository>().Object); 
            // Act
            bool result = checker.TimeChecker(start, end);

            // Assert
            Assert.That(result);
        }
        [Test]
        public void TimeChecker_exeption()
        {
            // Arrange
            var start = new DateTime(2024, 5, 7, 15, 0, 0);
            var end = new DateTime(2024, 5, 7, 14, 0, 0);
            
            var checker = new ReservationService(new Mock<IReservationRepository>().Object);
            // Act
            

            // Assert

            Assert.Throws< InvalidTimeException >( () =>checker.TimeChecker(start, end));
        }
    }
}