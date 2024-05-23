namespace RoeiVerenigingLibary;

public interface IDamageRepository
{
    public List<Damage> GetAllDamageReports();

    public Damage Update(int id, bool boatFixed, bool usable, string description);

    public Damage GetById(int id);

    public List<Damage> GetRelatedToUser(int memberId);

    public Damage Create(Member member, Boat boat, string description);
}