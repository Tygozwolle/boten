using RoeiVerenigingLibrary.Model;
using RoeiVerenigingWPF.Frames;
using System.Windows;
using System.Windows.Controls;

namespace RoeiVerenigingWPF.Pages.Member
{
    /// <summary>
    /// Interaction logic for ListEvents.xaml
    /// </summary>

    public partial class ListEvents : Page
    {
        //public List<Event> _events { get; set; }
        //private EventService _eventService { get; set; }

        private static RoeiVerenigingLibrary.Member member1 = new(5, "Pieter", "van", "huizen", "pieter@gmail.com", 2);
        private static EventParticipant eventParticipant = new(member1, 4, TimeSpan.MaxValue);
        private static List<EventParticipant> eventParticipantlist = new();
        private MainWindow MainWindow;
        public Event eEvent { get; set; }
        public ListEvents(MainWindow mainWindow)
        {
            MainWindow = mainWindow;
            //_events = _eventService.GetEvents(true);

            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            //MainWindow.MainContent.Navigate();
            //TODO:navigate naar aanmaken evenement
        }
    }
}
