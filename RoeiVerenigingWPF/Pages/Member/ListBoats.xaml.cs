using DataAccessLibrary;
using RoeiVerenigingLibrary;
using RoeiVerenigingWPF.Frames;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace RoeiVerenigingWPF.Pages
{
    /// <summary>
    ///     Interaction logic for ListBoats.xaml
    /// </summary>
    public partial class ListBoats : Page
    {
        private BoatService _service = new BoatService(new BoatRepository());
        public ListBoats(MainWindow mw)
        {
            InitializeComponent();
            BoatService service = new BoatService(new BoatRepository());
            DataContext = this;
            MainWindow = mw;
            boats = service.GetBoats();
        }

        public List<Boat> boats { get; set; }
        public MainWindow MainWindow { set; get; }

        private void Grid_Click(object sender, MouseButtonEventArgs e)
        {
            if (sender is Grid)
            {
                Grid casted = sender as Grid;
                object command = casted.Tag;
                int id = Int32.Parse(command.ToString());

                MainWindow.MainContent.Navigate(new AddReservation(MainWindow.LoggedInMember));
            }
        }
        
        private void ListView_Loaded(object sender, RoutedEventArgs e)
        {
            new Thread(async () =>
            {
                _service.GetImageBoats(boats);
                this.Dispatcher.Invoke(() =>
                {
                    ListView.Items.Refresh();
                });
            }).Start();
        }
    }
}