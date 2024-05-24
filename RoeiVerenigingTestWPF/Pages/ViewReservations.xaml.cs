using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using DataAccessLibrary;
using RoeiVerenigingLibary;
using RoeiVerenigingTestWPF.Frames;
using System.Windows;
using System.Windows.Controls;

namespace RoeiVerenigingTestWPF.Pages
{
    /// <summary>
    ///     Interaction logic for ViewUsers.xaml
    /// </summary>
    public partial class ViewReservations : Page
    {

        public ViewReservations(MainWindow mainWindow)
        {
            InitializeComponent();
            DataContext = this;
            ReservationService service = new ReservationService(new ReservationRepository());
            MainWindow = mainWindow;
            ReservationList = service.GetReservations(mainWindow.LoggedInMember);
        }
        public List<Reservation> ReservationList { get; set; }
        public MainWindow MainWindow { set; get; }

        public void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}