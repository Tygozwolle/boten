namespace RoeiVerenigingLibary
{
    public class Boats
    {
        public int Id { get; set; }
        public int CaptainSeat { get; set; }
        public int Seats { get; set; }
        //public Url Image { get; set; }

        public Boats(int id, int captainSeat, int Seats)
        {
            this.Id = id;
            CaptainSeat = captainSeat;
            this.Seats = Seats;

        }
    }
}
