namespace RoeiVerenigingLibary
{
    public class Boat
    {
        public int Id { get; set; }
        public Boolean CaptainSeat { get; set; }
        public int Seats { get; set; }

        public Boat(int id, Boolean captainSeat, int Seats)
        {
            this.Id = id;
            CaptainSeat = captainSeat;
            this.Seats = Seats;

        }
    }
}
