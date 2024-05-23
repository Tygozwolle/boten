using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using DataAccessLibary;
using RoeiVerenigingLibary;
using RoeiVerenigingLibrary;
using RoeiVerenigingWPF.Frames;
using RoeiVerenigingWPF.helpers;

namespace RoeiVerenigingWPF.Pages;

public partial class ManageDamage : Page
{
    private MainWindow _mainWindow { set; get; }
    public Damage Damage { set; get; }
    private ImageRepository _imageRepository = new ImageRepository();

    private DamageService _service = new DamageService(new DamageRepository());
    private List<ImageSource> _images;
    private int _imageIndex;

    private int imageIndex
    {
        get { return _imageIndex; }
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

    public ManageDamage(MainWindow mw, Damage damage)
    {
        InitializeComponent();
        DataContext = this;
        _mainWindow = mw;
        Damage = damage;
        Usable.IsChecked = damage.Usable;
        Fixed.IsChecked = damage.BoatFixed;
        Description.Text = damage.Description;
        qrCodeImage.Source = QrcodeMaker.qrcode(damage.Id);
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

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        bool _fixed = false;
        bool _usable = false;
        if (Fixed.IsChecked == true)
            _fixed = true;
        if (Usable.IsChecked == true)
            _usable = true;
        _service.Update(Damage.Id, _fixed, _usable, Description.Text);
        _mainWindow.MainContent.Navigate(new ManageDamageOverview(_mainWindow));
    }

    private void update_images_Click(object sender, RoutedEventArgs e)
    {
        update_images.IsEnabled = false;
        Mouse.OverrideCursor = Cursors.Wait;
        new Thread(() =>
        {
            EmailToDb.GetImagesFromEmail(_imageRepository);
            var UpdatedDamage = _service.GetById(Damage.Id);
            this.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = null;
                _mainWindow.MainContent.Navigate(new ManageDamage(_mainWindow, UpdatedDamage));
            });
        }).Start();
    }
}