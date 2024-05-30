#region

using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using DataAccessLibrary;
using RoeiVerenigingLibrary.Model;
using RoeiVerenigingLibrary.Services;
using RoeiVerenigingTestWPF.Frames;

#endregion

namespace RoeiVerenigingTestWPF.Pages;

/// <summary>
///     Interaction logic for ViewUsers.xaml
/// </summary>
public partial class ViewUsers : Page
{
    private readonly MainWindow _mainWindow;
    private readonly List<Member> _memberList;

    public ViewUsers(MainWindow mainWindow)
    {
        MemberService service = new MemberService(new MemberRepository());
        InitializeComponent();
        _mainWindow = mainWindow;
        DataContext = this;
        _memberList = service.GetMembers();
        ___UserList_.ItemsSource = _memberList;
        ___UserList_.Items.Filter = Filter;
    }
    public Member SelectedMember { get; set; }

    public void SortMember(object sender, RoutedEventArgs routedEventArgs)
    {
        try
        {
            ContextMenu sendercast = (ContextMenu)sender;
            string[] validStrings = { "Id", "FirstName", "LastName", "Email" };
            if (validStrings.Contains(sendercast.Name))
            {
                MenuItem routedEventArgsCast = (MenuItem)routedEventArgs.Source;
                if (routedEventArgsCast.Header.ToString() == "Ascending")
                {
                    ___UserList_.Items.SortDescriptions.Clear();
                    ___UserList_.Items.SortDescriptions.Add(new SortDescription(sendercast.Name,
                        ListSortDirection.Ascending));
                }
                else
                {
                    ___UserList_.Items.SortDescriptions.Clear();
                    ___UserList_.Items.SortDescriptions.Add(new SortDescription(sendercast.Name,
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
        CollectionViewSource.GetDefaultView(___UserList_.ItemsSource).Refresh();
    }

    private bool Filter(object item)
    {
        var result = new List<bool>();
        // filter ID
        if (string.IsNullOrEmpty(IdFilter.Text))
        {
            result.Add(true);
        }
        else
        {
            result.Add((item as Member).Id.ToString()
                .IndexOf($"{IdFilter.Text}", StringComparison.OrdinalIgnoreCase) >= 0);
        }

        //Filter FirstName
        if (string.IsNullOrEmpty(FirstNameFilter.Text))
        {
            result.Add(true);
        }
        else
        {
            result.Add((item as Member).FirstName.IndexOf($"{FirstNameFilter.Text}",
                StringComparison.OrdinalIgnoreCase) >= 0);
        }

        //Filter LastName
        if (string.IsNullOrEmpty(LastNameFilter.Text))
        {
            result.Add(true);
        }
        else
        {
            result.Add((item as Member).LastName.IndexOf($"{LastNameFilter.Text}",
                StringComparison.OrdinalIgnoreCase) >= 0);
        }

        //Filter Email
        if (string.IsNullOrEmpty(EmailFilter.Text))
        {
            result.Add(true);
        }
        else
        {
            result.Add((item as Member).Email.IndexOf($"{EmailFilter.Text}", StringComparison.OrdinalIgnoreCase) >=
                       0);
        }

        //Filter Roles
        if (string.IsNullOrEmpty(RolesFilter.Text))
        {
            result.Add(true);
        }
        else
        {
            result.Add((item as Member).RolesString.IndexOf($"{RolesFilter.Text}",
                StringComparison.OrdinalIgnoreCase) >= 0);
        }

        return !result.Contains(false);
    }

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