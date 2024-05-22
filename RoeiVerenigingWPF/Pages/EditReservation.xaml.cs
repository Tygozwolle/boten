using DataAccessLibary;
using RoeiVerenigingLibary;
using RoeiVerenigingWPF.Frames;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace RoeiVerenigingWPF.Pages;

public partial class EditReservation : Page
{
    public MainWindow Main { get; set; }
    public Member member { get; set; }
    public Reservation reservation { get; set; }
    BoatService boatService = new BoatService(new BoatRepository());
    public Boat boat { get; set; }


    public EditReservation(MainWindow main, int reservationId)
    {
        Main = main;
        InitializeComponent();
        ReservationService service = new ReservationService(new ReservationRepository());
        reservation = service.GetReservation(reservationId);
        boat = boatService.Getboat(reservation.BoatId);
        DataContext = this;



    }

    public void ClickImageController(object sender, RoutedEventArgs e)
    {
        Main.MainContent.Navigate(new ListBoats(Main));
    }
    public void ConfirmButton(object sender, RoutedEventArgs e)
    {
        MessageBox.Show("changed reservation");
    }

    private void TimePicker_TextChanged(object sender, TextChangedEventArgs e)
    {
        var textBox = FocusManager.GetFocusedElement(this) as TextBox;
        if (textBox != null)
        {
        }
    }
}
