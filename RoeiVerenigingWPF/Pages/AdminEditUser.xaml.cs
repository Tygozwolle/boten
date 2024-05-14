using System.Windows;
using System.Windows.Controls;
using DataAccessLibary;
using RoeiVerenigingLibary;
using RoeiVerenigingLibary.Exceptions;
using RoeiVerenigingWPF.Frames;

namespace RoeiVerenigingWPF.Pages;

public partial class AdminEditUser : Page
{
    private MainWindow _mainWindow;
    private int _memberId;
    private MemberService _service = new MemberService(new MemberRepository());

    public AdminEditUser(MainWindow mainWindow, int memberId)
    {
        InitializeComponent();
        _mainWindow = mainWindow;
        _memberId = memberId;

        Member selectedMember = _service.GetById(memberId);
        FirstName.Text = selectedMember.FirstName;
        Infix.Text = selectedMember.Infix;
        LastName.Text = selectedMember.LastName;
        Email.Text = selectedMember.Email;
        Level.Text = selectedMember.Level.ToString();
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            string firstName = FirstName.Text;
            string infix = Infix.Text;
            string lastName = LastName.Text;
            string email = Email.Text;
            if (!int.TryParse(Level.Text, out int level))
            {
                MessageBox.Show("Het niveau moet een nummer zijn");
                return;
            }

            Member updatedMember = _service.Update(_mainWindow.LoggedInMember, _memberId, firstName, infix, lastName,
                email, level
            );
            if (updatedMember != null)
            {
                MessageBox.Show(
                    $"{updatedMember.FirstName} {updatedMember.Infix} {updatedMember.LastName} is gewijzigd");
                _mainWindow.MainContent.Navigate(new ViewUsers(_mainWindow));
            }
        }
        catch (MemberAlreadyExistsException ex)
        {
            MessageBox.Show(ex.Message);
        }
        catch (IncorrectRightsExeption ex)
        {
            MessageBox.Show(ex.Message);
        }
    }
}