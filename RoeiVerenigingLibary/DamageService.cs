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
        public DamageServic(IDamageRepository repository)
        {
            _DamageRepository = repository;
        }
        public Damage CreateReport( Member member, Boat boat, string description)
        {
            _DamageRepository.Create( member, boat, description);
        }
    }
}
