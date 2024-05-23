using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using RoeiVerenigingWPF.Frames;
using RoeiVerenigingWPF.Pages;

namespace RoeiVerenigingWPF.Components;

public partial class Buttons : UserControl
{
    public Buttons()
    {
        InitializeComponent();
        ButtonsMenu_Loaded();
    }

    public MainWindow MainWindow { set; get; }

    public void ButtonsMenu_Loaded()
    {
        VerenigingsAfbeelding.Source =
            new BitmapImage(new Uri("/Images/twee-mensen-in-polyester-roeiboot.png", UriKind.Relative));
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        if (MainWindow.LoggedInMember == null)
        {
            MessageBox.Show("Log eerst in");
            return;
        }

        switch (sender)
        {
            case Button button when button == BotenButton:
                ChangeColorOfRectangle(BoatRectangle);
                MainWindow.MainContent.Navigate(new ListBoats(MainWindow));
                break;
            case Button button when button == DamageButton:
                ChangeColorOfRectangle(DamageRectangle);

                if (MainWindow.LoggedInMember.Roles.Contains("materiaal_commissaris"))
                {
                    MainWindow.MainContent.Navigate(new ManageDamageOverview(MainWindow));
                    break;
                }

                MainWindow.MainContent.Navigate(new DamageOverview(MainWindow));
                break;

            case Button button when button == EventsButton:
                ChangeColorOfRectangle(EventsRectangle);
                // MainWindow.MainContent.Navigate(new ViewReservations());
                break;

            case Button button when button == ReserveButton:
                ChangeColorOfRectangle(ReservationRectangle);
                MainWindow.MainContent.Navigate(new ViewReservations(MainWindow));
                break;
        }
    }

    private void ChangeColorOfRectangle(Grid rectangle)
    {
        var reservationColor = Color.FromArgb(255, 122, 178, 178); // This represents the color #0e5172

        switch (rectangle)
        {
            case Grid border when rectangle == BoatRectangle:
                BoatRectangle.Visibility = Visibility.Visible;
                DamageRectangle.Visibility = Visibility.Hidden;
                EventsRectangle.Visibility = Visibility.Hidden;
                ReservationRectangle.Visibility = Visibility.Hidden;
                break;
            case Grid border when rectangle == DamageRectangle:
                BoatRectangle.Visibility = Visibility.Hidden;
                DamageRectangle.Visibility = Visibility.Visible;
                EventsRectangle.Visibility = Visibility.Hidden;
                ReservationRectangle.Visibility = Visibility.Hidden;
                break;
            case Grid grid when rectangle == EventsRectangle:
                BoatRectangle.Visibility = Visibility.Hidden;
                DamageRectangle.Visibility = Visibility.Hidden;
                EventsRectangle.Visibility = Visibility.Visible;
                ReservationRectangle.Visibility = Visibility.Hidden;
                break;
            case Grid grid when rectangle == ReservationRectangle:
                BoatRectangle.Visibility = Visibility.Hidden;
                DamageRectangle.Visibility = Visibility.Hidden;
                EventsRectangle.Visibility = Visibility.Hidden;
                ReservationRectangle.Visibility = Visibility.Visible;
                break;
        }
    }
}