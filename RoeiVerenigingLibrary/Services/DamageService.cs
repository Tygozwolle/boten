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

        public Member MemberWithMostDamage(List<Damage> damageList)
        {
            Dictionary<Member, int> reservationDictionary = new Dictionary<Member, int>();

            Member mostDamageMember = null;
            foreach (Damage damage in damageList)
            {
                if (reservationDictionary.ContainsKey(damage.Member))
                {
                    // Increment the count for the existing boat
                    reservationDictionary[damage.Member]++;
                }
                else
                {
                    // Add the boat to the dictionary with an initial count of 1
                    reservationDictionary[damage.Member] = 1;
                }
            }

            Member mostPopularMember = null;
            int maxCount = 0;

            foreach (var entry in reservationDictionary)
            {
                if (entry.Value > maxCount)
                {
                    mostPopularMember = entry.Key;
                    maxCount = entry.Value;
                }
            }

            return mostPopularMember;
        }

        public int AmountOfOpenDamageReports(List<Damage> damages)
        {
            return damages.Count(damage => damage.BoatFixed);
        }

        public Dictionary<string, int> AllDamageReportsSorted()
        {
            Dictionary<string, int> divDict = new Dictionary<string, int>();
            divDict.Add("Kapot", 0);
            divDict.Add("Bruikbaar", 0);
            divDict.Add("Gemaakt", 0);
            var allReports = GetAll();
            foreach (var damage in allReports)
            {
                if (damage.BoatFixed)
                {
                    divDict["Gemaakt"]++;
                }
                else if (damage.Usable)
                {
                    divDict["Bruikbaar"]++;
                }
                else
                {
                    divDict["Kapot"]++;
                }
            }

            return divDict;
        }
    }
}