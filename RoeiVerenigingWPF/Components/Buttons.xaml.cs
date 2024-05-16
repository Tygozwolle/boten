using System.Windows;
using RoeiVerenigingWPF.Frames;
using RoeiVerenigingWPF.Pages;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace RoeiVerenigingWPF.Components;

public partial class Buttons : UserControl
{
    public MainWindow MainWindow { set; get; }

    public void ButtonsMenu_Loaded()
    {
        VerenigingsAfbeelding.Source =
            new BitmapImage(new Uri("/Images/twee-mensen-in-polyester-roeiboot.png", UriKind.Relative));
    }

    public Buttons()
    {
        InitializeComponent();
        ButtonsMenu_Loaded();
    }

    private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        if (MainWindow.LoggedInMember == null)
        {
            MessageBox.Show("Log eerst in");
            return;
        }
        switch (sender)
        {
            case Button button when button == BotenButton:
                MainWindow.MainContent.Navigate(new ListBoats(MainWindow));
                break;
            case Button button when button == DamageButton:
                if (MainWindow.LoggedInMember.Roles.Contains("materiaal_commissaris"))
                {
                    MainWindow.MainContent.Navigate(new ManageDamageOverview(MainWindow));
                    break;
                }
                MainWindow.MainContent.Navigate(new DamageOverview(MainWindow));
                break;
            case Button button when button == EventsButton:
                MainWindow.MainContent.Navigate(new ViewReservations());
                break;
            case Button button when button == ReserveButton:
                MainWindow.MainContent.Navigate(new AddReservation(MainWindow.LoggedInMember, 1));
                break;
            default:
                break;
        }
    }
}