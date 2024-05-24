using DataAccessLibrary;
using RoeiVerenigingLibary;
using RoeiVerenigingLibrary;
using RoeiVerenigingWPF.Frames;
using System.Windows;
using System.Windows.Controls;

namespace RoeiVerenigingWPF.Pages
{
    public partial class DamageOverview : Page
    {
        private readonly ImageRepository _imageRepository = new ImageRepository();
        private readonly DamageService _service = new DamageService(new DamageRepository());
        public DamageOverview(MainWindow mw)
        {
            InitializeComponent();
            DataContext = this;
            MainWindow = mw;
            Damages = _service.GetRelatedToUser(MainWindow.LoggedInMember);
            GetImagesFromMail();
            SetImages();
        }
        public MainWindow MainWindow { set; get; }
        public List<Damage> Damages { set; get; }
        private void GetImagesFromMail()
        {
            Task task = new Task(() =>
            {
                EmailToDb.GetImagesFromEmail(_imageRepository);
            });
            task.Start();
        }
        private void CreateNewDamageReport(object sender, RoutedEventArgs e)
        {
            MainWindow.MainContent.Navigate(new ListBoatsForDamageReport(MainWindow));
        }
        private void Button_Click(object sender, RoutedEventArgs e)
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
                Dispatcher.Invoke(() =>
                {
                    ListView.Items.Refresh();
                });
            }).Start();
        }
    }
}