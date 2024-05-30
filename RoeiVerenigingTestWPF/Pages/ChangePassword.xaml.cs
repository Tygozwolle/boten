using System.Windows;
using System.Windows.Controls;
using DataAccessLibrary;
using RoeiVerenigingLibrary.Services;
using RoeiVerenigingTestWPF.Frames;

namespace RoeiVerenigingTestWPF.Pages;

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
        var service = new MemberService(new MemberRepository());
        try
        {
            var password = CurrentPassword.Password;
            var newPassword = NewPassword.Password;
            var newPasswordConfirm = NewPasswordConfirm.Password;
            service.ChangePassword(_mainWindow.LoggedInMember, password, newPassword, newPasswordConfirm);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
            return;
        }

        MessageBox.Show("Wachtwoord is veranderd");
    }
}