using DataAccessLibrary;
using RoeiVerenigingLibary;
using RoeiVerenigingWPF.Frames;
using System.Windows.Controls;

namespace RoeiVerenigingWPF.Pages
{
    /// <summary>
    /// Interaction logic for ListBoatsForDamageReport.xaml
    /// </summary>
    public partial class ListBoatsForDamageReport : Page
    {
        public List<Boat> boats { get; set; }
        public MainWindow MainWindow { set; get; }

        public ListBoatsForDamageReport(MainWindow mw)
        {
            InitializeComponent();
            BoatService service = new BoatService(new BoatRepository());
            this.DataContext = this;
            MainWindow = mw;
            boats = service.Getboats();
        }
        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (sender is Button)
            {
                Button casted = sender as Button;
                object command = casted.CommandParameter;
                int id = Int32.Parse(command.ToString());

                MainWindow.MainContent.Navigate(new CreateDamageReport(MainWindow, id));
            }
        }
    }
}
