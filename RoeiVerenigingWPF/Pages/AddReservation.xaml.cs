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
        public DateTime? SelectedDate { get; set; }
        private DateTime? StartTime { get; set; }
        private DateTime? EndTime
        {
            get;
            set;
        }
//placeholder for logged in member
        private Member _member = new Member(30,  "rick", "wil", "roeien", "rick@windesheim.be", new List<string>());

        public DateTime? currentDateTime = DateTime.Now;

        public AddReservation()
        {
            InitializeComponent();
            DataContext = this;
            

        }

        public DateTime CurrentDateTime
        {
            get
            {
                return CurrentDateTime;
            }
            set
            {
                currentDateTime = value;
            }
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
            this.StartTime = BeginTimePicker.Value;
            this.EndTime = EndTimePicker.Value;
            this.SelectedDate = Calendar.SelectedDate;

            ReservationCreator r = new ReservationCreator(_member, 3, new ReservationRepository()); //om de een of andere reden pakt hij member.Id als Boat.Id
            try
            {
                if (r.TimeChecker(StartTime, EndTime))
                {
                    r.CommitToDb();
                }
            }
            catch(InvalidTimeException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        
    }
}