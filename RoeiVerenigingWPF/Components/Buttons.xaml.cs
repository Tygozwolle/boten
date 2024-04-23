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
        if(sender == ___BotenButton_)
        {
            //main.MainContent.Navigate(new );
            throw new NotImplementedException("reservering");
        }
        else if(sender == ___DamageButton_)
        {
            //  main.MainContent.Navigate(new );
            throw new NotImplementedException("damage");
        }
        else if(sender == ___EventsButton_)
        {
            //  main.MainContent.Navigate(new );
            throw new NotImplementedException("events");
        }
        else if(sender == ___ReserveButton_)
        {
            //  main.MainContent.Navigate(new );
            throw new NotImplementedException("reserve");
        }
    }
}