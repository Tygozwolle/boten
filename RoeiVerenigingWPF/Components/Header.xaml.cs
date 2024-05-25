using RoeiVerenigingWPF.Frames;
using RoeiVerenigingWPF.Pages;
using RoeiVerenigingWPF.Pages.Admin;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace RoeiVerenigingWPF.Components
{
    public partial class Header : UserControl
    {

        public Header()
        {
            InitializeComponent();
            DataContext = this;
            LoadButtonIcons();
        }
        public MainWindow MainWindow { set; get; }

        private void LoadButtonIcons()
        {
            UserAdd_Icon.Source = new BitmapImage(new Uri("/Images/Icons/user-plus.png", UriKind.Relative));
            Users_Icon.Source = new BitmapImage(new Uri("/Images/Icons/users.png", UriKind.Relative));
            Settings_Icon.Source = new BitmapImage(new Uri("/Images/Icons/settings.png", UriKind.Relative));
            PasswordChange_Icon.Source = new BitmapImage(new Uri("/Images/Icons/rectangle-ellipsis.png", UriKind.Relative));
            LogOut_Icon.Source = new BitmapImage(new Uri("/Images/Icons/log-out.png", UriKind.Relative));
        }

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
                        MainWindow.MainContent.Navigate(new EditUser(MainWindow));
                        break;
                    case "PasswordChange_Button":
                        MainWindow.MainContent.Navigate(new ChangePassword(MainWindow));
                        break;
                    case "Boat_Button":
                        MainWindow.MainContent.Navigate(new ManageBoatList(MainWindow));
                        break;
                    case "LogOut_Button":
                        MainWindow.LogOutMember();
                        MainWindow.LoginContent.Navigate(new Login(MainWindow));
                        break;
                }
            }
        }
    }
}