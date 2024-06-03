using Moq;
using RoeiVerenigingLibrary;
using RoeiVerenigingLibrary.Exceptions;
using RoeiVerenigingLibrary.Interfaces;
using RoeiVerenigingLibrary.Model;
using RoeiVerenigingLibrary.Services;

namespace RoeiVerenigingUnitTests
{
    public class EventTests
    {
        
    private Mock<IEventRepository> _eventRepositoryMock;
    private EventService _eventService;
    private Member _loggedInMember = new Member(2, "test", "test", "last", "mail.test@windesheim.com", new List<string> { "evenementen_commissaris" }, 3 );
    private List<Event> _eventsList;
    
    [SetUp]
    public void SetUp()
    {
        _eventRepositoryMock = new Mock<IEventRepository>();
        _eventService = new EventService(_eventRepositoryMock.Object);
        var eventList = new List<Event>()
        {
            new Event(new List<EventParticipant>(), DateTime.Now.AddDays(19), DateTime.Now.AddDays(20), "Test Event1", "Event1", 1, 10, new List<Boat>()),
            new Event(new List<EventParticipant>(), DateTime.Now.AddDays(18), DateTime.Now.AddDays(19), "Test Event2", "Event2", 2, 10, new List<Boat>())
        };
        _eventsList = eventList;
        _eventRepositoryMock.Setup(getall => getall.GetAll(false, false)).Returns(eventList);
    }

    [Test]
    public void CreateEvent_WhenEventCheckPasses_CallsCreateOnRepositoryAndReturnsResult()
    {
        // Arrange
        var startDate = DateTime.Now.AddDays(15);
        var endDate = DateTime.Now.AddDays(16);
        var description = "Test Event";
        var name = "Event";
        var maxParticipants = 10;
        var boats = new List<Boat>();
        var loggedInMember =_loggedInMember;
        var expectedEvent = new Event(new List<EventParticipant>(), startDate, endDate, description, name, 3, maxParticipants ,boats);
        _eventRepositoryMock.Setup(repo => repo.Create(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<List<Boat>>(), It.IsAny<Member>())).Returns(expectedEvent);

        // Act
        var result = _eventService.CreateEvent(startDate, endDate, description, name, maxParticipants, boats, loggedInMember);

        // Assert
        Assert.That((expectedEvent.Equals(result)));
        _eventRepositoryMock.Verify(repo => repo.Create(startDate, endDate, description, name, maxParticipants, boats, loggedInMember), Times.Once);
    }

    [Test]
    public void CreateEvent_WhenEventCheckFails_LessThan14Days()
    {
        // Arrange
        var startDate = DateTime.Now.AddDays(10); // less than 14 days in the future, so Eventcheck should fail
        var endDate = DateTime.Now.AddDays(11);
        var description = "Test Event";
        var name = "Event";
        var maxParticipants = 10;
        var boats = new List<Boat>();
        var loggedInMember = _loggedInMember;
        
        // Assert
        Assert.Throws<EventTimeException>((() => _eventService.CreateEvent(startDate, endDate, description, name, maxParticipants, boats, loggedInMember)));

    }
    [Test]
    public void CreateEvent_WhenEventCheckFails_AlreadyOnThisTime()
    {
        // Arrange
        var startDate = DateTime.Now.AddDays(19); // there is already an event on this date, so Eventcheck should fail
        var endDate = DateTime.Now.AddDays(20);
        var description = "Test Event";
        var name = "Event";
        var maxParticipants = 10;
        var boats = new List<Boat>();
        var loggedInMember = _loggedInMember;

        // Assert
        Assert.Throws<EventAlreadyOnThisTimeException>((() => _eventService.CreateEvent(startDate, endDate, description, name, maxParticipants, boats, loggedInMember)));

    }
    [Test]
    public void CreateEvent_WhenEventCheckFailsIncorrectRights()
    {
        // Arrange
        var startDate = DateTime.Now.AddDays(15); 
        var endDate = DateTime.Now.AddDays(16);
        var description = "Test Event";
        var name = "Event";
        var maxParticipants = 10;
        var boats = new List<Boat>();
        var loggedInMember = new Member(2, "test", "test", "last", "email@email.com", 3); // incorrect rights, so Eventcheck should fail
        
        // Assert
        Assert.Throws<IncorrectRightsException>((() => _eventService.CreateEvent(startDate, endDate, description, name, maxParticipants, boats, loggedInMember)));
        
    }
    [Test]
    public void CreateEvent_WhenEventCheckFailsInvalidTime()
    {
        // Arrange
        var startDate = DateTime.Now.AddDays(17); // start date is after end date, so Eventcheck should fail
        var endDate = DateTime.Now.AddDays(16);
        var description = "Test Event";
        var name = "Event";
        var maxParticipants = 10;
        var boats = new List<Boat>();
        var loggedInMember = _loggedInMember;
        
        // Assert
        Assert.Throws<InvalidTimeException>((() => _eventService.CreateEvent(startDate, endDate, description, name, maxParticipants, boats, loggedInMember)));
        
    }
    
