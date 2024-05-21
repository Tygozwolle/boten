namespace RoeiVerenigingLibary
{
    public class BoatService : IBoatRepository
    {
        private readonly IBoatRepository _boatRepository;

        public BoatService(IBoatRepository repository)
        {
            _boatRepository = repository;
        }
        public List<Boat>? Getboats()
        {
            List<Boat> boat;

            boat = _boatRepository.Getboats();

            return boat;
        }

        public Boat Getboat(int id)
        {
            Boat boat;

            boat = _boatRepository.Getboat(id);

            return boat;
        }
    }
}
