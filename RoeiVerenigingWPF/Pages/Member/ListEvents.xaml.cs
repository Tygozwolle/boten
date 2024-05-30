using RoeiVerenigingLibrary.Model;
using RoeiVerenigingLibrary.Services;
using System.Windows.Controls;

namespace RoeiVerenigingWPF.Pages.Member
{
    /// <summary>
    /// Interaction logic for ListEvents.xaml
    /// </summary>

    public partial class ListEvents : Page
    {
        public List<Event> _events { get; set; }
        private EventService _eventService { get; set; }
        public ListEvents()
        {
            _events = _eventService.GetEvents(true);

            InitializeComponent();
        }
    }
}
