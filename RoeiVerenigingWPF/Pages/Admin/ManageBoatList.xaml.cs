using DataAccessLibrary;
using RoeiVerenigingLibrary;
using RoeiVerenigingWPF.Frames;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace RoeiVerenigingWPF.Pages.Admin
{
    public partial class ManageBoatList : Page
    {
        private MainWindow _mainWindow;
        private BoatService _service = new BoatService(new BoatRepository());
        private List<Boat> Boats { get; set; }

        public ManageBoatList(MainWindow mw)
        {
            DataContext = this;
            Boats = _service.GetBoats();
            _mainWindow = mw;
            InitializeComponent();
        }

        private void Grid_Click(object sender, MouseButtonEventArgs e)
        {
            if (sender is Grid)
            {
                Grid casted = sender as Grid;
                object command = casted.Tag;
                int id = Int32.Parse(command.ToString());

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
                this.Dispatcher.Invoke(() => { ListView.Items.Refresh(); });
            }).Start();
        }
    }
}