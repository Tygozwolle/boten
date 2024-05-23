namespace RoeiVerenigingLibary
{
    public interface IBoatRepository
    {
        public List<Boat> Getboats();

        public Boat GetBoatById(int id);
    }
}