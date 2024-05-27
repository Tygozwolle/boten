using System.Drawing;
using System.Runtime.InteropServices.JavaScript;
using DataAccessLibrary;
using RoeiVerenigingLibrary;
using RoeiVerenigingLibrary.Exceptions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using Innovative.SolarCalculator;
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
        private List<Boat> _boatList;
        private List<Button> _selectedButtons = new List<Button>();
        private List<DateTime> _selectedTimes = new List<DateTime>();
        private DateTime _selectedDate;
        

        public AddReservation(Member loggedInMember, int boatId)
        {
            InitializeComponent();
            _loggedInMember = loggedInMember;
            _boatList = _boatService.Getboats();
            _reservationsList = _reservationService.GetReservations();
            DataContext = this;
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
                _selectedDate = (DateTime)calendar.SelectedDate;

                PopulateTimeContentGrid(_reservationService.GetAvailableTimes(_selectedDate, _reservationsList));
                SelectedDateTextBlock.Text = _selectedDate.ToString("dd MMMM yyyy");
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

                    if (_selectedTimes.Count == 1)
                    {
                        StartTimeTextBlock.Text = _selectedTimes[0].ToString("t");
                        EndTimeTextBlock.Text = _selectedTimes[0].AddHours(1).ToString("t");
                    }
                    else if (_selectedTimes.Count != 0)
                    {
                        StartTimeTextBlock.Text = _selectedTimes[0].ToString("t");
                        EndTimeTextBlock.Text = _selectedTimes[1].AddHours(1).ToString("t");
                    }
                }
                else
                {
                    MessageBox.Show("You can only select up to two consecutive hours.");
                }
            }
        }

        private void NextButton_OnClick(object sender, RoutedEventArgs e)
        {
            TimeBlockGrid.Visibility = Visibility.Hidden;
            BoatGrid.Visibility = Visibility.Visible;

            PopulateBoatGrid(_selectedDate, _selectedTimes[0], _selectedTimes[1]);
        }

        private void PopulateBoatGrid(DateTime selectedDate, DateTime startTime, DateTime endTime)
        {
            List<Boat> availableBoatList = new List<Boat>();
            availableBoatList = _boatList.Where(boat => !_reservationsList
                    .Any(reservation => reservation.BoatId == boat.Id &&
                                        reservation.StartTime.Date == selectedDate.Date &&
                                        reservation.StartTime < endTime &&
                                        reservation.EndTime > startTime))
                .ToList();

            foreach (var boat in availableBoatList)
            {
                BoatContentGrid.Children.Add(new TextBlock(){Text = boat.Id.ToString()});
                // todo: add availableBoatList in cards to the grid
            }
        }
    }
}