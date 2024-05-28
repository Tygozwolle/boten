using RoeiVerenigingLibrary.Interfaces;
using RoeiVerenigingLibrary.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoeiVerenigingLibrary.Services
{
    public class EventService
    {
        private IEventRepository _eventRepository;
        public EventService(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }
        public Event CreateEvent( DateTime startDate, DateTime endDate, string description, string name, int maxParticipants, List<Boat> boats, Member logedInMember)
        {
            return _eventRepository.Create(startDate, endDate, description, name, maxParticipants, boats, logedInMember);
        }
    }
}
