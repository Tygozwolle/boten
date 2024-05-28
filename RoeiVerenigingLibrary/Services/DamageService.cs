namespace RoeiVerenigingLibrary
{
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
            List<Task> tasks = new List<Task>(damages.Count);
            foreach (Damage damage in damages)
            {
                Task task = new Task(() =>
                {
                    Damage save = damage;
                    save.Images = new List<Stream> { imageRepository.GetFirstImage(save.Id) };
                });
                task.Start();
                tasks.Add(task);
            }

            Task.WaitAll(tasks.ToArray());
        }
    }
}