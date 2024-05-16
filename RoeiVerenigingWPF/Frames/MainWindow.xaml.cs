using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using RoeiVerenigingLibary;
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