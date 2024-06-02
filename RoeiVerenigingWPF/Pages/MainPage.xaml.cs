using DataAccessLibrary;
using RoeiVerenigingLibrary.Model;
using RoeiVerenigingLibrary.Services;
using RoeiVerenigingWPF.Frames;
using RoeiVerenigingWPF.Pages.Member;
using System.Windows.Controls;
using System.Windows.Input;

namespace RoeiVerenigingWPF.Pages
{
    public partial class MainPage : Page
    {
        //TODO: dit aanpassen naar database ophalen, skylabs ligt eruit
        public List<Event> _events { get; set; }
        private EventService _eventService = new(new EventRepository());


        public Event eEvent { get; set; }
        public MainPage(MainWindow mainWindow)
        {
            InitializeComponent();
            MainWindow = mainWindow;
            _events = _eventService.GetEvents(false);
            DataContext = this;
            //StatisticsFrame.Content = new ViewStatistics(mainWindow);
        }
        public MainWindow MainWindow { get; set; }

        private void UIElement_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.MainContent.Navigate(new ListEvents(MainWindow));
        }

        private void Grid_OnMouseDown(object sender, MouseButtonEventArgs e)
        {

        }
    }
}