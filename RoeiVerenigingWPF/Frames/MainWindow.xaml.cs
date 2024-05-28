using RoeiVerenigingLibrary;
using RoeiVerenigingWPF.helpers;
using RoeiVerenigingWPF.Pages;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Navigation;
using DataAccessLibary;
using QRCoder;
using RoeiVerenigingWPF.Pages.Admin;
using System.Security.Principal;

namespace RoeiVerenigingWPF.Frames
{
    public partial class MainWindow : INotifyPropertyChanged
    {
        private Member? _loggedInMember;

        public MainWindow()
        {
            InitializeComponent();
            SetupExceptionHandling();

            DataContext = this;
            ButtonClass.MainWindow = this;
            HeaderClass.MainWindow = this;
            LoginContent.Navigate(new Login(this));
            SetManage();

            MainContent.ContentRendered += MainContent_ContentRendered;

        }

        public Member? LoggedInMember
        {
            get => _loggedInMember;
            set
            {
                if (_loggedInMember != value)
                {
                    _loggedInMember = value;
                    OnPropertyChanged();
                }

                if (_loggedInMember.Roles.Contains("beheerder"))
                {
                    HeaderClass.Users_Button.Visibility = Visibility.Visible;
                    HeaderClass.UserAdd_Button.Visibility = Visibility.Visible;
                    HeaderClass.Boat_Button.Visibility = Visibility.Visible;
                }
                if (_loggedInMember.Roles.Contains("materiaal_commissaris"))
                {
                    HeaderClass.Boat_Button.Visibility = Visibility.Visible;
                }

                if (_loggedInMember != null)
                {
                    HeaderClass.LoggedInMemberName.Content =
                        value.FirstName;
                    HeaderClass.Visibility = Visibility.Visible;
                    ButtonClass.Visibility = Visibility.Visible;
                }
                else
                {
                    HeaderClass.LoggedInMemberName.Content = "Uitgelogd";
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void MainContent_ContentRendered(object? sender, EventArgs e)
        {
            ClearHistory();
        }

        private void ClearHistory()
        {
            if (MainContent.NavigationService.CanGoBack)
            {
                JournalEntry? entry = MainContent.NavigationService.RemoveBackEntry();
                RemoveAllHandlers.RemoveAllhandlersFromOpject(entry);
                while (entry != null)
                {
                    entry = MainContent.RemoveBackEntry();
                    RemoveAllHandlers.RemoveAllhandlersFromOpject(entry);
                }

            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void SetupExceptionHandling()
        {
            AppDomain.CurrentDomain.UnhandledException += LogUnhandledException;
        }

        private void LogUnhandledException(object sender, UnhandledExceptionEventArgs args)
        {
            try
            {
                Exception e = (Exception)args.ExceptionObject;
                MessageBox.Show(e.Message);
            }
            catch
            {
            }
        }

        public void LogOutMember()
        {
            HeaderClass.Visibility = Visibility.Hidden;
            ButtonClass.Visibility = Visibility.Hidden;
            MainContent.Visibility = Visibility.Hidden;
            LoginContent.Visibility = Visibility.Visible;
            _loggedInMember = null;
        }

        private void ManageApp_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Login login = (Login)LoginContent.Content;
                if (Config.ControlPassword == null || Config.ControlUsername == null || Config.ControlUsername == "" ||
                    Config.ControlPassword == "")
                {
                    LoginContent.Navigate(new ManageApp(this));
                    //ManageApp.IsEnabled = false;
                    //ManageApp.Visibility = Visibility.Hidden;
                    return;
                }

                var email = login.Email.Text;
                var password = login.Password.Password;
                if (password == Config.ControlPassword && email == Config.ControlUsername)
                {
                    LoginContent.Navigate(new ManageApp(this));
                    //ManageApp.IsEnabled = false;
                    //ManageApp.Visibility = Visibility.Hidden;
                }
            }
            catch
            {
                Application.Current.Shutdown();
            }

        }

        private void SetManage()
        {
            bool isElevated;
            using (WindowsIdentity identity = WindowsIdentity.GetCurrent())
            {
                WindowsPrincipal principal = new WindowsPrincipal(identity);
                isElevated = principal.IsInRole(WindowsBuiltInRole.Administrator);
            }

            if (isElevated)
            {
                LoginContent.Navigate(new ManageApp(this));
            }

        }
    }
}