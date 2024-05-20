using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoeiVerenigingLibary
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
            return damageRepository.GetRelatedToUser(loggedInMember.Id);
        }

        public Damage CreateReport(Member member, Boat boat, string description)
        {
            return damageRepository.Create(member, boat, description);
        }
    }
}