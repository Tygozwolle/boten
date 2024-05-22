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
            Reservation reservation = new Reservation(member, 4, DateTime.Now, DateTime.Now.AddHours(1));
            ReservationRepository
                .Setup(x => x.CreateReservation(It.IsAny<Member>(), It.IsAny<int>(), It.IsAny<DateTime>(),
                    It.IsAny<DateTime>())).Returns(reservation);
            var ReservationService = new ReservationService(ReservationRepository.Object);
            //Act
            var result = ReservationService.Create(member, 4, DateTime.Now, DateTime.Now.AddHours(1));
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

            Assert.Throws<InvalidTimeException>(() => checker.TimeChecker(start, end));
        }

        [Test]
        public void CreateReservation_ReservationExceedsMaxTime_ThrowsException()
        {
            // Arrange
            var reservationRepositoryMock = new Mock<IReservationRepository>();
            var reservationService = new ReservationService(reservationRepositoryMock.Object);
            var member = new Member(1, "simon", "van den", "Berg", "simon@windesheim.nl", new List<string>(), 1);
            DateTime startTime = DateTime.Now;
            DateTime endTime = startTime.AddHours(3); // Exceeds max reservation time of 2 hours

            // Act & Assert
            Assert.Throws<Exception>(() => reservationService.Create(member, 1, startTime, endTime));
        }

        [Test]
        public void CreateReservation_ReservationMoreThanTwoWeeksInAdvance_ThrowsException()
        {
            // Arrange
            var reservationRepositoryMock = new Mock<IReservationRepository>();
            var reservationService = new ReservationService(reservationRepositoryMock.Object);
            var member = new Member(1, "simon", "van den", "Berg", "simon@windesheim.nl", new List<string>(), 1);
            DateTime startTime = DateTime.Now.AddDays(15); // More than 2 weeks in advance
            DateTime endTime = startTime.AddHours(1);

            // Act & Assert
            Assert.Throws<Exception>(() => reservationService.Create(member, 1, startTime, endTime));
        }

        [Test]
        public void CreateReservation_ReservationInPast_ThrowsException()
        {
            // Arrange
            var reservationRepositoryMock = new Mock<IReservationRepository>();
            var reservationService = new ReservationService(reservationRepositoryMock.Object);
            var member = new Member(1, "simon", "van den", "Berg", "simon@windesheim.nl", new List<string>(), 1);
            DateTime startTime = DateTime.Now.AddDays(-1); // In the past
            DateTime endTime = startTime.AddHours(1);

            // Act & Assert
            Assert.Throws<Exception>(() => reservationService.Create(member, 1, startTime, endTime));
        }

        [Test]
        public void CreateReservation_ReservationNotInDaylight_ThrowsException()
        {
            // Arrange
            var reservationRepositoryMock = new Mock<IReservationRepository>();
            var reservationService = new ReservationService(reservationRepositoryMock.Object);
            var member = new Member(1, "simon", "van den", "Berg", "simon@windesheim.nl", new List<string>(), 1);
            DateTime now = DateTime.Now;
            DateTime startTime = new DateTime(now.Year, now.Month, now.Day, 22, 0, 0); // Not in daylight
            DateTime endTime = startTime.AddHours(1);

            // Act & Assert
            Assert.Throws<ReservationNotInDaylightException>(() =>
                reservationService.Create(member, 1, startTime, endTime));
        }

        [Test]
        public void CreateReservation_MemberAlreadyRentingTwoBoats_ThrowsException()
        {
            // Arrange
            var reservationRepositoryMock = new Mock<IReservationRepository>();
            var reservationService = new ReservationService(reservationRepositoryMock.Object);
            var member = new Member(1, "simon", "van den", "Berg", "simon@windesheim.nl", new List<string>(), 1);
            DateTime startTime = DateTime.Now;
            DateTime endTime = startTime.AddHours(1);

            reservationRepositoryMock.Setup(r => r.GetAmountOfBoatsCurrRenting(member.Id))
                .Returns(2); // Member is already renting 2 boats

            // Act & Assert
            Assert.Throws<MaxAmountOfReservationExceeded>(
                () => reservationService.Create(member, 1, startTime, endTime));
        }

        [Test]
        public void CreateReservation_BoatAlreadyReserved_ThrowsException()
        {
            // Arrange
            var reservationRepositoryMock = new Mock<IReservationRepository>();
            var reservationService = new ReservationService(reservationRepositoryMock.Object);
            var member = new Member(1, "simon", "van den", "Berg", "simon@windesheim.nl", new List<string>(), 1);
            DateTime startTime = DateTime.Now;
            DateTime endTime = startTime.AddHours(1);

            reservationRepositoryMock.Setup(r => r.BoatAlreadyReserved(1, startTime, endTime))
                .Returns(true); // Boat is already reserved

            // Act & Assert
            Assert.Throws<BoatAlreadyReservedException>(() => reservationService.Create(member, 1, startTime, endTime));
        }
    }
}