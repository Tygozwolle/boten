using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using Aspose.Email.Mime;
using DataAccessLibary;
using RoeiVerenigingLibary;
using RoeiVerenigingWPF.Frames;

namespace RoeiVerenigingWPF.Pages;

public partial class EditReservation : Page
{
    public Boat SelectedBoat { get; set; }
    public MainWindow Main { get; set; }
    public DateTime StartingTime { get; set; }
    public DateTime EndTime { get; set; }
    
    public Member member { get; set; }

    
    public EditReservation(MainWindow main,ViewReservations reservation)
    {
        Main = main;
        SelectedBoat = reservation.Boat;
        member = reservation.Member;
        StartingTime = reservation.StartTime;
        EndTime = reservation.EndTime;
        
        BeginTimePicker.Value = StartingTime;
        EndTimePicker.Value = EndTime;
        InitializeComponent();
        ReservationService service = new ReservationService(new ReservationRepository());
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
