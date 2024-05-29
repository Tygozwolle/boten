using System.Windows;
using System.Windows.Controls;
using DataAccessLibrary;
using RoeiVerenigingLibrary;
using RoeiVerenigingLibrary.Exceptions;
using RoeiVerenigingTestWPF.Frames;
using System.Windows;
using System.Windows.Controls;

namespace RoeiVerenigingTestWPF.Pages
{
    public partial class AdminEditUser : Page
    {
        private readonly MainWindow _mainWindow;
        private readonly int _memberId;
        private readonly Dictionary<string, CheckBox> _roleCheckBoxes = new Dictionary<string, CheckBox>();
        private readonly MemberService _service = new MemberService(new MemberRepository());

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

            var availableRoles = _service.GetAvailableRoles();
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
                if (!Int32.TryParse(Level.Text, out int level))
                {
                    MessageBox.Show("Het niveau moet een nummer zijn");
                    return;
                }

                var selectedRoles = _roleCheckBoxes
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
}