using System.IO.Compression;
using System.Windows;
using System.Windows.Controls;
using DataAccessLibrary;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Legends;
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
    
    private TrendService _trend = new TrendService();
    private ViewStatistics StatisticsWindow;
    public MainWindow Main;
    private List<Statistic> SelectedStatistics;
    private ReservationService _reservationService;
    private DamageService _damageService;
    private EventService _eventService;
    private BoatService _boatService;

    private PlotModel AddLine(PlotModel model, double[] x, double[] y, int lineThick, OxyColor colorValue)
    {
        var lineSeries = new LineSeries { StrokeThickness = lineThick, Color = colorValue };
        for (int i = 0; i < x.Length; i++)
        {
            lineSeries.Points.Add(new DataPoint(x[i], y[i]));
        }
        model.Series.Add(lineSeries);
        return model;
    }
    private PlotModel AddBar(PlotModel model, double[] y, OxyColor color, string tit = "")
    {
        OxyPlot.Series.BarSeries ser = new BarSeries();
        ser.Title = tit;
        ser.FillColor = color;
        ser.LabelPlacement = LabelPlacement.Inside;
        var dd = new List<BarItem>();
        foreach (double yy in y)
            dd.Add(new BarItem { Value = yy });
        ser.ItemsSource = dd;
        model.Series.Add(ser);
        return model;

    }
    private PlotModel AddCollumn(PlotModel model, double[][] x, double[][] y, double barWidth, OxyColor[] colorValue)
    {
        for (int s = 0; s < y.Length; s++)
        {
            var lineSeries = new LinearBarSeries { BarWidth = barWidth, FillColor = colorValue[s] };
            for (int i = 0; i < x[0].Length; i++)
            {
                lineSeries.Points.Add(new DataPoint(x[s][i], y[s][i]));
            }
            model.Series.Add(lineSeries);
        }
        return model;
    }
    
    
    public void PlotLineGraph()
    {
        //replace this with the actual data from database
        double[] x = Enumerable.Range(1, 100).Select(v => ((double)v) / 10).ToArray();
        double[] y = x.Select(v => v * v).ToArray();
        
        var model = new PlotModel { Title = "OxyPlot - Line Series" };
        model.Axes.Add(new LinearAxis
        {
            Position = AxisPosition.Left,
            Title = "Y",
        });
        model.Axes.Add(new LinearAxis
        {
            Position = AxisPosition.Bottom,
            Title = "Number of items",
        });
        model = AddLine(model, x, y, 2, OxyColors.Gray);
        model.Series[model.Series.Count - 1].Title = "X2";
        model.Legends.Add(new OxyPlot.Legends.Legend()
        {
            LegendPosition = LegendPosition.TopLeft,
            LegendFontSize = 12
        });
        PlotView.Model = model;
    }

    public void PlotBarGraph(double[] data)
    {

        // double[] data = { 1, 4, 9, 16, 25, 36, 49, 64, 81, 100 };
        var model = new PlotModel { Title = "OxyPlot - Bar Series" };
        model.Axes.Add(new CategoryAxis
        {
            Position = AxisPosition.Left,
            Title = "Y",
        });


        model = AddBar(model, data, OxyColors.DarkGreen);
        model.Series[model.Series.Count - 1].Title = "X2";
        model.Legends.Add(new OxyPlot.Legends.Legend()
        {
            LegendPosition = LegendPosition.BottomRight,
            LegendFontSize = 12
        });
        PlotView.Model = model;
    }
    private void PlotCollumn(object sender, RoutedEventArgs e)
    {
        double[][] x = new double[2][];
        x[0] = new double[] { 1.2, 2.1, 2.9, 4.5, 5, 5.7, 6.9, 8.3, 9.12, 10.0 };
        x[1] = new double[] { 1, 1.8, 3.3, 4.0, 5.7, 6.3, 7.6, 7.3, 8.9, 10.6 };
        double[][] y = new double[2][];
        y[0] = new double[] { 1.8, 3.8, 9.6, 15.1, 25.6, 38, 47, 60, 84, 102 };
        y[1] = new double[] { -1.9, -3.8, -9.6, -15.1, -25.6, -38, -47, -60, -84, -102 };
        var model = new PlotModel { Title = "OxyPlot - Column Series" };
        model.Axes.Add(new LinearAxis
        {
            Position = AxisPosition.Left,
            ExtraGridlines = new Double[] { 0 },
            Title = "Y",
        });
        model.Axes.Add(new LinearAxis
        {
            Position = AxisPosition.Bottom,
            Title = "Number of items",
        });
        var cl = new OxyColor[] { OxyColors.DarkGreen, OxyColors.DarkRed };

        model = AddCollumn(model, x, y, 15, cl);
        model.Series[model.Series.Count - 2].Title = "X2";
        model.Series[model.Series.Count - 1].Title = "-X2";
        model.Legends.Add(new OxyPlot.Legends.Legend()
        {
            LegendPosition = LegendPosition.BottomRight,
            LegendFontSize = 12
        });
        PlotView.Model = model;
        
    }

    public void FillDropDown(List<Statistic> SelectedStatistics)
    {
        foreach (var stat in SelectedStatistics)
        {
            ComboStats.Items.Add(stat.Name);
        }
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
            Key = "CategoryAxis"
        };
        plotModel.Axes.Add(categoryAxis);

        // Configure value axis
        var valueAxis = new OxyPlot.Axes.CategoryAxis
        {
            Position = OxyPlot.Axes.AxisPosition.Left,
            Title = "Boten",
            MinimumPadding = 0,
            AbsoluteMinimum = 0,
            ItemsSource = labels
        };
        plotModel.Axes.Add(valueAxis);

        return plotModel;
    }

    public void getData(object sender, SelectionChangedEventArgs e)
    {
        ComboBox comboBox = sender as ComboBox;
        switch (comboBox.SelectedItem.ToString())
        {
            case "Populairste boot:":
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
            case "Grootste Evenement:":
                Title.Content = "Grootste Evenementen";
                
                var biggest = _eventService.GetBiggest5EventspastMonth();
                amounts = new Double[biggest.Count];
                var names = new string[biggest.Count];
                for (int i = 0; i < biggest.Count; i++)
                {
                    names[i] = biggest[i].Name;
                    amounts[i] = biggest[i].Participants.Count;
                }

                PlotView.Model = PlotBarGraph(amounts, names);
                break;
            case "Totale reserveringstijd:":
                Title.Content = "Totale reserveringstijd";
                break;
            case "Open schademeldingen":
                Title.Content = "Open schademeldingen";
                break;
            case "Actiefste lid":
                Title.Content = "Actiefste lid";
                break;
            case "Minst actieve lid:":
                Title.Content = "Minst actieve lid";
                break;
            case "Ongelukkigste lid:":
                Title.Content = "Ongelukkigste lid";
                break;
            case "Aantal leden":
                Title.Content = "Aantal leden";
                break;
            case "Totaal aantal reserveringen":
                Title.Content = "aantal reserveringen";
                break;
            case "Aantal boten":
                Title.Content = "Aantal boten";
                break;
        }
        
    }
    

}