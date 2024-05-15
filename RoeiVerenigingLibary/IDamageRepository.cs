using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoeiVerenigingLibary
{
    public interface IDamageRepository
    { 
        public Damage Create(int id, Member member, Boat boat, string description);
    }
}
