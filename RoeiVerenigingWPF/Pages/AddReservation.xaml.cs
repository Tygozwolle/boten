using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace RoeiVerenigingWPF.Frames
{
    /// <summary>
    /// Interaction logic for AddReservation.xaml
    /// </summary>
    /// 
    public partial class AddReservation
    {
        public DateTime currentDateTime = DateTime.Now;

        public AddReservation()
        {
            InitializeComponent();
            DataContext = this;
        }

        public DateTime CurrentDateTime
        {
            get { return currentDateTime; }
            set { currentDateTime = value; }
        }


        void TimePicker_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = FocusManager.GetFocusedElement(this) as TextBox;
            if (textBox != null)
            {
            }
        }
    }
}