using DataAccessLibary;
using RoeiVerenigingLibary.Exceptions;
using RoeiVerenigingLibary;
using RoeiVerenigingTestWPF.Frames;
using System.Windows;
using System.Windows.Controls;

namespace RoeiVerenigingTestWPF.Pages
{
    /// <summary>
    /// Interaction logic for CreateUser.xaml
    /// </summary>
    public partial class CreateUser : Page
    {
        private MainWindow _mainWindow;

        public CreateUser(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MemberService service = new MemberService(new MemberRepository());
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