using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace RoeiVerenigingWPF.Views;

public partial class Buttons : UserControl
{
    public void ButtonsMenu_Loaded()
    {
        VerenigingsAfbeelding.Source =
            new BitmapImage(new Uri("/img/twee-mensen-in-polyester-roeiboot.png", UriKind.Relative));
    }
    public Buttons()
    {
        InitializeComponent();
        ButtonsMenu_Loaded();
    }
}