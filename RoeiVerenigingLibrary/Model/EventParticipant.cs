namespace RoeiVerenigingLibrary.Model;

public class EventParticipant : Member
{
    public TimeSpan ResultTime;
    public String Description;
    public int EventId;

    public EventParticipant(Member member, int eventId, TimeSpan time) : base(member.Id, member.FirstName, member.Infix,
        member.LastName, member.Email,
        member.Roles, member.Level)
    {
        ResultTime = time;
        EventId = eventId;
        Description = "Goed gedaan"; //Todo replace with data
    }

    public EventParticipant(int id, string firstName, string infix, string lastName, string email,
        int level) : base(id, firstName, infix, lastName, email, level){}

    public void saveResultTime(IEventResultRepository repository)
    {
        repository.SaveTime(this);
    }
}