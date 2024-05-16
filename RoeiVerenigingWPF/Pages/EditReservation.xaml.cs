using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using RoeiVerenigingLibary;
using RoeiVerenigingWPF.Frames;

namespace RoeiVerenigingWPF.Pages;

public partial class EditReservation : Page
{
    public Boat Boat;
    public MainWindow Main;
    public DateTime startTime;
    public DateTime endTime;
    
    
    public EditReservation(MainWindow main)
    {
        Main = main;
        ViewReservations reservation = new ViewReservations();
        // Boat = reservation.Boat;
        // member = reservation.Member;
        // startTime = reservation.StartTime;
        // endTime = reservation.EndTime;
        //
        // BeginTimePicker.Value = startTime;
        // EndTimePicker.Value = endTime;
        // BoatPicker.Content = Boat.Id;
        InitializeComponent();
       

    }

    public void ClickImageController(object sender, RoutedEventArgs e)
    {
        Main.MainContent.Navigate(new ListBoats(Main));
    }
    public void ConfirmButton(object sender, RoutedEventArgs e)
    {
        
    }
    
    private void TimePicker_TextChanged(object sender, TextChangedEventArgs e)
    {
        var textBox = FocusManager.GetFocusedElement(this) as TextBox;
        if (textBox != null)
        {
        }
    }
}
