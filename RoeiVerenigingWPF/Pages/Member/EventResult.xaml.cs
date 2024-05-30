using System.Windows.Controls;
using DataAccessLibrary;
using RoeiVerenigingLibrary;
using RoeiVerenigingLibrary.Model;
using RoeiVerenigingWPF.Frames;

namespace RoeiVerenigingWPF.Pages.Member;

public partial class EventResult : Page
{
    private MainWindow _MainWindow;
    public Event EventResults;
    private IEventResultRepository _EventReportsRepository = new EventResultRepository();
    public EventResult(MainWindow mw, Event _event)
    {
        DataContext = this;
        _MainWindow = mw;
        EventResults = _event;
        EventResults.AddParticipantsFromDatabase(_EventReportsRepository);
        InitializeComponent();
    }
}