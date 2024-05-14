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
    private Dictionary<string, CheckBox> _roleCheckBoxes = new Dictionary<string, CheckBox>();

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

        List<string> availableRoles = _service.GetAvailableRoles();
        foreach (string role in availableRoles)
        {
            CheckBox checkBox = new CheckBox { Content = role };
            _roleCheckBoxes[role] = checkBox;
            RolesPanel.Children.Add(checkBox);
        }

        foreach (string role in selectedMember.Roles)
        {
            if (_roleCheckBoxes.ContainsKey(role))
            {
                _roleCheckBoxes[role].IsChecked = true;
            }
        }
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            //get all data
            string firstName = FirstName.Text;
            string infix = Infix.Text;
            string lastName = LastName.Text;
            string email = Email.Text;
            if (!int.TryParse(Level.Text, out int level))
            {
                MessageBox.Show("Het niveau moet een nummer zijn");
                return;
            }
            
            List<string> selectedRoles = _roleCheckBoxes
                .Where(pair => pair.Value.IsChecked == true)
                .Select(pair => pair.Key)
                .ToList();
            //run the update methods from the service
            Member updatedMember = _service.Update(_mainWindow.LoggedInMember, _memberId, firstName, infix, lastName,
                email, level
            );
            _service.SetRoles(_memberId, selectedRoles);
            //check if updated
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