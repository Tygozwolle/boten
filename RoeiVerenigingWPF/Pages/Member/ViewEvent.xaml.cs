using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using DataAccessLibrary;
using RoeiVerenigingLibrary;
using RoeiVerenigingLibrary.Model;
using RoeiVerenigingLibrary.Services;
using RoeiVerenigingWPF.Frames;

namespace RoeiVerenigingWPF.Pages.Member;

public partial class ViewEvent : Page
{
    private MainWindow _MainWindow;
    public Event Events { get; set; }
    private IEventResultRepository _EventReportsRepository = new EventResultRepository();

    private readonly SolidColorBrush _textColor = new SolidColorBrush(Color.FromArgb(255, 4, 48, 73));
    private readonly SolidColorBrush _borderColor = new SolidColorBrush(Color.FromArgb(255, 19, 114, 160));

    private readonly SolidColorBrush
        _evenRowColor = new SolidColorBrush(Color.FromArgb(255, 232, 246, 252));

    private readonly SolidColorBrush
        _oddRowColor = new SolidColorBrush(Color.FromArgb(255, 182, 227, 251));

    public String Date
    {
        get { return Events.StartDate.Date.ToString("dd MMMM, yyyy"); }
    }

    public String StartTime
    {
        get { return Events.StartDate.ToString("HH:mm"); }
    }

    public String EndTime
    {
        get { return Events.EndDate.ToString("HH:mm"); }
    }

    public String ParticipantsCount
    {
        get { return Events.Participants.Count.ToString() + "/" + Events.MaxParticipants.ToString(); }
    }

    public ViewEvent(MainWindow mw, Event _event)
    {
        DataContext = this;
        _MainWindow = mw;
        Events = _event;
        Events.AddParticipantsFromDatabase(_EventReportsRepository);
        InitializeComponent();
        PopulateParticipantsView();
    }

    public void PopulateParticipantsView()
    {
        bool enableEdit = (_MainWindow.LoggedInMember.Roles.Contains("materiaal_commissaris") ||
                           _MainWindow.LoggedInMember.Roles.Contains("beheerder"));
        ReportView.Children.Clear();
        for (int i = 0; i < Events.Participants.Count; i++)
        {
            EventParticipant member = Events.Participants[i];
            Grid grid = new Grid();
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(10) });

            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(250) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(200) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(300) });

            grid.Children.Add(new TextBlock
            {
                Text = member.FirstName, VerticalAlignment = VerticalAlignment.Center, FontSize = 16,
                Foreground = _textColor
            });
            Grid.SetColumn(grid.Children[0], 0);

            grid.Children.Add(
                new TextBlock
                {
                    Text = member.Infix, VerticalAlignment = VerticalAlignment.Center, FontSize = 16,
                    Foreground = _textColor
                });
            Grid.SetColumn(grid.Children[1], 1);

            grid.Children.Add(new TextBlock
            {
                Text = member.LastName, VerticalAlignment = VerticalAlignment.Center, FontSize = 16,
                Foreground = _textColor
            });
            Grid.SetColumn(grid.Children[2], 2);

            Border border = new Border
            {
                BorderThickness = new Thickness(1),
                BorderBrush = _borderColor,
                CornerRadius = new CornerRadius(5),
                Padding = new Thickness(10),
                Child = grid,
                Background = i % 2 == 0 ? _evenRowColor : _oddRowColor // Alternate row background color
            };

            ReportView.Children.Add(border);
        }
    }


    public void Participate(object sender, RoutedEventArgs e)
    {
        try
        {
            //add participant
            new EventService(new EventRepository()).AddParticipant(Events, _MainWindow.LoggedInMember);
            Events = new EventRepository().GetEventById(Events.Id);
            Events.AddParticipantsFromDatabase(_EventReportsRepository);
            PopulateParticipantsView();
            AmountOfParticipants.Text = ParticipantsCount;
            ExceptionText.Text = "Je neemt nu deel";
            ExceptionText.Foreground = Brushes.Lime;
        }
        catch (Exception exception)
        {
            ExceptionText.Text = exception.Message;
            ExceptionText.Foreground = Brushes.Red;
        }
    }
}