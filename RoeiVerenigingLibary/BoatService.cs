namespace RoeiVerenigingLibary
{
    public class BoatService : IBoatRepository
    {
        private readonly IBoatRepository _boatRepository;

        public BoatService(IBoatRepository repository)
        {
            _boatRepository = repository;
        }
        public List<Boat>? Get()
        {
            List<Boat> boat;

            boat = _boatRepository.Get();

            return boat;
        }
    }
}
