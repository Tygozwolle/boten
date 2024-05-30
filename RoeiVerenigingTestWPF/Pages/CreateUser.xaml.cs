using System.Windows;
using System.Windows.Controls;
using DataAccessLibrary;
using RoeiVerenigingLibrary.Exceptions;
using RoeiVerenigingLibrary.Services;
using RoeiVerenigingTestWPF.Frames;

namespace RoeiVerenigingTestWPF.Pages;

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
        var service = new MemberService(new MemberRepository());
        try
        {
            var firstName = FirstName.Text;
            var infix = Infix.Text;
            var lastName = LastName.Text;
            var email = Email.Text;
            var password = Password.Password;
            var createdMember = service.Create(_mainWindow.LoggedInMember, firstName, infix, lastName, email,
                password);
            if (createdMember != null)
                MessageBox.Show(
                    $"{createdMember.FirstName} {createdMember.Infix} {createdMember.LastName} is aangemaakt met lidnummer {createdMember.Id}");
        }
        catch (MemberAlreadyExistsException ex)
        {
            MessageBox.Show(ex.Message);
        }
        catch (IncorrectRightsException ex)
        {
            MessageBox.Show(ex.Message);
        }
    }
}