using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using DataAccessLibary;
using RoeiVerenigingLibary;
using RoeiVerenigingWPF.Frames;

namespace RoeiVerenigingWPF.Pages
{
    /// <summary>
    /// Interaction logic for ViewUsers.xaml
    /// </summary>
    public partial class ViewReservations : Page
    {
        public string BoatFoto { get; set; }
        public List<Reservation> ReservationList { get; set; }
        public MainWindow MainWindow { set; get; }

        public ViewReservations(MainWindow mainWindow)
        {
            InitializeComponent();
            DataContext = this;
            ReservationService service = new ReservationService(new ReservationRepository());
            MainWindow = mainWindow;
            ReservationList = service.GetReservations(mainWindow.LoggedInMember);
        }

        public void Button_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}