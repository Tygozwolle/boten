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

namespace RoeiVerenigingWPF.Pages.Admin
{
    public partial class ManageBoat : Page
    {
        private MainWindow _mainWindow;
        private bool Eddit;
        private Boat boat;
        private bool ImageChanged = false;
        private Stream ImageStream;
        public ManageBoat(MainWindow mainWindow, Boat boat, bool eddit)
        {
            _mainWindow = mainWindow;
            InitializeComponent();
            this.boat = boat;
            Name.Text = boat.Name;
            Description.Text = boat.Description;
            Seats.Text = boat.Seats.ToString();
            Level.Text = boat.Level.ToString();
            Captain.IsChecked = boat.CaptainSeat;
            Eddit = eddit;
            if (eddit)
            {
                ButtonEditCreate.Content = "Bewerken";
                HeaderBoat.Content = "Bewerk boot";
                TextBlockBoat.Text = "Bewerk een boot aan, zodat de informatie correct is!";
                if (boat.Image != null) 
                {
                    Image.Source = ImageConverter.Convert(boat.Image);
                }
            }
            else
            {
                ButtonEditCreate.Content = "Aanmaken boot";
            }
        }
        public ManageBoat(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
            InitializeComponent();
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
                    if (boat != null)
                    {
                        MessageBox.Show(
                            $"{boat.Name} {boat.Description} {boat.Level} is aangepast met bootnummer {boat.Id}");
                    }
                }
                else
                {
                    createdBoat = service.Create(_mainWindow.LoggedInMember, name, discription, Int32.Parse(seats), captain, Int32.Parse(level));
                    if (createdBoat != null)
                    {
                        MessageBox.Show(
                            $"{createdBoat.Name} {createdBoat.Description} {createdBoat.Level} is aangemaakt met bootnummer {createdBoat.Id}");
                    }

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

        }
        private void ButtonUpload_Click(object sender, RoutedEventArgs e)
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
                        Image.Source = ImageConverter.Convert(stream);
                        ImageStream = stream;
                        ImageChanged = true;
                    }
                }
            }
        }
    }
}