namespace RoeiVerenigingLibary
{
    public interface IBoatRepository
    {
        public List<Boat> Get(int id, int captainSeat, int Seats);
    }
}
