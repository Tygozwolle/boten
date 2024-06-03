using System.Windows;
using System.Windows.Controls;
using DataAccessLibrary;
using RoeiVerenigingLibrary;
using RoeiVerenigingLibrary.Exceptions;
using RoeiVerenigingWPF.Frames;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

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
            if (ExceptionTextBlock.Foreground != Brushes.MediumSeaGreen)
            {
                MemberService service = new MemberService(new MemberRepository());
                try
                {
                    string firstName = FirstName.Text;
                    string infix = Infix.Text;
                    string lastName = LastName.Text;
                    string email = Email.Text;
                    RoeiVerenigingLibrary.Member updatedMember = service.Update(_mainWindow.LoggedInMember, firstName, infix, lastName, email
                    );
                    if (updatedMember != null)
                    {
                        _mainWindow.LoggedInMember = updatedMember;
                        ExceptionTextBlock.Text =
                            $"{updatedMember.FirstName} {updatedMember.Infix} {updatedMember.LastName} is gewijzigd";
                        ExceptionTextBlock.Foreground = Brushes.MediumSeaGreen;
                        ContinueButton.Content = "Verder";
                    }
                }
                catch (CantAccesDatabaseException ex)
                {
                    ExceptionTextBlock.Text = ex.Message;
                }
                catch (IncorrectRightsException ex)
                {
                    ExceptionTextBlock.Text = ex.Message;
                }
                catch (Exception ex)
                {
                    ExceptionTextBlock.Text = ex.Message;
                }
            }
            else
            {
                _mainWindow.MainContent.Navigate(new MainPage(_mainWindow)); 
            }
            
        }
    }
}