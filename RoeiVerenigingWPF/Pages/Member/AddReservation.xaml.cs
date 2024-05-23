using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DataAccessLibary;
using RoeiVerenigingLibary;
using RoeiVerenigingLibary.Exceptions;

namespace RoeiVerenigingWPF.Pages;

/// <summary>
///     Interaction logic for AddReservation.xaml
/// </summary>
public partial class AddReservation : Page
{
    private readonly Member _loggedInMember;

    private readonly ReservationService _service = new(new ReservationRepository());
    private readonly BoatService boatService = new(new BoatRepository());

    public AddReservation(Member loggedInMember, int boatId)
    {
        InitializeComponent();
        _loggedInMember = loggedInMember;
        boat = boatService.GetBoatById(boatId);
        DataContext = this;
    }

    public Boat boat { get; set; }

    private void TimePicker_TextChanged(object sender, TextChangedEventArgs e)
    {
        var textBox = FocusManager.GetFocusedElement(this) as TextBox;
        if (textBox != null)
        {
        }
    }

    public void ConfirmButton(object sender, RoutedEventArgs e)
    {
        //check if all fields are valid
        var startTime = BeginTimePicker.Value;
        var endTime = EndTimePicker.Value;
        var selectedDate = calendar.SelectedDate;
        if (!startTime.HasValue || !endTime.HasValue || !selectedDate.HasValue)
        {
            MessageBox.Show("Selecteer een datum en begin en eindtijd!");
            return;
        }

        //add date and time together
        var startDateTime = selectedDate.Value.Date.Add(startTime.Value.TimeOfDay);
        var endDateTime = selectedDate.Value.Date.Add(endTime.Value.TimeOfDay);

        try
        {
            if (_service.TimeChecker(startTime, endTime))
                _service.Create(_loggedInMember, boat.Id, startDateTime, endDateTime);
        }
        catch (InvalidTimeException invalidTimeException)
        {
            MessageBox.Show(invalidTimeException.Message);
            return;
        }
        catch (MaxAmountOfReservationExceeded maxAmountOfReservationExceededException)
        {
            MessageBox.Show(maxAmountOfReservationExceededException.Message);
            return;
        }
        catch (ReservationNotInDaylightException ex)
        {
            MessageBox.Show(ex.Message);
            return;
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
            return;
        }

        MessageBox.Show("Reservering aangemaakt");
        //todo: send to reservation overview
    }
}