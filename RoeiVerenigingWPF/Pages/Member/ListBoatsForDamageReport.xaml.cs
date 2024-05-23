using System.Windows;
using System.Windows.Controls;
using DataAccessLibary;
using RoeiVerenigingLibary;
using RoeiVerenigingWPF.Frames;

namespace RoeiVerenigingWPF.Pages;

/// <summary>
///     Interaction logic for ListBoatsForDamageReport.xaml
/// </summary>
public partial class ListBoatsForDamageReport : Page
{
    public ListBoatsForDamageReport(MainWindow mw)
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

            MainWindow.MainContent.Navigate(new CreateDamageReport(MainWindow, id));
        }
    }
}