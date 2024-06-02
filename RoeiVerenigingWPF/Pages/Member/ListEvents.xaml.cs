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

        public List<Event> _events { get; set; }
        private EventService _eventService = new(new EventRepository());
        private MainWindow MainWindow;
        public ListEvents(MainWindow mainWindow)
        {
            InitializeComponent();
            MainWindow = mainWindow;
            _events = _eventService.GetEvents(true);
            DataContext = this;
            InitializeComponent();
        }

        private void UIElement_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Grid)
            {
                Grid casted = sender as Grid;
                object command = casted.Tag;
                int id = Int32.Parse(command.ToString());


                //MainWindow.MainContent.Navigate());
            }
        }
    }
}
