using System.Windows;
using DataAccessLibrary;
using RoeiVerenigingLibrary;
using System.Windows.Controls;
using DataAccessLibrary;
using RoeiVerenigingLibrary;
using RoeiVerenigingLibrary.Exceptions;
using RoeiVerenigingWPF.Frames;

namespace RoeiVerenigingWPF.Pages.Admin
{
    public partial class ManageBoat : Page
    {
        private MainWindow _mainWindow;
        public ManageBoat(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
            InitializeComponent();
            Captain.IsChecked = false;
        }
        private void ToggleButtonClick(object sender, RoutedEventArgs e)
        {
            if (Captain.IsChecked == true)
            {
                Captain.Content = "Stuurman aanwezig";
            }
            else
            {
                Captain.Content = "Stuurman afwezig";
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            BoatService service = new BoatService(new BoatRepository());
            try
            {
                string name = Name.Text;
                string discription = Description.Text;
                string seats = Seats.Text;
                string level = Level.Text;
                bool captain = Captain.IsPressed;
                Boat createdBoat = service.Create(_mainWindow.LoggedInMember, name, discription, Int32.Parse(seats), captain, Int32.Parse(level));
                if (createdBoat != null)
                {
                    MessageBox.Show(
                        $"{createdBoat.Name} {createdBoat.Description} {createdBoat.Level} is aangemaakt met bootnummer {createdBoat.Id}");
                }
            }
            catch (MemberAlreadyExistsException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (IncorrectRightsExeption ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}