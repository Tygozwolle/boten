namespace RoeiVerenigingLibrary.Model;

public class EventParticipant : Member
{
    public TimeSpan ResultTime;
    public int EventId;

    public EventParticipant(Member member, int eventId, TimeSpan time) : base(member.Id, member.FirstName, member.Infix,
        member.LastName, member.Email,
        member.Roles, member.Level)
    {
        ResultTime = time;
        EventId = eventId;
    }

    public void saveResultTime(IEventResultRepository repository)
    {
        repository.SaveTime(this);
    }
}