namespace RoeiVerenigingLibary
{
    public interface IBoatRepository
    {
        public List<Boat> Getboats();

        public Boat Getboat(int id);
    }
}
