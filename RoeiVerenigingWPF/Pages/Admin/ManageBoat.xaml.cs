using System.Windows;
using DataAccessLibrary;
using RoeiVerenigingLibrary;
using System.Windows.Controls;
using DataAccessLibrary;
using RoeiVerenigingLibrary;
using RoeiVerenigingLibrary.Exceptions;
using RoeiVerenigingWPF.Frames;

namespace RoeiVerenigingWPF.Pages.Admin
{
    public partial class ManageBoat : Page
    {
        private MainWindow _mainWindow;
        private bool Eddit;
        private Boat boat;
        public ManageBoat(MainWindow mainWindow, Boat boat, bool eddit)
        {
            _mainWindow = mainWindow;
            InitializeComponent();
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
    }
}