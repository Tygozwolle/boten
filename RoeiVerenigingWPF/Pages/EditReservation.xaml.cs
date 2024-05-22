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
    public Boat boat { get; set; }
    public DateTime StartingTime { get; set; }
    public DateTime EndTime { get; set; }


    public EditReservation(MainWindow main, int reservationId)
    {
        Main = main;
        InitializeComponent();
        ReservationService service = new ReservationService(new ReservationRepository());
        reservation = service.GetReservation(reservationId);
        boat = boatService.GetBoatById(reservation.BoatId);
        DataContext = this;

        StartingTime = reservation.StartTime;
        EndTime = reservation.EndTime;
    }

    public void ConfirmButton(object sender, RoutedEventArgs e)
    {
        MessageBox.Show("changed reservation");
    }
}
