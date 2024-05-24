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
    /// <summary>
    ///     Interaction logic for ViewDamage.xaml
    /// </summary>
    public partial class ViewDamage : Page
    {
        private int _imageIndex;
        private readonly ImageRepository _imageRepository = new ImageRepository();
        private readonly List<ImageSource> _images;

        private readonly DamageService _service = new DamageService(new DamageRepository());

        public ViewDamage(MainWindow mw, Damage damage)
        {
            InitializeComponent();
            DataContext = this;
            _mainWindow = mw;
            Damage = damage;
            Description.Text = damage.Description;
            _images = ImageConverter.ConvertList(Damage.Images);
            qrCodeImage.Source = QrcodeMaker.qrcode(damage.Id);
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

        private int imageIndex
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
            imageIndex++;
            DamageImage.Source = _images[imageIndex];
        }

        private void PrevImage(object sender, RoutedEventArgs e)
        {
            imageIndex--;
            DamageImage.Source = _images[imageIndex];
        }

        private void update_images_Click(object sender, RoutedEventArgs e)
        {
            update_images.IsEnabled = false;
            Mouse.OverrideCursor = Cursors.Wait;
            new Thread(() =>
            {
                EmailToDb.GetImagesFromEmail(_imageRepository);
                Damage UpdatedDamage = _service.GetById(Damage.Id);
                Dispatcher.Invoke(() =>
                {
                    Mouse.OverrideCursor = null;
                    _mainWindow.MainContent.Navigate(new ViewDamage(_mainWindow, UpdatedDamage));
                });
            }).Start();
        }
    }
}