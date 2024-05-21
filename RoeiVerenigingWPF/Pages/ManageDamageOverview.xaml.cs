using System.Windows.Controls;
using DataAccessLibary;
using RoeiVerenigingLibary;
using RoeiVerenigingWPF.Frames;

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
        MainWindow = mw;
        Damages = _service.GetAll();
        GetImagesFromMail();
    }

    private void GetImagesFromMail()
    {
        Task task = new Task(() =>
        {
            EmailToDb.GetImagesFromEmail(_imageRepository);
        });
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
    }
}