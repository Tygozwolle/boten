namespace RoeiVerenigingLibrary
{
    public interface IBoatRepository
    {
        public List<Boat> Getboats();

        public Boat GetBoatById(int id);
        public Boat Create(string name, string description, int seats, bool captainSeat, int level);
        public Boat Update(Boat boat, string name, string description, int seats, bool captainSeat, int level);
        public void AddImage(Boat boat, Stream stream);
        public void Delete(Boat boat);
        public void UpdateImage(Boat boat, Stream stream);
        public Stream GetImage(Boat boat);
    }
}