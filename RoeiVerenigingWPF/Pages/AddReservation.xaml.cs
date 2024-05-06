using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DataAccessLibary;
using RoeiVerenigingLibary;
using RoeiVerenigingLibary.Exceptions;

namespace RoeiVerenigingWPF.Frames
{
    /// <summary>
    /// Interaction logic for AddReservation.xaml
    /// </summary>
    /// 
    public partial class AddReservation
    {
        private Member _loggedInMember;

        public DateTime? currentDateTime = DateTime.Now;

        private ReservationService _service = new ReservationService(new ReservationRepository());
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
            DateTime? StartTime = BeginTimePicker.Value;
            DateTime? EndTime = EndTimePicker.Value;
            DateTime? SelectedDate = Calendar.SelectedDate;
            if (!StartTime.HasValue || !EndTime.HasValue || !SelectedDate.HasValue)
            {
                return;
            }

            //add date and time together
            DateTime startDateTime = SelectedDate.Value.Date.Add(StartTime.Value.TimeOfDay);
            DateTime endDateTime = SelectedDate.Value.Date.Add(EndTime.Value.TimeOfDay);

            try
            {
                if (_service.TimeChecker(StartTime, EndTime))
                {
                    _service.Create(_loggedInMember, _boatId, startDateTime, endDateTime);
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