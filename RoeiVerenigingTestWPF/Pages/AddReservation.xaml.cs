#region

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DataAccessLibrary;
using RoeiVerenigingLibrary.Model;
using RoeiVerenigingLibrary.Services;

#endregion

namespace RoeiVerenigingTestWPF.Pages;

/// <summary>
///     Interaction logic for AddReservation.xaml
/// </summary>
public partial class AddReservation : Page
{
    private int _boatId;
    private Member _loggedInMember;

    private ReservationService _service = new ReservationService(new ReservationRepository());

    public AddReservation(Member loggedInMember, int boatId)
    {
        InitializeComponent();
        //todo: use boat_id from selected boat, set it in this constructor
        _boatId = boatId;
        _loggedInMember = loggedInMember;
        DataContext = this;
    }

    private void TimePicker_TextChanged(object sender, TextChangedEventArgs e)
    {
        TextBox? textBox = FocusManager.GetFocusedElement(this) as TextBox;
        if (textBox != null)
        {
        }
    }

    public void ConfirmButton(object sender, RoutedEventArgs e)
    {
        //check if all fields are valid
        // DateTime? startTime = BeginTimePicker.Value;
        // DateTime? endTime = EndTimePicker.Value;
        // DateTime? selectedDate = calendar.SelectedDate;
        // if (!startTime.HasValue || !endTime.HasValue || !selectedDate.HasValue)
        // {
        //     MessageBox.Show("Selecteer een datum en begin en eindtijd!");
        //     return;
        // }
        //
        // //add date and time together
        // DateTime startDateTime = selectedDate.Value.Date.Add(startTime.Value.TimeOfDay);
        // DateTime endDateTime = selectedDate.Value.Date.Add(endTime.Value.TimeOfDay);
        //
        // try
        // {
        //     if (_service.TimeChecker(startTime, endTime))
        //     {
        //         _service.Create(_loggedInMember, _boatId, startDateTime, endDateTime);
        //     }
        // }
        // catch (InvalidTimeException invalidTimeException)
        // {
        //     MessageBox.Show(invalidTimeException.Message);
        //     return;
        // }
        // catch (MaxAmountOfReservationExceeded maxAmountOfReservationExceededException)
        // {
        //     MessageBox.Show(maxAmountOfReservationExceededException.Message);
        //     return;
        // }
        // catch (ArgumentOutOfRangeException argumentOutOfRangeException)
        // {
        //     MessageBox.Show(argumentOutOfRangeException.Message);
        //     return;
        // }
        //
        // MessageBox.Show("Reservering aangemaakt");
        //todo: send to reservation overview
    }
}