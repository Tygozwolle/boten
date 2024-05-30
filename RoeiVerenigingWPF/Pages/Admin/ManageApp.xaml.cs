using System.Diagnostics;
using System.Security.Principal;
using System.Windows;
using System.Windows.Controls;
using DataAccessLibrary;
using RoeiVerenigingWPF.Frames;

namespace RoeiVerenigingWPF.Pages.Admin;

/// <summary>
///     Interaction logic for ManageApp.xaml
/// </summary>
public partial class ManageApp : Page
{
    private readonly MainWindow _MainWindow;

    public ManageApp(MainWindow mw)
    {
        _MainWindow = mw;
        _MainWindow.ManageApp.Content = "Afsluiten";
        _MainWindow.ManageApp.IsEnabled = true;
        _MainWindow.ManageApp.Visibility = Visibility.Visible;
        InitializeComponent();
        SetContent();
        CheckCorrectRights();
    }

    private void SetContent()
    {
        if (Config.DBAdress != null) DBAdress.Text = Config.DBAdress;

        if (Config.DBPort != null) DBPort.Text = Config.DBPort;

        if (Config.DBPassword != null) DBPassword.Password = Config.DBPassword;

        if (Config.DBUsername != null) DBUserName.Text = Config.DBUsername;

        if (Config.ControlUsername != null) Email.Text = Config.ControlUsername;

        if (Config.ControlPassword != null) Password.Password = Config.ControlPassword;
    }

    private void CheckCorrectRights()
    {
        bool isElevated;
        using (var identity = WindowsIdentity.GetCurrent())
        {
            var principal = new WindowsPrincipal(identity);
            isElevated = principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        if (!isElevated)
        {
            StartNewProcessAll();
            Application.Current.Shutdown();
        }
    }

    private void Change(object sender, RoutedEventArgs e)
    {
        var valid = TestConnection.TestString(DBUserName.Text, DBPassword.Password, DBAdress.Text, DBPort.Text,
            out var errorMassage);
        if (valid)
        {
            var messageBoxResult = MessageBox.Show(
                "Weet je zeker dat u de instellingen wilt wijzigen?", "Bevestiging",
                MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                Config.SetDBAdress(DBAdress.Text);
                Config.SetDBPort(DBPort.Text);
                Config.SetDBPassword(DBPassword.Password);
                Config.SetDBUsername(DBUserName.Text);
                Config.SetControlUsername(Email.Text);
                Config.SetControlPassword(Password.Password);
            }
        }
        else
        {
            var messageBoxResult = MessageBox.Show(
                $"Database instelling kloppen niet. \n {errorMassage} \n Wil u u account instellingen wijzigen?",
                "Database instellingen incorrect", MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                Config.SetControlUsername(Email.Text);
                Config.SetControlPassword(Password.Password);
            }
            else
            {
            }
        }
    }

    private static void StartNewProcessAll()
    {
        var startInfo = new ProcessStartInfo
        {
            FileName = Process.GetCurrentProcess().MainModule.FileName,
            Arguments = "UpdateAll",
            UseShellExecute = true,
            CreateNoWindow = false
        };
        startInfo.Verb = "runas";

        using (var process = new Process())
        {
            process.StartInfo = startInfo;
            process.Start();
        }

        Thread.Sleep(1000);
        Application.Current.Shutdown();
    }
}