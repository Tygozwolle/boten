using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoeiVerenigingLibrary.Model
{
    public class Event
    {
        public List<Member> Participants { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public int Id { get; set; }
        public int MaxParticipants { get; set; }
        public List<Boat> Boats { get; set; }
    }
}
