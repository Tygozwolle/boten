using RoeiVerenigingWPF.Frames;
using RoeiVerenigingWPF.Pages;
using System.Security.Policy;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace RoeiVerenigingWPF.Components;

public partial class Buttons : UserControl
{
    public MainWindow main {  set;  get; }
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
            case Button button when button == ___BotenButton_:
                //main.MainContent.Navigate(new );
                throw new NotImplementedException("reservering");

            case Button button when button == ___DamageButton_:
                //main.MainContent.Navigate(new );
                throw new NotImplementedException("damage");

            case Button button when button == ___EventsButton_:
                //main.MainContent.Navigate(new );
                throw new NotImplementedException("events");

            case Button button when button == ___ReserveButton_:
                //main.MainContent.Navigate(new );
                throw new NotImplementedException("reserve");

            default:
                break;
        }
    }
}