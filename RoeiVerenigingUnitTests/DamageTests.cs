using Moq;
using RoeiVerenigingLibrary.Interfaces;
using RoeiVerenigingLibrary.Model;
using RoeiVerenigingLibrary.Services;

namespace RoeiVerenigingUnitTests;

public class DamageTests
{
    private DamageService _damageService;
    private Mock<IDamageRepository> _mockDamageRepository;

    [SetUp]
    public void Setup()
    {
        _mockDamageRepository = new Mock<IDamageRepository>();
        _damageService = new DamageService(_mockDamageRepository.Object);
    }

    [Test]
    public void GetRelatedToUser_ReturnsEmpty_WhenNoItemsFound()
    {
        // Arrange
        var member = new Member(1, "John", "", "Doe", "john.doe@example.com", new List<string>(), 1);
        var damageReports = new List<Damage>();
        _mockDamageRepository.Setup(repo => repo.GetRelatedToUser(member)).Returns(damageReports);

        // Act
        var result = _damageService.GetRelatedToUser(member);

        // Assert
        Assert.That(result.Count == 0);
    }
}