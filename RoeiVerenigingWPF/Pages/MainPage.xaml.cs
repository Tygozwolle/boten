using RoeiVerenigingWPF.Frames;
using System.Windows.Controls;

namespace RoeiVerenigingWPF.Pages
{
    public partial class MainPage : Page
    {

        public MainPage(MainWindow mainWindow)
        {
            InitializeComponent();
            MainWindow = mainWindow;
        }
        public MainWindow MainWindow { get; set; }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }
    }
}