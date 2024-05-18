using System.Windows;
using System.Windows.Controls;
using DataAccessLibary;
using Microsoft.Extensions.Configuration;
using RoeiVerenigingLibary;
using RoeiVerenigingLibary.Exceptions;
using RoeiVerenigingWPF.Frames;

namespace RoeiVerenigingWPF.Pages;

public partial class Login : Page
{
    private MainWindow _mainWindow;

    public Login(MainWindow mainWindow)
    {
        InitializeComponent();
        _mainWindow = mainWindow;
#if INGELOGD
        IConfigurationRoot config = new ConfigurationBuilder().AddUserSecrets<Login>().Build();
        Email.Text = config["USER:username"];
        Password.Password = config["USER:password"];
#endif
#if INGELOGTBEHEER
        IConfigurationRoot config = new ConfigurationBuilder().AddUserSecrets<Login>().Build();
        Email.Text = config["ADMIN:username"];
        Password.Password = config["ADMIN:password"];
#endif
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
        _mainWindow.LoginContent.Visibility = Visibility.Hidden;
        _mainWindow.MainContent.Navigate(new MainPage(_mainWindow));
    }
}