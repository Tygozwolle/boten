using Castle.Components.DictionaryAdapter;
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
            var member = new Member(1, "simon", "van den", "Berg", "simon@windesheim.nl", new List<string>(), 1);
            var ReservationRepository = new Mock<IReservationRepository>();
            Reservation reservation = new Reservation(member, 4, new DateTime(3), new DateTime(4));
            ReservationRepository
                .Setup(x => x.CreateReservation(It.IsAny<Member>(), It.IsAny<int>(), It.IsAny<DateTime>(),
                    It.IsAny<DateTime>())).Returns(reservation);
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
        public void TimeChecker_exception()
        {
            // Arrange
            var start = new DateTime(2024, 5, 7, 15, 0, 0);
            var end = new DateTime(2024, 5, 7, 14, 0, 0);

            var checker = new ReservationService(new Mock<IReservationRepository>().Object);
            // Act


            // Assert

            Assert.Throws<InvalidTimeException>(() => checker.TimeChecker(start, end));
        }

        [TestCase(14, 18)]
        [TestCase(10, 14)]
        [TestCase(2, 5)]
        public void ReservationMaxTwoHours(int startTime, int endTime)
        {
            //Arrange
            var start = new DateTime(2024, 5, 7, startTime, 0, 0);
            var end = new DateTime(2024, 5, 7, endTime, 0, 0);
            var checker = new ReservationService(new Mock<IReservationRepository>().Object);
            
            //Assert
            Assert.Throws<InvalidTimeException>(() => checker.TimeChecker(start, end));
        }

        public void OnlyTwoBoatsPerMember()
        {
            //arrange
            var start = new DateTime(2024, 5, 06, 3, 00, 00);
            var end = new DateTime(2024, 5, 06, 4, 00, 00);

            var member = new Member(10, "Tygo", "van den", "Berg", "Tygo@zwolle.be", new List<string>(), 1);
            var reservationService = new ReservationService(new Mock<IReservationRepository>().Object);
            
            //Act
            reservationService.Create(member, 3, start, end);
            reservationService.Create(member, 4, start, end);
            
            //Assert
            Assert.Throws<ExceededAmountOfReservationsException>(() =>
                reservationService.Create(member,6, start, end));
        }

        public void BoatCannotBeReservedTwice()
        {
            
            //assign
            var start = new DateTime(2024, 5, 06, 3, 00, 00);
            var end = new DateTime(2024, 5, 06, 4, 00, 00);

            var member = new Member(10, "Tygo", "van den", "Berg", "Tygo@zwolle.be", new List<string>(), 1);
            var reservationService = new ReservationService(new Mock<IReservationRepository>().Object);

            reservationService.Create(member, 2, start, end);

            Assert.Throws<ExceededAmountOfReservationsException>(() =>
                reservationService.Create(member, 2, start, end));
        }
        
        
    }
}