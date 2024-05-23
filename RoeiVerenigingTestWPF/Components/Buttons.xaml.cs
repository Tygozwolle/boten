using RoeiVerenigingTestWPF.Frames;
using RoeiVerenigingTestWPF.Pages;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace RoeiVerenigingTestWPF.Components
{
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
            Color reservationColor = Color.FromArgb(255, 122, 178, 178); // This represents the color #0e5172

            switch (rectangle)
            {
                case Border border when rectangle == BoatRectangle:
                    BoatRectangle.Background = new SolidColorBrush(reservationColor);
                    BoatRectangle.BorderBrush = new SolidColorBrush(reservationColor);
                    DamageRectangle.Background = new SolidColorBrush(Colors.Transparent);
                    DamageRectangle.BorderBrush = new SolidColorBrush(Colors.Transparent);
                    EventsRectangle.Background = new SolidColorBrush(Colors.Transparent);
                    EventsRectangle.BorderBrush = new SolidColorBrush(Colors.Transparent);
                    ReservationRectangle.Background = new SolidColorBrush(Colors.Transparent);
                    ReservationRectangle.BorderBrush = new SolidColorBrush(Colors.Transparent);
                    break;
                case Border border when rectangle == DamageRectangle:
                    BoatRectangle.Background = new SolidColorBrush(Colors.Transparent);
                    BoatRectangle.BorderBrush = new SolidColorBrush(Colors.Transparent);
                    DamageRectangle.Background = new SolidColorBrush(reservationColor);
                    DamageRectangle.BorderBrush = new SolidColorBrush(reservationColor);
                    EventsRectangle.Background = new SolidColorBrush(Colors.Transparent);
                    EventsRectangle.BorderBrush = new SolidColorBrush(Colors.Transparent);
                    ReservationRectangle.Background = new SolidColorBrush(Colors.Transparent);
                    ReservationRectangle.BorderBrush = new SolidColorBrush(Colors.Transparent);
                    break;
                case Border border when rectangle == EventsRectangle:
                    BoatRectangle.Background = new SolidColorBrush(Colors.Transparent);
                    BoatRectangle.BorderBrush = new SolidColorBrush(Colors.Transparent);
                    DamageRectangle.Background = new SolidColorBrush(Colors.Transparent);
                    DamageRectangle.BorderBrush = new SolidColorBrush(Colors.Transparent);
                    EventsRectangle.Background = new SolidColorBrush(reservationColor);
                    EventsRectangle.BorderBrush = new SolidColorBrush(reservationColor);
                    ReservationRectangle.Background = new SolidColorBrush(Colors.Transparent);
                    ReservationRectangle.BorderBrush = new SolidColorBrush(Colors.Transparent);
                    break;
                case Border border when rectangle == ReservationRectangle:
                    BoatRectangle.Background = new SolidColorBrush(Colors.Transparent);
                    BoatRectangle.BorderBrush = new SolidColorBrush(Colors.Transparent);
                    DamageRectangle.Background = new SolidColorBrush(Colors.Transparent);
                    DamageRectangle.BorderBrush = new SolidColorBrush(Colors.Transparent);
                    EventsRectangle.Background = new SolidColorBrush(Colors.Transparent);
                    EventsRectangle.BorderBrush = new SolidColorBrush(Colors.Transparent);
                    ReservationRectangle.Background = new SolidColorBrush(reservationColor);
                    ReservationRectangle.BorderBrush = new SolidColorBrush(reservationColor);
                    break;
            }
        }
    }
}