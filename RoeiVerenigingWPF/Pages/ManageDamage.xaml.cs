using System.Windows;
using System.Windows.Controls;
using DataAccessLibary;
using RoeiVerenigingLibary;
using RoeiVerenigingWPF.Frames;

namespace RoeiVerenigingWPF.Pages;

public partial class ManageDamage : Page
{
    private MainWindow _mainWindow { set; get; }
    public Damage Damage { set; get; }

    private DamageService _service = new DamageService(new DamageRepository());
    public ManageDamage(MainWindow mw, Damage damage)
    {
        InitializeComponent();
        DataContext = this;
        _mainWindow = mw;
        Damage = damage;
        Usable.IsChecked = damage.Usable;
        Fixed.IsChecked = damage.BoatFixed;
        Description.Text = damage.Description;
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        bool _fixed = false;
        bool _usable = false;
        if (Fixed.IsChecked == true)
            _fixed = true;
        if (Usable.IsChecked == true)
            _usable = true;
        _service.Update(Damage.Id, _fixed, _usable, Description.Text);
        _mainWindow.MainContent.Navigate(new ManageDamageOverview(_mainWindow));
    }
}