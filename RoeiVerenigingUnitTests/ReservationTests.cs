using DataAccessLibrary;
using Moq;
using RoeiVerenigingLibrary;
using RoeiVerenigingLibrary.Exceptions;

namespace RoeiVerenigingUnitTests
{
    public class ReservationTests
    {
        [Test]
        public void ReservationSuccesfull()
        {
            var startTime = DateTime.Now.AddMinutes(1);
            var endTime = DateTime.Now.AddHours(1);
            //Arrange
            Member member = new Member(1, "simon", "van den", "Berg", "simon@windesheim.nl", new List<string>(), 1);
            var ReservationRepository = new Mock<IReservationRepository>();
            Reservation reservation = new Reservation(member, 4, startTime, endTime);
            ReservationRepository
                .Setup(x => x.CreateReservation(It.IsAny<Member>(), It.IsAny<int>(), It.IsAny<DateTime>(),
                    It.IsAny<DateTime>())).Returns(reservation);
            ReservationService ReservationService = new ReservationService(ReservationRepository.Object);
            //Act
            Reservation result = ReservationService.Create(member, 4, startTime, endTime);
            //Assert
            Assert.That(Equals(result, reservation));
        }

        [Test]
        public void TimeChecker_BothTimesValidAndStartBeforeEnd_ReturnsTrue()
        {
            // Arrange
            DateTime start = new DateTime(2024, 5, 7, 14, 0, 0);
            DateTime end = new DateTime(2024, 5, 7, 15, 0, 0);
            ReservationService checker = new ReservationService(new Mock<IReservationRepository>().Object);
            // Act
            bool result = checker.TimeChecker(start, end);

            // Assert
            Assert.That(result);
        }

        [Test]
        public void TimeChecker_exception()
        {
            // Arrange
            DateTime start = new DateTime(2024, 5, 7, 15, 0, 0);
            DateTime end = new DateTime(2024, 5, 7, 14, 0, 0);

            ReservationService checker = new ReservationService(new Mock<IReservationRepository>().Object);
            // Act


            // Assert

            Assert.Throws<InvalidTimeException>(() => checker.TimeChecker(start, end));
        }

        [Test]

        public void ReservationMaxTwoHours()
        {
            var start = DateTime.Now.AddMinutes(1);
            var end = DateTime.Now.AddHours(3);
            // Arrange
            Member member = new Member(1, "simon", "van den", "Berg", "simon@windesheim.nl", new List<string>(), 1);
            ReservationService reservationService = new ReservationService(new Mock<IReservationRepository>().Object);

            // Act & Assert
            Exception? ex = Assert.Throws<Exception>(() => reservationService.Create(member, 4, start, end));
            Assert.That(ex.Message, Is.EqualTo("Je kan voor maximaal 2 uur reserveren!"));
        }

        public void OnlyTwoBoatsPerMember()
        {
            //arrange
            DateTime start = new DateTime(2024, 5, 06, 3, 00, 00);
            DateTime end = new DateTime(2024, 5, 06, 4, 00, 00);

            Member member = new Member(10, "Tygo", "van den", "Berg", "Tygo@zwolle.be", new List<string>(), 1);
            ReservationService reservationService = new ReservationService(new Mock<IReservationRepository>().Object);

            //Act
            reservationService.Create(member, 3, start, end);
            reservationService.Create(member, 4, start, end);

            //Assert
            Assert.Throws<ExceededAmountOfReservationsException>(() =>
                reservationService.Create(member, 6, start, end));
        }

        public void BoatCannotBeReservedTwice()
        {
            //assign
            DateTime start = new DateTime(2024, 5, 06, 3, 00, 00);
            DateTime end = new DateTime(2024, 5, 06, 4, 00, 00);

            Member member = new Member(10, "Tygo", "van den", "Berg", "Tygo@zwolle.be", new List<string>(), 1);
            ReservationService reservationService = new ReservationService(new Mock<IReservationRepository>().Object);

            reservationService.Create(member, 2, start, end);

            Assert.Throws<ExceededAmountOfReservationsException>(() =>
                reservationService.Create(member, 2, start, end));
        }


        [Test]
        public void EditReservationWorks()
        {
            // Arrange
            Member member = new Member(1, "simon", "van den", "Berg", "simon@windesheim.nl", new List<string>(), 1);
            Reservation updatedReservation = new Reservation(member, 4, new DateTime(2024, 5, 7, 15, 0, 0),
                new DateTime(2024, 5, 7, 16, 0, 0));

            var reservationRepository = new Mock<IReservationRepository>();
            reservationRepository.Setup(x => x.ChangeReservation(1, member, 4, It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(updatedReservation);
            ReservationService reservationService = new ReservationService(reservationRepository.Object);

            // Act
            Reservation result = reservationService.ChangeReservation(1, member, 4, new DateTime(2024, 5, 7, 15, 0, 0),
                new DateTime(2024, 5, 7, 16, 0, 0));

            // Assert
            Assert.That(Equals(updatedReservation.StartTime, result.StartTime));
            Assert.That(Equals(updatedReservation.EndTime, result.EndTime));
        }

        [Test]
        public void CreateReservationSuccesfull()
        {
            DateTime startTime = DateTime.Now.AddMinutes(1);
            DateTime endTime = startTime.AddHours(1);
            Member loggedInMember = new Member(1, "Rick", "", "Hesp", "123@windesheim.be", new List<string>(), 1);
            Reservation reservation = new Reservation(loggedInMember, 3, startTime, endTime);
            var reservationRepository = new Mock<IReservationRepository>();
            reservationRepository.Setup(x => x.CreateReservation(loggedInMember, 3, startTime, endTime))
                .Returns(reservation);
            ReservationService reservationService = new ReservationService(reservationRepository.Object);
            Reservation result = reservationService.Create(loggedInMember, 3, startTime, endTime);

            Assert.That(Equals(result, reservation));
        }

        // [TestCase(new DateTime(2024, 04, 02, 2, 00, 00), new DateTime(2024, 04, 02, 2, 00, 00))]
        // [TestCase(new DateTime(2022, 04, 02, 2, 00, 00), new DateTime(2024, 04, 02, 2, 00, 00))]

        [Test]
        public void TrowsInvalidTimeException()
        {
            DateTime startTime = new DateTime(2024, 04, 02, 2, 00, 00);
            DateTime endTime = new DateTime(2024, 04, 02, 2, 00, 00);

            ReservationService service = new ReservationService(new ReservationRepository());
            Assert.Throws<InvalidTimeException>(() => service.TimeChecker(startTime, endTime));
        }

        [Test]
        public void OnlyTwoReservationsPerMember()
        {
            // Arrange
            DateTime start = DateTime.Now.AddMinutes(1);
            DateTime end = DateTime.Now.AddHours(1);
            Member member = new Member(1, "simon", "van den", "Berg", "simon@windesheim.nl", new List<string>(), 1);
            var mockRepository = new Mock<IReservationRepository>();
            mockRepository.Setup(r => r.GetAmountOfBoatsCurrRenting(member.Id)).Returns(2);
            ReservationService reservationService = new ReservationService(mockRepository.Object);

            // Act & Assert
            MaxAmountOfReservationExceeded? ex = Assert.Throws<MaxAmountOfReservationExceeded>(() =>
                reservationService.Create(member, 3, start, end));
            Assert.That(ex, Is.TypeOf<MaxAmountOfReservationExceeded>());
        }
    }
}