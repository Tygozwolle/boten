using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DataAccessLibary;
using RoeiVerenigingLibary;
using RoeiVerenigingWPF.Frames;

namespace RoeiVerenigingWPF.Pages
{
    /// <summary>
    /// Interaction logic for ViewUsers.xaml
    /// </summary>
    public partial class ViewReservations : Page
    {
        public Boat Boat { get; set; }
        public Member Member { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        
        public List<Reservation> ReservationList { get; set; }
        public MainWindow MainWindow { set; get; }
        
        public ViewReservations(){}

        public ViewReservations(MainWindow mainWindow)
        {
            InitializeComponent();
            DataContext = this;
            ReservationService service = new ReservationService(new ReservationRepository());
            MainWindow = mainWindow;
            ReservationList = service.GetReservations(mainWindow.LoggedInMember);
        }

        public void Button_Click(object sender, RoutedEventArgs e)
        {
            
        }

        public void Control_OnMouseDoubleClick(object sender, RoutedEventArgs e)
        {
            
            // StartTime = ReservationListFinder.SelectedItem.
            // EndTime = service.EndTime;
            // Boat = service.Boat;
            // Member = service.Member;
            MainWindow.MainContent.Navigate(new EditReservation(MainWindow));
        }
    }
}