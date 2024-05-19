namespace RoeiVerenigingLibary
{
    public class Boat
    {
        public int Id { get; set; }
        public Boolean CaptainSeat { get; set; }
        public int Seats { get; set; }
        public int Level { get; set; }

        public Boat(int id, Boolean captainSeat, int seats, int level)
        {
            Id = id;
            CaptainSeat = captainSeat;
            Seats = seats;
            Level = level;
        }
    }
}
