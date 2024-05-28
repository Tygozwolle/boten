using System.Drawing;
using System.Runtime.InteropServices.JavaScript;
using DataAccessLibrary;
using RoeiVerenigingLibrary;
using RoeiVerenigingLibrary.Exceptions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Innovative.SolarCalculator;
using RoeiVerenigingWPF.helpers;
using FontFamily = System.Windows.Media.FontFamily;
using Image = System.Windows.Controls.Image;

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

        private DateTime StartTime { get; set; }
        private DateTime EndTime { get; set; }
        private DateTime _selectedDate;
        private Boat _selectedBoat;


        public AddReservation(Member loggedInMember)
        {
            InitializeComponent();
            _loggedInMember = loggedInMember;
            _boatList = _boatService.Getboats();
            _reservationsList = _reservationService.GetReservations();
            DataContext = this;
            CheckIfMemberHas2Reservations();
        }

        private void CheckIfMemberHas2Reservations()
        {
            if (_loggedInMember.Roles.Contains("beheerder"))
            {
                WatchOutTextBlock.Visibility = Visibility.Hidden;
            }
            else if (_reservationService.GetReservations(_loggedInMember).Count >= 2)
            {
                WatchOutTextBlock.Visibility = Visibility.Visible;
            }
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

                if (_selectedDate < DateTime.Today)
                {
                    throw new ReservationNotInDaylightException(); // todo betere exception
                }

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
                        StartTime = _selectedTimes[0];
                        EndTime = _selectedTimes[0].AddHours(1);
                    }
                    else if (_selectedTimes.Count != 0)
                    {
                        StartTimeTextBlock.Text = _selectedTimes[0].ToString("t");
                        EndTimeTextBlock.Text = _selectedTimes[1].AddHours(1).ToString("t");
                        StartTime = _selectedTimes[0];
                        EndTime = _selectedTimes[1].AddHours(1);
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

            NextButton.Visibility = Visibility.Hidden;
            SaveButton.Visibility = Visibility.Visible;

            PopulateBoatGrid(_selectedDate, StartTime, EndTime);
        }

        private void PopulateBoatGrid(DateTime selectedDate, DateTime startTime, DateTime endTime)
        {
            List<Boat> availableBoatList = _boatList.Where(boat => !_reservationsList
                    .Any(reservation => reservation.BoatId == boat.Id &&
                                        reservation.StartTime.Date == selectedDate.Date &&
                                        reservation.StartTime < endTime &&
                                        reservation.EndTime > startTime))
                .ToList();

            // List<Boat> availableBoatList = new List<Boat>() { _boatService.GetBoatById(2) };

            BoatContentStackPanel.Children.Clear();

            foreach (var boat in availableBoatList)
            {
                Grid grid = new Grid();
                grid.MaxHeight = 200;

                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(40) });
                grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(300) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(300) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(200) });

                grid.Children.Add(new Image
                {
                    Source = new BitmapImage(new Uri("/Images/Image_not_available.png", UriKind.Relative)),
                    Height = 150, Width = 300, VerticalAlignment = VerticalAlignment.Top
                });
                Grid.SetColumn(grid.Children[0], 0);
                Grid.SetRowSpan(grid.Children[0], 2);
                Grid.SetRow(grid.Children[0], 0);

                grid.Children.Add(new TextBlock
                {
                    Text = boat.Name, VerticalAlignment = VerticalAlignment.Center, FontSize = 32,
                    Foreground = CustomColors.HeaderColor
                });
                Grid.SetColumn(grid.Children[1], 1);
                Grid.SetRow(grid.Children[1], 0);

                grid.Children.Add(new TextBlock
                {
                    Text = boat.Description, VerticalAlignment = VerticalAlignment.Center, FontSize = 16,
                    TextWrapping = TextWrapping.Wrap, Width = 290,
                    Foreground = CustomColors.SubHeaderColor
                });
                Grid.SetColumn(grid.Children[2], 1);
                Grid.SetRow(grid.Children[2], 1);

                grid.Children.Add(new TextBlock
                {
                    Text = $"Aantal man: {boat.Seats}", VerticalAlignment = VerticalAlignment.Bottom, FontSize = 24,
                    Foreground = CustomColors.HeaderColor, HorizontalAlignment = HorizontalAlignment.Right
                });
                Grid.SetColumn(grid.Children[3], 2);
                Grid.SetRow(grid.Children[3], 0);

                grid.Children.Add(new TextBlock
                {
                    Text = $"Stuurman? {boat.CaptainSeatToString()}", VerticalAlignment = VerticalAlignment.Top,
                    FontSize = 24,
                    Foreground = CustomColors.HeaderColor, HorizontalAlignment = HorizontalAlignment.Right
                });
                Grid.SetColumn(grid.Children[4], 2);
                Grid.SetRow(grid.Children[4], 1);


                Button button = new Button
                {
                    BorderThickness = new Thickness(5),
                    BorderBrush = CustomColors.OutsideBorderColor,
                    Padding = new Thickness(10),
                    Margin = new Thickness(10),
                    Background = CustomColors.MainBackgroundColor,
                    Content = grid,
                    Resources = new ResourceDictionary()
                };

                button.Tag = boat.Id;

                Style borderStyle = new Style(typeof(Border));
                borderStyle.Setters.Add(new Setter(Border.CornerRadiusProperty, new CornerRadius(15)));

                button.Resources.Add(typeof(Border), borderStyle);

                button.Click += (sender, e) =>
                {
                    button.Background = CustomColors.ButtonBackgroundColor;
                    int boatId = (int)((Button)sender).Tag;

                    Boat selectedBoat = _boatList.FirstOrDefault(b => b.Id == boatId);
                    if (selectedBoat != null)
                    {
                        _selectedBoat = selectedBoat;
                    }
                };

                BoatContentStackPanel.Children.Add(button);
            }
        }

        private void SaveButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (_reservationService.GetReservations(_loggedInMember).Count >= 2)
            {
                // todo: show message
            }
            else
            {
                _reservationService.Create(_loggedInMember, _selectedBoat.Id, StartTime, EndTime);
            }
        }
    }
}