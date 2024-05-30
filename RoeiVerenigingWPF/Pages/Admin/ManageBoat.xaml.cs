using System.IO;
using System.Windows;
using System.Windows.Controls;
using DataAccessLibrary;
using Microsoft.Win32;
using RoeiVerenigingLibrary.Exceptions;
using RoeiVerenigingLibrary.Model;
using RoeiVerenigingLibrary.Services;
using RoeiVerenigingWPF.Frames;
using RoeiVerenigingWPF.Helpers;

namespace RoeiVerenigingWPF.Pages.Admin;

public partial class ManageBoat : Page
{
    private Boat _boat;
    private readonly bool _edit;
    private bool _imageChanged;
    private Stream _imageStream;
    private readonly MainWindow _mainWindow;

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
        if (boat.Image != null) Image.Source = ImageConverter.Convert(boat.Image);
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
            Captain.Content = "Stuurman aanwezig";
        else
            Captain.Content = "Stuurman afwezig";
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        var service = new BoatService(new BoatRepository());
        try
        {
            var name = Name.Text;
            var description = Description.Text;
            var seats = Seats.Text;
            var level = Level.Text;
            var captain = Captain.IsPressed;
            Boat createdBoat;
            if (_edit)
            {
                _boat = service.Update(_mainWindow.LoggedInMember, _boat, name, description, int.Parse(seats),
                    captain, int.Parse(level));
                if (_imageChanged) service.UpdateImage(_mainWindow.LoggedInMember, _boat, _imageStream);

                if (_boat != null)
                    MessageBox.Show(
                        $"{_boat.Name} {_boat.Description} {_boat.Level} is aangepast met bootnummer {_boat.Id}");

                _mainWindow.MainContent.Navigate(new ManageBoatList(_mainWindow));
            }
            else
            {
                createdBoat = service.Create(_mainWindow.LoggedInMember, name, description, int.Parse(seats),
                    captain, int.Parse(level));
                if (_imageChanged) service.AddImage(_mainWindow.LoggedInMember, createdBoat, _imageStream);

                if (createdBoat != null)
                    MessageBox.Show(
                        $"{createdBoat.Name} {createdBoat.Description} {createdBoat.Level} is aangemaakt met bootnummer {createdBoat.Id}");

                _mainWindow.MainContent.Navigate(new ManageBoatList(_mainWindow));
            }
        }
        catch (IncorrectRightsException ex)
        {
            MessageBox.Show(ex.Message);
        }
        catch (FormatException)
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
        var service = new BoatService(new BoatRepository());
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
                using (var stream = fileDialog.OpenFile())
                {
                    if (stream != null)
                    {
                        var compressedStream = ResizeImage.ResizeTheImage(stream, 500, 500);
                        Image.Source = ImageConverter.Convert(compressedStream);
                        _imageStream = compressedStream;
                        _imageChanged = true;
                    }
                }
        }
        catch (Exception ex)
        {
            // ignored
        }
    }
}