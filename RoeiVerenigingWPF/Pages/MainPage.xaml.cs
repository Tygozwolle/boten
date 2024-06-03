using DataAccessLibrary;
using RoeiVerenigingLibrary.Model;
using RoeiVerenigingLibrary.Services;
using RoeiVerenigingWPF.Frames;
using RoeiVerenigingWPF.Pages.EventCommissioner;
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
            EventsList = _eventService.GetEventsFuture();
            DataContext = this;
            StatisticsFrame.Content = new ViewStatistics(mainWindow);
            if (MainWindow.LoggedInMember.Roles.Contains("beheerder") || MainWindow.LoggedInMember.Roles.Contains("evenementen_commissaris"))
            {
                SubText.Text = "Klik met u rechter muisknop om een evenement te wijzigen";
            }
        }

        public MainWindow MainWindow { get; set; }

        private void UIElement_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.MainContent.Navigate(new ListEvents(MainWindow, _eventService.GetEvents(), false));
        }

        private void Grid_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Grid)
            {
                object command = ((Grid)sender).Tag;
                int id = Int32.Parse(command.ToString());
                MainWindow.MainContent.Navigate(new ViewEvent(MainWindow,
                    new EventService(new EventRepository()).GetEventById(id)));
            }
        }
        private void UIElement_OnMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if(MainWindow.LoggedInMember.Roles.Contains("beheerder")|| MainWindow.LoggedInMember.Roles.Contains("evenementen_commissaris"))
            {
                if (sender is Grid)
                {
                    object command = ((Grid)sender).Tag;
                    int id = Int32.Parse(command.ToString());
                    MainWindow.MainContent.Navigate(new ManageEvent(MainWindow, _eventService.GetEventById(id)));
                }
            }
        }
    }
}