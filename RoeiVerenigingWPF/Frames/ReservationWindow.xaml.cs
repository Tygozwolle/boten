using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace RoeiVerenigingWPF.Frames
{
    /// <summary>
    /// Interaction logic for ReservationWindow.xaml
    /// </summary>
    /// 
    public partial class ReservationWindow : Window
    {

        public DateTime currentDateTime = DateTime.Now;

        public ReservationWindow()
        {
            InitializeComponent();
            DataContext = this;

        }

        public DateTime CurrentDateTime
        {
            get
            {
                return currentDateTime;
            }
            set
            {
                currentDateTime = value;
            }
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
