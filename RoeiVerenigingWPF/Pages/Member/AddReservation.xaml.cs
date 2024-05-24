using DataAccessLibary;
using RoeiVerenigingLibary;
using RoeiVerenigingLibary.Exceptions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace RoeiVerenigingWPF.Pages
{
    /// <summary>
    /// Interaction logic for AddReservation.xaml
    /// </summary>
    /// 
    public partial class AddReservation : Page
    {
        private Member _loggedInMember;

        private ReservationService _service = new(new ReservationRepository());
        private BoatService boatService = new BoatService(new BoatRepository());
        public Boat boat { get; set; }

        public AddReservation(Member loggedInMember, int boatId)
        {
            InitializeComponent();
            _loggedInMember = loggedInMember;
            boat = boatService.GetBoatById(boatId);
            DataContext = this;
        }

        // private void TimePicker_TextChanged(object sender, TextChangedEventArgs e)
        // {
        //     var textBox = FocusManager.GetFocusedElement(this) as TextBox;
        //     if (textBox != null)
        //     {
        //     }
        // }
        //
        // public void ConfirmButton(Object sender, RoutedEventArgs e)
        // {
        //     //check if all fields are valid
        //     DateTime? startTime = BeginTimePicker.Value;
        //     DateTime? endTime = EndTimePicker.Value;
        //     DateTime? selectedDate = calendar.SelectedDate;
        //     if (!startTime.HasValue || !endTime.HasValue || !selectedDate.HasValue)
        //     {
        //         MessageBox.Show("Selecteer een datum en begin en eindtijd!");
        //         return;
        //     }
        //
        //     //add date and time together
        //     DateTime startDateTime = selectedDate.Value.Date.Add(startTime.Value.TimeOfDay);
        //     DateTime endDateTime = selectedDate.Value.Date.Add(endTime.Value.TimeOfDay);
        //
        //     try
        //     {
        //         if (_service.TimeChecker(startTime, endTime))
        //         {
        //             _service.Create(_loggedInMember, boat.Id, startDateTime, endDateTime);
        //         }
        //     }
        //     catch (InvalidTimeException invalidTimeException)
        //     {
        //         MessageBox.Show(invalidTimeException.Message);
        //         return;
        //     }
        //     catch (MaxAmountOfReservationExceeded maxAmountOfReservationExceededException)
        //     {
        //         MessageBox.Show(maxAmountOfReservationExceededException.Message);
        //         return;
        //     }
        //     catch (ReservationNotInDaylightException ex)
        //     {
        //         MessageBox.Show(ex.Message);
        //         return;
        //     }
        //     catch (Exception ex)
        //     {
        //         MessageBox.Show(ex.Message);
        //         return;
        //     }
        //
        //     MessageBox.Show("Reservering aangemaakt");
        //     //todo: send to reservation overview
        // }
    }
}