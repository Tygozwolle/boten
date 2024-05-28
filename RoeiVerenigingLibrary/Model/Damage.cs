namespace RoeiVerenigingLibrary
{
    public class Damage
    {
        public Damage(int id, Member member, Boat boat, string description, bool boatFixed, bool usable)
        {
            Id = id;
            Member = member;
            Boat = boat;
            Description = description;
            BoatFixed = boatFixed;
            Usable = usable;
        }

        public Damage(int id, Member member, Boat boat, string description, bool boatFixed, bool usable,
            DateTime reportTime)
        {
            Id = id;
            Member = member;
            Boat = boat;
            Description = description;
            BoatFixed = boatFixed;
            Usable = usable;
            ReportTime = reportTime;
        }

        public Damage(int id, Member member, Boat boat, string description, bool boatFixed, bool usable,
            List<Stream> images)
        {
            Images = images;
            Id = id;
            Member = member;
            Boat = boat;
            Description = description;
            BoatFixed = boatFixed;
            Usable = usable;
        }

        public int Id { get; set; }
        public Member Member { get; set; }
        public Boat Boat { get; set; }
        public string Description { get; set; }
        public bool BoatFixed { get; set; }
        public bool Usable { get; set; }
        public DateTime ReportTime { get; set; }
        public List<Stream> Images { get; set; }
    }
}