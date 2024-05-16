namespace RoeiVerenigingLibary
{
    public class Boat
    {
        public int Id { get; set; }
        public Boolean CaptainSeat { get; set; }
        public int Seats { get; set; }

        public int Level { get; set; }
        public string Description { get; set; }

        public Boat(int id, Boolean captainSeat, int Seats, int level, string description)
        {
            this.Id = id;
            CaptainSeat = captainSeat;
            this.Seats = Seats;
            Level = level;
            this.Description = description;
        }
    }
}
