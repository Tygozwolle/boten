using Moq;
using RoeiVerenigingLibary;
using System.Collections.Generic;

namespace RoeiVerenigingUnitTests
{
    public class DamageTests
    {
        private Mock<IDamageRepository> _mockDamageRepository;
        private DamageService _damageService;

        [SetUp]
        public void Setup()
        {
            _mockDamageRepository = new Mock<IDamageRepository>();
            _damageService = new DamageService(_mockDamageRepository.Object);
        }

        [Test]
        public void GetAll_ReturnsAllDamageReports()
        {
            // Arrange
            var damageReports = new List<Damage>
            {
                new Damage(1, new Member(1, "John", "", "Doe", "john.doe@example.com", new List<string>(), 1),
                    new Boat(1, true, 4, 1), "description1", false, true),
                new Damage(2, new Member(2, "Jane", "", "Doe", "jane.doe@example.com", new List<string>(), 1),
                    new Boat(2, false, 2, 1), "description2", true, false)
            };
            _mockDamageRepository.Setup(repo => repo.GetAllDamageReports()).Returns(damageReports);

            // Act
            var result = _damageService.GetAll();

            // Assert
            Assert.That(Is.Equals(damageReports, result));
        }

        [Test]
        public void Update_UpdatesAndReturnsDamageReport()
        {
            // Arrange
            var updatedDamage = new Damage(1,
                new Member(1, "John", "", "Doe", "john.doe@example.com", new List<string>(), 1),
                new Boat(1, true, 4, 1), "updated description", true, true);
            _mockDamageRepository.Setup(repo => repo.Update(1, true, true, "updated description"))
                .Returns(updatedDamage);

            // Act
            var result = _damageService.Update(1, true, true, "updated description");

            // Assert
            Assert.That(Is.Equals(updatedDamage, result));
        }

        [Test]
        public void GetById_ReturnsDamageReportById()
        {
            // Arrange
            var damage = new Damage(1, new Member(1, "John", "", "Doe", "john.doe@example.com", new List<string>(), 1),
                new Boat(1, true, 4, 1), "description", false, true);
            _mockDamageRepository.Setup(repo => repo.GetById(1)).Returns(damage);

            // Act
            var result = _damageService.GetById(1);

            // Assert
            Assert.That(Is.Equals(damage, result));
        }

        [Test]
        public void GetRelatedToUser_ReturnsDamageReports_WhenItemsAreFound()
        {
            // Arrange
            Member member = new Member(1, "John", "", "Doe", "john.doe@example.com", new List<string>(), 1);
            var damageReports = new List<Damage>
            {
                new Damage(1, member,
                    new Boat(1, true, 4, 1), "description1", false, true),
                new Damage(2, member ,new Boat(2, false, 2, 1), "description2", true, false)
            };
            _mockDamageRepository.Setup(repo => repo.GetRelatedToUser(member.Id)).Returns(damageReports);

            // Act
            var result = _damageService.GetRelatedToUser(member);

            // Assert
            Assert.That(Is.Equals(damageReports, result));
        }

        [Test]
        public void GetRelatedToUser_ReturnsEmpty_WhenNoItemsFound()
        {
            // Arrange
            Member member = new Member(1, "John", "", "Doe", "john.doe@example.com", new List<string>(), 1);
            var damageReports = new List<Damage>();
            _mockDamageRepository.Setup(repo => repo.GetRelatedToUser(member.Id)).Returns(damageReports);

            // Act
            var result = _damageService.GetRelatedToUser(member);

            // Assert
            Assert.That(result.Count == 0);
        }
    }
}