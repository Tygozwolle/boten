using System.Windows;
using RoeiVerenigingWPF.Frames;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using RoeiVerenigingWPF.Pages;

namespace RoeiVerenigingWPF.Components;

public partial class Header : UserControl
{
    public MainWindow MainWindow { set; get; }

    public Header()
    {
        InitializeComponent();
        DataContext = this;
        LoadButtonIcons();
    }

    private void LoadButtonIcons()
    {
        UserAdd_Icon.Source = new BitmapImage(new Uri("/Images/Icons/user-plus.png", UriKind.Relative));
        Users_Icon.Source = new BitmapImage(new Uri("/Images/Icons/users.png", UriKind.Relative));
        Settings_Icon.Source = new BitmapImage(new Uri("/Images/Icons/settings.png", UriKind.Relative));
        PasswordChange_Icon.Source = new BitmapImage(new Uri("/Images/Icons/rectangle-ellipsis.png", UriKind.Relative));
        LogOut_Icon.Source = new BitmapImage(new Uri("/Images/Icons/log-out.png", UriKind.Relative));
    }

    // private void MemberFunctions_SelectionChanged(object sender, SelectionChangedEventArgs e)
    // {
    //     // ComboBox comboBox = (ComboBox)sender;
    //     // ComboBoxItem selectedItem = (ComboBoxItem)comboBox.SelectedItem;
    //     // string selectedContent = (string)selectedItem.Content;
    //     // comboBox.SelectedIndex = 0;
    //     //
    //     // if (!AllowNavigation(selectedContent))
    //     // {
    //     //     return;
    //     // }
    //     //
    //     // switch (selectedContent)
    //     // {
    //     //     case "Uitloggen":
    //     //     {
    //     //         MainWindow.LoggedInMember = null;
    //     //         MainWindow.MainContent.Navigate(new Login(MainWindow));
    //     //         break;
    //     //     }
    //     //     case "Nieuw Lid":
    //     //     {
    //     //         MainWindow.MainContent.Navigate(new CreateUser(MainWindow));
    //     //         break;
    //     //     }
    //     //     case "Gegevens wijzigen":
    //     //     {
    //     //         MainWindow.MainContent.Navigate(new EditUser(MainWindow));
    //     //         break;
    //     //     }
    //     //     case "Wachtwoord wijzigen":
    //     //     {
    //     //         MainWindow.MainContent.Navigate(new ChangePassword(MainWindow));
    //     //         break;
    //     //     }
    //     //     case "Leden":
    //     //     {
    //     //         MainWindow.MainContent.Navigate(new ViewUsers(MainWindow));
    //     //         break;
    //     //     }
    //     //     default:
    //     //     {
    //     //         //todo send to main page
    //     //         break;
    //     //     }
    //     // }
    // }
    //
    // private bool AllowNavigation(string selectedContent)
    // {
    //     // Check if MainWindow or LoggedInMember is null
    //     if (MainWindow == null || MainWindow.LoggedInMember == null)
    //     {
    //         return false;
    //     }
    //
    //     string memberName = MainWindow.LoggedInMember.FirstName + " " + MainWindow.LoggedInMember.Infix + " " +
    //                         MainWindow.LoggedInMember.LastName;
    //
    //     if (selectedContent == "Uitgelogd" || selectedContent == memberName)
    //         return false;
    //
    //     return true;
    // }
    private void Button_OnClick(object sender, RoutedEventArgs e)
    {
        if (sender is Button clickedButton)
        {
            string buttonName = clickedButton.Name;

            switch (buttonName)
            {
                case "UserAdd_Button":
                    MainWindow.MainContent.Navigate(new CreateUser(MainWindow));
                    break;
                case "Users_Button":
                    MainWindow.MainContent.Navigate(new ViewUsers(MainWindow));
                    break;
                case "Settings_Button":
                    // Add navigation logic for Settings button
                    break;
                case "PasswordChange_Button":
                    MainWindow.MainContent.Navigate(new ChangePassword(MainWindow));
                    break;
                case "LogOut_Button":
                    MainWindow.LoggedInMember = null;
                    MainWindow.MainContent.Navigate(new Login(MainWindow));
                    break;
                default:
                    // Handle any other button clicks here
                    break;
            }
        }
    }
}