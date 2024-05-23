using RoeiVerenigingTestWPF.Frames;
using System.Windows.Controls;

namespace RoeiVerenigingTestWPF.Pages
{
    public partial class MainPage : Page
    {

        public MainPage(MainWindow mainWindow)
        {
            InitializeComponent();
            MainWindow = mainWindow;
        }
        public MainWindow MainWindow { get; set; }
    }
}