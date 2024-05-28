using DataAccessLibrary;
using RoeiVerenigingLibrary;
using RoeiVerenigingWPF.Frames;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace RoeiVerenigingWPF.Pages
{
    /// <summary>
    ///     Interaction logic for ListBoatsForDamageReport.xaml
    /// </summary>
    public partial class ListBoatsForDamageReport : Page
    {

        public ListBoatsForDamageReport(MainWindow mw)
        {
            InitializeComponent();
            BoatService service = new BoatService(new BoatRepository());
            DataContext = this;
            MainWindow = mw;
            Boats = service.GetBoats();
        }
        public List<Boat> Boats { get; set; }
        public MainWindow MainWindow { set; get; }
        private BoatService _service = new BoatService(new BoatRepository());

        private void Grid_Click(object sender, MouseButtonEventArgs e)
        {
            if (sender is Grid)
            {
                Grid casted = sender as Grid;
                object command = casted.Tag;
                int id = Int32.Parse(command.ToString());

                MainWindow.MainContent.Navigate(new CreateDamageReport(MainWindow, id));
            }
        }
        private void ListView_Loaded(object sender, RoutedEventArgs e)
        {
            new Thread(async () =>
            {
                _service.GetImageBoats(Boats);
                this.Dispatcher.Invoke(() =>
                {
                    ListView.Items.Refresh();
                });
            }).Start();
        }
    }
}