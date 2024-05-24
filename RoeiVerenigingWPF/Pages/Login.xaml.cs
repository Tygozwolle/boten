using System.Windows;
using System.Windows.Controls;
using DataAccessLibrary;
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
#if !RELEASE
        DebugInlog();
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


#if !RELEASE
    private void DebugInlog()
    {
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
#if INGELOGTMATERIAAL
        IConfigurationRoot config = new ConfigurationBuilder().AddUserSecrets<Login>().Build();
        Email.Text = config["MATERIAAL:username"];
        Password.Password = config["MATERIAAL:password"];
#endif
#if INGELOGTEVENT
        IConfigurationRoot config = new ConfigurationBuilder().AddUserSecrets<Login>().Build();
        Email.Text = config["EVENT:username"];
        Password.Password = config["EVENT:password"];
#endif
    }
#endif
}