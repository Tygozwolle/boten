﻿using System.Windows;
using System.Windows.Controls;
using DataAccessLibary;
using RoeiVerenigingLibary;
using RoeiVerenigingTestWPF.Frames;

namespace RoeiVerenigingTestWPF.Pages;

public partial class ChangePassword : Page
{
    private MainWindow _mainWindow;

    public ChangePassword(MainWindow mainWindow)
    {
        InitializeComponent();
        _mainWindow = mainWindow;
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        MemberService service = new MemberService(new MemberRepository());
        try
        {
            string password = CurrentPassword.Password;
            string newPassword = NewPassword.Password;
            string newPasswordConfirm = NewPasswordConfirm.Password;
            service.ChangePassword(_mainWindow.LoggedInMember, password, newPassword, newPasswordConfirm);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
            return;
        }

        MessageBox.Show("Wachtwoord is veranderd");
    }
}