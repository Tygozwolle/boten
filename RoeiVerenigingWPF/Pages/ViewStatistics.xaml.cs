using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media.Imaging;
using DataAccessLibrary;
using RoeiVerenigingLibrary;
using RoeiVerenigingLibrary.Model;
using RoeiVerenigingWPF.Frames;
using RoeiVerenigingWPF.helpers;

namespace RoeiVerenigingWPF.Pages;

public partial class ViewStatistics : Page
{
    public List<Statistic> SelectedStatistics { get; set; }
    public List<Statistic> AllStatistics { get; set; }
    private ReservationService _reservationService = new ReservationService(new ReservationRepository());
    private BoatService _boatService = new BoatService(new BoatRepository());
    public MainWindow MainWindow;

    public ViewStatistics(MainWindow mainWindow)
    {
        InitializeComponent();
        MainWindow = mainWindow;
        FillAllStatistics();
    }

    private void FillAllStatistics()
    {
        AllStatistics = new List<Statistic>() { };
        SelectedStatistics = new List<Statistic>() { };

        AllStatistics.Add(new Statistic(1, "Totale reserveringstijd:",
            "Dit is de totale tijd van al jouw eigen reserveringen!", _reservationService.GetTotalReservationTime(MainWindow.LoggedInMember).ToString(), true));
        AllStatistics.Add(
            new Statistic(2, "Populaiste boot:", "Deze boot wordt het meeste gereserveerd!", _boatService.GetMostPopulairBoat(_reservationService.GetReservations()).Name, true));
        AllStatistics.Add(new Statistic(3, "Minst populaire boot:", "Deze boot wordt het minste gereserveerd!",
            "Anna 2.0", true));
        AllStatistics.Add(new Statistic(4, "Grootste evenement:", "Aan dit evenement deden de meeste mensen mee!",
            "Anna", true));
        AllStatistics.Add(new Statistic(5, "Actiefste lid:", "Dit lid heeft de meeste reseveringen!", "Pieter", true));
        AllStatistics.Add(new Statistic(6, "Ongelukkiste lid:", "Dit lid heeft de meeste schade gemeld!", "Tygo",
            true));
        AllStatistics.Add(new Statistic(7, "Aantal leden:", "Dit is het totale aantal leden.", "150 leden", true));
        AllStatistics.Add(new Statistic(8, "Totaal aantal reserveringen:",
            "Dit is het totale aantal reserveringen dat is gemaakt.", "300 reserveringen", true));
        AllStatistics.Add(new Statistic(9, "Open schademeldingen:",
            "Dit is de hoeveelheid schademeldingen die momenteel open staan", "20 meldingen", true));
        AllStatistics.Add(new Statistic(10, "Aantal boten", "Dit is het aantal boten dat de vereniging heeft!",
            "20 boten", true));

        foreach (Statistic statistic in AllStatistics)
        {
            if (statistic.Selected)
            {
                SelectedStatistics.Add(statistic);
            }
        }

        PopulateStatisticsGrid();
    }


    public void PopulateStatisticsGrid()
    {
        for (int i = 0; i < SelectedStatistics.Count; i++)
        {
            Statistic stat = SelectedStatistics[i];
            Grid grid = new Grid();

            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(40) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(90) });

            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(240) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(130) });

            grid.Tag = stat.Id;

            grid.Children.Add(new TextBlock
            {
                Text = stat.Name, VerticalAlignment = VerticalAlignment.Top, FontSize = 24,
                HorizontalAlignment = HorizontalAlignment.Center,
                Foreground = CustomColors.HeaderColor
            });
            Grid.SetColumnSpan(grid.Children[0], 2);
            Grid.SetRow(grid.Children[0], 0);

            grid.Children.Add(new TextBlock
            {
                Text = stat.Description, VerticalAlignment = VerticalAlignment.Top, FontSize = 16,
                HorizontalAlignment = HorizontalAlignment.Left, TextWrapping = TextWrapping.Wrap,
                Foreground = CustomColors.SubHeaderColor
            });
            Grid.SetRow(grid.Children[1], 1);
            Grid.SetColumn(grid.Children[1], 0);

            grid.Children.Add(new TextBlock
            {
                Text = stat.Value, VerticalAlignment = VerticalAlignment.Top, FontSize = 24,
                HorizontalAlignment = HorizontalAlignment.Right, TextWrapping = TextWrapping.Wrap,
                Foreground = CustomColors.SubHeaderColor
            });
            Grid.SetRow(grid.Children[2], 1);
            Grid.SetColumn(grid.Children[2], 1);

            Border border = new Border
            {
                BorderThickness = new Thickness(3),
                Background = CustomColors.TextBoxBackgroundColor,
                BorderBrush = CustomColors.OutsideBorderColor,
                CornerRadius = new CornerRadius(5),
                Padding = new Thickness(10),
                Margin = new Thickness(5),
                Child = grid,
            };

            StatisticsPanel.Children.Add(border);
        }
    }

    private List<string> CalculateStatistics()
    {
        List<string> calculatedList = [];


        return calculatedList;
    }
}