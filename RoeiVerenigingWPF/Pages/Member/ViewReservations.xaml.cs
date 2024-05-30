﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DataAccessLibrary;
using RoeiVerenigingLibrary.Model;
using RoeiVerenigingLibrary.Services;
using RoeiVerenigingWPF.Frames;

namespace RoeiVerenigingWPF.Pages.member;

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
        _MainWindow = mainWindow;
        ReservationList = service.GetReservations(mainWindow.LoggedInMember);
        BoatListFill(ReservationList);
    }

    public List<Reservation> ReservationList { get; set; }
    public List<Boat> BoatList { get; set; } = new();
    public BoatService BService { get; } = new(new BoatRepository());
    public MainWindow _MainWindow { set; get; }

    private void BoatListFill(List<Reservation> reservationList)
    {
        foreach (var reservation in reservationList) BoatList.Add(reservation.Boat);
    }

    private void ListView_Loaded(object sender, RoutedEventArgs e)
    {
        new Thread(async () =>
        {
            BService.GetImageBoats(BoatList);
            Dispatcher.Invoke((Action)(() => { ReservationListView.Items.Refresh(); }));
        }).Start();
    }

    private void Grid_Click(object sender, MouseButtonEventArgs e)
    {
        if (sender is Grid)
        {
            var casted = sender as Grid;
            var command = casted.Tag;
            var id = int.Parse(command.ToString());

            _MainWindow.MainContent.Navigate(new EditReservation(_MainWindow, id));
        }
    }
}