namespace RoeiVerenigingLibary;

public class DamageService(IDamageRepository damageRepository)
{
    public List<Damage> GetAll()
    {
        return damageRepository.GetAllDamageReports();
    }

    public Damage Update(int id, bool boatFixed, bool usable, string description)
    {
        return damageRepository.Update(id, boatFixed, usable, description);
    }

    public Damage GetById(int id)
    {
        return damageRepository.GetById(id);
    }
}