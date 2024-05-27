using System.Windows;
using DataAccessLibrary;
using RoeiVerenigingLibrary;
using System.Windows.Controls;
using DataAccessLibrary;
using Microsoft.Win32;
using RoeiVerenigingLibrary;
using RoeiVerenigingLibrary.Exceptions;
using RoeiVerenigingWPF.Frames;
using RoeiVerenigingWPF.helpers;
using System.DirectoryServices.ActiveDirectory;
using System.IO;
using System.Windows.Media.Imaging;

namespace RoeiVerenigingWPF.Pages.Admin
{
    public partial class ManageBoat : Page
    {
        private MainWindow _mainWindow;
        private bool Eddit;
        private Boat boat;
        private bool ImageChanged = false;
        private Stream ImageStream;
        public ManageBoat(MainWindow mainWindow, Boat boat)
        {
            _mainWindow = mainWindow;
            InitializeComponent();
            this.boat = boat;
            Name.Text = boat.Name;
            Description.Text = boat.Description;
            Seats.Text = boat.Seats.ToString();
            Level.Text = boat.Level.ToString();
            Delete_Button.Visibility = Visibility.Visible;
            Captain.IsChecked = boat.CaptainSeat;
            Eddit = true;
                ButtonEditCreate.Content = "Bewerken";
                HeaderBoat.Content = "Bewerk boot";
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
            BoatService service = new BoatService(new BoatRepository());
            try
            {
                string name = Name.Text;
                string discription = Description.Text;
                string seats = Seats.Text;
                string level = Level.Text;
                bool captain = Captain.IsPressed;
                Boat createdBoat;
                if (Eddit)
                {
                    boat = service.Update(_mainWindow.LoggedInMember, boat, name, discription, Int32.Parse(seats), captain, Int32.Parse(level));
                    if (ImageChanged)
                    {
                        service.UpdateImage(_mainWindow.LoggedInMember,boat, ImageStream);
                    }
                    if (boat != null)
                    {
                        MessageBox.Show(
                            $"{boat.Name} {boat.Description} {boat.Level} is aangepast met bootnummer {boat.Id}");
                    }
                    _mainWindow.MainContent.Navigate(new ManageBoatList(_mainWindow));
                }
                else
                {
                    createdBoat = service.Create(_mainWindow.LoggedInMember, name, discription, Int32.Parse(seats), captain, Int32.Parse(level));
                    if (ImageChanged)
                    {
                        service.AddImage(_mainWindow.LoggedInMember, createdBoat, ImageStream);
                    }
                    if (createdBoat != null)
                    {
                        MessageBox.Show(
                            $"{createdBoat.Name} {createdBoat.Description} {createdBoat.Level} is aangemaakt met bootnummer {createdBoat.Id}");
                    }
                    _mainWindow.MainContent.Navigate(new ManageBoatList(_mainWindow));
                }
            }
            catch (IncorrectRightsExeption ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (System.FormatException)
            {
                MessageBox.Show("Vul alle velden correct in");
            }
            catch (IncorrectLevelException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (NameEmptyExeception ex)
            {
                MessageBox.Show(ex.Message);
            }

            }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            BoatService service = new BoatService(new BoatRepository());
            service.Delete(_mainWindow.LoggedInMember, boat);
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
                            Stream compressedStream = RisizeImage.ResizeImage(stream, 500, 500);
                            Image.Source = ImageConverter.Convert(compressedStream);
                            ImageStream = compressedStream;
                            ImageChanged = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                
            }
        }
    }
}