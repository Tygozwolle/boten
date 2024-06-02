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
        public List<Event> EventsList { get; set; }
        private EventService _eventService = new(new EventRepository());
        public MainPage(MainWindow mainWindow)
        {
            InitializeComponent();
            MainWindow = mainWindow;
            EventsList = _eventService.GetEvents(false);
            DataContext = this;
            StatisticsFrame.Content = new ViewStatistics(mainWindow);
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