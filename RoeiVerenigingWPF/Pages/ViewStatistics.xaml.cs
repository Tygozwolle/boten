using System.Runtime.InteropServices.JavaScript;
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
    private DamageService _damageService = new DamageService(new DamageRepository());
    private MemberService _memberService = new MemberService(new MemberRepository());
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

        List<RoeiVerenigingLibrary.Member> memberList = _memberService.GetMembers();
        List<Reservation> reservationsList = _reservationService.GetReservations();
        List<Damage> damageList = _damageService.GetAll();
        List<Boat> boatList = _boatService.GetBoats();

        try
        {
            AllStatistics.Add(new Statistic(1, "Totale reserveringstijd:",
                "Dit is de totale tijd van al jouw eigen reserveringen!",
                _reservationService.GetTotalReservationTime(MainWindow.LoggedInMember, reservationsList) + " uur",
                true));
            AllStatistics.Add(
                new Statistic(2, "Populaiste boot:", "Deze boot wordt het meeste gereserveerd!",
                    _boatService.GetMostANdLeastPopulairBoat((_reservationService.GetReservations()))[1].Name, true));
            AllStatistics.Add(new Statistic(3, "Minst populaire boot:", "Deze boot wordt het minste gereserveerd!",
                _boatService.GetMostANdLeastPopulairBoat((_reservationService.GetReservations()))[0].Name, true));
            AllStatistics.Add(new Statistic(4, "Grootste evenement:", "Aan dit evenement deden de meeste mensen mee!",
                "Les 1", false));
            AllStatistics.Add(new Statistic(5, "Actiefste lid:", "Dit lid heeft de meeste reseveringen!",
                _reservationService.GetMostAndLeastActiveMember(reservationsList)[1].FirstName, true));
            AllStatistics.Add(new Statistic(7, "Minst actieve lid:", "Dit lid heeft de minste reserveringen staan.",
                _reservationService.GetMostAndLeastActiveMember(reservationsList)[0].FirstName,
                true));
            AllStatistics.Add(new Statistic(8, "Ongelukkiste lid:", "Dit lid heeft de meeste schade gemeld!",
                _damageService.MemberWithMostDamage(damageList).FirstName,
                true));
            AllStatistics.Add(new Statistic(9, "Aantal leden:", "Onze vereniging heeft zoveel leden!",
                memberList.Count.ToString(), true));
            AllStatistics.Add(new Statistic(10, "Totaal aantal reserveringen:",
                "Dit is het totale aantal reserveringen dat is gemaakt.", reservationsList.Count().ToString(), true));
            AllStatistics.Add(new Statistic(11, "Open schademeldingen:",
                "Dit is de hoeveelheid schademeldingen die momenteel open staan",
                _damageService.AmountOfOpenDamageReports(damageList).ToString(), true));
            AllStatistics.Add(new Statistic(12, "Aantal boten:",
                "Dit is het aantal boten dat de vereniging heeft!",
                boatList.Count().ToString(), true));
        }
        catch (Exception e)
        {
            ExceptionTextBlock.Text = e.Message;
        }
        

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