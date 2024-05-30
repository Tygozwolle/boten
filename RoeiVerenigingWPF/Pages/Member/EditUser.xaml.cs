﻿#region

using System.Windows;
using System.Windows.Controls;
using DataAccessLibrary;
using RoeiVerenigingLibrary.Exceptions;
using RoeiVerenigingLibrary.Services;
using RoeiVerenigingWPF.Frames;

#endregion

namespace RoeiVerenigingWPF.Pages.member;

public partial class EditUser : Page
{
    private readonly MainWindow _mainWindow;

    public EditUser(MainWindow mainWindow)
    {
        InitializeComponent();
        _mainWindow = mainWindow;
        FirstName.Text = mainWindow.LoggedInMember.FirstName;
        Infix.Text = mainWindow.LoggedInMember.Infix;
        LastName.Text = mainWindow.LoggedInMember.LastName;
        Email.Text = mainWindow.LoggedInMember.Email;
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        MemberService service = new MemberService(new MemberRepository());
        try
        {
            string firstName = FirstName.Text;
            string infix = Infix.Text;
            string lastName = LastName.Text;
            string email = Email.Text;
            RoeiVerenigingLibrary.Model.Member updatedMember = service.Update(_mainWindow.LoggedInMember, firstName, infix, lastName, email
            );
            if (updatedMember != null)
            {
                _mainWindow.LoggedInMember = updatedMember;
                MessageBox.Show(
                    $"{updatedMember.FirstName} {updatedMember.Infix} {updatedMember.LastName} is gewijzigd");
            }
        }
        catch (CantAccesDatabaseException ex)
        {
            MessageBox.Show(ex.Message);
        }
        catch (IncorrectRightsException ex)
        {
            MessageBox.Show(ex.Message);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }
}