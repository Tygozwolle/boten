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
                if (!(level >= 1 && level <= 10))
                {
                    throw new IncorrectLevelException();
                }
                else
                {
                    boat = _boatRepository.Create(name, description, seats, captainSeat, level);
                }
                return boat;
            }
            else
            {
                throw new IncorrectRightsExeption();
            }
        }
        public Boat Update(Member LogedInMember,Boat boat, string name, string description, int seats, bool captainSeat, int level)
        {
            if (LogedInMember.Roles.Contains("beheerder") || LogedInMember.Roles.Contains("materiaal_commissaris"))
            {
                if (!(level >= 1 && level <= 10))
                {
                    throw new IncorrectLevelException();
                }
                else
                {
                    return _boatRepository.Update(boat, name, description, seats, captainSeat, level);
                }
            }
            else
            {
                throw new IncorrectRightsExeption();
            }
        }
        public void AddImage(Member LogedInMember, Boat boat, Stream stream)
        {
            if (LogedInMember.Roles.Contains("beheerder")|| LogedInMember.Roles.Contains("materiaal_commissaris"))
            {
                _boatRepository.AddImage(boat, stream);
            }
            else
            {
                throw new IncorrectRightsExeption();
            }
        }
        public void Delete(Member LogedInMember, Boat boat)
        {
            if (LogedInMember.Roles.Contains("beheerder")|| LogedInMember.Roles.Contains("materiaal_commissaris"))
            {
                _boatRepository.Delete(boat);
            }
            else
            {
                throw new IncorrectRightsExeption();
            }
        }
        public void UpdateImage(Member LogedInMember, Boat boat, Stream stream)
        {
            if (LogedInMember.Roles.Contains("beheerder")|| LogedInMember.Roles.Contains("materiaal_commissaris"))
            {
                if (_boatRepository.GetImage(boat) != null)
                {
                    _boatRepository.UpdateImage(boat, stream);
                }
                else
                {
                    _boatRepository.AddImage(boat, stream);
                }
            }
            else
            {
                throw new IncorrectRightsExeption();
            }
        }
    }
}