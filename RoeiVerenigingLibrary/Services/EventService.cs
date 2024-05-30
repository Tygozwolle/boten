using RoeiVerenigingLibrary.Exceptions;
using RoeiVerenigingLibrary.Interfaces;
using RoeiVerenigingLibrary.Model;

namespace RoeiVerenigingLibrary.Services
{
    public class EventService
    {
        private IEventRepository _eventRepository;
        public EventService(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }
        public Event CreateEvent(DateTime startDate, DateTime endDate, string description, string name, int maxParticipants, List<Boat> boats, Member loggedInMember)
        {
            if (!(loggedInMember.Roles.Contains("evenementen_connissaris") || loggedInMember.Roles.Contains("beheerder")))
            {
                throw new IncorrectRightsException();
            }
            if (startDate < DateTime.Now.AddDays(14))
            {
                throw new EventTimeException();
            }
            if (startDate > endDate)
            {
                throw new InvalidTimeException();
            }
            return _eventRepository.Create(startDate, endDate, description, name, maxParticipants, boats, loggedInMember);
        }
        public Event UpdateEvent(Event events, DateTime startDate, DateTime endDate, string description, string name, int maxParticipants, Member loggedInMember)
        {
            if (!(loggedInMember.Roles.Contains("evenementen_connissaris") || loggedInMember.Roles.Contains("beheerder")))
            {
                throw new IncorrectRightsException();
            }
            if (startDate < DateTime.Now.AddDays(14))
            {
                throw new EventTimeException();
            }
            if (startDate > endDate)
            {
                throw new InvalidTimeException();
            }
            //TODO: check if max participants is not less than the amount of participants
            return _eventRepository.Change(events, startDate, endDate, description, name, maxParticipants);
        }

        public List<Event> GetEvents(bool all)
        {
            if (all)
            {
                return _eventRepository.GetEvents();
            }

            return _eventRepository.GetEvents();
        }
    }
}


