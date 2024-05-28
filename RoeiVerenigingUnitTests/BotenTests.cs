using Moq;
using RoeiVerenigingLibrary;
using RoeiVerenigingLibrary.Exceptions;

namespace RoeiVerenigingUnitTests
{
    public class BotenTests
    {
        private Mock<IBoatRepository> _mockBoatRepository;
        private BoatService _boatService;
        
        [SetUp]
        public void SetUp()
        {
            _mockBoatRepository = new Mock<IBoatRepository>();
            _boatService = new BoatService(_mockBoatRepository.Object);
        }
        [Test]
        public void Create_WithAdminRole_CreatesBoat()
        {
            // Arrange
            var member = new Member(1, "tygo", "van", "olst", "tygo@mail.nl",new List<string>{ "beheerder" }, 10 ) ;

            // Act
            var result = _boatService.Create(member, "Test Boat", "Test Description", 5, true, 1);

            // Assert
            _mockBoatRepository.Verify(r => r.Create(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<int>()), Times.Once);
        }

        [Test]
        public void Create_WithMaterialCommissarisRole_CreatesBoat()
        {
            // Arrange
            var member = new Member( 1, "tygo", "van", "olst", "tygo@mail.nl",new List<string>{ "materiaal_commissaris" }, 10 );

            // Act
            var result = _boatService.Create(member, "Test Boat", "Test Description", 5, true, 1);

            // Assert
            _mockBoatRepository.Verify(r => r.Create(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<int>()), Times.Once);
        }

        [Test]
        public void Create_WithoutRequiredRole_ThrowsException()
        {
            // Arrange
            var member = new Member( 1, "tygo", "van", "olst", "tygo@mail.nl",new List<string>(), 10 );

            // Act & Assert
            Assert.Throws<IncorrectRightsExeption>(() => _boatService.Create(member, "Test Boat", "Test Description", 5, true, 1));
            _mockBoatRepository.Verify(r => r.Create(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<int>()), Times.Never);
        }
    }
}