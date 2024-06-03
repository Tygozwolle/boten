using DataAccessLibrary;
using RoeiVerenigingLibrary;
using RoeiVerenigingWPF.Frames;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace RoeiVerenigingWPF.Pages
{
    public partial class ChangePassword : Page
    {
        private readonly MainWindow _mainWindow;

        public ChangePassword(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (ExceptionTextBlock.Foreground != Brushes.MediumSeaGreen)
            {
                ExceptionTextBlock.Foreground = Brushes.Red;
                MemberService service = new MemberService(new MemberRepository());
                try
                {
                    string password = CurrentPassword.Password;
                    string newPassword = NewPassword.Password;
                    string newPasswordConfirm = NewPasswordConfirm.Password;
                    service.ChangePassword(_mainWindow.LoggedInMember, password, newPassword, newPasswordConfirm);
                }
                catch (Exception ex)
                {
                    ExceptionTextBlock.Text = ex.Message;
                    return;
                }

                ExceptionTextBlock.Text = "Het wachtwoord is veranderd!";
                ExceptionTextBlock.Foreground = Brushes.MediumSeaGreen;
                ContinueButton.Content = "Verder";
            }
            else
            {
                _mainWindow.MainContent.Navigate(new MainPage(_mainWindow));
            }
        }
    }
}