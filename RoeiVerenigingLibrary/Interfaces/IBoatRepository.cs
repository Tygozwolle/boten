namespace RoeiVerenigingLibrary
{
    public interface IBoatRepository
    {
        public List<Boat> Getboats();

        public Boat GetBoatById(int id);
        public Boat Create(string name, string description, int seats, bool captainSeat, int level);
        public Boat Update(Boat boat, string name, string description, int seats, bool captainSeat, int level);
    }
}