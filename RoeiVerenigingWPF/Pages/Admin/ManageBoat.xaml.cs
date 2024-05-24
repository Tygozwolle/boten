using System.Windows;
using DataAccessLibrary;
using RoeiVerenigingLibrary;
using System.Windows.Controls;
using DataAccessLibrary;
using RoeiVerenigingLibrary;
using RoeiVerenigingLibrary.Exceptions;

namespace RoeiVerenigingWPF.Pages.Admin
{
    public partial class ManageBoat : Page
    {
        public ManageBoat()
        {
            InitializeComponent();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            BoatService service = new BoatService(new BoatRepository());
            try
            {
                string firstName = FirstName.Text;
                string infix = Infix.Text;
                string lastName = LastName.Text;
                string email = Email.Text;
                string password = Password.Password;
                Boat createdBoat = service.Create(_mainWindow.LoggedInMember, firstName, infix, lastName, email,
                    password);
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