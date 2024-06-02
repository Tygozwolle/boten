using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using DataAccessLibrary;
using RoeiVerenigingLibrary;
using RoeiVerenigingLibrary.Model;
using RoeiVerenigingWPF.Frames;
using Xceed.Wpf.Toolkit;

namespace RoeiVerenigingWPF.Pages.Member;

public partial class EventResult : Page
{
    private MainWindow _MainWindow;
    public Event EventResults { get; set; }
    private IEventResultRepository _EventReportsRepository = new EventResultRepository();

    private readonly SolidColorBrush _textColor = new SolidColorBrush(Color.FromArgb(255, 4, 48, 73));
    private readonly SolidColorBrush _borderColor = new SolidColorBrush(Color.FromArgb(255, 19, 114, 160));

    private readonly SolidColorBrush
        _evenRowColor = new SolidColorBrush(Color.FromArgb(255, 232, 246, 252));

    private readonly SolidColorBrush
        _oddRowColor = new SolidColorBrush(Color.FromArgb(255, 182, 227, 251));

    public String Date
    {
        get { return EventResults.StartDate.Date.ToString("dd MMMM, yyyy"); }
    }

    public String StartTime
    {
        get { return EventResults.StartDate.ToString("HH:mm"); }
    }

    public String EndTime
    {
        get { return EventResults.EndDate.ToString("HH:mm"); }
    }

    public String ParticipantsCount
    {
        get { return EventResults.Participants.Count.ToString() + "/" + EventResults.MaxParticipants.ToString(); }
    }

    public EventResult(MainWindow mw, Event _event)
    {
        DataContext = this;
        _MainWindow = mw;
        EventResults = _event;
        EventResults.AddParticipantsFromDatabase(_EventReportsRepository);
        InitializeComponent();
        PopulateResultView();
    }

    public void PopulateResultView()
    {
        bool enableEdit = (_MainWindow.LoggedInMember.Roles.Contains("materiaal_commissaris") ||
                           _MainWindow.LoggedInMember.Roles.Contains("beheerder"));
        ReportView.Children.Clear();
        for (int i = 0; i < EventResults.Participants.Count; i++)
        {
            EventParticipant member = EventResults.Participants[i];
            Grid grid = new Grid();
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(10) });

            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(100) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(100) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(150) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(100) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(200) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(290) });

            grid.Children.Add(new TextBlock
            {
                Text = (i + 1).ToString(), VerticalAlignment = VerticalAlignment.Center, FontSize = 16,
                Foreground = _textColor
            });
            Grid.SetColumn(grid.Children[0], 0);

            if (enableEdit)
            {
                TimePicker resultTimePicker = new TimePicker
                {
                    Value = new DateTime().Date + member.ResultTime,
                    Format = DateTimeFormat.Custom,
                    FormatString = "HH:mm:ss",
                    ShowDropDownButton = false,
                    Width = 80,
                    HorizontalAlignment = HorizontalAlignment.Left
                };

                grid.Children.Add(resultTimePicker);

                resultTimePicker.DataContext = member;

                Binding binding = new Binding("ResultTime");
                binding.Mode = BindingMode.TwoWay;
                binding.Converter = new TimeSpanToDateTimeConverter();
                resultTimePicker.SetBinding(TimePicker.ValueProperty, binding);
            }
            else
            {
                grid.Children.Add(new TextBlock
                {
                    Text = member.ResultTime.ToString(), VerticalAlignment = VerticalAlignment.Center, FontSize = 16,
                    Foreground = _textColor
                });
            }

            Grid.SetColumn(grid.Children[1], 1);


            grid.Children.Add(new TextBlock
            {
                Text = member.FirstName, VerticalAlignment = VerticalAlignment.Center, FontSize = 16,
                Foreground = _textColor
            });
            Grid.SetColumn(grid.Children[2], 2);

            grid.Children.Add(
                new TextBlock
                {
                    Text = member.Infix, VerticalAlignment = VerticalAlignment.Center, FontSize = 16,
                    Foreground = _textColor
                });
            Grid.SetColumn(grid.Children[3], 3);

            grid.Children.Add(new TextBlock
            {
                Text = member.LastName, VerticalAlignment = VerticalAlignment.Center, FontSize = 16,
                Foreground = _textColor
            });
            Grid.SetColumn(grid.Children[4], 4);

            if (enableEdit)
            {
                TextBox descriptionTextBox = new TextBox
                {
                    VerticalAlignment = VerticalAlignment.Center,
                    FontSize = 16,
                    Foreground = _textColor
                };

                grid.Children.Add(descriptionTextBox);

                descriptionTextBox.DataContext = member;

                Binding binding = new Binding("Description");
                binding.Mode = BindingMode.TwoWay;
                binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
                descriptionTextBox.SetBinding(TextBox.TextProperty, binding);
            }
            else
            {
                grid.Children.Add(new TextBlock
                {
                    Text = member.Description, VerticalAlignment = VerticalAlignment.Center, FontSize = 16,
                    Foreground = _textColor
                });
            }


            Grid.SetColumn(grid.Children[5], 5);

            Border border = new Border
            {
                BorderThickness = new Thickness(1),
                BorderBrush = _borderColor,
                CornerRadius = new CornerRadius(5),
                Padding = new Thickness(10),
                Child = grid,
                Background = i % 2 == 0 ? _evenRowColor : _oddRowColor // Alternate row background color
            };

            ReportView.Children.Add(border);
        }
    }

    public void SaveResults(object sender, RoutedEventArgs e)
    {
        try
        {
            foreach (var participant in EventResults.Participants)
            {
                participant.saveResultTime(_EventReportsRepository);
            }

            ExceptionText.Text = "De resultaten zijn opgeslagen!";
            ExceptionText.Foreground = Brushes.Lime;
        }
        catch (Exception exception)
        {
            ExceptionText.Text = exception.Message;
            ExceptionText.Foreground = Brushes.Red;
        }
    }
}