        [Test]
    public void UpdateEvent_WhenEventCheckPasses_CallsCreateOnRepositoryAndReturnsResult()
    {
        // Arrange
        var startDate = DateTime.Now.AddDays(15);
        var endDate = DateTime.Now.AddDays(16);
        var description = "Test Event";
        var name = "Event";
        var maxParticipants = 10;
        var boats = new List<Boat>();
        var loggedInMember =_loggedInMember;
        var eventOld = new Event(new List<EventParticipant>(), startDate.AddHours(2), endDate.AddHours(3), description+"hoi", name+"s", 3, maxParticipants ,boats);
        var expectedEvent = new Event(new List<EventParticipant>(), startDate, endDate, description, name, 3, maxParticipants ,boats);
        _eventRepositoryMock.Setup(repo => repo.Change(It.IsAny<Event>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<List<Boat>>(), It.IsAny<List<Boat>>())).Returns(expectedEvent);

        // Act
        var result = _eventService.UpdateEvent(eventOld, startDate, endDate, description, name, maxParticipants, loggedInMember, boats);

        // Assert
        Assert.That((expectedEvent.Equals(result)));
        _eventRepositoryMock.Verify(repo => repo.Change(eventOld, startDate, endDate, description, name, maxParticipants, new List<Boat>(), new List<Boat>()), Times.Once);
    }
    
        [Test]
        public void UpdateEvent_WhenEventCheckPasses_CallsCreateOnRepositoryAndReturnsResult_OverlappingTime()
        {
            // Arrange
            var eventOld = _eventsList[0];
            var startDate = eventOld.StartDate.AddHours(2);
            var endDate = eventOld.EndDate.AddHours(3);
            var description = "Test Event";
            var name = "Event";
            var maxParticipants = 10;
            var boats = new List<Boat>();
            var loggedInMember =_loggedInMember;
            
            var expectedEvent = new Event(new List<EventParticipant>(), startDate, endDate, description, name, eventOld.Id, maxParticipants ,boats);
            _eventRepositoryMock.Setup(repo => repo.Change(It.IsAny<Event>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<List<Boat>>(), It.IsAny<List<Boat>>())).Returns(expectedEvent);

            // Act
            var result = _eventService.UpdateEvent(eventOld, startDate, endDate, description, name, maxParticipants, loggedInMember, boats);

            // Assert
            Assert.That((expectedEvent.Equals(result)));
            _eventRepositoryMock.Verify(repo => repo.Change(eventOld, startDate, endDate, description, name, maxParticipants, new List<Boat>(), new List<Boat>()), Times.Once);
        }

    [Test]
    public void UpdateEvent_WhenEventCheckFails_LessThan14Days()
    {
        // Arrange
        var startDate = DateTime.Now.AddDays(10); // less than 14 days in the future, so Eventcheck should fail
        var endDate = DateTime.Now.AddDays(11);
        var description = "Test Event";
        var name = "Event";
        var maxParticipants = 10;
        var boats = new List<Boat>();
        var loggedInMember = _loggedInMember;
        var eventOld = new Event(new List<EventParticipant>(), startDate.AddHours(2), endDate.AddHours(3), description+"hoi", name+"s", 3, maxParticipants ,boats);
        // Assert
        Assert.Throws<EventTimeException>((() => _eventService.UpdateEvent(eventOld, startDate, endDate, description, name, maxParticipants, loggedInMember, boats)));

    }
    [Test]
    public void UpdateEvent_WhenEventCheckFails_AlreadyOnThisTime()
    {
        // Arrange
        var startDate = DateTime.Now.AddDays(19); // there is already an event on this date, so Eventcheck should fail
        var endDate = DateTime.Now.AddDays(20);
        var description = "Test Event";
        var name = "Event";
        var maxParticipants = 10;
        var boats = new List<Boat>();
        var loggedInMember = _loggedInMember;
        var eventOld = new Event(new List<EventParticipant>(), startDate.AddHours(2), endDate.AddHours(3), description+"hoi", name+"s", 3, maxParticipants ,boats);
        // Assert
        Assert.Throws<EventAlreadyOnThisTimeException>((() => _eventService.UpdateEvent(eventOld, startDate, endDate, description, name, maxParticipants, loggedInMember, boats)));

    }
    [Test]
    public void UpdateEvent_WhenEventCheckFailsIncorrectRights()
    {
        // Arrange
        var startDate = DateTime.Now.AddDays(15); 
        var endDate = DateTime.Now.AddDays(16);
        var description = "Test Event";
        var name = "Event";
        var maxParticipants = 10;
        var boats = new List<Boat>();
        var loggedInMember = new Member(2, "test", "test", "last", "email@email.com", 3); // incorrect rights, so Eventcheck should fail
        var eventOld = new Event(new List<EventParticipant>(), startDate.AddHours(2), endDate.AddHours(3), description+"hoi", name+"s", 3, maxParticipants ,boats);
        // Assert
        Assert.Throws<IncorrectRightsException>((() => _eventService.UpdateEvent(eventOld, startDate, endDate, description, name, maxParticipants, loggedInMember, boats)));
        
    }
    [Test]
    public void UpdateEvent_WhenEventCheckFailsInvalidTime()
    {
        // Arrange
        var startDate = DateTime.Now.AddDays(17); // start date is after end date, so Eventcheck should fail
        var endDate = DateTime.Now.AddDays(16);
        var description = "Test Event";
        var name = "Event";
        var maxParticipants = 10;
        var boats = new List<Boat>();
        var loggedInMember = _loggedInMember;
        var eventOld = new Event(new List<EventParticipant>(), startDate.AddHours(2), endDate.AddHours(3), description+"hoi", name+"s", 3, maxParticipants ,boats);
        // Assert
        Assert.Throws<InvalidTimeException>((() => _eventService.UpdateEvent(eventOld, startDate, endDate, description, name, maxParticipants, loggedInMember, boats)));
        
    }
    
    }
}