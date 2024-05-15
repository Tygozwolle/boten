using System.Windows.Controls;
using RoeiVerenigingLibary;
using RoeiVerenigingWPF.Frames;

namespace RoeiVerenigingWPF.Pages;

public partial class ManageDamage : Page
{
    private MainWindow _mainWindow { set; get; }
    public Damage Damage { set; get; }

    public ManageDamage(MainWindow mw, Damage damage)
    {
        InitializeComponent();
        DataContext = this;
        _mainWindow = mw;
        Damage = damage;
    }
}