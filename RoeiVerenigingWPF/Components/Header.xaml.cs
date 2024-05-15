using System.Windows;
using RoeiVerenigingWPF.Frames;
using System.Windows.Controls;
using System.Windows.Data;
using RoeiVerenigingWPF.Pages;

namespace RoeiVerenigingWPF.Components;

public partial class Header : UserControl
{
    public MainWindow MainWindow { set; get; }

    public Header()
    {
        InitializeComponent();
        DataContext = this;
    }

    private void MemberFunctions_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        ComboBox comboBox = (ComboBox)sender;
        ComboBoxItem selectedItem = (ComboBoxItem)comboBox.SelectedItem;
        string selectedContent = (string)selectedItem.Content;
        comboBox.SelectedIndex = 0;

        if (!AllowNavigation(selectedContent))
        {
            return;
        }

        switch (selectedContent)
        {
            case "Uitloggen":
            {
                MainWindow.LoggedInMember = null;
                MainWindow.MainContent.Navigate(new Login(MainWindow));
                break;
            }
            case "Nieuw Lid":
            {
                MainWindow.MainContent.Navigate(new CreateUser(MainWindow));
                break;
            }
            case "Gegevens wijzigen":
            {
                MainWindow.MainContent.Navigate(new EditUser(MainWindow));
                break;
            }
            case "Wachtwoord wijzigen":
            {
                MainWindow.MainContent.Navigate(new ChangePassword(MainWindow));
                break;
            }
            case "Leden":
            {
                MainWindow.MainContent.Navigate(new ViewUsers(MainWindow));
                break;
            }
            default:
            {
                //todo send to main page
                break;
            }
        }
    }

    private bool AllowNavigation(string selectedContent)
    {
        // Check if MainWindow or LoggedInMember is null
        if (MainWindow == null || MainWindow.LoggedInMember == null)
        {
            return false;
        }

        string memberName = MainWindow.LoggedInMember.FirstName + " " + MainWindow.LoggedInMember.Infix + " " +
                            MainWindow.LoggedInMember.LastName;

        if (selectedContent == "Uitgelogd" || selectedContent == memberName)
            return false;

        return true;
    }
}