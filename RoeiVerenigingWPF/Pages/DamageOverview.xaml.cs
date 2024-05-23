using System.Windows.Controls;
using DataAccessLibary;
using RoeiVerenigingLibary;
using RoeiVerenigingWPF.Frames;

namespace RoeiVerenigingWPF.Pages;

public partial class DamageOverview : Page
{
    public MainWindow MainWindow { set; get; }
    private DamageService _service = new DamageService(new DamageRepository()); 
    public List<Damage> Damages { set; get; }
    private ImageRepository _imageRepository = new ImageRepository();
    public DamageOverview(MainWindow mw)
    {
        InitializeComponent();
        DataContext = this;
        MainWindow = mw;
        Damages = _service.GetRelatedToUser(MainWindow.LoggedInMember);
        GetImagesFromMail();
        SetImages();
    }
    private void GetImagesFromMail()
    {
        Task task = new Task(() =>
        {
            EmailToDb.GetImagesFromEmail(_imageRepository);
        });
        task.Start();
    }
    private void CreateNewDamageReport(object sender, System.Windows.RoutedEventArgs e)
    {
        MainWindow.MainContent.Navigate(new ListDamageRoport(MainWindow));
    }
    private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        if (sender is Button)
        {
            Button casted = sender as Button;
            object command = casted.CommandParameter;
            int id = Int32.Parse(command.ToString());
            //todo send user to view page(the one with the qr code)
             MainWindow.MainContent.Navigate(new ViewDamage(MainWindow, _service.GetById(id)));
        }
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
            });
        }).Start();
    }
}