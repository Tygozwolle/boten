using RoeiVerenigingLibrary.Exceptions;

namespace RoeiVerenigingLibrary
{
    public class BoatService 
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

        public Boat GetBoatById(int id)
        {
            Boat boat;

            boat = _boatRepository.GetBoatById(id);

            return boat;
        }
        public Boat Create(Member LogedInMember, string name, string description, int seats, bool captainSeat, int level)
        {
            if (LogedInMember.Roles.Contains("beheerder")|| LogedInMember.Roles.Contains("materiaal_commissaris"))
            {
                Boat boat;

                boat = _boatRepository.Create(name, description, seats, captainSeat, level);

                return boat;
            }
            else
            {
                throw new IncorrectRightsExeption();
            }
        }
    }
}