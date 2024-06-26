﻿using System.Windows;
using System.Windows.Controls;
using DataAccessLibrary;
using RoeiVerenigingLibrary;
using RoeiVerenigingLibrary.Exceptions;
using RoeiVerenigingWPF.Frames;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace RoeiVerenigingWPF.Pages
{
    public partial class AdminEditUser : Page
    {
        private readonly MainWindow _mainWindow;
        private readonly int _memberId;
        private Dictionary<string, CheckBox> _roleCheckBoxes = new Dictionary<string, CheckBox>();
        private readonly MemberService _service = new MemberService(new MemberRepository());

        public AdminEditUser(MainWindow mainWindow, int memberId)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
            _memberId = memberId;
            
            RoeiVerenigingLibrary.Member selectedMember = _service.GetById(memberId);
            FirstName.Text = selectedMember.FirstName;
            Infix.Text = selectedMember.Infix;
            LastName.Text = selectedMember.LastName;
            Email.Text = selectedMember.Email;
            Level.Text = selectedMember.Level.ToString();

            foreach (string role in selectedMember.Roles)
            {
                if (selectedMember.Roles.Contains("beheerder"))
                {
                    admin.IsChecked = true;
                }

                if (selectedMember.Roles.Contains("materiaal_commissaris"))
                {
                    material_comm.IsChecked = true;
                }

                if (selectedMember.Roles.Contains("evenementen_commissaris"))
                {
                    event_comm.IsChecked = true;
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
            if (ExceptionTextBlock.Foreground != Brushes.MediumSeaGreen)
            {
                ExceptionTextBlock.Foreground = Brushes.Red;
                try
                {
                    //get all data
                    string firstName = FirstName.Text;
                    string infix = Infix.Text;
                    string lastName = LastName.Text;
                    string email = Email.Text;
                    if (!Int32.TryParse(Level.Text, out int level))
                    {
                        ExceptionTextBlock.Text = "Het niveau moet een nummer zijn";
                        return;
                    }

                    var selectedRoles = new List<string>();

                    if (admin.IsChecked == true)
                    {
                        selectedRoles.Add("beheerder");
                    }

                    if (material_comm.IsChecked == true)
                    {
                        selectedRoles.Add("materiaal_commissaris");
                    }

                    if (event_comm.IsChecked == true)
                    {
                        selectedRoles.Add("evenementen_commissaris");
                    }


                    //run the update methods from the service
                    RoeiVerenigingLibrary.Member updatedMember = _service.Update(_mainWindow.LoggedInMember, _memberId,
                        firstName, infix,
                        lastName,
                        email, level
                    );
                    _service.SetRoles(_memberId, selectedRoles);
                    //check if updated
                    if (updatedMember != null)
                    {
                        ExceptionTextBlock.Text =
                            $"{updatedMember.FirstName} {updatedMember.Infix} {updatedMember.LastName} is gewijzigd";
                        ExceptionTextBlock.Foreground = Brushes.MediumSeaGreen;
                        ContinueButton.Content = "Verder";
                    }
                }
                catch (CantAccesDatabaseException ex)
                {
                    ExceptionTextBlock.Text = ex.Message;
                }
                catch (IncorrectRightsException ex)
                {
                    ExceptionTextBlock.Text = ex.Message;
                }
                catch (Exception ex)
                {
                    ExceptionTextBlock.Text = ex.Message;
                }
            }
            else
            {
                _mainWindow.MainContent.Navigate(new ViewUsers(_mainWindow)); 
            }
        }
    }
}