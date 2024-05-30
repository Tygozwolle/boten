using System.Windows;
using System.Windows.Controls;
using DataAccessLibrary;
using RoeiVerenigingLibrary.Exceptions;
using RoeiVerenigingLibrary.Services;
using RoeiVerenigingTestWPF.Frames;

namespace RoeiVerenigingTestWPF.Pages;

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
        var service = new MemberService(new MemberRepository());
        try
        {
            var firstName = FirstName.Text;
            var infix = Infix.Text;
            var lastName = LastName.Text;
            var email = Email.Text;
            var updatedMember = service.Update(_mainWindow.LoggedInMember, firstName, infix, lastName, email
            );
            if (updatedMember != null)
            {
                _mainWindow.LoggedInMember = updatedMember;
                MessageBox.Show(
                    $"{updatedMember.FirstName} {updatedMember.Infix} {updatedMember.LastName} is gewijzigd");
            }
        }
        catch (CantAccesDatabaseException ex)
        {
            MessageBox.Show(ex.Message);
        }
        catch (IncorrectRightsException ex)
        {
            MessageBox.Show(ex.Message);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }
}