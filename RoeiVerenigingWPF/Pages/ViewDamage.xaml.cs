using DataAccessLibary;
using RoeiVerenigingLibary;
using RoeiVerenigingWPF.Frames;
using RoeiVerenigingWPF.helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RoeiVerenigingWPF.Pages
{
    /// <summary>
    /// Interaction logic for ViewDamage.xaml
    /// </summary>
    public partial class ViewDamage : Page
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

        public ViewDamage(MainWindow mw, Damage damage)
        {
            InitializeComponent();
            DataContext = this;
            _mainWindow = mw;
            Damage = damage;
            Description.Text = damage.Description;
            _images = ImageConverter.ConvertList(Damage.Images);
            if (_images.Count != 0)
            {
                DamageImage.Source = _images[0];
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
    }
}
