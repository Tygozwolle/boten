using System.Windows;
using System.Windows.Controls;
using DataAccessLibary;
using RoeiVerenigingLibary;
using RoeiVerenigingLibrary;
using RoeiVerenigingWPF.Frames;

namespace RoeiVerenigingWPF.Pages;

public partial class ManageDamageOverview : Page
{
    private readonly ImageRepository _imageRepository = new();
    private readonly DamageService _service = new(new DamageRepository());

    public ManageDamageOverview(MainWindow mw)
    {
        InitializeComponent();
        DataContext = this;
        Loaded += loadedEvent;
        MainWindow = mw;
        Damages = _service.GetAll();
        GetImagesFromMail();
        SetImages();
    }

    public MainWindow MainWindow { set; get; }
    public List<Damage> Damages { set; get; }

    private void GetImagesFromMail()
    {
        var task = new Task(() => { EmailToDb.GetImagesFromEmail(_imageRepository); });
        task.Start();
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button)
        {
            var casted = sender as Button;
            var command = casted.CommandParameter;
            var id = int.Parse(command.ToString());

            MainWindow.MainContent.Navigate(new ManageDamage(MainWindow, _service.GetById(id)));
        }

        Damages.Count();
    }

    private void SetImagesAsync()
    {
        var thread = new Thread(() => { SetImages(); });
        thread.Start();
    }

    private void SetImages()
    {
        new Thread(() =>
        {
            foreach (var damage in Damages)
            {
                var damageSave = damage;

                damage.Images = [_imageRepository.GetFirstImage(damageSave.Id)];
            }

            Dispatcher.Invoke(() => { ListView.Items.Refresh(); });
        }).Start();
    }

    private void loadedEvent(object sender, RoutedEventArgs args)
    {
        //   SetImagesAsync();
    }
}