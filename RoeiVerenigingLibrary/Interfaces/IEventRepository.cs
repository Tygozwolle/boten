using RoeiVerenigingLibrary.Model;

namespace RoeiVerenigingLibrary.Interfaces
{
    public interface IEventRepository
    {
       public List<Event> GetEvents();
       public List<Event> GetEventsFuture();
       public Event Create(DateTime startTime, DateTime endDate,string descriptions, string name, int maxParticipants,List<Boat> boats , Member member);
       public List<Event> GetAll(bool includeParticipants = true, bool includeBoats = true);
       public Event Change(Event events, DateTime startDate, DateTime endDate, string description, String name, int maxParticipants, List<Boat> boatsToAdd, List<Boat> boatsToRemove);
       public Event GetEventById(int id);
       public List<Event> GetEventsFromPastMonths(int AmountOfMonths);
       public List<int> GetEventReservationsIds(Event events);
       public Event Get(int id);
    }
}
