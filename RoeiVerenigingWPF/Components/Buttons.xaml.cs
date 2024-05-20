using System.Windows;
using RoeiVerenigingWPF.Frames;
using RoeiVerenigingWPF.Pages;
using System.Windows.Controls;
using System.Windows.Media;
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
                ChangeColorOfRectangle(BoatRectangle);
                MainWindow.MainContent.Navigate(new ListBoats(MainWindow));
                break;
            case Button button when button == DamageButton:
                ChangeColorOfRectangle(DamageRectangle);
                //MainWindow.MainContent.Navigate(new );
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
        Color reservationColor = Color.FromArgb(255, 122, 178, 178); // This represents the color #0e5172

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