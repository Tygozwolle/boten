namespace RoeiVerenigingLibrary.Model;

public class Event
{
    public Event(List<Member> participants, DateTime startDate, DateTime endDate, string description, string name,
        int id, int maxParticipants, List<Boat> boats)
    {
        Participants = participants;
        StartDate = startDate;
        EndDate = endDate;
        Description = description;
        Name = name;
        Id = id;
        MaxParticipants = maxParticipants;
        Boats = boats;
    }

    public List<Member> Participants { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Description { get; set; }
    public string Name { get; set; }
    public int Id { get; set; }
    public int MaxParticipants { get; set; }
    public List<Boat> Boats { get; set; }
}