namespace RoeiVerenigingLibrary
{
    public class Reservation
    {
        public Reservation(Member member, DateTime creationDate, DateTime startTime, DateTime endTime, Boat boat,
            int id)
        {
            Member = member;
            CreationDate = creationDate;
            StartTime = startTime;
            EndTime = endTime;
            Boat = boat;
            Id = id;
        }

        public Reservation(Member member, int boatId, DateTime startTime, DateTime endTime)
        {
            Member = member;
            StartTime = startTime;
            EndTime = endTime;
            BoatId = boatId;
        }

        public Member Member { get; set; }
        public DateTime CreationDate { get; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int BoatId { get; set; }
        public Boat Boat { get; set; }
        public int Id { get; }
    }
}