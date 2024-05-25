using DataAccessLibrary;
using RoeiVerenigingLibrary;
using RoeiVerenigingWPF.Frames;
using System.Windows;
using System.Windows.Controls;

namespace RoeiVerenigingWPF.Pages.Admin
{
    public partial class ManageBoatList : Page
    {
        private MainWindow _mainWindow;
        private BoatService _service = new BoatService(new BoatRepository());
        public List<Boat> Boats { get; set; }
        public ManageBoatList(MainWindow mw)
        {
            DataContext = this;
            Boats = _service.Getboats();
            _mainWindow = mw;
            InitializeComponent();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button)
            {
                Button casted = sender as Button;
                object command = casted.CommandParameter;
                int id = Int32.Parse(command.ToString());

                _mainWindow.MainContent.Navigate(new ManageBoat(_mainWindow, _service.GetBoatById(id)));
            }
        }
        private void Button_Create_Click(object sender, RoutedEventArgs e)
        {
            _mainWindow.MainContent.Navigate(new ManageBoat(_mainWindow));
        }
    }
}