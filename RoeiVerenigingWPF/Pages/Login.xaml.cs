using System.Windows;
using System.Windows.Controls;
using DataAccessLibary;
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
        }
        //todo: send to main page
    }
}