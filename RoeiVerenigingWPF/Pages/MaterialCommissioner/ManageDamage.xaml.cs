using DataAccessLibrary;
using RoeiVerenigingLibrary;
using RoeiVerenigingLibrary;
using RoeiVerenigingWPF.Frames;
using RoeiVerenigingWPF.helpers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace RoeiVerenigingWPF.Pages
{
    public partial class ManageDamage : Page
    {
        private int _imageIndex;
        private readonly ImageRepository _imageRepository = new ImageRepository();
        private readonly List<ImageSource> _images;

        private readonly DamageService _service = new DamageService(new DamageRepository());

        public ManageDamage(MainWindow mw, Damage damage)
        {
            InitializeComponent();
            DataContext = this;
            _mainWindow = mw;
            Damage = damage;
            Usable.IsChecked = damage.Usable;
            Fixed.IsChecked = damage.BoatFixed;
            Description.Text = damage.Description;
            qrCodeImage.Source = QrcodeMaker.Qrcode(damage.Id);
            _images = ImageConverter.ConvertList(Damage.Images);
            if (_images.Count != 0)
            {
                DamageImage.Source = _images[0];
            }
            else
            {
                ___Next_.IsEnabled = false;
                ___Prev_.IsEnabled = false;
            }
        }

        private MainWindow _mainWindow { get; }
        public Damage Damage { set; get; }

        private int ImageIndex
        {
            get => _imageIndex;
            set
            {
                if (value < 0)
                {
                    _imageIndex = _images.Count - 1;
                }
                else if (value > _images.Count - 1)
                {
                    _imageIndex = 0;
                }
                else
                {
                    _imageIndex = value;
                }
            }
        }

        private void NextImage(object sender, RoutedEventArgs e)
        {
            ImageIndex++;
            DamageImage.Source = _images[ImageIndex];
        }

        private void PrevImage(object sender, RoutedEventArgs e)
        {
            ImageIndex--;
            DamageImage.Source = _images[ImageIndex];
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            bool boatFixed = false;
            bool usable = false;
            if (Fixed.IsChecked == true)
                boatFixed = true;
            if (Usable.IsChecked == true)
                usable = true;
            _service.Update(Damage.Id, boatFixed, usable, Description.Text);
            _mainWindow.MainContent.Navigate(new ManageDamageOverview(_mainWindow));
        }

        private void update_images_Click(object sender, RoutedEventArgs e)
        {
            update_images.IsEnabled = false;
            Mouse.OverrideCursor = Cursors.Wait;
            new Thread(() =>
            {
                EmailToDb.GetImagesFromEmail(_imageRepository);
                Damage updatedDamage = _service.GetById(Damage.Id);
                Dispatcher.Invoke(() =>
                {
                    Mouse.OverrideCursor = null;
                    _mainWindow.MainContent.Navigate(new ManageDamage(_mainWindow, updatedDamage));
                });
            }).Start();
        }
    }
}