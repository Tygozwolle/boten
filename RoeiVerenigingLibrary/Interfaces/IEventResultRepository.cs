using RoeiVerenigingLibrary.Model;

namespace RoeiVerenigingLibrary;

public interface IEventResultRepository
{
    public void SaveTime(EventParticipant eventParticipant);
    public List<EventParticipant> GetParticipants(int eventId);
}