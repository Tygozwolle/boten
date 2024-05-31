using DataAccessLibrary;
using RoeiVerenigingLibrary;
using RoeiVerenigingLibrary.Exceptions;
using RoeiVerenigingLibrary.Model;
using RoeiVerenigingLibrary.Services;
using RoeiVerenigingWPF.Frames;
using RoeiVerenigingWPF.helpers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace RoeiVerenigingWPF.Pages.EventCommissioner
{
    public partial class ManageEvent : Page
    {

        private readonly Member _loggedInMember;
        private MainWindow _mainWindow;
        private readonly BoatService _boatService = new BoatService(new BoatRepository());
        private readonly EventService _eventService = new EventService(new EventRepository());
        private List<Boat> _boatList;
        private List<Button> _selectedButtons = new List<Button>();
        private List<DateTime> _selectedTimes = new List<DateTime>();
        private List<DateTime> _availableTimes = new List<DateTime>();
        private List<Boat> _selectedBoats = new List<Boat>();
        private DateTime StartTime { get; set; }
        private DateTime EndTime { get; set; }
        private DateTime _selectedDate;
        private Boat _selectedBoat;
        private Event _event;
        private bool _isEdit;
        private Dictionary<string, Button> _timeButtonDictionary;
        public ManageEvent(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
            InitializeComponent();
            _loggedInMember = mainWindow.LoggedInMember;
            
            DataContext = this;
            BoatGrid.Visibility = Visibility.Hidden;
        }
        public ManageEvent(MainWindow mainWindow, Event events)
        {
            _mainWindow = mainWindow;
            InitializeComponent();
            _loggedInMember = _mainWindow.LoggedInMember;
            _isEdit = true;
            DataContext = this;
            BoatGrid.Visibility = Visibility.Hidden;
            _event = events;
            SetEdit();
        }
        public ManageEvent(MainWindow mainWindow, int eventID) : this(mainWindow, new EventService(new EventRepository()).GetEventById(eventID))
        {
            
        }
        private void SetEdit()
        {
            _selectedDate = _event.StartDate.Date;
            // _selectedTimes.Add(_event.StartDate);
            //_selectedTimes.Add(_event.EndDate);
            _selectedBoats = _event.Boats;
            Description.Text = _event.Description;
            Name.Text = _event.Name;
            MaxPartisipants.Text = _event.MaxParticipants.ToString();
            _availableTimes = _eventService.GetAvailableTimes(_selectedDate, _event);
            PopulateTimeContentGrid(_availableTimes);
            _timeButtonDictionary.TryGetValue(_event.StartDate.ToString("HH:mm"), out Button button);
            TimeButton_Click(button, _event.StartDate);
            if (_event.EndDate.AddHours(-1) != _event.StartDate && _event.EndDate.AddMinutes(-59) != _event.StartDate)
            {
                Button button2 = null;
                if (_event.EndDate.Minute == 0)
                {
                    _timeButtonDictionary.TryGetValue(_event.EndDate.AddHours(-1).ToString("HH:mm"), out button2);
                }
                else
                {
                    _timeButtonDictionary.TryGetValue(_event.EndDate.AddMinutes(-59).ToString("HH:mm"), out button2);
                }
                if (button2 != null)
                {
                    TimeButton_Click(button2, _event.EndDate);
                }
            }
            
        }

        private void PopulateTimeContentGrid(List<DateTime> availableDates)
        {
            _timeButtonDictionary = new Dictionary<string, Button>();
            
            BoatGrid.Visibility = Visibility.Hidden;
            TimeBlockGrid.Visibility = Visibility.Visible;

            NextButton.Visibility = Visibility.Visible;
            SaveButton.Visibility = Visibility.Hidden;

            _selectedBoat = null;

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
                _timeButtonDictionary.Add(dateTime.ToString("HH:mm"), timeButton);
            }
        }
        private void DateIsSelected(object? sender, SelectionChangedEventArgs e)
        {
            var calendar = sender as Calendar;
            ClearExceptionText();
            TimeContentGrid.Children.Clear();

            try
            {
                if (calendar.SelectedDate != null)
                {
                    _selectedDate = (DateTime)calendar.SelectedDate;

                  
                     if (_selectedDate < DateTime.Today.AddDays(14))
                    {
                        SetExceptionText("Selecteer een datum minimaal 14 dagen in de toekomst!");
                        return;
                    }
                    _availableTimes = _eventService.GetAvailableTimes(_selectedDate);
                    PopulateTimeContentGrid(_availableTimes);

                    SelectedDateTextBlock.Text = _selectedDate.ToString("dd MMMM yyyy");
                    StartTimeTextBlock.Text = null;
                    EndTimeTextBlock.Text = null;
                }
            }
            catch (Exception ex)
            {
                ExceptionText.Text = ex.Message;
                TimeContentGrid.Children.Clear();
            }
        }
        private void ClearExceptionText()
        {
            ExceptionText.Text = "";
            ExceptionText.Visibility = Visibility.Collapsed;
            ExceptionText.Foreground = Brushes.Red;
            PageTitle.SetValue(Grid.ColumnSpanProperty, 2);
        }
        private void SetExceptionText(string message)
        {
            ExceptionText.Text = message;
            ExceptionText.Visibility = Visibility.Visible;
            ExceptionText.Foreground = Brushes.Red;
            PageTitle.SetValue(Grid.ColumnSpanProperty, 1);
        }
        private void TimeButton_Click(Button clickedButton, DateTime dateTime)
        {
            ClearExceptionText();
            
            if (_selectedTimes.Contains((dateTime)))
            {
                // Deselect the button
                _selectedButtons.Remove(clickedButton);
                _selectedTimes.Remove(dateTime);
                clickedButton.Background = CustomColors.MainBackgroundColor;
            }
            else
            {
                if (_selectedTimes.Count < 2)
                {
                    _selectedButtons.Add(clickedButton);
                    _selectedTimes.Add(dateTime);
                    clickedButton.Background = CustomColors.ButtonBackgroundColor;

                    if (_selectedTimes.First() > _selectedTimes.Last())
                    {
                        DateTime temp = _selectedTimes[0];
                        _selectedTimes[0] = _selectedTimes.Last();
                        _selectedTimes[_selectedTimes.Count - 1] = temp;
                    }

                    if (_selectedTimes.Count == 2)
                    {
                        _selectedTimes.Sort();
                    }
                }
                else
                {
                    SetExceptionText( "Selecteer het eerste en laatste tijdblok!");
                }
            }

            if (_selectedTimes.Count == 1)
            {
                StartTime = _selectedTimes[0];
                if (_selectedTimes[0].AddHours(1).Hour != 0)
                {
                    EndTime = _selectedTimes[0].AddHours(1);
                }
                else
                {
                    EndTime = _selectedTimes[0].AddMinutes(59).AddSeconds(59);
                }
                StartTimeTextBlock.Text = StartTime.ToString("t");
                EndTimeTextBlock.Text = EndTime.ToString("t");
            }
            else if (_selectedTimes.Count != 0)
            {
                if (CheckIfPosible())
                {
                    StartTime = _selectedTimes[0];
                    if (_selectedTimes[1].AddHours(1).Hour != 0)
                    {
                        EndTime = _selectedTimes[1].AddHours(1);
                    }
                    else
                    {
                        EndTime = _selectedTimes[1].AddMinutes(59).AddSeconds(59);
                    }
                    StartTimeTextBlock.Text = StartTime.ToString("t");
                    EndTimeTextBlock.Text = EndTime.ToString("t");
                }
                else
                {
                    SetExceptionText("Deze tijden zijn niet beschikbaar!");
                }
            }
            else
            {
                StartTimeTextBlock.Text = "";
                EndTimeTextBlock.Text = "";
            }
        }
        private bool CheckIfPosible()
        {
            if (_selectedTimes.Count < 2)
            {
                return true;
            }
            else if (_selectedTimes.Count == 2)
            {
                DateTime startTime = _selectedTimes[0];
                DateTime endTime = _selectedTimes[1];
                var range = Enumerable.Range(startTime.Hour, endTime.Hour);
                foreach (var start in range)
                {
                    if (!(_availableTimes.Contains(startTime.AddHours(start))))
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }
        private void NextButton_OnClick(object sender, RoutedEventArgs e)
        {
            PopulateBoatGrid(_selectedDate, StartTime, EndTime);

            TimeBlockGrid.Visibility = Visibility.Hidden;
            BoatGrid.Visibility = Visibility.Visible;

            NextButton.Visibility = Visibility.Hidden;
            SaveButton.Visibility = Visibility.Visible;
        }

        private void PopulateBoatGrid(DateTime selectedDate, DateTime startTime, DateTime endTime)
        {
            List<Boat> availableBoatList = _boatList;

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

                var imageconverter = new SingleStreamImageConverter(); 
                var source = imageconverter.Convert(boat.Image, typeof(ImageSource), null, null);
                                

                grid.Children.Add(new Image
                {
                    Source = (BitmapImage)source,
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
                    if (button.Background != CustomColors.ButtonBackgroundColor)
                    {
                        button.Background = CustomColors.ButtonBackgroundColor;
                        int boatId = (int)((Button)sender).Tag;
                        Boat selectedBoat = _boatList.FirstOrDefault(b => b.Id == boatId);

                        if (selectedBoat != null)
                        {
                            _selectedBoat = selectedBoat;
                            _selectedBoats.Add(_selectedBoat);
                        }
                    }
                    else
                    {
                        button.Background = CustomColors.MainBackgroundColor;
                        int boatId = (int)((Button)sender).Tag;
                        Boat selectedBoat = _boatList.FirstOrDefault(b => b.Id == boatId);

                        if (selectedBoat != null)
                        {
                            _selectedBoat = selectedBoat;
                            _selectedBoats.Remove(_selectedBoat);
                        }
                    }
                };

                BoatContentStackPanel.Children.Add(button);
            }
            ScrollViewerBoat.UpdateLayout();
        }

        private void SaveButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (_selectedBoats.Count >= 1)
            {
                try
                {
                    _eventService.CreateEvent(StartTime, EndTime, Description.Text, Name.Text, Int32.Parse(MaxPartisipants.Text), _selectedBoats, _loggedInMember);
                    ExceptionText.Text = "Het Evenement is aangemaakt!";
                    ExceptionText.Foreground = Brushes.Lime;
                    ExceptionText.Visibility = Visibility.Visible;
                    PageTitle.SetValue(Grid.ColumnSpanProperty, 1);
                }
                catch (Exception exception)
                {
                    SetExceptionText(exception.Message);
                }
            }
        }
        private void FrameworkElement_OnLoaded(object sender, RoutedEventArgs e)
        {
            new Task(() =>
            {
                _boatList = _boatService.GetBoats();
                _boatService.GetImageBoats(_boatList);
                this.Dispatcher.Invoke(() =>
                {
                    NextButton.IsEnabled = true;
                });
            }).Start();
        }
    }
}