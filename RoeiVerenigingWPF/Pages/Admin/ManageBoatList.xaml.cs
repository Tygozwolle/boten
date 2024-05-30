using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DataAccessLibrary;
using RoeiVerenigingLibrary.Model;
using RoeiVerenigingLibrary.Services;
using RoeiVerenigingWPF.Frames;

namespace RoeiVerenigingWPF.Pages.Admin;

public partial class ManageBoatList : Page
{
    private readonly MainWindow _mainWindow;
    private readonly BoatService _service = new(new BoatRepository());

    public ManageBoatList(MainWindow mw)
    {
        DataContext = this;
        Boats = _service.GetBoats();
        _mainWindow = mw;
        InitializeComponent();
    }

    public List<Boat> Boats { get; set; }

    private void Grid_Click(object sender, MouseButtonEventArgs e)
    {
        if (sender is Grid)
        {
            var casted = sender as Grid;
            var command = casted.Tag;
            var id = int.Parse(command.ToString());

            _mainWindow.MainContent.Navigate(new ManageBoat(_mainWindow, _service.GetBoatById(id)));
        }
    }

    private void Button_Create_Click(object sender, RoutedEventArgs e)
    {
        _mainWindow.MainContent.Navigate(new ManageBoat(_mainWindow));
    }

    private void Page_Initialized(object sender, EventArgs e)
    {
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