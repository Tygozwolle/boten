namespace RoeiVerenigingLibary
{
    public class BoatService : IBoatRepository
    {
        private readonly IBoatRepository _boatRepository;

        public BoatService(IBoatRepository repository)
        {
            _boatRepository = repository;
        }
        public List<Boat>? Get(int id, int captainSeat, int Seats)
        {
            List<Boat> boat;
            try
            {
                boat = _boatRepository.Get(id, captainSeat, Seats);
            }
            catch (Exception)
            {
                throw new Exception();
            }
            return boat;
        }
    }
}
