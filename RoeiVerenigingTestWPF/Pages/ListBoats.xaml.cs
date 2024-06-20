﻿using DataAccessLibrary;
using RoeiVerenigingLibrary;
using RoeiVerenigingTestWPF.Frames;
using System.Windows;
using System.Windows.Controls;
using RoeiVerenigingLibrary.Services;

namespace RoeiVerenigingTestWPF.Pages
{
    /// <summary>
    ///     Interaction logic for ListBoats.xaml
    /// </summary>
    public partial class ListBoats : Page
    {

        public ListBoats(MainWindow mw)
        {
            InitializeComponent();
            BoatService service = new BoatService(new BoatRepository());
            DataContext = this;
            MainWindow = mw;
            boats = service.GetBoats();
        }
        public List<Boat> boats { get; set; }
        public MainWindow MainWindow { set; get; }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button)
            {
                Button casted = sender as Button;
                object command = casted.CommandParameter;
                int id = Int32.Parse(command.ToString());

                MainWindow.MainContent.Navigate(new AddReservation(MainWindow.LoggedInMember, id));
            }
        }
    }
}