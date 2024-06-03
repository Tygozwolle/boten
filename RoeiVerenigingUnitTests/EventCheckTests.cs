using Moq;
using RoeiVerenigingLibrary;
using RoeiVerenigingLibrary.Interfaces;
using RoeiVerenigingLibrary.Model;
using RoeiVerenigingLibrary.Services;

namespace RoeiVerenigingUnitTests
{
    public class EventCheckTests
    {
        private Mock<IEventRepository> _eventRepositoryMock;
    private EventService _eventService;

    [SetUp]
    public void SetUp()
    {
        _eventRepositoryMock = new Mock<IEventRepository>();
        _eventService = new EventService(_eventRepositoryMock.Object);
    }

    [Test]
    public void GetAvailableTimes_WhenNoEvents_ReturnsAllTimes()
    {
        _eventRepositoryMock.Setup(repo => repo.GetAll(false, false)).Returns(new List<Event>());

        var result = _eventService.GetAvailableTimes(DateTime.Today);

        Assert.That(24 == result.Count);
    }

    [Test]
    public void GetAvailableTimes_WhenEventExists_ExcludesEventTimes()
    {
        var events = new List<Event>
        {
            new Event(new List<EventParticipant>(), DateTime.Today.AddHours(10), DateTime.Today.AddHours(12), "Test", "Test", 1, 1, new List<Boat>()),
        };
        _eventRepositoryMock.Setup(repo => repo.GetAll(false, false)).Returns(events);

        var result = _eventService.GetAvailableTimes(DateTime.Today);

        Assert.That(22 == result.Count);
        Assert.That(!result.Contains(DateTime.Today.AddHours(10)));
        Assert.That(!result.Contains(DateTime.Today.AddHours(11)));
    }

    [Test]
    public void GetAvailableTimes_WhenMultipleEventsExist_ExcludesAllEventTimes()
    {
        var events = new List<Event>
        {
            new Event(new List<EventParticipant>(), DateTime.Today.AddHours(10), DateTime.Today.AddHours(12), "Test", "Test", 1, 1, new List<Boat>()),
            new Event(new List<EventParticipant>(), DateTime.Today.AddHours(15), DateTime.Today.AddHours(17), "Test2", "Test2", 2, 1, new List<Boat>()),
        };
        _eventRepositoryMock.Setup(repo => repo.GetAll(false, false)).Returns(events);

        var result = _eventService.GetAvailableTimes(DateTime.Today);

        Assert.That(20 == result.Count);
        Assert.That(!result.Contains(DateTime.Today.AddHours(10)));
        Assert.That(!result.Contains(DateTime.Today.AddHours(11)));
        Assert.That(!result.Contains(DateTime.Today.AddHours(15)));
        Assert.That(!result.Contains(DateTime.Today.AddHours(16)));
    }
    
    
    [Test]
    public void GetAvailableTimes_WhenNoEvents_ReturnsAllTimes_Event()
    {
        var events = new List<Event>
        {
            new Event(new List<EventParticipant>(), DateTime.Today.AddHours(10), DateTime.Today.AddHours(12), "Test", "Test", 1, 1, new List<Boat>()),
        };
        _eventRepositoryMock.Setup(repo => repo.GetAll(false, false)).Returns(events);

        var result = _eventService.GetAvailableTimes(DateTime.Today, events[0]);

        Assert.That(24 == result.Count);
    }

    [Test]
    public void GetAvailableTimes_WhenEventExists_ExcludesEventTimes_Event()
    {
        var events = new List<Event>
        {
            new Event(new List<EventParticipant>(), DateTime.Today.AddHours(2), DateTime.Today.AddHours(9), "TestEvent", "TestEvent", 1, 1, new List<Boat>()),
            new Event(new List<EventParticipant>(), DateTime.Today.AddHours(10), DateTime.Today.AddHours(12), "Test", "Test", 1, 1, new List<Boat>()),
        };
        _eventRepositoryMock.Setup(repo => repo.GetAll(false, false)).Returns(events);

        var result = _eventService.GetAvailableTimes(DateTime.Today, events[0]);

        Assert.That(22 == result.Count);
        Assert.That(!result.Contains(DateTime.Today.AddHours(10)));
        Assert.That(!result.Contains(DateTime.Today.AddHours(11)));
    }

    [Test]
    public void GetAvailableTimes_WhenMultipleEventsExist_ExcludesAllEventTimes_Event()
    {
        var events = new List<Event>
        {
            new Event(new List<EventParticipant>(), DateTime.Today.AddHours(2), DateTime.Today.AddHours(9), "TestEvent", "TestEvent", 1, 1, new List<Boat>()),
            new Event(new List<EventParticipant>(), DateTime.Today.AddHours(10), DateTime.Today.AddHours(12), "Test", "Test", 2, 1, new List<Boat>()),
            new Event(new List<EventParticipant>(), DateTime.Today.AddHours(15), DateTime.Today.AddHours(17), "Test2", "Test2", 3, 1, new List<Boat>()),
        };
        _eventRepositoryMock.Setup(repo => repo.GetAll(false, false)).Returns(events);

        var result = _eventService.GetAvailableTimes(DateTime.Today, events[0]);

        Assert.That(20 == result.Count);
        Assert.That(!result.Contains(DateTime.Today.AddHours(10)));
        Assert.That(!result.Contains(DateTime.Today.AddHours(11)));
        Assert.That(!result.Contains(DateTime.Today.AddHours(15)));
        Assert.That(!result.Contains(DateTime.Today.AddHours(16)));
    }
    
    [Test]
    public void CheckIfEventIsPosibly_WhenNoEvents_ReturnsTrue()
    {
        _eventRepositoryMock.Setup(repo => repo.GetAll(false, false)).Returns(new List<Event>());

        var result = _eventService.CheckIfEventIsPosibly(DateTime.Today.AddHours(10), DateTime.Today.AddHours(12));

        Assert.That(result);
    }

    [Test]
    public void CheckIfEventIsPosibly_WhenEventExists_ReturnsFalse()
    {
        var events = new List<Event>
        {
            new Event(new List<EventParticipant>(), DateTime.Today.AddHours(10), DateTime.Today.AddHours(12), "Test", "Test", 2, 1, new List<Boat>())
        };
        _eventRepositoryMock.Setup(repo => repo.GetAll(false, false)).Returns(events);

        var result = _eventService.CheckIfEventIsPosibly(DateTime.Today.AddHours(10), DateTime.Today.AddHours(12));

        Assert.That(!result);
    }

    [Test]
    public void CheckIfEventIsPosibly_WhenMultipleEventsExist_ReturnsFalse()
    {
        var events = new List<Event>
        {
            new Event(new List<EventParticipant>(), DateTime.Today.AddHours(10), DateTime.Today.AddHours(12), "Test", "Test", 2, 1, new List<Boat>()),
            new Event(new List<EventParticipant>(), DateTime.Today.AddHours(15), DateTime.Today.AddHours(17), "Test2", "Test2", 3, 1, new List<Boat>()),
        };
        _eventRepositoryMock.Setup(repo => repo.GetAll(false, false)).Returns(events);

        var result = _eventService.CheckIfEventIsPosibly(DateTime.Today.AddHours(10), DateTime.Today.AddHours(12));

        Assert.That(!result);
    }
    }
}