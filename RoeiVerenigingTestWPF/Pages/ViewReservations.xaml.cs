﻿using System.Windows;
using System.Windows.Controls;
using DataAccessLibrary;
using RoeiVerenigingLibrary.Model;
using RoeiVerenigingLibrary.Services;
using RoeiVerenigingTestWPF.Frames;

namespace RoeiVerenigingTestWPF.Pages;

/// <summary>
///     Interaction logic for ViewUsers.xaml
/// </summary>
public partial class ViewReservations : Page
{
    public ViewReservations(MainWindow mainWindow)
    {
        InitializeComponent();
        DataContext = this;
        var service = new ReservationService(new ReservationRepository());
        MainWindow = mainWindow;
        ReservationList = service.GetReservations(mainWindow.LoggedInMember);
    }

    public List<Reservation> ReservationList { get; set; }
    public MainWindow MainWindow { set; get; }

    public void Button_Click(object sender, RoutedEventArgs e)
    {
    }
}