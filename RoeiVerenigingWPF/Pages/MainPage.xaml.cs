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
            StatisticsFrame.Content = new ViewStatistics(mainWindow);
        }
        public MainWindow MainWindow { get; set; }
    }
}