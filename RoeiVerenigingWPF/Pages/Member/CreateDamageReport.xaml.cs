using System.Windows;
using System.Windows.Controls;
using DataAccessLibrary;
using RoeiVerenigingLibrary.Model;
using RoeiVerenigingLibrary.Services;
using RoeiVerenigingWPF.Frames;

namespace RoeiVerenigingWPF.Pages.member;

/// <summary>
///     Interaction logic for CreateDamageReport.xaml
/// </summary>
public partial class CreateDamageReport : Page
{
    private readonly BoatService _serviceBoat = new(new BoatRepository());
    private readonly Boat boat;
    private readonly MainWindow mainWindow;
    private readonly DamageService service = new(new DamageRepository());
    private Damage damage;

    public CreateDamageReport(MainWindow mainWindow, int boatId)
    {
        this.mainWindow = mainWindow;
        boat = _serviceBoat.GetBoatById(boatId);
        DataContext = boat;
        InitializeComponent();
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        damage = service.CreateReport(mainWindow.LoggedInMember, boat, ___discription_.Text);
        mainWindow.MainContent.Navigate(new ViewDamage(mainWindow, damage));
    }
}