using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using DataAccessLibrary;
using RoeiVerenigingLibrary.Model;
using RoeiVerenigingLibrary.Services;
using RoeiVerenigingWPF.Frames;
using Color = System.Windows.Media.Color;

namespace RoeiVerenigingWPF.Pages.Admin;

/// <summary>
///     Interaction logic for ViewUsers.xaml
/// </summary>
public partial class ViewUsers : Page
{
    private readonly SolidColorBrush _borderColor = new(Color.FromArgb(255, 19, 114, 160));

    private readonly SolidColorBrush
        _evenRowColor = new(Color.FromArgb(255, 232, 246, 252)); // Background color for even rows

    private readonly MainWindow _mainWindow;
    private readonly List<Member> _memberList;

    private readonly SolidColorBrush
        _oddRowColor = new(Color.FromArgb(255, 182, 227, 251)); // Background color for odd rows

    private readonly SolidColorBrush _textColor = new(Color.FromArgb(255, 4, 48, 73));

    public ViewUsers(MainWindow mainWindow)
    {
        var service = new MemberService(new MemberRepository());
        InitializeComponent();
        _mainWindow = mainWindow;
        Search_Icon.Source = new BitmapImage(new Uri("/Images/Icons/search.png", UriKind.Relative));
        DataContext = this;
        _memberList = service.GetMembers();
        PopulateUserList(_memberList);
    }

    public Member SelectedMember { get; set; }

    public void PopulateUserList(List<Member> memberList)
    {
        UserStackPanel.Children.Clear();
        for (var i = 0; i < memberList.Count; i++)
        {
            var member = memberList[i];
            var grid = new Grid();
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(10) });

            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(50) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(150) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(150) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(200) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(250) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(250) });

            grid.Children.Add(new TextBlock
            {
                Text = member.Id.ToString(), VerticalAlignment = VerticalAlignment.Center, FontSize = 16,
                Foreground = _textColor
            });
            Grid.SetColumn(grid.Children[0], 0);

            grid.Children.Add(new TextBlock
            {
                Text = member.FirstName, VerticalAlignment = VerticalAlignment.Center, FontSize = 16,
                Foreground = _textColor
            });
            Grid.SetColumn(grid.Children[1], 1);

            grid.Children.Add(new TextBlock
            {
                Text = member.Infix, VerticalAlignment = VerticalAlignment.Center, FontSize = 16,
                Foreground = _textColor
            });
            Grid.SetColumn(grid.Children[2], 2);

            grid.Children.Add(
                new TextBlock
                {
                    Text = member.LastName, VerticalAlignment = VerticalAlignment.Center, FontSize = 16,
                    Foreground = _textColor
                });
            Grid.SetColumn(grid.Children[3], 3);

            grid.Children.Add(new TextBlock
            {
                Text = member.Email, VerticalAlignment = VerticalAlignment.Center, FontSize = 16,
                Foreground = _textColor
            });
            Grid.SetColumn(grid.Children[4], 4);

            grid.Children.Add(new TextBlock
            {
                Text = member.RolesString, VerticalAlignment = VerticalAlignment.Center, FontSize = 16,
                Foreground = _textColor
            });
            Grid.SetColumn(grid.Children[5], 5);

            var border = new Border
            {
                BorderThickness = new Thickness(1),
                BorderBrush = _borderColor,
                CornerRadius = new CornerRadius(5),
                Padding = new Thickness(10),
                Child = grid,
                Background = i % 2 == 0 ? _evenRowColor : _oddRowColor // Alternate row background color
            };

            border.MouseLeftButtonUp += (sender, e) =>
            {
                // Select the user here
            };

            // Track last click time and clicked border
            var lastClickTime = DateTime.MinValue;
            Border lastClickedBorder = null;

            // Handle PreviewMouseLeftButtonDown event for double-click
            border.PreviewMouseLeftButtonDown += (sender, e) =>
            {
                // Check if it's a double-click
                if (lastClickedBorder == sender && (DateTime.Now - lastClickTime).TotalMilliseconds < 500)
                {
                    var clickedUser = _memberList[UserStackPanel.Children.IndexOf((UIElement)sender)];

                    // Open a new page with the selected user information
                    var userDetailsPage = new AdminEditUser(_mainWindow, clickedUser.Id);
                    NavigationService.Navigate(userDetailsPage);
                }

                lastClickedBorder = (Border)sender;
                lastClickTime = DateTime.Now;
            };

            UserStackPanel.Children.Add(border);
        }
    }

    private void EditMember_Click(object sender, RoutedEventArgs e)
    {
        if (SelectedMember == null)
        {
            MessageBox.Show("Selecteer een lid");
            return;
        }

        _mainWindow.MainContent.Navigate(new AdminEditUser(_mainWindow, SelectedMember.Id));
    }


    private void ButtonFilterUsers(object sender, RoutedEventArgs e)
    {
        var clickedButton = sender as Button;
        var selectedMemberList = new List<Member>();

        if (clickedButton != null)
        {
            // Use the Name property to identify which button was clicked
            switch (clickedButton.Name)
            {
                case "Id":
                    selectedMemberList = _memberList.OrderBy(member => member.Id).ToList();
                    break;
                case "FirstName":
                    selectedMemberList = _memberList.OrderBy(member => member.FirstName).ToList();
                    break;
                case "Infix":
                    selectedMemberList = _memberList.OrderBy(member => member.Infix).ToList();
                    break;
                case "LastName":
                    selectedMemberList = _memberList.OrderBy(member => member.LastName).ToList();
                    break;
                case "Email":
                    selectedMemberList = _memberList.OrderBy(member => member.Email).ToList();
                    break;
                case "Roles":
                    selectedMemberList = _memberList.OrderBy(member => member.RolesString).ToList();
                    break;
            }

            PopulateUserList(selectedMemberList);
        }
    }

    public void TextFilterUsers(object sender, RoutedEventArgs e)
    {
        var selectedTextbox = sender as TextBox;
        var selectedMemberList = new List<Member>();

        if (selectedTextbox != null)
            try
            {
                switch (selectedTextbox.Name)
                {
                    case "SearchId":
                        selectedMemberList = _memberList
                            .Where(member => member.Id.ToString().Contains(selectedTextbox.Text))
                            .ToList();
                        break;
                    case "SearchFirstName":
                        selectedMemberList = _memberList
                            .Where(member => member.FirstName.ToString().Contains(selectedTextbox.Text))
                            .ToList();
                        break;
                    case "SearchInfix":
                        selectedMemberList = _memberList
                            .Where(member =>
                                member.Infix != null && member.Infix.ToString().Contains(selectedTextbox.Text))
                            .ToList();
                        break;
                    case "SearchLastName":
                        selectedMemberList = _memberList
                            .Where(member => member.LastName.ToString().Contains(selectedTextbox.Text))
                            .ToList();
                        break;
                    case "SearchEmail":
                        selectedMemberList = _memberList
                            .Where(member => member.Email.ToString().Contains(selectedTextbox.Text))
                            .ToList();
                        break;
                    case "SearchRoles":
                        selectedMemberList = _memberList
                            .Where(member => member.RolesString.ToString().Contains(selectedTextbox.Text))
                            .ToList();
                        break;
                }

                PopulateUserList(selectedMemberList);
            }


            catch (NullReferenceException exception)
            {
                PopulateUserList(new List<Member>());
            }
    }

    private void Search_Button_OnClick(object sender, RoutedEventArgs e)
    {
        if (SearchBar.Visibility == Visibility.Visible)
            SearchBar.Visibility = Visibility.Hidden;
        else
            SearchBar.Visibility = Visibility.Visible;
    }
}