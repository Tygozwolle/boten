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
        private MainWindow mainWindow;
        private Boat boat = new Boat(1, true, 4, 1);
        public CreateDamageReport(MainWindow mainWindow, Member loggedInMember, int boatId)
        {
            this.mainWindow = mainWindow;
            InitializeComponent();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.damage = service.CreateReport(mainWindow.LoggedInMember, boat, ___discription_.Text);
            mainWindow.MainContent.Navigate(new QRCodePage(damage.Id));
        }
    }
}
