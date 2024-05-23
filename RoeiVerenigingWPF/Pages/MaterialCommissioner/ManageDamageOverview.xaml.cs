using DataAccessLibary;
using RoeiVerenigingLibary;
using RoeiVerenigingLibrary;
using RoeiVerenigingWPF.Frames;
using System.Windows;
using System.Windows.Controls;

namespace RoeiVerenigingWPF.Pages
{
    public partial class ManageDamageOverview : Page
    {
        private readonly ImageRepository _imageRepository = new ImageRepository();
        private readonly DamageService _service = new DamageService(new DamageRepository());

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
            Task task = new Task(() => { EmailToDb.GetImagesFromEmail(_imageRepository); });
            task.Start();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
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
            Thread thread = new Thread(() =>
            {
                SetImages();
            });
            thread.Start();
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

        private void loadedEvent(object sender, RoutedEventArgs args)
        {
            //   SetImagesAsync();
        }
    }
}