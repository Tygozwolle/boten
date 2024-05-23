using DataAccessLibary;
using RoeiVerenigingLibary;
using RoeiVerenigingWPF.Frames;
using System.Windows;
using System.Windows.Controls;

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

        public ViewReservations(MainWindow mainWindow)
        {
            InitializeComponent();
            DataContext = this;
            ReservationService service = new ReservationService(new ReservationRepository());
            MainWindow = mainWindow;
            ReservationList = service.GetReservations(mainWindow.LoggedInMember);
        }

        public void SelectReservation(object sender, RoutedEventArgs e)
        {
            if (sender is Button)
            {
                Button casted = sender as Button;
                object command = casted.CommandParameter;
                int idReservation = Int32.Parse(command.ToString());

                MainWindow.MainContent.Navigate(new EditReservation(MainWindow, idReservation));
            }
        }
    }
}