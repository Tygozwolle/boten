namespace RoeiVerenigingLibary;

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

    public Boat GetBoatById(int id)
    {
        Boat boat;

        boat = _boatRepository.GetBoatById(id);

        return boat;
    }
}