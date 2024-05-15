using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoeiVerenigingLibary
{
    public class DamageService
    {
        private IDamageRepository _DamageRepository;
        public DamageService(IDamageRepository repository)
        {
            _DamageRepository = repository;
        }
        public Damage CreateReport( Member member, Boat boat, string description)
        {
          return  _DamageRepository.Create( member, boat, description);
        }
    }
}
