using System.ComponentModel;

namespace RoeiVerenigingLibrary.Model;

public class EventParticipant : Member, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;
    private TimeSpan _resultTime;
    private string _description;

    public TimeSpan ResultTime
    {
        get => _resultTime;
        set
        {
            if (_resultTime != value)
            {
                _resultTime = value;
                OnPropertyChanged(nameof(ResultTime));
            }
        }
    }

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

    public EventParticipant(Member member, int eventId, TimeSpan time, String description) : base(member.Id,
        member.FirstName, member.Infix,
        member.LastName, member.Email,
        member.Roles, member.Level)
    {
        ResultTime = time;
        EventId = eventId;
        Description = description;
    }

    public EventParticipant(int id, string firstName, string infix, string lastName, string email,
        int level) : base(id, firstName, infix, lastName, email, level){}

    public void saveResultTime(IEventResultRepository repository)
    {
        repository.SaveTime(this);
    }

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}