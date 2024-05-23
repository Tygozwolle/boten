using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DataAccessLibary;
using RoeiVerenigingWPF.Frames;

namespace RoeiVerenigingWPF.Pages.Admin
{
    /// <summary>
    /// Interaction logic for ManageApp.xaml
    /// </summary>
    public partial class ManageApp : Page
    {
        private MainWindow _MainWindow;

        public ManageApp(MainWindow mw)
        {
            _MainWindow = mw;
            _MainWindow.ManageApp.Content = "Terug";
            _MainWindow.ManageApp.IsEnabled = true;
            _MainWindow.ManageApp.Visibility = Visibility.Visible;
            InitializeComponent();
            SetContent();
        }

        private void SetContent()
        {
            if (Config.DBAdress != null)
            {
                DBAdress.Text = Config.DBAdress;
            }

            if (Config.DBPort != null)
            {
                DBPort.Text = Config.DBPort;
            }

            if (Config.DBPassword != null)
            {
                DBPassword.Password = Config.DBPassword;
            }

            if (Config.DBUsername != null)
            {
                DBUserName.Text = Config.DBUsername;
            }

            if (Config.ControlUsername != null)
            {
                Email.Text = Config.ControlUsername;
            }

            if (Config.ControlPassword != null)
            {
                Password.Password = Config.ControlPassword;
            }
        }
        private void Change(object sender, RoutedEventArgs e)
        {
            var valid = TestConection.TestString(DBUserName.Text, DBPassword.Password, DBAdress.Text, DBPort.Text);
            if (valid)
            {
                MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Weet je zeker dat u de instellingen wilt wijzigen?", "Bevestiging", System.Windows.MessageBoxButton.YesNo);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    Config.SetDBAdress(DBAdress.Text);
                    Config.SetDBPort(DBPort.Text);
                    Config.SetDBPassword(DBPassword.Password);
                    Config.SetDBUsername(DBUserName.Text);
                    Config.SetControlUsername(Email.Text);
                    Config.SetControlPassword(Password.Password);
                }
                else {return;}
            }
            else
            {
                MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Database instelling kloppen niet. \n Wil u u account instellingen wijzigen?", "Database instellingen incorrect", System.Windows.MessageBoxButton.YesNo);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    Config.SetControlUsername(Email.Text);
                    Config.SetControlPassword(Password.Password);
                }
                else
                {
                    return;
                }
            }

            _MainWindow.LoginContent.Navigate(new Login(_MainWindow));
        }
    }
}
