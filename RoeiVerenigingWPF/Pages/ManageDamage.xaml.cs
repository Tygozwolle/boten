using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using DataAccessLibary;
using RoeiVerenigingLibary;
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
    public ManageDamage(MainWindow mw, Damage damage)
    {
        InitializeComponent();
        DataContext = this;
        _mainWindow = mw;
        Damage = damage;
        Usable.IsChecked = damage.Usable;
        Fixed.IsChecked = damage.BoatFixed;
        Description.Text = damage.Description;
        _images = ImageConverter.ConvertList(Damage.Images);
        if (_images.Count != 0)
        {
            DamageImage.Source = _images[0];
        }
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
 
}