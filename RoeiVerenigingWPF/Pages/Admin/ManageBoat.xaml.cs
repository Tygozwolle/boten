using System.Windows;
using DataAccessLibrary;
using RoeiVerenigingLibrary;
using System.Windows.Controls;
using Microsoft.Win32;
using RoeiVerenigingLibrary.Exceptions;
using RoeiVerenigingWPF.Frames;
using RoeiVerenigingWPF.helpers;
using System.IO;
using System.Windows.Media;

namespace RoeiVerenigingWPF.Pages.Admin
{
    public partial class ManageBoat : Page
    {
        private MainWindow _mainWindow;
        private bool _edit;
        private Boat _boat;
        private bool _imageChanged = false;
        private Stream _imageStream;

        public ManageBoat(MainWindow mainWindow, Boat boat)
        {
            _mainWindow = mainWindow;
            InitializeComponent();
            
            _boat = boat;
            Name.Text = boat.Name;
            Description.Text = boat.DescriptionNoEnter;
            Seats.Text = boat.Seats.ToString();
            Level.Text = boat.Level.ToString();
            Delete_Button.Visibility = Visibility.Visible;
            Captain.IsChecked = boat.CaptainSeat;
            
            _edit = true;
            ButtonEditCreate.Content = "Opslaan";
            HeaderBoat.Content = "Boot aanpassen";
            TextBlockBoat.Text = "Wijzig hier de informatie van de boot.";
            if (boat.Image != null)
            {
                Image.Source = ImageConverter.Convert(boat.Image);
            }
        }

        public ManageBoat(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;

            InitializeComponent();
            Delete_Button.Visibility = Visibility.Hidden;
            Captain.IsChecked = false;
            ExceptionTextBlock.Foreground = Brushes.Red;
        }

        private void ToggleButtonClick(object sender, RoutedEventArgs e)
        {
            if (Captain.IsChecked == true)
            {
                Captain.Content = "Stuurman aanwezig";
            }
            else
            {
                Captain.Content = "Stuurman afwezig";
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ExceptionTextBlock.Foreground = Brushes.Red;
            BoatService service = new BoatService(new BoatRepository());
            try
            {
                string name = Name.Text;
                string description = Description.Text;
                string seats = Seats.Text;
                string level = Level.Text;
                bool captain = Captain.IsPressed;
                Boat createdBoat;
                if (_edit)
                {
                    _boat = service.Update(_mainWindow.LoggedInMember, _boat, name, description, Int32.Parse(seats),
                        captain, Int32.Parse(level));
                    if (_imageChanged)
                    {
                        service.UpdateImage(_mainWindow.LoggedInMember, _boat, _imageStream);
                    }

                    if (_boat != null)
                    {
                        ExceptionTextBlock.Foreground = Brushes.MediumSeaGreen;
                        ExceptionTextBlock.Text =
                            $"{_boat.Name} {_boat.Description} {_boat.Level} is aangepast met bootnummer {_boat.Id}";
                       
                    }

                    _mainWindow.MainContent.Navigate(new ManageBoatList(_mainWindow));
                }
                else
                {
                    createdBoat = service.Create(_mainWindow.LoggedInMember, name, description, Int32.Parse(seats),
                        captain, Int32.Parse(level));
                    if (_imageChanged)
                    {
                        service.AddImage(_mainWindow.LoggedInMember, createdBoat, _imageStream);
                    }

                    if (createdBoat != null)
                    {
                        ExceptionTextBlock.Foreground = Brushes.MediumSeaGreen;
                        ExceptionTextBlock.Text =
                            $"{createdBoat.Name} {createdBoat.Description} {createdBoat.Level} is aangemaakt met bootnummer {createdBoat.Id}";
                    }

                    _mainWindow.MainContent.Navigate(new ManageBoatList(_mainWindow));
                }
            }
            catch (IncorrectRightsException ex)
            {
                ExceptionTextBlock.Text = ex.Message;
            }
            catch (System.FormatException)
            {
                ExceptionTextBlock.Text = "Vul alle velden correct in";
            }
            catch (IncorrectLevelException ex)
            {
                ExceptionTextBlock.Text = ex.Message;
            }
            catch (NameEmptyExeception ex)
            {
                ExceptionTextBlock.Text = ex.Message;
            }
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            BoatService service = new BoatService(new BoatRepository());
            service.Delete(_mainWindow.LoggedInMember, _boat);
            _mainWindow.MainContent.Navigate(new ManageBoatList(_mainWindow));
        }

        private void ButtonUpload_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var fileDialog = new OpenFileDialog();
                fileDialog.Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg";
                fileDialog.Multiselect = false;
                fileDialog.ShowDialog(_mainWindow);
                if (fileDialog.CheckFileExists)
                {
                    using (Stream stream = fileDialog.OpenFile())
                    {
                        if (stream != null)
                        {
                            Stream compressedStream = ResizeImage.ResizeTheImage(stream, 500, 500);
                            Image.Source = ImageConverter.Convert(compressedStream);
                            _imageStream = compressedStream;
                            _imageChanged = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // ignored
            }
        }
    }
}