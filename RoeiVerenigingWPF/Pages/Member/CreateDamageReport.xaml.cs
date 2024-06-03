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
        private readonly Boat _boat;
        private Damage _damage;
        private readonly MainWindow _mainWindow;
        private readonly DamageService _service = new DamageService(new DamageRepository());
        public CreateDamageReport(MainWindow mainWindow, int boatId)
        {
            this._mainWindow = mainWindow;
            _boat = _serviceBoat.GetBoatById(boatId);
            DataContext= _boat;
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _damage = _service.CreateReport(_mainWindow.LoggedInMember, _boat, DescriptionTextBox.Text);
            _mainWindow.MainContent.Navigate(new ViewDamage(_mainWindow, _damage));
        }
    }
}