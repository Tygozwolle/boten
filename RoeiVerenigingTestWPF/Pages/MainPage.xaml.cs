#region

using System.Windows.Controls;
using RoeiVerenigingTestWPF.Frames;

#endregion

namespace RoeiVerenigingTestWPF.Pages;

public partial class MainPage : Page
{

    public MainPage(MainWindow mainWindow)
    {
        InitializeComponent();
        MainWindow = mainWindow;
    }
    public MainWindow MainWindow { get; set; }
}