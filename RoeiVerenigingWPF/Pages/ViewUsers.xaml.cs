using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using DataAccessLibary;
using RoeiVerenigingLibary;
using RoeiVerenigingWPF.Frames;

namespace RoeiVerenigingWPF.Pages
{
    /// <summary>
    /// Interaction logic for ViewUsers.xaml
    /// </summary>
    public partial class ViewUsers : Page
    {
        private List<Member> _memberList;
        public Member SelectedMember { get; set; }
        private MainWindow _mainWindow;

        public ViewUsers(MainWindow mainWindow)
        {
            MemberService service = new MemberService(new MemberRepository());
            InitializeComponent();
            _mainWindow = mainWindow;
            DataContext = this;
            _memberList = service.GetMembers();
            UserList.ItemsSource = _memberList;
            // UserList.Items.Filter = Filter;
        }

        public void SortMember(object sender, RoutedEventArgs routedEventArgs)
        {
            try
            {
                ContextMenu sendercast = (System.Windows.Controls.ContextMenu)sender;
                string[] validStrings = { "Id", "FirstName", "LastName", "Email" };
                if (validStrings.Contains(sendercast.Name))
                {
                    MenuItem routedEventArgsCast = (MenuItem)routedEventArgs.Source;
                    if (routedEventArgsCast.Header.ToString() == "Ascending")
                    {
                        UserList.Items.SortDescriptions.Clear();
                        UserList.Items.SortDescriptions.Add(new SortDescription(sendercast.Name,
                            ListSortDirection.Ascending));
                    }
                    else
                    {
                        UserList.Items.SortDescriptions.Clear();
                        UserList.Items.SortDescriptions.Add(new SortDescription(sendercast.Name,
                            ListSortDirection.Descending));
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        public void UpdateFilter(object sender, RoutedEventArgs routedEventArgs)
        {
            CollectionViewSource.GetDefaultView(UserList.ItemsSource).Refresh();
        }

        // private bool Filter(object item)
        // {
        //     List<bool> result = new List<bool>();
        //     // filter ID
        //     if (String.IsNullOrEmpty(IdFilter.Text))
        //     {
        //         result.Add(true);
        //     }
        //     else
        //     {
        //         result.Add(((item as Member).Id.ToString()
        //             .IndexOf($"{IdFilter.Text}", StringComparison.OrdinalIgnoreCase) >= 0));
        //     }
        //
        //     //Filter FirstName
        //     if (String.IsNullOrEmpty(FirstNameFilter.Text))
        //     {
        //         result.Add(true);
        //     }
        //     else
        //     {
        //         result.Add(((item as Member).FirstName.IndexOf($"{FirstNameFilter.Text}",
        //             StringComparison.OrdinalIgnoreCase) >= 0));
        //     }
        //
        //     //Filter LastName
        //     if (String.IsNullOrEmpty(LastNameFilter.Text))
        //     {
        //         result.Add(true);
        //     }
        //     else
        //     {
        //         result.Add(((item as Member).LastName.IndexOf($"{LastNameFilter.Text}",
        //             StringComparison.OrdinalIgnoreCase) >= 0));
        //     }
        //
        //     //Filter Email
        //     if (String.IsNullOrEmpty(EmailFilter.Text))
        //     {
        //         result.Add(true);
        //     }
        //     else
        //     {
        //         result.Add(((item as Member).Email.IndexOf($"{EmailFilter.Text}", StringComparison.OrdinalIgnoreCase) >=
        //                     0));
        //     }
        //
        //     //Filter Roles
        //     if (String.IsNullOrEmpty(RolesFilter.Text))
        //     {
        //         result.Add(true);
        //     }
        //     else
        //     {
        //         result.Add(((item as Member).RolesString.IndexOf($"{RolesFilter.Text}",
        //             StringComparison.OrdinalIgnoreCase) >= 0));
        //     }
        //
        //     return !result.Contains(false);
        // }

        private void ___EditMember__Click(object sender, RoutedEventArgs e)
        {
            if (SelectedMember == null)
            {
                MessageBox.Show("Selecteer een lid");
                return;
            }

            _mainWindow.MainContent.Navigate(new AdminEditUser(_mainWindow, SelectedMember.Id));
        }
    }
}