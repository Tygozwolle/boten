#region

using RoeiVerenigingLibrary.Model;

#endregion

namespace RoeiVerenigingLibrary.Interfaces;

public interface IEventRepository
{
    public Event Create(DateTime startTime, DateTime endDate,string descriptions, string name, int maxParticipants,List<Boat> boats , Member member);
    public Event Change(Event events, DateTime startDate, DateTime endDate, string description, string name, int maxParticipants);
}