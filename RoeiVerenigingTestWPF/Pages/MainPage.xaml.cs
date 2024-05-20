using System.Windows.Controls;
using RoeiVerenigingTestWPF.Frames;

namespace RoeiVerenigingTestWPF.Pages;

public partial class MainPage : Page
{
    public MainWindow MainWindow { get; set; }

    public MainPage(MainWindow mainWindow)
    {
        InitializeComponent();
        MainWindow = mainWindow;
    }
}