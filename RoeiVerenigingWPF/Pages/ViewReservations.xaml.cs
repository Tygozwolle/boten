using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DataAccessLibary;
using RoeiVerenigingLibary;

namespace RoeiVerenigingWPF.Pages
{
    /// <summary>
    /// Interaction logic for ViewUsers.xaml
    /// </summary>
    public partial class ViewReservations : Page
    {
        private List<Reservation> _reservationList;
        public ViewReservations()
        {
             ReservationService service = new ReservationService(new ReservationRepository());

            _reservationList = service.GetReservations();
            InitializeComponent();
            this.DataContext = this;
           
            ___ReservationList_.ItemsSource = _reservationList;
            ___ReservationList_.Items.Filter = Filter;
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
                        ___ReservationList_.Items.SortDescriptions.Clear();
                        ___ReservationList_.Items.SortDescriptions.Add(new SortDescription(sendercast.Name,
                            ListSortDirection.Ascending));
                    }
                    else
                    {
                        ___ReservationList_.Items.SortDescriptions.Clear();
                        ___ReservationList_.Items.SortDescriptions.Add(new SortDescription(sendercast.Name,
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
            CollectionViewSource.GetDefaultView(___ReservationList_.ItemsSource).Refresh();
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
                result.Add(((item as Reservation).Id.ToString().IndexOf($"{IdFilter.Text}", StringComparison.OrdinalIgnoreCase) >= 0)) ; 
            }

            // filter name
            if (String.IsNullOrEmpty(FullNameFilter.Text))
            {
                result.Add(true);
            }
            else
            {
                result.Add(((item as Reservation).Member.FullName.IndexOf($"{FullNameFilter.Text}", StringComparison.OrdinalIgnoreCase) >= 0));
            }

            //Filter BoatId
            if (String.IsNullOrEmpty(BoatIdFilter.Text))
            {
                result.Add(true);
            }
            else
            {
                result.Add(((item as Reservation).BoatID.ToString().IndexOf($"{BoatIdFilter.Text}", StringComparison.OrdinalIgnoreCase) >= 0));
            }

            //Filter StartTime
            if (String.IsNullOrEmpty(StartTimeFilter.Text))
            {
                result.Add(true);
            }
            else
            {
                result.Add(((item as Reservation).StartTime.ToString(format).IndexOf($"{StartTimeFilter.Text}", StringComparison.OrdinalIgnoreCase) >= 0));
            }

            //Filter EndTime
            if (String.IsNullOrEmpty(EndTimeFilter.Text))
            {
                result.Add(true);
            }
            else
            {
                result.Add(((item as Reservation).EndTime.ToString(format).IndexOf($"{EndTimeFilter.Text}", StringComparison.OrdinalIgnoreCase) >= 0));
            }

            //Filter CreationDate
            if (String.IsNullOrEmpty(CreationDateFilter.Text))
            {
                result.Add(true);
            }
            else
            {
                result.Add(((item as Reservation).CreationDate.ToString(format).IndexOf($"{CreationDateFilter.Text}", StringComparison.OrdinalIgnoreCase) >= 0));
            }

            return !result.Contains(false);
        }

        private void ___EditReserevation__Click(object sender, RoutedEventArgs e)
        {
            Reservation selectedReservation = (Reservation)___ReservationList_.SelectedItem;

        }
    }
        
    }


