using Moq;
using RoeiVerenigingLibary;

namespace RoeiVerenigingUnitTests
{
    public class BotenTests
    {
        [Test]
        public void Get_ReturnsListOfBoats()
        {
            // Arrange 
            // Mocking BoatRepository
            var mockBoatRepository = new Mock<IBoatRepository>();
            Boat boat = new Boat(1, true, 4);
            List<Boat> boatlist = new List<Boat>();
            boatlist.Add(boat);
            // Set up mock repository to return mocked connection
            mockBoatRepository.Setup(x => x.Getboats()).Returns(boatlist);

            // Creating BoatService with mocked repository
            var boatService = new BoatService(mockBoatRepository.Object);

            // Act
            var result = boatService.Getboats();

            Assert.That(Is.Equals(result, boatlist));
        }
    }
}
