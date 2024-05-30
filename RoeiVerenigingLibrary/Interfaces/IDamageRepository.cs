using RoeiVerenigingLibrary.Model;

namespace RoeiVerenigingLibrary.Interfaces;

public interface IDamageRepository
{
    public List<Damage> GetAllDamageReports();

    public Damage Update(int id, bool boatFixed, bool usable, string description);

    public Damage GetById(int id);

    public List<Damage> GetRelatedToUser(Member member);

    public Damage Create(Member member, Boat boat, string description);
}