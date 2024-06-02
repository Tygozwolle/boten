using NUnit.Framework;
using Moq;
using RoeiVerenigingLibrary;
using RoeiVerenigingLibrary.Model;

namespace RoeiVerenigingUnitTests;

public class EventResultTests
{
    [Test]
    public void SaveResultTime_CallsSaveTimeOnRepositoryWithCorrectParameters()
    {
        // Arrange
        var mockRepository = new Mock<IEventResultRepository>();
        Member member = new Member(1, "John", "", "Doe", "john.doe@example.com", new List<string>(), 1);
        var eventParticipant = new EventParticipant(member, 1, new TimeSpan(2, 0, 0), "Test Description");
        mockRepository.Setup(r => r.SaveTime(It.IsAny<EventParticipant>()));
        // Act
        eventParticipant.saveResultTime(mockRepository.Object);
        // Assert
        mockRepository.Verify(r => r.SaveTime(eventParticipant), Times.Once);
    }
}