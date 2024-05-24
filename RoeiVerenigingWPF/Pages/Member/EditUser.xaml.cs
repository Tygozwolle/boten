using System.Windows;
using System.Windows.Controls;
using DataAccessLibrary;
using RoeiVerenigingLibary;
using RoeiVerenigingLibary.Exceptions;
using RoeiVerenigingWPF.Frames;
using System.Windows;
using System.Windows.Controls;

namespace RoeiVerenigingWPF.Pages
{
    public partial class EditUser : Page
    {
        private readonly MainWindow _mainWindow;

        public EditUser(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
            FirstName.Text = mainWindow.LoggedInMember.FirstName;
            Infix.Text = mainWindow.LoggedInMember.Infix;
            LastName.Text = mainWindow.LoggedInMember.LastName;
            Email.Text = mainWindow.LoggedInMember.Email;
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
                Member updatedMember = service.Update(_mainWindow.LoggedInMember, firstName, infix, lastName, email
                );
                if (updatedMember != null)
                {
                    _mainWindow.LoggedInMember = updatedMember;
                    MessageBox.Show(
                        $"{updatedMember.FirstName} {updatedMember.Infix} {updatedMember.LastName} is gewijzigd");
                }
            }
            catch (CantAccesDatabaseException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (IncorrectRightsExeption ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}