
using DataAccessLibrary;
using RoeiVerenigingLibrary;
using RoeiVerenigingWPF.Frames;
using System.Windows;
using System.Windows.Controls;

namespace RoeiVerenigingWPF.Pages
{
    /// <summary>
    ///     Interaction logic for ViewUsers.xaml
    /// </summary>
    ///
    public partial class ViewReservations : Page
    {

        public ViewReservations(MainWindow mainWindow)
        {
            InitializeComponent();
            DataContext = this;
            ReservationService service = new ReservationService(new ReservationRepository());
            MainWindow = mainWindow;
            ReservationList = service.GetReservations(mainWindow.LoggedInMember);
            BoatListFill(ReservationList);
        }

        public List<Reservation> ReservationList { get; set; }
        public List<Boat> BoatList { get; set; } = new List<Boat>();
        public BoatService BService { get; } = new BoatService(new BoatRepository());
        public MainWindow MainWindow { set; get; }

        private void BoatListFill(List<Reservation> reservationList)
        {
            foreach (var reservation in reservationList)
            {
                this.BoatList.Add(reservation.Boat);
            }
        }

        private void ListView_Loaded(object sender, RoutedEventArgs e)
        {
            new Thread(async () =>
            {
                BService.GetImageBoats(BoatList);
                Dispatcher.Invoke((Action)(() =>
                {
                    ReservationListView.Items.Refresh();
                }));
            }).Start();
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