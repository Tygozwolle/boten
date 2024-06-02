﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using DataAccessLibrary;
using RoeiVerenigingLibrary;
using RoeiVerenigingLibrary.Model;
using RoeiVerenigingWPF.Frames;

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

            grid.Children.Add(new TextBlock
            {
                Text = member.ResultTime.ToString(), VerticalAlignment = VerticalAlignment.Center, FontSize = 16,
                Foreground = _textColor
            });
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

            grid.Children.Add(new TextBlock
            {
                Text = member.Description, VerticalAlignment = VerticalAlignment.Center, FontSize = 16,
                Foreground = _textColor
            });
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
}