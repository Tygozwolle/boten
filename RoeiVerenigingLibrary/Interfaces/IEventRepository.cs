using RoeiVerenigingLibrary.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoeiVerenigingLibrary.Interfaces
{
    public interface IEventRepository
    {
       public Event Create(DateTime startTime, DateTime endDate,string descriptions, string name, int maxParticipants,List<Boat> boats , Member member);
       public List<Event> GetAll();
       public Event Change(Event events, DateTime startDate, DateTime endDate, string description, String name, int maxParticipants);
       public List<int> GetEventReservationsIds(Event events);
       public Event Get(int id);
    }
}
