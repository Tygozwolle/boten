using System.Windows;
using System.Windows.Controls;
using DataAccessLibary;
using RoeiVerenigingLibary;
using RoeiVerenigingLibary.Exceptions;
using RoeiVerenigingTestWPF.Frames;

namespace RoeiVerenigingTestWPF.Pages;

public partial class Login : Page
{
    private MainWindow _mainWindow;

    public Login(MainWindow mainWindow)
    {
        InitializeComponent();
        _mainWindow = mainWindow;
    }

    public void LoginMember(object sender, RoutedEventArgs routedEventArgs)
    {
        string email = Email.Text;
        string password = Password.Password;

        MemberService service = new MemberService(new MemberRepository());
        try
        {
            _mainWindow.LoggedInMember = service.Login(email, password);
        }
        catch (IncorrectEmailOrPasswordException e)
        {
            MessageBox.Show(e.Message);
            return;
        }
        catch (InvalidEmailException e)
        {
            MessageBox.Show(e.Message);
            return;
        }

        _mainWindow.MainContent.Visibility = Visibility.Visible;
        // _mainWindow.LoginContent.Visibility = Visibility.Hidden;
        _mainWindow.MainContent.Navigate(new MainPage(_mainWindow));
    }
}