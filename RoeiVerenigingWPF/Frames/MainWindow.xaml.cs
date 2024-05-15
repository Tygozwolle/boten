using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using RoeiVerenigingLibary;
using RoeiVerenigingWPF.Pages;

namespace RoeiVerenigingWPF.Frames
{
    public partial class MainWindow : Window, INotifyPropertyChanged
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
                    this.HeaderClass.LoggedInMemberName.Content =
                        value.FirstName;
                }
                else
                {
                    HeaderClass.LoggedInMemberName.Content = "Uitgelogd";
                    HeaderClass.Users_Button.Visibility = Visibility.Hidden;
                    HeaderClass.UserAdd_Button.Visibility = Visibility.Hidden;
                }
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            SetupExceptionHandling();
            MainContent.Navigate(new Login(this));
            DataContext = this;
            ButtonClass.MainWindow = this;
            HeaderClass.MainWindow = this;
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
    }
}