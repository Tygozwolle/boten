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
        switch (sender)
        {
            case Button button when button == BotenButton:
                MainWindow.MainContent.Navigate(new ListBoats(MainWindow));
                break;
            case Button button when button == DamageButton:
                //MainWindow.MainContent.Navigate(new );
                throw new NotImplementedException("Schade");
            case Button button when button == EventsButton:
                // MainWindow.MainContent.Navigate(new ViewReservations());
                throw new NotImplementedException("Evenementen");
                break;
            case Button button when button == ReserveButton:
                MainWindow.MainContent.Navigate(new ViewReservations(MainWindow));
                break;
        }
    }
}