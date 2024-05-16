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
                throw new NotImplementedException("Schade");
            case Button button when button == EventsButton:
                ChangeColorOfRectangle(EventsRectangle);
                // MainWindow.MainContent.Navigate(new ViewReservations());
                throw new NotImplementedException("Evenementen");
                break;
            case Button button when button == ReserveButton:
                ChangeColorOfRectangle(ReservationRectangle);
                MainWindow.MainContent.Navigate(new ViewReservations(MainWindow));
                break;
        }
    }

    private void ChangeColorOfRectangle(Border rectangle)
    {
        switch (rectangle)
        {
            case Border border when rectangle == BoatRectangle:
                BoatRectangle.Background = new SolidColorBrush(Colors.White);
                BoatRectangle.BorderBrush = new SolidColorBrush(Colors.White);
                DamageRectangle.Background = new SolidColorBrush(Colors.Black);
                DamageRectangle.BorderBrush = new SolidColorBrush(Colors.Black);
                EventsRectangle.Background = new SolidColorBrush(Colors.Black);
                EventsRectangle.BorderBrush = new SolidColorBrush(Colors.Black);
                ReservationRectangle.Background = new SolidColorBrush(Colors.Black);
                ReservationRectangle.BorderBrush = new SolidColorBrush(Colors.Black);
                break;
            case Border border when rectangle == DamageRectangle:
                BoatRectangle.Background = new SolidColorBrush(Colors.Black);
                BoatRectangle.BorderBrush = new SolidColorBrush(Colors.Black);
                DamageRectangle.Background = new SolidColorBrush(Colors.White);
                DamageRectangle.BorderBrush = new SolidColorBrush(Colors.White);
                EventsRectangle.Background = new SolidColorBrush(Colors.Black);
                EventsRectangle.BorderBrush = new SolidColorBrush(Colors.Black);
                ReservationRectangle.Background = new SolidColorBrush(Colors.Black);
                ReservationRectangle.BorderBrush = new SolidColorBrush(Colors.Black);
                break;
            case Border border when rectangle == EventsRectangle:
                BoatRectangle.Background = new SolidColorBrush(Colors.Black);
                BoatRectangle.BorderBrush = new SolidColorBrush(Colors.Black);
                DamageRectangle.Background = new SolidColorBrush(Colors.Black);
                DamageRectangle.BorderBrush = new SolidColorBrush(Colors.Black);
                EventsRectangle.Background = new SolidColorBrush(Colors.White);
                EventsRectangle.BorderBrush = new SolidColorBrush(Colors.White);
                ReservationRectangle.Background = new SolidColorBrush(Colors.Black);
                ReservationRectangle.BorderBrush = new SolidColorBrush(Colors.Black);
                break;
            case Border border when rectangle == ReservationRectangle:
                BoatRectangle.Background = new SolidColorBrush(Colors.Black);
                BoatRectangle.BorderBrush = new SolidColorBrush(Colors.Black);
                DamageRectangle.Background = new SolidColorBrush(Colors.Black);
                DamageRectangle.BorderBrush = new SolidColorBrush(Colors.Black);
                EventsRectangle.Background = new SolidColorBrush(Colors.Black);
                EventsRectangle.BorderBrush = new SolidColorBrush(Colors.Black);
                ReservationRectangle.Background = new SolidColorBrush(Colors.White);
                ReservationRectangle.BorderBrush = new SolidColorBrush(Colors.White);
                break;
        }
    }
}