using System.Windows;
using RoeiVerenigingWPF.Frames;
using System.Windows.Controls;
using System.Windows.Input;
using RoeiVerenigingWPF.Pages.Admin;

namespace RoeiVerenigingWPF.Pages
{
    public partial class MainPage : Page
    {
        public MainPage(MainWindow mainWindow)
        {
            InitializeComponent();
            MainWindow = mainWindow;
            // StatisticsFrame.Content = new ViewStatistics(mainWindow);
            
        }
        public MainWindow MainWindow { get; set; }

        public void TrendButtonClick(object sender, RoutedEventArgs e)
        {
            MainWindow.MainContent.Navigate(new ViewTrends(new ViewStatistics(this.MainWindow)));
        }
    }
}