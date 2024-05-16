using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoeiVerenigingLibary
{
    public class Damage
    {
        public int Id { get; set; }
        public Member Member { get; set; }
        public Boat Boat { get; set; }
        public String Description { get; set; }
        public bool BoatFixed { get; set; }
        public bool Usable { get; set; }
        public List<Stream> Images { get; set; }

        public Damage(int id, Member member, Boat boat, string description, bool boatFixed, bool usable)
        {
            this.Id = id;
            this.Member = member;
            this.Boat = boat;
            this.Description = description;
            this.BoatFixed = boatFixed;
            this.Usable = usable;

        }
        
        public Damage(int id, Member member, Boat boat, string description, bool boatFixed, bool usable,
            List<Stream> images)
        {
            this.Images = images;
            this.Id = id;
            this.Member = member;
            this.Boat = boat;
            this.Description = description;
            this.BoatFixed = boatFixed;
            this.Usable = usable;
        }
    }
}