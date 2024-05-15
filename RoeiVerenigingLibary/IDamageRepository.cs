using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoeiVerenigingLibary
{
    public interface IDamageRepository
    {
        public List<Damage> GetAllDamageReports();

        public Damage Update(int id, bool boatFixed, bool usable, string description);

        public Damage GetById(int id);

        public List<Damage> GetRelatedToUser(int memberId);
    }
}
