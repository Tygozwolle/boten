using DataAccessLibary;
using RoeiVerenigingLibary;
using RoeiVerenigingWPF.Frames;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
        public CreateDamageReport(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow
            InitializeComponent();
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            damage = service.CreateReport(mainWindow.LoggedInMember, boat, ___discription_.Text)
        }
    }
}
