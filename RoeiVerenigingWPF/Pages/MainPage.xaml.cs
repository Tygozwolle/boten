using RoeiVerenigingLibrary.Model;
using RoeiVerenigingWPF.Frames;
using RoeiVerenigingWPF.Pages.Member;
using System.Windows.Controls;
using System.Windows.Input;

namespace RoeiVerenigingWPF.Pages
{
    public partial class MainPage : Page
    {
        //TODO: dit aanpassen naar database ophalen, skylabs ligt eruit
        private static RoeiVerenigingLibrary.Member member1 = new(5, "Pieter", "van", "huizen", "pieter@gmail.com", 2);
        private static EventParticipant eventParticipant = new(member1, 4, TimeSpan.MaxValue);
        private static List<EventParticipant> eventParticipantlist = new();

        public Event eEvent { get; set; }
        public MainPage(MainWindow mainWindow)
        {
            InitializeComponent();
            MainWindow = mainWindow;
            eventParticipantlist.Add(eventParticipant);
            eEvent = new Event(eventParticipantlist, DateTime.UtcNow, DateTime.UtcNow,
                "Dit is een hele mooie wedstrijd", "Pieters wedstrijd", 4, 20, null);
            DataContext = this;
            //StatisticsFrame.Content = new ViewStatistics(mainWindow);
        }
        public MainWindow MainWindow { get; set; }

        private void UIElement_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.MainContent.Navigate(new ListEvents(MainWindow));
        }
    }
}