
using DataAccessLibrary;
using RoeiVerenigingLibrary;
using RoeiVerenigingWPF.Frames;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using RoeiVerenigingWPF.Pages.Admin;

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
            _MainWindow = mainWindow;
            ReservationList = service.GetReservations(mainWindow.LoggedInMember);
            BoatListFill(ReservationList);
        }

        public List<Reservation> ReservationList { get; set; }
        public List<Boat> BoatList { get; set; } = new List<Boat>();
        public BoatService BService { get; } = new BoatService(new BoatRepository());
        public MainWindow _MainWindow { set; get; }

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

        private void Grid_Click(object sender, MouseButtonEventArgs e)
        {
            if (sender is Grid)
            {
                Grid casted = sender as Grid;
                object command = casted.Tag;
                int id = Int32.Parse(command.ToString());

                _MainWindow.MainContent.Navigate(new EditReservation(_MainWindow, id));
            }
        }
    }
}