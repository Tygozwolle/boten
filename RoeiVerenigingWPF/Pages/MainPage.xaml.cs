using System.Windows.Controls;
using RoeiVerenigingWPF.Frames;

namespace RoeiVerenigingWPF.Pages;

public partial class MainPage : Page
{
    public MainPage(MainWindow mainWindow)
    {
        InitializeComponent();
        MainWindow = mainWindow;
    }

    public MainWindow MainWindow { get; set; }
}