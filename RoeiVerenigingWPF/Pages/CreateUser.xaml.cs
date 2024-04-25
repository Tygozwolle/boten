using DataAccessLibary;
using RoeiVerenigingLibary.Exceptions;
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
                Member createdMember = service.Create(_mainWindow.LoggedInMember, firstName, infix, lastName, email, password);
                if (createdMember != null)
                {
                    MessageBox.Show($"{createdMember.FirstName} {createdMember.Infix} {createdMember.LastName} is aangemaakt met lidnummer {createdMember.Id}");
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
