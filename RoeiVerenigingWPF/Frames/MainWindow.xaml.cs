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
                this.___HeaderClass_.NameComboBoxItem.Content = value.FirstName + " " + value.LastName;
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            MainContent.Navigate(new Login(this));
            DataContext = this;
            ___ButtonClass_.MainWindow = this;
            ___HeaderClass_.MainWindow = this;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}