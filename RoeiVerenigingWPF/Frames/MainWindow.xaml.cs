using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Navigation;
using RoeiVerenigingLibary;
using RoeiVerenigingWPF.helpers;
using RoeiVerenigingWPF.Pages;

namespace RoeiVerenigingWPF.Frames
{
    public partial class MainWindow : INotifyPropertyChanged
    {
        private Member? _loggedInMember;

        public Member? LoggedInMember
        {
            get { return _loggedInMember; }
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

        public MainWindow()
        {
            InitializeComponent();
            SetupExceptionHandling();

            DataContext = this;
            ButtonClass.MainWindow = this;
            HeaderClass.MainWindow = this;
            LoginContent.Navigate(new Login(this));
            this.MainContent.ContentRendered += MainContent_ContentRendered;
        }

        private void MainContent_ContentRendered(object? sender, EventArgs e)
        {
            ClearHistory();
        }

        private void ClearHistory()
        {
            if (this.MainContent.NavigationService.CanGoBack)
            {
                var test = this.MainContent.NavigationService.Content;
                var nav = this.MainContent.NavigationService;
                var entry = this.MainContent.NavigationService.RemoveBackEntry();
                RemoveAllHandlers.RemoveAllhandlersFromOpject(entry);
                while (entry != null)
                {
                    entry = this.MainContent.RemoveBackEntry();
                    RemoveAllHandlers.RemoveAllhandlersFromOpject(entry);
                }

            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

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
    }
}