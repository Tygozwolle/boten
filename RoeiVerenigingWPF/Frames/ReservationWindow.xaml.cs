using System.Windows;

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
    }
}
