using DataAccessLibrary;
using RoeiVerenigingLibrary.Model;
using RoeiVerenigingLibrary.Services;
using RoeiVerenigingWPF.Frames;
using System.Windows.Controls;
using System.Windows.Input;

namespace RoeiVerenigingWPF.Pages.Member
{
    /// <summary>
    /// Interaction logic for ListEvents.xaml
    /// </summary>
    public partial class ListEvents : Page
    {
        public List<Event> EventsList { get; set; }
        private EventService _eventService = new(new EventRepository());
        private MainWindow _mainWindow;
        private bool _SendToResult;

        public ListEvents(MainWindow mainWindow, List<Event> events, bool sendToResult)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
            EventsList = events;
            _SendToResult = sendToResult;
            DataContext = this;
            InitializeComponent();
        }

        private void UIElement_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Grid)
            {
                object command = ((Grid)sender).Tag;
                int id = Int32.Parse(command.ToString());
                if (_SendToResult)
                {
                    _mainWindow.MainContent.Navigate(new EventResult(_mainWindow,
                        new EventService(new EventRepository()).GetEventById(id)));
                }
                else
                {
                    _mainWindow.MainContent.Navigate(new ViewEvent(_mainWindow,
                        new EventService(new EventRepository()).GetEventById(id)));
                }
            }
        }
    }
}