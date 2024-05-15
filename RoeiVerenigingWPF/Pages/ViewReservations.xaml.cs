using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
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
        public ViewReservations()
        {
            InitializeComponent();
            DataContext = this;
            ReservationService service = new ReservationService(new ReservationRepository());
            ReservationList = service.GetReservations();
        }

        public void Button_Click(object sender, RoutedEventArgs e)
        {
            
        }

        // public void ReservationClick(object sender, RoutedEventArgs e)
        // {
        //     Reservation selectedReservation = (Reservation)ReservationList.SelectedItem;
        //     IdFilter.Text = selectedReservation.Id.ToString();
        //     FullNameFilter.Text = selectedReservation.Member.FullName;
        //     BoatIdFilter.Text = selectedReservation.BoatId.ToString();
        //     StartTimeFilter.Text = selectedReservation.StartTime.ToString("t");
        //     EndTimeFilter.Text = selectedReservation.StartTime.ToString("t");
        //     CreationDateFilter.Focusable = false;
        //     CreationDateFilter.Text = selectedReservation.CreationDate.ToString("g");
        // }
        private void Control_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
        }
    }
}