using System.Windows.Controls;
using DataAccessLibrary;
using OxyPlot;
using OxyPlot.Series;
using RoeiVerenigingLibrary;
using RoeiVerenigingLibrary.Model;
using RoeiVerenigingLibrary.Services;
using RoeiVerenigingWPF.Frames;

namespace RoeiVerenigingWPF.Pages.Admin;

public partial class ViewTrends : Page
{
    public ViewTrends(List<Statistic> statistics)
    {
        SelectedStatistics = statistics;
    }
    
    public ViewTrends(MainWindow mainWindow, List<Statistic> selectedStatistics)
    {
        InitializeComponent();
        Main = mainWindow;
        DataContext = this;
        FillDropDown(selectedStatistics);
        _reservationService = new ReservationService(new ReservationRepository());
        _eventService = new EventService(new EventRepository());
        _damageService = new DamageService(new DamageRepository());
        _boatService = new BoatService(new BoatRepository());
        
    }
    
    public MainWindow Main;
    private List<Statistic> SelectedStatistics;
    private ReservationService _reservationService;
    private DamageService _damageService;
    private EventService _eventService;
    private BoatService _boatService;

    public void FillDropDown(List<Statistic> selectedStatistics)
    {
        ComboStats.Items.Add("Populairste boot");
        ComboStats.Items.Add("Minst populaire boot");
        ComboStats.Items.Add("Grootste evenement");
        ComboStats.Items.Add("Totale reserveringstijd");
        ComboStats.Items.Add("Open schademeldingen");
        ComboStats.Items.Add("Totaal aantal reserveringen");
        ComboStats.Items.Add("Aantal boten");
    }
    

    public PlotModel PlotBarGraph(double[] amounts, string[] labels)
    {
        var plotModel = new PlotModel();

        // Create a bar series
        var barGraph = new BarSeries
        {
            Title = "Amounts",
            LabelPlacement = LabelPlacement.Outside,
            LabelFormatString = "{0}"
        };
        Label Label = new Label();
        
        // Add data points to the bar series
        for (int i = 0; i < amounts.Length; i++)
        {
            barGraph.Items.Add(new BarItem { Value = amounts[i]});
        }

        // Add the bar series to the plot model
        plotModel.Series.Add(barGraph);

        // Configure category axis
        var categoryAxis = new OxyPlot.Axes.CategoryAxis
        {
            Position = OxyPlot.Axes.AxisPosition.Bottom,
            Key = "CategoryAxis",
            AbsoluteMaximum = 200,
            AbsoluteMinimum = 1
        };
        plotModel.Axes.Add(categoryAxis);

        // Configure value axis
        var valueAxis = new OxyPlot.Axes.CategoryAxis
        {
            Position = OxyPlot.Axes.AxisPosition.Left,
            Title = "Boten",
            MinimumPadding = 0,
            AbsoluteMinimum = -5,
            AbsoluteMaximum = 250,
            ItemsSource = labels
        };
        plotModel.Axes.Add(valueAxis);

        return plotModel;
    }

    public PlotModel plotLinegraph(double[] xAxis, double[] yAxis)
    {
        PlotModel plotModel = new PlotModel { Title = "Line Graph Example" };

        // Create a line series
        LineSeries lineSeries = new LineSeries
        {
            MarkerType = MarkerType.Circle,
            MarkerSize = 4,
            MarkerStroke = OxyColors.White,
            MarkerFill = OxyColors.Blue,
            Color = OxyColors.SkyBlue
        };

        // Add data points to the line series
        for (int i = 0; i < xAxis.Length; i++)
        {
            lineSeries.Points.Add(new DataPoint(xAxis[i], yAxis[i]));
        }

        // Add the line series to the plot model
        plotModel.Series.Add(lineSeries);
        
        return plotModel;
    }

    public PlotModel PlotPiechart(Dictionary<string, int> values)
    {
        PlotModel plotModel = new PlotModel();
        
        PieSeries pieSeries = new PieSeries
        {
            InsideLabelColor = OxyColors.Automatic,
            StrokeThickness = 2.0,
            InsideLabelPosition = 0.5,
            AngleSpan = 360, // Full circle
            StartAngle = 0 // Start angle
        };

        foreach (var kvp in values)
        {
            pieSeries.Slices.Add(new PieSlice(kvp.Key, kvp.Value));
        }
        
        plotModel.Series.Add(pieSeries);

        return plotModel;
    }

    public void getData(object sender, SelectionChangedEventArgs e)
    {
        ComboBox comboBox = sender as ComboBox;
        switch (comboBox.SelectedItem.ToString())
        {
            case "Populairste boot":
                Title.Content = "Populairste boot";
                var topValues = _boatService.GetTopFiveBoats(_reservationService.GetReservations());
                var getTop5 = topValues.OrderByDescending(pair => pair.Value)  //Linq query for getting top 5 values
                    .Take(5)
                    .ToDictionary(pair => pair.Key.Name, pair => (double)pair.Value);
                string[] boats = getTop5.Keys.ToArray();
                double[] amounts = getTop5.Values.ToArray();
                PlotView.Model = PlotBarGraph(amounts, boats);
                break;
            case "Minst populaire boot":
                Title.Content = "Minst populaire boot";
                var dictionary = _boatService.GetTopFiveBoats(_reservationService.GetReservations());
                var getLast5 = dictionary.OrderBy(pair => pair.Value)  //Linq query for getting top 5 values
                    .Take(5)
                    .ToDictionary(pair => pair.Key.Name, pair => (double)pair.Value);
                boats = getLast5.Keys.ToArray();
                amounts = getLast5.Values.ToArray();
                PlotView.Model = PlotBarGraph(amounts, boats);
                break;
            case "Grootste evenement":
                Title.Content = "Grootste Evenementen";
                var biggest = _eventService.GetBiggest5EventspastMonth();
                amounts = biggest.Values.ToArray();
                var names = biggest.Keys.ToArray();
                PlotView.Model = PlotBarGraph(amounts, names);
                break;
            case "Totale reserveringstijd":
                Title.Content = "Totale reserveringstijd per week"; //linegraph maken voor afgelopen 5 weken met hoeveelheid reserveringen voor de ingelogde member
                var weeklyReservationTime = _reservationService.GetWeeklyReservationTime(Main.LoggedInMember);
                var yAxis = weeklyReservationTime.Values.ToArray();
                var xAxis = weeklyReservationTime.Keys.ToArray();
                PlotView.Model = plotLinegraph(xAxis, yAxis);
                break;
            case "Open schademeldingen":
                Title.Content = "Open schademeldingen";
                var DamageReportData = _damageService.AllDamageReportsSorted();
                PlotView.Model = PlotPiechart(DamageReportData);
                break;
            case "Totaal aantal reserveringen":
                Title.Content = "aantal reserveringen";
                var weeklyReservations = _reservationService.GetAmountOfWeeklyreservations(Main.LoggedInMember);
                yAxis = weeklyReservations.Values.ToArray();
                xAxis = weeklyReservations.Keys.ToArray();
                PlotView.Model = plotLinegraph(xAxis, yAxis);
                break;
            case "Aantal boten":
                Title.Content = "Boten gesorteerd op Niveau";
                var sortedBoats = _boatService.BoatsPerLevel();
                PlotView.Model = PlotPiechart(sortedBoats);
                break;
        }
        
    }
    

}