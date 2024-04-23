using System.Windows;
using RoeiVerenigingWPF.Pages;

namespace RoeiVerenigingWPF.Frames;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        MainContent.Navigate(new Login());
    }
}