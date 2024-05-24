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
                Member createdMember = service.Create(_mainWindow.LoggedInMember, firstName, infix, lastName, email,
                    password);
                if (createdMember != null)
                {
                    MessageBox.Show(
                        $"{createdMember.FirstName} {createdMember.Infix} {createdMember.LastName} is aangemaakt met lidnummer {createdMember.Id}");
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