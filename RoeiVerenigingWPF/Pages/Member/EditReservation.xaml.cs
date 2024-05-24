using DataAccessLibrary;
using RoeiVerenigingLibary;
using RoeiVerenigingWPF.Frames;
using System.Windows;
using System.Windows.Controls;

namespace RoeiVerenigingWPF.Pages
{
    public partial class EditReservation : Page
    {
        private readonly BoatService boatService = new BoatService(new BoatRepository());
        private readonly ReservationService service = new ReservationService(new ReservationRepository());


        public EditReservation(MainWindow main, int reservationId)
        {
            Main = main;
            this.reservationId = reservationId;
            InitializeComponent();
            reservation = service.GetReservation(reservationId);
            boat = boatService.GetBoatById(reservation.BoatId);
            DataContext = this;

            StartingTime = reservation.StartTime;
            EndTime = reservation.EndTime;
        }
        public MainWindow Main { get; set; }
        public Member member { get; set; }
        public Reservation reservation { get; set; }
        public Boat boat { get; set; }
        private int reservationId { get; }
        public DateTime StartingTime { get; set; }
        public DateTime EndTime { get; set; }

        public void ConfirmButton(object sender, RoutedEventArgs e)
        {
            service.ChangeReservation(reservationId, reservation.Member, reservation.BoatId, StartingTime, EndTime); //id reserveren
            MessageBox.Show("Reservering gewijziged");

        }
    }
}