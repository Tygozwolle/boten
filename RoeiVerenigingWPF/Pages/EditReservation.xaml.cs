using DataAccessLibary;
using RoeiVerenigingLibary;
using RoeiVerenigingWPF.Frames;
using System.Windows;
using System.Windows.Controls;

namespace RoeiVerenigingWPF.Pages;

public partial class EditReservation : Page
{
    public MainWindow Main { get; set; }
    public Member member { get; set; }
    public Reservation reservation { get; set; }
    BoatService boatService = new BoatService(new BoatRepository());
    ReservationService service = new ReservationService(new ReservationRepository());
    public Boat boat { get; set; }
    private int reservationId { get; set; }
    public DateTime StartingTime { get; set; }
    public DateTime EndTime { get; set; }


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

    public void ConfirmButton(object sender, RoutedEventArgs e)
    {
        service.ChangeReservation(reservationId, reservation.Member, reservation.BoatId, StartingTime, EndTime); //id reserveren
        MessageBox.Show("changed reservation");

    }
}
