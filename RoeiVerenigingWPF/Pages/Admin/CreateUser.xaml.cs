using DataAccessLibrary;
using RoeiVerenigingLibrary;
using RoeiVerenigingLibrary.Exceptions;
using RoeiVerenigingWPF.Frames;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace RoeiVerenigingWPF.Pages
{
    /// <summary>
    ///     Interaction logic for CreateUser.xaml
    /// </summary>
    public partial class CreateUser : Page
    {
        private readonly MainWindow _mainWindow;

        public CreateUser(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (ExceptionText.Foreground != Brushes.MediumSeaGreen)
            {
                MemberService service = new MemberService(new MemberRepository());
                try
                {
                    string firstName = FirstName.Text;
                    string infix = Infix.Text;
                    string lastName = LastName.Text;
                    string email = Email.Text;
                    string password = Password.Password;
                    RoeiVerenigingLibrary.Member createdMember = service.Create(_mainWindow.LoggedInMember, firstName,
                        infix, lastName, email,
                        password);
                    if (createdMember != null)
                    {
                        ExceptionText.Foreground = Brushes.MediumSeaGreen;
                        ExceptionText.Text =
                            $"{createdMember.FirstName} {createdMember.Infix} {createdMember.LastName} is aangemaakt met lidnummer {createdMember.Id}";
                        ContinueButton.Content = "Verder";
                    }
                }
                catch (MemberAlreadyExistsException ex)
                {
                    ExceptionText.Text = ex.Message;
                }
                catch (IncorrectRightsException ex)
                {
                    ExceptionText.Text = ex.Message;
                }
            }
            else
            {
                _mainWindow.MainContent.Navigate(new MainPage(_mainWindow));
            }
        }
    }
}