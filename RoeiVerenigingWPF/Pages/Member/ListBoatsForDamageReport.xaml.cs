using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DataAccessLibrary;
using RoeiVerenigingLibrary.Model;
using RoeiVerenigingLibrary.Services;
using RoeiVerenigingWPF.Frames;

namespace RoeiVerenigingWPF.Pages.member;

/// <summary>
///     Interaction logic for ListBoatsForDamageReport.xaml
/// </summary>
public partial class ListBoatsForDamageReport : Page
{
    private readonly BoatService _service = new(new BoatRepository());

    public ListBoatsForDamageReport(MainWindow mw)
    {
        InitializeComponent();
        var service = new BoatService(new BoatRepository());
        DataContext = this;
        MainWindow = mw;
        Boats = service.GetBoats();
    }

    public List<Boat> Boats { get; set; }
    public MainWindow MainWindow { set; get; }

    private void Grid_Click(object sender, MouseButtonEventArgs e)
    {
        if (sender is Grid)
        {
            var casted = sender as Grid;
            var command = casted.Tag;
            var id = int.Parse(command.ToString());

            MainWindow.MainContent.Navigate(new CreateDamageReport(MainWindow, id));
        }
    }

    private void ListView_Loaded(object sender, RoutedEventArgs e)
    {
        new Thread(async () =>
        {
            _service.GetImageBoats(Boats);
            Dispatcher.Invoke(() => { ListView.Items.Refresh(); });
        }).Start();
    }
}