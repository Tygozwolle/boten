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
        private List<Reservation> _reservationList;
        private MainWindow _mainWindow;

        private Member getMember(MainWindow mainWindow)
        {
            return mainWindow.LoggedInMember;
        }

        public ViewReservations()
        {
            ReservationService service = new ReservationService(new ReservationRepository());

            _reservationList = service.GetReservations();
            InitializeComponent();
            this.DataContext = this;

            ReservationList.ItemsSource = _reservationList;
            ReservationList.Items.Filter = Filter;
        }

        public void SortReservation(object sender, RoutedEventArgs routedEventArgs)
        {
            try
            {
                ContextMenu sendercast = (System.Windows.Controls.ContextMenu)sender;
                string[] validStrings = { "Id", "FullName", "BoatId", "StartTime", "EndTime", "CreationDate" };
                if (validStrings.Contains(sendercast.Name))
                {
                    MenuItem routedEventArgsCast = (MenuItem)routedEventArgs.Source;
                    if (routedEventArgsCast.Header.ToString() == "Ascending")
                    {
                        ReservationList.Items.SortDescriptions.Clear();
                        ReservationList.Items.SortDescriptions.Add(new SortDescription(sendercast.Name,
                            ListSortDirection.Ascending));
                    }
                    else
                    {
                        ReservationList.Items.SortDescriptions.Clear();
                        ReservationList.Items.SortDescriptions.Add(new SortDescription(sendercast.Name,
                            ListSortDirection.Descending));
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        public void UpdateFilter(object sender, RoutedEventArgs routedEventArgs)
        {
            CollectionViewSource.GetDefaultView(ReservationList.ItemsSource).Refresh();
        }

        private bool Filter(object item)
        {
            const string format = "dd-MM-yyyy HH:mm:ss";
            List<bool> result = new List<bool>();
            // filter ID
            if (String.IsNullOrEmpty(IdFilter.Text))
            {
                result.Add(true);
            }
            else
            {
                result.Add(((item as Reservation).Id.ToString()
                    .IndexOf($"{IdFilter.Text}", StringComparison.OrdinalIgnoreCase) >= 0));
            }

            // filter name
            if (String.IsNullOrEmpty(FullNameFilter.Text))
            {
                result.Add(true);
            }
            else
            {
                result.Add(((item as Reservation).Member.FullName.IndexOf($"{FullNameFilter.Text}",
                    StringComparison.OrdinalIgnoreCase) >= 0));
            }

            //Filter BoatId
            if (String.IsNullOrEmpty(BoatIdFilter.Text))
            {
                result.Add(true);
            }
            else
            {
                result.Add(((item as Reservation).BoatId.ToString()
                    .IndexOf($"{BoatIdFilter.Text}", StringComparison.OrdinalIgnoreCase) >= 0));
            }

            //Filter StartTime
            if (String.IsNullOrEmpty(StartTimeFilter.Text))
            {
                result.Add(true);
            }
            else
            {
                result.Add(((item as Reservation).StartTime.ToString(format)
                    .IndexOf($"{StartTimeFilter.Text}", StringComparison.OrdinalIgnoreCase) >= 0));
            }

            //Filter EndTime
            if (String.IsNullOrEmpty(EndTimeFilter.Text))
            {
                result.Add(true);
            }
            else
            {
                result.Add(((item as Reservation).EndTime.ToString(format)
                    .IndexOf($"{EndTimeFilter.Text}", StringComparison.OrdinalIgnoreCase) >= 0));
            }

            //Filter CreationDate
            if (String.IsNullOrEmpty(CreationDateFilter.Text))
            {
                result.Add(true);
            }
            else
            {
                result.Add(((item as Reservation).CreationDate.ToString(format)
                    .IndexOf($"{CreationDateFilter.Text}", StringComparison.OrdinalIgnoreCase) >= 0));
            }

            return !result.Contains(false);
        }

        private void Reservation_Click(object sender, RoutedEventArgs e)
        {
            Reservation selectedReservation = (Reservation)ReservationList.SelectedItem;
            IdFilter.Text = selectedReservation.Id.ToString();
            FullNameFilter.Text = selectedReservation.Member.FullName;
            BoatIdFilter.Text = selectedReservation.BoatId.ToString();
            StartTimeFilter.Text = selectedReservation.StartTime.ToString("t");
            EndTimeFilter.Text = selectedReservation.StartTime.ToString("t");
            CreationDateFilter.Focusable = false;
            CreationDateFilter.Text = selectedReservation.CreationDate.ToString("g");
        }

        private void Confirm_Wijzigingen_Click(object sender, RoutedEventArgs e)
        {
            ReservationService service = new ReservationService(new ReservationRepository());
            service.ChangeReservation(_mainWindow.LoggedInMember, int.Parse(BoatIdFilter.Text),
                DateTime.Parse(StartTimeFilter.Text), DateTime.Parse(EndTimeFilter.Text));
            

        }
    }
}