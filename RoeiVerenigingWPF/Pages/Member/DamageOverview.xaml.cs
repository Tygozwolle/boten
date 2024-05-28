using DataAccessLibrary;
using RoeiVerenigingLibrary;
using RoeiVerenigingLibrary;
using RoeiVerenigingWPF.Frames;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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
        
        private void Grid_Click(object sender, MouseButtonEventArgs e)
        {
            if (sender is Grid)
            {
                Grid casted = sender as Grid;
                object command = casted.Tag;
                int id = Int32.Parse(command.ToString());

                MainWindow.MainContent.Navigate(new ViewDamage(MainWindow, _service.GetById(id)));
            }
        }
        
        //todo: Tygo will add the images async
        private void ListView_Loaded(object sender, RoutedEventArgs e)
        {
            new Thread(async () =>
            {
                _service.AddFirstImageToClass(Damages, _imageRepository);
                this.Dispatcher.Invoke(() =>
                {
                    ListView.Items.Refresh();
                });
            }).Start();
        }

     }
}