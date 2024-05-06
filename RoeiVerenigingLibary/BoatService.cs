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
            try
            {
                boat = _boatRepository.Get();
            }
            catch (Exception)
            {
                throw new Exception();
            }
            return boat;
        }
    }
}
