using DataAccessLibrary;
using RoeiVerenigingLibrary.Services;
using RoeiVerenigingWPF.Frames;
using RoeiVerenigingWPF.Pages;
using RoeiVerenigingWPF.Pages.Member;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace RoeiVerenigingWPF.Components
{
    public partial class Buttons : UserControl
    {
        public Buttons()
        {
            InitializeComponent();
            ButtonsMenu_Loaded();
        }

        public MainWindow MainWindow { set; get; }

        private void ButtonsMenu_Loaded()
        {
            VerenigingsAfbeelding.Source =
                new BitmapImage(new Uri("/Images/twee-mensen-in-polyester-roeiboot.png", UriKind.Relative));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            switch (sender)
            {
                case Button button when button == BotenButton:
                    try
                    {
                        MainWindow.MainContent.Navigate(new AddReservation(MainWindow.LoggedInMember));
                        ChangeColorOfRectangle(BoatRectangle);
                    }
                    catch (Exception exception)
                    {
                        ExceptionTextBlock.Text = exception.Message;
                    }

                    break;
                case Button button when button == DamageButton:
                    try
                    {
                        if (MainWindow.LoggedInMember.Roles.Contains("materiaal_commissaris"))
                        {
                            MainWindow.MainContent.Navigate(new ManageDamageOverview(MainWindow));
                            ChangeColorOfRectangle(DamageRectangle);

                            break;
                        }

                        MainWindow.MainContent.Navigate(new DamageOverview(MainWindow));
                        ChangeColorOfRectangle(DamageRectangle);
                    }
                    catch (Exception exception)
                    {
                        ExceptionTextBlock.Text = exception.Message;
                    }

                    break;

                case Button button when button == EventResultButton:
                    try
                    {
                        ChangeColorOfRectangle(EventResultRectangle);
                        MainWindow.MainContent.Navigate(new EventResult(MainWindow,
                            new EventService(new EventRepository()).GetEventById(1)));
                    }
                    catch (Exception exception)
                    {
                        ExceptionTextBlock.Text = exception.Message;
                    }

                    break;

                case Button button when button == ReserveButton:
                    try
                    {
                        MainWindow.MainContent.Navigate(new ViewReservations(MainWindow));
                        ChangeColorOfRectangle(ReservationRectangle);
                    }
                    catch (Exception exception)
                    {
                        ExceptionTextBlock.Text = exception.Message;
                    }

                    break;
            }
        }

        private void ChangeColorOfRectangle(Grid rectangle)
        {
            switch (rectangle)
            {
                case { } when rectangle == BoatRectangle:
                    BoatRectangle.Visibility = Visibility.Visible;
                    DamageRectangle.Visibility = Visibility.Hidden;
                    EventResultRectangle.Visibility = Visibility.Hidden;
                    ReservationRectangle.Visibility = Visibility.Hidden;
                    break;
                case { } when rectangle == DamageRectangle:
                    BoatRectangle.Visibility = Visibility.Hidden;
                    DamageRectangle.Visibility = Visibility.Visible;
                    EventResultRectangle.Visibility = Visibility.Hidden;
                    ReservationRectangle.Visibility = Visibility.Hidden;
                    break;
                case { } when rectangle == EventResultRectangle:
                    BoatRectangle.Visibility = Visibility.Hidden;
                    DamageRectangle.Visibility = Visibility.Hidden;
                    EventResultRectangle.Visibility = Visibility.Visible;
                    ReservationRectangle.Visibility = Visibility.Hidden;
                    break;
                case { } when rectangle == ReservationRectangle:
                    BoatRectangle.Visibility = Visibility.Hidden;
                    DamageRectangle.Visibility = Visibility.Hidden;
                    EventResultRectangle.Visibility = Visibility.Hidden;
                    ReservationRectangle.Visibility = Visibility.Visible;
                    break;
            }
        }
    }
}