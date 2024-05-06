using DataAccessLibary;
using RoeiVerenigingLibary;
using System.Windows.Controls;

namespace RoeiVerenigingWPF.Pages
{
    /// <summary>
    /// Interaction logic for ListBoats.xaml
    /// </summary>
    /// 

    public partial class ListBoats : Page
    {
        public required List<Boat> boats { get; set; }
        public ListBoats()
        {

            BoatService service = new BoatService(new BoatRepository());
            this.DataContext = this;

            DataContext = this;
            boats = service.Get();

            InitializeComponent();
        }
    }
}
