using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using DataAccessLibary;
using RoeiVerenigingLibary;
using RoeiVerenigingWPF.Frames;

namespace RoeiVerenigingWPF.Pages
{
    /// <summary>
    /// Interaction logic for ViewUsers.xaml
    /// </summary>
    public partial class ViewReservations : Page
    {
        public Boat Boat { get; set; }
        public Member Member { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        
        public List<Reservation> ReservationList { get; set; }
        public MainWindow MainWindow { set; get; }
        
        public ViewReservations(){}

        public ViewReservations(MainWindow mainWindow)
        {
            InitializeComponent();
            DataContext = this;
            ReservationService service = new ReservationService(new ReservationRepository());
            MainWindow = mainWindow;
            ReservationList = service.GetReservations(mainWindow.LoggedInMember);
        }

        // public void Button_Click(object sender, RoutedEventArgs e)
        // {
        //     List<string> textBlockValues = GetTextblockvaluesAsString(MainGrid);
        //     foreach (string value in textBlockValues)
        //     {
        //         MessageBox.Show(value);
        //     }
        // }

        private List<string> GetTextblockvaluesAsString(DependencyObject parent)
        {
            List<string> values = new List<string> ();
            GetAllValues(parent, values);
        
            return values;
        }
        
        private List<string> GetAllValues(DependencyObject parent, List<string> list)
        {
            int count = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < count; i++)
            {
                var childValue = VisualTreeHelper.GetChild(parent, i);
                
                if (childValue is TextBlock textblock)
                {
                    list.Add(textblock.Text);
                }
                
                GetAllValues(childValue, list);
            }
        
            return list;
        }

        public void SelectReservation(object sender, RoutedEventArgs e)
        {
            Reservation selectedItem = (Reservation)ReservationListFinder.SelectedItem;
            StartTime = selectedItem.StartTime;
            EndTime = selectedItem.EndTime;
            Boat = selectedItem.Boat;
            Member = selectedItem.Member;
            MainWindow.MainContent.Navigate(new EditReservation(MainWindow, this));
        }
    }
}