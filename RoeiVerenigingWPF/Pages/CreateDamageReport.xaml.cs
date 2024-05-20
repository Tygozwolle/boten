using DataAccessLibary;
using RoeiVerenigingLibary;
using RoeiVerenigingWPF.Frames;
using System.Windows;
using System.Windows.Controls;

namespace RoeiVerenigingWPF.Pages
{
    /// <summary>
    /// Interaction logic for CreateDamageReport.xaml
    /// </summary>
    public partial class CreateDamageReport : Page
    {
        private Damage damage;
        private DamageService service = new DamageService(new DamageRepository());
        private BoatService _serviceBoat = new BoatService(new BoatRepository());
        private Boat boat;
        private MainWindow mainWindow;
        public CreateDamageReport(MainWindow mainWindow, int boatId)
        {
            this.mainWindow = mainWindow;
            boat = _serviceBoat.Getboat(boatId);
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.damage= service.CreateReport(mainWindow.LoggedInMember, boat, ___discription_.Text);
            mainWindow.MainContent.Navigate(new ViewDamage(mainWindow , damage));
        }
    }
}
