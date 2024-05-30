using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DataAccessLibrary;
using RoeiVerenigingLibrary;
using RoeiVerenigingLibrary.Model;
using RoeiVerenigingLibrary.Services;
using RoeiVerenigingWPF.Frames;

namespace RoeiVerenigingWPF.Pages.member;

public partial class DamageOverview : Page
{
    private readonly ImageRepository _imageRepository = new();
    private readonly DamageService _service = new(new DamageRepository());

    public DamageOverview(MainWindow mw)
    {
        InitializeComponent();
        DataContext = this;
        MainWindow = mw;
        Damages = _service.GetRelatedToUser(MainWindow.LoggedInMember);
        GetImagesFromMail();
    }

    public MainWindow MainWindow { set; get; }
    public List<Damage> Damages { set; get; }

    private void GetImagesFromMail()
    {
        var task = new Task(() => { EmailToDb.GetImagesFromEmail(_imageRepository); });
        task.Start();
    }

    private void CreateNewDamageReport(object sender, RoutedEventArgs e)
    {
        MainWindow.MainContent.Navigate(new ListBoatsForDamageReport(MainWindow));
    }

    private void Grid_Click(object sender, MouseButtonEventArgs e)
    {
        if (sender is Grid)
        {
            var casted = sender as Grid;
            var command = casted.Tag;
            var id = int.Parse(command.ToString());

            MainWindow.MainContent.Navigate(new ViewDamage(MainWindow, _service.GetById(id)));
        }
    }

    //todo: Tygo will add the images async
    private void ListView_Loaded(object sender, RoutedEventArgs e)
    {
        new Thread(async () =>
        {
            _service.AddFirstImageToClass(Damages, _imageRepository);
            Dispatcher.Invoke(() => { ListView.Items.Refresh(); });
        }).Start();
    }
}