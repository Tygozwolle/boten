using System.Windows;
using System.Windows.Controls;
using DataAccessLibary;
using RoeiVerenigingLibary;
using RoeiVerenigingTestWPF.Frames;

namespace RoeiVerenigingTestWPF.Pages;

/// <summary>
///     Interaction logic for ListBoats.xaml
/// </summary>
public partial class ListBoats : Page
{
    public ListBoats(MainWindow mw)
    {
        InitializeComponent();
        var service = new BoatService(new BoatRepository());
        DataContext = this;
        MainWindow = mw;
        boats = service.Getboats();
    }

    public List<Boat> boats { get; set; }
    public MainWindow MainWindow { set; get; }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button)
        {
            var casted = sender as Button;
            var command = casted.CommandParameter;
            var id = int.Parse(command.ToString());

            MainWindow.MainContent.Navigate(new AddReservation(MainWindow.LoggedInMember, id));
        }
    }
}