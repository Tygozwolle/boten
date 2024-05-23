using System.Windows;
using System.Windows.Controls;
using DataAccessLibary;
using RoeiVerenigingLibary;
using RoeiVerenigingWPF.Frames;

namespace RoeiVerenigingWPF.Pages;

/// <summary>
///     Interaction logic for ViewUsers.xaml
/// </summary>
public partial class ViewReservations : Page
{
    public ViewReservations(MainWindow mainWindow)
    {
        InitializeComponent();
        DataContext = this;
        var service = new ReservationService(new ReservationRepository());
        MainWindow = mainWindow;
        ReservationList = service.GetReservations(mainWindow.LoggedInMember);
    }

    public List<Reservation> ReservationList { get; set; }
    public MainWindow MainWindow { set; get; }

    public void SelectReservation(object sender, RoutedEventArgs e)
    {
        if (sender is Button)
        {
            var casted = sender as Button;
            var command = casted.CommandParameter;
            var idReservation = int.Parse(command.ToString());

            MainWindow.MainContent.Navigate(new EditReservation(MainWindow, idReservation));
        }
    }
}