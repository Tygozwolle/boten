using DataAccessLibrary;
using RoeiVerenigingLibrary;
using RoeiVerenigingLibrary.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using RoeiVerenigingWPF.Frames;
using RoeiVerenigingWPF.helpers;

namespace RoeiVerenigingWPF.Pages
{
    public partial class EditReservation : Page
    {
        private readonly BoatService _boatService = new BoatService(new BoatRepository());
        private readonly ReservationService _reservationService = new ReservationService(new ReservationRepository());

        private List<Boat> _boatList;
        private List<Reservation> _reservationsList;
        private List<Button> _selectedButtons = new List<Button>();
        private List<DateTime> _selectedTimes = new List<DateTime>();

        public EditReservation(MainWindow mainWindow, int reservationId)
        {
            InitializeComponent();
            Main = mainWindow;
            ReservationId = reservationId;

            // Load reservation and boat details
            Reservation = _reservationService.GetReservation(reservationId);
            Boat = _boatService.GetBoatById(Reservation.BoatId);

            // Set DataContext for data binding
            DataContext = this;

            // Initialize start and end times
            StartingTime = Reservation.StartTime;
            EndTime = Reservation.EndTime;

            // Load boats and reservations
            LoadBoatsAndReservations();

            // Initialize UI elements
            SelectedDateTextBlock.Text = Reservation.StartTime.ToString("dd MMMM yyyy");
            StartTimeTextBlock.Text = Reservation.StartTime.ToString("t");
            EndTimeTextBlock.Text = Reservation.EndTime.ToString("t");
        }

        public MainWindow Main { get; set; }
        public RoeiVerenigingLibrary.Member Member { get; set; }
        public Reservation Reservation { get; set; }
        public Boat Boat { get; set; }
        private int ReservationId { get; }
        public DateTime StartingTime { get; set; }
        public DateTime EndTime { get; set; }

        private void LoadBoatsAndReservations()
        {
            _boatList = _boatService.GetBoats();
            _reservationsList = _reservationService.GetReservations();
        }

        private void DateIsSelected(object sender, SelectionChangedEventArgs e)
        {
            var calendar = sender as Calendar;
            ExceptionTextBlock.Text = "";
            ExceptionTextBlock.Foreground = Brushes.Red;
            TimeContentGrid.Children.Clear();
            
            try
            {
                if (calendar.SelectedDate != null)
                {
                    var selectedDate = (DateTime)calendar.SelectedDate;

                    if (selectedDate < DateTime.Today)
                    {
                        throw new ReservationInThePastException();
                    }
                    else if (selectedDate > DateTime.Today.AddDays(14) &&
                             (!Reservation.Member.Roles.Contains("beheerder") &&
                              !Reservation.Member.Roles.Contains("evenementen_commissaris")))
                    {
                        throw new DateTooFarInTheFuture();
                    }

                    PopulateTimeContentGrid(_reservationService.GetAvailableTimes(selectedDate, _boatList));

                    SelectedDateTextBlock.Text = selectedDate.ToString("dd MMMM yyyy");
                    StartTimeTextBlock.Text = null;
                    EndTimeTextBlock.Text = null;
                }
            }
            catch (Exception ex)
            {
                ExceptionTextBlock.Text = ex.Message;
                TimeContentGrid.Children.Clear();
            }
        }

        private void PopulateTimeContentGrid(List<DateTime> availableDates)
        {
            const int rowCount = 4;
            const int columnCount = 4;

            TimeContentGrid.Children.Clear();

            for (int i = 0; i < availableDates.Count; i++)
            {
                DateTime dateTime = availableDates[i];
                Button timeButton = new Button
                {
                    Content = dateTime.ToString("HH:mm"),
                    FontFamily = new FontFamily("Franklin Gothic Medium"),
                    FontSize = 24,
                    Foreground = CustomColors.HeaderColor,
                    Background = CustomColors.MainBackgroundColor,
                    BorderBrush = CustomColors.OutsideBorderColor,
                    Margin = new Thickness(10),
                    Height = 50,
                    Width = 100
                };

                timeButton.Click += (sender, e) => TimeButton_Click(sender as Button, dateTime);

                int row = i / rowCount;
                int column = i % columnCount;

                Grid.SetRow(timeButton, row);
                Grid.SetColumn(timeButton, column);

                TimeContentGrid.Children.Add(timeButton);
            }
        }

        private void TimeButton_Click(Button clickedButton, DateTime dateTime)
        {
            ExceptionTextBlock.Text = "";

            if (_selectedTimes.Contains(dateTime))
            {
                _selectedButtons.Remove(clickedButton);
                _selectedTimes.Remove(dateTime);
                clickedButton.Background = Brushes.Gray;
            }
            else
            {
                if (_selectedTimes.Count < 2)
                {
                    _selectedButtons.Add(clickedButton);
                    _selectedTimes.Add(dateTime);
                    clickedButton.Background = Brushes.LightBlue;

                    if (_selectedTimes.First() > _selectedTimes.Last())
                    {
                        var temp = _selectedTimes[0];
                        _selectedTimes[0] = _selectedTimes.Last();
                        _selectedTimes[_selectedTimes.Count - 1] = temp;
                    }

                    if (_selectedTimes.Count == 2)
                    {
                        _selectedTimes.Sort();
                        var difference = _selectedTimes[1] - _selectedTimes[0];
                        if (difference != TimeSpan.FromHours(1))
                        {
                            foreach (var button in _selectedButtons)
                            {
                                button.Background = Brushes.Gray;
                            }

                            _selectedButtons.Clear();
                            _selectedTimes.Clear();
                            ExceptionTextBlock.Text = "De tijdblokken moeten direct achter elkaar zijn!";
                        }
                    }
                }
                else
                {
                    ExceptionTextBlock.Text = "Je kan maar 2 uur achter elkaar selecteren!";
                }
            }

            if (_selectedTimes.Count == 1)
            {
                StartingTime = _selectedTimes[0];
                EndTime = _selectedTimes[0].AddHours(1);
                StartTimeTextBlock.Text = StartingTime.ToString("t");
                EndTimeTextBlock.Text = EndTime.ToString("t");
            }
            else if (_selectedTimes.Count == 2)
            {
                StartingTime = _selectedTimes[0];
                EndTime = _selectedTimes[1];
                StartTimeTextBlock.Text = StartingTime.ToString("t");
                EndTimeTextBlock.Text = EndTime.ToString("t");
            }
            else
            {
                StartTimeTextBlock.Text = null;
                EndTimeTextBlock.Text = null;
            }
        }

        public void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _reservationService.ChangeReservation(ReservationId, Reservation.Member, Reservation.BoatId, StartingTime, EndTime);
                MessageBox.Show("Reservering gewijzigd", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
                // Navigate back or close the window as needed
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Er is een fout opgetreden: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
