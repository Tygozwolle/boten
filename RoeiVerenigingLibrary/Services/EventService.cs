using RoeiVerenigingLibrary.Exceptions;
using RoeiVerenigingLibrary.Interfaces;
using RoeiVerenigingLibrary.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
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
        public Event CreateEvent(DateTime startDate, DateTime endDate, string description, string name, int maxParticipants, List<Boat> boats, Member loggedInMember)
        {
            if (Eventcheck(startDate, endDate, description, name, maxParticipants, boats, loggedInMember))
            {
                return _eventRepository.Create(startDate, endDate, description, name, maxParticipants, boats, loggedInMember);
            }
            return null;
        }
        public Event UpdateEvent(Event events, DateTime startDate, DateTime endDate, string description, string name, int maxParticipants, Member loggedInMember, List<Boat> boats)
        {
            if (Eventcheck(startDate, endDate, description, name, maxParticipants, boats, loggedInMember, events))
            {
                var boatsToAdd = new List<Boat>();
                var boatsToRemove = new List<Boat>();
                foreach (var boat in boats)
                {
                    if (!events.Boats.Any(b => b.Id == boat.Id))
                    {
                        boatsToAdd.Add(boat);
                    }
                }
                foreach (var boat in events.Boats)
                {
                    if (!boats.Any(b => b.Id == boat.Id))
                    {
                        boatsToRemove.Add(boat);
                    }
                }
                
                //TODO: check if max participants is not less than the amount of participants
                return _eventRepository.Change(events, startDate, endDate, description, name, maxParticipants , boatsToAdd, boatsToRemove);
            }
            return null;
        }
        private bool Eventcheck(DateTime startDate, DateTime endDate, string description, string name, int maxParticipants, List<Boat> boats, Member loggedInMember, Event? events)
        {
            if (!(loggedInMember.Roles.Contains("evenementen_commissaris") || loggedInMember.Roles.Contains("beheerder")))
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

            if (!CheckIfEventIsPosibly(startDate, endDate, events))
            {
                throw new EventAlreadyOnThisTimeException();
            }

            return true;
        }
        private bool Eventcheck(DateTime startDate, DateTime endDate, string description, string name, int maxParticipants, List<Boat> boats, Member loggedInMember)
        {
            return Eventcheck(startDate, endDate, description, name, maxParticipants, boats, loggedInMember, null);
        }
        
        public bool CheckIfEventIsPosibly(DateTime startTime, DateTime endTime)
        {
            return CheckIfEventIsPosibly(startTime, endTime, null);
        }
        public bool CheckIfEventIsPosibly(DateTime startTime, DateTime endTime, Event? currentEvent)
        {
            List<Event> events = _eventRepository.GetAll();
            foreach (Event eventob in events)
            {
                if (startTime < eventob.EndDate && endTime > eventob.StartDate)
                {
                    if (currentEvent == null)
                    {
                        return false;
                    }
                    if (eventob.Id != currentEvent?.Id)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        public List<DateTime> GetAvailableTimes(DateTime selcetedDate, Event events)
        {
            var timeAvailableList = GetAvailableTimes(selcetedDate);
            
            // DateTime endtime;
            //     if(events.EndDate.Minute != 0)
            //     {
            //         endtime = events.EndDate.AddMinutes(-59);
            //     }
            //     else
            //     {
            //         endtime = events.EndDate.AddHours(-1);
            //     }
            var timesToAdd = Enumerable.Range(0, (events.EndDate - events.StartDate).Hours)
                .Select(i => events.StartDate.AddHours(i)).ToList();
            foreach (var add in timesToAdd)
            {
                timeAvailableList.Add(add);
            }
            return timeAvailableList.OrderBy(x => x).ToList();;
        }
        public List<DateTime> GetAvailableTimes(DateTime selectedDate)
        {
            List<Event> events = _eventRepository.GetAll();

            DateTime startTime = selectedDate.Date;
            
            Dictionary<DateTime, int> reservationsPerTimeBlock = events
                .SelectMany(eventob => Enumerable.Range(0, (eventob.EndDate - eventob.StartDate).Hours)
                    .Select(i => eventob.StartDate.AddHours(i)))
                .GroupBy(time => time)
                .ToDictionary(group => group.Key, group => group.Count());
            
            List<DateTime> timeAvailableList = Enumerable.Range(0, 24)
                .Select(i => startTime.AddHours(i))
                .Where(time =>
                    !reservationsPerTimeBlock.ContainsKey(time))
                .ToList();

            return timeAvailableList;
        }
        public List<Event> GetEventsFromPastMonths(int AmountOfMonths)
        {
            return _eventRepository.GetEventsFromPastMonths(AmountOfMonths);
        }
        public Event GetEventById(int id)
        {
            return _eventRepository.Get(id);
        }
    }

}


