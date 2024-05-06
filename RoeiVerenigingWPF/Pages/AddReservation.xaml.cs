using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DataAccessLibary;
using RoeiVerenigingLibary;
using RoeiVerenigingLibary.Exceptions;
using RoeiVerenigingWPF.Frames;

namespace RoeiVerenigingWPF.Pages
{
    /// <summary>
    /// Interaction logic for AddReservation.xaml
    /// </summary>
    /// 
    public partial class AddReservation : Page
    {
        private Member _loggedInMember;

        public DateTime? currentDateTime = DateTime.Now;

        private ReservationService _service = new (new ReservationRepository());
        private int _boatId;

        public AddReservation(Member loggedInMember, int boatId)
        {
            InitializeComponent();
            //todo: use boat_id from selected boat, set it in this constructor
            _boatId = boatId;
            _loggedInMember = loggedInMember;
            DataContext = this;
        }

        public DateTime CurrentDateTime
        {
            get { return CurrentDateTime; }
            set { currentDateTime = value; }
        }


        private void TimePicker_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = FocusManager.GetFocusedElement(this) as TextBox;
            if (textBox != null)
            {
            }
        }

        public void ConfirmButton(Object sender, RoutedEventArgs e)
        {
            //check if all fields are valid
            DateTime? startTime = BeginTimePicker.Value;
            DateTime? endTime = EndTimePicker.Value;
            DateTime? selectedDate = Calendar.SelectedDate;
            if (!startTime.HasValue || !endTime.HasValue || !selectedDate.HasValue)
            {
                MessageBox.Show("Selecteer een datum en begin en eindtijd!");
                return;
            }

            //add date and time together
            DateTime startDateTime = selectedDate.Value.Date.Add(startTime.Value.TimeOfDay);
            DateTime endDateTime = selectedDate.Value.Date.Add(endTime.Value.TimeOfDay);

            try
            {
                if (_service.TimeChecker(startTime, endTime))
                {
                    Reservation reservation =
                        new Reservation(_loggedInMember, _boatId, startDateTime, endDateTime);
                    _service.Create(reservation);
                }
            }
            catch (InvalidTimeException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            MessageBox.Show("Reservering aangemaakt");
            //todo: send to reservation overview
        }
    }
}