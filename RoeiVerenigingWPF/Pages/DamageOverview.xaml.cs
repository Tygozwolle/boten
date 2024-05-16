using System.Windows.Controls;
using DataAccessLibary;
using RoeiVerenigingLibary;
using RoeiVerenigingWPF.Frames;

namespace RoeiVerenigingWPF.Pages;

public partial class DamageOverview : Page
{
    public MainWindow MainWindow { set; get; }
    private DamageService _service = new DamageService(new DamageRepository()); 
    public List<Damage> Damages { set; get; }
    
    public DamageOverview(MainWindow mw)
    {
        InitializeComponent();
        DataContext = this;
        MainWindow = mw;
        Damages = _service.GetRelatedToUser(MainWindow.LoggedInMember);
    }

    private void CreateNewDamageReport(object sender, System.Windows.RoutedEventArgs e)
    {
        MainWindow.MainContent.Navigate(new CreateDamageReport(MainWindow));
    }
    private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        if (sender is Button)
        {
            Button casted = sender as Button;
            object command = casted.CommandParameter;
            int id = Int32.Parse(command.ToString());
            //todo send user to view page(the one with the qr code)
             MainWindow.MainContent.Navigate(new ViewDamage(MainWindow, _service.GetById(id)));
        }
    }
}