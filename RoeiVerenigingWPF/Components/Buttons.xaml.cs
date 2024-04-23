using RoeiVerenigingWPF.Frames;
using System.Security.Policy;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace RoeiVerenigingWPF.Components;

public partial class Buttons : UserControl
{
    public MainWindow main { public set; public get; }
    public void ButtonsMenu_Loaded()
    {
        VerenigingsAfbeelding.Source =
            new BitmapImage(new Uri("/Images/twee-mensen-in-polyester-roeiboot.png", UriKind.Relative));
        
    }
    public Buttons()
    {
        InitializeComponent();
        ButtonsMenu_Loaded();
        main.MainContent.Navigate();
    }
}