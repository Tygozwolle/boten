using RoeiVerenigingWPF.Frames;
using RoeiVerenigingWPF.Pages;
using System.Security.Policy;
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
        switch (sender)
        {
            case Button button when button == BotenButton:

                //MainWindow.MainContent.Navigate(new CreateUser(MainWindow));

                throw new NotImplementedException("reservering");
                break;
            case Button button when button == DamageButton:
                //MainWindow.MainContent.Navigate(new );
                throw new NotImplementedException("damage");

            case Button button when button == EventsButton:
                //MainWindow.MainContent.Navigate(new );
                throw new NotImplementedException("events");

            case Button button when button == ReserveButton:
                MainWindow.MainContent.Navigate(new AddReservation(MainWindow.LoggedInMember, 1));
                // throw new NotImplementedException("reserve");
                break;
            default:
                break;
        }
    }
}