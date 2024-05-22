using System.IO;
using System.Windows;
using System.Windows.Controls;
using Aspose.Email.Clients.Exchange.WebService.Schema_2016;
using DataAccessLibary;
using RoeiVerenigingLibary;
using RoeiVerenigingWPF.Frames;
using Xceed.Wpf.Toolkit.Core;

namespace RoeiVerenigingWPF.Pages;

public partial class ManageDamageOverview : Page
{
    public MainWindow MainWindow { set; get; }
    private DamageService _service = new DamageService(new DamageRepository());
    private ImageRepository _imageRepository = new ImageRepository();
    public List<Damage> Damages { set; get; }

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

    private void GetImagesFromMail()
    {
        Task task = new Task(() => { EmailToDb.GetImagesFromEmail(_imageRepository); });
        task.Start();
    }

    private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        if (sender is Button)
        {
            Button casted = sender as Button;
            object command = casted.CommandParameter;
            int id = Int32.Parse(command.ToString());

            MainWindow.MainContent.Navigate(new ManageDamage(MainWindow, _service.GetById(id)));
        }

        Damages.Count();
    }

    private void SetImagesAsync()
    {
        //  List<Task> tasks = new List<Task>();
       var thread = new Thread(() =>
        {
          SetImages();

        });
       thread.Start();
       //thread.Join();
       // Task.WaitAll(tasks.ToArray());

    }

    private void SetImages()
    {
        new Thread(() =>
        {
            foreach (Damage damage in Damages)
            {
                Damage damageSave = damage;
                
                damage.Images = [_imageRepository.GetFirstImage(damageSave.Id)];
            }
            this.Dispatcher.Invoke(() =>
                                        {
                                            ListView.Items.Refresh();
                                            // ListView.ItemsSource = Damages;

                                        });
        }).Start();
    }

    private void loadedEvent(object sender, RoutedEventArgs args)
    {
     //   SetImagesAsync();
    }
}