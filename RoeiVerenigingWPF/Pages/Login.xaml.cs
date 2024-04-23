using System.Windows;
using System.Windows.Controls;
using DataAccessLibary;
using RoeiVerenigingLibary;
using RoeiVerenigingLibary.Exceptions;
using RoeiVerenigingWPF.Frames;

namespace RoeiVerenigingWPF.Pages;

public partial class Login : Page
{
    private MainWindow MainWindow;

    public Login(MainWindow mainWindow)
    {
        InitializeComponent();
        MainWindow = mainWindow;
    }

    public void LoginMember(object sender, RoutedEventArgs routedEventArgs)
    {
        string email = Email.Text;
        string password = Password.Password;

        MemberService service = new MemberService(new MemberRepository());
        try
        {
            MainWindow.LoggedInMember = service.Login(email, password);
        }
        catch (IncorrectEmailOrPasswordException e)
        {
            MessageBox.Show(e.Message);
        }
    }
}