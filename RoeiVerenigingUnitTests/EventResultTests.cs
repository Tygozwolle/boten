using NUnit.Framework;
using Moq;
using RoeiVerenigingLibrary;
using RoeiVerenigingLibrary.Exceptions;
using RoeiVerenigingLibrary.Model;

namespace RoeiVerenigingUnitTests;

public class EventResultTests
{
    [Test]
    public void SaveResultTime_WithBeheerderRole_CallsSaveTimeOnRepository()
    {
        // Arrange
        var mockRepository = new Mock<IEventResultRepository>();
        Member member = new Member(1, "John", "", "Doe", "john.doe@example.com", new List<string> { "beheerder" }, 1);
        var eventParticipant = new EventParticipant(member, 1, new TimeSpan(2, 0, 0), "Test Description");
        mockRepository.Setup(r => r.SaveTime(It.IsAny<EventParticipant>()));

        // Act
        eventParticipant.saveResultTime(mockRepository.Object, member);

        // Assert
        mockRepository.Verify(r => r.SaveTime(eventParticipant), Times.Once);
    }

    [Test]
    public void SaveResultTime_WithMateriaalCommissarisRole_CallsSaveTimeOnRepository()
    {
        // Arrange
        var mockRepository = new Mock<IEventResultRepository>();
        Member member = new Member(1, "John", "", "Doe", "john.doe@example.com",
            new List<string> { "materiaal_commissaris" }, 1);
        var eventParticipant = new EventParticipant(member, 1, new TimeSpan(2, 0, 0), "Test Description");
        mockRepository.Setup(r => r.SaveTime(It.IsAny<EventParticipant>()));

        // Act
        eventParticipant.saveResultTime(mockRepository.Object, member);

        // Assert
        mockRepository.Verify(r => r.SaveTime(eventParticipant), Times.Once);
    }

    [Test]
    public void SaveResultTime_WithoutRequiredRoles_ThrowsException()
    {
        // Arrange
        var mockRepository = new Mock<IEventResultRepository>();
        Member member = new Member(1, "John", "", "Doe", "john.doe@example.com", new List<string>(), 1);
        var eventParticipant = new EventParticipant(member, 1, new TimeSpan(2, 0, 0), "Test Description");
        mockRepository.Setup(r => r.SaveTime(It.IsAny<EventParticipant>()));

        // Act & Assert
        Assert.Throws<IncorrectRightsException>(() => eventParticipant.saveResultTime(mockRepository.Object, member));
    }
}