using RoeiVerenigingLibrary.Interfaces;
using RoeiVerenigingLibrary.Model;

namespace RoeiVerenigingLibrary.Services;

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

    public List<Damage> GetRelatedToUser(Member loggedInMember)
    {
        return damageRepository.GetRelatedToUser(loggedInMember);
    }

    public Damage CreateReport(Member member, Boat boat, string description)
    {
        return damageRepository.Create(member, boat, description);
    }

    public void AddFirstImageToClass(List<Damage> damages, IImageRepository imageRepository)
    {
        var tasks = new List<Task>(damages.Count);
        foreach (var damage in damages)
        {
            var task = new Task(() =>
            {
                var Save = damage;
                Save.Images = new List<Stream> { imageRepository.GetFirstImage(Save.Id) };
            });
            task.Start();
            tasks.Add(task);
        }

        Task.WaitAll(tasks.ToArray());
    }
}