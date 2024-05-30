using System.Windows.Controls;
using DataAccessLibrary;
using RoeiVerenigingLibrary;
using RoeiVerenigingLibrary.Model;
using RoeiVerenigingWPF.Frames;

namespace RoeiVerenigingWPF.Pages.Member;

public partial class EventResult : Page
{
    private MainWindow _MainWindow;
    public Event EventResults { get; set; }
    private IEventResultRepository _EventReportsRepository = new EventResultRepository();

    public String Date
    {
        get { return EventResults.StartDate.Date.ToString("dd MMMM, yyyy"); }
    }

    public String StartTime
    {
        get { return EventResults.StartDate.ToString("HH:mm"); }
    }

    public String EndTime
    {
        get { return EventResults.EndDate.ToString("HH:mm"); }
    }

    public String ParticipantsCount
    {
        get { return EventResults.Participants.Count.ToString() + "/" + EventResults.MaxParticipants.ToString(); }
    }

    public EventResult(MainWindow mw, Event _event)
    {
        DataContext = this;
        _MainWindow = mw;
        EventResults = _event;
        EventResults.AddParticipantsFromDatabase(_EventReportsRepository);
        InitializeComponent();
    }
}