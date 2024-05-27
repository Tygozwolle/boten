using DataAccessLibrary;
using RoeiVerenigingLibrary;
using RoeiVerenigingWPF.Frames;
using System.Windows;
using System.Windows.Controls;

namespace RoeiVerenigingWPF.Pages
{
    /// <summary>
    ///     Interaction logic for CreateDamageReport.xaml
    /// </summary>
    public partial class CreateDamageReport : Page
    {
        private readonly BoatService _serviceBoat = new BoatService(new BoatRepository());
        private readonly Boat boat;
        private Damage damage;
        private readonly MainWindow mainWindow;
        private readonly DamageService service = new DamageService(new DamageRepository());
        public CreateDamageReport(MainWindow mainWindow, int boatId)
        {
            this.mainWindow = mainWindow;
            boat = _serviceBoat.GetBoatById(boatId);
            DataContext= boat;
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            damage = service.CreateReport(mainWindow.LoggedInMember, boat, ___discription_.Text);
            mainWindow.MainContent.Navigate(new ViewDamage(mainWindow, damage));
        }
    }
}