﻿using System.Drawing;
using DataAccessLibrary;
using RoeiVerenigingLibrary;
using RoeiVerenigingLibrary.Exceptions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using RoeiVerenigingWPF.helpers;
using FontFamily = System.Windows.Media.FontFamily;

namespace RoeiVerenigingWPF.Pages
{
    /// <summary>
    ///     Interaction logic for AddReservation.xaml
    /// </summary>
    public partial class AddReservation : Page
    {
        private readonly Member _loggedInMember;

        private readonly ReservationService _reservationService = new ReservationService(new ReservationRepository());
        private readonly BoatService _boatService = new BoatService(new BoatRepository());
        private List<Reservation> _reservationsList;

        private List<Button> _selectedButtons = new List<Button>();
        private List<DateTime> _selectedTimes = new List<DateTime>();
        public Boat Boat { get; set; }

        public AddReservation(Member loggedInMember, int boatId)
        {
            InitializeComponent();
            _loggedInMember = loggedInMember;
            Boat = _boatService.GetBoatById(boatId);
            _reservationsList = _reservationService.GetReservations();
            DataContext = this;
        }


        public List<DateTime> GetAvailableTimes(DateTime selectedDate)
        {
            List<DateTime> timeAvailableList = new List<DateTime>();
            DateTime startTime = selectedDate.AddHours(6); // 6 AM selected date
            DateTime endTime = selectedDate.AddHours(22); // 10 PM selected date

            for (DateTime time = startTime; time < endTime; time = time.AddHours(1))
            {
                bool isAvailable =
                    !_reservationsList.Any(res => time < res.EndTime && time.AddHours(1) > res.StartTime);
                if (isAvailable)
                {
                    timeAvailableList.Add(time);
                }
            }

            return timeAvailableList;
        }

        private void PopulateTimeContentGrid(List<DateTime> availableDates)
        {
            _selectedTimes.Clear();
            _selectedButtons.Clear();

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

                var borderStyle = new Style(typeof(Border));
                borderStyle.Setters.Add(new Setter(Border.CornerRadiusProperty, new CornerRadius(15)));

                timeButton.Resources.Add(typeof(Border), borderStyle);

                timeButton.Click += (sender, e) => TimeButton_Click(sender as Button, dateTime);

                // Calculate row and column
                int row = i / rowCount;
                int column = i % columnCount;

                // Set Grid.Row and Grid.Column attached properties
                Grid.SetRow(timeButton, row);
                Grid.SetColumn(timeButton, column);

                TimeContentGrid.Children.Add(timeButton);

                // Add new rows and columns if needed
                if (row >= TimeContentGrid.RowDefinitions.Count)
                {
                    TimeContentGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                }

                if (column >= TimeContentGrid.ColumnDefinitions.Count)
                {
                    TimeContentGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
                }
            }
        }


        private void DateIsSelected(object? sender, SelectionChangedEventArgs e)
        {
            var calendar = sender as Calendar;
            if (calendar.SelectedDate != null)
            {
                DateTime selectedDate = (DateTime)calendar.SelectedDate;

                PopulateTimeContentGrid(GetAvailableTimes(selectedDate));
                SelectedDateTextBlock.Text = selectedDate.ToString("dd MMMM yyyy");
                StartTimeTextBlock.Text = null;
                EndTimeTextBlock.Text = null;
            }
        }

        private void TimeButton_Click(Button clickedButton, DateTime dateTime)
        {
            if (_selectedTimes.Contains((dateTime)))
            {
                // Deselect the button
                _selectedButtons.Remove(clickedButton);
                _selectedTimes.Remove(dateTime);
                clickedButton.Background = CustomColors.TextBoxBackgroundColor;
            }
            else
            {
                if (_selectedTimes.Count < 2)
                {
                    _selectedButtons.Add(clickedButton);
                    _selectedTimes.Add(dateTime);
                    clickedButton.Background = CustomColors.ButtonBackgroundColor;

                    if (_selectedTimes.Count == 2)
                    {
                        // Check if the selected times are consecutive
                        _selectedTimes.Sort();
                        TimeSpan difference = _selectedTimes[1] - _selectedTimes[0];
                        if (difference != TimeSpan.FromHours(1))
                        {
                            // Deselect all buttons if not consecutive
                            foreach (var button in _selectedButtons)
                            {
                                button.Background = CustomColors.TextBoxBackgroundColor;
                            }

                            _selectedButtons.Clear();
                            _selectedTimes.Clear();
                            MessageBox.Show("Selected times must be consecutive.");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("You can only select up to two consecutive hours.");
                }

                if (_selectedTimes.Count == 1)
                {
                    StartTimeTextBlock.Text = _selectedTimes[0].ToString("t");
                    EndTimeTextBlock.Text = _selectedTimes[0].AddHours(1).ToString("t");
                }
                else
                {
                    StartTimeTextBlock.Text = _selectedTimes[0].ToString("t");
                    EndTimeTextBlock.Text = _selectedTimes[1].AddHours(1).ToString("t");
                }
            }
        }


        // private void TimePicker_TextChanged(object sender, TextChangedEventArgs e)
        // {
        //     var textBox = FocusManager.GetFocusedElement(this) as TextBox;
        //     if (textBox != null)
        //     {
        //     }
        // }
        //
        // public void ConfirmButton(Object sender, RoutedEventArgs e)
        // {
        //     //check if all fields are valid
        //     DateTime? startTime = BeginTimePicker.Value;
        //     DateTime? endTime = EndTimePicker.Value;
        //     DateTime? selectedDate = calendar.SelectedDate;
        //     if (!startTime.HasValue || !endTime.HasValue || !selectedDate.HasValue)
        //     {
        //         MessageBox.Show("Selecteer een datum en begin en eindtijd!");
        //         return;
        //     }
        //
        //     //add date and time together
        //     DateTime startDateTime = selectedDate.Value.Date.Add(startTime.Value.TimeOfDay);
        //     DateTime endDateTime = selectedDate.Value.Date.Add(endTime.Value.TimeOfDay);
        //
        //     try
        //     {
        //         if (_reservationService.TimeChecker(startTime, endTime))
        //         {
        //             _reservationService.Create(_loggedInMember, boat.Id, startDateTime, endDateTime);
        //         }
        //     }
        //     catch (InvalidTimeException invalidTimeException)
        //     {
        //         MessageBox.Show(invalidTimeException.Message);
        //         return;
        //     }
        //     catch (MaxAmountOfReservationExceeded maxAmountOfReservationExceededException)
        //     {
        //         MessageBox.Show(maxAmountOfReservationExceededException.Message);
        //         return;
        //     }
        //     catch (ReservationNotInDaylightException ex)
        //     {
        //         MessageBox.Show(ex.Message);
        //         return;
        //     }
        //     catch (Exception ex)
        //     {
        //         MessageBox.Show(ex.Message);
        //         return;
        //     }
        //
        //     MessageBox.Show("Reservering aangemaakt");
        //     //todo: send to reservation overview
        // }
    }
}