using System.ComponentModel;

namespace RoeiVerenigingLibrary.Model;

public class EventParticipant : Member, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;
    public TimeSpan ResultTime;
    private string _description;

    public string Description
    {
        get => _description;
        set
        {
            if (_description != value)
            {
                _description = value;
                OnPropertyChanged(nameof(Description));
            }
        }
    }
    public int EventId;

    public EventParticipant(Member member, int eventId, TimeSpan time) : base(member.Id, member.FirstName, member.Infix,
        member.LastName, member.Email,
        member.Roles, member.Level)
    {
        ResultTime = time;
        EventId = eventId;
        Description = "Goed gedaan"; //Todo replace with data
    }

    public void saveResultTime(IEventResultRepository repository)
    {
        repository.SaveTime(this);
    }
    
    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}